using C64Studio.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;
using static C64Studio.Parser.BasicFileParser;

namespace C64Studio
{
  public class Project
  {
    public ProjectSettings    Settings = new ProjectSettings();
    public StudioCore         Core = null;
    private bool              m_Modified = false;
    public System.Windows.Forms.TreeNode           Node = null;


    public System.Collections.Generic.LinkedList<ProjectElement>   Elements = new LinkedList<ProjectElement>();


    public bool Modified
    {
      get
      {
        return m_Modified;
      }
    }



    public void SetModified()
    {
      m_Modified = true;
    }



    public Project()
    {
      ProjectConfig   configDefault = new ProjectConfig();

      configDefault.Name = "Default";

      Settings.Configuration( configDefault.Name, configDefault );
      Settings.CurrentConfig = configDefault;
    }



    public bool IsFilenameInUse( string Filename )
    {
      foreach ( var element in Elements )
      {
        if ( GR.Path.IsPathEqual( element.DocumentInfo.FullPath, Filename ) )
        {
          return true;
        }
      }
      return false;
    }



    GR.Memory.ByteBuffer ElementToBuffer( ProjectElement Element )
    {
      GR.Memory.ByteBuffer buffer = new GR.Memory.ByteBuffer();

      GR.IO.FileChunk chunkElement = new GR.IO.FileChunk( Types.FileChunk.PROJECT_ELEMENT );

      chunkElement.AppendU32( 1 );
      chunkElement.AppendU32( (uint)Element.DocumentInfo.Type );
      chunkElement.AppendString( Element.Name );
      chunkElement.AppendString( Element.Filename );

      GR.IO.FileChunk chunkElementData = new GR.IO.FileChunk( Types.FileChunk.PROJECT_ELEMENT_DATA );
      if ( Element.Document != null )
      {
        Element.Document.SaveToChunk( chunkElementData );
      }
      chunkElement.Append( chunkElementData.ToBuffer() );
      chunkElement.AppendString( Element.TargetFilename );
      chunkElement.AppendU32( (uint)Element.TargetType );
      chunkElement.AppendI32( Element.ForcedDependency.DependentOnFile.Count );
      foreach ( var dependency in Element.ForcedDependency.DependentOnFile )
      {
        chunkElement.AppendString( dependency.Filename );
      }

      chunkElement.AppendString( Element.StartAddress );
      // 2 free strings
      chunkElement.AppendString( "" );
      chunkElement.AppendString( "" );

      chunkElement.AppendI32( Element.Settings.Count );
      foreach ( KeyValuePair<string, ProjectElement.PerConfigSettings> configSetting in Element.Settings )
      {
        GR.IO.FileChunk chunkElementPerConfigSetting = new GR.IO.FileChunk( Types.FileChunk.PROJECT_ELEMENT_PER_CONFIG_SETTING );

        chunkElementPerConfigSetting.AppendString( configSetting.Key );
        chunkElementPerConfigSetting.AppendString( configSetting.Value.PreBuild );
        chunkElementPerConfigSetting.AppendString( configSetting.Value.CustomBuild );
        chunkElementPerConfigSetting.AppendString( configSetting.Value.PostBuild );
        chunkElementPerConfigSetting.AppendString( configSetting.Value.DebugFile );
        chunkElementPerConfigSetting.AppendI32( (int)configSetting.Value.DebugFileType );

        chunkElementPerConfigSetting.AppendI32( configSetting.Value.PreBuildChain.Active ? 1 : 0 );
        chunkElementPerConfigSetting.AppendI32( configSetting.Value.PreBuildChain.Entries.Count );
        foreach ( var buildChainEntry in configSetting.Value.PreBuildChain.Entries )
        {
          chunkElementPerConfigSetting.AppendString( buildChainEntry.ProjectName );
          chunkElementPerConfigSetting.AppendString( buildChainEntry.Config );
          chunkElementPerConfigSetting.AppendString( buildChainEntry.DocumentFilename );
          chunkElementPerConfigSetting.AppendString( buildChainEntry.PreDefines );
        }

        chunkElementPerConfigSetting.AppendI32( configSetting.Value.PostBuildChain.Active ? 1 : 0 );
        chunkElementPerConfigSetting.AppendI32( configSetting.Value.PostBuildChain.Entries.Count );
        foreach ( var buildChainEntry in configSetting.Value.PostBuildChain.Entries )
        {
          chunkElementPerConfigSetting.AppendString( buildChainEntry.ProjectName );
          chunkElementPerConfigSetting.AppendString( buildChainEntry.Config );
          chunkElementPerConfigSetting.AppendString( buildChainEntry.DocumentFilename );
          chunkElementPerConfigSetting.AppendString( buildChainEntry.PreDefines );
        }

        chunkElement.Append( chunkElementPerConfigSetting.ToBuffer() );
      }
      chunkElement.AppendI32( Element.IsShown ? 1 : 0 );
      chunkElement.AppendU32( (uint)Element.AssemblerType );

      chunkElement.AppendI32( Element.ProjectHierarchy.Count );
      foreach ( string hierarchyPart in Element.ProjectHierarchy )
      {
        chunkElement.AppendString( hierarchyPart );
      }
      // dependency - include symbols
      chunkElement.AppendI32( Element.ForcedDependency.DependentOnFile.Count );
      foreach ( var dependency in Element.ForcedDependency.DependentOnFile )
      {
        chunkElement.AppendI32( dependency.IncludeSymbols ? 1 : 0 );
      }
      // collapsed folding blocks
      chunkElement.AppendI32( Element.DocumentInfo.CollapsedFoldingBlocks.Count );
      foreach ( int foldStartLine in Element.DocumentInfo.CollapsedFoldingBlocks )
      {
        chunkElement.AppendI32( foldStartLine );
        //Debug.Log( "Save folded block for line " + foldStartLine );
      }

      // external dependencies
      chunkElement.AppendI32( Element.ExternalDependencies.DependentOnFile.Count );
      foreach ( var dependency in Element.ExternalDependencies.DependentOnFile )
      {
        chunkElement.AppendString( dependency.Filename );
      }

      // BASIC (that is sooo ugly)
      chunkElement.AppendU32( (uint)Element.BasicVersion );

      buffer.Append( chunkElement.ToBuffer() );

      if ( Element.Document != null )
      {
        GR.Memory.ByteBuffer displayDetails = Element.Document.DisplayDetails();
        if ( displayDetails != null )
        {
          GR.IO.FileChunk chunkElementDisplayData = new GR.IO.FileChunk( Types.FileChunk.PROJECT_ELEMENT_DISPLAY_DATA );
          chunkElementDisplayData.AppendString( Element.Filename );
          chunkElementDisplayData.AppendU32( displayDetails.Length );
          chunkElementDisplayData.Append( displayDetails );

          buffer.Append( chunkElementDisplayData.ToBuffer() );
        }
      }
      // child elements
      foreach ( System.Windows.Forms.TreeNode node in Element.Node.Nodes )
      {
        ProjectElement subElement = (ProjectElement)node.Tag;

        buffer.Append( ElementToBuffer( subElement ) );
      }

      return buffer;
    }



