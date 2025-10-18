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

      SeparateInstructionsAndOperands = true;
      IndentOperandsFromInstructions = 2;

      InsertSpacesBetweenOperands = true;

      IndentLabels = true;
      NumTabsIndentationLabels = 0;
    }



    public void ReadFromBuffer( IReader binIn )
    {
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
    }



    public string FormatStatementIndentation()
    {
      if ( ( !AutoFormatActive )
      || ( !IndentStatements ) )
      {
        return "";
      }
      return TabChar( NumTabsIndentationStatements );
    }



    public string FormatStatementLabel()
    {
      if ( ( !AutoFormatActive )
      || ( !IndentLabels ) )
      {
        return "";
      }
      return TabChar( NumTabsIndentationLabels );
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
      if ( tokens.Count == 0 )
      {
        return "";
      }
      var sb = new StringBuilder();
      if ( tokens.Any( t => parser.IsTokenOpcode( t.Type ) ) )
      {
        var opcodeToken = tokens.First( t =>parser.IsTokenOpcode( t.Type ) );
        int index = tokens.IndexOf( opcodeToken );

        sb.Append( opcodeToken.Content );

        if ( ( SeparateInstructionsAndOperands )
        &&   ( index + 1 < tokens.Count ) )
        {
          sb.Append( ' ', IndentOperandsFromInstructions );
        }
        sb.Append( parser.TokensToExpression( tokens, index + 1, tokens.Count - index - 1 ) );
      }
      else
      {
        sb.Append( parser.TokensToExpression( tokens ) );
      }
      return sb.ToString();
    }



  }
}