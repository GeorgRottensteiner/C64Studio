using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.ComponentModel;
using GR.Memory;
using C64Studio.Types;
using System.Drawing;

namespace C64Studio
{
  public enum Perspective
  {
    EDIT = 0,
    DEBUG = 1
  };

  public enum AppMode
  {
    UNDECIDED     = 0,
    GOOD_APP      = 1,
    PORTABLE_APP  = 2
  };

  public enum ToolWindowType
  {
    UNKNOWN,
    [Description( "Outline" )]
    OUTLINE,
    [Description( "Solution Explorer" )]
    SOLUTION_EXPLORER,
    [Description( "Output" )]
    OUTPUT,
    [Description( "CompileResult" )]
    COMPILE_RESULT,
    [Description( "Breakpoints" )]
    DEBUG_BREAKPOINTS,
    [Description( "Charset Editor" )]
    DEBUG_WATCH,
    [Description( "Memory" )]
    DEBUG_MEMORY,
    [Description( "Registers" )]
    DEBUG_REGISTERS,
    [Description( "Watch" )]
    CHARSET_EDITOR,
    [Description( "Sprite Editor" )]
    SPRITE_EDITOR,
    [Description( "Text Screen Editor" )]
    CHAR_SCREEN_EDITOR,
    [Description( "Graphic Screen Editor" )]
    GRAPHIC_SCREEN_EDITOR,
    [Description( "Map Editor" )]
    MAP_EDITOR,
    [Description( "PetSCII" )]
    PETSCII_TABLE,
    [Description( "Calculator" )]
    CALCULATOR,
    [Description( "Help" )]
    HELP,
    [Description( "Find/Replace" )]
    FIND_REPLACE,
    [Description( "Search Results" )]
    SEARCH_RESULTS,
    [Description( "Disassembler" )]
    DISASSEMBLER,
    [Description( "Binary Editor" )]
    BINARY_EDITOR,
    [Description( "Value Table Editor" )]
    VALUE_TABLE_EDITOR
  };

  public enum MemoryDisplayType
  {
    ASCII,
    CHARSET,
    SPRITES
  };

  public enum MemorySourceType
  {
    CPU,
    RAM
  };

  public class ToolWindow
  {
    public ToolWindowType     Type = ToolWindowType.UNKNOWN;
    public BaseDocument       Document = null;
    public ToolStripMenuItem  MenuItem = null;
    public string             ToolDescription = "";
    public GR.Collections.Map<Perspective,bool> Visible = new GR.Collections.Map<Perspective, bool>();
  };

  public class KeymapEntry
  {
    public Types.KeyboardKey  KeyboardKey = C64Studio.Types.KeyboardKey.UNDEFINED;
    public System.Windows.Forms.Keys Key = System.Windows.Forms.Keys.None;
  };


  public class StudioSettings
  {
    public GR.Collections.MultiMap<Keys, AcceleratorKey> Accelerators = new GR.Collections.MultiMap<Keys, AcceleratorKey>();

    public List<ToolInfo>                       ToolInfos = new List<ToolInfo>();

    public List<string>                         MRUProjects = new List<string>();
    public List<string>                         MRUFiles = new List<string>();

    public Dictionary<string,BaseDocument>      GenericTools = new Dictionary<string, BaseDocument>();

    public bool                                 PlaySoundOnSuccessfulBuild = true;
    public bool                                 PlaySoundOnBuildFailure = true;
    public bool                                 PlaySoundOnSearchFoundNoItem = true;
    public string                               MainWindowPlacement = "";
    public bool                                 TrueDriveEnabled = true;
    public string                               EmulatorToRun = "";
    public bool                                 AutoOpenLastSolution = false;
    public bool                                 LastSolutionWasEmpty = false;

    public int                                  TabSize             = 2;
    public bool                                 TabConvertToSpaces  = true;
    public bool                                 AllowTabs           = true;
    public bool                                 ASMHideLineNumbers  = false;
    public bool                                 ASMShowBytes        = true;
    public bool                                 ASMShowCycles       = true;
    public bool                                 ASMShowMiniView     = true;
    public bool                                 ASMAutoTruncateLiteralValues = false;
    public bool                                 ASMShowAutoComplete = true;
    public bool                                 ASMShowAddress      = true;
    public List<string>                         ASMLibraryPaths     = new List<string>();

    public bool                                 StripTrailingSpaces = false;

    public bool                                 OutlineShowLocalLabels = true;
    public bool                                 OutlineShowShortCutLabels = true;
    public bool                                 OutlineSortByIndex = true;
    public bool                                 OutlineSortByAlphabet = false;
    public string                               OutlineFilter = "";

    public bool                                 ToolbarActiveMain = true;
    public bool                                 ToolbarActiveDebugger = true;

    public int                                  MRUMaxCount = 4;

    public string                               SourceFontFamily = "Consolas";
    public float                                SourceFontSize = 9.0f;
    public FontStyle                            SourceFontStyle = FontStyle.Regular;
    public string                               BASICSourceFontFamily = "Consolas";
    public float                                BASICSourceFontSize = 9.0f;
    public FontStyle                            BASICSourceFontStyle = FontStyle.Regular;
    public bool                                 BASICUseNonC64Font = false;

    public List<string>                         FindArguments = new List<string>();
    public List<string>                         ReplaceArguments = new List<string>();
    public bool                                 LastFindIgnoreCase = true;
    public bool                                 LastFindWholeWord = false;
    public bool                                 LastFindRegexp = false;
    public bool                                 LastFindWrap = true;
    public int                                  LastFindTarget = 1;

    public BASICKeyMap                          BASICKeyMap = new BASICKeyMap();
    public bool                                 BASICStripSpaces = true;
    public bool                                 BASICShowControlCodesAsChars = true;
    public bool                                 BASICAutoToggleEntryMode = true;
    public bool                                 BASICStripREM = false;

    public MemoryDisplayType                    MemoryDisplay = MemoryDisplayType.ASCII;
    public MemorySourceType                     MemorySource = MemorySourceType.CPU;
    public Types.CharsetMode                    MemoryDisplayCharsetMode = Types.CharsetMode.HIRES;
    public int                                  MemoryDisplayCharsetBackgroundColor = 1;
    public int                                  MemoryDisplayCharsetCustomColor = 0;
    public int                                  MemoryDisplayCharsetMulticolor1 = 5;
    public int                                  MemoryDisplayCharsetMulticolor2 = 10;
    public int                                  MemoryDisplayCharsetMulticolor3 = 6;
    public int                                  MemoryDisplayCharsetMulticolor4 = 7;
    public int                                  MemoryDisplaySpriteBackgroundColor = 1;
    public bool                                 MemoryDisplaySpriteMulticolor = false;
    public int                                  MemoryDisplaySpriteCustomColor = 0;
    public int                                  MemoryDisplaySpriteMulticolor1 = 5;
    public int                                  MemoryDisplaySpriteMulticolor2 = 10;

    public bool                                 CheckForUpdates = true;
    public DateTime                             LastUpdateCheck = DateTime.MinValue;

    public AppMode                              StudioAppMode = AppMode.UNDECIDED;

    public GR.Collections.Map<string, LayoutInfo> ToolLayout = new GR.Collections.Map<string,LayoutInfo>();

    public GR.Collections.Set<Types.ErrorCode>  IgnoredWarnings = new GR.Collections.Set<C64Studio.Types.ErrorCode>();
    public GR.Collections.Set<Types.ErrorCode>  TreatWarningsAsErrors = new GR.Collections.Set<C64Studio.Types.ErrorCode>();

    public string                               DefaultProjectBasePath = "";
    public WeifenLuo.WinFormsUI.Docking.DockPanel   PanelMain = null;

    public GR.Collections.Map<Types.ColorableElement,Types.ColorSetting>  SyntaxColoring = new GR.Collections.Map<C64Studio.Types.ColorableElement, C64Studio.Types.ColorSetting>();

    public GR.Collections.Map<Types.Function, Types.FunctionInfo> Functions = new GR.Collections.Map<C64Studio.Types.Function, Types.FunctionInfo>();

    public GR.Collections.Map<ToolWindowType, ToolWindow> Tools = new GR.Collections.Map<ToolWindowType, ToolWindow>();

    public MainForm                             Main = null;

    private DeserializeDockContent m_deserializeDockContent;

