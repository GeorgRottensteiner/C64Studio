using RetroDevStudio;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Text;

namespace RetroDevStudio.Parser
{
  public abstract class ParserBase
  {
    public class ParseMessage
    {
      public List<ParseMessage>     ChildMessages = null;
      public ParseMessage           ParentMessage = null;
      public LineType               Type          = LineType.NONE;
      public string                 Message       = null;
      public Types.ErrorCode        Code          = Types.ErrorCode.OK;
      public string                 AlternativeFile = null;
      public int                    AlternativeLineIndex = -1;
      public int                    CharIndex = -1;
      public int                    Length = 0;


      public enum LineType
      {
        NONE,
        WARNING,
        ERROR,
        MESSAGE,
        SEVERE_WARNING
      };

      public ParseMessage( LineType Type, Types.ErrorCode Code, string Message )
      {
        this.Type = Type;
        this.Message = Message;
        this.Code = Code;
      }

      public ParseMessage( LineType Type, Types.ErrorCode Code, string Message, int CharIndex, int Length )
      {
        this.Type = Type;
        this.Message = Message;
        this.Code = Code;
        this.CharIndex = CharIndex;
        this.Length = Length;
      }

      private ParseMessage( LineType Type, Types.ErrorCode Code, string Message, string AlternativeFile, int AlternativeLineIndex )
      {
        this.Type = Type;
        this.Message = Message;
        this.Code = Code;
        this.AlternativeFile = AlternativeFile;
        this.AlternativeLineIndex = AlternativeLineIndex;
      }

      private ParseMessage( LineType Type, Types.ErrorCode Code, string Message, string AlternativeFile, int AlternativeLineIndex, int CharIndex, int Length )
      {
        this.Type = Type;
        this.Message = Message;
        this.Code = Code;
        this.AlternativeFile = AlternativeFile;
        this.AlternativeLineIndex = AlternativeLineIndex;
        this.CharIndex = CharIndex;
        this.Length = Length;
      }

      public void AddMessage( string Message, string AlternativeFile, int AlternativeLineIndex )
      {
        AddMessage( Message, AlternativeFile, AlternativeLineIndex, -1, 0 );
      }



      public void AddMessage( string Message, string AlternativeFile, int AlternativeLineIndex, int CharIndex, int Length )
      {
        if ( ChildMessages == null )
        {
          ChildMessages = new List<ParseMessage>();
        }
        var childMessage = new ParseMessage( Type, Code, Message, AlternativeFile, AlternativeLineIndex );
        childMessage.ParentMessage = this;
        ChildMessages.Add( childMessage );
      }

    };



    public class TokenSyntax
    {
      public string                   Token = "";
      public Types.ColorableElement   Type  = Types.ColorableElement.NONE;

      public TokenSyntax( string Token, Types.ColorableElement Type )
      {
        this.Token = Token;
        this.Type = Type;
      }
    };

    public GR.Collections.MultiMap<int, ParseMessage> Messages = new GR.Collections.MultiMap<int, ParseMessage>();

    protected int               m_ErrorMessages = 0;
    protected int               m_WarningMessages = 0;
    protected int               m_Messages = 0;

    protected CompileConfig     m_CompileConfig = null;

    protected Types.CompileTargetType m_CompileTarget = Types.CompileTargetType.PRG;

    protected string            m_CompileTargetFile = null;
    protected string            m_DefaultTargetExtension = ".prg";

    protected string            m_DocBasePath = "";
    protected string            m_Filename = "";


    public AssemblyOutput AssembledOutput = null;




    public int Errors
    {
      get
      {
        return m_ErrorMessages;
      }
    }



    public int Warnings
    {
      get
      {
        return m_WarningMessages;
      }
    }



    public int OutputMessages
    {
      get
      {
        return m_Messages;
      }
    }



    public virtual string DefaultTargetExtension
    {
      get
      {
        return m_DefaultTargetExtension;
      }
    }


    public virtual Types.CompileTargetType CompileTarget
    {
      get
      {
        return m_CompileTarget;
      }
    }



