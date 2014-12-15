using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace JIAOFENG.Practices.Library.Utility
{
    /// <summary>
    /// Summary description for NetStreamUtility.
    /// </summary>
    public static class NetStreamUtility
    {
        private const int DataLenthSize = 4;
        private const string stringEndFlag = "\0";
        private static readonly Encoding encode = System.Text.Encoding.UTF8;//GB2312,GBK

        /// <summary>
        /// Returns a 32-bit signed integer converted from four bytes at a specified position in a byte array
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int GetHostIntFromFourNetBytes(byte[] data)
        {
            int myValue = BitConverter.ToInt32(data, 0);
            return IPAddress.NetworkToHostOrder(myValue);
        }

        public static int GetHostIntFromFourNetBytes(ref byte[] data)
        {
            byte[] myData = data;
            int myValue = BitConverter.ToInt32(myData, 0);

            data = new byte[myData.Length - 4];
            System.Array.Copy(myData, 4, data, 0, data.Length);

            return IPAddress.NetworkToHostOrder(myValue);
        }

        public static byte[] WriteFourNetBytesFromHostInt(int myValue)
        {
            myValue = IPAddress.HostToNetworkOrder(myValue);
            return BitConverter.GetBytes(myValue);
        }

        public static byte[] WriteFourNetBytesFromHostInt(byte[] data, int myValue)
        {
            byte[] newData = new byte[data.Length + 4];
            data.CopyTo(newData, 0);
            myValue = IPAddress.HostToNetworkOrder(myValue);
            byte[] intData = BitConverter.GetBytes(myValue);
            intData.CopyTo(newData, data.Length);

            return newData;
        }

        public static short GetHostShortFromTwoNetBytes(byte[] data)
        {
            short myValue = BitConverter.ToInt16(data, 0);
            return IPAddress.NetworkToHostOrder(myValue);
        }

        public static short GetHostShortFromTwoNetBytes(ref byte[] data)
        {
            byte[] myData = data;
            short myValue = BitConverter.ToInt16(myData, 0);

            data = new byte[myData.Length - 2];
            System.Array.Copy(myData, 2, data, 0, data.Length);

            return IPAddress.NetworkToHostOrder(myValue);
        }

        public static byte[] WriteTwoNetBytesFromHostShort(short myValue)
        {
            myValue = IPAddress.HostToNetworkOrder(myValue);
            return BitConverter.GetBytes(myValue);
        }

        public static byte[] WriteTwoNetBytesFromHostShort(byte[] data, short myValue)
        {
            byte[] newData = new byte[data.Length + 2];
            data.CopyTo(newData, 0);
            myValue = IPAddress.HostToNetworkOrder(myValue);
            byte[] shortData = BitConverter.GetBytes(myValue);
            shortData.CopyTo(newData, data.Length);

            return newData;
        }

        public static string GetStringBasedOnShort(ref byte[] data)
        {
            string result;

            byte[] myData = data;
            int size = GetHostShortFromTwoNetBytes(myData);

            data = new byte[myData.Length - size - 2];
            System.Array.Copy(myData, size + 2, data, 0, data.Length);
            if (size == 0)
            {
                result = null;
            }
            else if (size == 1)
            {
                result = string.Empty;
            }
            else
            {
                result = encode.GetString(myData, 2, size);
                result = result.Substring(0, result.Length - 1);
            }
            return result;
        }

        public static string GetStringBasedOnInt(ref byte[] data)
        {
            string result;

            byte[] myData = data;
            int size = GetHostIntFromFourNetBytes(myData);

            data = new byte[myData.Length - size - 4];
            System.Array.Copy(myData, size + 4, data, 0, data.Length);

            if (size == 0)
            {
                result = null;
            }
            else if (size == 1)
            {
                result = string.Empty;
            }
            else
            {
                result = encode.GetString(myData, 4, size);
                result = result.Substring(0, result.Length - 1);
            }
            return result;
        }
        /// <summary>
        /// Read a line from the stream.
        /// A line is interpreted as all the bytes read until a CRLF or LF is encountered.<br/>
        /// CRLF pair or LF is not included in the string.
        /// </summary>
        /// <param name="stream">The stream from which the line is to be read</param>
        /// <returns>A line read from the stream returned as a byte array or <see langword="null"/> if no bytes were readable from the stream</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="stream"/> is <see langword="null"/></exception>
        public static byte[] ReadLineAsBytes(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            using (MemoryStream memoryStream = new MemoryStream())
            {
                while (true)
                {
                    int justRead = stream.ReadByte();
                    if (justRead == -1 && memoryStream.Length > 0)
                        break;

                    // Check if we started at the end of the stream we read from
                    // and we have not read anything from it yet
                    if (justRead == -1 && memoryStream.Length == 0)
                        return null;

                    char readChar = (char)justRead;

                    // Do not write \r or \n
                    if (readChar != '\r' && readChar != '\n')
                        memoryStream.WriteByte((byte)justRead);

                    // Last point in CRLF pair
                    if (readChar == '\n')
                        break;
                }

                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Read a line from the stream. <see cref="ReadLineAsBytes"/> for more documentation.
        /// </summary>
        /// <param name="stream">The stream to read from</param>
        /// <returns>A line read from the stream or <see langword="null"/> if nothing could be read from the stream</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="stream"/> is <see langword="null"/></exception>
        public static string ReadLineAsAscii(Stream stream)
        {
            byte[] readFromStream = ReadLineAsBytes(stream);
            return readFromStream != null ? Encoding.ASCII.GetString(readFromStream) : null;
        }
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }
        public static byte[] WriteNetBytesFromString(string myValue)
        {
            return WriteNetBytesFromString(new byte[0], myValue);
        }

        public static byte[] WriteNetBytesFromString(byte[] data, string myValue)
        {
            byte[] newData;
            if (myValue != null)
            {
                myValue += stringEndFlag;
                byte[] stringData = encode.GetBytes(myValue);
                short size = (short)stringData.Length;
                byte[] sizeData = WriteTwoNetBytesFromHostShort(size);
                newData = new byte[data.Length + stringData.Length + 2];
                data.CopyTo(newData, 0);
                sizeData.CopyTo(newData, data.Length);
                stringData.CopyTo(newData, data.Length + 2);
            }
            else
            {
                short size = 0;
                byte[] sizeData = WriteTwoNetBytesFromHostShort(size);
                newData = new byte[data.Length + 2];
                data.CopyTo(newData, 0);
                sizeData.CopyTo(newData, data.Length);
            }
            return newData;
        }

        public static byte[] WriteNetBytesFromLongString(string myValue)
        {
            return WriteNetBytesFromLongString(new byte[0], myValue);
        }

        public static byte[] WriteNetBytesFromLongString(byte[] data, string myValue)
        {
            byte[] newData;
            if (myValue != null)
            {
                myValue += stringEndFlag;
                byte[] stringData = encode.GetBytes(myValue);
                int size = stringData.Length;
                byte[] sizeData = WriteFourNetBytesFromHostInt(size);
                newData = new byte[data.Length + stringData.Length + 4];
                data.CopyTo(newData, 0);
                sizeData.CopyTo(newData, data.Length);
                stringData.CopyTo(newData, data.Length + 4);
            }
            else
            {
                int size = 0;
                byte[] sizeData = WriteFourNetBytesFromHostInt(size);
                newData = new byte[data.Length + 4];
                data.CopyTo(newData, 0);
                sizeData.CopyTo(newData, data.Length);
            }
            return newData;
        }

        public static bool SendData(NetworkStream ns, byte[] data)
        {
            int size = data.Length;
            byte[] dataSize = WriteFourNetBytesFromHostInt(size);
            try
            {
                ns.Write(dataSize, 0, dataSize.Length);
                ns.Write(data, 0, data.Length);
                ns.Flush();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static byte[] ReceiveVarData(NetworkStream ns)
        {
            int total = 0;
            int recv = 0;
            int size;
            int dataLeft;

            if (ns.CanRead)
            {
                byte[] dataSize = new byte[DataLenthSize];
                try
                {
                    recv = ns.Read(dataSize, 0, DataLenthSize);
                }
                catch //invalid data
                {
                    recv = 0;
                }
                if (recv == 0)
                {
                    return new byte[0];
                }

                size = GetHostIntFromFourNetBytes(dataSize);
                if (size <= 0)
                {
                    return new byte[0];
                }

                byte[] data = new byte[size];
                dataLeft = size;

                long dtStart = DateTime.Now.Ticks;
                while (total < size && DateTime.Now.Ticks - dtStart < 600000000)//60second
                {
                    if (ns.DataAvailable)
                    {
                        recv = ns.Read(data, total, dataLeft);
                        if (recv == 0)
                        {
                            return new byte[0];
                        }
                        total += recv;
                        dataLeft -= recv;
                    }
                    else
                    {
                        Thread.Sleep(50);
                    }
                }

                return data;
            }
            else
            {
                return new byte[0];
            }
        }
    }
}
