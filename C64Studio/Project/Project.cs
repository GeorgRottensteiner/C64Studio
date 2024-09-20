using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using WeifenLuo.WinFormsUI.Docking;
using static RetroDevStudio.Parser.BasicFileParser;
using RetroDevStudio.Documents;
using SourceControl;
using GR.IO;

namespace RetroDevStudio
{
  public class Project
  {
    public ProjectSettings    Settings = new ProjectSettings();
    public StudioCore         Core = null;
    private bool              m_Modified = false;
    public DecentForms.TreeView.TreeNode           Node = null;
    public MachineType        PreferredMachineType = MachineType.C64;
    public Controller         SourceControl = null;

    public List<ProjectElement>   Elements = new List<ProjectElement>();


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



    public GR.Memory.ByteBuffer ElementToBuffer( ProjectElement Element, bool IncludeChilds = true )
    {
      GR.Memory.ByteBuffer buffer = new GR.Memory.ByteBuffer();

      GR.IO.FileChunk chunkElement = new GR.IO.FileChunk( FileChunkConstants.PROJECT_ELEMENT );

      chunkElement.AppendU32( 1 );
      chunkElement.AppendU32( (uint)Element.DocumentInfo.Type );
      chunkElement.AppendString( Element.Name );
      chunkElement.AppendString( Element.Filename );

      GR.IO.FileChunk chunkElementData = new GR.IO.FileChunk( FileChunkConstants.PROJECT_ELEMENT_DATA );
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
        GR.IO.FileChunk chunkElementPerConfigSetting = new GR.IO.FileChunk( FileChunkConstants.PROJECT_ELEMENT_PER_CONFIG_SETTING );

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
      uint   flags = 0;
      if ( Element.IsShown )
      {
        flags |= 1;
      }
      chunkElement.AppendU32( flags );
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
      chunkElement.AppendU32( 0 );// (uint)Element.BasicVersion );

      // dependency - project
      chunkElement.AppendI32( Element.ForcedDependency.DependentOnFile.Count );
      foreach ( var dependency in Element.ForcedDependency.DependentOnFile )
      {
        chunkElement.AppendString( dependency.Project );
      }

      // replaces basicversion!
      chunkElement.AppendString( Element.BASICDialect );

      // bookmarks
      chunkElement.AppendI32( Element.DocumentInfo.Bookmarks.Count );
      foreach ( int lineIndex in Element.DocumentInfo.Bookmarks )
      {
        chunkElement.AppendI32( lineIndex );
      }

      chunkElement.AppendU32( (uint)( Element.BASICWriteTempFileWithoutMetaData ? 1 : 0 ) );

      buffer.Append( chunkElement.ToBuffer() );

      if ( Element.Document != null )
      {
        GR.Memory.ByteBuffer displayDetails = Element.Document.DisplayDetails();
        if ( displayDetails != null )
        {
          GR.IO.FileChunk chunkElementDisplayData = new GR.IO.FileChunk( FileChunkConstants.PROJECT_ELEMENT_DISPLAY_DATA );
          chunkElementDisplayData.AppendString( Element.Filename );
          chunkElementDisplayData.AppendU32( displayDetails.Length );
          chunkElementDisplayData.Append( displayDetails );

          buffer.Append( chunkElementDisplayData.ToBuffer() );
        }
      }
      // child elements
      if ( IncludeChilds )
      {
        foreach ( var node in Element.Node.Nodes )
        {
          ProjectElement subElement = ( (SolutionExplorer.TreeItemInfo)node.Tag ).Element;

          buffer.Append( ElementToBuffer( subElement ) );
        }
      }

      return buffer;
    }



