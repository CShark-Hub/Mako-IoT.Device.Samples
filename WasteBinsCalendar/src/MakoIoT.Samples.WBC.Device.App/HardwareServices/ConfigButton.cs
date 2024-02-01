using Iot.Device.Button;
using MakoIoT.Device.Services.ConfigurationManager.Events;
using MakoIoT.Device.Services.Interface;
using MakoIoT.Device.Services.Mediator;
using Microsoft.Extensions.Logging;

namespace MakoIoT.Samples.WBC.Device.App.HardwareServices
{
    public class ConfigButton
    {
        private readonly IMediator _mediator;
        private readonly ILog _logger;

        public ConfigButton(IMediator mediator, ILog logger)
        {
            _mediator = mediator;
            _logger = logger;

            var configModeButton = new GpioButton(buttonPin: 13);
            configModeButton.Press += ConfigModeButton_Press;
            
            var resetToDefaultsButton = new GpioButton(buttonPin: 12);
            resetToDefaultsButton.Press += ResetToDefaultsButton_Press;

        }

        private void ConfigModeButton_Press(object sender, System.EventArgs e)
        {
            _logger.Trace("Config Mode button pressed");
            _mediator.Publish(new ConfigModeToggleEvent { Mode = SwitchMode.Toggle });
        }

        private void ResetToDefaultsButton_Press(object sender, System.EventArgs e)
        {
            _logger.Trace("Reset To Defaults button pressed");
            _mediator.Publish(new ResetToDefaultsEvent());
        }

    }
}
