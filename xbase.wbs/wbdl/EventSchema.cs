using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase;
using System.Xml.Serialization;

namespace xbase.wbs.wbdl
{
    public class EventSchema : Schema
    {
        private string elementName;
        private string eventName;
        private string actionFlow;
        private SchemaList<NameValue> flowVars;

        /// <summary>
        /// 传递到流程中的变量；
        /// </summary>
        [XmlArrayItem("Var")]
        public SchemaList<NameValue> FlowVars
        {
            get { return flowVars; }
            set { flowVars = value; }
        }

        /// <summary>
        /// 流程ID
        /// </summary>
        public string ActionFlow
        {
            get { return actionFlow; }
            set { actionFlow = value; }
        }

        /// <summary>
        /// 事件名称 
        /// </summary>
        public string EventName
        {
            get { return eventName; }
            set { eventName = value; }
        }

        public string ElementName
        {
            get
            {
                return this.elementName;
            }
            set
            {
                this.elementName = value;
            }
        }
    }
}
