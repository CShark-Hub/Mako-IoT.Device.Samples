using System.Diagnostics;
using MakoIoT.Device.Services.Mediator;
using MakoIoT.Samples.Mediator.Device.Events;

namespace MakoIoT.Samples.Mediator.Device
{
    public class Service1 : IService1, IEventHandler
    {
        private readonly IMediator _mediator;

        public Service1(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void DoSomething()
        {
            _mediator.Publish(new Event2 { Text = "Hello from Service1 !" });
        }

        public void Handle(IEvent @event)
        {
            if (@event is Event1 event1)
            {
                Debug.WriteLine($"[{nameof(Service1)}] Event1 received. The data is: {event1.Data}");
            }
        }
    }
}
