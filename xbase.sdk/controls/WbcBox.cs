using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.umc;
using xbase.umc.attributes;

namespace xbase.sdk.controls
{
    [WboAttr(LifeCycle = LifeCycle.Global, Title = "空间工具箱")]
    public class WbcBox : IVisualWbo
    {
        #region IVisualWbo 成员

        string IVisualWbo.Render(string elementName)
        {
            WboAdmin wa = new WboAdmin();
            List<Wbo> oss = WboAdmin.GetVboTypes();
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<div class=""VboBox"">");


            for (int i = 0; i < oss.Count; i++)
            {
                Wbo os = oss[i];
                sb.Append("<div class=\"VboButton\" style=float:left>");
                sb.Append(os.title);
                sb.Append("</div>");
            }

            sb.Append("</div>");

            return sb.ToString();
        }

        #endregion


        public string Render()
        {
            throw new NotImplementedException();
        }
    }
}
