using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace C64Studio
{
  public partial class ProjectExplorer : BaseDocument
  {
    //public System.Windows.Forms.TreeNode      NodeProject = null;

    private System.Windows.Forms.TreeNode     m_ContextMenuNode = null;

    private System.Drawing.Font               m_BoldFont = null;

    private System.Windows.Forms.TreeNode     m_HighlightedNode = null;



    public ProjectExplorer()
    {
      InitializeComponent();

      //NodeProject = new System.Windows.Forms.TreeNode( "(No Project Loaded)", (int)ProjectElement.ElementType.PROJECT, (int)ProjectElement.ElementType.PROJECT );
      //treeProject.Nodes.Add( NodeProject );

      m_BoldFont = new System.Drawing.Font( treeProject.Font, System.Drawing.FontStyle.Bold );

      timerDragDrop.Tick += new EventHandler( timerDragDrop_Tick );
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
      ProjectElement element = (ProjectElement)e.Node.Tag;
      if ( element.Document == null )
      {
        element.Document = project.ShowDocument( element );
      }
      if ( ( element.Document != MainForm.ActiveDocument )
      &&   ( element.Document != null ) )
      {
        MainForm.ActiveDocument = element.Document;
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
          ProjectElement nodeElement = ElementFromNode( e.Node );
          if ( ( nodeElement != null )
          && ( nodeElement.Type == ProjectElement.ElementType.FOLDER ) )
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

            if ( ( isFolder )
            ||   ( isProject ) )
            {
              System.Windows.Forms.ToolStripMenuItem folderItem = new System.Windows.Forms.ToolStripMenuItem( "Remove from project" );
              folderItem.Tag = 0;
              folderItem.Click += new EventHandler( treeElementRemove_Click );
              contextMenu.Items.Add( folderItem );
            }

            System.Windows.Forms.ToolStripMenuItem subItemNewFolder = new System.Windows.Forms.ToolStripMenuItem( "Folder" );
            subItemNewFolder.Click += new EventHandler( subItemNewFolder_Click );
            subItem.DropDownItems.Add( subItemNewFolder );

            System.Windows.Forms.ToolStripMenuItem subItemNewASM = new System.Windows.Forms.ToolStripMenuItem( "ASM File" );
            subItemNewASM.Click += new EventHandler( projectAddASMFile_Click );
            subItem.DropDownItems.Add( subItemNewASM );

            System.Windows.Forms.ToolStripMenuItem subItemNewBasic = new System.Windows.Forms.ToolStripMenuItem( "Basic File" );
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

            System.Windows.Forms.ToolStripMenuItem subItemAddExisting = new System.Windows.Forms.ToolStripMenuItem( "Existing File" );
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

            item = new System.Windows.Forms.ToolStripMenuItem( "Properties" );
            item.Tag = 0;
            item.Click += new EventHandler( treeProjectProperties_Click );
            contextMenu.Items.Add( item );
          }
          else
          {
            // element properties
            ProjectElement element = (ProjectElement)m_ContextMenuNode.Tag;

            System.Windows.Forms.ToolStripMenuItem item;

            if ( GR.Path.IsPathEqual( element.Filename, element.Project.Settings.MainDocument ) )
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

          }
        }
        contextMenu.Show( treeProject.PointToScreen( e.Location ) );
      }
    }



    void subItemAddExistingProject_Click( object sender, EventArgs e )
    {
      System.Windows.Forms.OpenFileDialog dlgTool = new OpenFileDialog();

      dlgTool.Filter = MainForm.FilterString( Types.Constants.FILEFILTER_STUDIO );
      if ( dlgTool.ShowDialog() == DialogResult.OK )
      {
        MainForm.OpenProject( dlgTool.FileName );
      }
    }



    void subItemNewFolder_Click( object sender, EventArgs e )
    {
      Project         project = ProjectFromNode( m_ContextMenuNode );
      List<string>    hierarchy = GetElementHierarchy( m_ContextMenuNode );
      ProjectElement element = MainForm.CreateNewElement( ProjectElement.ElementType.FOLDER, "Folder", project, m_ContextMenuNode );
      if ( element != null )
      {
        element.Node.BeginEdit();
      }
    }



    void projectAddExistingFile_Click( object sender, EventArgs e )
    {
      MainForm.ImportExistingFile( m_ContextMenuNode );
    }



    bool ChooseFilename( ProjectElement.ElementType Type, string DefaultName, out string NewName )
    {
      Project  curProject = ProjectFromNode( m_ContextMenuNode );

      System.Windows.Forms.OpenFileDialog openDlg = new System.Windows.Forms.OpenFileDialog();
      openDlg.Title = "Specify new " + DefaultName;
      NewName = "";

      string    filterSource = "";
      switch ( Type )
      {
        case ProjectElement.ElementType.ASM_SOURCE:
          filterSource += Types.Constants.FILEFILTER_ASM;
          break;
        case ProjectElement.ElementType.BASIC_SOURCE:
          filterSource += Types.Constants.FILEFILTER_BASIC;
          break;
        case ProjectElement.ElementType.CHARACTER_SCREEN:
          filterSource += Types.Constants.FILEFILTER_CHARSET_SCREEN;
          break;
        case ProjectElement.ElementType.CHARACTER_SET:
          filterSource += Types.Constants.FILEFILTER_CHARSET;
          break;
        case ProjectElement.ElementType.GRAPHIC_SCREEN:
          filterSource += Types.Constants.FILEFILTER_GRAPHIC_SCREEN;
          break;
        case ProjectElement.ElementType.SPRITE_SET:
          filterSource += Types.Constants.FILEFILTER_SPRITE;
          break;
      }
      openDlg.Filter = MainForm.FilterString( filterSource );
      openDlg.InitialDirectory = curProject.Settings.BasePath;
      openDlg.CheckFileExists = false;
      openDlg.CheckPathExists = true;
      if ( openDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return false;
      }
      NewName = openDlg.FileName;
      return true;
    }



    void projectAddASMFile_Click( object sender, EventArgs e )
    {
      AddNewFile( ProjectElement.ElementType.ASM_SOURCE, "ASM File" );
    }



    void AddNewFile( ProjectElement.ElementType Type, string Description )
    {
      string newFilename;
      if ( !ChooseFilename( Type, Description, out newFilename ) )
      {
        return;
      }

      Project curProject = ProjectFromNode( m_ContextMenuNode );
      string localizedFilename = GR.Path.RelativePathTo( System.IO.Path.GetFullPath( curProject.Settings.BasePath ), true, newFilename, false );

      ProjectElement el = MainForm.CreateNewElement( Type, Description, curProject, m_ContextMenuNode );
      el.Filename = localizedFilename;
      el.Node.Text = System.IO.Path.GetFileName( localizedFilename );
      el.Document.SetDocumentFilename( localizedFilename );
      el.Document.Save();
    }



    void projectAddBasicFile_Click( object sender, EventArgs e )
    {
      AddNewFile( ProjectElement.ElementType.BASIC_SOURCE, "Basic File" );
    }



    void projectAddSpriteFile_Click( object sender, EventArgs e )
    {
      AddNewFile( ProjectElement.ElementType.SPRITE_SET, "Sprite Set" );
    }



    void projectAddCharacterFile_Click( object sender, EventArgs e )
    {
      AddNewFile( ProjectElement.ElementType.CHARACTER_SET, "Character Set" );
    }



    void projectAddCharacterScreenFile_Click( object sender, EventArgs e )
    {
      AddNewFile( ProjectElement.ElementType.CHARACTER_SCREEN, "Character Screen" );
    }



    void projectAddGraphicScreenFile_Click( object sender, EventArgs e )
    {
      AddNewFile( ProjectElement.ElementType.GRAPHIC_SCREEN, "Graphic Screen" );
    }



    void projectAddMap_Click( object sender, EventArgs e )
    {
      AddNewFile( ProjectElement.ElementType.MAP_EDITOR, "Map Editor" );
    }



    void treeMarkAsActive_Click( object sender, EventArgs e )
    {
      Project         project = ProjectFromNode( m_ContextMenuNode );
      ProjectElement  element = ElementFromNode( m_ContextMenuNode );

      if ( GR.Path.IsPathEqual( element.Filename, project.Settings.MainDocument ) )
      {
        // unmark
        m_HighlightedNode.NodeFont = treeProject.Font;// NodeProject.NodeFont;
        m_HighlightedNode.Text = m_HighlightedNode.Text;
        m_HighlightedNode = null;
        project.Settings.MainDocument = "";
        project.SetModified();
        return;
      }
      HighlightNode( m_ContextMenuNode );
      project.Settings.MainDocument = element.Filename;
      project.SetModified();
    }



    void RemoveAndDeleteElement( ProjectElement Element )
    {
      foreach ( TreeNode childNode in Element.Node.Nodes )
      {
        ProjectElement childElement = ElementFromNode( childNode );
        RemoveAndDeleteElement( childElement );
      }
      try
      {
        System.IO.File.Delete( GR.Path.Append( Element.Project.Settings.BasePath, Element.Filename ) );
      }
      catch ( System.Exception ex )
      {
        MainForm.AddToOutput( "Could not delete file " + GR.Path.Append( Element.Project.Settings.BasePath, Element.Filename ) + ", " + ex.Message );
      }
      if ( Element.Document != null )
      {
        MainForm.RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.DOCUMENT_CLOSED, Element.Document ) );
        Element.Document.Close();
      }
      Element.Project.RemoveElement( Element );
      Element.Node.Remove();
      MainForm.RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.ELEMENT_REMOVED, Element ) );
    }



    void RemoveElement( ProjectElement Element )
    {
      foreach ( TreeNode childNode in Element.Node.Nodes )
      {
        ProjectElement childElement = ElementFromNode( childNode );
        RemoveElement( childElement );
      }
      if ( Element.Document != null )
      {
        MainForm.RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.DOCUMENT_CLOSED, Element.Document ) );
        Element.Document.Close();
      }
      Element.Project.RemoveElement( Element );
      Element.Node.Remove();
      MainForm.RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.ELEMENT_REMOVED, Element ) );
    }



    void treeElementRemove_Click( object sender, EventArgs e )
    {
      if ( m_ContextMenuNode.Level == 0 )
      {
        // remove a (the?) project
        Project projectToRemove = ProjectFromNode( m_ContextMenuNode );

        if ( projectToRemove != null )
        {
          MainForm.CloseProject( projectToRemove );
        }
        return;
      }
      ProjectElement element = (ProjectElement)m_ContextMenuNode.Tag;

      if ( element.Type == ProjectElement.ElementType.FOLDER )
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
          MainForm.RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.DOCUMENT_CLOSED, element.Document ) );
          element.Document.Close();
        }
        element.Project.RemoveElement( element );
        treeProject.Nodes.Remove( m_ContextMenuNode );
        MainForm.RaiseApplicationEvent( new C64Studio.Types.ApplicationEvent( C64Studio.Types.ApplicationEvent.Type.ELEMENT_REMOVED, element ) );
        element.Project.SetModified();
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
        element.Project.SetModified();
      }
    }



    void treeElementRename_Click( object sender, EventArgs e )
    {
      if ( ( m_ContextMenuNode != null )
      &&   ( m_ContextMenuNode.Level >= 1 ) )
      {
        m_ContextMenuNode.BeginEdit();
      }
    }



    void DoProjectProperties( TreeNode Node )
    {
      Project project = ProjectFromNode( Node, false );
      if ( project == null )
      {
        return;
      }

      ProjectProperties dlgProps = new ProjectProperties( project, project.Settings, MainForm );
      dlgProps.ShowDialog();

      Node.Text = project.Settings.Name;
      if ( dlgProps.Modified )
      {
        project.SetModified();
      }
    }



    void treeProjectProperties_Click( object sender, EventArgs e )
    {
      DoProjectProperties( m_ContextMenuNode );
    }



    void treeElementProperties_Click( object sender, EventArgs e )
    {
      ProjectElement element = (ProjectElement)m_ContextMenuNode.Tag;

      ElementProperties   dlgProps = new ElementProperties( MainForm, element );

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
        ProjectElement element = (ProjectElement)m_ContextMenuNode.Tag;

        openFolderPath = GR.Path.RemoveFileSpec( element.Document.FullPath );
      }

      Process.Start( "explorer.exe", openFolderPath );
    }



    public void HighlightNode( System.Windows.Forms.TreeNode Node )
    {
      if ( m_HighlightedNode != null )
      {
        m_HighlightedNode.NodeFont = treeProject.Font;// NodeProject.NodeFont;
        m_HighlightedNode.Text = m_HighlightedNode.Text;
      }
      m_HighlightedNode = Node;
      Node.NodeFont = m_BoldFont;
      m_HighlightedNode.Text = m_HighlightedNode.Text;
    }



    private void treeProject_BeforeLabelEdit( object sender, System.Windows.Forms.NodeLabelEditEventArgs e )
    {
      if ( e.Node.Level < 1 )
      {
        e.CancelEdit = true;
        return;
      }
    }



    private void treeProject_AfterLabelEdit( object sender, System.Windows.Forms.NodeLabelEditEventArgs e )
    {
      if ( ( e.Node.Level < 1 )
      ||   ( string.IsNullOrEmpty( e.Label ) ) )
      {
        e.CancelEdit = true;
        return;
      }
      string    newText = e.Label;

      Project project = ProjectFromNode( e.Node );
      ProjectElement element = (ProjectElement)e.Node.Tag;
      if ( element.Type == ProjectElement.ElementType.FOLDER )
      {
        element.Name = newText;
        AdjustElementHierarchy( element, e.Node );
        project.SetModified();
        return;
      }

      string    oldFilename = element.Filename;
      if ( element.Document != null )
      {
        oldFilename = element.Document.FullPath;
      }

      string    newFilename = System.IO.Path.Combine( System.IO.Path.GetDirectoryName( oldFilename ), newText );

      try
      {
        System.IO.File.Move( oldFilename, newFilename );
      }
      catch ( System.Exception ex )
      {
        e.CancelEdit = true;
        System.Windows.Forms.MessageBox.Show( "An error occurred while renaming\r\n" + ex.Message, "Error while renaming" );
        return;
      }
      element.Name = System.IO.Path.GetFileNameWithoutExtension( newText );
      if ( element.Document != null )
      {
        element.Document.SetDocumentFilename( newFilename );
        element.Filename = System.IO.Path.GetFileName( newFilename );
        element.Document.SetModified();
      }
      else
      {
        element.Filename = newFilename;
      }
      AdjustElementHierarchy( element, e.Node );
      project.SetModified();
    }



    private void treeProject_KeyDown( object sender, System.Windows.Forms.KeyEventArgs e )
    {
      if ( e.KeyCode == System.Windows.Forms.Keys.F2 )
      {
        if ( treeProject.SelectedNode.Level >= 1 )
        {
          treeProject.SelectedNode.BeginEdit();
        }
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
        if ( curElement.Type == ProjectElement.ElementType.FOLDER )
        {
          list.Insert( 0, curElement.Name );
        }
      }
      return list;
    }



    private void AdjustElementHierarchy( ProjectElement DraggedElement, TreeNode DraggedNode )
    {
      DraggedElement.ProjectHierarchy = GetElementHierarchy( DraggedNode );
      foreach ( TreeNode node in DraggedNode.Nodes )
      {
        AdjustElementHierarchy( ElementFromNode( node ), node );
      }
    }



    private void treeProject_DragDrop( object sender, System.Windows.Forms.DragEventArgs e )
    {
      if ( ( e.Data.GetDataPresent( "System.Windows.Forms.TreeNode", false ) )
      &&   ( NodeMap != "" ) )
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
            draggedProject.Elements.AddLast( draggedElement );
          }
          else
          {
            LinkedListNode<ProjectElement> nodeTarget = draggedProject.Elements.Find( targetElement );
            draggedProject.Elements.AddAfter( nodeTarget, draggedElement );
          }
          draggedProject.SetModified();

        }
      }
      timerDragDrop.Stop();

      /*
      // Retrieve the client coordinates of the drop location.
      Point targetPoint = treeProject.PointToClient( new Point( e.X, e.Y ) );

      // Retrieve the node at the drop location.
      TreeNode targetNode = treeProject.GetNodeAt( targetPoint );

      // Retrieve the node that was dragged.
      TreeNode draggedNode = (TreeNode)e.Data.GetData( typeof( TreeNode ) );

      // Confirm that the node at the drop location is not 
      // the dragged node or a descendant of the dragged node.
      if ( ( !draggedNode.Equals( targetNode ) )
      &&   ( !ContainsNode( draggedNode, targetNode ) ) )
      {
        // If it is a move operation, remove the node from its current 
        // location and add it to the node at the drop location.
        if ( e.Effect == DragDropEffects.Move )
        {
          Project draggedProject = ProjectFromNode( draggedNode );
          Project projectUnderMouse = ProjectFromNode( targetNode );

          if ( draggedProject != projectUnderMouse )
          {
            e.Effect = DragDropEffects.None;
            return;
          }

          ProjectElement targetElement = ElementFromNode( targetNode );
          if ( ( targetElement != null )
          &&   ( targetElement.Type != ProjectElement.ElementType.PROJECT )
          &&   ( targetElement.Type != ProjectElement.ElementType.FOLDER ) )
          {
            // use parent of element
            TreeNode curNode = targetElement.Node;
            while ( curNode.Parent != null )
            {
              curNode = curNode.Parent;
              targetElement = ElementFromNode( curNode );
              if ( ( targetElement == null )
              ||   ( targetElement.Type == ProjectElement.ElementType.PROJECT )
              ||   ( targetElement.Type == ProjectElement.ElementType.FOLDER ) )
              {
                targetNode = curNode;
                break;
              }
            }
          }
          draggedNode.Remove();
          targetNode.Nodes.Add( draggedNode );

          // reset element hierarchy
          ProjectElement draggedElement = ElementFromNode( draggedNode );

          AdjustElementHierarchy( draggedElement, draggedNode );

          // reorder in element list
          draggedProject.Elements.Remove( draggedElement );
          if ( targetElement == null )
          {
            draggedProject.Elements.AddLast( draggedElement );
          }
          else
          {
            LinkedListNode<ProjectElement> nodeTarget = draggedProject.Elements.Find( targetElement );
            draggedProject.Elements.AddAfter( nodeTarget, draggedElement );
          }
          draggedProject.SetModified();
        }
        // Expand the node at the location 
        // to show the dropped node.
        targetNode.Expand();
      }*/
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
      return (Project)Node.Tag;
    }



    public ProjectElement ElementFromNode( TreeNode Node )
    {
      if ( Node.Level == 0 )
      {
        return null;
      }
      return (ProjectElement)Node.Tag;
    }



    bool CanNodeBeChildOf( TreeNode NodeParent, TreeNode NodeChild )
    {
      ProjectElement    element = ElementFromNode( NodeParent );
      if ( ( element == null )
      ||   ( element.Type == ProjectElement.ElementType.FOLDER ) )
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

      // A bit long, but to summarize, process the following code only if the nodeover is null
      // and either the nodeover is not the same thing as nodemoving UNLESSS nodeover happens
      // to be the last node in the branch (so we can allow drag & drop below a parent branch)
      if ( ( NodeOver != null )
      &&   ( ( NodeOver != NodeMoving )
      ||     ( ( NodeOver.Parent != null )
      &&       ( NodeOver.Index == ( NodeOver.Parent.Nodes.Count - 1 ) ) ) ) )
      {
        int OffsetY = treeProject.PointToClient( Cursor.Position ).Y - NodeOver.Bounds.Top;
        int NodeOverImageWidth = treeProject.ImageList.Images[NodeOver.ImageIndex].Size.Width + 8;
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
              NodeOver.Expand();
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
      int NodeOverImageWidth = treeProject.ImageList.Images[NodeOver.ImageIndex].Size.Width + 8;

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

          // hide drag image
          //DragHelper.ImageList_DragShowNolock( false );
          // scroll and refresh
          node.EnsureVisible();
          treeProject.Refresh();
          // show drag image
          //DragHelper.ImageList_DragShowNolock( true );

        }
      }
      // if mouse is near to the bottom, scroll down
      else if ( pt.Y > treeProject.Size.Height - 30 )
      {
        if ( node.NextVisibleNode != null )
        {
          node = node.NextVisibleNode;

          //DragHelper.ImageList_DragShowNolock( false );
          node.EnsureVisible();
          treeProject.Refresh();
          //DragHelper.ImageList_DragShowNolock( true );
        }
      } 

    }



  }
}
