using System;

namespace MakoIoT.Samples.WBC.Device.Services
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
        DateTime Now { get; }
    }
}
