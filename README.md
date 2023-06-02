# Mako-IoT Samples
## [Messaging](https://github.com/CShark-Hub/Mako-IoT.Device.Samples/tree/main/Messaging)
Demonstrates two-way communication between device (nanoFramework) and console app (.NET Core) through [message bus](https://github.com/CShark-Hub/Mako-IoT.Device.Services.Messaging). Routing is done automatically, based on message types. Strongly-typed data contracts are shared in code across both apps. Transport layer is done through MQTT.
## [Mediator](https://github.com/CShark-Hub/Mako-IoT.Device.Samples/tree/main/Mediator)
Practical example of [mediator](https://github.com/CShark-Hub/Mako-IoT.Device.Services.Mediator) usage. Demonstrates in-process communication between two services (classes) that don't know anything about one another.
## [Waste Bins Calendar](https://github.com/CShark-Hub/Mako-IoT.Device.Samples/tree/main/WasteBinsCalendar)
Project of a device, which indicates bins for colloctions on a given day. It demonstrates how to compose multiple MAKO IoT building blocks into a useful product :)
## [Configuration API](https://github.com/CShark-Hub/Mako-IoT.Device.Samples/tree/main/ConfigurationAPI)
Demonstrates how to use [web server](https://github.com/CShark-Hub/Mako-IoT.Device.Services.Server) in [WiFi access point](https://github.com/CShark-Hub/Mako-IoT.Device.Services.WiFi.AP) mode with [configuration manager](https://github.com/CShark-Hub/Mako-IoT.Device.Services.ConfigurationManager) and [API](https://github.com/CShark-Hub/Mako-IoT.Device.Services.ConfigurationApi) to configure a device.
## [Log Storage](https://github.com/CShark-Hub/Mako-IoT.Device.Samples/tree/main/LogStorage)
Example usage of [logging](https://github.com/CShark-Hub/Mako-IoT.Device.Services.Logging.Storage) to local storage and sending out the logs to Elasticsearch server.
