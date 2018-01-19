using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wbs.wbap
{
    /// <summary>
    /// aaa
    /// </summary>
    public class WbapResponse
    {
        private const string protocol = "wbap";
        private const string version = "1.0.0.0";
        private int errorNo = 0;
        private string message;
        private string onErrUrl;
        private string pageName;

        private WbapDataBody elementBinds = new WbapDataBody();
        private Dictionary<string, WbapControl> controls = new Dictionary<string, WbapControl>();

        private WbapAction action = new WbapAction();


        /// <summary>
        /// 
        /// </summary>
        public string PageName
        {
            get { return pageName; }
            set { pageName = value; }
        }


        public string OnErrUrl
        {
            get { return onErrUrl; }
            set { onErrUrl = value; }
        }

        public WbapAction Action
        {
            get { return action; }
            set { action = value; }
        }

        public int ErrorNo
        {
            get { return errorNo; }
            set { errorNo = value; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public WbapDataBody ElementBinds
        {
            get { return elementBinds; }
            set { elementBinds = value; }
        }

        public Dictionary<string, WbapControl> Controls
        {
            get { return controls; }
            set { controls = value; }
        }
    }
}
