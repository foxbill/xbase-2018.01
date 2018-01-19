using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wbs.wbap
{
    /// <summary>
    /// 返回jsLookup格式
    /// </summary>
    public class JsLookup
    {
        private List<string> keyFields = new List<string>();
        private Dictionary<string, string> map = new Dictionary<string, string>();
        private WbapList list = new WbapList();
        /// <summary>
        /// Hash结构
        /// </summary>
        public Dictionary<string, string> Map
        {
            get { return map; }
            set { map = value; }
        }
        /// <summary>
        /// 关键字列表
        /// </summary>
        public List<string> KeyFields
        {
            get { return keyFields; }
            set { keyFields = value; }
        }
        /// <summary>
        /// 列表数据
        /// </summary>
        public WbapList List
        {
            get { return list; }
            set { list = value; }
        }
    }
}
