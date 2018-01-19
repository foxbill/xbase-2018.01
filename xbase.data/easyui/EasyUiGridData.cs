using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.data.easyui
{
    public class EasyUiGridData
    {
        public List<List<DataListColumn>> columns = new List<List<DataListColumn>>();
    //    public ListData data = new ListData();

        public string loadMsg { get; set; }

        public bool pagination { get; set; }

        public bool rownumbers { get; set; }

        public bool singleSelect { get; set; }

        public bool checkOnSelect { get; set; }

        public bool selectOnCheck { get; set; }

        public string pagePosition { get; set; }

  //      public int pageNumber { get; set; }

        public int pageSize { get; set; }

        public int[] pageList { get; set; }

        public bool showHeader { get; set; }

        public bool showFooter { get; set; }

        public string title { get; set; }

      //  public int total { get; set; }

        public bool striped { get; set; }

        //   public List<ListDataRow> rows { get; set; }

        public bool multiSort { get; set; }

        public bool isEnableFilter = true;
        public bool remoteFilter = true;
        public int filterDelay = 40000000;
        //      public List<FilterRule> filterRules;


        public List<FilterInput> filterInputs { get; set; }

        public string url { get; set; }
    }
}

