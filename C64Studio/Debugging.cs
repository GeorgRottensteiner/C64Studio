using RetroDevStudio.Debugger;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;
using GR.Memory;
using RetroDevStudio.Types;
using RetroDevStudio.Documents;
using System.Linq;

namespace RetroDevStudio
{
  public class Debugging
  {
    [DllImport( "user32.dll" )]
    static extern public bool InvalidateRect( IntPtr hWnd, IntPtr lpRect, bool bErase );



    public IDebugger        Debugger              = null;
    public BaseDocument     MarkedDocument        = null;
    public int              MarkedDocumentLine    = -1;
    public int              CurrentCodePosition   = -1;
    public Project          DebuggedProject       = null;
    public int              OverrideDebugStart    = -1;
    public int              LateBreakpointOverrideDebugStart = -1;
    public bool             FirstActionAfterBreak = false;
    public string           TempDebuggerStartupFilename = "";
    public string           TempDebuggerBreakpointFilename = "";
    public DocumentInfo     DebuggedASMBase       = null;
    public DocumentInfo     DebugBaseDocumentRun  = null;
    public Disassembly      DebugDisassembly      = null;
    public StudioCore       Core = null;

    public MemoryView           MemoryCPU     = new MemoryView();
    public MemoryView           MemoryRAM     = new MemoryView();
    public MemoryView           ActiveMemory  = null;

    public List<DebugMemory>    MemoryViews = new List<DebugMemory>();

    public GR.Collections.Map<string, List<Types.Breakpoint>> BreakPoints = new GR.Collections.Map<string, List<RetroDevStudio.Types.Breakpoint>>();
    public List<Types.Breakpoint>                             BreakpointsToAddAfterStartup = new List<RetroDevStudio.Types.Breakpoint>();

    public CompileTargetType    DebugType = CompileTargetType.NONE;

    internal bool               InitialBreakpointIsTemporary;



    public delegate void MarkLineCallback( Project MarkProject, DocumentInfo Document, int Line );



    public Debugging( StudioCore Core )
    {
      this.Core = Core;
      ActiveMemory = MemoryCPU;
    }



    public void SetupDebugger( ToolInfo RunTool )
    {
      if ( RunTool == null )
      {
        return;
      }

      if ( RunTool.Filename.ToUpper().Contains( "XMEGA65" ) )
      {
        Debugger = new XMEGA65RemoteDebugger( Core );
      }
      else if ( RunTool.Name.ToUpper() == "TINY64 INTERNAL DEBUGGER" )
      {
        Debugger = new Tiny64Debugger( Core );
      }
      else if ( RunTool.DebugArguments.ToUpper().Contains( "-BINARYMONITOR" ) )
      {
        Debugger = new VICERemoteDebuggerBinaryInterface( Core );

        var viceDebugger = Debugger as VICERemoteDebuggerBinaryInterface;
        viceDebugger.DocumentEvent += new BaseDocument.DocumentEventHandler( Core.MainForm.Document_DocumentEvent );
      }
      else
      {
        Debugger = new VICERemoteDebugger( Core );

        var viceDebugger = Debugger as VICERemoteDebugger;
        viceDebugger.DocumentEvent += new BaseDocument.DocumentEventHandler( Core.MainForm.Document_DocumentEvent );
      }
      // default to currently visible memory view
      Debugger.SetAutoRefreshMemory( MemoryViewSections() );
      // pass on remembered wathes
      foreach ( var watch in Core.MainForm.m_DebugWatch.m_WatchEntries )
      {
        Debugger.AddWatchEntry( watch );
      }

      Debugger.DebugEvent += Core.MainForm.Debugger_DebugEvent;
    }



    private List<MemoryRefreshSection> MemoryViewSections()
    {
      var section = new List<MemoryRefreshSection>();

      foreach ( var view in MemoryViews )
      {
        section.Add( new MemoryRefreshSection()
        {
          StartAddress = view.MemoryStart,
          Size = view.MemorySize,
          Source = view.MemoryAsCPU ? MemorySource.AS_CPU : MemorySource.RAM
        } );
      }
      return section;
    }



