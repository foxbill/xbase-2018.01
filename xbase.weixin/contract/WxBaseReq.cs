using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace xbase.weixin.contract
{
    public class WxBaseReq : WxBaseMsg
    {
        /// <summary>
        /// 发送本实体到微信
        /// </summary>
        /// <param name="url"></param>
        /// <param name="respObject"></param>
        /// <returns></returns>
        public T send<T>(string url)
        {
            byte[] data = Encoding.GetEncoding("UTF-8").GetBytes(this.toXml());
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            if (request == null)
            {
                throw new ApplicationException(string.Format("Invalid url string: {0}", url));
            }
            // request.UserAgent = sUserAgent;  
            // request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            //    request.ContentLength = data.Length;

            Stream requestStream = request.GetRequestStream();

            requestStream.Write(data, 0, data.Length);
            requestStream.Close();

            try
            {
                Stream responseStream = request.GetResponse().GetResponseStream();
                return XmlConvert.getObj<T>(responseStream);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
