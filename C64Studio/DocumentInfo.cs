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

    // set if for example label mode is used (BASIC) - so all references point to the actual label mode code and not the generated code
    public Types.ASM.FileInfo             ASMFileInfoOriginal = new RetroDevStudio.Types.ASM.FileInfo();

    public SingleBuildInfo                LastBuildInfo = null;

    public Undo.UndoManager               UndoManager { get; set; } = new Undo.UndoManager();

    // temporary storage for BASIC label mode
    public Dictionary<string, SymbolInfo> LabelModeReferences = new Dictionary<string, SymbolInfo>();



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
        if ( GR.Path.IsPathRooted( DocumentFilename ) )
        {
          return DocumentFilename;
        }
        return GR.Path.Normalize( GR.Path.Append( Project.Settings.BasePath, DocumentFilename ), false );
      }
    }



    public string RelativePath
    {
      get
      {
        if ( ( Element == null )
        ||   ( Project == null ) )
        {
          return DocumentFilename;
        }
        if ( GR.Path.IsPathRooted( DocumentFilename ) )
        {
          return DocumentFilename;
        }
        return GR.Path.Normalize( GR.Path.RelativePathTo( FullPath, false, Project.Settings.BasePath, true ), false );
      }
    }



    public bool HasCustomBuild
    {
      get
      {
        // main document set and has custom build?
        if ( ( Project != null )
        &&   ( Project.Settings.CurrentConfig != null )
        &&   ( Project.Settings.MainDocument != null ) )
        {
          var mainElement = Project.GetElementByFilename( Project.Settings.MainDocument );
          if ( ( mainElement != null )
          &&   ( mainElement != Element )
          &&   ( !string.IsNullOrEmpty( Element.Settings[Project.Settings.CurrentConfig.Name].CustomBuild ) ) )
          {
            return true;
          }
        }

        // has custom build?
        if ( ( Project != null )
        &&   ( Project.Settings.CurrentConfig != null )
        &&   ( Element != null )
        &&   ( !string.IsNullOrEmpty( Element.Settings[Project.Settings.CurrentConfig.Name].CustomBuild ) ) )
        {
          return true;
        }
        return false;
      }
    }



    private static Dictionary<string,int>     _SetASMFileInfoStack = new Dictionary<string, int>();

    public void SetASMFileInfo( Types.ASM.FileInfo FileInfo )
    {
      if ( ( _SetASMFileInfoStack.TryGetValue( FullPath, out int stackCount ) )
      &&   ( stackCount > 0 ) )
      {
        return;
      }
      if ( !_SetASMFileInfoStack.ContainsKey( FullPath ) )
      {
        _SetASMFileInfoStack.Add( FullPath, 1 );
      }
      else
      {
        ++_SetASMFileInfoStack[FullPath];
      }

      SourceASMEx   asm = null;
      SourceBasicEx basic = null;

      var clonedInfo = FileInfo;

      if ( ( BaseDoc != null )
      &&   ( Type == ProjectElement.ElementType.ASM_SOURCE ) )
      {
        asm = BaseDoc as SourceASMEx;
      }
      if ( asm != null )
      {
        asm.DoNotFollowZoneSelectors = true;

        asm.SetLineInfos( clonedInfo );
      }
      if ( ( BaseDoc != null )
      &&   ( Type == ProjectElement.ElementType.BASIC_SOURCE ) )
      {
        basic = BaseDoc as SourceBasicEx;
      }
      if ( basic != null )
      {
        basic.SetLineInfos( clonedInfo );
      }

      //Debug.Log( $"      doc {FullPath} getting file info with spriteinitbytemsb {clonedInfo.Labels.ContainsKey( "spriteinitbytemsb" )}" );
      ASMFileInfo   = clonedInfo;
      KnownKeywords = clonedInfo.KnownTokens();
      KnownTokens   = clonedInfo.KnownTokenInfo();
      if ( BaseDoc != null )
      {
        BaseDoc.Core.TaskManager.AddTask( new Tasks.TaskUpdateKeywords( BaseDoc ) );
      }

      var compilableDoc = CompilableDocument;
      if ( compilableDoc != null )
      {
        compilableDoc.RemoveAllErrorMarkings();

        if ( !HasCustomBuild )
        {
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
                compilableDoc.MarkTextAsError( documentLine, message.CharIndex, message.Length, message.IsError );
              }
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
              if ( !Element.ForcedDependency.DependsOn( element.DocumentInfo.Project.Settings.Name, element.DocumentInfo.DocumentFilename ) )
              {
                element.DocumentInfo.SetASMFileInfo( clonedInfo );
              }
            }
          }
        }
      }
      if ( asm != null )
      {
        asm.DoNotFollowZoneSelectors = false;
      }

      --_SetASMFileInfoStack[FullPath];
      if ( _SetASMFileInfoStack[FullPath] == 0 )
      {
        _SetASMFileInfoStack.Remove( FullPath );
      }
    }



    public void MarkAsDirty()
    {
      if ( !HasBeenSuccessfullyBuilt )
      {
        return;
      }
      HasBeenSuccessfullyBuilt = false;

      if ( Element != null )
      {
        foreach ( var dependency in Element.ForcedDependency.DependentOnFile )
        {
          ProjectElement elementDependency = Project.GetElementByFilename(dependency.Filename);
          if ( elementDependency == null )
          {
            return;
          }
          elementDependency.DocumentInfo.MarkAsDirty();
        }
      }
      if ( Project != null )
      {
        lock ( DeducedDependency )
        {
          if ( !DeducedDependency.ContainsKey( Project.Settings.CurrentConfig.Name ) )
          {
            DeducedDependency.Add( Project.Settings.CurrentConfig.Name, new DependencyBuildState() );
          }
          foreach ( var deducedDependency in DeducedDependency[Project.Settings.CurrentConfig.Name].BuildState )
          {
            ProjectElement elementDependency = Project.GetElementByFilename(deducedDependency.Key);
            if ( elementDependency == null )
            {
              return;
            }
            elementDependency.DocumentInfo.MarkAsDirty();
          }
        }
      }
    }



  }
}
