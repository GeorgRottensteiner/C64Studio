using GR.Collections;
using GR.Generic;
using RetroDevStudio;
using RetroDevStudio.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RetroDevStudio.Types.ASM
{
  public enum LabelFileFormat
  {
    VICE          = 0,
    C64DEBUGGER   = 1
  };

  public class SourceInfo
  {
    public enum SourceInfoSource
    {
      CODE_DIRECT = 0,
      CODE_INCLUDE,
      MEDIA_INCLUDE,
      MACRO
    }

    public string             Filename = "";
    public string             FilenameParent = "";
    public string             FullPath = "";
    public int                GlobalStartLine = 0;
    public int                LocalStartLine = 0;
    public int                LineCount = 0;
    public SourceInfoSource   Source = SourceInfoSource.CODE_DIRECT;
  };



  public class LineInfo
  {
    [Flags]
    public enum LineFlags
    {
      // used for 65816 modes
      Accu16Bit                 = 0x00000001,
      Registers16Bit            = 0x00000002,

      OpcodeUsingLongMode       = 0x00000004,
      HasCollapsedContent       = 0x00000008,
      HideInPreprocessedOutput  = 0x00000010
    }

    public string                 AddressSource = "";
    public int                    AddressStart = -1;
    public int                    PseudoPCOffset = -1;    // -1 = not set, -2 at !REALPC, otherwise !PSEUDOPC pos
    public int                    NumBytes = 0;
    public int                    LineIndex = -1;
    public int                    LineOffsetInFront = 0;
    public string                 Line = "";
    public string                 Zone = "";
    public string                 CheapLabelZone = "";
    public List<Types.TokenInfo>  NeededParsedExpression = null;
    public List<Types.TokenInfo>  NeededParsedExpression2 = null;
    public GR.Collections.Map<byte, byte> LineCodeMapping = null;
    public Tiny64.Opcode          Opcode = null;
    public GR.Memory.ByteBuffer   LineData = null;
    public LineFlags              Flags = 0;
    public int                    CheckSum = -1;



    public bool HideInPreprocessedOutput
    {
      get
      {
        return ( Flags & LineFlags.HideInPreprocessedOutput ) != 0;
      }
      set
      {
        Flags &= ~LineFlags.HideInPreprocessedOutput;
        if ( value )
        {
          Flags |= LineFlags.HideInPreprocessedOutput;
        }
      }
    }

    public bool Accu16Bit
    {
      get
      {
        return ( Flags & LineFlags.Accu16Bit ) != 0;
      }
      set
      {
        Flags &= ~LineFlags.Accu16Bit;
        if ( value )
        {
          Flags |= LineFlags.Accu16Bit;
        }
      }
    }
    public bool Registers16Bit
    {
      get
      {
        return ( Flags & LineFlags.Registers16Bit ) != 0;
      }
      set
      {
        Flags &= ~LineFlags.Registers16Bit;
        if ( value )
        {
          Flags |= LineFlags.Registers16Bit;
        }
      }
    }
    public bool HasCollapsedContent
    {
      get
      {
        return ( Flags & LineFlags.HasCollapsedContent ) != 0;
      }
      set
      {
        Flags &= ~LineFlags.HasCollapsedContent;
        if ( value )
        {
          Flags |= LineFlags.HasCollapsedContent;
        }
      }
    }
    public bool OpcodeUsingLongMode
    {
      get
      {
        return ( Flags & LineFlags.OpcodeUsingLongMode ) != 0;
      }
      set
      {
        Flags &= ~LineFlags.OpcodeUsingLongMode;
        if ( value )
        {
          Flags |= LineFlags.OpcodeUsingLongMode;
        }
      }
    }
  }


  public class UnparsedEvalInfo
  {
    public string Name = "";
    public string ToEval = "";
    public int    LineIndex = -1;
    public bool   Used = false;
    public string Zone = "";
    public int    CharIndex = -1;
    public int    Length = 0;
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
    public string Info = "";
    public int CharIndex = -1;
    public int Length = 0;
    public SymbolInfo   Symbol = null;
    public bool IsForVariable = false;
  };



  public class FileInfo
  {
    public SortedDictionary<int, SourceInfo>      SourceInfo = new SortedDictionary<int, SourceInfo>();
    public Dictionary<int, int>                   AddressToLine = new Dictionary<int, int>();
    public Dictionary<int, LineInfo>              LineInfo = new Dictionary<int, LineInfo>();
    public Dictionary<string, UnparsedEvalInfo>   UnparsedLabels = new Dictionary<string, UnparsedEvalInfo>();
    public Dictionary<string, SymbolInfo>         Labels = new Dictionary<string, SymbolInfo>();
    public Dictionary<string, List<SymbolInfo>>   Zones = new Dictionary<string, List<SymbolInfo>>();

    // used for BASIC, 2-char-name mapped to original name
    public Dictionary<string, List<SymbolInfo>>   MappedVariables = new Dictionary<string, List<SymbolInfo>>();
    public Set<string>                            OriginalVariables = new Set<string>();

    public List<BankInfo>                         Banks = new List<BankInfo>();
    public List<TemporaryLabelInfo>               TempLabelInfo = new List<TemporaryLabelInfo>();
    public Parser.AssemblerSettings               AssemblerSettings = new AssemblerSettings();
    public Dictionary<int,Types.Breakpoint>       VirtualBreakpoints = new Dictionary<int,Breakpoint>();
    public LabelDumpSettings                      LabelDumpSettings = new LabelDumpSettings();
    public Tiny64.Processor                       Processor = Tiny64.Processor.Create6510();
    public GR.Collections.Map<Tupel<string,int>, Types.MacroFunctionInfo>    Macros = new Map<Tupel<string, int>, MacroFunctionInfo>();

    public List<int>                              FixedBreakpoints = new List<int>();

    public GR.Collections.MultiMap<int, Parser.ParserBase.ParseMessage>   Messages = new GR.Collections.MultiMap<int, Parser.ParserBase.ParseMessage>();

    public List<Types.AutoCompleteItemInfo>       _KnownTokens = new List<AutoCompleteItemInfo>();
    public GR.Collections.MultiMap<string, SymbolInfo> _KnownTokenInfo = new GR.Collections.MultiMap<string, SymbolInfo>();



    public FileInfo()
    {
      AssemblerSettings.SetAssemblerType( Types.AssemblerType.C64_STUDIO );
    }



    /*
    public FileInfo( FileInfo OtherInfo )
    {
      SourceInfo      = new SortedDictionary<int, SourceInfo>( OtherInfo.SourceInfo );
      AddressToLine   = new Dictionary<int, int>( OtherInfo.AddressToLine );
      LineInfo        = new Dictionary<int, LineInfo>( OtherInfo.LineInfo );
      UnparsedLabels  = new Dictionary<string, UnparsedEvalInfo>( OtherInfo.UnparsedLabels );
      Labels          = new Dictionary<string, SymbolInfo>( OtherInfo.Labels );
      Zones           = new Dictionary<string, List<SymbolInfo>>( OtherInfo.Zones );
      MappedVariables = new Dictionary<string, List<SymbolInfo>>( OtherInfo.MappedVariables );
      OriginalVariables = new Set<string>( OtherInfo.OriginalVariables );
      Banks = new List<BankInfo>( OtherInfo.Banks );
      TempLabelInfo = new List<TemporaryLabelInfo>( OtherInfo.TempLabelInfo );
      AssemblerSettings = OtherInfo.AssemblerSettings;
      Macros = new Map<Tupel<string, int>, MacroFunctionInfo>( OtherInfo.Macros );
      Messages = new MultiMap<int, ParserBase.ParseMessage>( OtherInfo.Messages );
    }*/



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
      LabelDumpSettings.Clear();
      MappedVariables.Clear();
      OriginalVariables.Clear();
      FixedBreakpoints.Clear();
      Messages.Clear();
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


    public bool FindZoneInfoFromDocumentLine( string DocumentFilename, int LineIndex, out string Zone, out string CheapLabelZone )
    {
      // find global line index
      int     globalLineIndex = 0;
      Zone = "";
      CheapLabelZone = "";

      if ( !FindGlobalLineIndex( LineIndex, DocumentFilename, out globalLineIndex ) )
      {
        return false;
      }

      // work from there
      while ( globalLineIndex >= 0 )
      {
        if ( LineInfo.ContainsKey( globalLineIndex ) )
        {
          Types.ASM.LineInfo lineInfo = LineInfo[globalLineIndex];

          Zone            = lineInfo.Zone;
          CheapLabelZone  = lineInfo.CheapLabelZone;
          break;
        }
        --globalLineIndex;
      }
      return true;
    }



    public bool FindTrueLineSource( int LineIndex, out string Filename, out int LocalLineIndex )
    {
      SourceInfo dummy;
      return FindTrueLineSource( LineIndex, out Filename, out LocalLineIndex, out dummy );
    }



    public bool FindTrueLineSource( int LineIndex, out string Filename, out int LocalLineIndex, out SourceInfo SrcInfo )
    {
      Filename = "";
      LocalLineIndex = -1;
      SrcInfo = null;

      /*
      var entry = SourceInfo.Values.Where( si => ( LineIndex >= si.GlobalStartLine ) && ( LineIndex < si.GlobalStartLine + si.LineCount ) ).FirstOrDefault();
      if ( entry != null )
      {
        Filename        = entry.Filename;
        LocalLineIndex  = LineIndex + entry.LocalStartLine - entry.GlobalStartLine;
        SrcInfo         = entry;
        return true;
      }*/
      
      foreach ( Types.ASM.SourceInfo sourceInfo in SourceInfo.Values )
      {
        if ( ( LineIndex >= sourceInfo.GlobalStartLine )
        &&   ( LineIndex < sourceInfo.GlobalStartLine + sourceInfo.LineCount ) )
        {
          Filename = sourceInfo.Filename;
          LocalLineIndex = LineIndex + sourceInfo.LocalStartLine - sourceInfo.GlobalStartLine;

          SrcInfo = sourceInfo;
          return true;
        }
      }
      return false;
    }



    public bool FindGlobalLineIndex( int LineIndex, string Filename, out int GlobalLineIndex )
    {
      GlobalLineIndex = -1;
      if ( Filename == null )
      {
        return false;
      }

      Types.ASM.SourceInfo    lastFound = null;
      try
      {
        foreach ( Types.ASM.SourceInfo sourceInfo in SourceInfo.Values )
        {
          if ( ( GR.Path.IsPathEqual( Filename, sourceInfo.Filename ) ) //( Filename.ToUpper() == sourceInfo.Filename.ToUpper() )
          &&   ( LineIndex >= sourceInfo.LocalStartLine )
          &&   ( LineIndex < sourceInfo.LocalStartLine + sourceInfo.LineCount ) )
          {
            // ugh, outer source info (for nested for loops) is wrong!
            lastFound = sourceInfo;
          }
        }
      }
      catch ( System.InvalidOperationException )
      {
        // SourceInfo.Values may be modified -> the actual issue
        return false;
      }

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



    internal void InsertLines( int GlobalLineIndex, int LocalLineIndex, int LineCount )
    {
      // move all infos/symbols below down
      List<Tupel<int,int>>    lineAddressesToMove = new List<Tupel<int, int>>();
      foreach ( var entry in AddressToLine )
      {
        if ( entry.Value >= GlobalLineIndex )
        {
          lineAddressesToMove.Add( new Tupel<int, int>( entry.Key, entry.Value ) );
        }
      }
      foreach ( var entry in lineAddressesToMove )
      {
        AddressToLine.Remove( entry.first );
      }
      foreach ( var entry in lineAddressesToMove )
      {
        AddressToLine.Add( entry.first, entry.second + LineCount );
      }

      foreach ( var bank in Banks )
      {
        if ( bank.StartLine >= GlobalLineIndex )
        {
          bank.StartLine += LineCount;
        }
      }
      foreach ( var label in Labels )
      {
        if ( label.Value.References.Any( r => r.Key >= GlobalLineIndex ) )
        {
          var newSet = new MultiMap<int, SymbolReference>();
          foreach ( var reference in label.Value.References )
          {
            if ( reference.Key >= GlobalLineIndex )
            {
              newSet.Add( reference.Key + LineCount, reference.Value );
            }
            else
            {
              newSet.Add( reference.Key, reference.Value );
            }
          }
          label.Value.References = newSet;
        }
        if ( label.Value.LineIndex >= GlobalLineIndex )
        {
          label.Value.LineIndex += LineCount;
          label.Value.LocalLineIndex += LineCount;
        }
      }

      List<LineInfo>    linesToMove = new List<ASM.LineInfo>();
      foreach ( var line in LineInfo )
      {
        if ( line.Key >= GlobalLineIndex )
        {
          linesToMove.Add( line.Value );
          line.Value.LineIndex += LineCount;
        }
      }
      foreach ( var lineToMove in linesToMove )
      {
        LineInfo.Remove( lineToMove.LineIndex - LineCount );
      }
      foreach ( var lineToMove in linesToMove )
      {
        LineInfo.Add( lineToMove.LineIndex, lineToMove );
      }
      // insert new lines
      for ( int i = 0; i < LineCount; ++i )
      {
        if ( ( !LineInfo.ContainsKey( LocalLineIndex + i ) )
        &&   ( LineInfo.ContainsKey( LocalLineIndex - 1 ) ) )
        {
          var info = new LineInfo() { AddressStart = LineInfo[LocalLineIndex - 1].AddressStart };
          info.LineIndex = LocalLineIndex + i;

          LineInfo.Add( LocalLineIndex + i, info );
        }
      }

      List<SourceInfo>    sourceInfosToMove = new List<ASM.SourceInfo>();
      foreach ( var sourceInfo in SourceInfo )
      {
        // Grow or move
        if ( ( sourceInfo.Value.GlobalStartLine < GlobalLineIndex )
        &&   ( sourceInfo.Value.GlobalStartLine + sourceInfo.Value.LineCount >= GlobalLineIndex + LineCount ) )
        {
          sourceInfo.Value.LineCount += LineCount;
        }
        if ( sourceInfo.Value.GlobalStartLine >= GlobalLineIndex )
        {
          sourceInfosToMove.Add( sourceInfo.Value );
        }
      }
      foreach ( var sourceInfoToMove in sourceInfosToMove )
      {
        SourceInfo.Remove( sourceInfoToMove.GlobalStartLine );
      }
      foreach ( var sourceInfoToMove in sourceInfosToMove )
      {
        sourceInfoToMove.GlobalStartLine += LineCount;
        sourceInfoToMove.LocalStartLine += LineCount;
        SourceInfo.Add( sourceInfoToMove.GlobalStartLine, sourceInfoToMove );
      }

      foreach ( var tempLabel in TempLabelInfo )
      {
        if ( tempLabel.LineIndex >= GlobalLineIndex )
        {
          tempLabel.LineIndex += LineCount;
        }
      }

      foreach ( var unparsedLabel in UnparsedLabels )
      {
        if ( unparsedLabel.Value.LineIndex >= GlobalLineIndex )
        {
          unparsedLabel.Value.LineIndex += LineCount;
        }
      }

      foreach ( var virtualBP in VirtualBreakpoints )
      {
        if ( virtualBP.Value.LineIndex >= GlobalLineIndex )
        {
          virtualBP.Value.LineIndex += LineCount;
        }
      }
      foreach ( var zoneList in Zones )
      {
        foreach ( var zone in zoneList.Value )
        {
          // Grow or move
          if ( ( zone.LineIndex < GlobalLineIndex )
          &&   ( zone.LineIndex + zone.LineCount >= GlobalLineIndex + LineCount ) )
          {
            zone.LineCount += LineCount;
          }
          if ( zone.LineIndex >= GlobalLineIndex )
          {
            zone.LineIndex += LineCount;
          }
        }
      }
    }



    internal void RemoveLines( int GlobalLineIndex, int LocalLineIndex, int LineCount )
    {
      // move all infos/symbols below up
      List<Tupel<int,int>>    lineAddressesToMove = new List<Tupel<int, int>>();
      List<Tupel<int,int>>    lineAddressesToRemove = new List<Tupel<int, int>>();
      foreach ( var entry in AddressToLine )
      {
        if ( entry.Value >= GlobalLineIndex + LineCount )
        {
          lineAddressesToMove.Add( new Tupel<int, int>( entry.Key, entry.Value ) );
        }
        else if ( ( entry.Value >= GlobalLineIndex )
        && ( entry.Value < GlobalLineIndex + LineCount ) )
        {
          lineAddressesToRemove.Add( new Tupel<int, int>( entry.Key, entry.Value ) );
        }
      }
      foreach ( var entry in lineAddressesToMove )
      {
        AddressToLine.Remove( entry.first );
      }
      foreach ( var entry in lineAddressesToRemove )
      {
        AddressToLine.Remove( entry.first );
      }
      foreach ( var entry in lineAddressesToMove )
      {
        AddressToLine.Add( entry.first, entry.second - LineCount );
      }

      foreach ( var bank in Banks )
      {
        if ( bank.StartLine >= GlobalLineIndex + LineCount )
        {
          bank.StartLine -= LineCount;
        }
      }
      foreach ( var label in Labels )
      {
        if ( label.Value.LineIndex >= GlobalLineIndex + LineCount )
        {
          label.Value.LineIndex -= LineCount;
          label.Value.LocalLineIndex -= LineCount;
        }
      }

      List<LineInfo>    linesToMove = new List<ASM.LineInfo>();
      List<LineInfo>    linesToRemove = new List<ASM.LineInfo>();
      foreach ( var line in LineInfo )
      {
        if ( line.Key >= GlobalLineIndex + LineCount )
        {
          linesToMove.Add( line.Value );
          line.Value.LineIndex -= LineCount;
        }
        else if ( line.Key >= GlobalLineIndex )
        {
          linesToRemove.Add( line.Value );
        }
      }
      foreach ( var lineToMove in linesToMove )
      {
        LineInfo.Remove( lineToMove.LineIndex + LineCount );
      }
      foreach ( var lineToMove in linesToRemove )
      {
        LineInfo.Remove( lineToMove.LineIndex );
      }
      foreach ( var lineToMove in linesToMove )
      {
        if ( LineInfo.ContainsKey( lineToMove.LineIndex ) )
        {
          LineInfo[lineToMove.LineIndex] = lineToMove;
        }
        else
        {
          LineInfo.Add( lineToMove.LineIndex, lineToMove );
        }
      }

      List<SourceInfo>    sourceInfosToMove = new List<ASM.SourceInfo>();
      List<SourceInfo>    sourceInfosToRemove = new List<ASM.SourceInfo>();
      foreach ( var sourceInfo in SourceInfo )
      {
        // shrink or move
        if ( sourceInfo.Value.GlobalStartLine + sourceInfo.Value.LineCount <= GlobalLineIndex )
        {
          continue;
        }
        if ( ( sourceInfo.Value.GlobalStartLine >= GlobalLineIndex )
        && ( sourceInfo.Value.GlobalStartLine + sourceInfo.Value.LineCount <= GlobalLineIndex + LineCount ) )
        {
          // completely inside, remove
          sourceInfosToRemove.Add( sourceInfo.Value );
          continue;
        }
        if ( ( sourceInfo.Value.GlobalStartLine < GlobalLineIndex )
        && ( sourceInfo.Value.GlobalStartLine + sourceInfo.Value.LineCount <= GlobalLineIndex + LineCount ) )
        {
          // outside top
          int     linesToCutTop = GlobalLineIndex - sourceInfo.Value.GlobalStartLine;
          //sourceInfo.Value.LocalStartLine -= linesToCutTop;
          //sourceInfo.Value.GlobalStartLine -= linesToCutTop;
          sourceInfo.Value.LineCount -= linesToCutTop;

          sourceInfosToMove.Add( sourceInfo.Value );
          continue;
        }
        if ( ( sourceInfo.Value.GlobalStartLine >= GlobalLineIndex )
        && ( sourceInfo.Value.GlobalStartLine + sourceInfo.Value.LineCount > GlobalLineIndex + LineCount ) )
        {
          // outside bottom
          int     linesToCut = sourceInfo.Value.GlobalStartLine + sourceInfo.Value.LineCount - ( GlobalLineIndex + LineCount );
          sourceInfo.Value.LineCount -= linesToCut;
          continue;
        }
        // cutting a part out of this
        sourceInfo.Value.LineCount -= LineCount;
      }
      foreach ( var sourceInfoToMove in sourceInfosToMove )
      {
        SourceInfo.Remove( sourceInfoToMove.GlobalStartLine );
      }
      foreach ( var sourceInfoToMove in sourceInfosToRemove )
      {
        SourceInfo.Remove( sourceInfoToMove.GlobalStartLine );
      }
      foreach ( var sourceInfoToMove in sourceInfosToMove )
      {
        sourceInfoToMove.GlobalStartLine -= LineCount;
        sourceInfoToMove.LocalStartLine -= LineCount;
        if ( !SourceInfo.ContainsKey( sourceInfoToMove.GlobalStartLine ) )
        {
          // safety measure, if there's duplicate entries somethings off
          SourceInfo.Add( sourceInfoToMove.GlobalStartLine, sourceInfoToMove );
        }
      }

      foreach ( var tempLabel in TempLabelInfo )
      {
        if ( tempLabel.LineIndex >= GlobalLineIndex + LineCount )
        {
          tempLabel.LineIndex -= LineCount;
        }
      }

      foreach ( var unparsedLabel in UnparsedLabels )
      {
        if ( unparsedLabel.Value.LineIndex >= GlobalLineIndex + LineCount )
        {
          unparsedLabel.Value.LineIndex -= LineCount;
        }
      }

      foreach ( var virtualBP in VirtualBreakpoints )
      {
        if ( virtualBP.Value.LineIndex >= GlobalLineIndex + LineCount )
        {
          virtualBP.Value.LineIndex -= LineCount;
        }
      }
      foreach ( var zoneList in Zones )
      {
        foreach ( var zone in zoneList.Value )
        {
          // shrink or move
          if ( ( zone.LineIndex < GlobalLineIndex )
          &&   ( zone.LineIndex + zone.LineCount >= GlobalLineIndex + LineCount ) )
          {
            zone.LineCount -= LineCount;
          }
          if ( zone.LineIndex >= GlobalLineIndex + LineCount )
          {
            zone.LineIndex -= LineCount;
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
      return (int)Labels[Token].AddressOrValue;
    }



    public GR.Collections.Set<int> FindAllReferences( string Token, string Zone, string CheapLabelParent, out SymbolInfo Symbol )
    {
      var list  = new GR.Collections.Set<int>();
      Symbol    = null;

      if ( AssemblerSettings != null )
      {
        if ( !AssemblerSettings.CaseSensitive )
        {
          Token = Token.ToUpper();
        }
      }
      if ( !Labels.ContainsKey( Token ) )
      {
        if ( Token.StartsWith( "@" ) )
        {
          if ( Labels.ContainsKey( CheapLabelParent + Token ) )
          {
            Symbol = Labels[CheapLabelParent + Token];
            list.AddRange( Symbol.References.Keys );
            list.Add( Symbol.LineIndex );
          }
        }
        if ( Labels.ContainsKey( Zone + Token ) )
        {
          Symbol = Labels[Zone + Token];
          list.AddRange( Symbol.References.Keys );
          list.Add( Symbol.LineIndex );
          return list;
        }

        if ( Token.StartsWith( "." ) )
        {
          Token = Zone + Token;
        }

        foreach ( var tempLabel in TempLabelInfo.Where( tl => tl.Name == Token ) )
        {
          Symbol = tempLabel.Symbol;

          list.AddRange( Symbol.References.Keys );
          list.Add( Symbol.LineIndex );
        }
        if ( list.Count > 0 )
        {
          return list;
        }

        var symbol = Macros.Keys.FirstOrDefault( m => m.first == Token );
        if ( symbol != null )
        {
          Symbol = Macros[symbol].Symbol;
          list.AddRange( Symbol.References.Keys );
          list.Add( Symbol.LineIndex );
        }
        return list;
      }
      Symbol = Labels[Token];
      list.AddRange( Symbol.References.Keys );
      list.Add( Symbol.LineIndex );
      return list;
    }



    public SymbolInfo TokenInfoFromName( string Token, string Zone, string CheapLabelParent, int GlobalLineIndex = -1, TokenInfo.TokenType TokenTypeToSearch = TokenInfo.TokenType.UNKNOWN )
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
        if ( ( TokenTypeToSearch == TokenInfo.TokenType.UNKNOWN )
        ||   ( TokenTypeToSearch == TokenInfo.TokenType.LABEL_CHEAP_LOCAL ) )
        {
          if ( Token.StartsWith( "@" ) )
          {
            if ( Labels.ContainsKey( CheapLabelParent + Token ) )
            {
              return Labels[CheapLabelParent + Token];
            }
          }
        }
        if ( ( TokenTypeToSearch == TokenInfo.TokenType.UNKNOWN )
        ||   ( TokenTypeToSearch == TokenInfo.TokenType.LABEL_LOCAL )
        ||   ( TokenTypeToSearch == TokenInfo.TokenType.LABEL_GLOBAL )
        ||   ( TokenTypeToSearch == TokenInfo.TokenType.LABEL_CHEAP_LOCAL )
        ||   ( TokenTypeToSearch == TokenInfo.TokenType.LABEL_INTERNAL ) )
        {
          if ( Labels.ContainsKey( Zone + Token ) )
          {
            return Labels[Zone + Token];
          }

          if ( Token.StartsWith( "." ) )
          {
            Token = Zone + Token;
          }

          TemporaryLabelInfo    tempLabel = null;
          if ( GlobalLineIndex != -1 )
          {
            tempLabel = TempLabelInfo.FirstOrDefault( tl => 
                    ( tl.Name == Token ) 
                && ( GlobalLineIndex >= tl.LineIndex ) 
                && ( ( tl.LineCount == -1 ) 
                ||   ( GlobalLineIndex < tl.LineIndex + tl.LineCount ) ) );
          }
          else
          {
            tempLabel = TempLabelInfo.FirstOrDefault( tl => tl.Name == Token );
          }
          if ( tempLabel != null )
          {
            return tempLabel.Symbol;
          }
        }

        if ( ( TokenTypeToSearch == TokenInfo.TokenType.UNKNOWN )
        ||   ( TokenTypeToSearch == TokenInfo.TokenType.CALL_MACRO ) )
        {
          var symbol = Macros.Keys.FirstOrDefault( m => m.first == Token );
          if ( symbol != null ) 
          {
            return Macros[symbol].Symbol;
          }
        }
        return null;
      }
      if ( ( TokenTypeToSearch == TokenInfo.TokenType.UNKNOWN )
      ||   ( TokenTypeToSearch == TokenInfo.TokenType.CALL_MACRO ) )
      {
        var symbol = Macros.Keys.FirstOrDefault( m => m.first == Token );
        if ( symbol != null )
        {
          return Macros[symbol].Symbol;
        }
      }

      if ( ( TokenTypeToSearch == TokenInfo.TokenType.UNKNOWN )
      ||   ( TokenTypeToSearch == TokenInfo.TokenType.LABEL_LOCAL )
      ||   ( TokenTypeToSearch == TokenInfo.TokenType.LABEL_GLOBAL )
      ||   ( TokenTypeToSearch == TokenInfo.TokenType.LABEL_CHEAP_LOCAL )
      ||   ( TokenTypeToSearch == TokenInfo.TokenType.LABEL_INTERNAL ) )
      {
        return Labels[Token];
      }
      return null;
    }



    public MacroFunctionInfo MacroFromName( string MacroName, int NumParams )
    {
      if ( AssemblerSettings != null )
      {
        if ( !AssemblerSettings.CaseSensitive )
        {
          MacroName = MacroName.ToUpper();
        }
      }
      if ( NumParams == -1 )
      {
        return Macros.FirstOrDefault( m => m.Key.first == MacroName ).Value;
      }

      var key = new Tupel<string, int>( MacroName, NumParams );
      if ( !Macros.ContainsKey( key ) )
      {
        return null;
      }
      return Macros[key];
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



    public string LabelsAsFile( LabelFileFormat Format )
    {
      StringBuilder   sb = new StringBuilder();

      var  sentLabels = new GR.Collections.Set<long>();
      foreach ( var token in Labels )
      {
        if ( token.Value.Type == SymbolInfo.Types.LABEL )
        {
          switch ( Format )
          {
            case LabelFileFormat.VICE:
              // VICE now bails on values outside 16bit
              if ( ( token.Value.AddressOrValue >= 0 )
              &&   ( token.Value.AddressOrValue <= 65535 )
              &&   ( !sentLabels.ContainsValue( token.Value.AddressOrValue ) ) )
              {
                sentLabels.Add( token.Value.AddressOrValue );
                sb.Append( "add_label $" );
                sb.Append( token.Value.AddressOrValue.ToString( "X4" ) );
                sb.Append( " ." );
                sb.AppendLine( token.Key.Replace( '.', '_' ).Replace( '-', '_' ).Replace( "+", "plus" ) );
              }
              break;
            case LabelFileFormat.C64DEBUGGER:
              sb.Append( "al C:" );
              sb.Append( token.Value.AddressOrValue.ToString( "X4" ) );
              sb.Append( " ." );
              sb.AppendLine( token.Key.Replace( '.', '_' ).Replace( '-', '_' ).Replace( "+", "plus" ) );
              break;
          }
        }
      }

      return sb.ToString();
    }



    public MultiMap<string, SymbolInfo> KnownTokenInfo()
    {
      if ( _KnownTokenInfo.Count > 0 )
      {
        return _KnownTokenInfo;
      }

      foreach ( var zoneList in Zones )
      {
        foreach ( var zone in zoneList.Value )
        {
          FindTrueLineSource( zone.LineIndex, out zone.DocumentFilename, out zone.LocalLineIndex, out zone.SourceInfo );
          _KnownTokenInfo.Add( zoneList.Key, zone );
        }
      }

      foreach ( KeyValuePair<string, SymbolInfo> label in Labels )
      {
        if ( !label.Value.FromDependency )
        {
          FindTrueLineSource( label.Value.LineIndex, out label.Value.DocumentFilename, out label.Value.LocalLineIndex, out label.Value.SourceInfo );
        }
        _KnownTokenInfo.Add( label.Key, label.Value );
      }
      foreach ( KeyValuePair<string, Types.ASM.UnparsedEvalInfo> label in UnparsedLabels )
      {
        var token = new SymbolInfo();

        token.Name = label.Key;
        FindTrueLineSource( label.Value.LineIndex, out token.DocumentFilename, out token.LineIndex, out token.SourceInfo );
        _KnownTokenInfo.Add( token.Name, token );
      }
      foreach ( var tempLabel in TempLabelInfo )
      {
        FindTrueLineSource( tempLabel.LineIndex, out tempLabel.Symbol.DocumentFilename, out tempLabel.Symbol.LocalLineIndex, out tempLabel.Symbol.SourceInfo );
        _KnownTokenInfo.Add( tempLabel.Name, tempLabel.Symbol );
      }
      return _KnownTokenInfo;
    }



    public List<AutoCompleteItemInfo> KnownTokens()
    {
      if ( _KnownTokens.Count > 0 )
      {
        return _KnownTokens;
      }

      foreach ( var label in Labels )
      {
        _KnownTokens.Add( new Types.AutoCompleteItemInfo() { Symbol = label.Value, Token = label.Key, ToolTipTitle = label.Key } );
      }
      foreach ( var unparsedLabel in UnparsedLabels )
      {
        _KnownTokens.Add( new Types.AutoCompleteItemInfo() { Token = unparsedLabel.Key, ToolTipTitle = unparsedLabel.Key } );
      }
      foreach ( var opcode in Processor.Opcodes )
      {
        _KnownTokens.Add( new Types.AutoCompleteItemInfo() { Token = opcode.Key, ToolTipTitle = opcode.Key } );
      }
      return _KnownTokens;
    }



  }
}
