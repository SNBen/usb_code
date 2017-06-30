using System;
using System.Drawing;
using System.Windows.Forms;

namespace SKKey.form
{
    class TrayHelper
    {
        #region
        //创建NotifyIcon对象 
        NotifyIcon notifyIcon = new NotifyIcon();
        //创建托盘图标对象 
        Icon ico;
        //创建托盘菜单对象 
        ContextMenu notifyContextMenu = new ContextMenu();

        Form linkedForm;
        #endregion

        public TrayHelper(Form form)
        {
            linkedForm = form;
            linkedForm.SizeChanged += Form_SizeChanged;
            ico = form.Icon;
            notifyIcon.DoubleClick += notifyIcon_DoubleClick;
        }

        private void Form_SizeChanged(object sender, EventArgs e)
        {
            //判断是否选择的是最小化按钮 
            if (linkedForm.WindowState == FormWindowState.Minimized)
            {
                //托盘显示图标等于托盘图标对象 
                //注意notifyIcon1是控件的名字而不是对象的名字 
                notifyIcon.Icon = ico;
                //隐藏任务栏区图标 
                linkedForm.ShowInTaskbar = false;
                //图标显示在托盘区 
                notifyIcon.Visible = true;
            }
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            //判断是否已经最小化于托盘 
            if (linkedForm.WindowState == FormWindowState.Minimized)
            {
                //还原窗体显示 
                linkedForm.WindowState = FormWindowState.Normal;
                //激活窗体并给予它焦点 
                linkedForm.Activate();
                //任务栏区显示图标 
                linkedForm.ShowInTaskbar = true;
                //托盘区图标隐藏 
                notifyIcon.Visible = false;
            }
        }
    }
}