    public GR.Memory.ByteBuffer Save()
    {
      GR.Memory.ByteBuffer bufferProject = new GR.Memory.ByteBuffer();

      bufferProject.Reserve( 1000000 );

      GR.IO.FileChunk chunkProject = new GR.IO.FileChunk( FileChunkConstants.PROJECT );

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
      uint  flags = 0;
      chunkProject.AppendU32( flags );
      chunkProject.AppendU32( (uint)PreferredMachineType );

      bufferProject.Append( chunkProject.ToBuffer() );

      foreach ( var node in Node.Nodes )
      {
        ProjectElement element = ( (SolutionExplorer.TreeItemInfo)node.Tag ).Element;

        bufferProject.Append( ElementToBuffer( element ) );
      }

      foreach ( ProjectConfig config in Settings.GetConfigurations() )
      {
        bufferProject.Append( config.Save() );
      }

      // only save watches once, for the active project
      foreach ( var watch in Settings.WatchEntries )
      {
        bufferProject.Append( watch.Save() );
      }

      // break points
      foreach ( var breakpoints in Settings.BreakPoints )
      {
        GR.IO.FileChunk chunkBreakPoints = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_BREAKPOINTS );
        chunkBreakPoints.AppendString( breakpoints.Key );

        foreach ( var bp in breakpoints.Value )
        {
          chunkBreakPoints.Append( bp.Save() );
        }

        bufferProject.Append( chunkBreakPoints.ToBuffer() );
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



    private DecentForms.TreeView.TreeNode NodeFromHierarchy( List<string> Hierarchy )
    {
      var curNode = Node;

      foreach ( string part in Hierarchy )
      {
        bool    partFound = false;
        foreach ( var childNode in curNode.Nodes )
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
      Settings.BasePath = GR.Path.GetDirectoryName( Settings.Filename );

      if ( !Load( projectData, true ) )
      {
        return false;
      }
      Settings.Filename = Filename;
      Settings.BasePath = GR.Path.GetDirectoryName( Settings.Filename );
      return true;
    }



    public bool Load( byte[] ProjectData, bool AutoCreateGUIItems )
    {
      string currentConfig = "Default";
      string activeElement = "";

      Node = new DecentForms.TreeView.TreeNode();
      Node.Tag = new SolutionExplorer.TreeItemInfo() { Project = this };
      Node.Collapse();

      GR.IO.MemoryReader memIn = new GR.IO.MemoryReader( ProjectData );

      GR.IO.FileChunk           chunk = new GR.IO.FileChunk();
      ushort                    origDebugStartAddress = 0;

      while ( chunk.ReadFromStream( memIn ) )
      {
        GR.IO.MemoryReader      memChunk = chunk.MemoryReader();
        switch ( chunk.Type )
        {
          case FileChunkConstants.PROJECT:
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
            memChunk.ReadUInt32();    // flags (all free)
            PreferredMachineType  = (MachineType)memChunk.ReadUInt32();
            if ( PreferredMachineType == MachineType.UNKNOWN )
            {
              PreferredMachineType = MachineType.C64;
            }
            if ( projectVersion == 1 )
            {
              if ( origDebugStartAddress == 2049 )
              {
                origDebugStartAddress = 0;
              }
            }

            Node.Text = Settings.Name;
            break;
          case FileChunkConstants.PROJECT_ELEMENT:
            // Element Info
            {
              // Version
              int elementVersion = (int)memChunk.ReadUInt32();

              ProjectElement.ElementType type = (ProjectElement.ElementType)memChunk.ReadUInt32();

              ProjectElement element = CreateElement( type, Node );

              if ( !FillElementFromSettingsStream( element, memChunk ) )
              {
                return false;
              }
              if ( element.DocumentInfo.Type == ProjectElement.ElementType.FOLDER )
              {
                element.Node.Text = element.Name;
              }
              else
              {
                element.Node.Text = GR.Path.GetFileName( element.Filename );
              }
              if ( Core.Navigating.Solution.IsNodeExpanded( element ) )
              {
                element.Node.Expand();
              }
              else
              {
                element.Node.Collapse();
              }

              // TODO - load other stuff
              if ( ( element != null )
              &&   ( element.IsShown )
              &&   ( AutoCreateGUIItems ) )
              {
                ShowDocument( element );
                if ( element.Document != null )
                {
                  element.Document.ShowHint = DockState.Document;
                }
              }
              if ( element.Document != null )
              {
                element.Document.RefreshDisplayOptions();
              }
            }
            break;
          case FileChunkConstants.PROJECT_ELEMENT_DISPLAY_DATA:
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
          case FileChunkConstants.PROJECT_CONFIG:
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
          case FileChunkConstants.PROJECT_WATCH_ENTRY:
            {
              WatchEntry watch = new WatchEntry();

              watch.Load( memChunk );
              Settings.WatchEntries.Add( watch );
            }
            break;
          case FileChunkConstants.SETTINGS_BREAKPOINTS:
            {
              string  bpKey = memChunk.ReadString();

              var subChunk = new FileChunk();

              while ( subChunk.ReadFromStream( memChunk ) )
              {
                var bp = new Breakpoint();
                bp.Load( subChunk.MemoryReader() );

                if ( !Settings.BreakPoints.ContainsKey( bpKey ) )
                {
                  Settings.BreakPoints.Add( bpKey, new List<Breakpoint>() );
                }
                Settings.BreakPoints[bpKey].Add( bp );
              }
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

        if ( AutoCreateGUIItems )
        {
          if ( ( !string.IsNullOrEmpty( element.Filename ) )
          &&   ( GR.Path.IsPathEqual( element.Filename, Settings.MainDocument ) ) )
          {
            Core.MainForm.m_SolutionExplorer.HighlightNode( element.Node );
          }

          Core.MainForm.RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.DOCUMENT_INFO_CREATED, element.DocumentInfo ) );
          Core.MainForm.RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.ELEMENT_CREATED, element ) );
        }
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



    public bool FillElementFromSettingsStream( ProjectElement element, MemoryReader memChunk )
    {
      element.Name      = memChunk.ReadString();
      element.Filename  = memChunk.ReadString();

      if ( string.IsNullOrEmpty( element.Filename ) )
      {
        // this could happen due to a bug I introduced with 7.9, give it a name so you can properly delete it
        element.Filename = "my name was empty.asm";
      }

      GR.IO.FileChunk           subChunk = new GR.IO.FileChunk();

      if ( !subChunk.ReadFromStream( memChunk ) )
      {
        return false;
      }
      if ( subChunk.Type != FileChunkConstants.PROJECT_ELEMENT_DATA )
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
        element.ForcedDependency.DependentOnFile.Add( new FileDependency.DependencyInfo( Settings.Name, dependency, true, false ) );
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
        if ( chunkElementPerConfigSetting.Type == FileChunkConstants.PROJECT_ELEMENT_PER_CONFIG_SETTING )
        {
          ProjectElement.PerConfigSettings    perConfigSetting = new ProjectElement.PerConfigSettings();
          GR.IO.MemoryReader      memSubChunk = chunkElementPerConfigSetting.MemoryReader();
          string    config = memSubChunk.ReadString();

          perConfigSetting.PreBuild     = memSubChunk.ReadString();
          perConfigSetting.CustomBuild  = memSubChunk.ReadString();
          perConfigSetting.PostBuild    = memSubChunk.ReadString();
          perConfigSetting.DebugFile    = memSubChunk.ReadString();
          perConfigSetting.DebugFileType = (RetroDevStudio.Types.CompileTargetType)memSubChunk.ReadInt32();

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

      uint  flags = memChunk.ReadUInt32();
      element.IsShown     = ( ( flags & 1 ) != 0 );
      element.AssemblerType = (RetroDevStudio.Types.AssemblerType)memChunk.ReadUInt32();

      int hierarchyPartCount = memChunk.ReadInt32();
      for ( int i = 0; i < hierarchyPartCount; ++i )
      {
        string part = memChunk.ReadString();

        element.ProjectHierarchy.Add( part );
      }

      if ( element.ProjectHierarchy.Count > 0 )
      {
        // node is sub-node, move accordingly
        var parentNode = NodeFromHierarchy( element.ProjectHierarchy );
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
      }

      // external dependencies
      int externalDependencyCount = memChunk.ReadInt32();
      for ( int i = 0; i < externalDependencyCount; ++i )
      {
        string dependency = memChunk.ReadString();
        element.ExternalDependencies.DependentOnFile.Add( new FileDependency.DependencyInfo( "", dependency, true, false ) );
      }

      var obsoleteBasicVersion = (BasicVersion)memChunk.ReadUInt32();

      // dependency - project
      dependencyCount = memChunk.ReadInt32();
      for ( int i = 0; i < dependencyCount; ++i )
      {
        element.ForcedDependency.DependentOnFile[i].Project = memChunk.ReadString();
      }

      element.BASICDialect = memChunk.ReadString();
      if ( string.IsNullOrEmpty( element.BASICDialect ) )
      {
        // old version, find dialect from obsoleteBasicVersion
        string  dialectKey = "BASIC V2";
        switch ( obsoleteBasicVersion )
        {
          case BasicVersion.C64_BASIC_V2:
            break;
          case BasicVersion.BASIC_LIGHTNING:
            dialectKey = "BASIC Lightning";
            break;
          case BasicVersion.LASER_BASIC:
            dialectKey = "Laser BASIC";
            break;
          case BasicVersion.SIMONS_BASIC:
            dialectKey = "Simon's BASIC";
            break;
          case BasicVersion.V3_5:
            dialectKey = "BASIC V3.5";
            break;
          case BasicVersion.V7_0:
            dialectKey = "BASIC V7.0";
            break;
        }
        if ( Core.Compiling.BASICDialects.ContainsKey( dialectKey ) )
        {
          element.BASICDialect = dialectKey;
        }
      }

      // bookmarks
      int     numBookmarks = memChunk.ReadInt32();
      element.DocumentInfo.Bookmarks = new GR.Collections.Set<int>();
      for ( int i = 0; i < numBookmarks; ++i )
      {
        int   lineIndex = memChunk.ReadInt32();
        element.DocumentInfo.Bookmarks.Add( lineIndex );
      }

      flags = memChunk.ReadUInt32();
      element.BASICWriteTempFileWithoutMetaData = ( flags & 1 ) != 0;

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
        Element.DocumentInfo.BaseDoc = document;
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
        else if ( document.LoadDocument() )
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
        &&   ( ( Element.Document is SourceASMEx )
        ||     ( Element.Document is SourceBasicEx ) ) )
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
                Element.DocumentInfo.SetASMFileInfo( element.DocumentInfo.ASMFileInfo );

                var sourceASM = (SourceASMEx)Element.Document;
                foreach ( var bps in Core.Debugging.BreakPoints.Values )
                {
                  foreach ( var bp in bps )
                  {
                    if ( bp.DocumentFilename == Element.DocumentInfo.FullPath )
                    {
                      sourceASM.AddBreakpoint( bp );
                    }
                  }
                }
              }
              setFromMainDoc = true;
            }
          }
        }
        if ( ( !setFromMainDoc )
        &&   ( Core.Compiling.ASMFileInfo != null )
        &&   ( Core.Compiling.ASMFileInfo.ContainsFile( Element.DocumentInfo.FullPath ) ) )
        {
          if ( !Core.Compiling.IsCurrentlyBuilding() )
          {
            Element.DocumentInfo.SetASMFileInfo( Core.Compiling.ASMFileInfo );
          }
        }
        Core.MainForm.m_Outline.RefreshFromDocument( Element.DocumentInfo.BaseDoc );
        Core.MainForm.m_LabelExplorer.RefreshFromDocument( Element.DocumentInfo.BaseDoc );
      }
      else
      {
        Element.Document.Show();
      }
      Element.Document.Select();
      Element.IsShown = true;
      Core.MainForm.RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.DOCUMENT_OPENED, Element.DocumentInfo ) );
      Core.MainForm.RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.ELEMENT_OPENED, Element ) );

      return Element.Document;
    }



    public ProjectElement CreateElement( ProjectElement.ElementType Type, DecentForms.TreeView.TreeNode ParentNode )
    {
      ProjectElement    element = new ProjectElement();
      element.DocumentInfo.Type = Type;
      element.DocumentInfo.Project = this;
      element.DocumentInfo.UndoManager.MainForm = Core.MainForm;

      if ( ParentNode != null )
      {
        element.Node = new DecentForms.TreeView.TreeNode();
        element.Node.Tag = new SolutionExplorer.TreeItemInfo() { Element = element };
        element.Node.ImageIndex = (int)Type;

        ParentNode.Nodes.Add( element.Node );
        element.Node.Parent.Expand();
      }

      foreach ( var configName in Settings.GetConfigurationNames() )
      {
        element.Settings[configName] = new ProjectElement.PerConfigSettings();
      }

      Elements.Add( element );

      m_Modified = true;
      return element;
    }



    public void RemoveElement( ProjectElement Element )
    {
      // remove from other elements dependency
      foreach ( ProjectElement element in Elements )
      {
        if ( element.ForcedDependency.DependsOn( Element.DocumentInfo.Project.Settings.Name, Element.Filename ) )
        {
          element.ForcedDependency.RemoveDependency( Element.DocumentInfo.Project.Settings.Name, Element.Filename );
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
      if ( GR.Path.IsPathRooted( ItemPath ) )
      {
        return ItemPath;
      }
      return GR.Path.Append( Settings.BasePath, ItemPath );
    }



  }
}
