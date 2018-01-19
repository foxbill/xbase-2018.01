using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.BaseTypes;
using System.Xml.Serialization;

namespace xbase.ObjectAction
{
    public class ActionOptions : IdsObject
    {
        private IdsObjectList<IdValueObject> propertys = new IdsObjectList<IdValueObject>();
        private IdsObjectList<IdValueObject> methodParams = new IdsObjectList<IdValueObject>();

        [XmlArrayItem("Property")]
        public IdsObjectList<IdValueObject> Propertys
        {
            get { return propertys; }
            set { propertys = value; }
        }

        [XmlArrayItem("Parameter")]
        public IdsObjectList<IdValueObject> MethodParams
        {
            get { return methodParams; }
            set { methodParams = value; }
        }

    }
}