    public void AddVirtualBreakpoints( Types.ASM.FileInfo ASMFileInfo )
    {
      if ( ASMFileInfo == null )
      {
        return;
      }
      foreach ( var virtualBP in ASMFileInfo.VirtualBreakpoints.Values )
      {
        virtualBP.IsVirtual = true;
        int globalLineIndex = -1;
        if ( !ASMFileInfo.FindGlobalLineIndex( virtualBP.LineIndex, virtualBP.DocumentFilename, out globalLineIndex ) )
        {
          Core.AddToOutputLine( "Cannot assign breakpoint for line " + virtualBP.LineIndex + ", no address found" );
          continue;
        }
        int address = ASMFileInfo.FindLineAddress( globalLineIndex );
        if ( address != -1 )
        {
          var existingBP = BreakpointAtAddress( address );

          if ( existingBP == null )
          {
            RetroDevStudio.Types.Breakpoint bp = new RetroDevStudio.Types.Breakpoint();

            bp.LineIndex = virtualBP.LineIndex;
            bp.Address = address;
            bp.TriggerOnExec = true;
            bp.IsVirtual = true;
            bp.DocumentFilename = virtualBP.DocumentFilename;
            bp.Virtual.Add( virtualBP );
            virtualBP.Address = address;
            // we just need any key (as null is not allowed)
            if ( !BreakPoints.ContainsKey( "RetroDevStudio.DebugBreakpoints" ) )
            {
              BreakPoints.Add( "RetroDevStudio.DebugBreakpoints", new List<RetroDevStudio.Types.Breakpoint>() );
            }
            BreakPoints["RetroDevStudio.DebugBreakpoints"].Add( bp );
          }
          else
          {
            // merge with existing
            existingBP.TriggerOnExec = true;
            existingBP.Virtual.Add( virtualBP );
          }
        }
        else
        {
          Core.AddToOutputLine( "Cannot assign breakpoint for line " + virtualBP.LineIndex + ", no address found" );
        }
      }
    }



    private Types.Breakpoint BreakpointAtAddress( int Address )
    {
      foreach ( var dock in BreakPoints.Keys )
      {
        foreach ( var bp in BreakPoints[dock] )
        {
          if ( bp.Address == Address )
          {
            return bp;
          }
        }
      }
      return null;
    }



    public void RemoveVirtualBreakpoints()
    {
      foreach ( var key in BreakPoints.Keys )
      {
        repeat:
        foreach ( Types.Breakpoint breakPoint in BreakPoints[key] )
        {
          if ( !breakPoint.HasNonVirtual() )
          {
            BreakPoints[key].Remove( breakPoint );
            goto repeat;
          }
        }
      }
    }



    public void ReseatBreakpoints( Project Project, Types.ASM.FileInfo ASMFileInfo )
    {
      foreach ( var key in BreakPoints.Keys )
      {
        foreach ( Types.Breakpoint breakPoint in BreakPoints[key] )
        {
          breakPoint.RemoteIndex = -1;
          breakPoint.IsVirtual = false;
          breakPoint.Virtual.Clear();
          breakPoint.Virtual.Add( breakPoint );

          // reseat breakpoints which are set to a line
          if ( ( key != "RetroDevStudio.DebugBreakpoints" )
          &&   ( breakPoint.LineIndex != -1 ) )
          {
            breakPoint.Address = -1;
            int globalLineIndex = 0;

            string  pathToFile = breakPoint.DocumentFilename;
            if ( Project != null )
            {
              pathToFile = Project.FullPath( breakPoint.DocumentFilename );
            }
            if ( ASMFileInfo.FindGlobalLineIndex( breakPoint.LineIndex, pathToFile, out globalLineIndex ) )
            {
              int address = ASMFileInfo.FindLineAddress( globalLineIndex );
              if ( breakPoint.Address != address )
              {
                breakPoint.Address = address;

                Core.MainForm.Document_DocumentEvent( new BaseDocument.DocEvent( BaseDocument.DocEvent.Type.BREAKPOINT_UPDATED, breakPoint ) );
              }
              if ( address != -1 )
              {
                //Debug.Log( "Found breakpoint at address " + address );
              }
            }
            else if ( breakPoint.AddressSource != null )
            {
              var address = ASMFileInfo.AddressFromToken( breakPoint.AddressSource );
              if ( address != -1 )
              {
                breakPoint.Address = (int)address;

                Core.MainForm.Document_DocumentEvent( new BaseDocument.DocEvent( BaseDocument.DocEvent.Type.BREAKPOINT_UPDATED, breakPoint ) );
              }
            }
          }
        }
      }
    }



