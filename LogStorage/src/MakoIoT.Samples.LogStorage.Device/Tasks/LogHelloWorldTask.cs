using MakoIoT.Device.Services.Interface;
using MakoIoT.Device.Services.Logging.Storage.Services;
using Microsoft.Extensions.Logging;

namespace MakoIoT.Samples.LogStorage.Device.Tasks
{
    /// <summary>
    /// Logs "Hello World!"
    /// </summary>
    public class LogHelloWorldTask : ITask
    {
        private readonly ILogHandlingService _logHandlingService;
        private readonly ILogger _logger;
        public string Id { get; }
        public LogHelloWorldTask(ILogger logger, ILogHandlingService logHandlingService)
        {
            _logger = logger;
            _logHandlingService = logHandlingService;
            Id = nameof(LogHelloWorldTask);
        }

        public void Execute()
        {
            _logger.LogInformation("Hello World!");
        }
    }
}
