using System;
using System.Threading;
using MakoIoT.Device;
using MakoIoT.Device.Services.Configuration.Extensions;
using MakoIoT.Device.Services.FileStorage.Extensions;
using MakoIoT.Device.Services.Logging.Extensions;
using MakoIoT.Device.Services.Scheduler.Configuration;
using MakoIoT.Device.Services.Scheduler.Extensions;
using MakoIoT.Samples.Blink.Device.App.HardwareServices;
using MakoIoT.Samples.Blink.Device.Interface;
using MakoIoT.Samples.Blink.Device.Tasks;
using nanoFramework.DependencyInjection;

namespace MakoIoT.Samples.Blink.Device.App
{
    public class Program
    {
        public static void Main()
        {
            DeviceBuilder.Create()
                .ConfigureDI(services =>
                {
                    services.AddSingleton(typeof(IBlinker), typeof(LedBlinker));
                })
                .AddLogging()
                .AddScheduler(o =>
                {
                    o.AddTask(typeof(BlinkTask), nameof(BlinkTask));
                })
                .AddFileStorage()
                .AddConfiguration(c =>
                {
                    c.WriteDefault(SchedulerConfig.SectionName, new SchedulerConfig
                    {
                        Tasks = new[]
                        {
                            new SchedulerTaskConfig { TaskId = nameof(BlinkTask), IntervalMs = 500 }
                        }
                    });
                })
                .Build()
                .Start();

            Thread.Sleep(Timeout.InfiniteTimeSpan);
        }
    }
}
