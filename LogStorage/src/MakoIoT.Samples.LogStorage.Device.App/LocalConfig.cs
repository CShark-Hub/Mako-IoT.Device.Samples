using MakoIoT.Device.Services.Logging.Storage.Configuration;
using MakoIoT.Device.Services.WiFi.Configuration;

namespace MakoIoT.Samples.LogStorage.Device.App
{
    internal partial class Config
    {
#if DEBUG
        internal static WiFiConfig WiFiConfig => new WiFiConfig
        {
            Ssid = "CSHARK-IoT",
            Password = "Yu4yfrNP2dTxp8Ns"
        };

        internal static LogStorageConfig LogStorageConfig => new LogStorageConfig
        {
            DeviceId = "My Mako-IoT device",
            ServiceUrl = "http://192.168.91.104:9200/logs-makoiot-devices/_doc",
            // ServiceAuthorization = "Basic ZWxhc3RpYzpjaGFuZ2VtZQ=="
        };
#endif
    }
}
