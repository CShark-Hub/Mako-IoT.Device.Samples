using MakoIoT.Device.Services.ConfigurationManager.Interface;
using MakoIoT.Samples.WBC.Device.Services;
using Microsoft.Extensions.Logging;
using nanoFramework.Runtime.Native;

namespace MakoIoT.Samples.WBC.Device.Esp32.HardwareServices
{
    public class DeviceControlService : IDeviceControl
    {
        private readonly ILogger _logger;
        private readonly IDisplayController _displayController;

        public DeviceControlService(ILogger logger, IDisplayController displayController)
        {
            _logger = logger;
            _displayController = displayController;
        }

        public void Reboot()
        {
            _logger.LogInformation("Rebooting...");
            Power.RebootDevice();
        }

        public void SignalConfigMode()
        {
            _displayController.DisplayConfigMode();
        }

        public void SignalNormalMode()
        {
            
        }
    }
}
