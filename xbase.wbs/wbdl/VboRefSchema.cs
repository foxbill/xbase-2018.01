using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.wbs.wbdl
{
    public class VboRefSchema:Schema
    {
        private string elementId;

        public string ElementId
        {
            get { return elementId; }
            set { elementId = value; }
        }
        private string objectCategory;

        public string ObjectCategory
        {
            get { return objectCategory; }
            set { objectCategory = value; }
        }
        private string objectType;

        public string ObjectType
        {
            get { return objectType; }
            set { objectType = value; }
        }
        private SchemaList<ParameterSchema> options;

        public SchemaList<ParameterSchema> Options
        {
            get { return options; }
            set { options = value; }
        }
    }
}
