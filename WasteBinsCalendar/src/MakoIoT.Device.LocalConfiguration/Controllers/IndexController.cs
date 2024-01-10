using System;
using System.Collections;
using System.IO;
using System.Text;
using MakoIoT.Device.LocalConfiguration.Extensions;
using MakoIoT.Device.SecureClient;
using MakoIoT.Device.Services.Configuration;
using MakoIoT.Device.Services.FileStorage.Interface;
using MakoIoT.Device.Services.Interface;
using MakoIoT.Device.Services.Server.WebServer;
using MakoIoT.Device.Services.WiFi.Configuration;
using MakoIoT.Samples.WBC.Device.Configuration;

namespace MakoIoT.Device.LocalConfiguration.Controllers
{
    public class IndexController : ControllerBase
    {
        private readonly ILog _logger;
        private readonly IConfigurationService _configService;
        private readonly IStreamStorageService _storageService;

        public IndexController(ILog logger, IConfigurationService configService, IStreamStorageService storageService) 
            : base(HtmlResources.GetString(HtmlResources.StringResources.index))
        {
            _logger = logger;
            _configService = configService;
            _storageService = storageService;
        }

        [Route("")]
        [Route("index.html")]
        [Method("GET")]
        public void Get(WebServerEventArgs e)
        {
            try
            {
                var wifiConfig = (WiFiConfig)_configService.GetConfigSection(WiFiConfig.SectionName, typeof(WiFiConfig));
                HtmlParams.AddOrUpdate("ssid", wifiConfig.Ssid);
                HtmlParams.AddOrUpdate("password", wifiConfig.Password);
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
                HtmlParams.AddOrUpdate("calendarUrl", wbcConfig.CalendarUrl);
                HtmlParams.AddOrUpdate("timeZone", wbcConfig.Timezone);
                if (wbcConfig.BinsNames != null)
                {
                    var bins = wbcConfig.BinsNames.Reverse();
                    HtmlParams.AddOrUpdate("binBlack", bins.GetValueOrDefault("Black", ""));
                    HtmlParams.AddOrUpdate("binBrown", bins.GetValueOrDefault("Brown", ""));
                    HtmlParams.AddOrUpdate("binYellow", bins.GetValueOrDefault("Yellow", ""));
                    HtmlParams.AddOrUpdate("binGreen", bins.GetValueOrDefault("Green", ""));
                    HtmlParams.AddOrUpdate("binBlue", bins.GetValueOrDefault("Blue", ""));
                    HtmlParams.AddOrUpdate("binRed", bins.GetValueOrDefault("Red", ""));
                }
            }
            catch (ConfigurationException)
            {
            }
            catch (Exception exception)
            {
                _logger.Error("Error loading wifi configuration", exception);
            }


            Render(e.Context.Response, false);
        }

        [Route("")]
        [Route("index.html")]
        [Method("POST")]
        public void Post(WebServerEventArgs e)
        {
            try
            {
                ParseForm(e.Context.Request, (fieldName, fileName, reader, boundary) =>
                {
                    if (fieldName == "httpsCertFile")
                        return SaveFile(reader, boundary, Constants.CertificateFile);
                    return reader.ReadLine();
                });

                _configService.UpdateConfigSection(WiFiConfig.SectionName, new WiFiConfig
                {
                    Ssid = (string)Form["ssid"],
                    Password = (string)Form["password"],
                });

                _configService.UpdateConfigSection(WasteBinsCalendarConfig.SectionName, new WasteBinsCalendarConfig
                {
                    CalendarUrl = (string)Form["calendarUrl"],
                    Timezone = (string)Form["timeZone"],
                    BinsNames = new Hashtable(6)
                    {
                        { "Black", (string)Form["binBlack"] },
                        { "Brown", (string)Form["binBrown"] },
                        { "Yellow", (string)Form["binYellow"] },
                        { "Green", (string)Form["binGreen"] },
                        { "Blue", (string)Form["binBlue"] },
                        { "Red", (string)Form["binRed"] }
                    }.Reverse()
                });
                
                HtmlParams.AddOrUpdate("messages", GetMessage("success", "Configuration updated"));
            }
            catch (Exception exception)
            {
                _logger.Error("Error updating configuration", exception);
                HtmlParams.AddOrUpdate("messages", GetMessage("danger", "Error updating configuration"));
            }

            Render(e.Context.Response, true);
        }

        private string SaveFile(StreamReader reader, string boundary, string fileName)
        {
            using var writer = _storageService.WriteToFileStream(fileName);
            string line = reader.ReadLine();
            while (line != null && !line.StartsWith(boundary))
            {
                writer.WriteLine(line);
                line = reader.ReadLine();
            }
            writer.Close();
            _logger.Trace($"File {fileName} saved");

            return line;
        }


        private static string GetMessage(string type, string text)
        {
            var html = new StringBuilder(HtmlResources.GetString(HtmlResources.StringResources.message));
            return html.Replace("{class}", type).Replace("{text}", text).ToString();
        }
    }
}