    public string CompileTargetFile
    {
      get
      {
        return m_CompileTargetFile;
      }
    }



    public ParseMessage AddError( int Line, Types.ErrorCode Code, string Text, int CharIndex = -1, int Length = 0 )
    {
      ParseMessage errorMessage = new ParseMessage( ParseMessage.LineType.ERROR, Code, Text, CharIndex, Length );
      Messages.Add( Line, errorMessage );
      ++m_ErrorMessages;
      return errorMessage;
    }



    public ParseMessage AddWarning( int Line, Types.ErrorCode Code, string Text, int CharIndex, int Length )
    {
      if ( ( m_CompileConfig != null )
      &&   ( m_CompileConfig.WarningsToTreatAsError.ContainsValue( Code ) ) )
      {
        return AddError( Line, Code, Text, CharIndex, Length );
      }

      ParseMessage warningMessage = new ParseMessage( ParseMessage.LineType.WARNING, Code, Text, CharIndex, Length );
      Messages.Add( Line, warningMessage );
      ++m_WarningMessages;
      return warningMessage;
    }



    public ParseMessage AddSevereWarning( int Line, Types.ErrorCode Code, string Text )
    {
      if ( m_CompileConfig.WarningsToTreatAsError.ContainsValue( Code ) )
      {
        return AddError( Line, Code, Text );
      }

      ParseMessage warningMessage = new ParseMessage( ParseMessage.LineType.SEVERE_WARNING, Code, Text );
      Messages.Add( Line, warningMessage );
      ++m_WarningMessages;
      return warningMessage;
    }



    public void AddOutputMessage( int Line, string Text )
    {
      ParseMessage message = new ParseMessage( ParseMessage.LineType.MESSAGE, Types.ErrorCode.OK, Text );
      Messages.Add( Line, message );
      ++m_Messages;
    }



    public abstract void Clear();
    public abstract bool Assemble( CompileConfig Config );
    public abstract bool Parse( string Content, ProjectConfig Configuration, CompileConfig Config, string AdditionalPredefines );



