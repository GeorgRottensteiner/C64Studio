using C64Studio.CustomRenderer;
using C64Studio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using static C64Studio.Parser.BasicFileParser;
using GR.IO;
using System.Linq;
using C64Models.BASIC;
using GR.Image;
using RetroDevStudioModels;



namespace C64Studio
{
  public partial class SourceBasicEx : CompilableDocument
  {
    int                                       m_CurrentMarkedLineIndex = -1;
    string                                    m_FilenameToOpen = "";
    System.Windows.Forms.ToolTip              m_ToolTip = new System.Windows.Forms.ToolTip();
    System.Drawing.Point                      m_LastTooltipPos = new System.Drawing.Point();
    bool                                      m_StringEnterMode = false;
    bool                                      m_LabelMode = false;
    bool                                      m_SymbolMode = false;
    public bool                               m_LowerCaseMode = false;
    System.Windows.Forms.Keys m_ControlKeyReplacement = System.Windows.Forms.Keys.Tab;
    System.Windows.Forms.Keys m_CommodoreKeyReplacement = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.ControlKey;
    private FastColoredTextBoxNS.AutocompleteMenu   AutoComplete = null;
    private FastColoredTextBoxNS.Style[]      m_TextStyles = new FastColoredTextBoxNS.Style[(int)Types.ColorableElement.LAST_ENTRY];
    private System.Text.RegularExpressions.Regex[]    m_TextRegExp = new System.Text.RegularExpressions.Regex[(int)Types.ColorableElement.LAST_ENTRY];

    private string                            m_CurrentHighlightText = null;

    private string                            m_StartAddress = "2049";
    private string                            m_BASICDialectName = null;
    private Dialect                           m_BASICDialect = null;

    private Parser.BasicFileParser            m_Parser = null;
    private bool                              m_InsideLoad = false;



    public override int CursorLine
    {
      get
      {
        return editSource.Selection.Start.iLine;
      }
    }



    public override FastColoredTextBoxNS.FastColoredTextBox SourceControl
    {
      get
      {
        return editSource;
      }
    }



    public SourceBasicEx( StudioCore Core )
    {
      this.Core = Core;

      DocumentInfo.Type = ProjectElement.ElementType.BASIC_SOURCE;
      DocumentInfo.UndoManager.MainForm = Core.MainForm;

      m_BASICDialectName  = Dialect.BASICV2.Name;
      m_BASICDialect      = Dialect.BASICV2;

      m_IsSaveable = true;

      InitializeComponent();

      // high DPI adjusting of controls in toolbar
      var controls = new List<Control>{ btnToggleLabelMode, btnToggleStringEntryMode, btnToggleSymbolMode, btnToggleUpperLowerCase, labelStartAddress, editBASICStartAddress, labelBASICVersion, comboBASICVersion };

      int   curX = btnToggleLabelMode.Left;
      for ( int i = 1; i < controls.Count; ++i )
      {
        curX = curX + controls[i - 1].Width + 10;
        controls[i].Left = curX;
      }

      int   oldHeight = editSource.Height;
      int   newHeight = oldHeight - ( ( comboBASICVersion.Bottom + 5 ) - editSource.Top );
      editSource.Top = comboBASICVersion.Bottom + 5;
      editSource.Height = newHeight;

      foreach ( var dialect in Core.Compiling.BASICDialects )
      {
        comboBASICVersion.Items.Add( new GR.Generic.Tupel<string, Dialect>( dialect.Key, dialect.Value ) );
      }
      editSource.SyntaxHighlighter = new BASICSyntaxHighlighter();
      comboBASICVersion.SelectedIndex = 0;

      btnToggleSymbolMode.Checked = Core.Settings.BASICShowControlCodesAsChars;
      AutoComplete = new FastColoredTextBoxNS.AutocompleteMenu( editSource );

      contextSource.Opening += new CancelEventHandler( contextSource_Opening );

      editSource.AutoIndentChars = false;
      editSource.SelectingWord += EditSource_SelectingWord;

      if ( !Core.Settings.BASICUseNonC64Font )
      {
        editSource.Font = new System.Drawing.Font( Core.MainForm.m_FontC64.Families[0], Core.Settings.SourceFontSize, Core.Settings.SourceFontStyle );
      }
      else
      {
        editSource.Font = new System.Drawing.Font( Core.Settings.BASICSourceFontFamily, Core.Settings.BASICSourceFontSize, Core.Settings.BASICSourceFontStyle );
      }

      editSource.LeftBracket = '(';
      editSource.RightBracket = ')';
      editSource.LeftBracket2 = '\x0';
      editSource.RightBracket2 = '\x0';
      editSource.CommentPrefix = "#";
      //editSource.CommentPrefix = "REM";

      RefreshDisplayOptions();

      editSource.LeftPadding = 40;   // space for marker symbol on left side
      editSource.AllowDrop = true;

      editSource.MouseEnter += new EventHandler( editSource_MouseEnter );
      editSource.MouseLeave += new EventHandler(editSource_MouseLeave);
      editSource.MouseMove += new System.Windows.Forms.MouseEventHandler( editSource_MouseMove );
      editSource.MouseDown += new System.Windows.Forms.MouseEventHandler( editSource_MouseDown );
      editSource.MouseUp += new System.Windows.Forms.MouseEventHandler( editSource_MouseUp );
      editSource.KeyDown += new System.Windows.Forms.KeyEventHandler( editSource_KeyDown );
      editSource.KeyPress += new System.Windows.Forms.KeyPressEventHandler( editSource_KeyPress );
      editSource.KeyUp += new System.Windows.Forms.KeyEventHandler( editSource_KeyUp );
      editSource.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler( editSource_PreviewKeyDown );
      editSource.TextChanged += new EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>( editSource_TextChanged );
      editSource.SelectionChangedDelayed += editSource_SelectionChangedDelayed;

      editSource.PreferredLineWidth = 80;


      editSource.KeyPressing += EditSource_KeyPressing;
      editSource.ZoomChanged += EditSource_ZoomChanged;

      editSource.TabLength = Core.Settings.TabSize;

      m_ToolTip.Active = true;
      m_ToolTip.SetToolTip( editSource, "x" );
      m_ToolTip.Popup += new System.Windows.Forms.PopupEventHandler( m_ToolTip_Popup );

      contextSource.Opened += new EventHandler( contextSource_Opened );

      m_StartAddress = "2049";
      editBASICStartAddress.Text = "2049";
    }



    private void EditSource_ZoomChanged( object sender, EventArgs e )
    {
      editSource.LeftPadding = (int)( 40 * DPIHandler.DPIX / 96.0f );

      float     fontSize = editSource.Font.Size;
      // i have no idea why +7
      fontSize *= 1.6f;

      editSource.CharHeight = editSource.LineInterval + (int)( fontSize * DPIHandler.DPIY / 96.0f );
    }



    private void editSource_MouseUp( object sender, MouseEventArgs e )
    {
      UpdateStatusInfo();
    }



    private void editSource_MouseDown( object sender, MouseEventArgs e )
    {
      UpdateStatusInfo();
    }



