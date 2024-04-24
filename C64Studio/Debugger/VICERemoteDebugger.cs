﻿using RetroDevStudio.Types;
using GR.Memory;
using RetroDevStudio;
using System;
using System.Collections.Generic;
using System.Text;
using RetroDevStudio.Documents;

namespace RetroDevStudio
{
  public class VICERemoteDebugger : IDebugger
  {
    public enum WinViceVersion
    {
      V_2_3 = 0,
      V_2_4 = 1,
      V_3_0 = 2
    };

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
    private StudioCore                Core = null;
    private List<WatchEntry>          m_WatchEntries = new List<WatchEntry>();
    private int                       m_BytesToSend = 0;
    private int                       m_BrokenAtBreakPoint = -1;
    private bool                      m_InitialBreakpointRemoved = false;
    private bool                      m_InitialBreakCompleted = false;
    private bool                      m_IsCartridge = false;
    public WinViceVersion             m_ViceVersion = WinViceVersion.V_2_3;
    public bool                       m_BinaryMemDump = true;
    private DebuggerState             m_State = DebuggerState.NOT_CONNECTED;

    private int                       m_LastRequestedMemoryStartAddress = 0;
    private int                       m_LastRequestedMemorySize = 32;
    private MemorySource              m_LastRequestedMemorySource = MemorySource.AS_CPU;
    private RegisterInfo              CurrentRegisterValues = new RegisterInfo();

    private MachineType               m_ConnectedMachine = MachineType.UNKNOWN;

    private int                       m_LastRequestID = 0;
    private bool                      m_ShuttingDown = false;

    private Dictionary<uint,RequestData>   m_UnansweredBinaryRequests = new Dictionary<uint, RequestData>();



    GR.Collections.Map<int, byte>     m_MemoryValues = new GR.Collections.Map<int, byte>();
    GR.Collections.Map<int, bool>     m_RequestedMemoryValues = new GR.Collections.Map<int, bool>();

    GR.Collections.Set<Types.Breakpoint> m_BreakPoints = new GR.Collections.Set<RetroDevStudio.Types.Breakpoint>();

    public event BaseDocument.DocumentEventHandler DocumentEvent;


    public VICERemoteDebugger( StudioCore Core )
    {
      this.Core = Core;
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
        m_InitialBreakCompleted = false;
        m_InitialBreakpointRemoved = false;
        connectResultReceived = false;
        m_ReceivedDataBin.Clear();
        m_ResponseLines.Clear();
        m_RequestQueue.Clear();
        m_UnansweredBinaryRequests.Clear();
        m_LastRequestID = 0;
        if ( client != null )
        {
          client.Disconnect( false );
          client.Close();
        }
        client = new System.Net.Sockets.Socket( System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp );
        client.BeginConnect( "127.0.0.1", 6510, new AsyncCallback( Connected ), client );
      }
      catch ( System.Net.Sockets.SocketException se )
      {
        Core.AddToOutput( "RemoteDebugger.Connect Exception:" + se.ToString() );
        return false;
      }
      while ( !connectResultReceived )
      {
        System.Threading.Thread.Sleep( 50 );
      }
      if ( client.Connected )
      {
        m_State = DebuggerState.RUNNING;

        // connected, force reset plus add break points now
        if ( m_IsCartridge )
        {
          Debug.Log( "Connected - force break" );
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
        client.BeginReceive( data, 0, size, System.Net.Sockets.SocketFlags.None, new AsyncCallback( ReceiveData ), client );
      }
      catch ( System.Net.Sockets.SocketException se )
      {
        Core.AddToOutput( "RemoteDebugger.Connected Exception:" + se.ToString() );
        //conStatus.Text = "Error connecting";
      }
      connectResultReceived = true;
    }



