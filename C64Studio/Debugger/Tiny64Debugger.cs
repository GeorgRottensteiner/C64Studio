using GR.Memory;
using RetroDevStudio.Documents;
using System;
using System.Collections.Generic;
using System.Threading;

namespace RetroDevStudio
{
  public class Tiny64Debugger : DebuggerBase, IDebugger
  {
    Tiny64.Emulator                   m_Emulator = new Tiny64.Emulator();

    Dictionary<int,LinkedList<string>> m_Labels = new Dictionary<int, LinkedList<string>>();
    private LinkedList<WatchEntry>    m_WatchEntries = new LinkedList<WatchEntry>();
    private bool                      m_IsCartridge = false;
    private volatile DebuggerState    m_State = DebuggerState.NOT_CONNECTED;

    private RegisterInfo              CurrentRegisterValues = new RegisterInfo();

    private MachineType               m_ConnectedMachine = MachineType.ANY;



    GR.Collections.Map<int, byte>     m_MemoryValues = new GR.Collections.Map<int, byte>();
    GR.Collections.Map<int, bool>     m_RequestedMemoryValues = new GR.Collections.Map<int, bool>();

    GR.Collections.Set<Types.Breakpoint> m_BreakPoints = new GR.Collections.Set<RetroDevStudio.Types.Breakpoint>();

    private bool                      m_InitialBreakpointHit = false;

    public event BaseDocument.DocumentEventHandler DocumentEvent;

    private Thread                    m_EmulatorThread;



    public Tiny64Debugger( StudioCore Core ) : base( Core )
    {
      m_Emulator.BreakpointHit += OnEmulatorBreakpointHit;

      m_EmulatorThread = new Thread( new ThreadStart( EmulatorUpdate ) );
    }



    private void EmulatorUpdate()
    {
      while ( m_State != DebuggerState.NOT_CONNECTED )
      {
        if ( ( State == DebuggerState.RUNNING )
        &&   ( m_Emulator.State == Tiny64.EmulatorState.RUNNING ) )
        {
          m_Emulator.RunCycles( 4096 );
        }
        else
        {
          Thread.Sleep( 500 );
        }
      }
    }



    private void OnEmulatorBreakpointHit()
    {
      if ( !m_InitialBreakpointHit )
      {
        m_InitialBreakpointHit = true;

        RemoveBreakpoint( 1 );
        Core.Debugging.OnInitialBreakpointReached( m_Emulator.Machine.CPU.PC );
        return;
      }
      //DebugEvent( new DebugEventData() { Type = RetroDevStudio.DebugEvent.UPDATE_BREAKPOINT );
      RefreshRegistersAndWatches();
      RefreshMemorySections();
    }



    public void RefreshMemorySections()
    {
      foreach ( var section in m_LastRefreshSections )
      {
        RefreshMemory( section.StartAddress, section.Size, section.Source );
      }
    }



    public bool ConnectToEmulator( bool IsCartridge )
    {
      if ( State != DebuggerState.NOT_CONNECTED )
      {
        return false;
      }
      m_IsCartridge = IsCartridge;

      m_Emulator.Reset();
      m_EmulatorThread.Start();
      m_State = DebuggerState.RUNNING;
      return true;
    }



