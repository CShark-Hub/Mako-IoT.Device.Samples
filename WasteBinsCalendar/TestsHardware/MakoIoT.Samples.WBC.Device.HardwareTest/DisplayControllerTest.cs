using System.Threading;
using MakoIoT.Device.Services.Logging;
using MakoIoT.Device.Services.Logging.Configuration;
using MakoIoT.Samples.WBC.Device.App.HardwareServices;
using MakoIoT.Samples.WBC.Device.Model;
using MakoIoT.Samples.WBC.Device.Services;
using Microsoft.Extensions.Logging;
using nanoFramework.TestFramework;

namespace MakoIoT.Samples.WBC.Device.HardwareTest
{
    [TestClass]
    public class DisplayControllerTest
    {
        [TestMethod]
        public void DisplayTodaysBins_should_do_transition()
        {
            var colors = new[] { BinColors.Blue, BinColors.Green, BinColors.Yellow };

            var sut = new DisplayController(
                new PwmPixelDriver(HardwareConsts.RPin, HardwareConsts.GPin, HardwareConsts.BPin,
                    HardwareConsts.Inverse, HardwareConsts.RFactor, HardwareConsts.GFactor, HardwareConsts.BFactor),
                new MakoIoTLogger(new LoggerConfig(LogLevel.Debug)));

            sut.DisplayTodaysBins(colors);

            Thread.Sleep(10000);

            sut.Blank();
        }

        [TestMethod]
        public void DisplayTodaysBins_should_do_fade_transition()
        {
            var colors = new[] { BinColors.Blue, BinColors.Green, BinColors.Yellow };

            var sut = new DisplayController(
                new PwmPixelDriver(HardwareConsts.RPin, HardwareConsts.GPin, HardwareConsts.BPin,
                    HardwareConsts.Inverse, HardwareConsts.RFactor, HardwareConsts.GFactor, HardwareConsts.BFactor),
                new MakoIoTLogger(new LoggerConfig(LogLevel.Debug)));

            sut.DisplayTomorrowsBins(colors);

            Thread.Sleep(10000);

            sut.Blank();
        }

        [TestMethod]
        public void DisplayUpdating_should_blink_smooth_purple()
        {
            var sut = new DisplayController(
                new PwmPixelDriver(HardwareConsts.RPin, HardwareConsts.GPin, HardwareConsts.BPin,
                    HardwareConsts.Inverse, HardwareConsts.RFactor, HardwareConsts.GFactor, HardwareConsts.BFactor),
                new MakoIoTLogger(new LoggerConfig(LogLevel.Debug)));

            sut.DisplayUpdating();

            Thread.Sleep(5000);

            sut.Blank();
        }

        [TestMethod]
        public void DisplayUpdatingError_should_blink_purple()
        {
            var sut = new DisplayController(
                new PwmPixelDriver(HardwareConsts.RPin, HardwareConsts.GPin, HardwareConsts.BPin,
                    HardwareConsts.Inverse, HardwareConsts.RFactor, HardwareConsts.GFactor, HardwareConsts.BFactor),
                new MakoIoTLogger(new LoggerConfig(LogLevel.Debug)));

            sut.DisplayUpdatingError();

            Thread.Sleep(5000);

            sut.Blank();
        }

        [TestMethod]
        public void DisplayUpdating_should_blink_red()
        {
            var sut = new DisplayController(
                new PwmPixelDriver(HardwareConsts.RPin, HardwareConsts.GPin, HardwareConsts.BPin,
                    HardwareConsts.Inverse, HardwareConsts.RFactor, HardwareConsts.GFactor, HardwareConsts.BFactor),
                new MakoIoTLogger(new LoggerConfig(LogLevel.Debug)));

            sut.DisplayError();

            Thread.Sleep(5000);

            sut.Blank();
        }
    }
}
