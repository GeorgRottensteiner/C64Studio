using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace C64Studio
{
  public partial class SourceASMEx : BaseDocument
  {
    private const uint TME_HOVER = 0x00000001;
    private const uint TME_LEAVE = 0x00000002;
    private const uint HOVER_DEFAULT = 0xFFFFFFFF;

    private const int BORDER_MARKER_WIDTH   = 20;
    private const int BORDER_SIZE_WIDTH     = 24;
    private const int BORDER_CYCLES_WIDTH   = 48;

     
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

    bool                                      m_RecalcZone = false;

    // inserting text flag
    bool                                      m_InsertingText = false;

    DateTime                                  m_LastChange = DateTime.Now;

    List<Types.ASM.LineInfo>                  m_LineInfos = new List<Types.ASM.LineInfo>();

    Timer                                     m_DelayedEventTimer = new Timer();

    private string                            m_CurrentHighlightText = null;



    private GR.Collections.Map<int,Types.Breakpoint>    m_BreakPoints = new GR.Collections.Map<int,C64Studio.Types.Breakpoint>();

    public bool                               DoNotFollowZoneSelectors = false;

    private Parser.ASMFileParser              Parser = new C64Studio.Parser.ASMFileParser();

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


    public SourceASMEx( StudioCore Core )
    {
      this.Core = Core;

      DocumentInfo.Type = ProjectElement.ElementType.ASM_SOURCE;
      DocumentInfo.UndoManager.MainForm = Core.MainForm;

      string opCodes = @"\b(lda|sta|ldy|sty|ldx|stx|rts|jmp|jsr|rti|sei|cli|asl|lsr|inc|dec|inx|dex|iny|dey|cpx|cpy|cmp|bit|bne|beq|bcc|bcs|bpl|bmi|adc|sec|clc|sbc|tax|tay|tya|txa|pha|pla|eor|and|ora|ror|rol|php|plp|clv|cld|bvc|bvs|brk|nop|txs|tsx|slo|rla|sre|rra|sax|lax|dcp|isc|anc|alr|arr|xaa|axs|ahx|shy|shx|tas|las|sed)\b";
      string pseudoOps = @"(!byte|!by|!basic|!8|!08|!word|!wo|!16|!text|!tx|!scr|!pet|!raw|!pseudopc|!realpc|!bank|!convtab|!ct|!binary|!bin|!bi|!source|!src|!to|!zone|!zn|!error|!serious|!warn|"
        + @"!message|!ifdef|!ifndef|!if|!fill|!fi|!align|!endoffile|!nowarn|!for|!end|!macro|!trace|!media|!mediasrc|!sl|!cpu|!set)\b";

      m_TextRegExp[(int)Types.ColorableElement.LITERAL_NUMBER] = new System.Text.RegularExpressions.Regex( @"\b\d+[\.]?\d*([eE]\-?\d+)?[lLdDfF]?\b|\B\$[a-fA-F\d]+\b|\b0x[a-fA-F\d]+\b" );
      m_TextRegExp[(int)Types.ColorableElement.LITERAL_STRING] = new System.Text.RegularExpressions.Regex( @"""""|''|"".*?[^\\]""|'.*?[^\\]'" );

      m_TextRegExp[(int)Types.ColorableElement.CODE] = new System.Text.RegularExpressions.Regex( opCodes, System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled );
      m_TextRegExp[(int)Types.ColorableElement.PSEUDO_OP] = new System.Text.RegularExpressions.Regex( pseudoOps, System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled );

      m_TextRegExp[(int)Types.ColorableElement.LABEL] = new System.Text.RegularExpressions.Regex( @"[.]{0,1}[+\-a-zA-Z]+[a-zA-Z_\d.]*[:]*" );
      m_TextRegExp[(int)Types.ColorableElement.COMMENT] = new System.Text.RegularExpressions.Regex( @";.*" );

      m_TextRegExp[(int)Types.ColorableElement.OPERATOR] = new System.Text.RegularExpressions.Regex( @"[+\-/*(){}=<>,#]" );


      m_IsSaveable = true;
      InitializeComponent();

      m_DelayedEventTimer.Interval = 500;
      m_DelayedEventTimer.Tick += m_DelayedEventTimer_Tick;

      AutoComplete = new FastColoredTextBoxNS.AutocompleteMenu( editSource );
      //AutoComplete.SearchPattern = @"([A-Za-z_.]|(?<=[A-Za-z_.][\w]))";
      AutoComplete.SearchPattern = @"[A-Za-z_.][\w]*";

      editSource.AutoIndentExistingLines = false;
      editSource.AutoIndentChars = false;

      // allows auto indent on new lines, indents similar to previous line
      editSource.AutoIndent = true;
      editSource.ShowLineNumbers = !Core.Settings.ASMHideLineNumbers;

      //editSource.FindEndOfFoldingBlockStrategy = FastColoredTextBoxNS.FindEndOfFoldingBlockStrategy.Strategy2;

      Parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      contextSource.Opening += new CancelEventHandler( contextSource_Opening );

      editSource.Font = new System.Drawing.Font( Core.Settings.SourceFontFamily, Core.Settings.SourceFontSize );
      //string wordChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_äöüÄÖÜß!1234567890";
      //editSource.Lexing.Keywords[0] = "lda sta ldy sty ldx stx rts jmp jsr rti sei cli asl lsr inc dec inx dex iny dey cpx cpy cmp bit bne beq bcc bcs bpl bmi adc sec clc sbc tax tay tya txa pha pla eor and ora ror rol php plp clv cld bvc bvs brk nop txs tsx !byte !by !08 !8 !word !wo !16 !zone !zn !text !tx !source !binary !bi !ct !convtab !align slo rla sre rra sax lax dcp isc anc alr arr xaa axs ahx shy shx tas las sed";
      editSource.KeyDown += new System.Windows.Forms.KeyEventHandler( editSource_KeyDown );
      editSource.KeyPress += new System.Windows.Forms.KeyPressEventHandler( editSource_KeyPress );
      editSource.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler( editSource_PreviewKeyDown );

      editSource.SelectionChanged += new EventHandler( editSource_SelectionChanged );

      editSource.MouseHover += new EventHandler( editSource_MouseHover );

      this.Activated += new EventHandler( SourceASM_Activated );

      editSource.TextChanged += new EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>( editSource_TextChanged );
      editSource.TextChanging += new EventHandler<FastColoredTextBoxNS.TextChangingEventArgs>( editSource_TextChanging );
      editSource.TextChangedDelayed += editSource_TextChangedDelayed;
      editSource.FoldingBlockStateChanged += editSource_FoldingBlockStateChanged;

      editSource.LineInserted += new EventHandler<FastColoredTextBoxNS.LineInsertedEventArgs>( editSource_LineInserted );
      editSource.LineRemoved += new EventHandler<FastColoredTextBoxNS.LineRemovedEventArgs>( editSource_LineRemoved );

      editSource.UndoRedoStateChanged += new EventHandler<EventArgs>( editSource_UndoRedoStateChanged );

      editSource.LeftBracket = '(';
      editSource.RightBracket = ')';
      editSource.LeftBracket2 = '\x0';
      editSource.RightBracket2 = '\x0';
      editSource.CommentPrefix = ";";
      editSource.SelectionChangedDelayed += editSource_SelectionChangedDelayed;

      RefreshDisplayOptions();

      editSource.ContextMenu = null;

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
      ResetAllStyles( e.ChangedRange );

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
      range.SetFoldingMarkers( "!zone", "!zone" );
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
        case C64Studio.Types.ColorableElement.EMPTY_SPACE:
          value = 0;
          break;
        case C64Studio.Types.ColorableElement.CURRENT_DEBUG_LINE:
          value = 1;
          break;
        case C64Studio.Types.ColorableElement.HIGHLIGHTED_SEARCH_RESULTS:
          value = 2;
          break;
        case C64Studio.Types.ColorableElement.COMMENT:
          value = 3;
          break;
        case C64Studio.Types.ColorableElement.LITERAL_STRING:
          value = 4;
          break;
        case C64Studio.Types.ColorableElement.LITERAL_NUMBER:
          value = 5;
          break;
        case C64Studio.Types.ColorableElement.OPERATOR:
          value = 6;
          break;
        case C64Studio.Types.ColorableElement.PSEUDO_OP:
          value = 7;
          break;
        case C64Studio.Types.ColorableElement.CODE:
          value = 8;
          break;
        case C64Studio.Types.ColorableElement.LABEL:
          value = 9;
          break;
        case C64Studio.Types.ColorableElement.ERROR_UNDERLINE:
          value = 10;
          break;
        case C64Studio.Types.ColorableElement.NONE:
          value = 11;
          break;

      }
      return value;
    }



    void editSource_LineInserted( object sender, FastColoredTextBoxNS.LineInsertedEventArgs e )
    {
      // move related breakpoints!
      for ( int i = 0; i < e.Count; ++i )
      {
        m_LineInfos.Insert( e.Index - 1, new Types.ASM.LineInfo() );
      }

      int                         insertedAtLine = e.Index;

      GR.Collections.Map<int,Types.Breakpoint>   origBreakpoints = new GR.Collections.Map<int,C64Studio.Types.Breakpoint>( m_BreakPoints );
      List<Types.Breakpoint>                     movedBreakpoints = new List<C64Studio.Types.Breakpoint>();

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
    }



    void editSource_LineRemoved( object sender, FastColoredTextBoxNS.LineRemovedEventArgs e )
    {
      //Debug.Log( "Lines removed " + e.Index + ", " + e.Count );
      m_LineInfos.RemoveRange( e.Index - 1, e.Count );

      // move related breakpoints!
      int deletedAtLine = e.Index;

      GR.Collections.Map<int,Types.Breakpoint> origBreakpoints = new GR.Collections.Map<int, C64Studio.Types.Breakpoint>( m_BreakPoints );

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
    }



    void editSource_TextChanging( object sender, FastColoredTextBoxNS.TextChangingEventArgs e )
    {
      //throw new NotImplementedException();
    }



    void editSource_TextChanged( object sender, FastColoredTextBoxNS.TextChangedEventArgs e )
    {
      m_LastChange = DateTime.Now;

      //FastColoredTextBoxNS.Range range = new FastColoredTextBoxNS.Range( editSource, e.ChangedRange.Start, new FastColoredTextBoxNS.Place( 0, 0 ) );

      //ShowAutoComplete();

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
        m_TextStyles[SyntaxElementStylePrio( Element )] = new FastColoredTextBoxNS.WavyLineStyle( 255, GR.Color.Helper.FromARGB( Core.Settings.SyntaxColoring[Element].FGColor ) );
        editSource.Styles[SyntaxElementStylePrio( Element )] = m_TextStyles[SyntaxElementStylePrio( Element )];
        return;
      }
      System.Drawing.Brush      foreBrush = new System.Drawing.SolidBrush( GR.Color.Helper.FromARGB( Core.Settings.SyntaxColoring[Element].FGColor ) );
      System.Drawing.Brush      backBrush = null;
      System.Drawing.FontStyle  fontStyle = System.Drawing.FontStyle.Regular;

      if ( Core.Settings.SyntaxColoring[Element].BGColorAuto )
      {
        backBrush = new System.Drawing.SolidBrush( GR.Color.Helper.FromARGB( Core.Settings.SyntaxColoring[Types.ColorableElement.EMPTY_SPACE].BGColor ) );
      }
      else
      {
        backBrush = new System.Drawing.SolidBrush( GR.Color.Helper.FromARGB( Core.Settings.SyntaxColoring[Element].BGColor ) );
      }

      m_TextStyles[SyntaxElementStylePrio( Element )] = new FastColoredTextBoxNS.TextStyle( foreBrush, backBrush, fontStyle );

      editSource.Styles[SyntaxElementStylePrio( Element )] = m_TextStyles[SyntaxElementStylePrio( Element )];

      // empty space
      editSource.BackColor = GR.Color.Helper.FromARGB( Core.Settings.SyntaxColoring[Types.ColorableElement.EMPTY_SPACE].BGColor );
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
      int y = editSource.PlaceToPoint( editSource.GetLine( LineIndex ).Start ).Y;
      System.Drawing.Rectangle    markerRect = new System.Drawing.Rectangle( 0, y, editSource.LeftIndent, editSource.LineInfos[LineIndex].WordWrapStringsCount * editSource.CharHeight );
      editSource.Invalidate( markerRect );
    }



    public void ToggleBreakpoint( int LineIndex )
    {
      // break point set
      if ( !m_BreakPoints.ContainsKey( LineIndex ) )
      {
        Types.Breakpoint bp = new C64Studio.Types.Breakpoint();

        bp.DocumentFilename = DocumentInfo.FullPath;
        bp.LineIndex = LineIndex;

        m_BreakPoints.Add( LineIndex, bp );

        InvalidateMarkerAreaAtLine( LineIndex );
      }
      else
      {
        if ( m_BreakPoints.ContainsKey( LineIndex ) )
        {
          Types.Breakpoint bp = m_BreakPoints[LineIndex];
          m_BreakPoints.Remove( LineIndex );

          InvalidateMarkerAreaAtLine( LineIndex );
        }
      }
    }



    void ToggleBreakpointOnEvent( int LineIndex )
    {
      // break point set
      if ( !m_BreakPoints.ContainsKey( LineIndex ) )
      {
        Types.Breakpoint bp = new C64Studio.Types.Breakpoint();

        bp.DocumentFilename = DocumentInfo.FullPath;
        bp.LineIndex = LineIndex;

        Types.ASM.FileInfo fileInfo = Core.Navigating.DetermineASMFileInfo( DocumentInfo );

        if ( Core.State == C64Studio.Types.StudioState.DEBUGGING_BROKEN )
        {
          int   globalLineIndex = -1;
          if ( fileInfo.FindGlobalLineIndex( LineIndex, DocumentInfo.FullPath, out globalLineIndex ) )
          {
            bp.Address = fileInfo.FindLineAddress( globalLineIndex );
            //Debug.Log( "Found address " + bp.Address.ToString( "x4" ) );
          }
          else
          {
            return;
          }
        }
        else if ( Core.State != C64Studio.Types.StudioState.NORMAL )
        {
          // cannot add breakpoints during this state
          return;
        }

        m_BreakPoints.Add( LineIndex, bp );

        RaiseDocEvent( new DocEvent( DocEvent.Type.BREAKPOINT_ADDED, bp ) );

        InvalidateMarkerAreaAtLine( LineIndex );
      }
      else
      {
        if ( m_BreakPoints.ContainsKey( LineIndex ) )
        {
          Types.Breakpoint bp = m_BreakPoints[LineIndex];
          m_BreakPoints.Remove( LineIndex );
          RaiseDocEvent( new DocEvent( DocEvent.Type.BREAKPOINT_REMOVED, bp ) );

          InvalidateMarkerAreaAtLine( LineIndex );
        }
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
      string    newInfo = "Row " + ( editSource.Selection.Start.iLine + 1 ).ToString() + ", Col " + editSource.Selection.Start.iChar.ToString();

      int       numBytes = 0;
      int       numCycles = 0;
      bool      hadPotentialEntry = false;
      int[]     potentialCycles = new int[3];

      if ( !editSource.Selection.IsEmpty )
      {
        int     startLine = editSource.Selection.Start.iLine;
        int     endLine = editSource.Selection.End.iLine;
        if ( startLine > endLine )
        {
          startLine = endLine;
          endLine = editSource.Selection.Start.iLine;
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
          string    selText = selRange.Text;
          if ( selText.EndsWith( System.Environment.NewLine ) )
          {
            --numLines;
          }
          newInfo += ", " + editSource.SelectionLength.ToString() + " bytes, " + numLines.ToString() + " lines selected";

        }
        else
        {
          newInfo += ", " + editSource.SelectionLength.ToString() + " bytes selected";
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
      FilterAutoComplete();

      m_LastZoneUpdateLine = CurrentLineIndex;

      

      string zone = FindZoneAtCaretPosition();
      //string localLabel = FindLocalLabelAtCaretPosition();
      //Debug.Log( "zone found:" + zone );

      C64Studio.Types.SymbolInfo symbol = (C64Studio.Types.SymbolInfo)comboZoneSelector.SelectedItem;
      if ( ( symbol != null )
      &&   ( zone != symbol.Zone ) )
      {
        for ( int i = 0; i < comboZoneSelector.Items.Count; ++i )
        {
          C64Studio.Types.SymbolInfo symbol2 = (C64Studio.Types.SymbolInfo)comboZoneSelector.Items[i];
          if ( symbol2.Zone == zone )
          {
            DoNotFollowZoneSelectors = true;
            comboZoneSelector.SelectedIndex = i;
            RefreshLocalSymbols();
            int localSymbolLine = -1;
            int bestLocalSymbolIndex = 0;

            for ( int j = 0; j < comboLocalLabelSelector.Items.Count; ++j )
            {
              C64Studio.Types.SymbolInfo symbol3 = (C64Studio.Types.SymbolInfo)comboLocalLabelSelector.Items[j];
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
          C64Studio.Types.SymbolInfo symbol3 = (C64Studio.Types.SymbolInfo)comboLocalLabelSelector.Items[j];
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

      //string zone = FindZoneFromLine( lineNumber );
      string zone = debugFileInfo.FindZoneFromDocumentLine( DocumentInfo.FullPath, lineNumber );

      //MainForm.EnsureFileIsParsed();
      Types.SymbolInfo tokenInfo = debugFileInfo.TokenInfoFromName( wordBelow, zone );
      if ( ( tokenInfo != null )
      &&   ( tokenInfo.Type != Types.SymbolInfo.Types.UNKNOWN ) )
      {
        string toolTipText = tokenInfo.Info;

        toolTipText += "$" + tokenInfo.AddressOrValue.ToString( "x4" ) + ", " + tokenInfo.AddressOrValue.ToString();

        byte    valueBelow = 0xcd;
        if ( Core.Debugging.Debugger.FetchValue( tokenInfo.AddressOrValue, out valueBelow ) )
        {
          toolTipText += " ($" + valueBelow.ToString( "x2" ) + "/" + valueBelow.ToString() + ")";
        }
        //m_ToolTip.Hide( editSource );
        //m_ToolTip.Show( toolTipText, editSource, editSource.PointToClient( System.Windows.Forms.Cursor.Position ) );
        if ( m_LastTooltipText != toolTipText )
        {
          m_LastTooltipText = toolTipText;
          m_ToolTip.SetToolTip( editSource, toolTipText );
        }
      }
      else
      {
        m_ToolTip.Hide( editSource );
      }
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
        C64Studio.Types.SymbolInfo symbol = (C64Studio.Types.SymbolInfo)comboZoneSelector.SelectedItem;
        currentZone = symbol.Name;
      }

      comboZoneSelector.Items.Clear();

      C64Studio.Types.SymbolInfo    globalSymbol = new C64Studio.Types.SymbolInfo();
      globalSymbol.Name = "Global";
      globalSymbol.LocalLineIndex = 0;
      globalSymbol.DocumentFilename = DocumentFilename;
      comboZoneSelector.Items.Add( globalSymbol );

      GR.Collections.Set<string>    keySet = DocumentInfo.KnownTokens.GetUniqueKeys();
      GR.Collections.Map<string,C64Studio.Types.SymbolInfo>  uniqueZones = new GR.Collections.Map<string, C64Studio.Types.SymbolInfo>();

      foreach ( string key in keySet )
      {
        List<C64Studio.Types.SymbolInfo>    listValues = DocumentInfo.KnownTokens.GetValues( key, false );

        foreach ( C64Studio.Types.SymbolInfo symbol in listValues )
        {
          if ( ( GR.Path.IsPathEqual( symbol.DocumentFilename, DocumentInfo.FullPath ) )
          &&   ( symbol.Type == C64Studio.Types.SymbolInfo.Types.ZONE ) )
          /*
          &&   ( !string.IsNullOrEmpty( symbol.Zone ) )
          &&   ( DocumentInfo.ASMFileInfo.Zones.ContainsKey( symbol.Zone ) ) )*/
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
      RefreshLocalSymbols();
      RefreshAutoComplete();
    }



    public override void OnKnownKeywordsChanged()
    {
      m_LastZoneUpdateLine = -1;
      RefreshLocalSymbols();
      RefreshAutoComplete();
    }



    void RefreshLocalSymbols()
    {
      string currentZone = "";
      if ( comboZoneSelector.SelectedIndex != -1 )
      {
        C64Studio.Types.SymbolInfo symbol = (C64Studio.Types.SymbolInfo)comboZoneSelector.SelectedItem;
        currentZone = symbol.Name;
        if ( currentZone == "Global" )
        {
          currentZone = "";
        }
      }
      string currentLabel = "";
      if ( comboLocalLabelSelector.SelectedIndex != -1 )
      {
        C64Studio.Types.SymbolInfo symbol = (C64Studio.Types.SymbolInfo)comboLocalLabelSelector.SelectedItem;
        currentLabel = symbol.Name;
      }

      comboLocalLabelSelector.Items.Clear();

      GR.Collections.Set<string> keySet = DocumentInfo.KnownTokens.GetUniqueKeys();
      string fullPath = DocumentInfo.FullPath;
      foreach ( string key in keySet )
      {
        List<C64Studio.Types.SymbolInfo> listValues = DocumentInfo.KnownTokens.GetValues( key, false );
        foreach ( C64Studio.Types.SymbolInfo symbol in listValues )
        {
          if ( ( symbol.Type == C64Studio.Types.SymbolInfo.Types.CONSTANT_1 )
          ||   ( symbol.Type == C64Studio.Types.SymbolInfo.Types.CONSTANT_2 )
          ||   ( symbol.Type == C64Studio.Types.SymbolInfo.Types.LABEL ) )
          {
            if ( ( GR.Path.IsPathEqual( symbol.DocumentFilename, fullPath ) )
            &&   ( symbol.Zone == currentZone ) )
            {
              int itemIndex = comboLocalLabelSelector.Items.Add( symbol );
              if ( symbol.Name == currentLabel )
              {
                comboLocalLabelSelector.SelectedIndex = itemIndex;
              }
            }
          }
        }
      }
      UpdateStatusInfo();
    }



    void FilterAutoComplete()
    {
      var newList = new List<FastColoredTextBoxNS.AutocompleteItem>();

      string curLine = editSource.Lines[CurrentLineIndex].TrimEnd( new char[] { '\r', '\n' } );
      int position = editSource.PlaceToPosition( editSource.Selection.Start );
      int posX = editSource.Selection.Start.iChar;

      // ugly quick hack, allow x and y as index fields for opcodes (e.g. sta $fff,x)
      int   errorPos = -1;
      var tokens = Parser.PrepareLineTokens( curLine, out errorPos );
      bool hadOpcodeFirst = false;
      if ( tokens != null )
      {
        // check for opcode or special command
        for ( int i = 0; i < tokens.Count; ++i )
        {
          if ( tokens[i].Type == C64Studio.Types.TokenInfo.TokenType.COMMENT )
          {
            break;
          }
          if ( ( tokens[i].Type == C64Studio.Types.TokenInfo.TokenType.OPCODE )
          ||   ( tokens[i].Type == C64Studio.Types.TokenInfo.TokenType.OPCODE_FIXED_NON_ZP )
          ||   ( tokens[i].Type == C64Studio.Types.TokenInfo.TokenType.OPCODE_FIXED_ZP ) )
          {
            // we've got an opcode
            hadOpcodeFirst = true;
            //Debug.Log( "opcode found" );
            if ( ( tokens[tokens.Count - 1].Type == C64Studio.Types.TokenInfo.TokenType.SEPARATOR )
            &&   ( tokens[tokens.Count - 1].Content == "," ) )
            {
              // comma is the last token
              var   newList2 = new List<FastColoredTextBoxNS.AutocompleteItem>();

              //List<string>    newList2 = new List<string>();

              newList2.Add( new FastColoredTextBoxNS.AutocompleteItem( "x" ) { ToolTipTitle = tokens[i].Content + ",x", ToolTipText = tokens[i].Content + ",x" } );
              newList2.Add( new FastColoredTextBoxNS.AutocompleteItem( "y" ) { ToolTipTitle = tokens[i].Content + ",y", ToolTipText = tokens[i].Content + ",y" } );


              //newList2.Add( "x" );
              //newList2.Add( "y" );

              ///editSource.AutoComplete.DropRestOfWord = true;
              ///editSource.AutoComplete.IsCaseSensitive = false;

              AutoComplete.Items.SetAutocompleteItems( newList2 );
              return;
            }
          }
          if ( ( tokens[i].Type == C64Studio.Types.TokenInfo.TokenType.SEPARATOR )
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

      int lineIndex = CurrentLineIndex;
      string wordBelow = FindWordFromPosition( CurrentPosition() - 1, lineIndex );
      string zone = FindZoneFromLine( lineIndex );
      /*
      if ( wordBelow.Length <= 0 )
      {
        return;
      }*/

      newList = new List<FastColoredTextBoxNS.AutocompleteItem>();

      //List<string>    newList = new List<string>();
      foreach ( var entry in DocumentInfo.KnownKeywords )
      {
        if ( entry.Token.StartsWith( C64Studio.Parser.ASMFileParser.InternalLabelPrefix ) )
        {
          continue;
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
        if ( entry.Key.StartsWith( C64Studio.Parser.ASMFileParser.InternalLabelPrefix ) )
        {
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

        ///editSource.AutoComplete.DropRestOfWord = true;
        ///editSource.AutoComplete.IsCaseSensitive = false;
        //editSource.AutoComplete.Show( wordBelow.Length, newList );

        AutoComplete.MinFragmentLength = 1;
        AutoComplete.AutoSize = true;
        AutoComplete.Items.SetAutocompleteItems( newList );
      }
    }



    void ShowAutoComplete()
    {
      AutoComplete.Show( true );
      /*
      return;

      //Debug.Log( "ShowAutoComplete" );
      string curLine = editSource.Lines[CurrentLineIndex].TrimEnd( new char[]{ '\r', '\n' } );
      int position = editSource.PlaceToPosition( editSource.Selection.Start );
      int posX = editSource.Selection.Start.iChar;

      // ugly quick hack, allow x and y
      int   errorPos = -1;
      var tokens = Parser.PrepareLineTokens( curLine, out errorPos );
      bool hadOpcodeFirst = false;
      if ( tokens != null )
      {
        // check for opcode or special command
        for ( int i = 0; i < tokens.Count; ++i )
        {
          if ( tokens[i].Type == C64Studio.Types.TokenInfo.TokenType.COMMENT )
          {
            break;
          }
          if ( ( tokens[i].Type == C64Studio.Types.TokenInfo.TokenType.OPCODE )
          ||   ( tokens[i].Type == C64Studio.Types.TokenInfo.TokenType.OPCODE_FIXED_NON_ZP )
          ||   ( tokens[i].Type == C64Studio.Types.TokenInfo.TokenType.OPCODE_FIXED_ZP ) )
          {
            // we've got an opcode
            hadOpcodeFirst = true;
            //Debug.Log( "opcode found" );
            if ( ( tokens[tokens.Count - 1].Type == C64Studio.Types.TokenInfo.TokenType.SEPARATOR )
            &&   ( tokens[tokens.Count - 1].Content == "," ) )
            {
              // comma is the last token
              var   newList2 = new List<FastColoredTextBoxNS.AutocompleteItem>();

              //List<string>    newList2 = new List<string>();

              newList2.Add( new FastColoredTextBoxNS.AutocompleteItem( "x" ) { ToolTipTitle = tokens[i].Content + ",x", ToolTipText = tokens[i].Content + ",x" } );
              newList2.Add( new FastColoredTextBoxNS.AutocompleteItem( "y" ) { ToolTipTitle = tokens[i].Content + ",y", ToolTipText = tokens[i].Content + ",y" } );


              //newList2.Add( "x" );
              //newList2.Add( "y" );

              ///editSource.AutoComplete.DropRestOfWord = true;
              ///editSource.AutoComplete.IsCaseSensitive = false;

              AutoComplete.Items.SetAutocompleteItems( newList2 );
              AutoComplete.Show( true );
              return;
            }
          }
          if ( ( tokens[i].Type == C64Studio.Types.TokenInfo.TokenType.SEPARATOR )
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

      /*
      if ( editSource.PositionIsOnComment( editSource.CurrentPos - 1 ) )
      {
        return;
      }*/


      /*
      int lineIndex = CurrentLineIndex;// editSource.Lines.FromPosition( editSource.CurrentPos - 1 ).Number;
      string wordBelow = FindWordFromPosition( CurrentPosition() - 1, lineIndex );
      string zone = FindZoneFromLine( lineIndex );
      if ( wordBelow.Length <= 0 )
      {
        return;
      }
      var   newList = new List<FastColoredTextBoxNS.AutocompleteItem>();
      //List<string>    newList = new List<string>();
      foreach ( var entry in DocumentInfo.KnownKeywords )
      {
        if ( entry.Token.StartsWith( wordBelow, StringComparison.CurrentCultureIgnoreCase ) )
        {
          newList.Add( new FastColoredTextBoxNS.AutocompleteItem( entry.Token ) { ToolTipTitle = entry.ToolTipTitle, ToolTipText = entry.ToolTipText } );
        }
        if ( ( wordBelow.StartsWith( "." ) )
        &&   ( entry.Token.StartsWith( zone + "." + wordBelow.Substring( 1 ), StringComparison.CurrentCultureIgnoreCase ) ) )
        {
          newList.Add( new FastColoredTextBoxNS.AutocompleteItem( entry.Token.Substring( zone.Length ) ) { ToolTipTitle = entry.ToolTipTitle, ToolTipText = entry.ToolTipText } );
        }
      }
      GR.Collections.Set<string>    uniqueKeys = DocumentInfo.KnownTokens.GetUniqueKeys();
      foreach ( string entry in uniqueKeys )
      {
        if ( entry.StartsWith( wordBelow, StringComparison.CurrentCultureIgnoreCase ) )
        {
          newList.Add( new FastColoredTextBoxNS.AutocompleteItem( entry ) { ToolTipTitle = entry, ToolTipText = "sowas" } );
        }
        if ( ( wordBelow.StartsWith( "." ) )
        &&   ( entry.StartsWith( zone + "." + wordBelow.Substring( 1 ), StringComparison.CurrentCultureIgnoreCase ) ) )
        {
          newList.Add( new FastColoredTextBoxNS.AutocompleteItem( entry.Substring( zone.Length ) ) { ToolTipTitle = entry.Substring( zone.Length ), ToolTipText = "tool tip text" } );
        }
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

        ///editSource.AutoComplete.DropRestOfWord = true;
        ///editSource.AutoComplete.IsCaseSensitive = false;
        //editSource.AutoComplete.Show( wordBelow.Length, newList );

        if ( wordBelow.StartsWith( "." ) )
        {
          AutoComplete.MinFragmentLength = 1;
        }
        else
        {
          AutoComplete.MinFragmentLength = 2;
        }
        AutoComplete.AutoSize = true;
        AutoComplete.Items.SetAutocompleteItems( newList );
        AutoComplete.Show( true );
      }*/
      //AutoComplete.Show( true );
    }



    void RefreshAutoComplete()
    {
      if ( editSource.InvokeRequired )
      {
        editSource.Invoke( new C64Studio.MainForm.ParameterLessCallback( RefreshAutoComplete ) );
        return;
      }

      var   newList = new List<FastColoredTextBoxNS.AutocompleteItem>();

      foreach ( var entry in DocumentInfo.KnownKeywords )
      {
        if ( entry.Token.StartsWith( C64Studio.Parser.ASMFileParser.InternalLabelPrefix ) )
        {
          // skip internal labels in autocomplete
          continue;
        }
        newList.Add( new FastColoredTextBoxNS.AutocompleteItem( entry.Token ) { ToolTipTitle = entry.ToolTipTitle, ToolTipText = entry.ToolTipText } );
        //newList.Add( new FastColoredTextBoxNS.AutocompleteItem( entry.Token.Substring( zone.Length ) ) { ToolTipTitle = entry.ToolTipTitle, ToolTipText = entry.ToolTipText } );
      }
      GR.Collections.Set<string>    uniqueKeys = DocumentInfo.KnownTokens.GetUniqueKeys();
      foreach ( string entry in uniqueKeys )
      {
        if ( entry.StartsWith( C64Studio.Parser.ASMFileParser.InternalLabelPrefix ) )
        {
          // skip internal labels in autocomplete
          continue;
        }
        newList.Add( new FastColoredTextBoxNS.AutocompleteItem( entry ) { ToolTipTitle = entry, ToolTipText = "sowas" } );
        //newList.Add( new FastColoredTextBoxNS.AutocompleteItem( entry.Substring( zone.Length ) ) { ToolTipTitle = entry.Substring( zone.Length ), ToolTipText = "tool tip text" } );
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
        //separatorCommenting.Visible = true;
      }
      else
      {
        commentSelectionToolStripMenuItem.Enabled = false;
        uncommentSelectionToolStripMenuItem.Enabled = false;
        //separatorCommenting.Visible = false;
      }
    }



    void contextSource_Opened( object sender, EventArgs e )
    {
      System.Drawing.Point mousePos = editSource.PointToClient( contextSource.Location );

      int position            = editSource.PointToPosition( mousePos );
      m_ContextMenuLineIndex  = editSource.PositionToPlace( position ).iLine;
      m_ContextMenuPosition   = position;

      bool    showOpenFile = false;
      string    lineBelow = editSource.Lines[m_ContextMenuLineIndex].Trim().ToUpper();
      if ( ( lineBelow.StartsWith( "!SOURCE" ) )
      ||   ( lineBelow.StartsWith( "!SRC" ) ) )
      {
        string    fileName = lineBelow.Substring( 4 ).Trim();
        if ( lineBelow.StartsWith( "!SOURCE" ) )
        {
          fileName = lineBelow.Substring( 7 ).Trim();
        }

        if ( ( fileName.Length > 2 )
        &&   ( fileName.StartsWith( "\"" ) )
        &&   ( fileName.EndsWith( "\"" ) ) )
        {
          showOpenFile = true;
          openFileToolStripMenuItem.Text = "Open \"" + fileName.Substring( 1, fileName.Length - 2 ) + "\"";
          m_FilenameToOpen = fileName.Substring( 1, fileName.Length - 2 );
        }
      }
      openFileToolStripMenuItem.Visible = showOpenFile;
      separatorCommenting.Visible = showOpenFile;

      copyToolStripMenuItem.Enabled = ( editSource.SelectionLength != 0 );
      cutToolStripMenuItem.Enabled = copyToolStripMenuItem.Enabled;
      pasteToolStripMenuItem.Enabled = System.Windows.Forms.Clipboard.ContainsText();
    }



    public override string GetContent()
    {
      return editSource.Text;
    }



    public override bool Load()
    {
      if ( DocumentInfo.DocumentFilename == null )
      {
        return false;
      }
      try
      {
        m_InsertingText = true;
        editSource.Text = System.IO.File.ReadAllText( DocumentInfo.FullPath );
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

      //if ( DocumentInfo.FullPath.EndsWith( "objects.asm" ) )
      //{
        //Debug.Log( "collapsing " + DocumentInfo.CollapsedFoldingBlocks.Count + " folded blocks" );
      //}
      UpdateFoldingBlocks();


      m_InsertingText = true;
      var collapsedBlocks = new GR.Collections.Set<int>( DocumentInfo.CollapsedFoldingBlocks );
      foreach ( int blockStart in collapsedBlocks )
      {
        //Debug.Log( "Trying to collapse for " + DocumentInfo.FullPath + ", line " + blockStart );
        if ( ( blockStart < 0 )
        ||   ( blockStart >= editSource.LinesCount ) )
        {
          // out of bounds
          //Debug.Log( "-out of bounds" );
          continue;
        }
        if ( editSource.TextSource[blockStart].FoldingStartMarker != null )
        {
          editSource.CollapseFoldingBlock( blockStart );
          //Debug.Log( "-ok" );
        }
        else
        {
          //Debug.Log( "-no folding start marker set" );
        }
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



    public override bool SaveAs()
    {
      return SaveCode( true );
    }



    public override bool Save()
    {
      return SaveCode( false );
    }



    private bool SaveCode( bool SaveAs )
    {
      string    saveFilename = "";

      if ( ( DocumentInfo.DocumentFilename == null )
      ||   ( SaveAs ) )
      {
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

        if ( SaveAs )
        {
          saveFilename = saveDlg.FileName;
        }
        else
        {
          if ( DocumentInfo.Project == null )
          {
            DocumentInfo.DocumentFilename = saveDlg.FileName;
          }
          else
          {
            SetDocumentFilename( GR.Path.RelativePathTo( saveDlg.FileName, false, System.IO.Path.GetFullPath( DocumentInfo.Project.Settings.BasePath ), true ) );
            DocumentInfo.Element.Name = System.IO.Path.GetFileName( DocumentInfo.DocumentFilename );
            DocumentInfo.Element.Filename = DocumentInfo.DocumentFilename;
            if ( DocumentInfo.Element.Settings.Count == 0 )
            {
              DocumentInfo.Element.Settings["Default"] = new ProjectElement.PerConfigSettings();
            }
          }
          Text = System.IO.Path.GetFileNameWithoutExtension( DocumentInfo.DocumentFilename ) + "*";
          SetupWatcher();
          saveFilename = DocumentInfo.FullPath;
        }
      }
      else
      {
        saveFilename = DocumentInfo.FullPath;
      }

      if ( !DoSave( saveFilename ) )
      {
        return false;
      }
      if ( !SaveAs )
      {
        SetUnmodified();
      }
      return true;
    }



    private bool DoSave( string Filename )
    {
      try
      {
        StoreFoldedBlocks();
        DisableFileWatcher();

        // trim trailing spaces
        if ( editSource.Lines.Count > 0 )
        {
          /*
          // TODO - das verhunzt UNDO!
          fullRange.StripTrailingSpaces();
           * */
        }
        System.IO.File.WriteAllText( Filename, GetContent() );

        editSource.TextSource.ClearIsChanged();
        editSource.Invalidate();
      }
      catch ( System.IO.IOException ex )
      {
        System.Windows.Forms.MessageBox.Show( "Could not save file " + Filename + ".\r\n" + ex.ToString(), "Could not save file" );
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


      /*
      foreach ( var block in editSource.FoldedBlocks )
      {
        DocumentInfo.CollapsedFoldingBlocks.Add( block.Key );
        Debug.Log( "Store for " + DocumentInfo.FullPath + ", line " + block.Key );
      }*/
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
          editSource[m_CurrentMarkedLineIndex].BackgroundBrush = new System.Drawing.SolidBrush( System.Drawing.Color.FromArgb( (int)Core.Settings.SyntaxColoring[Types.ColorableElement.CURRENT_DEBUG_LINE].BGColor ) );
        }
        else
        {
          Debug.Log( "Line Index out of bounds!" );
        }
      }
    }



    public override void SetCursorToLine( int Line, bool SetFocus )
    {
      if ( SetFocus )
      {
        editSource.Focus();
      }

      editSource.Navigate( Line );
      ///editSource.Caret.Goto( editSource.Lines[Line].StartPosition );
      ///editSource.Scrolling.ScrollToCaret();
      CenterOnCaret();
    }



    private void runToCursorToolStripMenuItem_Click( object sender, EventArgs e )
    {
      Core.MainForm.ApplyFunction( C64Studio.Types.Function.DEBUG_RUN_TO );
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
      int position = Position;
      string currentLine = editSource.ReTabifyLine( editSource.Lines[LineIndex], editSource.TabLength );
      // TODO - bloat Tabs again
      int posX = position - editSource.PlaceToPosition( new FastColoredTextBoxNS.Place( 0, LineIndex ) );
      if ( posX < 0 )
      {
        return "";
      }

      string wordBelow = "";

      string tokenAllowedChars = ".ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";
      if ( DocumentInfo.Element != null )
      {
        // TODO - should not be defined here
        switch ( DocumentInfo.Element.AssemblerType )
        {
          case Types.AssemblerType.PDS:
          case Types.AssemblerType.AUTO:
            tokenAllowedChars = "!ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß.";
            break;
          case Types.AssemblerType.DASM:
            tokenAllowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß.";
            break;
          default:
            tokenAllowedChars = ".ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";
            break;
        }
      }

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
        &&   ( Parser.ASMFileInfo.AssemblerSettings.Macros.ContainsKey( potentialPseudoOp.ToUpper() ) ) )
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
        int errorPos = -1;
        List<Types.TokenInfo> tokens = Core.Compiling.ParserASM.ParseTokenInfo( watchedSelection, 0, watchedSelection.Length, out errorPos );

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

      string zone = debugFileInfo.FindZoneFromDocumentLine( DocumentInfo.FullPath, lineIndex );

      // TODO - determine if known label/var
      int     result = -1;
      int     bytesGiven = 0;
      bool    failed = true;

      if ( Core.Compiling.ParserASM.ParseLiteralValue( wordBelow, out failed, out result, out bytesGiven ) )
      {
        WatchEntry entry    = new WatchEntry();
        entry.Name          = wordBelow;
        entry.SizeInBytes   = bytesGiven;
        entry.Type          = WatchEntry.DisplayType.HEX;
        entry.Address       = result;
        entry.IndexedX      = indexedX;
        entry.IndexedY      = indexedY;
        entry.DisplayMemory = true;
        entry.LiteralValue  = true;

        Debug.Log( "Address for " + wordBelow + " determined as " + entry.Address );

        Core.MainForm.AddWatchEntry( entry );
        return;
      }

      Types.SymbolInfo tokenInfo = debugFileInfo.TokenInfoFromName( wordBelow, zone );
      if ( tokenInfo != null )
      {
        WatchEntry entry = new WatchEntry();
        entry.Name        = wordBelow;
        entry.SizeInBytes = 1;
        entry.Type        = WatchEntry.DisplayType.HEX;
        entry.Address     = tokenInfo.AddressOrValue;
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



    private string FindZoneFromLine( int LineIndex )
    {
      Types.ASM.FileInfo debugFileInfo = Core.Navigating.DetermineASMFileInfo( DocumentInfo );
      if ( debugFileInfo == null )
      {
        return "";
      }
      if ( !debugFileInfo.IsDocumentPart( DocumentInfo.FullPath ) )
      {
        debugFileInfo = DocumentInfo.ASMFileInfo;
      }

      string  zone = "";

      while ( LineIndex >= 0 )
      {
        int     globalLineIndex = -1;
        if ( debugFileInfo.FindGlobalLineIndex( LineIndex, DocumentInfo.FullPath, out globalLineIndex ) )
        {
          if ( debugFileInfo.LineInfo.ContainsKey( globalLineIndex ) )
          {
            Types.ASM.LineInfo lineInfo = debugFileInfo.LineInfo[globalLineIndex];

            zone = lineInfo.Zone;
            break;
          }
        }
        else
        {
          return "";
        }
        --LineIndex;
      }
      return zone;
    }



    private string FindLocalLabelFromLine( int LineIndex )
    {
      Types.ASM.FileInfo debugFileInfo = Core.Navigating.DetermineASMFileInfo( DocumentInfo );
      if ( debugFileInfo == null )
      {
        return "";
      }

      int closestLine = -1;
      Types.SymbolInfo possibleLabel = null;
      int globalLineIndex = -1;

      if ( !debugFileInfo.FindGlobalLineIndex( LineIndex, DocumentInfo.FullPath, out globalLineIndex ) )
      {
        return "";
      }

      foreach ( Types.SymbolInfo symbol in debugFileInfo.Labels.Values )
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
      string wordBelow = FindWordFromPosition( m_ContextMenuPosition, m_ContextMenuLineIndex );
      string zone = FindZoneFromLine( m_ContextMenuLineIndex );

      Core.Navigating.GotoDeclaration( DocumentInfo, wordBelow, zone );
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



    public string FindZoneAtCaretPosition()
    {
      return FindZoneFromLine( CurrentLineIndex );
    }



    public string FindLocalLabelAtCaretPosition()
    {
      return FindLocalLabelFromLine( CurrentLineIndex );
    }



    public void CenterOnCaret()
    {
      // automatically centers
      //editSource.Navigate( CurrentLineIndex );
      editSource.DoSelectionVisible();
    }



    private void openFileToolStripMenuItem_Click( object sender, EventArgs e )
    {
      string docBasePath = GR.Path.RemoveFileSpec( DocumentInfo.FullPath );
      if ( ( string.IsNullOrEmpty( docBasePath ) )
      &&   ( DocumentInfo.Project != null ) )
      {
        docBasePath = DocumentInfo.Project.Settings.BasePath;
      }
      string            fullPath = GR.Path.Append( docBasePath, m_FilenameToOpen );
      ProjectElement    element = DocumentInfo.Project.GetElementByFilename( fullPath );
      if ( element != null )
      {
        DocumentInfo.Project.ShowDocument( element );
      }
      else
      {
        Core.MainForm.OpenFile( fullPath );
      }
    }



    private void showAddressToolStripMenuItem_Click( object sender, EventArgs e )
    {
      string wordBelow = FindWordFromPosition( m_ContextMenuPosition, m_ContextMenuLineIndex );
      string zone = FindZoneFromLine( m_ContextMenuLineIndex );

      Core.MainForm.EnsureFileIsParsed();
      Types.SymbolInfo tokenInfo = DocumentInfo.ASMFileInfo.TokenInfoFromName( wordBelow, zone );
      if ( ( tokenInfo != null )
      &&   ( tokenInfo.Type != Types.SymbolInfo.Types.UNKNOWN ) )
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
    }



    public override void Redo()
    {
      editSource.Redo();
    }



    public override bool CopyPossible
    {
      get
      {
        return !editSource.Selection.IsEmpty;
      }
    }



    public override bool CutPossible
    {
      get
      {
        return !editSource.Selection.IsEmpty;
      }
    }



    public override bool PastePossible
    {
      get
      {
        return true;
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
      SetCursorToLine( cursorLine, true );
    }



    public override void FillContent( string Text )
    {
      editSource.Text = Text;
      SetModified();
    }



    public override void RefreshDisplayOptions()
    {
      // Font
      editSource.Font = new System.Drawing.Font( Core.Settings.SourceFontFamily, Core.Settings.SourceFontSize );

      // Colors
      editSource.Language = FastColoredTextBoxNS.Language.Custom;//.VB;//FastColoredTextBoxNS.Language.Custom;
      editSource.CommentPrefix = ";";

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

      editSource.AllowTabs            = Core.Settings.AllowTabs;
      editSource.ConvertTabsToSpaces  = Core.Settings.TabConvertToSpaces;
      editSource.TabLength            = Core.Settings.TabSize;
      editSource.ShowLineNumbers      = !Core.Settings.ASMHideLineNumbers;

      if ( Core.Settings.ASMShowMiniView )
      {
        documentMap1.Visible = Core.Settings.ASMShowMiniView;
        editSource.Width = documentMap1.Left;
      }
      else
      {
        documentMap1.Visible = Core.Settings.ASMShowMiniView;
        editSource.Width = ClientSize.Width;
      }

      m_BreakpointOffset = 0;
      m_CycleOffset = -1;
      m_ByteSizeOffset = -1;

      int     newPadding = BORDER_MARKER_WIDTH;    // space for marker symbol on left side
      if ( Core.Settings.ASMShowBytes )
      {
        newPadding += BORDER_SIZE_WIDTH;
        m_ByteSizeOffset = BORDER_MARKER_WIDTH;
      }
      if ( Core.Settings.ASMShowCycles )
      {
        newPadding += BORDER_CYCLES_WIDTH;
        m_CycleOffset = BORDER_MARKER_WIDTH;
        if ( Core.Settings.ASMShowBytes )
        {
          m_CycleOffset += BORDER_SIZE_WIDTH;
        }
      }
      editSource.LeftPadding = newPadding;

      //call OnTextChanged for refresh syntax highlighting
      editSource.OnSyntaxHighlight( new FastColoredTextBoxNS.TextChangedEventArgs( editSource.Range ) );

      // update manually set accelerators
      UpdateKeyBinding( C64Studio.Types.Function.DELETE_LINE, FastColoredTextBoxNS.FCTBAction.DeleteLine );
      UpdateKeyBinding( C64Studio.Types.Function.COPY_LINE_DOWN, FastColoredTextBoxNS.FCTBAction.CopyLineDown );
      UpdateKeyBinding( C64Studio.Types.Function.COPY_LINE_UP, FastColoredTextBoxNS.FCTBAction.CopyLineUp );
      UpdateKeyBinding( C64Studio.Types.Function.MOVE_LINE_UP, FastColoredTextBoxNS.FCTBAction.MoveSelectedLinesUp );
      UpdateKeyBinding( C64Studio.Types.Function.MOVE_LINE_DOWN, FastColoredTextBoxNS.FCTBAction.MoveSelectedLinesDown );
    }



    private void UpdateKeyBinding( C64Studio.Types.Function Function, FastColoredTextBoxNS.FCTBAction Action )
    {
      editSource.HotkeysMapping.RemoveAllMappingsForAction( Action );

      var     accelerator = Core.Settings.DetermineAccelerator( Function );
      if ( accelerator != null )
      {
        editSource.HotkeysMapping.Remove( accelerator.Key );
        editSource.HotkeysMapping.Add( accelerator.Key, Action );
      }
    }



    public override void ApplyFunction( C64Studio.Types.Function Function )
    {
      switch ( Function )
      {
        case C64Studio.Types.Function.PRINT:
          editSource.Print( new FastColoredTextBoxNS.PrintDialogSettings 
                                  {
                                    ShowPageSetupDialog = true, ShowPrintDialog = true, ShowPrintPreviewDialog = true 
                                  } );
          break;
        case C64Studio.Types.Function.COMMENT_SELECTION:
          CommentSelection();
          break;
        case C64Studio.Types.Function.UNCOMMENT_SELECTION:
          UncommentSelection();
          break;
      }
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

      C64Studio.Types.SymbolInfo symbol = (C64Studio.Types.SymbolInfo)comboZoneSelector.SelectedItem;

      SetCursorToLine( symbol.LocalLineIndex, true );

      RefreshLocalSymbols();
    }



    private void comboLocalLabelSelector_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( DoNotFollowZoneSelectors )
      {
        return;
      }
      C64Studio.Types.SymbolInfo symbol = (C64Studio.Types.SymbolInfo)comboLocalLabelSelector.SelectedItem;

      SetCursorToLine( symbol.LocalLineIndex, true );
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

      Core.MainForm.CallHelp( wordBelow );
    }



    private void editSource_DragEnter( object sender, DragEventArgs e )
    {
      if ( e.Data.GetDataPresent( DataFormats.FileDrop ) )
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
      if ( m_BreakPoints.ContainsKey( e.LineIndex ) )
      {
        e.Graphics.DrawImage( C64Studio.Properties.Resources.breakpoint, m_BreakpointOffset, e.LineRect.Top );
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
              e.Graphics.DrawString( lineInfo.Opcode.NumCycles.ToString() + "+" + lineInfo.Opcode.NumPenaltyCycles.ToString(), editSource.Font, System.Drawing.SystemBrushes.WindowText, m_CycleOffset, e.LineRect.Top );
            }
            else
            {
              e.Graphics.DrawString( lineInfo.Opcode.NumCycles.ToString(), editSource.Font, System.Drawing.SystemBrushes.WindowText, m_CycleOffset, e.LineRect.Top );
            }
          }
        }
        if ( m_ByteSizeOffset != -1 )
        {
          e.Graphics.DrawString( lineInfo.NumBytes.ToString(), editSource.Font, System.Drawing.SystemBrushes.WindowText, m_ByteSizeOffset, e.LineRect.Top );
        }
      }
    }



    internal void SetLineInfos( Types.ASM.FileInfo FileInfo )
    {
      foreach ( var sourceInfo in FileInfo.SourceInfo )
      {
        if ( GR.Path.IsPathEqual( sourceInfo.Value.FullPath, DocumentInfo.FullPath ) )
        {
          for ( int i = 0; i < sourceInfo.Value.LineCount; ++i )
          {
            // TODO - happens if we edit code while compiling
            if ( ( sourceInfo.Value.LocalStartLine + i < m_LineInfos.Count )
            &&   ( FileInfo.LineInfo.ContainsKey( sourceInfo.Value.GlobalStartLine + i ) ) )
            {
              m_LineInfos[sourceInfo.Value.LocalStartLine + i] = FileInfo.LineInfo[sourceInfo.Value.GlobalStartLine + i];
            }
          }
        }
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
        int errorPos = -1;
        List<Types.TokenInfo> tokens = Core.Compiling.ParserASM.ParseTokenInfo( watchedSelection, 0, watchedSelection.Length, out errorPos );

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

      string  zone = debugFileInfo.FindZoneFromDocumentLine( DocumentInfo.FullPath, lineIndex );
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
      var tokenInfo = DocumentInfo.ASMFileInfo.TokenInfoFromName( wordBelow, zone );
      if ( ( tokenInfo != null )
      &&   ( tokenInfo.AddressOrValue != -1 ) )
      {
        address = tokenInfo.AddressOrValue;
      }
      else if ( DocumentInfo.ASMFileInfo.Labels.ContainsKey( wordBelow ) )
      {
        var labelInfo = DocumentInfo.ASMFileInfo.Labels[wordBelow];

        address = labelInfo.AddressOrValue;
      }
      else if ( debugFileInfo.Labels.ContainsKey( wordBelow ) )
      {
        var labelInfo = debugFileInfo.Labels[wordBelow];

        address = labelInfo.AddressOrValue;
      }

      if ( address != -1 )
      {
        Types.Breakpoint bp = new C64Studio.Types.Breakpoint();

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



    public void MarkTextAsError( int LineIndex, int CharPosStart, int CharLength )
    {
      var range = new FastColoredTextBoxNS.Range( editSource, new FastColoredTextBoxNS.Place( CharPosStart, LineIndex ), new FastColoredTextBoxNS.Place( CharPosStart + CharLength, LineIndex ) );

      range.SetStyle( FastColoredTextBoxNS.StyleIndex.Style10 );
    }



    internal void RemoveAllErrorMarkings()
    {
      editSource.ClearStyleWithoutAffectingFoldingMarkers( FastColoredTextBoxNS.StyleIndex.Style10 );
    }

  }
}