    public GR.Memory.ByteBuffer Save()
    {
      GR.Memory.ByteBuffer bufferProject = new GR.Memory.ByteBuffer();

      GR.IO.FileChunk chunkProject = new GR.IO.FileChunk( Types.FileChunk.PROJECT );

      // version 2 -> has adjusted debug start address (to get rid of 2049)
      chunkProject.AppendU32( 2 );
      chunkProject.AppendString( Settings.Name );
      chunkProject.AppendString( Settings.Filename );
      chunkProject.AppendU16( Settings.DebugPort );
      chunkProject.AppendU16( 0 );// obsolete Settings.DebugStartAddress
      chunkProject.AppendString( Settings.BuildTool );
      chunkProject.AppendString( Settings.RunTool );
      chunkProject.AppendString( Settings.MainDocument );
      chunkProject.AppendString( Settings.CurrentConfig.Name );
      if ( Core.MainForm.ActiveElement != null )
      {
        chunkProject.AppendString( Core.MainForm.ActiveElement.Filename );
      }
      else
      {
        chunkProject.AppendString( "" );
      }

      bufferProject.Append( chunkProject.ToBuffer() );

      foreach ( System.Windows.Forms.TreeNode node in Node.Nodes )
      {
        ProjectElement element = (ProjectElement)node.Tag;

        bufferProject.Append( ElementToBuffer( element ) );
      }

      foreach ( ProjectConfig config in Settings.GetConfigurations() )
      {
        bufferProject.Append( config.Save() );
      }

      foreach ( var watch in Core.MainForm.m_DebugWatch.m_WatchEntries )
      {
        bufferProject.Append( watch.Save() );
      }

      return bufferProject;
    }



