using RetroDevStudio.Documents;
using RetroDevStudio.Types;
using System.Collections.Generic;



namespace RetroDevStudio
{
  public class ProjectElement
  {
    public enum ElementType
    {
      INVALID = 0,
      ASM_SOURCE,
      SPRITE_SET,
      CHARACTER_SET,
      BASIC_SOURCE,
      GRAPHIC_SCREEN,
      CHARACTER_SCREEN,
      MAP_EDITOR,
      FOLDER,
      SOLUTION,
      PROJECT,
      DISASSEMBLER,
      BINARY_FILE,
      MEDIA_MANAGER,
      VALUE_TABLE
    };

    public enum BuildTypes
    {
      NONE,
      ASSEMBLER,
      COMMAND_LINE
    };

    public class PerConfigSettings
    {
      public enum BuildEvent
      {
        PRE,
        PRE_BUILD_CHAIN,
        CUSTOM,
        POST_BUILD_CHAIN,
        POST
      };
      public ElementType      Type = ElementType.INVALID;
      public BuildTypes       BuildType = BuildTypes.NONE;
      public string           PostBuild = "";
      public string           PreBuild = "";
      public string           CustomBuild = "";
      public string           DebugFile = null;
      public Types.CompileTargetType DebugFileType = RetroDevStudio.Types.CompileTargetType.NONE;
      public BuildChain       PreBuildChain = new BuildChain();
      public BuildChain       PostBuildChain = new BuildChain();
    };

    public DocumentInfo     DocumentInfo = new DocumentInfo();
    public BuildTypes       BuildType = BuildTypes.NONE;
    private string          m_Name = "";
    public BaseDocument     Document = null;
    public DecentForms.TreeView.TreeNode Node = null;
    private string          m_Filename = null;
    public string           TargetFilename = null;
    public string           CompileTargetFile = null;
    public string           StartAddress = "2049";
    public Types.CompileTargetType TargetType = Types.CompileTargetType.NONE;
    public Types.CompileTargetType CompileTarget = Types.CompileTargetType.NONE;
    public FileDependency   ForcedDependency = new FileDependency();
    public FileDependency   ExternalDependencies = new FileDependency();
    // per setting
    public GR.Collections.Map<string,PerConfigSettings>     Settings = new GR.Collections.Map<string, PerConfigSettings>();
    public bool             IsShown = false;
    public AssemblerType    AssemblerType = RetroDevStudio.Types.AssemblerType.AUTO;
    public List<string>     ProjectHierarchy = new List<string>();

    public string           BASICDialect = "BASIC V2";
    public bool             BASICWriteTempFileWithoutMetaData = false;



    public ProjectElement()
    {
      DocumentInfo.Element = this;
    }



    public string           Name
    {
      get
      {
        return m_Name;
      }
      set
      {
        m_Name = value;
        if ( Document != null )
        {
          Document.Text = value;
        }
      }
    }
    
    
    
    public string Filename
    {
      get
      {
        return m_Filename;
      }
      set
      {
        m_Filename = value;
        if ( Node != null )
        {
          Node.Text = System.IO.Path.GetFileName( value );
        }
      }
    }



    public bool IsDependentOn( string OtherDocumentFile )
    {
      // UGLY HACK to avoid racing condition when accessing the collection while it might be modified by pre parsing tasks
      var localCopy = new GR.Collections.Map<string,DependencyBuildState>( DocumentInfo.DeducedDependency );
      {
        foreach ( var deducedDependency in localCopy.Values )
        {
          foreach ( var dependencyFile in deducedDependency.BuildState.Keys )
          {
            if ( GR.Path.IsPathEqual( dependencyFile, OtherDocumentFile ) )
            {
              return true;
            }
          }
        }
      }

      var localCopy2 = new List<FileDependency.DependencyInfo>( ForcedDependency.DependentOnFile );
      {
        foreach ( var dependency in localCopy2 )
        {
          if ( string.Compare( dependency.Filename, System.IO.Path.GetFileName( OtherDocumentFile ), true ) == 0 )
          {
            return true;
          }

          // check indirect dependency
          ProjectElement  elementDependency = DocumentInfo.Project.GetElementByFilename( dependency.Filename );
          if ( ( elementDependency != null )
          &&   ( elementDependency.IsDependentOn( OtherDocumentFile ) ) )
          {
            return true;
          }
        }
      }
      return false;
    }




  }
}
