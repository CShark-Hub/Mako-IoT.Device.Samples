using System.Diagnostics;
using MakoIoT.Device.Services.Mediator;
using MakoIoT.Samples.Mediator.Device.Events;

namespace MakoIoT.Samples.Mediator.Device
{
    public class Service2 : IEventHandler
    {
        public void Handle(IEvent @event)
        {
            switch (@event)
            {
                case Event1 event1:
                    Debug.WriteLine($"[{nameof(Service2)}] Event1 received. The data is: {event1.Data}");
                    break;
                case Event2 event2:
                    Debug.WriteLine($"[{nameof(Service2)}] Event2 received The text is: {event2.Text}");
                    break;
            }
        }
    }
}
