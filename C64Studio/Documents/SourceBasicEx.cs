using C64Studio.CustomRenderer;
using C64Studio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace C64Studio
{
  public partial class SourceBasicEx : BaseDocument
  {
    int                                       m_CurrentMarkedLineIndex = -1;
    string                                    m_FilenameToOpen = "";
    System.Windows.Forms.ToolTip              m_ToolTip = new System.Windows.Forms.ToolTip();
    System.Drawing.Point                      m_LastTooltipPos = new System.Drawing.Point();
    bool                                      m_StringEnterMode = false;
    bool                                      m_LabelMode = false;
    bool                                      m_SymbolMode = false;
    bool                                      m_LowerCaseMode = false;
    System.Windows.Forms.Keys m_ControlKeyReplacement = System.Windows.Forms.Keys.Tab;
    System.Windows.Forms.Keys m_CommodoreKeyReplacement = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.ControlKey;
    private FastColoredTextBoxNS.AutocompleteMenu   AutoComplete = null;

    private FastColoredTextBoxNS.TextStyle[]  m_TextStyles = new FastColoredTextBoxNS.TextStyle[(int)Types.ColorableElement.LAST_ENTRY];
    private System.Text.RegularExpressions.Regex[]    m_TextRegExp = new System.Text.RegularExpressions.Regex[(int)Types.ColorableElement.LAST_ENTRY];

    private string                            m_CurrentHighlightText = null;


    public override int CursorLine
    {
      get
      {
        return editSource.Selection.Start.iLine;
      }
    }



    public SourceBasicEx( StudioCore Core )
    {
      this.Core = Core;

      DocumentInfo.Type = ProjectElement.ElementType.BASIC_SOURCE;
      DocumentInfo.UndoManager.MainForm = Core.MainForm;

      string opCodes = @"\b(";
      
      foreach ( var tokenInfo in Parser.BasicFileParser.m_Opcodes )
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
      //lda|sta|ldy|sty|ldx|stx|rts|jmp|jsr|rti|sei|cli|asl|lsr|inc|dec|inx|dex|iny|dey|cpx|cpy|cmp|bit|bne|beq|bcc|bcs|bpl|bmi|adc|sec|clc|sbc|tax|tay|tya|txa|pha|pla|eor|and|ora|ror|rol|php|plp|clv|cld|bvc|bvs|brk|nop|txs|tsx|slo|rla|sre|rra|sax|lax|dcp|isc|anc|alr|arr|xaa|axs|ahx|shy|shx|tas|las|sed)\b";
      string pseudoOps = @"(!byte|!by|!basic|!8|!08|!word|!wo|!16|!text|!tx|!scr|!pet|!raw|!pseudopc|!realpc|!bank|!convtab|!ct|!binary|!bin|!bi|!source|!src|!to|!zone|!zn|!error|!serious|!warn|"
        + @"!message|!ifdef|!ifndef|!if|!fill|!fi|!align|!endoffile|!nowarn|!for|!end|!macro|!trace|!media|!mediasrc|!sl|!cpu|!set)\b";

      m_TextRegExp[(int)Types.ColorableElement.LITERAL_NUMBER] = new System.Text.RegularExpressions.Regex( @"\b\d+[\.]?\d*([eE]\-?\d+)?[lLdDfF]?\b|\B\$[a-fA-F\d]+\b|\b0x[a-fA-F\d]+\b" );
      m_TextRegExp[(int)Types.ColorableElement.LITERAL_STRING] = new System.Text.RegularExpressions.Regex( @"""""|''|"".*?[^\\]""|'.*?[^\\]'" );

      m_TextRegExp[(int)Types.ColorableElement.CODE] = new System.Text.RegularExpressions.Regex( opCodes, System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled );
      m_TextRegExp[(int)Types.ColorableElement.PSEUDO_OP] = new System.Text.RegularExpressions.Regex( pseudoOps, System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled );

      m_TextRegExp[(int)Types.ColorableElement.LABEL] = new System.Text.RegularExpressions.Regex( @"[+\-a-zA-Z]+[a-zA-Z_\d]*[:]*" );
      //m_TextRegExp[(int)Types.SyntaxElement.COMMENT] = new System.Text.RegularExpressions.Regex( @";.*" );
      m_TextRegExp[(int)Types.ColorableElement.NONE] = new System.Text.RegularExpressions.Regex( @"\S" );

      m_IsSaveable = true;

      InitializeComponent();

      btnToggleSymbolMode.Checked = Core.Settings.BASICShowControlCodesAsChars;
      AutoComplete = new FastColoredTextBoxNS.AutocompleteMenu( editSource );

      contextSource.Opening += new CancelEventHandler( contextSource_Opening );

      editSource.AutoIndentChars = false;
      editSource.SyntaxHighlighter = new BASICSyntaxHighlighter();
      editSource.SelectingWord += EditSource_SelectingWord;

      if ( Core.Settings.DetermineAccelerator( C64Studio.Types.Function.DELETE_LINE ) != null )
      {
        editSource.HotkeysMapping.Add( Core.Settings.DetermineAccelerator( C64Studio.Types.Function.DELETE_LINE ).Key, FastColoredTextBoxNS.FCTBAction.DeleteLine );
      }

      if ( !Core.Settings.BASICUseNonC64Font )
      {
        editSource.Font = new System.Drawing.Font( Core.MainForm.m_FontC64.Families[0], Core.Settings.SourceFontSize );
      }
      else
      {
        editSource.Font = new System.Drawing.Font( Core.Settings.BASICSourceFontFamily, Core.Settings.BASICSourceFontSize );
      }

      string lexingKeywords = "";
      foreach ( Parser.BasicFileParser.Opcode opcode in Parser.BasicFileParser.m_Opcodes.Values )
      {
        if ( lexingKeywords.Length == 0 )
        {
          lexingKeywords = opcode.Command;
        }
        else
        {
          lexingKeywords += " " + opcode.Command;
        }
      }

      editSource.LeftBracket = '(';
      editSource.RightBracket = ')';
      editSource.LeftBracket2 = '\x0';
      editSource.RightBracket2 = '\x0';
      editSource.CommentPrefix = "REM";

      RefreshDisplayOptions();

      editSource.ContextMenu = null;
      editSource.LeftPadding = 40;   // space for marker symbol on left side
      editSource.AllowDrop = true;

      editSource.MouseEnter += new EventHandler( editSource_MouseEnter );
      editSource.MouseLeave += new EventHandler(editSource_MouseLeave);
      editSource.MouseMove += new System.Windows.Forms.MouseEventHandler( editSource_MouseMove );
      editSource.KeyDown += new System.Windows.Forms.KeyEventHandler( editSource_KeyDown );
      editSource.KeyPress += new System.Windows.Forms.KeyPressEventHandler( editSource_KeyPress );
      editSource.KeyUp += new System.Windows.Forms.KeyEventHandler( editSource_KeyUp );
      editSource.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler( editSource_PreviewKeyDown );
      editSource.TextChanged += new EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>( editSource_TextChanged );
      editSource.SelectionChangedDelayed += editSource_SelectionChangedDelayed;

      ///editSource.Scrolling.HorizontalScrollWidth = 3000;

      ///editSource.Indentation.UseTabs = !Core.Settings.TabConvertToSpaces;
      editSource.TabLength = Core.Settings.TabSize;

      //editSource.OnSyntaxHighlight

      m_ToolTip.Active = true;
      m_ToolTip.SetToolTip( editSource, "x" );
      m_ToolTip.Popup += new System.Windows.Forms.PopupEventHandler( m_ToolTip_Popup );

      contextSource.Opened += new EventHandler( contextSource_Opened );
    }



    private void EditSource_SelectingWord( object sender, FastColoredTextBoxNS.SelectingWordEventArgs e )
    {
      string    content = editSource.Lines[e.Place.iLine];
      if ( m_LowerCaseMode )
      {
        content = MakeUpperCase( content );
      }

      var info = Core.Compiling.ParserBasic.PureTokenizeLine( content, e.Place.iLine );

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
      /*
      //clear previous highlighting
      e.ChangedRange.ClearStyle( m_TextStyles );

      // apply all styles
      e.ChangedRange.SetStyle( m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.LITERAL_NUMBER )], m_TextRegExp[(int)Types.ColorableElement.LITERAL_NUMBER] );
      e.ChangedRange.SetStyle( m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.LITERAL_STRING )], m_TextRegExp[(int)Types.ColorableElement.LITERAL_STRING] );
      e.ChangedRange.SetStyle( m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.CODE )], m_TextRegExp[(int)Types.ColorableElement.CODE] );
      e.ChangedRange.SetStyle( m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.PSEUDO_OP )], m_TextRegExp[(int)Types.ColorableElement.PSEUDO_OP] );
      e.ChangedRange.SetStyle( m_TextStyles[SyntaxElementStylePrio( Types.ColorableElement.LABEL )], m_TextRegExp[(int)Types.ColorableElement.LABEL] );
      //e.ChangedRange.SetStyle( m_TextStyles[SyntaxElementStylePrio( Types.SyntaxElement.COMMENT )], m_TextRegExp[(int)Types.SyntaxElement.COMMENT] );
      */
      if ( UndoPossible )
      {
        SetModified();
      }
    }



    public override void RefreshDisplayOptions()
    {
      if ( !Core.Settings.BASICUseNonC64Font )
      {
        editSource.Font = new System.Drawing.Font( Core.MainForm.m_FontC64.Families[0], Core.Settings.SourceFontSize );
      }
      else
      {
        editSource.Font = new System.Drawing.Font( Core.Settings.BASICSourceFontFamily, Core.Settings.BASICSourceFontSize );
      }

      editSource.CharHeight = 18;

      editSource.Language = FastColoredTextBoxNS.Language.Custom;

      // adjust caret color (Thanks Tulan!)
      if ( ( 0.2126 * editSource.BackColor.R + 0.7152 * editSource.BackColor.G + 0.0722 * editSource.BackColor.B ) < 127.5 )
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

      editSource.CommentPrefix = "REM";

      //editSource.Indentation.UseTabs = !Core.Settings.TabConvertToSpaces;
      editSource.TabLength = Core.Settings.TabSize;

      //call OnTextChanged for refresh syntax highlighting
      editSource.OnTextChanged();
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

      //editSource.AddStyle( m_TextStyles[(int)Element] );
      editSource.Styles[SyntaxElementStylePrio( Element )] = m_TextStyles[SyntaxElementStylePrio( Element )];

      // empty space
      editSource.BackColor = GR.Color.Helper.FromARGB( Core.Settings.SyntaxColoring[Types.ColorableElement.EMPTY_SPACE].BGColor );
      editSource.SelectionColor = GR.Color.Helper.FromARGB( Core.Settings.SyntaxColoring[Types.ColorableElement.SELECTED_TEXT].FGColor );
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
      Core.MainForm.statusEditorDetails.Text = "Line " + ( CursorLine + 1 ).ToString() + ", Row " + editSource.Selection.Start.iChar.ToString();
    }

    
    
    void editSource_MouseMove( object sender, System.Windows.Forms.MouseEventArgs e )
    {
      if ( ( Math.Abs( m_LastTooltipPos.X - e.X ) >= 10 )
      ||   ( Math.Abs( m_LastTooltipPos.Y - e.Y ) >= 10 ) )
      {
        // moved too far
        m_ToolTip.Hide( editSource );
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
      int position = editSource.PointToPosition( m_LastTooltipPos );

      int lineNumber = editSource.PositionToPlace( position ).iLine;
      string wordBelow = FindWordFromPosition( position, lineNumber );
      string zone = FindZoneFromLine( lineNumber );

      /*
      //MainForm.EnsureFileIsParsed();
      Types.SymbolInfo tokenInfo = MainForm.ParserASM.TokenInfoFromName( wordBelow, zone );
      string toolTipText = "";
      if ( ( tokenInfo != null )
      &&   ( tokenInfo.Type != Types.SymbolInfo.Types.UNKNOWN ) )
      {
        toolTipText = "$" + tokenInfo.AddressOrValue.ToString( "x4" ) + ", " + tokenInfo.AddressOrValue.ToString();

        //m_ToolTip.Hide( editSource );
        //m_ToolTip.Show( toolTipText, editSource, editSource.PointToClient( System.Windows.Forms.Cursor.Position ) );
        m_ToolTip.SetToolTip( editSource, toolTipText );
      }
      else
      {
        m_ToolTip.Hide( editSource );
      }
       */
    }



    private void OpenAutoComplete()
    {
      /*
      if ( e.Ch == '"' )
      {
        m_StringEnterMode = !m_StringEnterMode;
      }*/

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
        content = MakeUpperCase( content );
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

        if ( m_SymbolMode )
        {
        }
        else
        {
          basicText = ReplaceAllSymbolsByMacros( basicText );
        }

        editSource.Text = basicText;
        editSource.ClearUndo();
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



    private string ReplaceAllSymbolsByMacros( string BasicText )
    {
      for ( int i = 0; i < BasicText.Length; ++i )
      {
        char    chartoCheck = BasicText[i];

        if ( chartoCheck > (char)255 )
        {
          var c64Key = Types.ConstantData.FindC64KeyByUnicode( chartoCheck );
          if ( c64Key != null )
          {
            if ( c64Key.Replacements.Count > 0 )
            {
              string    replacement = c64Key.Replacements[0];

              BasicText = BasicText.Substring( 0, i ) + "{" + replacement + "}" + BasicText.Substring( i + 1 );
              i += replacement.Length - 1;
            }
          }
        }
      }
      return BasicText;
    }



    private string ReplaceAllMacrosBySymbols( string BasicText, out bool HadError )
    {
      StringBuilder     sb = new StringBuilder();

      int               posInLine = 0;
      int               macroStartPos = 0;
      bool              insideMacro = false;

      HadError = false;

      while ( posInLine < BasicText.Length )
      {
        char    curChar = BasicText[posInLine];
        if ( insideMacro )
        {
          if ( curChar == '}' )
          {
            insideMacro = false;

            string macro = BasicText.Substring( macroStartPos + 1, posInLine - macroStartPos - 1 ).ToUpper();

            bool  foundMacro = false;
            foreach ( var key in Types.ConstantData.AllPhysicalKeyInfos )
            {
              if ( key.Replacements.Contains( macro ) )
              {
                sb.Append( key.CharValue );
                foundMacro = true;
                break;
              }
            }
            if ( !foundMacro )
            {
              Debug.Log( "Unknown macro " + macro );
              HadError = true;
              return null;
            }
          }
          ++posInLine;
          continue;
        }
        if ( curChar == '{' )
        {
          insideMacro = true;
          macroStartPos = posInLine;
          ++posInLine;
          continue;
        }
        // normal chars are passed on (also tabs, cr, lf)
        sb.Append( curChar );
        ++posInLine;
      }
      return sb.ToString();
    }



    private string FilterString( string Source )
    {
      return Source.Substring( 0, Source.Length - 1 );
    }



    public override bool SaveAs()
    {
      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Save Basic File as";
      saveDlg.Filter = FilterString( Types.Constants.FILEFILTER_BASIC + Types.Constants.FILEFILTER_ALL );
      if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return false;
      }
      if ( !DoSave( saveDlg.FileName ) )
      {
        return false;
      }
      return true;
    }



    public override bool Save()
    {
      if ( DocumentInfo.DocumentFilename == null )
      {
        System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

        saveDlg.Title = "Save Basic File as";
        saveDlg.Filter = FilterString( Types.Constants.FILEFILTER_BASIC + Types.Constants.FILEFILTER_ALL );
        if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
        {
          return false;
        }
        if ( DocumentInfo.Project == null )
        {
          DocumentInfo.DocumentFilename = saveDlg.FileName;
        }
        else
        {
          DocumentInfo.DocumentFilename = GR.Path.RelativePathTo( System.IO.Path.GetFullPath( DocumentInfo.Project.Settings.BasePath ), true, saveDlg.FileName, false );
          DocumentInfo.Element.Name = System.IO.Path.GetFileName( DocumentInfo.DocumentFilename );
          DocumentInfo.Element.Filename = DocumentInfo.DocumentFilename;
          if ( DocumentInfo.Element.Settings.Count == 0 )
          {
            DocumentInfo.Element.Settings["Default"] = new ProjectElement.PerConfigSettings();
          }
        }
        Text = System.IO.Path.GetFileNameWithoutExtension( DocumentInfo.DocumentFilename ) + "*";
        SetupWatcher();
      }
      if ( !DoSave( DocumentInfo.FullPath ) )
      {
        return false;
      }
      SetUnmodified();
      return true;
    }



    private bool DoSave( string Filename )
    {
      try
      {
        DisableFileWatcher();

        
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
        editSource[m_CurrentMarkedLineIndex].BackgroundBrush = null;
        m_CurrentMarkedLineIndex = -1;
      }
      if ( Set )
      {
        m_CurrentMarkedLineIndex = Line;
        editSource[m_CurrentMarkedLineIndex].BackgroundBrush = new System.Drawing.SolidBrush( System.Drawing.Color.FromArgb( (int)Core.Settings.SyntaxColoring[Types.ColorableElement.CURRENT_DEBUG_LINE].BGColor ) );
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
      //CenterOnCaret();
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



    public void CenterOnCaret()
    {
      // automatically centers
      editSource.Navigate( CurrentLineIndex );
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
        //return editSource.Clipboard.CanPaste;
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



    protected override bool ProcessCmdKey( ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData )
    {
      if ( ( keyData == m_ControlKeyReplacement )
      ||   ( keyData == m_CommodoreKeyReplacement ) )
      {
        // we misuse tab as command key, avoid common processing
        return true;
      }
      System.Windows.Forms.Keys bareKey = keyData & ~( System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.ShiftKey | System.Windows.Forms.Keys.Alt );

      bareKey = keyData;

      bool    controlPushed = false;
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

      //Debug.Log( "Key: " + keyData.ToString() + ", Bare Key: " + bareKey.ToString() );

      if ( Core.Settings.BASICKeyMap.KeymapEntryExists( bareKey ) )
      {
        //Debug.Log( "KeyData " + bareKey );

        var key = Core.Settings.BASICKeyMap.GetKeymapEntry( bareKey );

        if ( !Types.ConstantData.PhysicalKeyInfo.ContainsKey( key.KeyboardKey ) )
        {
          Debug.Log( "No physical key info for " + key.KeyboardKey );
        }
        var physKey = Types.ConstantData.PhysicalKeyInfo[key.KeyboardKey];

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
            m_StringEnterMode = !m_StringEnterMode;
          }
          if ( !m_StringEnterMode )
          {
            // BASIC short cut?
            if ( ( physKey.Normal.CharValue >= 'A' )
            &&   ( physKey.Normal.CharValue <= 'Z' ) )
            {
              // could be a token
              string  leftText = editSource.GetLineText( CursorLine ).Substring( 0, editSource.Selection.Start.iChar );
              if ( m_LowerCaseMode )
              {
                leftText = MakeUpperCase( leftText );
              }

              if ( ( leftText.Length >= 1 )
              &&   ( leftText[leftText.Length - 1] >= 'A' )
              &&   ( leftText[leftText.Length - 1] <= 'Z' ) )
              {
                leftText = leftText.ToLower() + physKey.Normal.CharValue;
                foreach ( var opcode in Parser.BasicFileParser.m_Opcodes.Values )
                {
                  if ( ( opcode.ShortCut != null )
                  &&   ( opcode.ShortCut.Length <= leftText.Length )
                  &&   ( string.Compare( opcode.ShortCut, 0, leftText, leftText.Length - opcode.ShortCut.Length, opcode.ShortCut.Length ) == 0 ) )
                  {
                    // TODO - case!
                    if ( m_LowerCaseMode )
                    {
                      editSource.SelectedText = MakeLowerCase( opcode.Command.Substring( opcode.ShortCut.Length - 1 ) );
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
              editSource.SelectedText = "" + c64Key.LowerCaseChar;
            }
            else
            {
              editSource.SelectedText = "" + c64Key.CharValue;
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



    private C64Character FindPhysicalKey( KeymapEntry Key, KeyModifier Modifier, char UnicodeChar )
    {
      if ( !Types.ConstantData.PhysicalKeyInfo.ContainsKey( Key.KeyboardKey ) )
      {
        return null;
      }
      var key = Types.ConstantData.PhysicalKeyInfo[Key.KeyboardKey];
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
      foreach ( var key in Types.ConstantData.AllPhysicalKeyInfos )
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
      ToggleLabelMode();
    }



    private bool ToggleLabelMode()
    {
      bool labelMode = !m_LabelMode;

      Core.MainForm.m_CompileResult.ClearMessages();
      Parser.BasicFileParser parser = new C64Studio.Parser.BasicFileParser( DocumentInfo.FullPath );
      parser.LabelMode = m_LabelMode;

      var compilerConfig = new C64Studio.Parser.CompileConfig() { Assembler = C64Studio.Types.AssemblerType.AUTO };
      if ( !parser.Parse( editSource.Text, null, compilerConfig ) )
      {
        Core.MainForm.m_CompileResult.UpdateFromMessages( parser, DocumentInfo.Project );
        Core.Navigating.UpdateFromMessages( parser.Messages,
                                            null,
                                            DocumentInfo.Project );
        Core.MainForm.m_CompileResult.Show();
        btnToggleLabelMode.Checked = false;
        return false;
      }
      string    result;
      if ( labelMode )
      {
        result = parser.EncodeToLabels();
      }
      else
      {
        result = parser.DecodeFromLabels();
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
      editSource.Text = result;
      m_LabelMode = labelMode;
      return true;
    }



    public override void FillContent( string Text )
    {
      if ( m_LowerCaseMode )
      {
        Text = MakeLowerCase( Text );
      }
      editSource.Text = Text;
      SetModified();
    }



    public override void InsertText( string Text )
    {
      editSource.SelectedText = Text;
      SetModified();
    }



    public override void ApplyFunction( C64Studio.Types.Function Function )
    {
      switch ( Function )
      {
        case C64Studio.Types.Function.FIND:
          ///editSource.FindReplace.ShowFind();
          break;
        case C64Studio.Types.Function.FIND_REPLACE:
          ///editSource.FindReplace.ShowReplace();
          break;
        case C64Studio.Types.Function.PRINT:
          editSource.Print();
          break;
      }
    }



    private void renumberToolStripMenuItem_Click( object sender, EventArgs e )
    {
      FormRenumberBASIC     formRenum = new FormRenumberBASIC( Core, this );

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
        newText = ReplaceAllMacrosBySymbols( editSource.Text, out hadError );
      }
      else
      {
        newText = ReplaceAllSymbolsByMacros( editSource.Text );
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
      if ( e.Data.GetDataPresent( DataFormats.FileDrop ) )
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
        text = MakeLowerCase( text );
      }
      else
      {
        text = MakeUpperCase( text );
      }
      /*
      StringBuilder   sb = new StringBuilder( text.Length );

      foreach ( var singleChar in text )
      {
        if ( m_LowerCaseMode )
        {
          if ( ( singleChar & 0xff00 ) == 0xee00 )
          {
            sb.Append( (char)( ( singleChar & 0x00ff ) | 0xef00 ) );
          }
          else if ( ( singleChar >= 'A')
          &&        ( singleChar <= 'Z' ) )
          {
            sb.Append( (char)( ( singleChar - 'A' + 1 ) | 0xef00 ) );
          }
          else
          {
            sb.Append( singleChar );
          }
        }
        else
        {
          if ( ( singleChar & 0xff00 ) == 0xef00 )
          {
            sb.Append( (char)( ( singleChar & 0x00ff ) | 0xee00 ) );
          }
          else
          {
            sb.Append( singleChar );
          }
        }
      }*/
      editSource.Text = text;

      editSource.VerticalScroll.Value = topLine;
      editSource.UpdateScrollbars();
    }



    private string MakeUpperCase( string BASICText )
    {
      StringBuilder   sb = new StringBuilder( BASICText.Length );

      foreach ( var singleChar in BASICText )
      {
        if ( ( singleChar & 0xff00 ) == 0xef00 )
        {
          char    newChar = (char)( ( singleChar & 0x00ff ) | 0xee00 );
          if ( ( newChar >= 0xee01 )
          &&   ( newChar <= 0xee01 + 25 ) )
          {
            sb.Append( (char)( newChar - 0xee01 + 'A' ) );
          }
          else
          {
            sb.Append( newChar );
          }
        }
        else
        {
          sb.Append( singleChar );
        }
      }
      return sb.ToString();
    }



    private string MakeLowerCase( string BASICText )
    {
      StringBuilder   sb = new StringBuilder( BASICText.Length );

      foreach ( var singleChar in BASICText )
      {
        if ( ( singleChar & 0xff00 ) == 0xee00 )
        {
          char    newChar = (char)( ( singleChar & 0x00ff ) | 0xef00 );
          if ( ( newChar >= 'A' )
          &&   ( newChar <= 'Z' ) )
          {
            sb.Append( (char)( newChar + 0xef01 - 'A' ) );
          }
          else
          {
            sb.Append( newChar );
          }
        }
        else if ( ( singleChar >= 'A' )
        &&        ( singleChar <= 'Z' ) )
        {
          sb.Append( (char)( singleChar + 0xef01 - 'A' ) );
        }
        else
        {
          sb.Append( singleChar );
        }
      }
      return sb.ToString();
    }

  }
}
