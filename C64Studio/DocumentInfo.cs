using RetroDevStudio.Documents;
using System.Collections.Generic;



namespace RetroDevStudio
{
  public class DocumentInfo
  {
    public BaseDocument                   BaseDoc = null;

    public string                         DocumentFilename = null;

    public Project                        Project = null;

    public ProjectElement                 Element = null;

    public ProjectElement.ElementType     Type = ProjectElement.ElementType.INVALID;

    public Types.ASM.FileInfo             ASMFileInfo = new RetroDevStudio.Types.ASM.FileInfo();

    public SingleBuildInfo                LastBuildInfo = null;

    public Undo.UndoManager UndoManager { get; set; } = new Undo.UndoManager();



    private List<Types.AutoCompleteItemInfo>    m_KnownKeywords = new List<Types.AutoCompleteItemInfo>();
    private GR.Collections.MultiMap<string, SymbolInfo> m_KnownTokens = new GR.Collections.MultiMap<string, SymbolInfo>();

    public GR.Collections.Set<int>        CollapsedFoldingBlocks = new GR.Collections.Set<int>();
    public GR.Collections.Set<int>        Bookmarks = new GR.Collections.Set<int>();



    public List<Types.AutoCompleteItemInfo> KnownKeywords
    {
      get
      {
        return m_KnownKeywords;
      }
      set
      {
        m_KnownKeywords = value;
      }
    }



    public GR.Collections.MultiMap<string, SymbolInfo> KnownTokens
    {
      get
      {
        return m_KnownTokens;
      }
      set
      {
        m_KnownTokens = value;
      }
    }





    public GR.Collections.Map<string,DependencyBuildState> DeducedDependency = new GR.Collections.Map<string, DependencyBuildState>();



    public DocumentInfo()
    {
      HasBeenSuccessfullyBuilt = false;
    }



    public bool HasBeenSuccessfullyBuilt
    {
      get;
      set;
    }



    public bool ContainsCode
    {
      get
      {
        if ( ( Type == ProjectElement.ElementType.ASM_SOURCE )
        ||   ( Type == ProjectElement.ElementType.BASIC_SOURCE ) )
        {
          return true;
        }
        return false;
      }
    }



    public bool Compilable
    {
      get
      {
        if ( ( Type == ProjectElement.ElementType.ASM_SOURCE )
        ||   ( Type == ProjectElement.ElementType.BASIC_SOURCE ) )
        {
          return true;
        }
        return false;
      }
    }



    public CompilableDocument CompilableDocument
    {
      get
      {
        if ( !Compilable )
        {
          return null;
        }
        return (CompilableDocument)BaseDoc;
      }
    }



    public string FullPath
    {
      get
      {
        if ( ( Element == null )
        ||   ( Project == null ) )
        {
          return DocumentFilename;
        }
        if ( System.IO.Path.IsPathRooted( DocumentFilename ) )
        {
          return DocumentFilename;
        }
        return GR.Path.Normalize( GR.Path.Append( Project.Settings.BasePath, DocumentFilename ), false );
      }
    }



    public void SetASMFileInfo( Types.ASM.FileInfo FileInfo )
    {
      SourceASMEx   asm = null;

      if ( ( BaseDoc != null )
      &&   ( Type == ProjectElement.ElementType.ASM_SOURCE ) )
      {
        asm = BaseDoc as SourceASMEx;
      }
      if ( asm != null )
      {
        asm.DoNotFollowZoneSelectors = true;

        asm.SetLineInfos( FileInfo );
      }

      ASMFileInfo   = FileInfo;
      KnownKeywords = FileInfo.KnownTokens();
      KnownTokens   = FileInfo.KnownTokenInfo();
      if ( BaseDoc != null )
      {
        BaseDoc.Core.MainForm.AddTask( new Tasks.TaskUpdateKeywords( BaseDoc ) );
      }

      var compilableDoc = CompilableDocument;
      if ( compilableDoc != null )
      {
        compilableDoc.RemoveAllErrorMarkings();
        foreach ( var msg in ASMFileInfo.Messages )
        {
          int lineIndex = msg.Key;
          Parser.ParserBase.ParseMessage message = msg.Value;

          var msgType = message.Type;

          if ( compilableDoc.Core.Settings.IgnoredWarnings.ContainsValue( message.Code ) )
          {
            // ignore warning
            continue;
          }

          string documentFile = "";
          int documentLine = -1;

          ASMFileInfo.FindTrueLineSource( lineIndex, out documentFile, out documentLine );
          if ( message.AlternativeFile == null )
          {
            message.AlternativeFile = documentFile;
            message.AlternativeLineIndex = documentLine;
          }

          if ( message.CharIndex != -1 )
          {
            CompilableDocument    compilableDocToUse = null;
            if ( Project == null )
            {
              var sourceDocInfo = compilableDoc.Core.MainForm.DetermineDocumentByFileName( documentFile );
              if ( sourceDocInfo != null )
              {
                compilableDocToUse = sourceDocInfo.CompilableDocument;
              }
            }
            else
            {
              var  sourceElement = Project.GetElementByFilename( documentFile );
              if ( sourceElement != null )
              {
                if ( sourceElement.Document != null )
                {
                  compilableDocToUse = sourceElement.DocumentInfo.CompilableDocument;
                }
              }
            }
            if ( compilableDocToUse == compilableDoc )
            {
              compilableDoc.MarkTextAsError( documentLine, message.CharIndex, message.Length );
            }
          }
        }
      }

      if ( Element != null )
      {
        // and to all it's deduced dependencies!
        foreach ( var dependencyBuildState in DeducedDependency.Values )
        {
          foreach ( var dependency in dependencyBuildState.BuildState.Keys )
          {
            ProjectElement    element = Project.GetElementByFilename( dependency );
            if ( ( element != null )
            &&   ( element.DocumentInfo.Type == ProjectElement.ElementType.ASM_SOURCE ) )
            {
              element.DocumentInfo.SetASMFileInfo( FileInfo );
            }
          }
        }
      }
      if ( asm != null )
      {
        asm.DoNotFollowZoneSelectors = false;
      }
    }

  }
}
