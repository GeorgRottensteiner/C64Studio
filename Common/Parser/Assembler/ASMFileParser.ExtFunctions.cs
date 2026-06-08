using GR.Memory;
using RetroDevStudio.Formats;
using RetroDevStudio.Parser;
using RetroDevStudio.Types;
using RetroDevStudio.Types.ASM;
using System;
using System.Collections.Generic;
using Tiny64;

namespace RetroDevStudio.Parser
{
  public partial class ASMFileParser : ParserBase
  {
    private System.Random         _Random = null;

    private GR.Collections.Map<GR.Generic.Tupel<string,int>,ExtFunctionInfo>    m_ExtFunctions = new GR.Collections.Map<GR.Generic.Tupel<string, int>, ExtFunctionInfo>();



    public void AddExtFunction( string Name, int NumArguments, int NumResults, ExtFunction Function )
    {
      ExtFunctionInfo   fInfo = new ExtFunctionInfo();
      fInfo.Name          = Name;
      fInfo.NumArguments  = NumArguments;
      fInfo.NumResults    = NumResults;
      fInfo.Function      = Function;

      m_ExtFunctions[new GR.Generic.Tupel<string, int>( Name, NumArguments )] = fInfo;
    }



    private List<Types.TokenInfo> ExtFileSize( List<Types.TokenInfo> Arguments )
    {
      List<Types.TokenInfo>     result = new List<Types.TokenInfo>();

      long                      fileSize = 0;

      try
      {
        string    filenameToUse = Arguments[0].Content;
        if ( ( filenameToUse.Length >= 2 )
        &&   ( filenameToUse.StartsWith( "\"" ) )
        &&   ( filenameToUse.EndsWith( "\"" ) ) )
        {
          filenameToUse = filenameToUse.Substring( 1, filenameToUse.Length - 2 );
        }

        if ( !GR.Path.IsPathRooted( filenameToUse ) )
        {
          filenameToUse = GR.Path.Append( m_DocBasePath, filenameToUse );
        }
        System.IO.FileInfo f = new System.IO.FileInfo( filenameToUse );
        fileSize = f.Length;
      }
      catch ( Exception )
      {
      }
      Types.TokenInfo   fileSizeToken = new Types.TokenInfo();
      fileSizeToken.Type = Types.TokenInfo.TokenType.LITERAL_NUMBER;
      fileSizeToken.Content = fileSize.ToString();
      result.Add( fileSizeToken );
      return result;
    }



    private List<Types.TokenInfo> ExtMathMin( List<Types.TokenInfo> Arguments )
    {
      List<Types.TokenInfo>     result = new List<Types.TokenInfo>();

      if ( !EvaluateTokens( 0, Arguments, 0, 1, out SymbolInfo arg1 ) )
      {
        return result;
      }
      if ( !EvaluateTokens( 0, Arguments, 1, 1, out SymbolInfo arg2 ) )
      {
        return result;
      }

      var resultToken = new Types.TokenInfo();

      if ( ( arg1.Type == SymbolInfo.Types.CONSTANT_REAL_NUMBER )
      ||   ( arg2.Type == SymbolInfo.Types.CONSTANT_REAL_NUMBER ) )
      {
        double     argV1 = arg1.ToNumber();
        double     argV2 = arg2.ToNumber();

        double    resultValue = Math.Min( argV1, argV2 );

        resultToken.Type = TokenInfo.TokenType.LITERAL_REAL_NUMBER;
        resultToken.Content = Util.DoubleToString( resultValue );
      }
      else
      {
        long    argV1 = arg1.ToInteger();
        long    argV2 = arg2.ToInteger();
        long    resultValue = Math.Min( argV1, argV2 );

        resultToken.Type = TokenInfo.TokenType.LITERAL_NUMBER;
        resultToken.Content = resultValue.ToString();
      }
      result.Add( resultToken );
      return result;
    }



