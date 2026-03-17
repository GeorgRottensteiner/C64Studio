namespace RetroDevStudio.Documents
{
  partial class SolutionExplorer
  {
    /// <summary>
    /// Erforderliche Designervariable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Verwendete Ressourcen bereinigen.
    /// </summary>
    /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
    protected override void Dispose( bool disposing )
    {
      if ( disposing && ( components != null ) )
      {
        components.Dispose();
      }
      base.Dispose( disposing );
    }

    #region Vom Komponenten-Designer generierter Code

    /// <summary>
    /// Erforderliche Methode für die Designerunterstützung.
    /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
    /// </summary>
    private void InitializeComponent()
    {
      components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SolutionExplorer));
      treeProject = new DecentForms.TreeView();
      imageListExplorer = new System.Windows.Forms.ImageList( components );
      timerDragDrop = new System.Windows.Forms.Timer( components );
      toolStrip1 = new System.Windows.Forms.ToolStrip();
      seBtnAddNewItem = new System.Windows.Forms.ToolStripSplitButton();
      projectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      seBtnAddNewFolder = new System.Windows.Forms.ToolStripMenuItem();
      seBtnAddNewASMFile = new System.Windows.Forms.ToolStripMenuItem();
      seBtnAddNewBASICFile = new System.Windows.Forms.ToolStripMenuItem();
      seBtnAddNewSpriteSet = new System.Windows.Forms.ToolStripMenuItem();
      seBtnAddNewCharacterSet = new System.Windows.Forms.ToolStripMenuItem();
      seBtnAddNewCharScreen = new System.Windows.Forms.ToolStripMenuItem();
      seBtnAddNewGraphicScreen = new System.Windows.Forms.ToolStripMenuItem();
      seBtnAddNewMap = new System.Windows.Forms.ToolStripMenuItem();
      seBtnAddExisting = new System.Windows.Forms.ToolStripButton();
      seBtnDelete = new System.Windows.Forms.ToolStripButton();
      toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      seBtnCloneSolution = new System.Windows.Forms.ToolStripButton();
      imageListSourceControlOverlay = new System.Windows.Forms.ImageList( components );
      seBtnAddNewTextFile = new System.Windows.Forms.ToolStripMenuItem();
      ( (System.ComponentModel.ISupportInitialize)m_FileWatcher ).BeginInit();
      toolStrip1.SuspendLayout();
      SuspendLayout();
      // 
      // treeProject
      // 
      treeProject.AllowDrag = true;
      treeProject.AllowDrop = true;
      treeProject.Anchor =    System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Bottom   |  System.Windows.Forms.AnchorStyles.Left   |  System.Windows.Forms.AnchorStyles.Right ;
      treeProject.BorderStyle = DecentForms.BorderStyle.FLAT;
      treeProject.ImageList = imageListExplorer;
      treeProject.LabelEdit = true;
      treeProject.Location = new System.Drawing.Point( 0, 28 );
      treeProject.Name = "treeProject";
      treeProject.ScrollAlwaysVisible = false;
      treeProject.SelectedNode = null;
      treeProject.SelectionMode = DecentForms.SelectionMode.NONE;
      treeProject.Size = new System.Drawing.Size( 534, 362 );
      treeProject.TabIndex = 0;
      treeProject.Text = "NoDblClkTreeView";
      treeProject.AfterCollapse +=  treeProject_AfterCollapse ;
      treeProject.AfterExpand +=  treeProject_AfterExpand ;
      treeProject.AfterSelect +=  treeProject_AfterSelect ;
      treeProject.NodeMouseClick +=  treeProject_NodeMouseClick ;
      treeProject.NodeMouseDoubleClick +=  treeProject_NodeMouseDoubleClick ;
      treeProject.BeforeLabelEdit +=  treeProject_BeforeLabelEdit ;
      treeProject.AfterLabelEdit +=  treeProject_AfterLabelEdit ;
      treeProject.DrawNode +=  treeProject_DrawNode ;
      treeProject.ItemDrag +=  treeProject_ItemDrag ;
      treeProject.DragDrop +=  treeProject_DragDrop ;
      treeProject.DragEnter +=  treeProject_DragEnter ;
      treeProject.DragOver +=  treeProject_DragOver ;
      treeProject.QueryContinueDrag +=  treeProject_QueryContinueDrag ;
      treeProject.KeyDown +=  treeProject_KeyDown ;
      treeProject.KeyUp +=  treeProject_KeyUp ;
      // 
      // imageListExplorer
      // 
      imageListExplorer.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
      imageListExplorer.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject( "imageListExplorer.ImageStream" );
      imageListExplorer.TransparentColor = System.Drawing.Color.Transparent;
      imageListExplorer.Images.SetKeyName( 0, "c64.ico" );
      imageListExplorer.Images.SetKeyName( 1, "source.ico" );
      imageListExplorer.Images.SetKeyName( 2, "spriteset.ico" );
      imageListExplorer.Images.SetKeyName( 3, "charset.ico" );
      imageListExplorer.Images.SetKeyName( 4, "source_basic.ico" );
      imageListExplorer.Images.SetKeyName( 5, "graphicscreen.ico" );
      imageListExplorer.Images.SetKeyName( 6, "charsetscreen.ico" );
      imageListExplorer.Images.SetKeyName( 7, "mapeditor.ico" );
      imageListExplorer.Images.SetKeyName( 8, "folder.ico" );
      imageListExplorer.Images.SetKeyName( 9, "solution.ico" );
      imageListExplorer.Images.SetKeyName( 10, "project.ico" );
      imageListExplorer.Images.SetKeyName( 11, "disassembler.ico" );
      imageListExplorer.Images.SetKeyName( 12, "binary.ico" );
      imageListExplorer.Images.SetKeyName( 13, "filemanager.ico" );
      imageListExplorer.Images.SetKeyName( 14, "valuetable.ico" );
      imageListExplorer.Images.SetKeyName( 15, "icon_textfile.ico" );
      // 
      // toolStrip1
      // 
      toolStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] { seBtnAddNewItem, seBtnAddExisting, seBtnDelete, toolStripSeparator2, seBtnCloneSolution } );
      toolStrip1.Location = new System.Drawing.Point( 0, 0 );
      toolStrip1.Name = "toolStrip1";
      toolStrip1.Size = new System.Drawing.Size( 534, 25 );
      toolStrip1.TabIndex = 1;
      toolStrip1.Text = "toolStrip1";
      // 
      // seBtnAddNewItem
      // 
      seBtnAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      seBtnAddNewItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] { projectToolStripMenuItem, toolStripSeparator1, seBtnAddNewFolder, seBtnAddNewASMFile, seBtnAddNewBASICFile, seBtnAddNewTextFile, seBtnAddNewSpriteSet, seBtnAddNewCharacterSet, seBtnAddNewCharScreen, seBtnAddNewGraphicScreen, seBtnAddNewMap } );
      seBtnAddNewItem.Enabled = false;
      seBtnAddNewItem.Image = (System.Drawing.Image)resources.GetObject( "seBtnAddNewItem.Image" );
      seBtnAddNewItem.ImageTransparentColor = System.Drawing.Color.Magenta;
      seBtnAddNewItem.Name = "seBtnAddNewItem";
      seBtnAddNewItem.Size = new System.Drawing.Size( 32, 22 );
      seBtnAddNewItem.Text = "Add New Item";
      // 
      // projectToolStripMenuItem
      // 
      projectToolStripMenuItem.Name = "projectToolStripMenuItem";
      projectToolStripMenuItem.Size = new System.Drawing.Size( 180, 22 );
      projectToolStripMenuItem.Text = "Project";
      projectToolStripMenuItem.Click +=  projectToolStripMenuItem_Click ;
      // 
      // toolStripSeparator1
      // 
      toolStripSeparator1.Name = "toolStripSeparator1";
      toolStripSeparator1.Size = new System.Drawing.Size( 177, 6 );
      // 
      // seBtnAddNewFolder
      // 
      seBtnAddNewFolder.Name = "seBtnAddNewFolder";
      seBtnAddNewFolder.Size = new System.Drawing.Size( 180, 22 );
      seBtnAddNewFolder.Text = "Folder";
      seBtnAddNewFolder.Click +=  seBtnAddNewFolder_Click ;
      // 
      // seBtnAddNewASMFile
      // 
      seBtnAddNewASMFile.Name = "seBtnAddNewASMFile";
      seBtnAddNewASMFile.Size = new System.Drawing.Size( 180, 22 );
      seBtnAddNewASMFile.Text = "ASM File";
      seBtnAddNewASMFile.Click +=  seBtnAddNewASMFile_Click ;
      // 
      // seBtnAddNewBASICFile
      // 
      seBtnAddNewBASICFile.Name = "seBtnAddNewBASICFile";
      seBtnAddNewBASICFile.Size = new System.Drawing.Size( 180, 22 );
      seBtnAddNewBASICFile.Text = "BASIC File";
      seBtnAddNewBASICFile.Click +=  seBtnAddNewBASICFile_Click ;
      // 
      // seBtnAddNewSpriteSet
      // 
      seBtnAddNewSpriteSet.Name = "seBtnAddNewSpriteSet";
      seBtnAddNewSpriteSet.Size = new System.Drawing.Size( 180, 22 );
      seBtnAddNewSpriteSet.Text = "Sprite Set";
      seBtnAddNewSpriteSet.Click +=  seBtnAddNewSpriteSet_Click ;
      // 
      // seBtnAddNewCharacterSet
      // 
      seBtnAddNewCharacterSet.Name = "seBtnAddNewCharacterSet";
      seBtnAddNewCharacterSet.Size = new System.Drawing.Size( 180, 22 );
      seBtnAddNewCharacterSet.Text = "Character Set";
      seBtnAddNewCharacterSet.Click +=  seBtnAddNewCharacterSet_Click ;
      // 
      // seBtnAddNewCharScreen
      // 
      seBtnAddNewCharScreen.Name = "seBtnAddNewCharScreen";
      seBtnAddNewCharScreen.Size = new System.Drawing.Size( 180, 22 );
      seBtnAddNewCharScreen.Text = "Character Screen";
      seBtnAddNewCharScreen.Click +=  seBtnAddNewCharScreen_Click ;
      // 
      // seBtnAddNewGraphicScreen
      // 
      seBtnAddNewGraphicScreen.Name = "seBtnAddNewGraphicScreen";
      seBtnAddNewGraphicScreen.Size = new System.Drawing.Size( 180, 22 );
      seBtnAddNewGraphicScreen.Text = "Graphic Screen";
      seBtnAddNewGraphicScreen.Click +=  seBtnAddNewGraphicScreen_Click ;
      // 
      // seBtnAddNewMap
      // 
      seBtnAddNewMap.Name = "seBtnAddNewMap";
      seBtnAddNewMap.Size = new System.Drawing.Size( 180, 22 );
      seBtnAddNewMap.Text = "Map";
      seBtnAddNewMap.Click +=  seBtnAddNewMap_Click ;
      // 
      // seBtnAddExisting
      // 
      seBtnAddExisting.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      seBtnAddExisting.Enabled = false;
      seBtnAddExisting.Image = (System.Drawing.Image)resources.GetObject( "seBtnAddExisting.Image" );
      seBtnAddExisting.ImageTransparentColor = System.Drawing.Color.Magenta;
      seBtnAddExisting.Name = "seBtnAddExisting";
      seBtnAddExisting.Size = new System.Drawing.Size( 23, 22 );
      seBtnAddExisting.Text = "Add Existing Item";
      seBtnAddExisting.Click +=  seBtnAddExisting_Click ;
      // 
      // seBtnDelete
      // 
      seBtnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      seBtnDelete.Enabled = false;
      seBtnDelete.Image = (System.Drawing.Image)resources.GetObject( "seBtnDelete.Image" );
      seBtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
      seBtnDelete.Name = "seBtnDelete";
      seBtnDelete.Size = new System.Drawing.Size( 23, 22 );
      seBtnDelete.Text = "Remove Item from Project";
      seBtnDelete.Click +=  seBtnDelete_Click ;
      // 
      // toolStripSeparator2
      // 
      toolStripSeparator2.Name = "toolStripSeparator2";
      toolStripSeparator2.Size = new System.Drawing.Size( 6, 25 );
      // 
      // seBtnCloneSolution
      // 
      seBtnCloneSolution.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      seBtnCloneSolution.Enabled = false;
      seBtnCloneSolution.Image = Properties.Resources.clone;
      seBtnCloneSolution.ImageTransparentColor = System.Drawing.Color.Magenta;
      seBtnCloneSolution.Name = "seBtnCloneSolution";
      seBtnCloneSolution.Size = new System.Drawing.Size( 23, 22 );
      seBtnCloneSolution.Text = "Clone Solution";
      seBtnCloneSolution.Click +=  seBtnCloneSolution_Click ;
      // 
      // imageListSourceControlOverlay
      // 
      imageListSourceControlOverlay.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
      imageListSourceControlOverlay.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject( "imageListSourceControlOverlay.ImageStream" );
      imageListSourceControlOverlay.TransparentColor = System.Drawing.Color.Transparent;
      imageListSourceControlOverlay.Images.SetKeyName( 0, "se_sc_new.ico" );
      imageListSourceControlOverlay.Images.SetKeyName( 1, "se_sc_uptodate.ico" );
      imageListSourceControlOverlay.Images.SetKeyName( 2, "se_sc_changes.ico" );
      imageListSourceControlOverlay.Images.SetKeyName( 3, "se_sc_ignore.ico" );
      imageListSourceControlOverlay.Images.SetKeyName( 4, "se_sc_conflict.ico" );
      // 
      // seBtnAddNewTextFile
      // 
      seBtnAddNewTextFile.Name = "seBtnAddNewTextFile";
      seBtnAddNewTextFile.Size = new System.Drawing.Size( 180, 22 );
      seBtnAddNewTextFile.Text = "Text File";
      seBtnAddNewTextFile.Click +=  seBtnAddNewTextFile_Click ;
      // 
      // SolutionExplorer
      // 
      ClientSize = new System.Drawing.Size( 534, 390 );
      Controls.Add( toolStrip1 );
      Controls.Add( treeProject );
      Icon = (System.Drawing.Icon)resources.GetObject( "$this.Icon" );
      Name = "SolutionExplorer";
      Text = "Solution Explorer";
      ( (System.ComponentModel.ISupportInitialize)m_FileWatcher ).EndInit();
      toolStrip1.ResumeLayout( false );
      toolStrip1.PerformLayout();
      ResumeLayout( false );
      PerformLayout();

    }

    #endregion

    public DecentForms.TreeView treeProject;
    private System.Windows.Forms.ImageList imageListExplorer;
    private System.Windows.Forms.Timer timerDragDrop;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripSplitButton seBtnAddNewItem;
    private System.Windows.Forms.ToolStripButton seBtnAddExisting;
    private System.Windows.Forms.ToolStripMenuItem seBtnAddNewFolder;
    private System.Windows.Forms.ToolStripMenuItem seBtnAddNewASMFile;
    private System.Windows.Forms.ToolStripMenuItem seBtnAddNewBASICFile;
    private System.Windows.Forms.ToolStripMenuItem seBtnAddNewSpriteSet;
    private System.Windows.Forms.ToolStripMenuItem seBtnAddNewCharacterSet;
    private System.Windows.Forms.ToolStripMenuItem seBtnAddNewCharScreen;
    private System.Windows.Forms.ToolStripMenuItem seBtnAddNewGraphicScreen;
    private System.Windows.Forms.ToolStripMenuItem seBtnAddNewMap;
    private System.Windows.Forms.ToolStripButton seBtnDelete;
    private System.Windows.Forms.ToolStripMenuItem projectToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripButton seBtnCloneSolution;
    private System.Windows.Forms.ImageList imageListSourceControlOverlay;
    private System.Windows.Forms.ToolStripMenuItem seBtnAddNewTextFile;
  }
}
