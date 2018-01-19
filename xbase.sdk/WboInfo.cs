using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.sdk
{
    public class WboInfo : Wbo
    {
        private string _comId;
        public string comId
        {
            get { return _comId; }
            set { _comId = value; }
        }
    }
}
