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
            this.changeShButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.exitButton = new System.Windows.Forms.Button();
            this.passwordBox2 = new System.Windows.Forms.TextBox();
            this.授权码 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.usedShLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.shBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.axCryptCtl1 = new AxCryp_Ctl.AxCryptCtl();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.taskServerIPBox = new System.Windows.Forms.TextBox();
            this.taskServerPortBox = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axCryptCtl1)).BeginInit();
            this.SuspendLayout();
            // 
            // changeShButton
            // 
            this.changeShButton.Location = new System.Drawing.Point(113, 354);
            this.changeShButton.Name = "changeShButton";
            this.changeShButton.Size = new System.Drawing.Size(75, 21);
            this.changeShButton.TabIndex = 1;
            this.changeShButton.Text = "确认";
            this.changeShButton.UseVisualStyleBackColor = true;
            this.changeShButton.Click += new System.EventHandler(this.changeShButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.taskServerPortBox);
            this.groupBox1.Controls.Add(this.taskServerIPBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.exitButton);
            this.groupBox1.Controls.Add(this.passwordBox2);
            this.groupBox1.Controls.Add(this.授权码);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.usedShLabel);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.passwordBox);
            this.groupBox1.Controls.Add(this.shBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.changeShButton);
            this.groupBox1.Location = new System.Drawing.Point(3, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(487, 470);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "非机柜版";
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(303, 354);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(75, 21);
            this.exitButton.TabIndex = 14;
            this.exitButton.Text = "安全退出";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // passwordBox2
            // 
            this.passwordBox2.Location = new System.Drawing.Point(113, 170);
            this.passwordBox2.Name = "passwordBox2";
            this.passwordBox2.PasswordChar = '*';
            this.passwordBox2.Size = new System.Drawing.Size(348, 21);
            this.passwordBox2.TabIndex = 13;
            // 
            // 授权码
            // 
            this.授权码.AutoSize = true;
            this.授权码.Location = new System.Drawing.Point(32, 170);
            this.授权码.Name = "授权码";
            this.授权码.Size = new System.Drawing.Size(53, 12);
            this.授权码.TabIndex = 12;
            this.授权码.Text = "确认密码";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(22, 32);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "正在使用税号";
            // 
            // usedShLabel
            // 
            this.usedShLabel.AutoSize = true;
            this.usedShLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.usedShLabel.Location = new System.Drawing.Point(111, 32);
            this.usedShLabel.Name = "usedShLabel";
            this.usedShLabel.Size = new System.Drawing.Size(0, 12);
            this.usedShLabel.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "密码";
            // 
            // passwordBox
            // 
            this.passwordBox.Location = new System.Drawing.Point(113, 113);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.PasswordChar = '*';
            this.passwordBox.Size = new System.Drawing.Size(348, 21);
            this.passwordBox.TabIndex = 8;
            // 
            // shBox
            // 
            this.shBox.Location = new System.Drawing.Point(113, 67);
            this.shBox.Name = "shBox";
            this.shBox.Size = new System.Drawing.Size(348, 21);
            this.shBox.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "税号";
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 234);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 15;
            this.label1.Text = "任务服务器IP";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 287);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 16;
            this.label4.Text = "任务服务器端口";
            // 
            // taskServerIPBox
            // 
            this.taskServerIPBox.Location = new System.Drawing.Point(113, 234);
            this.taskServerIPBox.Name = "taskServerIPBox";
            this.taskServerIPBox.Size = new System.Drawing.Size(348, 21);
            this.taskServerIPBox.TabIndex = 17;
            // 
            // taskServerPortBox
            // 
            this.taskServerPortBox.Location = new System.Drawing.Point(113, 287);
            this.taskServerPortBox.Name = "taskServerPortBox";
            this.taskServerPortBox.Size = new System.Drawing.Size(348, 21);
            this.taskServerPortBox.TabIndex = 18;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 494);
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
        private System.Windows.Forms.Button changeShButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox shBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox passwordBox;
        private Label label5;
        private Label usedShLabel;
        private Label 授权码;
        private TextBox passwordBox2;
        private Button exitButton;
        private TextBox taskServerPortBox;
        private TextBox taskServerIPBox;
        private Label label4;
        private Label label1;
    }
}

