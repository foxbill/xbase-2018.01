using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xbase.weixin.contract;

namespace xbase.weixin.pay
{
    /// <summary>
    /// 支付通知缓存
    /// </summary>
    public static class OrderNotifyPool
    {
        private static Dictionary<string, OrderNotify> pool = new Dictionary<string, OrderNotify>();
        private const int poolSize = 1000;//条

        private static string getKey(string openId, string appId, string tradeNo)
        {
            return openId + "|" + appId + "|" + tradeNo;
        }

        public static OrderNotify getNotify(string openId, string appId, string tradeNo)
        {
            string key = getKey(openId, appId, tradeNo);
            if (pool.ContainsKey(key))
                return pool[key];
            return null;
        }

        public static void clear()
        {
            pool.Clear();
        }

        public static void addNotify(OrderNotify notify)
        {
            if (pool.Count > poolSize)
                pool.Clear();

            string key = getKey(notify.openid, notify.appid, notify.out_trade_no);

            if (!pool.ContainsKey(key))
                pool.Add(key, notify);
            else
                pool[key] = notify;
        }

    }
}
