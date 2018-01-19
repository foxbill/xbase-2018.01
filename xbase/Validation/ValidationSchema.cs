using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.BaseTypes;
using System.Xml.Serialization;

namespace xbase.Validation
{
    public class ValidationSchema
    {
        private List<ValidationItemSchema> validators=new List<ValidationItemSchema>();
       
        [XmlArrayItem("Validator")]
        public List<ValidationItemSchema> Validators
        {
            get { return validators; }
            set { validators = value; }
        }

    }
}
