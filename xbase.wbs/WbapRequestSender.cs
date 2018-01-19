using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.wbs
{
    public class WbapRequestSender
    {
        private string elementName;
        private string rowId;
        private int index;
        private string value;

        public string ElementName
        {
            get { return elementName; }
            set { elementName = value; }
        }

        public string RowId
        {
            get { return rowId; }
            set { rowId = value; }
        }

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

    }
}
