using System.Net;
using System.Threading;
using MakoIoT.Device.Services.ConfigurationManager;
using MakoIoT.Device.Services.Server.WebServer;

namespace MakoIoT.Device.LocalConfiguration.Controllers
{
    public class ExitController
    {
        private readonly IConfigManager _configManager;

        public ExitController(IConfigManager configManager)
        {
            _configManager = configManager;
        }

        [Route("api/exit")]
        [Method("GET")]
        public void Get(WebServerEventArgs e)
        {
            MakoWebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.OK);

            new Thread(() =>
            {
                Thread.Sleep(5000);
                _configManager.StopConfigMode();
            }).Start();
        }
    }
}
