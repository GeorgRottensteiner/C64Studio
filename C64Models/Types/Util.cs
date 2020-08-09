using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GR.Memory;

namespace C64Studio
{
  public class Util
  {
    public static double StringToDouble( string Text )
    {
      double  result = 0;

      StringToDouble( Text, out result );
      return result;
    }



    public static bool StringToDouble( string Text, out double Result )
    {
      Result = 0;

      return double.TryParse( Text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out Result );
    }



    public static GR.Memory.ByteBuffer ToFilename( string Name )
    {
      GR.Memory.ByteBuffer bufName = new GR.Memory.ByteBuffer();
      for ( int i = 0; i < 16; ++i )
      {
        if ( i < Name.Length )
        {
          /*
          if ( Types.ConstantData.PETSCII.ContainsKey( Name[i] ) )
          {
            bufName.AppendU8( Types.ConstantData.PETSCII[Name[i]] );
          }*/
          var potChar = Types.ConstantData.PetSCIIToChar.Values.FirstOrDefault( v => v.CharValue == Name[i] );
          if ( potChar != null )
          {
            bufName.AppendU8( potChar.PetSCIIValue );
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



    public static string RemoveQuotes( string Orig )
    {
      if ( ( Orig.Length >= 2 )
      &&   ( Orig.StartsWith( "\"" ) )
      &&   ( Orig.EndsWith( "\"" ) ) )
      {
        return Orig.Substring( 1, Orig.Length - 2 );
      }
      return Orig;
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
        if ( !DataByteDirective.EndsWith( " " ) )
        {
          sb.Append( ' ' );
        }

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
              if ( !DataByteDirective.EndsWith( " " ) )
              {
                sb.Append( ' ' );
              }
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

      if ( LineOffset <= 0 )
      {
        LineOffset = 1;
      }
      if ( StartLine < 0 )
      {
        StartLine = 0;
      }
      int     dataPos = 0;

      while ( dataPos < Data.Length )
      {
        int     startLength = sb.Length;

        sb.Append( StartLine );
        sb.Append( "DATA" );

        bool    firstByte = true;

        while ( ( sb.Length - startLength < 80 )
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



    internal static string ToBASICData( GR.Memory.ByteBuffer Data, int StartLine, int LineOffset, int WrapByteCount )
    {
      StringBuilder   sb = new StringBuilder();

      if ( WrapByteCount < 1 )
      {
        return ToBASICData( Data, StartLine, LineOffset, WrapByteCount );
      }
      if ( LineOffset <= 0 )
      {
        LineOffset = 1;
      }
      if ( StartLine < 0 )
      {
        StartLine = 0;
      }
      int     dataPos = 0;

      while ( dataPos < Data.Length )
      {
        int     startLength = sb.Length;

        sb.Append( StartLine );
        sb.Append( "DATA" );

        bool    firstByte = true;
        int     numBytesInLine = 0;

        while ( ( numBytesInLine < WrapByteCount )
        &&      ( dataPos < Data.Length ) )
        {
          if ( !firstByte )
          {
            sb.Append( ',' );
          }
          firstByte = false;
          sb.Append( Data.ByteAt( dataPos ) );
          ++dataPos;
          ++numBytesInLine;
        }
        sb.AppendLine();

        StartLine += LineOffset;
      }
      return sb.ToString();
    }



    internal static string ToBASICHexData( GR.Memory.ByteBuffer Data, int StartLine, int LineOffset )
    {
      StringBuilder   sb = new StringBuilder();

      if ( LineOffset <= 0 )
      {
        LineOffset = 1;
      }
      if ( StartLine < 0 )
      {
        StartLine = 0;
      }
      int     dataPos = 0;

      while ( dataPos < Data.Length )
      {
        int     startLength = sb.Length;
        sb.Append( StartLine );
        sb.Append( "DATA" );

        bool    firstByte = true;

        while ( ( sb.Length - startLength < 76 )
        && ( dataPos < Data.Length ) )
        {
          if ( !firstByte )
          {
            sb.Append( ',' );
          }
          firstByte = false;
          sb.Append( Data.ByteAt( dataPos ).ToString( "X2" ) );
          ++dataPos;
        }
        sb.AppendLine();

        StartLine += LineOffset;
      }
      return sb.ToString();
    }



    internal static string ToBASICHexData( GR.Memory.ByteBuffer Data, int StartLine, int LineOffset, int WrapByteCount )
    {
      if ( WrapByteCount < 1 )
      {
        return ToBASICHexData( Data, StartLine, LineOffset );
      }

      StringBuilder   sb = new StringBuilder();

      if ( LineOffset <= 0 )
      {
        LineOffset = 1;
      }
      if ( StartLine < 0 )
      {
        StartLine = 0;
      }
      int     dataPos = 0;

      while ( dataPos < Data.Length )
      {
        int     startLength = sb.Length;
        sb.Append( StartLine );
        sb.Append( "DATA" );

        bool    firstByte = true;
        int     numBytesInLine = 0;

        while ( ( numBytesInLine < WrapByteCount )
        &&      ( dataPos < Data.Length ) )
        {
          if ( !firstByte )
          {
            sb.Append( ',' );
          }
          firstByte = false;
          sb.Append( Data.ByteAt( dataPos ).ToString( "X2" ) );
          ++dataPos;
          ++numBytesInLine;
        }
        sb.AppendLine();

        StartLine += LineOffset;
      }
      return sb.ToString();
    }



    internal static ByteBuffer FromBASICHex( string Text )
    {
      string[]  lines = Text.Split( new char[] { '\n' } );

      GR.Memory.ByteBuffer    resultData = new GR.Memory.ByteBuffer();

      for ( int i = 0; i < lines.Length; ++i )
      {
        string    cleanLine = lines[i].Trim().ToUpper();

        int   dataPos = cleanLine.IndexOf( "DATA" );
        if ( dataPos != -1 )
        {
          int     commaPos = -1;
          int     byteStartPos = dataPos + 4;

          do
          {
            commaPos = cleanLine.IndexOf( ',', byteStartPos );
            if ( commaPos == -1 )
            {
              commaPos = cleanLine.Length;
            }
            int     value = GR.Convert.ToI32( cleanLine.Substring( byteStartPos, commaPos - byteStartPos ).Trim(), 16 );
            resultData.AppendU8( (byte)value );

            byteStartPos = commaPos + 1;
          }
          while ( commaPos < cleanLine.Length );
        }
      }
      return resultData;
    }



    internal static string DoubleToString( double Value )
    {
      return Value.ToString( "0.00000000000000000000", System.Globalization.CultureInfo.InvariantCulture );
    }
  }
}
