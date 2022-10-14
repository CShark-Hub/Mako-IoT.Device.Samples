using MakoIoT.Device.Services.Logging.Storage.Configuration;
using MakoIoT.Device.Services.WiFi.Configuration;

namespace MakoIoT.Samples.LogStorage.Device.App
{
    internal partial class Config
    {
#if !DEBUG
        
        internal static WiFiConfig WiFiConfig => new WiFiConfig();
        internal static LogStorageConfig LogStorageConfig => new LogStorageConfig();
#endif
    }
}
