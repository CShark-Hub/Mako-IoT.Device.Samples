using MakoIoT.Device.Services.Configuration;
using MakoIoT.Device.Services.Interface;
using MakoIoT.Device.Services.Server.WebServer;
using MakoIoT.Device.Services.WiFi.Configuration;
using MakoIoT.Samples.WBC.Device.Configuration;
using System;
using System.Collections;
using System.Net;
using MakoIoT.Device.LocalConfiguration.Model;
using MakoIoT.Device.LocalConfiguration.Extensions;
using nanoFramework.Json;
using System.Text;
using MakoIoT.Device.Utilities.TimeZones;

namespace MakoIoT.Device.LocalConfiguration.Controllers.Api
{
    public class ConfigController
    {
        private readonly IConfigurationService _configService;
        private readonly ILog _logger;

        public ConfigController(ILog logger, IConfigurationService configService)
        {
            _logger = logger;
            _configService = configService;
        }

        [Route("api/config")]
        [Method("GET")]
        public void Get(WebServerEventArgs e)
        {
            try
            {
                var config = new configurationSettings();

                try
                {
                    var wifiConfig = (WiFiConfig)_configService.GetConfigSection(WiFiConfig.SectionName, typeof(WiFiConfig));
                    config.wifiSettings = new wifiSettings()
                    {
                        ssid = wifiConfig.Ssid,
                        password = wifiConfig.Password,
                    };
                }
                catch (ConfigurationException)
                {
                }
                catch (Exception exception)
                {
                    _logger.Error("Error loading wifi configuration", exception);
                }

                try
                {
                    var wbcConfig = (WasteBinsCalendarConfig)_configService.GetConfigSection(WasteBinsCalendarConfig.SectionName, typeof(WasteBinsCalendarConfig));
                    config.calendarSettings = new calendarSettings()
                    {
                        url = wbcConfig.CalendarUrl,
                        timeZone = wbcConfig.TimezoneString
                    };

                    if (wbcConfig.BinsNames != null)
                    {
                        var bins = wbcConfig.BinsNames.Reverse();
                        config.binNames = new binNames()
                        {
                            white = (string)bins.GetValueOrDefault("Black", ""),
                            brown = (string)bins.GetValueOrDefault("Brown", ""),
                            yellow = (string)bins.GetValueOrDefault("Yellow", ""),
                            green = (string)bins.GetValueOrDefault("Green", ""),
                            blue = (string)bins.GetValueOrDefault("Blue", ""),
                            red = (string)bins.GetValueOrDefault("Red", ""),
                        };
                    }
                    else
                    {
                        config.binNames = new binNames();
                    }
                }
                catch (ConfigurationException)
                {
                }
                catch (Exception exception)
                {
                    _logger.Error("Error loading wifi configuration", exception);
                }

                var configString = JsonConvert.SerializeObject(config);

                MakoWebServer.OutPutStream(e.Context.Response, configString);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                MakoWebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.InternalServerError);
                return;
            }
        }

        [Route("api/config")]
        [Method("POST")]
        public void Post(WebServerEventArgs e)
        {
            try
            {
                byte[] buffer = new byte[e.Context.Request.ContentLength64];
                e.Context.Request.InputStream.Read(buffer, 0, buffer.Length);
                string configString = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                _logger.Trace(configString);

                var config = (configurationSettings)JsonConvert.DeserializeObject(configString, typeof(configurationSettings));

                if (!ValidateTimeZone(config.calendarSettings.timeZone))
                {
                    MakoWebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.BadRequest);
                    return;
                }

                _configService.UpdateConfigSection(WiFiConfig.SectionName, new WiFiConfig
                {
                    Ssid = config.wifiSettings.ssid,
                    Password = config.wifiSettings.password,
                });

                _configService.UpdateConfigSection(WasteBinsCalendarConfig.SectionName, new WasteBinsCalendarConfig
                {
                    CalendarUrl = config.calendarSettings.url,
                    TimezoneString = config.calendarSettings.timeZone,
                    BinsNames = new Hashtable(6)
                    {
                        { "Black", config.binNames.white },
                        { "Brown", config.binNames.brown },
                        { "Yellow", config.binNames.yellow },
                        { "Green", config.binNames.green },
                        { "Blue", config.binNames.blue },
                        { "Red", config.binNames.red }
                    }.Reverse()
                });
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                MakoWebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.InternalServerError);
                return;
            }

            MakoWebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.OK);
        }

        private bool ValidateTimeZone(string timeZone)
        {
            if (string.IsNullOrEmpty(timeZone))
                return true;

            try
            {
                TimeZoneConverter.FromPosixString(timeZone.Split(';')[0]);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("Time zone parsing error", ex);
                return false;
            }
        }
    }
}
