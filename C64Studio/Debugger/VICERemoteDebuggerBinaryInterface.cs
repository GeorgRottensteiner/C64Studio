using RetroDevStudio.Types;
using GR.Memory;
using System;
using System.Collections.Generic;
using RetroDevStudio.Documents;
using System.Windows.Forms;

namespace RetroDevStudio
{
  public class VICERemoteDebuggerBinaryInterface : DebuggerBase, IDebugger
  {
    private enum BinaryMonitorCommand
    {
      MON_CMD_MEMORY_GET                = 0x01,
      MON_CMD_MEMORY_SET                = 0x02,
      MON_CMD_CHECKPOINT_GET            = 0x11,
      MON_CMD_CHECKPOINT_SET            = 0x12,
      MON_CMD_CHECKPOINT_DELETE         = 0x13,
      MON_CMD_CHECKPOINT_LIST           = 0x14,
      MON_CMD_CHECKPOINT_TOGGLE         = 0x15,
      MON_CMD_CONDITION_SET             = 0x22,
      MON_CMD_REGISTERS_GET             = 0x31,
      MON_CMD_REGISTERS_SET             = 0x32,
      MON_CMD_ADVANCE_INSTRUCTION       = 0x71,
      MON_CMD_KEYBOARD_FEED             = 0x72,
      MON_CMD_STEP_OUT                  = 0x73,
      MON_CMD_PING                      = 0x81,
      MON_CMD_BANKS_AVAILABLE           = 0x82,
      MON_CMD_REGISTERS_AVAILABLE       = 0x83,
      MON_CMD_EXIT                      = 0xaa,
      MON_CMD_QUIT                      = 0xbb,
      MON_CMD_RESET                     = 0xcc,
      MON_CMD_AUTOSTART                 = 0xdd
      
    };

    private enum BinaryMonitorCommandResponse
    {
      MON_RESPONSE_MEM_GET              = 0x01,
      MON_RESPONSE_CHECKPOINT_INFO      = 0x11,
      MON_RESPONSE_REGISTER_INFO        = 0x31,
      MON_RESPONSE_JAM                  = 0x61,
      MON_RESPONSE_STOPPED              = 0x62,
      MON_RESPONSE_RESUMED              = 0x63,

      // mirrored request types
      MON_RESPONSE_CHECKPOINT_DELETE    = 0x13,
      MON_RESPONSE_ADVANCE_INSTRUCTION  = 0x71,
      MON_RESPONSE_STEP_OUT             = 0x73,
      MON_RESPONSE_EXIT                 = 0xaa,
      MON_RESPONSE_QUIT                 = 0xbb,
      MON_RESPONSE_RESET                = 0xcc,
      MON_RESPONSE_AUTOSTART            = 0xdd

    };

    // these are for C64, don't know about others!
    private enum BinaryMonitorBankID
    {
      CPU = 0,
      RAM = 1,
      ROM = 2,
      IO  = 3,
      CART = 4
    };

    System.Net.Sockets.Socket         client = null;

    private byte[]                    data = new byte[1024];
    private byte[]                    m_DataToSend;
    private int                       size = 1024;
    private bool                      connectResultReceived = false;
    Dictionary<int,List<string>>      m_Labels = new Dictionary<int, List<string>>();
    private GR.Memory.ByteBuffer      m_ReceivedDataBin = new GR.Memory.ByteBuffer();
    private RequestData               m_Request = new RequestData( DebugRequestType.NONE );
    private List<string>              m_ResponseLines = new List<string>();
    private List<RequestData>         m_RequestQueue = new List<RequestData>();
    private List<WatchEntry>          m_WatchEntries = new List<WatchEntry>();
    private int                       m_BytesToSend = 0;
    private int                       m_BrokenAtBreakPoint = -1;
    private bool                      m_InitialBreakpointRemoved = false;
    private bool                      m_IsCartridge = false;
    private DebuggerState             m_State = DebuggerState.NOT_CONNECTED;
    private BinaryMonitorBankID       m_FullBinaryInterfaceBank = BinaryMonitorBankID.CPU;

    private RegisterInfo              CurrentRegisterValues = new RegisterInfo();

    private MachineType               m_ConnectedMachine = MachineType.ANY;

    private bool                      m_HandlingInitialBreakpoint = false;
    private bool                      m_ShuttingDown = false;

    private int                       m_LastRequestID = 0;

    private Dictionary<uint,RequestData>   m_UnansweredBinaryRequests = new Dictionary<uint, RequestData>();



    GR.Collections.Map<int, byte>     m_MemoryValues = new GR.Collections.Map<int, byte>();
    GR.Collections.Map<int, bool>     m_RequestedMemoryValues = new GR.Collections.Map<int, bool>();

    GR.Collections.Set<Types.Breakpoint> m_BreakPoints = new GR.Collections.Set<RetroDevStudio.Types.Breakpoint>();

    public event BaseDocument.DocumentEventHandler DocumentEvent;


    public VICERemoteDebuggerBinaryInterface( StudioCore Core ) : base( Core )
    {
    }



    private void Log( string Message )
    {
      //Core.AddToOutput( Message + Environment.NewLine );
      System.Diagnostics.Debug.WriteLine( Message );
      Debug.Log( Message );
    }



