using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace xbase.weixin.contract
{

    /// <summary>
    ///统一下单返回结果
    /// </summary>
    [XmlRoot("xml")]
    public class OrderResp : WxBaseResp
    {
        /// <summary>
        /// 微信分配的公众账号ID（企业号corpid即为此appId）
        /// </summary>
        [XmlIgnore]
        public string appid { get; set; }
        [XmlElement("appid")]
        public XmlNode appid_cdata { get { return appid.CDATA(); } set { appid = value.InnerText; } }

        /// <summary>
        /// 微信支付分配的商户号
        /// </summary>
        [XmlIgnore]
        public string mch_id { get; set; }
        [XmlElement("mch_id")]
        public XmlNode mch_id_cdata { get { return mch_id.CDATA(); } set { mch_id = value.InnerText; } }

        /// <summary>
        /// 调用接口提交的终端设备号
        /// </summary>
        [XmlIgnore]
        public string device_info { get; set; }
        [XmlElement("device_info")]
        public XmlNode device_info_cdata { get { return device_info.CDATA(); } set { device_info = value.InnerText; } }

        /// <summary>
        /// 随机字符串，不长于32位
        /// </summary>
        [XmlIgnore]
        public string nonce_str { get; set; }
        [XmlElement("nonce_str")]
        public XmlNode nonce_str_cdata { get { return nonce_str.CDATA(); } set { nonce_str = value.InnerText; } }

        /// <summary>
        /// 微信返回的签名，详见签名算法
        /// </summary>
        [XmlIgnore]
        public string sign { get; set; }
        [XmlElement("sign")]
        public XmlNode sign_cdata { get { return sign.CDATA(); } set { sign = value.InnerText; } }



        /// <summary>
        ///调用接口提交的交易类型，取值如下：JSAPI，NATIVE，APP，详细说明见参数规定
        /// </summary>
        [XmlIgnore]
        public string trade_type { get; set; }
        [XmlElement("trade_type")]
        public XmlNode trade_type_cdata { get { return trade_type.CDATA(); } set { trade_type = value.InnerText; } }

        /// <summary>
        /// 微信生成的预支付回话标识，用于后续接口调用中使用，该值有效期为2小时
        /// </summary>
        [XmlIgnore]
        public string prepay_id { get; set; }
        [XmlElement("prepay_id")]
        public XmlNode prepay_id_cdata { get { return prepay_id.CDATA(); } set { prepay_id = value.InnerText; } }

        /// <summary>
        /// trade_type为NATIVE是有返回，可将该参数值生成二维码展示出来进行扫码支付
        /// </summary>
        [XmlIgnore]
        public string code_url { get; set; }
        [XmlElement("code_url")]
        public XmlNode code_url_cdata { get { return code_url.CDATA(); } set { code_url = value.InnerText; } }

    }
}
