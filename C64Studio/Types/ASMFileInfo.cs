using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio.Types.ASM
{
  public class Opcode
  {
    public enum AddressingType
    {
      UNKNOWN,
      IMPLICIT,
      IMMEDIATE,
      ABSOLUTE,
      ABSOLUTE_X,
      ABSOLUTE_Y,
      ZEROPAGE,
      ZEROPAGE_X,
      ZEROPAGE_Y,
      INDIRECT,
      INDIRECT_X,
      INDIRECT_Y,
      RELATIVE
    };

    public string Mnemonic = "";
    public int ByteValue = -1;
    public int NumOperands = -1;
    public AddressingType Addressing = AddressingType.UNKNOWN;
    public int NumCycles = 0;
    public int NumPenaltyCycles = 0;
    public int PageBoundaryCycles = 0;
    public int BranchSamePagePenalty = 0;
    public int BranchOtherPagePenalty = 0;

    public Opcode( string Mnemonic, int ByteValue, AddressingType Addressing )
    {
      this.Mnemonic = Mnemonic;
      this.ByteValue = ByteValue;
      NumOperands = 0;
      this.Addressing = Addressing;
    }

    public Opcode( string Mnemonic, int ByteValue, AddressingType Addressing, int NumCycles, int PageBoundaryCycles )
    {
      this.Mnemonic = Mnemonic;
      this.ByteValue = ByteValue;
      NumOperands = 0;
      this.Addressing = Addressing;
      this.NumCycles = NumCycles;
      this.PageBoundaryCycles = PageBoundaryCycles;
      NumPenaltyCycles = PageBoundaryCycles;
    }

    public Opcode( string Mnemonic, int ByteValue, int NumOperands, AddressingType Addressing )
    {
      this.Mnemonic = Mnemonic;
      this.ByteValue = ByteValue;
      this.NumOperands = NumOperands;
      this.Addressing = Addressing;
    }

    public Opcode( string Mnemonic, int ByteValue, int NumOperands, AddressingType Addressing, int NumCycles, int PageBoundaryCycles )
    {
      this.Mnemonic = Mnemonic;
      this.ByteValue = ByteValue;
      this.NumOperands = NumOperands;
      this.Addressing = Addressing;
      this.NumCycles = NumCycles;
      this.PageBoundaryCycles = PageBoundaryCycles;
      NumPenaltyCycles = PageBoundaryCycles;
    }

    public Opcode( string Mnemonic, int ByteValue, int NumOperands, AddressingType Addressing, int NumCycles, int PageBoundaryCycles, int BranchSamePagePenalty, int BranchOtherPagePenalty )
    {
      this.Mnemonic = Mnemonic;
      this.ByteValue = ByteValue;
      this.NumOperands = NumOperands;
      this.Addressing = Addressing;
      this.NumCycles = NumCycles;
      this.PageBoundaryCycles = PageBoundaryCycles;
      this.BranchOtherPagePenalty = BranchOtherPagePenalty;
      this.BranchSamePagePenalty = BranchSamePagePenalty;
      NumPenaltyCycles = PageBoundaryCycles + BranchOtherPagePenalty + BranchSamePagePenalty;
    }
  };



  public class SourceInfo
  {
    public string Filename = "";
    public string FilenameParent = "";
    public string FullPath = "";
    public int    GlobalStartLine = 0;
    public int    LocalStartLine = 0;
    public int    LineCount = 0;
  };



  public class LineInfo
  {
    public string AddressSource = "";
    public int AddressStart = -1;
    public int PseudoPCOffset = -1;    // -1 = not set, -2 at !REALPC, otherwise !PSEUDOPC pos
    public int NumBytes = 0;
    public int LineIndex = -1;
    public string Line = "";
    public string Zone = "";
    public List<Types.TokenInfo> NeededParsedExpression = null;
    public GR.Collections.Map<byte, byte> LineCodeMapping = null;
    public Opcode Opcode = null;
    public GR.Memory.ByteBuffer LineData = null;
  };



  public class UnparsedEvalInfo
  {
    public string Name = "";
    public string ToEval = "";
    public int LineIndex = -1;
    public bool Used = false;
    public string Zone = "";
    public int CharIndex = -1;
    public int Length = 0;
  };



  public class BankInfo
  {
    public int Number = -1;
    public int SizeInBytes = 0;
    public int StartLine = -1;
    public int SizeInBytesStart = 0;
  };



  public class TemporaryLabelInfo
  {
    public string Name = "";
    public int LineIndex = 0;
    public int LineCount = 0;
    public int Value = 0;
    public string Info = "";
    public int CharIndex = -1;
    public int Length = 0;
  };



  public class FileInfo
  {
    public SortedDictionary<int, SourceInfo>      SourceInfo = new SortedDictionary<int, SourceInfo>();
    public Dictionary<int, int>                   AddressToLine = new Dictionary<int, int>();
    public Dictionary<int, LineInfo>              LineInfo = new Dictionary<int, LineInfo>();
    public Dictionary<string, UnparsedEvalInfo>   UnparsedLabels = new Dictionary<string, UnparsedEvalInfo>();
    public Dictionary<string, Types.SymbolInfo>   Labels = new Dictionary<string, Types.SymbolInfo>();
    public Dictionary<string, Types.SymbolInfo>   Zones = new Dictionary<string, Types.SymbolInfo>();
    public List<BankInfo>                         Banks = new List<BankInfo>();
    public List<TemporaryLabelInfo>               TempLabelInfo = new List<TemporaryLabelInfo>();
    public Parser.AssemblerSettings               AssemblerSettings = null;
    public Dictionary<int,Types.Breakpoint>       VirtualBreakpoints = new Dictionary<int,Breakpoint>();
    public string                                 LabelDumpFile = "";



    public void Clear()
    {
      SourceInfo.Clear();
      AddressToLine.Clear();
      LineInfo.Clear();

      Labels.Clear();
      UnparsedLabels.Clear();
      Zones.Clear();
      Banks.Clear();
      TempLabelInfo.Clear();
      VirtualBreakpoints.Clear();
      LabelDumpFile = "";
    }



    public bool ContainsFile( string DocumentFilename )
    {
      foreach ( Types.ASM.SourceInfo sourceInfo in SourceInfo.Values )
      {
        if ( GR.Path.IsPathEqual( sourceInfo.Filename, DocumentFilename ) )
        {
          return true;
        }
      }
      return false;
    }


    public string FindZoneFromDocumentLine( string DocumentFilename, int LineIndex )
    {
      // find global line index
      int     globalLineIndex = 0;

      if ( !FindGlobalLineIndex( LineIndex, DocumentFilename, out globalLineIndex ) )
      {
        return "";
      }

      // work from there
      string zone = "";

      while ( globalLineIndex >= 0 )
      {
        if ( LineInfo.ContainsKey( globalLineIndex ) )
        {
          Types.ASM.LineInfo lineInfo = LineInfo[globalLineIndex];

          zone = lineInfo.Zone;
          break;
        }
        --globalLineIndex;
      }
      return zone;
    }



    public bool FindTrueLineSource( int LineIndex, out string Filename, out int LocalLineIndex )
    {
      Filename = "";
      LocalLineIndex = -1;

      //dh.Log( "FindTrueLineSource for " + LineIndex );
      foreach ( Types.ASM.SourceInfo sourceInfo in SourceInfo.Values )
      {
        if ( ( LineIndex >= sourceInfo.GlobalStartLine )
        &&   ( LineIndex < sourceInfo.GlobalStartLine + sourceInfo.LineCount ) )
        {
          Filename = sourceInfo.Filename;
          //LocalLineIndex = LineIndex + sourceInfo.LocalStartLine - sourceInfo.GlobalStartLine;
          LocalLineIndex = LineIndex + sourceInfo.LocalStartLine - sourceInfo.GlobalStartLine;
          return true;
        }
      }
      //Debug.Log( "FindTrueLineSource for " + LineIndex + " failed" );
      return false;
    }



    public bool FindGlobalLineIndex( int LineIndex, string Filename, out int GlobalLineIndex )
    {
      GlobalLineIndex = -1;
      if ( Filename == null )
      {
        return false;
      }

      //dh.Log( "FindTrueLineSource for " + LineIndex );
      Types.ASM.SourceInfo    lastFound = null;
      foreach ( Types.ASM.SourceInfo sourceInfo in SourceInfo.Values )
      {
        if ( ( GR.Path.IsPathEqual( Filename, sourceInfo.Filename ) ) //( Filename.ToUpper() == sourceInfo.Filename.ToUpper() )
        &&   ( LineIndex >= sourceInfo.LocalStartLine )
        &&   ( LineIndex < sourceInfo.LocalStartLine + sourceInfo.LineCount ) )
        {
          // ugh, outer source info (for nested for loops) is wrong!
          lastFound = sourceInfo;
          //return true;
        }
      }
      //Debug.Log( "FindTrueLineSource for " + LineIndex + " failed" );

      if ( lastFound == null )
      {
        return false;
      }
      GlobalLineIndex = LineIndex + lastFound.GlobalStartLine - lastFound.LocalStartLine;
      return true;
    }



    public void PopulateAddressToLine()
    {
      foreach ( int lineIndex in LineInfo.Keys )
      {
        if ( LineInfo[lineIndex].AddressStart != -1 )
        {
          if ( ( AddressToLine.ContainsKey( LineInfo[lineIndex].AddressStart ) )
          &&   ( lineIndex > AddressToLine[LineInfo[lineIndex].AddressStart] ) )
          {
            AddressToLine[LineInfo[lineIndex].AddressStart] = lineIndex;
          }
          else if ( ( AddressToLine.ContainsKey( LineInfo[lineIndex].AddressStart ) )
          &&        ( LineInfo[lineIndex].NumBytes == 0 ) )
          {
            AddressToLine[LineInfo[lineIndex].AddressStart] = lineIndex;
          }
          else if ( !AddressToLine.ContainsKey( LineInfo[lineIndex].AddressStart ) )
          {
            AddressToLine.Add( LineInfo[lineIndex].AddressStart, lineIndex );
          }
        }
      }
    }



    public int AddressFromToken( string Token )
    {
      if ( !Labels.ContainsKey( Token ) )
      {
        return -1;
      }
      return Labels[Token].AddressOrValue;
    }



    public Types.SymbolInfo TokenInfoFromName( string Token, string Zone )
    {
      if ( AssemblerSettings != null )
      {
        if ( !AssemblerSettings.CaseSensitive )
        {
          Token = Token.ToUpper();
        }
      }
      if ( !Labels.ContainsKey( Token ) )
      {
        if ( Labels.ContainsKey( Zone + Token ) )
        {
          return Labels[Zone + Token];
        }
        return null;
      }
      return Labels[Token];
    }



    public int FindLineAddress( int LineIndex )
    {
      /*
      foreach ( int line in ASMFileInfo.LineInfo.Keys )
      {
        dh.Log( "Line " + line.ToString() + " starts at " + ASMFileInfo.LineInfo[line].AddressStart );
      }*/

      if ( !LineInfo.ContainsKey( LineIndex ) )
      {
        // ugly lower bounds hack
        while ( LineIndex > 0 )
        {
          --LineIndex;
          if ( LineInfo.ContainsKey( LineIndex ) )
          {
            return LineInfo[LineIndex].AddressStart;
          }
        }
        return -1;
      }
      return LineInfo[LineIndex].AddressStart;
    }



    public bool DocumentAndLineFromAddress( int Address, out string DocumentFile, out int DocumentLine )
    {
      DocumentFile = "";
      DocumentLine = -1;
      if ( !AddressToLine.ContainsKey( Address ) )
      {
        return false;
      }
      int globalLine = AddressToLine[Address];

      return FindTrueLineSource( globalLine, out DocumentFile, out DocumentLine );
    }



    public bool IsDocumentPart( string Filename )
    {
      if ( Filename == null )
      {
        return false;
      }
      foreach ( Types.ASM.SourceInfo sourceInfo in SourceInfo.Values )
      {
        if ( GR.Path.IsPathEqual( Filename, sourceInfo.Filename ) )
        //if ( Filename.ToUpper() == sourceInfo.Filename.ToUpper() )
        {
          return true;
        }
      }
      return false;
    }



    public string LabelsAsFile()
    {
      StringBuilder   sb = new StringBuilder();

      foreach ( var token in Labels )
      {
        if ( token.Value.Type == Types.SymbolInfo.Types.LABEL )
        {
          sb.Append( "add_label $" );
          sb.Append( token.Value.AddressOrValue.ToString( "X4" ) );
          sb.Append( " ." );
          sb.AppendLine( token.Key.Replace( '.', '_' ) );
        }
      }
      return sb.ToString();
    }

  }
}
