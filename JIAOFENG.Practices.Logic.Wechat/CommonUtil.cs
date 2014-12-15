using JIAOFENG.Practices.Library.Utility;
using JIAOFENG.Practices.Logic.Log;
using JIAOFENG.Practices.Logic.Wechat.Response.Message;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.Wechat
{
    public class CommonUtil
    {
        public static string HttpRequest(string requestUrl, string method = "GET", string body = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
            request.Method = method;
            if (!string.IsNullOrWhiteSpace(body))
            {
                byte[] content;
                //将URL编码后的字符串转化为字节
                content = System.Text.Encoding.UTF8.GetBytes(body);
                //设置请求的 ContentLength 
                request.ContentLength = content.Length;
                //获得请 求流
                Stream writer = request.GetRequestStream();
                //将请求参数写入流
                writer.Write(content, 0, content.Length);
                // 关闭请求流
                writer.Close();
            }
            using(HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                string encoding = response.ContentEncoding;
                if (string.IsNullOrWhiteSpace(encoding))
                {
                    encoding = "utf-8";
                }
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding)))
                {
                    return reader.ReadToEnd();
                }     
            }
        }
        public static bool SendTemplateMessage(TemplateMessage templateMessage, string requestUrl, string accessToken)
        {
            bool result = false;
            requestUrl = string.Format(requestUrl, accessToken);
            string body = templateMessage.ToJson();
            string strResult = HttpRequest(requestUrl, "POST", body);
            WechatResult error = JsonHelper.ToObjectFromJSON<WechatResult>(strResult);
            if (error != null)
            {
                if (error.errcode == 0)
                {
                    result = true;
                }
                else
                {
                    LogManager.LogError("SendTemplateMessage", error.errmsg, error.errcode.ToString(), error.errcode.ToString());
                }
            }
            return result;
        }
        public static int ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
    }
}
