using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using MakoIoT.Device.Services.Interface;

namespace MakoIoT.Device.SecureClient.Services
{
    public class ClientProvider : IClientProvider
    {
        private readonly IStorageService _storageService;

        public ClientProvider(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public HttpClient GetSecureHttpClient(string baseUrl)
        {
            return new HttpClient
            {
                BaseAddress = new Uri(baseUrl),
                HttpsAuthentCert = baseUrl.ToLower().StartsWith("https")
                    ? GetCertificate()
                    : null
            };
        }

        private X509Certificate GetCertificate()
        {
            if (_storageService.FileExists(Constants.CertificateFile))
                return new X509Certificate(_storageService.ReadFile(Constants.CertificateFile));
            return null;
        }
    }
}
