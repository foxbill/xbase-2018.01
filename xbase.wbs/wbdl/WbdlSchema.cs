using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using xbase;
using xbase.Validation;


namespace xbase.wbs.wbdl
{

    /// <summary>
    /// 表单信息定义
    /// </summary>
    public class WbdlSchema : Schema
    {

        private SchemaList<WbdlDataSchema> dataSources;//数据源
        private SchemaList<ElementDataSchema> elementDatas;//元素数据捆绑
        private SchemaList<ActionFlowSchema> actionFlows;//活动流程集合
        private SchemaList<EventSchema> events = new SchemaList<EventSchema>();//事件
        private SchemaList<WbdlControlSchema> controls = new SchemaList<WbdlControlSchema>();




        //Old Propertys
        private SchemaList<FieldBindSchema> fieldBinds = new SchemaList<FieldBindSchema>();
        private SchemaList<DataListBindSchema> dataListBinds = new SchemaList<DataListBindSchema>();
        private SchemaList<EventSchema> eventBinds = new SchemaList<EventSchema>();
        private SchemaList<ActionFlowSchema> actions = new SchemaList<ActionFlowSchema>();


        [XmlArrayItem("Event")]
        public SchemaList<EventSchema> Events
        {
            get { return events; }
            set { events = value; }
        }

        [XmlArrayItem("DataSource")]
        public SchemaList<WbdlDataSchema> DataSources
        {
            get { return dataSources; }
            set { dataSources = value; }
        }

        [XmlArrayItem("ActionFlow")]
        public SchemaList<ActionFlowSchema> ActionFlows
        {
            get { return actionFlows; }
            set { actionFlows = value; }
        }

        [XmlArrayItem("ElementData")]
        public SchemaList<ElementDataSchema> ElementDatas
        {
            get { return elementDatas; }
            set { elementDatas = value; }
        }

        /// <summary>
        /// 行为集合
        /// </summary>
        [XmlArrayItem("Action")]
        public SchemaList<ActionFlowSchema> Actions
        {
            get { return actions; }
        }
        /// <summary>
        /// 事件集合
        /// </summary>


        [XmlArrayItem("Control")]
        public SchemaList<WbdlControlSchema> Controls
        {
            get { return controls; }
        }

        [XmlArrayItem("EventBind")]
        public SchemaList<EventSchema> EventBinds
        {
            get { return eventBinds; }
        }
        /// <summary>
        /// 单个元素
        /// </summary>
        [XmlArrayItem("FieldBind")]
        public SchemaList<FieldBindSchema> FieldBinds
        {
            get { return fieldBinds; }
        }
        /// <summary>
        /// 多行元素集合
        /// </summary>
        [XmlArrayItem("DataListBind")]
        public SchemaList<DataListBindSchema> DataListBinds
        {
            get { return dataListBinds; }
        }
    }

    public enum ElementBindType { SEQ, REP }

    public class ElementDataSchema : Schema
    {
        private string dataSourceId;
        private string dataField;
        private string value;
        private ElementBindType bindType = ElementBindType.SEQ;
        private bool mapServerPath = false;
        private ValidationSchema validation = new ValidationSchema();
        private bool readOnly = false;

        public bool ReadOnly
        {
            get { return readOnly; }
            set { readOnly = value; }
        }

        public ValidationSchema Validation
        {
            get { return validation; }
            set { validation = value; }
        }

        public bool MapServerPath
        {
            get { return mapServerPath; }
            set { mapServerPath = value; }
        }

        public ElementBindType BindType
        {
            get { return bindType; }
            set { bindType = value; }
        }

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public string DataSourceId
        {
            get { return dataSourceId; }
            set { dataSourceId = value; }
        }

        public string DataField
        {
            get { return dataField; }
            set { dataField = value; }
        }


    }
}
