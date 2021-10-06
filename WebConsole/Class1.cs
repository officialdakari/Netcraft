using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NCore;
using NCore.netcraft.server.api;

namespace WebConsole
{
    public class Plugin : NCore.Plugin
    {
        public override NCore.Plugin Create()
        {
            SetOptions("WebConsole", "Netcraft server console in your browser", "1.0", new string[] { "DarkCoder15" });
            return this;
        }

        public override string OnLoad()
        {
            Thread th = new Thread(() =>
            {
                GetLogger().Info("Web server listening at http://localhost:5000");
                SimpleListenerExample(new string[] { "http://*:5000/" });
            });
            th.Start();
            Threads.Add(th);
            NCSApi.ConsoleLogEvent += (e) =>
            {
                switch(e.GetLevel())
                {
                    case "INFO":
                        html += $"<p>[{DateTime.Now.ToString()} {e.GetLevel()}]: {e.GetLine()}</p><br>\n";
                        break;
                    case "WARNING":
                        html += $"<p><font color=\"#d8a118\">[{DateTime.Now.ToString()} {e.GetLevel()}]: {e.GetLine()}</font></p><br>\n";
                        break;
                    case "ERROR":
                        html += $"<p><font color=\"red\">[{DateTime.Now.ToString()} {e.GetLevel()}]: {e.GetLine()}</font></p><br>\n";
                        break;
                    case "SEVERE":
                        html += $"<p>[{DateTime.Now.ToString()} {e.GetLevel()}]: {e.GetLine()}</p><br>\n";
                        break;
                }
            };
            return null;
        }
        string html = "";

        public override void OnUnload()
        {

        }

        private byte[] HttpDataReceived(ref HttpListenerResponse res, string data, Uri uri, NameValueCollection headers, string method, string contentType, Encoding contentEncoding)
        {
            string path = uri.AbsolutePath;
            string result = "";
            if(path == "/log")
            {
                result = $"<!DOCTYPE html>\n<html>\n<head><title>Log</title></head>\n<body>\n{html}\n</body>\n</html>";
            }
            return contentEncoding.GetBytes(result);
        }

        private void SimpleListenerExample(string[] prefixes)
        {
            if (!HttpListener.IsSupported)
            {
                GetLogger().Warning("Windows XP SP2 / Windows Server 2003 or higher is required to use the HttpListener class.");
                return;
            }
            // URI prefixes are required,
            // for example "http://contoso.com:8080/index/".
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            // Create a listener.
            HttpListener listener = new HttpListener();
            // Add the prefixes.
            foreach (string s in prefixes)
            {
                listener.Prefixes.Add(s);
            }
            listener.Start();
            // Note: The GetContext method blocks while waiting for a request.
            while (true)
            {
                try
                {
                    HttpListenerContext context = listener.GetContext();

                    HttpListenerRequest request = context.Request;
                    //Console.Write(request.HttpMethod + " ");
                    //if (request.HttpMethod != "POST") return;
                    // Obtain a response object.
                    HttpListenerResponse response = context.Response;
                    // Construct a response.
                    string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                    // Get a response stream and write the response to it.
                    //StreamReader sr = new StreamReader(request.InputStream, request.ContentEncoding);
                    //string data = sr.ReadToEnd();
                    //Console.WriteLine(data);
                    //Dictionary<string, string> d = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);

                    buffer = HttpDataReceived(ref response, request.ContentEncoding.GetString(ReadFully(request.InputStream)), request.Url, request.Headers, request.HttpMethod, request.ContentType, request.ContentEncoding);
                    response.ContentLength64 = buffer.Length;
                    System.IO.Stream output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    // You must close the output stream.
                    output.Close();
                }
                catch (Exception ex)
                {
                    if (!(ex is IndexOutOfRangeException))
                        Console.WriteLine(ex.GetType().ToString() + ": " + ex.Message);
                }
                //listener.Stop();
            }
        }

        byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}