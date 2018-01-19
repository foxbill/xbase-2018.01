using Senparc.Weixin.MP.CommonAPIs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using xbase.weixin.utilities;

namespace xbase.weixin
{
    static class WeiXinUtils
    {
        internal static string tryGetAccessToken()
        {
            string appId = WxConfigFile.config().AppID;
            string appSecret = WxConfigFile.config().AppSecret;
            return AccessTokenContainer.TryGetAccessToken(appId, appSecret);
        }

        internal static string getTempSign(object ReqObj, string signKey)
        {
            Type type = ReqObj.GetType();

            List<string> keys = new List<string>();
            Dictionary<string, object> keyValues = new Dictionary<string, object>();
            PropertyInfo[] pis = type.GetProperties();
            for (int i = 0; i < pis.Length; i++)
            {
                PropertyInfo pi = pis[i];
                object value = pi.GetValue(ReqObj);
                string key = pi.Name;
                if (value != null && !string.IsNullOrEmpty(value.ToString()) && !pi.PropertyType.Name.Equals("XmlNode"))
                {
                    keyValues.Add(key, value);
                    keys.Add(key);
                }
            }
            keys.Sort();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < keys.Count; i++)
            {
                sb.Append(keys[i]);
                sb.Append("=");
                sb.Append(keyValues[keys[i]]);
                sb.Append("&");
            }
            sb.Append("key=");
            sb.Append(signKey);

            return sb.ToString();
        }

        internal static string getSign(object ReqObj, string signKey)
        {
            return GetMD5(getTempSign(ReqObj, signKey));
        }

        internal static string GetMD5(string encypStr)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用GB2312编码方式把字符串转化为字节数组．
            inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);


            outputBye = m5.ComputeHash(inputBye);

            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }






        public static RespType Post<RespType>(object reqObj, string url)
        {
            string xml = XmlConvert.getXml(reqObj);
            string strResp = Post(xml, url);
            return XmlConvert.getObj<RespType>(xml);
        }

        public static string Post(string xml, string url)
        {
            Stream responseStream;
            byte[] data = Encoding.GetEncoding("UTF-8").GetBytes(xml);
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            if (request == null)
            {
                throw new ApplicationException(string.Format("Invalid url string: {0}", url));
            }
            // request.UserAgent = sUserAgent;  
            // request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            request.ContentLength = data.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();
            try
            {
                responseStream = request.GetResponse().GetResponseStream();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            string str = string.Empty;
            using (StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8")))
            {
                str = reader.ReadToEnd();
            }
            responseStream.Close();
            return str;

        }

        public static string get(string url)
        {
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            return wc.DownloadString(url);
        }

        /// <summary>
        /// 下载远程服务url的文件到本地服务器，并返回文件在本地服务器上的url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="virPath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string downloadFile(string url, string virPath, string fileName)
        {
            if (string.IsNullOrEmpty(virPath) || string.IsNullOrEmpty(fileName))
                throw new Exception("must need virPath and fileName");

            if (!virPath.StartsWith("/"))
                virPath = "/" + virPath;
            if (!virPath.EndsWith("/"))
                virPath += "/";
            virPath += fileName;
            string toPath = System.AppDomain.CurrentDomain.BaseDirectory + virPath.Replace("/", "\\");


            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            WebResponse response = request.GetResponse();
            string ext = response.Headers["Content-Type"];
            ext = "." + ext.Remove(0, ext.IndexOf("/") + 1).Trim().ToLower();
            toPath += ext;
            virPath += ext;
            
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                using (StreamWriter sw = new StreamWriter(new FileStream(toPath, FileMode.Create)))
                {
                    sw.Write(reader.ReadToEnd());
                    sw.Close();
                }
                reader.Close();
            }


            //FileStream stream = new FileStream(toPath, FileMode.Create);
            //try
            //{
            //    stream.Write(data, 0, data.Length);

            //    //foreach (var b in data)
            //    //{
            //    //    stream.WriteByte(b);
            //    //}
            //}
            //finally
            //{
            //    stream.Close();
            //}
            return WxConfigFile.config().Domain + virPath;
        }

        public static T getJsonObject<T>(string url)
        {
            //JavaScriptSerializer js = new JavaScriptSerializer();
            return JsonConvert.DeserializeObject<T>(get(url));
        }


    }
}
