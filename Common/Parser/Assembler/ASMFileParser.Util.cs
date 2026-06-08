using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RetroDevStudio.Types;
using RetroDevStudio.Types.ASM;
using GR.Collections;
using GR.Memory;
using RetroDevStudio;
using RetroDevStudio.Formats;
using Tiny64;
using RetroDevStudio.Converter;
using System.Security.Policy;
using GR.Generic;



namespace RetroDevStudio.Parser
{
  public partial class ASMFileParser : ParserBase
  {
    public static string DeQuote( string content )
    {
      if ( ( content.StartsWith( "\"" ) )
      &&   ( content.EndsWith( "\"" )
      &&   ( content.Length >= 2 ) ) )
      {
        return content.Substring( 1, content.Length - 2 );
      }
      return content;
    }



    public static bool IsInQuotes( string text )
    {
      if ( ( string.IsNullOrEmpty( text ) )
      ||   ( text.Length < 2 ) )
      {
        return false;
      }
      if ( ( text.StartsWith( "\"" ) )
      &&   ( text.EndsWith( "\"" ) ) )
      {
        return true;
      }
      return false;
    }




  }
}
