using System;
using System.Device.Pwm;
using MakoIoT.Device.Displays.Led;
using nanoFramework.Hardware.Esp32;

namespace MakoIoT.Samples.WBC.Device.Esp32.HardwareServices
{
    /// <summary>
    /// ESP32 PWM RGB LED driver
    /// </summary>
    public class PwmPixelDriver : IPixelDriver
    {
        private const int Frequency = 5000;

        private readonly PwmChannel _rChannel;
        private readonly PwmChannel _gChannel;
        private readonly PwmChannel _bChannel;
        private readonly bool _inverse;

        /// <summary>
        /// Red channel calibration factor [0..1]
        /// </summary>
        public double RFactor { get; }
        /// <summary>
        /// Green channel calibration factor [0..1]
        /// </summary>
        public double GFactor { get; }
        /// <summary>
        /// Blue channel calibration factor [0..1]
        /// </summary>
        public double BFactor { get; }

        /// <summary>
        /// Creates new instance of PwmPixelDriver
        /// </summary>
        /// <param name="rPin">Red channel pin no.</param>
        /// <param name="gPin">Green channel pin no.</param>
        /// <param name="bPin">Blue channel pin no.</param>
        /// <param name="inverse">True for common anode LED, False for common cathode</param>
        /// <param name="rFactor">Red channel calibration factor [0..1]</param>
        /// <param name="gFactor">Green channel calibration factor [0..1]</param>
        /// <param name="bFactor">Blue channel calibration factor [0..1]</param>
        public PwmPixelDriver(int rPin, int gPin, int bPin, bool inverse = false, double rFactor = 1, double gFactor = 1, double bFactor = 1)
        {
            _inverse = inverse;

            _rChannel = SetupChannel(rPin);
            _gChannel = SetupChannel(gPin);
            _bChannel = SetupChannel(bPin);

            GuardFactor(rFactor);
            GuardFactor(gFactor);
            GuardFactor(bFactor);

            RFactor = rFactor;
            GFactor = gFactor;
            BFactor = bFactor;
        }

        /// <inheritdoc/>
        public void SetPixel(Color color)
        {
            SetValue(_rChannel, color.R * RFactor);
            SetValue(_gChannel, color.G * GFactor);
            SetValue(_bChannel, color.B * BFactor);
        }

        private PwmChannel SetupChannel(int pin)
        {
            nanoFramework.Hardware.Esp32.Configuration.SetPinFunction(pin, DeviceFunction.PWM1);
            return PwmChannel.CreateFromPin(pin, Frequency);
        }

        private void SetValue(PwmChannel channel, double value)
        {
            double dc = value / (double)255;
            channel.DutyCycle = _inverse ? 1 - dc : dc;
        }

        private static void GuardFactor(double factor)
        {
            if (factor < 0 || factor > 1)
                throw new ArgumentOutOfRangeException(nameof(factor), "Factor must be in range [0,1]");
        }
    }
}
