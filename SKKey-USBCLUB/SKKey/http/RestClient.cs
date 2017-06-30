using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

using System.Collections.Generic;
using System.Net.Security;

using System.Security.Cryptography.X509Certificates;

namespace SKKey.http
{
    class RestClient
    {
        private static readonly log4net.ILog log = 
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly string DefaultUserAgent = 
            "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)";

        public RestClient()
        {

        }

        // 将 HttpWebResponse 返回结果转换成 string
        private static string getResponseString(HttpWebResponse response)
        {
            if (response == null)
                return null;

            string str = null;
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8")))
            {
                str = reader.ReadToEnd();
            }
            return str;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }


        public static string post(string url, string jsonBody, bool isJSON,int retryTimes)
        {
            log.Info("retry :" + retryTimes + " url：" + url + " jsonbody:" + jsonBody);
            string json = "";
            try
            {
                HttpWebRequest request = null;
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                    request = WebRequest.Create(url) as HttpWebRequest;
                    request.ProtocolVersion = HttpVersion.Version10;
                }
                else
                {
                    request = WebRequest.Create(url) as HttpWebRequest;
                }

                request.UserAgent = DefaultUserAgent;


                log.Debug("call rest post url=" + url);
                log.Debug("postdata:\r\n" + jsonBody);

                request.Method = "post";
                if (isJSON)
                {
                    request.ContentType = "application/json;charset=UTF-8";
                }
                else
                {
                    request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                }
                request.ReadWriteTimeout = 10000;
                request.Timeout = 10000;
                var stream = request.GetRequestStream();
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(jsonBody);
                    writer.Flush();
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                json = getResponseString(response);
                log.Info("response:\r\n" +"url \r\n"+ url +"\r\njsonBody:\r\n"+jsonBody+"\r\n res:\r\n"+ json);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("服务器返回异常");
                }
                return json;
            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
                if (retryTimes >= 1)
                {
                    throw new Exception("http 请求失败");
                }
                retryTimes++;
                log.Error("retry :"+ retryTimes+ " url："+url + " jsonbody:"+jsonBody);
                return post(url, jsonBody, isJSON, retryTimes);
            }
        }

        public static string postForm(string url, Dictionary<string, string> para)
        {
            string parastr = "";
            if (para == null)
            {
                return post(url, "", false,0);
            }
            foreach (string key in para.Keys)
            {
                parastr += key + "=" + para[key] + "&";
            }
            return post(url, parastr, false,0);
        }


        public static string postJSON(string url, object ob)
        {
            string parastr = "";
            if (ob == null)
            {
                return post(url, "", false,0);
            }
            try
            {
                parastr = JsonConvert.SerializeObject(ob);
            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
                throw new Exception("JSON 序列化错误");
            }
            return post(url, parastr, true,0);
        }

        public static T postJSONPCallback<T>(string url, Dictionary<string, string> ob)
        {
            if (url.IndexOf("?") != -1)
            {
                url += "&callback=CALLBACK";
            }
            else
            {
                url += "?callback=CALLBACK";
            }
            String ret = postForm(url, ob);

            if (ret == null || ret.Length <= 10)
            {
                log.Error("postJSONPCallback:url:" + url + ":ret:" + ret);
                throw new Exception("结果解析失败");
            }
            T t = default(T);
            try
            {
                ret = ret.Substring("CALLBACK".Length + 1, ret.Length - 10);
                t = JsonConvert.DeserializeObject<T>(ret);
            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
                throw new Exception("结果解析失败");
            }
            return JsonConvert.DeserializeObject<T>(ret);
        }













    }
}
