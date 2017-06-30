using SKKey.config;
using SKKey.ocx;
using SKKey.task;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

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

            String sh = ConfigManager.Instance.Config.sh;

            String password = ConfigManager.Instance.Config.password;

            String license = ConfigManager.Instance.Config.license;

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

            if (license != null)
            {
                sqmBox.Text = license;
            }

            TokenTask.tockenTaskRequestThreadInit();
        }

        private void changeShButton_Click(object sender, EventArgs e)
        {
            String license = sqmBox.Text;

            String sh = shBox.Text;

            String password = passwordBox.Text;

            String password2 = passwordBox2.Text;

            if (license == null || license.Trim().Length == 0)
            {
                MessageBox.Show("授权码不能为空！");
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

            if (!password.Equals(password2))
            {
                MessageBox.Show("两次密码输入不一致！");
                return;
            }

            Dictionary<string, string> map = new Dictionary<string, string>();

            map["license"] = license;

            map["sh"] = sh;

            map["password"] = password;

            ConfigManager.writeJSONConfigAndInit(map);
            TokenTask.errorPassword = false;
            TokenTask.errorLicense = false;
            usedShLabel.Text = sh;
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
    }
}