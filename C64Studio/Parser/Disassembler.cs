using System;
using System.Collections.Generic;
using System.Text;
using Tiny64;

namespace C64Studio.Parser
{
  public class Disassembler
  {
    GR.Memory.ByteBuffer                m_SourceData = null;
    bool[]                              m_CoveredData = null;

    public Processor                    m_Processor = null;



    public Disassembler( Processor Processor )
    {
      m_Processor = Processor;
    }



    protected bool DisassembleInstruction( GR.Memory.ByteBuffer Data, int DataStartAddress, int CodeStartAddress, out Tiny64.Opcode opcode, out bool NotComplete )
    {
      opcode = null;
      NotComplete = false;
      if ( ( CodeStartAddress < DataStartAddress )
      ||   ( CodeStartAddress >= DataStartAddress + Data.Length ) )
      {
        Debug.Log( "dis code outside of data" );
        NotComplete = true;
        return false;
      }
      byte instruction = Data.ByteAt( CodeStartAddress - DataStartAddress );
      if ( !m_Processor.OpcodeByValue.ContainsKey( instruction ) )
      {
        Debug.Log( "dis unknown opcode byte" );
        return false;
      }
      opcode = m_Processor.OpcodeByValue[instruction];
      if ( CodeStartAddress + opcode.NumOperands >= DataStartAddress + Data.Length )
      {
        // opcode with operands does not fit
        Debug.Log( "dis code operands outside of data" );
        NotComplete = true;
        return false;
      }
      return true;
    }



    private string MnemonicToString( Tiny64.Opcode opcode, GR.Memory.ByteBuffer Data, int DataStartAddress, int CodePos, GR.Collections.Set<ushort> AccessedAddresses, GR.Collections.Map<int,string> NamedLabels )
    {
      string output = opcode.Mnemonic.ToLower();

      ushort targetAddress = 0;
      bool    twoBytes = true;

      switch ( opcode.Addressing )
      {
        case Tiny64.Opcode.AddressingType.IMPLICIT:
          break;
        case Tiny64.Opcode.AddressingType.ABSOLUTE:
          targetAddress = Data.UInt16At( CodePos + 1 - DataStartAddress );
          break;
        case Tiny64.Opcode.AddressingType.ABSOLUTE_X:
          targetAddress = Data.UInt16At( CodePos + 1 - DataStartAddress );
          break;
        case Tiny64.Opcode.AddressingType.ABSOLUTE_Y:
          targetAddress = Data.UInt16At( CodePos + 1 - DataStartAddress );
          break;
        case Tiny64.Opcode.AddressingType.IMMEDIATE:
          targetAddress = Data.ByteAt( CodePos + 1 - DataStartAddress );
          twoBytes = false;
          break;
        case Tiny64.Opcode.AddressingType.INDIRECT:
          targetAddress = Data.UInt16At( CodePos + 1 - DataStartAddress );
          break;
        case Tiny64.Opcode.AddressingType.INDIRECT_X:
          targetAddress = Data.ByteAt( CodePos + 1 - DataStartAddress );
          twoBytes = false;
          break;
        case Tiny64.Opcode.AddressingType.INDIRECT_Y:
          targetAddress = Data.ByteAt( CodePos + 1 - DataStartAddress );
          twoBytes = false;
          break;
        case Tiny64.Opcode.AddressingType.RELATIVE:
          {
            // int delta = value - lineInfo.AddressStart - 2;
            sbyte relValue = (sbyte)Data.ByteAt( CodePos + 1 - DataStartAddress );

            targetAddress = (ushort)( relValue + 2 + CodePos );
          }
          break;
        case Tiny64.Opcode.AddressingType.ZEROPAGE:
          targetAddress = Data.ByteAt( CodePos + 1 - DataStartAddress );
          twoBytes = false;
          break;
        case Tiny64.Opcode.AddressingType.ZEROPAGE_X:
          targetAddress = Data.ByteAt( CodePos + 1 - DataStartAddress );
          twoBytes = false;
          break;
        case Tiny64.Opcode.AddressingType.ZEROPAGE_Y:
          targetAddress = Data.ByteAt( CodePos + 1 - DataStartAddress );
          twoBytes = false;
          break;
      }
      string    addressPlacement;

      if ( twoBytes )
      {
        addressPlacement = "$" + targetAddress.ToString( "x4" );
      }
      else
      {
        addressPlacement = "$" + targetAddress.ToString( "x2" );
      }

      if ( AccessedAddresses.ContainsValue( targetAddress ) )
      {
        addressPlacement = "label_" + targetAddress.ToString( "x4" );
      }

      if ( NamedLabels.ContainsKey( targetAddress ) )
      {
        addressPlacement = NamedLabels[targetAddress];
      }

      switch ( opcode.Addressing )
      {
        case Tiny64.Opcode.AddressingType.IMPLICIT:
          break;
        case Tiny64.Opcode.AddressingType.ABSOLUTE:
          output += " " + addressPlacement;
          break;
        case Tiny64.Opcode.AddressingType.ABSOLUTE_X:
          output += " " + addressPlacement + ", x";
          break;
        case Tiny64.Opcode.AddressingType.ABSOLUTE_Y:
          output += " " + addressPlacement + ", y";
          break;
        case Tiny64.Opcode.AddressingType.IMMEDIATE:
          output += " #" + addressPlacement;
          break;
        case Tiny64.Opcode.AddressingType.INDIRECT:
          output += " ( " + addressPlacement + " )";
          break;
        case Tiny64.Opcode.AddressingType.INDIRECT_X:
          output += " ( " + addressPlacement + ", x)";
          break;
        case Tiny64.Opcode.AddressingType.INDIRECT_Y:
          output += " ( " + addressPlacement + " ), y";
          break;
        case Tiny64.Opcode.AddressingType.RELATIVE:
          {
            // int delta = value - lineInfo.AddressStart - 2;

            output += " " + addressPlacement;
            //output += " (" + delta.ToString( "X2" ) + ")";
          }
          break;
        case Tiny64.Opcode.AddressingType.ZEROPAGE:
          output += " " + addressPlacement;
          break;
        case Tiny64.Opcode.AddressingType.ZEROPAGE_X:
          output += " " + addressPlacement + ", x";
          break;
        case Tiny64.Opcode.AddressingType.ZEROPAGE_Y:
          output += " " + addressPlacement + ", y";
          break;
      }
      return output;
    }



