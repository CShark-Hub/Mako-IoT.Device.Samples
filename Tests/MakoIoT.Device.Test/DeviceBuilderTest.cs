using nanoFramework.TestFramework;

namespace MakoIoT.Device.Test
{
    [TestClass]
    public class DeviceBuilderTest
    {
        [TestMethod]
        public void Build_Should_ReturnValidObject()
        {
            var builder = DeviceBuilder.Create();

            var device = builder.Build();

            Assert.IsNotNull(device);
        }

        [TestMethod]
        public void Build_should_register_Starting_event_on_Device()
        {
            bool startingCalled = false, stoppedCalled = false;


            var builder = DeviceBuilder.Create();

            builder.DeviceStarting += (sender) => { startingCalled = true; };
            builder.DeviceStopped += (sender) => { stoppedCalled = true; };

            var device = builder.Build();

            device.Start();

            Assert.IsTrue(startingCalled);
            Assert.IsFalse(stoppedCalled);
        }

        [TestMethod]
        public void Build_should_register_Stopped_event_on_Device()
        {
            bool startingCalled = false, stoppedCalled = false;


            var builder = DeviceBuilder.Create();

            builder.DeviceStarting += (sender) => { startingCalled = true; };
            builder.DeviceStopped += (sender) => { stoppedCalled = true; };

            var device = builder.Build();

            device.Stop();

            Assert.IsFalse(startingCalled);
            Assert.IsTrue(stoppedCalled);
        }
    }
}
