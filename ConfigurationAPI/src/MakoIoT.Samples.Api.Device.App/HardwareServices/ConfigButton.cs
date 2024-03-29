﻿using Iot.Device.Button;
using MakoIoT.Device.Services.ConfigurationManager.Events;
using MakoIoT.Device.Services.Mediator;
using Microsoft.Extensions.Logging;

namespace MakoIoT.Samples.Api.Device.App.HardwareServices
{
    public class ConfigButton
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public ConfigButton(IMediator mediator, ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;

            var configModeButton = new GpioButton(buttonPin: 18);
            configModeButton.Press += ConfigModeButton_Press;

            var resetToDefaultsButton = new GpioButton(buttonPin: 19);
            resetToDefaultsButton.Press += ResetToDefaultsButton_Press;

        }

        private void ConfigModeButton_Press(object sender, System.EventArgs e)
        {
            _logger.LogDebug("Config Mode button pressed");
            _mediator.Publish(new ConfigModeToggleEvent { Mode = SwitchMode.On});
        }

        private void ResetToDefaultsButton_Press(object sender, System.EventArgs e)
        {
            _logger.LogDebug("Reset To Defaults button pressed");
            _mediator.Publish(new ResetToDefaultsEvent());
        }

    }
}