    public string DisassembleBinary( GR.Memory.ByteBuffer Data, int DataStartAddress, int ExportStartAddress, int Length, DisassemblerSettings Settings )
    {
      StringBuilder sb = new StringBuilder();

      int wrapSize = 8;

      while ( Length >= wrapSize )
      {
        if ( Settings.AddLineAddresses )
        {
          sb.Append( "$" );
          sb.Append( ExportStartAddress.ToString( "X4" ) + ":" );
        }
        //sb.Append( "          !byte " );
        sb.Append( "!byte " );
        for ( int i = 0; i < wrapSize; ++i )
        {
          sb.Append( "$" + Data.ByteAt( ExportStartAddress - DataStartAddress + i ).ToString( "X2" ) );
          if ( i + 1 < wrapSize )
          {
            sb.Append( "," );
          }
        }
        //Debug.Log( outputB );
        sb.Append( "\r\n" );
        ExportStartAddress += wrapSize;
        Length -= wrapSize;
      }
      if ( Length > 0 )
      {
        if ( Settings.AddLineAddresses )
        {
          sb.Append( "$" );
          sb.Append( ExportStartAddress.ToString( "X4" ) + ":" );
        }
        //sb.Append( "          !byte " );
        sb.Append( "!byte " );
        for ( int i = 0; i < Length; ++i )
        {
          sb.Append( "$" + Data.ByteAt( ExportStartAddress - DataStartAddress + i ).ToString( "X2" ) );
          if ( i + 1 < Length )
          {
            sb.Append( "," );
          }
        }
        //Debug.Log( outputB );
        sb.Append( "\r\n" );
      }
      return sb.ToString();
    }



    public void SetData( GR.Memory.ByteBuffer Data )
    {
      m_SourceData  = Data;
      m_CoveredData = new bool[Data.Length];
    }



    public bool HasBASICJumpAddress( int DataStartAddress, out int SysAddress )
    {
      SysAddress = 0;
      if ( ( DataStartAddress == 0x0801 )
      ||   ( DataStartAddress == 0x0800 ) )
      {
        // validate basic header, look for jump address
        // 00 08 00 0C 08 0A 00 9E 32 30 36 34 00 00 00
        int dataPos = 0x0801 - DataStartAddress;

        ushort pointerToNextLine  = m_SourceData.UInt16At( dataPos );
        ushort lineNumber         = m_SourceData.UInt16At( dataPos + 2 );
        byte sysInstruction       = m_SourceData.ByteAt( dataPos + 4 );
        if ( sysInstruction != 0x9e )
        {
          // not a SYS command
          return false;
        }
        string sysAddress = "";
        dataPos += 5;
        while ( ( dataPos < m_SourceData.Length )
        &&      ( m_SourceData.ByteAt( dataPos ) != 0 ) )
        {
          sysAddress += m_SourceData.ByteAt( dataPos ).ToString( "X2" );
          ++dataPos;
        }
        GR.Memory.ByteBuffer sysAddrData = new GR.Memory.ByteBuffer();
        if ( !sysAddrData.AppendHex( sysAddress ) )
        {
          return false;
        }
        SysAddress = GR.Convert.ToI32( sysAddrData.ToAsciiString() );    
        return true;
      }
      return false;
    }



