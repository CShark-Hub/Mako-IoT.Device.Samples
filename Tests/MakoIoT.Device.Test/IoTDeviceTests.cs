using MakoIoT.Device.Services.Interface;
using MakoIoT.Device.Test.Mocks;
using nanoFramework.DependencyInjection;
using nanoFramework.TestFramework;
using System;

namespace MakoIoT.Device.Test
{
    [TestClass]
    public class IoTDeviceTests
    {
        [TestMethod]
        public void IsRegisted_Should_ReturnTrueIfObjectIsRegistered()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(typeof(IDeviceStartBehavior), typeof(DeviceStartBehaviorMock));
            var service = serviceCollection.BuildServiceProvider();

            var result = IoTDevice.IsRegistered(service, typeof(IDeviceStartBehavior));

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsRegisted_Should_ReturnFalseIfObjectIsNotRegistered()
        {
            var serviceCollection = new ServiceCollection();
            var service = serviceCollection.BuildServiceProvider();

            var result = IoTDevice.IsRegistered(service, typeof(IDeviceStartBehavior));

            Assert.IsFalse(result);
        }

    }
}
