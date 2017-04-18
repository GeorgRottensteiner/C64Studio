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


  }
}
