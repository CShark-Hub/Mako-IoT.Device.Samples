using System.Threading;
using MakoIoT.Device;
using MakoIoT.Device.Displays.Led;
using MakoIoT.Device.LocalConfiguration.Extensions;
using MakoIoT.Device.SecureClient.Services;
using MakoIoT.Device.Services.Configuration.Extensions;
using MakoIoT.Device.Services.ConfigurationManager;
using MakoIoT.Device.Services.ConfigurationManager.Events;
using MakoIoT.Device.Services.ConfigurationManager.Extensions;
using MakoIoT.Device.Services.ConfigurationManager.Interface;
using MakoIoT.Device.Services.FileStorage.Extensions;
using MakoIoT.Device.Services.Interface;
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
using MakoIoT.Samples.WBC.Device.App.HardwareServices;
using MakoIoT.Samples.WBC.Device.Configuration;
using MakoIoT.Samples.WBC.Device.Services;
using MakoIoT.Samples.WBC.Device.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

                    services.AddTransient(typeof(IClientProvider), typeof(ClientProvider));

                    services.AddSingleton(typeof(ConfigButton));
                })
#if DEBUG
                .AddLogging(new LoggerConfig(LogEventLevel.Trace))
#else
                .AddLogging(new LoggerConfig(LogEventLevel.Information))
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
                    }, true);

                    cfg.WriteDefault(WiFiConfig.SectionName, new WiFiConfig
                    {
                        ConnectionTimeout = 5
                    }, true);

                    cfg.WriteDefault(WiFiAPConfig.SectionName, new WiFiAPConfig
                    {
                        Ssid = "Waste Bins Calendar",
                        Password = "makoiot",
                    }, true);

                    cfg.WriteDefault(WasteBinsCalendarConfig.SectionName, new WasteBinsCalendarConfig
                    {
                        CalendarUrl = "",
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
                    }, true);
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
                     o.AddConfigurationWebsite();
                 })

                .Build();

                device.Start();

            //initialize hardware buttons
            var button = (ConfigButton)device.ServiceProvider.GetService(typeof(ConfigButton));

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
