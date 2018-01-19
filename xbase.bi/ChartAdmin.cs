using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using xbase.bi.schema;
using xbase.tree;

namespace xbase.bi
{
    public class ChartAdmin
    {
        public Dictionary<string, string> GetChartTypes()
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            ChartType[] values = (ChartType[])Enum.GetValues(typeof(ChartType));

            for (int i = 0; i < values.Length; i++)
            {
                ret.Add((int)values[i] + "", values[i].ToString());
            }
            return ret;

        }

        public Dictionary<string, string> GetMarkerStyles()
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            MarkerStyle[] values = (MarkerStyle[])Enum.GetValues(typeof(MarkerStyle));

            for (int i = 0; i < values.Length; i++)
            {
                ret.Add((int)values[i] + "", values[i].ToString());
            }
            return ret;
        }

        public xbase.SchemaObjectBreif[] GetChartList()
        {
            //            Dictionary<string, string> ret= new Dictionary<string, string>();
            return ChartShemaContainer.Instance().GetObjectBreifByFolder("");
            //            return ret;
        }

        public List<TreeNode> getChartTree()
        {
            xbase.SchemaObjectBreif[] chartList = GetChartList();
            List<TreeNode> ret = new List<TreeNode>();
            int i = 1;
            foreach (SchemaObjectBreif sob in chartList)
            {
                TreeNode node = new TreeNode();
                node.id = i+"";
                node.label = sob.Title;
                node.name = sob.Title;
                ret.Add(node);
                i++;
            }
            return ret;
        }

    }
}