    public bool ConnectToEmulator( bool IsCartridge )
    {
      /*
      Register
      PC program count
      AC a
      XR x
      YR y
      SP stack pointer
      00?
      01?
      NV-BDIZC (Status)
      LIN
      CYC
       */

      if ( State != DebuggerState.NOT_CONNECTED )
      {
        return false;
      }
      m_IsCartridge = IsCartridge;
      try
      {
        m_InitialBreakpointRemoved = false;
        connectResultReceived = false;
        m_ReceivedDataBin.Clear();
        m_ResponseLines.Clear();
        m_RequestQueue.Clear();
        m_UnansweredBinaryRequests.Clear();
        m_LastRequestID = 0;
        m_FullBinaryInterfaceBank = BinaryMonitorBankID.CPU;
        if ( client != null )
        {
          client.Disconnect( false );
          client.Close();
        }
        Core.AddToOutput( "Connecting to VICE binary interface..." + Environment.NewLine );
        client = new System.Net.Sockets.Socket( System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp );
        client.BeginConnect( "127.0.0.1", 6510, new AsyncCallback( Connected ), client );
      }
      catch ( System.Net.Sockets.SocketException se )
      {
        Core.AddToOutput( "RemoteDebugger.Connect Exception:" + se.ToString() + Environment.NewLine );
        return false;
      }
      while ( !connectResultReceived )
      {
        System.Threading.Thread.Sleep( 50 );
        Application.DoEvents();
      }
      if ( client.Connected )
      {
        m_State = DebuggerState.RUNNING;

        // connected, force reset plus add break points now
        if ( m_IsCartridge )
        {
          Log( "Connected - force break" );
          QueueRequest( DebugRequestType.STEP );
        }
      }
      return client.Connected;
    }



    void Connected( IAsyncResult iar )
    {
      client = (System.Net.Sockets.Socket)iar.AsyncState;
      try
      {
        client.EndConnect( iar );
        Core.AddToOutput( "Connected to VICE binary interface successfully" + Environment.NewLine );
        client.BeginReceive( data, 0, size, System.Net.Sockets.SocketFlags.None, new AsyncCallback( ReceiveData ), client );
      }
      catch ( System.Net.Sockets.SocketException se )
      {
        Core.AddToOutput( "Connecting to VICE binary interface failed: " + se.ToString() );
      }
      connectResultReceived = true;
    }



    void OnDataReceived( byte[] data, int Offset, int Length )
    {
      m_ReceivedDataBin.Append( data, Offset, Length );

      Log( "<<<<<<< OnDataReceived " + Length + " bytes received" );
      Log( "<<<<<<< OnDataReceived " + m_ReceivedDataBin.ToString( (int)m_ReceivedDataBin.Length - Length, Length ) );

      do
      {
        // have binary data?
        if ( !HandleBinaryInterface() )
        {
          return;
        }
      }
      while ( m_ReceivedDataBin.Length > 0 );
    }



