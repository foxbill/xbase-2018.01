using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Newtonsoft.Json;

namespace xbase.data.wbc
{
    public class VDataTable : IVisualWbo
    {
        //        private DataSource ds;
        private string elementName;

        public VDataTable(string name)
        {
            dataSource = name;
        }
        public string ElementName
        {
            get { return elementName; }
            set { elementName = value; }
        }

        private string controlName;

        public string ControlName
        {
            get { return controlName; }
            set { controlName = value; }
        }

        private string dataSource;

        public string DataSource
        {
            get { return dataSource; }
            set { dataSource = value; }
        }




        public string Render(string elementName)
        {
            StringBuilder sb = new StringBuilder();
            DataSource dsc = new DataSource(dataSource);
            sb.Append("<table border=1>");
            sb.Append("<thead name='");
            sb.Append(elementName);
            sb.Append(".head'>");

            sb.Append("<tr>");
            List<ListDataRow> page = dsc.rows();
            List<DataListColumn> fields = dsc.columns();

            foreach (DataListColumn fld in fields)
            {
                string fieldTitle = !string.IsNullOrEmpty(fld.title) ? fld.title : fld.field;
                sb.Append("<th name='");
                sb.Append(elementName);
                sb.Append(".");
                sb.Append(fld.field);
                sb.Append("'>");
                sb.Append(fld.title);
                sb.Append("</th>");
            }

            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");

            foreach (ListDataRow r in page)
            {

                sb.Append("<tr pk='");
               // sb.Append(JsonConvert.SerializeObject(r.Pk));
                sb.Append("' ");
                sb.Append("name='");
                sb.Append(elementName);
                sb.Append(".row'>");
                if (r.Count() > 0)
                    foreach (DataListColumn fld in fields)
                    {
                        sb.Append("<td name='");
                        sb.Append(elementName);
                        sb.Append(".");
                        sb.Append(fld.field);
                        sb.Append("'>");
                        sb.Append(r[fld.field]);
                        sb.Append("</td>");
                    }
                sb.Append("</tr>");
            }
            sb.Append("<tbody>");
            sb.Append("</Table>");
            return sb.ToString();
        }



        public string Render()
        {
            throw new NotImplementedException();
        }
    }
}
