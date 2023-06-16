using MakoIoT.Device.Services.Interface;
using MakoIoT.Samples.Blink.Device.Interface;

namespace MakoIoT.Samples.Blink.Device.Tasks
{
    public class BlinkTask : ITask
    {
        private readonly IBlinker _blinker;
        private bool _isOn;
        
        public string Id { get; }

        public BlinkTask(IBlinker blinker)
        {
            _blinker = blinker;
            Id = nameof(BlinkTask);
        }

        public void Execute()
        {
            _isOn = !_isOn;
            _blinker.Blink(_isOn);
        }
    }
}