    private bool HandleBinaryInterface()
    {
      int     curPos = 0;
      while ( true )
      {
        if ( curPos >= m_ReceivedDataBin.Length )
        {
          return true;
        }

        // byte 0: 0x02( STX )
        // byte 1: API version ID( currently 0x01 )
        // byte 2 - 5: response body length. Does not include any header fields
        // byte 6: response type
        // This is usually the same as the command ID
        // byte 7: error code
        // 0x00 ok, everything worked
        // 0x80 command length is not correct for this command
        // 0x81 an invalid parameter occurred
        // See Section 13.4[Binary Commands], page 202 for other error codes
        // byte 8 - 11: request ID
        // This is the request ID given to initiate this response.If the value is 0xffffffff,
        // Then the response was initiated by an event, such as hitting a checkpoint.

        //Log( "Incoming Binary Data is " + m_ReceivedDataBin.ToString() );


        byte  stx = m_ReceivedDataBin.ByteAt( curPos );
        if ( stx != 0x02 )
        {
          Log( "Corrupt data received - clearing buffer" );
          m_ReceivedDataBin.Clear();

          m_Request = new RequestData( DebugRequestType.NONE );
          return false;
        }
        if ( curPos + 12 > m_ReceivedDataBin.Length )
        {
          // data is not complete yet
          return false;
        }
        byte  apiVersion = m_ReceivedDataBin.ByteAt( curPos + 1 );
        if ( ( apiVersion != 1 )
        &&   ( apiVersion != 2 ) )
        {
          Log( "Unsupported API version " + apiVersion.ToString( "X2" ) + " received - clearing buffer" );
          m_ReceivedDataBin.Clear();
          m_Request = new RequestData( DebugRequestType.NONE );
          return false;
        }

        uint bodyLength = m_ReceivedDataBin.UInt32At( curPos + 2 );
        if ( curPos + 12 + bodyLength > m_ReceivedDataBin.Length )
        {
          // data is not complete yet
          return false;
        }

        var responseType = (BinaryMonitorCommandResponse)m_ReceivedDataBin.ByteAt ( curPos + 6 );
        byte errorCode    = m_ReceivedDataBin.ByteAt ( curPos + 7 );
        if ( errorCode != 0 )
        {
          Log( "Error " + errorCode.ToString( "X2" ) + " received, skipped command" );
          if ( errorCode == 0x80 )
          {
            Log( " -Length of command was wrong" );
          }
          else if ( errorCode == 0x81 )
          {
            Log( " -Length of command was wrong" );
          }

          m_ReceivedDataBin.TruncateFront( 12 + (int)bodyLength );
          m_Request = new RequestData( DebugRequestType.NONE );
          continue;
        }
        uint requestID = m_ReceivedDataBin.UInt32At( curPos + 8 );

        // 02 01 16000000 11 00 FFFFFFFF 010000000171A871A801010400010000000000000000
        // 02 01 26000000 31 00 FFFFFFFF 0900 
        //                                 03 03 71A8
        //                                 03 00 3A00
        //                                 03 01 0500
        //                                 03 02 1400
        //                                 03 04 F800
        //                                 03 FF 0000
        //                                 03 FF D000
        //                                 03 05 2300
        //                                 03 05 2300  <- ?

        // 0201160000001100FFFFFFFF010000000171A871A8010104000100000000000000000201260000003100FFFFFFFF0900030371A803003A0003010500030214000304F80003FF000003FFFF0003052300030523000201

        Log( "============ Response is " + responseType.ToString() );
        Log( "Processing package " + m_ReceivedDataBin.ToString( 0, 12 + (int)bodyLength ) + " for request " + requestID.ToString( "X8" ) );
        

        int    packagePos = curPos + 12;
        switch ( responseType )
        {
          case BinaryMonitorCommandResponse.MON_RESPONSE_REGISTER_INFO:
            {
              var info = new RegisterInfo();

              int numItems = m_ReceivedDataBin.UInt16At( packagePos );
              packagePos += 2;
              for ( int i = 0; i < numItems; ++i )
              {
                byte      itemSize = m_ReceivedDataBin.ByteAt( packagePos );
                byte      itemID = m_ReceivedDataBin.ByteAt( packagePos + 1 );
                ushort    itemValue = m_ReceivedDataBin.UInt16At( packagePos + 2 );

                packagePos += itemSize + 1;

                Log( "Register " + itemID.ToString( "X2" ) + " = " + itemValue.ToString( "X4" ) );

                switch ( itemID )
                {
                  case 0x00:
                    info.A = (byte)itemValue;
                    break;
                  case 0x01:
                    info.X = (byte)itemValue;
                    break;
                  case 0x02:
                    info.Y = (byte)itemValue;
                    break;
                  case 0x03:
                    info.PC = itemValue;
                    break;
                  case 0x04:
                    info.StackPointer = (byte)itemValue;
                    break;
                  case 0x05:
                    info.StatusFlags = (byte)itemValue;
                    break;
                  case 0x35:
                    info.RasterLine = itemValue;
                    break;
                  case 0x36:
                    info.Cycles = itemValue;
                    break;
                  case 0x37:
                    // $00
                    break;
                  case 0x38:
                    info.ProcessorPort01 = (byte)itemValue;
                    break;
                  default:
                    Log( "Invalid Item ID " + itemID.ToString( "X2" ) + " received" );
                    //m_ReceivedDataBin.Clear();
                    //return true;
                    break;
                }

                /*
                e_A = 0x00,
                e_X = 0x01,
                e_Y = 0x02,
                e_PC = 0x03,
                e_SP = 0x04,
                e_FLAGS = 0x05,
                e_Rasterline = 0x35,
                e_Cycle = 0x36  */
              }

              if ( !m_HandlingInitialBreakpoint )
              {
                var ded       = new DebugEventData();
                ded.Registers = info;
                ded.Type      = RetroDevStudio.DebugEvent.REGISTER_INFO;

                DebugEvent( ded );
              }
              m_Request = new RequestData( DebugRequestType.NONE );
            }
            break;
          case BinaryMonitorCommandResponse.MON_RESPONSE_CHECKPOINT_INFO:
            {
              uint    checkPointNumber  = m_ReceivedDataBin.UInt32At( packagePos );
              bool    currentlyHit      = ( m_ReceivedDataBin.ByteAt( packagePos + 4 ) == 1 );
              ushort  startAddress      = m_ReceivedDataBin.UInt16At( packagePos + 5 );
              ushort  endAddress        = m_ReceivedDataBin.UInt16At( packagePos + 7 );
              bool    stopWhenHit       = ( m_ReceivedDataBin.ByteAt( packagePos + 9 ) == 1 );
              bool    enabled           = ( m_ReceivedDataBin.ByteAt( packagePos + 10 ) == 1 );
              // 0x01: load, 0x02: store, 0x04: exec
              byte    cpuOperation      = m_ReceivedDataBin.ByteAt( packagePos + 11 );
              bool    temporary         = ( m_ReceivedDataBin.ByteAt( packagePos + 12 ) == 1 );
              uint    hitCount          = m_ReceivedDataBin.UInt32At( packagePos + 13 );
              uint    ignoreCount       = m_ReceivedDataBin.UInt32At( packagePos + 17 );
              bool    hasCondition      = ( m_ReceivedDataBin.ByteAt( packagePos + 21 ) == 1 );
              bool    wasInitialBreakpoint = false;

              Core.AddToOutput( "Breakpoint at address $" + startAddress.ToString( "X2" ) + " has ID " + checkPointNumber + ", enabled " + enabled + ", temporary " + temporary + System.Environment.NewLine );

              if ( currentlyHit )
              {
                m_BrokenAtBreakPoint = (int)checkPointNumber;
                OnBreakpointHit( wasInitialBreakpoint );
              }
              RequestData   origRequest= null;
              if ( m_UnansweredBinaryRequests.ContainsKey( requestID ) )
              {
                origRequest = m_UnansweredBinaryRequests[requestID];
                if ( origRequest.Type == DebugRequestType.ADD_BREAKPOINT )
                {
                  if ( ( (int)checkPointNumber != 0 )
                  &&   ( origRequest.Breakpoint != null ) )
                  {
                    origRequest.Breakpoint.RemoteIndex = (int)checkPointNumber;

                    RaiseDocumentEvent( new BaseDocument.DocEvent( BaseDocument.DocEvent.Type.BREAKPOINT_UPDATED, origRequest.Breakpoint ) );
                  }
                  else
                  {
                    Core.AddToOutput( "-is an unknown breakpoint" + System.Environment.NewLine );
                  }
                }
                else
                {
                  Core.AddToOutput( "-is an unknown breakpoint" + System.Environment.NewLine );
                }
              }
              else if ( !wasInitialBreakpoint )
              {
                Core.AddToOutput( "-is an unknown breakpoint" + System.Environment.NewLine );
              }
            }
            break;
          case BinaryMonitorCommandResponse.MON_RESPONSE_JAM:
            break;
          case BinaryMonitorCommandResponse.MON_RESPONSE_STOPPED:
            m_State = DebuggerState.PAUSED;
            m_Request = new RequestData( DebugRequestType.NONE );
            break;
          case BinaryMonitorCommandResponse.MON_RESPONSE_RESUMED:
            m_Request = new RequestData( DebugRequestType.NONE );
            m_HandlingInitialBreakpoint = false;
            break;
          case BinaryMonitorCommandResponse.MON_RESPONSE_CHECKPOINT_DELETE:
            if ( m_UnansweredBinaryRequests.ContainsKey( requestID ) )
            {
              var origRequest = m_UnansweredBinaryRequests[requestID];
              if ( origRequest.Type == DebugRequestType.DELETE_BREAKPOINT )
              {
                // remove from list
                foreach ( Types.Breakpoint breakPoint in m_BreakPoints )
                {
                  if ( breakPoint == m_Request.Breakpoint )
                  {
                    m_BreakPoints.Remove( breakPoint );
                    break;
                  }
                }
              }
            }
            break;
          case BinaryMonitorCommandResponse.MON_RESPONSE_ADVANCE_INSTRUCTION:
          case BinaryMonitorCommandResponse.MON_RESPONSE_STEP_OUT:
          case BinaryMonitorCommandResponse.MON_RESPONSE_EXIT:
          case BinaryMonitorCommandResponse.MON_RESPONSE_QUIT:
            // no action required
            m_Request = new RequestData( DebugRequestType.NONE );
            break;
          case BinaryMonitorCommandResponse.MON_RESPONSE_MEM_GET:
            // 020122000000010002000000200008A9002090FF28D0034C59A62060A64C97A8A90320FBA3A57B48A57A48A53A48
            {
              // byte 0-1: The length of the memory segment.
              // byte 2 +: The memory at the address.
              ushort      memLength  = m_ReceivedDataBin.UInt16At( packagePos );
              ByteBuffer  memContent = m_ReceivedDataBin.SubBuffer( packagePos + 2, memLength );

              if ( m_UnansweredBinaryRequests.ContainsKey( requestID ) )
              {
                // fetch remembered mem get params
                m_Request = m_UnansweredBinaryRequests[requestID];
                m_UnansweredBinaryRequests.Remove( requestID );

                Log( "MON_RESPONSE_MEM_GET from $" + m_Request.Parameter1.ToString( "X" ) + " to $" + m_Request.Parameter2.ToString( "X" ) );
              }

              OnMemoryDumpReceived( memContent );

              m_Request = new RequestData( DebugRequestType.NONE );
            }
            break;
          default:
            Log( "Unsupported response type " + ( (int)responseType ).ToString( "X2" ) + " received" );
            break;
        }

        m_ReceivedDataBin.TruncateFront( 12 + (int)bodyLength );
        //Log( "Incoming Data left " + m_ReceivedDataBin.ToString() );

        if ( m_Request.Type == DebugRequestType.NONE )
        {
          StartNextRequestIfAvailable();
        }
        else if ( m_ReceivedDataBin.Length > 0 )
        {
          Log( "We still have data left to handle, with request type " + m_Request.Type );
        }
      }
    }



