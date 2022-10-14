using System.Collections;
using System.Threading;
using MakoIoT.Device;
using MakoIoT.Device.Services.Configuration.Extensions;
using MakoIoT.Device.Services.Configuration.Metadata.Extensions;
using MakoIoT.Device.Services.ConfigurationApi.Extensions;
using MakoIoT.Device.Services.ConfigurationManager;
using MakoIoT.Device.Services.ConfigurationManager.Events;
using MakoIoT.Device.Services.ConfigurationManager.Extensions;
using MakoIoT.Device.Services.ConfigurationManager.Interface;
using MakoIoT.Device.Services.DependencyInjection;
using MakoIoT.Device.Services.FileStorage.Extensions;
using MakoIoT.Device.Services.Logging.Configuration;
using MakoIoT.Device.Services.Logging.Extensions;
using MakoIoT.Device.Services.Mediator.Extensions;
using MakoIoT.Device.Services.Server.Extensions;
using MakoIoT.Device.Services.Server.Services;
using MakoIoT.Device.Services.Server.WebServer;
using MakoIoT.Device.Services.WiFi.AP.Configuration;
using MakoIoT.Device.Services.WiFi.AP.Extensions;
using MakoIoT.Device.Services.WiFi.Configuration;
using MakoIoT.Device.Services.WiFi.Extensions;
using MakoIoT.Samples.Api.Device.App.HardwareServices;
using Microsoft.Extensions.Logging;

namespace MakoIoT.Samples.Api.Device.App
{
    public class Program
    {
        public static void Main()
        {
            DeviceBuilder.Create()
                .ConfigureDI(() =>
                {
                    DI.RegisterSingleton(typeof(IDeviceControl), typeof(DeviceControlService));
                })
                .AddMediator(o =>
                {
                    o.AddSubscriber(typeof(ConfigModeToggleEvent), typeof(IConfigManager));
                    o.AddSubscriber(typeof(ResetToDefaultsEvent), typeof(IConfigManager));
                })
                .AddLogging(new LoggerConfig(LogLevel.Debug, new Hashtable
                {
                    { nameof(DI), LogLevel.Information },
                    { nameof(Server), LogLevel.Trace },
                }))
                .AddFileStorage()
                .AddWiFi()
                .AddWiFiInterfaceManager()
                .AddConfigurationManager()
                .AddWebServer(o =>
                {
                    o.Port = 80;
                    o.Protocol = HttpProtocol.Http;
                    o.AddConfigurationApi();
                })
                .AddConfiguration(cfg =>
                {
                    cfg.WriteDefault(WiFiConfig.SectionName, new WiFiConfig());
                    cfg.WriteDefault(WiFiAPConfig.SectionName, new WiFiAPConfig
                    {
                        Ssid = "MAKO-IoT Device",
                        Password = "CSHARK4Makers",
                    });
                })
                .AddConfigurationMetadata(o =>
                {
                    o.AddDeviceMetadata(@$"{{""Name"":""MAKO-IoT Development Kit"",""Manufacturer"":""CSHARK"",""SerialNo"":""000001"",""ConfigSections"":
[{{""Name"":""{WiFiConfig.SectionName}"",""Label"":""Wi-Fi""}},
{{""Name"":""{WiFiAPConfig.SectionName}"",""Label"":""Wi-Fi Access Point"",""IsHidden"":true}}],""HideOtherSections"":true}}");
                    o.AddMetadata(WiFiConfig.SectionName, MakoIoT.Device.Services.WiFi.Configuration.Metadata.WiFiConfig);
                    o.AddMetadata(WiFiAPConfig.SectionName, MakoIoT.Device.Services.WiFi.AP.Configuration.Metadata.WiFiAPConfig);
                })
                .Build()
                .Start();

            //initialize hardware buttons
            var button = (ConfigButton)DI.BuildUp(typeof(ConfigButton));
            
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
