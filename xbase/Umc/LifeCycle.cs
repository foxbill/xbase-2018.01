using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace xbase.umc
{
    public enum LifeCycle
    {
        Global,
        Session,
        Request
    }

    public static class LifeCycleLabels
    {
        private static Dictionary<LifeCycle, string> labels = new Dictionary<LifeCycle, string>
        {
            {LifeCycle.Global,"全局"},
            {LifeCycle.Session,"会话"},
            {LifeCycle.Request,"请求"}
        };

        public static string get(LifeCycle index)
        {
            return labels[index];
        }
    }


}
