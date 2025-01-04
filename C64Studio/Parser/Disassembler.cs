using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tiny64;

namespace RetroDevStudio.Parser
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
        Debug.Log( "disassembler code outside of data" );
        NotComplete = true;
        return false;
      }
      byte instruction = Data.ByteAt( CodeStartAddress - DataStartAddress );
      if ( !m_Processor.OpcodeByValue.ContainsKey( instruction ) )
      {
        Debug.Log( "disassembler unknown opcode byte" );
        return false;
      }
      opcode = m_Processor.OpcodeByValue[instruction];
      if ( CodeStartAddress + opcode.OpcodeSize >= DataStartAddress + Data.Length )
      {
        // opcode with operands does not fit
        Debug.Log( "disassembler code operands outside of data" );
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
      bool    isImmediate = false;

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
        case Tiny64.Opcode.AddressingType.IMMEDIATE_ACCU:
        case Tiny64.Opcode.AddressingType.IMMEDIATE_REGISTER:
          targetAddress = Data.ByteAt( CodePos + 1 - DataStartAddress );
          twoBytes = false;
          isImmediate = true;
          break;
        case Tiny64.Opcode.AddressingType.INDIRECT:
          targetAddress = Data.UInt16At( CodePos + 1 - DataStartAddress );
          break;
        case Tiny64.Opcode.AddressingType.ZEROPAGE_INDIRECT_X:
          targetAddress = Data.ByteAt( CodePos + 1 - DataStartAddress );
          twoBytes = false;
          break;
        case Tiny64.Opcode.AddressingType.ZEROPAGE_INDIRECT_Y:
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
        default:
          Debug.Log( $"Unsupported addressing {opcode.Addressing}" );
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

      if ( !isImmediate )
      {
        if ( AccessedAddresses.ContainsValue( targetAddress ) )
        {
          addressPlacement = "label_" + targetAddress.ToString( "x4" );
        }

        if ( NamedLabels.ContainsKey( targetAddress ) )
        {
          addressPlacement = NamedLabels[targetAddress];
        }
        else if ( NamedLabels.ContainsKey( targetAddress - 1 ) )
        {
          addressPlacement = NamedLabels[targetAddress - 1] + "+1";
        }
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
        case Tiny64.Opcode.AddressingType.IMMEDIATE_ACCU:
        case Tiny64.Opcode.AddressingType.IMMEDIATE_REGISTER:
          output += " #" + addressPlacement;
          break;
        case Tiny64.Opcode.AddressingType.INDIRECT:
          output += " ( " + addressPlacement + " )";
          break;
        case Tiny64.Opcode.AddressingType.ZEROPAGE_INDIRECT_X:
          output += " ( " + addressPlacement + ", x)";
          break;
        case Tiny64.Opcode.AddressingType.ZEROPAGE_INDIRECT_Y:
          output += " ( " + addressPlacement + " ), y";
          break;
        case Tiny64.Opcode.AddressingType.RELATIVE:
          output += " " + addressPlacement;
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
        default:
          Debug.Log( $"Unsupported addressing {opcode.Addressing}" );
          break;
      }
      return output;
    }



    public string DisassembleBinary( GR.Memory.ByteBuffer Data, int DataStartAddress, int ExportStartAddress, int Length, DisassemblerSettings Settings )
    {
      StringBuilder sb = new StringBuilder();

      int wrapSize = 8;
      bool firstLine = true;

      while ( Length >= wrapSize )
      {
        if ( !firstLine )
        {
          AddLineAddress( Settings, sb, ExportStartAddress );
        }
        firstLine = false;

        sb.Append( "!byte " );
        for ( int i = 0; i < wrapSize; ++i )
        {
          sb.Append( "$" + Data.ByteAt( ExportStartAddress - DataStartAddress + i ).ToString( "X2" ) );
          if ( i + 1 < wrapSize )
          {
            sb.Append( "," );
          }
        }
        sb.Append( "\r\n" );
        ExportStartAddress += wrapSize;
        Length -= wrapSize;
      }
      if ( Length > 0 )
      {
        if ( !firstLine )
        {
          AddLineAddress( Settings, sb, ExportStartAddress );
        }
        sb.Append( "!byte " );
        for ( int i = 0; i < Length; ++i )
        {
          sb.Append( "$" + Data.ByteAt( ExportStartAddress - DataStartAddress + i ).ToString( "X2" ) );
          if ( i + 1 < Length )
          {
            sb.Append( "," );
          }
        }
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



    private void AddLineAddress( DisassemblerSettings Settings, StringBuilder sb, int trueAddress )
    {
      if ( Settings.AddLineAddresses )
      {
        sb.Append( "$" );
        sb.Append( trueAddress.ToString( "X4" ) + ": " );
      }
    }


    public bool Disassemble( int DataStartAddress, GR.Collections.Set<int> JumpedAtAddresses, GR.Collections.Map<int,string> NamedLabels, DisassemblerSettings Settings, out string Disassembly, out int FirstLineWithOpcode )
    {
      StringBuilder sb = new StringBuilder();
      Disassembly = "";
      FirstLineWithOpcode = 1;
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

      int     codeStartPos = progStepPos;

      GR.Collections.Map<ushort, GR.Generic.Tupel<Tiny64.Opcode, ushort>> disassembly = new GR.Collections.Map<ushort, GR.Generic.Tupel<Tiny64.Opcode, ushort>>();

      while ( addressesToCheck.Count > 0 )
      {
        progStepPos = addressesToCheck.First;
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

          if ( ( opcode.ByteValue == 0x4c )     // jmp
          ||   ( opcode.ByteValue == 0x20 ) )   // jsr
          {
            // absolute jump
            accessedAddresses.Add( m_SourceData.UInt16At( progStepPos + 1 - DataStartAddress ) );
            addressesToCheck.Add( m_SourceData.UInt16At( progStepPos + 1 - DataStartAddress ) );
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

          if ( Settings.StopAtReturns )
          {
            if ( IsReturnOpcode( opcode ) )
            {
              // end of code here
              break;
            }
          }

          progStepPos += opcode.OpcodeSize + 1;
        }
      }

      progStepPos = codeStartPos;

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

      var appearingLabels = new GR.Collections.Set<string>();
      var appearingLabelsPlusOne = new GR.Collections.Set<string>();
      int     trueAddress = DataStartAddress;
      while ( trueAddress < DataStartAddress + m_SourceData.Length )
      {
        var namedLabels = NamedLabels.Where( nl => nl.Key == trueAddress );
        appearingLabels.AddRange( namedLabels.Select( nl => nl.Value ) );

        // special, allow + 1
        namedLabels = NamedLabels.Where( nl => nl.Key == trueAddress + 1 );
        appearingLabelsPlusOne.AddRange( namedLabels.Select( nl => nl.Value ) );

        ++trueAddress;
      }

      // TODO - skip labels that are set inside the disassembly below
      bool addedNamedLabel = false;
      foreach ( var namedLabel in NamedLabels )
      {
        if ( ( appearingLabels.ContainsValue( namedLabel.Value ) )
        ||   ( appearingLabelsPlusOne.ContainsValue( namedLabel.Value ) ) )
        {
          continue;
        }
        addedNamedLabel = true;
        sb.Append( namedLabel.Value );
        sb.Append( " = $" );
        sb.AppendLine( namedLabel.Key.ToString( "X4" ) );
      }
      if ( addedNamedLabel )
      {
        sb.AppendLine();
      }

      trueAddress = DataStartAddress;
      bool    hadBytes = false;
      int     hadBytesStart = 0;
      int     localLineIndex = 1;
      while ( trueAddress < DataStartAddress + m_SourceData.Length )
      {
        if ( ( disassembly.ContainsKey( (ushort)trueAddress ) )
        ||   ( NamedLabels.Any( nl => nl.Key == trueAddress ) )
        ||   ( NamedLabels.Any( nl => nl.Key == trueAddress + 1 ) ) )
        {
          if ( hadBytes )
          {
            AddLineAddress( Settings, sb, trueAddress );

            sb.Append( DisassembleBinary( m_SourceData, DataStartAddress, hadBytesStart, trueAddress - hadBytesStart, Settings ) );
            hadBytes = false;
          }

          GR.Generic.Tupel<Tiny64.Opcode, ushort> instruction = null;

          if ( disassembly.ContainsKey( (ushort)trueAddress ) )
          {
            instruction = disassembly[(ushort)trueAddress];
          }

          if ( DataStartAddress == trueAddress )
          {
            FirstLineWithOpcode = localLineIndex;
          }
          ++localLineIndex;

          if ( accessedAddresses.ContainsValue( (ushort)trueAddress ) )
          {
            AddLineAddress( Settings, sb, trueAddress );

            // line break in front of named label
            sb.AppendLine();
            AddLineAddress( Settings, sb, trueAddress );

            if ( NamedLabels.ContainsKey( trueAddress ) )
            {
              sb.AppendLine( NamedLabels[trueAddress] );
            }
            else if ( NamedLabels.ContainsKey( trueAddress - 1 ) )
            {
              sb.Append( NamedLabels[trueAddress - 1] );
              sb.AppendLine( "+1" );
            }
            else
            {
              sb.AppendLine( "label_" + trueAddress.ToString( "x4" ) );
            }
          }
          else if ( NamedLabels.ContainsKey( trueAddress ) )
          {
            AddLineAddress( Settings, sb, trueAddress );

            // line break in front of named label
            sb.AppendLine();
            AddLineAddress( Settings, sb, trueAddress );

            sb.AppendLine( NamedLabels[trueAddress] );
          }

          if ( instruction != null )
          {
            AddLineAddress( Settings, sb, trueAddress );
          }

          if ( ( instruction != null )
          &&   ( instruction.first.OpcodeSize > 0 ) )
          {
            // is there a label jumping in the middle of the next opcode?
            var  namedLabelInside = NamedLabels.Where( l => ( l.Key > trueAddress ) && ( l.Key < trueAddress + 1 + instruction.first.OpcodeSize ) );
            foreach ( var addressInside in namedLabelInside )
            {
              sb.AppendLine();
              AddLineAddress( Settings, sb, trueAddress );

              sb.Append( addressInside.Value );
              sb.Append( " = * + " );
              sb.Append( addressInside.Key - trueAddress );
              sb.AppendLine();
            }

            var  addressesInside = accessedAddresses.Where( l => ( l > trueAddress ) && ( l < trueAddress + 1 + instruction.first.OpcodeSize ) );
            foreach ( var addressInside in addressesInside )
            {
              sb.AppendLine();
              AddLineAddress( Settings, sb, trueAddress );

              sb.Append( "label_" + addressInside.ToString( "x4" ) );
              sb.Append( " = * + " );
              sb.Append( addressInside - trueAddress );
              sb.AppendLine();
            }
          }

          if ( ( Settings.AddAssembledBytes )
          &&   ( instruction != null ) )
          {
            sb.Append( " " );
            sb.Append( instruction.first.ByteValue.ToString( "X2" ) );

            switch ( instruction.first.OpcodeSize )
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
          if ( instruction != null )
          {
            sb.Append( "   " + MnemonicToString( instruction.first, m_SourceData, DataStartAddress, trueAddress, accessedAddresses, NamedLabels ) );
            sb.Append( "\r\n" );
            trueAddress += instruction.first.OpcodeSize + 1;
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
        AddLineAddress( Settings, sb, hadBytesStart );
        sb.Append( DisassembleBinary( m_SourceData, DataStartAddress, hadBytesStart, trueAddress - hadBytesStart, Settings ) );
        hadBytes = false;
      }
      Disassembly = sb.ToString();
      return true;
    }



    private bool IsReturnOpcode( Opcode Opcode )
    {
      if ( ( Opcode.ByteValue == 0x40 )     // rts
      ||   ( Opcode.ByteValue == 0x60 )     // rti
      ||   ( Opcode.ByteValue == 0x4c ) )   // jmp
      {
        return true;
      }
      return false;
    }



  }
}
