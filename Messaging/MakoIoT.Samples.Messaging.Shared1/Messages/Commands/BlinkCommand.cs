using MakoIoT.Messages;

namespace MakoIoT.Samples.Messaging.Shared.Messages
{
    public class BlinkCommand : IMessage
    {
        public BlinkCommand()
        {
            MessageType = this.GetType().FullName;
        }
        public bool LedOn { get; set; }
        public string MessageType { get; set; }
    }
}
