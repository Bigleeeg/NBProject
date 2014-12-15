using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace EasyCoder
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            CodeCreator frm = new CodeCreator();
            Sunisoft.IrisSkin.SkinEngine skin = new Sunisoft.IrisSkin.SkinEngine((System.ComponentModel.Component)frm);
            skin.SkinFile = "Source/MacOS.ssk";
            skin.TitleFont = new System.Drawing.Font("微软雅黑", 10F);
            Application.Run(frm);
            //Application.Run(new CodeCreator());
        }
    }
}