    public StudioSettings()
    {
      m_deserializeDockContent = new DeserializeDockContent( GetContentFromPersistString );

      // known functions

      // functions from editing
      RegisterFunction( Function.BUILD, "Build", FunctionStudioState.NORMAL );
      RegisterFunction( Function.BUILD_AND_DEBUG, "Build and Debug", FunctionStudioState.NORMAL );
      RegisterFunction( Function.BUILD_AND_RUN, "Build and Run", FunctionStudioState.NORMAL );
      RegisterFunction( Function.COMPILE, "Compile", FunctionStudioState.NORMAL );
      RegisterFunction( Function.REBUILD, "Rebuild", FunctionStudioState.NORMAL );
      RegisterFunction( Function.BUILD_TO_PREPROCESSED_FILE, "Build Preprocessed File", FunctionStudioState.NORMAL );

      // functions for any state
      RegisterFunction( Function.CENTER_ON_CURSOR, "Center on Cursor", FunctionStudioState.ANY );
      RegisterFunction( Function.DELETE_LINE, "Delete Line", FunctionStudioState.ANY );
      RegisterFunction( Function.FIND, "Find", FunctionStudioState.ANY );
      RegisterFunction( Function.FIND_IN_PROJECT, "Find in Project", FunctionStudioState.ANY );
      RegisterFunction( Function.FIND_NEXT, "Find Next", FunctionStudioState.ANY );
      RegisterFunction( Function.FIND_REPLACE, "Replace", FunctionStudioState.ANY );
      RegisterFunction( Function.GO_TO_DECLARATION, "Go To Declaration", FunctionStudioState.ANY );
      RegisterFunction( Function.HELP, "Help", FunctionStudioState.ANY );
      RegisterFunction( Function.PRINT, "Print", FunctionStudioState.ANY );
      RegisterFunction( Function.REPLACE_IN_PROJECT, "Replace in Project", FunctionStudioState.ANY );
      RegisterFunction( Function.OPEN_FILES, "Open Files", FunctionStudioState.ANY );
      RegisterFunction( Function.SAVE_ALL, "Save All", FunctionStudioState.ANY );
      RegisterFunction( Function.SAVE_DOCUMENT, "Save Document", FunctionStudioState.ANY );
      RegisterFunction( Function.SAVE_DOCUMENT_AS, "Save Document As", FunctionStudioState.ANY );
      RegisterFunction( Function.MOVE_LINE_UP, "Move Line Up", FunctionStudioState.ANY );
      RegisterFunction( Function.MOVE_LINE_DOWN, "Move Line Down", FunctionStudioState.ANY );
      RegisterFunction( Function.COPY_LINE_UP, "Copy Line Up", FunctionStudioState.ANY );
      RegisterFunction( Function.COPY_LINE_DOWN, "Copy Line Down", FunctionStudioState.ANY );
      RegisterFunction( Function.COMMENT_SELECTION, "Comment Selection", FunctionStudioState.ANY );
      RegisterFunction( Function.UNCOMMENT_SELECTION, "Uncomment Selection", FunctionStudioState.ANY );
      RegisterFunction( Function.FIND_NEXT_MESSAGE, "Jump to next message", FunctionStudioState.ANY );
      RegisterFunction( Function.UNDO, "Undo", FunctionStudioState.ANY );
      RegisterFunction( Function.REDO, "Redo", FunctionStudioState.ANY );
      RegisterFunction( Function.COLLAPSE_ALL_FOLDING_BLOCKS, "Collapse all folding blocks", FunctionStudioState.ANY );
      RegisterFunction( Function.EXPAND_ALL_FOLDING_BLOCKS, "Expand all folding blocks", FunctionStudioState.ANY );
      RegisterFunction( Function.COPY, "Copy", Types.FunctionStudioState.ANY );
      RegisterFunction( Function.PASTE, "Paste", Types.FunctionStudioState.ANY );
      RegisterFunction( Function.CUT, "Cut", Types.FunctionStudioState.ANY );
      RegisterFunction( Function.JUMP_TO_LINE, "Jump to Line", FunctionStudioState.ANY );

      RegisterFunction( Function.GRAPHIC_ELEMENT_MIRROR_H, "Mirror Horizontal", FunctionStudioState.ANY );
      RegisterFunction( Function.GRAPHIC_ELEMENT_MIRROR_V, "Mirror Vertical", FunctionStudioState.ANY );
      RegisterFunction( Function.GRAPHIC_ELEMENT_SHIFT_L, "Shift Left", FunctionStudioState.ANY );
      RegisterFunction( Function.GRAPHIC_ELEMENT_SHIFT_R, "Shift Right", FunctionStudioState.ANY );
      RegisterFunction( Function.GRAPHIC_ELEMENT_SHIFT_U, "Shift Up", FunctionStudioState.ANY );
      RegisterFunction( Function.GRAPHIC_ELEMENT_SHIFT_D, "Shift Down", FunctionStudioState.ANY );
      RegisterFunction( Function.GRAPHIC_ELEMENT_ROTATE_L, "Rotate Left", FunctionStudioState.ANY );
      RegisterFunction( Function.GRAPHIC_ELEMENT_ROTATE_R, "Rotate Right", FunctionStudioState.ANY );
      RegisterFunction( Function.GRAPHIC_ELEMENT_INVERT, "Invert", FunctionStudioState.ANY );
      RegisterFunction( Function.GRAPHIC_ELEMENT_CUSTOM_COLOR, "Custom Color", FunctionStudioState.ANY );
      RegisterFunction( Function.GRAPHIC_ELEMENT_MULTI_COLOR_1, "Multi Color 1", FunctionStudioState.ANY );
      RegisterFunction( Function.GRAPHIC_ELEMENT_MULTI_COLOR_2, "Multi Color 2", FunctionStudioState.ANY );
      RegisterFunction( Function.GRAPHIC_ELEMENT_NEXT, "Next Element", FunctionStudioState.ANY );
      RegisterFunction( Function.GRAPHIC_ELEMENT_PREVIOUS, "Previous Element", FunctionStudioState.ANY );

      // functions for running debugger
      RegisterFunction( Function.DEBUG_BREAK, "Break into Debugger", FunctionStudioState.DEBUGGER_RUNNING );

      // functions for broken debugger
      RegisterFunction( Function.DEBUG_GO, "Debug Go", FunctionStudioState.DEBUGGER_BROKEN );
      RegisterFunction( Function.DEBUG_STEP, "Debug Step", FunctionStudioState.DEBUGGER_BROKEN );
      RegisterFunction( Function.DEBUG_STEP_OUT, "Debug Step Out", FunctionStudioState.DEBUGGER_BROKEN );
      RegisterFunction( Function.DEBUG_STEP_OVER, "Debug Step Over", FunctionStudioState.DEBUGGER_BROKEN );

      // functions for running/broken debugger
      RegisterFunction( Function.DEBUG_STOP, "Stop Debugging", FunctionStudioState.DEBUGGER_BROKEN | FunctionStudioState.DEBUGGER_RUNNING );

      // functions for broken debugger/editing
      RegisterFunction( Function.DEBUG_RUN_TO, "Run to Cursor", FunctionStudioState.DEBUGGER_BROKEN | FunctionStudioState.NORMAL );
      RegisterFunction( Function.TOGGLE_BREAKPOINT, "Toggle Breakpoint", FunctionStudioState.DEBUGGER_BROKEN | FunctionStudioState.NORMAL );
    }



    private void RegisterFunction( Function StudioFunction, string Description, FunctionStudioState StudioState )
    {
      Functions.Add( StudioFunction, new FunctionInfo( StudioFunction, Description, StudioState ) );
    }



    private IDockContent GetContentFromPersistString( string persistString )
    {
      //Debug.Log( "persist " + persistString );

      /*
      // ignore built in windows (map editor, etc.), for some reason restoring those breaks
      if ( ( persistString == "Map Editor" )
      ||   ( persistString == "Sprite Editor" )
      ||   ( persistString == "Charset Editor" )
      ||   ( persistString == "Help" )
      ||   ( persistString == "Graphic Screen Editor" )
      ||   ( persistString == "Text Screen Editor" ) )
      {
        // Ouch!
        return null;
      }*/
      foreach ( var toolEntry in Tools )
      {
        if ( persistString == toolEntry.Value.ToolDescription )
        {
          return toolEntry.Value.Document;
        }
      }
      //Debug.Log( "aua!" );
      //return new BaseDocument();
      return null;
    }



