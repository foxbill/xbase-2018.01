using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xbase.tree
{
    public class TreeNode
    {
        /// <summary>
        /// 链接框架名称
        /// </summary>
        public string target { get; set; }

        public Dictionary<string, string> attr = new Dictionary<string, string>();

        private List<string> _pathList;

        public List<string> pathList
        {
            get { return _pathList; }
            set { _pathList = value; }
        }

        private string _pkValue;

        public string pkValue
        {
            get { return _pkValue; }
            set { _pkValue = value; }
        }

        string _id;
        /// <summary>
        /// 节点id
        /// </summary>
        public string id
        {
            get { return _id; }
            set { _id = value; }
        }

        string _url;
        /// <summary>
        /// 节点Url
        /// </summary>
        public string url
        {
            get { return _url; }
            set { _url = value; }
        }



        string _label;
        /// <summary>
        /// 节点label
        /// </summary>
        public string label
        {
            get { return _label; }
            set { _label = value; }
        }

        public string name { get; set; }
        public string path { get; set; }

        List<TreeNode> _children = new List<TreeNode>();
        /// <summary>
        /// 子节点
        /// </summary>
        public List<TreeNode> children
        {
            get { return _children; }
            set { _children = value; }
        }


        int _nodeType;
        /// <summary>
        /// 数据库类型
        /// </summary>
        public int nodeType
        {
            get { return _nodeType; }
            set { _nodeType = value; }
        }

        public string title { get; set; }

        public string text { get; set; }
        public string state { get; set; }

        public string iconCls { get; set; }
    }
}
