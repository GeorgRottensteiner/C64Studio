using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using RetroDevStudio.Dialogs;
using RetroDevStudio.Types;
using System.Linq;
using SourceControl;
using static System.Windows.Forms.AxHost;
using System.Reflection;
using System.Xml.Linq;

namespace RetroDevStudio.Documents
{
  public partial class SolutionExplorer : BaseDocument
  {
    private System.Windows.Forms.TreeNode     m_ContextMenuNode = null;

    private System.Drawing.Font               m_BoldFont = null;

    private Dictionary<Project,System.Windows.Forms.TreeNode>   m_HighlightedNodes = new Dictionary<Project, TreeNode>();

    private System.Windows.Forms.TreeNode     m_MouseOverNode = null;
    private int                               m_MouseOverNodeTicks = 0;



    public SolutionExplorer( StudioCore Core )
    {
      this.Core = Core;

      InitializeComponent();

      GR.Image.DPIHandler.ResizeControlsForDPI( this );

      m_BoldFont = new System.Drawing.Font( treeProject.Font, System.Drawing.FontStyle.Bold );

      timerDragDrop.Tick += new EventHandler( timerDragDrop_Tick );

      Core.MainForm.ApplicationEvent += MainForm_ApplicationEvent;
    }



    private void MainForm_ApplicationEvent( Types.ApplicationEvent Event )
    {
      switch ( Event.EventType )
      {
        case Types.ApplicationEvent.Type.SOLUTION_CLOSED:
          seBtnAddExisting.Enabled = false;
          seBtnAddNewItem.Enabled = false;
          seBtnDelete.Enabled = false;
          seBtnCloneSolution.Enabled = false;
          break;
        case Types.ApplicationEvent.Type.SOLUTION_OPENED:
          // apply node expansions
          foreach ( var expandedNode in Core.Navigating.Solution.ExpandedNodes )
          {
            string[]    parts = expandedNode.Split( '*' );

            if ( ( parts.Length == 2 )
            &&   ( parts[0] == "Project" ) )
            {
              var project = Core.Navigating.Solution.GetProjectByName( parts[1] );
              if ( project != null )
              {
                project.Node.Expand();
              }
            }
            else if ( ( parts.Length == 3 )
            &&        ( parts[0] == "Element" ) )
            {
              var project = Core.Navigating.Solution.GetProjectByName( parts[1] );
              if ( project != null )
              {
                var element = project.GetElementByFilename( parts[2] );
                if ( element != null )
                {
                  element.Node.Expand();
                }
              }
            }
          }

          if ( Core.Navigating.Solution.ActiveProject != "" )
          {
            var project = Core.Navigating.Solution.GetProjectByName( Core.Navigating.Solution.ActiveProject );
            if ( project != null )
            {
              project.Node.NodeFont = m_BoldFont;
              project.Node.Text     = project.Node.Text;
            }
          }
          seBtnCloneSolution.Enabled = true;
          RefreshSourceControlState();
          break;
        case ApplicationEvent.Type.DOCUMENT_SAVED:
          RefreshSourceControlState();
          break;
      }
    }



    private void treeProject_NodeMouseDoubleClick( object sender, System.Windows.Forms.TreeNodeMouseClickEventArgs e )
    {
      if ( e.Node.Level == 0 )
      {
        // project node
        DoProjectProperties( e.Node );
        return;
      }
      Project project = ProjectFromNode( e.Node );
      ProjectElement element = ElementFromNode( e.Node );
      if ( element.Document == null )
      {
        element.Document = project.ShowDocument( element );
      }
      Core.Settings.UpdateInMRU( Core.Settings.MRUFiles, element.DocumentInfo.FullPath, Core.MainForm );

      if ( ( element.Document != Core.MainForm.ActiveDocument )
      &&   ( element.Document != null ) )
      {
        element.Document.Show();
      }
    }



