using SKKey.config;
using SKKey.ocx;
using SKKey.socket;
using SKKey.task;
using SKKey.utils;
using SKKey.websocket;
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

            if ("baiwang".Equals(controlVersion))
            {
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
            else
            {
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

        private void BTNTest_Click(object sender, EventArgs e)
        {
            int iResult = UsbclubOperator.USBShareUnit_Init();
            MessageBox.Show(String.Format("{0}", iResult));

            String USBID = "192.168.101.222:10001:3240";
            iResult = UsbclubOperator.OpenUSBPortByID(13, ref USBID);
            MessageBox.Show(String.Format("{0}", iResult));

            String sh = SH_textBox.Text;
            String pwd = null;
            String portInfo = null;

            XmlUtil.GetParamByTaxCode(sh, ref portInfo, ref pwd);
            var openInfo = UsbclubOperator.openPort(portInfo);
            if (!"0".Equals(openInfo["result"]))
            {
                log.Error("打开设备失败：" + openInfo);
            }

            var tsM = new TokenTask().getTocken("https://fpdk.cqsw.gov.cn/", "3.0.09", sh, pwd, "1");

            if (portInfo != null)
            {
                UsbclubOperator.closePort(portInfo);
            }
            MessageBox.Show(tsM.parameters["msg"]);
        }
    }
}