using MakoIoT.Device.LocalConfiguration.Controllers;
using MakoIoT.Device.Services.Server;

namespace MakoIoT.Device.LocalConfiguration.Extensions
{
    public static class WebServerOptionsExtension
    {
        /// <summary>
        /// Registers configuration website controllers.
        /// </summary>
        /// <param name="options"></param>
        public static void AddConfigurationWebsite(this WebServerOptions options)
        {
            options.AddController(typeof(ExitController));
            options.AddController(typeof(StaticFileController));
            options.AddController(typeof(ConfigController));
            options.AddController(typeof(CertController));
        }
    }
}
