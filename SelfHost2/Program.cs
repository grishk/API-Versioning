using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.Owin.Hosting;

namespace SelfHost2 {
    internal class Program {
        private const string _Url = "http://localhost:9009/";
        private const string _LaunchUrl = _Url + "swagger";
        private static readonly ManualResetEvent _ResetEvent = new ManualResetEvent(false);

        private static void Main(string[] args) {
            Console.CancelKeyPress += OnCancel;

            using (WebApp.Start<Startup>(_Url)) {
                Console.WriteLine("Content root path: " + Startup.ContentRootPath);
                Console.WriteLine("Now listening on: " + _Url);
                Console.WriteLine("Application started. Press Ctrl+C to shut down.");
                Process.Start(_LaunchUrl);
                _ResetEvent.WaitOne();
            }

            Console.CancelKeyPress -= OnCancel;
        }

        private static void OnCancel(object sender, ConsoleCancelEventArgs e) {
            Console.Write("Application is shutting down...");
            e.Cancel = true;
            _ResetEvent.Set();
        }
    }
}