﻿using SKKey.config;
using SKKey.ocx;
using SKKey.socket;
using SKKey.task;
using SKKey.utils;
using SKKey.websocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Reflection;

using log4net;

namespace SKKey.form
{
    public partial class MainForm : Form
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            TrayHelper trayHelper = new TrayHelper(this);
            WebOcxAccess.init(this.axCryptCtl1);

            if(ConfigManager.Instance.Config.license == null)
            {
                ConfigManager.Instance.Config.license = Guid.NewGuid().ToString("B");
            }

            

            String sh = ConfigManager.Instance.Config.sh;
            String password = ConfigManager.Instance.Config.password;
            String license = ConfigManager.Instance.Config.license;
            String PT_PWD = ConfigManager.Instance.Config.PT_PWD;

            if (sh != null)
            {
                usedShLabel.Text = sh;
                shBox.Text = sh;
            }

            if (password != null)
            {
                passwordBox.Text = password;
                passwordBox2.Text = password;
            }

            if (PT_PWD != null)
            {
                PT_PWD_textBox.Text = PT_PWD;
                PT_PWD_textBox2.Text = PT_PWD;
            }


            if (ConfigManager.Instance.Config.taskServerIP != null)
            {
                taskServerIPBox.Text = ConfigManager.Instance.Config.taskServerIP;
            }

            if (ConfigManager.Instance.Config.taskServerPort != null)
            {
                taskServerPortBox.Text = ConfigManager.Instance.Config.taskServerPort;
            }

            if (   ConfigManager.Instance.Config.sh != null
                && ConfigManager.Instance.Config.PT_PWD != null
                && ConfigManager.Instance.Config.password != null
                && ConfigManager.Instance.Config.taskServerIP != null
                && ConfigManager.Instance.Config.taskServerPort != null)
            {
                TokenTask.tockenTaskRequestThreadInit();
            }
        }

        private void changeShButton_Click(object sender, EventArgs e)
        {
            String sh = shBox.Text;

            String password = passwordBox.Text;
            String password2 = passwordBox2.Text;
            String StrIP = taskServerIPBox.Text;
            String StrPort = taskServerPortBox.Text;

            String PT_PWD   = PT_PWD_textBox.Text;
            String PT_PWD2  = PT_PWD_textBox2.Text;

            if (StrIP == null || StrIP.Trim().Length == 0)
            {
                MessageBox.Show("服务器IP地址 不能为空！");
                return;
            }

            if (StrPort == null || StrPort.Trim().Length == 0)
            {
                MessageBox.Show("服务器端口不能为空！");
                return;
            }

            if (sh == null || sh.Trim().Length == 0)
            {
                MessageBox.Show("税号不能为空！");
                return;
            }

            if (password == null || password.Trim().Length == 0)
            {
                MessageBox.Show("密码不能为空！");
                return;
            }

            if (PT_PWD == null || PT_PWD.Trim().Length == 0)
            {
                MessageBox.Show("平台密码不能为空！");
                return;
            }

            if (!PT_PWD.Equals(PT_PWD2))
            {
                MessageBox.Show("两次密码输入不一致！");
                return;
            }


            Dictionary<string, string> map = new Dictionary<string, string>();
            map["license"] = ConfigManager.Instance.Config.license;
            map["sh"] = sh;
            map["password"] = password;
            map["PT_PWD"] = PT_PWD;
            map["taskServerIP"] = StrIP;
            map["taskServerPort"] = StrPort;

            ConfigManager.writeJSONConfigAndInit(map);
            TokenTask.errorPassword = false;
            TokenTask.errorLicense = false;
            usedShLabel.Text = sh;

            new TokenTask().InitDevice();
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
                catch (Exception) { }
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}