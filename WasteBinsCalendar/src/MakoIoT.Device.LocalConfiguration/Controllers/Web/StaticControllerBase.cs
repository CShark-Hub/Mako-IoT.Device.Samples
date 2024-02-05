using System.Collections;
using System.IO;
using System.Net;

namespace MakoIoT.Device.LocalConfiguration.Controllers.Web
{
    public abstract class StaticControllerBase
    {
        protected static readonly Hashtable KnownMimeTypes = new()
        {
            {"aac", "audio/aac"},
            {"avi", "video/x-msvideo"},
            {"bmp", "image/bmp"},
            {"css", "text/css"},
            {"gif", "image/gif"},
            {"htm", "text/html; charset=utf-8"},
            {"html", "text/html; charset=utf-8"},
            {"ico", "image/vnd.microsoft.icon"},
            {"ics", "text/calendar"},
            {"jar", "application/java-archive"},
            {"jpg", "image/jpeg"},
            {"jpeg", "image/jpeg"},
            {"js", "text/javascript"},
            {"json", "application/json"},
            {"mid", "audio/midi"},
            {"midi", "audio/midi"},
            {"mp3", "audio/mpeg"},
            {"mp4", "video/mp4"},
            {"mpeg", "video/mpeg"},
            {"png", "image/png"},
            {"pdf", "application/pdf"},
            {"svg", "image/svg+xml"},
            {"txt", "text/plain"},
            {"wav", "audio/wav"},
            {"woff", "font/woff"},
            {"woff2", "font/woff2"},
            {"xhtml", "application/xhtml+xml"},
            {"xml", "application/xml"},
        };

        protected string DefaultMimeType = "application/octet-stream";
        protected int BufferSize = 1024;
        protected string RootPath = "I:\\";

        protected void Render(HttpListenerResponse response, string file, string contentType = null)
        {
            contentType ??= GetMimeType(file);
            if (File.Exists($"{RootPath}{file}.gz"))
            {
                response.Headers.Add("content-encoding", "gzip");
                file = $"{file}.gz";
            }

            if (!File.Exists($"{RootPath}{file}"))
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.ContentLength64 = 0;
                return;
            }

            var buffer = new byte[BufferSize];
            using var fileStream = File.OpenRead($"{RootPath}{file}");

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

        private string GetMimeType(string fileName)
        {
            var s = fileName.Split('.');
            return KnownMimeTypes.Contains(s[s.Length - 1]) ? (string)KnownMimeTypes[s[s.Length - 1]] : DefaultMimeType;
        }
    }
}
