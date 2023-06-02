using MakoIoT.Core.Services.Interface;
using MakoIoT.Core.Services.Messaging;
using MakoIoT.Core.Services.Mqtt;
using MakoIoT.Core.Services.Mqtt.Configuration;
using MakoIoT.Messages;
using MakoIoT.Samples.Messaging.Console;
using MakoIoT.Samples.Messaging.Shared.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ObjectFactory = MakoIoT.Samples.Messaging.Console.ObjectFactory;

Console.WriteLine("Hello!");

var serviceProvider = new ServiceCollection()
    .AddLogging(configure => configure.AddConsole())
    .AddSingleton<ICommunicationService, MqttCommunicationService>()
    .AddSingleton<IMessageBus, MessageBus>()
    .AddSingleton<IObjectFactory, ObjectFactory>()
    .AddTransient<HelloWorldConsumer>()
    .AddSingleton(new MqttConfig
    {
        BrokerAddress = "test.mosquitto.org",
        Port = 1883,
        UseTLS = false,
        ClientId = "console-client-1",
        TopicPrefix = "mako-iot-test"
    })
    .BuildServiceProvider();

var bus = serviceProvider.GetService<IMessageBus>();
bus.RegisterSubscriptionConsumer(typeof(HelloWorld), typeof(HelloWorldConsumer));

bus.Start();

string input;
do
{
    input = Console.ReadLine();
    IMessage msg = null;
    switch (input)
    {
        case "led on":
            msg = new BlinkCommand { LedOn = true };
            break;
        case "led off":
            msg = new BlinkCommand { LedOn = false };
            break;
        case "display":
            msg = new DisplayCommand { Text = $"Hello from Mako.IoT@{Environment.MachineName}" };
            break;
    }

    if (msg != null)
        bus.Send(msg, "device1");

} while (input != "");