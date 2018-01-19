using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wbs.wbap
{
    public class WbapControl
    {
        private string venderObject;
        public string VenderObject
        {
            get { return venderObject; }
            set { venderObject = value; }
        }

        private string venderUrl;
        public string VenderUrl
        {
            get { return venderUrl; }
            set { venderUrl = value; }
        }
        
        private object data;
        public object Data
        {
            get { return data; }
            set { data = value; }
        }
    }
}
