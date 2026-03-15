using FastColoredTextBoxNS;
using GR.Collections;
using GR.Image;
using GR.IO;
using GR.Memory;
using RetroDevStudio.CustomRenderer;
using RetroDevStudio.Dialogs;
using RetroDevStudio.Parser;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;



namespace RetroDevStudio.Documents
{
  public partial class TextFile : BaseDocument
  {
    private const int BORDER_MARKER_WIDTH   = 20;
    private const int BORDER_SIZE_WIDTH     = 24;
    private const int BORDER_CYCLES_WIDTH   = 48;
    private const int BORDER_ADDRESS_WIDTH  = 52;


    int                                       m_CurrentMarkedLineIndex = -1;
    int                                       m_ContextMenuLineIndex = -1;
    int                                       m_ContextMenuPosition = -1;

    // inserting text flag
    bool                                      m_InsertingText = false;

    DateTime                                  m_LastChange = DateTime.Now;
    Place                                     m_LastChangePosStart = new Place();
    Place                                     m_LastChangePosEnd = new Place();

    Timer                                     m_DelayedEventTimer = new Timer();

    private string                            m_CurrentHighlightText = null;
    private List<TextLocation>                m_CurrentHighlightLocations = new List<TextLocation>();

    private FastColoredTextBoxNS.Style[]  m_TextStyles = new FastColoredTextBoxNS.Style[(int)Types.ColorableElement.LAST_ENTRY];

    private delegate void delSimpleEventHandler();



    public override void RemoveBookmark( int LineIndex )
    {
      editSource.Bookmarks.Remove( LineIndex );
    }



    public override int CursorLine
    {
      get
      {
        return editSource.Selection.Start.iLine;
      }
    }



    public override int CursorPosInLine
    {
      get
      {
        return editSource.Selection.Start.iChar;
      }
    }



    public override FastColoredTextBoxNS.FastColoredTextBox SourceControl
    {
      get
      {
        return editSource;
      }
    }



    public TextFile( StudioCore Core )
    {
      this.Core = Core;

      DocumentInfo.Type = ProjectElement.ElementType.TEXT_FILE;
      DocumentInfo.UndoManager.MainForm = Core.MainForm;

      m_IsSaveable = true;

      InitializeComponent();

      copyToolStripMenuItem.Tag                         = Function.COPY;
      pasteToolStripMenuItem.Tag                        = Function.PASTE;
      cutToolStripMenuItem.Tag                          = Function.CUT;
      helpToolStripMenuItem.Tag                         = Function.HELP;
      addBookmarkHereToolStripMenuItem.Tag              = Function.BOOKMARK_ADD;
      removeBookmarkToolStripMenuItem.Tag               = Function.BOOKMARK_DELETE;
      removeAllBookmarksOfThisFileToolStripMenuItem.Tag = Function.BOOKMARK_DELETE_ALL;

      SuspendLayout();

      DPIHandler.ResizeControlsForDPI( this );

      m_DelayedEventTimer.Interval = 500;
      m_DelayedEventTimer.Tick += m_DelayedEventTimer_Tick;

      editSource.AutoIndentExistingLines = false;
      editSource.AutoIndentChars = false;
      editSource.WordCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_@$&äöüÄÖÜß";

      // allows auto indent on new lines, indents similar to previous line
      editSource.AutoIndent = true;
      editSource.ShowLineNumbers = !Core.Settings.ASMHideLineNumbers;

      contextSource.Opening += new CancelEventHandler( contextSource_Opening );

      editSource.Font = new System.Drawing.Font( Core.Settings.SourceFontFamily, Core.Settings.SourceFontSize, Core.Settings.SourceFontStyle );
      editSource.KeyDown += new System.Windows.Forms.KeyEventHandler( editSource_KeyDown );
      editSource.KeyPress += new System.Windows.Forms.KeyPressEventHandler( editSource_KeyPress );
      editSource.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler( editSource_PreviewKeyDown );

      editSource.SelectionChanged += new EventHandler( editSource_SelectionChanged );

      editSource.ZoomChanged += EditSource_ZoomChanged;

      this.Activated += new EventHandler( SourceASM_Activated );

      editSource.TextChanged += new EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>( editSource_TextChanged );
      editSource.TextChangedDelayed += editSource_TextChangedDelayed;
      editSource.TextInserted += EditSource_TextInserted;
      editSource.TextDeleted += EditSource_TextDeleted;
      editSource.FoldingBlockStateChanged += editSource_FoldingBlockStateChanged;

      editSource.BookmarkAdded += EditSource_BookmarkAdded;
      editSource.BookmarkRemoved += EditSource_BookmarkRemoved;

      editSource.UndoRedoStateChanged += new EventHandler<UndoRedoEventArgs>( editSource_UndoRedoStateChanged );

      editSource.LeftBracket = '(';
      editSource.RightBracket = ')';
      editSource.LeftBracket2 = '\x0';
      editSource.RightBracket2 = '\x0';
      editSource.SelectionChangedDelayed += editSource_SelectionChangedDelayed;
      editSource.PreferredLineWidth = Core.Settings.ASMShowMaxLineLengthIndicatorLength;
      editSource.ToolTipDisplayDuration = 30000;
      editSource.SyntaxHighlighter = null;

      editSource.AllowDrop = true;

      contextSource.Opened += new EventHandler( contextSource_Opened );

      ResumeLayout();
    }



    private void AutoComplete_Selecting( object sender, SelectingEventArgs e )
    {
      // called when intellisense option is to be inserted
      if ( e.Item.Text.StartsWith( "+" ) )
      {
        // cut off prefixed +
        if ( e.FragmentRange.CharBeforeStart == '+' )
        {
          e.InsertionText = e.Item.Text.Substring( 1 );
        }
      }
    }



    private void EditSource_BookmarkRemoved( object sender, BookmarkEventArgs e )
    {
      if ( m_InsertingText )
      {
        return;
      }
      RaiseDocEvent( new DocEvent( DocEvent.Type.BOOKMARK_REMOVED, e.Index ) );
      StoreBookmarks();
    }



    private void EditSource_BookmarkAdded( object sender, BookmarkEventArgs e )
    {
      if ( m_InsertingText )
      {
        return;
      }
      RaiseDocEvent( new DocEvent( DocEvent.Type.BOOKMARK_ADDED, e.Index ) );
      StoreBookmarks();
    }



