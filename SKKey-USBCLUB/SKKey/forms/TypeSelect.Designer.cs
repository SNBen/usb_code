namespace SKKey
{
    partial class TypeSelect
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BTN_S = new System.Windows.Forms.Button();
            this.PC = new System.Windows.Forms.RadioButton();
            this.USB = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // BTN_S
            // 
            this.BTN_S.Location = new System.Drawing.Point(160, 151);
            this.BTN_S.Name = "BTN_S";
            this.BTN_S.Size = new System.Drawing.Size(88, 23);
            this.BTN_S.TabIndex = 0;
            this.BTN_S.Text = "确定";
            this.BTN_S.UseVisualStyleBackColor = true;
            this.BTN_S.Click += new System.EventHandler(this.BTN_S_Click);
            // 
            // PC
            // 
            this.PC.AutoSize = true;
            this.PC.Location = new System.Drawing.Point(34, 66);
            this.PC.Name = "PC";
            this.PC.Size = new System.Drawing.Size(59, 16);
            this.PC.TabIndex = 2;
            this.PC.TabStop = true;
            this.PC.Text = "单机版";
            this.PC.UseVisualStyleBackColor = true;
            // 
            // USB
            // 
            this.USB.AutoSize = true;
            this.USB.Location = new System.Drawing.Point(298, 66);
            this.USB.Name = "USB";
            this.USB.Size = new System.Drawing.Size(59, 16);
            this.USB.TabIndex = 3;
            this.USB.TabStop = true;
            this.USB.Text = "机柜版";
            this.USB.UseVisualStyleBackColor = true;
            // 
            // TypeSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 261);
            this.Controls.Add(this.USB);
            this.Controls.Add(this.PC);
            this.Controls.Add(this.BTN_S);
            this.Name = "TypeSelect";
            this.Text = "模式选择";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BTN_S;
        private System.Windows.Forms.RadioButton PC;
        private System.Windows.Forms.RadioButton USB;
    }
}