    void OnDataReceived( byte[] data, int Offset, int Length )
    {
      m_ReceivedDataBin.Append( data, Offset, Length );

      //Debug.Log( "OnDataReceived " + m_ReceivedDataBin.ToString( Offset, Length ) );

      do
      {
        // have binary data?
        int binPos = m_ReceivedDataBin.Find( 0x02 );
        if ( binPos != -1 )
        {
          m_ReceivedDataBin.TruncateFront( binPos );
        }

        if ( ( m_ReceivedDataBin.Length > 0 )
        && ( m_ReceivedDataBin.ByteAt( 0 ) == 0x02 ) )
        {
          // binary dump
          if ( ( m_ReceivedDataBin.Length > 5 )
          && ( m_ReceivedDataBin.ByteAt( 0 ) == 0x02 ) )
          {
            uint answerLength = m_ReceivedDataBin.UInt32At( 1 );
            if ( m_ReceivedDataBin.Length >= 6 + answerLength )
            {
              // received all data
              // define MON_ERR_OK            0
              // #define MON_ERR_CMD_TOO_SHORT 0x80  /* command length is not enough for this command */
              // #define MON_ERR_INVALID_PARAMETER 0x81  /* command has invalid parameters */
              // byte 0: STX (0x02)
              // byte 1: answer length low
              // byte 2: answer length (bits 8-15)
              // byte 3: answer length (bits 16-23)
              // byte 4: answer length (bits 24-31, that is, high)
              // byte 5: error code
              // byte 6 - (answer length+6): the binary answer

              //Debug.Log( "Got MemDump data as " + m_ReceivedDataBin.SubBuffer( 0, 6 + (int)answerLength ).ToString() );
              // 0201010000 00 0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
              byte    resultCode = m_ReceivedDataBin.ByteAt( 5 );
              if ( resultCode != 0 )
              {
                Debug.Log( "Error receiving data: " + resultCode );
              }
              else
              {
                /*
                if ( m_Request.Parameter1 != m_Request.AdjustedStartAddress )
                {
                  Debug.Log( "Shifted start address" );
                }
                if ( ( answerLength != m_Request.Parameter2 - m_Request.Parameter1 + 1 )
                &&   ( m_Request.Parameter2 != -1 ) )
                {
                  Debug.Log( "warped size" );
                }
                Debug.Log( "Received " + answerLength + " bytes beginning at " + m_Request.Parameter1.ToString( "x" ) );
                  */
                for ( int i = 0; i < answerLength; ++i )
                {
                  m_MemoryValues[m_Request.Parameter1 + i] = m_ReceivedDataBin.ByteAt( i + 6 );
                }
                DebugEvent( new DebugEventData()
                {
                  Type = RetroDevStudio.DebugEvent.UPDATE_WATCH,
                  Request = m_Request,
                  Data = m_ReceivedDataBin.SubBuffer( 6, (int)answerLength )
                } );

                m_ResponseLines.Clear();
                m_ReceivedDataBin.TruncateFront( 6 + (int)answerLength );
                /*
                if ( !receivedData.Empty() )
                {
                  string stringData = Encoding.ASCII.GetString( receivedData.Data(), 0, (int)receivedData.Length );
                  m_ReceivedData = stringData;
                }*/
              }
              m_Request = new RequestData( DebugRequestType.NONE );
              StartNextRequestIfAvailable();
            }
            else
            {
              // not complete yet
              break;
            }
            /*
            byte 0: STX (0x02)
            byte 1: answer length low
            byte 2: answer length (bits 8-15)
            byte 3: answer length (bits 16-23)
            byte 4: answer length (bits 24-31, that is, high)
            byte 5: error code
            byte 6 - (answer length+6): the binary answer
              */
          }
        }
        else
        {
          int     linePos = m_ReceivedDataBin.Find( 0x0a );
          if ( linePos == -1 )
          {
            // binary dump data inside?
            linePos = m_ReceivedDataBin.Find( 0x02 );
          }

          //string stringData = Encoding.ASCII.GetString( m_ReceivedDataBin.Data(), 0, (int)m_ReceivedDataBin.Length );

          //Debug.Log( "Received " + receivedData.ToString() + " bytes (" + receivedData.ToString() + "), " + receivedData.ToAsciiString() );

          //receivedData.TruncateFront( (int)receivedData.Length );

          //int linePos = m_ReceivedData.IndexOf( '\n' );

          string    stringData = "";
          if ( linePos == -1 )
          {
            if ( ( m_ReceivedDataBin.Length == 10 )
            && ( m_ViceVersion >= WinViceVersion.V_2_4 ) )
            {
              // Vice 2.4 sometimes does NOT send an ending line break
              m_ReceivedDataBin.Clear();
              ProcessResponse();
              break;
            }
            else
            {
              // not enough data yet
              break;
            }
          }
          else
          {
            stringData = Encoding.ASCII.GetString( m_ReceivedDataBin.Data(), 0, linePos );
            m_ReceivedDataBin.TruncateFront( linePos + 1 );
          }

          if ( m_ViceVersion >= WinViceVersion.V_2_4 )
          {
            bool processed = false;
            while ( linePos != -1 )
            {
              //Debug.Log( "Cut at " + ( linePos + 1 ).ToString() );
              string line = stringData.Substring( 0, linePos );
              //stringData = stringData.Substring( linePos + 1 );

              linePos = stringData.IndexOf( '\n' );
              if ( linePos == -1 )
              {
                if ( stringData.Length == 10 )
                {
                  // Vice 2.4 sometimes does NOT send an ending line break
                  m_ResponseLines.Add( line );
                  processed = true;
                  ProcessResponse();
                  stringData = "";
                  break;
                }
              }

              //Debug.Log( "Line:" + line );
              //Debug.Log( "Receive data left " + m_ReceivedData );
              m_ResponseLines.Add( line );
            }
            if ( !processed )
            {
              ProcessResponse();
            }
          }
          else
          {
            while ( linePos != -1 )
            {
              //Debug.Log( "Cut at " + ( linePos + 1 ).ToString() );
              string line = stringData.Substring( 0, linePos );
              //stringData = stringData.Substring( linePos + 1 );

              linePos = stringData.IndexOf( '\n' );
              if ( linePos == -1 )
              {
                if ( ( stringData.Length == 10 )
                && ( m_ViceVersion >= WinViceVersion.V_2_4 ) )
                {
                  // Vice 2.4 sometimes does NOT send an ending line break
                  stringData = "";
                }
              }

              //Debug.Log( "Line:" + line );
              //Debug.Log( "Receive data left " + m_ReceivedData );
              m_ResponseLines.Add( line );

              ProcessResponse();

            }
          }
        }
      }
      while ( m_ReceivedDataBin.Length > 0 );
      //m_ReceivedData = receivedData;
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
          //Debug.Log( "Other side closed" );
          DebugEvent( new DebugEventData()
          {
            Type = RetroDevStudio.DebugEvent.EMULATOR_CLOSED
          }  );
          m_Request.Type = DebugRequestType.NONE;
          DisconnectFromEmulator();
          return;
        }

