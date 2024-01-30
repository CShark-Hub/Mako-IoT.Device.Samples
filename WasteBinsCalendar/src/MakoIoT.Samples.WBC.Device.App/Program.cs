using System.Reflection;
using System.Threading;
using MakoIoT.Device;
using MakoIoT.Device.Displays.Led;
using MakoIoT.Device.LocalConfiguration.Extensions;
using MakoIoT.Device.SecureClient.Services;
using MakoIoT.Device.Services.Configuration.Extensions;
using MakoIoT.Device.Services.ConfigurationManager;
using MakoIoT.Device.Services.ConfigurationManager.Behaviors;
using MakoIoT.Device.Services.ConfigurationManager.Events;
using MakoIoT.Device.Services.ConfigurationManager.Extensions;
using MakoIoT.Device.Services.ConfigurationManager.Interface;
using MakoIoT.Device.Services.ConfigurationManager.Services;
using MakoIoT.Device.Services.FileStorage;
using MakoIoT.Device.Services.FileStorage.Extensions;
using MakoIoT.Device.Services.FileStorage.Interface;
using MakoIoT.Device.Services.Interface;
using MakoIoT.Device.Services.Logging.Configuration;
using MakoIoT.Device.Services.Logging.Extensions;
using MakoIoT.Device.Services.Mediator.Extensions;
using MakoIoT.Device.Services.Scheduler.Configuration;
using MakoIoT.Device.Services.Scheduler.Extensions;
using MakoIoT.Device.Services.Server;
using MakoIoT.Device.Services.Server.Extensions;
using MakoIoT.Device.Services.Server.Services;
using MakoIoT.Device.Services.Server.WebServer;
using MakoIoT.Device.Services.WiFi.AP;
using MakoIoT.Device.Services.WiFi.AP.Configuration;
using MakoIoT.Device.Services.WiFi.AP.Extensions;
using MakoIoT.Device.Services.WiFi.Configuration;
using MakoIoT.Device.Services.WiFi.Extensions;
using MakoIoT.Samples.WBC.Device.App.HardwareServices;
using MakoIoT.Samples.WBC.Device.Configuration;
using MakoIoT.Samples.WBC.Device.Services;
using MakoIoT.Samples.WBC.Device.Tasks;
using Microsoft.Extensions.DependencyInjection;

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

                    services.AddTransient(typeof(IDeviceControl), typeof(DeviceControlService));

                    services.AddTransient(typeof(IClientProvider), typeof(ClientProvider));

                    services.AddSingleton(typeof(ConfigButton));
                    services.AddTransient(typeof(IStreamStorageService), typeof(FileStorageService));

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
                    });

                    cfg.WriteDefault(WiFiConfig.SectionName, new WiFiConfig());

                    cfg.WriteDefault(WiFiAPConfig.SectionName, new WiFiAPConfig
                    {
                        Ssid = "WBC device",
                        Password = "makoiotdevice",
                    });

                    cfg.WriteDefault(WasteBinsCalendarConfig.SectionName, new WasteBinsCalendarConfig
                    {
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
                     o.AddConfigurationWebsite();
                 })

                .Build();

                device.Start();

            //initialize hardware buttons
            device.ServiceProvider.GetService(typeof(ConfigButton));

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
