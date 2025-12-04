using GR.Collections;
using GR.IO;
using GR.Memory;
using RetroDevStudio.Parser;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace RetroDevStudio
{
  public class AutoFormatSettings
  {
    public bool     AutoFormatActive = true;
    public bool     IndentStatements = true;
    public int      NumTabsIndentationStatements = 5;

    public bool     IndentLabels = true;
    public int      NumTabsIndentationLabels = 0;

    public bool     IndentPseudoOpsLikeCode = true;
    public int      NumTabsIndentationPseudoOps = 5;
    public List<MacroInfo.PseudoOpType>   PseudoOpsToIndent = new List<MacroInfo.PseudoOpType>();

    public bool     SeparateInstructionsAndOperands = true;
    public int      IndentOperandsFromInstructions = 2;

    public bool     PutLabelsOnSeparateLine = true;

    public bool     InsertSpacesBetweenOperands = true;

    // these are saved outside this object
    public int      TabSize             = 2;
    public bool     TabConvertToSpaces  = true;
    public bool     StripTrailingSpaces = false;

    // - set a fixed number of spaces or tabs in front of (a selection of ) instructions (and not labels)
    // - set a fixed number of spaces or tabs in front of (a selection of ) pseudo operations (setting indentation of  { }  blocks to 1 tab)
    // - set a fixed number of spaces or tabs between (a selection of) instructions and operands
    // - set the case of (a selection of) instructions to upper or lower case. Including ,x and ,y additions to operands (not labels or comments)
    // - optionally collapse multiple empty lines to 1 empty line



    public ByteBuffer ToBuffer()
    {
      var result = new ByteBuffer();

      GR.IO.FileChunk   chunkDetails = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_CODE_FORMATTING );

      chunkDetails.AppendI32( AutoFormatActive ? 1 : 0 );
      chunkDetails.AppendI32( IndentStatements ? 1 : 0 );
      chunkDetails.AppendI32( NumTabsIndentationStatements );
      chunkDetails.AppendI32( IndentPseudoOpsLikeCode ? 1 : 0 );
      chunkDetails.AppendI32( NumTabsIndentationPseudoOps );
      chunkDetails.AppendI32( PutLabelsOnSeparateLine ? 1 : 0 );
      chunkDetails.AppendI32( SeparateInstructionsAndOperands ? 1 : 0 );
      chunkDetails.AppendI32( IndentOperandsFromInstructions );
      chunkDetails.AppendI32( InsertSpacesBetweenOperands ? 1 : 0 );
      chunkDetails.AppendI32( IndentLabels ? 1 : 0 );
      chunkDetails.AppendI32( NumTabsIndentationLabels );

      chunkDetails.AppendI32( PseudoOpsToIndent.Count );
      foreach ( var entry in PseudoOpsToIndent )
      {
        chunkDetails.AppendI32( (int)entry );
      }

      return chunkDetails.ToBuffer();
    }



    public void SetDefault()
    {
      NumTabsIndentationStatements = 5;
      IndentPseudoOpsLikeCode = true;
      PutLabelsOnSeparateLine = true;

      AutoFormatActive = true;
      IndentStatements = true;

      NumTabsIndentationPseudoOps = 5;
      PseudoOpsToIndent.Clear();

      SeparateInstructionsAndOperands = true;
      IndentOperandsFromInstructions = 2;

      InsertSpacesBetweenOperands = true;

      IndentLabels = true;
      NumTabsIndentationLabels = 0;
    }



    public void ReadFromBuffer( IReader binIn )
    {
      PseudoOpsToIndent.Clear();  

      AutoFormatActive = ( binIn.ReadInt32() != 0 );
      IndentStatements = ( binIn.ReadInt32() != 0 );
      NumTabsIndentationStatements = binIn.ReadInt32();
      IndentPseudoOpsLikeCode = ( binIn.ReadInt32() != 0 );
      NumTabsIndentationPseudoOps = binIn.ReadInt32();
      PutLabelsOnSeparateLine = ( binIn.ReadInt32() != 0 );
      SeparateInstructionsAndOperands = ( binIn.ReadInt32() != 0 );
      IndentOperandsFromInstructions = binIn.ReadInt32();
      InsertSpacesBetweenOperands = ( binIn.ReadInt32() != 0 );
      IndentLabels = ( binIn.ReadInt32() != 0 );
      NumTabsIndentationLabels = binIn.ReadInt32();

      int numPOsToIndent = binIn.ReadInt32();
      for ( int i = 0; i < numPOsToIndent; ++i )
      {
        PseudoOpsToIndent.Add( (MacroInfo.PseudoOpType)binIn.ReadInt32() );
      }
    }



    public string FormatStatementIndentation()
    {
      if ( ( !AutoFormatActive )
      ||   ( !IndentStatements ) )
      {
        return "";
      }
      return TabChar( NumTabsIndentationStatements );
    }



    public string FormatStatementLabel()
    {
      if ( ( !AutoFormatActive )
      ||   ( !IndentLabels ) )
      {
        return "";
      }
      return TabChar( NumTabsIndentationLabels );
    }



    public string FormatPseudoOpIndentation( TokenInfo tokenInfo, AssemblerSettings assemblerSettings )
    {
      if ( ( !AutoFormatActive )
      ||   ( !IndentPseudoOpsLikeCode ) )
      {
        return "";
      }
      if ( assemblerSettings.PseudoOps.TryGetValue( tokenInfo.Content.ToUpper(), out var po ) )
      {
        if ( PseudoOpsToIndent.Contains( po.Type ) )
        {
          return TabChar( NumTabsIndentationStatements );
        }
        return TabChar( NumTabsIndentationPseudoOps );
      }

      return TabChar( NumTabsIndentationPseudoOps );
    }



    private string TabChar( int numTabsIndentationStatements )
    {
      if ( TabConvertToSpaces )
      {
        return new string( ' ', TabSize * numTabsIndentationStatements );
      }
      return new string( '\t', numTabsIndentationStatements );
    }



    public string FormatStatement( ASMFileParser parser, List<TokenInfo> tokens )
    {
      return FormatStatement( parser, tokens, 0, tokens.Count );
    }



    public string FormatStatement( ASMFileParser parser, List<TokenInfo> tokens, int startIndex, int count )
    {
      if ( count == 0 )
      {
        return "";
      }
      var tokensSlice = tokens.Skip( startIndex ).Take( count ).ToList();
      var sb = new StringBuilder();
      if ( tokensSlice.Any( t => parser.IsTokenOpcode( t.Type ) ) )
      {
        var opcodeToken = tokensSlice.First( t =>parser.IsTokenOpcode( t.Type ) );
        int index = tokensSlice.IndexOf( opcodeToken );

        sb.Append( opcodeToken.Content );

        bool insertedFirstSpace = false;
        if ( ( SeparateInstructionsAndOperands )
        &&   ( index + 1 < tokensSlice.Count ) )
        {
          insertedFirstSpace = true;
          sb.Append( ' ', IndentOperandsFromInstructions );
        }
        if ( InsertSpacesBetweenOperands )
        {
          for ( int i = index + 1; i < tokensSlice.Count; ++i )
          {
            if ( ( i > index + 1 )
            ||   ( !insertedFirstSpace ) )
            {
              if ( tokensSlice[i].Content == "," )
              {
                // no space before comma
              }
              else if ( ( i > index +1 )
              &&        ( tokensSlice[i - 1].Content == "#" ) )
              {
                // no space after #
              }
              else
              {
                sb.Append( ' ' );
              }
            }
            sb.Append( tokensSlice[i].Content );
          }
        }
        else
        {
          sb.Append( parser.TokensToExpression( tokensSlice, index + 1, tokensSlice.Count - index - 1 ) );
        }
      }
      else
      {
        if ( InsertSpacesBetweenOperands )
        {
          sb.Append( tokensSlice[0].Content );
          for ( int i = 1; i < tokensSlice.Count; ++i )
          {
            sb.Append( ' ' );
            sb.Append( tokensSlice[i].Content );
          }
        }
        else
        {
          sb.Append( parser.TokensToExpression( tokensSlice ) );
        }
      }
      return sb.ToString();
    }



    public bool AutoFormatPseudoOpArguments( TokenInfo tokenInfo, AssemblerSettings assemblerSettings )
    {
      if ( !AutoFormatActive )
      {
        return false;
      }
      if ( !assemblerSettings.PseudoOps.TryGetValue( tokenInfo.Content.ToUpper(), out var po ) )
      {
        return false;
      }
      switch ( po.Type )
      {
        case MacroInfo.PseudoOpType.BYTE:
        case MacroInfo.PseudoOpType.WORD:
        case MacroInfo.PseudoOpType.WORD_BE:
        case MacroInfo.PseudoOpType.DWORD:
        case MacroInfo.PseudoOpType.DWORD_BE:
        case MacroInfo.PseudoOpType.TEXT:
        case MacroInfo.PseudoOpType.FILL:
        case MacroInfo.PseudoOpType.ALIGN:
        case MacroInfo.PseudoOpType.ALIGN_DASM:
        case MacroInfo.PseudoOpType.BANK:
        case MacroInfo.PseudoOpType.BASIC:
        case MacroInfo.PseudoOpType.ERROR:
        case MacroInfo.PseudoOpType.FOR:
        case MacroInfo.PseudoOpType.MESSAGE:
        case MacroInfo.PseudoOpType.ORG:
        case MacroInfo.PseudoOpType.PSEUDO_PC:
        case MacroInfo.PseudoOpType.REPEAT:
        case MacroInfo.PseudoOpType.SET:
        case MacroInfo.PseudoOpType.SKIP:
        case MacroInfo.PseudoOpType.TEXT_PET:
        case MacroInfo.PseudoOpType.TEXT_RAW:
        case MacroInfo.PseudoOpType.TEXT_SCREEN:
        case MacroInfo.PseudoOpType.TEXT_SCREEN_XOR:
        case MacroInfo.PseudoOpType.WHILE:
          return true;
      }
      return false;
    }



  }
}