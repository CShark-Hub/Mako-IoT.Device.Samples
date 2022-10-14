using System;
using MakoIoT.Samples.WBC.Device.Services;

namespace MakoIoT.Samples.WBC.Device.Tests.Mocks
{
    public class MockDateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow { get; set; }
        public DateTime Now => UtcNow;
    }
}