    public bool Save( string Filename )
    {
      Settings.Filename = Filename;
      GR.Memory.ByteBuffer projectData = Save();
      try
      {
        System.IO.File.WriteAllBytes( Filename, projectData.Data() );
      }
      catch ( System.IO.IOException )
      {
        return false;
      }
      m_Modified = false;
      return true;
    }



    private System.Windows.Forms.TreeNode NodeFromHierarchy( List<string> Hierarchy )
    {
      System.Windows.Forms.TreeNode   curNode = Node;

      foreach ( string part in Hierarchy )
      {
        bool    partFound = false;
        foreach ( System.Windows.Forms.TreeNode childNode in curNode.Nodes )
        {
          if ( childNode.Text == part )
          {
            curNode = childNode;
            partFound = true;
            break;
          }
        }
        if ( !partFound )
        {
          return curNode;
        }
      }
      return curNode;
    }



    public bool Load( string Filename )
    {
      byte[] projectData = null;
      try
      {
        projectData = System.IO.File.ReadAllBytes( Filename );
      }
      catch ( System.IO.IOException )
      {
        return false;
      }

      Settings.Filename = Filename;
      Settings.BasePath = System.IO.Path.GetDirectoryName( Settings.Filename );

      if ( !Load( projectData ) )
      {
        return false;
      }
      Settings.Filename = Filename;
      Settings.BasePath = System.IO.Path.GetDirectoryName( Settings.Filename );
      return true;
    }