        OnDataReceived( data, 0, recv );
        //Debug.Log( "Set up following BeginReceive" );
        if ( client != null )
        {
          client.BeginReceive( data, 0, size, System.Net.Sockets.SocketFlags.None, new AsyncCallback( ReceiveData ), client );
        }
      }
      catch ( System.ObjectDisposedException od )
      {
        Core.AddToOutputLine( "ReceiveData Exception:" + od.ToString() );
      }
      catch ( System.Net.Sockets.SocketException se )
      {
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
          Debug.Log( "Not all " + m_BytesToSend + " bytes were sent! (only " + sent + ")" );
          if ( sent > 0 )
          {
            m_BytesToSend -= sent;
            client.BeginSend( m_DataToSend, sent, m_BytesToSend, System.Net.Sockets.SocketFlags.None, new AsyncCallback( SendData ), client );
          }
          return;
        }
        else
        {
          //Debug.Log( "Sent " + sent + " bytes (expected " + m_BytesToSend + ")" );
        }

        Core.Debugging.ForceEmulatorRefresh();

        if ( ( m_BinaryMemDump )
        &&   ( m_ViceVersion >= WinViceVersion.V_2_4 )
        &&   ( ( m_Request.Type == DebugRequestType.MEM_DUMP )
        ||     ( m_Request.Type == DebugRequestType.TRACE_MEM_DUMP ) ) )
        {
          // skip processresponse
        }
        else
        {
          ProcessResponse();
        }
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



    public bool SendCommand( string Command )
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
      string    fullCommand = Command + "\n";

      System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();

      m_DataToSend = enc.GetBytes( fullCommand );

