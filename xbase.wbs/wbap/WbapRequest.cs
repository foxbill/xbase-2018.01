using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wbs.wbap
{
    public class WbapRequest
    {
        private string pageName;//准备作废
        private string url;

        private string actionName;
        private int step;
        private List<Validator> validators = new List<Validator>();
        private WbapDataBody elementBinds = new WbapDataBody();

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public List<Validator> Validators
        {
            get { return validators; }
            set { validators = value; }
        }

        public int Step
        {
            get { return step; }
            set { step = value; }
        }

        /// <summary>
        /// object : String,DataList
        /// </summary>
        public WbapDataBody ElementBinds
        {
            get { return elementBinds; }
        }

        public string PageName
        {
            get { return pageName; }
            set { pageName = value; }
        }

        public string ActionId
        {
            get { return actionName; }
            set { actionName = value; }
        }
    }
}
