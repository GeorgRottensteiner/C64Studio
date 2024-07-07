using System;
using System.Collections.Generic;
using System.Drawing;
using WeifenLuo.WinFormsUI.Docking;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using RetroDevStudio.IdleQueue;
using RetroDevStudio.Types;
using RetroDevStudio.CustomRenderer;
using RetroDevStudio.Parser;
using GR.Image;
using RetroDevStudio.Documents;
using RetroDevStudio.Dialogs;
using System.Linq;
using Disassembler = RetroDevStudio.Documents.Disassembler;
using System.IO;
using System.ComponentModel;



namespace RetroDevStudio
{
  public partial class MainForm : Form
  {
    private Project               m_CurrentProject = null;

    public OutputDisplay          m_Output = new OutputDisplay();

    public SolutionExplorer       m_SolutionExplorer = null;

    public BinaryDisplay          m_BinaryEditor = null;
    public DebugRegisters         m_DebugRegisters = new DebugRegisters();
    public DebugWatch             m_DebugWatch = new DebugWatch();
    public DebugMemory            m_DebugMemory = null;
    public DebugBreakpoints       m_DebugBreakpoints = null;
    public CompileResult          m_CompileResult = null;
    public CharsetEditor          m_CharsetEditor = null;
    public Documents.Disassembler m_Disassembler = null;
    public CharsetScreenEditor    m_CharScreenEditor = null;
    public GraphicScreenEditor    m_GraphicScreenEditor = null;
    public SpriteEditor           m_SpriteEditor = null;
    public MapEditor              m_MapEditor = null;
    public Calculator             m_Calculator = null;
    public PetSCIITable           m_PetSCIITable = null;
    public Outline                m_Outline = null;
    public LabelExplorer          m_LabelExplorer = null;
    public ValueTableEditor       m_ValueTableEditor = null;
    public Documents.Help         m_Help = null;
    public FormFindReplace        m_FindReplace = null;
    public FormFilesChanged       m_FilesChanged = null;

    public SearchResults          m_SearchResults = new SearchResults();
    public FindReferences         m_FindReferences = new FindReferences();
    public Bookmarks              m_Bookmarks = null;
    public Perspective            m_ActivePerspective = Perspective.DEBUG;

    public System.Diagnostics.Process CompilerProcess = null;

    public StudioCore             StudioCore = new StudioCore();

    private List<Tasks.Task>      m_Tasks = new List<RetroDevStudio.Tasks.Task>();
    private Tasks.Task            m_CurrentTask = null;

    private bool                  m_ChangingToolWindows = false;
    private bool                  m_LoadingProject = false;

    private BaseDocument          m_ActiveSource = null;
    internal ToolInfo             m_CurrentActiveTool = null;
    public SortedDictionary<string, Palette>  Palettes = new SortedDictionary<string, Palette>();

    private static MainForm       s_MainForm = null;
    public static bool            s_SystemShutdown = false;
    private List<IdleRequest> IdleQueue = new List<IdleRequest>();

    internal DocumentInfo         LastSearchableDocumentInfo = null;

    public Cursor                 CursorHand = null;
    public Cursor                 CursorGrab = null;



    public delegate void ApplicationEventHandler( Types.ApplicationEvent Event );

    public event ApplicationEventHandler ApplicationEvent;

    private GR.Collections.Set<BaseDocument> m_ExternallyChangedDocuments = new GR.Collections.Set<BaseDocument>();

    public System.Drawing.Text.PrivateFontCollection m_FontC64 = new System.Drawing.Text.PrivateFontCollection();



    delegate void AddToOutputAndShowCallback( string Text );
    delegate void SetGUIForWaitOnExternalToolCallback( bool Wait );
    delegate void SetDebuggerValuesCallback( RegisterInfo RegisterValues );
    delegate void StartDebugAtCallback( DocumentInfo DocumentToDebug, DocumentInfo DocumentToRun, int DebugAddress );
    public delegate void ParameterLessCallback();
    delegate void UpdateWatchInfoCallback( RequestData Request, GR.Memory.ByteBuffer Data );
    delegate bool ParseFileCallback( Parser.ParserBase Parser, DocumentInfo Document, ProjectConfig Configuration );
    public delegate void DocCallback( BaseDocument Document );
    public delegate void DocShowCallback( BaseDocument Document, bool Activate );
    delegate void DocumentEventHandlerCallback( BaseDocument.DocEvent Event );
    delegate void NotifyAllDocumentsCallback( bool CanToggleBreakpoints );
    delegate void TaskFinishedCallback( RetroDevStudio.Tasks.Task FinishedTask );
    public delegate void ASMFileInfoCallback( Types.ASM.FileInfo ASMFileInfo );




    /*
    // TASM decompile
    public Systems.CPUSystem            s_Processor = Systems.CPUSystem.Create6510System();
    public bool s_NextLabelIsSingle = false;



    private string MnemonicToString( Tiny64.Opcode opcode, GR.Memory.ByteBuffer Data, ref int CodePos )
    {
      string output = opcode.Mnemonic.ToLower();

      string  addressText = "";

      ++CodePos;
      if ( opcode.NumOperands > 0 )
      {
        bool dummy = false;
        addressText = DecompileNextValue( Data, ref CodePos, ref dummy );
      }
      switch ( opcode.Addressing )
      {
        case Tiny64.Opcode.AddressingType.IMPLICIT:
          break;
        case Tiny64.Opcode.AddressingType.ABSOLUTE:
          output += " " + addressText;
          break;
        case Tiny64.Opcode.AddressingType.ABSOLUTE_X:
          output += " " + addressText + ", x";
          break;
        case Tiny64.Opcode.AddressingType.ABSOLUTE_Y:
          output += " " + addressText + ", y";
          break;
        case Tiny64.Opcode.AddressingType.IMMEDIATE:
          output += " #" + addressText;
          break;
        case Tiny64.Opcode.AddressingType.INDIRECT:
          output += " ( " + addressText + " )";
          break;
        case Tiny64.Opcode.AddressingType.INDIRECT_X:
          output += " ( " + addressText + ", x)";
          break;
        case Tiny64.Opcode.AddressingType.INDIRECT_Y:
          output += " ( " + addressText + " ), y";
          break;
        case Tiny64.Opcode.AddressingType.RELATIVE:
          {
            // int delta = value - lineInfo.AddressStart - 2;

            output += " " + addressText;
            //output += " (" + delta.ToString( "X2" ) + ")";
          }
          break;
        case Tiny64.Opcode.AddressingType.ZEROPAGE:
          output += " " + addressText;
          break;
        case Tiny64.Opcode.AddressingType.ZEROPAGE_X:
          output += " " + addressText + ", x";
          break;
        case Tiny64.Opcode.AddressingType.ZEROPAGE_Y:
          output += " " + addressText + ", y";
          break;
      }
      return output;
    }



    public string DecompileNextValue( GR.Memory.ByteBuffer Data, ref int Pos, ref bool LineComplete )
    {
      LineComplete = false;

      byte valueToCheck = Data.ByteAt( Pos );

      switch ( valueToCheck )
      {
        case 0x28:
          // 28 = 1 byte value hex
          Pos += 2;
          return "$" + Data.ByteAt( Pos - 1 ).ToString( "X2" );
        case 0x2A:
          // 2A = 1 byte value decimal
          Pos += 2;
          return Data.ByteAt( Pos - 1 ).ToString( "D" );
        case 0x2B:
          // 2B = 2 byte value decimal
          Pos += 3;
          return Data.UInt16At( Pos - 2 ).ToString( "D" );
        case 0x2C:
          // 2C = 1 byte value binary
          Pos += 2;
          return "%" + Convert.ToString( Data.ByteAt( Pos - 1 ), 2 );
        case 0x29:
          // 29 = 2 byte value
          Pos += 3;
          return "$" + Data.UInt16At( Pos - 2 ).ToString( "X4" );
        case 0x38:
          // 38 = replacement label + index
          {
            byte    labelIndex = Data.ByteAt( Pos + 1 );

            Pos += 2;
            return "Label_No_" + labelIndex.ToString();
          }
        case 0x44:
          {
            // 44 = high byte of followup
            ++Pos;

            return ">" + DecompileNextValue( Data, ref Pos, ref LineComplete );
          }
        case 0x45:
          {
            // 45 = low byte of followup
            ++Pos;
            return "<" + DecompileNextValue( Data, ref Pos, ref LineComplete );
          }
      }
      ++Pos;
      return "?";
    }




    public string DecompileNext( GR.Memory.ByteBuffer Data, ref int Pos, ref bool LineComplete )
    {
      LineComplete = false;

      byte valueToCheck = Data.ByteAt( Pos );

      switch ( valueToCheck )
      {
        case 0x89:
          // 89 = full comment
          {
            string comment = "";

            ++Pos;

            while ( Pos < Data.Length )
            {
              byte  value = Data.ByteAt( Pos );

              if ( ( value >= 0x20 )
              &&   ( value <= 0x7f ) )
              {
                comment += (char)value;
                ++Pos;
              }
              else
              {
                break;
              }
            }
            LineComplete = true;
            return ";" + comment;
          }
        case 0x91:
        case 0x93:
        case 0x95:
        case 0x94:
        case 0x9A:
          if ( valueToCheck == 0x91 )
          {
            s_NextLabelIsSingle = true;
          }
          else
          {
            s_NextLabelIsSingle = false;
          }

          // 9A = end of line comment with different ending
          {
            string comment = "";

            ++Pos;

            while ( Pos < Data.Length )
            {
              byte  value = Data.ByteAt( Pos );

              if ( ( value != 0x30 )
              &&   ( value >= 0x20 )
              &&   ( value <= 0x7f ) )
              {
                comment += (char)value;
                ++Pos;
              }
              else
              {
                break;
              }
            }
            LineComplete = true;
            return comment;
          }
        case 0x92:
        case 0x9B:
          // 9A/9B = end of line comment
          {
            string comment = "";

            ++Pos;

            while ( Pos < Data.Length )
            {
              byte  value = Data.ByteAt( Pos );

              if ( ( value >= 0x20 )
              &&   ( value <= 0x7f ) )
              {
                comment += (char)value;
                ++Pos;
              }
              else
              {
                break;
              }
            }
            LineComplete = true;
            return comment;
          }
        case 0:
          // 00 = line done
          LineComplete = true;
          return "";
        case 0x03:
          // 03 = binary data
          {
            Pos += 1;

            string result = "";
            bool firstFollowup = true;

            while ( Pos < Data.Length )
            {
              byte    nextOpcode = Data.ByteAt( Pos );
              if ( ( nextOpcode == 0x28 )
              || ( nextOpcode == 0x29 )
              || ( nextOpcode == 0x2A ) )
              {
                if ( firstFollowup )
                {
                  firstFollowup = false;
                  result += " .byte " + DecompileNextValue( Data, ref Pos, ref LineComplete );
                }
                else
                {
                  result += "," + DecompileNextValue( Data, ref Pos, ref LineComplete );
                }
              }
              else
              {
                break;
              }
            }
            LineComplete = true;
            return result;
          }
        case 0x30:
          // 30 = label + index
          {
            byte    labelIndex = Data.ByteAt( Pos + 1 );
            byte    followUpDataType = Data.ByteAt( Pos + 2 );

            Pos += 2;
            LineComplete = false;

            if ( s_NextLabelIsSingle )
            {
              s_NextLabelIsSingle = false;

              LineComplete = true;
              return "Label_No_" + labelIndex.ToString();
            }

            string result = "Label_No_" + labelIndex.ToString() + " " + DecompileNext( Data, ref Pos, ref LineComplete );
            LineComplete = true;
            return result;
          }
      }
      if ( s_Processor.OpcodeByValue.ContainsKey( valueToCheck ) )
      {
        var opCode = s_Processor.OpcodeByValue[valueToCheck];

        // TODO - Labels!
        string result = MnemonicToString( opCode, Data, ref Pos );

        // comment appended?
        if ( ( Data.ByteAt( Pos ) == 0x91 )
        ||   ( Data.ByteAt( Pos ) == 0x92 )
        ||   ( Data.ByteAt( Pos ) == 0x93 )
        ||   ( Data.ByteAt( Pos ) == 0x94 )
        ||   ( Data.ByteAt( Pos ) == 0x95 )
        ||   ( Data.ByteAt( Pos ) == 0x9a )
        ||   ( Data.ByteAt( Pos ) == 0x9b ) )
        {
          result += " ;" + DecompileNext( Data, ref Pos, ref LineComplete );
        }

        LineComplete = true;

        return result;
      }
      return "?";
    }*/



    public MainForm( string[] args )
    {
      var splash = new FormSplashScreen();
      splash.Show();
      splash.Invalidate();
      splash.Refresh();

      s_MainForm = this;

      try
      {
        string basePath = System.Reflection.Assembly.GetExecutingAssembly().Location; //.CodeBase;
      
        if ( basePath.ToUpper().StartsWith( "FILE:///" ) )
        {
          basePath = basePath.Substring( 8 );
        }
        if ( basePath.ToUpper().StartsWith( @"FILE:\\" ) )
        {
          basePath = basePath.Substring( 7 );
        }
        string fontPath = System.IO.Path.Combine( System.IO.Path.GetDirectoryName( basePath ), @"C64_Pro_Mono_v1.0-STYLE.ttf" );

        m_FontC64.AddFontFile( fontPath );
      }
      catch ( Exception )
      {
        // fallback to straight path
        try
        {
          string fontPath = @"C64_Pro_Mono_v1.0-STYLE.ttf";

          m_FontC64.AddFontFile( fontPath );
        }
        catch ( Exception ex2 )
        {
          MessageBox.Show( "C64Studio can't find or open the C64 true type font file C64_Pro_Mono_v1.0-STYLE.ttf.\r\nMake sure it's in the path of RetroDevStudio.exe.\r\n\r\n" + ex2.Message, "Can't load font" );
          throw new Exception( "Missing font file 'C64_Pro_Mono_v1.0-STYLE.ttf'" );
        }
      }
      if ( m_FontC64.Families.Length == 0 )
      {
        MessageBox.Show( "C64Studio loaded the true type font file C64_Pro_Mono_v1.0-STYLE.ttf, but it does not properly work.\r\nMake sure it's in the path of RetroDevStudio.exe.\r\n", "Can't load font" );
        throw new Exception( "Failed to load font file 'C64_Pro_Mono_v1.0-STYLE.ttf'" );
      }

      /*
      // TASM - decompile from project file
      GR.Memory.ByteBuffer      data = GR.IO.File.ReadAllBytes( @"D:\privat\projekte\c64\TurboAssembler\rasteri_src.prg" );

      int       pos = 256;
      string    currentLine = "";
      string    result = "";

      s_NextLabelIsSingle = false;

      while ( pos < (int)data.Length )
      {
        bool lineComplete = false;
        string  decompiled = DecompileNext( data, ref pos, ref lineComplete );
        if ( decompiled.Length == 0 )
        {
          break;
        }


        //currentLine += decompiled;
        currentLine = decompiled + currentLine;

        if ( lineComplete )
        {
          result = currentLine + "\r\n" + result;
          currentLine = "";
        }
      }

      Debug.Log( result );
      return;*/


      // init custom renderer
      //ToolStripManager.Renderer = new CustomRenderer.CustomToolStripRenderer();
      //ToolStripManager.Renderer = new RetroDevStudio.CustomRenderer.LightToolStripRenderer();

      InitializeComponent();

      DPIHandler.ResizeControlsForDPI( this );

      panelMain.Theme = new VS2005Theme();
      panelMain.Height = ClientSize.Height - mainStatus.Height - mainMenu.Height - mainToolBuild.Height - 7;

      Application.Idle += new EventHandler( Application_Idle );

      panelMain.ShowDocumentIcon = true;

#if DEBUG
      debugToolStripMenuItem.Visible = true;
#else
      debugToolStripMenuItem.Visible = false;
#endif

      statusProgress.Visible = false;

      CursorHand = new Cursor( new MemoryStream( Properties.Resources.grab1 ) );
      CursorGrab = new Cursor( new MemoryStream( Properties.Resources.grab2 ) );

      StudioCore.MainForm = this;
      StudioCore.Settings.PanelMain = panelMain;
      StudioCore.Settings.Main = this;
      StudioCore.Initialise();

      Palette defaultPalette = new Palette();
      defaultPalette.Name = "C64Studio";

      Palettes.Add( defaultPalette.Name, defaultPalette );

      m_SolutionExplorer    = new SolutionExplorer( StudioCore );
      m_BinaryEditor        = new BinaryDisplay( StudioCore, new GR.Memory.ByteBuffer( 2 ), true, false );
      m_CharsetEditor       = new CharsetEditor( StudioCore );
      m_CompileResult       = new CompileResult( StudioCore );
      m_SpriteEditor        = new SpriteEditor( StudioCore );
      m_GraphicScreenEditor = new GraphicScreenEditor( StudioCore );
      m_CharScreenEditor    = new CharsetScreenEditor( StudioCore );
      m_PetSCIITable        = new PetSCIITable( StudioCore );
      m_Calculator          = new Calculator();
      m_MapEditor           = new MapEditor( StudioCore );
      m_Disassembler        = new Documents.Disassembler( StudioCore );
      m_DebugMemory         = new DebugMemory( StudioCore );
      m_DebugBreakpoints    = new DebugBreakpoints( StudioCore );
      m_ValueTableEditor    = new ValueTableEditor( StudioCore );
      m_FindReplace         = new FormFindReplace( StudioCore );
      m_Help                = new Documents.Help( StudioCore );
      m_Bookmarks           = new Bookmarks( StudioCore );
      m_Outline             = new Outline( StudioCore );
      m_LabelExplorer       = new LabelExplorer( StudioCore );

      m_BinaryEditor.SetInternal();
      m_CharsetEditor.SetInternal();
      m_SpriteEditor.SetInternal();
      m_GraphicScreenEditor.SetInternal();
      m_CharScreenEditor.SetInternal();
      m_MapEditor.SetInternal();
      m_Disassembler.SetInternal();
      m_ValueTableEditor.SetInternal();

      // butt ugly hack
      StudioCore.Settings.ToolTiny64.Type       = ToolInfo.ToolType.EMULATOR;
      StudioCore.Settings.ToolTiny64.Name       = "Tiny64 Internal Debugger";
      StudioCore.Settings.ToolTiny64.IsInternal = true;

      // build default panes
      AddToolWindow( ToolWindowType.OUTLINE, m_Outline, DockState.DockRight, outlineToolStripMenuItem, true, true );
      AddToolWindow( ToolWindowType.BINARY_EDITOR, m_BinaryEditor, DockState.Document, binaryEditorToolStripMenuItem, false, false );
      AddToolWindow( ToolWindowType.SOLUTION_EXPLORER, m_SolutionExplorer, DockState.DockRight, projectExplorerToolStripMenuItem, true, true );
      AddToolWindow( ToolWindowType.OUTPUT, m_Output, DockState.DockBottom, outputToolStripMenuItem, true, true );
      AddToolWindow( ToolWindowType.COMPILE_RESULT, m_CompileResult, DockState.DockBottom, compileResulttoolStripMenuItem, true, true );
      AddToolWindow( ToolWindowType.DEBUG_REGISTERS, m_DebugRegisters, DockState.DockRight, debugRegistersToolStripMenuItem, false, true );
      AddToolWindow( ToolWindowType.DEBUG_WATCH, m_DebugWatch, DockState.DockBottom, debugWatchToolStripMenuItem, false, true );
      AddToolWindow( ToolWindowType.DEBUG_MEMORY, m_DebugMemory, DockState.DockRight, debugMemoryToolStripMenuItem, false, true );
      AddToolWindow( ToolWindowType.DEBUG_BREAKPOINTS, m_DebugBreakpoints, DockState.DockRight, breakpointsToolStripMenuItem, false, true );
      m_DebugBreakpoints.DocumentEvent += new BaseDocument.DocumentEventHandler( Document_DocumentEvent );
      AddToolWindow( ToolWindowType.DISASSEMBLER, m_Disassembler, DockState.Document, disassemblerToolStripMenuItem, false, false );
      AddToolWindow( ToolWindowType.CHARSET_EDITOR, m_CharsetEditor, DockState.Document, charsetEditorToolStripMenuItem, false, false );
      AddToolWindow( ToolWindowType.SPRITE_EDITOR, m_SpriteEditor, DockState.Document, spriteEditorToolStripMenuItem, false, false );
      AddToolWindow( ToolWindowType.CHAR_SCREEN_EDITOR, m_CharScreenEditor, DockState.Document, charScreenEditorToolStripMenuItem, false, false );
      AddToolWindow( ToolWindowType.GRAPHIC_SCREEN_EDITOR, m_GraphicScreenEditor, DockState.Document, graphicScreenEditorToolStripMenuItem, false, false );
      AddToolWindow( ToolWindowType.MAP_EDITOR, m_MapEditor, DockState.Document, mapEditorToolStripMenuItem, false, false );
      AddToolWindow( ToolWindowType.PETSCII_TABLE, m_PetSCIITable, DockState.Float, petSCIITableToolStripMenuItem, false, false );
      AddToolWindow( ToolWindowType.CALCULATOR, m_Calculator, DockState.DockRight, calculatorToolStripMenuItem, false, false );
      AddToolWindow( ToolWindowType.HELP, m_Help, DockState.Document, helpToolStripMenuItem, false, false );
      AddToolWindow( ToolWindowType.FIND_REPLACE, m_FindReplace, DockState.Float, searchReplaceToolStripMenuItem, false, false );
      AddToolWindow( ToolWindowType.SEARCH_RESULTS, m_SearchResults, DockState.DockBottom, searchResultsToolStripMenuItem, false, false );
      AddToolWindow( ToolWindowType.VALUE_TABLE_EDITOR, m_ValueTableEditor, DockState.Document, valueTableEditorToolStripMenuItem, false, false );
      AddToolWindow( ToolWindowType.FIND_REFERENCES, m_FindReferences, DockState.DockBottom, findReferencesToolStripMenuItem, false, false );
      AddToolWindow( ToolWindowType.BOOKMARKS, m_Bookmarks, DockState.DockBottom, bookmarksToolStripMenuItem, false, false );
      AddToolWindow( ToolWindowType.LABEL_EXPLORER, m_LabelExplorer, DockState.DockRight, labelExplorerToolStripMenuItem, true, true );

      StudioCore.Settings.GenericTools[GR.EnumHelper.GetDescription( ToolWindowType.OUTLINE )]                = m_Outline;
      StudioCore.Settings.GenericTools[GR.EnumHelper.GetDescription( ToolWindowType.SOLUTION_EXPLORER )]      = m_SolutionExplorer;
      StudioCore.Settings.GenericTools[GR.EnumHelper.GetDescription( ToolWindowType.OUTPUT )]                 = m_Output;
      StudioCore.Settings.GenericTools[GR.EnumHelper.GetDescription( ToolWindowType.COMPILE_RESULT )]         = m_CompileResult;
      StudioCore.Settings.GenericTools[GR.EnumHelper.GetDescription( ToolWindowType.DEBUG_REGISTERS )]        = m_DebugRegisters;
      StudioCore.Settings.GenericTools[GR.EnumHelper.GetDescription( ToolWindowType.DEBUG_WATCH )]            = m_DebugWatch;
      StudioCore.Settings.GenericTools[GR.EnumHelper.GetDescription( ToolWindowType.DEBUG_MEMORY )]           = m_DebugMemory;
      StudioCore.Settings.GenericTools[GR.EnumHelper.GetDescription( ToolWindowType.DEBUG_BREAKPOINTS )]      = m_DebugBreakpoints;
      StudioCore.Settings.GenericTools[GR.EnumHelper.GetDescription( ToolWindowType.DISASSEMBLER )]           = m_Disassembler;
      StudioCore.Settings.GenericTools[GR.EnumHelper.GetDescription( ToolWindowType.CHARSET_EDITOR )]         = m_CharsetEditor;
      StudioCore.Settings.GenericTools[GR.EnumHelper.GetDescription( ToolWindowType.SPRITE_EDITOR )]          = m_SpriteEditor;
      StudioCore.Settings.GenericTools[GR.EnumHelper.GetDescription( ToolWindowType.CHAR_SCREEN_EDITOR )]     = m_CharScreenEditor;
      StudioCore.Settings.GenericTools[GR.EnumHelper.GetDescription( ToolWindowType.GRAPHIC_SCREEN_EDITOR )]  = m_GraphicScreenEditor;
      StudioCore.Settings.GenericTools[GR.EnumHelper.GetDescription( ToolWindowType.MAP_EDITOR )]             = m_MapEditor;
      StudioCore.Settings.GenericTools[GR.EnumHelper.GetDescription( ToolWindowType.PETSCII_TABLE )]          = m_PetSCIITable;
      StudioCore.Settings.GenericTools[GR.EnumHelper.GetDescription( ToolWindowType.CALCULATOR )]             = m_Calculator;
      StudioCore.Settings.GenericTools[GR.EnumHelper.GetDescription( ToolWindowType.HELP )]                   = m_Help;
      StudioCore.Settings.GenericTools[GR.EnumHelper.GetDescription( ToolWindowType.FIND_REPLACE )]           = m_FindReplace;
      StudioCore.Settings.GenericTools[GR.EnumHelper.GetDescription( ToolWindowType.SEARCH_RESULTS )]         = m_SearchResults;
      StudioCore.Settings.GenericTools[GR.EnumHelper.GetDescription( ToolWindowType.FIND_REFERENCES )]        = m_FindReferences;
      StudioCore.Settings.GenericTools[GR.EnumHelper.GetDescription( ToolWindowType.LABEL_EXPLORER )]         = m_LabelExplorer;


      StudioCore.Settings.Functions[Function.COMPILE].MenuItem = compileToolStripMenuItem;
      StudioCore.Settings.Functions[Function.COMPILE].ToolBarButton = mainToolCompile;
      StudioCore.Settings.Functions[Function.BUILD].MenuItem = buildToolStripMenuItem1;
      StudioCore.Settings.Functions[Function.BUILD].ToolBarButton = mainToolBuild;
      StudioCore.Settings.Functions[Function.BUILD_AND_DEBUG].MenuItem = debugToolStripMenuItem1;
      StudioCore.Settings.Functions[Function.BUILD_AND_DEBUG].ToolBarButton = mainToolDebug;
      StudioCore.Settings.Functions[Function.BUILD_AND_RUN].MenuItem = buildandRunToolStripMenuItem;
      StudioCore.Settings.Functions[Function.BUILD_AND_RUN].ToolBarButton = mainToolBuildAndRun;
      StudioCore.Settings.Functions[Function.REBUILD].MenuItem = rebuildToolStripMenuItem;
      StudioCore.Settings.Functions[Function.REBUILD].ToolBarButton = mainToolRebuild;
      StudioCore.Settings.Functions[Function.UNDO].MenuItem = undoToolStripMenuItem;
      StudioCore.Settings.Functions[Function.UNDO].ToolBarButton = mainToolUndo;
      StudioCore.Settings.Functions[Function.REDO].MenuItem = redoToolStripMenuItem;
      StudioCore.Settings.Functions[Function.REDO].ToolBarButton = mainToolRedo;
      StudioCore.Settings.Functions[Function.HELP].MenuItem = helpToolStripMenuItem1;
      StudioCore.Settings.Functions[Function.COMMENT_SELECTION].ToolBarButton = mainToolCommentSelection;
      StudioCore.Settings.Functions[Function.UNCOMMENT_SELECTION].ToolBarButton = mainToolUncommentSelection;
      StudioCore.Settings.Functions[Function.SAVE_DOCUMENT].MenuItem = saveToolStripMenuItem;
      StudioCore.Settings.Functions[Function.SAVE_DOCUMENT].ToolBarButton = mainToolSave;
      StudioCore.Settings.Functions[Function.SAVE_ALL].MenuItem = saveAllToolStripMenuItem;
      StudioCore.Settings.Functions[Function.SAVE_ALL].ToolBarButton = mainToolSaveAll;
      StudioCore.Settings.Functions[Function.SAVE_DOCUMENT_AS].MenuItem = saveAsToolStripMenuItem;
      StudioCore.Settings.Functions[Function.DEBUG_BREAK].ToolBarButton = mainDebugBreak;
      StudioCore.Settings.Functions[Function.DEBUG_GO].ToolBarButton = mainDebugGo;
      StudioCore.Settings.Functions[Function.DEBUG_STOP].ToolBarButton = mainDebugStop;
      StudioCore.Settings.Functions[Function.DEBUG_STEP_OVER].ToolBarButton = mainDebugStepOver;
      StudioCore.Settings.Functions[Function.DEBUG_STEP_OUT].ToolBarButton = mainDebugStepOut;
      StudioCore.Settings.Functions[Function.DEBUG_STEP].ToolBarButton = mainDebugStepInto;
      StudioCore.Settings.Functions[Function.FIND].MenuItem = searchToolStripMenuItem;
      StudioCore.Settings.Functions[Function.FIND].ToolBarButton = mainToolFind;
      StudioCore.Settings.Functions[Function.FIND_REPLACE].MenuItem = findReplaceToolStripMenuItem;
      StudioCore.Settings.Functions[Function.FIND_REPLACE].ToolBarButton = mainToolFindReplace;
      StudioCore.Settings.Functions[Function.PRINT].ToolBarButton = mainToolPrint;
      StudioCore.Settings.Functions[Function.BUILD_TO_PREPROCESSED_FILE].MenuItem = preprocessedFileToolStripMenuItem;
      StudioCore.Settings.Functions[Function.FIND_ALL_REFERENCES].MenuItem = findAllReferencesToolStripMenuItem;
      StudioCore.Settings.Functions[Function.NAVIGATE_BACK].MenuItem = navigateBackwardToolStripMenuItem;
      StudioCore.Settings.Functions[Function.RENAME_ALL_REFERENCES].MenuItem = renameAllReferencesToolStripMenuItem;
      StudioCore.Settings.Functions[Function.NAVIGATE_BACK].ToolBarButton = btnNavigateBackward;
      StudioCore.Settings.Functions[Function.NAVIGATE_FORWARD].MenuItem = navigateForwardToolStripMenuItem;
      StudioCore.Settings.Functions[Function.NAVIGATE_FORWARD].ToolBarButton = btnNavigateForward;
      StudioCore.Settings.Functions[Function.BUILD_TO_RELOCATION_FILE].MenuItem = relocationFileToolStripMenuItem;

      ApplicationEvent += m_Outline.OnApplicationEvent;
      ApplicationEvent += m_LabelExplorer.OnApplicationEvent;
      ApplicationEvent += m_DebugBreakpoints.OnApplicationEvent;
      ApplicationEvent += m_CompileResult.OnApplicationEvent;
      ApplicationEvent += m_DebugWatch.OnApplicationEvent;
      ApplicationEvent += m_SearchResults.OnApplicationEvent;
      ApplicationEvent += m_FindReferences.OnApplicationEvent;

      m_DebugMemory.hexView.TextFont = new System.Drawing.Font( m_FontC64.Families[0], 9, System.Drawing.GraphicsUnit.Pixel );
      m_DebugMemory.hexView.ByteCharConverter = new RetroDevStudio.Converter.PETSCIIToCharConverter();

      AddMediaFormatSubMenus( mediaToolStripMenuItem );
      AddMediaFormatSubMenus( toolbarNewMediaMenuItem );

      //DPIHandler.ResizeControlsForDPI( mainTools );
      //DPIHandler.ResizeControlsForDPI( debugTools  );
      debugTools.Left = mainTools.Right;

      // auto-set app mode by checking for existing settings files
      DetermineSettingsPath();

      if ( !LoadSettings() )
      {
        // that means either an error occurred or no settings file has been found (first startup?)
        // no settings file found, ask user which mode is wanted
        var form = new FormAppMode( StudioCore );
        form.ShowDialog();

        try
        {
          if ( StudioCore.Settings.BASICKeyMap.DefaultKeymaps.ContainsKey( (uint)System.Windows.Forms.InputLanguage.CurrentInputLanguage.Culture.LCID ) )
          {
            StudioCore.Settings.BASICKeyMap.Keymap = StudioCore.Settings.BASICKeyMap.DefaultKeymaps[(uint)System.Windows.Forms.InputLanguage.CurrentInputLanguage.Culture.LCID];
          }
          else
          {
            // default to english
            StudioCore.Settings.BASICKeyMap.Keymap = StudioCore.Settings.BASICKeyMap.DefaultKeymaps[9];
          }
        }
        catch ( Exception )
        {
          // default to english
          StudioCore.Settings.BASICKeyMap.Keymap = StudioCore.Settings.BASICKeyMap.DefaultKeymaps[9];
        }
        StudioCore.Settings.SetDefaultKeyBinding();
        StudioCore.Settings.SetDefaultColors();
      }
      else
      {
        foreach ( LayoutInfo layout in StudioCore.Settings.ToolLayout.Values )
        {
          layout.RestoreLayout();
        }
        if ( StudioCore.Settings.BASICKeyMap.Keymap.Count == 0 )
        {
          if ( StudioCore.Settings.BASICKeyMap.DefaultKeymaps.ContainsKey( (uint)System.Windows.Forms.InputLanguage.CurrentInputLanguage.Culture.LCID ) )
          {
            StudioCore.Settings.BASICKeyMap.Keymap = StudioCore.Settings.BASICKeyMap.DefaultKeymaps[(uint)System.Windows.Forms.InputLanguage.CurrentInputLanguage.Culture.LCID];
          }
          else
          {
            // default to english
            StudioCore.Settings.BASICKeyMap.Keymap = StudioCore.Settings.BASICKeyMap.DefaultKeymaps[9];
          }
        }
      }

      StudioCore.Settings.SanitizeSettings();

      if ( StudioCore.Settings.MainWindowPlacement != "" )
      {
        GR.Forms.WindowStateManager.GeometryFromString( StudioCore.Settings.MainWindowPlacement, this );
      }

      RefreshDisplayOnAllDocuments();
      ApplyMenuShortCuts();

      StudioCore.Compiling.ParserBasic.Settings.StripSpaces = StudioCore.Settings.BASICStripSpaces;
      StudioCore.Compiling.ParserBasic.Settings.StripREM    = StudioCore.Settings.BASICStripREM;
      m_Outline.checkShowLocalLabels.Image          = StudioCore.Settings.OutlineShowLocalLabels ? RetroDevStudio.Properties.Resources.flag_green_on.ToBitmap() : RetroDevStudio.Properties.Resources.flag_green_off.ToBitmap();
      m_Outline.checkShowShortCutLabels.Image       = StudioCore.Settings.OutlineShowShortCutLabels ? RetroDevStudio.Properties.Resources.flag_blue_on.ToBitmap() : RetroDevStudio.Properties.Resources.flag_blue_off.ToBitmap();
      m_Outline.checkSortAlphabetically.Enabled     = StudioCore.Settings.OutlineSortByIndex;
      m_Outline.checkSortBySource.Enabled           = !StudioCore.Settings.OutlineSortByIndex;
      m_Outline.editOutlineFilter.Text              = StudioCore.Settings.OutlineFilter;

      m_LabelExplorer.checkShowLocalLabels.Image      = StudioCore.Settings.LabelExplorerShowLocalLabels ? RetroDevStudio.Properties.Resources.flag_green_on.ToBitmap() : RetroDevStudio.Properties.Resources.flag_green_off.ToBitmap();
      m_LabelExplorer.checkShowShortCutLabels.Image   = StudioCore.Settings.LabelExplorerShowShortCutLabels ? RetroDevStudio.Properties.Resources.flag_blue_on.ToBitmap() : RetroDevStudio.Properties.Resources.flag_blue_off.ToBitmap();
      m_LabelExplorer.checkSortAlphabetically.Enabled = StudioCore.Settings.LabelExplorerSortByIndex;
      m_LabelExplorer.checkSortBySource.Enabled       = !StudioCore.Settings.LabelExplorerSortByIndex;
      m_LabelExplorer.editLabelExplorerFilter.Text    = StudioCore.Settings.LabelExplorerFilter;

      EmulatorListUpdated();

      if ( StudioCore.Settings.TrueDriveEnabled )
      {
        mainToolToggleTrueDrive.Image = Properties.Resources.toolbar_truedrive_enabled;
      }
      else
      {
        mainToolToggleTrueDrive.Image = Properties.Resources.toolbar_truedrive_disabled;
      }

      // place all toolbars
      SetToolPerspective( Perspective.EDIT );

      m_FindReplace.Fill( StudioCore.Settings );

      panelMain.ActiveContentChanged += new EventHandler( panelMain_ActiveContentChanged );
      panelMain.ActiveDocumentChanged += new EventHandler( panelMain_ActiveDocumentChanged );
      UpdateMenuMRU();
      UpdateUndoSettings();

      mainTools.Visible = StudioCore.Settings.ToolbarActiveMain;
      debugTools.Visible = StudioCore.Settings.ToolbarActiveDebugger;
      debugTools.Left = mainTools.Right;
      debugTools.Top = mainTools.Top;

      m_FindReplace.RefreshDisplayOptions();
      StudioCore.Settings.RefreshDisplayOnAllDocuments( StudioCore );

      SetGUIForWaitOnExternalTool( false );

      projectToolStripMenuItem.Visible      = false;
      solutionToolStripMenuItemTop.Visible  = false;

      panelMain.AllowDrop = true;
      panelMain.DragEnter += new DragEventHandler( MainForm_DragEnter );
      panelMain.DragDrop += new DragEventHandler( MainForm_DragDrop );

      m_Disassembler.RefreshDisplayOptions();

#if DEBUG
      showLineinfosToolStripMenuItem.Visible = true;
#endif


      //DumpPanes( panelMain, "" );

      ApplicationEvent += new ApplicationEventHandler( MainForm_ApplicationEvent );

      if ( StudioCore.Settings.CheckForUpdates )
      {
        CheckForUpdate();
      }

      if ( args.Length > 0 )
      {
        OpenFile( args[0] );
      }
      else if ( HandleRestart() )
      {
      }
      else if ( StudioCore.Settings.AutoOpenLastSolution )
      {
        if ( ( !StudioCore.Settings.LastSolutionWasEmpty )
        &&   ( StudioCore.Settings.MRUProjects.Count > 0 ) )
        {
          var idleRequest = new IdleRequest();
          idleRequest.OpenLastSolution = StudioCore.Settings.MRUProjects[0];

          IdleQueue.Add( idleRequest );
        }
      }

      IdleQueue.Add( new IdleRequest() { CloseSplashScreen = splash } );
    }



