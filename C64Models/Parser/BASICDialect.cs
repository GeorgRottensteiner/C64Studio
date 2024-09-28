using RetroDevStudio.Parser;
using System;
using System.Collections.Generic;
using System.Text;



namespace C64Models.BASIC
{
  public class Opcode
  {
    public string         Command = "";
    public int            InsertionValue = -1;
    public string         ShortCut = null;
    public bool           IsComment = false;                        // e.g. REM
    public bool           IsPreLabelToken = false;                  // at the start of a statement (no EXEC/PROC)
    public int            ArgumentIndexOfExpectedLineNumber = -1;   // 0 for THEN, GOTO, GOSUB, RUN, 1 for COLLISION (V7), etc.
    public bool           AllowsSeveralLineNumbers = false;         // true for (ON xxx) GOTO, GOSUB ..,..,..
    public bool           LineListRange = false;                    // LIST <from> or LIST <from>-<to> or LIST -<to>
    public bool           GoTokenToMayFollow = false;               // special case Commodore GO TO, can only do 1 line number!



    public Opcode( string Command, int InsertionValue )
    {
      this.Command        = Command;
      this.InsertionValue = InsertionValue;
    }

    public Opcode( string Command, int InsertionValue, string ShortCut )
    {
      this.Command        = Command;
      this.InsertionValue = InsertionValue;
      this.ShortCut       = ShortCut;
    }
  };



  public class Dialect
  {
    public string                           Name = "";
    public Dictionary<string, Opcode>       Opcodes = new Dictionary<string, Opcode>();
    public SortedDictionary<ushort, Opcode> OpcodesFromByte = new SortedDictionary<ushort, Opcode>();
    public Dictionary<string, Opcode>       ExOpcodes = new Dictionary<string, Opcode>();
    public string                           DefaultStartAddress = "2049";
    public int                              SafeLineLength = 80;
    public string                           HexPrefix = "";
    public string                           BinPrefix = "";
    public bool                             HasTextLabels = false;  // e.g. PROC names for TSB 
    public bool                             ExtendedTokensRecognizedInsideComment = false;
    public int                              MaxLineNumber = 63999;

    public static Dialect                   BASICV2;



