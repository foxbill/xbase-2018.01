using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace xbase.weixin.contract
{
    public class OrderNotify : OrderReq
    {
        /// <summary>
        /// 银行类型，采用字符串类型的银行标识，银行类型见银行列表
        /// </summary>
        [XmlIgnore]
        public string bank_type { get; set; }
        [XmlElement("bank_type")]
        public XmlNode bank_type_cdata { get { return bank_type.CDATA(); } set { bank_type = value.InnerText; } }


        /// <summary>
        /// 用户是否关注公众账号，Y-关注，N-未关注，仅在公众账号类型支付有效
        /// </summary>
        [XmlIgnore]
        public string is_subscribe { get; set; }
        [XmlElement("is_subscribe")]
        public XmlNode is_subscribe_cdata { get { return is_subscribe.CDATA(); } set { is_subscribe = value.InnerText; } }


        /// <summary>
        /// 商户系统的订单号，与请求(OrderReq)一致。
        /// </summary>
        [XmlIgnore]
        public string out_trade_no { get; set; }
        [XmlElement("out_trade_no")]
        public XmlNode out_trade_no_cdata { get { return out_trade_no.CDATA(); } set { out_trade_no = value.InnerText; } }

        /// <summary>
        /// 订单总金额，单位为分
        /// </summary>
        public int total_fee { get; set; }

        /// <summary>
        /// 货币类型，符合ISO4217标准的三位字母代码，默认人民币：CNY，其他值列表详见货币类型
        /// </summary>
        [XmlIgnore]
        public string fee_type { get; set; }
        [XmlElement("fee_type")]
        public XmlNode fee_type_cdata { get { return fee_type.CDATA(); } set { fee_type = value.InnerText; } }

        /// <summary>
        /// 现金支付金额订单现金支付金额，详见支付金额
        /// </summary>
        public int cash_fee { get; set; }

        /// <summary>
        /// 货币类型，符合ISO4217标准的三位字母代码，默认人民币：CNY，其他值列表详见货币类型
        /// </summary>
        [XmlIgnore]
        public string cash_fee_type { get; set; }
        [XmlElement("cash_fee_type")]
        public XmlNode cash_fee_type_cdata { get { return cash_fee_type.CDATA(); } set { cash_fee_type = value.InnerText; } }

        /// <summary>
        /// 代金券或立减优惠金额=订单总金额，订单总金额-代金券或立减优惠金额=现金支付金额，详见支付金额
        /// </summary>
        public int coupon_fee { get; set; }

        /// <summary>
        /// 代金券或立减优惠使用数量
        /// </summary>
        public int coupon_count { get; set; }


        /// <summary>
        /// 微信支付订单号
        /// </summary>
        [XmlIgnore]
        public string transaction_id { get; set; }
        [XmlElement("transaction_id")]
        public XmlNode transaction_id_cdata { get { return transaction_id.CDATA(); } set { transaction_id = value.InnerText; } }


        /// <summary>
        /// 商家数据包，原样返回
        /// </summary>
        [XmlIgnore]
        public string attach { get; set; }
        [XmlElement("attach")]
        public XmlNode attach_cdata { get { return attach.CDATA(); } set { attach = value.InnerText; } }


        /// <summary>
        /// 支付完成时间，格式为yyyyMMddHHmmss，如2009年12月25日9点10分10秒表示为20091225091010。其他详见时间规则
        /// </summary>
        [XmlIgnore]
        public string time_end { get; set; }
        [XmlElement("time_end")]
        public XmlNode time_end_cdata { get { return time_end.CDATA(); } set { time_end = value.InnerText; } }


    }
}
