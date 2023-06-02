using MakoIoT.Core.Services.Interface;
using MakoIoT.Samples.Messaging.Shared.Messages;

namespace MakoIoT.Samples.Messaging.Console
{
    public class HelloWorldConsumer : IConsumer<HelloWorld>
    {
        public void Consume(ConsumeContext<HelloWorld> context)
        {
            System.Console.WriteLine($"Message received: {context.Message.Text}");
        }
    }
}
