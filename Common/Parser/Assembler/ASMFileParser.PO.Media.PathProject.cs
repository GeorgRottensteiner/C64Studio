using GR.Memory;
using RetroDevStudio.Formats;
using RetroDevStudio.Parser;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetroDevStudio.Parser
{
  public partial class ASMFileParser : ParserBase
  {
    private bool POIncludeMediaPathProject( int lineIndex, bool binary, string subFilename, string method, List<List<TokenInfo>> paramTokens, string labelPrefix, out ByteBuffer dataToInclude, out string[] replacementLines )
    {
      replacementLines  = null;
      dataToInclude     = null;

      // path project
      // pathonly,index,count
      // path,index,count
      // pathlist,index,count
      if ( paramTokens.Count > 4 )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected <Path|PathOnly|PathList>[,<Index>[,<Count>]]" );
        return false;
      }
      if ( ( method != "PATH" )
      &&   ( method != "PATHONLY" )
      &&   ( method != "PATHLIST" ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Unknown method '" + method + "', supported values for this file type are PATH, PATHONLY or PATHLIST" );
        return false;
      }

      if ( ( binary )
      &&   ( method != "PATH" ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Binary export from path project is only supported for method 'PATH'" );
        return false;
      }

      var pathProject = new RetroDevStudio.Formats.PathProject();

      try
      {
        dataToInclude = GR.IO.File.ReadAllBytes( subFilename );
        if ( dataToInclude == null )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilename );
          return false;
        }
      }
      catch ( System.IO.IOException )
      {
        AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilename );
        return false;
      }
      if ( !pathProject.ReadFromBuffer( dataToInclude ) )
      {
        AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read path project from " + subFilename );
        return false;
      }

      string textToInclude = "";
      var pathData = pathProject.ExportData();

      int startIndex = 0;
      int numEntries = pathData.Count;

      if ( ( paramTokens.Count >= 3 )
      &&   ( EvaluateTokens( lineIndex, paramTokens[2], out SymbolInfo startIndexSymbol ) ) )
      {
        startIndex = startIndexSymbol.ToInt32();
      }
      if ( ( paramTokens.Count >= 4 )
      &&   ( EvaluateTokens( lineIndex, paramTokens[3], out SymbolInfo numCharsSymbol ) ) )
      {
        numEntries = numCharsSymbol.ToInt32();
      }
      if ( ( startIndex < 0 )
      ||   ( startIndex >= pathData.Count ) )
      {
        AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Invalid start index" );
        return false;
      }
      if ( numEntries <= 0 )
      {
        AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Invalid number of entries, must be >= 1" );
        return false;
      }
      if ( startIndex + numEntries > pathData.Count )
      {
        AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Invalid number of entries, path has "
              + pathData.Count + " entries, but we're trying to fetch up to " + ( startIndex + numEntries ) );
        return false;
      }

      if ( ( method == "PATHLIST" )
      ||   ( method == "PATH" ) )
      {
        var sbPathList = new StringBuilder();

        sbPathList.Append( labelPrefix );
        sbPathList.Append( "NUM_PATHS = " );
        sbPathList.AppendLine( numEntries.ToString() );

        sbPathList.Append( labelPrefix );
        sbPathList.AppendLine( "PATH_LIST_LO" );
        for ( int i = 0; i < numEntries; ++i )
        {
          sbPathList.Append( "!byte" );
          sbPathList.Append( ' ' );
          sbPathList.AppendLine( "<" + labelPrefix + "PATH_" + Util.StringToLabel( pathData[startIndex + i].first.ToUpper() ) );
        }
        sbPathList.AppendLine();
        sbPathList.Append( labelPrefix );
        sbPathList.AppendLine( "PATH_LIST_HI" );
        for ( int i = 0; i < numEntries; ++i )
        {
          sbPathList.Append( "!byte" );
          sbPathList.Append( ' ' );
          sbPathList.AppendLine( ">" + labelPrefix + "PATH_" + Util.StringToLabel( pathData[startIndex + i].first.ToUpper() ) );
        }
        sbPathList.AppendLine();

        textToInclude = sbPathList.ToString();
      }

      if ( ( method == "PATH" )
      ||   ( method == "PATHONLY" ) )
      {
        if ( binary )
        {
          var finalData = new ByteBuffer();
          for ( int i = 0; i < numEntries; ++i )
          {
            finalData.Append( pathData[startIndex + i].second );
          }
          dataToInclude = finalData;
        }
        else
        {
          for ( int i = 0; i < numEntries; ++i )
          {
            textToInclude += labelPrefix + "PATH_" + Util.StringToLabel( pathData[startIndex + i].first ).ToUpper() + System.Environment.NewLine;
            textToInclude += Util.ToASMData( pathData[startIndex + i].second, false, 0, "!byte", false ) + System.Environment.NewLine;
          }
        }
      }

      if ( !binary )
      {
        replacementLines = textToInclude.Split( new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries );
      }
      return true;
    }


  }
}
