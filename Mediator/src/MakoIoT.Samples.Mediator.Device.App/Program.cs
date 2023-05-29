using System.Threading;
using MakoIoT.Device;
using MakoIoT.Device.Services.Mediator;
using MakoIoT.Device.Services.Mediator.Extensions;
using MakoIoT.Samples.Mediator.Device.Events;
using nanoFramework.DependencyInjection;

namespace MakoIoT.Samples.Mediator.Device.App
{
    public class Program
    {
        public static void Main()
        {
            var device = DeviceBuilder.Create()
                .ConfigureDI(services => 
                {
                    services.AddSingleton(typeof(IService1), typeof(Service1));
                    services.AddSingleton(typeof(Service2), typeof(Service2));
                })
                .AddMediator(options =>
                {
                    options.AddSubscriber(typeof(Event1), typeof(IService1));
                    options.AddSubscriber(typeof(Event1), typeof(Service2));
                    options.AddSubscriber(typeof(Event2), typeof(Service2));
                })
                .Build();

            device.Start();

            var mediator = (IMediator)device.ServiceProvider.GetService(typeof(IMediator));

            mediator.Publish(new Event1 { Data = "Hello from Event1!" });
            mediator.Publish(new Event2 { Text = "Hello from Event2!" });
            
            var service1 = (IService1)device.ServiceProvider.GetService(typeof(IService1));
            service1.DoSomething();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
