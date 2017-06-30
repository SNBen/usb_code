using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using SKKey.form;
using SKKey.config;
using System.Diagnostics;
namespace SKKey
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool bCreatedNew;
            Mutex m = new Mutex(false, Application.ProductName, out bCreatedNew);
            if (!bCreatedNew)
            {
                MessageBox.Show("此程序已运行！");
                return;
            }
            Config config = ConfigManager.Instance.Config;
           
            Process[] proc = Process.GetProcessesByName("SKKeyWatch");
            if (proc.Length == 0)
            {
                System.Diagnostics.ProcessStartInfo Info = new System.Diagnostics.ProcessStartInfo();
                Info.FileName = "SKKeyWatch.exe";
                Info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                Info.WorkingDirectory = @"C:\baiwang\百望进项客户端\SKKey";
                Info.CreateNoWindow = true;
                try
                {
                    System.Diagnostics.Process.Start(Info);
                }
                catch (System.ComponentModel.Win32Exception)
                {

                }
            }
             
            Application.Run(new MainForm());

            
        }
    }
}
