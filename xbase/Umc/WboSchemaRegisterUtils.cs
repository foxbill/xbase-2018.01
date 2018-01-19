using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web.Services;
using xbase.umc.attributes;
namespace xbase.umc
{
    public static class WboSchemaRegisterUtils
    {
        /// <summary>
        /// 获取Wbo对象的注册Id
        /// </summary>
        /// <param name="type">Wbo对象的对象类型类型</param>
        /// <returns></returns>
        public static string getTypeRegId(Type type)
        {

            Object[] objAttrs = type.GetCustomAttributes(typeof(WboAttr), true);

            foreach (Attribute attr in objAttrs)
            {
                WboAttr wboAttr = attr as WboAttr;

                if (wboAttr != null)
                {
                    if (!String.IsNullOrEmpty(wboAttr.Id))
                        return wboAttr.Id;
                }
            }

            return type.Name;
        }
        public static T BuildObjectSchema<T>(Type type)
             where T : WboSchema, new()
        {
            return BuildObjectSchema<T>(type, null);
        }
        /// <summary>
        /// 生存Wbo对象的架构实体WboSchema
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T BuildObjectSchema<T>(Type type, string comId)
            where T : WboSchema, new()
        {
            T os = null;

            os = new T();
            os.Src = type.Assembly.Location;
            os.AssemblyName = type.Assembly.FullName;
            os.ClassName = type.FullName;
            os.Id = type.Name.Replace("<>","");
            if (!string.IsNullOrEmpty(comId))
                os.Id = comId;
            os.Title = os.Id;
            os.IsVisual = type.GetInterface(typeof(IVisualWbo).FullName) != null;


            Object[] webserviceAttrs = type.GetCustomAttributes(typeof(System.Web.Services.WebServiceBindingAttribute), true);

            if (webserviceAttrs.Length > 0)
            {
                os.Namespace = ((WebServiceBindingAttribute)webserviceAttrs[0]).Namespace;
            }
            else
                os.Namespace = type.Namespace;

            //else
            //    os.Namespace = os.ClassName.Substring(0, os.ClassName.LastIndexOf('.'));
            Object[] objAttrs = type.GetCustomAttributes(typeof(WboAttr), true);
            foreach (Attribute attr in objAttrs)
            {
                WboAttr umcAttr = attr as WboAttr;
                if (umcAttr != null)
                {
                    os.LifeCycle = umcAttr.LifeCycle;
                    if (!String.IsNullOrEmpty(umcAttr.Title))
                        os.Title = umcAttr.Title;
                    if (!String.IsNullOrEmpty(umcAttr.Id))
                        os.Id = umcAttr.Id;
                    os.Description = umcAttr.Description;

                    if (umcAttr.ContainerType != null)
                    {
                        os.ContainterType = umcAttr.ContainerType.AssemblyQualifiedName;
                    }
                    os.IsPublish = umcAttr.IsPublish;
                }
            }

            MethodInfo[] methods = type.GetMethods();

            for (int i = 0; i < methods.Length; i++)
            {
                MethodInfo mi = methods[i];
                string methodName = mi.Name;
                if (methodName.StartsWith("set_") || methodName.StartsWith("get_"))
                {
                    string propHame = methodName.Substring(4);
                    if (type.GetProperty(propHame) != null)
                        continue;
                }

                Object[] methodAttrs = mi.GetCustomAttributes(typeof(WboMethodAttr), true);
                WboMethodSchema wms = new WboMethodSchema();
                wms.Id = methodName;
                wms.Title = wms.Id;
                foreach (Attribute attr in methodAttrs)
                {
                    WboMethodAttr wma = attr as WboMethodAttr;
                    wms.Id = methodName;
                    wms.Title = wms.Id;
                    if (!String.IsNullOrEmpty(wma.Title))
                        wms.Title = wma.Title;
                    wms.Description = wma.Description;
                    wms.PermissionTypes = wma.PermissionTypes;
                    //   os.Methods.Add(wms);
                }

                // string methodName = methods[i].Name;
                wms.ReturnType = methods[i].ReturnType.ToString();
                //FunctionSchema fun = new FunctionSchema();
                //fun.Id = methodName;
                //fun.ObjectId = os.Id;
                //fun.MethodName = methods[i].Name;
                //fun.ReturnValue = methods[i].ReturnType.ToString();

                ParameterInfo[] paramInfos = methods[i].GetParameters();

                for (int j = 0; j < paramInfos.Length; j++)
                {
                    ParameterInfo pi = paramInfos[j];
                    //  ParamDef paramDef = new ParamDef();
                    //  paramDef.ParamName = pi.Name;
                    //  paramDef.DataType = pi.ParameterType.AssemblyQualifiedName;
                    //  fun.ParamDefs.Add(paramDef);
                    WboMethodParamSchema wmps = new WboMethodParamSchema();
                    wmps.DataType = pi.ParameterType.AssemblyQualifiedName;
                    wmps.Title = pi.Name;
                    wmps.Id = pi.Name;
                    if (pi.DefaultValue != null)
                        wmps.DefaultValue = pi.DefaultValue.ToString();
                    wms.Params.Add(wmps);
                }
                if (!os.Methods.Contains(wms))
                    os.Methods.Add(wms);
            }


            PropertyInfo[] pis = type.GetProperties();
            for (int i = 0; i < pis.Length; i++)
            {
                object[] pAttrs = pis[i].GetCustomAttributes(typeof(WboPropertyAttr), true);
                PropertyInfo pi = pis[i];
                Schema pros = new Schema();
                pros.Id = pi.Name;

                foreach (WboPropertyAttr prAttr in pAttrs)
                {
                    pros.Description = prAttr.Description;
                    pros.Title = prAttr.Title;
                }

                os.Properties.Add(pros);

            }

            if (type.IsSubclassOf(typeof(ISessionWbo)))
                os.LifeCycle = LifeCycle.Session;

            return os;

        }

    }
}
