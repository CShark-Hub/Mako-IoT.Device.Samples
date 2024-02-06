using MakoIoT.Device.SecureClient;
using MakoIoT.Device.Services.Interface;
using MakoIoT.Device.Services.Server.WebServer;
using MakoIoT.Device.WebServer.Controllers;

namespace MakoIoT.Samples.WBC.Device.LocalConfiguration.Controllers.Api
{
    public class CertController : FileUploadApiControllerBase
    {
        public CertController(ILog logger) : base(logger)
        {

        }


        [Route("api/cert")]
        [Method("POST")]
        public void Post(WebServerEventArgs e)
        {
            base.PostFile(e, $"I:\\{Constants.CertificateFile}");
        }
    }
}