    public void UpdateInMRU( List<string> MRU, string Filename, MainForm MainForm )
    {
      for ( int i = 0; i < MRU.Count; ++i )
      {
        if ( MRU[i] == Filename )
        {
          if ( i > 0 )
          {
            MRU.RemoveAt( i );
            MRU.Insert( 0, Filename );
            MainForm.UpdateMenuMRU();
          }
          return;
        }
      }
      MRU.Insert( 0, Filename );
      if ( MRU.Count > MRUMaxCount )
      {
        MRU.RemoveAt( MRUMaxCount );
      }
      MainForm.UpdateMenuMRU();
    }



    public void RemoveFromMRU( List<string> MRU, string Filename, MainForm MainForm )
    {
      for ( int i = 0; i < MRU.Count; ++i )
      {
        if ( MRU[i] == Filename )
        {
          MRU.RemoveAt( i );
          MainForm.UpdateMenuMRU();
          return;
        }
      }
    }



    public void AddAccelerator( AcceleratorKey Accelerator )
    {
      Accelerators.Add( Accelerator.Key, Accelerator );
    }



    public Keys DetermineAcceleratorKeyForFunction( Types.Function Function, Types.StudioState State )
    {
      Types.FunctionStudioState functionMask = FunctionMaskFromAppState( State );

      foreach ( var key in Accelerators )
      {
        if ( ( key.Value.Function == Function )
        &&   ( ( Functions[Function].State & functionMask ) != 0 ) )
        {
          return key.Key;
        }
      }
      return Keys.None;
    }



    public AcceleratorKey DetermineAccelerator( Keys Key, Types.StudioState State )
    {
      List<AcceleratorKey> possibleKeys = null;

      if ( Accelerators.ContainsKey( Key ) )
      {
        possibleKeys = Accelerators.GetValues( Key, true );
      }
      else
      {
        // secondary key?
        possibleKeys = new List<AcceleratorKey>();
        foreach ( var acc in Accelerators )
        {
          if ( acc.Value.SecondaryKey == Key )
          {
            possibleKeys.Add( acc.Value );
          }
        }
      }

      Types.FunctionStudioState functionMask = FunctionMaskFromAppState( State );

      if ( possibleKeys == null )
      {
        return null;
      }
      foreach ( AcceleratorKey key in possibleKeys )
      {
        if ( ( functionMask & Functions[key.Function].State ) != 0 )
        {
          return key;
        }
      }
      return null;
    }



    internal void RefreshDisplayOnAllDocuments( StudioCore Core )
    {
      // also refresh main elements
      Core.MainForm.RefreshDisplayOnAllDocuments();

      Core.Theming.ApplyThemeToToolStripItems( Core.MainForm.MainMenuStrip.Items );

      //foreach ( WeifenLuo.WinFormsUI.Docking.IDockContent doc in Core.MainForm.panelMain.Documents )
      foreach ( WeifenLuo.WinFormsUI.Docking.IDockContent doc in Core.MainForm.panelMain.Contents )
      {
        //if ( doc is BaseDocument )
        {
          BaseDocument baseDoc = (BaseDocument)doc;
          baseDoc.RefreshDisplayOptions();
        }
      }
      Core.MainForm.Invalidate();
    }



    private Types.FunctionStudioState FunctionMaskFromAppState( Types.StudioState State )
    {
      Types.FunctionStudioState   functionMask = 0;
      switch ( State )
      {
        case C64Studio.Types.StudioState.NORMAL:
          functionMask |= FunctionStudioState.NORMAL;
          break;
        case C64Studio.Types.StudioState.COMPILE:
        case C64Studio.Types.StudioState.BUILD:
        case C64Studio.Types.StudioState.BUILD_AND_DEBUG:
        case C64Studio.Types.StudioState.BUILD_AND_RUN:
          functionMask |= FunctionStudioState.BUILDING;
          break;
        case C64Studio.Types.StudioState.DEBUGGING_BROKEN:
          functionMask |= FunctionStudioState.DEBUGGER_BROKEN;
          break;
        case C64Studio.Types.StudioState.DEBUGGING_RUN:
          functionMask |= FunctionStudioState.DEBUGGER_RUNNING;
          break;
      }
      return functionMask;
    }



    public AcceleratorKey DetermineAccelerator( Types.Function Function )
    {
      foreach ( var accPair in Accelerators )
      {
        if ( accPair.Value.Function == Function )
        {
          return accPair.Value;
        }
      }
      return null;
    }



    private void EnumElements( GR.Strings.XMLElement Element )
    {
      List<GR.Strings.XMLElement>   elementsToRemove = new List<GR.Strings.XMLElement>();
      foreach ( var element in Element )
      {
        if ( element.Type == "Content" )
        {
          if ( ( element.Attribute( "PersistString" ) == "WeifenLuo.WinFormsUI.Docking.DockPanel+Persistor+DummyContent" )
          ||   ( element.Attribute( "PersistString" ) == "" ) )
          {
            elementsToRemove.Add( element );
          }
        }
        //Debug.Log( element.Type );
        EnumElements( element );
      }

      foreach ( var toRemove in elementsToRemove )
      {
        var parentElement = toRemove.Parent;

        parentElement.RemoveChild( toRemove );

        // decrease count
        int count = GR.Convert.ToI32( parentElement.Attribute( "Count" ) );
        --count;
        parentElement.SetAttribute( "Count", count.ToString() );
      }
    }



