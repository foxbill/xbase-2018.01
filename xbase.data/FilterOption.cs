using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.data
{
    public class FilterOption
    {
        private string _panelHeight = "auto";
        private List<OptionItem> _data;


        public int precision;
        public List<OptionItem> data
        {
            get { return _data; }
            set { _data = value; }
        }

        public string panelHeight
        {
            get { return _panelHeight; }
            set { _panelHeight = value; }
        }

    }
}
