using System;
using System.IO;
using System.Text;

namespace ConsoleLogger
{
    // Defines the data protocol for reading and writing strings on our stream
    public class StreamString
    {
        private readonly Stream ioStream;
        private readonly UnicodeEncoding streamEncoding;

        public StreamString(Stream ioStream)
        {
            this.ioStream = ioStream;
            this.streamEncoding = new UnicodeEncoding();
        }

        public string ReadString()
        {
            byte[] strSizeArr = new byte[sizeof(int)];
            this.ioStream.Read(strSizeArr, 0, sizeof(int));
            int strSize = BitConverter.ToInt32(strSizeArr, 0);
            byte[] inBuffer = new byte[strSize];
            this.ioStream.Read(inBuffer, 0, strSize);
            return this.streamEncoding.GetString(inBuffer);
        }

        public int WriteString(string outString)
        {
            byte[] outBuffer = this.streamEncoding.GetBytes(outString);
            byte[] strSize = BitConverter.GetBytes(outBuffer.Length);
            this.ioStream.Write(strSize, 0, strSize.Length);
            this.ioStream.Write(outBuffer, 0, outBuffer.Length);
            this.ioStream.Flush();
            return outBuffer.Length + 2;
        }
    }
}
