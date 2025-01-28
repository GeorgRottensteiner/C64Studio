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

    private void AssembleHeader( CompileTargetType targetType, ByteBuffer result, int startAddress )
    {
      switch ( targetType )
      {
        default:
          // applies to all formats so far
          result.AppendU16( (ushort)startAddress );
          break;
      }
    }



    private void AssembleLine( CompileTargetType targetType, ByteBuffer result, ref int curAddress, LineInfo info )
    {
      switch ( targetType )
      {
        case CompileTargetType.P_ZX81:
          // TODO
          break;
        default:
          // applies to all formats so far
          result.AppendU16( (ushort)( curAddress + info.LineData.Length + 5 ) );
          result.AppendU16( (ushort)info.LineNumber );
          result.Append( info.LineData );

          // end of line
          result.AppendU8( 0 );

          curAddress += (int)info.LineData.Length + 5;
          break;
      }
    }



    private void AssembleTrailer( CompileTargetType targetType, ByteBuffer result )
    {
      switch ( targetType )
      {
        default:
          // applies to all formats so far
          result.AppendU16( 0 );
          break;
      }
    }


  }
}
