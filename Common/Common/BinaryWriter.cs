using System;

namespace GR
{
  namespace IO
  {
	  /// <summary>
	  /// Zusammenfassung für BinaryWriter.
	  /// </summary>
	  public class BinaryWriter : IWriter
	  {

      private System.IO.Stream        m_Stream = null;

		  public BinaryWriter()
		  {
		  }

      public BinaryWriter( System.IO.Stream Stream )
		  {
        m_Stream = Stream;
		  }



      public override bool WriteF32( float value )
      {
        if ( m_Stream == null )
        {
          return false;
        }

        System.UInt32     ui32 = System.BitConverter.ToUInt32( System.BitConverter.GetBytes( value ), 0 );
        try
        {
          m_Stream.WriteByte( (byte)( ui32 & 0xff ) );
        m_Stream.WriteByte( (byte)( ( ui32 >> 8 ) & 0xff ) );
        m_Stream.WriteByte( (byte)( ( ui32 >> 16 ) & 0xff ) );
        m_Stream.WriteByte( (byte)( ( ui32 >> 24 ) & 0xff ) );
          return true;
      }
        catch ( System.Exception )
        {
          return false;
        }
      }



      public override bool WriteUInt32( System.UInt32 value )
      {
        if ( m_Stream == null )
        {
          return false;
        }
        try
        {
        m_Stream.WriteByte( (byte)( value & 0xff ) );
        m_Stream.WriteByte( (byte)( ( value >> 8 ) & 0xff ) );
        m_Stream.WriteByte( (byte)( ( value >> 16 ) & 0xff ) );
        m_Stream.WriteByte( (byte)( ( value >> 24 ) & 0xff ) );
          return true;
        }
        catch ( System.Exception )
        {
          return false;
        }
      }



      public override bool WriteUInt16( System.UInt16 value )
      {
        if ( m_Stream == null )
        {
          return false;
        }
        try
        {
        m_Stream.WriteByte( (byte)( value & 0xff ) );
        m_Stream.WriteByte( (byte)( ( value >> 8 ) & 0xff ) );
          return true;
        }
        catch ( System.Exception )
        {
          return false;
      }
      }



      public override bool WriteUInt8( System.Byte value )
      {
        if ( m_Stream == null )
        {
          return false;
        }
        try
        {
        m_Stream.WriteByte( value );
          return true;
        }
        catch ( System.Exception )
        {
          return false;
        }
      }



      public override bool WriteBlock( GR.Memory.ByteBuffer Buffer )
      {
        if ( Buffer != null )
        {
          return WriteBlock( Buffer, Buffer.Length );
        }
        return false;
      }



      public override bool WriteBlock( GR.Memory.ByteBuffer Buffer, UInt32 BytesToWrite )
      {
        if ( m_Stream == null )
        {
          return false;
        }
        try
        {
        m_Stream.Write( Buffer.Data(), 0, (int)BytesToWrite );
          return true;
        }
        catch ( System.Exception )
        {
          return false;
        }
      }



      public override bool WriteString( String text )
      {
        if ( m_Stream == null )
        {
          return false;
        }
        if ( !WriteUInt32( (UInt32)text.Length ) )
        {
          return false;
        }

        try
        {
        for ( int i = 0; i < text.Length; ++i )
        {
          m_Stream.WriteByte( (byte)text[i] );
        }
          return true;
        }
        catch ( System.Exception )
        {
          return false;
        }
      }



      public override bool WriteLine( String Line )
      {
        if ( m_Stream == null )
        {
          return false;
        }
        try
        {
          for ( int i = 0; i < Line.Length; ++i )
          {
            m_Stream.WriteByte( (byte)Line[i] );
          }
          m_Stream.WriteByte( 13 );
          m_Stream.WriteByte( 10 );
          return true;
        }
        catch ( System.Exception )
        {
          return false;
        } 
      }



      public override void Close()
      {
        m_Stream = null;
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
          return m_Stream.Position;
        }
      }

    }


  }
}