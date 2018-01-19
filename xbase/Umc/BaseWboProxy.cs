using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using xbase.Exceptions;
using xbase.local;

namespace xbase.umc
{
    public abstract class BaseWboProxy : WboProxy
    {
        public const string P_VALUE_NAME = "value";
        protected WboSchema wboSchema;
        protected Type type;
        protected Assembly assembly;


        protected BaseWboProxy(string objectTypeName)
        {
            this.wboSchema = WboSchemaContainer.Instance().GetItem(objectTypeName);
            loadAssembly();
        }

        protected BaseWboProxy(WboSchema wboSchema)
        {
            this.wboSchema = wboSchema;
            loadAssembly();
        }

        public static object[] convertParams(MethodBase methodBase, Dictionary<string, string> jsonNamedParams)
        {
            ParameterInfo[] paramInfos = methodBase.GetParameters();
            if (paramInfos.Length != jsonNamedParams.Count)
                return null;
            object[] ret = new object[paramInfos.Length];
            for (int i = 0; i < paramInfos.Length; i++)
            {
                ParameterInfo pi = paramInfos[i];
                if (!jsonNamedParams.ContainsKey(pi.Name))
                    return null;
                ret[i] = TypeConvertUtils.Convert(pi.ParameterType, jsonNamedParams[pi.Name]);
            }
            return ret;

        }
        public static object[] convertParams(MethodBase methodBase, string[] jsonParams)
        {
            ParameterInfo[] paramInfos = methodBase.GetParameters();
            if (paramInfos.Length != jsonParams.Length)
                return null;
            object[] ret = new object[paramInfos.Length];
            for (int i = 0; i < paramInfos.Length; i++)
            {
                ParameterInfo pi = paramInfos[i];
                try
                {
                    ret[i] = TypeConvertUtils.Convert(pi.ParameterType, jsonParams[i]);
                }
                catch
                {
                    return null;
                }
            }
            return ret;
        }

        /// <summary>
        /// 获取注册对象的类型
        /// </summary>
        /// <returns></returns>
        public Type getWboType()
        {
            return this.type;
        }

        public bool isStaticClass()
        {
            return (type.IsClass && type.IsAbstract && type.IsSealed);
        }

        protected abstract void loadAssembly();

        public object invokeMethd(object obj, string methodName, Dictionary<string, string> jsonNamedParams)
        {
            //  Type type = obj.GetType();
            if (jsonNamedParams == null || jsonNamedParams.Count() == 0)
            {

                MethodInfo mi = type.GetMethod(methodName, new Type[] { });
                if (mi == null)
                    throw new Exception(string.Format(Lang.NotFindMethod,type.Name,"0",methodName));
                if (mi.GetParameters().Length == 0)
                    return mi.Invoke(obj, null);
            }

            MethodInfo[] methods = type.GetMethods();
            foreach (MethodInfo mi in methods)
            {
                if (mi.Name.Equals(methodName, StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        object[] ps = convertParams(mi, jsonNamedParams);
                        if (ps != null)
                            return mi.Invoke(obj, ps);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }

            }
            throw new XException("对象中没有匹配的方法");
        }

        public object invokeMethd(object obj, string methodName, string[] jsonParams)
        {
            //   Type type = obj.GetType();
            if (jsonParams == null || jsonParams.Count() == 0)
            {
                MethodInfo mi = type.GetMethod(methodName);
                if (mi.GetParameters().Length == 0)
                    return mi.Invoke(obj, null);
            }
            MethodInfo[] methods = type.GetMethods();
            foreach (MethodInfo mi in methods)
            {
                if (mi.Name.Equals(methodName, StringComparison.OrdinalIgnoreCase))
                {
                    object[] ps = convertParams(mi, jsonParams);
                    if (ps != null)
                        return mi.Invoke(obj, ps);
                }
            }
            throw new XException("对象中没有匹配的方法");
        }

        public void setPropertyValue(object instance, string propertyName, object[] index, string jsonValue)
        {
            //   Type type = instance.GetType();
            PropertyInfo prop = type.GetProperty(propertyName);
            if (prop != null)
            {
                object value = TypeConvertUtils.Convert(prop.PropertyType, jsonValue);
                prop.SetValue(instance, value, index);
                return;
            }

            FieldInfo fld = type.GetField(propertyName);
            if (fld != null)
            {
                object value = TypeConvertUtils.Convert(fld.FieldType, jsonValue);
                fld.SetValue(instance, value);
                return;
            }

            throw new XException("不能发现属性成员" + propertyName);

        }

        public object getPropertyValue(object instance, string propertyName, object[] index)
        {
            //   Type type = instance.GetType();
            PropertyInfo prop = type.GetProperty(propertyName);
            if (prop != null)
                return prop.GetValue(instance, index);

            FieldInfo fld = type.GetField(propertyName);
            if (fld != null)
                return fld.GetValue(instance);

            throw new XException("不能发现属性成员" + propertyName);
        }

        public abstract object createObject(Dictionary<string, string> jsonNamedParams);

        public abstract object createObject(string objectName);

        public abstract object createObject(string[] jsonParams);



        public object invoke(object obj, string callName, Dictionary<string, string> callParams)
        {
            //  Type type = obj.GetType();
            MemberInfo[] mis = type.GetMember(callName);

            if (mis == null || mis.Count() < 1)
                throw new XException("不能发现执行成员"+obj.GetType().Name+"."+callName);

            if ((mis[0].MemberType & MemberTypes.Property) == MemberTypes.Property ||
                (mis[0].MemberType & MemberTypes.Field) == MemberTypes.Field)
            {
                string jsonValue = null;
                if (callParams.ContainsKey(P_VALUE_NAME))
                {
                    jsonValue = callParams[P_VALUE_NAME];
                    setPropertyValue(obj, callName, null, jsonValue);
                }
                return getPropertyValue(obj, callName, null);
            }
            else if ((mis[0].MemberType & MemberTypes.Method) == MemberTypes.Method)
            {
                return invokeMethd(obj, callName, callParams);
            }
            else
                throw new XException("不支持执行成员：" + callName);

        }

        public object invoke(object obj, string callName, string[] callParams)
        {
            // Type type = obj.GetType();
            MemberInfo[] mis = type.GetMember(callName);

            if (mis == null || mis.Count() < 1)
                throw new XException("不能发现执行成员");

            if ((mis[0].MemberType & MemberTypes.Property) == MemberTypes.Property ||
                (mis[0].MemberType & MemberTypes.Field) == MemberTypes.Field)
            {
                string jsonValue = null;
                if (callParams.Count() > 0)
                {
                    jsonValue = callParams[0];
                    setPropertyValue(obj, callName, null, jsonValue);
                }
                return getPropertyValue(obj, callName, null);
            }
            else if ((mis[0].MemberType & MemberTypes.Method) == MemberTypes.Method)
            {
                return invokeMethd(obj, callName, callParams);
            }
            else
                throw new XException("不支持执行成员：" + callName);
        }



        public bool hasMember(string memberName)
        {
           MemberInfo[] members=  type.GetMembers();
           foreach (MemberInfo member in members)
           {
               if (member.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase))
                   return true;
           }
           return false;
        }
    }
}
