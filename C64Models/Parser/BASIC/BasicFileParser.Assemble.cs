using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using RetroDevStudio.Parser;
using GR.Memory;
using RetroDevStudio;
using System.Linq;
using RetroDevStudio.Types;
using System.Diagnostics;
using RetroDevStudio.CheckSummer;
using System.Reflection;



namespace RetroDevStudio.Parser.BASIC
{
  public partial class BasicFileParser : ParserBase
  {

    private void AssembleHeader( ByteBuffer result, int startAddress )
    {
      switch ( _ParseContext.TargetType )
      {
        case CompileTargetType.P_ZX81:
          //result.AppendU16( (ushort)startAddress );
          result.AppendHex( "000000fffeffffffff0000ffffffff0000ffffffff00ffff00020000ffffff37000000000000008d0c0000a3f50000bc211840" );

          // PRFBUFF (32 spaces + N/L)
          result.AppendHex( "000000000000000000000000000000000000000000000000000000000000000076" );
          // MEMBOT
          result.AppendHex( "000000000000000000008420000000000000000000000000000000000000" );
          result.AppendU16( 0 );
          // ; System variables.
          // VERSN          !byte 0
          // E_PPC          !word 2
          // D_FILE         !word Display
          // DF_CC          !word Display + 1                ; First character of display
          // VARS           !word Variables
          // DEST           !word 0
          // E_LINE         !word BasicEnd
          // CH_ADD         !word BasicEnd + 4               ; Simulate SAVE "X"
          // X_PTR          !word 0
          // STKBOT         !word BasicEnd + 5
          // STKEND         !word BasicEnd + 5               ; Empty stack
          // BREG           !byte 0
          // MEM            !word MEMBOT
          // UNUSED1        !byte 0
          // DF_SZ          !byte 2
          // S_TOP          !word $0002                      ; Top program line number
          // LAST_K         !word $FDBF
          // DEBOUN         !byte 15
          // MARGIN         !byte 55
          // NXTLIN         !word Line2                      ; Next line address
          // OLDPPC         !word 0
          // FLAGX          !byte 0
          // STRLEN         !word 0
          // T_ADDR         !word $0C8D
          // SEED           !word 0
          // FRAMES         !word $F5A3
          // COORDS         !word 0
          // PR_CC          !byte $BC
          // S_POSN         !word $1821
          // CDFLAG         !byte $40
          // PRBUFF         !byte 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,$76  ; 32 Spaces + Newline
          // MEMBOT         !byte 0,0,0,0,0,0,0,0,0,0,$84,$20,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0      ; 30 zeros (no?)
          // UNUNSED2       !word 0
          // ;end of system variables

          break;
        default:
          // applies to all formats so far
          result.AppendU16( (ushort)startAddress );
          break;
      }
    }



    private void AssembleLine( ByteBuffer result, int curAddress, LineInfo info )
    {
      switch ( _ParseContext.TargetType )
      {
        case CompileTargetType.P_ZX81:
          result.AppendU16NetworkOrder( (ushort)info.LineNumber );
          result.AppendU16( (ushort)( info.LineData.Length + 1 ) );
          result.Append( info.LineData );
          result.AppendU8( 0x76 );
          break;
        default:
          // applies to all formats so far
          result.AppendU16( (ushort)( curAddress + info.LineData.Length + 5 ) );
          result.AppendU16( (ushort)info.LineNumber );
          result.Append( info.LineData );

          // end of line
          result.AppendU8( 0 );

          //curAddress += (int)info.LineData.Length + 5;
          break;
      }
    }



