namespace RetroDevStudio.Documents
{
  partial class GraphicScreenEditor
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraphicScreenEditor));
      GR.Image.FastImage fastImage1 = new GR.Image.FastImage();
      GR.Image.FastImage fastImage2 = new GR.Image.FastImage();
      GR.Image.FastImage fastImage3 = new GR.Image.FastImage();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.importImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveCharsetProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.closeCharsetProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.tabProject = new System.Windows.Forms.TabPage();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.btnImportCharsetFromImage = new DecentForms.Button();
      this.btnImportFromFile = new DecentForms.Button();
      this.groupExport = new System.Windows.Forms.GroupBox();
      this.checkInsertSpaces = new System.Windows.Forms.CheckBox();
      this.editExportBASICLineOffset = new System.Windows.Forms.TextBox();
      this.editExportBASICLineNo = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.btnExportToBASICHexData = new DecentForms.Button();
      this.btnExportToBASICData = new DecentForms.Button();
      this.comboCharScreens = new System.Windows.Forms.ComboBox();
      this.comboExportData = new System.Windows.Forms.ComboBox();
      this.comboExportType = new System.Windows.Forms.ComboBox();
      this.btnExportToCharScreen = new DecentForms.Button();
      this.labelCharInfoExport = new System.Windows.Forms.Label();
      this.btnExportAsBinary = new DecentForms.Button();
      this.btnExportToImage = new DecentForms.Button();
      this.btnExportAs = new DecentForms.Button();
      this.editPrefix = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.editWrapByteCount = new System.Windows.Forms.TextBox();
      this.checkExportToDataWrap = new System.Windows.Forms.CheckBox();
      this.checkExportToDataIncludeRes = new System.Windows.Forms.CheckBox();
      this.editDataExport = new System.Windows.Forms.TextBox();
      this.tabEditor = new System.Windows.Forms.TabPage();
      this.btnZoomOut = new DecentForms.Button();
      this.btnZoomIn = new DecentForms.Button();
      this.btnClearScreen = new DecentForms.Button();
      this.btnToolValidate = new DecentForms.RadioButton();
      this.btnToolSelect = new DecentForms.RadioButton();
      this.btnToolFill = new DecentForms.RadioButton();
      this.btnToolQuad = new DecentForms.RadioButton();
      this.btnToolRect = new DecentForms.RadioButton();
      this.btnToolLineDrag = new DecentForms.RadioButton();
      this.btnToolLine = new DecentForms.RadioButton();
      this.btnToolPaint = new DecentForms.RadioButton();
      this.label9 = new System.Windows.Forms.Label();
      this.label8 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.btnApplyScreenSize = new DecentForms.Button();
      this.editScreenHeight = new System.Windows.Forms.TextBox();
      this.editScreenWidth = new System.Windows.Forms.TextBox();
      this.label7 = new System.Windows.Forms.Label();
      this.screenVScroll = new DecentForms.VScrollBar();
      this.screenHScroll = new DecentForms.HScrollBar();
      this.comboCheckType = new System.Windows.Forms.ComboBox();
      this.btnMirrorY = new DecentForms.Button();
      this.btnMirrorX = new DecentForms.Button();
      this.btnShiftDown = new DecentForms.Button();
      this.btnShiftUp = new DecentForms.Button();
      this.btnShiftRight = new DecentForms.Button();
      this.btnShiftLeft = new DecentForms.Button();
      this.colorSelector = new GR.Forms.FastPictureBox();
      this.charEditor = new GR.Forms.FastPictureBox();
      this.btnPaste = new DecentForms.Button();
      this.btnCopy = new DecentForms.Button();
      this.btnCheck = new DecentForms.Button();
      this.btnFullCopy = new DecentForms.Button();
      this.btnPasteFromClipboard = new DecentForms.Button();
      this.labelCharInfo = new System.Windows.Forms.Label();
      this.checkMulticolor = new System.Windows.Forms.CheckBox();
      this.comboCharColor = new System.Windows.Forms.ComboBox();
      this.comboMulticolor2 = new System.Windows.Forms.ComboBox();
      this.comboMulticolor1 = new System.Windows.Forms.ComboBox();
      this.comboBackground = new System.Windows.Forms.ComboBox();
      this.pictureEditor = new GR.Forms.FastPictureBox();
      this.tabGraphicScreenEditor = new System.Windows.Forms.TabControl();
      this.tabColorMapping = new System.Windows.Forms.TabPage();
      this.groupColorMapping = new System.Windows.Forms.GroupBox();
      this.listColorMappingTargets = new RetroDevStudio.Controls.ArrangedItemList();
      this.comboColorMappingTargets = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.listColorMappingColors = new System.Windows.Forms.ListBox();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.labelCursorInfo = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      this.menuStrip1.SuspendLayout();
      this.tabProject.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.groupExport.SuspendLayout();
      this.tabEditor.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.colorSelector)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.charEditor)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureEditor)).BeginInit();
      this.tabGraphicScreenEditor.SuspendLayout();
      this.tabColorMapping.SuspendLayout();
      this.groupColorMapping.SuspendLayout();
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
            this.importImageToolStripMenuItem,
            this.saveCharsetProjectToolStripMenuItem,
            this.closeCharsetProjectToolStripMenuItem});
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
      this.fileToolStripMenuItem.Text = "&Screen";
      // 
      // importImageToolStripMenuItem
      // 
      this.importImageToolStripMenuItem.Name = "importImageToolStripMenuItem";
      this.importImageToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
      this.importImageToolStripMenuItem.Text = "&Import Image";
      this.importImageToolStripMenuItem.Click += new System.EventHandler(this.importImagetoolStripMenuItem_Click);
      // 
      // saveCharsetProjectToolStripMenuItem
      // 
      this.saveCharsetProjectToolStripMenuItem.Enabled = false;
      this.saveCharsetProjectToolStripMenuItem.Name = "saveCharsetProjectToolStripMenuItem";
      this.saveCharsetProjectToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
      this.saveCharsetProjectToolStripMenuItem.Text = "&Save Project";
      this.saveCharsetProjectToolStripMenuItem.Click += new System.EventHandler(this.saveCharsetProjectToolStripMenuItem_Click);
      // 
      // closeCharsetProjectToolStripMenuItem
      // 
      this.closeCharsetProjectToolStripMenuItem.Enabled = false;
      this.closeCharsetProjectToolStripMenuItem.Name = "closeCharsetProjectToolStripMenuItem";
      this.closeCharsetProjectToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
      this.closeCharsetProjectToolStripMenuItem.Text = "&Close Graphic Screen Project";
      this.closeCharsetProjectToolStripMenuItem.Click += new System.EventHandler(this.closeCharsetProjectToolStripMenuItem_Click);
      // 
      // tabProject
      // 
      this.tabProject.Controls.Add(this.groupBox1);
      this.tabProject.Controls.Add(this.groupExport);
      this.tabProject.Location = new System.Drawing.Point(4, 22);
      this.tabProject.Name = "tabProject";
      this.tabProject.Padding = new System.Windows.Forms.Padding(3);
      this.tabProject.Size = new System.Drawing.Size(956, 502);
      this.tabProject.TabIndex = 1;
      this.tabProject.Text = "Import/Export";
      this.tabProject.UseVisualStyleBackColor = true;
      // 
      // groupBox1
      // 
      this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.groupBox1.Controls.Add(this.btnImportCharsetFromImage);
      this.groupBox1.Controls.Add(this.btnImportFromFile);
      this.groupBox1.Location = new System.Drawing.Point(458, 6);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(324, 488);
      this.groupBox1.TabIndex = 4;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Import";
      // 
      // btnImportCharsetFromImage
      // 
      this.btnImportCharsetFromImage.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnImportCharsetFromImage.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnImportCharsetFromImage.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnImportCharsetFromImage.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnImportCharsetFromImage.Image = null;
      this.btnImportCharsetFromImage.Location = new System.Drawing.Point(6, 77);
      this.btnImportCharsetFromImage.Name = "btnImportCharsetFromImage";
      this.btnImportCharsetFromImage.Size = new System.Drawing.Size(117, 23);
      this.btnImportCharsetFromImage.TabIndex = 2;
      this.btnImportCharsetFromImage.Text = "From Image...";
      this.btnImportCharsetFromImage.Click += new DecentForms.EventHandler(this.btnImportCharsetFromFile_Click);
      // 
      // btnImportFromFile
      // 
      this.btnImportFromFile.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnImportFromFile.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnImportFromFile.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnImportFromFile.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnImportFromFile.Image = null;
      this.btnImportFromFile.Location = new System.Drawing.Point(6, 19);
      this.btnImportFromFile.Name = "btnImportFromFile";
      this.btnImportFromFile.Size = new System.Drawing.Size(117, 23);
      this.btnImportFromFile.TabIndex = 2;
      this.btnImportFromFile.Text = "From File...";
      this.btnImportFromFile.Click += new DecentForms.EventHandler(this.btnImportCharset_Click);
      // 
      // groupExport
      // 
      this.groupExport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.groupExport.Controls.Add(this.checkInsertSpaces);
      this.groupExport.Controls.Add(this.editExportBASICLineOffset);
      this.groupExport.Controls.Add(this.editExportBASICLineNo);
      this.groupExport.Controls.Add(this.label3);
      this.groupExport.Controls.Add(this.label4);
      this.groupExport.Controls.Add(this.btnExportToBASICHexData);
      this.groupExport.Controls.Add(this.btnExportToBASICData);
      this.groupExport.Controls.Add(this.comboCharScreens);
      this.groupExport.Controls.Add(this.comboExportData);
      this.groupExport.Controls.Add(this.comboExportType);
      this.groupExport.Controls.Add(this.btnExportToCharScreen);
      this.groupExport.Controls.Add(this.labelCharInfoExport);
      this.groupExport.Controls.Add(this.btnExportAsBinary);
      this.groupExport.Controls.Add(this.btnExportToImage);
      this.groupExport.Controls.Add(this.btnExportAs);
      this.groupExport.Controls.Add(this.editPrefix);
      this.groupExport.Controls.Add(this.label2);
      this.groupExport.Controls.Add(this.editWrapByteCount);
      this.groupExport.Controls.Add(this.checkExportToDataWrap);
      this.groupExport.Controls.Add(this.checkExportToDataIncludeRes);
      this.groupExport.Controls.Add(this.editDataExport);
      this.groupExport.Location = new System.Drawing.Point(11, 6);
      this.groupExport.Name = "groupExport";
      this.groupExport.Size = new System.Drawing.Size(441, 488);
      this.groupExport.TabIndex = 3;
      this.groupExport.TabStop = false;
      this.groupExport.Text = "Export";
      // 
      // checkInsertSpaces
      // 
      this.checkInsertSpaces.AutoSize = true;
      this.checkInsertSpaces.Location = new System.Drawing.Point(325, 166);
      this.checkInsertSpaces.Name = "checkInsertSpaces";
      this.checkInsertSpaces.Size = new System.Drawing.Size(91, 17);
      this.checkInsertSpaces.TabIndex = 30;
      this.checkInsertSpaces.Text = "Insert Spaces";
      this.checkInsertSpaces.UseVisualStyleBackColor = true;
      // 
      // editExportBASICLineOffset
      // 
      this.editExportBASICLineOffset.Location = new System.Drawing.Point(343, 221);
      this.editExportBASICLineOffset.Name = "editExportBASICLineOffset";
      this.editExportBASICLineOffset.Size = new System.Drawing.Size(73, 20);
      this.editExportBASICLineOffset.TabIndex = 28;
      this.editExportBASICLineOffset.Text = "10";
      // 
      // editExportBASICLineNo
      // 
      this.editExportBASICLineNo.Location = new System.Drawing.Point(209, 221);
      this.editExportBASICLineNo.Name = "editExportBASICLineNo";
      this.editExportBASICLineNo.Size = new System.Drawing.Size(61, 20);
      this.editExportBASICLineNo.TabIndex = 29;
      this.editExportBASICLineNo.Text = "10";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(276, 224);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(55, 13);
      this.label3.TabIndex = 26;
      this.label3.Text = "Line Step:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(155, 224);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(55, 13);
      this.label4.TabIndex = 27;
      this.label4.Text = "Start Line:";
      // 
      // btnExportToBASICHexData
      // 
      this.btnExportToBASICHexData.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnExportToBASICHexData.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnExportToBASICHexData.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnExportToBASICHexData.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnExportToBASICHexData.Image = null;
      this.btnExportToBASICHexData.Location = new System.Drawing.Point(6, 191);
      this.btnExportToBASICHexData.Name = "btnExportToBASICHexData";
      this.btnExportToBASICHexData.Size = new System.Drawing.Size(143, 23);
      this.btnExportToBASICHexData.TabIndex = 25;
      this.btnExportToBASICHexData.Text = "Export to BASIC data hex";
      this.btnExportToBASICHexData.Click += new DecentForms.EventHandler(this.btnExportToBASICDataHex_Click);
      // 
      // btnExportToBASICData
      // 
      this.btnExportToBASICData.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnExportToBASICData.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnExportToBASICData.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnExportToBASICData.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnExportToBASICData.Image = null;
      this.btnExportToBASICData.Location = new System.Drawing.Point(6, 162);
      this.btnExportToBASICData.Name = "btnExportToBASICData";
      this.btnExportToBASICData.Size = new System.Drawing.Size(143, 23);
      this.btnExportToBASICData.TabIndex = 25;
      this.btnExportToBASICData.Text = "Export to BASIC data";
      this.btnExportToBASICData.Click += new DecentForms.EventHandler(this.btnExportToBASICData_Click);
      // 
      // comboCharScreens
      // 
      this.comboCharScreens.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboCharScreens.FormattingEnabled = true;
      this.comboCharScreens.Location = new System.Drawing.Point(132, 74);
      this.comboCharScreens.Name = "comboCharScreens";
      this.comboCharScreens.Size = new System.Drawing.Size(303, 21);
      this.comboCharScreens.TabIndex = 11;
      // 
      // comboExportData
      // 
      this.comboExportData.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboExportData.FormattingEnabled = true;
      this.comboExportData.Location = new System.Drawing.Point(132, 47);
      this.comboExportData.Name = "comboExportData";
      this.comboExportData.Size = new System.Drawing.Size(303, 21);
      this.comboExportData.TabIndex = 11;
      // 
      // comboExportType
      // 
      this.comboExportType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboExportType.FormattingEnabled = true;
      this.comboExportType.Location = new System.Drawing.Point(132, 101);
      this.comboExportType.Name = "comboExportType";
      this.comboExportType.Size = new System.Drawing.Size(303, 21);
      this.comboExportType.TabIndex = 11;
      // 
      // btnExportToCharScreen
      // 
      this.btnExportToCharScreen.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnExportToCharScreen.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnExportToCharScreen.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnExportToCharScreen.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnExportToCharScreen.Image = null;
      this.btnExportToCharScreen.Location = new System.Drawing.Point(6, 72);
      this.btnExportToCharScreen.Name = "btnExportToCharScreen";
      this.btnExportToCharScreen.Size = new System.Drawing.Size(120, 23);
      this.btnExportToCharScreen.TabIndex = 8;
      this.btnExportToCharScreen.Text = "Export to Char Screen";
      this.btnExportToCharScreen.Click += new DecentForms.EventHandler(this.btnExportToCharScreen_Click);
      // 
      // labelCharInfoExport
      // 
      this.labelCharInfoExport.Location = new System.Drawing.Point(129, 24);
      this.labelCharInfoExport.Name = "labelCharInfoExport";
      this.labelCharInfoExport.Size = new System.Drawing.Size(306, 18);
      this.labelCharInfoExport.TabIndex = 10;
      this.labelCharInfoExport.Text = "No selected block";
      // 
      // btnExportAsBinary
      // 
      this.btnExportAsBinary.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnExportAsBinary.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnExportAsBinary.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnExportAsBinary.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnExportAsBinary.Image = null;
      this.btnExportAsBinary.Location = new System.Drawing.Point(6, 45);
      this.btnExportAsBinary.Name = "btnExportAsBinary";
      this.btnExportAsBinary.Size = new System.Drawing.Size(120, 23);
      this.btnExportAsBinary.TabIndex = 8;
      this.btnExportAsBinary.Text = "Export binary";
      this.btnExportAsBinary.Click += new DecentForms.EventHandler(this.btnExportAsBinary_Click);
      // 
      // btnExportToImage
      // 
      this.btnExportToImage.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnExportToImage.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnExportToImage.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnExportToImage.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnExportToImage.Image = null;
      this.btnExportToImage.Location = new System.Drawing.Point(6, 128);
      this.btnExportToImage.Name = "btnExportToImage";
      this.btnExportToImage.Size = new System.Drawing.Size(120, 23);
      this.btnExportToImage.TabIndex = 8;
      this.btnExportToImage.Text = "Export to Image";
      this.btnExportToImage.Click += new DecentForms.EventHandler(this.btnExportToImage_Click);
      // 
      // btnExportAs
      // 
      this.btnExportAs.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnExportAs.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnExportAs.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnExportAs.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnExportAs.Image = null;
      this.btnExportAs.Location = new System.Drawing.Point(6, 99);
      this.btnExportAs.Name = "btnExportAs";
      this.btnExportAs.Size = new System.Drawing.Size(120, 23);
      this.btnExportAs.TabIndex = 8;
      this.btnExportAs.Text = "Export image data as";
      this.btnExportAs.Click += new DecentForms.EventHandler(this.btnExportAsCharset_Click);
      // 
      // editPrefix
      // 
      this.editPrefix.Location = new System.Drawing.Point(238, 164);
      this.editPrefix.Name = "editPrefix";
      this.editPrefix.Size = new System.Drawing.Size(43, 20);
      this.editPrefix.TabIndex = 7;
      this.editPrefix.Text = "!byte ";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(298, 196);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(32, 13);
      this.label2.TabIndex = 6;
      this.label2.Text = "bytes";
      // 
      // editWrapByteCount
      // 
      this.editWrapByteCount.Enabled = false;
      this.editWrapByteCount.Location = new System.Drawing.Point(238, 193);
      this.editWrapByteCount.Name = "editWrapByteCount";
      this.editWrapByteCount.Size = new System.Drawing.Size(54, 20);
      this.editWrapByteCount.TabIndex = 5;
      this.editWrapByteCount.Text = "8";
      // 
      // checkExportToDataWrap
      // 
      this.checkExportToDataWrap.AutoSize = true;
      this.checkExportToDataWrap.Location = new System.Drawing.Point(158, 195);
      this.checkExportToDataWrap.Name = "checkExportToDataWrap";
      this.checkExportToDataWrap.Size = new System.Drawing.Size(64, 17);
      this.checkExportToDataWrap.TabIndex = 4;
      this.checkExportToDataWrap.Text = "Wrap at";
      this.checkExportToDataWrap.UseVisualStyleBackColor = true;
      this.checkExportToDataWrap.CheckedChanged += new System.EventHandler(this.checkExportToDataWrap_CheckedChanged);
      // 
      // checkExportToDataIncludeRes
      // 
      this.checkExportToDataIncludeRes.AutoSize = true;
      this.checkExportToDataIncludeRes.Location = new System.Drawing.Point(158, 166);
      this.checkExportToDataIncludeRes.Name = "checkExportToDataIncludeRes";
      this.checkExportToDataIncludeRes.Size = new System.Drawing.Size(74, 17);
      this.checkExportToDataIncludeRes.TabIndex = 4;
      this.checkExportToDataIncludeRes.Text = "Prefix with";
      this.checkExportToDataIncludeRes.UseVisualStyleBackColor = true;
      this.checkExportToDataIncludeRes.CheckedChanged += new System.EventHandler(this.checkExportToDataIncludeRes_CheckedChanged);
      // 
      // editDataExport
      // 
      this.editDataExport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.editDataExport.Location = new System.Drawing.Point(6, 263);
      this.editDataExport.Multiline = true;
      this.editDataExport.Name = "editDataExport";
      this.editDataExport.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.editDataExport.Size = new System.Drawing.Size(429, 219);
      this.editDataExport.TabIndex = 3;
      this.editDataExport.WordWrap = false;
      this.editDataExport.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.editDataExport_PreviewKeyDown);
      // 
      // tabEditor
      // 
      this.tabEditor.Controls.Add(this.btnZoomOut);
      this.tabEditor.Controls.Add(this.btnZoomIn);
      this.tabEditor.Controls.Add(this.btnClearScreen);
      this.tabEditor.Controls.Add(this.btnToolValidate);
      this.tabEditor.Controls.Add(this.btnToolSelect);
      this.tabEditor.Controls.Add(this.btnToolFill);
      this.tabEditor.Controls.Add(this.btnToolQuad);
      this.tabEditor.Controls.Add(this.btnToolRect);
      this.tabEditor.Controls.Add(this.btnToolLineDrag);
      this.tabEditor.Controls.Add(this.btnToolLine);
      this.tabEditor.Controls.Add(this.btnToolPaint);
      this.tabEditor.Controls.Add(this.label9);
      this.tabEditor.Controls.Add(this.label8);
      this.tabEditor.Controls.Add(this.label6);
      this.tabEditor.Controls.Add(this.label5);
      this.tabEditor.Controls.Add(this.btnApplyScreenSize);
      this.tabEditor.Controls.Add(this.editScreenHeight);
      this.tabEditor.Controls.Add(this.editScreenWidth);
      this.tabEditor.Controls.Add(this.label7);
      this.tabEditor.Controls.Add(this.screenVScroll);
      this.tabEditor.Controls.Add(this.screenHScroll);
      this.tabEditor.Controls.Add(this.comboCheckType);
      this.tabEditor.Controls.Add(this.btnMirrorY);
      this.tabEditor.Controls.Add(this.btnMirrorX);
      this.tabEditor.Controls.Add(this.btnShiftDown);
      this.tabEditor.Controls.Add(this.btnShiftUp);
      this.tabEditor.Controls.Add(this.btnShiftRight);
      this.tabEditor.Controls.Add(this.btnShiftLeft);
      this.tabEditor.Controls.Add(this.colorSelector);
      this.tabEditor.Controls.Add(this.charEditor);
      this.tabEditor.Controls.Add(this.btnPaste);
      this.tabEditor.Controls.Add(this.btnCopy);
      this.tabEditor.Controls.Add(this.btnCheck);
      this.tabEditor.Controls.Add(this.btnFullCopy);
      this.tabEditor.Controls.Add(this.btnPasteFromClipboard);
      this.tabEditor.Controls.Add(this.labelCursorInfo);
      this.tabEditor.Controls.Add(this.labelCharInfo);
      this.tabEditor.Controls.Add(this.checkMulticolor);
      this.tabEditor.Controls.Add(this.comboCharColor);
      this.tabEditor.Controls.Add(this.comboMulticolor2);
      this.tabEditor.Controls.Add(this.comboMulticolor1);
      this.tabEditor.Controls.Add(this.comboBackground);
      this.tabEditor.Controls.Add(this.pictureEditor);
      this.tabEditor.Location = new System.Drawing.Point(4, 22);
      this.tabEditor.Name = "tabEditor";
      this.tabEditor.Padding = new System.Windows.Forms.Padding(3);
      this.tabEditor.Size = new System.Drawing.Size(956, 502);
      this.tabEditor.TabIndex = 0;
      this.tabEditor.Text = "Screen";
      this.tabEditor.UseVisualStyleBackColor = true;
      // 
      // btnZoomOut
      // 
      this.btnZoomOut.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnZoomOut.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnZoomOut.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnZoomOut.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnZoomOut.Enabled = false;
      this.btnZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomOut.Image")));
      this.btnZoomOut.Location = new System.Drawing.Point(251, 438);
      this.btnZoomOut.Name = "btnZoomOut";
      this.btnZoomOut.Size = new System.Drawing.Size(26, 26);
      this.btnZoomOut.TabIndex = 8;
      this.toolTip1.SetToolTip(this.btnZoomOut, "Zoom Out");
      this.btnZoomOut.Click += new DecentForms.EventHandler(this.btnZoomOut_Click);
      // 
      // btnZoomIn
      // 
      this.btnZoomIn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnZoomIn.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnZoomIn.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnZoomIn.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomIn.Image")));
      this.btnZoomIn.Location = new System.Drawing.Point(219, 438);
      this.btnZoomIn.Name = "btnZoomIn";
      this.btnZoomIn.Size = new System.Drawing.Size(26, 26);
      this.btnZoomIn.TabIndex = 7;
      this.toolTip1.SetToolTip(this.btnZoomIn, "Zoom In");
      this.btnZoomIn.Click += new DecentForms.EventHandler(this.btnZoomIn_Click);
      // 
      // btnClearScreen
      // 
      this.btnClearScreen.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnClearScreen.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnClearScreen.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnClearScreen.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnClearScreen.Image = ((System.Drawing.Image)(resources.GetObject("btnClearScreen.Image")));
      this.btnClearScreen.Location = new System.Drawing.Point(73, 469);
      this.btnClearScreen.Name = "btnClearScreen";
      this.btnClearScreen.Size = new System.Drawing.Size(26, 26);
      this.btnClearScreen.TabIndex = 41;
      this.toolTip1.SetToolTip(this.btnClearScreen, "Clear Screen (set to spaces)");
      this.btnClearScreen.Click += new DecentForms.EventHandler(this.btnClearScreen_Click);
      // 
      // btnToolValidate
      // 
      this.btnToolValidate.Appearance = System.Windows.Forms.Appearance.Button;
      this.btnToolValidate.BorderStyle = DecentForms.BorderStyle.NONE;
      this.btnToolValidate.CheckAlign = DecentForms.ContentAlignment.MiddleLeft;
      this.btnToolValidate.Checked = true;
      this.btnToolValidate.Image = ((System.Drawing.Image)(resources.GetObject("btnToolValidate.Image")));
      this.btnToolValidate.Location = new System.Drawing.Point(182, 438);
      this.btnToolValidate.Name = "btnToolValidate";
      this.btnToolValidate.Size = new System.Drawing.Size(24, 24);
      this.btnToolValidate.TabIndex = 6;
      this.toolTip1.SetToolTip(this.btnToolValidate, "Check Validation");
      this.btnToolValidate.CheckedChanged += new DecentForms.EventHandler(this.btnToolValidate_CheckedChanged);
      // 
      // btnToolSelect
      // 
      this.btnToolSelect.Appearance = System.Windows.Forms.Appearance.Button;
      this.btnToolSelect.BorderStyle = DecentForms.BorderStyle.NONE;
      this.btnToolSelect.CheckAlign = DecentForms.ContentAlignment.MiddleLeft;
      this.btnToolSelect.Checked = false;
      this.btnToolSelect.Image = ((System.Drawing.Image)(resources.GetObject("btnToolSelect.Image")));
      this.btnToolSelect.Location = new System.Drawing.Point(152, 438);
      this.btnToolSelect.Name = "btnToolSelect";
      this.btnToolSelect.Size = new System.Drawing.Size(24, 24);
      this.btnToolSelect.TabIndex = 5;
      this.toolTip1.SetToolTip(this.btnToolSelect, "Selection");
      this.btnToolSelect.CheckedChanged += new DecentForms.EventHandler(this.btnToolSelect_CheckedChanged);
      // 
      // btnToolFill
      // 
      this.btnToolFill.Appearance = System.Windows.Forms.Appearance.Button;
      this.btnToolFill.BorderStyle = DecentForms.BorderStyle.NONE;
      this.btnToolFill.CheckAlign = DecentForms.ContentAlignment.MiddleLeft;
      this.btnToolFill.Checked = false;
      this.btnToolFill.Image = ((System.Drawing.Image)(resources.GetObject("btnToolFill.Image")));
      this.btnToolFill.Location = new System.Drawing.Point(128, 438);
      this.btnToolFill.Name = "btnToolFill";
      this.btnToolFill.Size = new System.Drawing.Size(24, 24);
      this.btnToolFill.TabIndex = 4;
      this.toolTip1.SetToolTip(this.btnToolFill, "Fill");
      this.btnToolFill.CheckedChanged += new DecentForms.EventHandler(this.btnToolFill_CheckedChanged);
      // 
      // btnToolQuad
      // 
      this.btnToolQuad.Appearance = System.Windows.Forms.Appearance.Button;
      this.btnToolQuad.BorderStyle = DecentForms.BorderStyle.NONE;
      this.btnToolQuad.CheckAlign = DecentForms.ContentAlignment.MiddleLeft;
      this.btnToolQuad.Checked = false;
      this.btnToolQuad.Image = ((System.Drawing.Image)(resources.GetObject("btnToolQuad.Image")));
      this.btnToolQuad.Location = new System.Drawing.Point(104, 438);
      this.btnToolQuad.Name = "btnToolQuad";
      this.btnToolQuad.Size = new System.Drawing.Size(24, 24);
      this.btnToolQuad.TabIndex = 3;
      this.toolTip1.SetToolTip(this.btnToolQuad, "Filled Box");
      this.btnToolQuad.CheckedChanged += new DecentForms.EventHandler(this.btnToolQuad_CheckedChanged);
      // 
      // btnToolRect
      // 
      this.btnToolRect.Appearance = System.Windows.Forms.Appearance.Button;
      this.btnToolRect.BorderStyle = DecentForms.BorderStyle.NONE;
      this.btnToolRect.CheckAlign = DecentForms.ContentAlignment.MiddleLeft;
      this.btnToolRect.Checked = false;
      this.btnToolRect.Image = ((System.Drawing.Image)(resources.GetObject("btnToolRect.Image")));
      this.btnToolRect.Location = new System.Drawing.Point(80, 438);
      this.btnToolRect.Name = "btnToolRect";
      this.btnToolRect.Size = new System.Drawing.Size(24, 24);
      this.btnToolRect.TabIndex = 2;
      this.toolTip1.SetToolTip(this.btnToolRect, "Rectangle");
      this.btnToolRect.CheckedChanged += new DecentForms.EventHandler(this.btnToolRect_CheckedChanged);
      // 
      // btnToolLineDrag
      // 
      this.btnToolLineDrag.Appearance = System.Windows.Forms.Appearance.Button;
      this.btnToolLineDrag.BorderStyle = DecentForms.BorderStyle.NONE;
      this.btnToolLineDrag.CheckAlign = DecentForms.ContentAlignment.MiddleLeft;
      this.btnToolLineDrag.Checked = false;
      this.btnToolLineDrag.Image = ((System.Drawing.Image)(resources.GetObject("btnToolLineDrag.Image")));
      this.btnToolLineDrag.Location = new System.Drawing.Point(56, 438);
      this.btnToolLineDrag.Name = "btnToolLineDrag";
      this.btnToolLineDrag.Size = new System.Drawing.Size(24, 24);
      this.btnToolLineDrag.TabIndex = 1;
      this.btnToolLineDrag.CheckedChanged += new DecentForms.EventHandler(this.btnToolLineDrag_CheckedChanged);
      // 
      // btnToolLine
      // 
      this.btnToolLine.Appearance = System.Windows.Forms.Appearance.Button;
      this.btnToolLine.BorderStyle = DecentForms.BorderStyle.NONE;
      this.btnToolLine.CheckAlign = DecentForms.ContentAlignment.MiddleLeft;
      this.btnToolLine.Checked = false;
      this.btnToolLine.Image = ((System.Drawing.Image)(resources.GetObject("btnToolLine.Image")));
      this.btnToolLine.Location = new System.Drawing.Point(32, 438);
      this.btnToolLine.Name = "btnToolLine";
      this.btnToolLine.Size = new System.Drawing.Size(24, 24);
      this.btnToolLine.TabIndex = 1;
      this.toolTip1.SetToolTip(this.btnToolLine, "Line");
      this.btnToolLine.CheckedChanged += new DecentForms.EventHandler(this.btnToolLine_CheckedChanged);
      // 
      // btnToolPaint
      // 
      this.btnToolPaint.Appearance = System.Windows.Forms.Appearance.Button;
      this.btnToolPaint.BorderStyle = DecentForms.BorderStyle.NONE;
      this.btnToolPaint.CheckAlign = DecentForms.ContentAlignment.MiddleLeft;
      this.btnToolPaint.Checked = false;
      this.btnToolPaint.Image = ((System.Drawing.Image)(resources.GetObject("btnToolPaint.Image")));
      this.btnToolPaint.Location = new System.Drawing.Point(8, 438);
      this.btnToolPaint.Name = "btnToolPaint";
      this.btnToolPaint.Size = new System.Drawing.Size(24, 24);
      this.btnToolPaint.TabIndex = 0;
      this.toolTip1.SetToolTip(this.btnToolPaint, "Single Pixel");
      this.btnToolPaint.CheckedChanged += new DecentForms.EventHandler(this.btnToolPaint_CheckedChanged);
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(681, 90);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(69, 13);
      this.label9.TabIndex = 35;
      this.label9.Text = "Custom Color";
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(680, 63);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(61, 13);
      this.label8.TabIndex = 35;
      this.label8.Text = "Multicolor 2";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(680, 36);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(61, 13);
      this.label6.TabIndex = 35;
      this.label6.Text = "Multicolor 1";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(681, 9);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(65, 13);
      this.label5.TabIndex = 35;
      this.label5.Text = "Background";
      // 
      // btnApplyScreenSize
      // 
      this.btnApplyScreenSize.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnApplyScreenSize.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnApplyScreenSize.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnApplyScreenSize.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnApplyScreenSize.Image = null;
      this.btnApplyScreenSize.Location = new System.Drawing.Point(893, 120);
      this.btnApplyScreenSize.Name = "btnApplyScreenSize";
      this.btnApplyScreenSize.Size = new System.Drawing.Size(50, 20);
      this.btnApplyScreenSize.TabIndex = 34;
      this.btnApplyScreenSize.Text = "Apply";
      this.toolTip1.SetToolTip(this.btnApplyScreenSize, "Apply new size");
      this.btnApplyScreenSize.Click += new DecentForms.EventHandler(this.btnApplyScreenSize_Click);
      // 
      // editScreenHeight
      // 
      this.editScreenHeight.Location = new System.Drawing.Point(846, 118);
      this.editScreenHeight.Name = "editScreenHeight";
      this.editScreenHeight.Size = new System.Drawing.Size(37, 20);
      this.editScreenHeight.TabIndex = 33;
      this.editScreenHeight.TextChanged += new System.EventHandler(this.editScreenHeight_TextChanged);
      // 
      // editScreenWidth
      // 
      this.editScreenWidth.Location = new System.Drawing.Point(803, 118);
      this.editScreenWidth.Name = "editScreenWidth";
      this.editScreenWidth.Size = new System.Drawing.Size(37, 20);
      this.editScreenWidth.TabIndex = 32;
      this.editScreenWidth.TextChanged += new System.EventHandler(this.editScreenWidth_TextChanged);
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(767, 121);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(30, 13);
      this.label7.TabIndex = 31;
      this.label7.Text = "Size:";
      // 
      // screenVScroll
      // 
      this.screenVScroll.Location = new System.Drawing.Point(655, 6);
      this.screenVScroll.Name = "screenVScroll";
      this.screenVScroll.Size = new System.Drawing.Size(16, 404);
      this.screenVScroll.TabIndex = 26;
      this.screenVScroll.Scroll += new DecentForms.EventHandler(this.screenVScroll_Scroll);
      // 
      // screenHScroll
      // 
      this.screenHScroll.Location = new System.Drawing.Point(8, 413);
      this.screenHScroll.Name = "screenHScroll";
      this.screenHScroll.Size = new System.Drawing.Size(644, 16);
      this.screenHScroll.TabIndex = 25;
      this.screenHScroll.Scroll += new DecentForms.EventHandler(this.screenHScroll_Scroll);
      // 
      // comboCheckType
      // 
      this.comboCheckType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboCheckType.FormattingEnabled = true;
      this.comboCheckType.Location = new System.Drawing.Point(377, 473);
      this.comboCheckType.Name = "comboCheckType";
      this.comboCheckType.Size = new System.Drawing.Size(100, 21);
      this.comboCheckType.TabIndex = 21;
      this.comboCheckType.SelectedIndexChanged += new System.EventHandler(this.comboCheckType_SelectedIndexChanged);
      // 
      // btnMirrorY
      // 
      this.btnMirrorY.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnMirrorY.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnMirrorY.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnMirrorY.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnMirrorY.Image = ((System.Drawing.Image)(resources.GetObject("btnMirrorY.Image")));
      this.btnMirrorY.Location = new System.Drawing.Point(844, 438);
      this.btnMirrorY.Name = "btnMirrorY";
      this.btnMirrorY.Size = new System.Drawing.Size(26, 26);
      this.btnMirrorY.TabIndex = 18;
      this.toolTip1.SetToolTip(this.btnMirrorY, "Mirror Vertically");
      this.btnMirrorY.Click += new DecentForms.EventHandler(this.btnMirrorY_Click);
      // 
      // btnMirrorX
      // 
      this.btnMirrorX.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnMirrorX.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnMirrorX.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnMirrorX.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnMirrorX.Image = ((System.Drawing.Image)(resources.GetObject("btnMirrorX.Image")));
      this.btnMirrorX.Location = new System.Drawing.Point(812, 438);
      this.btnMirrorX.Name = "btnMirrorX";
      this.btnMirrorX.Size = new System.Drawing.Size(26, 26);
      this.btnMirrorX.TabIndex = 19;
      this.toolTip1.SetToolTip(this.btnMirrorX, "Mirror Horizontally");
      this.btnMirrorX.Click += new DecentForms.EventHandler(this.btnMirrorX_Click);
      // 
      // btnShiftDown
      // 
      this.btnShiftDown.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnShiftDown.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnShiftDown.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnShiftDown.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnShiftDown.Image = ((System.Drawing.Image)(resources.GetObject("btnShiftDown.Image")));
      this.btnShiftDown.Location = new System.Drawing.Point(780, 438);
      this.btnShiftDown.Name = "btnShiftDown";
      this.btnShiftDown.Size = new System.Drawing.Size(26, 26);
      this.btnShiftDown.TabIndex = 20;
      this.toolTip1.SetToolTip(this.btnShiftDown, "Shift Down");
      this.btnShiftDown.Click += new DecentForms.EventHandler(this.btnShiftDown_Click);
      // 
      // btnShiftUp
      // 
      this.btnShiftUp.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnShiftUp.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnShiftUp.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnShiftUp.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnShiftUp.Image = ((System.Drawing.Image)(resources.GetObject("btnShiftUp.Image")));
      this.btnShiftUp.Location = new System.Drawing.Point(748, 438);
      this.btnShiftUp.Name = "btnShiftUp";
      this.btnShiftUp.Size = new System.Drawing.Size(26, 26);
      this.btnShiftUp.TabIndex = 15;
      this.toolTip1.SetToolTip(this.btnShiftUp, "Shift up");
      this.btnShiftUp.Click += new DecentForms.EventHandler(this.btnShiftUp_Click);
      // 
      // btnShiftRight
      // 
      this.btnShiftRight.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnShiftRight.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnShiftRight.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnShiftRight.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnShiftRight.Image = ((System.Drawing.Image)(resources.GetObject("btnShiftRight.Image")));
      this.btnShiftRight.Location = new System.Drawing.Point(716, 438);
      this.btnShiftRight.Name = "btnShiftRight";
      this.btnShiftRight.Size = new System.Drawing.Size(26, 26);
      this.btnShiftRight.TabIndex = 16;
      this.toolTip1.SetToolTip(this.btnShiftRight, "Shift right");
      this.btnShiftRight.Click += new DecentForms.EventHandler(this.btnShiftRight_Click);
      // 
      // btnShiftLeft
      // 
      this.btnShiftLeft.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnShiftLeft.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnShiftLeft.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnShiftLeft.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnShiftLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnShiftLeft.Image")));
      this.btnShiftLeft.Location = new System.Drawing.Point(684, 438);
      this.btnShiftLeft.Name = "btnShiftLeft";
      this.btnShiftLeft.Size = new System.Drawing.Size(26, 26);
      this.btnShiftLeft.TabIndex = 17;
      this.toolTip1.SetToolTip(this.btnShiftLeft, "Shift left");
      this.btnShiftLeft.Click += new DecentForms.EventHandler(this.btnShiftLeft_Click);
      // 
      // colorSelector
      // 
      this.colorSelector.AutoResize = false;
      this.colorSelector.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.colorSelector.DisplayPage = fastImage1;
      this.colorSelector.Image = null;
      this.colorSelector.Location = new System.Drawing.Point(683, 413);
      this.colorSelector.Name = "colorSelector";
      this.colorSelector.Size = new System.Drawing.Size(260, 19);
      this.colorSelector.TabIndex = 14;
      this.colorSelector.TabStop = false;
      this.colorSelector.MouseDown += new System.Windows.Forms.MouseEventHandler(this.colorSelector_MouseDown);
      // 
      // charEditor
      // 
      this.charEditor.AutoResize = false;
      this.charEditor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.charEditor.DisplayPage = fastImage2;
      this.charEditor.Image = null;
      this.charEditor.Location = new System.Drawing.Point(683, 148);
      this.charEditor.Name = "charEditor";
      this.charEditor.Size = new System.Drawing.Size(260, 260);
      this.charEditor.TabIndex = 14;
      this.charEditor.TabStop = false;
      this.charEditor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.charEditor_MouseDown);
      this.charEditor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.charEditor_MouseMove);
      // 
      // btnPaste
      // 
      this.btnPaste.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnPaste.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnPaste.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnPaste.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnPaste.Image = ((System.Drawing.Image)(resources.GetObject("btnPaste.Image")));
      this.btnPaste.Location = new System.Drawing.Point(41, 469);
      this.btnPaste.Name = "btnPaste";
      this.btnPaste.Size = new System.Drawing.Size(26, 26);
      this.btnPaste.TabIndex = 11;
      this.toolTip1.SetToolTip(this.btnPaste, "Paste at selected location");
      this.btnPaste.Click += new DecentForms.EventHandler(this.btnPaste_Click);
      // 
      // btnCopy
      // 
      this.btnCopy.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnCopy.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnCopy.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnCopy.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnCopy.Image = ((System.Drawing.Image)(resources.GetObject("btnCopy.Image")));
      this.btnCopy.Location = new System.Drawing.Point(9, 469);
      this.btnCopy.Name = "btnCopy";
      this.btnCopy.Size = new System.Drawing.Size(26, 26);
      this.btnCopy.TabIndex = 11;
      this.toolTip1.SetToolTip(this.btnCopy, "Copy selected 8x8 block");
      this.btnCopy.Click += new DecentForms.EventHandler(this.btnCopy_Click);
      // 
      // btnCheck
      // 
      this.btnCheck.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnCheck.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnCheck.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnCheck.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnCheck.Image = null;
      this.btnCheck.Location = new System.Drawing.Point(304, 471);
      this.btnCheck.Name = "btnCheck";
      this.btnCheck.Size = new System.Drawing.Size(67, 23);
      this.btnCheck.TabIndex = 6;
      this.btnCheck.Text = "Check as";
      this.btnCheck.Click += new DecentForms.EventHandler(this.btnCheck_Click);
      // 
      // btnFullCopy
      // 
      this.btnFullCopy.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnFullCopy.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnFullCopy.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnFullCopy.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnFullCopy.Image = null;
      this.btnFullCopy.Location = new System.Drawing.Point(117, 470);
      this.btnFullCopy.Name = "btnFullCopy";
      this.btnFullCopy.Size = new System.Drawing.Size(80, 23);
      this.btnFullCopy.TabIndex = 6;
      this.btnFullCopy.Text = "Full Copy";
      this.toolTip1.SetToolTip(this.btnFullCopy, "Copy Full Screen");
      this.btnFullCopy.Click += new DecentForms.EventHandler(this.btnFullCopyToClipboard_Click);
      // 
      // btnPasteFromClipboard
      // 
      this.btnPasteFromClipboard.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnPasteFromClipboard.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnPasteFromClipboard.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnPasteFromClipboard.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnPasteFromClipboard.Image = null;
      this.btnPasteFromClipboard.Location = new System.Drawing.Point(203, 470);
      this.btnPasteFromClipboard.Name = "btnPasteFromClipboard";
      this.btnPasteFromClipboard.Size = new System.Drawing.Size(84, 23);
      this.btnPasteFromClipboard.TabIndex = 6;
      this.btnPasteFromClipboard.Text = "Full Paste";
      this.toolTip1.SetToolTip(this.btnPasteFromClipboard, "Paste Full Screen");
      this.btnPasteFromClipboard.Click += new DecentForms.EventHandler(this.btnPasteFromClipboard_Click);
      // 
      // labelCharInfo
      // 
      this.labelCharInfo.Location = new System.Drawing.Point(483, 476);
      this.labelCharInfo.Name = "labelCharInfo";
      this.labelCharInfo.Size = new System.Drawing.Size(170, 24);
      this.labelCharInfo.TabIndex = 5;
      this.labelCharInfo.Text = "No selected block";
      // 
      // checkMulticolor
      // 
      this.checkMulticolor.AutoSize = true;
      this.checkMulticolor.Location = new System.Drawing.Point(683, 120);
      this.checkMulticolor.Name = "checkMulticolor";
      this.checkMulticolor.Size = new System.Drawing.Size(71, 17);
      this.checkMulticolor.TabIndex = 3;
      this.checkMulticolor.Text = "Multicolor";
      this.checkMulticolor.UseVisualStyleBackColor = true;
      this.checkMulticolor.CheckedChanged += new System.EventHandler(this.checkMulticolor_CheckedChanged);
      // 
      // comboCharColor
      // 
      this.comboCharColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboCharColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboCharColor.FormattingEnabled = true;
      this.comboCharColor.Location = new System.Drawing.Point(790, 87);
      this.comboCharColor.Name = "comboCharColor";
      this.comboCharColor.Size = new System.Drawing.Size(121, 21);
      this.comboCharColor.TabIndex = 1;
      this.comboCharColor.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboMulticolor_DrawItem);
      this.comboCharColor.SelectedIndexChanged += new System.EventHandler(this.comboCharColor_SelectedIndexChanged);
      // 
      // comboMulticolor2
      // 
      this.comboMulticolor2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboMulticolor2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboMulticolor2.FormattingEnabled = true;
      this.comboMulticolor2.Location = new System.Drawing.Point(790, 60);
      this.comboMulticolor2.Name = "comboMulticolor2";
      this.comboMulticolor2.Size = new System.Drawing.Size(121, 21);
      this.comboMulticolor2.TabIndex = 1;
      this.comboMulticolor2.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboColor_DrawItem);
      this.comboMulticolor2.SelectedIndexChanged += new System.EventHandler(this.comboMulticolor2_SelectedIndexChanged);
      // 
      // comboMulticolor1
      // 
      this.comboMulticolor1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboMulticolor1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboMulticolor1.FormattingEnabled = true;
      this.comboMulticolor1.Location = new System.Drawing.Point(790, 33);
      this.comboMulticolor1.Name = "comboMulticolor1";
      this.comboMulticolor1.Size = new System.Drawing.Size(121, 21);
      this.comboMulticolor1.TabIndex = 1;
      this.comboMulticolor1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboColor_DrawItem);
      this.comboMulticolor1.SelectedIndexChanged += new System.EventHandler(this.comboMulticolor1_SelectedIndexChanged);
      // 
      // comboBackground
      // 
      this.comboBackground.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboBackground.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBackground.FormattingEnabled = true;
      this.comboBackground.Location = new System.Drawing.Point(790, 6);
      this.comboBackground.Name = "comboBackground";
      this.comboBackground.Size = new System.Drawing.Size(121, 21);
      this.comboBackground.TabIndex = 1;
      this.comboBackground.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboColor_DrawItem);
      this.comboBackground.SelectedIndexChanged += new System.EventHandler(this.comboBackground_SelectedIndexChanged);
      // 
      // pictureEditor
      // 
      this.pictureEditor.AutoResize = false;
      this.pictureEditor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.pictureEditor.DisplayPage = fastImage3;
      this.pictureEditor.Image = null;
      this.pictureEditor.Location = new System.Drawing.Point(8, 6);
      this.pictureEditor.Name = "pictureEditor";
      this.pictureEditor.Size = new System.Drawing.Size(644, 404);
      this.pictureEditor.TabIndex = 0;
      this.pictureEditor.TabStop = false;
      this.pictureEditor.PostPaint += new GR.Forms.FastPictureBox.PostPaintCallback(this.pictureEditor_PostPaint);
      this.pictureEditor.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureEditor_Paint);
      this.pictureEditor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureEditor_MouseDown);
      this.pictureEditor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureEditor_MouseMove);
      this.pictureEditor.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureEditor_MouseUp);
      this.pictureEditor.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.pictureEditor_PreviewKeyDown);
      // 
      // tabGraphicScreenEditor
      // 
      this.tabGraphicScreenEditor.Controls.Add(this.tabEditor);
      this.tabGraphicScreenEditor.Controls.Add(this.tabColorMapping);
      this.tabGraphicScreenEditor.Controls.Add(this.tabProject);
      this.tabGraphicScreenEditor.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabGraphicScreenEditor.Location = new System.Drawing.Point(0, 24);
      this.tabGraphicScreenEditor.Name = "tabGraphicScreenEditor";
      this.tabGraphicScreenEditor.SelectedIndex = 0;
      this.tabGraphicScreenEditor.Size = new System.Drawing.Size(964, 528);
      this.tabGraphicScreenEditor.TabIndex = 0;
      // 
      // tabColorMapping
      // 
      this.tabColorMapping.Controls.Add(this.groupColorMapping);
      this.tabColorMapping.Location = new System.Drawing.Point(4, 22);
      this.tabColorMapping.Name = "tabColorMapping";
      this.tabColorMapping.Padding = new System.Windows.Forms.Padding(3);
      this.tabColorMapping.Size = new System.Drawing.Size(956, 502);
      this.tabColorMapping.TabIndex = 2;
      this.tabColorMapping.Text = "Color Mapping";
      this.tabColorMapping.UseVisualStyleBackColor = true;
      // 
      // groupColorMapping
      // 
      this.groupColorMapping.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupColorMapping.Controls.Add(this.listColorMappingTargets);
      this.groupColorMapping.Controls.Add(this.comboColorMappingTargets);
      this.groupColorMapping.Controls.Add(this.label1);
      this.groupColorMapping.Controls.Add(this.listColorMappingColors);
      this.groupColorMapping.Location = new System.Drawing.Point(6, 6);
      this.groupColorMapping.Name = "groupColorMapping";
      this.groupColorMapping.Size = new System.Drawing.Size(942, 488);
      this.groupColorMapping.TabIndex = 0;
      this.groupColorMapping.TabStop = false;
      this.groupColorMapping.Text = "Map Colors";
      // 
      // listColorMappingTargets
      // 
      this.listColorMappingTargets.AddButtonEnabled = true;
      this.listColorMappingTargets.AllowClone = false;
      this.listColorMappingTargets.DeleteButtonEnabled = false;
      this.listColorMappingTargets.HasOwnerDrawColumn = true;
      this.listColorMappingTargets.HighlightColor = System.Drawing.SystemColors.HotTrack;
      this.listColorMappingTargets.HighlightTextColor = System.Drawing.SystemColors.HighlightText;
      this.listColorMappingTargets.Location = new System.Drawing.Point(206, 16);
      this.listColorMappingTargets.MoveDownButtonEnabled = false;
      this.listColorMappingTargets.MoveUpButtonEnabled = false;
      this.listColorMappingTargets.MustHaveOneElement = true;
      this.listColorMappingTargets.Name = "listColorMappingTargets";
      this.listColorMappingTargets.SelectedIndex = -1;
      this.listColorMappingTargets.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      this.listColorMappingTargets.SelectionTextColor = System.Drawing.SystemColors.HighlightText;
      this.listColorMappingTargets.Size = new System.Drawing.Size(207, 263);
      this.listColorMappingTargets.TabIndex = 5;
      this.listColorMappingTargets.AddingItem += new RetroDevStudio.Controls.ArrangedItemList.AddingItemEventHandler(this.listColorMappingTargets_AddingItem);
      this.listColorMappingTargets.ItemAdded += new RetroDevStudio.Controls.ArrangedItemList.ItemModifiedEventHandler(this.listColorMappingTargets_ItemAdded);
      this.listColorMappingTargets.ItemRemoved += new RetroDevStudio.Controls.ArrangedItemList.ItemModifiedEventHandler(this.listColorMappingTargets_ItemRemoved);
      this.listColorMappingTargets.MovingItem += new RetroDevStudio.Controls.ArrangedItemList.ItemExchangingEventHandler(this.listColorMappingTargets_MovingItem);
      this.listColorMappingTargets.ItemMoved += new RetroDevStudio.Controls.ArrangedItemList.ItemExchangedEventHandler(this.listColorMappingTargets_ItemMoved);
      this.listColorMappingTargets.SelectedIndexChanged += new RetroDevStudio.Controls.ArrangedItemList.ItemModifiedEventHandler(this.listColorMappingTargets_SelectedIndexChanged);
      // 
      // comboColorMappingTargets
      // 
      this.comboColorMappingTargets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboColorMappingTargets.FormattingEnabled = true;
      this.comboColorMappingTargets.Location = new System.Drawing.Point(486, 16);
      this.comboColorMappingTargets.Name = "comboColorMappingTargets";
      this.comboColorMappingTargets.Size = new System.Drawing.Size(133, 21);
      this.comboColorMappingTargets.TabIndex = 3;
      this.comboColorMappingTargets.SelectedIndexChanged += new System.EventHandler(this.comboColorMappingTargets_SelectedIndexChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(419, 19);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(61, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Mapped to:";
      // 
      // listColorMappingColors
      // 
      this.listColorMappingColors.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.listColorMappingColors.FormattingEnabled = true;
      this.listColorMappingColors.ItemHeight = 16;
      this.listColorMappingColors.Location = new System.Drawing.Point(6, 19);
      this.listColorMappingColors.Name = "listColorMappingColors";
      this.listColorMappingColors.Size = new System.Drawing.Size(194, 260);
      this.listColorMappingColors.TabIndex = 0;
      this.listColorMappingColors.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listColorMappingColors_DrawItem);
      this.listColorMappingColors.SelectedIndexChanged += new System.EventHandler(this.listColorMappingColors_SelectedIndexChanged);
      // 
      // labelCursorInfo
      // 
      this.labelCursorInfo.Location = new System.Drawing.Point(483, 446);
      this.labelCursorInfo.Name = "labelCursorInfo";
      this.labelCursorInfo.Size = new System.Drawing.Size(191, 18);
      this.labelCursorInfo.TabIndex = 5;
      this.labelCursorInfo.Text = "No selected block";
      // 
      // GraphicScreenEditor
      // 
      this.ClientSize = new System.Drawing.Size(964, 552);
      this.Controls.Add(this.tabGraphicScreenEditor);
      this.Controls.Add(this.menuStrip1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "GraphicScreenEditor";
      this.Text = "Graphic Screen Editor";
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.tabProject.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupExport.ResumeLayout(false);
      this.groupExport.PerformLayout();
      this.tabEditor.ResumeLayout(false);
      this.tabEditor.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.colorSelector)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.charEditor)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureEditor)).EndInit();
      this.tabGraphicScreenEditor.ResumeLayout(false);
      this.tabColorMapping.ResumeLayout(false);
      this.groupColorMapping.ResumeLayout(false);
      this.groupColorMapping.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem importImageToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem closeCharsetProjectToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveCharsetProjectToolStripMenuItem;
    private System.Windows.Forms.TabPage tabProject;
    private System.Windows.Forms.GroupBox groupBox1;
    private DecentForms.Button btnImportCharsetFromImage;
    private DecentForms.Button btnImportFromFile;
    private System.Windows.Forms.GroupBox groupExport;
    private System.Windows.Forms.TextBox editPrefix;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox editWrapByteCount;
    private System.Windows.Forms.CheckBox checkExportToDataWrap;
    private System.Windows.Forms.CheckBox checkExportToDataIncludeRes;
    private System.Windows.Forms.TextBox editDataExport;
    private System.Windows.Forms.TabPage tabEditor;
    private DecentForms.Button btnPaste;
    private DecentForms.Button btnCopy;
    private DecentForms.Button btnPasteFromClipboard;
    private System.Windows.Forms.Label labelCharInfo;
    private System.Windows.Forms.CheckBox checkMulticolor;
    private System.Windows.Forms.ComboBox comboCharColor;
    private System.Windows.Forms.ComboBox comboMulticolor2;
    private System.Windows.Forms.ComboBox comboMulticolor1;
    private System.Windows.Forms.ComboBox comboBackground;
    private GR.Forms.FastPictureBox pictureEditor;
    private System.Windows.Forms.TabControl tabGraphicScreenEditor;
    private DecentForms.Button btnMirrorY;
    private DecentForms.Button btnMirrorX;
    private DecentForms.Button btnShiftDown;
    private DecentForms.Button btnShiftUp;
    private DecentForms.Button btnShiftRight;
    private DecentForms.Button btnShiftLeft;
    private GR.Forms.FastPictureBox charEditor;
    private DecentForms.Button btnExportAs;
    private DecentForms.Button btnCheck;
    private System.Windows.Forms.Label labelCharInfoExport;
    private System.Windows.Forms.ComboBox comboCheckType;
    private System.Windows.Forms.ComboBox comboExportType;
    private DecentForms.Button btnExportAsBinary;
    private System.Windows.Forms.ComboBox comboExportData;
    private DecentForms.HScrollBar screenHScroll;
    private DecentForms.VScrollBar screenVScroll;
    private System.Windows.Forms.ComboBox comboCharScreens;
    private DecentForms.Button btnExportToCharScreen;
    private DecentForms.Button btnApplyScreenSize;
    private System.Windows.Forms.TextBox editScreenHeight;
    private System.Windows.Forms.TextBox editScreenWidth;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TabPage tabColorMapping;
    private System.Windows.Forms.GroupBox groupColorMapping;
    private System.Windows.Forms.ComboBox comboColorMappingTargets;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ListBox listColorMappingColors;
    private Controls.ArrangedItemList listColorMappingTargets;
    private System.Windows.Forms.TextBox editExportBASICLineOffset;
    private System.Windows.Forms.TextBox editExportBASICLineNo;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private DecentForms.Button btnExportToBASICData;
    private GR.Forms.FastPictureBox colorSelector;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label5;
    private DecentForms.Button btnExportToImage;
    private DecentForms.Button btnFullCopy;
    private DecentForms.RadioButton btnToolValidate;
    private DecentForms.RadioButton btnToolSelect;
    private DecentForms.RadioButton btnToolFill;
    private DecentForms.RadioButton btnToolQuad;
    private DecentForms.RadioButton btnToolRect;
    private DecentForms.RadioButton btnToolPaint;
    private DecentForms.Button btnExportToBASICHexData;
    private System.Windows.Forms.ToolTip toolTip1;
    private DecentForms.Button btnClearScreen;
    private DecentForms.Button btnZoomIn;
    private DecentForms.Button btnZoomOut;
    private DecentForms.RadioButton btnToolLine;
    private DecentForms.RadioButton btnToolLineDrag;
    private System.Windows.Forms.CheckBox checkInsertSpaces;
    private System.Windows.Forms.Label labelCursorInfo;
  }
}
