namespace RetroDevStudio.Dialogs
{
  partial class Settings
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose( bool disposing )
    {
      if ( disposing && ( components != null ) )
      {
        components.Dispose();
      }
      base.Dispose( disposing );
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.tabPreferences = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.checkRightClickIsBGColor = new System.Windows.Forms.CheckBox();
            this.btnBrowseDefaultOpenSolutionPath = new System.Windows.Forms.Button();
            this.editDefaultOpenSolutionPath = new System.Windows.Forms.TextBox();
            this.comboAppMode = new System.Windows.Forms.ComboBox();
            this.btnSetDefaultsFont = new System.Windows.Forms.Button();
            this.checkBASICUseC64Font = new System.Windows.Forms.CheckBox();
            this.btnChangeBASICFont = new System.Windows.Forms.Button();
            this.labelBASICFontPreview = new System.Windows.Forms.Label();
            this.btnChooseFont = new System.Windows.Forms.Button();
            this.labelFontPreview = new System.Windows.Forms.Label();
            this.editMaxMRUEntries = new System.Windows.Forms.TextBox();
            this.editTabSize = new System.Windows.Forms.TextBox();
            this.label32 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.checkAutoOpenLastSolution = new System.Windows.Forms.CheckBox();
            this.checkStripTrailingSpaces = new System.Windows.Forms.CheckBox();
            this.checkConvertTabsToSpaces = new System.Windows.Forms.CheckBox();
            this.checkPlaySoundSearchTextNotFound = new System.Windows.Forms.CheckBox();
            this.checkPlaySoundCompileSuccessful = new System.Windows.Forms.CheckBox();
            this.checkPlaySoundCompileFail = new System.Windows.Forms.CheckBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tabTools = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.DgvArguments = new System.Windows.Forms.DataGridView();
            this.ColArgumentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColArgumentValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.checkPassLabelsToEmulator = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.editToolDiskArguments = new System.Windows.Forms.TextBox();
            this.label37 = new System.Windows.Forms.Label();
            this.editToolTapeArguments = new System.Windows.Forms.TextBox();
            this.label36 = new System.Windows.Forms.Label();
            this.editToolCartArguments = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.editToolDebugArguments = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.editToolProgramArguments = new System.Windows.Forms.TextBox();
            this.comboToolType = new System.Windows.Forms.ComboBox();
            this.editToolName = new System.Windows.Forms.TextBox();
            this.editWorkPath = new System.Windows.Forms.TextBox();
            this.btnBrowseToolWorkPath = new System.Windows.Forms.Button();
            this.btnBrowseTool = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.labelToolPath = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabKeyBindings = new System.Windows.Forms.TabPage();
            this.btnSetDefaultsKeyBinding = new System.Windows.Forms.Button();
            this.btnUnbindKey = new System.Windows.Forms.Button();
            this.btnBindKeySecondary = new System.Windows.Forms.Button();
            this.btnBindKey = new System.Windows.Forms.Button();
            this.editKeyBinding = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.listFunctions = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label9 = new System.Windows.Forms.Label();
            this.tabErrorsWarnings = new System.Windows.Forms.TabPage();
            this.comboASMEncoding = new System.Windows.Forms.ComboBox();
            this.btmASMLibraryPathBrowse = new System.Windows.Forms.Button();
            this.editASMLibraryPath = new System.Windows.Forms.TextBox();
            this.checkASMShowAddress = new System.Windows.Forms.CheckBox();
            this.checkASMShowAutoComplete = new System.Windows.Forms.CheckBox();
            this.checkASMAutoTruncateLiteralValues = new System.Windows.Forms.CheckBox();
            this.checkASMShowMiniMap = new System.Windows.Forms.CheckBox();
            this.checkASMShowSizes = new System.Windows.Forms.CheckBox();
            this.checkASMShowCycles = new System.Windows.Forms.CheckBox();
            this.checkASMShowLineNumbers = new System.Windows.Forms.CheckBox();
            this.label30 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.listHacks = new System.Windows.Forms.CheckedListBox();
            this.label35 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.listWarningsAsErrors = new System.Windows.Forms.CheckedListBox();
            this.label33 = new System.Windows.Forms.Label();
            this.listIgnoredWarnings = new System.Windows.Forms.CheckedListBox();
            this.label20 = new System.Windows.Forms.Label();
            this.tabColors = new System.Windows.Forms.TabPage();
            this.btnSetDefaultsColors = new System.Windows.Forms.Button();
            this.btnChooseBG = new System.Windows.Forms.Button();
            this.btnChooseFG = new System.Windows.Forms.Button();
            this.panelElementPreview = new System.Windows.Forms.Panel();
            this.comboElementBG = new System.Windows.Forms.ComboBox();
            this.comboElementFG = new System.Windows.Forms.ComboBox();
            this.label27 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.listColoring = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label16 = new System.Windows.Forms.Label();
            this.tabKeyMaps = new System.Windows.Forms.TabPage();
            this.btnUnbindBASICKeyMapBinding = new System.Windows.Forms.Button();
            this.btnBindBASICKeyMapBinding = new System.Windows.Forms.Button();
            this.editBASICKeyMapBinding = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.tabBASIC = new System.Windows.Forms.TabPage();
            this.checkBASICAutoToggleEntryMode = new System.Windows.Forms.CheckBox();
            this.checkBASICShowControlCodes = new System.Windows.Forms.CheckBox();
            this.checkBASICStripREM = new System.Windows.Forms.CheckBox();
            this.checkBASICStripSpaces = new System.Windows.Forms.CheckBox();
            this.label22 = new System.Windows.Forms.Label();
            this.BtnMacros = new System.Windows.Forms.Button();
            this.btnExportAllSettings = new System.Windows.Forms.Button();
            this.btnImportAllSettings = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnImportCurrentPageSettings = new System.Windows.Forms.Button();
            this.btnExportCurrentPageSettings = new System.Windows.Forms.Button();
            this.btnClone = new RetroDevStudio.Controls.CSButton();
            this.btnMoveDown = new RetroDevStudio.Controls.CSButton();
            this.btnMoveUp = new RetroDevStudio.Controls.CSButton();
            this.btnDelete = new RetroDevStudio.Controls.CSButton();
            this.btnAdd = new RetroDevStudio.Controls.CSButton();
            this.alistTools = new RetroDevStudio.Controls.ArrangedItemList();
            this.asmLibraryPathList = new RetroDevStudio.Controls.ArrangedItemList();
            this.listBASICKeyMap = new RetroDevStudio.Controls.MeasurableListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPreferences.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.tabTools.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvArguments)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabKeyBindings.SuspendLayout();
            this.tabErrorsWarnings.SuspendLayout();
            this.tabColors.SuspendLayout();
            this.tabKeyMaps.SuspendLayout();
            this.tabBASIC.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPreferences
            // 
            this.tabPreferences.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabPreferences.Controls.Add(this.tabGeneral);
            this.tabPreferences.Controls.Add(this.tabTools);
            this.tabPreferences.Controls.Add(this.tabKeyBindings);
            this.tabPreferences.Controls.Add(this.tabErrorsWarnings);
            this.tabPreferences.Controls.Add(this.tabColors);
            this.tabPreferences.Controls.Add(this.tabKeyMaps);
            this.tabPreferences.Controls.Add(this.tabBASIC);
            this.tabPreferences.Location = new System.Drawing.Point(-1, 5);
            this.tabPreferences.Name = "tabPreferences";
            this.tabPreferences.SelectedIndex = 0;
            this.tabPreferences.Size = new System.Drawing.Size(697, 463);
            this.tabPreferences.TabIndex = 0;
            this.tabPreferences.SelectedIndexChanged += new System.EventHandler(this.tabPreferences_SelectedIndexChanged);
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.checkRightClickIsBGColor);
            this.tabGeneral.Controls.Add(this.btnBrowseDefaultOpenSolutionPath);
            this.tabGeneral.Controls.Add(this.editDefaultOpenSolutionPath);
            this.tabGeneral.Controls.Add(this.comboAppMode);
            this.tabGeneral.Controls.Add(this.btnSetDefaultsFont);
            this.tabGeneral.Controls.Add(this.checkBASICUseC64Font);
            this.tabGeneral.Controls.Add(this.btnChangeBASICFont);
            this.tabGeneral.Controls.Add(this.labelBASICFontPreview);
            this.tabGeneral.Controls.Add(this.btnChooseFont);
            this.tabGeneral.Controls.Add(this.labelFontPreview);
            this.tabGeneral.Controls.Add(this.editMaxMRUEntries);
            this.tabGeneral.Controls.Add(this.editTabSize);
            this.tabGeneral.Controls.Add(this.label32);
            this.tabGeneral.Controls.Add(this.label31);
            this.tabGeneral.Controls.Add(this.label14);
            this.tabGeneral.Controls.Add(this.checkAutoOpenLastSolution);
            this.tabGeneral.Controls.Add(this.checkStripTrailingSpaces);
            this.tabGeneral.Controls.Add(this.checkConvertTabsToSpaces);
            this.tabGeneral.Controls.Add(this.checkPlaySoundSearchTextNotFound);
            this.tabGeneral.Controls.Add(this.checkPlaySoundCompileSuccessful);
            this.tabGeneral.Controls.Add(this.checkPlaySoundCompileFail);
            this.tabGeneral.Controls.Add(this.label15);
            this.tabGeneral.Controls.Add(this.label28);
            this.tabGeneral.Controls.Add(this.label29);
            this.tabGeneral.Controls.Add(this.label13);
            this.tabGeneral.Controls.Add(this.label11);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Size = new System.Drawing.Size(689, 437);
            this.tabGeneral.TabIndex = 2;
            this.tabGeneral.Text = "General";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // checkRightClickIsBGColor
            // 
            this.checkRightClickIsBGColor.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkRightClickIsBGColor.Location = new System.Drawing.Point(340, 225);
            this.checkRightClickIsBGColor.Name = "checkRightClickIsBGColor";
            this.checkRightClickIsBGColor.Size = new System.Drawing.Size(307, 24);
            this.checkRightClickIsBGColor.TabIndex = 11;
            this.checkRightClickIsBGColor.Text = "Right Click is Paint with BG Color";
            this.checkRightClickIsBGColor.UseVisualStyleBackColor = true;
            this.checkRightClickIsBGColor.CheckedChanged += new System.EventHandler(this.checkRightClickIsBGColor_CheckedChanged);
            // 
            // btnBrowseDefaultOpenSolutionPath
            // 
            this.btnBrowseDefaultOpenSolutionPath.Location = new System.Drawing.Point(624, 46);
            this.btnBrowseDefaultOpenSolutionPath.Name = "btnBrowseDefaultOpenSolutionPath";
            this.btnBrowseDefaultOpenSolutionPath.Size = new System.Drawing.Size(23, 20);
            this.btnBrowseDefaultOpenSolutionPath.TabIndex = 5;
            this.btnBrowseDefaultOpenSolutionPath.Text = "...";
            this.btnBrowseDefaultOpenSolutionPath.UseVisualStyleBackColor = true;
            this.btnBrowseDefaultOpenSolutionPath.Click += new System.EventHandler(this.btnBrowseDefaultOpenSolutionPath_Click);
            // 
            // editDefaultOpenSolutionPath
            // 
            this.editDefaultOpenSolutionPath.Location = new System.Drawing.Point(478, 46);
            this.editDefaultOpenSolutionPath.Name = "editDefaultOpenSolutionPath";
            this.editDefaultOpenSolutionPath.Size = new System.Drawing.Size(140, 20);
            this.editDefaultOpenSolutionPath.TabIndex = 4;
            // 
            // comboAppMode
            // 
            this.comboAppMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboAppMode.FormattingEnabled = true;
            this.comboAppMode.Items.AddRange(new object[] {
            "Undecided",
            "Normal (settings in UserAppData)",
            "Portable Mode (settings local)"});
            this.comboAppMode.Location = new System.Drawing.Point(28, 227);
            this.comboAppMode.Name = "comboAppMode";
            this.comboAppMode.Size = new System.Drawing.Size(209, 21);
            this.comboAppMode.TabIndex = 10;
            this.comboAppMode.SelectedIndexChanged += new System.EventHandler(this.comboAppMode_SelectedIndexChanged);
            // 
            // btnSetDefaultsFont
            // 
            this.btnSetDefaultsFont.Location = new System.Drawing.Point(478, 287);
            this.btnSetDefaultsFont.Name = "btnSetDefaultsFont";
            this.btnSetDefaultsFont.Size = new System.Drawing.Size(124, 23);
            this.btnSetDefaultsFont.TabIndex = 13;
            this.btnSetDefaultsFont.Text = "Set Default Fonts";
            this.btnSetDefaultsFont.UseVisualStyleBackColor = true;
            this.btnSetDefaultsFont.Click += new System.EventHandler(this.btnSetDefaultsFont_Click);
            // 
            // checkBASICUseC64Font
            // 
            this.checkBASICUseC64Font.AutoSize = true;
            this.checkBASICUseC64Font.Location = new System.Drawing.Point(336, 343);
            this.checkBASICUseC64Font.Name = "checkBASICUseC64Font";
            this.checkBASICUseC64Font.Size = new System.Drawing.Size(88, 17);
            this.checkBASICUseC64Font.TabIndex = 15;
            this.checkBASICUseC64Font.Text = "Use C64 font";
            this.checkBASICUseC64Font.UseVisualStyleBackColor = true;
            this.checkBASICUseC64Font.CheckedChanged += new System.EventHandler(this.checkBASICUseC64Font_CheckedChanged);
            // 
            // btnChangeBASICFont
            // 
            this.btnChangeBASICFont.Enabled = false;
            this.btnChangeBASICFont.Location = new System.Drawing.Point(242, 339);
            this.btnChangeBASICFont.Name = "btnChangeBASICFont";
            this.btnChangeBASICFont.Size = new System.Drawing.Size(88, 23);
            this.btnChangeBASICFont.TabIndex = 14;
            this.btnChangeBASICFont.Text = "Change Font";
            this.btnChangeBASICFont.UseVisualStyleBackColor = true;
            this.btnChangeBASICFont.Click += new System.EventHandler(this.btnChooseBASICFont_Click);
            // 
            // labelBASICFontPreview
            // 
            this.labelBASICFontPreview.Location = new System.Drawing.Point(27, 344);
            this.labelBASICFontPreview.Name = "labelBASICFontPreview";
            this.labelBASICFontPreview.Size = new System.Drawing.Size(210, 62);
            this.labelBASICFontPreview.TabIndex = 7;
            this.labelBASICFontPreview.Text = "BASIC Font Preview";
            // 
            // btnChooseFont
            // 
            this.btnChooseFont.Location = new System.Drawing.Point(242, 287);
            this.btnChooseFont.Name = "btnChooseFont";
            this.btnChooseFont.Size = new System.Drawing.Size(88, 23);
            this.btnChooseFont.TabIndex = 12;
            this.btnChooseFont.Text = "Change Font";
            this.btnChooseFont.UseVisualStyleBackColor = true;
            this.btnChooseFont.Click += new System.EventHandler(this.btnChooseFont_Click);
            // 
            // labelFontPreview
            // 
            this.labelFontPreview.Location = new System.Drawing.Point(27, 292);
            this.labelFontPreview.Name = "labelFontPreview";
            this.labelFontPreview.Size = new System.Drawing.Size(210, 44);
            this.labelFontPreview.TabIndex = 7;
            this.labelFontPreview.Text = "Font Preview";
            // 
            // editMaxMRUEntries
            // 
            this.editMaxMRUEntries.Location = new System.Drawing.Point(478, 69);
            this.editMaxMRUEntries.MaxLength = 2;
            this.editMaxMRUEntries.Name = "editMaxMRUEntries";
            this.editMaxMRUEntries.Size = new System.Drawing.Size(140, 20);
            this.editMaxMRUEntries.TabIndex = 6;
            this.editMaxMRUEntries.TextChanged += new System.EventHandler(this.editMaxMRUEntries_TextChanged);
            // 
            // editTabSize
            // 
            this.editTabSize.Location = new System.Drawing.Point(154, 155);
            this.editTabSize.MaxLength = 2;
            this.editTabSize.Name = "editTabSize";
            this.editTabSize.Size = new System.Drawing.Size(88, 20);
            this.editTabSize.TabIndex = 9;
            this.editTabSize.TextChanged += new System.EventHandler(this.editTabSize_TextChanged);
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(337, 75);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(95, 13);
            this.label32.TabIndex = 5;
            this.label32.Text = "Max. MRU entries:";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(337, 50);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(139, 13);
            this.label31.TabIndex = 5;
            this.label31.Text = "Default Solution Open Path:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(27, 158);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(52, 13);
            this.label14.TabIndex = 5;
            this.label14.Text = "Tab Size:";
            // 
            // checkAutoOpenLastSolution
            // 
            this.checkAutoOpenLastSolution.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkAutoOpenLastSolution.Location = new System.Drawing.Point(336, 21);
            this.checkAutoOpenLastSolution.Name = "checkAutoOpenLastSolution";
            this.checkAutoOpenLastSolution.Size = new System.Drawing.Size(311, 24);
            this.checkAutoOpenLastSolution.TabIndex = 3;
            this.checkAutoOpenLastSolution.Text = "Open last solution on startup";
            this.checkAutoOpenLastSolution.UseVisualStyleBackColor = true;
            this.checkAutoOpenLastSolution.CheckedChanged += new System.EventHandler(this.checkOpenLastSolution_CheckedChanged);
            // 
            // checkStripTrailingSpaces
            // 
            this.checkStripTrailingSpaces.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkStripTrailingSpaces.Location = new System.Drawing.Point(262, 125);
            this.checkStripTrailingSpaces.Name = "checkStripTrailingSpaces";
            this.checkStripTrailingSpaces.Size = new System.Drawing.Size(214, 24);
            this.checkStripTrailingSpaces.TabIndex = 8;
            this.checkStripTrailingSpaces.Text = "Strip Trailing Spaces/Tabs";
            this.checkStripTrailingSpaces.UseVisualStyleBackColor = true;
            this.checkStripTrailingSpaces.CheckedChanged += new System.EventHandler(this.checkStripTrailingSpaces_CheckedChanged);
            // 
            // checkConvertTabsToSpaces
            // 
            this.checkConvertTabsToSpaces.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkConvertTabsToSpaces.Location = new System.Drawing.Point(28, 125);
            this.checkConvertTabsToSpaces.Name = "checkConvertTabsToSpaces";
            this.checkConvertTabsToSpaces.Size = new System.Drawing.Size(214, 24);
            this.checkConvertTabsToSpaces.TabIndex = 7;
            this.checkConvertTabsToSpaces.Text = "Convert tabs to spaces";
            this.checkConvertTabsToSpaces.UseVisualStyleBackColor = true;
            this.checkConvertTabsToSpaces.CheckedChanged += new System.EventHandler(this.checkConvertTabsToSpaces_CheckedChanged);
            // 
            // checkPlaySoundSearchTextNotFound
            // 
            this.checkPlaySoundSearchTextNotFound.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkPlaySoundSearchTextNotFound.Location = new System.Drawing.Point(23, 71);
            this.checkPlaySoundSearchTextNotFound.Name = "checkPlaySoundSearchTextNotFound";
            this.checkPlaySoundSearchTextNotFound.Size = new System.Drawing.Size(214, 17);
            this.checkPlaySoundSearchTextNotFound.TabIndex = 2;
            this.checkPlaySoundSearchTextNotFound.Text = "Search Text not found";
            this.checkPlaySoundSearchTextNotFound.UseVisualStyleBackColor = true;
            // 
            // checkPlaySoundCompileSuccessful
            // 
            this.checkPlaySoundCompileSuccessful.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkPlaySoundCompileSuccessful.Location = new System.Drawing.Point(23, 48);
            this.checkPlaySoundCompileSuccessful.Name = "checkPlaySoundCompileSuccessful";
            this.checkPlaySoundCompileSuccessful.Size = new System.Drawing.Size(214, 17);
            this.checkPlaySoundCompileSuccessful.TabIndex = 1;
            this.checkPlaySoundCompileSuccessful.Text = "Build Successful";
            this.checkPlaySoundCompileSuccessful.UseVisualStyleBackColor = true;
            // 
            // checkPlaySoundCompileFail
            // 
            this.checkPlaySoundCompileFail.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkPlaySoundCompileFail.Location = new System.Drawing.Point(23, 25);
            this.checkPlaySoundCompileFail.Name = "checkPlaySoundCompileFail";
            this.checkPlaySoundCompileFail.Size = new System.Drawing.Size(214, 17);
            this.checkPlaySoundCompileFail.TabIndex = 0;
            this.checkPlaySoundCompileFail.Text = "Build Failed";
            this.checkPlaySoundCompileFail.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(9, 271);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(90, 13);
            this.label15.TabIndex = 2;
            this.label15.Text = "Text Editor Fonts:";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(333, 211);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(88, 13);
            this.label28.TabIndex = 2;
            this.label28.Text = "Editor Behaviour:";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(9, 211);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(92, 13);
            this.label29.TabIndex = 2;
            this.label29.Text = "Application Mode:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(9, 109);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(112, 13);
            this.label13.TabIndex = 2;
            this.label13.Text = "Text Editor Behaviour:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 7);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(128, 13);
            this.label11.TabIndex = 2;
            this.label11.Text = "Sound Events, play when";
            // 
            // tabTools
            // 
            this.tabTools.Controls.Add(this.btnClone);
            this.tabTools.Controls.Add(this.btnMoveDown);
            this.tabTools.Controls.Add(this.btnMoveUp);
            this.tabTools.Controls.Add(this.btnDelete);
            this.tabTools.Controls.Add(this.btnAdd);
            this.tabTools.Controls.Add(this.groupBox2);
            this.tabTools.Controls.Add(this.alistTools);
            this.tabTools.Controls.Add(this.checkPassLabelsToEmulator);
            this.tabTools.Controls.Add(this.groupBox1);
            this.tabTools.Controls.Add(this.comboToolType);
            this.tabTools.Controls.Add(this.editToolName);
            this.tabTools.Controls.Add(this.editWorkPath);
            this.tabTools.Controls.Add(this.btnBrowseToolWorkPath);
            this.tabTools.Controls.Add(this.btnBrowseTool);
            this.tabTools.Controls.Add(this.label7);
            this.tabTools.Controls.Add(this.labelToolPath);
            this.tabTools.Controls.Add(this.label6);
            this.tabTools.Controls.Add(this.label8);
            this.tabTools.Controls.Add(this.label3);
            this.tabTools.Controls.Add(this.label2);
            this.tabTools.Controls.Add(this.label1);
            this.tabTools.Location = new System.Drawing.Point(4, 22);
            this.tabTools.Name = "tabTools";
            this.tabTools.Padding = new System.Windows.Forms.Padding(3);
            this.tabTools.Size = new System.Drawing.Size(689, 437);
            this.tabTools.TabIndex = 0;
            this.tabTools.Text = "Tools";
            this.tabTools.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.DgvArguments);
            this.groupBox2.Location = new System.Drawing.Point(220, 294);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(449, 107);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Additional Arguments";
            // 
            // DgvArguments
            // 
            this.DgvArguments.AllowUserToAddRows = false;
            this.DgvArguments.AllowUserToDeleteRows = false;
            this.DgvArguments.AllowUserToResizeRows = false;
            this.DgvArguments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvArguments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColArgumentName,
            this.ColArgumentValue,
            this.ColEnabled});
            this.DgvArguments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DgvArguments.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.DgvArguments.Location = new System.Drawing.Point(3, 16);
            this.DgvArguments.MultiSelect = false;
            this.DgvArguments.Name = "DgvArguments";
            this.DgvArguments.RowHeadersVisible = false;
            this.DgvArguments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.DgvArguments.ShowCellErrors = false;
            this.DgvArguments.ShowRowErrors = false;
            this.DgvArguments.Size = new System.Drawing.Size(443, 88);
            this.DgvArguments.TabIndex = 0;
            this.DgvArguments.SelectionChanged += new System.EventHandler(this.DgvArguments_SelectionChanged);
            // 
            // ColArgumentName
            // 
            this.ColArgumentName.DataPropertyName = "Name";
            this.ColArgumentName.HeaderText = "Name";
            this.ColArgumentName.Name = "ColArgumentName";
            this.ColArgumentName.Width = 80;
            // 
            // ColArgumentValue
            // 
            this.ColArgumentValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColArgumentValue.DataPropertyName = "Value";
            this.ColArgumentValue.HeaderText = "Argument";
            this.ColArgumentValue.Name = "ColArgumentValue";
            // 
            // ColEnabled
            // 
            this.ColEnabled.DataPropertyName = "Enabled";
            this.ColEnabled.HeaderText = "EN";
            this.ColEnabled.Name = "ColEnabled";
            this.ColEnabled.Width = 24;
            // 
            // checkPassLabelsToEmulator
            // 
            this.checkPassLabelsToEmulator.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkPassLabelsToEmulator.Location = new System.Drawing.Point(510, 407);
            this.checkPassLabelsToEmulator.Name = "checkPassLabelsToEmulator";
            this.checkPassLabelsToEmulator.Size = new System.Drawing.Size(156, 24);
            this.checkPassLabelsToEmulator.TabIndex = 10;
            this.checkPassLabelsToEmulator.Text = "Forward labels to emulator";
            this.checkPassLabelsToEmulator.UseVisualStyleBackColor = true;
            this.checkPassLabelsToEmulator.CheckedChanged += new System.EventHandler(this.checkPassLabelsToEmulator_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.editToolDiskArguments);
            this.groupBox1.Controls.Add(this.label37);
            this.groupBox1.Controls.Add(this.editToolTapeArguments);
            this.groupBox1.Controls.Add(this.label36);
            this.groupBox1.Controls.Add(this.editToolCartArguments);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.editToolDebugArguments);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.editToolProgramArguments);
            this.groupBox1.Location = new System.Drawing.Point(220, 140);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(449, 148);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Runtime Arguments";
            // 
            // editToolDiskArguments
            // 
            this.editToolDiskArguments.Location = new System.Drawing.Point(83, 71);
            this.editToolDiskArguments.Name = "editToolDiskArguments";
            this.editToolDiskArguments.Size = new System.Drawing.Size(358, 20);
            this.editToolDiskArguments.TabIndex = 7;
            this.editToolDiskArguments.TextChanged += new System.EventHandler(this.editToolDiskArguments_TextChanged);
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(6, 74);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(31, 13);
            this.label37.TabIndex = 8;
            this.label37.Text = "Disk:";
            // 
            // editToolTapeArguments
            // 
            this.editToolTapeArguments.Location = new System.Drawing.Point(83, 45);
            this.editToolTapeArguments.Name = "editToolTapeArguments";
            this.editToolTapeArguments.Size = new System.Drawing.Size(358, 20);
            this.editToolTapeArguments.TabIndex = 5;
            this.editToolTapeArguments.TextChanged += new System.EventHandler(this.editToolTapeArguments_TextChanged);
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(6, 48);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(35, 13);
            this.label36.TabIndex = 6;
            this.label36.Text = "Tape:";
            // 
            // editToolCartArguments
            // 
            this.editToolCartArguments.Location = new System.Drawing.Point(83, 97);
            this.editToolCartArguments.Name = "editToolCartArguments";
            this.editToolCartArguments.Size = new System.Drawing.Size(358, 20);
            this.editToolCartArguments.TabIndex = 1;
            this.editToolCartArguments.TextChanged += new System.EventHandler(this.editToolCartArguments_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Program";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 100);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(52, 13);
            this.label12.TabIndex = 3;
            this.label12.Text = "Cartridge:";
            // 
            // editToolDebugArguments
            // 
            this.editToolDebugArguments.Location = new System.Drawing.Point(83, 123);
            this.editToolDebugArguments.Name = "editToolDebugArguments";
            this.editToolDebugArguments.Size = new System.Drawing.Size(358, 20);
            this.editToolDebugArguments.TabIndex = 2;
            this.editToolDebugArguments.TextChanged += new System.EventHandler(this.editToolDebugArguments_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Debug:";
            // 
            // editToolProgramArguments
            // 
            this.editToolProgramArguments.Location = new System.Drawing.Point(83, 19);
            this.editToolProgramArguments.Name = "editToolProgramArguments";
            this.editToolProgramArguments.Size = new System.Drawing.Size(358, 20);
            this.editToolProgramArguments.TabIndex = 0;
            this.editToolProgramArguments.TextChanged += new System.EventHandler(this.editToolPRGArguments_TextChanged);
            // 
            // comboToolType
            // 
            this.comboToolType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboToolType.FormattingEnabled = true;
            this.comboToolType.Location = new System.Drawing.Point(305, 58);
            this.comboToolType.Name = "comboToolType";
            this.comboToolType.Size = new System.Drawing.Size(364, 21);
            this.comboToolType.TabIndex = 7;
            this.comboToolType.SelectedIndexChanged += new System.EventHandler(this.comboToolType_SelectedIndexChanged);
            // 
            // editToolName
            // 
            this.editToolName.Location = new System.Drawing.Point(305, 29);
            this.editToolName.Name = "editToolName";
            this.editToolName.Size = new System.Drawing.Size(364, 20);
            this.editToolName.TabIndex = 6;
            this.editToolName.TextChanged += new System.EventHandler(this.editToolName_TextChanged);
            // 
            // editWorkPath
            // 
            this.editWorkPath.Location = new System.Drawing.Point(305, 114);
            this.editWorkPath.Name = "editWorkPath";
            this.editWorkPath.Size = new System.Drawing.Size(334, 20);
            this.editWorkPath.TabIndex = 9;
            this.editWorkPath.TextChanged += new System.EventHandler(this.editWorkPath_TextChanged);
            // 
            // btnBrowseToolWorkPath
            // 
            this.btnBrowseToolWorkPath.Location = new System.Drawing.Point(645, 112);
            this.btnBrowseToolWorkPath.Name = "btnBrowseToolWorkPath";
            this.btnBrowseToolWorkPath.Size = new System.Drawing.Size(24, 23);
            this.btnBrowseToolWorkPath.TabIndex = 5;
            this.btnBrowseToolWorkPath.Text = "...";
            this.btnBrowseToolWorkPath.UseVisualStyleBackColor = true;
            this.btnBrowseToolWorkPath.Click += new System.EventHandler(this.btnBrowseToolWorkPath_Click);
            // 
            // btnBrowseTool
            // 
            this.btnBrowseTool.Location = new System.Drawing.Point(645, 85);
            this.btnBrowseTool.Name = "btnBrowseTool";
            this.btnBrowseTool.Size = new System.Drawing.Size(24, 23);
            this.btnBrowseTool.TabIndex = 5;
            this.btnBrowseTool.Text = "...";
            this.btnBrowseTool.UseVisualStyleBackColor = true;
            this.btnBrowseTool.Click += new System.EventHandler(this.btnBrowseTool_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(219, 32);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Name:";
            // 
            // labelToolPath
            // 
            this.labelToolPath.AutoEllipsis = true;
            this.labelToolPath.Location = new System.Drawing.Point(302, 89);
            this.labelToolPath.Name = "labelToolPath";
            this.labelToolPath.Size = new System.Drawing.Size(337, 23);
            this.labelToolPath.TabIndex = 8;
            this.labelToolPath.Text = "Tool Path";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(219, 61);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Type:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(219, 117);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Working Path:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(219, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Executable:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(217, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Tool Settings:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Available Tools:";
            // 
            // tabKeyBindings
            // 
            this.tabKeyBindings.Controls.Add(this.btnSetDefaultsKeyBinding);
            this.tabKeyBindings.Controls.Add(this.btnUnbindKey);
            this.tabKeyBindings.Controls.Add(this.btnBindKeySecondary);
            this.tabKeyBindings.Controls.Add(this.btnBindKey);
            this.tabKeyBindings.Controls.Add(this.editKeyBinding);
            this.tabKeyBindings.Controls.Add(this.label10);
            this.tabKeyBindings.Controls.Add(this.listFunctions);
            this.tabKeyBindings.Controls.Add(this.label9);
            this.tabKeyBindings.Location = new System.Drawing.Point(4, 22);
            this.tabKeyBindings.Name = "tabKeyBindings";
            this.tabKeyBindings.Padding = new System.Windows.Forms.Padding(3);
            this.tabKeyBindings.Size = new System.Drawing.Size(689, 437);
            this.tabKeyBindings.TabIndex = 1;
            this.tabKeyBindings.Text = "Key Bindings";
            this.tabKeyBindings.UseVisualStyleBackColor = true;
            // 
            // btnSetDefaultsKeyBinding
            // 
            this.btnSetDefaultsKeyBinding.Location = new System.Drawing.Point(558, 409);
            this.btnSetDefaultsKeyBinding.Name = "btnSetDefaultsKeyBinding";
            this.btnSetDefaultsKeyBinding.Size = new System.Drawing.Size(124, 23);
            this.btnSetDefaultsKeyBinding.TabIndex = 4;
            this.btnSetDefaultsKeyBinding.Text = "Set Defaults";
            this.btnSetDefaultsKeyBinding.UseVisualStyleBackColor = true;
            this.btnSetDefaultsKeyBinding.Click += new System.EventHandler(this.btnSetDefaultsKeyBinding_Click);
            // 
            // btnUnbindKey
            // 
            this.btnUnbindKey.Enabled = false;
            this.btnUnbindKey.Location = new System.Drawing.Point(465, 409);
            this.btnUnbindKey.Name = "btnUnbindKey";
            this.btnUnbindKey.Size = new System.Drawing.Size(75, 23);
            this.btnUnbindKey.TabIndex = 4;
            this.btnUnbindKey.Text = "Unbind Key";
            this.btnUnbindKey.UseVisualStyleBackColor = true;
            this.btnUnbindKey.Click += new System.EventHandler(this.btnUnbindKey_Click);
            // 
            // btnBindKeySecondary
            // 
            this.btnBindKeySecondary.Location = new System.Drawing.Point(370, 409);
            this.btnBindKeySecondary.Name = "btnBindKeySecondary";
            this.btnBindKeySecondary.Size = new System.Drawing.Size(75, 23);
            this.btnBindKeySecondary.TabIndex = 4;
            this.btnBindKeySecondary.Text = "Bind 2nd";
            this.btnBindKeySecondary.UseVisualStyleBackColor = true;
            this.btnBindKeySecondary.Click += new System.EventHandler(this.btnBindKey2_Click);
            // 
            // btnBindKey
            // 
            this.btnBindKey.Location = new System.Drawing.Point(289, 409);
            this.btnBindKey.Name = "btnBindKey";
            this.btnBindKey.Size = new System.Drawing.Size(75, 23);
            this.btnBindKey.TabIndex = 4;
            this.btnBindKey.Text = "Bind Key";
            this.btnBindKey.UseVisualStyleBackColor = true;
            this.btnBindKey.Click += new System.EventHandler(this.btnBindKey_Click);
            // 
            // editKeyBinding
            // 
            this.editKeyBinding.Location = new System.Drawing.Point(79, 411);
            this.editKeyBinding.Name = "editKeyBinding";
            this.editKeyBinding.ReadOnly = true;
            this.editKeyBinding.Size = new System.Drawing.Size(204, 20);
            this.editKeyBinding.TabIndex = 3;
            this.editKeyBinding.Text = "Press Key here";
            this.editKeyBinding.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.editKeyBinding_PreviewKeyDown);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 414);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(66, 13);
            this.label10.TabIndex = 2;
            this.label10.Text = "Key Binding:";
            // 
            // listFunctions
            // 
            this.listFunctions.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.listFunctions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader8});
            this.listFunctions.FullRowSelect = true;
            this.listFunctions.HideSelection = false;
            this.listFunctions.Location = new System.Drawing.Point(3, 22);
            this.listFunctions.MultiSelect = false;
            this.listFunctions.Name = "listFunctions";
            this.listFunctions.Size = new System.Drawing.Size(676, 383);
            this.listFunctions.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listFunctions.TabIndex = 1;
            this.listFunctions.UseCompatibleStateImageBehavior = false;
            this.listFunctions.View = System.Windows.Forms.View.Details;
            this.listFunctions.SelectedIndexChanged += new System.EventHandler(this.listFunctions_SelectedIndexChanged);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "State";
            this.columnHeader4.Width = 126;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Function";
            this.columnHeader1.Width = 304;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Binding";
            this.columnHeader2.Width = 113;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "2nd Binding";
            this.columnHeader8.Width = 113;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 7);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Functions:";
            // 
            // tabErrorsWarnings
            // 
            this.tabErrorsWarnings.Controls.Add(this.comboASMEncoding);
            this.tabErrorsWarnings.Controls.Add(this.btmASMLibraryPathBrowse);
            this.tabErrorsWarnings.Controls.Add(this.editASMLibraryPath);
            this.tabErrorsWarnings.Controls.Add(this.checkASMShowAddress);
            this.tabErrorsWarnings.Controls.Add(this.checkASMShowAutoComplete);
            this.tabErrorsWarnings.Controls.Add(this.checkASMAutoTruncateLiteralValues);
            this.tabErrorsWarnings.Controls.Add(this.checkASMShowMiniMap);
            this.tabErrorsWarnings.Controls.Add(this.checkASMShowSizes);
            this.tabErrorsWarnings.Controls.Add(this.checkASMShowCycles);
            this.tabErrorsWarnings.Controls.Add(this.checkASMShowLineNumbers);
            this.tabErrorsWarnings.Controls.Add(this.label30);
            this.tabErrorsWarnings.Controls.Add(this.label26);
            this.tabErrorsWarnings.Controls.Add(this.asmLibraryPathList);
            this.tabErrorsWarnings.Controls.Add(this.listHacks);
            this.tabErrorsWarnings.Controls.Add(this.label35);
            this.tabErrorsWarnings.Controls.Add(this.label34);
            this.tabErrorsWarnings.Controls.Add(this.listWarningsAsErrors);
            this.tabErrorsWarnings.Controls.Add(this.label33);
            this.tabErrorsWarnings.Controls.Add(this.listIgnoredWarnings);
            this.tabErrorsWarnings.Controls.Add(this.label20);
            this.tabErrorsWarnings.Location = new System.Drawing.Point(4, 22);
            this.tabErrorsWarnings.Name = "tabErrorsWarnings";
            this.tabErrorsWarnings.Size = new System.Drawing.Size(689, 437);
            this.tabErrorsWarnings.TabIndex = 4;
            this.tabErrorsWarnings.Text = "Assembler";
            this.tabErrorsWarnings.UseVisualStyleBackColor = true;
            // 
            // comboASMEncoding
            // 
            this.comboASMEncoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboASMEncoding.FormattingEnabled = true;
            this.comboASMEncoding.Location = new System.Drawing.Point(390, 349);
            this.comboASMEncoding.Name = "comboASMEncoding";
            this.comboASMEncoding.Size = new System.Drawing.Size(261, 21);
            this.comboASMEncoding.TabIndex = 23;
            this.comboASMEncoding.SelectedIndexChanged += new System.EventHandler(this.comboASMEncoding_SelectedIndexChanged);
            // 
            // btmASMLibraryPathBrowse
            // 
            this.btmASMLibraryPathBrowse.Location = new System.Drawing.Point(342, 396);
            this.btmASMLibraryPathBrowse.Name = "btmASMLibraryPathBrowse";
            this.btmASMLibraryPathBrowse.Size = new System.Drawing.Size(35, 20);
            this.btmASMLibraryPathBrowse.TabIndex = 22;
            this.btmASMLibraryPathBrowse.Text = "...";
            this.btmASMLibraryPathBrowse.UseVisualStyleBackColor = true;
            this.btmASMLibraryPathBrowse.Click += new System.EventHandler(this.btmASMLibraryPathBrowse_Click);
            // 
            // editASMLibraryPath
            // 
            this.editASMLibraryPath.Location = new System.Drawing.Point(21, 397);
            this.editASMLibraryPath.Name = "editASMLibraryPath";
            this.editASMLibraryPath.Size = new System.Drawing.Size(315, 20);
            this.editASMLibraryPath.TabIndex = 21;
            // 
            // checkASMShowAddress
            // 
            this.checkASMShowAddress.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkASMShowAddress.Location = new System.Drawing.Point(21, 160);
            this.checkASMShowAddress.Name = "checkASMShowAddress";
            this.checkASMShowAddress.Size = new System.Drawing.Size(214, 24);
            this.checkASMShowAddress.TabIndex = 19;
            this.checkASMShowAddress.Text = "Show Address";
            this.checkASMShowAddress.UseVisualStyleBackColor = true;
            this.checkASMShowAddress.CheckedChanged += new System.EventHandler(this.checkASMShowAddress_CheckedChanged);
            // 
            // checkASMShowAutoComplete
            // 
            this.checkASMShowAutoComplete.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkASMShowAutoComplete.Location = new System.Drawing.Point(21, 138);
            this.checkASMShowAutoComplete.Name = "checkASMShowAutoComplete";
            this.checkASMShowAutoComplete.Size = new System.Drawing.Size(214, 24);
            this.checkASMShowAutoComplete.TabIndex = 19;
            this.checkASMShowAutoComplete.Text = "Show Auto-Complete";
            this.checkASMShowAutoComplete.UseVisualStyleBackColor = true;
            this.checkASMShowAutoComplete.CheckedChanged += new System.EventHandler(this.checkASMShowAutoComplete_CheckedChanged);
            // 
            // checkASMAutoTruncateLiteralValues
            // 
            this.checkASMAutoTruncateLiteralValues.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkASMAutoTruncateLiteralValues.Location = new System.Drawing.Point(21, 116);
            this.checkASMAutoTruncateLiteralValues.Name = "checkASMAutoTruncateLiteralValues";
            this.checkASMAutoTruncateLiteralValues.Size = new System.Drawing.Size(214, 24);
            this.checkASMAutoTruncateLiteralValues.TabIndex = 18;
            this.checkASMAutoTruncateLiteralValues.Text = "Truncate literal values";
            this.checkASMAutoTruncateLiteralValues.UseVisualStyleBackColor = true;
            this.checkASMAutoTruncateLiteralValues.CheckedChanged += new System.EventHandler(this.checkASMAutoTruncateLiteralValues_CheckedChanged);
            // 
            // checkASMShowMiniMap
            // 
            this.checkASMShowMiniMap.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkASMShowMiniMap.Location = new System.Drawing.Point(21, 93);
            this.checkASMShowMiniMap.Name = "checkASMShowMiniMap";
            this.checkASMShowMiniMap.Size = new System.Drawing.Size(214, 24);
            this.checkASMShowMiniMap.TabIndex = 17;
            this.checkASMShowMiniMap.Text = "Show Mini View";
            this.checkASMShowMiniMap.UseVisualStyleBackColor = true;
            this.checkASMShowMiniMap.CheckedChanged += new System.EventHandler(this.checkASMShowMiniView_CheckedChanged);
            // 
            // checkASMShowSizes
            // 
            this.checkASMShowSizes.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkASMShowSizes.Location = new System.Drawing.Point(21, 70);
            this.checkASMShowSizes.Name = "checkASMShowSizes";
            this.checkASMShowSizes.Size = new System.Drawing.Size(214, 24);
            this.checkASMShowSizes.TabIndex = 16;
            this.checkASMShowSizes.Text = "Show Sizes";
            this.checkASMShowSizes.UseVisualStyleBackColor = true;
            this.checkASMShowSizes.CheckedChanged += new System.EventHandler(this.checkASMShowSizes_CheckedChanged);
            // 
            // checkASMShowCycles
            // 
            this.checkASMShowCycles.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkASMShowCycles.Location = new System.Drawing.Point(21, 47);
            this.checkASMShowCycles.Name = "checkASMShowCycles";
            this.checkASMShowCycles.Size = new System.Drawing.Size(214, 24);
            this.checkASMShowCycles.TabIndex = 15;
            this.checkASMShowCycles.Text = "Show Cycles";
            this.checkASMShowCycles.UseVisualStyleBackColor = true;
            this.checkASMShowCycles.CheckedChanged += new System.EventHandler(this.checkASMShowCycles_CheckedChanged);
            // 
            // checkASMShowLineNumbers
            // 
            this.checkASMShowLineNumbers.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkASMShowLineNumbers.Location = new System.Drawing.Point(21, 23);
            this.checkASMShowLineNumbers.Name = "checkASMShowLineNumbers";
            this.checkASMShowLineNumbers.Size = new System.Drawing.Size(214, 24);
            this.checkASMShowLineNumbers.TabIndex = 14;
            this.checkASMShowLineNumbers.Text = "Show Line Numbers";
            this.checkASMShowLineNumbers.UseVisualStyleBackColor = true;
            this.checkASMShowLineNumbers.CheckedChanged += new System.EventHandler(this.checkASMShowLineNumbers_CheckedChanged);
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(9, 219);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(71, 13);
            this.label30.TabIndex = 13;
            this.label30.Text = "Library Paths:";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(9, 7);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(88, 13);
            this.label26.TabIndex = 13;
            this.label26.Text = "Assembler Editor:";
            // 
            // listHacks
            // 
            this.listHacks.CheckOnClick = true;
            this.listHacks.FormattingEnabled = true;
            this.listHacks.Location = new System.Drawing.Point(390, 240);
            this.listHacks.Name = "listHacks";
            this.listHacks.Size = new System.Drawing.Size(261, 79);
            this.listHacks.TabIndex = 3;
            this.listHacks.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listC64StudioHacks_ItemCheck);
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(387, 333);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(55, 13);
            this.label35.TabIndex = 2;
            this.label35.Text = "Encoding:";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(387, 219);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(207, 13);
            this.label34.TabIndex = 2;
            this.label34.Text = "Enable Hacks (C64Studio assembler only):";
            // 
            // listWarningsAsErrors
            // 
            this.listWarningsAsErrors.CheckOnClick = true;
            this.listWarningsAsErrors.FormattingEnabled = true;
            this.listWarningsAsErrors.Location = new System.Drawing.Point(390, 127);
            this.listWarningsAsErrors.Name = "listWarningsAsErrors";
            this.listWarningsAsErrors.Size = new System.Drawing.Size(261, 79);
            this.listWarningsAsErrors.TabIndex = 3;
            this.listWarningsAsErrors.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listWarningsAsErrors_ItemCheck);
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(387, 111);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(127, 13);
            this.label33.TabIndex = 2;
            this.label33.Text = "Treat Warnings as Errors:";
            // 
            // listIgnoredWarnings
            // 
            this.listIgnoredWarnings.CheckOnClick = true;
            this.listIgnoredWarnings.FormattingEnabled = true;
            this.listIgnoredWarnings.Location = new System.Drawing.Point(390, 23);
            this.listIgnoredWarnings.Name = "listIgnoredWarnings";
            this.listIgnoredWarnings.Size = new System.Drawing.Size(261, 79);
            this.listIgnoredWarnings.TabIndex = 3;
            this.listIgnoredWarnings.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listIgnoredWarnings_ItemCheck);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(387, 7);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(88, 13);
            this.label20.TabIndex = 2;
            this.label20.Text = "Ignore Warnings:";
            // 
            // tabColors
            // 
            this.tabColors.Controls.Add(this.btnSetDefaultsColors);
            this.tabColors.Controls.Add(this.btnChooseBG);
            this.tabColors.Controls.Add(this.btnChooseFG);
            this.tabColors.Controls.Add(this.panelElementPreview);
            this.tabColors.Controls.Add(this.comboElementBG);
            this.tabColors.Controls.Add(this.comboElementFG);
            this.tabColors.Controls.Add(this.label27);
            this.tabColors.Controls.Add(this.label19);
            this.tabColors.Controls.Add(this.label18);
            this.tabColors.Controls.Add(this.label17);
            this.tabColors.Controls.Add(this.listColoring);
            this.tabColors.Controls.Add(this.label16);
            this.tabColors.Location = new System.Drawing.Point(4, 22);
            this.tabColors.Name = "tabColors";
            this.tabColors.Size = new System.Drawing.Size(689, 437);
            this.tabColors.TabIndex = 3;
            this.tabColors.Text = "Colors";
            this.tabColors.UseVisualStyleBackColor = true;
            // 
            // btnSetDefaultsColors
            // 
            this.btnSetDefaultsColors.Location = new System.Drawing.Point(558, 409);
            this.btnSetDefaultsColors.Name = "btnSetDefaultsColors";
            this.btnSetDefaultsColors.Size = new System.Drawing.Size(124, 23);
            this.btnSetDefaultsColors.TabIndex = 8;
            this.btnSetDefaultsColors.Text = "Set Defaults";
            this.btnSetDefaultsColors.UseVisualStyleBackColor = true;
            this.btnSetDefaultsColors.Click += new System.EventHandler(this.btnSetDefaultsColors_Click);
            // 
            // btnChooseBG
            // 
            this.btnChooseBG.Location = new System.Drawing.Point(522, 67);
            this.btnChooseBG.Name = "btnChooseBG";
            this.btnChooseBG.Size = new System.Drawing.Size(75, 23);
            this.btnChooseBG.TabIndex = 7;
            this.btnChooseBG.Text = "Choose...";
            this.btnChooseBG.UseVisualStyleBackColor = true;
            this.btnChooseBG.Click += new System.EventHandler(this.btnChooseBG_Click);
            // 
            // btnChooseFG
            // 
            this.btnChooseFG.Location = new System.Drawing.Point(522, 21);
            this.btnChooseFG.Name = "btnChooseFG";
            this.btnChooseFG.Size = new System.Drawing.Size(75, 23);
            this.btnChooseFG.TabIndex = 7;
            this.btnChooseFG.Text = "Choose...";
            this.btnChooseFG.UseVisualStyleBackColor = true;
            this.btnChooseFG.Click += new System.EventHandler(this.btnChooseFG_Click);
            // 
            // panelElementPreview
            // 
            this.panelElementPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelElementPreview.Location = new System.Drawing.Point(273, 122);
            this.panelElementPreview.Name = "panelElementPreview";
            this.panelElementPreview.Size = new System.Drawing.Size(243, 64);
            this.panelElementPreview.TabIndex = 6;
            this.panelElementPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.panelElementPreview_Paint);
            // 
            // comboElementBG
            // 
            this.comboElementBG.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboElementBG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboElementBG.FormattingEnabled = true;
            this.comboElementBG.Location = new System.Drawing.Point(273, 69);
            this.comboElementBG.Name = "comboElementBG";
            this.comboElementBG.Size = new System.Drawing.Size(243, 21);
            this.comboElementBG.TabIndex = 5;
            this.comboElementBG.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboElementBG_DrawItem);
            this.comboElementBG.SelectedIndexChanged += new System.EventHandler(this.comboElementBG_SelectedIndexChanged);
            // 
            // comboElementFG
            // 
            this.comboElementFG.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboElementFG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboElementFG.FormattingEnabled = true;
            this.comboElementFG.Location = new System.Drawing.Point(273, 23);
            this.comboElementFG.Name = "comboElementFG";
            this.comboElementFG.Size = new System.Drawing.Size(243, 21);
            this.comboElementFG.TabIndex = 5;
            this.comboElementFG.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboElementFG_DrawItem);
            this.comboElementFG.SelectedIndexChanged += new System.EventHandler(this.comboElementFG_SelectedIndexChanged);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(270, 211);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(281, 13);
            this.label27.TabIndex = 4;
            this.label27.Text = "\"Auto\" takes the corresponding color from \"Empty Space\"";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(270, 106);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(48, 13);
            this.label19.TabIndex = 4;
            this.label19.Text = "Preview:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(270, 53);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(109, 13);
            this.label18.TabIndex = 4;
            this.label18.Text = "Element Background:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(270, 7);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(105, 13);
            this.label17.TabIndex = 4;
            this.label17.Text = "Element Foreground:";
            // 
            // listColoring
            // 
            this.listColoring.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.listColoring.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3});
            this.listColoring.FullRowSelect = true;
            this.listColoring.HideSelection = false;
            this.listColoring.Location = new System.Drawing.Point(12, 23);
            this.listColoring.Name = "listColoring";
            this.listColoring.Size = new System.Drawing.Size(252, 409);
            this.listColoring.TabIndex = 3;
            this.listColoring.UseCompatibleStateImageBehavior = false;
            this.listColoring.View = System.Windows.Forms.View.Details;
            this.listColoring.SelectedIndexChanged += new System.EventHandler(this.listColoring_SelectedIndexChanged);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Item";
            this.columnHeader3.Width = 240;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(9, 7);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(39, 13);
            this.label16.TabIndex = 2;
            this.label16.Text = "Colors:";
            // 
            // tabKeyMaps
            // 
            this.tabKeyMaps.Controls.Add(this.btnUnbindBASICKeyMapBinding);
            this.tabKeyMaps.Controls.Add(this.btnBindBASICKeyMapBinding);
            this.tabKeyMaps.Controls.Add(this.editBASICKeyMapBinding);
            this.tabKeyMaps.Controls.Add(this.label25);
            this.tabKeyMaps.Controls.Add(this.listBASICKeyMap);
            this.tabKeyMaps.Controls.Add(this.label21);
            this.tabKeyMaps.Location = new System.Drawing.Point(4, 22);
            this.tabKeyMaps.Name = "tabKeyMaps";
            this.tabKeyMaps.Size = new System.Drawing.Size(689, 437);
            this.tabKeyMaps.TabIndex = 5;
            this.tabKeyMaps.Text = "BASIC Key Maps";
            this.tabKeyMaps.UseVisualStyleBackColor = true;
            // 
            // btnUnbindBASICKeyMapBinding
            // 
            this.btnUnbindBASICKeyMapBinding.Enabled = false;
            this.btnUnbindBASICKeyMapBinding.Location = new System.Drawing.Point(374, 377);
            this.btnUnbindBASICKeyMapBinding.Name = "btnUnbindBASICKeyMapBinding";
            this.btnUnbindBASICKeyMapBinding.Size = new System.Drawing.Size(75, 23);
            this.btnUnbindBASICKeyMapBinding.TabIndex = 7;
            this.btnUnbindBASICKeyMapBinding.Text = "Unbind Key";
            this.btnUnbindBASICKeyMapBinding.UseVisualStyleBackColor = true;
            this.btnUnbindBASICKeyMapBinding.Click += new System.EventHandler(this.btnUnbindBASICKeyMapBinding_Click);
            // 
            // btnBindBASICKeyMapBinding
            // 
            this.btnBindBASICKeyMapBinding.Location = new System.Drawing.Point(293, 377);
            this.btnBindBASICKeyMapBinding.Name = "btnBindBASICKeyMapBinding";
            this.btnBindBASICKeyMapBinding.Size = new System.Drawing.Size(75, 23);
            this.btnBindBASICKeyMapBinding.TabIndex = 8;
            this.btnBindBASICKeyMapBinding.Text = "Bind Key";
            this.btnBindBASICKeyMapBinding.UseVisualStyleBackColor = true;
            this.btnBindBASICKeyMapBinding.Click += new System.EventHandler(this.btnBindBASICKeyMapBinding_Click);
            // 
            // editBASICKeyMapBinding
            // 
            this.editBASICKeyMapBinding.Location = new System.Drawing.Point(83, 379);
            this.editBASICKeyMapBinding.Name = "editBASICKeyMapBinding";
            this.editBASICKeyMapBinding.ReadOnly = true;
            this.editBASICKeyMapBinding.Size = new System.Drawing.Size(204, 20);
            this.editBASICKeyMapBinding.TabIndex = 6;
            this.editBASICKeyMapBinding.Text = "Press Key here";
            this.editBASICKeyMapBinding.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.editKeyMapBinding_PreviewKeyDown);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(11, 382);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(66, 13);
            this.label25.TabIndex = 5;
            this.label25.Text = "Key Binding:";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(8, 9);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(116, 13);
            this.label21.TabIndex = 2;
            this.label21.Text = "Applies to BASIC editor";
            // 
            // tabBASIC
            // 
            this.tabBASIC.Controls.Add(this.checkBASICAutoToggleEntryMode);
            this.tabBASIC.Controls.Add(this.checkBASICShowControlCodes);
            this.tabBASIC.Controls.Add(this.checkBASICStripREM);
            this.tabBASIC.Controls.Add(this.checkBASICStripSpaces);
            this.tabBASIC.Controls.Add(this.label22);
            this.tabBASIC.Location = new System.Drawing.Point(4, 22);
            this.tabBASIC.Name = "tabBASIC";
            this.tabBASIC.Padding = new System.Windows.Forms.Padding(3);
            this.tabBASIC.Size = new System.Drawing.Size(689, 437);
            this.tabBASIC.TabIndex = 6;
            this.tabBASIC.Text = "BASIC";
            this.tabBASIC.UseVisualStyleBackColor = true;
            // 
            // checkBASICAutoToggleEntryMode
            // 
            this.checkBASICAutoToggleEntryMode.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBASICAutoToggleEntryMode.Location = new System.Drawing.Point(23, 115);
            this.checkBASICAutoToggleEntryMode.Name = "checkBASICAutoToggleEntryMode";
            this.checkBASICAutoToggleEntryMode.Size = new System.Drawing.Size(266, 24);
            this.checkBASICAutoToggleEntryMode.TabIndex = 3;
            this.checkBASICAutoToggleEntryMode.Text = "Auto toggle entry mode on quotes (\")";
            this.checkBASICAutoToggleEntryMode.UseVisualStyleBackColor = true;
            this.checkBASICAutoToggleEntryMode.CheckedChanged += new System.EventHandler(this.checkBASICAutoToggleEntryMode_CheckedChanged);
            // 
            // checkBASICShowControlCodes
            // 
            this.checkBASICShowControlCodes.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBASICShowControlCodes.Location = new System.Drawing.Point(23, 85);
            this.checkBASICShowControlCodes.Name = "checkBASICShowControlCodes";
            this.checkBASICShowControlCodes.Size = new System.Drawing.Size(266, 24);
            this.checkBASICShowControlCodes.TabIndex = 3;
            this.checkBASICShowControlCodes.Text = "Show control codes as characters";
            this.checkBASICShowControlCodes.UseVisualStyleBackColor = true;
            this.checkBASICShowControlCodes.CheckedChanged += new System.EventHandler(this.checkBASICShowControlCodesAsChars_CheckedChanged);
            // 
            // checkBASICStripREM
            // 
            this.checkBASICStripREM.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBASICStripREM.Location = new System.Drawing.Point(23, 55);
            this.checkBASICStripREM.Name = "checkBASICStripREM";
            this.checkBASICStripREM.Size = new System.Drawing.Size(266, 24);
            this.checkBASICStripREM.TabIndex = 3;
            this.checkBASICStripREM.Text = "Strip REM statements";
            this.checkBASICStripREM.UseVisualStyleBackColor = true;
            this.checkBASICStripREM.CheckedChanged += new System.EventHandler(this.checkBASICStripREM_CheckedChanged);
            // 
            // checkBASICStripSpaces
            // 
            this.checkBASICStripSpaces.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBASICStripSpaces.Location = new System.Drawing.Point(23, 25);
            this.checkBASICStripSpaces.Name = "checkBASICStripSpaces";
            this.checkBASICStripSpaces.Size = new System.Drawing.Size(266, 24);
            this.checkBASICStripSpaces.TabIndex = 3;
            this.checkBASICStripSpaces.Text = "Strip spaces between code";
            this.checkBASICStripSpaces.UseVisualStyleBackColor = true;
            this.checkBASICStripSpaces.CheckedChanged += new System.EventHandler(this.checkBASICStripSpaces_CheckedChanged);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(9, 7);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(81, 13);
            this.label22.TabIndex = 2;
            this.label22.Text = "Parser Settings:";
            // 
            // BtnMacros
            // 
            this.BtnMacros.Location = new System.Drawing.Point(410, 474);
            this.BtnMacros.Name = "BtnMacros";
            this.BtnMacros.Size = new System.Drawing.Size(75, 23);
            this.BtnMacros.TabIndex = 5;
            this.BtnMacros.Text = "Macros";
            this.BtnMacros.UseVisualStyleBackColor = true;
            this.BtnMacros.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnExportAllSettings
            // 
            this.btnExportAllSettings.Location = new System.Drawing.Point(202, 474);
            this.btnExportAllSettings.Name = "btnExportAllSettings";
            this.btnExportAllSettings.Size = new System.Drawing.Size(75, 23);
            this.btnExportAllSettings.TabIndex = 9;
            this.btnExportAllSettings.Text = "Export all";
            this.btnExportAllSettings.UseVisualStyleBackColor = true;
            this.btnExportAllSettings.Click += new System.EventHandler(this.btnExportAllSettings_Click);
            // 
            // btnImportAllSettings
            // 
            this.btnImportAllSettings.Location = new System.Drawing.Point(5, 474);
            this.btnImportAllSettings.Name = "btnImportAllSettings";
            this.btnImportAllSettings.Size = new System.Drawing.Size(75, 23);
            this.btnImportAllSettings.TabIndex = 9;
            this.btnImportAllSettings.Text = "Import all";
            this.btnImportAllSettings.UseVisualStyleBackColor = true;
            this.btnImportAllSettings.Click += new System.EventHandler(this.btnImportAllSettings_Click);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.Location = new System.Drawing.Point(610, 474);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "Close";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnImportCurrentPageSettings
            // 
            this.btnImportCurrentPageSettings.Location = new System.Drawing.Point(86, 474);
            this.btnImportCurrentPageSettings.Name = "btnImportCurrentPageSettings";
            this.btnImportCurrentPageSettings.Size = new System.Drawing.Size(75, 23);
            this.btnImportCurrentPageSettings.TabIndex = 9;
            this.btnImportCurrentPageSettings.Text = "Import here";
            this.btnImportCurrentPageSettings.UseVisualStyleBackColor = true;
            this.btnImportCurrentPageSettings.Click += new System.EventHandler(this.btnImportCurrentPageSettings_Click);
            // 
            // btnExportCurrentPageSettings
            // 
            this.btnExportCurrentPageSettings.Location = new System.Drawing.Point(283, 474);
            this.btnExportCurrentPageSettings.Name = "btnExportCurrentPageSettings";
            this.btnExportCurrentPageSettings.Size = new System.Drawing.Size(75, 23);
            this.btnExportCurrentPageSettings.TabIndex = 9;
            this.btnExportCurrentPageSettings.Text = "Export here";
            this.btnExportCurrentPageSettings.UseVisualStyleBackColor = true;
            this.btnExportCurrentPageSettings.Click += new System.EventHandler(this.btnExportCurrentPageSettings_Click);
            // 
            // btnClone
            // 
            this.btnClone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClone.Enabled = false;
            this.btnClone.Image = ((System.Drawing.Image)(resources.GetObject("btnClone.Image")));
            this.btnClone.Location = new System.Drawing.Point(257, 404);
            this.btnClone.Name = "btnClone";
            this.btnClone.Size = new System.Drawing.Size(29, 23);
            this.btnClone.TabIndex = 14;
            this.btnClone.UseVisualStyleBackColor = true;
            this.btnClone.Click += new System.EventHandler(this.btnClone_Click);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMoveDown.Enabled = false;
            this.btnMoveDown.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveDown.Image")));
            this.btnMoveDown.Location = new System.Drawing.Point(362, 404);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(29, 23);
            this.btnMoveDown.TabIndex = 17;
            this.btnMoveDown.UseVisualStyleBackColor = true;
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMoveUp.Enabled = false;
            this.btnMoveUp.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveUp.Image")));
            this.btnMoveUp.Location = new System.Drawing.Point(327, 404);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(29, 23);
            this.btnMoveUp.TabIndex = 16;
            this.btnMoveUp.UseVisualStyleBackColor = true;
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Enabled = false;
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.Location = new System.Drawing.Point(292, 404);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(29, 23);
            this.btnDelete.TabIndex = 15;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Enabled = false;
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.Location = new System.Drawing.Point(222, 404);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(29, 23);
            this.btnAdd.TabIndex = 13;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // alistTools
            // 
            this.alistTools.AddButtonEnabled = true;
            this.alistTools.AllowClone = true;
            this.alistTools.DeleteButtonEnabled = false;
            this.alistTools.HasOwnerDrawColumn = true;
            this.alistTools.HighlightColor = System.Drawing.SystemColors.HotTrack;
            this.alistTools.HighlightTextColor = System.Drawing.SystemColors.HighlightText;
            this.alistTools.Location = new System.Drawing.Point(9, 29);
            this.alistTools.MoveDownButtonEnabled = false;
            this.alistTools.MoveUpButtonEnabled = false;
            this.alistTools.MustHaveOneElement = false;
            this.alistTools.Name = "alistTools";
            this.alistTools.SelectedIndex = -1;
            this.alistTools.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.alistTools.SelectionTextColor = System.Drawing.SystemColors.HighlightText;
            this.alistTools.Size = new System.Drawing.Size(184, 402);
            this.alistTools.TabIndex = 11;
            this.alistTools.AddingItem += new RetroDevStudio.Controls.ArrangedItemList.AddingItemEventHandler(this.alistTools_AddingItem);
            this.alistTools.CloningItem += new RetroDevStudio.Controls.ArrangedItemList.CloningItemEventHandler(this.alistTools_CloningItem);
            this.alistTools.ItemAdded += new RetroDevStudio.Controls.ArrangedItemList.ItemModifiedEventHandler(this.alistTools_ItemAdded);
            this.alistTools.ItemRemoved += new RetroDevStudio.Controls.ArrangedItemList.ItemModifiedEventHandler(this.alistTools_ItemRemoved);
            this.alistTools.ItemMoved += new RetroDevStudio.Controls.ArrangedItemList.ItemExchangedEventHandler(this.alistTools_ItemMoved);
            this.alistTools.SelectedIndexChanged += new RetroDevStudio.Controls.ArrangedItemList.ItemModifiedEventHandler(this.alistTools_SelectedIndexChanged);
            // 
            // asmLibraryPathList
            // 
            this.asmLibraryPathList.AddButtonEnabled = true;
            this.asmLibraryPathList.AllowClone = true;
            this.asmLibraryPathList.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.asmLibraryPathList.DeleteButtonEnabled = false;
            this.asmLibraryPathList.HasOwnerDrawColumn = true;
            this.asmLibraryPathList.HighlightColor = System.Drawing.SystemColors.HotTrack;
            this.asmLibraryPathList.HighlightTextColor = System.Drawing.SystemColors.HighlightText;
            this.asmLibraryPathList.Location = new System.Drawing.Point(21, 240);
            this.asmLibraryPathList.Margin = new System.Windows.Forms.Padding(48, 22, 48, 22);
            this.asmLibraryPathList.MoveDownButtonEnabled = false;
            this.asmLibraryPathList.MoveUpButtonEnabled = false;
            this.asmLibraryPathList.MustHaveOneElement = false;
            this.asmLibraryPathList.Name = "asmLibraryPathList";
            this.asmLibraryPathList.SelectedIndex = -1;
            this.asmLibraryPathList.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.asmLibraryPathList.SelectionTextColor = System.Drawing.SystemColors.HighlightText;
            this.asmLibraryPathList.Size = new System.Drawing.Size(356, 154);
            this.asmLibraryPathList.TabIndex = 20;
            this.asmLibraryPathList.AddingItem += new RetroDevStudio.Controls.ArrangedItemList.AddingItemEventHandler(this.asmLibraryPathList_AddingItem);
            this.asmLibraryPathList.ItemAdded += new RetroDevStudio.Controls.ArrangedItemList.ItemModifiedEventHandler(this.asmLibraryPathList_ItemAdded);
            this.asmLibraryPathList.ItemRemoved += new RetroDevStudio.Controls.ArrangedItemList.ItemModifiedEventHandler(this.asmLibraryPathList_ItemRemoved);
            this.asmLibraryPathList.ItemMoved += new RetroDevStudio.Controls.ArrangedItemList.ItemExchangedEventHandler(this.asmLibraryPathList_ItemMoved);
            // 
            // listBASICKeyMap
            // 
            this.listBASICKeyMap.AllowDrop = true;
            this.listBASICKeyMap.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.listBASICKeyMap.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader10});
            this.listBASICKeyMap.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBASICKeyMap.FullRowSelect = true;
            this.listBASICKeyMap.HideSelection = false;
            this.listBASICKeyMap.ItemHeight = 14;
            this.listBASICKeyMap.Location = new System.Drawing.Point(11, 25);
            this.listBASICKeyMap.MultiSelect = false;
            this.listBASICKeyMap.Name = "listBASICKeyMap";
            this.listBASICKeyMap.Size = new System.Drawing.Size(655, 348);
            this.listBASICKeyMap.TabIndex = 4;
            this.listBASICKeyMap.UseCompatibleStateImageBehavior = false;
            this.listBASICKeyMap.View = System.Windows.Forms.View.Details;
            this.listBASICKeyMap.SelectedIndexChanged += new System.EventHandler(this.listBASICKeyMap_SelectedIndexChanged);
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "C64 Key";
            this.columnHeader5.Width = 145;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "PETSCII";
            this.columnHeader6.Width = 62;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "PC Key";
            this.columnHeader7.Width = 309;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Display";
            this.columnHeader10.Width = 108;
            // 
            // Settings
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnOK;
            this.ClientSize = new System.Drawing.Size(697, 509);
            this.Controls.Add(this.BtnMacros);
            this.Controls.Add(this.btnExportCurrentPageSettings);
            this.Controls.Add(this.btnExportAllSettings);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnImportCurrentPageSettings);
            this.Controls.Add(this.btnImportAllSettings);
            this.Controls.Add(this.tabPreferences);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Preferences";
            this.tabPreferences.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            this.tabTools.ResumeLayout(false);
            this.tabTools.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DgvArguments)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabKeyBindings.ResumeLayout(false);
            this.tabKeyBindings.PerformLayout();
            this.tabErrorsWarnings.ResumeLayout(false);
            this.tabErrorsWarnings.PerformLayout();
            this.tabColors.ResumeLayout(false);
            this.tabColors.PerformLayout();
            this.tabKeyMaps.ResumeLayout(false);
            this.tabKeyMaps.PerformLayout();
            this.tabBASIC.ResumeLayout(false);
            this.tabBASIC.PerformLayout();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl tabPreferences;
    private System.Windows.Forms.TabPage tabTools;
    private System.Windows.Forms.TabPage tabKeyBindings;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label labelToolPath;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Button btnBrowseTool;
    private System.Windows.Forms.TextBox editToolProgramArguments;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox editToolDebugArguments;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.ComboBox comboToolType;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TextBox editToolName;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Button btnBrowseToolWorkPath;
    private System.Windows.Forms.TextBox editWorkPath;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.ListView listFunctions;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.TextBox editKeyBinding;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.Button btnBindKey;
    private System.Windows.Forms.TabPage tabGeneral;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.CheckBox checkPlaySoundCompileSuccessful;
    private System.Windows.Forms.CheckBox checkPlaySoundCompileFail;
    private System.Windows.Forms.TextBox editToolCartArguments;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Button BtnMacros;
    private System.Windows.Forms.TextBox editTabSize;
    private System.Windows.Forms.Label label14;
    private System.Windows.Forms.CheckBox checkConvertTabsToSpaces;
    private System.Windows.Forms.Label label13;
    private System.Windows.Forms.Button btnChooseFont;
    private System.Windows.Forms.Label labelFontPreview;
    private System.Windows.Forms.Label label15;
    private System.Windows.Forms.TabPage tabColors;
    private System.Windows.Forms.ListView listColoring;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.Label label16;
    private System.Windows.Forms.ComboBox comboElementFG;
    private System.Windows.Forms.Label label17;
    private System.Windows.Forms.ComboBox comboElementBG;
    private System.Windows.Forms.Label label18;
    private System.Windows.Forms.Panel panelElementPreview;
    private System.Windows.Forms.Label label19;
    private System.Windows.Forms.Button btnChooseBG;
    private System.Windows.Forms.Button btnChooseFG;
    private System.Windows.Forms.CheckBox checkPlaySoundSearchTextNotFound;
    private System.Windows.Forms.TabPage tabErrorsWarnings;
    private System.Windows.Forms.CheckedListBox listIgnoredWarnings;
    private System.Windows.Forms.Label label20;
    private System.Windows.Forms.Button btnUnbindKey;
    private System.Windows.Forms.ColumnHeader columnHeader4;
    private System.Windows.Forms.Button btnChangeBASICFont;
    private System.Windows.Forms.Label labelBASICFontPreview;
    private System.Windows.Forms.CheckBox checkBASICUseC64Font;
    private System.Windows.Forms.TabPage tabKeyMaps;
    private System.Windows.Forms.Label label21;
    private RetroDevStudio.Controls.MeasurableListView listBASICKeyMap;
    private System.Windows.Forms.ColumnHeader columnHeader5;
    private System.Windows.Forms.ColumnHeader columnHeader6;
    private System.Windows.Forms.ColumnHeader columnHeader7;
    private System.Windows.Forms.ColumnHeader columnHeader10;
    private System.Windows.Forms.Button btnUnbindBASICKeyMapBinding;
    private System.Windows.Forms.Button btnBindBASICKeyMapBinding;
    private System.Windows.Forms.TextBox editBASICKeyMapBinding;
    private System.Windows.Forms.Label label25;
    private System.Windows.Forms.TabPage tabBASIC;
    private System.Windows.Forms.CheckBox checkBASICStripSpaces;
    private System.Windows.Forms.Label label22;
    private System.Windows.Forms.Label label27;
    private System.Windows.Forms.CheckBox checkAutoOpenLastSolution;
    private System.Windows.Forms.Label label28;
    private System.Windows.Forms.Button btnSetDefaultsKeyBinding;
    private System.Windows.Forms.Button btnSetDefaultsColors;
    private System.Windows.Forms.Button btnSetDefaultsFont;
    private System.Windows.Forms.CheckBox checkPassLabelsToEmulator;
    private System.Windows.Forms.Button btnExportAllSettings;
    private System.Windows.Forms.Button btnImportAllSettings;
    private System.Windows.Forms.Button btnImportCurrentPageSettings;
    private System.Windows.Forms.Button btnExportCurrentPageSettings;
    private System.Windows.Forms.CheckBox checkBASICShowControlCodes;
    private System.Windows.Forms.ComboBox comboAppMode;
    private System.Windows.Forms.Label label29;
    private Controls.ArrangedItemList asmLibraryPathList;
    private System.Windows.Forms.CheckBox checkASMShowAutoComplete;
    private System.Windows.Forms.CheckBox checkASMAutoTruncateLiteralValues;
    private System.Windows.Forms.CheckBox checkASMShowMiniMap;
    private System.Windows.Forms.CheckBox checkASMShowSizes;
    private System.Windows.Forms.CheckBox checkASMShowCycles;
    private System.Windows.Forms.CheckBox checkASMShowLineNumbers;
    private System.Windows.Forms.Label label30;
    private System.Windows.Forms.Label label26;
    private System.Windows.Forms.Button btmASMLibraryPathBrowse;
    private System.Windows.Forms.TextBox editASMLibraryPath;
    private System.Windows.Forms.Button btnBrowseDefaultOpenSolutionPath;
    private System.Windows.Forms.TextBox editDefaultOpenSolutionPath;
    private System.Windows.Forms.Label label31;
    private System.Windows.Forms.CheckBox checkASMShowAddress;
    private System.Windows.Forms.Button btnBindKeySecondary;
    private System.Windows.Forms.ColumnHeader columnHeader8;
    private System.Windows.Forms.CheckBox checkBASICAutoToggleEntryMode;
    private System.Windows.Forms.CheckBox checkBASICStripREM;
    private System.Windows.Forms.CheckBox checkStripTrailingSpaces;
        private System.Windows.Forms.TextBox editMaxMRUEntries;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.CheckedListBox listWarningsAsErrors;
        private System.Windows.Forms.Label label33;
    private Controls.ArrangedItemList alistTools;
    private System.Windows.Forms.CheckedListBox listHacks;
    private System.Windows.Forms.Label label34;
    private System.Windows.Forms.ComboBox comboASMEncoding;
    private System.Windows.Forms.Label label35;
    private System.Windows.Forms.CheckBox checkRightClickIsBGColor;
        private System.Windows.Forms.TextBox editToolDiskArguments;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.TextBox editToolTapeArguments;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView DgvArguments;
        private Controls.CSButton btnClone;
        private Controls.CSButton btnMoveDown;
        private Controls.CSButton btnMoveUp;
        private Controls.CSButton btnDelete;
        private Controls.CSButton btnAdd;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColArgumentName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColArgumentValue;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColEnabled;
    }
}