using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using WeifenLuo.WinFormsUI.Docking;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using C64Studio.IdleQueue;
using C64Studio.Types;
using C64Studio.Displayer;
using C64Studio.CustomRenderer;

// 0.9f - added else for !ifdef macro
// 0.9b - fixed crash bug if opening project with modified active project


namespace C64Studio
{
  public partial class MainForm : Form
  {
    private Project               m_CurrentProject = null;

    public Solution               m_Solution = null;

    public OutputDisplay          m_Output = new OutputDisplay();

    public SolutionExplorer       m_SolutionExplorer = new SolutionExplorer();

    public BinaryDisplay          m_BinaryEditor = null;
    public DebugRegisters         m_DebugRegisters = new DebugRegisters();
    public DebugWatch             m_DebugWatch = new DebugWatch();
    public DebugMemory            m_DebugMemory = null;
    public DebugBreakpoints       m_DebugBreakpoints = new DebugBreakpoints();
    public CompileResult          m_CompileResult = new CompileResult();
    public CharsetEditor          m_CharsetEditor = null;
    public Disassembler           m_Disassembler = null;
    public CharsetScreenEditor    m_CharScreenEditor = null;
    public GraphicScreenEditor    m_GraphicScreenEditor = null;
    public SpriteEditor           m_SpriteEditor = null;
    public MapEditor              m_MapEditor = null;
    public Calculator             m_Calculator = null;
    public PetSCIITable           m_PetSCIITable = null;
    public Outline                m_Outline = new Outline();
    public ValueTableEditor       m_ValueTableEditor = null;
    public Help                   m_Help = new Help();
    public FormFindReplace        m_FindReplace = null;

    public SearchResults          m_SearchResults = new SearchResults();
    public Perspective            m_ActivePerspective = Perspective.DEBUG;

    public System.Diagnostics.Process CompilerProcess = null;
    private System.Diagnostics.Process m_ExternalProcess = null;

    public StudioCore             StudioCore = new StudioCore();

    private List<Tasks.Task>      m_Tasks = new List<C64Studio.Tasks.Task>();
    private Tasks.Task            m_CurrentTask = null;

    private System.DateTime       m_LastReceivedOutputTime;

    private bool                  m_ChangingToolWindows = false;
    private bool                  m_LoadingProject = false;

    private BaseDocument          m_ActiveSource = null;
    internal ToolInfo             m_CurrentActiveTool = null;
    public SortedDictionary<string, Types.Palette> Palettes = new SortedDictionary<string, C64Studio.Types.Palette>();

    private static MainForm s_MainForm = null;
    private List<IdleRequest> IdleQueue = new List<IdleRequest>();


    public delegate void ApplicationEventHandler( Types.ApplicationEvent Event );

    public event ApplicationEventHandler ApplicationEvent;

    private GR.Collections.Set<BaseDocument> m_ExternallyChangedDocuments = new GR.Collections.Set<BaseDocument>();

    public System.Drawing.Text.PrivateFontCollection m_FontC64 = new System.Drawing.Text.PrivateFontCollection();



    delegate void AddToOutputAndShowCallback( string Text );
    delegate void SetGUIForWaitOnExternalToolCallback( bool Wait );
    delegate void SetDebuggerValuesCallback( RegisterInfo RegisterValues );
    delegate void StartDebugAtCallback( DocumentInfo DocumentToDebug, DocumentInfo DocumentToRun, int DebugAddress );
    public delegate void ParameterLessCallback();
    delegate void UpdateWatchInfoCallback( VICERemoteDebugger.RequestData Request, GR.Memory.ByteBuffer Data );
    delegate bool ParseFileCallback( Parser.ParserBase Parser, DocumentInfo Document, ProjectConfig Configuration );
    public delegate void DocCallback( BaseDocument Document );
    delegate void DocumentEventHandlerCallback( BaseDocument.DocEvent Event );
    delegate void NotifyAllDocumentsCallback( bool CanToggleBreakpoints );
    delegate void TaskFinishedCallback( C64Studio.Tasks.Task FinishedTask );
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
      /*
      Tiny64.Machine    machine = new Tiny64.Machine();

      GR.Image.MemoryImage    img = new GR.Image.MemoryImage( 320, 200, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
      CustomRenderer.PaletteManager.ApplyPalette( img );

      while ( true )
      {
        // round about one frame
        int     numCycles = 19656;

        machine.RunCycles( numCycles );

        //Debug.Log( machine.CPU.PC.ToString( "X4" ) + ":" + opCode.ToString( "X2" ) + " A:" + CPU.Accu.ToString( "X2" ) + " X:" + CPU.X.ToString( "X2" ) + " Y:" + CPU.Y.ToString( "X2" ) + " " + ( Memory.RAM[0xc1] + ( Memory.RAM[0xc2] << 8 ) ).ToString( "X4" ) );
        //Debug.Log( machine.CPU.PC.ToString( "X4" ) + ": A:" + machine.CPU.Accu.ToString( "X2" ) + " X:" + machine.CPU.X.ToString( "X2" ) + " Y:" + machine.CPU.Y.ToString( "X2" ) + " " + ( machine.Memory.RAM[0xc1] + ( machine.Memory.RAM[0xc2] << 8 ) ).ToString( "X4" ) );

        // render image
        bool  vicActive = ( ( machine.Memory.VIC.ReadByte( 0x11 ) & 0x10 ) != 0 );
        if ( vicActive )
        {
          int   vicBank = ( machine.Memory.CIA2.ReadByte( 0 ) & 0x03 ) ^ 0x03;
          int   screenPos = ( ( machine.Memory.VIC.ReadByte( 0x18 ) & 0xf0 ) >> 4 ) * 1024 + vicBank * 16384;
          int   localCharDataPos = ( machine.Memory.VIC.ReadByte( 0x18 ) & 0x0e ) * 1024;
          int   charDataPos = localCharDataPos + vicBank * 16384;
          byte  bgColor = (byte)( machine.Memory.VIC.ReadByte( 0x21 ) & 0x0f );

          GR.Memory.ByteBuffer    charData = null;
          if ( ( ( vicBank == 0 )
          ||     ( vicBank == 2 ) )
          &&   ( localCharDataPos == 0x1000 ) )
          {
            // use default upper case chars
            charData = Types.ConstantData.UpperCaseCharset;
          }
          else if ( ( ( vicBank == 0 )
          ||          ( vicBank == 2 ) )
          &&        ( localCharDataPos == 0x2000 ) )
          {
            // use default lower case chars
            charData = Types.ConstantData.LowerCaseCharset;
          }
          else
          {
            // use RAM
            charData = new GR.Memory.ByteBuffer( machine.Memory.RAM );
            charData = charData.SubBuffer( charDataPos, 2048 );
          }
          for ( int y = 0; y < 25; ++y )
          {
            for ( int x = 0; x < 40; ++x )
            {
              byte    charIndex = machine.Memory.RAM[screenPos + x + y * 40];
              byte    charColor = machine.Memory.ColorRAM[x + y * 40];

              CharacterDisplayer.DisplayHiResChar( charData.SubBuffer( charIndex * 8, 8 ), bgColor, charColor, img, x * 8, y * 8 );
            }
          }
          DataObject dataObj = new DataObject();

          GR.Memory.ByteBuffer      dibData = img.CreateHDIBAsBuffer();

          System.IO.MemoryStream    ms = dibData.MemoryStream();

          // WTF - SetData requires streams, NOT global data (HGLOBAL)
          dataObj.SetData( "DeviceIndependentBitmap", ms );
          Clipboard.SetDataObject( dataObj, true );
        }
      }
       */

      s_MainForm = this;

      //m_FontC64.AddFontFile( @"D:\privat\projekte\C64Studio\C64Studio\C64_Pro_Mono_v1.0-STYLE.ttf" );

      try
      {
        string basePath = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
        if ( basePath.ToUpper().StartsWith( "FILE:///" ) )
        {
          basePath = basePath.Substring( 8 );
        }
        string fontPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(basePath), @"C64_Pro_Mono_v1.0-STYLE.ttf");


