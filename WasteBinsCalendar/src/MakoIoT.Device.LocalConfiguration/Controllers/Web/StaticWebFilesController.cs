using MakoIoT.Device.Services.Server.WebServer;

namespace MakoIoT.Device.LocalConfiguration.Controllers.Web
{
    public class StaticWebFilesController : StaticControllerBase
    {
        [Route("")]
        [Route("index.html")]
        [Method("GET")]
        public void GetIndex(WebServerEventArgs e)
        {
            Render(e.Context.Response, "index.html");
        }

        [Route("index.js")]
        [Method("GET")]
        public void GetJs(WebServerEventArgs e)
        {
            Render(e.Context.Response, "index.js");
        }

        [Route("index.css")]
        [Method("GET")]
        public void GetCss(WebServerEventArgs e)
        {
            Render(e.Context.Response, "index.css");
        }

        [Route("favicon.ico")]
        [Method("GET")]
        public void GetFavicon(WebServerEventArgs e)
        {
            Render(e.Context.Response, "favicon.ico");
        }

        [Route("info-square.svg")]
        [Method("GET")]
        public void GetInfoSquare(WebServerEventArgs e)
        {
            Render(e.Context.Response, "info-square.svg");
        }
    }
}
