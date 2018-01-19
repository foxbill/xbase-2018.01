using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wbs.wbap
{
    public class WbapList
    {
        public  const string TypeMarker = "_List";

        private string list;
        private string templateRow;
        private string dataRow;
        private bool isAdd = false;
        private List<string> columns = new List<string>();
        private List<string> columnTitles = new List<string>();
        private List<List<string>> deleteKeys = new List<List<string>>();
        private List<List<string>> data = new List<List<string>>();

        public bool IsAdd
        {
            get { return isAdd; }
            set { isAdd = value; }
        }

        public string DataRow
        {
            get { return dataRow; }
            set { dataRow = value; }
        }

        public string Id
        {
            get { return list; }
            set { list = value; }
        }

        public string TemplateRow
        {
            get { return templateRow; }
            set { templateRow = value; }
        }


        public List<string> Columns
        {
            get { return columns; }
        }

        public List<string> ColumnTitles
        {
            get { return columnTitles; }
        }

        public List<List<string>> DeleteKeys
        {
            get { return deleteKeys; }
        }

        public List<List<string>> Data
        {
            get { return data; }
        }

        public void Clear()
        {
            columns.Clear();
            columnTitles.Clear();
            deleteKeys.Clear();
            data.Clear();
        }

        public List<string> AppendRow()
        {
            List<string> row = new List<string>();
            for (int i = 0; i < columns.Count; i++) { row.Add(""); }
            data.Add(row);
            return row;
        }

    }
}
