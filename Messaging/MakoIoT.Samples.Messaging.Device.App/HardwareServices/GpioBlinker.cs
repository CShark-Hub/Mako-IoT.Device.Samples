using System.Device.Gpio;
using MakoIoT.Samples.Messaging.Device.Interface;

namespace MakoIoT.Samples.Messaging.Device.App.HardwareServices
{
    public class GpioBlinker : IBlinker
    {
        private readonly GpioController _gpio;
        private readonly GpioPin _led;

        public GpioBlinker()
        {
            _gpio = new GpioController();

            _led = _gpio.OpenPin(2, PinMode.Output);
        }

        public void Set(bool ledOn)
        {
            _led.Write(ledOn ? PinValue.High : PinValue.Low);
        }
    }
}
