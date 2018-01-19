using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wbs.wbap
{
    public class ClientAction
    {
        private string funcName;
        private string returnValue;
        private List<object> parameters=new List<object>();

        public string FuncName
        {
            get { return funcName; }
            set { funcName = value; }
        }

        public List<object> Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        public string ReturnValue
        {
            get { return returnValue; }
            set { returnValue = value; }
        }
    }
}