    public bool OnInitialBreakpointReached( int Address )
    {
      if ( ( BreakpointsToAddAfterStartup.Count == 0 )
      &&   ( Core.Debugging.OverrideDebugStart == -1 ) )
      {
        return false;
      }
      // now add all later breakpoints
      foreach ( Types.Breakpoint bp in BreakpointsToAddAfterStartup )
      {
        Debugger.AddBreakpoint( bp );
      }
      // only auto-go on if the initial break point was not the fake first breakpoint
      if ( Address != LateBreakpointOverrideDebugStart )
      {
        // need to add new intermediate break point
        Types.Breakpoint bpTemp = new RetroDevStudio.Types.Breakpoint();

        bpTemp.Address        = LateBreakpointOverrideDebugStart;
        bpTemp.TriggerOnExec  = true;
        bpTemp.Temporary      = true;

        Debugger.AddBreakpoint( bpTemp );
      }

      if ( MarkedDocument != null )
      {
        MarkLine( MarkedDocument.DocumentInfo.Project, MarkedDocument.DocumentInfo, -1 );
        MarkedDocument = null;
      }

      // keep running for non cart
      if ( !Parser.ASMFileParser.IsCartridge( DebugType ) )
      {
        Debugger.Run();
      }

      Core.Executing.BringToForeground();

      FirstActionAfterBreak = false;
      Core.MainForm.SetGUIForDebugging( true );

      // force a reset for cartridges, so the debugger starts at the init
      // can't reset on a normal file, just hope our break point is not hit too early
      if ( Parser.ASMFileParser.IsCartridge( DebugType ) )
      {
        Debugger.Reset();
      }
      return true;
    }



    public void UnmarkLine()
    {
      if ( MarkedDocument != null )
      {
        MarkedDocument.SetLineMarked( MarkedDocumentLine, false );
        MarkedDocument = null;
      }
    }



    public void MarkLine( Project MarkProject, DocumentInfo Document, int Line )
    {
      if ( MarkedDocument != null )
      {
        if ( MarkedDocument.InvokeRequired )
        {
          MarkedDocument.Invoke( new MarkLineCallback( MarkLine ), 
                                 new object[] { MarkProject, Document, Line } );
          return;
        }
      }
      UnmarkLine();

      string  inPath = Document.FullPath.Replace( "\\", "/" );
      if ( MarkProject != null )
      {
        foreach ( ProjectElement element in MarkProject.Elements )
        {
          string myPath = GR.Path.Append( MarkProject.Settings.BasePath, element.Filename ).Replace( "\\", "/" );
          if ( String.Compare( myPath, inPath, true ) == 0 )
          {
            BaseDocument doc = MarkProject.ShowDocument( element );
            MarkedDocument = doc;
            MarkedDocumentLine = Line;
            if ( doc != null )
            {
              doc.SetLineMarked( Line, Line != -1 );
            }
            return;
          }
        }
      }
      foreach ( IDockContent dockContent in Core.MainForm.panelMain.Documents )
      {
        BaseDocument baseDoc = (BaseDocument)dockContent;
        if ( baseDoc.DocumentFilename == null )
        {
          continue;
        }

        string myPath = baseDoc.DocumentFilename.Replace( "\\", "/" );
        if ( String.Compare( myPath, inPath, true ) == 0 )
        {
          MarkedDocument = baseDoc;
          MarkedDocumentLine = Line;
          baseDoc.Select();
          baseDoc.SetLineMarked( Line, Line != -1 );
          return;
        }
      }
    }



    public void DebugSetRegister( string Register, int Value )
    {
      if ( Debugger != null )
      {
        Debugger.SetRegister( Register, Value );
      }
    }



    public void ForceEmulatorRefresh()
    {
      if ( Core.Executing.RunProcess != null )
      {
        Debugger.ForceEmulatorRefresh();
      }
    }



