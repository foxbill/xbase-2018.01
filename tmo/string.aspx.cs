using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace tmo
{
    public partial class _string : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write(string.Format("{0:yyyy年MM月dd日}",DateTime.Parse("2002/3/5")));
        }
    }
}