using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio.Parser
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
      public string               Token = "";
      public Types.ColorableElement  Type  = C64Studio.Types.ColorableElement.NONE;

      public TokenSyntax( string Token, Types.ColorableElement Type )
      {
        this.Token = Token;
        this.Type = Type;
      }
    };

    public GR.Collections.MultiMap<int, ParseMessage> Messages = new GR.Collections.MultiMap<int, ParseMessage>();

    protected int m_ErrorMessages = 0;
    protected int m_WarningMessages = 0;
    protected int m_Messages = 0;

    protected Types.CompileTargetType m_CompileTarget = Types.CompileTargetType.PRG;

    protected string            m_CompileTargetFile = null;

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



    public Types.CompileTargetType CompileTarget
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



    public ParseMessage AddError( int Line, Types.ErrorCode Code, string Text )
    {
      ParseMessage errorMessage = new ParseMessage( ParseMessage.LineType.ERROR, Code, Text, -1, 0 );
      Messages.Add( Line, errorMessage );
      ++m_ErrorMessages;
      return errorMessage;
    }



    public ParseMessage AddError( int Line, Types.ErrorCode Code, string Text, int CharIndex, int Length )
    {
      ParseMessage errorMessage = new ParseMessage( ParseMessage.LineType.ERROR, Code, Text, CharIndex, Length );
      Messages.Add( Line, errorMessage );
      ++m_ErrorMessages;
      return errorMessage;
    }



    public ParseMessage AddWarning( int Line, Types.ErrorCode Code, string Text, int CharIndex, int Length )
    {
      ParseMessage warningMessage = new ParseMessage( ParseMessage.LineType.WARNING, Code, Text, CharIndex, Length );
      Messages.Add( Line, warningMessage );
      ++m_WarningMessages;
      return warningMessage;
    }



    public ParseMessage AddSevereWarning( int Line, Types.ErrorCode Code, string Text )
    {
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
      bool hasORG = ( Text.IndexOf( "ORG " ) != -1 ) || ( Text.IndexOf( "org " ) != -1 );
      bool hasInclude = ( Text.IndexOf( "INCLUDE " ) != -1 ) || ( Text.IndexOf( "INCLUDE " ) != -1 );
      bool hasTo = ( Text.IndexOf( "!to " ) != -1 ) || ( Text.IndexOf( "!TO " ) != -1 );
      bool hasEQU = ( Text.IndexOf( "EQU" ) != -1 );
      bool hasByte = ( Text.IndexOf( "!byte" ) != -1 ) || ( Text.IndexOf( "!BYTE" ) != -1 ) || ( Text.IndexOf( "!BY " ) != -1 );
      bool hasZone = ( Text.IndexOf( "!zone" ) != -1 ) || ( Text.IndexOf( "!ZONE" ) != -1 );
      bool hasProcessor = ( Text.IndexOf( "processor " ) != -1 ) || ( Text.IndexOf( "PROCESSOR " ) != -1 );

      if ( ( hasTo )
      ||   ( hasByte )
      ||   ( hasZone ) )
      {
        return C64Studio.Types.AssemblerType.C64_STUDIO;
      }
      if ( ( ( hasORG )
      ||     ( hasEQU )
      ||     ( hasInclude ) )
      && ( !hasByte ) )
      {
        return C64Studio.Types.AssemblerType.PDS;
      }
      if ( ( ( hasORG )
      && ( hasTo ) )
      || ( hasProcessor ) )
      {
        return C64Studio.Types.AssemblerType.DASM;
      }
      if ( hasByte )
      {
        return C64Studio.Types.AssemblerType.C64_STUDIO;
      }
      return C64Studio.Types.AssemblerType.AUTO;
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

      if ( Config.Assembler == C64Studio.Types.AssemblerType.AUTO )
      {
        // try to detect -> modifying passed in Config!!
        Config.Assembler = DetectAssemblerType( text );
      }
      if ( Config.Assembler == C64Studio.Types.AssemblerType.AUTO )
      {
        // safety fallback to avoid crashes
        Config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;
      }
      return Parse( text, Configuration, Config, AdditionalPredefines );
    }



    public abstract bool DocumentAndLineFromGlobalLine( int GlobalLine, out string DocumentFile, out int DocumentLine );



    public virtual GR.Collections.MultiMap<string, Types.SymbolInfo> KnownTokenInfo()
    {
      return new GR.Collections.MultiMap<string, Types.SymbolInfo>();
    }



    public virtual List<Types.AutoCompleteItemInfo> KnownTokens()
    {
      return new List<Types.AutoCompleteItemInfo>();
    }



  }
}