    private void treeProject_NodeMouseClick( object sender, System.Windows.Forms.TreeNodeMouseClickEventArgs e )
    {
      if ( e.Button == System.Windows.Forms.MouseButtons.Right )
      {
        System.Windows.Forms.ContextMenuStrip contextMenu = new System.Windows.Forms.ContextMenuStrip();
        m_ContextMenuNode = e.Node;

        if ( e.Node != null )
        {
          Project project = ProjectFromNode( e.Node );
          if ( project == null )
          {
            return;
          }

          bool isProjectOrFolder = ( e.Node.Level == 0 );
          bool isFolder = false;
          bool isProject = ( e.Node.Level == 0 );
          var info = (TreeItemInfo)e.Node.Tag;

          Debug.Log( $"{e.Node.Text} = {info.FileState}" );
          ProjectElement nodeElement = ElementFromNode( e.Node );
          if ( ( nodeElement != null )
          &&   ( nodeElement.DocumentInfo.Type == ProjectElement.ElementType.FOLDER ) )
          {
            isProjectOrFolder = true;
            isFolder = true;
          }

          if ( isProjectOrFolder )
          {
            // Project properties
            System.Windows.Forms.ToolStripMenuItem item = new System.Windows.Forms.ToolStripMenuItem( "Add" );
            item.Tag = 0;
            contextMenu.Items.Add( item );

            System.Windows.Forms.ToolStripMenuItem subItem = new System.Windows.Forms.ToolStripMenuItem( "New" );
            item.DropDownItems.Add( subItem );

            if ( isProject )
            {
              System.Windows.Forms.ToolStripMenuItem subItemRenameProject = new System.Windows.Forms.ToolStripMenuItem( "Rename Project" );
              subItemRenameProject.Click += new EventHandler( treeElementRename_Click );
              contextMenu.Items.Add( subItemRenameProject );
            }
            else if ( isFolder )
            {
              System.Windows.Forms.ToolStripMenuItem subItemRenameProject = new System.Windows.Forms.ToolStripMenuItem( "Rename Folder" );
              subItemRenameProject.Click += new EventHandler( treeElementRename_Click );
              contextMenu.Items.Add( subItemRenameProject );
            }

            if ( isProject )
            {
              System.Windows.Forms.ToolStripMenuItem subItemCloneProject = new System.Windows.Forms.ToolStripMenuItem( "Clone Project" );
              subItemCloneProject.Click += new EventHandler( subItemCloneProject_Click );
              contextMenu.Items.Add( subItemCloneProject );
            }

            if ( isProject )
            {
              if ( Core.Navigating.Solution.ActiveProject == project.Settings.Name )
              {
                var itemUnmark = new System.Windows.Forms.ToolStripMenuItem( "Unmark as active project" );
                itemUnmark.Tag = 0;
                itemUnmark.Click += new EventHandler( treeMarkProjectAsActive_Click );
                contextMenu.Items.Add( itemUnmark );
              }
              else
              {
                var itemMark = new System.Windows.Forms.ToolStripMenuItem( "Mark as active project" );
                itemMark.Tag = 0;
                itemMark.Click += new EventHandler( treeMarkProjectAsActive_Click );
                contextMenu.Items.Add( itemMark );
              }

              System.Windows.Forms.ToolStripMenuItem subItemNewProject = new System.Windows.Forms.ToolStripMenuItem( "Project" );
              subItemNewProject.Click += new EventHandler( subItemNewProject_Click );
              subItem.DropDownItems.Add( subItemNewProject );

              subItem.DropDownItems.Add( "-" );
            }
            System.Windows.Forms.ToolStripMenuItem subItemNewFolder = new System.Windows.Forms.ToolStripMenuItem( "Folder" );
            subItemNewFolder.Click += new EventHandler( subItemNewFolder_Click );
            subItem.DropDownItems.Add( subItemNewFolder );

            System.Windows.Forms.ToolStripMenuItem subItemNewASM = new System.Windows.Forms.ToolStripMenuItem( "ASM File" );
            subItemNewASM.Click += new EventHandler( projectAddASMFile_Click );
            subItem.DropDownItems.Add( subItemNewASM );

            System.Windows.Forms.ToolStripMenuItem subItemNewBasic = new System.Windows.Forms.ToolStripMenuItem( "BASIC File" );
            subItemNewBasic.Click += new EventHandler( projectAddBasicFile_Click );
            subItem.DropDownItems.Add( subItemNewBasic );

            System.Windows.Forms.ToolStripMenuItem subItemNewSprite = new System.Windows.Forms.ToolStripMenuItem( "Sprite Set" );
            subItemNewSprite.Click += new EventHandler( projectAddSpriteFile_Click );
            subItem.DropDownItems.Add( subItemNewSprite );

            System.Windows.Forms.ToolStripMenuItem subItemNewCharacter = new System.Windows.Forms.ToolStripMenuItem( "Character Set" );
            subItemNewCharacter.Click += new EventHandler( projectAddCharacterFile_Click );
            subItem.DropDownItems.Add( subItemNewCharacter );

            System.Windows.Forms.ToolStripMenuItem subItemNewCharacterScreen = new System.Windows.Forms.ToolStripMenuItem( "Character Screen" );
            subItemNewCharacterScreen.Click += new EventHandler( projectAddCharacterScreenFile_Click );
            subItem.DropDownItems.Add( subItemNewCharacterScreen );

            System.Windows.Forms.ToolStripMenuItem subItemNewGraphicScreen = new System.Windows.Forms.ToolStripMenuItem( "Graphic Screen" );
            subItemNewGraphicScreen.Click += new EventHandler( projectAddGraphicScreenFile_Click );
            subItem.DropDownItems.Add( subItemNewGraphicScreen );

            System.Windows.Forms.ToolStripMenuItem subItemNewMap = new System.Windows.Forms.ToolStripMenuItem( "Map" );
            subItemNewMap.Click += new EventHandler( projectAddMap_Click );
            subItem.DropDownItems.Add( subItemNewMap );

            System.Windows.Forms.ToolStripMenuItem subItemNewValueTable = new System.Windows.Forms.ToolStripMenuItem( "Value Table" );
            subItemNewValueTable.Click += new EventHandler( projectAddValueTable_Click );
            subItem.DropDownItems.Add( subItemNewValueTable );

            System.Windows.Forms.ToolStripMenuItem subItemAddExisting = new System.Windows.Forms.ToolStripMenuItem( "Existing File(s)" );
            subItemAddExisting.Click += new EventHandler( projectAddExistingFile_Click );
            item.DropDownItems.Add( subItemAddExisting );

            System.Windows.Forms.ToolStripMenuItem subItemAddExistingProject = new System.Windows.Forms.ToolStripMenuItem( "Existing Project" );
            subItemAddExistingProject.Click += new EventHandler( subItemAddExistingProject_Click );
            item.DropDownItems.Add( subItemAddExistingProject );

            contextMenu.Items.Add( "-" );

            item = new System.Windows.Forms.ToolStripMenuItem( "Open Explorer here" );
            item.Tag = 0;
            item.Click += new EventHandler( openFolderClick );
            contextMenu.Items.Add( item );

            contextMenu.Items.Add( "-" );

            if ( ( isFolder )
            ||   ( isProject ) )
            {
              System.Windows.Forms.ToolStripMenuItem folderItem = new System.Windows.Forms.ToolStripMenuItem( "Remove from solution" );
              folderItem.Tag = 0;
              folderItem.Click += new EventHandler( treeElementRemove_Click );
              contextMenu.Items.Add( folderItem );
            }

            if ( isFolder )
            {
              int     numCharScreens = 0;
              bool    onlyCharScreens = true;


              foreach ( System.Windows.Forms.TreeNode childNode in e.Node.Nodes )
              {
                ProjectElement childElement = ElementFromNode( childNode );
                if ( childElement != null )
                {
                  if ( childElement.DocumentInfo.Type == ProjectElement.ElementType.GRAPHIC_SCREEN )
                  {
                    ++numCharScreens;
                  }
                  else
                  {
                    onlyCharScreens = false;
                  }
                }
              }
              if ( onlyCharScreens )
              {
                item = new System.Windows.Forms.ToolStripMenuItem( "Calc combined charset" );
                item.Tag = 0;
                item.Click += new EventHandler( calcCombinedCharset_Click );
                contextMenu.Items.Add( item );
              }
            }

            contextMenu.Items.Add( "-" );

            item = new System.Windows.Forms.ToolStripMenuItem( "Properties" );
            item.Tag = 0;
            item.Click += new EventHandler( treeProjectProperties_Click );
            contextMenu.Items.Add( item );

            if ( ( isProject )
            &&   ( global::SourceControl.Controller.IsFunctional ) )
            {
              contextMenu.Items.Add( "-" );

              if ( global::SourceControl.Controller.IsFolderUnderSourceControl( project.FullPath( "" ) ) )
              {
                item = new System.Windows.Forms.ToolStripMenuItem( "Refresh State" );
                item.Tag = 0;
                item.Click += SourceControlRefreshState;
                contextMenu.Items.Add( item );

                if ( project.SourceControl.HasChanges )
                {
                  item = new System.Windows.Forms.ToolStripMenuItem( "Stage Changes" );
                  item.Tag = 0;
                  item.Click += SourceControlCommitChanges;
                  contextMenu.Items.Add( item );
                }

                if ( info.FileState == FileState.NewInWorkdir )
                {
                  item = new System.Windows.Forms.ToolStripMenuItem( "Add to repository" );
                  item.Tag = info;
                  item.Click += SourceControlAddToRepo;
                  contextMenu.Items.Add( item );

                  item = new System.Windows.Forms.ToolStripMenuItem( "Ignore" );
                  item.Tag = info;
                  item.Click += SourceControlIgnore;
                  contextMenu.Items.Add( item );
                }
                if ( ( ( info.FileState & FileState.NewInIndex ) != 0 )
                ||   ( ( info.FileState & FileState.ModifiedInIndex ) != 0 )
                ||   ( ( info.FileState & FileState.RenamedInIndex ) != 0 )
                ||   ( ( info.FileState & FileState.TypeChangeInIndex ) != 0 ) )
                {
                  item = new System.Windows.Forms.ToolStripMenuItem( "Remove from repository" );
                  item.Tag = info;
                  item.Click += SourceControlRemoveFromRepo;
                  contextMenu.Items.Add( item );
                }
              }
              else
              {
                item = new System.Windows.Forms.ToolStripMenuItem( "Create Repository" );
                item.Tag = 0;
                item.Click += CreateRepository;
                contextMenu.Items.Add( item );
              }
            }
          }
          else
          {
            // element properties
            ProjectElement element = ElementFromNode( m_ContextMenuNode );

            System.Windows.Forms.ToolStripMenuItem item;

            if ( element.Filename != null )
            {
              if ( GR.Path.IsPathEqual( element.Filename, element.DocumentInfo.Project.Settings.MainDocument ) )
              {
                item = new System.Windows.Forms.ToolStripMenuItem( "Unmark as active element" );
                item.Tag = 0;
                item.Click += new EventHandler( treeMarkAsActive_Click );
                contextMenu.Items.Add( item );
              }
              else
              {
                item = new System.Windows.Forms.ToolStripMenuItem( "Mark as active element" );
                item.Tag = 0;
                item.Click += new EventHandler( treeMarkAsActive_Click );
                contextMenu.Items.Add( item );
              }
            }

            contextMenu.Items.Add( "-" );

            item = new System.Windows.Forms.ToolStripMenuItem( "Copy" );
            item.Tag = 0;
            item.Click += new EventHandler( treeCopyElement_Click );
            contextMenu.Items.Add( item );

            item = new System.Windows.Forms.ToolStripMenuItem( "Paste" );
            item.Tag = 0;
            item.Click += new EventHandler( treePasteElement_Click );
            contextMenu.Items.Add( item );

            IDataObject dataObj = Clipboard.GetDataObject();
            if ( ( dataObj != null )
            &&   ( dataObj.GetDataPresent( "RetroDevStudio.SolutionFile" ) ) )
            {
              item.Enabled = true;
            }
            else
            {
              item.Enabled = false;
            }

            /*
            item = new System.Windows.Forms.ToolStripMenuItem( "Save As" );
            item.Tag = 0;
            item.Click += new EventHandler( treeSaveAsElement_Click );
            contextMenu.Items.Add( item );

            item = new System.Windows.Forms.ToolStripMenuItem( "Save Copy As" );
            item.Tag = 0;
            item.Click += new EventHandler( treeSaveCopyAsElement_Click );
            contextMenu.Items.Add( item );
            */

            contextMenu.Items.Add( "-" );

            if ( ( element.DocumentInfo.Type == ProjectElement.ElementType.ASM_SOURCE )
            ||   ( element.DocumentInfo.Type == ProjectElement.ElementType.BASIC_SOURCE ) )
            {
              item = new System.Windows.Forms.ToolStripMenuItem( "Build" );
              item.Tag = 0;
              item.Click += new EventHandler( treeElementBuild_Click );
              item.Enabled = ( Core.MainForm.AppState == RetroDevStudio.Types.StudioState.NORMAL );
              contextMenu.Items.Add( item );

              item = new System.Windows.Forms.ToolStripMenuItem( "Rebuild" );
              item.Tag = 0;
              item.Click += new EventHandler( treeElementRebuild_Click );
              item.Enabled = ( Core.MainForm.AppState == RetroDevStudio.Types.StudioState.NORMAL );
              contextMenu.Items.Add( item );
            }
            item = new System.Windows.Forms.ToolStripMenuItem( "Rename file" );
            item.Tag = 0;
            item.Click += new EventHandler( treeElementRename_Click );
            contextMenu.Items.Add( item );
            if ( String.IsNullOrEmpty( element.Filename ) )
            {
              item.Enabled = false;
            }

            item = new System.Windows.Forms.ToolStripMenuItem( "Remove from project" );
            item.Tag = 0;
            item.Click += new EventHandler( treeElementRemove_Click );
            contextMenu.Items.Add( item );

            contextMenu.Items.Add( "-" );

            item = new System.Windows.Forms.ToolStripMenuItem( "Open Explorer here" );
            item.Tag = 0;
            item.Click += new EventHandler( openFolderClick );
            contextMenu.Items.Add( item );

            contextMenu.Items.Add( "-" );

            item = new System.Windows.Forms.ToolStripMenuItem( "Properties" );
            item.Tag = 0;
            item.Click += new EventHandler( treeElementProperties_Click );
            contextMenu.Items.Add( item );

            if ( ( global::SourceControl.Controller.IsFunctional )
            &&   ( project.SourceControl != null ) )
            {
              contextMenu.Items.Add( "-" );

              if ( info.FileState == FileState.NewInWorkdir )
              {
                item = new System.Windows.Forms.ToolStripMenuItem( "Add to repository" );
                item.Tag = info;
                item.Click += SourceControlAddToRepo;
                contextMenu.Items.Add( item );

                item = new System.Windows.Forms.ToolStripMenuItem( "Ignore" );
                item.Tag = info;
                item.Click += SourceControlIgnore;
                contextMenu.Items.Add( item );
              }
              if ( ( ( info.FileState & FileState.NewInIndex ) != 0 )
              ||   ( ( info.FileState & FileState.ModifiedInIndex ) != 0 )
              ||   ( ( info.FileState & FileState.RenamedInIndex ) != 0 )
              ||   ( ( info.FileState & FileState.TypeChangeInIndex ) != 0 ) )
              {
                item = new System.Windows.Forms.ToolStripMenuItem( "Remove from repository" );
                item.Tag = info;
                item.Click += SourceControlRemoveFromRepo;
                contextMenu.Items.Add( item );
              }
            }
          }
        }
        contextMenu.Show( treeProject.PointToScreen( e.Location ) );
      }
    }



    private void SourceControlCommitChanges( object sender, EventArgs e )
    {
      var info = (TreeItemInfo)m_ContextMenuNode.Tag;
      if ( info.Project.SourceControl.CommitChanges() )
      {
        RefreshSourceControlState();
      }
    }



    private void SourceControlIgnore( object sender, EventArgs e )
    {
      var info = (TreeItemInfo)( (ToolStripMenuItem)sender ).Tag;

      var project = ProjectFromNode( m_ContextMenuNode );
      if ( info.Project != null )
      {
        if ( project.SourceControl.Ignore( System.IO.Path.GetFileName( info.Project.Settings.Filename ) ) )
        {
          info.FileState = project.SourceControl.GetFileState( System.IO.Path.GetFileName( info.Project.Settings.Filename ) );
          treeProject.Invalidate();
        }
      }
      else
      {
        if ( project.SourceControl.Ignore( info.Element.DocumentInfo.DocumentFilename ) )
        {
          info.FileState = project.SourceControl.GetFileState( info.Element.DocumentInfo.DocumentFilename );
          treeProject.Invalidate();
        }
      }
    }



    private void SourceControlRemoveFromRepo( object sender, EventArgs e )
    {
      var info = (TreeItemInfo)( (ToolStripMenuItem)sender ).Tag;

      var project = ProjectFromNode( m_ContextMenuNode );
      if ( info.Project != null )
      {
        if ( project.SourceControl.RemoveFileFromIndex( System.IO.Path.GetFileName( info.Project.Settings.Filename ) ) )
        {
          info.FileState = project.SourceControl.GetFileState( System.IO.Path.GetFileName( info.Project.Settings.Filename ) );
          treeProject.Invalidate();
        }
      }
      else
      {
        if ( project.SourceControl.RemoveFileFromIndex( info.Element.DocumentInfo.DocumentFilename ) )
        {
          info.FileState = project.SourceControl.GetFileState( info.Element.DocumentInfo.DocumentFilename );
          treeProject.Invalidate();
        }
      }
    }



