using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wbs.wbap
{
    public class JsOperate
    {
        private string runAt;
        private string returnValue;
        private string operateName;
        private Dictionary<string, Object> operateData = new Dictionary<string, object>();

        public string RunAt
        {
            get { return runAt; }
        }

        public string ReturnValue
        {
            get { return returnValue; }
        }

        public string OperateName
        {
            get { return operateName; }
        }

        public Dictionary<string, Object> OperateData
        {
            get { return operateData; }
        }
    }
}
