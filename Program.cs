using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace FortniteLauncher
{
    internal class Program
    {
        static string fltoken = "5360b1637173878a5a9a4938";
        static string caldera = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhY2NvdW50X2lkIjoiIiwiZ2VuZXJhdGVkIjoxNjY5MjY2MzA5LCJjYWxkZXJhR3VpZCI6Ijk4OTJkNzA4LWE5ODItNDczYy04MzBiLTYyYzU2MDBjYTFkMyIsImFjUHJvdmlkZXIiOiJFYXN5QW50aUNoZWF0Iiwibm90ZXMiOiIiLCJmYWxsYmFjayI6dHJ1ZX0.G6MZNH54MvImdJ_kcYkKJShJXjGZSP58aACy7pUYac8";

        [STAThread]
        static void Main(string[] args)
        {
            Console.Title = "CloudstorageEditor";
            Console.WriteLine("FortniteLauncher made by BiruFN#0746");

            Console.WriteLine("Please choose game path");
            var cofd = new CommonOpenFileDialog();
            cofd.IsFolderPicker = true;
            if (cofd.ShowDialog() != CommonFileDialogResult.Ok)
            {
                Environment.Exit(0);
            }
            var gamePath = cofd.FileName;

            Console.WriteLine("Please authorization epic account");
            var deviceToken = OAuth.GetDeviceToken();
            var deviceCode = OAuth.GetDeviceCode(deviceToken);
            var accessToken = OAuth.GetAccessToken(deviceCode);
            var exchangeCode = OAuth.GetExchangeCode(accessToken);

            new Thread(new ThreadStart(Listener.Start)).Start();

            var arguments = $"-AUTH_LOGIN=unused -AUTH_PASSWORD={exchangeCode} -AUTH_TYPE=exchangecode -epicapp=Fortnite -epicenv=Prod -EpicPortal -skippatchcheck -nobe -fromfl=eac -fltoken={fltoken} -caldera={caldera}";

            var launcher = new Process
            {
                StartInfo =
                {
                    FileName = $@"{gamePath}\FortniteGame\Binaries\Win64\FortniteLauncher.exe",
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow= true
                }
            };
            launcher.Start();
            foreach (ProcessThread thread in launcher.Threads)
            {
                SuspendThread(OpenThread(0x0002, false, thread.Id));
            }

            var shippingEac = new Process
            {
                StartInfo =
                {
                    FileName = $@"{gamePath}\FortniteGame\Binaries\Win64\FortniteClient-Win64-Shipping_EAC.exe",
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow= true
                }
            };
            shippingEac.Start();
            foreach (ProcessThread thread in shippingEac.Threads)
            {
                SuspendThread(OpenThread(0x0002, false, thread.Id));
            }

            var shipping = new Process
            {
                StartInfo =
                {
                    FileName = $@"{gamePath}\FortniteGame\Binaries\Win64\FortniteClient-Win64-Shipping.exe",
                    Arguments = arguments,
                    WorkingDirectory = $@"{gamePath}\FortniteGame\Binaries\Win64"
                }
            };
            shipping.Start();
            
            shipping.WaitForInputIdle();
            Injector.Inject(shipping.Id, $@"{Directory.GetCurrentDirectory()}\Cloudstorage.dll");

            shipping.WaitForExit();
            shippingEac.Kill();
            launcher.Kill();

            Environment.Exit(0);
        }

        [DllImport("kernel32.dll")]
        public static extern Int32 SuspendThread(IntPtr hThread);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenThread(int dwDesiredAccess, bool bInheritHandle, int dwThreadId);
    }
}
