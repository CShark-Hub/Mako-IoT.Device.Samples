using nanoFramework.TestFramework;
using System;
using System.Threading;
using MakoIoT.Device.Displays.Led;
using MakoIoT.Samples.WBC.Device.Esp32.HardwareServices;
using MakoIoT.Samples.WBC.Device.Model;
using MakoIoT.Samples.WBC.Device.Services;

namespace MakoIoT.Samples.WBC.Device.HardwareTest
{
    [TestClass]
    public class PwmPixelDriverTest
    {
        [TestMethod]
        public void SetPixel_should_display_all_colors()
        {
            Console.WriteLine($"TEST: {nameof(SetPixel_should_display_all_colors)}");

            var testColors = new Color[]
            {
                BinColors.Red, BinColors.Green, BinColors.Blue, BinColors.Black, BinColors.Brown, BinColors.Yellow,
                DisplayController.ColorBlank, DisplayController.ColorUpdating, DisplayController.ColorError, DisplayController.ColorConfigMode
            };

            var sut = new PwmPixelDriver(HardwareConsts.RPin, HardwareConsts.GPin, HardwareConsts.BPin, HardwareConsts.Inverse, HardwareConsts.RFactor, HardwareConsts.GFactor, HardwareConsts.BFactor);

            foreach (var color in testColors)
            {
                sut.SetPixel(color);
                Thread.Sleep(1000);
            }

            Assert.IsTrue(true);
        }
    }
}
