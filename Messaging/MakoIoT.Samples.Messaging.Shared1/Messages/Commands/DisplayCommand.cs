using MakoIoT.Messages;

namespace MakoIoT.Samples.Messaging.Shared.Messages
{
    public class DisplayCommand : IMessage
    {
        public DisplayCommand()
        {
            MessageType = this.GetType().FullName;
        }
        public string MessageType { get; set; }

        public string Text { get; set; }
    }
}
