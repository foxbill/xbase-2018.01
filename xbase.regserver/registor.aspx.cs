using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace xbase.regserver
{
    public partial class registor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string s = Code.Decode(txtRegCode.Text);
                UserInfo ui = JsonConvert.DeserializeObject<UserInfo>(s);
                ui.userName = txtUserName.Text.Trim();
                ui.accreditTime = DateTime.Now;
                ui.accreditLevel = 100;//1;  
                ui.userCount = 5;
                s = JsonConvert.SerializeObject(ui);
                txtAccCode.Text = Code.Encode(s);
                labMemo.Text = "授权成功，请将授权码中的信息复制到，系统注册页面并提交！";
            }
            catch
            {
                labMemo.Style.Add(HtmlTextWriterStyle.Color, "red");
                labMemo.Text = "注册码错误，请重新获取";
            }
        }
    }
}