using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using MakoIoT.Device.LocalConfiguration.Extensions;
using MakoIoT.Device.Services.Server.WebServer;

namespace MakoIoT.Device.LocalConfiguration.Controllers
{
    public abstract class ControllerBase
    {
        public delegate string FileUploadDelegate(string fieldName, string fileName, StreamReader contentsReader, string boundary);

        protected string Html;
        protected Hashtable HtmlParams;
        protected readonly Hashtable Form = new();

        protected ControllerBase(string html)
        {
            Html = html;
            HtmlParams = ParseParams();
        }

        protected void Render(HttpListenerResponse response, bool copyFormToParams)
        {
            if (copyFormToParams)
            {
                foreach (var formKey in Form.Keys)
                {
                    HtmlParams.AddOrUpdate(formKey, Form[formKey]);
                }
            }

            var htmlBuilder = new StringBuilder(Html);
            foreach (var key in HtmlParams.Keys)
            {
                htmlBuilder.Replace($"{{{key}}}", HtmlParams[key].ToString());
            }

            response.ContentType = "text/html; charset=utf-8";
            response.StatusCode = (int)HttpStatusCode.OK;
            MakoWebServer.OutPutStream(response, htmlBuilder.ToString());
        }

        protected void ParseForm(HttpListenerRequest request, FileUploadDelegate fileUploadDelegate = null)
        {
            ParseForm(request.Headers["content-type"], request.ContentLength64, request.InputStream, fileUploadDelegate);

        }

        protected void ParseForm(string contentType, long contentLength, Stream requestStream, FileUploadDelegate fileUploadDelegate = null)
        {
            if (contentType == "application/x-www-form-urlencoded")
            {
                ParseFormUrlencoded(contentLength, requestStream);
                return;
            }

            if (contentType.StartsWith("multipart/form-data"))
            {
                ParseFormMultipart(contentType, contentLength, requestStream, fileUploadDelegate);
                return;
            }

            throw new NotSupportedException("Content Type not supported");
        }

        protected Hashtable ParseParams()
        {
            var t = new Hashtable();
            int p = 0;
            p = Html.IndexOf('{', p);
            while (p >= 0)
            {
                int startIndex = p + 1;
                p = Html.IndexOf('}', p);
                var key = Html.Substring(startIndex, p - startIndex);
                if (!t.Contains(key))
                    t.Add(key, "");
                p = Html.IndexOf('{', p);
            }
            return t;
        }

        private void ParseFormUrlencoded(long contentLength, Stream requestStream)
        {
            byte[] buffer = new byte[contentLength];
            requestStream.Read(buffer, 0, buffer.Length);
            var items = Encoding.UTF8.GetString(buffer, 0, buffer.Length).Split('&');
            foreach (var item in items)
            {
                var i = item.Split('=');
                Form.AddOrUpdate(i[0], i.Length > 1 ? HttpUtility.UrlDecode(i[1]) : "");
            }
        }

        private void ParseFormMultipart(string contentType, long contentLength, Stream requestStream, FileUploadDelegate fileUploadDelegate)
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
                            if (fileUploadDelegate != null)
                            {
                                string fileName = dispositionItems.Length > 2
                                    ? dispositionItems[2].Split('=')[1].Trim('\"', ' ')
                                    : "";

                                line = fileUploadDelegate(fieldName, fileName, reader, boundary);
                            }
                        }
                        else
                        {
                            var fieldValue = "";
                            line = reader.ReadLine();
                            while (!line.StartsWith(boundary))
                            {
                                fieldValue += $"{line}\r\n";
                                line = reader.ReadLine();
                            }

                            fieldValue = fieldValue.TrimEnd('\r', '\n');
                            Form.AddOrUpdate(fieldName, fieldValue);
                        }
                    }
                }
                else
                {
                    line = reader.ReadLine();
                }
            }
        }
    }
}
