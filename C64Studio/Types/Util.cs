using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio
{
  public class Util
  {
    public static GR.Memory.ByteBuffer ToFilename( string Name )
    {
      GR.Memory.ByteBuffer bufName = new GR.Memory.ByteBuffer();
      for ( int i = 0; i < 16; ++i )
      {
        if ( i < Name.Length )
        {
          if ( Types.ConstantData.PETSCII.ContainsKey( Name[i] ) )
          {
            bufName.AppendU8( Types.ConstantData.PETSCII[Name[i]] );
          }
          else
          {
            bufName.AppendU8( (byte)Name[i] );
          }
        }
        else
        {
          bufName.AppendU8( 0xa0 );
        }
      }
      return bufName;
    }



    public static GR.Memory.ByteBuffer ToPETSCII( string Name )
    {
      GR.Memory.ByteBuffer bufName = new GR.Memory.ByteBuffer();
      for ( int i = 0; i < Name.Length; ++i )
      {
        if ( Types.ConstantData.PETSCII.ContainsKey( Name[i] ) )
        {
          bufName.AppendU8( Types.ConstantData.PETSCII[Name[i]] );
        }
        else
        {
          bufName.AppendU8( (byte)Name[i] );
        }
      }
      return bufName;
    }



    public static string FilenameToUnicode( GR.Memory.ByteBuffer Filename )
    {
      string filename = "";
      for ( int i = 0; i < Filename.Length; ++i )
      {
        if ( Filename.ByteAt( i ) == 0xa0 )
        {
          break;
        }
        byte petscii = Filename.ByteAt( i );
        filename += (char)Types.ConstantData.PETSCIIToUnicode[petscii];
      }
      return filename;
    }



    public static string PETSCIIToUnicode( GR.Memory.ByteBuffer Filename )
    {
      string filename = "";
      for ( int i = 0; i < Filename.Length; ++i )
      {
        byte petscii = Filename.ByteAt( i );
        filename += (char)Types.ConstantData.PETSCIIToUnicode[petscii];
      }
      return filename;
    }



    public static string FilenameToReadableUnicode( GR.Memory.ByteBuffer Filename )
    {
      string filename = "";
      for ( int i = 0; i < Filename.Length; ++i )
      {
        if ( Filename.ByteAt( i ) == 0xa0 )
        {
          break;
        }
        byte petscii = Filename.ByteAt( i );
        char character = Types.ConstantData.PETSCIIToUnicode[petscii];
        if ( ( character >= 32 )
        &&   ( character < 128 ) )
        {
          filename += character;
        }
        else
        {
          filename += 'X';
        }
      }
      return filename;
    }



    public static string ToASMData( GR.Memory.ByteBuffer Data, bool WrapData, int WrapByteCount, string DataByteDirective )
    {
      return ToASMData( Data, WrapData, WrapByteCount, DataByteDirective, true );
    }



    public static string ToASMData( GR.Memory.ByteBuffer Data, bool WrapData, int WrapByteCount, string DataByteDirective, bool AsHex )
    {
      StringBuilder   sb = new StringBuilder();

      if ( WrapData )
      {
        sb.Append( DataByteDirective );
        sb.Append( ' ' );

        int byteCount = 0;
        for ( int i = 0; i < Data.Length; ++i )
        {
          if ( AsHex )
          {
            sb.Append( '$' );
            sb.Append( Data.ByteAt( i ).ToString( "x2" ) );
          }
          else
          {
            sb.Append( Data.ByteAt( i ).ToString() );
          }

          ++byteCount;
          if ( ( byteCount < WrapByteCount )
          &&   ( i < Data.Length - 1 ) )
          {
            sb.Append( ',' );
          }
          if ( byteCount == WrapByteCount )
          {
            byteCount = 0;

            sb.AppendLine();
            if ( i < Data.Length - 1 )
            {
              sb.Append( DataByteDirective );
              sb.Append( ' ' );
            }
          }
        }
      }
      else
      {
        sb.Append( DataByteDirective );
        sb.Append( ' ' );
        for ( int i = 0; i < Data.Length; ++i )
        {
          if ( AsHex )
          {
            sb.Append( '$' );
            sb.Append( Data.ByteAt( i ).ToString( "x2" ) );
          }
          else
          {
            sb.Append( Data.ByteAt( i ).ToString() );
          }
          if ( i < Data.Length - 1 )
          {
            sb.Append( ',' );
          }
        }
      }
      return sb.ToString();
    }



    internal static string ToBASICData( GR.Memory.ByteBuffer Data, int StartLine, int LineOffset )
    {
      StringBuilder   sb = new StringBuilder();

      int     dataPos = 0;

      while ( dataPos < Data.Length )
      {
        sb.Append( StartLine );
        sb.Append( " DATA " );

        bool    firstByte = true;
        int     startLength = sb.Length;

        while ( ( sb.Length - startLength < 76 )
        &&      ( dataPos < Data.Length ) )
        {
          if ( !firstByte )
          {
            sb.Append( ',' );
          }
          firstByte = false;
          sb.Append( Data.ByteAt( dataPos ) );
          ++dataPos;
        }
        sb.AppendLine();

        StartLine += LineOffset;
      }
      return sb.ToString();
    }


  }
}
