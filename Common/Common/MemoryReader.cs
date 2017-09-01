using System;

namespace GR
{
  namespace IO
  {
	  /// <summary>
	  /// Zusammenfassung für MemoryReader.
	  /// </summary>
	  public class MemoryReader : IReader
	  {

      private GR.Memory.ByteBuffer        m_Buffer = null;
      private int                         m_Position = 0;

		  public MemoryReader()
		  {
		  }



      public MemoryReader( byte[] Data )
      {
        m_Buffer = new GR.Memory.ByteBuffer( Data );
      }



      public MemoryReader( GR.Memory.ByteBuffer Buffer )
		  {
        m_Buffer = Buffer;
		  }


      public override bool DataAvailable
      {
        get
        {
          return ( m_Position < m_Buffer.Length );
        }
      }



      public override float ReadF32()
      {
        if ( m_Buffer == null )
        {
          return 0.0f;
        }
        if ( m_Position + 4 > m_Buffer.Length )
        {
          return 0.0f;
        }
        System.UInt32     ui32 = (uint)m_Buffer.ByteAt( m_Position );

        ui32 |= ( (uint)m_Buffer.ByteAt( m_Position + 1 ) << 8 );
        ui32 |= ( (uint)m_Buffer.ByteAt( m_Position + 2 ) << 16 );
        ui32 |= ( (uint)m_Buffer.ByteAt( m_Position + 3 ) << 24 );

        m_Position += 4;

        float     Value = System.BitConverter.ToSingle( System.BitConverter.GetBytes( ui32 ), 0 );

        /*
        unsafe
        {
          Value = *(float*)( &ui32 );
        }*/

        return Value;
      }

      public override System.UInt32 ReadUInt32()
      {
        if ( m_Buffer == null )
        {
          return 0;
        }
        if ( m_Position + 4 > m_Buffer.Length )
        {
          return 0;
        }
        System.UInt32     ui32 = (uint)m_Buffer.ByteAt( m_Position );

        ui32 |= ( (uint)m_Buffer.ByteAt( m_Position + 1 ) << 8 );
        ui32 |= ( (uint)m_Buffer.ByteAt( m_Position + 2 ) << 16 );
        ui32 |= ( (uint)m_Buffer.ByteAt( m_Position + 3 ) << 24 );

        m_Position += 4;

        return ui32;
      }

      public override System.Int32 ReadInt32()
      {
        if ( m_Buffer == null )
        {
          return 0;
        }
        if ( m_Position + 4 > m_Buffer.Length )
        {
          return 0;
        }
        System.Int32     i32 = (int)m_Buffer.ByteAt( m_Position );

        i32 |= ( (int)m_Buffer.ByteAt( m_Position + 1 ) << 8 );
        i32 |= ( (int)m_Buffer.ByteAt( m_Position + 2 ) << 16 );
        i32 |= ( (int)m_Buffer.ByteAt( m_Position + 3 ) << 24 );

        m_Position += 4;

        return i32;
      }



      public override System.UInt16 ReadUInt16()
      {
        if ( m_Buffer == null )
        {
          return 0;
        }
        if ( m_Position + 2 > m_Buffer.Length )
        {
          return 0;
        }
        System.UInt16     ui16 = (UInt16)m_Buffer.UInt16At( m_Position );
        m_Position += 2;

        return ui16;
      }



      public override System.Byte ReadUInt8()
      {
        if ( m_Buffer == null )
        {
          return 0;
        }
        if ( m_Position + 1 > m_Buffer.Length )
        {
          return 0;
        }
        System.Byte   u8 = m_Buffer.ByteAt( m_Position );
        ++m_Position;
        return u8;
      }



      public override UInt32 ReadBlock( GR.Memory.ByteBuffer BufferTarget, UInt32 BytesToRead )
      {
        if ( m_Buffer == null )
        {
          return 0;
        }
        UInt32  BytesToReadNow = BytesToRead;

        if ( m_Position + BytesToRead > m_Buffer.Length )
        {
          BytesToReadNow = (UInt32)( m_Buffer.Length - m_Position );
        }
        UInt32    OriginalLength = (UInt32)BufferTarget.Length;

        BufferTarget.Resize( (UInt32)( BufferTarget.Length + BytesToReadNow ) );

        for ( int i = 0; i < BytesToReadNow; ++i )
        {
          BufferTarget.SetU8At( (int)OriginalLength + i, m_Buffer.ByteAt( m_Position + i ) );
        }
        m_Position += (int)BytesToReadNow;

        return BytesToReadNow;
      }



      public override String ReadString()
      {
        if ( m_Buffer == null )
        {
          return "";
        }
        if ( m_Position + 4 > m_Buffer.Length )
        {
          return "";
        }
        UInt32  Length = ReadUInt32();

        if ( m_Position + Length > m_Buffer.Length )
        {
          return "";
        }

        string      strResult = "";

        for ( int i = 0; i < Length; ++i )
        {
          char      cChar = (char)ReadUInt8();
          strResult += cChar;
        }
        return strResult;
      }



      public override bool ReadLine( out String Line )
      {
        Line = "";
        if ( m_Buffer == null )
        {
          return false;
        }

        byte nextChar = 0;

        while ( m_Position < m_Buffer.Length )
        {
          nextChar = ReadUInt8();
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
        m_Buffer = null;
        m_Position = 0;
      }

      public override long Size
      {
        get
        {
          if ( m_Buffer == null )
          {
            return 0;
          }
          return m_Buffer.Length;
        }
      }

      public override long Position
      {
        get
        {
          return m_Position;
        }
      }

    }


  }
}