    public int StartAddress
    {
      get
      {
        if ( DocumentInfo.Element != null )
        {
          return GR.Convert.ToI32( DocumentInfo.Element.StartAddress );
        }
        return GR.Convert.ToI32( m_StartAddress );
      }
    }



    public Dialect BASICDialect
    {
      get
      {
        if ( DocumentInfo.Element != null )
        {
          return Core.Compiling.BASICDialects[DocumentInfo.Element.BASICDialect];
        }
        return m_BASICDialect;
      }
    }



    private void EditSource_KeyPressing( object sender, KeyPressEventArgs e )
    {
      if ( e.KeyChar == '"' )
      {
        if ( Core.Settings.BASICAutoToggleEntryMode )
        {
          ToggleStringEntryMode();
        }
      }
    }



    private void EditSource_SelectingWord( object sender, FastColoredTextBoxNS.SelectingWordEventArgs e )
    {
      string    content = editSource.Lines[e.Place.iLine];
      if ( m_LowerCaseMode )
      {
        content = MakeUpperCase( content, Core.Settings.BASICUseNonC64Font );
      }

      var info = Core.Compiling.ParserBasic.PureTokenizeLine( content );
      /*
      int dummyLastLineNumber = -1;
      var info = Core.Compiling.ParserBasic.TokenizeLine( content, e.Place.iLine, ref dummyLastLineNumber );*/

      foreach ( var token in info.Tokens )
      {
        if ( ( token.StartIndex <= e.Place.iChar )
        &&   ( e.Place.iChar < token.StartIndex + token.Content.Length ) )
        {
          editSource.Selection = new FastColoredTextBoxNS.Range( editSource, token.StartIndex, e.Place.iLine, token.StartIndex + token.Content.Length, e.Place.iLine );
          e.Handled = true;
        }
      }
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



    void editSource_TextChanged( object sender, FastColoredTextBoxNS.TextChangedEventArgs e )
    {
      if ( UndoPossible )
      {
        SetModified();
      }
    }



    private void UpdateKeyBinding( C64Studio.Types.Function Function, FastColoredTextBoxNS.FCTBAction Action )
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



    public override void RefreshDisplayOptions()
    {
      base.RefreshDisplayOptions();

      float     fontSize = Core.Settings.SourceFontSize;
      if ( !Core.Settings.BASICUseNonC64Font )
      {
        editSource.Font = new System.Drawing.Font( Core.MainForm.m_FontC64.Families[0], Core.Settings.SourceFontSize, Core.Settings.SourceFontStyle );
      }
      else
      {
        editSource.Font = new System.Drawing.Font( Core.Settings.BASICSourceFontFamily, Core.Settings.BASICSourceFontSize, Core.Settings.BASICSourceFontStyle );
        fontSize = Core.Settings.BASICSourceFontSize;
      }
      // i have no idea why +7
      //fontSize += 7.0f;
      fontSize *= 1.6f;

      editSource.CharHeight = editSource.LineInterval + (int)( fontSize * DPIHandler.DPIY / 96.0f );

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

      editSource.ForeColor = GR.Color.Helper.FromARGB( Core.Settings.FGColor( ColorableElement.CODE ) );

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

      //editSource.CommentPrefix = "REM";
      editSource.CommentPrefix = "#";

      //editSource.Indentation.UseTabs = !Core.Settings.TabConvertToSpaces;
      editSource.TabLength = Core.Settings.TabSize;

      //call OnTextChanged for refresh syntax highlighting
      editSource.OnTextChanged();

      UpdateKeyBinding( C64Studio.Types.Function.COPY, FastColoredTextBoxNS.FCTBAction.Copy );
      UpdateKeyBinding( C64Studio.Types.Function.PASTE, FastColoredTextBoxNS.FCTBAction.Paste );
      UpdateKeyBinding( C64Studio.Types.Function.CUT, FastColoredTextBoxNS.FCTBAction.Cut );
      UpdateKeyBinding( C64Studio.Types.Function.DELETE_LINE, FastColoredTextBoxNS.FCTBAction.DeleteLine );

      UpdateKeyBinding( C64Studio.Types.Function.UNDO, FastColoredTextBoxNS.FCTBAction.Undo );
      UpdateKeyBinding( C64Studio.Types.Function.REDO, FastColoredTextBoxNS.FCTBAction.Redo );

      UpdateKeyBinding( C64Studio.Types.Function.COPY_LINE_DOWN, FastColoredTextBoxNS.FCTBAction.CopyLineDown );
      UpdateKeyBinding( C64Studio.Types.Function.COPY_LINE_UP, FastColoredTextBoxNS.FCTBAction.CopyLineUp );
      UpdateKeyBinding( C64Studio.Types.Function.MOVE_LINE_UP, FastColoredTextBoxNS.FCTBAction.MoveSelectedLinesUp );
      UpdateKeyBinding( C64Studio.Types.Function.MOVE_LINE_DOWN, FastColoredTextBoxNS.FCTBAction.MoveSelectedLinesDown );

      UpdateKeyBinding( C64Studio.Types.Function.FIND_NEXT, FastColoredTextBoxNS.FCTBAction.FindNext );

      UpdateKeyBinding( C64Studio.Types.Function.BOOKMARK_ADD, FastColoredTextBoxNS.FCTBAction.BookmarkLine );
      UpdateKeyBinding( C64Studio.Types.Function.BOOKMARK_DELETE, FastColoredTextBoxNS.FCTBAction.UnbookmarkLine );
      UpdateKeyBinding( C64Studio.Types.Function.BOOKMARK_PREVIOUS, FastColoredTextBoxNS.FCTBAction.GoPrevBookmark );
      UpdateKeyBinding( C64Studio.Types.Function.BOOKMARK_NEXT, FastColoredTextBoxNS.FCTBAction.GoNextBookmark );

      UpdateKeyBinding( Function.NAVIGATE_BACK, FastColoredTextBoxNS.FCTBAction.NavigateBackward );
      UpdateKeyBinding( Function.NAVIGATE_FORWARD, FastColoredTextBoxNS.FCTBAction.NavigateForward );
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

      //editSource.AddStyle( m_TextStyles[(int)Element] );
      editSource.Styles[SyntaxElementStylePrio( Element )] = m_TextStyles[SyntaxElementStylePrio( Element )];

      // empty space
      editSource.BackColor = GR.Color.Helper.FromARGB( Core.Settings.BGColor( Types.ColorableElement.EMPTY_SPACE ) );
      editSource.SelectionColor = GR.Color.Helper.FromARGB( Core.Settings.FGColor( Types.ColorableElement.SELECTED_TEXT ) );
    }



    void editSource_PreviewKeyDown( object sender, System.Windows.Forms.PreviewKeyDownEventArgs e )
    {
      UpdateStatusInfo();
    }



    void editSource_KeyUp( object sender, System.Windows.Forms.KeyEventArgs e )
    {
      UpdateStatusInfo();
    }



    void editSource_KeyPress( object sender, System.Windows.Forms.KeyPressEventArgs e )
    {
      UpdateStatusInfo();
    }



    void editSource_KeyDown( object sender, System.Windows.Forms.KeyEventArgs e )
    {
      UpdateStatusInfo();
    }



    void UpdateStatusInfo()
    {
      string    details = "Line " + ( CursorLine + 1 ).ToString() + ", Row " + editSource.Selection.Start.iChar.ToString();

      if ( !editSource.Selection.IsEmpty )
      {
        if ( editSource.Selection.End.iLine != editSource.Selection.Start.iLine )
        {
          var selRange = new FastColoredTextBoxNS.Range( editSource, editSource.Selection.Start, editSource.Selection.End );
          selRange.Normalize();

          int   numLines = selRange.End.iLine - selRange.Start.iLine + 1;
          if ( ( selRange.Start.iLine >= 0 )
          && ( selRange.End.iLine < editSource.LinesCount ) )
          {
            string    selText = selRange.Text;
            if ( selText.EndsWith( System.Environment.NewLine ) )
            {
              --numLines;
            }
          }
          details += ", " + editSource.SelectionLength.ToString() + " characters, " + numLines.ToString() + " lines selected";

        }
        else
        {
          details += ", " + editSource.SelectionLength.ToString() + " characters selected";
        }
      }
      Core.MainForm.statusEditorDetails.Text = details;
    }

    
    
    void editSource_MouseMove( object sender, System.Windows.Forms.MouseEventArgs e )
    {
      if ( ( Math.Abs( m_LastTooltipPos.X - e.X ) >= 10 )
      ||   ( Math.Abs( m_LastTooltipPos.Y - e.Y ) >= 10 ) )
      {
        // moved too far
        try
        {
          m_ToolTip.Hide( editSource );
        }
        catch ( Exception )
        {
        }
      }

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
      m_LastTooltipPos = editSource.PointToClient( System.Windows.Forms.Cursor.Position );
    }



    private void OpenAutoComplete()
    {
      // and in this Method you could check, if your Autocompletelist contains the last insterted Characters. If so you call the this.sciDocument.AutoComplete.Show(); Method of the Control
      // and it shows the AutocompleteList with the value that machtes the inserted Characters.
      int lineIndex = CursorLine;
      string wordBelow = FindWordFromPosition( editSource.PlaceToPosition( editSource.Selection.Start ) - 1, lineIndex );
      string zone = FindZoneFromLine( lineIndex );
      if ( wordBelow.Length == 0 )
      {
        return;
      }
      List<string>    newList = new List<string>();
      foreach ( var entry in DocumentInfo.KnownKeywords )
      {
        if ( entry.Token.StartsWith( wordBelow ) )
        {
          newList.Add( entry.Token );
        }
        if ( ( wordBelow.StartsWith( "." ) )
        &&   ( entry.Token.StartsWith( zone + "." + wordBelow.Substring( 1 ) ) ) )
        {
          newList.Add( entry.Token.Substring( zone.Length + 1 ) );
        }
      }
      if ( newList.Count > 0 )
      {
        if ( newList.Count == 1 )
        {
          if ( ( newList[0] == wordBelow )
          ||   ( newList[0] == zone + "." + wordBelow.Substring( 1 ) ) )
          {
            // only have the correct entry
            return;
          }
        }

        ///editSource.AutoComplete.DropRestOfWord = true;
        ///editSource.AutoComplete.IsCaseSensitive = false;
        ///editSource.AutoComplete.Show( wordBelow.Length, newList );
        AutoComplete.Items.SetAutocompleteItems( newList );
        AutoComplete.Show( true );
      }
    }



    void contextSource_Opening( object sender, CancelEventArgs e )
    {
    }



    void contextSource_Opened( object sender, EventArgs e )
    {
    }



    public override string GetContent()
    {
      string    content = editSource.Text;
      if ( m_LowerCaseMode )
      {
        content = MakeUpperCase( content, !Core.Settings.BASICUseNonC64Font );
      }

      return content;
    }



    public override bool Load()
    {
      if ( DocumentInfo.DocumentFilename == null )
      {
        return false;
      }
      try
      {
        string basicText = System.IO.File.ReadAllText( DocumentInfo.FullPath );

        // meta data on top?
        int     endOfLine = basicText.IndexOf( '\n' );
        string  firstLine;
        if ( endOfLine != -1 )
        {
          firstLine = basicText.Substring( 0, endOfLine ).Trim();
          if ( firstLine.StartsWith( "#C64Studio.MetaData.BASIC:" ) )
          {
            basicText = basicText.Substring( endOfLine + 1 );
          }
        }
        else
        {
          firstLine = basicText;
          basicText = "";
        }

        if ( firstLine.StartsWith( "#C64Studio.MetaData.BASIC:" ) )
        {
          int     commaPos = firstLine.IndexOf( ',', 26 );
          if ( commaPos != -1 )
          {
            m_StartAddress      = firstLine.Substring( 26, commaPos - 26 );
            m_BASICDialectName = firstLine.Substring( commaPos + 1 );
          }
        }

        // quick compatibility hack with petcat
        basicText = basicText.Replace( "~", "{SHIFT-ARROW UP}" );
        basicText = basicText.Replace( "\\", "{POUND}" );

        if ( !m_SymbolMode )
        {
          basicText = Core.Compiling.ParserBasic.ReplaceAllSymbolsByMacros( basicText );
        }

        if ( DocumentInfo.Element != null )
        {
          editBASICStartAddress.Text = DocumentInfo.Element.StartAddress;
          m_StartAddress = DocumentInfo.Element.StartAddress;

          foreach ( GR.Generic.Tupel<string, Dialect> entry in comboBASICVersion.Items )
          {
            if ( entry.first == DocumentInfo.Element.BASICDialect )
            {
              comboBASICVersion.SelectedItem = entry;
              break;
            }
          }
        }
        else
        {
          editBASICStartAddress.Text = m_StartAddress;
          foreach ( GR.Generic.Tupel<string, Dialect> entry in comboBASICVersion.Items )
          {
            if ( entry.first == m_BASICDialectName )
            {
              comboBASICVersion.SelectedItem = entry;
              break;
            }
          }
        }

        if ( string.IsNullOrEmpty( m_StartAddress ) )
        {
          m_StartAddress = "";
          editBASICStartAddress.Text = "";
        }

        m_LabelMode = IsInLabelMode( basicText );
        m_InsideLoad = true;
        btnToggleLabelMode.Checked = m_LabelMode;

        editSource.Text = basicText;
        editSource.ClearUndo();

        m_InsideLoad = false;
      }
      catch ( System.IO.IOException ex )
      {
        System.Windows.Forms.MessageBox.Show( "Could not load BASIC file " + DocumentInfo.FullPath + ".\r\n" + ex.Message, "Could not load file" );
        return false;
      }
      catch ( System.UnauthorizedAccessException ex )
      {
        System.Windows.Forms.MessageBox.Show( "Could not load BASIC file " + DocumentInfo.FullPath + ".\r\n" + ex.Message, "Could not load file" );
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



    private bool IsInLabelMode( string basicText )
    {
      string[]  lines = basicText.Split( new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries );

      foreach ( var line in lines )
      {
        if ( line.Length == 0 )
        {
          continue;
        }
        if ( char.IsDigit( line[0] ) )
        {
          return false;
        }
        if ( line.TrimStart().ToUpper().StartsWith( "LABEL" ) )
        {
          return true;
        }
      }
      return true;
    }



    public void SetBASICDialect( Dialect Dialect )
    {
      foreach ( GR.Generic.Tupel<string, Dialect> dialect in comboBASICVersion.Items )
      {
        if ( dialect.second == Dialect )
        {
          comboBASICVersion.SelectedItem = dialect;
          return;
        }
      }
    }



    private string FilterString( string Source )
    {
      return Source.Substring( 0, Source.Length - 1 );
    }



    protected override bool QueryFilename( out string Filename )
    {
      Filename = "";

      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Save Basic File as";
      saveDlg.Filter = FilterString( Types.Constants.FILEFILTER_BASIC + Types.Constants.FILEFILTER_ALL );
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
        DisableFileWatcher();

        string    content = GetContent();

        // add "meta data" in front
        string metaData = "#C64Studio.MetaData.BASIC:" + m_StartAddress + "," + m_BASICDialectName + "\r\n";

        System.IO.File.WriteAllText( FullPath, metaData + content );
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



    private void editSource_DocumentChanged( object sender, EventArgs e )
    {
      SetModified();
    }



    public override void SetLineMarked( int Line, bool Set )
    {
      if ( m_CurrentMarkedLineIndex != -1 )
      {
        editSource[m_CurrentMarkedLineIndex].BackgroundBrush = null;
        m_CurrentMarkedLineIndex = -1;
      }
      if ( Set )
      {
        m_CurrentMarkedLineIndex = Line;
        editSource[m_CurrentMarkedLineIndex].BackgroundBrush = new System.Drawing.SolidBrush( System.Drawing.Color.FromArgb( (int)Core.Settings.BGColor( Types.ColorableElement.CURRENT_DEBUG_LINE ) ) );
      }
    }



    public override void SetCursorToLine( int Line, bool SetFocus )
    {
      if ( SetFocus )
      {
        editSource.Focus();
      }
      editSource.Navigate( Line );
    }



    public string FindWordFromPosition( int Position, int LineIndex )
    {
      int position = Position;
      string currentLine = editSource.Lines[LineIndex];
      int posX = position - editSource.PositionToPlace( Position ).iChar;

      string wordBelow = "";

      string tokenAllowedChars = ".ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";

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



    private string FindZoneFromLine( int LineIndex )
    {
      string  zone = "";

      while ( LineIndex >= 0 )
      {
        int     globalLineIndex = -1;
        if ( Core.Compiling.ParserASM.ASMFileInfo.FindGlobalLineIndex( LineIndex, DocumentInfo.FullPath, out globalLineIndex ) )
        {
          if ( Core.Compiling.ParserASM.ASMFileInfo.LineInfo.ContainsKey( globalLineIndex ) )
          {
            Types.ASM.LineInfo lineInfo = Core.Compiling.ParserASM.ASMFileInfo.LineInfo[globalLineIndex];

            zone = lineInfo.Zone;
            break;
          }
        }
        --LineIndex;
      }
      return zone;
    }



    public string FindWordAtCaretPosition()
    {
      return FindWordFromPosition( editSource.PlaceToPosition( editSource.Selection.Start ), editSource.Selection.Start.iLine );
    }



    public string FindZoneAtCaretPosition()
    {
      return FindZoneFromLine( CurrentLineIndex );
    }



    private void openFileToolStripMenuItem_Click( object sender, EventArgs e )
    {
      string docBasePath = GR.Path.RemoveFileSpec( DocumentFilename );
      if ( DocumentInfo.Element != null )
      {
        docBasePath = DocumentInfo.Project.Settings.BasePath;
      }
      Core.MainForm.OpenFile( GR.Path.Append( docBasePath, m_FilenameToOpen ) );
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
      editSource.Paste();
    }



    public override GR.Memory.ByteBuffer DisplayDetails()
    {
      GR.Memory.ByteBuffer      displayData = new GR.Memory.ByteBuffer();

      displayData.AppendI32( CursorLine );
      displayData.AppendI32( editSource.PlaceToPosition( editSource.Selection.Start ) );

      return displayData;
    }



    public override void ApplyDisplayDetails( GR.Memory.ByteBuffer Buffer )
    {
      GR.IO.MemoryReader binReader = Buffer.MemoryReader();

      SetCursorToLine( binReader.ReadInt32(), true );
    }



    [DllImport( "user32.dll" )]
    static extern int MapVirtualKey( uint uCode, uint uMapType );



    public string KeyCodeToUnicode( Keys key )
    {
      byte[] keyboardState = new byte[255];
      bool keyboardStateStatus = GetKeyboardState(keyboardState);

      if ( !keyboardStateStatus )
      {
        return "";
      }

      uint virtualKeyCode = (uint)key;
      uint scanCode = (uint)MapVirtualKey(virtualKeyCode, 0);
      IntPtr inputLocaleIdentifier = GetKeyboardLayout(0);

      StringBuilder result = new StringBuilder();
      ToUnicodeEx( virtualKeyCode, scanCode, keyboardState, result, (int)5, (uint)0, inputLocaleIdentifier );

      // ^ are duplicated on every second press!
      if ( ( result.Length > 1 )
      &&   ( result.ToString().StartsWith( "^" ) ) )
      {
        return result.ToString().Substring( 1 );
      }
      return result.ToString();
    }

    [DllImport( "user32.dll" )]
    static extern bool GetKeyboardState( byte[] lpKeyState );

    [DllImport( "user32.dll" )]
    static extern IntPtr GetKeyboardLayout( uint idThread );

    [DllImport( "user32.dll" )]
    static extern int ToUnicodeEx( uint wVirtKey, uint wScanCode, byte[] lpKeyState, [Out, MarshalAs( UnmanagedType.LPWStr )] StringBuilder pwszBuff, int cchBuff, uint wFlags, IntPtr dwhkl );



    protected override bool ProcessCmdKey( ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData )
    {
      if ( ( keyData == m_ControlKeyReplacement )
      ||   ( keyData == m_CommodoreKeyReplacement ) )
      {
        // we misuse tab as command key, avoid common processing
        return true;
      }

      if ( !m_StringEnterMode )
      {
        if ( keyData == ( Keys.Control | Keys.A ) )
        {
          editSource.SelectAll();
          return true;
        }
        if ( keyData == ( Keys.Control | Keys.Add ) )
        {
          editSource.ChangeFontSize( 2 );
          return true;
        }
        if ( keyData == ( Keys.Control | Keys.Subtract ) )
        {
          editSource.ChangeFontSize( -2 );
          return true;
        }
      }

      System.Windows.Forms.Keys bareKey = keyData & ~( System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.ShiftKey | System.Windows.Forms.Keys.Alt );

      bareKey = keyData;

      bool    controlPushed = false;
      bool    altPushed = false;
      bool    commodorePushed = false;
      bool    shiftPushed = false;
      if ( ( bareKey & System.Windows.Forms.Keys.Shift ) == System.Windows.Forms.Keys.Shift )
      {
        bareKey &= ~System.Windows.Forms.Keys.Shift;
        shiftPushed = true;
      }
      if ( ( bareKey & System.Windows.Forms.Keys.Control ) == System.Windows.Forms.Keys.Control )
      {
        bareKey &= ~System.Windows.Forms.Keys.Control;
        commodorePushed = true;
      }
      if ( ( bareKey & System.Windows.Forms.Keys.Alt ) == System.Windows.Forms.Keys.Alt )
      {
        altPushed = true;
      }
      if ( GR.Win32.KeyboardInfo.GetKeyState( m_ControlKeyReplacement ).IsPressed )
      {
        controlPushed = true;
      }

      if ( ( !controlPushed )
      &&   ( !commodorePushed )
      &&   ( !shiftPushed ) )
      {
        if ( bareKey == System.Windows.Forms.Keys.Left )
        {
          bareKey = System.Windows.Forms.Keys.Right;
          shiftPushed = true;
        }
        if ( bareKey == System.Windows.Forms.Keys.Up )
        {
          bareKey = System.Windows.Forms.Keys.Down;
          shiftPushed = true;
        }
      }

      if ( !m_StringEnterMode )
      {
        // simply insert, no key mapping!
        // need uppercase when lowercase mode is not active!
        if ( //( !m_LowerCaseMode )
        //&&   ( !commodorePushed )
             ( !commodorePushed )
        &&   ( !altPushed ) )
        {
          string  actualChar = KeyCodeToUnicode( keyData );
          if ( ( ( (char)keyData >= 'A' )
          &&     ( (char)keyData <= 'Z' ) )
          ||   ( actualChar == "?" ) )
          {
            if ( shiftPushed )
            {
              if ( actualChar == "?" )
              {
                if ( InsideREM() )
                {
                  editSource.SelectedText = "?";
                  return true;
                }
                if ( m_LowerCaseMode )
                {
                  editSource.SelectedText = MakeLowerCase( "PRINT", Core.Settings.BASICUseNonC64Font );
                }
                else
                {
                  editSource.SelectedText = "PRINT";
                }
                return true;
              }
              // could be a token
              string  leftText = editSource.GetLineText( CursorLine ).Substring( 0, editSource.Selection.Start.iChar );
              if ( m_LowerCaseMode )
              {
                leftText = MakeUpperCase( leftText, Core.Settings.BASICUseNonC64Font );
              }

              if ( ( leftText.Length >= 1 )
              &&   ( leftText[leftText.Length - 1] >= 'A' )
              &&   ( leftText[leftText.Length - 1] <= 'Z' ) )
              {
                leftText = leftText.ToLower() + (char)keyData; 
                foreach ( var opcode in m_Parser.Settings.BASICDialect.Opcodes.Values )
                {
                  if ( ( opcode.ShortCut != null )
                  &&   ( opcode.ShortCut.Length > 0 )
                  &&   ( opcode.ShortCut.Length <= leftText.Length )
                  &&   ( string.Compare( opcode.ShortCut, 0, leftText, leftText.Length - opcode.ShortCut.Length, opcode.ShortCut.Length ) == 0 ) )
                  {
                    // TODO - case!
                    if ( m_LowerCaseMode )
                    {
                      editSource.SelectedText = MakeLowerCase( opcode.Command.Substring( opcode.ShortCut.Length - 1 ), Core.Settings.BASICUseNonC64Font );
                    }
                    else
                    {
                      editSource.SelectedText = opcode.Command.Substring( opcode.ShortCut.Length - 1 );
                    }
                    return true;
                  }
                }
              }
            }

            if ( m_LowerCaseMode )
            {
              if ( actualChar.Length == 1 )
              {
                InsertOrReplaceChar( actualChar[0] );
              }
            }
            else
            {
              InsertOrReplaceChar( char.ToUpper( (char)keyData ) );
            }
            return true;
          }
        }

        var mappedKey = KeyCodeToUnicode( keyData );

        //Debug.Log( "Barekey=" + bareKey + "/keyData = " + keyData + "/(char)keyData=" + (char)keyData + "/(int)bareKey=" + (int)bareKey + "/mappedKey=" + mappedKey );
        // hard coded mapping from ^ to arrow up (power)
        if ( mappedKey == "^" )
        {
          InsertOrReplaceChar( ConstantData.PhysicalKeyInfo[KeyboardKey.KEY_ARROW_UP].Normal.CharValue );
          return true;
        }
        // PI
        if ( mappedKey == "~" )
        {
          InsertOrReplaceChar( ConstantData.PhysicalKeyInfo[KeyboardKey.KEY_ARROW_UP].WithShift.CharValue );
          return true;
        }
        if ( ( (int)bareKey >= 0x30 )
        &&   ( !IsValidKey( mappedKey ) )
        &&   ( !Core.Settings.Accelerators.ContainsKey( keyData ) ) )
        {
          // swallow invalid keys
          //Debug.Log( "-swallowed" );
          return true;
        }
        return base.ProcessCmdKey( ref msg, keyData );
      }
      //Debug.Log( "Key: " + keyData.ToString() + ", Bare Key: " + bareKey.ToString() );

      if ( Core.Settings.BASICKeyMap.KeymapEntryExists( bareKey ) )
      {
        //Debug.Log( "KeyData " + bareKey );

        var key = Core.Settings.BASICKeyMap.GetKeymapEntry( bareKey );

        if ( !ConstantData.PhysicalKeyInfo.ContainsKey( key.KeyboardKey ) )
        {
          Debug.Log( "No physical key info for " + key.KeyboardKey );
        }
        var physKey = ConstantData.PhysicalKeyInfo[key.KeyboardKey];

        C64Character    c64Key = physKey.Normal;
        if ( shiftPushed )
        {
          c64Key = physKey.WithShift;
          if ( c64Key == null )
          {
            c64Key = physKey.Normal;
          }

          if ( c64Key.CharValue == '\"' )
          {
            ToggleStringEntryMode();
          }
          if ( !m_StringEnterMode )
          {
            // BASIC short cut?
            if ( CanKeyTriggerShortCut( physKey.Normal.CharValue, shiftPushed ) )
              /*
            if ( ( physKey.Normal.CharValue >= 'A' )
            &&   ( physKey.Normal.CharValue <= 'Z' ) )*/
            {
              // could be a token
              string  leftText = editSource.GetLineText( CursorLine ).Substring( 0, editSource.Selection.Start.iChar );
              if ( m_LowerCaseMode )
              {
                leftText = MakeUpperCase( leftText, Core.Settings.BASICUseNonC64Font );
              }

              if ( ( leftText.Length >= 1 )
              &&   ( leftText[leftText.Length - 1] >= 'A' )
              &&   ( leftText[leftText.Length - 1] <= 'Z' ) )
              {
                leftText = leftText.ToLower() + physKey.Normal.CharValue;
                foreach ( var opcode in m_Parser.Settings.BASICDialect.Opcodes.Values )
                {
                  if ( ( opcode.ShortCut != null )
                  &&   ( opcode.ShortCut.Length <= leftText.Length )
                  &&   ( string.Compare( opcode.ShortCut, 0, leftText, leftText.Length - opcode.ShortCut.Length, opcode.ShortCut.Length ) == 0 ) )
                  {
                    // TODO - case!
                    if ( m_LowerCaseMode )
                    {
                      editSource.SelectedText = MakeLowerCase( opcode.Command.Substring( opcode.ShortCut.Length - 1 ), Core.Settings.BASICUseNonC64Font );
                    }
                    else
                    {
                      editSource.SelectedText = opcode.Command.Substring( opcode.ShortCut.Length - 1 );
                    }
                    return true;
                  }
                }
              }
            }
          }
        }
        if ( controlPushed )
        {
          c64Key = physKey.WithControl;
          if ( c64Key == null )
          {
            c64Key = physKey.Normal;
          }
        }
        if ( commodorePushed )
        {
          c64Key = physKey.WithCommodore;
          if ( c64Key == null )
          {
            c64Key = physKey.Normal;
          }
        }

        if ( c64Key != null )
        {
          if ( ( ( !m_StringEnterMode )
          &&     ( ( c64Key.Type == KeyType.GRAPHIC_SYMBOL )
          ||       ( c64Key.Type == KeyType.CONTROL_CODE ) ) )
          ||   ( c64Key.Type == KeyType.EDITOR_CONTROL_CODE ) )
          {
            return base.ProcessCmdKey( ref msg, keyData );
          }
          if ( ( m_SymbolMode )
          ||   ( c64Key.Replacements.Count == 0 ) )
          {
            //Debug.Log( "Trying to map unknown token: " + key.ToString() );
            if ( m_LowerCaseMode )
            {
              if ( ( c64Key.LowerCaseDisplayChar >= 0xe041 )
              &&   ( c64Key.LowerCaseDisplayChar <= 0xe05a ) )
              {
                InsertOrReplaceChar( (char)( ( c64Key.LowerCaseDisplayChar & 0xff ) + 0x20 ) );
              }
              else
              {
                InsertOrReplaceChar( c64Key.LowerCaseDisplayChar );
              }
            }
            else
            {
              if ( ( c64Key.CharValue >= 0xe041 )
              &&   ( c64Key.CharValue <= 0xe05a ) )
              {
                InsertOrReplaceChar( (char)( c64Key.CharValue & 0xff ) );
              }
              else
              {
                InsertOrReplaceChar( c64Key.CharValue );
              }
            }
          }
          else
          {
            editSource.SelectedText = "{" + c64Key.Replacements[0] + "}";
          }
          return true;
        }
        return base.ProcessCmdKey( ref msg, keyData );
      }
      else
      {
        //Debug.Log( "-no keymapping found" );
      }
      // swallow unmapped keys that would produce text (or disallowed characters, e.g. small letters)
      return base.ProcessCmdKey( ref msg, keyData );
    }



    private bool InsideREM()
    {
      int     lineIndex = editSource.Selection.Start.iLine;

      int dummyLastLineNumber = -1;
      var lineInfo = m_Parser.TokenizeLine( editSource.Lines[lineIndex], lineIndex, ref dummyLastLineNumber );

      var rem = lineInfo.Tokens.Where( t => m_Parser.IsComment( t ) ).FirstOrDefault();
      return ( rem != null );
    }



    private bool CanKeyTriggerShortCut( char keyData, bool ShiftPushed )
    {
      if ( ( m_BASICDialect.Name == "BASIC Lightning" )
      ||   ( m_BASICDialect.Name == "Laser BASIC" ) )
      {
        return keyData == '.';
      }
      if ( ( ShiftPushed )
      &&   ( (char)keyData >= 'A' )
      &&   ( (char)keyData <= 'Z' ) )
      {
        return true;
      }
      return false;
    }


    private void InsertOrReplaceChar( char Key )
    {
      if ( editSource.IsReplaceMode )
      {
        editSource.Selection.GoRight( true );
        editSource.Selection.Inverse();
      }
      editSource.SelectedText = "" + Key;
    }



    private bool IsValidKey( string MappedKey )
    {
      if ( string.IsNullOrEmpty( MappedKey ) )
      {
        return false;
      }
      // check all keys!
      if ( !ConstantData.CharToC64Char.ContainsKey( MappedKey.ToUpper()[0] ) )
      {
        // the macro key?
        if ( ( MappedKey == "{" )
        ||   ( MappedKey == "_" )
        ||   ( MappedKey == "}" ) )
        {
          return true;
        }
        return false;
      }

      return true;
    }



    private C64Character FindPhysicalKey( KeymapEntry Key, KeyModifier Modifier, char UnicodeChar )
    {
      if ( !ConstantData.PhysicalKeyInfo.ContainsKey( Key.KeyboardKey ) )
      {
        return null;
      }
      var key = ConstantData.PhysicalKeyInfo[Key.KeyboardKey];
      if ( Modifier == KeyModifier.NORMAL )
      {
        return key.Normal;
      }
      if ( Modifier == KeyModifier.SHIFT )
      {
        return key.WithShift;
      }
      if ( Modifier == KeyModifier.COMMODORE )
      {
        return key.WithCommodore;
      }
      if ( Modifier == KeyModifier.CONTROL )
      {
        return key.WithControl;
      }

      return null;
      /*
      foreach ( var key in ConstantData.AllPhysicalKeyInfos )
      {
        if ( ( key.Modifier == Modifier )
        &&   ( key.CharValue == UnicodeChar ) )
        {
          return key;
        }
      }
      return null;*/
    }



    private void btnToggleLabelMode_CheckedChanged( object sender, EventArgs e )
    {
      if ( m_InsideLoad )
      {
        return;
      }

      ToggleLabelMode();
    }




    private bool ToggleLabelMode()
    {
      bool labelMode = !m_LabelMode;

      Core.MainForm.m_CompileResult.ClearMessages();

      string toggledContent;

      if ( !PerformLabelModeToggle( out toggledContent ) )
      {
        btnToggleLabelMode.Checked = false;
        return false;
      }

      editSource.Text = toggledContent;
      m_LabelMode = labelMode;
      return true;
    }



    public bool PerformLabelModeToggle( out string Result )
    {
      bool labelMode = !m_LabelMode;

      var settings = new Parser.BasicFileParser.ParserSettings();
      settings.StripSpaces = Core.Settings.BASICStripSpaces;
      settings.BASICDialect = m_BASICDialect;

      Parser.BasicFileParser parser = new C64Studio.Parser.BasicFileParser( settings, DocumentInfo.FullPath );
      parser.LabelMode = m_LabelMode;

      var compilerConfig = new C64Studio.Parser.CompileConfig() { Assembler = C64Studio.Types.AssemblerType.AUTO };

      string basicSource = editSource.Text;

      Result = basicSource;

      if ( m_LowerCaseMode )
      {
        basicSource = Parser.BasicFileParser.MakeUpperCase( basicSource, !Core.Settings.BASICUseNonC64Font );
      }

      if ( !parser.Parse( basicSource, null, compilerConfig, null ) )
      {
        Core.MainForm.m_CompileResult.UpdateFromMessages( parser, DocumentInfo.Project );
        Core.Navigating.UpdateFromMessages( parser.Messages,
                                            null,
                                            DocumentInfo.Project );
        Core.MainForm.m_CompileResult.Show();
        return false;
      }
      if ( labelMode )
      {
        Result = parser.EncodeToLabels();
      }
      else
      {
        Result = parser.DecodeFromLabels();
      }
      if ( m_SymbolMode )
      {
        Result = Parser.BasicFileParser.ReplaceAllMacrosBySymbols( Result, out bool hadError );
      }
      else
      {
        Result = Core.Compiling.ParserBasic.ReplaceAllSymbolsByMacros( Result );
      }

      if ( parser.Errors > 0 )
      {
        Core.MainForm.m_CompileResult.UpdateFromMessages( parser, DocumentInfo.Project );
        Core.Navigating.UpdateFromMessages( parser.Messages,
                                            null,
                                            DocumentInfo.Project );
        Core.MainForm.m_CompileResult.Show();
        return false;
      }

      if ( m_LowerCaseMode )
      {
        Result = Parser.BasicFileParser.MakeLowerCase( Result, !Core.Settings.BASICUseNonC64Font );
      }
      return true;
    }



    public override void FillContent( string Text, bool KeepCursorPosIntact )
    {
      if ( m_LowerCaseMode )
      {
        Text = MakeLowerCase( Text, Core.Settings.BASICUseNonC64Font );
      }

      int scrollOffset = editSource.VerticalScroll.Value;
      editSource.Navigate( editSource.Selection.Start.iLine );
      editSource.Text = Text;

      if ( KeepCursorPosIntact )
      {
        editSource.VerticalScroll.Value = scrollOffset;
        editSource.UpdateScrollbars();
      }

      SetModified();
    }



    public override void InsertText( string Text )
    {
      editSource.SelectedText = Text;
      SetModified();
    }



    public override bool ApplyFunction( C64Studio.Types.Function Function )
    {
      switch ( Function )
      {
        case Function.PASTE:
          if ( editBASICStartAddress.Focused )
          {
            return false;
          }
          break;
        case C64Studio.Types.Function.FIND:
          ///editSource.FindReplace.ShowFind();
          break;
        case C64Studio.Types.Function.FIND_REPLACE:
          ///editSource.FindReplace.ShowReplace();
          break;
        case C64Studio.Types.Function.PRINT:
          editSource.Print();
          return true;
        case Function.JUMP_TO_LINE:
          JumpToLine();
          return true;
      }
      return false;
    }



    private void JumpToLine()
    {
      var formLine = new FormGotoLine();

      if ( formLine.ShowDialog() == DialogResult.OK )
      {
        editSource.Navigate( formLine.LineNumber - 1 );
      }
    }



    private void renumberToolStripMenuItem_Click( object sender, EventArgs e )
    {
      int     firstLineNumber = 0;
      int     lastLineNumber  = 64000;
      if ( editSource.Selection.Start != editSource.Selection.End )
      {
        int     firstLine = editSource.Selection.Start.iLine;
        int     lastLine  = editSource.Selection.End.iLine;

        if ( firstLine > lastLine )
        {
          int dummy   = firstLine;
          firstLine   = lastLine;
          lastLine    = dummy;
        }

        int prevLine = -1;
        while ( firstLine < lastLine )
        {
          var lineInfo = Core.Compiling.ParserBasic.TokenizeLine( editSource.Lines[firstLine], 0, ref prevLine );
          if ( ( lineInfo != null )
          &&   ( lineInfo.Tokens.Count > 0 )
          &&   ( lineInfo.Tokens[0].TokenType == Token.Type.LINE_NUMBER ) )
          {
            firstLineNumber = GR.Convert.ToI32( lineInfo.Tokens[0].Content );
            break;
          }
          ++firstLine;
        }

        while ( lastLine > firstLine )
        {
          var lineInfo = Core.Compiling.ParserBasic.TokenizeLine( editSource.Lines[lastLine], 0, ref prevLine );
          if ( ( lineInfo != null )
          &&   ( lineInfo.Tokens.Count > 0 )
          &&   ( lineInfo.Tokens[0].TokenType == Token.Type.LINE_NUMBER ) )
          {
            lastLineNumber = GR.Convert.ToI32( lineInfo.Tokens[0].Content );
            break;
          }
          --lastLine;
        }
      }

      FormRenumberBASIC     formRenum = new FormRenumberBASIC( Core, this, m_SymbolMode, firstLineNumber, lastLineNumber );

      formRenum.ShowDialog();
    }



    private void btnToggleSymbolMode_CheckedChanged( object sender, EventArgs e )
    {
      m_SymbolMode = btnToggleSymbolMode.Checked;

      btnToggleSymbolMode.Image = m_SymbolMode ? global::C64Studio.Properties.Resources.toolbar_basic_symbols_enabled : global::C64Studio.Properties.Resources.toolbar_basic_symbols_disabled;

      bool    hadError = false;
      string  newText;

      if ( m_SymbolMode )
      {
        newText = Parser.BasicFileParser.ReplaceAllMacrosBySymbols( editSource.Text, out hadError );
      }
      else
      {
        newText = Core.Compiling.ParserBasic.ReplaceAllSymbolsByMacros( editSource.Text );
      }
      if ( hadError )
      {
        m_SymbolMode = !m_SymbolMode;
        btnToggleSymbolMode.Image = m_SymbolMode ? global::C64Studio.Properties.Resources.toolbar_basic_symbols_enabled : global::C64Studio.Properties.Resources.toolbar_basic_symbols_disabled;
        return;
      }
      int     offset = editSource.VerticalScroll.Value;

      editSource.Text = newText;
      editSource.VerticalScroll.Value = offset;
      editSource.UpdateScrollbars();
    }



    private void SourceBasicEx_DragEnter( object sender, System.Windows.Forms.DragEventArgs e )
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



    private void SourceBasicEx_DragDrop( object sender, System.Windows.Forms.DragEventArgs e )
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



    private void editSource_DragDrop( object sender, DragEventArgs e )
    {
      SourceBasicEx_DragDrop( sender, e );
    }



    private void editSource_DragEnter( object sender, DragEventArgs e )
    {
      SourceBasicEx_DragEnter( sender, e );
    }



    private void btnToggleUpperLowerCase_CheckedChanged( object sender, EventArgs e )
    {
      ToggleCase();
    }



    private void ToggleCase()
    {
      m_LowerCaseMode = !m_LowerCaseMode;

      int     topLine = editSource.VerticalScroll.Value;

      string    text = editSource.Text;

      if ( m_LowerCaseMode )
      {
        text = MakeLowerCase( text, !Core.Settings.BASICUseNonC64Font );
      }
      else
      {
        text = MakeUpperCase( text, !Core.Settings.BASICUseNonC64Font );
      }
      editSource.Text = text;

      editSource.VerticalScroll.Value = topLine;
      editSource.UpdateScrollbars();

      if ( m_LowerCaseMode )
      {
        btnToggleUpperLowerCase.Image = Properties.Resources.toolbar_basic_toggle_upperlowercase_up;
        toolTip1.SetToolTip( btnToggleUpperLowerCase, "Toggle Upper/Lower Case( Currently Lower Case )" );
      }
      else
      {
        btnToggleUpperLowerCase.Image = Properties.Resources.toolbar_basic_toggle_upperlowercase_down;
        toolTip1.SetToolTip( btnToggleUpperLowerCase, "Toggle Upper/Lower Case( Currently Upper Case )" );
      }
    }



    private void editBASICStartAddress_TextChanged( object sender, EventArgs e )
    {
      if ( DocumentInfo.Element != null )
      {
        if ( DocumentInfo.Element.StartAddress != editBASICStartAddress.Text )
        {
          DocumentInfo.Element.StartAddress = editBASICStartAddress.Text;
          SetModified();
        }
      }
      m_StartAddress = editBASICStartAddress.Text;
    }



    public override void OnApplicationEvent( ApplicationEvent Event )
    {
      base.OnApplicationEvent( Event );

      switch ( Event.EventType )
      {
        case ApplicationEvent.Type.DOCUMENT_ACTIVATED:
          UpdateStatusInfo();
          break;
        case ApplicationEvent.Type.ELEMENT_CREATED:
          // sanitize settings
          if ( Event.Doc == DocumentInfo )
          {
            if ( string.IsNullOrEmpty( m_StartAddress ) )
            {
              m_StartAddress = "2049";
              editBASICStartAddress.Text = m_StartAddress;
            }
          }
          break;
      }
    }



    private void comboBASICVersion_SelectedIndexChanged( object sender, EventArgs e )
    {
      string dialect = ( (GR.Generic.Tupel<string, Dialect>)comboBASICVersion.SelectedItem ).first;
      Dialect basicDialect = ( (GR.Generic.Tupel<string, Dialect>)comboBASICVersion.SelectedItem ).second;

      // override start address if it's default
      if ( m_BASICDialect != null )
      {
        if ( m_StartAddress == m_BASICDialect.DefaultStartAddress )
        {
          editBASICStartAddress.Text = basicDialect.DefaultStartAddress;
        }
      }

      if ( ( DocumentInfo != null )
      &&   ( DocumentInfo.Element != null ) )
      {
        DocumentInfo.Element.BASICDialect = dialect;
        SetModified();
      }

      var settings = new Parser.BasicFileParser.ParserSettings();
      settings.StripSpaces = Core.Settings.BASICStripSpaces;

      Core.Compiling.ParserBasic.Settings.StripSpaces   = Core.Settings.BASICStripSpaces;
      Core.Compiling.ParserBasic.Settings.BASICDialect  = basicDialect;
      Core.Compiling.ParserBasic.Settings.UpperCaseMode = !m_LowerCaseMode;
      Core.Compiling.ParserBasic.Settings.UseC64Font    = !Core.Settings.BASICUseNonC64Font;

      m_BASICDialectName  = dialect;
      m_BASICDialect      = basicDialect;

      m_Parser = new Parser.BasicFileParser( settings, "" );
      m_Parser.SetBasicDialect( basicDialect );
      ( (BASICSyntaxHighlighter)editSource.SyntaxHighlighter ).SetBASICDialect( basicDialect );

      editSource.PreferredLineWidth = basicDialect.SafeLineLength;

      string opCodes = @"\b(";

      foreach ( var tokenInfo in m_Parser.Settings.BASICDialect.Opcodes )
      {
        var token = tokenInfo.Key;

        if ( token.Length == 1 )
        {
          continue;
        }
        if ( token.EndsWith( "(" ) )
        {
          opCodes += token.Substring( 0, token.Length - 1 );
        }
        else
        {
          opCodes += token;
        }
        opCodes += '|';
      }
      opCodes = opCodes.Substring( 0, opCodes.Length - 1 ) + ")\b";

      m_TextRegExp[(int)Types.ColorableElement.CODE] = new System.Text.RegularExpressions.Regex( opCodes, System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled );
    }



    private void btnToggleStringEntryMode_CheckedChanged( object sender, EventArgs e )
    {
      ToggleStringEntryMode();
    }



    private void ToggleStringEntryMode()
    {
      m_StringEnterMode = !m_StringEnterMode;
      if ( m_StringEnterMode )
      {
        toolTip1.SetToolTip( btnToggleUpperLowerCase, "Toggle String Entry Mode (currently active)" );
      }
      else
      {
        toolTip1.SetToolTip( btnToggleUpperLowerCase, "Toggle String Entry Mode (currently inactive)" );
      }
      btnToggleStringEntryMode.Image = m_StringEnterMode ? global::C64Studio.Properties.Resources.toolbar_basic_string_mode_active : global::C64Studio.Properties.Resources.toolbar_basic_string_mode_inactive;
    }



    private void copyToolStripMenuItem_Click( object sender, EventArgs e )
    {
      Copy();
    }



    private void pasteToolStripMenuItem_Click( object sender, EventArgs e )
    {
      Paste();
    }



    private void cutToolStripMenuItem_Click( object sender, EventArgs e )
    {
      Cut();
    }



    public override GR.Memory.ByteBuffer SaveToBuffer()
    {
      var sourceData = new GR.IO.FileChunk( FileChunkConstants.SOURCE_BASIC );

      // version
      sourceData.AppendI32( 1 );
      sourceData.AppendString( editSource.Text );
      sourceData.AppendI32( CursorLine );
      sourceData.AppendU8( (byte)( m_LabelMode ? 1 : 0 ) );
      sourceData.AppendU8( (byte)( m_SymbolMode ? 1 : 0 ) );
      sourceData.AppendU8( (byte)( m_LowerCaseMode ? 1 : 0 ) );

      return sourceData.ToBuffer();
    }



    public override bool ReadFromReader( IReader Reader )
    {
      var chunk = new GR.IO.FileChunk();

      if ( ( !chunk.ReadFromStream( Reader ) )
      ||   ( chunk.Type != FileChunkConstants.SOURCE_BASIC ) )
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

      SetCursorToLine( reader.ReadInt32(), false );

      m_LabelMode     = ( reader.ReadUInt8() == 1 );
      btnToggleLabelMode.Checked = m_LabelMode;

      btnToggleSymbolMode.Checked = ( reader.ReadUInt8() == 1 );
      btnToggleSymbolMode_CheckedChanged( this, new EventArgs() );

      bool lowerCaseMode = ( reader.ReadUInt8() == 1 );
      if ( lowerCaseMode != m_LowerCaseMode )
      {
        ToggleCase();
      }
      return true;
    }



    private void editSource_LineVisited( object sender, FastColoredTextBoxNS.LineVisitedArgs e )
    {
      Core.Navigating.VisitedLine( DocumentInfo, e.LineIndex );
    }



    public bool GetCompilableCode( out string Code )
    {
      if ( m_LabelMode )
      {
        return PerformLabelModeToggle( out Code );
      }
      Code = GetContent();
      return true;
    }



  }
}