    private void SourceControlAddToRepo( object sender, EventArgs e )
    {
      var info = (TreeItemInfo)( (ToolStripMenuItem)sender ).Tag;

      var project = ProjectFromNode( m_ContextMenuNode );
      if ( info.Project != null )
      {
        if ( project.SourceControl.AddFileToIndex( System.IO.Path.GetFileName( info.Project.Settings.Filename ) ) )
        {
          info.FileState = project.SourceControl.GetFileState( System.IO.Path.GetFileName( info.Project.Settings.Filename ) );
          treeProject.Invalidate();
        }
      }
      else
      {
        if ( project.SourceControl.AddFileToIndex( info.Element.DocumentInfo.DocumentFilename ) )
        {
          info.FileState = project.SourceControl.GetFileState( info.Element.DocumentInfo.DocumentFilename );
          treeProject.Invalidate();
        }
      }
    }



    private void SourceControlRefreshState( object sender, EventArgs e )
    {
      RefreshSourceControlState();
    }



    private void CreateRepository( object sender, EventArgs e )
    {
      var project = ProjectFromNode( m_ContextMenuNode );
      if ( project == null )
      {
        return;
      }

      global::SourceControl.Controller.CreateRepositoryInFolder( project.FullPath( "" ) );
    }



    private void subItemRename_Click( object sender, EventArgs e )
    {
      treeProject.SelectedNode = m_ContextMenuNode;
      treeProject.StartLabelEdit();
    }



    private void treeSaveCopyAsElement_Click( object sender, EventArgs e )
    {
      throw new NotImplementedException();
    }



    private void treeSaveAsElement_Click( object sender, EventArgs e )
    {
      throw new NotImplementedException();
    }



    private void treeMarkProjectAsActive_Click( object sender, EventArgs e )
    {
      Project         project = ProjectFromNode( m_ContextMenuNode );
      if ( project == null )
      {
        return;
      }

      if ( Core.Navigating.Solution.ActiveProject != "" )
      {
        var unmarkProject = Core.Navigating.Solution.GetProjectByName( Core.Navigating.Solution.ActiveProject );
        if ( unmarkProject != null )
        {
          // unmark
          unmarkProject.Node.NodeFont = treeProject.Font;
          unmarkProject.Node.Text     = unmarkProject.Node.Text;

          Core.Navigating.Solution.ActiveProject = "";
          Core.Navigating.Solution.Modified = true;
        }
        if ( unmarkProject == project )
        {
          return;
        }
      }

      project.Node.NodeFont = m_BoldFont;
      project.Node.Text     = project.Node.Text;
      
      Core.Navigating.Solution.ActiveProject = project.Settings.Name;
      Core.Navigating.Solution.Modified = true;
    }



    private void subItemCloneProject_Click( object sender, EventArgs e )
    {
      var project = ProjectFromNode( m_ContextMenuNode, false );
      if ( project == null )
      {
        return;
      }
      Core.MainForm.CloneProject( project );
    }



    private void projectAddValueTable_Click( object sender, EventArgs e )
    {
      AddNewFile( ProjectElement.ElementType.VALUE_TABLE, "Value Table", m_ContextMenuNode );
    }



    private void subItemNewProject_Click( object sender, EventArgs e )
    {
      Core.MainForm.AddNewProject();
    }



    void subItemAddExistingProject_Click( object sender, EventArgs e )
    {
      string filename;

      if ( OpenFile( "Add existing project", Types.Constants.FILEFILTER_PROJECT, out filename ) )
      {
        Core.MainForm.OpenProject( filename );
      }
    }



    void subItemNewFolder_Click( object sender, EventArgs e )
    {
      AddNewFolder( m_ContextMenuNode );
    }



    void AddNewFolder( TreeNode Node )
    {
      if ( Node == null )
      {
        return;
      }
      Project         project = ProjectFromNode( Node );
      List<string>    hierarchy = GetElementHierarchy( Node );
      ProjectElement element = Core.MainForm.CreateNewElement( ProjectElement.ElementType.FOLDER, "Folder", project, Node );
      if ( element != null )
      {
        treeProject.SelectedNode = element.Node;
        treeProject.StartLabelEdit();
      }
    }



    void projectAddExistingFile_Click( object sender, EventArgs e )
    {
      Core.MainForm.ImportExistingFiles( m_ContextMenuNode );
    }



    void AddNewFile( ProjectElement.ElementType Type, string Description, TreeNode Node )
    {
      Project curProject = ProjectFromNode( Node );

      // find insertable top node
      ProjectElement    element = ElementFromNode( Node );
      if ( element != null )
      {
        while ( ( element.DocumentInfo.Type != ProjectElement.ElementType.PROJECT )
        &&      ( element.DocumentInfo.Type != ProjectElement.ElementType.SOLUTION )
        &&      ( element.DocumentInfo.Type != ProjectElement.ElementType.FOLDER ) )
        {
          Node = Node.Parent;
          element = ElementFromNode( Node );
          if ( element == null )
          {
            var     isProject = ProjectFromNode( Node );
            if ( isProject != null )
            {
              break;
            }
            return;
          }
        }
      }

      Core.MainForm.AddNewElement( Type, Description, curProject, Node );
    }



    void projectAddASMFile_Click( object sender, EventArgs e )
    {
      AddNewFile( ProjectElement.ElementType.ASM_SOURCE, "ASM File", m_ContextMenuNode );
    }



    void projectAddBasicFile_Click( object sender, EventArgs e )
    {
      AddNewFile( ProjectElement.ElementType.BASIC_SOURCE, "BASIC File", m_ContextMenuNode );
    }



    void projectAddSpriteFile_Click( object sender, EventArgs e )
    {
      AddNewFile( ProjectElement.ElementType.SPRITE_SET, "Sprite Set", m_ContextMenuNode );
    }



    void projectAddCharacterFile_Click( object sender, EventArgs e )
    {
      AddNewFile( ProjectElement.ElementType.CHARACTER_SET, "Character Set", m_ContextMenuNode );
    }



    void projectAddCharacterScreenFile_Click( object sender, EventArgs e )
    {
      AddNewFile( ProjectElement.ElementType.CHARACTER_SCREEN, "Character Screen", m_ContextMenuNode );
    }



    void projectAddGraphicScreenFile_Click( object sender, EventArgs e )
    {
      AddNewFile( ProjectElement.ElementType.GRAPHIC_SCREEN, "Graphic Screen", m_ContextMenuNode );
    }



    void projectAddMap_Click( object sender, EventArgs e )
    {
      AddNewFile( ProjectElement.ElementType.MAP_EDITOR, "Map Editor", m_ContextMenuNode );
    }



    void treeMarkAsActive_Click( object sender, EventArgs e )
    {
      Project         project = ProjectFromNode( m_ContextMenuNode );
      ProjectElement  element = ElementFromNode( m_ContextMenuNode );

      if ( GR.Path.IsPathEqual( element.Filename, project.Settings.MainDocument ) )
      {
        // unmark
        if ( m_HighlightedNodes.ContainsKey( project ) )
        {
          var node = m_HighlightedNodes[project];

          node.NodeFont = treeProject.Font;
          node.Text = node.Text;

          m_HighlightedNodes.Remove( project );
        }
        project.Settings.MainDocument = "";
        project.SetModified();
        return;
      }
      HighlightNode( m_ContextMenuNode );
      project.Settings.MainDocument = element.Filename;
      project.SetModified();
    }



    void treeCopyElement_Click( object sender, EventArgs e )
    {
      CopyElement( m_ContextMenuNode );
    }



    void CopyElement( TreeNode Node )
    {
      ProjectElement  element = ElementFromNode( Node );
      if ( element != null )
      {
        GR.Memory.ByteBuffer    fileData = new GR.Memory.ByteBuffer();
        // magic number
        fileData.AppendU32( 0x12345678 );

        string  filenameToCopy = element.DocumentInfo.FullPath;
        string  clipboardDataName = "RetroDevStudio.SolutionFile";
        if ( element.DocumentInfo.Type == ProjectElement.ElementType.FOLDER )
        {
          filenameToCopy = element.Name;
          clipboardDataName = "RetroDevStudio.Folder";
        }
        byte[]    fileName = Encoding.Unicode.GetBytes( filenameToCopy );
        fileData.AppendI32( fileName.Length );
        fileData.Append( fileName );

        byte[]    projectFile = Encoding.Unicode.GetBytes( element.DocumentInfo.Project.Settings.Filename );
        var projectData = new GR.Memory.ByteBuffer();
        projectData.AppendU32( 0x12345678 );
        projectData.AppendI32( projectFile.Length );
        projectData.Append( projectFile );

        DataObject dataObj = new DataObject();

        dataObj.SetData( clipboardDataName, false, fileData.MemoryStream() );
        dataObj.SetData( "RetroDevStudio.ProjectFile", false, projectData.MemoryStream() );

        Clipboard.SetDataObject( dataObj, true );
      }
    }



    void treePasteElement_Click( object sender, EventArgs e )
    {
      PasteElement( m_ContextMenuNode );
    }



