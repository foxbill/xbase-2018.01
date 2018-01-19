using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.tree;
using System.Web;
using Newtonsoft.Json;
using xbase.umc;
namespace xbase.admin
{
    public static class WboExplore
    {
        public static List<TreeNode> getWboNodes()
        {
            List<TreeNode> treeNodes = new List<TreeNode>();
            string[] xmls = WboSchemaContainer.Instance().GetIDsByFolder("");
            int nid = 0;
            foreach (string name in xmls)
            {
                WboSchema ws = getWboSchema(name);
                TreeNode tn = new TreeNode();
                tn.id = nid + "";
                nid++;
                tn.label = ws.Title;
                tn.name = name;
                tn.text = ws.Title;
                tn.url = "";
                treeNodes.Add(tn);
            }
            return treeNodes;
        }

        public static WboSchema getWboSchema(string id)
        {
            return (WboSchemaContainer.Instance().GetItem(id));
        }
        public static void saveWboSchema(WboSchema wboSchema)
        {
            if (wboSchema == null)
                return;
            WboProxy wp = WboProxyFactory.getWboProxy(wboSchema);

            if (wp.getWboType().IsSubclassOf(typeof(ISessionWbo)))
                wboSchema.LifeCycle = LifeCycle.Session;

            if (WboSchemaContainer.Instance().Contains(wboSchema.Id))
            {
                WboSchemaContainer.Instance().UpdateItem(wboSchema.Id, wboSchema);
            }
        }
        public static void saveWboSchema(string josn)
        {
            WboSchema wboSchema = (WboSchema)JsonConvert.DeserializeObject(josn, typeof(WboSchema));
            WboExplore.saveWboSchema(wboSchema);
        }

    }
}
