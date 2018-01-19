using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wbs.wbap
{
    public class WbapEvent
    {
        private string eventName;
        private WbapAction action = new WbapAction();

        public string EventName
        {
            get { return eventName; }
            set { eventName = value; }
        }

        public WbapAction Action
        {
            get { return action; }
        }



    }
}