    public GR.Memory.ByteBuffer ToBuffer( StudioCore Core )
    {
      GR.Memory.ByteBuffer SettingsData = new GR.Memory.ByteBuffer();

      foreach ( ToolInfo tool in ToolInfos )
      {
        GR.IO.FileChunk chunkTool = tool.ToChunk();

        SettingsData.Append( chunkTool.ToBuffer() );
      }
      /*
      foreach ( C64Studio.LayoutInfo layout in ToolLayout.Values )
      {
        layout.StoreLayout();
        SettingsData.Append( layout.ToBuffer() );
      }*/

      foreach ( AcceleratorKey key in Accelerators.Values )
      {
        GR.IO.FileChunk   chunkKey = key.ToChunk();
        SettingsData.Append( chunkKey.ToBuffer() );
      }

      GR.IO.FileChunk   chunkSoundSettings = new GR.IO.FileChunk( Types.FileChunk.SETTINGS_SOUND );
      chunkSoundSettings.AppendU8( (byte)( PlaySoundOnSuccessfulBuild ? 1 : 0 ) );
      
      chunkSoundSettings.AppendU8( (byte)( PlaySoundOnBuildFailure ? 1 : 0 ) );
      chunkSoundSettings.AppendU8( (byte)( PlaySoundOnSearchFoundNoItem ? 1 : 0 ) );
      SettingsData.Append( chunkSoundSettings.ToBuffer() );

      GR.IO.FileChunk   chunkMainWindowPlacement = new GR.IO.FileChunk( Types.FileChunk.SETTINGS_WINDOW );
      chunkMainWindowPlacement.AppendString( MainWindowPlacement );
      SettingsData.Append( chunkMainWindowPlacement.ToBuffer() );

      GR.IO.FileChunk chunkTabs = new GR.IO.FileChunk( Types.FileChunk.SETTINGS_TEXT_EDITOR );
      chunkTabs.AppendI32( TabSize );
      chunkTabs.AppendU8( (byte)( AllowTabs ? 1 : 0 ) );
      chunkTabs.AppendU8( (byte)( TabConvertToSpaces ? 1 : 0 ) );
      chunkTabs.AppendU8( (byte)( StripTrailingSpaces ? 1 : 0 ) );
      SettingsData.Append( chunkTabs.ToBuffer() );

      GR.IO.FileChunk chunkFont = new GR.IO.FileChunk( Types.FileChunk.SETTINGS_FONT );
      chunkFont.AppendString( SourceFontFamily );
      chunkFont.AppendI32( (int)SourceFontSize );
      chunkFont.AppendU8( (byte)( BASICUseNonC64Font ? 1 : 0 ) );
      chunkFont.AppendString( BASICSourceFontFamily );
      chunkFont.AppendI32( (int)BASICSourceFontSize );
      chunkFont.AppendI32( (int)SourceFontStyle );
      chunkFont.AppendI32( (int)BASICSourceFontStyle );

      SettingsData.Append( chunkFont.ToBuffer() );

      foreach ( Types.ColorableElement element in SyntaxColoring.Keys )
      {
        GR.IO.FileChunk chunkSyntaxColor = new GR.IO.FileChunk( Types.FileChunk.SETTINGS_SYNTAX_COLORING );

        chunkSyntaxColor.AppendU32( (uint)element );
        chunkSyntaxColor.AppendU32( SyntaxColoring[element].FGColor );
        chunkSyntaxColor.AppendU32( SyntaxColoring[element].BGColor );
        chunkSyntaxColor.AppendI32( SyntaxColoring[element].BGColorAuto ? 1 : 0 );

        SettingsData.Append( chunkSyntaxColor.ToBuffer() );
      }

      GR.IO.FileChunk chunkUI = new GR.IO.FileChunk( Types.FileChunk.SETTINGS_UI );
      chunkUI.AppendU8( (byte)( ToolbarActiveMain ? 1 : 0 ) );
      chunkUI.AppendU8( (byte)( ToolbarActiveDebugger ? 1 : 0 ) );
      chunkUI.AppendI32( MRUMaxCount );
      SettingsData.Append( chunkUI.ToBuffer() );

      GR.IO.FileChunk chunkRunEmu = new GR.IO.FileChunk( Types.FileChunk.SETTINGS_RUN_EMULATOR );
      chunkRunEmu.AppendU8( (byte)( TrueDriveEnabled ? 1 : 0 ) );
      chunkRunEmu.AppendString( EmulatorToRun );
      SettingsData.Append( chunkRunEmu.ToBuffer() );

      GR.IO.FileChunk chunkDefaults = new GR.IO.FileChunk( Types.FileChunk.SETTINGS_DEFAULTS );
      chunkDefaults.AppendString( DefaultProjectBasePath );
      SettingsData.Append( chunkDefaults.ToBuffer() );

      // dockpanel layout
      GR.IO.FileChunk chunkLayout = new GR.IO.FileChunk( Types.FileChunk.SETTINGS_DPS_LAYOUT );
      System.IO.MemoryStream    memOut = new System.IO.MemoryStream();
      //Debug.Log( "Save with state " + Main.m_DebugRegisters.DockState );

      PanelMain.SaveAsXml( memOut, Encoding.UTF8 );
      byte[] layoutData = memOut.ToArray();
      string xmlOutText = System.Text.Encoding.UTF8.GetString( layoutData );

      //Debug.Log( xmlOutText );
      // remove dummy elements (layout of non-tool windows)
      GR.Strings.XMLParser    parser = new GR.Strings.XMLParser();
      if ( !parser.Parse( xmlOutText ) )
      {
        Debug.Log( "Could not parse XML" );
      }
      else
      {
        EnumElements( parser );
        xmlOutText = parser.ToText();
      }

      //Debug.Log( xmlOutText );
      chunkLayout.AppendU32( (uint)layoutData.Length );
      chunkLayout.Append( layoutData );
      SettingsData.Append( chunkLayout.ToBuffer() );
      memOut.Close();
      memOut.Dispose();

      GR.IO.FileChunk chunkFindReplace = new GR.IO.FileChunk( Types.FileChunk.SETTINGS_FIND_REPLACE );
      chunkFindReplace.AppendU8( (byte)( LastFindIgnoreCase ? 1 : 0 ) );
      chunkFindReplace.AppendU8( (byte)( LastFindWholeWord ? 1 : 0 ) );
      chunkFindReplace.AppendU8( (byte)( LastFindRegexp ? 1 : 0 ) );
      chunkFindReplace.AppendU8( (byte)( LastFindWrap ? 1 : 0 ) );
      chunkFindReplace.AppendU8( (byte)LastFindTarget );

      chunkFindReplace.AppendI32( FindArguments.Count );
      foreach ( var findArg in FindArguments )
      {
        chunkFindReplace.AppendString( findArg );
      }
      chunkFindReplace.AppendI32( ReplaceArguments.Count );
      foreach ( var replaceArg in ReplaceArguments )
      {
        chunkFindReplace.AppendString( replaceArg );
      }
      SettingsData.Append( chunkFindReplace.ToBuffer() );

      GR.IO.FileChunk chunkIgnoredWarnings = new GR.IO.FileChunk( Types.FileChunk.SETTINGS_IGNORED_WARNINGS );
      chunkIgnoredWarnings.AppendI32( IgnoredWarnings.Count );
      foreach ( Types.ErrorCode ignoredWarning in IgnoredWarnings )
      {
        chunkIgnoredWarnings.AppendI32( (int)ignoredWarning );
      }
      SettingsData.Append( chunkIgnoredWarnings.ToBuffer() );

      GR.IO.FileChunk chunkWarningsAsErrors = new GR.IO.FileChunk( Types.FileChunk.SETTINGS_WARNINGS_AS_ERRORS );
      chunkWarningsAsErrors.AppendI32( TreatWarningsAsErrors.Count );
      foreach ( Types.ErrorCode warningAsError in TreatWarningsAsErrors )
      {
        chunkWarningsAsErrors.AppendI32( (int)warningAsError );
      }
      SettingsData.Append( chunkWarningsAsErrors.ToBuffer() );

      foreach ( var pair in GenericTools )
      {
        GR.Memory.ByteBuffer    displayDetailData = pair.Value.DisplayDetails();
        if ( displayDetailData != null )
        {
          GR.IO.FileChunk   chunkPanelDisplayDetails = new GR.IO.FileChunk( Types.FileChunk.SETTINGS_PANEL_DISPLAY_DETAILS );

          chunkPanelDisplayDetails.AppendString( pair.Key );

          chunkPanelDisplayDetails.AppendU32( displayDetailData.Length );
          chunkPanelDisplayDetails.Append( displayDetailData );

          SettingsData.Append( chunkPanelDisplayDetails.ToBuffer() );
        }
      }

      // ASM editor settings
      GR.IO.FileChunk chunkASMEditor = new GR.IO.FileChunk( Types.FileChunk.SETTINGS_ASSEMBLER_EDITOR );
      chunkASMEditor.AppendU8( (byte)( ASMHideLineNumbers ? 1 : 0 ) );
      chunkASMEditor.AppendU8( (byte)( !ASMShowCycles ? 1 : 0 ) );
      chunkASMEditor.AppendU8( (byte)( !ASMShowBytes ? 1 : 0 ) );
      chunkASMEditor.AppendU8( (byte)( !ASMShowMiniView ? 1 : 0 ) );
      chunkASMEditor.AppendU8( (byte)( ASMAutoTruncateLiteralValues ? 1 : 0 ) );
      chunkASMEditor.AppendU8( (byte)( !ASMShowAutoComplete ? 1 : 0 ) );
      chunkASMEditor.AppendI32( ASMLibraryPaths.Count );
      foreach ( var libPath in ASMLibraryPaths )
      {
        chunkASMEditor.AppendString( libPath );
      }
      chunkASMEditor.AppendU8( (byte)( !ASMShowAddress ? 1 : 0 ) );
      SettingsData.Append( chunkASMEditor.ToBuffer() );

      // Outline settings
      GR.IO.FileChunk chunkOutline = new GR.IO.FileChunk( Types.FileChunk.SETTINGS_OUTLINE );
      chunkOutline.AppendU8( (byte)( !OutlineShowLocalLabels ? 1 : 0 ) );
      chunkOutline.AppendU8( (byte)( !OutlineShowShortCutLabels ? 1 : 0 ) );
      chunkOutline.AppendU8( (byte)( !OutlineSortByIndex ? 1 : 0 ) );
      SettingsData.Append( chunkOutline.ToBuffer() );

      // BASIC settings
      GR.IO.FileChunk chunkBASICParser = new GR.IO.FileChunk( Types.FileChunk.SETTINGS_BASIC_PARSER );
      chunkBASICParser.AppendU8( (byte)( BASICStripSpaces ? 1 : 0 ) );
      chunkBASICParser.AppendU8( (byte)( BASICShowControlCodesAsChars ? 1 : 0 ) );
      chunkBASICParser.AppendU8( (byte)( !BASICAutoToggleEntryMode ? 1 : 0 ) );
      chunkBASICParser.AppendU8( (byte)( BASICStripREM ? 1 : 0 ) );
      SettingsData.Append( chunkBASICParser.ToBuffer() );

      // BASIC key map
      GR.IO.FileChunk chunkBASICKeyMap = new GR.IO.FileChunk( Types.FileChunk.SETTINGS_BASIC_KEYMAP );

      chunkBASICKeyMap.AppendI32( BASICKeyMap.Keymap.Count );
      foreach ( var entry in BASICKeyMap.Keymap )
      {
        chunkBASICKeyMap.AppendU32( (uint)entry.Key );
        chunkBASICKeyMap.AppendI32( (int)entry.Value.KeyboardKey );
      }
      SettingsData.Append( chunkBASICKeyMap.ToBuffer() );

      // environment behaviour settings
      GR.IO.FileChunk chunkEnvironment = new GR.IO.FileChunk( Types.FileChunk.SETTINGS_ENVIRONMENT );
      chunkEnvironment.AppendU8( (byte)( AutoOpenLastSolution ? 1 : 0 ) );
      chunkEnvironment.AppendU8( (byte)( LastSolutionWasEmpty ? 1 : 0 ) );
      chunkEnvironment.AppendU8( (byte)( !CheckForUpdates ? 1 : 0 ) );
      chunkEnvironment.AppendU8( (byte)LastUpdateCheck.Day );
      chunkEnvironment.AppendU8( (byte)LastUpdateCheck.Month );
      chunkEnvironment.AppendI32( (byte)LastUpdateCheck.Year );

      SettingsData.Append( chunkEnvironment.ToBuffer() );

      // tool window settings
      GR.IO.FileChunk chunkPerspectives = new GR.IO.FileChunk( Types.FileChunk.SETTINGS_PERSPECTIVES );
      foreach ( Perspective perspective in System.Enum.GetValues( typeof( Perspective ) ) )
      {
        GR.IO.FileChunk chunkPerspective = new GR.IO.FileChunk( Types.FileChunk.SETTINGS_PERSPECTIVE );

        chunkPerspective.AppendI32( (int)perspective );
        chunkPerspective.AppendI32( Tools.Count );
        foreach ( var tool in Tools )
        {
          chunkPerspective.AppendU32( (uint)tool.Key );
          chunkPerspective.AppendU8( (byte)( tool.Value.Visible[perspective] ? 1 : 0 ) );
        }
        chunkPerspectives.Append( chunkPerspective.ToBuffer() );
      }
      SettingsData.Append( chunkPerspectives.ToBuffer() );

      // hex view 
      GR.IO.FileChunk chunkHexView = new GR.IO.FileChunk( Types.FileChunk.SETTINGS_HEX_VIEW );

      chunkHexView.AppendI32( (int)MemoryDisplay );
      chunkHexView.AppendI32( (int)MemorySource );

      chunkHexView.AppendU8( (byte)MemoryDisplayCharsetMode );
      chunkHexView.AppendU8( (byte)MemoryDisplayCharsetBackgroundColor );
      chunkHexView.AppendU8( (byte)MemoryDisplayCharsetCustomColor );
      chunkHexView.AppendU8( (byte)MemoryDisplayCharsetMulticolor1 );
      chunkHexView.AppendU8( (byte)MemoryDisplayCharsetMulticolor2 );
      chunkHexView.AppendU8( (byte)MemoryDisplayCharsetMulticolor3 );
      chunkHexView.AppendU8( (byte)MemoryDisplayCharsetMulticolor4 );

      chunkHexView.AppendU8( (byte)( MemoryDisplaySpriteMulticolor ? 1 : 0 ) );
      chunkHexView.AppendU8( (byte)MemoryDisplaySpriteBackgroundColor );
      chunkHexView.AppendU8( (byte)MemoryDisplaySpriteCustomColor );
      chunkHexView.AppendU8( (byte)MemoryDisplaySpriteMulticolor1 );
      chunkHexView.AppendU8( (byte)MemoryDisplaySpriteMulticolor2 );
      SettingsData.Append( chunkHexView.ToBuffer() );


      // MRU projects
      GR.IO.FileChunk chunkMRUProjects = new GR.IO.FileChunk( Types.FileChunk.SETTINGS_MRU_PROJECTS );

      chunkMRUProjects.AppendI32( MRUProjects.Count );
      for ( int i = 0; i < MRUProjects.Count; ++i )
      {
        chunkMRUProjects.AppendString( MRUProjects[i] );
      }
      SettingsData.Append( chunkMRUProjects.ToBuffer() );

      // MRU files
      GR.IO.FileChunk chunkMRUFiles = new GR.IO.FileChunk( Types.FileChunk.SETTINGS_MRU_FILES );

      chunkMRUFiles.AppendI32( MRUFiles.Count );
      for ( int i = 0; i < MRUFiles.Count; ++i )
      {
        chunkMRUFiles.AppendString( MRUFiles[i] );
      }
      SettingsData.Append( chunkMRUFiles.ToBuffer() );

      return SettingsData;
    }



