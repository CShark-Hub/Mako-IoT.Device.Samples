using System.Net;
using MakoIoT.Device.Services.Server.WebServer;

namespace MakoIoT.Device.LocalConfiguration.Controllers
{
    public class StaticFileController
    {
        [Route("bundle.css")]
        [Method("GET")]
        public void BundleGet(WebServerEventArgs e)
        {
            e.Context.Response.Headers.Add("cache-control", "public, max-age=15552000");
            e.Context.Response.Headers.Add("content-encoding", "gzip");
            e.Context.Response.StatusCode = (int)HttpStatusCode.OK;
            MakoWebServer.SendFileOverHTTP(e.Context.Response, "bundle.css.gz",
                HtmlResources.GetBytes(HtmlResources.BinaryResources.bundle_css), "text/css");
        }
    }
}
