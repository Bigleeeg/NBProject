using JIAOFENG.Practices.Library.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JIAOFENG.Practices.Logic.Wechat
{
    public class MenuUtil
    {
        public static Menu GetMenu(XmlDocument doc)
        {
            Menu menu = new Menu();
            XmlElement rootElement = doc.DocumentElement;
            foreach (XmlNode node in rootElement.ChildNodes)
            {
                menu.Buttons.Add(GetButton(node));
            }
            return menu;
        }
        private static Button GetButton(XmlNode node)
        {
            switch(node.Name)
            {
                case "clickbutton":
                    return new ClickButton() {Name =node.Attributes["name"].Value, Type = node.Attributes["type"].Value, Key = node.Attributes["key"].Value };
                case "viewbutton":
                    return new ViewButton() { Name = node.Attributes["name"].Value, Type = node.Attributes["type"].Value, Url = node.Attributes["url"].Value };
                case "complexbutton":
                    ComplexButton complexButton = new ComplexButton(){ Name = node.Attributes["name"].Value };
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        complexButton.SubButtons.Add(GetButton(child));
                    }
                    return complexButton;
            }
            return null;
        }
        public static bool CreateMenu(Menu menu, string accessToken)
        {
            bool result = false;
            string requestUrl = ConfigurationManager.AppSettings["MenuCreateUrl"];
            requestUrl = string.Format(requestUrl, accessToken);
            string strResult = CommonUtil.HttpRequest(requestUrl, "Post", menu.ToJson());
            WechatResult error = JsonHelper.ToObjectFromJSON<WechatResult>(strResult);
            if (error != null)
            {
                if (error.errcode == 0)
                {
                    result = true;
                }
            }
            return result;
        }
    }
}
