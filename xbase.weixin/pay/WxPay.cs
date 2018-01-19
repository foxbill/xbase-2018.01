using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xbase.Exceptions;
using xbase.weixin.contract;

namespace xbase.weixin.pay
{
    public class WxPay : HttpWbo
    {

        /// <summary>
        /// 统一下单
        /// </summary>
        /// <returns></returns>
        public OrderResp unifiedOrder(OrderReq req)
        {
            //todo:微信登录
            const string reqUrl = "https://api.mch.weixin.qq.com/pay/unifiedorder";

            string openId = Session["openId"].ToString();
            if (string.IsNullOrEmpty(openId))
                throw new XException("用户没有登录微信");
            if (string.IsNullOrEmpty(req.out_trade_no))
                throw new XException("商家订单号(out_trade_no)必须提供");
            if (req.total_fee < 1)
                throw new XException("订单金额(total_fee)必须提供");


            req.appid = WxConfigFile.config().AppID;
            req.mch_id = WxConfigFile.config().MchId;
            req.trade_type = TradeType.JSAPI.ToString();
            req.nonce_str = PayUtil.GetNoncestr();
            req.notify_url = WxConfigFile.config().Domain + "/WxPay.orderNotify.call";
            req.openid = openId;
            req.sign = WeiXinUtils.getSign(req, WxConfigFile.config().SignKey);

            return req.send<OrderResp>(reqUrl);
            //            return WeiXinUtils.Post<OrderResp>(req, reqUrl);
            // req.sign = "";
        }

        public void orderNotify()
        {
            OrderNotify notify = XmlConvert.getObj<OrderNotify>(Request.InputStream);
            WxBaseMsg resp = new WxBaseMsg() { return_code = WxReturnCode.FAIL, return_msg = "OK" };
            if (notify.return_code.Equals(WxReturnCode.SUCCESS))
            {
                OrderNotifyPool.addNotify(notify);
                notify.return_code = WxReturnCode.SUCCESS;
                notify.return_code = "Recive notify fail";
                Response.Write(notify.toXml());
            }
            else
            {
                Response.Write(notify.toXml());
            }

        }

        public bool PayOk(string tradeNo)
        {
            return OrderNotifyPool.getNotify("12343214", WxConfigFile.config().AppID, tradeNo) != null;
        }

    }
}
