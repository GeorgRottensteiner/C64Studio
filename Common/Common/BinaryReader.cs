using System;

namespace GR
{
  namespace IO
  {
	  /// <summary>
	  /// Zusammenfassung für BinaryReader.
	  /// </summary>
	  public class BinaryReader : IReader, IDisposable
	  {

      private System.IO.Stream        m_Stream = null;
      private GR.Memory.ByteBuffer    m_Cache = new GR.Memory.ByteBuffer( 256 );
      private int                     m_CacheBytesUsed = 256;
      private bool                    m_IsOwner = false;


		  public BinaryReader()
		  {
		  }



      public BinaryReader( string Filename )
      {
        m_Stream = new System.IO.FileStream( Filename, System.IO.FileMode.Open, System.IO.FileAccess.Read );
        m_IsOwner = true;
      }



      public BinaryReader( System.IO.Stream Stream )
		  {
        m_Stream = Stream;
		  }



      public override bool DataAvailable
      {
        get
        {
          if ( m_Stream == null )
          {
            return false;
          }
          if ( m_CacheBytesUsed < m_Cache.Length )
          {
            return true;
          }
          return ( m_Stream.Position < m_Stream.Length );
        }
      }



      private bool EnsureReadBuffer( int NumBytes )
      {
        if ( m_Cache.Length - m_CacheBytesUsed >= NumBytes )
        {
          // data still in cache
          return true;
        }
        int     bytesInCache = (int)m_Cache.Length - m_CacheBytesUsed;

        if ( m_Stream.Position + NumBytes - bytesInCache > m_Stream.Length )
        {
          return false;
        }
        return true;
      }



      private byte NextByte()
      {
        if ( m_CacheBytesUsed + 1 <= m_Cache.Length )
        {
          ++m_CacheBytesUsed;
          return m_Cache.ByteAt( m_CacheBytesUsed - 1 );
        }

        int bytesRead = m_Stream.Read( m_Cache.Data(), 0, (int)m_Cache.Length );
        if ( bytesRead < m_Cache.Length )
        {
          m_Cache.TruncateAt( (uint)bytesRead );
        }
        m_CacheBytesUsed = 0;

        if ( bytesRead > 0 )
        {
          return NextByte();
        }
        return 0;
      }



      public override float ReadF32()
      {
        if ( m_Stream == null )
        {
          return 0;
        }
        if ( !EnsureReadBuffer( 4 ) )
        {
          return 0;
        }
        System.UInt32     ui32 = (uint)NextByte();
        ui32 |= ( (uint)NextByte() << 8 );
        ui32 |= ( (uint)NextByte() << 16 );
        ui32 |= ( (uint)NextByte() << 24 );

        float     Value = 0.0f;

        Value = System.BitConverter.ToSingle( System.BitConverter.GetBytes( ui32 ), 0 );
        return Value;
      }



      public override System.UInt32 ReadUInt32()
      {
        if ( m_Stream == null )
        {
          return 0;
        }
        if ( !EnsureReadBuffer( 4 ) )
        {
          return 0;
        }
        System.UInt32     ui32 = (uint)NextByte();
        ui32 |= ( (uint)NextByte() << 8 );
        ui32 |= ( (uint)NextByte() << 16 );
        ui32 |= ( (uint)NextByte() << 24 );

        return ui32;
      }



      public override System.Int32 ReadInt32()
      {
        if ( m_Stream == null )
        {
          return 0;
        }
        if ( !EnsureReadBuffer( 4 ) )
        {
          return 0;
        }
        System.Int32     i32 = (int)NextByte();
        i32 |= ( (int)NextByte() << 8 );
        i32 |= ( (int)NextByte() << 16 );
        i32 |= ( (int)NextByte() << 24 );

        return i32;
      }



      public override System.UInt16 ReadUInt16()
      {
        if ( m_Stream == null )
        {
          return 0;
        }
        if ( !EnsureReadBuffer( 2 ) )
        {
          return 0;
        }
        System.UInt16     ui16 = (UInt16)NextByte();

        ui16 |= (UInt16)( NextByte() << 8 );

        return ui16;
      }



      public override System.UInt32 ReadUInt32NetworkOrder()
      {
        if ( m_Stream == null )
        {
          return 0;
        }
        if ( !EnsureReadBuffer( 4 ) )
        {
          return 0;
        }
        System.UInt32     ui32 = (uint)( NextByte() << 24 );
        ui32 |= ( (uint)NextByte() << 16 );
        ui32 |= ( (uint)NextByte() << 8 );
        ui32 |= (uint)NextByte();

        return ui32;
      }



