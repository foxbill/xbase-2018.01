using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace xbase.web
{
    public static class WboHandlerTools
    {
        public static Dictionary<string, string> getRequestPostNameValueParams(HttpRequest request)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            foreach (string name in request.Form.AllKeys)
            {
                string value = request.Form[name];
                ret.Add(name, value);

            }
            return ret;
        }
    }
}