    private void EditSource_TextInserted( object sender, TextInsertedEventArgs e )
    {
    }



    private void EditSource_TextDeleted( object sender, TextDeletedEventArgs e )
    {
    }



    private void EditSource_ZoomChanged( object sender, EventArgs e )
    {
      AdjustFontSizeInLeftBorder();
    }



    private void AdjustFontSizeInLeftBorder()
    {
      float     zoomFactor = editSource.Zoom / 100.0f;

      int approxWidthOfChar = editSource.CharWidth;
      int approxWidthOfAddress = 5 * approxWidthOfChar;
      int numChars = 0;

      int     newPadding = (int)( BORDER_MARKER_WIDTH * zoomFactor );    // space for marker symbol on left side
      editSource.LeftPaddingInCharacters = numChars + 3;
    }



    void editSource_SelectionChangedDelayed( object sender, EventArgs e )
    {
      string    newHighlightText = null;
      if ( editSource.Selection.End.iLine != editSource.Selection.Start.iLine )
      {
        // no extra highlight 
        newHighlightText = null;
      }
      else
      {
        newHighlightText = editSource.SelectedText;
      }
      if ( string.IsNullOrEmpty( newHighlightText ) )
      {
        newHighlightText = null;
      }
      m_CurrentHighlightLocations.Clear();
      if ( m_CurrentHighlightText != newHighlightText )
      {
        if ( !string.IsNullOrEmpty( m_CurrentHighlightText ) )
        {
          editSource.Range.ClearStyle( m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.HIGHLIGHTED_SEARCH_RESULTS )] );
        }

