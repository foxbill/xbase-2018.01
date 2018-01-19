using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using xbase.tree;
using xbase.data.db;
using xbase.Exceptions;

namespace xbase.data
{
    /// <summary>
    /// 节点常量
    /// </summary>
    public static class DataNodeLabel
    {
        public static string VIEW_LABEL = "视图";
        public static string TABLE_LABEL = "表";
        public static string SP_LABEL = "存储过程";
        public static string CONSTRAINT_LABEL = "约束";
        public static string TRIGGER_LABEL = "触发器";
    }

    /// <summary>
    /// 获取树节点
    /// </summary>
    public static class DataExplore
    {
        public static List<TreeNode> getTree()
        {
            List<TreeNode> nodes = new List<TreeNode>();

            DataTable tb = ConnectionAdmin.getAllConnInfoTable();

            foreach (DataRow row in tb.Rows)
            {
                string conn = row["Name"].ToString();

                TreeNode connNode = new TreeNode()
                {
                    nodeType = (int)DataNodeType.DB,
                    label = conn,
                    name = conn,
                    text = conn,
                    attr = new Dictionary<string, string>() { { "conn", conn } },
                    id = conn
                };
                nodes.Add(connNode);

                connNode.children = new List<TreeNode>();
                List<TreeNode> subNodes = connNode.children;

                DatabaseAdmin db = null;
                try
                {
                    db = DatabaseAdmin.getInstance(conn);
                }
                catch (Exception e)
                {
                    TreeNode errNode = new TreeNode();
                    errNode.name = "连接错误:" + e.Message;
                    errNode.label = errNode.name;
                    subNodes.Add(errNode);
                }
                if (db != null)
                {
                    //string connName;
                    List<TreeNode> tbNodes = new List<TreeNode>();
                    List<TreeNode> viewNodes = new List<TreeNode>();
                    List<TreeNode> spNodes = new List<TreeNode>();
                    TreeNode tbNode = new TreeNode();
                    tbNode.nodeType = (int)DataNodeType.TABLE_FOLDER;
                    tbNode.label = DataNodeLabel.TABLE_LABEL;
                    tbNode.name = DataNodeLabel.TABLE_LABEL;
                    tbNode.attr = new Dictionary<string, string>() { { "conn", conn } };
                    tbNode.id = "Table";
                    tbNode.text = DataNodeLabel.TABLE_LABEL;
                    tbNode.children = tbNodes;
                    //Table节点
                    subNodes.Add(tbNode);

                    //获取Table

                    List<string> tbList = null;
                    try
                    {
                        tbList = db.getTableNames();
                    }
                    catch (Exception e)
                    {
                        tbNode.label = e.Message;
                        continue;
                    }

                    if (tbList == null)
                        continue;

                    foreach (string t in tbList)
                    {
                        List<TreeNode> tbChildNode = new List<TreeNode>();
                        List<TreeNode> trigNode = new List<TreeNode>();
                        List<TreeNode> contriantNode = new List<TreeNode>();

                        //表
                        tbNodes.Add(new TreeNode()
                        {
                            nodeType = (int)DataNodeType.TABLE,
                            label = t,
                            name = t,
                            text = t,
                            attr = new Dictionary<string, string>() { { "conn", conn } },
                            id = t,
                            children = tbChildNode
                        });
                    }



                    //存储过程
                    subNodes.Add(new TreeNode()
                    {
                        attr = new Dictionary<string, string>() { { "conn", conn } },
                        nodeType = (int)DataNodeType.SP_FOLDER,
                        label = DataNodeLabel.SP_LABEL,
                        name = DataNodeLabel.SP_LABEL,
                        text=DataNodeLabel.SP_LABEL,
                        id = "StoreProcess",
                        children = spNodes
                    });

                    //存储过程
                    List<string> spList = db.getProcNames();
                    if (spList != null)
                    {
                        foreach (string sp in spList)
                        {
                            spNodes.Add(new TreeNode()
                            {
                                attr = new Dictionary<string, string>() { { "conn", conn } },
                                nodeType = (int)DataNodeType.SP,
                                id =  sp,
                                name = sp,
                                text=sp,
                                label = sp
                            });
                        }

                    }

                    //视图
                    subNodes.Add(new TreeNode()
                    {
                        attr = new Dictionary<string, string>() { { "conn", conn } },
                        nodeType = (int)DataNodeType.VIEW_FOLDER,
                        label = DataNodeLabel.VIEW_LABEL,
                        name = DataNodeLabel.VIEW_LABEL,
                        id = "View",
                        text = DataNodeLabel.VIEW_LABEL,
                        children = viewNodes
                    });

                    //视图
                    List<string> viewList = db.getViewNames();
                    if (viewList != null)
                    {
                        foreach (string view in viewList)
                        {
                            viewNodes.Add(new TreeNode()
                            {
                                attr = new Dictionary<string, string>() { { "conn", conn } },
                                label = view,
                                name = view,
                                id = view,
                                text=view,
                                nodeType = (int)DataNodeType.VIEW
                            });
                        }
                    }

                }
            }



            return nodes;
        }

        public static List<string> getConstraintNames(string connName, string tableName)
        {
            DatabaseAdmin da = DatabaseAdmin.getInstance(connName);
            DataTable dt = da.getConstraintTable(tableName);
            List<string> ret = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                ret.Add(row[0].ToString());
            }
            return ret;
        }