    public bool OnVirtualBreakpointReached( Types.Breakpoint Breakpoint )
    {
      Debug.Log( "OnVirtualBreakpointReached" );
      bool    addedRequest = false;
      RequestData prevRequest = null;
      foreach ( var virtualBP in Breakpoint.Virtual )
      {
        if ( !virtualBP.IsVirtual )
        {
          continue;
        }
        var tokenInfos = Core.Compiling.ParserASM.ParseTokenInfo( virtualBP.Expression, 0, virtualBP.Expression.Length );
        if ( Core.Compiling.ParserASM.HasError() )
        {
          Core.AddToOutput( "Failed to ParseTokenInfo" + System.Environment.NewLine );
          continue;
        }
        int   result = -1;
        if ( !Core.Compiling.ParserASM.EvaluateTokens( -1, tokenInfos, out SymbolInfo resultSymbol ) )
        {
          Core.AddToOutput( "Failed to evaluate " + virtualBP.Expression + System.Environment.NewLine );
          continue;
        }
        result = resultSymbol.ToInt32();
        if ( ( result < 0 )
        ||   ( result >= 65536 ) )
        {
          Core.AddToOutput( "Evaluated address out of range " + result + System.Environment.NewLine );
          continue;
        }

        if ( prevRequest != null )
        {
          prevRequest.LastInGroup = false;
        }

        int     traceSize = 1;
        var requData = Debugger.RefreshTraceMemory( result, traceSize, virtualBP.Expression, virtualBP, Breakpoint );

        if ( requData.Parameter2 >= 0x10000 )
        {
          requData.Parameter2 = 0xffff;
        }

        prevRequest = requData;

        addedRequest = true;
      }
      if ( !addedRequest )
      {
        // and auto-go on with debugging
        Debugger.Run();
        return false;
      }
      return true;
    }



    public string PrepareAfterStartBreakPoints()
    {
      BreakpointsToAddAfterStartup.Clear();

      string  breakPointFile = "";
      //int     remoteIndex = 2;    // 1 is the init breakpoint
      foreach ( var key in BreakPoints.Keys )
      {
        foreach ( Types.Breakpoint breakPoint in BreakPoints[key] )
        {
          //if ( key != "RetroDevStudio.DebugBreakpoints" )
          {
            bool mustBeAddedLater = false;

            if ( breakPoint.Address != -1 )
            {
              if ( breakPoint.TriggerOnLoad )
              {
                // store for later addition
                BreakpointsToAddAfterStartup.Add( breakPoint );
                mustBeAddedLater = true;
              }
              if ( breakPoint.TriggerOnStore )
              {
                if ( !BreakpointsToAddAfterStartup.Contains( breakPoint ) )
                {
                  // store for later addition
                  BreakpointsToAddAfterStartup.Add( breakPoint );
                  mustBeAddedLater = true;
                }
                //request += "store ";
              }

              if ( !mustBeAddedLater )
              {
                BreakpointsToAddAfterStartup.Add( breakPoint );
                /*
                //Debug.Log( "Found breakpoint at address " + breakPoint.Address.ToString( "x4" ) );
                breakPointFile += "break $" + breakPoint.Address.ToString( "x4" ) + "\r\n";
                breakPoint.RemoteIndex = remoteIndex;
                ++remoteIndex;

                Core.MainForm.Document_DocumentEvent( new BaseDocument.DocEvent( BaseDocument.DocEvent.Type.BREAKPOINT_UPDATED, breakPoint ) );*/
              }
            }
            else
            {
              breakPoint.Address = -1;
              breakPoint.RemoteIndex = -1;

              Core.MainForm.Document_DocumentEvent( new BaseDocument.DocEvent( BaseDocument.DocEvent.Type.BREAKPOINT_UPDATED, breakPoint ) );
            }
          }
          /*
          else
          {
            // manual breakpoint
            string request = "break ";
            bool mustBeAddedOnStartup = false;

            if ( breakPoint.TriggerOnExec )
            {
              request += "exec ";
              mustBeAddedOnStartup = true;
            }
            if ( Debugger.SupportsFeature( DebuggerFeature.ADD_BREAKPOINTS_AFTER_STARTUP ) )
            {
              if ( breakPoint.TriggerOnLoad )
              {
                // store for later addition
                BreakpointsToAddAfterStartup.Add( breakPoint );
                //request += "load ";
              }
              if ( breakPoint.TriggerOnStore )
              {
                if ( !BreakpointsToAddAfterStartup.Contains( breakPoint ) )
                {
                  // store for later addition
                  BreakpointsToAddAfterStartup.Add( breakPoint );
                }
                //request += "store ";
              }
            }
            if ( mustBeAddedOnStartup )
            {
              request += "$" + breakPoint.Address.ToString( "x4" );
              if ( !string.IsNullOrEmpty( breakPoint.Conditions ) )
              {
                request += " if " + breakPoint.Conditions;
              }
              breakPointFile += request + "\r\n";
              breakPoint.RemoteIndex = remoteIndex;
              ++remoteIndex;
            }

            Core.MainForm.Document_DocumentEvent( new BaseDocument.DocEvent( BaseDocument.DocEvent.Type.BREAKPOINT_UPDATED, breakPoint ) );
          }*/
        }
      }
      return breakPointFile;
    }



