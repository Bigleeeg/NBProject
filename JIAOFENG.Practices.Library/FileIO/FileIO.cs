using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Dashinginfo.Practices.Library.File
{
    public class FileIO
    {
        private FileInfo file;
        public FileIO(string fileFullPath) : this(new FileInfo(fileFullPath))
        {
        }
        public FileIO(FileInfo file)
        {
            this.file = file;
            if (!this.file.Exists)
            {
                System.IO.File.Create(this.file.FullName);
            }            
        }
        public void Write(string data)
        {
            lock (this.file)
            {
                FileStream fs = this.file.Open(FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(data);
                sw.Close();
                fs.Close();
            }
        }
        public void WriteLine(string data)
        {
            lock (this.file)
            {
                FileStream fs = this.file.Open(FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(data);
                sw.Close();
                fs.Close();
            }
        }
        public MemoryStream ConvertToStream()
        {
            FileStream fileStream = new FileStream(this.file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            MemoryStream stream = new MemoryStream(bytes);
            return stream;
        }
    }
}
