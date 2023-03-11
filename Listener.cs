using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FortniteLauncher
{
    public class Listener
    {
        private static int _port = 4211;
        public static void Start()
        {
            var listener = new HttpListener();
            listener.Prefixes.Add($"http://127.0.0.1:{_port}/");
            try
            {
                new TcpClient("127.0.0.1", _port);
                Console.WriteLine($" {_port} port is in use");
            }
            catch
            {
                listener.Start();
                Console.WriteLine($"Listening on port {_port}");
            }
            for (; ; )
            {
                var context = listener.GetContext();
                if (context.Request.Url.LocalPath == "/fortnite/api/cloudstorage/system")
                {
                    var engine = new
                    {
                        uniqueFilename = "DefaultEngine.ini",
                        filename = "DefaultEngine.ini",
                        hash = Hash.GetSha1(Cloudstorage.defaultEngine),
                        hash256 = Hash.GetSha256(Cloudstorage.defaultEngine),
                        length = File.ReadAllText(Cloudstorage.defaultEngine).Length,
                        contentType = "application/octet-stream",
                        uploaded = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'"),
                        storageType = "S3",
                        doNotCache = false
                    };

                    var game = new
                    {
                        uniqueFilename = "DefaultGame.ini",
                        filename = "DefaultGame.ini",
                        hash = Hash.GetSha1(Cloudstorage.defaultGame),
                        hash256 = Hash.GetSha256(Cloudstorage.defaultGame),
                        length = File.ReadAllText(Cloudstorage.defaultGame).Length,
                        contentType = "application/octet-stream",
                        uploaded = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'"),
                        storageType = "S3",
                        doNotCache = false
                    };

                    var input = new
                    {
                        uniqueFilename = "DefaultInput.ini",
                        filename = "DefaultInput.ini",
                        hash = Hash.GetSha1(Cloudstorage.defaultInput),
                        hash256 = Hash.GetSha256(Cloudstorage.defaultInput),
                        length = File.ReadAllText(Cloudstorage.defaultInput).Length,
                        contentType = "application/octet-stream",
                        uploaded = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'"),
                        storageType = "S3",
                        doNotCache = false
                    };

                    var runtimeOptions = new
                    {
                        uniqueFilename = "DefaultRuntimeOptions.ini",
                        filename = "DefaultRuntimeOptions.ini",
                        hash = Hash.GetSha1(Cloudstorage.defaultRuntimeOptions),
                        hash256 = Hash.GetSha256(Cloudstorage.defaultRuntimeOptions),
                        length = File.ReadAllText(Cloudstorage.defaultRuntimeOptions).Length,
                        contentType = "application/octet-stream",
                        uploaded = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'"),
                        storageType = "S3",
                        doNotCache = false
                    };

                    var data = JsonConvert.SerializeObject(new[]
                    {
                        engine,
                        game,
                        input,
                        runtimeOptions
                    });

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = 200;
                    context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data).Length;
                    context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data), 0, Encoding.UTF8.GetBytes(data).Length);
                }

                if (context.Request.Url.LocalPath == "/fortnite/api/cloudstorage/system/DefaultEngine.ini")
                {
                    var data = File.ReadAllBytes(Cloudstorage.defaultEngine);

                    context.Response.ContentType = "application/octet-stream";
                    context.Response.StatusCode = 200;
                    context.Response.ContentLength64 = data.Length;
                    context.Response.OutputStream.Write(data, 0, data.Length);
                }

                if (context.Request.Url.LocalPath == "/fortnite/api/cloudstorage/system/DefaultGame.ini")
                {
                    var data = File.ReadAllBytes(Cloudstorage.defaultGame);

                    context.Response.ContentType = "application/octet-stream";
                    context.Response.StatusCode = 200;
                    context.Response.ContentLength64 = data.Length;
                    context.Response.OutputStream.Write(data, 0, data.Length);
                }

                if (context.Request.Url.LocalPath == "/fortnite/api/cloudstorage/system/DefaultInput.ini")
                {
                    var data = File.ReadAllBytes(Cloudstorage.defaultInput);

                    context.Response.ContentType = "application/octet-stream";
                    context.Response.StatusCode = 200;
                    context.Response.ContentLength64 = data.Length;
                    context.Response.OutputStream.Write(data, 0, data.Length);
                }

                if (context.Request.Url.LocalPath == "/fortnite/api/cloudstorage/system/DefaultRuntimeOptions.ini")
                {
                    var data = File.ReadAllBytes(Cloudstorage.defaultRuntimeOptions);

                    context.Response.ContentType = "application/octet-stream";
                    context.Response.StatusCode = 200;
                    context.Response.ContentLength64 = data.Length;
                    context.Response.OutputStream.Write(data, 0, data.Length);
                }

                if (context.Request.Url.LocalPath == "/fortnite/api/cloudstorage/system/config")
                {

                    var data = JsonConvert.SerializeObject(new
                    {

                    });

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = 200;
                    context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data).Length;
                    context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data), 0, Encoding.UTF8.GetBytes(data).Length);
                }

                if (context.Request.Url.LocalPath.Contains("/fortnite/api/cloudstorage/user"))
                {

                    var data = JsonConvert.SerializeObject(new
                    {

                    });

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = 200;
                    context.Response.ContentLength64 = Encoding.UTF8.GetBytes(data).Length;
                    context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(data), 0, Encoding.UTF8.GetBytes(data).Length);
                }
            }
        }
    }
}