    public void DebugStep()
    {
      if ( Debugger == null )
      {
        Core.AddToOutput( "No debugger attached" );
        return;
      }

      if ( !CurrentToolSupportsDebugging() )
      {
        return;
      }
      if ( ( Core.MainForm.AppState == Types.StudioState.DEBUGGING_BROKEN )
      ||   ( Core.MainForm.AppState == Types.StudioState.DEBUGGING_RUN ) )
      {
        Core.MainForm.m_DebugMemory.InvalidateAllMemory();
        Debugger.StepInto();
        Debugger.RefreshRegistersAndWatches();
        Debugger.SetAutoRefreshMemory( MemoryViewSections() );
        Debugger.RefreshMemorySections();

        Core.Executing.BringStudioToForeground();

        if ( Core.MainForm.AppState == Types.StudioState.DEBUGGING_RUN )
        {
          FirstActionAfterBreak = true;
        }
        Core.MainForm.AppState = Types.StudioState.DEBUGGING_BROKEN;

        Core.MainForm.SetGUIForDebugging( true );
      }
    }



    internal void DebugStepInto()
    {
      if ( !CurrentToolSupportsDebugging() )
      {
        return;
      }
      if ( ( Core.Debugging.Debugger.SupportsFeature( DebuggerFeature.REQUIRES_DOUBLE_ACTION_AFTER_BREAK ) )
      &&   ( Core.Debugging.FirstActionAfterBreak ) )
      {
        Core.Debugging.FirstActionAfterBreak = false;
        DebugStep();
      }
      DebugStep();
    }



    public void StepOver()
    {
      if ( Debugger == null )
      {
        Core.AddToOutput( "No debugger attached" );
        return;
      }

      if ( !CurrentToolSupportsDebugging() )
      {
        return;
      }
      if ( ( Core.MainForm.AppState == Types.StudioState.DEBUGGING_BROKEN )
      ||   ( Core.MainForm.AppState == Types.StudioState.DEBUGGING_RUN ) )
      {
        Core.MainForm.m_DebugMemory.InvalidateAllMemory();
        Debugger.StepOver();
        // TODO - what? step over ends in running state!
        /*
        Debugger.RefreshRegistersAndWatches();
        Debugger.SetAutoRefreshMemory( Core.MainForm.m_DebugMemory.MemoryStart,
                                       Core.MainForm.m_DebugMemory.MemorySize,
                                       Core.MainForm.m_DebugMemory.MemoryAsCPU ? MemorySource.AS_CPU : MemorySource.RAM );
        Debugger.RefreshMemory( Core.MainForm.m_DebugMemory.MemoryStart,
                                Core.MainForm.m_DebugMemory.MemorySize,
                                Core.MainForm.m_DebugMemory.MemoryAsCPU ? MemorySource.AS_CPU : MemorySource.RAM );*/

        if ( Core.MainForm.AppState == Types.StudioState.DEBUGGING_RUN )
        {
          FirstActionAfterBreak = true;
        }
        /*
        Core.Executing.BringStudioToForeground();
        Core.MainForm.AppState = Types.StudioState.DEBUGGING_BROKEN;
        Core.MainForm.SetGUIForDebugging( true );*/
      }
    }



