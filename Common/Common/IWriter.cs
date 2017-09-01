using System;

namespace GR
{
  namespace IO
  {
	  /// <summary>
	  /// Zusammenfassung für BinaryReader.
	  /// </summary>
	  public abstract class IWriter
	  {
      public abstract bool WriteUInt8( System.Byte Byte );
      public abstract bool WriteUInt16( System.UInt16 Word );
      public abstract bool WriteUInt32( System.UInt32 DWord );
      public abstract bool WriteF32( float Value );
      public abstract bool WriteBlock( GR.Memory.ByteBuffer Data );
      public abstract bool WriteBlock( GR.Memory.ByteBuffer Data, UInt32 BytesToWrite );
      public abstract bool WriteLine( string Line );
      public abstract bool WriteString( string Line );

      public abstract void Close();
      public abstract long Size { get; }
      public abstract long Position { get; }
    }


  }
}