    static Dialect()
    {
      // V2 is hard coded, so we always have at least one dialect file
      BASICV2 = new Dialect();
      BASICV2.Name = "BASIC V2";
      BASICV2.AddOpcode( "END", 0x80, "eN" );
      BASICV2.AddOpcode( "FOR", 0x81, "fO" );
      BASICV2.AddOpcode( "NEXT", 0x82, "nE" );
      BASICV2.AddOpcode( "DATA", 0x83, "dA" );
      BASICV2.AddOpcode( "INPUT#", 0x84, "iN" );
      BASICV2.AddOpcode( "INPUT", 0x85 );
      BASICV2.AddOpcode( "DIM", 0x86, "dI" );
      BASICV2.AddOpcode( "READ", 0x87, "rE" );
      BASICV2.AddOpcode( "LET", 0x88, "lE" );
      var goTo = BASICV2.AddOpcode( "GOTO", 0x89, "gO" );
      goTo.AllowsSeveralLineNumbers = true;
      BASICV2.AddOpcode( "RUN", 0x8A, "rU" ).ArgumentIndexOfExpectedLineNumber = 0;
      BASICV2.AddOpcode( "IF", 0x8B );
      BASICV2.AddOpcode( "RESTORE", 0x8C, "reS" );
      var gosub = BASICV2.AddOpcode( "GOSUB", 0x8D, "goS" );
      gosub.AllowsSeveralLineNumbers = true;
      BASICV2.AddOpcode( "RETURN", 0x8E, "reT" );
      BASICV2.AddOpcode( "REM", 0x8F ).IsComment = true;
      BASICV2.AddOpcode( "STOP", 0x90, "sT" );
      BASICV2.AddOpcode( "ON", 0x91 );
      BASICV2.AddOpcode( "WAIT", 0x92, "wA" );
      BASICV2.AddOpcode( "LOAD", 0x93, "lO" );
      BASICV2.AddOpcode( "SAVE", 0x94, "sA" );
      BASICV2.AddOpcode( "VERIFY", 0x95, "vE" );
      BASICV2.AddOpcode( "DEF", 0x96, "dE" );
      BASICV2.AddOpcode( "POKE", 0x97, "pO" );
      BASICV2.AddOpcode( "PRINT#", 0x98, "pR" );
      BASICV2.AddOpcode( "PRINT", 0x99 );
      BASICV2.AddOpcode( "?", 0x99 );
      BASICV2.AddOpcode( "CONT", 0x9A, "cO" );
      BASICV2.AddOpcode( "LIST", 0x9B, "lI" ).LineListRange = true;
      BASICV2.AddOpcode( "CLR", 0x9C, "cL" );
      BASICV2.AddOpcode( "CMD", 0x9D, "cM" );
      BASICV2.AddOpcode( "SYS", 0x9E, "sY" );
      BASICV2.AddOpcode( "OPEN", 0x9F, "oP" );
      BASICV2.AddOpcode( "CLOSE", 0xA0, "clO" );
      BASICV2.AddOpcode( "GET", 0xA1, "gE" );
      BASICV2.AddOpcode( "NEW", 0xA2 );
      BASICV2.AddOpcode( "TAB(", 0xA3, "tA" );
      BASICV2.AddOpcode( "TO", 0xA4 );
      BASICV2.AddOpcode( "FN", 0xA5 );
      BASICV2.AddOpcode( "SPC(", 0xA6, "sP" );
      BASICV2.AddOpcode( "THEN", 0xA7, "tH" ).ArgumentIndexOfExpectedLineNumber = 0;
      BASICV2.AddOpcode( "NOT", 0xA8, "nO" );
      BASICV2.AddOpcode( "STEP", 0xA9, "stE" );
      BASICV2.AddOpcode( "+", 0xAA );
      BASICV2.AddOpcode( "-", 0xAB );
      BASICV2.AddOpcode( "*", 0xAC );
      BASICV2.AddOpcode( "/", 0xAD );
      BASICV2.AddOpcode( "^", 0xAE );
      BASICV2.AddOpcode( "AND", 0xAF, "aN" );
      BASICV2.AddOpcode( "OR", 0xB0 );
      BASICV2.AddOpcode( ">", 0xB1 );
      BASICV2.AddOpcode( "=", 0xB2 );
      BASICV2.AddOpcode( "<", 0xB3 );
      BASICV2.AddOpcode( "SGN", 0xB4, "sG" );
      BASICV2.AddOpcode( "INT", 0xB5 );
      BASICV2.AddOpcode( "ABS", 0xB6, "aB" );
      BASICV2.AddOpcode( "USR", 0xB7, "uS" );
      BASICV2.AddOpcode( "FRE", 0xB8, "fE" );
      BASICV2.AddOpcode( "POS", 0xB9 );
      BASICV2.AddOpcode( "SQR", 0xBA, "sQ" );
      BASICV2.AddOpcode( "RND", 0xBB, "rN" );
      BASICV2.AddOpcode( "LOG", 0xBC );
      BASICV2.AddOpcode( "EXP", 0xBD, "eX" );
      BASICV2.AddOpcode( "COS", 0xBE );
      BASICV2.AddOpcode( "SIN", 0xBF, "sI" );
      BASICV2.AddOpcode( "TAN", 0xC0 );
      BASICV2.AddOpcode( "ATN", 0xC1, "aT" );
      BASICV2.AddOpcode( "PEEK", 0xC2, "pE" );
      BASICV2.AddOpcode( "LEN", 0xC3 );
      BASICV2.AddOpcode( "STR$", 0xC4, "stR" );
      BASICV2.AddOpcode( "VAL", 0xC5, "vA" );
      BASICV2.AddOpcode( "ASC", 0xC6, "aS" );
      BASICV2.AddOpcode( "CHR$", 0xC7, "cH" );
      BASICV2.AddOpcode( "LEFT$", 0xC8, "leF" );
      BASICV2.AddOpcode( "RIGHT$", 0xC9, "rI" );
      BASICV2.AddOpcode( "MID$", 0xCA, "mI" );
      var goToken = BASICV2.AddOpcode( "GO", 0xCB );
      goToken.GoTokenToMayFollow = true;

      // C64Studio extension
      BASICV2.AddExOpcode( "LABEL", 0xF0 );
    }



    public void Clear()
    {
      ExOpcodes.Clear();
      Opcodes.Clear();
      OpcodesFromByte.Clear();
    }



    public Opcode AddOpcode( string Opcode, int ByteValue )
    {
      var opcode = new Opcode( Opcode, ByteValue );
      Opcodes[Opcode] = opcode;

      if ( !OpcodesFromByte.ContainsKey( (ushort)ByteValue ) )
      {
        OpcodesFromByte[(ushort)ByteValue] = opcode;
      }
      else
      {
      }

      return opcode;
    }



    public Opcode AddOpcode( string Opcode, int ByteValue, string ShortCut )
    {
      var opcode = new Opcode( Opcode, ByteValue, ShortCut );
      Opcodes[Opcode] = opcode;
      if ( !OpcodesFromByte.ContainsKey( (ushort)ByteValue ) )
      {
        OpcodesFromByte[(ushort)ByteValue] = opcode;
      }
      else
      {
      }

      return opcode;
    }