      try
      {
        var bufferData = new GR.Memory.ByteBuffer( m_DataToSend );
        InterfaceLog( "Debugger>" + Command + "/" + bufferData.ToString() );
        m_BytesToSend = m_DataToSend.Length;

        // sync send
        int totalBytesSent = 0;

        while ( m_BytesToSend > 0 )
        {
          //Debug.Log( "Trying to send " + m_BytesToSend + " bytes" );
          int bytesSent = client.Send( m_DataToSend, totalBytesSent, m_BytesToSend, System.Net.Sockets.SocketFlags.None );
          //Debug.Log( "Sent " + bytesSent + " bytes" );
          if ( bytesSent == 0 )
          {
            InterfaceLog( "Could not send " + m_BytesToSend + " bytes" );
            break;
          }
          m_BytesToSend -= bytesSent;
          totalBytesSent += bytesSent;
        }
        // async send
        //client.BeginSend( m_DataToSend, 0, m_DataToSend.Length, System.Net.Sockets.SocketFlags.None, new AsyncCallback( SendData ), client );
      }
      catch ( Exception ex )
      {
        Core.AddToOutput( "SendCommand Exception:" + ex.ToString() );
      }
      return true;
    }



    private void InterfaceLog( string Text )
    {
      Debug.Log( Text );
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
        Debug.Log( "Debugger>" + Command.ToString() + ", " + m_DataToSend.Length + " bytes" );
        m_BytesToSend = m_DataToSend.Length;
        int totalBytesSent = 0;

        while ( m_BytesToSend > 0 )
        {
          int bytesSent = client.Send( m_DataToSend, totalBytesSent, m_BytesToSend, System.Net.Sockets.SocketFlags.None );
          if ( bytesSent == 0 )
          {
            Debug.Log( "Could not send " + m_BytesToSend + " bytes" );
            break;
          }
          Debug.Log( "Sent " + bytesSent + " bytes" );
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

      if ( ( m_BinaryMemDump )
      &&   ( m_ViceVersion >= WinViceVersion.V_2_4 )
      &&   ( ( m_Request.Type == DebugRequestType.MEM_DUMP )
      ||     ( m_Request.Type == DebugRequestType.TRACE_MEM_DUMP ) ) )
      {
        // skip processresponse
      }
      else
      {
        ProcessResponse();
      }
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



    private void RaiseDebugEvent( DebugEventData Data )
    {
      DebugEvent?.Invoke( Data );
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
      int breakpointToRemove = 1;
      m_State = DebuggerState.PAUSED;

      if ( ( ( m_ViceVersion == WinViceVersion.V_2_3 )
      &&     ( m_ResponseLines.Count >= 3 ) )
      || ( ( m_ViceVersion >= WinViceVersion.V_2_4 )
      &&   ( m_ResponseLines.Count >= 2 ) ) )
      {
        //(C:$1c1f) #2 (Break) .C:1c25   B1 17      LDA ($17),Y
        string firstLine = m_ResponseLines[0];
        int hashPos = firstLine.IndexOf( '#' );
        if ( hashPos != -1 )
        {
          int spacePos = m_ResponseLines[0].IndexOf( ' ', hashPos );
          if ( spacePos != -1 )
          {
            if ( !int.TryParse( m_ResponseLines[0].Substring( hashPos + 1, spacePos - hashPos - 1 ), out breakpointToRemove ) )
            {
              Debug.Log( "Failed to parse breakpoint ID" );
            }
            else
            {
              Debug.Log( "Break point ID = " + breakpointToRemove );
            }
          }
        }
        //m_ResponseLines.Clear();
        //m_Request = new RequestData( Request.NONE );
        m_Request = new RequestData( DebugRequestType.BREAK_INFO );
        ProcessResponse();
        return;
      }
      m_Request = new RequestData( DebugRequestType.BREAK_INFO );
    }



    private void ProcessResponse()
    {
      Debug.Log( "ProcessResponse for Request " + m_Request.Type.ToString() + ", lines " + m_ResponseLines.Count.ToString() );
      string    total = "";
      foreach ( var line in m_ResponseLines )
      {
        total += "\r\n" + line;
      }
      Debug.Log( "Responselines are:" + total );

      if ( ( total.ToUpper().Contains( "AN ERROR OCCURRED" ) )
      &&   ( total.ToUpper().Contains( "JAM" ) ) )
      {
        InterfaceLog( "CPU JAM encountered: " + total );
        InterfaceLog( "Disconnecting from VICE" );
        DisconnectFromEmulator();
        return;
      }

      switch ( m_Request.Type )
      {
        case DebugRequestType.READ_REGISTERS:
          if ( m_ResponseLines.Count >= 2 )
          {
            // ReadRegisters:(C:$0810)   ADDR AC XR YR SP 00 01 NV-BDIZC LIN CYC
            // ReadRegisters:.;0810 00 00 00 f6 2f 37 00100000 055 015
            string    registers = m_ResponseLines[m_ResponseLines.Count - 1];

            //if ( m_ViceVersion >= WinViceVersion.V_3_0 )
            {
              // since of 3.1 and data break points the register values may be in the third line - yay
              //  don't we just love a human text cmd line as interface
              // .C:8e77  AC A9 A6    LDY .ARMOR_TURN_POS - A:00 X:00 Y:08 SP:f2 ..-.....   29087560
              // (C:$8e77)   ADDR A  X  Y  SP 00 01 NV-BDIZC LIN CYC  STOPWATCH
              // could there be a nother line??
              if ( registers.IndexOf( "ADDR A  X  Y" ) != -1 )
              {
                // Incomplete registers data encountered, fetch another line?
                // yes! - require another one! 
                break;
              }
            }

            char[] separators = new char[1];
            separators[0] = ' ';
            string[]  registerValues = registers.Split( separators, StringSplitOptions.RemoveEmptyEntries );
            if ( ( ( m_ViceVersion == WinViceVersion.V_2_3 )
            &&     ( registerValues.Length == 10 ) )
            ||   ( ( m_ViceVersion >= WinViceVersion.V_2_4 )
            &&     ( registerValues.Length == 11 ) ) )
            {
              var ded = DebugEventDataFromRegisterValueString( registerValues );

              DebugEvent( ded );
            }
            m_ResponseLines.Clear();
            m_Request = new RequestData( DebugRequestType.NONE );
          }
          break;
        case DebugRequestType.NEXT:
          if ( ( m_ResponseLines.Count > 0 )
          && ( ( m_ResponseLines[0].IndexOf( "(Break)" ) != -1 )
          ||   ( m_ResponseLines[0].IndexOf( "(Stop on  exec" ) != -1 )
          ||   ( m_ResponseLines[0].IndexOf( "(Stop on  load" ) != -1 )
          ||   ( m_ResponseLines[0].IndexOf( "(Stop on store" ) != -1 ) ) )
          {
            // a unexpected break!
            HandleBreakpoint();
            break;
          }
          if ( ( ( m_ViceVersion == WinViceVersion.V_2_3 )
          &&     ( m_ResponseLines.Count == 3 ) )
          ||   ( ( m_ViceVersion >= WinViceVersion.V_2_4 )
          &&     ( m_ResponseLines.Count == 1 ) ) )
          {
            m_ResponseLines.Clear();
            m_Request = new RequestData( DebugRequestType.NONE );
            m_State = DebuggerState.PAUSED;
          }
          break;
        case DebugRequestType.RETURN:
          if ( ( ( m_ViceVersion == WinViceVersion.V_2_3 )
          &&     ( m_ResponseLines.Count == 3 ) )
          ||   ( ( m_ViceVersion >= WinViceVersion.V_2_4 )
          &&     ( m_ResponseLines.Count == 1 ) ) )
          {
            m_ResponseLines.Clear();
            m_State = DebuggerState.PAUSED;
            m_Request = new RequestData( DebugRequestType.NONE );

            // this was removed? It's required to refresh data
            RefreshRegistersAndWatches();
          }
          break;
        case DebugRequestType.EXIT:
          m_State = DebuggerState.RUNNING;
          m_ResponseLines.Clear();
          m_Request = new RequestData( DebugRequestType.NONE );
          break;
        case DebugRequestType.QUIT:
          if ( m_ResponseLines.Count == 1 )
          {
            m_ResponseLines.Clear();
            m_Request = new RequestData( DebugRequestType.NONE );
          }
          break;
        case DebugRequestType.STEP:
          if ( ( !m_InitialBreakCompleted )
          &&   ( m_ResponseLines.Count > 0 ) )
          {
            Debug.Log( "Initial break encountered" );
            // our first break
            m_InitialBreakCompleted = true;
            m_ResponseLines.Clear();
            m_State = DebuggerState.PAUSED;
            m_Request = new RequestData( DebugRequestType.NONE );

            // add break points now
            if ( Core.Debugging.OnInitialBreakpointReached( -1 ) )
            {
              //QueueRequest( Request.RESET );
            }
            break;
          }

          if ( ( ( m_ViceVersion == WinViceVersion.V_2_3 )
          &&     ( m_ResponseLines.Count == 3 ) )
          ||   ( ( m_ViceVersion >= WinViceVersion.V_2_4 )
          &&     ( m_ResponseLines.Count == 1 ) ) )
          {
            m_ResponseLines.Clear();
            m_State = DebuggerState.PAUSED;
            m_Request = new RequestData( DebugRequestType.NONE );
          }
          break;
        case DebugRequestType.BREAK_INFO:
          if ( ( ( m_ViceVersion == WinViceVersion.V_2_3 )
          &&     ( m_ResponseLines.Count >= 3 ) )
          ||   ( ( m_ViceVersion >= WinViceVersion.V_2_4 )
          &&     ( m_ResponseLines.Count >= 2 ) ) )
          {
            //(C:$1c1f) #2 (Break) .C:1c25   B1 17      LDA ($17),Y
            string  firstLine = m_ResponseLines[0];
            int hashPos = firstLine.IndexOf( '#' );
            if ( hashPos != -1 )
            {
              int spacePos = m_ResponseLines[0].IndexOf( ' ', hashPos );
              if ( spacePos != -1 )
              {
                if ( !int.TryParse( m_ResponseLines[0].Substring( hashPos + 1, spacePos - hashPos - 1 ), out m_BrokenAtBreakPoint ) )
                {
                  Debug.Log( "Failed to parse breakpoint ID" );
                }
                else
                {
                  //Debug.Log( "Break point ID = " + m_BrokenAtBreakPoint );
                }
              }
              else
              {
                Debug.Log( "Could not deduce breakpoint ID" );
              }
            }
            else
            {
              Debug.Log( "Could not deduce breakpoint ID 2" );
            }
            Debug.Log( "Last line, should be (C:$xxxx): " + m_ResponseLines[m_ResponseLines.Count - 1] );
            m_ResponseLines.Clear();

            OnBreakpointHit();
            m_Request = new RequestData( DebugRequestType.NONE );
          }
          break;
        case DebugRequestType.NONE:
          if ( ( m_ResponseLines.Count > 0 )
          &&   ( ( m_ResponseLines[0].IndexOf( "(Break)" ) != -1 )
          ||     ( m_ResponseLines[0].IndexOf( "(Stop on  exec" ) != -1 )
          ||     ( m_ResponseLines[0].IndexOf( "(Stop on  load" ) != -1 )
          ||     ( m_ResponseLines[0].IndexOf( "(Stop on store" ) != -1 ) ) )
          {
            // a unexpected break!
            HandleBreakpoint();
          }
          break;
        case DebugRequestType.RESET:
          m_ResponseLines.Clear();
          m_Request = new RequestData( DebugRequestType.NONE );
          break;
        case DebugRequestType.DELETE_BREAKPOINT:
          if ( m_ResponseLines.Count == 0 )
          {
            m_ResponseLines.Clear();
            if ( m_Request.Breakpoint != null )
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
            m_Request = new RequestData( DebugRequestType.NONE );
          }
          break;
        case DebugRequestType.ADD_BREAKPOINT:
          if ( m_ResponseLines.Count == 1 )
          {
            // [0] = "(C:$01f8) BREAK: 3 C:$0855   enabled"
            // oder  "BREAK: 4  C:$25c1  (Stop on exec)<10>(C:$0007) "
            // oder            #1 (Stop on  exec 9258)  279 018
            //                 .C:9258  A9 36       LDA #$36       - A:34 X:3F Y:00 SP:f2 .V-..IZC   29285379
            int breakpointID = 0;
            int responsePos = m_ResponseLines[0].IndexOf( "BREAK: " );
            if ( responsePos == -1 )
            {
              responsePos = m_ResponseLines[0].IndexOf( "WATCH: " );
            }
            if ( responsePos != -1 )
            {
              int spaceBehindPos = m_ResponseLines[0].IndexOf( ' ', responsePos + 7 );
              if ( spaceBehindPos != -1 )
              {
                string breakID = m_ResponseLines[0].Substring( responsePos + 7, spaceBehindPos - responsePos - 7 );
                int.TryParse( breakID, out breakpointID );
              }
            }
            //Debug.Log( "Breakpoint ID = " + breakpointID.ToString() );
            if ( ( breakpointID != 0 )
            &&   ( m_Request.Breakpoint != null ) )
            {
              m_Request.Breakpoint.RemoteIndex = breakpointID;

              RaiseDocumentEvent( new BaseDocument.DocEvent( BaseDocument.DocEvent.Type.BREAKPOINT_UPDATED, m_Request.Breakpoint ) );
            }
            m_ResponseLines.Clear();
            m_Request = new RequestData( DebugRequestType.NONE );
          }
          break;
        case DebugRequestType.MEM_DUMP:
        case DebugRequestType.TRACE_MEM_DUMP:
          {
            int expectedLines = 1 + ( m_Request.Parameter2 - m_Request.Parameter1 ) / 16;
            Debug.Log( "Expected lines:" + expectedLines );

            if ( m_ResponseLines.Count == expectedLines )
            {
              // (C:$0810) >C:00f1  87                                                   .

              GR.Memory.ByteBuffer    dumpData = new GR.Memory.ByteBuffer();
              foreach ( string line in m_ResponseLines )
              {
                int addressPos = line.IndexOf( ">C:" );
                if ( addressPos == -1 )
                {
                  addressPos = line.IndexOf( "C:" );
                }
                if ( ( addressPos != -1 )
                &&   ( addressPos < line.Length + 9 ) )
                {
                  string byteValues = line.Substring( addressPos + 9, 50 ).Replace( " ", "" );

                  dumpData.AppendHex( byteValues );
                }
              }

              OnMemoryDumpReceived( dumpData );

              m_ResponseLines.Clear();
            }
          }
          break;
        case DebugRequestType.RAM_MODE:
          if ( m_ResponseLines.Count == 0 )
          {
            m_ResponseLines.Clear();
            m_Request = new RequestData( DebugRequestType.NONE );
          }
          break;
        case DebugRequestType.SET_REGISTER:
          m_ResponseLines.Clear();
          m_Request = new RequestData( DebugRequestType.NONE );
          RefreshRegistersAndWatches();
          break;
        default:
          Debug.Log( "Unknown request state! " + m_Request.Type.ToString() );
          m_ResponseLines.Clear();
          break;
      }
      if ( m_Request.Type == DebugRequestType.NONE )
      {
        StartNextRequestIfAvailable();
      }
    }



    private void OnMemoryDumpReceived( ByteBuffer DumpData )
    {
      //Debug.Log( "Got MemDump data as " + DumpData.ToString() );
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
            //Debug.Log( "Virtual only, go on" );
            QueueRequest( DebugRequestType.EXIT );
          }
          else
          {
            //Debug.Log( "Has non virtual bp" );
            QueueRequest( DebugRequestType.REFRESH_VALUES );
            RefreshMemory( m_LastRequestedMemoryStartAddress, m_LastRequestedMemorySize, m_LastRequestedMemorySource );
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



    private void OnBreakpointHit()
    {
      m_State = DebuggerState.PAUSED;

      //Debug.Log( "Breakpoint " + m_BrokenAtBreakPoint + " hit" );
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
            Debug.Log( "Remove auto startup breakpoint " + breakPoint.RemoteIndex );
            QueueRequest( DebugRequestType.DELETE_BREAKPOINT, m_BrokenAtBreakPoint ).Breakpoint = breakPoint;
            brokenBP = null;
            break;
          }
        }
      }

      // newly set to we don't add new breakpoints or do the double command thing
      m_InitialBreakCompleted = true;

      bool skipRefresh = false;

      if ( ( !m_IsCartridge )
      &&   ( m_BrokenAtBreakPoint == 1 ) )
      {
        if ( ( Core.Debugging.InitialBreakpointIsTemporary )
        &&   ( !m_InitialBreakpointRemoved ) )
        {
          // auto break point
          m_InitialBreakpointRemoved = true;

          // DEBUGHACK
          //QueueRequest( Request.MEM_DUMP, 0, 0xffff );
          //Debug.Log( "Remove initial breakpoint " + m_BrokenAtBreakPoint );
          QueueRequest( DebugRequestType.DELETE_BREAKPOINT, m_BrokenAtBreakPoint );

          skipRefresh = Core.Debugging.OnInitialBreakpointReached( breakAddress );
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

      if ( !skipRefresh )
      {
        QueueRequest( DebugRequestType.REFRESH_VALUES );
        RefreshMemory( m_LastRequestedMemoryStartAddress, m_LastRequestedMemorySize, m_LastRequestedMemorySource );
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
 	    if ( ( m_RequestQueue.Count != 0 )
      &&   ( m_ReceivedDataBin.Length == 0 ) )
      {
        Debug.Log( "------> StartNextRequest:" + m_RequestQueue[0].Type );
        RequestData nextRequest = m_RequestQueue[0];
        m_RequestQueue.RemoveAt( 0 );

        SendRequest( nextRequest );
      }
    }



    private bool SendRequest( RequestData Data )
    {
      if ( m_Request.Type != DebugRequestType.NONE )
      {
        Debug.Log( "====> Trying to send request while processing another! (" + m_Request.Type + ")" );
        return false;
      }
      //Debug.Log( "Set request data to " + Data.Type.ToString() );
      m_Request = Data;
      m_ResponseLines.Clear();

      switch ( m_Request.Type )
      {
        case DebugRequestType.READ_REGISTERS:
          return SendCommand( "registers" );
        case DebugRequestType.SET_REGISTER:
          {
            string    command = "r ";
            switch ( m_Request.Parameter1 )
            {
              case 'A':
                command += "A";
                break;
              case 'X':
                command += "X";
                break;
              case 'Y':
                command += "Y";
                break;
              case 'F':
                command += "FL";
                break;
              case 'P':
                command += "PC";
                break;
              case 'S':
                command += "SP";
                break;
            }
            command += " = $" + m_Request.Parameter2.ToString( "X" );

            return SendCommand( command );
          }
        case DebugRequestType.NEXT:
          m_MemoryValues.Clear();
          m_RequestedMemoryValues.Clear();
          m_State = DebuggerState.RUNNING;
          return SendCommand( "next" );
        case DebugRequestType.STEP:
          m_MemoryValues.Clear();
          m_RequestedMemoryValues.Clear();
          m_State = DebuggerState.RUNNING;
          return SendCommand( "step" );
        case DebugRequestType.EXIT:
          m_Request.Type = DebugRequestType.NONE;
          m_State = DebuggerState.RUNNING;
          m_RequestedMemoryValues.Clear();
          return SendCommand( "exit" );
        case DebugRequestType.QUIT:
          return SendCommand( "quit" );
        case DebugRequestType.RETURN:
          m_MemoryValues.Clear();
          m_RequestedMemoryValues.Clear();
          m_State = DebuggerState.RUNNING;

          return SendCommand( "return" );
        case DebugRequestType.RESET:
          // hard reset
          return SendCommand( "reset 1" );
        case DebugRequestType.ADD_BREAKPOINT:
          {
            string request = "break ";

            if ( m_Request.Breakpoint.TriggerOnExec )
            {
              request += "exec ";
            }
            if ( m_ViceVersion > WinViceVersion.V_2_3 )
            {
              if ( m_Request.Breakpoint.TriggerOnLoad )
              {
                request += "load ";
              }
              if ( m_Request.Breakpoint.TriggerOnStore )
              {
                request += "store ";
              }
            }
            request += m_Request.Parameter1.ToString( "x" );
            if ( !string.IsNullOrEmpty( m_Request.Breakpoint.Conditions ) )
            {
              request += " if " + m_Request.Breakpoint.Conditions;
            }

            Debug.Log( "Add breakpoint request " + request );

            return SendCommand( request );
          }
        case DebugRequestType.DELETE_BREAKPOINT:
          return SendCommand( "delete " + m_Request.Parameter1.ToString() );
        case DebugRequestType.RAM_MODE:
          return SendCommand( "bank " + m_Request.Info );
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

            if ( !m_BinaryMemDump )
            //if ( m_ViceVersion == WinViceVersion.V_2_3 )
            {
              if ( m_Request.Parameter2 == -1 )
              {
                return SendCommand( "m $" + ( offset + m_Request.Parameter1 ).ToString( "x" ) );
              }
              return SendCommand( "m $" + ( offset + m_Request.Parameter1 ).ToString( "x" ) + " $" + ( offset + m_Request.Parameter2 ).ToString( "x" ) );
            }

            // binary request
            ushort    startAddress = (ushort)( offset + m_Request.Parameter1 );
            ushort    endAddress = (ushort)( offset + m_Request.Parameter2 );
            if ( m_Request.Parameter2 == -1 )
            {
              if ( startAddress + 1 >= 0x10000 )
              {
                // would over flow
                --startAddress;
                endAddress = 0xffff;
              }
              else
              {
                endAddress = (ushort)( startAddress + 1 );
              }
            }
            else if ( startAddress == endAddress )
            {
              if ( startAddress == 0xffff )
              {
                --startAddress;
              }
              else
              {
                ++endAddress;
              }
            }
            m_Request.AdjustedStartAddress = startAddress;

            // binary dump (since Vice 2.4)
            /* The binary remote monitor commands are injected into the "normal" commands. 
            The remote monitor detects a binary command because it starts with ASCII STX 
            (0x02). After this, there is one byte telling the length of the command. The 
            next byte describes the command. Currently, only 0x01 is implemented which 
            is "memdump". 

            Note that the command length byte (the one after STX) does *not* count the 
            STX, the command length nor the command byte.

            Also note that there is no termination character. The command length acts as 
            synchronisation point.
              
              
            For the memdump command, the next bytes are as follows:
            1. start address low
            2. start address high
            3. end address low
            4. end address high
            5. memspace

            The memspace describes which part of the computer you want to read: 
            0 --> the computer (C64)
            1 --> drive 8, 2 --> drive 9, 3 --> drive 10, 4 --> drive 11 

            So, for a memdump of 0xa0fe to 0xa123, you have to issue the bytes 
            (in this order):

            0x02 (STX), 0x05 (command length), 0x01 (command: memdump), 0xfe (SA low), 
            0xa0 (SA high), 0x23 (EA low), 0xa1 (EA high), 0x00 (computer memspace) 

            The answer looks as follows:

            byte 0: STX (0x02)
            byte 1: answer length low
            byte 2: answer length (bits 8-15)
            byte 3: answer length (bits 16-23)
            byte 4: answer length (bits 24-31, that is, high)
            byte 5: error code
            byte 6 - (answer length+6): the binary answer
            [...]

            Error codes are currently:
            0x00: ok, everything worked
            0x80: command length is not long enough for this specific command 
            0x81: an invalid parameter occurred
              */
            GR.Memory.ByteBuffer tmpData = new GR.Memory.ByteBuffer();

            tmpData.AppendU8( 0x02 );   // STX
            tmpData.AppendU8( 5 );      // length of command
            tmpData.AppendU8( 0x01 );   // MON_CMD_MEMDUMP = 1
            tmpData.AppendU16( startAddress );
            tmpData.AppendU16( endAddress );
            tmpData.AppendU8( 0 );    // mem space
            
            return SendCommand( tmpData );
          }
      }
      return true;
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
      Debug.Log( "QueueRequest - sending data directly? Queue has " + m_RequestQueue.Count + " entries, request is " + Data.Type + ", current request type is " + m_Request.Type + ", current incoming data is " + m_ReceivedDataBin.ToString() );
      if ( ( m_Request.Type != DebugRequestType.NONE )
      ||   ( m_RequestQueue.Count > 0 )
      ||   ( m_ReceivedDataBin.Length > 0 ) )
      {
        Debug.Log( "-no" );
        m_RequestQueue.Add( Data );
        return;
      }
      Debug.Log( "-yes" );
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
          Debug.Log( "Virtual bp!" );
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
            Debug.Log( "Queue - Remove breakpoint " + breakPoint.RemoteIndex );
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
          Debug.Log( "Queue - Remove breakpoint " + breakPoint.RemoteIndex );
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
            &&   ( client.Connected ) )
            {
              Debug.Log( "Queue - Remove breakpoint " + breakPoint.RemoteIndex );
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

      RefreshRegistersAndWatches();
      SetAutoRefreshMemory( Core.MainForm.m_DebugMemory.MemoryStart,
                                     Core.MainForm.m_DebugMemory.MemorySize,
                                     Core.MainForm.m_DebugMemory.MemoryAsCPU ? MemorySource.AS_CPU : MemorySource.RAM );
      RefreshMemory( Core.MainForm.m_DebugMemory.MemoryStart,
                              Core.MainForm.m_DebugMemory.MemorySize,
                              Core.MainForm.m_DebugMemory.MemoryAsCPU ? MemorySource.AS_CPU : MemorySource.RAM );
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
      switch ( Feature )
      {
        case DebuggerFeature.ADD_BREAKPOINTS_AFTER_STARTUP:
          return m_ViceVersion > VICERemoteDebugger.WinViceVersion.V_2_3;
        case DebuggerFeature.REQUIRES_DOUBLE_ACTION_AFTER_BREAK:
          return m_ViceVersion == VICERemoteDebugger.WinViceVersion.V_2_3;
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
      m_ConnectedMachine = EmulatorInfo.DetectMachineType( ToolRun );

      if ( ( fileVersion == null )
      ||   ( string.IsNullOrEmpty( fileVersion.ProductVersion ) ) )
      {
        //Core.AddToOutput( "Could not check emulator version, no ProductVersion tag was found, assume VICE > 3.2" );
        m_ViceVersion = VICERemoteDebugger.WinViceVersion.V_3_0;

        // TODO - borked with GTK 3.4 (introduction of new binary interface)
        //m_BinaryMemDump = true;
        m_BinaryMemDump = false;
        return true;
      }

      m_BinaryMemDump = false;
      if ( ( fileVersion.ProductVersion == "2.3" )
      ||   ( fileVersion.ProductVersion.StartsWith( "2.3." ) ) )
      {
        m_ViceVersion = VICERemoteDebugger.WinViceVersion.V_2_3;
      }
      else if ( ( fileVersion.ProductVersion == "2.4" )
      ||        ( fileVersion.ProductVersion.StartsWith( "2.4." ) ) )
      {
        m_ViceVersion = VICERemoteDebugger.WinViceVersion.V_2_4;
      }
      else if ( ( fileVersion.ProductVersion == "3.0" )
      ||        ( fileVersion.ProductVersion.StartsWith( "3.0." ) ) )
      {
        m_ViceVersion = VICERemoteDebugger.WinViceVersion.V_3_0;
        m_BinaryMemDump = true;
      }
      else if ( ( !string.IsNullOrEmpty( fileVersion.ProductVersion ) )
      &&        ( GR.Convert.ToI32( fileVersion.ProductVersion.Substring( 0, 1 ) ) >= 3 ) )
      {
        m_ViceVersion = VICERemoteDebugger.WinViceVersion.V_3_0;
        m_BinaryMemDump = true;
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
      if ( m_ViceVersion < VICERemoteDebugger.WinViceVersion.V_3_0 )
      {
        try
        {
          // hack that's needed for WinVICE to continue
          // fixed in WinVICE r25309
          RetroDevStudio.Debugging.InvalidateRect( Core.Executing.RunProcess.MainWindowHandle, IntPtr.Zero, false );
        }
        catch ( System.InvalidOperationException )
        {
        }
      }
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
