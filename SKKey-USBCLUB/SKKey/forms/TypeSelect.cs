using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SKKey
{
    public partial class TypeSelect : Form
    {
        public TypeSelect()
        {
            InitializeComponent();
        }

        private void BTN_S_Click(object sender, EventArgs e)
        {
            if(PC.Checked)
            {
                DialogResult = System.Windows.Forms.DialogResult.None;
            }
            else if(USB.Checked)
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            
        }
    }
}
