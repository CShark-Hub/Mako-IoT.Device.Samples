using System.Threading;
using MakoIoT.Device.Services.ConfigurationManager;
using MakoIoT.Device.Services.Server.WebServer;

namespace MakoIoT.Device.LocalConfiguration.Controllers
{
    public class ExitController : StaticControllerBase
    {
        private readonly IConfigManager _configManager;

        public ExitController(IConfigManager configManager)
        {
            _configManager = configManager;
        }

        [Route("exit.html")]
        [Method("GET")]
        public void Get(WebServerEventArgs e)
        {
            Render("exit.html", "text/html; charset=utf-8", e.Context.Response);

            new Thread(() =>
            {
                Thread.Sleep(5000);
                _configManager.StopConfigMode();
            }).Start();
        }
    }
}
