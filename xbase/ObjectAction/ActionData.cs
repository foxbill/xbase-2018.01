using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.ObjectAction
{
    public class ActionData
    {
        private string methodName;

        public string MethodName
        {
            get { return methodName; }
            set { methodName = value; }
        }
        private ActionOptions options = new ActionOptions();

        public ActionOptions Options
        {
            get { return options; }
            set { options = value; }
        }
    }
}