    private List<Types.TokenInfo> ExtMathMax( List<Types.TokenInfo> Arguments )
    {
      List<Types.TokenInfo>     result = new List<Types.TokenInfo>();

      if ( !EvaluateTokens( 0, Arguments, 0, 1, out SymbolInfo arg1 ) )
      {
        return result;
      }
      if ( !EvaluateTokens( 0, Arguments, 1, 1, out SymbolInfo arg2 ) )
      {
        return result;
      }

      var resultToken = new Types.TokenInfo();

      if ( ( arg1.Type == SymbolInfo.Types.CONSTANT_REAL_NUMBER )
      ||   ( arg2.Type == SymbolInfo.Types.CONSTANT_REAL_NUMBER ) )
      {
        double     argV1 = arg1.ToNumber();
        double     argV2 = arg2.ToNumber();

        double    resultValue = Math.Max( argV1, argV2 );

        resultToken.Type = TokenInfo.TokenType.LITERAL_REAL_NUMBER;
        resultToken.Content = Util.DoubleToString( resultValue );
      }
      else
      {
        long    argV1 = arg1.ToInteger();
        long    argV2 = arg2.ToInteger();
        long    resultValue = Math.Max( argV1, argV2 );

        resultToken.Type = TokenInfo.TokenType.LITERAL_NUMBER;
        resultToken.Content = resultValue.ToString();
      }
      result.Add( resultToken );
      return result;
    }



    private List<TokenInfo> ExtMathSinus( List<TokenInfo> Arguments )
    {
      var result = new List<TokenInfo>();

      if ( Arguments.Count != 1 )
      {
        return result;
      }
      if ( !EvaluateTokens( 0, Arguments, out SymbolInfo functionResult ) )
      {
        return result;
      }
      double argument = functionResult.ToNumber();

      var resultValue = new TokenInfo()
      {
        Type    = TokenInfo.TokenType.LITERAL_REAL_NUMBER,
        Content = Util.DoubleToString( Math.Sin( argument * Math.PI / 180.0f ) )
      };
      result.Add( resultValue );
      return result;
    }



    private List<TokenInfo> ExtMathCosinus( List<TokenInfo> Arguments )
    {
      var result = new List<TokenInfo>();

      if ( Arguments.Count != 1 )
      {
        //SetError( "Invalid argument count" );
        return result;
      }
      if ( !EvaluateTokens( 0, Arguments, out SymbolInfo functionResult ) )
      {
        return result;
      }
      double argument = functionResult.ToNumber();

      var resultValue = new TokenInfo()
      {
        Type = TokenInfo.TokenType.LITERAL_REAL_NUMBER,
        Content = Util.DoubleToString( Math.Cos( argument * Math.PI / 180.0f ) )
      };
      result.Add( resultValue );
      return result;
    }



    private List<TokenInfo> ExtMathTangens( List<TokenInfo> Arguments )
    {
      var result = new List<TokenInfo>();

      if ( Arguments.Count != 1 )
      {
        //SetError( "Invalid argument count" );
        return result;
      }
      if ( !EvaluateTokens( 0, Arguments, out SymbolInfo functionResult ) )
      {
        return result;
      }
      double argument = functionResult.ToNumber();

      var resultValue = new TokenInfo()
      {
        Type = TokenInfo.TokenType.LITERAL_REAL_NUMBER,
        Content = Util.DoubleToString( Math.Tan( argument * Math.PI / 180.0 ) )
      };
      result.Add( resultValue );
      return result;
    }



    private List<TokenInfo> ExtMathSquareRoot( List<TokenInfo> Arguments )
    {
      var result = new List<TokenInfo>();

      if ( Arguments.Count != 1 )
      {
        return result;
      }
      if ( !EvaluateTokens( 0, Arguments, out SymbolInfo functionResult ) )
      {
        return result;
      }
      double argument = functionResult.ToNumber();

      var resultValue = new TokenInfo()
      {
        Type    = TokenInfo.TokenType.LITERAL_REAL_NUMBER,
        Content = Util.DoubleToString( Math.Sqrt( argument ) )
      };
      result.Add( resultValue );
      return result;
    }



    private List<TokenInfo> ExtMathToRadians( List<TokenInfo> Arguments )
    {
      var result = new List<TokenInfo>();

      if ( Arguments.Count != 1 )
      {
        //SetError( "Invalid argument count" );
        return result;
      }
      if ( !EvaluateTokens( 0, Arguments, out SymbolInfo functionResult ) )
      {
        return result;
      }
      double argument = functionResult.ToNumber();
      var resultValue = new TokenInfo()
      {
        Type = TokenInfo.TokenType.LITERAL_REAL_NUMBER,
        Content = Util.DoubleToString( argument * Math.PI / 180.0f )
      };
      result.Add( resultValue );
      return result;
    }



