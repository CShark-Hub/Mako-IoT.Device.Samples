using System.Threading;
using MakoIoT.Device.Services.ConfigurationManager;
using MakoIoT.Device.Services.Server.WebServer;

namespace MakoIoT.Device.LocalConfiguration.Controllers
{
    public class ExitController : ControllerBase
    {
        private readonly IConfigManager _configManager;

        public ExitController(IConfigManager configManager)
            : base("exit.html")
        {
            _configManager = configManager;
        }

        [Route("exit.html")]
        [Method("GET")]
        public void Get(WebServerEventArgs e)
        {
            Render(e.Context.Response, false);

            new Thread(() =>
            {
                Thread.Sleep(5000);
                _configManager.StopConfigMode();
            }).Start();
        }
    }
}
