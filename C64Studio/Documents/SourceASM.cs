using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace C64Studio
{
  public partial class SourceASM : BaseDocument
  {
    private const uint TME_HOVER = 0x00000001;
    private const uint TME_LEAVE = 0x00000002;
    private const uint HOVER_DEFAULT = 0xFFFFFFFF;
     
    [DllImport("user32.dll")]
    public static extern bool TrackMouseEvent(ref TRACKMOUSEEVENT lpEventTrack);
     
    public struct TRACKMOUSEEVENT {
        public uint cbSize;
        public uint dwFlags;
        public IntPtr hwndTrack;
        public uint dwHoverTime;
    }

    private ScintillaNET.Marker               m_CurrentLineMarker = null;
    int                                       m_CurrentMarkedLineIndex = -1;
    int                                       m_ContextMenuLineIndex = -1;
    int                                       m_ContextMenuPosition = -1;
    string                                    m_FilenameToOpen = "";
    System.Windows.Forms.ToolTip              m_ToolTip = new System.Windows.Forms.ToolTip();
    System.Drawing.Point                      m_LastTooltipPos = new System.Drawing.Point();
    string                                    m_LastTooltipText;
    private GR.Collections.Map<int,Types.Breakpoint>    m_BreakPoints = new GR.Collections.Map<int,C64Studio.Types.Breakpoint>();

    public bool                               DoNotFollowZoneSelectors = false;
    private bool                              m_TogglingBreakpointsAllowed = true;

    private Parser.ASMFileParser              Parser = new C64Studio.Parser.ASMFileParser();



    public override int CursorLine
    {
      get
      {
        return editSource.Caret.LineNumber;
      }
    }


    public SourceASM( MainForm MainForm )
    {
      this.MainForm = MainForm;

      DocumentInfo.Type = ProjectElement.ElementType.ASM_SOURCE;
      DocumentInfo.UndoManager.MainForm = MainForm;

      m_IsSaveable = true;
      InitializeComponent();

      Parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      m_CurrentLineMarker = editSource.Markers[2];
      m_CurrentLineMarker.Symbol = ScintillaNET.MarkerSymbol.Background;
      //m_CurrentLineMarker.BackColor = System.Drawing.Color.Yellow;
      ApplySyntaxColoring( m_CurrentLineMarker, C64Studio.Types.SyntaxElement.CURRENT_DEBUG_LINE );

      contextSource.Opening += new CancelEventHandler( contextSource_Opening );

      RemoveCommandBinding( ScintillaNET.BindableCommand.LineCut );
      SetKeyBinding( ScintillaNET.BindableCommand.LineDelete, MainForm.Settings.DetermineAccelerator( C64Studio.Types.Function.DELETE_LINE ) );
      SetKeyBinding( ScintillaNET.BindableCommand.ShowFind, MainForm.Settings.DetermineAccelerator( C64Studio.Types.Function.FIND ) );
      SetKeyBinding( ScintillaNET.BindableCommand.FindNext, MainForm.Settings.DetermineAccelerator( C64Studio.Types.Function.FIND_NEXT ) );
      SetKeyBinding( ScintillaNET.BindableCommand.ShowReplace, MainForm.Settings.DetermineAccelerator( C64Studio.Types.Function.FIND_REPLACE ) );
      SetKeyBinding( ScintillaNET.BindableCommand.Print, MainForm.Settings.DetermineAccelerator( C64Studio.Types.Function.PRINT ) );

      editSource.Font = new System.Drawing.Font( MainForm.Settings.SourceFontFamily, MainForm.Settings.SourceFontSize );
      editSource.Lexing.Lexer = ScintillaNET.Lexer.Asm;
      string wordChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_äöüÄÖÜß!1234567890";
      editSource.Lexing.ReclassifyChars( wordChars.ToCharArray(), ScintillaNET.CharClassification.Word );
      editSource.Lexing.Keywords[0] = "lda sta ldy sty ldx stx rts jmp jsr rti sei cli asl lsr inc dec inx dex iny dey cpx cpy cmp bit bne beq bcc bcs bpl bmi adc sec clc sbc tax tay tya txa pha pla eor and ora ror rol php plp clv cld bvc bvs brk nop txs tsx !byte !by !08 !8 !word !wo !16 !zone !zn !text !tx !source !binary !bi !ct !convtab !align slo rla sre rra sax lax dcp isc anc alr arr xaa axs ahx shy shx tas las sed";
      editSource.KeyDown += new System.Windows.Forms.KeyEventHandler( editSource_KeyDown );
      editSource.KeyPress += new System.Windows.Forms.KeyPressEventHandler( editSource_KeyPress );
      editSource.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler( editSource_PreviewKeyDown );

      editSource.SelectionChanged += new EventHandler( editSource_SelectionChanged );

      editSource.MouseHover += new EventHandler( editSource_MouseHover );

      this.Activated += new EventHandler( SourceASM_Activated );

      editSource.TextDeleted += new EventHandler<ScintillaNET.TextModifiedEventArgs>( editSource_TextDeleted );
      editSource.TextInserted += new EventHandler<ScintillaNET.TextModifiedEventArgs>( editSource_TextInserted );

      /*
      editSource.Lexing.Lexer = ScintillaNET.Lexer.Container;
      editSource.Styles.ClearAll();
       */

      RefreshDisplayOptions();

      //editSource.StyleNeeded += new EventHandler<ScintillaNET.StyleNeededEventArgs>( editSource_StyleNeeded );

      //RefreshDisplayOptions();

      editSource.TextInserted += new EventHandler<ScintillaNET.TextModifiedEventArgs>( editSource_TextChanged );
      editSource.TextDeleted += new EventHandler<ScintillaNET.TextModifiedEventArgs>( editSource_TextChanged );
      editSource.ContextMenu = null;
      editSource.Margins[0].Width = 40;
      editSource.AllowDrop = true;
      //editSource.FileDrop += new EventHandler<ScintillaNET.FileDropEventArgs>( MainForm.MainForm_FileDrop );
      editSource.CharAdded += new EventHandler<ScintillaNET.CharAddedEventArgs>( editSource_CharAdded );
      editSource.MouseEnter += new EventHandler( editSource_MouseEnter );
      editSource.MouseLeave += new EventHandler(editSource_MouseLeave);
      editSource.MouseMove += new System.Windows.Forms.MouseEventHandler( editSource_MouseMove );

      // Breakpoint images
      editSource.Markers[0].SetImage( C64Studio.Properties.Resources.breakpoint );
      editSource.Margins.Margin1.AutoToggleMarkerNumber = 0;
      editSource.Margins.Margin1.Width = 16;
      editSource.Margins.Margin1.IsMarkerMargin = true;
      editSource.Margins.Margin1.IsClickable = true;
      editSource.MarkerChanged += new EventHandler<ScintillaNET.MarkerChangedEventArgs>( editSource_MarkerChanged );

      editSource.Indentation.UseTabs = !MainForm.Settings.TabConvertToSpaces;
      editSource.Indentation.TabWidth = MainForm.Settings.TabSize;

      editSource.AutoCompleteAccepted += new EventHandler<ScintillaNET.AutoCompleteAcceptedEventArgs>( editSource_AutoCompleteAccepted );

      m_ToolTip.Active = true;
      m_ToolTip.SetToolTip( editSource, "x" );
      m_ToolTip.Popup += new System.Windows.Forms.PopupEventHandler( m_ToolTip_Popup );

      contextSource.Opened += new EventHandler( contextSource_Opened );
    }



    void SourceASM_Activated( object sender, EventArgs e )
    {
    }



    void editSource_StyleNeeded( object sender, ScintillaNET.StyleNeededEventArgs e )
    {
      string rangeText = e.Range.Text;

      string[] lines = rangeText.Split( '\n' );

      Parser.ASMFileParser parser = new C64Studio.Parser.ASMFileParser();

      int curPos = e.Range.Start;
      foreach ( string line in lines )
      {
        /*
        List<Parser.ParserBase.TokenSyntax> tokens = parser.ParseTokensAndSyntax( line );

        foreach ( Parser.ParserBase.TokenSyntax token in tokens )
        {
          editSource.GetRange( curPos, token.Token.Length ).SetStyle( (int)token.Type );
          // pfui pfui wrong!
          curPos += token.Token.Length;
        }*/
      }
      /*
      for ( int lineIndex = e.Range.StartingLine.Number; lineIndex <= e.Range.EndingLine.Number; ++lineIndex )
      {
        int startChar = 0;
        if ( lineIndex == e.Range.StartingLine.Number )
        {
          startChar = e.Range.StartingLine.StartPosition - e.Range.Start;
        }
        int endChar = editSource.Lines[lineIndex].EndPosition;
        if ( lineIndex == e.Range.EndingLine.Number )
        {
          endChar = e.Range.StartingLine.StartPosition - e.Range.Start;
        }
      }*/
    }



    void ApplySyntaxColoring( ScintillaNET.Style Style, Types.SyntaxElement Element )
    {
      Style.ForeColor = GR.Color.Helper.FromARGB( MainForm.Settings.SyntaxColoring[Element].FGColor );
      if ( MainForm.Settings.SyntaxColoring[Element].BGColorAuto )
      {
        Style.BackColor = GR.Color.Helper.FromARGB( MainForm.Settings.SyntaxColoring[Types.SyntaxElement.EMPTY_SPACE].BGColor );
      }
      else
      {
        Style.BackColor = GR.Color.Helper.FromARGB( MainForm.Settings.SyntaxColoring[Element].BGColor );
      }
    }



    void ApplySyntaxColoring( ScintillaNET.Marker Marker, Types.SyntaxElement Element )
    {
      Marker.ForeColor = GR.Color.Helper.FromARGB( MainForm.Settings.SyntaxColoring[Element].FGColor );
      if ( MainForm.Settings.SyntaxColoring[Element].BGColorAuto )
      {
        Marker.BackColor = GR.Color.Helper.FromARGB( MainForm.Settings.SyntaxColoring[Types.SyntaxElement.EMPTY_SPACE].BGColor );
      }
      else
      {
        Marker.BackColor = GR.Color.Helper.FromARGB( MainForm.Settings.SyntaxColoring[Element].BGColor );
      }
    }



    void editSource_TextInserted( object sender, ScintillaNET.TextModifiedEventArgs e )
    {
      if ( e.LinesAddedCount > 0 )
      {
        // move related breakpoints!
        // Breakpoints are automatically adjusted via marker changed event

        int                         insertedAtLine = editSource.Lines.FromPosition( e.Position ).Number;

        GR.Collections.Map<int,Types.Breakpoint>   origBreakpoints = new GR.Collections.Map<int,C64Studio.Types.Breakpoint>( m_BreakPoints );

        foreach ( int breakpointLine in origBreakpoints.Keys )
        {
          if ( breakpointLine >= insertedAtLine )
          {
            editSource.Lines[breakpointLine].DeleteMarker( editSource.Markers[0] );
          }
        }
        foreach ( int breakpointLine in origBreakpoints.Keys )
        {
          if ( breakpointLine >= insertedAtLine )
          {
            editSource.Lines[breakpointLine + e.LinesAddedCount].AddMarker( editSource.Markers[0] );
          }
        }
        /*
        foreach ( int bpl in m_BreakPoints.Keys )
        {
          Debug.Log( "Breakpoint at line " + bpl );
        }*/
      }
    }



    public void RemoveBreakpoint( Types.Breakpoint Breakpoint )
    {
      if ( m_BreakPoints.ContainsKey( Breakpoint.LineIndex ) )
      {
        m_BreakPoints.Remove( Breakpoint.LineIndex );
        editSource.Lines[Breakpoint.LineIndex].DeleteMarker( editSource.Markers[0] );
      }
    }



    void editSource_TextDeleted( object sender, ScintillaNET.TextModifiedEventArgs e )
    {
      if ( e.LinesAddedCount < 0 )
      {
        // move related breakpoints!
        int deletedAtLine = editSource.Lines.FromPosition( e.Position ).Number;

        GR.Collections.Map<int,Types.Breakpoint> origBreakpoints = new GR.Collections.Map<int,C64Studio.Types.Breakpoint>( m_BreakPoints );

        foreach ( int breakpointLine in origBreakpoints.Keys )
        {
          if ( ( breakpointLine >= deletedAtLine )
          &&   ( breakpointLine < deletedAtLine - e.LinesAddedCount ) )
          {
            // BP was deleted!
            RaiseDocEvent( new DocEvent( DocEvent.Type.BREAKPOINT_REMOVED, origBreakpoints[breakpointLine] ) );
          }
          else if ( breakpointLine >= deletedAtLine )
          {
            Types.Breakpoint bpToMove = m_BreakPoints[breakpointLine];
            m_BreakPoints.Remove( breakpointLine );
            m_BreakPoints.Add( breakpointLine + e.LinesAddedCount, bpToMove );
            bpToMove.LineIndex += e.LinesAddedCount;
            RaiseDocEvent( new DocEvent( DocEvent.Type.BREAKPOINT_UPDATED, bpToMove ) );
          }
        }
      }
    }



    public override bool BreakpointToggleable
    {
      set
      {
        m_TogglingBreakpointsAllowed = value;
      }
    }



    public void ToggleBreakpoint( int LineIndex )
    {
      if ( editSource.Lines[LineIndex].GetMarkers().Contains( editSource.Markers[0] ) )
      {
        editSource.Lines[LineIndex].DeleteMarker( editSource.Markers[0] );
      }
      else
      {
        editSource.Lines[LineIndex].AddMarker( editSource.Markers[0] );
      }
      if ( editSource.Lines[LineIndex].GetMarkers().Contains( editSource.Markers[0] ) )
      {
        // break point set
        if ( !m_BreakPoints.ContainsKey( LineIndex ) )
        {
          Types.Breakpoint bp = new C64Studio.Types.Breakpoint();

          bp.DocumentFilename = DocumentInfo.FullPath;
          bp.LineIndex = LineIndex;

          m_BreakPoints.Add( LineIndex, bp );

          int globalLineIndex = 0;

          Types.ASM.FileInfo fileInfo = MainForm.DetermineASMFileInfo( DocumentInfo );

          if ( fileInfo.FindGlobalLineIndex( LineIndex, DocumentInfo.FullPath, out globalLineIndex ) )
          {
            bp.Address = fileInfo.FindLineAddress( globalLineIndex );
            Debug.Log( "Found address " + bp.Address.ToString( "x4" ) );
          }
          RaiseDocEvent( new DocEvent( DocEvent.Type.BREAKPOINT_ADDED, bp ) );
        }
      }
      else
      {
        if ( m_BreakPoints.ContainsKey( LineIndex ) )
        {
          Types.Breakpoint bp = m_BreakPoints[LineIndex];
          m_BreakPoints.Remove( LineIndex );
          RaiseDocEvent( new DocEvent( DocEvent.Type.BREAKPOINT_REMOVED, bp ) );
        }
      }
    }



    void ToggleBreakpointOnEvent( int LineIndex )
    {
      if ( editSource.Lines[LineIndex].GetMarkers().Contains( editSource.Markers[0] ) )
      {
        // break point set
        if ( !m_BreakPoints.ContainsKey( LineIndex ) )
        {
          Types.Breakpoint bp = new C64Studio.Types.Breakpoint();

          bp.DocumentFilename = DocumentInfo.FullPath;
          bp.LineIndex = LineIndex;

          m_BreakPoints.Add( LineIndex, bp );

          int globalLineIndex = 0;

          Types.ASM.FileInfo fileInfo = MainForm.DetermineASMFileInfo( DocumentInfo );

          if ( fileInfo.FindGlobalLineIndex( LineIndex, DocumentInfo.FullPath, out globalLineIndex ) )
          {
            bp.Address = fileInfo.FindLineAddress( globalLineIndex );
            Debug.Log( "Found address " + bp.Address.ToString( "x4" ) );
          }
          RaiseDocEvent( new DocEvent( DocEvent.Type.BREAKPOINT_ADDED, bp ) );
        }
      }
      else
      {
        if ( m_BreakPoints.ContainsKey( LineIndex ) )
        {
          Types.Breakpoint bp = m_BreakPoints[LineIndex];
          m_BreakPoints.Remove( LineIndex );
          RaiseDocEvent( new DocEvent( DocEvent.Type.BREAKPOINT_REMOVED, bp ) );
        }
      }
    }



    void editSource_MarkerChanged( object sender, ScintillaNET.MarkerChangedEventArgs e )
    {
      if ( m_TogglingBreakpointsAllowed )
      {
        ToggleBreakpointOnEvent( e.Line );
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
      UpdateStatusInfo();
    }



    void UpdateStatusInfo()
    {
      string    newInfo = "Row " + ( editSource.Caret.LineNumber + 1 ).ToString() + ", Col " + editSource.GetColumn( editSource.CurrentPos ).ToString();
      if ( editSource.Selection.Length > 0 )
      {
        if ( editSource.Selection.Range.IsMultiLine )
        {
          int   numLines = editSource.Selection.Range.EndingLine.Number - editSource.Selection.Range.StartingLine.Number + 1;
          if ( editSource.Selection.Range.Text.EndsWith( System.Environment.NewLine ) )
          {
            --numLines;
          }
          newInfo += ", " + editSource.Selection.Length.ToString() + " bytes, " + numLines.ToString() + " lines selected";

        }
        else
        {
          newInfo += ", " + editSource.Selection.Length.ToString() + " bytes selected";
        }
      }
      MainForm.statusEditorDetails.Text = newInfo;

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
              if ( ( symbol3.LocalLineIndex <= editSource.Caret.LineNumber )
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
          if ( ( symbol3.LocalLineIndex <= editSource.Caret.LineNumber )
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
      MainForm.m_FindReplace.ClearLastState();
      UpdateStatusInfo();
    }



    void editSource_SelectionChanged( object sender, EventArgs e )
    {
      UpdateStatusInfo();
    }



    void editSource_KeyDown( object sender, System.Windows.Forms.KeyEventArgs e )
    {
      UpdateStatusInfo();
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
      int position = editSource.PositionFromPoint( m_LastTooltipPos.X, m_LastTooltipPos.Y );

      int lineNumber = editSource.Lines.FromPosition( position ).Number;
      string wordBelow = FindWordFromPosition( position, lineNumber );
      if ( wordBelow.Length == 0 )
      {
        m_ToolTip.Hide( editSource );
        return;
      }

      Types.ASM.FileInfo    debugFileInfo = MainForm.DetermineASMFileInfo( DocumentInfo );
      if ( debugFileInfo == null )
      {
        m_ToolTip.Hide( editSource );
        return;
      }
      if ( !debugFileInfo.IsDocumentPart( DocumentInfo.FullPath ) )
      {
        debugFileInfo = MainForm.DetermineLocalASMFileInfo( DocumentInfo );
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
        if ( MainForm.Debugger.FetchValue( tokenInfo.AddressOrValue, out valueBelow ) )
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



    int StringCaseInsensitiveComparison( string s1, string s2 )
    {
      return String.Compare( s1, s2, true );
    }



    public override void OnKnownTokensChanged()
    {
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

      foreach ( string key in keySet )
      {
        List<C64Studio.Types.SymbolInfo>    listValues = DocumentInfo.KnownTokens.GetValues( key, false );
        foreach ( C64Studio.Types.SymbolInfo symbol in listValues )
        {
          if ( ( symbol.Type == C64Studio.Types.SymbolInfo.Types.ZONE )
          &&   ( symbol.DocumentFilename == DocumentInfo.FullPath ) )
          {
            int itemIndex = comboZoneSelector.Items.Add( symbol );
            if ( symbol.Name == currentZone )
            {
              comboZoneSelector.SelectedIndex = itemIndex;
            }
          }
        }
      }
      if ( comboZoneSelector.SelectedIndex == -1 )
      {
        comboZoneSelector.SelectedIndex = 0;
      }
      RefreshLocalSymbols();
    }



    public override void OnKnownKeywordsChanged()
    {
      RefreshLocalSymbols();
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
            if ( ( symbol.DocumentFilename == fullPath )
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
    }



    void editSource_CharAdded( object sender, ScintillaNET.CharAddedEventArgs e )
    {
      // and in this Method you could check, if your Autocompletelist contains the last inserted Characters. If so you call the this.sciDocument.AutoComplete.Show(); Method of the Control
      // and it shows the AutocompleteList with the value that machtes the inserted Characters.

      string curLine = editSource.Lines[editSource.Caret.LineNumber].Text.TrimEnd( new char[]{ '\r', '\n' } );
      int position = editSource.Caret.Position;
      int posX = position - editSource.Lines[editSource.Caret.LineNumber].StartPosition;

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
              List<string>    newList2 = new List<string>();

              newList2.Add( "x" );
              newList2.Add( "y" );

              editSource.AutoComplete.DropRestOfWord = true;
              editSource.AutoComplete.IsCaseSensitive = false;
              editSource.AutoComplete.Show( 0, newList2 );
              return;
            }
          }
          if ( ( tokens[i].Type == C64Studio.Types.TokenInfo.TokenType.SEPARATOR )
          &&   ( tokens[i].Content == "," ) )
          {
            if ( hadOpcodeFirst )
            {
              editSource.AutoComplete.Cancel();
              return;
            }
          }

          if ( posX < tokens[i].StartPos + tokens[i].Length )
          {
            break;
          }
        }
      }

      if ( editSource.PositionIsOnComment( editSource.CurrentPos - 1 ) )
      {
        return;
      }

      int lineIndex = editSource.Lines.FromPosition( editSource.CurrentPos - 1 ).Number;
      string wordBelow = FindWordFromPosition( editSource.CurrentPos - 1, lineIndex );
      string zone = FindZoneFromLine( lineIndex );
      if ( wordBelow.Length == 0 )
      {
        return;
      }
      List<string>    newList = new List<string>();
      foreach ( string entry in DocumentInfo.KnownKeywords )
      {
        if ( entry.StartsWith( wordBelow, StringComparison.CurrentCultureIgnoreCase ) )
        {
          newList.Add( entry );
        }
        if ( ( wordBelow.StartsWith( "." ) )
        &&   ( entry.StartsWith( zone + "." + wordBelow.Substring( 1 ), StringComparison.CurrentCultureIgnoreCase ) ) )
        {
          newList.Add( entry.Substring( zone.Length + 1 ) );
        }
      }
      GR.Collections.Set<string>    uniqueKeys = DocumentInfo.KnownTokens.GetUniqueKeys();
      foreach ( string entry in uniqueKeys )
      {
        if ( entry.StartsWith( wordBelow, StringComparison.CurrentCultureIgnoreCase ) )
        {
          newList.Add( entry );
        }
        if ( ( wordBelow.StartsWith( "." ) )
        &&   ( entry.StartsWith( zone + "." + wordBelow.Substring( 1 ), StringComparison.CurrentCultureIgnoreCase ) ) )
        {
          newList.Add( entry.Substring( zone.Length + 1 ) );
        }
      }
      if ( newList.Count > 0 )
      {
        newList.Sort( StringCaseInsensitiveComparison );
        // remove duplicates
        Int32 index = 0;
        while ( index < newList.Count - 1 )
        {
          if ( newList[index] == newList[index + 1] )
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
          if ( ( String.Compare( newList[0], wordBelow, StringComparison.CurrentCultureIgnoreCase ) == 0 )
          || ( String.Compare( newList[0], zone + "." + wordBelow.Substring( 1 ), StringComparison.CurrentCultureIgnoreCase ) == 0 ) )
          {
            // only have the correct entry
            return;
          }
        }

        editSource.AutoComplete.DropRestOfWord = true;
        editSource.AutoComplete.IsCaseSensitive = false;
        editSource.AutoComplete.Show( wordBelow.Length, newList );
      }
    }



    void editSource_AutoCompleteAccepted( object sender, ScintillaNET.AutoCompleteAcceptedEventArgs e )
    {
      //throw new NotImplementedException();
    }



    private void RemoveCommandBinding( ScintillaNET.BindableCommand Command )
    {
      List<ScintillaNET.KeyBinding>   boundKeys = editSource.Commands.GetKeyBindings( Command );
      foreach ( ScintillaNET.KeyBinding binding in boundKeys )
      {
        editSource.Commands.RemoveBinding( binding.KeyCode, binding.Modifiers, Command );
        //dh.Log( "Remove Binding " + ( binding.KeyCode | binding.Modifiers ) );
      }
    }



    private void SetKeyBinding( ScintillaNET.BindableCommand Command, AcceleratorKey Key )
    {
      if ( ( Key == null )
      ||   ( Key.Key == System.Windows.Forms.Keys.None ) )
      {
        // nothing defined, leave default key
        return;
      }
      RemoveCommandBinding( Command );
      editSource.Commands.AddBinding( Key.Key & System.Windows.Forms.Keys.KeyCode, Key.Key & System.Windows.Forms.Keys.Modifiers, Command );
      //dh.Log( "Set Binding " + Key.Key );
    }



    void contextSource_Opening( object sender, CancelEventArgs e )
    {
      if ( ( MainForm.AppState == Types.StudioState.DEBUGGING_BROKEN )
      ||   ( MainForm.AppState == Types.StudioState.NORMAL ) )
      {
        addToWatchToolStripMenuItem.Visible = true;
        toolStripSeparator2.Visible = true;
      }
      else
      {
        addToWatchToolStripMenuItem.Visible = false;
        toolStripSeparator2.Visible = false;
      }
      if ( editSource.Selection.Length > 0 )
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

      int position            = editSource.PositionFromPoint( mousePos.X, mousePos.Y );
      m_ContextMenuLineIndex  = editSource.Lines.FromPosition( position ).Number;
      m_ContextMenuPosition   = position;

      bool    showOpenFile = false;
      string    lineBelow = editSource.Lines[m_ContextMenuLineIndex].Text.Trim();
      if ( lineBelow.StartsWith( "!source" ) )
      {
        string    fileName = lineBelow.Substring( 7 ).Trim();

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

      copyToolStripMenuItem.Enabled = ( editSource.Selection.Length != 0 );
      cutToolStripMenuItem.Enabled = copyToolStripMenuItem.Enabled;
      pasteToolStripMenuItem.Enabled = System.Windows.Forms.Clipboard.ContainsText();
    }



    void editSource_TextChanged( object sender, ScintillaNET.TextModifiedEventArgs e )
    {
      SetModified();
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
        editSource.Text = System.IO.File.ReadAllText( DocumentInfo.FullPath );
        editSource.UndoRedo.EmptyUndoBuffer();
      }
      catch ( System.Exception ex )
      {
        System.Windows.Forms.MessageBox.Show( "Could not load ASM file " + DocumentInfo.FullPath + ".\r\n" + ex.Message, "Could not load file" );
        return false;
      }
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
        DisableFileWatcher();
        // trim trailing spaces
        if ( editSource.Lines.Count > 0 )
        {
          ScintillaNET.Range fullRange = new ScintillaNET.Range( 0, editSource.Lines[editSource.Lines.Count - 1].EndPosition, editSource );
          // TODO - das verhunzt UNDO!
          fullRange.StripTrailingSpaces();
        }
        System.IO.File.WriteAllText( Filename, GetContent() );
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



    private void editSource_DocumentChanged( object sender, EventArgs e )
    {
      SetModified();
    }



    public override void SetLineMarked( int Line, bool Set )
    {
      if ( m_CurrentMarkedLineIndex != -1 )
      {
        editSource.Lines[m_CurrentMarkedLineIndex].DeleteMarker( m_CurrentLineMarker );
        m_CurrentMarkedLineIndex = -1;
      }
      if ( Set )
      {
        m_CurrentMarkedLineIndex = Line;
        editSource.Lines[m_CurrentMarkedLineIndex].AddMarker( m_CurrentLineMarker );
      }
    }



    public override void SetCursorToLine( int Line, bool SetFocus )
    {
      if ( SetFocus )
      {
        editSource.Focus();
      }
      editSource.Caret.Goto( editSource.Lines[Line].StartPosition );
      editSource.Scrolling.ScrollToCaret();
      CenterOnCaret();
    }



    private void runToCursorToolStripMenuItem_Click( object sender, EventArgs e )
    {
      MainForm.ApplyFunction( C64Studio.Types.Function.DEBUG_RUN_TO );
    }



    public override int CurrentLineIndex
    {
      get
      {
        return editSource.Caret.LineNumber;
      }
    }



    public string FindWordFromPosition( int Position, int LineIndex )
    {
      int position = Position;
      string currentLine = editSource.Lines[LineIndex].Text;
      int posX = position - editSource.Lines[LineIndex].StartPosition;

      string wordBelow = "";

      string tokenAllowedChars = ".ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";
      if ( ( DocumentInfo.Element != null )
      &&   ( DocumentInfo.Element.AssemblerType != C64Studio.Types.AssemblerType.AUTO ) )
      {
        // TODO - should not be defined here
        switch ( DocumentInfo.Element.AssemblerType )
        {
          case C64Studio.Types.AssemblerType.PDS:
            tokenAllowedChars = "!ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß.";
            break;
          case C64Studio.Types.AssemblerType.DASM:
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
      return wordBelow;
    }



    private void addToWatchToolStripMenuItem_Click( object sender, EventArgs e )
    {
      // TODO - check for ,y or ,x
      int     lineIndex = m_ContextMenuLineIndex;
      string  wordBelow = FindWordFromPosition( m_ContextMenuPosition, m_ContextMenuLineIndex );
      bool    indexedX = false;
      bool    indexedY = false;

      if ( ( editSource.Selection.Length > 0 )
      &&   ( editSource.Selection.Range.StartingLine.Number == lineIndex )
      &&   ( editSource.Selection.Range.EndingLine.Number == lineIndex ) )
      {
        // there is a selection on this line
        string watchedSelection = editSource.Selection.Text;

        // allow for single label; label,x; label,y; (label),y; (label),x
        int errorPos = -1;
        List<Types.TokenInfo> tokens = MainForm.ParserASM.ParseTokenInfo( watchedSelection, 0, watchedSelection.Length, out errorPos );

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

      Types.ASM.FileInfo debugFileInfo = MainForm.DetermineASMFileInfo( DocumentInfo );
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

      if ( MainForm.ParserASM.ParseLiteralValue( wordBelow, out failed, out result, out bytesGiven ) )
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

        MainForm.AddWatchEntry( entry );
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

        MainForm.AddWatchEntry( entry );
      }
      else
      {
        System.Windows.Forms.MessageBox.Show( "Could not determine item address of " + wordBelow );
      }
    }



    private string FindZoneFromLine( int LineIndex )
    {
      Types.ASM.FileInfo debugFileInfo = MainForm.DetermineASMFileInfo( DocumentInfo );
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
      Types.ASM.FileInfo debugFileInfo = MainForm.DetermineASMFileInfo( DocumentInfo );
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

      MainForm.GotoDeclaration( DocumentInfo, wordBelow, zone );
      CenterOnCaret();
    }



    public string FindWordAtCaretPosition()
    {
      return FindWordFromPosition( editSource.Caret.Position, editSource.Caret.LineNumber );
    }



    public string FindZoneAtCaretPosition()
    {
      return FindZoneFromLine( editSource.Caret.LineNumber );
    }



    public string FindLocalLabelAtCaretPosition()
    {
      return FindLocalLabelFromLine( editSource.Caret.LineNumber );
    }



    public void CenterOnCaret()
    {
      int     firstLine = editSource.Lines.FirstVisible.Number;
      int     numVisible = editSource.Lines.VisibleCount;

      int     curCaretPos = editSource.Caret.LineNumber;

      int     wantedFirstLine = curCaretPos - numVisible / 2;

      editSource.Scrolling.ScrollBy( wantedFirstLine - firstLine, 0 );
    }



    private void openFileToolStripMenuItem_Click( object sender, EventArgs e )
    {
      string docBasePath = GR.Path.RemoveFileSpec( DocumentInfo.FullPath );
      if ( ( string.IsNullOrEmpty( docBasePath ) )
      &&   ( DocumentInfo.Project != null ) )
      {
        docBasePath = DocumentInfo.Project.Settings.BasePath;
      }
      MainForm.OpenFile( GR.Path.Append( docBasePath, m_FilenameToOpen ) );
    }



    private void showAddressToolStripMenuItem_Click( object sender, EventArgs e )
    {
      string wordBelow = FindWordFromPosition( m_ContextMenuPosition, m_ContextMenuLineIndex );
      string zone = FindZoneFromLine( m_ContextMenuLineIndex );

      MainForm.EnsureFileIsParsed();
      Types.SymbolInfo tokenInfo = DocumentInfo.ASMFileInfo.TokenInfoFromName( wordBelow, zone );
      if ( ( tokenInfo != null )
      &&   ( tokenInfo.Type != Types.SymbolInfo.Types.UNKNOWN ) )
      {
        MainForm.AddToOutput( "Value of " + wordBelow + ": $" + tokenInfo.AddressOrValue.ToString( "x4" ) + ", " + tokenInfo.AddressOrValue.ToString() + Environment.NewLine );
      }
      else
      {
        MainForm.AddToOutput( "Could not determine value for " + wordBelow + Environment.NewLine );
      }
    }



    public override bool UndoPossible
    {
      get
      {
        return editSource.UndoRedo.CanUndo;
      }
    }



    public override bool RedoPossible
    {
      get
      {
        return editSource.UndoRedo.CanRedo;
      }
    }



    public override void Undo()
    {
      editSource.UndoRedo.Undo();
    }



    public override void Redo()
    {
      editSource.UndoRedo.Redo();
    }



    public override bool CopyPossible
    {
      get
      {
        return editSource.Clipboard.CanCopy;
      }
    }



    public override bool CutPossible
    {
      get
      {
        return editSource.Clipboard.CanCut;
      }
    }



    public override bool PastePossible
    {
      get
      {
        return editSource.Clipboard.CanPaste;
      }
    }



    public override void Copy()
    {
      editSource.Clipboard.Copy();
    }


    public override void Cut()
    {
      editSource.Clipboard.Cut();
    }



    public override void Paste()
    {
      editSource.Clipboard.Paste();
    }



    public override GR.Memory.ByteBuffer DisplayDetails()
    {
      GR.Memory.ByteBuffer      displayData = new GR.Memory.ByteBuffer();

      displayData.AppendI32( editSource.Caret.LineNumber );
      displayData.AppendI32( editSource.Caret.Position );

      return displayData;
    }



    public override void ApplyDisplayDetails( GR.Memory.ByteBuffer Buffer )
    {
      GR.IO.MemoryReader binReader = Buffer.MemoryReader();

      SetCursorToLine( binReader.ReadInt32(), true );
    }



    public override void FillContent( string Text )
    {
      editSource.Text = Text;
      SetModified();
    }



    public override void RefreshDisplayOptions()
    {
      // 0 = some chars?
      // 1 = comment
      // 2 = numeric literal
      // 3 = string literal
      // 4 = operator
      // 5 = constants
      // 6 = keywords

      /*
      [BRACEBAD, 35]}	System.Collections.Generic.KeyValuePair<string,int>
      [BRACELIGHT, 34]}	System.Collections.Generic.KeyValuePair<string,int>
      [CALLTIP, 38]}	System.Collections.Generic.KeyValuePair<string,int>
      [CONTROLCHAR, 36]}	System.Collections.Generic.KeyValuePair<string,int>
      [DEFAULT, 32]}	System.Collections.Generic.KeyValuePair<string,int>
      [LINENUMBER, 33]}	System.Collections.Generic.KeyValuePair<string,int>
      [DOCUMENT_DEFAULT, 0]}	System.Collections.Generic.KeyValuePair<string,int>
      [COMMENT, 1]}	System.Collections.Generic.KeyValuePair<string,int>
      [NUMBER, 2]}	System.Collections.Generic.KeyValuePair<string,int>
      [STRING, 3]}	System.Collections.Generic.KeyValuePair<string,int>
      [OPERATOR, 4]}	System.Collections.Generic.KeyValuePair<string,int>
      [IDENTIFIER, 5]}	System.Collections.Generic.KeyValuePair<string,int>
      [CPUINSTRUCTION, 6]}	System.Collections.Generic.KeyValuePair<string,int>
      [MATHINSTRUCTION, 7]}	System.Collections.Generic.KeyValuePair<string,int>
      [REGISTER, 8]}	System.Collections.Generic.KeyValuePair<string,int>
      [DIRECTIVE, 9]}	System.Collections.Generic.KeyValuePair<string,int>
      [DIRECTIVEOPERAND, 10]}	System.Collections.Generic.KeyValuePair<string,int>
      [COMMENT	Block, 11]}	System.Collections.Generic.KeyValuePair<string,int>
      [CHARACTER, 12]}	System.Collections.Generic.KeyValuePair<string,int>
      [STRINGEOL, 13]}	System.Collections.Generic.KeyValuePair<string,int>
      [EXTINSTRUCTION, 14]}	System.Collections.Generic.KeyValuePair<string,int>
       */
      editSource.Lexing.Lexer = ScintillaNET.Lexer.Asm;
      string wordChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_äöüÄÖÜß!1234567890";
      editSource.Lexing.ReclassifyChars( wordChars.ToCharArray(), ScintillaNET.CharClassification.Word );
      editSource.Lexing.Keywords[0] = "lda sta ldy sty ldx stx rts jmp jsr rti sei cli asl lsr inc dec inx dex iny dey cpx cpy cmp bit bne beq bcc bcs bpl bmi adc sec clc sbc tax tay tya txa pha pla eor and ora ror rol php plp clv cld bvc bvs brk nop txs tsx !byte !by !08 !8 !word !wo !16 !zone !zn !text !tx !source !binary !bi !ct !convtab !align slo rla sre rra sax lax dcp isc anc alr arr xaa axs ahx shy shx tas las sed";

      // Font
      editSource.Font = new System.Drawing.Font( MainForm.Settings.SourceFontFamily, MainForm.Settings.SourceFontSize );

      // Colors
      /*
      // manual lexing
      ApplySyntaxColoring( editSource.Styles[0], Types.SyntaxElement.CODE );
      ApplySyntaxColoring( editSource.Styles[1], Types.SyntaxElement.CODE );
      ApplySyntaxColoring( editSource.Styles[2], Types.SyntaxElement.LITERAL_STRING );
      ApplySyntaxColoring( editSource.Styles[3], Types.SyntaxElement.LITERAL_NUMBER );
      ApplySyntaxColoring( editSource.Styles[4], Types.SyntaxElement.LABEL );
      ApplySyntaxColoring( editSource.Styles[5], Types.SyntaxElement.COMMENT );
      ApplySyntaxColoring( editSource.Styles[6], Types.SyntaxElement.MACRO );
      ApplySyntaxColoring( editSource.Styles[7], Types.SyntaxElement.CURRENT_DEBUG_LINE );
      */

      /*
      ApplySyntaxColoring( editSource.Styles[0], Types.SyntaxElement.CODE );
      ApplySyntaxColoring( editSource.Styles[1], Types.SyntaxElement.MACRO );
      ApplySyntaxColoring( editSource.Styles[2], Types.SyntaxElement.LITERAL_NUMBER );
      ApplySyntaxColoring( editSource.Styles[3], Types.SyntaxElement.LITERAL_STRING );
      ApplySyntaxColoring( editSource.Styles.Default, Types.SyntaxElement.CODE );
      ApplySyntaxColoring( editSource.Styles[7], Types.SyntaxElement.MACRO );*/

      // working
      ApplySyntaxColoring( editSource.Styles[1], Types.SyntaxElement.COMMENT );
      ApplySyntaxColoring( editSource.Styles[11], Types.SyntaxElement.COMMENT );
      ApplySyntaxColoring( editSource.Styles[2], Types.SyntaxElement.LITERAL_NUMBER );
      ApplySyntaxColoring( editSource.Styles[3], Types.SyntaxElement.LITERAL_STRING );
      ApplySyntaxColoring( editSource.Styles[5], Types.SyntaxElement.LABEL );
      ApplySyntaxColoring( editSource.Styles[6], Types.SyntaxElement.CODE );
      ApplySyntaxColoring( editSource.Styles[0], Types.SyntaxElement.EMPTY_SPACE );
      ApplySyntaxColoring( editSource.Styles[32], Types.SyntaxElement.EMPTY_SPACE );
      ApplySyntaxColoring( editSource.Styles[4], Types.SyntaxElement.OPERATOR );

      ApplySyntaxColoring( editSource.Styles[14], Types.SyntaxElement.MACRO );

      for ( int i = 0; i < editSource.Styles.Max.Index; ++i )
      {
        editSource.Styles[i].Font = editSource.Font;
      }

      editSource.Lexing.Colorize();
    }



    public override void ApplyFunction( C64Studio.Types.Function Function )
    {
      switch ( Function )
      {
        case C64Studio.Types.Function.FIND:
          //editSource.FindReplace.ShowFind();
          break;
        case C64Studio.Types.Function.FIND_REPLACE:
          //editSource.FindReplace.ShowReplace();
          break;
        case C64Studio.Types.Function.PRINT:
          editSource.Printing.Print();
          break;
      }
    }



    private void copyToolStripMenuItem_Click( object sender, EventArgs e )
    {
      editSource.Clipboard.Copy();
    }



    private void cutToolStripMenuItem_Click( object sender, EventArgs e )
    {
      editSource.Clipboard.Cut();
    }



    private void pasteToolStripMenuItem_Click( object sender, EventArgs e )
    {
      editSource.Clipboard.Paste();
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
      if ( editSource.Selection.Length == 0 )
      {
        return;
      }
      int startLine = editSource.Selection.Range.StartingLine.Number;
      int endLine = editSource.Selection.Range.EndingLine.Number;
      for ( int i = startLine; i <= endLine; ++i )
      {
        editSource.InsertText( editSource.Lines[i].StartPosition, ";" );
      }
      SetModified();
    }



    private void uncommentSelectionToolStripMenuItem_Click( object sender, EventArgs e )
    {
      if ( editSource.Selection.Length == 0 )
      {
        return;
      }
      int startLine = editSource.Selection.Range.StartingLine.Number;
      int endLine = editSource.Selection.Range.EndingLine.Number;
      bool modified = false;
      for ( int i = endLine; i >= startLine; --i )
      {
        if ( editSource.Lines[i].Text.StartsWith( ";" ) )
        {
          editSource.Lines[i].Text = editSource.Lines[i].Text.Substring( 1 ).TrimEnd();
          modified = true;
        }
      }
      if ( modified )
      {
        SetModified();
      }
    }



    private void helpToolStripMenuItem_Click( object sender, EventArgs e )
    {
      string wordBelow = FindWordFromPosition( m_ContextMenuPosition, m_ContextMenuLineIndex );

      MainForm.CallHelp( wordBelow );
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
            MainForm.OpenFile( file );
          }
        }
      }
      catch ( Exception ex )
      {
        Debug.Log( "Error in DragDrop function: " + ex.Message );
      }
    }

  }
}
