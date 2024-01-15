using System.IO;
using System.Net;
using System.Text;
using System.Collections;
using System.Diagnostics;
using MakoIoT.Device.LocalConfiguration.Extensions;
using System;
using System.Web;

namespace MakoIoT.Device.LocalConfiguration.Controllers
{
    public abstract class ControllerBase
    {
        public delegate string FileUploadDelegate(string fieldName, string fileName, StreamReader contentsReader, string boundary);

        protected readonly string BaseFile;
        protected Hashtable HtmlParams;
        protected readonly Hashtable Form = new();

        private Hashtable _paramsInstances;
        private int _sourceLength = 0;

        protected ControllerBase(string baseFile)
        {
            BaseFile = $"I:\\{baseFile}";
            ParseParams();
        }

        protected void Render(HttpListenerResponse response, bool copyFormToParams = false)
        {
            if (copyFormToParams)
            {
                foreach (var formKey in Form.Keys)
                {
                    HtmlParams.AddOrUpdate(formKey, Form[formKey]);
                }
            }

            //compute response length
            var totalLength = _sourceLength;
            foreach (string key in _paramsInstances.Keys)
            {
                totalLength += (int)_paramsInstances[key] * ((string)HtmlParams[key]).Length;
            }

            response.ContentType = "text/html; charset=utf-8";
            response.StatusCode = (int)HttpStatusCode.OK;
            response.ContentLength64 = totalLength;

            using var reader = new StreamReader(File.OpenRead(BaseFile));
            var writer = new StreamWriter(response.OutputStream);

            int transferredLength = 0;

            string line = reader.ReadLine();
            while (line != null)
            {
                line = ReplaceParams(line);

                transferredLength += line.Length;

                writer.Write(line);
                line = reader.ReadLine();
            }

            Debug.WriteLine($"totalLength={totalLength}, transferredLength={transferredLength}");

            writer.Flush();
            reader.Close();
        }

        protected void ParseParams()
        {
            HtmlParams = new Hashtable();
            _paramsInstances = new Hashtable();

            using var reader = new StreamReader(File.OpenRead(BaseFile));
            string line = reader.ReadLine();
            
            while (line != null)
            {
                _sourceLength += line.Length;

                if (line.IndexOf('{') > -1)
                {
                    var sp = line.Split('{', '}');
                    for (int i = 1; i < sp.Length; i += 2)
                    {
                        _sourceLength -= sp[i].Length + 2;
                        AddParam(sp[i]);
                    }
                }

                line = reader.ReadLine();
            }
            reader.Close();

        }

        private void AddParam(string key)
        {
            if (HtmlParams.Contains(key))
            {
                _paramsInstances[key] = (int)_paramsInstances[key] + 1;
            }
            else
            {
                HtmlParams.Add(key, string.Empty);
                _paramsInstances.Add(key, 1);
            }
        }

        private string ReplaceParams(string s)
        {
            if (s.IndexOf('{') == -1)
                return s;

            var builder = new StringBuilder();
            var sp = s.Split('{', '}');
            for (int i = 0; i < sp.Length; i++)
            {
                builder.Append(i % 2 == 0 ? sp[i] : (string)HtmlParams[sp[i]]);
            }

            return builder.ToString();
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
