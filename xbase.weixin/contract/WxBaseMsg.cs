using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace xbase.weixin.contract
{

    /// <summary>
    /// 微信回复结果基本信息
    /// </summary>
    public class WxBaseMsg
    {
        /// <summary>
        /// 返回： SUCCESS/FAIL
        /// 此字段是通信标识，非交易标识，交易是否成功需要查看result_code来判断
        /// </summary>
        [XmlIgnore]
        public string return_code { get; set; }
        [XmlElement("return_code")]
        public XmlNode return_code_cdata { get { return return_code.CDATA(); } set { return_code = value.InnerText; } }


        /// <summary>
        ///返回信息，如非空，为错误原因
        ///签名失败
        ///参数格式校验错误
        /// </summary>
        [XmlIgnore]
        public string return_msg { get; set; }
        [XmlElement("return_msg")]
        public XmlNode return_msg_cdata { get { return return_msg.CDATA(); } set { return_msg = value.InnerText; } }



        public string toXml()
        {
            return XmlConvert.getXml(this);
        }
    }
}
