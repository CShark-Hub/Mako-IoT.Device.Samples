using MakoIoT.Core.Services.Interface;

namespace MakoIoT.Samples.Messaging.Console
{
    public class ObjectFactory : IObjectFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ObjectFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public dynamic Create(Type type)
        {
            return (dynamic)_serviceProvider.GetService(type);
        }
    }
}
