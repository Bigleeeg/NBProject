using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.Wechat
{
    public enum AccessTokenTypeEnum
    {
        client_credential = 1,//公众帐号身份验证，可以用来配置公众平台，比如管理菜单
        snsapi_base = 2, //获得用户openid
        snsapi_userinfo //获得用户基本信息
    }
}
