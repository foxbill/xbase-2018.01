using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace xbase.web
{
    public static class HandlerUtils
    {

        internal static Dictionary<string, string> getPostParams(HttpContext context)
        {
            HttpRequest request = context.Request;
            Dictionary<string, string> ret = new Dictionary<string, string>();

            foreach (string name in request.Form.Keys)
            {
                string value = request.Form[name];
                if ("null".Equals(value, StringComparison.OrdinalIgnoreCase))
                    value = null;
                ret.Add(name, value);

            }
            return ret;

        }

        internal static Dictionary<string, string> getQueryParams(HttpContext context)
        {

            HttpRequest request = context.Request;

            if (request.QueryString == null || request.QueryString.Count < 1)
                return null;
            Dictionary<string, string> ret = new Dictionary<string, string>();

            foreach (string key in request.QueryString.Keys)
            {
                ret.Add(key, request.QueryString[key]);
            }

            return ret;
        }
    }
}