        m_FontC64.AddFontFile( fontPath ); // @"C64_Pro_Mono_v1.0-STYLE.ttf" );
      }
      catch ( Exception ex )
      {
        MessageBox.Show( "C64Studio can't find the C64 true type font file C64_Pro_Mono_v1.0-STYLE.ttf.\r\nMake sure it's in the path of C64Studio.exe.\r\n\r\n" + ex.Message, "Can't load font" );
        return;
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
      //ToolStripManager.Renderer = new C64Studio.CustomRenderer.LightToolStripRenderer();

      InitializeComponent();

      Application.Idle += new EventHandler( Application_Idle );

      panelMain.ShowDocumentIcon = true;

#if DEBUG
      debugToolStripMenuItem.Visible = true;
#else
      debugToolStripMenuItem.Visible = false;
#endif


      statusProgress.Visible = false;

      StudioCore.MainForm = this;
      StudioCore.Settings.PanelMain = panelMain;
      StudioCore.Settings.Main = this;

      //Parser.BasicFileParser.KeyMap = StudioCore.Settings.BASICKeyMap;

      Types.Palette defaultPalette = new C64Studio.Types.Palette();
      defaultPalette.Name = "C64Studio";

      Palettes.Add( defaultPalette.Name, defaultPalette );

      StudioCore.Debugging.Debugger = new VICERemoteDebugger( StudioCore );
      StudioCore.Debugging.Debugger.DebugEvent += Debugger_DebugEvent;

      m_BinaryEditor        = new BinaryDisplay( StudioCore, new GR.Memory.ByteBuffer( 2 ), true, false );
      m_CharsetEditor       = new CharsetEditor( StudioCore );
      m_SpriteEditor        = new SpriteEditor( StudioCore );
      m_GraphicScreenEditor = new GraphicScreenEditor( StudioCore );
      m_CharScreenEditor    = new CharsetScreenEditor( StudioCore );
      m_PetSCIITable        = new PetSCIITable( StudioCore );
      m_Calculator          = new Calculator();
      m_MapEditor           = new MapEditor( StudioCore );
      m_Disassembler        = new Disassembler( StudioCore );
      m_DebugMemory         = new DebugMemory( StudioCore );
      m_ValueTableEditor    = new ValueTableEditor( StudioCore );
      m_FindReplace         = new FormFindReplace( StudioCore );

      m_BinaryEditor.SetInternal();
      m_CharsetEditor.SetInternal();
      m_SpriteEditor.SetInternal();
      m_GraphicScreenEditor.SetInternal();
      m_CharScreenEditor.SetInternal();
      m_MapEditor.SetInternal();
      m_Disassembler.SetInternal();
      m_ValueTableEditor.SetInternal();

      // build default panes
      AddToolWindow( ToolWindowType.OUTLINE, m_Outline, DockState.DockRight, outlineToolStripMenuItem, true, true );
      AddToolWindow( ToolWindowType.BINARY_EDITOR, m_BinaryEditor, DockState.Document, binaryEditorToolStripMenuItem, false, false );
      AddToolWindow( ToolWindowType.SOLUTION_EXPLORER, m_SolutionExplorer, DockState.DockRight, projectExplorerToolStripMenuItem, true, true );
      AddToolWindow( ToolWindowType.OUTPUT, m_Output, DockState.DockBottom, outputToolStripMenuItem, true, true );
      AddToolWindow( ToolWindowType.COMPILE_RESULT, m_CompileResult, DockState.DockBottom, compileResulttoolStripMenuItem, true, true );
      AddToolWindow( ToolWindowType.DEBUG_REGISTERS, m_DebugRegisters, DockState.DockRight, debugRegistersToolStripMenuItem, false, true );
      AddToolWindow( ToolWindowType.DEBUG_WATCH, m_DebugWatch, DockState.DockBottom, debugWatchToolStripMenuItem, false, true );
      AddToolWindow( ToolWindowType.DEBUG_MEMORY, m_DebugMemory, DockState.DockRight, debugMemoryToolStripMenuItem, false, true );
      m_DebugMemory.ViewScrolled += new DebugMemory.DebugMemoryEventCallback( m_DebugMemory_ViewScrolled );
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

      var viceDebugger = StudioCore.Debugging.Debugger as VICERemoteDebugger;
      viceDebugger.DocumentEvent += new BaseDocument.DocumentEventHandler( Document_DocumentEvent );

      StudioCore.Settings.GenericTools["Outline"]             = m_Outline;
      StudioCore.Settings.GenericTools["SolutionExplorer"]    = m_SolutionExplorer;
      StudioCore.Settings.GenericTools["Output"]              = m_Output;
      StudioCore.Settings.GenericTools["CompileResult"]       = m_CompileResult;
      StudioCore.Settings.GenericTools["DebugRegisters"]      = m_DebugRegisters;
      StudioCore.Settings.GenericTools["DebugWatch"]          = m_DebugWatch;
      StudioCore.Settings.GenericTools["DebugMemory"]         = m_DebugMemory;
      StudioCore.Settings.GenericTools["DebugBreakpoints"]    = m_DebugBreakpoints;
      StudioCore.Settings.GenericTools["Disassembler"]        = m_Disassembler;
      StudioCore.Settings.GenericTools["CharsetEditor"]       = m_CharsetEditor;
      StudioCore.Settings.GenericTools["SpriteEditor"]        = m_SpriteEditor;
      StudioCore.Settings.GenericTools["CharScreenEditor"]    = m_CharScreenEditor;
      StudioCore.Settings.GenericTools["GraphicScreenEditor"] = m_GraphicScreenEditor;
      StudioCore.Settings.GenericTools["MapEditor"]           = m_MapEditor;
      StudioCore.Settings.GenericTools["PetSCIITable"]        = m_PetSCIITable;
      StudioCore.Settings.GenericTools["Calculator"]          = m_Calculator;
      StudioCore.Settings.GenericTools["Help"]                = m_Help;
      StudioCore.Settings.GenericTools["FindReplace"]         = m_FindReplace;
      StudioCore.Settings.GenericTools["SearchResults"]       = m_SearchResults;


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

      m_DebugMemory.hexView.TextFont = new System.Drawing.Font( m_FontC64.Families[0], 9, System.Drawing.GraphicsUnit.Pixel );
      m_DebugMemory.hexView.ByteCharConverter = new C64Studio.Converter.PETSCIIToCharConverter();

      // auto-set app mode by checking for existing settings files
      DetermineSettingsPath();

      if ( !LoadSettings() )
      {
        // that means either an error occurred or no settings file has been found (first startup?)
        // no settings file found, ask user which mode is wanted
        var form = new FormAppMode(StudioCore);
        form.ShowDialog();

        if ( StudioCore.Settings.BASICKeyMap.DefaultKeymaps.ContainsKey( (uint)System.Windows.Forms.InputLanguage.CurrentInputLanguage.Culture.LCID ) )
        {
          StudioCore.Settings.BASICKeyMap.Keymap = StudioCore.Settings.BASICKeyMap.DefaultKeymaps[(uint)System.Windows.Forms.InputLanguage.CurrentInputLanguage.Culture.LCID];
        }
        else
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

      ApplyMenuShortCuts();

      StudioCore.Compiling.ParserBasic.Settings.StripSpaces = StudioCore.Settings.BASICStripSpaces;
      m_Outline.checkShowLocalLabels.Image = StudioCore.Settings.OutlineShowLocalLabels ? C64Studio.Properties.Resources.flag_green_on.ToBitmap() : C64Studio.Properties.Resources.flag_green_off.ToBitmap();
      m_Outline.checkShowShortCutLabels.Image = StudioCore.Settings.OutlineShowShortCutLabels ? C64Studio.Properties.Resources.flag_blue_on.ToBitmap() : C64Studio.Properties.Resources.flag_blue_off.ToBitmap();

      EmulatorListUpdated();

      if ( StudioCore.Settings.TrueDriveEnabled )
      {
        mainToolToggleTrueDrive.Image = Properties.Resources.toolbar_truedrive_enabled;
      }
      else
      {
        //mainToolToggleTrueDrive.Image = Properties.Resources.toolbar_truedrive_disabled;
      }

      // place all toolbars
      SetToolPerspective( Perspective.EDIT );

      if ( StudioCore.Settings.MainWindowPlacement != "" )
      {
        GR.Forms.WindowStateManager.GeometryFromString( StudioCore.Settings.MainWindowPlacement, this );
      }

      /*
      foreach ( Types.ColorableElement syntax in Enum.GetValues( typeof( Types.ColorableElement ) ) )
      {
        if ( StudioCore.Settings.SyntaxColoring[syntax] == null )
        {
          switch ( syntax )
          {
            case C64Studio.Types.ColorableElement.NONE:
            case C64Studio.Types.ColorableElement.LABEL:
              // dark red
              StudioCore.Settings.SyntaxColoring[syntax] = new C64Studio.Types.ColorSetting( GR.EnumHelper.GetDescription( syntax ) );
              StudioCore.Settings.SyntaxColoring[syntax].FGColor = 0xff800000;
              break;
            case C64Studio.Types.ColorableElement.CURRENT_DEBUG_LINE:
              // yellow background
              StudioCore.Settings.SyntaxColoring[syntax] = new C64Studio.Types.ColorSetting( GR.EnumHelper.GetDescription( syntax ) );
              StudioCore.Settings.SyntaxColoring[syntax].BGColor = 0xffffff00;
              StudioCore.Settings.SyntaxColoring[syntax].BGColorAuto = false;
              break;
            case C64Studio.Types.ColorableElement.LITERAL_NUMBER:
            case C64Studio.Types.ColorableElement.OPERATOR:
            case C64Studio.Types.ColorableElement.LITERAL_STRING:
              // blue on background
              StudioCore.Settings.SyntaxColoring[syntax] = new C64Studio.Types.ColorSetting( GR.EnumHelper.GetDescription( syntax ) );
              StudioCore.Settings.SyntaxColoring[syntax].FGColor = 0xff0000ff;
              break;
            case C64Studio.Types.ColorableElement.COMMENT:
              // dark green on background
              StudioCore.Settings.SyntaxColoring[syntax] = new C64Studio.Types.ColorSetting( GR.EnumHelper.GetDescription( syntax ) );
              StudioCore.Settings.SyntaxColoring[syntax].FGColor = 0xff008000;
              break;
            case Types.ColorableElement.ERROR_UNDERLINE:
              // only forecolor needed, red
              StudioCore.Settings.SyntaxColoring[syntax] = new Types.ColorSetting( GR.EnumHelper.GetDescription( syntax ) );
              StudioCore.Settings.SyntaxColoring[syntax].FGColor = 0xffff0000;
              break;
          }
        }
        if ( StudioCore.Settings.SyntaxColoring[syntax] == null )
        {
          StudioCore.Settings.SyntaxColoring[syntax] = new C64Studio.Types.ColorSetting( GR.EnumHelper.GetDescription( syntax ) );
        }
      }*/
      m_FindReplace.Fill( StudioCore.Settings );

      panelMain.ActiveContentChanged += new EventHandler( panelMain_ActiveContentChanged );
      panelMain.ActiveDocumentChanged += new EventHandler( panelMain_ActiveDocumentChanged );
      StudioCore.Settings.ReadMRUFromRegistry();
      UpdateMenuMRU();
      UpdateUndoSettings();

      mainTools.Visible = StudioCore.Settings.ToolbarActiveMain;
      debugTools.Visible = StudioCore.Settings.ToolbarActiveDebugger;

      m_FindReplace.RefreshDisplayOptions();
      StudioCore.Settings.RefreshDisplayOnAllDocuments( StudioCore );

      SetGUIForWaitOnExternalTool( false );

      projectToolStripMenuItem.Visible = false;

      panelMain.AllowDrop = true;
      panelMain.DragEnter += new DragEventHandler( MainForm_DragEnter );
      panelMain.DragDrop += new DragEventHandler( MainForm_DragDrop );

      m_Disassembler.RefreshDisplayOptions();

#if DEBUG
      showLineinfosToolStripMenuItem.Visible = true;
#endif


      //DumpPanes( panelMain, "" );

      ApplicationEvent += new ApplicationEventHandler( MainForm_ApplicationEvent );

      CheckForUpdate();

      if ( args.Length > 0 )
      {
        OpenFile( args[0] );
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
    }



    internal void RefreshDisplayOnAllDocuments()
    {
      var bgColor = GR.Color.Helper.FromARGB( StudioCore.Settings.BGColor( ColorableElement.BACKGROUND_CONTROL ) );
      var fgColor = GR.Color.Helper.FromARGB( StudioCore.Settings.FGColor( ColorableElement.BACKGROUND_CONTROL ) );

      MainMenuStrip.BackColor = bgColor;
      MainMenuStrip.ForeColor = fgColor;

      mainTools.BackColor = bgColor;
      mainTools.ForeColor = fgColor;
      debugTools.BackColor = bgColor;
      debugTools.ForeColor = fgColor;

      mainStatus.BackColor = bgColor;
      mainStatus.ForeColor = fgColor;

      mainMenu.Renderer = new ToolStripSeparatorRenderer( StudioCore );

      StudioCore.Theming.ApplyThemeToToolStripItems( mainTools.Items );
      StudioCore.Theming.ApplyThemeToToolStripItems( debugTools.Items );

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
      // TODO - date check!
      if ( StudioCore.Settings.CheckForUpdates )
      {
        AddTask( new Tasks.TaskCheckForUpdate() );
      }

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



    void Debugger_DebugEvent( DebugEventData Event )
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
          UpdateWatchInfo( Event.Request, Event.Data );
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
          StudioCore.Debugging.Debugger.SetAutoRefreshMemory( request.DebugRequest.Parameter1,
                                                       request.DebugRequest.Parameter2,
                                                       ( request.DebugRequest.Type == VICERemoteDebugger.Request.REFRESH_MEMORY_RAM ) ? MemorySource.RAM : MemorySource.AS_CPU );

          StudioCore.Debugging.Debugger.RefreshMemory( request.DebugRequest.Parameter1,
                                                       request.DebugRequest.Parameter2,
                                                       ( request.DebugRequest.Type == VICERemoteDebugger.Request.REFRESH_MEMORY_RAM ) ? MemorySource.RAM : MemorySource.AS_CPU );
        }
        else if ( request.OpenLastSolution != null )
        {
          OpenFile( request.OpenLastSolution );
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
      foreach ( var tool in StudioCore.Settings.ToolInfos )
      {
        if ( tool.Type == ToolInfo.ToolType.EMULATOR )
        {
          int itemIndex = mainToolEmulator.Items.Add(new GR.Generic.Tupel<string, ToolInfo>(tool.Name, tool));
          if ( ( tool.Name.ToUpper() == StudioCore.Settings.EmulatorToRun )
          || ( oldTool == tool ) )
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



    public Types.Palette ActivePalette
    {
      get
      {
        // TODO - aus Palette-Liste
        return Types.ConstantData.Palette;
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

        if ( m_Solution != null )
        {
          foreach ( var project in m_Solution.Projects )
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



    void MainForm_ApplicationEvent( C64Studio.Types.ApplicationEvent Event )
    {
      switch ( Event.EventType )
      {
        case Types.ApplicationEvent.Type.KEY_BINDINGS_MODIFIED:
          ApplyMenuShortCuts();
          break;
        case C64Studio.Types.ApplicationEvent.Type.EMULATOR_LIST_CHANGED:
          EmulatorListUpdated();
          break;
        case C64Studio.Types.ApplicationEvent.Type.DOCUMENT_ACTIVATED:
          if ( Event.Doc == null )
          {
            mainToolPrint.Enabled = false;
          }
          else
          {
            mainToolPrint.Enabled = Event.Doc.ContainsCode;
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
        case C64Studio.Types.ApplicationEvent.Type.SOLUTION_OPENED:
          solutionToolStripMenuItem.Visible = true;
          solutionCloseToolStripMenuItem.Enabled = true;
          solutionSaveToolStripMenuItem1.Enabled = true;
          UpdateCaption();
          break;
        case C64Studio.Types.ApplicationEvent.Type.SOLUTION_CLOSED:
          solutionToolStripMenuItem.Visible = false;
          solutionCloseToolStripMenuItem.Enabled = false;
          solutionSaveToolStripMenuItem1.Enabled = false;

          m_Output.SetText( "" );
          m_CompileResult.ClearMessages();
          UpdateCaption();
          break;
        case C64Studio.Types.ApplicationEvent.Type.ACTIVE_PROJECT_CHANGED:
          m_DebugWatch.DebuggedProject = m_CurrentProject;
          m_DebugRegisters.DebuggedProject = m_CurrentProject;
          m_DebugMemory.DebuggedProject = m_CurrentProject;
          m_DebugBreakpoints.DebuggedProject = m_CurrentProject;
          UpdateCaption();
          break;
      }
    }



    void panelMain_ActiveDocumentChanged( object sender, EventArgs e )
    {
      var docInfo = ActiveDocumentInfo;
      RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.DOCUMENT_ACTIVATED, docInfo ) );

      mainToolFind.Enabled = ( docInfo != null ) ? docInfo.Compilable : false;
      mainToolFindReplace.Enabled = mainToolFind.Enabled;
      mainToolCommentSelection.Enabled = ( docInfo != null ) ? docInfo.Compilable : false;
      mainToolUncommentSelection.Enabled = ( docInfo != null ) ? docInfo.Compilable : false;

      if ( ActiveDocument == null )
      {
        saveToolStripMenuItem.Enabled = false;
        saveAsToolStripMenuItem.Enabled = false;
        mainToolSave.Enabled = false;
        fileCloseToolStripMenuItem.Enabled = false;
      }
      else
      {
        saveToolStripMenuItem.Enabled = ActiveDocument.Modified;
        saveAsToolStripMenuItem.Enabled = true;
        mainToolSave.Enabled = ActiveDocument.Modified;
        fileCloseToolStripMenuItem.Enabled = true;
      }
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

        if ( baseDoc.DocumentInfo.ContainsCode )
        {
          if ( m_ActiveSource != baseDoc )
          {
            m_ActiveSource = baseDoc;
            //Debug.Log( "m_Outline.RefreshFromDocument after active content change" );
            AddTask( new Tasks.TaskRefreshOutline( baseDoc ) );
          }
        }
        saveToolStripMenuItem.Enabled = baseDoc.Modified;
        saveAsToolStripMenuItem.Enabled = true;
        mainToolSave.Enabled = baseDoc.Modified;
      }
    }



    void AddToolWindow( ToolWindowType Type, BaseDocument Document, DockState DockState, ToolStripMenuItem MenuItem, bool VisibleEdit, bool VisibleDebug )
    {
      ToolWindow tool = new ToolWindow();

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
      /*
      if ( Visible )
      {
        tool.Document.Show( panelMain );
        tool.MenuItem.Checked = true;
      }
      else
      {
        //tool.Document.Show( panelMain );
        tool.Document.DockPanel = panelMain;
        tool.Document.DockState = DockState.Hidden;
        tool.MenuItem.Checked = false;
      }
       */
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
          tool.Visible[m_ActivePerspective] = tool.MenuItem.Checked;
          if ( tool.MenuItem.Checked )
          {
            tool.Document.Show( panelMain );
          }
          else
          {
            tool.Document.Hide();
          }
          m_ChangingToolWindows = false;
          return;
        }
      }
      m_ChangingToolWindows = false;
    }



    void m_DebugMemory_ViewScrolled( object Sender, DebugMemory.DebugMemoryEvent Event )
    {
      if ( AppState == Types.StudioState.DEBUGGING_BROKEN )
      {
        // request new memory
        VICERemoteDebugger.RequestData requestRefresh = new VICERemoteDebugger.RequestData(VICERemoteDebugger.Request.REFRESH_MEMORY, m_DebugMemory.MemoryStart, m_DebugMemory.MemorySize);
        requestRefresh.Reason = VICERemoteDebugger.RequestReason.MEMORY_FETCH;

        if ( !m_DebugMemory.MemoryAsCPU )
        {
          requestRefresh.Type = VICERemoteDebugger.Request.REFRESH_MEMORY_RAM;
        }


        IdleRequest debugFetch = new IdleRequest();
        debugFetch.DebugRequest = requestRefresh;

        IdleQueue.Add( debugFetch );
      }
    }


    public void UpdateMenuMRU()
    {
      /*
      int indexTop = fileToolStripMenuItem.DropDownItems.IndexOf(toolStripSeparatorAboveMRU);
      int index = fileToolStripMenuItem.DropDownItems.IndexOf(toolStripSeparatorBelowMRU);
      while ( indexTop + 1 < index )
      {
        fileToolStripMenuItem.DropDownItems[index - 1].Click -= menuMRUItem_Click;
        fileToolStripMenuItem.DropDownItems.RemoveAt( index - 1 );
        index = fileToolStripMenuItem.DropDownItems.IndexOf( toolStripSeparatorBelowMRU );
      }*/

      fileRecentlyOpenedProjectsToolStripMenuItem.DropDownItems.Clear();
      fileRecentlyOpenedFilesToolStripMenuItem.DropDownItems.Clear();
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
      if ( m_Solution == null )
      {
        return true;
      }
      SaveSolution();
      foreach ( Project project in m_Solution.Projects )
      {
        if ( !SaveProject( project ) )
        {
          return false;
        }
      }
      while ( m_Solution.Projects.Count > 0 )
      {
        if ( !CloseProject( m_Solution.Projects[0] ) )
        {
          return false;
        }
      }
      m_SolutionExplorer.treeProject.Nodes.Clear();
      mainToolConfig.Items.Clear();
      m_CurrentProject = null;
      projectToolStripMenuItem.Visible = false;
      StudioCore.Debugging.BreakPoints.Clear();
      //CloseAllDocuments();
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
            baseDoc.Save();
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
      result = result.Replace( "$(FilenameWithoutExtension)", System.IO.Path.GetFileNameWithoutExtension( fullDocPath ) );
      result = result.Replace( "$(FilePath)", GR.Path.RemoveFileSpec( fullDocPath ) );

      string targetFilename = "";
      if ( Document.Element == null )
      {
        targetFilename = StudioCore.Compiling.m_LastBuildInfo.TargetFile;
      }
      else
      {
        targetFilename = Document.Element.TargetFilename;
        if ( !string.IsNullOrEmpty( Document.Element.CompileTargetFile ) )
        {
          targetFilename = Document.Element.CompileTargetFile;
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
      string targetFilenameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(targetFilename);
      string fullTargetFilename = GR.Path.Append(targetPath, targetFilename);
      string fullTargetFilenameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fullTargetFilename);

      string runFilename = System.IO.Path.GetFileName(fullTargetFilename);
      string fullRunFilename = fullTargetFilename;
      string runPath = System.IO.Path.GetDirectoryName(fullRunFilename);

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
      string runFilenameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(runFilename);
      string fullRunFilenameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fullRunFilename);



      result = result.Replace( "$(BuildTargetPath)", targetPath );
      result = result.Replace( "$(BuildTargetFilename)", fullTargetFilename );
      result = result.Replace( "$(BuildTargetFilenameWithoutExtension)", fullTargetFilenameWithoutExtension );
      result = result.Replace( "$(BuildTargetFile)", targetFilename );
      result = result.Replace( "$(BuildTargetFileWithoutExtension)", targetFilenameWithoutExtension );
      result = result.Replace( "$(RunPath)", runPath );
      result = result.Replace( "$(RunFilename)", fullRunFilename );
      result = result.Replace( "$(RunFilenameWithoutExtension)", fullRunFilenameWithoutExtension );

      if ( Document.Project != null )
      {
        result = result.Replace( "$(ConfigName)", Document.Project.Settings.CurrentConfig.Name );
      }

      //m_DocumentToRun.Project.Settings.CurrentConfig.Name
      /*
    if ( mainToolConfig.SelectedItem != null )
    {
      result = result.Replace( "$(ConfigName)", mainToolConfig.SelectedItem.ToString() );
    }
    if ( Document.Project != null )
    {
      result = result.Replace( "$(ProjectPath)", Document.Project.Settings.BasePath );
    }*/
      result = result.Replace( "$(MediaManager)", "\"" + System.IO.Path.Combine( Application.StartupPath, "mediamanager.exe" ) + "\"" );
      result = result.Replace( "$(MediaTool)", "\"" + System.IO.Path.Combine( Application.StartupPath, "mediatool.exe" ) + "\"" );

      int debugStartAddress = StudioCore.Debugging.OverrideDebugStart;
      if ( debugStartAddress == -1 )
      {
        if ( Document.Project != null )
        {
          if ( !DetermineDebugStartAddress( Document, Document.Project.Settings.CurrentConfig.DebugStartAddressLabel, out debugStartAddress ) )
          {
            Error = true;
            StudioCore.AddToOutput( "Cannot determine value for debug start address from '" + Document.Project.Settings.CurrentConfig.DebugStartAddressLabel + "'" + System.Environment.NewLine );
            return "";
          }
        }
      }
      result = result.Replace( "$(DebugStartAddress)", debugStartAddress.ToString() );
      result = result.Replace( "$(DebugStartAddressHex)", debugStartAddress.ToString( "x" ) );

      // replace symbols
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
        string macroName = result.Substring(dollarPos + 2, macroEndPos - dollarPos - 2);

        if ( Document.Type == ProjectElement.ElementType.ASM_SOURCE )
        {
          string valueToInsert = "";
          if ( Document.ASMFileInfo.Labels.ContainsKey( macroName ) )
          {
            valueToInsert = Document.ASMFileInfo.Labels[macroName].AddressOrValue.ToString();
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
        }
        dollarPos = result.IndexOf( "$(", macroEndPos + 1 );
      }
      return result;
    }



    private bool DetermineDebugStartAddress( DocumentInfo Document, string Label, out int Address )
    {
      Address = -1;
      if ( Document.Type == ProjectElement.ElementType.ASM_SOURCE )
      {
        if ( Document.ASMFileInfo.Labels.ContainsKey( Label ) )
        {
          Address = Document.ASMFileInfo.Labels[Label].AddressOrValue;
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
      ushort     debugStartAddressValue = 2049;
      if ( ushort.TryParse( Label, out debugStartAddressValue ) )
      {
        Address = debugStartAddressValue;
        return true;
      }
      return false;
    }



    public bool RunCommand( DocumentInfo Doc, string StepDesc, string Command )
    {
      if ( !RunExternalCommand( Doc, Command ) )
      {
        StudioCore.AddToOutput( "-" + StepDesc + " step failed" + System.Environment.NewLine );
        return false;
      }
      StudioCore.AddToOutput( "-" + StepDesc + " step successful" + System.Environment.NewLine );
      return true;
    }



    public void DumpLabelFile( Types.ASM.FileInfo FileInfo )
    {
      StringBuilder sb = new StringBuilder();

      foreach ( var labelInfo in FileInfo.Labels )
      {
        sb.Append( labelInfo.Value.Name );
        sb.Append( " =$" );
        if ( labelInfo.Value.AddressOrValue > 255 )
        {
          sb.Append( labelInfo.Value.AddressOrValue.ToString( "X4" ) );
        }
        else
        {
          sb.Append( labelInfo.Value.AddressOrValue.ToString( "X2" ) );
        }
        sb.Append( "; " );
        if ( !labelInfo.Value.Used )
        {
          sb.AppendLine( "unused" );
        }
        else
        {
          sb.AppendLine();
        }
      }
      GR.IO.File.WriteAllText( FileInfo.LabelDumpFile, sb.ToString() );
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



    private bool StartCompile( DocumentInfo DocumentToBuild, DocumentInfo DocumentToDebug, DocumentInfo DocumentToRun, Solution Solution, bool CreatePreProcessedFile )
    {
      AddTask( new Tasks.TaskCompile( DocumentToBuild, DocumentToDebug, DocumentToRun, ActiveDocumentInfo, Solution, CreatePreProcessedFile ) );
      return true;
    }



    public void OnBuildFinished( DocumentInfo baseDoc, DocumentInfo ActiveDocumentInfo )
    {
    }



    private bool RunExternalCommand( string Command, DocumentInfo CommandDocument )
    {
      m_LastReceivedOutputTime = System.DateTime.Now;

      string fullCommand = Command;
      string args = "";
      if ( Command.StartsWith( "\"" ) )
      {
        int nextQuote = Command.IndexOf('"', 1);
        if ( nextQuote == -1 )
        {
          // invalid file
          StudioCore.AddToOutput( "Invalid command specified (" + Command + ")" );
          return false;
        }
        fullCommand = Command.Substring( 1, nextQuote - 1 );
        args = Command.Substring( nextQuote + 1 ).Trim();
      }
      else if ( Command.IndexOf( ' ' ) != -1 )
      {
        int spacePos = Command.IndexOf(' ');
        fullCommand = Command.Substring( 0, spacePos );
        args = Command.Substring( spacePos + 1 ).Trim();
      }

      fullCommand = "cmd.exe";

      bool error = false;
      bool errorAtArgs = false;

      string command = FillParameters( Command, CommandDocument, false, out error );
      if ( error )
      {
        return false;
      }
      args = "/C \"" + command + "\"";
      args = FillParameters( args, CommandDocument, false, out errorAtArgs );
      if ( ( error )
      ||   ( errorAtArgs ) )
      {
        return false;
      }


      //Debug.Log( "Args:" + args );
      //string command = fullCommand + " " + args;

      StudioCore.AddToOutput( command + System.Environment.NewLine );

      m_ExternalProcess = new System.Diagnostics.Process();
      m_ExternalProcess.StartInfo.FileName = fullCommand;
      m_ExternalProcess.StartInfo.WorkingDirectory = FillParameters( "$(BuildTargetPath)", CommandDocument, false, out error );

      if ( error )
      {
        return false;
      }
      if ( !System.IO.Directory.Exists( m_ExternalProcess.StartInfo.WorkingDirectory + "/" ) )
      {
        StudioCore.AddToOutput( "The determined working directory \"" + m_ExternalProcess.StartInfo.WorkingDirectory + "\" does not exist" + System.Environment.NewLine );
        return false;
      }

      m_ExternalProcess.StartInfo.CreateNoWindow = true;
      m_ExternalProcess.EnableRaisingEvents = true;
      m_ExternalProcess.StartInfo.Arguments = args;
      m_ExternalProcess.StartInfo.UseShellExecute = false;
      m_ExternalProcess.Exited += new EventHandler( m_ExternalProcess_Exited );

      m_ExternalProcess.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler( compilerProcess_OutputDataReceived );
      m_ExternalProcess.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler( compilerProcess_OutputDataReceived );
      m_ExternalProcess.StartInfo.RedirectStandardError = true;
      m_ExternalProcess.StartInfo.RedirectStandardOutput = true;

      try
      {
        if ( !m_ExternalProcess.Start() )
        {
          m_ExternalProcess.Close();
          return false;
        }
        m_ExternalProcess.BeginOutputReadLine();
        m_ExternalProcess.BeginErrorReadLine();
      }
      catch ( Win32Exception ex )
      {
        m_ExternalProcess.Close();
        StudioCore.AddToOutput( ex.Message + System.Environment.NewLine );
        return false;
      }

      //Debug.Log( "=============Start" );
      while ( !m_ExternalProcess.WaitForExit( 50 ) )
      {
        Application.DoEvents();
      }
      // DO NOT REMOVE: final DoEvents to let the app clear its invoke queue to the output display 
      Application.DoEvents();
      //Debug.Log( "=============Done" );

      /*
      // working wait
      while ( ( System.DateTime.Now - m_LastReceivedOutputTime ).TotalMilliseconds < 500  )
      {
        Application.DoEvents();
        System.Threading.Thread.Sleep( 20 );
      }
       */

      bool success = (m_ExternalProcess.ExitCode == 0);
      if ( !success )
      {
        StudioCore.AddToOutput( "External Command " + command + " exited with result code " + m_ExternalProcess.ExitCode.ToString() + System.Environment.NewLine );
      }
      m_ExternalProcess.Close();
      return success;
    }



    private bool RunExternalCommand( DocumentInfo Doc, string Command )
    {
      string[] commands = System.Text.RegularExpressions.Regex.Split(Command, System.Environment.NewLine);
      //Debug.Log( "Runexternalcommand " + Command );

      SetGUIForWaitOnExternalTool( true );
      foreach ( string command in commands )
      {
        if ( string.IsNullOrEmpty( command.Trim() ) )
        {
          continue;
        }
        if ( !RunExternalCommand( command, Doc ) )
        {
          SetGUIForWaitOnExternalTool( false );
          return false;
        }
      }
      SetGUIForWaitOnExternalTool( false );
      return true;
    }



    void m_ExternalProcess_Exited( object sender, EventArgs e )
    {
      /*
      System.Diagnostics.Process    process = (System.Diagnostics.Process)sender;
      string    output = process.StandardOutput.ReadToEnd();
      string    error = process.StandardError.ReadToEnd();

      AddToOutput( output );
      AddToOutput( error );*/
      /*
      System.Diagnostics.Process    process = (System.Diagnostics.Process)sender;
      int     exitCode = process.ExitCode;
      AddToOutput( "Tool Exited with result code " + exitCode.ToString() + System.Environment.NewLine );
      process.Close();
      SetGUIForWaitOnExternalTool( false );

      if ( exitCode != 0 )
      {
        // update errors/warnings
        AppState = State.NORMAL;
        return;
      }
      AppState = State.NORMAL;
       * */
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
      if ( !StartCompile( DocInfo, null, null, m_Solution, false ) )
      {
        AppState = Types.StudioState.NORMAL;
      }
    }



    public void Build( DocumentInfo Document )
    {
      Build( Document, false );
    }



    public void Build( DocumentInfo Document, bool CreatePreProcessedFile )
    {
      if ( AppState != Types.StudioState.NORMAL )
      {
        return;
      }
      if ( CreatePreProcessedFile )
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
      if ( !StartCompile( Document, null, null, m_Solution, CreatePreProcessedFile ) )
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
      if ( !StartCompile( Document, null, null, m_Solution, false ) )
      {
        AppState = Types.StudioState.NORMAL;
      }
    }



    private void mainToolCompile_Click( object sender, EventArgs e )
    {
      ApplyFunction( C64Studio.Types.Function.BUILD );
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
        m_Output.AppendText( Text );
        m_Output.Show();
      }
    }



    public void AddOutputMessages( Parser.ParserBase Parser )
    {
      foreach ( System.Collections.Generic.KeyValuePair<int, Parser.ParserBase.ParseMessage> msg in Parser.Messages )
      {
        Parser.ParserBase.ParseMessage message = msg.Value;
        if ( message.Type == C64Studio.Parser.ParserBase.ParseMessage.LineType.MESSAGE )
        {
          StudioCore.AddToOutput( message.Message + System.Environment.NewLine );
        }
      }
    }



    void compilerProcess_OutputDataReceived( object sender, System.Diagnostics.DataReceivedEventArgs e )
    {
      m_LastReceivedOutputTime = System.DateTime.Now;
      if ( !String.IsNullOrEmpty( e.Data ) )
      {
        StudioCore.AddToOutput( e.Data + System.Environment.NewLine );
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

        ToolInfo toolRun = StudioCore.DetermineTool(Document, true);
        if ( toolRun == null )
        {
          System.Windows.Forms.MessageBox.Show( "No emulator tool has been configured yet!", "Missing emulator tool" );
          StudioCore.AddToOutput( "There is no emulator tool configured!" );
          return false;
        }

        // check file version (WinVICE remote debugger changes)
        if ( !StudioCore.Debugging.Debugger.CheckEmulatorVersion( toolRun ) )
        {
          return false;
        }

        if ( !StudioCore.Executing.StartProcess( toolRun, Document ) )
        {
          return false;
        }

        if ( !System.IO.Directory.Exists( StudioCore.Executing.RunProcess.StartInfo.WorkingDirectory.Trim( new char[] { '\"' } ) ) )
        {
          StudioCore.AddToOutput( "The determined working directory" + StudioCore.Executing.RunProcess.StartInfo.WorkingDirectory + " does not exist" + System.Environment.NewLine );
          return false;
        }

        string runArguments = toolRun.PRGArguments;
        if ( IsCartridge( TargetType ) )
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
          string labelInfo = Document.ASMFileInfo.LabelsAsFile();
          if ( labelInfo.Length > 0 )
          {
            try
            {
              StudioCore.Debugging.TempDebuggerStartupFilename = System.IO.Path.GetTempFileName();
              System.IO.File.WriteAllText( StudioCore.Debugging.TempDebuggerStartupFilename, labelInfo );
              runArguments = "-moncommands \"" + StudioCore.Debugging.TempDebuggerStartupFilename + "\" " + runArguments;
            }
            catch ( System.IO.IOException ioe )
            {
              System.Windows.Forms.MessageBox.Show( ioe.Message, "Error writing temporary file" );
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
        return StudioCore.Executing.RunProcess.Start();
      }
      catch ( Exception ex )
      {
        StudioCore.AddToOutput( "Internal Error in RunCompiledFile: " + ex.ToString() );
        return false;
      }
    }



    internal bool EmulatorSupportsDebugging( ToolInfo Emulator )
    {
      return System.IO.Path.GetFileNameWithoutExtension( Emulator.Filename ).ToUpper().StartsWith( "X64" );
    }



    public bool DebugCompiledFile( DocumentInfo DocumentToDebug, DocumentInfo DocumentToRun )
    {
      if ( DocumentToDebug == null )
      {
        StudioCore.AddToOutput( "Debug document not found, this is an internal error!" );
        return false;
      }

      if ( DocumentToDebug.Element == null )
      {
        StudioCore.AddToOutput( "Debugging " + DocumentToDebug.DocumentFilename + System.Environment.NewLine );
      }
      else
      {
        StudioCore.AddToOutput( "Debugging " + DocumentToDebug.Element.Name + System.Environment.NewLine );
      }

      ToolInfo toolRun = StudioCore.DetermineTool(DocumentToRun, true);
      if ( toolRun == null )
      {
        System.Windows.Forms.MessageBox.Show( "No emulator tool has been configured yet!", "Missing emulator tool" );
        StudioCore.AddToOutput( "There is no emulator tool configured!" );
        return false;
      }

      if ( !StudioCore.Debugging.Debugger.CheckEmulatorVersion( toolRun ) )
      {
        return false;
      }

      StudioCore.Debugging.DebuggedASMBase = DocumentToDebug;
      StudioCore.Debugging.DebugBaseDocumentRun = DocumentToRun;

      m_DebugWatch.ReseatWatches( DocumentToDebug.ASMFileInfo );
      StudioCore.Debugging.Debugger.ClearCaches();
      StudioCore.Debugging.ReseatBreakpoints( DocumentToDebug.ASMFileInfo );
      StudioCore.Debugging.AddVirtualBreakpoints( DocumentToDebug.ASMFileInfo );
      //StudioCore.Debugging.Debugger.SetBreakPoints( StudioCore.Debugging.BreakPoints );
      StudioCore.Debugging.Debugger.ClearAllBreakpoints();
      StudioCore.Debugging.MarkedDocument = null;
      StudioCore.Debugging.MarkedDocumentLine = -1;
      /*
      Debug.Log( "Breakpoints." );
      foreach ( var bplist in m_BreakPoints.Values )
      {
        foreach ( var bp in bplist )
        {
          Debug.Log( "BP at " + bp.LineIndex + " in " + bp.DocumentFilename + " at " + bp.Address + "(" + bp.Address.ToString( "x4" ) + ") V=" + bp.IsVirtual );
          foreach ( var bpchild in bp.Virtual )
          {
            Debug.Log( "-BP at " + bpchild.LineIndex + " in " + bpchild.DocumentFilename + " at " + bpchild.Address + "(" + bp.Address.ToString( "x4" ) + ") V=" + bpchild.IsVirtual );
          }
        }
      }*/


      if ( !StudioCore.Executing.StartProcess( toolRun, DocumentToRun ) )
      {
        return false;
      }
      if ( !System.IO.Directory.Exists( StudioCore.Executing.RunProcess.StartInfo.WorkingDirectory.Trim( new char[] { '"' } ) ) )
      {
        StudioCore.AddToOutput( "The determined working directory " + StudioCore.Executing.RunProcess.StartInfo.WorkingDirectory + " does not exist" + System.Environment.NewLine );
        return false;
      }

      string breakPointFile = StudioCore.Debugging.PrepareAfterStartBreakPoints();

      string command = toolRun.DebugArguments;

      if ( ( toolRun.PassLabelsToEmulator )
      &&   ( StudioCore.Debugging.DebuggedASMBase.ASMFileInfo != null ) )
      {
        breakPointFile += StudioCore.Debugging.DebuggedASMBase.ASMFileInfo.LabelsAsFile();
      }

      if ( breakPointFile.Length > 0 )
      {
        try
        {
          StudioCore.Debugging.TempDebuggerStartupFilename = System.IO.Path.GetTempFileName();
          System.IO.File.WriteAllText( StudioCore.Debugging.TempDebuggerStartupFilename, breakPointFile );
          command += " -moncommands \"" + StudioCore.Debugging.TempDebuggerStartupFilename + "\"";
        }
        catch ( System.IO.IOException ioe )
        {
          System.Windows.Forms.MessageBox.Show( ioe.Message, "Error writing temporary file" );
          StudioCore.AddToOutput( "Error writing temporary file" );
          StudioCore.Debugging.TempDebuggerStartupFilename = "";
          return false;
        }
      }

      Types.CompileTargetType targetType = C64Studio.Types.CompileTargetType.NONE;
      if ( DocumentToRun.Element != null )
      {
        targetType = DocumentToRun.Element.TargetType;
      }

      string fileToRun = "";
      if ( DocumentToRun.Element != null )
      {
        fileToRun = DocumentToRun.Element.TargetFilename;
        ProjectElement.PerConfigSettings configSetting = DocumentToRun.Element.Settings[DocumentToRun.Project.Settings.CurrentConfig.Name];
        if ( !string.IsNullOrEmpty( configSetting.DebugFile ) )
        {
          targetType = configSetting.DebugFileType;
        }
      }

      if ( targetType == C64Studio.Types.CompileTargetType.NONE )
      {
        targetType = StudioCore.Compiling.m_LastBuildInfo.TargetType;
      }

      //ParserASM.CompileTarget != Types.CompileTargetType.NONE ) ? ParserASM.CompileTarget : DocumentToRun.Element.TargetType;

      // need to adjust initial breakpoint address for late added store/load breakpoints?
      if ( StudioCore.Debugging.BreakpointsToAddAfterStartup.Count > 0 )
      {
        // yes
        StudioCore.Debugging.LateBreakpointOverrideDebugStart = StudioCore.Debugging.OverrideDebugStart;

        // special start addresses for different run types

        if ( IsCartridge( targetType ) )
        {
          StudioCore.Debugging.OverrideDebugStart = 0x8000;
        }
        else
        {
          // directly after calling load from ram (as VICE does when autostarting a .prg file)
          // TODO - check with .t64, .tap, .d64
          StudioCore.Debugging.OverrideDebugStart = 0xe178;
        }
      }

      if ( StudioCore.Settings.TrueDriveEnabled )
      {
        command = toolRun.TrueDriveOnArguments + " " + command;
      }
      else
      {
        command = toolRun.TrueDriveOffArguments + " " + command;
      }

      bool error = false;

      StudioCore.Executing.RunProcess.StartInfo.Arguments = FillParameters( command, DocumentToRun, true, out error );
      if ( error )
      {
        return false;
      }

      if ( IsCartridge( targetType ) )
      {
        StudioCore.Executing.RunProcess.StartInfo.Arguments += " " + FillParameters( toolRun.CartArguments, DocumentToRun, true, out error );
      }
      else
      {
        StudioCore.Executing.RunProcess.StartInfo.Arguments += " " + FillParameters( toolRun.PRGArguments, DocumentToRun, true, out error );
      }
      if ( error )
      {
        return false;
      }

      StudioCore.AddToOutput( "Calling " + StudioCore.Executing.RunProcess.StartInfo.FileName + " with " + StudioCore.Executing.RunProcess.StartInfo.Arguments + System.Environment.NewLine );
      StudioCore.Executing.RunProcess.Exited += new EventHandler( runProcess_Exited );
      StudioCore.SetStatus( "Running..." );

      SetGUIForWaitOnExternalTool( true );

      if ( StudioCore.Executing.RunProcess.Start() )
      {
        DateTime    current = DateTime.Now;

        // new GTK VICE opens up with console window (yuck) which nicely interferes with WaitForInputIdle -> give it 5 seconds to open main window
        try
        {
          StudioCore.Executing.RunProcess.WaitForInputIdle( 5000 );
        }
        catch ( Exception ex )
        {
          Debug.Log( "WaitForInputIdle failed: " + ex.ToString() );
        }

        // only connect with debugger if VICE
        if ( EmulatorSupportsDebugging( toolRun ) )
        {
          if ( StudioCore.Debugging.Debugger.ConnectToEmulator() )
          {
            m_CurrentActiveTool = toolRun;
            StudioCore.Debugging.DebuggedProject = DocumentToRun.Project;
            AppState = Types.StudioState.DEBUGGING_RUN;
            SetGUIForDebugging( true );
          }
        }
        else
        {
          m_CurrentActiveTool = toolRun;
          StudioCore.Debugging.DebuggedProject = DocumentToRun.Project;
          AppState = Types.StudioState.DEBUGGING_RUN;
          SetGUIForDebugging( true );
        }
      }
      return true;
    }



    private bool IsCartridge( CompileTargetType Type )
    {
      if ( ( Type == Types.CompileTargetType.CARTRIDGE_MAGICDESK_BIN )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_MAGICDESK_CRT )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_EASYFLASH_BIN )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_EASYFLASH_CRT )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_RGCD_BIN )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_RGCD_CRT )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_GMOD2_BIN )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_GMOD2_CRT )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_16K_BIN )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_16K_CRT )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_8K_BIN )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_8K_CRT ) )
      {
        return true;
      }
      return false;
    }

    /*
    void compilerProcess_Exited( object sender, EventArgs e )
    {
      int     exitCode = CompilerProcess.ExitCode;
      AddToOutput( "Tool Exited with result code " + exitCode.ToString() + System.Environment.NewLine );
      CompilerProcess.Close();
      SetGUIForWaitOnExternalTool( false );

      ProjectConfig   config = m_Project.Settings.Configs[mainToolConfig.SelectedItem.ToString()];

      if ( exitCode != 0 )
      {
        // update errors/warnings
        ParseFile( ParserASM, ActiveDocument, config );
        AppState = State.NORMAL;
        return;
      }

      switch ( AppState )
      {
        case State.BUILD:
          ParseFile( ParserASM, ActiveDocument, config );
          AppState = State.NORMAL;
          break;
        case State.BUILD_AND_RUN:
          // run program
          RunCompiledFile( ActiveDocument, Types.CompileTargetType.NONE );
          break;
        case State.BUILD_AND_DEBUG:
          // run program
          DebugCompiledFile( ActiveDocument );
          break;
        default:
          AppState = State.NORMAL;
          break;
      }
    }
    */



    void runProcess_Exited( object sender, EventArgs e )
    {
      if ( StudioCore.Executing.RunProcess == null )
      {
        StudioCore.AddToOutput( "Run exited unexpectedly" + System.Environment.NewLine );
      }
      else
      {
        try
        {
          StudioCore.AddToOutput( "Run exited with result code " + StudioCore.Executing.RunProcess.ExitCode + System.Environment.NewLine );
          StudioCore.Executing.RunProcess.Close();
          StudioCore.Executing.RunProcess.Dispose();
        }
        catch ( System.Exception ex )
        {
          StudioCore.AddToOutput( "Run aborted with error: " + ex.Message + System.Environment.NewLine );
        }
      }
      StudioCore.Executing.RunProcess = null;

      StudioCore.Debugging.Debugger.DisconnectFromEmulator();

      if ( StudioCore.Debugging.TempDebuggerStartupFilename.Length > 0 )
      {
        try
        {
          System.IO.File.Delete( StudioCore.Debugging.TempDebuggerStartupFilename );
        }
        catch ( Exception ex )
        {
          StudioCore.AddToOutput( "Failed to delete temporary file " + StudioCore.Debugging.TempDebuggerStartupFilename + ", " + ex.Message );
        }
        StudioCore.Debugging.TempDebuggerStartupFilename = "";
      }

      AppState = Types.StudioState.NORMAL;

      StudioCore.Debugging.RemoveVirtualBreakpoints();
      StudioCore.SetStatus( "Ready", false, 0 );

      SetGUIForDebugging( false );
      SetGUIForWaitOnExternalTool( false );
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
      baseDoc.Save();
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



    public ProjectElement CreateNewElement( ProjectElement.ElementType Type, string StartName, Project Project, TreeNode ParentNode )
    {
      if ( Project == null )
      {
        return null;
      }
      Project projectToAdd = m_SolutionExplorer.ProjectFromNode(ParentNode);
      ProjectElement elementParent = m_SolutionExplorer.ElementFromNode(ParentNode);

      ProjectElement element = projectToAdd.CreateElement(Type, ParentNode);
      element.Name = StartName;
      element.Node.Text = StartName;
      element.ProjectHierarchy = m_SolutionExplorer.GetElementHierarchy( element.Node );
      element.DocumentInfo.Project = Project;

      if ( element.Document != null )
      {
        RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.DOCUMENT_CREATED, element.DocumentInfo ) );
      }
      projectToAdd.ShowDocument( element );
      if ( element.Document != null )
      {
        element.Document.SetModified();
      }
      RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.DOCUMENT_INFO_CREATED, element.DocumentInfo ) );
      return element;
    }



    public void AddBreakpoint( Types.Breakpoint Breakpoint )
    {
      if ( AppState == Types.StudioState.NORMAL )
      {
        if ( !StudioCore.Debugging.BreakPoints.ContainsKey( Breakpoint.DocumentFilename ) )
        {
          StudioCore.Debugging.BreakPoints[Breakpoint.DocumentFilename] = new List<C64Studio.Types.Breakpoint>();
        }
        StudioCore.Debugging.BreakPoints[Breakpoint.DocumentFilename].Add( Breakpoint );
        //Debug.Log( "add breakpoint for " + asm.DocumentFilename + " at line " + Breakpoint.LineIndex );
      }
      else if ( AppState == Types.StudioState.DEBUGGING_BROKEN )
      {
        if ( !StudioCore.Debugging.BreakPoints.ContainsKey( Breakpoint.DocumentFilename ) )
        {
          StudioCore.Debugging.BreakPoints[Breakpoint.DocumentFilename] = new List<C64Studio.Types.Breakpoint>();
        }
        StudioCore.Debugging.BreakPoints[Breakpoint.DocumentFilename].Add( Breakpoint );
        StudioCore.Debugging.Debugger.AddBreakpoint( Breakpoint );
        //Debug.Log( "add live breakpoint for " + asm.DocumentFilename + " at line " + Breakpoint.LineIndex );
      }
      else
      {
        return;
      }
      m_DebugBreakpoints.AddBreakpoint( Breakpoint );
    }



    private void RemoveBreakpoint( Types.Breakpoint Breakpoint )
    {
      if ( AppState == Types.StudioState.NORMAL )
      {
        if ( StudioCore.Debugging.BreakPoints.ContainsKey( Breakpoint.DocumentFilename ) )
        {
          foreach ( Types.Breakpoint breakPoint in StudioCore.Debugging.BreakPoints[Breakpoint.DocumentFilename] )
          {
            if ( breakPoint == Breakpoint )
            {
              StudioCore.Debugging.BreakPoints[Breakpoint.DocumentFilename].Remove( breakPoint );
              m_DebugBreakpoints.RemoveBreakpoint( breakPoint );
              StudioCore.Debugging.Debugger.RemoveBreakpoint( breakPoint.RemoteIndex );
              Debug.Log( "-removed" );
              break;
            }
          }
        }

        if ( Breakpoint.DocumentFilename != "C64Studio.DebugBreakpoints" )
        {
          ProjectElement element = CurrentProject.GetElementByFilename(Breakpoint.DocumentFilename);
          if ( ( element != null )
          &&   ( element.Document != null )
          &&   ( element.DocumentInfo.Type == ProjectElement.ElementType.ASM_SOURCE ) )
          {
            SourceASMEx asm = (SourceASMEx)element.Document;
            asm.RemoveBreakpoint( Breakpoint );

            Debug.Log( "remove breakpoint for " + asm.DocumentFilename + " at line " + Breakpoint.LineIndex );
          }
        }
      }
      else if ( AppState == Types.StudioState.DEBUGGING_BROKEN )
      {
        //Debug.Log( "try to remove live breakpoint for " + Event.Doc.DocumentFilename + " at line " + Event.LineIndex );
        if ( StudioCore.Debugging.BreakPoints.ContainsKey( Breakpoint.DocumentFilename ) )
        {
          foreach ( Types.Breakpoint breakPoint in StudioCore.Debugging.BreakPoints[Breakpoint.DocumentFilename] )
          {
            if ( breakPoint == Breakpoint )
            {
              StudioCore.Debugging.BreakPoints[Breakpoint.DocumentFilename].Remove( breakPoint );
              m_DebugBreakpoints.RemoveBreakpoint( breakPoint );

              StudioCore.Debugging.Debugger.RemoveBreakpoint( breakPoint.RemoteIndex, breakPoint );
              //Debug.Log( "-removed" );
              break;
            }
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
        case BaseDocument.DocEvent.Type.BREAKPOINT_ADDED:
          if ( ( AppState == Types.StudioState.NORMAL )
          || ( AppState == Types.StudioState.DEBUGGING_BROKEN ) )
          {
            AddBreakpoint( Event.Breakpoint );
          }
          break;
        case BaseDocument.DocEvent.Type.BREAKPOINT_REMOVED:
          if ( ( AppState == Types.StudioState.NORMAL )
          || ( AppState == Types.StudioState.DEBUGGING_BROKEN ) )
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



    private void AddNewDocumentOrElement( ProjectElement.ElementType Type, string Description, Project ParentProject, TreeNode ParentNode )
    {
      if ( ParentProject != null )
      {
        var dialogResult = System.Windows.Forms.MessageBox.Show( "Add the new document to the current project?\r\nIf you choose no, the document will be created not as part of the current project.", "Add to current project?", System.Windows.Forms.MessageBoxButtons.YesNoCancel );
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

      // project-less doc

      string newFilename;
      if ( !ChooseFilename( Type, Description, ParentProject, out newFilename ) )
      {
        return;
      }

      if ( System.IO.File.Exists( newFilename ) )
      {
        var result = System.Windows.Forms.MessageBox.Show( "There is already an existing file at " + newFilename + ".\r\nDo you want to overwrite it?", "Overwrite existing file?", MessageBoxButtons.YesNo );
        if ( result == DialogResult.No )
        {
          return;
        }
      }
      var doc = CreateNewDocument(Type, null);
      doc.SetDocumentFilename( newFilename );
      doc.SetModified();

      StudioCore.Settings.UpdateInMRU( StudioCore.Settings.MRUFiles, newFilename, this );
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
      FormProjectWizard projectWizard = new FormProjectWizard(ProjectName, StudioCore.Settings);

      if ( projectWizard.ShowDialog() != DialogResult.OK )
      {
        return null;
      }
      Project newProject = new Project();
      newProject.Core = StudioCore;
      newProject.Settings.Name = projectWizard.ProjectName;
      newProject.Settings.Filename = projectWizard.ProjectFilename;
      newProject.Settings.BasePath = System.IO.Path.GetDirectoryName( newProject.Settings.Filename );
      newProject.Node = new TreeNode();
      newProject.Node.Tag = newProject;
      newProject.Node.Text = newProject.Settings.Name;


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
      if ( m_Solution == null )
      {
        m_Solution = new Solution( this );
      }
      m_Solution.Projects.Add( newProject );

      // TODO - should be different
      m_SolutionExplorer.treeProject.Nodes.Add( newProject.Node );

      RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.SOLUTION_OPENED ) );

      SetActiveProject( newProject );

      SaveProject( newProject );
      UpdateUndoSettings();
      return newProject;
    }



    public bool CreateNewProject()
    {
      if ( !CloseAllProjects() )
      {
        return false;
      }
      return ( AddNewProject() != null );
    }



    public Project AddNewProject()
    {
      string projectName = "New Project";
      Project newProject = NewProjectWizard(projectName);
      if ( newProject == null )
      {
        return null;
      }
      projectToolStripMenuItem.Visible = true;

      //m_ProjectExplorer.NodeProject.Text  = newProject.Settings.Name;
      //m_ProjectExplorer.NodeProject.Tag   = newProject;

      foreach ( string configName in newProject.Settings.Configs.Keys )
      {
        mainToolConfig.Items.Add( configName );
        if ( ( newProject.Settings.CurrentConfig != null )
        && ( configName == newProject.Settings.CurrentConfig.Name ) )
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
      CreateNewProject();
    }



    public bool CloseProject( Project ProjectToClose )
    {
      if ( ProjectToClose == null )
      {
        return true;
      }
      if ( ProjectToClose.Modified )
      {
        System.Windows.Forms.DialogResult saveResult = System.Windows.Forms.MessageBox.Show("The project " + ProjectToClose.Settings.Name + " has been modified. Do you want to save the changes now?", "Save Changes?", MessageBoxButtons.YesNoCancel);

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
      /*
      if ( m_Project.Settings.Filename == null )
      {
        return false;
      }
       */
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
        DialogResult res = System.Windows.Forms.MessageBox.Show("There are changes in one or more items. Do you want to save them before closing?", "Unsaved changes, save now?", MessageBoxButtons.YesNoCancel);
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
              element.Document.Save();
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

        RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.DOCUMENT_INFO_REMOVED, element.DocumentInfo ) );
      }

      try
      {
        m_SolutionExplorer.treeProject.Nodes.Remove( ProjectToClose.Node );
      }
      catch
      {
      }

      // TODO - adjust GUI to changed project
      m_Solution.RemoveProject( ProjectToClose );
      if ( m_CurrentProject == ProjectToClose )
      {
        mainToolConfig.Items.Clear();
        m_CurrentProject = null;
      }

      projectToolStripMenuItem.Visible = false;
      StudioCore.Debugging.BreakPoints.Clear();
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
        CloseProject( m_CurrentProject );
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
        RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.ACTIVE_PROJECT_CHANGED, NewProject ) );
        if ( mainToolConfig.ComboBox != null )
        {
          mainToolConfig.Items.Clear();
          if ( NewProject != null )
          {
            foreach ( string configName in NewProject.Settings.Configs.Keys )
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
      if ( m_Solution != null )
      {
        foreach ( Project project in m_Solution.Projects )
        {
          if ( GR.Path.IsPathEqual( Filename, project.Settings.Filename ) )
          {
            System.Windows.Forms.MessageBox.Show( "The project " + Filename + " is already opened in this solution.", "Project already opened" );
            return null;
          }
        }
      }

      bool createdNewSolution = false;
      if ( m_Solution == null )
      {
        createdNewSolution = true;
        m_Solution = new Solution( this );

        SaveSolution();
      }

      Project newProject = new Project();
      newProject.Core = StudioCore;
      m_LoadingProject = true;
      if ( newProject.Load( Filename ) )
      {
        m_LoadingProject = false;
        m_Solution.Projects.Add( newProject );
        m_SolutionExplorer.treeProject.Nodes.Add( newProject.Node );
        projectToolStripMenuItem.Visible = true;
        //Settings.UpdateInMRU( newProject.Settings.Filename, this );

        if ( createdNewSolution )
        {
          RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.SOLUTION_OPENED ) );
        }

        SetActiveProject( newProject );

        AddTask( new Tasks.TaskPreparseFilesInProject( newProject, mainToolConfig.SelectedItem.ToString() ) );
        //PreparseFilesInProject( newProject );

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
            ParseFile( StudioCore.Compiling.ParserASM, element.DocumentInfo, newProject.Settings.Configs[SelectedConfig], false, false );

            //var knownTokens = ParserASM.KnownTokens();
            //var knownTokenInfos = ParserASM.KnownTokenInfo();
            //Debug.Log( "SetASMFileInfo on " + element.DocumentInfo.DocumentFilename );
            //element.DocumentInfo.SetASMFileInfo( ParserASM.ASMFileInfo, knownTokens, knownTokenInfos );

            updatedFiles.Add( element.DocumentInfo.FullPath );

            if ( element.Document != null )
            {
              ( (SourceASMEx)element.Document ).SetLineInfos( StudioCore.Compiling.ParserASM.ASMFileInfo );
            }

            AddTask( new Tasks.TaskUpdateKeywords( element.Document ) );

            foreach ( var dependencyBuildState in element.DocumentInfo.DeducedDependency.Values )
            {
              foreach ( var dependency in dependencyBuildState.BuildState.Keys )
              {
                ProjectElement element2 = newProject.GetElementByFilename(dependency);
                if ( ( element2 != null )
                && ( element2.DocumentInfo.Type == ProjectElement.ElementType.ASM_SOURCE ) )
                {
                  if ( element2.Document != null )
                  {
                    ( (SourceASMEx)element2.Document ).SetLineInfos( StudioCore.Compiling.ParserASM.ASMFileInfo );
                  }
                  updatedFiles.Add( element2.DocumentInfo.FullPath );
                }
              }
            }
          }
        }
        //m_CompileResult.ClearMessages();
        if ( m_ActiveSource != null )
        {
          //Debug.Log( "RefreshFromDocument after openproject" );
          m_Outline.RefreshFromDocument( m_ActiveSource );
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
      if ( CurrentProject != null )
      {
        Text = "C64Studio - " + CurrentProject.Settings.Name;
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
      if ( !StartCompile( DocumentToBuild, null, DocumentToRun, m_Solution, false ) )
      {
        AppState = Types.StudioState.NORMAL;
      }
    }



    private void mainToolCompileAndRun_Click( object sender, EventArgs e )
    {
      ApplyFunction( C64Studio.Types.Function.BUILD_AND_RUN );
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



    public bool ImportExistingFiles( TreeNode Node )
    {
      Project projectToAddTo = null;
      if ( Node != null )
      {
        projectToAddTo = m_SolutionExplorer.ProjectFromNode( Node );
      }
      if ( projectToAddTo == null )
      {
        projectToAddTo = m_CurrentProject;
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

      foreach ( var fileName in openDlg.FileNames )
      {
        string importFile = fileName;

        bool skipFile = false;

        if ( projectToAddTo.IsFilenameInUse( importFile ) )
        {
          System.Windows.Forms.MessageBox.Show( "File " + importFile + " is already part of this project", "File already added" );
          skipFile = true;
          break;
        }
        if ( skipFile )
        {
          continue;
        }

        // determine type by extension
        ProjectElement.ElementType type = ProjectElement.ElementType.ASM_SOURCE;
        string newFileExtension = System.IO.Path.GetExtension(GR.Path.RelativePathTo(importFile, false, System.IO.Path.GetFullPath(projectToAddTo.Settings.BasePath), true).ToUpper());

        if ( ( newFileExtension == ".CHARSETPROJECT" )
        || ( newFileExtension == ".CHR" ) )
        {
          type = ProjectElement.ElementType.CHARACTER_SET;
        }
        else if ( ( newFileExtension == ".SPRITEPROJECT" )
        || ( newFileExtension == ".SPR" ) )
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
        else if ( newFileExtension == ".BAS" )
        {
          type = ProjectElement.ElementType.BASIC_SOURCE;
        }
        else if ( newFileExtension == ".MAPPROJECT" )
        {
          type = ProjectElement.ElementType.MAP_EDITOR;
        }

        if ( !GR.Path.IsSubPath( System.IO.Path.GetFullPath( projectToAddTo.Settings.BasePath ), importFile ) )
        {
          // not a sub folder
          var result = System.Windows.Forms.MessageBox.Show("The item is not inside the current project folder. Should a copy be created in the project folder?",
                                                             "Create a local copy of the added file?",
                                                             MessageBoxButtons.YesNoCancel);
          if ( result == DialogResult.Cancel )
          {
            return false;
          }
          if ( result == DialogResult.Yes )
          {
            // create a copy
            string pureFileName = System.IO.Path.GetFileName(importFile);
            string newFileName = System.IO.Path.Combine(System.IO.Path.GetFullPath(projectToAddTo.Settings.BasePath), pureFileName);

            while ( System.IO.File.Exists( newFileName ) )
            {
              newFileName = System.IO.Path.Combine( System.IO.Path.GetFullPath( projectToAddTo.Settings.BasePath ), System.IO.Path.GetFileNameWithoutExtension( newFileName ) + "_" + System.IO.Path.GetExtension( newFileName ) );
            }
            try
            {
              System.IO.File.Copy( importFile, newFileName, false );
            }
            catch ( System.Exception ex )
            {
              StudioCore.AddToOutput( "Could not copy file to new location: " + ex.Message );
              return false;
            }
            importFile = newFileName;
          }
        }

        TreeNode parentNodeToInsertTo = Node;

        ProjectElement element = projectToAddTo.CreateElement(type, parentNodeToInsertTo);

        //string    relativeFilename = GR.Path.RelativePathTo( openDlg.FileName, false, System.IO.Path.GetFullPath( m_Project.Settings.BasePath ), true );
        string relativeFilename = GR.Path.RelativePathTo(System.IO.Path.GetFullPath(projectToAddTo.Settings.BasePath), true, importFile, false);
        element.Name = System.IO.Path.GetFileNameWithoutExtension( relativeFilename );
        element.Filename = relativeFilename;

        while ( parentNodeToInsertTo.Level >= 1 )
        {
          element.ProjectHierarchy.Insert( 0, parentNodeToInsertTo.Text );
          parentNodeToInsertTo = parentNodeToInsertTo.Parent;
        }
        projectToAddTo.ShowDocument( element );
        element.DocumentInfo.DocumentFilename = relativeFilename;
        if ( element.Document != null )
        {
          element.Document.SetDocumentFilename( relativeFilename );
        }
        projectToAddTo.SetModified();
      }
      return true;
    }



    private void importFileToolStripMenuItem_Click( object sender, EventArgs e )
    {
      ImportExistingFiles( null );
    }



    private void SetGUIForWaitOnExternalTool( bool Wait )
    {
      if ( InvokeRequired )
      {
        Invoke( new SetGUIForWaitOnExternalToolCallback( SetGUIForWaitOnExternalTool ), new object[] { Wait } );
      }
      else
      {
        // dockpanelsuite activates the first document, not the currently shown one if focus is set to it (by disabling the toolbar)
        BaseDocument    prevActiveDocument = ActiveDocument;

        mainTools.Enabled = !Wait;

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
          &&   ( subMenu.Text != "&Help" ) )   
          {
            subMenu.Enabled = !Wait;
          }
        }
        //mainMenu.Enabled = !Wait;
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
          menuWindowToolbarDebugger.Checked = debugTools.Enabled;
          if ( DebugModeActive )
          {
            SetToolPerspective( Perspective.DEBUG );
            mainDebugGo.Enabled = ( AppState != Types.StudioState.DEBUGGING_RUN );
            mainDebugBreak.Enabled = ( AppState == Types.StudioState.DEBUGGING_RUN );
          }
          else
          {
            SetToolPerspective( Perspective.EDIT );
            mainDebugGo.Enabled = true;
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
      if ( !StudioCore.Debugging.Debugger.ConnectToEmulator() )
      {
        Debug.Log( "Connect failed" );
      }
    }



    private void debugDisconnectToolStripMenuItem_Click( object sender, EventArgs e )
    {
      StudioCore.Debugging.Debugger.DisconnectFromEmulator();
    }



    private void filePreferencesToolStripMenuItem_Click( object sender, EventArgs e )
    {
      Settings prefDlg = new Settings(StudioCore, C64Studio.Settings.TabPage.GENERAL);

      prefDlg.ShowDialog();
    }



    private void SaveSettings()
    {
      StudioCore.Settings.MainWindowPlacement = GR.Forms.WindowStateManager.GeometryToString( this );

      m_FindReplace.ToSettings( StudioCore.Settings );

      GR.Memory.ByteBuffer SettingsData = StudioCore.Settings.ToBuffer( StudioCore );

      string settingFilename = SettingsPath();

      System.IO.Directory.CreateDirectory( System.IO.Directory.GetParent( settingFilename ).FullName );
      System.IO.File.WriteAllBytes( settingFilename, SettingsData.Data() );

      CloseSolution();
    }



    private void DetermineSettingsPath()
    {
      if ( StudioCore.Settings.StudioAppMode == AppMode.UNDECIDED )
      {
        // decide by checking for existance of settings file
        if ( System.IO.File.Exists( System.IO.Path.Combine( Application.StartupPath, "settings.dat" ) ) )
        {
          StudioCore.Settings.StudioAppMode = AppMode.PORTABLE_APP;
        }
        else if ( System.IO.File.Exists( System.IO.Path.Combine( Application.UserAppDataPath, "settings.dat" ) ) )
        {
          StudioCore.Settings.StudioAppMode = AppMode.GOOD_APP;
        }
      }
    }



    private string SettingsPath()
    {
      // prefix hard coded version number so we can use our proper version number
      string    userAppDataPath = GR.Path.ParentDirectory( Application.UserAppDataPath );

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

      if ( SettingsData.Empty() )
      {
        return false;
      }

      if ( !StudioCore.Settings.ReadFromBuffer( SettingsData ) )
      {
        return false;
      }

      m_DebugMemory.SetMemoryDisplayType();
      m_DebugMemory.ApplyHexViewColors();

      StudioCore.Settings.SanitizeSettings();
      return true;
    }



    private void MainForm_FormClosed( object sender, FormClosedEventArgs e )
    {
      StudioCore.Debugging.Debugger.Quit();
    }



    private void BuildAndDebug( DocumentInfo DocumentToBuild, DocumentInfo DocumentToDebug, DocumentInfo DocumentToRun )
    {
      if ( AppState != Types.StudioState.NORMAL )
      {
        return;
      }
      AppState = Types.StudioState.BUILD_AND_DEBUG;
      StudioCore.Debugging.OverrideDebugStart = -1;
      if ( !StartCompile( DocumentToBuild, DocumentToDebug, DocumentToRun, m_Solution, false ) )
      {
        AppState = Types.StudioState.NORMAL;
      }
    }



    private void mainToolDebug_Click( object sender, EventArgs e )
    {
      ApplyFunction( C64Studio.Types.Function.BUILD_AND_DEBUG );
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
          StudioCore.Debugging.MarkLine( StudioCore.Debugging.MarkedDocument.DocumentInfo.Project, StudioCore.Debugging.MarkedDocument.DocumentInfo.FullPath, -1 );
          StudioCore.Debugging.MarkedDocument = null;
        }

        StudioCore.Executing.BringToForeground();

        AppState = Types.StudioState.DEBUGGING_RUN;
        StudioCore.Debugging.FirstActionAfterBreak = false;

        StudioCore.MainForm.SetGUIForDebugging( true );
        //mainDebugGo.Enabled = false;
        //mainDebugBreak.Enabled = true;

        Types.Breakpoint tempBP = new C64Studio.Types.Breakpoint();
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
        if ( !StartCompile( DocumentToRun, DocumentToDebug, DocumentToRun, m_Solution, false ) )
        {
          AppState = Types.StudioState.NORMAL;
        }
      }
    }



    private void DebugGo()
    {
      if ( ( m_CurrentActiveTool != null )
      && ( !EmulatorSupportsDebugging( m_CurrentActiveTool ) ) )
      {
        return;
      }

      if ( AppState == Types.StudioState.DEBUGGING_BROKEN )
      {
        StudioCore.Debugging.Debugger.Run();

        if ( StudioCore.Debugging.MarkedDocument != null )
        {
          StudioCore.Debugging.MarkLine( StudioCore.Debugging.MarkedDocument.DocumentInfo.Project, StudioCore.Debugging.MarkedDocument.DocumentInfo.FullPath, -1 );
          StudioCore.Debugging.MarkedDocument = null;
        }

        StudioCore.Executing.BringToForeground();

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
        Invoke( new ParameterLessCallback( StopDebugging ) );
      }
      else
      {
        try
        {
          if ( ( m_CurrentActiveTool != null )
          && ( !EmulatorSupportsDebugging( m_CurrentActiveTool ) ) )
          {
            if ( StudioCore.Executing.RunProcess != null )
            {
              StudioCore.Executing.RunProcess.Kill();
              StudioCore.Executing.RunProcess = null;
            }
            return;
          }

          if ( ( AppState == Types.StudioState.DEBUGGING_BROKEN )
          || ( AppState == Types.StudioState.DEBUGGING_RUN ) )
          {
            // send any command to break into the monitor again
            StudioCore.Debugging.Debugger.Quit();

            if ( StudioCore.Debugging.MarkedDocument != null )
            {
              StudioCore.Debugging.MarkLine( StudioCore.Debugging.MarkedDocument.DocumentInfo.Project, StudioCore.Debugging.MarkedDocument.DocumentInfo.FullPath, -1 );
              StudioCore.Debugging.MarkedDocument = null;
            }
            /*
            if ( ( ActiveDocument != StudioCore.Debugging.DebugDisassembly )
            &&   ( ActiveDocumentInfo != null ) )
            {
              MarkLine( ActiveDocumentInfo.Project, ActiveDocumentInfo.FullPath, -1 );
            }
            else
            {
              StudioCore.Debugging.MarkedDocument = null;
              StudioCore.Debugging.MarkedDocumentLine = -1;
            }*/

            if ( StudioCore.Debugging.DebugDisassembly != null )
            {
              StudioCore.Debugging.DebugDisassembly.Close();
              StudioCore.Debugging.DebugDisassembly = null;
            }
            StudioCore.Debugging.CurrentCodePosition = -1;

            StudioCore.Debugging.DebuggedProject = null;
            m_CurrentActiveTool = null;
            StudioCore.Debugging.FirstActionAfterBreak = false;
            mainDebugGo.Enabled = false;
            mainDebugBreak.Enabled = false;
            AppState = Types.StudioState.NORMAL;
          }
        }
        catch ( System.Exception ex )
        {
          Debug.Log( ex.ToString() );
        }
      }
    }



    private void mainDebugStop_Click( object sender, EventArgs e )
    {
      ApplyFunction( C64Studio.Types.Function.DEBUG_STOP );
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
      if ( ( AppState == Types.StudioState.DEBUGGING_BROKEN )
      || ( AppState == Types.StudioState.DEBUGGING_RUN ) )
      {
        m_DebugMemory.InvalidateAllMemory();
        StudioCore.Debugging.Debugger.StepOut();
        StudioCore.Debugging.Debugger.RefreshRegistersAndWatches();
        StudioCore.Debugging.Debugger.SetAutoRefreshMemory( m_DebugMemory.MemoryStart,
                                                            m_DebugMemory.MemorySize,
                                                            m_DebugMemory.MemoryAsCPU ? MemorySource.AS_CPU : MemorySource.RAM );
        StudioCore.Debugging.Debugger.RefreshMemory( m_DebugMemory.MemoryStart,
                                                     m_DebugMemory.MemorySize,
                                                     m_DebugMemory.MemoryAsCPU ? MemorySource.AS_CPU : MemorySource.RAM );

        if ( AppState == Types.StudioState.DEBUGGING_RUN )
        {
          StudioCore.Debugging.FirstActionAfterBreak = true;
        }
        StudioCore.Executing.BringStudioToForeground();
        AppState = Types.StudioState.DEBUGGING_BROKEN;
        mainDebugGo.Enabled = true;
        mainDebugBreak.Enabled = false;
      }
    }



    private void refreshRegistersToolStripMenuItem_Click( object sender, EventArgs e )
    {
      if ( ( AppState == Types.StudioState.DEBUGGING_BROKEN )
      || ( AppState == Types.StudioState.DEBUGGING_RUN ) )
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
          && ( ( doc != Doc )
          || ( addedMainElement ) ) )
          {
            continue;
          }
          if ( doc == Doc )
          {
            addedMainElement = true;
          }
          foreach ( var deducedDependency in doc.DeducedDependency[doc.Project.Settings.CurrentConfig.Name].BuildState.Keys )
          {
            var element = doc.Project.GetElementByFilename(deducedDependency);
            if ( element != null )
            {
              if ( !tempSet.ContainsValue( element.DocumentInfo ) )
              {
                tempSet.Add( element.DocumentInfo );
                foundElement = true;
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
          StudioCore.Navigating.OpenDocumentAndGotoLine( StudioCore.Debugging.DebuggedProject, documentFile, documentLine );
          StudioCore.Debugging.MarkLine( StudioCore.Debugging.DebuggedProject, documentFile, documentLine );
          return true;
        }
      }
      if ( ( currentMarkedFile != null )
      && ( currentMarkedFile != activeFile ) )
      {
        //Debug.Log( "Try with activefile first" );
        if ( currentMarkedFile.ASMFileInfo.DocumentAndLineFromAddress( CurrentPos, out documentFile, out documentLine ) )
        {
          StudioCore.Navigating.OpenDocumentAndGotoLine( StudioCore.Debugging.DebuggedProject, documentFile, documentLine );
          StudioCore.Debugging.MarkLine( StudioCore.Debugging.DebuggedProject, documentFile, documentLine );
          return true;
        }
      }

      // if any left use the first one
      if ( ( foundMatches.Count > 0 )
      && ( foundMatches[0].ASMFileInfo.DocumentAndLineFromAddress( CurrentPos, out documentFile, out documentLine ) ) )
      {
        //Debug.Log( "use first of left overs: " + foundMatches[0].FullPath );
        StudioCore.Navigating.OpenDocumentAndGotoLine( StudioCore.Debugging.DebuggedProject, documentFile, documentLine );
        StudioCore.Debugging.MarkLine( StudioCore.Debugging.DebuggedProject, documentFile, documentLine );
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

          m_DebugRegisters.SetRegisters( Registers );
          /*
          m_DebugRegisters.SetRegisters( RegisterValues[1], RegisterValues[2], RegisterValues[3], RegisterValues[4],
                                         RegisterValues[7], RegisterValues[0].Substring( 2 ), RegisterValues[8], RegisterValues[9], RegisterValues[6] );
          int currentPos = GR.Convert.ToI32( RegisterValues[0].Substring( 2 ), 16 );*/
          int currentPos = Registers.PC;
          string documentFile = "";
          int documentLine = -1;

          //currentPos = 0x918;   // $918 -  $2cbb
          //currentPos = 0x2430;  // für fehler in Downhill innerhalb speedscroll nested loop
          if ( StudioCore.Debugging.DebuggedASMBase.ASMFileInfo.DocumentAndLineFromAddress( currentPos, out documentFile, out documentLine ) )
          {
            if ( ( StudioCore.Debugging.MarkedDocument == null )
            || ( !GR.Path.IsPathEqual( StudioCore.Debugging.MarkedDocument.DocumentInfo.FullPath, documentFile ) )
            || ( StudioCore.Debugging.MarkedDocumentLine != documentLine ) )
            {
              StudioCore.Navigating.OpenDocumentAndGotoLine( StudioCore.Debugging.DebuggedProject, documentFile, documentLine );
              StudioCore.Debugging.MarkLine( StudioCore.Debugging.DebuggedProject, documentFile, documentLine );
            }
          }
          else
          {
            // try to find info in file in dependency chain
            if ( !FindAndOpenBestMatchForLocation( currentPos ) )
            {
              ShowDisassemblyAt( currentPos );
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
      StudioCore.Debugging.Debugger.RefreshMemory( Address, 32, MemorySource.AS_CPU );
      StudioCore.Debugging.DebugDisassembly.SetText( "Disassembly will\r\nappear here" );

      StudioCore.Debugging.DebugDisassembly.SetCursorToLine( 1, true );

      if ( ( StudioCore.Debugging.MarkedDocument == null )
      || ( !GR.Path.IsPathEqual( StudioCore.Debugging.MarkedDocument.DocumentInfo.FullPath, "C64Studio-intermediatedisassembly" ) )
      || ( StudioCore.Debugging.MarkedDocumentLine != 1 ) )
      {
        if ( StudioCore.Debugging.MarkedDocument != null )
        {
          StudioCore.Debugging.MarkedDocument.SetLineMarked( StudioCore.Debugging.MarkedDocumentLine, false );
        }

        StudioCore.Debugging.MarkedDocument = StudioCore.Debugging.DebugDisassembly;
        StudioCore.Debugging.MarkedDocumentLine = 1;
        StudioCore.Debugging.DebugDisassembly.Select();
        StudioCore.Debugging.DebugDisassembly.SetLineMarked( 1, 1 != -1 );
      }
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
      foreach ( int address in StudioCore.Compiling.ParserASM.ASMFileInfo.AddressToLine.Keys )
      {
        Debug.Log( "Line " + StudioCore.Compiling.ParserASM.ASMFileInfo.AddressToLine[address].ToString() + ": " + address + ", " + StudioCore.Compiling.ParserASM.ASMFileInfo.LineInfo[StudioCore.Compiling.ParserASM.ASMFileInfo.AddressToLine[address]].Line );
      }
      foreach ( Types.ASM.SourceInfo sourceInfo in StudioCore.Compiling.ParserASM.ASMFileInfo.SourceInfo.Values )
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



    public DocumentInfo DetermineDocumentToCompile()
    {
      BaseDocument baseDocToCompile = ActiveContent;
      if ( ( ( baseDocToCompile != null )
      && ( !baseDocToCompile.DocumentInfo.Compilable ) )
      || ( baseDocToCompile == null ) )
      {
        baseDocToCompile = ActiveDocument;
      }
      if ( baseDocToCompile == null )
      {
        return null;
      }

      DocumentInfo docToCompile = baseDocToCompile.DocumentInfo;


      if ( ( docToCompile.Element != null )
      && ( !string.IsNullOrEmpty( docToCompile.Project.Settings.MainDocument ) ) )
      {
        ProjectElement element = docToCompile.Project.GetElementByFilename(docToCompile.Project.Settings.MainDocument);
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



    public void CallHelp( string Keyword )
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
        if ( StudioCore.Compiling.ParserASM.m_Processor.Opcodes.ContainsKey( Keyword.ToLower() ) )
        {
          m_Help.NavigateTo( "aay64h64/AAY64/B" + Keyword.ToUpper() + ".HTM" );
        }
        else if ( StudioCore.Compiling.ParserASM.ASMFileInfo.AssemblerSettings.Macros.ContainsKey( Keyword.ToUpper() ) )
        {
          m_Help.NavigateTo( "asm_macro.html#" + Keyword.Substring( 1 ).ToLower() );
        }
      }
    }



    public bool ApplyFunction( Types.Function Function )
    {
      switch ( Function )
      {
        case Types.Function.FIND_NEXT_MESSAGE:
          {
            StudioCore.Navigating.OpenSourceOfNextMessage();
          }
          break;
        case C64Studio.Types.Function.OPEN_FILES:
          {
            System.Windows.Forms.OpenFileDialog openDlg = new System.Windows.Forms.OpenFileDialog();
            openDlg.Title = "Open existing item";
            openDlg.Filter = FilterString( Types.Constants.FILEFILTER_ALL_SUPPORTED_FILES + Types.Constants.FILEFILTER_ASM + Types.Constants.FILEFILTER_CHARSET + Types.Constants.FILEFILTER_SPRITE + Types.Constants.FILEFILTER_BASIC + Types.Constants.FILEFILTER_BINARY_FILES + Types.Constants.FILEFILTER_ALL );

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
          break;
        case C64Studio.Types.Function.TOGGLE_BREAKPOINT:
          if ( ( AppState != Types.StudioState.NORMAL )
          &&   ( AppState != C64Studio.Types.StudioState.DEBUGGING_BROKEN ) )
          {
            break;
          }
          if ( ActiveDocument is SourceASMEx )
          {
            SourceASMEx asm = (SourceASMEx)ActiveDocument;

            asm.ToggleBreakpoint( asm.CurrentLineIndex );
          }
          break;
        case C64Studio.Types.Function.HELP:
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
            CallHelp( keywordBelow );
          }
          return true;
        case C64Studio.Types.Function.FIND_NEXT:
          m_FindReplace.FindNext( ActiveDocument );
          break;
        case C64Studio.Types.Function.FIND:
          {
            var compilableDoc = ActiveDocumentInfo.CompilableDocument;
            if ( compilableDoc != null )
            {
              m_FindReplace.AdjustSettings( compilableDoc.SourceControl );
            }
          }
          if ( m_FindReplace.Visible )
          {
            m_FindReplace.comboSearchText.Focus();
          }
          else
          {
            m_FindReplace.Show( panelMain );
          }
          StudioCore.Settings.Tools[ToolWindowType.FIND_REPLACE].Visible[m_ActivePerspective] = true;

          m_FindReplace.tabFindReplace.SelectedIndex = 0;
          m_FindReplace.comboSearchTarget.SelectedIndex = 1;
          m_FindReplace.AcceptButton = m_FindReplace.btnFindNext;
          break;
        case C64Studio.Types.Function.FIND_IN_PROJECT:
          {
            var compilableDoc = ActiveDocumentInfo.CompilableDocument;
            if ( compilableDoc != null )
            {
              m_FindReplace.AdjustSettings( compilableDoc.SourceControl );
            }
          }
          if ( m_FindReplace.Visible )
          {
            m_FindReplace.comboSearchText.Focus();
          }
          else
          {
            m_FindReplace.Show( panelMain );
          }
          StudioCore.Settings.Tools[ToolWindowType.FIND_REPLACE].Visible[m_ActivePerspective] = true;
          m_FindReplace.tabFindReplace.SelectedIndex = 0;
          m_FindReplace.comboSearchTarget.SelectedIndex = 3;
          m_FindReplace.AcceptButton = m_FindReplace.btnFindAll;
          break;
        case C64Studio.Types.Function.FIND_REPLACE:
          {
            var compilableDoc = ActiveDocumentInfo.CompilableDocument;
            if ( compilableDoc != null )
            {
              m_FindReplace.AdjustSettings( compilableDoc.SourceControl );
            }
          }
          if ( m_FindReplace.Visible )
          {
            m_FindReplace.comboReplaceSearchText.Focus();
          }
          else
          {
            m_FindReplace.Show( panelMain );
          }
          StudioCore.Settings.Tools[ToolWindowType.FIND_REPLACE].Visible[m_ActivePerspective] = true;
          m_FindReplace.tabFindReplace.SelectedIndex = 1;
          break;
        case C64Studio.Types.Function.REPLACE_IN_PROJECT:
          {
            var compilableDoc = ActiveDocumentInfo.CompilableDocument;
            if ( compilableDoc != null )
            {
              m_FindReplace.AdjustSettings( compilableDoc.SourceControl );
            }
          }
          if ( m_FindReplace.Visible )
          {
            m_FindReplace.comboReplaceSearchText.Focus();
          }
          else
          {
            m_FindReplace.Show( panelMain );
          }
          StudioCore.Settings.Tools[ToolWindowType.FIND_REPLACE].Visible[m_ActivePerspective] = true;
          m_FindReplace.tabFindReplace.SelectedIndex = 1;
          m_FindReplace.comboReplaceTarget.SelectedIndex = 3;
          break;
        case C64Studio.Types.Function.PRINT:
        case C64Studio.Types.Function.COMMENT_SELECTION:
        case C64Studio.Types.Function.UNCOMMENT_SELECTION:
        case Function.COLLAPSE_ALL_FOLDING_BLOCKS:
        case Function.EXPAND_ALL_FOLDING_BLOCKS:
        case Function.JUMP_TO_LINE:
          {
            var curDoc = ActiveDocumentInfo;
            if ( ( curDoc != null )
            &&   ( curDoc.BaseDoc != null )
            &&   ( curDoc.ContainsCode ) )
            {
              curDoc.BaseDoc.ApplyFunction( Function );
            }
          }
          break;
        case C64Studio.Types.Function.CENTER_ON_CURSOR:
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
            }
          }
          break;
        case C64Studio.Types.Function.DEBUG_STEP:
          StudioCore.Debugging.DebugStepInto();
          break;
        case C64Studio.Types.Function.DEBUG_STEP_OVER:
          StudioCore.Debugging.DebugStepOver();
          break;
        case C64Studio.Types.Function.DEBUG_STOP:
          StopDebugging();
          break;
        case C64Studio.Types.Function.DEBUG_GO:
          DebugGo();
          break;
        case C64Studio.Types.Function.DEBUG_BREAK:
          StudioCore.Debugging.DebugBreak();
          break;
        case C64Studio.Types.Function.DEBUG_RUN_TO:
          if ( ( AppState != Types.StudioState.NORMAL )
          &&   ( AppState != C64Studio.Types.StudioState.DEBUGGING_BROKEN ) )
          {
            break;
          }
          {
            DocumentInfo docToDebug = DetermineDocumentToHandle();
            DocumentInfo docToHandle = DetermineDocumentToCompile();
            DocumentInfo docActive = DetermineDocument();

            if ( ( docToDebug.Type != ProjectElement.ElementType.ASM_SOURCE )
            &&   ( docActive.Type != ProjectElement.ElementType.ASM_SOURCE ) )
            {
              break;
            }

            // this potentially starts a task
            EnsureFileIsParsed( docToHandle );

            // so this should become one too!
            AddTask( new Tasks.TaskDebugRunTo( docToHandle, docToDebug, docActive ) );

          }
          break;
        case C64Studio.Types.Function.SAVE_ALL:
          SaveSolution();
          if ( m_Solution != null )
          {
            foreach ( Project project in m_Solution.Projects )
            {
              SaveProject( project );
            }
            foreach ( Project project in m_Solution.Projects )
            {
              foreach ( ProjectElement element in project.Elements )
              {
                if ( element.Document != null )
                {
                  element.Document.Save();
                }
              }
            }
            // elements saving could have changed project settings, so save again
            foreach ( Project project in m_Solution.Projects )
            {
              SaveProject( project );
            }
            // save all changed non project files!
            foreach ( BaseDocument doc in panelMain.Documents )
            {
              if ( ( doc.DocumentInfo.Element == null )
              &&   ( doc.Modified ) )
              {
                doc.Save();
              }
            }
          }
          else
          {
            BaseDocument docToSave = ActiveContent;
            if ( docToSave != null )
            {
              docToSave.Save();
            }
          }
          break;
        case C64Studio.Types.Function.SAVE_DOCUMENT:
          {
            // save current document
            BaseDocument docToSave = ActiveContent;
            if ( ( docToSave != null )
            && ( !docToSave.IsSaveable ) )
            {
              docToSave = ActiveDocument;
            }
            if ( ( docToSave == null )
            || ( !docToSave.IsSaveable ) )
            {
              break;
            }

            if ( docToSave.DocumentInfo.Project == null )
            {
              docToSave.Save();
              return true;
            }

            if ( ( docToSave.DocumentInfo.Project == null )
            || ( docToSave.DocumentInfo.Project.Settings.BasePath == null )
            || ( docToSave.DocumentInfo.Element == null ) )
            {
              // no project yet (or no project element)
              if ( !SaveProject( docToSave.DocumentInfo.Project ) )
              {
                return true;
              }
            }
            docToSave.Save();
            if ( !SaveProject( docToSave.DocumentInfo.Project ) )
            {
              return true;
            }
          }
          break;
        case C64Studio.Types.Function.SAVE_DOCUMENT_AS:
          {
            // save current document as
            BaseDocument docToSave = ActiveContent;
            if ( ( docToSave != null )
            && ( !docToSave.IsSaveable ) )
            {
              docToSave = ActiveDocument;
            }
            if ( ( docToSave == null )
            || ( !docToSave.IsSaveable ) )
            {
              break;
            }

            if ( docToSave.DocumentInfo.Project == null )
            {
              docToSave.SaveAs();
              return true;
            }

            if ( ( docToSave.DocumentInfo.Project == null )
            || ( docToSave.DocumentInfo.Project.Settings.BasePath == null )
            || ( docToSave.DocumentInfo.Element == null ) )
            {
              // no project yet (or no project element)
              if ( !SaveProject( docToSave.DocumentInfo.Project ) )
              {
                return true;
              }
            }
            docToSave.SaveAs();
            if ( !SaveProject( docToSave.DocumentInfo.Project ) )
            {
              return true;
            }
          }
          break;
        case C64Studio.Types.Function.COMPILE:
          {
            DocumentInfo docToCompile = DetermineDocumentToCompile();
            if ( docToCompile != null )
            {
              Compile( docToCompile );
            }
          }
          break;
        case C64Studio.Types.Function.BUILD:
          {
            DocumentInfo docToCompile = DetermineDocumentToCompile();
            if ( docToCompile != null )
            {
              Build( docToCompile );
            }
          }
          break;
        case Function.BUILD_TO_PREPROCESSED_FILE:
          {
            DocumentInfo docToCompile = DetermineDocumentToCompile();
            if ( docToCompile != null )
            {
              Build( docToCompile, true );
            }
          }
          break;
        case C64Studio.Types.Function.REBUILD:
          {
            DocumentInfo docToCompile = DetermineDocumentToCompile();
            if ( docToCompile != null )
            {
              Rebuild( docToCompile );
            }
          }
          break;
        case C64Studio.Types.Function.BUILD_AND_RUN:
          {
            DocumentInfo docToCompile = DetermineDocumentToCompile();
            if ( docToCompile != null )
            {
              BuildAndRun( docToCompile, docToCompile );
            }
          }
          break;
        case C64Studio.Types.Function.BUILD_AND_DEBUG:
          if ( AppState == Types.StudioState.NORMAL )
          {
            DocumentInfo docToCompile = DetermineDocumentToCompile();
            if ( docToCompile != null )
            {
              BuildAndDebug( docToCompile, DetermineDocumentToHandle(), docToCompile );
            }
          }
          else if ( AppState == Types.StudioState.DEBUGGING_BROKEN )
          {
            DebugGo();
          }
          break;
        case C64Studio.Types.Function.GO_TO_DECLARATION:
          {
            DocumentInfo docToDebug = DetermineDocumentToCompile();
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
              string wordBelow = sourceEx.FindWordAtCaretPosition();
              string zone;
              string cheapLabelParent;

              sourceEx.FindZoneAtCaretPosition( out zone, out cheapLabelParent );
              StudioCore.Navigating.GotoDeclaration( docToHandle, wordBelow, zone, cheapLabelParent );
            }
          }
          break;
        case C64Studio.Types.Function.UNDO:
          BaseDocument docUndo = ActiveDocument;
          if ( ( docUndo != null )
          &&   ( docUndo.UndoPossible ) )
          {
            docUndo.Undo();
          }
          break;
        case C64Studio.Types.Function.REDO:
          BaseDocument docRedo = ActiveDocument;
          if ( ( docRedo != null )
          &&   ( docRedo.RedoPossible ) )
          {
            docRedo.Redo();
          }
          break;
        case C64Studio.Types.Function.DELETE_LINE:
        case C64Studio.Types.Function.COPY_LINE_UP:
        case C64Studio.Types.Function.COPY_LINE_DOWN:
        case C64Studio.Types.Function.MOVE_LINE_DOWN:
        case C64Studio.Types.Function.MOVE_LINE_UP:
          // let control handle it
          return false;
        case Function.COPY:
          if ( !ActiveContent.CopyPossible )
          {
            return false;
          }
          ActiveContent.Copy();
          break;
        case Function.PASTE:
          if ( !ActiveContent.PastePossible )
          {
            return false;
          }
          ActiveContent.Paste();
          break;
        case Function.CUT:
          if ( !ActiveContent.CutPossible )
          {
            return false;
          }
          ActiveContent.Cut();
          break;
      }
      return true;
    }



    public bool HandleCmdKey( ref Message msg, Keys keyData )
    {
      AcceleratorKey usedAccelerator = StudioCore.Settings.DetermineAccelerator(keyData, AppState);
      if ( usedAccelerator != null )
      {
        return ApplyFunction( usedAccelerator.Function );
      }
      return false;
    }



    protected override bool ProcessCmdKey( ref Message msg, Keys keyData )
    {
      AcceleratorKey usedAccelerator = StudioCore.Settings.DetermineAccelerator(keyData, AppState);
      if ( usedAccelerator != null )
      {
        return ApplyFunction( usedAccelerator.Function );
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
      || ( AppState == Types.StudioState.DEBUGGING_BROKEN )
      || ( AppState == Types.StudioState.NORMAL ) )
      {
        m_DebugWatch.AddWatchEntry( Watch );
        StudioCore.Debugging.Debugger.AddWatchEntry( Watch );

        if ( AppState == Types.StudioState.DEBUGGING_BROKEN )
        {
          StudioCore.Debugging.Debugger.RefreshRegistersAndWatches();
        }
      }
    }



    public void RemoveWatchEntry( WatchEntry Watch )
    {
      m_DebugWatch.RemoveWatchEntry( Watch );
      StudioCore.Debugging.Debugger.RemoveWatchEntry( Watch );
    }



    private void UpdateWatchInfo( VICERemoteDebugger.RequestData Request, GR.Memory.ByteBuffer Data )
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
        if ( Request.Type == VICERemoteDebugger.Request.MEM_DUMP )
        {
          // display disassembly?
          if ( Request.Parameter1 == StudioCore.Debugging.CurrentCodePosition )
          {
            if ( StudioCore.Debugging.DebugDisassembly != null )
            {
              // update disassembly
              Parser.Disassembler disassembler = new C64Studio.Parser.Disassembler(Tiny64.Processor.Create6510());
              string disassembly = "";

              disassembler.SetData( Data );

              GR.Collections.Set<int> jumpedAtAddresses = new GR.Collections.Set<int>();
              jumpedAtAddresses.Add( StudioCore.Debugging.CurrentCodePosition );
              GR.Collections.Map<int, string> namedLabels = new GR.Collections.Map<int, string>();
              if ( disassembler.Disassemble( StudioCore.Debugging.CurrentCodePosition, jumpedAtAddresses, namedLabels, true, out disassembly ) )
              {
                StudioCore.Debugging.DebugDisassembly.SetText( disassembly );
                StudioCore.Debugging.MarkedDocument.SetLineMarked( StudioCore.Debugging.MarkedDocumentLine, false );

                StudioCore.Debugging.MarkedDocument = StudioCore.Debugging.DebugDisassembly;
                StudioCore.Debugging.MarkedDocumentLine = 1;
                StudioCore.Debugging.DebugDisassembly.Select();
                StudioCore.Debugging.DebugDisassembly.SetLineMarked( 1, 1 != -1 );
              }
              else
              {
                if ( StudioCore.Debugging.MarkedDocument != null )
                {
                  StudioCore.Debugging.MarkLine( StudioCore.Debugging.MarkedDocument.DocumentInfo.Project, StudioCore.Debugging.MarkedDocument.DocumentInfo.FullPath, -1 );
                  StudioCore.Debugging.MarkedDocument = null;
                }
                StudioCore.Debugging.DebugDisassembly.SetText( "Disassembly\r\nfailed\r\n" + disassembly );
              }
            }
          }
        }

        if ( Request.Info == "C64Studio.MemDump" )
        {
          m_DebugMemory.UpdateMemory( Request, Data );
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



    public bool ParseFile( Parser.ParserBase Parser, DocumentInfo Document, ProjectConfig Configuration, bool OutputMessages, bool CreatePreProcessedFile )
    {
      //Debug.Log( "Parsefile called for " + Document.DocumentFilename );
      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.Assembler = Types.AssemblerType.AUTO;
      if ( Document.Element != null )
      {
        config.Assembler = Document.Element.AssemblerType;
      }
      config.AutoTruncateLiteralValues  = StudioCore.Settings.ASMAutoTruncateLiteralValues;
      config.CreatePreProcesseFile      = CreatePreProcessedFile;
      config.LibraryFiles               = StudioCore.Settings.ASMLibraryPaths;
      config.InputFile                  = Document.FullPath;

      string sourceCode = "";

      if ( Document.BaseDoc != null )
      {
        if ( ( Document.Type == ProjectElement.ElementType.ASM_SOURCE )
        ||   ( Document.Type == ProjectElement.ElementType.BASIC_SOURCE ) )
        {
          sourceCode = Document.BaseDoc.GetContent();
        }
      }

      bool result = Parser.ParseFile(Document.FullPath, sourceCode, Configuration, config);

      if ( ( config.Assembler != C64Studio.Types.AssemblerType.AUTO )
      &&   ( Document.BaseDoc != null )
      &&   ( Document.Element != null ) )
      {
        if ( Document.Element.AssemblerType != config.Assembler )
        {
          Document.Element.AssemblerType = config.Assembler;
          Document.BaseDoc.SetModified();
        }
      }

      if ( Document.Type == ProjectElement.ElementType.ASM_SOURCE )
      {
        C64Studio.Parser.ASMFileParser asmParser = (C64Studio.Parser.ASMFileParser)Parser;

        Document.ASMFileInfo = asmParser.ASMFileInfo;
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
      }

      if ( Document.Element != null )
      {
        Document.Element.CompileTarget = Parser.CompileTarget;
        Document.Element.CompileTargetFile = Parser.CompileTargetFile;
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
          buildState.BuildState.Add( dependency, lastChangeTime );
        }
      }

      if ( Document.Type == ProjectElement.ElementType.ASM_SOURCE )
      {
        SourceASMEx asm = Document.BaseDoc as SourceASMEx;
        if ( asm != null )
        {
          asm.DoNotFollowZoneSelectors = true;
        }

        if ( ( Document.Project != null )
        &&   ( !string.IsNullOrEmpty( Document.Project.Settings.MainDocument ) )
        &&   ( System.IO.Path.GetFileName( Document.FullPath ) == Document.Project.Settings.MainDocument ) )
        {
          // give all other files the same keywords!
          var knownTokens = StudioCore.Compiling.ParserASM.KnownTokens();
          GR.Collections.MultiMap<string, C64Studio.Types.SymbolInfo> knownTokenInfos = StudioCore.Compiling.ParserASM.KnownTokenInfo();

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
              &&   ( element2.DocumentInfo.Type == ProjectElement.ElementType.ASM_SOURCE ) )
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
              elementToUpdate.DocumentInfo.KnownKeywords = knownTokens;
              elementToUpdate.DocumentInfo.KnownTokens = knownTokenInfos;
              if ( elementToUpdate.Document != null )
              {
                AddTask( new Tasks.TaskUpdateKeywords( elementToUpdate.Document ) );
              }
            }
          }

          m_DebugBreakpoints.SetTokens( knownTokenInfos );
        }
        else
        {
          if ( Document != null )
          {
            Document.KnownKeywords = StudioCore.Compiling.ParserASM.KnownTokens();
            Document.KnownTokens = StudioCore.Compiling.ParserASM.KnownTokenInfo();
          }

          if ( !IsDocPartOfMainDocument( Document ) )
          {
            m_DebugBreakpoints.SetTokens( StudioCore.Compiling.ParserASM.KnownTokenInfo() );
          }
        }

        if ( asm != null )
        {
          asm.DoNotFollowZoneSelectors = false;
        }
      }

      if ( OutputMessages )
      {
        AddTask( new Tasks.TaskUpdateCompileResult( Parser, Document ) );
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
      && ( !Document.BaseDoc.FileParsed ) )
      || ( StudioCore.Compiling.NeedsRebuild( Document ) ) )
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
          config = Document.Project.Settings.Configs[mainToolConfig.SelectedItem.ToString()];
        }
        ParseFile( StudioCore.DetermineParser( Document ), Document, config, false, false );
      }
    }



    private void MainForm_FormClosing( object sender, FormClosingEventArgs e )
    {
      if ( e.Cancel )
      {
        return;
      }
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
      if ( m_Solution != null )
      {
        foreach ( Project project in m_Solution.Projects )
        {
          if ( ( project != null )
          && ( project.Modified ) )
          {
            DialogResult result = System.Windows.Forms.MessageBox.Show("The project " + project.Settings.Name + " has unsaved changes, save now?", "Save Project?", MessageBoxButtons.YesNoCancel);
            if ( result == DialogResult.Cancel )
            {
              e.Cancel = true;
              return;
            }
            e.Cancel = false;
            if ( result == DialogResult.Yes )
            {
              project.Save( project.Settings.Filename );
            }
          }
          else
          {
            e.Cancel = false;
          }
        }
      }
      SaveSettings();
      StudioCore.ShuttingDown = true;
    }



    private void aboutToolStripMenuItem1_Click( object sender, EventArgs e )
    {
      FormAbout about = new FormAbout();

      about.ShowDialog();
    }



    private void fileNewProjectToolStripMenuItem_Click( object sender, EventArgs e )
    {
      CreateNewProject();
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
      FormWizard wizard = new FormWizard();

      if ( wizard.ShowDialog() == DialogResult.OK )
      {
        /*
        ToolInfo      toolAssembler = new ToolInfo();

        toolAssembler.Name          = "ACME Assembler";
        toolAssembler.Filename      = wizard.editPathACME.Text;
        toolAssembler.Arguments     = "\"$(Filename)\"";
        toolAssembler.WorkPath      = "\"$(FilePath)\"";
        toolAssembler.Type          = ToolInfo.ToolType.ASSEMBLER;

        Settings.Tools.AddLast( toolAssembler );
         */

        ToolInfo toolEmulator = new ToolInfo();

        toolEmulator.Name = "WinVICE";
        toolEmulator.Filename = wizard.editPathVice.Text;
        toolEmulator.PRGArguments = "\"$(RunFilename)\"";
        toolEmulator.CartArguments = "-cartcrt \"$(RunFilename)\"";
        toolEmulator.WorkPath = "\"$(RunPath)\"";
        toolEmulator.DebugArguments = "-initbreak 0x$(DebugStartAddressHex) -remotemonitor";
        toolEmulator.TrueDriveOnArguments = "-truedrive +virtualdev";
        toolEmulator.TrueDriveOffArguments = "+truedrive -virtualdev";
        toolEmulator.Type = ToolInfo.ToolType.EMULATOR;

        StudioCore.Settings.ToolInfos.Add( toolEmulator );
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

      foreach ( string file in fileList )
      {
        OpenFile( file );
      }
    }



    public void CloseSolution()
    {
      if ( m_Solution != null )
      {
        CloseAllProjects();
        m_Solution.Projects.Clear();
        m_Solution = null;
        StudioCore.Settings.LastSolutionWasEmpty = true;

        // clear entries
        m_DebugWatch.ClearAllWatchEntries();
        StudioCore.Debugging.Debugger.ClearAllWatchEntries();
        StudioCore.Debugging.Debugger.ClearAllBreakpoints();

        RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.SOLUTION_CLOSED ) );
      }
    }



    public bool OpenSolution( string Filename )
    {
      CloseSolution();

      //AddTask( new C64Studio.Tasks.TaskOpenSolution( Filename ) );
      m_Solution = new Solution( this );

      GR.Memory.ByteBuffer solutionData = GR.IO.File.ReadAllBytes(Filename);
      if ( solutionData == null )
      {
        m_Solution = null;
        return false;
      }
      if ( !m_Solution.FromBuffer( solutionData, Filename ) )
      {
        StudioCore.Settings.RemoveFromMRU( StudioCore.Settings.MRUProjects, Filename, this );
        CloseSolution();
        m_Solution = null;
        return false;
      }
      StudioCore.Settings.UpdateInMRU( StudioCore.Settings.MRUProjects, Filename, this );
      StudioCore.Settings.LastSolutionWasEmpty = false;

      m_Solution.Modified = false;

      RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.SOLUTION_OPENED ) );
      return true;
    }



    public void SaveSolution()
    {
      if ( m_Solution == null )
      {
        return;
      }
      if ( string.IsNullOrEmpty( m_Solution.Filename ) )
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
        m_Solution.Filename = saveDlg.FileName;
      }
      GR.IO.File.WriteAllBytes( m_Solution.Filename, m_Solution.ToBuffer( m_Solution.Filename ) );
      m_Solution.Modified = false;
      StudioCore.Settings.UpdateInMRU( StudioCore.Settings.MRUProjects, m_Solution.Filename, this );
    }



    public BaseDocument OpenFile( string Filename )
    {
      BaseDocument document = null;

      string extension = System.IO.Path.GetExtension(Filename).ToUpper();
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
      else if ( ( extension == ".D64" )
      ||        ( extension == ".D71" )
      ||        ( extension == ".D81" )
      ||        ( extension == ".T64" )
      ||        ( extension == ".PRG" ) )
      {
        document = new FileManager( StudioCore, Filename );
        document.ShowHint = DockState.Float;
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
      else if ( extension == ".BAS" )
      {
        document = new SourceBasicEx( StudioCore );
        document.ShowHint = DockState.Document;
      }
      else if ( extension == ".MAPPROJECT" )
      {
        document = new MapEditor( StudioCore );
        document.ShowHint = DockState.Document;
      }
      else
      {
        document = new SourceASMEx( StudioCore );
        document.ShowHint = DockState.Document;
      }

      document.Core = StudioCore;
      document.SetDocumentFilename( Filename );
      document.Text = System.IO.Path.GetFileName( Filename );
      if ( !document.Load() )
      {
        return null;
      }

      document.Show( panelMain );
      document.Icon = IconFromType( document.DocumentInfo );
      document.DocumentInfo.UndoManager.MainForm = this;

      StudioCore.Settings.UpdateInMRU( StudioCore.Settings.MRUFiles, Filename, this );

      RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.DOCUMENT_INFO_CREATED, document.DocumentInfo ) );

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
          return C64Studio.Properties.Resources.source;
        case ProjectElement.ElementType.BASIC_SOURCE:
          return C64Studio.Properties.Resources.source_basic;
        case ProjectElement.ElementType.CHARACTER_SCREEN:
          return C64Studio.Properties.Resources.charsetscreen;
        case ProjectElement.ElementType.CHARACTER_SET:
          return C64Studio.Properties.Resources.charset;
        case ProjectElement.ElementType.FOLDER:
          return C64Studio.Properties.Resources.folder;
        case ProjectElement.ElementType.GRAPHIC_SCREEN:
          return C64Studio.Properties.Resources.graphicscreen;
        case ProjectElement.ElementType.MAP_EDITOR:
          return C64Studio.Properties.Resources.mapeditor;
        case ProjectElement.ElementType.PROJECT:
          return C64Studio.Properties.Resources.project;
        case ProjectElement.ElementType.SOLUTION:
          return C64Studio.Properties.Resources.solution;
        case ProjectElement.ElementType.SPRITE_SET:
          return C64Studio.Properties.Resources.spriteset;
        case ProjectElement.ElementType.BINARY_FILE:
          return C64Studio.Properties.Resources.binary;
        case ProjectElement.ElementType.VALUE_TABLE:
          return C64Studio.Properties.Resources.valuetable;
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
          newDoc = new Disassembler( StudioCore );
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



    public void AddNewElement( ProjectElement.ElementType Type, string Description, Project ParentProject, TreeNode ParentNode )
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
        el.Document.Save();
      }
    }



    public void MainForm_DragEnter( object sender, DragEventArgs e )
    {
      e.Effect = DragDropEffects.All;
    }



    public void AddTask( Tasks.Task Task )
    {
      //Debug.Log( "Add task " + Task.ToString() );
      Task.Core = StudioCore;
      StudioCore.TaskManager.AddTask( Task );
    }



    void Task_TaskFinished( C64Studio.Tasks.Task FinishedTask )
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
        case C64Studio.Tasks.Task.TaskType.PARSE_FILE:
          break;
        case C64Studio.Tasks.Task.TaskType.OPEN_SOLUTION:
          {
            var taskOS = (C64Studio.Tasks.TaskOpenSolution)FinishedTask;

            if ( !FinishedTask.TaskSuccessful )
            {
              StudioCore.Settings.RemoveFromMRU( StudioCore.Settings.MRUProjects, taskOS.SolutionFilename, this );
              CloseSolution();
            }
            else
            {
              StudioCore.Settings.UpdateInMRU( StudioCore.Settings.MRUProjects, taskOS.SolutionFilename, this );
              m_Solution = taskOS.Solution;
              m_Solution.Modified = false;
              RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.SOLUTION_OPENED ) );
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
          System.Threading.Thread workerThread = new System.Threading.Thread(new System.Threading.ThreadStart(m_CurrentTask.RunTask));

          StudioCore.SetStatus( m_CurrentTask.Description, true, 0 );

          workerThread.Start();
        }
      }
    }



    private void mainToolConfig_SelectedIndexChanged( object sender, EventArgs e )
    {
      m_CurrentProject.Settings.CurrentConfig = m_CurrentProject.Settings.Configs[mainToolConfig.SelectedItem.ToString()];

      ProjectConfigChanged();
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
      if ( InvokeRequired )
      {
        Invoke( new ParameterLessCallback( UpdateUndoSettings ) );
      }
      else
      {
        mainToolUndo.Enabled = ActiveDocument.UndoPossible;
        mainToolRedo.Enabled = ActiveDocument.RedoPossible;
        undoToolStripMenuItem.Enabled = ActiveDocument.UndoPossible;
        redoToolStripMenuItem.Enabled = ActiveDocument.RedoPossible;

        copyToolStripMenuItem.Enabled = ActiveDocument.CopyPossible;
        cutToolStripMenuItem.Enabled = ActiveDocument.CutPossible;
        pasteToolStripMenuItem.Enabled = ActiveDocument.PastePossible;
        deleteToolStripMenuItem.Enabled = ActiveDocument.DeletePossible;


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
      ApplyFunction( C64Studio.Types.Function.COMPILE );
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
        if ( System.Windows.Forms.MessageBox.Show( this, "The file " + Doc.DocumentInfo.FullPath + " has changed externally. Do you want to reload the file?", "Reload externally changed file?", MessageBoxButtons.YesNo ) == DialogResult.Yes )
        {
          int cursorLine = Doc.CursorLine;
          Doc.Load();
          Doc.SetModified();
          Doc.SetCursorToLine( cursorLine, true );
        }
        m_ExternallyChangedDocuments.Remove( Doc );
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
      document.Load();
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
      ApplyFunction( C64Studio.Types.Function.HELP );
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
      document.Load();
      document.Show( panelMain );
    }



    private void fileOpenToolStripMenuItem_Click( object sender, EventArgs e )
    {
      ApplyFunction( C64Studio.Types.Function.OPEN_FILES );
    }



    private void searchToolStripMenuItem_Click( object sender, EventArgs e )
    {
      ApplyFunction( C64Studio.Types.Function.FIND );
    }



    private void findReplaceToolStripMenuItem_Click( object sender, EventArgs e )
    {
      ApplyFunction( C64Studio.Types.Function.FIND_REPLACE );
    }



    private void mainToolFind_Click( object sender, EventArgs e )
    {
      ApplyFunction( C64Studio.Types.Function.FIND );
    }



    private void mainToolFindReplace_Click( object sender, EventArgs e )
    {
      ApplyFunction( C64Studio.Types.Function.FIND_REPLACE );
    }



    private void mainToolPrint_Click( object sender, EventArgs e )
    {
      ApplyFunction( C64Studio.Types.Function.PRINT );
    }



    private void mainToolSaveAll_Click( object sender, EventArgs e )
    {
      ApplyFunction( C64Studio.Types.Function.SAVE_ALL );
    }



    private void saveToolStripMenuItem_Click( object sender, EventArgs e )
    {
      ApplyFunction( C64Studio.Types.Function.SAVE_DOCUMENT );
    }



    private void saveAsToolStripMenuItem_Click( object sender, EventArgs e )
    {
      ApplyFunction( C64Studio.Types.Function.SAVE_DOCUMENT_AS );
    }



    private void saveAllToolStripMenuItem_Click( object sender, EventArgs e )
    {
      ApplyFunction( C64Studio.Types.Function.SAVE_ALL );
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
      document.Load();
      document.Show( panelMain );
    }



    private void dumpLabelsToolStripMenuItem_Click( object sender, EventArgs e )
    {
      DumpPanes( panelMain, "" );
    }



    private void DumpElementHierarchy( TreeNode Node, string Indent )
    {
      Project project = m_SolutionExplorer.ProjectFromNode(Node);
      ProjectElement element = m_SolutionExplorer.ElementFromNode(Node);
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
        foreach ( TreeNode subNode in Node.Nodes )
        {
          DumpElementHierarchy( subNode, Indent + " " );
        }
      }
    }



    private void dumpHierarchyToolStripMenuItem_Click( object sender, EventArgs e )
    {
      foreach ( TreeNode node in m_SolutionExplorer.treeProject.Nodes )
      {
        DumpElementHierarchy( node, "" );
      }
      Debug.Log( "by project:" );
      foreach ( TreeNode node in m_SolutionExplorer.treeProject.Nodes )
      {
        Project project = (Project)node.Tag;
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



    private void AddNewProjectAndOrSolution()
    {
      if ( m_Solution == null )
      {
        FormSolutionWizard solWizard = new FormSolutionWizard( "New Solution", StudioCore.Settings );
        if ( solWizard.ShowDialog() == DialogResult.OK )
        {
          try
          {
            System.IO.Directory.CreateDirectory( solWizard.SolutionPath );
          }
          catch ( System.Exception ex )
          {
            System.Windows.Forms.MessageBox.Show( "Could not create solution folder:" + System.Environment.NewLine + ex.Message, "Could not create solution folder" );
            return;
          }

          m_Solution = new Solution( this );
          m_Solution.Name = solWizard.SolutionName;
          m_Solution.Filename = solWizard.SolutionFilename;

          Project newProject = new Project();
          newProject.Core = StudioCore;
          newProject.Settings.Name = solWizard.SolutionName;
          newProject.Settings.Filename = solWizard.ProjectFilename;
          newProject.Settings.BasePath = System.IO.Path.GetDirectoryName( newProject.Settings.Filename );
          newProject.Node = new TreeNode();
          newProject.Node.Tag = newProject;
          newProject.Node.Text = newProject.Settings.Name;

          Text += " - " + newProject.Settings.Name;

          m_Solution.Projects.Add( newProject );

          m_SolutionExplorer.treeProject.Nodes.Add( newProject.Node );

          RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.SOLUTION_OPENED ) );

          SetActiveProject( newProject );
          projectToolStripMenuItem.Visible = true;

          SaveSolution();
          SaveProject( newProject );
          UpdateUndoSettings();
        }
      }
      else
      {
        AddNewProject();
      }
    }



    private void solutionSaveToolStripMenuItem1_Click( object sender, EventArgs e )
    {
      SaveSolution();
    }



    public bool ImportImage( string Filename, GR.Image.FastImage IncomingImage, Types.GraphicType ImportType, Types.MulticolorSettings MCSettings, out GR.Image.FastImage MappedImage, out Types.MulticolorSettings NewMCSettings, out bool PasteAsBlock )
    {
      PasteAsBlock = false;

      // shortcut possible? (check if palette matches ours)
      if ( IncomingImage == null )
      {
        IncomingImage = StudioCore.Imaging.LoadImageFromFile( Filename );
      }

      MappedImage = null;
      if ( IncomingImage.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed )
      {
        // match palette
        bool match = true;
        for ( int i = 0; i < 16; ++i )
        {
          if ( ( IncomingImage.PaletteRed( i ) != ( ( Types.ConstantData.Palette.ColorValues[i] & 0xff0000 ) >> 16 ) )
          ||   ( IncomingImage.PaletteGreen( i ) != ( ( Types.ConstantData.Palette.ColorValues[i] & 0xff00 ) >> 8 ) )
          ||   ( IncomingImage.PaletteBlue( i ) != ( ( Types.ConstantData.Palette.ColorValues[i] & 0xff ) ) ) )
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

      DlgGraphicImport importGFX = new DlgGraphicImport(StudioCore, ImportType, IncomingImage, Filename, MCSettings);
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
      FormLicense form = new FormLicense();

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

      //Debug.Log( data.ToString() );
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
      UnitTests.TestManager manager = new C64Studio.UnitTests.TestManager(this);

      manager.RunTests();
    }



    private void fileNewSolutionToolStripMenuItem_Click( object sender, EventArgs e )
    {
      CloseSolution();
      AddNewProjectAndOrSolution();
    }



    private void mainToolRebuild_Click( object sender, EventArgs e )
    {
      ApplyFunction( C64Studio.Types.Function.REBUILD );
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
      ApplyFunction( C64Studio.Types.Function.OPEN_FILES );
    }



    private void mainToolCommentSelection_Click( object sender, EventArgs e )
    {
      ApplyFunction( C64Studio.Types.Function.COMMENT_SELECTION );
    }



    private void mainToolUncommentSelection_Click( object sender, EventArgs e )
    {
      ApplyFunction( C64Studio.Types.Function.UNCOMMENT_SELECTION );
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
      ApplyFunction( C64Studio.Types.Function.COMPILE );
    }


    private void buildToolStripMenuItem1_Click( object sender, EventArgs e )
    {
      ApplyFunction( C64Studio.Types.Function.BUILD );
    }


    private void rebuildToolStripMenuItem_Click( object sender, EventArgs e )
    {
      ApplyFunction( C64Studio.Types.Function.REBUILD );
    }


    private void buildandRunToolStripMenuItem_Click( object sender, EventArgs e )
    {
      ApplyFunction( C64Studio.Types.Function.BUILD_AND_RUN );
    }


    private void debugToolStripMenuItem1_Click( object sender, EventArgs e )
    {
      ApplyFunction( C64Studio.Types.Function.BUILD_AND_DEBUG );
    }



    private void preprocessedFileToolStripMenuItem_Click( object sender, EventArgs e )
    {
      ApplyFunction( C64Studio.Types.Function.BUILD_TO_PREPROCESSED_FILE );
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

  }
}