    void ReceiveData( IAsyncResult iar )
    {
      try
      {
        System.Net.Sockets.Socket remote = (System.Net.Sockets.Socket)iar.AsyncState;
        int recv = remote.EndReceive( iar );
        if ( recv == 0 )
        {
          // we were closed
          //Log( "Other side closed" );
          DebugEvent( new DebugEventData()
          {
            Type = RetroDevStudio.DebugEvent.EMULATOR_CLOSED
          }  );
          m_Request.Type = DebugRequestType.NONE;
          DisconnectFromEmulator();
          return;
        }

        OnDataReceived( data, 0, recv );
        //Log( "Set up following BeginReceive" );
        if ( client != null )
        {
          client.BeginReceive( data, 0, size, System.Net.Sockets.SocketFlags.None, new AsyncCallback( ReceiveData ), client );
        }
      }
      catch ( System.ObjectDisposedException od )
      {
        if ( m_ShuttingDown )
        {
          return;
        }
        Core.AddToOutputLine( "ReceiveData Exception:" + od.ToString() );
      }
      catch ( System.Net.Sockets.SocketException se )
      {
        if ( m_ShuttingDown )
        {
          return;
        }
        Core.AddToOutputLine( "ReceiveData Exception:" + se.ToString() );
        Core.AddToOutputLine( "Connection to VICE was closed" );
        /*
        DebugEvent( new DebugEventData()
        {
          Type = RetroDevStudio.DebugEvent.EMULATOR_CLOSED
        } );*/
        m_Request.Type = DebugRequestType.NONE;
        DisconnectFromEmulator();

        Core.AddToOutputLine( "Attempt reconnect" );
        if ( !ConnectToEmulator( m_IsCartridge ) )
        {
          Core.AddToOutputLine( "Reconnect failed, stopping debug session" );
          DebugEvent( new DebugEventData()
          {
            Type = RetroDevStudio.DebugEvent.EMULATOR_CLOSED
          } );
        }
        else
        {
          Core.AddToOutputLine( "Reconnect successful" );
        }
      }
    }



    void SendData( IAsyncResult iar )
    {
      try
      {
        System.Net.Sockets.Socket remote = (System.Net.Sockets.Socket)iar.AsyncState;
        int sent = remote.EndSend( iar );

        if ( sent != m_BytesToSend )
        {
          Log( "Not all " + m_BytesToSend + " bytes were sent! (only " + sent + ")" );
          if ( sent > 0 )
          {
            m_BytesToSend -= sent;
            client.BeginSend( m_DataToSend, sent, m_BytesToSend, System.Net.Sockets.SocketFlags.None, new AsyncCallback( SendData ), client );
          }
          return;
        }
        else
        {
          Log( "Sent " + sent + " bytes (expected " + m_BytesToSend + ")" );
        }

        Core.Debugging.ForceEmulatorRefresh();
      }
      catch ( System.Net.Sockets.SocketException se )
      {
        Core.AddToOutput( "SendData Exception:" + se.ToString() );
      }
    }



    public void DisconnectFromEmulator()
    {
      if ( client != null )
      {
        try
        {
          client.Disconnect( true );
        }
        catch ( System.Net.Sockets.SocketException )
        {
          // who cares
          //m_MainForm.AddToOutput( "Exception in Disconnect: " + se.ToString() );
        }
        m_State = DebuggerState.NOT_CONNECTED;
        client = null;
        m_ResponseLines.Clear();
        m_Request = new RequestData( DebugRequestType.NONE );
        m_UnansweredBinaryRequests.Clear();
      }
    }