    void PasteElement( TreeNode Node )
    {
      if ( Node == null )
      {
        return;
      }
      Project   project = ProjectFromNode( Node.Parent );
      TreeNode  parentNodeToInsertTo = Node.Parent;
      if ( project == null )
      {
        // paste onto project directly?
        project = ProjectFromNode( Node );
        if ( Node.Parent == null )
        {
          parentNodeToInsertTo = Node;
        }
      }
      Project   sourceProject = project;

      if ( project != null )
      {
        IDataObject dataObj = Clipboard.GetDataObject();
        if ( dataObj == null )
        {
          return;
        }

        if ( dataObj.GetDataPresent( "RetroDevStudio.ProjectFile" ) )
        {
          System.IO.MemoryStream ms = (System.IO.MemoryStream)dataObj.GetData( "RetroDevStudio.ProjectFile" );

          GR.Memory.ByteBuffer clipData = new GR.Memory.ByteBuffer( (uint)ms.Length );

          ms.Read( clipData.Data(), 0, (int)ms.Length );

          GR.IO.MemoryReader memIn = clipData.MemoryReader();

          if ( memIn.ReadUInt32() != 0x12345678 )
          {
            return;
          }
          int   fileLength = memIn.ReadInt32();

          string sourceProjectFile  = Encoding.Unicode.GetString( clipData.Data(), 8, fileLength );
          sourceProject             = Core.Navigating.Solution.GetProjectByFilename( sourceProjectFile );
        }
        if ( dataObj.GetDataPresent( "RetroDevStudio.Folder" ) )
        {
          System.IO.MemoryStream ms = (System.IO.MemoryStream)dataObj.GetData( "RetroDevStudio.Folder" );

          GR.Memory.ByteBuffer clipData = new GR.Memory.ByteBuffer( (uint)ms.Length );

          ms.Read( clipData.Data(), 0, (int)ms.Length );

          GR.IO.MemoryReader memIn = clipData.MemoryReader();

          if ( memIn.ReadUInt32() != 0x12345678 )
          {
            return;
          }
          int   fileLength = memIn.ReadInt32();

          string  fileName = Encoding.Unicode.GetString( clipData.Data(), 8, fileLength );

          ProjectElement element = project.CreateElement( ProjectElement.ElementType.FOLDER, parentNodeToInsertTo );

          string relativeFilename = fileName;
          element.Name            = relativeFilename;
          element.Filename        = relativeFilename;

          while ( parentNodeToInsertTo.Level >= 1 )
          {
            element.ProjectHierarchy.Insert( 0, parentNodeToInsertTo.Text );
            parentNodeToInsertTo = parentNodeToInsertTo.Parent;
          }
          element.DocumentInfo.DocumentFilename = relativeFilename;
          if ( element.Document != null )
          {
            element.Document.SetDocumentFilename( relativeFilename );
          }
          project.SetModified();
        }

        if ( dataObj.GetDataPresent( "RetroDevStudio.SolutionFile" ) )
        {
          System.IO.MemoryStream ms = (System.IO.MemoryStream)dataObj.GetData( "RetroDevStudio.SolutionFile" );

          GR.Memory.ByteBuffer clipData = new GR.Memory.ByteBuffer( (uint)ms.Length );

          ms.Read( clipData.Data(), 0, (int)ms.Length );

          GR.IO.MemoryReader memIn = clipData.MemoryReader();

          if ( memIn.ReadUInt32() != 0x12345678 )
          {
            return;
          }
          int   fileLength = memIn.ReadInt32();

          string  fileName = Encoding.Unicode.GetString( clipData.Data(), 8, fileLength );

          ProjectElement  elementToCopy = sourceProject.GetElementByFilename( fileName );

          string  newFilenameTemplate = GR.Path.RenameFilenameWithoutExtension( fileName, System.IO.Path.GetFileNameWithoutExtension( fileName ) + " Copy" );
          string  newFilename = newFilenameTemplate;

          if ( sourceProject != project )
          {
            // copy file to different project, keep base path
            newFilenameTemplate = GR.Path.RelativePathTo( System.IO.Path.GetFullPath( sourceProject.Settings.BasePath ), true, fileName, false );
            newFilenameTemplate = project.FullPath( newFilenameTemplate );
            newFilename = newFilenameTemplate;
          }

          int     curAttempt = 2;
          while ( project.IsFilenameInUse( newFilename ) )
          {
            newFilename = GR.Path.RenameFilenameWithoutExtension( newFilenameTemplate, System.IO.Path.GetFileNameWithoutExtension( newFilenameTemplate ) + " " + curAttempt );
            ++curAttempt;
          }

          //Debug.Log( "Pasting: " + fileName + " as " + newFilename );

          try
          {
            System.IO.File.Copy( fileName, newFilename );
          }
          catch ( System.Exception ex )
          {
            System.Windows.Forms.MessageBox.Show( "Could not create copy of the file!\r\n" + ex.Message, "Error creating copy" );
            return;
          }

          ProjectElement element = project.CreateElement( elementToCopy.DocumentInfo.Type, parentNodeToInsertTo );

          string relativeFilename = GR.Path.RelativePathTo( System.IO.Path.GetFullPath( project.Settings.BasePath ), true, newFilename, false );
          element.Name = System.IO.Path.GetFileNameWithoutExtension( relativeFilename );
          element.Filename = relativeFilename;

          while ( parentNodeToInsertTo.Level >= 1 )
          {
            element.ProjectHierarchy.Insert( 0, parentNodeToInsertTo.Text );
            parentNodeToInsertTo = parentNodeToInsertTo.Parent;
          }
          element.DocumentInfo.DocumentFilename = relativeFilename;
          if ( element.Document != null )
          {
            element.Document.SetDocumentFilename( relativeFilename );
          }
          project.SetModified();
        }
        if ( dataObj.GetDataPresent( "FileDrop" ) )
        {
          // pasted a file (name) from explorer
          string[] files = dataObj.GetData("FileDrop") as string[];
          if ( files == null )
          {
            return;
          }
          foreach ( var fileToPaste in files )
          {
            Core.MainForm.AddExistingFileToProject( project, parentNodeToInsertTo, fileToPaste, true );
          }
        }
      }
    }