    internal void DebugStepOver()
    {
      if ( !CurrentToolSupportsDebugging() )
      {
        return;
      }
      if ( ( Core.Debugging.Debugger.SupportsFeature( DebuggerFeature.REQUIRES_DOUBLE_ACTION_AFTER_BREAK ) )
      &&   ( Core.Debugging.FirstActionAfterBreak ) )
      {
        Core.Debugging.FirstActionAfterBreak = false;
        StepOver();
      }
      StepOver();
    }



    public bool CurrentToolSupportsDebugging()
    {
      if ( ( Core.MainForm.m_CurrentActiveTool != null )
      &&   ( ( Core.MainForm.m_CurrentActiveTool.IsInternal )
      ||     ( Emulators.EmulatorInfo.SupportsDebugging( Core.MainForm.m_CurrentActiveTool.Filename ) ) ) )
      {
        return true;
      }
      return false;
    }



    public void DebugGo()
    {
      if ( !CurrentToolSupportsDebugging() )
      {
        return;
      }

      if ( Core.MainForm.AppState == Types.StudioState.DEBUGGING_BROKEN )
      {
        Core.MainForm.m_DebugMemory.InvalidateAllMemory();
        Debugger.Run();

        if ( MarkedDocument != null )
        {
          MarkLine( MarkedDocument.DocumentInfo.Project, MarkedDocument.DocumentInfo, -1 );
          MarkedDocument = null;
        }

        Core.Executing.BringToForeground();
        Core.MainForm.m_DebugRegisters.EnableRegisterOverrides( false );

        Core.MainForm.AppState = Types.StudioState.DEBUGGING_RUN;
        FirstActionAfterBreak = false;

        Core.MainForm.SetGUIForDebugging( true );
      }
    }



    internal void DebugBreak()
    {
      if ( Debugger == null )
      {
        Core.AddToOutput( "No debugger attached" );
        return;
      }

      if ( !CurrentToolSupportsDebugging() )
      {
        return;
      }

      if ( Core.MainForm.AppState == Types.StudioState.DEBUGGING_RUN )
      {
        // send any command to break into the monitor again
        try
        {
          Debugger.Break();

          Core.Executing.BringStudioToForeground();
        }
        catch ( Exception ex )
        {
          Core.AddToOutput( "Exception while debug break:" + ex.ToString() );
        }

        Core.MainForm.AppState = Types.StudioState.DEBUGGING_BROKEN;
        FirstActionAfterBreak = true;
        Core.MainForm.SetGUIForDebugging( true );
      }
    }



    private bool IsMemoryValid( int Offset )
    {
      return ( Core.Debugging.ActiveMemory.Flags[Offset] & MemoryView.RAMFlag.VALUE_KNOWN ) != 0;
    }



    internal void UpdateMemory( RequestData Request, ByteBuffer Data, bool AsRAM )
    {
      int Offset = Request.Parameter1;

      MemoryView memoryView = Core.Debugging.ActiveMemory;
      if ( AsRAM )
      {
        memoryView = Core.Debugging.MemoryRAM;
      }

      for ( int i = 0; i < Data.Length; ++i )
      {
        byte    ramByte = Data.ByteAt( i );

        if ( Request.Reason != RequestReason.MEMORY_FETCH )
        {
          if ( ramByte != memoryView.RAM.ByteAt( Offset + i ) )
          {
            // only mark as changed when we knew the orig value
            if ( ( memoryView.Flags[Offset + i] & MemoryView.RAMFlag.VALUE_KNOWN ) == 0 )
            {
              memoryView.Flags[Offset + i] |= MemoryView.RAMFlag.VALUE_CHANGED;
            }
          }
          else if ( ( memoryView.Flags[Offset + i] & MemoryView.RAMFlag.VALUE_KNOWN ) == 0 )
          {
            memoryView.Flags[Offset + i] &= ~MemoryView.RAMFlag.VALUE_CHANGED;
          }
        }
        memoryView.Flags[Offset + i] |= MemoryView.RAMFlag.VALUE_KNOWN;
        memoryView.RAM.SetU8At( Offset + i, ramByte );
      }

      foreach ( var debugView in MemoryViews )
      {
        debugView.UpdateMemory( Request, Data );
      }
    }



