using MakoIoT.Device.Services.Interface;

namespace MakoIoT.Samples.WBC.Device.Tests.Mocks
{
    public class MockNetworkProvider : INetworkProvider
    {
        public bool IsConnected { get; }
        public string ClientAddress { get; }
        public void Connect()
        {
            throw new System.NotImplementedException();
        }

        public void Disconnect()
        {
            throw new System.NotImplementedException();
        }
    }
}