    public void Dispose()
    {
      DisconnectFromEmulator();
    }



    private void InterfaceLog( string Text )
    {
      Log( Text );
    }



    public bool SendCommand( GR.Memory.ByteBuffer Command )
    {
      if ( client == null )
      {
        return false;
      }
      if ( !client.Connected )
      {
        if ( !ConnectToEmulator( Parser.ASMFileParser.IsCartridge( Core.Debugging.DebugType ) ) )
        {
          return false;
        }
      }
      m_DataToSend = Command.Data();

      try
      {
        Log( "Debugger>" + Command.ToString() + ", " + m_DataToSend.Length + " bytes" );
        m_BytesToSend = m_DataToSend.Length;
        int totalBytesSent = 0;

        while ( m_BytesToSend > 0 )
        {
          int bytesSent = client.Send( m_DataToSend, totalBytesSent, m_BytesToSend, System.Net.Sockets.SocketFlags.None );
          if ( bytesSent == 0 )
          {
            Log( "Could not send " + m_BytesToSend + " bytes" );
            break;
          }
          Log( "Sent " + bytesSent + " bytes" );
          m_BytesToSend -= bytesSent;
          totalBytesSent += bytesSent;
        }
        //client.BeginSend( m_DataToSend, 0, m_DataToSend.Length, System.Net.Sockets.SocketFlags.None, new AsyncCallback( SendData ), client );
      }
      catch ( System.IO.IOException ex )
      {
        Core.AddToOutput( "SendCommand Exception:" + ex.ToString() );
      }

      Core.Debugging.ForceEmulatorRefresh();
      return true;
    }



