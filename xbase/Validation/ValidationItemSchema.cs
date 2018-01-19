using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.BaseTypes;
using xbase.ObjectAction;
using System.Xml.Serialization;

namespace xbase.Validation
{
    public class ValidationItemSchema
    {
        private string validatorName;
        private IdsObjectList<IdValueObject> options = new IdsObjectList<IdValueObject>();

        public string ValidatorName
        {
            get { return validatorName; }
            set { validatorName = value; }
        }

        [XmlArrayItem("Option")]
        public IdsObjectList<IdValueObject> Options
        {
            get { return options; }
            set { options = value; }
        }

    }
}
