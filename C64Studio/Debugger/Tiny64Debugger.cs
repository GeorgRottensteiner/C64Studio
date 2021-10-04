using RetroDevStudio;
using System;
using System.Collections.Generic;



namespace C64Studio
{
  public class Tiny64Debugger : IDebugger
  {
    Tiny64.Emulator                   m_Emulator = new Tiny64.Emulator();

    Dictionary<int,LinkedList<string>> m_Labels = new Dictionary<int, LinkedList<string>>();
    private StudioCore                Core = null;
    private LinkedList<WatchEntry>    m_WatchEntries = new LinkedList<WatchEntry>();
    private bool                      m_IsCartridge = false;
    private DebuggerState             m_State = DebuggerState.NOT_CONNECTED;

    private int                       m_LastRequestedMemoryStartAddress = 0;
    private int                       m_LastRequestedMemorySize = 32;
    private MemorySource              m_LastRequestedMemorySource = MemorySource.AS_CPU;
    private RegisterInfo              CurrentRegisterValues = new RegisterInfo();

    private MachineType               m_ConnectedMachine = MachineType.UNKNOWN;



    GR.Collections.Map<int, byte>     m_MemoryValues = new GR.Collections.Map<int, byte>();
    GR.Collections.Map<int, bool>     m_RequestedMemoryValues = new GR.Collections.Map<int, bool>();

    GR.Collections.Set<Types.Breakpoint> m_BreakPoints = new GR.Collections.Set<C64Studio.Types.Breakpoint>();

    public event BaseDocument.DocumentEventHandler DocumentEvent;


    public Tiny64Debugger( StudioCore Core )
    {
      this.Core = Core;
    }



    public bool ConnectToEmulator( bool IsCartridge )
    {
      if ( State != DebuggerState.NOT_CONNECTED )
      {
        return false;
      }
      m_IsCartridge = IsCartridge;
      m_State = DebuggerState.RUNNING;

      m_Emulator.Reset();

      // to shut up the warning for now
      DebugEvent( new DebugEventData() );

      return true;
    }



    public void DisconnectFromEmulator()
    {
      m_State = DebuggerState.NOT_CONNECTED;
    }



    public void Dispose()
    {
      DisconnectFromEmulator();
    }



    private void InterfaceLog( string Text )
    {
      //Debug.Log( Text );
    }



    public void AddLabel( string Name, int Value )
    {
      if ( !m_Labels.ContainsKey( Value ) )
      {
        m_Labels.Add( Value, new LinkedList<string>() );
      }
      m_Labels[Value].AddLast( Name );
    }



    public void ClearCaches()
    {
      m_RequestedMemoryValues.Clear();
      m_MemoryValues.Clear();
      //m_WatchEntries.Clear();
    }



    public void ClearLabels()
    {
      m_Labels.Clear();
    }



    private void RaiseDocumentEvent( BaseDocument.DocEvent Event )
    {
      if ( DocumentEvent != null )
      {
        DocumentEvent( Event );
      }
    }



    public RequestData RefreshTraceMemory( int StartAddress, int Size, string Info, Types.Breakpoint VirtualBP, Types.Breakpoint TraceBP )
    {
      RequestData requData    = new RequestData( DebugRequestType.TRACE_MEM_DUMP );
      requData.Parameter1 = StartAddress;
      requData.Parameter2 = StartAddress + Size - 1;
      requData.MemDumpOffsetX = false; //watchEntry.IndexedX;
      requData.MemDumpOffsetY = false; //watchEntry.IndexedY;
      requData.Info = VirtualBP.Expression;
      requData.Breakpoint = TraceBP;

      QueueRequest( requData );

      return requData;
    }



    private void HandleBreakpoint()
    {
    }



    void RefreshMemory( int MemoryStartAddress, int MemorySize, bool AsCPU )
    {
      if ( AsCPU )
      {
        QueueRequest( DebugRequestType.REFRESH_MEMORY, MemoryStartAddress, MemorySize );
      }
      else
      {
        QueueRequest( DebugRequestType.REFRESH_MEMORY_RAM, MemoryStartAddress, MemorySize );
      }
    }




    public RequestData QueueRequest( DebugRequestType Request )
    {
      return QueueRequest( Request, -1, -1 );
    }



    public RequestData QueueRequest( DebugRequestType Request, int Param1 )
    {
      return QueueRequest( Request, Param1, -1 );
    }



    public RequestData QueueRequest( DebugRequestType Request, int Param1, int Param2 )
    {
      RequestData data = new RequestData( Request, Param1, Param2 );
      QueueRequest( data );
      return data;
    }
    