    public void AddLabel( string Name, int Value )
    {
      if ( !m_Labels.ContainsKey( Value ) )
      {
        m_Labels.Add( Value, new List<string>() );
      }
      m_Labels[Value].Add( Name );
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



    private void OnMemoryDumpReceived( ByteBuffer DumpData )
    {
      Debug.Log( "Got MemDump data as " + DumpData.ToString() );
      if ( m_Request.Type == DebugRequestType.TRACE_MEM_DUMP )
      {
        string    traceText = "Trace " + m_Request.Info + " from $" + m_Request.Parameter1.ToString( "X4" ) + " as $" + DumpData.ToString() + "/" + DumpData.ByteAt( 0 ) + System.Environment.NewLine;
        DebugEvent( new DebugEventData()
        {
          Type = RetroDevStudio.DebugEvent.TRACE_OUTPUT,
          Text = traceText
        } );

        if ( m_Request.LastInGroup )
        {
          // was there a "real" breakpoint along with the virtual one?
          if ( !m_Request.Breakpoint.HasNonVirtual() )
          {
            // and auto-go on with debugging
            Log( "Virtual only, go on" );
            QueueRequest( DebugRequestType.EXIT );
          }
          else
          {
            Log( "Has non virtual bp" );
            QueueRequest( DebugRequestType.REFRESH_VALUES );
            RefreshMemorySections();
          }
        }
      }
      else
      {
        for ( int i = 0; i < DumpData.Length; ++i )
        {
          m_MemoryValues[m_Request.Parameter1 + i] = DumpData.ByteAt( i );
        }
        DebugEvent( new DebugEventData()
        {
          Type = RetroDevStudio.DebugEvent.UPDATE_WATCH,
          Request = m_Request,
          Data = DumpData
        } );
      }
      m_Request = new RequestData( DebugRequestType.NONE );
    }



    private void OnBreakpointHit( bool WasInitialBreakpoint )
    {
      m_State = DebuggerState.PAUSED;
      WasInitialBreakpoint = false;

      Log( "Breakpoint " + m_BrokenAtBreakPoint + " hit" );
      // TODO - only remove if auto startup breakpoint
      int breakAddress = -1;
      Types.Breakpoint  brokenBP = null;
      foreach ( Types.Breakpoint breakPoint in m_BreakPoints )
      {
        if ( breakPoint.RemoteIndex == m_BrokenAtBreakPoint )
        {
          brokenBP = breakPoint;
          breakAddress = (int)breakPoint.Address;
          if ( breakPoint.Temporary )
          {
            Log( "Remove auto startup breakpoint " + breakPoint.RemoteIndex );
            QueueRequest( DebugRequestType.DELETE_BREAKPOINT, m_BrokenAtBreakPoint ).Breakpoint = breakPoint;
            brokenBP = null;
            WasInitialBreakpoint = true;
          }
        }
      }

      bool skipRefresh = false;

      if ( ( !m_IsCartridge )
      &&   ( m_BrokenAtBreakPoint == 1 ) )
      {
        if ( ( Core.Debugging.InitialBreakpointIsTemporary )
        &&   ( !m_InitialBreakpointRemoved ) )
        {
          // auto break point
          m_InitialBreakpointRemoved = true;
          m_HandlingInitialBreakpoint = true;

          // DEBUGHACK
          //QueueRequest( Request.MEM_DUMP, 0, 0xffff );
          Log( "Remove initial breakpoint " + m_BrokenAtBreakPoint );
          QueueRequest( DebugRequestType.DELETE_BREAKPOINT, m_BrokenAtBreakPoint );

          skipRefresh = Core.Debugging.OnInitialBreakpointReached( breakAddress );
          WasInitialBreakpoint = true;
        }
      }
      else
      {
        if ( ( brokenBP != null )
        &&   ( brokenBP.HasVirtual() ) )
        {
          // a trace breakpoint, only fetch trace info and continue
          skipRefresh = Core.Debugging.OnVirtualBreakpointReached( brokenBP );
          // we added a trace mem dump request
        }
      }

      if ( ( !skipRefresh )
      &&   ( !WasInitialBreakpoint ) )
      {
        QueueRequest( DebugRequestType.REFRESH_VALUES );
        RefreshMemorySections();
      }
      m_Request = new RequestData( DebugRequestType.NONE );
    }



    private DebugEventData DebugEventDataFromRegisterValueString( string[] registerValues )
    {
      DebugEventData  ded = new DebugEventData()
      {
        Type = RetroDevStudio.DebugEvent.REGISTER_INFO
      };

      ded.Registers = new RegisterInfo();

      ded.Registers.A = GR.Convert.ToU8( registerValues[1], 16 );
      ded.Registers.X = GR.Convert.ToU8( registerValues[2], 16 );
      ded.Registers.Y = GR.Convert.ToU8( registerValues[3], 16 );
      ded.Registers.StackPointer = GR.Convert.ToU8( registerValues[4], 16 );
      ded.Registers.StatusFlags = GR.Convert.ToU8( registerValues[7], 2 );
      ded.Registers.PC = GR.Convert.ToU16( registerValues[0].Substring( 2 ), 16 );
      ded.Registers.RasterLine = GR.Convert.ToU16( registerValues[8] );
      ded.Registers.Cycles = GR.Convert.ToI32( registerValues[9] );
      ded.Registers.ProcessorPort01 = GR.Convert.ToU8( registerValues[6], 16 );

      CurrentRegisterValues = ded.Registers;

      return ded;
    }



    private byte VICEFlagsToByte( string Flags )
    {
      byte    result = 0;


      // .V-..IZC
      result |= (byte)( ( Flags[0] == 'N' ) ? 0x80 : 0 );
      result |= (byte)( ( Flags[1] == 'V' ) ? 0x40 : 0 );
      //result |= (byte)0x20;
      result |= (byte)( ( Flags[3] == 'B' ) ? 0x10 : 0 );
      result |= (byte)( ( Flags[4] == 'D' ) ? 0x08 : 0 );
      result |= (byte)( ( Flags[5] == 'I' ) ? 0x04 : 0 );
      result |= (byte)( ( Flags[6] == 'Z' ) ? 0x02 : 0 );
      result |= (byte)( ( Flags[7] == 'C' ) ? 0x01 : 0 );

      return result;
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



    private void StartNextRequestIfAvailable()
    {
      RequestData request = null;

      while ( m_ReceivedDataBin.Length == 0 )
      {
        request = null;
        lock ( m_RequestQueue )
        {
          if ( m_RequestQueue.Count != 0 )
          {
            request = m_RequestQueue[0];
            m_RequestQueue.RemoveAt( 0 );

            Log( "------> StartNextRequest:" + request.Type );
            Log( $"   {m_RequestQueue.Count} requests left in queue" );
          }
        }
        if ( request == null )
        {
          break;
        }
        SendRequest( request );
      }
    }



    private bool SendRequest( RequestData Data )
    {
      if ( m_Request.Type != DebugRequestType.NONE )
      {
        Log( "====> Trying to send request while processing another! (" + m_Request.Type + ")" );
        return false;
      }
      //Log( "Set request data to " + Data.Type.ToString() );
      m_Request = Data;
      m_ResponseLines.Clear();

      switch ( m_Request.Type )
      {
        case DebugRequestType.SET_REGISTER:
          {
            // main memory plus count 1
            ByteBuffer    requestData = new ByteBuffer( "000100" );

            switch ( m_Request.Parameter1 )
            {
              case 'A':
                requestData.AppendU16NetworkOrder( 0x0300 );
                requestData.AppendU16( (byte)m_Request.Parameter2 );
                break;
              case 'X':
                requestData.AppendU16NetworkOrder( 0x0301 );
                requestData.AppendU16( (byte)m_Request.Parameter2 );
                break;
              case 'Y':
                requestData.AppendU16NetworkOrder( 0x0302 );
                requestData.AppendU16( (byte)m_Request.Parameter2 );
                break;
              case 'F':
                requestData.AppendU16NetworkOrder( 0x0305 );
                requestData.AppendU16( (byte)m_Request.Parameter2 );
                break;
              case 'P':
                requestData.AppendU16NetworkOrder( 0x0303 );
                requestData.AppendU16( (ushort)m_Request.Parameter2 );
                break;
              case 'S':
                requestData.AppendU16NetworkOrder( 0x0304 );
                requestData.AppendU16( (ushort)m_Request.Parameter2 );
                break;
            }
            /*
            byte 0: memspace
            Describes which part of the computer you want to write:
            • 0x00: main memory
            • 0x01: drive 8
            • 0x02: drive 9
            • 0x03: drive 10
            • 0x04: drive 11
            byte 1 - 2: The count of the array items
            byte 2 +: An array with items of structure:
            byte 0: Size of the item, excluding this byte byte 1: ID of the register byte 2 - 3:
            register value*/
            return SendBinaryCommand( BinaryMonitorCommand.MON_CMD_REGISTERS_SET, requestData, Data );
          }
        case DebugRequestType.READ_REGISTERS:
          // step over

          /*
          e_default_space = 0,
          e_comp_space,
          e_disk8_space,
          e_disk9_space,
          e_disk10_space,
          e_disk11_space,
          e_invalid_space
          */
          return SendBinaryCommand( BinaryMonitorCommand.MON_CMD_REGISTERS_GET, new ByteBuffer( "00" ), Data );
        case DebugRequestType.NEXT:
          m_MemoryValues.Clear();
          m_RequestedMemoryValues.Clear();
          m_State = DebuggerState.RUNNING;
          // step over
          return SendBinaryCommand( BinaryMonitorCommand.MON_CMD_ADVANCE_INSTRUCTION, new ByteBuffer( "010100" ), Data );
        case DebugRequestType.STEP:
          m_MemoryValues.Clear();
          m_RequestedMemoryValues.Clear();
          m_State = DebuggerState.RUNNING;
          // step into
          return SendBinaryCommand( BinaryMonitorCommand.MON_CMD_ADVANCE_INSTRUCTION, new ByteBuffer( "000100" ), Data );
        case DebugRequestType.EXIT:
          m_Request.Type = DebugRequestType.NONE;
          m_State = DebuggerState.RUNNING;
          m_RequestedMemoryValues.Clear();
          return SendBinaryCommand( BinaryMonitorCommand.MON_CMD_EXIT, null, Data );
        case DebugRequestType.QUIT:
          return SendBinaryCommand( BinaryMonitorCommand.MON_CMD_QUIT, null, Data );
        case DebugRequestType.RETURN:
          m_MemoryValues.Clear();
          m_RequestedMemoryValues.Clear();
          m_State = DebuggerState.RUNNING;

          return SendBinaryCommand( BinaryMonitorCommand.MON_CMD_STEP_OUT, new ByteBuffer( "000100" ), Data );
        case DebugRequestType.RESET:
          // hard reset
          return SendBinaryCommand( BinaryMonitorCommand.MON_CMD_RESET, new ByteBuffer( "01" ), Data );
        case DebugRequestType.ADD_BREAKPOINT:
          {
            // byte 0 - 1: start address
            // byte 2 - 3: end address
            // byte 4: stop when hit
            //  0x01: true, 0x00: false
            // byte 5: enabled
            //  0x01: true, 0x00: false
            // byte 6: CPU operation
            //  0x01: load, 0x02: store, 0x04: exec
            // byte 7: temporary

            var requestData = new ByteBuffer();
            requestData.AppendU16( (ushort)m_Request.Parameter1 );
            requestData.AppendU16( (ushort)m_Request.Parameter1 );
            requestData.AppendU8( 1 );    // stop when hit
            requestData.AppendU8( 1 );    // enabled

            byte    cpuOperation = 0;
            if ( m_Request.Breakpoint.TriggerOnExec )
            {
              cpuOperation |= 0x04;
            }
            if ( m_Request.Breakpoint.TriggerOnLoad )
            {
              cpuOperation |= 0x01;
            }
            if ( m_Request.Breakpoint.TriggerOnStore )
            {
              cpuOperation |= 0x02;
            }
            requestData.AppendU8( cpuOperation );
            requestData.AppendU8( 0 );    // temporary

            return SendBinaryCommand( BinaryMonitorCommand.MON_CMD_CHECKPOINT_SET, requestData, Data );
          }
        case DebugRequestType.DELETE_BREAKPOINT:
          {
            var requestBody = new GR.Memory.ByteBuffer();
            requestBody.AppendU32( (uint)m_Request.Parameter1 );

            return SendBinaryCommand( BinaryMonitorCommand.MON_CMD_CHECKPOINT_DELETE, requestBody, Data );
          }
        case DebugRequestType.RAM_MODE:
          switch ( m_Request.Info )
          {
            case "ram":
              m_FullBinaryInterfaceBank = BinaryMonitorBankID.RAM;
              break;
            case "cpu":
              m_FullBinaryInterfaceBank = BinaryMonitorBankID.CPU;
              break;
            default:
              Log( "Invalid Bank '" + m_Request.Info + "' requested!" );
              return false;
          }
          m_Request.Type = DebugRequestType.NONE;
          return true;
        case DebugRequestType.MEM_DUMP:
        case DebugRequestType.TRACE_MEM_DUMP:
          {
            int offset = 0;
            if ( m_Request.MemDumpOffsetX )
            {
              offset += CurrentRegisterValues.X;
            }
            if ( m_Request.MemDumpOffsetY )
            {
              offset += CurrentRegisterValues.Y;
            }
            m_Request.AppliedOffset = (byte)offset;

            // byte 0: side effects?
            // Should the read cause side effects?
            // byte 1 - 2: start address
            // byte 3 - 4: end address
            // byte 5: memspace
            // Describes which part of the computer you want to read:
            // • 0x00: main memory
            // • 0x01: drive 8
            // • 0x02: drive 9
            // • 0x03: drive 10
            // • 0x04: drive 11
            // byte 6 - 7: bank ID
            // Describes which bank you want.This is dependent on your machine.
            // See Section 13.4.14[MON CMD BANKS AVAILABLE], page 207.If the
            // memspace selected doesn’t support banks, this value is ignored.
            var requestBody = new ByteBuffer();
            requestBody.AppendU8( 0 );    // no side effects!
            requestBody.AppendU16( (ushort)( offset + m_Request.Parameter1 ) );
            if ( m_Request.Parameter2 == -1 )
            {
              requestBody.AppendU16( (ushort)( offset + m_Request.Parameter1 ) );
            }
            else
            {
              requestBody.AppendU16( (ushort)( offset + m_Request.Parameter2 ) );
            }

            requestBody.AppendU8( 0 );    // main memory

            //          "default","cpu","ram","rom","io","cart",
            //banknums[] = { 1, 0, 1, 2, 3, 4, -1 };
            requestBody.AppendU16( (ushort)m_FullBinaryInterfaceBank );    // CPU -> TODO, we also have RAM option!


            return SendBinaryCommand( BinaryMonitorCommand.MON_CMD_MEMORY_GET, requestBody, Data );
          }
      }
      return true;
    }



    private bool SendBinaryCommand( BinaryMonitorCommand Command, ByteBuffer RequestData, RequestData OriginatingRequest )
    {
      //return true;
      // UGLY HACK
      m_Request = new RetroDevStudio.RequestData( DebugRequestType.NONE );

      int   bodyLength = 0;
      if ( RequestData != null )
      {
        bodyLength = (int)RequestData.Length;
      }
      var fullRequest = new ByteBuffer( (uint)( 11 + bodyLength ) );

      ++m_LastRequestID;
      uint    requestID = (uint)m_LastRequestID;

      fullRequest.SetU8At( 0, 0x02 );     // STX
      fullRequest.SetU8At( 1, 0x01 );     // Version
      fullRequest.SetU32At( 2, (uint)bodyLength );
      fullRequest.SetU32At( 6, requestID );
      fullRequest.SetU8At( 10, (byte)Command );
      if ( RequestData != null )
      {
        RequestData.CopyTo( fullRequest, 0, bodyLength, 11 );
      }

      m_UnansweredBinaryRequests.Add( requestID, OriginatingRequest );
      /*
      if ( Command == BinaryMonitorCommand.MON_CMD_MEMORY_GET )
      {
        // byte 0: side effects?
        // Should the read cause side effects?
        // byte 1 - 2: start address
        // byte 3 - 4: end address
        m_UnansweredBinaryRequests.Add( requestID, new RequestData( DebugRequestType.MEM_DUMP ) { Parameter1 = RequestData.UInt16At( 1 ), Parameter2 = RequestData.UInt16At( 3 ), Info = "RetroDevStudio.MemDump" } );
      }*/

      InterfaceLog( ">>>>>>>>>>>>>>> Send Request " + Command.ToString() + ", request ID " + requestID );
      if ( RequestData != null )
      {
        InterfaceLog( "                     Command Body " + RequestData.ToString() );
      }

      // byte 0: 0x02( STX )
      // byte 1: API version ID( currently 0x01 )
      // byte 2 - 5: length
      //   Note that the command length does *not*count the STX, the command length,
      //  the command byte, or the request ID. Basically nothing in the header, just the
      //  body.
      // byte 6 - 9: request id
      //  In little endian order. All multibyte values are in little endian order, unless
      //  otherwise specified.There is no requirement for this to be unique, but it makes
      //  it easier to match up the responses if you do.
      // byte 10: The numeric command type
      // byte 11 +: The command body.

      return SendCommand( fullRequest );
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
      if ( State == DebuggerState.NOT_CONNECTED )
      {
        return;
      }

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
        Log( "Request " + gnu + " watch values" );
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
        }
        return;
      }

      // queue if there is an active request or queued incoming data
      Log( "QueueRequest - sending data directly? Queue has " + m_RequestQueue.Count + " entries, request is " + Data.Type + ", current request type is " + m_Request.Type + ", current incoming data is " + m_ReceivedDataBin.ToString() );
      if ( ( m_Request.Type != DebugRequestType.NONE )
      ||   ( m_RequestQueue.Count > 0 )
      ||   ( m_ReceivedDataBin.Length > 0 ) )
      {
        Log( "-no" );
        m_RequestQueue.Add( Data );
        return;
      }
      Log( "-yes" );
      SendRequest( Data );
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
      m_WatchEntries.Add( Watch );
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
          Log( "Virtual bp!" );
          break;
        }
      }
      if ( !added )
      {
        m_BreakPoints.Add( BreakPoint );
        if ( ( client != null )
        &&   ( client.Connected ) )
        {
          RequestData requData = new RequestData( DebugRequestType.ADD_BREAKPOINT, (int)BreakPoint.Address );
          requData.Breakpoint = BreakPoint;
          QueueRequest( requData );
        }
      }
    }



    public void RemoveBreakpoint( int BreakPointIndex )
    {
      foreach ( Types.Breakpoint breakPoint in m_BreakPoints )
      {
        if ( breakPoint.RemoteIndex == BreakPointIndex )
        {
          m_BreakPoints.Remove( breakPoint );

          if ( ( client != null )
          &&   ( client.Connected ) )
          {
            Log( "Queue - Remove breakpoint " + breakPoint.RemoteIndex );
            RequestData requData = new RequestData( DebugRequestType.DELETE_BREAKPOINT, breakPoint.RemoteIndex );
            QueueRequest( requData );
          }
          break;
        }
      }
    }



    public void ClearAllBreakpoints()
    {
      foreach ( var breakPoint in m_BreakPoints )
      {
        if ( ( client != null )
        &&   ( client.Connected ) )
        {
          Log( "Queue - Remove breakpoint " + breakPoint.RemoteIndex );
          RequestData requData = new RequestData( DebugRequestType.DELETE_BREAKPOINT, breakPoint.RemoteIndex );
          QueueRequest( requData );
        }
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

            if ( ( client != null )
            && ( client.Connected ) )
            {
              Log( "Queue - Remove breakpoint " + breakPoint.RemoteIndex );
              RequestData requData = new RequestData( DebugRequestType.DELETE_BREAKPOINT, breakPoint.RemoteIndex );
              QueueRequest( requData );
            }
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
      // technically not correct to set paused here (only good for binary interface?
      //m_State = DebuggerState.PAUSED;
      QueueRequest( DebugRequestType.EXIT );
    }



    public void Break()
    {
      StepOver();
      RefreshRegistersAndWatches();
      RefreshMemorySections();
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
      switch ( Feature )
      {
        case DebuggerFeature.REMOTE_MONITOR:
          return true;
      }
      return false;
    }



    public bool CheckEmulatorVersion( ToolInfo ToolRun )
    {
      if ( ToolRun == null )
      {
        Core.AddToOutput( "The emulator to run was not properly configured (ToolRun = null )" );
        return false;
      }

      System.Diagnostics.FileVersionInfo    fileVersion;

      try
      {
        fileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo( ToolRun.Filename );
      }
      catch ( System.Exception io )
      {
        Core.AddToOutput( "Could not check emulator version: " + io.Message );
        return false;
      }

      // find machine type from executable
      m_ConnectedMachine = Emulators.EmulatorInfo.DetectMachineType( ToolRun.Filename );

      // what an ugly hack check (there's no version resource anymore :( )
      //m_FullBinaryInterface = ToolRun.DebugArguments.ToUpper().Contains( "-BINARYMONITOR" );

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
      m_ShuttingDown = true;
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
        return m_ShuttingDown;
      }
    }



    public void RefreshMemorySections()
    {
      foreach ( var section in m_LastRefreshSections )
      {
        RefreshMemory( section.StartAddress, section.Size, section.Source );
      }
    }



    public void Reset()
    {
      QueueRequest( DebugRequestType.RESET );
    }



    public void SetRegister( string Register, int Value )
    {
      if ( Register.Length != 1 )
      {
        return;
      }
      QueueRequest( DebugRequestType.SET_REGISTER, Register[0], Value );
    }



    List<WatchEntry> IDebugger.CurrentWatches()
    {
      return m_WatchEntries;
    }



  }
}
