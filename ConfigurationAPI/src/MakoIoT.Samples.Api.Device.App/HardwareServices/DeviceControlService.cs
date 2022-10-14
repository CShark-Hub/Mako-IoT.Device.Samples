using MakoIoT.Device.Services.ConfigurationManager.Interface;
using Microsoft.Extensions.Logging;
using nanoFramework.Runtime.Native;

namespace MakoIoT.Samples.Api.Device.App.HardwareServices
{
    public class DeviceControlService : IDeviceControl
    {
        private readonly ILogger _logger;

        public DeviceControlService(ILogger logger)
        {
            _logger = logger;
        }

        public void Reboot()
        {
            _logger.LogInformation("Rebooting...");
            Power.RebootDevice();
        }

        public void SignalConfigMode()
        {
        }

        public void SignalNormalMode()
        {
        }
    }
}