    public void QueueRequest( RequestData Data )
    {
      if ( Data.Type == DebugRequestType.REFRESH_VALUES )
      {
        QueueRequest( DebugRequestType.READ_REGISTERS );

        int gnu = 0;
        foreach ( WatchEntry watchEntry in m_WatchEntries )
        {
          if ( watchEntry.DisplayMemory )
          {
            ++gnu;
            RequestData requData = new RequestData( DebugRequestType.MEM_DUMP );
            requData.Parameter1 = watchEntry.Address;
            requData.Parameter2 = watchEntry.Address + watchEntry.SizeInBytes - 1;
            requData.MemDumpOffsetX = watchEntry.IndexedX;
            requData.MemDumpOffsetY = watchEntry.IndexedY;
            requData.Info = watchEntry.Name;

            if ( requData.Parameter2 >= 0x10000 )
            {
              requData.Parameter2 = 0xffff;
            }
            QueueRequest( requData );
          }
        }
        Debug.Log( "Request " + gnu + " watch values" );
        return;
      }
      else if ( Data.Type == DebugRequestType.REFRESH_MEMORY )
      {
        RequestData requData  = new RequestData( DebugRequestType.MEM_DUMP );
        requData.Parameter1   = Data.Parameter1;
        requData.Parameter2   = Data.Parameter1 + Data.Parameter2 - 1;
        requData.Info         = "C64Studio.MemDump";
        requData.Reason       = Data.Reason;

        if ( requData.Parameter2 >= 0x10000 )
        {
          requData.Parameter2 = 0xffff;
        }

        QueueRequest( requData );
        return;
      }
      else if ( Data.Type == DebugRequestType.REFRESH_MEMORY_RAM )
      {
        if ( MachineSupportsBankCommand() )
        {
          RequestData requRAM = new RequestData( DebugRequestType.RAM_MODE );
          requRAM.Info = "ram";
          QueueRequest( requRAM );
        }

        RequestData requData  = new RequestData( DebugRequestType.MEM_DUMP );
        requData.Parameter1 = Data.Parameter1;
        requData.Parameter2 = Data.Parameter1 + Data.Parameter2 - 1;
        requData.Info = "C64Studio.MemDumpRAM";
        requData.Reason = Data.Reason;
        if ( requData.Parameter2 >= 0x10000 )
        {
          requData.Parameter2 = 0xffff;
        }
        QueueRequest( requData );

        if ( MachineSupportsBankCommand() )
        {
          var requRAM = new RequestData( DebugRequestType.RAM_MODE );
          requRAM.Info = "cpu";
          QueueRequest( requRAM );
        }
        return;
      }
    }



    private bool MachineSupportsBankCommand()
    {
      switch ( m_ConnectedMachine )
      {
        case MachineType.VIC20:
          return false;
      }
      return true;
    }



    public void AddWatchEntry( WatchEntry Watch )
    {
      m_WatchEntries.AddLast( Watch );
    }



    public void ClearAllWatchEntries()
    {
      m_WatchEntries.Clear();
    }



    public void RemoveWatchEntry( WatchEntry Watch )
    {
      m_WatchEntries.Remove( Watch );
    }



    public bool FetchValue( int Address, out byte Content )
    {
      Content = 0;
      if ( State != DebuggerState.PAUSED )
      {
        return false;
      }
      if ( !m_RequestedMemoryValues[Address] )
      {
        m_RequestedMemoryValues[Address] = true;
        m_MemoryValues.Remove( Address );

        RequestData requData = new RequestData( DebugRequestType.MEM_DUMP );
        requData.Parameter1 = Address;
        requData.Parameter2 = Address + 1 - 1;
        requData.Info = "";
        QueueRequest( requData );
      }
      else if ( m_MemoryValues.ContainsKey( Address ) )
      {
        Content = m_MemoryValues[Address];
        return true;
      }
      return false;
    }



    public void SetBreakPoints( GR.Collections.Map<string, List<Types.Breakpoint>> BreakPoints )
    {
      m_BreakPoints.Clear();
      foreach ( var key in BreakPoints.Keys )
      {
        foreach ( Types.Breakpoint breakPoint in BreakPoints[key] )
        {
          if ( breakPoint.Address != -1 )
          {
            m_BreakPoints.Add( breakPoint );
          }
        }
      }
    }



    public void AddBreakpoint( Types.Breakpoint BreakPoint )
    {
      if ( ( !BreakPoint.Temporary )
      &&   ( m_BreakPoints.ContainsValue( BreakPoint ) ) )
      {
        return;
      }

      bool  added = false;
      foreach ( Types.Breakpoint breakPoint in m_BreakPoints )
      {
        if ( breakPoint.Address == BreakPoint.Address )
        {
          // there is already a breakpoint here
          breakPoint.Virtual.Add( BreakPoint );
          added = true;
          Debug.Log( "Virtual bp!" );
          break;
        }
      }
      if ( !added )
      {
        m_BreakPoints.Add( BreakPoint );
      }
    }



    public void RemoveBreakpoint( int BreakPointIndex )
    {
      foreach ( Types.Breakpoint breakPoint in m_BreakPoints )
      {
        if ( breakPoint.RemoteIndex == BreakPointIndex )
        {
          m_BreakPoints.Remove( breakPoint );
          break;
        }
      }
    }



