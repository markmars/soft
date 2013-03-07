using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MarkMars.Common
{
	public class OffsetStream : Stream
	{
		private Stream BaseStream = null;
		private int m_nOffset = 0;
		public OffsetStream(Stream baseStream, int nOffset)
		{
			BaseStream = baseStream;
			m_nOffset = nOffset;
			BaseStream.Position = nOffset;
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			BaseStream.Write(buffer, offset + m_nOffset, count);
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return BaseStream.Read(buffer, offset, count);
		}

		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			switch (origin)
			{
				case SeekOrigin.Begin:
				case SeekOrigin.Current:
					return BaseStream.Seek(offset + m_nOffset, origin);
				case SeekOrigin.End:
					return BaseStream.Seek(offset, origin);
				default:
					throw new Exception();
			}
		}

		public override void Flush()
		{
			throw new NotImplementedException();
		}

		public override long Position
		{
			get
			{
				return BaseStream.Position - m_nOffset;
			}
			set
			{
				BaseStream.Position = value + m_nOffset;
			}
		}

		public override long Length
		{
			get 
			{
				return BaseStream.Length - m_nOffset;
			}
		}

		public override bool CanWrite
		{
			get 
			{
				return BaseStream.CanWrite;
			}
		}

		public override bool CanSeek
		{
			get 
			{
				return BaseStream.CanSeek;
			}
		}

		public override bool CanRead
		{
			get 
			{
				return BaseStream.CanRead;
			}
		}
	}
}