        public static string getConstraint(string connName, string tableName, string consName)
        {
            DatabaseAdmin da = DatabaseAdmin.getInstance(connName);
            return da.getConstraintText(consName, tableName);
        }

        public static ConnDef getConnDef(string connName)
        {
            return ConnectionAdmin.getConnDef(connName);
        }

        public static void saveConnDef(ConnDef connDef)
        {
            ConnectionAdmin.modifyConnection(connDef.Name, connDef.ConnStr, connDef.Provider);
        }

        public static TableDef getTableDef(string connName, string tableName)
        {
            DatabaseAdmin da = DatabaseAdmin.getInstance(connName);
            return da.getTableDef(tableName);
        }

        public static void modifyTable(string connName, TableDef tableDef)
        {
            DatabaseAdmin da = DatabaseAdmin.getInstance(connName);
            da.modifyTable(tableDef);
        }

        public static TableDef createTable(string tableName)
        {
            TableDef ret = new TableDef();
            ret.Name = tableName;
            FieldDef fieldDef = new FieldDef();
            fieldDef.Type = "nvarchar";
            fieldDef.Length = 50;

            ret.FieldDefs.Add(fieldDef);
            return ret;
        }

        public static List<string> getTriggerNames(string connName, string tableName)
        {
            DatabaseAdmin da = DatabaseAdmin.getInstance(connName);
            return da.getTriggerNames(tableName);
        }

        public static string getTrigger(string connName, string tableName, string triggerName)
        {
            DatabaseAdmin da = DatabaseAdmin.getInstance(connName);
            return da.getTriggerText(tableName, triggerName);
        }

        public static void modifyTrigger(string connName, string tableName, string triggerName, string triggerText)
        {
            DatabaseAdmin da = DatabaseAdmin.getInstance(connName);
            da.modifyTrigger(tableName, triggerName, triggerText);
        }

        public static string getViewScript(string connName, string viewName)
        {
            DatabaseAdmin da = DatabaseAdmin.getInstance(connName);
            return da.getViewScript(viewName);
        }

        public static void modifyViewScript(string connName, string viewName, string script)
        {
            DatabaseAdmin da = DatabaseAdmin.getInstance(connName);
            da.modifyViewScript(viewName, script);
        }

        public static string getSP(string connName, string spName)
        {
            DatabaseAdmin da = DatabaseAdmin.getInstance(connName);
            return da.getProcText(spName);
        }

        public static void modifySP(string connName, string spName, string script)
        {
            DatabaseAdmin da = DatabaseAdmin.getInstance(connName);
            da.modifyProc(spName, script);
        }

        public static Dictionary<string, string> getFieldTypes(string connName)
        {
            DatabaseAdmin da = DatabaseAdmin.getInstance(connName);
            return da.getFieldTypeList();
        }

        /// <summary>
        /// 删除字段
        /// </summary>
        /// <param name="connName">连接名</param>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">字段名</param>
        public static void deleteField(string connName, string tableName, string fieldName)
        {
            DatabaseAdmin da = DatabaseAdmin.getInstance(connName);
            da.deleteField(tableName, fieldName);
        }

        /// <summary>
        ///  删除约束
        /// </summary>
        /// <param name="connName">连接名</param>
        /// <param name="tableName">表名</param>
        /// <param name="ctrName">约束名</param>
        public static void deleteConstraint(string connName, string tableName, string ctrName)
        {
            DatabaseAdmin da = DatabaseAdmin.getInstance(connName);
            da.deleteConstraint(tableName, ctrName);
        }

        public static void deleteTrigger(string connName, string triggerName)
        {
            DatabaseAdmin da = DatabaseAdmin.getInstance(connName);
            da.deleteTrigger(triggerName);
        }


        public static Dictionary<string, string> getConnections()
        {
            List<string> conns = ConnectionAdmin.getConnNameList();
            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret.Add("", "默认");
            for (int i = 0; i < conns.Count(); i++)
            {
                string conn = conns[i];
                ret.Add(conn, conn);
            }
            return ret;
        }

        /// <summary>
        /// 删除连接
        /// </summary>
        /// <param name="connName">连接名</param>
        public static void deleteConn(string connName)
        {
            ConnectionAdmin.deleteConnection(connName);
        }

        public static Dictionary<string, string> getTables()
        {
            return getTables(null);
        }


        public static Dictionary<string, string> getTables(string connName)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            DatabaseAdmin dba = DatabaseAdmin.getInstance(connName);
            List<string> tables = dba.getTableNames();
            foreach (string table in tables)
            {
                string text = dba.getTableTitle(table);
                if (string.IsNullOrEmpty(text))
                    text = table;
                ret.Add(table, text);
            }
            return ret;
        }

        public static Dictionary<string, string> getFields(string connName, string tableName)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(tableName))
                return ret;
            DatabaseAdmin dba = DatabaseAdmin.getInstance(connName);
            TableDef tbDef = dba.getTableDef(tableName);

            ret.Add("", "");
            foreach (FieldDef field in tbDef.FieldDefs)
            {
                string title = string.IsNullOrEmpty(field.Title) ? field.Name : field.Title;
                ret.Add(field.Name, title);
            }
            return ret;
        }

        public static string getPrimaryKey(string connName, string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                return "";
            DatabaseAdmin dba = DatabaseAdmin.getInstance(connName);
            TableDef tbDef = dba.getTableDef(tableName);
            return tbDef.MainKeys[0].Name;
        }

    }
}

