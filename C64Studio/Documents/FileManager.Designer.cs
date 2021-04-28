namespace C64Studio
{
  partial class FileManager
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileManager));
      this.menuFileManager = new System.Windows.Forms.MenuStrip();
      this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.createEmptyTapeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.newDiskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.d64ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.d64With40TracksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.d71ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.d81ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.validateMediumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
      this.importFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
      this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.listFiles = new C64Studio.Controls.MeasurableListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.statusMedia = new System.Windows.Forms.StatusStrip();
      this.statusFileManager = new System.Windows.Forms.ToolStripStatusLabel();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.btnAddNew = new System.Windows.Forms.ToolStripButton();
      this.toolStripBtnImportFile = new System.Windows.Forms.ToolStripButton();
      this.toolStripBtnSave = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripBtnMoveFileUp = new System.Windows.Forms.ToolStripButton();
      this.toolStripBtnMoveFileDown = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripBtnExportToFile = new System.Windows.Forms.ToolStripButton();
      this.toolStripBtnOpenHex = new System.Windows.Forms.ToolStripButton();
      this.toolStripBtnOpenBASIC = new System.Windows.Forms.ToolStripButton();
      this.labelMediaTitle = new System.Windows.Forms.ToolStripLabel();
      this.importDirArtFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      this.menuFileManager.SuspendLayout();
      this.statusMedia.SuspendLayout();
      this.toolStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // menuFileManager
      // 
      this.menuFileManager.ImageScalingSize = new System.Drawing.Size(28, 28);
      this.menuFileManager.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
      this.menuFileManager.Location = new System.Drawing.Point(0, 0);
      this.menuFileManager.Name = "menuFileManager";
      this.menuFileManager.Size = new System.Drawing.Size(677, 24);
      this.menuFileManager.TabIndex = 0;
      this.menuFileManager.Text = "menuStrip1";
      // 
      // toolStripMenuItem1
      // 
      this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveasToolStripMenuItem,
            this.toolStripSeparator1,
            this.createEmptyTapeToolStripMenuItem,
            this.newDiskToolStripMenuItem,
            this.toolStripSeparator2,
            this.validateMediumToolStripMenuItem,
            this.toolStripSeparator3,
            this.importFileToolStripMenuItem,
            this.importDirArtFilesToolStripMenuItem,
            this.toolStripSeparator4,
            this.closeToolStripMenuItem});
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new System.Drawing.Size(87, 20);
      this.toolStripMenuItem1.Text = "&File Manager";
      // 
      // openToolStripMenuItem
      // 
      this.openToolStripMenuItem.Name = "openToolStripMenuItem";
      this.openToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
      this.openToolStripMenuItem.Text = "&Open...";
      this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
      // 
      // saveToolStripMenuItem
      // 
      this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
      this.saveToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
      this.saveToolStripMenuItem.Text = "&Save...";
      this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
      // 
      // saveasToolStripMenuItem
      // 
      this.saveasToolStripMenuItem.Enabled = false;
      this.saveasToolStripMenuItem.Name = "saveasToolStripMenuItem";
      this.saveasToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
      this.saveasToolStripMenuItem.Text = "Save &as...";
      this.saveasToolStripMenuItem.Click += new System.EventHandler(this.saveasToolStripMenuItem_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(179, 6);
      // 
      // createEmptyTapeToolStripMenuItem
      // 
      this.createEmptyTapeToolStripMenuItem.Name = "createEmptyTapeToolStripMenuItem";
      this.createEmptyTapeToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
      this.createEmptyTapeToolStripMenuItem.Text = "Create empty &Tape";
      this.createEmptyTapeToolStripMenuItem.Click += new System.EventHandler(this.createEmptyTapeToolStripMenuItem_Click);
      // 
      // newDiskToolStripMenuItem
      // 
      this.newDiskToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.d64ToolStripMenuItem,
            this.d64With40TracksToolStripMenuItem,
            this.d71ToolStripMenuItem,
            this.d81ToolStripMenuItem});
      this.newDiskToolStripMenuItem.Name = "newDiskToolStripMenuItem";
      this.newDiskToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
      this.newDiskToolStripMenuItem.Text = "Create empty &Disk";
      // 
      // d64ToolStripMenuItem
      // 
      this.d64ToolStripMenuItem.Name = "d64ToolStripMenuItem";
      this.d64ToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
      this.d64ToolStripMenuItem.Text = "D64";
      this.d64ToolStripMenuItem.Click += new System.EventHandler(this.d64ToolStripMenuItem_Click);
      // 
      // d64With40TracksToolStripMenuItem
      // 
      this.d64With40TracksToolStripMenuItem.Name = "d64With40TracksToolStripMenuItem";
      this.d64With40TracksToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
      this.d64With40TracksToolStripMenuItem.Text = "D64 with 40 tracks";
      this.d64With40TracksToolStripMenuItem.Click += new System.EventHandler(this.d64With40TracksToolStripMenuItem_Click);
      // 
      // d71ToolStripMenuItem
      // 
      this.d71ToolStripMenuItem.Name = "d71ToolStripMenuItem";
      this.d71ToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
      this.d71ToolStripMenuItem.Text = "D71";
      this.d71ToolStripMenuItem.Click += new System.EventHandler(this.d71ToolStripMenuItem_Click);
      // 
      // d81ToolStripMenuItem
      // 
      this.d81ToolStripMenuItem.Name = "d81ToolStripMenuItem";
      this.d81ToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
      this.d81ToolStripMenuItem.Text = "D81";
      this.d81ToolStripMenuItem.Click += new System.EventHandler(this.d81ToolStripMenuItem_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(179, 6);
      // 
      // validateMediumToolStripMenuItem
      // 
      this.validateMediumToolStripMenuItem.Enabled = false;
      this.validateMediumToolStripMenuItem.Name = "validateMediumToolStripMenuItem";
      this.validateMediumToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
      this.validateMediumToolStripMenuItem.Text = "&Validate medium";
      this.validateMediumToolStripMenuItem.Click += new System.EventHandler(this.validateMediumToolStripMenuItem_Click);
      // 
      // toolStripSeparator3
      // 
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new System.Drawing.Size(179, 6);
      // 
      // importFileToolStripMenuItem
      // 
      this.importFileToolStripMenuItem.Name = "importFileToolStripMenuItem";
      this.importFileToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
      this.importFileToolStripMenuItem.Text = "Import File...";
      this.importFileToolStripMenuItem.Click += new System.EventHandler(this.importFileToolStripMenuItem_Click);
      // 
      // toolStripSeparator4
      // 
      this.toolStripSeparator4.Name = "toolStripSeparator4";
      this.toolStripSeparator4.Size = new System.Drawing.Size(179, 6);
      // 
      // closeToolStripMenuItem
      // 
      this.closeToolStripMenuItem.Enabled = false;
      this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
      this.closeToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
      this.closeToolStripMenuItem.Text = "&Close";
      // 
      // listFiles
      // 
      this.listFiles.AllowDrop = true;
      this.listFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.listFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
      this.listFiles.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.listFiles.FullRowSelect = true;
      this.listFiles.HideSelection = false;
      this.listFiles.ItemHeight = 16;
      this.listFiles.Location = new System.Drawing.Point(0, 52);
      this.listFiles.Name = "listFiles";
      this.listFiles.Size = new System.Drawing.Size(677, 358);
      this.listFiles.TabIndex = 1;
      this.listFiles.UseCompatibleStateImageBehavior = false;
      this.listFiles.View = System.Windows.Forms.View.Details;
      this.listFiles.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listFiles_ItemDrag);
      this.listFiles.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listFiles_ItemSelectionChanged);
      this.listFiles.SelectedIndexChanged += new System.EventHandler(this.listFiles_SelectedIndexChanged);
      this.listFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.listFiles_DragDrop);
      this.listFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.listFiles_DragEnter);
      this.listFiles.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listFiles_MouseClick);
      this.listFiles.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.listFiles_PreviewKeyDown);
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Filename";
      this.columnHeader1.Width = 307;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Blocks";
      this.columnHeader2.Width = 96;
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "Type";
      // 
      // columnHeader4
      // 
      this.columnHeader4.Text = "Track";
      this.columnHeader4.Width = 40;
      // 
      // columnHeader5
      // 
      this.columnHeader5.Text = "Sector";
      this.columnHeader5.Width = 49;
      // 
      // statusMedia
      // 
      this.statusMedia.ImageScalingSize = new System.Drawing.Size(28, 28);
      this.statusMedia.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusFileManager});
      this.statusMedia.Location = new System.Drawing.Point(0, 413);
      this.statusMedia.Name = "statusMedia";
      this.statusMedia.Size = new System.Drawing.Size(677, 22);
      this.statusMedia.TabIndex = 2;
      this.statusMedia.Text = "statusStrip1";
      // 
      // statusFileManager
      // 
      this.statusFileManager.AutoSize = false;
      this.statusFileManager.Name = "statusFileManager";
      this.statusFileManager.Size = new System.Drawing.Size(300, 17);
      this.statusFileManager.Text = "toolStripStatusLabel1";
      this.statusFileManager.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // toolStrip1
      // 
      this.toolStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.toolStrip1.AutoSize = false;
      this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddNew,
            this.toolStripBtnImportFile,
            this.toolStripBtnSave,
            this.toolStripSeparator6,
            this.toolStripBtnMoveFileUp,
            this.toolStripBtnMoveFileDown,
            this.toolStripSeparator5,
            this.toolStripBtnExportToFile,
            this.toolStripBtnOpenHex,
            this.toolStripBtnOpenBASIC,
            this.labelMediaTitle});
      this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
      this.toolStrip1.Location = new System.Drawing.Point(0, 24);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(631, 20);
      this.toolStrip1.TabIndex = 3;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // btnAddNew
      // 
      this.btnAddNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.btnAddNew.Image = ((System.Drawing.Image)(resources.GetObject("btnAddNew.Image")));
      this.btnAddNew.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.btnAddNew.Name = "btnAddNew";
      this.btnAddNew.Size = new System.Drawing.Size(23, 17);
      this.btnAddNew.Text = "Add New File";
      this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
      // 
      // toolStripBtnImportFile
      // 
      this.toolStripBtnImportFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripBtnImportFile.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnImportFile.Image")));
      this.toolStripBtnImportFile.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripBtnImportFile.Name = "toolStripBtnImportFile";
      this.toolStripBtnImportFile.Size = new System.Drawing.Size(23, 17);
      this.toolStripBtnImportFile.Text = "Import File";
      this.toolStripBtnImportFile.ToolTipText = "Import File";
      this.toolStripBtnImportFile.Click += new System.EventHandler(this.toolStripBtnImportFile_Click);
      // 
      // toolStripBtnSave
      // 
      this.toolStripBtnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripBtnSave.Image = global::C64Studio.Properties.Resources.ToolSave;
      this.toolStripBtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripBtnSave.Name = "toolStripBtnSave";
      this.toolStripBtnSave.Size = new System.Drawing.Size(23, 17);
      this.toolStripBtnSave.Text = "Save";
      this.toolStripBtnSave.Click += new System.EventHandler(this.toolStripBtnSave_Click);
      // 
      // toolStripSeparator6
      // 
      this.toolStripSeparator6.Name = "toolStripSeparator6";
      this.toolStripSeparator6.Size = new System.Drawing.Size(6, 20);
      // 
      // toolStripBtnMoveFileUp
      // 
      this.toolStripBtnMoveFileUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripBtnMoveFileUp.Enabled = false;
      this.toolStripBtnMoveFileUp.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnMoveFileUp.Image")));
      this.toolStripBtnMoveFileUp.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripBtnMoveFileUp.Name = "toolStripBtnMoveFileUp";
      this.toolStripBtnMoveFileUp.Size = new System.Drawing.Size(23, 17);
      this.toolStripBtnMoveFileUp.Text = "toolStripButton1";
      this.toolStripBtnMoveFileUp.ToolTipText = "Move File Up";
      this.toolStripBtnMoveFileUp.Click += new System.EventHandler(this.toolStripBtnMoveFileUp_Click);
      // 
      // toolStripBtnMoveFileDown
      // 
      this.toolStripBtnMoveFileDown.AutoSize = false;
      this.toolStripBtnMoveFileDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripBtnMoveFileDown.Enabled = false;
      this.toolStripBtnMoveFileDown.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnMoveFileDown.Image")));
      this.toolStripBtnMoveFileDown.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripBtnMoveFileDown.Name = "toolStripBtnMoveFileDown";
      this.toolStripBtnMoveFileDown.Size = new System.Drawing.Size(23, 32);
      this.toolStripBtnMoveFileDown.Text = "toolStripButton1";
      this.toolStripBtnMoveFileDown.ToolTipText = "Move File Down";
      this.toolStripBtnMoveFileDown.Click += new System.EventHandler(this.toolStripBtnMoveFileDown_Click);
      // 
      // toolStripSeparator5
      // 
      this.toolStripSeparator5.Name = "toolStripSeparator5";
      this.toolStripSeparator5.Size = new System.Drawing.Size(6, 20);
      // 
      // toolStripBtnExportToFile
      // 
      this.toolStripBtnExportToFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripBtnExportToFile.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnExportToFile.Image")));
      this.toolStripBtnExportToFile.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripBtnExportToFile.Name = "toolStripBtnExportToFile";
      this.toolStripBtnExportToFile.Size = new System.Drawing.Size(23, 17);
      this.toolStripBtnExportToFile.Text = "toolStripButton1";
      this.toolStripBtnExportToFile.ToolTipText = "Export to File";
      this.toolStripBtnExportToFile.Click += new System.EventHandler(this.toolStripBtnExportToFile_Click);
      // 
      // toolStripBtnOpenHex
      // 
      this.toolStripBtnOpenHex.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripBtnOpenHex.Enabled = false;
      this.toolStripBtnOpenHex.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnOpenHex.Image")));
      this.toolStripBtnOpenHex.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripBtnOpenHex.Name = "toolStripBtnOpenHex";
      this.toolStripBtnOpenHex.Size = new System.Drawing.Size(23, 17);
      this.toolStripBtnOpenHex.Text = "toolStripButton1";
      this.toolStripBtnOpenHex.ToolTipText = "Open in Hex Editor";
      this.toolStripBtnOpenHex.Click += new System.EventHandler(this.toolStripBtnOpenHex_Click);
      // 
      // toolStripBtnOpenBASIC
      // 
      this.toolStripBtnOpenBASIC.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripBtnOpenBASIC.Enabled = false;
      this.toolStripBtnOpenBASIC.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnOpenBASIC.Image")));
      this.toolStripBtnOpenBASIC.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripBtnOpenBASIC.Name = "toolStripBtnOpenBASIC";
      this.toolStripBtnOpenBASIC.Size = new System.Drawing.Size(23, 17);
      this.toolStripBtnOpenBASIC.Text = "toolStripButton1";
      this.toolStripBtnOpenBASIC.ToolTipText = "Open in BASIC editor";
      this.toolStripBtnOpenBASIC.Click += new System.EventHandler(this.toolStripBtnOpenBASIC_Click);
      // 
      // labelMediaTitle
      // 
      this.labelMediaTitle.AutoSize = false;
      this.labelMediaTitle.Name = "labelMediaTitle";
      this.labelMediaTitle.Size = new System.Drawing.Size(300, 22);
      this.labelMediaTitle.Text = "toolStripLabel1";
      this.labelMediaTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.labelMediaTitle.Click += new System.EventHandler(this.labelMediaTitle_Click);
      // 
      // importDirArtFilesToolStripMenuItem
      // 
      this.importDirArtFilesToolStripMenuItem.Name = "importDirArtFilesToolStripMenuItem";
      this.importDirArtFilesToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
      this.importDirArtFilesToolStripMenuItem.Text = "Import Dir Art Files...";
      this.importDirArtFilesToolStripMenuItem.Click += new System.EventHandler(this.importDirArtFilesToolStripMenuItem_Click);
      // 
      // FileManager
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.ClientSize = new System.Drawing.Size(677, 435);
      this.Controls.Add(this.toolStrip1);
      this.Controls.Add(this.statusMedia);
      this.Controls.Add(this.listFiles);
      this.Controls.Add(this.menuFileManager);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MainMenuStrip = this.menuFileManager;
      this.Name = "FileManager";
      this.Text = "File Manager";
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      this.menuFileManager.ResumeLayout(false);
      this.menuFileManager.PerformLayout();
      this.statusMedia.ResumeLayout(false);
      this.statusMedia.PerformLayout();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.MenuStrip menuFileManager;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
    private C64Studio.Controls.MeasurableListView listFiles;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
    private System.Windows.Forms.StatusStrip statusMedia;
    private System.Windows.Forms.ToolStripStatusLabel statusFileManager;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem createEmptyTapeToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem newDiskToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripMenuItem validateMediumToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton toolStripBtnMoveFileUp;
    private System.Windows.Forms.ToolStripButton toolStripBtnMoveFileDown;
    private System.Windows.Forms.ColumnHeader columnHeader4;
    private System.Windows.Forms.ColumnHeader columnHeader5;
    private System.Windows.Forms.ToolStripMenuItem importFileToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    private System.Windows.Forms.ToolStripMenuItem saveasToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
    private System.Windows.Forms.ToolStripLabel labelMediaTitle;
    private System.Windows.Forms.ToolStripMenuItem d64ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem d81ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem d64With40TracksToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    private System.Windows.Forms.ToolStripButton toolStripBtnOpenHex;
    private System.Windows.Forms.ToolStripButton toolStripBtnOpenBASIC;
    private System.Windows.Forms.ToolStripButton toolStripBtnExportToFile;
    private System.Windows.Forms.ToolStripButton toolStripBtnImportFile;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
    private System.Windows.Forms.ToolStripMenuItem d71ToolStripMenuItem;
    private System.Windows.Forms.ToolStripButton toolStripBtnSave;
    private System.Windows.Forms.ToolStripButton btnAddNew;
    private System.Windows.Forms.ToolStripMenuItem importDirArtFilesToolStripMenuItem;
  }
}
