using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.umc
{
    public class ComWboProxy : DotNetWboProxy
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="objectTypeName">对象注册名</param>
        public ComWboProxy(string objectTypeName) :
            base(objectTypeName)
        {
        }

        /// <summary>
        /// 通过Wbo对象构架（WboSchema） 加载代理
        /// </summary>
        /// <param name="wboSchema">Wbo对象构架</param>
        public ComWboProxy(WboSchema wboSchema) :
            base(wboSchema)
        {
        }

        protected override void loadAssembly()
        {
            //  base.loadAssembly();
            this.type = Type.GetTypeFromProgID(this.wboSchema.AssemblyName);
        }

    }
}
