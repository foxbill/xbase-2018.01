using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase;

namespace xbase.wbs.wbdl
{
    public class WbdlControlSchema : Schema
    {
        private string venderObject;
        private string verderUrl;
        private ActionSchema dataFunction = new ActionSchema();

        public string VenderObject
        {
            get { return venderObject; }
            set { venderObject = value; }
        }

        public string VerderUrl
        {
            get { return verderUrl; }
            set { verderUrl = value; }
        }

        public ActionSchema DataFunction
        {
            get { return dataFunction; }
            set { dataFunction = value; }
        }
    }
}
