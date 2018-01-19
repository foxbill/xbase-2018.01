using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;
using xbase;
using xbase.BaseTypes;
using System.Data;

namespace xbase.data
{
    public class FieldMap
    {
        private string sourceField;
        private string destinationField;

        public string SourceField
        {
            get { return sourceField; }
            set { sourceField = value; }
        }

        public string DestinationField
        {
            get { return destinationField; }
            set { destinationField = value; }
        }
    }
    public class ReferenceSchema : Schema
    {
        private List<FieldMap> linkFields = new List<FieldMap>();

        private List<FieldMap> maps = new List<FieldMap>();

        public List<FieldMap> Maps
        {
            get { return maps; }
            set { maps = value; }
        }
        public List<FieldMap> LinkFields
        {
            get { return linkFields; }
            set { linkFields = value; }
        }

    }

    /// <summary>
    /// LookupMap Schema
    /// </summary>
    public class LookupMapSchema : Schema
    {
        private string lookupField;
        private string srcField;
        public string LookupField
        {
            get { return lookupField; }
            set { lookupField = value; }
        }
    }

    public class Param : Schema
    {
        private string dataType;

        public string DataType
        {
            get { return dataType; }
            set { dataType = value; }
        }
    }

    /// <summary>
    /// XTable中的lookup结构
    /// </summary>
    public class LookupFieldSchema : Schema
    {
        private string lookupTable;
        private string lookupField;
        private SchemaList<LookupMapSchema> lookupMap = new SchemaList<LookupMapSchema>();
        private SchemaList<Param> parameters = new SchemaList<Param>();

        public SchemaList<Param> Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        public SchemaList<LookupMapSchema> LookupMap
        {
            get { return lookupMap; }
            set { lookupMap = value; }
        }

        public string LookupField
        {
            get { return lookupField; }
            set { lookupField = value; }
        }

        public string LookupTable
        {
            get { return lookupTable; }
            set { lookupTable = value; }
        }
    }

    public class XValidationSchema : Schema
    {
        private SchemaList<Param> parameters = new SchemaList<Param>();

