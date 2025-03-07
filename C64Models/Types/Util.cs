﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using GR.Memory;
using RetroDevStudio.Types;
using RetroDevStudio.Formats;

namespace RetroDevStudio
{
  public partial class Util
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



    public static GR.Memory.ByteBuffer ToFilename( MediaFilenameType Type, string Name )
    {
      GR.Memory.ByteBuffer bufName = new GR.Memory.ByteBuffer();

      switch ( Type )
      {
        case MediaFilenameType.SPECTRUM:
          for ( int i = 0; i < Name.Length; ++i )
          {
            char     c = char.ToUpperInvariant( Name[i] );

            var key = ConstantData.AllPhysicalKeyInfos[MachineType.ZX81].FirstOrDefault( k => k.CharValue == c );
            if ( key != default( SingleKeyInfo ) )
            {
              bufName.AppendU8( key.NativeValue );
            }
            else
            {
              bufName.AppendU8( (byte)c );
            }
          }
          break;
        case MediaFilenameType.COMMODORE:
          for ( int i = 0; i < 16; ++i )
          {
            if ( i < Name.Length )
            {
              char     c = Name[i];

              ToggleCase( ref c );

              var potChar = ConstantData.PetSCIIToChar.Values.FirstOrDefault( v => v.CharValue == c );
              if ( potChar != null )
              {
                bufName.AppendU8( potChar.NativeValue );
              }
              else
              {
                bufName.AppendU8( (byte)c );
              }
            }
            else
            {
              bufName.AppendU8( 0xa0 );
            }
          }
          break;
        case MediaFilenameType.CPC:
          {
            int   dotPos = Name.IndexOf( '.' );
            if ( dotPos == -1 )
            {
              Name += ".bin";
              dotPos = Name.IndexOf( '.' );
            }
            string firstPart = Name.Substring( 0, dotPos ).Replace( " ", "" );
            string extension = Name.Substring( dotPos + 1 ).Replace( " ", "" );
            if ( firstPart.Length < 8 )
            {
              firstPart = firstPart.PadRight( 8, ' ' );
            }
            else if ( firstPart.Length > 8 )
            {
              firstPart = firstPart.Substring( 0, 8 );
            }
            if ( extension.Length < 3 )
            {
              extension = extension.PadRight( 3, ' ' );
            }
            else if ( extension.Length > 3 )
            {
              extension = extension.Substring( 0, 3 );
            }
            Name = firstPart + "." + extension;
            for ( int i = 0; i < 11; ++i )
            {
              if ( i < Name.Length )
              {
                char     c = Name[i];

                if ( c == ' ' )
                {
                  // space is not a valid char for CPC disks
                  continue;
                }

                bufName.AppendU8( (byte)c );
              }
              else
              {
                // fill with blanks
                bufName.AppendU8( 32 );
              }
            }
          }
          break;
        default:
          throw new Exception( $"Unsupported media filename type {Type}" );
      }
      return bufName;
    }