      public override System.Int32 ReadInt32NetworkOrder()
      {
        if ( m_Stream == null )
        {
          return 0;
        }
        if ( !EnsureReadBuffer( 4 ) )
        {
          return 0;
        }
        System.Int32     i32 = (int)( NextByte() << 24 );
        i32 |= ( (int)NextByte() << 16 );
        i32 |= ( (int)NextByte() << 8 );
        i32 |= (int)NextByte();

        return i32;
      }



      public override System.UInt16 ReadUInt16NetworkOrder()
      {
        if ( m_Stream == null )
        {
          return 0;
        }
        if ( !EnsureReadBuffer( 2 ) )
        {
          return 0;
        }
        System.UInt16     ui16 = (UInt16)( NextByte() << 8 );

        ui16 |= (UInt16)NextByte();

        return ui16;
      }



      public override System.Byte ReadUInt8()
      {
        if ( m_Stream == null )
        {
          return 0;
        }
        if ( !EnsureReadBuffer( 1 ) )
        {
          return 0;
        }
        return (System.Byte)NextByte(); 
      }



      public override UInt32 ReadBlock( GR.Memory.ByteBuffer BufferTarget, UInt32 BytesToRead )
      {
        if ( m_Stream == null )
        {
          return 0;
        }

        int   bytesInCache = (int)m_Cache.Length - m_CacheBytesUsed;

        // full in cache
        if ( BytesToRead <= bytesInCache )
        {
          BufferTarget.Resize( BytesToRead );
          m_Cache.CopyTo( BufferTarget, m_CacheBytesUsed, (int)BytesToRead );
          m_CacheBytesUsed += (int)BytesToRead;
          return BytesToRead;
        }

        UInt32    bytesRead = 0;

        // partially in cache
        BufferTarget.Resize( (uint)bytesInCache );
        m_Cache.CopyTo( BufferTarget, m_CacheBytesUsed, bytesInCache );
        m_CacheBytesUsed += bytesInCache;
        BytesToRead -= (uint)bytesInCache;

        bytesRead = (UInt32)bytesInCache;


        UInt32  BytesToReadNow = BytesToRead;
        if ( m_Stream.Position + BytesToRead > m_Stream.Length )
        {
          BytesToReadNow = (UInt32)( m_Stream.Length - m_Stream.Position );
        }
        UInt32    OriginalLength = (UInt32)BufferTarget.Length;

        BufferTarget.Resize( (UInt32)( BufferTarget.Length + BytesToReadNow ) );

        m_Stream.Read( BufferTarget.Data(), (int)OriginalLength, (int)BytesToReadNow );

        bytesRead += BytesToReadNow;

        return bytesRead;
      }



      public override String ReadString()
      {
        if ( m_Stream == null )
        {
          return "";
        }
        if ( !EnsureReadBuffer( 4 ) )
        {
          return "";
        }
        UInt32  Length = ReadUInt32();

        if ( !EnsureReadBuffer( (int)Length ) )
        {
          return "";
        }

        string      strResult = "";

        for ( int i = 0; i < Length; ++i )
        {
          char      cChar = (char)NextByte();
          strResult += cChar;
        }
        return strResult;
      }



      public override bool ReadLine( out String Line )
      {
        Line = "";
        if ( m_Stream == null )
        {
          return false;
        }

        byte nextChar = 0;

        while ( DataAvailable )
        {
          nextChar = NextByte();
          if ( nextChar == 10 )
          {
            // remove 13 if appended
            if ( Line.EndsWith( "\r" ) )
            {
              Line = Line.Substring( 0, Line.Length - 1 );
            }
            return true;
          }
          Line += (char)nextChar;
        }
        return ( Line.Length > 0 );
      }



      public override void Close()
      {
        if ( ( m_Stream != null )
        &&   ( m_IsOwner ) )
        {
          m_Stream.Close();
          m_Stream.Dispose();
        }
        m_Stream = null;
      }



      public void Dispose()
      {
        Close();
      }



      public override long Size
      {
        get
        {
          if ( m_Stream == null )
          {
            return 0;
          }
          return m_Stream.Length;
        }
      }



      public override long Position
      {
        get
        {
          if ( m_Stream == null )
          {
            return 0;
          }
          return m_Stream.Position - ( m_Cache.Length - m_CacheBytesUsed );
        }
      }



      public override void Skip( int BytesToSkip )
      {
        int   bytesInCache = (int)m_Cache.Length - m_CacheBytesUsed;

        if ( BytesToSkip <= bytesInCache )
        {
          m_CacheBytesUsed += BytesToSkip;
          return;
        }

        // partially in cache
        m_CacheBytesUsed += bytesInCache;
        BytesToSkip -= bytesInCache;

        // skip rest from stream
        if ( m_Stream.Position + BytesToSkip > m_Stream.Length )
        {
          BytesToSkip = (int)( m_Stream.Length - m_Stream.Position );
        }

        m_Stream.Seek( BytesToSkip, System.IO.SeekOrigin.Current );
      }



    }


  }
}