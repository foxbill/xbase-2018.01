using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace xbase.wbs
{
    public class FlowControlIf : IFlowControl
    {
        private string condition;

        #region FlowControl 成员

        public FlowStatus CheckDo()
        {

            throw new NotImplementedException();
        }

        public bool IsIterate
        {
            get { return false; }
        }

        #endregion
    }
}
