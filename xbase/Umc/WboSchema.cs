using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase;
using xbase.security;

namespace xbase.umc
{
    public enum AssemblyCategory
    {
        Unknow,
        DotNet,
        WebService,
        Com
    }

    public class WboSchema : Schema
    {
        private string assemblyName;
        private string className;
        private string objectUrl;
        private LifeCycle lifeCycle = LifeCycle.Request;
        private string containterType;
        private bool isPublish;
        private bool isVisual;
        private string src;
        private AssemblyCategory assemblyCategory;
        private string nameSpace;
        private PermissionTypes permissionTypes = PermissionTypes.Read;

        public PermissionTypes PermissionTypes
        {
            get { return permissionTypes; }
            set { permissionTypes = value; }
        }

        public string Namespace
        {
            get { return nameSpace; }
            set { nameSpace = value; }
        }
        public AssemblyCategory AssemblyCategory
        {
            get { return assemblyCategory; }
            set { assemblyCategory = value; }
        }
        public string Src
        {
            get { return src; }
            set { src = value; }
        }
        public bool IsVisual
        {
            get { return isVisual; }
            set { isVisual = value; }
        }


        public SchemaList<WboMethodSchema> Methods = new SchemaList<WboMethodSchema>();
        // public SchemaList<FunctionSchema> Functions = new SchemaList<FunctionSchema>();
        private SchemaList<Schema> properties = new SchemaList<Schema>();



        public bool IsPublish
        {
            get { return isPublish; }
            set { isPublish = value; }
        }

        public string ContainterType
        {
            get { return containterType; }
            set { containterType = value; }
        }


        public SchemaList<Schema> Properties
        {
            get { return properties; }
            set { properties = value; }
        }

        public string ObjectUrl
        {
            get { return objectUrl; }
            set { objectUrl = value; }
        }

        public LifeCycle LifeCycle
        {
            get { return lifeCycle; }
            set { lifeCycle = value; }
        }

        public string AssemblyName
        {
            get { return assemblyName; }
            set { assemblyName = value; }
        }

        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }


        public string AssemblyFile { get; set; }
    }

    public class JSObjectSchema : Schema
    {
        private string assemblyName;
        private string className;
        private string objectUrl;
        private LifeCycle lifeCycle;

        public string ObjectUrl
        {
            get { return objectUrl; }
            set { objectUrl = value; }
        }

        public LifeCycle LifeCycle
        {
            get { return lifeCycle; }
            set { lifeCycle = value; }
        }

        public string AssemblyName
        {
            get { return assemblyName; }
            set { assemblyName = value; }
        }

        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }

    }

}
