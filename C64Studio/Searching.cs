using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio
{
  public class Searching
  {
    public string             PreviousSearchedFile = "";
    public string             PreviousSearchedFileContent = null;
    public DateTime           PreviousSearchedFileTimeStamp;
    public StudioCore         Core = null;



    public Searching( StudioCore Core )
    {
      this.Core = Core;
    }



    public void ClearSearchResults()
    {
      Core.MainForm.m_SearchResults.ClearResults();
    }



    public void AddSearchResult( FormFindReplace.SearchLocation FoundLocation )
    {
      Core.MainForm.m_SearchResults.AddSearchResult( FoundLocation );
    }



    public void AddSearchResults( List<FormFindReplace.SearchLocation> FoundLocations )
    {
      Core.MainForm.m_SearchResults.AddSearchResults( FoundLocations );
    }



    internal string GetDocumentInfoText( DocumentInfo DocInfo )
    {
      string elementPath = "";
      if ( System.IO.Path.IsPathRooted( DocInfo.FullPath ) )
      {
        elementPath = DocInfo.FullPath;
      }
      else if ( DocInfo.Project != null )
      {
        elementPath = GR.Path.Normalize( GR.Path.Append( DocInfo.Project.Settings.BasePath, DocInfo.FullPath ), false );
      }
      else
      {
        elementPath = DocInfo.FullPath;
      }

      if ( DocInfo.BaseDoc != null )
      {
        if ( DocInfo.BaseDoc is SourceASMEx )
        {
          DateTime    lastModificationTimeStamp = ( (SourceASMEx)DocInfo.BaseDoc ).LastChange;

          if ( ( GR.Path.IsPathEqual( PreviousSearchedFile, elementPath ) )
          &&   ( lastModificationTimeStamp <= PreviousSearchedFileTimeStamp ) )
          {
            return Core.Searching.PreviousSearchedFileContent;
          }
          PreviousSearchedFile = elementPath;
          PreviousSearchedFileTimeStamp = lastModificationTimeStamp;
          PreviousSearchedFileContent = ( (SourceASMEx)DocInfo.BaseDoc ).editSource.Text;
          return PreviousSearchedFileContent;
        }
        else if ( DocInfo.BaseDoc is SourceBasicEx )
        {
          PreviousSearchedFile = elementPath;
          return DocInfo.BaseDoc.GetContent();
        }
        else if ( DocInfo.BaseDoc is Disassembler )
        {
          PreviousSearchedFile = elementPath;
          return ( (Disassembler)DocInfo.BaseDoc ).editDisassembly.Text;
        }
        return "";
      }

      return GetTextFromFile( elementPath );
    }



    internal string GetTextFromFile( string Filename )
    {
      // can we use cached text?
      bool    cacheIsUpToDate = false;

      DateTime    lastAccessTimeStamp;

      try
      {
        lastAccessTimeStamp = System.IO.File.GetLastWriteTime( Filename );

        cacheIsUpToDate = ( lastAccessTimeStamp <= PreviousSearchedFileTimeStamp );

        PreviousSearchedFileTimeStamp = lastAccessTimeStamp;
      }
      catch ( Exception )
      {
      }

      if ( ( GR.Path.IsPathEqual( PreviousSearchedFile, Filename ) )
      &&   ( cacheIsUpToDate )
      &&   ( PreviousSearchedFileContent != null ) )
      {
        return PreviousSearchedFileContent;
      }

      PreviousSearchedFileContent = GR.IO.File.ReadAllText( Filename );
      PreviousSearchedFile = Filename;
      return PreviousSearchedFileContent;
    }



  }
}
