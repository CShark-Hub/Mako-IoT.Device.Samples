using System;
using System.Net.Http;

namespace MakoIoT.Device.SecureClient.Services
{
    public interface IClientProvider
    {
        HttpClient GetSecureHttpClient(string baseUrl);
    }
}
