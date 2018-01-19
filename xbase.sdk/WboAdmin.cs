using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.umc;
using System.Reflection;
using xbase.Exceptions;
using xbase.umc.attributes;
using xbase.tree;

namespace xbase.sdk
{
    [WboAttr(Id = "WboAdmin", Title = "对象管理", LifeCycle = LifeCycle.Global, Description = "Wbo对象管理工具")]
    public class WboAdmin
    {

        /// <summary>
        /// 获取所有组件的Id、方法、属性及持久化实例
        /// </summary>
        /// <returns></returns>
        public static List<TreeNode> getWboNodes()
        {
            List<TreeNode> nodes = new List<TreeNode>();
            List<Wbo> wbos = GetObjectTypes();

            foreach (Wbo wbo in wbos)
            {
                TreeNode node = new TreeNode();
                node.id = wbo.name;
                node.text = wbo.title;
                node.label = wbo.title;
                nodes.Add(node);

                TreeNode propParentNode = new TreeNode();
                propParentNode.id = node.id + Umc.MemberSpliter + "properties";
                propParentNode.text = "属性";
                propParentNode.children = getWboPropertyNodes(node.id);
                node.children.Add(propParentNode);

                TreeNode methodParentNode = new TreeNode();
                methodParentNode.id = node.id + Umc.MemberSpliter + "method";
                methodParentNode.text = "方法";
                methodParentNode.children = getWboMethodNodes(node.id);
                node.children.Add(methodParentNode);
            }
            return nodes;
        }

        public static List<TreeNode> getWboMethodNodes(string comId)
        {
            List<TreeNode> ret = new List<TreeNode>();
            WboSchema wboSchema = WboSchemaContainer.Instance().GetItem(comId);
            foreach (WboMethodSchema wms in wboSchema.Methods)
            {
                TreeNode node = new TreeNode();
                node.text = string.IsNullOrEmpty(wms.Title) ? wms.Id : wms.Title;
                node.id = comId + Umc.MemberSpliter + wms.Id;
                ret.Add(node);
            }
            return ret;
        }

        public static List<TreeNode> getWboPropertyNodes(string comId)
        {
            List<TreeNode> ret = new List<TreeNode>();
            WboSchema wboSchema = WboSchemaContainer.Instance().GetItem(comId);
            foreach (Schema propSchema in wboSchema.Properties)
            {
                TreeNode node = new TreeNode();
                node.text = string.IsNullOrEmpty(propSchema.Title) ? propSchema.Id : propSchema.Title;
                node.id = comId + Umc.MemberSpliter + propSchema.Id;
                ret.Add(node);
            }
            return ret;
        }


        [WboMethodAttr(Description = "取对象原型", Title = "取对象原型")]
        public static Schema GetWboPrototype(string objectType)
        {
            if (WboSchemaContainer.Instance().Contains(objectType))
            {
                return WboSchemaContainer.Instance().GetItem(objectType);
            }
            return null;
        }

        [WboMethodAttr(Description = "获取对象实例配置", Title = "获取对象实例配置")]
        public Schema GetObjectSchema(string objectType, string objectId)
        {
            List<Wbo> ret = new List<Wbo>();

            WboSchema os = WboSchemaContainer.Instance().GetItem(objectType);

            if (String.IsNullOrEmpty(os.ContainterType))
                throw new XException("对象配置容器ContainterType没有指定，不能获得" + objectType + "的配置文件");

            Type t1 = Type.GetType(os.ContainterType);
            Type t = t1.BaseType;

            MethodInfo m = t.GetMethod("Instance", BindingFlags.Static | BindingFlags.Public);

            //     t.InvokeMember("Instance", BindingFlags.InvokeMethod | BindingFlags.Static, null, null, null, null);
            object containerIns = m.Invoke(null, null);


            //  SchemaContainer<Schema> objContainer = (containerIns as SchemaContainer<Schema>);

            MethodInfo GetItem = t.GetMethod("GetItem", BindingFlags.Public | BindingFlags.Instance);


            Schema schema = (Schema)GetItem.Invoke(containerIns, new object[] { objectId });

            return schema;
        }

