using RetroDevStudio.Documents;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace RetroDevStudio.Dialogs
{
  public partial class FormFindReplace : BaseDocument
  {
    public enum FindTarget
    {
      [Description("Current selection")]
      CURRENT_SELECTION = 0,
      [Description( "Active document" )]
      ACTIVE_DOCUMENT = 1,
      [Description( "All opened documents" )]
      ALL_OPEN_DOCUMENTS = 2,
      [Description( "Whole project" )]
      FULL_PROJECT = 3,
      [Description( "Whole Solution" )]
      FULL_SOLUTION = 4
    }


    public class SearchLocation
    {
      public DocumentInfo   FoundInDocument = null;
      public int            StartPosition = -1;
      public int            Length = 0;
      public int            LineNumber = -1;
      public string         AdditionalInfo = "";
      public string         FoundLine = "";
      public bool           EndReached = false;



      public SearchLocation()
      {
        StartPosition   = -1;
        LineNumber      = -1;
        Length          = 0;
        EndReached      = false;
        FoundInDocument = null;
      }



      public SearchLocation( SearchLocation RHS )
      {
        StartPosition   = RHS.StartPosition;
        Length          = RHS.Length;
        LineNumber      = RHS.LineNumber;
        EndReached      = RHS.EndReached;
        FoundInDocument = RHS.FoundInDocument;
        AdditionalInfo  = RHS.AdditionalInfo;
        FoundLine       = RHS.FoundLine;
      }



      public SearchLocation( int startPos, int length )
      {
        StartPosition   = startPos;
        Length          = length;
        LineNumber      = -1;
        EndReached      = false;
        FoundInDocument = null;
      }



      public void Clear()
      {
        StartPosition   = -1;
        LineNumber      = -1;
        Length          = 0;
        EndReached      = false;
        FoundInDocument = null;
        AdditionalInfo  = "";
      }



      public void SetEndReached()
      {
        EndReached = true;
      }



      public bool IsSameLocation( SearchLocation RHS )
      {
        return ( ( RHS.FoundInDocument == FoundInDocument )
        &&       ( RHS.EndReached == EndReached )
        &&       ( RHS.Length == Length )
        &&       ( RHS.StartPosition == StartPosition ) );
      }
    };



    private SearchLocation    LastSearchFound = new SearchLocation();
    private SearchLocation    LastReplaceFound = new SearchLocation();

    private SearchLocation    PreviousSearchSelection = new SearchLocation();

    private string            _LastErrorMessage = "";



    public FormFindReplace( StudioCore Core )
    {
      this.Core = Core;

      InitializeComponent();

      GR.Image.DPIHandler.ResizeControlsForDPI( this );

      foreach ( FindTarget target in Enum.GetValues( typeof( FindTarget ) ) )
      {
        comboSearchTarget.Items.Add( GR.EnumHelper.GetDescription( target ) );
        comboReplaceTarget.Items.Add( GR.EnumHelper.GetDescription( target ) );
      }
      comboSearchTarget.SelectedIndex = (int)FindTarget.ACTIVE_DOCUMENT;
      comboReplaceTarget.SelectedIndex = (int)FindTarget.ACTIVE_DOCUMENT;
      comboSearchText.Focus();
      AcceptButton = btnFindNext;
    }



    private bool HasSearchableControl( DocumentInfo DocInfo )
    {
      if ( ( DocInfo.Type == ProjectElement.ElementType.ASM_SOURCE )
      ||   ( DocInfo.Type == ProjectElement.ElementType.BASIC_SOURCE )
      ||   ( DocInfo.Type == ProjectElement.ElementType.DISASSEMBLER ) )
      {
        return true;
      }
      return false;
    }



    public override Size GetPreferredSize( Size proposedSize )
    {
      return new Size( 366, 328 );
    }



    public void ClearLastState()
    {
      LastReplaceFound.Clear();
      LastSearchFound.Clear();
    }



    public void AdjustSettings( FastColoredTextBoxNS.FastColoredTextBox Edit )
    {
      if ( Edit.SelectionLength == 0 )
      {
        if ( comboSearchTarget.SelectedIndex == (int)FindTarget.CURRENT_SELECTION )
        {
          comboSearchTarget.SelectedIndex = (int)FindTarget.ACTIVE_DOCUMENT;
        }
        if ( comboReplaceTarget.SelectedIndex == (int)FindTarget.CURRENT_SELECTION )
        {
          comboReplaceTarget.SelectedIndex = (int)FindTarget.ACTIVE_DOCUMENT;
        }
      }
      else if ( !Edit.Selection.Text.Contains( "\n" ) )
      {
        // only single line
        comboSearchText.Text = Edit.Selection.Text;
        comboReplaceSearchText.Text = Edit.Selection.Text;
      }
    }



    private void btnFindNext_Click( DecentForms.ControlBase Sender )
    {
      // continue searching from cursor
      if ( Core.MainForm.ActiveDocumentInfo != null )
      {
        var edit = EditFromDocumentEx( Core.MainForm.ActiveDocumentInfo );

        if ( edit != null )
        {
          bool  hadLastFind = ( LastSearchFound.FoundInDocument != null );
          LastSearchFound.Clear();
          LastSearchFound.FoundInDocument = Core.MainForm.ActiveDocumentInfo;
          LastSearchFound.StartPosition = edit.PlaceToPosition( edit.Selection.Start );
          if ( hadLastFind )
          {
            ++LastSearchFound.StartPosition;
          }
        }
      }
      FindNext( null, comboSearchText.Text, radioSearchDirDown.Checked );
    }



    public void FindNext( BaseDocument DirectlyFromSourceFile )
    {
      if ( Core.MainForm.ActiveDocumentInfo != null )
      {
        var edit = EditFromDocumentEx( Core.MainForm.ActiveDocumentInfo );

        if ( edit != null )
        {
          bool  hadLastFind = ( LastSearchFound.FoundInDocument != null );
          LastSearchFound.Clear();
          LastSearchFound.FoundInDocument = Core.MainForm.ActiveDocumentInfo;
          LastSearchFound.StartPosition = edit.PlaceToPosition( edit.Selection.Start );
          if ( hadLastFind )
          {
            ++LastSearchFound.StartPosition;
          }
        }
      }
      FindNext( DirectlyFromSourceFile, comboSearchText.Text, true );
    }



    public void FindPrevious( BaseDocument DirectlyFromSourceFile )
    {
      if ( Core.MainForm.ActiveDocumentInfo != null )
      {
        var edit = EditFromDocumentEx( Core.MainForm.ActiveDocumentInfo );

        if ( edit != null )
        {
          bool  hadLastFind = ( LastSearchFound.FoundInDocument != null );
          LastSearchFound.Clear();
          LastSearchFound.FoundInDocument = Core.MainForm.ActiveDocumentInfo;
          LastSearchFound.StartPosition = edit.PlaceToPosition( edit.Selection.Start );
          if ( hadLastFind )
          {
            --LastSearchFound.StartPosition;
          }
        }
      }
      FindNext( DirectlyFromSourceFile, comboSearchText.Text, false );
    }



    public void FindNext( BaseDocument DirectlyFromSourceFile, string SearchText, bool searchForward )
    {
      Core.MainForm.WriteToLog( "FindNext " + SearchText + " with " + (FindTarget)comboSearchTarget.SelectedIndex );
      if ( ( LastSearchFound != null )
      &&   ( LastSearchFound.FoundInDocument != null ) )
      {
        Core.MainForm.WriteToLog( "-continue from " + LastSearchFound.FoundInDocument.DocumentFilename + " at " + LastSearchFound.StartPosition );
      }

      if ( FindNextNew( SearchText,
                        searchForward,
                        checkSearchRegExp.Checked,
                        checkSearchFullWords.Checked,
                        checkSearchIgnoreCase.Checked,
                        checkSearchWrap.Checked,
                        (FindTarget)comboSearchTarget.SelectedIndex,
                        DirectlyFromSourceFile,
                        LastSearchFound ) )
      {
        Core.MainForm.WriteToLog( "-found in " + LastSearchFound.FoundInDocument.DocumentFilename + " at " + LastSearchFound.StartPosition );

        bool keepFindActive = ( Core.MainForm.ActiveContent == Core.MainForm.m_FindReplace );
        if ( LastSearchFound.FoundInDocument == null )
        {
          LastSearchFound.FoundInDocument = DirectlyFromSourceFile.DocumentInfo.Project.ShowDocument( LastSearchFound.FoundInDocument.Element ).DocumentInfo;
        }
        if ( LastSearchFound.FoundInDocument == null )
        {
          Debug.Log( "Failed to find document from LastSearchFound:" + LastSearchFound.FoundInDocument + ", line " + LastSearchFound.FoundLine );
          return;
        }
        if ( LastSearchFound.FoundInDocument.BaseDoc == null )
        {
          if ( ( LastSearchFound.FoundInDocument.Element != null )
          &&   ( LastSearchFound.FoundInDocument.Project != null ) )
          {
            LastSearchFound.FoundInDocument.Element.Document = LastSearchFound.FoundInDocument.Project.ShowDocument( LastSearchFound.FoundInDocument.Element );
            LastSearchFound.FoundInDocument.BaseDoc.Show( Core.MainForm.panelMain );
          }
        }
        else
        {
          LastSearchFound.FoundInDocument.BaseDoc.Show( Core.MainForm.panelMain );
        }

        var edit = EditFromDocumentEx( LastSearchFound.FoundInDocument );

        //Debug.Log( "Found in " + LastSearchFound.FoundInDocument.FullPath + ", line " + LastSearchFound.LineNumber + ", pos " + LastSearchFound.StartPosition );
        var foundRange = RangeFromSearchLocation( edit, LastSearchFound );

        //if ( (FindTarget)comboSearchTarget.SelectedIndex != FindTarget.CURRENT_SELECTION )
        {
          // do not modify selection!
          if ( foundRange != null )
          {
            edit.Navigate( foundRange );
            edit.Selection = foundRange;
          }
        }

        if ( keepFindActive )
        {
          Core.MainForm.m_FindReplace.Show( Core.MainForm.panelMain );
        }
        return;
      }
      else
      {
        Core.MainForm.WriteToLog( "-not found" );

        if ( !string.IsNullOrEmpty( _LastErrorMessage ) )
        {
          Core.SetStatus( "A problem occurred: " + _LastErrorMessage );
        }
        else
        {
          Core.SetStatus( "Searched text not found: " + comboSearchText.Text );
        }
        Core.Notification.ItemNotFound();
      }
    }



    private void HistoriseSearchString( string SearchString )
    {
      bool foundInHistory = false;
      foreach ( string obj in comboSearchText.Items )
      {
        if ( obj == SearchString )
        {
          foundInHistory = true;
          break;
        }
      }
      if ( !foundInHistory )
      {
        while ( comboSearchText.Items.Count >= 50 )
        {
          comboSearchText.Items.RemoveAt( comboSearchText.Items.Count - 1 );
        }
        comboSearchText.Items.Insert( 0, SearchString );
      }

      foundInHistory = false;
      foreach ( string obj in comboReplaceSearchText.Items )
      {
        if ( obj == SearchString )
        {
          foundInHistory = true;
          break;
        }
      }
      if ( !foundInHistory )
      {
        while ( comboReplaceSearchText.Items.Count >= 50 )
        {
          comboReplaceSearchText.Items.RemoveAt( comboReplaceSearchText.Items.Count - 1 );
        }
        comboReplaceSearchText.Items.Insert( 0, SearchString );
      }
    }



    private void HistoriseReplaceString( string ReplaceString )
    {
      bool foundInHistory = false;
      foreach ( string obj in Core.Settings.ReplaceWithArguments )
      {
        if ( obj == ReplaceString )
        {
          foundInHistory = true;
          break;
        }
      }
      if ( !foundInHistory )
      {
        while ( Core.Settings.ReplaceWithArguments.Count >= 50 )
        {
          Core.Settings.ReplaceWithArguments.RemoveAt( Core.Settings.ReplaceWithArguments.Count - 1 );
        }
        Core.Settings.ReplaceWithArguments.Insert( 0, ReplaceString );
      }
    }



    private bool IsCharPartOfWord( char TestChar )
    {
      if ( ( TestChar == '_' )
      ||   ( ( TestChar >= 'A' )
      &&     ( TestChar <= 'Z' ) )
      ||   ( ( TestChar >= 'a' )
      &&     ( TestChar <= 'z' ) )
      ||   ( ( TestChar >= '0' )
      &&     ( TestChar <= '9' ) ) )
      {
        return true;
      }
      return false;
    }



    private SearchLocation FindNextOccurrence( string SearchSource,
                                               string SearchString, 
                                               bool RegularExpression, bool WholeWords, bool IgnoreCase, bool Upwards, int LastPosition )
    {
      _LastErrorMessage = "";
      if ( string.IsNullOrEmpty( SearchString ) )
      {
        return new SearchLocation();
      }

      int     startPos = LastPosition + 1;
      if ( Upwards )
      {
        startPos = LastPosition - 1;
      }
      if ( LastPosition <= 0 )
      {
        startPos = 0;
      }

      if ( ( LastPosition == -1 )
      &&   ( Upwards ) )
      {
        startPos = SearchSource.Length;
      }

      if ( RegularExpression )
      {
        System.Text.RegularExpressions.RegexOptions regexOptions = System.Text.RegularExpressions.RegexOptions.Multiline;
        if ( Upwards )
        {
          regexOptions |= System.Text.RegularExpressions.RegexOptions.RightToLeft;
        }
        if ( IgnoreCase )
        {
          regexOptions |= System.Text.RegularExpressions.RegexOptions.IgnoreCase;
        }
        System.Text.RegularExpressions.Regex regEx = null;

        try
        {
          regEx = new System.Text.RegularExpressions.Regex( SearchString, regexOptions );
        }
        catch ( Exception ex )
        {
          _LastErrorMessage = "Invalid Regular Expression: " + ex.Message;
          return new SearchLocation();
        }

        System.Text.RegularExpressions.Match match = null;
        if ( Upwards )
        {
          match = regEx.Match( SearchSource, 0, startPos );
        }
        else if ( startPos < SearchSource.Length )
        {
          match = regEx.Match( SearchSource, startPos );
        }
        else
        {
          return new SearchLocation();
        }
        if ( match.Success )
        {
          return new SearchLocation( match.Index, match.Length );
        }
        return new SearchLocation();
      }

      StringComparison    compareFlags = StringComparison.CurrentCulture;
      if ( IgnoreCase )
      {
        compareFlags = StringComparison.CurrentCultureIgnoreCase;
      }

      find_next:;
      int pos = -1;
      if ( startPos <= SearchSource.Length )
      {
        if ( Upwards )
        {
          pos = SearchSource.LastIndexOf( SearchString, startPos, compareFlags );
        }
        else
        {
          pos = SearchSource.IndexOf( SearchString, startPos, compareFlags );
        }
      }
      if ( pos == -1 )
      {
        return new SearchLocation();
      }
      if ( WholeWords )
      {
        bool    isWholeWord = true;
        if ( ( pos > 0 )
        &&   ( IsCharPartOfWord( SearchSource[pos - 1] ) ) )
        {
          isWholeWord = false;
        }
        if ( ( pos + SearchString.Length < SearchSource.Length )
        &&   ( IsCharPartOfWord( SearchSource[pos + SearchString.Length] ) ) )
        {
          isWholeWord = false;
        }
        if ( !isWholeWord )
        {
          startPos = pos + 1;
          if ( startPos >= SearchSource.Length )
          {
            return new SearchLocation();
          }
          goto find_next;
        }
      }

      return new SearchLocation( pos, SearchString.Length );
    }



    private FastColoredTextBoxNS.FastColoredTextBox EditFromDocumentEx( DocumentInfo Document )
    {
      if ( ( Document == null )
      ||   ( Document.BaseDoc == null ) )
      {
        return null;
      }
      return Document.BaseDoc.SourceControl;
    }



    private ProjectElement GetFirstProjectElement( bool SearchDown )
    {
      if ( Core.MainForm.CurrentProject == null )
      {
        return null;
      }
      if ( SearchDown )
      {
        foreach ( ProjectElement element in Core.MainForm.CurrentProject.Elements )
        {
          if ( !HasSearchableControl( element.DocumentInfo ) )
          {
            continue;
          }
          return element;
        }
        return null;
      }
      ProjectElement lastElement = null;
      foreach ( ProjectElement element in Core.MainForm.CurrentProject.Elements )
      {
        if ( !HasSearchableControl( element.DocumentInfo ) )
        {
          continue;
        }
        lastElement = element;
      }
      return lastElement;
    }



    private ProjectElement GetNextProjectElement( ProjectElement ElementToSearch, bool SearchDown, bool Wrap )
    {
      if ( SearchDown )
      {
        bool foundEntry = false;
        foreach ( ProjectElement element in Core.MainForm.CurrentProject.Elements )
        {
          if ( !element.DocumentInfo.ContainsCode )
          {
            continue;
          }
          if ( foundEntry )
          {
            return element;
          }
          if ( element == ElementToSearch )
          {
            foundEntry = true;
          }
        }
        if ( !Wrap )
        {
          return null;
        }
        foreach ( ProjectElement element in Core.MainForm.CurrentProject.Elements )
        {
          if ( !element.DocumentInfo.ContainsCode )
          {
            continue;
          }
          if ( element == ElementToSearch )
          {
            return null;
          }
          return element;
        }
        return null;
      }
      // find previous doc
      ProjectElement previousElement = null;
      foreach ( ProjectElement element in Core.MainForm.CurrentProject.Elements )
      {
        if ( !element.DocumentInfo.ContainsCode )
        {
          continue;
        }
        if ( element == ElementToSearch )
        {
          return previousElement;
        }
        previousElement = element;
      }
      if ( ( previousElement == null )
      &&   ( !Wrap ) )
      {
        return null;
      }
      foreach ( ProjectElement element in Core.MainForm.CurrentProject.Elements )
      {
        if ( !element.DocumentInfo.ContainsCode )
        {
          continue;
        }
        previousElement = element;
      }
      return previousElement;
    }



    private ProjectElement GetFirstSolutionElement( bool SearchDown )
    {
      if ( Core.Navigating.Solution == null )
      {
        return null;
      }
      if ( SearchDown )
      {
        foreach ( var project in Core.Navigating.Solution.Projects )
        {
          foreach ( ProjectElement element in project.Elements )
          {
            if ( !HasSearchableControl( element.DocumentInfo ) )
            {
              continue;
            }
            return element;
          }
        }
        return null;
      }
      ProjectElement lastElement = null;
      foreach ( var project in Core.Navigating.Solution.Projects )
      {
        foreach ( ProjectElement element in project.Elements )
        {
          if ( !HasSearchableControl( element.DocumentInfo ) )
          {
            continue;
          }
          lastElement = element;
        }
      }
      return lastElement;
    }



    private ProjectElement GetNextSolutionElement( ProjectElement ElementToSearch, bool SearchDown, bool Wrap )
    {
      if ( SearchDown )
      {
        bool foundEntry = false;
        foreach ( var project in Core.Navigating.Solution.Projects )
        {
          foreach ( ProjectElement element in project.Elements )
          {
            if ( !element.DocumentInfo.ContainsCode )
            {
              continue;
            }
            if ( foundEntry )
            {
              return element;
            }
            if ( element == ElementToSearch )
            {
              foundEntry = true;
            }
          }
        }
        if ( !Wrap )
        {
          return null;
        }
        foreach ( var project in Core.Navigating.Solution.Projects )
        {
          foreach ( ProjectElement element in project.Elements )
          {
            if ( !element.DocumentInfo.ContainsCode )
            {
              continue;
            }
            if ( element == ElementToSearch )
            {
              return null;
            }
            return element;
          }
        }
        return null;
      }
      // find previous doc
      ProjectElement previousElement = null;
      foreach ( var project in Core.Navigating.Solution.Projects )
      {
        foreach ( ProjectElement element in project.Elements )
        {
          if ( !element.DocumentInfo.ContainsCode )
          {
            continue;
          }
          if ( element == ElementToSearch )
          {
            return previousElement;
          }
          previousElement = element;
        }
      }
      if ( ( previousElement == null )
      &&   ( !Wrap ) )
      {
        return null;
      }
      foreach ( var project in Core.Navigating.Solution.Projects )
      {
        foreach ( ProjectElement element in project.Elements )
        {
          if ( !element.DocumentInfo.ContainsCode )
          {
            continue;
          }
          previousElement = element;
        }
      }
      return previousElement;
    }



    private BaseDocument GetFirstOpenDocument( bool SearchDown )
    {
      if ( SearchDown )
      {
        foreach ( BaseDocument doc in Core.MainForm.panelMain.Documents )
        {
          if ( !doc.DocumentInfo.ContainsCode )
          {
            continue;
          }
          return doc;
        }
        return null;
      }
      // find previous doc
      BaseDocument previousDoc = null;
      foreach ( BaseDocument doc in Core.MainForm.panelMain.Documents )
      {
        if ( !doc.DocumentInfo.ContainsCode )
        {
          continue;
        }
        previousDoc = doc;
      }
      return previousDoc;
    }



    private BaseDocument GetNextOpenDocument( BaseDocument DocToSearch, bool SearchDown, bool Wrap )
    {
      if ( SearchDown )
      {
        bool foundEntry = false;
        foreach ( BaseDocument doc in Core.MainForm.panelMain.Documents )
        {
          if ( !doc.DocumentInfo.ContainsCode )
          {
            continue;
          }
          if ( foundEntry )
          {
            return doc;
          }
          if ( doc == DocToSearch )
          {
            foundEntry = true;
          }
        }
        if ( !Wrap )
        {
          return null;
        }
        foreach ( BaseDocument doc in Core.MainForm.panelMain.Documents )
        {
          if ( !doc.DocumentInfo.ContainsCode )
          {
            continue;
          }
          if ( doc == DocToSearch )
          {
            return null;
          }
          return doc;
        }
        return null;
      }
      // find previous doc
      BaseDocument previousDoc = null;
      foreach ( BaseDocument doc in Core.MainForm.panelMain.Documents )
      {
        if ( !doc.DocumentInfo.ContainsCode )
        {
          continue;
        }
        if ( doc == DocToSearch )
        {
          return previousDoc;
        }
        previousDoc = doc;
      }
      if ( ( previousDoc == null )
      && ( !Wrap ) )
      {
        return null;
      }
      foreach ( BaseDocument doc in Core.MainForm.panelMain.Documents )
      {
        if ( !doc.DocumentInfo.ContainsCode )
        {
          continue;
        }
        previousDoc = doc;
      }
      return previousDoc;
    }



    private int PositionFromCharacterPos( FastColoredTextBoxNS.FastColoredTextBox Edit, int CharacterPos )
    {
      return CharacterPos;
    }



    private int CharacterPosFromPosition( FastColoredTextBoxNS.FastColoredTextBox Edit, FastColoredTextBoxNS.Place Position )
    {
      return Edit.PlaceToPosition( Position );
    }



    private FastColoredTextBoxNS.Range RangeFromSearchLocation( FastColoredTextBoxNS.FastColoredTextBox Edit, SearchLocation Location )
    {
      /*
      // pos to column
      int pos;
      int col = edit.GetColumn( pos );
      // column to pos??
      //edit.Lines[0].St*/
      if ( Edit == null )
      {
        return null;
      }
      int   start = Location.StartPosition;
      int   virtualStart = Edit.PositionToVirtualPosition( start );

      if ( ( virtualStart >= Edit.Text.Length )
      ||   ( virtualStart + Location.Length > Edit.Text.Length ) )
      {
        return null;
      }

      var end = start + Location.Length;

      FastColoredTextBoxNS.Range foundRange = new FastColoredTextBoxNS.Range( Edit,
                                                                              Edit.PositionToPlace( start ).iChar,
                                                                              Edit.PositionToPlace( start ).iLine,
                                                                              Edit.PositionToPlace( end ).iChar,
                                                                              Edit.PositionToPlace( end ).iLine );
      return foundRange;
    }



    private bool FindNextNew( string SearchString, 
                              bool SearchDown, 
                              bool RegularExpression, 
                              bool WholeWords, 
                              bool IgnoreCase, 
                              bool Wrap, 
                              FindTarget Target, 
                              BaseDocument DirectlyFromSourceFile, 
                              SearchLocation LastFound )
    {
      if ( string.IsNullOrEmpty( SearchString ) )
      {
        LastFound.Clear();
        return false;
      }
      BaseDocument activeDocument = Core.MainForm.ActiveSearchDocument;
      if ( DirectlyFromSourceFile != null )
      {
        activeDocument = DirectlyFromSourceFile;
      }
      // add search text to "history"
      HistoriseSearchString( SearchString );

      int lastPosition = LastFound.StartPosition + LastFound.Length; //.StartPosition;
      SearchLocation newLocation;
      string textFromElement = "";
      DocumentInfo  docInfoToSearch = null;
      bool createdDummyEdit = false;

      FastColoredTextBoxNS.FastColoredTextBox  edit = null;

      if ( IsTargettingProjectOrSolutionWithoutProject( Target ) )
      {
        // fall back - full solution without solution
        Target = FindTarget.ALL_OPEN_DOCUMENTS;
      }

      if ( Target == FindTarget.ACTIVE_DOCUMENT )
      {
        if ( activeDocument == null )
        {
          LastFound.Clear();
          return false;
        }
        docInfoToSearch = activeDocument.DocumentInfo;
        edit = EditFromDocumentEx( activeDocument.DocumentInfo );
        if ( edit != null )
        {
          if ( DirectlyFromSourceFile != null )
          {
            lastPosition = CharacterPosFromPosition( edit, edit.Selection.Start );
          }
          else if ( lastPosition != -1 )
          {
            // virtualize pos
            lastPosition = edit.PositionToVirtualPosition( lastPosition );
          }

          textFromElement = edit.Text;
        }
        else
        {
          if ( activeDocument.DocumentInfo.Type == ProjectElement.ElementType.BASIC_SOURCE )
          {
            if ( activeDocument.DocumentInfo.BaseDoc != null )
            {
              textFromElement = ( (SourceBasicEx)activeDocument.DocumentInfo.BaseDoc ).GetContentForSearch();
            }
          }
          else
          {
            textFromElement = activeDocument.GetContent();
          }
        }

        newLocation = FindNextOccurrence( textFromElement, SearchString, RegularExpression, WholeWords, IgnoreCase, !SearchDown, lastPosition );
        newLocation.FoundInDocument = activeDocument.DocumentInfo;
        if ( newLocation.StartPosition == -1 )
        {
          if ( Wrap )
          {
            if ( SearchDown )
            {
              newLocation = FindNextOccurrence( textFromElement, SearchString, RegularExpression, WholeWords, IgnoreCase, !SearchDown, 0 );
            }
            else
            {
              newLocation = FindNextOccurrence( textFromElement, SearchString, RegularExpression, WholeWords, IgnoreCase, !SearchDown, edit.Text.Length );
            }
          }
          if ( newLocation.StartPosition == -1 )
          {
            LastFound.Clear();
            return false;
          }
        }
      }
      else if ( Target == FindTarget.CURRENT_SELECTION )
      {
        if ( activeDocument == null )
        {
          LastFound.Clear();
          return false;
        }
        docInfoToSearch = activeDocument.DocumentInfo;
        edit = EditFromDocumentEx( activeDocument.DocumentInfo );
        if ( ( edit == null )
        ||   ( edit.SelectionLength == 0 ) )
        {
          LastFound.Clear();
          return false;
        }

        int       searchStart = edit.PlaceToPosition( edit.Selection.Start );
        int       searchEnd = edit.PlaceToPosition( edit.Selection.End );

        if ( searchEnd < searchStart )
        {
          int temp = searchStart;
          searchStart = searchEnd;
          searchEnd = temp;
        }

        if ( ( lastPosition >= edit.PlaceToPosition( edit.Selection.Start ) )
        &&   ( lastPosition < edit.PlaceToPosition( edit.Selection.End ) ) )
        {
          searchStart = lastPosition - edit.PlaceToPosition( edit.Selection.Start );
        }

        if ( searchStart != -1 )
        {
          // virtualize pos
          searchStart = edit.PositionToVirtualPosition( searchStart );
        }

        textFromElement = edit.Text;
        newLocation = FindNextOccurrence( textFromElement, SearchString, RegularExpression, WholeWords, IgnoreCase, !SearchDown, searchStart - 1 );
        newLocation.FoundInDocument = activeDocument.DocumentInfo;
        if ( newLocation.StartPosition == -1 )
        {
          LastFound.Clear();
          return false;
        }

        if ( ( edit.SelectionLength != PreviousSearchSelection.Length )
        ||   ( edit.PlaceToPosition( edit.Selection.Start ) != PreviousSearchSelection.StartPosition )
        ||   ( activeDocument != PreviousSearchSelection.FoundInDocument.BaseDoc ) )
        {
          // store the previous search selection (finding something changes the selection!)
          PreviousSearchSelection.StartPosition = edit.PlaceToPosition( edit.Selection.Start );
          PreviousSearchSelection.Length = edit.SelectionLength;
          PreviousSearchSelection.FoundInDocument = activeDocument.DocumentInfo;
        }
      }
      else if ( Target == FindTarget.ALL_OPEN_DOCUMENTS )
      {
        BaseDocument docToSearch = null;
        if ( LastFound.FoundInDocument != null )
        {
          docToSearch = LastFound.FoundInDocument.BaseDoc;
        }
        if ( docToSearch == null )
        {
          docToSearch = GetFirstOpenDocument( SearchDown );
          if ( docToSearch == null )
          {
            LastFound.Clear();
            return false;
          }
        }
        BaseDocument firstSearchedDoc = docToSearch;

        if ( lastPosition != -1 )
        {
          // virtualize pos
          edit = EditFromDocumentEx( docToSearch.DocumentInfo );

          lastPosition = edit.PositionToVirtualPosition( lastPosition );
        }

        retry_search:
        ;
        edit = EditFromDocumentEx( docToSearch.DocumentInfo );
        docInfoToSearch = docToSearch.DocumentInfo;

        textFromElement = edit.Text;
        if ( activeDocument.DocumentInfo.Type == ProjectElement.ElementType.BASIC_SOURCE )
        {
          if ( activeDocument.DocumentInfo.BaseDoc != null )
          {
            textFromElement = ( (SourceBasicEx)activeDocument.DocumentInfo.BaseDoc ).GetContentForSearch();
          }
        }

        newLocation = FindNextOccurrence( textFromElement, SearchString, RegularExpression, WholeWords, IgnoreCase, !SearchDown, lastPosition );
        newLocation.FoundInDocument = activeDocument.DocumentInfo;
        if ( newLocation.StartPosition == -1 )
        {
          docToSearch = GetNextOpenDocument( docToSearch, SearchDown, Wrap );
          if ( docToSearch == null )
          {
            LastFound.Clear();
            return false;
          }
          if ( docToSearch == firstSearchedDoc )
          {
            LastFound.Clear();
            return false;
          }
          lastPosition = -1;
          goto retry_search;
        }
      }
      else if ( Target == FindTarget.FULL_PROJECT )
      {
        ProjectElement  elementToSearch = null;
        ProjectElement  firstElement = null;
        if ( LastFound.FoundInDocument != null )
        {
          elementToSearch = LastFound.FoundInDocument.Element;
          firstElement = elementToSearch;
        }
        if ( elementToSearch == null )
        {
          elementToSearch = GetFirstProjectElement( SearchDown );
          firstElement = elementToSearch;
        }
        if ( ( elementToSearch == null )
        ||   ( string.IsNullOrEmpty( elementToSearch.Filename ) ) )
        {
          LastFound.Clear();
          return false;
        }

        if ( lastPosition != -1 )
        {
          // virtualize pos
          edit = EditFromDocumentEx( elementToSearch.DocumentInfo );
          if ( edit == null )
          {
            edit = new FastColoredTextBoxNS.FastColoredTextBox();
            edit.AllowTabs = true;
            edit.TabLength = Core.Settings.FormatSettings.TabSize;
            textFromElement = Core.Searching.GetDocumentInfoText( elementToSearch.DocumentInfo );
            if ( elementToSearch.DocumentInfo.Type == ProjectElement.ElementType.BASIC_SOURCE )
            {
              if ( elementToSearch.DocumentInfo.BaseDoc != null )
              {
                textFromElement = ( (SourceBasicEx)elementToSearch.DocumentInfo.BaseDoc ).GetContentForSearch();
              }
            }
            edit.Text = textFromElement;
            createdDummyEdit = true;
          }
          lastPosition = edit.PositionToVirtualPosition( lastPosition );
        }

        retry_search:
        ;
        textFromElement = Core.Searching.GetDocumentInfoText( elementToSearch.DocumentInfo );
        if ( elementToSearch.DocumentInfo.Type == ProjectElement.ElementType.BASIC_SOURCE )
        {
          if ( elementToSearch.DocumentInfo.BaseDoc != null )
          {
            textFromElement = ( (SourceBasicEx)elementToSearch.DocumentInfo.BaseDoc ).GetContentForSearch();
          }
        }

        newLocation = FindNextOccurrence( textFromElement, SearchString, RegularExpression, WholeWords, IgnoreCase, !SearchDown, lastPosition );
        newLocation.FoundInDocument = elementToSearch.DocumentInfo;
        if ( newLocation.StartPosition == -1 )
        {
          elementToSearch = GetNextProjectElement( elementToSearch, SearchDown, true );
          if ( elementToSearch == null )
          {
            LastFound.Clear();
            return false;
          }
          if ( elementToSearch == firstElement )
          {
            // back to the first element, give up
            LastFound.Clear();
            return false;
          }
          lastPosition = -1;
          edit = null;
          goto retry_search;
        }
        docInfoToSearch = elementToSearch.DocumentInfo;
      }
      else if ( Target == FindTarget.FULL_SOLUTION )
      {
        ProjectElement  elementToSearch = null;
        ProjectElement  firstElement = null;
        Project         firstProject = null;

        if ( LastFound.FoundInDocument != null )
        {
          elementToSearch = LastFound.FoundInDocument.Element;
          firstElement = elementToSearch;
          if ( elementToSearch != null )
          {
            firstProject = elementToSearch.DocumentInfo.Project;
          }
        }
        if ( elementToSearch == null )
        {
          elementToSearch = GetFirstSolutionElement( SearchDown );
          firstElement = elementToSearch;
          firstProject = elementToSearch.DocumentInfo.Project;
        }
        if ( ( elementToSearch == null )
        ||   ( string.IsNullOrEmpty( elementToSearch.Filename ) ) )
        {
          LastFound.Clear();
          return false;
        }

        if ( lastPosition != -1 )
        {
          // virtualize pos
          edit = EditFromDocumentEx( elementToSearch.DocumentInfo );
          if ( edit == null )
          {
            edit = new FastColoredTextBoxNS.FastColoredTextBox();
            edit.AllowTabs = true; // Core.Settings.AllowTabs;
            edit.TabLength = Core.Settings.FormatSettings.TabSize;

            textFromElement = Core.Searching.GetDocumentInfoText( elementToSearch.DocumentInfo );
            if ( elementToSearch.DocumentInfo.Type == ProjectElement.ElementType.BASIC_SOURCE )
            {
              if ( elementToSearch.DocumentInfo.BaseDoc != null )
              {
                textFromElement = ( (SourceBasicEx)elementToSearch.DocumentInfo.BaseDoc ).GetContentForSearch();
              }
            }
            edit.Text = textFromElement;
            createdDummyEdit = true;
          }
          lastPosition = edit.PositionToVirtualPosition( lastPosition );
        }

        retry_search:
        ;
        textFromElement = Core.Searching.GetDocumentInfoText( elementToSearch.DocumentInfo );
        if ( elementToSearch.DocumentInfo.Type == ProjectElement.ElementType.BASIC_SOURCE )
        {
          if ( elementToSearch.DocumentInfo.BaseDoc != null )
          {
            textFromElement = ( (SourceBasicEx)elementToSearch.DocumentInfo.BaseDoc ).GetContentForSearch();
          }
        }

        newLocation = FindNextOccurrence( textFromElement, SearchString, RegularExpression, WholeWords, IgnoreCase, !SearchDown, lastPosition );
        newLocation.FoundInDocument = elementToSearch.DocumentInfo;
        if ( newLocation.StartPosition == -1 )
        {
          elementToSearch = GetNextSolutionElement( elementToSearch, SearchDown, Wrap );
          if ( elementToSearch == null )
          {
            LastFound.Clear();
            return false;
          }
          if ( elementToSearch == firstElement )
          {
            // back to the first element, give up
            LastFound.Clear();
            return false;
          }
          lastPosition = -1;
          edit = null;
          goto retry_search;
        }
        docInfoToSearch = elementToSearch.DocumentInfo;
      }
      else
      {
        // not handled yet
        LastFound.Clear();
        return false;
      }

      // find line from pos
      FindLineAndTextFromResult( newLocation, LastFound, textFromElement );

      // urgh - so we can use virtualpositiontoposition
      
      if ( edit == null )
      {
        edit = new FastColoredTextBoxNS.FastColoredTextBox();
        edit.AllowTabs = true;
        edit.TabLength = Core.Settings.FormatSettings.TabSize;

        edit.Text = textFromElement;
        createdDummyEdit = true;
      }

      var start = edit.VirtualPositionToPosition( newLocation.StartPosition );
      var end = edit.VirtualPositionToPosition( newLocation.StartPosition + newLocation.Length );

      newLocation.StartPosition = start;
      newLocation.Length        = end - start;
      newLocation.LineNumber    = LastFound.LineNumber;

      LastFound.FoundInDocument = docInfoToSearch;
      LastFound.StartPosition   = newLocation.StartPosition;
      LastFound.Length          = newLocation.Length;
      LastFound.LineNumber      = newLocation.LineNumber;

      if ( createdDummyEdit )
      {
        edit.Dispose();
      }
      return true;
    }



    private bool IsTargettingProjectOrSolutionWithoutProject( FindTarget Target )
    {
      if ( ( ( Target == FindTarget.FULL_PROJECT )
      &&     ( Core.Navigating.Solution == null ) )
      ||   ( ( Target == FindTarget.FULL_SOLUTION )
      &&     ( Core.Navigating.Solution == null ) ) )
      {
        return true;
      }
      return false;
    }



    private void FindLineAndTextFromResult( SearchLocation NewLocation, SearchLocation LastFound, string TextToSearch )
    {
      // find line number from text
      int numLines = 0;
      int curPos = 0;
      int lastPos = -1;

      while ( curPos < NewLocation.StartPosition )
      {
        lastPos = curPos;
        curPos = TextToSearch.IndexOf( '\n', curPos + 1 );
        ++numLines;
        if ( curPos == -1 )
        {
          // not found??
          break;
        }
      }
      if ( numLines == 0 )
      {
        numLines = 1;
      }

      // try to find zone
      if ( ( NewLocation.FoundInDocument != null )
      &&   ( NewLocation.FoundInDocument.Type == ProjectElement.ElementType.ASM_SOURCE )
      &&   ( NewLocation.FoundInDocument.ASMFileInfo != null ) )
      {
        if ( NewLocation.FoundInDocument.ASMFileInfo.FindZoneInfoFromDocumentLine( NewLocation.FoundInDocument.FullPath, numLines, out string zone, out string cheapLabelZone ) )
        {
          LastFound.AdditionalInfo = "zone " + zone;
        }
      }
      LastFound.LineNumber = numLines;
      if ( ( curPos != -1 )
      &&   ( lastPos != -1 ) )
      {
        LastFound.FoundLine = TextToSearch.Substring( lastPos + 1, curPos - lastPos - 2 );
      }
      else
      {
        int   nextLineBreak = TextToSearch.IndexOf( '\n', lastPos + 1 );
        if ( nextLineBreak == -1 )
        {
          LastFound.FoundLine = TextToSearch.Substring( lastPos + 1 );
        }
        else
        {
          LastFound.FoundLine = TextToSearch.Substring( lastPos + 1, nextLineBreak - lastPos - 1 ).TrimEnd();
        }
      }
    }



    private void btnSearchBookmark_Click( DecentForms.ControlBase Sender )
    {
      while ( true )
      {
        // TODO - das gibt noch eine Endlosschleife!
        if ( !FindNextNew( comboSearchText.Text,
                           radioSearchDirDown.Checked,
                           checkSearchRegExp.Checked,
                           checkSearchFullWords.Checked,
                           checkSearchIgnoreCase.Checked,
                           false,
                           (FindTarget)comboSearchTarget.SelectedIndex,
                           null,
                           LastSearchFound ) )
        {
          return;
        }
        var edit = EditFromDocumentEx( LastSearchFound.FoundInDocument );
        var searchFound =  RangeFromSearchLocation( edit, LastSearchFound );

        edit.Bookmarks.Add( searchFound.Start.iLine );
      }
    }



    private void tabFindReplace_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( tabFindReplace.SelectedIndex == 0 )
      {
        comboSearchText.Focus();
        AcceptButton = btnFindNext;
      }
      else if ( tabFindReplace.SelectedIndex == 1 )
      {
        comboReplaceSearchText.Focus();
        AcceptButton = btnReplaceFindNext;
      }
    }



    public void Fill( StudioSettings Settings )
    {
      checkSearchFullWords.Checked  = Settings.LastFindWholeWord;
      checkSearchIgnoreCase.Checked = Settings.LastFindIgnoreCase;
      checkSearchRegExp.Checked     = Settings.LastFindRegexp;
      checkSearchWrap.Checked       = Settings.LastFindWrap;

      comboSearchTarget.SelectedIndex = Settings.LastFindTarget;
      comboSearchText.Items.Clear();
      comboSearchText.Items.AddRange( Settings.FindArguments.ToArray() );
      comboReplaceSearchText.Items.Clear();
      comboReplaceSearchText.Items.AddRange( Settings.ReplaceArguments.ToArray() );
    }



    public void ToSettings( StudioSettings Settings )
    {
      Settings.LastFindWholeWord = checkSearchFullWords.Checked;
      Settings.LastFindIgnoreCase = checkSearchIgnoreCase.Checked;
      Settings.LastFindRegexp = checkSearchRegExp.Checked;
      Settings.LastFindWrap = checkSearchWrap.Checked;

      Settings.LastFindTarget = comboSearchTarget.SelectedIndex;
      Settings.FindArguments.Clear();
      foreach ( object obj in comboSearchText.Items )
      {
        Settings.FindArguments.Add( (string)obj );
      }
      Settings.ReplaceArguments.Clear();
      foreach ( object obj in comboReplaceSearchText.Items )
      {
        Settings.ReplaceArguments.Add( (string)obj );
      }
    }



    private void FormFindReplace_VisibleChanged( object sender, EventArgs e )
    {
      if ( Visible )
      {
        comboSearchText.Focus();
      }
    }



    private void FormFindReplace_KeyDown( object sender, KeyEventArgs e )
    {
      if ( e.KeyCode == Keys.Escape )
      {
        this.Hide();
        e.Handled = true;
      }
    }



    private void checkReplaceIgnoreCase_CheckedChanged( object sender, EventArgs e )
    {
      checkSearchIgnoreCase.Checked = checkReplaceIgnoreCase.Checked;
    }



    private void checkReplaceWholeWords_CheckedChanged( object sender, EventArgs e )
    {
      checkSearchFullWords.Checked = checkReplaceWholeWords.Checked;
    }



    private void checkReplaceRegexp_CheckedChanged( object sender, EventArgs e )
    {
      checkSearchRegExp.Checked = checkReplaceRegexp.Checked;
    }



    private void checkReplaceWrap_CheckedChanged( object sender, EventArgs e )
    {
      checkSearchWrap.Checked = checkReplaceWrap.Checked;
    }



    private void radioReplaceSearchUp_CheckedChanged( object sender, EventArgs e )
    {
      radioSearchDirUp.Checked = radioReplaceSearchUp.Checked;
    }



    private void radioReplaceSearchDown_CheckedChanged( object sender, EventArgs e )
    {
      radioSearchDirDown.Checked = radioReplaceSearchDown.Checked;
    }



    private void checkSearchIgnoreCase_CheckedChanged( object sender, EventArgs e )
    {
      checkReplaceIgnoreCase.Checked = checkSearchIgnoreCase.Checked;
    }



    private void checkSearchFullWords_CheckedChanged( object sender, EventArgs e )
    {
      checkReplaceWholeWords.Checked = checkSearchFullWords.Checked;
    }



    private void checkSearchRegExp_CheckedChanged( object sender, EventArgs e )
    {
      checkReplaceRegexp.Checked = checkReplaceRegexp.Checked;
    }



    private void checkSearchWrap_CheckedChanged( object sender, EventArgs e )
    {
      checkReplaceWrap.Checked = checkSearchWrap.Checked;
    }



    private void radioSearchDirUp_CheckedChanged( object sender, EventArgs e )
    {
      radioReplaceSearchUp.Checked = radioSearchDirUp.Checked;
    }



    private void radioSearchDirDown_CheckedChanged( object sender, EventArgs e )
    {
      radioReplaceSearchDown.Checked = radioSearchDirDown.Checked;
    }



    private void btnReplaceFindNext_Click( DecentForms.ControlBase Sender )
    {
      // continue searching from cursor
      if ( Core.MainForm.ActiveDocumentInfo != null )
      {
        var edit = EditFromDocumentEx( Core.MainForm.ActiveDocumentInfo );

        if ( edit != null )
        {
          bool  hadLastFind = ( LastReplaceFound.FoundInDocument != null );
          LastReplaceFound.Clear();
          LastReplaceFound.FoundInDocument = Core.MainForm.ActiveDocumentInfo;
          LastReplaceFound.StartPosition = edit.PlaceToPosition( edit.Selection.Start );
          if ( hadLastFind )
          {
            ++LastReplaceFound.StartPosition;
          }
        }
      }

      if ( FindNextNew( comboReplaceSearchText.Text,
                        radioReplaceSearchDown.Checked,
                        checkReplaceRegexp.Checked,
                        checkReplaceWholeWords.Checked,
                        checkReplaceIgnoreCase.Checked,
                        checkReplaceWrap.Checked,
                        (FindTarget)comboReplaceTarget.SelectedIndex,
                        null,
                        LastReplaceFound ) )
      {
        if ( LastReplaceFound.FoundInDocument == null )
        {
          Debug.Log( "Failed to find document from LastReplaceFound:" + LastReplaceFound.FoundInDocument + ", line " + LastReplaceFound.FoundLine );
          return;
        }
        if ( LastReplaceFound.FoundInDocument.BaseDoc == null )
        {
          if ( ( LastReplaceFound.FoundInDocument.Element != null )
          &&   ( LastReplaceFound.FoundInDocument.Project != null ) )
          {
            LastReplaceFound.FoundInDocument.Element.Document = LastReplaceFound.FoundInDocument.Project.ShowDocument( LastReplaceFound.FoundInDocument.Element );
            LastReplaceFound.FoundInDocument.BaseDoc.Show( Core.MainForm.panelMain );
          }
        }
        else
        {
          LastReplaceFound.FoundInDocument.BaseDoc.Show( Core.MainForm.panelMain );
        }

        var    edit = EditFromDocumentEx( LastReplaceFound.FoundInDocument );

        var foundRange =  RangeFromSearchLocation( edit, LastReplaceFound );

        edit.Navigate( foundRange );
        edit.Selection = foundRange;
        return;
      }
      else
      {
        Core.MainForm.WriteToLog( "-not found" );

        if ( !string.IsNullOrEmpty( _LastErrorMessage ) )
        {
          Core.SetStatus( "A problem occurred: " + _LastErrorMessage );
        }
        else
        {
          Core.SetStatus( "Searched text not found: " + comboReplaceSearchText.Text );
        }
        Core.Notification.ItemNotFound();
      }
    }



    private void btnReplaceNext_Click( DecentForms.ControlBase Sender )
    {
      // continue searching from cursor
      if ( Core.MainForm.ActiveDocumentInfo != null )
      {
        var edit = EditFromDocumentEx( Core.MainForm.ActiveDocumentInfo );

        if ( edit != null )
        {
          LastReplaceFound.Clear();
          LastReplaceFound.FoundInDocument = Core.MainForm.ActiveDocumentInfo;
          LastReplaceFound.StartPosition = edit.PlaceToPosition( edit.Selection.Start );

          // allow to re-find the current highlight
          var curPos = edit.Selection.Start;
          if ( LastReplaceFound.StartPosition > 0 )
          {
            // go left (a tab means more than one step!)
            if ( ( edit.AllowTabs )
            &&   ( curPos.iChar > 0 )
            &&   ( curPos.iChar - 1 < edit[curPos.iLine].Count )
            &&   ( edit[curPos.iLine][curPos.iChar - 1].c == '\t' ) )
            {
              int delta = curPos.iChar % edit.TabLength;
              if ( delta == 0 )
              {
                delta = edit.TabLength;
              }
              for ( int i = 0; i < delta; ++i )
              {
                if ( edit[curPos.iLine][curPos.iChar - 1].c == '\t' )
                {
                  curPos.Offset( -1, 0 );
                }
                else
                {
                  break;
                }
              }
            }
            else
            {
                curPos.Offset( -1, 0 );
            }
            LastReplaceFound.StartPosition = edit.PlaceToPosition( curPos );
          }
        }
      }

      HistoriseSearchString( comboReplaceSearchText.Text );
      HistoriseReplaceString( editReplaceWith.Text );

      if ( FindNextNew( comboReplaceSearchText.Text,
                        radioReplaceSearchDown.Checked,
                        checkReplaceRegexp.Checked,
                        checkReplaceWholeWords.Checked,
                        checkReplaceIgnoreCase.Checked,
                        checkReplaceWrap.Checked,
                        (FindTarget)comboReplaceTarget.SelectedIndex,
                        null,
                        LastReplaceFound ) )
      {
        var    edit = EditFromDocumentEx( LastReplaceFound.FoundInDocument );
        LastReplaceFound.FoundInDocument.BaseDoc.Show( Core.MainForm.panelMain );

        var replaceFound =  RangeFromSearchLocation( edit, LastReplaceFound );

        if ( replaceFound != null )
        {
          // store old selection
          var oldSelectionStart = edit.Selection.Start;
          var oldSelectionEnd = edit.Selection.End;
          int oldEnd = edit.PlaceToPosition( oldSelectionEnd );

          edit.Selection = replaceFound;
          edit.SelectedText = editReplaceWith.Text;

          // set location to after replaced text to avoid recursion
          LastReplaceFound.StartPosition += editReplaceWith.Text.Length;

          // restore old selection
          int newEnd = oldEnd + editReplaceWith.Text.Length - comboReplaceSearchText.Text.Length;
          var newPlace = edit.PositionToPlace( newEnd );

          edit.Selection.Start = oldSelectionStart;
          edit.Selection.End = newPlace;

          // automatically find next
          btnReplaceFindNext_Click( btnReplaceFindNext );
        }
        return;
      }
    }



    private void btnReplaceAll_Click( DecentForms.ControlBase Sender )
    {
      int occurrences = 0;
      Core.Searching.ClearSearchResults();

      FindTarget    replaceTarget = (FindTarget)comboReplaceTarget.SelectedIndex;

      HistoriseSearchString( comboReplaceSearchText.Text );
      HistoriseReplaceString( editReplaceWith.Text );

      if ( replaceTarget == FindTarget.CURRENT_SELECTION )
      {
        BaseDocument    docToReplaceIn = Core.MainForm.ActiveDocument;
        if ( docToReplaceIn == null )
        {
          return;
        }
        var edit = EditFromDocumentEx( docToReplaceIn.DocumentInfo );
        if ( edit == null )
        {
          return;
        }
        if ( edit.SelectionLength == 0 )
        {
          replaceTarget = FindTarget.ACTIVE_DOCUMENT;
        }
        else
        {
          // replace inside selection only
          string    replacement = edit.Selection.Text;

          // store old selection
          var oldSelectionStart = edit.Selection.Start;
          var oldSelectionEnd = edit.Selection.End;
          int oldEnd = edit.PlaceToPosition( oldSelectionEnd );


          string    replacedText = ReplaceTextInString( docToReplaceIn.DocumentInfo,
                                                        replacement,
                                                        comboReplaceSearchText.Text,
                                                        editReplaceWith.Text,
                                                        checkReplaceRegexp.Checked,
                                                        checkReplaceWholeWords.Checked,
                                                        checkReplaceIgnoreCase.Checked,
                                                        out occurrences );

          edit.SelectedText = replacedText;

          // restore old selection
          int newEnd = oldEnd + ( editReplaceWith.Text.Length - comboReplaceSearchText.Text.Length ) * occurrences;
          var newPlace = edit.PositionToPlace( newEnd );

          edit.Selection.Start = oldSelectionStart;
          edit.Selection.End = newPlace;


          Core.AddToOutput( "Replaced " + occurrences + " occurrences." + System.Environment.NewLine );
          return;
        }
      }

      Dictionary<DocumentInfo,string>     elementsToReplaceIn = new Dictionary<DocumentInfo,string>();
 
      switch ( replaceTarget )
      {
        case FindTarget.ACTIVE_DOCUMENT:
          {
            DocumentInfo    docInfoToReplaceIn = Core.MainForm.ActiveDocumentInfo;
            if ( docInfoToReplaceIn == null )
            {
              return;
            }
            string textFromElement = Core.Searching.GetDocumentInfoText( docInfoToReplaceIn );
            elementsToReplaceIn.Add( docInfoToReplaceIn, textFromElement );
          }
          break;
        case FindTarget.ALL_OPEN_DOCUMENTS:
          foreach ( BaseDocument doc in Core.MainForm.panelMain.Documents )
          {
            if ( ( doc.DocumentInfo.Type == ProjectElement.ElementType.ASM_SOURCE )
            ||   ( doc.DocumentInfo.Type == ProjectElement.ElementType.BASIC_SOURCE )
            ||   ( doc.DocumentInfo.Type == ProjectElement.ElementType.DISASSEMBLER ) )
            {
              string textFromElement = Core.Searching.GetDocumentInfoText( doc.DocumentInfo );
              elementsToReplaceIn.Add( doc.DocumentInfo, textFromElement );
            }
          }
          break;
        case FindTarget.FULL_PROJECT:
          if ( Core.Navigating.Solution != null )
          {
            foreach ( Project proj in Core.Navigating.Solution.Projects )
            {
              foreach ( ProjectElement element in proj.Elements )
              {
                DocumentInfo    docInfo = element.DocumentInfo;
                if ( ( docInfo.Type == ProjectElement.ElementType.ASM_SOURCE )
                ||   ( docInfo.Type == ProjectElement.ElementType.BASIC_SOURCE )
                ||   ( docInfo.Type == ProjectElement.ElementType.DISASSEMBLER ) )
                {
                  string textFromElement = Core.Searching.GetDocumentInfoText( docInfo );
                  elementsToReplaceIn.Add( docInfo, textFromElement );
                }
              }
            }
          }
          break;
        case FindTarget.FULL_SOLUTION:
          if ( Core.Navigating.Solution != null )
          {
            foreach ( Project proj in Core.Navigating.Solution.Projects )
            {
              foreach ( ProjectElement element in proj.Elements )
              {
                DocumentInfo    docInfo = element.DocumentInfo;
                if ( ( docInfo.Type == ProjectElement.ElementType.ASM_SOURCE )
                ||   ( docInfo.Type == ProjectElement.ElementType.BASIC_SOURCE )
                ||   ( docInfo.Type == ProjectElement.ElementType.DISASSEMBLER ) )
                {
                  string textFromElement = Core.Searching.GetDocumentInfoText( docInfo );
                  elementsToReplaceIn.Add( docInfo, textFromElement );
                }
              }
            }
          }
          break;
      }

      foreach ( var doc in elementsToReplaceIn )
      {
        string    replacement     = doc.Value;
        int       docOccurrences  = 0;

        string    replacedText = ReplaceTextInString( doc.Key,
                                                      replacement,
                                                      comboReplaceSearchText.Text,
                                                      editReplaceWith.Text,
                                                      checkReplaceRegexp.Checked,
                                                      checkReplaceWholeWords.Checked,
                                                      checkReplaceIgnoreCase.Checked,
                                                      out docOccurrences );

        if ( docOccurrences > 0 )
        {
          if ( string.IsNullOrEmpty( replacedText ) )
          {
            Core.AddToOutput( "Replaced text was empty!!!!" );
            Core.AddToOutput( "  Replacing '" + comboReplaceSearchText.Text + "'" );
            Core.AddToOutput( "  with '" + editReplaceWith.Text + "'" );
            Core.AddToOutput( "  RegEx " + checkReplaceRegexp.Checked + ", checkReplaceWholeWords.Checked " + checkReplaceWholeWords.Checked + ", checkReplaceIgnoreCase.Checked " + checkReplaceIgnoreCase.Checked );
            Core.AddToOutput( "Original Text was '" + replacement + "'" );
            Core.AddToOutput( "Replaced text was empty!!!!" );
          }

          SetDocumentText( doc.Key, replacedText );
        }

        occurrences += docOccurrences;
      }
      Core.AddToOutput( "Replaced " + occurrences + " occurrences." + System.Environment.NewLine );
    }



    private void SetDocumentText( DocumentInfo DocumentInfo, string NewText )
    {
      DocumentInfo.HasBeenSuccessfullyBuilt = false;
      if ( DocumentInfo.BaseDoc != null )
      {
        // easy - replace in edit
        DocumentInfo.BaseDoc.FillContent( NewText, true, true );
        return;
      }
      // write to file
      try
      {
        System.IO.File.WriteAllText( DocumentInfo.FullPath, NewText );
      }
      catch ( System.Exception ex )
      {
        Core.AddToOutput( "Error writing replacement to file " + DocumentInfo.FullPath + System.Environment.NewLine + ex.Message + System.Environment.NewLine );
      }
    }



    private string ReplaceTextInString( DocumentInfo DocInfo, 
                                        string TextToSearchIn,
                                        string StringToFind, 
                                        string StringToReplaceWith, 
                                        bool RegexAllowed, 
                                        bool WholeWords, 
                                        bool IgnoreCase,
                                        out int NumOccurences )
    {
      NumOccurences = 0;

      var searchResults = new List<SearchLocation>();

      SearchLocation      lookLocation = new SearchLocation();
      LastReplaceFound.Clear();
      while ( true )
      {
        SearchLocation newLocation = FindNextOccurrence( TextToSearchIn,
                                                         StringToFind,
                                                         RegexAllowed,
                                                         WholeWords,
                                                         IgnoreCase,
                                                         false,
                                                         LastReplaceFound.StartPosition );
        if ( newLocation.StartPosition == -1 )
        {
          Core.Searching.AddSearchResults( searchResults );
          return TextToSearchIn;
        }

        FindLineAndTextFromResult( newLocation, LastReplaceFound, TextToSearchIn );

        searchResults.Add( new SearchLocation()
          {
            FoundInDocument = DocInfo,
            LineNumber = LastReplaceFound.LineNumber,
            StartPosition = LastReplaceFound.StartPosition,
            Length = LastReplaceFound.Length,
            FoundLine = LastReplaceFound.FoundLine
          } );
                
        TextToSearchIn = TextToSearchIn.Substring( 0, newLocation.StartPosition ) 
                        + StringToReplaceWith 
                        + TextToSearchIn.Substring( newLocation.StartPosition + newLocation.Length );

        // offset start pos to new string
        LastReplaceFound.StartPosition = newLocation.StartPosition;
        // -1 because FindNextNew always does +1 to avoid re-finding the exact same entry
        LastReplaceFound.StartPosition += StringToReplaceWith.Length - 1;

        ++NumOccurences;
      }
    }



    private void btnFindAll_Click( DecentForms.ControlBase Sender )
    {
      int                     occurrences = 0;
      SearchLocation          firstFoundRange = null;
      List<SearchLocation>    results = new List<SearchLocation>();

      LastSearchFound.Clear();

      Core.Searching.ClearSearchResults();

      Core.AddToOutput( "Searching for " + comboSearchText.Text + System.Environment.NewLine );
      while ( true )
      {
        if ( !FindNextNew( comboSearchText.Text,
                           radioSearchDirDown.Checked,
                           checkSearchRegExp.Checked,
                           checkSearchFullWords.Checked,
                           checkSearchIgnoreCase.Checked,
                           false,
                           (FindTarget)comboSearchTarget.SelectedIndex,
                           null,
                           LastSearchFound ) )
        {
          if ( !string.IsNullOrEmpty( _LastErrorMessage ) )
          {
            Core.AddToOutput( "A problem occurred: " + _LastErrorMessage + System.Environment.NewLine );
          }
          Core.AddToOutput( "Found " + occurrences + " occurrences" + System.Environment.NewLine );
          if ( occurrences > 0 )
          {
            Core.Searching.AddSearchResults( results );
            Core.MainForm.m_SearchResults.SearchComplete();
            Core.MainForm.m_SearchResults.Show( Core.MainForm.panelMain );
          }
          else
          {
            Core.MainForm.m_Output.Show( Core.MainForm.panelMain );
          }
          return;
        }
        if ( firstFoundRange == null )
        {
          firstFoundRange = new SearchLocation( LastSearchFound );
        }
        else if ( firstFoundRange.IsSameLocation( LastSearchFound ) )
        {
          Core.AddToOutput( "Found " + occurrences + " occurrences" + System.Environment.NewLine );
          if ( occurrences > 0 )
          {
            Core.Searching.AddSearchResults( results );
            Core.MainForm.m_SearchResults.SearchComplete();
            Core.MainForm.m_SearchResults.Show( Core.MainForm.panelMain );
          }
          else
          {
            Core.MainForm.m_Output.Show( Core.MainForm.panelMain );
          }
          return;
        }

        ++occurrences;

        results.Add( new SearchLocation( LastSearchFound ) );
      }
    }



    private void comboReplaceTarget_SelectedIndexChanged( object sender, EventArgs e )
    {
      checkReplaceWrap.Enabled = ( ( comboReplaceTarget.SelectedIndex != (int)FindTarget.FULL_SOLUTION )
                                && ( comboReplaceTarget.SelectedIndex != (int)FindTarget.FULL_PROJECT ) );
      if ( !checkReplaceWrap.Enabled )
      {
        checkReplaceWrap.Checked = false;
      }
    }



    internal void CursorWasMoved( DocumentInfo Doc, int LineNumber, int CharPos )
    {
      LastReplaceFound.FoundInDocument = Doc;
      LastReplaceFound.LineNumber = LineNumber;
      LastReplaceFound.StartPosition = CharPos;

      LastSearchFound.FoundInDocument = Doc;
      LastSearchFound.LineNumber = LineNumber;
      LastSearchFound.StartPosition = CharPos;
    }



    private void editReplaceTarget_TextChanged( object sender, EventArgs e )
    {
      DetectTextChange( editReplaceWith, Core.Settings.ReplaceWithArguments );
    }



    public override void OnApplicationEvent( ApplicationEvent Event )
    {
      switch ( Event.EventType )
      {
        case ApplicationEvent.Type.SEARCH_HISTORY_UPDATED:
          comboSearchText.Items.Clear();
          comboSearchText.Items.AddRange( Core.Settings.FindArguments.ToArray() );
          break;
          case ApplicationEvent.Type.REPLACE_SEARCH_HISTORY_UPDATED:
          comboReplaceSearchText.Items.Clear();
          comboReplaceSearchText.Items.AddRange( Core.Settings.ReplaceArguments.ToArray() );
          break;
      }
      base.OnApplicationEvent( Event );
    }



  }
}
