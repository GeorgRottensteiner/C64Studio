namespace C64Studio
{
  partial class MainForm
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

    #region Vom Windows Form-Designer generierter Code

    /// <summary>
    /// Erforderliche Methode für die Designerunterstützung.
    /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      this.panelMain = new WeifenLuo.WinFormsUI.Docking.DockPanel();
      this.mainMenu = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.fileNewSolutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.fileNewProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.fileNewASMFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.fileNewBasicFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.fileNewSpriteFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.fileNewCharacterFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.fileNewCharacterScreenEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.fileNewGraphicScreenEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.fileNewMapEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.valueTableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.fileNewBinaryEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.mediaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.newTapeImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.newDiskImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.graphicImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.projectOpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.fileOpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
      this.projectOpenTapeDiskFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.editorOpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.fileCloseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparatorAboveMRU = new System.Windows.Forms.ToolStripSeparator();
      this.fileRecentlyOpenedFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparatorBelowMRU = new System.Windows.Forms.ToolStripSeparator();
      this.filePreferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.fileSetupWizardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
      this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.solutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.solutionAddNewProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.solutionAddExistingProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.solutionSaveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this.solutionCloseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.projectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.addNewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.projectAddNewASMFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.projectAddNewBASICFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.projectAddNewSpriteSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.projectAddNewCharacterSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.projectAddNewCharacterScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.projectAddNewGraphicScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.projectAddNewMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.closeProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
      this.importFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.selfParseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.showLineinfosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
      this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
      this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.findReplaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.buildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.compileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.buildToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this.rebuildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.buildandRunToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.debugToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this.preprocessedFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.debugConnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.debugDisconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.tToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.refreshRegistersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.debugBreakpointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.dumpLabelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.dumpHierarchyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.dumpDockStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.runTestsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.disassembleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.markErrorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.throwExceptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.disassembleToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.breakpointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.debugMemoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.debugRegistersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.debugWatchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
      this.binaryEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.charsetEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.charScreenEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.graphicScreenEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.mapEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.spriteEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.valueTableEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
      this.calculatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.compileResulttoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.disassemblerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.outlineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.outputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.petSCIITableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.searchReplaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.searchResultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.projectExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
      this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
      this.toolbarsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.menuWindowToolbarMain = new System.Windows.Forms.ToolStripMenuItem();
      this.menuWindowToolbarDebugger = new System.Windows.Forms.ToolStripMenuItem();
      this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
      this.licenseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this.mainTools = new System.Windows.Forms.ToolStrip();
      this.mainToolNewItem = new System.Windows.Forms.ToolStripSplitButton();
      this.solutionToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this.mainToolNewProject = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.mainToolNewASMFile = new System.Windows.Forms.ToolStripMenuItem();
      this.mainToolNewBasicFile = new System.Windows.Forms.ToolStripMenuItem();
      this.mainToolNewSpriteFile = new System.Windows.Forms.ToolStripMenuItem();
      this.mainToolNewCharsetFile = new System.Windows.Forms.ToolStripMenuItem();
      this.charsetScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.graphicScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.mapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this.emptyTapeT64ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.emptyDiskD64ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.mainToolOpenFile = new System.Windows.Forms.ToolStripButton();
      this.mainToolSave = new System.Windows.Forms.ToolStripButton();
      this.mainToolSaveAll = new System.Windows.Forms.ToolStripButton();
      this.mainToolCommentSelection = new System.Windows.Forms.ToolStripButton();
      this.mainToolUncommentSelection = new System.Windows.Forms.ToolStripButton();
      this.mainToolCompile = new System.Windows.Forms.ToolStripButton();
      this.mainToolBuild = new System.Windows.Forms.ToolStripButton();
      this.mainToolRebuild = new System.Windows.Forms.ToolStripButton();
      this.mainToolBuildAndRun = new System.Windows.Forms.ToolStripButton();
      this.mainToolDebug = new System.Windows.Forms.ToolStripButton();
      this.mainToolConfig = new System.Windows.Forms.ToolStripComboBox();
      this.mainToolToggleTrueDrive = new System.Windows.Forms.ToolStripButton();
      this.mainToolEmulator = new System.Windows.Forms.ToolStripComboBox();
      this.mainToolUndo = new System.Windows.Forms.ToolStripButton();
      this.mainToolRedo = new System.Windows.Forms.ToolStripButton();
      this.mainToolFind = new System.Windows.Forms.ToolStripButton();
      this.mainToolFindReplace = new System.Windows.Forms.ToolStripButton();
      this.mainToolPrint = new System.Windows.Forms.ToolStripButton();
      this.mainStatus = new System.Windows.Forms.StatusStrip();
      this.statusLabelInfo = new System.Windows.Forms.ToolStripStatusLabel();
      this.statusProgress = new System.Windows.Forms.ToolStripProgressBar();
      this.statusEditorDetails = new System.Windows.Forms.ToolStripStatusLabel();
      this.debugTools = new System.Windows.Forms.ToolStrip();
      this.mainDebugGo = new System.Windows.Forms.ToolStripButton();
      this.mainDebugBreak = new System.Windows.Forms.ToolStripButton();
      this.mainDebugStop = new System.Windows.Forms.ToolStripButton();
      this.mainDebugStepInto = new System.Windows.Forms.ToolStripButton();
      this.mainDebugStepOver = new System.Windows.Forms.ToolStripButton();
      this.mainDebugStepOut = new System.Windows.Forms.ToolStripButton();
      this.fileRecentlyOpenedProjectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.mainMenu.SuspendLayout();
      this.mainTools.SuspendLayout();
      this.mainStatus.SuspendLayout();
      this.debugTools.SuspendLayout();
      this.SuspendLayout();
      // 
      // panelMain
      // 
      this.panelMain.AllowDrop = true;
      this.panelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.panelMain.DockBackColor = System.Drawing.SystemColors.AppWorkspace;
      this.panelMain.Location = new System.Drawing.Point(0, 52);
      this.panelMain.Name = "panelMain";
      this.panelMain.Size = new System.Drawing.Size(1120, 600);
      this.panelMain.TabIndex = 0;
      // 
      // mainMenu
      // 
      this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.solutionToolStripMenuItem,
            this.projectToolStripMenuItem,
            this.editToolStripMenuItem,
            this.buildToolStripMenuItem,
            this.debugToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.windowToolStripMenuItem,
            this.aboutToolStripMenuItem});
      this.mainMenu.Location = new System.Drawing.Point(0, 0);
      this.mainMenu.Name = "mainMenu";
      this.mainMenu.Size = new System.Drawing.Size(1120, 24);
      this.mainMenu.TabIndex = 3;
      this.mainMenu.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.saveAllToolStripMenuItem,
            this.fileCloseToolStripMenuItem,
            this.toolStripSeparatorAboveMRU,
            this.fileRecentlyOpenedProjectsToolStripMenuItem,
            this.fileRecentlyOpenedFilesToolStripMenuItem,
            this.toolStripSeparatorBelowMRU,
            this.filePreferencesToolStripMenuItem,
            this.fileSetupWizardToolStripMenuItem,
            this.toolStripSeparator4,
            this.exitToolStripMenuItem});
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
      this.fileToolStripMenuItem.Text = "&File";
      // 
      // newToolStripMenuItem
      // 
      this.newToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileNewSolutionToolStripMenuItem,
            this.fileNewProjectToolStripMenuItem,
            this.toolStripSeparator2,
            this.fileNewASMFileToolStripMenuItem,
            this.fileNewBasicFileToolStripMenuItem,
            this.fileNewSpriteFileToolStripMenuItem,
            this.fileNewCharacterFileToolStripMenuItem,
            this.fileNewCharacterScreenEditorToolStripMenuItem,
            this.fileNewGraphicScreenEditorToolStripMenuItem,
            this.fileNewMapEditorToolStripMenuItem,
            this.valueTableToolStripMenuItem,
            this.fileNewBinaryEditorToolStripMenuItem,
            this.mediaToolStripMenuItem});
      this.newToolStripMenuItem.Image = global::C64Studio.Properties.Resources.ToolNewItem;
      this.newToolStripMenuItem.Name = "newToolStripMenuItem";
      this.newToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
      this.newToolStripMenuItem.Text = "&New";
      // 
      // fileNewSolutionToolStripMenuItem
      // 
      this.fileNewSolutionToolStripMenuItem.Name = "fileNewSolutionToolStripMenuItem";
      this.fileNewSolutionToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
      this.fileNewSolutionToolStripMenuItem.Text = "&Solution";
      this.fileNewSolutionToolStripMenuItem.Click += new System.EventHandler(this.fileNewSolutionToolStripMenuItem_Click);
      // 
      // fileNewProjectToolStripMenuItem
      // 
      this.fileNewProjectToolStripMenuItem.Name = "fileNewProjectToolStripMenuItem";
      this.fileNewProjectToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
      this.fileNewProjectToolStripMenuItem.Text = "&Project";
      this.fileNewProjectToolStripMenuItem.Click += new System.EventHandler(this.fileNewProjectToolStripMenuItem_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(160, 6);
      // 
      // fileNewASMFileToolStripMenuItem
      // 
      this.fileNewASMFileToolStripMenuItem.Name = "fileNewASMFileToolStripMenuItem";
      this.fileNewASMFileToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
      this.fileNewASMFileToolStripMenuItem.Text = "ASM File";
      this.fileNewASMFileToolStripMenuItem.Click += new System.EventHandler(this.fileNewASMFileToolStripMenuItem_Click);
      // 
      // fileNewBasicFileToolStripMenuItem
      // 
      this.fileNewBasicFileToolStripMenuItem.Name = "fileNewBasicFileToolStripMenuItem";
      this.fileNewBasicFileToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
      this.fileNewBasicFileToolStripMenuItem.Text = "BASIC File";
      this.fileNewBasicFileToolStripMenuItem.Click += new System.EventHandler(this.fileNewBasicFileToolStripMenuItem_Click);
      // 
      // fileNewSpriteFileToolStripMenuItem
      // 
      this.fileNewSpriteFileToolStripMenuItem.Name = "fileNewSpriteFileToolStripMenuItem";
      this.fileNewSpriteFileToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
      this.fileNewSpriteFileToolStripMenuItem.Text = "Sprite Set";
      this.fileNewSpriteFileToolStripMenuItem.Click += new System.EventHandler(this.fileNewSpriteFileToolStripMenuItem_Click);
      // 
      // fileNewCharacterFileToolStripMenuItem
      // 
      this.fileNewCharacterFileToolStripMenuItem.Name = "fileNewCharacterFileToolStripMenuItem";
      this.fileNewCharacterFileToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
      this.fileNewCharacterFileToolStripMenuItem.Text = "Character Set";
      this.fileNewCharacterFileToolStripMenuItem.Click += new System.EventHandler(this.fileNewCharacterFileToolStripMenuItem_Click);
      // 
      // fileNewCharacterScreenEditorToolStripMenuItem
      // 
      this.fileNewCharacterScreenEditorToolStripMenuItem.Name = "fileNewCharacterScreenEditorToolStripMenuItem";
      this.fileNewCharacterScreenEditorToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
      this.fileNewCharacterScreenEditorToolStripMenuItem.Text = "Character Screen";
      this.fileNewCharacterScreenEditorToolStripMenuItem.Click += new System.EventHandler(this.fileNewCharacterScreenEditorToolStripMenuItem_Click);
      // 
      // fileNewGraphicScreenEditorToolStripMenuItem
      // 
      this.fileNewGraphicScreenEditorToolStripMenuItem.Name = "fileNewGraphicScreenEditorToolStripMenuItem";
      this.fileNewGraphicScreenEditorToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
      this.fileNewGraphicScreenEditorToolStripMenuItem.Text = "Graphic Screen";
      this.fileNewGraphicScreenEditorToolStripMenuItem.Click += new System.EventHandler(this.fileNewScreenEditorToolStripMenuItem_Click);
      // 
      // fileNewMapEditorToolStripMenuItem
      // 
      this.fileNewMapEditorToolStripMenuItem.Name = "fileNewMapEditorToolStripMenuItem";
      this.fileNewMapEditorToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
      this.fileNewMapEditorToolStripMenuItem.Text = "Map";
      this.fileNewMapEditorToolStripMenuItem.Click += new System.EventHandler(this.fileNewMapEditorToolStripMenuItem_Click);
      // 
      // valueTableToolStripMenuItem
      // 
      this.valueTableToolStripMenuItem.Name = "valueTableToolStripMenuItem";
      this.valueTableToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
      this.valueTableToolStripMenuItem.Text = "Value Table";
      this.valueTableToolStripMenuItem.Click += new System.EventHandler(this.valueTableToolStripMenuItem_Click);
      // 
      // fileNewBinaryEditorToolStripMenuItem
      // 
      this.fileNewBinaryEditorToolStripMenuItem.Name = "fileNewBinaryEditorToolStripMenuItem";
      this.fileNewBinaryEditorToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
      this.fileNewBinaryEditorToolStripMenuItem.Text = "Binary Editor";
      this.fileNewBinaryEditorToolStripMenuItem.Click += new System.EventHandler(this.fileNewBinaryEditorToolStripMenuItem_Click);
      // 
      // mediaToolStripMenuItem
      // 
      this.mediaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newTapeImageToolStripMenuItem,
            this.newDiskImageToolStripMenuItem,
            this.graphicImageToolStripMenuItem});
      this.mediaToolStripMenuItem.Name = "mediaToolStripMenuItem";
      this.mediaToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
      this.mediaToolStripMenuItem.Text = "Media";
      // 
      // newTapeImageToolStripMenuItem
      // 
      this.newTapeImageToolStripMenuItem.Name = "newTapeImageToolStripMenuItem";
      this.newTapeImageToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
      this.newTapeImageToolStripMenuItem.Text = "Tape Image";
      this.newTapeImageToolStripMenuItem.Click += new System.EventHandler(this.newTapeImageToolStripMenuItem_Click);
      // 
      // newDiskImageToolStripMenuItem
      // 
      this.newDiskImageToolStripMenuItem.Name = "newDiskImageToolStripMenuItem";
      this.newDiskImageToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
      this.newDiskImageToolStripMenuItem.Text = "Disk Image";
      this.newDiskImageToolStripMenuItem.Click += new System.EventHandler(this.newDiskImageToolStripMenuItem_Click);
      // 
      // graphicImageToolStripMenuItem
      // 
      this.graphicImageToolStripMenuItem.Name = "graphicImageToolStripMenuItem";
      this.graphicImageToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
      this.graphicImageToolStripMenuItem.Text = "Screen Editor";
      // 
      // openToolStripMenuItem
      // 
      this.openToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.projectOpenToolStripMenuItem,
            this.fileOpenToolStripMenuItem,
            this.toolStripSeparator6,
            this.projectOpenTapeDiskFileMenuItem,
            this.editorOpenToolStripMenuItem});
      this.openToolStripMenuItem.Image = global::C64Studio.Properties.Resources.ToolOpenFile;
      this.openToolStripMenuItem.Name = "openToolStripMenuItem";
      this.openToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
      this.openToolStripMenuItem.Text = "&Open";
      // 
      // projectOpenToolStripMenuItem
      // 
      this.projectOpenToolStripMenuItem.Name = "projectOpenToolStripMenuItem";
      this.projectOpenToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
      this.projectOpenToolStripMenuItem.Text = "&Solution or Project...";
      this.projectOpenToolStripMenuItem.Click += new System.EventHandler(this.projectOpenToolStripMenuItem_Click);
      // 
      // fileOpenToolStripMenuItem
      // 
      this.fileOpenToolStripMenuItem.Name = "fileOpenToolStripMenuItem";
      this.fileOpenToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
      this.fileOpenToolStripMenuItem.Text = "File...";
      this.fileOpenToolStripMenuItem.Click += new System.EventHandler(this.fileOpenToolStripMenuItem_Click);
      // 
      // toolStripSeparator6
      // 
      this.toolStripSeparator6.Name = "toolStripSeparator6";
      this.toolStripSeparator6.Size = new System.Drawing.Size(178, 6);
      // 
      // projectOpenTapeDiskFileMenuItem
      // 
      this.projectOpenTapeDiskFileMenuItem.Name = "projectOpenTapeDiskFileMenuItem";
      this.projectOpenTapeDiskFileMenuItem.Size = new System.Drawing.Size(181, 22);
      this.projectOpenTapeDiskFileMenuItem.Text = "Tape/Disk &File...";
      this.projectOpenTapeDiskFileMenuItem.Click += new System.EventHandler(this.projectOpenTapeDiskFileMenuItem_Click);
      // 
      // editorOpenToolStripMenuItem
      // 
      this.editorOpenToolStripMenuItem.Name = "editorOpenToolStripMenuItem";
      this.editorOpenToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
      this.editorOpenToolStripMenuItem.Text = "Editor File...";
      this.editorOpenToolStripMenuItem.Click += new System.EventHandler(this.editorOpenToolStripMenuItem_Click);
      // 
      // saveToolStripMenuItem
      // 
      this.saveToolStripMenuItem.Enabled = false;
      this.saveToolStripMenuItem.Image = global::C64Studio.Properties.Resources.ToolSave;
      this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
      this.saveToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
      this.saveToolStripMenuItem.Text = "&Save";
      this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
      // 
      // saveAsToolStripMenuItem
      // 
      this.saveAsToolStripMenuItem.Enabled = false;
      this.saveAsToolStripMenuItem.Image = global::C64Studio.Properties.Resources.ToolSaveAll;
      this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
      this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
      this.saveAsToolStripMenuItem.Text = "Save &as...";
      this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
      // 
      // saveAllToolStripMenuItem
      // 
      this.saveAllToolStripMenuItem.Enabled = false;
      this.saveAllToolStripMenuItem.Name = "saveAllToolStripMenuItem";
      this.saveAllToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
      this.saveAllToolStripMenuItem.Text = "Save all";
      this.saveAllToolStripMenuItem.Click += new System.EventHandler(this.saveAllToolStripMenuItem_Click);
      // 
      // fileCloseToolStripMenuItem
      // 
      this.fileCloseToolStripMenuItem.Enabled = false;
      this.fileCloseToolStripMenuItem.Name = "fileCloseToolStripMenuItem";
      this.fileCloseToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
      this.fileCloseToolStripMenuItem.Text = "&Close";
      this.fileCloseToolStripMenuItem.Click += new System.EventHandler(this.fileCloseToolStripMenuItem_Click);
      // 
      // toolStripSeparatorAboveMRU
      // 
      this.toolStripSeparatorAboveMRU.Name = "toolStripSeparatorAboveMRU";
      this.toolStripSeparatorAboveMRU.Size = new System.Drawing.Size(204, 6);
      // 
      // fileRecentlyOpenedFilesToolStripMenuItem
      // 
      this.fileRecentlyOpenedFilesToolStripMenuItem.Name = "fileRecentlyOpenedFilesToolStripMenuItem";
      this.fileRecentlyOpenedFilesToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
      this.fileRecentlyOpenedFilesToolStripMenuItem.Text = "Recently opened files";
      // 
      // toolStripSeparatorBelowMRU
      // 
      this.toolStripSeparatorBelowMRU.Name = "toolStripSeparatorBelowMRU";
      this.toolStripSeparatorBelowMRU.Size = new System.Drawing.Size(204, 6);
      // 
      // filePreferencesToolStripMenuItem
      // 
      this.filePreferencesToolStripMenuItem.Name = "filePreferencesToolStripMenuItem";
      this.filePreferencesToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
      this.filePreferencesToolStripMenuItem.Text = "&Preferences...";
      this.filePreferencesToolStripMenuItem.Click += new System.EventHandler(this.filePreferencesToolStripMenuItem_Click);
      // 
      // fileSetupWizardToolStripMenuItem
      // 
      this.fileSetupWizardToolStripMenuItem.Name = "fileSetupWizardToolStripMenuItem";
      this.fileSetupWizardToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
      this.fileSetupWizardToolStripMenuItem.Text = "Setup Wizard...";
      this.fileSetupWizardToolStripMenuItem.Click += new System.EventHandler(this.fileSetupWizardToolStripMenuItem_Click);
      // 
      // toolStripSeparator4
      // 
      this.toolStripSeparator4.Name = "toolStripSeparator4";
      this.toolStripSeparator4.Size = new System.Drawing.Size(204, 6);
      // 
      // exitToolStripMenuItem
      // 
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
      this.exitToolStripMenuItem.Text = "E&xit";
      this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
      // 
      // solutionToolStripMenuItem
      // 
      this.solutionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.solutionAddNewProjectToolStripMenuItem,
            this.solutionAddExistingProjectToolStripMenuItem,
            this.solutionSaveToolStripMenuItem1,
            this.solutionCloseToolStripMenuItem});
      this.solutionToolStripMenuItem.Name = "solutionToolStripMenuItem";
      this.solutionToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
      this.solutionToolStripMenuItem.Text = "&Solution";
      // 
      // solutionAddNewProjectToolStripMenuItem
      // 
      this.solutionAddNewProjectToolStripMenuItem.Name = "solutionAddNewProjectToolStripMenuItem";
      this.solutionAddNewProjectToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
      this.solutionAddNewProjectToolStripMenuItem.Text = "Add &new project...";
      this.solutionAddNewProjectToolStripMenuItem.Click += new System.EventHandler(this.solutionAddNewProjectToolStripMenuItem_Click);
      // 
      // solutionAddExistingProjectToolStripMenuItem
      // 
      this.solutionAddExistingProjectToolStripMenuItem.Name = "solutionAddExistingProjectToolStripMenuItem";
      this.solutionAddExistingProjectToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
      this.solutionAddExistingProjectToolStripMenuItem.Text = "&Add existing project...";
      this.solutionAddExistingProjectToolStripMenuItem.Click += new System.EventHandler(this.solutionAddExistingProjectToolStripMenuItem_Click);
      // 
      // solutionSaveToolStripMenuItem1
      // 
      this.solutionSaveToolStripMenuItem1.Enabled = false;
      this.solutionSaveToolStripMenuItem1.Name = "solutionSaveToolStripMenuItem1";
      this.solutionSaveToolStripMenuItem1.Size = new System.Drawing.Size(188, 22);
      this.solutionSaveToolStripMenuItem1.Text = "&Save";
      this.solutionSaveToolStripMenuItem1.Click += new System.EventHandler(this.solutionSaveToolStripMenuItem1_Click);
      // 
      // solutionCloseToolStripMenuItem
      // 
      this.solutionCloseToolStripMenuItem.Enabled = false;
      this.solutionCloseToolStripMenuItem.Name = "solutionCloseToolStripMenuItem";
      this.solutionCloseToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
      this.solutionCloseToolStripMenuItem.Text = "&Close";
      this.solutionCloseToolStripMenuItem.Click += new System.EventHandler(this.solutionCloseToolStripMenuItem_Click);
      // 
      // projectToolStripMenuItem
      // 
      this.projectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewToolStripMenuItem,
            this.saveProjectToolStripMenuItem,
            this.closeProjectToolStripMenuItem,
            this.toolStripSeparator3,
            this.importFileToolStripMenuItem,
            this.selfParseToolStripMenuItem,
            this.showLineinfosToolStripMenuItem,
            this.propertiesToolStripMenuItem});
      this.projectToolStripMenuItem.Name = "projectToolStripMenuItem";
      this.projectToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
      this.projectToolStripMenuItem.Text = "&Project";
      // 
      // addNewToolStripMenuItem
      // 
      this.addNewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.projectAddNewASMFileToolStripMenuItem,
            this.projectAddNewBASICFileToolStripMenuItem,
            this.projectAddNewSpriteSetToolStripMenuItem,
            this.projectAddNewCharacterSetToolStripMenuItem,
            this.projectAddNewCharacterScreenToolStripMenuItem,
            this.projectAddNewGraphicScreenToolStripMenuItem,
            this.projectAddNewMapToolStripMenuItem});
      this.addNewToolStripMenuItem.Name = "addNewToolStripMenuItem";
      this.addNewToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
      this.addNewToolStripMenuItem.Text = "Add New";
      // 
      // projectAddNewASMFileToolStripMenuItem
      // 
      this.projectAddNewASMFileToolStripMenuItem.Name = "projectAddNewASMFileToolStripMenuItem";
      this.projectAddNewASMFileToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
      this.projectAddNewASMFileToolStripMenuItem.Text = "ASM File";
      this.projectAddNewASMFileToolStripMenuItem.Click += new System.EventHandler(this.projectAddNewASMFileToolStripMenuItem_Click);
      // 
      // projectAddNewBASICFileToolStripMenuItem
      // 
      this.projectAddNewBASICFileToolStripMenuItem.Name = "projectAddNewBASICFileToolStripMenuItem";
      this.projectAddNewBASICFileToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
      this.projectAddNewBASICFileToolStripMenuItem.Text = "BASIC File";
      this.projectAddNewBASICFileToolStripMenuItem.Click += new System.EventHandler(this.projectAddNewBASICFileToolStripMenuItem_Click);
      // 
      // projectAddNewSpriteSetToolStripMenuItem
      // 
      this.projectAddNewSpriteSetToolStripMenuItem.Name = "projectAddNewSpriteSetToolStripMenuItem";
      this.projectAddNewSpriteSetToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
      this.projectAddNewSpriteSetToolStripMenuItem.Text = "Sprite Set";
      this.projectAddNewSpriteSetToolStripMenuItem.Click += new System.EventHandler(this.projectAddNewSpriteSetToolStripMenuItem_Click);
      // 
      // projectAddNewCharacterSetToolStripMenuItem
      // 
      this.projectAddNewCharacterSetToolStripMenuItem.Name = "projectAddNewCharacterSetToolStripMenuItem";
      this.projectAddNewCharacterSetToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
      this.projectAddNewCharacterSetToolStripMenuItem.Text = "Character Set";
      this.projectAddNewCharacterSetToolStripMenuItem.Click += new System.EventHandler(this.projectAddNewCharacterSetToolStripMenuItem_Click);
      // 
      // projectAddNewCharacterScreenToolStripMenuItem
      // 
      this.projectAddNewCharacterScreenToolStripMenuItem.Name = "projectAddNewCharacterScreenToolStripMenuItem";
      this.projectAddNewCharacterScreenToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
      this.projectAddNewCharacterScreenToolStripMenuItem.Text = "Character Screen";
      this.projectAddNewCharacterScreenToolStripMenuItem.Click += new System.EventHandler(this.projectAddNewCharacterScreenToolStripMenuItem_Click);
      // 
      // projectAddNewGraphicScreenToolStripMenuItem
      // 
      this.projectAddNewGraphicScreenToolStripMenuItem.Name = "projectAddNewGraphicScreenToolStripMenuItem";
      this.projectAddNewGraphicScreenToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
      this.projectAddNewGraphicScreenToolStripMenuItem.Text = "Graphic Screen";
      this.projectAddNewGraphicScreenToolStripMenuItem.Click += new System.EventHandler(this.projectAddNewGraphicScreenToolStripMenuItem_Click);
      // 
      // projectAddNewMapToolStripMenuItem
      // 
      this.projectAddNewMapToolStripMenuItem.Name = "projectAddNewMapToolStripMenuItem";
      this.projectAddNewMapToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
      this.projectAddNewMapToolStripMenuItem.Text = "Map";
      this.projectAddNewMapToolStripMenuItem.Click += new System.EventHandler(this.projectAddNewMapToolStripMenuItem_Click);
      // 
      // saveProjectToolStripMenuItem
      // 
      this.saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
      this.saveProjectToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
      this.saveProjectToolStripMenuItem.Text = "&Save";
      this.saveProjectToolStripMenuItem.Click += new System.EventHandler(this.saveProjectToolStripMenuItem_Click);
      // 
      // closeProjectToolStripMenuItem
      // 
      this.closeProjectToolStripMenuItem.Name = "closeProjectToolStripMenuItem";
      this.closeProjectToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
      this.closeProjectToolStripMenuItem.Text = "&Close";
      this.closeProjectToolStripMenuItem.Click += new System.EventHandler(this.closeProjectToolStripMenuItem_Click);
      // 
      // toolStripSeparator3
      // 
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new System.Drawing.Size(151, 6);
      // 
      // importFileToolStripMenuItem
      // 
      this.importFileToolStripMenuItem.Name = "importFileToolStripMenuItem";
      this.importFileToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
      this.importFileToolStripMenuItem.Text = "Import File...";
      this.importFileToolStripMenuItem.Click += new System.EventHandler(this.importFileToolStripMenuItem_Click);
      // 
      // selfParseToolStripMenuItem
      // 
      this.selfParseToolStripMenuItem.Name = "selfParseToolStripMenuItem";
      this.selfParseToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
      this.selfParseToolStripMenuItem.Text = "SelfParse";
      this.selfParseToolStripMenuItem.Visible = false;
      this.selfParseToolStripMenuItem.Click += new System.EventHandler(this.selfParseToolStripMenuItem_Click);
      // 
      // showLineinfosToolStripMenuItem
      // 
      this.showLineinfosToolStripMenuItem.Name = "showLineinfosToolStripMenuItem";
      this.showLineinfosToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
      this.showLineinfosToolStripMenuItem.Text = "Show Lineinfos";
      this.showLineinfosToolStripMenuItem.Visible = false;
      this.showLineinfosToolStripMenuItem.Click += new System.EventHandler(this.showLineinfosToolStripMenuItem_Click);
      // 
      // propertiesToolStripMenuItem
      // 
      this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
      this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
      this.propertiesToolStripMenuItem.Text = "&Properties";
      this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
      // 
      // editToolStripMenuItem
      // 
      this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator7,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.toolStripSeparator10,
            this.searchToolStripMenuItem,
            this.findReplaceToolStripMenuItem});
      this.editToolStripMenuItem.Name = "editToolStripMenuItem";
      this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
      this.editToolStripMenuItem.Text = "&Edit";
      this.editToolStripMenuItem.DropDownOpening += new System.EventHandler(this.editToolStripMenuItem_DropDownOpening);
      // 
      // undoToolStripMenuItem
      // 
      this.undoToolStripMenuItem.Image = global::C64Studio.Properties.Resources.ToolUndo;
      this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
      this.undoToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
      this.undoToolStripMenuItem.Text = "&Undo";
      this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
      // 
      // redoToolStripMenuItem
      // 
      this.redoToolStripMenuItem.Image = global::C64Studio.Properties.Resources.ToolRedo;
      this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
      this.redoToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
      this.redoToolStripMenuItem.Text = "&Redo";
      this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
      // 
      // toolStripSeparator7
      // 
      this.toolStripSeparator7.Name = "toolStripSeparator7";
      this.toolStripSeparator7.Size = new System.Drawing.Size(140, 6);
      // 
      // cutToolStripMenuItem
      // 
      this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
      this.cutToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
      this.cutToolStripMenuItem.Text = "Cut";
      this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
      // 
      // copyToolStripMenuItem
      // 
      this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
      this.copyToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
      this.copyToolStripMenuItem.Text = "Copy";
      this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
      // 
      // pasteToolStripMenuItem
      // 
      this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
      this.pasteToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
      this.pasteToolStripMenuItem.Text = "Paste";
      this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
      // 
      // deleteToolStripMenuItem
      // 
      this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
      this.deleteToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
      this.deleteToolStripMenuItem.Text = "Delete";
      this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
      // 
      // toolStripSeparator10
      // 
      this.toolStripSeparator10.Name = "toolStripSeparator10";
      this.toolStripSeparator10.Size = new System.Drawing.Size(140, 6);
      // 
      // searchToolStripMenuItem
      // 
      this.searchToolStripMenuItem.Image = global::C64Studio.Properties.Resources.ToolFind;
      this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
      this.searchToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
      this.searchToolStripMenuItem.Text = "&Search...";
      this.searchToolStripMenuItem.Click += new System.EventHandler(this.searchToolStripMenuItem_Click);
      // 
      // findReplaceToolStripMenuItem
      // 
      this.findReplaceToolStripMenuItem.Image = global::C64Studio.Properties.Resources.ToolFindReplace;
      this.findReplaceToolStripMenuItem.Name = "findReplaceToolStripMenuItem";
      this.findReplaceToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
      this.findReplaceToolStripMenuItem.Text = "&Find/Replace";
      this.findReplaceToolStripMenuItem.Click += new System.EventHandler(this.findReplaceToolStripMenuItem_Click);
      // 
      // buildToolStripMenuItem
      // 
      this.buildToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compileToolStripMenuItem,
            this.buildToolStripMenuItem1,
            this.rebuildToolStripMenuItem,
            this.buildandRunToolStripMenuItem,
            this.debugToolStripMenuItem1,
            this.preprocessedFileToolStripMenuItem});
      this.buildToolStripMenuItem.Name = "buildToolStripMenuItem";
      this.buildToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
      this.buildToolStripMenuItem.Text = "&Build";
      // 
      // compileToolStripMenuItem
      // 
      this.compileToolStripMenuItem.Image = global::C64Studio.Properties.Resources.ToolCompile;
      this.compileToolStripMenuItem.Name = "compileToolStripMenuItem";
      this.compileToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
      this.compileToolStripMenuItem.Tag = "";
      this.compileToolStripMenuItem.Text = "&Compile";
      this.compileToolStripMenuItem.Click += new System.EventHandler(this.compileToolStripMenuItem_Click);
      // 
      // buildToolStripMenuItem1
      // 
      this.buildToolStripMenuItem1.Image = global::C64Studio.Properties.Resources.ToolBuild;
      this.buildToolStripMenuItem1.Name = "buildToolStripMenuItem1";
      this.buildToolStripMenuItem1.Size = new System.Drawing.Size(165, 22);
      this.buildToolStripMenuItem1.Text = "&Build";
      this.buildToolStripMenuItem1.Click += new System.EventHandler(this.buildToolStripMenuItem1_Click);
      // 
      // rebuildToolStripMenuItem
      // 
      this.rebuildToolStripMenuItem.Image = global::C64Studio.Properties.Resources.ToolRebuild;
      this.rebuildToolStripMenuItem.Name = "rebuildToolStripMenuItem";
      this.rebuildToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
      this.rebuildToolStripMenuItem.Text = "&Rebuild";
      this.rebuildToolStripMenuItem.Click += new System.EventHandler(this.rebuildToolStripMenuItem_Click);
      // 
      // buildandRunToolStripMenuItem
      // 
      this.buildandRunToolStripMenuItem.Image = global::C64Studio.Properties.Resources.ToolBuildAndRun;
      this.buildandRunToolStripMenuItem.Name = "buildandRunToolStripMenuItem";
      this.buildandRunToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
      this.buildandRunToolStripMenuItem.Text = "Build &and Run";
      this.buildandRunToolStripMenuItem.Click += new System.EventHandler(this.buildandRunToolStripMenuItem_Click);
      // 
      // debugToolStripMenuItem1
      // 
      this.debugToolStripMenuItem1.Image = global::C64Studio.Properties.Resources.ToolDebug;
      this.debugToolStripMenuItem1.Name = "debugToolStripMenuItem1";
      this.debugToolStripMenuItem1.Size = new System.Drawing.Size(165, 22);
      this.debugToolStripMenuItem1.Text = "D&ebug";
      this.debugToolStripMenuItem1.Click += new System.EventHandler(this.debugToolStripMenuItem1_Click);
      // 
      // preprocessedFileToolStripMenuItem
      // 
      this.preprocessedFileToolStripMenuItem.Name = "preprocessedFileToolStripMenuItem";
      this.preprocessedFileToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
      this.preprocessedFileToolStripMenuItem.Text = "Preprocessed File";
      this.preprocessedFileToolStripMenuItem.Click += new System.EventHandler(this.preprocessedFileToolStripMenuItem_Click);
      // 
      // debugToolStripMenuItem
      // 
      this.debugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.debugConnectToolStripMenuItem,
            this.debugDisconnectToolStripMenuItem,
            this.tToolStripMenuItem,
            this.refreshRegistersToolStripMenuItem,
            this.debugBreakpointsToolStripMenuItem,
            this.dumpLabelsToolStripMenuItem,
            this.dumpHierarchyToolStripMenuItem,
            this.dumpDockStateToolStripMenuItem,
            this.runTestsToolStripMenuItem,
            this.disassembleToolStripMenuItem,
            this.markErrorToolStripMenuItem,
            this.throwExceptionToolStripMenuItem});
      this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
      this.debugToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
      this.debugToolStripMenuItem.Text = "&Debug";
      // 
      // debugConnectToolStripMenuItem
      // 
      this.debugConnectToolStripMenuItem.Name = "debugConnectToolStripMenuItem";
      this.debugConnectToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
      this.debugConnectToolStripMenuItem.Text = "Connect";
      this.debugConnectToolStripMenuItem.Click += new System.EventHandler(this.debugConnectToolStripMenuItem_Click);
      // 
      // debugDisconnectToolStripMenuItem
      // 
      this.debugDisconnectToolStripMenuItem.Name = "debugDisconnectToolStripMenuItem";
      this.debugDisconnectToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
      this.debugDisconnectToolStripMenuItem.Text = "Disconnect";
      this.debugDisconnectToolStripMenuItem.Click += new System.EventHandler(this.debugDisconnectToolStripMenuItem_Click);
      // 
      // tToolStripMenuItem
      // 
      this.tToolStripMenuItem.Name = "tToolStripMenuItem";
      this.tToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
      this.tToolStripMenuItem.Text = "Test";
      // 
      // refreshRegistersToolStripMenuItem
      // 
      this.refreshRegistersToolStripMenuItem.Name = "refreshRegistersToolStripMenuItem";
      this.refreshRegistersToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
      this.refreshRegistersToolStripMenuItem.Text = "Refresh Registers";
      this.refreshRegistersToolStripMenuItem.Click += new System.EventHandler(this.refreshRegistersToolStripMenuItem_Click);
      // 
      // debugBreakpointsToolStripMenuItem
      // 
      this.debugBreakpointsToolStripMenuItem.Name = "debugBreakpointsToolStripMenuItem";
      this.debugBreakpointsToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
      this.debugBreakpointsToolStripMenuItem.Text = "Breakpoints";
      this.debugBreakpointsToolStripMenuItem.Click += new System.EventHandler(this.listBreakpointsToolStripMenuItem_Click);
      // 
      // dumpLabelsToolStripMenuItem
      // 
      this.dumpLabelsToolStripMenuItem.Name = "dumpLabelsToolStripMenuItem";
      this.dumpLabelsToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
      this.dumpLabelsToolStripMenuItem.Text = "Dump Labels";
      this.dumpLabelsToolStripMenuItem.Click += new System.EventHandler(this.dumpLabelsToolStripMenuItem_Click);
      // 
      // dumpHierarchyToolStripMenuItem
      // 
      this.dumpHierarchyToolStripMenuItem.Name = "dumpHierarchyToolStripMenuItem";
      this.dumpHierarchyToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
      this.dumpHierarchyToolStripMenuItem.Text = "Dump Hierarchy";
      this.dumpHierarchyToolStripMenuItem.Click += new System.EventHandler(this.dumpHierarchyToolStripMenuItem_Click);
      // 
      // dumpDockStateToolStripMenuItem
      // 
      this.dumpDockStateToolStripMenuItem.Name = "dumpDockStateToolStripMenuItem";
      this.dumpDockStateToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
      this.dumpDockStateToolStripMenuItem.Text = "Dump Dock State";
      this.dumpDockStateToolStripMenuItem.Click += new System.EventHandler(this.dumpDockStateToolStripMenuItem_Click);
      // 
      // runTestsToolStripMenuItem
      // 
      this.runTestsToolStripMenuItem.Name = "runTestsToolStripMenuItem";
      this.runTestsToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
      this.runTestsToolStripMenuItem.Text = "Run Tests";
      this.runTestsToolStripMenuItem.Click += new System.EventHandler(this.runTestsToolStripMenuItem_Click);
      // 
      // disassembleToolStripMenuItem
      // 
      this.disassembleToolStripMenuItem.Name = "disassembleToolStripMenuItem";
      this.disassembleToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
      this.disassembleToolStripMenuItem.Text = "Disassemble";
      // 
      // markErrorToolStripMenuItem
      // 
      this.markErrorToolStripMenuItem.Name = "markErrorToolStripMenuItem";
      this.markErrorToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
      this.markErrorToolStripMenuItem.Text = "Mark Error";
      this.markErrorToolStripMenuItem.Click += new System.EventHandler(this.markErrorToolStripMenuItem_Click);
      // 
      // throwExceptionToolStripMenuItem
      // 
      this.throwExceptionToolStripMenuItem.Name = "throwExceptionToolStripMenuItem";
      this.throwExceptionToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
      this.throwExceptionToolStripMenuItem.Text = "Throw Exception";
      this.throwExceptionToolStripMenuItem.Click += new System.EventHandler(this.throwExceptionToolStripMenuItem_Click);
      // 
      // toolsToolStripMenuItem
      // 
      this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.disassembleToolsToolStripMenuItem});
      this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
      this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
      this.toolsToolStripMenuItem.Text = "&Tools";
      // 
      // disassembleToolsToolStripMenuItem
      // 
      this.disassembleToolsToolStripMenuItem.Name = "disassembleToolsToolStripMenuItem";
      this.disassembleToolsToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
      this.disassembleToolsToolStripMenuItem.Text = "&Disassemble File...";
      this.disassembleToolsToolStripMenuItem.Click += new System.EventHandler(this.disassembleToolsToolStripMenuItem_Click);
      // 
      // windowToolStripMenuItem
      // 
      this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.breakpointsToolStripMenuItem,
            this.debugMemoryToolStripMenuItem,
            this.debugRegistersToolStripMenuItem,
            this.debugWatchToolStripMenuItem,
            this.toolStripSeparator11,
            this.binaryEditorToolStripMenuItem,
            this.charsetEditorToolStripMenuItem,
            this.charScreenEditorToolStripMenuItem,
            this.graphicScreenEditorToolStripMenuItem,
            this.mapEditorToolStripMenuItem,
            this.spriteEditorToolStripMenuItem,
            this.valueTableEditorToolStripMenuItem,
            this.toolStripSeparator13,
            this.calculatorToolStripMenuItem,
            this.compileResulttoolStripMenuItem,
            this.disassemblerToolStripMenuItem,
            this.outlineToolStripMenuItem,
            this.outputToolStripMenuItem,
            this.petSCIITableToolStripMenuItem,
            this.searchReplaceToolStripMenuItem,
            this.searchResultsToolStripMenuItem,
            this.projectExplorerToolStripMenuItem,
            this.toolStripSeparator12,
            this.helpToolStripMenuItem,
            this.toolStripSeparator8,
            this.toolbarsToolStripMenuItem});
      this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
      this.windowToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
      this.windowToolStripMenuItem.Text = "&Window";
      // 
      // breakpointsToolStripMenuItem
      // 
      this.breakpointsToolStripMenuItem.Name = "breakpointsToolStripMenuItem";
      this.breakpointsToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
      this.breakpointsToolStripMenuItem.Text = "Breakpoints";
      // 
      // debugMemoryToolStripMenuItem
      // 
      this.debugMemoryToolStripMenuItem.Name = "debugMemoryToolStripMenuItem";
      this.debugMemoryToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
      this.debugMemoryToolStripMenuItem.Text = "Debug &Memory";
      // 
      // debugRegistersToolStripMenuItem
      // 
      this.debugRegistersToolStripMenuItem.Name = "debugRegistersToolStripMenuItem";
      this.debugRegistersToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
      this.debugRegistersToolStripMenuItem.Text = "Debug &Registers";
      // 
      // debugWatchToolStripMenuItem
      // 
      this.debugWatchToolStripMenuItem.Name = "debugWatchToolStripMenuItem";
      this.debugWatchToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
      this.debugWatchToolStripMenuItem.Text = "Debug &Watch";
      // 
      // toolStripSeparator11
      // 
      this.toolStripSeparator11.Name = "toolStripSeparator11";
      this.toolStripSeparator11.Size = new System.Drawing.Size(184, 6);
      // 
      // binaryEditorToolStripMenuItem
      // 
      this.binaryEditorToolStripMenuItem.Name = "binaryEditorToolStripMenuItem";
      this.binaryEditorToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
      this.binaryEditorToolStripMenuItem.Text = "Binary Editor";
      // 
      // charsetEditorToolStripMenuItem
      // 
      this.charsetEditorToolStripMenuItem.Name = "charsetEditorToolStripMenuItem";
      this.charsetEditorToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
      this.charsetEditorToolStripMenuItem.Text = "Charset Editor";
      // 
      // charScreenEditorToolStripMenuItem
      // 
      this.charScreenEditorToolStripMenuItem.Name = "charScreenEditorToolStripMenuItem";
      this.charScreenEditorToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
      this.charScreenEditorToolStripMenuItem.Text = "Char Screen Editor";
      // 
      // graphicScreenEditorToolStripMenuItem
      // 
      this.graphicScreenEditorToolStripMenuItem.Name = "graphicScreenEditorToolStripMenuItem";
      this.graphicScreenEditorToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
      this.graphicScreenEditorToolStripMenuItem.Text = "Graphic Screen Editor";
      // 
      // mapEditorToolStripMenuItem
      // 
      this.mapEditorToolStripMenuItem.Name = "mapEditorToolStripMenuItem";
      this.mapEditorToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
      this.mapEditorToolStripMenuItem.Text = "Map Editor";
      // 
      // spriteEditorToolStripMenuItem
      // 
      this.spriteEditorToolStripMenuItem.Name = "spriteEditorToolStripMenuItem";
      this.spriteEditorToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
      this.spriteEditorToolStripMenuItem.Text = "Sprite Editor";
      // 
      // valueTableEditorToolStripMenuItem
      // 
      this.valueTableEditorToolStripMenuItem.Name = "valueTableEditorToolStripMenuItem";
      this.valueTableEditorToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
      this.valueTableEditorToolStripMenuItem.Text = "Value Table Editor";
      // 
      // toolStripSeparator13
      // 
      this.toolStripSeparator13.Name = "toolStripSeparator13";
      this.toolStripSeparator13.Size = new System.Drawing.Size(184, 6);
      // 
      // calculatorToolStripMenuItem
      // 
      this.calculatorToolStripMenuItem.Name = "calculatorToolStripMenuItem";
      this.calculatorToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
      this.calculatorToolStripMenuItem.Text = "Calculator";
      // 
      // compileResulttoolStripMenuItem
      // 
      this.compileResulttoolStripMenuItem.Name = "compileResulttoolStripMenuItem";
      this.compileResulttoolStripMenuItem.Size = new System.Drawing.Size(187, 22);
      this.compileResulttoolStripMenuItem.Text = "&Compile Result";
      // 
      // disassemblerToolStripMenuItem
      // 
      this.disassemblerToolStripMenuItem.Name = "disassemblerToolStripMenuItem";
      this.disassemblerToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
      this.disassemblerToolStripMenuItem.Text = "Disassembler";
      // 
      // outlineToolStripMenuItem
      // 
      this.outlineToolStripMenuItem.Name = "outlineToolStripMenuItem";
      this.outlineToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
      this.outlineToolStripMenuItem.Text = "Outline";
      // 
      // outputToolStripMenuItem
      // 
      this.outputToolStripMenuItem.Name = "outputToolStripMenuItem";
      this.outputToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
      this.outputToolStripMenuItem.Text = "&Output";
      // 
      // petSCIITableToolStripMenuItem
      // 
      this.petSCIITableToolStripMenuItem.Name = "petSCIITableToolStripMenuItem";
      this.petSCIITableToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
      this.petSCIITableToolStripMenuItem.Text = "PetSCII Table";
      // 
      // searchReplaceToolStripMenuItem
      // 
      this.searchReplaceToolStripMenuItem.Name = "searchReplaceToolStripMenuItem";
      this.searchReplaceToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
      this.searchReplaceToolStripMenuItem.Text = "&Search/Replace";
      // 
      // searchResultsToolStripMenuItem
      // 
      this.searchResultsToolStripMenuItem.Name = "searchResultsToolStripMenuItem";
      this.searchResultsToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
      this.searchResultsToolStripMenuItem.Text = "Search Results";
      // 
      // projectExplorerToolStripMenuItem
      // 
      this.projectExplorerToolStripMenuItem.Name = "projectExplorerToolStripMenuItem";
      this.projectExplorerToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
      this.projectExplorerToolStripMenuItem.Text = "Solution &Explorer";
      // 
      // toolStripSeparator12
      // 
      this.toolStripSeparator12.Name = "toolStripSeparator12";
      this.toolStripSeparator12.Size = new System.Drawing.Size(184, 6);
      // 
      // helpToolStripMenuItem
      // 
      this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      this.helpToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
      this.helpToolStripMenuItem.Text = "&Help";
      // 
      // toolStripSeparator8
      // 
      this.toolStripSeparator8.Name = "toolStripSeparator8";
      this.toolStripSeparator8.Size = new System.Drawing.Size(184, 6);
      // 
      // toolbarsToolStripMenuItem
      // 
      this.toolbarsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuWindowToolbarMain,
            this.menuWindowToolbarDebugger});
      this.toolbarsToolStripMenuItem.Name = "toolbarsToolStripMenuItem";
      this.toolbarsToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
      this.toolbarsToolStripMenuItem.Text = "&Toolbars";
      // 
      // menuWindowToolbarMain
      // 
      this.menuWindowToolbarMain.Checked = true;
      this.menuWindowToolbarMain.CheckOnClick = true;
      this.menuWindowToolbarMain.CheckState = System.Windows.Forms.CheckState.Checked;
      this.menuWindowToolbarMain.Name = "menuWindowToolbarMain";
      this.menuWindowToolbarMain.Size = new System.Drawing.Size(126, 22);
      this.menuWindowToolbarMain.Text = "&Main";
      this.menuWindowToolbarMain.Click += new System.EventHandler(this.menuWindowToolbarMain_Click);
      // 
      // menuWindowToolbarDebugger
      // 
      this.menuWindowToolbarDebugger.Checked = true;
      this.menuWindowToolbarDebugger.CheckOnClick = true;
      this.menuWindowToolbarDebugger.CheckState = System.Windows.Forms.CheckState.Checked;
      this.menuWindowToolbarDebugger.Name = "menuWindowToolbarDebugger";
      this.menuWindowToolbarDebugger.Size = new System.Drawing.Size(126, 22);
      this.menuWindowToolbarDebugger.Text = "&Debugger";
      this.menuWindowToolbarDebugger.Click += new System.EventHandler(this.menuWindowToolbarDebugger_Click);
      // 
      // aboutToolStripMenuItem
      // 
      this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem1,
            this.toolStripSeparator5,
            this.licenseToolStripMenuItem,
            this.aboutToolStripMenuItem1});
      this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
      this.aboutToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
      this.aboutToolStripMenuItem.Text = "&Help";
      // 
      // helpToolStripMenuItem1
      // 
      this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
      this.helpToolStripMenuItem1.Size = new System.Drawing.Size(113, 22);
      this.helpToolStripMenuItem1.Tag = "HELP";
      this.helpToolStripMenuItem1.Text = "&Help";
      this.helpToolStripMenuItem1.Click += new System.EventHandler(this.helpToolStripMenuItem1_Click);
      // 
      // toolStripSeparator5
      // 
      this.toolStripSeparator5.Name = "toolStripSeparator5";
      this.toolStripSeparator5.Size = new System.Drawing.Size(110, 6);
      // 
      // licenseToolStripMenuItem
      // 
      this.licenseToolStripMenuItem.Name = "licenseToolStripMenuItem";
      this.licenseToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
      this.licenseToolStripMenuItem.Text = "License";
      this.licenseToolStripMenuItem.Click += new System.EventHandler(this.licenseToolStripMenuItem_Click_1);
      // 
      // aboutToolStripMenuItem1
      // 
      this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
      this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(113, 22);
      this.aboutToolStripMenuItem1.Text = "&About";
      this.aboutToolStripMenuItem1.Click += new System.EventHandler(this.aboutToolStripMenuItem1_Click);
      // 
      // mainTools
      // 
      this.mainTools.Dock = System.Windows.Forms.DockStyle.None;
      this.mainTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainToolNewItem,
            this.mainToolOpenFile,
            this.mainToolSave,
            this.mainToolSaveAll,
            this.mainToolCommentSelection,
            this.mainToolUncommentSelection,
            this.mainToolCompile,
            this.mainToolBuild,
            this.mainToolRebuild,
            this.mainToolBuildAndRun,
            this.mainToolDebug,
            this.mainToolConfig,
            this.mainToolToggleTrueDrive,
            this.mainToolEmulator,
            this.mainToolUndo,
            this.mainToolRedo,
            this.mainToolFind,
            this.mainToolFindReplace,
            this.mainToolPrint});
      this.mainTools.Location = new System.Drawing.Point(0, 24);
      this.mainTools.Name = "mainTools";
      this.mainTools.Size = new System.Drawing.Size(658, 25);
      this.mainTools.TabIndex = 4;
      this.mainTools.Text = "toolStrip1";
      // 
      // mainToolNewItem
      // 
      this.mainToolNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.mainToolNewItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.solutionToolStripMenuItem1,
            this.mainToolNewProject,
            this.toolStripSeparator1,
            this.mainToolNewASMFile,
            this.mainToolNewBasicFile,
            this.mainToolNewSpriteFile,
            this.mainToolNewCharsetFile,
            this.charsetScreenToolStripMenuItem,
            this.graphicScreenToolStripMenuItem,
            this.mapToolStripMenuItem,
            this.toolStripSeparator9,
            this.toolStripMenuItem1});
      this.mainToolNewItem.Image = global::C64Studio.Properties.Resources.ToolNewItem;
      this.mainToolNewItem.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.mainToolNewItem.Name = "mainToolNewItem";
      this.mainToolNewItem.Size = new System.Drawing.Size(32, 22);
      this.mainToolNewItem.Text = "New Item";
      this.mainToolNewItem.ButtonClick += new System.EventHandler(this.mainToolNewItem_ButtonClick);
      // 
      // solutionToolStripMenuItem1
      // 
      this.solutionToolStripMenuItem1.Name = "solutionToolStripMenuItem1";
      this.solutionToolStripMenuItem1.Size = new System.Drawing.Size(163, 22);
      this.solutionToolStripMenuItem1.Text = "Solution...";
      this.solutionToolStripMenuItem1.Click += new System.EventHandler(this.solutionToolStripMenuItem1_Click);
      // 
      // mainToolNewProject
      // 
      this.mainToolNewProject.Name = "mainToolNewProject";
      this.mainToolNewProject.Size = new System.Drawing.Size(163, 22);
      this.mainToolNewProject.Text = "Project...";
      this.mainToolNewProject.Click += new System.EventHandler(this.mainToolNewProject_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(160, 6);
      // 
      // mainToolNewASMFile
      // 
      this.mainToolNewASMFile.Name = "mainToolNewASMFile";
      this.mainToolNewASMFile.Size = new System.Drawing.Size(163, 22);
      this.mainToolNewASMFile.Text = "ASM File";
      this.mainToolNewASMFile.Click += new System.EventHandler(this.mainToolNewASMFile_Click);
      // 
      // mainToolNewBasicFile
      // 
      this.mainToolNewBasicFile.Name = "mainToolNewBasicFile";
      this.mainToolNewBasicFile.Size = new System.Drawing.Size(163, 22);
      this.mainToolNewBasicFile.Text = "BASIC File";
      this.mainToolNewBasicFile.Click += new System.EventHandler(this.mainToolNewBasicFile_Click);
      // 
      // mainToolNewSpriteFile
      // 
      this.mainToolNewSpriteFile.Name = "mainToolNewSpriteFile";
      this.mainToolNewSpriteFile.Size = new System.Drawing.Size(163, 22);
      this.mainToolNewSpriteFile.Text = "Sprite Set";
      this.mainToolNewSpriteFile.Click += new System.EventHandler(this.mainToolNewSpriteFile_Click);
      // 
      // mainToolNewCharsetFile
      // 
      this.mainToolNewCharsetFile.Name = "mainToolNewCharsetFile";
      this.mainToolNewCharsetFile.Size = new System.Drawing.Size(163, 22);
      this.mainToolNewCharsetFile.Text = "Character Set";
      this.mainToolNewCharsetFile.Click += new System.EventHandler(this.mainToolNewCharsetFile_Click);
      // 
      // charsetScreenToolStripMenuItem
      // 
      this.charsetScreenToolStripMenuItem.Name = "charsetScreenToolStripMenuItem";
      this.charsetScreenToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
      this.charsetScreenToolStripMenuItem.Text = "Character Screen";
      this.charsetScreenToolStripMenuItem.Click += new System.EventHandler(this.charsetScreenToolStripMenuItem_Click);
      // 
      // graphicScreenToolStripMenuItem
      // 
      this.graphicScreenToolStripMenuItem.Name = "graphicScreenToolStripMenuItem";
      this.graphicScreenToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
      this.graphicScreenToolStripMenuItem.Text = "Graphic Screen";
      this.graphicScreenToolStripMenuItem.Click += new System.EventHandler(this.graphicScreenToolStripMenuItem_Click);
      // 
      // mapToolStripMenuItem
      // 
      this.mapToolStripMenuItem.Name = "mapToolStripMenuItem";
      this.mapToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
      this.mapToolStripMenuItem.Text = "Map";
      this.mapToolStripMenuItem.Click += new System.EventHandler(this.mapToolStripMenuItem_Click);
      // 
      // toolStripSeparator9
      // 
      this.toolStripSeparator9.Name = "toolStripSeparator9";
      this.toolStripSeparator9.Size = new System.Drawing.Size(160, 6);
      // 
      // toolStripMenuItem1
      // 
      this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.emptyTapeT64ToolStripMenuItem,
            this.emptyDiskD64ToolStripMenuItem});
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new System.Drawing.Size(163, 22);
      this.toolStripMenuItem1.Text = "Media";
      // 
      // emptyTapeT64ToolStripMenuItem
      // 
      this.emptyTapeT64ToolStripMenuItem.Name = "emptyTapeT64ToolStripMenuItem";
      this.emptyTapeT64ToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
      this.emptyTapeT64ToolStripMenuItem.Text = "Tape Image";
      this.emptyTapeT64ToolStripMenuItem.Click += new System.EventHandler(this.emptyTapeT64ToolStripMenuItem_Click);
      // 
      // emptyDiskD64ToolStripMenuItem
      // 
      this.emptyDiskD64ToolStripMenuItem.Name = "emptyDiskD64ToolStripMenuItem";
      this.emptyDiskD64ToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
      this.emptyDiskD64ToolStripMenuItem.Text = "Disk Image";
      this.emptyDiskD64ToolStripMenuItem.Click += new System.EventHandler(this.emptyDiskD64ToolStripMenuItem_Click);
      // 
      // mainToolOpenFile
      // 
      this.mainToolOpenFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.mainToolOpenFile.Image = global::C64Studio.Properties.Resources.ToolOpenFile;
      this.mainToolOpenFile.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.mainToolOpenFile.Name = "mainToolOpenFile";
      this.mainToolOpenFile.Size = new System.Drawing.Size(23, 22);
      this.mainToolOpenFile.Text = "Open File";
      this.mainToolOpenFile.Click += new System.EventHandler(this.mainToolOpenFile_Click);
      // 
      // mainToolSave
      // 
      this.mainToolSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.mainToolSave.Enabled = false;
      this.mainToolSave.Image = global::C64Studio.Properties.Resources.ToolSave;
      this.mainToolSave.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.mainToolSave.Name = "mainToolSave";
      this.mainToolSave.Size = new System.Drawing.Size(23, 22);
      this.mainToolSave.Text = "Save";
      this.mainToolSave.Click += new System.EventHandler(this.mainToolSave_Click);
      // 
      // mainToolSaveAll
      // 
      this.mainToolSaveAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.mainToolSaveAll.Enabled = false;
      this.mainToolSaveAll.Image = global::C64Studio.Properties.Resources.ToolSaveAll;
      this.mainToolSaveAll.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.mainToolSaveAll.Name = "mainToolSaveAll";
      this.mainToolSaveAll.Size = new System.Drawing.Size(23, 22);
      this.mainToolSaveAll.Text = "Save All";
      this.mainToolSaveAll.Click += new System.EventHandler(this.mainToolSaveAll_Click);
      // 
      // mainToolCommentSelection
      // 
      this.mainToolCommentSelection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.mainToolCommentSelection.Image = global::C64Studio.Properties.Resources.ToolCommentSelection;
      this.mainToolCommentSelection.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.mainToolCommentSelection.Name = "mainToolCommentSelection";
      this.mainToolCommentSelection.Size = new System.Drawing.Size(23, 22);
      this.mainToolCommentSelection.Text = "Comment Selection";
      this.mainToolCommentSelection.Click += new System.EventHandler(this.mainToolCommentSelection_Click);
      // 
      // mainToolUncommentSelection
      // 
      this.mainToolUncommentSelection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.mainToolUncommentSelection.Image = global::C64Studio.Properties.Resources.ToolUncommentSelection;
      this.mainToolUncommentSelection.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.mainToolUncommentSelection.Name = "mainToolUncommentSelection";
      this.mainToolUncommentSelection.Size = new System.Drawing.Size(23, 22);
      this.mainToolUncommentSelection.Text = "Uncomment Selection";
      this.mainToolUncommentSelection.Click += new System.EventHandler(this.mainToolUncommentSelection_Click);
      // 
      // mainToolCompile
      // 
      this.mainToolCompile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.mainToolCompile.Image = global::C64Studio.Properties.Resources.ToolCompile;
      this.mainToolCompile.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.mainToolCompile.Name = "mainToolCompile";
      this.mainToolCompile.Size = new System.Drawing.Size(23, 22);
      this.mainToolCompile.Text = "Compile";
      this.mainToolCompile.Click += new System.EventHandler(this.mainToolCompile_Click_1);
      // 
      // mainToolBuild
      // 
      this.mainToolBuild.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.mainToolBuild.Image = global::C64Studio.Properties.Resources.ToolBuild;
      this.mainToolBuild.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.mainToolBuild.Name = "mainToolBuild";
      this.mainToolBuild.Size = new System.Drawing.Size(23, 22);
      this.mainToolBuild.Text = "Build";
      this.mainToolBuild.Click += new System.EventHandler(this.mainToolCompile_Click);
      // 
      // mainToolRebuild
      // 
      this.mainToolRebuild.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.mainToolRebuild.Image = global::C64Studio.Properties.Resources.ToolRebuild;
      this.mainToolRebuild.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.mainToolRebuild.Name = "mainToolRebuild";
      this.mainToolRebuild.Size = new System.Drawing.Size(23, 22);
      this.mainToolRebuild.Text = "Rebuild";
      this.mainToolRebuild.Click += new System.EventHandler(this.mainToolRebuild_Click);
      // 
      // mainToolBuildAndRun
      // 
      this.mainToolBuildAndRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.mainToolBuildAndRun.Image = global::C64Studio.Properties.Resources.ToolBuildAndRun;
      this.mainToolBuildAndRun.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.mainToolBuildAndRun.Name = "mainToolBuildAndRun";
      this.mainToolBuildAndRun.Size = new System.Drawing.Size(23, 22);
      this.mainToolBuildAndRun.Text = "Build and Run";
      this.mainToolBuildAndRun.Click += new System.EventHandler(this.mainToolCompileAndRun_Click);
      // 
      // mainToolDebug
      // 
      this.mainToolDebug.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.mainToolDebug.Image = global::C64Studio.Properties.Resources.ToolDebug;
      this.mainToolDebug.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.mainToolDebug.Name = "mainToolDebug";
      this.mainToolDebug.Size = new System.Drawing.Size(23, 22);
      this.mainToolDebug.Text = "Debug";
      this.mainToolDebug.Click += new System.EventHandler(this.mainToolDebug_Click);
      // 
      // mainToolConfig
      // 
      this.mainToolConfig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.mainToolConfig.Name = "mainToolConfig";
      this.mainToolConfig.Size = new System.Drawing.Size(121, 25);
      this.mainToolConfig.SelectedIndexChanged += new System.EventHandler(this.mainToolConfig_SelectedIndexChanged);
      // 
      // mainToolToggleTrueDrive
      // 
      this.mainToolToggleTrueDrive.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.mainToolToggleTrueDrive.Image = global::C64Studio.Properties.Resources.toolbar_truedrive_enabled;
      this.mainToolToggleTrueDrive.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.mainToolToggleTrueDrive.Name = "mainToolToggleTrueDrive";
      this.mainToolToggleTrueDrive.Size = new System.Drawing.Size(23, 22);
      this.mainToolToggleTrueDrive.Text = "Toogle True Drive";
      this.mainToolToggleTrueDrive.Click += new System.EventHandler(this.mainToolToggleTrueDrive_Click);
      // 
      // mainToolEmulator
      // 
      this.mainToolEmulator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.mainToolEmulator.Name = "mainToolEmulator";
      this.mainToolEmulator.Size = new System.Drawing.Size(121, 25);
      this.mainToolEmulator.SelectedIndexChanged += new System.EventHandler(this.mainToolEmulator_SelectedIndexChanged);
      // 
      // mainToolUndo
      // 
      this.mainToolUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.mainToolUndo.Enabled = false;
      this.mainToolUndo.Image = global::C64Studio.Properties.Resources.ToolUndo;
      this.mainToolUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.mainToolUndo.Name = "mainToolUndo";
      this.mainToolUndo.Size = new System.Drawing.Size(23, 22);
      this.mainToolUndo.Text = "Undo";
      this.mainToolUndo.Click += new System.EventHandler(this.mainToolUndo_Click);
      // 
      // mainToolRedo
      // 
      this.mainToolRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.mainToolRedo.Enabled = false;
      this.mainToolRedo.Image = global::C64Studio.Properties.Resources.ToolRedo;
      this.mainToolRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.mainToolRedo.Name = "mainToolRedo";
      this.mainToolRedo.Size = new System.Drawing.Size(23, 22);
      this.mainToolRedo.Text = "Redo";
      this.mainToolRedo.Click += new System.EventHandler(this.mainToolRedo_Click);
      // 
      // mainToolFind
      // 
      this.mainToolFind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.mainToolFind.Enabled = false;
      this.mainToolFind.Image = global::C64Studio.Properties.Resources.ToolFind;
      this.mainToolFind.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.mainToolFind.Name = "mainToolFind";
      this.mainToolFind.Size = new System.Drawing.Size(23, 22);
      this.mainToolFind.Text = "Find";
      this.mainToolFind.Click += new System.EventHandler(this.mainToolFind_Click);
      // 
      // mainToolFindReplace
      // 
      this.mainToolFindReplace.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.mainToolFindReplace.Enabled = false;
      this.mainToolFindReplace.Image = global::C64Studio.Properties.Resources.ToolFindReplace;
      this.mainToolFindReplace.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.mainToolFindReplace.Name = "mainToolFindReplace";
      this.mainToolFindReplace.Size = new System.Drawing.Size(23, 22);
      this.mainToolFindReplace.Text = "Find/Replace";
      this.mainToolFindReplace.Click += new System.EventHandler(this.mainToolFindReplace_Click);
      // 
      // mainToolPrint
      // 
      this.mainToolPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.mainToolPrint.Enabled = false;
      this.mainToolPrint.Image = global::C64Studio.Properties.Resources.ToolPrint;
      this.mainToolPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.mainToolPrint.Name = "mainToolPrint";
      this.mainToolPrint.Size = new System.Drawing.Size(23, 22);
      this.mainToolPrint.Text = "Print Source Code";
      this.mainToolPrint.Click += new System.EventHandler(this.mainToolPrint_Click);
      // 
      // mainStatus
      // 
      this.mainStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabelInfo,
            this.statusProgress,
            this.statusEditorDetails});
      this.mainStatus.Location = new System.Drawing.Point(0, 655);
      this.mainStatus.Name = "mainStatus";
      this.mainStatus.Size = new System.Drawing.Size(1120, 22);
      this.mainStatus.TabIndex = 5;
      this.mainStatus.Text = "statusStrip1";
      // 
      // statusLabelInfo
      // 
      this.statusLabelInfo.AutoSize = false;
      this.statusLabelInfo.Name = "statusLabelInfo";
      this.statusLabelInfo.Size = new System.Drawing.Size(300, 17);
      this.statusLabelInfo.Text = "Ready";
      this.statusLabelInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // statusProgress
      // 
      this.statusProgress.Name = "statusProgress";
      this.statusProgress.Size = new System.Drawing.Size(100, 16);
      // 
      // statusEditorDetails
      // 
      this.statusEditorDetails.AutoSize = false;
      this.statusEditorDetails.Name = "statusEditorDetails";
      this.statusEditorDetails.Size = new System.Drawing.Size(390, 17);
      this.statusEditorDetails.Text = "--";
      this.statusEditorDetails.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // debugTools
      // 
      this.debugTools.Dock = System.Windows.Forms.DockStyle.None;
      this.debugTools.Enabled = false;
      this.debugTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainDebugGo,
            this.mainDebugBreak,
            this.mainDebugStop,
            this.mainDebugStepInto,
            this.mainDebugStepOver,
            this.mainDebugStepOut});
      this.debugTools.Location = new System.Drawing.Point(658, 24);
      this.debugTools.Name = "debugTools";
      this.debugTools.Size = new System.Drawing.Size(150, 25);
      this.debugTools.TabIndex = 8;
      this.debugTools.Text = "toolStrip1";
      // 
      // mainDebugGo
      // 
      this.mainDebugGo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.mainDebugGo.Image = global::C64Studio.Properties.Resources.DebugGo;
      this.mainDebugGo.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.mainDebugGo.Name = "mainDebugGo";
      this.mainDebugGo.Size = new System.Drawing.Size(23, 22);
      this.mainDebugGo.Text = "Resume";
      this.mainDebugGo.Click += new System.EventHandler(this.mainDebugGo_Click);
      // 
      // mainDebugBreak
      // 
      this.mainDebugBreak.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.mainDebugBreak.Image = global::C64Studio.Properties.Resources.DebugBreak;
      this.mainDebugBreak.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.mainDebugBreak.Name = "mainDebugBreak";
      this.mainDebugBreak.Size = new System.Drawing.Size(23, 22);
      this.mainDebugBreak.Text = "Break into Debug Mode";
      this.mainDebugBreak.Click += new System.EventHandler(this.mainDebugBreak_Click);
      // 
      // mainDebugStop
      // 
      this.mainDebugStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.mainDebugStop.Image = global::C64Studio.Properties.Resources.DebugStop;
      this.mainDebugStop.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.mainDebugStop.Name = "mainDebugStop";
      this.mainDebugStop.Size = new System.Drawing.Size(23, 22);
      this.mainDebugStop.Text = "Stop Debugging";
      this.mainDebugStop.Click += new System.EventHandler(this.mainDebugStop_Click);
      // 
      // mainDebugStepInto
      // 
      this.mainDebugStepInto.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.mainDebugStepInto.Image = global::C64Studio.Properties.Resources.DebugStepInto;
      this.mainDebugStepInto.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.mainDebugStepInto.Name = "mainDebugStepInto";
      this.mainDebugStepInto.Size = new System.Drawing.Size(23, 22);
      this.mainDebugStepInto.Text = "Step Into";
      this.mainDebugStepInto.Click += new System.EventHandler(this.mainDebugStepInto_Click);
      // 
      // mainDebugStepOver
      // 
      this.mainDebugStepOver.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.mainDebugStepOver.Image = global::C64Studio.Properties.Resources.DebugStepOver;
      this.mainDebugStepOver.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.mainDebugStepOver.Name = "mainDebugStepOver";
      this.mainDebugStepOver.Size = new System.Drawing.Size(23, 22);
      this.mainDebugStepOver.Text = "Step Over";
      this.mainDebugStepOver.Click += new System.EventHandler(this.mainDebugStepOver_Click);
      // 
      // mainDebugStepOut
      // 
      this.mainDebugStepOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.mainDebugStepOut.Image = global::C64Studio.Properties.Resources.DebugStepOut;
      this.mainDebugStepOut.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.mainDebugStepOut.Name = "mainDebugStepOut";
      this.mainDebugStepOut.Size = new System.Drawing.Size(23, 22);
      this.mainDebugStepOut.Text = "Step Out";
      this.mainDebugStepOut.Click += new System.EventHandler(this.mainDebugStepOut_Click);
      // 
      // fileRecentlyOpenedProjectsToolStripMenuItem
      // 
      this.fileRecentlyOpenedProjectsToolStripMenuItem.Name = "fileRecentlyOpenedProjectsToolStripMenuItem";
      this.fileRecentlyOpenedProjectsToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
      this.fileRecentlyOpenedProjectsToolStripMenuItem.Text = "Recently opened projects";
      // 
      // MainForm
      // 
      this.AllowDrop = true;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1120, 677);
      this.Controls.Add(this.mainStatus);
      this.Controls.Add(this.mainTools);
      this.Controls.Add(this.mainMenu);
      this.Controls.Add(this.panelMain);
      this.Controls.Add(this.debugTools);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.IsMdiContainer = true;
      this.MainMenuStrip = this.mainMenu;
      this.Name = "MainForm";
      this.Text = "C64Studio";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
      this.Load += new System.EventHandler(this.MainForm_Load);
      this.Shown += new System.EventHandler(this.MainForm_Shown);
      this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
      this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
      this.mainMenu.ResumeLayout(false);
      this.mainMenu.PerformLayout();
      this.mainTools.ResumeLayout(false);
      this.mainTools.PerformLayout();
      this.mainStatus.ResumeLayout(false);
      this.mainStatus.PerformLayout();
      this.debugTools.ResumeLayout(false);
      this.debugTools.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    public WeifenLuo.WinFormsUI.Docking.DockPanel panelMain;
    private System.Windows.Forms.MenuStrip mainMenu;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.ToolStrip mainTools;
    private System.Windows.Forms.StatusStrip mainStatus;
    private System.Windows.Forms.ToolStripButton mainToolBuild;
    private System.Windows.Forms.ToolStripButton mainToolSave;
    private System.Windows.Forms.ToolStripSplitButton mainToolNewItem;
    private System.Windows.Forms.ToolStripMenuItem mainToolNewProject;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem mainToolNewASMFile;
    private System.Windows.Forms.ToolStripMenuItem mainToolNewBasicFile;
    private System.Windows.Forms.ToolStripMenuItem projectToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveProjectToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem closeProjectToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem projectOpenToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparatorBelowMRU;
    private System.Windows.Forms.ToolStripButton mainToolBuildAndRun;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem debugConnectToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem debugDisconnectToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem tToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    private System.Windows.Forms.ToolStripMenuItem filePreferencesToolStripMenuItem;
    private System.Windows.Forms.ToolStripButton mainToolDebug;
    private System.Windows.Forms.ToolStrip debugTools;
    private System.Windows.Forms.ToolStripButton mainDebugGo;
    private System.Windows.Forms.ToolStripButton mainDebugBreak;
    private System.Windows.Forms.ToolStripButton mainDebugStop;
    private System.Windows.Forms.ToolStripButton mainDebugStepInto;
    private System.Windows.Forms.ToolStripButton mainDebugStepOver;
    private System.Windows.Forms.ToolStripButton mainDebugStepOut;
    private System.Windows.Forms.ToolStripMenuItem refreshRegistersToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem selfParseToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem showLineinfosToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem debugBreakpointsToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparatorAboveMRU;
    private System.Windows.Forms.ToolStripMenuItem fileRecentlyOpenedFilesToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem fileNewProjectToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripMenuItem fileNewASMFileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem fileSetupWizardToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem outputToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem projectExplorerToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem compileResulttoolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem debugRegistersToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem debugWatchToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem debugMemoryToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem charsetEditorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem spriteEditorToolStripMenuItem;
    public System.Windows.Forms.ToolStripProgressBar statusProgress;
    public System.Windows.Forms.ToolStripStatusLabel statusLabelInfo;
    private System.Windows.Forms.ToolStripMenuItem fileNewSpriteFileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem fileNewCharacterFileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem mainToolNewSpriteFile;
    private System.Windows.Forms.ToolStripMenuItem mainToolNewCharsetFile;
    private System.Windows.Forms.ToolStripButton mainToolUndo;
    private System.Windows.Forms.ToolStripButton mainToolRedo;
    private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
    private System.Windows.Forms.ToolStripButton mainToolCompile;
    public System.Windows.Forms.ToolStripComboBox mainToolConfig;
    private System.Windows.Forms.ToolStripMenuItem calculatorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem fileNewBasicFileToolStripMenuItem;
    public System.Windows.Forms.ToolStripStatusLabel statusEditorDetails;
    private System.Windows.Forms.ToolStripMenuItem projectOpenTapeDiskFileMenuItem;
    private System.Windows.Forms.ToolStripMenuItem outlineToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem mediaToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem newTapeImageToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem newDiskImageToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem emptyTapeT64ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem emptyDiskD64ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem fileNewGraphicScreenEditorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem graphicImageToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem importFileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem disassembleToolsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem fileNewCharacterScreenEditorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem petSCIITableToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem fileOpenToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
    private System.Windows.Forms.ToolStripMenuItem editorOpenToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
    private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem findReplaceToolStripMenuItem;
    private System.Windows.Forms.ToolStripButton mainToolFind;
    private System.Windows.Forms.ToolStripButton mainToolFindReplace;
    private System.Windows.Forms.ToolStripButton mainToolPrint;
    private System.Windows.Forms.ToolStripButton mainToolSaveAll;
    private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveAllToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem toolbarsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem menuWindowToolbarMain;
    private System.Windows.Forms.ToolStripMenuItem menuWindowToolbarDebugger;
    private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem breakpointsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem searchReplaceToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem charScreenEditorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem graphicScreenEditorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem fileNewBinaryEditorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem mapEditorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem fileNewMapEditorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem dumpLabelsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem dumpHierarchyToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem solutionToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem solutionAddExistingProjectToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem solutionCloseToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem fileCloseToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem solutionAddNewProjectToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem solutionSaveToolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem disassemblerToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem licenseToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem dumpDockStateToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem searchResultsToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
    private System.Windows.Forms.ToolStripMenuItem runTestsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem fileNewSolutionToolStripMenuItem;
    private System.Windows.Forms.ToolStripButton mainToolRebuild;
    private System.Windows.Forms.ToolStripMenuItem disassembleToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem solutionToolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem charsetScreenToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem graphicScreenToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem mapToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
    private System.Windows.Forms.ToolStripMenuItem addNewToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem projectAddNewASMFileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem projectAddNewBASICFileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem projectAddNewSpriteSetToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem projectAddNewCharacterSetToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem projectAddNewCharacterScreenToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem projectAddNewGraphicScreenToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem projectAddNewMapToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
    private System.Windows.Forms.ToolStripButton mainToolToggleTrueDrive;
    private System.Windows.Forms.ToolStripComboBox mainToolEmulator;
    private System.Windows.Forms.ToolStripButton mainToolOpenFile;
    private System.Windows.Forms.ToolStripButton mainToolCommentSelection;
    private System.Windows.Forms.ToolStripButton mainToolUncommentSelection;
    private System.Windows.Forms.ToolStripMenuItem markErrorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem throwExceptionToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem valueTableToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem buildToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem compileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem buildToolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem rebuildToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem buildandRunToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem preprocessedFileToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
    private System.Windows.Forms.ToolStripMenuItem binaryEditorToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
    private System.Windows.Forms.ToolStripMenuItem valueTableEditorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem fileRecentlyOpenedProjectsToolStripMenuItem;
  }
}

