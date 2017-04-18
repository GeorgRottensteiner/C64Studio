using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace C64Studio
{
  public partial class SourceBasic : BaseDocument
  {
    private ScintillaNET.Marker               m_CurrentLineMarker = null;
    int                                       m_CurrentMarkedLineIndex = -1;
    //int                                       m_ContextMenuLineIndex = -1;
    //int                                       m_ContextMenuPosition = -1;
    string                                    m_FilenameToOpen = "";
    System.Windows.Forms.ToolTip              m_ToolTip = new System.Windows.Forms.ToolTip();
    System.Drawing.Point                      m_LastTooltipPos = new System.Drawing.Point();
    bool                                      m_StringEnterMode = false;
    bool                                      m_LabelMode = false;
    System.Windows.Forms.Keys m_ControlKeyReplacement = System.Windows.Forms.Keys.Tab;



    public override int CursorLine
    {
      get
      {
        return editSource.Caret.LineNumber;
      }
    }



    public SourceBasic( MainForm MainForm )
    {
      this.MainForm = MainForm;

      DocumentInfo.Type = ProjectElement.ElementType.BASIC_SOURCE;
      DocumentInfo.UndoManager.MainForm = MainForm;

      m_IsSaveable = true;

      /*

        <wht> <dish> <ensh> <swlc> <down> <rvon> <home> <del> <esc>
                   <red>  <rght>  <grn>  <blu> <orng> <F1> <F3> <F5> <F7> <F2>
                   <F4> <F6> <F8> <sret> <swuc> <blk> <up> <rvof> <clr> <inst>
                   <brn>  <lred>  <gry1>  <gry2>  <lgrn>  <lblu>  <gry3> <pur>
                   <left> <yel> <cyn>

       List of alternate mnemonics:
                   <wht> <up/lo lock on>  <up/lo  lock  off>  <return>  <lower
                   case>  <down>  <rvs on> <home> <delete> <esc> <red> <right>
                   <grn> <blu> <orange> <f1> <f3> <f5>  <f7>  <f2>  <f4>  <f6>
                   <f8> <shift return> <upper case> <blk> <up> <rvs off> <clr>
                   <insert> <brown> <lt red> <grey1> <grey2>  <lt  green>  <lt
                   blue> <grey3> <pur> <left> <yel> <cyn>
      */

      InitializeComponent();

      m_CurrentLineMarker = editSource.Markers[0];
      m_CurrentLineMarker.Symbol = ScintillaNET.MarkerSymbol.Background;
      m_CurrentLineMarker.BackColor = System.Drawing.Color.Yellow;

      contextSource.Opening += new CancelEventHandler( contextSource_Opening );

      RemoveCommandBinding( ScintillaNET.BindableCommand.LineCut );
      SetKeyBinding( ScintillaNET.BindableCommand.LineDelete, MainForm.Settings.DetermineAccelerator( C64Studio.Types.Function.DELETE_LINE ) );
      SetKeyBinding( ScintillaNET.BindableCommand.ShowFind, MainForm.Settings.DetermineAccelerator( C64Studio.Types.Function.FIND ) );
      SetKeyBinding( ScintillaNET.BindableCommand.FindNext, MainForm.Settings.DetermineAccelerator( C64Studio.Types.Function.FIND_NEXT ) );
      SetKeyBinding( ScintillaNET.BindableCommand.ShowReplace, MainForm.Settings.DetermineAccelerator( C64Studio.Types.Function.FIND_REPLACE ) );
      SetKeyBinding( ScintillaNET.BindableCommand.Print, MainForm.Settings.DetermineAccelerator( C64Studio.Types.Function.PRINT ) );

      if ( !MainForm.Settings.BASICUseNonC64Font )
      {
        editSource.Font = new System.Drawing.Font( MainForm.m_FontC64.Families[0], MainForm.Settings.SourceFontSize );
      }
      else
      {
        editSource.Font = new System.Drawing.Font( MainForm.Settings.BASICSourceFontFamily, MainForm.Settings.BASICSourceFontSize );
      }

      editSource.Lexing.Lexer = ScintillaNET.Lexer.BlitzBasic;
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
      editSource.Lexing.Keywords[0] = lexingKeywords;

      //System.Windows.Forms.InputLanguage.CurrentInputLanguage

      // 1 = comment
      // 2 = numeric literal
      // 3 = string literal
      // 4 = operator
      // 5 = constants
      // 6 = keywords
      //editSource.Styles[1].ForeColor = System.Drawing.Color.Green;
      editSource.Styles[2].ForeColor = System.Drawing.Color.Blue;
      editSource.Styles[4].ForeColor = System.Drawing.Color.Green;
      editSource.Styles[5].ForeColor = System.Drawing.Color.Brown;
      editSource.Styles[2].Font = editSource.Font;
      editSource.Styles[4].Font = editSource.Font;
      editSource.Styles[5].Font = editSource.Font;
      //editSource.Styles[6].BackColor = System.Drawing.Color.Yellow; // KeyWords
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
      editSource.KeyDown += new System.Windows.Forms.KeyEventHandler( editSource_KeyDown );
      editSource.KeyPress += new System.Windows.Forms.KeyPressEventHandler( editSource_KeyPress );
      editSource.KeyUp += new System.Windows.Forms.KeyEventHandler( editSource_KeyUp );
      editSource.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler( editSource_PreviewKeyDown );

      editSource.Scrolling.HorizontalScrollWidth = 3000;

      editSource.Indentation.UseTabs = !MainForm.Settings.TabConvertToSpaces;
      editSource.Indentation.TabWidth = MainForm.Settings.TabSize;

      for ( int i = 0; i < editSource.Styles.Max.Index; ++i )
      {
        editSource.Styles[i].Font = editSource.Font;
      }

      m_ToolTip.Active = true;
      m_ToolTip.SetToolTip( editSource, "x" );
      m_ToolTip.Popup += new System.Windows.Forms.PopupEventHandler( m_ToolTip_Popup );

      contextSource.Opened += new EventHandler( contextSource_Opened );
    }



    public override void RefreshDisplayOptions()
    {
      if ( !MainForm.Settings.BASICUseNonC64Font )
      {
        editSource.Font = new System.Drawing.Font( MainForm.m_FontC64.Families[0], MainForm.Settings.SourceFontSize );
      }
      else
      {
        editSource.Font = new System.Drawing.Font( MainForm.Settings.BASICSourceFontFamily, MainForm.Settings.BASICSourceFontSize );
      }

      for ( int i = 0; i < editSource.Styles.Max.Index; ++i )
      {
        editSource.Styles[i].Font = editSource.Font;
      }

      editSource.Lexing.Colorize();
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
      MainForm.statusEditorDetails.Text = "Line " + ( editSource.Caret.LineNumber + 1 ).ToString() + ", Row " + editSource.GetColumn( editSource.CurrentPos ).ToString();
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
      int position = editSource.PositionFromPoint( m_LastTooltipPos.X, m_LastTooltipPos.Y );

      int lineNumber = editSource.Lines.FromPosition( position ).Number;
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



    void editSource_CharAdded( object sender, ScintillaNET.CharAddedEventArgs e )
    {
      if ( e.Ch == '"' )
      {
        m_StringEnterMode = !m_StringEnterMode;
      }

      // and in this Method you could check, if your Autocompletelist contains the last insterted Characters. If so you call the this.sciDocument.AutoComplete.Show(); Method of the Control
      // and it shows the AutocompleteList with the value that machtes the inserted Characters.
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
        if ( entry.StartsWith( wordBelow ) )
        {
          newList.Add( entry );
        }
        if ( ( wordBelow.StartsWith( "." ) )
        &&   ( entry.StartsWith( zone + "." + wordBelow.Substring( 1 ) ) ) )
        {
          newList.Add( entry.Substring( zone.Length + 1 ) );
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

        editSource.AutoComplete.DropRestOfWord = true;
        editSource.AutoComplete.IsCaseSensitive = false;
        editSource.AutoComplete.Show( wordBelow.Length, newList );
      }
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
    }



    void contextSource_Opened( object sender, EventArgs e )
    {
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
        string basicText = System.IO.File.ReadAllText( DocumentInfo.FullPath );

        uint curNeutralLangID = (uint)( System.Windows.Forms.InputLanguage.CurrentInputLanguage.Culture.LCID & 0xff );

        for ( int i = 0; i < basicText.Length; ++i )
        {
          char    chartoCheck = basicText[i];

          if ( chartoCheck > (char)255 )
          {
            // a character
            foreach ( var token in MainForm.Settings.BASICKeyMap.Keymap.Values )
            {
              if ( token.Shifted == chartoCheck )
              {
                Debug.Log( "found shifted" );
                
                basicText = basicText.Substring( 0, i ) + "{Shift-" + token.Normal + "}" + basicText.Substring( i + 1 );
              }
              else if ( token.Commodore == chartoCheck )
              {
                basicText = basicText.Substring( 0, i ) + "{CBM-" + token.Normal + "}" + basicText.Substring( i + 1 );
                Debug.Log( "found commodored" );
              }
            }
          }
        }

        editSource.Text = basicText;
        editSource.UndoRedo.EmptyUndoBuffer();
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
        if ( MainForm.ParserASM.ASMFileInfo.FindGlobalLineIndex( LineIndex, DocumentInfo.FullPath, out globalLineIndex ) )
        {
          if ( MainForm.ParserASM.ASMFileInfo.LineInfo.ContainsKey( globalLineIndex ) )
          {
            Types.ASM.LineInfo lineInfo = MainForm.ParserASM.ASMFileInfo.LineInfo[globalLineIndex];

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
      return FindWordFromPosition( editSource.Caret.Position, editSource.Caret.LineNumber );
    }



    public string FindZoneAtCaretPosition()
    {
      return FindZoneFromLine( editSource.Caret.LineNumber );
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
      string docBasePath = GR.Path.RemoveFileSpec( DocumentFilename );
      if ( DocumentInfo.Element != null )
      {
        docBasePath = DocumentInfo.Project.Settings.BasePath;
      }
      MainForm.OpenFile( GR.Path.Append( docBasePath, m_FilenameToOpen ) );
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



    System.Windows.Forms.Keys ResolveKey( char charToResolve ) 
    {
      switch ( charToResolve )
      {
        case 'A':
        case 'a':
          return System.Windows.Forms.Keys.A;
        case 'B':
        case 'b':
          return System.Windows.Forms.Keys.B;
        case 'C':
        case 'c':
          return System.Windows.Forms.Keys.C;
        case 'D':
        case 'd':
          return System.Windows.Forms.Keys.D;
        case 'E':
        case 'e':
          return System.Windows.Forms.Keys.E;
        case 'F':
        case 'f':
          return System.Windows.Forms.Keys.F;
        case 'G':
        case 'g':
          return System.Windows.Forms.Keys.G;
        case 'H':
        case 'h':
          return System.Windows.Forms.Keys.H;
        case 'I':
        case 'i':
          return System.Windows.Forms.Keys.I;
        case 'J':
        case 'j':
          return System.Windows.Forms.Keys.J;
        case 'K':
        case 'k':
          return System.Windows.Forms.Keys.K;
        case 'L':
        case 'l':
          return System.Windows.Forms.Keys.L;
        case 'M':
        case 'm':
          return System.Windows.Forms.Keys.M;
        case 'N':
        case 'n':
          return System.Windows.Forms.Keys.N;
        case 'O':
        case 'o':
          return System.Windows.Forms.Keys.O;
        case 'P':
        case 'p':
          return System.Windows.Forms.Keys.P;
        case 'Q':
        case 'q':
          return System.Windows.Forms.Keys.Q;
        case 'R':
        case 'r':
          return System.Windows.Forms.Keys.R;
        case 'S':
        case 's':
          return System.Windows.Forms.Keys.S;
        case 'T':
        case 't':
          return System.Windows.Forms.Keys.T;
        case 'U':
        case 'u':
          return System.Windows.Forms.Keys.U;
        case 'V':
        case 'v':
          return System.Windows.Forms.Keys.V;
        case 'W':
        case 'w':
          return System.Windows.Forms.Keys.W;
        case 'X':
        case 'x':
          return System.Windows.Forms.Keys.X;
        case 'Y':
        case 'y':
          return System.Windows.Forms.Keys.Y;
        case 'Z':
        case 'z':
          return System.Windows.Forms.Keys.Z;
        case ' ':
          return System.Windows.Forms.Keys.Space;
        case '0':
        case '1':
        case '2':
        case '3':
        case '4':
        case '5':
        case '6':
        case '7':
        case '8':
        case '9':
          return (System.Windows.Forms.Keys)charToResolve;
        case '+':
          return System.Windows.Forms.Keys.Add;
        case '-':
          return System.Windows.Forms.Keys.OemCloseBrackets;
        case ',':
          return (System.Windows.Forms.Keys)188;
          /*
        case ',':
        case ':':
          return (System.Windows.Forms.Keys)charToResolve;*/
      }
      return System.Windows.Forms.Keys.None;
    }



    private string MapString( string Text )
    {
      string result = "";

      uint curNeutralLangID = (uint)( System.Windows.Forms.InputLanguage.CurrentInputLanguage.Culture.LCID & 0xff );

      System.Windows.Forms.KeysConverter conv = new System.Windows.Forms.KeysConverter();

      //foreach ( char achar in Text )
      for ( int i = 0; i < Text.Length; ++i )
      {
        System.Windows.Forms.Keys keyData = ResolveKey( Text[i] );
        if ( keyData == m_ControlKeyReplacement )
        {
          // we misuse tab as command key, avoid common processing
          continue;
        }
        System.Windows.Forms.Keys bareKey = keyData & ~( System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.ShiftKey | System.Windows.Forms.Keys.Alt );

        bareKey = keyData;
        if ( ( bareKey & System.Windows.Forms.Keys.Shift ) == System.Windows.Forms.Keys.Shift )
        {
          bareKey &= ~System.Windows.Forms.Keys.Shift;
        }

        if ( MainForm.Settings.BASICKeyMap.KeymapEntryExists( keyData ) )
        {
          KeymapEntry key = MainForm.Settings.BASICKeyMap.GetKeymapEntry( keyData );
          if ( key.Commodore != 65535 )
          {
            if ( ( keyData & System.Windows.Forms.Keys.Shift ) == System.Windows.Forms.Keys.Shift )
            {
              result += key.Shifted;
              continue;
            }
            if ( GR.Win32.KeyboardInfo.GetKeyState( System.Windows.Forms.Keys.Tab ).IsPressed )
            //if ( ( keyData & m_ControlKeyReplacement ) == m_ControlKeyReplacement )
            {
              result += key.Commodore;
              continue;
            }
            result += key.Normal;
            continue;
          }
        }
        else if ( MainForm.Settings.BASICKeyMap.KeymapEntryExists( bareKey ) )
        {
          KeymapEntry key = MainForm.Settings.BASICKeyMap.GetKeymapEntry( bareKey );
          if ( ( keyData & System.Windows.Forms.Keys.Shift ) == System.Windows.Forms.Keys.Shift )
          {
            result += key.Shifted;
            continue;
          }
          else if ( GR.Win32.KeyboardInfo.GetKeyState( System.Windows.Forms.Keys.Tab ).IsPressed )
          //else if ( ( keyData & m_ControlKeyReplacement ) == m_ControlKeyReplacement )
          {
            result += key.Commodore;
            continue;
          }
          result += key.Normal;
          continue;
        }
      }
      return result;
    }



    protected override bool ProcessCmdKey( ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData )
    {
      if ( keyData == m_ControlKeyReplacement )
      {
        // we misuse tab as command key, avoid common processing
        return true;
      }
      uint curNeutralLangID = (uint)( System.Windows.Forms.InputLanguage.CurrentInputLanguage.Culture.LCID & 0xff );
      System.Windows.Forms.Keys bareKey = keyData & ~( System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.ShiftKey | System.Windows.Forms.Keys.Alt );

      bareKey = keyData;
      if ( ( bareKey & System.Windows.Forms.Keys.Shift ) == System.Windows.Forms.Keys.Shift )
      {
        bareKey &= ~System.Windows.Forms.Keys.Shift;
      }

      /*
      if ( ( bareKey & m_ControlKeyReplacement ) == m_ControlKeyReplacement )
      {
        bareKey &= ~m_ControlKeyReplacement;
      }*/
      /*
      if ( keyData != ( System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.ShiftKey ) )
      {
        Debug.Log( "hier" );
      }*/

      //Debug.Log( "Key: " + keyData.ToString() + ", Bare Key: " + bareKey.ToString() );

      if ( bareKey == (System.Windows.Forms.Keys)( System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V ) )
      {
        string    clipText = System.Windows.Forms.Clipboard.GetText();
        if ( clipText.Length > 0 )
        {
          editSource.InsertText( clipText );
          return true;
          /*
          string    resultingText = MapString( clipText );
          if ( resultingText.Length > 0 )
          {
            editSource.InsertText( resultingText );
            return true;
          }*/
        }
      }

      if ( MainForm.Settings.BASICKeyMap.KeymapEntryExists( keyData ) )
      {
        var key = MainForm.Settings.BASICKeyMap.GetKeymapEntry( keyData ); 
        if ( ( key.Commodore != 65535 )
        &&   ( ( keyData & System.Windows.Forms.Keys.Shift ) == System.Windows.Forms.Keys.Shift ) )
        {
          editSource.Selection.Text = "" + key.Shifted;
          return true;
        }
        if ( GR.Win32.KeyboardInfo.GetKeyState( System.Windows.Forms.Keys.Tab ).IsPressed )
        //if ( ( keyData & m_ControlKeyReplacement ) == m_ControlKeyReplacement )
        {
          editSource.Selection.Text = "" + key.Commodore;
          return true;
        }
        editSource.Selection.Text = "" + key.Normal;
        return true;
      }
      else if ( MainForm.Settings.BASICKeyMap.KeymapEntryExists( bareKey ) )
      {
        var key = MainForm.Settings.BASICKeyMap.GetKeymapEntry( bareKey );
        if ( ( keyData & System.Windows.Forms.Keys.Shift ) == System.Windows.Forms.Keys.Shift )
        {
          editSource.Selection.Text = "" + key.Shifted;
          return true;
        }
        else if ( GR.Win32.KeyboardInfo.GetKeyState( System.Windows.Forms.Keys.Tab ).IsPressed )
        //else if ( ( keyData & m_ControlKeyReplacement ) == m_ControlKeyReplacement )
        {
          editSource.Selection.Text = "" + key.Commodore;
          return true;
        }
        editSource.Selection.Text = "" + key.Normal;
        return true;
      }

      if ( m_StringEnterMode )
      {
        //Debug.Log( keyData.ToString() );
        switch ( bareKey )
        {
          case System.Windows.Forms.Keys.End:
            m_StringEnterMode = false;
            return true;
          case System.Windows.Forms.Keys.Down:
            editSource.InsertText( Parser.BasicFileParser.ActionTokenByValue[Parser.BasicFileParser.TokenValue.CURSOR_DOWN].Replacement );
            return true;
          case System.Windows.Forms.Keys.Up:
            editSource.InsertText( Parser.BasicFileParser.ActionTokenByValue[Parser.BasicFileParser.TokenValue.CURSOR_UP].Replacement );
            return true;
          case System.Windows.Forms.Keys.Left:
            editSource.InsertText( Parser.BasicFileParser.ActionTokenByValue[Parser.BasicFileParser.TokenValue.CURSOR_LEFT].Replacement );
            return true;
          case System.Windows.Forms.Keys.Right:
            editSource.InsertText( Parser.BasicFileParser.ActionTokenByValue[Parser.BasicFileParser.TokenValue.CURSOR_RIGHT].Replacement );
            return true;
          case System.Windows.Forms.Keys.Back:
            editSource.InsertText( Parser.BasicFileParser.ActionTokenByValue[Parser.BasicFileParser.TokenValue.DELETE].Replacement );
            return true;
          case System.Windows.Forms.Keys.Home:
            if ( ( ModifierKeys == System.Windows.Forms.Keys.ShiftKey )
            ||   ( ModifierKeys == System.Windows.Forms.Keys.Shift ) )
            {
              editSource.InsertText( Parser.BasicFileParser.ActionTokenByValue[Parser.BasicFileParser.TokenValue.CLEAR].Replacement );
            }
            else
            {
              editSource.InsertText( Parser.BasicFileParser.ActionTokenByValue[Parser.BasicFileParser.TokenValue.HOME].Replacement );
            }
            return true;
          case System.Windows.Forms.Keys.F1:
            editSource.InsertText( Parser.BasicFileParser.ActionTokenByValue[Parser.BasicFileParser.TokenValue.F1].Replacement );
            return true;
          case System.Windows.Forms.Keys.F2:
            editSource.InsertText( Parser.BasicFileParser.ActionTokenByValue[Parser.BasicFileParser.TokenValue.F2].Replacement );
            return true;
          case System.Windows.Forms.Keys.F3:
            editSource.InsertText( Parser.BasicFileParser.ActionTokenByValue[Parser.BasicFileParser.TokenValue.F3].Replacement );
            return true;
          case System.Windows.Forms.Keys.F4:
            editSource.InsertText( Parser.BasicFileParser.ActionTokenByValue[Parser.BasicFileParser.TokenValue.F4].Replacement );
            return true;
          case System.Windows.Forms.Keys.F5:
            editSource.InsertText( Parser.BasicFileParser.ActionTokenByValue[Parser.BasicFileParser.TokenValue.F5].Replacement );
            return true;
          case System.Windows.Forms.Keys.F6:
            editSource.InsertText( Parser.BasicFileParser.ActionTokenByValue[Parser.BasicFileParser.TokenValue.F6].Replacement );
            return true;
          case System.Windows.Forms.Keys.F7:
            editSource.InsertText( Parser.BasicFileParser.ActionTokenByValue[Parser.BasicFileParser.TokenValue.F7].Replacement );
            return true;
          case System.Windows.Forms.Keys.F8:
            editSource.InsertText( Parser.BasicFileParser.ActionTokenByValue[Parser.BasicFileParser.TokenValue.F8].Replacement );
            return true;
        }
        if ( GR.Win32.KeyboardInfo.GetKeyState( System.Windows.Forms.Keys.Tab ).IsPressed )
        {
          switch ( keyData )
          {
            case System.Windows.Forms.Keys.D1:
              editSource.InsertText( Parser.BasicFileParser.ActionTokenByValue[Parser.BasicFileParser.TokenValue.BLACK].Replacement );
              return true;
            case System.Windows.Forms.Keys.D2:
              editSource.InsertText( Parser.BasicFileParser.ActionTokenByValue[Parser.BasicFileParser.TokenValue.WHITE].Replacement );
              return true;
            case System.Windows.Forms.Keys.D3:
              editSource.InsertText( Parser.BasicFileParser.ActionTokenByValue[Parser.BasicFileParser.TokenValue.RED].Replacement );
              return true;
            case System.Windows.Forms.Keys.D4:
              editSource.InsertText( Parser.BasicFileParser.ActionTokenByValue[Parser.BasicFileParser.TokenValue.CYAN].Replacement );
              return true;
            case System.Windows.Forms.Keys.D5:
              editSource.InsertText( Parser.BasicFileParser.ActionTokenByValue[Parser.BasicFileParser.TokenValue.PURPLE].Replacement );
              return true;
            case System.Windows.Forms.Keys.D6:
              editSource.InsertText( Parser.BasicFileParser.ActionTokenByValue[Parser.BasicFileParser.TokenValue.GREEN].Replacement );
              return true;
            case System.Windows.Forms.Keys.D7:
              editSource.InsertText( Parser.BasicFileParser.ActionTokenByValue[Parser.BasicFileParser.TokenValue.BLUE].Replacement );
              return true;
            case System.Windows.Forms.Keys.D8:
              editSource.InsertText( Parser.BasicFileParser.ActionTokenByValue[Parser.BasicFileParser.TokenValue.YELLOW].Replacement );
              return true;
          }
        }
        if ( ModifierKeys == System.Windows.Forms.Keys.ControlKey )
        {
          switch ( keyData )
          {
            case System.Windows.Forms.Keys.D1:
              editSource.InsertText( Parser.BasicFileParser.ActionTokenByValue[Parser.BasicFileParser.TokenValue.ORANGE].Replacement );
              return true;
            case System.Windows.Forms.Keys.D2:
              editSource.InsertText( Parser.BasicFileParser.ActionTokenByValue[Parser.BasicFileParser.TokenValue.BROWN].Replacement );
              return true;
            case System.Windows.Forms.Keys.D3:
              editSource.InsertText( Parser.BasicFileParser.ActionTokenByValue[Parser.BasicFileParser.TokenValue.LIGHT_RED].Replacement );
              return true;
            case System.Windows.Forms.Keys.D4:
              editSource.InsertText( Parser.BasicFileParser.ActionTokenByValue[Parser.BasicFileParser.TokenValue.DARK_GREY].Replacement );
              return true;
            case System.Windows.Forms.Keys.D5:
              editSource.InsertText( Parser.BasicFileParser.ActionTokenByValue[Parser.BasicFileParser.TokenValue.GREY].Replacement );
              return true;
            case System.Windows.Forms.Keys.D6:
              editSource.InsertText( Parser.BasicFileParser.ActionTokenByValue[Parser.BasicFileParser.TokenValue.LIGHT_GREEN].Replacement );
              return true;
            case System.Windows.Forms.Keys.D7:
              editSource.InsertText( Parser.BasicFileParser.ActionTokenByValue[Parser.BasicFileParser.TokenValue.LIGHT_BLUE].Replacement );
              return true;
            case System.Windows.Forms.Keys.D8:
              editSource.InsertText( Parser.BasicFileParser.ActionTokenByValue[Parser.BasicFileParser.TokenValue.LIGHT_GREY].Replacement );
              return true;
          }
        }
      }

      return base.ProcessCmdKey( ref msg, keyData );
    }



    private void btnToggleLabelMode_CheckedChanged( object sender, EventArgs e )
    {
      ToggleLabelMode();
    }



    private bool ToggleLabelMode()
    {
      bool labelMode = !m_LabelMode;

      MainForm.m_CompileResult.ClearMessages();
      Parser.BasicFileParser parser = new C64Studio.Parser.BasicFileParser( DocumentInfo.FullPath );
      parser.LabelMode = m_LabelMode;
      if ( !parser.Parse( editSource.Text, null, C64Studio.Types.AssemblerType.AUTO ) )
      {
        MainForm.m_CompileResult.UpdateFromMessages( parser, DocumentInfo.Project );
        MainForm.m_CompileResult.Show();
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
        MainForm.m_CompileResult.UpdateFromMessages( parser, DocumentInfo.Project );
        MainForm.m_CompileResult.Show();
        return false;
      }
      editSource.Text = result;
      m_LabelMode = labelMode;
      return true;
    }



    public override void FillContent( string Text )
    {
      editSource.Text = Text;
      SetModified();
    }



    public override void InsertText( string Text )
    {
      editSource.Selection.Text = Text;
      SetModified();
    }



    public override void ApplyFunction( C64Studio.Types.Function Function )
    {
      switch ( Function )
      {
        case C64Studio.Types.Function.FIND:
          editSource.FindReplace.ShowFind();
          break;
        case C64Studio.Types.Function.FIND_REPLACE:
          editSource.FindReplace.ShowReplace();
          break;
        case C64Studio.Types.Function.PRINT:
          editSource.Printing.Print();
          break;
      }
    }



    private void renumberToolStripMenuItem_Click( object sender, EventArgs e )
    {
      /*
      FormRenumberBASIC     formRenum = new FormRenumberBASIC( MainForm, this );

      formRenum.ShowDialog();*/
    }

  }
}
