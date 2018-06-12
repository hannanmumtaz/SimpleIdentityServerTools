using System;

namespace SimpleIdentityServer.OpenId.Modularized.Startup
{
    public class Program
    {
        private static bool keepRunning = true;

        public static void Main(string[] args)
        {
            WebHost.Start();
            Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e) {
                e.Cancel = true;
                Program.keepRunning = false;
            };
            while (Program.keepRunning) { }
            Console.WriteLine("exited gracefully");
        }
    }
}
