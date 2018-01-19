using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.data
{
    public class DataListColumn
    {
        public string title;
        public string field;
        public int width;
        public int rowspan;
        public int colspan;

        /// <summary>
        ///水平对齐方式，值包括： 'left','right','center' 
        /// </summary>
        public string align;
        public bool hidden { get; set; }

        public string editor { get; set; }

        public string halign { get; set; }

        public bool sortable { get; set; }

        public string order { get; set; }

        public bool resizable { get; set; }

        public bool checkbox { get; set; }
    }
}
