using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wbs.wbap
{
    public class JsEvent
    {
        private string eventName;
        private WbapRequest requestEnv = new WbapRequest();

        public string EventName
        {
            get { return eventName; }
            set { eventName = value; }
        }

        public WbapRequest RequestEnv
        {
            get { return requestEnv; }
            set { requestEnv = value; }
        }

    }
}
