using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.wbs.wbdl
{
    public class DecisionControlSchema
    {
        private string target;

        public string Target
        {
            get { return target; }
            set { target = value; }
        }
        private string targetValue;

        public string TargetValue
        {
            get { return targetValue; }
            set { targetValue = value; }
        }
        private string gotoStep;

        public string GotoStep
        {
            get { return gotoStep; }
            set { gotoStep = value; }
        }
    }
}
