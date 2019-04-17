using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using C64Studio.Types;
using GR.Collections;
using GR.Memory;
using Tiny64;



namespace C64Studio.Parser
{
  public class ASMFileParser : ParserBase
  {
    private enum ParseLineResult
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
      public int                           LineIndex = 0;
      public int                           Pos = -1;
      public int                           Length = 0;
      public C64Studio.Types.ErrorCode     Code = C64Studio.Types.ErrorCode.OK;


      public ErrorInfo()
      {
      }

      public ErrorInfo( int LineIndex, int Pos, int Length, C64Studio.Types.ErrorCode Code )
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



    public delegate List<Types.TokenInfo> ExtFunction( List<Types.TokenInfo> Arguments );

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

    private GR.Collections.Map<string,int>                      m_OperatorPrecedence = new GR.Collections.Map<string,int>();

    private GR.Collections.Map<string, GR.Collections.Set<string>> m_LoadedFiles = new GR.Collections.Map<string, GR.Collections.Set<string>>();

    private int                         m_CompileCurrentAddress = -1;

    public GR.Collections.Set<string>   ExternallyIncludedFiles = new GR.Collections.Set<string>();

    private AssemblerSettings           m_AssemblerSettings = new AssemblerSettings();

    private List<Types.ErrorCode>       m_WarningsToIgnore = new List<C64Studio.Types.ErrorCode>();

    private StringBuilder               m_CurrentCommentSB = new StringBuilder();

    private GR.Collections.Map<byte, byte> m_TextCodeMappingScr = new GR.Collections.Map<byte, byte>();
    private GR.Collections.Map<byte, byte> m_TextCodeMappingPet = new GR.Collections.Map<byte, byte>();
    private GR.Collections.Map<byte, byte> m_TextCodeMappingRaw = new GR.Collections.Map<byte, byte>();

    private GR.Collections.Map<string,ExtFunctionInfo>    m_ExtFunctions = new GR.Collections.Map<string, ExtFunctionInfo>();

    private CompileConfig               m_CompileConfig = null;

    public Types.ASM.FileInfo           ASMFileInfo = new C64Studio.Types.ASM.FileInfo();

    public Types.ASM.FileInfo           InitialFileInfo = null;

    private ErrorInfo                   m_LastErrorInfo = new ErrorInfo();

    private bool                        DoLogSourceInfo = false;

    private string                      m_CurrentZoneName = "";



    public ASMFileParser()
    {
      m_OperatorPrecedence["-"] = 0;
      m_OperatorPrecedence["+"] = 0;
      m_OperatorPrecedence["/"] = 1;
      m_OperatorPrecedence["*"] = 1;
      m_OperatorPrecedence["%"] = 2;
      m_OperatorPrecedence["EOR"] = 3;
      m_OperatorPrecedence["eor"] = 3;
      m_OperatorPrecedence["XOR"] = 3;
      m_OperatorPrecedence["xor"] = 3;
      m_OperatorPrecedence["^"] = 6;
      m_OperatorPrecedence["OR"] = 4;
      m_OperatorPrecedence["or"] = 4;
      m_OperatorPrecedence["|"] = 4;
      m_OperatorPrecedence["AND"] = 5;
      m_OperatorPrecedence["and"] = 5;
      m_OperatorPrecedence["&"] = 5;
      m_OperatorPrecedence[">>"] = 6;
      m_OperatorPrecedence["<<"] = 6;
      m_OperatorPrecedence["<>"] = 6;
      m_OperatorPrecedence[">="] = 6;
      m_OperatorPrecedence["<="] = 6;
      m_OperatorPrecedence["!="] = 6;
      m_OperatorPrecedence["="] = 6;
      m_OperatorPrecedence[">"] = 7;
      m_OperatorPrecedence["<"] = 7;
      m_OperatorPrecedence["!"] = 7;
      m_OperatorPrecedence["~"] = 7;

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

      SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );
    }



    public void SetAssemblerType( C64Studio.Types.AssemblerType Type )
    {
      m_AssemblerSettings.SetAssemblerType( Type );
      if ( ASMFileInfo.AssemblerSettings == null )
      {
        ASMFileInfo.AssemblerSettings = new AssemblerSettings();
        ASMFileInfo.AssemblerSettings.SetAssemblerType( Type );
      }
    }



    private void SourceInfoLog( string Message )
    {
      if ( DoLogSourceInfo )
      {
        Debug.Log( Message );
      }
    }



    public void AddExtFunction( string Name, int NumArguments, int NumResults, ExtFunction Function )
    {
      ExtFunctionInfo   fInfo = new ExtFunctionInfo();
      fInfo.Name = Name;
      fInfo.NumArguments = NumArguments;
      fInfo.NumResults = NumResults;
      fInfo.Function = Function;

      m_ExtFunctions[Name] = fInfo;
    }



    private List<Types.TokenInfo> ExtFileSize( List<Types.TokenInfo> Arguments )
    {
      List<Types.TokenInfo>     result = new List<Types.TokenInfo>();

      long                      fileSize = 0;

      try
      {
        string    filenameToUse = Arguments[0].Content;

        if ( !System.IO.Path.IsPathRooted( filenameToUse ) )
        {
          filenameToUse = GR.Path.Append( m_DocBasePath, filenameToUse );
        }
        System.IO.FileInfo f = new System.IO.FileInfo( filenameToUse );
        fileSize = f.Length;
      }
      catch ( Exception )
      {
      }
      Types.TokenInfo   fileSizeToken = new C64Studio.Types.TokenInfo();
      fileSizeToken.Type = C64Studio.Types.TokenInfo.TokenType.LITERAL_NUMBER;
      fileSizeToken.Content = fileSize.ToString();
      result.Add( fileSizeToken );
      return result;
    }



    private List<Types.TokenInfo> ExtMathMin( List<Types.TokenInfo> Arguments )
    {
      List<Types.TokenInfo>     result = new List<Types.TokenInfo>();

      int     arg1 = GR.Convert.ToI32( Arguments[0].Content );
      int     arg2 = GR.Convert.ToI32( Arguments[1].Content );

      long    resultValue = Math.Min( arg1, arg2 );

      Types.TokenInfo   fileSizeToken = new C64Studio.Types.TokenInfo();
      fileSizeToken.Type = C64Studio.Types.TokenInfo.TokenType.LITERAL_NUMBER;
      fileSizeToken.Content = resultValue.ToString();
      result.Add( fileSizeToken );
      return result;
    }



    private List<Types.TokenInfo> ExtMathMax( List<Types.TokenInfo> Arguments )
    {
      List<Types.TokenInfo>     result = new List<Types.TokenInfo>();

      int     arg1 = GR.Convert.ToI32( Arguments[0].Content );
      int     arg2 = GR.Convert.ToI32( Arguments[1].Content );

      long    resultValue = Math.Max( arg1, arg2 );

      Types.TokenInfo   fileSizeToken = new C64Studio.Types.TokenInfo();
      fileSizeToken.Type = C64Studio.Types.TokenInfo.TokenType.LITERAL_NUMBER;
      fileSizeToken.Content = resultValue.ToString();
      result.Add( fileSizeToken );
      return result;
    }



    public Types.ASM.TemporaryLabelInfo AddTempLabel( string Name, int LineIndex, int LineCount, int Value, string Info )
    {
      return AddTempLabel( Name, LineIndex, LineCount, Value, Info, -1, 0 );
    }



    public Types.ASM.TemporaryLabelInfo AddTempLabel( string Name, int LineIndex, int LineCount, int Value, string Info, int CharIndex, int Length )
    {
      foreach ( Types.ASM.TemporaryLabelInfo oldTempInfo in ASMFileInfo.TempLabelInfo )
      {
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
            if ( ASMFileInfo.FindTrueLineSource( oldTempInfo.LineIndex, out filename, out localLine ) )
            {
              msg.AddMessage( "  already defined in " + filename + "(" + ( localLine + 1 ) + ")", filename, localLine, oldTempInfo.CharIndex, oldTempInfo.Length );
              //AddError( LineIndex, Types.ErrorCode.E1200_REDEFINITION_OF_LABEL, "  already defined in " + filename + "(" + ( localLine + 1 ) + ")", oldTempInfo.CharIndex, oldTempInfo.Length );
            }

            if ( Name == "__hla_STACK0" )
            {
              foreach ( var tempInfo2 in ASMFileInfo.TempLabelInfo )
              {
                if ( tempInfo2.Name == Name )
                {
                  Debug.Log( "Label " + Name + " already defined with value " + tempInfo2.Value + " for line " + ( tempInfo2.LineIndex + 1 ) + " to " + ( tempInfo2.LineIndex + 1 + tempInfo2.LineCount - 1 ) );
                }
              }
              Debug.Log( "During AddTempLabel for " + Name + " with value " + Value + " for line " + ( LineIndex + 1 ) + " to " + ( LineIndex + 1 + LineCount - 1 ) );
            }
            return null;
          }
        }
      }

      Types.ASM.TemporaryLabelInfo tempInfo = new C64Studio.Types.ASM.TemporaryLabelInfo();

      tempInfo.Name = Name;
      tempInfo.LineIndex  = LineIndex;
      tempInfo.LineCount  = LineCount;
      tempInfo.Value      = Value;
      tempInfo.Info       = Info;
      tempInfo.CharIndex  = CharIndex;
      tempInfo.Length     = Length;

      ASMFileInfo.TempLabelInfo.Add( tempInfo );

      //Debug.Log( "Add Temp Label " + Name + " at " + LineIndex );

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
      List<Types.ASM.TemporaryLabelInfo>    infosToAdd = new List<C64Studio.Types.ASM.TemporaryLabelInfo>();

      foreach ( Types.ASM.TemporaryLabelInfo oldTempInfo in ASMFileInfo.TempLabelInfo )
      {
        if ( ( oldTempInfo.LineCount != -1 )
        &&   ( oldTempInfo.LineIndex + oldTempInfo.LineCount > SourceIndex )
        &&   ( oldTempInfo.LineIndex <= SourceIndex + CopyLength )
        &&   ( oldTempInfo.Name != ExceptThisLabel ) )
        {
          // fully inside source scope
          // need to copy!
          Types.ASM.TemporaryLabelInfo tempInfo = new C64Studio.Types.ASM.TemporaryLabelInfo();

          tempInfo.Name       = oldTempInfo.Name;
          tempInfo.LineIndex  = oldTempInfo.LineIndex + TargetIndex - SourceIndex;
          tempInfo.LineCount  = oldTempInfo.LineCount;
          tempInfo.Value      = oldTempInfo.Value;
          tempInfo.Info       = oldTempInfo.Info;

          foreach ( var origTempInfo in ASMFileInfo.TempLabelInfo )
          {
            // we're cloning a label after a trailing label (label till the end) of the same name -> adjust line count!
            if ( ( origTempInfo.LineCount == -1 )
            &&   ( origTempInfo.Name == tempInfo.Name )
            &&   ( origTempInfo.LineIndex < tempInfo.LineIndex ) )
            {
              origTempInfo.LineCount = tempInfo.LineIndex - origTempInfo.LineIndex;
            }
          }
          infosToAdd.Add( tempInfo );
        }
      }

      ASMFileInfo.TempLabelInfo.AddRange( infosToAdd );
    }



    public Types.SymbolInfo AddLabel( string Name, int Value, int SourceLine, string Zone, int CharIndex, int Length )
    {
      string    filename;
      int       localIndex = 0;

      ASMFileInfo.FindTrueLineSource( SourceLine, out filename, out localIndex );

      if ( !ASMFileInfo.Labels.ContainsKey( Name ) )
      {
        Types.SymbolInfo token = new C64Studio.Types.SymbolInfo();
        token.Type            = Types.SymbolInfo.Types.LABEL;
        token.AddressOrValue  = Value;
        token.Name            = Name;
        token.LineIndex       = SourceLine;
        token.Zone            = Zone;
        token.DocumentFilename = filename;
        token.LocalLineIndex = localIndex;
        token.CharIndex       = CharIndex;
        token.Length          = Length;
        ASMFileInfo.Labels.Add( Name, token );
        return token;
      }
      if ( ASMFileInfo.Labels[Name].AddressOrValue != Value )
      {
        if ( Name != "*" )
        {
          var message = AddError( SourceLine, Types.ErrorCode.E1200_REDEFINITION_OF_LABEL, "Redefinition of label " + Name, CharIndex, Length );

          message.AddMessage( "  already defined in " + ASMFileInfo.Labels[Name].DocumentFilename + " at line " + ( ASMFileInfo.Labels[Name].LocalLineIndex + 1 ),
                              ASMFileInfo.Labels[Name].DocumentFilename,
                              ASMFileInfo.Labels[Name].LocalLineIndex,
                              ASMFileInfo.Labels[Name].CharIndex,
                              ASMFileInfo.Labels[Name].Length );
        }
      }
      ASMFileInfo.Labels[Name].AddressOrValue = Value;
      return ASMFileInfo.Labels[Name];
    }



    public void SetLabelValue( string Name, int Value )
    {
      if ( ASMFileInfo.Labels.ContainsKey( Name ) )
      {
        ASMFileInfo.Labels[Name].AddressOrValue = Value;
      }
    }



    public void RemoveLabel( string Name )
    {
      ASMFileInfo.Labels.Remove( Name );
    }



    public Types.SymbolInfo AddPreprocessorLabel( string Name, int Value, int SourceLine )
    {
      if ( ASMFileInfo.Labels.ContainsKey( Name ) )
      {
        AddError( SourceLine, Types.ErrorCode.E1201_REDEFINITION_OF_PREPROCESSOR_DEFINE, "Preprocessor define " + Name + " declared more than once" );
        return ASMFileInfo.Labels[Name];
      }
      Types.SymbolInfo token = AddLabel( Name, Value, SourceLine, "", -1, 0 );
      token.Type = C64Studio.Types.SymbolInfo.Types.PREPROCESSOR_LABEL;
      return token;
    }



    public void AddZone( string Name, int SourceLine, int CharIndex, int Length )
    {
      string  zoneFile;
      int     localLine;
      ASMFileInfo.FindTrueLineSource( SourceLine, out zoneFile, out localLine );

      if ( !ASMFileInfo.Zones.ContainsKey( Name ) )
      {
        Types.SymbolInfo token = new C64Studio.Types.SymbolInfo();
        token.Type      = Types.SymbolInfo.Types.ZONE;
        token.Name      = Name;
        token.LineIndex = SourceLine;
        token.Zone      = Name;
        token.CharIndex = CharIndex;
        token.Length    = Length;
        token.DocumentFilename = zoneFile;
        ASMFileInfo.Zones.Add( Name, token );
      }
      else
      {
        if ( ASMFileInfo.Zones[Name].LineIndex != SourceLine )
        {
          var msg = AddError( ASMFileInfo.Zones[Name].LineIndex, Types.ErrorCode.E1202_REDEFINITION_OF_ZONE, "Duplicate declaration of zone " + Name, CharIndex, Length );
          msg.AddMessage( "  already declared in file " + zoneFile + " at line " + ( localLine + 1 ), zoneFile, localLine );
        }
        ASMFileInfo.Zones[Name].LineIndex = SourceLine;
      }
    }



    public void AddConstantF( string Name, double Value, int SourceLine, string Info, int CharIndex, int Length )
    {
      string    filename = "";
      int       localIndex = -1;
      ASMFileInfo.FindTrueLineSource( SourceLine, out filename, out localIndex );

      if ( !ASMFileInfo.Labels.ContainsKey( Name ) )
      {
        Types.SymbolInfo token = new Types.SymbolInfo();
        token.Type = Types.SymbolInfo.Types.CONSTANT_F;
        token.RealValue = Value;
        token.Name = Name;
        token.LineIndex = SourceLine;
        token.Used = true;
        token.Info = Info;
        token.DocumentFilename = filename;
        token.LocalLineIndex = localIndex;

        ASMFileInfo.Labels.Add( Name, token );
      }
      else
      {
        ASMFileInfo.Labels[Name].RealValue = Value;
        ASMFileInfo.Labels[Name].Type = Types.SymbolInfo.Types.CONSTANT_F;
      }
    }



    public void AddConstant( string Name, int Value, int SourceLine, string Info, string Zone, int CharIndex, int Length )
    {
      string    filename = "";
      int       localIndex = -1;
      ASMFileInfo.FindTrueLineSource( SourceLine, out filename, out localIndex );

      // check if temp label exists
      foreach ( C64Studio.Types.ASM.TemporaryLabelInfo tempLabel in ASMFileInfo.TempLabelInfo )
      {
        if ( tempLabel.Name == Name )
        {
          AddTempLabel( Name, SourceLine, -1, Value, Info, CharIndex, Length );
          return;
        }
      }

      if ( !ASMFileInfo.Labels.ContainsKey( Name ) )
      {
        Types.SymbolInfo token = new Types.SymbolInfo();
        token.Type            = Types.SymbolInfo.Types.CONSTANT_2;
        token.AddressOrValue  = Value;
        token.Name            = Name;
        token.LineIndex       = SourceLine;
        token.Used            = true;
        token.Info            = Info;
        token.DocumentFilename = filename;
        token.LocalLineIndex  = localIndex;

        if ( Value < 256 )
        {
          token.Type = Types.SymbolInfo.Types.CONSTANT_1;
        }
        token.Zone = Zone;

        ASMFileInfo.Labels.Add( Name, token );
      }
      else
      {
        if ( ASMFileInfo.Labels[Name].AddressOrValue != Value )
        {
          if ( Name != "*" )
          {
            //Debug.Log( "add constant error" );

            // allow redefinition, turn into temp label
            C64Studio.Types.SymbolInfo  origLabel = ASMFileInfo.Labels[Name];

            ASMFileInfo.Labels.Remove( Name );

            // re-add orig as temp
            AddTempLabel( Name, origLabel.LineIndex, SourceLine - origLabel.LineIndex, origLabel.AddressOrValue, Info, CharIndex, Length );
            // add new label
            AddTempLabel( Name, SourceLine, -1, Value, Info, CharIndex, Length );
            return;
          }
        }
        ASMFileInfo.Labels[Name].AddressOrValue = Value;
      }
    }



    public void AddPreprocessorConstant( string Name, int Value, int SourceLine )
    {
      if ( !ASMFileInfo.Labels.ContainsKey( Name ) )
      {
        Types.SymbolInfo token = new Types.SymbolInfo();
        token.Type = Types.SymbolInfo.Types.PREPROCESSOR_CONSTANT_2;
        token.AddressOrValue = Value;
        token.Name = Name;
        token.LineIndex = SourceLine;
        token.Used = true;

        if ( Value < 256 )
        {
          token.Type = Types.SymbolInfo.Types.PREPROCESSOR_CONSTANT_1;
        }

        ASMFileInfo.Labels.Add( Name, token );
      }
      else
      {
        if ( ASMFileInfo.Labels[Name].AddressOrValue != Value )
        {
          if ( Name != "*" )
          {
            //Debug.Log( "add constant error" );
            AddError( SourceLine, Types.ErrorCode.E1203_REDEFINITION_OF_CONSTANT, "Redefinition of constant " + Name );
          }
        }
        ASMFileInfo.Labels[Name].AddressOrValue = Value;
      }
    }



    public Types.ASM.UnparsedEvalInfo AddUnparsedLabel( string Name, string Value, int SourceLine )
    {
      if ( !ASMFileInfo.UnparsedLabels.ContainsKey( Name ) )
      {
        Types.ASM.UnparsedEvalInfo evalInfo = new Types.ASM.UnparsedEvalInfo();
        evalInfo.Name       = Name;
        evalInfo.LineIndex  = SourceLine;
        evalInfo.ToEval     = Value;
        ASMFileInfo.UnparsedLabels.Add( Name, evalInfo );
        return evalInfo;
      }
      if ( ( String.IsNullOrEmpty( Value ) )
      &&   ( Name != "*" ) )
      {
        AddError( SourceLine, Types.ErrorCode.E1200_REDEFINITION_OF_LABEL, "Redefinition of label " + Name );
      }
      ASMFileInfo.UnparsedLabels[Name].ToEval = Value;
      return ASMFileInfo.UnparsedLabels[Name];
    }



    public bool ParseLiteralValue( string Value, out bool Failed, out int Result, out int NumGivenBytes )
    {
      Result = -1;
      NumGivenBytes = 0;
      Failed = false;

      if ( Value.StartsWith( "$" ) )
      {
        if ( int.TryParse( Value.Substring( 1 ), System.Globalization.NumberStyles.HexNumber, null, out Result ) )
        {
          NumGivenBytes = ( Value.Length - 1 + 1 ) / 2;
          return true;
        }
        Failed = true;
        return false;
      }
      else if ( Value.StartsWith( "0x" ) )
      {
        if ( int.TryParse( Value.Substring( 2 ), System.Globalization.NumberStyles.HexNumber, null, out Result ) )
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
      else if ( Value.StartsWith( "\"" ) )
      {
        if ( !Value.EndsWith( "\"" ) )
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

      if ( Value == "*" )
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

      int resultValue = 0;
      if ( int.TryParse( Value, out resultValue ) )
      {
        Result = resultValue;
        if ( Result >= 255 )
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

      int   dummyInt = -1;

      if ( !ParseLiteralValue( Value, out Failed, out dummyInt, out NumGivenBytes ) )
      {
        return false;
      }
      Result = (double)dummyInt;
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



    public bool ParseValue( int LineIndex, string Value, out int Result )
    {
      int  numDigits = 0;
      ClearErrorInfo();
      return ParseValue( LineIndex, Value, out Result, out numDigits );
    }



    public bool ParseValueNumeric( int LineIndex, string Value, out double Result )
    {
      ClearErrorInfo();

      if ( Util.StringToDouble( Value, out Result ) )
      {
        return true;
      }

      Result = -1;
      ClearErrorInfo();
      bool failed   = false;

      if ( ParseLiteralValueNumeric( Value, out failed, out Result ) )
      {
        return true;
      }
      if ( failed )
      {
        m_LastErrorInfo.Set( LineIndex, 0, Value.Length, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION );
        return false;
      }

      // check for temp labels
      foreach ( Types.ASM.TemporaryLabelInfo labelInfo in ASMFileInfo.TempLabelInfo )
      {
        if ( ( LineIndex >= labelInfo.LineIndex )
        &&   ( ( LineIndex < labelInfo.LineIndex + labelInfo.LineCount )
        ||     ( labelInfo.LineCount == -1 ) )
        &&   ( labelInfo.Name == Value ) )
        {
          Result = labelInfo.Value;
          return true;
        }
      }
      // parse labels
      if ( !ASMFileInfo.Labels.ContainsKey( Value ) )
      {
        if ( ( IsLocalLabel( Value ) )
        &&   ( !string.IsNullOrEmpty( m_CurrentZoneName ) ) )
        {
          // a local label inside a zone has the actual zone name in front!
          if ( ASMFileInfo.Labels.ContainsKey( m_CurrentZoneName + Value ) )
          {
            Result = ASMFileInfo.Labels[m_CurrentZoneName + Value].AddressOrValue;
            ASMFileInfo.Labels[m_CurrentZoneName + Value].Used = true;
            return true;
          }
        }
        if ( ASMFileInfo.UnparsedLabels.ContainsKey( Value ) )
        {
          ASMFileInfo.UnparsedLabels[Value].Used = true;
        }
        m_LastErrorInfo = new ErrorInfo( LineIndex, 0, Value.Length, Types.ErrorCode.E1010_UNKNOWN_LABEL );
        return false;
      }

      if ( ASMFileInfo.Labels[Value].Type == Types.SymbolInfo.Types.CONSTANT_F )
      {
        Result = ASMFileInfo.Labels[Value].RealValue;
      }
      else
      {
        Result = (double)ASMFileInfo.Labels[Value].AddressOrValue;
      }
      ASMFileInfo.Labels[Value].Used = true;
      return true;
    }



    public bool ParseValue( int LineIndex, string Value, out int Result, out int NumGivenBytes )
    {
      Result        = -1;
      NumGivenBytes = 0;
      ClearErrorInfo();
      bool failed   = false;

      if ( ParseLiteralValue( Value, out failed, out Result, out NumGivenBytes ) )
      {
        return true;
      }
      if ( failed )
      {
        m_LastErrorInfo.Set( LineIndex, 0, Value.Length, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION );
        return false;
      }

      // check for temp labels
      foreach ( Types.ASM.TemporaryLabelInfo labelInfo in ASMFileInfo.TempLabelInfo )
      {
        if ( ( LineIndex >= labelInfo.LineIndex )
        &&   ( ( LineIndex < labelInfo.LineIndex + labelInfo.LineCount )
        ||     ( labelInfo.LineCount == -1 ) )
        &&   ( labelInfo.Name == Value ) )
        {
          Result = labelInfo.Value;
          return true;
        }
      }
      // parse labels
      if ( !ASMFileInfo.Labels.ContainsKey( Value ) )
      {
        if ( ( IsLocalLabel( Value ) )
        &&   ( !string.IsNullOrEmpty( m_CurrentZoneName ) ) )
        {
          // a local label inside a zone has the actual zone name in front!
          if ( ASMFileInfo.Labels.ContainsKey( m_CurrentZoneName + Value ) )
          {
            Result = ASMFileInfo.Labels[m_CurrentZoneName + Value].AddressOrValue;
            ASMFileInfo.Labels[m_CurrentZoneName + Value].Used = true;
            return true;
          }
        }
        if ( ASMFileInfo.UnparsedLabels.ContainsKey( Value ) )
        {
          ASMFileInfo.UnparsedLabels[Value].Used = true;
        }
        m_LastErrorInfo = new ErrorInfo( LineIndex, 0, Value.Length, Types.ErrorCode.E1010_UNKNOWN_LABEL );
        return false;
      }

      Result = ASMFileInfo.Labels[Value].AddressOrValue;
      ASMFileInfo.Labels[Value].Used = true;
      return true;
    }



    private bool IsLocalLabel( string LabelName )
    {
      if ( string.IsNullOrEmpty( LabelName ) )
      {
        return false;
      }
      if ( ASMFileInfo.AssemblerSettings.AllowedTokenStartChars[Types.TokenInfo.TokenType.LABEL_LOCAL].IndexOf( LabelName[0] ) != -1 )
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



    private bool EvaluateLabel( int LineIndex, string LabelContent, out int Result )
    {
      ClearErrorInfo();

      List<Types.TokenInfo>  tokens = ParseTokenInfo( LabelContent, 0, LabelContent.Length );
      if ( m_LastErrorInfo.Code != Types.ErrorCode.OK )
      {
        Result = 0;
        return false;
      }

      //dh.Log( "Eval Label (" + LabelContent + ") = " + tokens.Count + " parts" );
      return EvaluateTokens( LineIndex, tokens, out Result );
    }



    private bool HandleOperator( int LineIndex, Types.TokenInfo OperatorToken, Types.TokenInfo Token1, Types.TokenInfo Token2, out int Result )
    {
      Result = -1;
      ClearErrorInfo();

      int token1 = -1;
      int token2 = -1;
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

      if ( opText == "*" )
      {
        Result = token1 * token2;
        return true;
      }
      else if ( opText == "/" )
      {
        if ( token2 == 0 )
        {
          return false;
        }
        Result = token1 / token2;
        return true;
      }
      else if ( opText == "%" )
      {
        if ( token2 == 0 )
        {
          return false;
        }
        Result = token1 % token2;
        return true;
      }
      else if ( opText == "+" )
      {
        Result = token1 + token2;
        return true;
      }
      else if ( opText == "-" )
      {
        Result = token1 - token2;
        return true;
      }
      else if ( ( opText == "&" )
      ||        ( opText == "AND" )
      ||        ( opText == "and" ) )
      {
        Result = token1 & token2;
        return true;
      }
      else if ( ( opText == "EOR" )
      ||        ( opText == "eor" )
      ||        ( opText == "XOR" )
      ||        ( opText == "xor" ) )
      {
        Result = token1 ^ token2;
        return true;
      }
      else if ( ( opText == "|" )
      ||        ( opText == "or" )
      ||        ( opText == "OR" ) )
      {
        Result = token1 | token2;
        return true;
      }
      else if ( opText == ">>" )
      {
        Result = token1 >> token2;
        return true;
      }
      else if ( opText == "^" )
      {
        Result = (int)Math.Pow( (int)token1, (int)token2 );
        return true;
      }
      else if ( opText == "<<" )
      {
        Result = token1 << token2;
        return true;
      }
      else if ( opText == "=" )
      {
        if ( token1 == token2 )
        {
          Result = 1;
        }
        else
        {
          Result = 0;
        }
        return true;
      }
      else if ( ( opText == "!=" )
      ||        ( opText == "<>" ) )
      {
        if ( token1 != token2 )
        {
          Result = 1;
        }
        else
        {
          Result = 0;
        }
        return true;
      }
      else if ( opText == ">" )
      {
        if ( token1 > token2 )
        {
          Result = 1;
        }
        else
        {
          Result = 0;
        }
        return true;
      }
      else if ( opText == "<" )
      {
        if ( token1 < token2 )
        {
          Result = 1;
        }
        else
        {
          Result = 0;
        }
        return true;
      }
      else if ( opText == ">=" )
      {
        if ( token1 >= token2 )
        {
          Result = 1;
        }
        else
        {
          Result = 0;
        }
        return true;
      }
      else if ( opText == "<=" )
      {
        if ( token1 <= token2 )
        {
          Result = 1;
        }
        else
        {
          Result = 0;
        }
        return true;
      }
      return false;
    }



    private bool HandleOperatorNumeric( int LineIndex, Types.TokenInfo OperatorToken, Types.TokenInfo Token1, Types.TokenInfo Token2, out double Result )
    {
      Result = 0;
      ClearErrorInfo();

      double token1 = 0;
      double token2 = 0;
      string    opText = OperatorToken.Content;

      if ( !ParseValueNumeric( LineIndex, Token1.Content, out token1 ) )
      {
        m_LastErrorInfo.Pos += Token1.StartPos;
        return false;
      }
      if ( !ParseValueNumeric( LineIndex, Token2.Content, out token2 ) )
      {
        m_LastErrorInfo.Pos += Token2.StartPos;
        return false;
      }

      if ( opText == "*" )
      {
        Result = token1 * token2;
        return true;
      }
      else if ( opText == "/" )
      {
        Result = token1 / token2;
        return true;
      }
      else if ( opText == "%" )
      {
        if ( token2 == 0 )
        {
          return false;
        }
        Result = token1 % token2;
        return true;
      }
      else if ( opText == "+" )
      {
        Result = token1 + token2;
        return true;
      }
      else if ( opText == "-" )
      {
        Result = token1 - token2;
        return true;
      }
      else if ( ( opText == "&" )
      || ( opText == "AND" )
      || ( opText == "and" ) )
      {
        Result = (int)token1 & (int)token2;
        return true;
      }
      else if ( ( opText == "EOR" )
      || ( opText == "eor" )
      || ( opText == "XOR" )
      || ( opText == "xor" ) )
      {
        Result = (int)token1 ^ (int)token2;
        return true;
      }
      else if ( ( opText == "|" )
      || ( opText == "or" )
      || ( opText == "OR" ) )
      {
        Result = (int)token1 | (int)token2;
        return true;
      }
      else if ( opText == ">>" )
      {
        Result = (int)token1 >> (int)token2;
        return true;
      }
      else if ( opText == "^" )
      {
        Result = Math.Pow( token1, token2 );
        return true;
      }
      else if ( opText == "<<" )
      {
        Result = (int)token1 << (int)token2;
        return true;
      }
      else if ( opText == "=" )
      {
        if ( token1 == token2 )
        {
          Result = 1;
        }
        else
        {
          Result = 0;
        }
        return true;
      }
      else if ( ( opText == "!=" )
      || ( opText == "<>" ) )
      {
        if ( token1 != token2 )
        {
          Result = 1;
        }
        else
        {
          Result = 0;
        }
        return true;
      }
      else if ( opText == ">" )
      {
        if ( token1 > token2 )
        {
          Result = 1;
        }
        else
        {
          Result = 0;
        }
        return true;
      }
      else if ( opText == "<" )
      {
        if ( token1 < token2 )
        {
          Result = 1;
        }
        else
        {
          Result = 0;
        }
        return true;
      }
      else if ( opText == ">=" )
      {
        if ( token1 >= token2 )
        {
          Result = 1;
        }
        else
        {
          Result = 0;
        }
        return true;
      }
      else if ( opText == "<=" )
      {
        if ( token1 <= token2 )
        {
          Result = 1;
        }
        else
        {
          Result = 0;
        }
        return true;
      }
      return false;
    }



    private List<Types.TokenInfo> ProcessExtFunction( string FunctionName, List<Types.TokenInfo> Tokens, int StartIndex, int Count )
    {
      ExtFunctionInfo   fInfo = m_ExtFunctions[FunctionName];

      // truncate not used args
      int     numArgs = Count;
      if ( Count > fInfo.NumArguments )
      {
        numArgs = fInfo.NumArguments;
      }

      List<Types.TokenInfo>     arguments = Tokens.GetRange( StartIndex, numArgs );
      while ( arguments.Count < fInfo.NumArguments )
      {
        Types.TokenInfo   emptyToken = new C64Studio.Types.TokenInfo();
        emptyToken.Content = "0";
        emptyToken.Type = C64Studio.Types.TokenInfo.TokenType.LITERAL_NUMBER;
        arguments.Add( emptyToken );
      }

      for ( int i = 0; i < arguments.Count; ++i )
      {
        Types.TokenInfo token = arguments[i];
        if ( token.Type == C64Studio.Types.TokenInfo.TokenType.LITERAL_STRING )
        {
          Types.TokenInfo  newToken = new C64Studio.Types.TokenInfo();
          newToken.OriginatingString = token.OriginatingString;
          newToken.StartPos = token.StartPos + 1;
          newToken.Length = token.Length - 2;

          arguments[i] = newToken;
        }
      }
      List<Types.TokenInfo>  results = fInfo.Function( arguments );
      while ( results.Count < fInfo.NumResults )
      {
        Types.TokenInfo   emptyToken = new C64Studio.Types.TokenInfo();
        emptyToken.Content = "0";
        emptyToken.Type = C64Studio.Types.TokenInfo.TokenType.LITERAL_NUMBER;
        results.Add( emptyToken );
      }
      if ( results.Count > fInfo.NumResults )
      {
        results.RemoveRange( fInfo.NumResults, results.Count - fInfo.NumResults );
      }
      return fInfo.Function( arguments );
    }



    private bool EvaluateExtFunction( string FunctionName, List<Types.TokenInfo> Tokens, int StartIndex, int Count, out int Result )
    {
      Result = 0;

      List<Types.TokenInfo> results = ProcessExtFunction( FunctionName, Tokens, StartIndex, Count );
      if ( results.Count != 1 )
      {
        return false;
      }
      Result = GR.Convert.ToI32( results[0].Content );
      return true;
    }



    private bool EvaluateExtFunctionNumeric( string FunctionName, List<Types.TokenInfo> Tokens, int StartIndex, int Count, out double Result )
    {
      Result = 0;

      List<Types.TokenInfo> results = ProcessExtFunction( FunctionName, Tokens, StartIndex, Count );
      if ( results.Count != 1 )
      {
        return false;
      }
      return Util.StringToDouble( results[0].Content, out Result );
    }



    public bool EvaluateTokens( int LineIndex, List<Types.TokenInfo> Tokens, out int Result )
    {
      int dummy = 0;
      ClearErrorInfo();
      return EvaluateTokens( LineIndex, Tokens, out Result, out dummy );
    }



    private bool EvaluateTokens( int LineIndex, List<Types.TokenInfo> Tokens, int StartIndex, int Count, out int Result )
    {
      int dummy = 0;
      ClearErrorInfo();
      return EvaluateTokens( LineIndex, Tokens, StartIndex, Count, out Result, out dummy );
    }



    private bool EvaluateTokens( int LineIndex, List<Types.TokenInfo> Tokens, out int Result, out int NumBytesGiven )
    {
      return EvaluateTokens( LineIndex, Tokens, 0, Tokens.Count, out Result, out NumBytesGiven );
    }



    public bool EvaluateTokensNumeric( int LineIndex, List<Types.TokenInfo> Tokens, out double Result )
    {
      ClearErrorInfo();
      return EvaluateTokensNumeric( LineIndex, Tokens, 0, Tokens.Count, out Result );
    }



    private bool EvaluateTokens( int LineIndex, List<Types.TokenInfo> Tokens, int StartIndex, int Count, out int Result, out int NumBytesGiven )
    {
      Result = -1;
      NumBytesGiven = 0;
      ClearErrorInfo();
      if ( Count == 0 )
      {
        return false;
      }

      int numBytesGiven = 0;
      int dummy = 0;

      if ( Count == 1 )
      {
        if ( !ParseValue( LineIndex, Tokens[StartIndex].Content, out Result, out NumBytesGiven ) )
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
          int     value = -1;

          // special case, try parsing as numeric directly to avoid plus/minus off by one edge case
          if ( Tokens[StartIndex].EndPos + 1 == Tokens[StartIndex + 1].StartPos )
          {
            if ( int.TryParse( Tokens[StartIndex].Content + Tokens[StartIndex + 1].Content, out value ) )
            {
              if ( ( value <= 0 )
              &&   ( value > -254 ) )
              {
                NumBytesGiven = 1;
              }
              else
              {
                NumBytesGiven = 2;
              }
              Result = value;
              return true;
            }
          }

          if ( EvaluateTokens( LineIndex, Tokens, StartIndex + 1, Count - 1, out value, out NumBytesGiven ) )
          {
            Result = -value;
            return true;
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
          return HandleOperator( LineIndex, tempTokenOperator, Tokens[StartIndex], tempToken, out Result );
        }
        else if ( Tokens[StartIndex].Content == "%" )
        {
          // a binary expression
          return ParseValue( LineIndex, TokensToExpression( Tokens, StartIndex, 2 ), out Result, out NumBytesGiven );
        }
        else if ( ( Tokens[StartIndex].Content == "!" )
        ||        ( Tokens[StartIndex].Content == "~" ) )
        {
          // binary not
          int     value = -1;

          if ( EvaluateTokens( LineIndex, Tokens, StartIndex + 1, Count - 1, out value, out NumBytesGiven ) )
          {
            if ( NumBytesGiven == 2 )
            {
              Result = 0xffff ^ value;
              return true;
            }
            Result = 0xff ^ value;
            return true;
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

        for ( int i = 0; i < Count; ++i )
        {
          if ( IsOpeningBraceChar( subTokenRange[i].Content ) )
          {
            bracketStartPos = i;
          }
          else if ( IsClosingBraceChar( subTokenRange[i].Content ) )
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

        if ( ( bracketStartPos != -1 )
        &&   ( bracketEndPos != -1 ) )
        {
          int     resultValue = -1;

          // could we have a function?
          if ( bracketStartPos > 0 )
          {
            string    possibleFunction = subTokenRange[bracketStartPos - 1].Content.ToLower();
            if ( m_ExtFunctions.ContainsKey( possibleFunction ) )
            {
              // handle function!
              if ( !EvaluateExtFunction( possibleFunction, subTokenRange, bracketStartPos + 1, bracketEndPos - bracketStartPos - 1, out resultValue ) )
              {
                return false;
              }
              int     startPosB = subTokenRange[bracketStartPos - 1].StartPos;
              subTokenRange.RemoveRange( bracketStartPos - 1, bracketEndPos - bracketStartPos + 2 );
              Count -= bracketEndPos - bracketStartPos + 2;

              Types.TokenInfo tokenResultF = new Types.TokenInfo();
              tokenResultF.Content = resultValue.ToString();
              tokenResultF.Type = Types.TokenInfo.TokenType.LITERAL_NUMBER;
              tokenResultF.StartPos = startPosB;

              subTokenRange.Insert( bracketStartPos - 1, tokenResultF );
              ++Count;
              evaluatedPart = true;
              continue;
            }
          }

          if ( !EvaluateTokens( LineIndex, subTokenRange, bracketStartPos + 1, bracketEndPos - bracketStartPos - 1, out resultValue, out numBytesGiven ) )
          {
            return false;
          }
          NumBytesGiven = Math.Max( numBytesGiven, NumBytesGiven );

          int     startPos = subTokenRange[bracketStartPos].StartPos;
          subTokenRange.RemoveRange( bracketStartPos, bracketEndPos - bracketStartPos + 1 );
          Count -= bracketEndPos - bracketStartPos + 1;

          Types.TokenInfo tokenResult = new Types.TokenInfo();
          tokenResult.Content = resultValue.ToString();
          tokenResult.Type = Types.TokenInfo.TokenType.LITERAL_NUMBER;
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
            foreach ( KeyValuePair<string,int> oper in m_OperatorPrecedence )
            {
              if ( subTokenRange[tokenIndex].Content == oper.Key )
              {
                if ( ( tokenIndex == 0 )
                &&   ( oper.Value != 7 ) )
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
            int result = -1;

            // check if we've got a hi/lo byte operator
            if ( highestPrecedence == 7 )
            {
              // must be directly connected
              // the token before must not be a evaluatable type
              //if ( ( subTokenRange[highestPrecedenceTokenIndex].StartPos + subTokenRange[highestPrecedenceTokenIndex].Length == subTokenRange[highestPrecedenceTokenIndex + 1].StartPos )
              if ( ( highestPrecedenceTokenIndex == 0 )
              ||     ( ( subTokenRange[highestPrecedenceTokenIndex - 1].Type != C64Studio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
              &&       ( subTokenRange[highestPrecedenceTokenIndex - 1].Type != C64Studio.Types.TokenInfo.TokenType.LABEL_INTERNAL )
              &&       ( subTokenRange[highestPrecedenceTokenIndex - 1].Type != C64Studio.Types.TokenInfo.TokenType.LABEL_LOCAL )
              &&       ( subTokenRange[highestPrecedenceTokenIndex - 1].Content != "*" )
              &&       ( subTokenRange[highestPrecedenceTokenIndex - 1].Type != C64Studio.Types.TokenInfo.TokenType.LITERAL_CHAR )
              &&       ( subTokenRange[highestPrecedenceTokenIndex - 1].Type != C64Studio.Types.TokenInfo.TokenType.LITERAL_NUMBER ) ) )
              {
                // eval hi/lo byte
                if ( subTokenRange[highestPrecedenceTokenIndex].Content == "<" )
                {
                  int     value = -1;
                  if ( EvaluateTokens( LineIndex, subTokenRange, highestPrecedenceTokenIndex + 1, 1, out value, out numBytesGiven ) )
                  {
                    NumBytesGiven = Math.Max( numBytesGiven, NumBytesGiven );

                    int resultValue = ( value & 0x00ff );

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
                  int value = -1;
                  if ( EvaluateTokens( LineIndex, subTokenRange, highestPrecedenceTokenIndex + 1, 1, out value, out numBytesGiven ) )
                  {
                    NumBytesGiven = Math.Max( numBytesGiven, NumBytesGiven );

                    int resultValue = ( value & 0xff00 ) >> 8;
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
                else if ( ( subTokenRange[highestPrecedenceTokenIndex].Content == "~" )
                ||        ( subTokenRange[highestPrecedenceTokenIndex].Content == "!" ) )
                {
                  int     value = -1;

                  if ( EvaluateTokens( LineIndex, subTokenRange, highestPrecedenceTokenIndex + 1, Count - highestPrecedenceTokenIndex - 1, out value, out NumBytesGiven ) )
                  {
                    if ( NumBytesGiven == 2 )
                    {
                      Result = 0xffff ^ value;
                    }
                    else
                    {
                      Result = 0xff ^ value;
                    }

                    subTokenRange.RemoveRange( highestPrecedenceTokenIndex, 2 );

                    Types.TokenInfo tokenResult = new Types.TokenInfo();
                    tokenResult.Content = Result.ToString();
                    tokenResult.Type = Types.TokenInfo.TokenType.LITERAL_NUMBER;
                    subTokenRange.Insert( highestPrecedenceTokenIndex, tokenResult );
                    evaluatedPart = true;
                    Count -= 1;
                    return true;
                  }
                  return false;
                }
              }
            }
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
              tokenResult.Content = result.ToString();
              tokenResult.Type = Types.TokenInfo.TokenType.LITERAL_NUMBER;
              tokenResult.StartPos = startPos;
              subTokenRange.Insert( highestPrecedenceTokenIndex - 1, tokenResult );
              evaluatedPart = true;
              Count -= 2;
            }
          }
        }
      }
      while ( evaluatedPart );

      if ( Count == 1 )
      {
        return ParseValue( LineIndex, subTokenRange[0].Content, out Result, out NumBytesGiven );
      }
      if ( !HasError() )
      {
        m_LastErrorInfo.Set( LineIndex, subTokenRange[0].StartPos, subTokenRange[subTokenRange.Count - 1].EndPos - subTokenRange[0].StartPos, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION );
      }
      return false;
    }



    private bool EvaluateTokensNumeric( int LineIndex, List<Types.TokenInfo> Tokens, int StartIndex, int Count, out double Result )
    {
      Result = 0;
      ClearErrorInfo();
      if ( Count == 0 )
      {
        return false;
      }

      int numBytesGiven = 0;
      double dummy = 0;

      if ( Count == 1 )
      {
        if ( !ParseValueNumeric( LineIndex, Tokens[StartIndex].Content, out dummy ) )
        {
          // adjust start pos
          // adjust length since we could have a replaced token (globalized local label)
          m_LastErrorInfo.Pos += Tokens[StartIndex].StartPos;
          m_LastErrorInfo.Length = Tokens[StartIndex].Length;
          return false;
        }
        Result = (double)dummy;
        return true;
      }
      if ( Count == 2 )
      {
        // unary operators
        if ( Tokens[StartIndex].Content == "-" )
        {
          double     value = -1;

          if ( EvaluateTokensNumeric( LineIndex, Tokens, StartIndex + 1, Count - 1, out value ) )
          {
            Result = -value;
            return true;
          }
        }
        else if ( Tokens[StartIndex + 1].Content.StartsWith( "%" ) )
        {
          // a "x % y" case, where % is prefixed to the second operator
          int     dummyInt = -1;
          if ( ParseValue( LineIndex, Tokens[StartIndex].Content, out dummyInt, out numBytesGiven ) )
          {
          }
          if ( ParseValue( LineIndex, Tokens[StartIndex + 1].Content.Substring( 1 ), out dummyInt, out numBytesGiven ) )
          {
          }
          Types.TokenInfo   tempTokenOperator = new Types.TokenInfo();
          tempTokenOperator.Content = "%";
          tempTokenOperator.StartPos = Tokens[StartIndex + 1].StartPos;
          tempTokenOperator.Length = 1;

          Types.TokenInfo   tempToken = new Types.TokenInfo();
          tempToken.Content = Tokens[StartIndex + 1].Content.Substring( 1 );
          tempToken.StartPos = Tokens[StartIndex + 1].StartPos + 1;
          tempToken.Length = Tokens[StartIndex + 1].Length - 1;

          int   resultInt = 0;
          if ( !HandleOperator( LineIndex, tempTokenOperator, Tokens[StartIndex], tempToken, out resultInt ) )
          {
            return false;
          }
          Result = (double)resultInt;
          return true;
        }
        else if ( Tokens[StartIndex].Content == "%" )
        {
          // a binary expression
          if ( !ParseValueNumeric( LineIndex, TokensToExpression( Tokens, StartIndex, 2 ), out dummy ) )
          {
            return false;
          }
          Result = (double)dummy;
          return true;
        }
        else if ( ( Tokens[StartIndex].Content == "!" )
        ||        ( Tokens[StartIndex].Content == "~" ) )
        {
          // binary not
          int     value = -1;

          if ( EvaluateTokens( LineIndex, Tokens, StartIndex + 1, Count - 1, out value ) )
          {
            Result = 0xffff ^ value;
            return true;
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

        for ( int i = 0; i < Count; ++i )
        {
          if ( IsOpeningBraceChar( subTokenRange[i].Content ) )
          {
            bracketStartPos = i;
          }
          else if ( IsClosingBraceChar( subTokenRange[i].Content ) )
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

        if ( ( bracketStartPos != -1 )
        &&   ( bracketEndPos != -1 ) )
        {
          double resultValue = 0;

          // could we have a function?
          if ( bracketStartPos > 0 )
          {
            string    possibleFunction = subTokenRange[bracketStartPos - 1].Content.ToLower();
            if ( m_ExtFunctions.ContainsKey( possibleFunction ) )
            {
              // handle function!
              if ( !EvaluateExtFunctionNumeric( possibleFunction, subTokenRange, bracketStartPos + 1, bracketEndPos - bracketStartPos - 1, out resultValue ) )
              {
                return false;
              }
              int     startPosB = subTokenRange[bracketStartPos - 1].StartPos;
              subTokenRange.RemoveRange( bracketStartPos - 1, bracketEndPos - bracketStartPos + 2 );
              Count -= bracketEndPos - bracketStartPos + 2;

              Types.TokenInfo tokenResultF = new Types.TokenInfo();
              tokenResultF.Content = resultValue.ToString( "0.00000000000000000000", System.Globalization.CultureInfo.InvariantCulture );
              tokenResultF.Type = Types.TokenInfo.TokenType.LITERAL_REAL_NUMBER;
              tokenResultF.StartPos = startPosB;

              subTokenRange.Insert( bracketStartPos - 1, tokenResultF );
              ++Count;
              evaluatedPart = true;
              continue;
            }
          }

          if ( !EvaluateTokensNumeric( LineIndex, subTokenRange, bracketStartPos + 1, bracketEndPos - bracketStartPos - 1, out resultValue ) )
          {
            return false;
          }

          int     startPos = subTokenRange[bracketStartPos].StartPos;
          subTokenRange.RemoveRange( bracketStartPos, bracketEndPos - bracketStartPos + 1 );
          Count -= bracketEndPos - bracketStartPos + 1;

          Types.TokenInfo tokenResult = new Types.TokenInfo();
          tokenResult.Content = Result.ToString( "0.00000000000000000000", System.Globalization.CultureInfo.InvariantCulture );
          tokenResult.Type = Types.TokenInfo.TokenType.LITERAL_REAL_NUMBER;
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
            foreach ( KeyValuePair<string, int> oper in m_OperatorPrecedence )
            {
              if ( subTokenRange[tokenIndex].Content == oper.Key )
              {
                if ( ( tokenIndex == 0 )
                && ( oper.Value != 7 ) )
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
            double result = 0;

            // check if we've got a hi/lo byte operator
            if ( highestPrecedence == 7 )
            {
              // must be directly connected
              // the token before must not be a evaluatable type
              //if ( ( subTokenRange[highestPrecedenceTokenIndex].StartPos + subTokenRange[highestPrecedenceTokenIndex].Length == subTokenRange[highestPrecedenceTokenIndex + 1].StartPos )
              if ( ( highestPrecedenceTokenIndex == 0 )
              ||   ( ( subTokenRange[highestPrecedenceTokenIndex - 1].Type != C64Studio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
              &&     ( subTokenRange[highestPrecedenceTokenIndex - 1].Type != C64Studio.Types.TokenInfo.TokenType.LABEL_INTERNAL )
              &&     ( subTokenRange[highestPrecedenceTokenIndex - 1].Type != C64Studio.Types.TokenInfo.TokenType.LABEL_LOCAL )
              &&     ( subTokenRange[highestPrecedenceTokenIndex - 1].Content != "*" )
              &&     ( subTokenRange[highestPrecedenceTokenIndex - 1].Type != C64Studio.Types.TokenInfo.TokenType.LITERAL_CHAR )
              &&     ( subTokenRange[highestPrecedenceTokenIndex - 1].Type != C64Studio.Types.TokenInfo.TokenType.LITERAL_NUMBER ) ) )
              {
                // eval hi/lo byte
                if ( subTokenRange[highestPrecedenceTokenIndex].Content == "<" )
                {
                  int     value = -1;
                  if ( EvaluateTokens( LineIndex, subTokenRange, highestPrecedenceTokenIndex + 1, 1, out value, out numBytesGiven ) )
                  {
                    int resultValue = ( value & 0x00ff );

                    subTokenRange.RemoveRange( highestPrecedenceTokenIndex, 2 );

                    Types.TokenInfo tokenResult = new Types.TokenInfo();
                    tokenResult.Content = Result.ToString( "0.00000000000000000000", System.Globalization.CultureInfo.InvariantCulture );
                    tokenResult.Type = Types.TokenInfo.TokenType.LITERAL_REAL_NUMBER;
                    subTokenRange.Insert( highestPrecedenceTokenIndex, tokenResult );
                    evaluatedPart = true;
                    Count -= 1;
                    continue;
                  }
                  return false;
                }
                else if ( subTokenRange[highestPrecedenceTokenIndex].Content == ">" )
                {
                  int value = -1;
                  if ( EvaluateTokens( LineIndex, subTokenRange, highestPrecedenceTokenIndex + 1, 1, out value, out numBytesGiven ) )
                  {
                    int resultValue = ( value & 0xff00 ) >> 8;
                    subTokenRange.RemoveRange( highestPrecedenceTokenIndex, 2 );

                    Types.TokenInfo tokenResult = new Types.TokenInfo();
                    tokenResult.Content = Result.ToString( "0.00000000000000000000", System.Globalization.CultureInfo.InvariantCulture );
                    tokenResult.Type = Types.TokenInfo.TokenType.LITERAL_REAL_NUMBER;
                    subTokenRange.Insert( highestPrecedenceTokenIndex, tokenResult );
                    evaluatedPart = true;
                    Count -= 1;
                    continue;
                  }
                  return false;
                }
                else if ( ( subTokenRange[highestPrecedenceTokenIndex].Content == "~" )
                || ( subTokenRange[highestPrecedenceTokenIndex].Content == "!" ) )
                {
                  int     value = -1;

                  if ( EvaluateTokens( LineIndex, subTokenRange, highestPrecedenceTokenIndex + 1, Count - highestPrecedenceTokenIndex - 1, out value ) )
                  {
                    Result = 0xffff ^ value;

                    subTokenRange.RemoveRange( highestPrecedenceTokenIndex, 2 );

                    Types.TokenInfo tokenResult = new Types.TokenInfo();
                    tokenResult.Content = Result.ToString( "0.00000000000000000000", System.Globalization.CultureInfo.InvariantCulture );
                    tokenResult.Type = Types.TokenInfo.TokenType.LITERAL_REAL_NUMBER;
                    subTokenRange.Insert( highestPrecedenceTokenIndex, tokenResult );
                    evaluatedPart = true;
                    Count -= 1;
                    return true;
                  }
                  return false;
                }
              }
            }
            if ( HandleOperatorNumeric( LineIndex, subTokenRange[highestPrecedenceTokenIndex], subTokenRange[highestPrecedenceTokenIndex - 1], subTokenRange[highestPrecedenceTokenIndex + 1], out result ) )
            {
              int dummyInt;
              if ( ParseValue( LineIndex, subTokenRange[highestPrecedenceTokenIndex - 1].Content, out dummyInt, out numBytesGiven ) )
              {
              }
              if ( ParseValue( LineIndex, subTokenRange[highestPrecedenceTokenIndex + 1].Content, out dummyInt, out numBytesGiven ) )
              {
              }

              int     startPos = subTokenRange[highestPrecedenceTokenIndex - 1].StartPos;
              subTokenRange.RemoveRange( highestPrecedenceTokenIndex - 1, 3 );

              Types.TokenInfo tokenResult = new Types.TokenInfo();
              tokenResult.Content = result.ToString( "0.00000000000000000000", System.Globalization.CultureInfo.InvariantCulture );
              tokenResult.Type = Types.TokenInfo.TokenType.LITERAL_REAL_NUMBER;
              tokenResult.StartPos = startPos;
              subTokenRange.Insert( highestPrecedenceTokenIndex - 1, tokenResult );
              evaluatedPart = true;
              Count -= 2;
            }
          }
        }
      }
      while ( evaluatedPart );

      if ( Count == 1 )
      {
        return ParseValueNumeric( LineIndex, subTokenRange[0].Content, out Result );
      }
      if ( !HasError() )
      {
        m_LastErrorInfo.Set( LineIndex, subTokenRange[0].StartPos, subTokenRange[subTokenRange.Count - 1].EndPos - subTokenRange[0].StartPos, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION );
      }
      return false;
    }



    private bool IsOpeningBraceChar( string Token )
    {
      if ( ( Token == "(" )
      ||   ( Token == AssemblerSettings.INTERNAL_OPENING_BRACE ) )
      {
        return true;
      }
      return false;
    }



    private bool IsClosingBraceChar( string Token )
    {
      if ( ( Token == ")" )
      ||   ( Token == AssemblerSettings.INTERNAL_CLOSING_BRACE ) )
      {
        return true;
      }
      return false;
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

      do
      {
        if ( lineInfo.NeededParsedExpression[tokenIndex].Content == "," )
        {
          // found an expression
          if ( tokenIndex - expressionStartIndex == 1 )
          {
            if ( ( lineInfo.NeededParsedExpression[expressionStartIndex].Content.StartsWith( "\"" ) )
            && ( lineInfo.NeededParsedExpression[expressionStartIndex].Length > 1 )
            && ( lineInfo.NeededParsedExpression[expressionStartIndex].Content.EndsWith( "\"" ) ) )
            {
              // a text
              foreach ( char aChar in lineInfo.NeededParsedExpression[expressionStartIndex].Content.Substring( 1, lineInfo.NeededParsedExpression[expressionStartIndex].Length - 2 ) )
              {
                // map to PETSCII!
                lineData.AppendU8( MapTextCharacter( lineInfo.LineCodeMapping, (byte)aChar ) );
              }
            }
            else
            {
              int value = -1;
              if ( !EvaluateTokens( lineIndex, lineInfo.NeededParsedExpression, expressionStartIndex, tokenIndex - expressionStartIndex, out value ) )
              {
                if ( AddErrors )
                {
                  AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Failed to evaluate " + TokensToExpression( lineInfo.NeededParsedExpression, expressionStartIndex, tokenIndex - expressionStartIndex ) );
                }
                return ParseLineResult.RETURN_FALSE;
              }
              lineData.AppendU8( (byte)value );
            }
          }
          else
          {
            int value = -1;
            if ( !EvaluateTokens( lineIndex, lineInfo.NeededParsedExpression, expressionStartIndex, tokenIndex - expressionStartIndex, out value ) )
            {
              if ( AddErrors )
              {
                AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Failed to evaluate " + TokensToExpression( lineInfo.NeededParsedExpression, expressionStartIndex, tokenIndex - expressionStartIndex ) );
              }
              return ParseLineResult.RETURN_FALSE;
            }
            lineData.AppendU8( (byte)value );
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
              && ( lineInfo.NeededParsedExpression[expressionStartIndex].Length > 1 )
              && ( lineInfo.NeededParsedExpression[expressionStartIndex].Content.EndsWith( "\"" ) ) )
              {
                // a text
                foreach ( char aChar in lineInfo.NeededParsedExpression[expressionStartIndex].Content.Substring( 1, lineInfo.NeededParsedExpression[expressionStartIndex].Length - 2 ) )
                {
                  // map to PETSCII!
                  lineData.AppendU8( MapTextCharacter( lineInfo.LineCodeMapping, (byte)aChar ) );
                }
              }
              else
              {
                int value = -1;
                if ( !EvaluateTokens( lineIndex, lineInfo.NeededParsedExpression, expressionStartIndex, tokenIndex - expressionStartIndex, out value ) )
                {
                  if ( AddErrors )
                  {
                    AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Failed to evaluate " + TokensToExpression( lineInfo.NeededParsedExpression, expressionStartIndex, tokenIndex - expressionStartIndex ) );
                  }
                  return ParseLineResult.RETURN_FALSE;
                }
                lineData.AppendU8( (byte)value );
              }
            }
            else
            {
              int value = -1;
              if ( !EvaluateTokens( lineIndex, lineInfo.NeededParsedExpression, expressionStartIndex, tokenIndex - expressionStartIndex, out value ) )
              {
                if ( AddErrors )
                {
                  AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Failed to evaluate " + TokensToExpression( lineInfo.NeededParsedExpression, expressionStartIndex, tokenIndex - expressionStartIndex ) );
                }
                return ParseLineResult.RETURN_FALSE;
              }
              lineData.AppendU8( (byte)value );
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
      foreach ( int lineIndex in ASMFileInfo.LineInfo.Keys )
      {
        Types.ASM.LineInfo lineInfo = ASMFileInfo.LineInfo[lineIndex];

        if ( m_CompileCurrentAddress == -1 )
        {
          if ( ( lineInfo.NumBytes == 0 )
          &&   ( lineInfo.AddressSource != "*" ) )
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
        if ( lineInfo.AddressSource == "*" )
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
        if ( ASMFileInfo.UnparsedLabels.Count > 0 )
        {
          foreach ( Types.ASM.UnparsedEvalInfo evalInfo in ASMFileInfo.UnparsedLabels.Values )
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
        foreach ( string label in ASMFileInfo.UnparsedLabels.Keys )
        {
          //dh.Log( "Unparsed label:" + label + ", " + m_UnparsedLabels[label].ToEval );
          int     result = -1;

          // set program counter
          int     curLine = ASMFileInfo.UnparsedLabels[label].LineIndex;
          while ( ( !ASMFileInfo.LineInfo.ContainsKey( curLine ) )
          &&      ( curLine >= 0 ) )
          {
            --curLine;
          }
          if ( curLine == -1 )
          {
            continue;
          }
          m_CompileCurrentAddress = ASMFileInfo.LineInfo[curLine].AddressStart;
          trueCompileCurrentAddress = m_CompileCurrentAddress;

          if ( EvaluateLabel( ASMFileInfo.UnparsedLabels[label].LineIndex, ASMFileInfo.UnparsedLabels[label].ToEval, out result ) )
          {
            //dh.Log( "evaluated unparsed label " + label + " to " + result );
            if ( ASMFileInfo.Labels.ContainsKey( label ) )
            {
              AddError( ASMFileInfo.UnparsedLabels[label].LineIndex, C64Studio.Types.ErrorCode.E1200_REDEFINITION_OF_LABEL, "Redefinition of label " + ASMFileInfo.UnparsedLabels[label].Name );
              continue;
            }

            Types.SymbolInfo token = new Types.SymbolInfo();
            token.Type            = Types.SymbolInfo.Types.LABEL;
            token.AddressOrValue  = result;
            token.Name            = label;
            token.LineIndex       = ASMFileInfo.UnparsedLabels[label].LineIndex;
            token.Used            = true;
            token.Zone            = ASMFileInfo.UnparsedLabels[label].Zone;
            ASMFileInfo.Labels.Add( label, token );

            ASMFileInfo.UnparsedLabels.Remove( label );
            newLabelDetermined = true;
            goto redo;
          }
        }
      }
      while ( newLabelDetermined );

      foreach ( int lineIndex in ASMFileInfo.LineInfo.Keys )
      {
        Types.ASM.LineInfo lineInfo = ASMFileInfo.LineInfo[lineIndex];

        m_CompileCurrentAddress = lineInfo.AddressStart;
        if ( lineInfo.NeededParsedExpression != null )
        {
          if ( lineInfo.NeededParsedExpression.Count == 0 )
          {
            AddError( lineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "Syntax Error" );
            return false;
          }
          // strip prefixed #
          if ( lineInfo.NeededParsedExpression[0].Content.StartsWith( "#" ) )
          {
            if ( lineInfo.NeededParsedExpression[0].Length == 1 )
            {
              lineInfo.NeededParsedExpression.RemoveAt( 0 );
            }
            else
            {
              lineInfo.NeededParsedExpression[0].Content = lineInfo.NeededParsedExpression[0].Content.Substring( 1 );
            }
          }
          string    lineToCheck = lineInfo.Line;

          if ( !lineToCheck.StartsWith( m_AssemblerSettings.MacroPrefix ) )
          {
            int   spacePos = lineToCheck.IndexOf( " " );
            if ( spacePos != -1 )
            {
              lineToCheck = lineToCheck.Substring( spacePos + 1 ).Trim();
            }
          }

          bool  isMacro = false;
          if ( ( m_AssemblerSettings.MacroPrefix.Length != 0 )
          &&   ( lineToCheck.StartsWith( m_AssemblerSettings.MacroPrefix ) ) )
          {
            isMacro = true;
          }
          if ( m_AssemblerSettings.MacroPrefix.Length == 0 )
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
            if ( m_AssemblerSettings.Macros.ContainsKey( startToken ) )
            {
              isMacro = true;
            }
          }

          if ( isMacro )
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
            bool pseudoOpHandled = false;
            if ( m_AssemblerSettings.Macros.ContainsKey( startToken ) )
            {
              var pseudoOp = m_AssemblerSettings.Macros[startToken];

              switch ( pseudoOp.Type )
              {
                case C64Studio.Types.MacroInfo.MacroType.BASIC:
                  {
                    int lineSize = -1;
                    if ( POBasic( lineInfo.Line, lineInfo.NeededParsedExpression, lineInfo.LineIndex, lineInfo, m_TextCodeMappingRaw, false, out lineSize ) != ParseLineResult.OK )
                    {
                      AddError( lineIndex, C64Studio.Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, 
                        "Failed to evaluate expression: " + TokensToExpression( lineInfo.NeededParsedExpression ) );
                    }
                    pseudoOpHandled = true;
                  }
                  break;
                default:
                  //AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Unhandled pseudo op during DetermineUnparsedLabels" );
                  break;
              }
            }

            if ( pseudoOpHandled )
            {
            }
            else if ( ( startToken == "!BYTE" )
            ||        ( startToken == "DW" )
            ||        ( startToken == ".BYTE" )
            ||        ( startToken == ".WORD" )
            ||        ( startToken == "DC.W" )
            ||        ( startToken == "!BY" )
            ||        ( startToken == "!8" )
            ||        ( startToken == "!08" )
            ||        ( startToken == "!16" )
            ||        ( startToken == "!WO" )
            ||        ( startToken == "!WORD" ) )
            {
              bool isByte = ( startToken == "!BYTE" ) || ( startToken == "!BY" ) || ( startToken == "!8" ) || ( startToken == "!08" ) || ( startToken == "DB" ) || ( startToken == "DC.B" ) || ( startToken == "DL" ) || ( startToken == "DH" ) || ( startToken == ".BYTE" );

              if ( isByte )
              {
                PODataByte( lineIndex, lineInfo.NeededParsedExpression, 0, lineInfo.NeededParsedExpression.Count, lineInfo, Types.MacroInfo.MacroType.BYTE, lineInfo.LineCodeMapping, false );
              }
              else
              {
                int     lineInBytes = 0;
                var result = PODataWord( lineInfo.NeededParsedExpression, lineInfo.LineIndex, 0, lineInfo.NeededParsedExpression.Count, lineInfo, lineToCheck, false, out lineInBytes );
              }
            }
            else if ( ( startToken == "!TEXT" )
            ||        ( startToken == "!TX" )
            ||        ( startToken == "!SCR" )
            ||        ( startToken == "!RAW" )
            ||        ( startToken == "!PET" )
            ||        ( startToken == "DC.B" )
            ||        ( startToken == "DC.V" )
            ||        ( startToken == "DB" )
            ||        ( startToken == "DH" )
            ||        ( startToken == "DL" ) )
            {
              var result = FinalParseData( lineInfo, lineIndex, true );
              if ( result == ParseLineResult.RETURN_FALSE )
              {
                return false;
              }
            }
            else if ( ( startToken == "!FILL" )
            ||        ( startToken == "!FI" )
            ||        ( startToken == ".BYTE" )
            ||        ( startToken == "DS" )
            ||        ( startToken == "DS.B" ) )
            {
              int tokenCommaIndex = -1;

              for ( int i = 0; i < lineInfo.NeededParsedExpression.Count; ++i )
              {
                if ( lineInfo.NeededParsedExpression[i].Content == "," )
                {
                  tokenCommaIndex = i;
                  break;
                }
              }
              if ( tokenCommaIndex == -1 )
              {
                AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate !fill expression" );
                return false;
              }

              int     count = -1;
              int     value = -1;
              int     dummyBytesGiven;

              if ( !EvaluateTokens( lineIndex, lineInfo.NeededParsedExpression, 0, tokenCommaIndex, out count, out dummyBytesGiven ) )
              {
                AddError( lineIndex, 
                          Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, 
                          "Could not evaluate " + TokensToExpression( lineInfo.NeededParsedExpression, 0, tokenCommaIndex ),
                          lineInfo.NeededParsedExpression[0].StartPos,
                          lineInfo.NeededParsedExpression[tokenCommaIndex - 1].EndPos + 1 - lineInfo.NeededParsedExpression[0].StartPos );
              }
              if ( !EvaluateTokens( lineIndex, lineInfo.NeededParsedExpression, tokenCommaIndex + 1, lineInfo.NeededParsedExpression.Count - tokenCommaIndex - 1, out value ) )
              {
                AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate " + TokensToExpression( lineInfo.NeededParsedExpression, tokenCommaIndex + 1, lineInfo.NeededParsedExpression.Count - tokenCommaIndex - 1 ) );
              }
              GR.Memory.ByteBuffer lineData = new GR.Memory.ByteBuffer();
              for ( int i = 0; i < count; ++i )
              {
                lineData.AppendU8( (byte)value );
              }
              lineInfo.LineData = lineData;
            }
            else
            {
              AddError( lineIndex, Types.ErrorCode.E1301_MACRO_UNKNOWN, "Unsupported pseudo op " + startToken );
              return false;
            }
          }
          else
          {
            int value = -1;
            if ( lineInfo.NeededParsedExpression.Count == 1 )
            {
              if ( lineInfo.NeededParsedExpression[0].Content.StartsWith( "+" ) )
              {
                // special case of forward local label
                string    closestLabel = "";
                int       closestLine = 5000000;
                foreach ( string label in ASMFileInfo.Labels.Keys )
                {
                  if ( label.StartsWith( InternalLabelPrefix + lineInfo.NeededParsedExpression[0].Content + InternalLabelPostfix ) )
                  {
                    int lineNo = -1;
                    if ( int.TryParse( label.Substring( ( InternalLabelPrefix + lineInfo.NeededParsedExpression[0].Content + InternalLabelPostfix ).Length ), out lineNo ) )
                    {
                      if ( ( lineNo > lineIndex )
                      &&   ( lineNo < closestLine ) )
                      {
                        closestLine   = lineNo;
                        closestLabel  = label;
                      }
                    }
                  }
                }
                if ( closestLine != 5000000 )
                {
                  lineInfo.NeededParsedExpression[0].Content = closestLabel;
                }
             } 
            }
            if ( !EvaluateTokens( lineIndex, lineInfo.NeededParsedExpression, out value ) )
            {
              /*
              Debug.Log( "need to assemble unparsed expression:" );
              Debug.Log( "=> Could not parse!" );
              Debug.Log( "=> from line: " + lineInfo.Line );
               */
              if ( !HasError() )
              {
                Debug.Log( "EvaluateTokens failed without error info!" );
                AddError( lineIndex, 
                          Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, 
                          "Could not evaluate " + TokensToExpression( lineInfo.NeededParsedExpression ),
                          lineInfo.NeededParsedExpression[0].StartPos,
                          lineInfo.NeededParsedExpression[lineInfo.NeededParsedExpression.Count - 1].EndPos + 1 - lineInfo.NeededParsedExpression[0].StartPos );
              }
              else if ( ( m_LastErrorInfo.Pos >= 0 )
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
                Debug.Log( "for line " + TokensToExpression( lineInfo.NeededParsedExpression ) );
                AddError( lineIndex,
                          Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION,
                          "Could not evaluate " + TokensToExpression( lineInfo.NeededParsedExpression ),
                          lineInfo.NeededParsedExpression[0].StartPos,
                          lineInfo.NeededParsedExpression[lineInfo.NeededParsedExpression.Count - 1].EndPos + 1 - lineInfo.NeededParsedExpression[0].StartPos );
              }
            }
            else
            {
              //dh.Log( "Parsed to: " + value );
              if ( lineInfo.Opcode != null )
              {
                if ( ( lineInfo.Opcode.ByteValue == 0x6C )
                &&   ( m_Processor.Name == "6510" )
                &&   ( ( value & 0xff ) == 0xff ) )
                {
                  AddWarning( lineIndex,
                              Types.ErrorCode.W0007_POTENTIAL_PROBLEM,
                              "A indirect JMP with an address ending on 0xff will not work as expected on NMOS CPUs",
                              lineInfo.NeededParsedExpression[0].StartPos,
                              lineInfo.NeededParsedExpression[lineInfo.NeededParsedExpression.Count - 1].EndPos + 1 - lineInfo.NeededParsedExpression[0].StartPos );
                }
                // check value size
                if ( ( lineInfo.Opcode.Addressing == Tiny64.Opcode.AddressingType.INDIRECT_Y )
                ||   ( lineInfo.Opcode.Addressing == Tiny64.Opcode.AddressingType.INDIRECT_X )
                ||   ( lineInfo.Opcode.Addressing == Tiny64.Opcode.AddressingType.IMMEDIATE )
                ||   ( lineInfo.Opcode.Addressing == Tiny64.Opcode.AddressingType.ZEROPAGE_X )
                ||   ( lineInfo.Opcode.Addressing == Tiny64.Opcode.AddressingType.ZEROPAGE_Y ) )
                {
                  if ( !ValidByteValue( value ) )
                  {
                    AddError( lineIndex, 
                              Types.ErrorCode.E1002_VALUE_OUT_OF_BOUNDS_BYTE, 
                              "Value out of bounds for byte, needs to be >= -128 and <= 255. Expression:"
                                + TokensToExpression( lineInfo.NeededParsedExpression ),
                              lineInfo.NeededParsedExpression[0].StartPos,
                              lineInfo.NeededParsedExpression[lineInfo.NeededParsedExpression.Count - 1].EndPos - lineInfo.NeededParsedExpression[0].StartPos + 1 );
                    lineInfo.LineData.AppendU8( 0 );
                    continue;
                  }
                }

                if ( lineInfo.Opcode.Addressing == Tiny64.Opcode.AddressingType.RELATIVE )
                {
                  int delta = value - lineInfo.AddressStart - 2;
                  if ( ( delta < -128 )
                  ||   ( delta > 127 ) )
                  {
                    AddError( lineIndex, Types.ErrorCode.E1100_RELATIVE_JUMP_TOO_FAR, "Relative jump too far, trying to jump " + delta + " bytes" );
                    lineInfo.LineData.AppendU8( 0 );
                    //return false;
                  }
                  else
                  {
                    lineInfo.LineData.AppendU8( (byte)delta );
                  }
                }
                else if ( lineInfo.Opcode.NumOperands == 1 )
                {
                  lineInfo.LineData.AppendU8( (byte)value );
                }
                else if ( lineInfo.Opcode.NumOperands == 2 )
                {
                  if ( ( value < 0 )
                  ||   ( value > 0xffff ) )
                  {
                    AddError( lineIndex,
                              Types.ErrorCode.E1003_VALUE_OUT_OF_BOUNDS_WORD,
                              "Value $" + value.ToString( "X" ) + " (" + value + ") is out of bounds",
                              lineInfo.NeededParsedExpression[0].StartPos,
                              lineInfo.NeededParsedExpression[lineInfo.NeededParsedExpression.Count - 1].EndPos - lineInfo.NeededParsedExpression[0].StartPos + 1 );
                  }
                  lineInfo.LineData.AppendU16( (ushort)value );
                }
              }
              lineInfo.NeededParsedExpression = null;
            }
          }
        }
      }

      foreach ( string label in ASMFileInfo.UnparsedLabels.Keys )
      {
        //dh.Log( "Still unevaluated label:" + label + ", " + m_UnparsedLabels[label].ToEval );
        AddError( ASMFileInfo.UnparsedLabels[label].LineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Failed to evaluate " + ASMFileInfo.UnparsedLabels[label].ToEval );
      }
      if ( m_ErrorMessages > 0 )
      {
        return false;
      }
      return ( ASMFileInfo.UnparsedLabels.Count == 0 );
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



    private void PODataByte( int LineIndex, List<Types.TokenInfo> lineTokenInfos, int StartIndex, int Count, Types.ASM.LineInfo info, Types.MacroInfo.MacroType Type, GR.Collections.Map<byte, byte> TextMapping, bool AllowNeededExpression )
    {
      GR.Memory.ByteBuffer data = new GR.Memory.ByteBuffer();

      int commaCount = 0;
      int firstTokenIndex = StartIndex;
      for ( int tokenIndex = StartIndex; tokenIndex < StartIndex + Count; ++tokenIndex )
      {
        string token = lineTokenInfos[tokenIndex].Content;

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
            int     byteValue = -1;
            int     numBytesGiven = 0;

            if ( EvaluateTokens( LineIndex, lineTokenInfos, firstTokenIndex, tokenIndex - firstTokenIndex, out byteValue, out numBytesGiven ) )
            {
              switch ( Type )
              {
                case C64Studio.Types.MacroInfo.MacroType.LOW_BYTE:
                  byteValue = byteValue & 0x00ff;
                  break;
                case C64Studio.Types.MacroInfo.MacroType.HIGH_BYTE:
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
        int byteValue = -1;
        int numBytesGiven = 0;
        if ( EvaluateTokens( LineIndex, lineTokenInfos, firstTokenIndex, lineTokenInfos.Count - firstTokenIndex, out byteValue, out numBytesGiven ) )
        {
          switch ( Type )
          {
            case C64Studio.Types.MacroInfo.MacroType.LOW_BYTE:
              byteValue = byteValue & 0x00ff;
              break;
            case C64Studio.Types.MacroInfo.MacroType.HIGH_BYTE:
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
        else if ( AllowNeededExpression )
        {
          info.NeededParsedExpression = lineTokenInfos.GetRange( StartIndex, Count );
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



    private bool ValidByteValue( int ByteValue )
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

        if ( token.Type != C64Studio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
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



    private ParseLineResult HandleScopeEnd( GR.Collections.Map<string, Types.MacroFunctionInfo> macroFunctions,
                                 List<Types.ScopeInfo> ScopeList,
                                 ref int lineIndex,
                                 ref int intermediateLineOffset,
                                 ref String[] Lines )
    {
      if ( ScopeList.Count == 0 )
      {
        AddError( lineIndex, Types.ErrorCode.E1007_MISSING_LOOP_START, "Missing loop start or opening bracket" );
        return ParseLineResult.ERROR_ABORT;
      }

      Types.ScopeInfo   lastOpenedScope = ScopeList[ScopeList.Count - 1];

      if ( ( lastOpenedScope.Type != Types.ScopeInfo.ScopeType.MACRO_FUNCTION )
      &&   ( lastOpenedScope.Type != Types.ScopeInfo.ScopeType.LOOP )
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
        for ( int i = 0; i < macroInfo.Content.Length; ++i )
        {
          var lineTokens = ParseTokenInfo( macroInfo.Content[i], 0, macroInfo.Content[i].Length );
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
                AddError( macroInfo.LineIndex + 1 + i, ErrorCode.E1302_MALFORMED_MACRO, "Macro " + macroInfo.Name + " is calling itself" );
                return ParseLineResult.ERROR_ABORT;
              }
            }
          }
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
          lineReplacement = RelabelLocalLabelsForLoop( lineReplacement, ScopeList, lineIndex );
          System.Array.Copy( lineReplacement, 0, Lines, lastLoop.LineIndex + 1, lastLoop.LoopLength );

          // fix up internal labels
          lastLoop.Content = RelabelLocalLabelsForLoop( lastLoop.Content, ScopeList, lineIndex );
        }

        // label exists only after !for
        AddTempLabel( lastLoop.Label, lineIndex - lastLoop.LoopLength, lastLoop.LoopLength, lastLoop.CurrentValue, "" );

        bool  endReached = false;

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
          Lines[lastLoop.LineIndex] = ";ex for loop";
          Lines[lineIndex] = ";ex loop end";

          //Debug.Log( "Cloning last loop for " + lastLoop.Label );
          CloneTempLabelsExcept( lastLoop.LineIndex, lastLoop.LoopLength, lineIndex - lastLoop.LoopLength - 1, lastLoop.Label );

          DumpTempLabelInfos( "__hla_STACK0" );

          //Debug.Log( "Last loop for " + lastLoop.Label + " reached" );
          //CloneSourceInfos( lastLoop.LineIndex, lastLoop.LoopLength, lineIndex - lastLoop.LoopLength );
        }
        else
        {
          lastLoop.CurrentValue += lastLoop.StepValue;

          // restart loop
          SetLabelValue( lastLoop.Label, lastLoop.CurrentValue );

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

          DumpLines( Lines, "a" );

          string[] newLines = new string[Lines.Length + linesToCopy];

          System.Array.Copy( Lines, 0, newLines, 0, lineIndex );
          System.Array.Copy( lastLoop.Content, 0, newLines, lineIndex, linesToCopy );
          System.Array.Copy( Lines, lineIndex + lineLoopEndOffset, newLines, lineIndex + linesToCopy, Lines.Length - lineIndex - lineLoopEndOffset );

          // fix up internal labels
          lastLoop.Content = RelabelLocalLabelsForLoop( lastLoop.Content, ScopeList, lineIndex );

          DumpLines( newLines, "b" );

          // also copy scoped variables if overlapping!!!
          if ( !endReached )
          {
            //Debug.Log( "Cloning loop " + lastLoop.CurrentValue + "/" + lastLoop.EndValue + " for " + lastLoop.Label );
            CloneTempLabelsExcept( lastLoop.LineIndex, linesToCopy, lineIndex - 1, lastLoop.Label );
            DumpTempLabelInfos( "__hla_STACK0" );
          }

          // adjust source infos to make lookup work correctly
          string outerFilename = "";
          int outerLineIndex = -1;
          ASMFileInfo.FindTrueLineSource( lastLoop.LineIndex, out outerFilename, out outerLineIndex );


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
      return ParseLineResult.OK;
    }



    int       dumpCount = 0;
    private void DumpLines( string[] lines, string Text )
    {
      /*
      string    outName = "before" + dumpCount + ".txt";
      if ( Text == "b" )
      {
        outName = "after" + dumpCount + ".txt";
      }
      string    outPut = "Step " + dumpCount + "\r\n" + string.Join( "\r\n", lines );
      System.IO.File.WriteAllText( outName, outPut );*/
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



    public List<Types.TokenInfo> PrepareLineTokens( string Line )
    {
      List<Types.TokenInfo> lineTokenInfos = ParseTokenInfo( Line, 0, Line.Length );
      if ( HasError() )
      {
        return null;
      }
      if ( lineTokenInfos.Count == 0 )
      {
        return lineTokenInfos;
      }

      // evaluate, could be a label in front?
      // merge + with local token for possible macro functions
      if ( ( lineTokenInfos.Count >= 2 )
      && ( lineTokenInfos[0].Content == m_AssemblerSettings.MacroFunctionCallPrefix )
      && ( ( lineTokenInfos[1].Type == C64Studio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
      || ( lineTokenInfos[1].Type == C64Studio.Types.TokenInfo.TokenType.LABEL_LOCAL ) )
      && ( lineTokenInfos[0].StartPos + lineTokenInfos[0].Length == lineTokenInfos[1].StartPos ) )
      {
        lineTokenInfos[1].Type = C64Studio.Types.TokenInfo.TokenType.CALL_MACRO;
        lineTokenInfos[1].Content = lineTokenInfos[0].Content + lineTokenInfos[1].Content;
        lineTokenInfos[1].StartPos = lineTokenInfos[0].StartPos;
        lineTokenInfos[1].Length = lineTokenInfos[1].Length + lineTokenInfos[0].Length;
        lineTokenInfos.RemoveAt( 0 );
      }
      if ( ( lineTokenInfos[0].Type == C64Studio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
      ||   ( lineTokenInfos[0].Type == C64Studio.Types.TokenInfo.TokenType.LABEL_LOCAL ) )
      {
        // could be a label in front
        if ( ( lineTokenInfos.Count >= 3 )
        && ( lineTokenInfos[1].Content == m_AssemblerSettings.MacroFunctionCallPrefix )
        && ( ( lineTokenInfos[2].Type == C64Studio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
        || ( lineTokenInfos[2].Type == C64Studio.Types.TokenInfo.TokenType.LABEL_LOCAL ) )
        && ( lineTokenInfos[1].StartPos + lineTokenInfos[1].Length == lineTokenInfos[2].StartPos ) )
        {
          lineTokenInfos[2].Type = C64Studio.Types.TokenInfo.TokenType.CALL_MACRO;
          lineTokenInfos[2].Content = lineTokenInfos[1].Content + lineTokenInfos[2].Content;
          lineTokenInfos[2].StartPos = lineTokenInfos[1].StartPos;
          lineTokenInfos[2].Length = lineTokenInfos[2].Length + lineTokenInfos[1].Length;
          lineTokenInfos.RemoveAt( 1 );
        }
      }

      return lineTokenInfos;
    }



    // find macro in following lines (
    private int FindLoopEnd( string[] Lines, int StartIndex, List<Types.ScopeInfo> StackDefineBlocks )
    {
      List<Types.ScopeInfo> stackDefineBlocks = new List<C64Studio.Types.ScopeInfo>( StackDefineBlocks );

      int loopCount = 1;


      for ( int lineIndex = StartIndex; lineIndex < Lines.Length; ++lineIndex )
      {
        string parseLine = Lines[lineIndex];

        List<Types.TokenInfo> lineTokenInfos = PrepareLineTokens( parseLine );
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

            //if ( parseLine.ToUpper().StartsWith( "!IF" ) )
            if ( lineTokenInfos[0].Content.ToUpper().StartsWith( "!IF" ) )
            {
              // a new block starts here!
              // false, since it doesn't matter
              Types.ScopeInfo scope = new C64Studio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.IF_OR_IFDEF );
              scope.StartIndex = lineIndex;
              scope.Active = false;
              stackDefineBlocks.Add( scope );

              OnScopeAdded( scope );
            }
            else if ( ( lineTokenInfos[0].Content.ToUpper().StartsWith( "!PSEUDOPC" ) )
            &&        ( lineTokenInfos[lineTokenInfos.Count - 1].Content == "{" ) )
            {
              // ACME style pseudo pc with bracket
              Types.ScopeInfo scope = new C64Studio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.PSEUDO_PC );
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
              Types.ScopeInfo scope = new C64Studio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.ADDRESS );
              scope.StartIndex = lineIndex;
              scope.Active = false;
              stackDefineBlocks.Add( scope );
              OnScopeAdded( scope );
            }
            else if ( ( lineTokenInfos[0].Content.ToUpper().StartsWith( "!ZONE" ) )
            &&        ( lineTokenInfos.Count >= 2 )
            &&        ( lineTokenInfos[1].Content == "{" ) )
            {
              Types.ScopeInfo   zoneScope = new C64Studio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.ZONE );
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

        if ( lineTokenInfos[0].Type == C64Studio.Types.TokenInfo.TokenType.MACRO )
        {
          isMacroIndex = 0;
        }
        if ( ( lineTokenInfos[0].Type == C64Studio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
        ||   ( lineTokenInfos[0].Type == C64Studio.Types.TokenInfo.TokenType.LABEL_LOCAL )
        ||   ( lineTokenInfos[0].Type == C64Studio.Types.TokenInfo.TokenType.LABEL_INTERNAL ) )
        {
          if ( m_AssemblerSettings.Macros.ContainsKey( lineTokenInfos[0].Content.ToUpper() ) )
          {
            isMacroIndex = 0;
          }
          else if ( lineTokenInfos.Count > 1 )
          {
            if ( ( lineTokenInfos[1].Type == C64Studio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
            ||   ( lineTokenInfos[1].Type == C64Studio.Types.TokenInfo.TokenType.LABEL_LOCAL )
            ||   ( lineTokenInfos[1].Type == C64Studio.Types.TokenInfo.TokenType.LABEL_INTERNAL ) )
            {
              if ( m_AssemblerSettings.Macros.ContainsKey( lineTokenInfos[1].Content.ToUpper() ) )
              {
                isMacroIndex = 1;
              }
            }
          }
        }

        if ( isMacroIndex != -1 )
        {
          Types.MacroInfo macro = m_AssemblerSettings.Macros[lineTokenInfos[isMacroIndex].Content.ToUpper()];
          
          if ( macro.Type == C64Studio.Types.MacroInfo.MacroType.LOOP_END )
          {
            --loopCount;
            if ( loopCount == 0 )
            {
              return lineIndex;
            }
          }
          else if ( macro.Type == C64Studio.Types.MacroInfo.MacroType.LOOP_START )
          {
            ++loopCount;
          }
        }
      }

      return -1;
    }



    private bool POIncludeMedia( List<Types.TokenInfo> lineTokenInfos, int lineIndex, bool Binary, Types.ASM.LineInfo info, string ParentFilename, out int lineSizeInBytes, out string[] ReplacementLines )
    {
      ReplacementLines = null;
      lineSizeInBytes = 0;


      List<List<Types.TokenInfo>>   paramTokens = new List<List<C64Studio.Types.TokenInfo>>();
      paramTokens.Add( new List<C64Studio.Types.TokenInfo>() );

      int             paramPos = 0;

      for ( int i = 1; i < lineTokenInfos.Count; ++i )
      {
        if ( lineTokenInfos[i].Content == "," )
        {
          ++paramPos;
          if ( paramPos >= 7 )
          {
            if ( Binary )
            {
              AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected !media <Filename>[,<MediaParams>...]" );
            }
            else
            {
              AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected !mediasrc <Filename>,SrcLabelPrefix[,<MediaParams>...]" );
            }
            return false;
          }
          paramTokens.Add( new List<C64Studio.Types.TokenInfo>() );
        }
        else
        {
          paramTokens[paramPos].Add( lineTokenInfos[i] );
        }
      }

      if ( paramTokens.Count < 2 )
      {
        if ( Binary )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected !media <Filename>[,<MediaParams>...]" );
        }
        else
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected !mediasrc <Filename>,SrcLabelPrefix[,<MediaParams>...]" );
        }
        return false;
      }

      // validate filename
      if ( ( paramTokens[0].Count != 1 )
      ||   ( paramTokens[0][0].Length < 2 ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected string literal as filename" );
        return false;
      }
      if ( ( !paramTokens[0][0].Content.StartsWith( "\"" ) )
      ||   ( !paramTokens[0][0].Content.EndsWith( "\"" ) ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1307_FILENAME_INCOMPLETE, "Expected proper file name between apostrophes" );
        return false;
      }
      string    subFilename = paramTokens[0][0].Content.Substring( 1, paramTokens[0][0].Length - 2 );
      int       includeMethodParamIndex = 1;
      string    labelPrefix = "";

      subFilename = BuildFullPath( System.IO.Path.GetDirectoryName( ParentFilename ), subFilename );

      if ( !Binary )
      {
        if ( paramTokens.Count < 3 )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected !mediasrc <Filename>,SrcLabelPrefix[,<MediaParams>...]" );
          return false;
        }
        // label prefix 
        if ( ( paramTokens[1].Count != 1 )
        ||   ( ( paramTokens[1][0].Type != C64Studio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
        &&     ( paramTokens[1][0].Type != C64Studio.Types.TokenInfo.TokenType.LABEL_LOCAL ) ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected proper global or local label prefix" );
          return false;
        }
        labelPrefix = paramTokens[1][0].Content;

        paramTokens.RemoveAt( 1 );
      }

      string    extension = System.IO.Path.GetExtension( subFilename ).ToUpper();
      if ( ( paramTokens[includeMethodParamIndex].Count != 1 )
      ||   ( paramTokens[includeMethodParamIndex][0].Type != C64Studio.Types.TokenInfo.TokenType.LABEL_GLOBAL ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected known import method" );
        return false;
      }
      string    method = paramTokens[includeMethodParamIndex][0].Content.ToUpper();

      GR.Memory.ByteBuffer    dataToInclude = new GR.Memory.ByteBuffer();


      if ( extension == ".CHR" )
      {
        if ( !Binary )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Assembly include for charsets is not supported" );
          return false;
        }

        // char set file
        //  char,index,count
        if ( paramTokens.Count > 4 )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected <Char>[,<Index>[,<Count>]]" );
          return false;
        }
        if ( method != "CHAR" )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Unknown method, supported values for this file name are CHAR" );
          return false;
        }
        int   startIndex = 0;
        int   numChars = 256;

        if ( ( paramTokens.Count >= 3 )
        &&   ( !EvaluateTokens( lineIndex, paramTokens[2], out startIndex ) ) )
        {
          startIndex = 0;
        }
        if ( ( paramTokens.Count >= 4 )
        &&   ( !EvaluateTokens( lineIndex, paramTokens[3], out numChars ) ) )
        {
          numChars = 256;
        }
        try
        {
          dataToInclude = GR.IO.File.ReadAllBytes( subFilename );
          if ( dataToInclude == null )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilename );
            return false;
          }
        }
        catch ( System.IO.IOException )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilename );
          return false;
        }
        if ( ( startIndex < 0 )
        ||   ( startIndex * 8 >= dataToInclude.Length ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid start index " + startIndex );
          return false;
        }
        if ( ( numChars <= 0 )
        ||   ( ( startIndex + numChars ) * 8 > dataToInclude.Length ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid char count " + numChars );
          return false;
        }
        dataToInclude = dataToInclude.SubBuffer( startIndex * 8, numChars * 8 );
      }
      else if ( extension == ".CHARSETPROJECT" )
      {
        if ( !Binary )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Assembly include for charset projects is not supported" );
          return false;
        }

        // character project file
        // char,index,count
        if ( paramTokens.Count > 4 )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected <Char|CharColor>[,<Index>[,<Count>]]" );
          return false;
        }
        if ( ( method != "CHAR" )
        &&   ( method != "CHARCOLOR" ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Unknown method, supported values for this file name are CHAR or CHARCOLOR" );
          return false;
        }
        int   startIndex = 0;
        int   numChars = 256;

        if ( ( paramTokens.Count >= 3 )
        &&   ( !EvaluateTokens( lineIndex, paramTokens[2], out startIndex ) ) )
        {
          startIndex = 0;
        }
        if ( ( paramTokens.Count >= 4 )
        &&   ( !EvaluateTokens( lineIndex, paramTokens[3], out numChars ) ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Failed to evaluate expression " + TokensToExpression( paramTokens[3] ) );
          return false;
        }
        Formats.CharsetProject    charProject = new C64Studio.Formats.CharsetProject();

        try
        {
          dataToInclude = GR.IO.File.ReadAllBytes( subFilename );
          if ( dataToInclude == null )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilename );
            return false;
          }
        }
        catch ( System.IO.IOException )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilename );
          return false;
        }
        if ( !charProject.ReadFromBuffer( dataToInclude ) )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read charset project file " + subFilename );
          return false;
        }
        if ( ( startIndex < 0 )
        ||   ( startIndex >= charProject.NumCharacters ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid start index " + startIndex );
          return false;
        }
        if ( ( numChars <= 0 )
        || ( ( startIndex + numChars ) > charProject.NumCharacters ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid char count " + numChars );
          return false;
        }
        GR.Memory.ByteBuffer    charData = new GR.Memory.ByteBuffer( (uint)( numChars * 8 ) );

        for ( int i = 0; i < numChars; ++i )
        {
          charProject.Characters[startIndex + i].Data.CopyTo( charData, 0, 8, i * 8 );
        }
        if ( method == "CHARCOLOR" )
        {
          for ( int i = 0; i < numChars; ++i )
          {
            charData.AppendU8( (byte)charProject.Characters[startIndex + i].Color );
          }
        }
        dataToInclude = charData;
      }
      else if ( extension == ".VALUETABLEPROJECT" )
      {
        if ( !Binary )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Assembly include for value table projects is not supported" );
          return false;
        }

        // value table project file
        if ( paramTokens.Count > 4 )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected <Data>[,<Offset>[,<Bytes>]]" );
          return false;
        }
        if ( method != "DATA" )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Unknown method, supported values for this file name are DATA" );
          return false;
        }
        int     numBytes = -1;
        int     startIndex = 0;
        if ( ( paramTokens.Count >= 3 )
        &&   ( !EvaluateTokens( lineIndex, paramTokens[2], out startIndex ) ) )
        {
          startIndex = 0;
        }
        if ( ( paramTokens.Count >= 4 )
        &&   ( !EvaluateTokens( lineIndex, paramTokens[3], out numBytes ) ) )
        {
          numBytes = 0;
        }
        Formats.ValueTableProject   valueTableProject = new Formats.ValueTableProject();

        try
        {
          dataToInclude = GR.IO.File.ReadAllBytes( subFilename );
          if ( dataToInclude == null )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilename );
            return false;
          }
        }
        catch ( System.IO.IOException )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilename );
          return false;
        }
        if ( !valueTableProject.ReadFromBuffer( dataToInclude ) )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read value table project file " + subFilename );
          return false;
        }
        dataToInclude = valueTableProject.GenerateTableData();
        if ( ( startIndex < 0 )
        ||   ( startIndex >= dataToInclude.Length ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid start index " + startIndex );
          return false;
        }
        if ( numBytes== -1 )
        {
          numBytes = (int)dataToInclude.Length;
        }
        if ( ( numBytes <= 0 )
        || ( ( startIndex + numBytes ) > dataToInclude.Length ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid byte count " + numBytes );
          return false;
        }
        dataToInclude = dataToInclude.SubBuffer( 0, numBytes );
      }
      else if ( extension == ".SPR" )
      {
        if ( !Binary )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Assembly include for sprites is not supported" );
          return false;
        }

        if ( ( method != "SPRITE" )
        &&   ( method != "SPRITEDATA" ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Unknown method, supported values for this file name are SPRITE or SPRITEDATA" );
          return false;
        }

        if ( ( method == "SPRITE" )
        &&   ( paramTokens.Count > 4 ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected <Sprite>[,<Index>[,<Count>]]" );
          return false;
        }
        if ( ( method == "SPRITEDATA" )
        &&   ( paramTokens.Count != 6 ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected <Spritedata>,<Index>,<Count>,<Offset>,<NumBytes>" );
          return false;
        }

        if ( method == "SPRITE" )
        {
          int   startIndex = 0;
          int   numSprites = -1;

          if ( ( paramTokens.Count >= 3 )
          &&   ( !EvaluateTokens( lineIndex, paramTokens[2], out startIndex ) ) )
          {
            startIndex = 0;
          }
          if ( ( paramTokens.Count >= 4 )
          &&   ( !EvaluateTokens( lineIndex, paramTokens[3], out numSprites ) ) )
          {
            numSprites = -1;
          }
          try
          {
            dataToInclude = GR.IO.File.ReadAllBytes( subFilename );
            if ( dataToInclude == null )
            {
              AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilename );
              return false;
            }
          }
          catch ( System.IO.IOException )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilename );
            return false;
          }
          if ( numSprites == -1 )
          {
            numSprites = (int)dataToInclude.Length / 64;
          }
          if ( ( startIndex < 0 )
          ||   ( startIndex * 64 >= dataToInclude.Length ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid start index " + startIndex );
            return false;
          }
          if ( ( numSprites <= 0 )
          ||   ( ( startIndex + numSprites ) * 64 > dataToInclude.Length ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid sprite count " + numSprites );
            return false;
          }
          dataToInclude = dataToInclude.SubBuffer( startIndex * 64, numSprites * 64 );
        }
        else if ( method == "SPRITEDATA" )
        {
          int   startIndex = 0;
          int   numSprites = -1;
          int   offsetBytes = 0;
          int   numBytes = numSprites * 64;

          if ( !EvaluateTokens( lineIndex, paramTokens[2], out startIndex ) )
          {
            startIndex = 0;
          }
          if ( !EvaluateTokens( lineIndex, paramTokens[3], out numSprites ) )
          {
            numSprites = -1;
          }
          if ( !EvaluateTokens( lineIndex, paramTokens[4], out offsetBytes ) )
          {
            offsetBytes = 0;
          }
          if ( !EvaluateTokens( lineIndex, paramTokens[5], out numBytes ) )
          {
            numBytes = numSprites * 64;
          }

          try
          {
            dataToInclude = GR.IO.File.ReadAllBytes( subFilename );
            if ( dataToInclude == null )
            {
              AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilename );
              return false;
            }
          }
          catch ( System.IO.IOException )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilename );
            return false;
          }
          if ( numSprites == -1 )
          {
            numSprites = (int)dataToInclude.Length / 64;
          }
          if ( ( startIndex < 0 )
          ||   ( startIndex * 64 >= dataToInclude.Length ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid start index " + startIndex );
            return false;
          }
          if ( ( numSprites <= 0 )
          ||   ( ( startIndex + numSprites ) * 64 > dataToInclude.Length ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid sprite count " + numSprites );
            return false;
          }
          dataToInclude = dataToInclude.SubBuffer( startIndex * 64, numSprites * 64 );
          if ( ( offsetBytes >= dataToInclude.Length )
          ||   ( offsetBytes < 0 ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid data offset " + offsetBytes );
            return false;
          }
          if ( ( offsetBytes + numBytes > dataToInclude.Length )
          ||   ( numBytes <= 0 ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid data size " + numBytes );
            return false;
          }
          dataToInclude = dataToInclude.SubBuffer( offsetBytes, numBytes );
        }
      }
      else if ( extension == ".SPRITEPROJECT" )
      {
        if ( !Binary )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Assembly include for sprite projects is not supported" );
          return false;
        }

        if ( ( method != "SPRITE" )
        &&   ( method != "SPRITEDATA" ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Unknown method, supported values for this file name are SPRITE and SPRITEDATA" );
          return false;
        }

        // sprite set file
        // sprites,index,count
        if ( ( method == "SPRITE" )
        &&   ( paramTokens.Count > 4 ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected <Sprite>[,<Index>[,<Count>]]" );
          return false;
        }
        if ( ( method == "SPRITEDATA" )
        &&   ( paramTokens.Count != 6 ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected <SpriteData>,<Index>,<Count>,<Offset>,<NumBytes>" );
          return false;
        }
        if ( method == "SPRITE" )
        {
          int   startIndex = 0;
          int   numSprites = -1; 

          if ( ( paramTokens.Count >= 3 )
          &&   ( !EvaluateTokens( lineIndex, paramTokens[2], out startIndex ) ) )
          {
            startIndex = 0;
          }
          if ( ( paramTokens.Count >= 4 )
          &&   ( !EvaluateTokens( lineIndex, paramTokens[3], out numSprites ) ) )
          {
            numSprites = -1;
          }
          Formats.SpriteProject   spriteProject = new C64Studio.Formats.SpriteProject();

          try
          {
            dataToInclude = GR.IO.File.ReadAllBytes( subFilename );
            if ( dataToInclude == null )
            {
              AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilename );
              return false;
            }
          }
          catch ( System.IO.IOException )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilename );
            return false;
          }
          if ( !spriteProject.ReadFromBuffer( dataToInclude ) )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read sprite project file " + subFilename );
            return false;
          }
          if ( numSprites == -1 )
          {
            numSprites = spriteProject.NumSprites;
          }
          if ( ( startIndex < 0 )
          ||   ( startIndex >= spriteProject.NumSprites ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid start index " + startIndex );
            return false;
          }
          if ( ( numSprites <= 0 )
          ||   ( ( startIndex + numSprites ) > spriteProject.NumSprites ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid sprite count " + numSprites );
            return false;
          }
          GR.Memory.ByteBuffer    spriteData = new GR.Memory.ByteBuffer( (uint)( numSprites * 64 ) );

          for ( int i = 0; i < numSprites; ++i )
          {
            spriteProject.Sprites[startIndex + i].Data.CopyTo( spriteData, 0, 63, i * 64 );
          }
          dataToInclude = spriteData;
        }
        else if ( method == "SPRITEDATA" )
        {
          int   startIndex = 0;
          int   numSprites = 256;
          int   offsetBytes = 0;
          int   numBytes = numSprites * 64;

          if ( !EvaluateTokens( lineIndex, paramTokens[2], out startIndex ) )
          {
            startIndex = 0;
          }
          if ( !EvaluateTokens( lineIndex, paramTokens[3], out numSprites ) )
          {
            numSprites = 256;
          }
          if ( !EvaluateTokens( lineIndex, paramTokens[4], out offsetBytes ) )
          {
            offsetBytes = 0;
          }
          if ( !EvaluateTokens( lineIndex, paramTokens[5], out numBytes ) )
          {
            numBytes = numSprites * 64;
          }

          Formats.SpriteProject   spriteProject = new C64Studio.Formats.SpriteProject();

          try
          {
            dataToInclude = GR.IO.File.ReadAllBytes( subFilename );
            if ( dataToInclude == null )
            {
              AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilename );
              return false;
            }
          }
          catch ( System.IO.IOException )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilename );
            return false;
          }
          if ( !spriteProject.ReadFromBuffer( dataToInclude ) )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read sprite project file " + subFilename );
            return false;
          }
          if ( numSprites == -1 )
          {
            numSprites = spriteProject.NumSprites;
          }
          if ( ( startIndex < 0 )
          ||   ( startIndex >= spriteProject.NumSprites ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid start index " + startIndex );
            return false;
          }
          if ( ( numSprites <= 0 )
          ||   ( ( startIndex + numSprites ) > spriteProject.NumSprites ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid sprite count " + numSprites );
            return false;
          }
          GR.Memory.ByteBuffer    spriteData = new GR.Memory.ByteBuffer( (uint)( numSprites * 64 ) );

          for ( int i = 0; i < numSprites; ++i )
          {
            spriteProject.Sprites[startIndex + i].Data.CopyTo( spriteData, 0, 63, i * 64 );
          }

          if ( ( offsetBytes >= spriteData.Length )
          ||   ( offsetBytes < 0 ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid data offset " + offsetBytes );
            return false;
          }
          if ( ( offsetBytes + numBytes > spriteData.Length )
          ||   ( numBytes <= 0 ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid data size " + numBytes );
            return false;
          }
          dataToInclude = spriteData.SubBuffer( offsetBytes, numBytes );
        }
      }
      else if ( extension == ".SPD" )
      {
        if ( !Binary )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Assembly include for sprite projects is not supported" );
          return false;
        }

        if ( ( method != "SPRITE" )
        &&   ( method != "SPRITEDATA" ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Unknown method, supported values for this file name are SPRITE and SPRITEDATA" );
          return false;
        }

        // sprite set file
        // sprites,index,count
        if ( ( method == "SPRITE" )
        &&   ( paramTokens.Count > 4 ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected <Sprite>[,<Index>[,<Count>]]" );
          return false;
        }
        if ( ( method == "SPRITEDATA" )
        &&   ( paramTokens.Count != 6 ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected <SpriteData>,<Index>,<Count>,<Offset>,<NumBytes>" );
          return false;
        }
        if ( method == "SPRITE" )
        {
          int   startIndex = 0;
          int   numSprites = -1;

          if ( ( paramTokens.Count >= 3 )
          &&   ( !EvaluateTokens( lineIndex, paramTokens[2], out startIndex ) ) )
          {
            startIndex = 0;
          }
          if ( ( paramTokens.Count >= 4 )
          &&   ( !EvaluateTokens( lineIndex, paramTokens[3], out numSprites ) ) )
          {
            numSprites = -1;
          }
          Formats.SpritePadProject    spriteProject = new Formats.SpritePadProject();

          try
          {
            dataToInclude = GR.IO.File.ReadAllBytes( subFilename );
            if ( dataToInclude == null )
            {
              AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilename );
              return false;
            }
          }
          catch ( System.IO.IOException )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilename );
            return false;
          }
          if ( !spriteProject.ReadFromBuffer( dataToInclude ) )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read sprite project file " + subFilename );
            return false;
          }
          if ( numSprites == -1 )
          {
            numSprites = spriteProject.NumSprites;
          }
          if ( ( startIndex < 0 )
          ||   ( startIndex >= spriteProject.NumSprites ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid start index " + startIndex );
            return false;
          }
          if ( ( numSprites <= 0 )
          ||   ( ( startIndex + numSprites ) > spriteProject.NumSprites ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid sprite count " + numSprites );
            return false;
          }
          GR.Memory.ByteBuffer    spriteData = new GR.Memory.ByteBuffer( (uint)( numSprites * 64 ) );

          for ( int i = 0; i < numSprites; ++i )
          {
            spriteProject.Sprites[startIndex + i].Data.CopyTo( spriteData, 0, 63, i * 64 );
          }
          dataToInclude = spriteData;
        }
        else if ( method == "SPRITEDATA" )
        {
          int   startIndex = 0;
          int   numSprites = 256;
          int   offsetBytes = 0;
          int   numBytes = numSprites * 64;

          if ( !EvaluateTokens( lineIndex, paramTokens[2], out startIndex ) )
          {
            startIndex = 0;
          }
          if ( !EvaluateTokens( lineIndex, paramTokens[3], out numSprites ) )
          {
            numSprites = 256;
          }
          if ( !EvaluateTokens( lineIndex, paramTokens[4], out offsetBytes ) )
          {
            offsetBytes = 0;
          }
          if ( !EvaluateTokens( lineIndex, paramTokens[5], out numBytes ) )
          {
            numBytes = numSprites * 64;
          }

          Formats.SpritePadProject    spriteProject = new Formats.SpritePadProject();

          try
          {
            dataToInclude = GR.IO.File.ReadAllBytes( subFilename );
            if ( dataToInclude == null )
            {
              AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilename );
              return false;
            }
          }
          catch ( System.IO.IOException )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilename );
            return false;
          }
          if ( !spriteProject.ReadFromBuffer( dataToInclude ) )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read sprite project file " + subFilename );
            return false;
          }
          if ( ( startIndex < 0 )
          ||   ( startIndex >= spriteProject.NumSprites ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid start index " + startIndex );
            return false;
          }
          if ( ( numSprites <= 0 )
          ||   ( ( startIndex + numSprites ) > spriteProject.NumSprites ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid sprite count " + numSprites );
            return false;
          }
          GR.Memory.ByteBuffer    spriteData = new GR.Memory.ByteBuffer( (uint)( numSprites * 64 ) );

          for ( int i = 0; i < numSprites; ++i )
          {
            spriteProject.Sprites[startIndex + i].Data.CopyTo( spriteData, 0, 63, i * 64 );
          }

          if ( ( offsetBytes >= spriteData.Length )
          ||   ( offsetBytes < 0 ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid data offset " + offsetBytes );
            return false;
          }
          if ( ( offsetBytes + numBytes > spriteData.Length )
          || ( numBytes <= 0 ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid data size " + numBytes );
            return false;
          }
          dataToInclude = spriteData.SubBuffer( offsetBytes, numBytes );
        }
      }
      else if ( extension == ".CHARSCREEN" )
      {
        // screen
        // char,x,y,width,height
        // color,x,y,width,height
        // charcolor,x,y,width,height
        // colorchar,x,y,width,height
        // charvert,x,y,width,height
        // colorvert,x,y,width,height
        // charcolorvert,x,y,width,height
        // colorcharvert,x,y,width,height
        if ( paramTokens.Count > 6 )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected <Char|CharVert|Color|ColorVert|CharColor|CharColorVert|ColorChar|ColorCharVert|CharSet>[,<X>[,<Y>[,<Width>[,<Height>]]]]" );
          return false;
        }
        if ( ( method != "CHAR" )
        &&   ( method != "COLOR" )
        &&   ( method != "CHARSET" ) 
        &&   ( method != "CHARCOLOR" ) 
        &&   ( method != "COLORCHAR" )
        &&   ( method != "CHARVERT" )
        &&   ( method != "COLORVERT" )
        &&   ( method != "CHARCOLORVERT" )
        &&   ( method != "COLORCHARVERT" ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Unknown method, supported values for this file name are CHAR, COLOR, CHARCOLOR, COLORCHAR, CHARVERT, COLORVERT, CHARCOLORVERT, COLORCHARVERT and CHARSET" );
          return false;
        }

        Formats.CharsetScreenProject    screenProject = new C64Studio.Formats.CharsetScreenProject();

        try
        {
          dataToInclude = GR.IO.File.ReadAllBytes( subFilename );
          if ( dataToInclude == null )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilename );
            return false;
          }
        }
        catch ( System.IO.IOException )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilename );
          return false;
        }
        if ( !screenProject.ReadFromBuffer( dataToInclude ) )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read charset screen project from " + subFilename );
          return false;
        }

        if ( method == "CHARSET" )
        {
          if ( !Binary )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Export as CHARSET is only supported for binary" );
            return false;
          }

          int startIndex = 0;
          int numChars = screenProject.CharSet.NumCharacters;

          if ( ( paramTokens.Count >= 3 )
          &&   ( !EvaluateTokens( lineIndex, paramTokens[2], out startIndex ) ) )
          {
            startIndex = 0;
          }
          if ( ( paramTokens.Count >= 4 )
          &&   ( !EvaluateTokens( lineIndex, paramTokens[3], out numChars ) ) )
          {
            numChars = screenProject.CharSet.NumCharacters;
          }
          if ( ( startIndex < 0 )
          ||   ( startIndex >= screenProject.CharSet.NumCharacters ) )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Invalid start index" );
            return false;
          }
          if ( ( numChars <= 0 )
          ||   ( startIndex + numChars > screenProject.CharSet.NumCharacters ) )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Invalid number of characters" );
            return false;
          }
          dataToInclude = screenProject.CharSet.CharacterData().SubBuffer( startIndex * 8, numChars * 8 );
        }
        else
        {
          int   x = 0;
          int   y = 0;
          int   w = screenProject.ScreenWidth;
          int   h = screenProject.ScreenHeight;

          if ( ( paramTokens.Count >= 3 )
          &&   ( !EvaluateTokens( lineIndex, paramTokens[2], out x ) ) )
          {
            x = 0;
          }
          if ( ( paramTokens.Count >= 4 )
          &&   ( !EvaluateTokens( lineIndex, paramTokens[3], out y ) ) )
          {
            y = 0;
          }
          if ( ( paramTokens.Count >= 5 )
          &&   ( !EvaluateTokens( lineIndex, paramTokens[4], out w ) ) )
          {
            w = screenProject.ScreenWidth;
          }
          if ( ( paramTokens.Count >= 6 )
          &&   ( !EvaluateTokens( lineIndex, paramTokens[5], out h ) ) )
          {
            h = screenProject.ScreenHeight;
          }

          if ( ( x < 0 )
          ||   ( x >= screenProject.ScreenWidth )
          ||   ( y < 0 )
          ||   ( y >= screenProject.ScreenHeight )
          ||   ( w < 0 )
          ||   ( x + w > screenProject.ScreenWidth )
          ||   ( h < 0 )
          ||   ( y + h > screenProject.ScreenHeight ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid coordinates" );
            return false;
          }

          dataToInclude.Clear();

          GR.Memory.ByteBuffer    charData = new GR.Memory.ByteBuffer();
          GR.Memory.ByteBuffer    colorData = new GR.Memory.ByteBuffer();
          GR.Memory.ByteBuffer    charSet = new GR.Memory.ByteBuffer();

          bool    rowByRow = !method.EndsWith( "VERT" );


          if ( !screenProject.ExportToBuffer( out charData, out colorData, out charSet, x, y, w, h, rowByRow ) )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Failed to export data from " + subFilename );
            return false;
          }

          string    textToInclude = "";
          if ( ( method == "CHAR" )
          ||   ( method == "CHARVERT" ) )
          {
            if ( !Binary )
            {
              textToInclude = labelPrefix + "_CHARS" + System.Environment.NewLine;
              textToInclude += Util.ToASMData( charData, false, 0, MacroByType( Types.MacroInfo.MacroType.BYTE ) );

              ReplacementLines = textToInclude.Split( new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries );
            }
            else
            {
              dataToInclude = charData;
            }
          }
          else if ( ( method == "COLOR" )
          ||        ( method == "COLORVERT" ) )
          {
            if ( !Binary )
            {
              textToInclude = labelPrefix + "_COLOR" + System.Environment.NewLine;
              textToInclude += Util.ToASMData( colorData, false, 0, MacroByType( Types.MacroInfo.MacroType.BYTE ) );
              ReplacementLines = textToInclude.Split( new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries );
            }
            else
            {
              dataToInclude = colorData;
            }
          }
          else if ( ( method == "CHARCOLOR" )
          ||        ( method == "CHARCOLORVERT" ) )
          {
            if ( !Binary )
            {
              textToInclude = labelPrefix + "_CHARS" + System.Environment.NewLine;
              textToInclude += Util.ToASMData( charData, false, 0, MacroByType( Types.MacroInfo.MacroType.BYTE ) ) + System.Environment.NewLine;
              textToInclude += labelPrefix + "_COLOR" + System.Environment.NewLine;
              textToInclude += Util.ToASMData( colorData, false, 0, MacroByType( Types.MacroInfo.MacroType.BYTE ) );

              ReplacementLines = textToInclude.Split( new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries );
            }
            else
            {
              dataToInclude = charData + colorData;
            }
          }
          else if ( ( method == "COLORCHAR" )
          ||        ( method == "COLORCHARVERT" ) )
          {
            if ( !Binary )
            {
              textToInclude = labelPrefix + "_COLOR" + System.Environment.NewLine;
              textToInclude += Util.ToASMData( colorData, false, 0, MacroByType( Types.MacroInfo.MacroType.BYTE ) ) + System.Environment.NewLine;
              textToInclude += labelPrefix + "_CHARS" + System.Environment.NewLine;
              textToInclude += Util.ToASMData( charData, false, 0, MacroByType( Types.MacroInfo.MacroType.BYTE ) );
              ReplacementLines = textToInclude.Split( new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries );
            }
            else
            {
              dataToInclude = colorData + charData;
            }
          }
        }
      }
      else if ( extension == ".GRAPHICSCREEN" )
      {
        // graphic screen
        // bitmap,x,y,width,height
        // bitmapscreen,x,y,width,height
        // bitmapscreencolor,x,y,width,height
        // screen,x,y,width,height
        // color,x,y,width,height
        if ( paramTokens.Count > 6 )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected <Bitmap|BitmapScreen|BitmapScreenColor|BitmapHires|BitmapHiresScreen|BitmapHiresScreenColor|Screen|Color>[,<X>[,<Y>[,<Width>[,<Height>]]]]" );
          return false;
        }
        if ( ( method != "BITMAP" )
        &&   ( method != "BITMAPSCREEN" )
        &&   ( method != "BITMAPSCREENCOLOR" )
        &&   ( method != "BITMAPHIRES" )
        &&   ( method != "BITMAPHIRESSCREEN" )
        &&   ( method != "BITMAPHIRESSCREENCOLOR" )
        &&   ( method != "SCREEN" )
        &&   ( method != "COLOR" ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Unknown method, supported values for this file name are BITMAP, BITMAPSCREEN, BITMAPSCREENCOLOR, BITMAPHIRES, BITMAPHIRESSCREEN, BITMAPHIRESSCREENCOLOR, SCREEN and COLOR" );
          return false;
        }
        Formats.GraphicScreenProject screenProject = new C64Studio.Formats.GraphicScreenProject();

        try
        {
          dataToInclude = GR.IO.File.ReadAllBytes( subFilename );
          if ( dataToInclude == null )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilename );
            return false;
          }
        }
        catch ( System.IO.IOException )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilename );
          return false;
        }
        if ( !screenProject.ReadFromBuffer( dataToInclude ) )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read graphic screen project from " + subFilename );
          return false;
        }

        int   x = 0;
        int   y = 0;
        int   w = screenProject.ScreenWidth;
        int   h = screenProject.ScreenHeight;

        if ( ( paramTokens.Count >= 3 )
        &&   ( !EvaluateTokens( lineIndex, paramTokens[2], out x ) ) )
        {
          x = 0;
        }
        if ( ( paramTokens.Count >= 4 )
        &&   ( !EvaluateTokens( lineIndex, paramTokens[3], out y ) ) )
        {
          y = 0;
        }
        if ( ( paramTokens.Count >= 5 )
        &&   ( !EvaluateTokens( lineIndex, paramTokens[4], out w ) ) )
        {
          w = screenProject.ScreenWidth;
        }
        if ( ( paramTokens.Count >= 6 )
        && ( !EvaluateTokens( lineIndex, paramTokens[5], out h ) ) )
        {
          h = screenProject.ScreenHeight;
        }
        if ( ( x < 0 )
        ||   ( x >= screenProject.ScreenWidth )
        ||   ( y < 0 )
        ||   ( y >= screenProject.ScreenHeight )
        ||   ( w < 0 )
        ||   ( x + w > screenProject.ScreenWidth )
        ||   ( h < 0 )
        ||   ( y + h > screenProject.ScreenHeight )
        ||   ( x % 8 != 0 )
        ||   ( y % 8 != 0 )
        ||   ( w % 8 != 0 )
        ||   ( h % 8 != 0 ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid coordinates (x,y,width,height must be multiples of 8)" );
          return false;
        }

        dataToInclude.Clear();

        bool    importAsMC = !method.Contains( "BITMAPHIRES" );

        GR.Memory.ByteBuffer screenChar   = new GR.Memory.ByteBuffer();
        GR.Memory.ByteBuffer screenColor  = new GR.Memory.ByteBuffer();
        GR.Memory.ByteBuffer bitmapData   = new GR.Memory.ByteBuffer();

        if ( importAsMC )
        {
          screenProject.ImageToMCBitmapData( screenProject.ColorMapping, null, null, out bitmapData, out screenChar, out screenColor );
        }
        else
        {
          screenProject.ImageToHiresBitmapData( null, null, out bitmapData, out screenChar, out screenColor );
        }

        GR.Memory.ByteBuffer    bitmapClipped = new GR.Memory.ByteBuffer( (uint)( w / 8 * h / 8 * 8 ) );
        GR.Memory.ByteBuffer    colorClipped = new GR.Memory.ByteBuffer( (uint)( w / 8 * h / 8 ) );
        GR.Memory.ByteBuffer    screenClipped = new GR.Memory.ByteBuffer( (uint)( w / 8 * h / 8 ) );

        int numBytesCopiedBM = 0;
        int numBytesCopiedColor = 0;
        for ( int j = 0; j < h / 8; ++j )
        {
          bitmapData.CopyTo( bitmapClipped, ( y + j * 8 ) * screenProject.ScreenWidth / 8 + x, ( w / 8 ) * 8, numBytesCopiedBM );
          numBytesCopiedBM += w;

          screenChar.CopyTo( screenClipped, ( y / 8 + j ) * screenProject.ScreenWidth / 8 + x / 8, w / 8, numBytesCopiedColor );
          screenColor.CopyTo( colorClipped, ( y / 8 + j ) * screenProject.ScreenWidth / 8 + x / 8, w / 8, numBytesCopiedColor );
          numBytesCopiedColor += w / 8;
        }

        string textToInclude = "";

        if ( method.StartsWith( "BITMAP" ) )
        {
          if ( !Binary )
          {
            textToInclude += labelPrefix + "_BITMAP_DATA" + System.Environment.NewLine;
            textToInclude += Util.ToASMData( bitmapClipped, false, 0, MacroByType( Types.MacroInfo.MacroType.BYTE ) ) + System.Environment.NewLine;
          }
          else
          {
            dataToInclude.Append( bitmapClipped );
          }
        }
        if ( method.IndexOf( "SCREEN" ) != -1 )
        {
          if ( !Binary )
          {
            textToInclude += labelPrefix + "_SCREEN_DATA" + System.Environment.NewLine;
            textToInclude += Util.ToASMData( screenClipped, false, 0, MacroByType( Types.MacroInfo.MacroType.BYTE ) ) + System.Environment.NewLine;
          }
          else
          {
            dataToInclude.Append( screenClipped );
          }
        }
        if ( method.IndexOf( "COLOR" ) != -1 )
        {
          if ( !Binary )
          {
            textToInclude += labelPrefix + "_COLOR_DATA" + System.Environment.NewLine;
            textToInclude += Util.ToASMData( colorClipped, false, 0, MacroByType( Types.MacroInfo.MacroType.BYTE ) ) + System.Environment.NewLine;
          }
          else
          {
            dataToInclude.Append( colorClipped );
          }
        }

        if ( !Binary )
        {
          ReplacementLines = textToInclude.Split( new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries );
        }
      }
      else if ( extension == ".MAPPROJECT" )
      {
        if ( Binary )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Binary export from map project is not supported" );
          return false;
        }

        // map project
        // map,index,count
        // tile,index,count
        // maptile
        if ( paramTokens.Count > 4 )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected <Map|Tile|MapTile|TileElements|TileData>[,<Index>[,<Count>]]" );
          return false;
        }
        if ( ( method != "TILE" )
        &&   ( method != "TILEDATA" )
        &&   ( method != "MAP" )
        &&   ( method != "MAPVERTICAL" )
        &&   ( method != "TILEELEMENTS" )
        &&   ( method != "MAPTILE" )
        &&   ( method != "MAPVERTICALTILE" ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Unknown method, supported values for this file name are MAP, MAPVERTICAL, TILE, TILEDATA, TILEELEMENTS, MAPTILE, MAPVERTICALTILE" );
          return false;
        }

        Formats.MapProject map = new C64Studio.Formats.MapProject();

        try
        {
          dataToInclude = GR.IO.File.ReadAllBytes( subFilename );
          if ( dataToInclude == null )
          {
            AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilename );
            return false;
          }
        }
        catch ( System.IO.IOException )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilename );
          return false;
        }
        if ( !map.ReadFromBuffer( dataToInclude ) )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read map project from " + subFilename );
          return false;
        }

        string textToInclude = "";

        if ( method == "TILEELEMENTS" )
        {
          map.ExportTilesAsElements( out textToInclude, labelPrefix, false, 0, MacroByType( C64Studio.Types.MacroInfo.MacroType.BYTE ) );
        }
        else if ( method == "MAP" )
        {
          map.ExportMapsAsAssembly( false, out textToInclude, labelPrefix, false, 0, MacroByType( C64Studio.Types.MacroInfo.MacroType.BYTE ) );
        }
        else if ( method == "MAPVERTICAL" )
        {
          map.ExportMapsAsAssembly( true, out textToInclude, labelPrefix, false, 0, MacroByType( C64Studio.Types.MacroInfo.MacroType.BYTE ) );
        }
        else if ( method == "TILE" )
        {
          map.ExportTilesAsAssembly( out textToInclude, labelPrefix, false, 0, MacroByType( C64Studio.Types.MacroInfo.MacroType.BYTE ) );
        }
        else if ( method == "TILEDATA" )
        {
          map.ExportTileDataAsAssembly( out textToInclude, labelPrefix, false, 0, MacroByType( C64Studio.Types.MacroInfo.MacroType.BYTE ) );
        }
        else if ( method == "MAPTILE" )
        {
          string  dummy;
          map.ExportTilesAsAssembly( out dummy, labelPrefix, false, 0, MacroByType( C64Studio.Types.MacroInfo.MacroType.BYTE ) );
          textToInclude += dummy;

          map.ExportMapsAsAssembly( false, out dummy, labelPrefix, false, 0, MacroByType( C64Studio.Types.MacroInfo.MacroType.BYTE ) );
          textToInclude += dummy;
          //Debug.Log( textToInclude );
        }
        else if ( method == "MAPVERTICALTILE" )
        {
          string  dummy;
          map.ExportTilesAsAssembly( out dummy, labelPrefix, false, 0, MacroByType( C64Studio.Types.MacroInfo.MacroType.BYTE ) );
          textToInclude += dummy;

          map.ExportMapsAsAssembly( true, out dummy, labelPrefix, false, 0, MacroByType( C64Studio.Types.MacroInfo.MacroType.BYTE ) );
          textToInclude += dummy;
          //Debug.Log( textToInclude );
        }

        if ( !Binary )
        {
          ReplacementLines = textToInclude.Split( new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries );
        }
      }
      else
      {
        AddError( lineIndex, C64Studio.Types.ErrorCode.E2002_UNSUPPORTED_FILE_TYPE, "Unknown file type" );
        return false;
      }

      ExternallyIncludedFiles.Add( subFilename );

      if ( !Binary )
      {
        return true;
      }

      info.LineData = dataToInclude;
      info.NumBytes = (int)info.LineData.Length;
      lineSizeInBytes = (int)dataToInclude.Length;
      return true;
    }



    private ParseLineResult POLoopStart( List<Types.TokenInfo> lineTokenInfos, int lineIndex, Types.ASM.LineInfo info, ref string[] Lines, List<Types.ScopeInfo> Scopes, out int lineSizeInBytes )
    {
      lineSizeInBytes = 0;

      // DO <num>
      if ( lineTokenInfos.Count < 2 )
      {
        AddError( lineIndex, C64Studio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Malformed macro, expected DO <Expression>" );
      }
      else
      {
        int numLoops = -1;

        if ( EvaluateTokens( lineIndex, lineTokenInfos, 1, lineTokenInfos.Count - 1, out numLoops ) )
        {
          bool hadError = false;
          if ( numLoops <= 0 )
          {
            AddError( lineIndex, C64Studio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Loop count must be positive" );
            hadError = true;
          }
          if ( !hadError )
          {
            // TODO - find matching loop end and copy lines now (to avoid auto-inserting macros only in the first iteration)
            int nextLineIndex = FindLoopEnd( Lines, lineIndex + 1, Scopes );
            if ( nextLineIndex == -1 )
            {
              AddError( lineIndex, C64Studio.Types.ErrorCode.E1008_MISSING_LOOP_END, "Missing loop end" );
              hadError = true;
            }
            else
            {
              int loopLength = nextLineIndex - lineIndex - 1;
              string[] tempContent = new string[loopLength * ( numLoops - 1 )];

              for ( int i = 0; i < numLoops - 1; ++i )
              {
                System.Array.Copy( Lines, lineIndex + 1, tempContent, i * loopLength, loopLength );
              }

              string[] replacementLines = RelabelLocalLabelsForLoop( tempContent, Scopes, lineIndex );

              string[] newLines = new string[Lines.Length + replacementLines.Length];

              System.Array.Copy( Lines, 0, newLines, 0, lineIndex + 1 + loopLength );
              //System.Array.Copy( Lines, lineIndex - loopBlockLength, newLines, lineIndex, linesToCopy );
              System.Array.Copy( replacementLines, 0, newLines, lineIndex + 1 + loopLength, replacementLines.Length );

              // replaces the REPEND
              newLines[lineIndex + 1 + loopLength + replacementLines.Length] = "";
              System.Array.Copy( Lines, nextLineIndex + 1, newLines, lineIndex + 1 + loopLength + replacementLines.Length + 1, Lines.Length - nextLineIndex - 1 );

              // adjust source infos to make lookup work correctly
              string outerFilename = "";
              int outerLineIndex = -1;
              ASMFileInfo.FindTrueLineSource( lineIndex + 1, out outerFilename, out outerLineIndex );

              //ASMFileInfo.LineInfo.Remove( lineIndex );

              for ( int i = 0; i < numLoops - 1; ++i )
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
          }
        }
        else
        {
          AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate " + TokensToExpression( lineTokenInfos, 1, lineTokenInfos.Count - 1 ) );
        }
      }
      return ParseLineResult.OK;
    }



    private ParseLineResult POFill( List<Types.TokenInfo> lineTokenInfos, int lineIndex, Types.ASM.LineInfo info, string parseLine, out int lineSizeInBytes )
    {
      lineSizeInBytes = 0;
      ClearErrorInfo();

      string fillNumberToken = "";
      bool firstToken = true;
      int tokenIndex = 0;
      bool  hadComma = false;
      foreach ( Types.TokenInfo token in lineTokenInfos )
      {
        if ( token.Content == "," )
        {
          hadComma = true;
          int numBytes = -1;

          List<Types.TokenInfo> tokens = ParseTokenInfo( fillNumberToken, 0, fillNumberToken.Length );
          if ( !EvaluateTokens( lineIndex, tokens, out numBytes ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Could not determine fill parameter " + fillNumberToken );
            return ParseLineResult.RETURN_NULL;
          }
          info.NumBytes = numBytes;
          info.Line = parseLine;
          info.NeededParsedExpression = lineTokenInfos.GetRange( 1, lineTokenInfos.Count - 1 );
          break;
        }
        if ( firstToken )
        {
          firstToken = false;
        }
        else
        {
          fillNumberToken += token.Content;
        }
        ++tokenIndex;
      }

      if ( !hadComma )
      {
        // only number of bytes, default 0
        hadComma = true;
        int numBytes = -1;
        List<Types.TokenInfo> tokens = ParseTokenInfo( fillNumberToken, 0, fillNumberToken.Length );
        if ( !EvaluateTokens( lineIndex, tokens, out numBytes ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Could not evaluate fill count parameter: " + fillNumberToken.Substring( m_LastErrorInfo.Pos, m_LastErrorInfo.Length ) );
          return ParseLineResult.RETURN_NULL;
        }
        info.NumBytes = numBytes;
        info.Line = parseLine;
        info.LineData = new GR.Memory.ByteBuffer( (uint)numBytes );
      }

      if ( info.NumBytes == 0 )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Macro malformed, expect !FILL <Count>,<Value>" );
        return ParseLineResult.RETURN_NULL;
      }
      lineSizeInBytes = info.NumBytes;
      return ParseLineResult.OK;
    }



    private ParseLineResult POText( List<Types.TokenInfo> lineTokenInfos, Types.ASM.LineInfo info, String parseLine, GR.Collections.Map<byte,byte> TextMapping, out int lineSizeInBytes )
    {
      int numBytes = 0;
      int commaCount = 0;
      int literalCount = 0;

      bool firstToken = true;
      foreach ( Types.TokenInfo token in lineTokenInfos )
      {
        if ( firstToken )
        {
          firstToken = false;
          continue;
        }
        if ( ( token.Content.StartsWith( "\"" ) )
        &&   ( token.Content.EndsWith( "\"" ) ) )
        {
          numBytes += token.Length - 2;
          ++literalCount;
        }
        else if ( token.Content == "," )
        {
          ++commaCount;
        }
      }
      // !text 128 + 15, LETTER_O, LETTER_F, LETTER_SPACE
      if ( commaCount + 1 - literalCount > 0 )
      {
        numBytes += commaCount + 1 - literalCount;
      }
      info.NumBytes               = numBytes;
      info.Line                   = parseLine;
      info.NeededParsedExpression = lineTokenInfos.GetRange( 1, lineTokenInfos.Count - 1 );
      info.LineCodeMapping        = TextMapping;
      lineSizeInBytes             = info.NumBytes;

      return ParseLineResult.OK;
    }



    private ParseLineResult POBasic( string Line, List<Types.TokenInfo> lineTokenInfos, int lineIndex, Types.ASM.LineInfo info, GR.Collections.Map<byte,byte> textCodeMapping, bool AllowLaterEvaluation, out int lineSizeInBytes )
    {
      lineSizeInBytes = 13;
      info.NumBytes   = 13;

      int       basicLineNumber = 10;
      GR.Memory.ByteBuffer    commentData = new ByteBuffer();

      List<int> tokenParams = new List<int>();
      bool      paramsValid = false;
      int       jumpAddress = -1;

      int       realNumParams = 1;
      int       numDigits = -1;

      List<List<Types.TokenInfo>>   poParams = new List<List<Types.TokenInfo>>();

      int     firstTokenIndex = 1;
      int     secondTokenIndex = -1;
      for ( int i = 1; i < lineTokenInfos.Count; ++i )
      {
        if ( ( lineTokenInfos[i].Type == Types.TokenInfo.TokenType.SEPARATOR )
        &&   ( lineTokenInfos[i].Content == "," ) )
        {
          if ( i - firstTokenIndex > 0 )
          {
            poParams.Add( lineTokenInfos.GetRange( firstTokenIndex, i - firstTokenIndex ) );
          }
          else
          {
            poParams.Add( new List<Types.TokenInfo>() );
          }
          
          ++realNumParams;
          firstTokenIndex = i + 1;
        }
        else if ( ( firstTokenIndex > 1 )
        &&        ( secondTokenIndex == -1 ) )
        {
          secondTokenIndex = i;
        }

      }
      if ( firstTokenIndex < lineTokenInfos.Count )
      {
        poParams.Add( lineTokenInfos.GetRange( firstTokenIndex, lineTokenInfos.Count - firstTokenIndex ) );
      }

      if ( lineTokenInfos.Count == 1 )
      {
        // !basic
        paramsValid = true;
      }
      else if ( ( lineTokenInfos.Count > 1 )
      &&        ( poParams.Count == 1 ) )
      {
        // !basic <jump address>
        if ( !EvaluateTokens( lineIndex, poParams[0], 0, poParams[0].Count, out jumpAddress ) )
        {
          // could not fully parse
          info.NeededParsedExpression = lineTokenInfos;
          info.Line = Line;
          // can we use 4 digits?
          if ( info.AddressStart + 12 < 10000 )
          {
            lineSizeInBytes = 12;
            info.NumBytes = 12;
          }
          if ( !AllowLaterEvaluation )
          {
            info.NeededParsedExpression = poParams[0];
          }
          return ParseLineResult.OK_PARSE_EXPRESSION_LATER;
        }
        if ( ( jumpAddress < 0 )
        ||   ( jumpAddress >= 65536 ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1003_VALUE_OUT_OF_BOUNDS_WORD, "Jump target address is out of bounds" );
          return ParseLineResult.RETURN_NULL;
        }
        paramsValid = true;
      }
      else if ( ( lineTokenInfos.Count > 1 )
      &&        ( realNumParams == 2 ) )
      {
        // !basic <line number>,<jump address>
        if ( !EvaluateTokens( lineIndex, poParams[0], 0, poParams[0].Count, out basicLineNumber ) )
        {
          // could not fully parse
          info.NeededParsedExpression = lineTokenInfos;
          info.Line = Line;
          // can we use 4 digits?
          if ( info.AddressStart + 12 < 10000 )
          {
            lineSizeInBytes = 12;
            info.NumBytes = 12;
          }
          if ( !AllowLaterEvaluation )
          {
            info.NeededParsedExpression = poParams[0];
          }
          return ParseLineResult.OK_PARSE_EXPRESSION_LATER;
        }
        if ( ( basicLineNumber < 0 )
        ||   ( basicLineNumber > 63999 ) )
        {
          AddError( lineIndex, Types.ErrorCode.E3001_BASIC_INVALID_LINE_NUMBER, "Unsupported line number, must be in the range 0 to 63999" );
          return ParseLineResult.RETURN_NULL;
        }

        if ( !EvaluateTokens( lineIndex, poParams[1], 0, poParams[1].Count, out jumpAddress ) )
        {
          // could not fully parse
          info.NeededParsedExpression = lineTokenInfos;
          info.Line = Line;
          // can we use 4 digits?
          if ( info.AddressStart + 12 < 10000 )
          {
            lineSizeInBytes = 12;
            info.NumBytes = 12;
          }
          if ( !AllowLaterEvaluation )
          {
            info.NeededParsedExpression = poParams[1];
          }
          return ParseLineResult.OK_PARSE_EXPRESSION_LATER;
        }
        if ( ( jumpAddress < 0 )
        ||   ( jumpAddress >= 65536 ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1003_VALUE_OUT_OF_BOUNDS_WORD, "Jump target address is out of bounds" );
          return ParseLineResult.RETURN_NULL;
        }
        paramsValid = true;
      }
      else if ( ( lineTokenInfos.Count > 1 )
      &&        ( realNumParams >= 3 ) )
      {
        info.NeededParsedExpression = null;
        // !basic <line number>,<comment>[,<comment-bytes>],<jump address>
        if ( !EvaluateTokens( lineIndex, poParams[0], 0, poParams[0].Count, out basicLineNumber ) )
        {
          // could not fully parse
          info.NeededParsedExpression = lineTokenInfos;
          info.Line = Line;
          // can we use 4 digits?
          if ( info.AddressStart + 12 < 10000 )
          {
            lineSizeInBytes = 12;
            info.NumBytes = 12;
          }
          if ( !AllowLaterEvaluation )
          {
            info.NeededParsedExpression = poParams[0];
          }
          return ParseLineResult.OK_PARSE_EXPRESSION_LATER;
        }
        if ( ( basicLineNumber < 0 )
        ||   ( basicLineNumber > 63999 ) )
        {
          AddError( lineIndex, Types.ErrorCode.E3001_BASIC_INVALID_LINE_NUMBER, "Unsupported line number, must be in the range 0 to 63999" );
          return ParseLineResult.RETURN_NULL;
        }

        var dummyLineInfo = new Types.ASM.LineInfo();
        var subRange = lineTokenInfos.GetRange( secondTokenIndex, lineTokenInfos.Count - secondTokenIndex - 2 );

        for ( int i = 1; i < poParams.Count - 1; ++i )
        {
          GR.Memory.ByteBuffer    dataOut;

          if ( EvaluateTokensBinary( lineIndex, poParams[i], textCodeMapping, out dataOut ) )
          {
            if ( dummyLineInfo.LineData == null )
            {
              dummyLineInfo.LineData = new ByteBuffer();
            }
            dummyLineInfo.LineData.Append( dataOut );
          }
          else
          {
            AddError( info.LineIndex,
                      Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION,
                      "Failed to evaluate expression " + TokensToExpression( poParams[i], 0, poParams[i].Count ),
                      poParams[i][0].StartPos,
                      poParams[i][poParams[i].Count - 1].EndPos - poParams[i][0].StartPos + 1 );
            return ParseLineResult.ERROR_ABORT;
          }
        }

        var commentDataTemp = dummyLineInfo.LineData;
        if ( commentDataTemp == null )
        {
          commentDataTemp = new GR.Memory.ByteBuffer();
        }

        int   lengthOfCommentData = (int)commentDataTemp.Length;

        if ( !EvaluateTokens( lineIndex, poParams[poParams.Count - 1], 0, poParams[poParams.Count - 1].Count, out jumpAddress ) )
        {
          // could not fully parse
          info.NeededParsedExpression = lineTokenInfos;// poParams[poParams.Count - 1];
          info.Line = Line;
          // can we use 4 digits?
          if ( info.AddressStart + 12 + lengthOfCommentData < 10000 )
          {
            lineSizeInBytes = 12 + lengthOfCommentData;
            info.NumBytes = 12 + lengthOfCommentData;
          }
          if ( !AllowLaterEvaluation )
          {
            info.NeededParsedExpression = poParams[poParams.Count - 1];
          }
          return ParseLineResult.OK_PARSE_EXPRESSION_LATER;
        }
        if ( ( jumpAddress < 0 )
        ||   ( jumpAddress >= 65536 ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1003_VALUE_OUT_OF_BOUNDS_WORD, "Jump target address is out of bounds" );
          return ParseLineResult.RETURN_NULL;
        }
        commentData = commentDataTemp;
        paramsValid = true;
      }

      if ( ( lineTokenInfos.Count < 1 )
      ||   ( !paramsValid ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected !basic [<jump address>] or !basic <line number>,<jump address> or !basic <line number>,<comment>[,<more comment-bytes>],<jump address>" );
        return ParseLineResult.RETURN_NULL;
      }

      if ( jumpAddress == -1 )
      {
        int   endAddress = info.AddressStart + 8 + 5;
        jumpAddress = endAddress - 5 + CalcNumDigits( endAddress );
      }

      numDigits = CalcNumDigits( jumpAddress );

      /*
      if ( jumpAddress != -1 )
      {
        jumpAddress -= 5 - numDigits;
      }*/

      info.LineData = new GR.Memory.ByteBuffer();

      // Startadresse der folgenden Programmzeile in der Reihenfolge Low - Byte, High - Byte($0000 kennzeichnet das Programmende ).
      // Zeilennummer in der Form Low - Byte, High - Byte.
      // Der eigentliche Programmcode (bis zu 250 Bytes) im Token-Format.
      // Ein Null - Byte kennzeichnet das Ende der Programmzeile.

      info.LineData.AppendU8( (byte)( 0x0b - 5 + numDigits ) );
      // 0x08   high byte of address of next line
      // 0xa000 line number 160 ?
      // 0x9e   SYS
      info.LineData.AppendHex( "08" );
      info.LineData.AppendU16( (ushort)basicLineNumber );
      info.LineData.AppendHex( "9E" );
      int     lineLength = (int)( 2 + 3 + numDigits + commentData.Length + 1 );
      info.LineData.SetU16At( 0, (ushort)( info.AddressStart + lineLength ) );

      lineSizeInBytes = (int)info.LineData.Length + numDigits + 1 + (int)commentData.Length;

      if ( jumpAddress == -1 )
      {
        jumpAddress = info.AddressStart + lineSizeInBytes + 2;
      }

      if ( numDigits >= 5 )
      {
        info.LineData.AppendU8( (byte)( 0x30 + jumpAddress / 10000 ) );
      }
      if ( numDigits >= 4 )
      {
        info.LineData.AppendU8( (byte)( 0x30 + ( ( jumpAddress / 1000 ) % 10 ) ) );
      }
      if ( numDigits >= 3 )
      {
        info.LineData.AppendU8( (byte)( 0x30 + ( ( jumpAddress / 100 ) % 10 ) ) );
      }
      if ( numDigits >= 2 )
      {
        info.LineData.AppendU8( (byte)( 0x30 + ( ( jumpAddress / 10 ) % 10 ) ) );
      }
      info.LineData.AppendU8( (byte)( 0x30 + ( jumpAddress % 10 ) ) );

      //info.LineData.AppendU8( 0x20 );
      info.LineData.Append( commentData );

      info.LineData.AppendHex( "000000" );

      lineSizeInBytes = (int)info.LineData.Length;
      info.NumBytes = lineSizeInBytes;

      return ParseLineResult.OK;
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
              int     result = -1;
              if ( !ParseValue( LineIndex, token.Content, out result ) )
              {
                return false;
              }
              DataOut.AppendU8( (byte)result );
            }
            break;
          case TokenInfo.TokenType.LABEL_GLOBAL:
          case TokenInfo.TokenType.LABEL_CHEAP_LOCAL:
          case TokenInfo.TokenType.LABEL_INTERNAL:
          case TokenInfo.TokenType.LABEL_LOCAL:
            {
              int   result = -1;
              if ( !ParseValue( LineIndex, token.Content, out result, out numBytes ) )
              {
                return false;
              }
              DataOut.AppendU8( (byte)result );
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
          int value = -1;

          if ( !EvaluateTokens( lineIndex, lineTokenInfos, expressionStartIndex, tokenIndex - expressionStartIndex, out value ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate " + TokensToExpression( lineTokenInfos, expressionStartIndex, tokenIndex - expressionStartIndex ) );
            return ParseLineResult.RETURN_NULL;
          }          tokenParams.Add( value );
          expressionStartIndex = tokenIndex + 1;
        }
        ++tokenIndex;
        if ( tokenIndex == lineTokenInfos.Count )
        {
          if ( expressionStartIndex <= tokenIndex - 1 )
          {
            // there's still data to evaluate
            int value = -1;

            if ( !EvaluateTokens( lineIndex, lineTokenInfos, expressionStartIndex, tokenIndex - expressionStartIndex, out value ) )
            {
              AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate " + TokensToExpression( lineTokenInfos, expressionStartIndex, tokenIndex - expressionStartIndex ) );
              return ParseLineResult.RETURN_NULL;
            }
            tokenParams.Add( value );
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



    private ParseLineResult PODataWord( List<Types.TokenInfo> lineTokenInfos, int LineIndex, int StartIndex, int Count, Types.ASM.LineInfo info, String parseLine, bool AllowNeededExpression, out int lineSizeInBytes )
    {
      GR.Memory.ByteBuffer data = new GR.Memory.ByteBuffer();

      int commaCount = 0;
      int firstTokenIndex = StartIndex;
      for ( int tokenIndex = StartIndex; tokenIndex < StartIndex + Count; ++tokenIndex )
      {
        string token = lineTokenInfos[tokenIndex].Content;

        if ( ( tokenIndex == StartIndex )
        &&   ( token == "#" ) )
        {
          // direct value?
          if ( ( lineTokenInfos.Count > 2 )
          && ( lineTokenInfos[2].Content != "#" )
          && ( lineTokenInfos[2].Content != "." ) )
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

            if ( EvaluateTokens( LineIndex, lineTokenInfos, firstTokenIndex, tokenIndex - firstTokenIndex, out wordValue, out numBytesGiven ) )
            {
              if ( !ValidWordValue( wordValue ) )
              {
                AddError( info.LineIndex,
                          Types.ErrorCode.E1003_VALUE_OUT_OF_BOUNDS_WORD,
                          "Value out of bounds for word, needs to be >= -32768 and <= 65535. Expression:" + TokensToExpression( lineTokenInfos, firstTokenIndex, tokenIndex - firstTokenIndex ),
                          lineTokenInfos[firstTokenIndex].StartPos,
                          lineTokenInfos[tokenIndex - 1].EndPos - lineTokenInfos[firstTokenIndex].StartPos + 1 );
              }
              data.AppendU16( (ushort)wordValue );
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
        if ( EvaluateTokens( LineIndex, lineTokenInfos, firstTokenIndex, lineTokenInfos.Count - firstTokenIndex, out wordValue, out numBytesGiven ) )
        {
          if ( !ValidWordValue( wordValue ) )
          {
            AddError( info.LineIndex,
                      Types.ErrorCode.E1003_VALUE_OUT_OF_BOUNDS_WORD, 
                      "Value out of bounds for word, needs to be >= -32768 and <= 65535. Expression:" + TokensToExpression( lineTokenInfos, firstTokenIndex, lineTokenInfos.Count - firstTokenIndex ),
                      lineTokenInfos[firstTokenIndex].StartPos,
                      lineTokenInfos[lineTokenInfos.Count - 1].EndPos - lineTokenInfos[firstTokenIndex].StartPos + 1 );
          }
          data.AppendU16( (ushort)wordValue );
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



    private bool ValidWordValue( int WordValue )
    {
      if ( ( !m_CompileConfig.AutoTruncateLiteralValues )
      &&   ( ( WordValue < -32768 )
      ||     ( WordValue > 65535 ) ) )
      {
        return false;
      }
      return true;
    }



    private ParseLineResult POIncludeSource( bool LibraryFile, string subFilename, string ParentFilename, ref int lineIndex, ref string[] Lines )
    {
      SourceInfoLog( "Include file " + subFilename + ", lib file " + LibraryFile );
      if ( m_LoadedFiles[ParentFilename] == null )
      {
        m_LoadedFiles[ParentFilename] = new GR.Collections.Set<string>();
      }

      if ( GR.Path.IsPathEqual( ParentFilename, subFilename ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1400_CIRCULAR_INCLUSION, "Circular inclusion in line " + lineIndex );
        return ParseLineResult.RETURN_NULL;
      }

      if ( DoLogSourceInfo )
      {
        string subFilenameFull2 = GR.Path.Append( System.IO.Path.GetDirectoryName( ParentFilename ), subFilename );
        if ( !OrigLines.ContainsKey( subFilenameFull2 ) )
        {
          OrigLines.Add( subFilenameFull2, new string[Lines.Length] );
          Array.Copy( Lines, OrigLines[subFilenameFull2], Lines.Length );
        }
      }

      m_LoadedFiles[ParentFilename].Add( subFilename );

      string[]  subFile = null;
      string subFilenameFull = subFilename;

      if ( LibraryFile )
      {
        subFilenameFull = DetermineFullLibraryFilePath( subFilename );
        if ( string.IsNullOrEmpty( subFilenameFull ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1307_FILENAME_INCOMPLETE, "Can't find matching library file in line " + lineIndex );
          return ParseLineResult.RETURN_NULL;
        }
      }
      else
      {
        subFilenameFull = BuildFullPath( System.IO.Path.GetDirectoryName( ParentFilename ), subFilename );
      }

      if ( GR.Path.IsPathEqual( ParentFilename, subFilenameFull ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1400_CIRCULAR_INCLUSION, "Circular inclusion in line " + lineIndex );
        return ParseLineResult.RETURN_NULL;
      }

      ExternallyIncludedFiles.Add( subFilenameFull );
      //Debug.Log( "Read subfile " + subFilename );
      try
      {
        subFile = System.IO.File.ReadAllLines( subFilenameFull );
      }
      catch ( System.IO.IOException )
      {
        AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilenameFull );
        return ParseLineResult.RETURN_NULL;
      }

      CleanLines( subFile );
      if ( subFile.Length == 0 )
      {
        // included empty file messes up source info, skip to adding it
        return ParseLineResult.CALL_CONTINUE;
      }

      Types.ASM.SourceInfo sourceInfo = new Types.ASM.SourceInfo();
      sourceInfo.Filename = subFilenameFull;
      sourceInfo.FullPath = subFilenameFull;
      sourceInfo.GlobalStartLine = lineIndex;
      sourceInfo.LineCount = subFile.Length;
      sourceInfo.FilenameParent = ParentFilename;

      SourceInfoLog( "-include at global index " + lineIndex );
      SourceInfoLog( "-has " + subFile.Length + " lines" );

      InsertSourceInfo( sourceInfo );

      //string[] result = new string[Lines.Length + subFile.Length - 1];
      string[] result = new string[Lines.Length + subFile.Length];

      System.Array.Copy( Lines, 0, result, 0, lineIndex );
      System.Array.Copy( subFile, 0, result, lineIndex, subFile.Length );
      //System.Array.Copy( Lines, lineIndex + 1, result, lineIndex + subFile.Length, Lines.Length - lineIndex - 1 );

      // this keeps the !source line in the final code, makes working with source infos easier though
      System.Array.Copy( Lines, lineIndex, result, lineIndex + subFile.Length, Lines.Length - lineIndex );
      // replace !source with empty line (otherwise source infos would have one line more!)
      result[lineIndex + subFile.Length] = "";

      Lines = result;

      ASMFileInfo.LineInfo.Remove( lineIndex );

      --lineIndex;
      return ParseLineResult.CALL_CONTINUE;
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



    private ParseLineResult POIncludeBinary( List<Types.TokenInfo> lineTokenInfos, int lineIndex, Types.ASM.LineInfo info, out int lineSizeInBytes )
    {
      lineSizeInBytes = 0;


      int             paramPos = 0;
      List<Types.TokenInfo> paramsFile = new List<Types.TokenInfo>();
      List<Types.TokenInfo> paramsSize = new List<Types.TokenInfo>();
      List<Types.TokenInfo> paramsSkip = new List<Types.TokenInfo>();
      for ( int i = 1; i < lineTokenInfos.Count; ++i )
      {
        if ( lineTokenInfos[i].Content == "," )
        {
          ++paramPos;
          if ( paramPos > 2 )
          {
            AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Macro not formatted as expected. Expected !binary <Filename>,<Size>,<Skip>" );
            return ParseLineResult.RETURN_NULL;
          }
        }
        else
        {
          switch ( paramPos )
          {
            case 0:
              paramsFile.Add( lineTokenInfos[i] );
              break;
            case 1:
              paramsSize.Add( lineTokenInfos[i] );
              break;
            case 2:
              paramsSkip.Add( lineTokenInfos[i] );
              break;
          }
        }
      }
      if ( ( paramPos > 2 )
      ||   ( paramsFile.Count != 1 ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Macro not formatted as expected. Expected !binary <Filename>,<Size>,<Skip>" );
        return ParseLineResult.RETURN_NULL;
      }

      string subFilename = "";

      if ( m_AssemblerSettings.IncludeExpectsStringLiteral )
      {
        if ( ( !paramsFile[0].Content.StartsWith( "\"" ) )
        ||   ( !paramsFile[0].Content.EndsWith( "\"" ) ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1307_FILENAME_INCOMPLETE, "Expected proper file name between apostrophes" );
          return ParseLineResult.RETURN_NULL;
        }
        subFilename = paramsFile[0].Content.Substring( 1, paramsFile[0].Length - 2 );
      }
      else
      {
        subFilename = paramsFile[0].Content;
      }

      int     fileSize = -1;
      int     fileSkip = -1;
      bool    fileSizeValid = EvaluateTokens( lineIndex, paramsSize, out fileSize );
      bool    fileSkipValid = EvaluateTokens( lineIndex, paramsSkip, out fileSkip );

      if ( ( paramsSize.Count > 0 )
      &&   ( !fileSizeValid ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Cannot evaluate size argument" );
        return ParseLineResult.RETURN_NULL;
      }
      if ( ( paramsSkip.Count > 0 )
      &&   ( !fileSkipValid ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Cannot evaluate skip argument" );
        return ParseLineResult.RETURN_NULL;
      }

      // special case, allow 0 length as all bytes
      if ( ( fileSizeValid )
      && ( fileSize == 0 ) )
      {
        fileSizeValid = false;
      }

      GR.Memory.ByteBuffer    subFile = null;
      //byte[] subFile = null;



      try
      {
        subFile = GR.IO.File.ReadAllBytes( BuildFullPath( m_DocBasePath, subFilename ) );
        if ( subFile == null )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + GR.Path.Append( m_DocBasePath, subFilename ) );
          return ParseLineResult.RETURN_NULL;
        }
      }
      catch ( System.IO.IOException )
      {
        AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + GR.Path.Append( m_DocBasePath, subFilename ) );
        return ParseLineResult.RETURN_NULL;
      }
      ExternallyIncludedFiles.Add( GR.Path.Append( m_DocBasePath, subFilename ) );

      //StringBuilder   builder = new StringBuilder( subFile.Length * 4 );
      int             maxBytes = (int)subFile.Length;
      if ( !fileSkipValid )
      {
        fileSkip = 0;
      }
      if ( !fileSizeValid )
      {
        fileSize = maxBytes - fileSkip;
      }

      if ( fileSkip + fileSize > maxBytes )
      {
        // more bytes requested than the file holds
        // as ACME fills up with zeroes we follow along
        uint  bytesToAdd = (uint)( fileSkip + fileSize - maxBytes );
        subFile.Append( new GR.Memory.ByteBuffer( bytesToAdd ) );
      }
      if ( fileSkip > maxBytes )
      {
        AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Trying to skip more bytes than the file " + GR.Path.Append( m_DocBasePath, subFilename ) + " holds" );
        return ParseLineResult.RETURN_NULL;
      }

      info.LineData = subFile.SubBuffer( fileSkip, fileSize );
      info.NumBytes = (int)info.LineData.Length;
      lineSizeInBytes = fileSize;

      return ParseLineResult.OK;
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
        if ( Scopes[i].Loop != null )
        {
          return true;
        }
      }
      return false;
    }



    private ParseLineResult POCallMacro( List<Types.TokenInfo> lineTokenInfos, 
                                         ref int lineIndex, 
                                         Types.ASM.LineInfo info, 
                                         string parseLine,
                                         string ParentFilename, 
                                         string labelInFront,
                                         GR.Collections.Map<string, Types.MacroFunctionInfo> macroFunctions, 
                                         ref string[] Lines, 
                                         List<Types.ScopeInfo> Scopes, 
                                         out int lineSizeInBytes )
    {
      // +macro Macroname [param1[,param2]]
      lineSizeInBytes = 0;
      string functionName = lineTokenInfos[0].Content.Substring( 1 );
      if ( ( !macroFunctions.ContainsKey( functionName ) )
      ||   ( macroFunctions[functionName].LineEnd == -1 ) )
      {
        AddError( lineIndex, C64Studio.Types.ErrorCode.E1301_MACRO_UNKNOWN, "Unknown macro function " + functionName );
      }
      else
      {
        Types.MacroFunctionInfo  functionInfo = macroFunctions[functionName];

        List<string>  param = new List<string>();
        List<bool>    paramIsRef = new List<bool>();
        int           startIndex = 1;
        bool          hadError = false;

        for ( int i = 1; i < lineTokenInfos.Count; ++i )
        {
          if ( lineTokenInfos[i].Content == "," )
          {
            // separator
            // we're using a custom internal brace to not mix up opcode detection with expression parsing
            param.Add( AssemblerSettings.INTERNAL_OPENING_BRACE + parseLine.Substring( lineTokenInfos[startIndex].StartPos, lineTokenInfos[i].StartPos - lineTokenInfos[startIndex].StartPos ) + AssemblerSettings.INTERNAL_CLOSING_BRACE );

            // is reference properly matched?
            if ( param.Count > functionInfo.ParametersAreReferences.Count )
            {
              AddError( lineIndex, C64Studio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Referenced parameters are not matching macro definition" );
              hadError = true;
            }
            else if ( param[param.Count - 1].StartsWith( "~" ) != functionInfo.ParametersAreReferences[param.Count - 1] )
            {
              AddError( lineIndex, C64Studio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Referenced parameters are not matching macro definition" );
              hadError = true;
            }

            if ( ( !hadError )
            &&   ( functionInfo.ParametersAreReferences[param.Count - 1] ) )
            {
              param[param.Count - 1] = param[param.Count - 1].Substring( 1 );

              string paramName = param[param.Count - 1];

              if ( ASMFileInfo.UnparsedLabels.ContainsKey( paramName ) )
              {
                AddLabel( paramName, 0, lineIndex, info.Zone, -1, 0 );
              }
            }
            startIndex = i + 1;
          }
        }
        if ( ( startIndex == lineTokenInfos.Count )
        &&   ( startIndex > 1 ) )
        {
          param.Add( "" );
        }
        else if ( startIndex < lineTokenInfos.Count )
        {
          // why was there this substring statement?
          //param.Add( parseLine.Substring( lineTokenInfos[startIndex].StartPos ) );
          if ( lineTokenInfos.Count - startIndex == 1 )
          {
            param.Add( TokensToExpression( lineTokenInfos, startIndex, lineTokenInfos.Count - startIndex ) );
          }
          else
          {
            // braces so potential original operators are evaluated before the rest is
            param.Add( AssemblerSettings.INTERNAL_OPENING_BRACE + TokensToExpression( lineTokenInfos, startIndex, lineTokenInfos.Count - startIndex ) + AssemblerSettings.INTERNAL_CLOSING_BRACE );
          }
          //param.Add( lineTokenInfos[startIndex].Content );
          // is reference properly matched?
          if ( param.Count > functionInfo.ParametersAreReferences.Count )
          {
            AddError( lineIndex, C64Studio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Referenced parameters are not matching macro definition" );
            hadError = true;
          }
          else if ( param[param.Count - 1].StartsWith( "~" ) != functionInfo.ParametersAreReferences[param.Count - 1] )
          {
            AddError( lineIndex, C64Studio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Referenced parameters are not matching macro definition" );
            hadError = true;
          }
          if ( ( !hadError )
          &&   ( functionInfo.ParametersAreReferences[param.Count - 1] ) )
          {
            param[param.Count - 1] = param[param.Count - 1].Substring( 1 );
            string paramName = param[param.Count - 1];

            if ( ASMFileInfo.UnparsedLabels.ContainsKey( paramName ) )
            {
              AddLabel( paramName, 0, lineIndex, info.Zone, -1, 0 );
            }
          }
        }
        if ( ( !m_AssemblerSettings.MacrosHaveVariableNumberOfArguments )
        &&   ( param.Count != functionInfo.ParameterNames.Count ) )
        {
          AddError( lineIndex, C64Studio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Parameter count does not match" );
        }
        else if ( !hadError )
        {
          //string[] replacementLines = RelabelLocalLabels( functionInfo.Content );

          int lineIndexInMacro = -1;
          string[] replacementLines = RelabelLocalLabelsForMacro( Lines, Scopes, lineIndex, functionName, functionInfo, param, out lineIndexInMacro );
          if ( replacementLines == null )
          {
            AddError( lineIndexInMacro, C64Studio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Syntax error during macro replacement at position " + m_LastErrorInfo.Pos );
          }
          else
          {
            // insert macro code with clearing macro call line
            // readd label if there was one before the +macro
            //if ( labelInFront.Length > 0 )
            {
              // if label in front insert macro one line below!
              string[] newLines = new string[Lines.Length + replacementLines.Length];

              System.Array.Copy( Lines, 0, newLines, 0, lineIndex + 1 );
              System.Array.Copy( replacementLines, 0, newLines, lineIndex + 1, replacementLines.Length );
              if ( Lines.Length - lineIndex - 1 >= 1 )
              {
                System.Array.Copy( Lines, lineIndex + 1, newLines, lineIndex + 1 + replacementLines.Length, Lines.Length - lineIndex - 1 );
              }

              newLines[lineIndex] = labelInFront;

              // adjust source infos to make lookup work correctly
              Types.ASM.SourceInfo sourceInfo = new Types.ASM.SourceInfo();
              sourceInfo.Filename = functionInfo.ParentFileName;
              sourceInfo.FullPath = functionInfo.ParentFileName;
              sourceInfo.GlobalStartLine = lineIndex + 1;
              sourceInfo.LineCount = replacementLines.Length;
              string dummy;
              ASMFileInfo.FindTrueLineSource( functionInfo.LineIndex + 1, out dummy, out sourceInfo.LocalStartLine );

              //Debug.Log( "Add subfile section at " + ( LineOffset + lineIndex + 1 ) + " for " + ParentFilename + " with " + sourceInfo.LineCount + " lines" );
              InsertSourceInfo( sourceInfo );

              Lines = newLines;

              return ParseLineResult.CALL_CONTINUE;
            }
            /*
            else
            {
              string[] newLines = new string[Lines.Length + replacementLines.Length - 1];

              System.Array.Copy( Lines, 0, newLines, 0, lineIndex );
              System.Array.Copy( replacementLines, 0, newLines, lineIndex, replacementLines.Length );
              if ( Lines.Length - lineIndex - 1 >= 1 )
              {
                System.Array.Copy( Lines, lineIndex + 1, newLines, lineIndex + replacementLines.Length, Lines.Length - lineIndex - 1 );
              }

              // remove probably stored info on line
              ASMFileInfo.LineInfo.Remove( lineIndex );

              if ( replacementLines.Length > 0 )
              {
                // adjust source infos to make lookup work correctly
                Types.ASM.SourceInfo sourceInfo = new Types.ASM.SourceInfo();
                sourceInfo.Filename = ParentFilename;
                sourceInfo.FullPath = ParentFilename;
                sourceInfo.GlobalStartLine = lineIndex;
                sourceInfo.LineCount = replacementLines.Length;
                //sourceInfo.LocalStartLine = functionInfo.LineIndex + 1;
                string dummy;
                ASMFileInfo.FindTrueLineSource( functionInfo.LineIndex + 1, out dummy, out sourceInfo.LocalStartLine );

                sourceInfo.Filename = dummy;
                sourceInfo.FullPath = dummy;

                //Debug.Log( "Add subfile section at " + ( LineOffset + lineIndex + 1 ) + " for " + ParentFilename + " with " + sourceInfo.LineCount + " lines" );
                InsertSourceInfo( sourceInfo );
              }
              Lines = newLines;
            }*/

            /*
            // readd label if there was one before the +macro
            if ( labelInFront.Length > 0 )
            {
              // if label in front insert macro one line below!
              string[] newLines = new string[Lines.Length + replacementLines.Length];

              System.Array.Copy( Lines, 0, newLines, 0, lineIndex + 1  );
              System.Array.Copy( replacementLines, 0, newLines, lineIndex + 1, replacementLines.Length );
              if ( Lines.Length - lineIndex - 1 >= 1 )
              {
                System.Array.Copy( Lines, lineIndex + 1, newLines, lineIndex + 1 + replacementLines.Length, Lines.Length - lineIndex - 1 );
              }

              newLines[lineIndex] = labelInFront;

              // adjust source infos to make lookup work correctly
              Types.ASM.SourceInfo sourceInfo = new Types.ASM.SourceInfo();
              sourceInfo.Filename = ParentFilename;
              sourceInfo.FullPath = ParentFilename;
              sourceInfo.GlobalStartLine = lineIndex + 1;
              sourceInfo.LineCount = replacementLines.Length;
              string dummy;
              ASMFileInfo.FindTrueLineSource( functionInfo.LineIndex + 1, out dummy, out sourceInfo.LocalStartLine );

              //Debug.Log( "Add subfile section at " + ( LineOffset + lineIndex + 1 ) + " for " + ParentFilename + " with " + sourceInfo.LineCount + " lines" );
              InsertSourceInfo( sourceInfo );

              Lines = newLines;

              return ParseLineResult.CALL_CONTINUE;
            }
            else
            {
              string[] newLines = new string[Lines.Length + replacementLines.Length - 1];

              System.Array.Copy( Lines, 0, newLines, 0, lineIndex );
              System.Array.Copy( replacementLines, 0, newLines, lineIndex, replacementLines.Length );
              if ( Lines.Length - lineIndex - 1 >= 1 )
              {
                System.Array.Copy( Lines, lineIndex + 1, newLines, lineIndex + replacementLines.Length, Lines.Length - lineIndex - 1 );
              }

              // remove probably stored info on line
              ASMFileInfo.LineInfo.Remove( lineIndex );

              if ( replacementLines.Length > 0 )
              {
                // adjust source infos to make lookup work correctly
                Types.ASM.SourceInfo sourceInfo = new Types.ASM.SourceInfo();
                sourceInfo.Filename = ParentFilename;
                sourceInfo.FullPath = ParentFilename;
                sourceInfo.GlobalStartLine = lineIndex;
                sourceInfo.LineCount = replacementLines.Length;
                //sourceInfo.LocalStartLine = functionInfo.LineIndex + 1;
                string dummy;
                ASMFileInfo.FindTrueLineSource( functionInfo.LineIndex + 1, out dummy, out sourceInfo.LocalStartLine );

                sourceInfo.Filename = dummy;
                sourceInfo.FullPath = dummy;

                //Debug.Log( "Add subfile section at " + ( LineOffset + lineIndex + 1 ) + " for " + ParentFilename + " with " + sourceInfo.LineCount + " lines" );
                InsertSourceInfo( sourceInfo );
              }
              Lines = newLines;
            }
            --lineIndex;
            return ParseLineResult.CALL_CONTINUE;*/
          }
        }
      }
      return ParseLineResult.OK;
    }


    Dictionary<string,string[]>   OrigLines = null;



    private void IncludePreviousSymbols()
    {
      // include previous symbols
      if ( InitialFileInfo != null )
      {
        foreach ( var entry in InitialFileInfo.Labels )
        {
          if ( ( entry.Value.Type != C64Studio.Types.SymbolInfo.Types.PREPROCESSOR_CONSTANT_1 )
          &&   ( entry.Value.Type != C64Studio.Types.SymbolInfo.Types.PREPROCESSOR_CONSTANT_2 )
          &&   ( entry.Value.Type != C64Studio.Types.SymbolInfo.Types.PREPROCESSOR_LABEL ) )
          {
            if ( ( entry.Value.Type == C64Studio.Types.SymbolInfo.Types.LABEL )
            &&   ( entry.Key.StartsWith( InternalLabelPrefix ) ) )
            {
              // do not pass on internal local labels
              continue;
            }
            C64Studio.Types.SymbolInfo    symbol = new C64Studio.Types.SymbolInfo();
            symbol.AddressOrValue = entry.Value.AddressOrValue;
            symbol.DocumentFilename = entry.Value.DocumentFilename;
            symbol.LocalLineIndex = entry.Value.LocalLineIndex;
            symbol.Name = entry.Value.Name;
            symbol.Type = entry.Value.Type;
            symbol.Used = true;
            symbol.Zone = entry.Value.Zone;
            symbol.FromDependency = true;
            symbol.Info = entry.Value.Info;

            ASMFileInfo.Labels.Add( entry.Key, symbol );
          }
        }
      }
    }




    private string[] PreProcess( string[] Lines, string ParentFilename, ProjectConfig Configuration )
    {
      List<Types.ScopeInfo>   stackScopes = new List<C64Studio.Types.ScopeInfo>();
      //List<Types.LoopInfo>    loopList = new List<C64Studio.Types.LoopInfo>();
      GR.Collections.Map<string,Types.MacroFunctionInfo>     macroFunctions = new GR.Collections.Map<string,C64Studio.Types.MacroFunctionInfo>();

      ASMFileInfo.Labels.Clear();
      m_CurrentCommentSB = new StringBuilder();

      if ( DoLogSourceInfo )
      {
        OrigLines = new Dictionary<string, string[]>();
        OrigLines[ParentFilename] = new string[Lines.Length];
        Array.Copy( Lines, OrigLines[ParentFilename], Lines.Length );
      }

      IncludePreviousSymbols();

      ASMFileInfo.UnparsedLabels.Clear();
      ASMFileInfo.Zones.Clear();
      ASMFileInfo.LineInfo.Clear();
      ASMFileInfo.TempLabelInfo.Clear();

      stackScopes.Clear();
      Messages.Clear();
      
      m_WarningMessages = 0;
      m_ErrorMessages = 0;
      if ( Configuration != null )
      {
        ParseAndAddPreDefines( Configuration.Defines );
      }
      AddPreprocessorConstant( "ASSEMBLER_C64STUDIO", 1, -1 );

      GR.Collections.Map<byte, byte> textCodeMapping = m_TextCodeMappingRaw;
      Dictionary<string,string> previousMinusLabel = new Dictionary<string, string>();

      int   programStepPos = -1;
      int   lineSizeInBytes = 0;
      int   sizeInBytes = 0;
      m_CompileCurrentAddress = -1;
      int trueCompileCurrentAddress = -1;
      string cheapLabelParent = "";
      int   intermediateLineOffset = 0;
      bool  hadCommentInLine = false;
      bool hadMacro = false;

      //int lineIndex = 0;
      //foreach ( string line in Lines )
      for ( int lineIndex = 0; lineIndex < Lines.Length; ++lineIndex )
      {
        // there was a reason for this (leading spaces), but shouldn't be needed anymore
        //string parseLine = Lines[lineIndex].Trim();
        string parseLine = "";
        if ( Lines[lineIndex] != null )
        {
          parseLine = Lines[lineIndex].TrimEnd();
        }

        lineSizeInBytes = 0;
        hadCommentInLine = false;
        hadMacro = false;

        // TODO - damit geht ; in Strings auch nicht!
        int commentPos = -1;

        if ( FindStartOfComment( parseLine, out commentPos ) )
        {
          m_CurrentCommentSB.AppendLine( parseLine.Substring( commentPos + 1 ) );
          parseLine         = parseLine.Substring( 0, commentPos );
          hadCommentInLine  = true;
        }

        Types.ASM.LineInfo info = new Types.ASM.LineInfo();
        info.LineIndex      = lineIndex;
        info.Zone           = m_CurrentZoneName;
        info.CheapLabelZone = cheapLabelParent;
        info.AddressStart   = programStepPos;

        if ( ScopeInsideMacroDefinition( stackScopes ) )
        {
          // do not store code inside a macro definition
        }
        else
        {
          if ( ScopeInsideLoop( stackScopes ) )
          {
            // if a loop is repeated line infos may already exist
            if ( ASMFileInfo.LineInfo.ContainsKey( lineIndex ) )
            {
              // TODO - add second address lookup entry
              info = ASMFileInfo.LineInfo[lineIndex];

              info.AddressStart = programStepPos;
              info.Zone = m_CurrentZoneName;
            }
            else
            {
              ASMFileInfo.LineInfo.Add( lineIndex, info );
            }
          }
          else
          {
            ASMFileInfo.LineInfo.Add( lineIndex, info );
          }
        }

        List<Types.TokenInfo> lineTokenInfos = PrepareLineTokens( parseLine );
        if ( lineTokenInfos == null )
        {
          AddError( lineIndex, C64Studio.Types.ErrorCode.E1000_SYNTAX_ERROR, "Syntax error at position " + ( m_LastErrorInfo.Pos + 1 ).ToString() + " (" + parseLine[m_LastErrorInfo.Pos] + ")" );
          continue;
        }

        recheck_line:;

        // split lines by ':'
        int   localIndex = 0;
        string filename = "";
        if ( !ASMFileInfo.FindTrueLineSource( lineIndex, out filename, out localIndex ) )
        {
          DumpSourceInfos( OrigLines, Lines );
          AddError( lineIndex, Types.ErrorCode.E1401_INTERNAL_ERROR, "Can't determine filename from line" );
          return null;
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

        if ( !m_AssemblerSettings.CaseSensitive )
        {
          // turn all labels/macros to upper case
          foreach ( Types.TokenInfo token in lineTokenInfos )
          {
            if ( ( token.Type == Types.TokenInfo.TokenType.LABEL_GLOBAL )
            ||   ( token.Type == Types.TokenInfo.TokenType.LABEL_INTERNAL )
            ||   ( token.Type == Types.TokenInfo.TokenType.LABEL_CHEAP_LOCAL )
            ||   ( token.Type == Types.TokenInfo.TokenType.LABEL_LOCAL ) )
            {
              token.Content = token.Content.ToUpper();
            }
          }
        }


        // do we have a DASM scope operator? (must skip scope check then)
        bool  isDASMScopePseudoOP = false;
        int   tokenOffset = 0;
        if ( ( lineTokenInfos.Count > 1 )
        &&   ( !m_AssemblerSettings.Macros.ContainsKey( lineTokenInfos[0].Content.ToUpper() ) )
        &&   ( ( lineTokenInfos[0].Type == Types.TokenInfo.TokenType.LABEL_GLOBAL )
        ||     ( lineTokenInfos[0].Type == Types.TokenInfo.TokenType.LABEL_INTERNAL )
        ||     ( lineTokenInfos[0].Type == Types.TokenInfo.TokenType.LABEL_CHEAP_LOCAL )
        ||     ( lineTokenInfos[0].Type == Types.TokenInfo.TokenType.LABEL_LOCAL ) ) )
        {
          ++tokenOffset;
        }
        if ( ( ( lineTokenInfos[0].Type == Types.TokenInfo.TokenType.LABEL_GLOBAL )
        ||     ( lineTokenInfos[0].Type == Types.TokenInfo.TokenType.LABEL_INTERNAL )
        ||     ( lineTokenInfos[0].Type == Types.TokenInfo.TokenType.LABEL_CHEAP_LOCAL )
        ||     ( lineTokenInfos[0].Type == Types.TokenInfo.TokenType.LABEL_LOCAL ) )
        &&   ( m_AssemblerSettings.Macros.ContainsKey( lineTokenInfos[tokenOffset].Content.ToUpper() ) ) )
        {
          var macroInfo = m_AssemblerSettings.Macros[lineTokenInfos[tokenOffset].Content.ToUpper()];
          if ( ( macroInfo.Type == C64Studio.Types.MacroInfo.MacroType.IF )
          ||   ( macroInfo.Type == C64Studio.Types.MacroInfo.MacroType.ELSE )
          ||   ( macroInfo.Type == C64Studio.Types.MacroInfo.MacroType.END_IF ) )
          {
            isDASMScopePseudoOP = true;
          }
        }


        if ( ( lineTokenInfos.Count > 0 )
        &&   ( lineTokenInfos[0].Content != "}" )
        &&   ( !isDASMScopePseudoOP ) )
        {
          bool isActive = true;
          for ( int i = 0; i < stackScopes.Count; ++i )
          {
            if ( !stackScopes[i].Active )
            {
              isActive = false;
              break;
            }
          }
          if ( !isActive )
          {
            // defined away
            Types.ScopeInfo.ScopeType   detectedScopeType = ScopeInfo.ScopeType.UNKNOWN;

            // TODO - HACK UGLY - use keywords from AssemblerSettings!
            if ( TokenIsConditionalThatStartsScope( lineTokenInfos[0] ) )
            {
              // a new block starts here!
              // false, since it doesn't matter
              Types.ScopeInfo scope = new C64Studio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.IF_OR_IFDEF );
              scope.StartIndex = lineIndex;
              scope.Active = false;
              stackScopes.Add( scope );

              OnScopeAdded( scope );
            }
            else if ( ( TokenIsPseudoPC( lineTokenInfos[0] ) )
            &&        ( lineTokenInfos[lineTokenInfos.Count - 1].Content == "{" ) )
            {
              // ACME style pseudo pc with bracket
              Types.ScopeInfo scope = new ScopeInfo( Types.ScopeInfo.ScopeType.PSEUDO_PC );
              scope.StartIndex = lineIndex;
              scope.Active = false;
              stackScopes.Add( scope );
              OnScopeAdded( scope );
            }
            else if ( ( TokenStartsScope( lineTokenInfos[0], out detectedScopeType ) )
            &&        ( lineTokenInfos[lineTokenInfos.Count - 1].Content == "{" ) )
            {
              // ACME style other scopes with bracket
              Types.ScopeInfo scope = new C64Studio.Types.ScopeInfo( detectedScopeType );
              scope.StartIndex = lineIndex;
              scope.Active = false;
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
          if ( !previousMinusLabel.ContainsKey( lineTokenInfos[0].Content ) )
          {
            previousMinusLabel.Add( lineTokenInfos[0].Content, InternalLabelPrefix + lineTokenInfos[0].Content + InternalLabelPostfix + lineIndex.ToString() );
          }
          else
          {
            previousMinusLabel[lineTokenInfos[0].Content] = InternalLabelPrefix + lineTokenInfos[0].Content + InternalLabelPostfix + lineIndex.ToString();
          }
        }
        // identify internal labels as such
        if ( ( lineTokenInfos[0].Type != C64Studio.Types.TokenInfo.TokenType.CALL_MACRO )
        &&   ( ( lineTokenInfos[0].Content.StartsWith( "-" ) )
        ||     ( lineTokenInfos[0].Content.StartsWith( "+" ) ) ) )
        {
          lineTokenInfos[0].Content = InternalLabelPrefix + lineTokenInfos[0].Content + InternalLabelPostfix + lineIndex.ToString();
          lineTokenInfos[0].Type = TokenInfo.TokenType.LABEL_INTERNAL;
        }

        string  upToken = lineTokenInfos[0].Content.ToUpper();

        // if PDS global labels automatically work as zone
        if ( m_AssemblerSettings.GlobalLabelsAutoZone )
        {
          if ( ( lineTokenInfos[0].Type != C64Studio.Types.TokenInfo.TokenType.CALL_MACRO )
          &&   ( ( !m_Processor.Opcodes.ContainsKey( upToken.ToLower() ) )
          &&     ( ( ( m_AssemblerSettings.MacroPrefix.Length == 0 )
          &&       ( !m_AssemblerSettings.Macros.ContainsKey( upToken ) ) )
          ||     ( ( m_AssemblerSettings.MacroPrefix.Length > 0 )
          &&       ( !upToken.StartsWith( m_AssemblerSettings.MacroPrefix ) ) ) ) ) )
          {
            if ( lineTokenInfos[0].Type == Types.TokenInfo.TokenType.LABEL_GLOBAL )
            {
              // auto-zone
              m_CurrentZoneName = lineTokenInfos[0].Content;
              info.Zone = m_CurrentZoneName;
            }
          }
        }

        //prefix zone to local labels
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
            lineTokenInfos[i].Content = m_CurrentZoneName + lineTokenInfos[i].Content;
            if ( i == 0 )
            {
              upToken = lineTokenInfos[i].Content.ToUpper();
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

        string labelInFront = "";
        Types.TokenInfo tokenInFront = null;

        if ( ( lineTokenInfos.Count >= 3 )
        &&   ( m_AssemblerSettings.DefineSeparatorKeywords.ContainsValue( lineTokenInfos[1].Content )
        &&   ( ( m_AssemblerSettings.MacroPrefix.Length == 0 )
        ||     ( !lineTokenInfos[0].Content.StartsWith( m_AssemblerSettings.MacroPrefix ) ) ) ) )
        {
          // a define
          if ( ScopeInsideMacroDefinition( stackScopes ) )
          {
            continue;
          }


          int equPos = lineTokenInfos[1].StartPos;
          //string defineName = parseLine.Substring( 0, equPos ).Trim();
          string defineName = lineTokenInfos[0].Content;
          if ( !m_AssemblerSettings.CaseSensitive )
          {
            defineName = defineName.ToUpper();
          }
          int   defineLength = lineTokenInfos[lineTokenInfos.Count - 1].StartPos + lineTokenInfos[lineTokenInfos.Count - 1].Length - ( equPos + lineTokenInfos[1].Content.Length );
          string defineValue = TokensToExpression( lineTokenInfos, 2, lineTokenInfos.Count - 2 );
            //parseLine.Substring( equPos + lineTokenInfos[1].Content.Length, defineLength ).Trim();

          List<Types.TokenInfo>  valueTokens = ParseTokenInfo( defineValue, 0, defineValue.Length );
          int address = -1;

          /*
          if ( lineTokenInfos[0].Type == C64Studio.Types.TokenInfo.TokenType.LABEL_LOCAL )
          {
            defineName = zoneName + defineName;
          }*/

          if ( defineName == "*" )
          {
            // set program step
            int     newStepPos = 0;

            List<Types.TokenInfo> tokens = ParseTokenInfo( defineValue, 0, defineValue.Length );
            if ( !EvaluateTokens( lineIndex, tokens, out newStepPos ) )
            {
              AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate * position value", lineTokenInfos[0].StartPos, lineTokenInfos[0].Length );
              return null;
            }
            programStepPos = newStepPos;
            m_CompileCurrentAddress = programStepPos;
            trueCompileCurrentAddress = programStepPos;

            info.AddressStart = programStepPos;
          }
          else
          {
            if ( ScopeInsideMacroDefinition( stackScopes ) )
            {
              continue;
            }
            if ( !EvaluateTokens( lineIndex, valueTokens, out address ) )
            {
              if ( defineName == "*" )
              {
                AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Cannot evaluate expression for *", lineTokenInfos[0].StartPos, lineTokenInfos[0].Length );
              }
              else
              {
                AddUnparsedLabel( defineName, defineValue, lineIndex );
              }
            }
            else 
            {
              AddConstant( defineName, address, lineIndex, m_CurrentCommentSB.ToString(), m_CurrentZoneName, lineTokenInfos[0].StartPos, lineTokenInfos[0].Length );
              if ( defineName == "*" )
              {
                if ( ( address >= 0 )
                &&   ( address <= 0xffff ) )
                {
                  AddError( lineIndex, Types.ErrorCode.E1003_VALUE_OUT_OF_BOUNDS_WORD, "Evaluated constant out of bounds, " + address + " must be >= 0 and <= 65535", lineTokenInfos[0].StartPos, lineTokenInfos[0].Length );
                }
                else
                {
                  programStepPos = address;
                  trueCompileCurrentAddress = programStepPos;
                  info.AddressSource = "*";
                  //Debug.Log( "Set * to " + address.ToString( "X4" ) );
                }
              }
            }
          }
          m_CurrentCommentSB = new StringBuilder();
          continue;
        }
        else if ( ( lineTokenInfos[0].Type != C64Studio.Types.TokenInfo.TokenType.CALL_MACRO )
        &&        ( ( !m_Processor.Opcodes.ContainsKey( upToken.ToLower() ) )
        &&           ( ( ( m_AssemblerSettings.MacroPrefix.Length == 0 )
        &&               ( !m_AssemblerSettings.Macros.ContainsKey( upToken ) ) )
        ||             ( ( m_AssemblerSettings.MacroPrefix.Length > 0 )
        &&               ( !upToken.StartsWith( m_AssemblerSettings.MacroPrefix ) ) ) ) ) )
        {
          // not a token, not a macro, must be a label in front
          if ( upToken == "}" )
          {
            if ( stackScopes.Count == 0 )
            {
              AddError( lineIndex, Types.ErrorCode.E1004_MISSING_OPENING_BRACKET, "Missing opening brace" );
            }
            else
            {
              Types.ScopeInfo   closingScope = stackScopes[stackScopes.Count - 1];

              switch ( closingScope.Type )
              {
                case Types.ScopeInfo.ScopeType.MACRO_FUNCTION:
                  if ( closingScope.Macro != null )
                  {
                    if ( lineTokenInfos.Count != 1 )
                    {
                      AddError( lineIndex, C64Studio.Types.ErrorCode.E1000_SYNTAX_ERROR, "Closing brace must be single element" );
                      continue;
                    }
                    var result = HandleScopeEnd( macroFunctions, stackScopes, ref lineIndex, ref intermediateLineOffset, ref Lines );
                    if ( result == ParseLineResult.CALL_CONTINUE )
                    {
                      --lineIndex;
                      continue;
                    }
                    else if ( result == ParseLineResult.ERROR_ABORT )
                    {
                      return null;
                    }
                  }
                  break;
                case Types.ScopeInfo.ScopeType.LOOP:
                  if ( closingScope.Loop != null )
                  {
                    if ( lineTokenInfos.Count != 1 )
                    {
                      AddError( lineIndex, C64Studio.Types.ErrorCode.E1000_SYNTAX_ERROR, "Closing brace must be single element" );
                      continue;
                    }
                    OnScopeRemoved( lineIndex, stackScopes );
                    stackScopes.RemoveAt( stackScopes.Count - 1 );
                    var result = HandleScopeEnd( macroFunctions, stackScopes, ref lineIndex, ref intermediateLineOffset, ref Lines );
                    if ( result == ParseLineResult.CALL_CONTINUE )
                    {
                      --lineIndex;
                      continue;
                    }
                    else if ( result == ParseLineResult.ERROR_ABORT )
                    {
                      return null;
                    }
                  }
                  break;
                case Types.ScopeInfo.ScopeType.ZONE:
                  if ( lineTokenInfos.Count != 1 )
                  {
                    AddError( lineIndex, C64Studio.Types.ErrorCode.E1000_SYNTAX_ERROR, "Closing brace must be single element" );
                    continue;
                  }
                  OnScopeRemoved( lineIndex, stackScopes );
                  stackScopes.RemoveAt( stackScopes.Count - 1 );
                  m_CurrentZoneName = "";
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
                  &&   ( lineTokenInfos[0].Content == "}" )
                  &&   ( lineTokenInfos[2].Content == "{" )
                  &&   ( lineTokenInfos[1].Content.ToUpper() == "ELSE" ) )
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
                  &&        ( lineTokenInfos[0].Content == "}" )
                  &&        ( lineTokenInfos[lineTokenInfos.Count - 1].Content == "{" )
                  &&        ( lineTokenInfos[1].Content.ToUpper() == "ELSE" )
                  &&        ( lineTokenInfos[2].Content.ToUpper() == "IF" ) )
                  {
                    if ( !ScopeInsideMacroDefinition( stackScopes ) )
                    {
                      // else if

                      // end previous block
                      var prevScope = stackScopes[stackScopes.Count - 1];
                      stackScopes.RemoveAt( stackScopes.Count - 1 );

                      // start new block
                      int defineResult = -1;

                      Types.ScopeInfo scope = new C64Studio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.IF_OR_IFDEF );
                      scope.StartIndex = lineIndex;
                      if ( !EvaluateTokens( lineIndex, lineTokenInfos, 3, lineTokenInfos.Count - 3 - 1, out defineResult ) )
                      {
                        AddError( lineIndex, C64Studio.Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate expression: "
                                  + TokensToExpression( lineTokenInfos, 3, lineTokenInfos.Count - 3 - 1 ),
                                  lineTokenInfos[3].StartPos, lineTokenInfos[lineTokenInfos.Count - 1].EndPos + 1 - lineTokenInfos[3].StartPos );
                        scope.Active = true;
                        scope.IfChainHadActiveEntry = true;
                      }
                      else if ( defineResult == 0 )
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
              //++lineIndex;
              continue;
            }
          }
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
              AddLabel( labelInFront, -1, lineIndex, m_CurrentZoneName, lineTokenInfos[0].StartPos, lineTokenInfos[0].Length );
              //AddError( lineIndex, Types.ErrorCode.E0002_CODE_WITHOUT_START_ADDRESS, "Can't provide value if no start address is set", lineTokenInfos[0].StartPos, lineTokenInfos[0].Length );
            }
          }

          // cut off label for neededparsedexpression
          if ( lineTokenInfos.Count > 1 )
          {
            info.Line = parseLine.Substring( lineTokenInfos[1].StartPos );
            parseLine = parseLine.Substring( lineTokenInfos[1].StartPos );

            // shift all tokens back
            lineTokenInfos = ParseTokenInfo( parseLine, 0, parseLine.Length );
            // insert dummy entry to be removed later
            lineTokenInfos.Insert( 0, new TokenInfo() );
          }
          else
          {
            parseLine = "";
            info.Line = "";
          }
          lineTokenInfos.RemoveAt( 0 );
          if ( lineTokenInfos.Count == 0 )
          {
            upToken = "";
          }
          else
          {
            upToken = lineTokenInfos[0].Content.ToUpper();

            // hack - macro call after label
            if ( ( lineTokenInfos.Count >= 2 )
            &&   ( lineTokenInfos[0].Type == C64Studio.Types.TokenInfo.TokenType.OPERATOR )
            &&   ( lineTokenInfos[0].Content == "+" )
            &&   ( lineTokenInfos[0].StartPos + lineTokenInfos[0].Length == lineTokenInfos[1].StartPos )
            &&   ( lineTokenInfos[1].Type == C64Studio.Types.TokenInfo.TokenType.LABEL_GLOBAL ) )
            {
              lineTokenInfos[0].Type = C64Studio.Types.TokenInfo.TokenType.CALL_MACRO;
              lineTokenInfos[0].Content = "+" + lineTokenInfos[1].Content;
              lineTokenInfos[0].Length += lineTokenInfos[1].Length;
              lineTokenInfos.RemoveAt( 1 );
              upToken = lineTokenInfos[0].Content.ToUpper();
            }
          }
        }
        if ( ( lineTokenInfos.Count > 0 )
        &&   ( ( lineTokenInfos[0].Type == C64Studio.Types.TokenInfo.TokenType.OPCODE )
        ||     ( lineTokenInfos[0].Type == C64Studio.Types.TokenInfo.TokenType.OPCODE_DIRECT_VALUE )
        ||     ( lineTokenInfos[0].Type == C64Studio.Types.TokenInfo.TokenType.OPCODE_FIXED_NON_ZP )
        ||     ( lineTokenInfos[0].Type == C64Studio.Types.TokenInfo.TokenType.OPCODE_FIXED_ZP ) ) )
        {
          List<Tiny64.Opcode> possibleOpcodes = new List<Tiny64.Opcode>( m_Processor.Opcodes[upToken.ToLower()] );

          //Debug.Log( "TODO - if either ZP option is active dismiss unfitting possible opcodes" );

          if ( lineTokenInfos[0].Type == C64Studio.Types.TokenInfo.TokenType.OPCODE_FIXED_NON_ZP )
          {
            // dismiss any zp based opcodes
            for ( int i = 0; i < possibleOpcodes.Count; ++i )
            {
              var     opcode = possibleOpcodes[i];

              if ( ( opcode.Addressing == Tiny64.Opcode.AddressingType.ZEROPAGE )
              ||   ( opcode.Addressing == Tiny64.Opcode.AddressingType.ZEROPAGE_X )
              ||   ( opcode.Addressing == Tiny64.Opcode.AddressingType.ZEROPAGE_Y ) )
              {
                possibleOpcodes.RemoveAt( i );
                --i;
                continue;
              }
            }
          }
          else if ( lineTokenInfos[0].Type == C64Studio.Types.TokenInfo.TokenType.OPCODE_DIRECT_VALUE )
          {
            // dismiss any zp based opcodes
            for ( int i = 0; i < possibleOpcodes.Count; ++i )
            {
              var     opcode = possibleOpcodes[i];

              if ( opcode.Addressing != Tiny64.Opcode.AddressingType.IMMEDIATE )
              {
                possibleOpcodes.RemoveAt( i );
                --i;
                continue;
              }
            }
          }
          else if ( lineTokenInfos[0].Type == C64Studio.Types.TokenInfo.TokenType.OPCODE_FIXED_ZP )
          {
            // dismiss any non zp based opcodes
            for ( int i = 0; i < possibleOpcodes.Count; ++i )
            {
              var     opcode = possibleOpcodes[i];

              if ( ( opcode.Addressing != Tiny64.Opcode.AddressingType.ZEROPAGE )
              &&   ( opcode.Addressing != Tiny64.Opcode.AddressingType.ZEROPAGE_X )
              &&   ( opcode.Addressing != Tiny64.Opcode.AddressingType.ZEROPAGE_Y ) )
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
                      C64Studio.Types.ErrorCode.E1105_INVALID_OPCODE, 
                      "Cannot deduce matching opcode from zero page settings",
                      lineTokenInfos[0].StartPos,
                      lineTokenInfos[0].Length );
            continue;
          }

          info.Line = parseLine;
          Tiny64.Opcode estimatedOpcode = EstimateOpcode( lineIndex, lineTokenInfos, possibleOpcodes, ref info );
          if ( estimatedOpcode != null )
          {
            //dh.Log( "Found Token " + opcode.Mnemonic + ", size " + info.NumBytes.ToString() + " in line " + parseLine );
            info.NumBytes = estimatedOpcode.NumOperands + 1;
            info.Opcode   = estimatedOpcode;
          }

          if ( info.Opcode != null )
          {
            if ( info.Opcode.NumOperands == 0 )
            {
              if ( info.LineData == null )
              {
                info.LineData = new GR.Memory.ByteBuffer();
              }
              info.LineData.AppendU8( (byte)info.Opcode.ByteValue );
              info.NeededParsedExpression = null;
            }
            else if ( info.Opcode.NumOperands == 1 )
            {
              if ( info.LineData == null )
              {
                info.LineData = new GR.Memory.ByteBuffer();
              }
              info.LineData.AppendU8( (byte)info.Opcode.ByteValue );
              int byteValue = -1;

              // strip prefixed #
              if ( info.Opcode.Addressing == Tiny64.Opcode.AddressingType.IMMEDIATE )
              {
                if ( lineTokenInfos[1].Content.StartsWith( "#" ) )
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

              if ( EvaluateTokens( lineIndex, lineTokenInfos, 1, lineTokenInfos.Count - 1, out byteValue ) )
              {
                if ( info.Opcode.Addressing == Tiny64.Opcode.AddressingType.RELATIVE )
                {
                  int delta = byteValue - info.AddressStart - 2;
                  if ( ( delta < -128 )
                  ||   ( delta > 127 ) )
                  {
                    AddError( lineIndex, 
                              Types.ErrorCode.E1100_RELATIVE_JUMP_TOO_FAR, 
                              "Relative jump too far, trying to jump " + delta + " bytes",
                              lineTokenInfos[1].StartPos,
                              lineTokenInfos[lineTokenInfos.Count - 1].EndPos + 1 - lineTokenInfos[1].StartPos );
                    info.LineData.AppendU8( 0 );
                  }
                  else
                  {
                    info.LineData.AppendU8( (byte)delta );
                  }
                }
                else if ( info.Opcode.Addressing == Tiny64.Opcode.AddressingType.IMMEDIATE )
                {
                  if ( !ValidByteValue( byteValue ) )
                  {
                    AddError( lineIndex,
                              Types.ErrorCode.E1002_VALUE_OUT_OF_BOUNDS_BYTE,
                              "Value out of bounds for byte, needs to be >= -128 and <= 255. Expression:" + TokensToExpression( lineTokenInfos, 1, lineTokenInfos.Count - 1 ),
                              lineTokenInfos[1].StartPos,
                              lineTokenInfos[lineTokenInfos.Count - 1].EndPos + 1 - lineTokenInfos[1].StartPos );

                    info.LineData.AppendU8( 0 );
                  }
                  else
                  {
                    info.LineData.AppendU8( (byte)byteValue );
                  }
                }
                else
                {
                  info.LineData.AppendU8( (byte)byteValue );
                }
                info.NeededParsedExpression = null;
              }
              else
              {
                info.NeededParsedExpression = lineTokenInfos.GetRange( 1, lineTokenInfos.Count - 1 );
              }
            }
            else if ( info.Opcode.NumOperands == 2 )
            {
              if ( info.LineData == null )
              {
                info.LineData = new GR.Memory.ByteBuffer();
              }
              info.LineData.AppendU8( (byte)info.Opcode.ByteValue );
              int byteValue = -1;

              int countTokens = lineTokenInfos.Count - 1;
              // discard prepended ,x or ,y
              if ( ( info.Opcode.Addressing == Tiny64.Opcode.AddressingType.ABSOLUTE_X )
              ||   ( info.Opcode.Addressing == Tiny64.Opcode.AddressingType.ABSOLUTE_Y )
              ||   ( info.Opcode.Addressing == Tiny64.Opcode.AddressingType.ZEROPAGE_X )
              ||   ( info.Opcode.Addressing == Tiny64.Opcode.AddressingType.ZEROPAGE_Y )
              ||   ( info.Opcode.Addressing == Tiny64.Opcode.AddressingType.INDIRECT_X )
              ||   ( info.Opcode.Addressing == Tiny64.Opcode.AddressingType.INDIRECT_Y ) )
              {
                countTokens -= 2;
              }
              if ( EvaluateTokens( lineIndex, lineTokenInfos, 1, countTokens, out byteValue ) )
              {
                if ( ( info.Opcode.ByteValue == 0x6C )
                &&   ( m_Processor.Name == "6510" )
                &&   ( ( byteValue & 0xff ) == 0xff ) )
                {
                  AddWarning( lineIndex,
                              Types.ErrorCode.W0007_POTENTIAL_PROBLEM,
                              "A indirect JMP with an address ending on 0xff will not work as expected on NMOS CPUs",
                              lineTokenInfos[1].StartPos,
                              lineTokenInfos[lineTokenInfos.Count - 1].EndPos + 1 - lineTokenInfos[1].StartPos );
                }

                if ( ( !m_CompileConfig.AutoTruncateLiteralValues )
                &&   ( ( byteValue < 0 )
                ||     ( byteValue >= 65536 ) ) )
                {
                  AddError( lineIndex,
                            Types.ErrorCode.E1003_VALUE_OUT_OF_BOUNDS_WORD,
                            "Value $" + byteValue.ToString( "X" ) + " (" + byteValue + ") is out of bounds",
                            lineTokenInfos[1].StartPos,
                            lineTokenInfos[1 + countTokens - 1].EndPos + 1 - lineTokenInfos[1].StartPos );
                }
                info.LineData.AppendU16( (ushort)byteValue );
                info.NeededParsedExpression = null;
              }
              else
              {
                // TODO - could be better, why save all, check and trunc later??
                info.NeededParsedExpression = lineTokenInfos.GetRange( 1, lineTokenInfos.Count - 1 );
              }
            }

            if ( ScopeInsideMacroDefinition( stackScopes ) )
            {
              info.LineData = null;
              info.NumBytes = 0;
              info.Opcode = null;
              info.NeededParsedExpression = null;
            }

            if ( info.NeededParsedExpression != null )
            {
              // remove unneeded tokens, depending on opcode
              //Debug.Log( "needed expression for " + info.Line );
              switch ( info.Opcode.Addressing )
              {
                case Tiny64.Opcode.AddressingType.ABSOLUTE_X:
                case Tiny64.Opcode.AddressingType.ZEROPAGE_X:
                  if ( ( info.NeededParsedExpression.Count < 2 )
                  ||   ( info.NeededParsedExpression[info.NeededParsedExpression.Count - 2].Content != "," )
                  ||   ( info.NeededParsedExpression[info.NeededParsedExpression.Count - 1].Content.ToUpper() != "X" ) )
                  {
                    AddError( lineIndex, 
                              Types.ErrorCode.E1305_EXPECTED_TRAILING_SYMBOL, 
                              "Expected trailing ,x",
                              info.NeededParsedExpression[0].StartPos,
                              info.NeededParsedExpression[info.NeededParsedExpression.Count - 1].EndPos + 1 - info.NeededParsedExpression[0].StartPos );
                  }
                  else
                  {
                    info.NeededParsedExpression.RemoveRange( info.NeededParsedExpression.Count - 2, 2 );
                  }
                  break;
                case Tiny64.Opcode.AddressingType.INDIRECT_X:
                  if ( ( info.NeededParsedExpression.Count < 4 )
                  ||   ( !IsOpeningBraceChar( info.NeededParsedExpression[0].Content )
                  ||   ( info.NeededParsedExpression[info.NeededParsedExpression.Count - 3].Content != "," )
                  ||   ( info.NeededParsedExpression[info.NeededParsedExpression.Count - 2].Content.ToUpper() != "X" )
                  ||   ( !IsClosingBraceChar( info.NeededParsedExpression[info.NeededParsedExpression.Count - 1].Content ) ) ) )
                  {
                    if ( ( info.NeededParsedExpression.Count < 4 )
                    ||   ( !IsOpeningBraceChar( info.NeededParsedExpression[0].Content )
                    ||   ( !IsClosingBraceChar( info.NeededParsedExpression[info.NeededParsedExpression.Count - 3].Content ) )
                    ||   ( info.NeededParsedExpression[info.NeededParsedExpression.Count - 2].Content != "," )
                    ||   ( info.NeededParsedExpression[info.NeededParsedExpression.Count - 1].Content.ToUpper() != "X" ) ) )
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
                case Tiny64.Opcode.AddressingType.ABSOLUTE_Y:
                case Tiny64.Opcode.AddressingType.ZEROPAGE_Y:
                case Tiny64.Opcode.AddressingType.INDIRECT_Y:
                  // in case of case Opcode.AddressingType.INDIRECT_Y the brackets are parsed out already!
                  if ( ( info.NeededParsedExpression.Count < 2 )
                  ||   ( info.NeededParsedExpression[info.NeededParsedExpression.Count - 2].Content != "," )
                  ||   ( info.NeededParsedExpression[info.NeededParsedExpression.Count - 1].Content.ToUpper() != "Y" ) )
                  {
                    AddError( lineIndex, 
                              Types.ErrorCode.E1305_EXPECTED_TRAILING_SYMBOL, 
                              "Expected trailing ,y",
                              info.NeededParsedExpression[0].StartPos,
                              info.NeededParsedExpression[info.NeededParsedExpression.Count - 1].EndPos + 1 - info.NeededParsedExpression[0].StartPos );
                  }
                  else
                  {
                    info.NeededParsedExpression.RemoveRange( info.NeededParsedExpression.Count - 2, 2 );
                  }
                  break;
              }
              if ( ( info.NeededParsedExpression.Count == 1 )
              &&   ( info.NeededParsedExpression[0].Content.StartsWith( "-" ) ) )
              {
                if ( previousMinusLabel.ContainsKey( info.NeededParsedExpression[0].Content ) )
                {
                  info.NeededParsedExpression[0].Content = previousMinusLabel[info.NeededParsedExpression[0].Content];
                }
              }
            }
          }
          lineSizeInBytes = info.NumBytes;
          evaluatedContent = true;
        }

        // macro function
        if ( ( lineTokenInfos.Count > 0 )
        &&   ( lineTokenInfos[0].Type == C64Studio.Types.TokenInfo.TokenType.CALL_MACRO ) )
        {
          evaluatedContent = true;
          hadMacro = true;

          if ( !ScopeInsideMacroDefinition( stackScopes ) )
          {
            var result = POCallMacro( lineTokenInfos, ref lineIndex, info, parseLine, ParentFilename, labelInFront, macroFunctions, ref Lines, stackScopes, out lineSizeInBytes );
            if ( result == ParseLineResult.CALL_CONTINUE )
            {
              continue;
            }
          }
        }
        else if ( ( m_AssemblerSettings.MacroPrefix.Length > 0 )
        &&        ( upToken.StartsWith( m_AssemblerSettings.MacroPrefix ) ) )
        {
          // a macro
          hadMacro = true;
          if ( !m_AssemblerSettings.Macros.ContainsKey( upToken ) )
          {
            string    tokenToDisplay = lineTokenInfos[0].Content;
            if ( ( lineTokenInfos.Count > 1 )
            &&   ( lineTokenInfos[0].EndPos + 1 == lineTokenInfos[1].StartPos ) )
            {
              tokenToDisplay += lineTokenInfos[1].Content;
            }
            AddWarning( lineIndex, 
                        Types.ErrorCode.E1301_MACRO_UNKNOWN,
                        "Unsupported macro " + tokenToDisplay + ", this might result in a broken build",
                        lineTokenInfos[0].StartPos,
                        tokenToDisplay.Length );
          }
          else
          {
            Types.MacroInfo   macro = m_AssemblerSettings.Macros[upToken];
            if ( macro.Type == Types.MacroInfo.MacroType.END_OF_FILE )
            {
              break;
            }
            else if ( macro.Type == C64Studio.Types.MacroInfo.MacroType.TRACE )
            {
              string  traceFilename;
              int     localLineIndex = -1;
              if ( ASMFileInfo.FindTrueLineSource( lineIndex, out traceFilename, out localLineIndex ) )
              {
                AddVirtualBreakpoint( localLineIndex, traceFilename, TokensToExpression( lineTokenInfos, 1, lineTokenInfos.Count - 1 ) );
              }
            }
            else if ( macro.Type == Types.MacroInfo.MacroType.ERROR )
            {
              AddError( lineIndex, Types.ErrorCode.E1308_USER_ERROR, EvaluateAsText( lineIndex, lineTokenInfos, 1, lineTokenInfos.Count - 1 ) );
            }
            else if ( macro.Type == Types.MacroInfo.MacroType.WARN )
            {
              AddWarning( lineIndex,
                          Types.ErrorCode.W0005_USER_WARNING,
                          EvaluateAsText( lineIndex, lineTokenInfos, 1, lineTokenInfos.Count - 1 ),
                          lineTokenInfos[1].StartPos,
                          lineTokenInfos[lineTokenInfos.Count - 1].EndPos + 1 - lineTokenInfos[1].StartPos );
            }
            else if ( macro.Type == Types.MacroInfo.MacroType.MESSAGE )
            {
              AddOutputMessage( lineIndex, EvaluateAsText( lineIndex, lineTokenInfos, 1, lineTokenInfos.Count - 1 ) );
            }
            else if ( macro.Type == Types.MacroInfo.MacroType.SET )
            {
              if ( ( lineTokenInfos.Count >= 4 )
              && ( m_AssemblerSettings.DefineSeparatorKeywords.ContainsValue( lineTokenInfos[2].Content )
              && ( ( m_AssemblerSettings.MacroPrefix.Length == 0 )
              || ( !lineTokenInfos[1].Content.StartsWith( m_AssemblerSettings.MacroPrefix ) ) ) ) )
              {
                if ( ScopeInsideMacroDefinition( stackScopes ) )
                {
                  continue;
                }
                // a define
                int equPos = lineTokenInfos[2].StartPos;
                string defineName = lineTokenInfos[1].Content;
                if ( !m_AssemblerSettings.CaseSensitive )
                {
                  defineName = defineName.ToUpper();
                }
                string defineValue = parseLine.Substring( equPos + lineTokenInfos[2].Content.Length ).Trim();
                List<Types.TokenInfo>  valueTokens = ParseTokenInfo( defineValue, 0, defineValue.Length );
                int address = -1;

                if ( lineTokenInfos[0].Type == C64Studio.Types.TokenInfo.TokenType.LABEL_LOCAL )
                {
                  defineName = m_CurrentZoneName + defineName;
                }

                if ( defineName == "*" )
                {
                  // set program step
                  int     newStepPos = 0;

                  List<Types.TokenInfo> tokens = ParseTokenInfo( defineValue, 0, defineValue.Length );
                  if ( !EvaluateTokens( lineIndex, tokens, out newStepPos ) )
                  {
                    AddError( lineIndex,
                              Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION,
                              "Could not evaluate * position value",
                              lineTokenInfos[3].StartPos,
                              lineTokenInfos[lineTokenInfos.Count - 1].EndPos + 1 - lineTokenInfos[3].StartPos );
                    return null;
                  }
                  programStepPos = newStepPos;
                  m_CompileCurrentAddress = programStepPos;
                  trueCompileCurrentAddress = programStepPos;
                }
                else
                {
                  if ( !EvaluateTokens( lineIndex, valueTokens, out address ) )
                  {
                    if ( defineName == "*" )
                    {
                      AddError( lineIndex,
                                Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION,
                                "Cannot evaluate expression for *",
                                valueTokens[0].StartPos,
                                valueTokens[valueTokens.Count - 1].EndPos + 1 - valueTokens[0].StartPos );
                    }
                    else
                    {
                      AddUnparsedLabel( defineName, defineValue, lineIndex );
                    }
                  }
                  else
                  {
                    AddConstant( defineName,
                                 address,
                                 lineIndex,
                                 m_CurrentCommentSB.ToString(),
                                 m_CurrentZoneName,
                                 valueTokens[0].StartPos,
                                 valueTokens[valueTokens.Count - 1].EndPos + 1 - valueTokens[0].StartPos );
                    if ( defineName == "*" )
                    {
                      if ( ( address >= 0 )
                      && ( address <= 0xffff ) )
                      {
                        AddError( lineIndex,
                                  Types.ErrorCode.E1003_VALUE_OUT_OF_BOUNDS_WORD,
                                  "Evaluated constant out of bounds, " + address + " must be >= 0 and <= 65535",
                                  valueTokens[0].StartPos,
                                  valueTokens[valueTokens.Count - 1].EndPos + 1 - valueTokens[0].StartPos );
                      }
                      else
                      {
                        programStepPos = address;
                        trueCompileCurrentAddress = programStepPos;
                        info.AddressSource = "*";
                      }
                    }
                  }
                }
                m_CurrentCommentSB = new StringBuilder();
                continue;
              }
            }
            else if ( macro.Type == Types.MacroInfo.MacroType.PSEUDO_PC )
            {
              var result = POPseudoPC( info, stackScopes, lineIndex, lineTokenInfos, 1, lineTokenInfos.Count - 1 );
              if ( result == ParseLineResult.RETURN_NULL )
              {
                return null;
              }
            }
            else if ( macro.Type == Types.MacroInfo.MacroType.REAL_PC )
            {
              PORealPC( info );
            }
            else if ( macro.Type == Types.MacroInfo.MacroType.INCLUDE_SOURCE )
            {
              string  subFilename = "";
              bool    libraryFile = false;

              if ( ( lineTokenInfos.Count == 2 )
              &&   ( lineTokenInfos[1].Type == Types.TokenInfo.TokenType.LITERAL_STRING ) )
              {
                // regular include
                subFilename = lineTokenInfos[1].Content.Substring( 1, lineTokenInfos[1].Length - 2 );
              }
              else if ( ( lineTokenInfos.Count > 3 )
              &&        ( lineTokenInfos[1].Content == "<" )
              &&        ( lineTokenInfos[lineTokenInfos.Count - 1].Content == ">" ) )
              {
                // library include
                subFilename = TokensToExpression( lineTokenInfos, 2, lineTokenInfos.Count - 3 );
                libraryFile = true;
              }
              else
              {
                AddError( lineIndex,
                          Types.ErrorCode.E1302_MALFORMED_MACRO,
                          "Expecting file name, either \"filename\" or \"library filename\"",
                          lineTokenInfos[0].StartPos,
                          lineTokenInfos[0].Length );
                return null;
              }

              localIndex = 0;
              filename = "";
              if ( !ASMFileInfo.FindTrueLineSource( lineIndex, out filename, out localIndex ) )
              {
                DumpSourceInfos( OrigLines, Lines );
                AddError( lineIndex, Types.ErrorCode.E1401_INTERNAL_ERROR, "Includes caused a problem" );
                return null;
              }

              ParseLineResult   plResult = POIncludeSource( libraryFile, subFilename, filename, ref lineIndex, ref Lines );
              if ( plResult == ParseLineResult.RETURN_NULL )
              {
                return null;
              }
              else if ( plResult == ParseLineResult.CALL_CONTINUE )
              {
                continue;
              }
            }
            else if ( macro.Type == Types.MacroInfo.MacroType.FILL )
            {
              var result = POFill( lineTokenInfos, lineIndex, info, parseLine, out lineSizeInBytes );
              if ( result == ParseLineResult.CALL_CONTINUE )
              {
                continue;
              }
              else if ( result == ParseLineResult.RETURN_NULL )
              {
                return null;
              }
            }
            else if ( macro.Type == Types.MacroInfo.MacroType.INCLUDE_BINARY )
            {
              var result = POIncludeBinary( lineTokenInfos, lineIndex, info, out lineSizeInBytes );
              if ( result == ParseLineResult.CALL_CONTINUE )
              {
                continue;
              }
              else if ( result == ParseLineResult.RETURN_NULL )
              {
                return null;
              }
            }
            else if ( macro.Type == Types.MacroInfo.MacroType.INCLUDE_MEDIA )
            {
              string[] replacementLines = null;
              int dummy;
              ASMFileInfo.FindTrueLineSource( lineIndex, out filename, out dummy );

              if ( !POIncludeMedia( lineTokenInfos, lineIndex, true, info, filename, out lineSizeInBytes, out replacementLines ) )
              {
                return null;
              }
            }
            else if ( macro.Type == Types.MacroInfo.MacroType.INCLUDE_MEDIA_SOURCE )
            {
              string[] replacementLines = null;
              int dummy;
              ASMFileInfo.FindTrueLineSource( lineIndex, out filename, out dummy );
              if ( !POIncludeMedia( lineTokenInfos, lineIndex, false, info, filename, out lineSizeInBytes, out replacementLines ) )
              {
                return null;
              }
              if ( replacementLines.Length == 0 )
              {
                // included empty file messes up source info, skip to adding it
                continue;
              }

              string    docFile;
              int       docLine;

              DocumentAndLineFromGlobalLine( lineIndex, out docFile, out docLine );

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

              ASMFileInfo.LineInfo.Remove( lineIndex );

              --lineIndex;
              continue;
            }
            else if ( macro.Type == Types.MacroInfo.MacroType.COMPILE_TARGET )
            {
              // !to targetfilename,outputtype
              //     outputtype = cbm (default) oder plain
              if ( !string.IsNullOrEmpty( m_CompileTargetFile ) )
              {
                AddWarning( lineIndex,
                            C64Studio.Types.ErrorCode.W0004_TARGET_FILENAME_ALREADY_PROVIDED,
                            "A target file name was already provided, ignoring this one",
                            -1,
                            0 );
              }
              else
              {
                lineTokenInfos.RemoveAt( 0 );

                if ( ( lineTokenInfos.Count != 3 )
                || ( lineTokenInfos[1].Content != "," ) )
                {
                  AddError( lineIndex,
                            Types.ErrorCode.E1302_MALFORMED_MACRO,
                            "Expected !to <Filename>,<Type = cbm, plain, cart8bin, cart8crt, cart16bin, cart16crt, magicdeskbin, magicdeskcrt, easyflashbin, easyflashcrt, rgcdbin, rgcdcrt, t64, tap or d64>" );
                  return null;
                }
                if ( lineTokenInfos[0].Type != Types.TokenInfo.TokenType.LITERAL_STRING )
                {
                  AddError( lineIndex,
                            Types.ErrorCode.E1307_FILENAME_INCOMPLETE,
                            "String as file name expected",
                            lineTokenInfos[0].StartPos,
                            lineTokenInfos[0].Length );
                  return null;
                }
                string    targetType = lineTokenInfos[2].Content.ToUpper();
                if ( ( targetType != "CBM" )
                &&   ( targetType != "PLAIN" )
                &&   ( targetType != "CART8BIN" )
                &&   ( targetType != "CART8CRT" )
                &&   ( targetType != "CART16BIN" )
                &&   ( targetType != "CART16CRT" )
                &&   ( targetType != "MAGICDESKBIN" )
                &&   ( targetType != "MAGICDESKCRT" )
                &&   ( targetType != "RGCDBIN" )
                &&   ( targetType != "RGCDCRT" )
                &&   ( targetType != "GMOD2BIN" )
                &&   ( targetType != "GMOD2CRT" )
                &&   ( targetType != "EASYFLASHBIN" )
                &&   ( targetType != "EASYFLASHCRT" )
                &&   ( targetType != "D64" )
                &&   ( targetType != "TAP" )
                &&   ( targetType != "T64" ) )
                {
                  AddError( lineIndex,
                            Types.ErrorCode.E1304_UNSUPPORTED_TARGET_TYPE,
                            "Unsupported target type " + lineTokenInfos[2].Content + ", only cbm, plain, t64, tap, d64, cart8bin, cart8crt, cart16bin, cart16crt, magicdeskbin, magicdeskcrt, easyflashbin, easyflashcrt, rgcdbin, rgcdcrt, gmod2bin or gmod2crt supported",
                            lineTokenInfos[2].StartPos,
                            lineTokenInfos[2].Length );
                  return null;
                }
                filename = lineTokenInfos[0].Content.Substring( 1, lineTokenInfos[0].Length - 2 );
                // do not append to absolute path!
                if ( System.IO.Path.IsPathRooted( filename ) )
                {
                  m_CompileTargetFile = filename;
                }
                else
                {
                  m_CompileTargetFile = GR.Path.Append( m_DocBasePath, filename );
                }
                if ( targetType == "CBM" )
                {
                  m_CompileTarget = Types.CompileTargetType.PRG;
                }
                else if ( targetType == "PLAIN" )
                {
                  m_CompileTarget = Types.CompileTargetType.PLAIN;
                }
                else if ( targetType == "T64" )
                {
                  m_CompileTarget = Types.CompileTargetType.T64;
                }
                else if ( targetType == "TAP" )
                {
                  m_CompileTarget = Types.CompileTargetType.TAP;
                }
                else if ( targetType == "D64" )
                {
                  m_CompileTarget = Types.CompileTargetType.D64;
                }
                else if ( targetType == "CART8BIN" )
                {
                  m_CompileTarget = Types.CompileTargetType.CARTRIDGE_8K_BIN;
                }
                else if ( targetType == "CART8CRT" )
                {
                  m_CompileTarget = Types.CompileTargetType.CARTRIDGE_8K_CRT;
                }
                else if ( targetType == "CART16BIN" )
                {
                  m_CompileTarget = Types.CompileTargetType.CARTRIDGE_16K_BIN;
                }
                else if ( targetType == "CART16CRT" )
                {
                  m_CompileTarget = Types.CompileTargetType.CARTRIDGE_16K_CRT;
                }
                else if ( targetType == "MAGICDESKBIN" )
                {
                  m_CompileTarget = Types.CompileTargetType.CARTRIDGE_MAGICDESK_BIN;
                }
                else if ( targetType == "MAGICDESKCRT" )
                {
                  m_CompileTarget = Types.CompileTargetType.CARTRIDGE_MAGICDESK_CRT;
                }
                else if ( targetType == "RGCDBIN" )
                {
                  m_CompileTarget = Types.CompileTargetType.CARTRIDGE_RGCD_BIN;
                }
                else if ( targetType == "RGCDCRT" )
                {
                  m_CompileTarget = Types.CompileTargetType.CARTRIDGE_RGCD_CRT;
                }
                else if ( targetType == "EASYFLASHBIN" )
                {
                  m_CompileTarget = Types.CompileTargetType.CARTRIDGE_EASYFLASH_BIN;
                }
                else if ( targetType == "EASYFLASHCRT" )
                {
                  m_CompileTarget = Types.CompileTargetType.CARTRIDGE_EASYFLASH_CRT;
                }
                else if ( targetType == "GMOD2BIN" )
                {
                  m_CompileTarget = Types.CompileTargetType.CARTRIDGE_GMOD2_BIN;
                }
                else if ( targetType == "GMOD2CRT" )
                {
                  m_CompileTarget = Types.CompileTargetType.CARTRIDGE_GMOD2_CRT;
                }
              }
            }
            else if ( macro.Type == Types.MacroInfo.MacroType.ADDRESS )
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
                /*
                // remove starting tokens
                for ( int i = 0; i < openingBracketTokenIndex; ++i )
                {
                  lineTokenInfos.RemoveAt( 0 );
                }*/
                stackScopes.Add( new ScopeInfo( ScopeInfo.ScopeType.ADDRESS ) { Active = true, StartIndex = lineIndex } );
                continue;
              }
              else
              {
                lineTokenInfos.RemoveAt( 0 );
                goto recheck_line;
              }
            }
            else if ( macro.Type == Types.MacroInfo.MacroType.IFDEF )
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
                //string defineCheck = parseLine.Substring( 6, startBracket - 6 ).Trim();

                List<Types.TokenInfo> tokens = lineTokenInfos.GetRange( 1, openingBracketTokenIndex - 1 );
                List<Types.TokenInfo> trailingtokens = lineTokenInfos.GetRange( openingBracketTokenIndex + 1, lineTokenInfos.Count - openingBracketTokenIndex - 1 );

                //Debug.Log( "TODO - check if trailing tokens } else {, " + trailingtokens.Count + " tokens found" );
                bool hadElse = false;
                if ( trailingtokens.Count > 0 )
                {
                  // check for } else {
                  if ( trailingtokens.Count >= 3 )
                  {
                    if ( ( trailingtokens[trailingtokens.Count - 3].Content == "}" )
                    &&   ( trailingtokens[trailingtokens.Count - 2].Content.ToUpper() == "ELSE" )
                    &&   ( trailingtokens[trailingtokens.Count - 1].Content == "{" ) )
                    {
                      hadElse = true;
                    }
                  }
                  if ( !hadElse )
                  {
                    AddError( lineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "Syntax error, expected no tokens after bracket" );
                    return null;
                  }
                }
                {
                  int defineResult = -1;
                  Types.ScopeInfo scope = new C64Studio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.IF_OR_IFDEF );
                  scope.StartIndex = lineIndex;

                  // only evaluate the first token
                  // TODO - have to evaluate the rest of the line if it exists!!
                  if ( !EvaluateTokens( lineIndex, tokens, 0, 1, out defineResult ) )
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
            else if ( macro.Type == C64Studio.Types.MacroInfo.MacroType.LABEL_FILE )
            {
              lineTokenInfos.RemoveAt( 0 );

              if ( lineTokenInfos.Count != 1 )
              {
                AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Expected !sl <Filename>" );
                return null;
              }
              if ( lineTokenInfos[0].Type != Types.TokenInfo.TokenType.LITERAL_STRING )
              {
                AddError( lineIndex,
                          Types.ErrorCode.E1307_FILENAME_INCOMPLETE,
                          "String as file name expected",
                          lineTokenInfos[0].StartPos,
                          lineTokenInfos[0].Length );
                return null;
              }
              if ( !string.IsNullOrEmpty( ASMFileInfo.LabelDumpFile ) )
              {
                AddWarning( lineIndex,
                            C64Studio.Types.ErrorCode.W0006_LABEL_DUMP_FILE_ALREADY_GIVEN,
                            "Label dump file name has already been provided",
                            lineTokenInfos[0].StartPos,
                            lineTokenInfos[lineTokenInfos.Count - 1].EndPos + 1 - lineTokenInfos[0].StartPos );
              }

              string fileName = lineTokenInfos[0].Content.Substring( 1, lineTokenInfos[0].Content.Length - 2 );
              ASMFileInfo.LabelDumpFile = fileName;
            }
            else if ( macro.Type == Types.MacroInfo.MacroType.MACRO )
            {
              string macroName = "";
              string outerFilename = "";
              int localLineIndex = 0;
              ASMFileInfo.FindTrueLineSource( lineIndex, out outerFilename, out localLineIndex );

              if ( POMacro( macroFunctions, outerFilename, lineIndex, lineTokenInfos, stackScopes, out macroName ) )
              {
                if ( m_AssemblerSettings.MacroIsZone )
                {
                  m_CurrentZoneName = macroName;
                  info.Zone = m_CurrentZoneName;
                }
              }
            }
            else if ( macro.Type == Types.MacroInfo.MacroType.FOR )
            {
              // !FOR var = start TO stop
              POFor( stackScopes, m_CurrentZoneName, ref intermediateLineOffset, lineIndex, lineTokenInfos );
            }
            else if ( macro.Type == Types.MacroInfo.MacroType.END )
            {
              var result = HandleScopeEnd( macroFunctions, stackScopes, ref lineIndex, ref intermediateLineOffset, ref Lines );
              if ( result == ParseLineResult.CALL_CONTINUE )
              {
                --lineIndex;
                continue;
              }
              else if ( result == ParseLineResult.ERROR_ABORT )
              {
                return null;
              }
            }
            else if ( macro.Type == Types.MacroInfo.MacroType.IFNDEF )
            {
              if ( ScopeInsideMacroDefinition( stackScopes ) )
              {
                // Skip !if check inside macro definition

                // still need to add scope!
                Types.ScopeInfo scope = new C64Studio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.IF_OR_IFDEF );
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
                int     pseudoOpEndPos = lineTokenInfos[0].StartPos + lineTokenInfos[0].Length;
                string defineCheck = parseLine.Substring( pseudoOpEndPos, startBracket - pseudoOpEndPos ).Trim();

                List<Types.TokenInfo> tokens = ParseTokenInfo( defineCheck, 0, defineCheck.Length );

                int defineResult = -1;

                Types.ScopeInfo scope = new C64Studio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.IF_OR_IFDEF );
                scope.StartIndex = lineIndex;
                if ( !EvaluateTokens( lineIndex, tokens, out defineResult ) )
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
            else if ( macro.Type == Types.MacroInfo.MacroType.IF )
            {
              if ( ScopeInsideMacroDefinition( stackScopes ) )
              {
                // Skip !if check inside macro definition

                // still need to add scope!
                Types.ScopeInfo scope = new C64Studio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.IF_OR_IFDEF );
                scope.StartIndex = lineIndex;
                scope.Active = false;

                stackScopes.Add( scope );
                OnScopeAdded( scope );
                continue;
              }

              // !ifdef MUSIC_ON {
              int startBracket = parseLine.IndexOf( "{" );
              if ( startBracket == -1 )
              {
                AddError( lineIndex, Types.ErrorCode.E1004_MISSING_OPENING_BRACKET, "Missing opening brace" );
              }
              else
              {
                int     posAfterMacro = labelInFront.Length;
                if ( labelInFront.Length == 0 )
                {
                  posAfterMacro = lineTokenInfos[0].StartPos + lineTokenInfos[0].Length;
                }
                string expressionCheck = parseLine.Substring( posAfterMacro, startBracket - posAfterMacro ).Trim();

                List<Types.TokenInfo> tokens = ParseTokenInfo( expressionCheck, 0, expressionCheck.Length );

                int defineResult = -1;

                Types.ScopeInfo scope = new C64Studio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.IF_OR_IFDEF );
                scope.StartIndex = lineIndex;
                if ( !EvaluateTokens( lineIndex, tokens, out defineResult ) )
                {
                  AddError( lineIndex, C64Studio.Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate expression: " + expressionCheck );
                  scope.Active = true;
                  scope.IfChainHadActiveEntry = true;
                }
                else if ( defineResult == 0 )
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
            else if ( macro.Type == Types.MacroInfo.MacroType.ELSE )
            {
              if ( ScopeInsideMacroDefinition( stackScopes ) )
              {
                // Skip !if check inside macro definition

                // might still need to toggle scope, but keep it as inactive
                continue;
              }

              //Debug.Log( "other else" );
              if ( stackScopes.Count == 0 )
              {
                AddError( lineIndex, C64Studio.Types.ErrorCode.E1309_ELSE_WITHOUT_IF, "Else statement without if encountered" );
              }
              else
              {
                stackScopes[stackScopes.Count - 1].Active = !stackScopes[stackScopes.Count - 1].IfChainHadActiveEntry;
                //stackScopes[stackScopes.Count - 1].Active = !stackScopes[stackScopes.Count - 1].Active;
                //Debug.Log( "toggle scope active " + lineIndex );
              }
            }
            else if ( macro.Type == Types.MacroInfo.MacroType.END_IF )
            {
              if ( ScopeInsideMacroDefinition( stackScopes ) )
              {
                // Skip !if check inside macro definition

                // still need to remove scope!
                OnScopeRemoved( lineIndex, stackScopes );
                stackScopes.RemoveAt( stackScopes.Count - 1 );
                continue;
              }

              //Debug.Log( "other endif" );
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
            else if ( macro.Type == Types.MacroInfo.MacroType.ZONE )
            {
              if ( lineTokenInfos[lineTokenInfos.Count - 1].Content == "{" )
              {
                lineTokenInfos.RemoveAt( lineTokenInfos.Count - 1 );

                // TODO - check of zonescope exists, no nestes zone scopes!

                Types.ScopeInfo   zoneScope = new C64Studio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.ZONE );
                zoneScope.StartIndex = lineIndex;

                stackScopes.Add( zoneScope );
                OnScopeAdded( zoneScope );
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
                  // anonymous zone
                  m_CurrentZoneName = "anon_scope_" + lineIndex.ToString();
                }
                else
                {
                  m_CurrentZoneName = lineTokenInfos[1].Content;
                  zoneToken = lineTokenInfos[1];
                }
                info.Zone = m_CurrentZoneName;

                AddZone( m_CurrentZoneName, lineIndex, zoneToken.StartPos, zoneToken.Length );
              }
            }
            else if ( macro.Type == Types.MacroInfo.MacroType.BANK )
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
                if ( !EvaluateTokens( lineIndex, paramsNo, out number ) )
                {
                  string expressionCheck = TokensToExpression( paramsNo );

                  AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate expression " + expressionCheck );
                }
                else if ( ( paramsSize.Count > 0 )
                && ( !EvaluateTokens( lineIndex, paramsSize, out size ) ) )
                {
                  string expressionCheck = TokensToExpression( paramsNo );

                  AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate expression " + expressionCheck );
                }
                else
                {
                  if ( ASMFileInfo.Banks.Count > 0 )
                  {
                    // fill from previous bank
                    Types.ASM.BankInfo lastBank = ASMFileInfo.Banks[ASMFileInfo.Banks.Count - 1];

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

                  Types.ASM.BankInfo bank = new C64Studio.Types.ASM.BankInfo();
                  bank.Number = number;
                  bank.SizeInBytes = size;
                  bank.StartLine = lineIndex;
                  bank.SizeInBytesStart = sizeInBytes + info.NumBytes;

                  foreach ( Types.ASM.BankInfo oldBank in ASMFileInfo.Banks )
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

                  ASMFileInfo.Banks.Add( bank );
                }
              }
            }
            else if ( macro.Type == Types.MacroInfo.MacroType.ALIGN )
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
                return null;
              }

              do
              {
                if ( lineTokenInfos[tokenIndex].Content == "," )
                {
                  // found an expression
                  int value = -1;

                  if ( !EvaluateTokens( lineIndex, lineTokenInfos, expressionStartIndex, tokenIndex - expressionStartIndex, out value ) )
                  {
                    AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate " + TokensToExpression( lineTokenInfos, expressionStartIndex, tokenIndex - expressionStartIndex ) );
                    return null;
                  }
                  tokenParams.Add( value );
                  expressionStartIndex = tokenIndex + 1;
                }
                ++tokenIndex;
                if ( tokenIndex == lineTokenInfos.Count )
                {
                  if ( expressionStartIndex <= tokenIndex - 1 )
                  {
                    // there's still data to evaluate
                    int value = -1;
                    if ( !EvaluateTokens( lineIndex, lineTokenInfos, expressionStartIndex, tokenIndex - expressionStartIndex, out value ) )
                    {
                      AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate " + TokensToExpression( lineTokenInfos, expressionStartIndex, tokenIndex - expressionStartIndex ) );
                      return null;
                    }
                    tokenParams.Add( value );
                  }
                }
              }
              while ( tokenIndex < lineTokenInfos.Count );

              if ( ( tokenParams.Count < 2 )
              ||   ( tokenParams.Count > 3 ) )
              {
                AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Macro not formatted as expected. Expected !align <AndValue>,<EqualValue>[,<FillValue>]" );
                return null;
              }
              byte fillValue = 0;
              if ( tokenParams.Count == 3 )
              {
                if ( !ValidByteValue( tokenParams[2] ) )
                {
                  AddError( lineIndex, Types.ErrorCode.E1002_VALUE_OUT_OF_BOUNDS_BYTE, "FillValue out of bounds" );
                  return null;
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
                  return null;
                }
              }
              lineSizeInBytes = info.NumBytes;
            }
            else if ( macro.Type == Types.MacroInfo.MacroType.ALIGN_DASM )
            {
              var parseResult = POAlignDASM( lineTokenInfos, lineIndex, info, ref programStepPos, out lineSizeInBytes );
              if ( parseResult == ParseLineResult.RETURN_NULL )
              {
                return null;
              }
            }
            else if ( ( macro.Type == Types.MacroInfo.MacroType.BYTE )
            ||        ( macro.Type == Types.MacroInfo.MacroType.LOW_BYTE )
            ||        ( macro.Type == Types.MacroInfo.MacroType.HIGH_BYTE ) )
            {
              PODataByte( lineIndex, lineTokenInfos, 1, lineTokenInfos.Count - 1, info, macro.Type, textCodeMapping, true );
              info.Line = parseLine;
              lineSizeInBytes = info.NumBytes;
            }
            else if ( macro.Type == Types.MacroInfo.MacroType.WORD )
            {
              var result = PODataWord( lineTokenInfos, lineIndex, 1, lineTokenInfos.Count - 1, info, parseLine, true, out lineSizeInBytes );
              if ( result == ParseLineResult.RETURN_NULL )
              {
                return null;
              }
              else if ( result == ParseLineResult.CALL_CONTINUE )
              {
                continue;
              }
            }
            else if ( macro.Type == Types.MacroInfo.MacroType.TEXT_SCREEN )
            {
              POText( lineTokenInfos, info, parseLine, m_TextCodeMappingScr, out lineSizeInBytes );
            }
            else if ( macro.Type == Types.MacroInfo.MacroType.TEXT_PET )
            {
              POText( lineTokenInfos, info, parseLine, m_TextCodeMappingPet, out lineSizeInBytes );
            }
            else if ( macro.Type == Types.MacroInfo.MacroType.TEXT )
            {
              POText( lineTokenInfos, info, parseLine, textCodeMapping, out lineSizeInBytes );
            }
            else if ( macro.Type == Types.MacroInfo.MacroType.TEXT_RAW )
            {
              POText( lineTokenInfos, info, parseLine, m_TextCodeMappingRaw, out lineSizeInBytes );
            }
            else if ( macro.Type == Types.MacroInfo.MacroType.CONVERSION_TAB )
            {
              if ( lineTokenInfos.Count < 2 )
              {
                AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Expected !CT <Type = raw or scr or pet or mapping list>" );
              }
              else if ( lineTokenInfos[1].Content.ToUpper() == "RAW" )
              {
                textCodeMapping = m_TextCodeMappingRaw;
              }
              else if ( lineTokenInfos[1].Content.ToUpper() == "SCR" )
              {
                textCodeMapping = m_TextCodeMappingScr;
              }
              else if ( lineTokenInfos[1].Content.ToUpper() == "PET" )
              {
                textCodeMapping = m_TextCodeMappingPet;
              }
              else
              {
                // expecting mapping table
                if ( ( textCodeMapping == m_TextCodeMappingPet )
                || ( textCodeMapping == m_TextCodeMappingRaw )
                || ( textCodeMapping == m_TextCodeMappingScr ) )
                {
                  // only reset mapping if previously mapping was a predefined one
                  textCodeMapping = new GR.Collections.Map<byte, byte>();
                }
                else
                {
                  // create new instance to avoid modifying previously stored mappings
                  textCodeMapping = new GR.Collections.Map<byte, byte>( textCodeMapping );
                }

                GR.Memory.ByteBuffer data = new GR.Memory.ByteBuffer();

                int commaCount = 0;
                int startTokenIndex = 1;
                for ( int tokenIndex = 1; tokenIndex < lineTokenInfos.Count; ++tokenIndex )
                {
                  string token = lineTokenInfos[tokenIndex].Content;

                  if ( ( tokenIndex == 1 )
                  && ( token == "#" ) )
                  {
                    // direct value?
                    if ( ( lineTokenInfos.Count > 2 )
                    && ( lineTokenInfos[2].Content != "#" )
                    && ( lineTokenInfos[2].Content != "." ) )
                    {
                      // not a binary value
                      continue;
                    }
                  }

                  if ( token == "," )
                  {
                    if ( startTokenIndex < tokenIndex )
                    {
                      int aByte = -1;

                      if ( EvaluateTokens( lineIndex, lineTokenInfos, startTokenIndex, tokenIndex - startTokenIndex, out aByte ) )
                      {
                        data.AppendU8( (byte)aByte );
                      }
                      else
                      {
                        // could not fully parse
                        //dh.Log( "Could not fully parse !byte line: " + parseLine );
                        AddError( lineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "Could not parse " + TokensToExpression( lineTokenInfos, startTokenIndex, lineTokenInfos.Count - startTokenIndex ) );
                      }
                    }
                    ++commaCount;
                    startTokenIndex = tokenIndex + 1;
                  }
                  /*
                else if ( !parseFailed )
                {
                  int aByte = -1;

                  if ( ParseValue( token, out aByte ) )
                  {
                    data.AppendU8( (byte)aByte );
                  }
                  else if ( token == "." )
                  {
                    data.AppendU8( 0 );
                  }
                  else if ( token == "#" )
                  {
                    data.AppendU8( 1 );
                  }
                  else
                  {
                    // could not fully parse
                    //dh.Log( "Could not fully parse !byte line: " + parseLine );
                    AddError( lineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "Could not parse " + token );
                    parseFailed = true;
                  }
                }
                   */
                }
                if ( startTokenIndex < lineTokenInfos.Count )
                {
                  int aByte = -1;

                  if ( EvaluateTokens( lineIndex, lineTokenInfos, startTokenIndex, lineTokenInfos.Count - startTokenIndex, out aByte ) )
                  {
                    data.AppendU8( (byte)aByte );
                  }
                  else
                  {
                    // could not fully parse
                    //dh.Log( "Could not fully parse !byte line: " + parseLine );
                    AddError( lineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "Could not parse " + TokensToExpression( lineTokenInfos, startTokenIndex, lineTokenInfos.Count - startTokenIndex ) );
                  }
                }
                if ( ( data.Length % 2 ) != 0 )
                {
                  AddError( lineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "Mapping table must have pairs of bytes, found " + data.Length + " bytes" );
                }
                else
                {
                  for ( int mapping = 0; mapping < data.Length / 2; ++mapping )
                  {
                    textCodeMapping[data.ByteAt( mapping * 2 )] = data.ByteAt( mapping * 2 + 1 );
                  }
                }
              }
            }
            else if ( macro.Type == Types.MacroInfo.MacroType.IGNORE )
            {
              labelInFront = "";
            }
            else if ( macro.Type == C64Studio.Types.MacroInfo.MacroType.BASIC )
            {
              var parseResult = POBasic( parseLine, lineTokenInfos, lineIndex, info, textCodeMapping, true, out lineSizeInBytes );
              if ( parseResult == ParseLineResult.RETURN_NULL )
              {
                return null;
              }
            }
            else
            {
              AddError( lineIndex, Types.ErrorCode.E1301_MACRO_UNKNOWN, "Macro " + macro.Type + " currently has no effect!" );
            }
          }
          evaluatedContent = true;
        }

        // PDS style macros look like labels!
        if ( ( !evaluatedContent )
        &&   ( m_AssemblerSettings.Macros.ContainsKey( upToken ) ) )
        {
          // TODO - ugly, copied code!!
          hadMacro = true;
          Types.MacroInfo macroInfo = m_AssemblerSettings.Macros[upToken];
          if ( macroInfo.Type == C64Studio.Types.MacroInfo.MacroType.HEX )
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
                  AddError( lineIndex, C64Studio.Types.ErrorCode.E1000_SYNTAX_ERROR, "Malformed hex data" );
                  return null;
                }
              }
              else
              {
                if ( !info.LineData.AppendHex( tokenHex.Content ) )
                {
                  AddError( lineIndex, C64Studio.Types.ErrorCode.E1000_SYNTAX_ERROR, "Malformed hex data" );
                  return null;
                }
              }
            }
            info.NumBytes = (int)info.LineData.Length;
            lineSizeInBytes = (int)info.LineData.Length;
          }
          else if ( macroInfo.Type == C64Studio.Types.MacroInfo.MacroType.BASIC )
          {
            var parseResult = POBasic( parseLine, lineTokenInfos, lineIndex, info, textCodeMapping, true, out lineSizeInBytes );
            if ( parseResult == ParseLineResult.RETURN_NULL )
            {
              return null;
            }
          }
          else if ( macroInfo.Type == C64Studio.Types.MacroInfo.MacroType.ORG )
          {
            // set program step
            int newStepPos = 0;
            int newPseudoPos = -2;

            int commaPos = -1;
            int commaCount = 0;
            for ( int i = 1; i < lineTokenInfos.Count; ++i )
            {
              if ( ( lineTokenInfos[i].Type == C64Studio.Types.TokenInfo.TokenType.SEPARATOR )
              && ( lineTokenInfos[i].Content == "," ) )
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
              return null;
            }
            if ( commaCount == 1 )
            {
              if ( !EvaluateTokens( lineIndex, lineTokenInfos, 1, commaPos - 1, out newStepPos ) )
              {
                AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate ORG position value" );
                return null;
              }
              if ( !EvaluateTokens( lineIndex, lineTokenInfos, commaPos + 1, lineTokenInfos.Count - commaPos - 1, out newPseudoPos ) )
              {
                AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate ORG pseudo position value" );
                return null;
              }
            }
            else
            {
              if ( !EvaluateTokens( lineIndex, lineTokenInfos, 1, lineTokenInfos.Count - 1, out newStepPos ) )
              {
                AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate ORG position value" );
                return null;
              }
            }
            programStepPos = newStepPos;
            m_CompileCurrentAddress = programStepPos;
            trueCompileCurrentAddress = programStepPos;

            // either the new value or -2
            info.PseudoPCOffset = newPseudoPos;
          }
          else if ( macroInfo.Type == Types.MacroInfo.MacroType.PSEUDO_PC )
          {
            var result = POPseudoPC( info, stackScopes, lineIndex, lineTokenInfos, 1, lineTokenInfos.Count - 1 );
            if ( result == ParseLineResult.RETURN_NULL )
            {
              return null;
            }
          }
          else if ( macroInfo.Type == Types.MacroInfo.MacroType.REAL_PC )
          {
            PORealPC( info );
          }
          else if ( ( macroInfo.Type == Types.MacroInfo.MacroType.BYTE )
          ||        ( macroInfo.Type == Types.MacroInfo.MacroType.LOW_BYTE )
          ||        ( macroInfo.Type == Types.MacroInfo.MacroType.HIGH_BYTE ) )
          {
            PODataByte( lineIndex, lineTokenInfos, 1, lineTokenInfos.Count - 1, info, macroInfo.Type, textCodeMapping, true );
            info.Line = parseLine;
            lineSizeInBytes = info.NumBytes;
          }
          else if ( macroInfo.Type == Types.MacroInfo.MacroType.WORD )
          {
            var result = PODataWord( lineTokenInfos, lineIndex, 1, lineTokenInfos.Count - 1, info, parseLine, true, out lineSizeInBytes );
            if ( result == ParseLineResult.RETURN_NULL )
            {
              return null;
            }
            else if ( result == ParseLineResult.CALL_CONTINUE )
            {
              continue;
            }
          }
          else if ( macroInfo.Type == Types.MacroInfo.MacroType.FILL )
          {
            var result = POFill( lineTokenInfos, lineIndex, info, parseLine, out lineSizeInBytes );
            if ( result == ParseLineResult.CALL_CONTINUE )
            {
              continue;
            }
            else if ( result == ParseLineResult.RETURN_NULL )
            {
              return null;
            }
          }
          else if ( macroInfo.Type == Types.MacroInfo.MacroType.LOOP_START )
          {
            var result = POLoopStart( lineTokenInfos, lineIndex, info, ref Lines, stackScopes, out lineSizeInBytes );
            if ( result == ParseLineResult.RETURN_NULL )
            {
              return null;
            }
            else if ( result == ParseLineResult.CALL_CONTINUE )
            {
              continue;
            }
          }
          else if ( macroInfo.Type == Types.MacroInfo.MacroType.TEXT )
          {
            POText( lineTokenInfos, info, parseLine, textCodeMapping, out lineSizeInBytes );
          }
          else if ( macroInfo.Type == Types.MacroInfo.MacroType.MACRO )
          {
            string macroName = "";

            string outerFilename = "";
            int localLineIndex = 0;
            ASMFileInfo.FindTrueLineSource( lineIndex, out outerFilename, out localLineIndex );

            if ( POMacro( macroFunctions, outerFilename, lineIndex, lineTokenInfos, stackScopes, out macroName ) )
            {
              if ( m_AssemblerSettings.MacroIsZone )
              {
                m_CurrentZoneName = macroName;
                info.Zone = m_CurrentZoneName;
              }
            }
          }
          else if ( ( macroInfo.Type == Types.MacroInfo.MacroType.END )
          ||        ( macroInfo.Type == Types.MacroInfo.MacroType.LOOP_END ) )
          {
            var result = HandleScopeEnd( macroFunctions, stackScopes, ref lineIndex, ref intermediateLineOffset, ref Lines );
            if ( result == ParseLineResult.CALL_CONTINUE )
            {
              --lineIndex;
              continue;
            }
            else if ( result == ParseLineResult.ERROR_ABORT )
            {
              return null;
            }
          }
          else if ( macroInfo.Type == Types.MacroInfo.MacroType.INCLUDE_BINARY )
          {
            var result = POIncludeBinary( lineTokenInfos, lineIndex, info, out lineSizeInBytes );
            if ( result == ParseLineResult.CALL_CONTINUE )
            {
              continue;
            }
            else if ( result == ParseLineResult.RETURN_NULL )
            {
              return null;
            }
          }
          else if ( macroInfo.Type == Types.MacroInfo.MacroType.INCLUDE_SOURCE )
          {
            string  subFilename = "";
            bool    libraryFile = false;

            if ( ( lineTokenInfos.Count == 2 )
            &&   ( lineTokenInfos[1].Type == Types.TokenInfo.TokenType.LITERAL_STRING ) )
            {
              // regular include
              subFilename = lineTokenInfos[1].Content.Substring( 1, lineTokenInfos[1].Length - 2 );
            }
            else if ( ( lineTokenInfos.Count > 3 )
            &&        ( lineTokenInfos[1].Content == "<" )
            &&        ( lineTokenInfos[lineTokenInfos.Count - 1].Content == ">" ) )
            {
              // library include
              subFilename = TokensToExpression( lineTokenInfos, 2, lineTokenInfos.Count - 3 );
              libraryFile = true;
            }
            else 
            {
              AddError( lineIndex, 
                        Types.ErrorCode.E1302_MALFORMED_MACRO, 
                        "Expecting file name, either \"filename\" or \"library filename\"",
                        lineTokenInfos[0].StartPos,
                        lineTokenInfos[0].Length );
              return null;
            }

            localIndex = 0;
            filename = "";
            if ( !ASMFileInfo.FindTrueLineSource( lineIndex, out filename, out localIndex ) )
            {
              DumpSourceInfos( OrigLines, Lines );
              AddError( lineIndex, Types.ErrorCode.E1401_INTERNAL_ERROR, "Includes caused a problem" );
              return null;
            }
            ParseLineResult   plResult = POIncludeSource( libraryFile, subFilename, ParentFilename, ref lineIndex, ref Lines );
            if ( plResult == ParseLineResult.RETURN_NULL )
            {
              return null;
            }
            else if ( plResult == ParseLineResult.CALL_CONTINUE )
            {
              continue;
            }
          }
          else if ( macroInfo.Type == Types.MacroInfo.MacroType.IF )
          {
            int defineResult = -1;

            Types.ScopeInfo scope = new C64Studio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.IF_OR_IFDEF );
            scope.StartIndex = lineIndex;
            if ( !EvaluateTokens( lineIndex, lineTokenInfos, 1, lineTokenInfos.Count - 1, out defineResult ) )
            {
              AddError( lineIndex, C64Studio.Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate expression: " + TokensToExpression( lineTokenInfos, 1, lineTokenInfos.Count - 1 ) );
              scope.Active = true;
              scope.IfChainHadActiveEntry = true;
            }
            else if ( defineResult == 0 )
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
          else if ( macroInfo.Type == Types.MacroInfo.MacroType.ELSE )
          {
            if ( stackScopes.Count == 0 )
            {
              AddError( lineIndex, C64Studio.Types.ErrorCode.E1309_ELSE_WITHOUT_IF, "Else statement without if encountered" );
            }
            else
            {
              stackScopes[stackScopes.Count - 1].Active = !stackScopes[stackScopes.Count - 1].IfChainHadActiveEntry;
              //stackScopes[stackScopes.Count - 1].Active = !stackScopes[stackScopes.Count - 1].Active;
              //Debug.Log( "toggle else " + lineIndex );
            }
          }
          else if ( macroInfo.Type == Types.MacroInfo.MacroType.END_IF )
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
          else if ( macroInfo.Type == Types.MacroInfo.MacroType.ALIGN_DASM )
          {
            var parseResult = POAlignDASM( lineTokenInfos, lineIndex, info, ref programStepPos, out lineSizeInBytes );
            if ( parseResult == ParseLineResult.RETURN_NULL )
            {
              return null;
            }
          }
          else if ( macroInfo.Type != C64Studio.Types.MacroInfo.MacroType.IGNORE )
          {
            AddError( lineIndex, C64Studio.Types.ErrorCode.E1301_MACRO_UNKNOWN, "Unsupported macro " + macroInfo.Keyword + " encountered" );
            return null;
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
                AddError( lineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "Syntax error: " + TokensToExpression( lineTokenInfos ) );
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
          if ( info.AddressSource == "*" )
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
        if ( !hadMacro )
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
          if ( scope.Loop == null )
          {
            AddError( scope.StartIndex, Types.ErrorCode.E1005_MISSING_CLOSING_BRACKET, "Missing closing bracket" );
          }
        }
      }
      foreach ( Types.MacroFunctionInfo macroFunction in macroFunctions.Values )
      {
        if ( macroFunction.LineEnd == -1 )
        {
          AddError( macroFunction.LineIndex, Types.ErrorCode.E1008_MISSING_LOOP_END, "Macro function " + macroFunction.Name + ", started in line " + ( macroFunction.LineIndex + 1 ) + ", is missing end statement" );
        }
      }

      if ( ASMFileInfo.Banks.Count > 0 )
      {
        // fill from previous bank
        Types.ASM.LineInfo info = new Types.ASM.LineInfo();

        info.LineIndex = Lines.Length;
        Types.ASM.BankInfo lastBank = ASMFileInfo.Banks[ASMFileInfo.Banks.Count - 1];

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

      //Debug.Log( "PreProcess done" );
      m_CompileCurrentAddress = -1;
      return Lines;
    }



    private bool MatchesMacroByType( string Token, MacroInfo.MacroType Type )
    {
      string    upToken = Token.ToUpper();

      foreach ( var macro in m_AssemblerSettings.Macros )
      {
        if ( ( macro.Value.Type == Type )
        &&   ( macro.Value.Keyword == upToken ) )
        {
          return true;
        }
      }
      return false;
    }



    private bool TokenStartsScope( TokenInfo Token, out Types.ScopeInfo.ScopeType Type )
    {
      Type = ScopeInfo.ScopeType.UNKNOWN;
      if ( Token.Type == TokenInfo.TokenType.MACRO )
      {
        if ( MatchesMacroByType( Token.Content, MacroInfo.MacroType.ADDRESS ) )
        {
          Type = ScopeInfo.ScopeType.ADDRESS;
          return true;
        }
        if ( MatchesMacroByType( Token.Content, MacroInfo.MacroType.ZONE ) )
        {
          Type = ScopeInfo.ScopeType.ZONE;
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

        if ( ( token.Type == C64Studio.Types.TokenInfo.TokenType.SEPARATOR )
        &&   ( token.Content == ":" ) )
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

          if ( ( token.Type == C64Studio.Types.TokenInfo.TokenType.SEPARATOR )
          &&   ( token.Content == ":" ) )
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
        sourceInfo.LineCount        = newLines.Length;
        sourceInfo.FilenameParent   = ParentFilename;

        string  dummyFile = "";
        int     localFileIndex = -1;
        ASMFileInfo.FindTrueLineSource( lineIndex, out dummyFile, out localFileIndex );
        sourceInfo.LocalStartLine   = localFileIndex;

        SourceInfoLog( "-include at global index " + lineIndex );
        SourceInfoLog( "-has " + sourceInfo.LineCount + " lines" );

        InsertSourceInfo( sourceInfo, true, true );

        string[] result = new string[Lines.Length + sourceInfo.LineCount];

        System.Array.Copy( Lines, 0, result, 0, lineIndex + 1 );
        System.Array.Copy( newLines, 0, result, lineIndex + 1, newLines.Length );

        // this keeps the !source line in the final code, makes working with source infos easier though
        System.Array.Copy( Lines, lineIndex + 1, result, lineIndex + newLines.Length + 1, Lines.Length - lineIndex - 1 );

        // replace !source with empty line (otherwise source infos would have one line more!)
        //result[lineIndex + newLines.Length] = "";
        result[lineIndex] = "";

        Lines = result;

        ASMFileInfo.LineInfo.Remove( lineIndex );

        --lineIndex;
        return ParseLineResult.CALL_CONTINUE;
      }
      return ParseLineResult.OK;
    }



    private bool TokenIsPseudoPC( TokenInfo Token )
    {
      if ( ( Token.Type == TokenInfo.TokenType.MACRO )
      &&   ( Token.Content.ToUpper() == MacroByType( MacroInfo.MacroType.PSEUDO_PC ) ) )
      {
        return true;
      }
      return false;
    }



    private bool TokenIsConditionalThatStartsScope( TokenInfo Token )
    {
      if ( ( Token.Type == TokenInfo.TokenType.MACRO )
      &&   ( ( Token.Content.ToUpper() == MacroByType( MacroInfo.MacroType.IF ).ToUpper() )
      ||     ( Token.Content.ToUpper() == MacroByType( MacroInfo.MacroType.IFDEF ).ToUpper() )
      ||     ( Token.Content.ToUpper() == MacroByType( MacroInfo.MacroType.IFNDEF ).ToUpper() ) ) )
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
          if ( ( m_AssemblerSettings.AllowedTokenStartChars.ContainsKey( C64Studio.Types.TokenInfo.TokenType.COMMENT_IF_FIRST_CHAR ) )
          &&   ( m_AssemblerSettings.AllowedTokenStartChars[C64Studio.Types.TokenInfo.TokenType.COMMENT_IF_FIRST_CHAR].IndexOf( aChar ) != -1 ) )
          {
            commentPos = 0;
            break;
          }
          if ( m_AssemblerSettings.AllowedTokenStartChars[C64Studio.Types.TokenInfo.TokenType.COMMENT].IndexOf( aChar ) != -1 )
          {
            commentPos = i;
            return true;
          }
        }
        if ( m_AssemblerSettings.AllowedTokenStartChars[C64Studio.Types.TokenInfo.TokenType.COMMENT].IndexOf( aChar ) != -1 )
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



    private ParseLineResult POPseudoPC( Types.ASM.LineInfo info, List<Types.ScopeInfo> Scopes, int lineIndex, List<Types.TokenInfo> lineTokenInfos, int TokenStartIndex, int TokenCount )
    {
      if ( TokenCount == 0 )
      {
        AddError( lineIndex,
                  Types.ErrorCode.E1000_SYNTAX_ERROR,
                  "Expression expected",
                  lineTokenInfos[0].StartPos,
                  lineTokenInfos[lineTokenInfos.Count - 1].EndPos );
        return ParseLineResult.RETURN_NULL;
      }
      if ( lineTokenInfos[TokenStartIndex + TokenCount - 1].Content == "{" )
      {
        // a real PC with bracket
        var scopeInfo = new Types.ScopeInfo( Types.ScopeInfo.ScopeType.PSEUDO_PC );
        scopeInfo.StartIndex = lineIndex;
        scopeInfo.Active = true;

        Scopes.Add( scopeInfo );
        OnScopeAdded( scopeInfo );
        --TokenCount;
      }

      int pseudoStepPos = -1;
      if ( !EvaluateTokens( lineIndex, lineTokenInfos, TokenStartIndex, TokenCount, out pseudoStepPos ) )
      {
        string expressionCheck = TokensToExpression( lineTokenInfos, TokenStartIndex, TokenCount );

        AddError( lineIndex,
                  Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION,
                  "Could not evaluate expression " + TokensToExpression( lineTokenInfos, 1, lineTokenInfos.Count - 1 ),
                  lineTokenInfos[1].StartPos,
                  lineTokenInfos[lineTokenInfos.Count - 1].EndPos + 1 - lineTokenInfos[1].StartPos );
        return ParseLineResult.RETURN_NULL;
      }
      info.PseudoPCOffset = pseudoStepPos;
      return ParseLineResult.OK;
    }



    private void POFor( List<Types.ScopeInfo> stackScopes, string zoneName, ref int intermediateLineOffset, int lineIndex, List<Types.TokenInfo> lineTokenInfos )
    {
      if ( ScopeInsideMacroDefinition( stackScopes ) )
      {
        // ignore for loop if we are inside a macro definition!

        // add dummy scope so !ends properly match
        Types.LoopInfo loop = new Types.LoopInfo();

        loop.LineIndex = lineIndex;

        Types.ScopeInfo   scope = new C64Studio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.LOOP );

        scope.Active = true;
        scope.Loop = loop;
        scope.StartIndex = lineIndex;
        stackScopes.Add( scope );
        return;
      }
      if ( lineTokenInfos.Count < 5 )
      {
        AddError( lineIndex, C64Studio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Malformed macro, expect !FOR <Variable> = <Start Value Expression> TO <End Value Expression [STEP] <Step Value Expression>" );
      }
      else
      {
        if ( ( ( lineTokenInfos[1].Type != C64Studio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
        &&     ( lineTokenInfos[1].Type != C64Studio.Types.TokenInfo.TokenType.LABEL_LOCAL ) )
        ||   ( lineTokenInfos[2].Content != "=" ) )
        {
          AddError( lineIndex,
                    C64Studio.Types.ErrorCode.E1302_MALFORMED_MACRO,
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
                      C64Studio.Types.ErrorCode.E1302_MALFORMED_MACRO,
                      "Malformed macro, expect !FOR <Variable> = <Start Value Expression> TO <End Value Expression [STEP] <Step Value Expression>",
                      lineTokenInfos[1].StartPos,
                      lineTokenInfos[lineTokenInfos.Count - 1].EndPos + 1 - lineTokenInfos[1].StartPos );
            hadError = true;
          }
          if ( indexStep != -1 )
          {
            if ( !EvaluateTokens( lineIndex, lineTokenInfos, indexStep + 1, lineTokenInfos.Count - indexStep - 1, out stepValue ) )
            {
              AddError( lineIndex,
                        C64Studio.Types.ErrorCode.E1302_MALFORMED_MACRO,
                        "Malformed macro, expect !FOR <Variable> = <Start Value Expression> TO <End Value Expression [STEP] <Step Value Expression>",
                        lineTokenInfos[indexStep + 1].StartPos,
                        lineTokenInfos[lineTokenInfos.Count - 1].EndPos + 1 - lineTokenInfos[indexStep + 1].StartPos );
              hadError = true;
            }
            else if ( stepValue == 0 )
            {
              AddError( lineIndex,
                        C64Studio.Types.ErrorCode.E1302_MALFORMED_MACRO,
                        "Value of step must not be zero",
                        lineTokenInfos[indexStep + 1].StartPos,
                        lineTokenInfos[lineTokenInfos.Count - 1].EndPos + 1 - lineTokenInfos[indexStep + 1].StartPos );
              hadError = true;
            }
          }
          else
          {
            indexStep = lineTokenInfos.Count;
          }

          if ( !hadError )
          {
            int   startValue = 0;
            int   endValue = 0;
            if ( !EvaluateTokens( lineIndex, lineTokenInfos, 3, indexTo - 3, out startValue ) )
            {
              AddError( lineIndex,
                        C64Studio.Types.ErrorCode.E1302_MALFORMED_MACRO,
                        "Could not evaluate start value",
                        lineTokenInfos[3].StartPos,
                        lineTokenInfos[indexTo - 3].EndPos + 1 - lineTokenInfos[3].StartPos );
              hadError = true;
            }
            else if ( !EvaluateTokens( lineIndex, lineTokenInfos, indexTo + 1, indexStep - indexTo - 1, out endValue ) )
            {
              AddError( lineIndex,
                        C64Studio.Types.ErrorCode.E1302_MALFORMED_MACRO,
                        "Could not evaluate end value",
                        lineTokenInfos[indexTo + 1].StartPos,
                        lineTokenInfos[indexStep - indexTo - 1].EndPos + 1 - lineTokenInfos[indexTo + 1].StartPos );
              hadError = true;
            }
            else if ( ( stepValue < 0 )
            &&        ( endValue >= startValue ) )
            {
              AddError( lineIndex,
                        C64Studio.Types.ErrorCode.E1302_MALFORMED_MACRO,
                        "End value must be lower than start value with negative step",
                        lineTokenInfos[indexTo + 1].StartPos,
                        lineTokenInfos[indexStep - 1].EndPos + 1 - lineTokenInfos[indexTo + 1].StartPos );
              hadError = true;
            }
            else if ( ( stepValue > 0 )
            &&        ( endValue < startValue ) )
            {
              AddError( lineIndex,
                        C64Studio.Types.ErrorCode.E1302_MALFORMED_MACRO,
                        "End value must be higher than start value with positive step",
                        lineTokenInfos[indexTo + 1].StartPos,
                        lineTokenInfos[indexStep - indexTo - 1].EndPos + 1 - lineTokenInfos[indexTo + 1].StartPos );
              hadError = true;
            }

            if ( !hadError )
            {
              Types.LoopInfo loop = new Types.LoopInfo();

              loop.Label = lineTokenInfos[1].Content;
              loop.LineIndex = lineIndex;
              loop.StartValue = startValue;
              loop.EndValue = endValue;
              loop.StepValue = stepValue;
              loop.CurrentValue = startValue;

              Types.ScopeInfo   scope = new C64Studio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.LOOP );
              // TODO - active depends on parent scopes
              scope.Active = true;
              scope.Loop = loop;
              scope.StartIndex = lineIndex;
              stackScopes.Add( scope );

              AddLabel( loop.Label, loop.StartValue, lineIndex + 1, zoneName, lineTokenInfos[1].StartPos, lineTokenInfos[1].Length );
              intermediateLineOffset = 0;

              //Debug.Log( "add for label for " + loop.Label );
              //Debug.Log( "Add for loop scope for " + loop.Label + " at " + lineIndex );
            }
          }
        }
      }
    }



    // relabels local labels in macros to avoid clashes with duplicate calls - spares parameters
    private string[] RelabelLocalLabelsForMacro( string[] Lines, List<Types.ScopeInfo> Scopes, int lineIndex, string functionName, Types.MacroFunctionInfo functionInfo, List<string> param, out int LineIndexInsideMacro )
    {
      string[] replacementLines = new string[functionInfo.LineEnd - functionInfo.LineIndex - 1];
      int replacementLineIndex = 0;

      LineIndexInsideMacro = -1;
      ClearErrorInfo();

      for ( int i = functionInfo.LineIndex + 1; i < functionInfo.LineEnd; ++i )
      {
        List<Types.TokenInfo> tokens = ParseTokenInfo( functionInfo.Content[i - functionInfo.LineIndex - 1], 0, functionInfo.Content[i - functionInfo.LineIndex - 1].Length );
        if ( tokens == null )
        {
          // there was an error!
          LineIndexInsideMacro = i;
          return null;
        }
        var originatingTokens = new List<Types.TokenInfo>();
        for ( int j = 0; j < tokens.Count; ++j )
        {
          var clonedToken = new Types.TokenInfo() { Content = tokens[j].Content, Length = tokens[j].Length, OriginatingString = tokens[j].OriginatingString, StartPos = tokens[j].StartPos, Type = tokens[j].Type };
          originatingTokens.Add( clonedToken );
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
            //tokens[0].Type = TokenInfo.TokenType.LABEL_INTERNAL;
          }
        }
        bool replacedParam = false;
        bool modifiedToken = false;

        List<Types.TokenInfo>  replacingTokens = new List<Types.TokenInfo>();
        for ( int tokenIndex = 0; tokenIndex < tokens.Count; ++tokenIndex )
        {
          var token = tokens[tokenIndex];
          modifiedToken = false;
          // may look useless, but stores actual content in token
          token.Content = token.Content;
          if ( ( token.Type == C64Studio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
          ||   ( token.Type == C64Studio.Types.TokenInfo.TokenType.LABEL_LOCAL )
          ||   ( token.Type == C64Studio.Types.TokenInfo.TokenType.LABEL_INTERNAL ) )
          {
            bool                    tokenIsExpression = false;
            List<Types.TokenInfo>   tempTokens = new List<C64Studio.Types.TokenInfo>();

            for ( int j = 0; j < functionInfo.ParameterNames.Count; ++j )
            {
              if ( ( token.Content == functionInfo.ParameterNames[j] )
              &&   ( !functionInfo.ParametersAreReferences[j] ) )
              {
                // replace parameter!
                modifiedToken = true;
                token.Content = param[j];
                token.Length = param[j].Length;

                tempTokens = ParseTokenInfo( token.Content, 0, token.Content.Length );
                for ( int k = 0; k < tempTokens.Count; ++k )
                {
                  // may look useless, but actually fetches the substring and stores it in the content cache
                  tempTokens[k].Content = tempTokens[k].Content;
                  tempTokens[k].Length = tempTokens[k].Content.Length;
                }
                if ( ( !HasError() )
                &&   ( tempTokens.Count >= 1 ) )
                {
                  if ( ( tempTokens[0].Type != C64Studio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
                  &&   ( tempTokens[0].Type != C64Studio.Types.TokenInfo.TokenType.LABEL_LOCAL ) )
                  {
                    tokenIsExpression = true;
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
                    /*
                    for ( int k = 0; k < tokens.Count; ++k )
                    {
                      while ( tokens[k].StartPos > sb.Length )
                      {
                        sb.Append( ' ' );
                        ++curOffset;
                      }
                      sb.Append( tokens[k].Content );
                      tokens[k].StartPos = curOffset;
                      curOffset = sb.Length;
                    }*/
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
            if ( ( token.Type == C64Studio.Types.TokenInfo.TokenType.LABEL_LOCAL )
            ||   ( token.Type == C64Studio.Types.TokenInfo.TokenType.LABEL_INTERNAL ) )
            {
              tokenIsExpression = true;
              if ( tokenIsExpression )
              {
                if ( !modifiedToken )
                {
                  modifiedToken = true;
                  //tokens.RemoveAt( tokenIndex );
                  //tokens.InsertRange( tokenIndex, tempTokens );
                  if ( token.Type == C64Studio.Types.TokenInfo.TokenType.LABEL_INTERNAL )
                  {
                    // TODO - take i in account, 
                    //token.Content = "_c64studiointernal_" + functionName + "_" + functionInfo.LineIndex.ToString() + "_" + lineIndex.ToString() + "_" + GetLoopGUID( Scopes ) + token.Content;
                    token.Content += InternalLabelPrefix + functionName + "_" + functionInfo.LineIndex.ToString() + "_" + lineIndex.ToString() + "_" + GetLoopGUID( Scopes );
                    //token.Content = token.Content.Replace( "+", "_plus_" );
                    //token.Content = token.Content.Replace( "-", "_minus_" );
                    //Debug.Log( "Replaced internal label in line " + i + " with " + token.Content );
                  }
                  else if ( !ScopeInsideLoop( Scopes ) )
                  {
                    // uniquefy labels
                    token.Content = "_c64studiointernal_" + functionName + "_" + functionInfo.LineIndex.ToString() + "_" + lineIndex.ToString() + "_" + token.Content;
                  }
                  else
                  {
                    // need to take loop into account, force new local label!
                    token.Content = m_AssemblerSettings.AllowedTokenStartChars[C64Studio.Types.TokenInfo.TokenType.LABEL_LOCAL]
                                  + "_c64studiointernal_" + functionName + "_" + functionInfo.LineIndex.ToString() + "_" + lineIndex.ToString() + "_" + GetLoopGUID( Scopes ) + "_" + token.Content;
                  }
                  replacingTokens.Add( token );
                }
                /*
                // re-position following tokens
                int     curStartPos = 0;
                if ( tokenIndex > 0 )
                {
                  curStartPos = tokens[tokenIndex - 1].StartPos + tokens[tokenIndex - 1].Length;
                }

                // only reset startpos for newly inserted tokens (and rely on other tokens to have content correctly set)
                for ( int j = tokenIndex; j < tokenIndex + tempTokens.Count; ++j )
                {
                  // +1 to put a space between tokens
                  tokens[j].StartPos = curStartPos + 1;
                  curStartPos = tokens[j].StartPos + tokens[j].Length;
                }*/
              }
                /*
              else if ( !ScopeInsideLoop( Scopes ) )
              {
                //token.Content = functionName + "_" + i.ToString() + "_" + lineIndex.ToString() + "_" + token.Content;
                token.Content = functionName + "_" + functionInfo.LineIndex.ToString() + "_" + lineIndex.ToString() + "_" + token.Content;
              }
              else
              {
                // need to take loop into account, force new local label!
                token.Content = m_AssemblerSettings.AllowedTokenStartChars[C64Studio.Types.TokenInfo.TokenType.LABEL_LOCAL]
                              + functionName + "_" + functionInfo.LineIndex.ToString() + "_" + lineIndex.ToString() + "_" + GetLoopGUID( Scopes ) + "_" + token.Content;
              }*/
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
        //Debug.Log( replacementLines[replacementLineIndex] );
        ++replacementLineIndex;
      }
      return replacementLines;
    }



    private string[] RelabelLocalLabelsForLoop( string[] Lines, List<Types.ScopeInfo> Scopes, int lineIndex )
    {
      string[] replacementLines = new string[Lines.Length];
      int replacementLineIndex = 0;

      for ( int i = 0; i < Lines.Length; ++i )
      {
        List<Types.TokenInfo> tokens = ParseTokenInfo( Lines[i], 0, Lines[i].Length );
        bool replacedParam = false;

        int   tokenIndex = 0;
        foreach ( Types.TokenInfo token in tokens )
        {
          if ( ( token.Type == C64Studio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
          ||   ( token.Type == C64Studio.Types.TokenInfo.TokenType.LABEL_LOCAL )
          ||   ( token.Type == C64Studio.Types.TokenInfo.TokenType.LABEL_INTERNAL ) )
          {
            // Dasm - Macro local labels start with ., add macro and LINE and loop specific prefix
            if ( token.Type == C64Studio.Types.TokenInfo.TokenType.LABEL_LOCAL )
            {
              // need to take loop into account, force new local label!
              /*
              token.Content = m_AssemblerSettings.AllowedTokenStartChars[C64Studio.Types.TokenInfo.TokenType.LABEL_LOCAL]
                            + GetLoopGUID( Scopes ) + "_" + i.ToString() + "_" + lineIndex.ToString() + "_" + token.Content;*/
              token.Content = m_AssemblerSettings.AllowedTokenStartChars[C64Studio.Types.TokenInfo.TokenType.LABEL_LOCAL]
                            + AssemblerSettings.INTERNAL_LOCAL_LOOP_LABEL_PREFIX
                            + GetLoopGUID( Scopes ) + "_" + lineIndex.ToString() + "_" + token.Content;
              replacedParam = true;
            }
            else if ( token.Type == C64Studio.Types.TokenInfo.TokenType.LABEL_INTERNAL )
            {
              // need to take loop into account, force new local label!
              //token.Content += GetLoopGUID( Scopes ) + "_" + i.ToString() + "_" + lineIndex.ToString();
              token.Content += GetLoopGUID( Scopes ) + "_" + lineIndex.ToString();
              //Debug.Log( "RelabelLocalLabelsForLoop Replaced internal label at line " + i + " in loop with " + token.Content );
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
        //Debug.Log( replacementLines[replacementLineIndex] );
        ++replacementLineIndex;
      }
      return replacementLines;
    }



    private bool POMacro( GR.Collections.Map<string, Types.MacroFunctionInfo> macroFunctions, 
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

      if ( ( m_AssemblerSettings.MacrosHaveVariableNumberOfArguments )
      &&   ( lineTokenInfos.Count != 2 ) )
      {
        AddError( lineIndex, C64Studio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Malformed macro, expect !MACRO <Macroname>" );
        hadError = true;
      }
      else if ( lineTokenInfos.Count < 2 )
      {
        AddError( lineIndex, C64Studio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Malformed macro, expect !MACRO <Macroname> [<Param1>[,<Param2>[...]]]" );
        hadError = true;
      }
      else if ( lineTokenInfos[1].Type != C64Studio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
      {
        AddError( lineIndex, C64Studio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Macro name must be formatted like a global label" );
        hadError = true;
      }
      else
      {
        string macroName = lineTokenInfos[1].Content;
        

        if ( macroFunctions.ContainsKey( macroName ) )
        {
          AddError( lineIndex, C64Studio.Types.ErrorCode.E1200_REDEFINITION_OF_LABEL, "Macro function name is already in use" );
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
                AddError( lineIndex, C64Studio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Parameter names must be separated by comma" );
                hadError = true;
              }
            }
            else if ( ( !shouldParameterBeAComma )
            &&        ( ( lineTokenInfos[i].Type != C64Studio.Types.TokenInfo.TokenType.OPERATOR )
            ||          ( ( lineTokenInfos[i].Type == C64Studio.Types.TokenInfo.TokenType.OPERATOR )
            &&            ( lineTokenInfos[i].Content != "~" ) ) )
            &&        ( lineTokenInfos[i].Type != C64Studio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
            &&        ( lineTokenInfos[i].Type != C64Studio.Types.TokenInfo.TokenType.LABEL_LOCAL ) )
            {
              AddError( lineIndex, C64Studio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Parameter name must be formatted like a global label" );
              hadError = true;
            }
            else if ( ( !shouldParameterBeAComma )
            &&        ( lineTokenInfos[i].Type == C64Studio.Types.TokenInfo.TokenType.OPERATOR )
            &&        ( lineTokenInfos[i].Content == "~" ) )
            {
              if ( ( i + 1 >= lineTokenInfos.Count )
              ||   ( ( lineTokenInfos[i + 1].Type != C64Studio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
              &&     ( lineTokenInfos[i + 1].Type != C64Studio.Types.TokenInfo.TokenType.LABEL_LOCAL ) ) )
              {
                AddError( lineIndex, C64Studio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Syntax error, expected parameter name after ~" );
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
            Types.MacroFunctionInfo macroFunction = new C64Studio.Types.MacroFunctionInfo();

            macroFunction.Name            = macroName;
            macroFunction.LineIndex       = lineIndex;
            macroFunction.ParentFileName  = OuterFilename;
            macroFunction.ParameterNames  = param;
            macroFunction.ParametersAreReferences = paramIsRef;
            macroFunction.UsesBracket     = hasBracket;

            macroFunctions.Add( macroName, macroFunction );

            MacroFunctionName = macroName;

            // macro scope
            Types.ScopeInfo scope = new C64Studio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.MACRO_FUNCTION );

            scope.StartIndex  = lineIndex;
            scope.Macro       = macroFunction;
            scope.Active      = true;
            Scopes.Add( scope );

            //Debug.Log( "add macro scope for " + macroName + " in line " + lineIndex );
          }
        }
      }
      return !hadError;
    }



    private void AddVirtualBreakpoint( int LineIndex, string DocumentFilename, string Expression )
    {
      Types.Breakpoint    virtualBP = new C64Studio.Types.Breakpoint();
      virtualBP.LineIndex = LineIndex;
      virtualBP.Expression  = Expression;
      virtualBP.DocumentFilename = DocumentFilename;
      ASMFileInfo.VirtualBreakpoints.Add( LineIndex, virtualBP );
    }



    private void CloneSourceInfos( int SourceIndex, int CopyLength, int TargetIndex )
    {
      List<Types.ASM.SourceInfo>    infosToAdd = new List<C64Studio.Types.ASM.SourceInfo>();

      foreach ( Types.ASM.SourceInfo oldInfo in ASMFileInfo.SourceInfo.Values )
      {
        if ( ( oldInfo.LineCount != -1 )
        &&   ( oldInfo.GlobalStartLine >= SourceIndex )
        &&   ( oldInfo.GlobalStartLine + oldInfo.LineCount <= SourceIndex + CopyLength ) )
        {
          // fully inside source scope
          // need to copy!
          Types.ASM.SourceInfo tempInfo = new C64Studio.Types.ASM.SourceInfo();

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
      foreach ( var zoneInfo in ASMFileInfo.Zones.Values )
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

      List<Types.ASM.SourceInfo> movedInfos = new List<Types.ASM.SourceInfo>();
      foreach ( Types.ASM.SourceInfo oldInfo in ASMFileInfo.SourceInfo.Values )
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
        foreach ( int key in ASMFileInfo.SourceInfo.Keys )
        {
          if ( ASMFileInfo.SourceInfo[key] == oldInfo )
          {
            ASMFileInfo.SourceInfo.Remove( key );
            break;
          }
        }
      }

      bool    dumpInfos = false;

      if ( ASMFileInfo.SourceInfo.ContainsKey( sourceInfo.GlobalStartLine ) )
      {
        Debug.Log( "Source Info already exists at global line index " + sourceInfo.GlobalStartLine );
        return;
      }

      ASMFileInfo.SourceInfo.Add( sourceInfo.GlobalStartLine, sourceInfo );
      foreach ( Types.ASM.SourceInfo oldInfo in movedInfos )
      {
        if ( oldInfo.LineCount != 0 )
        {
          if ( ASMFileInfo.SourceInfo.ContainsKey( oldInfo.GlobalStartLine ) )
          {
            Debug.Log( "Trying to insert duplicate source info at global line index " + oldInfo.GlobalStartLine );
            dumpInfos = true;
          }
          else
          {
            ASMFileInfo.SourceInfo.Add( oldInfo.GlobalStartLine, oldInfo );
          }
        }
      }

      if ( dumpInfos )
      {
        // dump source infos
        int fullLines = 0;
        foreach ( var pair in ASMFileInfo.SourceInfo )
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
      m_CompileTarget = Types.CompileTargetType.PRG;
      m_CompileTargetFile = null;
      m_CompileCurrentAddress = -1;
      ExternallyIncludedFiles.Clear();

      AssembledOutput = null;
      Messages.Clear();
      m_ErrorMessages = 0;
      m_WarningMessages = 0;
      ASMFileInfo = new C64Studio.Types.ASM.FileInfo();
      m_LoadedFiles.Clear();
      m_Filename = "";
    }



    public override GR.Collections.MultiMap<string, Types.SymbolInfo> KnownTokenInfo()
    {
      GR.Collections.MultiMap<string, Types.SymbolInfo> knownTokens = new GR.Collections.MultiMap<string, Types.SymbolInfo>();

      foreach ( KeyValuePair<string, Types.SymbolInfo> zone in ASMFileInfo.Zones )
      {
        DocumentAndLineFromGlobalLine( zone.Value.LineIndex, out zone.Value.DocumentFilename, out zone.Value.LocalLineIndex );
        knownTokens.Add( zone.Key, zone.Value );
      }
      foreach ( KeyValuePair<string, Types.SymbolInfo> label in ASMFileInfo.Labels )
      {
        if ( !label.Value.FromDependency )
        {
          DocumentAndLineFromGlobalLine( label.Value.LineIndex, out label.Value.DocumentFilename, out label.Value.LocalLineIndex );
        }
        knownTokens.Add( label.Key, label.Value );
      }
      foreach ( KeyValuePair<string, Types.ASM.UnparsedEvalInfo> label in ASMFileInfo.UnparsedLabels )
      {
        Types.SymbolInfo token = new C64Studio.Types.SymbolInfo();

        token.Name = label.Key;
        DocumentAndLineFromGlobalLine( label.Value.LineIndex, out token.DocumentFilename, out token.LineIndex );
        knownTokens.Add( token.Name, token );
      }
      return knownTokens;
    }



    public override List<Types.AutoCompleteItemInfo> KnownTokens()
    {
      List<Types.AutoCompleteItemInfo> knownTokens = new List<Types.AutoCompleteItemInfo>();

      //knownTokens.AddRange( m_Zones.Keys );
      foreach ( var label in ASMFileInfo.Labels )
      {
        knownTokens.Add( new Types.AutoCompleteItemInfo() { Symbol = label.Value, Token = label.Key, ToolTipTitle = label.Key } );
      }
      foreach ( var unparsedLabel in ASMFileInfo.UnparsedLabels )
      {
        knownTokens.Add( new Types.AutoCompleteItemInfo() { Token = unparsedLabel.Key, ToolTipTitle = unparsedLabel.Key } );
      }
      foreach ( var opcode in m_Processor.Opcodes )
      {
        knownTokens.Add( new Types.AutoCompleteItemInfo() { Token = opcode.Key, ToolTipTitle = opcode.Key } );
      }
      return knownTokens;
    }



    public void ParseAndAddPreDefines( string PreDefines )
    {
      string[]    makros = PreDefines.Split( '\n' );

      foreach ( string makro in makros )
      {
        string singleMakro = makro.Trim();

        List<Types.TokenInfo> lineTokens = ParseTokenInfo( singleMakro, 0, singleMakro.Length );

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
          List<Types.TokenInfo>  valueTokens = ParseTokenInfo( defineValue, 0, defineValue.Length );
          int address = -1;
          if ( !EvaluateTokens( -1, valueTokens, out address ) )
          {
            AddError( -1, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Cannot evaluate predefine expression " + defineValue );
          }
          else
          {
            AddPreprocessorConstant( defineName, address, -1 );
          }
        }
      }
    }



    private void DumpTempLabelInfos()
    {
      foreach ( var entry in ASMFileInfo.TempLabelInfo )
      {
        Debug.Log( "From line " + ( entry.LineIndex + 1 ) + " to " + ( entry.LineIndex + 1 + entry.LineCount - 1 ) + ", name " + entry.Name + ", value " + entry.Value );
      }
    }



    private void DumpTempLabelInfos( string Name )
    {
      foreach ( var entry in ASMFileInfo.TempLabelInfo )
      {
        if ( entry.Name == Name )
        {
          Debug.Log( "From line " + ( entry.LineIndex + 1 ) + " to " + ( entry.LineIndex + 1 + entry.LineCount - 1 ) + ", name " + entry.Name + ", value " + entry.Value );
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
      foreach ( KeyValuePair<int,Types.ASM.SourceInfo> pair in ASMFileInfo.SourceInfo )
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



    public override bool Parse( string Content, ProjectConfig Configuration, CompileConfig Config )
    {
      m_CompileConfig = Config;

      m_AssemblerSettings.SetAssemblerType( Config.Assembler );

      string[] lines = Content.Replace( "\r\n", "\n" ).Replace( '\r', '\n' ).Replace( '\t', ' ' ).Split( '\n' );

      CleanLines( lines );
      //Debug.Log( "Filesplit" );

      Types.ASM.SourceInfo sourceInfo = new Types.ASM.SourceInfo();
      sourceInfo.Filename         = m_Filename;
      sourceInfo.GlobalStartLine  = 0;
      sourceInfo.LineCount        = lines.Length;
      sourceInfo.FullPath         = m_Filename;

      ASMFileInfo = new C64Studio.Types.ASM.FileInfo();
      ASMFileInfo.SourceInfo.Add( sourceInfo.GlobalStartLine, sourceInfo );
      ASMFileInfo.AssemblerSettings = m_AssemblerSettings;
      ASMFileInfo.LabelDumpFile     = Config.LabelDumpFile;

      m_WarningsToIgnore.Clear();

      lines = PreProcess( lines, m_Filename, Configuration );

      DumpSourceInfos( OrigLines, lines );

      DetermineUnparsedLabels();

      ASMFileInfo.PopulateAddressToLine();

      foreach ( Types.SymbolInfo token in ASMFileInfo.Labels.Values )
      {
        if ( ( !token.Used )
        &&   ( token.Name != "*" )
        &&   ( !token.Name.Contains( AssemblerSettings.INTERNAL_LOCAL_LOOP_LABEL_PREFIX ) )
        &&   ( token.Type != C64Studio.Types.SymbolInfo.Types.PREPROCESSOR_LABEL ) )
        {
          AddWarning( token.LineIndex, 
                      Types.ErrorCode.W1000_UNUSED_LABEL, 
                      "Unused label " + token.Name,
                      token.CharIndex,
                      token.Length );
        }
      }

      // create preprocessed file even with errors (might be the reason to get the preprocessed file in the first place)
      if ( Config.CreatePreProcesseFile )
      {
        CreatePreProcessedFile( Config.InputFile, lines, ASMFileInfo );
      }

      if ( ( ASMFileInfo.UnparsedLabels.Count > 0 )
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
            writer.Write( i.ToString( formatString ) );
            writer.Write( "  " );

            if ( FileInfo.LineInfo.ContainsKey( i ) )
            {
              var     info = FileInfo.LineInfo[i];
              if ( info != null )
              {
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
                writer.Write( "????              " );
                writer.WriteLine( Lines[i].TrimStart() );
              }
            }
            else
            {
              writer.Write( "????              " );
              writer.WriteLine( Lines[i].TrimStart() );
            }
          }
          writer.Close();
        }

        ParseMessage message = new ParseMessage( ParseMessage.LineType.MESSAGE, Types.ErrorCode.OK, "Preprocessed file written to " + pathLog );
        message.AlternativeFile = pathLog;
        message.AlternativeLineIndex = 0;
        Messages.Add( -1, message );
        ++m_Messages;
      }
      catch ( Exception ex )
      {
        AddWarning( -1, Types.ErrorCode.E1401_INTERNAL_ERROR, "Can't write preprocessed file:" + ex.Message, 0, 0 );
      }
    }



    private bool TargetTypeRequiresLoadAddress( Types.CompileTargetType Type )
    {
      if ( ( Type != Types.CompileTargetType.PLAIN )
      &&   ( Type != C64Studio.Types.CompileTargetType.CARTRIDGE_16K_BIN )
      &&   ( Type != C64Studio.Types.CompileTargetType.CARTRIDGE_16K_CRT )
      &&   ( Type != C64Studio.Types.CompileTargetType.CARTRIDGE_8K_BIN )
      &&   ( Type != C64Studio.Types.CompileTargetType.CARTRIDGE_8K_CRT )
      &&   ( Type != C64Studio.Types.CompileTargetType.CARTRIDGE_EASYFLASH_BIN )
      &&   ( Type != C64Studio.Types.CompileTargetType.CARTRIDGE_EASYFLASH_CRT )
      &&   ( Type != C64Studio.Types.CompileTargetType.CARTRIDGE_GMOD2_BIN )
      &&   ( Type != C64Studio.Types.CompileTargetType.CARTRIDGE_GMOD2_CRT )
      &&   ( Type != C64Studio.Types.CompileTargetType.CARTRIDGE_RGCD_BIN )
      &&   ( Type != C64Studio.Types.CompileTargetType.CARTRIDGE_RGCD_CRT )
      &&   ( Type != C64Studio.Types.CompileTargetType.CARTRIDGE_MAGICDESK_BIN )
      &&   ( Type != C64Studio.Types.CompileTargetType.CARTRIDGE_MAGICDESK_CRT ) )
      {
        return true;
      }
      return false;
    }



    public override bool Assemble( CompileConfig Config )
    {
      if ( Config.OutputFile == null )
      {
        AddError( -1, C64Studio.Types.ErrorCode.E0001_NO_OUTPUT_FILENAME, "No output file name provided" );
        return false;
      }
      m_CompileTargetFile = Config.OutputFile;

      GR.Memory.ByteBuffer    result = new GR.Memory.ByteBuffer();
      GR.Memory.ByteBuffer    currentResultBlock = new GR.Memory.ByteBuffer();

      Types.MemoryMap         memoryMap = new Types.MemoryMap();

      List<GR.Generic.Tupel<int,Types.ASMSegment>>      builtSegments = new List<GR.Generic.Tupel<int, Types.ASMSegment>>();


      int     memoryBlockStartAddress = -1;
      int     memoryBlockLength = 0;


      int     currentAddress = -1;
      int     trueCurrentAddress = -1;
      bool    startBytesSet = false;
      int     fileStartAddress = -1;
      int     dataOffset = 0;
      foreach ( Types.ASM.LineInfo line in ASMFileInfo.LineInfo.Values )
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
              fileStartAddress = currentAddress;

              if ( TargetTypeRequiresLoadAddress( Config.TargetType ) )
              {
                result.AppendU16( (ushort)currentAddress );
                dataOffset = 2;
              }
            }
            memoryBlockStartAddress = currentAddress;


            //Debug.Log( "ASM - new block starts at " + line.AddressStart );

            var asmSegment = new Types.ASMSegment();
            asmSegment.StartAddress = line.AddressStart;
            asmSegment.GlobalLineIndex = line.LineIndex;
            ASMFileInfo.FindTrueLineSource( line.LineIndex, out asmSegment.Filename, out asmSegment.LocalLineIndex );

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
            ASMFileInfo.FindTrueLineSource( line.LineIndex, out asmSegment.Filename, out asmSegment.LocalLineIndex );
            builtSegments.Add( new GR.Generic.Tupel<int, Types.ASMSegment>( asmSegment.StartAddress, asmSegment ) );
            currentResultBlock = asmSegment.Data;
          }
          //Debug.Log( "Address jump from " + currentAddress.ToString( "x" ) + " to " + line.AddressStart.ToString( "x" ) );
          //Debug.Log( "-at line " + line.Line );
          if ( line.LineData != null )
          {
            //Debug.Log( "-at line index " + line.LineData.ToString() );
          }

          /*
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
            }
          }
          else
          {
            // was pseudo PC before
            if ( line.PseudoPCOffset == -2 )
            {
              // entered REALPC
            }
          }*/

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

                //Debug.Log( "Filler!" );

                if ( currentAddress - fileStartAddress + newBytes > result.Length )
                {
                  memoryMap.InsertEntry( new Types.MemoryMapEntry( memoryBlockStartAddress, currentAddress - memoryBlockStartAddress ) );

                  result.Append( new GR.Memory.ByteBuffer( (uint)( currentAddress - fileStartAddress + newBytes - result.Length ) ) );

                  memoryBlockStartAddress = line.AddressStart;
                  memoryBlockLength = 0;
                }
                //result.Append( new GR.Memory.ByteBuffer( (uint)( line.AddressStart - currentAddress ) ) );
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
                memoryMap.InsertEntry( new Types.MemoryMapEntry( memoryBlockStartAddress, currentAddress - memoryBlockStartAddress ) );

                result.Append( new GR.Memory.ByteBuffer( (uint)( line.AddressStart - currentAddress ) ) );

                memoryBlockStartAddress = line.AddressStart;
                memoryBlockLength = 0;
              }
            }
          }
          if ( setNewAddress )
          {
            currentAddress = line.AddressStart;
            memoryBlockLength = currentAddress - memoryBlockStartAddress;
          }
        }
        if ( line.LineData != null )
        {
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
      }

      memoryBlockLength = currentAddress - memoryBlockStartAddress;
      if ( memoryBlockLength > 0 )
      {
        memoryMap.InsertEntry( new Types.MemoryMapEntry( memoryBlockStartAddress, memoryBlockLength ) );
      }

      int     previousLabelStart = -1;
      string  previousLabelName = "";
      foreach ( var label in ASMFileInfo.Labels )
      {
        if ( ( label.Value.Type == Types.SymbolInfo.Types.LABEL )
        &&   ( label.Key.IndexOf( '.' ) == -1 )
        &&   ( !label.Key.StartsWith( InternalLabelPrefix ) ) )
        {
          if ( previousLabelStart != -1 )
          {
            memoryMap.InsertEntry( new Types.MemoryMapEntry( previousLabelStart, label.Value.AddressOrValue - previousLabelStart, previousLabelName ) );
          }
          previousLabelStart = label.Value.AddressOrValue;
          previousLabelName = label.Key;
        }
      }
      /*
      foreach ( string label in m_Labels.Keys )
      {
        dh.Log( "Label " + label + " = " + m_Labels[label].AddressOrValue.ToString( "x" ) );
      }
       */

      //memoryMap.Dump();

      // determine load address
      int lowestStart = 65536;
      int highestEnd = -1;
      foreach ( var segment in builtSegments )
      {
        lowestStart = Math.Min( segment.first, lowestStart );
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
      // combine blocks
      GR.Memory.ByteBuffer    assembledData = new GR.Memory.ByteBuffer();

      if ( builtSegments.Count > 0 )
      {
        assembledData = new GR.Memory.ByteBuffer( (uint)( highestEnd - lowestStart ) );
        foreach ( var segment in builtSegments )
        {
          segment.second.Data.CopyTo( assembledData, 0, segment.second.Length, segment.first - lowestStart );
        }
      }
      
      // prefix load address
      if ( TargetTypeRequiresLoadAddress( Config.TargetType ) )
      {
        assembledData = new GR.Memory.ByteBuffer( 2 ) + assembledData;
        assembledData.SetU16At( 0, (ushort)lowestStart );
      }

      //Assembly = result;
      AssembledOutput = new AssemblyOutput();
      AssembledOutput.Assembly = assembledData;
      AssembledOutput.OriginalAssemblyStartAddress  = lowestStart;
      AssembledOutput.OriginalAssemblySize          = highestEnd - lowestStart;

      string    outputPureFilename = "HURZ";
      try
      {
        outputPureFilename = System.IO.Path.GetFileNameWithoutExtension( Config.OutputFile );
      }
      catch ( Exception )
      {
        // arghh exceptions!
      }

      if ( Config.TargetType == Types.CompileTargetType.T64 )
      {
        Formats.T64 t64 = new C64Studio.Formats.T64();

        Formats.T64.FileRecord  record = new C64Studio.Formats.T64.FileRecord();

        record.Filename = Util.ToFilename( outputPureFilename );
        record.StartAddress = (ushort)fileStartAddress;
        record.C64FileType = C64Studio.Types.FileType.PRG;
        record.EntryType = 1;

        t64.TapeInfo.Description = "C64S tape file\r\nDemo tape";
        t64.TapeInfo.UserDescription = "USERDESC";
        t64.FileRecords.Add( record );
        t64.FileDatas.Add( AssembledOutput.Assembly );

        AssembledOutput.Assembly = t64.Compile();
      }
      else if ( Config.TargetType == Types.CompileTargetType.TAP )
      {
        Formats.Tap tap = new C64Studio.Formats.Tap();

        tap.WriteFile( Util.ToFilename( outputPureFilename ), AssembledOutput.Assembly, C64Studio.Types.FileType.PRG );
        AssembledOutput.Assembly = tap.Compile();
      }
      else if ( Config.TargetType == Types.CompileTargetType.D64 )
      {
        Formats.D64 d64 = new C64Studio.Formats.D64();

        d64.CreateEmptyMedia();

        GR.Memory.ByteBuffer    bufName = Util.ToFilename( outputPureFilename );
        d64.WriteFile( bufName, AssembledOutput.Assembly, C64Studio.Types.FileType.PRG );

        AssembledOutput.Assembly = d64.Compile();
      }
      else if ( ( Config.TargetType == Types.CompileTargetType.CARTRIDGE_8K_BIN )
      || ( Config.TargetType == Types.CompileTargetType.CARTRIDGE_8K_CRT ) )
      {
        if ( AssembledOutput.Assembly.Length < 8192 )
        {
          // fill up
          AssembledOutput.Assembly = AssembledOutput.Assembly + new GR.Memory.ByteBuffer( 8192 - AssembledOutput.Assembly.Length );
        }
        else if ( AssembledOutput.Assembly.Length > 8192 )
        {
          AddError( 0, Types.ErrorCode.E1102_PROGRAM_TOO_LARGE, "Assembly too large, " + AssembledOutput.Assembly.Length.ToString() + " > 8192" );
          return false;
        }
        if ( Config.TargetType == Types.CompileTargetType.CARTRIDGE_8K_CRT )
        {
          // build cartridge header
          GR.Memory.ByteBuffer    header = new GR.Memory.ByteBuffer();

          header.AppendHex( "43363420434152545249444745202020" ); // "C64 CARTRIDGE   "
          header.AppendU32NetworkOrder( 0x40 );     // file header length
          header.AppendU16NetworkOrder( 0x0100 );   // version (currently only 1.00)
          header.AppendU16( 0 );                    // cartridge type
          header.AppendU8( 0 );  // exrom
          header.AppendU8( 0 );  // game

          // reserved
          header.AppendU8( 0 );
          header.AppendU8( 0 );
          header.AppendU8( 0 );
          header.AppendU8( 0 );
          header.AppendU8( 0 );
          header.AppendU8( 0 );

          // cartridge name
          string name = System.IO.Path.GetFileNameWithoutExtension( m_CompileTargetFile ).ToUpper();

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

          GR.Memory.ByteBuffer chip = new GR.Memory.ByteBuffer();

          chip.AppendHex( "43484950" );   // chip
          uint length = 16 + AssembledOutput.Assembly.Length;
          chip.AppendU32NetworkOrder( length );
          chip.AppendU16( 0 );  // ROM
          chip.AppendU16( 0 );  // Bank number
          chip.AppendU16NetworkOrder( 0x8000 ); // loading start address
          chip.AppendU16NetworkOrder( 0x2000 ); // rom size

          chip.Append( AssembledOutput.Assembly );

          AssembledOutput.Assembly = header + chip;
        }
      }
      else if ( ( Config.TargetType == Types.CompileTargetType.CARTRIDGE_16K_BIN )
      || ( Config.TargetType == Types.CompileTargetType.CARTRIDGE_16K_CRT ) )
      {
        if ( AssembledOutput.Assembly.Length < 16384 )
        {
          // fill up
          AssembledOutput.Assembly = AssembledOutput.Assembly + new GR.Memory.ByteBuffer( 16384 - AssembledOutput.Assembly.Length );
        }
        else if ( AssembledOutput.Assembly.Length > 16384 )
        {
          AddError( 0, Types.ErrorCode.E1102_PROGRAM_TOO_LARGE, "Assembly too large, " + AssembledOutput.Assembly.Length.ToString() + " > 16384" );
          return false;
        }
        if ( Config.TargetType == Types.CompileTargetType.CARTRIDGE_16K_CRT )
        {
          // build cartridge header
          GR.Memory.ByteBuffer    header = new GR.Memory.ByteBuffer();

          header.AppendHex( "43363420434152545249444745202020" ); // "C64 CARTRIDGE   "
          header.AppendU32NetworkOrder( 0x40 );
          header.AppendU16NetworkOrder( 0x0100 );
          header.AppendU16( 0 );
          header.AppendU8( 0 );  // exrom
          header.AppendU8( 0 );  // game

          // reserved
          header.AppendU8( 0 );
          header.AppendU8( 0 );
          header.AppendU8( 0 );
          header.AppendU8( 0 );
          header.AppendU8( 0 );
          header.AppendU8( 0 );

          // cartridge name
          string name = System.IO.Path.GetFileNameWithoutExtension( m_CompileTargetFile ).ToUpper();

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

          GR.Memory.ByteBuffer chip = new GR.Memory.ByteBuffer();

          chip.AppendHex( "43484950" );   // chip
          uint length = 16 + AssembledOutput.Assembly.Length;
          chip.AppendU32NetworkOrder( length );
          chip.AppendU16( 0 );  // ROM
          chip.AppendU16( 0 );  // Bank number
          chip.AppendU16NetworkOrder( 0x8000 ); // loading start address
          chip.AppendU16NetworkOrder( 0x4000 ); // rom size

          chip.Append( AssembledOutput.Assembly );

          AssembledOutput.Assembly = header + chip;
        }
      }
      else if ( ( Config.TargetType == Types.CompileTargetType.CARTRIDGE_MAGICDESK_BIN )
      || ( Config.TargetType == Types.CompileTargetType.CARTRIDGE_MAGICDESK_CRT ) )
      {
        if ( AssembledOutput.Assembly.Length < 65536 )
        {
          // fill up
          AssembledOutput.Assembly = AssembledOutput.Assembly + new GR.Memory.ByteBuffer( 65536 - AssembledOutput.Assembly.Length );
        }
        else if ( AssembledOutput.Assembly.Length > 65536 )
        {
          AddError( 0, Types.ErrorCode.E1102_PROGRAM_TOO_LARGE, "Assembly too large, " + AssembledOutput.Assembly.Length.ToString() + " > 65536" );
          return false;
        }
        if ( Config.TargetType == Types.CompileTargetType.CARTRIDGE_MAGICDESK_CRT )
        {
          // build cartridge header
          GR.Memory.ByteBuffer    header = new GR.Memory.ByteBuffer();

          header.AppendHex( "43363420434152545249444745202020" ); // "C64 CARTRIDGE   "
          header.AppendU32NetworkOrder( 0x40 );
          header.AppendU16NetworkOrder( 0x0100 );
          header.AppendU16NetworkOrder( 0x13 ); // Magic Desk
          header.AppendU8( 0 );     // EXROM
          header.AppendU8( 1 );     // GAME

          // reserved
          header.AppendU8( 0 );
          header.AppendU8( 0 );
          header.AppendU8( 0 );
          header.AppendU8( 0 );
          header.AppendU8( 0 );
          header.AppendU8( 0 );

          // cartridge name
          string name = System.IO.Path.GetFileNameWithoutExtension( m_CompileTargetFile ).ToUpper();

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

          // 8 x 8kb
          var   assembledCode = AssembledOutput.Assembly;
          AssembledOutput.Assembly = header;
          for ( int i = 0; i < 8; ++i )
          {
            GR.Memory.ByteBuffer chip = new GR.Memory.ByteBuffer();

            chip.AppendHex( "43484950" );   // chip
            uint length = 16 + 8192;
            chip.AppendU32NetworkOrder( length );
            chip.AppendU16NetworkOrder( 0 );  // ROM
            chip.AppendU16NetworkOrder( (ushort)i );  // Bank number
            chip.AppendU16NetworkOrder( 0x8000 ); // loading start address
            chip.AppendU16NetworkOrder( 0x2000 ); // rom size

            chip.Append( assembledCode.SubBuffer( i * 0x2000, 0x2000 ) );

            AssembledOutput.Assembly += chip;
          }
        }
      }
      else if ( ( Config.TargetType == Types.CompileTargetType.CARTRIDGE_RGCD_BIN )
      || ( Config.TargetType == Types.CompileTargetType.CARTRIDGE_RGCD_CRT ) )
      {
        if ( AssembledOutput.Assembly.Length < 65536 )
        {
          // fill up
          AssembledOutput.Assembly = AssembledOutput.Assembly + new GR.Memory.ByteBuffer( 65536 - AssembledOutput.Assembly.Length );
        }
        else if ( AssembledOutput.Assembly.Length > 65536 )
        {
          AddError( 0, Types.ErrorCode.E1102_PROGRAM_TOO_LARGE, "Assembly too large, " + AssembledOutput.Assembly.Length.ToString() + " > 65536" );
          return false;
        }
        if ( Config.TargetType == Types.CompileTargetType.CARTRIDGE_RGCD_CRT )
        {
          // build cartridge header
          GR.Memory.ByteBuffer    header = new GR.Memory.ByteBuffer();

          header.AppendHex( "43363420434152545249444745202020" ); // "C64 CARTRIDGE   "
          header.AppendU32NetworkOrder( 0x40 );
          header.AppendU16NetworkOrder( 0x0100 );
          header.AppendU16NetworkOrder( 0x39 ); // RGCD
          header.AppendU8( 0 );     // EXROM
          header.AppendU8( 1 );     // GAME

          // reserved
          header.AppendU8( 0 );
          header.AppendU8( 0 );
          header.AppendU8( 0 );
          header.AppendU8( 0 );
          header.AppendU8( 0 );
          header.AppendU8( 0 );

          // cartridge name
          string name = System.IO.Path.GetFileNameWithoutExtension( m_CompileTargetFile ).ToUpper();

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

          // 8 x 8kb
          var   assembledCode = AssembledOutput.Assembly;
          AssembledOutput.Assembly = header;
          for ( int i = 0; i < 8; ++i )
          {
            GR.Memory.ByteBuffer chip = new GR.Memory.ByteBuffer();

            chip.AppendHex( "43484950" );   // chip
            uint length = 16 + 8192;
            chip.AppendU32NetworkOrder( length );
            chip.AppendU16NetworkOrder( 0 );  // ROM
            chip.AppendU16NetworkOrder( (ushort)i );  // Bank number
            chip.AppendU16NetworkOrder( 0x8000 ); // loading start address
            chip.AppendU16NetworkOrder( 0x2000 ); // rom size

            chip.Append( assembledCode.SubBuffer( i * 0x2000, 0x2000 ) );

            AssembledOutput.Assembly += chip;
          }
        }
      }
      else if ( ( Config.TargetType == Types.CompileTargetType.CARTRIDGE_EASYFLASH_BIN )
      || ( Config.TargetType == Types.CompileTargetType.CARTRIDGE_EASYFLASH_CRT ) )
      {
        GR.Memory.ByteBuffer    resultingAssembly = AssembledOutput.Assembly;
        if ( resultingAssembly.Length < 524288 )
        {
          // fill up
          resultingAssembly = resultingAssembly + new GR.Memory.ByteBuffer( 524288 - resultingAssembly.Length );
        }
        else if ( resultingAssembly.Length > 524288 )
        {
          AddError( 0, Types.ErrorCode.E1102_PROGRAM_TOO_LARGE, "Assembly too large, " + resultingAssembly.Length.ToString() + " > 524288" );
          return false;
        }
        if ( Config.TargetType == Types.CompileTargetType.CARTRIDGE_EASYFLASH_CRT )
        {
          // build cartridge header
          GR.Memory.ByteBuffer    header = new GR.Memory.ByteBuffer();

          header.AppendHex( "43363420434152545249444745202020" ); // "C64 CARTRIDGE   "
          header.AppendU32NetworkOrder( 0x40 );
          header.AppendU16NetworkOrder( 0x0100 );
          header.AppendU16NetworkOrder( 0x20 ); // Easyflash
          header.AppendU8( 1 );     // EXROM
          header.AppendU8( 0 );     // GAME

          // reserved
          header.AppendU8( 0 );
          header.AppendU8( 0 );
          header.AppendU8( 0 );
          header.AppendU8( 0 );
          header.AppendU8( 0 );
          header.AppendU8( 0 );

          // cartridge name
          string name = System.IO.Path.GetFileNameWithoutExtension( m_CompileTargetFile ).ToUpper();

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

          // 64 x 8kb
          AssembledOutput.Assembly = header;
          for ( int i = 0; i < 64; ++i )
          {
            GR.Memory.ByteBuffer chip = new GR.Memory.ByteBuffer();

            chip.AppendHex( "43484950" );   // chip
            uint length = 16 + 8192;
            chip.AppendU32NetworkOrder( length );
            chip.AppendU16NetworkOrder( 2 );  // Flash ROM
            chip.AppendU16NetworkOrder( (ushort)( i / 2 ) );  // Bank number
            if ( ( i % 2 ) == 0 )
            {
              chip.AppendU16NetworkOrder( 0x8000 ); // loading start address
            }
            else
            {
              chip.AppendU16NetworkOrder( 0xA000 ); // loading start address
            }
            chip.AppendU16NetworkOrder( 0x2000 ); // rom size

            chip.Append( resultingAssembly.SubBuffer( i * 0x2000, 0x2000 ) );

            AssembledOutput.Assembly += chip;
          }
        }
        else
        {
          AssembledOutput.Assembly = resultingAssembly;
        }
      }
      else if ( ( Config.TargetType == Types.CompileTargetType.CARTRIDGE_GMOD2_BIN )
      ||        ( Config.TargetType == Types.CompileTargetType.CARTRIDGE_GMOD2_CRT ) )
      {
        GR.Memory.ByteBuffer    resultingAssembly = AssembledOutput.Assembly;
        if ( resultingAssembly.Length < 524288 )
        {
          // fill up
          resultingAssembly = resultingAssembly + new GR.Memory.ByteBuffer( 524288 - resultingAssembly.Length );
        }
        else if ( resultingAssembly.Length > 524288 )
        {
          AddError( 0, Types.ErrorCode.E1102_PROGRAM_TOO_LARGE, "Assembly too large, " + resultingAssembly.Length.ToString() + " > 524288" );
          return false;
        }
        if ( Config.TargetType == Types.CompileTargetType.CARTRIDGE_GMOD2_CRT )
        {
          // build cartridge header
          GR.Memory.ByteBuffer    header = new GR.Memory.ByteBuffer();

          header.AppendHex( "43363420434152545249444745202020" ); // "C64 CARTRIDGE   "
          header.AppendU32NetworkOrder( 0x40 );
          header.AppendU16NetworkOrder( 0x0100 );
          header.AppendU16NetworkOrder( 60 ); // Easyflash
          header.AppendU8( 0 );     // EXROM
          header.AppendU8( 1 );     // GAME

          // reserved
          header.AppendU8( 0 );
          header.AppendU8( 0 );
          header.AppendU8( 0 );
          header.AppendU8( 0 );
          header.AppendU8( 0 );
          header.AppendU8( 0 );

          // cartridge name
          string name = System.IO.Path.GetFileNameWithoutExtension( m_CompileTargetFile ).ToUpper();

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

          // 64 x 8kb
          AssembledOutput.Assembly = header;
          for ( int i = 0; i < 64; ++i )
          {
            GR.Memory.ByteBuffer chip = new GR.Memory.ByteBuffer();

            chip.AppendHex( "43484950" );   // chip
            uint length = 16 + 8192;
            chip.AppendU32NetworkOrder( length );
            chip.AppendU16NetworkOrder( 0 );
            chip.AppendU16NetworkOrder( (ushort)i );  // Bank number
            chip.AppendU16NetworkOrder( 0x8000 ); // loading start address
            chip.AppendU16NetworkOrder( 0x2000 ); // rom size

            chip.Append( resultingAssembly.SubBuffer( i * 0x2000, 0x2000 ) );

            AssembledOutput.Assembly += chip;
          }
        }
        else
        {
          AssembledOutput.Assembly = resultingAssembly;
        }
      }
      return true;
    }



    /*
    public List<Types.TokenInfo> ParseTokenInfo( string Source, int Start, int Length, out int ErrorPos )
    {
      ErrorPos = -1;

      ErrorInfo   errInfo = null;

      var result = ParseTokenInfo( Source, Start, Length, out errInfo );
      if ( errInfo != null )
      {
        ErrorPos = errInfo.Pos;
      }
      return result;
    }*/



    public List<Types.TokenInfo> ParseTokenInfo( string Source, int Start, int Length )
    {
      ClearErrorInfo();

      List<Types.TokenInfo> result = new List<Types.TokenInfo>();

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
          foreach ( string op in m_OperatorPrecedence.Keys )
          {
            if ( op.Length >= charPos - tokenStartPos + 1 )
            {
              if ( string.Compare( op, 0, Source, tokenStartPos, charPos - tokenStartPos + 1 ) == 0 )
              {
                if ( op.Length == charPos - tokenStartPos + 1 )
                {
                  ++completeOperators;
                }
                ++possibleOperators;
              }
            }
          }
          if ( ( possibleOperators == 1 )
          &&   ( completeOperators == 1 ) )
          {
            if ( ( charPos + 1 < Start + Length )
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
          /*
          foreach ( string op in m_OperatorPrecedence.Keys )
          {
            if ( op.Length >= charPos - tokenStartPos + 1 )
            {
              if ( ( op.Length == charPos - tokenStartPos + 1 )
                string.Compare( op, 0, Source, tokenStartPos, charPos - tokenStartPos + 1 ) == 0 )
              {
                TokenInfo token = new TokenInfo();
                token.Type = currentTokenType;
                token.OriginatingString = Source;
                token.StartPos = tokenStartPos;
                token.Length = charPos - tokenStartPos;
                result.Add( token );

                currentTokenType = TokenInfo.TokenType.UNKNOWN;
                tokenStartPos = charPos + 1;
                ++charPos;
                continue;
              }
            }
          }*/
          // operator must be one of the shorter ones
          foreach ( string op in m_OperatorPrecedence.Keys )
          {
            if ( op.Length == 1 )
            {
              if ( op[0] == Source[charPos - 1] )
              {
                // make sure we don't accidentally identify a pseudo op as operator
                bool    foundPseudoOp = false;

                if ( ( !string.IsNullOrEmpty( ASMFileInfo.AssemblerSettings.MacroPrefix ) )
                &&   ( op[0] == ASMFileInfo.AssemblerSettings.MacroPrefix[0] ) )
                {
                  // is there a pseudo op following?
                  foreach ( var pseudoOp in ASMFileInfo.AssemblerSettings.Macros )
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
                  Types.TokenInfo token = new Types.TokenInfo();
                  token.Type = currentTokenType;
                  token.OriginatingString = Source;
                  token.StartPos = tokenStartPos;
                  token.Length = 1;
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
            m_LastErrorInfo.Set( -1, charPos, 1, C64Studio.Types.ErrorCode.E1000_SYNTAX_ERROR );
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
              Types.TokenInfo token = new Types.TokenInfo();
              token.Type = currentTokenType;
              token.OriginatingString = Source;
              token.StartPos = tokenStartPos;
              token.Length = charPos - tokenStartPos + 1;

              // special case, labels with :; : is not part of label
              if ( ( currentTokenType == C64Studio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
              ||   ( currentTokenType == C64Studio.Types.TokenInfo.TokenType.LABEL_INTERNAL ) )
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
              Types.TokenInfo token = new Types.TokenInfo();
              token.Type = currentTokenType;
              token.OriginatingString = Source;
              token.StartPos = tokenStartPos;
              token.Length = charPos - tokenStartPos;
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
              Types.TokenInfo token = new Types.TokenInfo();
              token.Type = currentTokenType;
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
          else// 
          {
            // auto end token
            Types.TokenInfo token = new Types.TokenInfo();
            token.Type = currentTokenType;
            token.OriginatingString = Source;
            token.StartPos = tokenStartPos;
            token.Length = charPos - tokenStartPos;
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
            Types.TokenInfo token = new Types.TokenInfo();
            token.Type = Types.TokenInfo.TokenType.SEPARATOR;
            token.OriginatingString = Source;
            token.StartPos = tokenStartPos;
            token.Length = 1;
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
            //if ( charPos > 0 )
            {
              foreach ( string op in m_OperatorPrecedence.Keys )
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
                    tokenStartPos    = charPos;
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
            }
            if ( possibleOperators == 0 )
            {
              if ( charPos > tokenStartPos )
              {
                // some chars were before, but they didn't form a token
                m_LastErrorInfo.Set( -1, tokenStartPos, charPos - tokenStartPos, C64Studio.Types.ErrorCode.E1000_SYNTAX_ERROR );
                result.Clear();
                return result;
              }
              foreach ( KeyValuePair<Types.TokenInfo.TokenType, string> pair in m_AssemblerSettings.AllowedTokenStartChars )
              {
                if ( pair.Value.IndexOf( curChar ) != -1 )
                {
                  currentTokenType = pair.Key;
                  tokenStartPos = charPos;

                  if ( currentTokenType == C64Studio.Types.TokenInfo.TokenType.COMMENT )
                  {
                    // the rest of the line is a comment!
                    Types.TokenInfo token = new Types.TokenInfo();
                    token.Type = currentTokenType;
                    token.OriginatingString = Source;
                    token.StartPos = tokenStartPos;
                    token.Length = Length - tokenStartPos;
                    result.Add( token );

                    return result;
                  }
                  break;
                }
              }
              // separator char?
              if ( currentTokenType == C64Studio.Types.TokenInfo.TokenType.UNKNOWN )
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
      }

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

      // collapse % if prefixed to literal number
      if ( result.Count >= 3 )
      {
        for ( int i = 0; i < result.Count - 1; ++i )
        {
          // collapse - with literal to negative number, this requires all kind of checks
          if ( ( i > 0 )
          &&   ( result[i].Content == "-" )
          &&   ( result[i + 1].Type == Types.TokenInfo.TokenType.LITERAL_NUMBER )
          &&   ( result[i].StartPos + result[i].Length == result[i + 1].StartPos )
          &&   ( ( result[i - 1].EndPos + 1 < result[i].StartPos )
          ||     ( ( result[i - 1].EndPos + 1 == result[i].StartPos )
          &&       ( result[i - 1].Type != TokenInfo.TokenType.LITERAL_NUMBER ) ) )
          &&   ( result[i - 1].Type != TokenInfo.TokenType.LABEL_CHEAP_LOCAL )
          &&   ( result[i - 1].Type != TokenInfo.TokenType.LABEL_GLOBAL )
          &&   ( result[i - 1].Type != TokenInfo.TokenType.LABEL_INTERNAL )
          &&   ( result[i - 1].Type != TokenInfo.TokenType.LABEL_LOCAL )
          &&   ( ( result[i - 1].Type != TokenInfo.TokenType.SEPARATOR )
          ||     ( ( result[i - 1].Content != AssemblerSettings.INTERNAL_CLOSING_BRACE )
          &&       ( result[i - 1].Content != ")" ) ) )
          &&   ( result[i - 1].Content != "*" ) )
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
          &&   ( result[i + 1].Type == C64Studio.Types.TokenInfo.TokenType.SEPARATOR )
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
          &&   ( result[i + 1].Type == C64Studio.Types.TokenInfo.TokenType.LABEL_LOCAL )
          &&   ( result[i].StartPos + result[i].Length == result[i + 1].StartPos ) )
          {
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
          token.Type = C64Studio.Types.TokenInfo.TokenType.OPCODE;
        }
      }

      // collapse label with size identifier
      if ( result.Count >= 3 )
      {
        for ( int i = 0; i < result.Count - 2; ++i )
        {
          if ( ( result[i].Type == C64Studio.Types.TokenInfo.TokenType.OPCODE )
          &&   ( result[i].StartPos + result[i].Length == result[i + 1].StartPos )
          &&   ( result[i + 1].Type == C64Studio.Types.TokenInfo.TokenType.OPERATOR )
          &&   ( result[i + 1].Content == "+" )
          &&   ( result[i + 1].StartPos + result[i + 1].Length == result[i + 2].StartPos )
          &&   ( result[i + 2].Type == C64Studio.Types.TokenInfo.TokenType.LITERAL_NUMBER )
          &&   ( ( result[i + 2].Content == "1" )
          ||     ( result[i + 2].Content == "2" ) ) )
          {
            // combine!
            if ( result[i + 2].Content == "1" )
            {
              result[i].Type = C64Studio.Types.TokenInfo.TokenType.OPCODE_FIXED_ZP;
            }
            else
            {
              result[i].Type = C64Studio.Types.TokenInfo.TokenType.OPCODE_FIXED_NON_ZP;
            }
            result.RemoveRange( i + 1, 2 );
            break;
          }
        }
      }

      // macro'd internal labels turn up as global label, operator, global label, collapse into one token
      for ( int i = 0; i < result.Count; ++i )
      {
        if ( ( result[i].Type == C64Studio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
        &&   ( result[i].Content == InternalLabelPrefix ) )
        {
          int     curPos = i + 1;

          while ( true )
          {
            if ( ( result[curPos].Type == C64Studio.Types.TokenInfo.TokenType.OPERATOR )
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
            &&        ( result[curPos].Type == C64Studio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
            &&        ( result[curPos].StartPos == result[curPos - 1].StartPos + result[curPos - 1].Length )
            &&        ( result[curPos].Content.StartsWith( InternalLabelPostfix ) ) )
            {
              // final label
              result[i].Type = C64Studio.Types.TokenInfo.TokenType.LABEL_INTERNAL;
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
      return result;
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



    private Tiny64.Opcode EstimateOpcode( int LineIndex, List<Types.TokenInfo> LineTokens, List<Tiny64.Opcode> PossibleOpcodes, ref Types.ASM.LineInfo info )
    {
      // lineTokens[0] contains the mnemonic
      if ( LineTokens.Count == 0 )
      {
        // can't be, error!
        AddError( LineIndex,
                  Types.ErrorCode.E1300_OPCODE_AMBIGIOUS,
                  "Could not determine correct opcode for empty line", 0, 0 );
        return null;
      }

      if ( ( LineTokens[0].Type == Types.TokenInfo.TokenType.OPCODE_DIRECT_VALUE )
      &&   ( PossibleOpcodes.Count == 1 )
      &&   ( PossibleOpcodes[0].Addressing == Tiny64.Opcode.AddressingType.IMMEDIATE ) )
      {
        // that one is given
        return PossibleOpcodes[0];
      }

      bool endsWithCommaX = false;
      bool endsWithCommaY = false;
      bool oneParamInBrackets = false;
      bool twoParamsInBrackets = false;
      int numBytesFirstParam = 0;
      int expressionTokenStartIndex = 1;
      int expressionTokenCount = LineTokens.Count - 1;
      if ( LineTokens.Count >= 2 )
      {
        if ( ( LineTokens[LineTokens.Count - 2].Content == "," )
        &&   ( LineTokens[LineTokens.Count - 1].Content.ToUpper() == "X" ) )
        {
          endsWithCommaX = true;
          expressionTokenCount = LineTokens.Count - 2 - expressionTokenStartIndex;
        }
        if ( ( LineTokens[LineTokens.Count - 2].Content == "," )
        &&   ( LineTokens[LineTokens.Count - 1].Content.ToUpper() == "Y" ) )
        {
          endsWithCommaY = true;
          expressionTokenCount -= 2;
          expressionTokenCount = LineTokens.Count - 2 - expressionTokenStartIndex;
        }
        if ( LineTokens[1].Content == "(" )
        {
          int tokenPos = 2;
          int numBracketCount = 1;
          expressionTokenStartIndex = 2;
          while ( ( tokenPos < LineTokens.Count )
          &&      ( ( LineTokens[tokenPos].Content != ")" )
          ||        ( numBracketCount > 1 ) ) )
          {
            if ( LineTokens[tokenPos].Content == ")" )
            {
              --numBracketCount;
            }
            else if ( LineTokens[tokenPos].Content == "(" )
            {
              ++numBracketCount;
            }
            if ( LineTokens[tokenPos].Content == "," )
            {
              twoParamsInBrackets = true;
              expressionTokenCount = tokenPos - expressionTokenStartIndex;
              break;
            }
            ++tokenPos;
          }
          if ( !twoParamsInBrackets )
          {
            oneParamInBrackets = true;
            expressionTokenCount = tokenPos - expressionTokenStartIndex;
          }
        }
        else
        {
          // an expression or identifier or address
          // TODO - expressions are built from one or several parts!

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
                  return opcode;
                }
              }
            }
          }


          int value = -1;
          int numGivenBytes = 0;

          // determine addressing from parameter size
          bool  couldEvaluate = EvaluateTokens( LineIndex, LineTokens, expressionTokenStartIndex, expressionTokenCount, out value, out numGivenBytes );
          if ( couldEvaluate )
          //if ( ParseValue( LineIndex, LineTokens[1].Content, out value, out numGivenBytes ) )
          {
            if ( numGivenBytes > 0 )
            {
              numBytesFirstParam = numGivenBytes;
            }
            else if ( ( value & 0xff00 ) != 0 )
            {
              numBytesFirstParam = 2;
            }
            else
            {
              numBytesFirstParam = 1;
            }
          }
          else
          {
            // TODO!!!!
            //AddError( LineIndex, C64Studio.Types.ErrorCode.E1000_SYNTAX_ERROR, "Could not evaluate tokens" );
            //return null;
          }

          // force to 1 byte 
          if ( LineTokens[0].Type == C64Studio.Types.TokenInfo.TokenType.OPCODE_FIXED_ZP )
          {
            numBytesFirstParam = 1;
          }
          else if ( LineTokens[0].Type == C64Studio.Types.TokenInfo.TokenType.OPCODE_FIXED_NON_ZP )
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
        addressing = Tiny64.Opcode.AddressingType.IMMEDIATE;

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
            int     expressionResult = -1;
            if ( EvaluateTokens( LineIndex, extraTokens, out expressionResult ) )
            {
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
          // wrong! cannot be (address),x
          AddError( LineIndex,
                    Types.ErrorCode.E1300_OPCODE_AMBIGIOUS,
                    "Could not determine correct opcode for " + LineTokens[0].Content,
                    LineTokens[0].StartPos,
                    LineTokens[0].Length );

          return null;
          //addressing = Opcode.AddressingType.INDIRECT_X;
        }
        else if ( endsWithCommaY )
        {
          addressing = Tiny64.Opcode.AddressingType.INDIRECT_Y;
        }
        else
        {
          addressing = Tiny64.Opcode.AddressingType.INDIRECT;
        }
      }
      else if ( twoParamsInBrackets )
      {
        addressing = Tiny64.Opcode.AddressingType.INDIRECT_X;
      }
      else
      {
        addressing = Tiny64.Opcode.AddressingType.IMPLICIT;
      }

      foreach ( Tiny64.Opcode opcode in PossibleOpcodes )
      {
        if ( opcode.Addressing == addressing )
        {
          return opcode;
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
      // was in braces, could be simple braces around
      if ( addressing == Tiny64.Opcode.AddressingType.INDIRECT )
      {
        AddError( LineIndex, Types.ErrorCode.E1105_INVALID_OPCODE, "Opcode does not support indirect addressing (remove braces)" );
        return null;
        /*
        // could be absolute, but also zeropage, determine by parameter size
        int value = -1;
        int numGivenBytes = 0;

        // determine addressing from parameter size
        if ( EvaluateTokens( LineIndex, LineTokens, expressionTokenStartIndex, expressionTokenCount, out value, out numGivenBytes ) )
        //if ( ParseValue( LineIndex, LineTokens[1].Content, out value, out numGivenBytes ) )
        {
          if ( numGivenBytes > 0 )
          {
            if ( numGivenBytes == 1 )
            {
              addressing = Tiny64.Opcode.AddressingType.ZEROPAGE;
            }
            else
            {
              addressing = Tiny64.Opcode.AddressingType.ABSOLUTE;
            }
          }
          else if ( ( value & 0xff00 ) != 0 )
          {
            addressing = Tiny64.Opcode.AddressingType.ABSOLUTE;
          }
          else
          {
            addressing = Tiny64.Opcode.AddressingType.ZEROPAGE;
          }
        }
        else
        {
          addressing = Tiny64.Opcode.AddressingType.ABSOLUTE;
        }*/
      }
      foreach ( Tiny64.Opcode opcode in PossibleOpcodes )
      {
        if ( opcode.Addressing == addressing )
        {
          return opcode;
        }
      }

      AddError( LineIndex,
          Types.ErrorCode.E1300_OPCODE_AMBIGIOUS,
          "Could not determine correct opcode for " + LineTokens[0].Content,
          LineTokens[0].StartPos,
          LineTokens[0].Length );

      return null;
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



    private string TokensToExpression( List<Types.TokenInfo> Tokens )
    {
      return TokensToExpression( Tokens, 0, Tokens.Count );
    }



    private string TokensToExpression( List<Types.TokenInfo> Tokens, int StartIndex, int Count )
    {
      StringBuilder sb = new StringBuilder();

      for ( int i = 0; i < Count; ++i )
      {
        sb.Append( Tokens[StartIndex + i].Content );
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



    private string EvaluateAsText( int lineIndex, List<Types.TokenInfo> Tokens, int StartIndex, int Count )
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
            Types.TokenInfo   startToken = Tokens[startTokenIndex];
            if ( ( StartIndex + i - startTokenIndex == 1 )
            &&   ( startToken.Type == C64Studio.Types.TokenInfo.TokenType.LITERAL_STRING ) )
            {
              sb.Append( startToken.Content.Substring ( 1, startToken.Content.Length - 2 ) );
            }
            else
            {
              int result = -1;
              if ( !EvaluateTokens( lineIndex, Tokens, startTokenIndex, StartIndex + i - startTokenIndex, out result ) )
              {
                return "";
              }
              sb.Append( result );
            }
          }
          startTokenIndex = StartIndex + i + 1;
          continue;
        }
      }
      if ( startTokenIndex < StartIndex + Count )
      {
        // something left to do
        Types.TokenInfo   token = Tokens[startTokenIndex];
        if ( ( StartIndex + Count - startTokenIndex == 1 )
        &&   ( token.Type == C64Studio.Types.TokenInfo.TokenType.LITERAL_STRING ) )
        {
          sb.Append( token.Content.Substring( 1, token.Content.Length - 2 ) );
        }
        else
        {
          int result = -1;
          if ( !EvaluateTokens( lineIndex, Tokens, startTokenIndex, StartIndex + Count - startTokenIndex, out result ) )
          {
            return "";
          }
          sb.Append( result );
        }
      }
      return sb.ToString();
    }



    public override bool DocumentAndLineFromGlobalLine( int GlobalLine, out string DocumentFile, out int DocumentLine )
    {
      return ASMFileInfo.FindTrueLineSource( GlobalLine, out DocumentFile, out DocumentLine );
    }



    public void DumpLabels()
    {
      foreach ( string label in ASMFileInfo.Labels.Keys )
      {
        Debug.Log( "Label " + label + " = " + ASMFileInfo.Labels[label].AddressOrValue.ToString( "x" ) );
      }
    }



    public string MacroByType( Types.MacroInfo.MacroType Type )
    {
      foreach ( var macro in m_AssemblerSettings.Macros )
      {
        if ( macro.Value.Type == Type )
        {
          return macro.Key;
        }
      }
      return "";
    }

  }
}
