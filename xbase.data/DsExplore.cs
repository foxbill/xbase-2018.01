using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.tree;
using System.Data;

namespace xbase.data
{
    public static class DsExplore
    {
        private static SchemaContainer<DataSourceSchema> container = DataSourceSchemaContainer.Instance();
        private static void loadSubNodes(List<TreeNode> subNodes, string path, ref int id)
        {
            string[] folders = container.GetSchemaFolders(path);

            for (int i = 0; i < folders.Length; i++)
            {
                string subFolder = folders[i];
                TreeNode node = new TreeNode();
                node.label = subFolder;
                node.path = path;
                node.nodeType = (int)DsExploreNodeType.folder;
                node.id = id + "";
                node.text = subFolder;
                subNodes.Add(node);
                id++;
                loadSubNodes(node.children, path + "\\" + subFolder, ref id);
            }
            string[] dataSources = container.GetIDsByFolder(path);
            for (int i = 0; i < dataSources.Count(); i++)
            {
                string dsId = dataSources[i];
                DataSourceSchema ts = container.GetItem(path + "\\" + dsId);
                TreeNode node = new TreeNode();
                node.text = ts.Title + "(" + dsId + ")";
                node.label = node.text;
                node.title = node.text;
                if (string.IsNullOrEmpty(node.label))
                    node.label = dsId;
                node.name = node.text;
                node.id = dsId;
                node.path = path;
                node.nodeType = (int)DsExploreNodeType.dataSource;
                //node.id = id + "";
                subNodes.Add(node);
                id++;
            }


        }

        public static List<TreeNode> getTree()
        {
            int id = 1;
            List<TreeNode> ret = new List<TreeNode>();
            loadSubNodes(ret, "", ref id);
            return ret;
        }

        public static Dictionary<string, string> getDsNames()
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            string[] dataSources = container.GetSchemaIds();
            for (int i = 0; i < dataSources.Count(); i++)
            {
                string dsId = dataSources[i];
                DataSourceSchema ts = container.GetItem(dsId);
                string text = ts.Title;
                if (string.IsNullOrEmpty(text))
                    text = dsId;
                ret.Add(dsId, text);
            }
            //ret
            Dictionary<string, string> tables = DataExplore.getTables();
            foreach (string key in tables.Keys)
            {
                ret.Add(key, tables[key]);
            }
            
            return ret;
        }


        public static DataSourceSchema buildTableSchema(string connName, string tableName, string dsId)
        {
            DataSourceSchema ret = DataSourceSchemaBuilder.BuildTableSchema(connName, tableName);
            DataSourceSchemaContainer.Instance().AddItem(dsId, ret);
            return ret;
        }

        public static DataSourceSchema getDsSchema(string dsId)
        {
            return container.GetItem(dsId);
        }

        public static void updateDsSchema(string dsId, DataSourceSchema dss)
        {
            container.UpdateItem(dsId, dss);
        }

        public static void deleteDs(string dsId)
        {
            container.DeleteItem(dsId);
        }

        public static List<ValueTextPair<int>> getDbTypes()
        {
            return DbTypeCaptions.valueTextPairs();
        }

        public static List<ValueTextPair<int>> getParameterDirections()
        {
            return ParameterDirectionCaptions.valueTextPairs();
        }

        public static Dictionary<int, string> getDataExtandTypes()
        {
            return new Dictionary<int, string>
            { 
                {(int)DataExtendType.None,"无"},
                {(int)DataExtendType.File,"未知文件"},
                {(int)DataExtendType.ImageFile,"图像"},
                {(int)DataExtendType.HtmlFile,"网页文件"},
                {(int)DataExtendType.Html,"超文本"}
              };
        }

    }
}
