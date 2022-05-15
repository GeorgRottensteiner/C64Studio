namespace RetroDevStudio
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SolutionExplorer));
      this.treeProject = new RetroDevStudio.NoDblClkTreeView();
      this.imageListExplorer = new System.Windows.Forms.ImageList(this.components);
      this.timerDragDrop = new System.Windows.Forms.Timer(this.components);
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.seBtnAddNewItem = new System.Windows.Forms.ToolStripSplitButton();
      this.projectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.seBtnAddNewFolder = new System.Windows.Forms.ToolStripMenuItem();
      this.seBtnAddNewASMFile = new System.Windows.Forms.ToolStripMenuItem();
      this.seBtnAddNewBASICFile = new System.Windows.Forms.ToolStripMenuItem();
      this.seBtnAddNewSpriteSet = new System.Windows.Forms.ToolStripMenuItem();
      this.seBtnAddNewCharacterSet = new System.Windows.Forms.ToolStripMenuItem();
      this.seBtnAddNewCharScreen = new System.Windows.Forms.ToolStripMenuItem();
      this.seBtnAddNewGraphicScreen = new System.Windows.Forms.ToolStripMenuItem();
      this.seBtnAddNewMap = new System.Windows.Forms.ToolStripMenuItem();
      this.seBtnAddExisting = new System.Windows.Forms.ToolStripButton();
      this.seBtnDelete = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.seBtnCloneSolution = new System.Windows.Forms.ToolStripButton();
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      this.toolStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // treeProject
      // 
      this.treeProject.AllowDrop = true;
      this.treeProject.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.treeProject.ImageIndex = 0;
      this.treeProject.ImageList = this.imageListExplorer;
      this.treeProject.LabelEdit = true;
      this.treeProject.Location = new System.Drawing.Point(0, 28);
      this.treeProject.Name = "treeProject";
      this.treeProject.SelectedImageIndex = 0;
      this.treeProject.Size = new System.Drawing.Size(534, 362);
      this.treeProject.TabIndex = 0;
      this.treeProject.Text = "NoDblClkTreeView";
      this.treeProject.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeProject_BeforeLabelEdit);
      this.treeProject.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeProject_AfterLabelEdit);
      this.treeProject.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.treeProject_AfterCollapse);
      this.treeProject.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeProject_AfterExpand);
      this.treeProject.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeProject_ItemDrag);
      this.treeProject.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeProject_AfterSelect);
      this.treeProject.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeProject_NodeMouseClick);
      this.treeProject.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeProject_NodeMouseDoubleClick);
      this.treeProject.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeProject_DragDrop);
      this.treeProject.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeProject_DragEnter);
      this.treeProject.DragOver += new System.Windows.Forms.DragEventHandler(this.treeProject_DragOver);
      this.treeProject.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeProject_KeyDown);
      // 
      // imageListExplorer
      // 
      this.imageListExplorer.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListExplorer.ImageStream")));
      this.imageListExplorer.TransparentColor = System.Drawing.Color.Transparent;
      this.imageListExplorer.Images.SetKeyName(0, "c64.ico");
      this.imageListExplorer.Images.SetKeyName(1, "source.ico");
      this.imageListExplorer.Images.SetKeyName(2, "spriteset.ico");
      this.imageListExplorer.Images.SetKeyName(3, "charset.ico");
      this.imageListExplorer.Images.SetKeyName(4, "source_basic.ico");
      this.imageListExplorer.Images.SetKeyName(5, "graphicscreen.ico");
      this.imageListExplorer.Images.SetKeyName(6, "charsetscreen.ico");
      this.imageListExplorer.Images.SetKeyName(7, "mapeditor.ico");
      this.imageListExplorer.Images.SetKeyName(8, "folder.ico");
      this.imageListExplorer.Images.SetKeyName(9, "solution.ico");
      this.imageListExplorer.Images.SetKeyName(10, "project.ico");
      this.imageListExplorer.Images.SetKeyName(11, "disassembler.ico");
      this.imageListExplorer.Images.SetKeyName(12, "binary.ico");
      this.imageListExplorer.Images.SetKeyName(13, "filemanager.ico");
      this.imageListExplorer.Images.SetKeyName(14, "valuetable.ico");
      // 
      // toolStrip1
      // 
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.seBtnAddNewItem,
            this.seBtnAddExisting,
            this.seBtnDelete,
            this.toolStripSeparator2,
            this.seBtnCloneSolution});
      this.toolStrip1.Location = new System.Drawing.Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(534, 25);
      this.toolStrip1.TabIndex = 1;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // seBtnAddNewItem
      // 
      this.seBtnAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.seBtnAddNewItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.projectToolStripMenuItem,
            this.toolStripSeparator1,
            this.seBtnAddNewFolder,
            this.seBtnAddNewASMFile,
            this.seBtnAddNewBASICFile,
            this.seBtnAddNewSpriteSet,
            this.seBtnAddNewCharacterSet,
            this.seBtnAddNewCharScreen,
            this.seBtnAddNewGraphicScreen,
            this.seBtnAddNewMap});
      this.seBtnAddNewItem.Enabled = false;
      this.seBtnAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("seBtnAddNewItem.Image")));
      this.seBtnAddNewItem.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.seBtnAddNewItem.Name = "seBtnAddNewItem";
      this.seBtnAddNewItem.Size = new System.Drawing.Size(32, 22);
      this.seBtnAddNewItem.Text = "Add New Item";
      // 
      // projectToolStripMenuItem
      // 
      this.projectToolStripMenuItem.Name = "projectToolStripMenuItem";
      this.projectToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
      this.projectToolStripMenuItem.Text = "Project";
      this.projectToolStripMenuItem.Click += new System.EventHandler(this.projectToolStripMenuItem_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(160, 6);
      // 
      // seBtnAddNewFolder
      // 
      this.seBtnAddNewFolder.Name = "seBtnAddNewFolder";
      this.seBtnAddNewFolder.Size = new System.Drawing.Size(163, 22);
      this.seBtnAddNewFolder.Text = "Folder";
      this.seBtnAddNewFolder.Click += new System.EventHandler(this.seBtnAddNewFolder_Click);
      // 
      // seBtnAddNewASMFile
      // 
      this.seBtnAddNewASMFile.Name = "seBtnAddNewASMFile";
      this.seBtnAddNewASMFile.Size = new System.Drawing.Size(163, 22);
      this.seBtnAddNewASMFile.Text = "ASM File";
      this.seBtnAddNewASMFile.Click += new System.EventHandler(this.seBtnAddNewASMFile_Click);
      // 
      // seBtnAddNewBASICFile
      // 
      this.seBtnAddNewBASICFile.Name = "seBtnAddNewBASICFile";
      this.seBtnAddNewBASICFile.Size = new System.Drawing.Size(163, 22);
      this.seBtnAddNewBASICFile.Text = "BASIC File";
      this.seBtnAddNewBASICFile.Click += new System.EventHandler(this.seBtnAddNewBASICFile_Click);
      // 
      // seBtnAddNewSpriteSet
      // 
      this.seBtnAddNewSpriteSet.Name = "seBtnAddNewSpriteSet";
      this.seBtnAddNewSpriteSet.Size = new System.Drawing.Size(163, 22);
      this.seBtnAddNewSpriteSet.Text = "Sprite Set";
      this.seBtnAddNewSpriteSet.Click += new System.EventHandler(this.seBtnAddNewSpriteSet_Click);
      // 
      // seBtnAddNewCharacterSet
      // 
      this.seBtnAddNewCharacterSet.Name = "seBtnAddNewCharacterSet";
      this.seBtnAddNewCharacterSet.Size = new System.Drawing.Size(163, 22);
      this.seBtnAddNewCharacterSet.Text = "Character Set";
      this.seBtnAddNewCharacterSet.Click += new System.EventHandler(this.seBtnAddNewCharacterSet_Click);
      // 
      // seBtnAddNewCharScreen
      // 
      this.seBtnAddNewCharScreen.Name = "seBtnAddNewCharScreen";
      this.seBtnAddNewCharScreen.Size = new System.Drawing.Size(163, 22);
      this.seBtnAddNewCharScreen.Text = "Character Screen";
      this.seBtnAddNewCharScreen.Click += new System.EventHandler(this.seBtnAddNewCharScreen_Click);
      // 
      // seBtnAddNewGraphicScreen
      // 
      this.seBtnAddNewGraphicScreen.Name = "seBtnAddNewGraphicScreen";
      this.seBtnAddNewGraphicScreen.Size = new System.Drawing.Size(163, 22);
      this.seBtnAddNewGraphicScreen.Text = "Graphic Screen";
      this.seBtnAddNewGraphicScreen.Click += new System.EventHandler(this.seBtnAddNewGraphicScreen_Click);
      // 
      // seBtnAddNewMap
      // 
      this.seBtnAddNewMap.Name = "seBtnAddNewMap";
      this.seBtnAddNewMap.Size = new System.Drawing.Size(163, 22);
      this.seBtnAddNewMap.Text = "Map";
      this.seBtnAddNewMap.Click += new System.EventHandler(this.seBtnAddNewMap_Click);
      // 
      // seBtnAddExisting
      // 
      this.seBtnAddExisting.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.seBtnAddExisting.Enabled = false;
      this.seBtnAddExisting.Image = ((System.Drawing.Image)(resources.GetObject("seBtnAddExisting.Image")));
      this.seBtnAddExisting.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.seBtnAddExisting.Name = "seBtnAddExisting";
      this.seBtnAddExisting.Size = new System.Drawing.Size(23, 22);
      this.seBtnAddExisting.Text = "Add Existing Item";
      this.seBtnAddExisting.Click += new System.EventHandler(this.seBtnAddExisting_Click);
      // 
      // seBtnDelete
      // 
      this.seBtnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.seBtnDelete.Enabled = false;
      this.seBtnDelete.Image = ((System.Drawing.Image)(resources.GetObject("seBtnDelete.Image")));
      this.seBtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.seBtnDelete.Name = "seBtnDelete";
      this.seBtnDelete.Size = new System.Drawing.Size(23, 22);
      this.seBtnDelete.Text = "Delete";
      this.seBtnDelete.Click += new System.EventHandler(this.seBtnDelete_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
      // 
      // seBtnCloneSolution
      // 
      this.seBtnCloneSolution.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.seBtnCloneSolution.Enabled = false;
      this.seBtnCloneSolution.Image = global::RetroDevStudio.Properties.Resources.clone;
      this.seBtnCloneSolution.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.seBtnCloneSolution.Name = "seBtnCloneSolution";
      this.seBtnCloneSolution.Size = new System.Drawing.Size(23, 22);
      this.seBtnCloneSolution.Text = "Clone Solution";
      this.seBtnCloneSolution.Click += new System.EventHandler(this.seBtnCloneSolution_Click);
      // 
      // SolutionExplorer
      // 
      this.ClientSize = new System.Drawing.Size(534, 390);
      this.Controls.Add(this.toolStrip1);
      this.Controls.Add(this.treeProject);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "SolutionExplorer";
      this.Text = "Solution Explorer";
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    public NoDblClkTreeView treeProject;
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
  }
}