    private void AddMediaFormatSubMenus( ToolStripMenuItem MenuItem )
    {
      var categories = new Dictionary<MediaType,ToolStripMenuItem>();
      var machineGroup = new Dictionary<MediaType,GR.Collections.Set<string>>();

      foreach ( var entry in Lookup.MediaFormatCategories.OrderBy( e => e.Value.second ) )
      {
        var mediaType = entry.Value.first;
        string machineCategory = entry.Value.second;
        string typeCategory = GR.EnumHelper.GetDescription( mediaType );
        if ( !categories.ContainsKey( mediaType ) )
        {
          var menuMediaType = new ToolStripMenuItem( typeCategory );
          MenuItem.DropDownItems.Add( menuMediaType );

          categories.Add( mediaType, menuMediaType );
        }

        // machine group
        if ( !machineGroup.ContainsKey( mediaType ) )
        {
          machineGroup.Add( mediaType, new GR.Collections.Set<string>() );
        }
        if ( !machineGroup[mediaType].Contains( machineCategory ) )
        {
          // add separator
          if ( categories[mediaType].DropDownItems.Count > 0 )
          {
            categories[mediaType].DropDownItems.Add( new ToolStripSeparator() );
          }

          var categoryItem = new ToolStripMenuItem( machineCategory );
          categoryItem.Enabled = false;
          categories[mediaType].DropDownItems.Add( categoryItem );

          machineGroup[mediaType].Add( machineCategory );
        }

        // the actual menu item
        var menuItem = new ToolStripMenuItem( GR.EnumHelper.GetDescription( entry.Key ) );
        menuItem.Tag = entry.Key;
        menuItem.Click += NewMediaItem_Click;
        categories[mediaType].DropDownItems.Add( menuItem );
      }

      /*
      var categories = new Dictionary<MediaType,ToolStripMenuItem>();
      var machineGroup = new Dictionary<MediaType,GR.Collections.Set<string>>();

      foreach ( MediaFormatType mediaFormatType in Enum.GetValues( typeof( MediaFormatType ) ) )
      {
        if ( mediaFormatType == MediaFormatType.UNKNOWN )
        {
          continue;
        }
        MediaType mediaTypeOfEnum = MediaType.UNKNOWN;
        string  categoryOfEnum = "";

        var enumType = mediaFormatType.GetType();
        var name = Enum.GetName( enumType, mediaFormatType );
        var allAttributes = enumType.GetField( name ).GetCustomAttributes( false );
        foreach ( var attribute in allAttributes )
        {
          if ( attribute is MediaTypeAttribute )
          {
            var mediaType = attribute as MediaTypeAttribute;
            mediaTypeOfEnum = mediaType.Type;
            if ( !categories.ContainsKey( mediaType.Type ) )
            {
              var menuMediaType = new ToolStripMenuItem( GR.EnumHelper.GetDescription(  mediaType.Type ) );
              MenuItem.DropDownItems.Add( menuMediaType );

              categories.Add( mediaType.Type, menuMediaType );
            }
          }
          if ( attribute is CategoryAttribute )
          {
            var category = attribute as CategoryAttribute;

            categoryOfEnum = category.Category;
          }
        }

        if ( !machineGroup.ContainsKey( mediaTypeOfEnum ) )
        {
          machineGroup.Add( mediaTypeOfEnum, new GR.Collections.Set<string>() );
        }
        if ( !machineGroup[mediaTypeOfEnum].Contains( categoryOfEnum ) )
        {
          // add separator
          if ( categories[mediaTypeOfEnum].DropDownItems.Count > 0 )
          {
            categories[mediaTypeOfEnum].DropDownItems.Add( new ToolStripSeparator() );
          }

          var categoryItem = new ToolStripMenuItem( categoryOfEnum );
          categoryItem.Enabled = false;
          categories[mediaTypeOfEnum].DropDownItems.Add( categoryItem );

          machineGroup[mediaTypeOfEnum].Add( categoryOfEnum );
        }

        // the actual menu item
        var menuItem = new ToolStripMenuItem( GR.EnumHelper.GetDescription( mediaFormatType ) );
        menuItem.Tag = mediaFormatType;
        menuItem.Click += NewMediaItem_Click;
        categories[mediaTypeOfEnum].DropDownItems.Add( menuItem );
      }*/
    }



    private void NewMediaItem_Click( object sender, EventArgs e )
    {
      var mediaTypeToCreate = (MediaFormatType)( ( (ToolStripMenuItem)sender ).Tag );

      FileManager doc = new FileManager( StudioCore, "" );
      doc.ShowHint  = DockState.Float;
      doc.Core      = StudioCore;
      doc.CreateEmptyMedia( mediaTypeToCreate );
      doc.Show( panelMain );
    }



    internal Project CloneProject( Project Project )
    {
      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Clone Project as";
      saveDlg.Filter = FilterString( Types.Constants.FILEFILTER_PROJECT + Types.Constants.FILEFILTER_ALL );
      if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return null;
      }

      var projectData = Project.Save();

      var newProject  = new Project();
      newProject.Core = StudioCore;
      if ( !newProject.Load( projectData.Data(), false ) )
      {
        return null;
      }

      newProject.Settings.Filename  = saveDlg.FileName;
      newProject.Settings.Name      = System.IO.Path.GetFileNameWithoutExtension( saveDlg.FileName );
      newProject.Settings.BasePath  = System.IO.Path.GetDirectoryName( saveDlg.FileName );

      if ( !newProject.Save( saveDlg.FileName ) )
      {
        return null;
      }

      // clone all contained files
      for ( int i = 0; i < Project.Elements.Count; ++i )
      {
        var sourceElement = Project.Elements[i];
        var targetElement = newProject.Elements[i];

        if ( sourceElement.DocumentInfo.Type == ProjectElement.ElementType.FOLDER )
        {
          continue;
        }

        if ( GR.Path.IsSubPath( Project.Settings.BasePath, sourceElement.DocumentInfo.FullPath ) )
        {
          if ( !System.IO.File.Exists( targetElement.DocumentInfo.FullPath ) )
          {
            System.IO.File.Copy( sourceElement.DocumentInfo.FullPath, targetElement.DocumentInfo.FullPath );
          }
        }
      }

      // and add to existing solution
      return OpenProject( saveDlg.FileName );
    }



    internal void RefreshDisplayOnAllDocuments()
    {
      uint colorSelectionBackground = StudioCore.Settings.FGColor( ColorableElement.SELECTED_TEXT );
      // make transparent
      if ( ( colorSelectionBackground & 0xff000000 ) == 0xff000000 )
      {
        colorSelectionBackground = ( colorSelectionBackground & 0x00ffffff ) | 0x40000000;
      }

      DecentForms.ControlRenderer.ColorControlText                = StudioCore.Settings.FGColor( ColorableElement.CONTROL_TEXT );
      DecentForms.ControlRenderer.ColorControlTextMouseOver       = StudioCore.Settings.FGColor( ColorableElement.CONTROL_TEXT );
      DecentForms.ControlRenderer.ColorControlTextSelected        = StudioCore.Settings.FGColor( ColorableElement.SELECTED_TEXT );
      DecentForms.ControlRenderer.ColorControlBackground          = StudioCore.Settings.BGColor( ColorableElement.BACKGROUND_BUTTON );
      DecentForms.ControlRenderer.ColorControlBackgroundMouseOver = colorSelectionBackground;
      DecentForms.ControlRenderer.ColorControlBackgroundSelected  = colorSelectionBackground;
      DecentForms.ControlRenderer.ColorControlActiveBackground    = StudioCore.Settings.BGColor( Types.ColorableElement.BACKGROUND_CONTROL );
      DecentForms.ControlRenderer.ColorControlBorderFlat          = StudioCore.Settings.FGColor( ColorableElement.CONTROL_TEXT );

      var bgColor = GR.Color.Helper.FromARGB( StudioCore.Settings.BGColor( ColorableElement.BACKGROUND_CONTROL ) );
      var fgColor = GR.Color.Helper.FromARGB( StudioCore.Settings.FGColor( ColorableElement.BACKGROUND_CONTROL ) );

      this.BackColor = bgColor;

      MainMenuStrip.BackColor = bgColor;
      MainMenuStrip.ForeColor = fgColor;

      mainTools.BackColor = bgColor;
      mainTools.ForeColor = fgColor;
      debugTools.BackColor = bgColor;
      debugTools.ForeColor = fgColor;

      mainStatus.BackColor = bgColor;
      mainStatus.ForeColor = fgColor;

      mainMenu.Renderer = new ToolStripSeparatorRenderer( StudioCore );

      StudioCore.Theming.ApplyThemeToToolStripItems( mainTools, mainTools.Items );
      StudioCore.Theming.ApplyThemeToToolStripItems( debugTools, debugTools.Items );

      panelMain.Theme.Skin.DockPaneStripSkin.DocumentGradient.InactiveTabGradient.StartColor = StudioCore.Theming.DarkenColor( bgColor );
      panelMain.Theme.Skin.DockPaneStripSkin.DocumentGradient.InactiveTabGradient.EndColor = StudioCore.Theming.DarkenColor( bgColor );
      panelMain.Theme.Skin.DockPaneStripSkin.DocumentGradient.InactiveTabGradient.TextColor = fgColor;
      panelMain.Theme.Skin.DockPaneStripSkin.DocumentGradient.ActiveTabGradient.StartColor = bgColor;
      panelMain.Theme.Skin.DockPaneStripSkin.DocumentGradient.ActiveTabGradient.EndColor = bgColor;
      panelMain.Theme.Skin.DockPaneStripSkin.DocumentGradient.ActiveTabGradient.TextColor = fgColor;
      panelMain.Theme.Skin.DockPaneStripSkin.DocumentGradient.DockStripGradient.StartColor = StudioCore.Theming.DarkenColor( bgColor );
      panelMain.Theme.Skin.DockPaneStripSkin.DocumentGradient.DockStripGradient.EndColor = StudioCore.Theming.DarkenColor( bgColor );

      panelMain.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveTabGradient.StartColor = bgColor;
      panelMain.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveTabGradient.EndColor = bgColor;
      panelMain.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveTabGradient.TextColor = fgColor;
      panelMain.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveTabGradient.StartColor = StudioCore.Theming.DarkenColor( bgColor );
      panelMain.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveTabGradient.EndColor = StudioCore.Theming.DarkenColor( bgColor );
      panelMain.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveTabGradient.TextColor = fgColor;
      panelMain.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.DockStripGradient.StartColor = StudioCore.Theming.DarkenColor( bgColor );
      panelMain.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.DockStripGradient.EndColor = StudioCore.Theming.DarkenColor( bgColor );

      panelMain.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveCaptionGradient.StartColor = bgColor;
      panelMain.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveCaptionGradient.EndColor = StudioCore.Theming.DarkenColor( bgColor );
      panelMain.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveCaptionGradient.TextColor = fgColor;
      panelMain.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveCaptionGradient.StartColor = StudioCore.Theming.DarkenColor( bgColor );
      panelMain.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveCaptionGradient.EndColor = StudioCore.Theming.DarkenColor( StudioCore.Theming.DarkenColor( bgColor ) );
      panelMain.Theme.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveCaptionGradient.TextColor = fgColor;
      panelMain.Theme.Skin.AutoHideStripSkin.DockStripBackground.StartColor = StudioCore.Theming.DarkenColor( bgColor );
      panelMain.Theme.Skin.AutoHideStripSkin.DockStripBackground.EndColor = StudioCore.Theming.DarkenColor( bgColor );


      panelMain.DockBackColor = StudioCore.Theming.DarkenColor( bgColor );
      panelMain.ForeColor = fgColor;
      foreach ( var pane in panelMain.Panes )
      {
        pane.DockPanel.DockBackColor = StudioCore.Theming.DarkenColor( bgColor );
        pane.DockPanel.ForeColor = fgColor;
      }

      foreach ( Control ctrl in this.Controls )
      {
        if ( ctrl is MdiClient )
        {
          ctrl.BackColor = StudioCore.Theming.DarkenColor( bgColor );
        }
      }
    }



    private void CheckForUpdate()
    {
      StudioCore.TaskManager.AddTask( new Tasks.TaskCheckForUpdate() );
    }



    private void ApplyMenuShortCuts()
    {
      if ( InvokeRequired )
      {
        Invoke( new ParameterLessCallback( ApplyMenuShortCuts ) );
        return;
      }

      foreach ( var function in StudioCore.Settings.Functions.Values )
      {
        if ( function.MenuItem != null )
        {
          var key = StudioCore.Settings.DetermineAcceleratorKeyForFunction( function.Function, AppState );
          try
          {
            function.MenuItem.ShortcutKeys = key;
          }
          catch ( Exception )
          {
            // there are some enum values that are invalid as keys!
          }
        }
        if ( function.ToolBarButton != null )
        {
          var key = StudioCore.Settings.DetermineAcceleratorKeyForFunction( function.Function, AppState );
          if ( key != Keys.None )
          {
            function.ToolBarButton.ToolTipText = function.Description + System.Environment.NewLine + KeyToNiceString( key );
          }
          else
          {
            function.ToolBarButton.ToolTipText = function.Description;
          }
        }
      }
    }



    private string KeyToNiceString( Keys Key )
    {
      StringBuilder sb = new StringBuilder();

      if ( ( Key & Keys.Control ) != 0 )
      {
        sb.Append( "Ctrl+" );
      }
      if ( ( Key & Keys.Shift) != 0 )
      {
        sb.Append( "Shift+" );
      }
      if ( ( Key & Keys.Alt ) != 0 )
      {
        sb.Append( "Alt+" );
      }
      sb.Append( Key & ~( Keys.Control | Keys.Shift | Keys.Alt ) );
      return sb.ToString();
    }



    public void Debugger_DebugEvent( DebugEventData Event )
    {
      switch ( Event.Type )
      {
        case DebugEvent.REGISTER_INFO:
          SetDebuggerValues( Event.Registers );
          break;
        case DebugEvent.EMULATOR_CLOSED:
          StopDebugging();
          break;
        case DebugEvent.UPDATE_WATCH:
          if ( ( IsWatchShowingCurrentDebuggedProject() )
          ||   ( Event.Request.Type == DebugRequestType.MEM_DUMP ) )
            /*
          ||   ( ( Event.Request.Type == DebugRequestType.MEM_DUMP )
          &&     ( Event.Request.Parameter1 == StudioCore.Debugging.CurrentCodePosition ) ) )*/
          {
            UpdateWatchInfo( Event.Request, Event.Data );
          }
          break;
        case DebugEvent.TRACE_OUTPUT:
          AddToOutputAndShow( Event.Text );
          break;
      }
    }



    void SetToolPerspective( Perspective NewPerspective )
    {
      if ( m_ActivePerspective == NewPerspective )
      {
        return;
      }
      StudioCore.Settings.Perspectives.StoreActiveContent( m_ActivePerspective );
      m_ActivePerspective = NewPerspective;

      var previousActiveDoc = ActiveDocumentInfo;

      foreach ( ToolWindow tool in StudioCore.Settings.Tools.Values )
      {
        if ( tool.Type == ToolWindowType.FIND_REPLACE )
        {
          // to not toggle visiblity of this
          continue;
        }
        if ( tool.Visible[NewPerspective] )
        {
          if ( tool.Document.Visible )
          {
            tool.Document.Show( panelMain );
          }
          tool.MenuItem.Checked = true;
        }
        else
        {
          tool.Document.DockPanel = panelMain;
          tool.Document.DockState = DockState.Hidden;
          tool.MenuItem.Checked = false;
        }
      }

      // restore previous active doc if it exists
      if ( ( previousActiveDoc != null )
      &&   ( previousActiveDoc.BaseDoc != null ) )
      {
        previousActiveDoc.BaseDoc.Show();
      }

      StudioCore.Settings.Perspectives.RestoreActiveContent( m_ActivePerspective );

      // handle additional debug memory views
      foreach ( var view in StudioCore.Debugging.MemoryViews.Skip( 1 ) )
      {
        if ( m_ActivePerspective == Perspective.DEBUG )
        {
          view.Show();
        }
        else
        {
          view.Hide();
        }
      }
    }



    void Application_Idle( object sender, EventArgs e )
    {
      s_MainForm.OnIdle();
    }



    void OnIdle()
    {
      if ( IdleQueue.Count > 0 )
      {
        var request = IdleQueue[0];
        IdleQueue.RemoveAt( 0 );

        if ( request.DebugRequest != null )
        {
          StudioCore.Debugging.RefreshMemorySections();
        }
        else if ( request.OpenLastSolution != null )
        {
          OpenFile( request.OpenLastSolution );
        }
        else if ( request.CloseSplashScreen != null )
        {
          request.CloseSplashScreen.Close();
        }
      }
    }



    void EmulatorListUpdated()
    {
      ToolInfo oldTool = null;
      if ( mainToolEmulator.SelectedItem != null )
      {
        oldTool = ( (GR.Generic.Tupel<string, ToolInfo>)mainToolEmulator.SelectedItem ).second;
      }

      mainToolEmulator.Items.Clear();
      mainToolEmulator.Items.Add( new GR.Generic.Tupel<string, ToolInfo>( StudioCore.Settings.ToolTiny64.Name, StudioCore.Settings.ToolTiny64 ) );
      foreach ( var tool in StudioCore.Settings.ToolInfos )
      {
        if ( tool.Type == ToolInfo.ToolType.EMULATOR )
        {
          int itemIndex = mainToolEmulator.Items.Add( new GR.Generic.Tupel<string, ToolInfo>( tool.Name, tool ) );
          if ( ( tool.Name.ToUpper() == StudioCore.Settings.EmulatorToRun )
          ||   ( oldTool == tool ) )
          {
            mainToolEmulator.SelectedIndex = itemIndex;
          }
        }
      }
      if ( ( mainToolEmulator.Items.Count != 0 )
      && ( mainToolEmulator.SelectedIndex == -1 ) )
      {
        mainToolEmulator.SelectedIndex = 0;
      }
    }



    public Palette ActivePalette
    {
      get
      {
        // TODO - aus Palette-Liste
        return ConstantData.Palette;
      }
    }



    public Project CurrentProject
    {
      get
      {
        return m_CurrentProject;
      }
    }



    public List<DocumentInfo> DocumentInfos
    {
      get
      {
        List<DocumentInfo> list = new List<DocumentInfo>();

        if ( StudioCore.Navigating.Solution != null )
        {
          foreach ( var project in StudioCore.Navigating.Solution.Projects )
          {
            foreach ( var element in project.Elements )
            {
              list.Add( element.DocumentInfo );
            }
          }
        }
        foreach ( BaseDocument doc in panelMain.Documents )
        {
          if ( doc.DocumentInfo.Element == null )
          {
            list.Add( doc.DocumentInfo );
          }
        }
        return list;
      }
    }



    void DumpPanes( DockPanel Panel, string Indent )
    {
      Debug.Log( "Tools" );

      foreach ( ToolWindow tool in StudioCore.Settings.Tools.Values )
      {
        Debug.Log( tool.ToolDescription + " visible " + tool.Visible );
        Debug.Log( "-state " + tool.Document.DockState );
      }

      Debug.Log( Indent + "Panel " + Panel.Name );
      foreach ( IDockContent content in Panel.Documents )
      {
        Debug.Log( Indent + "-doc-" + content.ToString() );
      }
      foreach ( DockPane pane in Panel.Panes )
      {
        Debug.Log( Indent + "-pan-" + pane.DockState + "-" + pane.Name );
        foreach ( DockContent dock in pane.Contents )
        {
          Debug.Log( Indent + "--dock-" + dock.Name );
          if ( pane.DockState == DockState.Float )
          {
            Debug.Log( Indent + " pos " + dock.Location );
          }
        }
        //DumpPanes( pane.DockPanel, Indent + " " );
      }
      foreach ( FloatWindow wnd in Panel.FloatWindows )
      {
        Debug.Log( Indent + "-float-" + wnd.DockState + "-" + wnd.Name );
        Debug.Log( Indent + " pos " + wnd.Location + ", size " + wnd.Size );
        foreach ( DockPane pane in wnd.NestedPanes )
        {
          Debug.Log( Indent + "--pan-" + pane.DockState + "-" + pane.Name );
          Debug.Log( Indent + "-- pos " + pane.Location + ", size " + pane.Size );
          foreach ( DockContent dock in pane.Contents )
          {
            Debug.Log( Indent + "---dock-" + dock.Name );
            if ( pane.DockState == DockState.Float )
            {
              Debug.Log( Indent + "--- pos " + dock.Location );
            }
          }
        }
        //DumpPanes( pane.DockPanel, Indent + " " );
      }

      /*
      foreach ( DockWindow window in Panel.DockWindows )
      {
        Debug.Log( Indent + "-wnd-" + window.Name );
        //DumpPanes( window.NestedPanes, Indent + " " );
      }*/
    }



    void MainForm_ApplicationEvent( RetroDevStudio.Types.ApplicationEvent Event )
    {
      switch ( Event.EventType )
      {
        case Types.ApplicationEvent.Type.KEY_BINDINGS_MODIFIED:
          ApplyMenuShortCuts();
          break;
        case RetroDevStudio.Types.ApplicationEvent.Type.EMULATOR_LIST_CHANGED:
          EmulatorListUpdated();
          break;
        case RetroDevStudio.Types.ApplicationEvent.Type.DOCUMENT_ACTIVATED:
          if ( Event.Doc == null )
          {
            mainToolPrint.Enabled = false;
          }
          else
          {
            mainToolPrint.Enabled = Event.Doc.ContainsCode;
            if ( IsCodeDocument( Event.Doc ) )
            {
              StudioCore.Navigating.LastActiveCodeDocument = Event.Doc;
            }
          }
          // current project changed?
          if ( Event.Project == null )
          {
            SetActiveProject( null );
          }
          else
          {
            SetActiveProject( Event.Project );
          }
          break;
        case Types.ApplicationEvent.Type.PROJECT_RENAMED:
          UpdateCaption();
          break;
        case Types.ApplicationEvent.Type.SOLUTION_RENAMED:
          UpdateCaption();
          // adapt MRU entries
          for ( int i = 0; i < StudioCore.Settings.MRUFiles.Count; ++i )
          {
            if ( StudioCore.Settings.MRUFiles[i] == Event.OriginalValue )
            {
              StudioCore.Settings.MRUFiles[i] = Event.UpdatedValue;
            }
          }
          for ( int i = 0; i < StudioCore.Settings.MRUProjects.Count; ++i )
          {
            if ( StudioCore.Settings.MRUProjects[i] == Event.OriginalValue )
            {
              StudioCore.Settings.MRUProjects[i] = Event.UpdatedValue;
            }
          }
          UpdateMenuMRU();
          break;
        case Types.ApplicationEvent.Type.SOLUTION_OPENED:
          solutionToolStripMenuItem.Enabled                   = true;
          solutionAddNewProjectToolStripMenuItem.Enabled      = true;
          solutionAddExistingProjectToolStripMenuItem.Enabled = true;
          solutionCloseToolStripMenuItem.Enabled              = true;
          solutionSaveToolStripMenuItem.Enabled               = true;
          solutionCloneToolStripMenuItem.Enabled              = true;
          solutionRenameToolStripMenuItem.Enabled             = !string.IsNullOrEmpty( StudioCore.Navigating.Solution.Filename );
          UpdateCaption();
          break;
        case Types.ApplicationEvent.Type.SOLUTION_CLOSED:
          m_CurrentProject = null;
          solutionToolStripMenuItem.Enabled = false;
          solutionAddNewProjectToolStripMenuItem.Enabled = false;
          solutionAddExistingProjectToolStripMenuItem.Enabled = false;
          solutionCloseToolStripMenuItem.Enabled = false;
          solutionSaveToolStripMenuItem.Enabled = false;
          solutionCloneToolStripMenuItem.Enabled = false;
          solutionRenameToolStripMenuItem.Enabled = false;

          m_Output.SetText( "" );
          m_CompileResult.ClearMessages();
          UpdateCaption();
          break;
        case Types.ApplicationEvent.Type.PROJECT_CLOSED:
          if ( ( StudioCore.Navigating.Solution != null )
          &&   ( StudioCore.Navigating.Solution.Projects.Count > 1 ) )
          {
            removeProjectFromSolutionToolStripMenuItem.Enabled = true;
          }
          else
          {
            removeProjectFromSolutionToolStripMenuItem.Enabled = false;
          }
          break;
        case Types.ApplicationEvent.Type.ACTIVE_PROJECT_CHANGED:
          m_DebugWatch.DebuggedProject = m_CurrentProject;
          m_DebugWatch.ClearAllWatchEntries();
          if ( m_CurrentProject != null )
          {
            foreach ( var watch in m_CurrentProject.Settings.WatchEntries )
            {
              m_DebugWatch.AddWatchEntry( watch );
            }
          }
          else if ( StudioCore.Debugging.Debugger != null )
          {
            // projectless debugging, use watches from debugger
            foreach ( var watch in StudioCore.Debugging.Debugger.CurrentWatches() )
            {
              m_DebugWatch.AddWatchEntry( watch );
            }
          }
          m_DebugRegisters.DebuggedProject    = m_CurrentProject;
          m_DebugMemory.DebuggedProject       = m_CurrentProject;
          m_DebugBreakpoints.DebuggedProject  = m_CurrentProject;
          UpdateCaption();
          break;
        case Types.ApplicationEvent.Type.DOCUMENT_CLOSED:
          if ( LastSearchableDocumentInfo == Event.Doc )
          {
            LastSearchableDocumentInfo = null;
          }
          if ( StudioCore.Navigating.LastActiveCodeDocument == Event.Doc )
          {
            StudioCore.Navigating.LastActiveCodeDocument = null;
          }
          break;
      }
    }



