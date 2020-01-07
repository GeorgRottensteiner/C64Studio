using C64Studio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using static C64Studio.Parser.BasicFileParser;

namespace C64Studio
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
      public Types.CompileTargetType DebugFileType = C64Studio.Types.CompileTargetType.NONE;
      public BuildChain       PreBuildChain = new BuildChain();
      public BuildChain       PostBuildChain = new BuildChain();
    };

    public DocumentInfo     DocumentInfo = new DocumentInfo();
    public BuildTypes       BuildType = BuildTypes.NONE;
    private string          m_Name = "";
    public BaseDocument     Document = null;
    public System.Windows.Forms.TreeNode Node = null;
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
    public AssemblerType    AssemblerType = C64Studio.Types.AssemblerType.AUTO;
    public BasicVersion     BasicVersion = BasicVersion.C64_BASIC_V2;
    public List<string>     ProjectHierarchy = new List<string>();



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
      foreach ( var deducedDependency in DocumentInfo.DeducedDependency.Values )
      {
        foreach ( var dependencyFile in deducedDependency.BuildState.Keys )
        {
          if ( GR.Path.IsPathEqual( dependencyFile, OtherDocumentFile ) )
          {
            return true;
          }
        }
      }

      foreach ( var dependency in ForcedDependency.DependentOnFile )
      {
        if ( string.Compare( dependency.Filename, System.IO.Path.GetFileName( OtherDocumentFile ), true ) == 0 )
        {
          return true;
        }

        // check indirect dependency
        ProjectElement  elementDependency = DocumentInfo.Project.GetElementByFilename( dependency.Filename );
        if ( elementDependency.IsDependentOn( OtherDocumentFile ) )
        {
          return true;
        }
      }
      return false;
    }




  }
}