    private void SetLayoutFromData( GR.Memory.ByteBuffer Data )
    {
      PanelMain.SuspendLayout( true );

      // need to clear all
      foreach ( var toolEntry in Tools )
      {
        toolEntry.Value.Document.DockPanel = null;
      }
      Main.CloseAllDocuments();

      //Debug.Log( Data.ToAsciiString() );

      System.IO.MemoryStream    memIn = new System.IO.MemoryStream( Data.Data(), false );

      try
      {
        PanelMain.LoadFromXml( memIn, m_deserializeDockContent );
      }
      catch ( Exception ex )
      {
        Debug.Log( "SetLayoutFromData: " + ex.Message );
      }

      memIn.Close();
      memIn.Dispose();
      PanelMain.ResumeLayout( false, true );
    }



    public bool ReadFromBuffer( GR.Memory.ByteBuffer SettingsData )
    {
      IgnoredWarnings.Clear();

      GR.IO.BinaryReader binReader = new GR.IO.BinaryReader( SettingsData.MemoryStream() );

      GR.IO.FileChunk chunkData = new GR.IO.FileChunk();

      while ( chunkData.ReadFromStream( binReader ) )
      {
        switch ( chunkData.Type )
        {
          case Types.FileChunk.SETTINGS_TOOL:
            {
              ToolInfo tool = new ToolInfo();

              tool.FromChunk( chunkData );

              // sanitize args
              tool.CartArguments = tool.CartArguments.Replace( "$(BuildTargetFilename)", "$(RunFilename)" );
              tool.PRGArguments = tool.PRGArguments.Replace( "$(BuildTargetFilename)", "$(RunFilename)" );
              tool.WorkPath = tool.WorkPath.Replace( "$(FilePath)", "$(RunPath)" );

              if ( string.IsNullOrEmpty( tool.TrueDriveOnArguments ) )
              {
                tool.TrueDriveOnArguments = "-truedrive +virtualdev";
                tool.TrueDriveOffArguments = "+truedrive -virtualdev";
              }
              if ( tool.PRGArguments.Contains( "-truedrive " ) )
              {
                tool.PRGArguments = tool.PRGArguments.Replace( "-truedrive ", "" );
              }
              if ( tool.PRGArguments.Contains( " -truedrive" ) )
              {
                tool.PRGArguments = tool.PRGArguments.Replace( " -truedrive", "" );
              }
              ToolInfos.Add( tool );
            }
            break;
          case Types.FileChunk.SETTINGS_ACCELERATOR:
            {
              AcceleratorKey key = new AcceleratorKey();

              key.FromChunk( chunkData );

              Accelerators.Add( key.Key, key );
            }
            break;
          case Types.FileChunk.SETTINGS_DPS_LAYOUT:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              uint                    size = binIn.ReadUInt32();
              GR.Memory.ByteBuffer    tempData = new GR.Memory.ByteBuffer();
              binIn.ReadBlock( tempData, size );
              SetLayoutFromData( tempData );
            }
            break;
          case Types.FileChunk.SETTINGS_SOUND:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              PlaySoundOnSuccessfulBuild    = ( binIn.ReadUInt8() != 0 );
              PlaySoundOnBuildFailure       = ( binIn.ReadUInt8() != 0 );
              PlaySoundOnSearchFoundNoItem  = ( binIn.ReadUInt8() != 0 );
            }
            break;
          case Types.FileChunk.SETTINGS_WINDOW:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              MainWindowPlacement = binIn.ReadString();
            }
            break;
          case Types.FileChunk.SETTINGS_TEXT_EDITOR:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              TabSize = binIn.ReadInt32();
              if ( ( TabSize <= 0 )
              ||   ( TabSize >= 100 ) )
              {
                TabSize = 2;
              }
              AllowTabs           = ( binIn.ReadUInt8() != 0 );
              TabConvertToSpaces  = ( binIn.ReadUInt8() != 0 );
              StripTrailingSpaces = ( binIn.ReadUInt8() != 0 );
            }
            break;
          case Types.FileChunk.SETTINGS_FONT:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              SourceFontFamily        = binIn.ReadString();
              SourceFontSize          = (float)binIn.ReadInt32();

