using MakoIoT.Messages;

namespace MakoIoT.Samples.Messaging.Shared.Messages
{
    public class HelloWorld : IMessage
    {
        public string MessageType { get; set; }

        public HelloWorld()
        {
            MessageType = this.GetType().FullName;
        }

        public string Text { get; set; }
    }
}
