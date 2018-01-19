using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.umc.attributes
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class WboAttr : System.Attribute
    {
        public LifeCycle LifeCycle;
        public double Version;
        public string Title;
        public string Id;
        public string Description;
        public Type ContainerType;
        public bool IsPublish = false;

        public WboAttr()
        {
            Version = 1.0;
            this.LifeCycle = LifeCycle.Session;
        }
    }

}
