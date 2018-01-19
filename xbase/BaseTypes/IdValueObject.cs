using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.BaseTypes
{
    public class IdValueObject:IdsObject
    {
        private string value;

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    }
}
