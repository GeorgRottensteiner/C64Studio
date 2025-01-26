using GR.Memory;
using RetroDevStudio.Formats;
using RetroDevStudio.Parser;
using RetroDevStudio.Types;
using RetroDevStudio.Types.ASM;
using System;
using System.Collections.Generic;



namespace RetroDevStudio.Parser
{
  public partial class ASMFileParser : ParserBase
  {
    private ByteBuffer AssembleTarget( CompileTarget Target, ByteBuffer Assembly, string OutputPureFilename, int FileStartAddress )
    {
      switch ( Target.Type )
      {
        case CompileTargetType.PRG:
        case CompileTargetType.PLAIN:
        case CompileTargetType.NONE:
          return Assembly;
        case Types.CompileTargetType.T64:
          {
            Formats.T64 t64 = new RetroDevStudio.Formats.T64();

            Formats.T64.FileRecord  record = new RetroDevStudio.Formats.T64.FileRecord();

            record.Filename       = Util.ToFilename( Formats.MediaFilenameType.COMMODORE, OutputPureFilename );
            record.StartAddress   = (ushort)FileStartAddress;
            record.FileTypeNative = FileTypeNative.COMMODORE_PRG;
            record.EntryType      = 1;

            t64.TapeInfo.Description = "C64S tape file\r\nDemo tape";
            t64.TapeInfo.UserDescription = "USERDESC";
            t64.FileRecords.Add( record );
            t64.FileDatas.Add( Assembly.SubBuffer( 2 ) );

            return t64.Compile();
          }
        case Types.CompileTargetType.TAP:
          {
            Formats.Tap tap = new RetroDevStudio.Formats.Tap();

            tap.WriteFile( Util.ToFilename( Formats.MediaFilenameType.COMMODORE, OutputPureFilename ), Assembly, FileTypeNative.COMMODORE_PRG );
            return tap.Compile();
          }
        case Types.CompileTargetType.D64:
          {
            Formats.D64 d64 = new RetroDevStudio.Formats.D64();

            d64.CreateEmptyMedia();

            GR.Memory.ByteBuffer    bufName = Util.ToFilename( Formats.MediaFilenameType.COMMODORE, OutputPureFilename );
            d64.WriteFile( bufName, Assembly, FileTypeNative.COMMODORE_PRG );

            return d64.Compile();
          }
        case Types.CompileTargetType.DSK:
          {
            var disk = new RetroDevStudio.Formats.CPCDSK();

            disk.CreateEmptyMedia();

            GR.Memory.ByteBuffer    bufName = Util.ToFilename( Formats.MediaFilenameType.CPC, OutputPureFilename );
            disk.WriteFile( bufName, Assembly, FileTypeNative.COMMODORE_PRG );

            return disk.Compile();
          }
        case Types.CompileTargetType.D81:
          {
            Formats.D81 d81 = new RetroDevStudio.Formats.D81();

            d81.CreateEmptyMedia();

            GR.Memory.ByteBuffer    bufName = Util.ToFilename( Formats.MediaFilenameType.COMMODORE, OutputPureFilename );
            d81.WriteFile( bufName, Assembly, FileTypeNative.COMMODORE_PRG );

            return d81.Compile();
          }
        case Types.CompileTargetType.CARTRIDGE_NES:
          {
            var inesHeader = new ByteBuffer( "4E45531A000000000000000000000000" );

            inesHeader.SetU8At( 4, m_CompileTarget.NESPrgROMUnits16k );
            inesHeader.SetU8At( 5, m_CompileTarget.NESChrROMUnits8k );
            inesHeader.SetU8At( 6, (byte)( ( m_CompileTarget.NESMapper << 4 ) | ( m_CompileTarget.NESMirroring & 0x0f ) ) );
            inesHeader.SetU8At( 7, (byte)( m_CompileTarget.NESMapper & 0xf0 ) );

            return inesHeader + Assembly;
          }
        case Types.CompileTargetType.CARTRIDGE_8K_BIN:
        case Types.CompileTargetType.CARTRIDGE_8K_CRT:
          {
            if ( !ValidateAssemblySize( Assembly, 8192, out ByteBuffer resultingAssembly ) )
            {
              return null;
            }

            if ( Target.Type == Types.CompileTargetType.CARTRIDGE_8K_CRT )
            {
              // build cartridge header
              resultingAssembly = CompileCartridgeHeader( 0, 0, 0, m_CompileTargetFile ) 
                                + CompileCartridgeChips( resultingAssembly, 0, 0x8000, 0x2000 );
            }
            return resultingAssembly;
          }
        case Types.CompileTargetType.CARTRIDGE_16K_BIN:
        case Types.CompileTargetType.CARTRIDGE_16K_CRT:
          {
            if ( !ValidateAssemblySize( Assembly, 16384, out ByteBuffer resultingAssembly ) )
            {
              return null;
            }
            if ( Target.Type == Types.CompileTargetType.CARTRIDGE_16K_CRT )
            {
              var header = CompileCartridgeHeader( 0, 0, 0, m_CompileTargetFile );
              resultingAssembly = CompileCartridgeHeader( 0, 0, 0, m_CompileTargetFile )
                                + CompileCartridgeChips( resultingAssembly, 0, 0x8000, 0x4000 );
            }
            return resultingAssembly;
          }
        case Types.CompileTargetType.CARTRIDGE_ULTIMAX_4K_BIN:
        case Types.CompileTargetType.CARTRIDGE_ULTIMAX_4K_CRT:
          {
            if ( !ValidateAssemblySize( Assembly, 4096, out ByteBuffer resultingAssembly ) )
            {
              return null;
            }
            if ( Target.Type == Types.CompileTargetType.CARTRIDGE_ULTIMAX_4K_CRT )
            {
              resultingAssembly = CompileCartridgeHeader( 0, 1, 0, m_CompileTargetFile )
                                + CompileCartridgeChips( resultingAssembly, 0, 0xF000, 0x1000 );
            }
            return resultingAssembly;
          }
        case Types.CompileTargetType.CARTRIDGE_ULTIMAX_8K_BIN:
        case Types.CompileTargetType.CARTRIDGE_ULTIMAX_8K_CRT:
          {
            if ( !ValidateAssemblySize( Assembly, 8192, out ByteBuffer resultingAssembly ) )
            {
              return null;
            }
            if ( Target.Type == Types.CompileTargetType.CARTRIDGE_ULTIMAX_8K_CRT )
            {
              resultingAssembly = CompileCartridgeHeader( 0, 1, 0, m_CompileTargetFile )
                                + CompileCartridgeChips( resultingAssembly, 0, 0xE000, 0x2000 );
            }
            return resultingAssembly;
          }
        case Types.CompileTargetType.CARTRIDGE_ULTIMAX_16K_BIN:
        case Types.CompileTargetType.CARTRIDGE_ULTIMAX_16K_CRT:
          {
            if ( !ValidateAssemblySize( Assembly, 16384, out ByteBuffer resultingAssembly ) )
            {
              return null;
            }
            if ( Target.Type == Types.CompileTargetType.CARTRIDGE_ULTIMAX_16K_CRT )
            {
              resultingAssembly = CompileCartridgeHeader( 0, 1, 0, m_CompileTargetFile )
                                + CompileCartridgeChips( resultingAssembly, 0, 0x8000, 0xe000, 0x2000, true );
            }
            return resultingAssembly;
          }
        case Types.CompileTargetType.CARTRIDGE_MAGICDESK_BIN_32K:
        case Types.CompileTargetType.CARTRIDGE_MAGICDESK_CRT_32K:
          {
            if ( !ValidateAssemblySize( Assembly, 32768, out ByteBuffer resultingAssembly ) )
            {
              return null;
            }
            if ( Target.Type == Types.CompileTargetType.CARTRIDGE_MAGICDESK_CRT_32K )
            {
              resultingAssembly = CompileCartridgeHeader( 0x13, 0, 1, m_CompileTargetFile )
                                + CompileCartridgeChips( resultingAssembly, 0, 0x8000, 0x2000 );
            }
            return resultingAssembly;
          }
        case Types.CompileTargetType.CARTRIDGE_MAGICDESK_BIN_64K:
        case Types.CompileTargetType.CARTRIDGE_MAGICDESK_CRT_64K:
          {
            if ( !ValidateAssemblySize( Assembly, 65536, out ByteBuffer resultingAssembly ) )
            {
              return null;
            }
            if ( Target.Type == Types.CompileTargetType.CARTRIDGE_MAGICDESK_CRT_64K )
            {
              resultingAssembly = CompileCartridgeHeader( 0x13, 0, 1, m_CompileTargetFile )
                                + CompileCartridgeChips( resultingAssembly, 0, 0x8000, 0x2000 );
            }
            return resultingAssembly;
          }
        case Types.CompileTargetType.CARTRIDGE_MAGICDESK_BIN_128K:
        case Types.CompileTargetType.CARTRIDGE_MAGICDESK_CRT_128K:
          {
            if ( !ValidateAssemblySize( Assembly, 65536 * 2, out ByteBuffer resultingAssembly ) )
            {
              return null;
            }
            if ( Target.Type == Types.CompileTargetType.CARTRIDGE_MAGICDESK_CRT_128K )
            {
              resultingAssembly = CompileCartridgeHeader( 0x13, 0, 1, m_CompileTargetFile )
                                + CompileCartridgeChips( resultingAssembly, 0, 0x8000, 0x2000 );
            }
            return resultingAssembly;
          }
        case Types.CompileTargetType.CARTRIDGE_MAGICDESK_BIN_256K:
        case Types.CompileTargetType.CARTRIDGE_MAGICDESK_CRT_256K:
          {
            if ( !ValidateAssemblySize( Assembly, 65536 * 4, out ByteBuffer resultingAssembly ) )
            {
              return null;
            }
            if ( Target.Type == Types.CompileTargetType.CARTRIDGE_MAGICDESK_CRT_256K )
            {
              resultingAssembly = CompileCartridgeHeader( 0x13, 0, 1, m_CompileTargetFile )
                                + CompileCartridgeChips( resultingAssembly, 0, 0x8000, 0x2000 );
            }
            return resultingAssembly;
          }
        case Types.CompileTargetType.CARTRIDGE_MAGICDESK_BIN_512K:
        case Types.CompileTargetType.CARTRIDGE_MAGICDESK_CRT_512K:
          {
            if ( !ValidateAssemblySize( Assembly, 65536 * 8, out ByteBuffer resultingAssembly ) )
            {
              return null;
            }
            if ( Target.Type == Types.CompileTargetType.CARTRIDGE_MAGICDESK_CRT_512K )
            {
              resultingAssembly = CompileCartridgeHeader( 0x13, 0, 1, m_CompileTargetFile )
                                + CompileCartridgeChips( resultingAssembly, 0, 0x8000, 0x2000 );
            }
            return resultingAssembly;
          }
        case Types.CompileTargetType.CARTRIDGE_MAGICDESK_BIN_1M:
        case Types.CompileTargetType.CARTRIDGE_MAGICDESK_CRT_1M:
          {
            if ( !ValidateAssemblySize( Assembly, 65536 * 16, out ByteBuffer resultingAssembly ) )
            {
              return null;
            }
            if ( Target.Type == Types.CompileTargetType.CARTRIDGE_MAGICDESK_CRT_1M )
            {
              resultingAssembly = CompileCartridgeHeader( 0x13, 0, 1, m_CompileTargetFile )
                                + CompileCartridgeChips( resultingAssembly, 0, 0x8000, 0x2000 );
            }
            return resultingAssembly;
          }
        case Types.CompileTargetType.CARTRIDGE_RGCD_BIN:
        case Types.CompileTargetType.CARTRIDGE_RGCD_CRT:
          {
            if ( !ValidateAssemblySize( Assembly, 65536, out ByteBuffer resultingAssembly ) )
            {
              return null;
            }
            if ( Target.Type == Types.CompileTargetType.CARTRIDGE_RGCD_CRT )
            {
              resultingAssembly = CompileCartridgeHeader( 0x39, 0, 1, m_CompileTargetFile )
                                + CompileCartridgeChips( resultingAssembly, 0, 0x8000, 0x2000 );
            }
            return resultingAssembly;
          }
        case Types.CompileTargetType.CARTRIDGE_EASYFLASH_BIN:
        case Types.CompileTargetType.CARTRIDGE_EASYFLASH_CRT:
          {
            if ( !ValidateAssemblySize( Assembly, 2 * 524288, out ByteBuffer resultingAssembly ) )
            {
              return null;
            }
            if ( Target.Type == Types.CompileTargetType.CARTRIDGE_EASYFLASH_CRT )
            {
              resultingAssembly = CompileCartridgeHeader( 0x20, 1, 0, m_CompileTargetFile )
                                + CompileCartridgeChips( resultingAssembly, 2, 0x8000, 0xa000, 0x2000, false );
            }
            return resultingAssembly;
          }
        case Types.CompileTargetType.CARTRIDGE_GMOD2_BIN:
        case Types.CompileTargetType.CARTRIDGE_GMOD2_CRT:
          {
            if ( !ValidateAssemblySize( Assembly, 524288, out ByteBuffer resultingAssembly ) )
            {
              return null;
            }
            if ( Target.Type == Types.CompileTargetType.CARTRIDGE_GMOD2_CRT )
            {
              resultingAssembly = CompileCartridgeHeader( 60, 0, 1, m_CompileTargetFile )
                                + CompileCartridgeChips( resultingAssembly, 0, 0x8000, 0x2000 );
            }
            return resultingAssembly;
          }
        default:
          Debug.Log( $"Unsupported target type {Target.Type} in AssembleTarget encountered" );
          break;
      }
      return null;
    }