    void panelMain_ActiveDocumentChanged( object sender, EventArgs e )
    {
      var docInfo = ActiveDocumentInfo;
      RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.DOCUMENT_ACTIVATED, docInfo ) );

      mainToolFind.Enabled = ( docInfo != null ) ? docInfo.Compilable : false;
      mainToolFindReplace.Enabled = mainToolFind.Enabled;
      mainToolCommentSelection.Enabled = ( docInfo != null ) ? docInfo.Compilable : false;
      mainToolUncommentSelection.Enabled = ( docInfo != null ) ? docInfo.Compilable : false;

      if ( IsSearchableDocument( docInfo ) )
      {
        LastSearchableDocumentInfo = docInfo;
      }

      if ( ActiveDocument == null )
      {
        saveToolStripMenuItem.Enabled = false;
        saveAsToolStripMenuItem.Enabled = false;
        mainToolSave.Enabled = false;
        fileCloseToolStripMenuItem.Enabled = false;
        printToolStripMenuItem.Enabled = false;
      }
      else
      {
        printToolStripMenuItem.Enabled = true;
        saveToolStripMenuItem.Enabled = ActiveDocument.Modified;
        saveAsToolStripMenuItem.Enabled = true;
        mainToolSave.Enabled = ActiveDocument.Modified;
        fileCloseToolStripMenuItem.Enabled = true;
      }
    }



    private bool IsSearchableDocument( DocumentInfo DocInfo )
    {
      if ( DocInfo == null )
      {
        return false;
      }
      if ( ( DocInfo.Type == ProjectElement.ElementType.ASM_SOURCE )
      ||   ( DocInfo.Type == ProjectElement.ElementType.BASIC_SOURCE )
      ||   ( DocInfo.Type == ProjectElement.ElementType.DISASSEMBLER ) )
      {
        return true;
      }
      return false;
    }



    private bool IsCodeDocument( DocumentInfo DocInfo )
    {
      if ( DocInfo == null )
      {
        return false;
      }
      if ( ( DocInfo.Type == ProjectElement.ElementType.ASM_SOURCE )
      ||   ( DocInfo.Type == ProjectElement.ElementType.BASIC_SOURCE ) )
      {
        return true;
      }
      return false;
    }



    public void RaiseApplicationEvent( Types.ApplicationEvent Event )
    {
      if ( ApplicationEvent != null )
      {
        ApplicationEvent( Event );
      }
    }



    public Types.StudioState AppState
    {
      get
      {
        return StudioCore.State;
      }
      set
      {
        if ( StudioCore.State != value )
        {
          StudioCore.State = value;

          ApplyMenuShortCuts();

          bool canToggleBreakpoints = false;

          if ( ( StudioCore.State == Types.StudioState.NORMAL )
          ||   ( StudioCore.State == Types.StudioState.DEBUGGING_BROKEN ) )
          {
            canToggleBreakpoints = true;
          }

          NotifyAllDocuments( canToggleBreakpoints );
        }
      }
    }



    void NotifyAllDocuments( bool CanToggleBreakpoints )
    {
      if ( InvokeRequired )
      {
        Invoke( new NotifyAllDocumentsCallback( NotifyAllDocuments ), new object[] { CanToggleBreakpoints } );
        return;
      }

      foreach ( BaseDocument doc in panelMain.Contents )
      {
        doc.BreakpointToggleable = CanToggleBreakpoints;
      }
    }



    void panelMain_ActiveContentChanged( object sender, EventArgs e )
    {
      BaseDocument baseDoc = ActiveContent;
      if ( baseDoc == null )
      {
        mainToolUndo.Enabled = false;
        mainToolRedo.Enabled = false;
        undoToolStripMenuItem.Enabled = false;
        redoToolStripMenuItem.Enabled = false;
      }
      else
      {
        mainToolUndo.Enabled = baseDoc.UndoPossible;
        mainToolRedo.Enabled = baseDoc.RedoPossible;
        undoToolStripMenuItem.Enabled = baseDoc.UndoPossible;
        redoToolStripMenuItem.Enabled = baseDoc.RedoPossible;

        mainToolUndo.ToolTipText = baseDoc.UndoInfo;
        mainToolRedo.ToolTipText = baseDoc.RedoInfo;

        if ( baseDoc.DocumentInfo.ContainsCode )
        {
          if ( m_ActiveSource != baseDoc )
          {
            m_ActiveSource = baseDoc;
            StudioCore.TaskManager.AddTask( new Tasks.TaskRefreshOutlineAndLabelExplorer( baseDoc ) );
          }
        }
        saveToolStripMenuItem.Enabled   = baseDoc.Modified;
        saveAsToolStripMenuItem.Enabled = baseDoc.IsSaveable;
        mainToolSave.Enabled = baseDoc.Modified;
      }
    }



    void AddToolWindow( ToolWindowType Type, BaseDocument Document, DockState DockState, ToolStripMenuItem MenuItem, bool VisibleEdit, bool VisibleDebug )
    {
      var tool = new ToolWindow();

      tool.Document = Document;
      tool.Document.Core = StudioCore;
      tool.Document.ShowHint = DockState;
      tool.Document.FormClosed += new FormClosedEventHandler( Document_FormClosed );
      tool.Document.VisibleChanged += new EventHandler( Document_VisibleChanged );
      tool.Document.HideOnClose = true;
      tool.MenuItem = MenuItem;
      tool.MenuItem.CheckOnClick = true;
      tool.MenuItem.CheckedChanged += new EventHandler( MenuItem_CheckedChanged );
      tool.Visible[Perspective.EDIT] = VisibleEdit;
      tool.Visible[Perspective.DEBUG] = VisibleDebug;
      tool.ToolDescription = GR.EnumHelper.GetDescription( Type );
      tool.Type = Type;

      LayoutInfo layout = null;
      if ( StudioCore.Settings.ToolLayout.ContainsKey( MenuItem.Text ) )
      {
        layout = StudioCore.Settings.ToolLayout[MenuItem.Text];
      }
      else
      {
        layout = new LayoutInfo();
        StudioCore.Settings.ToolLayout.Add( MenuItem.Text, layout );
      }
      layout.Name = MenuItem.Text;
      layout.Document = Document;
      layout.StoreLayout();
      StudioCore.Settings.Tools[Type] = tool;
    }



    void Document_VisibleChanged( object sender, EventArgs e )
    {
      BaseDocument baseDoc = (BaseDocument)sender;
      if ( !baseDoc.IsHidden )
      {
        return;
      }
      if ( m_ChangingToolWindows )
      {
        return;
      }
      m_ChangingToolWindows = true;
      foreach ( ToolWindow tool in StudioCore.Settings.Tools.Values )
      {
        if ( tool.Document == sender )
        {
          tool.MenuItem.Checked = !tool.Document.IsHidden;
          tool.Visible[m_ActivePerspective] = !tool.Document.IsHidden;
          break;
        }
      }
      m_ChangingToolWindows = false;
    }



    void Document_FormClosed( object sender, FormClosedEventArgs e )
    {
      if ( m_ChangingToolWindows )
      {
        return;
      }
      m_ChangingToolWindows = true;
      foreach ( ToolWindow tool in StudioCore.Settings.Tools.Values )
      {
        if ( tool.Document == sender )
        {
          tool.MenuItem.Checked = false;
          tool.Visible[m_ActivePerspective] = false;
          break;
        }
      }
      m_ChangingToolWindows = false;
    }



    void MenuItem_CheckedChanged( object sender, EventArgs e )
    {
      if ( m_ChangingToolWindows )
      {
        return;
      }
      m_ChangingToolWindows = true;
      foreach ( ToolWindow tool in StudioCore.Settings.Tools.Values )
      {
        if ( tool.MenuItem == sender )
        {
          if ( tool.MenuItem.Checked != tool.Document.Visible )
          {
            tool.Visible[m_ActivePerspective] = tool.MenuItem.Checked;
            if ( tool.MenuItem.Checked )
            {
              tool.Document.Show( panelMain );
            }
            else
            {
              tool.Document.Hide();
            }
          }
          m_ChangingToolWindows = false;
          return;
        }
      }
      m_ChangingToolWindows = false;
    }



    public void m_DebugMemory_ViewScrolled( object Sender, DebugMemory.DebugMemoryEvent Event )
    {
      if ( AppState == Types.StudioState.DEBUGGING_BROKEN )
      {
        // request new memory
        RequestData requestRefresh = new RequestData( DebugRequestType.REFRESH_MEMORY, Event.Viewer.MemoryStart, Event.Viewer.MemorySize );
        requestRefresh.Reason = RequestReason.MEMORY_FETCH;

        if ( !Event.Viewer.MemoryAsCPU )
        {
          requestRefresh.Type = DebugRequestType.REFRESH_MEMORY_RAM;
        }


        IdleRequest debugFetch = new IdleRequest();
        debugFetch.DebugRequest = requestRefresh;

        IdleQueue.Add( debugFetch );
      }
    }



    public void UpdateMenuMRU()
    {
      fileRecentlyOpenedProjectsToolStripMenuItem.DropDownItems.Clear();
      fileRecentlyOpenedFilesToolStripMenuItem.DropDownItems.Clear();

      StudioCore.Settings.MRUProjects = StudioCore.Settings.MRUProjects.Distinct().ToList();
      StudioCore.Settings.MRUFiles = StudioCore.Settings.MRUFiles.Distinct().ToList();

      foreach ( string entry in StudioCore.Settings.MRUProjects )
      {
        ToolStripMenuItem menuItem = new ToolStripMenuItem(entry);
        menuItem.Click += new EventHandler( menuMRUItem_Click );
        fileRecentlyOpenedProjectsToolStripMenuItem.DropDownItems.Add( menuItem );
      }
      foreach ( string entry in StudioCore.Settings.MRUFiles )
      {
        ToolStripMenuItem menuItem = new ToolStripMenuItem(entry);
        menuItem.Click += new EventHandler( menuMRUItem_Click );
        fileRecentlyOpenedFilesToolStripMenuItem.DropDownItems.Add( menuItem );
      }
    }



    bool CloseAllProjects()
    {
      if ( ( StudioCore.Navigating.Solution == null )
      ||   ( !System.IO.Directory.Exists( System.IO.Path.GetDirectoryName( StudioCore.Navigating.Solution.Filename ) ) ) )
      {
        // we have no solution, or the solution was deleted externally
        return true;
      }
      SaveSolution();
      foreach ( Project project in StudioCore.Navigating.Solution.Projects )
      {
        if ( !SaveProject( project ) )
        {
          return false;
        }
      }
      if ( !s_SystemShutdown )
      {
        while ( StudioCore.Navigating.Solution.Projects.Count > 0 )
        {
          if ( !CloseProject( StudioCore.Navigating.Solution.Projects[0] ) )
          {
            return false;
          }
        }
      }
      m_SolutionExplorer.treeProject.Nodes.Clear();
      mainToolConfig.Items.Clear();
      m_CurrentProject = null;
      projectToolStripMenuItem.Visible = false;
      StudioCore.Debugging.BreakPoints.Clear();
      return true;
    }



    void menuMRUItem_Click( object sender, EventArgs e )
    {
      string extension = System.IO.Path.GetExtension(sender.ToString()).ToUpper();

      if ( extension == ".C64" )
      {
        CloseSolution();
        if ( OpenProject( sender.ToString() ) == null )
        {
          StudioCore.Settings.RemoveFromMRU( StudioCore.Settings.MRUProjects, sender.ToString(), this );
        }
      }
      else if ( extension == ".S64" )
      {
        CloseSolution();
        if ( !OpenSolution( sender.ToString() ) )
        {
          StudioCore.Settings.RemoveFromMRU( StudioCore.Settings.MRUProjects, sender.ToString(), this );
        }
      }
      else
      {
        var newDoc = OpenFile( sender.ToString() );
        if ( newDoc == null )
        {
          StudioCore.Settings.RemoveFromMRU( StudioCore.Settings.MRUFiles, sender.ToString(), this );
        }
        else
        {
          StudioCore.Compiling.PreparseDocument( newDoc );
        }
      }
    }


    // P/Invoke declarations     
    [DllImport( "user32.dll" )]
    private static extern IntPtr WindowFromPoint( Point pt );
    [DllImport( "user32.dll" )]
    private static extern IntPtr SendMessage( IntPtr hWnd, int msg, IntPtr wp, IntPtr lp );



    private void exitToolStripMenuItem_Click( object sender, EventArgs e )
    {
      Close();
    }



    public void SaveAllDocuments()
    {
      if ( InvokeRequired )
      {
        Invoke( new ParameterLessCallback( SaveAllDocuments ) );
        return;
      }

      foreach ( IDockContent dockContent in panelMain.Contents )
      {
        BaseDocument baseDoc = (BaseDocument)dockContent;

        if ( baseDoc.Modified )
        {
          if ( !baseDoc.IsInternal )
          {
            baseDoc.Save( BaseDocument.SaveMethod.SAVE );
          }
        }
      }
      saveAllToolStripMenuItem.Enabled = false;
      saveToolStripMenuItem.Enabled = false;
      saveAsToolStripMenuItem.Enabled = true;
    }



    public DocumentInfo ActiveDocumentInfo
    {
      get
      {
        var baseDoc = (BaseDocument)panelMain.ActiveDocument;
        if ( baseDoc == null )
        {
          return null;
        }
        return baseDoc.DocumentInfo;
      }
      set
      {
        if ( value.BaseDoc != null )
        {
          value.BaseDoc.Select();
        }
      }
    }



    public BaseDocument ActiveSearchDocument
    {
      get
      {
        if ( LastSearchableDocumentInfo == null )
        {
          return null;
        }
        return (BaseDocument)LastSearchableDocumentInfo.BaseDoc;
      }
    }



    public BaseDocument ActiveDocument
    {
      get
      {
        /*
        // if active messes up compile
        if ( panelMain.ActiveContent != null )
        {
          return (BaseDocument)panelMain.ActiveContent;
        }*/
        return (BaseDocument)panelMain.ActiveDocument;
      }
      set
      {
        value.Select();
      }
    }



    public BaseDocument ActiveContent
    {
      get
      {
        return (BaseDocument)panelMain.ActiveContent;
      }
      set
      {
        value.Select();
      }
    }



    public ProjectElement ActiveElement
    {
      get
      {
        BaseDocument doc = (BaseDocument)panelMain.ActiveContent;
        if ( doc == null )
        {
          return null;
        }
        return doc.DocumentInfo.Element;
      }
    }



    public string FillParameters( string Mask, DocumentInfo Document, bool FillForRunning, out bool Error )
    {
      Error = false;
      if ( Document == null )
      {
        return Mask;
      }
      string fullDocPath = Document.FullPath;
      if ( fullDocPath == null )
      {
        return Mask;
      }
      string result = Mask.Replace( "$(Filename)", fullDocPath );
      result = result.Replace( "$(File)", System.IO.Path.GetFileName( fullDocPath ) );
      result = result.Replace( "$(FilenameWithoutExtension)", System.IO.Path.GetFileNameWithoutExtension( fullDocPath ) );
      result = result.Replace( "$(FilePath)", GR.Path.RemoveFileSpec( fullDocPath ) );

      string targetFilename = "";

      if ( Document.Element == null )
      {
        if ( StudioCore.Compiling.m_LastBuildInfo.TryGetValue( Document.FullPath, out SingleBuildInfo lastBuildInfoOfThisFile ) )
        {
          targetFilename = lastBuildInfoOfThisFile.TargetFile;
        }
      }
      else
      {
        targetFilename = Document.Element.TargetFilename;
        if ( !string.IsNullOrEmpty( Document.Element.CompileTargetFile ) )
        {
          targetFilename = Document.Element.CompileTargetFile;
        }
        if ( ( string.IsNullOrEmpty( targetFilename ) )
        &&   ( Document.Type == ProjectElement.ElementType.BASIC_SOURCE ) )
        {
          targetFilename = GR.Path.RenameExtension( fullDocPath, ".prg" );
        }
      }
      string targetPath = "";
      if ( !string.IsNullOrEmpty( targetFilename ) )
      {
        targetPath = System.IO.Path.GetDirectoryName( targetFilename );
      }
      if ( string.IsNullOrEmpty( targetPath ) )
      {
        targetPath = Document.Project.Settings.BasePath;
      }
      targetFilename = System.IO.Path.GetFileName( targetFilename );
      string targetFilenameWithoutPath = System.IO.Path.GetFileName( targetFilename );
      string targetFilenameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension( targetFilename );
      string fullTargetFilename = GR.Path.Append( targetPath, targetFilename );
      string fullTargetFilenameWithoutPath = System.IO.Path.GetFileName( fullTargetFilename );
      string fullTargetFilenameWithoutExtension = System.IO.Path.Combine( targetPath, System.IO.Path.GetFileNameWithoutExtension( fullTargetFilename ) );

      string runFilename = System.IO.Path.GetFileName( fullTargetFilename );
      string fullRunFilename = fullTargetFilename;
      string runPath = System.IO.Path.GetDirectoryName( fullRunFilename );

      // alternative run file name
      if ( ( FillForRunning )
      &&   ( Document.Element != null ) )
      {
        ProjectElement.PerConfigSettings configSettingRun = Document.Element.Settings[Document.Project.Settings.CurrentConfig.Name];
        if ( !string.IsNullOrEmpty( configSettingRun.DebugFile ) )
        {
          if ( configSettingRun.DebugFile.Contains( "$(Run" ) )
          {
            Error = true;
            StudioCore.AddToOutput( "Alternative run file name contains forbidden macro $(RunPath), $(RunFilename) or $(RunFilenameWithoutExtension)" + System.Environment.NewLine );
            return "";
          }
          fullRunFilename = FillParameters( configSettingRun.DebugFile, Document, false, out Error );
          if ( Error )
          {
            return "";
          }
          if ( !System.IO.Path.IsPathRooted( fullRunFilename ) )
          {
            // prepend build target path to filename
            fullRunFilename = System.IO.Path.Combine( targetPath, fullRunFilename );
          }
          runPath = System.IO.Path.GetDirectoryName( fullRunFilename );
          runFilename = System.IO.Path.GetFileName( fullRunFilename );
        }
      }
      string runFilenameWithoutExtension      = System.IO.Path.GetFileNameWithoutExtension(runFilename);
      string runFilenameWithoutPath           = System.IO.Path.GetFileName(runFilename);
      string fullRunFilenameWithoutExtension  = System.IO.Path.GetFileNameWithoutExtension(fullRunFilename);



      result = result.Replace( "$(BuildTargetPath)", targetPath );
      result = result.Replace( "$(BuildTargetFilename)", fullTargetFilename );
      result = result.Replace( "$(BuildTargetFilenameWithoutExtension)", fullTargetFilenameWithoutExtension );
      result = result.Replace( "$(BuildTargetFile)", targetFilename );
      result = result.Replace( "$(BuildTargetFileWithoutExtension)", targetFilenameWithoutExtension );
      result = result.Replace( "$(RunPath)", runPath );
      result = result.Replace( "$(RunFilename)", fullRunFilename );
      result = result.Replace( "$(RunFile)", runFilenameWithoutPath );
      result = result.Replace( "$(RunFilenameWithoutExtension)", fullRunFilenameWithoutExtension );

      if ( Document.Project != null )
      {
        result = result.Replace( "$(ConfigName)", Document.Project.Settings.CurrentConfig.Name );
        result = result.Replace( "$(ProjectPath)", Document.Project.Settings.BasePath );
      }
      if ( ( StudioCore.Navigating.Solution != null )
      &&   ( !string.IsNullOrEmpty( StudioCore.Navigating.Solution.Filename ) ) )
      {
        result = result.Replace( "$(SolutionPath)", System.IO.Path.GetDirectoryName( StudioCore.Navigating.Solution.Filename ) );
      }
      result = result.Replace( "$(MediaManager)", "\"" + System.IO.Path.Combine( Application.StartupPath, "mediamanager.exe" ) + "\"" );
      result = result.Replace( "$(MediaTool)", "\"" + System.IO.Path.Combine( Application.StartupPath, "mediatool.exe" ) + "\"" );

      int debugStartAddress = StudioCore.Debugging.OverrideDebugStart;
      {
        if ( ( Document.Project != null )
        &&   ( !string.IsNullOrEmpty( Document.Project.Settings.CurrentConfig.DebugStartAddressLabel ) ) )
        {
          int   dummy = -1;
          if ( !DetermineDebugStartAddress( Document, Document.Project.Settings.CurrentConfig.DebugStartAddressLabel, out dummy ) )
          {
            Error = true;
            StudioCore.AddToOutput( "Cannot determine value for debug start address from '" + Document.Project.Settings.CurrentConfig.DebugStartAddressLabel + "'" + System.Environment.NewLine );
            return "";
          }
          if ( dummy != 0 )
          {
            debugStartAddress = dummy;
          }
        }
      }
      result = result.Replace( "$(DebugStartAddress)", debugStartAddress.ToString() );
      result = result.Replace( "$(DebugStartAddressHex)", debugStartAddress.ToString( "x" ) );

      // replace symbols
      var parser = new ASMFileParser();

      int dollarPos = result.IndexOf("$(");
      while ( dollarPos != -1 )
      {
        int macroEndPos = result.IndexOf(')', dollarPos);

        if ( macroEndPos == -1 )
        {
          Error = true;
          StudioCore.AddToOutput( "Malformed Macro encountered at command " + Mask );
          return "";
        }
        string macroName = result.Substring( dollarPos + 2, macroEndPos - dollarPos - 2 );
        bool asHex = false;
        if ( macroName.StartsWith( "0x" ) )
        {
          asHex = true;
          macroName = macroName.Substring( 2 );
        }

        var tokens = parser.ParseTokenInfo( macroName, 0, macroName.Length, parser.m_TextCodeMappingRaw );
        parser.InjectASMFileInfo( Document.ASMFileInfo );
        if ( !parser.EvaluateTokens( 0, tokens, parser.m_TextCodeMappingRaw, out SymbolInfo macroValueSymbol ) )
        {
          Error = true;
          StudioCore.AddToOutput( "Failed to evaluate macro '" + macroName + "' encountered at command " + Mask + System.Environment.NewLine );
          return "";
        }
        long macroValue = macroValueSymbol.ToInteger( );
        string valueToInsert = "";
        if ( asHex )
        {
          valueToInsert = macroValue.ToString( "X" );
        }
        else
        {
          valueToInsert = macroValue.ToString();
        }
        result = result.Substring( 0, dollarPos ) + valueToInsert + result.Substring( macroEndPos + 1 );
        macroEndPos = dollarPos + valueToInsert.Length;
        /*
        if ( Document.Type == ProjectElement.ElementType.ASM_SOURCE )
        {
          string valueToInsert = "";
          if ( Document.ASMFileInfo.Labels.ContainsKey( macroName ) )
          {
            if ( asHex )
            {
              valueToInsert = Document.ASMFileInfo.Labels[macroName].AddressOrValue.ToString( "X" );
            }
            else
            {
              valueToInsert = Document.ASMFileInfo.Labels[macroName].AddressOrValue.ToString();
            }
          }
          else
          {
            Error = true;
            StudioCore.AddToOutput( "Unknown macro " + macroName + " encountered at command " + Mask + System.Environment.NewLine );
            return "";
          }
          result = result.Substring( 0, dollarPos ) + valueToInsert + result.Substring( macroEndPos + 1 );
          macroEndPos = dollarPos + valueToInsert.Length;
        }
        else
        {
          Error = true;
          StudioCore.AddToOutput( "Unknown macro " + macroName + " encountered at command " + Mask + System.Environment.NewLine );
          return "";
        }*/
        dollarPos = result.IndexOf( "$(", dollarPos );
      }
      return result;
    }



    public bool CloneSolution( Solution SourceSolution )
    {
      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Clone Solution as";
      saveDlg.Filter = FilterString( Types.Constants.FILEFILTER_SOLUTION + Types.Constants.FILEFILTER_ALL );

      if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return false;
      }

      StringBuilder   sb = new StringBuilder();

      var newSolutionFilename = saveDlg.FileName;

      var solutionData = SourceSolution.ToBuffer();

      var newSolution  = new Solution( this );
      if ( !newSolution.FromBuffer( solutionData, newSolutionFilename ) )
      {
        return false;
      }

      var solutionDir = System.IO.Path.GetDirectoryName( SourceSolution.Filename );
      var newSolutionDir = System.IO.Path.GetDirectoryName( newSolutionFilename );

      // clone all contained projects
      foreach ( var project in SourceSolution.Projects )
      {
        var projectData = project.Save();

        var clonedProject = new Project();
        clonedProject.Core = StudioCore;
        if ( !clonedProject.Load( projectData.Data(), false ) )
        {
          return false;
        }
        // need to re-map project?
        if ( GR.Path.IsSubPath( solutionDir, project.Settings.BasePath ) )
        {
          var relativeProjectFilePath = GR.Path.RelativePathTo( solutionDir, true, project.Settings.BasePath, true );
          var relativeProjectPath = GR.Path.RelativePathTo( solutionDir, true, System.IO.Path.GetDirectoryName( project.Settings.Filename ), true );

          clonedProject.Settings.BasePath = GR.Path.Append( newSolutionDir, relativeProjectFilePath );
          clonedProject.Settings.Filename = GR.Path.Append( GR.Path.Append( newSolutionDir, relativeProjectPath ), System.IO.Path.GetFileName( clonedProject.Settings.Filename ) );

          if ( !System.IO.Directory.Exists( clonedProject.Settings.BasePath ) )
          {
            System.IO.Directory.CreateDirectory( clonedProject.Settings.BasePath );
          }

          for ( int i = 0; i < project.Elements.Count; ++i )
          {
            var sourceElement = project.Elements[i];
            var targetElement = clonedProject.Elements[i];

            if ( sourceElement.DocumentInfo.Type == ProjectElement.ElementType.FOLDER )
            {
              continue;
            }

            if ( GR.Path.IsSubPath( project.Settings.BasePath, sourceElement.DocumentInfo.FullPath ) )
            {
              // don't overwrite?
              if ( !System.IO.File.Exists( targetElement.DocumentInfo.FullPath ) )
              {
                // need subfolder?
                string    targetSubfolder = System.IO.Path.GetDirectoryName( targetElement.DocumentInfo.FullPath );
                if ( !System.IO.Directory.Exists( targetSubfolder ) )
                {
                  System.IO.Directory.CreateDirectory( targetSubfolder );
                }
                System.IO.File.Copy( sourceElement.DocumentInfo.FullPath, targetElement.DocumentInfo.FullPath );
              }
            }
          }
          sb.AppendLine( $"Cloned project '{project.Settings.Filename}' to '{clonedProject.Settings.Filename}'" );
        }
        else
        {
          // what to do with projects outside?? just keep them?
          sb.AppendLine( $"Cloned project reference '{project.Settings.Filename}'" );
        }
        clonedProject.Save( clonedProject.Settings.Filename );

        newSolution.Projects.Add( clonedProject );
      }

      if ( !GR.IO.File.WriteAllBytes( newSolutionFilename, newSolution.ToBuffer() ) )
      {
        return false;
      }
      sb.AppendLine( $"Cloned solution '{SourceSolution.Filename}' to '{newSolutionFilename}'" );

      // and add to existing solution
      if ( !OpenSolution( saveDlg.FileName ) )
      {
        return false;
      }

      StudioCore.AddToOutput( sb.ToString() );
      return true;
    }



    public bool DetermineDebugStartAddress( DocumentInfo Document, string Label, out int Address )
    {
      Address = -1;
      if ( Document.Type == ProjectElement.ElementType.ASM_SOURCE )
      {
        if ( Document.ASMFileInfo.Labels.ContainsKey( Label ) )
        {
          Address = (int)Document.ASMFileInfo.Labels[Label].AddressOrValue;
          return true;
        }
      }
      if ( Label.StartsWith( "0x" ) )
      {
        if ( Label.Length >= 3 )
        {
          Address = System.Convert.ToUInt16( Label.Substring( 2 ), 16 );
          return true;
        }
      }
      else if ( Label.StartsWith( "$" ) )
      {
        if ( Label.Length >= 2 )
        {
          Address = System.Convert.ToUInt16( Label.Substring( 1 ), 16 );
          return true;
        }
      }
      ushort     debugStartAddressValue = 0;
      if ( ushort.TryParse( Label, out debugStartAddressValue ) )
      {
        Address = debugStartAddressValue;
        return true;
      }
      return false;
    }



    public void MarkAsDirty( DocumentInfo DocInfo )
    {
      if ( DocInfo == null )
      {
        return;
      }
      if ( !DocInfo.HasBeenSuccessfullyBuilt )
      {
        return;
      }
      DocInfo.HasBeenSuccessfullyBuilt = false;

      if ( DocInfo.Element != null )
      {
        foreach ( var dependency in DocInfo.Element.ForcedDependency.DependentOnFile )
        {
          ProjectElement elementDependency = DocInfo.Project.GetElementByFilename(dependency.Filename);
          if ( elementDependency == null )
          {
            return;
          }
          MarkAsDirty( elementDependency.DocumentInfo );
        }
      }
      if ( DocInfo.Project != null )
      {
        lock ( DocInfo.DeducedDependency )
        {
          if ( !DocInfo.DeducedDependency.ContainsKey( DocInfo.Project.Settings.CurrentConfig.Name ) )
          {
            DocInfo.DeducedDependency.Add( DocInfo.Project.Settings.CurrentConfig.Name, new DependencyBuildState() );
          }
          foreach ( var deducedDependency in DocInfo.DeducedDependency[DocInfo.Project.Settings.CurrentConfig.Name].BuildState )
          {
            ProjectElement elementDependency = DocInfo.Project.GetElementByFilename(deducedDependency.Key);
            if ( elementDependency == null )
            {
              return;
            }
            MarkAsDirty( elementDependency.DocumentInfo );
          }
        }
      }
    }



    private bool StartCompile( DocumentInfo DocumentToBuild, DocumentInfo DocumentToDebug, DocumentInfo DocumentToRun, Solution Solution, bool CreatePreProcessedFile, bool CreateRelocationFile )
    {
      StudioCore.TaskManager.AddTask( new Tasks.TaskCompile( DocumentToBuild, DocumentToDebug, DocumentToRun, ActiveDocumentInfo, Solution, CreatePreProcessedFile, CreateRelocationFile ) );
      return true;
    }



    public void OnBuildFinished( DocumentInfo baseDoc, DocumentInfo ActiveDocumentInfo )
    {
    }



    public void Rebuild( DocumentInfo DocInfo )
    {
      if ( AppState != Types.StudioState.NORMAL )
      {
        return;
      }

      MarkAsDirty( DocInfo );

      AppState = Types.StudioState.BUILD;
      StudioCore.Debugging.OverrideDebugStart = -1;
      if ( !StartCompile( DocInfo, null, null, StudioCore.Navigating.Solution, false, false ) )
      {
        AppState = Types.StudioState.NORMAL;
      }
    }



    public void Build( DocumentInfo Document )
    {
      Build( Document, false, false );
    }



    public void Build( DocumentInfo Document, bool CreatePreProcessedFile, bool CreateRelocationFile )
    {
      if ( AppState != Types.StudioState.NORMAL )
      {
        return;
      }
      if ( CreateRelocationFile )
      {
        AppState = Types.StudioState.BUILD_RELOCATION_FILE;

        // always build if relocation file is wanted
        MarkAsDirty( Document );
      }
      else if ( CreatePreProcessedFile )
      {
        AppState = Types.StudioState.BUILD_PRE_PROCESSED_FILE;
        // always build if preprocessed file is wanted
        MarkAsDirty( Document );
      }
      else
      {
        AppState = Types.StudioState.BUILD;
      }
      StudioCore.Debugging.OverrideDebugStart = -1;
      if ( !StartCompile( Document, null, null, StudioCore.Navigating.Solution, CreatePreProcessedFile, CreateRelocationFile ) )
      {
        AppState = Types.StudioState.NORMAL;
      }
    }



    private void Compile( DocumentInfo Document )
    {
      if ( AppState != Types.StudioState.NORMAL )
      {
        return;
      }
      AppState = Types.StudioState.COMPILE;
      StudioCore.Debugging.OverrideDebugStart = -1;
      if ( !StartCompile( Document, null, null, StudioCore.Navigating.Solution, false, false ) )
      {
        AppState = Types.StudioState.NORMAL;
      }
    }



    private void mainToolCompile_Click( object sender, EventArgs e )
    {
      ApplyFunction( RetroDevStudio.Types.Function.BUILD );
    }



    public void AddToOutputAndShow( string Text )
    {
      if ( InvokeRequired )
      {
        try
        {
          Invoke( new AddToOutputAndShowCallback( AddToOutputAndShow ), new object[] { Text } );
        }
        catch ( System.ObjectDisposedException )
        {
        }
      }
      else
      {
        //Debug.Log( Text );
        m_Output.AppendText( Text );
        m_Output.Show();
      }
    }



    public void AddOutputMessages( Types.ASM.FileInfo ASMFileInfo )
    {
      if ( ASMFileInfo == null )
      {
        return;
      }
      foreach ( System.Collections.Generic.KeyValuePair<int, Parser.ParserBase.ParseMessage> msg in ASMFileInfo.Messages )
      {
        Parser.ParserBase.ParseMessage message = msg.Value;
        if ( message.Type == RetroDevStudio.Parser.ParserBase.ParseMessage.LineType.MESSAGE )
        {
          StudioCore.AddToOutput( message.Message + System.Environment.NewLine );
        }
      }
    }



    void ReadLabelsFromFile( string Filename )
    {
      StudioCore.Debugging.Debugger.ClearLabels();
      try
      {
        string[] labelInfos = System.IO.File.ReadAllLines(Filename);

        foreach ( string labelInfo in labelInfos )
        {
          int equPos = labelInfo.IndexOf('=');
          int semPos = labelInfo.IndexOf(';');
          if ( equPos != -1 )
          {
            string labelName = labelInfo.Substring(0, equPos).Trim();
            string labelValueText = "";
            int labelValue = -1;
            if ( semPos == -1 )
            {
              labelValueText = labelInfo.Substring( equPos + 1 ).Trim();
            }
            else
            {
              labelValueText = labelInfo.Substring( equPos + 1, semPos - equPos - 1 ).Trim();
            }
            if ( labelValueText.StartsWith( "$" ) )
            {
              labelValue = System.Convert.ToInt32( labelValueText.Substring( 1 ), 16 );
            }
            else
            {
              int.TryParse( labelValueText, out labelValue );
            }
            StudioCore.Debugging.Debugger.AddLabel( labelName, labelValue );
            //dh.Log( "Label: " + labelName + "=" + labelValue );
          }
        }
      }
      catch ( System.IO.IOException io )
      {
        StudioCore.AddToOutput( "ReadLabelsFromFile failed: " + io.ToString() + System.Environment.NewLine );
      }
    }



    public bool RunCompiledFile( DocumentInfo Document, Types.CompileTargetType TargetType )
    {
      try
      {
        if ( Document.Element != null )
        {
          StudioCore.AddToOutput( "Running " + Document.Element.Name + System.Environment.NewLine );
        }
        else
        {
          StudioCore.AddToOutput( "Running " + Document.DocumentFilename + System.Environment.NewLine );
        }

        ToolInfo toolRun = StudioCore.DetermineTool( Document, ToolInfo.ToolType.EMULATOR );
        if ( toolRun == null )
        {
          StudioCore.MessageBox( "No emulator tool has been configured yet!", "Missing emulator tool" );
          StudioCore.AddToOutput( "There is no emulator tool configured!" );
          return false;
        }
        if ( toolRun.IsInternal )
        {
          StudioCore.MessageBox( "The internal Tiny64 debugger is not able to run files, only debug!", "Cannot run file!" );
          StudioCore.AddToOutput( "The internal Tiny64 debugger is not able to run files, only debug!" );
          return false;
        }

        // check file version (WinVICE remote debugger changes)
        if ( ( StudioCore.Debugging.Debugger != null )
        &&   ( !StudioCore.Debugging.Debugger.CheckEmulatorVersion( toolRun ) ) )
        {
          return false;
        }

        if ( !StudioCore.Executing.PrepareStartProcess( toolRun, Document ) )
        {
          return false;
        }

        if ( !System.IO.Directory.Exists( StudioCore.Executing.RunProcess.StartInfo.WorkingDirectory.Trim( new char[] { '\"' } ) ) )
        {
          StudioCore.AddToOutput( "The determined working directory" + StudioCore.Executing.RunProcess.StartInfo.WorkingDirectory + " does not exist" + System.Environment.NewLine );
          return false;
        }

        string runArguments = toolRun.PRGArguments;
        if ( Parser.ASMFileParser.IsCartridge( TargetType ) )
        {
          runArguments = toolRun.CartArguments;
        }
        if ( StudioCore.Settings.TrueDriveEnabled )
        {
          runArguments = toolRun.TrueDriveOnArguments + " " + runArguments;
        }
        else
        {
          runArguments = toolRun.TrueDriveOffArguments + " " + runArguments;
        }

        if ( ( Document != null )
        &&   ( Document.ASMFileInfo != null )
        &&   ( toolRun.PassLabelsToEmulator ) )
        {
          string labelInfo = Document.ASMFileInfo.LabelsAsFile( EmulatorInfo.LabelFormat( toolRun ) );
          if ( labelInfo.Length > 0 )
          {
            try
            {
              StudioCore.Debugging.TempDebuggerStartupFilename = System.IO.Path.GetTempFileName();

              switch ( EmulatorInfo.LabelFormat( toolRun ) )
              {
                case Types.ASM.LabelFileFormat.C64DEBUGGER:
                  runArguments = "-vicesymbols \"" + StudioCore.Debugging.TempDebuggerStartupFilename + "\" "
                      + runArguments;
                  break;
                case Types.ASM.LabelFileFormat.VICE:
                default:
                  runArguments = "-moncommands \"" + StudioCore.Debugging.TempDebuggerStartupFilename + "\" " + runArguments;
                  labelInfo += "\nexit\n";
                  break;
              }
              System.IO.File.WriteAllText( StudioCore.Debugging.TempDebuggerStartupFilename, labelInfo );
            }
            catch ( System.IO.IOException ioe )
            {
              StudioCore.MessageBox( ioe.Message, "Error writing temporary file" );
              StudioCore.AddToOutput( "Error writing temporary file" );
              StudioCore.Debugging.TempDebuggerStartupFilename = "";
              return false;
            }
          }
        }
        bool error = false;
        StudioCore.Executing.RunProcess.StartInfo.Arguments = FillParameters( runArguments, Document, true, out error );
        if ( error )
        {
          return false;
        }

        StudioCore.Executing.RunProcess.Exited += new EventHandler( runProcess_Exited );
        StudioCore.AddToOutput( "Calling " + StudioCore.Executing.RunProcess.StartInfo.FileName + " with " + StudioCore.Executing.RunProcess.StartInfo.Arguments + System.Environment.NewLine );
        StudioCore.SetStatus( "Running..." );

        SetGUIForWaitOnExternalTool( true );

        if ( !StudioCore.Executing.StartPreparedProcess() )
        {
          return false;
        }
        return true;
      }
      catch ( Exception ex )
      {
        SetGUIForWaitOnExternalTool( false );
        StudioCore.AddToOutput( "Internal Error in RunCompiledFile: " + ex.ToString() );
        StudioCore.SetStatus( "Ready" );
        return false;
      }
    }



    public void runProcess_Exited( object sender, EventArgs e )
    {
      if ( StudioCore.Executing.RunProcess == null )
      {
        StudioCore.AddToOutput( "Run exited unexpectedly" + System.Environment.NewLine );
      }
      else
      {
        try
        {
          if ( StudioCore.Executing.EventOutCompleted != null )
          {
            StudioCore.Executing.EventOutCompleted.WaitOne();
            StudioCore.Executing.EventOutCompleted.Close();
            StudioCore.Executing.EventOutCompleted = null;
          }
          if ( StudioCore.Executing.EventErrCompleted != null )
          {
            StudioCore.Executing.EventErrCompleted.WaitOne();
            StudioCore.Executing.EventErrCompleted.Close();
            StudioCore.Executing.EventErrCompleted = null;
          }

          StudioCore.AddToOutput( "Run exited with result code " + StudioCore.Executing.RunProcess?.ExitCode + System.Environment.NewLine );
          StudioCore.Executing.RunProcess?.Close();
          StudioCore.Executing.RunProcess?.Dispose();
        }
        catch ( System.Exception ex )
        {
          StudioCore.AddToOutput( "Run aborted with error: " + ex.Message + System.Environment.NewLine );
        }
      }
      StudioCore.Executing.RunProcess = null;

      StopDebugging();
    }



    private void mainToolSave_Click( object sender, EventArgs e )
    {
      BaseDocument baseDoc = (BaseDocument)panelMain.ActiveDocument;
      if ( baseDoc == null )
      {
        return;
      }
      if ( ( baseDoc.DocumentInfo.Project != null )
      && ( baseDoc.DocumentInfo.Project.Modified ) )
      {
        if ( !SaveProject( baseDoc.DocumentInfo.Project ) )
        {
          return;
        }
      }
      baseDoc.Save( BaseDocument.SaveMethod.SAVE );
      if ( baseDoc.DocumentInfo.Project != null )
      {
        baseDoc.DocumentInfo.Project.Save( baseDoc.DocumentInfo.Project.Settings.Filename );
      }
    }



    public ProjectElement CreateNewElement( ProjectElement.ElementType Type, string StartName, Project Project )
    {
      if ( Project == null )
      {
        CreateNewDocument( Type, null );
        return null;
      }
      return CreateNewElement( Type, StartName, Project, Project.Node );
    }



    public ProjectElement CreateNewElement( ProjectElement.ElementType Type, string StartName, Project Project, DecentForms.TreeView.TreeNode ParentNode )
    {
      if ( Project == null )
      {
        return null;
      }
      Project projectToAdd = m_SolutionExplorer.ProjectFromNode( ParentNode );
      ProjectElement elementParent = m_SolutionExplorer.ElementFromNode( ParentNode );

      ProjectElement element = projectToAdd.CreateElement( Type, ParentNode );
      element.Name = StartName;
      element.Node.Text = StartName;
      element.ProjectHierarchy = m_SolutionExplorer.GetElementHierarchy( element.Node );
      element.DocumentInfo.Project = Project;

      if ( element.Document != null )
      {
        RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.DOCUMENT_CREATED, element.DocumentInfo ) );
      }
      projectToAdd.ShowDocument( element );
      if ( element.Document != null )
      {
        element.Document.SetModified();
      }
      RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.DOCUMENT_INFO_CREATED, element.DocumentInfo ) );
      return element;
    }



    public void AddBreakpoint( Types.Breakpoint Breakpoint )
    {
      if ( AppState == Types.StudioState.NORMAL )
      {
        if ( !StudioCore.Debugging.BreakPoints.ContainsKey( Breakpoint.DocumentFilename ) )
        {
          StudioCore.Debugging.BreakPoints[Breakpoint.DocumentFilename] = new List<RetroDevStudio.Types.Breakpoint>();
        }
        StudioCore.Debugging.BreakPoints[Breakpoint.DocumentFilename].Add( Breakpoint );
      }
      else if ( AppState == Types.StudioState.DEBUGGING_BROKEN )
      {
        if ( !StudioCore.Debugging.BreakPoints.ContainsKey( Breakpoint.DocumentFilename ) )
        {
          StudioCore.Debugging.BreakPoints[Breakpoint.DocumentFilename] = new List<RetroDevStudio.Types.Breakpoint>();
        }
        StudioCore.Debugging.BreakPoints[Breakpoint.DocumentFilename].Add( Breakpoint );
        StudioCore.Debugging.Debugger.AddBreakpoint( Breakpoint );
      }
      else
      {
        return;
      }
      m_DebugBreakpoints.AddBreakpoint( Breakpoint );
    }



    private void RemoveBreakpoint( Types.Breakpoint Breakpoint )
    {
      if ( ( AppState == Types.StudioState.NORMAL )
      ||   ( AppState == Types.StudioState.DEBUGGING_BROKEN ) )
      {
        if ( StudioCore.Debugging.BreakPoints.ContainsKey( Breakpoint.DocumentFilename ) )
        {
          foreach ( Types.Breakpoint breakPoint in StudioCore.Debugging.BreakPoints[Breakpoint.DocumentFilename] )
          {
            if ( breakPoint == Breakpoint )
            {
              StudioCore.Debugging.BreakPoints[Breakpoint.DocumentFilename].Remove( breakPoint );
              m_DebugBreakpoints.RemoveBreakpoint( breakPoint );
              if ( AppState == Types.StudioState.NORMAL )
              {
                StudioCore.Debugging.Debugger?.RemoveBreakpoint( breakPoint.RemoteIndex );
              }
              else
              {
                StudioCore.Debugging.Debugger.RemoveBreakpoint( breakPoint.RemoteIndex, breakPoint );
              }
              break;
            }
          }
        }

        if ( Breakpoint.DocumentFilename != "RetroDevStudio.DebugBreakpoints" )
        {
          var doc = StudioCore.Navigating.FindDocumentByFilename( Breakpoint.DocumentFilename );
          if ( ( doc != null )
          &&   ( doc.Type == ProjectElement.ElementType.ASM_SOURCE ) )
          {
            SourceASMEx asm = (SourceASMEx)doc.BaseDoc;
            asm?.RemoveBreakpoint( Breakpoint );
          }
        }
      }
    }



    public void Document_DocumentEvent( BaseDocument.DocEvent Event )
    {
      if ( InvokeRequired )
      {
        Invoke( new DocumentEventHandlerCallback( Document_DocumentEvent ), new object[] { Event } );
        return;
      }

      switch ( Event.EventType )
      {
        case BaseDocument.DocEvent.Type.BOOKMARK_ADDED:
          m_Bookmarks.AddBookmark( Event.LineIndex, Event.Doc.DocumentInfo );
          break;
        case BaseDocument.DocEvent.Type.BOOKMARK_REMOVED:
          m_Bookmarks.RemoveBookmark( Event.LineIndex, Event.Doc.DocumentInfo );
          break;
        case BaseDocument.DocEvent.Type.ALL_BOOKMARKS_OF_DOCUMENT_REMOVED:
          m_Bookmarks.RemoveAllBookmarksForDocument( Event.Doc.DocumentInfo );
          break;
        case BaseDocument.DocEvent.Type.BOOKMARKS_UPDATED:
          m_Bookmarks.UpdateAllBookmarksForDocument( Event.Doc.DocumentInfo );
          break;
        case BaseDocument.DocEvent.Type.BREAKPOINT_ADDED:
          if ( ( AppState == Types.StudioState.NORMAL )
          ||   ( AppState == Types.StudioState.DEBUGGING_BROKEN ) )
          {
            AddBreakpoint( Event.Breakpoint );
          }
          break;
        case BaseDocument.DocEvent.Type.BREAKPOINT_REMOVED:
          if ( ( AppState == Types.StudioState.NORMAL )
          ||   ( AppState == Types.StudioState.DEBUGGING_BROKEN ) )
          {
            RemoveBreakpoint( Event.Breakpoint );
          }
          break;
        case BaseDocument.DocEvent.Type.BREAKPOINT_UPDATED:
          // address changed
          m_DebugBreakpoints.UpdateBreakpoint( Event.Breakpoint );
          break;
      }
    }



    private void AddNewDocumentOrElement( ProjectElement.ElementType Type, string Description, Project ParentProject, DecentForms.TreeView.TreeNode ParentNode )
    {
      if ( ParentProject != null )
      {
        var dialogResult = System.Windows.Forms.MessageBox.Show( "Add the new document to the current project?\r\nIf you choose no, the document will not be created as part of the current project.", "Add to current project?", System.Windows.Forms.MessageBoxButtons.YesNoCancel );
        if ( dialogResult == DialogResult.Cancel )
        {
          return;
        }
        if ( dialogResult == DialogResult.Yes )
        {
          AddNewElement( Type, Description, ParentProject, ParentNode );
          return;
        }
        // fall through
      }

      // project-less doc - no forced file name
      var doc = CreateNewDocument( Type, null );
      doc.SetModified();
    }



    private void mainToolNewASMFile_Click( object sender, EventArgs e )
    {
      AddNewDocumentOrElement( ProjectElement.ElementType.ASM_SOURCE, "ASM File", m_CurrentProject, ( m_CurrentProject != null ) ? m_CurrentProject.Node : null );
    }



    private void mainToolNewItem_ButtonClick( object sender, EventArgs e )
    {
      if ( m_CurrentProject == null )
      {
        mainToolNewProject_Click( sender, e );
        return;
      }
    }



    public Project NewProjectWizard( string ProjectName )
    {
      var projectWizard = new FormProjectWizard( ProjectName, StudioCore.Settings, StudioCore );

      if ( projectWizard.ShowDialog() != DialogResult.OK )
      {
        return null;
      }

      Project newProject = new Project();

      newProject.Core               = StudioCore;
      newProject.Settings.Name      = projectWizard.ProjectName;
      newProject.Settings.Filename  = projectWizard.ProjectFilename;
      newProject.Settings.BasePath  = System.IO.Path.GetDirectoryName( newProject.Settings.Filename );
      newProject.Node               = new DecentForms.TreeView.TreeNode();
      newProject.Node.Tag           = new SolutionExplorer.TreeItemInfo() { Project = newProject };
      newProject.Node.Text          = newProject.Settings.Name;


      try
      {
        System.IO.Directory.CreateDirectory( projectWizard.ProjectPath );
      }
      catch ( System.Exception e )
      {
        System.Windows.Forms.MessageBox.Show( "Could not create project folder:" + System.Environment.NewLine + e.Message, "Could not create project folder" );
        return null;
      }

      Text += " - " + newProject.Settings.Name;

      // TODO - adjust GUI to changed project
      bool addedSolution = false;
      if ( StudioCore.Navigating.Solution == null )
      {
        StudioCore.Navigating.Solution = new Solution( this );
        addedSolution = true;
      }
      StudioCore.Navigating.Solution.Projects.Add( newProject );

      if ( projectWizard.CreateRepository )
      {
        bool repoCreated = global::SourceControl.Controller.CreateRepositoryInFolder( newProject.FullPath( "" ), out SourceControl.Controller controller );
        if ( SourceControl.Controller.IsFolderUnderSourceControl( newProject.FullPath( "" ) ) )
        {
          newProject.SourceControl = controller;

          // add the project file .c64
          newProject.SourceControl.AddFileToRepository( newProject.Settings.Filename );
        }
      }

      // TODO - should be different
      m_SolutionExplorer.treeProject.Nodes.Add( newProject.Node );
      newProject.Node.Expand();

      RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.SOLUTION_OPENED ) );
      foreach ( var project in StudioCore.Navigating.Solution.Projects )
      {
        RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.PROJECT_OPENED, project ) );
      }

      SetActiveProject( newProject );

      if ( SaveProject( newProject ) )
      {
        if ( ( projectWizard.CreateRepository )
        &&   ( newProject.SourceControl != null ) )
        {
          if ( addedSolution )
          {
            newProject.SourceControl.AddFileToRepository( StudioCore.Navigating.Solution.Filename );
          }
          newProject.SourceControl.StageAllChanges();
          newProject.SourceControl.CommitAllChanges( StudioCore.Settings.SourceControlInfo.CommitAuthor, StudioCore.Settings.SourceControlInfo.CommitAuthorEmail, "Initial" );
        }
      }

      UpdateUndoSettings();
      return newProject;
    }



    public bool CreateNewProject()
    {
      return ( AddNewProject( false ) != null );
    }



    public Project AddNewProject( bool AddToSolution )
    {
      if ( StudioCore.Navigating.Solution == null )
      {
        return AddNewProjectAndOrSolution();
      }

      string projectName = "New Project";
      Project newProject = NewProjectWizard( projectName );
      if ( newProject == null )
      {
        return null;
      }

      if ( ( !AddToSolution )
      &&   ( !CloseSolution() ) )
      {
        return null;
      }


      projectToolStripMenuItem.Visible = true;
      foreach ( var configName in newProject.Settings.GetConfigurationNames() )
      {
        mainToolConfig.Items.Add( configName );
        if ( ( newProject.Settings.CurrentConfig != null )
        &&   ( configName == newProject.Settings.CurrentConfig.Name ) )
        {
          mainToolConfig.SelectedItem = configName;
        }
      }
      if ( mainToolConfig.SelectedItem == null )
      {
        mainToolConfig.SelectedItem = "Default";
      }
      return newProject;
    }



    private void mainToolNewProject_Click( object sender, EventArgs e )
    {
      AddNewProject( true );
    }



    public bool CloseProject( Project ProjectToClose )
    {
      if ( s_SystemShutdown )
      {
        // changes are saved as restart data
        return true;
      }

      if ( ProjectToClose == null )
      {
        return true;
      }
      if ( ProjectToClose.Modified )
      {
        System.Windows.Forms.DialogResult saveResult = System.Windows.Forms.MessageBox.Show( "The project " + ProjectToClose.Settings.Name + " has been modified. Do you want to save the changes now?", "Save Changes?", MessageBoxButtons.YesNoCancel );

        if ( saveResult == DialogResult.Yes )
        {
          if ( !SaveProject( ProjectToClose ) )
          {
            return false;
          }
        }
        else if ( saveResult == DialogResult.Cancel )
        {
          return false;
        }
      }
      bool changes = false;
      foreach ( ProjectElement element in ProjectToClose.Elements )
      {
        if ( element.Document != null )
        {
          if ( element.Document.Modified )
          {
            changes = true;
            break;
          }
        }
      }

      if ( changes )
      {
        var endButtons = MessageBoxButtons.YesNoCancel;
        if ( StudioCore.ShuttingDown )
        {
          endButtons = MessageBoxButtons.YesNo;
        }
        DialogResult res = System.Windows.Forms.MessageBox.Show("There are changes in one or more items. Do you want to save them before closing?", "Unsaved changes, save now?", endButtons );
        if ( res == DialogResult.Cancel )
        {
          return false;
        }
        if ( res == DialogResult.Yes )
        {
          foreach ( ProjectElement element in ProjectToClose.Elements )
          {
            if ( element.Document != null )
            {
              element.Document.Save( BaseDocument.SaveMethod.SAVE );
            }
          }
        }
      }

      foreach ( ProjectElement element in ProjectToClose.Elements )
      {
        if ( element.Document != null )
        {
          element.Document.ForceClose();
        }

        RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.DOCUMENT_INFO_REMOVED, element.DocumentInfo ) );
      }

      try
      {
        m_SolutionExplorer.treeProject.Nodes.Remove( ProjectToClose.Node );
      }
      catch
      {
      }

      // adjust GUI to changed project
      StudioCore.Navigating.Solution.RemoveProject( ProjectToClose );
      RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.PROJECT_CLOSED, ProjectToClose ) );
      if ( m_CurrentProject == ProjectToClose )
      {
        mainToolConfig.Items.Clear();
        m_CurrentProject = null;
      }

      projectToolStripMenuItem.Visible = false;
      StudioCore.Debugging.RemoveAllBreakpoints();
      return true;
    }



    public string FilterString( string Source )
    {
      if ( string.IsNullOrEmpty( Source ) )
      {
        return Source;
      }
      return Source.Substring( 0, Source.Length - 1 );
    }



    public bool SaveProject( Project ProjectToSave )
    {
      if ( ProjectToSave == null )
      {
        return false;
      }
      if ( ProjectToSave.Settings.Filename == null )
      {
        System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

        saveDlg.Title = "Save Project as";
        saveDlg.Filter = FilterString( Types.Constants.FILEFILTER_PROJECT + Types.Constants.FILEFILTER_ALL );
        if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
        {
          return false;
        }
        ProjectToSave.Settings.Filename = saveDlg.FileName;
        ProjectToSave.Settings.BasePath = System.IO.Path.GetDirectoryName( saveDlg.FileName );
      }
      if ( !ProjectToSave.Save( ProjectToSave.Settings.Filename ) )
      {
        return false;
      }
      //Settings.UpdateInMRU( ProjectToSave.Settings.Filename, this );
      return true;
    }



    private void closeProjectToolStripMenuItem_Click( object sender, EventArgs e )
    {
      if ( m_CurrentProject != null )
      {
        if ( StudioCore.Navigating.Solution.Projects.Count == 1 )
        {
          System.Windows.Forms.MessageBox.Show( "You can't remove the last project from a solution.", "Last Project!", MessageBoxButtons.OK );
          return;
        }
        CloseProject( m_CurrentProject );
        StudioCore.Navigating.Solution.Modified = true;
        SaveSolution();
      }
    }



    private void saveProjectToolStripMenuItem_Click( object sender, EventArgs e )
    {
      SaveProject( m_CurrentProject );
    }



    public void SetActiveProject( Project NewProject )
    {
      if ( m_LoadingProject )
      {
        return;
      }
      if ( m_CurrentProject != NewProject )
      {
        m_CurrentProject = NewProject;
        RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.ACTIVE_PROJECT_CHANGED, NewProject ) );
        if ( mainToolConfig.ComboBox != null )
        {
          mainToolConfig.Items.Clear();
          if ( NewProject != null )
          {
            foreach ( var configName in NewProject.Settings.GetConfigurationNames() )
            {
              mainToolConfig.Items.Add( configName );
              if ( ( NewProject.Settings.CurrentConfig != null )
              && ( configName == NewProject.Settings.CurrentConfig.Name ) )
              {
                mainToolConfig.SelectedItem = configName;
              }
            }
            if ( mainToolConfig.SelectedItem == null )
            {
              mainToolConfig.SelectedItem = "Default";
            }
          }
        }
      }
    }



    public Project OpenProject( string Filename )
    {
      if ( StudioCore.Navigating.Solution != null )
      {
        foreach ( Project project in StudioCore.Navigating.Solution.Projects )
        {
          if ( GR.Path.IsPathEqual( Filename, project.Settings.Filename ) )
          {
            System.Windows.Forms.MessageBox.Show( "The project " + Filename + " is already opened in this solution.", "Project already opened" );
            return null;
          }
        }
      }

      bool createdNewSolution = false;
      if ( StudioCore.Navigating.Solution == null )
      {
        createdNewSolution = true;
        StudioCore.Navigating.Solution = new Solution( this );
        StudioCore.Navigating.Solution.DuringLoad = true;
        SaveSolution();
      }

      Project newProject = new Project();
      newProject.Core = StudioCore;
      m_LoadingProject = true;
      if ( newProject.Load( Filename ) )
      {
        m_LoadingProject = false;
        StudioCore.Navigating.Solution.Projects.Add( newProject );
        m_SolutionExplorer.treeProject.Nodes.Add( newProject.Node );

        StudioCore.Navigating.Solution.DuringLoad = true;

        projectToolStripMenuItem.Visible = true;

        if ( SourceControl.Controller.IsFolderUnderSourceControl( newProject.FullPath( "" ) ) )
        {
          newProject.SourceControl = new SourceControl.Controller( newProject.FullPath( "" ) );
        }

        if ( createdNewSolution )
        {
          RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.SOLUTION_OPENED ) );
        }
        RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.PROJECT_OPENED, newProject ) );

        SetActiveProject( newProject );

        StudioCore.TaskManager.AddTask( new Tasks.TaskPreparseFilesInProject( newProject, mainToolConfig.SelectedItem.ToString() ) );

        return newProject;
      }
      m_LoadingProject = false;
      return null;
    }



    public void PreparseFilesInProject( Project newProject, string SelectedConfig )
    {
      List<string> updatedFiles = new List<string>();

      List<ProjectElement> elementsToPreParse = new List<ProjectElement>(newProject.Elements);

      try
      {
        // check if main doc is set, parse this one first as it's likely to include most other files
        if ( !string.IsNullOrEmpty( newProject.Settings.MainDocument ) )
        {
          ProjectElement mainElement = newProject.GetElementByFilename(newProject.Settings.MainDocument);

          if ( mainElement != null )
          {
            elementsToPreParse.Remove( mainElement );
            elementsToPreParse.Insert( 0, mainElement );
          }
        }


        foreach ( ProjectElement element in elementsToPreParse )
        {
          if ( element.Document == null )
          {
            continue;
          }
          if ( element.DocumentInfo.Type == ProjectElement.ElementType.ASM_SOURCE )
          {
            if ( updatedFiles.Contains( element.DocumentInfo.FullPath ) )
            {
              // do not reparse already parsed element
              continue;
            }
            ParseFile( StudioCore.Compiling.ParserASM, element.DocumentInfo, newProject.Settings.Configuration( SelectedConfig ), null, false, false, false, out Types.ASM.FileInfo asmFileInfo );
            updatedFiles.Add( element.DocumentInfo.FullPath );

            if ( element.Document != null )
            {
              ( (SourceASMEx)element.Document ).SetLineInfos( asmFileInfo );
            }
            var uniqueDocs = new GR.Collections.Set<string>();
            foreach ( var doc in asmFileInfo.SourceInfo )
            {
              uniqueDocs.Add( doc.Value.FullPath );
            }
            foreach ( var doc in uniqueDocs )
            {
              var actualDoc = newProject.GetElementByFilename( doc );
              if ( actualDoc != null )
              {
                actualDoc.DocumentInfo.SetASMFileInfo( asmFileInfo );
              }
            }

            StudioCore.TaskManager.AddTask( new Tasks.TaskUpdateKeywords( element.Document ) );

            foreach ( var dependencyBuildState in element.DocumentInfo.DeducedDependency.Values )
            {
              foreach ( var dependency in dependencyBuildState.BuildState.Keys )
              {
                ProjectElement element2 = newProject.GetElementByFilename(dependency);
                if ( ( element2 != null )
                &&   ( element2.DocumentInfo.Type == ProjectElement.ElementType.ASM_SOURCE ) )
                {
                  if ( element2.Document != null )
                  {
                    ( (SourceASMEx)element2.Document ).SetLineInfos( asmFileInfo );
                  }
                  updatedFiles.Add( element2.DocumentInfo.FullPath );
                }
              }
            }
          }
          else if ( element.DocumentInfo.Type == ProjectElement.ElementType.BASIC_SOURCE )
          {
            if ( updatedFiles.Contains( element.DocumentInfo.FullPath ) )
            {
              // do not reparse already parsed element
              continue;
            }
            ParseFile( StudioCore.Compiling.ParserBasic, element.DocumentInfo, newProject.Settings.Configuration( SelectedConfig ), null, false, false, false, out Types.ASM.FileInfo asmFileInfo );
            updatedFiles.Add( element.DocumentInfo.FullPath );

            StudioCore.TaskManager.AddTask( new Tasks.TaskUpdateKeywords( element.Document ) );
          }
        }
        //m_CompileResult.ClearMessages();
        if ( m_ActiveSource != null )
        {
          //Debug.Log( "RefreshFromDocument after openproject" );
          m_Outline.RefreshFromDocument( m_ActiveSource );
          m_LabelExplorer.RefreshFromDocument( m_ActiveSource );
        }
        UpdateCaption();
        SaveSolution();
      }
      catch ( Exception ex )
      {
        StudioCore.AddToOutput( "An error occurred during opening and preparsing the project\r\n" + ex.ToString() );
      }
    }



    private void UpdateCaption()
    {
      if ( InvokeRequired )
      {
        Invoke( new ParameterLessCallback( UpdateCaption ) );
        return;
      }
      if ( ( CurrentProject != null )
      &&   ( StudioCore.Navigating.Solution != null ) )
      {
        Text = "C64Studio - " + StudioCore.Navigating.Solution.Name + " - " + CurrentProject.Settings.Name;
      }
      else
      {
        Text = "C64Studio";
      }
    }



    private void projectOpenToolStripMenuItem_Click( object sender, EventArgs e )
    {
      CloseSolution();

      System.Windows.Forms.OpenFileDialog openDlg = new System.Windows.Forms.OpenFileDialog();
      openDlg.Title = "Open solution or project";
      openDlg.Filter = FilterString( Types.Constants.FILEFILTER_SOLUTION_OR_PROJECTS + Types.Constants.FILEFILTER_SOLUTION + Types.Constants.FILEFILTER_PROJECT + Types.Constants.FILEFILTER_ALL );
      openDlg.InitialDirectory = StudioCore.Settings.DefaultProjectBasePath;
      if ( openDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return;
      }
      string extension = System.IO.Path.GetExtension(openDlg.FileName).ToUpper();
      if ( extension == ".S64" )
      {
        OpenSolution( openDlg.FileName );
      }
      else if ( extension == ".C64" )
      {
        OpenProject( openDlg.FileName );
      }
    }



    private void editorOpenToolStripMenuItem_Click( object sender, EventArgs e )
    {
      System.Windows.Forms.OpenFileDialog openDlg = new System.Windows.Forms.OpenFileDialog();
      openDlg.Title = "Open editor file";
      openDlg.Filter = FilterString( Types.Constants.FILEFILTER_CHARSET_SCREEN + Types.Constants.FILEFILTER_GRAPHIC_SCREEN + Types.Constants.FILEFILTER_ALL );
      if ( openDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return;
      }
      OpenFile( openDlg.FileName );
    }



    private void BuildAndRun( DocumentInfo DocumentToBuild, DocumentInfo DocumentToRun )
    {
      if ( AppState != Types.StudioState.NORMAL )
      {
        return;
      }
      AppState = Types.StudioState.BUILD_AND_RUN;
      StudioCore.Debugging.OverrideDebugStart = -1;
      if ( !StartCompile( DocumentToBuild, null, DocumentToRun, StudioCore.Navigating.Solution, false, false ) )
      {
        AppState = Types.StudioState.NORMAL;
      }
    }



    private void mainToolCompileAndRun_Click( object sender, EventArgs e )
    {
      ApplyFunction( RetroDevStudio.Types.Function.BUILD_AND_RUN );
    }



    public void ProjectConfigChanged()
    {
      if ( m_CurrentProject != null )
      {
        foreach ( var element in m_CurrentProject.Elements )
        {
          element.DocumentInfo.HasBeenSuccessfullyBuilt = false;
        }
      }
      foreach ( IDockContent dockContent in panelMain.Documents )
      {
        BaseDocument baseDoc = (BaseDocument)dockContent;

        baseDoc.DocumentInfo.HasBeenSuccessfullyBuilt = false;
      }
    }



    public bool ImportExistingFiles( DecentForms.TreeView.TreeNode Node )
    {
      Project projectToAddTo = null;
      if ( Node != null )
      {
        projectToAddTo = m_SolutionExplorer.ProjectFromNode( Node );
      }
      if ( projectToAddTo == null )
      {
        projectToAddTo = m_CurrentProject;
        if ( projectToAddTo == null )
        {
          return false;
        }
        Node = projectToAddTo.Node;
      }
      if ( projectToAddTo == null )
      {
        return false;
      }

      if ( !SaveProject( projectToAddTo ) )
      {
        return false;
      }

      System.Windows.Forms.OpenFileDialog openDlg = new System.Windows.Forms.OpenFileDialog();
      openDlg.Title = "Open existing item";
      openDlg.Filter = FilterString( Types.Constants.FILEFILTER_ALL_SUPPORTED_FILES + Types.Constants.FILEFILTER_ASM + Types.Constants.FILEFILTER_CHARSET + Types.Constants.FILEFILTER_SPRITE + Types.Constants.FILEFILTER_BASIC + Types.Constants.FILEFILTER_BINARY_FILES + Types.Constants.FILEFILTER_ALL );
      openDlg.InitialDirectory = projectToAddTo.Settings.BasePath;
      openDlg.Multiselect = true;
      if ( openDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return false;
      }

      AddExistingFilesToProject( projectToAddTo, Node, openDlg.FileNames, false );
      return true;
    }



    public void AddExistingFilesToProject( Project ProjectToAddTo, DecentForms.TreeView.TreeNode Node, string[] Filenames, bool CopyToProjectFolderWithoutAsking )
    {
      foreach ( var fileName in Filenames )
      {
        AddExistingFileToProject( ProjectToAddTo, Node, fileName, CopyToProjectFolderWithoutAsking );
      }
    }



    public void AddExistingFileToProject( Project ProjectToAddTo, DecentForms.TreeView.TreeNode Node, string Filename, bool CopyToProjectFolderWithoutAsking )
    {
      string importFile = Filename;

      if ( ProjectToAddTo.IsFilenameInUse( importFile ) )
      {
        StudioCore.MessageBox( "File " + importFile + " is already part of this project", "File already added" );
        return;
      }

      // determine type by extension
      ProjectElement.ElementType type = ProjectElement.ElementType.ASM_SOURCE;
      string newFileExtension = System.IO.Path.GetExtension( GR.Path.RelativePathTo( importFile, false, System.IO.Path.GetFullPath( ProjectToAddTo.Settings.BasePath ), true ).ToUpper() );

      if ( newFileExtension == ".C64" )
      {
        type = ProjectElement.ElementType.PROJECT;

        var project = StudioCore.MainForm.OpenProject( importFile );
        return;
      }
      else if ( ( newFileExtension == ".CHARSETPROJECT" )
      ||        ( newFileExtension == ".CHR" ) )
      {
        type = ProjectElement.ElementType.CHARACTER_SET;
      }
      else if ( ( newFileExtension == ".SPRITEPROJECT" )
      ||        ( newFileExtension == ".SPR" ) )
      {
        type = ProjectElement.ElementType.SPRITE_SET;
      }
      else if ( newFileExtension == ".BIN" )
      {
        type = ProjectElement.ElementType.BINARY_FILE;
      }
      else if ( newFileExtension == ".CHARSCREEN" )
      {
        type = ProjectElement.ElementType.CHARACTER_SCREEN;
      }
      else if ( newFileExtension == ".GRAPHICSCREEN" )
      {
        type = ProjectElement.ElementType.GRAPHIC_SCREEN;
      }
      else if ( ( newFileExtension == ".BAS" )
      ||        ( newFileExtension == ".B" ) )
      {
        type = ProjectElement.ElementType.BASIC_SOURCE;
      }
      else if ( newFileExtension == ".MAPPROJECT" )
      {
        type = ProjectElement.ElementType.MAP_EDITOR;
      }

      if ( !GR.Path.IsSubPath( System.IO.Path.GetFullPath( ProjectToAddTo.Settings.BasePath ), importFile ) )
      {
        // not a sub folder
        DialogResult    doCopy = DialogResult.Cancel;

        if ( CopyToProjectFolderWithoutAsking )
        {
          doCopy = DialogResult.Yes;
        }
        else
        {
          doCopy = System.Windows.Forms.MessageBox.Show( "The item is not inside the current project folder. Should a copy be created in the project folder?",
                                                         "Create a local copy of the added file?",
                                                         MessageBoxButtons.YesNoCancel );
        }
        if ( doCopy == DialogResult.Cancel )
        {
          return;
        }
        if ( doCopy == DialogResult.Yes )
        {
          // create a copy
          string pureFileName = System.IO.Path.GetFileName( importFile );
          string newFileName = System.IO.Path.Combine( System.IO.Path.GetFullPath( ProjectToAddTo.Settings.BasePath ), pureFileName);

          while ( System.IO.File.Exists( newFileName ) )
          {
            newFileName = System.IO.Path.Combine( System.IO.Path.GetFullPath( ProjectToAddTo.Settings.BasePath ), System.IO.Path.GetFileNameWithoutExtension( newFileName ) + "_" + System.IO.Path.GetExtension( newFileName ) );
          }
          try
          {
            System.IO.File.Copy( importFile, newFileName, false );
          }
          catch ( System.Exception ex )
          {
            StudioCore.AddToOutput( "Could not copy file to new location: " + ex.Message );
            return;
          }
          importFile = newFileName;
        }
      }

      DecentForms.TreeView.TreeNode parentNodeToInsertTo = Node;

      ProjectElement element = ProjectToAddTo.CreateElement( type, parentNodeToInsertTo );

      string relativeFilename = GR.Path.RelativePathTo( System.IO.Path.GetFullPath( ProjectToAddTo.Settings.BasePath ), true, importFile, false );
      element.Name = System.IO.Path.GetFileNameWithoutExtension( relativeFilename );
      element.Filename = relativeFilename;

      while ( parentNodeToInsertTo.Level >= 1 )
      {
        element.ProjectHierarchy.Insert( 0, parentNodeToInsertTo.Text );
        parentNodeToInsertTo = parentNodeToInsertTo.Parent;
      }
      ProjectToAddTo.ShowDocument( element );
      element.DocumentInfo.DocumentFilename = relativeFilename;
      if ( element.Document != null )
      {
        element.Document.SetDocumentFilename( relativeFilename );
      }
      RaiseApplicationEvent( new ApplicationEvent( Types.ApplicationEvent.Type.ELEMENT_ADDED, element ) );
      ProjectToAddTo.SetModified();
    }



    private void importFileToolStripMenuItem_Click( object sender, EventArgs e )
    {
      ImportExistingFiles( null );
    }



    public void SetGUIForWaitOnExternalTool( bool Wait )
    {
      if ( InvokeRequired )
      {
        Invoke( new SetGUIForWaitOnExternalToolCallback( SetGUIForWaitOnExternalTool ), new object[] { Wait } );
      }
      else
      {
        // dockpanelsuite activates the first document, not the currently shown one if focus is set to it (by disabling the toolbar)
        BaseDocument    prevActiveDocument = ActiveDocument;

        mainToolCompile.Enabled         = !Wait;
        mainToolBuild.Enabled           = !Wait;
        mainToolRebuild.Enabled         = !Wait;
        mainToolBuildAndRun.Enabled     = !Wait;
        mainToolDebug.Enabled           = !Wait;
        mainToolConfig.Enabled          = !Wait;
        mainToolToggleTrueDrive.Enabled = !Wait;
        mainToolEmulator.Enabled        = !Wait;

        if ( Wait )
        {
          if ( ActiveDocument != prevActiveDocument )
          {
            ActiveDocument = prevActiveDocument;
          }
        }

        foreach ( ToolStripMenuItem subMenu in mainMenu.Items )
        {
          // leave Window and Help submenu intact!
          if ( ( subMenu.Text != "&Window" )
          &&   ( subMenu.Text != "&Help" )
          &&   ( subMenu.Text != "&Edit" ) )
          {
            subMenu.Enabled = !Wait;
          }
        }
      }
    }



    public void SetGUIForDebugging( bool DebugModeActive )
    {
      if ( InvokeRequired )
      {
        Invoke( new SetGUIForWaitOnExternalToolCallback( SetGUIForDebugging ), new object[] { DebugModeActive } );
      }
      else
      {
        try
        {
          debugTools.Enabled = DebugModeActive;
          debugTools.Left = mainTools.Right;
          debugTools.Top = mainTools.Top;

          menuWindowToolbarDebugger.Checked = debugTools.Enabled;
          if ( DebugModeActive )
          {
            SetToolPerspective( Perspective.DEBUG );
            mainDebugGo.Enabled = ( AppState != Types.StudioState.DEBUGGING_RUN );
            mainDebugBreak.Enabled = ( AppState == Types.StudioState.DEBUGGING_RUN );

            m_DebugRegisters.EnableRegisterOverrides( AppState == StudioState.DEBUGGING_BROKEN );
          }
          else
          {
            SetToolPerspective( Perspective.EDIT );
            mainDebugGo.Enabled = true;
            m_DebugRegisters.EnableRegisterOverrides( false );
          }
        }
        catch ( NullReferenceException )
        {
          // may happen during shutdown
        }
      }
    }



    private void debugConnectToolStripMenuItem_Click( object sender, EventArgs e )
    {
      if ( !StudioCore.Debugging.Debugger.ConnectToEmulator( Parser.ASMFileParser.IsCartridge( StudioCore.Debugging.DebugType ) ) )
      {
        Debug.Log( "Connect failed" );
      }
    }



    private void debugDisconnectToolStripMenuItem_Click( object sender, EventArgs e )
    {
      StudioCore.Debugging.Debugger.DisconnectFromEmulator();
    }



    private void SaveSettings()
    {
      StudioCore.Settings.MainWindowPlacement = GR.Forms.WindowStateManager.GeometryToString( this );

      m_FindReplace.ToSettings( StudioCore.Settings );

      GR.Memory.ByteBuffer SettingsData = StudioCore.Settings.ToBuffer( StudioCore );

      string settingFilename = SettingsPath();

      System.IO.Directory.CreateDirectory( System.IO.Directory.GetParent( settingFilename ).FullName );
      if ( !GR.IO.File.WriteAllBytes( settingFilename, SettingsData ) )
      {
        StudioCore.AddToOutput( "Failed to write settings to file" + System.Environment.NewLine );
      }

      CloseSolution();
    }



    private void DetermineSettingsPath()
    {
      if ( StudioCore.Settings.StudioAppMode == AppMode.UNDECIDED )
      {
        // decide by checking for existance of settings file
        var settingsPath = System.IO.Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ), "GR Games" );
        settingsPath = System.IO.Path.Combine( settingsPath, "C64Studio" );
        settingsPath = System.IO.Path.Combine( settingsPath, "1.0.0.0" );
        settingsPath = System.IO.Path.Combine( settingsPath, "settings.dat" );
        if ( System.IO.File.Exists( System.IO.Path.Combine( Application.StartupPath, "settings.dat" ) ) )
        {
          StudioCore.Settings.StudioAppMode = AppMode.PORTABLE_APP;
        }
        else if ( System.IO.File.Exists( settingsPath ) )
        {
          StudioCore.Settings.StudioAppMode = AppMode.GOOD_APP;
        }
      }
    }



    private string SettingsPath()
    {
      // prefix hard coded version number so we can use our proper version number
      // we do NOT use Application.UserAppDataPath as that creates a folder with the real version number which is not what we want
      // for backwards compatiblity reasons we're stuck with 1.0.0.0 here
      string    userAppDataPath = Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData );
      userAppDataPath = System.IO.Path.Combine( userAppDataPath, "GR Games" );
      userAppDataPath = System.IO.Path.Combine( userAppDataPath, "C64Studio" );
      userAppDataPath = System.IO.Path.Combine( userAppDataPath, "1.0.0.0" );

      try
      {
        if ( StudioCore.Settings.StudioAppMode == AppMode.PORTABLE_APP )
        {
          // return local path
          return System.IO.Path.Combine( Application.StartupPath, "settings.dat" );
        }
        // return clean user app data path
        return System.IO.Path.Combine( userAppDataPath, "settings.dat" );
      }
      catch ( Exception )
      {
        // fallback to clean path
        return System.IO.Path.Combine( userAppDataPath, "settings.dat" );
      }
    }



    private bool LoadSettings()
    {
      StudioCore.Settings.Core = StudioCore;


      string SettingFile = SettingsPath();

      GR.Memory.ByteBuffer SettingsData = null;
      try
      {
        SettingsData = new GR.Memory.ByteBuffer( System.IO.File.ReadAllBytes( SettingFile ) );
      }
      catch ( System.IO.FileNotFoundException )
      {
        return false;
      }
      catch ( System.IO.DirectoryNotFoundException )
      {
        return false;
      }

      if ( SettingsData.Empty() )
      {
        return false;
      }

      if ( !StudioCore.Settings.ReadFromBuffer( SettingsData ) )
      {
        return false;
      }

      RaiseApplicationEvent( new ApplicationEvent( Types.ApplicationEvent.Type.SETTINGS_LOADED ) );

      // TODO - additional memory views!
      m_DebugMemory.SetMemoryDisplayType();
      m_DebugMemory.ApplyHexViewColors();

      StudioCore.Settings.SanitizeSettings();

      for ( int i = 0; i < StudioCore.Settings.DebugMemoryViews.Count; ++i )
      {
        if ( i < StudioCore.Debugging.MemoryViews.Count )
        {
          StudioCore.Debugging.MemoryViews[i].RestoreViewFromSettings( StudioCore.Settings.DebugMemoryViews[i] );
        }
      }

      return true;
    }



    private void MainForm_FormClosed( object sender, FormClosedEventArgs e )
    {
      StudioCore.Debugging.Debugger?.Quit();
    }



    private void BuildAndDebug( DocumentInfo DocumentToBuild, DocumentInfo DocumentToDebug, DocumentInfo DocumentToRun )
    {
      if ( AppState != Types.StudioState.NORMAL )
      {
        return;
      }
      AppState = Types.StudioState.BUILD_AND_DEBUG;
      StudioCore.Debugging.OverrideDebugStart = -1;
      if ( !StartCompile( DocumentToBuild, DocumentToDebug, DocumentToRun, StudioCore.Navigating.Solution, false, false ) )
      {
        AppState = Types.StudioState.NORMAL;
      }
    }



    private void mainToolDebug_Click( object sender, EventArgs e )
    {
      ApplyFunction( RetroDevStudio.Types.Function.BUILD_AND_DEBUG );
    }



    public void RunToAddress( DocumentInfo DocumentToDebug, DocumentInfo DocumentToRun, int DebugAddress )
    {
      if ( AppState == Types.StudioState.NORMAL )
      {
        StartDebugAt( DocumentToDebug, DocumentToRun, DebugAddress );
      }
      else if ( AppState == Types.StudioState.DEBUGGING_BROKEN )
      {
        // unmark current marked line
        if ( StudioCore.Debugging.MarkedDocument != null )
        {
          StudioCore.Debugging.MarkLine( StudioCore.Debugging.MarkedDocument.DocumentInfo.Project, StudioCore.Debugging.MarkedDocument.DocumentInfo, -1 );
          StudioCore.Debugging.MarkedDocument = null;
        }

        StudioCore.Executing.BringToForeground();

        AppState = Types.StudioState.DEBUGGING_RUN;
        StudioCore.Debugging.FirstActionAfterBreak = false;

        StudioCore.MainForm.SetGUIForDebugging( true );
        //mainDebugGo.Enabled = false;
        //mainDebugBreak.Enabled = true;

        Types.Breakpoint tempBP = new RetroDevStudio.Types.Breakpoint();
        tempBP.Address = DebugAddress;
        tempBP.Temporary = true;
        Debug.Log( "Try to add Breakpoint at $" + DebugAddress.ToString( "X4" ) );
        StudioCore.Debugging.Debugger.AddBreakpoint( tempBP );
        StudioCore.Debugging.Debugger.Run();
      }
    }



    private void StartDebugAt( DocumentInfo DocumentToDebug, DocumentInfo DocumentToRun, int DebugAddress )
    {
      if ( AppState != Types.StudioState.NORMAL )
      {
        return;
      }

      if ( InvokeRequired )
      {
        Invoke( new StartDebugAtCallback( StartDebugAt ), new object[] { DocumentToDebug, DocumentToRun, DebugAddress } );
      }
      else
      {
        AppState = Types.StudioState.BUILD_AND_DEBUG;
        StudioCore.Debugging.OverrideDebugStart = DebugAddress;
        if ( !StartCompile( DocumentToRun, DocumentToDebug, DocumentToRun, StudioCore.Navigating.Solution, false, false ) )
        {
          AppState = Types.StudioState.NORMAL;
        }
      }
    }



    private void DebugGo()
    {
      if ( ( m_CurrentActiveTool != null )
      &&   ( !EmulatorInfo.SupportsDebugging( m_CurrentActiveTool ) ) )
      {
        return;
      }

      if ( AppState == Types.StudioState.DEBUGGING_BROKEN )
      {
        m_DebugMemory.InvalidateAllMemory();
        StudioCore.Debugging.Debugger.Run();

        if ( StudioCore.Debugging.MarkedDocument != null )
        {
          StudioCore.Debugging.MarkLine( StudioCore.Debugging.MarkedDocument.DocumentInfo.Project, StudioCore.Debugging.MarkedDocument.DocumentInfo, -1 );
          StudioCore.Debugging.MarkedDocument = null;
        }

        StudioCore.Executing.BringToForeground();
        m_DebugRegisters.EnableRegisterOverrides( false );

        AppState = Types.StudioState.DEBUGGING_RUN;
        StudioCore.Debugging.FirstActionAfterBreak = false;
        mainDebugGo.Enabled = false;
        mainDebugBreak.Enabled = true;
      }
    }



    private void mainDebugGo_Click( object sender, EventArgs e )
    {
      DebugGo();
    }



    private void mainDebugBreak_Click( object sender, EventArgs e )
    {
      StudioCore.Debugging.DebugBreak();
    }



    public void StopDebugging()
    {
      if ( InvokeRequired )
      {
        try
        {
          Invoke( new ParameterLessCallback( StopDebugging ) );
        }
        catch ( ObjectDisposedException )
        {
        }
      }
      else
      {
        try
        {
          if ( ( m_CurrentActiveTool != null )
          &&   ( !EmulatorInfo.SupportsDebugging( m_CurrentActiveTool ) ) )
          {
            if ( StudioCore.Executing.RunProcess != null )
            {
              try
              {
                StudioCore.Executing.RunProcess.Kill();
              }
              catch ( Exception )
              {
              }
              StudioCore.Executing.RunProcess = null;
            }

            AppState = Types.StudioState.NORMAL;
            StudioCore.SetStatus( "Ready", false, 0 );

            SetGUIForDebugging( false );
            SetGUIForWaitOnExternalTool( false );
            StudioCore.Executing.BringStudioToForeground();
            return;
          }

          if ( StudioCore.Executing.RunProcess != null )
          {
            StudioCore.Executing.RunProcess.Exited -= runProcess_Exited;
            try
            {
              StudioCore.Executing.RunProcess.Kill();
            }
            catch ( Exception )
            {
            }
            StudioCore.Executing.RunProcess = null;
          }

          if ( StudioCore.Debugging.TempDebuggerStartupFilename.Length > 0 )
          {
            try
            {
              System.IO.File.Delete( StudioCore.Debugging.TempDebuggerStartupFilename );
            }
            catch ( Exception ex )
            {
              StudioCore.AddToOutput( "Failed to delete temporary file " + StudioCore.Debugging.TempDebuggerStartupFilename + ", " + ex.Message + Environment.NewLine );
            }
            StudioCore.Debugging.TempDebuggerStartupFilename = "";
          }
          if ( StudioCore.Debugging.TempDebuggerStartupFilename.Length > 0 )
          {
            try
            {
              System.IO.File.Delete( StudioCore.Debugging.TempDebuggerBreakpointFilename );
            }
            catch ( Exception ex )
            {
              StudioCore.AddToOutput( "Failed to delete temporary file " + StudioCore.Debugging.TempDebuggerBreakpointFilename + ", " + ex.Message + Environment.NewLine );
            }
            StudioCore.Debugging.TempDebuggerBreakpointFilename = "";
          }

          if ( ( AppState == Types.StudioState.DEBUGGING_BROKEN )
          ||   ( AppState == Types.StudioState.DEBUGGING_RUN ) )
          {
            // send any command to break into the monitor again
            StudioCore.Debugging.Debugger?.Quit();
            StudioCore.Debugging.Debugger?.DisconnectFromEmulator();

            if ( StudioCore.Debugging.MarkedDocument != null )
            {
              StudioCore.Debugging.MarkLine( StudioCore.Debugging.MarkedDocument.DocumentInfo.Project, StudioCore.Debugging.MarkedDocument.DocumentInfo, -1 );
              StudioCore.Debugging.MarkedDocument = null;
            }
            if ( StudioCore.Debugging.DebugDisassembly != null )
            {
              StudioCore.AddToOutput( "Closing Disassembly window" );
              StudioCore.Debugging.DebugDisassembly.Close();
              StudioCore.Debugging.DebugDisassembly = null;
            }
            StudioCore.Debugging.CurrentCodePosition = -1;

            StudioCore.Debugging.DebuggedProject = null;
            StudioCore.Debugging.Debugger = null;
            StudioCore.Debugging.FirstActionAfterBreak = false;
            mainDebugGo.Enabled = false;
            mainDebugBreak.Enabled = false;
            m_CurrentActiveTool = null;

            StudioCore.Debugging.RemoveVirtualBreakpoints();
          }
          AppState = Types.StudioState.NORMAL;
          StudioCore.SetStatus( "Ready", false, 0 );

          SetGUIForDebugging( false );
          SetGUIForWaitOnExternalTool( false );
          StudioCore.Executing.BringStudioToForeground();
        }
        catch ( System.Exception ex )
        {
          Debug.Log( ex.ToString() );
        }
      }
    }



    private void mainDebugStop_Click( object sender, EventArgs e )
    {
      ApplyFunction( RetroDevStudio.Types.Function.DEBUG_STOP );
    }



    private void mainDebugStepInto_Click( object sender, EventArgs e )
    {
      StudioCore.Debugging.DebugStep();
    }



    private void mainDebugStepOver_Click( object sender, EventArgs e )
    {
      StudioCore.Debugging.StepOver();
    }



    private void mainDebugStepOut_Click( object sender, EventArgs e )
    {
      StudioCore.Debugging.DebugStepOut();
    }



    private void refreshRegistersToolStripMenuItem_Click( object sender, EventArgs e )
    {
      if ( ( AppState == Types.StudioState.DEBUGGING_BROKEN )
      ||   ( AppState == Types.StudioState.DEBUGGING_RUN ) )
      {
        StudioCore.Debugging.Debugger.RefreshRegistersAndWatches();
        if ( AppState == Types.StudioState.DEBUGGING_RUN )
        {
          StudioCore.Debugging.FirstActionAfterBreak = true;
        }
        StudioCore.Executing.BringStudioToForeground();
        AppState = Types.StudioState.DEBUGGING_BROKEN;
      }
    }



    private GR.Collections.Set<DocumentInfo> FindDocumentsInDependencyChain( DocumentInfo Doc )
    {
      GR.Collections.Set<DocumentInfo> tempSet = new GR.Collections.Set<DocumentInfo>();

      if ( Doc.Element == null )
      {
        return tempSet;
      }


      tempSet.Add( Doc );
      bool foundElement = true;
      bool addedMainElement = false;
      while ( foundElement )
      {
        foundElement = false;

        retry:
        foreach ( DocumentInfo doc in tempSet )
        {
          if ( ( tempSet.ContainsValue( doc ) )
          &&   ( ( doc != Doc )
          ||     ( addedMainElement ) ) )
          {
            continue;
          }
          if ( doc == Doc )
          {
            addedMainElement = true;
          }
          lock ( doc.DeducedDependency )
          {
            foreach ( var deducedDependency in doc.DeducedDependency[doc.Project.Settings.CurrentConfig.Name].BuildState.Keys )
            {
              var element = doc.Project.GetElementByFilename( deducedDependency );
              if ( element != null )
              {
                if ( !tempSet.ContainsValue( element.DocumentInfo ) )
                {
                  tempSet.Add( element.DocumentInfo );
                  foundElement = true;
                }
              }
            }
          }

          if ( doc.Element != null )
          {
            foreach ( var file in doc.Element.ForcedDependency.DependentOnFile )
            {
              var element = doc.Project.GetElementByFilename(file.Filename);
              if ( element != null )
              {
                if ( !tempSet.ContainsValue( element.DocumentInfo ) )
                {
                  tempSet.Add( element.DocumentInfo );
                  foundElement = true;
                }
              }
            }
          }
          if ( foundElement )
          {
            goto retry;
          }
        }
      }
      return tempSet;
    }



    private bool FindAndOpenBestMatchForLocation( int CurrentPos )
    {
      // TODO - check active file first, then active project, then any
      string documentFile = "";
      int documentLine = 0;

      DocumentInfo currentMarkedFile = null;
      if ( StudioCore.Debugging.MarkedDocument != null )
      {
        currentMarkedFile = StudioCore.Debugging.MarkedDocument.DocumentInfo;
      }

      DocumentInfo activeFile = ActiveDocumentInfo;

      List<DocumentInfo> foundMatches = new List<DocumentInfo>();


      // find any match
      GR.Collections.Set<DocumentInfo> dependentDocuments = FindDocumentsInDependencyChain(StudioCore.Debugging.DebugBaseDocumentRun);
      foreach ( DocumentInfo doc in dependentDocuments )
      {
        if ( doc.Type == ProjectElement.ElementType.ASM_SOURCE )
        {
          if ( doc.ASMFileInfo.DocumentAndLineFromAddress( CurrentPos, out documentFile, out documentLine ) )
          {
            if ( ( StudioCore.Debugging.MarkedDocument == null )
            || ( !GR.Path.IsPathEqual( StudioCore.Debugging.MarkedDocument.DocumentInfo.FullPath, documentFile ) )
            || ( StudioCore.Debugging.MarkedDocumentLine != documentLine ) )
            {
              foundMatches.Add( doc );
            }
          }
        }
      }

      /*
      Debug.Log( "Found " + foundMatches.Count + " potential matches" );
      Debug.Log( "ActiveFile = " + ( ( activeFile != null ) ? activeFile.FullPath : "null" ) );
      Debug.Log( "currentMarkedFile = " + ( ( currentMarkedFile != null ) ? currentMarkedFile.FullPath : "null" ) );
      foreach ( var docInfo in foundMatches )
      {
        Debug.Log( "-candidate " + docInfo.FullPath );
      }*/

      if ( activeFile != null )
      {
        //Debug.Log( "Try with activefile first" );
        if ( activeFile.ASMFileInfo.DocumentAndLineFromAddress( CurrentPos, out documentFile, out documentLine ) )
        {
          var docInfo = StudioCore.Navigating.FindDocumentInfoByPath( documentFile );
          StudioCore.Navigating.OpenDocumentAndGotoLine( StudioCore.Debugging.DebuggedProject, docInfo, documentLine );
          StudioCore.Debugging.MarkLine( StudioCore.Debugging.DebuggedProject, docInfo, documentLine );
          return true;
        }
      }
      if ( ( currentMarkedFile != null )
      && ( currentMarkedFile != activeFile ) )
      {
        //Debug.Log( "Try with activefile first" );
        if ( currentMarkedFile.ASMFileInfo.DocumentAndLineFromAddress( CurrentPos, out documentFile, out documentLine ) )
        {
          var docInfo = StudioCore.Navigating.FindDocumentInfoByPath( documentFile );
          StudioCore.Navigating.OpenDocumentAndGotoLine( StudioCore.Debugging.DebuggedProject, docInfo, documentLine );
          StudioCore.Debugging.MarkLine( StudioCore.Debugging.DebuggedProject, docInfo, documentLine );
          return true;
        }
      }

      // if any left use the first one
      if ( ( foundMatches.Count > 0 )
      && ( foundMatches[0].ASMFileInfo.DocumentAndLineFromAddress( CurrentPos, out documentFile, out documentLine ) ) )
      {
        //Debug.Log( "use first of left overs: " + foundMatches[0].FullPath );
        var docInfo = StudioCore.Navigating.FindDocumentInfoByPath( documentFile );
        StudioCore.Navigating.OpenDocumentAndGotoLine( StudioCore.Debugging.DebuggedProject, docInfo, documentLine );
        StudioCore.Debugging.MarkLine( StudioCore.Debugging.DebuggedProject, docInfo, documentLine );
        return true;
      }
      return false;
    }



    public void SetDebuggerValues( RegisterInfo Registers )
    {
      if ( InvokeRequired )
      {
        Invoke( new SetDebuggerValuesCallback( SetDebuggerValues ), new object[] { Registers } );
      }
      else
      {
        try
        {
          // ReadRegisters:(C:$0810)   ADDR AC XR YR SP 00 01 NV-BDIZC LIN CYC
          if ( AppState == Types.StudioState.DEBUGGING_RUN )
          {
            StudioCore.Debugging.FirstActionAfterBreak = true;
          }
          StudioCore.Executing.BringStudioToForeground();
          AppState = Types.StudioState.DEBUGGING_BROKEN;

          // only update if we're not during closing of debugger
          if ( ( StudioCore.Debugging.Debugger != null )
          &&   ( !StudioCore.Debugging.Debugger.ShuttingDown ) )
          {
            m_DebugRegisters.SetRegisters( Registers );
            m_DebugRegisters.EnableRegisterOverrides( true );
            int currentPos = Registers.PC;
            string documentFile = "";
            int documentLine = -1;

            //currentPos = 0x918;   // $918 -  $2cbb
            //currentPos = 0x2430;  // für fehler in Downhill innerhalb speedscroll nested loop
            if ( StudioCore.Debugging.DebuggedASMBase.ASMFileInfo.DocumentAndLineFromAddress( currentPos, out documentFile, out documentLine ) )
            {
              if ( ( StudioCore.Debugging.MarkedDocument == null )
              ||   ( !GR.Path.IsPathEqual( StudioCore.Debugging.MarkedDocument.DocumentInfo.FullPath, documentFile ) )
              ||   ( StudioCore.Debugging.MarkedDocumentLine != documentLine ) )
              {
                var docInfo = StudioCore.Navigating.FindDocumentInfoByPath( documentFile );
                StudioCore.Navigating.OpenDocumentAndGotoLine( StudioCore.Debugging.DebuggedProject, docInfo, documentLine );
                StudioCore.Debugging.MarkLine( StudioCore.Debugging.DebuggedProject, docInfo, documentLine );

                // hide disassembly window
                if ( StudioCore.Debugging.DebugDisassembly != null )
                {
                  StudioCore.Debugging.DebugDisassembly.Close();
                  StudioCore.Debugging.DebugDisassembly = null;
                }
              }
            }
            else
            {
              // try to find info in file in dependency chain
              if ( !FindAndOpenBestMatchForLocation( currentPos ) )
              {
                StudioCore.AddToOutput( "Failed to match address $" + currentPos.ToString( "X4" ) + " to code, showing disassembly" + System.Environment.NewLine );
                ShowDisassemblyAt( currentPos );
              }
            }
          }
          mainDebugGo.Enabled = true;
          mainDebugBreak.Enabled = false;
        }
        catch ( System.Exception ex )
        {
          Debug.Log( ex.ToString() );
        }
      }
    }



    private void ShowDisassemblyAt( int Address )
    {
      StudioCore.Debugging.CurrentCodePosition = Address;
      if ( StudioCore.Debugging.DebugDisassembly == null )
      {
        StudioCore.Debugging.DebugDisassembly = new Disassembly( StudioCore );
        StudioCore.Debugging.DebugDisassembly.RefreshDisplayOptions();

        StudioCore.Debugging.DebugDisassembly.Name = "Disassembly";
        StudioCore.Debugging.DebugDisassembly.SetDocumentFilename( "Disassembly" );
        StudioCore.Debugging.DebugDisassembly.Show( panelMain );
      }

      // put disassembly in there
      StudioCore.Debugging.DebugDisassembly.SetText( "Disassembly will\r\nappear here" );

      StudioCore.Debugging.DebugDisassembly.SetCursorToLine( 1, 0, true );

      if ( ( StudioCore.Debugging.MarkedDocument == null )
      ||   ( !GR.Path.IsPathEqual( StudioCore.Debugging.MarkedDocument.DocumentInfo.FullPath, "C64Studio-intermediatedisassembly" ) )
      ||   ( StudioCore.Debugging.MarkedDocumentLine != 1 ) )
      {
        if ( StudioCore.Debugging.MarkedDocument != null )
        {
          StudioCore.Debugging.MarkedDocument.SetLineMarked( StudioCore.Debugging.MarkedDocumentLine, false );
        }

        StudioCore.Debugging.MarkedDocument = StudioCore.Debugging.DebugDisassembly;
        StudioCore.Debugging.MarkedDocumentLine = 1;
        StudioCore.Debugging.DebugDisassembly.Select();
        StudioCore.Debugging.DebugDisassembly.SetLineMarked( 1, true );
        StudioCore.Debugging.DebugDisassembly.SetAddress( Address );
      }
      StudioCore.Debugging.Debugger.RefreshMemory( Address, 32, MemorySource.AS_CPU );
    }



    private void selfParseToolStripMenuItem_Click( object sender, EventArgs e )
    {
      DocumentInfo doc = ActiveDocumentInfo;
      if ( doc == null )
      {
        return;
      }
      if ( doc.Type != ProjectElement.ElementType.ASM_SOURCE )
      {
        return;
      }
      EnsureFileIsParsed();

      StudioCore.Compiling.ParserASM.DumpLabels();
    }



    private void showLineinfosToolStripMenuItem_Click( object sender, EventArgs e )
    {
      DocumentInfo doc = ActiveDocumentInfo;
      if ( doc == null )
      {
        return;
      }
      if ( doc.Type != ProjectElement.ElementType.ASM_SOURCE )
      {
        return;
      }
      EnsureFileIsParsed();
      foreach ( int address in doc.ASMFileInfo.AddressToLine.Keys )
      {
        Debug.Log( "Line " + doc.ASMFileInfo.AddressToLine[address].ToString() + ": " + address + ", " + doc.ASMFileInfo.LineInfo[doc.ASMFileInfo.AddressToLine[address]].Line );
      }
      foreach ( Types.ASM.SourceInfo sourceInfo in doc.ASMFileInfo.SourceInfo.Values )
      {
        Debug.Log( "Source " + sourceInfo.Filename + " in " + sourceInfo.FilenameParent + " from line " + sourceInfo.GlobalStartLine + " to " + ( sourceInfo.GlobalStartLine + sourceInfo.LineCount - 1 ) + " orig at " + sourceInfo.LocalStartLine + " to " + ( sourceInfo.LocalStartLine + sourceInfo.LineCount - 1 ) );
      }
    }



    private bool IsDocPartOfMainDocument( DocumentInfo Doc )
    {
      if ( Doc.Project == null )
      {
        return false;
      }
      ProjectElement element = Doc.Project.GetElementByFilename(Doc.Project.Settings.MainDocument);
      if ( ( element != null )
      && ( element.Document != null ) )
      {
        if ( ( Doc != null )
        && ( element.DocumentInfo.Type == ProjectElement.ElementType.ASM_SOURCE ) )
        {
          if ( ( element.DocumentInfo.ASMFileInfo.LineInfo.Count != 0 )
          && ( !element.DocumentInfo.ASMFileInfo.IsDocumentPart( Doc.FullPath ) ) )
          {
            return false;
          }
        }
        return true;
      }
      return false;
    }



    public DocumentInfo DetermineDocumentByFileName( string Filename )
    {
      foreach ( var docInfo in DocumentInfos )
      {
        if ( ( !string.IsNullOrEmpty( docInfo.FullPath ) )
        && ( GR.Path.IsPathEqual( docInfo.FullPath, Filename ) ) )
        {
          return docInfo;
        }
      }
      return null;
    }



    public DocumentInfo DetermineDocument()
    {
      BaseDocument baseDocToCompile = ActiveContent;
      if ( ( baseDocToCompile != null )
      && ( !baseDocToCompile.DocumentInfo.Compilable ) )
      {
        baseDocToCompile = ActiveDocument;
      }
      if ( baseDocToCompile == null )
      {
        return null;
      }
      return baseDocToCompile.DocumentInfo;
    }



    public DocumentInfo DetermineDocumentToCompile( bool AllowMainProjectOverride )
    {
      BaseDocument baseDocToCompile = ActiveContent;

      if ( ( AllowMainProjectOverride )
      &&   ( StudioCore.Navigating.Solution != null )
      &&   ( StudioCore.Navigating.Solution.ActiveProject != "" ) )
      {
        var project = StudioCore.Navigating.Solution.GetProjectByName( StudioCore.Navigating.Solution.ActiveProject );
        if ( project != null )
        {
          ProjectElement element = project.GetElementByFilename( project.Settings.MainDocument );
          if ( element != null )
          {
            baseDocToCompile = element.Document;
          }
        }
      }

      if ( ( ( baseDocToCompile != null )
      &&     ( !baseDocToCompile.DocumentInfo.Compilable ) )
      ||   ( baseDocToCompile == null ) )
      {
        baseDocToCompile = ActiveDocument;
      }
      if ( baseDocToCompile == null )
      {
        return null;
      }

      DocumentInfo docToCompile = baseDocToCompile.DocumentInfo;


      if ( ( docToCompile.Element != null )
      &&   ( !string.IsNullOrEmpty( docToCompile.Project.Settings.MainDocument ) ) )
      {
        ProjectElement element = docToCompile.Project.GetElementByFilename( docToCompile.Project.Settings.MainDocument );
        if ( element != null )
        //&&   ( element.Document != null ) )
        {
          if ( ( docToCompile != null )
          &&   ( ( element.DocumentInfo.Type == ProjectElement.ElementType.ASM_SOURCE )
          ||     ( element.DocumentInfo.Type == ProjectElement.ElementType.BASIC_SOURCE ) ) )
          {
            if ( ( element.DocumentInfo.ASMFileInfo != null )
            &&   ( element.DocumentInfo.ASMFileInfo.LineInfo.Count != 0 )
            &&   ( docToCompile.Compilable )
            &&   ( !element.DocumentInfo.ASMFileInfo.IsDocumentPart( docToCompile.FullPath ) )
            &&   ( !( element.IsDependentOn( docToCompile.FullPath ) ) ) )
            {
              return docToCompile;
            }
            return element.DocumentInfo;
          }
        }
      }

      // are we included in a different file and we know it?
      if ( ( docToCompile == null )
      || ( !docToCompile.Compilable ) )
      {
        if ( baseDocToCompile != null )
        {
          return baseDocToCompile.DocumentInfo;
        }
        return null;
      }
      return docToCompile;
    }



    private DocumentInfo DetermineDocumentToHandle()
    {
      BaseDocument baseDocToCompile = ActiveContent;
      if ( baseDocToCompile == null )
      {
        baseDocToCompile = ActiveDocument;
      }
      if ( ( baseDocToCompile != null )
      &&   ( !baseDocToCompile.DocumentInfo.Compilable ) )
      {
        baseDocToCompile = ActiveDocument;
      }
      if ( baseDocToCompile == null )
      {
        return null;
      }

      DocumentInfo docToCompile = baseDocToCompile.DocumentInfo;

      // if there is a main document AND we are part of it's compile chain
      if ( ( docToCompile.Element != null )
      &&   ( !string.IsNullOrEmpty( docToCompile.Project.Settings.MainDocument ) ) )
      {
        ProjectElement element = docToCompile.Project.GetElementByFilename(docToCompile.Project.Settings.MainDocument);
        if ( ( element != null )
        &&   ( element.Document != null ) )
        {
          if ( docToCompile != null )
          {
            if ( ( docToCompile.Compilable )
            &&   ( !element.DocumentInfo.ASMFileInfo.IsDocumentPart( docToCompile.FullPath ) )
            &&   ( !element.IsDependentOn( docToCompile.FullPath ) ) )
            {
              return docToCompile;
            }
          }
          docToCompile = element.DocumentInfo;
        }
      }

      if ( ( docToCompile == null )
      ||   ( !docToCompile.Compilable ) )
      {
        return null;
      }
      return docToCompile;
    }



    public void CallHelp( string Keyword, DocumentInfo Doc )
    {
      m_ChangingToolWindows = true;
      if ( string.IsNullOrEmpty( m_Help.Text ) )
      {
        m_Help.Text = "Help";
      }
      StudioCore.Settings.Tools[ToolWindowType.HELP].Document.Show( panelMain );
      StudioCore.Settings.Tools[ToolWindowType.HELP].MenuItem.Checked = true;
      StudioCore.Settings.Tools[ToolWindowType.HELP].Visible[m_ActivePerspective] = true;
      m_ChangingToolWindows = false;

      if ( !string.IsNullOrEmpty( Keyword ) )
      {
        if ( Doc != null )
        {
          if ( Doc.ASMFileInfo.Processor.Opcodes.ContainsKey( Keyword.ToLower() ) )
          {
            m_Help.NavigateTo( "aay64h64/AAY64/B" + Keyword.ToUpper() + ".HTM" );
            return;
          }
          else if ( ( Doc.ASMFileInfo.AssemblerSettings != null )
          &&        ( Doc.ASMFileInfo.AssemblerSettings.PseudoOps.ContainsKey( Keyword.ToUpper() ) ) )
          {
            m_Help.NavigateTo( "asm_macro.html#" + Keyword.Substring( 1 ).ToLower() );
            return;
          }
        }
        if ( StudioCore.Compiling.ParserASM.m_Processor.Opcodes.ContainsKey( Keyword.ToLower() ) )
        {
          m_Help.NavigateTo( "aay64h64/AAY64/B" + Keyword.ToUpper() + ".HTM" );
        }
        else if ( StudioCore.Compiling.ASMFileInfo.AssemblerSettings.PseudoOps.ContainsKey( Keyword.ToUpper() ) )
        {
          m_Help.NavigateTo( "asm_macro.html#" + Keyword.Substring( 1 ).ToLower() );
        }
      }
    }



    public bool ApplyFunction( Types.Function Function )
    {
      switch ( Function )
      {
        case Function.BOOKMARK_DELETE_ALL:
          if ( ActiveContent != null )
          {
            return ActiveContent.ApplyFunction( Function );
          }
          return false;
        case Types.Function.FIND_NEXT_MESSAGE:
          StudioCore.Navigating.OpenSourceOfNextMessage();
          return true;
        case RetroDevStudio.Types.Function.OPEN_FILES:
          {
            System.Windows.Forms.OpenFileDialog openDlg = new System.Windows.Forms.OpenFileDialog();
            openDlg.Title = "Open existing item";
            openDlg.Filter = FilterString( Types.Constants.FILEFILTER_ALL_SUPPORTED_FILES + Types.Constants.FILEFILTER_ASM + Types.Constants.FILEFILTER_CHARSET + Types.Constants.FILEFILTER_SPRITE + Types.Constants.FILEFILTER_BASIC + Types.Constants.FILEFILTER_BINARY_FILES +Types.Constants.FILEFILTER_DISASSEMBLY + Types.Constants.FILEFILTER_ALL );

            if ( m_CurrentProject != null )
            {
              openDlg.InitialDirectory = m_CurrentProject.Settings.BasePath;
            }
            openDlg.Multiselect = true;
            if ( openDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
            {
              return true;
            }

            foreach ( var fileName in openDlg.FileNames )
            {
              OpenFile( fileName );
            }
          }
          return true;
        case RetroDevStudio.Types.Function.TOGGLE_BREAKPOINT:
          if ( ( AppState != Types.StudioState.NORMAL )
          &&   ( AppState != RetroDevStudio.Types.StudioState.DEBUGGING_BROKEN ) )
          {
            break;
          }
          if ( ActiveDocument is SourceASMEx )
          {
            SourceASMEx asm = (SourceASMEx)ActiveDocument;

            asm.ToggleBreakpoint( asm.CurrentLineIndex );
            return true;
          }
          break;
        case RetroDevStudio.Types.Function.HELP:
          {
            string keywordBelow = null;
            if ( ( ActiveContent != null )
            &&   ( ActiveContent is SourceASMEx ) )
            {
              SourceASMEx asm = ActiveContent as SourceASMEx;

              if ( asm.editSource.SelectionLength > 0 )
              {
                keywordBelow = asm.editSource.Selection.Text;
              }
              else
              {
                keywordBelow = asm.FindWordAtCaretPosition();
              }
            }
            CallHelp( keywordBelow, ActiveDocumentInfo );
          }
          return true;
        case RetroDevStudio.Types.Function.FIND_NEXT:
          m_FindReplace.FindNext( ActiveDocument );
          return true;
        case RetroDevStudio.Types.Function.FIND:
          if ( ActiveDocumentInfo != null )
          {
            var compilableDoc = ActiveDocumentInfo.CompilableDocument;
            if ( compilableDoc != null )
            {
              m_FindReplace.AdjustSettings( compilableDoc.SourceControl );
            }
          }
          if ( !m_FindReplace.Visible )
          {
            m_FindReplace.Show( panelMain );
          }
          m_FindReplace.comboSearchText.Focus();
          StudioCore.Settings.Tools[ToolWindowType.FIND_REPLACE].Visible[m_ActivePerspective] = true;

          m_FindReplace.tabFindReplace.SelectedIndex = 0;
          m_FindReplace.comboSearchTarget.SelectedIndex = 1;
          m_FindReplace.AcceptButton = m_FindReplace.btnFindNext;
          return true;
        case RetroDevStudio.Types.Function.FIND_IN_PROJECT:
          {
            if ( ActiveDocumentInfo != null )
            {
              var compilableDoc = ActiveDocumentInfo.CompilableDocument;
              if ( compilableDoc != null )
              {
                m_FindReplace.AdjustSettings( compilableDoc.SourceControl );
              }
            }
          }
          if ( !m_FindReplace.Visible )
          {
            m_FindReplace.Show( panelMain );
          }
          m_FindReplace.comboSearchText.Focus();
          StudioCore.Settings.Tools[ToolWindowType.FIND_REPLACE].Visible[m_ActivePerspective] = true;
          m_FindReplace.tabFindReplace.SelectedIndex = 0;
          // whole solution per default
          m_FindReplace.comboSearchTarget.SelectedIndex = 4;
          m_FindReplace.AcceptButton = m_FindReplace.btnFindAll;
          return true;
        case RetroDevStudio.Types.Function.FIND_REPLACE:
          if ( ActiveDocumentInfo != null )
          {
            var compilableDoc = ActiveDocumentInfo.CompilableDocument;
            if ( compilableDoc != null )
            {
              m_FindReplace.AdjustSettings( compilableDoc.SourceControl );
            }
          }
          if ( !m_FindReplace.Visible )
          {
            m_FindReplace.Show( panelMain );
          }
          m_FindReplace.comboReplaceSearchText.Focus();
          StudioCore.Settings.Tools[ToolWindowType.FIND_REPLACE].Visible[m_ActivePerspective] = true;
          m_FindReplace.tabFindReplace.SelectedIndex = 1;
          return true;
        case RetroDevStudio.Types.Function.REPLACE_IN_PROJECT:
          {
            var compilableDoc = ActiveDocumentInfo.CompilableDocument;
            if ( compilableDoc != null )
            {
              m_FindReplace.AdjustSettings( compilableDoc.SourceControl );
            }
          }
          if ( !m_FindReplace.Visible )
          {
            m_FindReplace.Show( panelMain );
          }
          m_FindReplace.comboReplaceSearchText.Focus();
          StudioCore.Settings.Tools[ToolWindowType.FIND_REPLACE].Visible[m_ActivePerspective] = true;
          m_FindReplace.tabFindReplace.SelectedIndex = 1;
          m_FindReplace.comboReplaceTarget.SelectedIndex = 3;
          return true;
        case Function.PRINT:
        case Function.COMMENT_SELECTION:
        case Function.UNCOMMENT_SELECTION:
        case Function.COLLAPSE_ALL_FOLDING_BLOCKS:
        case Function.EXPAND_ALL_FOLDING_BLOCKS:
        case Function.JUMP_TO_LINE:
          // pass through to document
          {
            var curDoc = ActiveDocumentInfo;
            if ( ( curDoc != null )
            &&   ( curDoc.BaseDoc != null )
            &&   ( curDoc.ContainsCode ) )
            {
              return curDoc.BaseDoc.ApplyFunction( Function );
            }
          }
          break;
        case Function.GRAPHIC_ELEMENT_MIRROR_H:
        case Function.GRAPHIC_ELEMENT_MIRROR_V:
        case Function.GRAPHIC_ELEMENT_SHIFT_D:
        case Function.GRAPHIC_ELEMENT_SHIFT_U:
        case Function.GRAPHIC_ELEMENT_SHIFT_L:
        case Function.GRAPHIC_ELEMENT_SHIFT_R:
        case Function.GRAPHIC_ELEMENT_ROTATE_L:
        case Function.GRAPHIC_ELEMENT_ROTATE_R:
        case Function.GRAPHIC_ELEMENT_NEXT:
        case Function.GRAPHIC_ELEMENT_PREVIOUS:
        case Function.GRAPHIC_ELEMENT_MULTI_COLOR_1:
        case Function.GRAPHIC_ELEMENT_MULTI_COLOR_2:
        case Function.GRAPHIC_ELEMENT_CUSTOM_COLOR:
        case Function.GRAPHIC_ELEMENT_BACKGROUND_COLOR:
        case Function.GRAPHIC_ELEMENT_INVERT:
          // pass through to document
          {
            var curDoc = ActiveDocumentInfo;
            if ( ( curDoc != null )
            &&   ( curDoc.BaseDoc != null ) )
            {
              //Debug.Log( "ActiveCOntrol " + ActiveControl );
              return curDoc.BaseDoc.ApplyFunction( Function );
            }
          }
          break;
        case RetroDevStudio.Types.Function.CENTER_ON_CURSOR:
          {
            // save current document
            BaseDocument curDoc = ActiveContent;
            if ( ( curDoc != null )
            &&   ( !curDoc.DocumentInfo.ContainsCode ) )
            {
              curDoc = ActiveDocument;
            }
            if ( curDoc != null )
            {
              var compilableDoc = curDoc.DocumentInfo.CompilableDocument;
              compilableDoc?.CenterOnCaret();
              return true;
            }
          }
          break;
        case RetroDevStudio.Types.Function.DEBUG_STEP:
          StudioCore.Debugging.DebugStepInto();
          return true;
        case RetroDevStudio.Types.Function.DEBUG_STEP_OVER:
          StudioCore.Debugging.DebugStepOver();
          return true;
        case Function.DEBUG_STEP_OUT:
          StudioCore.Debugging.DebugStepOut();
          return true;
        case RetroDevStudio.Types.Function.DEBUG_STOP:
          StopDebugging();
          return true;
        case RetroDevStudio.Types.Function.DEBUG_GO:
          DebugGo();
          return true;
        case RetroDevStudio.Types.Function.DEBUG_BREAK:
          StudioCore.Debugging.DebugBreak();
          return true;
        case RetroDevStudio.Types.Function.DEBUG_RUN_TO:
          if ( ( AppState != Types.StudioState.NORMAL )
          &&   ( AppState != RetroDevStudio.Types.StudioState.DEBUGGING_BROKEN ) )
          {
            break;
          }
          {
            DocumentInfo docToDebug = DetermineDocumentToHandle();
            DocumentInfo docToHandle = DetermineDocumentToCompile( false );
            DocumentInfo docActive = DetermineDocument();

            if ( ( docToDebug == null )
            ||   ( docActive == null )
            ||   ( ( docToDebug.Type != ProjectElement.ElementType.ASM_SOURCE )
            &&     ( docActive.Type != ProjectElement.ElementType.ASM_SOURCE ) ) )
            {
              break;
            }

            // this potentially starts a task
            EnsureFileIsParsed( docToHandle );

            // so this should become one too!
            StudioCore.TaskManager.AddTask( new Tasks.TaskDebugRunTo( docToHandle, docToDebug, docActive ) );
          }
          return true;
        case RetroDevStudio.Types.Function.SAVE_ALL:
          SaveSolution();
          if ( StudioCore.Navigating.Solution != null )
          {
            foreach ( Project project in StudioCore.Navigating.Solution.Projects )
            {
              SaveProject( project );
            }
            foreach ( Project project in StudioCore.Navigating.Solution.Projects )
            {
              foreach ( ProjectElement element in project.Elements )
              {
                if ( element.Document != null )
                {
                  element.Document.Save( BaseDocument.SaveMethod.SAVE );
                }
              }
            }
            // elements saving could have changed project settings, so save again
            foreach ( Project project in StudioCore.Navigating.Solution.Projects )
            {
              SaveProject( project );
            }
            // save all changed non project files!
            foreach ( BaseDocument doc in panelMain.Documents )
            {
              if ( ( doc.DocumentInfo.Element == null )
              &&   ( doc.Modified ) )
              {
                doc.Save( BaseDocument.SaveMethod.SAVE );
              }
            }
          }
          else
          {
            BaseDocument docToSave = ActiveContent;
            if ( docToSave != null )
            {
              docToSave.Save( BaseDocument.SaveMethod.SAVE );
            }
          }
          return true;
        case RetroDevStudio.Types.Function.SAVE_DOCUMENT:
          {
            // save current document
            BaseDocument docToSave = ActiveContent;
            if ( ( docToSave != null )
            &&   ( !docToSave.IsSaveable ) )
            {
              docToSave = ActiveDocument;
            }
            if ( ( docToSave == null )
            ||   ( !docToSave.IsSaveable ) )
            {
              break;
            }

            if ( docToSave.DocumentInfo.Project == null )
            {
              docToSave.Save( BaseDocument.SaveMethod.SAVE );
              return true;
            }

            if ( ( docToSave.DocumentInfo.Project == null )
            ||   ( docToSave.DocumentInfo.Project.Settings.BasePath == null )
            ||   ( docToSave.DocumentInfo.Element == null ) )
            {
              // no project yet (or no project element)
              if ( !SaveProject( docToSave.DocumentInfo.Project ) )
              {
                return true;
              }
            }
            docToSave.Save( BaseDocument.SaveMethod.SAVE );
            if ( !SaveProject( docToSave.DocumentInfo.Project ) )
            {
              return true;
            }
          }
          return true;
        case RetroDevStudio.Types.Function.SAVE_DOCUMENT_AS:
          {
            // save current document as
            BaseDocument docToSave = ActiveContent;
            if ( ( docToSave != null )
            &&   ( !docToSave.IsSaveable ) )
            {
              docToSave = ActiveDocument;
            }
            if ( ( docToSave == null )
            ||   ( !docToSave.IsSaveable ) )
            {
              break;
            }

            if ( docToSave.DocumentInfo.Project == null )
            {
              if ( docToSave.Save( BaseDocument.SaveMethod.SAVE_COPY_AS, out string newFilename ) )
              {
                if ( StudioCore.Navigating.FindDocumentByFilename( newFilename ) == null )
                {
                  OpenFile( newFilename );
                }
              }
              return true;
            }

            if ( ( docToSave.DocumentInfo.Project == null )
            ||   ( docToSave.DocumentInfo.Project.Settings.BasePath == null )
            ||   ( docToSave.DocumentInfo.Element == null ) )
            {
              // no project yet (or no project element)
              if ( !SaveProject( docToSave.DocumentInfo.Project ) )
              {
                return true;
              }
            }
            if ( !docToSave.Save( BaseDocument.SaveMethod.SAVE_COPY_AS, out string newFilename2 ) )
            {
              return true;
            }
            if ( !SaveProject( docToSave.DocumentInfo.Project ) )
            {
              return true;
            }
            OpenFile( newFilename2 );
          }
          return true;
        case RetroDevStudio.Types.Function.COMPILE:
          {
            DocumentInfo docToCompile = DetermineDocumentToCompile( true );
            if ( docToCompile != null )
            {
              Compile( docToCompile );
              return true;
            }
          }
          break;
        case RetroDevStudio.Types.Function.BUILD:
          {
            DocumentInfo docToCompile = DetermineDocumentToCompile( true );
            if ( docToCompile != null )
            {
              Build( docToCompile );
              return true;
            }
          }
          break;
        case Function.BUILD_TO_PREPROCESSED_FILE:
          {
            DocumentInfo docToCompile = DetermineDocumentToCompile( true );
            if ( docToCompile != null )
            {
              Build( docToCompile, true, false );
              return true;
            }
          }
          break;
        case Function.BUILD_TO_RELOCATION_FILE:
          {
            DocumentInfo docToCompile = DetermineDocumentToCompile( true );
            if ( docToCompile != null )
            {
              Build( docToCompile, false, true );
              return true;
            }
          }
          break;
        case RetroDevStudio.Types.Function.REBUILD:
          {
            DocumentInfo docToCompile = DetermineDocumentToCompile( true );
            if ( docToCompile != null )
            {
              Rebuild( docToCompile );
              return true;
            }
          }
          break;
        case RetroDevStudio.Types.Function.BUILD_AND_RUN:
          {
            DocumentInfo docToCompile = DetermineDocumentToCompile( true );
            if ( docToCompile != null )
            {
              BuildAndRun( docToCompile, docToCompile );
              return true;
            }
          }
          break;
        case RetroDevStudio.Types.Function.BUILD_AND_DEBUG:
          if ( AppState == Types.StudioState.NORMAL )
          {
            DocumentInfo docToCompile = DetermineDocumentToCompile( true );
            if ( docToCompile != null )
            {
              BuildAndDebug( docToCompile, DetermineDocumentToHandle(), docToCompile );
              return true;
            }
          }
          else if ( AppState == Types.StudioState.DEBUGGING_BROKEN )
          {
            DebugGo();
            return true;
          }
          break;
        case RetroDevStudio.Types.Function.GO_TO_DECLARATION:
          {
            DocumentInfo docToDebug = DetermineDocumentToCompile( false );
            DocumentInfo docToHandle = DetermineDocument();

            if ( docToDebug == null )
            {
              docToDebug = docToHandle;
            }
            if ( docToDebug.Type != ProjectElement.ElementType.ASM_SOURCE )
            {
              break;
            }
            SourceASMEx sourceEx = docToHandle.BaseDoc as SourceASMEx;

            //EnsureFileIsParsed( docToDebug );

            if ( sourceEx != null )
            {
              sourceEx.GoToDeclarationAtCaretPosition();
              return true;
            }
          }
          break;
        case RetroDevStudio.Types.Function.UNDO:
          BaseDocument docUndo = ActiveDocument;
          if ( ( docUndo != null )
          &&   ( docUndo.UndoPossible ) )
          {
            docUndo.Undo();
            return true;
          }
          break;
        case RetroDevStudio.Types.Function.REDO:
          BaseDocument docRedo = ActiveDocument;
          if ( ( docRedo != null )
          &&   ( docRedo.RedoPossible ) )
          {
            docRedo.Redo();
            return true;
          }
          break;
        case RetroDevStudio.Types.Function.DELETE_LINE:
        case RetroDevStudio.Types.Function.COPY_LINE_UP:
        case RetroDevStudio.Types.Function.COPY_LINE_DOWN:
        case RetroDevStudio.Types.Function.MOVE_LINE_DOWN:
        case RetroDevStudio.Types.Function.MOVE_LINE_UP:
          // let control handle it
          return false;
        case Function.COPY:
          if ( ( ActiveContent == null )
          ||   ( !ActiveContent.CopyPossible ) )
          {
            return false;
          }
          ActiveContent.Copy();
          return true;
        case Function.PASTE:
          if ( ( ActiveContent == null )
          ||   ( !ActiveContent.PastePossible ) )
          {
            return false;
          }
          ActiveContent.Paste();
          return true;
        case Function.CUT:
          if ( ( ActiveContent == null )
          ||   ( !ActiveContent.CutPossible ) )
          {
            return false;
          }
          ActiveContent.Cut();
          return true;
        case Function.FIND_ALL_REFERENCES:
        case Function.RENAME_ALL_REFERENCES:
          {
            DocumentInfo docToHandle = DetermineDocument();

            if ( docToHandle.Type != ProjectElement.ElementType.ASM_SOURCE )
            {
              break;
            }
            SourceASMEx sourceEx = docToHandle.BaseDoc as SourceASMEx;

            if ( sourceEx != null )
            {
              return sourceEx.ApplyFunction( Function );
            }
          }
          return false;
      }
      return false;
    }



    public bool HandleCmdKey( ref Message msg, Keys keyData )
    {
      AcceleratorKey usedAccelerator = StudioCore.Settings.DetermineAccelerator( keyData, AppState );
      if ( usedAccelerator != null )
      {
        return ApplyFunction( usedAccelerator.Function );
      }
      return false;
    }



    protected override bool ProcessCmdKey( ref Message msg, Keys keyData )
    {
      if ( keyData == (Keys)( Keys.Control | Keys.Tab ) )
      {
        int curIndex = 0;
        foreach ( var doc in panelMain.Documents )
        {
          if ( doc == panelMain.ActiveDocument )
          {
            int nextIndex = ( curIndex + 1 ) % panelMain.DocumentsCount;

            IDockContent    nextDoc = panelMain.Documents.ElementAt( nextIndex );
            nextDoc.DockHandler.Activate();
            break;
          }
          ++curIndex;
        }
        return true;
      }
      if ( keyData == (Keys)( Keys.Control | Keys.Tab | Keys.Shift ) )
      {
        int curIndex = 0;
        foreach ( var doc in panelMain.Documents )
        {
          if ( doc == panelMain.ActiveDocument )
          {
            int nextIndex = ( curIndex + panelMain.DocumentsCount - 1 ) % panelMain.DocumentsCount;

            IDockContent    nextDoc = panelMain.Documents.ElementAt( nextIndex );
            nextDoc.DockHandler.Activate();
            break;
          }
          ++curIndex;
        }
        return true;
      }
      AcceleratorKey usedAccelerator = StudioCore.Settings.DetermineAccelerator( keyData, AppState );
      if ( usedAccelerator != null )
      {
        if ( ApplyFunction( usedAccelerator.Function ) )
        {
          return true;
        }
      }
      return base.ProcessCmdKey( ref msg, keyData );
    }



    private void listBreakpointsToolStripMenuItem_Click( object sender, EventArgs e )
    {
      if ( AppState == Types.StudioState.DEBUGGING_BROKEN )
      {
        StudioCore.Debugging.Debugger.Break();
      }
    }



    public void AddWatchEntry( WatchEntry Watch )
    {
      if ( ( AppState == Types.StudioState.DEBUGGING_RUN )
      ||   ( AppState == Types.StudioState.DEBUGGING_BROKEN )
      ||   ( AppState == Types.StudioState.NORMAL ) )
      {
        m_DebugWatch.AddWatchEntry( Watch );

        m_CurrentProject?.Settings.WatchEntries.Add( Watch );

        if ( IsWatchShowingCurrentDebuggedProject() )
        {
          StudioCore.Debugging.Debugger?.AddWatchEntry( Watch );

          if ( AppState == Types.StudioState.DEBUGGING_BROKEN )
          {
            StudioCore.Debugging.Debugger?.RefreshRegistersAndWatches();
          }
        }
      }
    }



    private bool IsWatchShowingCurrentDebuggedProject()
    {
      if ( ( StudioCore.Debugging.Debugger != null )
      &&   ( StudioCore.Debugging.DebuggedProject == m_CurrentProject ) )
      {
        return true;
      }
      return false;
    }



    public void RemoveWatchEntry( WatchEntry Watch )
    {
      m_CurrentProject?.Settings.WatchEntries.Remove( Watch );
      m_DebugWatch.RemoveWatchEntry( Watch );
      if ( IsWatchShowingCurrentDebuggedProject() )
      {
        StudioCore.Debugging.Debugger?.RemoveWatchEntry( Watch );
      }
    }



    private void UpdateWatchInfo( RequestData Request, GR.Memory.ByteBuffer Data )
    {
      if ( InvokeRequired )
      {
        try
        {
          Invoke( new UpdateWatchInfoCallback( UpdateWatchInfo ), new object[] { Request, Data } );
        }
        catch ( System.Exception ex )
        {
          Debug.Log( ex.Message );
        }
      }
      else
      {
        if ( Request.Type == DebugRequestType.MEM_DUMP )
        {
          // display disassembly?
          if ( Request.Parameter1 == StudioCore.Debugging.CurrentCodePosition )
          {
            if ( StudioCore.Debugging.DebugDisassembly != null )
            {
              // update disassembly
              Parser.Disassembler disassembler = new RetroDevStudio.Parser.Disassembler( Tiny64.Processor.Create6510() );
              string disassembly = "";

              disassembler.SetData( Data );

              GR.Collections.Set<int> jumpedAtAddresses = new GR.Collections.Set<int>();
              jumpedAtAddresses.Add( StudioCore.Debugging.CurrentCodePosition );
              GR.Collections.Map<int, string> namedLabels = new GR.Collections.Map<int, string>();

              var settings = new DisassemblerSettings() { AddLineAddresses = true, AddAssembledBytes = true };

              if ( disassembler.Disassemble( StudioCore.Debugging.CurrentCodePosition, jumpedAtAddresses, namedLabels, settings, out disassembly, out int firstLineIndexWithOpcode ) )
              {
                StudioCore.Debugging.DebugDisassembly.SetText( disassembly );
                StudioCore.Debugging.MarkedDocument?.SetLineMarked( StudioCore.Debugging.MarkedDocumentLine, false );

                StudioCore.Debugging.MarkedDocument = StudioCore.Debugging.DebugDisassembly;
                StudioCore.Debugging.MarkedDocumentLine = 1;
                StudioCore.Debugging.DebugDisassembly.Select();
                StudioCore.Debugging.DebugDisassembly.ReadjustMarkedLine();
                //StudioCore.Debugging.DebugDisassembly.SetLineMarked( 1, 1 != -1 );
              }
              else
              {
                if ( StudioCore.Debugging.MarkedDocument != null )
                {
                  StudioCore.Debugging.MarkLine( StudioCore.Debugging.MarkedDocument.DocumentInfo.Project, StudioCore.Debugging.MarkedDocument.DocumentInfo, -1 );
                  StudioCore.Debugging.MarkedDocument = null;
                }
                StudioCore.Debugging.DebugDisassembly.SetText( "Disassembly\r\nfailed\r\n" + disassembly );
              }
            }
          }
        }

        if ( Request.Info == "RetroDevStudio.MemDump" )
        {
          StudioCore.Debugging.UpdateMemory( Request, Data, false );
        }
        else if ( Request.Info == "RetroDevStudio.MemDumpRAM" )
        {
          StudioCore.Debugging.UpdateMemory( Request, Data, true );
        }
        else
        {
          m_DebugWatch.UpdateValue( Request, Data );
        }
      }
    }



    public void EnsureFileIsParsed()
    {
      if ( ActiveDocumentInfo != null )
      {
        EnsureFileIsParsed( ActiveDocumentInfo );
      }
    }



    public bool ParseFile( Parser.ParserBase Parser, DocumentInfo Document, ProjectConfig Configuration, string AdditionalPredefines, bool OutputMessages, bool CreatePreProcessedFile, bool CreateRelocationFile, out RetroDevStudio.Types.ASM.FileInfo ASMFileInfo )
    {
      ASMFileInfo = null;

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.Assembler = Types.AssemblerType.AUTO;
      if ( Document.Element != null )
      {
        config.Assembler = Document.Element.AssemblerType;
      }
      config.AutoTruncateLiteralValues  = StudioCore.Settings.ASMAutoTruncateLiteralValues;
      config.CreatePreProcesseFile      = CreatePreProcessedFile;
      config.CreateRelocationFile       = CreateRelocationFile;
      config.LibraryFiles               = StudioCore.Settings.ASMLibraryPaths;
      config.InputFile                  = Document.FullPath;
      config.WarningsToTreatAsError     = StudioCore.Settings.TreatWarningsAsErrors;
      config.EnabledHacks               = StudioCore.Settings.EnabledC64StudioHacks;

      string sourceCode = "";

      if ( Document.BaseDoc != null )
      {
        if ( Document.Type == ProjectElement.ElementType.ASM_SOURCE )
        {
          sourceCode = Document.BaseDoc.GetContent();
        }
        else if ( Document.Type == ProjectElement.ElementType.BASIC_SOURCE )
        {
          if ( !( (SourceBasicEx)Document.BaseDoc ).GetCompilableCode( out sourceCode ) )
          {
            return false;
          }
        }
      }

      bool result = Parser.ParseFile( Document.FullPath, sourceCode, Configuration, config, AdditionalPredefines, 
                                      Document.LabelModeReferences, out ASMFileInfo );

      Document.ASMFileInfo = ASMFileInfo;

      if ( Document.Type == ProjectElement.ElementType.BASIC_SOURCE )
      {
        if ( ASMFileInfo != null )
        {
          if ( Document.ASMFileInfoOriginal != null )
          {
            ASMFileInfo = Document.ASMFileInfoOriginal;

            Document.ASMFileInfo = Document.ASMFileInfoOriginal;
            Document.ASMFileInfoOriginal = null;
          }
        }
      }


      if ( ( config.Assembler != RetroDevStudio.Types.AssemblerType.AUTO )
      &&   ( Document.BaseDoc != null )
      &&   ( Document.Element != null ) )
      {
        if ( Document.Element.AssemblerType != config.Assembler )
        {
          Document.Element.AssemblerType = config.Assembler;
          Document.BaseDoc.SetModified();
        }
      }

      DependencyBuildState buildState = null;
      if ( Configuration != null )
      {
        buildState = Document.DeducedDependency[Configuration.Name];
        if ( buildState == null )
        {
          buildState = new DependencyBuildState();
          Document.DeducedDependency[Configuration.Name] = buildState;
        }
        buildState.Clear();
        

        // auto-add all external dependencies with their current time stamp
        if ( Document.Element != null )
        {
          foreach ( var externalDependency in Document.Element.ExternalDependencies.DependentOnFile )
          {
            string fullPath = Document.Project.FullPath( externalDependency.Filename );

            DateTime    fileTime = new DateTime();

            try
            {
              fileTime = System.IO.File.GetLastWriteTime( fullPath );
            }
            catch
            {
            }
            buildState.BuildState[fullPath].TimeStampOfSourceFile = fileTime;
          }
        }
      }

      if ( Document.Element != null )
      {
        Document.Element.CompileTarget      = Parser.CompileTarget;
        Document.Element.CompileTargetFile  = Parser.CompileTargetFile;
      }
      if ( buildState != null )
      {
        foreach ( string dependency in StudioCore.Compiling.ParserASM.ExternallyIncludedFiles )
        {
          DateTime lastChangeTime = new DateTime();
          try
          {
            lastChangeTime = System.IO.File.GetLastWriteTime( dependency );
          }
          catch
          {
          }
          buildState.BuildState.Add( dependency, new SingleBuildInfo() { TimeStampOfSourceFile = lastChangeTime, TargetFile = Parser.CompileTargetFile, TargetType = Parser.CompileTarget }  );
        }
      }

      if ( Document.Type == ProjectElement.ElementType.ASM_SOURCE )
      {
        SourceASMEx asm = Document.BaseDoc as SourceASMEx;
        if ( asm != null )
        {
          asm.DoNotFollowZoneSelectors = true;
        }

        var knownTokens     = ASMFileInfo.KnownTokens();
        var knownTokenInfos = ASMFileInfo.KnownTokenInfo();

        if ( ( Document.Project != null )
        &&   ( !string.IsNullOrEmpty( Document.Project.Settings.MainDocument ) )
        &&   ( System.IO.Path.GetFileName( Document.FullPath ) == Document.Project.Settings.MainDocument ) )
        {
          // give all other files the same keywords!
          // from source info
          GR.Collections.Set<string> filesToUpdate = new GR.Collections.Set<string>();
          foreach ( Types.ASM.SourceInfo sourceInfo in Document.ASMFileInfo.SourceInfo.Values )
          {
            filesToUpdate.Add( sourceInfo.FullPath );
          }

          // from deduced dependencies
          foreach ( var dependencyBuildState in Document.DeducedDependency.Values )
          {
            foreach ( var dependency in dependencyBuildState.BuildState.Keys )
            {
              ProjectElement element2 = Document.Project.GetElementByFilename(dependency);
              if ( ( element2 != null )
              && ( element2.DocumentInfo.Type == ProjectElement.ElementType.ASM_SOURCE ) )
              {
                filesToUpdate.Add( element2.DocumentInfo.FullPath );
              }
            }
          }

          foreach ( string fileToUpdate in filesToUpdate )
          {
            ProjectElement elementToUpdate = Document.Project.GetElementByFilename(fileToUpdate);
            if ( elementToUpdate != null )
            {
              elementToUpdate.DocumentInfo.KnownKeywords  = knownTokens;
              elementToUpdate.DocumentInfo.KnownTokens    = knownTokenInfos;
              if ( elementToUpdate.Document != null )
              {
                StudioCore.TaskManager.AddTask( new Tasks.TaskUpdateKeywords( elementToUpdate.Document ) );
              }
            }
          }

          m_DebugBreakpoints.SetTokens( knownTokenInfos );
        }
        else
        {
          if ( Document != null )
          {
            Document.KnownKeywords  = knownTokens;
            Document.KnownTokens    = knownTokenInfos;
          }

          if ( !IsDocPartOfMainDocument( Document ) )
          {
            m_DebugBreakpoints.SetTokens( knownTokenInfos );
          }
        }

        if ( asm != null )
        {
          asm.DoNotFollowZoneSelectors = false;
        }
      }
      else if ( Document.Type == ProjectElement.ElementType.BASIC_SOURCE )
      {
        if ( ASMFileInfo != null )
        {
          if ( Document.ASMFileInfoOriginal != null )
          {
            Document.ASMFileInfo          = Document.ASMFileInfoOriginal;
            Document.ASMFileInfoOriginal  = null;
          }

          Document.KnownKeywords  = Document.ASMFileInfo.KnownTokens();
          Document.KnownTokens    = Document.ASMFileInfo.KnownTokenInfo();
        }
      }

      if ( OutputMessages )
      {
        StudioCore.TaskManager.AddTask( new Tasks.TaskUpdateCompileResult( Document.ASMFileInfo, Document ) );
      }
      if ( ( result )
      &&   ( Document.BaseDoc != null ) )
      {
        Document.BaseDoc.FileParsed = true;
      }

      return result;
    }



    public void EnsureFileIsParsed( DocumentInfo Document )
    {
      if ( ( ( Document.BaseDoc != null )
      &&     ( !Document.BaseDoc.FileParsed ) )
      ||   ( StudioCore.Compiling.NeedsRebuild( Document ) ) )
      {
        if ( StudioCore.Compiling.NeedsRebuild( Document ) )
        {
          Compile( Document );

          if ( Document.BaseDoc != null )
          {
            Document.BaseDoc.FileParsed = true;
          }
          return;
        }
        if ( Document.BaseDoc != null )
        {
          Document.BaseDoc.FileParsed = true;
        }
        ProjectConfig config = null;
        if ( Document.Element != null )
        {
          config = Document.Project.Settings.Configuration( mainToolConfig.SelectedItem.ToString() );
        }
        ParseFile( StudioCore.DetermineParser( Document ), Document, config, null, false, false, false, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo );
      }
    }



    private void MainForm_FormClosing( object sender, FormClosingEventArgs e )
    {
      if ( e.Cancel )
      {
        return;
      }

      if ( s_SystemShutdown )
      {
        // changes are saved as restart data
        SaveSettings();
        StudioCore.ShuttingDown = true;
        return;
      }

      Program.s_Exiting = true;

      if ( m_CurrentProject == null )
      {
        try
        {
          foreach ( IDockContent dock in panelMain.Contents )
          {
            if ( dock is BaseDocument )
            {
              BaseDocument doc = (BaseDocument)dock;
              if ( doc.Modified )
              {
                DialogResult saveRequestResult = doc.CloseAfterModificationRequest();
                if ( saveRequestResult == DialogResult.Cancel )
                {
                  e.Cancel = true;
                  return;
                }
              }
            }
          }
        }
        catch ( Exception ex )
        {
          Debug.Log( ex.Message );
        }
      }

      // check ALL projects
      List<string>  itemsWithChanges = new List<string>();

      foreach ( BaseDocument doc in panelMain.Documents )
      {
        if ( doc.Modified )
        {
          itemsWithChanges.Add( doc.Name );
        }
      }

      if ( StudioCore.Navigating.Solution != null )
      {
        foreach ( Project project in StudioCore.Navigating.Solution.Projects )
        {
          if ( project != null )
          {
            if ( project.Modified )
            {
              itemsWithChanges.Add( project.Settings.Name );
            }
            foreach ( var element in project.Elements )
            {
              if ( ( element.Document != null )
              &&   ( element.Document.Modified ) )
              {
                itemsWithChanges.Add( element.Name );
              }
            }
          }
        }
      }

      if ( itemsWithChanges.Any() )
      {
        DialogResult result = System.Windows.Forms.MessageBox.Show( "There are unsaved changes, Really shut down?", "Unsaved changes! Shut down?", MessageBoxButtons.YesNo );
        if ( result != DialogResult.Yes )
        {
          e.Cancel = true;
          return;
        }
      }
      e.Cancel = false;
      StudioCore.ShuttingDown = true;
      SaveSettings();
    }



    private void aboutToolStripMenuItem1_Click( object sender, EventArgs e )
    {
      FormAbout about = new FormAbout( StudioCore );

      about.ShowDialog();
    }



    private void fileNewProjectToolStripMenuItem_Click( object sender, EventArgs e )
    {
      AddNewProject( true );
    }



    private void fileNewASMFileToolStripMenuItem_Click( object sender, EventArgs e )
    {
      AddNewDocumentOrElement( ProjectElement.ElementType.ASM_SOURCE, "ASM File", m_CurrentProject, ( m_CurrentProject != null ) ? m_CurrentProject.Node : null );
    }



    private void fileNewMapEditorToolStripMenuItem_Click( object sender, EventArgs e )
    {
      AddNewDocumentOrElement( ProjectElement.ElementType.MAP_EDITOR, "Map Project", m_CurrentProject, ( m_CurrentProject != null ) ? m_CurrentProject.Node : null );
    }



    private void fileNewBasicFileToolStripMenuItem_Click( object sender, EventArgs e )
    {
      AddNewDocumentOrElement( ProjectElement.ElementType.BASIC_SOURCE, "BASIC File", m_CurrentProject, ( m_CurrentProject != null ) ? m_CurrentProject.Node : null );
    }



    private void fileSetupWizardToolStripMenuItem_Click( object sender, EventArgs e )
    {
      var wizard = new FormWizard( StudioCore );

      if ( wizard.ShowDialog() == DialogResult.OK )
      {
        string emulatorFilename = wizard.editPathEmulator.Text;

        ToolInfo toolEmulator = new ToolInfo();

        toolEmulator.Filename = wizard.editPathEmulator.Text;

        EmulatorInfo.SetDefaultRunArguments( toolEmulator );

        StudioCore.Settings.ToolInfos.Add( toolEmulator );
        RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.EMULATOR_LIST_CHANGED ) );
      }
    }



    private void MainForm_Shown( object sender, EventArgs e )
    {
      if ( StudioCore.Settings.ToolInfos.Count == 0 )
      {
        if ( System.Windows.Forms.MessageBox.Show( "There are currently no tools setup. Do you want to do this now?", "Setup Tools", MessageBoxButtons.YesNo ) == DialogResult.Yes )
        {
          fileSetupWizardToolStripMenuItem_Click( this, null );
        }
      }
    }



    public void MainForm_DragDrop( object sender, DragEventArgs e )
    {
      if ( !e.Data.GetDataPresent( DataFormats.FileDrop ) )
      {
        return;
      }
      string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop);
      if ( fileList != null )
      {
        foreach ( string file in fileList )
        {
          OpenFile( file );
        }
      }
    }



    public bool CloseSolution()
    {
      if ( StudioCore.Navigating.Solution != null )
      {
        if ( !CloseAllProjects() )
        {
          return false;
        }
        StudioCore.Navigating.Solution.Projects.Clear();
        StudioCore.Navigating.Solution = null;
        StudioCore.Settings.LastSolutionWasEmpty = true;

        // clear entries
        m_DebugWatch.ClearAllWatchEntries();
        if ( StudioCore.Debugging.Debugger != null )
        {
          StudioCore.Debugging.Debugger.ClearAllWatchEntries();
          StudioCore.Debugging.Debugger.ClearAllBreakpoints();
        }
        solutionToolStripMenuItemTop.Visible = false;
        RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.SOLUTION_CLOSED ) );
      }
      return true;
    }



    public bool OpenSolution( string Filename )
    {
      CloseSolution();

      StudioCore.Navigating.Solution = new Solution( this );

      GR.Memory.ByteBuffer solutionData = GR.IO.File.ReadAllBytes(Filename);
      if ( solutionData == null )
      {
        StudioCore.Navigating.Solution = null;
        return false;
      }
      StudioCore.Navigating.Solution.DuringLoad = true;
      if ( !StudioCore.Navigating.Solution.FromBuffer( solutionData, Filename ) )
      {
        StudioCore.Settings.RemoveFromMRU( StudioCore.Settings.MRUProjects, Filename, this );
        CloseSolution();
        StudioCore.Navigating.Solution = null;
        return false;
      }
      StudioCore.Settings.UpdateInMRU( StudioCore.Settings.MRUProjects, Filename, this );
      StudioCore.Settings.LastSolutionWasEmpty = false;

      StudioCore.Navigating.Solution.Modified   = false;
      StudioCore.Navigating.Solution.DuringLoad = false;
      solutionToolStripMenuItemTop.Visible      = true;

      RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.SOLUTION_OPENED ) );
      return true;
    }



    public void SaveSolution()
    {
      if ( StudioCore.Navigating.Solution == null )
      {
        return;
      }
      if ( string.IsNullOrEmpty( StudioCore.Navigating.Solution.Filename ) )
      {
        if ( InvokeRequired )
        {
          // skip saving if we're in the wrong thread!
          return;
        }
        System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

        saveDlg.Title = "Save Solution as";
        saveDlg.Filter = FilterString( Types.Constants.FILEFILTER_SOLUTION + Types.Constants.FILEFILTER_ALL );
        if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
        {
          return;
        }
        StudioCore.Navigating.Solution.Filename = saveDlg.FileName;
      }
      GR.IO.File.WriteAllBytes( StudioCore.Navigating.Solution.Filename, StudioCore.Navigating.Solution.ToBuffer() );
      StudioCore.Navigating.Solution.Modified = false;
      StudioCore.Settings.UpdateInMRU( StudioCore.Settings.MRUProjects, StudioCore.Navigating.Solution.Filename, this );

      RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.SOLUTION_SAVED ) );
    }



    public BaseDocument OpenFile( string Filename )
    {
      BaseDocument document = null;

      string extension = System.IO.Path.GetExtension( Filename ).ToUpper();
      if ( extension == ".C64" )
      {
        OpenProject( Filename );
        return null;
      }
      else if ( extension == ".S64" )
      {
        OpenSolution( Filename );
        return null;
      }

      Project  project;
      if ( ( StudioCore.Navigating.Solution != null )
      &&   ( StudioCore.Navigating.Solution.FilenameUsed( Filename, out project ) ) )
      {
        // file is part of a project!
        StudioCore.Settings.UpdateInMRU( StudioCore.Settings.MRUFiles, Filename, this );
        return project.ShowDocument( project.GetElementByFilename( Filename ) );
      }
      // file already opened?
      var docInfo = StudioCore.Navigating.FindDocumentInfoByPath( Filename );
      if ( ( docInfo != null )
      &&   ( docInfo.BaseDoc != null ) )
      {
        StudioCore.Settings.UpdateInMRU( StudioCore.Settings.MRUFiles, Filename, this );
        docInfo.BaseDoc.Show();
        return docInfo.BaseDoc;
      }

      bool    openDirectFile = true;

      if ( ( extension == ".D64" )
      ||   ( extension == ".D71" )
      ||   ( extension == ".D81" )
      ||   ( extension == ".T64" )
      ||   ( extension == ".DSK" )
      ||   ( extension == ".ADF" )
      ||   ( extension == ".PRG" ) )
      {
        document = new FileManager( StudioCore, Filename );
        document.ShowHint = DockState.Float;
        openDirectFile = false;
      }
      else if ( extension == ".DISASSEMBLY" )
      {
        document = new Disassembler( StudioCore );
        document.ShowHint = DockState.Document;
      }
      else if ( ( extension == ".SPRITEPROJECT" )
      ||        ( extension == ".SPR" ) )
      {
        document = new SpriteEditor( StudioCore );
        document.ShowHint = DockState.Document;
      }
      else if ( extension == ".VALUETABLEPROJECT" )
      {
        document = new ValueTableEditor( StudioCore );
        document.ShowHint = DockState.Document;
      }
      else if ( ( extension == ".CHARSETPROJECT" )
      ||        ( extension == ".CHR" ) )
      {
        document = new CharsetEditor( StudioCore );
        document.ShowHint = DockState.Document;
      }
      else if ( extension == ".CHARSCREEN" )
      {
        document = new CharsetScreenEditor( StudioCore );
        document.ShowHint = DockState.Document;
      }
      else if ( extension == ".GRAPHICSCREEN" )
      {
        document = new GraphicScreenEditor( StudioCore );
        document.ShowHint = DockState.Document;
      }
      else if ( ( extension == ".BAS" )
      ||        ( extension == ".B" ) )
      {
        document = new SourceBasicEx( StudioCore );
        document.ShowHint = DockState.Document;
        document.DocumentEvent += new BaseDocument.DocumentEventHandler( StudioCore.MainForm.Document_DocumentEvent );
      }
      else if ( extension == ".MAPPROJECT" )
      {
        document = new MapEditor( StudioCore );
        document.ShowHint = DockState.Document;
      }
      else if ( extension == ".CTM" )
      {
        // a charpad file
        openDirectFile = false;
        document = new MapEditor( StudioCore );
        document.ShowHint = DockState.Document;
        ( (MapEditor)document ).OpenCharpadFile( Filename );
      }
      else if ( extension == ".SPD" )
      {
        // a SpritePad file
        openDirectFile = false;
        document = new SpriteEditor( StudioCore );
        document.ShowHint = DockState.Document;
        ( (SpriteEditor)document ).ImportSprites( Filename, true, true );
      }
      else
      {
        document = new SourceASMEx( StudioCore );
        document.ShowHint = DockState.Document;
        document.DocumentEvent += new BaseDocument.DocumentEventHandler( StudioCore.MainForm.Document_DocumentEvent );
      }

      document.Core = StudioCore;
      if ( openDirectFile )
      {
        document.SetDocumentFilename( Filename );
        document.Text = System.IO.Path.GetFileName( Filename );
        if ( !document.LoadDocument() )
        {
          document.ToolTipText = "";
          return null;
        }
        document.ToolTipText = document.DocumentInfo.FullPath;
      }
      document.Show( panelMain );
      document.Icon = IconFromType( document.DocumentInfo );
      document.DocumentInfo.UndoManager.MainForm = this;
      ApplicationEvent += document.OnApplicationEvent;

      StudioCore.Settings.UpdateInMRU( StudioCore.Settings.MRUFiles, Filename, this );

      RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.DOCUMENT_INFO_CREATED, document.DocumentInfo ) );

      return document;
    }



    public Icon IconFromType( DocumentInfo DocInfo )
    {
      if ( DocInfo.Element == null )
      {
        return System.Drawing.SystemIcons.Asterisk;
      }

      switch ( DocInfo.Type )
      {
        case ProjectElement.ElementType.ASM_SOURCE:
          return RetroDevStudio.Properties.Resources.source;
        case ProjectElement.ElementType.BASIC_SOURCE:
          return RetroDevStudio.Properties.Resources.source_basic;
        case ProjectElement.ElementType.CHARACTER_SCREEN:
          return RetroDevStudio.Properties.Resources.charsetscreen;
        case ProjectElement.ElementType.CHARACTER_SET:
          return RetroDevStudio.Properties.Resources.charset;
        case ProjectElement.ElementType.FOLDER:
          return RetroDevStudio.Properties.Resources.folder;
        case ProjectElement.ElementType.GRAPHIC_SCREEN:
          return RetroDevStudio.Properties.Resources.graphicscreen;
        case ProjectElement.ElementType.MAP_EDITOR:
          return RetroDevStudio.Properties.Resources.mapeditor;
        case ProjectElement.ElementType.PROJECT:
          return RetroDevStudio.Properties.Resources.project;
        case ProjectElement.ElementType.SOLUTION:
          return RetroDevStudio.Properties.Resources.solution;
        case ProjectElement.ElementType.SPRITE_SET:
          return RetroDevStudio.Properties.Resources.spriteset;
        case ProjectElement.ElementType.BINARY_FILE:
          return RetroDevStudio.Properties.Resources.binary;
        case ProjectElement.ElementType.VALUE_TABLE:
          return RetroDevStudio.Properties.Resources.valuetable;
      }
      return System.Drawing.SystemIcons.Asterisk;
    }



    public BaseDocument CreateNewDocument( ProjectElement.ElementType Type, Project Project )
    {
      BaseDocument newDoc = null;

      switch ( Type )
      {
        case ProjectElement.ElementType.ASM_SOURCE:
          newDoc = new SourceASMEx( StudioCore );
          break;
        case ProjectElement.ElementType.BASIC_SOURCE:
          newDoc = new SourceBasicEx( StudioCore );
          break;
        case ProjectElement.ElementType.CHARACTER_SCREEN:
          newDoc = new CharsetScreenEditor( StudioCore );
          break;
        case ProjectElement.ElementType.CHARACTER_SET:
          newDoc = new CharsetEditor( StudioCore );
          break;
        case ProjectElement.ElementType.GRAPHIC_SCREEN:
          newDoc = new GraphicScreenEditor( StudioCore );
          break;
        case ProjectElement.ElementType.MAP_EDITOR:
          newDoc = new MapEditor( StudioCore );
          break;
        case ProjectElement.ElementType.SPRITE_SET:
          newDoc = new SpriteEditor( StudioCore );
          break;
        case ProjectElement.ElementType.DISASSEMBLER:
          newDoc = new Documents.Disassembler( StudioCore );
          break;
        case ProjectElement.ElementType.BINARY_FILE:
          newDoc = new BinaryDisplay( StudioCore, null, true, false );
          break;
        case ProjectElement.ElementType.VALUE_TABLE:
          newDoc = new ValueTableEditor( StudioCore );
          break;
      }
      if ( newDoc != null )
      {
        newDoc.DocumentInfo.Project = Project;
        newDoc.DocumentInfo.Type = Type;
        newDoc.DocumentInfo.UndoManager.MainForm = this;
        newDoc.DocumentInfo.BaseDoc = newDoc;
        newDoc.ShowHint = DockState.Document;
        newDoc.Core = StudioCore;
        newDoc.Text = "*";
        newDoc.Show( panelMain );
        newDoc.DocumentInfo.Project = Project;
        newDoc.Icon = IconFromType( newDoc.DocumentInfo );

        ApplicationEvent += newDoc.OnApplicationEvent;
      }
      return newDoc;
    }



    bool ChooseFilename( ProjectElement.ElementType Type, string DefaultName, Project ParentProject, out string NewName )
    {
      System.Windows.Forms.OpenFileDialog openDlg = new System.Windows.Forms.OpenFileDialog();
      openDlg.Title = "Specify new " + DefaultName;
      NewName = "";

      string filterSource = "";
      switch ( Type )
      {
        case ProjectElement.ElementType.ASM_SOURCE:
          filterSource += Types.Constants.FILEFILTER_ASM;
          break;
        case ProjectElement.ElementType.BASIC_SOURCE:
          filterSource += Types.Constants.FILEFILTER_BASIC;
          break;
        case ProjectElement.ElementType.CHARACTER_SCREEN:
          filterSource += Types.Constants.FILEFILTER_CHARSET_SCREEN;
          break;
        case ProjectElement.ElementType.CHARACTER_SET:
          filterSource += Types.Constants.FILEFILTER_CHARSET_PROJECT;
          break;
        case ProjectElement.ElementType.GRAPHIC_SCREEN:
          filterSource += Types.Constants.FILEFILTER_GRAPHIC_SCREEN;
          break;
        case ProjectElement.ElementType.SPRITE_SET:
          filterSource += Types.Constants.FILEFILTER_SPRITE_PROJECT;
          break;
        case ProjectElement.ElementType.MAP_EDITOR:
          filterSource += Types.Constants.FILEFILTER_MAP;
          break;
        case ProjectElement.ElementType.VALUE_TABLE:
          filterSource += Types.Constants.FILEFILTER_VALUE_TABLE_PROJECT;
          break;
      }
      openDlg.Filter = FilterString( filterSource );
      if ( ParentProject != null )
      {
        openDlg.InitialDirectory = ParentProject.Settings.BasePath;
      }
      else
      {
        openDlg.InitialDirectory = StudioCore.Settings.DefaultProjectBasePath;
      }
      openDlg.CheckFileExists = false;
      openDlg.CheckPathExists = true;
      if ( openDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return false;
      }
      NewName = openDlg.FileName;
      return true;
    }



    public void AddNewElement( ProjectElement.ElementType Type, string Description, Project ParentProject, DecentForms.TreeView.TreeNode ParentNode )
    {
      string newFilename;
      if ( !ChooseFilename( Type, Description, ParentProject, out newFilename ) )
      {
        return;
      }

      if ( ParentProject != null )
      {
        foreach ( ProjectElement projElement in ParentProject.Elements )
        {
          if ( GR.Path.IsPathEqual( newFilename, ParentProject.FullPath( projElement.Filename ) ) )
          {
            System.Windows.Forms.MessageBox.Show( "File " + newFilename + " is already part of this project", "File already added" );
            return;
          }
        }
      }
      if ( System.IO.File.Exists( newFilename ) )
      {
        var result = System.Windows.Forms.MessageBox.Show("There is already an existing file at " + newFilename + ".\r\nDo you want to overwrite it?", "Overwrite existing file?", MessageBoxButtons.YesNo);
        if ( result == DialogResult.No )
        {
          return;
        }
      }

      if ( ParentProject != null )
      {
        string localizedFilename = GR.Path.RelativePathTo(System.IO.Path.GetFullPath(ParentProject.Settings.BasePath), true, newFilename, false);

        ProjectElement el = CreateNewElement(Type, Description, ParentProject, ParentNode);
        el.Filename = localizedFilename;
        el.Node.Text = System.IO.Path.GetFileName( localizedFilename );
        el.Document.SetDocumentFilename( localizedFilename );
        el.Document.Save( BaseDocument.SaveMethod.SAVE );
      }
    }



    public void MainForm_DragEnter( object sender, DragEventArgs e )
    {
      e.Effect = DragDropEffects.All;
    }



    void Task_TaskFinished( RetroDevStudio.Tasks.Task FinishedTask )
    {
      if ( InvokeRequired )
      {
        Invoke( new TaskFinishedCallback( Task_TaskFinished ), new object[] { FinishedTask } );
        return;
      }

      //Debug.Log( "task finished" );
      m_CurrentTask = null;

      switch ( FinishedTask.Type )
      {
        case RetroDevStudio.Tasks.Task.TaskType.PARSE_FILE:
          break;
        case RetroDevStudio.Tasks.Task.TaskType.OPEN_SOLUTION:
          {
            var taskOS = (RetroDevStudio.Tasks.TaskOpenSolution)FinishedTask;

            if ( !FinishedTask.TaskSuccessful )
            {
              StudioCore.Settings.RemoveFromMRU( StudioCore.Settings.MRUProjects, taskOS.SolutionFilename, this );
              CloseSolution();
            }
            else
            {
              StudioCore.Settings.UpdateInMRU( StudioCore.Settings.MRUProjects, taskOS.SolutionFilename, this );
              StudioCore.Navigating.Solution = taskOS.Solution;
              StudioCore.Navigating.Solution.Modified = false;
              RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.SOLUTION_OPENED ) );
              foreach ( var project in StudioCore.Navigating.Solution.Projects )
              {
                RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.PROJECT_OPENED, project ) );
              }
            }
          }
          break;
      }

      StudioCore.SetStatus( "Ready", false, 0 );

      StartNextTask();
    }



    private void StartNextTask()
    {
      if ( m_Tasks.Count > 0 )
      {
        m_CurrentTask = m_Tasks[0];
        m_Tasks.RemoveAt( 0 );

        /*
        if ( m_CurrentTask.InvokeRequired )
        {
          //Debug.Log( "Running direct task" );
          m_CurrentTask.RunTask();
        }
        else*/
        {
          //Debug.Log( "Running threaded task" );
          System.Threading.Thread workerThread = new System.Threading.Thread( new System.Threading.ThreadStart( m_CurrentTask.RunTask ) );

          StudioCore.SetStatus( m_CurrentTask.Description, true, 0 );

          workerThread.Start();
        }
      }
    }



    private void mainToolConfig_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( ( m_CurrentProject != null )
      &&   ( m_CurrentProject.Settings.CurrentConfig != m_CurrentProject.Settings.Configuration( mainToolConfig.SelectedItem.ToString() ) ) )
      {
        m_CurrentProject.Settings.CurrentConfig = m_CurrentProject.Settings.Configuration( mainToolConfig.SelectedItem.ToString() );

        ProjectConfigChanged();
      }
    }



    private void fileNewSpriteFileToolStripMenuItem_Click( object sender, EventArgs e )
    {
      AddNewDocumentOrElement( ProjectElement.ElementType.SPRITE_SET, "Sprite Set", m_CurrentProject, ( m_CurrentProject != null ) ? m_CurrentProject.Node : null );
    }



    private void fileNewCharacterFileToolStripMenuItem_Click( object sender, EventArgs e )
    {
      AddNewDocumentOrElement( ProjectElement.ElementType.CHARACTER_SET, "Character Set", m_CurrentProject, ( m_CurrentProject != null ) ? m_CurrentProject.Node : null );
    }



    private void mainToolNewSpriteFile_Click( object sender, EventArgs e )
    {
      AddNewDocumentOrElement( ProjectElement.ElementType.SPRITE_SET, "Sprite Set", m_CurrentProject, ( m_CurrentProject != null ) ? m_CurrentProject.Node : null );
    }



    private void mainToolNewCharsetFile_Click( object sender, EventArgs e )
    {
      AddNewDocumentOrElement( ProjectElement.ElementType.CHARACTER_SET, "Character Set", m_CurrentProject, ( m_CurrentProject != null ) ? m_CurrentProject.Node : null );
    }



    private void mainToolUndo_Click( object sender, EventArgs e )
    {
      if ( ActiveDocument != null )
      {
        ActiveDocument.Undo();
        UpdateUndoSettings();
      }
    }



    private void mainToolRedo_Click( object sender, EventArgs e )
    {
      if ( ActiveDocument != null )
      {
        ActiveDocument.Redo();
        UpdateUndoSettings();
      }
    }



    public void UpdateUndoSettings()
    {
      if ( InvokeRequired )
      {
        Invoke( new ParameterLessCallback( UpdateUndoSettings ) );
        return;
      }

      btnNavigateForward.Enabled = StudioCore.Navigating.NavigateForwardPossible;
      btnNavigateBackward.Enabled = StudioCore.Navigating.NavigateBackwardPossible;

      if ( ActiveDocument == null )
      {
        mainToolUndo.Enabled = false;
        mainToolRedo.Enabled = false;
        undoToolStripMenuItem.Enabled = false;
        redoToolStripMenuItem.Enabled = false;

        copyToolStripMenuItem.Enabled = false;
        cutToolStripMenuItem.Enabled = false;
        pasteToolStripMenuItem.Enabled = false;
        deleteToolStripMenuItem.Enabled = false;
        return;
      }
      mainToolUndo.Enabled = ActiveDocument.UndoPossible;
      mainToolRedo.Enabled = ActiveDocument.RedoPossible;
      undoToolStripMenuItem.Enabled = ActiveDocument.UndoPossible;
      redoToolStripMenuItem.Enabled = ActiveDocument.RedoPossible;

      copyToolStripMenuItem.Enabled = ActiveDocument.CopyPossible;
      cutToolStripMenuItem.Enabled = ActiveDocument.CutPossible;
      pasteToolStripMenuItem.Enabled = ActiveDocument.PastePossible;
      deleteToolStripMenuItem.Enabled = ActiveDocument.DeletePossible;

      mainToolUndo.ToolTipText = ActiveDocument.UndoInfo;
      mainToolRedo.ToolTipText = ActiveDocument.RedoInfo;


      bool modifications = false;
      foreach ( BaseDocument doc in panelMain.Contents )
      {
        if ( doc.Modified )
        {
          modifications = true;
          break;
        }
      }
      saveToolStripMenuItem.Enabled = ActiveDocument.Modified;
      saveAsToolStripMenuItem.Enabled = true;
      saveAllToolStripMenuItem.Enabled = modifications;
      mainToolSave.Enabled = ActiveDocument.Modified;
      mainToolSaveAll.Enabled = modifications;
    }



    private void undoToolStripMenuItem_Click( object sender, EventArgs e )
    {
      mainToolUndo_Click( null, null );
    }



    private void redoToolStripMenuItem_Click( object sender, EventArgs e )
    {
      mainToolRedo_Click( null, null );
    }



    private void mainToolCompile_Click_1( object sender, EventArgs e )
    {
      ApplyFunction( RetroDevStudio.Types.Function.COMPILE );
    }



    public void OnDocumentExternallyChanged( BaseDocument Doc )
    {
      if ( InvokeRequired )
      {
        if ( m_ExternallyChangedDocuments.ContainsValue( Doc ) )
        {
          return;
        }
        m_ExternallyChangedDocuments.Add( Doc );
        BeginInvoke( new DocCallback( OnDocumentExternallyChanged ), new object[] { Doc } );
      }
      else
      {
        if ( m_FilesChanged == null )
        {
          m_FilesChanged = new FormFilesChanged( Doc.DocumentInfo, StudioCore );
          if ( m_FilesChanged.ShowDialog( this ) == DialogResult.OK )
          {
            // TODO - prefer project files (need to be watched as well)
            foreach ( var changedDoc in m_FilesChanged.ChangedDocuments )
            {
              int cursorLine = changedDoc.BaseDoc.CursorLine;
              int charPos = changedDoc.BaseDoc.CursorPosInLine;
              changedDoc.BaseDoc.LoadDocument();
              //changedDoc.BaseDoc.SetModified();
              changedDoc.BaseDoc.SetCursorToLine( cursorLine, charPos, true );
            }
          }
          m_FilesChanged.Dispose();
          m_FilesChanged = null;
          m_ExternallyChangedDocuments.Clear();
        }
        else
        {
          m_ExternallyChangedDocuments.Remove( Doc );
          m_FilesChanged.AddChangedFile( Doc.DocumentInfo );
        }
      }
    }



    private void projectOpenTapeDiskFileMenuItem_Click( object sender, EventArgs e )
    {
      System.Windows.Forms.OpenFileDialog openDlg = new System.Windows.Forms.OpenFileDialog();
      openDlg.Title = "Open Tape/Disk File";
      openDlg.Filter = FilterString( Types.Constants.FILEFILTER_MEDIA_FILES + Types.Constants.FILEFILTER_TAPE + Types.Constants.FILEFILTER_DISK + Types.Constants.FILEFILTER_ALL );
      if ( openDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return;
      }
      OpenFile( openDlg.FileName );
    }



    private void newTapeImageToolStripMenuItem_Click( object sender, EventArgs e )
    {
      FileManager doc = new FileManager(StudioCore, "");
      doc.ShowHint = DockState.Float;
      doc.CreateEmptyTapeImage();
      doc.Core = StudioCore;
      doc.Show( panelMain );
    }



    private void newDiskImageToolStripMenuItem_Click( object sender, EventArgs e )
    {
      FileManager doc = new FileManager(StudioCore, "");
      doc.ShowHint = DockState.Float;
      doc.CreateEmptyDiskImage();
      doc.Core = StudioCore;
      doc.Show( panelMain );
    }



    private void emptyTapeT64ToolStripMenuItem_Click( object sender, EventArgs e )
    {
      FileManager doc = new FileManager(StudioCore, "");
      doc.ShowHint = DockState.Float;
      doc.CreateEmptyTapeImage();
      doc.Core = StudioCore;
      doc.Show( panelMain );
    }



    private void emptyDiskD64ToolStripMenuItem_Click( object sender, EventArgs e )
    {
      FileManager doc = new FileManager(StudioCore, "");
      doc.ShowHint = DockState.Float;
      doc.CreateEmptyDiskImage();
      doc.Core = StudioCore;
      doc.Show( panelMain );
    }



    private void mainToolNewBasicFile_Click( object sender, EventArgs e )
    {
      AddNewDocumentOrElement( ProjectElement.ElementType.BASIC_SOURCE, "BASIC File", m_CurrentProject, ( m_CurrentProject != null ) ? m_CurrentProject.Node : null );
    }



    private void fileNewScreenEditorToolStripMenuItem_Click( object sender, EventArgs e )
    {
      BaseDocument document = new GraphicScreenEditor(StudioCore);
      document.ShowHint = DockState.Document;
      document.Core = StudioCore;
      document.Text = "New Graphic Screen";
      document.LoadDocument();
      document.Show( panelMain );
    }



    private void propertiesToolStripMenuItem_Click( object sender, EventArgs e )
    {
      if ( m_CurrentProject == null )
      {
        return;
      }
      ProjectProperties dlgProps = new ProjectProperties(m_CurrentProject, m_CurrentProject.Settings, StudioCore);
      dlgProps.ShowDialog();

      if ( dlgProps.Modified )
      {
        m_CurrentProject.SetModified();
      }
    }



    private void helpToolStripMenuItem1_Click( object sender, EventArgs e )
    {
      ApplyFunction( RetroDevStudio.Types.Function.HELP );
    }



    private void disassembleToolsToolStripMenuItem_Click( object sender, EventArgs e )
    {
      CreateNewDocument( ProjectElement.ElementType.DISASSEMBLER, m_CurrentProject );
    }



    private void fileNewCharacterScreenEditorToolStripMenuItem_Click( object sender, EventArgs e )
    {
      BaseDocument document = new CharsetScreenEditor(StudioCore);
      document.ShowHint = DockState.Document;
      document.Core = StudioCore;
      document.Text = "New Character Screen";
      document.LoadDocument();
      document.Show( panelMain );
    }



    private void fileOpenToolStripMenuItem_Click( object sender, EventArgs e )
    {
      ApplyFunction( RetroDevStudio.Types.Function.OPEN_FILES );
    }



    private void searchToolStripMenuItem_Click( object sender, EventArgs e )
    {
      ApplyFunction( RetroDevStudio.Types.Function.FIND );
    }



    private void findReplaceToolStripMenuItem_Click( object sender, EventArgs e )
    {
      ApplyFunction( RetroDevStudio.Types.Function.FIND_REPLACE );
    }



    private void mainToolFind_Click( object sender, EventArgs e )
    {
      ApplyFunction( RetroDevStudio.Types.Function.FIND );
    }



    private void mainToolFindReplace_Click( object sender, EventArgs e )
    {
      ApplyFunction( RetroDevStudio.Types.Function.FIND_REPLACE );
    }



    private void mainToolPrint_Click( object sender, EventArgs e )
    {
      ApplyFunction( RetroDevStudio.Types.Function.PRINT );
    }



    private void mainToolSaveAll_Click( object sender, EventArgs e )
    {
      ApplyFunction( RetroDevStudio.Types.Function.SAVE_ALL );
    }



    private void saveToolStripMenuItem_Click( object sender, EventArgs e )
    {
      ApplyFunction( RetroDevStudio.Types.Function.SAVE_DOCUMENT );
    }



    private void saveAsToolStripMenuItem_Click( object sender, EventArgs e )
    {
      ApplyFunction( RetroDevStudio.Types.Function.SAVE_DOCUMENT_AS );
    }



    private void saveAllToolStripMenuItem_Click( object sender, EventArgs e )
    {
      ApplyFunction( RetroDevStudio.Types.Function.SAVE_ALL );
    }



    private void menuWindowToolbarMain_Click( object sender, EventArgs e )
    {
      StudioCore.Settings.ToolbarActiveMain = menuWindowToolbarMain.Checked;
      mainTools.Visible = StudioCore.Settings.ToolbarActiveMain;
    }



    private void menuWindowToolbarDebugger_Click( object sender, EventArgs e )
    {
      StudioCore.Settings.ToolbarActiveDebugger = menuWindowToolbarDebugger.Checked;
      debugTools.Visible = StudioCore.Settings.ToolbarActiveDebugger;
    }



    private void fileNewBinaryEditorToolStripMenuItem_Click( object sender, EventArgs e )
    {
      GR.Memory.ByteBuffer emptyData = new GR.Memory.ByteBuffer(2);
      BaseDocument document = new BinaryDisplay(StudioCore, emptyData, true, true);
      document.ShowHint = DockState.Document;
      document.Core = StudioCore;
      document.Text = "New Binary Data";
      document.LoadDocument();
      document.Show( panelMain );
    }



    private void dumpLabelsToolStripMenuItem_Click( object sender, EventArgs e )
    {
      DumpPanes( panelMain, "" );
    }



    private void DumpElementHierarchy( DecentForms.TreeView.TreeNode Node, string Indent )
    {
      Project project = m_SolutionExplorer.ProjectFromNode( Node );
      ProjectElement element = m_SolutionExplorer.ElementFromNode( Node );
      if ( ( element == null )
      && ( Node.Level > 0 ) )
      {
        Debug.Log( Indent + Node.Text );
      }
      else
      {
        if ( element == null )
        {
          Debug.Log( Indent + Node.Text );
        }
        else
        {
          string hier = string.Join(">", element.ProjectHierarchy.ToArray());
          Debug.Log( Indent + Node.Text + "(" + hier + ")" );
        }
        foreach ( var subNode in Node.Nodes )
        {
          DumpElementHierarchy( subNode, Indent + " " );
        }
      }
    }



    private void dumpHierarchyToolStripMenuItem_Click( object sender, EventArgs e )
    {
      foreach ( var node in m_SolutionExplorer.treeProject.Nodes )
      {
        DumpElementHierarchy( node, "" );
      }
      Debug.Log( "by project:" );
      foreach ( var node in m_SolutionExplorer.treeProject.Nodes )
      {
        Project project = m_SolutionExplorer.ProjectFromNode( node );
        Debug.Log( "Project " + project.Settings.Name );

        foreach ( ProjectElement element in project.Elements )
        {
          string hier = string.Join(">", element.ProjectHierarchy.ToArray());
          Debug.Log( "-" + element.Name + " (" + hier + ")" );
        }
      }
    }



    private void solutionAddExistingProjectToolStripMenuItem_Click( object sender, EventArgs e )
    {
      System.Windows.Forms.OpenFileDialog dlgTool = new OpenFileDialog();

      dlgTool.Filter = FilterString( Types.Constants.FILEFILTER_PROJECT );
      if ( dlgTool.ShowDialog() == DialogResult.OK )
      {
        if ( OpenProject( dlgTool.FileName ) != null )
        {
          m_CurrentProject.SetModified();
        }
      }
    }



    protected override void OnClosing( CancelEventArgs e )
    {
      base.OnClosing( e );
      if ( !e.Cancel )
      {
        RaiseApplicationEvent( new ApplicationEvent( Types.ApplicationEvent.Type.SHUTTING_DOWN ) );
      }
    }



    private void solutionCloseToolStripMenuItem_Click( object sender, EventArgs e )
    {
      CloseSolution();
    }



    private void fileCloseToolStripMenuItem_Click( object sender, EventArgs e )
    {
      if ( ActiveDocument != null )
      {
        ActiveDocument.Close();
      }
    }



    private void solutionAddNewProjectToolStripMenuItem_Click( object sender, EventArgs e )
    {
      AddNewProjectAndOrSolution();
    }



    private Project AddNewSolution()
    {
      var solWizard = new FormSolutionWizard( "New Solution", StudioCore.Settings );
      if ( solWizard.ShowDialog() != DialogResult.OK )
      {
        return null;
      }
      CloseSolution();
      if ( solWizard.CreateNewFolderForSolution )
      {
        try
        {
          System.IO.Directory.CreateDirectory( solWizard.SolutionPath );
        }
        catch ( System.Exception ex )
        {
          System.Windows.Forms.MessageBox.Show( "Could not create solution folder:" + System.Environment.NewLine + ex.Message, "Could not create solution folder" );
          return null;
        }
      }

      StudioCore.Navigating.Solution = new Solution( this );
      StudioCore.Navigating.Solution.Name = solWizard.SolutionName;
      StudioCore.Navigating.Solution.Filename = solWizard.SolutionFilename;

      try
      {
        System.IO.Directory.CreateDirectory( System.IO.Path.GetDirectoryName( solWizard.ProjectFilename ) );
      }
      catch ( System.Exception ex )
      {
        System.Windows.Forms.MessageBox.Show( "Could not create project folder:" + System.Environment.NewLine + ex.Message, "Could not create project folder" );
        return null;
      }

      Project newProject = new Project();
      newProject.Core = StudioCore;
      newProject.Settings.Name = System.IO.Path.GetFileNameWithoutExtension( solWizard.ProjectFilename );
      newProject.Settings.Filename = solWizard.ProjectFilename;
      newProject.Settings.BasePath = System.IO.Path.GetDirectoryName( newProject.Settings.Filename );
      newProject.Node = new DecentForms.TreeView.TreeNode();
      newProject.Node.Tag = new SolutionExplorer.TreeItemInfo() { Project = newProject };
      newProject.Node.Text = newProject.Settings.Name;

      Text += " - " + newProject.Settings.Name;

      StudioCore.Navigating.Solution.Projects.Add( newProject );

      if ( solWizard.CreateRepository )
      {
        global::SourceControl.Controller.CreateRepositoryInFolder( newProject.FullPath( "" ), out SourceControl.Controller controller );
        if ( SourceControl.Controller.IsFolderUnderSourceControl( newProject.FullPath( "" ) ) )
        {
          newProject.SourceControl = controller;
        }
      }

      m_SolutionExplorer.treeProject.Nodes.Add( newProject.Node );
      newProject.Node.Collapse();

      SetActiveProject( newProject );
      projectToolStripMenuItem.Visible = true;
      solutionToolStripMenuItemTop.Visible = true;

      SaveSolution();
      if ( SaveProject( newProject ) )
      {
        if ( solWizard.CreateRepository )
        {
          if ( SourceControl.Controller.IsFolderUnderSourceControl( newProject.FullPath( "" ) ) )
          {
            newProject.SourceControl.AddFileToRepository( System.IO.Path.GetFileName( newProject.Settings.Filename ) );
            newProject.SourceControl.AddFileToRepository( System.IO.Path.GetFileName( StudioCore.Navigating.Solution.Filename ) );
            newProject.SourceControl.StageAllChanges();
            newProject.SourceControl.CommitAllChanges( StudioCore.Settings.SourceControlInfo.CommitAuthor, StudioCore.Settings.SourceControlInfo.CommitAuthorEmail, "Initial" );
          }
        }
      }
      RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.SOLUTION_OPENED ) );
      RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.PROJECT_OPENED, newProject ) );

      UpdateUndoSettings();

      return newProject;
    }



    private Project AddNewProjectAndOrSolution()
    {
      if ( StudioCore.Navigating.Solution == null )
      {
        return AddNewSolution();
      }
      return AddNewProject( true );
    }



    public bool ImportImage( string Filename, GR.Image.FastImage IncomingImage, Types.GraphicType ImportType, ColorSettings MCSettings, int ItemWidth, int ItemHeight, out GR.Image.FastImage MappedImage, out ColorSettings NewMCSettings, out bool PasteAsBlock )
    {
      PasteAsBlock = false;

      // shortcut possible? (check if palette matches ours)
      if ( IncomingImage == null )
      {
        IncomingImage = StudioCore.Imaging.LoadImageFromFile( Filename );
      }

      MappedImage = null;
      if ( IncomingImage.PixelFormat == GR.Drawing.PixelFormat.Format8bppIndexed )
      {
        // match palette
        bool match = true;
        for ( int i = 0; i < 16; ++i )
        {
          if ( ( IncomingImage.PaletteRed( i ) != ( ( ConstantData.Palette.ColorValues[i] & 0xff0000 ) >> 16 ) )
          ||   ( IncomingImage.PaletteGreen( i ) != ( ( ConstantData.Palette.ColorValues[i] & 0xff00 ) >> 8 ) )
          ||   ( IncomingImage.PaletteBlue( i ) != ( ( ConstantData.Palette.ColorValues[i] & 0xff ) ) ) )
          {
            match = false;
            break;
          }
        }
        if ( match )
        {
          MappedImage = IncomingImage;
          NewMCSettings = MCSettings;
          return true;
        }
      }

      DlgGraphicImport importGFX = new DlgGraphicImport( StudioCore, ImportType, IncomingImage, Filename, MCSettings, ItemWidth, ItemHeight );
      if ( importGFX.ShowDialog() != DialogResult.OK )
      {
        IncomingImage.Dispose();
        NewMCSettings = MCSettings;
        return false;
      }
      PasteAsBlock = importGFX.PasteAsBlock;

      IncomingImage.Dispose();
      MappedImage = importGFX.ConvertedImage;
      NewMCSettings = importGFX.MultiColorSettings;
      return true;
    }



    private void licenseToolStripMenuItem_Click_1( object sender, EventArgs e )
    {
      FormLicense form = new FormLicense( StudioCore );

      form.ShowDialog();
    }



    private void DumpPanel( string Indent, DockPanel Panel )
    {
      BaseDocument dummy = m_SolutionExplorer;

      //Panel.SaveAsXml( @"d:\gnu.xml" );

      Debug.Log( Indent + "Container" );
      foreach ( var docs in Panel.Documents )
      {
        if ( docs is BaseDocument )
        {
          Debug.Log( Indent + "-document " + ( (BaseDocument)docs ).Text );
          if ( docs == Panel.ActiveDocument )
          {
            Debug.Log( Indent + " =Active" );
          }
        }
      }
      foreach ( var pane in Panel.Panes )
      {
        Debug.Log( Indent + "-Pane at " + pane.DockState + " is visible: " + pane.Visible + " at " + pane.Location.X + "," + pane.Location.Y );

        foreach ( BaseDocument content in pane.Contents )
        {
          if ( pane.DockState == content.DockState )
          {
            Debug.Log( Indent + " -" + content.Visible + " = " + content.Text );
            if ( content == pane.ActiveContent )
            {
              Debug.Log( Indent + " =Active" );
            }

            Form form = content;

            while ( ( form != null )
            && ( form != this ) )
            {
              form = form.ParentForm;
              if ( ( form != null )
              && ( form != this ) )
              {
                Debug.Log( "-parented by " + form.Text + "(" + form.ToString() + ") at " + form.Location );
              }
            }
          }
          else if ( !content.Visible )
          {
            Debug.Log( Indent + " -" + content.Visible + " = " + content.Text );
          }
        }
      }
    }



    private void dumpDockStateToolStripMenuItem_Click( object sender, EventArgs e )
    {
      System.IO.MemoryStream memOut = new System.IO.MemoryStream();

      panelMain.SaveAsXml( memOut, Encoding.UTF8 );

      GR.Memory.ByteBuffer data = new GR.Memory.ByteBuffer(memOut.ToArray());

      Debug.Log( data.ToString() );
      DumpPanel( "", panelMain );

      /*
      foreach ( var tool in Settings.Tools )
      {
        Form  form = tool.Value.Document;


        Debug.Log( "Tool " + tool.Key + " is visible:" + tool.Value.Document.Visible );
        Debug.Log( " at " + tool.Value.Document.DockState + " at " + tool.Value.Document.Location.X + "," + tool.Value.Document.Location.Y );
        while ( ( form != null )
        &&      ( form != this ) )
        {
          form = form.ParentForm;
          if ( form != null )
          {
            Debug.Log( "-parented by " + form.Text );
          }
        }
      }*/
    }



    public void CloseAllDocuments()
    {
      if ( panelMain.DocumentStyle == DocumentStyle.SystemMdi )
      {
        foreach ( Form form in MdiChildren )
        {
          form.Close();
        }
      }
      else
      {
        foreach ( IDockContent document in panelMain.DocumentsToArray() )
        {
          document.DockHandler.Close();
        }
      }
    }



    private void runTestsToolStripMenuItem_Click( object sender, EventArgs e )
    {
      UnitTests.TestManager manager = new RetroDevStudio.UnitTests.TestManager(this);

      manager.RunTests();
    }



    private void fileNewSolutionToolStripMenuItem_Click( object sender, EventArgs e )
    {
      AddNewSolution();
    }



    private void mainToolRebuild_Click( object sender, EventArgs e )
    {
      ApplyFunction( RetroDevStudio.Types.Function.REBUILD );
    }



    private void charsetScreenToolStripMenuItem_Click( object sender, EventArgs e )
    {
      AddNewDocumentOrElement( ProjectElement.ElementType.CHARACTER_SCREEN, "Character Screen", m_CurrentProject, ( m_CurrentProject != null ) ? m_CurrentProject.Node : null );
    }



    private void graphicScreenToolStripMenuItem_Click( object sender, EventArgs e )
    {
      AddNewDocumentOrElement( ProjectElement.ElementType.GRAPHIC_SCREEN, "Graphic Screen", m_CurrentProject, ( m_CurrentProject != null ) ? m_CurrentProject.Node : null );
    }



    private void mapToolStripMenuItem_Click( object sender, EventArgs e )
    {
      AddNewDocumentOrElement( ProjectElement.ElementType.MAP_EDITOR, "Map Project", m_CurrentProject, ( m_CurrentProject != null ) ? m_CurrentProject.Node : null );
    }



    private void solutionToolStripMenuItem1_Click( object sender, EventArgs e )
    {
      AddNewProjectAndOrSolution();
    }



    private void projectAddNewASMFileToolStripMenuItem_Click( object sender, EventArgs e )
    {
      AddNewElement( ProjectElement.ElementType.ASM_SOURCE, "ASM File", m_CurrentProject, ( m_CurrentProject != null ) ? m_CurrentProject.Node : null );
    }



    private void projectAddNewBASICFileToolStripMenuItem_Click( object sender, EventArgs e )
    {
      AddNewElement( ProjectElement.ElementType.BASIC_SOURCE, "BASIC File", m_CurrentProject, ( m_CurrentProject != null ) ? m_CurrentProject.Node : null );
    }



    private void projectAddNewSpriteSetToolStripMenuItem_Click( object sender, EventArgs e )
    {
      AddNewElement( ProjectElement.ElementType.SPRITE_SET, "Sprite Set", m_CurrentProject, ( m_CurrentProject != null ) ? m_CurrentProject.Node : null );
    }



    private void projectAddNewCharacterSetToolStripMenuItem_Click( object sender, EventArgs e )
    {
      AddNewElement( ProjectElement.ElementType.CHARACTER_SET, "Character Set", m_CurrentProject, ( m_CurrentProject != null ) ? m_CurrentProject.Node : null );
    }



    private void projectAddNewCharacterScreenToolStripMenuItem_Click( object sender, EventArgs e )
    {
      AddNewElement( ProjectElement.ElementType.CHARACTER_SCREEN, "Charset Screen", m_CurrentProject, ( m_CurrentProject != null ) ? m_CurrentProject.Node : null );
    }



    private void projectAddNewGraphicScreenToolStripMenuItem_Click( object sender, EventArgs e )
    {
      AddNewElement( ProjectElement.ElementType.GRAPHIC_SCREEN, "Graphic Screen", m_CurrentProject, ( m_CurrentProject != null ) ? m_CurrentProject.Node : null );
    }



    private void projectAddNewMapToolStripMenuItem_Click( object sender, EventArgs e )
    {
      AddNewElement( ProjectElement.ElementType.ASM_SOURCE, "ASM File", m_CurrentProject, ( m_CurrentProject != null ) ? m_CurrentProject.Node : null );
    }



    private void editToolStripMenuItem_DropDownOpening( object sender, EventArgs e )
    {
      UpdateUndoSettings();
    }



    private void cutToolStripMenuItem_Click( object sender, EventArgs e )
    {
      if ( ActiveDocument != null )
      {
        ActiveDocument.Cut();
        UpdateUndoSettings();
      }
    }



    private void copyToolStripMenuItem_Click( object sender, EventArgs e )
    {
      if ( ActiveDocument != null )
      {
        ActiveDocument.Copy();
        UpdateUndoSettings();
      }
    }



    private void pasteToolStripMenuItem_Click( object sender, EventArgs e )
    {
      if ( ActiveDocument != null )
      {
        ActiveDocument.Paste();
        UpdateUndoSettings();
      }
    }



    private void deleteToolStripMenuItem_Click( object sender, EventArgs e )
    {
      if ( ActiveDocument != null )
      {
        ActiveDocument.Delete();
        UpdateUndoSettings();
      }
    }



    private void mainToolToggleTrueDrive_Click( object sender, EventArgs e )
    {
      StudioCore.Settings.TrueDriveEnabled = !StudioCore.Settings.TrueDriveEnabled;
      if ( StudioCore.Settings.TrueDriveEnabled )
      {
        mainToolToggleTrueDrive.Image = Properties.Resources.toolbar_truedrive_enabled;
      }
      else
      {
        mainToolToggleTrueDrive.Image = Properties.Resources.toolbar_truedrive_disabled;
      }
    }



    private void mainToolEmulator_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( mainToolEmulator.SelectedIndex == -1 )
      {
        StudioCore.Settings.EmulatorToRun = "";
      }
      else
      {
        StudioCore.Settings.EmulatorToRun = ( (GR.Generic.Tupel<string, ToolInfo>)mainToolEmulator.SelectedItem ).first.ToUpper();
      }
    }



    private void mainToolOpenFile_Click( object sender, EventArgs e )
    {
      ApplyFunction( RetroDevStudio.Types.Function.OPEN_FILES );
    }



    private void mainToolCommentSelection_Click( object sender, EventArgs e )
    {
      ApplyFunction( RetroDevStudio.Types.Function.COMMENT_SELECTION );
    }



    private void mainToolUncommentSelection_Click( object sender, EventArgs e )
    {
      ApplyFunction( RetroDevStudio.Types.Function.UNCOMMENT_SELECTION );
    }



    private void throwExceptionToolStripMenuItem_Click( object sender, EventArgs e )
    {
      throw new Exception( "oh the noes" );
    }



    private void valueTableToolStripMenuItem_Click( object sender, EventArgs e )
    {
      AddNewDocumentOrElement( ProjectElement.ElementType.VALUE_TABLE, "Value Table", m_CurrentProject, ( m_CurrentProject != null ) ? m_CurrentProject.Node : null );
    }


    private void compileToolStripMenuItem_Click( object sender, EventArgs e )
    {
      ApplyFunction( RetroDevStudio.Types.Function.COMPILE );
    }


    private void buildToolStripMenuItem1_Click( object sender, EventArgs e )
    {
      ApplyFunction( RetroDevStudio.Types.Function.BUILD );
    }


    private void rebuildToolStripMenuItem_Click( object sender, EventArgs e )
    {
      ApplyFunction( RetroDevStudio.Types.Function.REBUILD );
    }


    private void buildandRunToolStripMenuItem_Click( object sender, EventArgs e )
    {
      ApplyFunction( RetroDevStudio.Types.Function.BUILD_AND_RUN );
    }


    private void debugToolStripMenuItem1_Click( object sender, EventArgs e )
    {
      ApplyFunction( RetroDevStudio.Types.Function.BUILD_AND_DEBUG );
    }



    private void preprocessedFileToolStripMenuItem_Click( object sender, EventArgs e )
    {
      ApplyFunction( RetroDevStudio.Types.Function.BUILD_TO_PREPROCESSED_FILE );
    }



    private void debugTestToolStripMenuItem_Click( object sender, EventArgs e )
    {
      // This is an example test that just downloads all 64KB of RAM quickly from the emulator and displays the round-trip time.
      var senderGUI = ( sender as ToolStripItem );
      Action<bool, string> showResult = null;
      showResult =
        ( isError, result ) =>
        {
          if ( InvokeRequired )
          {
            Invoke( showResult, isError, result );
          }
          else
          {
            MessageBox.Show( ( string.IsNullOrEmpty( result ) ? "No result" : result ), "Debug Test", MessageBoxButtons.OK, ( isError ? MessageBoxIcon.Exclamation : MessageBoxIcon.None ) );
            senderGUI.Enabled = true;
          }
        };

      senderGUI.Enabled = false;
      try
      {
        var debugger = StudioCore.Debugging.Debugger;
        if ( debugger.State == DebuggerState.NOT_CONNECTED )
        {
          showResult( true, string.Format( "Please first Debug|Connect to the emulator.{0}(using the remote monitor option)", Environment.NewLine ) );
        }
        else
        {
          var sw = System.Diagnostics.Stopwatch.StartNew();
          DebugEventHandler debugEventHandler = null;
          debugEventHandler =
            ( DebugEventData debugEventData ) =>
            {
              if ( debugEventData.Type == DebugEvent.UPDATE_WATCH )
              {
                sw.Stop();
                debugger.DebugEvent -= debugEventHandler;

                byte[] dataBytes = debugEventData.Data.Data();
                var dataString = BitConverter.ToString(dataBytes);
                if ( dataString.Length > 1023 )
                {
                  dataString = dataString.Substring( 0, 510 ) + "..." + dataString.Substring( ( dataString.Length - 510 ), 510 );
                }

                string message = string.Format( "Received all {1} bytes of RAM in {2:G} milliseconds :{0}{0}{3}", Environment.NewLine, debugEventData.Data.Length.ToString(), sw.Elapsed.TotalMilliseconds, dataString );
                showResult( false, message );
              }
            };

          debugger.DebugEvent += debugEventHandler;
          debugger.RefreshMemory( 0, 65536, MemorySource.AS_CPU );
        }
      }
      catch ( Exception exception )
      {
        Debug.Log( exception.ToString() );
        showResult( true, exception.Message );
      }
    }



    private void memoryViewToolStripMenuItem_Click( object sender, EventArgs e )
    {
      var document = new DebugMemory( StudioCore );
      document.ShowHint = DockState.Float;
      document.Core = StudioCore;
      document.Text = "Memory View";
      document.LoadDocument();
      document.Show( panelMain );
    }



    public void WriteToLog( string Info )
    {
      //System.IO.File.AppendAllText( "testlog.txt", Info + System.Environment.NewLine );
    }




    private static int WM_QUERYENDSESSION = 0x11;
    private static int WM_ENDSESSION = 0x16;



    protected override void WndProc( ref System.Windows.Forms.Message m )
    {
      if ( m.Msg == WM_QUERYENDSESSION )
      {
        s_SystemShutdown = true;
      }
      else if ( m.Msg == WM_ENDSESSION )
      {
        if ( m.WParam != IntPtr.Zero )
        {
          // system shut down
          OnSystemShutDown();
        }
        s_SystemShutdown = false;
      }

      // If this is WM_QUERYENDSESSION, the closing event should be  
      // raised in the base WndProc.  
      base.WndProc( ref m );
    }



    private void OnSystemShutDown()
    {
      var chunkRestartData = new GR.IO.FileChunk( FileChunkConstants.RESTART_INFO );

      var chunkRestartInfo = new GR.IO.FileChunk( FileChunkConstants.RESTART_DATA );
      // version
      chunkRestartInfo.AppendI32( 1 );
      if ( StudioCore.Navigating.Solution != null )
      {
        chunkRestartInfo.AppendString( StudioCore.Navigating.Solution.Filename );
        if ( StudioCore.Navigating.Solution.Modified )
        {
          var solutionData = StudioCore.Navigating.Solution.ToBuffer();
          chunkRestartInfo.AppendU32( solutionData.Length );
          chunkRestartInfo.Append( solutionData );
        }
      }
      else
      {
        chunkRestartInfo.AppendString( "" );
        chunkRestartInfo.AppendU32( 0 );
      }

      chunkRestartData.Append( chunkRestartInfo.ToBuffer() );

      foreach ( BaseDocument doc in panelMain.Documents )
      {
        if ( doc.Modified )
        {
          var chunkDocInfo = new GR.IO.FileChunk( FileChunkConstants.RESTART_DOC_INFO );

          chunkDocInfo.AppendString( doc.DocumentInfo.FullPath );
          if ( doc.DocumentInfo.Project != null )
          {
            chunkDocInfo.AppendString( doc.DocumentInfo.Project.Settings.Name );
          }
          else
          {
            chunkDocInfo.AppendString( "" );
          }

          var docData = doc.SaveToBuffer();
          chunkDocInfo.AppendU32( docData.Length );
          chunkDocInfo.Append( docData );

          chunkRestartData.Append( chunkDocInfo.ToBuffer() );
        }
      }

      string    restartDataPath = GR.Path.RenameFile( SettingsPath(), "restart.dat" );
      GR.IO.File.WriteAllBytes( restartDataPath, chunkRestartData.ToBuffer() );
    }



    private void systemShutdownToolStripMenuItem_Click( object sender, EventArgs e )
    {
      s_SystemShutdown = true;
      OnSystemShutDown();
      Close();
    }



    private bool HandleRestart()
    {
      string    restartDataPath = GR.Path.RenameFile( SettingsPath(), "restart.dat" );

      if ( !System.IO.File.Exists( restartDataPath ) )
      {
        return false;
      }

      var restartData = GR.IO.File.ReadAllBytes( restartDataPath );
      if ( restartData == null )
      {
        return false;
      }

      try
      {
        System.IO.File.Delete( restartDataPath );
      }
      catch ( Exception )
      {
        // so what?
      }

      var chunk = new GR.IO.FileChunk();
      var memIn = restartData.MemoryReader();

      if ( !chunk.ReadFromStream( memIn ) )
      {
        return false;
      }
      if ( chunk.Type != FileChunkConstants.RESTART_INFO )
      {
        return false;
      }

      var chunkReader = chunk.MemoryReader();
      var subChunk = new GR.IO.FileChunk();

      while ( subChunk.ReadFromStream( chunkReader ) )
      {
        var subChunkReader = subChunk.MemoryReader();
        switch ( subChunk.Type )
        {
          case FileChunkConstants.RESTART_DATA:
            {
              uint      version = subChunkReader.ReadUInt32();
              if ( version != 1 )
              {
                return false;
              }
              string    solutionFile = subChunkReader.ReadString();
              if ( !string.IsNullOrEmpty( solutionFile ) )
              {
                if ( !OpenSolution( solutionFile ) )
                {
                  return false;
                }
                uint  dataSize = subChunkReader.ReadUInt32();
                var   solutionData = new GR.Memory.ByteBuffer( dataSize );

                if ( subChunkReader.ReadBlock( solutionData, dataSize ) != dataSize )
                {
                  return false;
                }
                if ( !StudioCore.Navigating.Solution.FromBuffer( solutionData, solutionFile ) )
                {
                  return false;
                }
                StudioCore.Navigating.Solution.Modified = true;
              }
            }
            break;
          case FileChunkConstants.RESTART_DOC_INFO:
            {
              string    docPath = subChunkReader.ReadString();
              string    docProjectName = subChunkReader.ReadString();
              uint      docDataSize = subChunkReader.ReadUInt32();
              var       docData = new GR.Memory.ByteBuffer();

              if ( subChunkReader.ReadBlock( docData, docDataSize ) != docDataSize )
              {
                return false;
              }

              // quick hack, do not restore in built editors
              if ( !string.IsNullOrEmpty( docPath ) )
              {
                BaseDocument    doc = null;
                Project         project = null;
                if ( StudioCore.Navigating.Solution != null )
                {
                  project = StudioCore.Navigating.Solution.GetProjectByName( docProjectName );
                }
                if ( project == null )
                {
                  doc = OpenFile( docPath );
                }
                else
                {
                  doc = project.ShowDocument( project.GetElementByFilename( docPath ) );
                }
                if ( doc != null )
                {
                  if ( !doc.ReadFromReader( docData.MemoryReader() ) )
                  {
                    return false;
                  }
                  doc.SetModified();
                }
              }
            }
            break;
        }
      }

      return true;
    }



    private void checkForUpdateToolStripMenuItem_Click( object sender, EventArgs e )
    {
      CheckForUpdate();
    }



    private void btnNavigatePrevious_Click( object sender, EventArgs e )
    {
      StudioCore.Navigating.NavigateBack();
    }



    private void btnNavigateForward_Click( object sender, EventArgs e )
    {
      StudioCore.Navigating.NavigateForward();
    }



    private void navigateBackwardToolStripMenuItem_Click( object sender, EventArgs e )
    {
      StudioCore.Navigating.NavigateBack();
    }



    private void navigateForwardToolStripMenuItem_Click( object sender, EventArgs e )
    {
      StudioCore.Navigating.NavigateForward();
    }



    private void solutionCloneToolStripMenuItem_Click( object sender, EventArgs e )
    {
      if ( StudioCore.Navigating.Solution != null )
      {
        CloneSolution( StudioCore.Navigating.Solution );
      }
    }



    private void relocationFileToolStripMenuItem_Click( object sender, EventArgs e )
    {
      ApplyFunction( RetroDevStudio.Types.Function.BUILD_TO_RELOCATION_FILE );
    }



    private void solutionRenameToolStripMenuItem_Click( object sender, EventArgs e )
    {
      if ( StudioCore.Navigating.Solution == null )
      {
        return;
      }

      var formRename = new FormRenameSolution( StudioCore, StudioCore.Navigating.Solution.Name );

      formRename.ShowDialog();
    }



    private void preferencesToolStripMenuItem_Click( object sender, EventArgs e )
    {
      var prefDlg = new FormPreferences( StudioCore );

      prefDlg.ShowDialog();
    }



    private void solutionSaveToolStripMenuItem_Click( object sender, EventArgs e )
    {
      SaveSolution();
    }



    private void dumpSourceInfoToolStripMenuItem_Click( object sender, EventArgs e )
    {
      if ( ( ActiveDocumentInfo != null )
      &&   ( ActiveDocumentInfo.ASMFileInfo != null ) )
      {
        foreach ( KeyValuePair<int, Types.ASM.SourceInfo> pair in ActiveDocumentInfo.ASMFileInfo.SourceInfo )
        {
          Debug.Log( "From line " + ( pair.Value.GlobalStartLine + 1 ) + " to " + ( pair.Value.GlobalStartLine + pair.Value.LineCount - 1 + 1 ) + ", local " + ( pair.Value.LocalStartLine + 1 ) + ", " + pair.Value.LineCount + " lines from " + pair.Value.Filename );
        }
      }
    }



    private void printToolStripMenuItem_Click( object sender, EventArgs e )
    {
      ApplyFunction( RetroDevStudio.Types.Function.PRINT );
    }



  }
}
