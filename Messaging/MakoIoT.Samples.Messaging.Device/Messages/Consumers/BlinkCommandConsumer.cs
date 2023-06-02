using MakoIoT.Device.Services.Interface;
using MakoIoT.Samples.Messaging.Device.Interface;
using MakoIoT.Samples.Messaging.Shared.Messages;
using Microsoft.Extensions.Logging;

namespace MakoIoT.Samples.Messaging.Device.Messages.Consumers
{
    public class BlinkCommandConsumer : IConsumer
    {
        private readonly IBlinker _blinker;
        private readonly ILogger _logger;

        public BlinkCommandConsumer(IBlinker blinker, ILogger logger)
        {
            _blinker = blinker;
            _logger = logger;
        }

        public void Consume(ConsumeContext context)
        {
            var cmd = (BlinkCommand)context.Message;
            _logger.LogDebug($"Setting LED to {cmd.LedOn}");
            _blinker.Set(cmd.LedOn);
        }
    }
}