    private List<TokenInfo> ExtMathToDegrees( List<TokenInfo> Arguments )
    {
      var result = new List<TokenInfo>();

      if ( Arguments.Count != 1 )
      {
        //SetError( "Invalid argument count" );
        return result;
      }
      if ( !EvaluateTokens( 0, Arguments, out SymbolInfo functionResult ) )
      {
        return result;
      }
      double argument = functionResult.ToNumber();

      var resultValue = new TokenInfo()
      {
        Type = TokenInfo.TokenType.LITERAL_REAL_NUMBER,
        Content = Util.DoubleToString( argument * 180.0f / Math.PI )
      };
      result.Add( resultValue );
      return result;
    }



    private List<TokenInfo> ExtMathFloor( List<TokenInfo> Arguments )
    {
      var result = new List<TokenInfo>();

      if ( Arguments.Count != 1 )
      {
        //SetError( "Invalid argument count" );
        return result;
      }
      if ( !EvaluateTokens( 0, Arguments, out SymbolInfo functionResult ) )
      {
        return result;
      }
      double argument = functionResult.ToNumber();
      var resultValue = new TokenInfo()
      {
        Type = TokenInfo.TokenType.LITERAL_REAL_NUMBER,
        Content = Util.DoubleToString( Math.Floor( argument ) )
      };
      result.Add( resultValue );
      return result;
    }



    private List<TokenInfo> ExtMathRandom( List<TokenInfo> Arguments )
    {
      var result = new List<TokenInfo>();

      if ( Arguments.Count != 1 )
      {
        //SetError( "Invalid argument count" );
        return result;
      }
      if ( !EvaluateTokens( 0, Arguments, out SymbolInfo functionResult ) )
      {
        return result;
      }
      var argument = functionResult.ToInteger();
      var resultValue = new TokenInfo()
      {
        Type    = TokenInfo.TokenType.LITERAL_NUMBER
      };

      if ( argument == 0 )
      {
        resultValue.Content = "0";
      }
      else
      {
        if ( _Random == null )
        {
          _Random = new Random();
        }
        resultValue.Content = _Random.Next( (int)argument ).ToString();
      }
      result.Add( resultValue );
      return result;
    }



    private List<TokenInfo> ExtMathRandomRange( List<TokenInfo> Arguments )
    {
      var result = new List<TokenInfo>();

      if ( Arguments.Count != 2 )
      {
        //SetError( "Invalid argument count" );
        return result;
      }
      if ( ( !EvaluateTokens( 0, Arguments, 0, 1, out SymbolInfo arg1 ) )
      ||   ( !EvaluateTokens( 0, Arguments, 1, 1, out SymbolInfo arg2 ) ) )
      {
        return result;
      }

      int rangeMin = arg1.ToInt32();
      int rangeMax = arg2.ToInt32();

      if ( rangeMax < rangeMin )
      {
        return result;
      }
      var resultValue = new TokenInfo()
      {
        Type    = TokenInfo.TokenType.LITERAL_NUMBER
      };

      if ( rangeMax == rangeMin )
      {
        resultValue.Content = rangeMin.ToString();
      }
      else
      {
        if ( _Random == null )
        {
          _Random = new Random();
        }
        resultValue.Content = _Random.Next( (int)rangeMin, (int)rangeMax ).ToString();
      }
      result.Add( resultValue );
      return result;
    }



    private List<TokenInfo> ExtMathRandomSeed( List<TokenInfo> Arguments )
    {
      var result = new List<TokenInfo>();

      if ( Arguments.Count != 1 )
      {
        return result;
      }
      if ( !EvaluateTokens( 0, Arguments, out SymbolInfo functionResult ) )
      {
        return result;
      }
      var seed = functionResult.ToInt32();
      var resultValue = new TokenInfo()
      {
        Type    = TokenInfo.TokenType.LITERAL_NUMBER
      };

      if ( seed == -1 )
      {
        _Random = new Random();
      }
      else
      {
        _Random = new Random( seed );
      }
      return result;
    }




    private List<TokenInfo> ExtMathCeiling( List<TokenInfo> Arguments )
    {
      var result = new List<TokenInfo>();

      if ( Arguments.Count != 1 )
      {
        //SetError( "Invalid argument count" );
        return result;
      }
      if ( !EvaluateTokens( 0, Arguments, out SymbolInfo functionResult ) )
      {
        return result;
      }
      double argument = functionResult.ToNumber();
      var resultValue = new TokenInfo()
      {
        Type = TokenInfo.TokenType.LITERAL_REAL_NUMBER,
        Content = Util.DoubleToString( Math.Ceiling( argument ) )
      };
      result.Add( resultValue );
      return result;
    }



  }
}