    void RemoveAndDeleteElement( ProjectElement Element )
    {
      foreach ( TreeNode childNode in Element.Node.Nodes )
      {
        ProjectElement childElement = ElementFromNode( childNode );
        RemoveAndDeleteElement( childElement );
      }
      if ( Element.DocumentInfo.Type != ProjectElement.ElementType.FOLDER )
      {
        try
        {
          System.IO.File.Delete( GR.Path.Append( Element.DocumentInfo.Project.Settings.BasePath, Element.Filename ) );
        }
        catch ( System.Exception ex )
        {
          Core.AddToOutput( "Could not delete file " + GR.Path.Append( Element.DocumentInfo.Project.Settings.BasePath, Element.Filename ) + ", " + ex.Message );
        }
      }
      if ( Element.Document != null )
      {
        Core.MainForm.RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.DOCUMENT_CLOSED, Element.DocumentInfo ) );
        Element.Document.Close();
      }
      Core.Navigating.Solution.RemoveElement( Element );
      Core.MainForm.RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.ELEMENT_REMOVED, Element ) );
      Core.MainForm.RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.DOCUMENT_INFO_REMOVED, Element.DocumentInfo ) );
    }



    void RemoveElement( ProjectElement Element )
    {
      foreach ( TreeNode childNode in Element.Node.Nodes )
      {
        ProjectElement childElement = ElementFromNode( childNode );
        if ( childElement != null )
        {
          RemoveElement( childElement );
        }
      }
      if ( Element.Document != null )
      {
        Core.MainForm.RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.DOCUMENT_CLOSED, Element.DocumentInfo ) );
        Element.Document.Close();
      }
      Core.Navigating.Solution.RemoveElement( Element );
      Core.MainForm.RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.ELEMENT_REMOVED, Element ) );
      Core.MainForm.RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.DOCUMENT_INFO_REMOVED, Element.DocumentInfo ) );
    }



    void treeElementRemove_Click( object sender, EventArgs e )
    {
      DeleteNode( m_ContextMenuNode );
    }



    void DeleteNode( TreeNode Node )
    {
      if ( Node == null )
      {
        return;
      }
      if ( Node.Level == 0 )
      {
        // remove a (the?) project
        Project projectToRemove = ProjectFromNode( Node );

        if ( projectToRemove != null )
        {
          if ( Core.Navigating.Solution.Projects.Count == 1 )
          {
            System.Windows.Forms.MessageBox.Show( "You can't remove the last project from a solution.", "Last Project!", MessageBoxButtons.OK );
            return;
          }
          Core.MainForm.CloseProject( projectToRemove );
          Core.Navigating.Solution.Modified = true;
          Core.MainForm.SaveSolution();
        }
        return;
      }
      ProjectElement element = ElementFromNode( Node );

      if ( element.DocumentInfo.Type == ProjectElement.ElementType.FOLDER )
      {
        System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show( "You chose to remove the folder " + element.Name + ". Do you also want to delete the files of its childs?", "Delete files?", System.Windows.Forms.MessageBoxButtons.YesNoCancel );
        if ( result == DialogResult.Cancel )
        {
          return;
        }
        if ( result == DialogResult.No )
        {
          // simply remove all elements
          RemoveElement( element );
        }
        else
        {
          // actually delete files recursively
          RemoveAndDeleteElement( element );
        }
        return;
      }

      if ( String.IsNullOrEmpty( element.Filename ) )
      {
        if ( element.Document != null )
        {
          Core.MainForm.RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.DOCUMENT_CLOSED, element.DocumentInfo ) );
          element.Document.Close();
        }
        element.DocumentInfo.Project.RemoveElement( element );
        treeProject.Nodes.Remove( Node );
        Core.MainForm.RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.ELEMENT_REMOVED, element ) );
        Core.MainForm.RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.DOCUMENT_INFO_REMOVED, element.DocumentInfo ) );
        element.DocumentInfo.Project.SetModified();
      }
      else
      {
        // there is a file attached
        System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show( "You chose to remove an element from the project. Do you also want to delete the file?", "Delete File?", System.Windows.Forms.MessageBoxButtons.YesNoCancel );

        if ( result == System.Windows.Forms.DialogResult.Cancel )
        {
          return;
        }
        if ( result == System.Windows.Forms.DialogResult.Yes )
        {
          RemoveAndDeleteElement( element );
        }
        else
        {
          RemoveElement( element );
        }
        element.DocumentInfo.Project.SetModified();
      }
    }



    void treeElementRename_Click( object sender, EventArgs e )
    {
      if ( ( m_ContextMenuNode != null )
      &&   ( m_ContextMenuNode.Level >= 0 ) )
      {
        treeProject.SelectedNode = m_ContextMenuNode;
        treeProject.StartLabelEdit();
      }
    }



    void treeElementBuild_Click( object sender, EventArgs e )
    {
      var elementToBuild = ElementFromNode( m_ContextMenuNode );
      if ( elementToBuild != null )
      {
        if ( elementToBuild.Document == null )
        {
          if ( elementToBuild.DocumentInfo.Project.ShowDocument( elementToBuild ) == null )
          {
            return;
          }
        }
        elementToBuild.DocumentInfo.Project.Core.MainForm.Build( elementToBuild.DocumentInfo );
      }
    }



    void treeElementRebuild_Click( object sender, EventArgs e )
    {
      var elementToBuild = ElementFromNode( m_ContextMenuNode );
      if ( elementToBuild != null )
      {
        elementToBuild.DocumentInfo.Project.Core.MainForm.Rebuild( elementToBuild.DocumentInfo );
      }
    }



    void DoProjectProperties( TreeNode Node )
    {
      Project project = ProjectFromNode( Node, false );
      if ( project == null )
      {
        return;
      }

      var dlgProps = new ProjectProperties( project, project.Settings, Core );
      dlgProps.ShowDialog();

      if ( dlgProps.Modified )
      {
        project.SetModified();
      }
    }



    void calcCombinedCharset_Click( object sender, EventArgs e )
    {
      var projects = new List<string>();

      string basePath = "";

      foreach ( System.Windows.Forms.TreeNode childNode in m_ContextMenuNode.Nodes )
      {
        var element = ElementFromNode( childNode );
        if ( ( element != null )
        &&   ( element.DocumentInfo.Type == ProjectElement.ElementType.GRAPHIC_SCREEN ) )
        {
          Debug.Log( "Add project " + element.DocumentInfo.FullPath );
          if ( string.IsNullOrEmpty( basePath ) )
          {
            basePath = GR.Path.RemoveFileSpec( element.DocumentInfo.FullPath );
          }
          projects.Add( element.DocumentInfo.FullPath );
        }
      }

      Converter.CombinedGraphicsToCharset   converter = new RetroDevStudio.Converter.CombinedGraphicsToCharset();

      converter.ConvertScreens( basePath, projects );
    }



    void treeProjectProperties_Click( object sender, EventArgs e )
    {
      DoProjectProperties( m_ContextMenuNode );
    }



    void treeElementProperties_Click( object sender, EventArgs e )
    {
      ProjectElement element = ElementFromNode( m_ContextMenuNode );

      ElementProperties   dlgProps = new ElementProperties( Core, element );

      dlgProps.ShowDialog();
    }



    void openFolderClick( object sender, EventArgs e )
    {
      Project project = ProjectFromNode( m_ContextMenuNode, false );
      string    openFolderPath = "";
      if ( project != null )
      {
        // project folder
        openFolderPath = project.Settings.BasePath;
      }
      else
      {
        ProjectElement element = ElementFromNode( m_ContextMenuNode );

        if ( element.Document == null )
        {
          openFolderPath = element.Filename;
          if ( !System.IO.Path.IsPathRooted( openFolderPath ) )
          {
            openFolderPath = GR.Path.Normalize( GR.Path.Append( element.DocumentInfo.Project.Settings.BasePath, openFolderPath ), false );
          }
          openFolderPath = GR.Path.RemoveFileSpec( openFolderPath );
        }
        else
        {
          openFolderPath = GR.Path.RemoveFileSpec( element.DocumentInfo.FullPath );
        }
      }

      Process.Start( "explorer.exe", openFolderPath );
    }



    public void HighlightNode( System.Windows.Forms.TreeNode Node )
    {
      var project = ProjectFromNode( Node );
      if ( m_HighlightedNodes.ContainsKey( project ) )
      {
        var node = m_HighlightedNodes[project];

        node.NodeFont = treeProject.Font;
        node.Text = node.Text;

        m_HighlightedNodes.Remove( project );
      }

      m_HighlightedNodes.Add( project, Node );
      Node.NodeFont = m_BoldFont;
      Node.Text     = Node.Text;
    }



    private void treeProject_BeforeLabelEdit( object sender, System.Windows.Forms.NodeLabelEditEventArgs e )
    {
      if ( e.Node.Level < 1 )
      {
        e.CancelEdit = true;
        return;
      }
      ProjectElement element = ElementFromNode( e.Node );
      if ( ( element.DocumentInfo.Type != ProjectElement.ElementType.FOLDER )
      &&   ( element.DocumentInfo.Type != ProjectElement.ElementType.PROJECT ) )
      {
        e.Node.Text = System.IO.Path.GetFileNameWithoutExtension( e.Node.Text );
      }
    }



    private void treeProject_AfterLabelEdit( object sender, System.Windows.Forms.NodeLabelEditEventArgs e )
    {
      if ( ( e.Node.Level < 0 )
      ||   ( string.IsNullOrEmpty( e.Label ) ) )
      {
        e.CancelEdit = true;
        return;
      }
      string    newText = e.Label;
      Project project = ProjectFromNode( e.Node );

      if ( e.Node.Level == 0 )
      {
        if ( Core.Navigating.Solution.IsValidProjectName( newText ) )
        {
          string    originalValue = project.Settings.Name;
          Core.Navigating.Solution.RenameProject( project, newText );

          Core.MainForm.RaiseApplicationEvent( new RetroDevStudio.Types.ApplicationEvent( RetroDevStudio.Types.ApplicationEvent.Type.PROJECT_RENAMED )
              {
                OriginalValue = originalValue,
                UpdatedValue = newText 
              }
            );
        }
        return;
      }

      ProjectElement element = ElementFromNode( e.Node );
      if ( element.DocumentInfo.Type != ProjectElement.ElementType.FOLDER )
      {
        newText = e.Label + System.IO.Path.GetExtension( e.Node.Text );
      }
      else
      {
        newText = e.Label;
      }

      if ( element.DocumentInfo.Type == ProjectElement.ElementType.FOLDER )
      {
        element.Name = newText;
        e.Node.Text = newText;
        AdjustElementHierarchy( element, e.Node );
        project.SetModified();
        return;
      }

      string    oldFilename = element.DocumentInfo.FullPath;
      string    newFilename = System.IO.Path.Combine( System.IO.Path.GetDirectoryName( oldFilename ), newText );

      if ( System.IO.Path.GetExtension( newFilename ).ToUpper() != System.IO.Path.GetExtension( oldFilename ).ToUpper() )
      {
        e.CancelEdit = true;
        System.Windows.Forms.MessageBox.Show( "You can't change the extension of an included file!", "Renaming extension forbidden", MessageBoxButtons.OK );
        return;
      }

      if ( element.Document != null )
      {
        element.Document.DisableFileWatcher();
      }
      try
      {
        System.IO.File.Move( oldFilename, newFilename );
      }
      catch ( System.IO.FileNotFoundException )
      {
        // ignore this specific error, go on renaming
      }
      catch ( System.Exception ex )
      {
        e.CancelEdit = true;
        System.Windows.Forms.MessageBox.Show( "An error occurred while renaming\r\n" + ex.Message, "Error while renaming" );
        if ( element.Document != null )
        {
          element.Document.EnableFileWatcher();
        }
        return;
      }
      if ( Core.Navigating.Solution != null )
      {
        Core.Navigating.Solution.RenameElement( element, oldFilename, newFilename );
      }
      AdjustElementHierarchy( element, e.Node );

      if ( element.Document != null )
      {
        element.Document.EnableFileWatcher();
      }

      project.SetModified();
    }



    private void treeProject_KeyDown( object sender, System.Windows.Forms.KeyEventArgs e )
    {
      if ( e.KeyCode == System.Windows.Forms.Keys.F2 )
      {
        if ( treeProject.SelectedNode.Level >= 0 )
        {
          treeProject.StartLabelEdit();
          e.Handled = true;
          e.SuppressKeyPress = true;
        }
      }
      else if ( e.KeyCode == System.Windows.Forms.Keys.Delete )
      {
        DeleteNode( treeProject.SelectedNode );
        e.Handled = true;
        e.SuppressKeyPress = true;
      }
      else if ( ( e.KeyCode == System.Windows.Forms.Keys.C )
      &&        ( e.Control ) )
      {
        CopyElement( treeProject.SelectedNode );
        e.Handled = true;
        e.SuppressKeyPress = true;
      }
      else if ( ( e.KeyCode == System.Windows.Forms.Keys.V )
      &&        ( e.Control ) )
      {
        PasteElement( treeProject.SelectedNode );
        e.Handled = true;
        e.SuppressKeyPress = true;
      }
    }



    private void treeProject_ItemDrag( object sender, System.Windows.Forms.ItemDragEventArgs e )
    {
      System.Windows.Forms.TreeNode node = (System.Windows.Forms.TreeNode)e.Item;

      if ( node.Level == 0 )
      {
        return;
      }
      timerDragDrop.Interval = 100;
      timerDragDrop.Start();
      DoDragDrop( e.Item, DragDropEffects.Move );

      // drag drop done either way, remove drop indicator if still present
      Refresh();
    }



    private bool ContainsNode( TreeNode node1, TreeNode node2 )
    {
      // Check the parent node of the second node.
      if ( node2.Parent == null )
      {
        return false;
      }
      if ( node2.Parent.Equals( node1 ) )
      {
        return true;
      }
      // If the parent node is not null or equal to the first node, 
      // call the ContainsNode method recursively using the parent of 
      // the second node.
      return ContainsNode( node1, node2.Parent );
    }



    public List<string> GetElementHierarchy( TreeNode Node )
    {
      List<string> list = new List<string>();

      TreeNode curNode = Node;
      while ( curNode.Parent != null )
      {
        curNode = curNode.Parent;

        ProjectElement curElement = ElementFromNode( curNode );
        if ( curElement == null )
        {
          break;
        }
        if ( curElement.DocumentInfo.Type == ProjectElement.ElementType.FOLDER )
        {
          list.Insert( 0, curElement.Name );
        }
      }
      return list;
    }



    private void AdjustElementHierarchy( ProjectElement DraggedElement, TreeNode DraggedNode )
    {
      DraggedElement.Node = DraggedNode;
      DraggedElement.ProjectHierarchy = GetElementHierarchy( DraggedNode );
      foreach ( TreeNode node in DraggedNode.Nodes )
      {
        AdjustElementHierarchy( ElementFromNode( node ), node );
      }
    }



    private void treeProject_DragDrop( object sender, System.Windows.Forms.DragEventArgs e )
    {
      if ( ( e.Data.GetDataPresent( "System.Windows.Forms.TreeNode", false ) )
      &&   ( !string.IsNullOrEmpty( NodeMap ) ) )
      {
        TreeNode MovingNode = (TreeNode)e.Data.GetData( "System.Windows.Forms.TreeNode" );
        string[] NodeIndexes = this.NodeMap.Split( '|' );
        TreeNodeCollection InsertCollection = treeProject.Nodes;
        for ( int i = 0; i < NodeIndexes.Length - 1; i++ )
        {
          InsertCollection = InsertCollection[Int32.Parse( NodeIndexes[i] )].Nodes;
        }

        if ( InsertCollection != null )
        {
          // node will change, reapply new node!
          InsertCollection.Insert( Int32.Parse( NodeIndexes[NodeIndexes.Length - 1] ), (TreeNode)MovingNode.Clone() );
          treeProject.SelectedNode = InsertCollection[Int32.Parse( NodeIndexes[NodeIndexes.Length - 1] )];
          MovingNode.Remove();

          // reset element hierarchy
          TreeNode draggedNode = treeProject.SelectedNode;

          ProjectElement draggedElement = ElementFromNode( draggedNode );

          AdjustElementHierarchy( draggedElement, draggedNode );

          // reorder in element list
          Project draggedProject = ProjectFromNode( draggedNode );

          draggedProject.Elements.Remove( draggedElement );

          ProjectElement targetElement = ElementFromNode( draggedNode.Parent );
          if ( targetElement == null )
          {
            draggedProject.Elements.Add( draggedElement );
          }
          else
          {
            draggedProject.Elements.Insert( draggedProject.Elements.IndexOf( targetElement ) + 1, draggedElement );
          }
          draggedProject.SetModified();
        }
      }
      timerDragDrop.Stop();
    }



    private void treeProject_DragEnter( object sender, System.Windows.Forms.DragEventArgs e )
    {
      e.Effect = e.AllowedEffect;
    }



    public Project ProjectFromNode( TreeNode Node )
    {
      return ProjectFromNode( Node, true );
    }



    public Project ProjectFromNode( TreeNode Node, bool Recursive )
    {
      if ( Node == null )
      {
        return null;
      }

      if ( Recursive )
      {
        while ( Node.Level > 0 )
        {
          Node = Node.Parent;
        }
      }
      if ( Node.Level != 0 )
      {
        return null;
      }
      return ( (TreeItemInfo)Node.Tag ).Project;
    }



    public ProjectElement ElementFromNode( TreeNode Node )
    {
      if ( ( Node == null )
      ||   ( Node.Level == 0 ) )
      {
        return null;
      }
      return ( (TreeItemInfo)Node.Tag ).Element;
    }



    bool CanNodeBeInProject( TreeNode NodeParent, TreeNode NodeChild )
    {
      Project   origProject = ProjectFromNode( NodeChild );
      Project   newProject = ProjectFromNode( NodeParent );

      // can only drag inside one project
      if ( origProject != newProject )
      {
        return false;
      }
      return true;
    }



    bool CanNodeBeChildOf( TreeNode NodeParent, TreeNode NodeChild )
    {
      ProjectElement    element = ElementFromNode( NodeParent );
      if ( ( element == null )
      ||   ( element.DocumentInfo.Type == ProjectElement.ElementType.FOLDER ) )
      {
        // either project or folder
        return true;
      }
      return false;
    }



    private string NodeMap;
    private const int MAPSIZE = 128;
    private StringBuilder NewNodeMap = new StringBuilder( MAPSIZE );

    private void treeProject_DragOver( object sender, System.Windows.Forms.DragEventArgs e )
    {
      TreeNode NodeOver = treeProject.GetNodeAt( treeProject.PointToClient( Cursor.Position ) );
      TreeNode NodeMoving = (TreeNode)e.Data.GetData( "System.Windows.Forms.TreeNode" );

      if ( !CanNodeBeInProject( NodeOver, NodeMoving ) )
      {
        e.Effect = DragDropEffects.None;
        return;
      }
      e.Effect = DragDropEffects.Move;

      // A bit long, but to summarize, process the following code only if the nodeover is null
      // and either the nodeover is not the same thing as nodemoving UNLESSS nodeover happens
      // to be the last node in the branch (so we can allow drag & drop below a parent branch)
      if ( ( NodeOver != null )
      &&   ( ( NodeOver != NodeMoving )
      ||     ( ( NodeOver.Parent != null )
      &&       ( NodeOver.Index == ( NodeOver.Parent.Nodes.Count - 1 ) ) ) ) )
      {
        int OffsetY = treeProject.PointToClient( Cursor.Position ).Y - NodeOver.Bounds.Top;

        if ( ( OffsetY < ( NodeOver.Bounds.Height / 2 ) )
        &&   ( NodeOver == treeProject.Nodes[0] ) )
        {
          // not above the first node!
          e.Effect = DragDropEffects.None;
          return;
        }

        int NodeOverImageWidth = 16;
        if ( NodeOver.ImageIndex != -1 )
        {
          NodeOverImageWidth = treeProject.ImageList.Images[NodeOver.ImageIndex].Size.Width + 8;
        }
        Graphics g = treeProject.CreateGraphics();

        if ( !CanNodeBeChildOf( NodeOver, NodeMoving ) )
        {
          if ( OffsetY < ( NodeOver.Bounds.Height / 2 ) )
          {
            //this.lblDebug.Text = "top";

            TreeNode tnParadox = NodeOver;
            while ( tnParadox.Parent != null )
            {
              if ( tnParadox.Parent == NodeMoving )
              {
                NodeMap = "";
                return;
              }
              tnParadox = tnParadox.Parent;
            }
            SetNewNodeMap( NodeOver, false );
            if ( SetMapsEqual() )
            {
              return;
            }
            Refresh();
            DrawLeafTopPlaceholders( NodeOver );
          }
          else
          {
            //this.lblDebug.Text = "bottom";

            TreeNode tnParadox = NodeOver;
            while ( tnParadox.Parent != null )
            {
              if ( tnParadox.Parent == NodeMoving )
              {
                NodeMap = "";
                return;
              }
              tnParadox = tnParadox.Parent;
            }
            TreeNode ParentDragDrop = null;
            // If the node the mouse is over is the last node of the branch we should allow
            // the ability to drop the "nodemoving" node BELOW the parent node
            if ( ( NodeOver.Parent != null )
            &&   ( NodeOver.Index == ( NodeOver.Parent.Nodes.Count - 1 ) ) )
            {
              int XPos = treeProject.PointToClient( Cursor.Position ).X;
              if ( XPos < NodeOver.Bounds.Left )
              {
                ParentDragDrop = NodeOver.Parent;

                if ( XPos < ( ParentDragDrop.Bounds.Left - treeProject.ImageList.Images[ParentDragDrop.ImageIndex].Size.Width ) )
                {
                  if ( ParentDragDrop.Parent != null )
                  {
                    ParentDragDrop = ParentDragDrop.Parent;
                  }
                }
              }
            }
            // Since we are in a special case here, use the ParentDragDrop node as the current "nodeover"
            SetNewNodeMap( ParentDragDrop != null ? ParentDragDrop : NodeOver, true );
            if ( SetMapsEqual() )
            {
              return;
            }
            Refresh();
            DrawLeafBottomPlaceholders( NodeOver, ParentDragDrop );
          }
        }
        else
        {
          if ( OffsetY < ( NodeOver.Bounds.Height / 3 ) )
          {
            //this.lblDebug.Text = "folder top";

            TreeNode tnParadox = NodeOver;
            while ( tnParadox.Parent != null )
            {
              if ( tnParadox.Parent == NodeMoving )
              {
                NodeMap = "";
                return;
              }

              tnParadox = tnParadox.Parent;
            }
            SetNewNodeMap( NodeOver, false );
            if ( SetMapsEqual() )
            {
              return;
            }
            Refresh();
            DrawFolderTopPlaceholders( NodeOver );
          }
          else if ( ( NodeOver.Parent != null )
          &&        ( NodeOver.Index == 0 ) 
          &&        ( OffsetY > ( NodeOver.Bounds.Height - ( NodeOver.Bounds.Height / 3 ) ) ) )
          {
            //this.lblDebug.Text = "folder bottom";

            TreeNode tnParadox = NodeOver;
            while ( tnParadox.Parent != null )
            {
              if ( tnParadox.Parent == NodeMoving )
              {
                NodeMap = "";
                return;
              }

              tnParadox = tnParadox.Parent;
            }
            SetNewNodeMap( NodeOver, true );
            if ( SetMapsEqual() )
            {
              return;
            }
            Refresh();
            DrawFolderTopPlaceholders( NodeOver );
          }
          else
          {
            //this.lblDebug.Text = "folder over";

            if ( NodeOver.Nodes.Count > 0 )
            {

              //NodeOver.Expand();
              //this.Refresh();
            }
            else
            {
              if ( NodeMoving == NodeOver )
              {
                return;
              }
              TreeNode tnParadox = NodeOver;
              while ( tnParadox.Parent != null )
              {
                if ( tnParadox.Parent == NodeMoving )
                {
                  NodeMap = "";
                  return;
                }

                tnParadox = tnParadox.Parent;
              }
              SetNewNodeMap( NodeOver, false );
              NewNodeMap = NewNodeMap.Insert( NewNodeMap.Length, "|0" );

              if ( SetMapsEqual() )
              {
                return;
              }
              Refresh();
              DrawAddToFolderPlaceholder( NodeOver );
            }
          }
        }
      }
    }



    private void DrawLeafTopPlaceholders( TreeNode NodeOver )
    {
      Graphics g = treeProject.CreateGraphics();

      int NodeOverImageWidth = treeProject.ImageList.Images[NodeOver.ImageIndex].Size.Width + 8;
      int LeftPos = NodeOver.Bounds.Left - NodeOverImageWidth;
      int RightPos = treeProject.Width - 4;

      Point[] LeftTriangle = new Point[5]{
												   new Point(LeftPos, NodeOver.Bounds.Top - 4),
												   new Point(LeftPos, NodeOver.Bounds.Top + 4),
												   new Point(LeftPos + 4, NodeOver.Bounds.Y),
												   new Point(LeftPos + 4, NodeOver.Bounds.Top - 1),
												   new Point(LeftPos, NodeOver.Bounds.Top - 5)};

      Point[] RightTriangle = new Point[5]{
													new Point(RightPos, NodeOver.Bounds.Top - 4),
													new Point(RightPos, NodeOver.Bounds.Top + 4),
													new Point(RightPos - 4, NodeOver.Bounds.Y),
													new Point(RightPos - 4, NodeOver.Bounds.Top - 1),
													new Point(RightPos, NodeOver.Bounds.Top - 5)};


      g.FillPolygon( System.Drawing.Brushes.Black, LeftTriangle );
      g.FillPolygon( System.Drawing.Brushes.Black, RightTriangle );
      g.DrawLine( new System.Drawing.Pen( Color.Black, 2 ), new Point( LeftPos, NodeOver.Bounds.Top ), new Point( RightPos, NodeOver.Bounds.Top ) );

    }



    private void DrawLeafBottomPlaceholders( TreeNode NodeOver, TreeNode ParentDragDrop )
    {
      Graphics g = treeProject.CreateGraphics();

      int NodeOverImageWidth = treeProject.ImageList.Images[NodeOver.ImageIndex].Size.Width + 8;
      // Once again, we are not dragging to node over, draw the placeholder using the ParentDragDrop bounds
      int LeftPos, RightPos;
      if ( ParentDragDrop != null )
        LeftPos = ParentDragDrop.Bounds.Left - ( treeProject.ImageList.Images[ParentDragDrop.ImageIndex].Size.Width + 8 );
      else
        LeftPos = NodeOver.Bounds.Left - NodeOverImageWidth;
      RightPos = treeProject.Width - 4;

      Point[] LeftTriangle = new Point[5]{
												   new Point(LeftPos, NodeOver.Bounds.Bottom - 4),
												   new Point(LeftPos, NodeOver.Bounds.Bottom + 4),
												   new Point(LeftPos + 4, NodeOver.Bounds.Bottom),
												   new Point(LeftPos + 4, NodeOver.Bounds.Bottom - 1),
												   new Point(LeftPos, NodeOver.Bounds.Bottom - 5)};

      Point[] RightTriangle = new Point[5]{
													new Point(RightPos, NodeOver.Bounds.Bottom - 4),
													new Point(RightPos, NodeOver.Bounds.Bottom + 4),
													new Point(RightPos - 4, NodeOver.Bounds.Bottom),
													new Point(RightPos - 4, NodeOver.Bounds.Bottom - 1),
													new Point(RightPos, NodeOver.Bounds.Bottom - 5)};


      g.FillPolygon( System.Drawing.Brushes.Black, LeftTriangle );
      g.FillPolygon( System.Drawing.Brushes.Black, RightTriangle );
      g.DrawLine( new System.Drawing.Pen( Color.Black, 2 ), new Point( LeftPos, NodeOver.Bounds.Bottom ), new Point( RightPos, NodeOver.Bounds.Bottom ) );
    }



    private void DrawFolderTopPlaceholders( TreeNode NodeOver )
    {
      Graphics g = treeProject.CreateGraphics();
      int NodeOverImageWidth = 16;
      if ( NodeOver.ImageIndex != -1 )
      {
        NodeOverImageWidth = treeProject.ImageList.Images[NodeOver.ImageIndex].Size.Width + 8;
      }

      int LeftPos, RightPos;
      LeftPos = NodeOver.Bounds.Left - NodeOverImageWidth;
      RightPos = treeProject.Width - 4;

      Point[] LeftTriangle = new Point[5]{
												   new Point(LeftPos, NodeOver.Bounds.Top - 4),
												   new Point(LeftPos, NodeOver.Bounds.Top + 4),
												   new Point(LeftPos + 4, NodeOver.Bounds.Y),
												   new Point(LeftPos + 4, NodeOver.Bounds.Top - 1),
												   new Point(LeftPos, NodeOver.Bounds.Top - 5)};

      Point[] RightTriangle = new Point[5]{
													new Point(RightPos, NodeOver.Bounds.Top - 4),
													new Point(RightPos, NodeOver.Bounds.Top + 4),
													new Point(RightPos - 4, NodeOver.Bounds.Y),
													new Point(RightPos - 4, NodeOver.Bounds.Top - 1),
													new Point(RightPos, NodeOver.Bounds.Top - 5)};


      g.FillPolygon( System.Drawing.Brushes.Black, LeftTriangle );
      g.FillPolygon( System.Drawing.Brushes.Black, RightTriangle );
      g.DrawLine( new System.Drawing.Pen( Color.Black, 2 ), new Point( LeftPos, NodeOver.Bounds.Top ), new Point( RightPos, NodeOver.Bounds.Top ) );

    }



    private void DrawAddToFolderPlaceholder( TreeNode NodeOver )
    {
      Graphics g = treeProject.CreateGraphics();
      int RightPos = NodeOver.Bounds.Right + 6;
      Point[] RightTriangle = new Point[5]{
													new Point(RightPos, NodeOver.Bounds.Y + (NodeOver.Bounds.Height / 2) + 4),
													new Point(RightPos, NodeOver.Bounds.Y + (NodeOver.Bounds.Height / 2) + 4),
													new Point(RightPos - 4, NodeOver.Bounds.Y + (NodeOver.Bounds.Height / 2)),
													new Point(RightPos - 4, NodeOver.Bounds.Y + (NodeOver.Bounds.Height / 2) - 1),
													new Point(RightPos, NodeOver.Bounds.Y + (NodeOver.Bounds.Height / 2) - 5)};

      this.Refresh();
      g.FillPolygon( System.Drawing.Brushes.Black, RightTriangle );
    }



    private void SetNewNodeMap( TreeNode tnNode, bool boolBelowNode )
    {
      NewNodeMap.Length = 0;

      if ( boolBelowNode )
        NewNodeMap.Insert( 0, (int)tnNode.Index + 1 );
      else
        NewNodeMap.Insert( 0, (int)tnNode.Index );
      TreeNode tnCurNode = tnNode;

      while ( tnCurNode.Parent != null )
      {
        tnCurNode = tnCurNode.Parent;

        if ( NewNodeMap.Length == 0 && boolBelowNode == true )
        {
          NewNodeMap.Insert( 0, ( tnCurNode.Index + 1 ) + "|" );
        }
        else
        {
          NewNodeMap.Insert( 0, tnCurNode.Index + "|" );
        }
      }
    }



    private bool SetMapsEqual()
    {
      if ( this.NewNodeMap.ToString() == this.NodeMap )
        return true;
      else
      {
        this.NodeMap = this.NewNodeMap.ToString();
        return false;
      }
    }



    void timerDragDrop_Tick( object sender, EventArgs e )
    {
      // get node at mouse position
      Point pt = PointToClient( MousePosition );
      TreeNode node = treeProject.GetNodeAt( pt );

      if ( m_MouseOverNode != node )
      {
        m_MouseOverNode = node;
        m_MouseOverNodeTicks = 0;
      }
      else
      {
        ++m_MouseOverNodeTicks;
      }
      if ( node == null )
      {
        return;
      }

      // if mouse is near to the top, scroll up
      if ( pt.Y < 30 )
      {
        // set actual node to the upper one
        if ( node.PrevVisibleNode != null )
        {
          node = node.PrevVisibleNode;

          // check if scroll still required
          if ( !node.IsVisible )
          {
            // hide drag image
            //DragHelper.ImageList_DragShowNolock( false );
            // scroll and refresh
            node.EnsureVisible();
            treeProject.Refresh();
            // show drag image
            //DragHelper.ImageList_DragShowNolock( true );
          }
        }
      }
      // if mouse is near to the bottom, scroll down
      else if ( pt.Y > treeProject.Size.Height - 30 )
      {
        if ( node.NextVisibleNode != null )
        {
          node = node.NextVisibleNode;

          if ( !node.IsVisible )
          {
            //DragHelper.ImageList_DragShowNolock( false );
            node.EnsureVisible();
            treeProject.Refresh();
            //DragHelper.ImageList_DragShowNolock( true );
          }
        }
      }

      if ( node.Nodes.Count > 0 )
      {
        // 10 ticks equals 1 second
        if ( m_MouseOverNodeTicks == 10 )
        {
          node.Expand();
        }
      }
    }



    private void seBtnAddNewFolder_Click( object sender, EventArgs e )
    {
      AddNewFolder( treeProject.SelectedNode );
    }



    private void seBtnAddNewASMFile_Click( object sender, EventArgs e )
    {
      AddNewFile( ProjectElement.ElementType.ASM_SOURCE, "ASM File", treeProject.SelectedNode );
    }



    private void seBtnAddNewBASICFile_Click( object sender, EventArgs e )
    {
      AddNewFile( ProjectElement.ElementType.BASIC_SOURCE, "BASIC File", treeProject.SelectedNode );
    }



    private void seBtnAddNewSpriteSet_Click( object sender, EventArgs e )
    {
      AddNewFile( ProjectElement.ElementType.SPRITE_SET, "Sprite Set", treeProject.SelectedNode );
    }



    private void seBtnAddNewCharacterSet_Click( object sender, EventArgs e )
    {
      AddNewFile( ProjectElement.ElementType.CHARACTER_SET, "Character Set", treeProject.SelectedNode );
    }



    private void seBtnAddNewCharScreen_Click( object sender, EventArgs e )
    {
      AddNewFile( ProjectElement.ElementType.CHARACTER_SCREEN, "Character Screen", treeProject.SelectedNode );
    }



    private void seBtnAddNewGraphicScreen_Click( object sender, EventArgs e )
    {
      AddNewFile( ProjectElement.ElementType.GRAPHIC_SCREEN, "Graphic Screen", treeProject.SelectedNode );
    }



    private void seBtnAddNewMap_Click( object sender, EventArgs e )
    {
      AddNewFile( ProjectElement.ElementType.MAP_EDITOR, "Map Editor", treeProject.SelectedNode );
    }



    private void seBtnAddExisting_Click( object sender, EventArgs e )
    {
      Core.MainForm.ImportExistingFiles( treeProject.SelectedNode );
    }



    private void seBtnDelete_Click( object sender, EventArgs e )
    {
      DeleteNode( treeProject.SelectedNode );
    }



    private void treeProject_AfterSelect( object sender, TreeViewEventArgs e )
    {
      if ( treeProject.SelectedNode == null )
      {
        seBtnAddExisting.Enabled  = false;
        seBtnAddNewItem.Enabled   = false;
        seBtnDelete.Enabled       = false;
        return;
      }
      seBtnAddExisting.Enabled  = true;
      seBtnAddNewItem.Enabled   = true;
      seBtnDelete.Enabled       = true;
    }



    private void projectToolStripMenuItem_Click( object sender, EventArgs e )
    {
      Core.MainForm.AddNewProject();
    }



    private void treeProject_AfterExpand( object sender, TreeViewEventArgs e )
    {
      if ( e.Node.Tag != null )
      {
        var project = ProjectFromNode( e.Node );
        if ( project != null )
        {
          Core.Navigating.Solution.ExpandNode( project );
        }
        var element = ElementFromNode( e.Node );
        if ( ( element != null )
        &&   ( element.DocumentInfo.Type == ProjectElement.ElementType.FOLDER ) )
        {
          Core.Navigating.Solution.ExpandNode( element );
        }
      }
    }



    private void treeProject_AfterCollapse( object sender, TreeViewEventArgs e )
    {
      if ( e.Node.Tag != null )
      {
        var project = ProjectFromNode( e.Node );
        if ( project != null )
        {
          Core.Navigating.Solution.CollapseNode( project );
        }
        var element = ElementFromNode( e.Node );
        if ( ( element != null )
        &&   ( element.DocumentInfo.Type == ProjectElement.ElementType.FOLDER ) )
        {
          Core.Navigating.Solution.CollapseNode( element );
        }

      }
    }



    private void seBtnCloneSolution_Click( object sender, EventArgs e )
    {
      if ( Core.Navigating.Solution == null )
      {
        return;
      }
      Core.MainForm.CloneSolution( Core.Navigating.Solution );
    }



    // Returns the bounds of the specified node, including the region 
    // occupied by the node label and any node tag displayed.
    private Rectangle NodeBounds( TreeNode Node )
    {
      // Set the return value to the normal node bounds.
      Rectangle bounds = Node.Bounds;
      /*
      if ( node.Tag != null )
      {
        // Retrieve a Graphics object from the TreeView handle
        // and use it to calculate the display width of the tag.
        
      }*/

      Font nodeFont = Node.NodeFont;
      if ( nodeFont == null )
      {
        nodeFont = Font;
      }
      Graphics g = CreateGraphics();
      int tagWidth = (int)g.MeasureString( Node.Text, nodeFont ).Width + 4;

      // Adjust the node bounds using the calculated value.
      bounds.Width = tagWidth;
      //bounds.Offset( tagWidth / 2, 0 );
      //bounds = Rectangle.Inflate( bounds, tagWidth / 2, 0 );
      g.Dispose();

      return bounds;
    }



    private void treeProject_DrawNode( object sender, DrawTreeNodeEventArgs e )
    {
      //e.DrawDefault = true;
      Rectangle nodeRect = NodeBounds( e.Node );// e.Node.Bounds;

      TreeItemInfo info = (TreeItemInfo)e.Node.Tag; 

      // 1. draw expand/collapse icon
      if ( e.Node.Nodes.Count > 1 )
      {
        Point ptExpand = new Point( nodeRect.Location.X - 36, nodeRect.Location.Y + ( nodeRect.Height - 8 ) / 2 );
        if ( e.Node.IsExpanded )
        {
          e.Graphics.DrawRectangle( System.Drawing.SystemPens.WindowFrame, ptExpand.X, ptExpand.Y, 8, 8 );
          e.Graphics.DrawLine( System.Drawing.SystemPens.WindowFrame, ptExpand.X + 2, ptExpand.Y + 4, ptExpand.X + 6, ptExpand.Y + 4 );
        }
        else
        {
          e.Graphics.DrawRectangle( System.Drawing.SystemPens.WindowFrame, ptExpand.X, ptExpand.Y, 8, 8 );
          e.Graphics.DrawLine( System.Drawing.SystemPens.WindowFrame, ptExpand.X + 2, ptExpand.Y + 4, ptExpand.X + 6, ptExpand.Y + 4 );
          e.Graphics.DrawLine( System.Drawing.SystemPens.WindowFrame, ptExpand.X + 4, ptExpand.Y + 2, ptExpand.X + 4, ptExpand.Y + 6 );
        }
      }

      // 2. draw node icon
      if ( treeProject.ImageList != null )
      {
        if ( treeProject.ImageList.Images.ContainsKey( e.Node.ImageKey ) )
        {
          Image nodeImg = treeProject.ImageList.Images[e.Node.ImageKey];

          Point ptNodeIcon = new Point( nodeRect.Location.X - 20, nodeRect.Location.Y + ( nodeRect.Height - nodeImg.Height ) / 2 );

          e.Graphics.DrawImage( nodeImg, ptNodeIcon );
        }
        else if ( e.Node.ImageIndex < treeProject.ImageList.Images.Count )
        {
          // autofallback to first image??
          if ( e.Node.ImageIndex == -1 )
          {
            e.Node.ImageIndex = 0;
          }
          Image nodeImg = treeProject.ImageList.Images[e.Node.ImageIndex];

          Point ptNodeIcon = new Point( nodeRect.Location.X - 20, nodeRect.Location.Y + ( nodeRect.Height - nodeImg.Height ) / 2 );

          e.Graphics.DrawImage( nodeImg, ptNodeIcon );

          var scImageIndex = SourceControlIconFromState( info.FileState );

          if ( scImageIndex != -1 )
          {
            Point ptSCIcon = new Point( ptNodeIcon.X + nodeImg.Width - 8, ptNodeIcon.Y + nodeImg.Height - 8 );

            e.Graphics.DrawImage( imageListSourceControlOverlay.Images[scImageIndex], ptSCIcon.X, ptSCIcon.Y, 8, 8 );
          } 
        }
      }
      Font nodeFont = e.Node.NodeFont;
      if ( nodeFont == null )
      {
        nodeFont = ( (TreeView)sender ).Font;
      }
      e.DrawDefault = false;

      // node text
      var bounds = NodeBounds( e.Node );
      var textBounds = Rectangle.Inflate( bounds, 3, 0 );
      var bgBounds = new Rectangle( bounds.Location, bounds.Size );
      bgBounds.Offset( -3, 0 );

      // Draw the background and node text for a selected node.
      if ( ( e.State & TreeNodeStates.Selected ) != 0 )
      {
        // Draw the background of the selected node. The NodeBounds
        // method makes the highlight rectangle large enough to
        // include the text of a node tag, if one is present.
        uint color = Core.Settings.FGColor( ColorableElement.SELECTED_TEXT );
        uint bgColor = Core.Settings.BGColor( ColorableElement.BACKGROUND_CONTROL );

        // make transparent
        if ( ( color & 0xff000000 ) == 0xff000000 )
        {
          color = ( color & 0x00ffffff ) | 0x40000000;
          color = ModulateColor( color, bgColor );
        }
        e.Graphics.FillRectangle( new SolidBrush( GR.Color.Helper.FromARGB( color ) ), bgBounds );

        // Retrieve the node font. If the node font has not been set, use the TreeView font.
        uint colorText = Core.Settings.FGColor( ColorableElement.SELECTED_TEXT );
        e.Graphics.DrawString( e.Node.Text, nodeFont, new SolidBrush( GR.Color.Helper.FromARGB( colorText ) ), textBounds );
      }
      else
      {
        if ( ( e.State & TreeNodeStates.Focused ) != 0 )
        {
          uint color = Core.Settings.BGColor( ColorableElement.BACKGROUND_CONTROL );

          e.Graphics.FillRectangle( new SolidBrush( GR.Color.Helper.FromARGB( color ) ), bgBounds );
        }
        uint colorText = Core.Settings.FGColor( ColorableElement.CONTROL_TEXT );
        e.Graphics.DrawString( e.Node.Text, nodeFont, new SolidBrush( GR.Color.Helper.FromARGB( colorText ) ), textBounds );
      }

      // If the node has focus, draw the focus rectangle large, making
      // it large enough to include the text of the node tag, if present.
      if ( ( e.State & TreeNodeStates.Focused ) != 0 )
      {
        using ( Pen focusPen = new Pen( GR.Color.Helper.FromARGB( Core.Settings.FGColor( ColorableElement.CONTROL_TEXT ) ) ) )
        {
          focusPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
          bounds.Size = new Size( bgBounds.Width - 1, bgBounds.Height - 1 );
          bounds.Offset( -3, 0 );
          e.Graphics.DrawRectangle( focusPen, bounds );
        }
      }
    }



    private int SourceControlIconFromState( FileState State )
    {
      if ( State == FileState.Nonexistent )
      {
        return -1;
      }

      // new, upd, change, ignore, conflict
      if ( State == FileState.Ignored )
      {
        return 3;
      }
      if ( ( State & FileState.NewInIndex ) != 0 )
      {
        return 0;
      }
      if ( ( State == FileState.ModifiedInIndex )
      ||   ( State == FileState.ModifiedInWorkdir ) )
      {
        return 2;
      }
      if ( State == FileState.Conflicted )
      {
        return 4;
      }
      if ( State == FileState.Unaltered )
      {
        return 1;
      }
      return -1;
    }



    private uint ModulateColor( uint Color1, uint Color2 )
    {
      uint a1 = ( Color1 >> 24 );
      uint r1 = ( Color1 >> 16 ) & 0xff;
      uint g1 = ( Color1 >> 8  ) & 0xff;
      uint b1 = Color1 & 0xff;

      uint a2 = ( Color2 >> 24 );
      uint r2 = ( Color2 >> 16 ) & 0xff;
      uint g2 = ( Color2 >> 8  ) & 0xff;
      uint b2 = Color2 & 0xff;

      if ( a1 + a2 == 0 )
      {
        return 0xff000000;
      }

      uint color = 0xff000000
        | ( ( ( r1 * a1 + r2 * a2 ) / ( a1 + a2 ) ) << 16 )
        | ( ( ( g1 * a1 + g2 * a2 ) / ( a1 + a2 ) ) << 8 )
        | ( ( ( b1 * a1 + b2 * a2 ) / ( a1 + a2 ) ) << 0 );

      return color;
    }



    private void RefreshSourceControlState()
    {
      if ( !global::SourceControl.Controller.IsFunctional )
      {
        return;
      }

      bool  modified = false;

      foreach ( TreeNode projectNode in treeProject.Nodes )
      {
        var itemInfo = (TreeItemInfo)projectNode.Tag;
        if ( itemInfo.FileState != global::SourceControl.FileState.Nonexistent )
        {
          itemInfo.FileState = global::SourceControl.FileState.Nonexistent;
          modified = true;
        }

        var project = ProjectFromNode( projectNode );
        var scInfos = new List<SourceControl.FileInfo>();
        if ( project.SourceControl != null )
        {
          scInfos = project.SourceControl.CurrentAddedFiles();
          var scEntry = scInfos.FirstOrDefault( sc => sc.Filename == System.IO.Path.GetFileName( project.Settings.Filename ) );
          if ( scEntry != null )
          {
            itemInfo.FileState = (SourceControl.FileState)(int)scEntry.FileState;
          }
          else
          {
            itemInfo.FileState = global::SourceControl.FileState.Nonexistent;
          }
        }

        // iterate recursively over all nodes
        foreach ( TreeNode node in projectNode.Nodes )
        {
          RefreshNodeState( node, project, scInfos );
        }
      }
      if ( modified )
      {
        treeProject.Invalidate();
      }
    }



    private void RefreshNodeState( TreeNode Node, Project Project, List<SourceControl.FileInfo> SCInfos )
    {
      var element = ElementFromNode( Node );
      if ( element.DocumentInfo.Type == ProjectElement.ElementType.FOLDER )
      {
        foreach ( TreeNode node in Node.Nodes )
        {
          RefreshNodeState( node, Project, SCInfos );
        }
        return;
      }
      var treeInfo = (TreeItemInfo)Node.Tag;
      var scEntry = SCInfos.FirstOrDefault( sc => sc.Filename == element.DocumentInfo.DocumentFilename );
      if ( scEntry != null )
      {
        treeInfo.FileState = (SourceControl.FileState)(int)scEntry.FileState;
      }
      else
      {
        treeInfo.FileState = global::SourceControl.FileState.Nonexistent;
      }
    }



    public class TreeItemInfo
    {
      // per project
      public Project                          Project = null;

      // per element
      public ProjectElement                   Element = null;
      public global::SourceControl.FileState  FileState = global::SourceControl.FileState.Nonexistent;

    }
  }
}
