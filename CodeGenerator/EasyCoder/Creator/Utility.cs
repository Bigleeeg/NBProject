using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace EasyCoder
{
    public static class Utility
    {
        public static string ReadFile(string filePath)
        {
            FileStream stream = File.OpenRead(filePath);
            StreamReader sr = new StreamReader(stream);
            string template = sr.ReadToEnd();
            sr.Close();
            stream.Close();

            return template;
        }
    }
}
