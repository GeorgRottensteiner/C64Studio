namespace C64Studio
{
  partial class CharsetScreenEditor
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CharsetScreenEditor));
      GR.Image.FastImage fastImage1 = new GR.Image.FastImage();
      GR.Image.FastImage fastImage2 = new GR.Image.FastImage();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.importCharsetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveCharsetProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.closeCharsetProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.tabProject = new System.Windows.Forms.TabPage();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.btnDefaultLowerCase = new System.Windows.Forms.Button();
      this.btnDefaultUppercase = new System.Windows.Forms.Button();
      this.btnImportCharset = new System.Windows.Forms.Button();
      this.btnClearImportData = new System.Windows.Forms.Button();
      this.btnImportFromASM = new System.Windows.Forms.Button();
      this.btnImportFromFile = new System.Windows.Forms.Button();
      this.editDataImport = new System.Windows.Forms.TextBox();
      this.groupExport = new System.Windows.Forms.GroupBox();
      this.checkExportASMAsPetSCII = new System.Windows.Forms.CheckBox();
      this.editExportBASICLineOffset = new System.Windows.Forms.TextBox();
      this.editExportBASICLineNo = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.checkExportHex = new System.Windows.Forms.CheckBox();
      this.comboExportArea = new System.Windows.Forms.ComboBox();
      this.editAreaHeight = new System.Windows.Forms.TextBox();
      this.labelAreaHeight = new System.Windows.Forms.Label();
      this.editExportY = new System.Windows.Forms.TextBox();
      this.editAreaWidth = new System.Windows.Forms.TextBox();
      this.labelAreaY = new System.Windows.Forms.Label();
      this.labelAreaWidth = new System.Windows.Forms.Label();
      this.editExportX = new System.Windows.Forms.TextBox();
      this.labelAreaX = new System.Windows.Forms.Label();
      this.label8 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.comboExportData = new System.Windows.Forms.ComboBox();
      this.label5 = new System.Windows.Forms.Label();
      this.comboExportOrientation = new System.Windows.Forms.ComboBox();
      this.comboCharsetFiles = new System.Windows.Forms.ComboBox();
      this.comboBasicFiles = new System.Windows.Forms.ComboBox();
      this.editPrefix = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.editWrapByteCount = new System.Windows.Forms.TextBox();
      this.checkExportToDataWrap = new System.Windows.Forms.CheckBox();
      this.checkExportToDataIncludeRes = new System.Windows.Forms.CheckBox();
      this.btnExportCharset = new System.Windows.Forms.Button();
      this.editDataExport = new System.Windows.Forms.TextBox();
      this.btnExportToBASICData = new System.Windows.Forms.Button();
      this.btnExportToBasic = new System.Windows.Forms.Button();
      this.btnExportToFile = new System.Windows.Forms.Button();
      this.btnExportToData = new System.Windows.Forms.Button();
      this.tabEditor = new System.Windows.Forms.TabPage();
      this.checkShowGrid = new System.Windows.Forms.CheckBox();
      this.checkApplyColors = new System.Windows.Forms.CheckBox();
      this.checkApplyCharacter = new System.Windows.Forms.CheckBox();
      this.label9 = new System.Windows.Forms.Label();
      this.comboCharsetMode = new System.Windows.Forms.ComboBox();
      this.labelInfo = new System.Windows.Forms.Label();
      this.btnToolText = new System.Windows.Forms.RadioButton();
      this.btnToolSelect = new System.Windows.Forms.RadioButton();
      this.btnToolFill = new System.Windows.Forms.RadioButton();
      this.btnToolQuad = new System.Windows.Forms.RadioButton();
      this.btnToolRect = new System.Windows.Forms.RadioButton();
      this.btnToolEdit = new System.Windows.Forms.RadioButton();
      this.btnApplyScreenSize = new System.Windows.Forms.Button();
      this.editScreenHeight = new System.Windows.Forms.TextBox();
      this.editScreenWidth = new System.Windows.Forms.TextBox();
      this.screenVScroll = new System.Windows.Forms.VScrollBar();
      this.screenHScroll = new System.Windows.Forms.HScrollBar();
      this.label7 = new System.Windows.Forms.Label();
      this.labelBGColor4 = new System.Windows.Forms.Label();
      this.labelMColor2 = new System.Windows.Forms.Label();
      this.labelMColor1 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.panelCharacters = new GR.Forms.ImageListbox();
      this.comboBGColor4 = new System.Windows.Forms.ComboBox();
      this.comboMulticolor2 = new System.Windows.Forms.ComboBox();
      this.comboMulticolor1 = new System.Windows.Forms.ComboBox();
      this.comboBackground = new System.Windows.Forms.ComboBox();
      this.panelCharColors = new GR.Forms.FastPictureBox();
      this.pictureEditor = new GR.Forms.FastPictureBox();
      this.tabCharsetEditor = new System.Windows.Forms.TabControl();
      this.tabCharset = new System.Windows.Forms.TabPage();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.btnMoveSelectionToTarget = new System.Windows.Forms.Button();
      this.editMoveTargetIndex = new System.Windows.Forms.TextBox();
      this.label10 = new System.Windows.Forms.Label();
      this.panelCharsetDetails = new GR.Forms.ImageListbox();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.checkOverrideOriginalColorSettings = new System.Windows.Forms.CheckBox();
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      this.menuStrip1.SuspendLayout();
      this.tabProject.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.groupExport.SuspendLayout();
      this.tabEditor.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.panelCharColors)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureEditor)).BeginInit();
      this.tabCharsetEditor.SuspendLayout();
      this.tabCharset.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(964, 24);
      this.menuStrip1.TabIndex = 1;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importCharsetToolStripMenuItem,
            this.saveCharsetProjectToolStripMenuItem,
            this.closeCharsetProjectToolStripMenuItem});
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
      this.fileToolStripMenuItem.Text = "&Screen";
      // 
      // importCharsetToolStripMenuItem
      // 
      this.importCharsetToolStripMenuItem.Name = "importCharsetToolStripMenuItem";
      this.importCharsetToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
      this.importCharsetToolStripMenuItem.Text = "Import &Charset...";
      this.importCharsetToolStripMenuItem.Click += new System.EventHandler(this.importCharsetToolStripMenuItem_Click);
      // 
      // saveCharsetProjectToolStripMenuItem
      // 
      this.saveCharsetProjectToolStripMenuItem.Enabled = false;
      this.saveCharsetProjectToolStripMenuItem.Name = "saveCharsetProjectToolStripMenuItem";
      this.saveCharsetProjectToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
      this.saveCharsetProjectToolStripMenuItem.Text = "&Save Project";
      this.saveCharsetProjectToolStripMenuItem.Click += new System.EventHandler(this.saveCharsetProjectToolStripMenuItem_Click);
      // 
      // closeCharsetProjectToolStripMenuItem
      // 
      this.closeCharsetProjectToolStripMenuItem.Enabled = false;
      this.closeCharsetProjectToolStripMenuItem.Name = "closeCharsetProjectToolStripMenuItem";
      this.closeCharsetProjectToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
      this.closeCharsetProjectToolStripMenuItem.Text = "&Close Charset Project";
      this.closeCharsetProjectToolStripMenuItem.Click += new System.EventHandler(this.closeCharsetProjectToolStripMenuItem_Click);
      // 
      // tabProject
      // 
      this.tabProject.Controls.Add(this.groupBox1);
      this.tabProject.Controls.Add(this.groupExport);
      this.tabProject.Location = new System.Drawing.Point(4, 22);
      this.tabProject.Name = "tabProject";
      this.tabProject.Padding = new System.Windows.Forms.Padding(3);
      this.tabProject.Size = new System.Drawing.Size(956, 512);
      this.tabProject.TabIndex = 1;
      this.tabProject.Text = "Import/Export";
      this.tabProject.UseVisualStyleBackColor = true;
      // 
      // groupBox1
      // 
      this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox1.Controls.Add(this.btnDefaultLowerCase);
      this.groupBox1.Controls.Add(this.btnDefaultUppercase);
      this.groupBox1.Controls.Add(this.btnImportCharset);
      this.groupBox1.Controls.Add(this.btnClearImportData);
      this.groupBox1.Controls.Add(this.btnImportFromASM);
      this.groupBox1.Controls.Add(this.btnImportFromFile);
      this.groupBox1.Controls.Add(this.editDataImport);
      this.groupBox1.Location = new System.Drawing.Point(458, 6);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(490, 498);
      this.groupBox1.TabIndex = 4;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Import";
      // 
      // btnDefaultLowerCase
      // 
      this.btnDefaultLowerCase.Location = new System.Drawing.Point(129, 77);
      this.btnDefaultLowerCase.Name = "btnDefaultLowerCase";
      this.btnDefaultLowerCase.Size = new System.Drawing.Size(117, 23);
      this.btnDefaultLowerCase.TabIndex = 3;
      this.btnDefaultLowerCase.Text = "Default Lowercase";
      this.btnDefaultLowerCase.UseVisualStyleBackColor = true;
      this.btnDefaultLowerCase.Click += new System.EventHandler(this.btnDefaultLowerCase_Click);
      // 
      // btnDefaultUppercase
      // 
      this.btnDefaultUppercase.Location = new System.Drawing.Point(6, 77);
      this.btnDefaultUppercase.Name = "btnDefaultUppercase";
      this.btnDefaultUppercase.Size = new System.Drawing.Size(117, 23);
      this.btnDefaultUppercase.TabIndex = 4;
      this.btnDefaultUppercase.Text = "Default Uppercase";
      this.btnDefaultUppercase.UseVisualStyleBackColor = true;
      this.btnDefaultUppercase.Click += new System.EventHandler(this.btnDefaultUppercase_Click);
      // 
      // btnImportCharset
      // 
      this.btnImportCharset.Location = new System.Drawing.Point(6, 48);
      this.btnImportCharset.Name = "btnImportCharset";
      this.btnImportCharset.Size = new System.Drawing.Size(117, 23);
      this.btnImportCharset.TabIndex = 1;
      this.btnImportCharset.Text = "Charset...";
      this.btnImportCharset.UseVisualStyleBackColor = true;
      this.btnImportCharset.Click += new System.EventHandler(this.btnImportCharset_Click);
      // 
      // btnClearImportData
      // 
      this.btnClearImportData.Location = new System.Drawing.Point(129, 106);
      this.btnClearImportData.Name = "btnClearImportData";
      this.btnClearImportData.Size = new System.Drawing.Size(117, 23);
      this.btnClearImportData.TabIndex = 0;
      this.btnClearImportData.Text = "Clear";
      this.btnClearImportData.UseVisualStyleBackColor = true;
      this.btnClearImportData.Click += new System.EventHandler(this.btnClearImportData_Click);
      // 
      // btnImportFromASM
      // 
      this.btnImportFromASM.Location = new System.Drawing.Point(6, 106);
      this.btnImportFromASM.Name = "btnImportFromASM";
      this.btnImportFromASM.Size = new System.Drawing.Size(117, 23);
      this.btnImportFromASM.TabIndex = 0;
      this.btnImportFromASM.Text = "From ASM";
      this.btnImportFromASM.UseVisualStyleBackColor = true;
      this.btnImportFromASM.Click += new System.EventHandler(this.btnImportFromASM_Click);
      // 
      // btnImportFromFile
      // 
      this.btnImportFromFile.Location = new System.Drawing.Point(6, 19);
      this.btnImportFromFile.Name = "btnImportFromFile";
      this.btnImportFromFile.Size = new System.Drawing.Size(117, 23);
      this.btnImportFromFile.TabIndex = 0;
      this.btnImportFromFile.Text = "From File...";
      this.btnImportFromFile.UseVisualStyleBackColor = true;
      this.btnImportFromFile.Click += new System.EventHandler(this.btnImportFromFile_Click);
      // 
      // editDataImport
      // 
      this.editDataImport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.editDataImport.Location = new System.Drawing.Point(6, 143);
      this.editDataImport.Multiline = true;
      this.editDataImport.Name = "editDataImport";
      this.editDataImport.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.editDataImport.Size = new System.Drawing.Size(478, 349);
      this.editDataImport.TabIndex = 20;
      this.editDataImport.WordWrap = false;
      this.editDataImport.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editDataImport_KeyPress);
      // 
      // groupExport
      // 
      this.groupExport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.groupExport.Controls.Add(this.checkExportASMAsPetSCII);
      this.groupExport.Controls.Add(this.editExportBASICLineOffset);
      this.groupExport.Controls.Add(this.editExportBASICLineNo);
      this.groupExport.Controls.Add(this.label3);
      this.groupExport.Controls.Add(this.label4);
      this.groupExport.Controls.Add(this.checkExportHex);
      this.groupExport.Controls.Add(this.comboExportArea);
      this.groupExport.Controls.Add(this.editAreaHeight);
      this.groupExport.Controls.Add(this.labelAreaHeight);
      this.groupExport.Controls.Add(this.editExportY);
      this.groupExport.Controls.Add(this.editAreaWidth);
      this.groupExport.Controls.Add(this.labelAreaY);
      this.groupExport.Controls.Add(this.labelAreaWidth);
      this.groupExport.Controls.Add(this.editExportX);
      this.groupExport.Controls.Add(this.labelAreaX);
      this.groupExport.Controls.Add(this.label8);
      this.groupExport.Controls.Add(this.label6);
      this.groupExport.Controls.Add(this.comboExportData);
      this.groupExport.Controls.Add(this.label5);
      this.groupExport.Controls.Add(this.comboExportOrientation);
      this.groupExport.Controls.Add(this.comboCharsetFiles);
      this.groupExport.Controls.Add(this.comboBasicFiles);
      this.groupExport.Controls.Add(this.editPrefix);
      this.groupExport.Controls.Add(this.label2);
      this.groupExport.Controls.Add(this.editWrapByteCount);
      this.groupExport.Controls.Add(this.checkExportToDataWrap);
      this.groupExport.Controls.Add(this.checkExportToDataIncludeRes);
      this.groupExport.Controls.Add(this.btnExportCharset);
      this.groupExport.Controls.Add(this.editDataExport);
      this.groupExport.Controls.Add(this.btnExportToBASICData);
      this.groupExport.Controls.Add(this.btnExportToBasic);
      this.groupExport.Controls.Add(this.btnExportToFile);
      this.groupExport.Controls.Add(this.btnExportToData);
      this.groupExport.Location = new System.Drawing.Point(11, 6);
      this.groupExport.Name = "groupExport";
      this.groupExport.Size = new System.Drawing.Size(441, 498);
      this.groupExport.TabIndex = 3;
      this.groupExport.TabStop = false;
      this.groupExport.Text = "Export";
      // 
      // checkExportASMAsPetSCII
      // 
      this.checkExportASMAsPetSCII.AutoSize = true;
      this.checkExportASMAsPetSCII.Location = new System.Drawing.Point(264, 222);
      this.checkExportASMAsPetSCII.Name = "checkExportASMAsPetSCII";
      this.checkExportASMAsPetSCII.Size = new System.Drawing.Size(93, 17);
      this.checkExportASMAsPetSCII.TabIndex = 25;
      this.checkExportASMAsPetSCII.Text = "Prefer PetSCII";
      this.checkExportASMAsPetSCII.UseVisualStyleBackColor = true;
      // 
      // editExportBASICLineOffset
      // 
      this.editExportBASICLineOffset.Location = new System.Drawing.Point(341, 274);
      this.editExportBASICLineOffset.Name = "editExportBASICLineOffset";
      this.editExportBASICLineOffset.Size = new System.Drawing.Size(73, 20);
      this.editExportBASICLineOffset.TabIndex = 23;
      this.editExportBASICLineOffset.Text = "10";
      // 
      // editExportBASICLineNo
      // 
      this.editExportBASICLineNo.Location = new System.Drawing.Point(170, 274);
      this.editExportBASICLineNo.Name = "editExportBASICLineNo";
      this.editExportBASICLineNo.Size = new System.Drawing.Size(98, 20);
      this.editExportBASICLineNo.TabIndex = 24;
      this.editExportBASICLineNo.Text = "10";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(274, 277);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(61, 13);
      this.label3.TabIndex = 21;
      this.label3.Text = "Line Offset:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(117, 277);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(47, 13);
      this.label4.TabIndex = 22;
      this.label4.Text = "Line No:";
      // 
      // checkExportHex
      // 
      this.checkExportHex.AutoSize = true;
      this.checkExportHex.Checked = true;
      this.checkExportHex.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkExportHex.Location = new System.Drawing.Point(118, 222);
      this.checkExportHex.Name = "checkExportHex";
      this.checkExportHex.Size = new System.Drawing.Size(92, 17);
      this.checkExportHex.TabIndex = 16;
      this.checkExportHex.Text = "Export as Hex";
      this.checkExportHex.UseVisualStyleBackColor = true;
      // 
      // comboExportArea
      // 
      this.comboExportArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboExportArea.FormattingEnabled = true;
      this.comboExportArea.Location = new System.Drawing.Point(118, 50);
      this.comboExportArea.Name = "comboExportArea";
      this.comboExportArea.Size = new System.Drawing.Size(317, 21);
      this.comboExportArea.TabIndex = 1;
      this.comboExportArea.SelectedIndexChanged += new System.EventHandler(this.comboExportArea_SelectedIndexChanged);
      // 
      // editAreaHeight
      // 
      this.editAreaHeight.Location = new System.Drawing.Point(283, 103);
      this.editAreaHeight.Name = "editAreaHeight";
      this.editAreaHeight.Size = new System.Drawing.Size(54, 20);
      this.editAreaHeight.TabIndex = 5;
      // 
      // labelAreaHeight
      // 
      this.labelAreaHeight.AutoSize = true;
      this.labelAreaHeight.Location = new System.Drawing.Point(236, 106);
      this.labelAreaHeight.Name = "labelAreaHeight";
      this.labelAreaHeight.Size = new System.Drawing.Size(41, 13);
      this.labelAreaHeight.TabIndex = 7;
      this.labelAreaHeight.Text = "Height:";
      // 
      // editExportY
      // 
      this.editExportY.Location = new System.Drawing.Point(283, 77);
      this.editExportY.Name = "editExportY";
      this.editExportY.Size = new System.Drawing.Size(54, 20);
      this.editExportY.TabIndex = 3;
      // 
      // editAreaWidth
      // 
      this.editAreaWidth.Location = new System.Drawing.Point(159, 103);
      this.editAreaWidth.Name = "editAreaWidth";
      this.editAreaWidth.Size = new System.Drawing.Size(54, 20);
      this.editAreaWidth.TabIndex = 4;
      // 
      // labelAreaY
      // 
      this.labelAreaY.AutoSize = true;
      this.labelAreaY.Location = new System.Drawing.Point(236, 80);
      this.labelAreaY.Name = "labelAreaY";
      this.labelAreaY.Size = new System.Drawing.Size(17, 13);
      this.labelAreaY.TabIndex = 5;
      this.labelAreaY.Text = "Y:";
      // 
      // labelAreaWidth
      // 
      this.labelAreaWidth.AutoSize = true;
      this.labelAreaWidth.Location = new System.Drawing.Point(115, 106);
      this.labelAreaWidth.Name = "labelAreaWidth";
      this.labelAreaWidth.Size = new System.Drawing.Size(38, 13);
      this.labelAreaWidth.TabIndex = 6;
      this.labelAreaWidth.Text = "Width:";
      // 
      // editExportX
      // 
      this.editExportX.Location = new System.Drawing.Point(159, 77);
      this.editExportX.Name = "editExportX";
      this.editExportX.Size = new System.Drawing.Size(54, 20);
      this.editExportX.TabIndex = 2;
      // 
      // labelAreaX
      // 
      this.labelAreaX.AutoSize = true;
      this.labelAreaX.Location = new System.Drawing.Point(115, 80);
      this.labelAreaX.Name = "labelAreaX";
      this.labelAreaX.Size = new System.Drawing.Size(17, 13);
      this.labelAreaX.TabIndex = 4;
      this.labelAreaX.Text = "X:";
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(3, 53);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(65, 13);
      this.label8.TabIndex = 2;
      this.label8.Text = "Export Area:";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(3, 24);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(66, 13);
      this.label6.TabIndex = 0;
      this.label6.Text = "Export Data:";
      // 
      // comboExportData
      // 
      this.comboExportData.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboExportData.FormattingEnabled = true;
      this.comboExportData.Items.AddRange(new object[] {
            "characters, then colors",
            "characters only",
            "colors only",
            "colors, then characters"});
      this.comboExportData.Location = new System.Drawing.Point(118, 21);
      this.comboExportData.Name = "comboExportData";
      this.comboExportData.Size = new System.Drawing.Size(317, 21);
      this.comboExportData.TabIndex = 0;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(129, 248);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(61, 13);
      this.label5.TabIndex = 18;
      this.label5.Text = "Orientation:";
      // 
      // comboExportOrientation
      // 
      this.comboExportOrientation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboExportOrientation.FormattingEnabled = true;
      this.comboExportOrientation.Items.AddRange(new object[] {
            "row by row",
            "column by column"});
      this.comboExportOrientation.Location = new System.Drawing.Point(217, 243);
      this.comboExportOrientation.Name = "comboExportOrientation";
      this.comboExportOrientation.Size = new System.Drawing.Size(131, 21);
      this.comboExportOrientation.TabIndex = 19;
      // 
      // comboCharsetFiles
      // 
      this.comboCharsetFiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboCharsetFiles.FormattingEnabled = true;
      this.comboCharsetFiles.Location = new System.Drawing.Point(117, 170);
      this.comboCharsetFiles.Name = "comboCharsetFiles";
      this.comboCharsetFiles.Size = new System.Drawing.Size(160, 21);
      this.comboCharsetFiles.TabIndex = 9;
      // 
      // comboBasicFiles
      // 
      this.comboBasicFiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBasicFiles.FormattingEnabled = true;
      this.comboBasicFiles.Location = new System.Drawing.Point(117, 143);
      this.comboBasicFiles.Name = "comboBasicFiles";
      this.comboBasicFiles.Size = new System.Drawing.Size(160, 21);
      this.comboBasicFiles.TabIndex = 7;
      // 
      // editPrefix
      // 
      this.editPrefix.Location = new System.Drawing.Point(214, 197);
      this.editPrefix.Name = "editPrefix";
      this.editPrefix.Size = new System.Drawing.Size(43, 20);
      this.editPrefix.TabIndex = 12;
      this.editPrefix.Text = "!byte ";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(394, 200);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(32, 13);
      this.label2.TabIndex = 15;
      this.label2.Text = "bytes";
      // 
      // editWrapByteCount
      // 
      this.editWrapByteCount.Enabled = false;
      this.editWrapByteCount.Location = new System.Drawing.Point(347, 197);
      this.editWrapByteCount.Name = "editWrapByteCount";
      this.editWrapByteCount.Size = new System.Drawing.Size(41, 20);
      this.editWrapByteCount.TabIndex = 14;
      this.editWrapByteCount.Text = "40";
      // 
      // checkExportToDataWrap
      // 
      this.checkExportToDataWrap.AutoSize = true;
      this.checkExportToDataWrap.Location = new System.Drawing.Point(264, 199);
      this.checkExportToDataWrap.Name = "checkExportToDataWrap";
      this.checkExportToDataWrap.Size = new System.Drawing.Size(64, 17);
      this.checkExportToDataWrap.TabIndex = 13;
      this.checkExportToDataWrap.Text = "Wrap at";
      this.checkExportToDataWrap.UseVisualStyleBackColor = true;
      this.checkExportToDataWrap.CheckedChanged += new System.EventHandler(this.checkExportToDataWrap_CheckedChanged);
      // 
      // checkExportToDataIncludeRes
      // 
      this.checkExportToDataIncludeRes.AutoSize = true;
      this.checkExportToDataIncludeRes.Location = new System.Drawing.Point(118, 199);
      this.checkExportToDataIncludeRes.Name = "checkExportToDataIncludeRes";
      this.checkExportToDataIncludeRes.Size = new System.Drawing.Size(74, 17);
      this.checkExportToDataIncludeRes.TabIndex = 11;
      this.checkExportToDataIncludeRes.Text = "Prefix with";
      this.checkExportToDataIncludeRes.UseVisualStyleBackColor = true;
      this.checkExportToDataIncludeRes.CheckedChanged += new System.EventHandler(this.checkExportToDataIncludeRes_CheckedChanged);
      // 
      // btnExportCharset
      // 
      this.btnExportCharset.Location = new System.Drawing.Point(6, 168);
      this.btnExportCharset.Name = "btnExportCharset";
      this.btnExportCharset.Size = new System.Drawing.Size(106, 23);
      this.btnExportCharset.TabIndex = 8;
      this.btnExportCharset.Text = "to Charset...";
      this.btnExportCharset.UseVisualStyleBackColor = true;
      this.btnExportCharset.Click += new System.EventHandler(this.btnExportToCharset_Click);
      // 
      // editDataExport
      // 
      this.editDataExport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.editDataExport.Location = new System.Drawing.Point(6, 301);
      this.editDataExport.Multiline = true;
      this.editDataExport.Name = "editDataExport";
      this.editDataExport.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.editDataExport.Size = new System.Drawing.Size(429, 191);
      this.editDataExport.TabIndex = 20;
      this.editDataExport.WordWrap = false;
      this.editDataExport.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editDataExport_KeyPress);
      this.editDataExport.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.editDataExport_PreviewKeyDown);
      // 
      // btnExportToBASICData
      // 
      this.btnExportToBASICData.Location = new System.Drawing.Point(6, 272);
      this.btnExportToBASICData.Name = "btnExportToBASICData";
      this.btnExportToBASICData.Size = new System.Drawing.Size(106, 23);
      this.btnExportToBASICData.TabIndex = 6;
      this.btnExportToBASICData.Text = "to BASIC data";
      this.btnExportToBASICData.UseVisualStyleBackColor = true;
      this.btnExportToBASICData.Click += new System.EventHandler(this.btnExportToBASICData_Click);
      // 
      // btnExportToBasic
      // 
      this.btnExportToBasic.Location = new System.Drawing.Point(6, 141);
      this.btnExportToBasic.Name = "btnExportToBasic";
      this.btnExportToBasic.Size = new System.Drawing.Size(106, 23);
      this.btnExportToBasic.TabIndex = 6;
      this.btnExportToBasic.Text = "to Basic...";
      this.btnExportToBasic.UseVisualStyleBackColor = true;
      this.btnExportToBasic.Click += new System.EventHandler(this.btnExportToBasic_Click);
      // 
      // btnExportToFile
      // 
      this.btnExportToFile.Location = new System.Drawing.Point(6, 243);
      this.btnExportToFile.Name = "btnExportToFile";
      this.btnExportToFile.Size = new System.Drawing.Size(106, 23);
      this.btnExportToFile.TabIndex = 17;
      this.btnExportToFile.Text = "as binary file";
      this.btnExportToFile.UseVisualStyleBackColor = true;
      this.btnExportToFile.Click += new System.EventHandler(this.btnExportToFile_Click);
      // 
      // btnExportToData
      // 
      this.btnExportToData.Location = new System.Drawing.Point(6, 197);
      this.btnExportToData.Name = "btnExportToData";
      this.btnExportToData.Size = new System.Drawing.Size(106, 23);
      this.btnExportToData.TabIndex = 10;
      this.btnExportToData.Text = "as assembly source";
      this.btnExportToData.UseVisualStyleBackColor = true;
      this.btnExportToData.Click += new System.EventHandler(this.btnExportToData_Click);
      // 
      // tabEditor
      // 
      this.tabEditor.Controls.Add(this.checkOverrideOriginalColorSettings);
      this.tabEditor.Controls.Add(this.checkShowGrid);
      this.tabEditor.Controls.Add(this.checkApplyColors);
      this.tabEditor.Controls.Add(this.checkApplyCharacter);
      this.tabEditor.Controls.Add(this.label9);
      this.tabEditor.Controls.Add(this.comboCharsetMode);
      this.tabEditor.Controls.Add(this.labelInfo);
      this.tabEditor.Controls.Add(this.btnToolText);
      this.tabEditor.Controls.Add(this.btnToolSelect);
      this.tabEditor.Controls.Add(this.btnToolFill);
      this.tabEditor.Controls.Add(this.btnToolQuad);
      this.tabEditor.Controls.Add(this.btnToolRect);
      this.tabEditor.Controls.Add(this.btnToolEdit);
      this.tabEditor.Controls.Add(this.btnApplyScreenSize);
      this.tabEditor.Controls.Add(this.editScreenHeight);
      this.tabEditor.Controls.Add(this.editScreenWidth);
      this.tabEditor.Controls.Add(this.screenVScroll);
      this.tabEditor.Controls.Add(this.screenHScroll);
      this.tabEditor.Controls.Add(this.label7);
      this.tabEditor.Controls.Add(this.labelBGColor4);
      this.tabEditor.Controls.Add(this.labelMColor2);
      this.tabEditor.Controls.Add(this.labelMColor1);
      this.tabEditor.Controls.Add(this.label1);
      this.tabEditor.Controls.Add(this.panelCharacters);
      this.tabEditor.Controls.Add(this.comboBGColor4);
      this.tabEditor.Controls.Add(this.comboMulticolor2);
      this.tabEditor.Controls.Add(this.comboMulticolor1);
      this.tabEditor.Controls.Add(this.comboBackground);
      this.tabEditor.Controls.Add(this.panelCharColors);
      this.tabEditor.Controls.Add(this.pictureEditor);
      this.tabEditor.ImageKey = "(none)";
      this.tabEditor.Location = new System.Drawing.Point(4, 22);
      this.tabEditor.Name = "tabEditor";
      this.tabEditor.Padding = new System.Windows.Forms.Padding(3);
      this.tabEditor.Size = new System.Drawing.Size(956, 512);
      this.tabEditor.TabIndex = 0;
      this.tabEditor.Text = "Screen";
      this.tabEditor.UseVisualStyleBackColor = true;
      // 
      // checkShowGrid
      // 
      this.checkShowGrid.AutoSize = true;
      this.checkShowGrid.Location = new System.Drawing.Point(838, 33);
      this.checkShowGrid.Name = "checkShowGrid";
      this.checkShowGrid.Size = new System.Drawing.Size(75, 17);
      this.checkShowGrid.TabIndex = 37;
      this.checkShowGrid.Text = "Show Grid";
      this.checkShowGrid.UseVisualStyleBackColor = true;
      this.checkShowGrid.CheckedChanged += new System.EventHandler(this.checkShowGrid_CheckedChanged);
      // 
      // checkApplyColors
      // 
      this.checkApplyColors.Appearance = System.Windows.Forms.Appearance.Button;
      this.checkApplyColors.Checked = true;
      this.checkApplyColors.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkApplyColors.Image = global::C64Studio.Properties.Resources.charscreen_colors;
      this.checkApplyColors.Location = new System.Drawing.Point(198, 432);
      this.checkApplyColors.Name = "checkApplyColors";
      this.checkApplyColors.Size = new System.Drawing.Size(24, 24);
      this.checkApplyColors.TabIndex = 36;
      this.toolTip1.SetToolTip(this.checkApplyColors, "Affect Colors");
      this.checkApplyColors.UseVisualStyleBackColor = true;
      this.checkApplyColors.CheckedChanged += new System.EventHandler(this.checkApplyColors_CheckedChanged);
      // 
      // checkApplyCharacter
      // 
      this.checkApplyCharacter.Appearance = System.Windows.Forms.Appearance.Button;
      this.checkApplyCharacter.Checked = true;
      this.checkApplyCharacter.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkApplyCharacter.Image = global::C64Studio.Properties.Resources.charscreen_chars;
      this.checkApplyCharacter.Location = new System.Drawing.Point(168, 432);
      this.checkApplyCharacter.Name = "checkApplyCharacter";
      this.checkApplyCharacter.Size = new System.Drawing.Size(24, 24);
      this.checkApplyCharacter.TabIndex = 36;
      this.toolTip1.SetToolTip(this.checkApplyCharacter, "Affect Characters");
      this.checkApplyCharacter.UseVisualStyleBackColor = true;
      this.checkApplyCharacter.CheckedChanged += new System.EventHandler(this.checkApplyCharacter_CheckedChanged);
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(827, 6);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(37, 13);
      this.label9.TabIndex = 35;
      this.label9.Text = "Mode:";
      // 
      // comboCharsetMode
      // 
      this.comboCharsetMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboCharsetMode.FormattingEnabled = true;
      this.comboCharsetMode.Location = new System.Drawing.Point(870, 3);
      this.comboCharsetMode.Name = "comboCharsetMode";
      this.comboCharsetMode.Size = new System.Drawing.Size(83, 21);
      this.comboCharsetMode.TabIndex = 34;
      this.comboCharsetMode.SelectedIndexChanged += new System.EventHandler(this.comboCharsetMode_SelectedIndexChanged);
      // 
      // labelInfo
      // 
      this.labelInfo.Location = new System.Drawing.Point(337, 438);
      this.labelInfo.Name = "labelInfo";
      this.labelInfo.Size = new System.Drawing.Size(315, 45);
      this.labelInfo.TabIndex = 33;
      this.labelInfo.Text = "Pos: 0,0  Offset: $0000\r\nline 2";
      // 
      // btnToolText
      // 
      this.btnToolText.Appearance = System.Windows.Forms.Appearance.Button;
      this.btnToolText.Image = ((System.Drawing.Image)(resources.GetObject("btnToolText.Image")));
      this.btnToolText.Location = new System.Drawing.Point(128, 432);
      this.btnToolText.Name = "btnToolText";
      this.btnToolText.Size = new System.Drawing.Size(24, 24);
      this.btnToolText.TabIndex = 32;
      this.toolTip1.SetToolTip(this.btnToolText, "Direct Text Entry");
      this.btnToolText.UseVisualStyleBackColor = true;
      this.btnToolText.CheckedChanged += new System.EventHandler(this.btnToolText_CheckedChanged);
      // 
      // btnToolSelect
      // 
      this.btnToolSelect.Appearance = System.Windows.Forms.Appearance.Button;
      this.btnToolSelect.Image = global::C64Studio.Properties.Resources.tool_select;
      this.btnToolSelect.Location = new System.Drawing.Point(104, 432);
      this.btnToolSelect.Name = "btnToolSelect";
      this.btnToolSelect.Size = new System.Drawing.Size(24, 24);
      this.btnToolSelect.TabIndex = 32;
      this.toolTip1.SetToolTip(this.btnToolSelect, "Selection");
      this.btnToolSelect.UseVisualStyleBackColor = true;
      this.btnToolSelect.CheckedChanged += new System.EventHandler(this.btnToolSelect_CheckedChanged);
      // 
      // btnToolFill
      // 
      this.btnToolFill.Appearance = System.Windows.Forms.Appearance.Button;
      this.btnToolFill.Image = global::C64Studio.Properties.Resources.tool_fill;
      this.btnToolFill.Location = new System.Drawing.Point(80, 432);
      this.btnToolFill.Name = "btnToolFill";
      this.btnToolFill.Size = new System.Drawing.Size(24, 24);
      this.btnToolFill.TabIndex = 32;
      this.toolTip1.SetToolTip(this.btnToolFill, "Fill");
      this.btnToolFill.UseVisualStyleBackColor = true;
      this.btnToolFill.CheckedChanged += new System.EventHandler(this.btnToolFill_CheckedChanged);
      // 
      // btnToolQuad
      // 
      this.btnToolQuad.Appearance = System.Windows.Forms.Appearance.Button;
      this.btnToolQuad.Image = global::C64Studio.Properties.Resources.tool_quad;
      this.btnToolQuad.Location = new System.Drawing.Point(56, 432);
      this.btnToolQuad.Name = "btnToolQuad";
      this.btnToolQuad.Size = new System.Drawing.Size(24, 24);
      this.btnToolQuad.TabIndex = 32;
      this.toolTip1.SetToolTip(this.btnToolQuad, "Filled Rectangle");
      this.btnToolQuad.UseVisualStyleBackColor = true;
      this.btnToolQuad.CheckedChanged += new System.EventHandler(this.btnToolQuad_CheckedChanged);
      // 
      // btnToolRect
      // 
      this.btnToolRect.Appearance = System.Windows.Forms.Appearance.Button;
      this.btnToolRect.Image = global::C64Studio.Properties.Resources.tool_rect;
      this.btnToolRect.Location = new System.Drawing.Point(32, 432);
      this.btnToolRect.Name = "btnToolRect";
      this.btnToolRect.Size = new System.Drawing.Size(24, 24);
      this.btnToolRect.TabIndex = 32;
      this.toolTip1.SetToolTip(this.btnToolRect, "Rectangle");
      this.btnToolRect.UseVisualStyleBackColor = true;
      this.btnToolRect.CheckedChanged += new System.EventHandler(this.btnToolRect_CheckedChanged);
      // 
      // btnToolEdit
      // 
      this.btnToolEdit.Appearance = System.Windows.Forms.Appearance.Button;
      this.btnToolEdit.Checked = true;
      this.btnToolEdit.Image = global::C64Studio.Properties.Resources.tool_none;
      this.btnToolEdit.Location = new System.Drawing.Point(8, 432);
      this.btnToolEdit.Name = "btnToolEdit";
      this.btnToolEdit.Size = new System.Drawing.Size(24, 24);
      this.btnToolEdit.TabIndex = 32;
      this.btnToolEdit.TabStop = true;
      this.toolTip1.SetToolTip(this.btnToolEdit, "Single Character");
      this.btnToolEdit.UseVisualStyleBackColor = true;
      this.btnToolEdit.CheckedChanged += new System.EventHandler(this.btnToolEdit_CheckedChanged);
      // 
      // btnApplyScreenSize
      // 
      this.btnApplyScreenSize.Location = new System.Drawing.Point(838, 110);
      this.btnApplyScreenSize.Name = "btnApplyScreenSize";
      this.btnApplyScreenSize.Size = new System.Drawing.Size(67, 20);
      this.btnApplyScreenSize.TabIndex = 30;
      this.btnApplyScreenSize.Text = "Apply";
      this.btnApplyScreenSize.UseVisualStyleBackColor = true;
      this.btnApplyScreenSize.Click += new System.EventHandler(this.btnApplyScreenSize_Click);
      // 
      // editScreenHeight
      // 
      this.editScreenHeight.Location = new System.Drawing.Point(794, 110);
      this.editScreenHeight.Name = "editScreenHeight";
      this.editScreenHeight.Size = new System.Drawing.Size(37, 20);
      this.editScreenHeight.TabIndex = 29;
      this.editScreenHeight.TextChanged += new System.EventHandler(this.editScreenHeight_TextChanged);
      // 
      // editScreenWidth
      // 
      this.editScreenWidth.Location = new System.Drawing.Point(751, 110);
      this.editScreenWidth.Name = "editScreenWidth";
      this.editScreenWidth.Size = new System.Drawing.Size(37, 20);
      this.editScreenWidth.TabIndex = 29;
      this.editScreenWidth.TextChanged += new System.EventHandler(this.editScreenWidth_TextChanged);
      // 
      // screenVScroll
      // 
      this.screenVScroll.Location = new System.Drawing.Point(655, 6);
      this.screenVScroll.Name = "screenVScroll";
      this.screenVScroll.Size = new System.Drawing.Size(16, 404);
      this.screenVScroll.TabIndex = 28;
      this.screenVScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.screenVScroll_Scroll);
      // 
      // screenHScroll
      // 
      this.screenHScroll.Location = new System.Drawing.Point(8, 413);
      this.screenHScroll.Name = "screenHScroll";
      this.screenHScroll.Size = new System.Drawing.Size(644, 16);
      this.screenHScroll.TabIndex = 27;
      this.screenHScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.screenHScroll_Scroll);
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(677, 114);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(30, 13);
      this.label7.TabIndex = 22;
      this.label7.Text = "Size:";
      // 
      // labelBGColor4
      // 
      this.labelBGColor4.AutoSize = true;
      this.labelBGColor4.Location = new System.Drawing.Point(677, 87);
      this.labelBGColor4.Name = "labelBGColor4";
      this.labelBGColor4.Size = new System.Drawing.Size(58, 13);
      this.labelBGColor4.TabIndex = 22;
      this.labelBGColor4.Text = "BGColor 4:";
      // 
      // labelMColor2
      // 
      this.labelMColor2.AutoSize = true;
      this.labelMColor2.Location = new System.Drawing.Point(677, 60);
      this.labelMColor2.Name = "labelMColor2";
      this.labelMColor2.Size = new System.Drawing.Size(64, 13);
      this.labelMColor2.TabIndex = 22;
      this.labelMColor2.Text = "Multicolor 2:";
      // 
      // labelMColor1
      // 
      this.labelMColor1.AutoSize = true;
      this.labelMColor1.Location = new System.Drawing.Point(677, 33);
      this.labelMColor1.Name = "labelMColor1";
      this.labelMColor1.Size = new System.Drawing.Size(64, 13);
      this.labelMColor1.TabIndex = 22;
      this.labelMColor1.Text = "Multicolor 1:";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(677, 6);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(68, 13);
      this.label1.TabIndex = 22;
      this.label1.Text = "Background:";
      // 
      // panelCharacters
      // 
      this.panelCharacters.AutoScroll = true;
      this.panelCharacters.AutoScrollHorizontalMaximum = 100;
      this.panelCharacters.AutoScrollHorizontalMinimum = 0;
      this.panelCharacters.AutoScrollHPos = 0;
      this.panelCharacters.AutoScrollVerticalMaximum = -23;
      this.panelCharacters.AutoScrollVerticalMinimum = 0;
      this.panelCharacters.AutoScrollVPos = 0;
      this.panelCharacters.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panelCharacters.EnableAutoScrollHorizontal = true;
      this.panelCharacters.EnableAutoScrollVertical = true;
      this.panelCharacters.HottrackColor = ((uint)(2151694591u));
      this.panelCharacters.ItemHeight = 8;
      this.panelCharacters.ItemWidth = 8;
      this.panelCharacters.Location = new System.Drawing.Point(677, 150);
      this.panelCharacters.Name = "panelCharacters";
      this.panelCharacters.PixelFormat = System.Drawing.Imaging.PixelFormat.DontCare;
      this.panelCharacters.SelectedIndex = -1;
      this.panelCharacters.Size = new System.Drawing.Size(260, 260);
      this.panelCharacters.TabIndex = 21;
      this.panelCharacters.TabStop = true;
      this.panelCharacters.VisibleAutoScrollHorizontal = false;
      this.panelCharacters.VisibleAutoScrollVertical = false;
      this.panelCharacters.SelectedIndexChanged += new System.EventHandler(this.panelCharacters_SelectedIndexChanged);
      // 
      // comboBGColor4
      // 
      this.comboBGColor4.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboBGColor4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBGColor4.FormattingEnabled = true;
      this.comboBGColor4.Location = new System.Drawing.Point(751, 84);
      this.comboBGColor4.Name = "comboBGColor4";
      this.comboBGColor4.Size = new System.Drawing.Size(70, 21);
      this.comboBGColor4.TabIndex = 1;
      this.comboBGColor4.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboColor_DrawItem);
      this.comboBGColor4.SelectedIndexChanged += new System.EventHandler(this.comboBGColor4_SelectedIndexChanged);
      // 
      // comboMulticolor2
      // 
      this.comboMulticolor2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboMulticolor2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboMulticolor2.FormattingEnabled = true;
      this.comboMulticolor2.Location = new System.Drawing.Point(751, 57);
      this.comboMulticolor2.Name = "comboMulticolor2";
      this.comboMulticolor2.Size = new System.Drawing.Size(70, 21);
      this.comboMulticolor2.TabIndex = 1;
      this.comboMulticolor2.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboColor_DrawItem);
      this.comboMulticolor2.SelectedIndexChanged += new System.EventHandler(this.comboMulticolor2_SelectedIndexChanged);
      // 
      // comboMulticolor1
      // 
      this.comboMulticolor1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboMulticolor1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboMulticolor1.FormattingEnabled = true;
      this.comboMulticolor1.Location = new System.Drawing.Point(751, 30);
      this.comboMulticolor1.Name = "comboMulticolor1";
      this.comboMulticolor1.Size = new System.Drawing.Size(70, 21);
      this.comboMulticolor1.TabIndex = 1;
      this.comboMulticolor1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboColor_DrawItem);
      this.comboMulticolor1.SelectedIndexChanged += new System.EventHandler(this.comboMulticolor1_SelectedIndexChanged);
      // 
      // comboBackground
      // 
      this.comboBackground.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboBackground.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBackground.FormattingEnabled = true;
      this.comboBackground.Location = new System.Drawing.Point(751, 3);
      this.comboBackground.Name = "comboBackground";
      this.comboBackground.Size = new System.Drawing.Size(70, 21);
      this.comboBackground.TabIndex = 1;
      this.comboBackground.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboColor_DrawItem);
      this.comboBackground.SelectedIndexChanged += new System.EventHandler(this.comboBackground_SelectedIndexChanged);
      // 
      // panelCharColors
      // 
      this.panelCharColors.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panelCharColors.DisplayPage = fastImage1;
      this.panelCharColors.Image = null;
      this.panelCharColors.Location = new System.Drawing.Point(677, 416);
      this.panelCharColors.Name = "panelCharColors";
      this.panelCharColors.Size = new System.Drawing.Size(260, 20);
      this.panelCharColors.TabIndex = 0;
      this.panelCharColors.TabStop = false;
      this.panelCharColors.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureCharColor_MouseDown);
      this.panelCharColors.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureCharColor_MouseMove);
      // 
      // pictureEditor
      // 
      this.pictureEditor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.pictureEditor.DisplayPage = fastImage2;
      this.pictureEditor.Image = null;
      this.pictureEditor.Location = new System.Drawing.Point(8, 6);
      this.pictureEditor.Name = "pictureEditor";
      this.pictureEditor.Size = new System.Drawing.Size(644, 404);
      this.pictureEditor.TabIndex = 0;
      this.pictureEditor.TabStop = false;
      this.pictureEditor.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureEditor_Paint);
      this.pictureEditor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureEditor_MouseDown);
      this.pictureEditor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureEditor_MouseMove);
      this.pictureEditor.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.pictureEditor_PreviewKeyDown);
      // 
      // tabCharsetEditor
      // 
      this.tabCharsetEditor.Controls.Add(this.tabEditor);
      this.tabCharsetEditor.Controls.Add(this.tabCharset);
      this.tabCharsetEditor.Controls.Add(this.tabProject);
      this.tabCharsetEditor.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabCharsetEditor.Location = new System.Drawing.Point(0, 24);
      this.tabCharsetEditor.Name = "tabCharsetEditor";
      this.tabCharsetEditor.SelectedIndex = 0;
      this.tabCharsetEditor.Size = new System.Drawing.Size(964, 538);
      this.tabCharsetEditor.TabIndex = 0;
      // 
      // tabCharset
      // 
      this.tabCharset.Controls.Add(this.groupBox2);
      this.tabCharset.Controls.Add(this.panelCharsetDetails);
      this.tabCharset.Location = new System.Drawing.Point(4, 22);
      this.tabCharset.Name = "tabCharset";
      this.tabCharset.Size = new System.Drawing.Size(956, 512);
      this.tabCharset.TabIndex = 2;
      this.tabCharset.Text = "Charset";
      this.tabCharset.UseVisualStyleBackColor = true;
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.btnMoveSelectionToTarget);
      this.groupBox2.Controls.Add(this.editMoveTargetIndex);
      this.groupBox2.Controls.Add(this.label10);
      this.groupBox2.Location = new System.Drawing.Point(274, 3);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(249, 45);
      this.groupBox2.TabIndex = 23;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Move Selection To";
      // 
      // btnMoveSelectionToTarget
      // 
      this.btnMoveSelectionToTarget.Location = new System.Drawing.Point(161, 11);
      this.btnMoveSelectionToTarget.Name = "btnMoveSelectionToTarget";
      this.btnMoveSelectionToTarget.Size = new System.Drawing.Size(75, 23);
      this.btnMoveSelectionToTarget.TabIndex = 2;
      this.btnMoveSelectionToTarget.Text = "Move";
      this.btnMoveSelectionToTarget.UseVisualStyleBackColor = true;
      this.btnMoveSelectionToTarget.Click += new System.EventHandler(this.btnMoveSelectionToTarget_Click);
      // 
      // editMoveTargetIndex
      // 
      this.editMoveTargetIndex.Location = new System.Drawing.Point(82, 13);
      this.editMoveTargetIndex.Name = "editMoveTargetIndex";
      this.editMoveTargetIndex.Size = new System.Drawing.Size(73, 20);
      this.editMoveTargetIndex.TabIndex = 1;
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(6, 16);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(70, 13);
      this.label10.TabIndex = 0;
      this.label10.Text = "Target Index:";
      // 
      // panelCharsetDetails
      // 
      this.panelCharsetDetails.AutoScroll = true;
      this.panelCharsetDetails.AutoScrollHorizontalMaximum = 100;
      this.panelCharsetDetails.AutoScrollHorizontalMinimum = 0;
      this.panelCharsetDetails.AutoScrollHPos = 0;
      this.panelCharsetDetails.AutoScrollVerticalMaximum = -23;
      this.panelCharsetDetails.AutoScrollVerticalMinimum = 0;
      this.panelCharsetDetails.AutoScrollVPos = 0;
      this.panelCharsetDetails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panelCharsetDetails.EnableAutoScrollHorizontal = true;
      this.panelCharsetDetails.EnableAutoScrollVertical = true;
      this.panelCharsetDetails.HottrackColor = ((uint)(2151694591u));
      this.panelCharsetDetails.ItemHeight = 8;
      this.panelCharsetDetails.ItemWidth = 8;
      this.panelCharsetDetails.Location = new System.Drawing.Point(8, 3);
      this.panelCharsetDetails.Name = "panelCharsetDetails";
      this.panelCharsetDetails.PixelFormat = System.Drawing.Imaging.PixelFormat.DontCare;
      this.panelCharsetDetails.SelectedIndex = -1;
      this.panelCharsetDetails.Size = new System.Drawing.Size(260, 260);
      this.panelCharsetDetails.TabIndex = 22;
      this.panelCharsetDetails.TabStop = true;
      this.panelCharsetDetails.VisibleAutoScrollHorizontal = false;
      this.panelCharsetDetails.VisibleAutoScrollVertical = false;
      // 
      // checkOverrideOriginalColorSettings
      // 
      this.checkOverrideOriginalColorSettings.AutoSize = true;
      this.checkOverrideOriginalColorSettings.Location = new System.Drawing.Point(838, 59);
      this.checkOverrideOriginalColorSettings.Name = "checkOverrideOriginalColorSettings";
      this.checkOverrideOriginalColorSettings.Size = new System.Drawing.Size(96, 17);
      this.checkOverrideOriginalColorSettings.TabIndex = 37;
      this.checkOverrideOriginalColorSettings.Text = "Override Mode";
      this.checkOverrideOriginalColorSettings.UseVisualStyleBackColor = true;
      this.checkOverrideOriginalColorSettings.CheckedChanged += new System.EventHandler(this.checkOverrideMode_CheckedChanged);
      // 
      // CharsetScreenEditor
      // 
      this.ClientSize = new System.Drawing.Size(964, 562);
      this.Controls.Add(this.tabCharsetEditor);
      this.Controls.Add(this.menuStrip1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "CharsetScreenEditor";
      this.Text = "Text Screen Editor";
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.tabProject.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupExport.ResumeLayout(false);
      this.groupExport.PerformLayout();
      this.tabEditor.ResumeLayout(false);
      this.tabEditor.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.panelCharColors)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureEditor)).EndInit();
      this.tabCharsetEditor.ResumeLayout(false);
      this.tabCharset.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem importCharsetToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem closeCharsetProjectToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveCharsetProjectToolStripMenuItem;
    private System.Windows.Forms.TabPage tabProject;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Button btnImportFromFile;
    private System.Windows.Forms.GroupBox groupExport;
    private System.Windows.Forms.TextBox editPrefix;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox editWrapByteCount;
    private System.Windows.Forms.CheckBox checkExportToDataWrap;
    private System.Windows.Forms.CheckBox checkExportToDataIncludeRes;
    private System.Windows.Forms.TextBox editDataExport;
    private System.Windows.Forms.Button btnExportToData;
    private System.Windows.Forms.TabPage tabEditor;
    private System.Windows.Forms.ComboBox comboMulticolor2;
    private System.Windows.Forms.ComboBox comboMulticolor1;
    private System.Windows.Forms.ComboBox comboBackground;
    private GR.Forms.FastPictureBox pictureEditor;
    private System.Windows.Forms.TabControl tabCharsetEditor;
    private GR.Forms.ImageListbox panelCharacters;
    private GR.Forms.FastPictureBox panelCharColors;
    private System.Windows.Forms.Label labelMColor2;
    private System.Windows.Forms.Label labelMColor1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button btnExportToBasic;
    private System.Windows.Forms.ComboBox comboBasicFiles;
    private System.Windows.Forms.Button btnImportCharset;
    private System.Windows.Forms.ComboBox comboExportOrientation;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Button btnExportToFile;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.ComboBox comboExportData;
    private System.Windows.Forms.VScrollBar screenVScroll;
    private System.Windows.Forms.HScrollBar screenHScroll;
    private System.Windows.Forms.Button btnApplyScreenSize;
    private System.Windows.Forms.TextBox editScreenHeight;
    private System.Windows.Forms.TextBox editScreenWidth;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.RadioButton btnToolEdit;
    private System.Windows.Forms.RadioButton btnToolRect;
    private System.Windows.Forms.RadioButton btnToolFill;
    private System.Windows.Forms.RadioButton btnToolSelect;
    private System.Windows.Forms.RadioButton btnToolQuad;
    private System.Windows.Forms.Label labelInfo;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.ComboBox comboExportArea;
    private System.Windows.Forms.TextBox editExportY;
    private System.Windows.Forms.Label labelAreaY;
    private System.Windows.Forms.TextBox editExportX;
    private System.Windows.Forms.Label labelAreaX;
    private System.Windows.Forms.TextBox editAreaHeight;
    private System.Windows.Forms.Label labelAreaHeight;
    private System.Windows.Forms.TextBox editAreaWidth;
    private System.Windows.Forms.Label labelAreaWidth;
    private System.Windows.Forms.CheckBox checkExportHex;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.ComboBox comboCharsetMode;
    private System.Windows.Forms.Label labelBGColor4;
    private System.Windows.Forms.ComboBox comboBGColor4;
    private System.Windows.Forms.ComboBox comboCharsetFiles;
    private System.Windows.Forms.Button btnExportCharset;
    private System.Windows.Forms.Button btnDefaultLowerCase;
    private System.Windows.Forms.Button btnDefaultUppercase;
    private System.Windows.Forms.Button btnExportToBASICData;
    private System.Windows.Forms.TextBox editExportBASICLineOffset;
    private System.Windows.Forms.TextBox editExportBASICLineNo;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.CheckBox checkApplyCharacter;
    private System.Windows.Forms.CheckBox checkApplyColors;
    private System.Windows.Forms.RadioButton btnToolText;
    private System.Windows.Forms.CheckBox checkShowGrid;
    private System.Windows.Forms.TextBox editDataImport;
    private System.Windows.Forms.Button btnImportFromASM;
    private System.Windows.Forms.Button btnClearImportData;
    private System.Windows.Forms.CheckBox checkExportASMAsPetSCII;
    private System.Windows.Forms.TabPage tabCharset;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.Button btnMoveSelectionToTarget;
    private System.Windows.Forms.TextBox editMoveTargetIndex;
    private System.Windows.Forms.Label label10;
    private GR.Forms.ImageListbox panelCharsetDetails;
    private System.Windows.Forms.CheckBox checkOverrideOriginalColorSettings;
  }
}