    private ByteBuffer CompileCartridgeChips( ByteBuffer ResultingAssembly, ushort ROMType, ushort StartAddress, ushort SliceSize )
    {
      var allChips = new ByteBuffer();
      for ( int i = 0; i < ResultingAssembly.Length / SliceSize; ++i )
      {
        var chip = new GR.Memory.ByteBuffer();

        chip.AppendHex( "43484950" );   // chip
        uint length = (uint)( 16 + SliceSize );
        chip.AppendU32NetworkOrder( length );
        // 0 = ROM, 2 = flash ROM
        chip.AppendU16( ROMType );
        chip.AppendU16NetworkOrder( (ushort)i );  // Bank number
        chip.AppendU16NetworkOrder( StartAddress ); // loading start address
        chip.AppendU16NetworkOrder( SliceSize ); // rom size

        chip.Append( ResultingAssembly.SubBuffer( i * SliceSize, SliceSize ) );

        allChips += chip;
      }
      return allChips;
    }



    private ByteBuffer CompileCartridgeChips( ByteBuffer ResultingAssembly, ushort ROMType, ushort StartAddress, ushort SecondaryStartAddress, ushort SliceSize, bool UltimaxCartridge )
    {
      var allChips = new ByteBuffer();
      for ( int i = 0; i < ResultingAssembly.Length / SliceSize; ++i )
      {
        var chip = new GR.Memory.ByteBuffer();

        chip.AppendHex( "43484950" );   // chip
        uint length = (uint)( 16 + SliceSize );
        chip.AppendU32NetworkOrder( length );
        // 0 = ROM, 2 = flash ROM
        chip.AppendU16NetworkOrder( ROMType );
        // Bank number
        if ( UltimaxCartridge )
        {
          chip.AppendU16NetworkOrder( (ushort)i );
        }
        else
        {
          chip.AppendU16NetworkOrder( (ushort)( i / 2 ) );
        }
        if ( ( i % 2 ) == 0 )
        {
          chip.AppendU16NetworkOrder( StartAddress );
        }
        else
        {
          chip.AppendU16NetworkOrder( SecondaryStartAddress );
        }
        chip.AppendU16NetworkOrder( SliceSize );

        chip.Append( ResultingAssembly.SubBuffer( i * SliceSize, SliceSize ) );

        allChips += chip;
      }
      return allChips;
    }