              BASICUseNonC64Font      = ( binIn.ReadUInt8() != 0 );
              BASICSourceFontFamily   = binIn.ReadString();
              BASICSourceFontSize     = (float)binIn.ReadInt32();
              if ( BASICSourceFontSize <= 0 )
              {
                BASICSourceFontSize = 9.0f;
              }

              SourceFontStyle         = (FontStyle)binIn.ReadInt32();
              BASICSourceFontStyle    = (FontStyle)binIn.ReadInt32();
            }
            break;
          case Types.FileChunk.SETTINGS_SYNTAX_COLORING:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              Types.ColorableElement   element = (C64Studio.Types.ColorableElement)binIn.ReadUInt32();

              Types.ColorSetting color = new C64Studio.Types.ColorSetting( GR.EnumHelper.GetDescription( element ) );
              color.FGColor = binIn.ReadUInt32();
              color.BGColor = binIn.ReadUInt32();
              color.BGColorAuto = ( binIn.ReadUInt32() != 0 );

              color.FGColor |= 0xff000000;
              color.BGColor |= 0xff000000;

              SyntaxColoring.Add( element, color );
            }
            break;
          case Types.FileChunk.SETTINGS_UI:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              ToolbarActiveMain     = ( binIn.ReadUInt8() == 1 );
              ToolbarActiveDebugger = ( binIn.ReadUInt8() == 1 );
              MRUMaxCount           = binIn.ReadInt32();

              if ( MRUMaxCount == 0 )
              {
                MRUMaxCount = 4;
              }
            }
            break;
          case Types.FileChunk.SETTINGS_RUN_EMULATOR:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              TrueDriveEnabled = ( binIn.ReadUInt8() != 0 );
              EmulatorToRun = binIn.ReadString();
            }
            break;
          case Types.FileChunk.SETTINGS_DEFAULTS:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              DefaultProjectBasePath = binIn.ReadString();
            }
            break;
          case Types.FileChunk.SETTINGS_OUTLINE:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              OutlineShowLocalLabels = ( binIn.ReadUInt8() == 0 );
              OutlineShowShortCutLabels = ( binIn.ReadUInt8() == 0 );
              OutlineSortByIndex = ( binIn.ReadUInt8() == 0 );
              OutlineSortByAlphabet = !OutlineSortByIndex;
            }
            break;
          case Types.FileChunk.SETTINGS_FIND_REPLACE:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              LastFindIgnoreCase = ( binIn.ReadUInt8() == 1 );
              LastFindWholeWord = ( binIn.ReadUInt8() == 1 );
              LastFindRegexp = ( binIn.ReadUInt8() == 1 );
              LastFindWrap = ( binIn.ReadUInt8() == 1 );
              LastFindTarget = binIn.ReadUInt8();

              int numFindArguments = binIn.ReadInt32();
              for ( int i = 0; i < numFindArguments; ++i )
              {
                FindArguments.Add( binIn.ReadString() );
              }
              int numReplaceArguments = binIn.ReadInt32();
              for ( int i = 0; i < numReplaceArguments; ++i )
              {
                ReplaceArguments.Add( binIn.ReadString() );
              }
            }
            break;
          case Types.FileChunk.SETTINGS_IGNORED_WARNINGS:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              int numIgnoredWarnings = binIn.ReadInt32();

              for ( int i = 0; i < numIgnoredWarnings; ++i )
              {
                IgnoredWarnings.Add( (C64Studio.Types.ErrorCode)binIn.ReadInt32() );
              }
            }
            break;
          case Types.FileChunk.SETTINGS_WARNINGS_AS_ERRORS:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              int numWarnignsAsErrors = binIn.ReadInt32();

              for ( int i = 0; i < numWarnignsAsErrors; ++i )
              {
                TreatWarningsAsErrors.Add( (C64Studio.Types.ErrorCode)binIn.ReadInt32() );
              }
            }
            break;
          case Types.FileChunk.SETTINGS_PANEL_DISPLAY_DETAILS:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              string    toolName = binIn.ReadString();

              if ( GenericTools.ContainsKey( toolName ) )
              {
                uint    length = binIn.ReadUInt32();
                GR.Memory.ByteBuffer    data = new GR.Memory.ByteBuffer();

                binIn.ReadBlock( data, length );
                GenericTools[toolName].ApplyDisplayDetails( data );
              }
            }
            break;
          case Types.FileChunk.SETTINGS_BASIC_KEYMAP:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              int     numEntries = binIn.ReadInt32();

              uint     neutralLang = (uint)( System.Globalization.CultureInfo.CurrentCulture.KeyboardLayoutId & 0xff );

              if ( ( neutralLang != 7 )
              &&   ( neutralLang != 9 ) )
              {
                neutralLang = 9;
              }

              for ( int i = 0; i < numEntries; ++i )
              {
                Keys  key                 = (Keys)binIn.ReadUInt32();
                Types.KeyboardKey  cmdKey = (C64Studio.Types.KeyboardKey)binIn.ReadInt32();

                //Debug.Log( "Key " + key.ToString() + " maps to " + cmdKey );

                var keyMapEntry = new KeymapEntry();

                foreach ( var entry in BASICKeyMap.DefaultKeymaps[neutralLang] )
                {
                  if ( entry.Value.KeyboardKey == cmdKey )
                  {
                    BASICKeyMap.Keymap[key] = entry.Value;
                    //Debug.Log( "-inserted" );
                    break;
                  }
                }
              }
            }
            break;
          case Types.FileChunk.SETTINGS_ASSEMBLER_EDITOR:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              ASMHideLineNumbers  = ( binIn.ReadUInt8() != 0 );
              ASMShowCycles       = ( binIn.ReadUInt8() == 0 );
              ASMShowBytes        = ( binIn.ReadUInt8() == 0 );
              ASMShowMiniView     = ( binIn.ReadUInt8() == 0 );
              ASMAutoTruncateLiteralValues = ( binIn.ReadUInt8() != 0 );
              ASMShowAutoComplete = ( binIn.ReadUInt8() == 0 );

              int numLibPaths = binIn.ReadInt32();
              for ( int i = 0; i < numLibPaths; ++i )
              {
                ASMLibraryPaths.Add( binIn.ReadString() );
              }

              ASMShowAddress = ( binIn.ReadUInt8() == 0 );
            }
            break;
          case Types.FileChunk.SETTINGS_BASIC_PARSER:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              BASICStripSpaces              = ( binIn.ReadUInt8() != 0 );
              BASICShowControlCodesAsChars  = ( binIn.ReadUInt8() != 0 );
              BASICAutoToggleEntryMode      = ( binIn.ReadUInt8() == 0 );
              BASICStripREM                 = ( binIn.ReadUInt8() != 0 );
            }
            break;
          case Types.FileChunk.SETTINGS_ENVIRONMENT:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              AutoOpenLastSolution  = ( binIn.ReadUInt8() != 0 );
              LastSolutionWasEmpty  = ( binIn.ReadUInt8() != 0 );
              CheckForUpdates       = ( binIn.ReadUInt8() == 0 );

              int     day   = binIn.ReadUInt8();
              int     month = binIn.ReadUInt8();
              int     year  = binIn.ReadInt32();

              try
              {
                if ( ( day == 0 )
                &&   ( month == 0 )
                &&   ( year == 0 ) )
                {
                    LastUpdateCheck = DateTime.Now;
                }
                else
                {
                    LastUpdateCheck = new DateTime( year, month, day );
                }
              }
              catch ( ArgumentOutOfRangeException )
              {
                LastUpdateCheck = DateTime.Now;
              }
            }
            break;
          case Types.FileChunk.SETTINGS_PERSPECTIVES:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              GR.IO.FileChunk subChunkData = new GR.IO.FileChunk();

              while ( subChunkData.ReadFromStream( binIn ) )
              {
                GR.IO.IReader subBinIn = subChunkData.MemoryReader();

                switch ( subChunkData.Type )
                {
                  case Types.FileChunk.SETTINGS_PERSPECTIVE:
                    {
                      Perspective   perspective = (Perspective)subBinIn.ReadInt32();
                      int           numEntries  = subBinIn.ReadInt32();

                      for ( int j = 0; j < numEntries; ++j )
                      {
                        ToolWindowType    toolWindow = (ToolWindowType)subBinIn.ReadInt32();
                        bool              visible = ( subBinIn.ReadUInt8() == 1 );


                        Tools[toolWindow].Visible[perspective] = visible;
                      }
                    }
                    break;
                }
              }
            }
            break;
          case Types.FileChunk.SETTINGS_HEX_VIEW:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              MemoryDisplay = (MemoryDisplayType)binIn.ReadInt32();
              MemorySource = (MemorySourceType)binIn.ReadInt32();
              MemoryDisplayCharsetMode = (Types.CharsetMode)binIn.ReadUInt8();
              MemoryDisplayCharsetBackgroundColor = binIn.ReadUInt8();
              MemoryDisplayCharsetCustomColor = binIn.ReadUInt8();
              MemoryDisplayCharsetMulticolor1 = binIn.ReadUInt8();
              MemoryDisplayCharsetMulticolor2 = binIn.ReadUInt8();
              MemoryDisplayCharsetMulticolor3 = binIn.ReadUInt8();
              MemoryDisplayCharsetMulticolor4 = binIn.ReadUInt8();
              MemoryDisplaySpriteMulticolor = ( binIn.ReadUInt8() != 0 );
              MemoryDisplaySpriteBackgroundColor = binIn.ReadUInt8();
              MemoryDisplaySpriteCustomColor = binIn.ReadUInt8();
              MemoryDisplaySpriteMulticolor1 = binIn.ReadUInt8();
              MemoryDisplaySpriteMulticolor2 = binIn.ReadUInt8();
            }
            break;
          case Types.FileChunk.SETTINGS_MRU_PROJECTS:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              int   numEntries = binIn.ReadInt32();
              for ( int i = 0; i < numEntries; ++i )
              {
                string    file = binIn.ReadString();
                MRUProjects.Add( file );
              }
            }
            break;
          case Types.FileChunk.SETTINGS_MRU_FILES:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              int   numEntries = binIn.ReadInt32();
              for ( int i = 0; i < numEntries; ++i )
              {
                string    file = binIn.ReadString();
                MRUFiles.Add( file );
              }
            }
            break;
        }
      }
      return true;
    }



    private void SetKeyBindingKey( C64Studio.Types.Function Function, Keys Key )
    {
      foreach ( var accPair in Accelerators )
      {
        if ( accPair.Value.Function == Function )
        {
          Accelerators.Remove( accPair.Key, accPair.Value );
          break;
        }
      }

      if ( Key != Keys.None )
      {
        AcceleratorKey key = new AcceleratorKey( Key, Function );
        key.Key = Key;
        Accelerators.Add( key.Key, key );
      }
    }



    public void SetDefaultKeyBinding()
    {
      SetKeyBindingKey( C64Studio.Types.Function.SAVE_DOCUMENT, Keys.Control | Keys.S );
      SetKeyBindingKey( C64Studio.Types.Function.BUILD_AND_RUN, Keys.Control | Keys.F5 );
      SetKeyBindingKey( C64Studio.Types.Function.BUILD, Keys.F7 );
      SetKeyBindingKey( C64Studio.Types.Function.BUILD_AND_DEBUG, Keys.F5 );
      SetKeyBindingKey( C64Studio.Types.Function.DEBUG_GO, Keys.F5 );
      SetKeyBindingKey( C64Studio.Types.Function.DEBUG_STEP_OUT, Keys.F9 );
      SetKeyBindingKey( C64Studio.Types.Function.CENTER_ON_CURSOR, Keys.Clear );
      SetKeyBindingKey( C64Studio.Types.Function.DELETE_LINE, Keys.Control | Keys.Y );
      SetKeyBindingKey( C64Studio.Types.Function.DEBUG_STEP, Keys.F11 );
      SetKeyBindingKey( C64Studio.Types.Function.DEBUG_STEP_OVER, Keys.F10 );
      SetKeyBindingKey( C64Studio.Types.Function.DEBUG_STOP, Keys.Shift | Keys.F5 );
      SetKeyBindingKey( C64Studio.Types.Function.DEBUG_RUN_TO, Keys.Control | Keys.F10 );
      SetKeyBindingKey( C64Studio.Types.Function.GO_TO_DECLARATION, Keys.Control | Keys.G );
      SetKeyBindingKey( C64Studio.Types.Function.COMPILE, Keys.Control | Keys.B );
      SetKeyBindingKey( C64Studio.Types.Function.FIND, Keys.Control | Keys.F );
      SetKeyBindingKey( C64Studio.Types.Function.FIND_NEXT, Keys.Control | Keys.L );
      SetKeyBindingKey( C64Studio.Types.Function.FIND_REPLACE, Keys.Control | Keys.H );
      SetKeyBindingKey( C64Studio.Types.Function.FIND_IN_PROJECT, Keys.Control | Keys.Shift | Keys.F );
      SetKeyBindingKey( C64Studio.Types.Function.REPLACE_IN_PROJECT, Keys.Control | Keys.Shift | Keys.H );
      SetKeyBindingKey( C64Studio.Types.Function.SAVE_ALL, Keys.Control | Keys.Shift | Keys.S );
      SetKeyBindingKey( C64Studio.Types.Function.PRINT, Keys.Control | Keys.P );
      SetKeyBindingKey( C64Studio.Types.Function.HELP, Keys.F1 );
      SetKeyBindingKey( C64Studio.Types.Function.SAVE_DOCUMENT_AS, Keys.F12 );
      SetKeyBindingKey( C64Studio.Types.Function.TOGGLE_BREAKPOINT, Keys.Shift | Keys.F9 );
      SetKeyBindingKey( C64Studio.Types.Function.UNDO, Keys.Alt | Keys.Back );
      SetKeyBindingKey( C64Studio.Types.Function.REDO, Keys.Shift | Keys.Alt | Keys.Back );
      SetKeyBindingKey( C64Studio.Types.Function.COPY, Keys.Control | Keys.C );
      SetKeyBindingKey( C64Studio.Types.Function.PASTE, Keys.Control | Keys.V );
      SetKeyBindingKey( C64Studio.Types.Function.CUT, Keys.Control | Keys.X );
    }



    public void SetSyntaxColor( C64Studio.Types.ColorableElement Element, uint FGColor, uint BGColor, bool AutoBGColor )
    {
      if ( SyntaxColoring[Element] == null )
      {
        SyntaxColoring[Element] = new C64Studio.Types.ColorSetting( GR.EnumHelper.GetDescription( Element ), FGColor, BGColor );
      }
      else
      {
        SyntaxColoring[Element].FGColor = FGColor;
        SyntaxColoring[Element].BGColor = BGColor;
      }
      SyntaxColoring[Element].BGColorAuto = AutoBGColor;
    }



    public void SetDefaultColors()
    {
      SetSyntaxColor( C64Studio.Types.ColorableElement.NONE, 0xff800000, 0xffffffff, true );
      SetSyntaxColor( C64Studio.Types.ColorableElement.CODE, 0xff000000, 0xffffffff, true );
      SetSyntaxColor( C64Studio.Types.ColorableElement.LITERAL_STRING, 0xff008000, 0xffffffff, true );
      SetSyntaxColor( C64Studio.Types.ColorableElement.LITERAL_NUMBER, 0xff0000ff, 0xffffffff, true );
      SetSyntaxColor( C64Studio.Types.ColorableElement.LABEL, 0xff800000, 0xffffffff, true );
      SetSyntaxColor( C64Studio.Types.ColorableElement.COMMENT, 0xff008000, 0xffffffff, true );
      SetSyntaxColor( C64Studio.Types.ColorableElement.PSEUDO_OP, 0xffff8000, 0xffffffff, true );
      SetSyntaxColor( C64Studio.Types.ColorableElement.CURRENT_DEBUG_LINE, 0xff000000, 0xffffff00, false );
      SetSyntaxColor( C64Studio.Types.ColorableElement.EMPTY_SPACE, 0xff000000, 0xffffffff, false );
      SetSyntaxColor( C64Studio.Types.ColorableElement.OPERATOR, 0xff000080, 0xffffffff, true );
      SetSyntaxColor( C64Studio.Types.ColorableElement.ERROR_UNDERLINE, 0xffff0000, 0xffffffff, false );
      SetSyntaxColor( C64Studio.Types.ColorableElement.HIGHLIGHTED_SEARCH_RESULTS, 0xff000080, 0xffff8000, true );
      SetSyntaxColor( C64Studio.Types.ColorableElement.SELECTED_TEXT, 0xff800000, 0xff800000, true );
      SetSyntaxColor( C64Studio.Types.ColorableElement.BACKGROUND_CONTROL, 0xff000000, 0xfff0f0f0, true );
      SetSyntaxColor( C64Studio.Types.ColorableElement.CHANGED_DEBUG_ELEMENT, 0xffff0000, 0xfff0f0f0, true );

      foreach ( Types.ColorableElement color in System.Enum.GetValues( typeof( Types.ColorableElement ) ) )
      {
        if ( !SyntaxColoring.ContainsKey( color ) )
        {
          SyntaxColoring[color] = new Types.ColorSetting( GR.EnumHelper.GetDescription( color ) )
          {
            FGColor = 0xff000000,
            BGColor = 0xffffffff,
            BGColorAuto = false
          };
        }
      }
    }



    public uint FGColor( Types.ColorableElement Element )
    {
      if ( !SyntaxColoring.ContainsKey( Element ) )
      {
        SyntaxColoring[Element] = new Types.ColorSetting( GR.EnumHelper.GetDescription( Element ) )
        {
          FGColor = 0xff000000,
          BGColor = 0xffffffff,
          BGColorAuto = false
        };
      }
      return SyntaxColoring[Element].FGColor;
    }



    public bool BGColorIsAuto( Types.ColorableElement Element )
    {
      if ( !SyntaxColoring.ContainsKey( Element ) )
      {
        SyntaxColoring[Element] = new Types.ColorSetting( GR.EnumHelper.GetDescription( Element ) )
        {
          FGColor = 0xff000000,
          BGColor = 0xffffffff,
          BGColorAuto = false
        };
      }
      return SyntaxColoring[Element].BGColorAuto;
    }



    public uint BGColor( Types.ColorableElement Element )
    {
      if ( !SyntaxColoring.ContainsKey( Element ) )
      {
        SyntaxColoring[Element] = new Types.ColorSetting( GR.EnumHelper.GetDescription( Element ) )
        {
          FGColor = 0xff000000,
          BGColor = 0xffffffff,
          BGColorAuto = false
        };
      }
      if ( SyntaxColoring[Element].BGColorAuto )
      {
        return BGColor( ColorableElement.EMPTY_SPACE );
      }
      return SyntaxColoring[Element].BGColor;
    }



    public void SanitizeSettings()
    {
      if ( SyntaxColoring.Count == 0 )
      {
        SetDefaultColors();
      }

      foreach ( var syntaxColor in SyntaxColoring )
      {
        var trueBGColor = syntaxColor.Value.BGColor;
        if ( syntaxColor.Value.BGColorAuto )
        {
          trueBGColor = SyntaxColoring[Types.ColorableElement.EMPTY_SPACE].BGColor;
        }
        if ( trueBGColor == syntaxColor.Value.FGColor )
        {
          // white on white or similar -> force black on white
          syntaxColor.Value.FGColor = 0xff000000;
        }
      }

      if ( ( AllowTabs )
      &&   ( TabConvertToSpaces ) )
      {
        // both are weird
        TabConvertToSpaces = true;
        AllowTabs = false;
      }
      if ( TabSize < 1 )
      {
        TabSize = 1;
      }
      // randomly big value, no reason
      if ( TabSize > 800 )
      {
        TabSize = 800;
      }

      // key bindings
      ValidateOrSetKeyBindingKey( C64Studio.Types.Function.SAVE_DOCUMENT, Keys.Control | Keys.S );
      ValidateOrSetKeyBindingKey( C64Studio.Types.Function.BUILD_AND_RUN, Keys.Control | Keys.F5 );
      ValidateOrSetKeyBindingKey( C64Studio.Types.Function.BUILD, Keys.F7 );
      ValidateOrSetKeyBindingKey( C64Studio.Types.Function.BUILD_AND_DEBUG, Keys.F5 );
      ValidateOrSetKeyBindingKey( C64Studio.Types.Function.DEBUG_GO, Keys.F5 );
      ValidateOrSetKeyBindingKey( C64Studio.Types.Function.DEBUG_STEP_OUT, Keys.F9 );
      ValidateOrSetKeyBindingKey( C64Studio.Types.Function.CENTER_ON_CURSOR, Keys.Clear );
      ValidateOrSetKeyBindingKey( C64Studio.Types.Function.DELETE_LINE, Keys.Control | Keys.Y );
      ValidateOrSetKeyBindingKey( C64Studio.Types.Function.DEBUG_STEP, Keys.F11 );
      ValidateOrSetKeyBindingKey( C64Studio.Types.Function.DEBUG_STEP_OVER, Keys.F10 );
      ValidateOrSetKeyBindingKey( C64Studio.Types.Function.DEBUG_STOP, Keys.Shift | Keys.F5 );
      ValidateOrSetKeyBindingKey( C64Studio.Types.Function.DEBUG_RUN_TO, Keys.Control | Keys.F10 );
      ValidateOrSetKeyBindingKey( C64Studio.Types.Function.GO_TO_DECLARATION, Keys.Control | Keys.G );
      ValidateOrSetKeyBindingKey( C64Studio.Types.Function.COMPILE, Keys.Control | Keys.B );
      ValidateOrSetKeyBindingKey( C64Studio.Types.Function.FIND, Keys.Control | Keys.F );
      ValidateOrSetKeyBindingKey( C64Studio.Types.Function.FIND_NEXT, Keys.Control | Keys.L );
      ValidateOrSetKeyBindingKey( C64Studio.Types.Function.FIND_REPLACE, Keys.Control | Keys.H );
      ValidateOrSetKeyBindingKey( C64Studio.Types.Function.FIND_IN_PROJECT, Keys.Control | Keys.Shift | Keys.F );
      ValidateOrSetKeyBindingKey( C64Studio.Types.Function.REPLACE_IN_PROJECT, Keys.Control | Keys.Shift | Keys.H );
      ValidateOrSetKeyBindingKey( C64Studio.Types.Function.SAVE_ALL, Keys.Control | Keys.Shift | Keys.S );
      ValidateOrSetKeyBindingKey( C64Studio.Types.Function.PRINT, Keys.Control | Keys.P );
      ValidateOrSetKeyBindingKey( C64Studio.Types.Function.HELP, Keys.F1 );
      ValidateOrSetKeyBindingKey( C64Studio.Types.Function.SAVE_DOCUMENT_AS, Keys.F12 );
      ValidateOrSetKeyBindingKey( C64Studio.Types.Function.TOGGLE_BREAKPOINT, Keys.Shift | Keys.F9 );
      ValidateOrSetKeyBindingKey( C64Studio.Types.Function.UNDO, Keys.Alt | Keys.Back );
      ValidateOrSetKeyBindingKey( C64Studio.Types.Function.REDO, Keys.Shift | Keys.Alt | Keys.Back );
      ValidateOrSetKeyBindingKey( C64Studio.Types.Function.COPY, Keys.Control | Keys.C );
      ValidateOrSetKeyBindingKey( C64Studio.Types.Function.PASTE, Keys.Control | Keys.V );
      ValidateOrSetKeyBindingKey( C64Studio.Types.Function.CUT, Keys.Control | Keys.X );
    }



    private void ValidateOrSetKeyBindingKey( Function Func, Keys KeyBinding )
    {
      foreach ( var accPair in Accelerators )
      {
        if ( accPair.Value.Function == Func )
        {
          return;
        }
      }
      SetKeyBindingKey( Func, KeyBinding );
    }

  }
}
