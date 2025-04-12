using GR.Memory;
using RetroDevStudio;
using System;
using System.Collections.Generic;



namespace RetroDevStudio
{
  public enum DebuggerFeature
  {
    REMOTE_MONITOR,
    ADD_BREAKPOINTS_AFTER_STARTUP,
    REQUIRES_DOUBLE_ACTION_AFTER_BREAK
  };

  public enum MemorySource
  {
    AS_CPU,
    RAM
  };

  public enum DebugEvent
  {
    UNKNOWN,
    UPDATE_MEMORY,
    UPDATE_WATCH,
    EMULATOR_CLOSED,
    REGISTER_INFO,
    TRACE_OUTPUT,
    UPDATE_BREAKPOINT
  };

  public enum DebuggerState
  {
    NOT_CONNECTED,
    PAUSED,
    RUNNING
  };

  public class RegisterInfo
  {
    public byte     A = 0;
    public byte     X = 0;
    public byte     Y = 0;
    public byte     StackPointer = 0;
    public byte     StatusFlags = 0;
    public ushort   PC = 0;
    public int      RasterLine = 0;
    public int      Cycles = 0;
    public byte     ProcessorPort01 = 0;
  };

  public enum DebugRequestType
  {
    NONE = 0,
    BREAK_INFO,
    DELETE_BREAKPOINT,
    REFRESH_VALUES,
    REFRESH_MEMORY,
    REFRESH_MEMORY_RAM,
    READ_REGISTERS,
    RETURN,
    NEXT,
    STEP,
    EXIT,
    QUIT,
    MEM_DUMP,
    ADD_BREAKPOINT,
    TRACE_MEM_DUMP,
    RAM_MODE,
    RESET,
    SET_REGISTER
  };

  public enum RequestReason
  {
    UNKNOWN = 0,
    MEMORY_FETCH
  };

  public class RequestData
  {
    public DebugRequestType            Type;
    public int                Parameter1 = -1;
    public int                Parameter2 = -1;
    public string             Info = "";
    public Types.Breakpoint   Breakpoint = null;
    public bool               MemDumpOffsetX = false;
    public bool               MemDumpOffsetY = false;
    public byte               AppliedOffset = 0;    // offset resulting from ,x or ,y
    public bool               LastInGroup = true;
    public int                AdjustedStartAddress = -1;
    public RequestReason      Reason = RequestReason.UNKNOWN;



    public RequestData( DebugRequestType Type )
    {
      this.Type = Type;
    }

    public RequestData( DebugRequestType Type, int Parameter1 )
    {
      this.Type = Type;
      this.Parameter1 = Parameter1;
    }

    public RequestData( DebugRequestType Type, int Parameter1, int Parameter2 )
    {
      this.Type = Type;
      this.Parameter1 = Parameter1;
      this.Parameter2 = Parameter2;
    }
  };


  public class DebugEventData
  {
    public MemorySource       Source  = MemorySource.AS_CPU;
    public DebugEvent         Type    = DebugEvent.UPDATE_MEMORY;
    public RegisterInfo       Registers = null;
    public Types.Breakpoint   VirtualBP = null;
    public RequestData        Request = null;
    public ByteBuffer         Data = null;
    public string             Text = null;
  };

  public delegate void DebugEventHandler( DebugEventData Event );



  public interface IDebugger : IDisposable
  {
    event DebugEventHandler    DebugEvent;



    bool ShuttingDown
    {
      get;
    }

    DebuggerState State
    {
      get;
    }

    Machine ConnectedMachine
    {
      get;
    }

    bool SupportsFeature( DebuggerFeature Feature );

    bool CheckEmulatorVersion( ToolInfo ToolRun );

    bool Start( ToolInfo toolRun );
    void Quit();

    void ClearCaches();

    bool ConnectToEmulator( bool IsCartridge );
    void DisconnectFromEmulator();



    void AddLabel( string Name, int Value );
    void ClearLabels();



    void AddWatchEntry( WatchEntry Watch );
    void ClearAllWatchEntries();
    void RemoveWatchEntry( WatchEntry Watch );
    List<WatchEntry> CurrentWatches();


    // get memory content
    //   returns true if value is directly available
    //           false is value has to be fetched and will be sent per event later
    bool FetchValue( int Address, out byte Content );



    void SetBreakPoints( GR.Collections.Map<string, List<Types.Breakpoint>> BreakPoints );
    void AddBreakpoint( Types.Breakpoint BreakPoint );
    void RemoveBreakpoint( int BreakPointIndex );
    void DeleteBreakpoint( int RemoteIndex, Types.Breakpoint BP );
    void RemoveBreakpoint( int BreakPointIndex, Types.Breakpoint BP );
    void ClearAllBreakpoints();
    void Reset();



    void StepInto();
    void StepOver();
    void StepOut();
    void RefreshRegistersAndWatches();
    void RefreshMemory( int StartAddress, int Size, MemorySource Source );
    void SetAutoRefreshMemory( List<MemoryRefreshSection> Sections );
    void RefreshMemorySections();
    RequestData RefreshTraceMemory( int StartAddress, int Size, string Info, Types.Breakpoint VirtualBP, Types.Breakpoint TraceBP );

    // keep running until breakpoint hit or user pauses
    void Run();

    void Break();
    void SetShuttingDown();

    void ForceEmulatorRefresh();



    void SetRegister( string Register, int Value );
    
  }


}
