/**
 * -----------
 * 微信支付
 * -----------
 *    
 * @function  wxPay
 *    
 * @param {object} order 订单对象
 * @param {function} fn 支付结果回掉函数
 *
 * Example
 * -----------
 *       wxPay({
 *           out_trade_no: no,       //订单号
 *           total_fee: 1,           //付款金额(分)
 *           body: "测试订单" + no,  //付款说明
 *           attach: "昆明分店"      //附加信息
 *       },
 *       function(res){     //@param {bool} res true支付成功，false支付失败
 *           if(res) 
 *               alter('付款成功');
 *           else
 *              alter('付款失败');
 *       })
 * -----------
 */

function wxPay(order, fn) {

    var param = $.rCall("WxPay.unifiedOrder", { order: order });

    if (param.Err) {
        fn(false);
        return;
    }

    function onBridgeReady() {
        WeixinJSBridge.invoke(
           'getBrandWCPayRequest', param,
            function (res) {
                if (fn)
                    fn(res);
            }
        );
    }
    if (typeof WeixinJSBridge == "undefined") {
        if (document.addEventListener) {
            document.addEventListener('WeixinJSBridgeReady', onBridgeReady, false);
        } else if (document.attachEvent) {
            document.attachEvent('WeixinJSBridgeReady', onBridgeReady);
            document.attachEvent('onWeixinJSBridgeReady', onBridgeReady);
        }
    } else {
        onBridgeReady();
    }
}