    static public Types.AssemblerType DetectAssemblerType( string Text )
    {
      bool hasORG = false;
      bool hasMAC = false;
      bool hasInclude = false;
      bool hasDotInclude = false;
      bool hasDotByte = false;
      bool hasTo = false;
      bool hasEQU = false;
      bool hasByte = false;
      bool hasText = false;
      bool hasZone = false;
      bool hasLZone = false;
      bool hasProcessor = false;
      bool hasSemicolonComments = false;
      bool hasDotText = false;
      bool hasDotTarget = false;

      var memoryReader = new GR.IO.MemoryReader( Encoding.UTF8.GetBytes( Text ) );

      while ( memoryReader.ReadLine( out string line ) )
      {
        line = line.Trim();

        if ( line.StartsWith( ";" ) )
        {
          // must be a comment
          hasSemicolonComments = true;
        }
        string  lineUppercase = line.ToUpper();
        int     orgPos = -1;
        if ( ( orgPos = lineUppercase.IndexOf( "ORG " ) ) != -1 ) 
        {
          if ( ( line.IndexOf( '"' ) != -1 )
          &&   ( line.IndexOf( '"' ) < orgPos ) )
          {
            hasORG = true;
          }
        }
        if ( line.IndexOf( "MAC " ) != -1 )
        {
          hasMAC = true;
        }
        if ( lineUppercase.IndexOf( "INCLUDE " ) != -1 ) 
        {
          hasInclude = true;
        }
        if ( lineUppercase.IndexOf( ".INCLUDE " ) != -1 ) 
        {
          hasDotInclude = true;
        }
        if ( lineUppercase.IndexOf( ".BYTE " ) != -1 ) 
        {
          hasDotByte = true;
        }
        if ( lineUppercase.IndexOf( ".TEXT " ) != -1 )
        {
            hasDotText = true;
        }
        if ( lineUppercase.IndexOf( "!TO " ) != -1 )
        {
          hasTo = true;
        }
        if ( line.IndexOf( "EQU " ) != -1 )
        {
          hasEQU = true;
        }
        if ( ( lineUppercase.IndexOf( "!BYTE" ) != -1 ) 
        ||   ( lineUppercase.IndexOf( "!BY " ) != -1 ) )
        {
          hasByte = true;
        }
        if ( ( lineUppercase.IndexOf( "!TEXT" ) != -1 )
        ||   ( lineUppercase.IndexOf( "!TX " ) != -1 ) )
        {
          hasByte = true;
        }
        if ( lineUppercase.IndexOf( "!ZONE" ) != -1 )
        {
          hasZone = true;
        }
        if ( lineUppercase.IndexOf( "!LZONE" ) != -1 )
        {
          hasLZone = true;
        }
        if ( lineUppercase.IndexOf( "PROCESSOR " ) != -1 )
        {
          hasProcessor = true;
        }
        if ( lineUppercase.IndexOf( ".TARGET " ) != -1 )
        {
          hasDotTarget = true;
        }

        // early detection
        if ( ( hasTo )
        ||   ( hasByte )
        ||   ( hasLZone )
        ||   ( hasZone ) )
        {
          return Types.AssemblerType.C64_STUDIO;
        }

        if ( ( ( hasORG )
        &&     ( hasTo )
        &&     ( !hasByte )
        &&     ( !hasText ) )
        ||   ( hasProcessor )
        ||   ( hasMAC ) )
        {
          return Types.AssemblerType.DASM;
        }
        if ( ( ( hasORG )
        ||     ( hasEQU )
        ||     ( hasInclude ) )
        &&   ( !hasByte ) )
        {
          return Types.AssemblerType.PDS;
        }
        if ( ( ( hasDotByte )
        &&     ( hasDotText )
        &&     ( !hasORG )
        &&     ( !hasEQU ) ) 
        ||   ( hasDotTarget )
        ||   ( hasDotInclude ) )
        {
          return Types.AssemblerType.TASM;
        }
      }

      if ( ( hasByte )
      ||   ( hasSemicolonComments ) )
      {
        return Types.AssemblerType.C64_STUDIO;
      }
      return Types.AssemblerType.AUTO;
    }



    public bool ParseFile( string Filename, string SourceCode, ProjectConfig Configuration, CompileConfig Config, string AdditionalPredefines )
    {
      Clear();

      if ( string.IsNullOrEmpty( Filename ) )
      {
        return false;
      }

      m_Filename = Filename;
      m_DocBasePath = GR.Path.RemoveFileSpec( Filename );
      if ( Filename.Length == 0 )
      {
        return false;
      }

      string text = SourceCode;

      if ( string.IsNullOrEmpty( text ) )
      {
        try
        {
          text = System.IO.File.ReadAllText( Filename );
        }
        catch ( System.Exception )
        {
          AddError( -1, Types.ErrorCode.E2000_FILE_OPEN_ERROR, "Could not open file " + Filename );
          return false;
        }
      }

      if ( Config.Assembler == Types.AssemblerType.AUTO )
      {
        // try to detect -> modifying passed in Config!!
        Config.Assembler = DetectAssemblerType( text );
      }
      if ( Config.Assembler == Types.AssemblerType.AUTO )
      {
        // safety fallback to avoid crashes
        Config.Assembler = Types.AssemblerType.C64_STUDIO;
      }

      return Parse( text, Configuration, Config, AdditionalPredefines );
    }



    public abstract bool DocumentAndLineFromGlobalLine( int GlobalLine, out string DocumentFile, out int DocumentLine );



    public virtual GR.Collections.MultiMap<string, SymbolInfo> KnownTokenInfo()
    {
      return new GR.Collections.MultiMap<string, SymbolInfo>();
    }



    public virtual List<Types.AutoCompleteItemInfo> KnownTokens()
    {
      return new List<Types.AutoCompleteItemInfo>();
    }



  }
}
