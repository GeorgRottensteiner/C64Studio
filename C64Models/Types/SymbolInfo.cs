namespace RetroDevStudio
{
  public class SymbolInfo
  {
    public enum Types
    {
      UNKNOWN = 0,
      LABEL,
      PREPROCESSOR_LABEL,
      PREPROCESSOR_CONSTANT_1,
      PREPROCESSOR_CONSTANT_2,
      CONSTANT_1,
      CONSTANT_2,
      ZONE,
      CONSTANT_REAL_NUMBER,
      VARIABLE_NUMBER,
      VARIABLE_INTEGER,
      VARIABLE_STRING,
      VARIABLE_ARRAY
    };

    public Types      Type = Types.UNKNOWN;
    public string     Name = "";
    public int        LineIndex = 0;            // global
    public int        LineCount = -1;           // global (-1 is for complete file)
    public string     DocumentFilename = "";
    public int        LocalLineIndex = 0;
    public int        AddressOrValue = -1;
    public string     String = "";
    public double     RealValue = 0;
    public string     Zone = "";
    public bool       FromDependency = false;
    public string     Info = "";
    public int        CharIndex = -1;
    public int        Length = 0;
    public C64Studio.Types.ASM.SourceInfo SourceInfo = null;
    public GR.Collections.Set<int>  References = new GR.Collections.Set<int>();



    public override string ToString()
    {
      return Name;
    }



    public bool IsInteger()
    {
      if ( ( Type == SymbolInfo.Types.CONSTANT_1 )
      ||   ( Type == SymbolInfo.Types.CONSTANT_2 )
      ||   ( Type == SymbolInfo.Types.PREPROCESSOR_CONSTANT_1 )
      ||   ( Type == SymbolInfo.Types.PREPROCESSOR_CONSTANT_2 ) )
      {
        return true;
      }
      return false;
    }



    public int ToInteger()
    {
      if ( Type == Types.CONSTANT_REAL_NUMBER )
      {
        return (int)RealValue;
      }
      if ( Type == Types.VARIABLE_STRING )
      {
        if ( String.Length == 0 )
        {
          return 0;
        }
        if ( ( String.StartsWith( "\"" ) )
        &&   ( String.EndsWith( "\"" ) ) )
        {
          if ( String.Length > 2 )
          {
            return (char)String[1];
          }
          return 0;
        }
        return (char)String[0];
      }
      return AddressOrValue;
    }



    public double ToNumber()
    {
      if ( Type == Types.CONSTANT_REAL_NUMBER )
      {
        return RealValue;
      }
      return (double)AddressOrValue;
    }

  }


}