    public void DisconnectFromEmulator()
    {
      m_State = DebuggerState.NOT_CONNECTED;
      m_EmulatorThread.Join();
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
        requData.Info         = "RetroDevStudio.MemDump";
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
        RefreshMemoryFromRAM( Data.Parameter1, Data.Parameter1 + Data.Parameter2 - 1 );
        /*
        if ( MachineSupportsBankCommand() )
        {
          RequestData requRAM = new RequestData( DebugRequestType.RAM_MODE );
          requRAM.Info = "ram";
          QueueRequest( requRAM );
        }

        RequestData requData  = new RequestData( DebugRequestType.MEM_DUMP );
        requData.Parameter1 = Data.Parameter1;
        requData.Parameter2 = Data.Parameter1 + Data.Parameter2 - 1;
        requData.Info = "RetroDevStudio.MemDumpRAM";
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
        }*/
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
      BreakPoint.RemoteIndex = m_Emulator.AddBreakpoint( (ushort)BreakPoint.Address, BreakPoint.TriggerOnLoad, BreakPoint.TriggerOnStore, BreakPoint.TriggerOnExec );
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
      if ( m_Emulator.State != Tiny64.EmulatorState.RUNNING )
      {
        m_Emulator.RemoveBreakpoint( BreakPointIndex );
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



    public void InjectFile( ByteBuffer InjectFile )
    {
      m_Emulator.Machine.InjectFileAfterStartup = InjectFile;
    }



    public void StepInto()
    {
      m_Emulator.StepInto();
      RefreshRegistersAndWatches();
      RefreshMemorySections();
    }



    public void StepOver()
    {
      m_State = DebuggerState.RUNNING;
      m_Emulator.StepOver();
      if ( m_Emulator.State != Tiny64.EmulatorState.RUNNING )
      {
        m_State = DebuggerState.PAUSED;
        RefreshRegistersAndWatches();
        RefreshMemorySections();
      }
    }



    public void StepOut()
    {
      m_State = DebuggerState.RUNNING;
      m_Emulator.StepOut();
      if ( m_Emulator.State != Tiny64.EmulatorState.RUNNING )
      {
        m_State = DebuggerState.PAUSED;
        RefreshRegistersAndWatches();
        RefreshMemorySections();
      }
    }



    public void RefreshRegistersAndWatches()
    {
      var ded       = new DebugEventData();
      ded.Registers = new RegisterInfo()
      {
        A = m_Emulator.Machine.CPU.Accu,
        X = m_Emulator.Machine.CPU.X,
        Y = m_Emulator.Machine.CPU.Y,
        Cycles = m_Emulator.Machine.VIC.CycleInRasterLinePos,
        PC = m_Emulator.Machine.CPU.PC,
        ProcessorPort01 = m_Emulator.Machine.Memory.RAM[1],
        RasterLine = m_Emulator.Machine.VIC.RasterPos,
        StackPointer = m_Emulator.Machine.CPU.StackPointer,
        StatusFlags = m_Emulator.Machine.CPU.Flags
      };
      ded.Type = RetroDevStudio.DebugEvent.REGISTER_INFO;

      DebugEvent( ded );
      //QueueRequest( DebugRequestType.REFRESH_VALUES );
    }



    public void RefreshMemoryFromCPU( int StartAddress, int Size )
    {
      var ded  = new DebugEventData();
      ded.Type = RetroDevStudio.DebugEvent.UPDATE_WATCH;
      ded.Data = m_Emulator.Machine.Memory.ForCPU( StartAddress, Size );
      ded.Request = new RequestData( DebugRequestType.MEM_DUMP );
      ded.Request.Parameter1 = StartAddress;
      ded.Request.Info = "RetroDevStudio.MemDump";

      DebugEvent( ded );

      //QueueRequest( DebugRequestType.REFRESH_MEMORY, StartAddress, Size );
    }



    public void RefreshMemoryFromRAM( int StartAddress, int Size )
    {
      var ded  = new DebugEventData();
      ded.Type = RetroDevStudio.DebugEvent.UPDATE_WATCH;
      ded.Request = new RequestData( DebugRequestType.MEM_DUMP );
      ded.Data = new GR.Memory.ByteBuffer( (uint)Size );
      ded.Request.Parameter1 = StartAddress;
      ded.Request.Info = "RetroDevStudio.MemDumpRAM";

      System.Array.Copy( m_Emulator.Machine.Memory.RAM, StartAddress, ded.Data.Data(), 0, Size );

      DebugEvent( ded );
    }



    public void Run()
    {
      m_Emulator.State  = Tiny64.EmulatorState.RUNNING;
      m_State           = DebuggerState.RUNNING;
    }



    public void Break()
    {
      m_Emulator.State = Tiny64.EmulatorState.PAUSED;
      RefreshRegistersAndWatches();
      RefreshMemorySections();
      m_State = DebuggerState.PAUSED;

      Debug.Log( "Break at total cycles " + m_Emulator.Machine.TotalCycles );
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
      if ( StartAddress + Size > 65536 )
      {
        Size = 65536 - StartAddress;
      }

      switch ( Source )
      {
        case MemorySource.AS_CPU:
          RefreshMemoryFromCPU( StartAddress, Size );
          break;
        case MemorySource.RAM:
          RefreshMemoryFromRAM( StartAddress, Size );
          break;
      }
    }



    public void Quit()
    {
      m_Emulator.State = Tiny64.EmulatorState.STOPPED;
      m_State = DebuggerState.NOT_CONNECTED;
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

    

    public void Reset()
    {
      m_Emulator.Reset();
    }



    public void SetRegister( string Register, int Value )
    {
      throw new NotImplementedException();
    }



    List<WatchEntry> IDebugger.CurrentWatches()
    {
      var watches = new List<WatchEntry>();

      return watches;
    }






  }
}
