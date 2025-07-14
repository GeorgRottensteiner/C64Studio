﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.ComponentModel;
using GR.Memory;
using RetroDevStudio.Types;
using System.Drawing;
using RetroDevStudio.Documents;
using System.Linq;
using System.Management;

namespace RetroDevStudio
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
    [Description( "Compiler Messages" )]
    COMPILE_RESULT,
    [Description( "Breakpoints" )]
    DEBUG_BREAKPOINTS,
    [Description( "Watch" )]
    DEBUG_WATCH,
    [Description( "Memory" )]
    DEBUG_MEMORY,
    [Description( "Registers" )]
    DEBUG_REGISTERS,
    [Description( "Charset Editor" )]
    CHARSET_EDITOR,
    [Description( "Sprite Editor" )]
    SPRITE_EDITOR,
    [Description( "Text Screen Editor" )]
    CHAR_SCREEN_EDITOR,
    [Description( "Graphic Screen Editor" )]
    GRAPHIC_SCREEN_EDITOR,
    [Description( "Map Editor" )]
    MAP_EDITOR,
    [Description( "PETSCII" )]
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
    VALUE_TABLE_EDITOR,
    [Description( "Find References" )]
    FIND_REFERENCES,
    [Description( "Bookmarks") ]
    BOOKMARKS,
    [Description( "Label Explorer" )]
    LABEL_EXPLORER,
    [Description( "Palette Editor" )]
    PALETTE_EDITOR
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
    public Types.PhysicalKey  KeyboardKey = RetroDevStudio.Types.PhysicalKey.UNDEFINED;
    public System.Windows.Forms.Keys Key = System.Windows.Forms.Keys.None;
  };

  public class DebugMemoryViewSettings
  {
    public int      DebugMemoryOffset = 0;
    public int      DebugMemoryByteOffset = 0;
    public int      DebugMemoryNumBytesPerLine = 8;
  };

  public enum SortBy
  {
    INDEX = 0,
    ALPHABET,
    TYPE
  }

  public class StudioSettings
  {
    public StudioCore                           Core = null;

    public GR.Collections.MultiMap<Keys, AcceleratorKey> Accelerators = new GR.Collections.MultiMap<Keys, AcceleratorKey>();

    public List<ToolInfo>                       ToolInfos = new List<ToolInfo>();
    public ToolInfo                             ToolTiny64 = new ToolInfo();

    public List<string>                         MRUProjects = new List<string>();
    public List<string>                         MRUFiles = new List<string>();

    public Dictionary<string,BaseDocument>      GenericTools = new Dictionary<string, BaseDocument>();

    public Dictionary<string, Dialogs.DlgDeactivatableMessage.UserChoice>       StoredDialogResults = new Dictionary<string, Dialogs.DlgDeactivatableMessage.UserChoice>();

    public bool                                 PlaySoundOnSuccessfulBuild = true;
    public bool                                 PlaySoundOnBuildFailure = true;
    public bool                                 PlaySoundOnSearchFoundNoItem = true;
    public string                               MainWindowPlacement = "";
    public bool                                 TrueDriveEnabled = true;
    public string                               EmulatorToRun = "";
    public bool                                 AutoOpenLastSolution = false;
    public bool                                 LastSolutionWasEmpty = false;
    public bool                                 ShowCompilerMessagesAfterBuild = true;
    public bool                                 ShowOutputDisplayAfterBuild = true;

    public int                                  TabSize             = 2;
    public int                                  CaretWidth          = 1;
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
    public bool                                 ASMShowShortCutLabels = true;
    public int                                  ASMShowMaxLineLengthIndicatorLength = 0;
    public bool                                 ASMLabelFileIgnoreAssemblerIDLabels = false;

    public bool                                 StripTrailingSpaces = false;

    public bool                                 OutlineShowLocalLabels = true;
    public bool                                 OutlineShowShortCutLabels = true;
    public SortBy                               OutlineSorting = SortBy.INDEX;
    public string                               OutlineFilter = "";

    public bool                                 LabelExplorerShowLocalLabels = true;
    public bool                                 LabelExplorerShowShortCutLabels = true;
    public SortBy                               LabelExplorerSorting = SortBy.INDEX;
    public string                               LabelExplorerFilter = "";

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
    public int                                  BASICShowMaxLineLengthIndicatorLength = 80;

    public bool                                 BehaviourRightClickIsBGColorPaint = false;

    public List<string>                         FindArguments = new List<string>();
    public List<string>                         ReplaceArguments = new List<string>();
    public List<string>                         ReplaceWithArguments = new List<string>();
    public bool                                 LastFindIgnoreCase = true;
    public bool                                 LastFindWholeWord = false;
    public bool                                 LastFindRegexp = false;
    public bool                                 LastFindWrap = true;
    public int                                  LastFindTarget = 1;

    public BASICKeyMap                          BASICKeyMap = new BASICKeyMap();
    public bool                                 BASICStripSpaces = true;
    public bool                                 BASICShowControlCodesAsChars = true;
    public bool                                 BASICAutoToggleEntryMode = true;
    public bool                                 BASICAutoToggleEntryModeOnPosition = true;
    public bool                                 BASICStripREM = false;

    public MemoryDisplayType                    MemoryDisplay = MemoryDisplayType.ASCII;
    public MemorySourceType                     MemorySource = MemorySourceType.CPU;
    public TextMode                             MemoryDisplayCharsetMode = TextMode.COMMODORE_40_X_25_HIRES;
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

    public MachineType                          PreferredMachineType = MachineType.C64;

    public Encoding                             SourceFileEncoding = Encoding.UTF8;

    public int                                  HelpZoomFactor = 100;

    public List<DebugMemoryViewSettings>        DebugMemoryViews = new List<DebugMemoryViewSettings>();

    public bool                                 CheckForUpdates = true;
    public DateTime                             LastUpdateCheck = DateTime.MinValue;

    public AppMode                              StudioAppMode = AppMode.UNDECIDED;

    public Dictionary<PaletteType,List<Palette>>  Palettes = new Dictionary<PaletteType, List<Palette>>();

    public GR.Collections.Set<Types.ErrorCode>  IgnoredWarnings = new GR.Collections.Set<RetroDevStudio.Types.ErrorCode>();
    public GR.Collections.Set<Types.ErrorCode>  TreatWarningsAsErrors = new GR.Collections.Set<RetroDevStudio.Types.ErrorCode>();
    public GR.Collections.Set<Parser.AssemblerSettings.Hacks>  EnabledC64StudioHacks = new GR.Collections.Set<Parser.AssemblerSettings.Hacks>();

    public string                               DefaultProjectBasePath = "";
    public WeifenLuo.WinFormsUI.Docking.DockPanel   PanelMain = null;

    public GR.Collections.Map<Types.ColorableElement,Types.ColorSetting>  SyntaxColoring = new GR.Collections.Map<RetroDevStudio.Types.ColorableElement, RetroDevStudio.Types.ColorSetting>();

    public GR.Collections.Map<Types.Function, Types.FunctionInfo> Functions = new GR.Collections.Map<RetroDevStudio.Types.Function, Types.FunctionInfo>();

    public GR.Collections.Map<ToolWindowType, ToolWindow> Tools = new GR.Collections.Map<ToolWindowType, ToolWindow>();

    public MainForm                             Main = null;

    private DeserializeDockContent m_deserializeDockContent;

    public PerspectiveDetails                   Perspectives = null;

    public SourceControlInfo                    SourceControlInfo = new SourceControlInfo();
    public DialogSettings                       DialogSettings = new DialogSettings();

    private int                                 _functionIndex = 0;



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
      RegisterFunction( Function.BUILD_TO_RELOCATION_FILE, "Build Relocation File", FunctionStudioState.NORMAL );

      // functions for any state
      RegisterFunction( Function.CENTER_ON_CURSOR, "Center on Cursor", FunctionStudioState.ANY );
      RegisterFunction( Function.DELETE_LINE, "Delete Line", FunctionStudioState.ANY );
      RegisterFunction( Function.FIND, "Find", FunctionStudioState.ANY );
      RegisterFunction( Function.FIND_IN_PROJECT, "Find in Project", FunctionStudioState.ANY );
      RegisterFunction( Function.FIND_NEXT, "Find Next", FunctionStudioState.ANY );
      RegisterFunction( Function.FIND_REPLACE, "Replace", FunctionStudioState.ANY );
      RegisterFunction( Function.FIND_ALL_REFERENCES, "Find all references", FunctionStudioState.ANY );
      RegisterFunction( Function.RENAME_ALL_REFERENCES, "Rename all references", FunctionStudioState.ANY );
      RegisterFunction( Function.REPLACE_IN_PROJECT, "Replace in Project", FunctionStudioState.ANY );

      RegisterFunction( Function.GO_TO_DECLARATION, "Go To Declaration", FunctionStudioState.ANY );
      RegisterFunction( Function.HELP, "Help", FunctionStudioState.ANY );
      RegisterFunction( Function.PRINT, "Print", FunctionStudioState.ANY );
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
      RegisterFunction( Function.TOGGLE_SELECTION, "Toggle Selection", FunctionStudioState.ANY );
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
      RegisterFunction( Function.GRAPHIC_ELEMENT_BACKGROUND_COLOR, "Background Color", FunctionStudioState.ANY );
      RegisterFunction( Function.GRAPHIC_ELEMENT_NEXT, "Next Element", FunctionStudioState.ANY );
      RegisterFunction( Function.GRAPHIC_ELEMENT_PREVIOUS, "Previous Element", FunctionStudioState.ANY );

      RegisterFunction( Function.BOOKMARK_ADD, "Add Bookmark", FunctionStudioState.ANY );
      RegisterFunction( Function.BOOKMARK_DELETE, "Delete Bookmark", FunctionStudioState.ANY );
      RegisterFunction( Function.BOOKMARK_DELETE_ALL, "Delete All Bookmarks", FunctionStudioState.ANY );
      RegisterFunction( Function.BOOKMARK_NEXT, "Next Bookmark", FunctionStudioState.ANY );
      RegisterFunction( Function.BOOKMARK_PREVIOUS, "Previous Bookmark", FunctionStudioState.ANY );

      RegisterFunction( Function.NAVIGATE_BACK, "Navigate Backward", FunctionStudioState.ANY );
      RegisterFunction( Function.NAVIGATE_FORWARD, "Navigate Forward", FunctionStudioState.ANY );

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

      // start with default palettes
      Palettes.Add( PaletteType.C64, new List<Palette>() { ConstantData.DefaultPaletteC64() } );
      Palettes.Add( PaletteType.C128_VDC, new List<Palette>() { ConstantData.DefaultPaletteC128() } );
      Palettes.Add( PaletteType.VIC20, new List<Palette>() { ConstantData.DefaultPaletteVIC20() } );
      Palettes.Add( PaletteType.MEGA65, new List<Palette>() { ConstantData.DefaultPaletteMega65_256() } );
      Palettes.Add( PaletteType.COMMANDER_X16, new List<Palette>() { ConstantData.DefaultPaletteCommanderX16() } );
      Palettes.Add( PaletteType.NES, new List<Palette>() { ConstantData.DefaultPaletteNES() } );
    }



    private void RegisterFunction( Function StudioFunction, string Description, FunctionStudioState StudioState )
    {
      Functions.Add( StudioFunction, new FunctionInfo( StudioFunction, Description, StudioState, _functionIndex ) );
      ++_functionIndex;
    }



    private IDockContent GetContentFromPersistString( string persistString )
    {
      if ( string.IsNullOrEmpty( persistString ) )
      {
        return null;
      }
      //Debug.Log( $"persistString = {persistString}" );

      foreach ( var toolEntry in Tools )
      {
        if ( persistString == toolEntry.Value.ToolDescription )
        {
          //Debug.Log( $"--found as actual tool" );
          return toolEntry.Value.Document;
        }
      }
      // manual dynamic view?
      switch ( persistString )
      {
        case "Memory View":
          {
            var view = new DebugMemory( Core );
            // mark as "memory view" (which means additional memory view)
            view.Text = "Memory View";
            view.Hide();

            //Debug.Log( $"--found as additional memory view" );

            return view;
          }
      }
      //Debug.Log( "persist doc not found for " + persistString );
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

      Core.Theming.ApplyThemeToToolStripItems( Core.MainForm.MainMenuStrip, Core.MainForm.MainMenuStrip.Items );

      //foreach ( WeifenLuo.WinFormsUI.Docking.IDockContent doc in Core.MainForm.panelMain.Documents )
      foreach ( WeifenLuo.WinFormsUI.Docking.IDockContent doc in Core.MainForm.panelMain.Contents )
      {
        if ( doc is BaseDocument )
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
        case RetroDevStudio.Types.StudioState.NORMAL:
          functionMask |= FunctionStudioState.NORMAL;
          break;
        case RetroDevStudio.Types.StudioState.COMPILE:
        case RetroDevStudio.Types.StudioState.BUILD:
        case RetroDevStudio.Types.StudioState.BUILD_AND_DEBUG:
        case RetroDevStudio.Types.StudioState.BUILD_AND_RUN:
          functionMask |= FunctionStudioState.BUILDING;
          break;
        case RetroDevStudio.Types.StudioState.DEBUGGING_BROKEN:
          functionMask |= FunctionStudioState.DEBUGGER_BROKEN;
          break;
        case RetroDevStudio.Types.StudioState.DEBUGGING_RUN:
          functionMask |= FunctionStudioState.DEBUGGER_RUNNING;
          break;
      }
      return functionMask;
    }



    public AcceleratorKey DetermineAccelerator( Types.Function Function )
    {
      return Accelerators.Values.FirstOrDefault( acc => acc.Function == Function );
    }



    public AcceleratorKey FindAccelerator( Keys KeyCombination )
    {
      return Accelerators.Values.FirstOrDefault( acc => ( acc.Key == KeyCombination ) || ( acc.SecondaryKey == KeyCombination ) );
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
      foreach ( RetroDevStudio.LayoutInfo layout in ToolLayout.Values )
      {
        layout.StoreLayout();
        SettingsData.Append( layout.ToBuffer() );
      }*/

      foreach ( AcceleratorKey key in Accelerators.Values )
      {
        GR.IO.FileChunk   chunkKey = key.ToChunk();
        SettingsData.Append( chunkKey.ToBuffer() );
      }

      GR.IO.FileChunk   chunkSoundSettings = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_SOUND );
      chunkSoundSettings.AppendU8( (byte)( PlaySoundOnSuccessfulBuild ? 1 : 0 ) );
      chunkSoundSettings.AppendU8( (byte)( PlaySoundOnBuildFailure ? 1 : 0 ) );
      chunkSoundSettings.AppendU8( (byte)( PlaySoundOnSearchFoundNoItem ? 1 : 0 ) );
      SettingsData.Append( chunkSoundSettings.ToBuffer() );

      GR.IO.FileChunk   chunkMainWindowPlacement = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_WINDOW );
      chunkMainWindowPlacement.AppendString( MainWindowPlacement );
      SettingsData.Append( chunkMainWindowPlacement.ToBuffer() );

      GR.IO.FileChunk   chunkStoredDialogResults = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_DIALOG_DECISIONS );
      chunkStoredDialogResults.AppendI32( StoredDialogResults.Count );
      foreach ( var entry in StoredDialogResults )
      {
        chunkStoredDialogResults.AppendString( entry.Key );
        chunkStoredDialogResults.AppendI32( (int)entry.Value );
      }
      SettingsData.Append( chunkStoredDialogResults.ToBuffer() );

      GR.IO.FileChunk chunkTabs = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_TEXT_EDITOR );
      chunkTabs.AppendI32( TabSize );
      chunkTabs.AppendU8( 1 );// (byte)( AllowTabs ? 1 : 0 ) );
      chunkTabs.AppendU8( (byte)( TabConvertToSpaces ? 1 : 0 ) );
      chunkTabs.AppendU8( (byte)( StripTrailingSpaces ? 1 : 0 ) );
      chunkTabs.AppendString( SourceFileEncoding.WebName );
      chunkTabs.AppendI32( BASICShowMaxLineLengthIndicatorLength );
      chunkTabs.AppendI32( ASMShowMaxLineLengthIndicatorLength );
      chunkTabs.AppendI32( CaretWidth );
      SettingsData.Append( chunkTabs.ToBuffer() );

      GR.IO.FileChunk chunkFont = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_FONT );
      chunkFont.AppendString( SourceFontFamily );
      // -1 means use the float value later on
      chunkFont.AppendI32( -1 );
      chunkFont.AppendU8( (byte)( BASICUseNonC64Font ? 1 : 0 ) );
      chunkFont.AppendString( BASICSourceFontFamily );
      // -1 means use the float value later on
      chunkFont.AppendI32( -1 );
      chunkFont.AppendI32( (int)SourceFontStyle );
      chunkFont.AppendI32( (int)BASICSourceFontStyle );
      chunkFont.AppendF32( SourceFontSize );
      chunkFont.AppendF32( BASICSourceFontSize );

      SettingsData.Append( chunkFont.ToBuffer() );

      foreach ( Types.ColorableElement element in SyntaxColoring.Keys )
      {
        GR.IO.FileChunk chunkSyntaxColor = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_SYNTAX_COLORING );

        chunkSyntaxColor.AppendU32( (uint)element );
        chunkSyntaxColor.AppendU32( SyntaxColoring[element].FGColor );
        chunkSyntaxColor.AppendU32( SyntaxColoring[element].BGColor );
        chunkSyntaxColor.AppendI32( SyntaxColoring[element].BGColorAuto ? 1 : 0 );

        SettingsData.Append( chunkSyntaxColor.ToBuffer() );
      }

      GR.IO.FileChunk chunkUI = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_UI );
      chunkUI.AppendU8( (byte)( ToolbarActiveMain ? 1 : 0 ) );
      chunkUI.AppendU8( (byte)( ToolbarActiveDebugger ? 1 : 0 ) );
      chunkUI.AppendI32( MRUMaxCount );
      SettingsData.Append( chunkUI.ToBuffer() );

      GR.IO.FileChunk chunkRunEmu = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_RUN_EMULATOR );
      chunkRunEmu.AppendU8( (byte)( TrueDriveEnabled ? 1 : 0 ) );
      chunkRunEmu.AppendString( EmulatorToRun );
      SettingsData.Append( chunkRunEmu.ToBuffer() );

      GR.IO.FileChunk chunkDefaults = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_DEFAULTS );
      chunkDefaults.AppendString( DefaultProjectBasePath );
      chunkDefaults.AppendU32( (uint)PreferredMachineType );
      SettingsData.Append( chunkDefaults.ToBuffer() );

      // dockpanel layout
      GR.IO.FileChunk chunkLayout = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_DPS_LAYOUT );
      System.IO.MemoryStream    memOut = new System.IO.MemoryStream();

      PanelMain.SaveAsXml( memOut, Encoding.UTF8 );
      byte[] layoutData = memOut.ToArray();
      string xmlOutText = System.Text.Encoding.UTF8.GetString( layoutData );

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
      chunkLayout.AppendU32( (uint)layoutData.Length );
      chunkLayout.Append( layoutData );

      Core.Settings.Perspectives.StoreActiveContent( Core.MainForm.m_ActivePerspective );
      chunkLayout.Append( Core.Settings.Perspectives.ToBuffer() );

      SettingsData.Append( chunkLayout.ToBuffer() );
      memOut.Close();
      memOut.Dispose();

      GR.IO.FileChunk chunkFindReplace = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_FIND_REPLACE );
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
      chunkFindReplace.AppendI32( ReplaceWithArguments.Count );
      foreach ( var replaceArg in ReplaceWithArguments )
      {
        chunkFindReplace.AppendString( replaceArg );
      }
      SettingsData.Append( chunkFindReplace.ToBuffer() );

      GR.IO.FileChunk chunkIgnoredWarnings = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_IGNORED_WARNINGS );
      chunkIgnoredWarnings.AppendI32( IgnoredWarnings.Count );
      foreach ( Types.ErrorCode ignoredWarning in IgnoredWarnings )
      {
        chunkIgnoredWarnings.AppendI32( (int)ignoredWarning );
      }
      SettingsData.Append( chunkIgnoredWarnings.ToBuffer() );

      GR.IO.FileChunk chunkWarningsAsErrors = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_WARNINGS_AS_ERRORS );
      chunkWarningsAsErrors.AppendI32( TreatWarningsAsErrors.Count );
      foreach ( Types.ErrorCode warningAsError in TreatWarningsAsErrors )
      {
        chunkWarningsAsErrors.AppendI32( (int)warningAsError );
      }
      SettingsData.Append( chunkWarningsAsErrors.ToBuffer() );

      GR.IO.FileChunk chunkEditorBehaviours = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_EDITOR_BEHAVIOURS );
      chunkEditorBehaviours.AppendI32( BehaviourRightClickIsBGColorPaint ? 1 : 0 );
      SettingsData.Append( chunkEditorBehaviours.ToBuffer() );

      

      GR.IO.FileChunk chunkC64StudioHacks = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_C64STUDIO_HACKS );
      chunkC64StudioHacks.AppendI32( EnabledC64StudioHacks.Count );
      foreach ( Types.ErrorCode c64StudioHack in EnabledC64StudioHacks )
      {
        chunkC64StudioHacks.AppendI32( (int)c64StudioHack );
      }
      SettingsData.Append( chunkC64StudioHacks.ToBuffer() );


      foreach ( var pair in GenericTools )
      {
        GR.Memory.ByteBuffer    displayDetailData = pair.Value.DisplayDetails();
        if ( displayDetailData != null )
        {
          GR.IO.FileChunk   chunkPanelDisplayDetails = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_PANEL_DISPLAY_DETAILS );

          chunkPanelDisplayDetails.AppendString( pair.Key );

          chunkPanelDisplayDetails.AppendU32( displayDetailData.Length );
          chunkPanelDisplayDetails.Append( displayDetailData );

          SettingsData.Append( chunkPanelDisplayDetails.ToBuffer() );
        }
      }

      // ASM editor settings
      GR.IO.FileChunk chunkASMEditor = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_ASSEMBLER_EDITOR );
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
      chunkASMEditor.AppendU8( (byte)( !ASMShowShortCutLabels ? 1 : 0 ) );
      chunkASMEditor.AppendU8( (byte)( ASMLabelFileIgnoreAssemblerIDLabels ? 1 : 0 ) );
      SettingsData.Append( chunkASMEditor.ToBuffer() );

      // Outline settings
      GR.IO.FileChunk chunkOutline = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_OUTLINE );
      chunkOutline.AppendU8( (byte)( !OutlineShowLocalLabels ? 1 : 0 ) );
      chunkOutline.AppendU8( (byte)( !OutlineShowShortCutLabels ? 1 : 0 ) );
      chunkOutline.AppendU8( (byte)( OutlineSorting ) );
      chunkOutline.AppendString( OutlineFilter );
      SettingsData.Append( chunkOutline.ToBuffer() );

      // Label Explorer settings
      GR.IO.FileChunk chunkLabelExplorer = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_LABEL_EXPLORER );
      chunkLabelExplorer.AppendU8( (byte)( !LabelExplorerShowLocalLabels ? 1 : 0 ) );
      chunkLabelExplorer.AppendU8( (byte)( !LabelExplorerShowShortCutLabels ? 1 : 0 ) );
      chunkLabelExplorer.AppendU8( (byte)( LabelExplorerSorting ) );
      chunkLabelExplorer.AppendString( LabelExplorerFilter );
      SettingsData.Append( chunkLabelExplorer.ToBuffer() );


      // BASIC settings
      GR.IO.FileChunk chunkBASICParser = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_BASIC_PARSER );
      chunkBASICParser.AppendU8( (byte)( BASICStripSpaces ? 1 : 0 ) );
      chunkBASICParser.AppendU8( (byte)( BASICShowControlCodesAsChars ? 1 : 0 ) );
      chunkBASICParser.AppendU8( (byte)( !BASICAutoToggleEntryMode ? 1 : 0 ) );
      chunkBASICParser.AppendU8( (byte)( BASICStripREM ? 1 : 0 ) );
      chunkBASICParser.AppendU8( (byte)( BASICAutoToggleEntryModeOnPosition ? 1 : 0 ) );
      SettingsData.Append( chunkBASICParser.ToBuffer() );

      // BASIC key map
      GR.IO.FileChunk chunkBASICKeyMap = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_BASIC_KEYMAP );

      chunkBASICKeyMap.AppendI32( BASICKeyMap.Keymap.Count );
      foreach ( var entry in BASICKeyMap.Keymap )
      {
        chunkBASICKeyMap.AppendU32( (uint)entry.Key );
        chunkBASICKeyMap.AppendI32( (int)entry.Value.KeyboardKey );
      }
      SettingsData.Append( chunkBASICKeyMap.ToBuffer() );

      // environment behaviour settings
      GR.IO.FileChunk chunkEnvironment = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_ENVIRONMENT );
      chunkEnvironment.AppendU8( (byte)( AutoOpenLastSolution ? 1 : 0 ) );
      chunkEnvironment.AppendU8( (byte)( LastSolutionWasEmpty ? 1 : 0 ) );
      chunkEnvironment.AppendU8( (byte)( !CheckForUpdates ? 1 : 0 ) );
      chunkEnvironment.AppendU8( (byte)LastUpdateCheck.Day );
      chunkEnvironment.AppendU8( (byte)LastUpdateCheck.Month );
      chunkEnvironment.AppendI32( (byte)LastUpdateCheck.Year );
      chunkEnvironment.AppendU8( (byte)( !ShowCompilerMessagesAfterBuild ? 1 : 0 ) );
      chunkEnvironment.AppendU8( (byte)( !ShowOutputDisplayAfterBuild ? 1 : 0 ) );

      SettingsData.Append( chunkEnvironment.ToBuffer() );

      // tool window settings
      GR.IO.FileChunk chunkPerspectives = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_PERSPECTIVES );
      foreach ( Perspective perspective in System.Enum.GetValues( typeof( Perspective ) ) )
      {
        GR.IO.FileChunk chunkPerspective = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_PERSPECTIVE );

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
      GR.IO.FileChunk chunkHexView = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_HEX_VIEW );

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
      GR.IO.FileChunk chunkMRUProjects = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_MRU_PROJECTS );

      chunkMRUProjects.AppendI32( MRUProjects.Count );
      for ( int i = 0; i < MRUProjects.Count; ++i )
      {
        chunkMRUProjects.AppendString( MRUProjects[i] );
      }
      SettingsData.Append( chunkMRUProjects.ToBuffer() );

      // MRU files
      GR.IO.FileChunk chunkMRUFiles = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_MRU_FILES );

      chunkMRUFiles.AppendI32( MRUFiles.Count );
      for ( int i = 0; i < MRUFiles.Count; ++i )
      {
        chunkMRUFiles.AppendString( MRUFiles[i] );
      }
      SettingsData.Append( chunkMRUFiles.ToBuffer() );

      // SC info
      GR.IO.FileChunk chunkSC = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_SOURCE_CONTROL );
      chunkSC.AppendString( SourceControlInfo.CommitAuthor );
      chunkSC.AppendString( SourceControlInfo.CommitAuthorEmail );
      chunkSC.AppendI32( SourceControlInfo.CreateSolutionRepository ? 0 : 1 );
      chunkSC.AppendI32( SourceControlInfo.CreateProjectRepository ? 0 : 1 );
      SettingsData.Append( chunkSC.ToBuffer() );

      // Help
      GR.IO.FileChunk chunkHelp = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_HELP );
      chunkHelp.AppendI32( HelpZoomFactor );
      SettingsData.Append( chunkHelp.ToBuffer() );

      // Memory View
      foreach ( var debugView in Core.Debugging.MemoryViews )
      {
        GR.IO.FileChunk chunkMemoryView = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_MEMORY_VIEW );

        chunkMemoryView.AppendI32( debugView.Offset );
        chunkMemoryView.AppendI32( debugView.ByteOffset );
        chunkMemoryView.AppendI32( debugView.ByteWidth );

        SettingsData.Append( chunkMemoryView.ToBuffer() );
      }

      // dialog appearance info
      SettingsData.Append( DialogSettings.ToBuffer() );

      // Palettes
      foreach ( var palSystem in Palettes )
      {
        foreach ( var palette in palSystem.Value )
        {
          GR.IO.FileChunk chunkPalette = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_PALETTE );

          chunkPalette.AppendI32( (int)palSystem.Key );
          chunkPalette.Append( palette.ToBuffer() );

          SettingsData.Append( chunkPalette.ToBuffer() );
        }
      }

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

      

      if ( ( Data.Length >= 3 )
      &&   ( Data.ByteAt( 0 ) == 0xef )
      &&   ( Data.ByteAt( 1 ) == 0xbb )
      &&   ( Data.ByteAt( 2 ) == 0xbf ) )
      {
        // strip BOM
        Data = Data.SubBuffer( 3 );
      }

      /*
      var layout = System.Text.Encoding.UTF8.GetString( Data.Data() );
      layout = layout.Replace( "PersistString=\"\"", "PersistString=\"DummyContent\"" );
      Data = new ByteBuffer( System.Text.Encoding.UTF8.GetBytes( layout ) );
      */

      System.IO.MemoryStream    memIn = new System.IO.MemoryStream( Data.Data(), false );

      //var layout = System.Text.Encoding.UTF8.GetString( Data.Data() );
      //Debug.Log( "Layout from" + layout );

      try
      {
        PanelMain.LoadFromXml( memIn, m_deserializeDockContent );
      }
      catch ( Exception ex )
      {
        Debug.Log( "SetLayoutFromData: " + ex.ToString() );
        Debug.Log( Data.ToAsciiString() );
      }

      memIn.Close();
      memIn.Dispose();
      PanelMain.ResumeLayout( false, true );
    }



    public bool ReadFromBuffer( GR.Memory.ByteBuffer SettingsData )
    {
      IgnoredWarnings.Clear();
      Palettes.Clear();

      GR.IO.BinaryReader binReader = new GR.IO.BinaryReader( SettingsData.MemoryStream() );

      GR.IO.FileChunk chunkData = new GR.IO.FileChunk();

      while ( chunkData.ReadFromStream( binReader ) )
      {
        switch ( chunkData.Type )
        {
          case FileChunkConstants.SETTINGS_TOOL:
            {
              ToolInfo tool = new ToolInfo();

              tool.FromChunk( chunkData );

              // sanitize args
              tool.CartArguments  = tool.CartArguments.Replace( "$(BuildTargetFilename)", "$(RunFilename)" );
              tool.PRGArguments   = tool.PRGArguments.Replace( "$(BuildTargetFilename)", "$(RunFilename)" );
              tool.WorkPath       = tool.WorkPath.Replace( "$(FilePath)", "$(RunPath)" );

              ToolInfos.Add( tool );
            }
            break;
          case FileChunkConstants.SETTINGS_ACCELERATOR:
            {
              AcceleratorKey key = new AcceleratorKey();

              key.FromChunk( chunkData );

              Accelerators.Add( key.Key, key );
            }
            break;
          case FileChunkConstants.SETTINGS_DPS_LAYOUT:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              uint                    size = binIn.ReadUInt32();
              GR.Memory.ByteBuffer    tempData = new GR.Memory.ByteBuffer();
              binIn.ReadBlock( tempData, size );
              SetLayoutFromData( tempData );

              Perspectives.ReadFromBuffer( binIn );
            }
            break;
          case FileChunkConstants.SETTINGS_SOUND:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              PlaySoundOnSuccessfulBuild    = ( binIn.ReadUInt8() != 0 );
              PlaySoundOnBuildFailure       = ( binIn.ReadUInt8() != 0 );
              PlaySoundOnSearchFoundNoItem  = ( binIn.ReadUInt8() != 0 );
            }
            break;
          case FileChunkConstants.SETTINGS_DIALOG_DECISIONS:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              int numEntries = binIn.ReadInt32();

              for ( int i = 0; i < numEntries; ++i )
              {
                string  key = binIn.ReadString();
                Dialogs.DlgDeactivatableMessage.UserChoice choice = (Dialogs.DlgDeactivatableMessage.UserChoice)binIn.ReadInt32();

                StoredDialogResults.Add( key, choice );
              }
            }
            break;
          case FileChunkConstants.SETTINGS_WINDOW:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              MainWindowPlacement = binIn.ReadString();
            }
            break;
          case FileChunkConstants.SETTINGS_TEXT_EDITOR:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              TabSize = binIn.ReadInt32();
              if ( ( TabSize <= 0 )
              ||   ( TabSize >= 100 ) )
              {
                TabSize = 2;
              }
              binIn.ReadUInt8();  // was AllowTabs = ( binIn.ReadUInt8() != 0 );
              TabConvertToSpaces  = ( binIn.ReadUInt8() != 0 );
              StripTrailingSpaces = ( binIn.ReadUInt8() != 0 );

              string  encodingName = binIn.ReadString();
              if ( !string.IsNullOrEmpty( encodingName ) )
              {
                try
                {
                  if ( encodingName.ToUpper() == "UTF-8" )
                  {
                    SourceFileEncoding = new System.Text.UTF8Encoding( false );
                  }
                  else
                  {
                    SourceFileEncoding = Encoding.GetEncoding( encodingName );
                  }
                }
                catch ( Exception )
                {
                  SourceFileEncoding = new System.Text.UTF8Encoding( false );
                }
              }

              BASICShowMaxLineLengthIndicatorLength = binIn.ReadInt32();
              if ( ( BASICShowMaxLineLengthIndicatorLength <= 0 )
              ||   ( BASICShowMaxLineLengthIndicatorLength >= 200 ) )
              {
                BASICShowMaxLineLengthIndicatorLength = 80;
              }
              ASMShowMaxLineLengthIndicatorLength = binIn.ReadInt32();
              if ( ( ASMShowMaxLineLengthIndicatorLength <= 0 )
              ||   ( ASMShowMaxLineLengthIndicatorLength >= 200 ) )
              {
                ASMShowMaxLineLengthIndicatorLength = 0;
              }
              CaretWidth = binIn.ReadInt32();
              if ( ( CaretWidth <= 0 )
              ||   ( CaretWidth >= 200 ) )
              {
                CaretWidth = 1;
              }
            }
            break;
          case FileChunkConstants.SETTINGS_FONT:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              SourceFontFamily        = binIn.ReadString();
              int   obsoleteSourceFontSize = binIn.ReadInt32();

              BASICUseNonC64Font      = ( binIn.ReadUInt8() != 0 );
              BASICSourceFontFamily   = binIn.ReadString();
              int obsoleteBASICSourceFontSize = binIn.ReadInt32();

              SourceFontStyle         = (FontStyle)binIn.ReadInt32();
              BASICSourceFontStyle    = (FontStyle)binIn.ReadInt32();

              float sourceFontSize          = binIn.ReadF32();
              float basicSourceFontSize     = binIn.ReadF32();

              if ( obsoleteSourceFontSize == -1 )
              {
                SourceFontSize = sourceFontSize;
              }
              else
              {
                SourceFontSize = (float)obsoleteSourceFontSize;
              }
              if ( obsoleteBASICSourceFontSize == -1 )
              {
                BASICSourceFontSize = basicSourceFontSize;
              }
              else
              {
                BASICSourceFontSize = (float)obsoleteBASICSourceFontSize;
              }
              if ( BASICSourceFontSize <= 0 )
              {
                BASICSourceFontSize = 9.0f;
              }
            }
            break;
          case FileChunkConstants.SETTINGS_SYNTAX_COLORING:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              Types.ColorableElement   element = (RetroDevStudio.Types.ColorableElement)binIn.ReadUInt32();

              Types.ColorSetting color = new RetroDevStudio.Types.ColorSetting( GR.EnumHelper.GetDescription( element ) );
              color.FGColor = binIn.ReadUInt32();
              color.BGColor = binIn.ReadUInt32();
              color.BGColorAuto = ( binIn.ReadUInt32() != 0 );

              color.FGColor |= 0xff000000;
              color.BGColor |= 0xff000000;

              SyntaxColoring.Add( element, color );
            }
            break;
          case FileChunkConstants.SETTINGS_UI:
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
          case FileChunkConstants.SETTINGS_RUN_EMULATOR:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              TrueDriveEnabled = ( binIn.ReadUInt8() != 0 );
              EmulatorToRun = binIn.ReadString();
            }
            break;
          case FileChunkConstants.SETTINGS_DEFAULTS:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              DefaultProjectBasePath  = binIn.ReadString();
              PreferredMachineType    = (MachineType)binIn.ReadUInt32();
              if ( PreferredMachineType == MachineType.ANY )
              {
                PreferredMachineType = MachineType.C64;
              }
            }
            break;
          case FileChunkConstants.SETTINGS_OUTLINE:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              OutlineShowLocalLabels    = ( binIn.ReadUInt8() == 0 );
              OutlineShowShortCutLabels = ( binIn.ReadUInt8() == 0 );
              OutlineSorting            = (SortBy)binIn.ReadUInt8();
              OutlineFilter             = binIn.ReadString();
            }
            break;
          case FileChunkConstants.SETTINGS_LABEL_EXPLORER:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              LabelExplorerShowLocalLabels    = ( binIn.ReadUInt8() == 0 );
              LabelExplorerShowShortCutLabels = ( binIn.ReadUInt8() == 0 );
              LabelExplorerSorting            = (SortBy)binIn.ReadUInt8();
              LabelExplorerFilter             = binIn.ReadString();
            }
            break;
          case FileChunkConstants.SETTINGS_FIND_REPLACE:
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
              FindArguments = FindArguments.Distinct().ToList();
              int numReplaceArguments = binIn.ReadInt32();
              for ( int i = 0; i < numReplaceArguments; ++i )
              {
                ReplaceArguments.Add( binIn.ReadString() );
              }
              ReplaceArguments = ReplaceArguments.Distinct().ToList();
              int numReplaceWithArguments = binIn.ReadInt32();
              for ( int i = 0; i < numReplaceWithArguments; ++i )
              {
                ReplaceWithArguments.Add( binIn.ReadString() );
              }
              ReplaceWithArguments = ReplaceWithArguments.Distinct().ToList();
            }
            break;
          case FileChunkConstants.SETTINGS_IGNORED_WARNINGS:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              int numIgnoredWarnings = binIn.ReadInt32();

              for ( int i = 0; i < numIgnoredWarnings; ++i )
              {
                IgnoredWarnings.Add( (RetroDevStudio.Types.ErrorCode)binIn.ReadInt32() );
              }
            }
            break;
          case FileChunkConstants.SETTINGS_EDITOR_BEHAVIOURS:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              BehaviourRightClickIsBGColorPaint = ( binIn.ReadInt32() == 1 );
            }
            break;
          case FileChunkConstants.SETTINGS_WARNINGS_AS_ERRORS:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              int numWarnignsAsErrors = binIn.ReadInt32();

              for ( int i = 0; i < numWarnignsAsErrors; ++i )
              {
                TreatWarningsAsErrors.Add( (RetroDevStudio.Types.ErrorCode)binIn.ReadInt32() );
              }
            }
            break;
          case FileChunkConstants.SETTINGS_C64STUDIO_HACKS:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              int numC64StudioHacks = binIn.ReadInt32();

              for ( int i = 0; i < numC64StudioHacks; ++i )
              {
                EnabledC64StudioHacks.Add( (Parser.AssemblerSettings.Hacks)binIn.ReadInt32() );
              }
            }
            break;
          case FileChunkConstants.SETTINGS_PANEL_DISPLAY_DETAILS:
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
          case FileChunkConstants.SETTINGS_BASIC_KEYMAP:
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
                Types.PhysicalKey  cmdKey = (RetroDevStudio.Types.PhysicalKey)binIn.ReadInt32();

                var keyMapEntry = new KeymapEntry();

                foreach ( var entry in BASICKeyMap.DefaultKeymaps[neutralLang] )
                {
                  if ( entry.Value.KeyboardKey == cmdKey )
                  {
                    BASICKeyMap.Keymap[key] = new KeymapEntry() { Key = key, KeyboardKey = cmdKey };
                    break;
                  }
                }
                // add simulated keys manually
                if ( ( cmdKey == PhysicalKey.KEY_SIM_CURSOR_LEFT )
                ||   ( cmdKey == PhysicalKey.KEY_SIM_CURSOR_UP ) )
                {
                  BASICKeyMap.Keymap[key] = new KeymapEntry() { Key = key, KeyboardKey = cmdKey };
                }
              }
            }
            break;
          case FileChunkConstants.SETTINGS_ASSEMBLER_EDITOR:
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

              ASMShowAddress        = ( binIn.ReadUInt8() == 0 );
              ASMShowShortCutLabels = ( binIn.ReadUInt8() == 0 );
              ASMLabelFileIgnoreAssemblerIDLabels = ( binIn.ReadUInt8() != 0 );
            }
            break;
          case FileChunkConstants.SETTINGS_BASIC_PARSER:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              BASICStripSpaces                        = ( binIn.ReadUInt8() != 0 );
              BASICShowControlCodesAsChars            = ( binIn.ReadUInt8() != 0 );
              BASICAutoToggleEntryMode                = ( binIn.ReadUInt8() == 0 );
              BASICStripREM                           = ( binIn.ReadUInt8() != 0 );
              BASICAutoToggleEntryModeOnPosition      = ( binIn.ReadUInt8() != 0 );
            }
            break;
          case FileChunkConstants.SETTINGS_ENVIRONMENT:
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

              ShowCompilerMessagesAfterBuild  = ( binIn.ReadUInt8() == 0 );
              ShowOutputDisplayAfterBuild     = ( binIn.ReadUInt8() == 0 );
            }
            break;
          case FileChunkConstants.SETTINGS_PERSPECTIVES:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              GR.IO.FileChunk subChunkData = new GR.IO.FileChunk();

              while ( subChunkData.ReadFromStream( binIn ) )
              {
                GR.IO.IReader subBinIn = subChunkData.MemoryReader();

                switch ( subChunkData.Type )
                {
                  case FileChunkConstants.SETTINGS_PERSPECTIVE:
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
          case FileChunkConstants.SETTINGS_PALETTE:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              PaletteType palSystem = (PaletteType)binIn.ReadInt32();
              if ( !Palettes.ContainsKey( palSystem ) )
              {
                Palettes.Add( palSystem, new List<Palette>() );
              }

              GR.IO.FileChunk chunkPalette = new GR.IO.FileChunk();
              if ( chunkPalette.ReadFromStream( binIn ) )
              {
                var pal = Palette.Read( chunkPalette.MemoryReader() );
                Palettes[palSystem].Add( pal );
              }
            }
            break;
          case FileChunkConstants.SETTINGS_HEX_VIEW:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              MemoryDisplay = (MemoryDisplayType)binIn.ReadInt32();
              MemorySource = (MemorySourceType)binIn.ReadInt32();
              MemoryDisplayCharsetMode = (TextMode)binIn.ReadUInt8();
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
          case FileChunkConstants.SETTINGS_MRU_PROJECTS:
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
          case FileChunkConstants.SETTINGS_MRU_FILES:
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
          case FileChunkConstants.SETTINGS_SOURCE_CONTROL:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              SourceControlInfo.CommitAuthor              = binIn.ReadString();
              SourceControlInfo.CommitAuthorEmail         = binIn.ReadString();
              SourceControlInfo.CreateSolutionRepository  = ( binIn.ReadInt32() == 0 );
              SourceControlInfo.CreateProjectRepository   = ( binIn.ReadInt32() == 0 );
            }
            break;
          case FileChunkConstants.SETTINGS_HELP:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              HelpZoomFactor = binIn.ReadInt32();

              HelpZoomFactor = Math.Max( 10, HelpZoomFactor );
              HelpZoomFactor = Math.Min( 1000, HelpZoomFactor );
            }
            break;
          case FileChunkConstants.SETTINGS_MEMORY_VIEW:
            {
              GR.IO.IReader binIn = chunkData.MemoryReader();

              var debugView = new DebugMemoryViewSettings();

              debugView.DebugMemoryOffset           = binIn.ReadInt32();
              debugView.DebugMemoryByteOffset       = binIn.ReadInt32();
              debugView.DebugMemoryNumBytesPerLine  = binIn.ReadInt32();

              if ( ( debugView.DebugMemoryNumBytesPerLine != 8 )
              &&   ( debugView.DebugMemoryNumBytesPerLine != 16 )
              &&   ( debugView.DebugMemoryNumBytesPerLine != 24 )
              &&   ( debugView.DebugMemoryNumBytesPerLine != 32 )
              &&   ( debugView.DebugMemoryNumBytesPerLine != 40 ) )
              {
                debugView.DebugMemoryNumBytesPerLine = 8;
                debugView.DebugMemoryByteOffset %= debugView.DebugMemoryNumBytesPerLine;
              }

              DebugMemoryViews.Add( debugView );
            }
            break;
          case FileChunkConstants.SETTINGS_DIALOG_APPEARANCE:
            DialogSettings.ReadFromBuffer( chunkData.MemoryReader() );
            break;
        }
      }
      return true;
    }



    private void SetKeyBindingKey( RetroDevStudio.Types.Function Function, Keys Key, Keys AltKey = Keys.None )
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
        Accelerators.Add( Key, new AcceleratorKey(Key, AltKey, Function));
      }
    }



    public void SetDefaultKeyBinding()
    {
      SetKeyBindingKey( RetroDevStudio.Types.Function.SAVE_DOCUMENT, Keys.Control | Keys.S );
      SetKeyBindingKey( RetroDevStudio.Types.Function.BUILD_AND_RUN, Keys.Control | Keys.F5 );
      SetKeyBindingKey( RetroDevStudio.Types.Function.BUILD, Keys.F7 );
      SetKeyBindingKey( RetroDevStudio.Types.Function.BUILD_AND_DEBUG, Keys.F5 );
      SetKeyBindingKey( RetroDevStudio.Types.Function.DEBUG_GO, Keys.F5 );
      SetKeyBindingKey( RetroDevStudio.Types.Function.DEBUG_STEP_OUT, Keys.F9 );
      SetKeyBindingKey( RetroDevStudio.Types.Function.CENTER_ON_CURSOR, Keys.Clear );
      SetKeyBindingKey( RetroDevStudio.Types.Function.DELETE_LINE, Keys.Control | Keys.Y );
      SetKeyBindingKey( RetroDevStudio.Types.Function.DEBUG_STEP, Keys.F11 );
      SetKeyBindingKey( RetroDevStudio.Types.Function.DEBUG_STEP_OVER, Keys.F10 );
      SetKeyBindingKey( RetroDevStudio.Types.Function.DEBUG_STOP, Keys.Shift | Keys.F5 );
      SetKeyBindingKey( RetroDevStudio.Types.Function.DEBUG_RUN_TO, Keys.Control | Keys.F10 );
      SetKeyBindingKey( RetroDevStudio.Types.Function.GO_TO_DECLARATION, Keys.Control | Keys.G );
      SetKeyBindingKey( RetroDevStudio.Types.Function.COMPILE, Keys.Control | Keys.B );
      SetKeyBindingKey( RetroDevStudio.Types.Function.FIND, Keys.Control | Keys.F );
      SetKeyBindingKey( RetroDevStudio.Types.Function.FIND_NEXT, Keys.Control | Keys.L );
      SetKeyBindingKey( RetroDevStudio.Types.Function.FIND_REPLACE, Keys.Control | Keys.H );
      SetKeyBindingKey( RetroDevStudio.Types.Function.FIND_IN_PROJECT, Keys.Control | Keys.Shift | Keys.F );
      SetKeyBindingKey( RetroDevStudio.Types.Function.REPLACE_IN_PROJECT, Keys.Control | Keys.Shift | Keys.H );
      SetKeyBindingKey( RetroDevStudio.Types.Function.SAVE_ALL, Keys.Control | Keys.Shift | Keys.S );
      SetKeyBindingKey( RetroDevStudio.Types.Function.PRINT, Keys.Control | Keys.P );
      SetKeyBindingKey( RetroDevStudio.Types.Function.HELP, Keys.F1 );
      SetKeyBindingKey( RetroDevStudio.Types.Function.SAVE_DOCUMENT_AS, Keys.F12 );
      SetKeyBindingKey( RetroDevStudio.Types.Function.TOGGLE_BREAKPOINT, Keys.Shift | Keys.F9 );
      SetKeyBindingKey( RetroDevStudio.Types.Function.UNDO, Keys.Alt | Keys.Back, Keys.Control | Keys.Z);
      SetKeyBindingKey( RetroDevStudio.Types.Function.REDO, Keys.Shift | Keys.Alt | Keys.Back, Keys.Control | Keys.Shift | Keys.Z);
      SetKeyBindingKey( RetroDevStudio.Types.Function.COPY, Keys.Control | Keys.C );
      SetKeyBindingKey( RetroDevStudio.Types.Function.PASTE, Keys.Control | Keys.V );
      SetKeyBindingKey( RetroDevStudio.Types.Function.CUT, Keys.Control | Keys.X );
    }



    public void SetSyntaxColor( RetroDevStudio.Types.ColorableElement Element, uint FGColor, uint BGColor, bool AutoBGColor )
    {
      if ( SyntaxColoring[Element] == null )
      {
        SyntaxColoring[Element] = new RetroDevStudio.Types.ColorSetting( GR.EnumHelper.GetDescription( Element ), FGColor, BGColor );
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
      SetSyntaxColor( RetroDevStudio.Types.ColorableElement.NONE, 0xff800000, 0xffffffff, true );
      SetSyntaxColor( RetroDevStudio.Types.ColorableElement.CODE, 0xff000000, 0xffffffff, true );
      SetSyntaxColor( RetroDevStudio.Types.ColorableElement.LITERAL_STRING, 0xff008000, 0xffffffff, true );
      SetSyntaxColor( RetroDevStudio.Types.ColorableElement.LITERAL_NUMBER, 0xff0000ff, 0xffffffff, true );
      SetSyntaxColor( RetroDevStudio.Types.ColorableElement.LABEL, 0xff800000, 0xffffffff, true );
      SetSyntaxColor( RetroDevStudio.Types.ColorableElement.COMMENT, 0xff008000, 0xffffffff, true );
      SetSyntaxColor( RetroDevStudio.Types.ColorableElement.PSEUDO_OP, 0xffff8000, 0xffffffff, true );
      SetSyntaxColor( RetroDevStudio.Types.ColorableElement.CURRENT_DEBUG_LINE, 0xff000000, 0xffffff00, false );
      SetSyntaxColor( RetroDevStudio.Types.ColorableElement.EMPTY_SPACE, 0xff000000, 0xffffffff, false );
      SetSyntaxColor( RetroDevStudio.Types.ColorableElement.OPERATOR, 0xff000080, 0xffffffff, true );
      SetSyntaxColor( RetroDevStudio.Types.ColorableElement.ERROR_UNDERLINE, 0xffff0000, 0xffffffff, false );
      SetSyntaxColor( RetroDevStudio.Types.ColorableElement.HIGHLIGHTED_SEARCH_RESULTS, 0xff000080, 0xffff8000, true );
      SetSyntaxColor( RetroDevStudio.Types.ColorableElement.SELECTED_TEXT, 0xff800000, 0xff800000, true );
      SetSyntaxColor( RetroDevStudio.Types.ColorableElement.BACKGROUND_CONTROL, 0xff000000, 0xfff0f0f0, true );
      SetSyntaxColor( RetroDevStudio.Types.ColorableElement.CHANGED_DEBUG_ELEMENT, 0xffff0000, 0xfff0f0f0, true );
      SetSyntaxColor( RetroDevStudio.Types.ColorableElement.SELECTION_FRAME, 0xff80ff80, 0xff000000, true );
      SetSyntaxColor( RetroDevStudio.Types.ColorableElement.BACKGROUND_BUTTON, 0xff000000, 0xffE1E1E1, true );

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
      if ( ( SyntaxColoring[Element].BGColorAuto )
      &&   ( Element != ColorableElement.EMPTY_SPACE ) )
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

      foreach ( Types.ColorableElement element in System.Enum.GetValues( typeof( Types.ColorableElement ) ) )
      {
        if ( !SyntaxColoring.ContainsKey( element ) )
        {
          if ( element == ColorableElement.MINI_MAP )
          {
            // was FG red per default
            SyntaxColoring.Add( element, new ColorSetting( GR.EnumHelper.GetDescription( element ), 0xffff0000, 0xffffffff ) );
          }
          else
          {
            SyntaxColoring.Add( element, new ColorSetting( GR.EnumHelper.GetDescription( element ), 0xff000000, 0xffffffff ) );
          }
        }
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

      AllowTabs = true;
      /*
      if ( ( AllowTabs )
      &&   ( TabConvertToSpaces ) )
      {
        // both are weird
        TabConvertToSpaces = true;
        AllowTabs = false;
      }*/
      if ( TabSize < 1 )
      {
        TabSize = 1;
      }
      // randomly big value, no reason
      if ( TabSize > 800 )
      {
        TabSize = 800;
      }
      if ( ( CaretWidth < 1 )
      ||   ( CaretWidth >= 200 ) )
      {
        CaretWidth = 1;
      }

      if ( SourceFontSize <= 0.0f )
      {
        SourceFontFamily = "Consolas";
        SourceFontSize = 9.0f;
      }
      if ( BASICSourceFontSize <= 0.0f )
      {
        BASICSourceFontFamily = "Consolas";
        BASICSourceFontSize = 9.0f;
        BASICUseNonC64Font = true;
      }


      // key bindings
      ValidateOrSetKeyBindingKey( RetroDevStudio.Types.Function.SAVE_DOCUMENT, Keys.Control | Keys.S );
      ValidateOrSetKeyBindingKey( RetroDevStudio.Types.Function.BUILD_AND_RUN, Keys.Control | Keys.F5 );
      ValidateOrSetKeyBindingKey( RetroDevStudio.Types.Function.BUILD, Keys.F7 );
      ValidateOrSetKeyBindingKey( RetroDevStudio.Types.Function.BUILD_AND_DEBUG, Keys.F5 );
      ValidateOrSetKeyBindingKey( RetroDevStudio.Types.Function.DEBUG_GO, Keys.F5 );
      ValidateOrSetKeyBindingKey( RetroDevStudio.Types.Function.DEBUG_STEP_OUT, Keys.F9 );
      ValidateOrSetKeyBindingKey( RetroDevStudio.Types.Function.CENTER_ON_CURSOR, Keys.Clear );
      ValidateOrSetKeyBindingKey( RetroDevStudio.Types.Function.DELETE_LINE, Keys.Control | Keys.Y );
      ValidateOrSetKeyBindingKey( RetroDevStudio.Types.Function.DEBUG_STEP, Keys.F11 );
      ValidateOrSetKeyBindingKey( RetroDevStudio.Types.Function.DEBUG_STEP_OVER, Keys.F10 );
      ValidateOrSetKeyBindingKey( RetroDevStudio.Types.Function.DEBUG_STOP, Keys.Shift | Keys.F5 );
      ValidateOrSetKeyBindingKey( RetroDevStudio.Types.Function.DEBUG_RUN_TO, Keys.Control | Keys.F10 );
      ValidateOrSetKeyBindingKey( RetroDevStudio.Types.Function.GO_TO_DECLARATION, Keys.Control | Keys.G );
      ValidateOrSetKeyBindingKey( RetroDevStudio.Types.Function.COMPILE, Keys.Control | Keys.B );
      ValidateOrSetKeyBindingKey( RetroDevStudio.Types.Function.FIND, Keys.Control | Keys.F );
      ValidateOrSetKeyBindingKey( RetroDevStudio.Types.Function.FIND_NEXT, Keys.Control | Keys.L );
      ValidateOrSetKeyBindingKey( RetroDevStudio.Types.Function.FIND_REPLACE, Keys.Control | Keys.H );
      ValidateOrSetKeyBindingKey( RetroDevStudio.Types.Function.FIND_IN_PROJECT, Keys.Control | Keys.Shift | Keys.F );
      ValidateOrSetKeyBindingKey( RetroDevStudio.Types.Function.REPLACE_IN_PROJECT, Keys.Control | Keys.Shift | Keys.H );
      ValidateOrSetKeyBindingKey( RetroDevStudio.Types.Function.SAVE_ALL, Keys.Control | Keys.Shift | Keys.S );
      ValidateOrSetKeyBindingKey( RetroDevStudio.Types.Function.PRINT, Keys.Control | Keys.P );
      ValidateOrSetKeyBindingKey( RetroDevStudio.Types.Function.HELP, Keys.F1 );
      ValidateOrSetKeyBindingKey( RetroDevStudio.Types.Function.SAVE_DOCUMENT_AS, Keys.F12 );
      ValidateOrSetKeyBindingKey( RetroDevStudio.Types.Function.TOGGLE_BREAKPOINT, Keys.Shift | Keys.F9 );
      ValidateOrSetKeyBindingKey( RetroDevStudio.Types.Function.UNDO, Keys.Alt | Keys.Back, Keys.Control | Keys.Z);
      ValidateOrSetKeyBindingKey( RetroDevStudio.Types.Function.REDO, Keys.Shift | Keys.Alt | Keys.Back, Keys.Control | Keys.Shift | Keys.Z);
      ValidateOrSetKeyBindingKey( RetroDevStudio.Types.Function.COPY, Keys.Control | Keys.C );
      ValidateOrSetKeyBindingKey( RetroDevStudio.Types.Function.PASTE, Keys.Control | Keys.V );
      ValidateOrSetKeyBindingKey( RetroDevStudio.Types.Function.CUT, Keys.Control | Keys.X );

      // auto-add baselib path
      if ( ASMLibraryPaths.Count == 0 )
      {
        ASMLibraryPaths.Add( GR.Path.Append( Application.StartupPath, "baselib" ) );
      }

      SanitizePalettes();

      while ( ReplaceArguments.Count >= 50 )
      {
        ReplaceArguments.RemoveAt( ReplaceArguments.Count - 1 );
      }
      while ( FindArguments.Count >= 50 )
      {
        FindArguments.RemoveAt( FindArguments.Count - 1 );
      }
    }



    private void ValidateOrSetKeyBindingKey( Function Func, Keys KeyBinding, Keys AltKeyBinding = Keys.None )
    {
      foreach ( var accPair in Accelerators )
      {
        if ( accPair.Value.Function == Func )
        {
          return;
        }
      }
      SetKeyBindingKey( Func, KeyBinding, AltKeyBinding );
    }



    internal void SanitizePalettes()
    {
      foreach ( PaletteType palType in System.Enum.GetValues( typeof( PaletteType ) ) )
      {
        if ( !Core.Settings.Palettes.ContainsKey( palType ) )
        {
          Core.Settings.Palettes.Add( palType, new List<Palette>() );
        }
        if ( Core.Settings.Palettes[palType].Count == 0 )
        {
          Core.Settings.Palettes[palType].Add( PaletteManager.PaletteFromType( palType ) );
        }
      }
    }



  }
}