    private void AssembleTrailer( ByteBuffer result )
    {
      switch ( _ParseContext.TargetType )
      {
        case CompileTargetType.P_ZX81:
          {
            int   startOffset = 0;

            // display, N/L, 24 empty lines, variables end
            ushort displayPos = (ushort)( result.Length + 16393 );
            result.AppendHex( "7676767676767676767676767676767676767676767676767680" );

            ushort memBottom = (ushort)( startOffset + 84 + 16393 );
            ushort variablesPos = (ushort)( result.Length - 1 + 16393 );
            ushort basicEnd = (ushort)( result.Length + 16393 );

            
            result.SetU16At( startOffset + 3, displayPos );
            result.SetU16At( startOffset + 5, (ushort)( displayPos + 1 ) );
            result.SetU16At( startOffset + 7, variablesPos );
            result.SetU16At( startOffset + 11, basicEnd );
            result.SetU16At( startOffset + 13, (ushort)( basicEnd + 4 ) );
            result.SetU16At( startOffset + 17, (ushort)( basicEnd + 5 ) );
            result.SetU16At( startOffset + 19, (ushort)( basicEnd + 5 ) );
            result.SetU16At( startOffset + 22, memBottom );
          }
          break;
        default:
          // applies to all formats so far
          result.AppendU16( 0 );
          break;
      }
    }



    private byte MapTokenToByteValue( byte tokenValue )
    {
      return MapCharToNativeByte( (char)tokenValue, _ParseContext.TargetType );
    }



    private void AssembleTokenCompleted( LineInfo info )
    {
      switch ( _ParseContext.TargetType )
      {
        case CompileTargetType.P_ZX81:
          // if the last token is a number ZX81 adds a number byte plus a float (tf!)
          if ( ( info.Tokens.Count >= 1 )
          &&   ( info.Tokens.Last().TokenType == Token.Type.NUMERIC_LITERAL ) )
          {
            float number = GR.Convert.ToF32( info.Tokens.Last().Content );
            info.LineData.AppendU8( 0x7e );
            AssembleZX81NumberToBytes( number, info.LineData );
          }
          break;
      }
    }



    private void AssembleZX81NumberToBytes( float value, ByteBuffer result )
    {
      byte  sign = 0;
      uint  mantissa = 0;
      int   exp = 0;
      //int   newVal = 0;

      // If it's a small integer, then store as such, else it's the 5 byte float.

      if ( value == 0 )
      {
        result.AppendU8( 0 );
        result.AppendU8( 0 );
        result.AppendU8( 0 );
        result.AppendU8( 0 );
        result.AppendU8( 0 );
      }
      /*
      if ( ( value >= -65535 ) 
      &&   ( value < 65536 ) 
      &&   ( value == Math.Round( value ) ) )
      {
        result.AppendU8( 0 );
        if ( value >= 0 )
        {
          result.AppendU8( 0 );
        }
        else
        {
          result.AppendU8( 0xff );
          value += 65536;
        }
        // would be round if float
        int newVal = (int)value;
        result.AppendU8( (byte)( newVal & 0xff ) );
        result.AppendU8( (byte)( newVal >> 8 ) );
        result.AppendU8( 0 );
      }*/
      else
      {
        // 5 byte floating point.

        // Determine the sign
        if ( value < 0 )
        {
          sign = 0x80;
          value = -value;
        }
        else
        {
          sign = 0;
        }

        // Find the exponent.
        exp = (int)Math.Floor( Math.Log( value, 2 ) );
        //Exp:= Floor( log2( Value ) );
        if ( ( exp < -129 )
        ||   ( exp > 126 ) )
        {
          // not possible!
          return;
        }

        // And the mantissa. Multiply by a big number for later shifting
        mantissa = (uint)( ( ( value / Math.Pow( 2.0, exp ) - 1.0 ) * 2147483648 ) + 0.5 );

        // Now store the bytes. Shift the by now huge mantissa
        // and grab each byte as it falls off the end.
        // First the Exponent.
        result.AppendU8( (byte)( exp + 0x81 ) );
        // Next the Mantissa - note that we ignore the high bit of the first byte.
        result.AppendU8( (byte)( ( ( mantissa >> 24 ) & 0x7f ) | sign ) );
        result.AppendU8( (byte)( ( mantissa >> 16 ) & 0xff ) );
        result.AppendU8( (byte)( ( mantissa >> 8 ) & 0xff ) );
        result.AppendU8( (byte)( mantissa & 0xff ) );
      }
    }


  }
}
