using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.data;

namespace xbase.bi.schema
{

    public class SubjectSchema : xbase.Schema
    {
        private string tableId;
        private string chartId;
        private string pageLink;

        public string PageLink
        {
            get { return pageLink; }
            set { pageLink = value; }
        }


        private string text;
        private xbase.SchemaList<SubjectSchema> childSubjects = new xbase.SchemaList<SubjectSchema>();

        public string ChartId
        {
            get { return chartId; }
            set { chartId = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }


        public xbase.SchemaList<SubjectSchema> ChildSubjects
        {
            get { return childSubjects; }
            set { childSubjects = value; }
        }

        public string TableId
        {
            get { return tableId; }
            set { tableId = value; }
        }

    }

    public class ContentItem:xbase.Schema
    {
        private string  itemType;
        private string objectName;
        private List<xbase.NamedValueSchema> propertys;
        private List<xbase.NamedValueSchema> attribules;

    }

}
