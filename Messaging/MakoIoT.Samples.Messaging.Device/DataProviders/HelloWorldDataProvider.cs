using MakoIoT.Device.Services.Interface;
using MakoIoT.Samples.Messaging.Shared.Messages;

namespace MakoIoT.Samples.Messaging.Device.DataProviders
{
    public class HelloWorldDataProvider :IDataProvider
    {
        public HelloWorldDataProvider()
        {
            Id = nameof(HelloWorldDataProvider);
        }

        public string Id { get; }
        public void GetData()
        {
            DataReceived?.Invoke(this, new MessageEventArgs(new HelloWorld { Text = $"Hello from {Id}!" }));
        }

        public event MessageEventHandler DataReceived;
    }
}
