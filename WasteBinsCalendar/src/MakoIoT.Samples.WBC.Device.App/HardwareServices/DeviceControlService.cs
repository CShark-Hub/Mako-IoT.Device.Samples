using MakoIoT.Device.Services.ConfigurationManager.Interface;
using MakoIoT.Device.Services.Interface;
using MakoIoT.Samples.WBC.Device.Services;
using nanoFramework.Runtime.Native;

namespace MakoIoT.Samples.WBC.Device.App.HardwareServices
{
    public class DeviceControlService : IDeviceControl
    {
        private readonly ILog _logger;
        private readonly IDisplayController _displayController;

        public DeviceControlService(ILog logger, IDisplayController displayController)
        {
            _logger = logger;
            _displayController = displayController;
        }

        public void Reboot()
        {
            _logger.Information("Rebooting...");
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
