using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using RetroDevStudio.Types;
using FastColoredTextBoxNS;

using System.Linq;
using System.Drawing;
using GR.Memory;
using GR.IO;
using System.Globalization;
using GR.Image;
using RetroDevStudio.Parser;
using RetroDevStudio.Dialogs;



namespace RetroDevStudio.Documents
{
  public partial class SourceASMEx : CompilableDocument
  {
    private const uint TME_HOVER = 0x00000001;
    private const uint TME_LEAVE = 0x00000002;
    private const uint HOVER_DEFAULT = 0xFFFFFFFF;

    private const int BORDER_MARKER_WIDTH   = 20;
    private const int BORDER_SIZE_WIDTH     = 24;
    private const int BORDER_CYCLES_WIDTH   = 48;
    private const int BORDER_ADDRESS_WIDTH  = 52;


    [DllImport("user32.dll")]
    public static extern bool TrackMouseEvent(ref TRACKMOUSEEVENT lpEventTrack);
     
    public struct TRACKMOUSEEVENT {
        public uint cbSize;
        public uint dwFlags;
        public IntPtr hwndTrack;
        public uint dwHoverTime;
    }

    int                                       m_CurrentMarkedLineIndex = -1;
    int                                       m_ContextMenuLineIndex = -1;
    int                                       m_ContextMenuPosition = -1;
    string                                    m_FilenameToOpen = "";
    System.Windows.Forms.ToolTip              m_ToolTip = new System.Windows.Forms.ToolTip();
    System.Drawing.Point                      m_LastTooltipPos = new System.Drawing.Point();
    string                                    m_LastTooltipText;
    int                                       m_LastZoneUpdateLine = -1;

    int                                       m_BreakpointOffset = 0;
    int                                       m_CycleOffset = -1;
    int                                       m_ByteSizeOffset = -1;
    int                                       m_AddressOffset = -1;

    bool                                      m_RecalcZone = false;

    // inserting text flag
    bool                                      m_InsertingText = false;

    DateTime                                  m_LastChange = DateTime.Now;

    Timer                                     m_DelayedEventTimer = new Timer();

    private string                            m_CurrentHighlightText = null;

    private string                            m_SyntaxColoringCurrentKnownCPU       = "";
    private AssemblerType                     m_SyntaxColoringCurrentKnownAssembler = AssemblerType.AUTO;

    private delegate void delSimpleEventHandler();



    private GR.Collections.Map<int,Types.Breakpoint>    m_BreakPoints = new GR.Collections.Map<int,RetroDevStudio.Types.Breakpoint>();

    public bool                               DoNotFollowZoneSelectors = false;

    private Parser.ASMFileParser              Parser = new RetroDevStudio.Parser.ASMFileParser();

    private FastColoredTextBoxNS.AutocompleteMenu   AutoComplete = null;

    private FastColoredTextBoxNS.Style[]  m_TextStyles = new FastColoredTextBoxNS.Style[(int)Types.ColorableElement.LAST_ENTRY];
    private System.Text.RegularExpressions.Regex[]    m_TextRegExp = new System.Text.RegularExpressions.Regex[(int)Types.ColorableElement.LAST_ENTRY];
    private  List<FastColoredTextBoxNS.AutocompleteItem> m_CurrentAutoCompleteSource = null;



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



