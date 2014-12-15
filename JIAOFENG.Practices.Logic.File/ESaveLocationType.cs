using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIAOFENG.Practices.Logic.File
{
    public enum ESaveLocationType
    {
        WebFolder = 1,//网站文件夹（本地相对路径）
        FileServer = 2,//文件服务器（局域网绝对路径）
        LocalDB = 3,//本地数据库（放置文件的数据库表）
        MongoDB = 4//文件数据库（Mogo数据库）
    }
}
