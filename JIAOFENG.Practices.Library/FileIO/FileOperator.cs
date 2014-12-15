using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace JIAOFENG.Practices.Library.FileIO
{
    public class FileOperator
    {
        private FileInfo file;
        public FileOperator(string fileFullPath)
            : this(new FileInfo(fileFullPath))
        {
        }
        public FileOperator(FileInfo file)
        {
            this.file = file;
            if (!Directory.Exists(file.DirectoryName))
            {
                Directory.CreateDirectory(file.DirectoryName);
            }
        }
        public void Write(string data)
        {
            lock (this.file)
            {
                FileStream fs = this.file.Open(FileMode.Append, FileAccess.Write, FileShare.None);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(data);
                sw.Close();
                fs.Close();
            }
        }
        public void Write(byte[] array, int offset, int count)
        {
            lock (this.file)
            {
                FileStream fs = this.file.Open(FileMode.Append, FileAccess.Write, FileShare.None);
                fs.Write(array, offset, count);
                fs.Close();
            }
        }
        public void WriteLine(string data)
        {
            lock (this.file)
            {
                FileStream fs = this.file.Open(FileMode.Append, FileAccess.Write, FileShare.None);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(data);
                sw.Close();
                fs.Close();
            }
        }
        public byte[] ConvertToBytes()
        {
            using (FileStream fileStream = new FileStream(this.file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                byte[] bytes = new byte[fileStream.Length];
                fileStream.Read(bytes, 0, bytes.Length);
                return bytes;
            } 
        }
        public MemoryStream ConvertToStream()
        {
            MemoryStream stream = new MemoryStream(ConvertToBytes());
            return stream;
        }
    }
}
