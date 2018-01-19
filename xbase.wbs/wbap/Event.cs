using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wbs.wbap
{
    class Event
    {
        private string eventName;

        public string EventName
        {
            get { return eventName; }
            set { eventName = value; }
        }
        private RequestEnv requestEnv = new RequestEnv();

        public RequestEnv RequestEnv
        {
            get { return requestEnv; }
        }
    }
}
