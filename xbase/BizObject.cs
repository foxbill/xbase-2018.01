using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace xbase
{
    /// <summary>
    /// 业务对象基类
    /// </summary>
    public class BizObject
    {
        private string id;
        private string sessionId = "";

        public BizObject() { }

        public   BizObject(Schema schema)
        {
            SetSchema(schema);
        }

        public void SetSchema(Schema schema)
        {
            Type schemaType = schema.GetType();
            Type thisType = this.GetType();

            PropertyInfo[] thisProps = thisType.GetProperties();

            foreach (PropertyInfo thisProp in thisProps)
            {
                string propName = thisProp.Name;

                PropertyInfo schemaProp = schemaType.GetProperty(propName, BindingFlags.Public);

                if (schemaType == null) continue;

                if (schemaProp.PropertyType == thisProp.PropertyType && !propName.Equals("Id"))
                {
                    // Type.i
                    thisProp.SetValue(this, schemaProp.GetValue(schema, null), null);
                }
                else if (schemaType.IsSubclassOf(typeof(Schema)))
                {
                    object thisPropValue = thisProp.GetValue(this, null);
                    if (thisProp.PropertyType.IsSubclassOf(typeof(BizObject)))
                    {
                        (thisPropValue as BizObject).SetSchema(schemaProp.GetValue(schema, null) as Schema);
                    }
                }
                else if (schemaType.IsSubclassOf(typeof(SchemaList<Schema>)))
                {
                    throw new NotImplementedException();
                }
            }
        }
        /// <summary>
        /// 对象Id
        /// </summary>
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 会话标志
        /// </summary>
        public string SessionId
        {
            get { return sessionId; }
        }
    }
}