    internal void DebugStepOut()
    {
      if ( Debugger == null )
      {
        Core.AddToOutput( "No debugger attached" );
        return;
      }
      if ( !CurrentToolSupportsDebugging() )
      {
        return;
      }

      if ( ( Core.MainForm.AppState == Types.StudioState.DEBUGGING_BROKEN )
      ||   ( Core.MainForm.AppState == Types.StudioState.DEBUGGING_RUN ) )
      {
        Core.MainForm.m_DebugMemory.InvalidateAllMemory();
        Debugger.StepOut();
        // TODO - what? step out ends in running state!
        if ( Debugger.State == DebuggerState.PAUSED )
        {
          Debugger.RefreshRegistersAndWatches();
          Debugger.SetAutoRefreshMemory( MemoryViewSections() );
          Debugger.RefreshMemorySections();

          if ( Core.MainForm.AppState == Types.StudioState.DEBUGGING_RUN )
          {
            FirstActionAfterBreak = true;
          }
          Core.Executing.BringStudioToForeground();
          Core.MainForm.AppState = Types.StudioState.DEBUGGING_BROKEN;
          Core.MainForm.SetGUIForDebugging( true );
        }
      }
    }



    internal void RemoveAllBreakpoints()
    {
      /*
      var allBPs = BreakPoints.C
      foreach ( var bp in 
      RemoveBreakpoint

      Core.MainForm.m_DebugBreakpoints.Re*/

      foreach ( var bps in BreakPoints )
      {
        foreach ( var bp in bps.Value )
        {
          Core.MainForm.m_DebugBreakpoints.RemoveBreakpoint( bp );
        }
      }
      BreakPoints.Clear();
    }



    // C64Debugger format - doesn't support different breakpoint types?
    internal string BreakpointsToFile( Types.ASM.LabelFileFormat labelFileFormat )
    {
      StringBuilder   sb = new StringBuilder();

      switch ( labelFileFormat )
      {
        case Types.ASM.LabelFileFormat.C64DEBUGGER:
        case Types.ASM.LabelFileFormat.RETRODEBUGGER:
          foreach ( var bpFiles in Core.Debugging.BreakPoints )
          {
            foreach ( var bp in bpFiles.Value )
            {
              sb.Append( "break " );
              sb.Append( bp.Address.ToString( "X" ) );
              sb.AppendLine();
            }
          }
          break;
          /*
        case Types.ASM.LabelFileFormat.RETRODEBUGGER:
          sb.Append( "{\r\n  Version: \"1\"\r\n  Segments: [\r\n    {\r\n      Name: Default\r\n      Breakpoints: [\r\n" );
          foreach ( var bpFiles in Core.Debugging.BreakPoints )
          {
            foreach ( var bp in bpFiles.Value )
            {
              sb.AppendLine( "        {" );
              sb.AppendLine( "          Type: CpuPC" );
              sb.AppendLine( "          Items: [" );
              sb.AppendLine( "            {" );
              sb.AppendLine( "              IsActive: true" );
              sb.Append(     "              Addr: " );
              sb.AppendLine( bp.Address.ToString() );
              sb.AppendLine( "              Actions: 2" );
              sb.AppendLine( "              Data: 0" );
              sb.AppendLine( "            }" );
            }
          }
          sb.AppendLine( "          ]" );
          sb.AppendLine( "        }" );
          sb.AppendLine( "        {" );
          sb.AppendLine( "          Type: Memory" );
          sb.AppendLine( "        }" );
          sb.AppendLine( "        {" );
          sb.AppendLine( "          Type: RasterLine" );
          sb.AppendLine( "        }" );
          sb.AppendLine( "      ]" );
          sb.AppendLine( "    }" );
          sb.AppendLine( "  ]" );
          sb.AppendLine( "}" );
          break;*/
      }

      return sb.ToString();
    }



    internal void RefreshMemorySections()
    {
      if ( Debugger != null )
      {
        Debugger.SetAutoRefreshMemory( MemoryViewSections() );
        Debugger.RefreshMemorySections();
      }
    }



