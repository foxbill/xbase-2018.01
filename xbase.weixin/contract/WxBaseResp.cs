using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace xbase.weixin.contract
{
    public class WxBaseResp : WxBaseMsg
    {
        /// <summary>
        /// 返回SUCCESS/FAIL
        /// </summary>
        [XmlIgnore]
        public string result_code { get; set; }
        [XmlElement("result_code")]
        public XmlNode result_code_cdata { get { return result_code.CDATA(); } set { result_code = value.InnerText; } }

        /// <summary>
        /// 详细参见第6节错误列表
        /// </summary>
        [XmlIgnore]
        public string err_code { get; set; }
        [XmlElement("err_code")]
        public XmlNode err_code_cdata { get { return err_code.CDATA(); } set { err_code = value.InnerText; } }



        /// <summary>
        /// 错误返回的信息描述
        /// </summary>
        [XmlIgnore]
        public string err_code_des { get; set; }
        [XmlElement("err_code_des")]
        public XmlNode err_code_des_cdata { get { return err_code_des.CDATA(); } set { err_code_des = value.InnerText; } }

    }
}
