using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using Microsoft.Owin.Hosting;

namespace SelfHost
{
    class Program
    {
        const string Url = "http://localhost:9007/";
        const string LaunchUrl = Url + "swagger";
        static readonly ManualResetEvent resetEvent = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            Console.CancelKeyPress += OnCancel;

            using (WebApp.Start<Startup>(Url))
            {
                Console.WriteLine("Content root path: " + Startup.ContentRootPath);
                Console.WriteLine("Now listening on: " + Url);
                Console.WriteLine("Application started. Press Ctrl+C to shut down.");
                Process.Start(LaunchUrl);
                resetEvent.WaitOne();
            }

            Console.CancelKeyPress -= OnCancel;
        }

        static void OnCancel(object sender, ConsoleCancelEventArgs e)
        {
            Console.Write("Application is shutting down...");
            e.Cancel = true;
            resetEvent.Set();
        }
    }
}
