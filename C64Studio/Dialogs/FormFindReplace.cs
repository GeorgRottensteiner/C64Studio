using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace C64Studio
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
      FULL_SOLUTION = 3
    }


    public class SearchLocation
    {
      public DocumentInfo   FoundInDocument = null;
      public int            StartPosition = -1;
      public int            Length = 0;
      public int            LineNumber = -1;
      public string         FoundLine = "";
      public bool           EndReached = false;



      public SearchLocation()
      {
        StartPosition = -1;
        LineNumber = -1;
        Length = 0;
        EndReached = false;
        FoundInDocument = null;
      }



      public SearchLocation( SearchLocation RHS )
      {
        StartPosition = RHS.StartPosition;
        Length = RHS.Length;
        LineNumber = RHS.LineNumber;
        EndReached = RHS.EndReached;
        FoundInDocument = RHS.FoundInDocument;
        FoundLine = RHS.FoundLine;
      }



      public SearchLocation( int StartPos, int Length )
      {
        this.StartPosition = StartPos;
        this.Length = Length;
        LineNumber = -1;
        EndReached = false;
        FoundInDocument = null;
      }



      public void Clear()
      {
        StartPosition = -1;
        LineNumber = -1;
        Length = 0;
        EndReached = false;
        FoundInDocument = null;
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



    public FormFindReplace()
    {
      InitializeComponent();

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
      /*
      else
      {
        comboSearchTarget.SelectedIndex = (int)FindTarget.CURRENT_SELECTION;
        comboReplaceTarget.SelectedIndex = (int)FindTarget.CURRENT_SELECTION;
      }*/
    }



    private void btnFindNext_Click( object sender, EventArgs e )
    {
      FindNext( null, comboSearchText.Text );
    }



    public void FindNext( BaseDocument DirectlyFromSourceFile )
    {
      FindNext( DirectlyFromSourceFile, comboSearchText.Text );
    }



    public void FindNext( BaseDocument DirectlyFromSourceFile, string SearchText )
    {
      /*
      if ( DirectlyFromSourceFile )
      {
        LastSearchFound.StartPosition = EditC64Filename.
      }
      else
      {
        LastSearchCursorPos = -1;
        LastReplaceCursorPos = -1;
      }*/

      if ( FindNextNew( SearchText,
                        radioSearchDirDown.Checked,
                        checkSearchRegExp.Checked,
                        checkSearchFullWords.Checked,
                        checkSearchIgnoreCase.Checked,
                        checkSearchWrap.Checked,
                        (FindTarget)comboSearchTarget.SelectedIndex,
                        DirectlyFromSourceFile,
                        LastSearchFound ) )
      {
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

        var foundRange = RangeFromSearchLocation( edit, LastSearchFound );

        if ( (FindTarget)comboSearchTarget.SelectedIndex != FindTarget.CURRENT_SELECTION )
        {
          // do not modify selection!
          if ( foundRange != null )
          {
            edit.Navigate( foundRange.Start.iLine );
            edit.Selection = foundRange;
          }
          //edit.Selection..ShowLines();
          //edit.Caret.Position = edit.Selection.Range.End;
          //edit.Selection.Range.Select();
        }
        else
        {
          //foundRange.ShowLines();
          //edit.Caret.Position = foundRange.End;
        }

        if ( keepFindActive )
        {
          Core.MainForm.m_FindReplace.Show( Core.MainForm.panelMain );
        }
        return;
      }
      else
      {
        Core.SetStatus( "Searched text not found:" + comboSearchText.Text );
        if ( Core.Settings.PlaySoundOnSearchFoundNoItem )
        {
          System.Media.SystemSounds.Asterisk.Play();
        }
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
        if ( comboSearchText.Items.Count >= 50 )
        {
          comboSearchText.Items.RemoveAt( comboSearchText.Items.Count - 1 );
        }
        comboSearchText.Items.Insert( 0, SearchString );
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



    private SearchLocation FindNextOccurrence( string SearchSource, string SearchString, bool RegularExpression, bool WholeWords, bool IgnoreCase, bool Upwards, int LastPosition )
    {
      if ( string.IsNullOrEmpty( SearchString ) )
      {
        return new SearchLocation();
      }

      int     startPos = LastPosition + 1;
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
        System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex( SearchString, regexOptions );

        System.Text.RegularExpressions.Match match = null;
        if ( Upwards )
        {
          match = regEx.Match( SearchSource, 0, startPos );
        }
        else
        {
          match = regEx.Match( SearchSource, startPos );
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
          pos = SearchSource.LastIndexOf( SearchString, 0, startPos );
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
      if ( Document == null )
      {
        return null;
      }

      if ( Document.BaseDoc == null )
      {
        return null;
      }
      if ( Document.Type == ProjectElement.ElementType.ASM_SOURCE )
      {
        return ( (SourceASMEx)Document.BaseDoc ).editSource;
      }
      else if ( Document.Type == ProjectElement.ElementType.BASIC_SOURCE )
      {
        return ( (SourceBasicEx)Document.BaseDoc ).editSource;
      }
      else if ( Document.Type == ProjectElement.ElementType.DISASSEMBLER )
      {
        return ( (Disassembler)Document.BaseDoc ).editDisassembly;
      }
      return null;
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

      /*
      var start = Edit.VirtualPositionToPosition( Location.StartPosition );
      var end = Edit.VirtualPositionToPosition( Location.StartPosition + Location.Length );
       */
      //var start = Location.StartPosition;
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
      BaseDocument activeDocument = Core.MainForm.ActiveDocument;
      if ( DirectlyFromSourceFile != null )
      {
        activeDocument = DirectlyFromSourceFile;
      }
      // add search text to "history"
      HistoriseSearchString( SearchString );

      int lastPosition = LastFound.StartPosition;

      FastColoredTextBoxNS.FastColoredTextBox  edit = null;

      if ( Target == FindTarget.ACTIVE_DOCUMENT )
      {
        if ( activeDocument == null )
        {
          LastFound.Clear();
          return false;
        }
        edit = EditFromDocumentEx( activeDocument.DocumentInfo );
        if ( edit == null )
        {
          LastFound.Clear();
          return false;
        }
        if ( DirectlyFromSourceFile != null )
        {
          lastPosition = CharacterPosFromPosition( edit, edit.Selection.Start );
        }
        else if ( lastPosition != -1 )
        {
          // virtualize pos
          lastPosition = edit.PositionToVirtualPosition( lastPosition );
        }

        SearchLocation newLocation = FindNextOccurrence( edit.Text, SearchString, RegularExpression, WholeWords, IgnoreCase, !SearchDown, lastPosition );
        if ( newLocation.StartPosition == -1 )
        {
          if ( Wrap )
          {
            if ( SearchDown )
            {
              newLocation = FindNextOccurrence( edit.Text, SearchString, RegularExpression, WholeWords, IgnoreCase, !SearchDown, 0 );
            }
            else
            {
              newLocation = FindNextOccurrence( edit.Text, SearchString, RegularExpression, WholeWords, IgnoreCase, !SearchDown, edit.Text.Length );
            }
          }
          if ( newLocation.StartPosition == -1 )
          {
            LastFound.Clear();
            return false;
          }
        }

        // find line from pos
        FindLineAndTextFromResult( activeDocument, newLocation, LastFound, edit.Text );
        newLocation.LineNumber = LastFound.LineNumber;

        var start = edit.VirtualPositionToPosition( newLocation.StartPosition );
        var end = edit.VirtualPositionToPosition( newLocation.StartPosition + newLocation.Length );

        newLocation.StartPosition = start;
        newLocation.Length = end - start;

        LastFound.FoundInDocument = activeDocument.DocumentInfo;
        LastFound.StartPosition   = newLocation.StartPosition;
        LastFound.Length          = newLocation.Length;
        LastFound.LineNumber      = newLocation.LineNumber;

        return true;
      }
      else if ( Target == FindTarget.CURRENT_SELECTION )
      {
        if ( activeDocument == null )
        {
          LastFound.Clear();
          return false;
        }
        edit = EditFromDocumentEx( activeDocument.DocumentInfo );
        if ( ( edit == null )
        ||   ( edit.SelectionLength == 0 ) )
        {
          LastFound.Clear();
          return false;
        }

        if ( edit.SelectionLength == SearchString.Length )
        {
          // re-select previous search selection
          edit.Selection = new FastColoredTextBoxNS.Range( edit, 
                                                           edit.PositionToPlace( PreviousSearchSelection.StartPosition ), 
                                                           edit.PositionToPlace( PreviousSearchSelection.StartPosition + PreviousSearchSelection.Length ) );
          //edit.Selection.Range.ShowLines();
          //edit.Caret.Position = edit.Selection.Range.End;
          //edit.Selection.Range.Select();
        }

        int       searchStart = 0;
        int       offset = edit.PlaceToPosition( edit.Selection.Start );
        if ( ( lastPosition >= edit.PlaceToPosition( edit.Selection.Start ) )
        &&   ( lastPosition < edit.PlaceToPosition( edit.Selection.End ) ) )
        {
          searchStart = lastPosition - edit.PlaceToPosition( edit.Selection.Start );
        }
        string    selectionText = edit.Selection.Text;
        SearchLocation newLocation = FindNextOccurrence( selectionText, SearchString, RegularExpression, WholeWords, IgnoreCase, !SearchDown, searchStart - 1 );
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
          PreviousSearchSelection.StartPosition   = edit.PlaceToPosition( edit.Selection.Start );
          PreviousSearchSelection.Length          = edit.SelectionLength;
          PreviousSearchSelection.FoundInDocument = activeDocument.DocumentInfo;
        }

        newLocation.StartPosition += offset;

        // find line from pos
        FindLineAndTextFromResult( activeDocument, newLocation, LastFound, edit.Text );

        LastFound.FoundInDocument = activeDocument.DocumentInfo;
        LastFound.StartPosition = newLocation.StartPosition;
        LastFound.Length = newLocation.Length;
        return true;
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

        retry_search:
        ;
        edit = EditFromDocumentEx( docToSearch.DocumentInfo );
        SearchLocation newLocation = FindNextOccurrence( edit.Text, SearchString, RegularExpression, WholeWords, IgnoreCase, !SearchDown, lastPosition );
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
        // find line from pos
        FindLineAndTextFromResult( docToSearch, newLocation, LastFound, edit.Text );

        LastFound.FoundInDocument = docToSearch.DocumentInfo;
        LastFound.StartPosition = newLocation.StartPosition;
        LastFound.Length = newLocation.Length;

        return true;
      }
      else if ( Target == FindTarget.FULL_SOLUTION )
      {
        ProjectElement  elementToSearch = null;
        ProjectElement  firstElement = null;
        if ( LastFound.FoundInDocument != null )
        {
          elementToSearch = LastFound.FoundInDocument.Element;
        }
        if ( elementToSearch == null )
        {
          elementToSearch = GetFirstProjectElement( SearchDown );
          firstElement = elementToSearch;
        }
        if ( ( elementToSearch == null )
        || ( string.IsNullOrEmpty( elementToSearch.Filename ) ) )
        {
          LastFound.Clear();
          return false;
        }

        retry_search:
        ;
        string textFromElement = Core.Searching.GetDocumentInfoText( elementToSearch.DocumentInfo );
        SearchLocation newLocation = FindNextOccurrence( textFromElement, SearchString, RegularExpression, WholeWords, IgnoreCase, !SearchDown, lastPosition );
        if ( newLocation.StartPosition == -1 )
        {
          elementToSearch = GetNextProjectElement( elementToSearch, SearchDown, Wrap );
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
          goto retry_search;
        }

        if ( elementToSearch.DocumentInfo != null )
        {
          var edit2 = EditFromDocumentEx( elementToSearch.DocumentInfo );
          if ( edit2 != null )
          {
            var start = edit2.VirtualPositionToPosition( newLocation.StartPosition );
            var end = edit2.VirtualPositionToPosition( newLocation.StartPosition + newLocation.Length );

            newLocation.StartPosition = start;
            newLocation.Length = end - start;
          }
        }


        // find line from pos
        FindLineAndTextFromResult( elementToSearch.Document, newLocation, LastFound, textFromElement );

        LastFound.FoundInDocument = elementToSearch.DocumentInfo;
        LastFound.StartPosition = newLocation.StartPosition;
        LastFound.Length = newLocation.Length;
        return true;
      }

      // not handled yet
      LastFound.Clear();
      return false;
    }



    private void FindLineAndTextFromResult( BaseDocument DocToSearch, SearchLocation NewLocation, SearchLocation LastFound, string TextToSearch )
    {
      /*
      if ( DocToSearch != null )
      {
        var edit = EditFromDocumentEx( DocToSearch.DocumentInfo );
        int pos = PositionFromCharacterPos( edit, NewLocation.StartPosition );
        if ( edit != null )
        {
          pos = edit.VirtualPositionToPosition( pos );
        }

        LastFound.LineNumber = edit.PositionToPlace( pos ).iLine;
        LastFound.FoundLine = edit[LastFound.LineNumber].Text;
        LastFound.LineNumber++;

        NewLocation.LineNumber = LastFound.LineNumber;
        NewLocation.FoundLine   = LastFound.FoundLine;
      }
      else*/
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
        LastFound.LineNumber = numLines;
        if ( ( curPos != -1 )
        &&   ( lastPos != -1 ) )
        {
          LastFound.FoundLine = TextToSearch.Substring( lastPos + 1, curPos - lastPos - 2 );
        }
        else
        {
          LastFound.FoundLine = TextToSearch.Substring( lastPos + 1 );
        }
      }
    }



    private void btnSearchBookmark_Click( object sender, EventArgs e )
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
        ///searchFound.StartingLine.AddMarker( edit.FindReplace.Marker );
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



    public void Fill( C64Studio.StudioSettings Settings )
    {
      checkSearchFullWords.Checked  = Settings.LastFindWholeWord;
      checkSearchIgnoreCase.Checked = Settings.LastFindIgnoreCase;
      checkSearchRegExp.Checked     = Settings.LastFindRegexp;
      checkSearchWrap.Checked       = Settings.LastFindWrap;

      comboSearchTarget.SelectedIndex = Settings.LastFindTarget;
      comboSearchText.Items.Clear();
      foreach ( var findArg in Settings.FindArguments )
      {
        comboSearchText.Items.Add( findArg );
      }
      comboReplaceSearchText.Items.Clear();
      foreach ( var replaceArg in Settings.ReplaceArguments )
      {
        comboReplaceSearchText.Items.Add( replaceArg );
      }
    }



    public void ToSettings( C64Studio.StudioSettings Settings )
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



    private void btnReplaceFindNext_Click( object sender, EventArgs e )
    {
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

        var foundRange =  RangeFromSearchLocation( edit, LastReplaceFound );
        ///replaceFound.ShowLines();
        ///replaceFound.GotoEnd();
        ///replaceFound.Select();

        edit.Navigate( foundRange.Start.iLine );
        edit.Selection = foundRange;
        return;
      }
    }



    private void btnReplaceNext_Click( object sender, EventArgs e )
    {
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
        ///replaceFound.ShowLines();
        ///replaceFound.GotoEnd();
        ///replaceFound.Select();

        // store old selection
        var oldSelectionStart = edit.Selection.Start;
        var oldSelectionEnd = edit.Selection.End;
        int oldEnd = edit.PlaceToPosition( oldSelectionEnd );

        edit.Selection = replaceFound;
        edit.SelectedText = comboReplaceWith.Text;

        // set location to after replaced text to avoid recursion
        LastReplaceFound.StartPosition += comboReplaceWith.Text.Length;

        // restore old selection
        int newEnd = oldEnd + comboReplaceWith.Text.Length - comboReplaceSearchText.Text.Length;
        var newPlace = edit.PositionToPlace( newEnd );

        edit.Selection.Start = oldSelectionStart;
        edit.Selection.End = newPlace;
        return;
      }
    }



    private void btnReplaceAll_Click( object sender, EventArgs e )
    {
      int occurrences = 0;
      Core.Searching.ClearSearchResults();

      FindTarget    replaceTarget = (FindTarget)comboReplaceTarget.SelectedIndex;

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
                                                        comboReplaceWith.Text,
                                                        checkReplaceRegexp.Checked,
                                                        checkReplaceWholeWords.Checked,
                                                        checkReplaceIgnoreCase.Checked,
                                                        out occurrences );
          edit.SelectedText = replacedText;

          // restore old selection
          int newEnd = oldEnd + ( comboReplaceWith.Text.Length - comboReplaceSearchText.Text.Length ) * occurrences;
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
        case FindTarget.FULL_SOLUTION:
          if ( Core.MainForm.m_Solution != null )
          {
            foreach ( Project proj in Core.MainForm.m_Solution.Projects )
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
                                                      comboReplaceWith.Text,
                                                      checkReplaceRegexp.Checked,
                                                      checkReplaceWholeWords.Checked,
                                                      checkReplaceIgnoreCase.Checked,
                                                      out docOccurrences );

        if ( docOccurrences > 0 )
        {
          SetDocumentText( doc.Key, replacedText );
        }

        occurrences += docOccurrences;
      }
      Core.AddToOutput( "Replaced " + occurrences + " occurrences." + System.Environment.NewLine );


      /*
      // special case, we replace in the string and replace it
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

      string replaceSource = edit.Selection.Text;
      string replacement = replaceSource;
      bool    hadSelection = true;

      if ( edit.SelectionLength == 0 )
      {
        replacement = edit.Text;
        hadSelection = false;
      }

      LastReplaceFound.Clear();
      Core.Searching.ClearSearchResults();
      while ( true )
      {
        SearchLocation newLocation = FindNextOccurrence( replacement,
                                          comboReplaceSearchText.Text,
                                          checkReplaceRegexp.Checked,
                                          checkReplaceWholeWords.Checked,
                                          checkReplaceIgnoreCase.Checked,
                                          false,
                                          LastReplaceFound.StartPosition );
        if ( newLocation.StartPosition == -1 )
        {
          if ( hadSelection )
          {
            edit.SelectedText = replacement;
          }
          else
          {
            int     origFirstLine = edit.VisibleRange.Start.iLine;

            edit.Text = replacement;

            int firstLine = edit.VisibleRange.Start.iLine;
            //edit.Scrolling.ScrollBy( origFirstLine - firstLine, 0 );
          }
          Core.MainForm.m_SearchResults.SearchComplete();
          return;
        }

        // find line from pos
        FindLineAndTextFromResult( docToReplaceIn, newLocation, LastReplaceFound, replacement );

        LastReplaceFound.FoundInDocument  = docToReplaceIn.DocumentInfo;
        LastReplaceFound.StartPosition    = newLocation.StartPosition;
        LastReplaceFound.Length           = newLocation.Length;
        LastReplaceFound.LineNumber       = newLocation.LineNumber;

        replacement = replacement.Substring( 0, newLocation.StartPosition ) + comboReplaceWith.Text + replacement.Substring( newLocation.StartPosition + newLocation.Length );

        // offset start pos to new string
        LastReplaceFound.StartPosition += comboReplaceWith.Text.Length;

        ++occurrences;
        Core.Searching.AddSearchResult( LastReplaceFound );
      }*/
    }



    private void SetDocumentText( C64Studio.DocumentInfo DocumentInfo, string NewText )
    {
      DocumentInfo.HasBeenSuccessfullyBuilt = false;
      if ( DocumentInfo.BaseDoc != null )
      {
        // easy - replace in edit
        DocumentInfo.BaseDoc.FillContent( NewText );
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
          return TextToSearchIn;
        }

        FindLineAndTextFromResult( DocInfo.BaseDoc, newLocation, LastReplaceFound, TextToSearchIn );
        Core.Searching.AddSearchResult( new SearchLocation() 
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
        LastReplaceFound.StartPosition += StringToReplaceWith.Length;

        ++NumOccurences;
      }

    }



    private void btnFindAll_Click( object sender, EventArgs e )
    {
      int                     occurrences = 0;
      SearchLocation          firstFoundRange = null;
      List<SearchLocation>    results = new List<SearchLocation>();

      LastSearchFound.Clear();

      Core.Searching.ClearSearchResults();

      Core.AddToOutput( "Searching for " + comboSearchText.Text + System.Environment.NewLine );
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
        //Debug.Log( "Found in " + LastSearchFound.FoundInDocument.DocumentFilename + " at " + LastSearchFound.StartPosition );

        ++occurrences;

        results.Add( new SearchLocation( LastSearchFound ) );
      }
    }



    private void comboReplaceTarget_SelectedIndexChanged( object sender, EventArgs e )
    {
      checkReplaceWrap.Enabled = ( comboReplaceTarget.SelectedIndex == 3 );
      if ( !checkReplaceWrap.Enabled )
      {
        checkReplaceWrap.Checked = false;
      }
    }



  }
}
