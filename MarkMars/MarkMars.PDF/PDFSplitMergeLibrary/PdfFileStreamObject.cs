using System;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace MarkMars.PDF
{
    internal class PdfFileStreamObject : PdfFileObject
    {
        private Byte[] streamBuffer;
        private Int32 streamStartOffset, streamLength;

        /// <summary>
        /// ¹¹Ôìº¯Êý
        /// </summary>        
        internal PdfFileStreamObject(PdfFileObject obj)
        {
            this.address = obj.address;
            this.length = obj.length;
            this.text = obj.text;
            this.number = obj.number;
            this.PdfFile = obj.PdfFile;
            this.LoadStreamBuffer();                        
        }
        
        private void LoadStreamBuffer()
        {
            Match m1 = Regex.Match(this.text, @"stream\s*");
            this.streamStartOffset = m1.Index + m1.Value.Length;
            this.streamLength = this.length - this.streamStartOffset;
            this.streamBuffer = new byte[this.streamLength];
            this.PdfFile.memory.Seek(this.address+this.streamStartOffset, SeekOrigin.Begin);
            this.PdfFile.memory.Read(this.streamBuffer, 0,this.streamLength);

            this.PdfFile.memory.Seek(this.address,SeekOrigin.Begin);
            StreamReader sr = new StreamReader(this.PdfFile.memory);
            Char[] startChars = new Char[this.streamStartOffset];
            sr.ReadBlock(startChars, 0, this.streamStartOffset);
            StringBuilder sb = new StringBuilder();
            sb.Append(startChars);
            this.text = sb.ToString();           
        }

        internal override void Transform(System.Collections.Hashtable TransformationHash)
        {
            base.Transform(TransformationHash);
        }

        internal override long WriteToStream(Stream Stream)
        {
            StreamWriter sw = new StreamWriter(Stream);
            sw.Write(this.text);
            sw.Flush();
            new MemoryStream(this.streamBuffer).WriteTo(Stream);
            sw.Flush();

            return this.streamLength + this.text.Length;
        }       
    }
}
