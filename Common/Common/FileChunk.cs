using System;

namespace GR
{
  namespace IO
  {
	  /// <summary>
	  /// Zusammenfassung für FileChunk.
	  /// </summary>
	  public class FileChunk : GR.Memory.ByteBuffer
	  {

      private System.UInt16             m_Type = 0;

		  public FileChunk()
		  {
		  }

      public FileChunk( System.UInt16 Type )
		  {
        m_Type = Type;
		  }

      public bool ReadFromStream( GR.IO.IReader InStream )
      {
        Clear();
        if ( InStream == null )
        {
          return false;
        }
        if ( InStream.Position + 6 >= InStream.Size )
        {
          return false;
        }
        m_Type = InStream.ReadUInt16();
        UInt32  Size = InStream.ReadUInt32();

        return ( InStream.ReadBlock( this, Size ) == Size );
      }

      public bool WriteToStream( System.IO.BinaryWriter outStream )
      {
        if ( outStream == null )
        {
          return false;
        }
        outStream.Write( Type );
        outStream.Write( base.Length );
        outStream.Write( Data() );
        return true;
      }

      public new uint Length
      {
        get
        {
          return base.Length + 2;
        }
      }

      public GR.Memory.ByteBuffer ToBuffer()
      {
        GR.Memory.ByteBuffer    newBuffer = new GR.Memory.ByteBuffer();
        newBuffer.Reserve( (int)Length );

        newBuffer.AppendU16( Type );
        newBuffer.AppendU32( base.Length );
        newBuffer.Append( this );
        return newBuffer;
      }

      public UInt16 Type
      {
        get
        {
          return m_Type;
        }
      }

      public GR.IO.BinaryReader BinaryReader()
      {
        return new GR.IO.BinaryReader( MemoryStream() );
      }



      public void AppendF32( float Value )
      {
        Append( System.BitConverter.GetBytes( Value ) );
      }



	  }
  }
}