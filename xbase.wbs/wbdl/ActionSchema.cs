using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase;
using System.Xml.Serialization;

namespace xbase.wbs.wbdl
{
    public enum RunAtType { Server, Client }

    public class ParameterSchema : Schema
    {
        private string value;

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    }

    public class ActionSchema : Schema
    {
        private SchemaList<ParameterSchema> parameters = new SchemaList<ParameterSchema>();
        private string returnValue;
        private string methodName;
        private string dataSourceId;
        private RunAtType runAt = RunAtType.Server;
        private List<DataPropertySchema> props;
        private string clientScript;
        private List<DecisionControlSchema> decisions;
        private List<ActionSchema> actions;

        [XmlArrayItem("Action")]
        public List<ActionSchema> Actions
        {
            get { return actions; }
            set { actions = value; }
        }

        [XmlArrayItem("Decision")]
        public List<DecisionControlSchema> Decisions
        {
            get { return decisions; }
            set { decisions = value; }
        }
        
        public string ClientScript
        {
            get { return clientScript; }
            set { clientScript = value; }
        }


        [XmlArrayItem("Prop")]
        public List<DataPropertySchema> Props
        {
            get { return props; }
            set { props = value; }
        }


        public string DataSourceId
        {
            get { return dataSourceId; }
            set { dataSourceId = value; }
        }

        public RunAtType RunAt
        {
            get { return runAt; }
            set { runAt = value; }
        }

        public string MethodName
        {
            get { return methodName; }
            set { methodName = value; }
        }

        [XmlArrayItem("Parameter")]
        public SchemaList<ParameterSchema> Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        public string ReturnValue
        {
            get { return returnValue; }
            set { returnValue = value; }
        }

        internal bool IsRunAtClient()
        {
            return this.runAt==RunAtType.Client;
        }
    }

    /// <summary>
    /// 行为实体
    /// </summary>
    public class ActionFlowSchema : Schema
    {
        private string returnValue;
        private List<ActionSchema> actions = new SchemaList<ActionSchema>();
        private List<DataPropertySchema> vars = new List<DataPropertySchema>();

        [XmlArrayItem("Var")]
        public List<DataPropertySchema> Vars
        {
            get { return vars; }
            set { vars = value; }
        }

        [XmlArrayItem("Action")]
        public List<ActionSchema> Actions
        {
            get { return actions; }
            set { actions = value; }
        }


    }

}
