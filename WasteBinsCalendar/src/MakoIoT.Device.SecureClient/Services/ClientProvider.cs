using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using MakoIoT.Device.Services.Interface;

namespace MakoIoT.Device.SecureClient.Services
{
    public class ClientProvider : IClientProvider
    {
        private readonly ILog _log;
        private readonly IStorageService _storageService;

        public ClientProvider(IStorageService storageService, ILog log)
        {
            _storageService = storageService;
            _log = log;
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
            {
                try
                {
                    _log.Trace($"Reading certificate from file {Constants.CertificateFile}");
                    var cs = _storageService.ReadFile(Constants.CertificateFile);
                    _log.Trace($"Certificate string length: {cs.Length}");
                    return new X509Certificate(cs);
                }
                catch (Exception e)
                {
                    _log.Error(e);
                }

            }

            return null;
        }
    }
}
