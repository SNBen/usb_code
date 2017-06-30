using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
namespace SKKeyWatch
{
    class Program
    {
        static void Main(string[] args)
        {
            new Thread(watch).Start();
        }

        private static void watch()
        {
            while (true)
            {
                try
                {
                    doWatch();
                }
                catch (Exception e) { }
                Thread.Sleep(1000);
            }
        }

        private static void doWatch()
        {
            Process[] procs = Process.GetProcessesByName("SKKey");
            if (procs.Length == 0)
            {

                System.Diagnostics.ProcessStartInfo Info = new System.Diagnostics.ProcessStartInfo();
                Info.FileName = "SKKey.exe";
                Info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                Info.WorkingDirectory = @"C:\baiwang\百望进项客户端\SKKey";
                Info.CreateNoWindow = false;
                Info.WindowStyle = ProcessWindowStyle.Normal;

                Process.Start(Info);
             

                

            }

            
        }
    }
}
