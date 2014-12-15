using com.igetui.api.openservice;
using JIAOFENG.Practices.Library.Common;
using JIAOFENG.Practices.Library.Utility;
using JIAOFENG.Practices.Logic.AppPush;
using JIAOFENG.Practices.Logic.Log;
using JIAOFENG.Practices.Logic.Wechat;
using JIAOFENG.Practices.Logic.Wechat.Response.Message;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //AppManager.Add("test", "111111111111111111,", "hhhh", 1);

            TemplateMessage t = new TemplateMessage();
            t.TemplateId = "Mgm5wdTU814pog8cv-EvYrVDtaJrT2m6yRk22HUHmQI";
            //WechatManager.Add()
            //TemplateFactory f = new TemplateFactory(ConfigurationManager.AppSettings["AppId"], ConfigurationManager.AppSettings["AppKey"]);
            //ITemplate t = f.CreateNotificationTemplate("Hou", "Fengtao", "你好！");
            ////ITemplate t = f.CreateTransmissionTemplate("你好！");
            //Pusher p = new Pusher(ConfigurationManager.AppSettings["Host"], ConfigurationManager.AppSettings["AppKey"], ConfigurationManager.AppSettings["MasterSecret"], ConfigurationManager.AppSettings["AppId"]);
            //AppPushResult r = p.PushMessageToApp(t);
            //Console.WriteLine(r.result);
            //Console.WriteLine(r.contentId);
            //Console.Read();
            //TestEntity t = new TestEntity();
            //object o = "string";
            //string s = JsonHelper.ToJSON(o);
        }
       
    }
    public class TestEntity : IPrimaryKey
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
