using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.tree;
using xbase.data.db;
using System.Data;
using System.Data.Common;
using xbase.data.easyui;
using xbase.data;

namespace xbase.olap
{
    public class Hierarchy
    {
        private string _name;
        private string _hasAll = "true";
        private string _primaryKey = "";

        private Table _table = new Table();
        private List<Level> _levels = new List<Level>();

        /// <summary>
        /// 层次结构的名称，如果未指定则使用维度名称
        /// </summary>
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Table table
        {
            get { return _table; }
            set { _table = value; }
        }

        public List<Level> levels
        {
            get { return _levels; }
            set { _levels = value; }
        }
        /// <summary>
        /// table 的主键
        /// </summary>
        public string primaryKey
        {
            get { return _primaryKey; }
            set { _primaryKey = value; }
        }

        public string hasAll
        {
            get { return _hasAll; }
            set { _hasAll = value; }
        }

        public List<TreeNode> drillMembers()
        {
            return drillMembers(null);
        }

        private List<TreeNode> drillMembersParentChild(List<string> memberPath)
        {
            return null;
        }
        private List<TreeNode> drillMembersComm(List<string> memberPath)
        {
            if (levels == null || levels.Count < 1) throw new NoDefineLevelException();


            StringBuilder sbSql = new StringBuilder();

            int memberPathCount;
            StringBuilder sbWhere = getDrillWhere(memberPath, out memberPathCount);
            if (memberPathCount > levels.Count - 1) throw new OlapLevelOverException();

            string drillField = levels[memberPathCount].column;
            sbSql.Append(" Select ");
            sbSql.Append(drillField);
            sbSql.Append(" From ");
            sbSql.Append(table.name);
            if (sbWhere.Length > 0)
            {
                sbSql.Append(" Where ");
                sbSql.Append(sbWhere);
            }
            sbSql.Append(" Group By ");


            sbSql.Append(drillField);



            DatabaseAdmin dba = DatabaseAdmin.getInstance(_table.connection);
            DataTable tb = dba.executeTable(sbSql.ToString());
            List<TreeNode> nodes = new List<TreeNode>();

            foreach (DataRow row in tb.Rows)
            {
                TreeNode node = new TreeNode();
                node.text = row[drillField].ToString();
                node.attr.Add("memberField", drillField);
                if (memberPathCount + 1 < levels.Count)
                {
                    List<string> subMembers = new List<string>();
                    if (memberPath != null && memberPath.Count > 0)
                        subMembers = new List<string>(memberPath);
                    subMembers.Add(node.text);
                    node.children = drillMembers(subMembers);
                }

                nodes.Add(node);

            }
            return nodes;
        }

        public List<TreeNode> drillMembers(List<string> memberPath)
        {
            if (levels.Count == 1 && !string.IsNullOrEmpty(levels[0].parentColumn))
                return drillMembersParentChild(memberPath);
            else
                return drillMembersComm(memberPath);

        }

        private StringBuilder getDrillWhere(List<string> memberPath, out int memberPathCount)
        {
            memberPathCount = 0;
            if (memberPath != null && memberPath.Count > 0)
                memberPathCount = memberPath.Count();

            StringBuilder sbWhere = new StringBuilder();
            for (int i = 0; i < memberPathCount; i++)
            {
                if (i > levels.Count - 1) break;

                string field = levels[i].column;

                sbWhere.Append(" and ");
                sbWhere.Append(field);
                sbWhere.Append("='");
                sbWhere.Append(memberPath[i]);
                sbWhere.Append("'");
            }
            if (sbWhere.Length > 5)
                sbWhere.Remove(0, 5);
            return sbWhere;
        }

        public EasyUiGridData drillTableToGrid(List<string> parentMembers)
        {
            DatabaseAdmin dba = DatabaseAdmin.getInstance(table.connection);
            TableDef tbf = dba.getTableDef(table.name);
            string title = string.IsNullOrEmpty(tbf.Title) ? table.name : tbf.Title;
            EasyUiGridData ret = EUGridUtils.getGrid(table.connection, title, tbf.FieldDefs);
            return ret;
        }

        public ListData drillTable(List<string> parentMembers)
        {
            return drillTable(null, null, parentMembers);
        }

        public ListData drillTable(string table, string fk, List<string> parentMembers)
        {

            if (string.IsNullOrEmpty(table)) table = this.table.name;
            if (string.IsNullOrEmpty(fk)) fk = this.primaryKey;

            EasyUiGridData grid = new EasyUiGridData();
            StringBuilder sql = new StringBuilder();

            //纬度表
            sql.Append("Select ");
            sql.Append(this.primaryKey);
            sql.Append(" Into #t ");
            sql.Append(" From  ");
            sql.Append(this.table.name);
            int memberPathCount;
            StringBuilder sbWhere = getDrillWhere(parentMembers, out memberPathCount);
            if (sbWhere.Length > 0)
            {
                sql.Append(" Where ");
                sql.Append(sbWhere);
            }
            sql.Append(" Group By ");
            sql.Append(primaryKey);
            sql.Append(";");

            //事实表
            sql.Append(" Select ");
            sql.Append(table);
            sql.Append(".* From ");
            sql.Append(table);
            sql.Append(",");
            sql.Append("#t");
            sql.Append(" Where ");
            sql.Append(table);
            sql.Append(".");
            sql.Append(fk);
            sql.Append("=#t.");
            sql.Append(primaryKey);


            ListData ret = new ListData();

            DatabaseAdmin dba = DatabaseAdmin.getInstance(this.table.connection);
            DataTable tb = dba.executeTable(sql.ToString());

            ret.total = tb.Rows.Count;
            ret.rows = new List<ListDataRow>();
            foreach (DataRow dRow in tb.Rows)
            {
                ListDataRow row = new ListDataRow();
                foreach (DataColumn col in tb.Columns)
                {
                    row.Add(col.ColumnName, dRow[col].ToString());
                }
                ret.rows.Add(row);
            }
            return ret;
        }


    }
}