        [WboMethodAttr(Description = "获取取对象实例摘要列表", Title = "取对象配置列表")]
        public List<WboInfo> GetObjectList(string objectType)
        {

            List<WboInfo> ret = new List<WboInfo>();
            WboSchema os = WboSchemaContainer.Instance().GetItem(objectType);

            if (String.IsNullOrEmpty(os.ContainterType))
            {
                WboInfo objSum = new WboInfo();
                objSum.name = "";
                objSum.title = os.Title;
                objSum.comId = objectType;
                ret.Add(objSum);
                return ret;
            }


            Type t1 = Type.GetType(os.ContainterType);
            if (t1 == null)
                throw new XException("不能重程序中加载类型，请检查配置：" + os.ContainterType);
            Type t = t1.BaseType;

            MethodInfo m = t.GetMethod("Instance", BindingFlags.Static | BindingFlags.Public);

            //     t.InvokeMember("Instance", BindingFlags.InvokeMethod | BindingFlags.Static, null, null, null, null);
            object containerIns = m.Invoke(null, null);


            //  SchemaContainer<Schema> objContainer = (containerIns as SchemaContainer<Schema>);

            MethodInfo GetSchemaIds = t.GetMethod("GetSchemaIds", BindingFlags.Public | BindingFlags.Instance);
            MethodInfo GetItem = t.GetMethod("GetItem", BindingFlags.Public | BindingFlags.Instance);

            object oObjIds = GetSchemaIds.Invoke(containerIns, null);
            string[] objIds = (oObjIds as string[]);
            //objContainer.GetSchemaIds();


            for (int i = 0; i < objIds.Length; i++)
            {
                Schema s = (Schema)GetItem.Invoke(containerIns, new object[] { objIds[i] });
                WboInfo objSum = new WboInfo();
                objSum.name = s.Id;
                objSum.title = s.Title;
                if (string.IsNullOrEmpty(objSum.title))
                    objSum.title = s.Id;
                objSum.description = s.Description;
                objSum.comId = objectType;
                ret.Add(objSum);
            }


            return ret;
        }

        [WboMethodAttr(Description = "获取对象类型", Title = "获取注册对象类型")]
        public static List<Wbo> GetObjectTypes()
        {
            List<Wbo> ret = new List<Wbo>();
            string[] ids = WboSchemaContainer.Instance().GetSchemaIds();
            for (int i = 0; i < ids.Length; i++)
            {
                string id = ids[i];
                WboSchema os = WboSchemaContainer.Instance().GetItem(id);
                if (os.IsPublish)
                {
                    Wbo objSum = new Wbo();
                    objSum.name = os.Id;
                    objSum.title = os.Title;
                    objSum.description = os.Description;
                    ret.Add(objSum);
                }
            }
            return ret;
        }

        /// <summary>
        /// 返回注册到系统的所有可视化控件的摘要信息列表
        /// </summary>
        /// <returns></returns>
        [WboMethodAttr(Description = "返回注册到系统的所有可视化控件的摘要信息列表", Title = "获取可视化控件列表")]
        public static List<Wbo> GetVboTypes()
        {
            List<Wbo> ret = new List<Wbo>();
            string[] ids = WboSchemaContainer.Instance().GetSchemaIds();
            for (int i = 0; i < ids.Length; i++)
            {
                string id = ids[i];
                WboSchema os = WboSchemaContainer.Instance().GetItem(id);
                if (os.IsPublish && os.IsVisual)
                {
                    Wbo objSum = new Wbo();
                    objSum.name = os.Id;
                    objSum.title = os.Title;
                    objSum.description = os.Description;
                    ret.Add(objSum);
                }
            }
            return ret;
        }

    }

}
