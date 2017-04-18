using C64Studio.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio
{
  public class DocumentInfo
  {
    public BaseDocument                   BaseDoc = null;

    public string                         DocumentFilename = null;

    public Project                        Project = null;

    public ProjectElement                 Element = null;

    public ProjectElement.ElementType     Type = ProjectElement.ElementType.INVALID;

    public Types.ASM.FileInfo             ASMFileInfo = new C64Studio.Types.ASM.FileInfo();

    public Undo.UndoManager               UndoManager = new C64Studio.Undo.UndoManager();

    private List<Types.AutoCompleteItemInfo>    m_KnownKeywords = new List<Types.AutoCompleteItemInfo>();
    private GR.Collections.MultiMap<string, Types.SymbolInfo> m_KnownTokens = new GR.Collections.MultiMap<string, C64Studio.Types.SymbolInfo>();

    public GR.Collections.Set<int>        CollapsedFoldingBlocks = new GR.Collections.Set<int>();



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



    public GR.Collections.MultiMap<string, Types.SymbolInfo> KnownTokens
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



    public void SetASMFileInfo( Types.ASM.FileInfo FileInfo, List<Types.AutoCompleteItemInfo> KnownTokenList, GR.Collections.MultiMap<string, Types.SymbolInfo> KnownTokenInfo )
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

      ASMFileInfo = FileInfo;

      KnownKeywords = KnownTokenList;
      KnownTokens = KnownTokenInfo;
      if ( BaseDoc != null )
      {
        BaseDoc.OnKnownKeywordsChanged();
        BaseDoc.OnKnownTokensChanged();
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
              element.DocumentInfo.SetASMFileInfo( FileInfo, KnownTokenList, KnownTokenInfo );
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
