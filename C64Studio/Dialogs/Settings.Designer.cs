namespace C64Studio
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
      this.tabPreferences = new System.Windows.Forms.TabControl();
      this.tabGeneral = new System.Windows.Forms.TabPage();
      this.comboAppMode = new System.Windows.Forms.ComboBox();
      this.btnSetDefaultsFont = new System.Windows.Forms.Button();
      this.checkBASICUseC64Font = new System.Windows.Forms.CheckBox();
      this.btnChangeBASICFont = new System.Windows.Forms.Button();
      this.labelBASICFontPreview = new System.Windows.Forms.Label();
      this.btnChooseFont = new System.Windows.Forms.Button();
      this.labelFontPreview = new System.Windows.Forms.Label();
      this.editTabSize = new System.Windows.Forms.TextBox();
      this.label14 = new System.Windows.Forms.Label();
      this.checkAutoOpenLastSolution = new System.Windows.Forms.CheckBox();
      this.checkAllowTabs = new System.Windows.Forms.CheckBox();
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
      this.checkPassLabelsToEmulator = new System.Windows.Forms.CheckBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.button1 = new System.Windows.Forms.Button();
      this.editToolCartArguments = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.label12 = new System.Windows.Forms.Label();
      this.editToolTrueDriveOffArguments = new System.Windows.Forms.TextBox();
      this.label24 = new System.Windows.Forms.Label();
      this.editToolTrueDriveOnArguments = new System.Windows.Forms.TextBox();
      this.label23 = new System.Windows.Forms.Label();
      this.editToolDebugArguments = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.editToolPRGArguments = new System.Windows.Forms.TextBox();
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
      this.btnDeleteTool = new System.Windows.Forms.Button();
      this.btnCloneTool = new System.Windows.Forms.Button();
      this.btnAddTool = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.listTools = new System.Windows.Forms.ListBox();
      this.tabKeyBindings = new System.Windows.Forms.TabPage();
      this.btnSetDefaultsKeyBinding = new System.Windows.Forms.Button();
      this.btnUnbindKey = new System.Windows.Forms.Button();
      this.btnBindKey = new System.Windows.Forms.Button();
      this.editKeyBinding = new System.Windows.Forms.TextBox();
      this.label10 = new System.Windows.Forms.Label();
      this.listFunctions = new System.Windows.Forms.ListView();
      this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.label9 = new System.Windows.Forms.Label();
      this.tabErrorsWarnings = new System.Windows.Forms.TabPage();
      this.asmLibraryPathList = new C64Studio.ArrangedItemList();
      this.checkASMShowAutoComplete = new System.Windows.Forms.CheckBox();
      this.checkASMAutoTruncateLiteralValues = new System.Windows.Forms.CheckBox();
      this.checkASMShowMiniMap = new System.Windows.Forms.CheckBox();
      this.checkASMShowSizes = new System.Windows.Forms.CheckBox();
      this.checkASMShowCycles = new System.Windows.Forms.CheckBox();
      this.checkASMShowLineNumbers = new System.Windows.Forms.CheckBox();
      this.label30 = new System.Windows.Forms.Label();
      this.label26 = new System.Windows.Forms.Label();
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
      this.listBASICKeyMap = new C64Studio.Controls.MeasurableListView();
      this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.label21 = new System.Windows.Forms.Label();
      this.tabBASIC = new System.Windows.Forms.TabPage();
      this.checkBASICShowControlCodes = new System.Windows.Forms.CheckBox();
      this.checkBASICStripSpaces = new System.Windows.Forms.CheckBox();
      this.label22 = new System.Windows.Forms.Label();
      this.btnExportAllSettings = new System.Windows.Forms.Button();
      this.btnImportAllSettings = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.btnImportCurrentPageSettings = new System.Windows.Forms.Button();
      this.btnExportCurrentPageSettings = new System.Windows.Forms.Button();
      this.tabPreferences.SuspendLayout();
      this.tabGeneral.SuspendLayout();
      this.tabTools.SuspendLayout();
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
      this.tabPreferences.Location = new System.Drawing.Point(-2, 8);
      this.tabPreferences.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tabPreferences.Name = "tabPreferences";
      this.tabPreferences.SelectedIndex = 0;
      this.tabPreferences.Size = new System.Drawing.Size(1046, 712);
      this.tabPreferences.TabIndex = 0;
      // 
      // tabGeneral
      // 
      this.tabGeneral.Controls.Add(this.comboAppMode);
      this.tabGeneral.Controls.Add(this.btnSetDefaultsFont);
      this.tabGeneral.Controls.Add(this.checkBASICUseC64Font);
      this.tabGeneral.Controls.Add(this.btnChangeBASICFont);
      this.tabGeneral.Controls.Add(this.labelBASICFontPreview);
      this.tabGeneral.Controls.Add(this.btnChooseFont);
      this.tabGeneral.Controls.Add(this.labelFontPreview);
      this.tabGeneral.Controls.Add(this.editTabSize);
      this.tabGeneral.Controls.Add(this.label14);
      this.tabGeneral.Controls.Add(this.checkAutoOpenLastSolution);
      this.tabGeneral.Controls.Add(this.checkAllowTabs);
      this.tabGeneral.Controls.Add(this.checkConvertTabsToSpaces);
      this.tabGeneral.Controls.Add(this.checkPlaySoundSearchTextNotFound);
      this.tabGeneral.Controls.Add(this.checkPlaySoundCompileSuccessful);
      this.tabGeneral.Controls.Add(this.checkPlaySoundCompileFail);
      this.tabGeneral.Controls.Add(this.label15);
      this.tabGeneral.Controls.Add(this.label28);
      this.tabGeneral.Controls.Add(this.label29);
      this.tabGeneral.Controls.Add(this.label13);
      this.tabGeneral.Controls.Add(this.label11);
      this.tabGeneral.Location = new System.Drawing.Point(4, 29);
      this.tabGeneral.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tabGeneral.Name = "tabGeneral";
      this.tabGeneral.Size = new System.Drawing.Size(1038, 679);
      this.tabGeneral.TabIndex = 2;
      this.tabGeneral.Text = "General";
      this.tabGeneral.UseVisualStyleBackColor = true;
      // 
      // comboAppMode
      // 
      this.comboAppMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboAppMode.FormattingEnabled = true;
      this.comboAppMode.Items.AddRange(new object[] {
            "Undecided",
            "Normal (settings in UserAppData)",
            "Portable Mode (settings local)"});
      this.comboAppMode.Location = new System.Drawing.Point(42, 349);
      this.comboAppMode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.comboAppMode.Name = "comboAppMode";
      this.comboAppMode.Size = new System.Drawing.Size(312, 28);
      this.comboAppMode.TabIndex = 6;
      this.comboAppMode.SelectedIndexChanged += new System.EventHandler(this.comboAppMode_SelectedIndexChanged);
      // 
      // btnSetDefaultsFont
      // 
      this.btnSetDefaultsFont.Location = new System.Drawing.Point(717, 442);
      this.btnSetDefaultsFont.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnSetDefaultsFont.Name = "btnSetDefaultsFont";
      this.btnSetDefaultsFont.Size = new System.Drawing.Size(186, 35);
      this.btnSetDefaultsFont.TabIndex = 15;
      this.btnSetDefaultsFont.Text = "Set Default Fonts";
      this.btnSetDefaultsFont.UseVisualStyleBackColor = true;
      this.btnSetDefaultsFont.Click += new System.EventHandler(this.btnSetDefaultsFont_Click);
      // 
      // checkBASICUseC64Font
      // 
      this.checkBASICUseC64Font.AutoSize = true;
      this.checkBASICUseC64Font.Location = new System.Drawing.Point(504, 528);
      this.checkBASICUseC64Font.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.checkBASICUseC64Font.Name = "checkBASICUseC64Font";
      this.checkBASICUseC64Font.Size = new System.Drawing.Size(122, 24);
      this.checkBASICUseC64Font.TabIndex = 9;
      this.checkBASICUseC64Font.Text = "Use C64 font";
      this.checkBASICUseC64Font.UseVisualStyleBackColor = true;
      this.checkBASICUseC64Font.CheckedChanged += new System.EventHandler(this.checkBASICUseC64Font_CheckedChanged);
      // 
      // btnChangeBASICFont
      // 
      this.btnChangeBASICFont.Enabled = false;
      this.btnChangeBASICFont.Location = new System.Drawing.Point(363, 522);
      this.btnChangeBASICFont.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnChangeBASICFont.Name = "btnChangeBASICFont";
      this.btnChangeBASICFont.Size = new System.Drawing.Size(132, 35);
      this.btnChangeBASICFont.TabIndex = 16;
      this.btnChangeBASICFont.Text = "Change Font";
      this.btnChangeBASICFont.UseVisualStyleBackColor = true;
      this.btnChangeBASICFont.Click += new System.EventHandler(this.btnChooseBASICFont_Click);
      // 
      // labelBASICFontPreview
      // 
      this.labelBASICFontPreview.Location = new System.Drawing.Point(40, 529);
      this.labelBASICFontPreview.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.labelBASICFontPreview.Name = "labelBASICFontPreview";
      this.labelBASICFontPreview.Size = new System.Drawing.Size(315, 95);
      this.labelBASICFontPreview.TabIndex = 7;
      this.labelBASICFontPreview.Text = "BASIC Font Preview";
      // 
      // btnChooseFont
      // 
      this.btnChooseFont.Location = new System.Drawing.Point(363, 442);
      this.btnChooseFont.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnChooseFont.Name = "btnChooseFont";
      this.btnChooseFont.Size = new System.Drawing.Size(132, 35);
      this.btnChooseFont.TabIndex = 14;
      this.btnChooseFont.Text = "Change Font";
      this.btnChooseFont.UseVisualStyleBackColor = true;
      this.btnChooseFont.Click += new System.EventHandler(this.btnChooseFont_Click);
      // 
      // labelFontPreview
      // 
      this.labelFontPreview.Location = new System.Drawing.Point(40, 449);
      this.labelFontPreview.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.labelFontPreview.Name = "labelFontPreview";
      this.labelFontPreview.Size = new System.Drawing.Size(315, 68);
      this.labelFontPreview.TabIndex = 7;
      this.labelFontPreview.Text = "Font Preview";
      // 
      // editTabSize
      // 
      this.editTabSize.Location = new System.Drawing.Point(224, 262);
      this.editTabSize.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.editTabSize.MaxLength = 1;
      this.editTabSize.Name = "editTabSize";
      this.editTabSize.Size = new System.Drawing.Size(130, 26);
      this.editTabSize.TabIndex = 5;
      this.editTabSize.TextChanged += new System.EventHandler(this.editTabSize_TextChanged);
      // 
      // label14
      // 
      this.label14.AutoSize = true;
      this.label14.Location = new System.Drawing.Point(38, 266);
      this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label14.Name = "label14";
      this.label14.Size = new System.Drawing.Size(75, 20);
      this.label14.TabIndex = 5;
      this.label14.Text = "Tab Size:";
      // 
      // checkAutoOpenLastSolution
      // 
      this.checkAutoOpenLastSolution.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkAutoOpenLastSolution.Location = new System.Drawing.Point(520, 317);
      this.checkAutoOpenLastSolution.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.checkAutoOpenLastSolution.Name = "checkAutoOpenLastSolution";
      this.checkAutoOpenLastSolution.Size = new System.Drawing.Size(321, 37);
      this.checkAutoOpenLastSolution.TabIndex = 13;
      this.checkAutoOpenLastSolution.Text = "Open last solution on startup";
      this.checkAutoOpenLastSolution.UseVisualStyleBackColor = true;
      this.checkAutoOpenLastSolution.CheckedChanged += new System.EventHandler(this.checkOpenLastSolution_CheckedChanged);
      // 
      // checkAllowTabs
      // 
      this.checkAllowTabs.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkAllowTabs.Location = new System.Drawing.Point(34, 192);
      this.checkAllowTabs.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.checkAllowTabs.Name = "checkAllowTabs";
      this.checkAllowTabs.Size = new System.Drawing.Size(321, 37);
      this.checkAllowTabs.TabIndex = 3;
      this.checkAllowTabs.Text = "Allow Tabs";
      this.checkAllowTabs.UseVisualStyleBackColor = true;
      this.checkAllowTabs.CheckedChanged += new System.EventHandler(this.checkAllowTabs_CheckedChanged);
      // 
      // checkConvertTabsToSpaces
      // 
      this.checkConvertTabsToSpaces.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkConvertTabsToSpaces.Location = new System.Drawing.Point(34, 228);
      this.checkConvertTabsToSpaces.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.checkConvertTabsToSpaces.Name = "checkConvertTabsToSpaces";
      this.checkConvertTabsToSpaces.Size = new System.Drawing.Size(321, 37);
      this.checkConvertTabsToSpaces.TabIndex = 4;
      this.checkConvertTabsToSpaces.Text = "Convert tabs to spaces";
      this.checkConvertTabsToSpaces.UseVisualStyleBackColor = true;
      this.checkConvertTabsToSpaces.CheckedChanged += new System.EventHandler(this.checkConvertTabsToSpaces_CheckedChanged);
      // 
      // checkPlaySoundSearchTextNotFound
      // 
      this.checkPlaySoundSearchTextNotFound.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkPlaySoundSearchTextNotFound.Location = new System.Drawing.Point(34, 109);
      this.checkPlaySoundSearchTextNotFound.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.checkPlaySoundSearchTextNotFound.Name = "checkPlaySoundSearchTextNotFound";
      this.checkPlaySoundSearchTextNotFound.Size = new System.Drawing.Size(321, 26);
      this.checkPlaySoundSearchTextNotFound.TabIndex = 2;
      this.checkPlaySoundSearchTextNotFound.Text = "Search Text not found";
      this.checkPlaySoundSearchTextNotFound.UseVisualStyleBackColor = true;
      // 
      // checkPlaySoundCompileSuccessful
      // 
      this.checkPlaySoundCompileSuccessful.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkPlaySoundCompileSuccessful.Location = new System.Drawing.Point(34, 74);
      this.checkPlaySoundCompileSuccessful.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.checkPlaySoundCompileSuccessful.Name = "checkPlaySoundCompileSuccessful";
      this.checkPlaySoundCompileSuccessful.Size = new System.Drawing.Size(321, 26);
      this.checkPlaySoundCompileSuccessful.TabIndex = 1;
      this.checkPlaySoundCompileSuccessful.Text = "Build Successful";
      this.checkPlaySoundCompileSuccessful.UseVisualStyleBackColor = true;
      // 
      // checkPlaySoundCompileFail
      // 
      this.checkPlaySoundCompileFail.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkPlaySoundCompileFail.Location = new System.Drawing.Point(34, 38);
      this.checkPlaySoundCompileFail.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.checkPlaySoundCompileFail.Name = "checkPlaySoundCompileFail";
      this.checkPlaySoundCompileFail.Size = new System.Drawing.Size(321, 26);
      this.checkPlaySoundCompileFail.TabIndex = 0;
      this.checkPlaySoundCompileFail.Text = "Build Failed";
      this.checkPlaySoundCompileFail.UseVisualStyleBackColor = true;
      // 
      // label15
      // 
      this.label15.AutoSize = true;
      this.label15.Location = new System.Drawing.Point(14, 417);
      this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label15.Name = "label15";
      this.label15.Size = new System.Drawing.Size(134, 20);
      this.label15.TabIndex = 2;
      this.label15.Text = "Text Editor Fonts:";
      // 
      // label28
      // 
      this.label28.AutoSize = true;
      this.label28.Location = new System.Drawing.Point(500, 292);
      this.label28.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label28.Name = "label28";
      this.label28.Size = new System.Drawing.Size(102, 20);
      this.label28.TabIndex = 2;
      this.label28.Text = "Environment:";
      // 
      // label29
      // 
      this.label29.AutoSize = true;
      this.label29.Location = new System.Drawing.Point(14, 325);
      this.label29.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label29.Name = "label29";
      this.label29.Size = new System.Drawing.Size(135, 20);
      this.label29.TabIndex = 2;
      this.label29.Text = "Application Mode:";
      // 
      // label13
      // 
      this.label13.AutoSize = true;
      this.label13.Location = new System.Drawing.Point(14, 168);
      this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label13.Name = "label13";
      this.label13.Size = new System.Drawing.Size(115, 20);
      this.label13.TabIndex = 2;
      this.label13.Text = "Tab Behaviour:";
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Location = new System.Drawing.Point(14, 11);
      this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(187, 20);
      this.label11.TabIndex = 2;
      this.label11.Text = "Sound Events, play when";
      // 
      // tabTools
      // 
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
      this.tabTools.Controls.Add(this.btnDeleteTool);
      this.tabTools.Controls.Add(this.btnCloneTool);
      this.tabTools.Controls.Add(this.btnAddTool);
      this.tabTools.Controls.Add(this.label2);
      this.tabTools.Controls.Add(this.label1);
      this.tabTools.Controls.Add(this.listTools);
      this.tabTools.Location = new System.Drawing.Point(4, 29);
      this.tabTools.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tabTools.Name = "tabTools";
      this.tabTools.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tabTools.Size = new System.Drawing.Size(1038, 679);
      this.tabTools.TabIndex = 0;
      this.tabTools.Text = "Tools";
      this.tabTools.UseVisualStyleBackColor = true;
      // 
      // checkPassLabelsToEmulator
      // 
      this.checkPassLabelsToEmulator.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkPassLabelsToEmulator.Location = new System.Drawing.Point(334, 520);
      this.checkPassLabelsToEmulator.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.checkPassLabelsToEmulator.Name = "checkPassLabelsToEmulator";
      this.checkPassLabelsToEmulator.Size = new System.Drawing.Size(544, 37);
      this.checkPassLabelsToEmulator.TabIndex = 9;
      this.checkPassLabelsToEmulator.Text = "Forward labels to emulator";
      this.checkPassLabelsToEmulator.UseVisualStyleBackColor = true;
      this.checkPassLabelsToEmulator.CheckedChanged += new System.EventHandler(this.checkPassLabelsToEmulator_CheckedChanged);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.button1);
      this.groupBox1.Controls.Add(this.editToolCartArguments);
      this.groupBox1.Controls.Add(this.label4);
      this.groupBox1.Controls.Add(this.label12);
      this.groupBox1.Controls.Add(this.editToolTrueDriveOffArguments);
      this.groupBox1.Controls.Add(this.label24);
      this.groupBox1.Controls.Add(this.editToolTrueDriveOnArguments);
      this.groupBox1.Controls.Add(this.label23);
      this.groupBox1.Controls.Add(this.editToolDebugArguments);
      this.groupBox1.Controls.Add(this.label5);
      this.groupBox1.Controls.Add(this.editToolPRGArguments);
      this.groupBox1.Location = new System.Drawing.Point(333, 215);
      this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.groupBox1.Size = new System.Drawing.Size(568, 295);
      this.groupBox1.TabIndex = 8;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Runtime Arguments";
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(24, 234);
      this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(112, 35);
      this.button1.TabIndex = 7;
      this.button1.Text = "Macros";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // editToolCartArguments
      // 
      this.editToolCartArguments.Location = new System.Drawing.Point(124, 69);
      this.editToolCartArguments.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.editToolCartArguments.Name = "editToolCartArguments";
      this.editToolCartArguments.Size = new System.Drawing.Size(420, 26);
      this.editToolCartArguments.TabIndex = 6;
      this.editToolCartArguments.TextChanged += new System.EventHandler(this.editToolCartArguments_TextChanged);
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(9, 34);
      this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(79, 20);
      this.label4.TabIndex = 3;
      this.label4.Text = "PRG/T64:";
      // 
      // label12
      // 
      this.label12.AutoSize = true;
      this.label12.Location = new System.Drawing.Point(9, 74);
      this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label12.Name = "label12";
      this.label12.Size = new System.Drawing.Size(78, 20);
      this.label12.TabIndex = 3;
      this.label12.Text = "Cartridge:";
      // 
      // editToolTrueDriveOffArguments
      // 
      this.editToolTrueDriveOffArguments.Location = new System.Drawing.Point(124, 189);
      this.editToolTrueDriveOffArguments.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.editToolTrueDriveOffArguments.Name = "editToolTrueDriveOffArguments";
      this.editToolTrueDriveOffArguments.Size = new System.Drawing.Size(420, 26);
      this.editToolTrueDriveOffArguments.TabIndex = 6;
      this.editToolTrueDriveOffArguments.TextChanged += new System.EventHandler(this.editToolTrueDriveOffArguments_TextChanged);
      // 
      // label24
      // 
      this.label24.AutoSize = true;
      this.label24.Location = new System.Drawing.Point(9, 194);
      this.label24.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label24.Name = "label24";
      this.label24.Size = new System.Drawing.Size(108, 20);
      this.label24.TabIndex = 3;
      this.label24.Text = "True Drive off:";
      // 
      // editToolTrueDriveOnArguments
      // 
      this.editToolTrueDriveOnArguments.Location = new System.Drawing.Point(124, 149);
      this.editToolTrueDriveOnArguments.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.editToolTrueDriveOnArguments.Name = "editToolTrueDriveOnArguments";
      this.editToolTrueDriveOnArguments.Size = new System.Drawing.Size(420, 26);
      this.editToolTrueDriveOnArguments.TabIndex = 6;
      this.editToolTrueDriveOnArguments.TextChanged += new System.EventHandler(this.editToolTrueDriveOnArguments_TextChanged);
      // 
      // label23
      // 
      this.label23.AutoSize = true;
      this.label23.Location = new System.Drawing.Point(9, 154);
      this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label23.Name = "label23";
      this.label23.Size = new System.Drawing.Size(107, 20);
      this.label23.TabIndex = 3;
      this.label23.Text = "True Drive on:";
      // 
      // editToolDebugArguments
      // 
      this.editToolDebugArguments.Location = new System.Drawing.Point(124, 109);
      this.editToolDebugArguments.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.editToolDebugArguments.Name = "editToolDebugArguments";
      this.editToolDebugArguments.Size = new System.Drawing.Size(420, 26);
      this.editToolDebugArguments.TabIndex = 6;
      this.editToolDebugArguments.TextChanged += new System.EventHandler(this.editToolDebugArguments_TextChanged);
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(9, 114);
      this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(61, 20);
      this.label5.TabIndex = 3;
      this.label5.Text = "Debug:";
      // 
      // editToolPRGArguments
      // 
      this.editToolPRGArguments.Location = new System.Drawing.Point(124, 29);
      this.editToolPRGArguments.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.editToolPRGArguments.Name = "editToolPRGArguments";
      this.editToolPRGArguments.Size = new System.Drawing.Size(420, 26);
      this.editToolPRGArguments.TabIndex = 6;
      this.editToolPRGArguments.TextChanged += new System.EventHandler(this.editToolPRGArguments_TextChanged);
      // 
      // comboToolType
      // 
      this.comboToolType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboToolType.FormattingEnabled = true;
      this.comboToolType.Location = new System.Drawing.Point(458, 89);
      this.comboToolType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.comboToolType.Name = "comboToolType";
      this.comboToolType.Size = new System.Drawing.Size(442, 28);
      this.comboToolType.TabIndex = 7;
      this.comboToolType.SelectedIndexChanged += new System.EventHandler(this.comboToolType_SelectedIndexChanged);
      // 
      // editToolName
      // 
      this.editToolName.Location = new System.Drawing.Point(458, 45);
      this.editToolName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.editToolName.Name = "editToolName";
      this.editToolName.Size = new System.Drawing.Size(442, 26);
      this.editToolName.TabIndex = 6;
      this.editToolName.TextChanged += new System.EventHandler(this.editToolName_TextChanged);
      // 
      // editWorkPath
      // 
      this.editWorkPath.Location = new System.Drawing.Point(458, 175);
      this.editWorkPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.editWorkPath.Name = "editWorkPath";
      this.editWorkPath.Size = new System.Drawing.Size(397, 26);
      this.editWorkPath.TabIndex = 6;
      this.editWorkPath.TextChanged += new System.EventHandler(this.editWorkPath_TextChanged);
      // 
      // btnBrowseToolWorkPath
      // 
      this.btnBrowseToolWorkPath.Location = new System.Drawing.Point(866, 172);
      this.btnBrowseToolWorkPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnBrowseToolWorkPath.Name = "btnBrowseToolWorkPath";
      this.btnBrowseToolWorkPath.Size = new System.Drawing.Size(36, 35);
      this.btnBrowseToolWorkPath.TabIndex = 5;
      this.btnBrowseToolWorkPath.Text = "...";
      this.btnBrowseToolWorkPath.UseVisualStyleBackColor = true;
      this.btnBrowseToolWorkPath.Click += new System.EventHandler(this.btnBrowseToolWorkPath_Click);
      // 
      // btnBrowseTool
      // 
      this.btnBrowseTool.Location = new System.Drawing.Point(866, 129);
      this.btnBrowseTool.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnBrowseTool.Name = "btnBrowseTool";
      this.btnBrowseTool.Size = new System.Drawing.Size(36, 35);
      this.btnBrowseTool.TabIndex = 5;
      this.btnBrowseTool.Text = "...";
      this.btnBrowseTool.UseVisualStyleBackColor = true;
      this.btnBrowseTool.Click += new System.EventHandler(this.btnBrowseTool_Click);
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(328, 49);
      this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(55, 20);
      this.label7.TabIndex = 3;
      this.label7.Text = "Name:";
      // 
      // labelToolPath
      // 
      this.labelToolPath.AutoEllipsis = true;
      this.labelToolPath.Location = new System.Drawing.Point(453, 137);
      this.labelToolPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.labelToolPath.Name = "labelToolPath";
      this.labelToolPath.Size = new System.Drawing.Size(404, 35);
      this.labelToolPath.TabIndex = 4;
      this.labelToolPath.Text = "Tool Path";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(328, 94);
      this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(47, 20);
      this.label6.TabIndex = 3;
      this.label6.Text = "Type:";
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(328, 180);
      this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(108, 20);
      this.label8.TabIndex = 3;
      this.label8.Text = "Working Path:";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(328, 137);
      this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(92, 20);
      this.label3.TabIndex = 3;
      this.label3.Text = "Executable:";
      // 
      // btnDeleteTool
      // 
      this.btnDeleteTool.Location = new System.Drawing.Point(231, 566);
      this.btnDeleteTool.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnDeleteTool.Name = "btnDeleteTool";
      this.btnDeleteTool.Size = new System.Drawing.Size(58, 35);
      this.btnDeleteTool.TabIndex = 2;
      this.btnDeleteTool.Text = "Del";
      this.btnDeleteTool.UseVisualStyleBackColor = true;
      this.btnDeleteTool.Click += new System.EventHandler(this.btnRemove_Click);
      // 
      // btnCloneTool
      // 
      this.btnCloneTool.Enabled = false;
      this.btnCloneTool.Location = new System.Drawing.Point(86, 566);
      this.btnCloneTool.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnCloneTool.Name = "btnCloneTool";
      this.btnCloneTool.Size = new System.Drawing.Size(63, 35);
      this.btnCloneTool.TabIndex = 2;
      this.btnCloneTool.Text = "Clone";
      this.btnCloneTool.UseVisualStyleBackColor = true;
      this.btnCloneTool.Click += new System.EventHandler(this.btnCloneTool_Click);
      // 
      // btnAddTool
      // 
      this.btnAddTool.Location = new System.Drawing.Point(14, 566);
      this.btnAddTool.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnAddTool.Name = "btnAddTool";
      this.btnAddTool.Size = new System.Drawing.Size(63, 35);
      this.btnAddTool.TabIndex = 2;
      this.btnAddTool.Text = "Add";
      this.btnAddTool.UseVisualStyleBackColor = true;
      this.btnAddTool.Click += new System.EventHandler(this.btnAddTool_Click);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(326, 11);
      this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(106, 20);
      this.label2.TabIndex = 1;
      this.label2.Text = "Tool Settings:";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(14, 11);
      this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(118, 20);
      this.label1.TabIndex = 1;
      this.label1.Text = "Available Tools:";
      // 
      // listTools
      // 
      this.listTools.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.listTools.FormattingEnabled = true;
      this.listTools.ItemHeight = 20;
      this.listTools.Location = new System.Drawing.Point(14, 49);
      this.listTools.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.listTools.Name = "listTools";
      this.listTools.Size = new System.Drawing.Size(274, 504);
      this.listTools.TabIndex = 0;
      this.listTools.SelectedIndexChanged += new System.EventHandler(this.listTools_SelectedIndexChanged);
      // 
      // tabKeyBindings
      // 
      this.tabKeyBindings.Controls.Add(this.btnSetDefaultsKeyBinding);
      this.tabKeyBindings.Controls.Add(this.btnUnbindKey);
      this.tabKeyBindings.Controls.Add(this.btnBindKey);
      this.tabKeyBindings.Controls.Add(this.editKeyBinding);
      this.tabKeyBindings.Controls.Add(this.label10);
      this.tabKeyBindings.Controls.Add(this.listFunctions);
      this.tabKeyBindings.Controls.Add(this.label9);
      this.tabKeyBindings.Location = new System.Drawing.Point(4, 29);
      this.tabKeyBindings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tabKeyBindings.Name = "tabKeyBindings";
      this.tabKeyBindings.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tabKeyBindings.Size = new System.Drawing.Size(1038, 679);
      this.tabKeyBindings.TabIndex = 1;
      this.tabKeyBindings.Text = "Key Bindings";
      this.tabKeyBindings.UseVisualStyleBackColor = true;
      // 
      // btnSetDefaultsKeyBinding
      // 
      this.btnSetDefaultsKeyBinding.Location = new System.Drawing.Point(837, 629);
      this.btnSetDefaultsKeyBinding.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnSetDefaultsKeyBinding.Name = "btnSetDefaultsKeyBinding";
      this.btnSetDefaultsKeyBinding.Size = new System.Drawing.Size(186, 35);
      this.btnSetDefaultsKeyBinding.TabIndex = 4;
      this.btnSetDefaultsKeyBinding.Text = "Set Defaults";
      this.btnSetDefaultsKeyBinding.UseVisualStyleBackColor = true;
      this.btnSetDefaultsKeyBinding.Click += new System.EventHandler(this.btnSetDefaultsKeyBinding_Click);
      // 
      // btnUnbindKey
      // 
      this.btnUnbindKey.Enabled = false;
      this.btnUnbindKey.Location = new System.Drawing.Point(555, 629);
      this.btnUnbindKey.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnUnbindKey.Name = "btnUnbindKey";
      this.btnUnbindKey.Size = new System.Drawing.Size(112, 35);
      this.btnUnbindKey.TabIndex = 4;
      this.btnUnbindKey.Text = "Unbind Key";
      this.btnUnbindKey.UseVisualStyleBackColor = true;
      this.btnUnbindKey.Click += new System.EventHandler(this.btnUnbindKey_Click);
      // 
      // btnBindKey
      // 
      this.btnBindKey.Location = new System.Drawing.Point(434, 629);
      this.btnBindKey.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnBindKey.Name = "btnBindKey";
      this.btnBindKey.Size = new System.Drawing.Size(112, 35);
      this.btnBindKey.TabIndex = 4;
      this.btnBindKey.Text = "Bind Key";
      this.btnBindKey.UseVisualStyleBackColor = true;
      this.btnBindKey.Click += new System.EventHandler(this.btnBindKey_Click);
      // 
      // editKeyBinding
      // 
      this.editKeyBinding.Location = new System.Drawing.Point(118, 632);
      this.editKeyBinding.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.editKeyBinding.Name = "editKeyBinding";
      this.editKeyBinding.ReadOnly = true;
      this.editKeyBinding.Size = new System.Drawing.Size(304, 26);
      this.editKeyBinding.TabIndex = 3;
      this.editKeyBinding.Text = "Press Key here";
      this.editKeyBinding.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.editKeyBinding_PreviewKeyDown);
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(10, 637);
      this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(96, 20);
      this.label10.TabIndex = 2;
      this.label10.Text = "Key Binding:";
      // 
      // listFunctions
      // 
      this.listFunctions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.listFunctions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader1,
            this.columnHeader2});
      this.listFunctions.FullRowSelect = true;
      this.listFunctions.HideSelection = false;
      this.listFunctions.Location = new System.Drawing.Point(10, 35);
      this.listFunctions.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.listFunctions.MultiSelect = false;
      this.listFunctions.Name = "listFunctions";
      this.listFunctions.Size = new System.Drawing.Size(1012, 587);
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
      this.columnHeader1.Width = 360;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Binding";
      this.columnHeader2.Width = 160;
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(14, 11);
      this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(83, 20);
      this.label9.TabIndex = 0;
      this.label9.Text = "Functions:";
      // 
      // tabErrorsWarnings
      // 
      this.tabErrorsWarnings.Controls.Add(this.asmLibraryPathList);
      this.tabErrorsWarnings.Controls.Add(this.checkASMShowAutoComplete);
      this.tabErrorsWarnings.Controls.Add(this.checkASMAutoTruncateLiteralValues);
      this.tabErrorsWarnings.Controls.Add(this.checkASMShowMiniMap);
      this.tabErrorsWarnings.Controls.Add(this.checkASMShowSizes);
      this.tabErrorsWarnings.Controls.Add(this.checkASMShowCycles);
      this.tabErrorsWarnings.Controls.Add(this.checkASMShowLineNumbers);
      this.tabErrorsWarnings.Controls.Add(this.label30);
      this.tabErrorsWarnings.Controls.Add(this.label26);
      this.tabErrorsWarnings.Controls.Add(this.listIgnoredWarnings);
      this.tabErrorsWarnings.Controls.Add(this.label20);
      this.tabErrorsWarnings.Location = new System.Drawing.Point(4, 29);
      this.tabErrorsWarnings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tabErrorsWarnings.Name = "tabErrorsWarnings";
      this.tabErrorsWarnings.Size = new System.Drawing.Size(1038, 679);
      this.tabErrorsWarnings.TabIndex = 4;
      this.tabErrorsWarnings.Text = "Assembler";
      this.tabErrorsWarnings.UseVisualStyleBackColor = true;
      // 
      // asmLibraryPathList
      // 
      this.asmLibraryPathList.AddButtonEnabled = true;
      this.asmLibraryPathList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.asmLibraryPathList.AutoSize = true;
      this.asmLibraryPathList.DeleteButtonEnabled = false;
      this.asmLibraryPathList.Location = new System.Drawing.Point(31, 369);
      this.asmLibraryPathList.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.asmLibraryPathList.MoveDownButtonEnabled = false;
      this.asmLibraryPathList.MoveUpButtonEnabled = false;
      this.asmLibraryPathList.MustHaveOneElement = false;
      this.asmLibraryPathList.Name = "asmLibraryPathList";
      this.asmLibraryPathList.Size = new System.Drawing.Size(865, 210);
      this.asmLibraryPathList.TabIndex = 20;
      this.asmLibraryPathList.AddingItem += new C64Studio.ArrangedItemList.AddingItemEventHandler(this.asmLibraryPathList_AddingItem);
      // 
      // checkASMShowAutoComplete
      // 
      this.checkASMShowAutoComplete.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkASMShowAutoComplete.Location = new System.Drawing.Point(31, 213);
      this.checkASMShowAutoComplete.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.checkASMShowAutoComplete.Name = "checkASMShowAutoComplete";
      this.checkASMShowAutoComplete.Size = new System.Drawing.Size(321, 37);
      this.checkASMShowAutoComplete.TabIndex = 19;
      this.checkASMShowAutoComplete.Text = "Show Auto-Complete";
      this.checkASMShowAutoComplete.UseVisualStyleBackColor = true;
      // 
      // checkASMAutoTruncateLiteralValues
      // 
      this.checkASMAutoTruncateLiteralValues.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkASMAutoTruncateLiteralValues.Location = new System.Drawing.Point(31, 178);
      this.checkASMAutoTruncateLiteralValues.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.checkASMAutoTruncateLiteralValues.Name = "checkASMAutoTruncateLiteralValues";
      this.checkASMAutoTruncateLiteralValues.Size = new System.Drawing.Size(321, 37);
      this.checkASMAutoTruncateLiteralValues.TabIndex = 18;
      this.checkASMAutoTruncateLiteralValues.Text = "Truncate literal values";
      this.checkASMAutoTruncateLiteralValues.UseVisualStyleBackColor = true;
      // 
      // checkASMShowMiniMap
      // 
      this.checkASMShowMiniMap.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkASMShowMiniMap.Location = new System.Drawing.Point(31, 143);
      this.checkASMShowMiniMap.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.checkASMShowMiniMap.Name = "checkASMShowMiniMap";
      this.checkASMShowMiniMap.Size = new System.Drawing.Size(321, 37);
      this.checkASMShowMiniMap.TabIndex = 17;
      this.checkASMShowMiniMap.Text = "Show Mini View";
      this.checkASMShowMiniMap.UseVisualStyleBackColor = true;
      // 
      // checkASMShowSizes
      // 
      this.checkASMShowSizes.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkASMShowSizes.Location = new System.Drawing.Point(31, 107);
      this.checkASMShowSizes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.checkASMShowSizes.Name = "checkASMShowSizes";
      this.checkASMShowSizes.Size = new System.Drawing.Size(321, 37);
      this.checkASMShowSizes.TabIndex = 16;
      this.checkASMShowSizes.Text = "Show Sizes";
      this.checkASMShowSizes.UseVisualStyleBackColor = true;
      // 
      // checkASMShowCycles
      // 
      this.checkASMShowCycles.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkASMShowCycles.Location = new System.Drawing.Point(31, 72);
      this.checkASMShowCycles.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.checkASMShowCycles.Name = "checkASMShowCycles";
      this.checkASMShowCycles.Size = new System.Drawing.Size(321, 37);
      this.checkASMShowCycles.TabIndex = 15;
      this.checkASMShowCycles.Text = "Show Cycles";
      this.checkASMShowCycles.UseVisualStyleBackColor = true;
      // 
      // checkASMShowLineNumbers
      // 
      this.checkASMShowLineNumbers.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkASMShowLineNumbers.Location = new System.Drawing.Point(31, 36);
      this.checkASMShowLineNumbers.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.checkASMShowLineNumbers.Name = "checkASMShowLineNumbers";
      this.checkASMShowLineNumbers.Size = new System.Drawing.Size(321, 37);
      this.checkASMShowLineNumbers.TabIndex = 14;
      this.checkASMShowLineNumbers.Text = "Show Line Numbers";
      this.checkASMShowLineNumbers.UseVisualStyleBackColor = true;
      // 
      // label30
      // 
      this.label30.AutoSize = true;
      this.label30.Location = new System.Drawing.Point(14, 337);
      this.label30.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label30.Name = "label30";
      this.label30.Size = new System.Drawing.Size(105, 20);
      this.label30.TabIndex = 13;
      this.label30.Text = "Library Paths:";
      // 
      // label26
      // 
      this.label26.AutoSize = true;
      this.label26.Location = new System.Drawing.Point(14, 11);
      this.label26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label26.Name = "label26";
      this.label26.Size = new System.Drawing.Size(134, 20);
      this.label26.TabIndex = 13;
      this.label26.Text = "Assembler Editor:";
      // 
      // listIgnoredWarnings
      // 
      this.listIgnoredWarnings.CheckOnClick = true;
      this.listIgnoredWarnings.FormattingEnabled = true;
      this.listIgnoredWarnings.Location = new System.Drawing.Point(484, 36);
      this.listIgnoredWarnings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.listIgnoredWarnings.Name = "listIgnoredWarnings";
      this.listIgnoredWarnings.Size = new System.Drawing.Size(390, 277);
      this.listIgnoredWarnings.TabIndex = 3;
      this.listIgnoredWarnings.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listIgnoredWarnings_ItemCheck);
      // 
      // label20
      // 
      this.label20.AutoSize = true;
      this.label20.Location = new System.Drawing.Point(480, 11);
      this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label20.Name = "label20";
      this.label20.Size = new System.Drawing.Size(130, 20);
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
      this.tabColors.Location = new System.Drawing.Point(4, 29);
      this.tabColors.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tabColors.Name = "tabColors";
      this.tabColors.Size = new System.Drawing.Size(1038, 679);
      this.tabColors.TabIndex = 3;
      this.tabColors.Text = "Colors";
      this.tabColors.UseVisualStyleBackColor = true;
      // 
      // btnSetDefaultsColors
      // 
      this.btnSetDefaultsColors.Location = new System.Drawing.Point(837, 629);
      this.btnSetDefaultsColors.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnSetDefaultsColors.Name = "btnSetDefaultsColors";
      this.btnSetDefaultsColors.Size = new System.Drawing.Size(186, 35);
      this.btnSetDefaultsColors.TabIndex = 8;
      this.btnSetDefaultsColors.Text = "Set Defaults";
      this.btnSetDefaultsColors.UseVisualStyleBackColor = true;
      this.btnSetDefaultsColors.Click += new System.EventHandler(this.btnSetDefaultsColors_Click);
      // 
      // btnChooseBG
      // 
      this.btnChooseBG.Location = new System.Drawing.Point(783, 103);
      this.btnChooseBG.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnChooseBG.Name = "btnChooseBG";
      this.btnChooseBG.Size = new System.Drawing.Size(112, 35);
      this.btnChooseBG.TabIndex = 7;
      this.btnChooseBG.Text = "Choose...";
      this.btnChooseBG.UseVisualStyleBackColor = true;
      this.btnChooseBG.Click += new System.EventHandler(this.btnChooseBG_Click);
      // 
      // btnChooseFG
      // 
      this.btnChooseFG.Location = new System.Drawing.Point(783, 32);
      this.btnChooseFG.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnChooseFG.Name = "btnChooseFG";
      this.btnChooseFG.Size = new System.Drawing.Size(112, 35);
      this.btnChooseFG.TabIndex = 7;
      this.btnChooseFG.Text = "Choose...";
      this.btnChooseFG.UseVisualStyleBackColor = true;
      this.btnChooseFG.Click += new System.EventHandler(this.btnChooseFG_Click);
      // 
      // panelElementPreview
      // 
      this.panelElementPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panelElementPreview.Location = new System.Drawing.Point(410, 188);
      this.panelElementPreview.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.panelElementPreview.Name = "panelElementPreview";
      this.panelElementPreview.Size = new System.Drawing.Size(364, 97);
      this.panelElementPreview.TabIndex = 6;
      this.panelElementPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.panelElementPreview_Paint);
      // 
      // comboElementBG
      // 
      this.comboElementBG.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboElementBG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboElementBG.FormattingEnabled = true;
      this.comboElementBG.Location = new System.Drawing.Point(410, 106);
      this.comboElementBG.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.comboElementBG.Name = "comboElementBG";
      this.comboElementBG.Size = new System.Drawing.Size(362, 27);
      this.comboElementBG.TabIndex = 5;
      this.comboElementBG.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboElementBG_DrawItem);
      this.comboElementBG.SelectedIndexChanged += new System.EventHandler(this.comboElementBG_SelectedIndexChanged);
      // 
      // comboElementFG
      // 
      this.comboElementFG.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboElementFG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboElementFG.FormattingEnabled = true;
      this.comboElementFG.Location = new System.Drawing.Point(410, 35);
      this.comboElementFG.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.comboElementFG.Name = "comboElementFG";
      this.comboElementFG.Size = new System.Drawing.Size(362, 27);
      this.comboElementFG.TabIndex = 5;
      this.comboElementFG.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboElementFG_DrawItem);
      this.comboElementFG.SelectedIndexChanged += new System.EventHandler(this.comboElementFG_SelectedIndexChanged);
      // 
      // label27
      // 
      this.label27.AutoSize = true;
      this.label27.Location = new System.Drawing.Point(405, 325);
      this.label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label27.Name = "label27";
      this.label27.Size = new System.Drawing.Size(415, 20);
      this.label27.TabIndex = 4;
      this.label27.Text = "\"Auto\" takes the corresponding color from \"Empty Space\"";
      // 
      // label19
      // 
      this.label19.AutoSize = true;
      this.label19.Location = new System.Drawing.Point(405, 163);
      this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label19.Name = "label19";
      this.label19.Size = new System.Drawing.Size(67, 20);
      this.label19.TabIndex = 4;
      this.label19.Text = "Preview:";
      // 
      // label18
      // 
      this.label18.AutoSize = true;
      this.label18.Location = new System.Drawing.Point(405, 82);
      this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label18.Name = "label18";
      this.label18.Size = new System.Drawing.Size(162, 20);
      this.label18.TabIndex = 4;
      this.label18.Text = "Element Background:";
      // 
      // label17
      // 
      this.label17.AutoSize = true;
      this.label17.Location = new System.Drawing.Point(405, 11);
      this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label17.Name = "label17";
      this.label17.Size = new System.Drawing.Size(159, 20);
      this.label17.TabIndex = 4;
      this.label17.Text = "Element Foreground:";
      // 
      // listColoring
      // 
      this.listColoring.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.listColoring.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3});
      this.listColoring.FullRowSelect = true;
      this.listColoring.Location = new System.Drawing.Point(18, 35);
      this.listColoring.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.listColoring.Name = "listColoring";
      this.listColoring.Size = new System.Drawing.Size(376, 413);
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
      this.label16.Location = new System.Drawing.Point(14, 11);
      this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label16.Name = "label16";
      this.label16.Size = new System.Drawing.Size(58, 20);
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
      this.tabKeyMaps.Location = new System.Drawing.Point(4, 29);
      this.tabKeyMaps.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tabKeyMaps.Name = "tabKeyMaps";
      this.tabKeyMaps.Size = new System.Drawing.Size(1038, 679);
      this.tabKeyMaps.TabIndex = 5;
      this.tabKeyMaps.Text = "BASIC Key Maps";
      this.tabKeyMaps.UseVisualStyleBackColor = true;
      // 
      // btnUnbindBASICKeyMapBinding
      // 
      this.btnUnbindBASICKeyMapBinding.Enabled = false;
      this.btnUnbindBASICKeyMapBinding.Location = new System.Drawing.Point(561, 580);
      this.btnUnbindBASICKeyMapBinding.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnUnbindBASICKeyMapBinding.Name = "btnUnbindBASICKeyMapBinding";
      this.btnUnbindBASICKeyMapBinding.Size = new System.Drawing.Size(112, 35);
      this.btnUnbindBASICKeyMapBinding.TabIndex = 7;
      this.btnUnbindBASICKeyMapBinding.Text = "Unbind Key";
      this.btnUnbindBASICKeyMapBinding.UseVisualStyleBackColor = true;
      this.btnUnbindBASICKeyMapBinding.Click += new System.EventHandler(this.btnUnbindBASICKeyMapBinding_Click);
      // 
      // btnBindBASICKeyMapBinding
      // 
      this.btnBindBASICKeyMapBinding.Location = new System.Drawing.Point(440, 580);
      this.btnBindBASICKeyMapBinding.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnBindBASICKeyMapBinding.Name = "btnBindBASICKeyMapBinding";
      this.btnBindBASICKeyMapBinding.Size = new System.Drawing.Size(112, 35);
      this.btnBindBASICKeyMapBinding.TabIndex = 8;
      this.btnBindBASICKeyMapBinding.Text = "Bind Key";
      this.btnBindBASICKeyMapBinding.UseVisualStyleBackColor = true;
      this.btnBindBASICKeyMapBinding.Click += new System.EventHandler(this.btnBindBASICKeyMapBinding_Click);
      // 
      // editBASICKeyMapBinding
      // 
      this.editBASICKeyMapBinding.Location = new System.Drawing.Point(124, 583);
      this.editBASICKeyMapBinding.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.editBASICKeyMapBinding.Name = "editBASICKeyMapBinding";
      this.editBASICKeyMapBinding.ReadOnly = true;
      this.editBASICKeyMapBinding.Size = new System.Drawing.Size(304, 26);
      this.editBASICKeyMapBinding.TabIndex = 6;
      this.editBASICKeyMapBinding.Text = "Press Key here";
      this.editBASICKeyMapBinding.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.editKeyMapBinding_PreviewKeyDown);
      // 
      // label25
      // 
      this.label25.AutoSize = true;
      this.label25.Location = new System.Drawing.Point(16, 588);
      this.label25.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label25.Name = "label25";
      this.label25.Size = new System.Drawing.Size(96, 20);
      this.label25.TabIndex = 5;
      this.label25.Text = "Key Binding:";
      // 
      // listBASICKeyMap
      // 
      this.listBASICKeyMap.AllowDrop = true;
      this.listBASICKeyMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.listBASICKeyMap.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader10});
      this.listBASICKeyMap.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.listBASICKeyMap.FullRowSelect = true;
      this.listBASICKeyMap.ItemHeight = 14;
      this.listBASICKeyMap.Location = new System.Drawing.Point(16, 38);
      this.listBASICKeyMap.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.listBASICKeyMap.MultiSelect = false;
      this.listBASICKeyMap.Name = "listBASICKeyMap";
      this.listBASICKeyMap.Size = new System.Drawing.Size(980, 533);
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
      // label21
      // 
      this.label21.AutoSize = true;
      this.label21.Location = new System.Drawing.Point(12, 14);
      this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label21.Name = "label21";
      this.label21.Size = new System.Drawing.Size(176, 20);
      this.label21.TabIndex = 2;
      this.label21.Text = "Applies to BASIC editor";
      // 
      // tabBASIC
      // 
      this.tabBASIC.Controls.Add(this.checkBASICShowControlCodes);
      this.tabBASIC.Controls.Add(this.checkBASICStripSpaces);
      this.tabBASIC.Controls.Add(this.label22);
      this.tabBASIC.Location = new System.Drawing.Point(4, 29);
      this.tabBASIC.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tabBASIC.Name = "tabBASIC";
      this.tabBASIC.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.tabBASIC.Size = new System.Drawing.Size(1038, 679);
      this.tabBASIC.TabIndex = 6;
      this.tabBASIC.Text = "BASIC";
      this.tabBASIC.UseVisualStyleBackColor = true;
      // 
      // checkBASICShowControlCodes
      // 
      this.checkBASICShowControlCodes.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkBASICShowControlCodes.Location = new System.Drawing.Point(34, 85);
      this.checkBASICShowControlCodes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.checkBASICShowControlCodes.Name = "checkBASICShowControlCodes";
      this.checkBASICShowControlCodes.Size = new System.Drawing.Size(399, 37);
      this.checkBASICShowControlCodes.TabIndex = 3;
      this.checkBASICShowControlCodes.Text = "Show control codes as characters";
      this.checkBASICShowControlCodes.UseVisualStyleBackColor = true;
      this.checkBASICShowControlCodes.CheckedChanged += new System.EventHandler(this.checkBASICShowControlCodesAsChars_CheckedChanged);
      // 
      // checkBASICStripSpaces
      // 
      this.checkBASICStripSpaces.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkBASICStripSpaces.Location = new System.Drawing.Point(34, 38);
      this.checkBASICStripSpaces.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.checkBASICStripSpaces.Name = "checkBASICStripSpaces";
      this.checkBASICStripSpaces.Size = new System.Drawing.Size(399, 37);
      this.checkBASICStripSpaces.TabIndex = 3;
      this.checkBASICStripSpaces.Text = "Strip spaces between code";
      this.checkBASICStripSpaces.UseVisualStyleBackColor = true;
      this.checkBASICStripSpaces.CheckedChanged += new System.EventHandler(this.checkBASICStripSpaces_CheckedChanged);
      // 
      // label22
      // 
      this.label22.AutoSize = true;
      this.label22.Location = new System.Drawing.Point(14, 11);
      this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label22.Name = "label22";
      this.label22.Size = new System.Drawing.Size(122, 20);
      this.label22.TabIndex = 2;
      this.label22.Text = "Parser Settings:";
      // 
      // btnExportAllSettings
      // 
      this.btnExportAllSettings.Location = new System.Drawing.Point(303, 729);
      this.btnExportAllSettings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnExportAllSettings.Name = "btnExportAllSettings";
      this.btnExportAllSettings.Size = new System.Drawing.Size(112, 35);
      this.btnExportAllSettings.TabIndex = 9;
      this.btnExportAllSettings.Text = "Export all";
      this.btnExportAllSettings.UseVisualStyleBackColor = true;
      this.btnExportAllSettings.Click += new System.EventHandler(this.btnExportAllSettings_Click);
      // 
      // btnImportAllSettings
      // 
      this.btnImportAllSettings.Location = new System.Drawing.Point(8, 729);
      this.btnImportAllSettings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnImportAllSettings.Name = "btnImportAllSettings";
      this.btnImportAllSettings.Size = new System.Drawing.Size(112, 35);
      this.btnImportAllSettings.TabIndex = 9;
      this.btnImportAllSettings.Text = "Import all";
      this.btnImportAllSettings.UseVisualStyleBackColor = true;
      this.btnImportAllSettings.Click += new System.EventHandler(this.btnImportAllSettings_Click);
      // 
      // btnOK
      // 
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnOK.Location = new System.Drawing.Point(915, 729);
      this.btnOK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(112, 35);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "Close";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // btnImportCurrentPageSettings
      // 
      this.btnImportCurrentPageSettings.Location = new System.Drawing.Point(129, 729);
      this.btnImportCurrentPageSettings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnImportCurrentPageSettings.Name = "btnImportCurrentPageSettings";
      this.btnImportCurrentPageSettings.Size = new System.Drawing.Size(112, 35);
      this.btnImportCurrentPageSettings.TabIndex = 9;
      this.btnImportCurrentPageSettings.Text = "Import here";
      this.btnImportCurrentPageSettings.UseVisualStyleBackColor = true;
      this.btnImportCurrentPageSettings.Click += new System.EventHandler(this.btnImportCurrentPageSettings_Click);
      // 
      // btnExportCurrentPageSettings
      // 
      this.btnExportCurrentPageSettings.Location = new System.Drawing.Point(424, 729);
      this.btnExportCurrentPageSettings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.btnExportCurrentPageSettings.Name = "btnExportCurrentPageSettings";
      this.btnExportCurrentPageSettings.Size = new System.Drawing.Size(112, 35);
      this.btnExportCurrentPageSettings.TabIndex = 9;
      this.btnExportCurrentPageSettings.Text = "Export here";
      this.btnExportCurrentPageSettings.UseVisualStyleBackColor = true;
      this.btnExportCurrentPageSettings.Click += new System.EventHandler(this.btnExportCurrentPageSettings_Click);
      // 
      // Settings
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnOK;
      this.ClientSize = new System.Drawing.Size(1046, 783);
      this.Controls.Add(this.btnExportCurrentPageSettings);
      this.Controls.Add(this.btnExportAllSettings);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnImportCurrentPageSettings);
      this.Controls.Add(this.btnImportAllSettings);
      this.Controls.Add(this.tabPreferences);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
    private System.Windows.Forms.ListBox listTools;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Button btnDeleteTool;
    private System.Windows.Forms.Button btnAddTool;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label labelToolPath;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Button btnBrowseTool;
    private System.Windows.Forms.TextBox editToolPRGArguments;
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
    private System.Windows.Forms.Button button1;
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
    private C64Studio.Controls.MeasurableListView listBASICKeyMap;
    private System.Windows.Forms.ColumnHeader columnHeader5;
    private System.Windows.Forms.ColumnHeader columnHeader6;
    private System.Windows.Forms.ColumnHeader columnHeader7;
    private System.Windows.Forms.ColumnHeader columnHeader10;
    private System.Windows.Forms.TextBox editToolTrueDriveOnArguments;
    private System.Windows.Forms.Label label23;
    private System.Windows.Forms.TextBox editToolTrueDriveOffArguments;
    private System.Windows.Forms.Label label24;
    private System.Windows.Forms.Button btnUnbindBASICKeyMapBinding;
    private System.Windows.Forms.Button btnBindBASICKeyMapBinding;
    private System.Windows.Forms.TextBox editBASICKeyMapBinding;
    private System.Windows.Forms.Label label25;
    private System.Windows.Forms.TabPage tabBASIC;
    private System.Windows.Forms.CheckBox checkBASICStripSpaces;
    private System.Windows.Forms.Label label22;
    private System.Windows.Forms.CheckBox checkAllowTabs;
    private System.Windows.Forms.Label label27;
    private System.Windows.Forms.CheckBox checkAutoOpenLastSolution;
    private System.Windows.Forms.Label label28;
    private System.Windows.Forms.Button btnSetDefaultsKeyBinding;
    private System.Windows.Forms.Button btnSetDefaultsColors;
    private System.Windows.Forms.Button btnSetDefaultsFont;
    private System.Windows.Forms.CheckBox checkPassLabelsToEmulator;
    private System.Windows.Forms.Button btnCloneTool;
    private System.Windows.Forms.Button btnExportAllSettings;
    private System.Windows.Forms.Button btnImportAllSettings;
    private System.Windows.Forms.Button btnImportCurrentPageSettings;
    private System.Windows.Forms.Button btnExportCurrentPageSettings;
    private System.Windows.Forms.CheckBox checkBASICShowControlCodes;
    private System.Windows.Forms.ComboBox comboAppMode;
    private System.Windows.Forms.Label label29;
    private ArrangedItemList asmLibraryPathList;
    private System.Windows.Forms.CheckBox checkASMShowAutoComplete;
    private System.Windows.Forms.CheckBox checkASMAutoTruncateLiteralValues;
    private System.Windows.Forms.CheckBox checkASMShowMiniMap;
    private System.Windows.Forms.CheckBox checkASMShowSizes;
    private System.Windows.Forms.CheckBox checkASMShowCycles;
    private System.Windows.Forms.CheckBox checkASMShowLineNumbers;
    private System.Windows.Forms.Label label30;
    private System.Windows.Forms.Label label26;
  }
}