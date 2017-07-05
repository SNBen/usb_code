using SKKey.ocx;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace SKKey.form
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.hangxinRadioButton = new System.Windows.Forms.RadioButton();
            this.baiwangRadioButton = new System.Windows.Forms.RadioButton();
            this.taskServerPortBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.taskServerIPBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.submitButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.axCryptCtl1 = new AxCryp_Ctl.AxCryptCtl();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axCryptCtl1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.hangxinRadioButton);
            this.groupBox1.Controls.Add(this.baiwangRadioButton);
            this.groupBox1.Controls.Add(this.taskServerPortBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.taskServerIPBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.submitButton);
            this.groupBox1.Controls.Add(this.exitButton);
            this.groupBox1.Location = new System.Drawing.Point(3, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(489, 299);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "机柜版";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(50, 198);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 22;
            this.label3.Text = "控件版本";
            this.label3.Visible = false;
            // 
            // hangxinRadioButton
            // 
            this.hangxinRadioButton.AutoSize = true;
            this.hangxinRadioButton.Location = new System.Drawing.Point(301, 198);
            this.hangxinRadioButton.Name = "hangxinRadioButton";
            this.hangxinRadioButton.Size = new System.Drawing.Size(59, 16);
            this.hangxinRadioButton.TabIndex = 21;
            this.hangxinRadioButton.TabStop = true;
            this.hangxinRadioButton.Text = "航信版";
            this.hangxinRadioButton.UseVisualStyleBackColor = true;
            this.hangxinRadioButton.Visible = false;
            // 
            // baiwangRadioButton
            // 
            this.baiwangRadioButton.AutoSize = true;
            this.baiwangRadioButton.Location = new System.Drawing.Point(153, 198);
            this.baiwangRadioButton.Name = "baiwangRadioButton";
            this.baiwangRadioButton.Size = new System.Drawing.Size(59, 16);
            this.baiwangRadioButton.TabIndex = 20;
            this.baiwangRadioButton.TabStop = true;
            this.baiwangRadioButton.Text = "百望版";
            this.baiwangRadioButton.UseVisualStyleBackColor = true;
            this.baiwangRadioButton.Visible = false;
            this.baiwangRadioButton.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // taskServerPortBox
            // 
            this.taskServerPortBox.Location = new System.Drawing.Point(153, 135);
            this.taskServerPortBox.Name = "taskServerPortBox";
            this.taskServerPortBox.Size = new System.Drawing.Size(318, 21);
            this.taskServerPortBox.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 138);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 18;
            this.label2.Text = "任务服务器端口";
            // 
            // taskServerIPBox
            // 
            this.taskServerIPBox.Location = new System.Drawing.Point(153, 68);
            this.taskServerIPBox.Name = "taskServerIPBox";
            this.taskServerIPBox.Size = new System.Drawing.Size(318, 21);
            this.taskServerIPBox.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 16;
            this.label1.Text = "任务服务器IP";
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(51, 256);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(75, 21);
            this.submitButton.TabIndex = 15;
            this.submitButton.Text = "确认";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(170, 256);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(75, 21);
            this.exitButton.TabIndex = 14;
            this.exitButton.Text = "安全退出";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // axCryptCtl1
            // 
            this.axCryptCtl1.Enabled = true;
            this.axCryptCtl1.Location = new System.Drawing.Point(3, 12);
            this.axCryptCtl1.Name = "axCryptCtl1";
            this.axCryptCtl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axCryptCtl1.OcxState")));
            this.axCryptCtl1.Size = new System.Drawing.Size(33, 29);
            this.axCryptCtl1.TabIndex = 0;
            this.axCryptCtl1.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 314);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.axCryptCtl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "任务客户端";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axCryptCtl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxCryp_Ctl.AxCryptCtl axCryptCtl1;
        private System.Windows.Forms.GroupBox groupBox1;
        private Button exitButton;
        private Button submitButton;
        private Label label1;
        private TextBox taskServerIPBox;
        private Label label2;
        private TextBox taskServerPortBox;
        private RadioButton baiwangRadioButton;
        private RadioButton hangxinRadioButton;
        private Label label3;
    }
}

