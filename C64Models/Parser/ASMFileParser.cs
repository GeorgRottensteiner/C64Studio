using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RetroDevStudio.Types;
using RetroDevStudio.Types.ASM;
using GR.Collections;
using GR.Memory;
using RetroDevStudio;
using RetroDevStudio.Formats;
using Tiny64;
using RetroDevStudio.Converter;
using System.Security.Policy;



namespace RetroDevStudio.Parser
{
  public partial class ASMFileParser : ParserBase
  {
    private enum PathResolving
    { 
      FROM_FILE,
      FROM_LIBRARIES_PATH,
      FROM_FILE_AND_LIBRARIES_PATH
    }

    public enum ParseLineResult
    {
      OK,
      OK_PARSE_EXPRESSION_LATER,
      RETURN_NULL,
      RETURN_FALSE,
      CALL_CONTINUE,
      ERROR_ABORT
    }

    public class ErrorInfo
    {
      public int                 LineIndex = 0;
      public int                 Pos = -1;
      public int                 Length = 0;
      public Types.ErrorCode     Code = Types.ErrorCode.OK;


      public ErrorInfo()
      {
      }

      public ErrorInfo( int LineIndex, int Pos, int Length, Types.ErrorCode Code )
      {
        this.LineIndex = LineIndex;
        this.Pos = Pos;
        this.Length = Length;
        this.Code = Code;
      }

      public void Set( int LineIndex, int StartPos, int Length, Types.ErrorCode Code )
      {
        this.LineIndex = LineIndex;
        Pos = StartPos;
        this.Length = Length;
        this.Code = Code;
      }
    };



    public delegate List<Types.TokenInfo> ExtFunction( List<Types.TokenInfo> Arguments, GR.Collections.Map<byte, byte> TextCodeMapping );

    public class ExtFunctionInfo
    {
      public string         Name = "";
      public int            NumArguments = 0;
      public int            NumResults = 0;
      public ExtFunction    Function = null;
    };

    internal static string              InternalLabelPrefix = "c64_local_label";
    internal static string              InternalLabelPostfix = "_";

    public Processor                    m_Processor = Processor.Create6510();

    private GR.Collections.Map<string, GR.Collections.Set<string>> m_LoadedFiles = new GR.Collections.Map<string, GR.Collections.Set<string>>();

    private int                         m_CompileCurrentAddress = -1;
    private bool                        m_Assume16BitAccu = false;
    private bool                        m_Assume16BitRegisters = false;

    public GR.Collections.Set<string>   ExternallyIncludedFiles = new GR.Collections.Set<string>();

    private AssemblerSettings           m_AssemblerSettings = new AssemblerSettings();

    private List<Types.ErrorCode>       m_WarningsToIgnore = new List<Types.ErrorCode>();

    private StringBuilder               m_CurrentCommentSB = new StringBuilder();

    public GR.Collections.Map<byte, byte> m_TextCodeMappingScr = new GR.Collections.Map<byte, byte>();
    public GR.Collections.Map<byte, byte> m_TextCodeMappingPet = new GR.Collections.Map<byte, byte>();
    public GR.Collections.Map<byte, byte> m_TextCodeMappingRaw = new GR.Collections.Map<byte, byte>();

    private ErrorInfo                   m_LastErrorInfo = new ErrorInfo();

    private bool                        DoLogSourceInfo = false;

    private string                      m_CurrentZoneName = "";
    private string                      m_CurrentGlobalZoneName = "";

    private int                         m_TemporaryFillLoopPos = -1;
    private bool                        m_CurrentSegmentIsVirtual = false;

    private const int                   HIGHEST_OPERATOR_PRECEDENCE = 8;

    private ParseContext                _ParseContext = null;



    public ASMFileParser()
    {
      for ( int i = 0; i < 256; ++i )
      {
        m_TextCodeMappingRaw[(byte)i] = (byte)i;
      }
      for ( int i = 0; i < 256; ++i )
      {
        byte byteValue = (byte)i;

        if ( ( byteValue >= (byte)'a' )
        &&   ( byteValue <= (byte)'z' ) )
        {
          m_TextCodeMappingScr[byteValue] = (byte)( byteValue - 'a' + 1 );
          m_TextCodeMappingPet[byteValue] = (byte)( byteValue - ( 'a' - 'A' ) );
        }
        else if ( byteValue == (byte)'£' )
        {
          m_TextCodeMappingScr[byteValue] = (byte)28;
          m_TextCodeMappingPet[byteValue] = (byte)92;
        }
        else if ( ( byteValue >= (byte)'[' )
        &&        ( byteValue <= (byte)'_' ) )
        {
          m_TextCodeMappingScr[byteValue] = (byte)( byteValue - 64 );
          m_TextCodeMappingPet[byteValue] = (byte)( byteValue - 64 );
        }
        else if ( byteValue >= (byte)'`' )
        {
          m_TextCodeMappingScr[byteValue] = (byte)64;
          m_TextCodeMappingPet[byteValue] = (byte)64;
        }
        else if ( byteValue == (byte)'@' )
        {
          m_TextCodeMappingScr[byteValue] = 0;
          m_TextCodeMappingPet[byteValue] = (byte)64;
        }
        else
        {
          m_TextCodeMappingScr[byteValue] = byteValue;
          if ( ( byteValue >= (byte)'A' )
          &&   ( byteValue <= (byte)'Z' ) )
          {
            m_TextCodeMappingPet[byteValue] = (byte)( byteValue - ( 'a' - 'A' ) + 128 + 32 );
          }
        }
      }

      m_Processor = Processor.Create6510();

      AddExtFunction( "io.filesize", 1, 1, ExtFileSize );
      AddExtFunction( "math.min", 2, 1, ExtMathMin );
      AddExtFunction( "math.max", 2, 1, ExtMathMax );
      AddExtFunction( "math.sin", 1, 1, ExtMathSinus );
      AddExtFunction( "math.cos", 1, 1, ExtMathCosinus );
      AddExtFunction( "math.tan", 1, 1, ExtMathTangens );
      AddExtFunction( "math.toradians", 1, 1, ExtMathToRadians );
      AddExtFunction( "math.todegrees", 1, 1, ExtMathToDegrees );
      AddExtFunction( "math.floor", 1, 1, ExtMathFloor );
      AddExtFunction( "math.ceiling", 1, 1, ExtMathCeiling );
      AddExtFunction( "math.random", 1, 1, ExtMathRandom );
      AddExtFunction( "math.random", 2, 1, ExtMathRandomRange );
      AddExtFunction( "math.randomseed", 1, 0, ExtMathRandomSeed );

      SetAssemblerType( Types.AssemblerType.C64_STUDIO );
    }



    public AssemblerSettings AssemblerSettings
    {
      get
      {
        return m_AssemblerSettings;
      }
    }



    public void SetAssemblerType( Types.AssemblerType Type )
    {
      m_AssemblerSettings.SetAssemblerType( Type );
      if ( m_ASMFileInfo.AssemblerSettings == null )
      {
        m_ASMFileInfo.AssemblerSettings = new AssemblerSettings();
        m_ASMFileInfo.AssemblerSettings.SetAssemblerType( Type );
      }
    }



    private void SourceInfoLog( string Message )
    {
      if ( DoLogSourceInfo )
      {
        Debug.Log( Message );
      }
    }



    public override Types.CompileTargetType CompileTarget
    {
      get
      {
        if ( m_CompileTarget != CompileTargetType.NONE )
        {
          return m_CompileTarget;
        }
        return m_AssemblerSettings.DefaultTargetType;
      }
    }



    public override string DefaultTargetExtension
    {
      get
      {
        if ( !string.IsNullOrEmpty( m_CompileTargetFile ) )
        {
          return base.DefaultTargetExtension;
        }
        return m_AssemblerSettings.DefaultTargetExtension;
      }
    }



    public Types.ASM.TemporaryLabelInfo AddTempLabel( string Name, int LineIndex, int LineCount, SymbolInfo Value, string Info )
    {
      return AddTempLabel( Name, LineIndex, LineCount, Value, Info, -1, 0 );
    }



    public Types.ASM.TemporaryLabelInfo AddTempLabel( string Name, int LineIndex, int LineCount, SymbolInfo Value, string Info, int CharIndex, int Length )
    {
      if ( string.IsNullOrEmpty( Value.Name ) )
      {
        Value.Name = Name;
      }

      for ( int i = 0; i < m_ASMFileInfo.TempLabelInfo.Count; ++i )
      {
        var oldTempInfo = m_ASMFileInfo.TempLabelInfo[m_ASMFileInfo.TempLabelInfo.Count - i - 1];

        if ( oldTempInfo.Name == Name )
        {
          if ( ( oldTempInfo.LineIndex < LineIndex )
          &&   ( oldTempInfo.LineCount == -1 ) )
          {
            // a temp label till the end
            oldTempInfo.LineCount = LineIndex - oldTempInfo.LineIndex;
            break;
          }

          if ( oldTempInfo.LineIndex + oldTempInfo.LineCount > LineIndex )
          {
            // overlap! no duplicate!
            var msg = AddError( LineIndex, Types.ErrorCode.E1200_REDEFINITION_OF_LABEL, "Redefinition of label " + Name, CharIndex, Length );

            string    filename;
            int       localLine = 0;
            if ( m_ASMFileInfo.FindTrueLineSource( oldTempInfo.LineIndex, out filename, out localLine ) )
            {
              msg.AddMessage( "  already defined in " + filename + "(" + ( localLine + 1 ) + ")", filename, localLine, oldTempInfo.CharIndex, oldTempInfo.Length );
              //AddError( LineIndex, Types.ErrorCode.E1200_REDEFINITION_OF_LABEL, "  already defined in " + filename + "(" + ( localLine + 1 ) + ")", oldTempInfo.CharIndex, oldTempInfo.Length );
            }
            return null;
          }
        }
      }

      Types.ASM.TemporaryLabelInfo tempInfo = new Types.ASM.TemporaryLabelInfo();

      tempInfo.Name       = Name;
      tempInfo.LineIndex  = LineIndex;
      tempInfo.LineCount  = LineCount;
      tempInfo.Symbol     = Value;
      tempInfo.Info       = Info;
      tempInfo.CharIndex  = CharIndex;
      tempInfo.Length     = Length;

      m_ASMFileInfo.TempLabelInfo.Add( tempInfo );

      return tempInfo;
    }



    internal bool HasError()
    {
      return m_LastErrorInfo.Code != Types.ErrorCode.OK;
    }



    internal ErrorInfo GetError()
    {
      return m_LastErrorInfo;
    }



    private void CloneTempLabelsExcept( int SourceIndex, int CopyLength, int TargetIndex, string ExceptThisLabel )
    {
      List<Types.ASM.TemporaryLabelInfo>    infosToAdd = new List<Types.ASM.TemporaryLabelInfo>();

      foreach ( Types.ASM.TemporaryLabelInfo oldTempInfo in m_ASMFileInfo.TempLabelInfo )
      {
        if ( ( oldTempInfo.LineCount != -1 )
        &&   ( oldTempInfo.LineIndex + oldTempInfo.LineCount > SourceIndex )
        &&   ( oldTempInfo.LineIndex <= SourceIndex + CopyLength )
        &&   ( oldTempInfo.Name != ExceptThisLabel )
        &&   ( oldTempInfo.IsForVariable ) )
        {
          // fully inside source scope
          // need to copy!
          Types.ASM.TemporaryLabelInfo tempInfo = new Types.ASM.TemporaryLabelInfo();

          tempInfo.Name       = oldTempInfo.Name;
          tempInfo.LineIndex  = oldTempInfo.LineIndex + TargetIndex - SourceIndex;
          tempInfo.LineCount  = oldTempInfo.LineCount;
          tempInfo.Symbol     = new SymbolInfo( oldTempInfo.Symbol );
          tempInfo.Info       = oldTempInfo.Info;

          foreach ( var origTempInfo in m_ASMFileInfo.TempLabelInfo )
          {
            // we're cloning a label after a trailing label (label till the end) of the same name -> adjust line count!
            if ( ( origTempInfo.LineCount == -1 )
            &&   ( origTempInfo.Name == tempInfo.Name )
            &&   ( origTempInfo.LineIndex < tempInfo.LineIndex ) )
            {
              origTempInfo.LineCount = tempInfo.LineIndex - origTempInfo.LineIndex;
            }
          }

          //Debug.Log( $"Cloned temp label {tempInfo.Name} for line {tempInfo.LineIndex},{tempInfo.LineCount} with value {tempInfo.Symbol.AddressOrValue}" );
          infosToAdd.Add( tempInfo );
        }
      }

      m_ASMFileInfo.TempLabelInfo.AddRange( infosToAdd );
    }



    public SymbolInfo AddLabel( string Name, int Value, int SourceLine, string Zone, int CharIndex, int Length )
    {
      string          filename;
      int             localIndex = 0;
      SourceInfo      sourceInfo = null;

      m_ASMFileInfo.FindTrueLineSource( SourceLine, out filename, out localIndex, out sourceInfo );

      if ( !m_ASMFileInfo.Labels.ContainsKey( Name ) )
      {
        var token = new SymbolInfo();
        token.Type            = SymbolInfo.Types.LABEL;
        token.AddressOrValue  = Value;
        token.Name            = Name;
        token.LineIndex       = SourceLine;
        token.Zone            = Zone;
        token.DocumentFilename = filename;
        token.LocalLineIndex  = localIndex;
        token.CharIndex       = CharIndex;
        token.Length          = Length;
        token.SourceInfo      = sourceInfo;

        m_ASMFileInfo.Labels.Add( Name, token );
        return token;
      }
      if ( m_ASMFileInfo.Labels[Name].AddressOrValue != Value )
      {
        if ( Name != "*" )
        {
          var message = AddError( SourceLine, Types.ErrorCode.E1200_REDEFINITION_OF_LABEL, "Redefinition of label " + Name, CharIndex, Length );

          message.AddMessage( "  already defined in " + m_ASMFileInfo.Labels[Name].DocumentFilename + " at line " + ( m_ASMFileInfo.Labels[Name].LocalLineIndex + 1 ),
                              m_ASMFileInfo.Labels[Name].DocumentFilename,
                              m_ASMFileInfo.Labels[Name].LocalLineIndex,
                              m_ASMFileInfo.Labels[Name].CharIndex,
                              m_ASMFileInfo.Labels[Name].Length );
        }
      }
      m_ASMFileInfo.Labels[Name].AddressOrValue = Value;
      return m_ASMFileInfo.Labels[Name];
    }



    public void SetLabelValue( string Name, int Value )
    {
      if ( m_ASMFileInfo.Labels.ContainsKey( Name ) )
      {
        m_ASMFileInfo.Labels[Name].AddressOrValue = Value;
      }
    }



    public void RemoveLabel( string Name )
    {
      m_ASMFileInfo.Labels.Remove( Name );
    }



    public SymbolInfo AddPreprocessorLabel( string Name, int Value, int SourceLine )
    {
      if ( m_ASMFileInfo.Labels.ContainsKey( Name ) )
      {
        AddError( SourceLine, Types.ErrorCode.E1201_REDEFINITION_OF_PREPROCESSOR_DEFINE, "Preprocessor define " + Name + " declared more than once" );
        return m_ASMFileInfo.Labels[Name];
      }
      SymbolInfo token = AddLabel( Name, Value, SourceLine, "", -1, 0 );
      token.Type = SymbolInfo.Types.PREPROCESSOR_LABEL;
      return token;
    }



    public void AddZone( string Name, int SourceLine, int CharIndex, int Length )
    {
      string        zoneFile;
      int           localLine;
      SourceInfo    srcInfo;
      m_ASMFileInfo.FindTrueLineSource( SourceLine, out zoneFile, out localLine, out srcInfo );

      var token = new SymbolInfo();
      token.Type              = SymbolInfo.Types.ZONE;
      token.Name              = Name;
      token.LineIndex         = SourceLine;
      token.Zone              = Name;
      token.CharIndex         = CharIndex;
      token.Length            = Length;
      token.DocumentFilename  = zoneFile;
      token.SourceInfo        = srcInfo;

      if ( !m_ASMFileInfo.Zones.ContainsKey( Name ) )
      {
        m_ASMFileInfo.Zones.Add( Name, new List<SymbolInfo>() );
      }
      m_ASMFileInfo.Zones[Name].Add( token );
    }



    public void AddConstantF( string Name, SymbolInfo Value, int SourceLine, string Info, string Zone, int CharIndex, int Length )
    {
      string      filename = "";
      int         localIndex = -1;
      SourceInfo  srcInfo;
      m_ASMFileInfo.FindTrueLineSource( SourceLine, out filename, out localIndex, out srcInfo );

      // check if temp label exists
      foreach ( RetroDevStudio.Types.ASM.TemporaryLabelInfo tempLabel in m_ASMFileInfo.TempLabelInfo )
      {
        if ( tempLabel.Name == Name )
        {
          AddTempLabel( Name, SourceLine, -1, Value, Info, CharIndex, Length );
          return;
        }
      }

      if ( !m_ASMFileInfo.Labels.ContainsKey( Name ) )
      {
        SymbolInfo token = new SymbolInfo();
        token.Type              = SymbolInfo.Types.CONSTANT_REAL_NUMBER;
        token.RealValue         = Value.RealValue;
        token.Name              = Name;
        token.LineIndex         = SourceLine;
        token.Info              = Info;
        token.DocumentFilename  = filename;
        token.LocalLineIndex    = localIndex;
        token.SourceInfo        = srcInfo;
        token.Zone              = Zone;
        token.References.Add( SourceLine );

        m_ASMFileInfo.Labels.Add( Name, token );
      }
      else
      {
        if ( ( m_ASMFileInfo.Labels[Name].RealValue != Value.RealValue )
        ||   ( m_ASMFileInfo.Labels[Name].Type != Value.Type ) )
        {
          if ( Name != "*" )
          {
            // allow redefinition, turn into temp label
            var origLabel = m_ASMFileInfo.Labels[Name];

            m_ASMFileInfo.Labels.Remove( Name );

            // re-add orig as temp
            AddTempLabel( Name, origLabel.LineIndex, SourceLine - origLabel.LineIndex, origLabel, Info, CharIndex, Length );

            // add new label
            Value.Name              = Name;
            Value.LineIndex         = SourceLine;
            Value.Info              = Info;
            Value.DocumentFilename  = filename;
            Value.LocalLineIndex    = localIndex;
            Value.SourceInfo        = srcInfo;
            Value.References.Add( SourceLine );
            Value.Zone              = Zone;
            AddTempLabel( Name, SourceLine, -1, Value, Info, CharIndex, Length );
            return;
          }
        }
        m_ASMFileInfo.Labels[Name].RealValue  = Value.RealValue;
        m_ASMFileInfo.Labels[Name].Type       = SymbolInfo.Types.CONSTANT_REAL_NUMBER;
      }
    }



    public void AddConstantString( string Name, SymbolInfo Value, int SourceLine, string Info, string Zone, int CharIndex, int Length )
    {
      string      filename = "";
      int         localIndex = -1;
      SourceInfo  srcInfo;
      m_ASMFileInfo.FindTrueLineSource( SourceLine, out filename, out localIndex, out srcInfo );

      // check if temp label exists
      foreach ( RetroDevStudio.Types.ASM.TemporaryLabelInfo tempLabel in m_ASMFileInfo.TempLabelInfo )
      {
        if ( tempLabel.Name == Name )
        {
          AddTempLabel( Name, SourceLine, -1, Value, Info, CharIndex, Length );
          return;
        }
      }

      if ( !m_ASMFileInfo.Labels.ContainsKey( Name ) )
      {
        SymbolInfo token = new SymbolInfo();
        token.Type              = SymbolInfo.Types.CONSTANT_STRING;
        token.String            = Value.String;
        token.Name              = Name;
        token.LineIndex         = SourceLine;
        token.Info              = Info;
        token.DocumentFilename  = filename;
        token.LocalLineIndex    = localIndex;
        token.SourceInfo        = srcInfo;
        token.Zone              = Zone;
        token.References.Add( SourceLine );

        m_ASMFileInfo.Labels.Add( Name, token );
      }
      else
      {
        if ( ( m_ASMFileInfo.Labels[Name].String != Value.String )
        ||   ( m_ASMFileInfo.Labels[Name].Type != Value.Type ) )
        {
          if ( Name != "*" )
          {
            // allow redefinition, turn into temp label
            var origLabel = m_ASMFileInfo.Labels[Name];

            m_ASMFileInfo.Labels.Remove( Name );

            // re-add orig as temp
            AddTempLabel( Name, origLabel.LineIndex, SourceLine - origLabel.LineIndex, origLabel, Info, CharIndex, Length );

            // add new label
            Value.Name              = Name;
            Value.LineIndex         = SourceLine;
            Value.Info              = Info;
            Value.DocumentFilename  = filename;
            Value.LocalLineIndex    = localIndex;
            Value.SourceInfo        = srcInfo;
            Value.References.Add( SourceLine );
            Value.Zone              = Zone;
            AddTempLabel( Name, SourceLine, -1, Value, Info, CharIndex, Length );
            return;
          }
        }

        m_ASMFileInfo.Labels[Name].String = Value.String;
        m_ASMFileInfo.Labels[Name].Type   = SymbolInfo.Types.CONSTANT_STRING;
      }
    }



    public void AddConstant( string Name, SymbolInfo Value, int SourceLine, string Info, string Zone, int CharIndex, int Length )
    {
      string      filename = "";
      int         localIndex = -1;
      SourceInfo  srcInfo;
      m_ASMFileInfo.FindTrueLineSource( SourceLine, out filename, out localIndex, out srcInfo );

      // check if temp label exists
      foreach ( RetroDevStudio.Types.ASM.TemporaryLabelInfo tempLabel in m_ASMFileInfo.TempLabelInfo )
      {
        if ( tempLabel.Name == Name )
        {
          AddTempLabel( Name, SourceLine, -1, Value, Info, CharIndex, Length );
          return;
        }
      }

      if ( !m_ASMFileInfo.Labels.ContainsKey( Name ) )
      {
        Value.Name              = Name;
        Value.LineIndex         = SourceLine;
        Value.Info              = Info;
        Value.DocumentFilename  = filename;
        Value.LocalLineIndex    = localIndex;
        Value.SourceInfo        = srcInfo;
        Value.References.Add( SourceLine );
        Value.Zone              = Zone;

        m_ASMFileInfo.Labels.Add( Name, Value );
      }
      else
      {
        if ( ( m_ASMFileInfo.Labels[Name] != Value )
        ||   ( m_ASMFileInfo.Labels[Name].Type != Value.Type ) )
        {
          if ( Name != "*" )
          {
            // allow redefinition, turn into temp label
            var origLabel = m_ASMFileInfo.Labels[Name];

            m_ASMFileInfo.Labels.Remove( Name );

            // re-add orig as temp
            AddTempLabel( Name, origLabel.LineIndex, SourceLine - origLabel.LineIndex, origLabel, Info, CharIndex, Length );

            // add new label
            Value.Name              = Name;
            Value.LineIndex         = SourceLine;
            Value.Info              = Info;
            Value.DocumentFilename  = filename;
            Value.LocalLineIndex    = localIndex;
            Value.SourceInfo        = srcInfo;
            Value.References.Add( SourceLine );
            Value.Zone              = Zone;
            AddTempLabel( Name, SourceLine, -1, Value, Info, CharIndex, Length );
            return;
          }
        }
        m_ASMFileInfo.Labels[Name] = Value;
      }
    }



    public void AddPreprocessorConstant( string Name, long Value, int SourceLine )
    {
      if ( !m_ASMFileInfo.Labels.ContainsKey( Name ) )
      {
        SymbolInfo token = new SymbolInfo();
        token.Type = SymbolInfo.Types.PREPROCESSOR_CONSTANT_2;
        token.AddressOrValue = Value;
        token.Name = Name;
        token.LineIndex = SourceLine;
        token.References.Add( SourceLine );

        if ( Value < 256 )
        {
          token.Type = SymbolInfo.Types.PREPROCESSOR_CONSTANT_1;
        }

        m_ASMFileInfo.Labels.Add( Name, token );
      }
      else
      {
        if ( m_ASMFileInfo.Labels[Name].AddressOrValue != Value )
        {
          if ( Name != "*" )
          {
            //Debug.Log( "add constant error" );
            AddError( SourceLine, Types.ErrorCode.E1203_REDEFINITION_OF_CONSTANT, "Redefinition of constant " + Name );
          }
        }
        m_ASMFileInfo.Labels[Name].AddressOrValue = Value;
      }
    }



    public Types.ASM.UnparsedEvalInfo AddUnparsedLabel( string Name, string Value, int SourceLine )
    {
      if ( !m_ASMFileInfo.UnparsedLabels.ContainsKey( Name ) )
      {
        Types.ASM.UnparsedEvalInfo evalInfo = new Types.ASM.UnparsedEvalInfo();
        evalInfo.Name       = Name;
        evalInfo.LineIndex  = SourceLine;
        evalInfo.ToEval     = Value;
        m_ASMFileInfo.UnparsedLabels.Add( Name, evalInfo );
        return evalInfo;
      }
      if ( ( String.IsNullOrEmpty( Value ) )
      &&   ( Name != "*" ) )
      {
        AddError( SourceLine, Types.ErrorCode.E1200_REDEFINITION_OF_LABEL, "Redefinition of label " + Name );
      }
      m_ASMFileInfo.UnparsedLabels[Name].ToEval = Value;
      return m_ASMFileInfo.UnparsedLabels[Name];
    }



    public bool ParseLiteralValue( string Value, out bool Failed, out long Result, out int NumGivenBytes )
    {
      Result = -1;
      NumGivenBytes = 0;
      Failed = false;

      // hex
      if ( ( Value.StartsWith( "$" ) )
      ||   ( Value.StartsWith( "&" ) ) )
      {
        if ( long.TryParse( Value.Substring( 1 ), System.Globalization.NumberStyles.HexNumber, null, out Result ) )
        {
          NumGivenBytes = ( Value.Length - 1 + 1 ) / 2;
          return true;
        }
        Failed = true;
        return false;
      }
      else if ( Value.StartsWith( "0x" ) )
      {
        if ( long.TryParse( Value.Substring( 2 ), System.Globalization.NumberStyles.HexNumber, null, out Result ) )
        {
          NumGivenBytes = ( Value.Length - 2 + 1 ) / 2;
          return true;
        }
        Failed = true;
        return false;
      }
      else if ( ( Value.StartsWith( "%" ) )
      ||        ( Value.StartsWith( "#" ) ) )
      {
        string    convertValue = Value.Substring( 1 );
        if ( IsBinary( convertValue ) )
        {
          Result = GR.Convert.ToI32( convertValue, 2 );
          NumGivenBytes = ( Value.Length - 1 + 7 ) / 8;
          return true;
        }
        convertValue = convertValue.Replace( '#', '1' ).Replace( '.', '0' );
        if ( IsBinary( convertValue ) )
        {
          Result = GR.Convert.ToI32( convertValue, 2 );
          NumGivenBytes = ( Value.Length - 1 + 7 ) / 8;
          return true;
        }
        Failed = true;
        return false;
      }
      else if ( Value.StartsWith( "'" ) )
      {
        if ( !Value.EndsWith( "'" ) )
        {
          Failed = true;
          return false;
        }
        if ( Value.Length != 3 )
        {
          Failed = true;
          return false;
        }
        char dummy;
        if ( char.TryParse( Value.Substring( 1, 1 ), out dummy ) )
        {
          Result = dummy;
          return true;
        }
        Failed = true;
        return false;
      }

      if ( ( Value == "*" )
      ||   ( Value == "." ) )
      {
        if ( m_CompileCurrentAddress == -1 )
        {
          // cannot evaluate yet
          Failed = true;
          return false;
        }
        Result = m_CompileCurrentAddress;
        return true;
      }

      if ( long.TryParse( Value, out long resultValue ) )
      {
        Result = resultValue;
        if ( Result > 255 )
        {
          NumGivenBytes = 2;
        }
        else
        {
          NumGivenBytes = 1;
        }
        return true;
      }
      return false;
    }



    public bool ParseLiteralValueNumeric( string Value, out bool Failed, out double Result )
    {
      Result = 0;
      int NumGivenBytes = 0;
      Failed = false;

      if ( Util.StringToDouble( Value, out Result ) )
      {
        return true;
      }

      long   dummyInt = -1;

      if ( !ParseLiteralValue( Value, out Failed, out dummyInt, out NumGivenBytes ) )
      {
        return false;
      }
      Result = (double)dummyInt;
      return true;
    }



    public bool ParseLiteralValueString( string Value, out bool Failed, out string Result )
    {
      Result = "";
      Failed = false;

      if ( ( Value.Length < 2 )
      ||   ( !Value.StartsWith( "\"" ) )
      ||   ( !Value.EndsWith( "\"" ) ) )
      {
        Failed = true;
        return false;
      }

      Result = Value.Substring( 1, Value.Length - 2 );
      return true;
    }



    private bool IsBinary( string ConvertValue )
    {
      for ( int i = 0; i < ConvertValue.Length; ++i )
      {
        if ( ( ConvertValue[i] != '0' )
        &&   ( ConvertValue[i] != '1' ) )
        {
          return false;
        }
      }
      return true;
    }



    private void ClearErrorInfo()
    {
      m_LastErrorInfo.Code = Types.ErrorCode.OK;
      m_LastErrorInfo.Length = 0;
      m_LastErrorInfo.LineIndex = -1;
      m_LastErrorInfo.Pos = 0;
    }



    public bool ParseValue( int LineIndex, string Value, out SymbolInfo ResultingSymbol )
    {
      int  numDigits = 0;
      ClearErrorInfo();
      return ParseValue( LineIndex, Value, out ResultingSymbol, out numDigits );
    }



    public bool ParseValue( int LineIndex, string Value, out SymbolInfo ResultingSymbol, out int NumGivenBytes )
    {
      ResultingSymbol = null;
      NumGivenBytes = 0;
      ClearErrorInfo();
      bool failed   = false;

      if ( ParseLiteralValue( Value, out failed, out long IntegerValue, out NumGivenBytes ) )
      {
        ResultingSymbol = CreateIntegerSymbol( IntegerValue, out NumGivenBytes );
        return true;
      }
      // a good idea? collapse doubles to byte
      double  numericResult = 0;
      if ( ParseLiteralValueNumeric( Value, out failed, out numericResult ) )
      {
        ResultingSymbol = CreateNumberSymbol( numericResult );
        return true;
      }
      if ( ParseLiteralValueString( Value, out bool failedDummy, out string stringResult ) )
      {
        ResultingSymbol = CreateStringSymbol( stringResult );
        return true;
      }
      if ( failed )
      {
        m_LastErrorInfo.Set( LineIndex, 0, Value.Length, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION );
        return false;
      }

      if ( ( m_TemporaryFillLoopPos != -1 )
      &&   ( Value == "i" ) )
      {
        ResultingSymbol = CreateIntegerSymbol( m_TemporaryFillLoopPos, out NumGivenBytes );
        return true;
      }

      // check for temp labels
      bool    firstTempLabelFound = false;
      int     firstTempLabelLineIndex = 999999999;
      TemporaryLabelInfo    tempLabelInfo = null;

      foreach ( Types.ASM.TemporaryLabelInfo labelInfo in m_ASMFileInfo.TempLabelInfo )
      {
        if ( ( LineIndex >= labelInfo.LineIndex )
        &&   ( ( LineIndex < labelInfo.LineIndex + labelInfo.LineCount )
        ||     ( labelInfo.LineCount == -1 ) )
        &&   ( labelInfo.Name == Value ) )
        {
          ResultingSymbol = labelInfo.Symbol;
          ResultingSymbol.References.Add( LineIndex );
          return true;
        }
        if ( ( labelInfo.Name == Value )
        &&   ( labelInfo.LineIndex < firstTempLabelLineIndex ) )
        {
          firstTempLabelLineIndex = labelInfo.LineIndex;
          firstTempLabelFound = true;
          tempLabelInfo = labelInfo;
        }
      }

      if ( firstTempLabelFound )
      {
        // we had a temp label, but accessed it before the first definition, fall back to the first occurrence
        ResultingSymbol = tempLabelInfo.Symbol;
        ResultingSymbol.References.Add( LineIndex );
        return true;
      }



      // parse labels
      if ( !m_ASMFileInfo.Labels.ContainsKey( Value ) )
      {
        if ( ( IsLocalLabel( Value ) )
        &&   ( m_ASMFileInfo.LineInfo.ContainsKey( LineIndex ) )
        &&   ( !string.IsNullOrEmpty( m_ASMFileInfo.LineInfo[LineIndex].Zone ) ) )
        {
          // a local label inside a zone has the actual zone name in front!
          string    zoneName = m_ASMFileInfo.LineInfo[LineIndex].Zone;
          if ( m_ASMFileInfo.Labels.ContainsKey( zoneName + Value ) )
          {
            ResultingSymbol = CreateIntegerSymbol( m_ASMFileInfo.Labels[zoneName + Value].AddressOrValue, out NumGivenBytes );
            m_ASMFileInfo.Labels[zoneName + Value].References.Add( LineIndex );
            return true;
          }
        }
        if ( m_ASMFileInfo.UnparsedLabels.ContainsKey( Value ) )
        {
          m_ASMFileInfo.UnparsedLabels[Value].Used = true;
        }
        m_LastErrorInfo = new ErrorInfo( LineIndex, 0, Value.Length, Types.ErrorCode.E1010_UNKNOWN_LABEL );
        return false;
      }

      if ( m_ASMFileInfo.Labels[Value].Type == SymbolInfo.Types.CONSTANT_REAL_NUMBER )
      {
        ResultingSymbol = CreateNumberSymbol( m_ASMFileInfo.Labels[Value].RealValue );
      }
      else if ( m_ASMFileInfo.Labels[Value].Type == SymbolInfo.Types.CONSTANT_STRING )
      {
        ResultingSymbol = CreateStringSymbol( m_ASMFileInfo.Labels[Value].String );
      }
      else
      {
        ResultingSymbol = CreateIntegerSymbol( m_ASMFileInfo.Labels[Value].AddressOrValue, out NumGivenBytes );
      }
      m_ASMFileInfo.Labels[Value].References.Add( LineIndex );
      return true;
    }



    public SymbolInfo CreateNumberSymbol( double Value )
    {
      var symbol = new SymbolInfo();

      symbol.Type       = SymbolInfo.Types.CONSTANT_REAL_NUMBER;
      symbol.RealValue  = Value;

      return symbol;
    }



    public SymbolInfo CreateStringSymbol( string Value )
    {
      var symbol = new SymbolInfo();

      symbol.Type   = SymbolInfo.Types.CONSTANT_STRING;
      symbol.String = Value;

      return symbol;
    }



    public SymbolInfo CreateIntegerSymbol( long Value )
    {
      int     dummy;

      return CreateIntegerSymbol( Value, out dummy );
    }



    public SymbolInfo CreateIntegerSymbol( long Value, out int NumBytesGiven )
    {
      var symbol = new SymbolInfo();

      symbol.Type = SymbolInfo.Types.CONSTANT_1;
      symbol.AddressOrValue = Value;
      //symbol.AddressOrValue &= 0xffff;
      if ( symbol.AddressOrValue > 255 )
      {
        symbol.Type = SymbolInfo.Types.CONSTANT_2;
        NumBytesGiven = 2;
      }
      else
      {
        NumBytesGiven = 1;
      }
      return symbol;
    }



    private bool IsLocalLabel( string LabelName )
    {
      if ( string.IsNullOrEmpty( LabelName ) )
      {
        return false;
      }
      if ( m_ASMFileInfo.AssemblerSettings.AllowedTokenStartChars[Types.TokenInfo.TokenType.LABEL_LOCAL].IndexOf( LabelName[0] ) != -1 )
      {
        return true;
      }
      return false;
    }



    private int CountChar( string Source, char Char )
    {
      int count = 0;

      for ( int i = 0; i < Source.Length; ++i )
      {
        if ( Source[i] == Char )
        {
          ++count;
        }
      }
      return count;
    }



    private bool EvaluateLabel( int LineIndex, string LabelContent, GR.Collections.Map<byte, byte> TextCodeMapping, out long Result )
    {
      Result = 0;
      ClearErrorInfo();

      List<Types.TokenInfo>  tokens = ParseTokenInfo( LabelContent, 0, LabelContent.Length, TextCodeMapping );
      if ( m_LastErrorInfo.Code != Types.ErrorCode.OK )
      {
        return false;
      }

      //dh.Log( "Eval Label (" + LabelContent + ") = " + tokens.Count + " parts" );
      if ( !EvaluateTokens( LineIndex, tokens, TextCodeMapping, out SymbolInfo symbol ) )
      {
        return false;
      }

      Result = symbol.AddressOrValue;
      return true;
    }



    private bool HandleOperator( int LineIndex, Types.TokenInfo OperatorToken, Types.TokenInfo Token1, Types.TokenInfo Token2, out SymbolInfo Symbol )
    {
      Symbol = null;
      ClearErrorInfo();

      SymbolInfo token1 = null;
      SymbolInfo token2 = null;
      string    opText = OperatorToken.Content;

      if ( !ParseValue( LineIndex, Token1.Content, out token1 ) )
      {
        m_LastErrorInfo.Pos += Token1.StartPos;
        return false;
      }
      if ( !ParseValue( LineIndex, Token2.Content, out token2 ) )
      {
        m_LastErrorInfo.Pos += Token2.StartPos;
        return false;
      }

      // TODO - elevate to numeric if required!
      if ( ( token1.Type == SymbolInfo.Types.CONSTANT_STRING )
      &&   ( opText == "+" ) )
      {
        // allows anything
        if ( ( token1.String.Length == 3 )    // 3 = " + char + "
        &&   ( token2.IsInteger() ) )
        {
          // special case, string of length 1 is treated as char!
          Symbol = CreateStringSymbol( "" + (char)( token1.String[1] + token2.AddressOrValue ) );
        }
        else
        {
          Symbol = CreateStringSymbol( token1.ToString() + token2.ToString() );
        }
        return true;
      }
      else
      {
        if ( ( token1.Type != SymbolInfo.Types.CONSTANT_REAL_NUMBER )
        &&   ( token1.Type != SymbolInfo.Types.CONSTANT_1 )
        &&   ( token1.Type != SymbolInfo.Types.CONSTANT_STRING )
        &&   ( token1.Type != SymbolInfo.Types.TEMP_LABEL )
        &&   ( token1.Type != SymbolInfo.Types.CONSTANT_2 ) )
        {
          AddError( LineIndex, ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Cannot use arithmetic on non-numeric first argument", Token1.StartPos, Token1.Length );
          m_LastErrorInfo.Pos += Token1.StartPos;
          return false;
        }
        if ( ( token2.Type != SymbolInfo.Types.CONSTANT_REAL_NUMBER )
        &&   ( token2.Type != SymbolInfo.Types.CONSTANT_1 )
        &&   ( token2.Type != SymbolInfo.Types.CONSTANT_STRING )
        &&   ( token2.Type != SymbolInfo.Types.TEMP_LABEL )
        &&   ( token2.Type != SymbolInfo.Types.CONSTANT_2 ) )
        {
          AddError( LineIndex, ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Cannot use arithmetic on non-numeric second argument", Token2.StartPos, Token2.Length );
          m_LastErrorInfo.Pos += Token2.StartPos;
          return false;
        }
      }
      if ( ( token1.Type == SymbolInfo.Types.CONSTANT_STRING )
      ||   ( token2.Type == SymbolInfo.Types.CONSTANT_STRING ) )
      {
        string  firstArg  = token1.ToString();
        string  secondArg = token2.ToString();

        if ( ( opText == "=" )
        ||   ( opText == "==" ) )
        {
          Symbol = CreateIntegerSymbol( ( firstArg == secondArg ) ? 0xff : 0 );
          return true;
        }
        else if ( ( opText == "!=" )
        || ( opText == "<>" ) )
        {
          Symbol = CreateIntegerSymbol( ( firstArg != secondArg ) ? 0xff : 0 );
          return true;
        }
        else if ( opText == ">" )
        {
          int   stringCompareResult = string.Compare( firstArg, secondArg );
          Symbol = CreateIntegerSymbol( ( stringCompareResult > 0 ) ? 0xff : 0 );
          return true;
        }
        else if ( opText == "<" )
        {
          int   stringCompareResult = string.Compare( firstArg, secondArg );
          Symbol = CreateIntegerSymbol( ( stringCompareResult < 0 ) ? 0xff : 0 );
          return true;
        }
        else if ( opText == ">=" )
        {
          int   stringCompareResult = string.Compare( firstArg, secondArg );
          Symbol = CreateIntegerSymbol( ( stringCompareResult >= 0 ) ? 0xff : 0 );
          return true;
        }
        else if ( opText == "<=" )
        {
          int   stringCompareResult = string.Compare( firstArg, secondArg );
          Symbol = CreateIntegerSymbol( ( stringCompareResult <= 0 ) ? 0xff : 0 );
          return true;
        }
      }
      else if ( ( token1.Type == SymbolInfo.Types.CONSTANT_REAL_NUMBER )
      ||        ( token2.Type == SymbolInfo.Types.CONSTANT_REAL_NUMBER ) )
      {
        // elevate to real number!
        double    firstArg = (double)token1.AddressOrValue;
        if ( token1.Type == SymbolInfo.Types.CONSTANT_REAL_NUMBER )
        {
          firstArg = token1.RealValue;
        }
        double    secondArg = (double)token2.AddressOrValue;
        if ( token2.Type == SymbolInfo.Types.CONSTANT_REAL_NUMBER )
        {
          secondArg = token2.RealValue;
        }

        if ( opText == "*" )
        {
          Symbol = CreateNumberSymbol( firstArg * secondArg );
          return true;
        }
        else if ( opText == "/" )
        {
          Symbol = CreateNumberSymbol( firstArg / secondArg );
          return true;
        }
        else if ( opText == "+" )
        {
          Symbol = CreateNumberSymbol( firstArg + secondArg );
          return true;
        }
        else if ( opText == "-" )
        {
          Symbol = CreateNumberSymbol( firstArg - secondArg );
          return true;
        }
        else if ( opText == "^" )
        {
          Symbol = CreateNumberSymbol( Math.Pow( firstArg, secondArg ) );
          return true;
        }
        else if ( ( opText == "=" )
        ||        ( opText == "==" ) )
        {
          Symbol = CreateIntegerSymbol( ( firstArg == secondArg ) ? 0xff : 0 );
          return true;
        }
        else if ( ( opText == "!=" )
        ||        ( opText == "<>" ) )
        {
          Symbol = CreateIntegerSymbol( ( firstArg != secondArg ) ? 0xff : 0 );
          return true;
        }
        else if ( opText == ">" )
        {
          Symbol = CreateIntegerSymbol( ( firstArg > secondArg ) ? 0xff : 0 );
          return true;
        }
        else if ( opText == "<" )
        {
          Symbol = CreateIntegerSymbol( ( firstArg < secondArg ) ? 0xff : 0 );
          return true;
        }
        else if ( opText == ">=" )
        {
          Symbol = CreateIntegerSymbol( ( firstArg >= secondArg ) ? 0xff : 0 );
          return true;
        }
        else if ( opText == "<=" )
        {
          Symbol = CreateIntegerSymbol( ( firstArg <= secondArg ) ? 0xff : 0 );
          return true;
        }
        return false;
      }

      var arg1 = token1.AddressOrValue;
      var arg2 = token2.AddressOrValue;

      if ( opText == "*" )
      {
        Symbol = CreateIntegerSymbol( arg1 * arg2 );
        return true;
      }
      else if ( opText == "/" )
      {
        if ( arg2 == 0 )
        {
          return false;
        }
        Symbol = CreateIntegerSymbol( arg1 / arg2 );
        return true;
      }
      else if ( opText == "%" )
      {
        if ( arg2 == 0 )
        {
          return false;
        }
        Symbol = CreateIntegerSymbol( arg1 % arg2 );
        return true;
      }
      else if ( opText == "+" )
      {
        Symbol = CreateIntegerSymbol( arg1 + arg2 );
        return true;
      }
      else if ( opText == "-" )
      {
        Symbol = CreateIntegerSymbol( arg1 - arg2 );
        return true;
      }
      else if ( ( opText == "&" )
      ||        ( opText == "AND" )
      ||        ( opText == "and" ) )
      {
        Symbol = CreateIntegerSymbol( arg1 & arg2 );
        return true;
      }
      else if ( ( opText == "EOR" )
      ||        ( opText == "eor" )
      ||        ( opText == "~" )
      ||        ( opText == "XOR" )
      ||        ( opText == "xor" ) )
      {
        Symbol = CreateIntegerSymbol( arg1 ^ arg2 );
        return true;
      }
      else if ( ( opText == "|" )
      ||        ( opText == "or" )
      ||        ( opText == "OR" ) )
      {
        Symbol = CreateIntegerSymbol( arg1 | arg2 );
        return true;
      }
      else if ( ( opText == "!" )
      &&        ( !m_AssemblerSettings.HasBinaryNot ) )
      {
        // PDS style or
        Symbol = CreateIntegerSymbol( arg1 | arg2 );
        return true;
      }
      else if ( opText == ">>" )
      {
        Symbol = CreateIntegerSymbol( arg1 >> (int)arg2 );
        return true;
      }
      else if ( opText == "^" )
      {
        Symbol = CreateIntegerSymbol( (int)Math.Pow( arg1, arg2 ) );
        return true;
      }
      else if ( opText == "<<" )
      {
        Symbol = CreateIntegerSymbol( arg1 << (int)arg2 );
        return true;
      }
      else if ( ( opText == "=" )
      ||        ( opText == "==" ) )
      {
        Symbol = CreateIntegerSymbol( ( arg1 == arg2 ) ? 0xff : 0 );
        return true;
      }
      else if ( ( opText == "!=" )
      ||        ( opText == "<>" ) )
      {
        Symbol = CreateIntegerSymbol( ( arg1 != arg2 ) ? 0xff : 0 );
        return true;
      }
      else if ( opText == ">" )
      {
        Symbol = CreateIntegerSymbol( ( arg1 > arg2 ) ? 0xff : 0 );
        return true;
      }
      else if ( opText == "<" )
      {
        Symbol = CreateIntegerSymbol( ( arg1 < arg2 ) ? 0xff : 0 );
        return true;
      }
      else if ( opText == ">=" )
      {
        Symbol = CreateIntegerSymbol( ( arg1 >= arg2 ) ? 0xff : 0 );
        return true;
      }
      else if ( opText == "<=" )
      {
        Symbol = CreateIntegerSymbol( ( arg1 <= arg2 ) ? 0xff : 0 );
        return true;
      }
      return false;
    }



    private List<Types.TokenInfo> ProcessExtFunction( int LineIndex, string FunctionName, List<Types.TokenInfo> Tokens, int StartIndex, int Count, GR.Collections.Map<byte, byte> TextCodeMapping )
    {
      // split arguments, eg. "extfunction( ..,.. )"
      if ( Tokens.Count < 3 )
      {
        // this should not be possible
        AddError( LineIndex, m_LastErrorInfo.Code, "Failed to evaluate " + TokensToExpression( Tokens, StartIndex, Count ) + " for extended function '" + FunctionName + "'" );
        return null;
      }

      var parseResult = ParseLineInParameters( Tokens, StartIndex, Count, LineIndex, false, out List<List<Types.TokenInfo>> arguments );

      ExtFunctionInfo   fInfo = null;
      foreach ( var entry in m_ExtFunctions )
      {
        if ( ( entry.Key.first == FunctionName )
        &&   ( entry.Key.second == arguments.Count ) )
        {
          fInfo = entry.Value;
          break;
        }
      }
      if ( fInfo == null )
      {
        AddError( LineIndex, m_LastErrorInfo.Code, "Failed to evaluate " + TokensToExpression( Tokens, StartIndex, Count ) + " for extended function '" + FunctionName + "'" );
        return null;
      }

      // truncate not used args
      int     numArgs = Count;
      if ( Count > fInfo.NumArguments )
      {
        numArgs = fInfo.NumArguments;
      }


      List<Types.TokenInfo>         functionArguments = new List<TokenInfo>();
      int                           argIndex = 0;

      foreach ( var argumentExpressions in arguments )
      {
        if ( argumentExpressions.Count == 1 )
        {
          functionArguments.Add( argumentExpressions[0] );
          ++argIndex;
          continue;
        }

        SymbolInfo     result;
        if ( !EvaluateTokens( LineIndex, argumentExpressions, TextCodeMapping, out result ) )
        {
          AddError( LineIndex, m_LastErrorInfo.Code, "Failed to evaluate expression " + TokensToExpression( argumentExpressions ) 
            + " for argument " + ( argIndex + 1 ) + " for extended function '" + FunctionName + "'" );
          return null;
        }
        Types.TokenInfo   emptyToken = new RetroDevStudio.Types.TokenInfo();
        if ( result.Type == SymbolInfo.Types.CONSTANT_REAL_NUMBER )
        {
          emptyToken.Content = Util.DoubleToString( result.RealValue );
          emptyToken.Type = RetroDevStudio.Types.TokenInfo.TokenType.LITERAL_REAL_NUMBER;
        }
        else
        {
          emptyToken.Content = result.AddressOrValue.ToString();
          emptyToken.Type = RetroDevStudio.Types.TokenInfo.TokenType.LITERAL_NUMBER;
        }

        functionArguments.Add( emptyToken );
        ++argIndex;
      }

      while ( functionArguments.Count < fInfo.NumArguments )
      {
        Types.TokenInfo   emptyToken = new RetroDevStudio.Types.TokenInfo();
        emptyToken.Content = "0";
        emptyToken.Type = RetroDevStudio.Types.TokenInfo.TokenType.LITERAL_NUMBER;

        functionArguments.Add( emptyToken );
      }

      // run ext function
      List<Types.TokenInfo>  results = fInfo.Function( functionArguments, TextCodeMapping );

      // make sure we have the expected number of result tokens
      while ( results.Count < fInfo.NumResults )
      {
        Types.TokenInfo   emptyToken = new RetroDevStudio.Types.TokenInfo();
        emptyToken.Content = "0";
        emptyToken.Type = RetroDevStudio.Types.TokenInfo.TokenType.LITERAL_NUMBER;
        results.Add( emptyToken );
      }
      if ( results.Count > fInfo.NumResults )
      {
        results.RemoveRange( fInfo.NumResults, results.Count - fInfo.NumResults );
      }
      return results;
    }



    public bool EvaluateTokens( int LineIndex, List<Types.TokenInfo> Tokens, GR.Collections.Map<byte, byte> TextCodeMapping, out SymbolInfo ResultingToken )
    {
      int dummy = 0;
      ClearErrorInfo();
      return EvaluateTokens( LineIndex, Tokens, TextCodeMapping, out ResultingToken, out dummy );
    }



    public bool EvaluateTokens( int LineIndex, List<Types.TokenInfo> Tokens, int StartIndex, int Count, GR.Collections.Map<byte, byte> TextCodeMapping, out SymbolInfo ResultingToken )
    {
      int dummy = 0;
      ClearErrorInfo();
      return EvaluateTokens( LineIndex, Tokens, StartIndex, Count, TextCodeMapping, out ResultingToken, out dummy );
    }



    private bool EvaluateTokens( int LineIndex, List<Types.TokenInfo> Tokens, GR.Collections.Map<byte, byte> TextCodeMapping, out SymbolInfo ResultingToken, out int NumBytesGiven )
    {
      return EvaluateTokens( LineIndex, Tokens, 0, Tokens.Count, TextCodeMapping, out ResultingToken, out NumBytesGiven );
    }



    private bool EvaluateTokens( int LineIndex, List<Types.TokenInfo> Tokens, int StartIndex, int Count, GR.Collections.Map<byte, byte> TextCodeMapping, out SymbolInfo ResultingToken, out int NumBytesGiven )
    {
      ResultingToken = null;
      NumBytesGiven = 0;
      ClearErrorInfo();
      if ( Count == 0 )
      {
        return false;
      }

      int numBytesGiven = 0;
      SymbolInfo dummy;

      if ( Count == 1 )
      {
        // an actual result
        switch ( Tokens[StartIndex].Type )
        {
          case TokenInfo.TokenType.OPERATOR:
            // a forward/backward label?
            if ( Tokens[StartIndex].Content.StartsWith( "+" ) )
            {
              // special case of forward local label
              if ( FindForwardLocalLabel( LineIndex, m_ASMFileInfo.LineInfo[LineIndex], Tokens[StartIndex].Content, out string closestLabel, out int closestLine ) )
              {
                Tokens[StartIndex].Content  = closestLabel;
                Tokens[StartIndex].Type     = TokenInfo.TokenType.LABEL_LOCAL;

                if ( EvaluateTokens( LineIndex, Tokens, StartIndex, 1, TextCodeMapping, out ResultingToken ) )
                {
                  int result = (int)ResultingToken.ToInteger();
                  result &= 0xffff;
                  if ( result > 255 )
                  {
                    NumBytesGiven = 2;
                  }
                  else
                  {
                    NumBytesGiven = 1;
                  }
                  return true;
                }
              }
            }
            break;
          case TokenInfo.TokenType.LITERAL_REAL_NUMBER:
            {
              var symbol = new SymbolInfo();

              symbol.RealValue = Util.StringToDouble( Tokens[StartIndex].Content );
              symbol.Type = SymbolInfo.Types.CONSTANT_REAL_NUMBER;
              symbol.LineIndex = LineIndex;

              int result = (int)symbol.RealValue;
              result &= 0xffff;
              if ( result > 255 )
              {
                NumBytesGiven = 2;
              }
              else
              {
                NumBytesGiven = 1;
              }
              ResultingToken = symbol;
            }
            return true;
          case TokenInfo.TokenType.LITERAL_NUMBER:
            {
              var symbol = new SymbolInfo();

              if ( !ParseLiteralValue( Tokens[StartIndex].Content, out bool failed, out symbol.AddressOrValue, out NumBytesGiven ) )
              {
                return false;
              }
              symbol.Type = SymbolInfo.Types.CONSTANT_1;
              symbol.LineIndex = LineIndex;

              long result = symbol.AddressOrValue;
              result &= 0xffff;
              if ( result > 255 )
              {
                symbol.Type = SymbolInfo.Types.CONSTANT_2;
              }

              ResultingToken = symbol;
            }
            return true;
          case TokenInfo.TokenType.LITERAL_STRING:
            {
              var symbol = new SymbolInfo();
              symbol.AddressOrValue = 1;
              symbol.Type = SymbolInfo.Types.CONSTANT_STRING;
              symbol.String = Tokens[StartIndex].Content;
              symbol.LineIndex = LineIndex;
              NumBytesGiven = Tokens[StartIndex].Length;

              symbol.String = BasicFileParser.ReplaceAllMacrosByPETSCIICode( symbol.String, TextCodeMapping, out bool hadError );
              ResultingToken = symbol;
            }
            return true;
          case TokenInfo.TokenType.LITERAL_CHAR:
            {
              var symbol = new SymbolInfo();
              symbol.AddressOrValue = (byte)Tokens[StartIndex].Content[1];
              symbol.Type = SymbolInfo.Types.CONSTANT_1;
              symbol.LineIndex = LineIndex;
              NumBytesGiven = 1;
              ResultingToken = symbol;
            }
            return true;
        }

        if ( !ParseValue( LineIndex, Tokens[StartIndex].Content, out ResultingToken, out NumBytesGiven ) )
        {
          // adjust start pos
          // adjust length since we could have a replaced token (globalized local label)
          m_LastErrorInfo.Pos += Tokens[StartIndex].StartPos;
          m_LastErrorInfo.Length = Tokens[StartIndex].Length;
          return false;
        }
        return true;
      }
      if ( Count == 2 )
      {
        // unary operators
        if ( Tokens[StartIndex].Content == "-" )
        {
          if ( EvaluateTokens( LineIndex, Tokens, StartIndex + 1, Count - 1, TextCodeMapping, out ResultingToken, out NumBytesGiven ) )
          {
            if ( ResultingToken.Type == SymbolInfo.Types.CONSTANT_REAL_NUMBER )
            {
              ResultingToken.RealValue = -ResultingToken.RealValue;
              return true;
            }
            else if ( ResultingToken.IsInteger() )
            {
              ResultingToken.AddressOrValue = -ResultingToken.AddressOrValue;
              return true;
            }
            AddError( LineIndex, ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Cannot negate symbol type " + ResultingToken.Type );
            return false;
          }
        }
        else if ( Tokens[StartIndex + 1].Content.StartsWith( "%" ) )
        {
          // a "x % y" case, where % is prefixed to the second operator
          if ( ParseValue( LineIndex, Tokens[StartIndex].Content, out dummy, out numBytesGiven ) )
          {
            NumBytesGiven = Math.Max( numBytesGiven, NumBytesGiven );
          }
          if ( ParseValue( LineIndex, Tokens[StartIndex + 1].Content.Substring( 1 ), out dummy, out numBytesGiven ) )
          {
            NumBytesGiven = Math.Max( numBytesGiven, NumBytesGiven );
          }
          Types.TokenInfo   tempTokenOperator = new Types.TokenInfo();
          tempTokenOperator.Content = "%";
          tempTokenOperator.StartPos = Tokens[StartIndex + 1].StartPos;
          tempTokenOperator.Length = 1;

          Types.TokenInfo   tempToken = new Types.TokenInfo();
          tempToken.Content = Tokens[StartIndex + 1].Content.Substring( 1 );
          tempToken.StartPos = Tokens[StartIndex + 1].StartPos + 1;
          tempToken.Length = Tokens[StartIndex + 1].Length - 1;
          return HandleOperator( LineIndex, tempTokenOperator, Tokens[StartIndex], tempToken, out ResultingToken );
        }
        else if ( Tokens[StartIndex].Content == "%" )
        {
          // a binary expression
          return ParseValue( LineIndex, TokensToExpression( Tokens, StartIndex, 2 ), out ResultingToken, out NumBytesGiven );
        }
        else if ( ( m_AssemblerSettings.HasBinaryNot )
        &&        ( ( Tokens[StartIndex].Content == "!" )
        ||           ( Tokens[StartIndex].Content == "~" ) ) )
        {
          // binary not
          SymbolInfo     value;

          if ( EvaluateTokens( LineIndex, Tokens, StartIndex + 1, Count - 1, TextCodeMapping, out value, out NumBytesGiven ) )
          {
            if ( value.IsInteger() )
            {
              ResultingToken = CreateIntegerSymbol( value.ToInteger() );
              if ( NumBytesGiven == 2 )
              {
                ResultingToken.AddressOrValue = 0xffff ^ ResultingToken.AddressOrValue;
                return true;
              }
              ResultingToken.AddressOrValue = 0xff ^ ResultingToken.AddressOrValue;
              return true;
            }
          }
          return false;
        }
      }

      if ( Count < 2 )
      {
        return false;
      }

      List<Types.TokenInfo> subTokenRange = Tokens.GetRange( StartIndex, Count );

      bool  evaluatedPart = false;

      do
      {
        evaluatedPart = false;
        // check brackets first

        // find bracket pair
        int     bracketStartPos = -1;
        int     bracketEndPos = -1;

        if ( !FindInnermostBracketPositions( LineIndex, subTokenRange, 0, Count, ref bracketStartPos, ref bracketEndPos ) )
        {
          return false;
        }

        if ( ( bracketStartPos != -1 )
        &&   ( bracketEndPos != -1 ) )
        {
          SymbolInfo resultValue;

          // could we have a function?
          if ( bracketStartPos > 0 )
          {
            string    possibleFunction = subTokenRange[bracketStartPos - 1].Content.ToLower();
            if ( m_ExtFunctions.Any( ef => ef.Key.first == possibleFunction ) )
            {
              // handle function!
              List<Types.TokenInfo> results = ProcessExtFunction( LineIndex, possibleFunction, subTokenRange,  bracketStartPos + 1, bracketEndPos - bracketStartPos - 1, TextCodeMapping );
              if ( ( results == null )
              ||   ( results.Count != 1 ) )
              {
                return false;
              }

              int     startPosB = subTokenRange[bracketStartPos - 1].StartPos;
              subTokenRange.RemoveRange( bracketStartPos - 1, bracketEndPos - bracketStartPos + 2 );
              Count -= bracketEndPos - bracketStartPos + 2;

              results[0].StartPos = startPosB;

              subTokenRange.Insert( bracketStartPos - 1, results[0] );
              ++Count;
              evaluatedPart = true;
              continue;
            }
          }

          if ( subTokenRange[bracketStartPos].Content == AssemblerSettings.SQUARE_BRACKETS_OPEN )
          {
            // a temporary expression! -> we inject "i" as counting label
            SymbolInfo    oldValue = null;
            var countSymbol = new SymbolInfo() { Name = "i" };
            countSymbol.AddressOrValue = m_TemporaryFillLoopPos;

            if ( m_ASMFileInfo.Labels.ContainsKey( "i" ) )
            {
              oldValue = m_ASMFileInfo.Labels["i"];
              m_ASMFileInfo.Labels["i"] = countSymbol;
            }
            else
            {
              // add temporary label inside expression
              m_ASMFileInfo.Labels.Add( "i", countSymbol );
            }

            bool evaluationResult = EvaluateTokens( LineIndex, subTokenRange, bracketStartPos + 1, bracketEndPos - bracketStartPos - 1, TextCodeMapping, out resultValue, out numBytesGiven );

            // restore temp symbol
            if ( oldValue == null )
            {
              m_ASMFileInfo.Labels.Remove( "i" );
            }
            else
            {
              m_ASMFileInfo.Labels["i"] = oldValue;
            }

            if ( !evaluationResult )
            {
              return false;
            }
          }
          else if ( !EvaluateTokens( LineIndex, subTokenRange, bracketStartPos + 1, bracketEndPos - bracketStartPos - 1, TextCodeMapping, out resultValue, out numBytesGiven ) )
          {
            return false;
          }
          NumBytesGiven = Math.Max( numBytesGiven, NumBytesGiven );

          int     startPos = subTokenRange[bracketStartPos].StartPos;
          subTokenRange.RemoveRange( bracketStartPos, bracketEndPos - bracketStartPos + 1 );
          Count -= bracketEndPos - bracketStartPos + 1;

          Types.TokenInfo tokenResult = new Types.TokenInfo();
          if ( resultValue.Type == SymbolInfo.Types.CONSTANT_REAL_NUMBER )
          {
            tokenResult.Content = Util.DoubleToString( resultValue.RealValue );
            tokenResult.Type = Types.TokenInfo.TokenType.LITERAL_REAL_NUMBER;
          }
          else if ( resultValue.Type == SymbolInfo.Types.CONSTANT_STRING )
          {
            tokenResult.Content = '"' + resultValue.ToString() + '"';
            tokenResult.Type    = Types.TokenInfo.TokenType.LITERAL_STRING;
          }
          else
          {
            tokenResult.Content = resultValue.ToInteger().ToString();
            tokenResult.Type = Types.TokenInfo.TokenType.LITERAL_NUMBER;
          }
          tokenResult.StartPos = startPos;
          subTokenRange.Insert( bracketStartPos, tokenResult );
          ++Count;
          evaluatedPart = true;
          continue;
        }
        if ( Count >= 2 )
        {
          int highestPrecedence = -1;
          int highestPrecedenceTokenIndex = -1;
          int numHighestPrecedenceEntries = 0;
          for ( int tokenIndex = 0; tokenIndex < Count - 1; ++tokenIndex )
          {
            foreach ( KeyValuePair<string,int> oper in m_AssemblerSettings.OperatorPrecedence )
            {
              if ( subTokenRange[tokenIndex].Content == oper.Key )
              {
                if ( ( tokenIndex == 0 )
                &&   ( oper.Value != HIGHEST_OPERATOR_PRECEDENCE ) )
                {
                  // only allow < and > on first pos
                  continue;
                }

                if ( oper.Value > highestPrecedence )
                {
                  numHighestPrecedenceEntries = 1;
                  highestPrecedence = oper.Value;
                  highestPrecedenceTokenIndex = tokenIndex;
                }
                else if ( oper.Value == highestPrecedence )
                {
                  ++numHighestPrecedenceEntries;
                }
              }
            }
          }
          if ( highestPrecedence != -1 )
          {
            // evaluate token now!
            SymbolInfo result = null;

            // check if we've got a hi/lo byte operator
            if ( highestPrecedence == HIGHEST_OPERATOR_PRECEDENCE )
            {
              // must be directly connected
              // the token before must not be a evaluatable type
              //if ( ( subTokenRange[highestPrecedenceTokenIndex].StartPos + subTokenRange[highestPrecedenceTokenIndex].Length == subTokenRange[highestPrecedenceTokenIndex + 1].StartPos )
              if ( ( highestPrecedenceTokenIndex == 0 )
              ||     ( ( subTokenRange[highestPrecedenceTokenIndex - 1].Type != RetroDevStudio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
              &&       ( subTokenRange[highestPrecedenceTokenIndex - 1].Type != RetroDevStudio.Types.TokenInfo.TokenType.LABEL_INTERNAL )
              &&       ( subTokenRange[highestPrecedenceTokenIndex - 1].Type != RetroDevStudio.Types.TokenInfo.TokenType.LABEL_LOCAL )
              &&       ( subTokenRange[highestPrecedenceTokenIndex - 1].Content != "*" )
              &&       ( subTokenRange[highestPrecedenceTokenIndex - 1].Type != RetroDevStudio.Types.TokenInfo.TokenType.LITERAL_CHAR )
              &&       ( subTokenRange[highestPrecedenceTokenIndex - 1].Type != RetroDevStudio.Types.TokenInfo.TokenType.LITERAL_STRING )
              &&       ( subTokenRange[highestPrecedenceTokenIndex - 1].Type != RetroDevStudio.Types.TokenInfo.TokenType.LITERAL_NUMBER ) ) )
              {
                // eval hi/lo byte, only locally for next token!!
                if ( subTokenRange[highestPrecedenceTokenIndex].Content == "<" )
                {
                  SymbolInfo value;
                  if ( EvaluateTokens( LineIndex, subTokenRange, highestPrecedenceTokenIndex + 1, 1, TextCodeMapping, out value, out numBytesGiven ) )
                  {
                    NumBytesGiven = Math.Max( numBytesGiven, NumBytesGiven );

                    long resultValue = ( value.ToInteger() & 0x00ff );

                    subTokenRange.RemoveRange( highestPrecedenceTokenIndex, 2 );

                    Types.TokenInfo tokenResult = new Types.TokenInfo();
                    tokenResult.Content = resultValue.ToString();
                    tokenResult.Type = Types.TokenInfo.TokenType.LITERAL_NUMBER;
                    subTokenRange.Insert( highestPrecedenceTokenIndex, tokenResult );
                    evaluatedPart = true;
                    Count -= 1;
                    continue;
                  }
                  return false;
                }
                else if ( subTokenRange[highestPrecedenceTokenIndex].Content == ">" )
                {
                  SymbolInfo value;
                  if ( EvaluateTokens( LineIndex, subTokenRange, highestPrecedenceTokenIndex + 1, 1, TextCodeMapping, out value, out numBytesGiven ) )
                  {
                    NumBytesGiven = Math.Max( numBytesGiven, NumBytesGiven );

                    long resultValue = ( value.ToInteger() & 0xff00 ) >> 8;
                    subTokenRange.RemoveRange( highestPrecedenceTokenIndex, 2 );

                    Types.TokenInfo tokenResult = new Types.TokenInfo();
                    tokenResult.Content = resultValue.ToString();
                    tokenResult.Type = Types.TokenInfo.TokenType.LITERAL_NUMBER;
                    subTokenRange.Insert( highestPrecedenceTokenIndex, tokenResult );
                    evaluatedPart = true;
                    Count -= 1;
                    continue;
                  }
                  return false;
                }
                else if ( ( m_AssemblerSettings.HasBinaryNot )
                &&        ( ( subTokenRange[highestPrecedenceTokenIndex].Content == "~" )
                ||          ( subTokenRange[highestPrecedenceTokenIndex].Content == "!" ) ) )
                {
                  SymbolInfo     value;

                  if ( EvaluateTokens( LineIndex, subTokenRange, highestPrecedenceTokenIndex + 1, Count - highestPrecedenceTokenIndex - 1, TextCodeMapping, out value, out NumBytesGiven ) )
                  {
                    if ( !value.IsInteger() )
                    {
                      return false;
                    }
                    if ( NumBytesGiven == 2 )
                    {
                      value.AddressOrValue = 0xffff ^ value.AddressOrValue;
                    }
                    else
                    {
                      value.AddressOrValue = 0xff ^ value.AddressOrValue;
                    }

                    subTokenRange.RemoveRange( highestPrecedenceTokenIndex, 2 );

                    Types.TokenInfo tokenResult = new Types.TokenInfo();
                    tokenResult.Content = value.AddressOrValue.ToString();
                    tokenResult.Type = Types.TokenInfo.TokenType.LITERAL_NUMBER;
                    subTokenRange.Insert( highestPrecedenceTokenIndex, tokenResult );
                    evaluatedPart = true;
                    Count -= 1;
                    continue;
                  }
                  return false;
                }
              }
            }

            // is second token with minus in front?
            if ( ( highestPrecedenceTokenIndex >= 1 )
            &&   ( highestPrecedenceTokenIndex + 2 < subTokenRange.Count )
            &&   ( subTokenRange[highestPrecedenceTokenIndex + 1].Type == TokenInfo.TokenType.OPERATOR )
            &&   ( subTokenRange[highestPrecedenceTokenIndex + 1].Content == "-" ) )
            {
              SymbolInfo     value;
              if ( !EvaluateTokens( LineIndex, subTokenRange, highestPrecedenceTokenIndex + 2, 1, TextCodeMapping, out value, out NumBytesGiven ) )
              {
                return false;
              }
              if ( !value.IsInteger() )
              {
                return false;
              }

              long     negatedResult = -value.AddressOrValue;

              int     startPos = subTokenRange[highestPrecedenceTokenIndex + 1].StartPos;
              subTokenRange.RemoveRange( highestPrecedenceTokenIndex + 1, 2 );

              Types.TokenInfo tokenResult = new Types.TokenInfo();
              tokenResult.Content = negatedResult.ToString();
              tokenResult.Type = Types.TokenInfo.TokenType.LITERAL_NUMBER;
              tokenResult.StartPos = startPos;
              subTokenRange.Insert( highestPrecedenceTokenIndex + 1, tokenResult );
              evaluatedPart = true;
              Count -= 1;
            }

            if ( ( highestPrecedenceTokenIndex >= 1 )
            &&   ( highestPrecedenceTokenIndex + 1 < subTokenRange.Count ) )
            {
              if ( HandleOperator( LineIndex, subTokenRange[highestPrecedenceTokenIndex], subTokenRange[highestPrecedenceTokenIndex - 1], subTokenRange[highestPrecedenceTokenIndex + 1], out result ) )
              {
                if ( ParseValue( LineIndex, subTokenRange[highestPrecedenceTokenIndex - 1].Content, out dummy, out numBytesGiven ) )
                {
                  NumBytesGiven = Math.Max( numBytesGiven, NumBytesGiven );
                }
                if ( ParseValue( LineIndex, subTokenRange[highestPrecedenceTokenIndex + 1].Content, out dummy, out numBytesGiven ) )
                {
                  NumBytesGiven = Math.Max( numBytesGiven, NumBytesGiven );
                }

                int     startPos = subTokenRange[highestPrecedenceTokenIndex - 1].StartPos;
                subTokenRange.RemoveRange( highestPrecedenceTokenIndex - 1, 3 );

                Types.TokenInfo tokenResult = new Types.TokenInfo();
                if ( result.Type == SymbolInfo.Types.CONSTANT_REAL_NUMBER )
                {
                  tokenResult.Content = Util.DoubleToString( result.RealValue );
                  tokenResult.Type = Types.TokenInfo.TokenType.LITERAL_REAL_NUMBER;
                }
                else if ( result.Type == SymbolInfo.Types.CONSTANT_STRING )
                {
                  tokenResult.Content = '"' + result.ToString() + '"';
                  tokenResult.Type = Types.TokenInfo.TokenType.LITERAL_STRING;
                }
                else
                {
                  tokenResult.Content = result.ToInteger().ToString();
                  tokenResult.Type = Types.TokenInfo.TokenType.LITERAL_NUMBER;
                }
                tokenResult.StartPos = startPos;
                subTokenRange.Insert( highestPrecedenceTokenIndex - 1, tokenResult );
                evaluatedPart = true;
                Count -= 2;
              }
            }
          }
        }
      }
      while ( evaluatedPart );

      if ( Count == 1 )
      {
        return ParseValue( LineIndex, subTokenRange[0].Content, out ResultingToken, out NumBytesGiven );
      }
      if ( !HasError() )
      {
        m_LastErrorInfo.Set( LineIndex, subTokenRange[0].StartPos, subTokenRange[subTokenRange.Count - 1].EndPos - subTokenRange[0].StartPos + 1, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION );
      }
      return false;
    }



    private bool EvaluatesToNumeric( int LineIndex, List<TokenInfo> TokenRange, int Index )
    {
      // is directly a real
      if ( TokenRange[Index].Type == TokenInfo.TokenType.LITERAL_REAL_NUMBER )
      {
        return true;
      }

      if ( ( TokenRange[Index].Type != TokenInfo.TokenType.LABEL_GLOBAL )
      &&   ( TokenRange[Index].Type != TokenInfo.TokenType.LABEL_LOCAL ) )
      {
        // only labels may be defined as real numbers
        return false;
      }

      // parse labels
      if ( !m_ASMFileInfo.Labels.TryGetValue( TokenRange[Index].Content, out SymbolInfo tokenValue ) )
      {
        return false;
      }

      return ( ( tokenValue.Type == SymbolInfo.Types.CONSTANT_REAL_NUMBER )
      ||       ( tokenValue.Type == SymbolInfo.Types.VARIABLE_NUMBER ) );
    }



    private bool FindInnermostBracketPositions( int LineIndex, List<TokenInfo> subTokenRange, int Offset, int Count, ref int bracketStartPos, ref int bracketEndPos )
    {
      bracketStartPos = -1;
      bracketEndPos = -1;
      for ( int i = Offset; i < Count; ++i )
      {
        if ( IsOpeningBraceChar( subTokenRange[i].Content ) )
        {
          if ( ( bracketStartPos != -1 )
          &&   ( subTokenRange[bracketStartPos].Content == AssemblerSettings.SQUARE_BRACKETS_OPEN )
          &&   ( subTokenRange[i].Content != AssemblerSettings.SQUARE_BRACKETS_OPEN ) )
          {
            // outer square brackets override!
            continue;
          }
          bracketStartPos = i;
        }
        else if ( ( IsClosingBraceChar( subTokenRange[i].Content ) )
        &&        ( bracketStartPos != -1 )
        &&        ( IsMatchingBrace( subTokenRange[bracketStartPos].Content, subTokenRange[i].Content ) ) )
        {
          if ( bracketStartPos == -1 )
          {
            // syntax error!
            // closing bracket without opening bracket
            m_LastErrorInfo.Set( LineIndex, bracketEndPos, 1, Types.ErrorCode.E1004_MISSING_OPENING_BRACKET );
            return false;
          }
          bracketEndPos = i;
          break;
        }
      }
      if ( ( bracketStartPos != -1 )
      &&   ( bracketEndPos == -1 ) )
      {
        // syntax error
        // opening bracket without closing bracket
        m_LastErrorInfo.Set( LineIndex, bracketStartPos, 1, Types.ErrorCode.E1005_MISSING_CLOSING_BRACKET );
        return false;
      }
      return true;
    }


    private bool IsMatchingBrace( string OpeningBrace, string ClosingBrace )
    {
      int     index = m_AssemblerSettings.OpenBracketChars.IndexOf( OpeningBrace );
      if ( ( index != -1 )
      &&   ( index < m_AssemblerSettings.CloseBracketChars.Length ) )
      {
        return ClosingBrace == m_AssemblerSettings.CloseBracketChars.Substring( index, 1 );
      }
      return false;
    }



    private bool IsTrueOpeningBraceChar( string Token )
    {
      if ( ( Token == "(" )
      ||   ( Token == "[" ) )
      {
        return true;
      }
      return false;
    }



    private bool IsOpeningBraceChar( string Token )
    {
      return m_AssemblerSettings.OpenBracketChars.Contains( Token );
    }



    private bool IsClosingBraceChar( string Token )
    {
      return m_AssemblerSettings.CloseBracketChars.Contains( Token );
    }



    private byte MapTextCharacter( GR.Collections.Map<byte, byte> Mapping, byte Source )
    {
      if ( ( Mapping != null )
      &&   ( Mapping.ContainsKey( Source ) ) )
      {
        return Mapping[Source];
      }
      return Source;
    }



    private ParseLineResult FinalParseData( Types.ASM.LineInfo lineInfo, int lineIndex, bool AddErrors )
    {
      GR.Memory.ByteBuffer  lineData = new GR.Memory.ByteBuffer();
      int                   tokenIndex = 0;
      int                   expressionStartIndex = 0;
      byte                  xorValue = 0;

      var origTokens = ParseTokenInfo( lineInfo.Line, 0, lineInfo.Line.Length, lineInfo.LineCodeMapping );
      if ( ( origTokens.Count > 0 )
      &&   ( origTokens[0].Type == TokenInfo.TokenType.PSEUDO_OP )
      &&   ( m_AssemblerSettings.PseudoOps.ContainsKey( origTokens[0].Content.ToUpper() ) )
      &&   ( m_AssemblerSettings.PseudoOps[origTokens[0].Content.ToUpper()].Type == MacroInfo.PseudoOpType.TEXT_SCREEN_XOR ) )
      {
        if ( !ParseLineInParameters( origTokens, 1, origTokens.Count - 1, lineIndex, true, out List<List<TokenInfo>> lineParams ) )
        {
          return ParseLineResult.ERROR_ABORT;
        }
        // lineParams[0] is the XOR value
        if ( !EvaluateTokens( lineIndex, lineParams[0], lineInfo.LineCodeMapping, out SymbolInfo resultingValue ) )
        {
          if ( AddErrors )
          {
            AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Failed to evaluate " + TokensToExpression( lineParams[0] ) );
          }
          return ParseLineResult.RETURN_FALSE;
        }
        xorValue = (byte)resultingValue.ToInt32();
      }

      do
      {
        if ( lineInfo.NeededParsedExpression[tokenIndex].Content == "," )
        {
          // found an expression
          if ( tokenIndex - expressionStartIndex == 1 )
          {
            if ( ( lineInfo.NeededParsedExpression[expressionStartIndex].Content.StartsWith( "\"" ) )
            &&   ( lineInfo.NeededParsedExpression[expressionStartIndex].Length > 1 )
            &&   ( lineInfo.NeededParsedExpression[expressionStartIndex].Content.EndsWith( "\"" ) ) )
            {
              string    textLiteral = lineInfo.NeededParsedExpression[expressionStartIndex].Content.Substring( 1, lineInfo.NeededParsedExpression[expressionStartIndex].Length - 2 );

              textLiteral = BasicFileParser.ReplaceAllMacrosByPETSCIICode( textLiteral, lineInfo.LineCodeMapping, out bool hadError );
              if ( ( hadError )
              &&   ( AddErrors ) )
              {
                AddError( lineIndex, Types.ErrorCode.E3005_BASIC_UNKNOWN_MACRO, "Failed to evaluate " + TokensToExpression( lineInfo.NeededParsedExpression, expressionStartIndex, tokenIndex - expressionStartIndex ) );
                return ParseLineResult.RETURN_FALSE;
              }

              // a text
              foreach ( char aChar in textLiteral )
              {
                // map to PETSCII!
                lineData.AppendU8( (byte)( (byte)aChar ^ xorValue ) );
              }
            }
            else
            {
              SymbolInfo value;
              if ( !EvaluateTokens( lineIndex, lineInfo.NeededParsedExpression, expressionStartIndex, tokenIndex - expressionStartIndex, lineInfo.LineCodeMapping, out value ) )
              {
                if ( AddErrors )
                {
                  AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Failed to evaluate " + TokensToExpression( lineInfo.NeededParsedExpression, expressionStartIndex, tokenIndex - expressionStartIndex ) );
                }
                return ParseLineResult.RETURN_FALSE;
              }
              lineData.AppendU8( (byte)( (byte)value.ToInteger() ^ xorValue ) );
            }
          }
          else
          {
            SymbolInfo value;
            if ( !EvaluateTokens( lineIndex, lineInfo.NeededParsedExpression, expressionStartIndex, tokenIndex - expressionStartIndex, lineInfo.LineCodeMapping, out value ) )
            {
              if ( AddErrors )
              {
                AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Failed to evaluate " + TokensToExpression( lineInfo.NeededParsedExpression, expressionStartIndex, tokenIndex - expressionStartIndex ) );
              }
              return ParseLineResult.RETURN_FALSE;
            }
            lineData.AppendU8( (byte)( (byte)value.ToInteger() ^ xorValue ) );
          }
          expressionStartIndex = tokenIndex + 1;
        }
        ++tokenIndex;
        if ( tokenIndex == lineInfo.NeededParsedExpression.Count )
        {
          if ( expressionStartIndex <= tokenIndex - 1 )
          {
            // there's still data to evaluate
            if ( tokenIndex - expressionStartIndex == 1 )
            {
              if ( ( lineInfo.NeededParsedExpression[expressionStartIndex].Content.StartsWith( "\"" ) )
              &&   ( lineInfo.NeededParsedExpression[expressionStartIndex].Length > 1 )
              &&   ( lineInfo.NeededParsedExpression[expressionStartIndex].Content.EndsWith( "\"" ) ) )
              {
                string    textLiteral = lineInfo.NeededParsedExpression[expressionStartIndex].Content.Substring( 1, lineInfo.NeededParsedExpression[expressionStartIndex].Length - 2 );

                textLiteral = BasicFileParser.ReplaceAllMacrosByPETSCIICode( textLiteral, lineInfo.LineCodeMapping, out bool hadError );
                if ( ( hadError )
                &&   ( AddErrors ) )
                {
                  AddError( lineIndex, Types.ErrorCode.E3005_BASIC_UNKNOWN_MACRO, "Failed to evaluate " + TokensToExpression( lineInfo.NeededParsedExpression, expressionStartIndex, tokenIndex - expressionStartIndex ) );
                  return ParseLineResult.RETURN_FALSE;
                }

                // a text
                foreach ( char aChar in textLiteral )
                {
                  // map to PETSCII!
                  lineData.AppendU8( (byte)( (byte)aChar ^ xorValue ) );
                }
              }
              else
              {
                SymbolInfo value;
                if ( !EvaluateTokens( lineIndex, lineInfo.NeededParsedExpression, expressionStartIndex, tokenIndex - expressionStartIndex, lineInfo.LineCodeMapping, out value ) )
                {
                  if ( AddErrors )
                  {
                    AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Failed to evaluate " + TokensToExpression( lineInfo.NeededParsedExpression, expressionStartIndex, tokenIndex - expressionStartIndex ) );
                  }
                  return ParseLineResult.RETURN_FALSE;
                }
                lineData.AppendU8( (byte)( (byte)value.ToInteger() ^ xorValue ) );
              }
            }
            else
            {
              SymbolInfo value;
              if ( !EvaluateTokens( lineIndex, lineInfo.NeededParsedExpression, expressionStartIndex, tokenIndex - expressionStartIndex, lineInfo.LineCodeMapping, out value ) )
              {
                if ( AddErrors )
                {
                  AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Failed to evaluate " + TokensToExpression( lineInfo.NeededParsedExpression, expressionStartIndex, tokenIndex - expressionStartIndex ) );
                }
                return ParseLineResult.RETURN_FALSE;
              }
              lineData.AppendU8( (byte)( (byte)value.ToInteger() ^ xorValue ) );
            }
          }
        }
      }
      while ( tokenIndex < lineInfo.NeededParsedExpression.Count );

      lineInfo.LineData = lineData;
      return ParseLineResult.OK;
    }



    private bool DetermineUnparsedLabels()
    {
      bool newLabelDetermined = false;

      // recalc all line start addresses
      int     trueCompileCurrentAddress = -1;
      m_CompileCurrentAddress = -1;
      string    zoneName = "";
      foreach ( int lineIndex in m_ASMFileInfo.LineInfo.Keys )
      {
        Types.ASM.LineInfo lineInfo = m_ASMFileInfo.LineInfo[lineIndex];

        if ( m_CompileCurrentAddress == -1 )
        {
          if ( ( lineInfo.NumBytes == 0 )
          &&   ( !lineInfo.AddressSource.StartsWith( "*" ) ) )
          {
            // defines before program counter are allowed
            continue;
          }
          if ( ( lineInfo.AddressStart == -1 )
          &&   ( lineInfo.NumBytes > 0 ) )
          {
            AddError( lineIndex, Types.ErrorCode.E0002_CODE_WITHOUT_START_ADDRESS, "Code without start address encountered (missing *=)" );
            return false;
          }
          m_CompileCurrentAddress = lineInfo.AddressStart;
          trueCompileCurrentAddress = m_CompileCurrentAddress;
        }
        if ( lineInfo.AddressSource.StartsWith( "*" ) )
        {
          // set program counter
          m_CompileCurrentAddress = lineInfo.AddressStart;
          trueCompileCurrentAddress = m_CompileCurrentAddress;
        }

        m_CompileCurrentAddress = lineInfo.AddressStart;
        zoneName = lineInfo.Zone;

        m_CompileCurrentAddress += lineInfo.NumBytes;
        trueCompileCurrentAddress += lineInfo.NumBytes;
      }

      if ( m_CompileCurrentAddress == -1 )
      {
        if ( m_ASMFileInfo.UnparsedLabels.Count > 0 )
        {
          foreach ( Types.ASM.UnparsedEvalInfo evalInfo in m_ASMFileInfo.UnparsedLabels.Values )
          {
            AddError( evalInfo.LineIndex, Types.ErrorCode.E0002_CODE_WITHOUT_START_ADDRESS, "Code without start address encountered (missing *=)" );
          }
          return false;
        }
      }

      do
      {
        newLabelDetermined = false;

        redo:;
        foreach ( string label in m_ASMFileInfo.UnparsedLabels.Keys )
        {
          long     result = -1;

          // set program counter
          int     curLine = m_ASMFileInfo.UnparsedLabels[label].LineIndex;
          while ( ( !m_ASMFileInfo.LineInfo.ContainsKey( curLine ) )
          &&      ( curLine >= 0 ) )
          {
            --curLine;
          }
          if ( curLine == -1 )
          {
            continue;
          }
          m_CompileCurrentAddress = m_ASMFileInfo.LineInfo[curLine].AddressStart;
          trueCompileCurrentAddress = m_CompileCurrentAddress;

          if ( EvaluateLabel( m_ASMFileInfo.UnparsedLabels[label].LineIndex, m_ASMFileInfo.UnparsedLabels[label].ToEval, m_ASMFileInfo.LineInfo[curLine].LineCodeMapping, out result ) )
          {
            if ( m_ASMFileInfo.Labels.ContainsKey( label ) )
            {
              AddError( m_ASMFileInfo.UnparsedLabels[label].LineIndex, RetroDevStudio.Types.ErrorCode.E1200_REDEFINITION_OF_LABEL, "Redefinition of label " + m_ASMFileInfo.UnparsedLabels[label].Name );
              continue;
            }

            SymbolInfo token = new SymbolInfo();
            token.Type            = SymbolInfo.Types.LABEL;
            token.AddressOrValue  = result;
            token.Name            = label;
            token.LineIndex       = m_ASMFileInfo.UnparsedLabels[label].LineIndex;
            token.References.Add( m_ASMFileInfo.UnparsedLabels[label].LineIndex );
            token.Zone            = m_ASMFileInfo.UnparsedLabels[label].Zone;
            m_ASMFileInfo.Labels.Add( label, token );

            m_ASMFileInfo.UnparsedLabels.Remove( label );
            newLabelDetermined = true;
            goto redo;
          }
        }
      }
      while ( newLabelDetermined );

      foreach ( int lineIndex in m_ASMFileInfo.LineInfo.Keys )
      {
        Types.ASM.LineInfo lineInfo = m_ASMFileInfo.LineInfo[lineIndex];

        m_CompileCurrentAddress = lineInfo.AddressStart;
        if ( lineInfo.NeededParsedExpression != null )
        {
          if ( lineInfo.NeededParsedExpression.Count == 0 )
          {
            AddError( lineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "Syntax Error" );
            return false;
          }
          var result = HandleNeededParsedExpression( lineIndex, lineInfo, ref lineInfo.NeededParsedExpression, 0 );
          if ( result == ParseLineResult.RETURN_FALSE )
          {
            return false;
          }
          else if ( result == ParseLineResult.CALL_CONTINUE )
          {
            continue;
          }
        }
        if ( lineInfo.NeededParsedExpression2 != null )
        {
          if ( lineInfo.NeededParsedExpression2.Count == 0 )
          {
            AddError( lineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "Syntax Error" );
            return false;
          }
          var result = HandleNeededParsedExpression( lineIndex, lineInfo, ref lineInfo.NeededParsedExpression2, 1 );
          if ( result == ParseLineResult.RETURN_FALSE )
          {
            return false;
          }
          else if ( result == ParseLineResult.CALL_CONTINUE )
          {
            continue;
          }
        }
      }

      foreach ( string label in m_ASMFileInfo.UnparsedLabels.Keys )
      {
        //dh.Log( "Still unevaluated label:" + label + ", " + m_UnparsedLabels[label].ToEval );
        AddError( m_ASMFileInfo.UnparsedLabels[label].LineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Failed to evaluate " + m_ASMFileInfo.UnparsedLabels[label].ToEval );
      }
      if ( m_ErrorMessages > 0 )
      {
        return false;
      }
      return ( m_ASMFileInfo.UnparsedLabels.Count == 0 );
    }



    private ParseLineResult HandleNeededParsedExpression( int lineIndex, LineInfo lineInfo, ref List<TokenInfo> NeededParsedExpression, int ExpressionIndex )
    {
      bool    hasExpressions = false;
      if ( ( lineInfo.Opcode != null )
      &&   ( lineInfo.Opcode.ParserExpressions != null ) )
      {
        hasExpressions = lineInfo.Opcode.ParserExpressions.Count > 0;
      }

      // strip prefixed #
      if ( NeededParsedExpression[0].Content.StartsWith( "#" ) )
      {
        if ( NeededParsedExpression[0].Length == 1 )
        {
          NeededParsedExpression.RemoveAt( 0 );
        }
        else
        {
          NeededParsedExpression[0].Content = NeededParsedExpression[0].Content.Substring( 1 );
        }
      }
      string    lineToCheck = lineInfo.Line;

      if ( !lineToCheck.StartsWith( m_AssemblerSettings.POPrefix ) )
      {
        int   spacePos = lineToCheck.IndexOf( " " );
        if ( spacePos != -1 )
        {
          lineToCheck = lineToCheck.Substring( spacePos + 1 ).Trim();
        }
      }

      bool  isPseudoOP = false;
      if ( ( m_AssemblerSettings.POPrefix.Length != 0 )
      &&   ( lineToCheck.StartsWith( m_AssemblerSettings.POPrefix ) ) )
      {
        isPseudoOP = true;
      }
      if ( m_AssemblerSettings.POPrefix.Length == 0 )
      {
        string startToken = "";
        lineToCheck = lineToCheck.Trim();
        int spacePos = lineToCheck.IndexOf( ' ' );
        if ( spacePos == -1 )
        {
          startToken = lineToCheck.ToUpper();
        }
        else
        {
          startToken = lineToCheck.Substring( 0, spacePos ).ToUpper();
        }
        if ( m_AssemblerSettings.PseudoOps.ContainsKey( startToken ) )
        {
          isPseudoOP = true;
        }
      }

      if ( isPseudoOP )
      {
        string startToken = "";
        int spacePos = lineToCheck.IndexOf( ' ' );
        if ( spacePos == -1 )
        {
          startToken = lineToCheck.ToUpper();
        }
        else
        {
          startToken = lineToCheck.Substring( 0, spacePos ).ToUpper();
        }
        if ( m_AssemblerSettings.PseudoOps.ContainsKey( startToken ) )
        {
          var pseudoOp = m_AssemblerSettings.PseudoOps[startToken];

          switch ( pseudoOp.Type )
          {
            case RetroDevStudio.Types.MacroInfo.PseudoOpType.BASIC:
              {
                int lineSize = -1;
                if ( POBasic( lineInfo.Line, NeededParsedExpression, lineInfo.LineIndex, lineInfo, m_TextCodeMappingRaw, false, lineInfo.HideInPreprocessedOutput, out lineSize ) != ParseLineResult.OK )
                {
                  AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION,
                    "Failed to evaluate expression: " + TokensToExpression( NeededParsedExpression ) );
                }
              }
              break;
            case MacroInfo.PseudoOpType.BYTE:
              PODataByte( lineIndex, NeededParsedExpression, 0, NeededParsedExpression.Count, lineInfo, Types.MacroInfo.PseudoOpType.BYTE, lineInfo.LineCodeMapping, false );
              break;
            case MacroInfo.PseudoOpType.WORD:
              {
                int     lineInBytes = 0;
                var result = PODataWord( NeededParsedExpression, lineInfo.LineIndex, 0, NeededParsedExpression.Count, lineInfo, lineToCheck, false, true, out lineInBytes );
                if ( result == ParseLineResult.RETURN_FALSE )
                {
                  return ParseLineResult.RETURN_FALSE;
                }
              }
              break;
            case MacroInfo.PseudoOpType.WORD_BE:
              {
                int     lineInBytes = 0;
                var result = PODataWord( NeededParsedExpression, lineInfo.LineIndex, 0, NeededParsedExpression.Count, lineInfo, lineToCheck, false, false, out lineInBytes );
                if ( result == ParseLineResult.RETURN_FALSE )
                {
                  return ParseLineResult.RETURN_FALSE;
                }
              }
              break;
            case MacroInfo.PseudoOpType.DWORD:
              {
                int     lineInBytes = 0;
                var result = PODataDWord( NeededParsedExpression, lineInfo.LineIndex, 0, NeededParsedExpression.Count, lineInfo, lineToCheck, false, true, out lineInBytes );
                if ( result == ParseLineResult.RETURN_FALSE )
                {
                  return ParseLineResult.RETURN_FALSE;
                }
              }
              break;
            case MacroInfo.PseudoOpType.DWORD_BE:
              {
                int     lineInBytes = 0;
                var result = PODataDWord( NeededParsedExpression, lineInfo.LineIndex, 0, NeededParsedExpression.Count, lineInfo, lineToCheck, false, false, out lineInBytes );
                if ( result == ParseLineResult.RETURN_FALSE )
                {
                  return ParseLineResult.RETURN_FALSE;
                }
              }
              break;
            case MacroInfo.PseudoOpType.TEXT:
            case MacroInfo.PseudoOpType.TEXT_PET:
            case MacroInfo.PseudoOpType.TEXT_RAW:
            case MacroInfo.PseudoOpType.TEXT_SCREEN:
            case MacroInfo.PseudoOpType.TEXT_SCREEN_XOR:
              {
                var result = FinalParseData( lineInfo, lineIndex, true );
                if ( result == ParseLineResult.RETURN_FALSE )
                {
                  return ParseLineResult.RETURN_FALSE;
                }
              }
              break;
            case MacroInfo.PseudoOpType.FILL:
              {
                int tokenCommaIndex = -1;

                for ( int i = 0; i < NeededParsedExpression.Count; ++i )
                {
                  if ( NeededParsedExpression[i].Content == "," )
                  {
                    tokenCommaIndex = i;
                    break;
                  }
                }
                if ( tokenCommaIndex == -1 )
                {
                  AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate " + startToken + " expression" );
                  return ParseLineResult.RETURN_FALSE;
                }

                long    count = -1;
                long    value = -1;
                int     dummyBytesGiven;

                if ( !EvaluateTokens( lineIndex, NeededParsedExpression, 0, tokenCommaIndex, lineInfo.LineCodeMapping, out SymbolInfo symbol, out dummyBytesGiven ) )
                {
                  AddError( lineIndex,
                            Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION,
                            "Could not evaluate " + TokensToExpression( NeededParsedExpression, 0, tokenCommaIndex ),
                            NeededParsedExpression[0].StartPos,
                            NeededParsedExpression[tokenCommaIndex - 1].EndPos + 1 - NeededParsedExpression[0].StartPos );
                }
                count = symbol.ToInteger();
                if ( !EvaluateTokens( lineIndex, NeededParsedExpression, tokenCommaIndex + 1, NeededParsedExpression.Count - tokenCommaIndex - 1, lineInfo.LineCodeMapping, out symbol ) )
                {
                  AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate " + TokensToExpression( NeededParsedExpression, tokenCommaIndex + 1, NeededParsedExpression.Count - tokenCommaIndex - 1 ) );
                }
                value = symbol.ToInteger();
                GR.Memory.ByteBuffer lineData = new GR.Memory.ByteBuffer();
                for ( int i = 0; i < count; ++i )
                {
                  lineData.AppendU8( (byte)value );
                }
                lineInfo.LineData = lineData;
              }
              break;
            default:
              AddError( lineIndex, Types.ErrorCode.E1301_PSEUDO_OPERATION, "Unsupported pseudo op " + startToken );
              return ParseLineResult.RETURN_FALSE;
          }
        }
      }
      else
      {
        SymbolInfo value = null;
        if ( NeededParsedExpression.Count == 1 )
        {
          if ( NeededParsedExpression[0].Content.StartsWith( "+" ) )
          {
            // special case of forward local label
            if ( FindForwardLocalLabel( lineIndex, lineInfo, NeededParsedExpression[0].Content, out string closestLabel, out int closestLine ) )
            {
              NeededParsedExpression[0].Content = closestLabel;
            }
          }
        }

        if ( ( ( lineInfo.Opcode == null )
        ||   ( ( lineInfo.Opcode != null )
        &&     ( lineInfo.Opcode.Addressing != Opcode.AddressingType.ZEROPAGE_RELATIVE )
        &&     ( lineInfo.Opcode.Addressing != Opcode.AddressingType.ZEROPAGE_INDIRECT_SP_Y ) ) )
        &&     ( !EvaluateTokens( lineIndex, NeededParsedExpression, lineInfo.LineCodeMapping, out value ) ) )
        {
          /*
          if ( HasError() )
          {
            Debug.Log( $"LineIndex {lineIndex}, could not evaluate " + TokensToExpression( NeededParsedExpression ) + $" from {NeededParsedExpression[0].StartPos} to {NeededParsedExpression[NeededParsedExpression.Count - 1].EndPos}" );
          }*/

          if ( !HasError() )
          {
            Debug.Log( "EvaluateTokens failed without error info!" );
            AddError( lineIndex,
                      Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION,
                      "Could not evaluate " + TokensToExpression( NeededParsedExpression ),
                      NeededParsedExpression[0].StartPos,
                      NeededParsedExpression[NeededParsedExpression.Count - 1].EndPos + 1 - NeededParsedExpression[0].StartPos );
          }
          else if ( ( m_LastErrorInfo.Pos >= 0 )
          &&        ( m_LastErrorInfo.Length > 0 )
          &&        ( m_LastErrorInfo.Pos + m_LastErrorInfo.Length <= lineInfo.Line.Length ) )
          {
            AddError( lineIndex,
                      m_LastErrorInfo.Code,
                      "Could not evaluate " + lineInfo.Line.Substring( m_LastErrorInfo.Pos, m_LastErrorInfo.Length ),
                      m_LastErrorInfo.Pos,
                      m_LastErrorInfo.Length );
          }
          else
          {
            Debug.Log( "EvaluateTokens failed with error info, but pos/length was out of bounds!" );
            Debug.Log( "for line " + TokensToExpression( NeededParsedExpression ) );
            AddError( lineIndex,
                      Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION,
                      "Could not evaluate " + TokensToExpression( NeededParsedExpression ),
                      NeededParsedExpression[0].StartPos,
                      NeededParsedExpression[NeededParsedExpression.Count - 1].EndPos + 1 - NeededParsedExpression[0].StartPos );
          }
        }
        else
        {
          long     addressValue = 0;
          if ( value != null )
          {
            addressValue = value.ToInteger();
          }
          if ( lineInfo.Opcode != null )
          {
            if ( ( lineInfo.Opcode.ByteValue == 0x6C )
            &&   ( m_Processor.Name == "6510" )
            &&   ( ( addressValue & 0xff ) == 0xff ) )
            {
              AddWarning( lineIndex,
                          Types.ErrorCode.W0007_POTENTIAL_PROBLEM,
                          "A indirect JMP with an address ending on 0xff will not work as expected on NMOS CPUs",
                          NeededParsedExpression[0].StartPos,
                          NeededParsedExpression[NeededParsedExpression.Count - 1].EndPos + 1 - NeededParsedExpression[0].StartPos );
            }
            // check value size
            if ( hasExpressions )
            {
              if ( ( lineInfo.Opcode.ParserExpressions[ExpressionIndex].Type == Opcode.OpcodePartialExpression.EXPRESSION_32BIT )
              ||   ( lineInfo.Opcode.ParserExpressions[ExpressionIndex].Type == Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT ) )
              {
                if ( !ValidDWordValue( addressValue ) )
                {
                  AddError( lineIndex,
                            Types.ErrorCode.E1015_VALUE_OUT_OF_BOUNDS_32BIT,
                            "Value $" + addressValue.ToString( "X" ) + " (" + value + ") is out of bounds",
                            NeededParsedExpression[0].StartPos,
                            NeededParsedExpression[NeededParsedExpression.Count - 1].EndPos - NeededParsedExpression[0].StartPos + 1 );
                }
                ApplyOpcodePatch( lineInfo, (uint)addressValue, ExpressionIndex );

                NeededParsedExpression = null;
                return ParseLineResult.OK;
              }
              else if ( ( lineInfo.Opcode.ParserExpressions[ExpressionIndex].Type == Opcode.OpcodePartialExpression.EXPRESSION_24BIT )
              ||        ( lineInfo.Opcode.ParserExpressions[ExpressionIndex].Type == Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_24BIT ) )
              {
                if ( !Valid24BitValue( addressValue ) )
                {
                  AddError( lineIndex,
                            Types.ErrorCode.E1013_VALUE_OUT_OF_BOUNDS_24BIT,
                            "Value $" + addressValue.ToString( "X" ) + " (" + value + ") is out of bounds",
                            NeededParsedExpression[0].StartPos,
                            NeededParsedExpression[NeededParsedExpression.Count - 1].EndPos - NeededParsedExpression[0].StartPos + 1 );
                }
                ApplyOpcodePatch( lineInfo, (uint)addressValue, ExpressionIndex );

                NeededParsedExpression = null;
                return ParseLineResult.OK;
              }
              else if ( ( lineInfo.Opcode.ParserExpressions[ExpressionIndex].Type == Opcode.OpcodePartialExpression.EXPRESSION_16BIT )
              ||        ( lineInfo.Opcode.ParserExpressions[ExpressionIndex].Type == Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT ) )
              {
                if ( !Valid16BitAddressValue( addressValue ) )
                {
                  AddError( lineIndex,
                            Types.ErrorCode.E1003_VALUE_OUT_OF_BOUNDS_WORD,
                            "Value $" + addressValue.ToString( "X" ) + " (" + value + ") is out of bounds",
                            NeededParsedExpression[0].StartPos,
                            NeededParsedExpression[NeededParsedExpression.Count - 1].EndPos - NeededParsedExpression[0].StartPos + 1 );
                }
                ApplyOpcodePatch( lineInfo, (uint)addressValue, ExpressionIndex );

                NeededParsedExpression = null;
                return ParseLineResult.OK;
              }
              else if ( ( lineInfo.Opcode.ParserExpressions[ExpressionIndex].Type == Opcode.OpcodePartialExpression.EXPRESSION_8BIT )
              ||        ( lineInfo.Opcode.ParserExpressions[ExpressionIndex].Type == Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT ) )
              {
                if ( !ValidByteValue( addressValue ) )
                {
                  AddError( lineIndex,
                            Types.ErrorCode.E1002_VALUE_OUT_OF_BOUNDS_BYTE,
                            "Value out of bounds for byte, needs to be >= -128 and <= 255. Expression:"
                              + TokensToExpression( NeededParsedExpression ),
                            NeededParsedExpression[0].StartPos,
                            NeededParsedExpression[NeededParsedExpression.Count - 1].EndPos - NeededParsedExpression[0].StartPos + 1 );
                }
                ApplyOpcodePatch( lineInfo, (uint)addressValue, ExpressionIndex );

                NeededParsedExpression = null;
                return ParseLineResult.OK;
              }
              else if ( lineInfo.Opcode.ParserExpressions[ExpressionIndex].Type == Opcode.OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE )
              {
                long delta = addressValue - lineInfo.AddressStart - 2;
                if ( !Valid8BitRelativeValue( delta ) )
                {
                  AddError( lineIndex, Types.ErrorCode.E1100_RELATIVE_JUMP_TOO_FAR, "Relative jump too far, trying to jump " + delta + " bytes" );
                }
                ApplyOpcodePatch( lineInfo, (byte)delta, ExpressionIndex );
                NeededParsedExpression = null;
                return ParseLineResult.OK;
              }
              else if ( lineInfo.Opcode.ParserExpressions[ExpressionIndex].Type == Opcode.OpcodePartialExpression.EXPRESSION_16BIT_RELATIVE )
              {
                long delta = 0;
                if ( m_Processor.Name == "65816" )
                {
                  delta = addressValue - lineInfo.AddressStart - 3;
                }
                else
                {
                  delta = addressValue - lineInfo.AddressStart - 2;
                }
                if ( !Valid16BitRelativeValue( delta ) )
                {
                  AddError( lineIndex, Types.ErrorCode.E1100_RELATIVE_JUMP_TOO_FAR, "Relative jump too far, trying to jump " + delta + " bytes" );
                }
                ApplyOpcodePatch( lineInfo, (ushort)delta, ExpressionIndex );
                NeededParsedExpression = null;
                return ParseLineResult.OK;
              }
              else if ( ( lineInfo.Opcode.ParserExpressions[ExpressionIndex].Type == Opcode.OpcodePartialExpression.VALUE_FROM_LIST )
              ||        ( lineInfo.Opcode.ParserExpressions[ExpressionIndex].Type == Opcode.OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST ) )
                {
                if ( !ValidateExpressionValueRange( ref addressValue, lineInfo, ExpressionIndex, out int valueRangeListIndex ) )
                {
                  AddError( lineIndex,
                            Types.ErrorCode.E1014_VALUE_OUT_OF_BOUNDS_RANGE,
                            "Value $" + addressValue.ToString( "X" ) + $" ({addressValue}) is not in the range of {ListValidValues( lineInfo.Opcode.ParserExpressions[ExpressionIndex].ValidValues[valueRangeListIndex].ValidValues )}.",
                            NeededParsedExpression[0].StartPos,
                            NeededParsedExpression[NeededParsedExpression.Count - 1].EndPos + 1 - NeededParsedExpression[0].StartPos );
                }
              }
              else if ( lineInfo.Opcode.ParserExpressions[ExpressionIndex].Type == Opcode.OpcodePartialExpression.TOKEN_LIST )
              {
                // nothing to do, why is that even here?
              }
              else if ( lineInfo.Opcode.ParserExpressions[ExpressionIndex].Type == Opcode.OpcodePartialExpression.COMPLEX )
              {
                foreach ( var complexValue in lineInfo.Opcode.ParserExpressions[ExpressionIndex].ValidValues )
                {
                  if ( complexValue.Expression == Opcode.OpcodePartialExpression.EXPRESSION_8BIT )
                  {
                    if ( !ValidByteValue( addressValue ) )
                    {
                      AddError( lineIndex,
                                Types.ErrorCode.E1002_VALUE_OUT_OF_BOUNDS_BYTE,
                                "Value out of bounds for byte, needs to be >= -128 and <= 255. Expression:"
                                  + TokensToExpression( NeededParsedExpression ),
                                NeededParsedExpression[0].StartPos,
                                NeededParsedExpression[NeededParsedExpression.Count - 1].EndPos - NeededParsedExpression[0].StartPos + 1 );
                    }
                    ApplyOpcodePatch( lineInfo, (uint)addressValue, ExpressionIndex );

                    NeededParsedExpression = null;
                    return ParseLineResult.OK;
                  }
                  else if ( complexValue.Expression == Opcode.OpcodePartialExpression.EXPRESSION_16BIT )
                  {
                    if ( !Valid16BitAddressValue( addressValue ) )
                    {
                      AddError( lineIndex,
                                Types.ErrorCode.E1003_VALUE_OUT_OF_BOUNDS_WORD,
                                "Value $" + addressValue.ToString( "X" ) + " (" + value + ") is out of bounds",
                                NeededParsedExpression[0].StartPos,
                                NeededParsedExpression[NeededParsedExpression.Count - 1].EndPos - NeededParsedExpression[0].StartPos + 1 );
                    }
                    ApplyOpcodePatch( lineInfo, (uint)addressValue, ExpressionIndex );

                    NeededParsedExpression = null;
                    return ParseLineResult.OK;
                  }
                  else if ( complexValue.Expression != Opcode.OpcodePartialExpression.UNUSED )
                  {
                    Debug.Log( "oha" );
                  }
                }
              }
              else
              {
                Debug.Log( "Missing expression late parsing for " + lineInfo.Opcode.ParserExpressions[ExpressionIndex].Type );
              }
            }

            if ( ( lineInfo.Opcode.Addressing == Tiny64.Opcode.AddressingType.ZEROPAGE_INDIRECT_Y )
            ||   ( lineInfo.Opcode.Addressing == Tiny64.Opcode.AddressingType.ZEROPAGE_INDIRECT_X )
            ||   ( lineInfo.Opcode.Addressing == Tiny64.Opcode.AddressingType.ZEROPAGE_INDIRECT_Z )
            ||   ( lineInfo.Opcode.Addressing == Tiny64.Opcode.AddressingType.IMMEDIATE_8BIT )
            ||   ( lineInfo.Opcode.Addressing == Tiny64.Opcode.AddressingType.IMMEDIATE_ACCU )
            ||   ( lineInfo.Opcode.Addressing == Tiny64.Opcode.AddressingType.IMMEDIATE_REGISTER )
            ||   ( lineInfo.Opcode.Addressing == Tiny64.Opcode.AddressingType.ZEROPAGE_X )
            ||   ( lineInfo.Opcode.Addressing == Tiny64.Opcode.AddressingType.ZEROPAGE_Y ) )
            {
              if ( !ValidByteValue( addressValue ) )
              {
                AddError( lineIndex,
                          Types.ErrorCode.E1002_VALUE_OUT_OF_BOUNDS_BYTE,
                          "Value out of bounds for byte, needs to be >= -128 and <= 255. Expression:"
                            + TokensToExpression( NeededParsedExpression ),
                          NeededParsedExpression[0].StartPos,
                          NeededParsedExpression[NeededParsedExpression.Count - 1].EndPos - NeededParsedExpression[0].StartPos + 1 );
                lineInfo.LineData.AppendU8( 0 );
                return ParseLineResult.CALL_CONTINUE;
              }
            }
            if ( lineInfo.Opcode.Addressing == Opcode.AddressingType.ZEROPAGE_RELATIVE )
            {
              // this has two seperate expressions
              List<List<TokenInfo>> tokenInfos;
              if ( !ParseLineInParameters( NeededParsedExpression, 0, NeededParsedExpression.Count, lineIndex, true, out tokenInfos ) )
              {
                AddError( lineIndex,
                          m_LastErrorInfo.Code,
                          "Failed to parse opcode arguments" );
              }
              else if ( tokenInfos.Count != 2 )
              {
                AddError( lineIndex,
                          ErrorCode.E1000_SYNTAX_ERROR,
                          "Expected two arguments to zeropage relative addressing opcode." );
              }
              else
              {
                if ( !EvaluateTokens( lineIndex, tokenInfos[0], lineInfo.LineCodeMapping, out SymbolInfo zeroPageValueSymbol ) )
                {
                  AddError( lineIndex,
                            m_LastErrorInfo.Code,
                            "Could not evaluate " + lineInfo.Line.Substring( m_LastErrorInfo.Pos, m_LastErrorInfo.Length ),
                            m_LastErrorInfo.Pos,
                            m_LastErrorInfo.Length );
                }
                else if ( !EvaluateTokens( lineIndex, tokenInfos[1], lineInfo.LineCodeMapping, out SymbolInfo relativeValueSymbol ) )
                {
                  AddError( lineIndex,
                           m_LastErrorInfo.Code,
                           "Could not evaluate " + lineInfo.Line.Substring( m_LastErrorInfo.Pos, m_LastErrorInfo.Length ),
                           m_LastErrorInfo.Pos,
                           m_LastErrorInfo.Length );
                }
                else
                {
                  long  zeroPageValue = zeroPageValueSymbol.ToInteger();
                  long  relativeValue = relativeValueSymbol.ToInteger();
                  // zeropage numerand
                  if ( !ValidByteValue( zeroPageValue ) )
                  {
                    AddError( lineIndex,
                              Types.ErrorCode.E1002_VALUE_OUT_OF_BOUNDS_BYTE,
                              "Value out of bounds for byte, needs to be >= -128 and <= 255. Expression:"
                                + TokensToExpression( tokenInfos[0] ),
                              tokenInfos[0][0].StartPos,
                              tokenInfos[0][tokenInfos[0].Count - 1].EndPos - tokenInfos[0][0].StartPos + 1 );
                  }
                  else
                  {
                    if ( hasExpressions )
                    {
                      ApplyOpcodePatch( lineInfo, (byte)zeroPageValue, ExpressionIndex );
                    }
                    else
                    {
                      lineInfo.LineData.AppendU8( (byte)zeroPageValue );
                    }
                  }

                  // relative label
                  long delta = relativeValue - lineInfo.AddressStart - 3;
                  if ( !Valid8BitRelativeValue( delta ) )
                  {
                    AddError( lineIndex, Types.ErrorCode.E1100_RELATIVE_JUMP_TOO_FAR, "Relative jump too far, trying to jump " + delta + " bytes" );
                    if ( !hasExpressions )
                    {
                      lineInfo.LineData.AppendU8( 0 );
                    }
                  }
                  else
                  {
                    if ( hasExpressions )
                    {
                      ApplyOpcodePatch( lineInfo, (byte)delta, ExpressionIndex );
                    }
                    else
                    {
                      lineInfo.LineData.AppendU8( (byte)delta );
                    }
                  }
                }
              }
            }
            else if ( lineInfo.Opcode.Addressing == Opcode.AddressingType.ZEROPAGE_INDIRECT_SP_Y )
            {
              // this has two seperate expressions
              List<List<TokenInfo>> tokenInfos;
              if ( !ParseLineInParameters( NeededParsedExpression, 1, NeededParsedExpression.Count - 3, lineIndex, true, out tokenInfos ) )
              {
                AddError( lineIndex,
                          m_LastErrorInfo.Code,
                          "Failed to parse opcode arguments" );
              }
              else if ( tokenInfos.Count != 2 )
              {
                AddError( lineIndex,
                          ErrorCode.E1000_SYNTAX_ERROR,
                          "Expected two arguments to zeropage SP relative addressing opcode." );
              }
              else
              {
                if ( !EvaluateTokens( lineIndex, tokenInfos[0], lineInfo.LineCodeMapping, out SymbolInfo zeroPageValueSymbol ) )
                {
                  AddError( lineIndex,
                            m_LastErrorInfo.Code,
                            "Could not evaluate " + lineInfo.Line.Substring( m_LastErrorInfo.Pos, m_LastErrorInfo.Length ),
                            m_LastErrorInfo.Pos,
                            m_LastErrorInfo.Length );
                }
                else
                {
                  // zeropage numerand
                  long zeroPageValue = zeroPageValueSymbol.ToInteger();
                  if ( !ValidByteValue( zeroPageValue ) )
                  {
                    AddError( lineIndex,
                              Types.ErrorCode.E1002_VALUE_OUT_OF_BOUNDS_BYTE,
                              "Value out of bounds for byte, needs to be >= -128 and <= 255. Expression:"
                                + TokensToExpression( tokenInfos[0] ),
                              tokenInfos[0][0].StartPos,
                              tokenInfos[0][tokenInfos[0].Count - 1].EndPos - tokenInfos[0][0].StartPos + 1 );
                  }
                  else
                  {
                    if ( hasExpressions )
                    {
                      ApplyOpcodePatch( lineInfo, (byte)zeroPageValue, ExpressionIndex );
                    }
                    else
                    {
                      lineInfo.LineData.AppendU8( (byte)zeroPageValue );
                    }
                  }
                }
              }
            }
            else if ( lineInfo.Opcode.Addressing == Tiny64.Opcode.AddressingType.RELATIVE )
            {
              long delta = addressValue - lineInfo.AddressStart - 2;
              if ( !Valid8BitRelativeValue( delta ) )
              {
                AddError( lineIndex, Types.ErrorCode.E1100_RELATIVE_JUMP_TOO_FAR, "Relative jump too far, trying to jump " + delta + " bytes" );
                if ( !hasExpressions )
                {
                  lineInfo.LineData.AppendU8( 0 );
                }
              }
              else
              {
                if ( hasExpressions )
                {
                  ApplyOpcodePatch( lineInfo, (byte)delta, ExpressionIndex );
                }
                else
                {
                  lineInfo.LineData.AppendU8( (byte)delta );
                }
              }
            }
            else if ( lineInfo.Opcode.Addressing == Tiny64.Opcode.AddressingType.RELATIVE_16 )
            {
              long delta = 0;
              if ( m_Processor.Name == "65816" )
              {
                delta = addressValue - lineInfo.AddressStart - 3;
              }
              else
              {
                delta = addressValue - lineInfo.AddressStart - 2;
              }
              if ( !Valid16BitRelativeValue( delta ) )
              {
                AddError( lineIndex, Types.ErrorCode.E1100_RELATIVE_JUMP_TOO_FAR, "Relative jump too far, trying to jump " + delta + " bytes" );
                if ( !hasExpressions )
                {
                  lineInfo.LineData.AppendU16( 0 );
                }
              }
              else
              {
                if ( hasExpressions )
                {
                  ApplyOpcodePatch( lineInfo, (ushort)delta, ExpressionIndex );
                }
                else
                {
                  lineInfo.LineData.AppendU16( (ushort)delta );
                }
              }
            }
            else if ( lineInfo.Opcode.NumOperands == 1 )
            {
              if ( ( ( lineInfo.Opcode.Addressing == Tiny64.Opcode.AddressingType.IMMEDIATE_ACCU )
              &&     ( lineInfo.Accu16Bit ) )
              ||   ( ( lineInfo.Opcode.Addressing == Tiny64.Opcode.AddressingType.IMMEDIATE_REGISTER )
              &&     ( lineInfo.Registers16Bit ) ) )
              {
                if ( !ValidWordValue( addressValue ) )
                {
                  AddError( lineIndex,
                            Types.ErrorCode.E1003_VALUE_OUT_OF_BOUNDS_WORD,
                            "Value $" + addressValue.ToString( "X" ) + " (" + addressValue + ") is out of bounds",
                            NeededParsedExpression[0].StartPos,
                            NeededParsedExpression[NeededParsedExpression.Count - 1].EndPos - NeededParsedExpression[0].StartPos + 1 );
                  if ( !hasExpressions )
                  {
                    lineInfo.LineData.AppendU16( 0 );
                  }
                }
                else
                {
                  if ( hasExpressions )
                  {
                    ApplyOpcodePatch( lineInfo, (ushort)addressValue, ExpressionIndex );
                  }
                  else
                  {
                    lineInfo.LineData.AppendU16( (ushort)addressValue );
                  }
                }
                ++lineInfo.NumBytes;
              }
              else
              {
                if ( hasExpressions )
                {
                  ApplyOpcodePatch( lineInfo, (byte)addressValue, ExpressionIndex );
                }
                else
                {
                  lineInfo.LineData.AppendU8( (byte)addressValue );
                }
              }
            }
            else if ( lineInfo.Opcode.NumOperands == 2 )
            {
              if ( !Valid16BitAddressValue( addressValue ) )
              {
                AddError( lineIndex,
                          Types.ErrorCode.E1003_VALUE_OUT_OF_BOUNDS_WORD,
                          "Value $" + addressValue.ToString( "X" ) + " (" + value + ") is out of bounds",
                          NeededParsedExpression[0].StartPos,
                          NeededParsedExpression[NeededParsedExpression.Count - 1].EndPos - NeededParsedExpression[0].StartPos + 1 );
              }
              if ( hasExpressions )
              {
                ApplyOpcodePatch( lineInfo, (ushort)addressValue, ExpressionIndex );
              }
              else
              {
                lineInfo.LineData.AppendU16( (ushort)addressValue );
              }
            }
            else if ( lineInfo.Opcode.NumOperands == 3 )
            {
              if ( !Valid24BitAddressValue( addressValue ) )
              {
                AddError( lineIndex,
                          Types.ErrorCode.E1013_VALUE_OUT_OF_BOUNDS_24BIT,
                          "Value $" + addressValue.ToString( "X" ) + " (" + value + ") is out of bounds",
                          NeededParsedExpression[0].StartPos,
                          NeededParsedExpression[NeededParsedExpression.Count - 1].EndPos - NeededParsedExpression[0].StartPos + 1 );
              }
              if ( hasExpressions )
              {
                ApplyOpcodePatch( lineInfo, (uint)addressValue, ExpressionIndex );
              }
              else
              {
                lineInfo.LineData.AppendU24( (uint)addressValue );
              }
            }
            else if ( lineInfo.Opcode.NumOperands == 4 )
            {
              if ( !ValidDWordValue( addressValue ) )
              {
                AddError( lineIndex,
                          Types.ErrorCode.E1015_VALUE_OUT_OF_BOUNDS_32BIT,
                          "Value $" + addressValue.ToString( "X" ) + " (" + value + ") is out of bounds",
                          NeededParsedExpression[0].StartPos,
                          NeededParsedExpression[NeededParsedExpression.Count - 1].EndPos - NeededParsedExpression[0].StartPos + 1 );
              }
              if ( hasExpressions )
              {
                ApplyOpcodePatch( lineInfo, (uint)addressValue, ExpressionIndex );
              }
              else
              {
                lineInfo.LineData.AppendU32( (uint)addressValue );
              }
            }
          }
          NeededParsedExpression = null;
        }
      }
      return ParseLineResult.OK;
    }



    private bool FindForwardLocalLabel( int LineIndex, LineInfo LineInfo, string Content, out string ClosestLabel, out int ClosestLine )
    {
      ClosestLabel = "";
      ClosestLine = 5000000;

      int NumSteps = 1;

      if ( m_AssemblerSettings.LocalLabelStacking )
      {
        // look up in forward label list
        // Content.Lentgh is the number of labels to go forward (+ = the next, ++ the 2nd next, etc.)
        NumSteps  = Content.Length;
        Content   = "+";

        int nextLine = LineIndex;
        while ( NumSteps > 0 )
        {
          if ( !_ParseContext.ForwardLabelStacked.TryGetHigherKey( nextLine + 1, out nextLine ) )
          {
            return false;
          }
          --NumSteps;
        }
        ClosestLine   = nextLine;
        ClosestLabel  = _ParseContext.ForwardLabelStacked[nextLine];
        return true;
      }

      foreach ( string label in m_ASMFileInfo.Labels.Keys )
      {
        if ( ( LineInfo.NeededParsedExpression != null )
        &&   ( label.StartsWith( InternalLabelPrefix + Content + InternalLabelPostfix ) ) )
        {
          int lineNo = -1;
          if ( int.TryParse( label.Substring( ( InternalLabelPrefix + Content + InternalLabelPostfix ).Length ), out lineNo ) )
          {
            if ( ( lineNo > LineIndex )
            &&   ( lineNo < ClosestLine ) )
            {
              ClosestLine = lineNo;
              ClosestLabel = label;
            }
          }
        }
        else if ( ( LineInfo.NeededParsedExpression == null )
        &&        ( label.StartsWith( InternalLabelPrefix + InternalLabelPostfix ) ) )
        {
          int lineNo = -1;
          if ( int.TryParse( label.Substring( ( InternalLabelPrefix + InternalLabelPostfix ).Length ), out lineNo ) )
          {
            if ( ( lineNo > LineIndex )
            &&   ( lineNo < ClosestLine ) )
            {
              ClosestLine   = lineNo;
              ClosestLabel  = label;
            }
          }
        }
      }
      return ClosestLine != 5000000;
    }



    private void CleanLines( string[] Lines )
    {
      for ( int i = 0; i < Lines.Length; ++i )
      {
        string tempLine = Lines[i].TrimEnd();
        tempLine = tempLine.Replace( '\t', ' ' );
        Lines[i] = tempLine;
      }
    }



    private void PODataByte( int LineIndex, List<Types.TokenInfo> lineTokenInfos, int StartIndex, int Count, Types.ASM.LineInfo info, Types.MacroInfo.PseudoOpType Type, GR.Collections.Map<byte, byte> TextMapping, bool AllowNeededExpression )
    {
      GR.Memory.ByteBuffer data = new GR.Memory.ByteBuffer();

      int commaCount = 0;
      int firstTokenIndex = StartIndex;
      int insideOtherBrackets = 0;
      for ( int tokenIndex = StartIndex; tokenIndex < StartIndex + Count; ++tokenIndex )
      {
        string token = lineTokenInfos[tokenIndex].Content;

        if ( IsOpeningBraceChar( token ) )
        {
          ++insideOtherBrackets;
          continue;
        }
        if ( IsClosingBraceChar( token ) )
        {
          --insideOtherBrackets;
          
        }
        if ( insideOtherBrackets > 0 )
        {
          continue;
        }

        if ( ( tokenIndex == StartIndex )
        &&   ( token == "#" ) )
        {
          // direct value?
          if ( ( lineTokenInfos.Count > 2 )
          &&   ( lineTokenInfos[2].Content != "#" )
          &&   ( lineTokenInfos[2].Content != "." ) )
          {
            // not a binary value
            continue;
          }
        }

        if ( token == "," )
        {
          ++commaCount;

          if ( tokenIndex - firstTokenIndex >= 1 )
          {
            int     numBytesGiven = 0;

            if ( ( tokenIndex - firstTokenIndex == 1 )
            &&   ( lineTokenInfos[firstTokenIndex].Content == "?" ) )
            {
              AddError( info.LineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "Virtual value only allowed as single value. Expression:"
                           + TokensToExpression( lineTokenInfos, firstTokenIndex, tokenIndex - firstTokenIndex ),
                           lineTokenInfos[firstTokenIndex].StartPos,
                           lineTokenInfos[tokenIndex - 1].EndPos - lineTokenInfos[firstTokenIndex].StartPos + 1 );
            }

            if ( EvaluateTokens( LineIndex, lineTokenInfos, firstTokenIndex, tokenIndex - firstTokenIndex, TextMapping, out SymbolInfo byteValueSymbol, out numBytesGiven ) )
            {
              if ( byteValueSymbol.Type == SymbolInfo.Types.CONSTANT_STRING )
              {
                if ( ( byteValueSymbol.String.StartsWith( "\"" ) )
                &&   ( byteValueSymbol.String.Length > 1 )
                &&   ( byteValueSymbol.String.EndsWith( "\"" ) ) )
                {
                  string    textLiteral = byteValueSymbol.String.Substring( 1, byteValueSymbol.String.Length - 2 );

                  textLiteral = BasicFileParser.ReplaceAllMacrosByPETSCIICode( textLiteral, TextMapping, out bool hadError );
                  if ( hadError )
                  {
                    AddError( LineIndex, Types.ErrorCode.E3005_BASIC_UNKNOWN_MACRO, "Failed to evaluate " + textLiteral );
                    return;
                  }

                  if ( textLiteral.Length > 1 )
                  {
                    AddError( LineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "More than one character in literal is not allowed in this context" );
                    return;
                  }

                  // a text
                  foreach ( char aChar in textLiteral )
                  {
                    // map to PETSCII!
                    data.AppendU8( (byte)aChar );
                  }
                }
              }
              else
              {
                long   byteValue = byteValueSymbol.ToInteger();
                switch ( Type )
                {
                  case RetroDevStudio.Types.MacroInfo.PseudoOpType.LOW_BYTE:
                    byteValue = byteValue & 0x00ff;
                    break;
                  case RetroDevStudio.Types.MacroInfo.PseudoOpType.HIGH_BYTE:
                    byteValue = ( byteValue >> 8 ) & 0xff;
                    break;
                }
                if ( !ValidByteValue( byteValue ) )
                {
                  AddError( info.LineIndex, Types.ErrorCode.E1002_VALUE_OUT_OF_BOUNDS_BYTE, "Value out of bounds for byte, needs to be >= -128 and <= 255. Expression:"
                            + TokensToExpression( lineTokenInfos, firstTokenIndex, tokenIndex - firstTokenIndex ),
                            lineTokenInfos[firstTokenIndex].StartPos,
                            lineTokenInfos[tokenIndex - 1].EndPos - lineTokenInfos[firstTokenIndex].StartPos + 1 );
                }
                data.AppendU8( (byte)byteValue );
              }
            }
            else if ( AllowNeededExpression )
            {
              info.NeededParsedExpression = lineTokenInfos.GetRange( StartIndex, Count );
            }
            else
            {
              AddError( info.LineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Cannot evaluate expression " 
                        + TokensToExpression( lineTokenInfos, firstTokenIndex, tokenIndex - firstTokenIndex ),
                        lineTokenInfos[firstTokenIndex].StartPos,
                        lineTokenInfos[tokenIndex - 1].EndPos - lineTokenInfos[firstTokenIndex].StartPos + 1 );
            }
          }
          firstTokenIndex = tokenIndex + 1;
        }
      }
      if ( ( firstTokenIndex > 0 )
      &&   ( firstTokenIndex == lineTokenInfos.Count )
      &&   ( commaCount > 0 ) )
      {
        // last parameter has no value!
        AddError( info.LineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "Missing value after last separator."
                          + TokensToExpression( lineTokenInfos, lineTokenInfos.Count - 1, 1 ),
                          lineTokenInfos[lineTokenInfos.Count - 1].StartPos,
                          lineTokenInfos[lineTokenInfos.Count - 1].Length );
      }
         
      if ( firstTokenIndex + 1 <= lineTokenInfos.Count )
      {
        int numBytesGiven = 0;

        if ( ( lineTokenInfos.Count - firstTokenIndex == 1 )
        &&   ( lineTokenInfos[firstTokenIndex].Content == "?" ) )
        {
          info.NumBytes = 1;
          return;
        }

        if ( EvaluateTokens( LineIndex, lineTokenInfos, firstTokenIndex, lineTokenInfos.Count - firstTokenIndex, TextMapping, out SymbolInfo byteValueSymbol, out numBytesGiven ) )
        {
          if ( byteValueSymbol.Type == SymbolInfo.Types.CONSTANT_STRING )
          {
            if ( ( byteValueSymbol.String.StartsWith( "\"" ) )
            &&   ( byteValueSymbol.String.Length > 1 )
            &&   ( byteValueSymbol.String.EndsWith( "\"" ) ) )
            {
              string    textLiteral = byteValueSymbol.String.Substring( 1, byteValueSymbol.String.Length - 2 );

              textLiteral = BasicFileParser.ReplaceAllMacrosByPETSCIICode( textLiteral, TextMapping, out bool hadError );
              if ( hadError )
              {
                AddError( LineIndex, Types.ErrorCode.E3005_BASIC_UNKNOWN_MACRO, "Failed to evaluate " + textLiteral );
                return;
              }

              if ( textLiteral.Length > 1 )
              {
                AddError( LineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "More than one character in literal is not allowed in this context" );
                return;
              }

              // a text
              foreach ( char aChar in textLiteral )
              {
                // map to PETSCII!
                data.AppendU8( (byte)aChar );
              }
            }
          }
          else
          {
            long byteValue = byteValueSymbol.ToInteger();
            switch ( Type )
            {
              case RetroDevStudio.Types.MacroInfo.PseudoOpType.LOW_BYTE:
                byteValue = byteValue & 0x00ff;
                break;
              case RetroDevStudio.Types.MacroInfo.PseudoOpType.HIGH_BYTE:
                byteValue = ( byteValue >> 8 ) & 0xff;
                break;
            }
            if ( !ValidByteValue( byteValue ) )
            {
              AddError( info.LineIndex, Types.ErrorCode.E1002_VALUE_OUT_OF_BOUNDS_BYTE, "Value out of bounds for byte, needs to be >= -128 and <= 255. Expression:"
                        + TokensToExpression( lineTokenInfos, firstTokenIndex, lineTokenInfos.Count - firstTokenIndex ),
                        lineTokenInfos[firstTokenIndex].StartPos,
                        lineTokenInfos[lineTokenInfos.Count - 1].EndPos - lineTokenInfos[firstTokenIndex].StartPos + 1 );
            }
            data.AppendU8( (byte)byteValue );
          }
        }
        else if ( AllowNeededExpression )
        {
          info.NeededParsedExpression = lineTokenInfos.GetRange( StartIndex, Count );

          AdjustLabelCasing( info.NeededParsedExpression );
        }
        else
        {
          AddError( info.LineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Cannot evaluate expression "
                    + TokensToExpression( lineTokenInfos, firstTokenIndex, lineTokenInfos.Count - firstTokenIndex ),
                    lineTokenInfos[firstTokenIndex].StartPos,
                    lineTokenInfos[lineTokenInfos.Count - 1].EndPos - lineTokenInfos[firstTokenIndex].StartPos + 1 );
        }
      }
      if ( ( ( AllowNeededExpression )
      &&     ( info.NeededParsedExpression == null ) )
      ||   ( !AllowNeededExpression ) )
      {
        info.LineData = data;
      }
      info.NumBytes = commaCount + 1;
    }



    private bool IsTokenLabel( TokenInfo.TokenType Type )
    {
      if ( ( Type == TokenInfo.TokenType.LABEL_CHEAP_LOCAL )
      ||   ( Type == TokenInfo.TokenType.LABEL_GLOBAL )
      ||   ( Type == TokenInfo.TokenType.LABEL_INTERNAL )
      ||   ( Type == TokenInfo.TokenType.LABEL_LOCAL ) )
      {
        return true;
      }
      return false;
    }



    private bool ValidByteValue( long ByteValue )
    {
      if ( ( !m_CompileConfig.AutoTruncateLiteralValues )
      &&   ( ( ByteValue < -128 )
      ||     ( ByteValue > 255 ) ) )
      {
        return false;
      }
      return true;
    }



    private int FindTokenContent( List<Types.TokenInfo> lineTokenInfos, string SearchContent )
    {
      string    searchUpper = SearchContent.ToUpper();
      for ( int i = 0; i < lineTokenInfos.Count; ++i )
      {
        Types.TokenInfo   token = lineTokenInfos[i];

        if ( token.Type != RetroDevStudio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
        {
          continue;
        }
        if ( token.Content.ToUpper() == searchUpper )
        {
          return i;
        }
      }
      return -1;
    }



    private ParseLineResult HandleScopeEnd( GR.Collections.Map<GR.Generic.Tupel<string,int>, Types.MacroFunctionInfo> macroFunctions,
                                 List<Types.ScopeInfo> ScopeList,
                                 List<TokenInfo> lineTokenInfos, 
                                 GR.Collections.Map<byte, byte> TextCodeMapping,
                                 ref int lineIndex,
                                 ref int intermediateLineOffset,
                                 ref String[] Lines )
    {
      if ( ScopeList.Count == 0 )
      {
        AddError( lineIndex, Types.ErrorCode.E1007_MISSING_LOOP_START, "Missing loop start or opening bracket for " + lineTokenInfos[0].Content );
        return ParseLineResult.ERROR_ABORT;
      }

      Types.ScopeInfo   lastOpenedScope = ScopeList[ScopeList.Count - 1];

      if ( ( lastOpenedScope.Type != Types.ScopeInfo.ScopeType.MACRO_FUNCTION )
      &&   ( lastOpenedScope.Type != Types.ScopeInfo.ScopeType.LOOP )
      &&   ( lastOpenedScope.Type != Types.ScopeInfo.ScopeType.DO_UNTIL )
      &&   ( lastOpenedScope.Type != Types.ScopeInfo.ScopeType.PSEUDO_PC ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1007_MISSING_LOOP_START, "Missing loop start" );
        return ParseLineResult.ERROR_ABORT;
      }
      else if ( lastOpenedScope.Macro != null )
      {
        var macroInfo = lastOpenedScope.Macro;

        OnScopeRemoved( lineIndex, ScopeList );
        ScopeList.RemoveAt( ScopeList.Count - 1 );

        // closing a macro function
        macroInfo.LineEnd = lineIndex;

        // backup macro content
        macroInfo.Content = new string[macroInfo.LineEnd - macroInfo.LineIndex - 1];
        System.Array.Copy( Lines, macroInfo.LineIndex + 1, macroInfo.Content, 0, macroInfo.LineEnd - macroInfo.LineIndex - 1 );

        // safety check, see if macro contains call to itself
        if ( m_ASMFileInfo.FindTrueLineSource( lineIndex, out string filename, out int localLineIndex, out SourceInfo srcInfo ) )
        {
          for ( int i = 0; i < macroInfo.Content.Length; ++i )
          {
            var lineTokens = ParseTokenInfo( macroInfo.Content[i], 0, macroInfo.Content[i].Length, TextCodeMapping );
            if ( lineTokens != null )
            {
              for ( int j = 0; j < lineTokens.Count; ++j )
              {
                if ( ( lineTokens[j].Type == TokenInfo.TokenType.OPERATOR )
                &&   ( lineTokens[j].Content == "+" )
                &&   ( j + 1 < lineTokens.Count )
                &&   ( lineTokens[j].EndPos + 1 == lineTokens[j + 1].StartPos )
                &&   ( lineTokens[j + 1].Type == TokenInfo.TokenType.LABEL_GLOBAL )
                &&   ( lineTokens[j + 1].Content == macroInfo.Name ) )
                {
                  if ( m_AssemblerSettings.MacrosCanBeOverloaded )
                  {
                    int numParams = EstimateNumberOfParameters( lineTokens, 2, lineTokens.Count - 2 );
                    if ( numParams == macroInfo.ParameterNames.Count )
                    {
                      AddError( macroInfo.LineIndex + 1 + i, ErrorCode.E1302_MALFORMED_MACRO, $"Macro{macroInfo.Name} is calling itself" );
                      return ParseLineResult.ERROR_ABORT;
                    }
                  }
                  else
                  {
                    AddError( macroInfo.LineIndex + 1 + i, ErrorCode.E1302_MALFORMED_MACRO, $"Macro{macroInfo.Name} is calling itself" );
                    return ParseLineResult.ERROR_ABORT;
                  }
                }
              }
            }
          }
        }
      }
      else if ( lastOpenedScope.RepeatUntil != null )
      {
        var repeatUntil = lastOpenedScope.RepeatUntil;

        // if inside macro definition do not evaluate now!
        if ( ScopeInsideMacroDefinition( ScopeList ) )
        {
          //Debug.Log( "Loop end inside macro, do nothing, close scope" );

          OnScopeRemoved( lineIndex, ScopeList );
          ScopeList.RemoveAt( ScopeList.Count - 1 );
          return ParseLineResult.OK;
        }

        // fetch line data in between
        SourceInfoLog( "Insert repeat until block from " + Lines[repeatUntil.LineIndex] );

        // loop body
        int loopBlockLength = repeatUntil.LoopLength;
        if ( loopBlockLength == -1 )
        {
          loopBlockLength = lineIndex - repeatUntil.LineIndex - 1;
          repeatUntil.LoopLength = loopBlockLength;
        }

        // backup loop content
        if ( repeatUntil.Content == null )
        {
          repeatUntil.Content = new string[repeatUntil.LoopLength];
          System.Array.Copy( Lines, repeatUntil.LineIndex + 1, repeatUntil.Content, 0, repeatUntil.LoopLength );

          // fix up internal labels for first loop
          var lineReplacement = new string[repeatUntil.LoopLength];
          System.Array.Copy( Lines, repeatUntil.LineIndex + 1, lineReplacement, 0, repeatUntil.LoopLength );
          lineReplacement = RelabelLocalLabelsForLoop( lineReplacement, ScopeList, lineIndex, TextCodeMapping );
          System.Array.Copy( lineReplacement, 0, Lines, repeatUntil.LineIndex + 1, repeatUntil.LoopLength );

          // fix up internal labels
          repeatUntil.Content = RelabelLocalLabelsForLoop( repeatUntil.Content, ScopeList, lineIndex, TextCodeMapping );
        }

        bool  endReached = false;

        if ( !EvaluateTokens( lineIndex, lineTokenInfos, 1, lineTokenInfos.Count - 1, TextCodeMapping, out SymbolInfo resultSymbol ) )
        {
          AddError( repeatUntil.LineIndex + 1, ErrorCode.E1000_SYNTAX_ERROR, "Could not evaluate " + TokensToExpression( lineTokenInfos, 1, lineTokenInfos.Count - 1 ) );
          return ParseLineResult.ERROR_ABORT;
        }

        long result = resultSymbol.ToInteger();

        ++repeatUntil.NumRepeats;

        if ( result != 0 )
        {
          endReached = true;
        }
        if ( repeatUntil.NumRepeats >= 1000 )
        {
          AddError( repeatUntil.LineIndex + 1, ErrorCode.E1108_SAFETY_BREAK, "Safety abort: REPEAT UNTIL had more than 999 loops: " + TokensToExpression( lineTokenInfos, 1, lineTokenInfos.Count - 1 ) );
          return ParseLineResult.ERROR_ABORT;
        }

        if ( endReached )
        {
          // loop is done
          intermediateLineOffset = 0;

          OnScopeRemoved( lineIndex, ScopeList );
          ScopeList.RemoveAt( ScopeList.Count - 1 );


          // blank out !for and !end
          Lines[repeatUntil.LineIndex] = ";ex repeat";
          Lines[lineIndex] = ";ex until";

          //Debug.Log( "Cloning last loop for " + lastLoop.Label );
          CloneTempLabelsExcept( repeatUntil.LineIndex, repeatUntil.LoopLength, lineIndex - repeatUntil.LoopLength - 1, "" );
        }
        else
        {
          // copy loop content for next loop

          int linesToCopy = loopBlockLength;
          int lineLoopEndOffset = 0;

          // end reached now?
          if ( endReached )
          {
            //++linesToCopy;
            lineLoopEndOffset = 0;
          }

          DumpLines( Lines, "a" );

          string[] newLines = new string[Lines.Length + linesToCopy];

          System.Array.Copy( Lines, 0, newLines, 0, lineIndex );
          System.Array.Copy( repeatUntil.Content, 0, newLines, lineIndex, linesToCopy );
          System.Array.Copy( Lines, lineIndex + lineLoopEndOffset, newLines, lineIndex + linesToCopy, Lines.Length - lineIndex - lineLoopEndOffset );

          // fix up internal labels
          repeatUntil.Content = RelabelLocalLabelsForLoop( repeatUntil.Content, ScopeList, lineIndex, TextCodeMapping );

          //DumpLines( newLines, "b" );

          // also copy scoped variables if overlapping!!!
          if ( !endReached )
          {
            //Debug.Log( "Cloning loop " + lastLoop.CurrentValue + "/" + lastLoop.EndValue + " for " + lastLoop.Label );
            CloneTempLabelsExcept( repeatUntil.LineIndex + repeatUntil.NumRepeats * repeatUntil.LoopLength, 
                                   linesToCopy, 
                                   lineIndex - 1, "" );
          }

          // adjust source infos to make lookup work correctly
          string outerFilename = "";
          int outerLineIndex = -1;
          m_ASMFileInfo.FindTrueLineSource( repeatUntil.LineIndex, out outerFilename, out outerLineIndex );


          Types.ASM.SourceInfo sourceInfo = new Types.ASM.SourceInfo();
          //sourceInfo.Filename = ParentFilename;
          sourceInfo.Filename = outerFilename;
          sourceInfo.FullPath = outerFilename;
          sourceInfo.GlobalStartLine = lineIndex;
          sourceInfo.LineCount = linesToCopy;
          sourceInfo.LocalStartLine = outerLineIndex + 1 + intermediateLineOffset;

          if ( endReached )
          {
            intermediateLineOffset -= lineIndex - repeatUntil.LineIndex - 1;
          }

          SourceInfoLog( "Add subfile section at " + sourceInfo.LocalStartLine + " (global " + sourceInfo.GlobalStartLine + ") for " + sourceInfo.FilenameParent + " with " + sourceInfo.LineCount + " lines" );

          InsertSourceInfo( sourceInfo, true, false );

          // scheint die Ursache zu sein!!
          // clone all source infos inside the loop
          CloneSourceInfos( sourceInfo.LocalStartLine, linesToCopy, lineIndex );

          Lines = newLines;

          DumpSourceInfos( OrigLines, Lines );

          //Debug.Log( "New total " + Lines.Length + " lines" );

          // TEST TEST TEST
          //lineIndex += linesToCopy;
          return ParseLineResult.CALL_CONTINUE;
        }
      }
      else if ( lastOpenedScope.Loop != null )
      {
        var    lastLoop = lastOpenedScope.Loop;

        // if inside macro definition do not evaluate now!
        if ( ScopeInsideMacroDefinition( ScopeList ) )
        {
          //Debug.Log( "Loop end inside macro, do nothing, close scope" );

          OnScopeRemoved( lineIndex, ScopeList );
          ScopeList.RemoveAt( ScopeList.Count - 1 );
          return ParseLineResult.OK;
        }

        // fetch line data in between
        SourceInfoLog( "Insert loop block for " + lastLoop.Label + " from " + Lines[lastLoop.LineIndex] );

        // loop body
        int loopBlockLength = lastLoop.LoopLength;
        if ( loopBlockLength == -1 )
        {
          loopBlockLength = lineIndex - lastLoop.LineIndex - 1;
          lastLoop.LoopLength = loopBlockLength;
        }

        // backup loop content
        if ( lastLoop.Content == null )
        {
          lastLoop.Content = new string[lastLoop.LoopLength];
          System.Array.Copy( Lines, lastLoop.LineIndex + 1, lastLoop.Content, 0, lastLoop.LoopLength );

          // fix up internal labels for first loop
          var lineReplacement = new string[lastLoop.LoopLength];
          System.Array.Copy( Lines, lastLoop.LineIndex + 1, lineReplacement, 0, lastLoop.LoopLength );
          lineReplacement = RelabelLocalLabelsForLoop( lineReplacement, ScopeList, lineIndex, TextCodeMapping );

          for ( int j = 0; j < lineReplacement.Length; ++j )
          {
            lineReplacement[j] = lineReplacement[j] + "; loop with " + lastLoop.Label + "=" + lastLoop.CurrentValue;
          }

          System.Array.Copy( lineReplacement, 0, Lines, lastLoop.LineIndex + 1, lastLoop.LoopLength );

          // fix up internal labels
          lastLoop.Content = RelabelLocalLabelsForLoop( lastLoop.Content, ScopeList, lineIndex, TextCodeMapping );
        }

        bool  endReached = false;

        // re-evaluate end token value
        if ( !EvaluateTokens( lineIndex, lastLoop.EndValueTokens, 0, lastLoop.EndValueTokens.Count, lastLoop.EndValueTokensTextmapping, out SymbolInfo endValueSymbol ) )
        {
          AddError( lineIndex,
                    RetroDevStudio.Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION,
                    $"Could not evaluate end value for {lastLoop.IterationCount}th iteration",
                    lastLoop.EndValueTokens[0].StartPos,
                    lastLoop.EndValueTokens.Last().EndPos + 1 - lastLoop.EndValueTokens[0].StartPos );
          return ParseLineResult.ERROR_ABORT;
        }
        ++lastLoop.IterationCount;
        lastLoop.EndValue = endValueSymbol.ToInt32();
        //Debug.Log( $"Loop for {lastLoop.Label}, {lastLoop.IterationCount}th iteration, end value from {TokensToExpression( lastLoop.EndValueTokens )} = {lastLoop.EndValue}" );

        if ( ( lastLoop.CurrentValue == lastLoop.EndValue )
        ||   ( ( lastLoop.StepValue > 0 )
        &&     ( lastLoop.CurrentValue + lastLoop.StepValue > lastLoop.EndValue ) )
        ||   ( ( lastLoop.StepValue < 0 )
        &&     ( lastLoop.CurrentValue + lastLoop.StepValue < lastLoop.EndValue ) ) )
        {
          endReached = true;
        }

        if ( endReached )
        {
          // loop is done
          RemoveLabel( lastLoop.Label );

          intermediateLineOffset = 0;

          OnScopeRemoved( lineIndex, ScopeList );
          ScopeList.RemoveAt( ScopeList.Count - 1 );


          // blank out !for and !end
          Lines[lastLoop.LineIndex] = ";was for loop start for " + lastLoop.Label;
          Lines[lineIndex] = ";was loop end for " + lastLoop.Label;

          //Debug.Log( "Cloning last loop for " + lastLoop.Label );
          CloneTempLabelsExcept( lastLoop.LineIndex, lastLoop.LoopLength, lineIndex - lastLoop.LoopLength - 1, lastLoop.Label );

          //Debug.Log( "Last loop for " + lastLoop.Label + " reached" );
          //CloneSourceInfos( lastLoop.LineIndex, lastLoop.LoopLength, lineIndex - lastLoop.LoopLength );
        }
        else
        {
          lastLoop.CurrentValue += lastLoop.StepValue;

          // restart loop
          var tempLabelSymbol = CreateIntegerSymbol( lastLoop.CurrentValue );
          tempLabelSymbol.Type = SymbolInfo.Types.TEMP_LABEL;
          tempLabelSymbol.AddressOrValue = lastLoop.CurrentValue;

          //Debug.Log( $"AddTempLabel for {lastLoop.Label}, {lastLoop.IterationCount}th iteration, end value from {TokensToExpression( lastLoop.EndValueTokens )} = {lastLoop.EndValue}" );

          AddTempLabel( lastLoop.Label,
                        lineIndex,
                        lastLoop.LoopLength,
                        tempLabelSymbol,
                        "" ).IsForVariable = true;

          int linesToCopy = loopBlockLength;
          int lineLoopEndOffset = 0;

          // end reached now?
          if ( ( lastLoop.CurrentValue == lastLoop.EndValue )
          ||   ( ( lastLoop.StepValue > 0 )
          &&     ( lastLoop.CurrentValue + lastLoop.StepValue > lastLoop.EndValue ) )
          ||   ( ( lastLoop.StepValue < 0 )
          &&     ( lastLoop.CurrentValue + lastLoop.StepValue < lastLoop.EndValue ) ) )
          {
            endReached = true;
          }

          if ( endReached )
          {
            //++linesToCopy;
            lineLoopEndOffset = 0;
          }

          string[] newLines = new string[Lines.Length + linesToCopy];

          System.Array.Copy( Lines, 0, newLines, 0, lineIndex );
          System.Array.Copy( lastLoop.Content, 0, newLines, lineIndex, linesToCopy );

          for ( int j = 0; j < lastLoop.Content.Length; ++j )
          {
            newLines[lineIndex + j] = newLines[lineIndex + j] + "; loop with " + lastLoop.Label + "=" + lastLoop.CurrentValue;
          }

          System.Array.Copy( Lines, lineIndex + lineLoopEndOffset, newLines, lineIndex + linesToCopy, Lines.Length - lineIndex - lineLoopEndOffset );

          // fix up internal labels
          lastLoop.Content = RelabelLocalLabelsForLoop( lastLoop.Content, ScopeList, lineIndex, TextCodeMapping );

          //DumpLines( newLines, "b" );

          // also copy scoped variables if overlapping!!!
          //if ( !endReached )
          {
            // TODO - really required?
            CloneTempLabelsExcept( lastLoop.LineIndex, linesToCopy, lineIndex - 1, lastLoop.Label );
          }

          // adjust source infos to make lookup work correctly
          string outerFilename = "";
          int outerLineIndex = -1;
          m_ASMFileInfo.FindTrueLineSource( lastLoop.LineIndex, out outerFilename, out outerLineIndex );


          Types.ASM.SourceInfo sourceInfo = new Types.ASM.SourceInfo();
          //sourceInfo.Filename = ParentFilename;
          sourceInfo.Filename = outerFilename;
          sourceInfo.FullPath = outerFilename;
          sourceInfo.GlobalStartLine = lineIndex;
          sourceInfo.LineCount = linesToCopy;
          sourceInfo.LocalStartLine = outerLineIndex + 1 + intermediateLineOffset;

          if ( endReached )
          {
            intermediateLineOffset -= lineIndex - lastLoop.LineIndex - 1;
          }

          SourceInfoLog( "Add subfile section at " + sourceInfo.LocalStartLine + " (global " + sourceInfo.GlobalStartLine + ") for " + sourceInfo.FilenameParent + " with " + sourceInfo.LineCount + " lines" );

          InsertSourceInfo( sourceInfo, true, false );

          // scheint die Ursache zu sein!!
          // clone all source infos inside the loop
          CloneSourceInfos( sourceInfo.LocalStartLine, linesToCopy, lineIndex );

          Lines = newLines;

          DumpSourceInfos( OrigLines, Lines );

          //Debug.Log( "New total " + Lines.Length + " lines" );

          // TEST TEST TEST
          //lineIndex += linesToCopy;
          return ParseLineResult.CALL_CONTINUE;
        }
      }
      else
      {
        AddError( lineIndex, ErrorCode.E1401_INTERNAL_ERROR, "Unhandled scope/loop end" );
        return ParseLineResult.ERROR_ABORT;
      }
      return ParseLineResult.OK;
    }



    int       dumpCount = 0;
    private void DumpLines( string[] lines, string Text )
    {
      string    outName = "before" + dumpCount + ".txt";
      if ( Text == "b" )
      {
        outName = "after" + dumpCount + ".txt";
      }
      string    outPut = "Step " + dumpCount + "\r\n" + string.Join( "\r\n", lines );
      System.IO.File.WriteAllText( outName, outPut );
    }



    private string GetLoopGUID( List<Types.ScopeInfo> Scopes )
    {
      StringBuilder sb = new StringBuilder();

      foreach ( var scope in Scopes )
      {
        if ( scope.Loop != null )
        {
          sb.Append( scope.Loop.CurrentValue.ToString() + "_" );
        }
      }
      return sb.ToString();
    }



    public List<Types.TokenInfo> PrepareLineTokens( string Line, GR.Collections.Map<byte, byte> TextCodeMapping )
    {
      List<Types.TokenInfo> lineTokenInfos = ParseTokenInfo( Line, 0, Line.Length, TextCodeMapping );
      if ( HasError() )
      {
        return null;
      }
      if ( lineTokenInfos.Count == 0 )
      {
        return lineTokenInfos;
      }

      if ( ( !m_AssemblerSettings.IncludeExpectsStringLiteral )
      &&   ( m_AssemblerSettings.IncludeHasOnlyFilename ) )
      {
        // PDS style, everything after include is a file name
        if ( lineTokenInfos.Count > 1 )
        {
          if ( lineTokenInfos[0].Content.ToUpper() == "INCLUDE" )
          {
            int   tokenPos = 0;

            while ( ( tokenPos + 1 < lineTokenInfos.Count )
            &&      ( lineTokenInfos[tokenPos + 1].Type != TokenInfo.TokenType.COMMENT ) )
            {
              ++tokenPos;
            }
            string    combinedFilename = TokensToExpression( lineTokenInfos, 1, tokenPos );

            lineTokenInfos.RemoveRange( 1, lineTokenInfos.Count - 1 );

            var filenameToken = new TokenInfo();
            filenameToken.Content = combinedFilename;
            filenameToken.Type = TokenInfo.TokenType.LITERAL_STRING;
            filenameToken.StartPos = lineTokenInfos[0].EndPos + 1;
            filenameToken.OriginatingString = lineTokenInfos[0].OriginatingString;

            lineTokenInfos.Add( filenameToken );
          }
        }
      }

      if ( ( m_AssemblerSettings.EnabledHacks.ContainsValue( AssemblerSettings.Hacks.ALLOW_DOT_BYTE_INSTRUCTION ) )
      &&   ( m_AssemblerSettings.AssemblerType == AssemblerType.C64_STUDIO ) )
      {
        if ( ( lineTokenInfos.Count >= 1 )
        &&   ( lineTokenInfos[0].Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_LOCAL )
        &&   ( lineTokenInfos[0].Content.ToUpper() == ".BYTE" ) )
        {
          lineTokenInfos[0].Type = TokenInfo.TokenType.PSEUDO_OP;
          lineTokenInfos[0].Content = "!byte";
        }
        if ( ( lineTokenInfos.Count >= 1 )
        &&   ( lineTokenInfos[0].Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_LOCAL )
        &&   ( lineTokenInfos[0].Content.ToUpper() == ".WORD" ) )
        {
          lineTokenInfos[0].Type = TokenInfo.TokenType.PSEUDO_OP;
          lineTokenInfos[0].Content = "!word";
        }
        if ( ( lineTokenInfos.Count >= 2 )
        &&   ( IsLabelInFront( lineTokenInfos, lineTokenInfos[0].Content.ToUpper() ) )
        &&   ( lineTokenInfos[1].Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_LOCAL )
        &&   ( lineTokenInfos[1].Content.ToUpper() == ".BYTE" ) )
        {
          lineTokenInfos[1].Type = TokenInfo.TokenType.PSEUDO_OP;
          lineTokenInfos[1].Content = "!byte";
        }
        if ( ( lineTokenInfos.Count >= 2 )
        &&   ( IsLabelInFront( lineTokenInfos, lineTokenInfos[0].Content.ToUpper() ) )
        &&   ( lineTokenInfos[1].Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_LOCAL )
        &&   ( lineTokenInfos[1].Content.ToUpper() == ".WORD" ) )
        {
          lineTokenInfos[1].Type = TokenInfo.TokenType.PSEUDO_OP;
          lineTokenInfos[1].Content = "!word";
        }
      }
      // evaluate, could be a label in front?
      // merge + with local token for possible macro functions
      if ( ( lineTokenInfos.Count >= 2 )
      &&   ( m_AssemblerSettings.MacroFunctionCallPrefix.Contains( lineTokenInfos[0].Content ) )
      &&   ( ( lineTokenInfos[1].Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
      ||     ( lineTokenInfos[1].Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_LOCAL ) )
      &&   ( lineTokenInfos[0].StartPos + lineTokenInfos[0].Length == lineTokenInfos[1].StartPos ) )
      {
        lineTokenInfos[1].Type = RetroDevStudio.Types.TokenInfo.TokenType.CALL_MACRO;
        lineTokenInfos[1].Content = lineTokenInfos[0].Content + lineTokenInfos[1].Content;
        lineTokenInfos[1].StartPos = lineTokenInfos[0].StartPos;
        lineTokenInfos[1].Length = lineTokenInfos[1].Length + lineTokenInfos[0].Length;
        lineTokenInfos.RemoveAt( 0 );
      }

      // ++labelname should be concattenated
      if ( ( lineTokenInfos.Count >= 2 )
      &&   ( lineTokenInfos[0].Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_INTERNAL )
      &&   ( lineTokenInfos[1].Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
      &&   ( lineTokenInfos[0].StartPos + lineTokenInfos[0].Length == lineTokenInfos[1].StartPos ) )
      {
        lineTokenInfos[0].Content += lineTokenInfos[1].Content;
        lineTokenInfos[1].Length += lineTokenInfos[1].Length;
        lineTokenInfos.RemoveAt( 1 );
      }

      if ( ( lineTokenInfos[0].Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
      ||   ( lineTokenInfos[0].Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_LOCAL ) )
      {
        // could be a label in front
        if ( ( lineTokenInfos.Count >= 3 )
        &&   ( m_AssemblerSettings.MacroFunctionCallPrefix.Contains( lineTokenInfos[1].Content ) )
        &&   ( ( lineTokenInfos[2].Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
        ||     ( lineTokenInfos[2].Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_LOCAL ) )
        &&   ( lineTokenInfos[1].StartPos + lineTokenInfos[1].Length == lineTokenInfos[2].StartPos ) )
        {
          lineTokenInfos[2].Type = RetroDevStudio.Types.TokenInfo.TokenType.CALL_MACRO;
          lineTokenInfos[2].Content = lineTokenInfos[1].Content + lineTokenInfos[2].Content;
          lineTokenInfos[2].StartPos = lineTokenInfos[1].StartPos;
          lineTokenInfos[2].Length = lineTokenInfos[2].Length + lineTokenInfos[1].Length;
          lineTokenInfos.RemoveAt( 1 );
        }
      }

      // DASM has pseudo ops that look like local labels (urgh)
      foreach ( var token in lineTokenInfos )
      {
        if ( token.Type == TokenInfo.TokenType.LABEL_LOCAL )
        {
          if ( m_AssemblerSettings.PseudoOps.ContainsKey( token.Content.ToUpper() ) )
          {
            token.Type = TokenInfo.TokenType.PSEUDO_OP;
          }
        }
      }
      return lineTokenInfos;
    }



    // find macro in following lines (
    private int FindLoopEnd( string[] Lines, int StartIndex, List<Types.ScopeInfo> StackDefineBlocks, GR.Collections.Map<byte, byte> TextCodeMapping )
    {
      List<Types.ScopeInfo> stackDefineBlocks = new List<RetroDevStudio.Types.ScopeInfo>( StackDefineBlocks );

      int loopCount = 1;


      for ( int lineIndex = StartIndex; lineIndex < Lines.Length; ++lineIndex )
      {
        string parseLine = Lines[lineIndex];

        List<Types.TokenInfo> lineTokenInfos = PrepareLineTokens( parseLine, TextCodeMapping );
        if ( lineTokenInfos == null )
        {
          return -1;
        }
        if ( ( lineTokenInfos.Count > 0 )
        &&   ( lineTokenInfos[0].Content != "}" ) )
        {
          bool isActive = true;
          for ( int i = 0; i < stackDefineBlocks.Count; ++i )
          {
            if ( !stackDefineBlocks[i].Active )
            {
              isActive = false;
              break;
            }
          }
          if ( !isActive )
          {
            // defined away
            // UGH - we need to look for opening/closing braces inside non-active blocks to keep the scope stack correct
            MacroInfo   macro = null;
            if ( m_AssemblerSettings.PseudoOps.ContainsKey( lineTokenInfos[0].Content.ToUpper() ) )
            {
              macro = m_AssemblerSettings.PseudoOps[lineTokenInfos[0].Content.ToUpper()];
            }

            if ( ( macro != null )
            &&   ( ( macro.Type == MacroInfo.PseudoOpType.IFDEF )
            ||     ( macro.Type == MacroInfo.PseudoOpType.IF ) ) )
            {
              // a new block starts here!
              // false, since it doesn't matter
              Types.ScopeInfo scope = new RetroDevStudio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.IF_OR_IFDEF );
              scope.StartIndex = lineIndex;
              scope.Active = false;
              stackDefineBlocks.Add( scope );

              OnScopeAdded( scope );
            }
            else if ( ( lineTokenInfos[0].Content.ToUpper().StartsWith( "!PSEUDOPC" ) )
            &&        ( lineTokenInfos[lineTokenInfos.Count - 1].Content == "{" ) )
            {
              // ACME style pseudo pc with bracket
              Types.ScopeInfo scope = new RetroDevStudio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.PSEUDO_PC );
              scope.StartIndex = lineIndex;
              scope.Active = false;
              stackDefineBlocks.Add( scope );
              OnScopeAdded( scope );
            }
            else if ( ( lineTokenInfos[0].Content.ToUpper().StartsWith( "!ADDR" ) )
            &&        ( lineTokenInfos.Count >= 2 )
            &&        ( lineTokenInfos[1].Content == "{" ) )
            {
              // ACME style Addr with bracket
              Types.ScopeInfo scope = new RetroDevStudio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.ADDRESS );
              scope.StartIndex = lineIndex;
              scope.Active = false;
              stackDefineBlocks.Add( scope );
              OnScopeAdded( scope );
            }
            else if ( ( lineTokenInfos[0].Content.ToUpper().StartsWith( "!ZONE" ) )
            &&        ( lineTokenInfos.Count >= 2 )
            &&        ( lineTokenInfos[1].Content == "{" ) )
            {
              Types.ScopeInfo   zoneScope = new RetroDevStudio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.ZONE );
              zoneScope.StartIndex = lineIndex;
              zoneScope.Active = false;

              stackDefineBlocks.Add( zoneScope );
              OnScopeAdded( zoneScope );
            }
            continue;
          }
        }


        if ( lineTokenInfos.Count == 0 )
        {
          continue;
        }

        int     isMacroIndex = -1;

        if ( lineTokenInfos[0].Type == RetroDevStudio.Types.TokenInfo.TokenType.PSEUDO_OP )
        {
          isMacroIndex = 0;
        }
        if ( ( lineTokenInfos[0].Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
        ||   ( lineTokenInfos[0].Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_LOCAL )
        ||   ( lineTokenInfos[0].Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_INTERNAL ) )
        {
          if ( m_AssemblerSettings.PseudoOps.ContainsKey( lineTokenInfos[0].Content.ToUpper() ) )
          {
            isMacroIndex = 0;
          }
          else if ( lineTokenInfos.Count > 1 )
          {
            if ( ( lineTokenInfos[1].Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
            ||   ( lineTokenInfos[1].Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_LOCAL )
            ||   ( lineTokenInfos[1].Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_INTERNAL ) )
            {
              if ( m_AssemblerSettings.PseudoOps.ContainsKey( lineTokenInfos[1].Content.ToUpper() ) )
              {
                isMacroIndex = 1;
              }
            }
          }
        }

        if ( isMacroIndex != -1 )
        {
          Types.MacroInfo macro = m_AssemblerSettings.PseudoOps[lineTokenInfos[isMacroIndex].Content.ToUpper()];
          
          if ( macro.Type == RetroDevStudio.Types.MacroInfo.PseudoOpType.LOOP_END )
          {
            --loopCount;
            if ( loopCount == 0 )
            {
              return lineIndex;
            }
          }
          else if ( macro.Type == RetroDevStudio.Types.MacroInfo.PseudoOpType.LOOP_START )
          {
            ++loopCount;
          }
        }
      }

      return -1;
    }



    private bool IsList( List<TokenInfo> Tokens )
    {
      if ( Tokens.Count < 2 )
      {
        return false;
      }
      if ( ( Tokens[0].Content == "[" )
      &&   ( Tokens[Tokens.Count - 1].Content == "]" ) )
      {
        return true;
      }
      return false;
    }



    private bool ContainsExpression( List<TokenInfo> Tokens )
    {
      if ( Tokens.Count < 2 )
      {
        return false;
      }

      bool  hasOpen = false;
      for ( int i = 0; i < Tokens.Count; ++i )
      {
        if ( Tokens[i].Content == "[" )
        {
          hasOpen = true;
        }
        else if ( Tokens[i].Content == "]" )
        {
          if ( hasOpen )
          {
            return true;
          }
        }
      }
      return false;
    }



    private int ActualTextTokenLength( TokenInfo Token )
    {
      int     curPos = 1;
      int     macroStartPos = -1;
      int     actualLength = 0;
      bool    insideMacro = false;

      while ( ( curPos < Token.Length )
      &&      ( Token.Content[curPos] != '"' ) )
      {
        char    currentChar = Token.Content[curPos];

        if ( currentChar == '{' )
        {
          if ( insideMacro )
          {
            // malformed macro!
            Debug.Log( "ActualTextTokenLength malformed!" );
            return 0;
          }
          macroStartPos = curPos;
          insideMacro = true;
        }
        else if ( currentChar == '}' )
        {
          if ( !insideMacro )
          {
            // malformed macro!
            Debug.Log( "ActualTextTokenLength malformed!" );
            return 0;
          }
          insideMacro = false;

          string macro = Token.Content.Substring( macroStartPos + 1, curPos - macroStartPos - 1 ).ToUpper();
          int macroCount = 1;

          macro = Parser.BasicFileParser.DetermineMacroCount( macro, out macroCount );

          actualLength += macroCount;
        }
        else if ( !insideMacro )
        {
          ++actualLength;
        }
        ++curPos;
      }

      return actualLength;
    }



    private bool EvaluateTokensBinary( int LineIndex, List<TokenInfo> Tokens, Map<byte, byte> TextCodeMapping, out ByteBuffer DataOut )
    {
      DataOut = new ByteBuffer();

      int numBytes = 0;

      foreach ( var token in Tokens )
      {
        switch ( token.Type )
        {
          case TokenInfo.TokenType.LITERAL_STRING:
            if ( ( token.Content.StartsWith( "\"" ) )
            &&   ( token.Content.EndsWith( "\"" ) ) )
            {
              numBytes += token.Length - 2;

              foreach ( char aChar in token.Content.Substring( 1, token.Content.Length - 2 ) )
              {
                DataOut.AppendU8( MapTextCharacter( TextCodeMapping, (byte)aChar ) );
              }
            }
            break;
          case TokenInfo.TokenType.LITERAL_NUMBER:
            {
              if ( !ParseValue( LineIndex, token.Content, out SymbolInfo resultSymbol ) )
              {
                return false;
              }
              DataOut.AppendU8( (byte)resultSymbol.ToInteger() );
            }
            break;
          case TokenInfo.TokenType.LABEL_GLOBAL:
          case TokenInfo.TokenType.LABEL_CHEAP_LOCAL:
          case TokenInfo.TokenType.LABEL_INTERNAL:
          case TokenInfo.TokenType.LABEL_LOCAL:
            {
              if ( !ParseValue( LineIndex, token.Content, out SymbolInfo resultSymbol, out numBytes ) )
              {
                return false;
              }
              DataOut.AppendU8( (byte)resultSymbol.ToInteger() );
            }
            break;
          default:
            return false;
        }
      }

      return true;
    }



    private int CalcNumDigits( int Number )
    {
      if ( Number < 10 )
      {
        return 1;
      }
      if ( Number < 100 )
      {
        return 2;
      }
      if ( Number < 1000 )
      {
        return 3;
      }
      if ( Number < 10000 )
      {
        return 4;
      }
      if ( Number >= 100000 )
      {
        Debug.Log( "CalcNumDigits line number > 99999 encountered!" );
      }
      return 5;
    }



    private ParseLineResult POAlignDASM( List<Types.TokenInfo> lineTokenInfos, int lineIndex, Types.ASM.LineInfo info, ref int programStepPos, out int lineSizeInBytes )
    {
      // ALIGN byte boundary<,fillvalue>
      lineSizeInBytes = 0;

      List<int> tokenParams = new List<int>();
      int tokenIndex = 1;
      int expressionStartIndex = 1;

      if ( lineTokenInfos.Count < 2 )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected align <byte boundary>,[,<FillValue>]" );
        return ParseLineResult.RETURN_NULL;
      }

      do
      {
        if ( lineTokenInfos[tokenIndex].Content == "," )
        {
          // found an expression
          if ( !EvaluateTokens( lineIndex, lineTokenInfos, expressionStartIndex, tokenIndex - expressionStartIndex, info.LineCodeMapping, out SymbolInfo value ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate " + TokensToExpression( lineTokenInfos, expressionStartIndex, tokenIndex - expressionStartIndex ) );
            return ParseLineResult.RETURN_NULL;
          }          
          tokenParams.Add( value.ToInt32() );
          expressionStartIndex = tokenIndex + 1;
        }
        ++tokenIndex;
        if ( tokenIndex == lineTokenInfos.Count )
        {
          if ( expressionStartIndex <= tokenIndex - 1 )
          {
            // there's still data to evaluate
            if ( !EvaluateTokens( lineIndex, lineTokenInfos, expressionStartIndex, tokenIndex - expressionStartIndex, info.LineCodeMapping, out SymbolInfo value ) )
            {
              AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate " + TokensToExpression( lineTokenInfos, expressionStartIndex, tokenIndex - expressionStartIndex ) );
              return ParseLineResult.RETURN_NULL;
            }
            tokenParams.Add( value.ToInt32() );
          }
        }
      }
      while ( tokenIndex < lineTokenInfos.Count );

      if ( ( tokenParams.Count < 1 )
      ||   ( tokenParams.Count > 3 ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected align <byte boundary>,[,<FillValue>]" );
        return ParseLineResult.RETURN_NULL;
      }
      byte fillValue = 0;
      if ( tokenParams.Count == 2 )
      {
        if ( !ValidByteValue( tokenParams[1] ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1002_VALUE_OUT_OF_BOUNDS_BYTE, "FillValue out of bounds" );
          return ParseLineResult.RETURN_NULL;
        }
        fillValue = (byte)tokenParams[1];
      }
      info.LineData = new GR.Memory.ByteBuffer();
      while ( ( programStepPos % tokenParams[0] ) != 0 )
      {
        ++programStepPos;
        ++info.NumBytes;
        info.LineData.AppendU8( fillValue );
        if ( programStepPos == 65536 )
        {
          AddError( lineIndex, Types.ErrorCode.E1102_PROGRAM_TOO_LARGE, "Program step pos out of bounds, check !align condition" );
          return ParseLineResult.RETURN_NULL;
        }
      }
      lineSizeInBytes = info.NumBytes;

      return ParseLineResult.OK;
    }



    private ParseLineResult PODataWord( List<Types.TokenInfo> lineTokenInfos, int LineIndex, int StartIndex, int Count, Types.ASM.LineInfo info, String parseLine, bool AllowNeededExpression, bool LittleEndian, out int lineSizeInBytes )
    {
      GR.Memory.ByteBuffer data = new GR.Memory.ByteBuffer();

      int commaCount = 0;
      int firstTokenIndex = StartIndex;
      int insideOtherBrackets = 0;
      for ( int tokenIndex = StartIndex; tokenIndex < StartIndex + Count; ++tokenIndex )
      {
        string token = lineTokenInfos[tokenIndex].Content;

        if ( IsOpeningBraceChar( token ) )
        {
          ++insideOtherBrackets;
          continue;
        }
        if ( IsClosingBraceChar( token ) )
        {
          --insideOtherBrackets;

        }
        if ( insideOtherBrackets > 0 )
        {
          continue;
        }

        if ( ( tokenIndex == StartIndex )
        &&   ( token == "#" ) )
        {
          // direct value?
          if ( ( lineTokenInfos.Count > 2 )
          &&   ( lineTokenInfos[2].Content != "#" )
          &&   ( lineTokenInfos[2].Content != "." ) )
          {
            // not a binary value
            continue;
          }
        }

        if ( token == "," )
        {
          ++commaCount;

          if ( tokenIndex - firstTokenIndex >= 1 )
          {
            int     wordValue = -1;
            int     numBytesGiven = 0;

            if ( ( tokenIndex - firstTokenIndex == 1 )
            &&   ( lineTokenInfos[firstTokenIndex].Content == "?" ) )
            {
              AddError( info.LineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "Virtual value only allowed as single value. Expression:"
                           + TokensToExpression( lineTokenInfos, firstTokenIndex, tokenIndex - firstTokenIndex ),
                           lineTokenInfos[firstTokenIndex].StartPos,
                           lineTokenInfos[tokenIndex - 1].EndPos - lineTokenInfos[firstTokenIndex].StartPos + 1 );
            }

            if ( EvaluateTokens( LineIndex, lineTokenInfos, firstTokenIndex, tokenIndex - firstTokenIndex, info.LineCodeMapping, out SymbolInfo wordValueSymbol, out numBytesGiven ) )
            {
              if ( wordValueSymbol.Type == SymbolInfo.Types.CONSTANT_STRING )
              {
                if ( ( wordValueSymbol.String.StartsWith( "\"" ) )
                &&   ( wordValueSymbol.String.Length > 1 )
                &&   ( wordValueSymbol.String.EndsWith( "\"" ) ) )
                {
                  string    textLiteral = wordValueSymbol.String.Substring( 1, wordValueSymbol.String.Length - 2 );

                  textLiteral = BasicFileParser.ReplaceAllMacrosByPETSCIICode( textLiteral, info.LineCodeMapping, out bool hadError );
                  if ( hadError )
                  {
                    AddError( LineIndex, Types.ErrorCode.E3005_BASIC_UNKNOWN_MACRO, "Failed to evaluate " + textLiteral );
                    lineSizeInBytes = 0;
                    return ParseLineResult.ERROR_ABORT;
                  }

                  // a text
                  foreach ( char aChar in textLiteral )
                  {
                    // map to PETSCII!
                    if ( LittleEndian )
                    {
                      data.AppendU16( (byte)aChar );
                    }
                    else
                    {
                      data.AppendU16NetworkOrder( (byte)aChar );
                    }
                  }
                }
              }
              else
              {
                wordValue = wordValueSymbol.ToInt32();
                if ( !ValidWordValue( wordValue ) )
                {
                  AddError( info.LineIndex,
                            Types.ErrorCode.E1003_VALUE_OUT_OF_BOUNDS_WORD,
                            "Value out of bounds for word, needs to be >= -32768 and <= 65535. Expression:" + TokensToExpression( lineTokenInfos, firstTokenIndex, tokenIndex - firstTokenIndex ),
                            lineTokenInfos[firstTokenIndex].StartPos,
                            lineTokenInfos[tokenIndex - 1].EndPos - lineTokenInfos[firstTokenIndex].StartPos + 1 );
                }
                if ( LittleEndian )
                {
                  data.AppendU16( (ushort)wordValue );
                }
                else
                {
                  data.AppendU16NetworkOrder( (ushort)wordValue );
                }
              }
            }
            else if ( AllowNeededExpression )
            {
              info.NeededParsedExpression = lineTokenInfos.GetRange( StartIndex, Count );
            }
            else
            {
              AddError( info.LineIndex,
                          Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION,
                          "Failed to evaluate expression " + TokensToExpression( lineTokenInfos, firstTokenIndex, tokenIndex - firstTokenIndex ),
                          lineTokenInfos[firstTokenIndex].StartPos,
                          lineTokenInfos[tokenIndex - 1].EndPos - lineTokenInfos[firstTokenIndex].StartPos + 1 );
            }
          }
          firstTokenIndex = tokenIndex + 1;
        }
      }
      if ( ( firstTokenIndex > 0 )
      &&   ( firstTokenIndex == lineTokenInfos.Count )
      &&   ( commaCount > 0 ) )
      {
        // last parameter has no value!
        AddError( info.LineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "Missing value after last separator."
                          + TokensToExpression( lineTokenInfos, lineTokenInfos.Count - 1, 1 ),
                          lineTokenInfos[lineTokenInfos.Count - 1].StartPos,
                          lineTokenInfos[lineTokenInfos.Count - 1].Length );
      }
      if ( firstTokenIndex + 1 <= lineTokenInfos.Count )
      {
        int wordValue = -1;
        int numBytesGiven = 0;

        if ( ( lineTokenInfos.Count - firstTokenIndex == 1 )
        &&   ( lineTokenInfos[firstTokenIndex].Content == "?" ) )
        {
          info.NumBytes = 2;
          lineSizeInBytes = 2;
          return ParseLineResult.ERROR_ABORT;
        }

        if ( EvaluateTokens( LineIndex, lineTokenInfos, firstTokenIndex, lineTokenInfos.Count - firstTokenIndex, info.LineCodeMapping, out SymbolInfo wordValueSymbol, out numBytesGiven ) )
        {
          if ( wordValueSymbol.Type == SymbolInfo.Types.CONSTANT_STRING )
          {
            if ( ( wordValueSymbol.String.StartsWith( "\"" ) )
            &&   ( wordValueSymbol.String.Length > 1 )
            &&   ( wordValueSymbol.String.EndsWith( "\"" ) ) )
            {
              string    textLiteral = wordValueSymbol.String.Substring( 1, wordValueSymbol.String.Length - 2 );

              textLiteral = BasicFileParser.ReplaceAllMacrosByPETSCIICode( textLiteral, info.LineCodeMapping, out bool hadError );
              if ( hadError )
              {
                AddError( LineIndex, Types.ErrorCode.E3005_BASIC_UNKNOWN_MACRO, "Failed to evaluate " + textLiteral );
                lineSizeInBytes = 0;
                return ParseLineResult.ERROR_ABORT;
              }

              // a text
              foreach ( char aChar in textLiteral )
              {
                // map to PETSCII!
                if ( LittleEndian )
                {
                  data.AppendU16( (byte)aChar );
                }
                else
                {
                  data.AppendU16NetworkOrder( (byte)aChar );
                }
              }
            }
          }
          else
          {
            wordValue = wordValueSymbol.ToInt32();
            if ( !ValidWordValue( wordValue ) )
            {
              AddError( info.LineIndex,
                        Types.ErrorCode.E1003_VALUE_OUT_OF_BOUNDS_WORD,
                        "Value out of bounds for word, needs to be >= -32768 and <= 65535. Expression:" + TokensToExpression( lineTokenInfos, firstTokenIndex, lineTokenInfos.Count - firstTokenIndex ),
                        lineTokenInfos[firstTokenIndex].StartPos,
                        lineTokenInfos[lineTokenInfos.Count - 1].EndPos - lineTokenInfos[firstTokenIndex].StartPos + 1 );
            }
            if ( LittleEndian )
            {
              data.AppendU16( (ushort)wordValue );
            }
            else
            {
              data.AppendU16NetworkOrder( (ushort)wordValue );
            }
          }
        }
        else if ( AllowNeededExpression )
        {
          info.NeededParsedExpression = lineTokenInfos.GetRange( StartIndex, Count );
        }
        else
        {
          AddError( info.LineIndex,
                      Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION,
                      "Failed to evaluate expression " + TokensToExpression( lineTokenInfos, firstTokenIndex, lineTokenInfos.Count - firstTokenIndex ),
                      lineTokenInfos[firstTokenIndex].StartPos,
                      lineTokenInfos[lineTokenInfos.Count - 1].EndPos - lineTokenInfos[firstTokenIndex].StartPos + 1 );
        }
      }
      // TODO - this is a ugly check if there was an error or not
      if ( ( ( AllowNeededExpression )
      &&     ( info.NeededParsedExpression == null ) )
      ||   ( !AllowNeededExpression ) )
      {
        info.LineData = data;
      }
      info.NumBytes = 2 * ( 1 + commaCount );
      info.Line = parseLine;
      lineSizeInBytes = info.NumBytes;
      return ParseLineResult.OK;
    }



    private ParseLineResult PODataDWord( List<Types.TokenInfo> lineTokenInfos, int LineIndex, int StartIndex, int Count, Types.ASM.LineInfo info, String parseLine, bool AllowNeededExpression, bool LittleEndian, out int lineSizeInBytes )
    {
      GR.Memory.ByteBuffer data = new GR.Memory.ByteBuffer();

      int commaCount = 0;
      int firstTokenIndex = StartIndex;
      int insideOtherBrackets = 0;
      for ( int tokenIndex = StartIndex; tokenIndex < StartIndex + Count; ++tokenIndex )
      {
        string token = lineTokenInfos[tokenIndex].Content;

        if ( IsOpeningBraceChar( token ) )
        {
          ++insideOtherBrackets;
          continue;
        }
        if ( IsClosingBraceChar( token ) )
        {
          --insideOtherBrackets;

        }
        if ( insideOtherBrackets > 0 )
        {
          continue;
        }

        if ( ( tokenIndex == StartIndex )
        &&   ( token == "#" ) )
        {
          // direct value?
          if ( ( lineTokenInfos.Count > 2 )
          &&   ( lineTokenInfos[2].Content != "#" )
          &&   ( lineTokenInfos[2].Content != "." ) )
          {
            // not a binary value
            continue;
          }
        }

        if ( token == "," )
        {
          ++commaCount;

          if ( tokenIndex - firstTokenIndex >= 1 )
          {
            int     wordValue = -1;
            int     numBytesGiven = 0;

            if ( ( tokenIndex - firstTokenIndex == 1 )
            &&   ( lineTokenInfos[firstTokenIndex].Content == "?" ) )
            {
              AddError( info.LineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "Virtual value only allowed as single value. Expression:"
                           + TokensToExpression( lineTokenInfos, firstTokenIndex, tokenIndex - firstTokenIndex ),
                           lineTokenInfos[firstTokenIndex].StartPos,
                           lineTokenInfos[tokenIndex - 1].EndPos - lineTokenInfos[firstTokenIndex].StartPos + 1 );
            }

            if ( EvaluateTokens( LineIndex, lineTokenInfos, firstTokenIndex, tokenIndex - firstTokenIndex, info.LineCodeMapping, out SymbolInfo wordValueSymbol, out numBytesGiven ) )
            {
              if ( wordValueSymbol.Type == SymbolInfo.Types.CONSTANT_STRING )
              {
                if ( ( wordValueSymbol.String.StartsWith( "\"" ) )
                &&   ( wordValueSymbol.String.Length > 1 )
                &&   ( wordValueSymbol.String.EndsWith( "\"" ) ) )
                {
                  string    textLiteral = wordValueSymbol.String.Substring( 1, wordValueSymbol.String.Length - 2 );

                  textLiteral = BasicFileParser.ReplaceAllMacrosByPETSCIICode( textLiteral, info.LineCodeMapping, out bool hadError );
                  if ( hadError )
                  {
                    AddError( LineIndex, Types.ErrorCode.E3005_BASIC_UNKNOWN_MACRO, "Failed to evaluate " + textLiteral );
                    lineSizeInBytes = 0;
                    return ParseLineResult.ERROR_ABORT;
                  }

                  // a text
                  foreach ( char aChar in textLiteral )
                  {
                    // map to PETSCII!
                    if ( LittleEndian )
                    {
                      data.AppendU32( (byte)aChar );
                    }
                    else
                    {
                      data.AppendU32NetworkOrder( (byte)aChar );
                    }
                  }
                }
              }
              else
              {
                wordValue = wordValueSymbol.ToInt32();
                if ( !ValidDWordValue( wordValue ) )
                {
                  AddError( info.LineIndex,
                            Types.ErrorCode.E1003_VALUE_OUT_OF_BOUNDS_WORD,
                            "Value out of bounds for word, needs to be >= −2147483648 and <= 4294967295. Expression:" + TokensToExpression( lineTokenInfos, firstTokenIndex, tokenIndex - firstTokenIndex ),
                            lineTokenInfos[firstTokenIndex].StartPos,
                            lineTokenInfos[tokenIndex - 1].EndPos - lineTokenInfos[firstTokenIndex].StartPos + 1 );
                }
                if ( LittleEndian )
                {
                  data.AppendU32( (uint)wordValue );
                }
                else
                {
                  data.AppendU32NetworkOrder( (uint)wordValue );
                }
              }
            }
            else if ( AllowNeededExpression )
            {
              info.NeededParsedExpression = lineTokenInfos.GetRange( StartIndex, Count );
            }
            else
            {
              AddError( info.LineIndex,
                          Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION,
                          "Failed to evaluate expression " + TokensToExpression( lineTokenInfos, firstTokenIndex, tokenIndex - firstTokenIndex ),
                          lineTokenInfos[firstTokenIndex].StartPos,
                          lineTokenInfos[tokenIndex - 1].EndPos - lineTokenInfos[firstTokenIndex].StartPos + 1 );
            }
          }
          firstTokenIndex = tokenIndex + 1;
        }
      }
      if ( ( firstTokenIndex > 0 )
      &&   ( firstTokenIndex == lineTokenInfos.Count )
      &&   ( commaCount > 0 ) )
      {
        // last parameter has no value!
        AddError( info.LineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "Missing value after last separator."
                          + TokensToExpression( lineTokenInfos, lineTokenInfos.Count - 1, 1 ),
                          lineTokenInfos[lineTokenInfos.Count - 1].StartPos,
                          lineTokenInfos[lineTokenInfos.Count - 1].Length );
      }
      if ( firstTokenIndex + 1 <= lineTokenInfos.Count )
      {
        int wordValue = -1;
        int numBytesGiven = 0;

        if ( ( lineTokenInfos.Count - firstTokenIndex == 1 )
        &&   ( lineTokenInfos[firstTokenIndex].Content == "?" ) )
        {
          info.NumBytes = 4;
          lineSizeInBytes = 4;
          return ParseLineResult.ERROR_ABORT;
        }

        if ( EvaluateTokens( LineIndex, lineTokenInfos, firstTokenIndex, lineTokenInfos.Count - firstTokenIndex, info.LineCodeMapping, out SymbolInfo wordValueSymbol, out numBytesGiven ) )
        {
          if ( wordValueSymbol.Type == SymbolInfo.Types.CONSTANT_STRING )
          {
            if ( ( wordValueSymbol.String.StartsWith( "\"" ) )
            &&   ( wordValueSymbol.String.Length > 1 )
            &&   ( wordValueSymbol.String.EndsWith( "\"" ) ) )
            {
              string    textLiteral = wordValueSymbol.String.Substring( 1, wordValueSymbol.String.Length - 2 );

              textLiteral = BasicFileParser.ReplaceAllMacrosByPETSCIICode( textLiteral, info.LineCodeMapping, out bool hadError );
              if ( hadError )
              {
                AddError( LineIndex, Types.ErrorCode.E3005_BASIC_UNKNOWN_MACRO, "Failed to evaluate " + textLiteral );
                lineSizeInBytes = 0;
                return ParseLineResult.ERROR_ABORT;
              }

              // a text
              foreach ( char aChar in textLiteral )
              {
                // map to PETSCII!
                if ( LittleEndian )
                {
                  data.AppendU32( (byte)aChar );
                }
                else
                {
                  data.AppendU32NetworkOrder( (byte)aChar );
                }
              }
            }
          }
          else
          {
            wordValue = wordValueSymbol.ToInt32();
            if ( !ValidDWordValue( wordValue ) )
            {
              AddError( info.LineIndex,
                        Types.ErrorCode.E1003_VALUE_OUT_OF_BOUNDS_WORD,
                        "Value out of bounds for dword, needs to be >= −2147483648 and <= 4294967295. Expression:" + TokensToExpression( lineTokenInfos, firstTokenIndex, lineTokenInfos.Count - firstTokenIndex ),
                        lineTokenInfos[firstTokenIndex].StartPos,
                        lineTokenInfos[lineTokenInfos.Count - 1].EndPos - lineTokenInfos[firstTokenIndex].StartPos + 1 );
            }
            if ( LittleEndian )
            {
              data.AppendU32( (uint)wordValue );
            }
            else
            {
              data.AppendU32NetworkOrder( (uint)wordValue );
            }
          }
        }
        else if ( AllowNeededExpression )
        {
          info.NeededParsedExpression = lineTokenInfos.GetRange( StartIndex, Count );
        }
        else
        {
          AddError( info.LineIndex,
                      Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION,
                      "Failed to evaluate expression " + TokensToExpression( lineTokenInfos, firstTokenIndex, lineTokenInfos.Count - firstTokenIndex ),
                      lineTokenInfos[firstTokenIndex].StartPos,
                      lineTokenInfos[lineTokenInfos.Count - 1].EndPos - lineTokenInfos[firstTokenIndex].StartPos + 1 );
        }
      }
      // TODO - this is a ugly check if there was an error or not
      if ( ( ( AllowNeededExpression )
      &&     ( info.NeededParsedExpression == null ) )
      ||   ( !AllowNeededExpression ) )
      {
        info.LineData = data;
      }
      info.NumBytes = 4 * ( 1 + commaCount );
      info.Line = parseLine;
      lineSizeInBytes = info.NumBytes;
      return ParseLineResult.OK;
    }



    private bool Valid8BitRelativeValue( long Delta )
    {
      if ( ( Delta < -128 )
      ||   ( Delta > 127 ) )
      {
        return false;
      }
      return true;
    }



    private bool Valid16BitRelativeValue( long Delta )
    {
      if ( ( Delta < -32768 )
      ||   ( Delta > 32767 ) )
      {
        return false;
      }
      return true;
    }



    private bool Valid16BitAddressValue( long AddressValue )
    {
      if ( ( AddressValue < 0 )
      ||   ( AddressValue > 0xffff ) )
      {
        return false;
      }
      return true;
    }



    private bool Valid24BitAddressValue( long AddressValue )
    {
      if ( ( AddressValue < 0 )
      ||   ( AddressValue > 0xffffff ) )
      {
        return false;
      }
      return true;
    }



    private bool ValidWordValue( long WordValue )
    {
      if ( ( !m_CompileConfig.AutoTruncateLiteralValues )
      &&   ( ( WordValue < short.MinValue )
      ||     ( WordValue > ushort.MaxValue ) ) )
      {
        return false;
      }
      return true;
    }



    private bool Valid24BitValue( long Value24 )
    {
      if ( ( !m_CompileConfig.AutoTruncateLiteralValues )
      &&   ( ( Value24 < -8388608 )
      ||     ( Value24 > 0x00ffffff ) ) )
      {
        return false;
      }
      return true;
    }
    
    
    
    private bool ValidDWordValue( long DWordValue )
    {
      if ( ( !m_CompileConfig.AutoTruncateLiteralValues )
      &&   ( ( DWordValue < int.MinValue )
      ||     ( DWordValue > uint.MaxValue ) ) )
      {
        return false;
      }
      return true;
    }
    
    
    
    private string BuildFullPath( string ParentPath, string SubFilename )
    {
      if ( System.IO.Path.IsPathRooted( SubFilename ) )
      {
        return SubFilename;
      }
      return GR.Path.Append( ParentPath, SubFilename );
    }



    private string DetermineFullLibraryFilePath( string subFilename )
    {
      foreach ( var libFile in m_CompileConfig.LibraryFiles )
      {
        string    fullBasePath = libFile;
        if ( !System.IO.Path.IsPathRooted( libFile ) )
        {
#if DEBUG
          fullBasePath = System.IO.Path.GetFullPath( "../../" + libFile );
#else
          fullBasePath = System.IO.Path.GetFullPath( libFile );
#endif
        }
        if ( System.IO.File.Exists( System.IO.Path.Combine( fullBasePath, subFilename ) ) )
        {
          return System.IO.Path.Combine( fullBasePath, subFilename );
        }

      }
      return "";
    }



    private bool ScopeInsideMacroDefinition( List<Types.ScopeInfo> Scopes )
    {
      for ( int i = Scopes.Count - 1; i >= 0; --i )
      {
        if ( Scopes[i].Macro != null )
        {
          return true;
        }
      }
      return false;
    }



    private bool ScopeInsideLoop( List<Types.ScopeInfo> Scopes )
    {
      for ( int i = Scopes.Count - 1; i >= 0; --i )
      {
        if ( ( Scopes[i].Loop != null )
        ||   ( Scopes[i].RepeatUntil != null ) )
        {
          return true;
        }
      }
      return false;
    }



    Dictionary<string,string[]>   OrigLines = null;



    private void IncludePreviousSymbols()
    {
      // include previous symbols
      if ( InitialFileInfo != null )
      {
        foreach ( var entry in InitialFileInfo.Labels )
        {
          if ( ( entry.Value.Type != SymbolInfo.Types.PREPROCESSOR_CONSTANT_1 )
          &&   ( entry.Value.Type != SymbolInfo.Types.PREPROCESSOR_CONSTANT_2 )
          &&   ( entry.Value.Type != SymbolInfo.Types.PREPROCESSOR_LABEL ) )
          {
            if ( ( entry.Value.Type == SymbolInfo.Types.LABEL )
            &&   ( entry.Key.StartsWith( InternalLabelPrefix ) ) )
            {
              // do not pass on internal local labels
              continue;
            }
            var symbol = new SymbolInfo();
            symbol.AddressOrValue = entry.Value.AddressOrValue;
            symbol.DocumentFilename = entry.Value.DocumentFilename;
            symbol.LocalLineIndex = entry.Value.LocalLineIndex;
            symbol.Name = entry.Value.Name;
            symbol.Type = entry.Value.Type;
            symbol.References.Add( entry.Value.LineIndex );
            symbol.Zone = entry.Value.Zone;
            symbol.FromDependency = true;
            symbol.Info           = entry.Value.Info;
            symbol.SourceInfo     = entry.Value.SourceInfo;

            m_ASMFileInfo.Labels.Add( entry.Key, symbol );
          }
        }
      }
    }



    private bool IsKnownLabel( TokenInfo Token )
    {
      if ( !IsTokenLabel( Token.Type ) )
      {
        return false;
      }
      return ( m_ASMFileInfo.Labels.ContainsKey( Token.Content ) )
          || ( m_ASMFileInfo.UnparsedLabels.ContainsKey( Token.Content ) );
    }



    private string[] PreProcess( string[] Lines, string ParentFilename, ProjectConfig Configuration, string AdditionalPredefines, out bool HadFatalError )
    {
      _ParseContext = new ParseContext();
      var stackScopes = new List<RetroDevStudio.Types.ScopeInfo>();

      m_ASMFileInfo.Labels.Clear();
      m_CurrentCommentSB = new StringBuilder();
      HadFatalError = false;

      if ( DoLogSourceInfo )
      {
        OrigLines = new Dictionary<string, string[]>();
        OrigLines[ParentFilename] = new string[Lines.Length];
        Array.Copy( Lines, OrigLines[ParentFilename], Lines.Length );
      }

      IncludePreviousSymbols();

      m_ASMFileInfo.UnparsedLabels.Clear();
      m_ASMFileInfo.Zones.Clear();
      m_ASMFileInfo.LineInfo.Clear();
      m_ASMFileInfo.TempLabelInfo.Clear();
      m_ASMFileInfo.Processor = Tiny64.Processor.Create6510();
      m_ASMFileInfo.Macros    = new Map<GR.Generic.Tupel<string, int>, MacroFunctionInfo>();

      stackScopes.Clear();
      m_ASMFileInfo.Messages.Clear();

      m_WarningMessages = 0;
      m_ErrorMessages = 0;

      // default text code mapping
      var textCodeMapping = m_TextCodeMappingRaw;
      if ( AssemblerSettings.AllowsCustomTextMappings )
      {
        _ParseContext.TextMappings["none"]    = m_TextCodeMappingRaw;
        _ParseContext.TextMappings["screen"]  = m_TextCodeMappingScr;
      }

      if ( !string.IsNullOrEmpty( AdditionalPredefines ) )
      {
        ParseAndAddPreDefines( AdditionalPredefines, textCodeMapping );
      }
      if ( Configuration != null )
      {
        ParseAndAddPreDefines( Configuration.Defines, textCodeMapping );
      }
      AddPreprocessorConstant( "ASSEMBLER_C64STUDIO", 1, -1 );
      AddPreprocessorConstant( "ASSEMBLER_RETRODEVSTUDIO", 1, -1 );

      Dictionary<string,string> previousMinusLabel = new Dictionary<string, string>();
      SortedList<int,string>    previousMinusLabelStacked = new SortedList<int, string>();
      

      int   programStepPos = -1;
      int   lineSizeInBytes = 0;
      int   sizeInBytes = 0;
      m_CompileCurrentAddress = -1;
      int trueCompileCurrentAddress = -1;
      string cheapLabelParent = "";
      int   intermediateLineOffset = 0;
      bool  hadCommentInLine = false;
      bool hadPseudoOp = false;
      bool hideInPreprocessedOutput = false;
      m_CurrentZoneName = "";
      m_CurrentGlobalZoneName = "";
      m_CurrentSegmentIsVirtual = false;

      for ( int lineIndex = 0; lineIndex < Lines.Length; ++lineIndex )
      {
        string parseLine = "";
        if ( Lines[lineIndex] != null )
        {
          parseLine = Lines[lineIndex].TrimEnd();
        }

        lineSizeInBytes = 0;
        hadCommentInLine = false;
        hadPseudoOp = false;

        int commentPos = -1;

        if ( FindStartOfComment( parseLine, out commentPos ) )
        {
          m_CurrentCommentSB.AppendLine( parseLine.Substring( commentPos + 1 ) );
          parseLine = parseLine.Substring( 0, commentPos );
          hadCommentInLine = true;
        }

        Types.ASM.LineInfo info       = new Types.ASM.LineInfo();
        info.LineIndex                = lineIndex;
        info.Zone                     = m_CurrentZoneName;
        info.CheapLabelZone           = cheapLabelParent;
        info.AddressStart             = programStepPos;
        info.HideInPreprocessedOutput = hideInPreprocessedOutput;
        info.Accu16Bit                = m_Assume16BitAccu;
        info.Registers16Bit           = m_Assume16BitRegisters;

        if ( !ScopeInsideMacroDefinition( stackScopes ) )
        {
          // do not store code inside a macro definition
          if ( ScopeInsideLoop( stackScopes ) )
          {
            // if a loop is repeated line infos may already exist
            if ( m_ASMFileInfo.LineInfo.ContainsKey( lineIndex ) )
            {
              // TODO - add second address lookup entry
              info = m_ASMFileInfo.LineInfo[lineIndex];

              info.AddressStart = programStepPos;
              info.Zone         = m_CurrentZoneName;
            }
            else
            {
              m_ASMFileInfo.LineInfo.Add( lineIndex, info );
            }
          }
          else
          {
            m_ASMFileInfo.LineInfo.Add( lineIndex, info );
          }
        }

        List<Types.TokenInfo> lineTokenInfos = PrepareLineTokens( parseLine, textCodeMapping );
        if ( lineTokenInfos == null )
        {
          AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1000_SYNTAX_ERROR, "Syntax error at position " + ( m_LastErrorInfo.Pos + 1 ).ToString() + " (" + parseLine[m_LastErrorInfo.Pos] + ")" );
          continue;
        }

        recheck_line:
        ;

        // split lines by ':'
        int   localIndex = 0;
        string filename = "";
        if ( !m_ASMFileInfo.FindTrueLineSource( lineIndex, out filename, out localIndex ) )
        {
          DumpSourceInfos( OrigLines, Lines );
          AddError( lineIndex, Types.ErrorCode.E1401_INTERNAL_ERROR, "Can't determine filename from line" );

          HadFatalError = true;
          return Lines;
        }
        var handleSeparatorsResult = HandleLineSeparators( ref lineIndex, lineTokenInfos, ref Lines, filename );
        if ( handleSeparatorsResult == ParseLineResult.CALL_CONTINUE )
        {
          continue;
        }

        if ( lineTokenInfos.Count == 0 )
        {
          if ( !hadCommentInLine )
          {
            m_CurrentCommentSB = new StringBuilder();
          }
          continue;
        }

        AdjustLabelCasing( lineTokenInfos );

        // PDS/DASM macro call?
        DetectPDSOrDASMMacroCall( m_ASMFileInfo.Macros, lineTokenInfos );

        // do we have a DASM scope operator? (must skip scope check then)
        bool  isDASMScopePseudoOP = false;
        int   tokenOffset = 0;
        if ( ( lineTokenInfos.Count > 1 )
        &&   ( !m_AssemblerSettings.PseudoOps.ContainsKey( lineTokenInfos[0].Content.ToUpper() ) )
        &&   ( IsTokenLabel( lineTokenInfos[0].Type ) ) )
        {
          ++tokenOffset;
        }
        if ( ( IsTokenLabel( lineTokenInfos[0].Type  ) )
        &&   ( m_AssemblerSettings.PseudoOps.ContainsKey( lineTokenInfos[tokenOffset].Content.ToUpper() ) ) )
        {
          var macroInfo = m_AssemblerSettings.PseudoOps[lineTokenInfos[tokenOffset].Content.ToUpper()];
          if ( ( macroInfo.Type == RetroDevStudio.Types.MacroInfo.PseudoOpType.IF )
          ||   ( macroInfo.Type == RetroDevStudio.Types.MacroInfo.PseudoOpType.IFNDEF )
          ||   ( macroInfo.Type == RetroDevStudio.Types.MacroInfo.PseudoOpType.IFDEF )
          ||   ( macroInfo.Type == RetroDevStudio.Types.MacroInfo.PseudoOpType.ELSE )
          ||   ( macroInfo.Type == RetroDevStudio.Types.MacroInfo.PseudoOpType.END_IF ) )
          {
            isDASMScopePseudoOP = true;
          }
        }

        bool isScopingActive = true;
        bool isOuterScopingActive = true;
        for ( int i = 0; i < stackScopes.Count; ++i )
        {
          if ( !stackScopes[i].Active )
          {
            if ( ( i + 1 ) < stackScopes.Count )
            {
              isOuterScopingActive = false;
            }
            isScopingActive = false;
            break;
          }
        }

        if ( ( lineTokenInfos.Count > 0 )
        &&   ( !TokenIsConditionalThatEndsScope( lineTokenInfos[tokenOffset] ) )
        &&   ( !isDASMScopePseudoOP ) )
        {
          if ( !isScopingActive )
          {
            // defined away
            Types.ScopeInfo.ScopeType   detectedScopeType = ScopeInfo.ScopeType.UNKNOWN;

            // TODO - HACK UGLY - use keywords from AssemblerSettings!
            if ( TokenIsConditionalThatStartsScope( lineTokenInfos[tokenOffset] ) )
            {
              // a new block starts here!
              // false, since it doesn't matter
              Types.ScopeInfo scope = new RetroDevStudio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.IF_OR_IFDEF );
              scope.StartIndex = lineIndex;
              scope.Active = false;
              stackScopes.Add( scope );

              OnScopeAdded( scope );
            }
            else if ( ( TokenIsPseudoPC( lineTokenInfos[tokenOffset] ) )
            &&        ( lineTokenInfos[lineTokenInfos.Count - 1].Content == "{" ) )
            {
              // ACME style pseudo pc with bracket
              Types.ScopeInfo scope = new ScopeInfo( Types.ScopeInfo.ScopeType.PSEUDO_PC );
              scope.StartIndex = lineIndex;
              scope.Active = false;
              stackScopes.Add( scope );
              OnScopeAdded( scope );
            }
            else if ( ( TokenStartsScope( lineTokenInfos, tokenOffset, out detectedScopeType ) )
            &&        ( lineTokenInfos[lineTokenInfos.Count - 1].Content == "{" ) )
            {
              // ACME style other scopes with bracket
              Types.ScopeInfo scope = new RetroDevStudio.Types.ScopeInfo( detectedScopeType );
              scope.StartIndex = lineIndex;
              scope.Active = false;

              if ( detectedScopeType == ScopeInfo.ScopeType.MACRO_FUNCTION )
              {
                scope.Macro = new MacroFunctionInfo();
                scope.Macro.UsesBracket = true;
              }

              stackScopes.Add( scope );
              OnScopeAdded( scope );
            }

            info.Line = "";
            info.NumBytes = 0;

            //++lineIndex;
            continue;
            //parseLine = ";" + parseLine;
          }
        }

        bool      evaluatedContent = false;
        if ( lineTokenInfos[0].Content.StartsWith( "-" ) )
        {
          if ( ( m_AssemblerSettings.LocalLabelStacking )
          &&   ( lineTokenInfos[0].Content.Length > 1 ) )
          {
            AddError( lineIndex, ErrorCode.E1000_SYNTAX_ERROR, "Local labels (+/n) must be single character", lineTokenInfos[0].StartPos, lineTokenInfos[0].Length );
          }
          else if ( m_AssemblerSettings.LocalLabelStacking )
          {
            previousMinusLabelStacked.Add( lineIndex, InternalLabelPrefix + lineTokenInfos[0].Content + InternalLabelPostfix + lineIndex.ToString() );
          }
          else
          {
            if ( !previousMinusLabel.ContainsKey( lineTokenInfos[0].Content ) )
            {
              previousMinusLabel.Add( lineTokenInfos[0].Content, InternalLabelPrefix + lineTokenInfos[0].Content + InternalLabelPostfix + lineIndex.ToString() );
            }
            else
            {
              previousMinusLabel[lineTokenInfos[0].Content] = InternalLabelPrefix + lineTokenInfos[0].Content + InternalLabelPostfix + lineIndex.ToString();
            }
          }
        }
        if ( ( lineTokenInfos[0].Content.StartsWith( "+" ) )
        &&   ( m_AssemblerSettings.LocalLabelStacking ) )
        {
          if ( lineTokenInfos[0].Content.Length > 1 )
          {
            AddError( lineIndex, ErrorCode.E1000_SYNTAX_ERROR, "Local labels (+/n) must be single character", lineTokenInfos[0].StartPos, lineTokenInfos[0].Length );
          }
          else
          {
            _ParseContext.ForwardLabelStacked.Add( lineIndex, InternalLabelPrefix + lineTokenInfos[0].Content + InternalLabelPostfix + lineIndex.ToString() );
          }
        }

        // identify internal labels as such
        DetectInternalLabels( lineIndex, lineTokenInfos );
        StripInternalBrackets( lineTokenInfos, 1 );

        string  upToken = lineTokenInfos[0].Content.ToUpper();

        // if PDS global labels automatically work as zone
        StartNewZoneOnGlobalLabel( info, lineTokenInfos, upToken );

        //prefix zone to local labels
        if ( !ScopeInsideMacroDefinition( stackScopes ) )
        {
          PrefixZoneToLocalLabels( ref cheapLabelParent, lineTokenInfos, ref upToken );
        }

        string labelInFront = "";
        Types.TokenInfo tokenInFront = null;

        if ( upToken == "}" )
        {
          if ( stackScopes.Count == 0 )
          {
            AddError( lineIndex, Types.ErrorCode.E1004_MISSING_OPENING_BRACKET, "Missing opening brace" );
            continue;
          }
          Types.ScopeInfo   closingScope = stackScopes[stackScopes.Count - 1];

          switch ( closingScope.Type )
          {
            case Types.ScopeInfo.ScopeType.MACRO_FUNCTION:
              if ( closingScope.Macro != null )
              {
                if ( lineTokenInfos.Count != 1 )
                {
                  AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1000_SYNTAX_ERROR, "Closing brace must be single element" );
                  continue;
                }
                var result = HandleScopeEnd( m_ASMFileInfo.Macros, stackScopes, lineTokenInfos, textCodeMapping, ref lineIndex, ref intermediateLineOffset, ref Lines );
                if ( result == ParseLineResult.CALL_CONTINUE )
                {
                  --lineIndex;
                  continue;
                }
                else if ( result == ParseLineResult.ERROR_ABORT )
                {
                  HadFatalError = true;
                  return Lines;
                }
              }
              else
              {
                AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1401_INTERNAL_ERROR, "Macro function scope encountered, but no macro function set" );
                continue;
              }
              break;
            case Types.ScopeInfo.ScopeType.LOOP:
              if ( closingScope.Loop != null )
              {
                if ( lineTokenInfos.Count != 1 )
                {
                  AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1000_SYNTAX_ERROR, "Closing brace must be single element" );
                  continue;
                }
                OnScopeRemoved( lineIndex, stackScopes );
                stackScopes.RemoveAt( stackScopes.Count - 1 );
                var result = HandleScopeEnd( m_ASMFileInfo.Macros, stackScopes, lineTokenInfos, textCodeMapping, ref lineIndex, ref intermediateLineOffset, ref Lines );
                if ( result == ParseLineResult.CALL_CONTINUE )
                {
                  --lineIndex;
                  continue;
                }
                else if ( result == ParseLineResult.ERROR_ABORT )
                {
                  HadFatalError = true;
                  return Lines;
                }
              }
              break;
            case Types.ScopeInfo.ScopeType.ZONE:
              if ( lineTokenInfos.Count != 1 )
              {
                AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1000_SYNTAX_ERROR, "Closing brace must be single element" );
                continue;
              }
              OnScopeRemoved( lineIndex, stackScopes );
              stackScopes.RemoveAt( stackScopes.Count - 1 );
              DetermineActiveZone( stackScopes );
              break;
            case Types.ScopeInfo.ScopeType.PSEUDO_PC:
              PORealPC( info );
              OnScopeRemoved( lineIndex, stackScopes );
              stackScopes.RemoveAt( stackScopes.Count - 1 );
              m_CompileCurrentAddress = trueCompileCurrentAddress;
              info.AddressStart = trueCompileCurrentAddress;
              programStepPos = m_CompileCurrentAddress;
              break;
            default:
              // normal scope end
              if ( ( lineTokenInfos.Count == 3 )
              && ( lineTokenInfos[0].Content == "}" )
              && ( lineTokenInfos[2].Content == "{" )
              && ( lineTokenInfos[1].Content.ToUpper() == "ELSE" ) )
              {
                if ( !ScopeInsideMacroDefinition( stackScopes ) )
                {
                  stackScopes[stackScopes.Count - 1].Active = !stackScopes[stackScopes.Count - 1].IfChainHadActiveEntry;
                  //stackScopes[stackScopes.Count - 1].Active = !stackScopes[stackScopes.Count - 1].Active;
                  //Debug.Log( "toggle scope state " + lineIndex );
                }
              }
              else if ( lineTokenInfos.Count == 1 )
              {
                OnScopeRemoved( lineIndex, stackScopes );
                stackScopes.RemoveAt( stackScopes.Count - 1 );
              }
              else if ( ( lineTokenInfos.Count >= 4 )
              && ( lineTokenInfos[0].Content == "}" )
              && ( lineTokenInfos[lineTokenInfos.Count - 1].Content == "{" )
              && ( lineTokenInfos[1].Content.ToUpper() == "ELSE" )
              && ( lineTokenInfos[2].Content.ToUpper() == "IF" ) )
              {
                if ( !ScopeInsideMacroDefinition( stackScopes ) )
                {
                  // else if

                  // end previous block
                  var prevScope = stackScopes[stackScopes.Count - 1];
                  stackScopes.RemoveAt( stackScopes.Count - 1 );

                  // start new block
                  long defineResult = -1;

                  Types.ScopeInfo scope = new RetroDevStudio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.IF_OR_IFDEF );
                  scope.StartIndex = lineIndex;


                  if ( !isOuterScopingActive )
                  {
                    // need no evaluation, can skip over this one, since it is inactive anyway
                    scope.Active = false;
                    scope.IfChainHadActiveEntry = false;
                  }
                  else if ( ( prevScope.Active )
                  || ( prevScope.IfChainHadActiveEntry ) )
                  {
                    // need no evaluation, can skip over this one, since it is inactive anyway
                    scope.Active = false;
                    scope.IfChainHadActiveEntry = true;
                  }
                  else
                  {
                    if ( !EvaluateTokens( lineIndex, lineTokenInfos, 3, lineTokenInfos.Count - 3 - 1, textCodeMapping, out SymbolInfo defineResultSymbol ) )
                    {
                      AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate expression: "
                                + TokensToExpression( lineTokenInfos, 3, lineTokenInfos.Count - 3 - 1 ),
                                lineTokenInfos[3].StartPos, lineTokenInfos[lineTokenInfos.Count - 1].EndPos + 1 - lineTokenInfos[3].StartPos );
                      scope.Active = true;
                      scope.IfChainHadActiveEntry = true;
                      //return null;
                    }
                    else
                    {
                      defineResult = defineResultSymbol.ToInteger();
                      if ( defineResult == 0 )
                      {
                        scope.Active = false;
                      }
                      else
                      {
                        scope.Active = true;
                        scope.IfChainHadActiveEntry = true;
                      }
                      // if chain already had an active entry?
                      if ( prevScope.IfChainHadActiveEntry )
                      {
                        scope.Active = false;
                        scope.IfChainHadActiveEntry = true;
                      }
                    }
                  }
                  stackScopes.Add( scope );
                }
              }
              else
              {
                AddError( lineIndex,
                          Types.ErrorCode.E1006_MALFORMED_BLOCK_CLOSE_STATEMENT,
                          "Malformed block close statement, expecting single \"}\", \"} else {\" or \"} else if <expression> {\"",
                          lineTokenInfos[0].StartPos,
                          lineTokenInfos[lineTokenInfos.Count - 1].EndPos + 1 - lineTokenInfos[0].StartPos );
              }
              break;
          }
          continue;
        }
        else if ( IsDefine( lineTokenInfos ) )
        {
          // a define
          var callReturn = PODefine( stackScopes, lineTokenInfos, lineIndex, info, textCodeMapping, ref programStepPos, ref trueCompileCurrentAddress );
          if ( callReturn == ParseLineResult.ERROR_ABORT )
          {
            HadFatalError = true;
            return Lines;
          }
          continue;
        }
        else if ( m_ExtFunctions.Any( ef => ( string.Compare( ef.Key.first, upToken, StringComparison.InvariantCultureIgnoreCase ) == 0 ) ) )
        {
          string    possibleFunction = upToken.ToLower();
          if ( m_ExtFunctions.Any( ef => ef.Key.first == possibleFunction ) )
          {
            // handle function!
            ParseLineInParameters( lineTokenInfos, 1, lineTokenInfos.Count - 1, lineIndex, true, out List<List<Types.TokenInfo>> arguments );
            if ( arguments.Count != 1 )
            {
              if ( arguments[1].Count > 0 )
              {
                AddError( lineIndex, ErrorCode.E1000_SYNTAX_ERROR, "Garbage at end of line", arguments[1][0].StartPos, parseLine.Length - arguments[1][0].StartPos );
              }
              else
              {
                AddError( lineIndex, ErrorCode.E1000_SYNTAX_ERROR, "Garbage at end of line" );
              }
              HadFatalError = true;
              return Lines;
            }
            if ( ( arguments[0].Count < 2 )
            ||   ( !IsOpeningBraceChar( arguments[0][0].Content ) )
            ||   ( !IsClosingBraceChar( arguments[0][arguments[0].Count - 1].Content ) ) )
            {
              AddError( lineIndex, ErrorCode.E1000_SYNTAX_ERROR, "Expected parenthesis for function call", arguments[0][0].StartPos, arguments[0][arguments[0].Count - 1].EndPos - arguments[1][0].StartPos );
              HadFatalError = true;
              return Lines;
            }

            List<Types.TokenInfo> results = ProcessExtFunction( lineIndex, possibleFunction, arguments[0], 1, arguments[0].Count - 2, textCodeMapping );
            if ( results == null )
            {
              HadFatalError = true;
              return Lines;
            }
            continue;
          }
        }
        else if ( IsLabelInFront( lineTokenInfos, upToken ) )
        {
          var callResult = HandleLabelInFront( lineIndex, info, lineTokenInfos, ref upToken, stackScopes, textCodeMapping,
            ref programStepPos, ref trueCompileCurrentAddress, ref labelInFront, ref tokenInFront, ref parseLine );
          if ( callResult == ParseLineResult.ERROR_ABORT )
          {
            HadFatalError = true;
            return Lines;
          }
          if ( callResult == ParseLineResult.CALL_CONTINUE )
          {
            continue;
          }
        }

        HandleRestOfLineAfterLabelThatLooksLikeAnOpcode:;
        if ( ( lineTokenInfos.Count > 0 )
        &&   ( IsOpcode( lineTokenInfos[0].Type ) ) )
        {
          List<Tiny64.Opcode> possibleOpcodes = new List<Tiny64.Opcode>( m_Processor.Opcodes[upToken.ToLower()] );

          //Debug.Log( "TODO - if either ZP option is active dismiss unfitting possible opcodes" );
          if ( lineTokenInfos[0].Type == RetroDevStudio.Types.TokenInfo.TokenType.OPCODE_FIXED_NON_ZP )
          {
            // dismiss any zp based opcodes
            for ( int i = 0; i < possibleOpcodes.Count; ++i )
            {
              var     opcode = possibleOpcodes[i];

              if ( ( opcode.Addressing == Tiny64.Opcode.AddressingType.ZEROPAGE )
              ||   ( opcode.Addressing == Tiny64.Opcode.AddressingType.ZEROPAGE_X )
              ||   ( opcode.Addressing == Tiny64.Opcode.AddressingType.ZEROPAGE_Y )
              ||   ( opcode.Addressing == Tiny64.Opcode.AddressingType.ZEROPAGE_INDIRECT_X )
              ||   ( opcode.Addressing == Tiny64.Opcode.AddressingType.ZEROPAGE_INDIRECT_Y ) )
              {
                possibleOpcodes.RemoveAt( i );
                --i;
                continue;
              }
            }
          }
          else if ( lineTokenInfos[0].Type == RetroDevStudio.Types.TokenInfo.TokenType.OPCODE_DIRECT_VALUE )
          {
            // dismiss any zp based opcodes
            for ( int i = 0; i < possibleOpcodes.Count; ++i )
            {
              var     opcode = possibleOpcodes[i];

              if ( ( opcode.Addressing != Opcode.AddressingType.IMMEDIATE_ACCU )
              &&   ( opcode.Addressing != Opcode.AddressingType.IMMEDIATE_8BIT )
              &&   ( opcode.Addressing != Opcode.AddressingType.IMMEDIATE_REGISTER ) )
              {
                possibleOpcodes.RemoveAt( i );
                --i;
                continue;
              }
            }
          }
          else if ( lineTokenInfos[0].Type == RetroDevStudio.Types.TokenInfo.TokenType.OPCODE_FIXED_ZP )
          {
            // dismiss any non zp based opcodes
            for ( int i = 0; i < possibleOpcodes.Count; ++i )
            {
              var     opcode = possibleOpcodes[i];

              if ( ( opcode.Addressing != Tiny64.Opcode.AddressingType.ZEROPAGE )
              &&   ( opcode.Addressing != Tiny64.Opcode.AddressingType.ZEROPAGE_X )
              &&   ( opcode.Addressing != Tiny64.Opcode.AddressingType.ZEROPAGE_Y )
              &&   ( opcode.Addressing != Tiny64.Opcode.AddressingType.ZEROPAGE_INDIRECT_X )
              &&   ( opcode.Addressing != Tiny64.Opcode.AddressingType.ZEROPAGE_INDIRECT_Y ) )
              {
                possibleOpcodes.RemoveAt( i );
                --i;
                continue;
              }
            }
          }

          if ( possibleOpcodes.Count == 0 )
          {
            AddError( lineIndex,
                      RetroDevStudio.Types.ErrorCode.E1105_INVALID_OPCODE,
                      "Cannot deduce matching opcode from zero page settings",
                      lineTokenInfos[0].StartPos,
                      lineTokenInfos[0].Length );
            continue;
          }

          info.Line             = parseLine;
          info.LineCodeMapping  = textCodeMapping;

          var estimatedOpcode = EstimateOpcode( lineIndex, lineTokenInfos, possibleOpcodes, ref info, out List<List<TokenInfo>> opcodeExpressions, out ulong resultingOpcodePatchValue, out bool hadError );
          if ( estimatedOpcode != null )
          {
            //Debug.Log( "Found Token " + estimatedOpcode.first.Mnemonic + ", size " + info.NumBytes.ToString() + " in line " + parseLine );
            info.NumBytes             = estimatedOpcode.first.NumOperands + SizeOfOpcode( estimatedOpcode.first.ByteValue );
            info.Opcode               = estimatedOpcode.first;
            info.OpcodeUsingLongMode  = estimatedOpcode.second;
            if ( info.Opcode.ParserExpressions.Count > 0 )
            {
              info.NumBytes = SizeOfOpcode( estimatedOpcode.first.ByteValue );
            }

            if ( ( estimatedOpcode.first.Addressing == Opcode.AddressingType.IMMEDIATE_ACCU )
            ||   ( estimatedOpcode.first.Addressing == Opcode.AddressingType.IMMEDIATE_REGISTER )
            ||   ( estimatedOpcode.first.Addressing == Opcode.AddressingType.IMMEDIATE_8BIT )
            ||   ( estimatedOpcode.first.Addressing == Opcode.AddressingType.IMMEDIATE_16BIT ) )
            {
              // immediate may have the '#' in front of a literal (another ugly hack by yours truly)
              if ( ( lineTokenInfos.Count >= 2 )
              &&   ( lineTokenInfos[1].Type == TokenInfo.TokenType.LITERAL_NUMBER )
              &&   ( lineTokenInfos[1].Content.StartsWith( "#" ) ) )
              {
                if ( lineTokenInfos[1].Length == 1 )
                {
                  // it was only a # ??????
                  lineTokenInfos.RemoveAt( 1 );
                }
                else
                {
                  lineTokenInfos[1].Content = lineTokenInfos[1].Content.Substring( 1 );
                  --lineTokenInfos[1].Length;
                  ++lineTokenInfos[1].StartPos;
                }
              }
            }
          }
          else if ( !hadError )
          {
            // so there's a label with the same name as a opcode?
            if ( !ScopeInsideMacroDefinition( stackScopes ) )
            {
              if ( programStepPos != -1 )
              {
                // only add if we know the start address!
                //AddLabel( upToken, programStepPos, lineIndex, m_CurrentZoneName, lineTokenInfos[0].StartPos, lineTokenInfos[0].Length );
                tokenInFront = lineTokenInfos[0];
                labelInFront = tokenInFront.Content;
                lineTokenInfos.RemoveAt( 0 );
                if ( lineTokenInfos.Count > 0 )
                {
                  upToken = lineTokenInfos[0].Content.ToUpper();
                }
              }
              else
              {
                // label without value, like a define
                if ( ( m_AssemblerSettings.MacroKeywordAfterName )
                &&   ( lineTokenInfos.Count >= 2 )
                &&   ( lineTokenInfos[1].Content == MacroByType( MacroInfo.PseudoOpType.MACRO ) ) )
                {
                  // a PDS style macro definition
                }
                else
                {
                  AddLabel( upToken, -1, lineIndex, m_CurrentZoneName, lineTokenInfos[0].StartPos, lineTokenInfos[0].Length );
                }
              }
              goto HandleRestOfLineAfterLabelThatLooksLikeAnOpcode;
            }
          }
          if ( info.Opcode != null )
          {
            if ( info.Opcode.NumOperands == 0 )
            {
              if ( ( lineTokenInfos.Count > 1 )
              &&   ( info.Opcode.ParserExpressions.Count == 0 ) )
              {
                AddError( lineIndex, ErrorCode.E1000_SYNTAX_ERROR, "Garbage at end of line", lineTokenInfos[1].StartPos, parseLine.Length - lineTokenInfos[1].StartPos );
              }
              else
              {
                if ( info.LineData == null )
                {
                  info.LineData = new GR.Memory.ByteBuffer();
                }
                AppendOpcodeValue( info, resultingOpcodePatchValue );
                info.NeededParsedExpression = null;
              }
            }
            else if ( info.Opcode.NumOperands == 1 )
            {
              if ( info.LineData == null )
              {
                info.LineData = new GR.Memory.ByteBuffer();
              }
              AppendOpcodeValue( info, resultingOpcodePatchValue );
              long byteValue = -1;

              // strip prefixed #
              if ( ( info.Opcode.Addressing == Opcode.AddressingType.IMMEDIATE_ACCU )
              ||   ( info.Opcode.Addressing == Opcode.AddressingType.IMMEDIATE_8BIT )
              ||   ( info.Opcode.Addressing == Opcode.AddressingType.IMMEDIATE_16BIT )
              ||   ( info.Opcode.Addressing == Opcode.AddressingType.IMMEDIATE_REGISTER ) )
              {
                if ( ( lineTokenInfos.Count > 1 )
                && ( lineTokenInfos[1].Content.StartsWith( "#" ) ) )
                {
                  if ( lineTokenInfos[1].Length == 1 )
                  {
                    lineTokenInfos.RemoveAt( 1 );
                  }
                  else
                  {
                    lineTokenInfos[1].Content = lineTokenInfos[1].Content.Substring( 1 );
                  }
                }
              }
              if ( ScopeInsideMacroDefinition( stackScopes ) )
              {
                continue;
              }

              int   rounds = 1;
              bool  hasExpressions = info.Opcode.ParserExpressions.Count > 0;
              if ( ( opcodeExpressions != null )
              &&   ( opcodeExpressions.Count > 0 ) )
              {
                rounds = opcodeExpressions.Count;
              }

              for ( int round = 0; round < rounds; ++round )
              {
                int               startIndex = 1;
                int               count = lineTokenInfos.Count - 1;
                List<TokenInfo>   tokensToEvaluate = lineTokenInfos;

                if ( hasExpressions )
                {
                  // we now only have the actual required tokens
                  startIndex        = 0;
                  count             = opcodeExpressions[round].Count;
                  tokensToEvaluate  = opcodeExpressions[round];
                }
                else
                {
                  startIndex        += info.Opcode.StartingTokenCount;
                  count             -= info.Opcode.StartingTokenCount + info.Opcode.TrailingTokenCount;
                  tokensToEvaluate  = lineTokenInfos.GetRange( startIndex, count );
                  startIndex        = 0;
                  count             = tokensToEvaluate.Count;
                }

                if ( EvaluateTokens( lineIndex, tokensToEvaluate, startIndex, count, textCodeMapping, out SymbolInfo byteValueSymbol ) )
                {
                  byteValue = byteValueSymbol.ToInt32();
                  if ( !ValidateExpressionValueRange( ref byteValue, info, round, out int valueRangeListIndex ) )
                  {
                    AddError( lineIndex,
                              Types.ErrorCode.E1014_VALUE_OUT_OF_BOUNDS_RANGE,
                              "Value $" + byteValue.ToString( "X" ) + $" ({byteValue}) is not in the range of {ListValidValues( info.Opcode.ParserExpressions[round].ValidValues[valueRangeListIndex].ValidValues )}.",
                              tokensToEvaluate[startIndex].StartPos,
                              tokensToEvaluate[count - 1].EndPos + 1 - tokensToEvaluate[startIndex].StartPos );
                  }
                  else if ( info.Opcode.Addressing == Tiny64.Opcode.AddressingType.RELATIVE )
                  {
                    long delta = byteValue - info.AddressStart - 2;
                    if ( !Valid8BitRelativeValue( delta ) )
                    {
                      AddError( lineIndex,
                                Types.ErrorCode.E1100_RELATIVE_JUMP_TOO_FAR,
                                "Relative jump too far, trying to jump " + delta + " bytes",
                                lineTokenInfos[1].StartPos,
                                lineTokenInfos[lineTokenInfos.Count - 1].EndPos + 1 - lineTokenInfos[1].StartPos );
                      if ( !hasExpressions )
                      {
                        info.LineData.AppendU8( 0 );
                      }
                    }
                    else
                    {
                      if ( hasExpressions )
                      {
                        ApplyOpcodePatch( info, (byte)delta, round );
                      }
                      else
                      {
                        info.LineData.AppendU8( (byte)delta );
                      }
                    }
                  }
                  // 8 bit only?
                  else if ( ( info.Opcode.Addressing == Opcode.AddressingType.IMMEDIATE_ACCU )
                  ||        ( info.Opcode.Addressing == Opcode.AddressingType.IMMEDIATE_8BIT )
                  ||        ( info.Opcode.Addressing == Opcode.AddressingType.ZEROPAGE_X )
                  ||        ( info.Opcode.Addressing == Opcode.AddressingType.ZEROPAGE_Y )
                  ||        ( info.Opcode.Addressing == Opcode.AddressingType.ZEROPAGE )
                  ||        ( info.Opcode.Addressing == Opcode.AddressingType.ZEROPAGE_INDIRECT_X )
                  ||        ( info.Opcode.Addressing == Opcode.AddressingType.ZEROPAGE_INDIRECT_Y )
                  ||        ( info.Opcode.Addressing == Opcode.AddressingType.IMMEDIATE_REGISTER ) )
                  {
                    if ( ( ( info.Opcode.Addressing == Tiny64.Opcode.AddressingType.IMMEDIATE_ACCU )
                    &&     ( info.Accu16Bit ) )
                    ||   ( ( info.Opcode.Addressing == Tiny64.Opcode.AddressingType.IMMEDIATE_REGISTER )
                    &&     ( info.Registers16Bit ) ) )
                    {
                      if ( !ValidWordValue( byteValue ) )
                      {
                        AddError( lineIndex,
                                  Types.ErrorCode.E1003_VALUE_OUT_OF_BOUNDS_WORD,
                                  "Value $" + byteValue.ToString( "X" ) + " (" + byteValue + ") is out of bounds",
                                  lineTokenInfos[startIndex].StartPos,
                                  lineTokenInfos[lineTokenInfos.Count - 1].EndPos + 1 - lineTokenInfos[1].StartPos );
                        if ( !hasExpressions )
                        {
                          info.LineData.AppendU16( 0 );
                        }
                      }
                      else
                      {
                        if ( hasExpressions )
                        {
                          ApplyOpcodePatch( info, (ushort)byteValue, round );
                        }
                        else
                        {
                          info.LineData.AppendU16( (ushort)byteValue );
                        }
                      }
                      ++info.NumBytes;
                    }
                    else if ( !ValidByteValue( byteValue ) )
                    {
                      AddError( lineIndex,
                                Types.ErrorCode.E1002_VALUE_OUT_OF_BOUNDS_BYTE,
                                "Value out of bounds for byte, needs to be >= -128 and <= 255. Expression:" + TokensToExpression( lineTokenInfos, 1, lineTokenInfos.Count - 1 ),
                                lineTokenInfos[1].StartPos,
                                lineTokenInfos[lineTokenInfos.Count - 1].EndPos + 1 - lineTokenInfos[1].StartPos );
                      if ( !hasExpressions )
                      {
                        info.LineData.AppendU8( 0 );
                      }
                    }
                    else
                    {
                      if ( hasExpressions )
                      {
                        ApplyOpcodePatch( info, (byte)byteValue, round );
                      }
                      else
                      {
                        info.LineData.AppendU8( (byte)byteValue );
                      }
                    }
                  }
                  else
                  {
                    if ( hasExpressions )
                    {
                      ApplyOpcodePatch( info, (byte)byteValue, round );
                    }
                    else
                    {
                      info.LineData.AppendU8( (byte)byteValue );
                    }
                  }
                  if ( round == 0 )
                  {
                    info.NeededParsedExpression = null;
                  }
                  else
                  {
                    info.NeededParsedExpression2 = null;
                  }
                }
                else
                {
                  if ( round == 0 )
                  {
                    info.NeededParsedExpression = tokensToEvaluate;
                  }
                  else
                  {
                    info.NeededParsedExpression2 = tokensToEvaluate;
                  }
                }
              }
            }
            else if ( info.Opcode.NumOperands == 2 )
            {
              if ( info.LineData == null )
              {
                info.LineData = new GR.Memory.ByteBuffer();
              }
              AppendOpcodeValue( info, resultingOpcodePatchValue );
              long byteValue = -1;

              int   rounds = 1;
              bool  hasExpressions = false;
              if ( ( opcodeExpressions != null )
              &&   ( opcodeExpressions.Count > 0 ) )
              {
                rounds = opcodeExpressions.Count;
                hasExpressions = true;
              }

              for ( int round = 0; round < rounds; ++round )
              {
                int               startIndex = 1;
                int               count = lineTokenInfos.Count - 1;
                List<TokenInfo>   tokensToEvaluate = lineTokenInfos;

                if ( hasExpressions )
                {
                  // we now only have the actual required tokens
                  startIndex        = 0;
                  count             = opcodeExpressions[round].Count;
                  tokensToEvaluate  = opcodeExpressions[round];
                }
                else
                {
                  startIndex        += info.Opcode.StartingTokenCount;
                  count             -= info.Opcode.StartingTokenCount + info.Opcode.TrailingTokenCount;
                  tokensToEvaluate  = lineTokenInfos.GetRange( startIndex, count );
                  startIndex        = 0;
                  count             = tokensToEvaluate.Count;
                }

                if ( info.Opcode.Addressing == Tiny64.Opcode.AddressingType.ZEROPAGE_RELATIVE )
                {
                  // this has two seperate expressions
                  List<List<TokenInfo>> tokenInfos;
                  if ( !ParseLineInParameters( tokensToEvaluate, startIndex, count, lineIndex, true, out tokenInfos ) )
                  {
                    AddError( lineIndex,
                              m_LastErrorInfo.Code,
                              "Failed to parse opcode arguments" );
                  }
                  else if ( tokenInfos.Count != 2 )
                  {
                    AddError( lineIndex,
                              ErrorCode.E1000_SYNTAX_ERROR,
                              "Expected two arguments to zeropage relative addressing opcode." );
                  }
                  else
                  {
                    if ( ( !EvaluateTokens( lineIndex, tokenInfos[0], textCodeMapping, out SymbolInfo firstValueSymbol ) )
                    ||   ( !EvaluateTokens( lineIndex, tokenInfos[1], textCodeMapping, out SymbolInfo secondValueSymbol ) ) )
                    {
                      info.NeededParsedExpression = tokensToEvaluate.GetRange( 1, count );
                    }
                    else
                    {
                      int firstValue = firstValueSymbol.ToInt32();
                      int secondValue = secondValueSymbol.ToInt32();

                      // zeropage numerand
                      if ( !ValidByteValue( firstValue ) )
                      {
                        AddError( lineIndex,
                                  Types.ErrorCode.E1002_VALUE_OUT_OF_BOUNDS_BYTE,
                                  "Value out of bounds for byte, needs to be >= -128 and <= 255. Expression:"
                                    + TokensToExpression( tokenInfos[0] ),
                                  tokenInfos[0][0].StartPos,
                                  tokenInfos[0][tokenInfos[0].Count - 1].EndPos - tokenInfos[0][0].StartPos + 1 );

                        if ( !hasExpressions )
                        {
                          info.LineData.AppendU8( 0 );
                        }
                      }
                      else
                      {
                        if ( hasExpressions )
                        {
                          ApplyOpcodePatch( info, (byte)firstValue, round );
                        }
                        else
                        {
                          info.LineData.AppendU8( (byte)firstValue );
                        }
                      }

                      // relative label
                      if ( info.Opcode.Addressing == Tiny64.Opcode.AddressingType.ZEROPAGE_RELATIVE )
                      {
                        int delta = secondValue - info.AddressStart - 3;
                        if ( ( delta < -128 )
                        ||   ( delta > 127 ) )
                        {
                          AddError( lineIndex, Types.ErrorCode.E1100_RELATIVE_JUMP_TOO_FAR, "Relative jump too far, trying to jump " + delta + " bytes" );
                          if ( !hasExpressions )
                          {
                            info.LineData.AppendU8( 0 );
                          }
                        }
                        else
                        {
                          if ( hasExpressions )
                          {
                            ApplyOpcodePatch( info, (byte)delta, round );
                          }
                          else
                          {
                            info.LineData.AppendU8( (byte)delta );
                          }
                        }
                      }
                      else
                      {
                        if ( hasExpressions )
                        {
                          ApplyOpcodePatch( info, (byte)secondValue, round );
                        }
                        else
                        {
                          info.LineData.AppendU8( (byte)secondValue );
                        }
                      }
                    }
                  }
                }
                else if ( EvaluateTokens( lineIndex, tokensToEvaluate, startIndex, count, textCodeMapping, out SymbolInfo byteValueSymbol ) )
                {
                  byteValue = byteValueSymbol.ToInt32();
                  if ( ( info.Opcode.ByteValue == 0x6C )
                  &&   ( m_Processor.Name == "6510" )
                  &&   ( ( byteValue & 0xff ) == 0xff ) )
                  {
                    AddWarning( lineIndex,
                                Types.ErrorCode.W0007_POTENTIAL_PROBLEM,
                                "A indirect JMP with an address ending on 0xff will not work as expected on NMOS CPUs",
                                tokensToEvaluate[startIndex].StartPos,
                                tokensToEvaluate[tokensToEvaluate.Count - 1].EndPos + 1 - tokensToEvaluate[startIndex].StartPos );
                  }

                  if ( !ValidateExpressionValueRange( ref byteValue, info, round, out int valueRangeListIndex ) )
                  {
                    AddError( lineIndex,
                              Types.ErrorCode.E1014_VALUE_OUT_OF_BOUNDS_RANGE,
                              "Value $" + byteValue.ToString( "X" ) + $" ({byteValue}) is not in the range of {ListValidValues( info.Opcode.ParserExpressions[round].ValidValues[valueRangeListIndex].ValidValues )}.",
                              tokensToEvaluate[startIndex].StartPos,
                              tokensToEvaluate[startIndex + count - 1].EndPos + 1 - tokensToEvaluate[startIndex].StartPos );
                  }
                  else if ( !ValidWordValue( byteValue ) )
                  {
                    AddError( lineIndex,
                              Types.ErrorCode.E1003_VALUE_OUT_OF_BOUNDS_WORD,
                              "Value $" + byteValue.ToString( "X" ) + " (" + byteValue + ") is out of bounds",
                              tokensToEvaluate[startIndex].StartPos,
                              tokensToEvaluate[startIndex + count - 1].EndPos + 1 - tokensToEvaluate[startIndex].StartPos );
                  }
                  Debug.Log( "TODO - match real size of value to expression if applicable!" );

                  if ( info.Opcode.Addressing == Tiny64.Opcode.AddressingType.RELATIVE_16 )
                  {
                    // TODO - was -2 (recheck!)
                    long delta = 0;
                    if ( m_Processor.Name == "65816" )
                    {
                      delta = byteValue - info.AddressStart - 3;
                    }
                    else
                    {
                      delta = byteValue - info.AddressStart - 2;
                    }
                    if ( ( delta < -32768 )
                    ||   ( delta > 32767 ) )
                    {
                      AddError( lineIndex,
                                Types.ErrorCode.E1100_RELATIVE_JUMP_TOO_FAR,
                                "Relative jump too far, trying to jump " + delta + " bytes",
                                tokensToEvaluate[startIndex].StartPos,
                                tokensToEvaluate[startIndex + count - 1].EndPos + 1 - tokensToEvaluate[startIndex].StartPos );
                      if ( !hasExpressions )
                      {
                        info.LineData.AppendU16( 0 );
                      }
                    }
                    else
                    {
                      if ( hasExpressions )
                      {
                        ApplyOpcodePatch( info, (ushort)delta, round );
                      }
                      else
                      {
                        info.LineData.AppendU16( (ushort)delta );
                      }
                    }
                  }
                  else
                  {
                    if ( hasExpressions )
                    {
                      ApplyOpcodePatch( info, (ushort)byteValue, round );
                    }
                    else
                    {
                      info.LineData.AppendU16( (ushort)byteValue );
                    }
                  }
                  if ( round == 0 )
                  {
                    info.NeededParsedExpression = null;
                  }
                  else
                  {
                    info.NeededParsedExpression2 = null;
                  }
                }
                else
                {
                  if ( round == 0 )
                  {
                    info.NeededParsedExpression = tokensToEvaluate;
                  }
                  else
                  {
                    info.NeededParsedExpression2 = tokensToEvaluate;
                  }
                }
              }
            }
            else if ( info.Opcode.NumOperands == 3 )
            {
              if ( info.LineData == null )
              {
                info.LineData = new GR.Memory.ByteBuffer();
              }
              AppendOpcodeValue( info, resultingOpcodePatchValue );
              int               byteValue = -1;
              int               startIndex = 1;
              int               count = lineTokenInfos.Count - 1;
              List<TokenInfo>   tokensToEvaluate = lineTokenInfos;

              bool  hasExpressions = false;
              if ( ( opcodeExpressions != null )
              &&   ( opcodeExpressions.Count > 0 ) )
              {
                int rounds = opcodeExpressions.Count;
                hasExpressions = true;
              }

              if ( hasExpressions )
              {
                // we now only have the actual required tokens
                startIndex        = 0;
                count             = opcodeExpressions[0].Count;
                tokensToEvaluate  = opcodeExpressions[0];
              }
              else
              {
                startIndex        += info.Opcode.StartingTokenCount;
                count             -= info.Opcode.StartingTokenCount + info.Opcode.TrailingTokenCount;
                tokensToEvaluate  = lineTokenInfos.GetRange( startIndex, count );
                startIndex        = 0;
                count             = tokensToEvaluate.Count;
              }

              if ( EvaluateTokens( lineIndex, tokensToEvaluate, startIndex, count, textCodeMapping, out SymbolInfo byteValueSymbol ) )
              {
                byteValue = byteValueSymbol.ToInt32();
                if ( !Valid24BitValue( byteValue ) )
                {
                  AddError( lineIndex,
                            Types.ErrorCode.E1013_VALUE_OUT_OF_BOUNDS_24BIT,
                            "Value $" + byteValue.ToString( "X" ) + " (" + byteValue + ") is out of bounds",
                            tokensToEvaluate[startIndex].StartPos,
                            tokensToEvaluate[count - 1].EndPos + 1 - tokensToEvaluate[startIndex].StartPos );
                }

                if ( hasExpressions )
                {
                  ApplyOpcodePatch( info, (uint)byteValue, 0 );
                }
                else
                {
                  info.LineData.AppendU16( (ushort)byteValue );
                  info.LineData.AppendU8( (byte)( byteValue >> 16 ) );
                }
                info.NeededParsedExpression = null;
              }
              else
              {
                info.NeededParsedExpression = tokensToEvaluate;
              }
            }
            else if ( info.Opcode.NumOperands == 4 )
            {
              if ( info.LineData == null )
              {
                info.LineData = new GR.Memory.ByteBuffer();
              }
              AppendOpcodeValue( info, resultingOpcodePatchValue );
              uint  byteValue = 0;
              int   rounds    = opcodeExpressions.Count;

              for ( int round = 0; round < rounds; ++round )
              {
                int               startIndex = 0;
                int               count = opcodeExpressions[round].Count;
                List<TokenInfo>   tokensToEvaluate = opcodeExpressions[round];

                if ( EvaluateTokens( lineIndex, tokensToEvaluate, startIndex, count, textCodeMapping, out SymbolInfo byteValueSymbol ) )
                {
                  byteValue = (uint)byteValueSymbol.ToInteger();

                  if ( !ValidDWordValue( byteValue ) )
                  {
                    AddError( lineIndex,
                              Types.ErrorCode.E1015_VALUE_OUT_OF_BOUNDS_32BIT,
                              "Value $" + byteValue.ToString( "X" ) + " (" + byteValue + ") is out of bounds",
                              tokensToEvaluate[startIndex].StartPos,
                              tokensToEvaluate[count - 1].EndPos + 1 - tokensToEvaluate[startIndex].StartPos );
                  }

                  ApplyOpcodePatch( info, byteValue, round );
                  if ( round == 0 )
                  {
                    info.NeededParsedExpression = null;
                  }
                  else
                  {
                    info.NeededParsedExpression2 = null;
                  }
                }
                else
                {
                  if ( round == 0 )
                  {
                    info.NeededParsedExpression = tokensToEvaluate;
                  }
                  else
                  {
                    info.NeededParsedExpression2 = tokensToEvaluate;
                  }
                }
              }
            }
            else
            {
              AddError( lineIndex,
                        Types.ErrorCode.E1300_OPCODE_AMBIGIOUS,
                        "Could not determine correct opcode for " + lineTokenInfos[0].Content,
                        lineTokenInfos[0].StartPos,
                        lineTokenInfos[0].Length );
            }

            if ( ScopeInsideMacroDefinition( stackScopes ) )
            {
              info.LineData               = null;
              info.NumBytes               = 0;
              info.Opcode                 = null;
              info.NeededParsedExpression = null;
            }

            if ( info.NeededParsedExpression != null )
            {
              // TODO - get rid of this monstrosity!
              // remove unneeded tokens, depending on opcode
              switch ( info.Opcode.Addressing )
              {
                case Tiny64.Opcode.AddressingType.ZEROPAGE_INDIRECT_X:
                  if ( ( info.NeededParsedExpression.Count < 4 )
                  || ( !IsOpeningBraceChar( info.NeededParsedExpression[0].Content )
                  || ( info.NeededParsedExpression[info.NeededParsedExpression.Count - 3].Content != "," )
                  || ( info.NeededParsedExpression[info.NeededParsedExpression.Count - 2].Content.ToUpper() != "X" )
                  || ( !IsClosingBraceChar( info.NeededParsedExpression[info.NeededParsedExpression.Count - 1].Content ) ) ) )
                  {
                    if ( ( info.NeededParsedExpression.Count < 4 )
                    || ( !IsOpeningBraceChar( info.NeededParsedExpression[0].Content )
                    || ( !IsClosingBraceChar( info.NeededParsedExpression[info.NeededParsedExpression.Count - 3].Content ) )
                    || ( info.NeededParsedExpression[info.NeededParsedExpression.Count - 2].Content != "," )
                    || ( info.NeededParsedExpression[info.NeededParsedExpression.Count - 1].Content.ToUpper() != "X" ) ) )
                    {
                      AddError( lineIndex,
                                Types.ErrorCode.E1306_EXPECTED_BRACKETS_AND_TRAILING_SYMBOL,
                                "Expected round brackets and trailing ,x",
                                info.NeededParsedExpression[0].StartPos,
                                info.NeededParsedExpression[info.NeededParsedExpression.Count - 1].EndPos + 1 - info.NeededParsedExpression[0].StartPos );
                    }
                    else
                    {
                      info.NeededParsedExpression.RemoveRange( info.NeededParsedExpression.Count - 3, 3 );
                      info.NeededParsedExpression.RemoveAt( 0 );
                    }
                  }
                  else
                  {
                    info.NeededParsedExpression.RemoveRange( info.NeededParsedExpression.Count - 3, 3 );
                    info.NeededParsedExpression.RemoveAt( 0 );
                  }
                  break;
                case Tiny64.Opcode.AddressingType.ZEROPAGE_INDIRECT_Z:
                  // in case of case Opcode.AddressingType.INDIRECT_Z the brackets are parsed out already!
                  if ( ( info.NeededParsedExpression.Count < 2 )
                  || ( info.NeededParsedExpression[info.NeededParsedExpression.Count - 2].Content != "," )
                  || ( info.NeededParsedExpression[info.NeededParsedExpression.Count - 1].Content.ToUpper() != "Z" ) )
                  {
                    AddError( lineIndex,
                              Types.ErrorCode.E1305_EXPECTED_TRAILING_SYMBOL,
                              "Expected trailing ,z",
                              info.NeededParsedExpression[0].StartPos,
                              info.NeededParsedExpression[info.NeededParsedExpression.Count - 1].EndPos + 1 - info.NeededParsedExpression[0].StartPos );
                  }
                  else
                  {
                    info.NeededParsedExpression.RemoveRange( info.NeededParsedExpression.Count - 2, 2 );
                  }
                  break;
              }

              // look up - labels
              if ( ( info.NeededParsedExpression.Count == 1 )
              &&   ( info.NeededParsedExpression[0].Content.StartsWith( "-" ) ) )
              {
                if ( m_AssemblerSettings.LocalLabelStacking )
                {
                  // need to count backwards
                  int     numSteps = info.NeededParsedExpression[0].Content.Length;
                  int     curLine = info.LineIndex;
                  while ( numSteps > 0 )
                  {
                    if ( !previousMinusLabelStacked.TryGetLowerKey( curLine - 1, out int nextLine ) )
                    {
                      break;
                    }
                    curLine = nextLine;
                    --numSteps;
                    if ( numSteps == 0 )
                    {
                      info.NeededParsedExpression[0].Content = previousMinusLabelStacked[nextLine];
                    }
                  }
                }
                else
                {
                  if ( previousMinusLabel.ContainsKey( info.NeededParsedExpression[0].Content ) )
                  {
                    info.NeededParsedExpression[0].Content = previousMinusLabel[info.NeededParsedExpression[0].Content];
                  }
                }
              }
            }
          }
          lineSizeInBytes = info.NumBytes;
          evaluatedContent = true;
        }

        // macro function
        if ( ( lineTokenInfos.Count > 0 )
        &&   ( lineTokenInfos[0].Type == RetroDevStudio.Types.TokenInfo.TokenType.CALL_MACRO ) )
        {
          evaluatedContent = true;
          hadPseudoOp = true;

          if ( !ScopeInsideMacroDefinition( stackScopes ) )
          {
            var result = POCallMacro( lineTokenInfos, ref lineIndex, info, parseLine, ParentFilename, labelInFront, m_ASMFileInfo.Macros, ref Lines, stackScopes, textCodeMapping, out lineSizeInBytes );
            if ( result == ParseLineResult.CALL_CONTINUE )
            {
              continue;
            }
          }
        }
        else if ( ( m_AssemblerSettings.POPrefix.Length > 0 )
        &&        ( upToken.StartsWith( m_AssemblerSettings.POPrefix ) ) )
        {
          // a pseudo op
          hadPseudoOp = true;
          if ( !m_AssemblerSettings.PseudoOps.ContainsKey( upToken ) )
          {
            string    tokenToDisplay = lineTokenInfos[0].Content;
            if ( ( lineTokenInfos.Count > 1 )
            &&   ( lineTokenInfos[0].EndPos + 1 == lineTokenInfos[1].StartPos ) )
            {
              tokenToDisplay += lineTokenInfos[1].Content;
            }
            AddWarning( lineIndex,
                        Types.ErrorCode.E1301_PSEUDO_OPERATION,
                        "Unsupported pseudo op " + tokenToDisplay + ", this might result in a broken build",
                        lineTokenInfos[0].StartPos,
                        tokenToDisplay.Length );
          }
          else
          {
            Types.MacroInfo   pseudoOp = m_AssemblerSettings.PseudoOps[upToken];
            if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.END_OF_FILE )
            {
              break;
            }
            else if ( pseudoOp.Type == RetroDevStudio.Types.MacroInfo.PseudoOpType.NO_WARNING )
            {
              ParseLineResult   plResult = PONoWarning( lineTokenInfos, ref lineIndex, ref Lines );
              if ( plResult == ParseLineResult.RETURN_NULL )
              {
                HadFatalError = true;
                return Lines;
              }
              else if ( plResult == ParseLineResult.CALL_CONTINUE )
              {
                continue;
              }
            }
            else if ( pseudoOp.Type == RetroDevStudio.Types.MacroInfo.PseudoOpType.TRACE )
            {
              string  traceFilename;
              int     localLineIndex = -1;
              if ( m_ASMFileInfo.FindTrueLineSource( lineIndex, out traceFilename, out localLineIndex ) )
              {
                AddVirtualBreakpoint( localLineIndex, traceFilename, TokensToExpression( lineTokenInfos, 1, lineTokenInfos.Count - 1 ) );
              }
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.ERROR )
            {
              if ( !ScopeInsideMacroDefinition( stackScopes ) )
              {
                AddError( lineIndex, Types.ErrorCode.E1308_USER_ERROR, EvaluateAsText( lineIndex, lineTokenInfos, 1, lineTokenInfos.Count - 1, textCodeMapping ) );
              }
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.WARN )
            {
              if ( !ScopeInsideMacroDefinition( stackScopes ) )
              {
                AddWarning( lineIndex,
                            Types.ErrorCode.W0005_USER_WARNING,
                            EvaluateAsText( lineIndex, lineTokenInfos, 1, lineTokenInfos.Count - 1, textCodeMapping ),
                            lineTokenInfos[0].StartPos,
                            lineTokenInfos[0].Length );
              }
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.MESSAGE )
            {
              if ( !ScopeInsideMacroDefinition( stackScopes ) )
              {
                AddOutputMessage( lineIndex, EvaluateAsText( lineIndex, lineTokenInfos, 1, lineTokenInfos.Count - 1, textCodeMapping ) );
              }
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.SET )
            {
              var callReturn = PODefine( stackScopes, lineTokenInfos.GetRange( 1, lineTokenInfos.Count - 1 ), lineIndex, info, textCodeMapping, ref programStepPos, ref trueCompileCurrentAddress );
              if ( callReturn == ParseLineResult.ERROR_ABORT )
              {
                HadFatalError = true;
                return Lines;
              }
              continue;
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.PSEUDO_PC )
            {
              var result = POPseudoPC( info, stackScopes, lineIndex, lineTokenInfos, 1, lineTokenInfos.Count - 1 );
              if ( result == ParseLineResult.RETURN_NULL )
              {
                HadFatalError = true;
                return Lines;
              }
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.REAL_PC )
            {
              PORealPC( info );
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.INCLUDE_SOURCE )
            {
              string          subFilename = "";
              PathResolving   resolving = PathResolving.FROM_FILE;

              if ( m_AssemblerSettings.IncludeHasOnlyFilename )
              {
                // PDS style
                subFilename = TokensToExpression( lineTokenInfos, 1, lineTokenInfos.Count - 1 );
              }
              else if ( ( lineTokenInfos.Count == 2 )
              && ( lineTokenInfos[1].Type == Types.TokenInfo.TokenType.LITERAL_STRING ) )
              {
                // regular include
                subFilename = lineTokenInfos[1].Content.Substring( 1, lineTokenInfos[1].Length - 2 );
              }
              else if ( ( lineTokenInfos.Count > 3 )
              && ( lineTokenInfos[1].Content == "<" )
              && ( lineTokenInfos[lineTokenInfos.Count - 1].Content == ">" ) )
              {
                // library include
                subFilename = TokensToExpression( lineTokenInfos, 2, lineTokenInfos.Count - 3 );
                resolving = PathResolving.FROM_LIBRARIES_PATH;
              }
              else
              {
                AddError( lineIndex,
                          Types.ErrorCode.E1302_MALFORMED_MACRO,
                          "Expecting file name, either \"filename\" or <library filename>",
                          lineTokenInfos[0].StartPos,
                          lineTokenInfos[0].Length );
                HadFatalError = true;
                return Lines;
              }

              localIndex = 0;
              filename = "";
              if ( !m_ASMFileInfo.FindTrueLineSource( lineIndex, out filename, out localIndex ) )
              {
                DumpSourceInfos( OrigLines, Lines );
                AddError( lineIndex, Types.ErrorCode.E1401_INTERNAL_ERROR, "Includes caused a problem" );
                HadFatalError = true;
                return Lines;
              }

              ParseLineResult   plResult = POIncludeSource( resolving, subFilename, filename, ref lineIndex, ref Lines );
              if ( plResult == ParseLineResult.RETURN_NULL )
              {
                HadFatalError = true;
                return Lines;
              }
              else if ( plResult == ParseLineResult.CALL_CONTINUE )
              {
                continue;
              }
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.ADD_INCLUDE_SOURCE )
            {
              string  folderPath = "";

              if ( m_AssemblerSettings.IncludeHasOnlyFilename )
              {
                // PDS style
                folderPath = TokensToExpression( lineTokenInfos, 1, lineTokenInfos.Count - 1 );
              }
              else
              {
                AddError( lineIndex,
                          Types.ErrorCode.E1302_MALFORMED_MACRO,
                          "Expecting directory name",
                          lineTokenInfos[0].StartPos,
                          lineTokenInfos[0].Length );
                HadFatalError = true;
                return Lines;
              }

              localIndex = 0;
              filename = "";
              if ( !m_ASMFileInfo.FindTrueLineSource( lineIndex, out filename, out localIndex ) )
              {
                DumpSourceInfos( OrigLines, Lines );
                AddError( lineIndex, Types.ErrorCode.E1401_INTERNAL_ERROR, "Includes caused a problem" );
                HadFatalError = true;
                return Lines;
              }

              ParseLineResult   plResult = POAddLibraryPath( folderPath, filename, ref lineIndex );
              if ( plResult == ParseLineResult.RETURN_NULL )
              {
                HadFatalError = true;
                return Lines;
              }
              else if ( plResult == ParseLineResult.CALL_CONTINUE )
              {
                continue;
              }
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.FILL )
            {
              var result = POFill( lineTokenInfos, lineIndex, info, parseLine, out lineSizeInBytes );
              if ( result == ParseLineResult.CALL_CONTINUE )
              {
                continue;
              }
              else if ( result == ParseLineResult.RETURN_NULL )
              {
                HadFatalError = true;
                return Lines;
              }
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.INCLUDE_BINARY )
            {
              var result = POIncludeBinary( lineTokenInfos, lineIndex, info, out lineSizeInBytes );
              if ( result == ParseLineResult.CALL_CONTINUE )
              {
                continue;
              }
              else if ( result == ParseLineResult.RETURN_NULL )
              {
                HadFatalError = true;
                return Lines;
              }
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.INCLUDE_MEDIA )
            {
              string[] replacementLines = null;
              int dummy;
              m_ASMFileInfo.FindTrueLineSource( lineIndex, out filename, out dummy );

              if ( !POIncludeMedia( lineTokenInfos, lineIndex, true, info, filename, out lineSizeInBytes, out replacementLines ) )
              {
                HadFatalError = true;
                return Lines;
              }
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.INCLUDE_MEDIA_SOURCE )
            {
              string[] replacementLines = null;
              int dummy;
              m_ASMFileInfo.FindTrueLineSource( lineIndex, out filename, out dummy );
              if ( !POIncludeMedia( lineTokenInfos, lineIndex, false, info, filename, out lineSizeInBytes, out replacementLines ) )
              {
                HadFatalError = true;
                return Lines;
              }
              if ( replacementLines.Length == 0 )
              {
                // included empty file messes up source info, skip to adding it
                continue;
              }

              m_ASMFileInfo.FindTrueLineSource( lineIndex, out string docFile, out int docLine );

              for ( int i = 0; i < replacementLines.Length; ++i )
              {
                Types.ASM.SourceInfo incSourceInfo = new Types.ASM.SourceInfo();
                incSourceInfo.Filename = docFile;
                incSourceInfo.FullPath = docFile;

                incSourceInfo.GlobalStartLine = lineIndex + i;
                incSourceInfo.LocalStartLine = docLine;
                incSourceInfo.LineCount = 1;
                incSourceInfo.FilenameParent = ParentFilename;

                InsertSourceInfo( incSourceInfo );
              }

              string[] result = new string[Lines.Length + replacementLines.Length];

              System.Array.Copy( Lines, 0, result, 0, lineIndex );
              System.Array.Copy( replacementLines, 0, result, lineIndex, replacementLines.Length );
              System.Array.Copy( Lines, lineIndex, result, lineIndex + replacementLines.Length, Lines.Length - lineIndex );
              // replace !source with empty line (otherwise source infos would have one line more!)
              result[lineIndex + replacementLines.Length] = "";

              Lines = result;

              m_ASMFileInfo.LineInfo.Remove( lineIndex );

              --lineIndex;
              continue;
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.COMPILE_TARGET )
            {
              var result = POTo( lineTokenInfos, lineIndex );
              if ( result == ParseLineResult.ERROR_ABORT )
              {
                HadFatalError = true;
                return Lines;
              }
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.ADDRESS )
            {
              int     openingBracketTokenIndex = -1;
              for ( int i = 0; i < lineTokenInfos.Count; ++i )
              {
                if ( lineTokenInfos[i].Content == "{" )
                {
                  openingBracketTokenIndex = i;
                  break;
                }
              }

              if ( openingBracketTokenIndex != -1 )
              {
                stackScopes.Add( new ScopeInfo( ScopeInfo.ScopeType.ADDRESS ) { Active = true, StartIndex = lineIndex } );
                continue;
              }
              else
              {
                lineTokenInfos.RemoveAt( 0 );
                goto recheck_line;
              }
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.IFDEF )
            {
              // !ifdef MUSIC_ON {
              int     openingBracketTokenIndex = -1;
              for ( int i = 0; i < lineTokenInfos.Count; ++i )
              {
                if ( lineTokenInfos[i].Content == "{" )
                {
                  openingBracketTokenIndex = i;
                  break;
                }
              }

              if ( openingBracketTokenIndex == -1 )
              {
                AddError( lineIndex,
                          Types.ErrorCode.E1004_MISSING_OPENING_BRACKET,
                          "Missing opening brace",
                          lineTokenInfos[0].StartPos,
                          lineTokenInfos[lineTokenInfos.Count - 1].EndPos + 1 - lineTokenInfos[0].StartPos );
              }
              else
              {
                List<Types.TokenInfo> tokens = lineTokenInfos.GetRange( 1, openingBracketTokenIndex - 1 );
                List<Types.TokenInfo> trailingtokens = lineTokenInfos.GetRange( openingBracketTokenIndex + 1, lineTokenInfos.Count - openingBracketTokenIndex - 1 );

                bool hadElse = false;
                if ( trailingtokens.Count > 0 )
                {
                  // check for } else {
                  if ( trailingtokens.Count >= 3 )
                  {
                    if ( ( trailingtokens[trailingtokens.Count - 3].Content == "}" )
                    && ( trailingtokens[trailingtokens.Count - 2].Content.ToUpper() == "ELSE" )
                    && ( trailingtokens[trailingtokens.Count - 1].Content == "{" ) )
                    {
                      hadElse = true;
                    }
                  }
                  if ( !hadElse )
                  {
                    AddError( lineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "Syntax error, expected no tokens after bracket" );
                    HadFatalError = true;
                    return Lines;
                  }
                }
                {
                  Types.ScopeInfo scope = new RetroDevStudio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.IF_OR_IFDEF );
                  scope.StartIndex = lineIndex;

                  // only evaluate the first token

                  StripInternalBrackets( tokens );//, 1 );
                  // TODO - have to evaluate the rest of the line if it exists!!
                  if ( !EvaluateTokens( lineIndex, tokens, 0, 1, textCodeMapping, out SymbolInfo defineResultSymbol ) )
                  {
                    scope.Active = hadElse;
                  }
                  else
                  {
                    scope.Active = !hadElse;
                  }
                  if ( scope.Active )
                  {
                    scope.IfChainHadActiveEntry = true;
                  }
                  stackScopes.Add( scope );
                  //Debug.Log( "Add Scope ifdefa " + lineIndex );
                }
              }
            }
            else if ( pseudoOp.Type == RetroDevStudio.Types.MacroInfo.PseudoOpType.LABEL_FILE )
            {
              lineTokenInfos.RemoveAt( 0 );

              if ( lineTokenInfos.Count != 1 )
              {
                AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Expected !sl <Filename>" );
                HadFatalError = true;
                return Lines;
              }
              if ( lineTokenInfos[0].Type != Types.TokenInfo.TokenType.LITERAL_STRING )
              {
                AddError( lineIndex,
                          Types.ErrorCode.E1307_FILENAME_INCOMPLETE,
                          "String as file name expected",
                          lineTokenInfos[0].StartPos,
                          lineTokenInfos[0].Length );
                HadFatalError = true;
                return Lines;
              }
              if ( !string.IsNullOrEmpty( m_ASMFileInfo.LabelDumpFile ) )
              {
                AddWarning( lineIndex,
                            RetroDevStudio.Types.ErrorCode.W0006_LABEL_DUMP_FILE_ALREADY_GIVEN,
                            "Label dump file name has already been provided",
                            lineTokenInfos[0].StartPos,
                            lineTokenInfos[lineTokenInfos.Count - 1].EndPos + 1 - lineTokenInfos[0].StartPos );
              }

              string fileName = lineTokenInfos[0].Content.Substring( 1, lineTokenInfos[0].Content.Length - 2 );
              m_ASMFileInfo.LabelDumpFile = fileName;
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.MACRO )
            {
              string macroName = "";
              string outerFilename = "";
              int localLineIndex = 0;
              m_ASMFileInfo.FindTrueLineSource( lineIndex, out outerFilename, out localLineIndex );

              if ( POMacro( labelInFront, m_CurrentZoneName, m_ASMFileInfo.Macros, outerFilename, lineIndex, lineTokenInfos, stackScopes, out macroName ) )
              {
                if ( m_AssemblerSettings.MacroIsZone )
                {
                  m_CurrentZoneName = macroName;
                  info.Zone = m_CurrentZoneName;
                }
              }
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.FOR )
            {
              // !FOR var = start TO stop
              POFor( stackScopes, m_CurrentZoneName, ref intermediateLineOffset, lineIndex, lineTokenInfos, textCodeMapping );
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.END )
            {
              var result = HandleScopeEnd( m_ASMFileInfo.Macros, stackScopes, lineTokenInfos, textCodeMapping, ref lineIndex, ref intermediateLineOffset, ref Lines );
              if ( result == ParseLineResult.CALL_CONTINUE )
              {
                --lineIndex;
                continue;
              }
              else if ( result == ParseLineResult.ERROR_ABORT )
              {
                HadFatalError = true;
                return Lines;
              }
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.IFNDEF )
            {
              if ( ScopeInsideMacroDefinition( stackScopes ) )
              {
                // Skip !if check inside macro definition

                // still need to add scope!
                Types.ScopeInfo scope = new RetroDevStudio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.IF_OR_IFDEF );
                scope.StartIndex = lineIndex;
                scope.Active = false;

                stackScopes.Add( scope );
                OnScopeAdded( scope );
                continue;
              }

              int startBracket = parseLine.IndexOf( "{" );
              if ( startBracket == -1 )
              {
                AddError( lineIndex, Types.ErrorCode.E1004_MISSING_OPENING_BRACKET, "Missing opening brace" );
              }
              else
              {
                Types.ScopeInfo scope = new RetroDevStudio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.IF_OR_IFDEF );
                scope.StartIndex = lineIndex;
                if ( !EvaluateTokens( lineIndex, lineTokenInfos, 1, lineTokenInfos.Count - 2, textCodeMapping, out SymbolInfo defineResultSymbol ) )
                {
                  scope.Active = true;
                  scope.IfChainHadActiveEntry = true;
                }
                else
                {
                  scope.Active = false;
                }
                stackScopes.Add( scope );
                OnScopeAdded( scope );
              }
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.IF )
            {
              if ( ScopeInsideMacroDefinition( stackScopes ) )
              {
                // Skip !if check inside macro definition

                // still need to add scope!
                Types.ScopeInfo scope = new RetroDevStudio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.IF_OR_IFDEF );
                scope.StartIndex = lineIndex;
                scope.Active = false;

                stackScopes.Add( scope );
                OnScopeAdded( scope );
                continue;
              }

              // !ifdef MUSIC_ON {
              int startBracket = parseLine.IndexOf( "{" );
              int numTokensForIf = lineTokenInfos.Count - 1;
              if ( ( startBracket == -1 )
              &&   ( !m_AssemblerSettings.IfWithoutBrackets ) )
              {
                AddError( lineIndex, Types.ErrorCode.E1004_MISSING_OPENING_BRACKET, "Missing opening brace" );
              }
              else
              {
                if ( !m_AssemblerSettings.IfWithoutBrackets )
                {
                  --numTokensForIf;
                }
                int     posAfterMacro = labelInFront.Length;
                if ( labelInFront.Length == 0 )
                {
                  posAfterMacro = lineTokenInfos[0].StartPos + lineTokenInfos[0].Length;
                }
                Types.ScopeInfo scope = new RetroDevStudio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.IF_OR_IFDEF );
                scope.StartIndex = lineIndex;

                if ( !EvaluateTokens( lineIndex, lineTokenInfos, 1, numTokensForIf, textCodeMapping, out SymbolInfo defineResultSymbol ) )
                {
                  AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate expression: " + TokensToExpression( lineTokenInfos, 1, numTokensForIf ) );
                  scope.Active = true;
                  scope.IfChainHadActiveEntry = true;
                  HadFatalError = true;
                  return Lines;
                }
                else if ( defineResultSymbol.ToInteger() == 0 )
                {
                  scope.Active = false;
                }
                else
                {
                  scope.Active = true;
                  scope.IfChainHadActiveEntry = true;
                }
                stackScopes.Add( scope );
                OnScopeAdded( scope );
              }
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.ELSE )
            {
              if ( ScopeInsideMacroDefinition( stackScopes ) )
              {
                // Skip !if check inside macro definition

                // might still need to toggle scope, but keep it as inactive
                continue;
              }

              if ( stackScopes.Count == 0 )
              {
                AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1309_ELSE_WITHOUT_IF, "Else statement without if encountered" );
              }
              else
              {
                stackScopes[stackScopes.Count - 1].Active = !stackScopes[stackScopes.Count - 1].IfChainHadActiveEntry;
              }
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.END_IF )
            {
              if ( ScopeInsideMacroDefinition( stackScopes ) )
              {
                // Skip !if check inside macro definition

                // still need to remove scope!
                OnScopeRemoved( lineIndex, stackScopes );
                stackScopes.RemoveAt( stackScopes.Count - 1 );
                continue;
              }

              if ( stackScopes.Count == 0 )
              {
                AddError( lineIndex, Types.ErrorCode.E1310_END_IF_WITHOUT_SCOPE, "End if without scope" );
              }
              else
              {
                OnScopeRemoved( lineIndex, stackScopes );
                stackScopes.RemoveAt( stackScopes.Count - 1 );
              }
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.ZONE )
            {
              POZone( stackScopes, lineIndex, info, lineTokenInfos, false );
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.LZONE )
            {
              POZone( stackScopes, lineIndex, info, lineTokenInfos, true );
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.BANK )
            {
              // !BANK no,size
              int paramPos = 0;
              List<Types.TokenInfo> paramsNo = new List<Types.TokenInfo>();
              List<Types.TokenInfo> paramsSize = new List<Types.TokenInfo>();
              for ( int i = 1; i < lineTokenInfos.Count; ++i )
              {
                if ( lineTokenInfos[i].Content == "," )
                {
                  ++paramPos;
                  if ( paramPos >= 2 )
                  {
                    AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Macro not formatted as expected. Expected !bank <Number>,<Size>", lineTokenInfos[i].StartPos, lineTokenInfos[i].Length );
                    break;
                  }
                }
                else
                {
                  switch ( paramPos )
                  {
                    case 0:
                      paramsNo.Add( lineTokenInfos[i] );
                      break;
                    case 1:
                      paramsSize.Add( lineTokenInfos[i] );
                      break;
                  }
                }
              }
              if ( ( paramPos == 0 )
              || ( paramPos > 1 ) )
              {
                AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Macro not formatted as expected. Expected !bank <Number>[,<Size>]" );
              }
              else
              {
                int number = -1;
                int size = -1;
                SymbolInfo sizeSymbol = null;
                if ( !EvaluateTokens( lineIndex, paramsNo, textCodeMapping, out SymbolInfo numberSymbol ) )
                {
                  string expressionCheck = TokensToExpression( paramsNo );

                  AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate expression " + expressionCheck );
                }
                else if ( ( paramsSize.Count > 0 )
                && ( !EvaluateTokens( lineIndex, paramsSize, textCodeMapping, out sizeSymbol ) ) )
                {
                  string expressionCheck = TokensToExpression( paramsNo );

                  AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate expression " + expressionCheck );
                }
                else
                {
                  number = numberSymbol.ToInt32();
                  size = sizeSymbol.ToInt32();
                  if ( m_ASMFileInfo.Banks.Count > 0 )
                  {
                    // fill from previous bank
                    Types.ASM.BankInfo lastBank = m_ASMFileInfo.Banks[m_ASMFileInfo.Banks.Count - 1];

                    // size was not given, reuse from previous bank
                    if ( paramsSize.Count == 0 )
                    {
                      size = lastBank.SizeInBytes;
                    }

                    if ( sizeInBytes <= lastBank.SizeInBytesStart + lastBank.SizeInBytes )
                    {
                      // we need to fill

                      int delta = lastBank.SizeInBytesStart + lastBank.SizeInBytes - sizeInBytes;

                      info.NumBytes = delta;
                      info.LineData = new GR.Memory.ByteBuffer( (uint)delta );
                      lineSizeInBytes = delta;
                    }
                    else
                    {
                      int overflow = sizeInBytes - lastBank.SizeInBytesStart;
                      AddError( lineIndex, Types.ErrorCode.E1101_BANK_TOO_BIG, "Bank " + lastBank.Number + " contains too much bytes, " + lastBank.SizeInBytes + " chosen, " + overflow + " encountered" );
                    }
                  }
                  if ( size == 0 )
                  {
                    AddError( lineIndex, Types.ErrorCode.E1104_BANK_SIZE_INVALID, "Bank size is invalid" );
                  }

                  Types.ASM.BankInfo bank = new RetroDevStudio.Types.ASM.BankInfo();
                  bank.Number = number;
                  bank.SizeInBytes = size;
                  bank.StartLine = lineIndex;
                  bank.SizeInBytesStart = sizeInBytes + info.NumBytes;

                  foreach ( Types.ASM.BankInfo oldBank in m_ASMFileInfo.Banks )
                  {
                    if ( oldBank.Number == number )
                    {
                      AddWarning( lineIndex,
                                  Types.ErrorCode.W0003_BANK_INDEX_ALREADY_USED,
                                  "Bank with index " + number + " already exists",
                                  lineTokenInfos[0].StartPos,
                                  lineTokenInfos[lineTokenInfos.Count - 1].EndPos + 1 - lineTokenInfos[0].StartPos );
                    }
                  }

                  m_ASMFileInfo.Banks.Add( bank );
                }
              }
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.ALIGN )
            {
              // !ALIGN andvalue,equalvalue,fillvalue
              List<int> tokenParams = new List<int>();
              /*
              string andValue = "";
              string equalValue = "";
              string fillValue = "";
              */
              int tokenIndex = 1;
              int expressionStartIndex = 1;

              if ( lineTokenInfos.Count < 2 )
              {
                AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Macro not formatted as expected. Expected !align <AndValue>,<EqualValue>[,<FillValue>]" );
                HadFatalError = true;
                return Lines;
              }

              do
              {
                if ( lineTokenInfos[tokenIndex].Content == "," )
                {
                  // found an expression
                  if ( !EvaluateTokens( lineIndex, lineTokenInfos, expressionStartIndex, tokenIndex - expressionStartIndex, textCodeMapping, out SymbolInfo value ) )
                  {
                    AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate " + TokensToExpression( lineTokenInfos, expressionStartIndex, tokenIndex - expressionStartIndex ) );
                    HadFatalError = true;
                    return Lines;
                  }
                  tokenParams.Add( value.ToInt32() );
                  expressionStartIndex = tokenIndex + 1;
                }
                ++tokenIndex;
                if ( tokenIndex == lineTokenInfos.Count )
                {
                  if ( expressionStartIndex <= tokenIndex - 1 )
                  {
                    // there's still data to evaluate
                    if ( !EvaluateTokens( lineIndex, lineTokenInfos, expressionStartIndex, tokenIndex - expressionStartIndex, textCodeMapping, out SymbolInfo value ) )
                    {
                      AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate " + TokensToExpression( lineTokenInfos, expressionStartIndex, tokenIndex - expressionStartIndex ) );
                      HadFatalError = true;
                      return Lines;
                    }
                    tokenParams.Add( value.ToInt32() );
                  }
                }
              }
              while ( tokenIndex < lineTokenInfos.Count );

              if ( ( tokenParams.Count < 2 )
              || ( tokenParams.Count > 3 ) )
              {
                AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Macro not formatted as expected. Expected !align <AndValue>,<EqualValue>[,<FillValue>]" );
                HadFatalError = true;
                return Lines;
              }
              byte fillValue = 0;
              if ( tokenParams.Count == 3 )
              {
                if ( !ValidByteValue( tokenParams[2] ) )
                {
                  AddError( lineIndex, Types.ErrorCode.E1002_VALUE_OUT_OF_BOUNDS_BYTE, "FillValue out of bounds" );
                  HadFatalError = true;
                  return Lines;
                }
                fillValue = (byte)tokenParams[2];
              }
              info.LineData = new GR.Memory.ByteBuffer();
              while ( ( programStepPos & tokenParams[0] ) != tokenParams[1] )
              {
                ++programStepPos;
                ++info.NumBytes;
                info.LineData.AppendU8( fillValue );
                if ( programStepPos == 65536 )
                {
                  AddError( lineIndex, Types.ErrorCode.E1102_PROGRAM_TOO_LARGE, "Program step pos out of bounds, check !align condition" );
                  HadFatalError = true;
                  return Lines;
                }
              }
              lineSizeInBytes = info.NumBytes;
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.PREPROCESSED_LIST )
            {
              POPreprocessedList( lineTokenInfos, lineIndex, info, ref hideInPreprocessedOutput );
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.ALIGN_DASM )
            {
              var parseResult = POAlignDASM( lineTokenInfos, lineIndex, info, ref programStepPos, out lineSizeInBytes );
              if ( parseResult == ParseLineResult.RETURN_NULL )
              {
                HadFatalError = true;
                return Lines;
              }
            }
            else if ( ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.BYTE )
            || ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.LOW_BYTE )
            || ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.HIGH_BYTE ) )
            {
              PODataByte( lineIndex, lineTokenInfos, 1, lineTokenInfos.Count - 1, info, pseudoOp.Type, textCodeMapping, true );
              info.Line = parseLine;
              lineSizeInBytes = info.NumBytes;
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.WORD )
            {
              var result = PODataWord( lineTokenInfos, lineIndex, 1, lineTokenInfos.Count - 1, info, parseLine, true, true, out lineSizeInBytes );
              if ( result == ParseLineResult.RETURN_NULL )
              {
                HadFatalError = true;
                return Lines;
              }
              else if ( result == ParseLineResult.CALL_CONTINUE )
              {
                continue;
              }
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.WORD_BE )
            {
              var result = PODataWord( lineTokenInfos, lineIndex, 1, lineTokenInfos.Count - 1, info, parseLine, true, false, out lineSizeInBytes );
              if ( result == ParseLineResult.RETURN_NULL )
              {
                HadFatalError = true;
                return Lines;
              }
              else if ( result == ParseLineResult.CALL_CONTINUE )
              {
                continue;
              }
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.DWORD )
            {
              var result = PODataDWord( lineTokenInfos, lineIndex, 1, lineTokenInfos.Count - 1, info, parseLine, true, true, out lineSizeInBytes );
              if ( result == ParseLineResult.RETURN_NULL )
              {
                HadFatalError = true;
                return Lines;
              }
              else if ( result == ParseLineResult.CALL_CONTINUE )
              {
                continue;
              }
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.DWORD_BE )
            {
              var result = PODataDWord( lineTokenInfos, lineIndex, 1, lineTokenInfos.Count - 1, info, parseLine, true, false, out lineSizeInBytes );
              if ( result == ParseLineResult.RETURN_NULL )
              {
                HadFatalError = true;
                return Lines;
              }
              else if ( result == ParseLineResult.CALL_CONTINUE )
              {
                continue;
              }
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.TEXT_SCREEN )
            {
              POText( lineIndex, lineTokenInfos, info, parseLine, m_TextCodeMappingScr, out lineSizeInBytes );
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.TEXT_SCREEN_XOR )
            {
              POTextXor( lineIndex, lineTokenInfos, info, parseLine, m_TextCodeMappingScr, out lineSizeInBytes );
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.TEXT_PET )
            {
              POText( lineIndex, lineTokenInfos, info, parseLine, m_TextCodeMappingPet, out lineSizeInBytes );
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.TEXT )
            {
              POText( lineIndex, lineTokenInfos, info, parseLine, textCodeMapping, out lineSizeInBytes );
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.TEXT_RAW )
            {
              POText( lineIndex, lineTokenInfos, info, parseLine, m_TextCodeMappingRaw, out lineSizeInBytes );
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.CONVERSION_TAB )
            {
              textCodeMapping = POConversionTab( pseudoOp.Keyword, textCodeMapping, lineIndex, lineTokenInfos );
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.BREAK_POINT )
            {
              // these just need to be remembered
              Debug.Log( "TODO - Store breakpoint address" );
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.IGNORE )
            {
              labelInFront = "";
            }
            else if ( pseudoOp.Type == RetroDevStudio.Types.MacroInfo.PseudoOpType.BASIC )
            {
              var parseResult = POBasic( parseLine, lineTokenInfos, lineIndex, info, textCodeMapping, true, hideInPreprocessedOutput, out lineSizeInBytes );
              if ( parseResult == ParseLineResult.RETURN_NULL )
              {
                HadFatalError = true;
                return Lines;
              }
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.HEX )
            {
              // HEX - special macro
              var parseResult = POACMEHex( info, lineTokenInfos, lineIndex, out lineSizeInBytes );
              if ( parseResult == ParseLineResult.RETURN_NULL )
              {
                HadFatalError = true;
                return Lines;
              }
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.SKIP )
            {
              var parseResult = POSkip( lineTokenInfos, lineIndex, info, textCodeMapping, ref programStepPos, ref trueCompileCurrentAddress );
              if ( ( parseResult == ParseLineResult.RETURN_NULL )
              ||   ( parseResult == ParseLineResult.ERROR_ABORT ) )
              {
                HadFatalError = true;
                return Lines;
              }
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.CPU )
            {
              var parseResult = POCPU( lineTokenInfos, lineIndex );
              if ( parseResult == ParseLineResult.RETURN_NULL )
              {
                HadFatalError = true;
                return Lines;
              }
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.CONVERSION_TAB_TASS )
            {
              textCodeMapping = POConversionTabTASS( pseudoOp.Keyword, textCodeMapping, lineIndex, lineTokenInfos );
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.CONVERSION_TAB_TASS_ENTRY )
            {
              var result = POConversionTabTASSEntry( pseudoOp.Keyword, textCodeMapping, lineIndex, lineTokenInfos, out textCodeMapping );
              if ( result != ParseLineResult.OK )
              {
                HadFatalError = true;
                return Lines;
              }
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.ASSUME_16BIT_ACCUMULATOR_65816 )
            {
              var result = PO65816Assume16BitAccu( lineIndex );
              if ( result != ParseLineResult.OK )
              {
                HadFatalError = true;
                return Lines;
              }
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.ASSUME_8BIT_ACCUMULATOR_65816 )
            {
              var result = PO65816Assume8BitAccu( lineIndex );
              if ( result != ParseLineResult.OK )
              {
                HadFatalError = true;
                return Lines;
              }
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.ASSUME_16BIT_REGISTERS_65816 )
            {
              var result = PO65816Assume16BitRegisters( lineIndex );
              if ( result != ParseLineResult.OK )
              {
                HadFatalError = true;
                return Lines;
              }
            }
            else if ( pseudoOp.Type == Types.MacroInfo.PseudoOpType.ASSUME_8BIT_REGISTERS_65816 )
            {
              var result = PO65816Assume8BitRegisters( lineIndex );
              if ( result != ParseLineResult.OK )
              {
                HadFatalError = true;
                return Lines;
              }
            }
            else
            {
              AddError( lineIndex, Types.ErrorCode.E1301_PSEUDO_OPERATION, $"Macro {pseudoOp.Keyword} currently has no effect!" );
            }
          }
          evaluatedContent = true;
        }

        // PDS/DASM style pseudo ops look like labels!
        if ( ( !evaluatedContent )
        &&   ( m_AssemblerSettings.PseudoOps.ContainsKey( upToken ) ) )
        {
          // TODO - ugly, copied code!!
          hadPseudoOp = true;
          Types.MacroInfo macroInfo = m_AssemblerSettings.PseudoOps[upToken];
          if ( macroInfo.Type == RetroDevStudio.Types.MacroInfo.PseudoOpType.HEX )
          {
            // HEX - special macro
            info.LineData = new GR.Memory.ByteBuffer();
            for ( int i = 1; i < lineTokenInfos.Count; ++i )
            {
              Types.TokenInfo tokenHex = lineTokenInfos[i];
              if ( ( tokenHex.Length % 2 ) != 0 )
              {
                if ( !info.LineData.AppendHex( "0" + tokenHex.Content ) )
                {
                  AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1000_SYNTAX_ERROR, "Malformed hex data" );
                  HadFatalError = true;
                  return Lines;
                }
              }
              else
              {
                if ( !info.LineData.AppendHex( tokenHex.Content ) )
                {
                  AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1000_SYNTAX_ERROR, "Malformed hex data" );
                  HadFatalError = true;
                  return Lines;
                }
              }
            }
            info.NumBytes = (int)info.LineData.Length;
            lineSizeInBytes = (int)info.LineData.Length;
          }
          else if ( macroInfo.Type == Types.MacroInfo.PseudoOpType.IFNDEF )
          {
            if ( ScopeInsideMacroDefinition( stackScopes ) )
            {
              // Skip !if check inside macro definition

              // still need to add scope!
              Types.ScopeInfo scopeIfNotDef = new RetroDevStudio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.IF_OR_IFDEF );
              scopeIfNotDef.StartIndex = lineIndex;
              scopeIfNotDef.Active = false;

              stackScopes.Add( scopeIfNotDef );
              OnScopeAdded( scopeIfNotDef );
              continue;
            }
            int     pseudoOpEndPos = lineTokenInfos[0].StartPos + lineTokenInfos[0].Length;
            string defineCheck = parseLine.Substring( pseudoOpEndPos ).Trim();

            List<Types.TokenInfo> tokens = ParseTokenInfo( defineCheck, 0, defineCheck.Length, textCodeMapping );

            if ( ( tokens.Count != 1 )
            ||   ( !IsTokenLabel( tokens[0].Type ) ) )
            {
              AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1000_SYNTAX_ERROR, "Expected single label" );
              HadFatalError = true;
              return Lines;
            }

            Types.ScopeInfo scope = new RetroDevStudio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.IF_OR_IFDEF );
            scope.StartIndex = lineIndex;
            scope.Active = !IsKnownLabel( tokens[0] );
            stackScopes.Add( scope );
            OnScopeAdded( scope );
          }
          else if ( macroInfo.Type == Types.MacroInfo.PseudoOpType.IFDEF )
          {
            if ( ScopeInsideMacroDefinition( stackScopes ) )
            {
              // Skip !if check inside macro definition

              // still need to add scope!
              Types.ScopeInfo scopeIfNotDef = new RetroDevStudio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.IF_OR_IFDEF );
              scopeIfNotDef.StartIndex = lineIndex;
              scopeIfNotDef.Active = false;

              stackScopes.Add( scopeIfNotDef );
              OnScopeAdded( scopeIfNotDef );
              continue;
            }
            int     pseudoOpEndPos = lineTokenInfos[0].StartPos + lineTokenInfos[0].Length;
            string defineCheck = parseLine.Substring( pseudoOpEndPos ).Trim();

            List<Types.TokenInfo> tokens = ParseTokenInfo( defineCheck, 0, defineCheck.Length, textCodeMapping );
            StripInternalBrackets( tokens );

            if ( ( tokens.Count != 1 )
            ||   ( !IsTokenLabel( tokens[0].Type ) ) )
            {
              AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1000_SYNTAX_ERROR, "Expected single label" );
              HadFatalError = true;
              return Lines;
            }

            Types.ScopeInfo scope = new RetroDevStudio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.IF_OR_IFDEF );
            scope.StartIndex = lineIndex;
            scope.Active = IsKnownLabel( tokens[0] );
            stackScopes.Add( scope );
            OnScopeAdded( scope );
          }
          else if ( macroInfo.Type == Types.MacroInfo.PseudoOpType.END_OF_FILE )
          {
            break;
          }
          else if ( macroInfo.Type == Types.MacroInfo.PseudoOpType.SEG )
          {
            m_CurrentSegmentIsVirtual = false;
          }
          else if ( macroInfo.Type == Types.MacroInfo.PseudoOpType.SEG_VIRTUAL )
          {
            m_CurrentSegmentIsVirtual = true;
          }
          else if ( macroInfo.Type == RetroDevStudio.Types.MacroInfo.PseudoOpType.BASIC )
          {
            var parseResult = POBasic( parseLine, lineTokenInfos, lineIndex, info, textCodeMapping, true, hideInPreprocessedOutput, out lineSizeInBytes );
            if ( parseResult == ParseLineResult.RETURN_NULL )
            {
              HadFatalError = true;
              return Lines;
            }
          }
          else if ( macroInfo.Type == RetroDevStudio.Types.MacroInfo.PseudoOpType.ORG )
          {
            // set program step
            int newStepPos = 0;
            int newPseudoPos = -2;

            int commaPos = -1;
            int commaCount = 0;
            for ( int i = 1; i < lineTokenInfos.Count; ++i )
            {
              if ( ( lineTokenInfos[i].Type == RetroDevStudio.Types.TokenInfo.TokenType.SEPARATOR )
              &&   ( lineTokenInfos[i].Content == "," ) )
              {
                if ( commaCount == 0 )
                {
                  commaPos = i;
                }
                ++commaCount;
              }
            }
            if ( commaCount > 1 )
            {
              AddError( lineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "Malformed ORG directive, expected ORG Position[,PseudoPosition]" );
              HadFatalError = true;
              return Lines;
            }
            if ( commaCount == 1 )
            {
              if ( !EvaluateTokens( lineIndex, lineTokenInfos, 1, commaPos - 1, textCodeMapping, out SymbolInfo newStepPosSymbol ) )
              {
                AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate ORG position value" );
                HadFatalError = true;
                return Lines;
              }
              if ( !EvaluateTokens( lineIndex, lineTokenInfos, commaPos + 1, lineTokenInfos.Count - commaPos - 1, textCodeMapping, out SymbolInfo newPseudoPosSymbol ) )
              {
                AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate ORG pseudo position value" );
                HadFatalError = true;
                return Lines;
              }
              newStepPos = newStepPosSymbol.ToInt32();
              newPseudoPos = newPseudoPosSymbol.ToInt32();
            }
            else
            {
              if ( !EvaluateTokens( lineIndex, lineTokenInfos, 1, lineTokenInfos.Count - 1, textCodeMapping, out SymbolInfo newStepPosSymbol ) )
              {
                AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate ORG position value" );
                HadFatalError = true;
                return Lines;
              }
              newStepPos = newStepPosSymbol.ToInt32();
            }
            programStepPos = newStepPos;
            m_CompileCurrentAddress = programStepPos;
            trueCompileCurrentAddress = programStepPos;

            // either the new value or -2
            info.PseudoPCOffset = newPseudoPos;
          }
          else if ( macroInfo.Type == Types.MacroInfo.PseudoOpType.PSEUDO_PC )
          {
            var result = POPseudoPC( info, stackScopes, lineIndex, lineTokenInfos, 1, lineTokenInfos.Count - 1 );
            if ( result == ParseLineResult.RETURN_NULL )
            {
              HadFatalError = true;
              return Lines;
            }
          }
          else if ( macroInfo.Type == Types.MacroInfo.PseudoOpType.REAL_PC )
          {
            PORealPC( info );
          }
          else if ( ( macroInfo.Type == Types.MacroInfo.PseudoOpType.BYTE )
          ||        ( macroInfo.Type == Types.MacroInfo.PseudoOpType.LOW_BYTE )
          ||        ( macroInfo.Type == Types.MacroInfo.PseudoOpType.HIGH_BYTE ) )
          {
            PODataByte( lineIndex, lineTokenInfos, 1, lineTokenInfos.Count - 1, info, macroInfo.Type, textCodeMapping, true );
            info.Line = parseLine;
            lineSizeInBytes = info.NumBytes;
          }
          else if ( macroInfo.Type == Types.MacroInfo.PseudoOpType.WORD )
          {
            var result = PODataWord( lineTokenInfos, lineIndex, 1, lineTokenInfos.Count - 1, info, parseLine, true, true, out lineSizeInBytes );
            if ( result == ParseLineResult.RETURN_NULL )
            {
              HadFatalError = true;
              return Lines;
            }
            else if ( result == ParseLineResult.CALL_CONTINUE )
            {
              continue;
            }
          }
          else if ( macroInfo.Type == Types.MacroInfo.PseudoOpType.WORD_BE )
          {
            var result = PODataWord( lineTokenInfos, lineIndex, 1, lineTokenInfos.Count - 1, info, parseLine, true, false, out lineSizeInBytes );
            if ( result == ParseLineResult.RETURN_NULL )
            {
              HadFatalError = true;
              return Lines;
            }
            else if ( result == ParseLineResult.CALL_CONTINUE )
            {
              continue;
            }
          }
          else if ( macroInfo.Type == Types.MacroInfo.PseudoOpType.DWORD )
          {
            var result = PODataDWord( lineTokenInfos, lineIndex, 1, lineTokenInfos.Count - 1, info, parseLine, true, true, out lineSizeInBytes );
            if ( result == ParseLineResult.RETURN_NULL )
            {
              HadFatalError = true;
              return Lines;
            }
            else if ( result == ParseLineResult.CALL_CONTINUE )
            {
              continue;
            }
          }
          else if ( macroInfo.Type == Types.MacroInfo.PseudoOpType.DWORD_BE )
          {
            var result = PODataDWord( lineTokenInfos, lineIndex, 1, lineTokenInfos.Count - 1, info, parseLine, true, false, out lineSizeInBytes );
            if ( result == ParseLineResult.RETURN_NULL )
            {
              HadFatalError = true;
              return Lines;
            }
            else if ( result == ParseLineResult.CALL_CONTINUE )
            {
              continue;
            }
          }
          else if ( macroInfo.Type == Types.MacroInfo.PseudoOpType.FILL )
          {
            var result = POFill( lineTokenInfos, lineIndex, info, parseLine, out lineSizeInBytes );
            if ( result == ParseLineResult.CALL_CONTINUE )
            {
              continue;
            }
            else if ( result == ParseLineResult.RETURN_NULL )
            {
              HadFatalError = true;
              return Lines;
            }
          }
          else if ( macroInfo.Type == Types.MacroInfo.PseudoOpType.LOOP_START )
          {
            var result = POLoopStart( lineTokenInfos, lineIndex, info, ref Lines, stackScopes, out lineSizeInBytes );
            if ( result == ParseLineResult.RETURN_NULL )
            {
              HadFatalError = true;
              return Lines;
            }
            else if ( result == ParseLineResult.CALL_CONTINUE )
            {
              continue;
            }
          }
          else if ( macroInfo.Type == Types.MacroInfo.PseudoOpType.TEXT )
          {
            POText( lineIndex, lineTokenInfos, info, parseLine, textCodeMapping, out lineSizeInBytes );
          }
          else if ( macroInfo.Type == Types.MacroInfo.PseudoOpType.MACRO )
          {
            string macroName = "";

            string outerFilename = "";
            int localLineIndex = 0;
            m_ASMFileInfo.FindTrueLineSource( lineIndex, out outerFilename, out localLineIndex );

            if ( POMacro( labelInFront, m_CurrentZoneName, m_ASMFileInfo.Macros, outerFilename, lineIndex, lineTokenInfos, stackScopes, out macroName ) )
            {
              if ( m_AssemblerSettings.MacroIsZone )
              {
                m_CurrentZoneName = macroName;
                info.Zone         = m_CurrentZoneName;
              }
            }
          }
          else if ( ( macroInfo.Type == Types.MacroInfo.PseudoOpType.END )
          ||        ( macroInfo.Type == Types.MacroInfo.PseudoOpType.LOOP_END ) )
          {
            if ( ( macroInfo.Type == Types.MacroInfo.PseudoOpType.LOOP_END )
            &&   ( m_AssemblerSettings.LoopEndHasNoScope ) )
            {
              continue;
            }
            var result = HandleScopeEnd( m_ASMFileInfo.Macros, stackScopes, lineTokenInfos, textCodeMapping, ref lineIndex, ref intermediateLineOffset, ref Lines );
            if ( result == ParseLineResult.CALL_CONTINUE )
            {
              --lineIndex;
              continue;
            }
            else if ( result == ParseLineResult.ERROR_ABORT )
            {
              HadFatalError = true;
              return Lines;
            }
          }
          else if ( macroInfo.Type == Types.MacroInfo.PseudoOpType.INCLUDE_BINARY )
          {
            var result = POIncludeBinary( lineTokenInfos, lineIndex, info, out lineSizeInBytes );
            if ( result == ParseLineResult.CALL_CONTINUE )
            {
              continue;
            }
            else if ( result == ParseLineResult.RETURN_NULL )
            {
              HadFatalError = true;
              return Lines;
            }
          }
          else if ( macroInfo.Type == Types.MacroInfo.PseudoOpType.INCLUDE_SOURCE )
          {
            string          subFilename = "";
            PathResolving   resolving = PathResolving.FROM_FILE;

            if ( m_AssemblerSettings.IncludeHasOnlyFilename )
            {
              // PDS style
              subFilename = TokensToExpression( lineTokenInfos, 1, lineTokenInfos.Count - 1 );
            }
            else if ( ( lineTokenInfos.Count == 2 )
            && ( lineTokenInfos[1].Type == Types.TokenInfo.TokenType.LITERAL_STRING ) )
            {
              // regular include
              subFilename = lineTokenInfos[1].Content.Substring( 1, lineTokenInfos[1].Length - 2 );
            }
            else if ( ( lineTokenInfos.Count > 3 )
            && ( lineTokenInfos[1].Content == "<" )
            && ( lineTokenInfos[lineTokenInfos.Count - 1].Content == ">" ) )
            {
              // library include
              subFilename = TokensToExpression( lineTokenInfos, 2, lineTokenInfos.Count - 3 );
              resolving = PathResolving.FROM_LIBRARIES_PATH;
            }
            else
            {
              AddError( lineIndex,
                        Types.ErrorCode.E1302_MALFORMED_MACRO,
                        "Expecting file name, either \"filename\" or \"library filename\"",
                        lineTokenInfos[0].StartPos,
                        lineTokenInfos[0].Length );
              HadFatalError = true;
              return Lines;
            }

            localIndex = 0;
            filename = "";
            if ( !m_ASMFileInfo.FindTrueLineSource( lineIndex, out filename, out localIndex ) )
            {
              DumpSourceInfos( OrigLines, Lines );
              AddError( lineIndex, Types.ErrorCode.E1401_INTERNAL_ERROR, "Includes caused a problem" );
              HadFatalError = true;
              return Lines;
            }
            ParseLineResult   plResult = POIncludeSource( resolving, subFilename, ParentFilename, ref lineIndex, ref Lines );
            if ( plResult == ParseLineResult.RETURN_NULL )
            {
              HadFatalError = true;
              return Lines;
            }
            else if ( plResult == ParseLineResult.CALL_CONTINUE )
            {
              continue;
            }
          }
          else if ( macroInfo.Type == Types.MacroInfo.PseudoOpType.IF )
          {
            if ( ScopeInsideMacroDefinition( stackScopes ) )
            {
              // Skip !if check inside macro definition

              // still need to add scope!
              Types.ScopeInfo scopeIfNotDef = new RetroDevStudio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.IF_OR_IFDEF );
              scopeIfNotDef.StartIndex = lineIndex;
              scopeIfNotDef.Active = false;

              stackScopes.Add( scopeIfNotDef );
              OnScopeAdded( scopeIfNotDef );
              continue;
            }

            Types.ScopeInfo scope = new RetroDevStudio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.IF_OR_IFDEF );
            scope.StartIndex = lineIndex;
            if ( !EvaluateTokens( lineIndex, lineTokenInfos, 1, lineTokenInfos.Count - 1, textCodeMapping, out SymbolInfo defineResult ) )
            {
              AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate expression: " + TokensToExpression( lineTokenInfos, 1, lineTokenInfos.Count - 1 ) );
              scope.Active = true;
              scope.IfChainHadActiveEntry = true;
            }
            else if ( defineResult.ToInteger() == 0 )
            {
              scope.Active = false;
            }
            else
            {
              scope.Active = true;
              scope.IfChainHadActiveEntry = true;
            }
            stackScopes.Add( scope );
            //Debug.Log( "add scope if " + lineIndex );
          }
          else if ( macroInfo.Type == Types.MacroInfo.PseudoOpType.ELSE )
          {
            if ( stackScopes.Count == 0 )
            {
              AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1309_ELSE_WITHOUT_IF, "Else statement without if encountered" );
            }
            else
            {
              stackScopes[stackScopes.Count - 1].Active = !stackScopes[stackScopes.Count - 1].IfChainHadActiveEntry;
              //stackScopes[stackScopes.Count - 1].Active = !stackScopes[stackScopes.Count - 1].Active;
              //Debug.Log( "toggle else " + lineIndex );
            }
          }
          else if ( macroInfo.Type == Types.MacroInfo.PseudoOpType.END_IF )
          {
            if ( stackScopes.Count == 0 )
            {
              AddError( lineIndex, Types.ErrorCode.E1310_END_IF_WITHOUT_SCOPE, "End if without scope" );
            }
            else
            {
              OnScopeRemoved( lineIndex, stackScopes );
              stackScopes.RemoveAt( stackScopes.Count - 1 );
            }
          }
          else if ( macroInfo.Type == MacroInfo.PseudoOpType.ALIGN_DASM )
          {
            var parseResult = POAlignDASM( lineTokenInfos, lineIndex, info, ref programStepPos, out lineSizeInBytes );
            if ( parseResult == ParseLineResult.RETURN_NULL )
            {
              HadFatalError = true;
              return Lines;
            }
          }
          else if ( macroInfo.Type == MacroInfo.PseudoOpType.REPEAT )
          {
            var parseResult = PORepeat( lineTokenInfos, lineIndex, ref Lines, info, stackScopes );
            if ( parseResult == ParseLineResult.RETURN_NULL )
            {
              HadFatalError = true;
              return Lines;
            }
            else if ( parseResult == ParseLineResult.CALL_CONTINUE )
            {
              continue;
            }
          }
          else if ( macroInfo.Type == Types.MacroInfo.PseudoOpType.ADD_INCLUDE_SOURCE )
          {
            string  folderPath = "";

            if ( m_AssemblerSettings.IncludeHasOnlyFilename )
            {
              // PDS style
              folderPath = TokensToExpression( lineTokenInfos, 1, lineTokenInfos.Count - 1 );
            }
            else
            {
              AddError( lineIndex,
                        Types.ErrorCode.E1302_MALFORMED_MACRO,
                        "Expecting directory name",
                        lineTokenInfos[0].StartPos,
                        lineTokenInfos[0].Length );
              HadFatalError = true;
              return Lines;
            }

            localIndex = 0;
            filename = "";
            if ( !m_ASMFileInfo.FindTrueLineSource( lineIndex, out filename, out localIndex ) )
            {
              DumpSourceInfos( OrigLines, Lines );
              AddError( lineIndex, Types.ErrorCode.E1401_INTERNAL_ERROR, "Includes caused a problem" );
              HadFatalError = true;
              return Lines;
            }

            ParseLineResult   plResult = POAddLibraryPath( folderPath, filename, ref lineIndex );
            if ( plResult == ParseLineResult.RETURN_NULL )
            {
              HadFatalError = true;
              return Lines;
            }
            else if ( plResult == ParseLineResult.CALL_CONTINUE )
            {
              continue;
            }
          }
          else if ( macroInfo.Type == Types.MacroInfo.PseudoOpType.ZONE )
          {
            POZone( stackScopes, lineIndex, info, lineTokenInfos, false );
          }
          else if ( macroInfo.Type == Types.MacroInfo.PseudoOpType.LZONE )
          {
            POZone( stackScopes, lineIndex, info, lineTokenInfos, true );
          }
          else if ( macroInfo.Type == Types.MacroInfo.PseudoOpType.MESSAGE )
          {
            if ( !ScopeInsideMacroDefinition( stackScopes ) )
            {
              AddOutputMessage( lineIndex, EvaluateAsText( lineIndex, lineTokenInfos, 1, lineTokenInfos.Count - 1, textCodeMapping ) );
            }
          }
          else if ( macroInfo.Type == Types.MacroInfo.PseudoOpType.BREAK_POINT )
          {
            m_ASMFileInfo.FixedBreakpoints.Add( m_CompileCurrentAddress );
            Debug.Log( $"TODO - Breakpoint needs to be stored {m_CompileCurrentAddress}" );
          }
          else if ( ( macroInfo.Type != MacroInfo.PseudoOpType.IGNORE )
          &&        ( macroInfo.Type != MacroInfo.PseudoOpType.ERROR ) )
          {
            AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1301_PSEUDO_OPERATION, "Unsupported pseudo op " + macroInfo.Keyword + " encountered" );
            HadFatalError = true;
            return Lines;
          }
          labelInFront = "";
          evaluatedContent = true;
        }

        if ( !ScopeInsideMacroDefinition( stackScopes ) )
        {
          if ( labelInFront.Length > 0 )
          {
            // a label
            if ( programStepPos != -1 )
            {
              // only if we have a valid address!
              var label = AddLabel( labelInFront, programStepPos, lineIndex, m_CurrentZoneName, tokenInFront.StartPos, tokenInFront.Length );

              label.Info = m_CurrentCommentSB.ToString();
              m_CurrentCommentSB = new StringBuilder();

              if ( ( !evaluatedContent )
              &&   ( lineTokenInfos.Count >= 1 ) )
              {
                // potentially label which is an opcode?
                if ( m_Processor.Opcodes.ContainsKey( labelInFront.ToLower() ) )
                {
                  AddError( lineIndex,
                            Types.ErrorCode.E1300_OPCODE_AMBIGIOUS,
                            "Could not determine correct opcode for " + labelInFront,
                            tokenInFront.StartPos,
                            tokenInFront.Length );
                }
                else
                {
                  AddError( lineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "Syntax error: " + TokensToExpression( lineTokenInfos ) );
                }
              }
            }
            else
            {
              // a label as define, set to value -1
              AddLabel( labelInFront, -1, lineIndex, m_CurrentZoneName, tokenInFront.StartPos, tokenInFront.Length );
              //AddError( lineIndex, Types.ErrorCode.E0002_CODE_WITHOUT_START_ADDRESS, "Can't provide value if no start address is set", tokenInFront.StartPos, tokenInFront.Length );
            }
          }
          else if ( ( !evaluatedContent )
          &&        ( lineTokenInfos.Count > 0 ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "Syntax error: " + TokensToExpression( lineTokenInfos ) );
          }
          programStepPos += lineSizeInBytes;
          //Debug.Log( "line " + lineIndex + ", address = " + programStepPos.ToString( "X4" ) + ", startaddress = " + info.AddressStart.ToString( "X4" ) );

          sizeInBytes += lineSizeInBytes;
          trueCompileCurrentAddress += lineSizeInBytes;

          info.AddressStart = m_CompileCurrentAddress;
          if ( m_CompileCurrentAddress != -1 )
          {
            m_CompileCurrentAddress += lineSizeInBytes;
          }
          if ( info.AddressSource.StartsWith( "*" ) )
          {
            // set program counter
            m_CompileCurrentAddress = info.AddressStart;
          }
          if ( info.PseudoPCOffset == -2 )
          {
            // !REALPC
            m_CompileCurrentAddress = trueCompileCurrentAddress;
            info.AddressStart = trueCompileCurrentAddress;
          }
          else if ( info.PseudoPCOffset != -1 )
          {
            m_CompileCurrentAddress = info.PseudoPCOffset;
          }
          else if ( m_CompileCurrentAddress != trueCompileCurrentAddress )
          {
            info.PseudoPCOffset = m_CompileCurrentAddress - lineSizeInBytes;
          }
          programStepPos = m_CompileCurrentAddress;
        }
        if ( info.PseudoPCOffset >= 0 )
        {
          info.AddressStart = info.PseudoPCOffset;
        }
        //Debug.Log( info.LineIndex.ToString() + ":" + info.AddressStart.ToString( "X4" ) + "/" + info.PseudoPCOffset.ToString() + " " + info.Line );
        //if ( hadCommentInLine )
        if ( !hadPseudoOp )
        {
          // we had a comment after some other content, clear for next line
          m_CurrentCommentSB = new StringBuilder();
        }
      }

      if ( ScopeInsideLoop( stackScopes ) )
      {
        foreach ( var scope in stackScopes )
        {
          if ( scope.Loop != null )
          {
            AddError( scope.Loop.LineIndex, Types.ErrorCode.E1008_MISSING_LOOP_END, "Loop " + scope.Loop.Label + ", started in line " + ( scope.Loop.LineIndex + 1 ) + ", is missing end statement" );
          }
        }
      }
      if ( stackScopes.Count > 0 )
      {
        foreach ( Types.ScopeInfo scope in stackScopes )
        {
          if ( scope.Type == ScopeInfo.ScopeType.REPEAT )
          {
            AddError( scope.StartIndex, Types.ErrorCode.E1005_MISSING_CLOSING_BRACKET, "REPEAT is not properly ended" );
          }
          else if ( scope.Loop == null )
          {
            AddError( scope.StartIndex, Types.ErrorCode.E1005_MISSING_CLOSING_BRACKET, "Missing closing bracket" );
          }
        }
      }
      foreach ( Types.MacroFunctionInfo macroFunction in m_ASMFileInfo.Macros.Values )
      {
        if ( macroFunction.LineEnd == -1 )
        {
          AddError( macroFunction.LineIndex, Types.ErrorCode.E1008_MISSING_LOOP_END, "Macro function " + macroFunction.Name + ", started in line " + ( macroFunction.LineIndex + 1 ) + ", is missing end statement" );
        }
      }

      if ( m_ASMFileInfo.Banks.Count > 0 )
      {
        // fill from previous bank
        Types.ASM.LineInfo info = new Types.ASM.LineInfo();

        info.LineIndex = Lines.Length;
        info.HideInPreprocessedOutput = hideInPreprocessedOutput;
        Types.ASM.BankInfo lastBank = m_ASMFileInfo.Banks[m_ASMFileInfo.Banks.Count - 1];

        if ( sizeInBytes <= lastBank.SizeInBytesStart + lastBank.SizeInBytes )
        {
          // we need to fill
          int delta = lastBank.SizeInBytesStart + lastBank.SizeInBytes - sizeInBytes;

          info.NumBytes = delta;
          info.LineData = new GR.Memory.ByteBuffer( (uint)delta );
        }
        else
        {
          int overflow = sizeInBytes - lastBank.SizeInBytesStart;
          AddError( Lines.Length, Types.ErrorCode.E1101_BANK_TOO_BIG, "Bank " + lastBank.Number + " contains too much bytes, " + lastBank.SizeInBytes + " chosen, " + overflow + " encountered" );
        }
      }

      /*
      foreach ( var line in ASMFileInfo.LineInfo )
      {
        ASMFileInfo.FindTrueLineSource( line.Key, out string filename, out int localLineIndex );
        Debug.Log( "Line " + line.Key.ToString( "D3" ) + " ($" + line.Value.AddressStart.ToString( "X4" ) + ": " + line.Value.NumBytes + " bytes: " + line.Value.Line + ", from line " + localLineIndex );
      }*/

      m_CompileCurrentAddress = -1;
      return Lines;
    }



    private ParseLineResult HandleLabelInFront( int lineIndex, LineInfo info, List<TokenInfo> lineTokenInfos, ref string upToken,
          List<ScopeInfo> stackScopes, Map<byte, byte> textCodeMapping, 
          ref int programStepPos, ref int trueCompileCurrentAddress,
          ref string labelInFront, ref TokenInfo tokenInFront, ref string parseLine )
    {
      // not a token, not a macro, must be a label in front
      labelInFront = lineTokenInfos[0].Content;
      tokenInFront = lineTokenInfos[0];

      if ( !ScopeInsideMacroDefinition( stackScopes ) )
      {
        if ( programStepPos != -1 )
        {
          // only add if we know the start address!
          AddLabel( labelInFront, programStepPos, lineIndex, m_CurrentZoneName, lineTokenInfos[0].StartPos, lineTokenInfos[0].Length );
        }
        else
        {
          // label without value, like a define

          if ( ( m_AssemblerSettings.MacroKeywordAfterName )
          &&   ( lineTokenInfos.Count >= 2 )
          &&   ( lineTokenInfos[1].Content == MacroByType( MacroInfo.PseudoOpType.MACRO ) ) )
          {
            // a PDS style macro definition
          }
          else
          {
            AddLabel( labelInFront, -1, lineIndex, m_CurrentZoneName, lineTokenInfos[0].StartPos, lineTokenInfos[0].Length );
          }
        }
      }

      // cut off label for neededparsedexpression
      if ( lineTokenInfos.Count > 1 )
      {
        info.Line = parseLine.Substring( lineTokenInfos[1].StartPos );
        parseLine = info.Line;

        // shift all tokens back
        //lineTokenInfos.RemoveAt( 0 );
      }
      else
      {
        parseLine = "";
        info.Line = "";
      }
      lineTokenInfos.RemoveAt( 0 );

      // butt ugly woraround, we could have e.g. "label *=*+2"
      if ( IsDefine( lineTokenInfos ) )
      {
        // a define
        var callReturn = PODefine( stackScopes, lineTokenInfos, lineIndex, info, textCodeMapping, ref programStepPos, ref trueCompileCurrentAddress );
        if ( callReturn == ParseLineResult.ERROR_ABORT )
        {
          return callReturn;
        }
        return ParseLineResult.CALL_CONTINUE;
      }
      if ( lineTokenInfos.Count == 0 )
      {
        upToken = "";
      }
      else
      {
        upToken = lineTokenInfos[0].Content.ToUpper();

        // hack - macro call after label
        if ( ( lineTokenInfos.Count >= 2 )
        &&   ( lineTokenInfos[0].Type == RetroDevStudio.Types.TokenInfo.TokenType.OPERATOR )
        &&   ( lineTokenInfos[0].Content == "+" )
        &&   ( lineTokenInfos[0].StartPos + lineTokenInfos[0].Length == lineTokenInfos[1].StartPos )
        &&   ( lineTokenInfos[1].Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_GLOBAL ) )
        {
          lineTokenInfos[0].Type = RetroDevStudio.Types.TokenInfo.TokenType.CALL_MACRO;
          lineTokenInfos[0].Content = "+" + lineTokenInfos[1].Content;
          lineTokenInfos[0].Length += lineTokenInfos[1].Length;
          lineTokenInfos.RemoveAt( 1 );
          upToken = lineTokenInfos[0].Content.ToUpper();
        }
      }
      return ParseLineResult.OK;
    }



    private string ListValidValues( List<Opcode.ValidValue> Values )
    {
      var sb = new StringBuilder();

      for ( int i = 0; i < Values.Count; ++i )
      {
        sb.Append( Values[i].Key );
        if ( i + 2 < Values.Count )
        {
          sb.Append( ", " );
        }
        else if ( i + 1 < Values.Count )
        {
          sb.Append( " or " );
        }
      }
      return sb.ToString();
    }



    private bool ValidateExpressionValueRange( ref long ByteValue, LineInfo Info, int Round, out int ValueRangeListIndex )
    {
      ValueRangeListIndex = 0;
      if ( ( Info.Opcode == null )
      ||   ( Info.Opcode.ParserExpressions.Count == 0 ) )
      {
        return true;
      }
      var curExpression = Info.Opcode.ParserExpressions[Round];

      if ( ( curExpression.Type != Opcode.OpcodePartialExpression.EXPRESSION_24BIT )
      &&   ( curExpression.Type != Opcode.OpcodePartialExpression.EXPRESSION_16BIT )
      &&   ( curExpression.Type != Opcode.OpcodePartialExpression.EXPRESSION_32BIT )
      &&   ( curExpression.Type != Opcode.OpcodePartialExpression.EXPRESSION_8BIT )
      &&   ( curExpression.Type != Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT )
      &&   ( curExpression.Type != Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT )
      &&   ( curExpression.Type != Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_24BIT )
      &&   ( curExpression.Type != Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT ) )
      {
        return true;
      }
      if ( Round >= curExpression.ValidValues.Count )
      {
        return true;
      }

      if ( ( curExpression.Type == Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT )
      ||   ( curExpression.Type == Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT )
      ||   ( curExpression.Type == Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_24BIT )
      ||   ( curExpression.Type == Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT ) )
      {
        // encapsulated expression with two validvalues have no list
        if ( curExpression.ValidValues.Count == 2 )
        {
          return true;
        }
        // otherwise it's the list in between those 3
        Round = 1;
      }

      var values = curExpression.ValidValues[Round].ValidValues;
      if ( values.Count == 0 )
      {
        return true;
      }
      ValueRangeListIndex = Round;
      string  stringizedValue = ByteValue.ToString();
      var matchingValue = values.FirstOrDefault( v => v.Key == stringizedValue );
      if ( matchingValue == null )
      {
        return false;
      }
      ByteValue = (long)matchingValue.ReplacementValue;
      return true;
    }



    private void ApplyOpcodePatch( LineInfo Info, uint Value, int ExpressionIndex )
    {
      Opcode.OpcodeExpression   currentExpression = null;

      int expIndex = 0;
      int trueExpindex = 0;
      int replacementShift = 0;
      while ( expIndex < Info.Opcode.ParserExpressions.Count )
      {
        if ( ( Info.Opcode.ParserExpressions[expIndex].Type == Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT )
        ||   ( Info.Opcode.ParserExpressions[expIndex].Type == Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT )
        ||   ( Info.Opcode.ParserExpressions[expIndex].Type == Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_24BIT )
        ||   ( Info.Opcode.ParserExpressions[expIndex].Type == Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT )
        ||   ( Info.Opcode.ParserExpressions[expIndex].Type == Opcode.OpcodePartialExpression.COMPLEX )
        ||   ( Info.Opcode.ParserExpressions[expIndex].Type == Opcode.OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE )
        ||   ( Info.Opcode.ParserExpressions[expIndex].Type == Opcode.OpcodePartialExpression.EXPRESSION_16BIT_RELATIVE )
        ||   ( Info.Opcode.ParserExpressions[expIndex].Type == Opcode.OpcodePartialExpression.EXPRESSION_8BIT )
        ||   ( Info.Opcode.ParserExpressions[expIndex].Type == Opcode.OpcodePartialExpression.EXPRESSION_16BIT )
        ||   ( Info.Opcode.ParserExpressions[expIndex].Type == Opcode.OpcodePartialExpression.EXPRESSION_24BIT )
        ||   ( Info.Opcode.ParserExpressions[expIndex].Type == Opcode.OpcodePartialExpression.EXPRESSION_32BIT ) )
        {
          if ( trueExpindex == ExpressionIndex )
          {
            currentExpression = Info.Opcode.ParserExpressions[expIndex];
            replacementShift = currentExpression.ReplacementValueShift;

            if ( currentExpression.Type == Opcode.OpcodePartialExpression.COMPLEX )
            {
              // in case of complex the replacement shift is taken from the expression sub expression
              var subExpression = currentExpression.ValidValues.FirstOrDefault( vv => vv.Expression != Opcode.OpcodePartialExpression.UNUSED );
              if ( subExpression != null )
              {
                replacementShift = subExpression.ReplacementValueShift;
              }
            }

            // swap bytes
            if ( m_Processor.LittleEndian )
            {
              if ( ( currentExpression.Type == Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT )
              ||   ( currentExpression.Type == Opcode.OpcodePartialExpression.EXPRESSION_16BIT ) )
              {
                Value = (ushort)( ( ( Value & 0xff00 ) >> 8 ) | ( ( Value & 0x00ff ) << 8 ) );
              }
              if ( ( currentExpression.Type == Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_24BIT )
              ||   ( currentExpression.Type == Opcode.OpcodePartialExpression.EXPRESSION_24BIT ) )
              {
                Value = (uint)( ( ( Value & 0xff0000 ) >> 16 ) | ( ( Value & 0x00ff ) << 16 ) | ( Value & 0x00ff00 ) );
              }
              if ( ( currentExpression.Type == Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT )
              ||   ( currentExpression.Type == Opcode.OpcodePartialExpression.EXPRESSION_32BIT ) )
              {
                Value = (uint)( ( ( Value & 0xff000000 ) >> 24 )
                              | ( ( Value & 0x00ff0000 ) >> 8 )
                              | ( ( Value & 0x0000ff00 ) << 8 ) 
                              | ( ( Value & 0x000000ff ) << 24 ) );
              }
            }
            break;
          }
          ++trueExpindex;
        }
        ++expIndex;
      }
      if ( currentExpression == null )
      {
        return;
      }

      currentExpression.ResultingReplacementValue = (ulong)Value << replacementShift;

      switch ( currentExpression.Type )
      {
        case Opcode.OpcodePartialExpression.EXPRESSION_8BIT:
        case Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT:
          Value = Value & 0xff;
          break;
        case Opcode.OpcodePartialExpression.EXPRESSION_16BIT:
        case Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT:
          Value = Value & 0xffff;
          break;
        case Opcode.OpcodePartialExpression.EXPRESSION_24BIT:
        case Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_24BIT:
          Value = Value & 0xffffff;
          break;
        case Opcode.OpcodePartialExpression.EXPRESSION_32BIT:
        case Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT:
          Value = Value & 0xffffffff;
          break;
      }

      // patch into line data
      if ( ( currentExpression.ResultingReplacementValue & 0xff00000000000000ul ) != 0 )
      {
        Info.LineData.SetU64NetworkOrderAt( (int)Info.LineData.Length - 8, Info.LineData.UInt64NetworkOrderAt( (int)Info.LineData.Length - 8 ) | currentExpression.ResultingReplacementValue );
        return;
      }
      if ( ( currentExpression.ResultingReplacementValue & 0xff000000000000ul ) > 0 )
      {
        Info.LineData.SetU8At( (int)Info.LineData.Length - 7, (byte)( Info.LineData.ByteAt( (int)Info.LineData.Length - 7 ) | ( ( currentExpression.ResultingReplacementValue >> 48 ) & 0xff ) ) );
        Info.LineData.SetU16NetworkOrderAt( (int)Info.LineData.Length - 6, (ushort)( Info.LineData.UInt16NetworkOrderAt( (int)Info.LineData.Length - 6 ) | ( ( currentExpression.ResultingReplacementValue >> 32 ) & 0xffff ) ) );
        Info.LineData.SetU32NetworkOrderAt( (int)Info.LineData.Length - 4, (uint)( Info.LineData.UInt32NetworkOrderAt( (int)Info.LineData.Length - 4 ) | (uint)currentExpression.ResultingReplacementValue ) );
      }
      if ( ( currentExpression.ResultingReplacementValue & 0xff0000000000ul ) > 0 )
      {
        Info.LineData.SetU16NetworkOrderAt( (int)Info.LineData.Length - 6, (ushort)( Info.LineData.UInt16NetworkOrderAt( (int)Info.LineData.Length - 6 ) | ( ( currentExpression.ResultingReplacementValue >> 32 ) & 0xffff ) ) );
        Info.LineData.SetU32NetworkOrderAt( (int)Info.LineData.Length - 4, (uint)( Info.LineData.UInt32NetworkOrderAt( (int)Info.LineData.Length - 4 ) | (uint)currentExpression.ResultingReplacementValue ) );
      }
      if ( ( currentExpression.ResultingReplacementValue & 0xff00000000 ) > 0 )
      {
        Info.LineData.SetU8At( (int)Info.LineData.Length - 5, (byte)( Info.LineData.ByteAt( (int)Info.LineData.Length - 5 ) | ( ( currentExpression.ResultingReplacementValue >> 32 ) & 0xff ) ) );
        Info.LineData.SetU32NetworkOrderAt( (int)Info.LineData.Length - 4, (uint)( Info.LineData.UInt32NetworkOrderAt( (int)Info.LineData.Length - 4 ) | (uint)currentExpression.ResultingReplacementValue ) );
      }
      if ( ( currentExpression.ResultingReplacementValue & 0xff000000 ) > 0 )
      {
        Info.LineData.SetU32NetworkOrderAt( (int)Info.LineData.Length - 4, (uint)( Info.LineData.UInt32NetworkOrderAt( (int)Info.LineData.Length - 4 ) | (uint)currentExpression.ResultingReplacementValue ) );
      }
      else if ( ( currentExpression.ResultingReplacementValue & 0x00ff0000 ) > 0 )
      {
        Info.LineData.SetU24NetworkOrderAt( (int)Info.LineData.Length - 3, (uint)( Info.LineData.UInt24NetworkOrderAt( (int)Info.LineData.Length - 3 ) | (uint)currentExpression.ResultingReplacementValue ) );
      }
      else if ( ( currentExpression.ResultingReplacementValue & 0x0000ff00 ) > 0 )
      {
        Info.LineData.SetU16NetworkOrderAt( (int)Info.LineData.Length - 2, (ushort)( Info.LineData.UInt16NetworkOrderAt( (int)Info.LineData.Length - 2 ) | (ushort)currentExpression.ResultingReplacementValue ) );
      }
      else
      {
        Info.LineData.SetU8At( (int)Info.LineData.Length - 1 , (byte)( Info.LineData.ByteAt( (int)Info.LineData.Length - 1 ) | (byte)currentExpression.ResultingReplacementValue ) );
      }
    }



    private void AppendOpcodeValue( LineInfo Info, ulong ResultingOpcodePatchValue )
    {
      HandleM65OpcodePrefixes( Info );

      ulong    opcodeValue = Info.Opcode.ByteValue;
      if ( ResultingOpcodePatchValue != ulong.MaxValue )
      {
        opcodeValue |= ResultingOpcodePatchValue;
      }
      if ( ( opcodeValue & 0xff00000000000000 ) != 0 )
      {
        Info.LineData.AppendU64NetworkOrder( opcodeValue );
        return;
      }
      if ( ( opcodeValue & 0xff000000000000 ) != 0 )
      {
        Info.LineData.AppendU8( (byte)( opcodeValue >> 48 ) );
        Info.LineData.AppendU16NetworkOrder( (ushort)( opcodeValue >> 32 ) );
        Info.LineData.AppendU32NetworkOrder( (uint)opcodeValue );
        return;
      }
      if ( ( opcodeValue & 0xff0000000000 ) != 0 )
      {
        Info.LineData.AppendU16NetworkOrder( (ushort)( opcodeValue >> 32 ) );
        Info.LineData.AppendU32NetworkOrder( (uint)opcodeValue );
        return;
      }
      if ( ( opcodeValue & 0xff00000000 ) != 0 )
      {
        Info.LineData.AppendU8( (byte)( opcodeValue >> 32 ) );
        Info.LineData.AppendU32NetworkOrder( (uint)opcodeValue );
        return;
      }
      if ( ( opcodeValue & 0xff000000 ) != 0 )
      {
        Info.LineData.AppendU32NetworkOrder( (uint)opcodeValue );
        return;
      }
      if ( ( opcodeValue & 0x00ff0000 ) != 0 )
      {
        Info.LineData.AppendU8( (byte)( opcodeValue >> 16 ) );
        Info.LineData.AppendU16NetworkOrder( (ushort)opcodeValue );
        return;
      }
      if ( ( opcodeValue & 0x0000ff00 ) != 0 )
      {
        Info.LineData.AppendU16NetworkOrder( (ushort)opcodeValue );
        return;
      }
      Info.LineData.AppendU8( (byte)opcodeValue );
    }



    private int SizeOfOpcode( ulong ByteValue )
    {
      if ( ( ByteValue & 0xff00000000000000 ) != 0 )
      {
        return 8;
      }

      if ( ( ByteValue & 0xff000000000000 ) != 0 )
      {
        return 7;
      }
      if ( ( ByteValue & 0xff0000000000 ) != 0 )
      {
        return 6;
      }
      if ( ( ByteValue & 0xff00000000 ) != 0 )
      {
        return 5;
      }
      if ( ( ByteValue & 0xff000000 ) != 0 )
      {
        return 4;
      }
      if ( ( ByteValue & 0x00ff0000 ) != 0 )
      {
        return 3;
      }
      if ( ( ByteValue & 0x0000ff00 ) != 0 )
      {
        return 2;
      }
      return 1;
    }



    private void DetermineActiveZone( List<ScopeInfo> Scopes )
    {
      for ( int i = Scopes.Count - 1; i >= 0; --i )
      {
        if ( Scopes[i].Type == ScopeInfo.ScopeType.ZONE )
        {
          m_CurrentZoneName = Scopes[i].Name;
          return;
        }
      }
      m_CurrentZoneName = m_CurrentGlobalZoneName;
    }



    private void StripInternalBrackets( TokenInfo Token )
    {
      if ( ( Token.Content.StartsWith( AssemblerSettings.INTERNAL_OPENING_BRACE ) )
      &&   ( Token.Content.EndsWith( AssemblerSettings.INTERNAL_CLOSING_BRACE ) ) )
      {
        Token.Content = Token.Content.Substring( 1, Token.Content.Length - 2 );
      }
    }



    private void StripInternalBrackets( List<TokenInfo> LineTokenInfos )
    {
      while ( ( LineTokenInfos.Count >= 2 )
      &&      ( LineTokenInfos[0].Content == AssemblerSettings.INTERNAL_OPENING_BRACE )
      &&      ( LineTokenInfos.Last().Content == AssemblerSettings.INTERNAL_CLOSING_BRACE ) )
      {
        LineTokenInfos.RemoveAt( 0 );
        LineTokenInfos.RemoveAt( LineTokenInfos.Count - 1 );
      }
    }



    private void StripInternalBrackets( List<TokenInfo> LineTokenInfos, int CenterTokenIndex )
    {
      if ( ( CenterTokenIndex < 1 )
      ||   ( CenterTokenIndex + 1 >= LineTokenInfos.Count ) )
      {
        return;
      }
      while ( ( CenterTokenIndex >= 1 )
      &&      ( CenterTokenIndex + 1 < LineTokenInfos.Count )
      &&      ( LineTokenInfos[CenterTokenIndex - 1].Content == AssemblerSettings.INTERNAL_OPENING_BRACE )
      &&      ( LineTokenInfos[CenterTokenIndex + 1].Content == AssemblerSettings.INTERNAL_CLOSING_BRACE )
      &&      ( IsTokenLabel( LineTokenInfos[CenterTokenIndex].Type ) ) )
      {
        LineTokenInfos.RemoveAt( CenterTokenIndex + 1 );
        LineTokenInfos.RemoveAt( CenterTokenIndex - 1 );
      }
    }



    private void POPreprocessedList( List<TokenInfo> lineTokenInfos, int lineIndex, LineInfo info, ref bool HideInPreprocessedOutput )
    {
      if ( lineTokenInfos.Count != 2 )
      {
        AddError( lineIndex, ErrorCode.E1302_MALFORMED_MACRO, "Malformed pseudo op !list, expecting arguments on or off" );
        return;
      }
      if ( lineTokenInfos[1].Content.ToUpper() == "OFF" )
      {
        HideInPreprocessedOutput = true;
        info.HideInPreprocessedOutput = true;
      }
      else if ( lineTokenInfos[1].Content.ToUpper() == "ON" )
      {
        HideInPreprocessedOutput = false;
      }
      else
      {
        AddError( lineIndex, ErrorCode.E1302_MALFORMED_MACRO, "Malformed pseudo op !list, expecting arguments on or off" );
      }
    }



    private ParseLineResult PODefine( List<ScopeInfo> stackScopes, List<TokenInfo> lineTokenInfos, int lineIndex, LineInfo info, Map<byte,byte> textCodeMapping, ref int programStepPos, ref int trueCompileCurrentAddress )
    {
      if ( ScopeInsideMacroDefinition( stackScopes ) )
      {
        return ParseLineResult.CALL_CONTINUE;
      }
      StripInternalBrackets( lineTokenInfos, 1 );
      int equPos = lineTokenInfos[1].StartPos;
      string operatorToken = lineTokenInfos[1].Content;

      string defineName = lineTokenInfos[0].Content;
      if ( !m_AssemblerSettings.CaseSensitive )
      {
        defineName = defineName.ToUpper();
      }
      int   defineLength = lineTokenInfos[lineTokenInfos.Count - 1].StartPos + lineTokenInfos[lineTokenInfos.Count - 1].Length - ( equPos + lineTokenInfos[1].Content.Length );
      string defineValue = TokensToExpression( lineTokenInfos, 2, lineTokenInfos.Count - 2 );
      // avoid detecting as label
      if ( m_AssemblerSettings.LabelsMustBeAtStartOfLine )
      {
        defineValue = " " + defineValue;
      }

      List<Types.TokenInfo>  valueTokens = ParseTokenInfo( defineValue, 0, defineValue.Length, textCodeMapping );

      if ( defineName == "*" )
      {
        // set program step
        info.AddressSource = "*";

        List<Types.TokenInfo> tokens = ParseTokenInfo( defineValue, 0, defineValue.Length, textCodeMapping );
        if ( ( tokens.Count > 0 )
        &&   ( tokens[tokens.Count - 1].Type == TokenInfo.TokenType.LITERAL_STRING ) )
        {
          info.AddressSource = "*" + tokens[tokens.Count - 1].Content;
          tokens.RemoveAt( tokens.Count - 1 );
        }
        if ( !EvaluateTokens( lineIndex, tokens, textCodeMapping, out SymbolInfo newStepPosSymbol ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate * position value", lineTokenInfos[0].StartPos, lineTokenInfos[0].Length );
          return ParseLineResult.ERROR_ABORT;
        }

        var origPos = CreateIntegerSymbol( programStepPos );
        if ( !HandleAssignmentOperator( lineIndex, lineTokenInfos, origPos, operatorToken, newStepPosSymbol, out SymbolInfo resultingValue ) )
        {
          return ParseLineResult.ERROR_ABORT;
        }

        programStepPos = resultingValue.ToInt32();
        m_CompileCurrentAddress = programStepPos;
        trueCompileCurrentAddress = programStepPos;

        info.AddressStart = programStepPos;
      }
      else
      {
        if ( ScopeInsideMacroDefinition( stackScopes ) )
        {
          return ParseLineResult.CALL_CONTINUE;
        }
        if ( !EvaluateTokens( lineIndex, valueTokens, textCodeMapping, out SymbolInfo addressSymbol ) )
        {
          if ( !IsPlainAssignment( operatorToken ) )
          {
            AddError( lineIndex, ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Assignment operators must be solvable in one pass" );
            return ParseLineResult.ERROR_ABORT;
          }
          AddUnparsedLabel( defineName, defineValue, lineIndex );
        }
        else
        {
          EvaluateTokens( lineIndex, lineTokenInfos, 0, 1, textCodeMapping, out SymbolInfo originalValue );
          if ( !HandleAssignmentOperator( lineIndex, lineTokenInfos, originalValue, operatorToken, addressSymbol, out SymbolInfo resultingValue ) )
          {
            return ParseLineResult.ERROR_ABORT;
          }

          if ( resultingValue.Type == SymbolInfo.Types.CONSTANT_REAL_NUMBER )
          {
            AddConstantF( defineName, resultingValue, lineIndex, m_CurrentCommentSB.ToString(), m_CurrentZoneName, lineTokenInfos[0].StartPos, lineTokenInfos[0].Length );
          }
          else if ( resultingValue.Type == SymbolInfo.Types.CONSTANT_STRING )
          {
            AddConstantString( defineName, resultingValue, lineIndex, m_CurrentCommentSB.ToString(), m_CurrentZoneName, lineTokenInfos[0].StartPos, lineTokenInfos[0].Length );
          }
          else
          {
            AddConstant( defineName, resultingValue, lineIndex, m_CurrentCommentSB.ToString(), m_CurrentZoneName, lineTokenInfos[0].StartPos, lineTokenInfos[0].Length );
          }
        }
      }
      m_CurrentCommentSB = new StringBuilder();

      return ParseLineResult.CALL_CONTINUE;
    }



    private bool IsPlainAssignment( string operatorToken )
    {
      return m_AssemblerSettings.PlainAssignmentOperatos.Contains( operatorToken );
    }



    private bool HandleAssignmentOperator( int lineIndex, List<TokenInfo> lineTokenInfos, SymbolInfo originalValue, string operatorToken, SymbolInfo newValue, out SymbolInfo resultingValue )
    {
      resultingValue = null;
      switch ( operatorToken )
      {
        case "+=":
          if ( originalValue == null )
          {
            AddError( lineIndex, ErrorCode.E1009_INVALID_VALUE, "Cannot modify not existing variable" );
            return false;
          }
          if ( originalValue.Type == SymbolInfo.Types.CONSTANT_STRING )
          {
            resultingValue = CreateStringSymbol( originalValue.ToString() + newValue.ToString() );
            return true;
          }
          if ( NeedsPromotionToNumber( originalValue, newValue ) )
          {
            resultingValue = CreateNumberSymbol( originalValue.ToNumber() + newValue.ToNumber() );
            return true;
          }
          if ( originalValue.Type != newValue.Type )
          {
            AddError( lineIndex, ErrorCode.E1011_TYPE_MISMATCH, "Mismatching types, cannot evaluate" );
            return false;
          }
          resultingValue = CreateIntegerSymbol( originalValue.ToInteger() + newValue.ToInteger() );
          return true;
        case "-=":
          if ( originalValue == null )
          {
            AddError( lineIndex, ErrorCode.E1009_INVALID_VALUE, "Cannot modify not existing variable" );
            return false;
          }
          if ( NeedsPromotionToNumber( originalValue, newValue ) )
          {
            resultingValue = CreateNumberSymbol( originalValue.ToNumber() - newValue.ToNumber() );
            return true;
          }
          if ( originalValue.Type != newValue.Type )
          {
            AddError( lineIndex, ErrorCode.E1011_TYPE_MISMATCH, "Mismatching types, cannot evaluate" );
            return false;
          }
          resultingValue = CreateIntegerSymbol( originalValue.ToInteger() - newValue.ToInteger() );
          return true;
        case "*=":
          if ( originalValue == null )
          {
            AddError( lineIndex, ErrorCode.E1009_INVALID_VALUE, "Cannot modify not existing variable" );
            return false;
          }
          if ( NeedsPromotionToNumber( originalValue, newValue ) )
          {
            resultingValue = CreateNumberSymbol( originalValue.ToNumber() * newValue.ToNumber() );
            return true;
          }
          if ( originalValue.Type != newValue.Type )
          {
            AddError( lineIndex, ErrorCode.E1011_TYPE_MISMATCH, "Mismatching types, cannot evaluate" );
            return false;
          }
          resultingValue = CreateIntegerSymbol( originalValue.ToInteger() * newValue.ToInteger() );
          return true;
        case "/=":
          if ( originalValue == null )
          {
            AddError( lineIndex, ErrorCode.E1009_INVALID_VALUE, "Cannot modify not existing variable" );
            return false;
          }
          if ( NeedsPromotionToNumber( originalValue, newValue ) )
          {
            if ( newValue.ToNumber() == 0 )
            {
              AddError( lineIndex, ErrorCode.E1009_INVALID_VALUE, "Divide by zero detected" );
              return false;
            }

            resultingValue = CreateNumberSymbol( originalValue.ToNumber() / newValue.ToNumber() );
            return true;
          }
          if ( originalValue.Type != newValue.Type )
          {
            AddError( lineIndex, ErrorCode.E1011_TYPE_MISMATCH, "Mismatching types, cannot evaluate" );
            return false;
          }
          if ( newValue.ToInteger() == 0 )
          {
            AddError( lineIndex, ErrorCode.E1009_INVALID_VALUE, "Divide by zero detected" );
            return false;
          }
          resultingValue = CreateIntegerSymbol( originalValue.ToInteger() / newValue.ToInteger() );
          return true;
        case "%=":
          if ( originalValue == null )
          {
            AddError( lineIndex, ErrorCode.E1009_INVALID_VALUE, "Cannot modify not existing variable" );
            return false;
          }
          if ( NeedsPromotionToNumber( originalValue, newValue ) )
          {
            if ( newValue.ToNumber() == 0 )
            {
              AddError( lineIndex, ErrorCode.E1009_INVALID_VALUE, "Divide by zero detected" );
              return false;
            }

            resultingValue = CreateNumberSymbol( originalValue.ToNumber() % newValue.ToNumber() );
            return true;
          }
          if ( originalValue.Type != newValue.Type )
          {
            AddError( lineIndex, ErrorCode.E1011_TYPE_MISMATCH, "Mismatching types, cannot evaluate" );
            return false;
          }
          if ( newValue.ToInteger() == 0 )
          {
            AddError( lineIndex, ErrorCode.E1009_INVALID_VALUE, "Divide by zero detected" );
            return false;
          }
          resultingValue = CreateIntegerSymbol( originalValue.ToInteger() % newValue.ToInteger() );
          return true;
        case "&=":
          if ( originalValue == null )
          {
            AddError( lineIndex, ErrorCode.E1009_INVALID_VALUE, "Cannot modify not existing variable" );
            return false;
          }
          if ( !originalValue.IsInteger() )
          {
            AddError( lineIndex, ErrorCode.E1009_INVALID_VALUE, "Type must be integer for binary and" );
            return false;
          }
          if ( !newValue.IsInteger() )
          {
            AddError( lineIndex, ErrorCode.E1009_INVALID_VALUE, "Type must be integer for binary and" );
            return false;
          }
          resultingValue = CreateIntegerSymbol( originalValue.ToInteger() & newValue.ToInteger() );
          return true;
        case "<<=":
          if ( originalValue == null )
          {
            AddError( lineIndex, ErrorCode.E1009_INVALID_VALUE, "Cannot modify not existing variable" );
            return false;
          }
          if ( originalValue.Type != newValue.Type )
          {
            AddError( lineIndex, ErrorCode.E1011_TYPE_MISMATCH, "Mismatching types, cannot evaluate" );
            return false;
          }
          resultingValue = CreateIntegerSymbol( originalValue.ToInteger() << newValue.ToInt32() );
          return true;
        case ">>=":
          if ( originalValue == null )
          {
            AddError( lineIndex, ErrorCode.E1009_INVALID_VALUE, "Cannot modify not existing variable" );
            return false;
          }
          if ( originalValue.Type != newValue.Type )
          {
            AddError( lineIndex, ErrorCode.E1011_TYPE_MISMATCH, "Mismatching types, cannot evaluate" );
            return false;
          }
          resultingValue = CreateIntegerSymbol( originalValue.ToInteger() >> newValue.ToInt32() );
          return true;
        default:
          if ( m_AssemblerSettings.PlainAssignmentOperatos.Contains( operatorToken ) )
          {
            resultingValue = newValue;
            return true;
          }
          break;
      }
      AddError( lineIndex, ErrorCode.E1012_IMPLEMENTATION_MISSING, $"Implementation for operator '{operatorToken}' is missing!" );
      return false;
    }



    private bool NeedsPromotionToNumber( SymbolInfo OriginalValue, SymbolInfo NewValue )
    {
      if ( ( ( OriginalValue.IsInteger() )
      &&     ( NewValue.Type == SymbolInfo.Types.CONSTANT_REAL_NUMBER ) )
      ||   ( ( OriginalValue.Type == SymbolInfo.Types.CONSTANT_REAL_NUMBER )
      &&     ( NewValue.IsInteger() ) )
      ||   ( ( OriginalValue.Type == SymbolInfo.Types.CONSTANT_REAL_NUMBER )
      &&     ( NewValue.Type == SymbolInfo.Types.CONSTANT_REAL_NUMBER ) ) )
      {
        return true;
      }
      return false;
    }



    private string ListKeys( IEnumerable<string> LookupList )
    {
      StringBuilder   sb = new StringBuilder();

      int numEntries = LookupList.Count();

      for ( int i = 0; i < numEntries; ++i )
      {
        if ( i + 1 == numEntries )
        {
          sb.Append( " or " );
        }
        else if ( i > 0 )
        {
          sb.Append( ", " );
        }
        sb.Append( LookupList.ElementAt( i ) );
      }

      return sb.ToString();
    }



    private void HandleM65OpcodePrefixes( LineInfo info )
    {
      if ( m_Processor.Name != "M65" )
      {
        return;
      }

      // m65 long mode is using prefixed "nop"
      if ( ( info.OpcodeUsingLongMode )
      &&   ( info.Opcode.NumNopsToPrefix == 2 ) )
      {
        // long mode combined with quad mode, prefix NEG:NEG:EOM
        info.LineData.AppendU16( 0x4242 );
        info.LineData.AppendU8( 0xea );
        info.NumBytes += 3;
      }
      else if ( info.OpcodeUsingLongMode )
      {
        info.LineData.AppendU8( 0xea );
        ++info.NumBytes;
      }
      else
      {
        for ( int i = 0; i < info.Opcode.NumNopsToPrefix; ++i )
        {
          info.LineData.AppendU8( 0x42 );
          ++info.NumBytes;
        }
      }
    }



    private void POZone( List<ScopeInfo> stackScopes, int lineIndex, LineInfo info, List<TokenInfo> lineTokenInfos, bool AutoGlobalLabel )
    {
      bool  scopedZone = false;

      if ( lineTokenInfos[lineTokenInfos.Count - 1].Content == "{" )
      {
        lineTokenInfos.RemoveAt( lineTokenInfos.Count - 1 );

        // TODO - check of zonescope exists, no nestes zone scopes!

        Types.ScopeInfo   zoneScope = new RetroDevStudio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.ZONE );
        zoneScope.StartIndex  = lineIndex;
        stackScopes.Add( zoneScope );
        OnScopeAdded( zoneScope );
        scopedZone = true;
      }
      if ( lineTokenInfos.Count > 2 )
      {
        AddError( lineIndex, Types.ErrorCode.E1303_MALFORMED_ZONE_DESCRIPTOR, "Expected single zone descriptor" );
      }
      else
      {
        var   zoneToken = lineTokenInfos[0];
        if ( lineTokenInfos.Count == 1 )
        {
          // anonymous zone -> all upper case 
          // TODO - really?
          //m_CurrentZoneName = "ANON_SCOPE_" + lineIndex.ToString();
          // back to global zone 
          DetermineActiveZone( stackScopes );
          return;
        }
        else
        {
          m_CurrentZoneName       = DeQuote( lineTokenInfos[1].Content );
          if ( !scopedZone )
          {
            m_CurrentGlobalZoneName = m_CurrentZoneName;
          }
          else
          {
            stackScopes.Last().Name = m_CurrentZoneName;
          }
          zoneToken = lineTokenInfos[1];
        }
        info.Zone = m_CurrentZoneName;

        AddZone( m_CurrentZoneName, lineIndex, zoneToken.StartPos, zoneToken.Length );
        if ( AutoGlobalLabel )
        {
          var label = AddLabel( m_CurrentZoneName, m_CompileCurrentAddress, lineIndex, m_CurrentZoneName, zoneToken.StartPos, zoneToken.Length );

          label.Info = m_CurrentCommentSB.ToString();
          m_CurrentCommentSB = new StringBuilder();
        }
      }
    }



    private bool IsLabelInFront( List<TokenInfo> lineTokenInfos, string UpToken )
    {
      if ( ( lineTokenInfos[0].Type != RetroDevStudio.Types.TokenInfo.TokenType.CALL_MACRO )
      &&   ( lineTokenInfos[0].Type != RetroDevStudio.Types.TokenInfo.TokenType.PSEUDO_OP )
      &&   ( ( !m_Processor.Opcodes.ContainsKey( UpToken.ToLower() ) )
      &&     ( ( m_AssemblerSettings.POPrefix.Length == 0 )
      &&       ( ( ( m_AssemblerSettings.LabelsMustBeAtStartOfLine )
      &&           ( lineTokenInfos[0].StartPos == 0 ) )
      ||         ( !m_AssemblerSettings.PseudoOps.ContainsKey( UpToken ) ) )
      ||     ( ( m_AssemblerSettings.POPrefix.Length > 0 )
      &&       ( !UpToken.StartsWith( m_AssemblerSettings.POPrefix ) ) ) ) ) )
      {
        return true;
      }
      return false;
    }



    private bool IsOpcode( TokenInfo.TokenType TokenType )
    {
      if ( ( TokenType == RetroDevStudio.Types.TokenInfo.TokenType.OPCODE )
      ||   ( TokenType == RetroDevStudio.Types.TokenInfo.TokenType.OPCODE_DIRECT_VALUE )
      ||   ( TokenType == RetroDevStudio.Types.TokenInfo.TokenType.OPCODE_FIXED_NON_ZP )
      ||   ( TokenType == RetroDevStudio.Types.TokenInfo.TokenType.OPCODE_FIXED_ZP ) )
      {
        return true;
      }
      return false;
    }



    private void AdjustLabelCasing( List<TokenInfo> lineTokenInfos )
    {
      if ( !m_AssemblerSettings.CaseSensitive )
      {
        // turn all labels/macros to upper case
        foreach ( Types.TokenInfo token in lineTokenInfos )
        {
          if ( IsTokenLabel( token.Type ) )
          {
            token.Content = token.Content.ToUpper();
          }
        }
      }
    }



    private void DetectPDSOrDASMMacroCall( Map<GR.Generic.Tupel<string,int>, MacroFunctionInfo> macroFunctions, List<TokenInfo> lineTokenInfos )
    {
      // PDS?
      if ( ( lineTokenInfos.Count >= 1 )
      &&   ( m_AssemblerSettings.MacroFunctionCallPrefix.Count == 0 )
      &&   ( lineTokenInfos[0].Type == Types.TokenInfo.TokenType.LABEL_GLOBAL )
      &&   ( macroFunctions.Keys.Any( m => m.first == lineTokenInfos[0].Content ) ) )
      {
        lineTokenInfos[0].Type = TokenInfo.TokenType.CALL_MACRO;
      }

      if ( ( lineTokenInfos.Count >= 1 )
      &&   ( lineTokenInfos[0].Type == Types.TokenInfo.TokenType.LABEL_GLOBAL )
      &&   ( m_AssemblerSettings.MacroFunctionCallPrefix.Count != 0 )
      &&   ( lineTokenInfos[0].Content.StartsWith( m_AssemblerSettings.MacroFunctionCallPrefix[0] ) )
      &&   ( macroFunctions.Keys.Any( m => m.first == lineTokenInfos[0].Content ) ) )
      {
        lineTokenInfos[0].Type = TokenInfo.TokenType.CALL_MACRO;
      }
    }



    private static void DetectInternalLabels( int lineIndex, List<TokenInfo> lineTokenInfos )
    {
      if ( ( lineTokenInfos[0].Type != RetroDevStudio.Types.TokenInfo.TokenType.CALL_MACRO )
      &&   ( ( lineTokenInfos[0].Content.StartsWith( "-" ) )
      ||     ( lineTokenInfos[0].Content.StartsWith( "+" ) ) ) )
      {
        lineTokenInfos[0].Content = InternalLabelPrefix + lineTokenInfos[0].Content + InternalLabelPostfix + lineIndex.ToString();
        lineTokenInfos[0].Type = TokenInfo.TokenType.LABEL_INTERNAL;
      }
    }



    private void StartNewZoneOnGlobalLabel( LineInfo info, List<TokenInfo> lineTokenInfos, string upToken )
    {
      if ( m_AssemblerSettings.GlobalLabelsAutoZone )
      {
        if ( ( lineTokenInfos[0].Type != RetroDevStudio.Types.TokenInfo.TokenType.CALL_MACRO )
        &&   ( ( !m_Processor.Opcodes.ContainsKey( upToken.ToLower() ) )
        &&   ( ( ( m_AssemblerSettings.POPrefix.Length == 0 )
        &&       ( !m_AssemblerSettings.PseudoOps.ContainsKey( upToken ) ) )
        ||     ( ( m_AssemblerSettings.POPrefix.Length > 0 )
        &&       ( !upToken.StartsWith( m_AssemblerSettings.POPrefix ) ) ) ) ) )
        {
          if ( lineTokenInfos[0].Type == Types.TokenInfo.TokenType.LABEL_GLOBAL )
          {
            // auto-zone
            m_CurrentZoneName = lineTokenInfos[0].Content;
            info.Zone = m_CurrentZoneName;
          }
        }
      }
    }



    private bool IsDefine( List<TokenInfo> LineTokenInfos )
    {
      if ( ( LineTokenInfos.Count >= 3 )
      &&   ( m_AssemblerSettings.DefineSeparatorKeywords.ContainsValue( LineTokenInfos[1].Content )
      &&   ( ( m_AssemblerSettings.POPrefix.Length == 0 )
      ||     ( !LineTokenInfos[0].Content.StartsWith( m_AssemblerSettings.POPrefix ) ) ) ) )
      {
        return true;
      }
      return false;
    }



    private void PrefixZoneToLocalLabels( ref string cheapLabelParent, List<TokenInfo> lineTokenInfos, ref string upToken )
    {
      if ( ( lineTokenInfos[0].Type == TokenInfo.TokenType.PSEUDO_OP )
      &&   ( m_AssemblerSettings.PseudoOps.ContainsKey( upToken ) )
      &&   ( m_AssemblerSettings.PseudoOps[upToken].Type == MacroInfo.PseudoOpType.MACRO ) )
      {
        // no prefixing for macro arguments!
        return;
      }

      for ( int i = 0; i < lineTokenInfos.Count; ++i )
      {
        if ( ( lineTokenInfos[i].Type == Types.TokenInfo.TokenType.LABEL_GLOBAL )
        &&   ( i == 0 ) )
        {
          cheapLabelParent = lineTokenInfos[i].Content;
        }
        if ( ( lineTokenInfos[i].Type == Types.TokenInfo.TokenType.LABEL_LOCAL )
        ||   ( lineTokenInfos[i].Type == Types.TokenInfo.TokenType.LABEL_GLOBAL ) )
        {
          if ( ( m_AssemblerSettings.LabelPostfix.Length > 0 )
          &&   ( lineTokenInfos[i].Content.EndsWith( m_AssemblerSettings.LabelPostfix ) ) )
          {
            lineTokenInfos[i].Length -= 1;
            lineTokenInfos[i].Content = null;
          }
        }
        if ( lineTokenInfos[i].Type == Types.TokenInfo.TokenType.LABEL_LOCAL )
        {
          // ugly hack to avoid prefixing relative paths
          if ( ( lineTokenInfos[i].Content != ".." )
          &&   ( lineTokenInfos[i].Content != "." ) )   // DASM alternative to *
          {
            lineTokenInfos[i].Content = m_CurrentZoneName + lineTokenInfos[i].Content;
            if ( i == 0 )
            {
              upToken = lineTokenInfos[i].Content.ToUpper();
            }
          }
        }
        if ( lineTokenInfos[i].Type == Types.TokenInfo.TokenType.LABEL_CHEAP_LOCAL )
        {
          lineTokenInfos[i].Content = cheapLabelParent + lineTokenInfos[i].Content;
          if ( i == 0 )
          {
            upToken = lineTokenInfos[i].Content.ToUpper();
          }
        }
      }
    }



    private ParseLineResult PORepeat( List<TokenInfo> lineTokenInfos, int lineIndex, ref string[] Lines, LineInfo info, List<ScopeInfo> Scopes )
    {
      if ( ScopeInsideMacroDefinition( Scopes ) )
      {
        // Skip if inside macro definition
        return ParseLineResult.CALL_CONTINUE;
      }


      List<List<TokenInfo>>   lineParams;

      if ( !ParseLineInParameters( lineTokenInfos, 1, lineTokenInfos.Count - 1, lineIndex, false, out lineParams ) )
      {
        return ParseLineResult.ERROR_ABORT;
      }
      if ( lineParams.Count != 1 )
      {
        AddError( lineIndex, ErrorCode.E1000_SYNTAX_ERROR, "Expected one argument" );
        return ParseLineResult.ERROR_ABORT;
      }
      if ( !EvaluateTokens( lineIndex, lineParams[0], info.LineCodeMapping, out SymbolInfo numRepeatsSymbol ) )
      {
        return ParseLineResult.ERROR_ABORT;
      }
      int numRepeats = numRepeatsSymbol.ToInt32();
      if ( ( numRepeats < 0 )
      ||   ( numRepeats > 99999 ) )
      {
        AddError( lineIndex,
                  ErrorCode.E1107_ARGUMENT_OUT_OF_BOUNDS,
                  "Number of repeats needs to be >= 0 and <= 99999",
                  lineParams[0][0].StartPos,
                  lineParams[0][lineParams[0].Count - 1].EndPos - lineParams[0][0].StartPos );
        return ParseLineResult.ERROR_ABORT;
      }

      int nextLineIndex = lineIndex + 1;
      if ( nextLineIndex >= Lines.Length )
      {
        AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1008_MISSING_LOOP_END, "REPEAT at end of code encountered" );
        return ParseLineResult.ERROR_ABORT;
      }
      int loopLength = 1;
      string[] tempContent = new string[loopLength * ( numRepeats - 1 )];

      for ( int i = 0; i < numRepeats - 1; ++i )
      {
        System.Array.Copy( Lines, lineIndex + 1, tempContent, i * loopLength, loopLength );
      }

      string[] replacementLines = RelabelLocalLabelsForLoop( tempContent, Scopes, lineIndex, info.LineCodeMapping );

      string[] newLines = new string[Lines.Length + replacementLines.Length];

      System.Array.Copy( Lines, 0, newLines, 0, lineIndex + 1 + loopLength );
      System.Array.Copy( replacementLines, 0, newLines, lineIndex + 1 + loopLength, replacementLines.Length );

      // replaces the REPEND
      System.Array.Copy( Lines, nextLineIndex + 1, newLines, lineIndex + 1 + loopLength + replacementLines.Length, Lines.Length - nextLineIndex - 1 );

      // adjust source infos to make lookup work correctly
      string outerFilename = "";
      int outerLineIndex = -1;
      m_ASMFileInfo.FindTrueLineSource( lineIndex + 1, out outerFilename, out outerLineIndex );

      for ( int i = 0; i < numRepeats - 1; ++i )
      {
        Types.ASM.SourceInfo sourceInfo = new Types.ASM.SourceInfo();
        sourceInfo.Filename = outerFilename;
        sourceInfo.FullPath = outerFilename;
        sourceInfo.GlobalStartLine = lineIndex + 1 + ( 1 + i ) * loopLength;
        sourceInfo.LineCount = loopLength;
        sourceInfo.LocalStartLine = outerLineIndex;

        InsertSourceInfo( sourceInfo );
      }


      Lines = newLines;

      //Debug.Log( "New total " + Lines.Length + " lines" );
      return ParseLineResult.CALL_CONTINUE;
    }



    // TODO - add expression parsing (parenthesis)
    public bool ParseLineInParameters( List<TokenInfo> lineTokenInfos, int Offset, int Count, int LineIndex, bool AllowEmptyParams, out List<List<TokenInfo>> lineParams )
    {
      int     paramStartIndex = Offset;
      int     bracketStackDepth = 0;

      lineParams = new List<List<TokenInfo>>();

      for ( int i = 0; i < Count; ++i )
      {
        var token = lineTokenInfos[Offset + i];

        if ( IsOpeningBraceChar( token.Content ) )
        {
          ++bracketStackDepth;
          continue;
        }
        if ( IsClosingBraceChar( token.Content ) )
        {
          --bracketStackDepth;
          continue;
        }
        if ( bracketStackDepth > 0 )
        {
          continue;
        }
        if ( ( token.Type == TokenInfo.TokenType.SEPARATOR )
        &&   ( token.Content == "," ) )
        {
          if ( ( !AllowEmptyParams )
          &&   ( Offset + i == paramStartIndex ) )
          {
            // empty?
            AddError( LineIndex, ErrorCode.E1000_SYNTAX_ERROR, "Empty Parameter, expected a value or expression", token.StartPos, token.Length );
            return false;
          }
          lineParams.Add( lineTokenInfos.GetRange( paramStartIndex, Offset + i - paramStartIndex ) );

          paramStartIndex = Offset + i + 1;
          continue;
        }
      }
      if ( ( !AllowEmptyParams )
      &&   ( paramStartIndex >= Offset + Count ) )
      {
        // empty?
        AddError( LineIndex, ErrorCode.E1000_SYNTAX_ERROR, "Empty Parameter, expected a value or expression", lineTokenInfos[lineTokenInfos.Count - 1].StartPos, 1 );
        return false;
      }
      lineParams.Add( lineTokenInfos.GetRange( paramStartIndex, Offset + Count - paramStartIndex ) );

      return true;
    }



    private string DeQuote( string Content )
    {
      if ( ( Content.StartsWith( "\"" ) )
      &&   ( Content.EndsWith( "\"" )
      &&   ( Content.Length >=  2 ) ) )
      {
        return Content.Substring( 1, Content.Length - 2 );
      }
      return Content;
    }



    private bool MatchesMacroByType( string Token, MacroInfo.PseudoOpType Type )
    {
      string    upToken = Token.ToUpper();

      foreach ( var macro in m_AssemblerSettings.PseudoOps )
      {
        if ( ( macro.Value.Type == Type )
        &&   ( macro.Value.Keyword == upToken ) )
        {
          return true;
        }
      }
      return false;
    }



    private bool TokenStartsScope( List<TokenInfo> Tokens, int StartTokenIndex, out Types.ScopeInfo.ScopeType Type )
    {
      Type = ScopeInfo.ScopeType.UNKNOWN;
      if ( Tokens[StartTokenIndex].Type == TokenInfo.TokenType.PSEUDO_OP )
      {
        if ( MatchesMacroByType( Tokens[StartTokenIndex].Content, MacroInfo.PseudoOpType.ADDRESS ) )
        {
          Type = ScopeInfo.ScopeType.ADDRESS;
          return true;
        }
        if ( MatchesMacroByType( Tokens[StartTokenIndex].Content, MacroInfo.PseudoOpType.ZONE ) )
        {
          Type = ScopeInfo.ScopeType.ZONE;
          return true;
        }
        // a ACME style !macro with opening bracket
        if ( ( MatchesMacroByType( Tokens[StartTokenIndex].Content, MacroInfo.PseudoOpType.MACRO ) )
        &&   ( Tokens[Tokens.Count - 1].Content == "{" ) )
        {
          Type = ScopeInfo.ScopeType.MACRO_FUNCTION;
          return true;
        }
      }
      return false;
    }



    private ParseLineResult HandleLineSeparators( ref int lineIndex, List<TokenInfo> lineTokenInfos, ref string[] Lines, string ParentFilename )
    {
      bool    doesContainSeparator = false;
      int     numSeparators = 0;
      for ( int tokenIndex = 0; tokenIndex < lineTokenInfos.Count; ++tokenIndex )
      {
        var token = lineTokenInfos[tokenIndex];

        if ( ( token.Type == RetroDevStudio.Types.TokenInfo.TokenType.SEPARATOR )
        &&   ( m_AssemblerSettings.LineSeparatorChars.Contains( token.Content ) ) )
        {
          doesContainSeparator = true;
          ++numSeparators;
        }
      }
      if ( doesContainSeparator )
      {
        string[]      newLines = new string[numSeparators + 1];

        int     partStartIndex = 0;
        int     partIndex = 0;
        for ( int tokenIndex = 0; tokenIndex < lineTokenInfos.Count; ++tokenIndex )
        {
          var token = lineTokenInfos[tokenIndex];

          if ( ( token.Type == RetroDevStudio.Types.TokenInfo.TokenType.SEPARATOR )
          &&   ( m_AssemblerSettings.LineSeparatorChars.Contains( token.Content ) ) )
          {
            newLines[partIndex] = TokensToExpression( lineTokenInfos, partStartIndex, tokenIndex - partStartIndex );
            partStartIndex = tokenIndex + 1;
            ++partIndex;
          }
        }
        if ( partStartIndex < lineTokenInfos.Count )
        {
          newLines[partIndex] = TokensToExpression( lineTokenInfos, partStartIndex, lineTokenInfos.Count - partStartIndex );
        }
        // if any part was null, set to empty
        for ( int i = 0; i < newLines.Length; ++i )
        {
          if ( newLines[i] == null )
          {
            newLines[i] = "";
          }
        }

        Types.ASM.SourceInfo sourceInfo = new Types.ASM.SourceInfo();
        sourceInfo.Filename         = ParentFilename;
        sourceInfo.FullPath         = ParentFilename;
        sourceInfo.GlobalStartLine  = lineIndex;
        sourceInfo.LineCount        = newLines.Length - 1;
        sourceInfo.FilenameParent   = ParentFilename;

        string  dummyFile = "";
        int     localFileIndex = -1;
        m_ASMFileInfo.FindTrueLineSource( lineIndex, out dummyFile, out localFileIndex );
        sourceInfo.LocalStartLine   = localFileIndex;

        SourceInfoLog( "-include at global index " + lineIndex );
        SourceInfoLog( "-has " + sourceInfo.LineCount + " lines" );

        InsertSourceInfo( sourceInfo, true, true );

        string[] result = new string[Lines.Length + sourceInfo.LineCount];

        System.Array.Copy( Lines, 0, result, 0, lineIndex );
        System.Array.Copy( newLines, 0, result, lineIndex, newLines.Length );

        // this keeps the !source line in the final code, makes working with source infos easier though
        System.Array.Copy( Lines, lineIndex + 1, result, lineIndex + newLines.Length, Lines.Length - lineIndex - 1 );

        // replace !source with empty line (otherwise source infos would have one line more!)
        //result[lineIndex] = "";

        Lines = result;

        m_ASMFileInfo.LineInfo.Remove( lineIndex );

        --lineIndex;
        return ParseLineResult.CALL_CONTINUE;
      }
      return ParseLineResult.OK;
    }



    private bool TokenIsPseudoPC( TokenInfo Token )
    {
      if ( ( Token.Type == TokenInfo.TokenType.PSEUDO_OP )
      &&   ( Token.Content.ToUpper() == MacroByType( MacroInfo.PseudoOpType.PSEUDO_PC ) ) )
      {
        return true;
      }
      return false;
    }



    private bool TokenIsConditionalThatStartsScope( TokenInfo Token )
    {
      if ( ( Token.Type == TokenInfo.TokenType.PSEUDO_OP )
      &&   ( ( Token.Content.ToUpper() == MacroByType( MacroInfo.PseudoOpType.IF ).ToUpper() )
      ||     ( Token.Content.ToUpper() == MacroByType( MacroInfo.PseudoOpType.IFDEF ).ToUpper() )
      ||     ( Token.Content.ToUpper() == MacroByType( MacroInfo.PseudoOpType.IFNDEF ).ToUpper() ) ) )
      {
        return true;
      }
      return false;
    }



    private bool TokenIsConditionalThatEndsScope( TokenInfo Token )
    {
      if ( ( Token.Type == TokenInfo.TokenType.PSEUDO_OP )
      &&   ( ( Token.Content.ToUpper() == MacroByType( MacroInfo.PseudoOpType.END_IF ).ToUpper() )
      ||     ( Token.Content.ToUpper() == MacroByType( MacroInfo.PseudoOpType.ELSE ).ToUpper() )
      ||     ( Token.Content.ToUpper() == MacroByType( MacroInfo.PseudoOpType.END ).ToUpper() ) ) )
      {
        return true;
      }
      if ( Token.Content == "}" )
      {
        return true;
      }
      return false;
    }

            


    private bool FindStartOfComment( string parseLine, out int commentPos )
    {
 	    bool firstNonWhiteSpaceCharFound = false;
      bool insideStringLiteral = false;
      bool insideCharLiteral = false;

      commentPos = -1;

      for ( int i = 0; i < parseLine.Length; ++i )
      {
        char aChar = parseLine[i];
        if ( aChar == '\'' )
        {
          if ( !insideStringLiteral )
          {
            insideCharLiteral = !insideCharLiteral;
          }
        }
        if ( aChar == '"' )
        {
          insideStringLiteral = !insideStringLiteral;
        }
        if ( ( insideCharLiteral )
        ||   ( insideStringLiteral ) )
        {
          continue;
        }
        if ( ( !firstNonWhiteSpaceCharFound )
        &&   ( aChar != ' ' )
        &&   ( aChar != '\t' ) )
        {
          firstNonWhiteSpaceCharFound = true;
          if ( ( m_AssemblerSettings.AllowedTokenStartChars.ContainsKey( RetroDevStudio.Types.TokenInfo.TokenType.COMMENT_IF_FIRST_CHAR ) )
          &&   ( m_AssemblerSettings.AllowedTokenStartChars[RetroDevStudio.Types.TokenInfo.TokenType.COMMENT_IF_FIRST_CHAR].IndexOf( aChar ) != -1 ) )
          {
            commentPos = 0;
            return true;
          }
          if ( m_AssemblerSettings.AllowedTokenStartChars[RetroDevStudio.Types.TokenInfo.TokenType.COMMENT].IndexOf( aChar ) != -1 )
          {
            commentPos = i;
            return true;
          }
        }
        if ( m_AssemblerSettings.AllowedTokenStartChars[RetroDevStudio.Types.TokenInfo.TokenType.COMMENT].IndexOf( aChar ) != -1 )
        {
          commentPos = i;
          return true;
        }
      }
      return false;
    } 



    void OnScopeRemoved( int LineIndex, List<Types.ScopeInfo> Scopes )
    {
      /*
      var scope = Scopes[Scopes.Count - 1];
      string  doc;
      int       localLine = -1;

      ASMFileInfo.FindTrueLineSource( LineIndex, out doc, out localLine );
      Debug.Log( "Scope " + scope.Type + " removed in " + doc + " at " + ( localLine + 1 ) );*/
    }



    private void OnScopeAdded( Types.ScopeInfo scope )
    {
      /*
      string  doc;
      int       localLine = -1;

      ASMFileInfo.FindTrueLineSource( scope.StartIndex, out doc, out localLine );
      Debug.Log( "Scope " + scope.Type + " added in " + doc + " at " + ( localLine + 1 ) );*/
    }



    private void PORealPC( Types.ASM.LineInfo info )
    {
      info.PseudoPCOffset = -2;
    }



    private void POFor( List<Types.ScopeInfo> stackScopes, string zoneName, ref int intermediateLineOffset, int lineIndex, List<Types.TokenInfo> lineTokenInfos, GR.Collections.Map<byte, byte> TextCodeMapping )
    {
      if ( ScopeInsideMacroDefinition( stackScopes ) )
      {
        // ignore for loop if we are inside a macro definition!

        // add dummy scope so !ends properly match
        Types.LoopInfo loop = new Types.LoopInfo();

        loop.LineIndex = lineIndex;

        Types.ScopeInfo   scope = new RetroDevStudio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.LOOP );

        scope.Active = true;
        scope.Loop = loop;
        scope.StartIndex = lineIndex;
        stackScopes.Add( scope );
        return;
      }
      if ( lineTokenInfos.Count < 5 )
      {
        AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Malformed macro, expect !FOR <Variable> = <Start Value Expression> TO <End Value Expression [STEP] <Step Value Expression>" );
      }
      else
      {
        if ( ( ( lineTokenInfos[1].Type != RetroDevStudio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
        &&     ( lineTokenInfos[1].Type != RetroDevStudio.Types.TokenInfo.TokenType.LABEL_LOCAL ) )
        ||   ( lineTokenInfos[2].Content != "=" ) )
        {
          AddError( lineIndex,
                    RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO,
                    "Malformed macro, expect !FOR <Variable> = <Start Value Expression> TO <End Value Expression [STEP] <Step Value Expression>",
                    lineTokenInfos[1].StartPos,
                    lineTokenInfos[lineTokenInfos.Count - 1].EndPos + 1 - lineTokenInfos[1].StartPos );
        }
        else
        {
          int     indexTo = FindTokenContent( lineTokenInfos, "to" );
          int     indexStep = FindTokenContent( lineTokenInfos, "step" );
          int     stepValue = 1;
          bool    hadError = false;

          if ( ( indexTo == -1 )
          ||   ( indexTo < 3 ) )
          {
            AddError( lineIndex,
                      RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO,
                      "Malformed macro, expect !FOR <Variable> = <Start Value Expression> TO <End Value Expression [STEP] <Step Value Expression>",
                      lineTokenInfos[1].StartPos,
                      lineTokenInfos[lineTokenInfos.Count - 1].EndPos + 1 - lineTokenInfos[1].StartPos );
            hadError = true;
          }
          if ( indexStep != -1 )
          {
            if ( !EvaluateTokens( lineIndex, lineTokenInfos, indexStep + 1, lineTokenInfos.Count - indexStep - 1, TextCodeMapping, out SymbolInfo stepValueSymbol ) )
            {
              AddError( lineIndex,
                        RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO,
                        "Malformed macro, expect !FOR <Variable> = <Start Value Expression> TO <End Value Expression [STEP] <Step Value Expression>",
                        lineTokenInfos[indexStep + 1].StartPos,
                        lineTokenInfos[lineTokenInfos.Count - 1].EndPos + 1 - lineTokenInfos[indexStep + 1].StartPos );
              hadError = true;
            }
            else
            {
              stepValue = stepValueSymbol.ToInt32();
              if ( stepValue == 0 )
              {
                AddError( lineIndex,
                          RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO,
                          "Value of step must not be zero",
                          lineTokenInfos[indexStep + 1].StartPos,
                          lineTokenInfos[lineTokenInfos.Count - 1].EndPos + 1 - lineTokenInfos[indexStep + 1].StartPos );
                hadError = true;
              }
            }
          }
          else
          {
            indexStep = lineTokenInfos.Count;
          }

          if ( !hadError )
          {
            var endValueTokens = lineTokenInfos.GetRange( indexTo + 1, indexStep - indexTo - 1 );
            int startValue = 0;
            int endValue = 0;
            if ( !EvaluateTokens( lineIndex, lineTokenInfos, 3, indexTo - 3, TextCodeMapping, out SymbolInfo startValueSymbol ) )
            {
              AddError( lineIndex,
                        RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO,
                        "Could not evaluate start value",
                        lineTokenInfos[3].StartPos,
                        lineTokenInfos[indexTo - 3].EndPos + 1 - lineTokenInfos[3].StartPos );
              hadError = true;
            }
            else if ( !EvaluateTokens( lineIndex, lineTokenInfos, indexTo + 1, indexStep - indexTo - 1, TextCodeMapping, out SymbolInfo endValueSymbol ) )
            {
              AddError( lineIndex,
                        RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO,
                        "Could not evaluate end value",
                        lineTokenInfos[indexTo + 1].StartPos,
                        lineTokenInfos[indexStep - indexTo - 1].EndPos + 1 - lineTokenInfos[indexTo + 1].StartPos );
              hadError = true;
            }
            else
            {
              startValue = startValueSymbol.ToInt32();
              endValue = endValueSymbol.ToInt32();
              if ( ( stepValue < 0 )
              &&   ( endValue >= startValue ) )
              {
                AddError( lineIndex,
                          RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO,
                          "End value must be lower than start value with negative step",
                          lineTokenInfos[indexTo + 1].StartPos,
                          lineTokenInfos[indexStep - 1].EndPos + 1 - lineTokenInfos[indexTo + 1].StartPos );
                hadError = true;
              }
              else if ( ( stepValue > 0 )
              &&        ( endValue < startValue ) )
              {
                AddError( lineIndex,
                          RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO,
                          "End value must be higher than start value with positive step",
                          lineTokenInfos[indexTo + 1].StartPos,
                          lineTokenInfos[indexStep - indexTo - 1].EndPos + 1 - lineTokenInfos[indexTo + 1].StartPos );
                hadError = true;
              }
            }
            if ( !hadError )
            {
              Types.LoopInfo loop = new Types.LoopInfo();

              loop.Label                      = lineTokenInfos[1].Content;
              loop.LineIndex                  = lineIndex;
              loop.StartValue                 = startValue;
              loop.EndValue                   = endValue;
              loop.StepValue                  = stepValue;
              loop.CurrentValue               = startValue;
              loop.EndValueTokens             = endValueTokens;
              loop.EndValueTokensTextmapping  = TextCodeMapping;

              //Debug.Log( $"Begin Loop for {loop.Label}" );

              Types.ScopeInfo   scope = new RetroDevStudio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.LOOP );
              // TODO - active depends on parent scopes
              scope.Active = true;
              scope.Loop = loop;
              scope.StartIndex = lineIndex;
              stackScopes.Add( scope );

              AddTempLabel( loop.Label, lineIndex + 1, -1, CreateIntegerSymbol( startValue ), "" ).IsForVariable = true;

              //AddLabel( loop.Label, loop.StartValue, lineIndex + 1, zoneName, lineTokenInfos[1].StartPos, lineTokenInfos[1].Length );
              intermediateLineOffset = 0;

              //Debug.Log( "add for label for " + loop.Label );
              //Debug.Log( "Add for loop scope for " + loop.Label + " at " + lineIndex );
            }
          }
        }
      }
    }



    // relabels local labels in macros to avoid clashes with duplicate calls - spares parameters
    private string[] RelabelLocalLabelsForMacro( string[] Lines, List<Types.ScopeInfo> Scopes, int lineIndex, string functionName, Types.MacroFunctionInfo functionInfo, List<string> paramName, List<string> param, GR.Collections.Map<byte, byte> TextCodeMapping, out int LineIndexInsideMacro )
    {
      string[] replacementLines = new string[functionInfo.LineEnd - functionInfo.LineIndex - 1];
      int replacementLineIndex = 0;

      LineIndexInsideMacro = -1;
      ClearErrorInfo();

      for ( int i = functionInfo.LineIndex + 1; i < functionInfo.LineEnd; ++i )
      {
        List<Types.TokenInfo> tokens = ParseTokenInfo( functionInfo.Content[i - functionInfo.LineIndex - 1], 0, functionInfo.Content[i - functionInfo.LineIndex - 1].Length, TextCodeMapping );
        if ( tokens == null )
        {
          // there was an error!
          LineIndexInsideMacro = i;
          return null;
        }
        // text replace macro
        bool replacedParam = false;

        for ( int j = 0; j < tokens.Count; ++j )
        {
          if ( ( j + 2 < tokens.Count )
          &&   ( ( tokens[j].Type == TokenInfo.TokenType.LABEL_GLOBAL )
          ||     ( tokens[j].Type == TokenInfo.TokenType.LABEL_LOCAL ) )
          &&   ( tokens[j].Content.EndsWith( "#" ) )
          &&   ( tokens[j + 1].Type == TokenInfo.TokenType.SEPARATOR )
          &&   ( tokens[j + 1].Content == "#" ) 
          &&   ( paramName.Contains( tokens[j + 2].Content ) ) )
          {
            int     paramIndex = paramName.IndexOf( tokens[j + 2].Content );

            var replacementToken = ParseTokenInfo( param[paramIndex], 0, param[paramIndex].Length, TextCodeMapping );

            if ( !EvaluateTokens( lineIndex + i, replacementToken, 0, 1, TextCodeMapping, out SymbolInfo resultingToken ) )
            {
              // there was an error!
              LineIndexInsideMacro = i;
              return null;
            }

            Debug.Log( "RelabelLocalLabelsForMacro with " + resultingToken.ToString() );

            tokens[j].Content = tokens[j].Content.Substring( 0, tokens[j].Content.Length - 1 ) + resultingToken.ToString();

            tokens.RemoveRange( j + 1, 2 );
            j = 0;

            replacedParam = true;
            continue;
          }
        }

        var originatingTokens = new List<Types.TokenInfo>();
        for ( int j = 0; j < tokens.Count; ++j )
        {
          var clonedToken = new Types.TokenInfo() { Content = tokens[j].Content, Length = tokens[j].Length, OriginatingString = tokens[j].OriginatingString, StartPos = tokens[j].StartPos, Type = tokens[j].Type };
          originatingTokens.Add( clonedToken );
        }

        // preserve indenting (PDS requires this to differ between labels and macros)
        if ( ( tokens.Count > 0 )
        &&   ( m_AssemblerSettings.LabelsMustBeAtStartOfLine )
        &&   ( tokens[0].StartPos > 0 ) )
        {
          tokens.Insert( 0, new TokenInfo() { StartPos = 0, Content = new string( ' ', tokens[0].StartPos ), Length = tokens[0].StartPos } );
        }

        if ( tokens.Count > 1 )
        {
          if ( ( tokens[0].Type == TokenInfo.TokenType.OPERATOR )
          &&   ( ( tokens[0].Content.StartsWith( "+" ) )
          ||     ( tokens[0].Content.StartsWith( "-" ) ) ) )
          {
            if ( ( tokens.Count > 1 )
            &&   ( tokens[1].Type == TokenInfo.TokenType.LABEL_GLOBAL )
            &&   ( tokens[1].StartPos == tokens[0].StartPos + tokens[0].Length )
            &&   ( tokens[1].Content.StartsWith( InternalLabelPrefix ) ) )
            {
              tokens[0].Type = TokenInfo.TokenType.LABEL_INTERNAL;
            }
            else if ( ( tokens.Count > 1 )
            &&        ( tokens[1].StartPos > tokens[0].StartPos + tokens[0].Length ) )
            {
              // not directly connected to anything
              tokens[0].Type = TokenInfo.TokenType.LABEL_INTERNAL;
            }
            else if ( tokens.Count == 1 )
            {
              tokens[0].Type = TokenInfo.TokenType.LABEL_INTERNAL;
            }
          }
        }
        bool modifiedToken = false;

        List<Types.TokenInfo>  replacingTokens = new List<Types.TokenInfo>();
        for ( int tokenIndex = 0; tokenIndex < tokens.Count; ++tokenIndex )
        {
          var token = tokens[tokenIndex];
          modifiedToken = false;

          // may look useless, but stores actual content in token
          token.Content = token.Content;

          if ( token.Type == TokenInfo.TokenType.MACRO_PARAMETER )
          {
            int     paramIndex = GR.Convert.ToI32( token.Content.Substring( 1, token.Content.Length - 2 ) );

            if ( ( paramIndex >= 1 )
            &&   ( paramIndex <= param.Count ) )
            {
              // replace parameter
              modifiedToken = true;

              int     oldLength = token.Content.Length;
              token.Content = param[paramIndex - 1];

              if ( ( tokenIndex == 0 )
              &&   ( token.Content.StartsWith( AssemblerSettings.INTERNAL_OPENING_BRACE ) )
              &&   ( token.Content.EndsWith( AssemblerSettings.INTERNAL_CLOSING_BRACE ) ) )
              {
                token.Content = token.Content.Substring( 1, token.Content.Length - 2 );
              }

              int     newLength = token.Content.Length;
              token.Length = newLength;

              // shift offsets
              for ( int j = tokenIndex + 1; j < tokens.Count; ++j )
              {
                tokens[j].Content = tokens[j].Content;
                tokens[j].Length = tokens[j].Content.Length;
                tokens[j].StartPos += newLength - oldLength;
              }
              replacingTokens.Add( token );
              replacedParam = true;
            }
          }

          if ( ( m_AssemblerSettings.MacrosUseCheapLabelsAsParameters )
          &&   ( token.Type == TokenInfo.TokenType.LABEL_CHEAP_LOCAL ) )
          {
            if ( token.Content.Length > 1 )
            {
              int     paramIndex = GR.Convert.ToI32( token.Content.Substring( 1 ) );
              if ( ( paramIndex >= 1 )
              &&   ( paramIndex <= param.Count ) )
              {
                // replace parameter
                modifiedToken = true;

                int     oldLength = token.Content.Length;
                token.Content = param[paramIndex - 1];

                if ( ( tokenIndex == 0 )
                &&   ( token.Content.StartsWith( AssemblerSettings.INTERNAL_OPENING_BRACE ) )
                &&   ( token.Content.EndsWith( AssemblerSettings.INTERNAL_CLOSING_BRACE ) ) )
                {
                  token.Content = token.Content.Substring( 1, token.Content.Length - 2 );
                }

                int     newLength = token.Content.Length;
                token.Length = newLength;

                // shift offsets
                for ( int j = tokenIndex + 1; j < tokens.Count; ++j )
                {
                  tokens[j].Content = tokens[j].Content;
                  tokens[j].Length = tokens[j].Content.Length;
                  tokens[j].StartPos += newLength - oldLength;
                }
                replacingTokens.Add( token );
                replacedParam = true;
              }
            }
          }
          if ( ( token.Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
          ||   ( token.Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_LOCAL )
          ||   ( token.Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_INTERNAL ) )
          {
            List<Types.TokenInfo>   tempTokens = new List<RetroDevStudio.Types.TokenInfo>();

            for ( int j = 0; j < functionInfo.ParameterNames.Count; ++j )
            {
              if ( ( token.Content == functionInfo.ParameterNames[j] )
              &&   ( !functionInfo.ParametersAreReferences[j] ) )
              {
                // replace parameter!
                modifiedToken = true;
                token.Content = param[j];
                token.Length = param[j].Length;

                tempTokens = ParseTokenInfo( token.Content, 0, token.Content.Length, TextCodeMapping );
                for ( int k = 0; k < tempTokens.Count; ++k )
                {
                  // may look useless, but actually fetches the substring and stores it in the content cache
                  tempTokens[k].Content = tempTokens[k].Content;
                  tempTokens[k].Length = tempTokens[k].Content.Length;
                }
                if ( ( !HasError() )
                &&   ( tempTokens.Count >= 1 ) )
                {
                  if ( ( tempTokens[0].Type != RetroDevStudio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
                  &&   ( tempTokens[0].Type != RetroDevStudio.Types.TokenInfo.TokenType.LABEL_LOCAL ) )
                  {
                  }
                  else
                  {
                    // keep offsets intact!
                    StringBuilder   sb = new StringBuilder();

                    int     curOffset = 0;
                    for ( int k = 0; k < originatingTokens.Count; ++k )
                    {
                      while ( originatingTokens[k].StartPos > sb.Length )
                      {
                        sb.Append( ' ' );
                        ++curOffset;
                      }
                      sb.Append( originatingTokens[k].Content );
                      originatingTokens[k].StartPos = curOffset;
                      curOffset = sb.Length;
                    }
                    string    newLine = sb.ToString();
                    for ( int k = 0; k < tokens.Count; ++k )
                    {
                      tokens[k].OriginatingString = newLine;
                    }
                  }
                }
                replacingTokens.Add( token );
                replacedParam = true;
                break;
              }
            }
            // Dasm - Macro local labels start with ., add macro and LINE and loop specific prefix
            if ( ( token.Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_LOCAL )
            ||   ( token.Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_INTERNAL ) )
            {
              if ( !modifiedToken )
              {
                modifiedToken = true;
                //tokens.RemoveAt( tokenIndex );
                //tokens.InsertRange( tokenIndex, tempTokens );
                if ( token.Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_INTERNAL )
                {
                  if ( token.Content[0] == '-' )
                  {
                    // TODO - take i in account, 
                    token.Content += InternalLabelPrefix + functionName + "_" + functionInfo.LineIndex.ToString() + "_" + lineIndex.ToString() + "_" + GetLoopGUID( Scopes );
                    //token.Content = token.Content.Replace( "+", "_plus_" );
                    //token.Content = token.Content.Replace( "-", "_minus_" );
                    //Debug.Log( "Replaced internal label in line " + i + " with " + token.Content );
                  }
                }
                else if ( !ScopeInsideLoop( Scopes ) )
                {
                  // uniquefy labels
                  token.Content = "_C64STUDIOINTERNAL_" + functionName + "_" + functionInfo.LineIndex.ToString() + "_" + lineIndex.ToString() + "_" + token.Content;
                }
                else
                {
                  // need to take loop into account, force new local label!
                  token.Content = m_AssemblerSettings.AllowedTokenStartChars[RetroDevStudio.Types.TokenInfo.TokenType.LABEL_LOCAL]
                                + "_C64STUDIOINTERNAL_" + functionName + "_" + functionInfo.LineIndex.ToString() + "_" + lineIndex.ToString() + "_" + GetLoopGUID( Scopes ) + "_" + token.Content;
                }
                replacingTokens.Add( token );
              }
              replacedParam = true;
            }
          }
          //++tokenIndex;
          if ( !modifiedToken )
          {
            replacingTokens.Add( token );
          }
        }
        if ( replacedParam )
        {
          replacementLines[replacementLineIndex] = TokensToExpression( tokens );
        }
        else
        {
          replacementLines[replacementLineIndex] = Lines[i];
        }
        ++replacementLineIndex;
      }
      return replacementLines;
    }



    private string[] RelabelLocalLabelsForLoop( string[] Lines, List<Types.ScopeInfo> Scopes, int lineIndex, GR.Collections.Map<byte, byte> TextCodeMapping )
    {
      string[] replacementLines = new string[Lines.Length];
      int replacementLineIndex = 0;

      for ( int i = 0; i < Lines.Length; ++i )
      {
        List<Types.TokenInfo> tokens = ParseTokenInfo( Lines[i], 0, Lines[i].Length, TextCodeMapping );
        bool replacedParam = false;

        int   tokenIndex = 0;
        foreach ( Types.TokenInfo token in tokens )
        {
          if ( ( token.Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
          ||   ( token.Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_LOCAL )
          ||   ( token.Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_INTERNAL ) )
          {
            // Dasm - Macro local labels start with ., add macro and LINE and loop specific prefix
            if ( token.Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_LOCAL )
            {
              // need to take loop into account, force new local label!
              token.Content = m_AssemblerSettings.AllowedTokenStartChars[RetroDevStudio.Types.TokenInfo.TokenType.LABEL_LOCAL]
                            + AssemblerSettings.INTERNAL_LOCAL_LOOP_LABEL_PREFIX
                            + GetLoopGUID( Scopes ) + "_" + lineIndex.ToString() + "_" + token.Content;
              replacedParam = true;
            }
            else if ( token.Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_INTERNAL )
            {
              // need to take loop into account, force new local label!
              replacedParam = true;
            }
          }
          ++tokenIndex;
        }
        if ( replacedParam )
        {
          replacementLines[replacementLineIndex] = TokensToExpression( tokens );
        }
        else
        {
          replacementLines[replacementLineIndex] = Lines[i];
        }
        ++replacementLineIndex;
      }
      return replacementLines;
    }



    private bool POMacro( string LabelInFront, string Zone, GR.Collections.Map<GR.Generic.Tupel<string,int>, Types.MacroFunctionInfo> macroFunctions, 
                          string OuterFilename,
                          int lineIndex, 
                          List<Types.TokenInfo> lineTokenInfos, 
                          List<Types.ScopeInfo> Scopes, 
                          out string MacroFunctionName )
    {
      // !macro Macroname [param1[,param2]]
      bool  hadError = false;
      bool  hasBracket = false;

      MacroFunctionName = "";
      if ( lineTokenInfos[lineTokenInfos.Count - 1].Content == "{" )
      {
        hasBracket = true;
        lineTokenInfos.RemoveAt( lineTokenInfos.Count - 1 );
      }

      if ( m_AssemblerSettings.MacroKeywordAfterName )
      {
        // PDS style (<macroname> MACRO)
        if ( lineTokenInfos.Count != 1 )
        {
          AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Malformed macro, expect <Macroname> MACRO" );
          return false;
        }
        if ( macroFunctions.Keys.Any( m => m.first == LabelInFront ) )
        {
          AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1200_REDEFINITION_OF_LABEL, $"Macro name {LabelInFront} is already in use" );
          return false;
        }
        Types.MacroFunctionInfo macroFunction = new RetroDevStudio.Types.MacroFunctionInfo();

        macroFunction.Name            = LabelInFront;
        macroFunction.LineIndex       = lineIndex;
        macroFunction.ParentFileName  = OuterFilename;
        macroFunction.UsesBracket     = hasBracket;

        macroFunction.Symbol                  = new SymbolInfo();
        macroFunction.Symbol.Name             = LabelInFront;
        macroFunction.Symbol.LocalLineIndex   = lineIndex;
        macroFunction.Symbol.Type             = SymbolInfo.Types.MACRO;
        macroFunction.Symbol.DocumentFilename = OuterFilename;
        macroFunction.Symbol.Zone             = Zone;
        macroFunction.Symbol.References.Add( lineIndex );
        macroFunction.Symbol.NumArguments     = -1;


        macroFunctions.Add( new GR.Generic.Tupel<string, int>( LabelInFront, -1 ), macroFunction );

        MacroFunctionName = LabelInFront;

        // macro scope
        Types.ScopeInfo scope = new RetroDevStudio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.MACRO_FUNCTION );

        scope.StartIndex  = lineIndex;
        scope.Macro       = macroFunction;
        scope.Active      = true;
        Scopes.Add( scope );
        return true;
      }

      if ( ( m_AssemblerSettings.MacrosHaveVariableNumberOfArguments )
      &&   ( lineTokenInfos.Count != 2 ) )
      {
        AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Malformed macro, expect !MACRO <Macroname>" );
        hadError = true;
      }
      else if ( lineTokenInfos.Count < 2 )
      {
        AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Malformed macro, expect !MACRO <Macroname> [<Param1>[,<Param2>[...]]]" );
        hadError = true;
      }
      else if ( lineTokenInfos[1].Type != RetroDevStudio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
      {
        AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Macro name must be formatted like a global label" );
        hadError = true;
      }
      else
      {
        string macroName = lineTokenInfos[1].Content;
        
        if ( ( !m_AssemblerSettings.MacrosCanBeOverloaded )
        &&   ( macroFunctions.Keys.Any( m => m.first == macroName ) ) )
        {
          AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1200_REDEFINITION_OF_LABEL, $"Macro name {macroName} is already in use" );
          hadError = true;
        }
        else
        {
          List<string>  param = new List<string>();
          List<bool>    paramIsRef = new List<bool>();

          bool shouldParameterBeAComma = false;

          for ( int i = 2; i < lineTokenInfos.Count; ++i )
          {
            if ( shouldParameterBeAComma )
            {
              if ( lineTokenInfos[i].Content != "," )
              {
                AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Parameter names must be separated by comma" );
                hadError = true;
              }
            }
            else if ( ( !shouldParameterBeAComma )
            &&        ( ( lineTokenInfos[i].Type != RetroDevStudio.Types.TokenInfo.TokenType.OPERATOR )
            ||          ( ( lineTokenInfos[i].Type == RetroDevStudio.Types.TokenInfo.TokenType.OPERATOR )
            &&            ( lineTokenInfos[i].Content != "~" ) ) )
            &&        ( lineTokenInfos[i].Type != RetroDevStudio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
            &&        ( lineTokenInfos[i].Type != RetroDevStudio.Types.TokenInfo.TokenType.LABEL_LOCAL ) )
            {
              AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Parameter name must be formatted like a global label" );
              hadError = true;
            }
            else if ( ( !shouldParameterBeAComma )
            &&        ( lineTokenInfos[i].Type == RetroDevStudio.Types.TokenInfo.TokenType.OPERATOR )
            &&        ( lineTokenInfos[i].Content == "~" ) )
            {
              if ( ( i + 1 >= lineTokenInfos.Count )
              ||   ( ( lineTokenInfos[i + 1].Type != RetroDevStudio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
              &&     ( lineTokenInfos[i + 1].Type != RetroDevStudio.Types.TokenInfo.TokenType.LABEL_LOCAL ) ) )
              {
                AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Syntax error, expected parameter name after ~" );
                hadError = true;
              }
              else
              {
                // ACME speciality, parameter is a reference (means it changes the outside value)
                param.Add( lineTokenInfos[i + 1].Content );
                paramIsRef.Add( true );
                ++i;
                shouldParameterBeAComma = true;
                continue;
              }
            }
            else
            {
              param.Add( lineTokenInfos[i].Content );
              paramIsRef.Add( false );
            }
            shouldParameterBeAComma = !shouldParameterBeAComma;
          }
          if ( !hadError )
          {
            Types.MacroFunctionInfo macroFunction = new RetroDevStudio.Types.MacroFunctionInfo();

            macroFunction.Name                    = macroName;
            macroFunction.LineIndex               = lineIndex;
            macroFunction.ParentFileName          = OuterFilename;
            macroFunction.ParameterNames          = param;
            macroFunction.ParametersAreReferences = paramIsRef;
            macroFunction.UsesBracket             = hasBracket;

            macroFunction.Symbol                  = new SymbolInfo();
            macroFunction.Symbol.Name             = macroName;
            macroFunction.Symbol.LocalLineIndex   = lineIndex;
            macroFunction.Symbol.Type             = SymbolInfo.Types.MACRO;
            macroFunction.Symbol.DocumentFilename = OuterFilename;
            macroFunction.Symbol.Zone             = Zone;
            macroFunction.Symbol.NumArguments     = param.Count;
            macroFunction.Symbol.References.Add( lineIndex );

            macroFunctions.Add( new GR.Generic.Tupel<string, int>( macroName, param.Count ), macroFunction );

            MacroFunctionName = macroName;

            // macro scope
            Types.ScopeInfo scope = new RetroDevStudio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.MACRO_FUNCTION );

            scope.StartIndex  = lineIndex;
            scope.Macro       = macroFunction;
            scope.Active      = true;

            OnScopeAdded( scope );
            Scopes.Add( scope );

            //Debug.Log( "add macro scope for " + macroName + " in line " + lineIndex );
          }
        }
      }
      return !hadError;
    }



    private void AddVirtualBreakpoint( int LineIndex, string DocumentFilename, string Expression )
    {
      Types.Breakpoint    virtualBP = new RetroDevStudio.Types.Breakpoint();
      virtualBP.LineIndex = LineIndex;
      virtualBP.Expression  = Expression;
      virtualBP.DocumentFilename = DocumentFilename;
      m_ASMFileInfo.VirtualBreakpoints.Add( LineIndex, virtualBP );
    }



    private void CloneSourceInfos( int SourceIndex, int CopyLength, int TargetIndex )
    {
      List<Types.ASM.SourceInfo>    infosToAdd = new List<RetroDevStudio.Types.ASM.SourceInfo>();

      foreach ( Types.ASM.SourceInfo oldInfo in m_ASMFileInfo.SourceInfo.Values )
      {
        if ( ( oldInfo.LineCount != -1 )
        &&   ( oldInfo.GlobalStartLine >= SourceIndex )
        &&   ( oldInfo.GlobalStartLine + oldInfo.LineCount <= SourceIndex + CopyLength ) )
        {
          // fully inside source scope
          // need to copy!
          Types.ASM.SourceInfo tempInfo = new RetroDevStudio.Types.ASM.SourceInfo();

          tempInfo.LineCount        = oldInfo.LineCount;
          tempInfo.LocalStartLine   = oldInfo.LocalStartLine;
          tempInfo.Filename         = oldInfo.Filename;
          tempInfo.FilenameParent   = oldInfo.FilenameParent;
          tempInfo.GlobalStartLine  = oldInfo.GlobalStartLine + TargetIndex - SourceIndex;

          infosToAdd.Add( tempInfo );
        }
      }

      foreach ( var infoToAdd in infosToAdd )
      {
        //Debug.Log( "Adding cloned source info at " + infoToAdd.GlobalStartLine + " to " + ( infoToAdd.GlobalStartLine + infoToAdd.LineCount - 1 ) + " from orig " + ( 1 + infoToAdd.GlobalStartLine - ( TargetIndex - SourceIndex ) ) );
        InsertSourceInfo( infoToAdd, false, false );
      }
    }



    private void InsertSourceInfo( Types.ASM.SourceInfo sourceInfo )
    {
      InsertSourceInfo( sourceInfo, true, false );
    }



    /// <summary>
    /// Inserts SourceInfo in other Infos, removes the line of an existing overlapping info at the location of the new info!!
    /// </summary>
    /// <param name="sourceInfo"></param>
    /// <param name="AllowShifting"></param>
    /// 

    private void InsertSourceInfo( Types.ASM.SourceInfo sourceInfo, bool AllowShifting, bool OverwriteFirstLineOfOverlapping )
    {
      int lineCount = sourceInfo.LineCount;
      /*
      if ( OverwriteFirstLineOfOverlapping )
      {
        --lineCount;
      }*/

      // move zones
      foreach ( var zoneList in m_ASMFileInfo.Zones.Values )
      {
        foreach ( var zoneInfo in zoneList )
        {
          if ( zoneInfo.LineIndex >= sourceInfo.GlobalStartLine + lineCount )
          {
            zoneInfo.LineIndex += lineCount;
            continue;
          }
          else if ( zoneInfo.LineIndex < sourceInfo.GlobalStartLine )
          {
            continue;
          }
          // inside (simply grow) -> not split?
          zoneInfo.LineCount += lineCount;
        }
      }

      List<Types.ASM.SourceInfo> movedInfos = new List<Types.ASM.SourceInfo>();
      foreach ( Types.ASM.SourceInfo oldInfo in m_ASMFileInfo.SourceInfo.Values )
      {
        if ( !AllowShifting )
        {
          // only split
          if ( ( sourceInfo.GlobalStartLine > oldInfo.GlobalStartLine )
          &&   ( sourceInfo.GlobalStartLine + sourceInfo.LineCount <= oldInfo.GlobalStartLine + oldInfo.LineCount ) )
          {
            // inside split
            Types.ASM.SourceInfo secondHalf = new Types.ASM.SourceInfo();
            secondHalf.Filename         = oldInfo.Filename;
            secondHalf.FilenameParent   = oldInfo.FilenameParent;
            secondHalf.FullPath         = oldInfo.FullPath;
            secondHalf.GlobalStartLine  = sourceInfo.GlobalStartLine + sourceInfo.LineCount;
            secondHalf.LineCount        = ( oldInfo.GlobalStartLine + oldInfo.LineCount ) - ( sourceInfo.GlobalStartLine + sourceInfo.LineCount );

            oldInfo.LineCount = sourceInfo.GlobalStartLine - oldInfo.GlobalStartLine;

            secondHalf.LocalStartLine = oldInfo.LocalStartLine + oldInfo.LineCount;
            movedInfos.Add( secondHalf );
            break;
          }
          else if ( ( oldInfo.GlobalStartLine < sourceInfo.GlobalStartLine )
          &&        ( oldInfo.GlobalStartLine + oldInfo.LineCount > sourceInfo.GlobalStartLine )
          &&        ( oldInfo.GlobalStartLine + oldInfo.LineCount <= sourceInfo.GlobalStartLine + sourceInfo.LineCount ) )
          {
            // oldInfo overlaps into new sourceinfo, split second half
            oldInfo.LineCount = sourceInfo.GlobalStartLine - oldInfo.GlobalStartLine;
          }
          else if ( ( oldInfo.GlobalStartLine >= sourceInfo.GlobalStartLine )
          &&        ( oldInfo.GlobalStartLine < sourceInfo.GlobalStartLine + sourceInfo.LineCount ) )
          {
            // oldInfo starts inside new sourceinfo, split first half
            oldInfo.LineCount -= sourceInfo.GlobalStartLine + sourceInfo.LineCount - oldInfo.GlobalStartLine;
            oldInfo.GlobalStartLine = sourceInfo.GlobalStartLine + sourceInfo.LineCount;

            movedInfos.Add( oldInfo );
            break;
          }
        }
        else
        {
          if ( oldInfo.GlobalStartLine >= sourceInfo.GlobalStartLine )
          {
            // shift down completely
            /*
            if ( OverwriteFirstLineOfOverlapping )
            {
              if ( lineCount > 1 )
              {
                oldInfo.GlobalStartLine += lineCount - 1;
                movedInfos.Add( oldInfo );
              }
            }
            else*/
            {
              oldInfo.GlobalStartLine += lineCount;
              movedInfos.Add( oldInfo );
            }
          }
          else if ( oldInfo.GlobalStartLine + oldInfo.LineCount > sourceInfo.GlobalStartLine )
          {
            // only split if snippets do not end at the same line
            //if ( sourceInfo.GlobalStartLine + sourceInfo.LineCount != oldInfo.GlobalStartLine + oldInfo.LineCount )
            {
              // split!
              Types.ASM.SourceInfo secondHalf = new Types.ASM.SourceInfo();
              secondHalf.Filename = oldInfo.Filename;
              secondHalf.FilenameParent = oldInfo.FilenameParent;
              secondHalf.FullPath = oldInfo.FullPath;
              secondHalf.GlobalStartLine = sourceInfo.GlobalStartLine + sourceInfo.LineCount;
              secondHalf.LineCount = oldInfo.LineCount - ( sourceInfo.GlobalStartLine - oldInfo.GlobalStartLine );

              oldInfo.LineCount -= secondHalf.LineCount;

              secondHalf.LocalStartLine = oldInfo.LocalStartLine + oldInfo.LineCount;

              /*
              if ( OverwriteFirstLineOfOverlapping )
              {
                // BREAKING CHANGE !!! -> second half gets first line removed!!
                ++secondHalf.LocalStartLine;
                --secondHalf.LineCount;
              }*/
              movedInfos.Add( secondHalf );
            }
            /*
            else
            {
              oldInfo.LineCount -= sourceInfo.LineCount;
            }*/
          }
        }
      }
      foreach ( Types.ASM.SourceInfo oldInfo in movedInfos )
      {
        foreach ( int key in m_ASMFileInfo.SourceInfo.Keys )
        {
          if ( m_ASMFileInfo.SourceInfo[key] == oldInfo )
          {
            m_ASMFileInfo.SourceInfo.Remove( key );
            break;
          }
        }
      }

      bool    dumpInfos = false;

      if ( m_ASMFileInfo.SourceInfo.ContainsKey( sourceInfo.GlobalStartLine ) )
      {
        Debug.Log( "Source Info already exists at global line index " + sourceInfo.GlobalStartLine );
        return;
      }

      m_ASMFileInfo.SourceInfo.Add( sourceInfo.GlobalStartLine, sourceInfo );
      foreach ( Types.ASM.SourceInfo oldInfo in movedInfos )
      {
        if ( oldInfo.LineCount != 0 )
        {
          if ( m_ASMFileInfo.SourceInfo.ContainsKey( oldInfo.GlobalStartLine ) )
          {
            Debug.Log( "Trying to insert duplicate source info at global line index " + oldInfo.GlobalStartLine );
            dumpInfos = true;
          }
          else
          {
            m_ASMFileInfo.SourceInfo.Add( oldInfo.GlobalStartLine, oldInfo );
          }
        }
      }

      if ( dumpInfos )
      {
        // dump source infos
        int fullLines = 0;
        foreach ( var pair in m_ASMFileInfo.SourceInfo )
        {
          var info = pair.Value;
          //Debug.Log( "Key " + pair.Key + ": Source from " + info.GlobalStartLine + ", " + info.LineCount + " lines, from file " + info.Filename + " at offset " + info.LocalStartLine );
          Debug.Log( "From " + info.GlobalStartLine + " to " + ( info.GlobalStartLine + info.LineCount - 1 ) + ", " + info.LineCount + " lines, from file " + System.IO.Path.GetFileNameWithoutExtension( info.Filename ) + " at offset " + info.LocalStartLine );
          fullLines += info.LineCount;
        }
        Debug.Log( "Total " + fullLines + " lines" );
      }
    }



    public override void Clear()
    {
      m_CompileTarget         = Types.CompileTargetType.NONE;
      m_CompileTargetFile     = null;
      m_CompileCurrentAddress = -1;
      ExternallyIncludedFiles.Clear();

      AssembledOutput = null;
      m_ASMFileInfo.Messages.Clear();
      m_ErrorMessages = 0;
      m_WarningMessages = 0;
      m_ASMFileInfo = new RetroDevStudio.Types.ASM.FileInfo();
      m_LoadedFiles.Clear();
      m_Filename = "";
    }



    public void ParseAndAddPreDefines( string PreDefines, GR.Collections.Map<byte, byte> TextCodeMapping )
    {
      string[]    makros = PreDefines.Split( '\n' );

      foreach ( string makro in makros )
      {
        string singleMakro = makro.Trim();

        List<Types.TokenInfo> lineTokens = ParseTokenInfo( singleMakro, 0, singleMakro.Length, TextCodeMapping );

        if ( lineTokens.Count == 1 )
        {
          AddPreprocessorLabel( lineTokens[0].Content, 1, -1 );
        }
        else if ( ( lineTokens.Count >= 3 )
        &&        ( m_AssemblerSettings.DefineSeparatorKeywords.ContainsValue( lineTokens[1].Content ) ) )
        {
          // a define
          int equPos = lineTokens[1].StartPos;
          string defineName = singleMakro.Substring( 0, equPos ).Trim();
          string defineValue = singleMakro.Substring( equPos + 1 ).Trim();
          List<Types.TokenInfo>  valueTokens = ParseTokenInfo( defineValue, 0, defineValue.Length, TextCodeMapping );
          if ( !EvaluateTokens( -1, valueTokens, TextCodeMapping, out SymbolInfo address ) )
          {
            AddError( -1, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Cannot evaluate predefine expression " + defineValue );
          }
          else
          {
            // TODO - allow real numbers
            AddPreprocessorConstant( defineName, address.ToInteger(), -1 );
          }
        }
      }
    }



    private void DumpTempLabelInfos()
    {
      foreach ( var entry in m_ASMFileInfo.TempLabelInfo )
      {
        if ( entry.LineCount == -1 )
        {
          Debug.Log( "From line " + ( entry.LineIndex + 1 ) + " to the end, name " + entry.Name + ", value " + entry.Symbol.ToString() );
        }
        else
        {
          Debug.Log( "From line " + ( entry.LineIndex + 1 ) + " to " + ( entry.LineIndex + 1 + entry.LineCount - 1 ) + ", name " + entry.Name + ", value " + entry.Symbol.ToString() );
        }
      }
    }



    private void DumpTempLabelInfos( string Name )
    {
      foreach ( var entry in m_ASMFileInfo.TempLabelInfo )
      {
        if ( entry.Name == Name )
        {
          if ( entry.LineCount == -1 )
          {
            Debug.Log( "From line " + ( entry.LineIndex + 1 ) + " to the end, name " + entry.Name + ", value " + entry.Symbol.ToString() );
          }
          else
          {
            Debug.Log( "From line " + ( entry.LineIndex + 1 ) + " to " + ( entry.LineIndex + 1 + entry.LineCount - 1 ) + ", name " + entry.Name + ", value " + entry.Symbol.ToString() );
          }
        }
      }
    }



    private void DumpSourceInfos( Dictionary<string,string[]> OrigLines, string[] Lines )
    {
      if ( !DoLogSourceInfo )
      {
        return;
      }
      // source infos
      StringBuilder   sb = new StringBuilder();


      Debug.Log( "=======> Step " + dumpCount );
      foreach ( KeyValuePair<int,Types.ASM.SourceInfo> pair in m_ASMFileInfo.SourceInfo )
      {
        Debug.Log( "From line " + ( pair.Value.GlobalStartLine + 1 ) + " to " + ( pair.Value.GlobalStartLine + pair.Value.LineCount - 1 + 1 ) + ", local " + ( pair.Value.LocalStartLine + 1 ) + ", " + pair.Value.LineCount + " lines from " + pair.Value.Filename );
        for ( int i = 0; i < pair.Value.LineCount; ++i )
        {
          if ( pair.Value.LocalStartLine + i >= OrigLines[pair.Value.Filename].Length )
          {
            Debug.Log( "DumpSourceInfos: Out of bounds! " + ( pair.Value.LocalStartLine + i ).ToString() + " >= " + OrigLines[pair.Value.Filename].Length );
            sb.AppendLine( "OUT OF BOUNDS!" );
            break;
          }
          else
          {
            sb.AppendLine( OrigLines[pair.Value.Filename][pair.Value.LocalStartLine + i] );
          }
        }
        sb.AppendLine( "======================" );
      }
      string    outFile = "sourceinfo" + dumpCount + ".txt";
      ++dumpCount;

      System.IO.File.WriteAllText( outFile, sb.ToString() );
    }



    public override bool Parse( string Content, ProjectConfig Configuration, CompileConfig Config, string AdditionalPredefines, out Types.ASM.FileInfo ASMFileInfo  )
    {
      ASMFileInfo = null;

      m_CompileConfig = Config;
      // clone list - don't modify original!
      m_CompileConfig.LibraryFiles = new List<string>( Config.LibraryFiles );

      // default -> TODO override via commandline
      m_Processor = Processor.Create6510();
      m_AssemblerSettings.SetAssemblerType( Config.Assembler );
      m_AssemblerSettings.EnabledHacks = Config.EnabledHacks;

      m_Assume16BitAccu       = false;
      m_Assume16BitRegisters  = false;
      _Random                 = null;

      string[] lines = Content.Replace( "\r\n", "\n" ).Replace( '\r', '\n' ).Replace( '\t', ' ' ).Split( '\n' );

      CleanLines( lines );

      var sourceInfo = new Types.ASM.SourceInfo();
      sourceInfo.Filename         = m_Filename;
      sourceInfo.GlobalStartLine  = 0;
      sourceInfo.LineCount        = lines.Length;
      sourceInfo.FullPath         = m_Filename;

      m_ASMFileInfo = new RetroDevStudio.Types.ASM.FileInfo();
      m_ASMFileInfo.SourceInfo.Add( sourceInfo.GlobalStartLine, sourceInfo );
      m_ASMFileInfo.AssemblerSettings = m_AssemblerSettings;
      m_ASMFileInfo.LabelDumpFile     = Config.LabelDumpFile;

      m_WarningsToIgnore.Clear();

      bool    hadFatalError = false;
      lines = PreProcess( lines, m_Filename, Configuration, AdditionalPredefines, out hadFatalError );

      DumpSourceInfos( OrigLines, lines );

      if ( !hadFatalError )
      {
        DetermineUnparsedLabels();
        m_ASMFileInfo.PopulateAddressToLine();
        foreach ( SymbolInfo token in m_ASMFileInfo.Labels.Values )
        {
          if ( ( token.References.Count == 0 )
          &&   ( token.Name != "*" )
          &&   ( !token.Name.Contains( AssemblerSettings.INTERNAL_LOCAL_LOOP_LABEL_PREFIX ) )
          &&   ( token.Type != SymbolInfo.Types.PREPROCESSOR_LABEL ) )
          {
            AddWarning( token.LineIndex, 
                        Types.ErrorCode.W1000_UNUSED_LABEL, 
                        "Unused label " + token.Name,
                        token.CharIndex,
                        token.Length );
          }
        }
      }

      // create preprocessed file even with errors (might be the reason to get the preprocessed file in the first place)
      if ( Config.CreatePreProcesseFile )
      {
        CreatePreProcessedFile( Config.InputFile, lines, m_ASMFileInfo );
      }
      if ( Config.CreateRelocationFile )
      {
        CreateRelocationFile( Config.InputFile, lines, m_ASMFileInfo );
      }

      ASMFileInfo = m_ASMFileInfo;
      if ( ( m_ASMFileInfo.UnparsedLabels.Count > 0 )
      ||   ( m_ErrorMessages > 0 ) )
      {
        return false;
      }

      return true;
    }



    private void CreatePreProcessedFile( string SourceFile, string[] Lines, Types.ASM.FileInfo FileInfo )
    {
      try
      {
        string pathLog = System.IO.Path.Combine( System.IO.Path.GetDirectoryName( SourceFile ), System.IO.Path.GetFileNameWithoutExtension( SourceFile ) + ".dump" );

        if ( Lines == null )
        {
          return;
        }

        using ( StreamWriter writer = File.CreateText( pathLog ) )
        {
          int     numLineDigits = Lines.Length.ToString().Length;

          string  formatString = "D" + numLineDigits.ToString();

          for ( int i = 0; i < Lines.Length; ++i )
          {
            if ( FileInfo.LineInfo.ContainsKey( i ) )
            {
              var     info = FileInfo.LineInfo[i];
              if ( info != null )
              {
                if ( info.HideInPreprocessedOutput )
                {
                  continue;
                }

                writer.Write( i.ToString( formatString ) );
                writer.Write( "  " );
                if ( info.AddressStart < 0 )
                {
                  writer.Write( " ----" );
                }
                else
                {
                  writer.Write( "$" + info.AddressStart.ToString( "X4" ) );
                }

                writer.Write( "  " );
                if ( info.LineData != null )
                {
                  int     numBytesToPrint = (int)info.LineData.Length;
                  int     numLettersPrinted = 0;

                  for ( int j = 0; j < numBytesToPrint; ++j )
                  {
                    writer.Write( info.LineData.ByteAt( j ).ToString( "X2" ) );
                    numLettersPrinted += 2;
                    if ( j + 1 < numBytesToPrint )
                    {
                      writer.Write( ' ' );
                      ++numLettersPrinted;
                    }
                  }
                  for ( int j = numLettersPrinted; j < 10; ++j )
                  {
                    writer.Write( ' ' );
                  }
                  //writer.Write( info.LineData.ToString() );
                }
                else
                {
                  writer.Write( "          " );
                }
                writer.Write( "  " );
                writer.WriteLine( Lines[i].TrimStart() );
              }
              else
              {
                writer.Write( i.ToString( formatString ) );
                writer.Write( "  " );

                writer.Write( "????              " );
                writer.WriteLine( Lines[i].TrimStart() );
              }
            }
            else
            {
              writer.Write( i.ToString( formatString ) );
              writer.Write( "  " );

              writer.Write( "????              " );
              writer.WriteLine( Lines[i].TrimStart() );
            }
          }
          writer.Close();
        }

        ParseMessage message = new ParseMessage( ParseMessage.LineType.MESSAGE, Types.ErrorCode.OK, "Preprocessed file written to " + pathLog );
        message.AlternativeFile = pathLog;
        message.AlternativeLineIndex = 0;
        m_ASMFileInfo.Messages.Add( -1, message );
        ++m_Messages;
      }
      catch ( Exception ex )
      {
        AddWarning( -1, Types.ErrorCode.E1401_INTERNAL_ERROR, "Can't write preprocessed file:" + ex.Message, 0, 0 );
      }
    }



    private void CreateRelocationFile( string SourceFile, string[] Lines, Types.ASM.FileInfo FileInfo )
    {
      try
      {
        string pathLog = System.IO.Path.Combine( System.IO.Path.GetDirectoryName( SourceFile ), System.IO.Path.GetFileNameWithoutExtension( SourceFile ) + ".loc" );

        if ( Lines == null )
        {
          return;
        }

        using ( StreamWriter writer = File.CreateText( pathLog ) )
        {
          foreach ( var info in FileInfo.Labels )
          {
            foreach ( var reference in info.Value.References )
            {
              if ( FileInfo.LineInfo.ContainsKey( reference ) )
              {
                var line = FileInfo.LineInfo[reference];

                // TODO - check if we're inside the code
                // is absolute opcode?
                if ( line.Opcode != null )
                {
                  if ( ( line.Opcode.Addressing == Opcode.AddressingType.ABSOLUTE )
                  ||   ( line.Opcode.Addressing == Opcode.AddressingType.ABSOLUTE_INDIRECT_X )
                  ||   ( line.Opcode.Addressing == Opcode.AddressingType.ABSOLUTE_X )
                  ||   ( line.Opcode.Addressing == Opcode.AddressingType.ABSOLUTE_Y ) )
                  {
                    writer.WriteLine( "Label " + info.Key );
                    writer.WriteLine( "  used in line " + line.Line );
                  }
                }
              }
              else
              {
                Debug.Log( "Reference found in unknown line " + reference );
              }
            }
          }
        }
        ParseMessage message = new ParseMessage( ParseMessage.LineType.MESSAGE, Types.ErrorCode.OK, "Relocation file written to " + pathLog );
        message.AlternativeFile = pathLog;
        message.AlternativeLineIndex = 0;
        m_ASMFileInfo.Messages.Add( -1, message );
        ++m_Messages;
      }
      catch ( Exception ex )
      {
        AddWarning( -1, Types.ErrorCode.E1401_INTERNAL_ERROR, "Can't write relocation file:" + ex.Message, 0, 0 );
      }
    }



    public static bool IsCartridge( CompileTargetType Type )
    {
      if ( ( Type == Types.CompileTargetType.CARTRIDGE_MAGICDESK_BIN_32K )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_MAGICDESK_CRT_32K )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_MAGICDESK_BIN_64K )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_MAGICDESK_CRT_64K )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_MAGICDESK_BIN_128K )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_MAGICDESK_CRT_128K )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_MAGICDESK_BIN_256K )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_MAGICDESK_CRT_256K )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_MAGICDESK_BIN_512K )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_MAGICDESK_CRT_512K )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_MAGICDESK_BIN_1M )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_MAGICDESK_CRT_1M )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_EASYFLASH_BIN )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_EASYFLASH_CRT )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_RGCD_BIN )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_RGCD_CRT )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_GMOD2_BIN )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_GMOD2_CRT )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_16K_BIN )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_16K_CRT )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_8K_BIN )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_8K_CRT )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_ULTIMAX_4K_BIN )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_ULTIMAX_4K_CRT )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_ULTIMAX_16K_BIN )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_ULTIMAX_16K_CRT )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_ULTIMAX_8K_BIN )
      ||   ( Type == Types.CompileTargetType.CARTRIDGE_ULTIMAX_8K_CRT ) )
      {
        return true;
      }
      return false;
    }



    private bool TargetTypeRequiresLoadAddress( Types.CompileTargetType Type )
    {
      if ( ( Type != Types.CompileTargetType.PLAIN )
      &&   ( !IsCartridge( Type ) ) )
      {
        return true;
      }
      return false;
    }



    public override bool Assemble( CompileConfig Config )
    {
      if ( Config.OutputFile == null )
      {
        AddError( -1, RetroDevStudio.Types.ErrorCode.E0001_NO_OUTPUT_FILENAME, "No output file name provided" );
        return false;
      }
      m_CompileTargetFile = Config.OutputFile;

      GR.Memory.ByteBuffer    result = new GR.Memory.ByteBuffer();
      GR.Memory.ByteBuffer    currentResultBlock = new GR.Memory.ByteBuffer();

      Types.MemoryMap         memoryMap = new Types.MemoryMap();

      List<GR.Generic.Tupel<int,Types.ASMSegment>>      builtSegments = new List<GR.Generic.Tupel<int, Types.ASMSegment>>();


      int     memoryBlockStartAddress = -1;
      int     memoryBlockLength = 0;
      int     memoryBlockActualDataLength = 0;


      int     currentAddress = -1;
      int     trueCurrentAddress = -1;
      bool    startBytesSet = false;
      int     fileStartAddress = -1;
      int     dataOffset = 0;

      foreach ( Types.ASM.LineInfo line in m_ASMFileInfo.LineInfo.Values )
      {
        if ( line.AddressStart != -1 )
        {
          if ( ( fileStartAddress == -1 )
          ||   ( fileStartAddress > line.AddressStart ) )
          {
            // file start must be lowest address
            fileStartAddress = line.AddressStart;
          }
        }
      }

      string  lastAddressSourceDesc = "";
      int     currentMemBlockActualSize = 0;
      foreach ( Types.ASM.LineInfo line in m_ASMFileInfo.LineInfo.Values )
      {
        if ( currentAddress == -1 )
        {
          if ( line.AddressStart != -1 )
          {
            currentAddress = line.AddressStart;
            trueCurrentAddress = currentAddress;
            if ( !startBytesSet )
            {
              startBytesSet = true;
              //fileStartAddress = currentAddress;

              if ( TargetTypeRequiresLoadAddress( Config.TargetType ) )
              {
                result.AppendU16( (ushort)currentAddress );
                dataOffset = 2;
              }
            }
            //memoryBlockStartAddress = currentAddress;
            //memoryBlockActualDataLength = 0;

            var asmSegment = new Types.ASMSegment();
            asmSegment.StartAddress = line.AddressStart;
            asmSegment.GlobalLineIndex = line.LineIndex;
            m_ASMFileInfo.FindTrueLineSource( line.LineIndex, out asmSegment.Filename, out asmSegment.LocalLineIndex );

            builtSegments.Add( new GR.Generic.Tupel<int, Types.ASMSegment>( asmSegment.StartAddress, asmSegment ) );
            currentResultBlock = asmSegment.Data;

          }
        }
        bool setNewAddress = true;
        if ( ( line.AddressStart != -1 )
        &&   ( line.AddressStart != currentAddress ) )
        {
          //Debug.Log( "ASM - new followup block starts at " + line.AddressStart );

          if ( line.PseudoPCOffset == -1 )
          {
            var asmSegment = new Types.ASMSegment();
            asmSegment.StartAddress = line.AddressStart;
            asmSegment.GlobalLineIndex = line.LineIndex;
            m_ASMFileInfo.FindTrueLineSource( line.LineIndex, out asmSegment.Filename, out asmSegment.LocalLineIndex );
            builtSegments.Add( new GR.Generic.Tupel<int, Types.ASMSegment>( asmSegment.StartAddress, asmSegment ) );
            currentResultBlock = asmSegment.Data;
          }
          //Debug.Log( "Address jump from " + currentAddress.ToString( "x" ) + " to " + line.AddressStart.ToString( "x" ) );
          //Debug.Log( "-at line " + line.Line );
          if ( line.LineData != null )
          {
            //Debug.Log( "-at line index " + line.LineData.ToString() );
            memoryBlockActualDataLength += (int)line.LineData.Length;
          }

          // need to fill a gap?
          if ( trueCurrentAddress == currentAddress )
          {
            // was proper PC before
            if ( line.PseudoPCOffset >= 0 )
            {
              // entered PSEUDOPC
            }
            else
            {
              // proper PC to proper PC with jump
              if ( line.AddressStart > currentAddress )
              {
                // need to go forward 
                int   newBytes = line.AddressStart - currentAddress;

                if ( currentAddress - fileStartAddress + newBytes > result.Length )
                {
                  /*
                  //if ( memoryBlockActualDataLength > 0 )
                  {
                    var entry = new Types.MemoryMapEntry( memoryBlockStartAddress, currentAddress - memoryBlockStartAddress );
                    entry.Description = lastAddressSourceDesc;
                    memoryMap.InsertEntry( entry );
                    lastAddressSourceDesc = "";
                  }*/

                  result.Append( new GR.Memory.ByteBuffer( (uint)( currentAddress - fileStartAddress + newBytes - result.Length ) ) );

                  //memoryBlockStartAddress = line.AddressStart;
                  //memoryBlockLength = 0;
                  //memoryBlockActualDataLength = 0;
                }
                // went forward in memory
                var asmSegment = new Types.ASMSegment();
                asmSegment.StartAddress     = line.AddressStart;
                asmSegment.GlobalLineIndex  = line.LineIndex;
                m_ASMFileInfo.FindTrueLineSource( line.LineIndex, out asmSegment.Filename, out asmSegment.LocalLineIndex );
                builtSegments.Add( new GR.Generic.Tupel<int, Types.ASMSegment>( asmSegment.StartAddress, asmSegment ) );
                currentResultBlock = asmSegment.Data;
              }
              else if ( line.AddressStart < currentAddress )
              {
                // went backward in memory
                var asmSegment = new Types.ASMSegment();
                asmSegment.StartAddress     = line.AddressStart;
                asmSegment.GlobalLineIndex  = line.LineIndex;
                m_ASMFileInfo.FindTrueLineSource( line.LineIndex, out asmSegment.Filename, out asmSegment.LocalLineIndex );
                builtSegments.Add( new GR.Generic.Tupel<int, Types.ASMSegment>( asmSegment.StartAddress, asmSegment ) );
                currentResultBlock = asmSegment.Data;

                /*
                if ( memoryBlockActualDataLength > 0 )
                {
                  var entry = new Types.MemoryMapEntry( memoryBlockStartAddress, memoryBlockActualDataLength );
                  entry.Description = lastAddressSourceDesc;
                  memoryMap.InsertEntry( entry );
                  lastAddressSourceDesc = "";
                }*/
                //memoryBlockStartAddress = line.AddressStart;
                //memoryBlockLength = 0;
              }
              else if ( line.AddressSource.StartsWith( "*" ) )
              {
                // a new block starts here
                /*
                var entry = new Types.MemoryMapEntry( memoryBlockStartAddress, memoryBlockActualDataLength );
                entry.Description = lastAddressSourceDesc;
                memoryMap.InsertEntry( entry );
                lastAddressSourceDesc = "";
                */
                //memoryBlockStartAddress = line.AddressStart;
                //memoryBlockLength = 0;
              }
              trueCurrentAddress = line.AddressStart;
            }
          }
          else
          {
            // was pseudo PC before
            if ( line.PseudoPCOffset == -2 )
            {
              // entered REALPC
            }
            else
            {
              if ( line.AddressStart > currentAddress )
              {
                // only add to memory map when it has actual data (e.g. not virtual)
                /*
                //if ( memoryBlockActualDataLength > 0 )
                {
                  var entry = new Types.MemoryMapEntry( memoryBlockStartAddress, memoryBlockActualDataLength );
                  entry.Description = lastAddressSourceDesc;
                  memoryMap.InsertEntry( entry );
                  lastAddressSourceDesc = "";
                }*/

                result.Append( new GR.Memory.ByteBuffer( (uint)( line.AddressStart - currentAddress ) ) );

                //memoryBlockStartAddress = line.AddressStart;
                //memoryBlockLength = 0;
              }
            }
          }
          if ( setNewAddress )
          {
            currentAddress = line.AddressStart;
            //memoryBlockLength = currentAddress - memoryBlockStartAddress;
          }
        }
        if ( line.LineData != null )
        {
          memoryBlockActualDataLength += (int)line.LineData.Length;

          // insert data (append or overwrite)
          if ( ( fileStartAddress + result.Length - dataOffset <= trueCurrentAddress )
          ||   ( line.PseudoPCOffset >= 0 ) )
          {
            while ( fileStartAddress + result.Length - dataOffset < trueCurrentAddress )
            {
              result.AppendU8( 0 );
            }
            result.Append( line.LineData );
            currentResultBlock.Append( line.LineData );
          }
          else
          {
            // overwrite
            int offsetInBuffer = currentAddress - fileStartAddress + dataOffset;
            for ( int i = 0; i < line.LineData.Length; ++i )
            {
              if ( offsetInBuffer + i < result.Length )
              {
                result.SetU8At( offsetInBuffer + i, line.LineData.ByteAt( i ) );
              }
              else
              {
                result.AppendU8( line.LineData.ByteAt( i ) );
              }
            }
            currentResultBlock.Append( line.LineData );
          }
          currentAddress += (int)line.LineData.Length;
          trueCurrentAddress += (int)line.LineData.Length;
        }

        if ( line.AddressSource.StartsWith( "*" ) )
        {
          if ( ( memoryMap.Entries.Count > 0 )
          &&   ( memoryMap.Entries[memoryMap.Entries.Count - 1].StartAddress != line.AddressStart ) )
          // &&   ( memoryBlockActualDataLength > 0 ) )
          {
            // a new memory map entry
            var entry = new Types.MemoryMapEntry( memoryBlockStartAddress, currentMemBlockActualSize );
            entry.Description = lastAddressSourceDesc;
            memoryMap.InsertEntry( entry );
            lastAddressSourceDesc = "";
          }
          else if ( ( memoryMap.Entries.Count == 0 )
          &&        ( memoryBlockStartAddress != -1 ) )
          {
            // a new memory map entry
            var entry = new Types.MemoryMapEntry( memoryBlockStartAddress, currentMemBlockActualSize );
            entry.Description = lastAddressSourceDesc;
            memoryMap.InsertEntry( entry );
          }
          lastAddressSourceDesc = line.AddressSource.Substring( 1 ).Trim( '"' );
          memoryBlockStartAddress = line.AddressStart;
          memoryBlockLength = 0;
          memoryBlockActualDataLength = 0;
          currentMemBlockActualSize = 0;
        }
        currentMemBlockActualSize += line.NumBytes;
      }

      if ( memoryBlockStartAddress > -1 )
      {
        memoryBlockLength = currentAddress - memoryBlockStartAddress;
        /*
        if ( ( memoryBlockLength > 0 )
        &&   ( memoryBlockActualDataLength > 0 ) )*/
        {
          var entry = new Types.MemoryMapEntry( memoryBlockStartAddress, memoryBlockActualDataLength );
          entry.Description = lastAddressSourceDesc;
          memoryMap.InsertEntry( entry );
        }
      }

      // determine load address
      int lowestStart = 65536;
      int highestEnd = -1;
      foreach ( var mapSegment in memoryMap.Entries )
      {
        if ( mapSegment.Length == 0 )
        {
          continue;
        }
        lowestStart = Math.Min( mapSegment.StartAddress, lowestStart );
        highestEnd = Math.Max( mapSegment.StartAddress + mapSegment.Length, highestEnd );

        if ( mapSegment.StartAddress < 0 )
        // ( segment.second.StartAddress + segment.second.Length >= 65535 ) )
        {
          int     globalLineIndex = -1;
          if ( m_ASMFileInfo.AddressToLine.ContainsKey( mapSegment.StartAddress ) )
          {
            globalLineIndex = m_ASMFileInfo.AddressToLine[mapSegment.StartAddress];
          }
          AddError( globalLineIndex,
                    Types.ErrorCode.E1106_SEGMENT_OUT_OF_BOUNDS,
                    "Segment from $"
                      + mapSegment.StartAddress.ToString( "X" )
                      + " to $" + ( mapSegment.StartAddress + mapSegment.Length - 1 ).ToString( "X" )
                      + " is out of bounds" );
        }
      }
      if ( lowestStart == 65536 )
      {
        // all virtual or no data at all? we still need a lowest address
        foreach ( var mapSegment in memoryMap.Entries )
        {
          lowestStart = Math.Min( mapSegment.StartAddress, lowestStart );
          highestEnd = Math.Max( mapSegment.StartAddress + mapSegment.Length, highestEnd );
        }
      }

      if ( ( memoryMap.Entries.Count == 0 )
      ||   ( builtSegments.Count > 0 ) )
      {
        // count from segments?
        int     newLowestStart = 65536;
        foreach ( var segment in builtSegments )
        {
          if ( segment.second.Length == 0 )
          {
            // ignore virtual segments
            continue;
          }
          newLowestStart = Math.Min( segment.first, newLowestStart );
          highestEnd = Math.Max( segment.second.StartAddress + segment.second.Length, highestEnd );

          // check, whether segments bigger than $ffff are possible!
          if ( segment.first < 0 )
          // ( segment.second.StartAddress + segment.second.Length >= 65535 ) )
          {
            AddError( segment.second.GlobalLineIndex,
                      Types.ErrorCode.E1106_SEGMENT_OUT_OF_BOUNDS,
                      "Segment from $"
                        + segment.second.StartAddress.ToString( "X" )
                        + " to $" + ( segment.second.StartAddress + segment.second.Length - 1 ).ToString( "X" )
                        + " is out of bounds" );
          }
        }
        if ( newLowestStart != 65536 )
        {
          lowestStart = newLowestStart;
        }
      }

      if ( ( lowestStart == 65536 )
      &&   ( ( memoryMap.Entries.Count != 0 )
      ||     ( builtSegments.Count > 0 ) ) )
      {
        // no real data here, and no start address either
        AddError( 0, Types.ErrorCode.E0002_CODE_WITHOUT_START_ADDRESS, "Code without start address encountered (missing *=)" );
        return false;
      }

      // check for overlaps
      if ( builtSegments.Count > 1 )
      {
        for ( int i = 0; i < builtSegments.Count - 1; ++i )
        {
          for ( int j = i + 1; j < builtSegments.Count; ++j )
          {
            if ( builtSegments[i].second.Overlaps( builtSegments[j].second ) )
            {
              //Debug.Log( "block overlap! " + builtSegments[i].first + "," + builtSegments[i].second.Length + " <> " + builtSegments[j].first + "," + builtSegments[j].second.Length );
              var message = AddSevereWarning( builtSegments[i].second.GlobalLineIndex, Types.ErrorCode.W0001_SEGMENT_OVERLAP, "Segment starts inside another one, overwriting it" );
              if ( message != null )
              {
                message.AddMessage( "  overlapping block starts in " + builtSegments[j].second.Filename + " at line " + ( builtSegments[j].second.LocalLineIndex + 1 ),
                                    builtSegments[j].second.Filename,
                                    builtSegments[j].second.LocalLineIndex );
                message.AddMessage( "  first block from $" + builtSegments[i].second.StartAddress.ToString( "X4" ) + " to $" + ( builtSegments[i].second.StartAddress + builtSegments[i].second.Length - 1 ).ToString( "X4" ),
                                    builtSegments[i].second.Filename,
                                    builtSegments[i].second.LocalLineIndex );
                message.AddMessage( "  second block from $" + builtSegments[j].second.StartAddress.ToString( "X4" ) + " to $" + ( builtSegments[j].second.StartAddress + builtSegments[j].second.Length - 1 ).ToString( "X4" ),
                                    builtSegments[j].second.Filename,
                                    builtSegments[j].second.LocalLineIndex );
              }
            }
          }
        }
      }
      // combine blocks
      GR.Memory.ByteBuffer    assembledData = new GR.Memory.ByteBuffer();

      if ( builtSegments.Count > 0 )
      {
        if ( highestEnd == -1 )
        {
          assembledData = new GR.Memory.ByteBuffer();
        }
        else
        {
          assembledData = new GR.Memory.ByteBuffer( (uint)( highestEnd - lowestStart ) );
          foreach ( var segment in builtSegments )
          {
            segment.second.Data.CopyTo( assembledData, 0, segment.second.Length, segment.first - lowestStart );
          }
        }
      }
      
      // prefix load address
      if ( TargetTypeRequiresLoadAddress( Config.TargetType ) )
      {
        assembledData = new GR.Memory.ByteBuffer( 2 ) + assembledData;
        assembledData.SetU16At( 0, (ushort)lowestStart );
      }

      // empty?
      if ( ( lowestStart == 65536 )
      &&   ( highestEnd == -1 ) )
      {
        lowestStart = 0;
        highestEnd = 0;
      }


      //Assembly = result;
      AssembledOutput = new AssemblyOutput();
      AssembledOutput.Assembly = assembledData;
      AssembledOutput.OriginalAssemblyStartAddress  = lowestStart;
      AssembledOutput.OriginalAssemblySize          = highestEnd - lowestStart;
      AssembledOutput.MemoryMap                     = memoryMap;

      string    outputPureFilename = "HURZ";
      try
      {
        outputPureFilename = System.IO.Path.GetFileNameWithoutExtension( Config.OutputFile );
      }
      catch ( Exception )
      {
        // arghh exceptions!
      }

      var resultingBinary = AssembleTarget( Config.TargetType, AssembledOutput.Assembly, outputPureFilename, fileStartAddress );
      if ( resultingBinary == null )
      {
        return false;
      }
      AssembledOutput.Assembly = resultingBinary;

      return true;
    }



    public List<Types.TokenInfo> ParseTokenInfo( string Source, int Start, int Length, GR.Collections.Map<byte, byte> TextCodeMapping )
    {
      ClearErrorInfo();

      List <Types.TokenInfo> result = new List<Types.TokenInfo>();

      if ( ( String.IsNullOrEmpty( Source ) )
      ||   ( Start >= Source.Length )
      ||   ( Start + Length > Source.Length ) )
      {
        return result;
      }

      int     charPos = Start;
      int     tokenStartPos = Start;
      Types.TokenInfo.TokenType currentTokenType = Types.TokenInfo.TokenType.UNKNOWN;

      while ( charPos < Start + Length )
      {
        char curChar = Source[charPos];

        next_token:
        if ( currentTokenType == Types.TokenInfo.TokenType.OPERATOR )
        {
          // operators are special
          int possibleOperators = 0;
          int completeOperators = 0;
          int completeOperatorLength = 0;

          foreach ( string op in m_AssemblerSettings.OperatorPrecedence.Keys )
          {
            if ( op.Length >= charPos - tokenStartPos + 1 )
            {
              if ( ( tokenStartPos + op.Length <= Source.Length )
              &&   ( string.Compare( op, 0, Source, tokenStartPos, op.Length ) == 0 ) )
              {
                // in case of overlap choose the longer match
                if ( op.Length > completeOperatorLength )
                {
                  completeOperators = 0;
                }
                ++completeOperators;
                completeOperatorLength = Math.Max( op.Length, completeOperatorLength );
                ++possibleOperators;
              }
            }
          }
          if ( completeOperators == 1 )
          {
            if ( ( charPos + 1 < Start + Length )
            &&   ( m_AssemblerSettings.AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_GLOBAL].IndexOf( Source[tokenStartPos] ) != -1 )
            &&   ( m_AssemblerSettings.AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_GLOBAL].IndexOf( Source[charPos + 1] ) != -1 ) )
            {
              // we have a text token which is not separated
            }
            else
            {
              var token = new Types.TokenInfo()
              {
                Type              = Types.TokenInfo.TokenType.OPERATOR,
                OriginatingString = Source,
                StartPos          = tokenStartPos,
                Length            = completeOperatorLength
              };
              result.Add( token );

              currentTokenType = Types.TokenInfo.TokenType.UNKNOWN;
              tokenStartPos = tokenStartPos + completeOperatorLength;
              charPos       = tokenStartPos;
              continue;
            }
          }
          // operator must be one of the shorter ones
          foreach ( string op in m_AssemblerSettings.OperatorPrecedence.Keys )
          {
            if ( op.Length == 1 )
            {
              if ( op[0] == Source[charPos - 1] )
              {
                // make sure we don't accidentally identify a pseudo op as operator
                bool    foundPseudoOp = false;

                if ( ( !string.IsNullOrEmpty( m_ASMFileInfo.AssemblerSettings.POPrefix ) )
                &&   ( op[0] == m_ASMFileInfo.AssemblerSettings.POPrefix[0] ) )
                {
                  // is there a pseudo op following?
                  foreach ( var pseudoOp in m_ASMFileInfo.AssemblerSettings.PseudoOps )
                  {
                    if ( ( Source.Length - charPos + 1 >= pseudoOp.Key.Length )
                    &&   ( string.Compare( Source, charPos - 1, pseudoOp.Key, 0, pseudoOp.Key.Length, true ) == 0 ) )
                    {
                      foundPseudoOp = true;
                      break;
                    }
                  }
                }
                if ( !foundPseudoOp )
                {
                  var token = new Types.TokenInfo()
                  {
                    Type              = currentTokenType,
                    OriginatingString = Source,
                    StartPos          = tokenStartPos,
                    Length            = 1
                  };
                  result.Add( token );

                  currentTokenType = Types.TokenInfo.TokenType.UNKNOWN;
                  tokenStartPos = charPos;
                  goto next_token;
                }
              }
            }
          }
          // we arrived here, so it was no operator in the first case
          currentTokenType = Types.TokenInfo.TokenType.UNKNOWN;
          charPos = tokenStartPos;
          foreach ( KeyValuePair<Types.TokenInfo.TokenType, string> pair in m_AssemblerSettings.AllowedTokenStartChars )
          {
            if ( pair.Value.IndexOf( Source[tokenStartPos] ) != -1 )
            {
              currentTokenType = pair.Key;
              ++charPos;
              break;
            }
          }
          if ( currentTokenType == Types.TokenInfo.TokenType.UNKNOWN )
          {
            m_LastErrorInfo.Set( -1, charPos, 1, RetroDevStudio.Types.ErrorCode.E1000_SYNTAX_ERROR );
            result.Clear();
            return result;
          }
          continue;
        }
        else if ( currentTokenType != Types.TokenInfo.TokenType.UNKNOWN )
        {
          if ( m_AssemblerSettings.AllowedTokenEndChars.ContainsKey( currentTokenType ) )
          {
            if ( m_AssemblerSettings.AllowedTokenEndChars[currentTokenType].IndexOf( curChar ) != -1 )
            {
              // end char of token found
              var token = new Types.TokenInfo()
              {
                Type              = currentTokenType,
                OriginatingString = Source,
                StartPos          = tokenStartPos,
                Length            = charPos - tokenStartPos + 1
              };

              // special case, labels with :; : is not part of label
              if ( ( currentTokenType == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
              ||   ( currentTokenType == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_INTERNAL ) )
              {
                if ( curChar == ':' )
                {
                  --token.Length;
                }
              }
              result.Add( token );

              currentTokenType = Types.TokenInfo.TokenType.UNKNOWN;
              tokenStartPos = charPos + 1;
              ++charPos;
              continue;
            }
          }
          if ( m_AssemblerSettings.AllowedTokenChars.ContainsKey( currentTokenType ) )
          {
            if ( m_AssemblerSettings.AllowedTokenChars[currentTokenType].IndexOf( curChar ) != -1 )
            {
              // token continues

            }
            else
            {
              // end char of token found
              var token = new Types.TokenInfo()
              {
                Type              = currentTokenType,
                OriginatingString = Source,
                StartPos          = tokenStartPos,
                Length            = charPos - tokenStartPos
              };
              result.Add( token );

              currentTokenType = Types.TokenInfo.TokenType.UNKNOWN;
              tokenStartPos = charPos;
              goto next_token;
            }
          }
          else if ( m_AssemblerSettings.AllowedTokenEndChars.ContainsKey( currentTokenType ) )
          {
            if ( m_AssemblerSettings.AllowedTokenEndChars[currentTokenType].IndexOf( curChar ) != -1 )
            {
              // end char of token found
              var token = new Types.TokenInfo()
              {
                Type              = currentTokenType,
                OriginatingString = Source,
                StartPos          = tokenStartPos,
                Length            = charPos - tokenStartPos + 1
              };
              result.Add( token );

              currentTokenType = Types.TokenInfo.TokenType.UNKNOWN;
              tokenStartPos = charPos + 1;
              ++charPos;
              continue;
            }
          }
          else// 
          {
            // auto end token
            var token = new Types.TokenInfo()
            {
              Type              = currentTokenType,
              OriginatingString = Source,
              StartPos          = tokenStartPos,
              Length            = charPos - tokenStartPos
            };
            result.Add( token );

            currentTokenType = Types.TokenInfo.TokenType.UNKNOWN;

            if ( IsBlankChar( curChar ) )
            {
              tokenStartPos = charPos + 1;
              continue;
            }
            tokenStartPos = charPos;
            goto next_token;
          }
        }
        else
        {
          if ( m_AssemblerSettings.AllowedSingleTokens.IndexOf( curChar ) != -1 )
          {
            var token = new Types.TokenInfo()
            {
              Type              = Types.TokenInfo.TokenType.SEPARATOR,
              OriginatingString = Source,
              StartPos          = tokenStartPos,
              Length            = 1
            };
            result.Add( token );

            currentTokenType = Types.TokenInfo.TokenType.UNKNOWN;
            tokenStartPos = charPos + 1;
            ++charPos;
            continue;
          }
          else
          {
            if ( IsBlankChar( curChar ) )
            {
              ++tokenStartPos;
              ++charPos;
              continue;
            }
            int possibleOperators = 0;
            int completeOperators = 0;

            foreach ( string op in m_AssemblerSettings.OperatorPrecedence.Keys )
            {
              if ( curChar == op[0] )
              {
                currentTokenType = Types.TokenInfo.TokenType.OPERATOR;
                if ( op.Length == 1 )
                {
                  ++completeOperators;
                }
                ++possibleOperators;
              }
            }
            if ( ( possibleOperators == 1 )
            &&   ( completeOperators == 1 ) )
            {
              if ( result.Count == 0 )
              {
                if ( m_AssemblerSettings.AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_LOCAL].IndexOf( curChar ) != -1 )
                {
                  // could also be an internal label!
                  currentTokenType = Types.TokenInfo.TokenType.LABEL_LOCAL;
                  tokenStartPos = charPos;
                  ++charPos;
                  continue;
                }
              }

              if ( ( charPos < Start + Length )
              &&   ( m_AssemblerSettings.AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_GLOBAL].IndexOf( Source[tokenStartPos] ) != -1 )
              &&   ( m_AssemblerSettings.AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_GLOBAL].IndexOf( Source[charPos + 1] ) != -1 ) )
              {
                // we have a text token which is not separated
              }
              else
              {
                Types.TokenInfo token = new Types.TokenInfo();
                token.Type = Types.TokenInfo.TokenType.OPERATOR;
                token.OriginatingString = Source;
                token.StartPos = tokenStartPos;
                token.Length = charPos - tokenStartPos + 1;
                result.Add( token );

                currentTokenType = Types.TokenInfo.TokenType.UNKNOWN;
                tokenStartPos = charPos + 1;
                ++charPos;
                continue;
              }
            }
            if ( possibleOperators == 0 )
            {
              if ( charPos > tokenStartPos )
              {
                // some chars were before, but they didn't form a token
                m_LastErrorInfo.Set( -1, tokenStartPos, charPos - tokenStartPos, RetroDevStudio.Types.ErrorCode.E1000_SYNTAX_ERROR );
                result.Clear();
                return result;
              }
              foreach ( KeyValuePair<Types.TokenInfo.TokenType, string> pair in m_AssemblerSettings.AllowedTokenStartChars )
              {
                if ( pair.Value.IndexOf( curChar ) != -1 )
                {
                  currentTokenType = pair.Key;
                  tokenStartPos = charPos;

                  if ( currentTokenType == RetroDevStudio.Types.TokenInfo.TokenType.COMMENT )
                  {
                    // the rest of the line is a comment!
                    Types.TokenInfo token = new Types.TokenInfo();
                    token.Type = currentTokenType;
                    token.OriginatingString = Source;
                    token.StartPos = tokenStartPos;
                    token.Length = Length - tokenStartPos;
                    result.Add( token );

                    goto all_tokens_handled;
                  }
                  break;
                }
              }
              // separator char?
              if ( currentTokenType == RetroDevStudio.Types.TokenInfo.TokenType.UNKNOWN )
              {
                bool    found = false;
                foreach ( var separatorChar in m_AssemblerSettings.StatementSeparatorChars )
                {
                  if ( curChar == separatorChar )
                  {
                    Types.TokenInfo token = new Types.TokenInfo();
                    token.Type = Types.TokenInfo.TokenType.SEPARATOR;
                    token.OriginatingString = Source;
                    token.StartPos = tokenStartPos;
                    token.Length = charPos - tokenStartPos + 1;
                    result.Add( token );

                    currentTokenType = Types.TokenInfo.TokenType.UNKNOWN;
                    tokenStartPos = charPos + 1;
                    ++charPos;
                    found = true;
                    break;
                  }
                }
                if ( found )
                {
                  continue;
                }
              }

            }
          }
        }
        ++charPos;
      }
      if ( ( tokenStartPos < charPos )
      &&   ( currentTokenType != Types.TokenInfo.TokenType.UNKNOWN ) )
      {
        // auto end token
        Types.TokenInfo token = new Types.TokenInfo();
        token.Type = currentTokenType;
        token.OriginatingString = Source;
        token.StartPos = tokenStartPos;
        token.Length = charPos - tokenStartPos;
        result.Add( token );
      }

      all_tokens_handled:
      CollapseLabelsAtStartOfLine( result );

      // collapse binary tokens (with # and .), before doing preprocessor!
      CollapseBinaryTokens( result );

      // collapse ## (with labels!)
      CollapsePreprocessorLabels( result, TextCodeMapping );

      // if the last token is an internal label (+/-), and something useless is following, it is interpreted as operator
      if ( ( result.Count >= 2 )
      &&   ( result[result.Count - 1].Type == TokenInfo.TokenType.COMMENT )
      &&   ( result[result.Count - 2].Type == TokenInfo.TokenType.OPERATOR )
      &&   ( ( result[result.Count - 2].Content == "+" )
      ||     ( result[result.Count - 2].Content == "-" ) ) )
      {
        result[result.Count - 1].Type = TokenInfo.TokenType.LABEL_INTERNAL;
        result.RemoveAt( result.Count - 1 );
      }

      /*
      // collapse # 
      if ( ( result.Count >= 3 )
      &&   ( result[1].Type == Types.TokenInfo.TokenType.SEPARATOR )
      &&   ( result[1].Length == 1 )
      &&   ( Source[result[1].StartPos] == '#' )
      &&   ( result[2].Type == Types.TokenInfo.TokenType.LITERAL_NUMBER )
      &&   ( result[2].StartPos == result[1].StartPos + 1 ) )
      {
        --result[2].StartPos;
        ++result[2].Length;
        result.RemoveAt( 1 );
      }*/

      // collapse # if prefixed to < or >
      /*
      if ( result.Count >= 3 )
      {
        for ( int i = 2; i < result.Count - 1; ++i )
        {
          if ( ( result[i].Content == "#" )
          &&   ( ( result[i + 1].Content == "<" )
          ||     ( result[i + 1].Content == ">" ) )
          &&   ( result[i].StartPos + result[i].Length == result[i + 1].StartPos ) )
          {
            // collapse
            result.RemoveAt( i );
            --i;
            continue;
          }
        }
      }*/

      CollapsePDSLocalLabels( result );

      // labels must be left?
      if ( m_AssemblerSettings.LabelsMustBeAtStartOfLine )
      {
        if ( ( result.Count > 0 )
        &&   ( result[0].StartPos > 0 )
        &&   ( result[0].Type == TokenInfo.TokenType.LABEL_GLOBAL )
        &&   ( !m_AssemblerSettings.PseudoOps.ContainsKey( result[0].Content.ToUpper() ) ) )
        {
          // PDS - labels must be at the very start, if not, they are NOT labels (but probably macros)
          result[0].Type = TokenInfo.TokenType.CALL_MACRO;
        }
      }


      // collapse & if prefixed to literal number
      if ( ( m_AssemblerSettings.AllowedTokenStartChars[TokenInfo.TokenType.LITERAL_NUMBER].Contains( "&" ) )
      &&   ( result.Count >= 2 ) )
      {
        for ( int i = 0; i < result.Count - 1; ++i )
        {
          if ( ( result[i].Content == "&" )
          &&   ( ( result[i + 1].Type == Types.TokenInfo.TokenType.LITERAL_NUMBER )
          ||     ( result[i + 1].Type == Types.TokenInfo.TokenType.LABEL_GLOBAL ) )
          &&   ( result[i].StartPos + result[i].Length == result[i + 1].StartPos ) )
          {
            // collapse
            result[i].Content = "&" + result[i + 1].Content;
            result[i].Length = result[i].Content.Length;
            result[i].Type = Types.TokenInfo.TokenType.LITERAL_NUMBER;
            result.RemoveAt( i + 1 );
            --i;
            continue;
          }
        }
      }

      // collapse literal number if followed by local label with numbers (real number)
      if ( result.Count >= 2 )
      {
        for ( int i = 0; i < result.Count - 1; ++i )
        {
          if ( ( result[i].Type == TokenInfo.TokenType.LITERAL_NUMBER )
          &&   ( result[i + 1].Type == Types.TokenInfo.TokenType.LABEL_LOCAL )
          &&   ( IsNumeric( result[i + 1].Content.Substring( 1 ) ) )
          &&   ( result[i].StartPos + result[i].Length == result[i + 1].StartPos ) )
          {
            // collapse
            result[i].Content += result[i + 1].Content;
            result[i].Length += result[i + 1].Content.Length;
            result[i].Type = Types.TokenInfo.TokenType.LITERAL_REAL_NUMBER;
            result.RemoveAt( i + 1 );
            --i;
            continue;
          }
        }
      }

      // collapse % if prefixed to literal number
      if ( result.Count >= 3 )
      {
        for ( int i = 0; i < result.Count - 1; ++i )
        {
          // collapse - with literal to negative number, this requires all kind of checks
          if ( CanCollapseMinusInFrontOfLiteralToNegativeNumber( result, i ) )
          {
            // collapse 
            result[i].Content = "-" + result[i + 1].Content;
            result[i].Length = result[i].Content.Length;
            result[i].Type = Types.TokenInfo.TokenType.LITERAL_NUMBER;
            result.RemoveAt( i + 1 );
            --i;
            continue;
          }

          if ( ( result[i].Content == "%" )
          &&   ( result[i + 1].Type == Types.TokenInfo.TokenType.LITERAL_NUMBER )
          &&   ( result[i].StartPos + result[i].Length == result[i + 1].StartPos ) )
          {
            // collapse
            result[i].Content = "%" + result[i + 1].Content;
            result[i].Length = result[i].Content.Length;
            result[i].Type = Types.TokenInfo.TokenType.LITERAL_NUMBER;
            result.RemoveAt( i + 1 );
            --i;
            continue;
          }

          // collapse binary representations
          if ( ( result[i].Content[0] == '%' )
          &&   ( result[i + 1].Type == RetroDevStudio.Types.TokenInfo.TokenType.SEPARATOR )
          &&   ( result[i].StartPos + result[i].Length == result[i + 1].StartPos )
          &&   ( result[i + 1].Content == "#" ) )
          {
            // collapse
            result[i].Content += result[i + 1].Content;
            result[i].Length += result[i + 1].Content.Length;
            result[i].Type = Types.TokenInfo.TokenType.LITERAL_NUMBER;
            result.RemoveAt( i + 1 );
            --i;
            continue;
          }
          // collapse binary representations
          if ( ( result[i].Content[0] == '%' )
          &&   ( result[i + 1].Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_LOCAL )
          &&   ( result[i].StartPos + result[i].Length == result[i + 1].StartPos ) )
          {
            // # and . are allowed
            bool  allValid = true;
            for ( int j = 0; j < result[i + 1].Content.Length; ++j )
            { 
              if ( ( result[i + 1].Content[j] != '.' )
              &&   ( result[i + 1].Content[j] != '#' ) )
              {
                allValid = false;
                break;
              }
            }
            if ( allValid )
            {
              // collapse completely
              result[i].Content += result[i + 1].Content;
              result[i].Length += result[i + 1].Content.Length;
              result[i].Type = Types.TokenInfo.TokenType.LITERAL_NUMBER;
              result.RemoveAt( i + 1 );
              --i;
              continue;
            }

            // several dots are connected as local label, only use first .
            int   dotEnd = -1;

            while ( ( dotEnd + 1 < result[i + 1].Content.Length )
            &&      ( result[i + 1].Content[dotEnd + 1] == '.' ) )
            {
              ++dotEnd;
            }

            if ( dotEnd == -1 )
            {
              // nothing to do
              continue;
            }
            if ( dotEnd + 1 == result[i + 1].Content.Length )
            {
              // only dots
              // collapse completely
              result[i].Content += result[i + 1].Content;
              result[i].Length += result[i + 1].Content.Length;
              result[i].Type = Types.TokenInfo.TokenType.LITERAL_NUMBER;
              result.RemoveAt( i + 1 );
              --i;
              continue;
            }
            else
            {
              // split and collapse 
              result[i].Content += result[i + 1].Content.Substring( 0, dotEnd + 1 );
              result[i].Length += dotEnd + 1;
              result[i].Type = Types.TokenInfo.TokenType.LITERAL_NUMBER;
              result[i + 1].StartPos += dotEnd + 1;
              result[i + 1].Length -= dotEnd + 1;
              result[i + 1].Content = null;
              --i;
              continue;
            }
          }
        }
      }

      foreach ( var token in result )
      {
        string    lowerToken = token.Content.ToLower();
        // special case opcode with # as sign of direct value
        if ( lowerToken.EndsWith( "#" ) )
        {
          lowerToken = lowerToken.Substring( 0, lowerToken.Length - 1 );
          if ( m_Processor.Opcodes.ContainsKey( lowerToken ) )
          {
            --token.Length;
            token.Type = Types.TokenInfo.TokenType.OPCODE_DIRECT_VALUE;
            break;
          }
        }
        if ( m_Processor.Opcodes.ContainsKey( lowerToken ) )
        {
          token.Type = RetroDevStudio.Types.TokenInfo.TokenType.OPCODE;
        }
      }

      // collapse label with size identifier
      if ( result.Count >= 3 )
      {
        for ( int i = 0; i < result.Count - 2; ++i )
        {
          if ( result[i].Type == TokenInfo.TokenType.CALL_MACRO )
          {
            foreach ( var opID1 in m_AssemblerSettings.OpcodeSizeIdentifierOneByteOperands )
            {
              if ( result[i].Length >= opID1.Length + m_AssemblerSettings.OpcodeSizeIdentifierSeparator.Length )
              {
                if ( ( string.Compare( result[i].Content, result[i].Length - opID1.Length,
                                       opID1, 0, opID1.Length ) == 0 )
                &&   ( string.Compare( result[i].Content, result[i].Length - opID1.Length - m_AssemblerSettings.OpcodeSizeIdentifierSeparator.Length,
                                       m_AssemblerSettings.OpcodeSizeIdentifierSeparator, 0, m_AssemblerSettings.OpcodeSizeIdentifierSeparator.Length ) == 0 ) )
                {
                  string lowerToken = result[i].Content.Substring( 0, result[i].Content.Length - opID1.Length - m_AssemblerSettings.OpcodeSizeIdentifierSeparator.Length );
                  if ( m_Processor.Opcodes.ContainsKey( lowerToken ) )
                  {
                    result[i].Type = TokenInfo.TokenType.OPCODE_FIXED_ZP;
                    result[i].Content = lowerToken;
                    break;
                  }
                }
              }
            }
            foreach ( var opID2 in m_AssemblerSettings.OpcodeSizeIdentifierTwoByteOperands )
            {
              if ( result[i].Length >= opID2.Length + m_AssemblerSettings.OpcodeSizeIdentifierSeparator.Length )
              {
                if ( ( string.Compare( result[i].Content, result[i].Length - opID2.Length,
                                       opID2, 0, opID2.Length ) == 0 )
                &&   ( string.Compare( result[i].Content, result[i].Length - opID2.Length - m_AssemblerSettings.OpcodeSizeIdentifierSeparator.Length,
                                       m_AssemblerSettings.OpcodeSizeIdentifierSeparator, 0, m_AssemblerSettings.OpcodeSizeIdentifierSeparator.Length ) == 0 ) )
                {
                  string lowerToken = result[i].Content.Substring( 0, result[i].Content.Length - opID2.Length - m_AssemblerSettings.OpcodeSizeIdentifierSeparator.Length );
                  if ( m_Processor.Opcodes.ContainsKey( lowerToken ) )
                  {
                    result[i].Type = TokenInfo.TokenType.OPCODE_FIXED_NON_ZP;
                    result[i].Content = lowerToken;
                    break;
                  }
                }
              }
            }
          }
          if ( ( result[i].Type == RetroDevStudio.Types.TokenInfo.TokenType.OPCODE )
          &&   ( result[i].StartPos + result[i].Length == result[i + 1].StartPos )
          &&   ( result[i + 1].Content == m_AssemblerSettings.OpcodeSizeIdentifierSeparator )
          &&   ( result[i + 1].StartPos + result[i + 1].Length == result[i + 2].StartPos )
          &&   ( ( m_AssemblerSettings.OpcodeSizeIdentifierOneByteOperands.Contains( result[i + 2].Content ) )
          ||     ( m_AssemblerSettings.OpcodeSizeIdentifierTwoByteOperands.Contains( result[i + 2].Content ) ) ) )
          {
            // combine!
            if ( m_AssemblerSettings.OpcodeSizeIdentifierOneByteOperands.Contains( result[i + 2].Content ) )
            {
              result[i].Type = RetroDevStudio.Types.TokenInfo.TokenType.OPCODE_FIXED_ZP;
            }
            else
            {
              result[i].Type = RetroDevStudio.Types.TokenInfo.TokenType.OPCODE_FIXED_NON_ZP;
            }
            result.RemoveRange( i + 1, 2 );
            break;
          }
        }
      }

      // macro'd internal labels turn up as global label, operator, global label, collapse into one token
      for ( int i = 0; i < result.Count; ++i )
      {
        if ( ( result[i].Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
        && ( result[i].Content == InternalLabelPrefix ) )
        {
          int     curPos = i + 1;

          while ( true )
          {
            if ( ( result[curPos].Type == RetroDevStudio.Types.TokenInfo.TokenType.OPERATOR )
            &&   ( result[curPos].StartPos == result[curPos - 1].StartPos + result[curPos - 1].Length )
            &&   ( ( result[curPos].Content == "-" )
            ||     ( result[curPos].Content == "+" ) ) )
            {
              // continue
              result[i].Content += result[curPos].Content;
              result[i].Length += result[curPos].Length;
              ++curPos;
            }
            else if ( ( curPos < result.Count )
            &&        ( result[curPos].Type == RetroDevStudio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
            &&        ( result[curPos].StartPos == result[curPos - 1].StartPos + result[curPos - 1].Length )
            &&        ( result[curPos].Content.StartsWith( InternalLabelPostfix ) ) )
            {
              // final label
              result[i].Type = RetroDevStudio.Types.TokenInfo.TokenType.LABEL_INTERNAL;
              result[i].Content += result[curPos].Content;
              result[i].Length += result[curPos].Length;
              result.RemoveRange( i + 1, curPos - i );
              i = result.Count;
              break;
            }
            else
            {
              break;
            }
          }
        }
      }

      // collapse internal labels
      if ( result.Count > 1 )
      {
        for ( int i = 0; i < result.Count - 1; ++i )
        {
          if ( ( ( ( result[i].Type == Types.TokenInfo.TokenType.OPERATOR )
          &&       ( result[i].Length == 1 ) )
          ||     ( result[i].Type == Types.TokenInfo.TokenType.LABEL_INTERNAL ) )
          &&   ( result[i + 1].Type == Types.TokenInfo.TokenType.OPERATOR )
          &&   ( result[i + 1].Length == 1 )
          &&   ( result[i].StartPos + result[i].Length == result[i + 1].StartPos )
          &&   ( m_AssemblerSettings.AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_INTERNAL].IndexOf( result[i].Content[0] ) != -1 )
          &&   ( result[i].Content[0] == result[i + 1].Content[0] ) )
          {
            ++result[i].Length;
            result[i].Type = Types.TokenInfo.TokenType.LABEL_INTERNAL;
            result.RemoveAt( i + 1 );
            i = -1;
            continue;
          }
          // internal label in front
          if ( ( result[i].Type == TokenInfo.TokenType.OPERATOR )
          &&   ( ( result[i].Content.StartsWith( "+" ) )
          ||     ( result[i].Content.StartsWith( "-" ) ) ) )
          {
            // is directly connected to global label then the internal label is split
            if ( ( result.Count > 1 )
            &&   ( result[i + 1].Type == TokenInfo.TokenType.LABEL_GLOBAL )
            &&   ( result[i + 1].StartPos == result[i].StartPos + result[i].Length )
            &&   ( result[i + 1].Content.StartsWith( InternalLabelPrefix ) ) )
            {
              result[i].Type = TokenInfo.TokenType.LABEL_INTERNAL;
              result[i].Content += result[i + 1].Content;
              result[i].Length += result[i + 1].Length;
              result.RemoveAt( i + 1 );
              i = -1;
              continue;
            }
          }
        }
        // opcode followed by internal label?
        if ( ( result[0].Type == TokenInfo.TokenType.OPCODE )
        ||   ( result[0].Type == TokenInfo.TokenType.OPCODE_FIXED_NON_ZP )
        ||   ( result[0].Type == TokenInfo.TokenType.OPCODE_FIXED_ZP )
        ||   ( result[0].Type == TokenInfo.TokenType.OPCODE_DIRECT_VALUE ) )
        {
          if ( ( result[1].Type == TokenInfo.TokenType.OPERATOR )
          &&   ( ( result[1].Content.StartsWith( "+" ) )
          ||     ( result[1].Content.StartsWith( "-" ) ) ) )
          {
            result[1].Type = TokenInfo.TokenType.LABEL_INTERNAL;
          }
        }
      }
      if ( result.Count >= 1 )
      {
        // starting with internal label?
        if ( ( result[0].Type == TokenInfo.TokenType.OPERATOR )
        &&   ( ( result[0].Content.StartsWith( "+" ) )
        ||     ( result[0].Content.StartsWith( "-" ) ) ) )
        {
          if ( ( result.Count > 1 )
          &&   ( result[1].Type == TokenInfo.TokenType.LABEL_GLOBAL )
          &&   ( result[1].StartPos == result[0].StartPos + result[0].Length )
          &&   ( result[1].Content.StartsWith( InternalLabelPrefix ) ) )
          {
            result[0].Type = TokenInfo.TokenType.LABEL_INTERNAL;
          }
          else if ( result.Count == 1 )
          {
            result[0].Type = TokenInfo.TokenType.LABEL_INTERNAL;
          }
        }
      }

      // DASM style macro params
      bool    foundMacroParam = true;
      while ( ( result.Count >= 3 )
      &&      ( foundMacroParam ) )
      {
        foundMacroParam = false;
        for ( int i = 0; i < result.Count - 2; ++i )
        {
          if ( ( result[i].Content == "{" )
          &&   ( result[i].StartPos + result[i].Length == result[i + 1].StartPos )
          &&   ( result[i + 1].Type == TokenInfo.TokenType.LITERAL_NUMBER )
          &&   ( result[i + 1].StartPos + result[i + 1].Length == result[i + 2].StartPos )
          &&   ( result[i + 2].Content == "}" ) )
          {
            result[i].Content += result[i + 1].Content + result[i + 2].Content;
            result[i].Type = TokenInfo.TokenType.MACRO_PARAMETER;
            result.RemoveRange( i + 1, 2 );
            foundMacroParam = true;
            break;
          }
        }
      }
      return result;
    }



    private void CollapseBinaryTokens( List<TokenInfo> Result )
    {
      retry:;
      for ( int i = 1; i < Result.Count; ++i )
      {
        if ( ( Result[i].Type == TokenInfo.TokenType.OPERATOR )
        &&   ( Result[i].Content == "%" ) )
        {
          int     firstCollapseTokenIndex = -1;
          int     lastCollapseTokenIndex = -1;
          int     lastEndPos = Result[i].EndPos;
          for ( int j = i + 1; j < Result.Count; ++j )
          {
            if ( ( Result[j].StartPos == lastEndPos + 1 )
            &&   ( !Result[j].Content.Any( c => c != '.' && c != '#' ) ) )
            {
              if ( firstCollapseTokenIndex == -1 )
              {
                firstCollapseTokenIndex = j;
              }
              lastCollapseTokenIndex = j;
              lastEndPos = Result[j].EndPos;
            }
            else
            {
              break;
            }
          }

          if ( ( firstCollapseTokenIndex != -1 )
          &&   ( lastCollapseTokenIndex > firstCollapseTokenIndex ) )
          {
            for ( int j = firstCollapseTokenIndex; j <= lastCollapseTokenIndex; ++j )
            {
              Result[i].Content += Result[j].Content;
              Result[i].Length += Result[j].Length;
            }
            Result[i].Type = TokenInfo.TokenType.LITERAL_NUMBER;
            Result.RemoveRange( firstCollapseTokenIndex, lastCollapseTokenIndex - firstCollapseTokenIndex + 1 );
            goto retry;
          }
        }
      }
    }



    private bool IsNumeric( string Text )
    {
      if ( string.IsNullOrEmpty( Text ) )
      {
        return false;
      }
      foreach ( char character in Text )
      {
        if ( !char.IsNumber( character ) )
        {
          return false;
        }
      }
      return true;
    }



    private void CollapseLabelsAtStartOfLine( List<TokenInfo> Result )
    {
      if ( ( !m_AssemblerSettings.LabelsMustBeAtStartOfLine )
      ||   ( Result.Count <= 1 ) )
      {
        return;
      }

      // DASM mode, everything at the start of the line until an optional : is a label
      if ( Result[0].StartPos != 0 )
      {
        return;
      }

      // is there a ':'?
      int     lastIndex = Result.Count - 1;
      bool    endsWithColon = false;
      for ( int i = 1; i < Result.Count; ++i )
      {
        if ( Result[i].StartPos > Result[i - 1].StartPos + Result[i - 1].Length )
        {
          // tokens are not connected
          lastIndex = i - 1;
          break;
        }
        if ( Result[i].Content == ":" )
        {
          endsWithColon = true;
          lastIndex = i - 1;
          break;
        }
        if ( ( Result[i].Content == ":" )
        ||   ( Result[i].Content == "=" ) )
        {
          lastIndex = i - 1;
          break;
        }
      }
      for ( int i = 1; i <= lastIndex; ++i )
      {
        Result[0].Content += Result[i].Content;
        Result[0].Length += Result[i].Length;
      }
      // remove colon to avoid later problems
      if ( endsWithColon )
      {
        ++lastIndex;
      }
      if ( lastIndex > 1 )
      {
        Result.RemoveRange( 1, lastIndex );
      }
    }



    private bool CanCollapseMinusInFrontOfLiteralToNegativeNumber( List<TokenInfo> Tokens, int IndexOfMinusToken )
    {
      if ( ( IndexOfMinusToken > 0 )
      &&   ( IndexOfMinusToken + 1 < Tokens.Count ) )
      {
        var minusToken = Tokens[IndexOfMinusToken];
        var prevToken = Tokens[IndexOfMinusToken - 1];
        var nextToken = Tokens[IndexOfMinusToken + 1];

        if ( ( minusToken.Content == "-" )
        &&   ( Tokens[IndexOfMinusToken + 1].Type == Types.TokenInfo.TokenType.LITERAL_NUMBER )
        &&   ( Tokens[IndexOfMinusToken].StartPos + Tokens[IndexOfMinusToken].Length == Tokens[IndexOfMinusToken + 1].StartPos )
        &&   ( ( prevToken.EndPos + 1 < Tokens[IndexOfMinusToken].StartPos )
        ||     ( ( prevToken.EndPos + 1 == Tokens[IndexOfMinusToken].StartPos )
        &&       ( prevToken.Type != TokenInfo.TokenType.LITERAL_NUMBER )
        &&       ( prevToken.Type != TokenInfo.TokenType.LITERAL_STRING )
        &&       ( prevToken.Type != TokenInfo.TokenType.LITERAL_REAL_NUMBER )
        &&       ( prevToken.Type != TokenInfo.TokenType.LITERAL_CHAR ) ) )
        &&   ( !IsTokenLabel( Tokens[IndexOfMinusToken - 1].Type ) )
        &&   ( ( prevToken.Type != TokenInfo.TokenType.SEPARATOR )
        ||     ( ( Tokens[IndexOfMinusToken - 1].Content != AssemblerSettings.INTERNAL_CLOSING_BRACE )
        &&       ( Tokens[IndexOfMinusToken - 1].Content != ")" ) ) )
        &&   ( Tokens[IndexOfMinusToken - 1].Content != "*" ) )
        {
          return true;
        }
      }
      return false;
    }



    private void CollapsePreprocessorLabels( List<TokenInfo> result, GR.Collections.Map<byte, byte> TextCodeMapping )
    {
      // collapse <label#>#<value> (make a single token label name)
      if ( result.Count >= 3 )
      {
        for ( int i = 1; i <= result.Count - 2; ++i )
        {
          if ( ( IsTokenLabel( result[i -1].Type ) )
          &&   ( result[i - 1].Content.EndsWith( "#" ) )
          &&   ( result[i].Content == "#" )
          &&   ( IsTokenLabel( result[i - 1].Type ) )
          &&   ( IsTokenLabel( result[i + 1].Type ) ) )
          {
            // collapse
            if ( EvaluateLabel( -1, result[i + 1].Content, TextCodeMapping, out long labelValue ) )
            {
              result[i - 1].Content = result[i - 1].Content.Substring( 0, result[i - 1].Length - 1 ) + labelValue.ToString();
              result[i - 1].Length  = result[i - 1].Content.Length;
              result.RemoveRange( i, 2 );
              --i;
              continue;
            }
          }
        }
      }
      // collapse <label>##<value> (make a single token label name)
      if ( result.Count >= 4 )
      {
        for ( int i = 1; i <= result.Count - 3; ++i )
        {
          if ( ( IsTokenLabel( result[i -1].Type ) )
          &&   ( result[i].Content == "#" )
          &&   ( result[i + 1].Content == "#" )
          &&   ( IsTokenLabel( result[i + 2].Type ) ) )
          {
            // collapse
            if ( EvaluateLabel( -1, result[i + 2].Content, TextCodeMapping, out long labelValue ) )
            {
              result[i - 1].Content = result[i - 1].Content + labelValue.ToString();
              result[i - 1].Length  = result[i - 1].Content.Length;
              result.RemoveRange( i, 3 );
              --i;
              continue;
            }
          }
        }
      }
      ClearErrorInfo();
    }



    private void CollapsePDSLocalLabels( List<TokenInfo> result )
    {
      // collapse PDS local label (! is interpreted as operator)
      if ( ( m_AssemblerSettings.AllowedTokenStartChars[TokenInfo.TokenType.LABEL_LOCAL].Contains( "!" ) )
      && ( result.Count >= 2 ) )
      {
        for ( int i = 0; i < result.Count - 1; ++i )
        {
          if ( ( result[i].Content == "!" )
          &&   ( ( result[i + 1].Type == Types.TokenInfo.TokenType.LABEL_GLOBAL )
          ||     ( result[i + 1].Type == Types.TokenInfo.TokenType.LITERAL_NUMBER ) )
          &&   ( result[i].StartPos + result[i].Length == result[i + 1].StartPos ) )
          {
            // collapse
            result[i].Content = "!" + result[i + 1].Content;
            result[i].Length = result[i].Content.Length;
            result[i].Type = Types.TokenInfo.TokenType.LABEL_LOCAL;
            result.RemoveAt( i + 1 );
            --i;
            continue;
          }
        }
      }
    }
    
    
    
    private bool IsBlankChar( char Character )
    {
      if ( ( Character == ' ' )
      ||   ( Character == '\t' ) )
      {
        return true;
      }
      return false;
    }



    private GR.Generic.Tupel<Tiny64.Opcode, bool> EstimateOpcode( int LineIndex, List<Types.TokenInfo> LineTokens, List<Tiny64.Opcode> PossibleOpcodes, ref Types.ASM.LineInfo info, out List<List<TokenInfo>> OpcodeExpressions, out ulong ResultingOpcodePatchValue, out bool HadError )
    {
      ResultingOpcodePatchValue = 0;
      OpcodeExpressions         = null;
      HadError                  = false;

      // lineTokens[0] contains the mnemonic
      if ( LineTokens.Count == 0 )
      {
        // can't be, error!
        AddError( LineIndex,
                  Types.ErrorCode.E1300_OPCODE_AMBIGIOUS,
                  "Could not determine correct opcode for empty line", 0, 0 );
        HadError = true;
        return null;
      }

      if ( ( LineTokens[0].Type == Types.TokenInfo.TokenType.OPCODE_DIRECT_VALUE )
      &&   ( PossibleOpcodes.Count == 1 )
      &&   ( ( PossibleOpcodes[0].Addressing == Tiny64.Opcode.AddressingType.IMMEDIATE_ACCU )
      ||     ( PossibleOpcodes[0].Addressing == Opcode.AddressingType.IMMEDIATE_8BIT )
      ||     ( PossibleOpcodes[0].Addressing == Tiny64.Opcode.AddressingType.IMMEDIATE_REGISTER ) ) )
      {
        // that one is given
        return new GR.Generic.Tupel<Tiny64.Opcode, bool>( PossibleOpcodes[0], false );
      }

      if ( MatchOpcodeToExpression( LineIndex, PossibleOpcodes, LineTokens, out Opcode matchingOpcode, out OpcodeExpressions, out ResultingOpcodePatchValue ) )
      {
        return new GR.Generic.Tupel<Opcode, bool>( matchingOpcode, false );
      }
      if ( m_Processor.Name == "Motorola 68000" )
      {
        AddError( LineIndex, Types.ErrorCode.E1105_INVALID_OPCODE, "Cannot determine opcode: " + LineTokens[0].Content, LineTokens[0].StartPos, LineTokens[0].Length );
        HadError = true;
        return null;
      }

      // TODO - get rid of all this crap below!
      bool endsWithCommaX = false;
      bool endsWithCommaY = false;
      bool endsWithCommaZ = false;
      bool oneParamInBrackets = false;
      bool twoParamsInBrackets = false;
      bool secondParamIsSP = false;
      bool startWithOpeningBrace = false;
      bool endsWithClosingBrace = false;
      bool longMode = false;
      int numBytesFirstParam = 0;
      int expressionTokenStartIndex = 1;
      int expressionTokenCount = LineTokens.Count - 1;
      int expressionTokenCountForLaterEvaluation = LineTokens.Count - 1;

      if ( LineTokens.Count >= 2 )
      {
        if ( ( LineTokens[LineTokens.Count - 2].Content == "," )
        &&   ( LineTokens[LineTokens.Count - 1].Content.ToUpper() == "X" ) )
        {
          endsWithCommaX = true;
          expressionTokenCount = LineTokens.Count - 2 - expressionTokenStartIndex;
          expressionTokenCountForLaterEvaluation = expressionTokenCount + 2; 
        }
        if ( ( LineTokens[LineTokens.Count - 2].Content == "," )
        &&   ( LineTokens[LineTokens.Count - 1].Content.ToUpper() == "Y" ) )
        {
          endsWithCommaY = true;
          expressionTokenCount = LineTokens.Count - 2 - expressionTokenStartIndex;
          expressionTokenCountForLaterEvaluation = expressionTokenCount + 2;
        }
        if ( ( LineTokens[LineTokens.Count - 2].Content == "," )
        &&   ( LineTokens[LineTokens.Count - 1].Content.ToUpper() == "Z" ) )
        {
          endsWithCommaZ = true;
          expressionTokenCount = LineTokens.Count - 2 - expressionTokenStartIndex;
          expressionTokenCountForLaterEvaluation = expressionTokenCount + 2;
        }

        // TODO detect , if not ,x or ,y
        if ( ( !endsWithCommaX )
        &&   ( !endsWithCommaY )
        &&   ( !endsWithCommaZ ) )
        {
          for ( int i = 1; i < LineTokens.Count - 1; ++i )
          {
            if ( LineTokens[i].Content == "," )
            {
              // potential zp,rel addressing
              foreach ( var potentialOp in PossibleOpcodes )
              {
                if ( potentialOp.Addressing == Opcode.AddressingType.ZEROPAGE_RELATIVE )
                {
                  return new GR.Generic.Tupel<Tiny64.Opcode, bool>( potentialOp, longMode );
                }
              }
            }
          }
        }

        if ( IsTrueOpeningBraceChar( LineTokens[1].Content ) )
        {
          startWithOpeningBrace = true;
          longMode = ( LineTokens[1].Content == AssemblerSettings.SQUARE_BRACKETS_OPEN );
          int tokenPos = 2;
          int numBracketCount = 1;
          int numBracketPairs = 0;
          expressionTokenStartIndex = 2;
          expressionTokenCount -= 2;

          endsWithClosingBrace = IsClosingBraceChar( LineTokens[expressionTokenStartIndex + expressionTokenCount].Content );

          while ( ( tokenPos < expressionTokenStartIndex + expressionTokenCount )
          &&      ( ( !IsMatchingBrace( LineTokens[1].Content, LineTokens[tokenPos].Content ) )
          ||        ( numBracketCount > 1 ) ) )
          {
            if ( IsClosingBraceChar( LineTokens[tokenPos].Content ) )
            {
              --numBracketCount;
            }
            else if ( IsOpeningBraceChar( LineTokens[tokenPos].Content ) )
            {
              ++numBracketCount;
              ++numBracketPairs;
            }
            if ( LineTokens[tokenPos].Content == "," )
            {
              twoParamsInBrackets = true;
              expressionTokenCount = tokenPos - expressionTokenStartIndex;
              if ( ( tokenPos + 2 < LineTokens.Count )
              &&   ( LineTokens[tokenPos + 1].Content.ToUpper() == "S" )
              &&   ( IsClosingBraceChar( LineTokens[tokenPos + 2].Content ) ) )
              {
                secondParamIsSP = true;
              }
              break;
            }
            ++tokenPos;
          }
          if ( !twoParamsInBrackets )
          {
            oneParamInBrackets = true;

            int numGivenBytes = 0;
            bool  couldEvaluate = EvaluateTokens( LineIndex, LineTokens, expressionTokenStartIndex, expressionTokenCount, info.LineCodeMapping, out SymbolInfo value, out numGivenBytes );
            if ( couldEvaluate )
            {
              if ( numGivenBytes > 0 )
              {
                numBytesFirstParam = numGivenBytes;
              }
              else if ( ( value.ToInteger() & 0xff00 ) != 0 )
              {
                numBytesFirstParam = 2;
              }
              else
              {
                numBytesFirstParam = 1;
              }
            }
            //expressionTokenCount = tokenPos - expressionTokenStartIndex;
          }
          if ( ( numBracketPairs > 1 )
          &&   ( tokenPos != expressionTokenCount )
          &&   ( !endsWithClosingBrace ) )
          {
            // no matching brackets
            oneParamInBrackets = false;
            twoParamsInBrackets = false;
          }
          
        }
        else
        {
          // an expression or identifier or address
          // TODO - expressions are built from one or several parts!
          if ( ( LineTokens.Count >= 2 )
          &&   ( LineTokens[LineTokens.Count - 1].Content.ToUpper() == "S" )
          &&   ( LineTokens[LineTokens.Count - 2].Content == "," ) )
          {
            secondParamIsSP = true;
            expressionTokenCount -= 2;
          }

          // special case LSR, ASL, ROR, ROL allow A as pseudo parameter
          if ( ( PossibleOpcodes[0].Mnemonic == "lsr" )
          ||   ( PossibleOpcodes[0].Mnemonic == "asl" )
          ||   ( PossibleOpcodes[0].Mnemonic == "ror" )
          ||   ( PossibleOpcodes[0].Mnemonic == "rol" ) )
          {
            if ( ( LineTokens.Count == 2 )
            &&   ( LineTokens[1].Content.ToUpper() == "A" ) )
            {
              // in this case the undecorated version wants to be used
              foreach ( Tiny64.Opcode opcode in PossibleOpcodes )
              {
                if ( opcode.Addressing == Tiny64.Opcode.AddressingType.IMPLICIT )
                {
                  LineTokens.RemoveAt( 1 );
                  return new GR.Generic.Tupel<Tiny64.Opcode, bool>( opcode, longMode );
                }
              }
            }
          }


          long value = -1;
          int numGivenBytes = 0;

          if ( ( m_AssemblerSettings.GreaterOrLessThanAtBeginningAffectFullExpression )
          &&   ( LineTokens.Count >= 2 )
          &&   ( LineTokens[1].Content == "#" )
          &&   ( ( LineTokens[2].Content == ">" )
          ||     ( LineTokens[2].Content == "<" ) ) )
          {
            // since it's hi/lo it's always 1
            numBytesFirstParam = 1;
            // still call evaluate tokens since it will collapse the result
            bool couldEvaluate = EvaluateTokens( LineIndex, LineTokens, expressionTokenStartIndex + 2, expressionTokenCount - 2, info.LineCodeMapping, out SymbolInfo valueSymbol, out numGivenBytes );
            if ( couldEvaluate )
            {
              value = valueSymbol.ToInteger();
              if ( LineTokens[2].Content == ">" )
              {
                LineTokens.RemoveRange( expressionTokenStartIndex + 2, expressionTokenCount - 2 );
                LineTokens[2].Content = ( ( value >> 8 ) & 0xff ).ToString();
                LineTokens[2].Type    = TokenInfo.TokenType.LITERAL_NUMBER;
              }
              else
              {
                LineTokens.RemoveRange( expressionTokenStartIndex + 2, expressionTokenCount - 2 );
                LineTokens[2].Content = ( value & 0xff ).ToString();
                LineTokens[2].Type = TokenInfo.TokenType.LITERAL_NUMBER;
              }
            }
          }
          else
          {
            // determine addressing from parameter size
            bool  couldEvaluate = EvaluateTokens( LineIndex, LineTokens, expressionTokenStartIndex, expressionTokenCount, info.LineCodeMapping, out SymbolInfo valueSymbol, out numGivenBytes );
            if ( couldEvaluate )
            //if ( ParseValue( LineIndex, LineTokens[1].Content, out value, out numGivenBytes ) )
            {
              value = valueSymbol.ToInteger();
              if ( numGivenBytes > 0 )
              {
                numBytesFirstParam = numGivenBytes;
              }
              else if ( ( value & 0xff00 ) != 0 )
              {
                numBytesFirstParam = 2;
              }
              else if ( ( value & 0xff0000 ) != 0 )
              {
                numBytesFirstParam = 3;
              }
              else
              {
                numBytesFirstParam = 1;
              }
            }
            else
            {
              // guess opcode later
              // have to assume the worst
              //numBytesFirstParam = 2;
              // TODO!!!!
              //AddError( LineIndex, RetroDevStudio.Types.ErrorCode.E1000_SYNTAX_ERROR, "Could not evaluate tokens" );
              //return null;
            }
          }
          // force to 1 byte 
          if ( LineTokens[0].Type == RetroDevStudio.Types.TokenInfo.TokenType.OPCODE_FIXED_ZP )
          {
            numBytesFirstParam = 1;
          }
          else if ( LineTokens[0].Type == RetroDevStudio.Types.TokenInfo.TokenType.OPCODE_FIXED_NON_ZP )
          {
            numBytesFirstParam = 2;
          }
        }
      }
      Tiny64.Opcode.AddressingType addressing = Tiny64.Opcode.AddressingType.UNKNOWN;

      if ( PossibleOpcodes.Count == 1 )
      {
        addressing = PossibleOpcodes[0].Addressing;
      }
      if ( LineTokens.Count == 1 )
      {
        addressing = Tiny64.Opcode.AddressingType.IMPLICIT;
      }
      else if ( ( LineTokens.Count >= 2 )
      &&        ( LineTokens[1].Content.StartsWith( "#" ) ) )
      {
        addressing = Tiny64.Opcode.AddressingType.IMMEDIATE_ACCU;

        if ( LineTokens.Count > 2 )
        {
          List<Types.TokenInfo> extraTokens = new List<Types.TokenInfo>();

          if ( LineTokens[1].Length > 1 )
          {
            Types.TokenInfo token = new Types.TokenInfo();
            token.Content = LineTokens[1].Content.Substring( 1 );
            token.Length = token.Content.Length;
            token.Type = Types.TokenInfo.TokenType.UNKNOWN;
            extraTokens.Add( token );
          }
          if ( LineTokens.Count > 2 )
          {
            for ( int i = 2; i < LineTokens.Count; ++i )
            {
              extraTokens.Add( LineTokens[i] );
            }
          }
          if ( extraTokens.Count > 0 )
          {
            long     expressionResult = -1;
            if ( ( m_AssemblerSettings.GreaterOrLessThanAtBeginningAffectFullExpression )
            &&   ( ( extraTokens[0].Content == "<" )
            ||     ( extraTokens[0].Content == ">" ) ) )
            {
              if ( EvaluateTokens( LineIndex, extraTokens, 1, extraTokens.Count - 1, info.LineCodeMapping, out SymbolInfo expressionResultSymbol2 ) )
              {
                expressionResult = expressionResultSymbol2.ToInteger();
                if ( extraTokens[0].Content == "<" )
                {
                  expressionResult &= 0xff;
                }
                else
                {
                  expressionResult = ( expressionResult >> 8 ) & 0xff;
                }

                LineTokens.RemoveRange( 2, LineTokens.Count - 2 );
                if ( LineTokens[1].Length > 1 )
                {
                  LineTokens[1].Length = 1;
                  LineTokens[1].Content = LineTokens[1].Content.Substring( 0, 1 );
                }
                Types.TokenInfo token = new Types.TokenInfo();
                token.Content = expressionResult.ToString();
                token.Length = token.Content.Length;
                LineTokens.Add( token );
              }
              else
              {
                info.NeededParsedExpression = extraTokens;
              }
            }
            if ( EvaluateTokens( LineIndex, extraTokens, info.LineCodeMapping, out SymbolInfo expressionResultSymbol ) )
            {
              expressionResult = expressionResultSymbol.ToInteger();
              LineTokens.RemoveRange( 2, LineTokens.Count - 2 );
              if ( LineTokens[1].Length > 1 )
              {
                LineTokens[1].Length = 1;
                LineTokens[1].Content = LineTokens[1].Content.Substring( 0, 1 );
              }
              Types.TokenInfo token = new Types.TokenInfo();
              token.Content = expressionResult.ToString();
              token.Length = token.Content.Length;
              LineTokens.Add( token );
            }
            else
            {
              info.NeededParsedExpression = extraTokens;
            }
          }
        }
      }
      else if ( ( !oneParamInBrackets )
      &&        ( !twoParamsInBrackets ) )
      {
        if ( endsWithCommaX )
        {
          if ( numBytesFirstParam == 1 )
          {
            addressing = Tiny64.Opcode.AddressingType.ZEROPAGE_X;
          }
          else if ( numBytesFirstParam == 3 )
          {
            addressing = Tiny64.Opcode.AddressingType.ABSOLUTE_LONG_X;
          }
          else
          {
            addressing = Tiny64.Opcode.AddressingType.ABSOLUTE_X;
          }
        }
        else if ( endsWithCommaY )
        {
          if ( numBytesFirstParam == 1 )
          {
            addressing = Tiny64.Opcode.AddressingType.ZEROPAGE_Y;
          }
          else
          {
            addressing = Tiny64.Opcode.AddressingType.ABSOLUTE_Y;
          }
        }
        else if ( endsWithCommaZ )
        {
          if ( numBytesFirstParam == 1 )
          {
            addressing = Tiny64.Opcode.AddressingType.ZEROPAGE_INDIRECT_Z;
          }
        }
        else if ( secondParamIsSP )
        {
          if ( numBytesFirstParam == 1 )
          {
            addressing = Tiny64.Opcode.AddressingType.STACK_RELATIVE;
          }
        }
        else
        {
          if ( numBytesFirstParam == 1 )
          {
            // don't map relatiev to zeropage, even inside zero page
            if ( addressing != Opcode.AddressingType.RELATIVE )
            {
              addressing = Tiny64.Opcode.AddressingType.ZEROPAGE;
            }
          }
          else if ( numBytesFirstParam == 2 )
          {
            if ( ( PossibleOpcodes.Count > 1 )
            &&   ( addressing == Tiny64.Opcode.AddressingType.UNKNOWN ) )
            {
              addressing = Tiny64.Opcode.AddressingType.ABSOLUTE;
            }
          }
          else if ( numBytesFirstParam == 3 )
          {
            if ( ( PossibleOpcodes.Count > 1 )
            &&   ( addressing == Tiny64.Opcode.AddressingType.UNKNOWN ) )
            {
              if ( PossibleOpcodes.Any( op => op.Addressing == Opcode.AddressingType.ABSOLUTE_LONG ) )
              {
                addressing = Tiny64.Opcode.AddressingType.ABSOLUTE_LONG;
              }
              else
              {
                addressing = Tiny64.Opcode.AddressingType.ABSOLUTE;
              }
            }
          }
          else if ( addressing == Tiny64.Opcode.AddressingType.UNKNOWN )
          {
            addressing = Tiny64.Opcode.AddressingType.ABSOLUTE;
          }
        }
      }
      else if ( oneParamInBrackets )
      {
        if ( endsWithCommaX )
        {
          // no fallback anymore
          AddError( LineIndex, Types.ErrorCode.E1105_INVALID_OPCODE, "Opcode does not support indirect addressing with ,x outside (remove braces)" );
          HadError = true;
          return null;
        }
        else if ( endsWithCommaY )
        {
          if ( ( longMode )
          &&   ( PossibleOpcodes.Any( op => op.Addressing == Opcode.AddressingType.ZEROPAGE_INDIRECT_Y_LONG ) ) )
          {
            return new GR.Generic.Tupel<Tiny64.Opcode, bool>( PossibleOpcodes.First( op => op.Addressing == Opcode.AddressingType.ZEROPAGE_INDIRECT_Y_LONG ), longMode );
          }
          else
          {
            addressing = Tiny64.Opcode.AddressingType.ZEROPAGE_INDIRECT_Y;
          }
        }
        else if ( endsWithCommaZ )
        {
          addressing = Tiny64.Opcode.AddressingType.ZEROPAGE_INDIRECT_Z;
        }
        else if ( ( startWithOpeningBrace )
        &&        ( endsWithClosingBrace ) )
        {
          if ( longMode )
          {
            if ( PossibleOpcodes.Any( op => op.Addressing == Opcode.AddressingType.ABSOLUTE_INDIRECT_LONG ) )
            {
              addressing = Opcode.AddressingType.ABSOLUTE_INDIRECT_LONG;
            }
            else if ( PossibleOpcodes.Any( op => op.Addressing == Opcode.AddressingType.ZEROPAGE_INDIRECT_LONG ) )
            {
              addressing = Tiny64.Opcode.AddressingType.ZEROPAGE_INDIRECT_LONG;
            }
            else
            {
              // fallback to round brackets
              addressing = Tiny64.Opcode.AddressingType.INDIRECT;
            }
          }
          else
          {
            addressing = Tiny64.Opcode.AddressingType.INDIRECT;
          }
        }
        else
        {
          if ( numBytesFirstParam == 1 )
          {
            addressing = Tiny64.Opcode.AddressingType.ZEROPAGE;
          }
          else
          {
            addressing = Tiny64.Opcode.AddressingType.ABSOLUTE;
          }
        }
      }
      else if ( twoParamsInBrackets )
      {
        // could be ABSOLUTE_INDIRECT_X (WDC 65C02)
        foreach ( Tiny64.Opcode opcode in PossibleOpcodes )
        {
          if ( ( secondParamIsSP )
          &&   ( opcode.Addressing == Opcode.AddressingType.ZEROPAGE_INDIRECT_SP_Y )
          &&   ( endsWithCommaY ) )
          {
            return new GR.Generic.Tupel<Tiny64.Opcode, bool>( opcode, longMode );
          }
          else if ( ( secondParamIsSP )
          &&        ( opcode.Addressing == Opcode.AddressingType.ZEROPAGE_INDIRECT_SP ) )
          {
            return new GR.Generic.Tupel<Tiny64.Opcode, bool>( opcode, longMode );
          }
          if ( opcode.Addressing == Opcode.AddressingType.ABSOLUTE_INDIRECT_X )
          {
            return new GR.Generic.Tupel<Tiny64.Opcode, bool>( opcode, longMode );
          }
        }
        addressing = Tiny64.Opcode.AddressingType.ZEROPAGE_INDIRECT_X;
      }
      else
      {
        addressing = Tiny64.Opcode.AddressingType.IMPLICIT;
      }

      foreach ( Tiny64.Opcode opcode in PossibleOpcodes )
      {
        if ( ( addressing == Opcode.AddressingType.IMMEDIATE_ACCU )
        &&   ( ( opcode.Addressing == addressing )
        ||     ( opcode.Addressing == Opcode.AddressingType.IMMEDIATE_8BIT )
        ||     ( opcode.Addressing == Opcode.AddressingType.IMMEDIATE_REGISTER ) ) )
        {
          return new GR.Generic.Tupel<Tiny64.Opcode, bool>( opcode, longMode );
        }
        else if ( opcode.Addressing == addressing )
        {
          return new GR.Generic.Tupel<Tiny64.Opcode, bool>( opcode, longMode );
        }
      }
      if ( addressing == Tiny64.Opcode.AddressingType.ZEROPAGE_X )
      {
        // no zeropage kind found, try full
        addressing = Tiny64.Opcode.AddressingType.ABSOLUTE_X;
      }
      if ( addressing == Tiny64.Opcode.AddressingType.ZEROPAGE_Y )
      {
        // no zeropage kind found, try full
        addressing = Tiny64.Opcode.AddressingType.ABSOLUTE_Y;
      }
      if ( addressing == Tiny64.Opcode.AddressingType.ZEROPAGE )
      {
        // no zeropage kind found, try full
        addressing = Tiny64.Opcode.AddressingType.ABSOLUTE;
      }
      // psw
      if ( addressing == Opcode.AddressingType.IMMEDIATE_ACCU )
      {
        addressing = Opcode.AddressingType.IMMEDIATE_16BIT;
      }
      // was in braces, could be simple braces around
      if ( addressing == Tiny64.Opcode.AddressingType.INDIRECT )
      {
        // could be zeropage indirect
        foreach ( Tiny64.Opcode opcode in PossibleOpcodes )
        {
          if ( opcode.Addressing == Opcode.AddressingType.ZEROPAGE_INDIRECT )
          {
            addressing = Opcode.AddressingType.ZEROPAGE_INDIRECT;
            return new GR.Generic.Tupel<Tiny64.Opcode, bool>( opcode, longMode );
          }
        }

        // then it was regular braces, just evaluate as non indirect
        //addressing = Opcode.AddressingType.ZEROPAGE;
        AddError( LineIndex, Types.ErrorCode.E1105_INVALID_OPCODE, "Opcode does not support indirect addressing (remove braces)" );
        HadError = true;
        return null;
      }
      foreach ( Tiny64.Opcode opcode in PossibleOpcodes )
      {
        if ( opcode.Addressing == addressing )
        {
          return new GR.Generic.Tupel<Tiny64.Opcode, bool>( opcode, longMode );
        }
      }

      // not error if label?
      /*
      AddError( LineIndex,
          Types.ErrorCode.E1300_OPCODE_AMBIGIOUS,
          "Could not determine correct opcode for " + LineTokens[0].Content,
          LineTokens[0].StartPos,
          LineTokens[0].Length );*/

      return null;
    }



    private bool MatchOpcodeToExpression( int LineIndex, List<Opcode> PossibleOpcodes, List<TokenInfo> LineTokens, out Opcode MatchingOpcode, out List<List<TokenInfo>> Expressions, out ulong ShiftedOpcodePatchValue )
    {
      MatchingOpcode          = null;
      ShiftedOpcodePatchValue = 0;
      Expressions             = null;

      int expressionTokenStartIndex = - 1;
      int expressionTokenCount = 0;

      if ( !ParseLineInParameters( LineTokens, 1, LineTokens.Count - 1, LineIndex, true, out List<List<TokenInfo>> lineParams ) )
      {
        return false;
      }
      // strip off comments
      foreach ( var parms in lineParams )
      {
        var tokenComment = parms.FirstOrDefault( t => t.Type == TokenInfo.TokenType.COMMENT );
        if ( tokenComment != null )
        {
          int   commentIndex = parms.IndexOf( tokenComment );
          parms.RemoveRange( commentIndex, parms.Count - commentIndex );
        }
      }

      foreach ( var potentialOpcode in PossibleOpcodes )
      {
        if ( potentialOpcode.ParserExpressions.Count > 0 )
        {
          if ( Expressions != null )
          {
            Expressions.Clear();
          }
          ShiftedOpcodePatchValue = 0;
          bool    isMatch = ( lineParams.Count == potentialOpcode.ParserExpressions.Count );

          int currentExpression = 0;
          int currentParam = 0;

          while ( ( currentParam < lineParams.Count )
          &&      ( currentExpression < potentialOpcode.ParserExpressions.Count )
          &&      ( isMatch ) )
          {
            var potentialExpression = potentialOpcode.ParserExpressions[currentExpression];
            var matchParam = lineParams[currentParam];

            switch ( potentialExpression.Type )
            {
              case Opcode.OpcodePartialExpression.EMPTY:
                isMatch = ( matchParam.Count == 0 );
                ++currentParam;
                ++currentExpression;
                break;
              case Opcode.OpcodePartialExpression.EXPRESSION_8BIT:
              case Opcode.OpcodePartialExpression.EXPRESSION_16BIT:
              case Opcode.OpcodePartialExpression.EXPRESSION_24BIT:
              case Opcode.OpcodePartialExpression.EXPRESSION_32BIT:
              case Opcode.OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE:
              case Opcode.OpcodePartialExpression.EXPRESSION_16BIT_RELATIVE:
                if ( matchParam.Count == 0 )
                {
                  isMatch = false;
                }
                else
                {
                  // TODO - verify size of param if possible, otherwise choose the highest bit length
                  expressionTokenStartIndex = LineTokens.IndexOf( matchParam[0] );
                  expressionTokenCount = matchParam.Count;

                  var newTokens = LineTokens.GetRange( expressionTokenStartIndex, expressionTokenCount );
                  if ( Expressions == null )
                  {
                    Expressions = new List<List<TokenInfo>>();
                  }
                  Expressions.Add( newTokens );
                }
                ++currentParam;
                ++currentExpression;
                break;
              case Opcode.OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST:
                if ( ( matchParam.Count < potentialExpression.ValidValues[0].ValidValues.Count )
                ||   ( matchParam.Count < potentialExpression.ValidValues[2].ValidValues.Count ) )
                {
                  isMatch = false;
                }
                else
                {
                  for ( int i = 0; i < potentialExpression.ValidValues[0].ValidValues.Count; ++i )
                  {
                    if ( string.Compare( matchParam[i].Content, potentialExpression.ValidValues[0].ValidValues[i].Key, true ) != 0 )
                    {
                      isMatch = false;
                      break;
                    }
                  }
                  if ( isMatch )
                  {
                    for ( int i = 0; i < potentialExpression.ValidValues[2].ValidValues.Count; ++i )
                    {
                      if ( string.Compare( matchParam[matchParam.Count - potentialExpression.ValidValues[2].ValidValues.Count + i].Content, potentialExpression.ValidValues[2].ValidValues[i].Key, true ) != 0 )
                      {
                        isMatch = false;
                        break;
                      }
                    }
                  }
                  if ( isMatch )
                  {
                    // everything else must now be the value from list
                    expressionTokenStartIndex = LineTokens.IndexOf( matchParam[potentialExpression.ValidValues[0].ValidValues.Count] );
                    expressionTokenCount      = matchParam.Count - potentialExpression.ValidValues[0].ValidValues.Count - potentialExpression.ValidValues[2].ValidValues.Count;

                    if ( expressionTokenCount != 1 )
                    {
                      isMatch = false;
                    }
                    else
                    {
                      var validValue = potentialExpression.ValidValues[1].ValidValues.FirstOrDefault( vv => string.Compare( vv.Key, LineTokens[expressionTokenStartIndex].Content, true ) == 0 );
                      if ( validValue == null )
                      {
                        isMatch = false;
                      }
                      else
                      {
                        ShiftedOpcodePatchValue |= validValue.ReplacementValue << potentialExpression.ReplacementValueShift;
                      }
                    }
                  }
                }
                ++currentParam;
                ++currentExpression;
                break;
              case Opcode.OpcodePartialExpression.COMPLEX:
                if ( potentialExpression.ValidValues[0].Expression != Opcode.OpcodePartialExpression.UNUSED )
                {
                  // match the rest from the back
                  for ( int i = 0; i < potentialExpression.ValidValues.Count - 1; ++i )
                  {
                    int matchIndex = matchParam.Count - 1 - i;
                    if ( matchIndex < 1 )
                    {
                      isMatch = false;
                      break;
                    }
                    var matchValues = potentialExpression.ValidValues[potentialExpression.ValidValues.Count - 1 - i];

                    var validValue = matchValues.ValidValues.FirstOrDefault( vv => string.Compare( vv.Key, matchParam[matchIndex].Content, true ) == 0 );
                    if ( validValue == null )
                    {
                      isMatch = false;
                      break;
                    }
                    else if ( validValue.ReplacementValue != ulong.MaxValue )
                    {
                      ShiftedOpcodePatchValue |= validValue.ReplacementValue << matchValues.ReplacementValueShift;
                    }
                  }
                  if ( isMatch )
                  {
                    // the rest is the expression
                    expressionTokenStartIndex = 0;
                    expressionTokenCount      = matchParam.Count - potentialExpression.ValidValues.Count + 1;

                    var newTokens = matchParam.GetRange( expressionTokenStartIndex, expressionTokenCount );
                    if ( Expressions == null )
                    {
                      Expressions = new List<List<TokenInfo>>();
                    }
                    Expressions.Add( newTokens );
                  }
                }
                else if ( potentialExpression.ValidValues.Last().Expression != Opcode.OpcodePartialExpression.UNUSED )
                {
                  // match the rest from the front
                  Debug.Log( "MatchOpcodeToExpression, Complex with expression at end not yet supported" );
                }
                else
                {
                  // no expression? match all
                  if ( matchParam.Count != potentialExpression.ValidValues.Count )
                  {
                    isMatch = false;
                  }
                  else
                  {
                    for ( int i = 0; i < matchParam.Count; ++i )
                    {
                      var matchValues = potentialExpression.ValidValues[i];

                      var validValue = matchValues.ValidValues.FirstOrDefault( vv => string.Compare( vv.Key, matchParam[i].Content, true ) == 0 );
                      if ( validValue == null )
                      {
                        isMatch = false;
                        break;
                      }
                      else if ( validValue.ReplacementValue != ulong.MaxValue )
                      {
                        ShiftedOpcodePatchValue |= validValue.ReplacementValue << matchValues.ReplacementValueShift;
                      }
                    }
                  }
                }
                ++currentParam;
                ++currentExpression;
                break;
              case Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT:
              case Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT:
              case Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_24BIT:
              case Opcode.OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT:
                if ( ( matchParam.Count < potentialExpression.ValidValues[0].ValidValues.Count )
                ||   ( matchParam.Count < potentialExpression.ValidValues.Last().ValidValues.Count ) )
                {
                  isMatch = false;
                }
                else
                {
                  for ( int i = 0; i < potentialExpression.ValidValues[0].ValidValues.Count; ++i )
                  {
                    if ( string.Compare( matchParam[i].Content, potentialExpression.ValidValues[0].ValidValues[i].Key, true ) != 0 )
                    {
                      isMatch = false;
                      break;
                    }
                  }
                  if ( isMatch )
                  {
                    for ( int i = 0; i < potentialExpression.ValidValues.Last().ValidValues.Count; ++i )
                    {
                      if ( string.Compare( matchParam[matchParam.Count - potentialExpression.ValidValues.Last().ValidValues.Count + i].Content, potentialExpression.ValidValues.Last().ValidValues[i].Key, true ) != 0 )
                      {
                        isMatch = false;
                        break;
                      }
                    }
                    if ( isMatch )
                    {
                      expressionTokenStartIndex = LineTokens.IndexOf( matchParam[potentialExpression.ValidValues[0].ValidValues.Count] );
                      expressionTokenCount      = matchParam.Count - potentialExpression.ValidValues[0].ValidValues.Count - potentialExpression.ValidValues.Last().ValidValues.Count;

                      var newTokens = LineTokens.GetRange( expressionTokenStartIndex, expressionTokenCount );
                      if ( Expressions == null )
                      {
                        Expressions = new List<List<TokenInfo>>();
                      }
                      Expressions.Add( newTokens );
                    }
                  }
                }
                ++currentParam;
                ++currentExpression;
                break;
              case Opcode.OpcodePartialExpression.TOKEN_LIST:
                if ( matchParam.Count != potentialExpression.ValidValues[0].ValidValues.Count )
                {
                  isMatch = false;
                }
                else
                {
                  for ( int i = 0; i < matchParam.Count; ++i )
                  {
                    if ( string.Compare( matchParam[i].Content, potentialExpression.ValidValues[0].ValidValues[i].Key, true ) != 0 )
                    {
                      isMatch = false;
                      break;
                    }
                  }
                }
                ++currentParam;
                ++currentExpression;
                break;
              case Opcode.OpcodePartialExpression.VALUE_FROM_LIST:
                if ( matchParam.Count != potentialExpression.ValidValues.Count )
                {
                  isMatch = false;
                }
                else
                {
                  foreach ( var matchTest in potentialExpression.ValidValues )
                  {
                    var validValue = matchTest.ValidValues.FirstOrDefault( vv => string.Compare( vv.Key, matchParam[0].Content, true ) == 0 );
                    if ( validValue == null )
                    {
                      isMatch = false;
                    }
                    else
                    {
                      ShiftedOpcodePatchValue |= validValue.ReplacementValue << potentialExpression.ReplacementValueShift;
                    }
                  }
                }
                ++currentParam;
                ++currentExpression;
                break;
              default:
                Debug.Log( $"Not implemented {potentialExpression.Type}!" );
                return false;
            }
          }
          if ( ( currentParam < lineParams.Count )
          ||   ( currentExpression < potentialOpcode.ParserExpressions.Count ) )
          {
            // both lists did not match up
            isMatch = false;
            // special case - no arguments (but we need to have parserexpressions set)
            if ( ( lineParams.Count == 0 )
            &&   ( potentialOpcode.ParserExpressions.Count == 1 )
            &&   ( potentialOpcode.ParserExpressions[0].Type == Opcode.OpcodePartialExpression.EMPTY ) )
            {
              isMatch = true;
            }
          }

          if ( isMatch )
          {
            if ( MatchingOpcode != null )
            {
              // more than one match
              return false;
            }
            MatchingOpcode = potentialOpcode;
            return true;
          }
        }
        else
        {
          // direct match, but must have no further tokens
          // Only for expression CPUs?
          if ( ( m_Processor.Name == "Motorola 68000" )
          ||   ( m_Processor.Name == "Z80" ) )
          {
            if ( ( lineParams.Count == 1 )
            &&   ( lineParams[0].Count == 0 ) )
            {
              MatchingOpcode = potentialOpcode;
              return true;
            }
          }
        }
      }
      if ( MatchingOpcode == null )
      {
        return false;
      }
      // cut out expression!
      if ( expressionTokenCount != 0 )
      {
        /*
        var newTokens = new List<TokenInfo>();
        if ( expressionTokenStartIndex > 0 )
        {
          newTokens.AddRange( LineTokens.GetRange( 0, expressionTokenStartIndex ) );
        }
        if ( expressionTokenStartIndex + expressionTokenCount < LineTokens.Count )
        {
          newTokens.AddRange( LineTokens.GetRange( expressionTokenStartIndex + expressionTokenCount, LineTokens.Count - expressionTokenStartIndex - expressionTokenCount ) );
        }*/
        var newTokens = LineTokens.GetRange( expressionTokenStartIndex, expressionTokenCount );

        LineTokens.Clear();
        LineTokens.AddRange( newTokens );
      }
      else
      {
        LineTokens.Clear();
      }
      return true;
    }



    private void ReplaceToken( ref List<string> Tokens, string TokenOld, string TokenNew )
    {
      for ( int i = 0; i < Tokens.Count; ++i )
      {
        if ( Tokens[i] == TokenOld )
        {
          Tokens[i] = TokenNew;
        }
      }
    }



    internal string TokensToExpression( List<Types.TokenInfo> Tokens )
    {
      return TokensToExpression( Tokens, 0, Tokens.Count );
    }



    private string TokensToExpression( List<Types.TokenInfo> Tokens, int StartIndex, int Count )
    {
      StringBuilder sb = new StringBuilder();

      for ( int i = 0; i < Count; ++i )
      {
        sb.Append( Tokens[StartIndex + i].Content );
        // BOO - HISS!
        if ( Tokens[StartIndex + i].Type == TokenInfo.TokenType.OPCODE_FIXED_NON_ZP )
        {
          sb.Append( "+2" );
        }
        else if ( Tokens[StartIndex + i].Type == TokenInfo.TokenType.OPCODE_FIXED_ZP )
        {
          sb.Append( "+1" );
        }

        if ( ( i + 1 < Count )
        &&   ( Tokens[StartIndex + i].StartPos + Tokens[StartIndex + i].Length < Tokens[StartIndex + i + 1].StartPos ) )
        {
          // requires spaces
          int numSpaces = Tokens[StartIndex + i + 1].StartPos - ( Tokens[StartIndex + i].StartPos + Tokens[StartIndex + i].Length );
          for ( int j = 0; j < numSpaces; ++j )
          {
            sb.Append( ' ' );
          }
        }
      }
      return sb.ToString();
    }



    private string EvaluateAsText( int lineIndex, List<Types.TokenInfo> Tokens, int StartIndex, int Count, GR.Collections.Map<byte, byte> TextCodeMapping )
    {
      StringBuilder sb = new StringBuilder();

      int     startTokenIndex = StartIndex;

      for ( int i = 0; i < Count; ++i )
      {
        Types.TokenInfo   token = Tokens[StartIndex + i];

        if ( token.Content == "," )
        {
          if ( StartIndex + i > startTokenIndex )
          {
            if ( ( sb.Length > 0 )
            &&   ( m_AssemblerSettings.MessageAutoIncludesBlanksBetweenParameters ) )
            {
              sb.Append( ' ' );
            }
            sb.Append( EvaluateAsText( lineIndex, Tokens, startTokenIndex, StartIndex + i - startTokenIndex, TextCodeMapping ) );
          }
          startTokenIndex = StartIndex + i + 1;
          continue;
        }
      }
      if ( startTokenIndex < StartIndex + Count )
      {
        // something left to do
        if ( ( sb.Length > 0 )
        &&   ( m_AssemblerSettings.MessageAutoIncludesBlanksBetweenParameters ) )
        {
          sb.Append( ' ' );
        }

        Types.TokenInfo   token = Tokens[startTokenIndex];
        if ( ( StartIndex + Count - startTokenIndex == 1 )
        &&   ( token.Type == RetroDevStudio.Types.TokenInfo.TokenType.LITERAL_STRING ) )
        {
          sb.Append( token.Content.Substring( 1, token.Content.Length - 2 ) );
        }
        else
        {
          SymbolInfo  result;

          if ( ( StartIndex + Count - startTokenIndex >= 3 )
          &&   ( Tokens[startTokenIndex].Content == "[" )
          &&   ( Tokens[startTokenIndex + Count - 2].Content == "]" )
          &&   ( Tokens[startTokenIndex + Count - 1].Content == "D" ) )
          {
            // DASM special, evaluate and enter as string
            if ( !EvaluateTokens( lineIndex, Tokens, startTokenIndex + 1, StartIndex + Count - startTokenIndex - 3, TextCodeMapping, out result ) )
            {
              return "";
            }
          }
          else if ( !EvaluateTokens( lineIndex, Tokens, startTokenIndex, StartIndex + Count - startTokenIndex, TextCodeMapping, out result ) )
          {
            // treat as empty string (e.g. undefined symbol)
            result = new SymbolInfo();
          }
          if ( result.IsInteger() )
          {
            sb.Append( $"{result.ToString()}/$" + result.AddressOrValue.ToString( "X" ) );
          }
          else
          {
            sb.Append( result.ToString() );
          }
        }
      }
      return sb.ToString();
    }



    public void DumpLabels()
    {
      foreach ( string label in m_ASMFileInfo.Labels.Keys )
      {
        Debug.Log( "Label " + label + " = " + m_ASMFileInfo.Labels[label].AddressOrValue.ToString( "x" ) );
      }
    }



    public void DumpMacros()
    {
      foreach ( var macro in m_ASMFileInfo.Macros )
      {
        Debug.Log( $"Macro {macro.Key.first} in line {macro.Key.second}" );
      }
    }



    public string MacroByType( Types.MacroInfo.PseudoOpType Type )
    {
      foreach ( var macro in m_AssemblerSettings.PseudoOps )
      {
        if ( macro.Value.Type == Type )
        {
          return macro.Key;
        }
      }
      return "";
    }



    public new ParseMessage AddWarning( int Line, Types.ErrorCode Code, string Text, int CharIndex, int Length )
    {
      if ( m_WarningsToIgnore.Contains( Code ) )
      {
        return null;
      }
      return base.AddWarning( Line, Code, Text, CharIndex, Length );
    }



    public new ParseMessage AddSevereWarning( int Line, Types.ErrorCode Code, string Text )
    {
      if ( m_WarningsToIgnore.Contains( Code ) )
      {
        return null;
      }
      return base.AddSevereWarning( Line, Code, Text );
    }



  }
}
