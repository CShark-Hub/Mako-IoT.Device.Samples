using System.IO;
using System.Net;

namespace MakoIoT.Device.LocalConfiguration.Controllers
{
    public abstract class StaticControllerBase
    {
        protected void Render(string file, string contentType, HttpListenerResponse response)
        {
            var buffer = new byte[1024];
            using var fileStream = File.OpenRead($"I:\\{file}");

            response.ContentType = contentType;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.ContentLength64 = fileStream.Length;

            int read;
            while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                response.OutputStream.Write(buffer, 0, read);
            }

            response.OutputStream.Flush();
            fileStream.Close();
        }
    }
}
