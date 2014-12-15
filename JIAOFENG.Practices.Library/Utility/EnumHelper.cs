using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace JIAOFENG.Practices.Library.Utility
{

    /// <summary>
    /// Coding by DuanXiaochao in 03/20/2013
    /// </summary>
    public class EnumHelper
    {
        //Cache Enum values
        private static Dictionary<Type, Dictionary<int, string>> cache = new Dictionary<Type, Dictionary<int, string>>();
        /// <summary>
        /// 调用方式EnumHelp.GetEunm(typeof(枚举类型));
        /// </summary>
        /// <param name="enumDesc"></param>
        /// <returns></returns>
        public static Dictionary<int, string> GetEunm(Type enumDesc)
        {
            //Cache
            if (cache.Keys.Contains(enumDesc))
            {
                return cache[enumDesc];
            }

            //new Dictionary
            Dictionary<int, string> dic = new Dictionary<int, string>();
            if (enumDesc == null)
                return dic;

            Type _enumType = enumDesc;
            FieldInfo[] InfoStyle = _enumType.GetFields();
            for (int i = 1; i < InfoStyle.Length; i++)
            {
                FieldInfo fi = InfoStyle[i];
                DescriptionAttribute dna = (DescriptionAttribute)Attribute.GetCustomAttribute(
                   fi, typeof(DescriptionAttribute));
                object o = Enum.Parse(enumDesc, InfoStyle[i].Name);
                if (dna != null && !dna.Description.IsNullOrWhiteSpace())
                {
                    dic.Add((int)o, dna.Description);              
                }
                else
                {
                    dic.Add((int)o, o.ToString());
                }
            }
            cache.Add(enumDesc, dic);
            return dic;
        }

        public static string GetEnumDescription(int value, Type enumDesc)
        {
            Dictionary<int, string> dic = GetEunm(enumDesc);
            if (dic.Keys.Contains(value))
            {
                return dic[value];
            }
            else
            {
                return string.Empty;
            }           
        }
    }
}