        public SchemaList<Param> Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }
    }

    public enum DataExtendType
    {
        None = 0,
        File = 1,
        ImageFile = 2,
        HtmlFile = 3,
        Html = 4
    }

    /// <summary>
    /// XTable中的Field结构
    /// </summary>
    public class FieldSchema : Schema
    {
        private string editFormat;
        private string defaultValue;
        private bool visable = true;
        //        private string displayName;
        private DbType dataType = DbType.String;
        private string expression;
        private bool readOnly;
        private bool isAutoInc;
        private bool isGUID;
        private bool isKey;
        private string alias;
        private bool isInForm = true;
        private bool isInCard = true;
        private OptionSchema option = new OptionSchema();

        private DataExtendType extendType = DataExtendType.None;
        private SchemaList<XValidationSchema> valids = new SchemaList<XValidationSchema>();



        public OptionSchema Option
        {
            get { return option; }
            set { option = value; }
        }
        //   public string CodeTable { get; set; }
        public bool IsInForm
        {
            get { return isInForm; }
            set { isInForm = value; }
        }

        public bool IsInCard
        {
            get { return isInCard; }
            set { isInCard = value; }
        }


        public int DisplayWidth = 60;

        public bool IsKey
        {
            get { return isKey; }
            set { isKey = value; }
        }


        public DataExtendType ExtendType
        {
            get { return extendType; }
            set { extendType = value; }
        }

        public bool ReadOnly
        {
            get { return readOnly; }
            set { readOnly = value; }
        }

        public string Expression
        {
            get { return expression; }
            set { expression = value; }
        }

        public SchemaList<XValidationSchema> Valids
        {
            get { return valids; }
            set { valids = value; }
        }

        public DbType DataType
        {
            get { return dataType; }
            set { dataType = value; }
        }

        //public string DisplayName
        //{
        //    get { return displayName; }
        //    set { displayName = value; }
        //}

        public bool Visable
        {
            get { return visable; }
            set { visable = value; }
        }

        public string EditFormat
        {
            get { return editFormat; }
            set { editFormat = value; }
        }

        public string DefaultValue
        {
            get { return defaultValue; }
            set { defaultValue = value; }
        }

        public bool IsAutoInc
        {
            get { return isAutoInc; }
            set { isAutoInc = value; }
        }

        public bool IsGUID
        {
            get { return isGUID; }
            set { isGUID = value; }
        }

        public string Alias
        {
            get { return alias; }
            set { alias = value; }
        }

        public string Editor { get; set; }

    }

    public class FkTableSchema : Schema
    {
        //        private string masterTable;
        private string masterFields;
        private string fkFields;

        public string MasterTable
        {
            get { return Id; }
            set { Id = value; }
        }

        public string MasterFields
        {
            get { return masterFields; }
            set { masterFields = value; }
        }

        public string FkFields
        {
            get { return fkFields; }
            set { fkFields = value; }
        }
    }


    public static class XTableSchemaConst
    {
        public const string FieldSplitor = ",";
    }

    public class UserSQLCommand : Schema
    {
        private string sqlText;

        public string SqlText
        {
            get { return sqlText; }
            set { sqlText = value; }
        }
    }




    public class SubTableSchema
    {
        private List<string> fks = new List<string>();
        public string Name { get; set; }

        public List<string> Fks
        {
            get { return fks; }
            set { fks = value; }
        }
    }


    /// <summary>
    /// XTable结构
    /// </summary>
    public class DataSourceSchema : Schema
    {
        private CommandSchema insertCommand = new CommandSchema();
        private CommandSchema updateCommand = new CommandSchema();
        private CommandSchema deleteCommand = new CommandSchema();
        private CommandSchema selectCommand = new CommandSchema();

        private List<SubTableSchema> subTables = new List<SubTableSchema>();



        private string connectionName;
        private string tableName;

        private int pageSize = 10;



        private SchemaList<FieldSchema> fields = new SchemaList<FieldSchema>();
        private SchemaList<LookupFieldSchema> lookupFields = new SchemaList<LookupFieldSchema>();
        private SchemaList<ReferenceSchema> references = new SchemaList<ReferenceSchema>();
        private SchemaList<FkTableSchema> fks = new SchemaList<FkTableSchema>();
        private SchemaList<UserSQLCommand> userSqlCommands = new SchemaList<UserSQLCommand>();


        private bool isAutoNewLine;

        public List<SubTableSchema> SubTables
        {
            get { return subTables; }
            set { subTables = value; }
        }

        public bool IsAutoNewLine
        {
            get { return isAutoNewLine; }
            set { isAutoNewLine = value; }
        }

        public SchemaList<UserSQLCommand> UserSqlCommands
        {
            get { return userSqlCommands; }
            set { userSqlCommands = value; }
        }
        //        private SchemaList<

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        public string ConnectionName
        {
            get { return connectionName; }
            set { connectionName = value; }
        }

        public SchemaList<ReferenceSchema> References
        {
            get { return references; }
            set { references = value; }
        }

        public SchemaList<LookupFieldSchema> LookupFields
        {
            get { return lookupFields; }
            set { lookupFields = value; }
        }

        public List<string> PrimaryKeys
        {
            get
            {
                List<string> ret = new List<string>();
                foreach (FieldSchema fld in fields)
                {
                    if (fld.IsKey) ret.Add(fld.Id);
                }
                return ret;
            }
            set
            {
                foreach (string pkName in value)
                {
                    Fields.GetItem(pkName).IsKey = true;
                }
            }
        }

        public CommandSchema InsertCommand
        {
            get { return insertCommand; }
            set { insertCommand = value; }
        }

        public CommandSchema UpdateCommand
        {
            get { return updateCommand; }
            set { updateCommand = value; }
        }

        public CommandSchema DeleteCommand
        {
            get { return deleteCommand; }
            set { deleteCommand = value; }
        }

        public CommandSchema SelectCommand
        {
            get { return selectCommand; }
            set { selectCommand = value; }
        }

        public SchemaList<FieldSchema> Fields
        {
            get { return fields; }
            set { fields = value; }
        }

        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }

        public bool IsPagingByParams { get; set; }
    }

    public enum XParamDataType { STRING, NUMBER, DATE }


}