    public SourceASMEx( StudioCore Core )
    {
      this.Core = Core;

      DocumentInfo.Type = ProjectElement.ElementType.ASM_SOURCE;
      DocumentInfo.UndoManager.MainForm = Core.MainForm;

      m_TextRegExp[(int)Types.ColorableElement.LITERAL_NUMBER] = new System.Text.RegularExpressions.Regex( @"\b\d+[\.]?\d*([eE]\-?\d+)?[lLdDfF]?\b|\B\$[a-fA-F\d]+\b|\b0x[a-fA-F\d]+\b" );
      m_TextRegExp[(int)Types.ColorableElement.LITERAL_STRING] = new System.Text.RegularExpressions.Regex( @"""""|''|"".*?[^\\]""|'.*?[^\\]'" );

      m_TextRegExp[(int)Types.ColorableElement.LABEL] = new System.Text.RegularExpressions.Regex( @"[.@]{0,1}[+\-a-zA-Z]+[a-zA-Z_\d.]*[:]*" );
      m_TextRegExp[(int)Types.ColorableElement.COMMENT] = new System.Text.RegularExpressions.Regex( @";.*" );

      m_TextRegExp[(int)Types.ColorableElement.OPERATOR] = new System.Text.RegularExpressions.Regex( @"[+\-/*(){}=<>,#%]" );
      m_TextRegExp[(int)Types.ColorableElement.NONE] = new System.Text.RegularExpressions.Regex( @"\S" );

      m_IsSaveable = true;

      InitializeComponent();

      copyToolStripMenuItem.Tag                         = Function.COPY;
      pasteToolStripMenuItem.Tag                        = Function.PASTE;
      cutToolStripMenuItem.Tag                          = Function.CUT;
      runToCursorToolStripMenuItem.Tag                  = Function.DEBUG_RUN_TO;
      gotoDeclarationToolStripMenuItem.Tag              = Function.GO_TO_DECLARATION;
      findAllReferencesToolStripMenuItem.Tag            = Function.FIND_ALL_REFERENCES;
      renameAllReferencesToolStripMenuItem.Tag          = Function.RENAME_ALL_REFERENCES;
      helpToolStripMenuItem.Tag                         = Function.HELP;
      commentSelectionToolStripMenuItem.Tag             = Function.COMMENT_SELECTION;
      uncommentSelectionToolStripMenuItem.Tag           = Function.UNCOMMENT_SELECTION;
      addBookmarkHereToolStripMenuItem.Tag              = Function.BOOKMARK_ADD;
      removeBookmarkToolStripMenuItem.Tag               = Function.BOOKMARK_DELETE;
      removeAllBookmarksOfThisFileToolStripMenuItem.Tag = Function.BOOKMARK_DELETE_ALL;

      SuspendLayout();

      DPIHandler.ResizeControlsForDPI( this );

      m_DelayedEventTimer.Interval = 500;
      m_DelayedEventTimer.Tick += m_DelayedEventTimer_Tick;

      AutoComplete = new FastColoredTextBoxNS.AutocompleteMenu( editSource );
      //AutoComplete.SearchPattern = @"([A-Za-z_.]|(?<=[A-Za-z_.][\w]))";
      AutoComplete.SearchPattern = @"[A-Za-z_.][\w.]*";
      AutoComplete.PrepareOpening += AutoComplete_PrepareOpening;
      AutoComplete.PrepareSorting += AutoComplete_PrepareSorting;

      editSource.AutoIndentExistingLines = false;
      editSource.AutoIndentChars = false;
      editSource.WordCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_@";

      // allows auto indent on new lines, indents similar to previous line
      editSource.AutoIndent = true;
      editSource.ShowLineNumbers = !Core.Settings.ASMHideLineNumbers;

      //editSource.FindEndOfFoldingBlockStrategy = FastColoredTextBoxNS.FindEndOfFoldingBlockStrategy.Strategy2;

      Parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      contextSource.Opening += new CancelEventHandler( contextSource_Opening );

      editSource.Font = new System.Drawing.Font( Core.Settings.SourceFontFamily, Core.Settings.SourceFontSize, Core.Settings.SourceFontStyle );
      editSource.KeyDown += new System.Windows.Forms.KeyEventHandler( editSource_KeyDown );
      editSource.KeyPress += new System.Windows.Forms.KeyPressEventHandler( editSource_KeyPress );
      editSource.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler( editSource_PreviewKeyDown );

      editSource.SelectionChanged += new EventHandler( editSource_SelectionChanged );

      editSource.MouseHover += new EventHandler( editSource_MouseHover );
      editSource.ZoomChanged += EditSource_ZoomChanged;

      this.Activated += new EventHandler( SourceASM_Activated );

      editSource.TextChanged += new EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>( editSource_TextChanged );
      editSource.TextChangedDelayed += editSource_TextChangedDelayed;
      editSource.TextInserted += EditSource_TextInserted;
      editSource.TextDeleted += EditSource_TextDeleted;
      editSource.FoldingBlockStateChanged += editSource_FoldingBlockStateChanged;

      editSource.LineInserted += new EventHandler<FastColoredTextBoxNS.LineInsertedEventArgs>( editSource_LineInserted );
      editSource.LineRemoved += new EventHandler<FastColoredTextBoxNS.LineRemovedEventArgs>( editSource_LineRemoved );

      editSource.BookmarkAdded += EditSource_BookmarkAdded;
      editSource.BookmarkRemoved += EditSource_BookmarkRemoved;

      editSource.UndoRedoStateChanged += new EventHandler<EventArgs>( editSource_UndoRedoStateChanged );

      editSource.LeftBracket = '(';
      editSource.RightBracket = ')';
      editSource.LeftBracket2 = '\x0';
      editSource.RightBracket2 = '\x0';
      editSource.CommentPrefix = ";";
      editSource.SelectionChangedDelayed += editSource_SelectionChangedDelayed;
      editSource.PreferredLineWidth = Core.Settings.ASMShowMaxLineLengthIndicatorLength;

      btnShowShortCutLabels.Image = Core.Settings.ASMShowShortCutLabels ? RetroDevStudio.Properties.Resources.flag_blue_on.ToBitmap() : RetroDevStudio.Properties.Resources.flag_blue_off.ToBitmap();

      UpdatePseudoOpSyntaxColoringSource();
      UpdateOpcodeSyntaxColoringSource();

      RefreshDisplayOptions();

      //editSource.ContextMenuStrip = null;
      //editSource.ContextMenu = null;

      editSource.AllowDrop = true;
      editSource.MouseEnter += new EventHandler( editSource_MouseEnter );
      editSource.MouseLeave += new EventHandler(editSource_MouseLeave);
      editSource.MouseMove += new System.Windows.Forms.MouseEventHandler( editSource_MouseMove );

      m_ToolTip.Active = true;
      m_ToolTip.SetToolTip( editSource, "x" );
      m_ToolTip.Popup += new System.Windows.Forms.PopupEventHandler( m_ToolTip_Popup );

      // we start out with one line
      m_LineInfos.Add( new Types.ASM.LineInfo() );

      contextSource.Opened += new EventHandler( contextSource_Opened );

      ResumeLayout();
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
      int     firstLine = e.InsertedRange.Start.iLine;
      int     count = e.InsertedRange.End.iLine - e.InsertedRange.Start.iLine;
      if ( firstLine > e.InsertedRange.End.iLine )
      {
        firstLine = e.InsertedRange.End.iLine;
        count = -count;
      }
      if ( count == 0 )
      {
        return;
      }

      // special case, if we insert an empty line, insert "below"
      int     indexToNotify = firstLine;

      /*
      if ( editSource.Lines[firstLine].Trim().Length == 0 )
      {
        ++indexToNotify;
      }*/

      if ( !m_InsertingText )
      {
        Core.Navigating.InsertLines( DocumentInfo, firstLine, count );
      }

      // move related breakpoints!
      for ( int i = 0; i < count; ++i )
      {
        var info = new Types.ASM.LineInfo();
        if ( ( indexToNotify > 0 )
        &&   ( indexToNotify - 1 < m_LineInfos.Count ) )
        {
          info.AddressStart = m_LineInfos[indexToNotify - 1].AddressStart;
        }
        while ( indexToNotify >= m_LineInfos.Count )
        {
          m_LineInfos.Add( new Types.ASM.LineInfo() );
        }
        m_LineInfos.Insert( indexToNotify, info );
      }

      if ( !m_InsertingText )
      {
        int                         insertedAtLine = firstLine;

        GR.Collections.Map<int,Types.Breakpoint>   origBreakpoints = new GR.Collections.Map<int,RetroDevStudio.Types.Breakpoint>( m_BreakPoints );
        List<Types.Breakpoint>                     movedBreakpoints = new List<RetroDevStudio.Types.Breakpoint>();

        foreach ( int breakpointLine in origBreakpoints.Keys )
        {
          var bp = origBreakpoints[breakpointLine];

          if ( breakpointLine >= insertedAtLine )
          {
            bp.LineIndex += count;
            movedBreakpoints.Add( bp );

            RaiseDocEvent( new DocEvent( DocEvent.Type.BREAKPOINT_UPDATED, bp ) );
          }
          else
          {
            movedBreakpoints.Add( bp );
          }
        }
        m_BreakPoints.Clear();

        foreach ( var bp in movedBreakpoints )
        {
          m_BreakPoints[bp.LineIndex] = bp;
        }
        //UpdateFoldingBlocks();
        //StoreFoldedBlocks();
      }
    }



    private void EditSource_TextDeleted( object sender, TextDeletedEventArgs e )
    {
      int     firstLine = e.DeletedRange.Start.iLine;
      int     count = e.DeletedRange.End.iLine - e.DeletedRange.Start.iLine;
      if ( firstLine > e.DeletedRange.End.iLine )
      {
        firstLine = e.DeletedRange.End.iLine;
        count = -count;
      }
      if ( count == 0 )
      {
        return;
      }
      Core.Navigating.RemoveLines( DocumentInfo, firstLine, count );

      //Debug.Log( "Lines removed " + e.Index + ", " + e.Count );
      if ( ( firstLine >= 0 )
      &&   ( firstLine + count <= m_LineInfos.Count ) )
      {
        m_LineInfos.RemoveRange( firstLine, count );
      }

      // move related breakpoints!
      int deletedAtLine = firstLine;

      GR.Collections.Map<int,Types.Breakpoint> origBreakpoints = new GR.Collections.Map<int, RetroDevStudio.Types.Breakpoint>( m_BreakPoints );

      foreach ( int breakpointLine in origBreakpoints.Keys )
      {
        if ( ( breakpointLine >= firstLine )
        &&   ( breakpointLine < firstLine + count ) )
        {
          // BP was deleted!
          RaiseDocEvent( new DocEvent( DocEvent.Type.BREAKPOINT_REMOVED, origBreakpoints[breakpointLine] ) );
        }
        else if ( breakpointLine >= deletedAtLine )
        {
          Types.Breakpoint bpToMove = m_BreakPoints[breakpointLine];
          m_BreakPoints.Remove( breakpointLine );
          m_BreakPoints.Add( breakpointLine - count, bpToMove );
          bpToMove.LineIndex -= count;
          RaiseDocEvent( new DocEvent( DocEvent.Type.BREAKPOINT_UPDATED, bpToMove ) );
        }
      }
      //UpdateFoldingBlocks();
      //StoreFoldedBlocks();
    }



    private void AutoComplete_PrepareSorting( object sender, PrepareSortingEventArgs e )
    {
      GR.Collections.MultiMap<int,AutocompleteItem>     sortedItems = new GR.Collections.MultiMap<int, AutocompleteItem>();

      string[]  filterParts = e.FilterText.Split( '.' );

      foreach ( var item in AutoComplete.Items.AllItems )// e.FilteredItems )
      {
        int     bestRelevance = 100000;

        string[] itemParts = item.Text.Split( '.' );

        if ( filterParts.Length <= itemParts.Length )
        {
          for ( int i = 0; i < Math.Min( filterParts.Length, itemParts.Length ); ++i )
          {
            int   relevance = 100000;

            string    itemText = itemParts[i];
            string    filterText = filterParts[i];

            if ( itemText == filterText )
            {
              // full match
              relevance = 0;
            }
            else if ( string.Compare( itemText, filterText, true ) == 0 )
            {
              // full match but case mismatch
              relevance = 1;
            }
            else if ( itemText.StartsWith( filterText ) )
            {
              // starting with exact match -> TODO - number of chars appended?
              relevance = 2;
            }
            else if ( string.Compare( itemText, 0, filterText, 0, filterText.Length, true ) == 0 )
            {
              // starting with exact match but mismatching case -> TODO - number of chars appended?
              relevance = 3;
            }
            else if ( itemText.Contains( filterText ) )
            {
              // containing exact match -> TODO - number of chars appended?
              relevance = 5;
            }
            else if ( CultureInfo.InvariantCulture.CompareInfo.IndexOf( itemText, filterText, CompareOptions.IgnoreCase ) > 0 )
            {
              // containing case mismatch -> TODO - number of chars appended?
              relevance = 6;
            }
            //Debug.Log( "Item " + item.Text + ", relevance " + relevance );

            if ( relevance < bestRelevance )
            {
              bestRelevance = relevance;
            }
          }
        }
        sortedItems.Add( bestRelevance, item );
      }

      e.FilteredItems = sortedItems.Values;
    }



    private void AutoComplete_PrepareOpening( object sender, PrepareOpeningEventArgs e )
    {
      if ( e == null )
      {
        return;
      }
      // check the 
      int     sourceLineIndex = e.StartPos.iLine;
      if ( ( sourceLineIndex < 0 )
      ||   ( sourceLineIndex >= editSource.LinesCount ) )
      {
        return;
      }

      // no autocomplete on comments
      string    line = editSource.Lines[sourceLineIndex];
      var tokens = Parser.ParseTokenInfo( line, 0, line.Length, Parser.m_TextCodeMappingScr );

      if ( tokens != null )
      {
        foreach ( var token in tokens )
        {
          if ( ( token.StartPos < e.StartPos.iChar )
          &&   ( token.Type == TokenInfo.TokenType.COMMENT ) )
          {
            e.Cancel = true;
            return;
          }
        }
      }
      /*
      // this is just a attempt on context dependant info!
      // TODO - adjust the content depending on the content
      string    line = editSource.Lines[sourceLineIndex];
      var tokens = Parser.ParseTokenInfo( line, 0, line.Length );

      if ( ( tokens != null )
      &&   ( tokens.Count > 0 )
      &&   ( tokens[0].Type == TokenInfo.TokenType.MACRO )
      &&   ( 
      &&   ( tokens[0].Content.ToUpper() == "!BASIC" ) )
      {
        var   newList = new List<FastColoredTextBoxNS.AutocompleteItem>();

        int i = 0;
        newList.Add( new FastColoredTextBoxNS.AutocompleteItem( "hullo" ) { ToolTipTitle = tokens[i].Content + ",x", ToolTipText = tokens[i].Content + ",x" } );

        AutoComplete.Items.SetAutocompleteItems( newList );
        return;
      }*/

      // common stuff
      FilterAutoComplete( sourceLineIndex );
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
      if ( Core.Settings.ASMShowAddress )
      {
        m_AddressOffset = newPadding;
        newPadding += approxWidthOfAddress + approxWidthOfChar;
        numChars += 5 + 1;
      }
      if ( Core.Settings.ASMShowBytes )
      {
        m_ByteSizeOffset = newPadding;
        newPadding += 3 * approxWidthOfChar;
        numChars += 3;
      }
      if ( Core.Settings.ASMShowCycles )
      {
        m_CycleOffset = newPadding;
        newPadding += 4 * approxWidthOfChar;
        numChars += 4;
      }
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
      if ( m_CurrentHighlightText != newHighlightText )
      {
        if ( !string.IsNullOrEmpty( m_CurrentHighlightText ) )
        {
          editSource.Range.ClearStyle( m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.HIGHLIGHTED_SEARCH_RESULTS )] );
        }

        m_CurrentHighlightText = newHighlightText;
        //ResetStyles( editSource.Range );
        if ( !string.IsNullOrEmpty( m_CurrentHighlightText ) )
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



    void m_DelayedEventTimer_Tick( object sender, EventArgs e )
    {
      m_DelayedEventTimer.Stop();

      if ( m_RecalcZone )
      {
        m_RecalcZone = false;
        UpdateSelectedZone();
      }
    }



    void editSource_FoldingBlockStateChanged( object sender, EventArgs e )
    {
      StoreFoldedBlocks();
      //Debug.Log( "Folded block state changed" );
    }



    void editSource_TextChangedDelayed( object sender, FastColoredTextBoxNS.TextChangedEventArgs e )
    {
      //return;
      //Debug.Log( "editSource_TextChangedDelayed for " + DocumentInfo.FullPath );
      //ResetAllStyles( e.ChangedRange );

      //ShowAutoComplete();

      UpdateFoldingBlocks();
      StoreFoldedBlocks();

      // select new zone?
      UpdateSelectedZone();
    }



    void ResetAllStyles( FastColoredTextBoxNS.Range Range )
    {
      //clear previous highlighting but error highlighting
      var indexMask = editSource.GetStyleIndexMask( m_TextStyles );
      indexMask = (FastColoredTextBoxNS.StyleIndex)( ( (int)indexMask ) & ~(int)FastColoredTextBoxNS.StyleIndex.Style10 );
      Range.ClearStyle( indexMask );

      // apply all styles
      Range.SetStyle( m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.LITERAL_NUMBER )], m_TextRegExp[(int)Types.ColorableElement.LITERAL_NUMBER] );
      Range.SetStyle( m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.LITERAL_STRING )], m_TextRegExp[(int)Types.ColorableElement.LITERAL_STRING] );
      Range.SetStyle( m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.CODE )], m_TextRegExp[(int)Types.ColorableElement.CODE] );
      Range.SetStyle( m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.PSEUDO_OP )], m_TextRegExp[(int)Types.ColorableElement.PSEUDO_OP] );
      Range.SetStyle( m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.LABEL )], m_TextRegExp[(int)Types.ColorableElement.LABEL] );
      Range.SetStyle( m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.COMMENT )], m_TextRegExp[(int)Types.ColorableElement.COMMENT] );
      Range.SetStyle( m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.OPERATOR )], m_TextRegExp[(int)Types.ColorableElement.OPERATOR] );
      Range.SetStyle( m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.NONE )], m_TextRegExp[(int)Types.ColorableElement.NONE] );
      if ( m_CurrentHighlightText != null )
      {
        string    regex = m_CurrentHighlightText.Replace( @"\", @"\\" );
        regex = regex.Replace( @"^", @"\^" );
        regex = regex.Replace( @"$", @"\$" );
        regex = regex.Replace( @".", @"\." );
        regex = regex.Replace( @"|", @"\|" );
        regex = regex.Replace( @"?", @"\?" );
        regex = regex.Replace( @"*", @"\*" );
        regex = regex.Replace( @"+", @"\+" );
        regex = regex.Replace( @"(", @"\(" );
        regex = regex.Replace( @")", @"\)" );
        regex = regex.Replace( @"{", @"\{" );
        regex = regex.Replace( @"[", @"\[" );

        Range.SetStyle( m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.HIGHLIGHTED_SEARCH_RESULTS )], regex );
      }
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
      int     value = 10;

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
        case Types.ColorableElement.LABEL:
          value = 9;
          break;
        case Types.ColorableElement.ERROR_UNDERLINE:
          value = 10;
          break;
        case Types.ColorableElement.NONE:
          value = 11;
          break;

      }
      return value;
    }



    void editSource_LineInserted( object sender, FastColoredTextBoxNS.LineInsertedEventArgs e )
    {
      DocumentInfo.Bookmarks.Clear();
      foreach ( var bm in editSource.Bookmarks )
      {
        DocumentInfo.Bookmarks.Add( bm.LineIndex );
      }
      RaiseDocEvent( new DocEvent( DocEvent.Type.BOOKMARKS_UPDATED ) );



      /*
      // special case, if we insert an empty line, insert "below"
      int     indexToNotify = e.Index;

      if ( editSource.Lines[e.Index].Trim().Length == 0 )
      {
        ++indexToNotify;
      }

      if ( !m_InsertingText )
      {
        Core.Navigating.InsertLines( DocumentInfo, e.Index, e.Count );
      }

      // move related breakpoints!
      for ( int i = 0; i < e.Count; ++i )
      {
        var info = new Types.ASM.LineInfo();
        if ( ( indexToNotify > 0 )
        &&   ( indexToNotify - 1 < m_LineInfos.Count ) )
        {
          info.AddressStart = m_LineInfos[indexToNotify - 1].AddressStart;
        }
        m_LineInfos.Insert( indexToNotify - 1, info );
      }

      if ( !m_InsertingText )
      {
        int                         insertedAtLine = e.Index;

        GR.Collections.Map<int,Types.Breakpoint>   origBreakpoints = new GR.Collections.Map<int,RetroDevStudio.Types.Breakpoint>( m_BreakPoints );
        List<Types.Breakpoint>                     movedBreakpoints = new List<RetroDevStudio.Types.Breakpoint>();

        foreach ( int breakpointLine in origBreakpoints.Keys )
        {
          var bp = origBreakpoints[breakpointLine];

          if ( breakpointLine >= insertedAtLine )
          {
            bp.LineIndex += e.Count;
            movedBreakpoints.Add( bp );
          }
          else
          {
            movedBreakpoints.Add( bp );
          }
        }
        m_BreakPoints.Clear();

        foreach ( var bp in movedBreakpoints )
        {
          m_BreakPoints[bp.LineIndex] = bp;
        }
        //UpdateFoldingBlocks();
        //StoreFoldedBlocks();
      }*/
    }



    void editSource_LineRemoved( object sender, FastColoredTextBoxNS.LineRemovedEventArgs e )
    {
      DocumentInfo.Bookmarks.Clear();
      foreach ( var bm in editSource.Bookmarks )
      {
        DocumentInfo.Bookmarks.Add( bm.LineIndex );
      }
      RaiseDocEvent( new DocEvent( DocEvent.Type.BOOKMARKS_UPDATED ) );

      /*
      Core.Navigating.RemoveLines( DocumentInfo, e.Index, e.Count );

      //Debug.Log( "Lines removed " + e.Index + ", " + e.Count );
      m_LineInfos.RemoveRange( e.Index - 1, e.Count );

      // move related breakpoints!
      int deletedAtLine = e.Index;

      GR.Collections.Map<int,Types.Breakpoint> origBreakpoints = new GR.Collections.Map<int, RetroDevStudio.Types.Breakpoint>( m_BreakPoints );

      foreach ( int breakpointLine in origBreakpoints.Keys )
      {
        if ( ( breakpointLine >= deletedAtLine )
        &&   ( breakpointLine < deletedAtLine + e.Count ) )
        {
          // BP was deleted!
          RaiseDocEvent( new DocEvent( DocEvent.Type.BREAKPOINT_REMOVED, origBreakpoints[breakpointLine] ) );
        }
        else if ( breakpointLine >= deletedAtLine )
        {
          Types.Breakpoint bpToMove = m_BreakPoints[breakpointLine];
          m_BreakPoints.Remove( breakpointLine );
          m_BreakPoints.Add( breakpointLine - e.Count, bpToMove );
          bpToMove.LineIndex -= e.Count;
          RaiseDocEvent( new DocEvent( DocEvent.Type.BREAKPOINT_UPDATED, bpToMove ) );
        }
      }
      //UpdateFoldingBlocks();
      //StoreFoldedBlocks();
      */
    }



    void editSource_TextChanged( object sender, FastColoredTextBoxNS.TextChangedEventArgs e )
    {
      m_LastChange = DateTime.Now;

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

      ResetAllStyles( e.ChangedRange );

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
      if ( Element == Types.ColorableElement.ERROR_UNDERLINE )
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



    public void RemoveBreakpoint( Types.Breakpoint Breakpoint )
    {
      if ( m_BreakPoints.ContainsKey( Breakpoint.LineIndex ) )
      {
        m_BreakPoints.Remove( Breakpoint.LineIndex );
        InvalidateMarkerAreaAtLine( Breakpoint.LineIndex );
      }
    }



    public override bool BreakpointToggleable
    {
      set
      {
        //editSource.Margins.Margin1.IsClickable = value;
      }
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



    public void ToggleBreakpoint( int LineIndex )
    {
      ToggleBreakpointOnEvent( LineIndex );
    }



    void ToggleBreakpointOnEvent( int LineIndex )
    {
      // break point set
      if ( !m_BreakPoints.ContainsKey( LineIndex ) )
      {
        if ( SetBreakpoint( LineIndex, out Breakpoint bp ) )
        {
          RaiseDocEvent( new DocEvent( DocEvent.Type.BREAKPOINT_ADDED, bp ) );
        }
      }
      else
      {
        RemoveBreakpoint( LineIndex );
      }
    }



    private bool SetBreakpoint( int LineIndex, out Breakpoint BP )
    {
      BP = null;


      var bp = new RetroDevStudio.Types.Breakpoint();

      bp.DocumentFilename = DocumentInfo.FullPath;
      bp.LineIndex = LineIndex;

      Types.ASM.FileInfo fileInfo = Core.Navigating.DetermineASMFileInfo( DocumentInfo );

      if ( Core.State == RetroDevStudio.Types.StudioState.DEBUGGING_BROKEN )
      {
        int   globalLineIndex = -1;
        if ( fileInfo.FindGlobalLineIndex( LineIndex, DocumentInfo.FullPath, out globalLineIndex ) )
        {
          bp.Address = fileInfo.FindLineAddress( globalLineIndex );
          //Debug.Log( "Found address " + bp.Address.ToString( "x4" ) );
        }
        else
        {
          return false;
        }
      }
      else if ( Core.State != RetroDevStudio.Types.StudioState.NORMAL )
      {
        // cannot add breakpoints during this state
        return false;
      }

      AddBreakpoint( bp );

      BP = bp;
      return true;
    }



    public void AddBreakpoint( Breakpoint BP )
    {
      m_BreakPoints.Add( BP.LineIndex, BP );
      InvalidateMarkerAreaAtLine( BP.LineIndex );
    }



    public void RemoveBreakpoint( int LineIndex )
    {
      if ( m_BreakPoints.ContainsKey( LineIndex ) )
      {
        Types.Breakpoint bp = m_BreakPoints[LineIndex];
        m_BreakPoints.Remove( LineIndex );
        RaiseDocEvent( new DocEvent( DocEvent.Type.BREAKPOINT_REMOVED, bp ) );

        InvalidateMarkerAreaAtLine( LineIndex );
      }
    }



    void editSource_MouseHover( object sender, EventArgs e )
    {
      //Debug.Log( "hover" );
      TRACKMOUSEEVENT trackMouseEvent = new TRACKMOUSEEVENT();
      trackMouseEvent.hwndTrack = ( (Control)sender ).Handle;
      trackMouseEvent.dwFlags = TME_HOVER;
      trackMouseEvent.dwHoverTime = HOVER_DEFAULT;
      trackMouseEvent.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf( trackMouseEvent );
      TrackMouseEvent( ref trackMouseEvent );

      if ( !m_ToolTip.Active )
      {
        m_ToolTip.Show( "hurz", editSource );
        //Console.WriteLine( "Show tooltip" );
      }
    }



    void editSource_PreviewKeyDown( object sender, System.Windows.Forms.PreviewKeyDownEventArgs e )
    {
      //UpdateStatusInfo();
    }



    void UpdateStatusInfo()
    {
      if ( Core.MainForm.ActiveDocumentInfo != this.DocumentInfo )
      {
        return;
      }

      string    newInfo = "Row " + ( editSource.Selection.Start.iLine + 1 ).ToString() + ", Col " + editSource.Selection.Start.iChar.ToString();

      int       numBytes = 0;
      int       numCycles = 0;
      bool      hadPotentialEntry = false;
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
      for ( int i = startLine; i <= endLine; ++i )
      {
        var lineInfo = FetchLineInfo( i );
        if ( lineInfo != null )
        {
          numBytes += lineInfo.NumBytes;
          if ( lineInfo.Opcode != null )
          {
            numCycles += lineInfo.Opcode.NumCycles;

            if ( lineInfo.Opcode.PageBoundaryCycles != 0 )
            {
              potentialCycles[0] += lineInfo.Opcode.PageBoundaryCycles;
              hadPotentialEntry = true;
            }
            if ( lineInfo.Opcode.BranchOtherPagePenalty != 0 )
            {
              potentialCycles[1] += lineInfo.Opcode.BranchOtherPagePenalty;
              hadPotentialEntry = true;
            }
            if ( lineInfo.Opcode.BranchSamePagePenalty != 0 )
            {
              potentialCycles[2] += lineInfo.Opcode.BranchSamePagePenalty;
              hadPotentialEntry = true;
            }
          }
        }
      }

      if ( editSource.Selection.End.iLine != editSource.Selection.Start.iLine )
      {
        var selRange = new FastColoredTextBoxNS.Range( editSource, editSource.Selection.Start, editSource.Selection.End );
        selRange.Normalize();

        int   numLines = selRange.End.iLine - selRange.Start.iLine + 1;
        if ( ( selRange.Start.iLine >= 0 )
        &&   ( selRange.End.iLine < editSource.LinesCount ) )
        {
          string    selText = selRange.Text;
          if ( selText.EndsWith( System.Environment.NewLine ) )
          {
            --numLines;
          }
        }
        newInfo += ", " + editSource.SelectionLength.ToString() + " characters, " + numLines.ToString() + " lines selected";

      }
      else
      {
        newInfo += ", " + editSource.SelectionLength.ToString() + " characters selected";
      }
      if ( !hadPotentialEntry )
      {
        newInfo += ", " + numBytes + " bytes, " + numCycles + " cycles";
      }
      else
      {
        newInfo += ", " + numBytes + " bytes, " + numCycles + "(+";
        for ( int i = 0; i < 3; ++i )
        {
          if ( potentialCycles[i] > 0 )
          {
            if ( i + 1 < 3 )
            {
              newInfo += potentialCycles[i] + "/";
            }
            else
            {
              newInfo += potentialCycles[i];
            }
          }
        }
        newInfo += ") cycles";
      }
      Core.MainForm.statusEditorDetails.Text = newInfo;

      if ( !m_RecalcZone )
      {
        m_RecalcZone = true;
        m_DelayedEventTimer.Start();
      }
    }



    void UpdateSelectedZone()
    {
      if ( m_LastZoneUpdateLine == CurrentLineIndex )
      {
        return;
      }

      // adjust auto complete if zone might have changed
      FilterAutoComplete( CurrentLineIndex );

      m_LastZoneUpdateLine = CurrentLineIndex;

      

      string zone;
      string cheapLabelParent;

      FindZoneAtCaretPosition( out zone, out cheapLabelParent );

      var symbol = (SymbolInfo)comboZoneSelector.SelectedItem;
      if ( ( symbol != null )
      &&   ( zone != symbol.Zone ) )
      {
        for ( int i = 0; i < comboZoneSelector.Items.Count; ++i )
        {
          var symbol2 = (SymbolInfo)comboZoneSelector.Items[i];
          if ( symbol2.Zone == zone )
          {
            DoNotFollowZoneSelectors = true;
            comboZoneSelector.SelectedIndex = i;
            RefreshLocalSymbols();
            int localSymbolLine = -1;
            int bestLocalSymbolIndex = 0;

            for ( int j = 0; j < comboLocalLabelSelector.Items.Count; ++j )
            {
              var symbol3 = (SymbolInfo)( (ComboItem)comboLocalLabelSelector.Items[j] ).Tag;
              if ( ( symbol3.LocalLineIndex <= CurrentLineIndex )
              &&   ( symbol3.LocalLineIndex > localSymbolLine ) )
              {
                localSymbolLine = symbol3.LocalLineIndex;
                bestLocalSymbolIndex = j;
              }
            }
            if ( comboLocalLabelSelector.Items.Count > 0 )
            {
              comboLocalLabelSelector.SelectedIndex = bestLocalSymbolIndex;
            }
            DoNotFollowZoneSelectors = false;
            break;
          }
        }
      }
      else
      {
        DoNotFollowZoneSelectors = true;
        int localSymbolLine = -1;
        int bestLocalSymbolIndex = 0;

        for ( int j = 0; j < comboLocalLabelSelector.Items.Count; ++j )
        {
          var symbol3 = (SymbolInfo)( (ComboItem)comboLocalLabelSelector.Items[j] ).Tag;
          if ( ( symbol3.LocalLineIndex <= CurrentLineIndex )
          &&   ( symbol3.LocalLineIndex > localSymbolLine ) )
          {
            localSymbolLine = symbol3.LocalLineIndex;
            bestLocalSymbolIndex = j;
          }
        }
        if ( comboLocalLabelSelector.Items.Count > 0 )
        {
          comboLocalLabelSelector.SelectedIndex = bestLocalSymbolIndex;
        }
        DoNotFollowZoneSelectors = false;
      }
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
      //UpdateStatusInfo();
    }



    void editSource_MouseMove( object sender, System.Windows.Forms.MouseEventArgs e )
    {
      UpdateToolTip();
    }



    void editSource_MouseLeave( object sender, EventArgs e )
    {
      m_ToolTip.Hide( editSource );
    }



    void editSource_MouseEnter( object sender, EventArgs e )
    {
      m_ToolTip.Active = true;
    }



    void m_ToolTip_Popup( object sender, System.Windows.Forms.PopupEventArgs e )
    {
      //UpdateToolTip();
    }



    void UpdateToolTip()
    {
      m_LastTooltipPos = editSource.PointToClient( System.Windows.Forms.Cursor.Position );
      int position = editSource.PointToPosition( m_LastTooltipPos );

      int lineNumber = editSource.PositionToPlace( position ).iLine;
      string wordBelow = FindWordFromPosition( position, lineNumber );
      if ( wordBelow.Length == 0 )
      {
        m_ToolTip.Hide( editSource );
        return;
      }
      Types.ASM.FileInfo    debugFileInfo = Core.Navigating.DetermineASMFileInfo( DocumentInfo );
      if ( debugFileInfo == null )
      {
        m_ToolTip.Hide( editSource );
        return;
      }
      if ( !debugFileInfo.IsDocumentPart( DocumentInfo.FullPath ) )
      {
        debugFileInfo = Core.Navigating.DetermineLocalASMFileInfo( DocumentInfo );
        if ( !debugFileInfo.IsDocumentPart( DocumentInfo.FullPath ) )
        {
          m_ToolTip.Hide( editSource );
          return;
        }
      }

      string zone;
      string cheapLabelParent;

      debugFileInfo.FindZoneInfoFromDocumentLine( DocumentInfo.FullPath, lineNumber, out zone, out cheapLabelParent );

      debugFileInfo.FindGlobalLineIndex( lineNumber, DocumentInfo.FullPath, out int globalLineIndex );

      SymbolInfo tokenInfo = debugFileInfo.TokenInfoFromName( wordBelow, zone, cheapLabelParent, globalLineIndex );
      if ( ( tokenInfo != null )
      &&   ( tokenInfo.Type != SymbolInfo.Types.UNKNOWN ) )
      {
        string toolTipText = tokenInfo.Info;

        if ( tokenInfo.IsInteger() )
        {
          toolTipText += "$" + tokenInfo.AddressOrValue.ToString( "x4" ) + ", " + tokenInfo.AddressOrValue.ToString();

          byte    valueBelow = 0xcd;
          if ( ( Core.Debugging.Debugger != null )
          &&   ( Core.Debugging.Debugger.FetchValue( (int)tokenInfo.AddressOrValue, out valueBelow ) ) )
          {
            toolTipText += " ($" + valueBelow.ToString( "x2" ) + "/" + valueBelow.ToString() + ")";
          }
        }
        else
        {
          toolTipText += tokenInfo.ToString();
        }
        
        if ( m_LastTooltipText != toolTipText )
        {
          m_LastTooltipText = toolTipText;
          m_ToolTip.SetToolTip( editSource, toolTipText );
        }
        return;
      }
      var macroInfo = debugFileInfo.MacroFromName( wordBelow );
      if ( macroInfo != null )
      {
        string toolTipText = macroInfo.Name;

        foreach ( var parameter in macroInfo.ParameterNames )
        {
          toolTipText += " " + parameter;
        }
        if ( m_LastTooltipText != toolTipText )
        {
          m_LastTooltipText = toolTipText;
          m_ToolTip.SetToolTip( editSource, toolTipText );
        }
        return;
      }

      string  upperWord = wordBelow.ToUpper();
      if ( Parser.ASMFileInfo.AssemblerSettings.PseudoOps.ContainsKey( upperWord ) )
      {
        return;
      }
      m_ToolTip.Hide( editSource );
    }



    int AutoCompleteItemComparison( FastColoredTextBoxNS.AutocompleteItem i1, FastColoredTextBoxNS.AutocompleteItem i2 )
    {
      return String.Compare( i1.Text, i2.Text, true );
    }



    int StringCaseInsensitiveComparison( string s1, string s2 )
    {
      return String.Compare( s1, s2, true );
    }



    public override void OnKnownTokensChanged()
    {
      m_LastZoneUpdateLine = -1;

      string currentZone = "Global";
      if ( comboZoneSelector.SelectedIndex != -1 )
      {
        var symbol = (SymbolInfo)comboZoneSelector.SelectedItem;
        currentZone = symbol.Name;
      }

      comboZoneSelector.BeginUpdate();
      comboZoneSelector.Items.Clear();

      var globalSymbol = new SymbolInfo();
      globalSymbol.Name           = "Global";
      globalSymbol.LocalLineIndex = 0;
      globalSymbol.CharIndex      = 0;
      globalSymbol.DocumentFilename = DocumentFilename;
      comboZoneSelector.Items.Add( globalSymbol );

      GR.Collections.Set<string>    keySet = DocumentInfo.KnownTokens.GetUniqueKeys();
      var  uniqueZones = new GR.Collections.Map<string, SymbolInfo>();

      foreach ( string key in keySet )
      {
        List<SymbolInfo>    listValues = DocumentInfo.KnownTokens.GetValues( key, false );

        foreach ( SymbolInfo symbol in listValues )
        {
          if ( ( ( ( symbol.SourceInfo != null )
          &&       ( symbol.SourceInfo.FullPath == DocumentInfo.FullPath ) )
          ||     ( GR.Path.IsPathEqual( symbol.DocumentFilename, DocumentInfo.FullPath ) ) )
          &&   ( symbol.Type == SymbolInfo.Types.ZONE ) )
          {
            uniqueZones.Add( symbol.Zone, symbol );
          }
        }
      }

      // direct zones inside the file, zone from previous file (if applicable)
      foreach ( var zone in uniqueZones )
      {
        int itemIndex = comboZoneSelector.Items.Add( zone.Value );
        if ( zone.Key == currentZone )
        {
          comboZoneSelector.SelectedIndex = itemIndex;
        }
      }

      if ( comboZoneSelector.SelectedIndex == -1 )
      {
        comboZoneSelector.SelectedIndex = 0;
      }
      comboZoneSelector.EndUpdate();
    }



    public override void OnApplicationEvent( ApplicationEvent Event )
    {
      base.OnApplicationEvent( Event );

      switch ( Event.EventType )
      {
        case ApplicationEvent.Type.KEY_BINDINGS_MODIFIED:
          break;
        case ApplicationEvent.Type.DOCUMENT_ACTIVATED:
          UpdateStatusInfo();
          break;
        case ApplicationEvent.Type.SETTING_MODIFIED:
          if ( Event.OriginalValue == "ASMShowShortCutLabels" )
          {
            btnShowShortCutLabels.Image = Core.Settings.ASMShowShortCutLabels ? RetroDevStudio.Properties.Resources.flag_blue_on.ToBitmap() : RetroDevStudio.Properties.Resources.flag_blue_off.ToBitmap();
            RefreshLocalSymbols();
          }
          break;
      }
    }



    public override void OnKnownKeywordsChanged()
    {
      if ( Parser != null )
      {
        // adjust syntax coloring to use assembler type/cpu type (different opcodes per cpu)
        bool  modified = false;
        if ( m_SyntaxColoringCurrentKnownCPU != DocumentInfo.ASMFileInfo.Processor.Name )
        {
          m_SyntaxColoringCurrentKnownCPU = DocumentInfo.ASMFileInfo.Processor.Name;

          UpdateOpcodeSyntaxColoringSource();
          modified = true;
        }

        if ( ( DocumentInfo.ASMFileInfo.AssemblerSettings != null )
        &&   ( m_SyntaxColoringCurrentKnownAssembler != DocumentInfo.ASMFileInfo.AssemblerSettings.AssemblerType ) )
        {
          m_SyntaxColoringCurrentKnownAssembler = DocumentInfo.ASMFileInfo.AssemblerSettings.AssemblerType;

          UpdatePseudoOpSyntaxColoringSource();
          modified = true;
        }
        if ( modified )
        {
          //ResetAllStyles( editSource.Range );

          // only update pseudo op style regex
          var fullRange = editSource.Range;
          var styles = new Style[]
            {
              m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.PSEUDO_OP )],
              m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.CODE )]
            };
          fullRange.ClearStyle( styles );
          fullRange.SetStyle( m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.PSEUDO_OP )], m_TextRegExp[(int)Types.ColorableElement.PSEUDO_OP] );
          fullRange.SetStyle( m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.CODE )], m_TextRegExp[(int)Types.ColorableElement.CODE] );

          editSource.OnSyntaxHighlight( new FastColoredTextBoxNS.TextChangedEventArgs( editSource.Range ) );
        }
      }

      if ( InvokeRequired )
      {
        Invoke( new delSimpleEventHandler( UpdateKeywordsAndTokens ) );
      }
      else
      {
        UpdateKeywordsAndTokens();
      }
    }



    private void UpdateKeywordsAndTokens()
    {
      m_LastZoneUpdateLine = -1;
      RefreshLocalSymbols();
      RefreshAutoComplete();
    }



    private void UpdatePseudoOpSyntaxColoringSource()
    {
      var sb = new StringBuilder();

      if ( DocumentInfo.ASMFileInfo.AssemblerSettings == null )
      {
        DocumentInfo.ASMFileInfo.AssemblerSettings = new AssemblerSettings();
        DocumentInfo.ASMFileInfo.AssemblerSettings.SetAssemblerType( AssemblerType.C64_STUDIO );
      }

      sb.Append( @"(" );
      sb.Append( string.Join( "|", DocumentInfo.ASMFileInfo.AssemblerSettings.PseudoOps.Keys.ToArray() ) );
      sb.Append( @")\b" );

      m_TextRegExp[(int)Types.ColorableElement.PSEUDO_OP] = new System.Text.RegularExpressions.Regex( sb.ToString(), System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled );
    }



    private void UpdateOpcodeSyntaxColoringSource()
    {
      var sb = new StringBuilder();

      sb.Append( @"\b(" );
      sb.Append( string.Join( "|", DocumentInfo.ASMFileInfo.Processor.Opcodes.Keys.ToArray() ) );
      sb.Append( @")\b" );

      m_TextRegExp[(int)Types.ColorableElement.CODE] = new System.Text.RegularExpressions.Regex( sb.ToString(), System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled );
    }



    void RefreshLocalSymbols()
    {
      string currentZone = "";
      if ( comboZoneSelector.SelectedIndex != -1 )
      {
        var symbol = (SymbolInfo)comboZoneSelector.SelectedItem;
        currentZone = symbol.Name;
        if ( currentZone == "Global" )
        {
          currentZone = "";
        }
      }
      string currentLabel = "";
      if ( comboLocalLabelSelector.SelectedIndex != -1 )
      {
        var symbol = (SymbolInfo)( (ComboItem)comboLocalLabelSelector.SelectedItem ).Tag;
        currentLabel = symbol.Name;
      }

      comboLocalLabelSelector.BeginUpdate();
      comboLocalLabelSelector.Items.Clear();

      GR.Collections.Set<string> keySet = DocumentInfo.KnownTokens.GetUniqueKeys();
      string fullPath = DocumentInfo.FullPath;
      foreach ( string key in keySet )
      {
        List<SymbolInfo> listValues = DocumentInfo.KnownTokens.GetValues( key );
        foreach ( SymbolInfo symbol in listValues )
        {
          if ( ( symbol.Type == SymbolInfo.Types.CONSTANT_1 )
          ||   ( symbol.Type == SymbolInfo.Types.CONSTANT_2 )
          ||   ( symbol.Type == SymbolInfo.Types.LABEL ) )
          {
            if ( ( ( ( symbol.SourceInfo != null )
            &&       ( symbol.SourceInfo.FullPath == DocumentInfo.FullPath ) )
            ||     ( GR.Path.IsPathEqual( symbol.DocumentFilename, fullPath ) ) )
            &&   ( symbol.Zone == currentZone ) )
            {
              if ( ( symbol.Type == SymbolInfo.Types.LABEL )
              &&   ( symbol.Name.StartsWith( ASMFileParser.InternalLabelPrefix ) )
              &&   ( !Core.Settings.ASMShowShortCutLabels ) )
              {
                // skip
              }
              else
              {
                int itemIndex = comboLocalLabelSelector.Items.Add( new ComboItem( symbol.Name, symbol ) );
                if ( symbol.Name == currentLabel )
                {
                  comboLocalLabelSelector.SelectedIndex = itemIndex;
                }
              }
            }
          }
        }
      }
      comboLocalLabelSelector.EndUpdate();
      UpdateStatusInfo();
    }



    void FilterAutoComplete( int LineIndex )
    {
      if ( ( LineIndex < 0 )
      ||   ( LineIndex >= editSource.LinesCount ) )
      {
        return;
      }

      var newList = new List<FastColoredTextBoxNS.AutocompleteItem>();
      string curLine = editSource.Lines[LineIndex].TrimEnd( new char[] { '\r', '\n' } );
      int position = editSource.PlaceToPosition( editSource.Selection.Start );
      int posX = editSource.Selection.Start.iChar;

      // ugly quick hack, allow x and y as index fields for opcodes (e.g. sta $fff,x)
      var tokens = Parser.PrepareLineTokens( curLine, Core.Compiling.ParserASM.m_TextCodeMappingRaw );
      bool hadOpcodeFirst = false;
      if ( tokens != null )
      {
        // check for opcode or special command
        for ( int i = 0; i < tokens.Count; ++i )
        {
          if ( tokens[i].Type == RetroDevStudio.Types.TokenInfo.TokenType.COMMENT )
          {
            break;
          }
          if ( ( tokens[i].Type == RetroDevStudio.Types.TokenInfo.TokenType.OPCODE )
          ||   ( tokens[i].Type == RetroDevStudio.Types.TokenInfo.TokenType.OPCODE_FIXED_NON_ZP )
          ||   ( tokens[i].Type == RetroDevStudio.Types.TokenInfo.TokenType.OPCODE_FIXED_ZP ) )
          {
            // we've got an opcode
            hadOpcodeFirst = true;

            for ( int j = i + 1; j < tokens.Count; ++j )
            {
              if ( ( tokens[j].StartPos < posX )
              &&   ( tokens[j].Type == RetroDevStudio.Types.TokenInfo.TokenType.SEPARATOR )
              &&   ( tokens[j].Content == "," ) )

              {
                // comma is in the list, assume lda/sta with ,x or ,y
                var   newList2 = new List<FastColoredTextBoxNS.AutocompleteItem>();

                newList2.Add( new FastColoredTextBoxNS.AutocompleteItem( "x" ) { ToolTipTitle = tokens[i].Content + ",x", ToolTipText = tokens[i].Content + ",x" } );
                newList2.Add( new FastColoredTextBoxNS.AutocompleteItem( "y" ) { ToolTipTitle = tokens[i].Content + ",y", ToolTipText = tokens[i].Content + ",y" } );

                AutoComplete.Items.SetAutocompleteItems( newList2 );
                return;
              }
            }
          }
          if ( ( tokens[i].Type == RetroDevStudio.Types.TokenInfo.TokenType.SEPARATOR )
          &&   ( tokens[i].Content == "," ) )
          {
            if ( hadOpcodeFirst )
            {
              //editSource.AutoComplete.Cancel();
              return;
            }
          }

          if ( posX < tokens[i].StartPos + tokens[i].Length )
          {
            break;
          }
        }
      }

      // common filter, filter by given zone, allow local symbols without zone prefix

      int lineIndex = LineIndex;
      string wordBelow = FindWordFromPosition( CurrentPosition() - 1, lineIndex );
      string zone;
      string cheapLabelParent;

      if ( wordBelow.StartsWith( "$" ) )
      {
        AutoComplete.MinFragmentLength = 1;
        AutoComplete.AutoSize = true;
        AutoComplete.Items.SetAutocompleteItems( new List<string>() );
        return;
      }

      FindZoneFromLine( lineIndex, out zone, out cheapLabelParent );
      /*
      if ( wordBelow.Length <= 0 )
      {
        return;
      }*/

      newList = new List<FastColoredTextBoxNS.AutocompleteItem>();

      var uniqueKeys = new GR.Collections.Set<string>();

      //List<string>    newList = new List<string>();
      foreach ( var entry in DocumentInfo.KnownKeywords )
      {
        if ( entry.Token.StartsWith( RetroDevStudio.Parser.ASMFileParser.InternalLabelPrefix ) )
        {
          continue;
        }

        if ( entry.Symbol != null )
        {
          uniqueKeys.Add( entry.Symbol.Name );
        }

        string    toolTipText = entry.ToolTipText;
        if ( ( entry.Symbol != null )
        &&   ( !string.IsNullOrEmpty( entry.Symbol.Info ) ) )
        {
          toolTipText = entry.Symbol.Info.Trim() + "\n" + toolTipText;
        }
        if ( entry.Symbol != null )
        {
          toolTipText += "$" + entry.Symbol.AddressOrValue.ToString( "X4" ) + "," + entry.Symbol.AddressOrValue.ToString();
        }

        // always add common entries
        newList.Add( new FastColoredTextBoxNS.AutocompleteItem( entry.Token )
        {
          ToolTipTitle = entry.ToolTipTitle,
          ToolTipText = toolTipText
        } );
        // special case local labels
        if ( entry.Token.StartsWith( zone + ".", StringComparison.CurrentCultureIgnoreCase ) )
        {
          newList.Add( new FastColoredTextBoxNS.AutocompleteItem( entry.Token.Substring( zone.Length ) )
          {
            ToolTipTitle = entry.ToolTipTitle,
            ToolTipText = toolTipText
          } );
        }
      }
      foreach ( var entry in DocumentInfo.KnownTokens )
      {
        if ( entry.Key.StartsWith( RetroDevStudio.Parser.ASMFileParser.InternalLabelPrefix ) )
        {
          continue;
        }

        if ( uniqueKeys.Contains( entry.Key ) )
        {
          // do not override entries!
          continue;
        }

        string    toolTipText =  "$" + entry.Value.AddressOrValue.ToString( "X4" ) + "," + entry.Value.AddressOrValue.ToString();

        // always add common entries
        newList.Add( new FastColoredTextBoxNS.AutocompleteItem( entry.Key ) { ToolTipTitle = entry.Key, ToolTipText = toolTipText } );
        // special case local labels
        if ( entry.Key.StartsWith( zone + ".", StringComparison.CurrentCultureIgnoreCase ) )
        {
          newList.Add( new FastColoredTextBoxNS.AutocompleteItem( entry.Key.Substring( zone.Length ) ) { ToolTipTitle = entry.Key.Substring( zone.Length ), ToolTipText = toolTipText } );
        }
      }

      // add macros
      string  macroPrefix = "";
      if ( Parser.ASMFileInfo.AssemblerSettings.MacroFunctionCallPrefix.Count > 0 )
      {
        macroPrefix = Parser.ASMFileInfo.AssemblerSettings.MacroFunctionCallPrefix[0];
      }
      foreach ( var entry in DocumentInfo.ASMFileInfo.Macros )
      {
        if ( uniqueKeys.Contains( entry.Key ) )
        {
          // do not override entries!
          continue;
        }
        // list parameters
        string    toolTipText =  entry.Key;

        foreach ( var parameter in entry.Value.ParameterNames )
        {
          toolTipText += " " + parameter;
        }

        // always add common entries
        newList.Add( new FastColoredTextBoxNS.AutocompleteItem( macroPrefix + entry.Key ) { ToolTipTitle = macroPrefix + entry.Key, ToolTipText = toolTipText } );
      }
      if ( newList.Count > 0 )
      {
        newList.Sort( AutoCompleteItemComparison );

        // remove duplicates
        Int32 index = 0;
        while ( index < newList.Count - 1 )
        {
          if ( newList[index].Text == newList[index + 1].Text )
          {
            newList.RemoveAt( index );
          }
          else
          {
            index++;
          }
        }
        if ( newList.Count == 1 )
        {
          if ( ( String.Compare( newList[0].Text, wordBelow, StringComparison.CurrentCultureIgnoreCase ) == 0 )
          ||   ( String.Compare( newList[0].Text, zone + "." + wordBelow.Substring( 1 ), StringComparison.CurrentCultureIgnoreCase ) == 0 ) )
          {
            // only have the correct entry
            return;
          }
        }
        // TODO - sort newList by relevance


        AutoComplete.MinFragmentLength = 1;
        AutoComplete.AutoSize = true;
        AutoComplete.Items.SetAutocompleteItems( newList );
      }
    }



    void ShowAutoComplete()
    {
      if ( Core.Settings.ASMShowAutoComplete )
      {
        AutoComplete.Show( true );
      }
    }



    void RefreshAutoComplete()
    {
      if ( editSource.InvokeRequired )
      {
        editSource.Invoke( new RetroDevStudio.MainForm.ParameterLessCallback( RefreshAutoComplete ) );
        return;
      }

      var   newList = new List<FastColoredTextBoxNS.AutocompleteItem>();

      if ( Parser.ASMFileInfo.AssemblerSettings != null )
      {
        // add pseudo ops
        foreach ( var pseudoOp in Parser.ASMFileInfo.AssemblerSettings.PseudoOps )
        {
          newList.Add( new AutocompleteItem( pseudoOp.Key ) { ToolTipTitle = pseudoOp.Key, ToolTipText = "Pseudo OP" } );
        }
      }
      foreach ( var entry in DocumentInfo.KnownKeywords )
      {
        if ( entry.Token.StartsWith( RetroDevStudio.Parser.ASMFileParser.InternalLabelPrefix ) )
        {
          // skip internal labels in autocomplete
          continue;
        }
        newList.Add( new FastColoredTextBoxNS.AutocompleteItem( entry.Token ) { ToolTipTitle = entry.ToolTipTitle, ToolTipText = entry.ToolTipText } );
      }
      GR.Collections.Set<string>    uniqueKeys = DocumentInfo.KnownTokens.GetUniqueKeys();
      foreach ( string entry in uniqueKeys )
      {
        if ( entry.StartsWith( RetroDevStudio.Parser.ASMFileParser.InternalLabelPrefix ) )
        {
          // skip internal labels in autocomplete
          continue;
        }

        newList.Add( new FastColoredTextBoxNS.AutocompleteItem( entry ) { ToolTipTitle = entry, ToolTipText = "sowas" } );
      }
      if ( newList.Count > 0 )
      {
        newList.Sort( AutoCompleteItemComparison );

        // remove duplicates
        Int32 index = 0;
        while ( index < newList.Count - 1 )
        {
          if ( newList[index].Text == newList[index + 1].Text )
          {
            newList.RemoveAt( index );
          }
          else
          {
            index++;
          }
        }
        AutoComplete.MinFragmentLength = 2;
        AutoComplete.AutoSize = true;
        AutoComplete.Items.SetAutocompleteItems( newList );
      }
      m_CurrentAutoCompleteSource = newList;
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
        }
      }

      // bookmarks
      addBookmarkHereToolStripMenuItem.Enabled = !editSource.Bookmarks.Any( bm => bm.LineIndex == m_ContextMenuLineIndex );
      removeBookmarkToolStripMenuItem.Enabled = editSource.Bookmarks.Any( bm => bm.LineIndex == m_ContextMenuLineIndex );
      removeAllBookmarksOfThisFileToolStripMenuItem.Enabled = editSource.Bookmarks.Any();

      if ( ( Core.MainForm.AppState == Types.StudioState.DEBUGGING_BROKEN )
      ||   ( Core.MainForm.AppState == Types.StudioState.NORMAL ) )
      {
        addToWatchToolStripMenuItem.Visible = true;
        addBreakpointToolStripMenuItem.Visible = true;
        addDataBreakpointToolStripMenuItem.Visible = true;
        toolStripSeparator2.Visible = true;
      }
      else
      {
        addToWatchToolStripMenuItem.Visible = false;
        addBreakpointToolStripMenuItem.Visible = false;
        addDataBreakpointToolStripMenuItem.Visible = false;
        toolStripSeparator2.Visible = false;
      }
      if ( editSource.SelectionLength > 0 )
      {
        commentSelectionToolStripMenuItem.Enabled = true;
        uncommentSelectionToolStripMenuItem.Enabled = true;
      }
      else
      {
        commentSelectionToolStripMenuItem.Enabled = false;
        uncommentSelectionToolStripMenuItem.Enabled = false;
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
      if ( ( lineBelow.StartsWith( "!SOURCE" ) )
      ||   ( lineBelow.StartsWith( "!SRC" ) ) )
      {
        string    fileName = editSource.Lines[m_ContextMenuLineIndex].Trim().Substring( 4 ).Trim();
        if ( lineBelow.StartsWith( "!SOURCE" ) )
        {
          fileName = editSource.Lines[m_ContextMenuLineIndex].Trim().Substring( 7 ).Trim();
        }

        if ( ( fileName.Length > 2 )
        &&   ( fileName.StartsWith( "\"" ) )
        &&   ( fileName.EndsWith( "\"" ) ) )
        {
          showOpenFile = true;
          openFileToolStripMenuItem.Text = "Open \"" + fileName.Substring( 1, fileName.Length - 2 ) + "\"";
          m_FilenameToOpen = fileName.Substring( 1, fileName.Length - 2 );
        }
        if ( ( fileName.Length > 2 )
        &&   ( fileName.StartsWith( "<" ) )
        &&   ( fileName.EndsWith( ">" ) ) )
        {
          showOpenFile = true;
          openFileToolStripMenuItem.Text = "Open <" + fileName.Substring( 1, fileName.Length - 2 ) + ">";
          m_FilenameToOpen = fileName.Substring( 1, fileName.Length - 2 );

          m_FilenameToOpen = DetermineFullLibraryFilePath( m_FilenameToOpen );
        }
      }
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
        if ( !System.IO.Path.IsPathRooted( libFile ) )
        {
#if DEBUG
          fullBasePath = System.IO.Path.GetFullPath( "../../" + libFile );
#else
          fullBasePath = System.IO.Path.GetFullPath( libFile );
#endif
        }
        if ( System.IO.File.Exists( System.IO.Path.Combine( fullBasePath, SubFilename ) ) )
        {
          return System.IO.Path.Combine( fullBasePath, SubFilename );
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
        System.Windows.Forms.MessageBox.Show( "Could not load ASM file " + DocumentInfo.FullPath + ".\r\n" + ex.Message, "Could not load file" );
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
      m_InsertingText = false;


      SetUnmodified();
      if ( string.IsNullOrEmpty( m_FileWatcher.Path ) )
      {
        SetupWatcher();
        EnableFileWatcher();
      }
      return true;
    }



    private string FilterString( string Source )
    {
      return Source.Substring( 0, Source.Length - 1 );
    }



    protected override bool QueryFilename( out string Filename )
    {
      Filename = "";


      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Save ASM File as";
      saveDlg.Filter = FilterString( Types.Constants.FILEFILTER_ASM + Types.Constants.FILEFILTER_ALL );
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

          if ( Core.Settings.StripTrailingSpaces )
          {
            editSource.StripTrailingSpaces();
          }
        }
        System.IO.File.WriteAllText( FullPath, GetContent(), Core.Settings.SourceFileEncoding );

        editSource.TextSource.ClearIsChanged();
        editSource.Invalidate();
      }
      catch ( System.IO.IOException ex )
      {
        System.Windows.Forms.MessageBox.Show( "Could not save file " + FullPath + ".\r\n" + ex.ToString(), "Could not save file" );
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
      //if ( DocumentInfo.FullPath.EndsWith( "fighter.asm" ) )
      //{
        //Debug.Log( "StoreFoldedBlocks" );
      //}
      DocumentInfo.CollapsedFoldingBlocks.Clear();

      foreach ( var block in editSource.FoldedBlocks )
      {
        for ( int iLine = 0; iLine < editSource.LinesCount; iLine++ )
        {
          if ( editSource.TextSource[iLine].UniqueId == block.Key )
          {
            DocumentInfo.CollapsedFoldingBlocks.Add( iLine );
            //Debug.Log( "Store for " + DocumentInfo.FullPath + ", line " + iLine );
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
        //return editSource.Caret.LineNumber;
        return editSource.Selection.Start.iLine;
      }
    }



    public string FindWordFromPosition( int Position, int LineIndex )
    {
      int dummy;

      return FindWordFromPosition( Position, LineIndex, out dummy );
    }



    public string FindWordFromPosition( int Position, int LineIndex, out int TrueStartCharInLine )
    {
      int position = Position;
      string currentLine = editSource.ReTabifyLine( editSource.Lines[LineIndex], editSource.TabLength );
      // TODO - bloat Tabs again
      TrueStartCharInLine = position - editSource.PlaceToPosition( new FastColoredTextBoxNS.Place( 0, LineIndex ) );
      if ( TrueStartCharInLine < 0 )
      {
        // should not happen!
        return "";
      }

      string wordBelow = "";

      // TODO - use assembler settings!!

      // add $ so it doesn't show when starting a hex number
      string tokenAllowedChars = ".@ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_$";
      if ( DocumentInfo.Element != null )
      {
        // TODO - should not be defined here
        switch ( DocumentInfo.Element.AssemblerType )
        {
          case Types.AssemblerType.PDS:
          case Types.AssemblerType.AUTO:
            tokenAllowedChars = "!@ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß.$";
            break;
          case Types.AssemblerType.DASM:
            tokenAllowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß.$";
            break;
          default:
            tokenAllowedChars = ".@ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_$";
            break;
        }
      }

      int posX = TrueStartCharInLine;
      int left = posX;
      if ( left >= currentLine.Length )
      {
        left = currentLine.Length - 1;
        return "";
      }
      while ( left >= 0 )
      {
        char curChar = currentLine[left];
        if ( tokenAllowedChars.IndexOf( curChar ) == -1 )
        {
          break;
        }
        wordBelow = curChar + wordBelow;
        --left;
      }

      TrueStartCharInLine = left;

      int right = posX + 1;
      while ( right < currentLine.Length )
      {
        char curChar = currentLine[right];
        if ( tokenAllowedChars.IndexOf( curChar ) == -1 )
        {
          break;
        }
        wordBelow += curChar;
        ++right;
      }

      if ( ( left >= 0 )
      &&   ( DocumentInfo.Element != null )
      &&   ( DocumentInfo.Element.AssemblerType == Types.AssemblerType.C64_STUDIO )
      &&   ( currentLine[left] == '!' ) )
      {
        // a pseudo op?
        string    potentialPseudoOp = '!' + wordBelow;

        if ( ( Parser.ASMFileInfo.AssemblerSettings != null )
        &&   ( Parser.ASMFileInfo.AssemblerSettings.PseudoOps.ContainsKey( potentialPseudoOp.ToUpper() ) ) )
        {
          wordBelow = potentialPseudoOp;
        }
      }
      return wordBelow;
    }



    private void addToWatchToolStripMenuItem_Click( object sender, EventArgs e )
    {
      int     lineIndex = m_ContextMenuLineIndex;
      string  wordBelow = FindWordFromPosition( m_ContextMenuPosition, m_ContextMenuLineIndex );
      bool    indexedX = false;
      bool    indexedY = false;

      if ( ( editSource.SelectionLength > 0 )
      &&   ( editSource.Selection.Start.iLine == lineIndex )
      &&   ( editSource.Selection.End.iLine == lineIndex ) )
      {
        // there is a selection on this line
        string watchedSelection = editSource.Selection.Text;

        // allow for single label; label,x; label,y; (label),y; (label),x
        List<Types.TokenInfo> tokens = Core.Compiling.ParserASM.ParseTokenInfo( watchedSelection, 0, watchedSelection.Length, Core.Compiling.ParserASM.m_TextCodeMappingRaw );

        if ( tokens.Count != 0 )
        {
          // single label
          if ( tokens.Count == 1 )
          {
            wordBelow = tokens[0].Content;
          }
          else if ( tokens.Count == 3 )
          {
            // label,x  or label,y
            if ( ( tokens[1].Content == "," )
            &&   ( tokens[2].Content.ToUpper() == "X" ) )
            {
              indexedX = true;
              wordBelow = tokens[0].Content;
            }
            else if ( ( tokens[1].Content == "," )
            &&        ( tokens[2].Content.ToUpper() == "Y" ) )
            {
              indexedY = true;
              wordBelow = tokens[0].Content;
            }
            else
            {
              System.Windows.Forms.MessageBox.Show( "Could not determine watch item from selection" );
              return;
            }
          }
          else
          {
            System.Windows.Forms.MessageBox.Show( "Could not determine watch item from selection" );
            return;
          }
        }
      }

      Types.ASM.FileInfo debugFileInfo = Core.Navigating.DetermineASMFileInfo( DocumentInfo );
      if ( debugFileInfo == null )
      {
        System.Windows.Forms.MessageBox.Show( "Could not determine item address of " + wordBelow );
        return;
      }

      string zone;
      string cheapLabelParent;
      
      debugFileInfo.FindZoneInfoFromDocumentLine( DocumentInfo.FullPath, lineIndex, out zone, out cheapLabelParent );

      // TODO - determine if known label/var
      long    result = -1;
      int     bytesGiven = 0;
      bool    failed = true;

      if ( Core.Compiling.ParserASM.ParseLiteralValue( wordBelow, out failed, out result, out bytesGiven ) )
      {
        WatchEntry entry    = new WatchEntry();
        entry.Name          = wordBelow;
        entry.SizeInBytes   = 1;
        entry.Type          = WatchEntry.DisplayType.HEX;
        entry.Address       = (int)result;
        entry.IndexedX      = indexedX;
        entry.IndexedY      = indexedY;
        entry.DisplayMemory = true;
        entry.LiteralValue  = true;

        Debug.Log( "Address for " + wordBelow + " determined as " + entry.Address );

        Core.MainForm.AddWatchEntry( entry );
        return;
      }

      SymbolInfo tokenInfo = debugFileInfo.TokenInfoFromName( wordBelow, zone, cheapLabelParent );
      if ( tokenInfo != null )
      {
        WatchEntry entry = new WatchEntry();
        entry.Name        = wordBelow;
        entry.SizeInBytes = 1;
        entry.Type        = WatchEntry.DisplayType.HEX;
        entry.Address     = (int)tokenInfo.AddressOrValue;
        entry.IndexedX    = indexedX;
        entry.IndexedY    = indexedY;
        entry.DisplayMemory = true;

        Debug.Log( "Address for " + wordBelow + " determined as " + entry.Address );

        Core.MainForm.AddWatchEntry( entry );
      }
      else
      {
        System.Windows.Forms.MessageBox.Show( "Could not determine item address of " + wordBelow );
      }
    }



    private void addShowMemoryToolStripMenuItem_Click( object sender, EventArgs e )
    {
      string wordBelow = FindWordFromPosition( m_ContextMenuPosition, m_ContextMenuLineIndex );
      string zone;
      string cheapLabelParent;

      FindZoneFromLine( m_ContextMenuLineIndex, out zone, out cheapLabelParent );

      //Core.MainForm.EnsureFileIsParsed();
      SymbolInfo tokenInfo = DocumentInfo.ASMFileInfo.TokenInfoFromName( wordBelow, zone, cheapLabelParent );
      if ( ( tokenInfo != null )
      &&   ( tokenInfo.Type != SymbolInfo.Types.UNKNOWN ) )
      {
        if ( ( tokenInfo.AddressOrValue >= 0 )
        &&   ( tokenInfo.AddressOrValue <= 65535 ) )
        {
          int line = (int)tokenInfo.AddressOrValue / Core.MainForm.m_DebugMemory.hexView.BytesPerLine;
          Core.MainForm.m_DebugMemory.Show();
          Core.MainForm.m_DebugMemory.hexView.PerformScrollToLine( line );
          Core.MainForm.m_DebugMemory.RefreshViewScroller();
        }
        else
        {
          System.Windows.Forms.MessageBox.Show( "Could not determine item address of " + wordBelow );
        }
      }
    }



    private bool FindZoneFromLine( int LineIndex, out string Zone, out string CheapLabelParent )
    {
      Zone = "";
      CheapLabelParent = "";

      Types.ASM.FileInfo debugFileInfo = Core.Navigating.DetermineASMFileInfo( DocumentInfo );
      if ( debugFileInfo == null )
      {
        return false;
      }
      if ( !debugFileInfo.IsDocumentPart( DocumentInfo.FullPath ) )
      {
        debugFileInfo = DocumentInfo.ASMFileInfo;
      }

      while ( LineIndex >= 0 )
      {
        int     globalLineIndex = -1;
        if ( debugFileInfo.FindGlobalLineIndex( LineIndex, DocumentInfo.FullPath, out globalLineIndex ) )
        {
          if ( debugFileInfo.LineInfo.ContainsKey( globalLineIndex ) )
          {
            Types.ASM.LineInfo lineInfo = debugFileInfo.LineInfo[globalLineIndex];

            Zone              = lineInfo.Zone;
            CheapLabelParent  = lineInfo.CheapLabelZone;
            break;
          }
        }
        else
        {
          return false;
        }
        --LineIndex;
      }
      return true;
    }



    private string FindLocalLabelFromLine( int LineIndex )
    {
      Types.ASM.FileInfo debugFileInfo = Core.Navigating.DetermineASMFileInfo( DocumentInfo );
      if ( debugFileInfo == null )
      {
        return "";
      }

      int closestLine = -1;
      SymbolInfo possibleLabel = null;
      int globalLineIndex = -1;

      if ( !debugFileInfo.FindGlobalLineIndex( LineIndex, DocumentInfo.FullPath, out globalLineIndex ) )
      {
        return "";
      }

      foreach ( SymbolInfo symbol in debugFileInfo.Labels.Values )
      {
        if ( ( symbol.LineIndex <= globalLineIndex )
        &&   ( symbol.LineIndex > closestLine ) )
        {
          closestLine = symbol.LineIndex;
          possibleLabel = symbol;
        }
      }
      if ( possibleLabel == null )
      {
        return "";
      }
      return possibleLabel.Name;
    }



    private void gotoDeclarationToolStripMenuItem_Click( object sender, EventArgs e )
    {
      GoToDeclaration();
    }



    private void GoToDeclaration()
    {
      string wordBelow = FindWordFromPosition( m_ContextMenuPosition, m_ContextMenuLineIndex );
      string zone;
      string cheapLabelParent;

      Core.Navigating.VisitedLine( DocumentInfo, m_ContextMenuLineIndex );

      FindZoneFromLine( m_ContextMenuLineIndex, out zone, out cheapLabelParent );

      Core.Navigating.GotoDeclaration( DocumentInfo, wordBelow, zone, cheapLabelParent );
      CenterOnCaret();
    }



    private int CurrentPosition()
    {
      //return editSource.Caret.Position;
      return editSource.PlaceToPosition( editSource.Selection.Start );
    }



    public string FindWordAtCaretPosition()
    {
      return FindWordFromPosition( CurrentPosition(), CurrentLineIndex );
    }



    public bool FindZoneAtCaretPosition( out string Zone, out string CheapLabelParent )
    {
      return FindZoneFromLine( CurrentLineIndex, out Zone, out CheapLabelParent );
    }



    public string FindLocalLabelAtCaretPosition()
    {
      return FindLocalLabelFromLine( CurrentLineIndex );
    }



    private void openFileToolStripMenuItem_Click( object sender, EventArgs e )
    {
      if ( DocumentInfo == null )
      {
        return;
      }
      string docBasePath = GR.Path.RemoveFileSpec( DocumentInfo.FullPath );
      if ( ( string.IsNullOrEmpty( docBasePath ) )
      &&   ( DocumentInfo.Project != null ) )
      {
        docBasePath = DocumentInfo.Project.Settings.BasePath;
      }
      string            fullPath = m_FilenameToOpen;

      if ( !System.IO.Path.IsPathRooted( fullPath ) )
      {
        fullPath = GR.Path.Append( docBasePath, fullPath );
      }
      if ( DocumentInfo.Project == null )
      {
        Core.MainForm.OpenFile( fullPath );
        return;
      }
      ProjectElement    element = DocumentInfo.Project.GetElementByFilename( fullPath );
      if ( element != null )
      {
        DocumentInfo.Project.ShowDocument( element );
      }
      else
      {
        var newDoc = Core.MainForm.OpenFile( fullPath );
        if ( newDoc != null )
        {
          Core.Compiling.PreparseDocument( newDoc );
        }
      }
    }



    private void showAddressToolStripMenuItem_Click( object sender, EventArgs e )
    {
      string wordBelow = FindWordFromPosition( m_ContextMenuPosition, m_ContextMenuLineIndex );
      string zone;
      string cheapLabelParent;
      
      FindZoneFromLine( m_ContextMenuLineIndex, out zone, out cheapLabelParent );

      Core.MainForm.EnsureFileIsParsed();
      SymbolInfo tokenInfo = DocumentInfo.ASMFileInfo.TokenInfoFromName( wordBelow, zone, cheapLabelParent );
      if ( ( tokenInfo != null )
      &&   ( tokenInfo.Type != SymbolInfo.Types.UNKNOWN ) )
      {
        Core.AddToOutput( "Value of " + wordBelow + ": $" + tokenInfo.AddressOrValue.ToString( "x4" ) + ", " + tokenInfo.AddressOrValue.ToString() + Environment.NewLine );
      }
      else
      {
        Core.AddToOutput( "Could not determine value for " + wordBelow + Environment.NewLine );
      }
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
    }



    public override void Paste()
    {
      m_InsertingText = true;
      editSource.Paste();
      m_InsertingText = false;
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
    }



    public override void RefreshDisplayOptions()
    {
      base.RefreshDisplayOptions();

      BackColor = Core.Theming.DarkenColor( GR.Color.Helper.FromARGB( Core.Settings.BGColor( ColorableElement.BACKGROUND_CONTROL ) ) );

      // Font
      editSource.Font = new System.Drawing.Font( Core.Settings.SourceFontFamily, Core.Settings.SourceFontSize, Core.Settings.SourceFontStyle );

      // Colors
      editSource.Language = FastColoredTextBoxNS.Language.Custom;//.VB;//FastColoredTextBoxNS.Language.Custom;
      editSource.CommentPrefix = ";";

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
      ApplySyntaxColoring( Types.ColorableElement.LABEL );
      ApplySyntaxColoring( Types.ColorableElement.CODE );
      ApplySyntaxColoring( Types.ColorableElement.OPERATOR );
      ApplySyntaxColoring( Types.ColorableElement.CURRENT_DEBUG_LINE );
      ApplySyntaxColoring( Types.ColorableElement.NONE );
      ApplySyntaxColoring( Types.ColorableElement.PSEUDO_OP );
      ApplySyntaxColoring( Types.ColorableElement.HIGHLIGHTED_SEARCH_RESULTS );
      ApplySyntaxColoring( Types.ColorableElement.ERROR_UNDERLINE );

      editSource.AllowTabs            = true; //Core.Settings.AllowTabs;
      editSource.ConvertTabsToSpaces  = Core.Settings.TabConvertToSpaces;
      editSource.TabLength            = Core.Settings.TabSize;
      editSource.ShowLineNumbers      = !Core.Settings.ASMHideLineNumbers;
      editSource.PreferredLineWidth   = Core.Settings.ASMShowMaxLineLengthIndicatorLength;

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
      m_BreakpointOffset = 0;
      m_CycleOffset = -1;
      m_ByteSizeOffset = -1;
      m_AddressOffset = -1;

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

      AutoComplete.Enabled = Core.Settings.ASMShowAutoComplete;

      miniMap.ForeColor = GR.Color.Helper.FromARGB( 0xffff0000 );
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
        case Types.Function.COMMENT_SELECTION:
          CommentSelection();
          return true;
        case Types.Function.UNCOMMENT_SELECTION:
          UncommentSelection();
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
        case Function.FIND_ALL_REFERENCES:
          FindAllReferences( editSource.PlaceToPosition( editSource.Selection.Start ), editSource.Selection.Start.iLine );
          return true;
        case Function.RENAME_ALL_REFERENCES:
          RenameAllReferences( editSource.PlaceToPosition( editSource.Selection.Start ), editSource.Selection.Start.iLine );
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



    private void cutToolStripMenuItem_Click( object sender, EventArgs e )
    {
      editSource.Cut();
    }



    private void pasteToolStripMenuItem_Click( object sender, EventArgs e )
    {
      editSource.Paste();
    }



    private void comboZoneSelector_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( DoNotFollowZoneSelectors )
      {
        return;
      }

      SymbolInfo symbol = (SymbolInfo)comboZoneSelector.SelectedItem;

      SetCursorToLine( symbol.LocalLineIndex, symbol.CharIndex, true );

      RefreshLocalSymbols();
    }



    private void comboLocalLabelSelector_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( DoNotFollowZoneSelectors )
      {
        return;
      }
      SymbolInfo symbol = (SymbolInfo)( (ComboItem)comboLocalLabelSelector.SelectedItem ).Tag;

      SetCursorToLine( symbol.LocalLineIndex, symbol.CharIndex > -1 ? symbol.CharIndex : 0, true );
    }



    private void commentSelectionToolStripMenuItem_Click( object sender, EventArgs e )
    {
      CommentSelection();
    }



    private void CommentSelection()
    {
      if ( editSource.Selection.IsEmpty )
      {
        return;
      }
      editSource.InsertLinePrefix( ";" );
      SetModified();
    }



    private void uncommentSelectionToolStripMenuItem_Click( object sender, EventArgs e )
    {
      UncommentSelection();
    }



    private void UncommentSelection()
    {
      if ( editSource.SelectionLength == 0 )
      {
        return;
      }
      editSource.RemoveLinePrefix( ";" );
      SetModified();
    }



    private void helpToolStripMenuItem_Click( object sender, EventArgs e )
    {
      string wordBelow = FindWordFromPosition( m_ContextMenuPosition, m_ContextMenuLineIndex );

      Core.MainForm.CallHelp( wordBelow, DocumentInfo );
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



    private void editSource_MouseClick( object sender, MouseEventArgs e )
    {
      if ( e.X < editSource.LeftIndentLine - 7 )
      {
        ToggleBreakpointOnEvent( editSource.PointToPlace( e.Location ).iLine );
      }
    }



    Types.ASM.LineInfo FetchLineInfo( int LineIndex )
    {
      if ( ( LineIndex < 0 )
      ||   ( LineIndex >= m_LineInfos.Count ) )
      {
        return null;
      }
      return m_LineInfos[LineIndex];
    }



    private void editSource_PaintLine( object sender, FastColoredTextBoxNS.PaintLineEventArgs e )
    {
      // display breakpoints
      var textBrush = new SolidBrush( editSource.ForeColor );

      if ( m_BreakPoints.ContainsKey( e.LineIndex ) )
      {
        e.Graphics.DrawImage( RetroDevStudio.Properties.Resources.breakpoint, m_BreakpointOffset, e.LineRect.Top );
      }

      var lineInfo = FetchLineInfo( e.LineIndex );
      if ( lineInfo != null )
      {
        if ( m_CycleOffset != -1 )
        {
          if ( lineInfo.Opcode != null )
          {
            if ( lineInfo.Opcode.NumPenaltyCycles > 0 )
            {
              e.Graphics.DrawString( lineInfo.Opcode.NumCycles.ToString() + "+" + lineInfo.Opcode.NumPenaltyCycles.ToString(), editSource.Font, textBrush, m_CycleOffset, e.LineRect.Top );
            }
            else
            {
              e.Graphics.DrawString( lineInfo.Opcode.NumCycles.ToString(), editSource.Font, textBrush, m_CycleOffset, e.LineRect.Top );
            }
          }
        }
        if ( m_ByteSizeOffset != -1 )
        {
          e.Graphics.DrawString( lineInfo.NumBytes.ToString(), editSource.Font, textBrush, m_ByteSizeOffset, e.LineRect.Top );
        }
        if ( m_AddressOffset != -1 )
        {
          if ( lineInfo.AddressStart != -1 )
          {
            e.Graphics.DrawString( "$" + lineInfo.AddressStart.ToString( "X4" ), editSource.Font, textBrush, m_AddressOffset, e.LineRect.Top );
          }
          else
          {
            e.Graphics.DrawString( "????", editSource.Font, textBrush, m_AddressOffset, e.LineRect.Top );
          }
        }
      }
    }



    internal void SetLineInfos( Types.ASM.FileInfo FileInfo )
    {
      try
      {
        GR.Collections.Set<int>   setLines = new GR.Collections.Set<int>();
        string                    myFullPath = DocumentInfo.FullPath;

        foreach ( var lineInfo in FileInfo.LineInfo )
        {
          FileInfo.FindTrueLineSource( lineInfo.Key, out string filename, out int localLineIndex );

          // Windows filenames don't care for case (as the gods intended)
          if ( string.Compare( filename, myFullPath, true ) == 0 )
          {
            var newInfo = lineInfo.Value;

            if ( !setLines.ContainsValue( localLineIndex ) )
            {
              while ( localLineIndex >= m_LineInfos.Count )
              {
                m_LineInfos.Add( new Types.ASM.LineInfo() );
              }
              m_LineInfos[localLineIndex] = new Types.ASM.LineInfo()
              {
                AddressStart = newInfo.AddressStart,
                NumBytes = newInfo.NumBytes,
                Line = newInfo.Line,
                LineIndex = localLineIndex,
                Zone = newInfo.Zone,
                Opcode = newInfo.Opcode
              };

              setLines.Add( localLineIndex );
            }
            else
            {
              while ( localLineIndex >= m_LineInfos.Count )
              {
                m_LineInfos.Add( new Types.ASM.LineInfo() );
              }

              // accumulate values!
              var curInfo = m_LineInfos[localLineIndex];

              curInfo.NumBytes += newInfo.NumBytes;

              // TODO - cycles!
              curInfo.HasCollapsedContent = true;
            }
          }
        }
      }
      catch ( InvalidOperationException )
      {
        // HACK - this one may happen if another thread parses and we try to use the collection at the same time
      }
    }



    internal DateTime LastChange
    {
      get
      {
        return m_LastChange;
      }
    }



    private void AddDataBreakPoint( bool TriggerOnExec, bool TriggerOnRead, bool TriggerOnWrite )
    {
      int     lineIndex = m_ContextMenuLineIndex;
      string  wordBelow = FindWordFromPosition( m_ContextMenuPosition, m_ContextMenuLineIndex );

      if ( ( editSource.SelectionLength > 0 )
      &&   ( editSource.Selection.Start.iLine == lineIndex )
      &&   ( editSource.Selection.End.iLine == lineIndex ) )
      {
        // there is a selection on this line
        string watchedSelection = editSource.Selection.Text;

        // allow for single label; label,x; label,y; (label),y; (label),x
        List<Types.TokenInfo> tokens = Core.Compiling.ParserASM.ParseTokenInfo( watchedSelection, 0, watchedSelection.Length, Core.Compiling.ParserASM.m_TextCodeMappingRaw );

        if ( tokens.Count > 0 )
        {
          wordBelow = tokens[0].Content;
        }
      }

      Types.ASM.FileInfo debugFileInfo = Core.Navigating.DetermineASMFileInfo( DocumentInfo );
      if ( debugFileInfo == null )
      {
        System.Windows.Forms.MessageBox.Show( "Could not determine item address of " + wordBelow );
        return;
      }

      string  zone;
      string cheapLabelParent;
      
      debugFileInfo.FindZoneInfoFromDocumentLine( DocumentInfo.FullPath, lineIndex, out zone, out cheapLabelParent );

      int     address = -1;
      string  addressSource = wordBelow;

      if ( DocumentInfo.Element != null )
      {
        if ( ( DocumentInfo.Element.AssemblerType == Types.AssemblerType.C64_STUDIO )
        ||   ( DocumentInfo.Element.AssemblerType == Types.AssemblerType.AUTO ) )
        {
          if ( addressSource.StartsWith( "." ) )
          {
            addressSource = zone + addressSource;
          }
        }
      }
      var tokenInfo = DocumentInfo.ASMFileInfo.TokenInfoFromName( wordBelow, zone, cheapLabelParent );
      if ( ( tokenInfo != null )
      &&   ( tokenInfo.AddressOrValue != -1 ) )
      {
        address = (int)tokenInfo.AddressOrValue;
      }
      else if ( DocumentInfo.ASMFileInfo.Labels.ContainsKey( wordBelow ) )
      {
        var labelInfo = DocumentInfo.ASMFileInfo.Labels[wordBelow];

        address = (int)labelInfo.AddressOrValue;
      }
      else if ( debugFileInfo.Labels.ContainsKey( wordBelow ) )
      {
        var labelInfo = debugFileInfo.Labels[wordBelow];

        address = (int)labelInfo.AddressOrValue;
      }

      if ( address != -1 )
      {
        Types.Breakpoint bp = new RetroDevStudio.Types.Breakpoint();

        bp.LineIndex      = -1;
        bp.Address        = address;
        bp.TriggerOnStore = TriggerOnWrite;
        bp.TriggerOnLoad  = TriggerOnRead;
        bp.TriggerOnExec  = TriggerOnExec;
        bp.AddressSource  = addressSource;

        RaiseDocEvent( new DocEvent( DocEvent.Type.BREAKPOINT_ADDED, bp ) );
        return;
      }
      System.Windows.Forms.MessageBox.Show( "Could not determine item address of " + wordBelow );
    }



    private void addBreakpointToolStripMenuItem_Click( object sender, EventArgs e )
    {
      ToggleBreakpoint( m_ContextMenuLineIndex );
    }



    private void readAndWriteToolStripMenuItem_Click( object sender, EventArgs e )
    {
      AddDataBreakPoint( false, true, true );
    }

    
    
    private void readOnlyToolStripMenuItem_Click( object sender, EventArgs e )
    {
      AddDataBreakPoint( false, true, false );
    }



    private void writeOnlyToolStripMenuItem_Click( object sender, EventArgs e )
    {
      AddDataBreakPoint( false, false, true );
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

      var sourceData = new GR.IO.FileChunk( FileChunkConstants.SOURCE_ASM );

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
      ||   ( chunk.Type != FileChunkConstants.SOURCE_ASM ) )
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



    private void addSubtractDataValuesToolStripMenuItem_Click( object sender, EventArgs e )
    {
      var formDelta = new FormDeltaValue( Core );

      if ( formDelta.ShowDialog() != DialogResult.OK )
      {
        return;
      }

      int     firstLine = editSource.Selection.Start.iLine;
      int     endLine = editSource.Selection.End.iLine;

      if ( firstLine > endLine )
      {
        int   temp = firstLine;
        firstLine = endLine;
        endLine = temp;
      }

      if ( editSource.SelectionLength == 0 )
      {
        firstLine = m_ContextMenuLineIndex;
        endLine = firstLine;
      }


      editSource.BeginAutoUndo();

      for ( int lineIndex = firstLine; lineIndex <= endLine; ++lineIndex )
      {
        string    text = editSource.Lines[lineIndex];

        var tokens = Parser.ParseTokenInfo( text, 0, text.Length, Core.Compiling.ParserASM.m_TextCodeMappingRaw );

        int     firstLiteralTokenIndex = 1;

        if ( ( tokens.Count > 0 )
        &&   ( ( tokens[0].Type == TokenInfo.TokenType.LABEL_LOCAL )
        ||     ( tokens[0].Type == TokenInfo.TokenType.LABEL_INTERNAL )
        ||     ( tokens[0].Type == TokenInfo.TokenType.LABEL_GLOBAL )
        ||     ( tokens[0].Type == TokenInfo.TokenType.LABEL_CHEAP_LOCAL ) ) )
        {
          firstLiteralTokenIndex = 2;
        }

        if ( ( tokens.Count > firstLiteralTokenIndex )
        &&   ( tokens[firstLiteralTokenIndex - 1].Type == TokenInfo.TokenType.PSEUDO_OP ) )
        {
          string  upperToken = tokens[firstLiteralTokenIndex - 1].Content.ToUpper();
          if ( Parser.ASMFileInfo.AssemblerSettings.PseudoOps.ContainsKey( upperToken ) )
          {
            var pseudoOp = Parser.ASMFileInfo.AssemblerSettings.PseudoOps[upperToken];

            if ( ( pseudoOp.Type == MacroInfo.PseudoOpType.BYTE )
            ||   ( pseudoOp.Type == MacroInfo.PseudoOpType.WORD )
            ||   ( pseudoOp.Type == MacroInfo.PseudoOpType.LOW_BYTE )
            ||   ( pseudoOp.Type == MacroInfo.PseudoOpType.HIGH_BYTE )
            ||   ( pseudoOp.Type == MacroInfo.PseudoOpType.TEXT )
            ||   ( pseudoOp.Type == MacroInfo.PseudoOpType.TEXT_PET )
            ||   ( pseudoOp.Type == MacroInfo.PseudoOpType.TEXT_RAW )
            ||   ( pseudoOp.Type == MacroInfo.PseudoOpType.TEXT_SCREEN ) )
            {
              // we only touch literal values!
              for ( int i = firstLiteralTokenIndex; i < tokens.Count; ++i )
              {
                if ( ( tokens[i].Type == TokenInfo.TokenType.LITERAL_NUMBER )
                &&   ( ( ( ( i >= firstLiteralTokenIndex )
                &&         ( tokens[i - 1].Content == "," ) )
                ||       ( i == firstLiteralTokenIndex ) ) )
                ||     ( ( ( ( i + 1 < tokens.Count )
                &&         ( tokens[i + 1].Content == "," ) )
                ||       ( i + 1 == tokens.Count ) ) ) )
                {
                  if ( Parser.EvaluateTokens( i, tokens, i, 1, Core.Compiling.ParserASM.m_TextCodeMappingRaw, out SymbolInfo resultValueSymbol ) )
                  {
                    int resultValue = resultValueSymbol.ToInt32();
                    resultValue += formDelta.Delta;

                    if ( formDelta.InsertAsHex )
                    {
                      tokens[i].Content = "$" + resultValue.ToString( "X2" );
                    }
                    else
                    {
                      tokens[i].Content = resultValue.ToString();
                    }
                  }
                }
              }
              string    newLine = Parser.TokensToExpression( tokens );
              if ( tokens[0].StartPos > 0 )
              {
                newLine = new string( ' ', tokens[0].StartPos ) + newLine;
              }
              editSource.SetLineText( lineIndex, newLine ); 
            }
          }
        }
      }
      editSource.EndAutoUndo();

      SetModified();
    }



    private void findAllReferencesToolStripMenuItem_Click( object sender, EventArgs e )
    {
      FindAllReferences( m_ContextMenuPosition, m_ContextMenuLineIndex );
    }



    private bool FindReferences( int PositionInCode, int LineIndexInCode, out Types.ASM.FileInfo FileInfo, out SymbolInfo FoundSymbol )
    {
      FoundSymbol = null;
      FileInfo    = null;


      int     lineIndex = LineIndexInCode;
      string  wordBelow = FindWordFromPosition( PositionInCode, LineIndexInCode );

      if ( ( editSource.SelectionLength > 0 )
      &&   ( editSource.Selection.Start.iLine == lineIndex )
      &&   ( editSource.Selection.End.iLine == lineIndex ) )
      {
        // there is a selection on this line
        string watchedSelection = editSource.Selection.Text;

        // allow for single label; label,x; label,y; (label),y; (label),x
        List<Types.TokenInfo> tokens = Core.Compiling.ParserASM.ParseTokenInfo( watchedSelection, 0, watchedSelection.Length, Core.Compiling.ParserASM.m_TextCodeMappingRaw );

        if ( tokens.Count != 0 )
        {
          // single label
          if ( tokens.Count == 1 )
          {
            wordBelow = tokens[0].Content;
          }
          else
          {
            System.Windows.Forms.MessageBox.Show( "Could not determine symbol from selection" );
            return false;
          }
        }
      }

      FileInfo = Core.Navigating.DetermineASMFileInfo( DocumentInfo );
      if ( FileInfo == null )
      {
        System.Windows.Forms.MessageBox.Show( "Could not determine symbol of " + wordBelow );
        return false;
      }

      string zone;
      string cheapLabelParent;

      FileInfo.FindZoneInfoFromDocumentLine( DocumentInfo.FullPath, lineIndex, out zone, out cheapLabelParent );

      FoundSymbol = FileInfo.TokenInfoFromName( wordBelow, zone, cheapLabelParent );
      if ( FoundSymbol == null )
      {
        System.Windows.Forms.MessageBox.Show( "Unrecognized symbol, a recompile may be required" );
        return false;
      }

      // TODO - verify all files (and thus references) are up to date
      //foreach ( var reference in tokenInfo.References

      return true;
    }



    private void FindAllReferences( int PositionInCode, int LineIndexInCode )
    {
      if ( !FindReferences( PositionInCode, LineIndexInCode, out Types.ASM.FileInfo debugFileInfo, out SymbolInfo tokenInfo ) )
      {
        return;
      }
      Core.MainForm.m_FindReferences.UpdateReferences( DocumentInfo.Project, debugFileInfo, tokenInfo );
      Core.MainForm.m_FindReferences.Show();
    }



    private void RenameAllReferences( int PositionInCode, int LineIndexInCode )
    {
      if ( !FindReferences( PositionInCode, LineIndexInCode, out Types.ASM.FileInfo debugFileInfo, out SymbolInfo tokenInfo ) )
      {
        return;
      }

      var dlgRename = new FormRenameReference( Core, tokenInfo, debugFileInfo, Parser );

      dlgRename.ShowDialog();
    }



    private void editSource_LineVisited( object sender, LineVisitedArgs e )
    {
      Core.Navigating.VisitedLine( DocumentInfo, e.LineIndex );
    }



    private void btnCloseAllZones_Click( object sender, EventArgs e )
    {
      foreach ( var line in editSource.LineInfos )
      {
        if ( line.VisibleState != FastColoredTextBoxNS.VisibleState.Visible )
        {
          // a block is folded
          editSource.ExpandAllFoldingBlocks();
          return;
        }
      }
      editSource.CollapseAllFoldingBlocks();
    }



    private void convertDecimalToHexToolStripMenuItem_Click( object sender, EventArgs e )
    {
      int     firstLine = editSource.Selection.Start.iLine;
      int     endLine = editSource.Selection.End.iLine;

      if ( firstLine > endLine )
      {
        int   temp = firstLine;
        firstLine = endLine;
        endLine = temp;
      }

      if ( editSource.SelectionLength == 0 )
      {
        firstLine = m_ContextMenuLineIndex;
        endLine = firstLine;
      }


      editSource.BeginAutoUndo();

      for ( int lineIndex = firstLine; lineIndex <= endLine; ++lineIndex )
      {
        string    text = editSource.Lines[lineIndex];

        var tokens = Parser.ParseTokenInfo( text, 0, text.Length, Core.Compiling.ParserASM.m_TextCodeMappingRaw );

        int     firstLiteralTokenIndex = 1;

        if ( ( tokens.Count > 0 )
        &&   ( ( tokens[0].Type == TokenInfo.TokenType.LABEL_LOCAL )
        ||     ( tokens[0].Type == TokenInfo.TokenType.LABEL_INTERNAL )
        ||     ( tokens[0].Type == TokenInfo.TokenType.LABEL_GLOBAL )
        ||     ( tokens[0].Type == TokenInfo.TokenType.LABEL_CHEAP_LOCAL ) ) )
        {
          firstLiteralTokenIndex = 2;
        }

        if ( ( tokens.Count > firstLiteralTokenIndex )
        &&   ( tokens[firstLiteralTokenIndex - 1].Type == TokenInfo.TokenType.PSEUDO_OP ) )
        {
          string  upperToken = tokens[firstLiteralTokenIndex - 1].Content.ToUpper();
          if ( Parser.ASMFileInfo.AssemblerSettings.PseudoOps.ContainsKey( upperToken ) )
          {
            var pseudoOp = Parser.ASMFileInfo.AssemblerSettings.PseudoOps[upperToken];

            if ( ( pseudoOp.Type == MacroInfo.PseudoOpType.BYTE )
            ||   ( pseudoOp.Type == MacroInfo.PseudoOpType.WORD )
            ||   ( pseudoOp.Type == MacroInfo.PseudoOpType.LOW_BYTE )
            ||   ( pseudoOp.Type == MacroInfo.PseudoOpType.HIGH_BYTE )
            ||   ( pseudoOp.Type == MacroInfo.PseudoOpType.TEXT )
            ||   ( pseudoOp.Type == MacroInfo.PseudoOpType.TEXT_PET )
            ||   ( pseudoOp.Type == MacroInfo.PseudoOpType.TEXT_RAW )
            ||   ( pseudoOp.Type == MacroInfo.PseudoOpType.TEXT_SCREEN ) )
            {
              // we only touch literal values!
              for ( int i = firstLiteralTokenIndex; i < tokens.Count; ++i )
              {
                if ( ( tokens[i].Type == TokenInfo.TokenType.LITERAL_NUMBER )
                &&   ( ( ( ( i >= firstLiteralTokenIndex )
                &&         ( tokens[i - 1].Content == "," ) )
                ||       ( i == firstLiteralTokenIndex ) ) )
                ||     ( ( ( ( i + 1 < tokens.Count )
                &&         ( tokens[i + 1].Content == "," ) )
                ||       ( i + 1 == tokens.Count ) ) ) )
                {
                  if ( Parser.EvaluateTokens( i, tokens, i, 1, Core.Compiling.ParserASM.m_TextCodeMappingRaw, out SymbolInfo resultValueSymbol ) )
                  {
                    int resultValue = resultValueSymbol.ToInt32();
                    tokens[i].Content = "$" + resultValue.ToString( "X2" );
                  }
                }
              }
              string    newLine = Parser.TokensToExpression( tokens );
              if ( tokens[0].StartPos > 0 )
              {
                newLine = new string( ' ', tokens[0].StartPos ) + newLine;
              }
              editSource.SetLineText( lineIndex, newLine ); 
            }
          }
        }
      }
      editSource.EndAutoUndo();

      SetModified();    
    }



    private void convertHexToDecimalToolStripMenuItem1_Click( object sender, EventArgs e )
    {
      int     firstLine = editSource.Selection.Start.iLine;
      int     endLine = editSource.Selection.End.iLine;

      if ( firstLine > endLine )
      {
        int   temp = firstLine;
        firstLine = endLine;
        endLine = temp;
      }

      if ( editSource.SelectionLength == 0 )
      {
        firstLine = m_ContextMenuLineIndex;
        endLine = firstLine;
      }


      editSource.BeginAutoUndo();

      for ( int lineIndex = firstLine; lineIndex <= endLine; ++lineIndex )
      {
        string    text = editSource.Lines[lineIndex];

        var tokens = Parser.ParseTokenInfo( text, 0, text.Length, Core.Compiling.ParserASM.m_TextCodeMappingRaw );

        int     firstLiteralTokenIndex = 1;

        if ( ( tokens.Count > 0 )
        &&   ( ( tokens[0].Type == TokenInfo.TokenType.LABEL_LOCAL )
        ||     ( tokens[0].Type == TokenInfo.TokenType.LABEL_INTERNAL )
        ||     ( tokens[0].Type == TokenInfo.TokenType.LABEL_GLOBAL )
        ||     ( tokens[0].Type == TokenInfo.TokenType.LABEL_CHEAP_LOCAL ) ) )
        {
          firstLiteralTokenIndex = 2;
        }

        if ( ( tokens.Count > firstLiteralTokenIndex )
        &&   ( tokens[firstLiteralTokenIndex - 1].Type == TokenInfo.TokenType.PSEUDO_OP ) )
        {
          string  upperToken = tokens[firstLiteralTokenIndex - 1].Content.ToUpper();
          if ( Parser.ASMFileInfo.AssemblerSettings.PseudoOps.ContainsKey( upperToken ) )
          {
            var pseudoOp = Parser.ASMFileInfo.AssemblerSettings.PseudoOps[upperToken];

            if ( ( pseudoOp.Type == MacroInfo.PseudoOpType.BYTE )
            ||   ( pseudoOp.Type == MacroInfo.PseudoOpType.WORD )
            ||   ( pseudoOp.Type == MacroInfo.PseudoOpType.LOW_BYTE )
            ||   ( pseudoOp.Type == MacroInfo.PseudoOpType.HIGH_BYTE )
            ||   ( pseudoOp.Type == MacroInfo.PseudoOpType.TEXT )
            ||   ( pseudoOp.Type == MacroInfo.PseudoOpType.TEXT_PET )
            ||   ( pseudoOp.Type == MacroInfo.PseudoOpType.TEXT_RAW )
            ||   ( pseudoOp.Type == MacroInfo.PseudoOpType.TEXT_SCREEN ) )
            {
              // we only touch literal values!
              for ( int i = firstLiteralTokenIndex; i < tokens.Count; ++i )
              {
                if ( ( tokens[i].Type == TokenInfo.TokenType.LITERAL_NUMBER )
                &&   ( ( ( ( i >= firstLiteralTokenIndex )
                &&         ( tokens[i - 1].Content == "," ) )
                ||       ( i == firstLiteralTokenIndex ) ) )
                ||     ( ( ( ( i + 1 < tokens.Count )
                &&         ( tokens[i + 1].Content == "," ) )
                ||       ( i + 1 == tokens.Count ) ) ) )
                {
                  if ( Parser.EvaluateTokens( i, tokens, i, 1, Core.Compiling.ParserASM.m_TextCodeMappingRaw, out SymbolInfo resultValueSymbol ) )
                  {
                    int resultValue = resultValueSymbol.ToInt32();
                    tokens[i].Content = resultValue.ToString();
                  }
                }
              }
              string    newLine = Parser.TokensToExpression( tokens );
              if ( tokens[0].StartPos > 0 )
              {
                newLine = new string( ' ', tokens[0].StartPos ) + newLine;
              }
              editSource.SetLineText( lineIndex, newLine ); 
            }
          }
        }
      }
      editSource.EndAutoUndo();

      SetModified();    
    }



    private void btnShowShortCutLabels_Click( object sender, EventArgs e )
    {
      Core.Settings.ASMShowShortCutLabels = !Core.Settings.ASMShowShortCutLabels;

      Core.MainForm.RaiseApplicationEvent( new ApplicationEvent( ApplicationEvent.Type.SETTING_MODIFIED ) { OriginalValue = "ASMShowShortCutLabels" }  );
    }



    private void renameAllReferencesToolStripMenuItem_Click( object sender, EventArgs e )
    {
      RenameAllReferences( m_ContextMenuPosition, m_ContextMenuLineIndex );
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



  }
}
