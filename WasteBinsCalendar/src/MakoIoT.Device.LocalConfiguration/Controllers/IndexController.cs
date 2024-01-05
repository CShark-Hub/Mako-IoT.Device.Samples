using System;
using System.IO;
using System.Text;
using MakoIoT.Device.LocalConfiguration.Extensions;
using MakoIoT.Device.Services.Configuration;
using MakoIoT.Device.Services.FileStorage.Interface;
using MakoIoT.Device.Services.Interface;
using MakoIoT.Device.Services.Server.WebServer;
using MakoIoT.Device.Services.WiFi.Configuration;

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

                    return reader.ReadLine();
                });

                _configService.UpdateConfigSection(WiFiConfig.SectionName, new WiFiConfig
                {
                    Ssid = (string)Form["ssid"],
                    Password = (string)Form["password"],
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

            return line;
        }


        private static string GetMessage(string type, string text)
        {
            var html = new StringBuilder(HtmlResources.GetString(HtmlResources.StringResources.message));
            return html.Replace("{class}", type).Replace("{text}", text).ToString();
        }
    }
}
