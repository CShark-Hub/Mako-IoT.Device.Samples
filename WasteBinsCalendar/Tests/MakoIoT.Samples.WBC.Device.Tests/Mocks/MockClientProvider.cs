using System.Net.Http;
using MakoIoT.Device.SecureClient.Services;

namespace MakoIoT.Samples.WBC.Device.Tests.Mocks
{
    public class MockClientProvider : IClientProvider
    {
        public HttpClient GetSecureHttpClient(string baseUrl)
        {
            return new HttpClient();
        }
    }
}
