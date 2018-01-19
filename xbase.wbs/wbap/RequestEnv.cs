using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wbs.wbap
{
    /*
     * 
     {
        FormId:"34324",
        AcionId:"34324",
        RunAt:"Server",
     * 
        RequestBody:{
            Method1:{
                RunAt:”Server”,
                  Params:{
                    Param1:”@Html”,
                    Param2:{
                            ListBable:{
                              fieldName:”aaa”,
                           fields:[field1,field2,field3,field4]
                              rows:[[],[],[] [],[],[][],[],[][],[]];
                        }
                    },
                }
            }
            Method1:{
                RunAt:”Server”,
                  Params:{
                    Param1:”@Html”,
                    Param2:”@234324”
                }
            }
        }
     }     

     */

    public class MethodRef
    {
        private string runAt;
        private string methodName;
        private string returnValue;

        public string ReturnValue
        {
            get { return returnValue; }
            set { returnValue = value; }
        }

        public string MethodName
        {
            get { return methodName; }
            set { methodName = value; }
        }

        private Dictionary<string, Object> parameters = new Dictionary<string, object>();

        public string RunAt
        {
            get { return runAt; }
            set { runAt = value; }
        }

        public Dictionary<string, Object> Parameters
        {
            get { return parameters; }
        }

    }

    public class RequestEnv
    {
        private string formId;
        private string actionId;
        private Dictionary<string, MethodRef> body = new Dictionary<string, MethodRef>();

        public string FormId
        {
            get { return formId; }
            set { formId = value; }
        }


        public string ActionId
        {
            get { return actionId; }
            set { actionId = value; }
        }

        public Dictionary<string, MethodRef> Body
        {
            get { return body; }
            set { body = value; }
        }

    }
}