    public bool Load( byte[] ProjectData )
    {
      string currentConfig = "Default";
      string activeElement = "";

      Node = new System.Windows.Forms.TreeNode();
      Node.Tag = this;

      GR.IO.MemoryReader memIn = new GR.IO.MemoryReader( ProjectData );

      GR.IO.FileChunk           chunk = new GR.IO.FileChunk();
      ushort                    origDebugStartAddress = 0;

      while ( chunk.ReadFromStream( memIn ) )
      {
        GR.IO.MemoryReader      memChunk = chunk.MemoryReader();
        switch ( chunk.Type )
        {
          case Types.FileChunk.PROJECT:
            // Project Info

            // Version
            uint projectVersion          = memChunk.ReadUInt32();
            Settings.Name         = memChunk.ReadString();
            Settings.Filename     = memChunk.ReadString();
            Settings.DebugPort    = memChunk.ReadUInt16();
            origDebugStartAddress = memChunk.ReadUInt16();
            Settings.BuildTool    = memChunk.ReadString();
            Settings.RunTool      = memChunk.ReadString();
            Settings.MainDocument = memChunk.ReadString();
            currentConfig         = memChunk.ReadString();
            activeElement         = memChunk.ReadString();

            if ( projectVersion == 1 )
            {
              if ( origDebugStartAddress == 2049 )
              {
                origDebugStartAddress = 0;
              }
            }

            Node.Text             = Settings.Name;
            break;
          case Types.FileChunk.PROJECT_ELEMENT:
            // Element Info
            {
              // Version
              int elementVersion = (int)memChunk.ReadUInt32();

              ProjectElement.ElementType type = (ProjectElement.ElementType)memChunk.ReadUInt32();

              //System.Windows.Forms.TreeNode nodeParent = NodeFromHierarchy( 

              ProjectElement element = CreateElement( type, Node );
              element.Name = memChunk.ReadString();
              //element.Filename = System.IO.Path.GetFileName( memChunk.ReadString() );
              element.Filename = memChunk.ReadString();
              if ( element.DocumentInfo.Type == ProjectElement.ElementType.FOLDER )
              {
                element.Node.Text = element.Name;
              }
              else
              {
                element.Node.Text = System.IO.Path.GetFileName( element.Filename );
              }

              GR.IO.FileChunk           subChunk = new GR.IO.FileChunk();

              if ( !subChunk.ReadFromStream( memChunk ) )
              {
                return false;
              }
              if ( subChunk.Type != Types.FileChunk.PROJECT_ELEMENT_DATA )
              {
                return false;
              }
              // Element Data
              element.DocumentInfo.DocumentFilename = element.Filename;
              if ( element.Document != null )
              {
                if ( !element.Document.ReadFromReader( subChunk.MemoryReader() ) )
                {
                  Elements.Remove( element );
                  element.Document.Dispose();
                  element = null;
                }
                else
                {
                  element.Document.SetDocumentFilename( element.Filename );
                }
              }
              element.TargetFilename = memChunk.ReadString();
              element.TargetType = (Types.CompileTargetType)memChunk.ReadUInt32();
              int dependencyCount = memChunk.ReadInt32();
              for ( int i = 0; i < dependencyCount; ++i )
              {
                string dependency = memChunk.ReadString();
                element.ForcedDependency.DependentOnFile.Add( new FileDependency.DependencyInfo( dependency, true, false ) );
              }
              element.StartAddress = memChunk.ReadString();
              // 2 free strings
              memChunk.ReadString();
              memChunk.ReadString();

              int     perConfigSettingCount = memChunk.ReadInt32();
              for ( int i = 0; i < perConfigSettingCount; ++i )
              {
                GR.IO.FileChunk chunkElementPerConfigSetting = new GR.IO.FileChunk();
                chunkElementPerConfigSetting.ReadFromStream( memChunk );
                if ( chunkElementPerConfigSetting.Type == Types.FileChunk.PROJECT_ELEMENT_PER_CONFIG_SETTING )
                {
                  ProjectElement.PerConfigSettings    perConfigSetting = new ProjectElement.PerConfigSettings();
                  GR.IO.MemoryReader      memSubChunk = chunkElementPerConfigSetting.MemoryReader();
                  string    config = memSubChunk.ReadString();

                  perConfigSetting.PreBuild     = memSubChunk.ReadString();
                  perConfigSetting.CustomBuild  = memSubChunk.ReadString();
                  perConfigSetting.PostBuild    = memSubChunk.ReadString();
                  perConfigSetting.DebugFile    = memSubChunk.ReadString();
                  perConfigSetting.DebugFileType = (C64Studio.Types.CompileTargetType)memSubChunk.ReadInt32();

                  perConfigSetting.PreBuildChain.Active = ( memSubChunk.ReadInt32() == 1 );
                  int numEntries = memSubChunk.ReadInt32();
                  for ( int j = 0; j < numEntries; ++j )
                  {
                    var entry = new BuildChainEntry();

                    entry.ProjectName       = memSubChunk.ReadString();
                    entry.Config            = memSubChunk.ReadString();
                    entry.DocumentFilename  = memSubChunk.ReadString();
                    entry.PreDefines        = memSubChunk.ReadString();

                    perConfigSetting.PreBuildChain.Entries.Add( entry );
                  }

                  perConfigSetting.PostBuildChain.Active = ( memSubChunk.ReadInt32() == 1 );
                  numEntries = memSubChunk.ReadInt32();
                  for ( int j = 0; j < numEntries; ++j )
                  {
                    var entry = new BuildChainEntry();

                    entry.ProjectName = memSubChunk.ReadString();
                    entry.Config = memSubChunk.ReadString();
                    entry.DocumentFilename = memSubChunk.ReadString();
                    entry.PreDefines = memSubChunk.ReadString();

                    perConfigSetting.PostBuildChain.Entries.Add( entry );
                  }
                  element.Settings[config] = perConfigSetting;
                }
              }

              element.IsShown = ( memChunk.ReadInt32() != 0 );
              element.AssemblerType = (C64Studio.Types.AssemblerType)memChunk.ReadUInt32();

              int hierarchyPartCount = memChunk.ReadInt32();
              for ( int i = 0; i < hierarchyPartCount; ++i )
              {
                string part = memChunk.ReadString();

                element.ProjectHierarchy.Add( part );
              }

              if ( element.ProjectHierarchy.Count > 0 )
              {
                // node is sub-node, move accordingly
                System.Windows.Forms.TreeNode parentNode = NodeFromHierarchy( element.ProjectHierarchy );
                if ( ( parentNode != null )
                &&   ( parentNode != element.Node.Parent ) )
                {
                  element.Node.Remove();
                  parentNode.Nodes.Add( element.Node );
                }
              }

              // dependency - include symbols
              dependencyCount = memChunk.ReadInt32();
              for ( int i = 0; i < dependencyCount; ++i )
              {
                element.ForcedDependency.DependentOnFile[i].IncludeSymbols = ( memChunk.ReadInt32() != 0 );
              }

              // code folding entries
              int     numFoldingEntries = memChunk.ReadInt32();
              element.DocumentInfo.CollapsedFoldingBlocks = new GR.Collections.Set<int>();
              for ( int i = 0; i < numFoldingEntries; ++i )
              {
                int   collapsedBlockLine = memChunk.ReadInt32();
                element.DocumentInfo.CollapsedFoldingBlocks.Add( collapsedBlockLine  );
                //Debug.Log( "Get collapsed blocked for " + element.DocumentInfo.FullPath + ", line " + collapsedBlockLine );
              }

              // external dependencies
              int externalDependencyCount = memChunk.ReadInt32();
              for ( int i = 0; i < externalDependencyCount; ++i )
              {
                string dependency = memChunk.ReadString();
                element.ExternalDependencies.DependentOnFile.Add( new FileDependency.DependencyInfo( dependency, true, false ) );
              }

              element.BasicVersion = (BasicVersion)memChunk.ReadUInt32();

              // TODO - load other stuff
              if ( ( element != null )
              &&   ( element.IsShown ) )
              {
                ShowDocument( element );
                if ( element.Document != null )
                {
                  element.Document.ShowHint = DockState.Document;
                  //element.Document.Show( MainForm.panelMain );
                }
              }
            }
            break;
          case Types.FileChunk.PROJECT_ELEMENT_DISPLAY_DATA:
            {
              string    elementFilename = memChunk.ReadString();

              ProjectElement  element = GetElementByFilename( elementFilename );
              if ( element != null )
              {
                UInt32    numBytes = memChunk.ReadUInt32();
                GR.Memory.ByteBuffer    displayData = new GR.Memory.ByteBuffer();
                memChunk.ReadBlock( displayData, numBytes );

                if ( element.Document != null )
                {
                  element.Document.ApplyDisplayDetails( displayData );
                }
              }
            }
            break;
          case Types.FileChunk.PROJECT_CONFIG:
            {
              ProjectConfig  config = new ProjectConfig();

              config.Load( memChunk );

              if ( string.IsNullOrEmpty( config.DebugStartAddressLabel ) )
              {
                config.DebugStartAddressLabel = origDebugStartAddress.ToString();
              }

              Settings.Configuration( config.Name, config );
            }
            break;
          case Types.FileChunk.PROJECT_WATCH_ENTRY:
            {
              WatchEntry watch = new WatchEntry();

              watch.Load( memChunk );
              Core.MainForm.AddWatchEntry( watch );
              //Debug.Log( "loaded watch entry for " + watch.Name );
            }
            break;
        }
      }
      if ( Settings.GetConfigurationCount() == 0 )
      {
        // there must be one config
        ProjectConfig   config = new ProjectConfig();

        config.Name = "Default";
        Settings.Configuration( config.Name, config );
        Settings.CurrentConfig = config;
      }
      else
      {
        Settings.CurrentConfig = Settings.Configuration( currentConfig );
        if ( Settings.CurrentConfig == null )
        {
          Settings.CurrentConfig = Settings.GetConfigurations().First();
        }
      }
      foreach ( ProjectElement element in Elements )
      {
        if ( element.Settings.Count == 0 )
        {
          foreach ( var configName in Settings.GetConfigurationNames() )
          {
            // needs a default setting!
            element.Settings[configName] = new ProjectElement.PerConfigSettings();
          }
        }
        if ( ( !string.IsNullOrEmpty( element.Filename ) )
        &&   ( GR.Path.IsPathEqual( element.Filename, Settings.MainDocument ) ) )
        {
          Core.MainForm.m_SolutionExplorer.HighlightNode( element.Node );
        }

        Core.MainForm.RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.DOCUMENT_INFO_CREATED, element.DocumentInfo ) );
        Core.MainForm.RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.ELEMENT_CREATED, element ) );
      }



      if ( !String.IsNullOrEmpty( activeElement ) )
      {
        ProjectElement    element = GetElementByFilename( activeElement );
        if ( ( element != null )
        &&   ( element.Document != null ) )
        {
          element.Document.Show();
        }
      }
      m_Modified = false;
      return true;
    }



    public BaseDocument ShowDocument( ProjectElement Element )
    {
      if ( Element.DocumentInfo.Type == ProjectElement.ElementType.FOLDER )
      {
        return null;
      }
      if ( Element.Document == null )
      {
        BaseDocument    document = Core.MainForm.CreateNewDocument( Element.DocumentInfo.Type, Element.DocumentInfo.Project );
        if ( document == null )
        {
          System.Windows.Forms.MessageBox.Show( "Could not create document for " + Element.DocumentInfo.Type.ToString(), "Error creating document" );
          return null;
        }

        Element.Document = document;
        Element.Document.ShowHint = DockState.Document;
        Element.Document.Icon = Core.MainForm.IconFromType( Element.DocumentInfo );
        document.SetProjectElement( Element );
        document.Core = Core;
        document.SetDocumentFilename( Element.Filename );
        if ( Element.DocumentInfo.Project == null )
        {
          // icon for non project documents
          document.Icon = System.Drawing.SystemIcons.Asterisk;
        }
        document.ToolTipText = "";
        if ( document.DocumentFilename == null )
        {
          // a new file
          Element.Name = Element.Name;
          Element.Document.Show( Core.MainForm.panelMain );
        }
        else if ( document.Load() )
        {
          document.ToolTipText = document.DocumentInfo.FullPath;
          Element.Name = document.Text;
            //Element.Name;
          Element.Document.Show( Core.MainForm.panelMain );
        }
        else if ( !string.IsNullOrEmpty( Element.Filename ) )
        {
          Element.Document = null;
          return null;
        }

        if ( Element.Document != null )
        {
          Element.Document.DocumentInfo = Element.DocumentInfo;
          Element.DocumentInfo.BaseDoc  = Element.Document;
        }

        if ( ( Element.Document != null )
        &&   ( Element.Document is SourceASMEx ) )
        {
          Element.Document.DocumentEvent += new BaseDocument.DocumentEventHandler( Core.MainForm.Document_DocumentEvent );
        }

        // set known tokens if we have any
        bool setFromMainDoc = false;
        if ( !string.IsNullOrEmpty( Settings.MainDocument ) )
        {
          var element = GetElementByFilename( Settings.MainDocument );
          if ( ( element != null )
          &&   ( element.DocumentInfo.Type == ProjectElement.ElementType.ASM_SOURCE )
          &&   ( element.DocumentInfo.ASMFileInfo != null ) )
          {
            if ( element.DocumentInfo.ASMFileInfo.ContainsFile( Element.DocumentInfo.FullPath ) )
            {
              if ( !Core.Compiling.IsCurrentlyBuilding() )
              {
                Element.DocumentInfo.SetASMFileInfo( element.DocumentInfo.ASMFileInfo, element.DocumentInfo.KnownKeywords, element.DocumentInfo.KnownTokens );
              }
              setFromMainDoc = true;
            }
          }
        }
        if ( ( !setFromMainDoc )
        &&   ( Core.Compiling.ParserASM.ASMFileInfo.ContainsFile( Element.DocumentInfo.FullPath ) ) )
        {
          if ( !Core.Compiling.IsCurrentlyBuilding() )
          {
            Element.DocumentInfo.SetASMFileInfo( Core.Compiling.ParserASM.ASMFileInfo, Core.Compiling.ParserASM.KnownTokens(), Core.Compiling.ParserASM.KnownTokenInfo() );
          }
        }
        //Debug.Log( "m_Outline.RefreshFromDocument after showdoc" );
        Core.MainForm.m_Outline.RefreshFromDocument( Element.DocumentInfo.BaseDoc );
      }
      Element.Document.Select();
      Element.IsShown = true;
      Core.MainForm.RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.DOCUMENT_OPENED, Element.DocumentInfo ) );
      Core.MainForm.RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.ELEMENT_OPENED, Element ) );
      return Element.Document;
    }



    public ProjectElement CreateElement( ProjectElement.ElementType Type, System.Windows.Forms.TreeNode ParentNode )
    {
      ProjectElement    element = new ProjectElement();
      element.DocumentInfo.Type = Type;
      element.DocumentInfo.Project = this;
      element.DocumentInfo.UndoManager.MainForm = Core.MainForm;
      element.Node      = new System.Windows.Forms.TreeNode();
      element.Node.Tag  = element;
      element.Node.ImageIndex = (int)Type;
      element.Node.SelectedImageIndex = (int)Type;
      

      ParentNode.Nodes.Add( element.Node );
      //MainForm.m_ProjectExplorer.NodeProject.Nodes.Add( element.Node );
      element.Node.Parent.Expand();

      foreach ( var configName in Settings.GetConfigurationNames() )
      {
        element.Settings[configName] = new ProjectElement.PerConfigSettings();
      }

      Elements.AddLast( element );

      m_Modified = true;
      return element;
    }



    public void RemoveElement( ProjectElement Element )
    {
      // remove from other elements dependency
      foreach ( ProjectElement element in Elements )
      {
        if ( element.ForcedDependency.DependsOn( Element.Filename ) )
        {
          element.ForcedDependency.RemoveDependency( Element.Filename );
        }
      }

      Elements.Remove( Element );

      if ( ( Element.DocumentInfo.Type != ProjectElement.ElementType.FOLDER )
      &&   ( Element.Filename != null )
      &&   ( GR.Path.IsPathEqual( Element.Filename, Settings.MainDocument ) ) )
      {
        Settings.MainDocument = "";
      }
      m_Modified = true;
    }



    public ProjectElement GetElementByFilename( string Filename )
    {
      foreach ( ProjectElement element in Elements )
      {
        if ( ( element.DocumentInfo != null )
        &&   ( GR.Path.IsPathEqual( element.DocumentInfo.FullPath, Filename ) ) )
        {
          return element;
        }
        if ( element.Filename == Filename )
        {
          return element;
        }
      }
      return null;
    }



    public GR.Collections.Set<FileDependency.DependencyInfo> GetDependencies( ProjectElement Element )
    {
      var dependencies = new GR.Collections.Set<FileDependency.DependencyInfo>();

      foreach ( var dependency in Element.ForcedDependency.DependentOnFile )
      {
        if ( !dependencies.ContainsValue( dependency ) )
        {
          dependencies.Add( dependency );

          ProjectElement    otherElement = GetElementByFilename( dependency.Filename );
          if ( otherElement != null )
          {
            dependencies.Merge( GetDependencies( otherElement ) );
          }
        }
      }
      return dependencies;
    }



    public string FullPath( string ItemPath )
    {
      if ( System.IO.Path.IsPathRooted( ItemPath ) )
      {
        return ItemPath;
      }
      return GR.Path.Append( Settings.BasePath, ItemPath );
    }

  }
}
