using System.Net;
using MakoIoT.Device.Services.Server.WebServer;

namespace MakoIoT.Device.LocalConfiguration.Controllers
{
    public class StaticFileController : StaticControllerBase
    {
        [Route("bundle.css")]
        [Method("GET")]
        public void BundleGet(WebServerEventArgs e)
        {
            e.Context.Response.Headers.Add("cache-control", "public, max-age=15552000");
            e.Context.Response.Headers.Add("content-encoding", "gzip");
            Render("bundle.css.gz", "text/css", e.Context.Response);
        }
    }
}
