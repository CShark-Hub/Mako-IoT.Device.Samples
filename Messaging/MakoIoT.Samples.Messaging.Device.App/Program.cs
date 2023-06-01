using System.Collections;
using System.Threading;
using MakoIoT.Device;
using MakoIoT.Device.Services.Configuration.Extensions;
using MakoIoT.Device.Services.DataProviders.Configuration;
using MakoIoT.Device.Services.DataProviders.Extensions;
using MakoIoT.Device.Services.FileStorage.Extensions;
using MakoIoT.Device.Services.Interface;
using MakoIoT.Device.Services.Logging.Configuration;
using MakoIoT.Device.Services.Logging.Extensions;
using MakoIoT.Device.Services.Messaging.Extensions;
using MakoIoT.Device.Services.Mqtt.Configuration;
using MakoIoT.Device.Services.Mqtt.Extensions;
using MakoIoT.Device.Services.WiFi.Configuration;
using MakoIoT.Device.Services.WiFi.Extensions;
using MakoIoT.Samples.Messaging.Device.App.HardwareServices;
using MakoIoT.Samples.Messaging.Device.DataProviders;
using MakoIoT.Samples.Messaging.Device.Interface;
using MakoIoT.Samples.Messaging.Device.Messages.Consumers;
using MakoIoT.Samples.Messaging.Shared.Messages;
using Microsoft.Extensions.Logging;
using nanoFramework.DependencyInjection;

namespace MakoIoT.Samples.Messaging.Device.App
{
    public class Program
    {
        public static void Main()
        {
            DeviceBuilder.Create()
                .ConfigureDI(services =>
                {
                    services.AddSingleton(typeof(IBlinker), typeof(GpioBlinker));
                })
                .AddLogging(new LoggerConfig(LogLevel.Debug, new Hashtable
                {
                    { "DI", LogLevel.Debug },
                    { "MqttCommunicationService", LogLevel.Trace }
                }))
                .AddFileStorage()
                .AddMqtt()
                .AddMessageBus(o =>
                {
                    o.AddDirectMessageConsumer(typeof(BlinkCommand), typeof(BlinkCommandConsumer), ConsumeStrategy.LastMessageWins);
                })
                .AddWiFi()
                .AddDataProviders(o =>
                {
                    o.AddDataProvider(typeof(HelloWorldDataProvider), nameof(HelloWorldDataProvider));
                })
                .AddConfiguration(cfg =>
                {
                    cfg.WriteDefault(DataProvidersConfig.SectionName, new DataProvidersConfig
                    {
                        Providers = new[]
                        {
                            new DataProviderConfig { DataProviderId = "HelloWorldDataProvider", PollInterval = 5000 }
                        }
                    }, true);

                    cfg.WriteDefault(WiFiConfig.SectionName, new WiFiConfig
                    {
                        Ssid = "",
                        Password = ""
                    }, true);

                    cfg.WriteDefault(MqttConfig.SectionName, new MqttConfig
                    {
                        BrokerAddress = "test.mosquitto.org",
                        Port = 1883,
                        UseTLS = false,
                        ClientId = "device1",
                        TopicPrefix = "mako-iot-test"
                    }, true);
                })
                .Build()
                .Start();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
