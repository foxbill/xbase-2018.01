using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace xbase.weixin.contract
{
    public static class XmlUtil
    {
        /// <summary>
        /// 字符串转换为XML CDATA 节点
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static XmlNode CDATA(this string str)
        {
            XmlNode node = new XmlDocument().CreateNode(XmlNodeType.CDATA, "", "");
            node.InnerText = str;
            return node;
        }

        /// <summary>
        /// 整数串转换为XML CDATA 节点
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static XmlNode CDATA(this int val)
        {
            XmlNode node = new XmlDocument().CreateNode(XmlNodeType.CDATA, "", "");
            node.InnerText = val.ToString();
            return node;
        }

    }
}
