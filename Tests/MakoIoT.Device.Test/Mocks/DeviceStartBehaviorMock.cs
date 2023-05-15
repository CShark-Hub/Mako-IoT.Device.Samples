using MakoIoT.Device.Services.Interface;
using System;

namespace MakoIoT.Device.Test.Mocks
{
    internal class DeviceStartBehaviorMock : IDeviceStartBehavior
    {
        public bool DeviceStarting()
        {
            return false;
        }
    }
}
