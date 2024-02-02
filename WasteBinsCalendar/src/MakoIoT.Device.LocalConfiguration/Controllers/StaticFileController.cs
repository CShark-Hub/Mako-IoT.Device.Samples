using MakoIoT.Device.Services.Server.WebServer;

namespace MakoIoT.Device.LocalConfiguration.Controllers
{
    public class StaticFileController : StaticControllerBase
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


        [Route("appconfig.json")]
        [Method("GET")]
        public void GetAppconfig(WebServerEventArgs e)
        {
            Render(e.Context.Response, "appconfig.json");
        }
    }
}
