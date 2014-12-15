using System;
using System.Web;
using System.Web.UI;
using JIAOFENG.Practices.Library.Cryptography;

namespace JIAOFENG.Practices.Library.Utility
{
    public sealed class CookieHelper
    {
        public static void AddCookie(string name, string value)
        {
            TimeSpan expires = new TimeSpan(30, 0 , 0 , 0);
            AddCookie(name, value, expires);
        }
        public static void AddCookie(string name, string value, TimeSpan expires)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception("Cookie name is invalidate");
            }
            if (value == null)
            {
                value = string.Empty;
            }
            HttpCookie cookie = new HttpCookie(name, value);
            cookie.Expires = DateTime.Now.Add(expires);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static string GetCookie(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception("Cookie name is invalidate");
            }
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            return cookie == null ? "" : cookie.Value;
        }
        
        public static void AddEncodeCookie(string name, string value)
        {
            TimeSpan expires = new TimeSpan(30, 0, 0, 0);
            AddEncodeCookie(name, value, expires);
        }
        public static void AddEncodeCookie(string name, string value, TimeSpan expires)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception("Cookie name is invalidate");
            }
            if (value == null)
            {
                value = string.Empty;
            }
            HttpCookie cookie = new HttpCookie(name, DESCryptography.Encrypt(value, name + "dashing!"));
            cookie.Expires = DateTime.Now.Add(expires);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static string GetEncodeCookie(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception("Cookie name is invalidate");
            }
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            return cookie == null ? "" : DESCryptography.Decrypt(cookie.Value, name + "dashing!");
        }
    }
}