    public void AddExOpcode( string Opcode, int ByteValue )
    {
      ExOpcodes[Opcode] = new Opcode( Opcode, ByteValue );
    }



    public static Dialect ParseFromFile( string File, out string ErrorMessage )
    {
      ErrorMessage = "";
      var dialect = new Dialect();
      using ( var reader = new GR.IO.BinaryReader( File ) )
      {
        string    line;
        bool      firstLine = true;
        int       lineIndex = 0;
        bool      exOpcodes = false;

        while ( reader.ReadLine( out line ) )
        {
          ++lineIndex;
          line = line.Trim();
          if ( ( string.IsNullOrEmpty( line ) )
          || ( line.StartsWith( "#" ) ) )
          {
            continue;
          }
          if ( line.StartsWith( "StartAddress=" ) )
          {
            dialect.DefaultStartAddress = line.Substring( 13 );
            continue;
          }
          else if ( line.StartsWith( "SafeLineLength=" ) )
          {
            dialect.SafeLineLength = GR.Convert.ToI32( line.Substring( 15 ) );
            continue;
          }
          else if ( line.StartsWith( "MaxLineNumber=" ) )
          {
            dialect.MaxLineNumber = GR.Convert.ToI32( line.Substring( 14 ) );
            continue;
          }
          else if ( line.StartsWith( "HexPrefix=" ) )
          {
            dialect.HexPrefix = line.Substring( 10 );
            continue;
          }
          else if ( line.StartsWith( "BinPrefix=" ) )
          {
            dialect.BinPrefix = line.Substring( 10 );
            continue;
          }
          else if ( line == "HasTextLabels" )
          {
            dialect.HasTextLabels = true;
            continue;
          }
          else if ( line == "ExtendedTokensRecognizedInsideComment" )
          {
            dialect.ExtendedTokensRecognizedInsideComment = true;
            continue;
          }

          // skip header
          if ( firstLine )
          {
            firstLine = false;
            continue;
          }


          if ( line == "ExOpcodes" )
          {
            exOpcodes = true;
            continue;
          }

          string[] parts = line.Split( ';' );
          if ( ( parts.Length != 3 )
          && ( parts.Length != 4 ) )
          {
            ErrorMessage = $"Invalid BASIC format file '{File}', expected three or four columns in line {lineIndex}";
            return null;
          }
          if ( exOpcodes )
          {
            dialect.AddExOpcode( parts[0], GR.Convert.ToI32( parts[1], 16 ) );
          }
          else
          {
            var opCode = dialect.AddOpcode( parts[0], GR.Convert.ToI32( parts[1], 16 ), parts[2] );

            if ( parts.Length == 4 )
            {
              string[]    extraInfo = parts[3].Split( ',' );

              for ( int i = 0; i < extraInfo.Length; ++i )
              {
                if ( string.Compare( extraInfo[i], "COMMENT", true ) == 0 )
                {
                  opCode.IsComment = true;
                }
                else if ( string.Compare( extraInfo[i], "GOTOKEN", true ) == 0 )
                {
                  opCode.GoTokenToMayFollow = true;
                }
                else if ( string.Compare( extraInfo[i], "LINELISTRANGE", true ) == 0 )
                {
                  opCode.LineListRange = true;
                }
                else if ( string.Compare( extraInfo[i], "PRELABELTOKEN", true ) == 0 )
                {
                  opCode.IsPreLabelToken = true;
                }
                else if ( extraInfo[i].ToUpper().StartsWith( "LINENUMBERAT:" ) )
                {
                  int   argNo = GR.Convert.ToI32( extraInfo[i].Substring( "LINENUMBERAT:".Length ) );
                  opCode.ArgumentIndexOfExpectedLineNumber = argNo;
                }
                else if ( string.Compare( extraInfo[i], "LISTOFLINENUMBERS", true ) == 0 )
                {
                  opCode.AllowsSeveralLineNumbers = true;
                }
                else
                {
                  ErrorMessage = $"Invalid BASIC format file '{File}', unknown extra info {extraInfo[i]} in line {lineIndex}";
                  return null;
                }
              }
            }
          }
        }
      }
      dialect.Name = GR.Path.GetFileNameWithoutExtension( File );

      return dialect;
    }



    public bool IsComment( BasicFileParser.Token BasicToken )
    {
      if ( Opcodes.ContainsKey( BasicToken.Content ) )
      {
        return Opcodes[BasicToken.Content].IsComment;
      }
      return false;
    }



    public bool TokenDoesNotParseOtherTokens( BasicFileParser.Token BasicToken )
    {
      if ( BasicToken.Content == "DATA" )
      {
        return true;
      }
      return false;
    }



  }
}