    private ByteBuffer CompileCartridgeHeader( ushort CartridgeType, byte ExRom, byte Game, string CompileTargetFile )
    {
      var header = new GR.Memory.ByteBuffer();

      header.AppendHex( "43363420434152545249444745202020" ); // "C64 CARTRIDGE   "
      header.AppendU32NetworkOrder( 0x40 );                   // file header length
      header.AppendU16NetworkOrder( 0x0100 );                 // version (currently only 1.00)
      header.AppendU16NetworkOrder( CartridgeType );
      header.AppendU8( ExRom );
      header.AppendU8( Game );

      // reserved
      header.AppendU8( 0 );
      header.AppendU8( 0 );
      header.AppendU8( 0 );
      header.AppendU8( 0 );
      header.AppendU8( 0 );
      header.AppendU8( 0 );

      // cartridge name
      string name = GR.Path.GetFileNameWithoutExtension( CompileTargetFile ).ToUpper();

      if ( name.Length > 32 )
      {
        name = name.Substring( 0, 32 );
      }
      while ( name.Length < 32 )
      {
        name += (char)0;
      }
      foreach ( char aChar in name )
      {
        header.AppendU8( (byte)aChar );
      }
      return header;
    }



    private bool ValidateAssemblySize( ByteBuffer Assembly, int RequiredSize, out ByteBuffer resultingAssembly )
    {
      resultingAssembly = Assembly;
      if ( resultingAssembly.Length < RequiredSize )
      {
        // fill up
        resultingAssembly += new GR.Memory.ByteBuffer( (uint)( RequiredSize - resultingAssembly.Length ) );
      }
      else if ( resultingAssembly.Length > RequiredSize )
      {
        AddError( 0, Types.ErrorCode.E1102_PROGRAM_TOO_LARGE, $"Assembly too large, {resultingAssembly.Length} > {RequiredSize}" );
        return false;
      }
      return true;
    }



  }
}
