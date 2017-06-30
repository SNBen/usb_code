using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WebSocket4Net;
using System.Threading;
using SuperSocket.ClientEngine;

using Newtonsoft.Json;

using SKKey.http;

using SKKey.ocx;
using SKKey.websocket;
using SKKey.task;
using SKKey.config;
using System.Diagnostics;

namespace SKKey.form
{
    public partial class MainForm : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            TrayHelper trayHelper = new TrayHelper(this);
            WebOcxAccess.init(this.axCryptCtl1);

            String taskServerIP = ConfigManager.Instance.Config.taskServerIP;

            String taskServerPort = ConfigManager.Instance.Config.taskServerPort;

            if (taskServerIP != null)
            {
                taskServerIPBox.Text = taskServerIP;
            }

            if (taskServerPort != null)
            {
                taskServerPortBox.Text = taskServerPort;
            }

            String controlVersion = ConfigManager.Instance.Config.controlVersion;

            if ("baiwang".Equals(controlVersion)) {
                baiwangRadioButton.Checked = true;
            }
            TokenTask.tockenTaskRequestThreadInit();
        }

        private void changeShButton_Click(object sender, EventArgs e)
        {
            TokenTask.errorLicense = false;
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            onClose();

            System.Environment.Exit(0);
        }

        public void onClose()
        {

            Process[] procs = Process.GetProcessesByName("SKKeyWatch");

            foreach (Process proc in procs)
            {
                try
                {
                    proc.Kill();
                }
                catch (Exception exp) { }
            }
           
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            String taskServerIP = taskServerIPBox.Text;

            String taskServerPort = taskServerPortBox.Text;



            if (taskServerIP == null || taskServerIP.Trim().Length == 0)
            {
                MessageBox.Show("任务服务器IP不能为空！");
                return;
            }

            if (taskServerPort == null || taskServerPort.Trim().Length == 0)
            {
                MessageBox.Show("任务服务器地址不能为空！");
                return;
            }

            Dictionary<string, string> map = new Dictionary<string, string>();

            map["taskServerIP"] = taskServerIP;
            map["taskServerPort"] = taskServerPort;

            if (baiwangRadioButton.Checked)
            {
                map["controlVersion"] = "baiwang";
            }
            else {
                map["controlVersion"] = "hangxin";
            }
            ConfigManager.writeJSONConfigAndInit(map);
            TaskWebsocketClient.init();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }


    }
}
