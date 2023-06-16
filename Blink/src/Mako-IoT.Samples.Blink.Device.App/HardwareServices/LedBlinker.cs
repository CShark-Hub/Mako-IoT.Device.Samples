using System;
using System.Device.Gpio;
using MakoIoT.Samples.Blink.Device.Interface;

namespace MakoIoT.Samples.Blink.Device.App.HardwareServices
{
    public class LedBlinker : IBlinker
    {
        private readonly GpioPin _ledPin;

        public LedBlinker()
        {
            var gpio = new GpioController();
            _ledPin = gpio.OpenPin(2, PinMode.Output);
        }

        public void Blink(bool isOn)
        {
            _ledPin.Write(isOn ? PinValue.High : PinValue.Low);
        }
    }
}
