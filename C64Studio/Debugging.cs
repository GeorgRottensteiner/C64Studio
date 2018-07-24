using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;


namespace C64Studio
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
    public DocumentInfo     DebuggedASMBase       = null;
    public DocumentInfo     DebugBaseDocumentRun  = null;
    public Disassembly      DebugDisassembly      = null;
    public StudioCore       Core = null;

    public GR.Collections.Map<string, List<Types.Breakpoint>> BreakPoints = new GR.Collections.Map<string, List<C64Studio.Types.Breakpoint>>();
    public List<Types.Breakpoint>                             BreakpointsToAddAfterStartup = new List<C64Studio.Types.Breakpoint>();


    public Debugging( StudioCore Core )
    {
      this.Core = Core;
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
          Core.AddToOutput( "Cannot assign breakpoint for line " + virtualBP.LineIndex + ", no address found" + System.Environment.NewLine );
          continue;
        }
        int address = ASMFileInfo.FindLineAddress( globalLineIndex );
        if ( address != -1 )
        {
          var existingBP = BreakpointAtAddress( address );

          if ( existingBP == null )
          {
            C64Studio.Types.Breakpoint bp = new C64Studio.Types.Breakpoint();

            bp.LineIndex = virtualBP.LineIndex;
            bp.Address = address;
            bp.TriggerOnExec = true;
            bp.IsVirtual = true;
            bp.DocumentFilename = virtualBP.DocumentFilename;
            bp.Virtual.Add( virtualBP );
            virtualBP.Address = address;
            // we just need any key (as null is not allowed)
            if ( !BreakPoints.ContainsKey( "C64Studio.DebugBreakpoints" ) )
            {
              BreakPoints.Add( "C64Studio.DebugBreakpoints", new List<C64Studio.Types.Breakpoint>() );
            }
            BreakPoints["C64Studio.DebugBreakpoints"].Add( bp );
            //AddBreakpoint( bp );
            Debug.Log( "Add virtual bp for $" + address.ToString( "X4" ) );
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
          Core.AddToOutput( "Cannot assign breakpoint for line " + virtualBP.LineIndex + ", no address found" + System.Environment.NewLine );
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



    public void ReseatBreakpoints( Types.ASM.FileInfo ASMFileInfo )
    {
      foreach ( var key in BreakPoints.Keys )
      {
        foreach ( Types.Breakpoint breakPoint in BreakPoints[key] )
        {
          breakPoint.RemoteIndex = -1;
          breakPoint.IsVirtual = false;
          breakPoint.Virtual.Clear();
          breakPoint.Virtual.Add( breakPoint );

          if ( key != "C64Studio.DebugBreakpoints" )
          {
            breakPoint.Address = -1;
            int globalLineIndex = 0;
            if ( ASMFileInfo.FindGlobalLineIndex( breakPoint.LineIndex, breakPoint.DocumentFilename, out globalLineIndex ) )
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
                breakPoint.Address = address;

                Core.MainForm.Document_DocumentEvent( new BaseDocument.DocEvent( BaseDocument.DocEvent.Type.BREAKPOINT_UPDATED, breakPoint ) );
              }
            }
          }
        }
      }
    }



    public bool OnInitialBreakpointReached( int Address, int BreakpointIndex )
    {
      if ( BreakpointsToAddAfterStartup.Count == 0 )
      {
        return false;
      }
      // now add all later breakpoints
      foreach ( Types.Breakpoint bp in BreakpointsToAddAfterStartup )
      {
        if ( ( bp.TriggerOnLoad )
        ||   ( bp.TriggerOnStore ) )
        {
          if ( bp.TriggerOnExec )
          {
            // this was already added, remove
            Debugger.DeleteBreakpoint( bp.RemoteIndex, bp );
            bp.RemoteIndex = -1;
          }
          /*
          VICERemoteDebugger.RequestData delData = new VICERemoteDebugger.RequestData( VICERemoteDebugger.Request.ADD_BREAKPOINT, bp.Address );
          delData.Breakpoint = bp;
          Debugger.QueueRequest( delData );
          */
          Debugger.AddBreakpoint( bp );
        }
      }
      // only auto-go on if the initial break point was not the fake first breakpoint
      if ( Address != LateBreakpointOverrideDebugStart )
      {
        // need to add new intermediate break point
        Types.Breakpoint bpTemp = new C64Studio.Types.Breakpoint();

        bpTemp.Address = LateBreakpointOverrideDebugStart;
        bpTemp.TriggerOnExec = true;
        bpTemp.Temporary = true;

        Debugger.AddBreakpoint( bpTemp );
        /*
        RemoteDebugger.RequestData addNewBP = new RemoteDebugger.RequestData( RemoteDebugger.Request.ADD_BREAKPOINT, m_LateBreakpointOverrideDebugStart );
        addNewBP.Breakpoint = bpTemp;
        Debugger.QueueRequest( addNewBP );*/
      }
      // and auto-go on with debugging
      Debugger.Run();

      if ( MarkedDocument != null )
      {
        MarkLine( MarkedDocument.DocumentInfo.Project, MarkedDocument.DocumentInfo.FullPath, -1 );
        MarkedDocument = null;
      }

      Core.Executing.BringToForeground();

      FirstActionAfterBreak = false;
      Core.MainForm.SetGUIForDebugging( true );
      return true;
    }



    public void MarkLine( Project MarkProject, string DocumentFilename, int Line )
    {
      if ( MarkedDocument != null )
      {
        if ( MarkedDocument.InvokeRequired )
        {
          MarkedDocument.Invoke( new Navigating.OpenDocumentAndGotoLineCallback( MarkLine ), new object[] { MarkProject, DocumentFilename, Line } );
          return;
        }
        MarkedDocument.SetLineMarked( MarkedDocumentLine, false );
      }
      string  inPath = DocumentFilename.Replace( "\\", "/" );
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
      VICERemoteDebugger.RequestData prevRequest = null;
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
        if ( !Core.Compiling.ParserASM.EvaluateTokens( -1, tokenInfos, out result ) )
        {
          Core.AddToOutput( "Failed to evaluate " + virtualBP.Expression + System.Environment.NewLine );
          continue;
        }
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
        var requData = Debugger.RefreshTraceMemory( result, result + traceSize - 1, virtualBP.Expression, virtualBP, Breakpoint );

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
      int     remoteIndex = 2;    // 1 is the init breakpoint
      foreach ( var key in BreakPoints.Keys )
      {
        foreach ( Types.Breakpoint breakPoint in BreakPoints[key] )
        {
          if ( key != "C64Studio.DebugBreakpoints" )
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
                //Debug.Log( "Found breakpoint at address " + breakPoint.Address.ToString( "x4" ) );
                breakPointFile += "break $" + breakPoint.Address.ToString( "x4" ) + "\r\n";
                breakPoint.RemoteIndex = remoteIndex;
                ++remoteIndex;

                Core.MainForm.Document_DocumentEvent( new BaseDocument.DocEvent( BaseDocument.DocEvent.Type.BREAKPOINT_UPDATED, breakPoint ) );
              }
            }
            else
            {
              breakPoint.Address = -1;
              breakPoint.RemoteIndex = -1;

              Core.MainForm.Document_DocumentEvent( new BaseDocument.DocEvent( BaseDocument.DocEvent.Type.BREAKPOINT_UPDATED, breakPoint ) );
            }
          }
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
          }
        }
      }
      return breakPointFile;
    }



    public void DebugStep()
    {
      if ( ( Core.MainForm.m_CurrentActiveTool != null )
      &&   ( !Core.MainForm.EmulatorSupportsDebugging( Core.MainForm.m_CurrentActiveTool ) ) )
      {
        return;
      }
      if ( ( Core.MainForm.AppState == Types.StudioState.DEBUGGING_BROKEN )
      ||   ( Core.MainForm.AppState == Types.StudioState.DEBUGGING_RUN ) )
      {
        Core.MainForm.m_DebugMemory.InvalidateAllMemory();
        Debugger.StepInto();
        Debugger.RefreshRegistersAndWatches();
        Debugger.SetAutoRefreshMemory( Core.MainForm.m_DebugMemory.MemoryStart,
                                       Core.MainForm.m_DebugMemory.MemorySize,
                                       Core.MainForm.m_DebugMemory.MemoryAsCPU ? MemorySource.AS_CPU : MemorySource.RAM );
        Debugger.RefreshMemory( Core.MainForm.m_DebugMemory.MemoryStart,
                                Core.MainForm.m_DebugMemory.MemorySize,
                                Core.MainForm.m_DebugMemory.MemoryAsCPU ? MemorySource.AS_CPU : MemorySource.RAM );

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
      if ( ( Core.MainForm.m_CurrentActiveTool != null )
      &&   ( !Core.MainForm.EmulatorSupportsDebugging( Core.MainForm.m_CurrentActiveTool ) ) )
      {
        return;
      }
      if ( ( Core.MainForm.AppState == Types.StudioState.DEBUGGING_BROKEN )
      ||   ( Core.MainForm.AppState == Types.StudioState.DEBUGGING_RUN ) )
      {
        Core.MainForm.m_DebugMemory.InvalidateAllMemory();
        Debugger.StepOver();
        Debugger.RefreshRegistersAndWatches();
        Debugger.SetAutoRefreshMemory( Core.MainForm.m_DebugMemory.MemoryStart,
                                       Core.MainForm.m_DebugMemory.MemorySize,
                                       Core.MainForm.m_DebugMemory.MemoryAsCPU ? MemorySource.AS_CPU : MemorySource.RAM );
        Debugger.RefreshMemory( Core.MainForm.m_DebugMemory.MemoryStart,
                                Core.MainForm.m_DebugMemory.MemorySize,
                                Core.MainForm.m_DebugMemory.MemoryAsCPU ? MemorySource.AS_CPU : MemorySource.RAM );

        if ( Core.MainForm.AppState == Types.StudioState.DEBUGGING_RUN )
        {
          FirstActionAfterBreak = true;
        }
        Core.Executing.BringStudioToForeground();
        Core.MainForm.AppState = Types.StudioState.DEBUGGING_BROKEN;
        Core.MainForm.SetGUIForDebugging( true );
      }
    }



    internal void DebugStepOver()
    {
      if ( ( Core.Debugging.Debugger.SupportsFeature( DebuggerFeature.REQUIRES_DOUBLE_ACTION_AFTER_BREAK ) )
      &&   ( Core.Debugging.FirstActionAfterBreak ) )
      {
        Core.Debugging.FirstActionAfterBreak = false;
        StepOver();
      }
      StepOver();
    }



    internal void DebugBreak()
    {
      if ( ( Core.MainForm.m_CurrentActiveTool != null )
      &&   ( !Core.MainForm.EmulatorSupportsDebugging( Core.MainForm.m_CurrentActiveTool ) ) )
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
        //mainDebugGo.Enabled = true;
        //mainDebugBreak.Enabled = false;
      }
    }
  }
}