        m_CurrentHighlightText = newHighlightText;
        if ( m_CurrentHighlightLocations.Any() )
        {
          foreach ( var entry in m_CurrentHighlightLocations )
          {
            editSource.GetRange( new Place( entry.StartIndex, entry.LineIndex ), new Place( entry.StartIndex + entry.Length, entry.LineIndex ) )
                      .SetStyle( m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.HIGHLIGHTED_SEARCH_RESULTS )] );
          }
        }
        else if ( !string.IsNullOrEmpty( m_CurrentHighlightText ) )
        {
          string    regex = m_CurrentHighlightText.Replace( @"\", @"\\" );
          regex = regex.Replace( @"^", @"\^" );
          regex = regex.Replace( @"$", @"\$" );
          regex = regex.Replace( @".", @"\." );
          regex = regex.Replace( @"|", @"\|" );
          regex = regex.Replace( @"?", @"\?" );
          regex = regex.Replace( @"*", @"\*" );
          regex = regex.Replace( @"+", @"\+" );
          regex = regex.Replace( @"-", @"\-" );
          regex = regex.Replace( @"(", @"\(" );
          regex = regex.Replace( @")", @"\)" );
          regex = regex.Replace( @"{", @"\{" );
          regex = regex.Replace( @"[", @"\[" );

          editSource.Range.SetStyle( m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.HIGHLIGHTED_SEARCH_RESULTS )], regex );
        }
      }
    }



    public override void HighlightText( int LineIndex, int CharPos, int Length )
    {
      m_CurrentHighlightLocations.Clear();
      editSource.Selection = new FastColoredTextBoxNS.Range( editSource, CharPos, LineIndex, CharPos + Length, LineIndex );
    }



    public override void HighlightOccurrences( int LineIndex, int CharPos, int Length, List<TextLocation> Locations )
    {
      m_CurrentHighlightLocations = Locations;
      editSource.Selection = new FastColoredTextBoxNS.Range( editSource, CharPos, LineIndex, CharPos + Length, LineIndex );
    }



    void m_DelayedEventTimer_Tick( object sender, EventArgs e )
    {
      m_DelayedEventTimer.Stop();
    }



    void editSource_FoldingBlockStateChanged( object sender, EventArgs e )
    {
      StoreFoldedBlocks();
    }



    void editSource_TextChangedDelayed( object sender, FastColoredTextBoxNS.TextChangedEventArgs e )
    {
      UpdateBookmarkComments( e.ChangedRange );
    }



    private void UpdateBookmarkComments( FastColoredTextBoxNS.Range changedRange )
    {
      int   firstLine = changedRange.Start.iLine;
      int   lastLine = changedRange.End.iLine;

      if ( lastLine < firstLine )
      {
        int   temp = lastLine;
        lastLine = firstLine;
        firstLine = temp;
      }

      RaiseDocEvent( new DocEvent( DocEvent.Type.BOOKMARKS_UPDATED ) );
    }

    
    
    void ResetAllStyles( FastColoredTextBoxNS.Range Range )
    {
      //clear previous highlighting but error highlighting
      var indexMask = editSource.GetStyleIndexMask( m_TextStyles );

      // mask out error style (11 matched SyntaxPrio)
      indexMask = (FastColoredTextBoxNS.StyleIndex)( ( (int)indexMask ) & ~( (int)FastColoredTextBoxNS.StyleIndex.Style12 | (int)FastColoredTextBoxNS.StyleIndex.Style13 ) );
      Range.ClearStyle( indexMask );
    }



    internal void UpdateFoldingBlocks()
    {
      if ( m_InsertingText )
      {
        return;
      }
      // have to use full range, otherwise folding blocks get messed up
      FastColoredTextBoxNS.Range range = editSource.Range;

      range.ClearFoldingMarkers();
      //set folding markers
      range.SetFoldingMarkers( "{", "}" );
      range.SetFoldingMarkers( "![l]{0,1}zone", "![l]{0,1}zone" );
      //range.SetFoldingMarkers( "!lzone", "![l]{0,1}zone" );
      range.SetFoldingMarkers( "!macro", "!end" );
      range.SetFoldingMarkers( "!for", "!end" );
      range.SetFoldingMarkers( "!region", "!endregion" );
    }



    void editSource_UndoRedoStateChanged( object sender, EventArgs e )
    {
      Core.MainForm.UpdateUndoSettings();
    }



    // lower value means higher prio?
    int SyntaxElementStylePrio( Types.ColorableElement Element )
    {
      int     value = 14;

      switch ( Element )
      {
        case Types.ColorableElement.EMPTY_SPACE:
          value = 0;
          break;
        case Types.ColorableElement.CURRENT_DEBUG_LINE:
          value = 1;
          break;
        case Types.ColorableElement.HIGHLIGHTED_SEARCH_RESULTS:
          value = 2;
          break;
        case Types.ColorableElement.COMMENT:
          value = 3;
          break;
        case Types.ColorableElement.LITERAL_STRING:
          value = 4;
          break;
        case Types.ColorableElement.OPERATOR:
          value = 5;
          break;
        case Types.ColorableElement.PSEUDO_OP:
          value = 6;
          break;
        case Types.ColorableElement.LITERAL_NUMBER:
          value = 7;
          break;
        case Types.ColorableElement.CODE:
          value = 8;
          break;
        case Types.ColorableElement.MACRO_CALL:
          value = 9;
          break;
        case Types.ColorableElement.LABEL:
          value = 10;
          break;
        case ColorableElement.IMMEDIATE_LABEL:
          value = 11;
          break;
        case Types.ColorableElement.ERROR_UNDERLINE:
          value = 12;
          break;
        case Types.ColorableElement.WARNING_UNDERLINE:
          value = 13;
          break;
        case Types.ColorableElement.NONE:
          value = 14;
          break;

      }
      return value;
    }



    private int DetermineSelectionStartCharPos()
    {
      if ( editSource.Selection.Start.iLine < editSource.Selection.End.iLine )
      {
        return editSource.Selection.Start.iChar;
      }
      else if ( editSource.Selection.Start.iLine == editSource.Selection.End.iLine )
      {
        return Math.Min( editSource.Selection.Start.iChar, editSource.Selection.End.iChar );
      }
      return editSource.Selection.End.iChar;
    }



    void editSource_TextChanged( object sender, FastColoredTextBoxNS.TextChangedEventArgs e )
    {
      m_LastChange = DateTime.Now;
      m_LastChangePosStart = editSource.Selection.Start; // e.ChangedRange.Start;
      m_LastChangePosEnd = editSource.Selection.End; // e.ChangedRange.End;

      // only reset style in active line to keep speed up
      if ( e.ChangedRange.Start.iLine == e.ChangedRange.End.iLine )
      {
        ResetAllStyles( e.ChangedRange );
      }
      else
      {
        // reset only affected line
        ResetAllStyles( editSource.GetLine( e.ChangedRange.Start.iLine ) );
      }

      //ResetAllStyles( e.ChangedRange );
      editSource.OnSyntaxHighlight( new FastColoredTextBoxNS.TextChangedEventArgs( e.ChangedRange ) );

      if ( UndoPossible )
      {
        SetModified();
      }
    }



    void SourceASM_Activated( object sender, EventArgs e )
    {
    }



    void ApplySyntaxColoring( Types.ColorableElement Element )
    {
      if ( ( Element == Types.ColorableElement.ERROR_UNDERLINE )
      ||   ( Element == Types.ColorableElement.WARNING_UNDERLINE ) )
      {
        m_TextStyles[SyntaxElementStylePrio( Element )] = new FastColoredTextBoxNS.WavyLineStyle( 255, GR.Color.Helper.FromARGB( Core.Settings.FGColor( Element ) ) );

        editSource.Styles[SyntaxElementStylePrio( Element )] = m_TextStyles[SyntaxElementStylePrio( Element )];
        return;
      }

      System.Drawing.Brush      foreBrush = new System.Drawing.SolidBrush( GR.Color.Helper.FromARGB( Core.Settings.FGColor( Element ) ) );
      System.Drawing.Brush      backBrush = null;
      System.Drawing.FontStyle  fontStyle = editSource.Font.Style;

      backBrush = new System.Drawing.SolidBrush( GR.Color.Helper.FromARGB( Core.Settings.BGColor( Element ) ) );
      m_TextStyles[SyntaxElementStylePrio( Element )] = new FastColoredTextBoxNS.TextStyle( foreBrush, backBrush, fontStyle );

      editSource.Styles[SyntaxElementStylePrio( Element )] = m_TextStyles[SyntaxElementStylePrio( Element )];

      // empty space
      editSource.BackColor = GR.Color.Helper.FromARGB( Core.Settings.BGColor( Types.ColorableElement.EMPTY_SPACE ) );
      editSource.IndentBackColor = Core.Theming.DarkenColor( GR.Color.Helper.FromARGB( Core.Settings.BGColor( Types.ColorableElement.BACKGROUND_CONTROL ) ) );
      editSource.SelectionColor = GR.Color.Helper.FromARGB( Core.Settings.FGColor( Types.ColorableElement.SELECTED_TEXT ) );
      editSource.LineNumberColor = GR.Color.Helper.FromARGB( Core.Settings.FGColor( Types.ColorableElement.LINE_NUMBERS ) );
    }



    void InvalidateMarkerAreaAtLine( int LineIndex )
    {
      if ( ( LineIndex >= 0 )
      &&   ( LineIndex < editSource.LinesCount ) )
      {
        int y = editSource.PlaceToPoint( editSource.GetLine( LineIndex ).Start ).Y;
        System.Drawing.Rectangle    markerRect = new System.Drawing.Rectangle( 0, y, editSource.LeftIndent, editSource.LineInfos[LineIndex].WordWrapStringsCount * editSource.CharHeight );
        editSource.Invalidate( markerRect );
      }
    }



    void editSource_PreviewKeyDown( object sender, System.Windows.Forms.PreviewKeyDownEventArgs e )
    {
    }



    void UpdateStatusInfo()
    {
      if ( Core.MainForm.ActiveDocumentInfo != this.DocumentInfo )
      {
        return;
      }

      string    newInfo = "Row " + ( editSource.Selection.Start.iLine + 1 ).ToString() + ", Col " + editSource.Selection.Start.iChar.ToString();

      int[]     potentialCycles = new int[3];

      int     startLine = editSource.Selection.Start.iLine;
      int     endLine = startLine;

      if ( !editSource.Selection.IsEmpty )
      {
        startLine = editSource.Selection.Start.iLine;
        endLine = editSource.Selection.End.iLine;
        if ( startLine > endLine )
        {
          startLine = endLine;
          endLine = editSource.Selection.Start.iLine;
        }
      }

      int   numLines = 0;
      if ( editSource.Selection.End.iLine != editSource.Selection.Start.iLine )
      {
        var selRange = new FastColoredTextBoxNS.Range( editSource, editSource.Selection.Start, editSource.Selection.End );
        selRange.Normalize();

        numLines = selRange.End.iLine - selRange.Start.iLine + 1;
        if ( ( selRange.Start.iLine >= 0 )
        &&   ( selRange.End.iLine < editSource.LinesCount ) )
        {
          string    selText = selRange.Text;
          if ( selText.EndsWith( System.Environment.NewLine ) )
          {
            --numLines;
            --endLine;
          }
        }
      }

      if ( numLines >= 1 )
      {
        newInfo += ", " + editSource.SelectionLength.ToString() + " characters, " + numLines.ToString() + " lines selected";
      }
      else
      {
        newInfo += ", " + editSource.SelectionLength.ToString() + " characters selected";
      }
      Core.MainForm.statusEditorDetails.Text = newInfo;
    }



    void editSource_KeyPress( object sender, System.Windows.Forms.KeyPressEventArgs e )
    {
      Core.MainForm.m_FindReplace.ClearLastState();
      UpdateStatusInfo();
    }



    void editSource_SelectionChanged( object sender, EventArgs e )
    {
      UpdateStatusInfo();

      System.Drawing.Point mousePos = editSource.PointToClient( Control.MousePosition );

      int position            = editSource.PointToPosition( mousePos );
      m_ContextMenuLineIndex  = editSource.PositionToPlace( position ).iLine;
      m_ContextMenuPosition   = position;
    }



    void editSource_KeyDown( object sender, System.Windows.Forms.KeyEventArgs e )
    {
    }



    public override void OnApplicationEvent( ApplicationEvent Event )
    {
      base.OnApplicationEvent( Event );

      switch ( Event.EventType )
      {
        case ApplicationEvent.Type.KEY_BINDINGS_MODIFIED:
          break;
        case ApplicationEvent.Type.ACTIVE_DOCUMENT_CHANGED:
          UpdateStatusInfo();
          break;
        case ApplicationEvent.Type.SETTING_MODIFIED:
          break;
        case ApplicationEvent.Type.DOCUMENT_INFO_CREATED:
          break;
      }
    }



    void contextSource_Opening( object sender, CancelEventArgs e )
    {
      System.Drawing.Point mousePos = editSource.PointToClient( Control.MousePosition );

      int position            = editSource.PointToPosition( mousePos );
      m_ContextMenuLineIndex  = editSource.PositionToPlace( position ).iLine;
      m_ContextMenuPosition   = position;

      foreach ( var item in contextSource.Items )
      {
        if ( item is ToolStripMenuItem )
        {
          var menu = (ToolStripMenuItem)item;
          if ( menu.Tag is Function )
          {
            try
            {
              menu.ShortcutKeys = Core.Settings.DetermineAcceleratorKeyForFunction( (Function)menu.Tag, Core.State );
            }
            catch ( Exception )
            {
            }
          }
          foreach ( ToolStripItem subItem in menu.DropDownItems )
          {
            if ( subItem is ToolStripMenuItem )
            {
              var subMenu = (ToolStripMenuItem)subItem;
              if ( subMenu.Tag is Function )
              {
                try
                {
                  subMenu.ShortcutKeys = Core.Settings.DetermineAcceleratorKeyForFunction( (Function)subMenu.Tag, Core.State );
                }
                catch ( Exception )
                {
                }
              }
            }
          }
        }
      }

      // bookmarks
      addBookmarkHereToolStripMenuItem.Enabled = !editSource.Bookmarks.Any( bm => bm.LineIndex == m_ContextMenuLineIndex );
      removeBookmarkToolStripMenuItem.Enabled = editSource.Bookmarks.Any( bm => bm.LineIndex == m_ContextMenuLineIndex );
      removeAllBookmarksOfThisFileToolStripMenuItem.Enabled = editSource.Bookmarks.Any();

      if ( ( Core.MainForm.AppState == Types.StudioState.DEBUGGING_BROKEN )
      ||   ( Core.MainForm.AppState == Types.StudioState.NORMAL ) )
      {
        toolStripSeparator2.Visible = true;
      }
      else
      {
        toolStripSeparator2.Visible = false;
      }
      if ( !Core.Settings.ASMShowMiniView )
      {
        showMiniOverviewToolStripMenuItem.Text = "Show Mini Overview";
      }
      else
      {
        showMiniOverviewToolStripMenuItem.Text = "Hide Mini Overview";
      }
      Core.Theming.ApplyThemeToToolStripItems( contextSource, contextSource.Items );
    }



    void contextSource_Opened( object sender, EventArgs e )
    {
      bool    showOpenFile = false;
      string    lineBelow = editSource.Lines[m_ContextMenuLineIndex].Trim().ToUpper();
      openFileToolStripMenuItem.Visible = showOpenFile;
      separatorCommenting.Visible = showOpenFile;

      copyToolStripMenuItem.Enabled = ( editSource.SelectionLength != 0 );
      cutToolStripMenuItem.Enabled = copyToolStripMenuItem.Enabled;
      pasteToolStripMenuItem.Enabled = System.Windows.Forms.Clipboard.ContainsText();
    }



    private string DetermineFullLibraryFilePath( string SubFilename )
    {
      foreach ( var libFile in Core.Settings.ASMLibraryPaths )
      {
        string    fullBasePath = libFile;
        if ( !GR.Path.IsPathRooted( libFile ) )
        {
#if DEBUG
          fullBasePath = System.IO.Path.GetFullPath( "../../" + libFile );
#else
          fullBasePath = System.IO.Path.GetFullPath( libFile );
#endif
        }
        if ( System.IO.File.Exists( GR.Path.Append( fullBasePath, SubFilename ) ) )
        {
          return GR.Path.Append( fullBasePath, SubFilename );
        }
      }
      return "";
    }



    public override string GetContent()
    {
      return editSource.Text;
    }



    public override bool LoadDocument()
    {
      if ( DocumentInfo.DocumentFilename == null )
      {
        return false;
      }

      try
      {
        m_InsertingText = true;
        editSource.Text = System.IO.File.ReadAllText( DocumentInfo.FullPath, Core.Settings.SourceFileEncoding );
        m_InsertingText = false;

        editSource.ClearUndo();
        editSource.TextSource.ClearIsChanged();

        m_LastChange = DateTime.Now;
      }
      catch ( System.Exception ex )
      {
        Core.Notification.MessageBox( "Could not load file", "Could not load text file " + DocumentInfo.FullPath + ".\r\n" + ex.Message );
        return false;
      }

      UpdateFoldingBlocks();


      m_InsertingText = true;
      var collapsedBlocks = new GR.Collections.Set<int>( DocumentInfo.CollapsedFoldingBlocks );
      foreach ( int blockStart in collapsedBlocks )
      {
        if ( ( blockStart < 0 )
        ||   ( blockStart >= editSource.LinesCount ) )
        {
          // out of bounds
          continue;
        }
        if ( editSource.TextSource[blockStart].FoldingStartMarker != null )
        {
          editSource.CollapseFoldingBlock( blockStart );
        }
      }

      var bookmarks = new GR.Collections.Set<int>( DocumentInfo.Bookmarks );
      foreach ( int lineIndex in bookmarks )
      {
        if ( ( lineIndex < 0 )
        ||   ( lineIndex >= editSource.LinesCount ) )
        {
          // out of bounds
          continue;
        }
        editSource.Bookmarks.Add( lineIndex );
      }
      editSource.OnSyntaxHighlight( new FastColoredTextBoxNS.TextChangedEventArgs( editSource.Range ) );
      m_InsertingText = false;


      SetUnmodified();
      if ( string.IsNullOrEmpty( m_FileWatcher.Path ) )
      {
        SetupWatcher();
        EnableFileWatcher();
      }
      return true;
    }



    protected override bool QueryFilename( string PreviousFilename, out string Filename )
    {
      Filename = "";

      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Save Text File as";
      saveDlg.Filter = Core.MainForm.FilterString( Types.Constants.FILEFILTER_TEXT_FILE + Types.Constants.FILEFILTER_ALL );
      saveDlg.FileName = GR.Path.GetFileName( PreviousFilename );
      if ( DocumentInfo.Project != null )
      {
        saveDlg.InitialDirectory = DocumentInfo.Project.Settings.BasePath;
      }
      if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return false;
      }

      Filename = saveDlg.FileName;
      return true;
    }



    protected override bool PerformSave( string FullPath )
    {
      try
      {
        StoreFoldedBlocks();
        DisableFileWatcher();

        // trim trailing spaces
        if ( editSource.Lines.Count > 0 )
        {
          int caretLine = editSource.Selection.Start.iLine;
          int caretPos = editSource.Selection.Start.iChar;

          if ( Core.Settings.FormatSettings.StripTrailingSpaces )
          {
            editSource.StripTrailingSpaces();
          }
        }
        System.IO.File.WriteAllText( FullPath, GetContent(), Core.Settings.SourceFileEncoding );

        editSource.TextSource.ClearIsChanged();
        editSource.Invalidate();

        Core.TaskManager.AddTask( new Tasks.TaskRefreshSourceControlState( DocumentInfo ) );
      }
      catch ( System.Exception ex )
      {
        Core.Notification.MessageBox( "Could not save file", "Could not save file " + FullPath + ".\r\n" + ex.ToString() );
        EnableFileWatcher();
        return false;
      }
      EnableFileWatcher();
      return true;
    }



    protected override void OnClosing( CancelEventArgs e )
    {
      base.OnClosing( e );

      if ( !e.Cancel )
      {
        // store folded blocks
        StoreFoldedBlocks();
      }
    }



    private void StoreFoldedBlocks()
    {
      if ( m_InsertingText )
      {
        return;
      }
      DocumentInfo.CollapsedFoldingBlocks.Clear();

      foreach ( var block in editSource.FoldedBlocks )
      {
        for ( int iLine = 0; iLine < editSource.LinesCount; iLine++ )
        {
          if ( editSource.TextSource[iLine].UniqueId == block.Key )
          {
            DocumentInfo.CollapsedFoldingBlocks.Add( iLine );
          }
        }
      }
    }



    private void StoreBookmarks()
    {
      if ( m_InsertingText )
      {
        return;
      }
      DocumentInfo.Bookmarks.Clear();
      DocumentInfo.Bookmarks.AddRange( editSource.Bookmarks.Select( bm => bm.LineIndex ) );
    }



    private void editSource_DocumentChanged( object sender, EventArgs e )
    {
      SetModified();
    }



    public override void SetLineMarked( int Line, bool Set )
    {
      if ( m_CurrentMarkedLineIndex != -1 )
      {
        if ( m_CurrentMarkedLineIndex < editSource.LinesCount )
        {
          editSource[m_CurrentMarkedLineIndex].BackgroundBrush = null;
        }
        else
        {
          Debug.Log( "Line Index out of bounds!" );
        }
        m_CurrentMarkedLineIndex = -1;
      }
      if ( Set )
      {
        if ( Line < editSource.LinesCount )
        {
          m_CurrentMarkedLineIndex = Line;
          editSource[m_CurrentMarkedLineIndex].BackgroundBrush = new System.Drawing.SolidBrush( System.Drawing.Color.FromArgb( (int)Core.Settings.BGColor( Types.ColorableElement.CURRENT_DEBUG_LINE ) ) );
        }
        else
        {
          Debug.Log( "Line Index out of bounds!" );
        }
      }
    }



    public void CenterOnCaret()
    {
      // automatically centers
      SourceControl.DoSelectionVisible();
    }



    public override void SetCursorToLine( int Line, int CharIndex, bool SetFocus )
    {
      if ( SetFocus )
      {
        editSource.Focus();
      }

      if ( ( Line < 0 )
      ||   ( Line >= editSource.LinesCount ) )
      {
        Line = 0;
      }
      editSource.Navigate( Line, CharIndex );
      CenterOnCaret();
    }



    public override void SelectText( int Line, int CharIndex, int Length )
    {
      editSource.Selection = new FastColoredTextBoxNS.Range( editSource, CharIndex, Line, CharIndex + Length, Line );
    }



    private void runToCursorToolStripMenuItem_Click( object sender, EventArgs e )
    {
      Core.MainForm.ApplyFunction( RetroDevStudio.Types.Function.DEBUG_RUN_TO );
    }



    public override int CurrentLineIndex
    {
      get
      {
        return editSource.Selection.Start.iLine;
      }
    }



    private int CurrentPosition()
    {
      return editSource.PlaceToPosition( editSource.Selection.Start );
    }



    public override bool UndoPossible
    {
      get
      {
        return editSource.UndoEnabled;
      }
    }



    public override bool RedoPossible
    {
      get
      {
        return editSource.RedoEnabled;
      }
    }



    public override void Undo()
    {
      editSource.Undo();
      SetModified();
    }



    public override void Redo()
    {
      editSource.Redo();
      SetModified();
    }



    public override bool CopyPossible
    {
      get
      {
        return ( !editSource.Selection.IsEmpty )
            && ( editSource.Focused );
      }
    }



    public override bool CutPossible
    {
      get
      {
        return ( !editSource.Selection.IsEmpty )
            && ( editSource.Focused );
      }
    }



    public override bool PastePossible
    {
      get
      {
        return editSource.Focused;
      }
    }



    public override void Copy()
    {
      editSource.Copy();
    }


    public override void Cut()
    {
      editSource.Cut();
      //ShowAutoComplete();
    }



    public override void Paste()
    {
      editSource.Paste();
      UpdateFoldingBlocks();
      StoreFoldedBlocks();
    }



    public override GR.Memory.ByteBuffer DisplayDetails()
    {
      GR.Memory.ByteBuffer      displayData = new GR.Memory.ByteBuffer();

      displayData.AppendI32( editSource.Selection.Start.iLine );
      displayData.AppendI32( editSource.Selection.Start.iChar );

      return displayData;
    }



    public override void ApplyDisplayDetails( GR.Memory.ByteBuffer Buffer )
    {
      GR.IO.MemoryReader binReader = Buffer.MemoryReader();

      int     cursorLine = binReader.ReadInt32();
      int     charIndex = binReader.ReadInt32();

      while ( ( cursorLine > 0 )
      &&      ( cursorLine < editSource.LineInfos.Count )
      &&      ( editSource.LineInfos[cursorLine].VisibleState != FastColoredTextBoxNS.VisibleState.Visible ) )
      {
        --cursorLine;
      }
      /*
      // TODO - this opens a folded block if cursor is inside!!!!
      foreach ( var block in editSource.FoldedBlocks )
      {
        if ( block.Key > cursorLine )
        {
          // this block is below our cursor
          continue;
        }
        // check if the wanted line is inside a block, if so, move upwards

        int     curLine = block;
        if ( 
        int     blockEnd = editSource.TextSource[block].FindE
        editSource.Fold
      }*/
      SetCursorToLine( cursorLine, charIndex, true );
    }



    public override void FillContent( string Text, bool KeepCursorPosIntact, bool KeepBookmarksIntact )
    {
      m_InsertingText = true;

      int scrollOffset = editSource.VerticalScroll.Value;
      editSource.Navigate( editSource.Selection.Start.iLine );

      List<int> currentBookmarks = null;
      if ( KeepBookmarksIntact )
      {
        currentBookmarks = new List<int>();
        currentBookmarks.AddRange( editSource.Bookmarks.Select( x => x.LineIndex ) );
      }
      editSource.BeginAutoUndo();
      editSource.TextSource.Manager.ExecuteCommand( new BookmarkCommand( editSource.TextSource ) );
      editSource.Text = Text;

      if ( KeepCursorPosIntact )
      {
        editSource.VerticalScroll.Value = scrollOffset;
        editSource.UpdateScrollbars();
      }
      if ( KeepBookmarksIntact )
      {
        editSource.Bookmarks.Clear();
        foreach ( var origBookmark in currentBookmarks )
        {
          editSource.Bookmarks.Add( origBookmark );
        }
        // re-keep again for redo
        editSource.TextSource.Manager.ExecuteCommand( new BookmarkCommand( editSource.TextSource ) );
      }
      editSource.EndAutoUndo();

      m_InsertingText = false;
      SetModified();
      editSource.OnSyntaxHighlight( new FastColoredTextBoxNS.TextChangedEventArgs( editSource.Range ) );
    }



    public override void RefreshDisplayOptions()
    {
      base.RefreshDisplayOptions();

      BackColor = Core.Theming.DarkenColor( GR.Color.Helper.FromARGB( Core.Settings.BGColor( ColorableElement.BACKGROUND_CONTROL ) ) );

      // Font
      editSource.Font = new System.Drawing.Font( Core.Settings.SourceFontFamily, Core.Settings.SourceFontSize, Core.Settings.SourceFontStyle );

      // Colors
      editSource.Language = FastColoredTextBoxNS.Language.Custom;

      // adjust caret color (Thanks Tulan!)
      System.Drawing.Color    backColorForCaret = GR.Color.Helper.FromARGB( Core.Settings.BGColor( ColorableElement.EMPTY_SPACE ) );
      if ( ( 0.2126 * backColorForCaret.R + 0.7152 * backColorForCaret.G + 0.0722 * backColorForCaret.B ) < 127.5 )
      {
        editSource.CaretColor = System.Drawing.Color.White;
      }
      else
      {
        editSource.CaretColor = System.Drawing.Color.Black;
      }

      ApplySyntaxColoring( Types.ColorableElement.EMPTY_SPACE );
      ApplySyntaxColoring( Types.ColorableElement.COMMENT );
      ApplySyntaxColoring( Types.ColorableElement.LITERAL_NUMBER );
      ApplySyntaxColoring( Types.ColorableElement.LITERAL_STRING );
      ApplySyntaxColoring( Types.ColorableElement.IMMEDIATE_LABEL );
      ApplySyntaxColoring( Types.ColorableElement.LABEL );
      ApplySyntaxColoring( Types.ColorableElement.CODE );
      ApplySyntaxColoring( Types.ColorableElement.MACRO_CALL );
      ApplySyntaxColoring( Types.ColorableElement.OPERATOR );
      ApplySyntaxColoring( Types.ColorableElement.CURRENT_DEBUG_LINE );
      ApplySyntaxColoring( Types.ColorableElement.NONE );
      ApplySyntaxColoring( Types.ColorableElement.PSEUDO_OP );
      ApplySyntaxColoring( Types.ColorableElement.HIGHLIGHTED_SEARCH_RESULTS );
      ApplySyntaxColoring( Types.ColorableElement.ERROR_UNDERLINE );
      ApplySyntaxColoring( Types.ColorableElement.WARNING_UNDERLINE );

      editSource.AllowTabs            = true;
      editSource.ConvertTabsToSpaces  = Core.Settings.FormatSettings.TabConvertToSpaces;
      editSource.TabLength            = Core.Settings.FormatSettings.TabSize;
      editSource.ShowLineNumbers      = !Core.Settings.ASMHideLineNumbers;
      editSource.PreferredLineWidth   = Core.Settings.ASMShowMaxLineLengthIndicatorLength;
      editSource.CaretWidth           = Core.Settings.CaretWidth;

      if ( Core.Settings.ASMShowMiniView )
      {
        miniMap.Visible = Core.Settings.ASMShowMiniView;
        editSource.Width = miniMap.Left;
      }
      else
      {
        miniMap.Visible = Core.Settings.ASMShowMiniView;
        editSource.Width = ClientSize.Width;
      }
      AdjustFontSizeInLeftBorder();

      //call OnTextChanged for refresh syntax highlighting
      ResetAllStyles( editSource.Range );
      editSource.OnSyntaxHighlight( new FastColoredTextBoxNS.TextChangedEventArgs( editSource.Range ) );

      // update manually set accelerators
      UpdateKeyBinding( RetroDevStudio.Types.Function.DELETE_LINE, FastColoredTextBoxNS.FCTBAction.DeleteLine );
      UpdateKeyBinding( RetroDevStudio.Types.Function.COPY_LINE_DOWN, FastColoredTextBoxNS.FCTBAction.CopyLineDown );
      UpdateKeyBinding( RetroDevStudio.Types.Function.COPY_LINE_UP, FastColoredTextBoxNS.FCTBAction.CopyLineUp );
      UpdateKeyBinding( RetroDevStudio.Types.Function.MOVE_LINE_UP, FastColoredTextBoxNS.FCTBAction.MoveSelectedLinesUp );
      UpdateKeyBinding( RetroDevStudio.Types.Function.MOVE_LINE_DOWN, FastColoredTextBoxNS.FCTBAction.MoveSelectedLinesDown );

      UpdateKeyBinding( RetroDevStudio.Types.Function.COPY, FastColoredTextBoxNS.FCTBAction.Copy );
      UpdateKeyBinding( RetroDevStudio.Types.Function.PASTE, FastColoredTextBoxNS.FCTBAction.Paste );
      UpdateKeyBinding( RetroDevStudio.Types.Function.CUT, FastColoredTextBoxNS.FCTBAction.Cut );

      UpdateKeyBinding( RetroDevStudio.Types.Function.FIND_NEXT, FastColoredTextBoxNS.FCTBAction.FindNext );

      UpdateKeyBinding( RetroDevStudio.Types.Function.BOOKMARK_ADD, FastColoredTextBoxNS.FCTBAction.BookmarkLine );
      UpdateKeyBinding( RetroDevStudio.Types.Function.BOOKMARK_DELETE, FastColoredTextBoxNS.FCTBAction.UnbookmarkLine );
      UpdateKeyBinding( RetroDevStudio.Types.Function.BOOKMARK_PREVIOUS, FastColoredTextBoxNS.FCTBAction.GoPrevBookmark );
      UpdateKeyBinding( RetroDevStudio.Types.Function.BOOKMARK_NEXT, FastColoredTextBoxNS.FCTBAction.GoNextBookmark );

      UpdateKeyBinding( RetroDevStudio.Types.Function.UNDO, FastColoredTextBoxNS.FCTBAction.Undo );
      UpdateKeyBinding( RetroDevStudio.Types.Function.REDO, FastColoredTextBoxNS.FCTBAction.Redo );

      UpdateKeyBinding( Function.NAVIGATE_BACK, FastColoredTextBoxNS.FCTBAction.NavigateBackward );
      UpdateKeyBinding( Function.NAVIGATE_FORWARD, FastColoredTextBoxNS.FCTBAction.NavigateForward );

      miniMap.ForeColor = GR.Color.Helper.FromARGB( Core.Settings.FGColor( ColorableElement.MINI_MAP ) );
      miniMap.BackColor = GR.Color.Helper.FromARGB( Core.Settings.BGColor( ColorableElement.EMPTY_SPACE ) );
    }



    private void UpdateKeyBinding( Types.Function Function, FastColoredTextBoxNS.FCTBAction Action )
    {
      editSource.HotkeysMapping.RemoveAllMappingsForAction( Action );

      var     accelerator = Core.Settings.DetermineAccelerator( Function );
      if ( accelerator != null )
      {
        editSource.HotkeysMapping.Remove( accelerator.Key );
        editSource.HotkeysMapping.Add( accelerator.Key, Action );
        if ( accelerator.SecondaryKey != Keys.None )
        {
          editSource.HotkeysMapping.Remove( accelerator.SecondaryKey );
          editSource.HotkeysMapping.Add( accelerator.SecondaryKey, Action );
        }
      }
    }



    public override bool ApplyFunction( RetroDevStudio.Types.Function Function )
    {
      switch ( Function )
      {
        case Types.Function.PRINT:
          editSource.Print( new FastColoredTextBoxNS.PrintDialogSettings 
                                  {
                                    ShowPageSetupDialog = true, ShowPrintDialog = true, ShowPrintPreviewDialog = true
                                  } );
          return true;
        case Function.COLLAPSE_ALL_FOLDING_BLOCKS:
          CollapseAllFoldingBlocks();
          return true;
        case Function.EXPAND_ALL_FOLDING_BLOCKS:
          ExpandAllFoldingBlocks();
          return true;
        case Function.JUMP_TO_LINE:
          JumpToLine();
          return true;
        case Function.BOOKMARK_DELETE_ALL:
          if ( !editSource.Bookmarks.Any() )
          {
            return true;
          }
          editSource.Bookmarks.Clear();

          RaiseDocEvent( new DocEvent( DocEvent.Type.ALL_BOOKMARKS_OF_DOCUMENT_REMOVED ) );
          StoreBookmarks();
          editSource.Invalidate();
          return true;
      }
      return false;
    }



    private void JumpToLine()
    {
      var formLine = new FormGotoLine( Core );

      if ( formLine.ShowDialog() == DialogResult.OK )
      {
        editSource.Navigate( formLine.LineNumber - 1 );
      }
    }



    private void CollapseAllFoldingBlocks()
    {
      editSource.CollapseAllFoldingBlocks();
    }



    private void ExpandAllFoldingBlocks()
    {
      editSource.ExpandAllFoldingBlocks();
    }



    private void copyToolStripMenuItem_Click( object sender, EventArgs e )
    {
      editSource.Copy();
    }



    private void pasteToolStripMenuItem_Click( object sender, EventArgs e )
    {
      editSource.Paste();
    }



    private void editSource_DragEnter( object sender, DragEventArgs e )
    {
      if ( ( e.Data.GetDataPresent( DataFormats.FileDrop ) )
      ||   ( e.Data.GetDataPresent( DataFormats.Text ) ) )
      {
        e.Effect = DragDropEffects.Copy;
      }
      else
      {
        e.Effect = DragDropEffects.None;
      }
    }



    private void editSource_DragDrop( object sender, DragEventArgs e )
    {
      try
      {
        Array a = (Array)e.Data.GetData( DataFormats.FileDrop );
        if ( a != null )
        {
          foreach ( string file in a )
          {
            Core.MainForm.OpenFile( file );
          }
        }
        else if ( e.Data.GetDataPresent( DataFormats.Text ) )
        {
          base.OnDragDrop( e );
        }
      }
      catch ( Exception ex )
      {
        Debug.Log( "Error in DragDrop function: " + ex.Message );
      }
    }



    internal DateTime LastChange
    {
      get
      {
        return m_LastChange;
      }
    }



    private void hideToolStripMenuItem_Click( object sender, EventArgs e )
    {
      if ( Core.Settings.ASMShowMiniView )
      {
        Core.Settings.ASMShowMiniView = false;
        Core.Settings.RefreshDisplayOnAllDocuments( Core );
      }
    }



    private void showMiniOverviewToolStripMenuItem_Click( object sender, EventArgs e )
    {
      Core.Settings.ASMShowMiniView = !Core.Settings.ASMShowMiniView;
      Core.Settings.RefreshDisplayOnAllDocuments( Core );
    }



    public override ByteBuffer SaveToBuffer()
    {
      var currentBookmarks = new List<int>();
      currentBookmarks.AddRange( editSource.Bookmarks.Select( x => x.LineIndex ) );

      var sourceData = new GR.IO.FileChunk( FileChunkConstants.SOURCE_TEXT_FILE );

      // version
      sourceData.AppendI32( 1 );
      sourceData.AppendString( editSource.Text );
      sourceData.AppendI32( CursorLine );
      sourceData.AppendI32( CursorPosInLine );

      var bookmarkData = new GR.IO.FileChunk( FileChunkConstants.BOOKMARKS );
      bookmarkData.AppendI32( currentBookmarks.Count );
      foreach ( var bookmarkLine in currentBookmarks )
      {
        bookmarkData.AppendI32( bookmarkLine );
      }
      sourceData.Append( bookmarkData.ToBuffer() );

      return sourceData.ToBuffer();
    }



    public override bool ReadFromReader( IReader Reader )
    {
      var chunk = new GR.IO.FileChunk();

      if ( ( !chunk.ReadFromStream( Reader ) )
      ||   ( chunk.Type != FileChunkConstants.SOURCE_TEXT_FILE ) )
      {
        return false;
      }

      var reader = chunk.BinaryReader();

      int version = reader.ReadInt32();
      if ( version != 1 )
      {
        return false;
      }

      editSource.Text = reader.ReadString();

      int   cursorLine = reader.ReadInt32();
      int   cursorPos = reader.ReadInt32();

      SetCursorToLine( cursorLine, cursorPos, false );


      var subChunk = new GR.IO.FileChunk();

      while ( subChunk.ReadFromStream( Reader ) )
      {
        var subReader = subChunk.MemoryReader();
        switch ( subChunk.Type )
        {
          case FileChunkConstants.BOOKMARKS:
            {
              var   bookmarks = new List<int>();
              int   numBookmarks = subReader.ReadInt32();
              for ( int i = 0; i < numBookmarks; ++i )
              {
                bookmarks.Add( subReader.ReadInt32() );
              }

              editSource.BeginAutoUndo();
              editSource.Bookmarks.Clear();
              foreach ( var origBookmark in bookmarks )
              {
                editSource.Bookmarks.Add( origBookmark );
              }
            }
            break;
        }
      }

      return true;
    }



    private void ApplyDelta( TokenInfo token, long value, int delta, bool insertAsHex )
    {
      value += delta;

      if ( insertAsHex )
      {
        token.Content = "$" + value.ToString( "X2" );
      }
      else
      {
        token.Content = value.ToString();
      }
    }



    private void editSource_LineVisited( object sender, LineVisitedArgs e )
    {
      Core.Navigating.VisitedLine( DocumentInfo, e.LineIndex );
    }



    private void DecimalToHex( TokenInfo token, long value )
    {
      token.Content = "$" + value.ToString( "X2" );
    }



    public void SetLineText( string ReplacedText, int LineIndex )
    {
      if ( ( LineIndex < 0 )
      ||   ( LineIndex >= editSource.LinesCount ) )
      {
        return;
      }

      int oldV = editSource.VerticalScroll.Value;
      int oldH = editSource.HorizontalScroll.Value;

      editSource.Selection.Start  = new Place( 0, LineIndex );
      editSource.Selection.End    = new Place( editSource.Lines[LineIndex].Length, LineIndex );
      editSource.SelectedText     = ReplacedText;

      editSource.SetScrollOffsets( oldH, oldV );
    }



    private void addBookmarkHereToolStripMenuItem_Click( object sender, EventArgs e )
    {
      if ( editSource.Bookmarks.Any( bm => bm.LineIndex == m_ContextMenuLineIndex ) )
      {
        return;
      }
      editSource.Bookmarks.Add( m_ContextMenuLineIndex );
    }



    private void removeBookmarkToolStripMenuItem_Click( object sender, EventArgs e )
    {
      if ( !editSource.Bookmarks.Any( bm => bm.LineIndex == m_ContextMenuLineIndex ) )
      {
        return;
      }
      editSource.Bookmarks.Remove( m_ContextMenuLineIndex );
    }



    private void removeAllBookmarksOfThisFileToolStripMenuItem_Click( object sender, EventArgs e )
    {
      ApplyFunction( Function.BOOKMARK_DELETE_ALL );
    }



    private void goToLineToolStripMenuItem_Click( object sender, EventArgs e )
    {
      JumpToLine();
    }



    protected override void MarkModifiedLines( List<int> list )
    {
      foreach ( var line in list )
      {
        if ( ( line >= 0 ) 
        &&   ( line < editSource.LinesCount ) )
        {
          editSource.MarkLineAsChanged( line );
        }
      }
    }



  }
}