    public void ClearAllBreakpoints()
    {
      foreach ( var breakPoint in m_BreakPoints )
      {
      }
      m_BreakPoints.Clear();
    }



    public void RemoveBreakpoint( int BreakPointIndex, Types.Breakpoint BP )
    {
      foreach ( Types.Breakpoint breakPoint in m_BreakPoints )
      {
        if ( breakPoint.RemoteIndex == BreakPointIndex )
        {
          breakPoint.Virtual.Remove( BP );
          if ( breakPoint.Virtual.Count == 0 )
          {
            m_BreakPoints.Remove( breakPoint );
            break;
          }
          if ( ( breakPoint.LineIndex == BP.LineIndex )
          &&   ( breakPoint.IsVirtual == BP.IsVirtual )
          &&   ( breakPoint.Expression == BP.Expression )
          &&   ( breakPoint.Conditions == BP.Conditions )
          &&   ( breakPoint.DocumentFilename == BP.DocumentFilename ) )
          {
            // the main bp is the one to be removed (copy virtual up)
            breakPoint.LineIndex = breakPoint.Virtual[0].LineIndex;
            breakPoint.DocumentFilename = breakPoint.Virtual[0].DocumentFilename;
            breakPoint.IsVirtual = breakPoint.Virtual[0].IsVirtual;
            breakPoint.Expression = breakPoint.Virtual[0].Expression;
            breakPoint.Conditions = breakPoint.Virtual[0].Conditions;
          }
        }
      }
    }



    public void StepInto()
    {
      QueueRequest( DebugRequestType.STEP );
    }



    public void StepOver()
    {
      QueueRequest( DebugRequestType.NEXT );
    }



    public void StepOut()
    {
      QueueRequest( DebugRequestType.RETURN );
    }



    public void RefreshRegistersAndWatches()
    {
      QueueRequest( DebugRequestType.REFRESH_VALUES );
    }



    public void RefreshMemory( int StartAddress, int Size )
    {
      QueueRequest( DebugRequestType.REFRESH_MEMORY, StartAddress, Size );
    }



    public void Run()
    {
      QueueRequest( DebugRequestType.EXIT );
    }



    public void Break()
    {
      StepOver();
      RefreshRegistersAndWatches();
      RefreshMemory( m_LastRequestedMemoryStartAddress, m_LastRequestedMemorySize, m_LastRequestedMemorySource );
      m_State = DebuggerState.PAUSED;
      /*
      if ( SendCommand( "break" ) )
      {
        m_State = DebuggerState.PAUSED;
      }*/
    }



    public void DeleteBreakpoint( int RemoteIndex, Types.Breakpoint bp )
    {
      RequestData requData = new RequestData( DebugRequestType.DELETE_BREAKPOINT, bp.RemoteIndex );
      requData.Breakpoint = bp;
      QueueRequest( requData );
    }



    public bool SupportsFeature( DebuggerFeature Feature )
    {
      return false;
    }



    public bool CheckEmulatorVersion( ToolInfo ToolRun )
    {
      if ( ToolRun == null )
      {
        Core.AddToOutput( "The emulator to run was not properly configured (ToolRun = null )" );
        return false;
      }

      return true;
    }



    public bool Start( ToolInfo toolRun )
    {
      if ( !CheckEmulatorVersion( toolRun ) )
      {
        return false;
      }
      throw new NotImplementedException();
    }



    public void ForceEmulatorRefresh()
    {
    }



    public void RefreshMemory( int StartAddress, int Size, MemorySource Source )
    {
      switch ( Source )
      {
        case MemorySource.AS_CPU:
          RefreshMemory( StartAddress, Size );
          break;
        case MemorySource.RAM:
          {
            RequestData data = new RequestData( DebugRequestType.REFRESH_MEMORY_RAM, StartAddress, Size );
            QueueRequest( data );
          }
          break;
      }
    }



    public void Quit()
    {
      QueueRequest( DebugRequestType.QUIT );
    }


    public event DebugEventHandler DebugEvent;


    public DebuggerState State
    {
      get
      {
        return m_State;
      }
    }



    public Machine ConnectedMachine
    {
      get
      {
        return Machine.FromType( m_ConnectedMachine );
      }
    }



    public bool ShuttingDown
    {
      get
      {
        return false;
      }
    }

    

    public void SetAutoRefreshMemory( int StartAddress, int Size, MemorySource Source )
    {
      m_LastRequestedMemoryStartAddress = StartAddress;
      m_LastRequestedMemorySize         = Size;
      m_LastRequestedMemorySource       = Source;
    }



    public void Reset()
    {
      QueueRequest( DebugRequestType.RESET );
    }



    public void SetRegister( string Register, int Value )
    {
      throw new NotImplementedException();
    }



    List<WatchEntry> IDebugger.CurrentWatches()
    {
      throw new NotImplementedException();
    }
  }
}
