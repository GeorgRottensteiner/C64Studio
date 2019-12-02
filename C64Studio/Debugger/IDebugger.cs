using C64Studio.Types;
using GR.Memory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace C64Studio
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
    TRACE_OUTPUT
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

  public class DebugEventData
  {
    public MemorySource       Source  = MemorySource.AS_CPU;
    public DebugEvent         Type    = DebugEvent.UPDATE_MEMORY;
    public RegisterInfo       Registers = null;
    public Types.Breakpoint   VirtualBP = null;
    public C64Studio.VICERemoteDebugger.RequestData    Request = null;
    public ByteBuffer         Data = null;
    public string             Text = null;
  };

  public delegate void DebugEventHandler( DebugEventData Event ); 


  public interface IDebugger : IDisposable
  {
    event DebugEventHandler    DebugEvent;



    DebuggerState State
    {
      get;
    }

    bool SupportsFeature( DebuggerFeature Feature );

    bool CheckEmulatorVersion( ToolInfo ToolRun );


    Machine ConnectedMachine
    {
      get;
    }


    bool Start( ToolInfo toolRun );
    void Quit();

    void ClearCaches();

    bool ConnectToEmulator();
    void DisconnectFromEmulator();



    void AddLabel( string Name, int Value );
    void ClearLabels();



    void AddWatchEntry( WatchEntry Watch );
    void ClearAllWatchEntries();
    void RemoveWatchEntry( WatchEntry Watch );


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



    void StepInto();
    void StepOver();
    void StepOut();
    void RefreshRegistersAndWatches();
    void RefreshMemory( int StartAddress, int Size, MemorySource Source );
    void SetAutoRefreshMemory( int StartAddress, int Size, MemorySource Source );
    VICERemoteDebugger.RequestData RefreshTraceMemory( int StartAddress, int Size, string Info, Types.Breakpoint VirtualBP, Types.Breakpoint TraceBP );

    // keep running until breakpoint hit or user pauses
    void Run();

    void Break();


    void ForceEmulatorRefresh();
  }


}
