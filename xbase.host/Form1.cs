using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace xbase.host
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGetRegisterString_Click(object sender, EventArgs e)
        {
            txtRegistCode.Text = RegMachine.getRegistorCode();
        }

        private void btnTestAccreditString_Click(object sender, EventArgs e)
        {
            UserInfo ui = RegMachine.writeAccredit(txtAccreditCode.Text.Trim());
            if (ui == null)
                txtMsg.Text = "非法授权字符串，授权失败";
            else
                txtMsg.Text = "授权成功\n用户名称：" + ui.userName;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtRegistCode.Text = RegMachine.getRegistorCode();
        }


    }
}
