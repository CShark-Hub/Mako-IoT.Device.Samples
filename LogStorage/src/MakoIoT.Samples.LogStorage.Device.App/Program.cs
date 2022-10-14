using System.Collections;
using System.Threading;
using MakoIoT.Device;
using MakoIoT.Device.Services.Configuration.Extensions;
using MakoIoT.Device.Services.FileStorage.Extensions;
using MakoIoT.Device.Services.Logging.Configuration;
using MakoIoT.Device.Services.Logging.Storage.Configuration;
using MakoIoT.Device.Services.Logging.Storage.Events;
using MakoIoT.Device.Services.Logging.Storage.Extensions;
using MakoIoT.Device.Services.Logging.Storage.Services;
using MakoIoT.Device.Services.Logging.Storage.Tasks;
using MakoIoT.Device.Services.Mediator.Extensions;
using MakoIoT.Device.Services.Scheduler.Configuration;
using MakoIoT.Device.Services.Scheduler.Extensions;
using MakoIoT.Device.Services.WiFi.Configuration;
using MakoIoT.Device.Services.WiFi.Extensions;
using MakoIoT.Samples.LogStorage.Device.Tasks;
using Microsoft.Extensions.Logging;

namespace MakoIoT.Samples.LogStorage.Device.App
{
    public class Program
    {
        public static void Main()
        {
            DeviceBuilder.Create()
                .ConfigureDI(() =>
                {

                })
                .AddLoggingWithStorage(options =>
                {
                    options.LoggerConfig = new LoggerConfig(LogLevel.Debug, new Hashtable
                    {
                        { "DI", LogLevel.Debug },
                    });
                    options.MemoryLogStorageMaxCapacity = 10;
                    options.MaxLogFiles = 10;
                    options.DeleteLogFilesOnStartup = true;
                })
                .AddMediator(o =>
                {
                    o.AddSubscriber(typeof(MemoryLogStorageMaxCapacityReached), typeof(ILogHandlingService));
                })
                .AddFileStorage()
                .AddWiFi()
                .AddConfiguration(cfg =>
                {
                    cfg.WriteDefault(WiFiConfig.SectionName, Config.WiFiConfig, true);

                    cfg.WriteDefault(LogStorageConfig.SectionName, Config.LogStorageConfig, true);

                    cfg.WriteDefault(SchedulerConfig.SectionName, new SchedulerConfig
                    {
                        Tasks = new[]
                        {
                            new SchedulerTaskConfig { TaskId = nameof(LogHelloWorldTask), IntervalMs = 10000 },
                            new SchedulerTaskConfig { TaskId = nameof(FlushLogsTask), IntervalMs = 60000 }
                        }
                    }, true);
                })
                .AddScheduler(o =>
                {
                    o.AddTask(typeof(LogHelloWorldTask), nameof(LogHelloWorldTask));
                    o.AddTask(typeof(FlushLogsTask), nameof(FlushLogsTask));
                })
                .Build()
                .Start();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