    internal void RemoveMemoryView( DebugMemory DebugMemoryView )
    {
      MemoryViews.Remove( DebugMemoryView );
      if ( Debugger != null )
      {
        Debugger.SetAutoRefreshMemory( MemoryViewSections() );
      }
    }



    internal void AddMemoryView( DebugMemory DebugMemoryView )
    {
      MemoryViews.Add( DebugMemoryView );
      if ( Debugger != null )
      {
        Debugger.RefreshMemory( DebugMemoryView.MemoryStart, DebugMemoryView.MemorySize, DebugMemoryView.MemoryAsCPU ? MemorySource.AS_CPU : MemorySource.RAM );
      }
    }



    internal bool IsDebuggerConnectedToActiveProject()
    {
      if ( Debugger == null )
      {
        return false;
      }
      if ( Core.MainForm.CurrentProject == null )
      {
        return true;
      }
      return Core.MainForm.CurrentProject == DebuggedProject;
    }



    public void DebugStop()
    {
      Debugger?.SetShuttingDown();
      /*
      if ( CurrentToolSupportsDebugging() )
      {

        if ( Core.Executing.RunProcess != null )
        {
          try
          {
            Core.Executing.RunProcess.Kill();
          }
          catch ( Exception )
          {
          }
          Core.Executing.RunProcess = null;
        }

        Core.MainForm.AppState = Types.StudioState.NORMAL;
        Core.SetStatus( "Ready" );

        Core.MainForm.SetGUIForDebugging( false );
        Core.MainForm.SetGUIForWaitOnExternalTool( false );
        Core.Executing.BringStudioToForeground();
        return;
      }*/

      if ( Core.Executing.RunProcess != null )
      {
        Core.Executing.RunProcess.Exited -= Core.MainForm.runProcess_Exited;
        try
        {
          Core.Executing.RunProcess.Kill();
        }
        catch ( Exception )
        {
        }
        Core.Executing.RunProcess = null;
      }

      if ( TempDebuggerStartupFilename.Length > 0 )
      {
        try
        {
          System.IO.File.Delete( TempDebuggerStartupFilename );
        }
        catch ( Exception ex )
        {
          Core.AddToOutput( "Failed to delete temporary file " + TempDebuggerStartupFilename + ", " + ex.Message + Environment.NewLine );
        }
        TempDebuggerStartupFilename = "";
      }
      if ( TempDebuggerStartupFilename.Length > 0 )
      {
        try
        {
          System.IO.File.Delete( TempDebuggerBreakpointFilename );
        }
        catch ( Exception ex )
        {
          Core.AddToOutput( "Failed to delete temporary file " + TempDebuggerBreakpointFilename + ", " + ex.Message + Environment.NewLine );
        }
        TempDebuggerBreakpointFilename = "";
      }

      if ( ( Core.MainForm.AppState == Types.StudioState.DEBUGGING_BROKEN )
      ||   ( Core.MainForm.AppState == Types.StudioState.DEBUGGING_RUN ) )
      {
        // send any command to break into the monitor again
        Debugger?.Quit();
        Debugger?.DisconnectFromEmulator();

        if ( MarkedDocument != null )
        {
          MarkLine( MarkedDocument.DocumentInfo.Project, MarkedDocument.DocumentInfo, -1 );
          MarkedDocument = null;
        }
        if ( DebugDisassembly != null )
        {
          Core.AddToOutputLine( "Closing Disassembly window" );
          DebugDisassembly.Invoke( new Action( DebugDisassembly.Close ), new object[] { } );
          DebugDisassembly = null;
        }
        CurrentCodePosition = -1;

        DebuggedProject = null;
        Debugger = null;
        FirstActionAfterBreak = false;
        Core.MainForm.m_CurrentActiveTool = null;

        RemoveVirtualBreakpoints();
      }
      Core.MainForm.AppState = Types.StudioState.NORMAL;
      Core.SetStatus( "Ready" );

      Core.MainForm.SetGUIForDebugging( false );
      Core.MainForm.SetGUIForWaitOnExternalTool( false );
      Core.Executing.BringStudioToForeground();
    }



  }
}
