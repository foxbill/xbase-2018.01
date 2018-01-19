using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase;
using System.Web;

namespace xbase.umc
{
    public class WboSchemaContainer : SchemaContainer<WboSchema>
    {

        public static SchemaContainer<WboSchema> Instance()
        {
            if (instance == null)
                Initialize(HttpRuntime.AppDomainAppPath + "App_Data\\umc\\object\\");
            return instance;
        }
    }

    public class JSObjectSchemaContainer : SchemaContainer<JSObjectSchema>
    {

    }
}
