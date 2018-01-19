using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wbs.wbap
{
    public class Validator
    {
        private string element;
        private string validatorName;
        private List<string>  parameters=new List<string>();

        public string Element
        {
            get { return element; }
            set { element = value; }
        }

        public string ValidatorName
        {
            get { return validatorName; }
            set { validatorName = value; }
        }


        public List<string> Parameters
        {
            get { return parameters; }
        }
    }
}
