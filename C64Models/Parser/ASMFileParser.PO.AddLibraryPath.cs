using GR.Memory;
using RetroDevStudio.Formats;
using RetroDevStudio.Parser;
using RetroDevStudio.Types;
using RetroDevStudio.Types.ASM;
using System;
using System.Collections.Generic;
using Tiny64;

namespace RetroDevStudio.Parser
{
  public partial class ASMFileParser : ParserBase
  {
    private ParseLineResult POAddLibraryPath( string FolderPath, string ParentFilename, ref int LineIndex )
    {
      try
      {
        string fullPath = GR.Path.Append( System.IO.Path.GetDirectoryName( ParentFilename ), FolderPath );
        fullPath = GR.Path.Normalize( fullPath, true );

        // don't add duplicates
        foreach ( var libFile in m_CompileConfig.LibraryFiles )
        {
          if ( GR.Path.IsPathEqual( fullPath, libFile ) )
          {
            return ParseLineResult.OK;
          }
        }
        m_CompileConfig.LibraryFiles.Add( fullPath );
        return ParseLineResult.OK;
      }
      catch ( Exception ex )
      {
        AddError( LineIndex, ErrorCode.E1401_INTERNAL_ERROR, ex.ToString() );
      }
      return ParseLineResult.ERROR_ABORT;
    }



  }
}
