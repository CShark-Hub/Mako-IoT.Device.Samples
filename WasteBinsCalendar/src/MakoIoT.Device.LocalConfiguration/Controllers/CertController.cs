using System;
using System.IO;
using System.Net;
using MakoIoT.Device.SecureClient;
using MakoIoT.Device.Services.FileStorage.Interface;
using MakoIoT.Device.Services.Interface;
using MakoIoT.Device.Services.Server.WebServer;

namespace MakoIoT.Device.LocalConfiguration.Controllers
{
    public class CertController
    {
        private readonly IStreamStorageService _storageService;
        private readonly ILog _logger;

        public CertController(ILog logger, IStreamStorageService storageService)
        {
            _logger = logger;
            _storageService = storageService;
        }


        [Route("api/cert")]
        [Method("POST")]
        public void Post(WebServerEventArgs e)
        {
            try
            {
                ParseFormMultipart(e.Context.Request.Headers["content-type"], e.Context.Request.InputStream);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                MakoWebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.InternalServerError);
                return;
            }

            MakoWebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.OK);
        }

        private void ParseFormMultipart(string contentType, Stream requestStream)
        {
            var boundary = $"--{contentType.Substring(contentType.IndexOf("boundary=") + 9)}";
            var finalBoundary = $"{boundary}--";

            var reader = new RequestStreamReader(requestStream);
            var line = "";
            while (line != finalBoundary)
            {
                if (line == boundary)
                {
                    line = reader.ReadLine();
                    if (line.ToLower().StartsWith("content-disposition:"))
                    {
                        var dispositionItems = line.Split(';');
                        var fieldName = dispositionItems.Length > 1
                            ? dispositionItems[1].Split('=')[1].Trim('\"', ' ')
                            : "";

                        line = reader.ReadLine();

                        if (line.ToLower().StartsWith("content-type:"))
                        {
                            //skip empty line
                            reader.ReadLine();

                            //process file
                            line = SaveFile(reader, boundary, Constants.CertificateFile);
                        }
                    }
                }
                else
                {
                    line = reader.ReadLine();
                }
            }
        }

        private string SaveFile(StreamReader reader, string boundary, string fileName)
        {
            //skip empty lines
            string line;
            do
            {
                line = reader.ReadLine();

            } while (line == "");

            if (line == null || line.StartsWith(boundary))
                return line;

            using var writer = _storageService.WriteToFileStream(fileName);
            while (line != null && !line.StartsWith(boundary))
            {
                writer.WriteLine(line);
                line = reader.ReadLine();
            }
            writer.Close();
            _logger.Trace($"File {fileName} saved");

            return line;
        }
    }
}