    public bool Disassemble( int DataStartAddress, GR.Collections.Set<int> JumpedAtAddresses, GR.Collections.Map<int,string> NamedLabels, DisassemblerSettings Settings, out string Disassembly )
    {
      StringBuilder sb = new StringBuilder();
      Disassembly = "";
      if ( JumpedAtAddresses.Count == 0 )
      {
        return false;
      }

      int progStepPos = JumpedAtAddresses.First;

      GR.Collections.Set<ushort> accessedAddresses = new GR.Collections.Set<ushort>();
      GR.Collections.Set<int>    addressesToCheck = new GR.Collections.Set<int>( JumpedAtAddresses );
      GR.Collections.Set<int>    addressesChecked = new GR.Collections.Set<int>();
      GR.Collections.Set<ushort> probableLabel = new GR.Collections.Set<ushort>();

      // check for basic header
      int sysAddress = -1;

      if ( HasBASICJumpAddress( DataStartAddress, out sysAddress ) )
      {
        progStepPos = sysAddress;
        addressesToCheck.Add( progStepPos );
      }
        /*
      else
      {
        // automatically check at data start address
        addressesToCheck.Add( DataStartAddress );
      }*/

      int     codeStartPos = progStepPos;

      GR.Collections.Map<ushort, GR.Generic.Tupel<Tiny64.Opcode, ushort>> disassembly = new GR.Collections.Map<ushort, GR.Generic.Tupel<Tiny64.Opcode, ushort>>();

      while ( addressesToCheck.Count > 0 )
      {
        progStepPos = addressesToCheck.First;
        //Debug.Log( "check address:" + progStepPos );
        addressesToCheck.Remove( progStepPos );
        if ( addressesChecked.ContainsValue( progStepPos ) )
        {
          continue;
        }
        while ( true )
        {
          if ( progStepPos < DataStartAddress )
          {
            break;
          }
          /*
            sb.Append( "Jumped to address before data\r\n" );
            Disassembly = sb.ToString();
            return false;
          }*/
          if ( progStepPos >= DataStartAddress + m_SourceData.Length )
          {
            // reached the end
            break;
          }

          Tiny64.Opcode  opcode = null;
          bool              outsideData = false;
          if ( !DisassembleInstruction( m_SourceData, DataStartAddress, progStepPos, out opcode, out outsideData ) )
          {
            if ( !outsideData )
            {
              sb.Append( "Failed to disassemble data $" + m_SourceData.ByteAt( progStepPos - DataStartAddress ).ToString( "X2" ) + " at location " + progStepPos + "($" + progStepPos.ToString( "X4" ) + ")\r\n" );
              Disassembly = sb.ToString();
              return false;
            }
          }

          if ( outsideData )
          {
            break;
          }

          addressesChecked.Add( progStepPos );
          //Debug.Log( "Mnemonic: " + OpcodeToString( opcode, Data, progStepPos + 1 - DataStartAddress ) );
          //Debug.Log( progStepPos.ToString( "X4" ) + ": " + MnemonicToString( opcode, Data, DataStartAddress, progStepPos ) );

          if ( ( opcode.ByteValue == 0x4c )     // jmp
          ||   ( opcode.ByteValue == 0x20 ) )   // jsr
          {
            // absolute jump
            accessedAddresses.Add( m_SourceData.UInt16At( progStepPos + 1 - DataStartAddress ) );
            addressesToCheck.Add( m_SourceData.UInt16At( progStepPos + 1 - DataStartAddress ) );
            //Debug.Log( "access address " + Data.UInt16At( progStepPos + 1 ).ToString( "X4" ) );
          }
          else if ( opcode.ByteValue == 0x6c )     // jmp indirect
          {
            probableLabel.Add( m_SourceData.UInt16At( progStepPos + 1 - DataStartAddress ) );
          }
          else if ( opcode.Addressing == Tiny64.Opcode.AddressingType.RELATIVE )
          {
            int targetAddress = (sbyte)m_SourceData.ByteAt( progStepPos + 1 - DataStartAddress ) + 2 + progStepPos;
            probableLabel.Add( (ushort)targetAddress );
            addressesToCheck.Add( targetAddress );
            accessedAddresses.Add( (ushort)targetAddress );
          }

          disassembly[(ushort)progStepPos] = new GR.Generic.Tupel<Tiny64.Opcode, ushort>( opcode, m_SourceData.UInt16At( progStepPos + 1 ) );

          if ( ( opcode.ByteValue == 0x40 )     // rts
          ||   ( opcode.ByteValue == 0x60 )     // rti
          ||   ( opcode.ByteValue == 0x4c ) )   // jmp
          {
            // end of code here
            break;
          }

          //string output = MnemonicToString( opcode, Data, DataStartAddress, progStepPos );
          //Debug.Log( output );
          progStepPos += opcode.NumOperands + 1;
        }
      }

      progStepPos = codeStartPos;
      //foreach ( KeyValuePair<ushort,GR.Generic.Tupel<Opcode, ushort>> instruction in disassembly )

      // remove potential labels that are not in our code
      GR.Collections.Set<ushort>    addressesToRemove = new GR.Collections.Set<ushort>();
      foreach ( var accessedAddress in accessedAddresses )
      {
        if ( !disassembly.ContainsKey( accessedAddress ) )
        {
          addressesToRemove.Add( accessedAddress );
        }
      }
      foreach ( var addressToRemove in addressesToRemove )
      {
        accessedAddresses.Remove( addressToRemove );
      }

      sb.Append( "* = $" );
      sb.AppendLine( DataStartAddress.ToString( "x4" ) );

      if ( !Settings.AddLineAddresses )
      {
        foreach ( var namedLabel in NamedLabels )
        {
          sb.Append( namedLabel.Value );
          sb.Append( " = $" );
          sb.AppendLine( namedLabel.Key.ToString( "X4" ) );
        }
        if ( NamedLabels.Count > 0 )
        {
          sb.AppendLine();
        }
      }

      int     trueAddress = DataStartAddress;
      bool    hadBytes = false;
      int     hadBytesStart = 0;
      while ( trueAddress < DataStartAddress + m_SourceData.Length )
      {
        if ( disassembly.ContainsKey( (ushort)trueAddress ) )
        {
          if ( hadBytes )
          {
            sb.Append( DisassembleBinary( m_SourceData, DataStartAddress, hadBytesStart, trueAddress - hadBytesStart, Settings ) );
            hadBytes = false;
          }
          GR.Generic.Tupel<Tiny64.Opcode, ushort> instruction = disassembly[(ushort)trueAddress];
          if ( Settings.AddLineAddresses )
          {
            sb.Append( "$" );
            sb.Append( trueAddress.ToString( "X4" ) + ": " );
          }

          if ( accessedAddresses.ContainsValue( (ushort)trueAddress ) )
          {
            // line break in front of named label
            sb.AppendLine();
            if ( Settings.AddLineAddresses )
            {
              sb.Append( "$" );
              sb.Append( trueAddress.ToString( "X4" ) + ": " );
            }

            if ( NamedLabels.ContainsKey( trueAddress ) )
            {
              sb.AppendLine( NamedLabels[trueAddress] );
            }
            else
            {
              sb.Append( "label_" + trueAddress.ToString( "x4" ) + "\r\n" );
            }
            if ( Settings.AddLineAddresses )
            {
              sb.Append( "$" );
              sb.Append( trueAddress.ToString( "X4" ) + ": " );
            }
          }
          else if ( NamedLabels.ContainsKey( trueAddress ) )
          {
            // line break in front of named label
            sb.AppendLine();
            if ( Settings.AddLineAddresses )
            {
              sb.Append( "$" );
              sb.Append( trueAddress.ToString( "X4" ) + ": " );
            }

            sb.AppendLine( NamedLabels[trueAddress] );
            if ( Settings.AddLineAddresses )
            {
              sb.Append( "$" );
              sb.Append( trueAddress.ToString( "X4" ) + ": " );
            }
          }

          if ( Settings.AddAssembledBytes )
          {
            sb.Append( " " );
            sb.Append( instruction.first.ByteValue.ToString( "X2" ) );

            switch ( instruction.first.NumOperands )
            {
              case 0:
                sb.Append( "      " );
                break;
              case 1:
                sb.Append( " " );
                sb.Append( m_SourceData.ByteAt( trueAddress + 1 - DataStartAddress ).ToString( "X2" ) );
                sb.Append( "   " );
                break;
              case 2:
                sb.Append( " " );
                sb.Append( m_SourceData.ByteAt( trueAddress + 1 - DataStartAddress ).ToString( "X2" ) );
                sb.Append( " " );
                sb.Append( m_SourceData.ByteAt( trueAddress + 1 - DataStartAddress + 1 ).ToString( "X2" ) );
                break;
            }
          }
          sb.Append( "   " + MnemonicToString( instruction.first, m_SourceData, DataStartAddress, trueAddress, accessedAddresses, NamedLabels ) );
          sb.Append( "\r\n" );
          trueAddress += instruction.first.NumOperands + 1;
        }
        else
        {
          if ( !hadBytes )
          {
            hadBytes = true;
            hadBytesStart = trueAddress;
          }
          ++trueAddress;
        }
      }
      if ( hadBytes )
      {
        sb.Append( DisassembleBinary( m_SourceData, DataStartAddress, hadBytesStart, trueAddress - hadBytesStart, Settings ) );
        hadBytes = false;
      }
      Disassembly = sb.ToString();
      return true;
    }


  }
}
