using MakoIoT.Device.Services.Server;
using MakoIoT.Samples.WBC.Device.LocalConfiguration.Controllers.Api;
using MakoIoT.Samples.WBC.Device.LocalConfiguration.Controllers.Web;

namespace MakoIoT.Samples.WBC.Device.LocalConfiguration.Extensions
{
    public static class WebServerOptionsExtension
    {
        /// <summary>
        /// Registers configuration website controllers.
        /// </summary>
        /// <param name="options"></param>
        public static void AddConfigurationWebsite(this WebServerOptions options)
        {
            //web
            options.AddController(typeof(StaticWebFilesController));
            options.AddController(typeof(AppconfigController));

            //api
            options.AddController(typeof(ExitController));
            options.AddController(typeof(ConfigController));
            options.AddController(typeof(CertController));
        }
    }
}
