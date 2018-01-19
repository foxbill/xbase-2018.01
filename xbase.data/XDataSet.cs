using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase;
using System.Data;
using System.Data.Common;
//using xbase.data.db;

namespace xbase.data
{
    public class XRelationSchema : Schema
    {
        private string masterTableName;
        private List<string> masterFields = new List<string>();
        private string detailTableName;
        private List<string> detailFields = new List<string>();
        private bool nested;

        public string MasterTableName
        {
            get { return masterTableName; }
            set { masterTableName = value; }
        }


        public List<string> MasterFields
        {
            get { return masterFields; }
            set { masterFields = value; }
        }
        public string DetailTableName
        {
            get { return detailTableName; }
            set { detailTableName = value; }
        }

        public List<string> DetailFields
        {
            get { return detailFields; }
            set { detailFields = value; }
        }

        public bool Nested
        {
            get { return nested; }
            set { nested = value; }
        }
    }


    public class XDataSetSchema : Schema
    {
        private List<XRelationSchema> relations = new List<XRelationSchema>();
        private List<DataSourceSchema> tables = new List<DataSourceSchema>();

        public List<XRelationSchema> Relations
        {
            get { return relations; }
            set { relations = value; }
        }

        public List<DataSourceSchema> Tables
        {
            get { return tables; }
            set { tables = value; }
        }
    }

    public class XDataSetSchemaContainer : SchemaContainer<XDataSetSchema>
    {
    }

 
    /// <summary>
    /// xdataset relations definition
    /// </summary>
    public class XDataSetRelation
    {
        private string masterTableName;
        private string masterField;
        private string detailTableName;
        private string detailField;

        public string DetailTableName
        {
            get { return detailTableName; }
            set { detailTableName = value; }
        }

        public string MasterTableName
        {
            get { return masterTableName; }
            set { masterTableName = value; }
        }

        public string DetailField
        {
            get { return detailField; }
            set { detailField = value; }
        }

        public string MasterField
        {
            get { return masterField; }
            set { masterField = value; }
        }
    }
}
