using System.Net;
using MakoIoT.Device.Services.Server.WebServer;

namespace MakoIoT.Device.LocalConfiguration.Controllers
{
    public class StaticFileController : StaticControllerBase
    {
        [Route("static/{fileName}")]
        [Method("GET")]
        public void Get(string fileName, WebServerEventArgs e)
        {
            Render(e.Context.Response, fileName);
        }
    }
}
