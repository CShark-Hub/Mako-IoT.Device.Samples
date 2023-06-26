using System.Collections;
using System.Threading;
using MakoIoT.Device;
using MakoIoT.Device.Displays.Led;
using MakoIoT.Device.Services.Configuration.Extensions;
using MakoIoT.Device.Services.Configuration.Metadata.Extensions;
using MakoIoT.Device.Services.ConfigurationApi.Extensions;
using MakoIoT.Device.Services.ConfigurationManager;
using MakoIoT.Device.Services.ConfigurationManager.Events;
using MakoIoT.Device.Services.ConfigurationManager.Extensions;
using MakoIoT.Device.Services.ConfigurationManager.Interface;
using MakoIoT.Device.Services.FileStorage.Extensions;
using MakoIoT.Device.Services.Logging.Configuration;
using MakoIoT.Device.Services.Logging.Extensions;
using MakoIoT.Device.Services.Mediator.Extensions;
using MakoIoT.Device.Services.Scheduler.Configuration;
using MakoIoT.Device.Services.Scheduler.Extensions;
using MakoIoT.Device.Services.Server.Extensions;
using MakoIoT.Device.Services.Server.WebServer;
using MakoIoT.Device.Services.WiFi.AP.Configuration;
using MakoIoT.Device.Services.WiFi.AP.Extensions;
using MakoIoT.Device.Services.WiFi.Configuration;
using MakoIoT.Device.Services.WiFi.Extensions;
using MakoIoT.Samples.WBC.Device.Configuration;
using MakoIoT.Samples.WBC.Device.Esp32.HardwareServices;
using MakoIoT.Samples.WBC.Device.Services;
using MakoIoT.Samples.WBC.Device.Tasks;
using Microsoft.Extensions.Logging;
using nanoFramework.DependencyInjection;

namespace MakoIoT.Samples.WBC.Device.App
{
    public class Program
    {
        public static void Main()
        {
            var device = DeviceBuilder.Create()
                .ConfigureDI(services => 
                {
                    services.AddSingleton(typeof(IBinsScheduleService), typeof(BinsScheduleService));
                    services.AddSingleton(typeof(IDisplayController), typeof(DisplayController));
                    services.AddSingleton(typeof(IDateTimeProvider), typeof(DateTimeProvider));

                    //Set R,G,B pins here. Fine tune LED color with calibration factors.
                    //Values going to RGB pins are multiplied by corresponding factor.
                    services.AddSingleton(typeof(IPixelDriver), new PwmPixelDriver(27, 26, 25, true));

                    services.AddSingleton(typeof(IDeviceControl), typeof(DeviceControlService));
                })
#if DEBUG
                .AddLogging(new LoggerConfig(LogLevel.Debug, 
                    new Hashtable
                    {
                        {"DI", LogLevel.Information}
                    }))
#else
                .AddLogging(new LoggerConfig(LogLevel.Information))
#endif
                .AddWiFi()
                .AddConfiguration(cfg =>
                {
                    cfg.WriteDefault(SchedulerConfig.SectionName, new SchedulerConfig
                    {
                        Tasks = new[]
                        {
                            new SchedulerTaskConfig { TaskId = nameof(ShowBinsScheduleTask), IntervalMs = 30000 }
                        }
                    });

                    cfg.WriteDefault(WiFiConfig.SectionName, new WiFiConfig());

                    cfg.WriteDefault(WiFiAPConfig.SectionName, new WiFiAPConfig
                    {
                        Ssid = "MAKO-IoT Device",
                        Password = "CSHARK4Makers",
                    });

                    cfg.WriteDefault(WasteBinsCalendarConfig.SectionName, new WasteBinsCalendarConfig
                    {
                        Timezone = "CET-1CEST,M3.5.0,M10.5.0/3",
                        BinsNames = new()
                        {
                            { "zmieszane", "Black" },
                            { "BIO", "Brown" },
                            { "tworzywa", "Yellow" },
                            { "szkło", "Green" },
                            { "papier", "Blue" },
                            { "SZOP", "Red" }
                        },
                    });
                })
                .AddFileStorage()
                .AddScheduler(options =>
                {
                    options.AddTask(typeof(ShowBinsScheduleTask), nameof(ShowBinsScheduleTask));
                })
                 .AddMediator(o =>
                 {
                     o.AddSubscriber(typeof(ConfigModeToggleEvent), typeof(IConfigManager));
                     o.AddSubscriber(typeof(ResetToDefaultsEvent), typeof(IConfigManager));
                 })
                 .AddWiFiInterfaceManager()
                 .AddConfigurationManager()
                 .AddWebServer(o =>
                 {
                     o.Port = 80;
                     o.Protocol = HttpProtocol.Http;
                      o.AddConfigurationApi();
                 })
//                 .AddConfigurationMetadata(o =>
//                 {
//                     o.AddDeviceMetadata(@$"{{""Name"":""Kubełek 1.0"",""Manufacturer"":""CSHARK"",""SerialNo"":""000001"",""ConfigSections"":
// [{{""Name"":""{WiFiConfig.SectionName}"",""Label"":""Wi-Fi""}},
// {{""Name"":""{WasteBinsCalendarConfig.SectionName}"",""Label"":""Waste Collection Calendar""}},
// {{""Name"":""{WiFiAPConfig.SectionName}"",""Label"":""Wi-Fi Access Point"",""IsHidden"":true}}],""HideOtherSections"":true}}");
//                     o.AddMetadata(WiFiConfig.SectionName, MakoIoT.Device.Services.WiFi.Configuration.Metadata.WiFiConfig);
//                     o.AddMetadata(WiFiAPConfig.SectionName, MakoIoT.Device.Services.WiFi.AP.Configuration.Metadata.WiFiAPConfig);
//                     o.AddMetadata(WasteBinsCalendarConfig.SectionName, @"{""Name"":""WasteBinsCalendar"",""Label"":""Waste Collection Calendar"",""IsHidden"":false,""Parameters"":[{""Name"":""CalendarUrl"",""Type"":""string"",""Label"":""Calendar URL (iCal)"",""IsHidden"":false,""IsSecret"":false,""DefaultValue"":null},{""Name"":""ServiceCertificate"",""Type"":""text"",""Label"":""HTTPS Certificate (required if URL is https://)"",""IsHidden"":false,""IsSecret"":false,""DefaultValue"":null},{""Name"":""Timezone"",""Type"":""timezone"",""Label"":""Time zone"",""IsHidden"":false,""IsSecret"":false,""DefaultValue"":null},{""Name"":""BinsNames"",""Type"":""text"",""Label"":""Calendar event text to bin colour mapping"",""IsHidden"":false,""IsSecret"":false,""DefaultValue"":null}]}");
//                 })
                .Build();

                device.Start();

            //initialize hardware buttons
            var button = (ConfigButton)device.ServiceProvider.GetService(typeof(ConfigButton));

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
