using System;
using System.Collections.Generic;
namespace xbase.umc
{
    public interface WboProxy
    {
        bool isStaticClass();
        Type getWboType();
        object createObject(System.Collections.Generic.Dictionary<string, string> jsonNamedParams);
        object createObject(string objectName);
        object createObject(string[] jsonParams);
        object getPropertyValue(object obj, string propertyName, object[] index);
        object invokeMethd(object obj, string methodName, Dictionary<string, string> jsonNamedParams);
        object invokeMethd(object obj, string methodName, string[] jsonParams);
        void setPropertyValue(object obj, string propertyName, object[] index, string jsonValue);
        object invoke(object obj, string callName, Dictionary<string, string> callParams);
        object invoke(object obj, string callName, string[] callParams);

        bool hasMember(string p);
    }
}
