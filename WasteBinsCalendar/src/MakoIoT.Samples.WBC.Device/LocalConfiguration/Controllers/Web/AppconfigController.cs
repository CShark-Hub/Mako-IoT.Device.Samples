using System.Net;
using MakoIoT.Device.Services.Interface;
using MakoIoT.Device.Services.Server.WebServer;
using MakoIoT.Device.Services.WiFi.AP.Configuration;

namespace MakoIoT.Samples.WBC.Device.LocalConfiguration.Controllers.Web
{
    public class AppconfigController
    {
        private readonly string _ipAddress;

        public AppconfigController(IConfigurationService configService)
        {
            _ipAddress =
                ((WiFiAPConfig)configService.GetConfigSection(WiFiAPConfig.SectionName, typeof(WiFiAPConfig)))
                .IpAddress;
        }


        [Route("appconfig.json")]
        [Method("GET")]
        public void GetAppconfig(WebServerEventArgs e)
        {
            string appconfig = $@"{{""backendUrl"": ""http://{_ipAddress}/api""}}";
            e.Context.Response.ContentType = "application/json";
            e.Context.Response.StatusCode = (int)HttpStatusCode.OK;
            MakoWebServer.OutPutStream(e.Context.Response, appconfig);
        }
    }
}