    public static void ToggleCase( ref char c )
    {
      if ( ( c >= 'A' )
      &&   ( c <= 'Z' ) )
      {
        c = (char)( c + 'a' - 'A' );
      }
      else if ( ( c >= 'a' )
      &&        ( c <= 'z' ) )
      {
        c = (char)( c - 'a' + 'A' );
      }
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
        if ( ConstantData.PETSCII.ContainsKey( Name[i] ) )
        {
          bufName.AppendU8( ConstantData.PETSCII[Name[i]] );
        }
        else
        {
          bufName.AppendU8( (byte)Name[i] );
        }
      }
      return bufName;
    }



    public static string FilenameToUnicode( MediaFilenameType FilenameType, GR.Memory.ByteBuffer Filename )
    {
      string filename = "";

      switch ( FilenameType )
      {
        case MediaFilenameType.COMMODORE:
          {
            int numShiftSpacesAtEnd = 0;
            int pos = (int)Filename.Length;
            while ( ( pos > 0 )
            &&      ( Filename.ByteAt( pos - 1 ) == 0xa0 ) )
            {
              --pos;
            }
            numShiftSpacesAtEnd = (int)Filename.Length - pos;

            for ( int i = 0; i < Filename.Length - numShiftSpacesAtEnd; ++i )
            {
              byte petscii = Filename.ByteAt( i );
              filename += (char)ConstantData.PETSCIIToUnicode[petscii];
            }
          }
          break;
        case MediaFilenameType.CPC:
          filename = ASCIIEncoding.ASCII.GetString( Filename.Data() );
          break;
        case MediaFilenameType.AMIGA:
          filename = ASCIIEncoding.ASCII.GetString( Filename.Data() );
          break;
        case MediaFilenameType.SPECTRUM:
          {
            for ( int i = 0; i < Filename.Length; ++i )
            {
              byte c = Filename[i];

              var key = ConstantData.AllPhysicalKeyInfos[MachineType.ZX81].FirstOrDefault( k => k.NativeValue == c );
              if ( key != default( SingleKeyInfo ) )
              {
                filename += key.CharValue;
              }
              else
              {
                filename += (char)c;
              }
            }
          }
          break;
        default:
          throw new Exception( $"Unsupported Filenametype {FilenameType}" );
      }
      return filename;
    }



    public static string PETSCIIToUnicode( GR.Memory.ByteBuffer Filename )
    {
      string filename = "";
      for ( int i = 0; i < Filename.Length; ++i )
      {
        byte petscii = Filename.ByteAt( i );
        filename += (char)ConstantData.PETSCIIToUnicode[petscii];
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
        char character = ConstantData.PETSCIIToUnicode[petscii];
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



    internal static string ToBASICData( GR.Memory.ByteBuffer Data, int StartLine, int LineOffset, int WrapByteCount, int WrapCharCount, bool InsertSpaces, bool AsHex )
    {
      if ( AsHex )
      {
        return ToBASICHexData( Data, StartLine, LineOffset, WrapByteCount, WrapCharCount, InsertSpaces );
      }
      return ToBASICData( Data, StartLine, LineOffset, WrapByteCount, WrapCharCount, InsertSpaces );
    }



    internal static string ToBASICData( GR.Memory.ByteBuffer Data, int StartLine, int LineOffset, int WrapByteCount, int WrapCharCount, bool InsertSpaces )
    {
      StringBuilder   sb = new StringBuilder();

      if ( WrapByteCount < 1 )
      {
        WrapByteCount = 80;
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
        if ( InsertSpaces )
        {
          sb.Append( ' ' );
        }
        sb.Append( "DATA" );
        if ( InsertSpaces )
        {
          sb.Append( ' ' );
        }

        bool    firstByte = true;
        int     numBytesInLine = 0;
        int     numByteValuesInLine = 0;

        if ( WrapCharCount > 0 )
        {
          if ( WrapCharCount < 20 )
          {
            WrapCharCount = 20;
          }
          // length of line number plus token
          numBytesInLine = StartLine.ToString().Length + 4;

          while ( ( numBytesInLine < WrapCharCount )
          &&      ( numByteValuesInLine < WrapByteCount )
          &&      ( dataPos < Data.Length ) )
          {
            int   numCharsToAdd = Data.ByteAt( dataPos ).ToString().Length;
            if ( !firstByte )
            {
              ++numCharsToAdd;
            }
            if ( numBytesInLine + numCharsToAdd > WrapCharCount )
            {
              break;
            }

            if ( !firstByte )
            {
              sb.Append( ',' );
            }
            firstByte = false;
            sb.Append( Data.ByteAt( dataPos ) );
            ++dataPos;
            numBytesInLine += numCharsToAdd;
            ++numByteValuesInLine;
          }
        }
        else
        {
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
        }
        sb.AppendLine();

        StartLine += LineOffset;
      }
      return sb.ToString();
    }



    internal static string ToBASICHexData( GR.Memory.ByteBuffer Data, int StartLine, int LineOffset, bool InsertSpaces )
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
        if ( InsertSpaces )
        {
          sb.Append( ' ' );
        }
        sb.Append( "DATA" );
        if ( InsertSpaces )
        {
          sb.Append( ' ' );
        }

        bool    firstByte = true;

        while ( ( sb.Length - startLength < 76 )
        &&      ( dataPos < Data.Length ) )
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



    internal static string ToBASICHexData( GR.Memory.ByteBuffer Data, int StartLine, int LineOffset, int WrapByteCount, int WrapCharCount, bool InsertSpaces )
    {
      if ( ( WrapByteCount < 1 )
      &&   ( WrapCharCount < 1 ) )
      {
        return ToBASICHexData( Data, StartLine, LineOffset, InsertSpaces );
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
        if ( InsertSpaces )
        {
          sb.Append( ' ' );
        }
        sb.Append( "DATA" );
        if ( InsertSpaces )
        {
          sb.Append( ' ' );
        }

        bool    firstByte = true;
        int     numBytesInLine = 0;

        if ( WrapCharCount > 0 )
        {
          if ( WrapCharCount < 20 )
          {
            WrapCharCount = 20;
          }

          // size of line number + DATA token
          numBytesInLine = StartLine.ToString().Length + 1;
          while ( ( numBytesInLine + 3 <= WrapCharCount )
          &&      ( dataPos < Data.Length ) )
          {
            if ( !firstByte )
            {
              sb.Append( ',' );
              ++numBytesInLine;
            }
            firstByte = false;
            sb.Append( Data.ByteAt( dataPos ).ToString( "X2" ) );
            ++dataPos;
            numBytesInLine += 2;
          }
        }
        else
        {

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
        }
        sb.AppendLine();

        StartLine += LineOffset;
      }
      return sb.ToString();
    }



    internal static ByteBuffer FromBASIC( string Text )
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
            int     value = GR.Convert.ToI32( cleanLine.Substring( byteStartPos, commaPos - byteStartPos ).Trim() );
            resultData.AppendU8( (byte)value );

            byteStartPos = commaPos + 1;
          }
          while ( commaPos < cleanLine.Length );
        }
      }
      return resultData;
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
      //return Value.ToString( "G", System.Globalization.CultureInfo.InvariantCulture );
      return Value.ToString( "0." + new string( '#', 339 ) ).Replace( ",", "." );
    }



    internal static ByteBuffer FromASMData( string Text )
    {
      var asmParser = new Parser.ASMFileParser();

      var config        = new Parser.CompileConfig();
      config.TargetType = CompileTargetType.PLAIN;
      config.OutputFile = "temp.bin";
      config.Assembler  = AssemblerType.C64_STUDIO;

      string    temp = "* = $0801\n" + Text;
      if ( Text.Contains( ".BYTE" ) )
      {
        config.Assembler = AssemblerType.DASM;
        // DASM requires pseudo ops to be not at the left most border
        temp = "  ORG $0801\n" + Text.Replace( ".BYTE", " .BYTE" );
      }

      if ( ( asmParser.Parse( temp, null, config, null, out Types.ASM.FileInfo asmFileInfo ) )
      &&   ( asmParser.Assemble( config ) ) )
      {
        return asmParser.AssembledOutput.Assembly;
      }
      return null;
    }



  }
}
