using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace xbase.weixin.contract
{
    [XmlRoot("xml")]
    public class OrderReq:WxBaseReq
    {
        [XmlIgnore]
        public string spbill_create_ip { get; set; }
        [XmlElement("spbill_create_ip")]
        public XmlNode spbill_create_ip_cdata { get { return spbill_create_ip.CDATA(); } set { spbill_create_ip = value.InnerText; } }

        [XmlIgnore]
        public string notify_url { get; set; }
        [XmlElement("notify_url")]
        public XmlNode notify_url_cdata { get { return notify_url.CDATA(); } set { notify_url = value.InnerText; } }

        /// <summary>
        /// 商户系统的订单号，与通知消息一致返回。
        /// </summary>
        [XmlIgnore]
        public string out_trade_no { get; set; }
        [XmlElement("out_trade_no")]
        public XmlNode out_trade_no_cdata { get { return out_trade_no.CDATA(); } set { out_trade_no = value.InnerText; } }

        [XmlIgnore]
        public string appid { get; set; }
        [XmlElement("appid")]
        public XmlNode appid_cdata { get { return appid.CDATA(); } set { appid = value.InnerText; } }

        /// <summary>
        /// 订单总金额，单位为分，详见支付金额
        /// </summary>
        public int total_fee { get; set; }

        /// <summary>
        /// 符合ISO 4217标准的三位字母代码，默认人民币：CNY，其他值列表详见货币类型
        /// </summary>
        [XmlIgnore]
        public string fee_type { get; set; }
        [XmlElement("fee_type")]
        public XmlNode fee_type_cdata { get { return fee_type.CDATA(); } set { fee_type = value.InnerText; } }

        [XmlIgnore]
        public string nonce_str { get; set; }
        [XmlElement("nonce_str")]
        public XmlNode nonce_str_cdata { get { return nonce_str.CDATA(); } set { nonce_str = value.InnerText; } }


        [XmlIgnore]
        public string sign { get; set; }
        [XmlElement("sign")]
        public XmlNode sign_cdata { get { return sign.CDATA(); } set { sign = value.InnerText; } }


        [XmlIgnore]
        public string openid { get; set; }
        [XmlElement("openid")]
        public XmlNode openid_cdata { get { return openid.CDATA(); } set { openid = value.InnerText; } }

        [XmlIgnore]
        public string mch_id { get; set; }
        [XmlElement("mch_id")]
        public XmlNode mch_id_cdata { get { return mch_id.CDATA(); } set { mch_id = value.InnerText; } }

        /// <summary>
        /// 取值如下：JSAPI，NATIVE，APP，详细说明见参数规定
        /// </summary>
        [XmlIgnore]
        public string trade_type { get; set; }
        [XmlElement("trade_type")]
        public XmlNode trade_type_cdata { get { return trade_type.CDATA(); } set { trade_type = value.InnerText; } }

        
        [XmlIgnore]
        public string body { get; set; }
        [XmlElement("body")]
        public XmlNode body_cdata { get { return body.CDATA(); } set { body = value.InnerText; } }
    }
}
