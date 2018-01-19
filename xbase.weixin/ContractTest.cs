using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using xbase.weixin.contract;

namespace xbase.weixin
{
    public static class ContractTest
    {
        const string orderReq = @"<xml><spbill_create_ip><![CDATA[8.8.8.8]]></spbill_create_ip>
<notify_url><![CDATA[www.weiweihi.com]]></notify_url>
<out_trade_no><![CDATA[1833431771763549]]></out_trade_no>
<appid><![CDATA[wxbe855a981c34aa3f]]></appid>
<total_fee>1</total_fee>
<nonce_str><![CDATA[D554F7BB7BE44A7267068A7DF88DDD20]]></nonce_str>
<sign><![CDATA[4A3E3CF7205C9A319010ABE2A49F4F65]]></sign>
<openid><![CDATA[o3IHxjrPzMVZIJOgYMH1PyoTW_Tg]]></openid>
<mch_id><![CDATA[10017762]]></mch_id>
<trade_type><![CDATA[JSAPI]]></trade_type>
<body><![CDATA[test]]></body>
</xml>";

        private static string SeriObj(object obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializer xmlSer = new XmlSerializer(obj.GetType());
                XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
                xmlns.Add(string.Empty, string.Empty);
                StreamWriter writer = new StreamWriter(stream, Encoding.GetEncoding("utf-8"));
                xmlSer.Serialize(writer, obj, xmlns);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        private static T DeSeriObj<T>(string xml)
        {
            using (MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml)))
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(T));
                StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                return (T)xmlSer.Deserialize(reader);
            }
        }

        public static string getOrderReq()
        {
            OrderReq op = new OrderReq();
            op.spbill_create_ip = "192.168.111";
            op.notify_url = "http://";
            op.out_trade_no = "aaa";
            string s = XmlConvert.getXml(op);
            return s;
        }

        public static string getSign()
        {
            OrderReq op = new OrderReq();
            op.spbill_create_ip = "192.168.111";
            op.notify_url = "http://";
            op.out_trade_no = "aaa";
            op.total_fee = 33;
            op.openid = "asdfasdf";
            op.appid = "xxxxxxx";

            return WeiXinUtils.getSign(op, "abcd3dafasdfadf");
        }

        public static OrderReq getOrderReqJson()
        {
            return XmlConvert.getObj<OrderReq>(orderReq);
        }



    }
}
