namespace C64Studio
{
  partial class MapEditor
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
      GR.Image.FastImage fastImage1 = new GR.Image.FastImage();
      GR.Image.FastImage fastImage2 = new GR.Image.FastImage();
      GR.Image.FastImage fastImage3 = new GR.Image.FastImage();
      GR.Image.FastImage fastImage4 = new GR.Image.FastImage();
      GR.Image.FastImage fastImage5 = new GR.Image.FastImage();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapEditor));
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.importCharsetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveCharsetProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.closeCharsetProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.tabProject = new System.Windows.Forms.TabPage();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.btnImportCharset = new System.Windows.Forms.Button();
      this.btnImportFromFile = new System.Windows.Forms.Button();
      this.groupExport = new System.Windows.Forms.GroupBox();
      this.label6 = new System.Windows.Forms.Label();
      this.comboExportData = new System.Windows.Forms.ComboBox();
      this.label5 = new System.Windows.Forms.Label();
      this.comboExportOrientation = new System.Windows.Forms.ComboBox();
      this.editPrefix = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.editWrapByteCount = new System.Windows.Forms.TextBox();
      this.checkExportToDataWrap = new System.Windows.Forms.CheckBox();
      this.checkExportToDataIncludeRes = new System.Windows.Forms.CheckBox();
      this.editDataExport = new System.Windows.Forms.TextBox();
      this.btnExportToFile = new System.Windows.Forms.Button();
      this.btnExportToData = new System.Windows.Forms.Button();
      this.tabEditor = new System.Windows.Forms.TabPage();
      this.btnToolSelect = new System.Windows.Forms.RadioButton();
      this.btnToolFill = new System.Windows.Forms.RadioButton();
      this.btnToolQuad = new System.Windows.Forms.RadioButton();
      this.btnToolRect = new System.Windows.Forms.RadioButton();
      this.btnToolEdit = new System.Windows.Forms.RadioButton();
      this.labelEditInfo = new System.Windows.Forms.Label();
      this.groupMapExtraData = new System.Windows.Forms.GroupBox();
      this.editMapExtraData = new System.Windows.Forms.TextBox();
      this.label20 = new System.Windows.Forms.Label();
      this.comboTiles = new System.Windows.Forms.ComboBox();
      this.comboMaps = new System.Windows.Forms.ComboBox();
      this.groupSize = new System.Windows.Forms.GroupBox();
      this.comboMapAlternativeMode = new System.Windows.Forms.ComboBox();
      this.comboMapAlternativeBGColor4 = new System.Windows.Forms.ComboBox();
      this.comboMapMultiColor2 = new System.Windows.Forms.ComboBox();
      this.comboMapBGColor = new System.Windows.Forms.ComboBox();
      this.comboMapMultiColor1 = new System.Windows.Forms.ComboBox();
      this.btnCopy = new System.Windows.Forms.Button();
      this.btnMapAdd = new System.Windows.Forms.Button();
      this.btnMapDelete = new System.Windows.Forms.Button();
      this.btnMapApply = new System.Windows.Forms.Button();
      this.label14 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.label22 = new System.Windows.Forms.Label();
      this.label23 = new System.Windows.Forms.Label();
      this.editMapName = new System.Windows.Forms.TextBox();
      this.label13 = new System.Windows.Forms.Label();
      this.label21 = new System.Windows.Forms.Label();
      this.label18 = new System.Windows.Forms.Label();
      this.editTileSpacingH = new System.Windows.Forms.TextBox();
      this.editMapHeight = new System.Windows.Forms.TextBox();
      this.editTileSpacingW = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.editMapWidth = new System.Windows.Forms.TextBox();
      this.mapHScroll = new System.Windows.Forms.HScrollBar();
      this.mapVScroll = new System.Windows.Forms.VScrollBar();
      this.label19 = new System.Windows.Forms.Label();
      this.pictureEditor = new GR.Forms.FastPictureBox();
      this.tabMapEditor = new System.Windows.Forms.TabControl();
      this.tabTiles = new System.Windows.Forms.TabPage();
      this.comboTileMode = new System.Windows.Forms.ComboBox();
      this.btnMoveTileDown = new System.Windows.Forms.Button();
      this.btnMoveTileUp = new System.Windows.Forms.Button();
      this.btnTileDelete = new System.Windows.Forms.Button();
      this.btnTileApply = new System.Windows.Forms.Button();
      this.btnTileAdd = new System.Windows.Forms.Button();
      this.listTileChars = new System.Windows.Forms.ListView();
      this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.listTileInfo = new System.Windows.Forms.ListView();
      this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.editTileName = new System.Windows.Forms.TextBox();
      this.editTileHeight = new System.Windows.Forms.TextBox();
      this.editTileWidth = new System.Windows.Forms.TextBox();
      this.label17 = new System.Windows.Forms.Label();
      this.label24 = new System.Windows.Forms.Label();
      this.labelTilesBGColor4 = new System.Windows.Forms.Label();
      this.labelTilesMulticolor2 = new System.Windows.Forms.Label();
      this.label16 = new System.Windows.Forms.Label();
      this.labelTilesMulticolor1 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label15 = new System.Windows.Forms.Label();
      this.panelCharacters = new GR.Forms.ImageListbox();
      this.comboTileBGColor4 = new System.Windows.Forms.ComboBox();
      this.comboTileMulticolor2 = new System.Windows.Forms.ComboBox();
      this.comboTileMulticolor1 = new System.Windows.Forms.ComboBox();
      this.comboTileBackground = new System.Windows.Forms.ComboBox();
      this.panelCharColors = new GR.Forms.FastPictureBox();
      this.pictureTileDisplay = new GR.Forms.FastPictureBox();
      this.tabPage1 = new System.Windows.Forms.TabPage();
      this.label7 = new System.Windows.Forms.Label();
      this.label8 = new System.Windows.Forms.Label();
      this.label9 = new System.Windows.Forms.Label();
      this.imageListbox1 = new GR.Forms.ImageListbox();
      this.checkBox1 = new System.Windows.Forms.CheckBox();
      this.comboBox1 = new System.Windows.Forms.ComboBox();
      this.comboBox2 = new System.Windows.Forms.ComboBox();
      this.comboBox3 = new System.Windows.Forms.ComboBox();
      this.fastPictureBox1 = new GR.Forms.FastPictureBox();
      this.fastPictureBox2 = new GR.Forms.FastPictureBox();
      this.tabPage2 = new System.Windows.Forms.TabPage();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.button1 = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.label10 = new System.Windows.Forms.Label();
      this.comboBox4 = new System.Windows.Forms.ComboBox();
      this.label11 = new System.Windows.Forms.Label();
      this.comboBox5 = new System.Windows.Forms.ComboBox();
      this.comboBox6 = new System.Windows.Forms.ComboBox();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.label12 = new System.Windows.Forms.Label();
      this.textBox2 = new System.Windows.Forms.TextBox();
      this.checkBox2 = new System.Windows.Forms.CheckBox();
      this.checkBox3 = new System.Windows.Forms.CheckBox();
      this.textBox3 = new System.Windows.Forms.TextBox();
      this.button3 = new System.Windows.Forms.Button();
      this.button4 = new System.Windows.Forms.Button();
      this.button5 = new System.Windows.Forms.Button();
      this.btnSetNextTileChar = new System.Windows.Forms.Button();
      this.btnCopyTileCharToNextIncreased = new System.Windows.Forms.Button();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      this.menuStrip1.SuspendLayout();
      this.tabProject.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.groupExport.SuspendLayout();
      this.tabEditor.SuspendLayout();
      this.groupMapExtraData.SuspendLayout();
      this.groupSize.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureEditor)).BeginInit();
      this.tabMapEditor.SuspendLayout();
      this.tabTiles.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.panelCharColors)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureTileDisplay)).BeginInit();
      this.tabPage1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.fastPictureBox1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.fastPictureBox2)).BeginInit();
      this.tabPage2.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox3.SuspendLayout();
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
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
      this.fileToolStripMenuItem.Text = "&Map Project";
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
      this.tabProject.Size = new System.Drawing.Size(956, 475);
      this.tabProject.TabIndex = 1;
      this.tabProject.Text = "Project";
      this.tabProject.UseVisualStyleBackColor = true;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.btnImportCharset);
      this.groupBox1.Controls.Add(this.btnImportFromFile);
      this.groupBox1.Location = new System.Drawing.Point(458, 6);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(324, 343);
      this.groupBox1.TabIndex = 4;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Import";
      // 
      // btnImportCharset
      // 
      this.btnImportCharset.Location = new System.Drawing.Point(6, 48);
      this.btnImportCharset.Name = "btnImportCharset";
      this.btnImportCharset.Size = new System.Drawing.Size(117, 23);
      this.btnImportCharset.TabIndex = 2;
      this.btnImportCharset.Text = "Charset...";
      this.btnImportCharset.UseVisualStyleBackColor = true;
      this.btnImportCharset.Click += new System.EventHandler(this.btnImportCharset_Click);
      // 
      // btnImportFromFile
      // 
      this.btnImportFromFile.Location = new System.Drawing.Point(6, 19);
      this.btnImportFromFile.Name = "btnImportFromFile";
      this.btnImportFromFile.Size = new System.Drawing.Size(117, 23);
      this.btnImportFromFile.TabIndex = 2;
      this.btnImportFromFile.Text = "From File...";
      this.btnImportFromFile.UseVisualStyleBackColor = true;
      this.btnImportFromFile.Click += new System.EventHandler(this.btnImportFromFile_Click);
      // 
      // groupExport
      // 
      this.groupExport.Controls.Add(this.label6);
      this.groupExport.Controls.Add(this.comboExportData);
      this.groupExport.Controls.Add(this.label5);
      this.groupExport.Controls.Add(this.comboExportOrientation);
      this.groupExport.Controls.Add(this.editPrefix);
      this.groupExport.Controls.Add(this.label2);
      this.groupExport.Controls.Add(this.editWrapByteCount);
      this.groupExport.Controls.Add(this.checkExportToDataWrap);
      this.groupExport.Controls.Add(this.checkExportToDataIncludeRes);
      this.groupExport.Controls.Add(this.editDataExport);
      this.groupExport.Controls.Add(this.btnExportToFile);
      this.groupExport.Controls.Add(this.btnExportToData);
      this.groupExport.Location = new System.Drawing.Point(11, 6);
      this.groupExport.Name = "groupExport";
      this.groupExport.Size = new System.Drawing.Size(441, 343);
      this.groupExport.TabIndex = 3;
      this.groupExport.TabStop = false;
      this.groupExport.Text = "Export";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(3, 24);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(66, 13);
      this.label6.TabIndex = 11;
      this.label6.Text = "Export Data:";
      // 
      // comboExportData
      // 
      this.comboExportData.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboExportData.FormattingEnabled = true;
      this.comboExportData.Location = new System.Drawing.Point(118, 21);
      this.comboExportData.Name = "comboExportData";
      this.comboExportData.Size = new System.Drawing.Size(317, 21);
      this.comboExportData.TabIndex = 10;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(129, 86);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(61, 13);
      this.label5.TabIndex = 9;
      this.label5.Text = "Orientation:";
      // 
      // comboExportOrientation
      // 
      this.comboExportOrientation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboExportOrientation.FormattingEnabled = true;
      this.comboExportOrientation.Items.AddRange(new object[] {
            "row by row",
            "column by column"});
      this.comboExportOrientation.Location = new System.Drawing.Point(217, 81);
      this.comboExportOrientation.Name = "comboExportOrientation";
      this.comboExportOrientation.Size = new System.Drawing.Size(131, 21);
      this.comboExportOrientation.TabIndex = 8;
      // 
      // editPrefix
      // 
      this.editPrefix.Location = new System.Drawing.Point(214, 52);
      this.editPrefix.Name = "editPrefix";
      this.editPrefix.Size = new System.Drawing.Size(43, 20);
      this.editPrefix.TabIndex = 7;
      this.editPrefix.Text = "          !byte ";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(394, 55);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(32, 13);
      this.label2.TabIndex = 6;
      this.label2.Text = "bytes";
      // 
      // editWrapByteCount
      // 
      this.editWrapByteCount.Enabled = false;
      this.editWrapByteCount.Location = new System.Drawing.Point(347, 52);
      this.editWrapByteCount.Name = "editWrapByteCount";
      this.editWrapByteCount.Size = new System.Drawing.Size(41, 20);
      this.editWrapByteCount.TabIndex = 5;
      this.editWrapByteCount.Text = "40";
      // 
      // checkExportToDataWrap
      // 
      this.checkExportToDataWrap.AutoSize = true;
      this.checkExportToDataWrap.Location = new System.Drawing.Point(264, 54);
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
      this.checkExportToDataIncludeRes.Location = new System.Drawing.Point(118, 54);
      this.checkExportToDataIncludeRes.Name = "checkExportToDataIncludeRes";
      this.checkExportToDataIncludeRes.Size = new System.Drawing.Size(74, 17);
      this.checkExportToDataIncludeRes.TabIndex = 4;
      this.checkExportToDataIncludeRes.Text = "Prefix with";
      this.checkExportToDataIncludeRes.UseVisualStyleBackColor = true;
      this.checkExportToDataIncludeRes.CheckedChanged += new System.EventHandler(this.checkExportToDataIncludeRes_CheckedChanged);
      // 
      // editDataExport
      // 
      this.editDataExport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.editDataExport.Location = new System.Drawing.Point(6, 139);
      this.editDataExport.Multiline = true;
      this.editDataExport.Name = "editDataExport";
      this.editDataExport.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.editDataExport.Size = new System.Drawing.Size(429, 198);
      this.editDataExport.TabIndex = 3;
      this.editDataExport.WordWrap = false;
      this.editDataExport.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editDataExport_KeyPress);
      // 
      // btnExportToFile
      // 
      this.btnExportToFile.Location = new System.Drawing.Point(6, 81);
      this.btnExportToFile.Name = "btnExportToFile";
      this.btnExportToFile.Size = new System.Drawing.Size(106, 23);
      this.btnExportToFile.TabIndex = 2;
      this.btnExportToFile.Text = "as binary file";
      this.btnExportToFile.UseVisualStyleBackColor = true;
      this.btnExportToFile.Click += new System.EventHandler(this.btnExportToFile_Click);
      // 
      // btnExportToData
      // 
      this.btnExportToData.Location = new System.Drawing.Point(6, 52);
      this.btnExportToData.Name = "btnExportToData";
      this.btnExportToData.Size = new System.Drawing.Size(106, 23);
      this.btnExportToData.TabIndex = 2;
      this.btnExportToData.Text = "as assembly source";
      this.btnExportToData.UseVisualStyleBackColor = true;
      this.btnExportToData.Click += new System.EventHandler(this.btnExportToData_Click);
      // 
      // tabEditor
      // 
      this.tabEditor.Controls.Add(this.btnToolSelect);
      this.tabEditor.Controls.Add(this.btnToolFill);
      this.tabEditor.Controls.Add(this.btnToolQuad);
      this.tabEditor.Controls.Add(this.btnToolRect);
      this.tabEditor.Controls.Add(this.btnToolEdit);
      this.tabEditor.Controls.Add(this.labelEditInfo);
      this.tabEditor.Controls.Add(this.groupMapExtraData);
      this.tabEditor.Controls.Add(this.comboTiles);
      this.tabEditor.Controls.Add(this.comboMaps);
      this.tabEditor.Controls.Add(this.groupSize);
      this.tabEditor.Controls.Add(this.mapHScroll);
      this.tabEditor.Controls.Add(this.mapVScroll);
      this.tabEditor.Controls.Add(this.label19);
      this.tabEditor.Controls.Add(this.pictureEditor);
      this.tabEditor.Location = new System.Drawing.Point(4, 22);
      this.tabEditor.Name = "tabEditor";
      this.tabEditor.Padding = new System.Windows.Forms.Padding(3);
      this.tabEditor.Size = new System.Drawing.Size(956, 475);
      this.tabEditor.TabIndex = 0;
      this.tabEditor.Text = "Map";
      this.tabEditor.UseVisualStyleBackColor = true;
      // 
      // btnToolSelect
      // 
      this.btnToolSelect.Appearance = System.Windows.Forms.Appearance.Button;
      this.btnToolSelect.Image = global::C64Studio.Properties.Resources.tool_select;
      this.btnToolSelect.Location = new System.Drawing.Point(103, 432);
      this.btnToolSelect.Name = "btnToolSelect";
      this.btnToolSelect.Size = new System.Drawing.Size(24, 24);
      this.btnToolSelect.TabIndex = 36;
      this.btnToolSelect.UseVisualStyleBackColor = true;
      this.btnToolSelect.CheckedChanged += new System.EventHandler(this.btnToolSelect_CheckedChanged);
      // 
      // btnToolFill
      // 
      this.btnToolFill.Appearance = System.Windows.Forms.Appearance.Button;
      this.btnToolFill.Image = global::C64Studio.Properties.Resources.tool_fill;
      this.btnToolFill.Location = new System.Drawing.Point(79, 432);
      this.btnToolFill.Name = "btnToolFill";
      this.btnToolFill.Size = new System.Drawing.Size(24, 24);
      this.btnToolFill.TabIndex = 37;
      this.btnToolFill.UseVisualStyleBackColor = true;
      this.btnToolFill.CheckedChanged += new System.EventHandler(this.btnToolFill_CheckedChanged);
      // 
      // btnToolQuad
      // 
      this.btnToolQuad.Appearance = System.Windows.Forms.Appearance.Button;
      this.btnToolQuad.Image = global::C64Studio.Properties.Resources.tool_quad;
      this.btnToolQuad.Location = new System.Drawing.Point(55, 432);
      this.btnToolQuad.Name = "btnToolQuad";
      this.btnToolQuad.Size = new System.Drawing.Size(24, 24);
      this.btnToolQuad.TabIndex = 35;
      this.btnToolQuad.UseVisualStyleBackColor = true;
      this.btnToolQuad.CheckedChanged += new System.EventHandler(this.btnToolQuad_CheckedChanged);
      // 
      // btnToolRect
      // 
      this.btnToolRect.Appearance = System.Windows.Forms.Appearance.Button;
      this.btnToolRect.Image = global::C64Studio.Properties.Resources.tool_rect;
      this.btnToolRect.Location = new System.Drawing.Point(31, 432);
      this.btnToolRect.Name = "btnToolRect";
      this.btnToolRect.Size = new System.Drawing.Size(24, 24);
      this.btnToolRect.TabIndex = 33;
      this.btnToolRect.UseVisualStyleBackColor = true;
      this.btnToolRect.CheckedChanged += new System.EventHandler(this.btnToolRect_CheckedChanged);
      // 
      // btnToolEdit
      // 
      this.btnToolEdit.Appearance = System.Windows.Forms.Appearance.Button;
      this.btnToolEdit.Checked = true;
      this.btnToolEdit.Image = global::C64Studio.Properties.Resources.tool_none;
      this.btnToolEdit.Location = new System.Drawing.Point(7, 432);
      this.btnToolEdit.Name = "btnToolEdit";
      this.btnToolEdit.Size = new System.Drawing.Size(24, 24);
      this.btnToolEdit.TabIndex = 34;
      this.btnToolEdit.TabStop = true;
      this.btnToolEdit.UseVisualStyleBackColor = true;
      this.btnToolEdit.CheckedChanged += new System.EventHandler(this.btnToolEdit_CheckedChanged);
      // 
      // labelEditInfo
      // 
      this.labelEditInfo.Location = new System.Drawing.Point(681, 433);
      this.labelEditInfo.Name = "labelEditInfo";
      this.labelEditInfo.Size = new System.Drawing.Size(265, 23);
      this.labelEditInfo.TabIndex = 32;
      this.labelEditInfo.Text = "Tile Info";
      // 
      // groupMapExtraData
      // 
      this.groupMapExtraData.Controls.Add(this.editMapExtraData);
      this.groupMapExtraData.Controls.Add(this.label20);
      this.groupMapExtraData.Location = new System.Drawing.Point(681, 222);
      this.groupMapExtraData.Name = "groupMapExtraData";
      this.groupMapExtraData.Size = new System.Drawing.Size(272, 161);
      this.groupMapExtraData.TabIndex = 31;
      this.groupMapExtraData.TabStop = false;
      this.groupMapExtraData.Text = "Extra Data";
      // 
      // editMapExtraData
      // 
      this.editMapExtraData.Location = new System.Drawing.Point(6, 36);
      this.editMapExtraData.Multiline = true;
      this.editMapExtraData.Name = "editMapExtraData";
      this.editMapExtraData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.editMapExtraData.Size = new System.Drawing.Size(258, 119);
      this.editMapExtraData.TabIndex = 1;
      this.editMapExtraData.TextChanged += new System.EventHandler(this.editMapExtraData_TextChanged);
      this.editMapExtraData.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editMapExtraData_KeyPress);
      // 
      // label20
      // 
      this.label20.AutoSize = true;
      this.label20.Location = new System.Drawing.Point(6, 20);
      this.label20.Name = "label20";
      this.label20.Size = new System.Drawing.Size(139, 13);
      this.label20.TabIndex = 0;
      this.label20.Text = "Additional Binary Data (Hex)";
      // 
      // comboTiles
      // 
      this.comboTiles.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboTiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboTiles.FormattingEnabled = true;
      this.comboTiles.Location = new System.Drawing.Point(684, 389);
      this.comboTiles.Name = "comboTiles";
      this.comboTiles.Size = new System.Drawing.Size(264, 21);
      this.comboTiles.TabIndex = 30;
      this.comboTiles.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboTiles_DrawItem);
      this.comboTiles.SelectedIndexChanged += new System.EventHandler(this.comboTiles_SelectedIndexChanged);
      // 
      // comboMaps
      // 
      this.comboMaps.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboMaps.Enabled = false;
      this.comboMaps.FormattingEnabled = true;
      this.comboMaps.Location = new System.Drawing.Point(752, 6);
      this.comboMaps.Name = "comboMaps";
      this.comboMaps.Size = new System.Drawing.Size(201, 21);
      this.comboMaps.TabIndex = 29;
      this.comboMaps.SelectedIndexChanged += new System.EventHandler(this.comboMaps_SelectedIndexChanged);
      // 
      // groupSize
      // 
      this.groupSize.Controls.Add(this.comboMapAlternativeMode);
      this.groupSize.Controls.Add(this.comboMapAlternativeBGColor4);
      this.groupSize.Controls.Add(this.comboMapMultiColor2);
      this.groupSize.Controls.Add(this.comboMapBGColor);
      this.groupSize.Controls.Add(this.comboMapMultiColor1);
      this.groupSize.Controls.Add(this.btnCopy);
      this.groupSize.Controls.Add(this.btnMapAdd);
      this.groupSize.Controls.Add(this.btnMapDelete);
      this.groupSize.Controls.Add(this.btnMapApply);
      this.groupSize.Controls.Add(this.label14);
      this.groupSize.Controls.Add(this.label1);
      this.groupSize.Controls.Add(this.label22);
      this.groupSize.Controls.Add(this.label23);
      this.groupSize.Controls.Add(this.editMapName);
      this.groupSize.Controls.Add(this.label13);
      this.groupSize.Controls.Add(this.label21);
      this.groupSize.Controls.Add(this.label18);
      this.groupSize.Controls.Add(this.editTileSpacingH);
      this.groupSize.Controls.Add(this.editMapHeight);
      this.groupSize.Controls.Add(this.editTileSpacingW);
      this.groupSize.Controls.Add(this.label3);
      this.groupSize.Controls.Add(this.editMapWidth);
      this.groupSize.Location = new System.Drawing.Point(681, 33);
      this.groupSize.Name = "groupSize";
      this.groupSize.Size = new System.Drawing.Size(272, 183);
      this.groupSize.TabIndex = 28;
      this.groupSize.TabStop = false;
      this.groupSize.Text = "Map Details";
      // 
      // comboMapAlternativeMode
      // 
      this.comboMapAlternativeMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboMapAlternativeMode.FormattingEnabled = true;
      this.comboMapAlternativeMode.Location = new System.Drawing.Point(63, 97);
      this.comboMapAlternativeMode.Name = "comboMapAlternativeMode";
      this.comboMapAlternativeMode.Size = new System.Drawing.Size(121, 21);
      this.comboMapAlternativeMode.TabIndex = 29;
      this.comboMapAlternativeMode.SelectedIndexChanged += new System.EventHandler(this.comboMapAlternativeMode_SelectedIndexChanged);
      // 
      // comboMapAlternativeBGColor4
      // 
      this.comboMapAlternativeBGColor4.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboMapAlternativeBGColor4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboMapAlternativeBGColor4.FormattingEnabled = true;
      this.comboMapAlternativeBGColor4.Location = new System.Drawing.Point(182, 151);
      this.comboMapAlternativeBGColor4.Name = "comboMapAlternativeBGColor4";
      this.comboMapAlternativeBGColor4.Size = new System.Drawing.Size(82, 21);
      this.comboMapAlternativeBGColor4.TabIndex = 28;
      this.comboMapAlternativeBGColor4.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboAlternativeColor_DrawItem);
      this.comboMapAlternativeBGColor4.SelectedIndexChanged += new System.EventHandler(this.comboMapBGColor4_SelectedIndexChanged);
      // 
      // comboMapMultiColor2
      // 
      this.comboMapMultiColor2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboMapMultiColor2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboMapMultiColor2.FormattingEnabled = true;
      this.comboMapMultiColor2.Location = new System.Drawing.Point(182, 124);
      this.comboMapMultiColor2.Name = "comboMapMultiColor2";
      this.comboMapMultiColor2.Size = new System.Drawing.Size(82, 21);
      this.comboMapMultiColor2.TabIndex = 28;
      this.comboMapMultiColor2.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboAlternativeColor_DrawItem);
      this.comboMapMultiColor2.SelectedIndexChanged += new System.EventHandler(this.comboMapMultiColor2_SelectedIndexChanged);
      // 
      // comboMapBGColor
      // 
      this.comboMapBGColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboMapBGColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboMapBGColor.FormattingEnabled = true;
      this.comboMapBGColor.Location = new System.Drawing.Point(63, 151);
      this.comboMapBGColor.Name = "comboMapBGColor";
      this.comboMapBGColor.Size = new System.Drawing.Size(82, 21);
      this.comboMapBGColor.TabIndex = 28;
      this.comboMapBGColor.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboAlternativeColor_DrawItem);
      this.comboMapBGColor.SelectedIndexChanged += new System.EventHandler(this.comboMapBGColor_SelectedIndexChanged);
      // 
      // comboMapMultiColor1
      // 
      this.comboMapMultiColor1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboMapMultiColor1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboMapMultiColor1.FormattingEnabled = true;
      this.comboMapMultiColor1.Location = new System.Drawing.Point(63, 124);
      this.comboMapMultiColor1.Name = "comboMapMultiColor1";
      this.comboMapMultiColor1.Size = new System.Drawing.Size(82, 21);
      this.comboMapMultiColor1.TabIndex = 28;
      this.comboMapMultiColor1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboAlternativeColor_DrawItem);
      this.comboMapMultiColor1.SelectedIndexChanged += new System.EventHandler(this.comboMapMultiColor1_SelectedIndexChanged);
      // 
      // btnCopy
      // 
      this.btnCopy.Location = new System.Drawing.Point(231, 17);
      this.btnCopy.Name = "btnCopy";
      this.btnCopy.Size = new System.Drawing.Size(35, 23);
      this.btnCopy.TabIndex = 27;
      this.btnCopy.Text = "Cpy";
      this.btnCopy.UseVisualStyleBackColor = true;
      this.btnCopy.Click += new System.EventHandler(this.btnMapCopy_Click);
      // 
      // btnMapAdd
      // 
      this.btnMapAdd.Location = new System.Drawing.Point(191, 17);
      this.btnMapAdd.Name = "btnMapAdd";
      this.btnMapAdd.Size = new System.Drawing.Size(35, 23);
      this.btnMapAdd.TabIndex = 27;
      this.btnMapAdd.Text = "Add";
      this.btnMapAdd.UseVisualStyleBackColor = true;
      this.btnMapAdd.Click += new System.EventHandler(this.btnMapAdd_Click);
      // 
      // btnMapDelete
      // 
      this.btnMapDelete.Enabled = false;
      this.btnMapDelete.Location = new System.Drawing.Point(191, 69);
      this.btnMapDelete.Name = "btnMapDelete";
      this.btnMapDelete.Size = new System.Drawing.Size(75, 23);
      this.btnMapDelete.TabIndex = 27;
      this.btnMapDelete.Text = "Delete";
      this.btnMapDelete.UseVisualStyleBackColor = true;
      this.btnMapDelete.Click += new System.EventHandler(this.btnMapDelete_Click);
      // 
      // btnMapApply
      // 
      this.btnMapApply.Enabled = false;
      this.btnMapApply.Location = new System.Drawing.Point(191, 43);
      this.btnMapApply.Name = "btnMapApply";
      this.btnMapApply.Size = new System.Drawing.Size(75, 23);
      this.btnMapApply.TabIndex = 27;
      this.btnMapApply.Text = "Apply";
      this.btnMapApply.UseVisualStyleBackColor = true;
      this.btnMapApply.Click += new System.EventHandler(this.btnMapApply_Click);
      // 
      // label14
      // 
      this.label14.AutoSize = true;
      this.label14.Location = new System.Drawing.Point(148, 154);
      this.label14.Name = "label14";
      this.label14.Size = new System.Drawing.Size(31, 13);
      this.label14.TabIndex = 25;
      this.label14.Text = "BG4:";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(9, 22);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(30, 13);
      this.label1.TabIndex = 25;
      this.label1.Text = "Size:";
      // 
      // label22
      // 
      this.label22.AutoSize = true;
      this.label22.Location = new System.Drawing.Point(148, 127);
      this.label22.Name = "label22";
      this.label22.Size = new System.Drawing.Size(32, 13);
      this.label22.TabIndex = 25;
      this.label22.Text = "MC2:";
      // 
      // label23
      // 
      this.label23.AutoSize = true;
      this.label23.Location = new System.Drawing.Point(9, 154);
      this.label23.Name = "label23";
      this.label23.Size = new System.Drawing.Size(25, 13);
      this.label23.TabIndex = 25;
      this.label23.Text = "BG:";
      // 
      // editMapName
      // 
      this.editMapName.Location = new System.Drawing.Point(63, 71);
      this.editMapName.Name = "editMapName";
      this.editMapName.Size = new System.Drawing.Size(122, 20);
      this.editMapName.TabIndex = 26;
      // 
      // label13
      // 
      this.label13.AutoSize = true;
      this.label13.Location = new System.Drawing.Point(9, 100);
      this.label13.Name = "label13";
      this.label13.Size = new System.Drawing.Size(37, 13);
      this.label13.TabIndex = 25;
      this.label13.Text = "Mode:";
      // 
      // label21
      // 
      this.label21.AutoSize = true;
      this.label21.Location = new System.Drawing.Point(9, 127);
      this.label21.Name = "label21";
      this.label21.Size = new System.Drawing.Size(32, 13);
      this.label21.TabIndex = 25;
      this.label21.Text = "MC1:";
      // 
      // label18
      // 
      this.label18.AutoSize = true;
      this.label18.Location = new System.Drawing.Point(9, 74);
      this.label18.Name = "label18";
      this.label18.Size = new System.Drawing.Size(38, 13);
      this.label18.TabIndex = 25;
      this.label18.Text = "Name:";
      // 
      // editTileSpacingH
      // 
      this.editTileSpacingH.Location = new System.Drawing.Point(128, 45);
      this.editTileSpacingH.Name = "editTileSpacingH";
      this.editTileSpacingH.Size = new System.Drawing.Size(56, 20);
      this.editTileSpacingH.TabIndex = 26;
      this.editTileSpacingH.Text = "2";
      // 
      // editMapHeight
      // 
      this.editMapHeight.Location = new System.Drawing.Point(128, 19);
      this.editMapHeight.Name = "editMapHeight";
      this.editMapHeight.Size = new System.Drawing.Size(56, 20);
      this.editMapHeight.TabIndex = 26;
      this.editMapHeight.Text = "12";
      // 
      // editTileSpacingW
      // 
      this.editTileSpacingW.Location = new System.Drawing.Point(63, 45);
      this.editTileSpacingW.Name = "editTileSpacingW";
      this.editTileSpacingW.Size = new System.Drawing.Size(60, 20);
      this.editTileSpacingW.TabIndex = 26;
      this.editTileSpacingW.Text = "2";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(9, 48);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(45, 13);
      this.label3.TabIndex = 25;
      this.label3.Text = "Tilesize:";
      // 
      // editMapWidth
      // 
      this.editMapWidth.Location = new System.Drawing.Point(63, 19);
      this.editMapWidth.Name = "editMapWidth";
      this.editMapWidth.Size = new System.Drawing.Size(60, 20);
      this.editMapWidth.TabIndex = 26;
      this.editMapWidth.Text = "20";
      // 
      // mapHScroll
      // 
      this.mapHScroll.Location = new System.Drawing.Point(8, 413);
      this.mapHScroll.Name = "mapHScroll";
      this.mapHScroll.Size = new System.Drawing.Size(644, 16);
      this.mapHScroll.TabIndex = 24;
      this.mapHScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.mapHScroll_Scroll);
      // 
      // mapVScroll
      // 
      this.mapVScroll.Location = new System.Drawing.Point(655, 6);
      this.mapVScroll.Name = "mapVScroll";
      this.mapVScroll.Size = new System.Drawing.Size(16, 404);
      this.mapVScroll.TabIndex = 23;
      this.mapVScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.mapVScroll_Scroll);
      // 
      // label19
      // 
      this.label19.AutoSize = true;
      this.label19.Location = new System.Drawing.Point(678, 9);
      this.label19.Name = "label19";
      this.label19.Size = new System.Drawing.Size(68, 13);
      this.label19.TabIndex = 25;
      this.label19.Text = "Current Map:";
      // 
      // pictureEditor
      // 
      this.pictureEditor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.pictureEditor.DisplayPage = fastImage1;
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
      // tabMapEditor
      // 
      this.tabMapEditor.Controls.Add(this.tabEditor);
      this.tabMapEditor.Controls.Add(this.tabTiles);
      this.tabMapEditor.Controls.Add(this.tabProject);
      this.tabMapEditor.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabMapEditor.Location = new System.Drawing.Point(0, 24);
      this.tabMapEditor.Name = "tabMapEditor";
      this.tabMapEditor.SelectedIndex = 0;
      this.tabMapEditor.Size = new System.Drawing.Size(964, 501);
      this.tabMapEditor.TabIndex = 0;
      // 
      // tabTiles
      // 
      this.tabTiles.Controls.Add(this.btnCopyTileCharToNextIncreased);
      this.tabTiles.Controls.Add(this.btnSetNextTileChar);
      this.tabTiles.Controls.Add(this.comboTileMode);
      this.tabTiles.Controls.Add(this.btnMoveTileDown);
      this.tabTiles.Controls.Add(this.btnMoveTileUp);
      this.tabTiles.Controls.Add(this.btnTileDelete);
      this.tabTiles.Controls.Add(this.btnTileApply);
      this.tabTiles.Controls.Add(this.btnTileAdd);
      this.tabTiles.Controls.Add(this.listTileChars);
      this.tabTiles.Controls.Add(this.listTileInfo);
      this.tabTiles.Controls.Add(this.editTileName);
      this.tabTiles.Controls.Add(this.editTileHeight);
      this.tabTiles.Controls.Add(this.editTileWidth);
      this.tabTiles.Controls.Add(this.label17);
      this.tabTiles.Controls.Add(this.label24);
      this.tabTiles.Controls.Add(this.labelTilesBGColor4);
      this.tabTiles.Controls.Add(this.labelTilesMulticolor2);
      this.tabTiles.Controls.Add(this.label16);
      this.tabTiles.Controls.Add(this.labelTilesMulticolor1);
      this.tabTiles.Controls.Add(this.label4);
      this.tabTiles.Controls.Add(this.label15);
      this.tabTiles.Controls.Add(this.panelCharacters);
      this.tabTiles.Controls.Add(this.comboTileBGColor4);
      this.tabTiles.Controls.Add(this.comboTileMulticolor2);
      this.tabTiles.Controls.Add(this.comboTileMulticolor1);
      this.tabTiles.Controls.Add(this.comboTileBackground);
      this.tabTiles.Controls.Add(this.panelCharColors);
      this.tabTiles.Controls.Add(this.pictureTileDisplay);
      this.tabTiles.Location = new System.Drawing.Point(4, 22);
      this.tabTiles.Name = "tabTiles";
      this.tabTiles.Padding = new System.Windows.Forms.Padding(3);
      this.tabTiles.Size = new System.Drawing.Size(956, 475);
      this.tabTiles.TabIndex = 2;
      this.tabTiles.Text = "Tiles";
      this.tabTiles.UseVisualStyleBackColor = true;
      // 
      // comboTileMode
      // 
      this.comboTileMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboTileMode.FormattingEnabled = true;
      this.comboTileMode.Location = new System.Drawing.Point(701, 92);
      this.comboTileMode.Name = "comboTileMode";
      this.comboTileMode.Size = new System.Drawing.Size(121, 21);
      this.comboTileMode.TabIndex = 27;
      this.comboTileMode.SelectedIndexChanged += new System.EventHandler(this.comboTileMode_SelectedIndexChanged);
      // 
      // btnMoveTileDown
      // 
      this.btnMoveTileDown.Enabled = false;
      this.btnMoveTileDown.Location = new System.Drawing.Point(53, 217);
      this.btnMoveTileDown.Name = "btnMoveTileDown";
      this.btnMoveTileDown.Size = new System.Drawing.Size(44, 23);
      this.btnMoveTileDown.TabIndex = 26;
      this.btnMoveTileDown.Text = "Down";
      this.btnMoveTileDown.UseVisualStyleBackColor = true;
      this.btnMoveTileDown.Click += new System.EventHandler(this.btnMoveTileDown_Click);
      // 
      // btnMoveTileUp
      // 
      this.btnMoveTileUp.Enabled = false;
      this.btnMoveTileUp.Location = new System.Drawing.Point(3, 217);
      this.btnMoveTileUp.Name = "btnMoveTileUp";
      this.btnMoveTileUp.Size = new System.Drawing.Size(44, 23);
      this.btnMoveTileUp.TabIndex = 26;
      this.btnMoveTileUp.Text = "Up";
      this.btnMoveTileUp.UseVisualStyleBackColor = true;
      this.btnMoveTileUp.Click += new System.EventHandler(this.btnMoveTileUp_Click);
      // 
      // btnTileDelete
      // 
      this.btnTileDelete.Enabled = false;
      this.btnTileDelete.Location = new System.Drawing.Point(530, 90);
      this.btnTileDelete.Name = "btnTileDelete";
      this.btnTileDelete.Size = new System.Drawing.Size(75, 23);
      this.btnTileDelete.TabIndex = 25;
      this.btnTileDelete.Text = "Delete";
      this.btnTileDelete.UseVisualStyleBackColor = true;
      this.btnTileDelete.Click += new System.EventHandler(this.btnTileDelete_Click);
      // 
      // btnTileApply
      // 
      this.btnTileApply.Enabled = false;
      this.btnTileApply.Location = new System.Drawing.Point(449, 90);
      this.btnTileApply.Name = "btnTileApply";
      this.btnTileApply.Size = new System.Drawing.Size(75, 23);
      this.btnTileApply.TabIndex = 25;
      this.btnTileApply.Text = "Apply";
      this.btnTileApply.UseVisualStyleBackColor = true;
      this.btnTileApply.Click += new System.EventHandler(this.btnTileApply_Click);
      // 
      // btnTileAdd
      // 
      this.btnTileAdd.Location = new System.Drawing.Point(368, 90);
      this.btnTileAdd.Name = "btnTileAdd";
      this.btnTileAdd.Size = new System.Drawing.Size(75, 23);
      this.btnTileAdd.TabIndex = 25;
      this.btnTileAdd.Text = "Add";
      this.btnTileAdd.UseVisualStyleBackColor = true;
      this.btnTileAdd.Click += new System.EventHandler(this.btnAddTile_Click);
      // 
      // listTileChars
      // 
      this.listTileChars.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
      this.listTileChars.FullRowSelect = true;
      this.listTileChars.HideSelection = false;
      this.listTileChars.Location = new System.Drawing.Point(3, 247);
      this.listTileChars.Name = "listTileChars";
      this.listTileChars.Size = new System.Drawing.Size(176, 162);
      this.listTileChars.TabIndex = 24;
      this.listTileChars.UseCompatibleStateImageBehavior = false;
      this.listTileChars.View = System.Windows.Forms.View.Details;
      this.listTileChars.SelectedIndexChanged += new System.EventHandler(this.listTileChars_SelectedIndexChanged);
      // 
      // columnHeader5
      // 
      this.columnHeader5.Text = "Nr.";
      this.columnHeader5.Width = 35;
      // 
      // columnHeader6
      // 
      this.columnHeader6.Text = "Char";
      this.columnHeader6.Width = 50;
      // 
      // columnHeader7
      // 
      this.columnHeader7.Text = "Color";
      this.columnHeader7.Width = 50;
      // 
      // listTileInfo
      // 
      this.listTileInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
      this.listTileInfo.FullRowSelect = true;
      this.listTileInfo.HideSelection = false;
      this.listTileInfo.Location = new System.Drawing.Point(3, 8);
      this.listTileInfo.Name = "listTileInfo";
      this.listTileInfo.Size = new System.Drawing.Size(359, 203);
      this.listTileInfo.TabIndex = 24;
      this.listTileInfo.UseCompatibleStateImageBehavior = false;
      this.listTileInfo.View = System.Windows.Forms.View.Details;
      this.listTileInfo.SelectedIndexChanged += new System.EventHandler(this.listTileInfo_SelectedIndexChanged);
      // 
      // columnHeader4
      // 
      this.columnHeader4.Text = "Nr.";
      this.columnHeader4.Width = 35;
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Name";
      this.columnHeader1.Width = 200;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Info";
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "Used No.";
      // 
      // editTileName
      // 
      this.editTileName.Location = new System.Drawing.Point(432, 62);
      this.editTileName.Name = "editTileName";
      this.editTileName.Size = new System.Drawing.Size(100, 20);
      this.editTileName.TabIndex = 23;
      // 
      // editTileHeight
      // 
      this.editTileHeight.Location = new System.Drawing.Point(432, 35);
      this.editTileHeight.Name = "editTileHeight";
      this.editTileHeight.Size = new System.Drawing.Size(100, 20);
      this.editTileHeight.TabIndex = 23;
      // 
      // editTileWidth
      // 
      this.editTileWidth.Location = new System.Drawing.Point(432, 8);
      this.editTileWidth.Name = "editTileWidth";
      this.editTileWidth.Size = new System.Drawing.Size(100, 20);
      this.editTileWidth.TabIndex = 23;
      // 
      // label17
      // 
      this.label17.AutoSize = true;
      this.label17.Location = new System.Drawing.Point(368, 65);
      this.label17.Name = "label17";
      this.label17.Size = new System.Drawing.Size(38, 13);
      this.label17.TabIndex = 22;
      this.label17.Text = "Name:";
      // 
      // label24
      // 
      this.label24.AutoSize = true;
      this.label24.Location = new System.Drawing.Point(658, 95);
      this.label24.Name = "label24";
      this.label24.Size = new System.Drawing.Size(37, 13);
      this.label24.TabIndex = 22;
      this.label24.Text = "Mode:";
      // 
      // labelTilesBGColor4
      // 
      this.labelTilesBGColor4.AutoSize = true;
      this.labelTilesBGColor4.Location = new System.Drawing.Point(809, 37);
      this.labelTilesBGColor4.Name = "labelTilesBGColor4";
      this.labelTilesBGColor4.Size = new System.Drawing.Size(61, 13);
      this.labelTilesBGColor4.TabIndex = 22;
      this.labelTilesBGColor4.Text = "BG Color 4:";
      // 
      // labelTilesMulticolor2
      // 
      this.labelTilesMulticolor2.AutoSize = true;
      this.labelTilesMulticolor2.Location = new System.Drawing.Point(809, 11);
      this.labelTilesMulticolor2.Name = "labelTilesMulticolor2";
      this.labelTilesMulticolor2.Size = new System.Drawing.Size(64, 13);
      this.labelTilesMulticolor2.TabIndex = 22;
      this.labelTilesMulticolor2.Text = "Multicolor 2:";
      // 
      // label16
      // 
      this.label16.AutoSize = true;
      this.label16.Location = new System.Drawing.Point(368, 38);
      this.label16.Name = "label16";
      this.label16.Size = new System.Drawing.Size(61, 13);
      this.label16.TabIndex = 22;
      this.label16.Text = "Tile Height:";
      // 
      // labelTilesMulticolor1
      // 
      this.labelTilesMulticolor1.AutoSize = true;
      this.labelTilesMulticolor1.Location = new System.Drawing.Point(658, 38);
      this.labelTilesMulticolor1.Name = "labelTilesMulticolor1";
      this.labelTilesMulticolor1.Size = new System.Drawing.Size(64, 13);
      this.labelTilesMulticolor1.TabIndex = 22;
      this.labelTilesMulticolor1.Text = "Multicolor 1:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(368, 11);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(58, 13);
      this.label4.TabIndex = 22;
      this.label4.Text = "Tile Width:";
      // 
      // label15
      // 
      this.label15.AutoSize = true;
      this.label15.Location = new System.Drawing.Point(658, 11);
      this.label15.Name = "label15";
      this.label15.Size = new System.Drawing.Size(68, 13);
      this.label15.TabIndex = 22;
      this.label15.Text = "Background:";
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
      this.panelCharacters.Location = new System.Drawing.Point(371, 123);
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
      // comboTileBGColor4
      // 
      this.comboTileBGColor4.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboTileBGColor4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboTileBGColor4.FormattingEnabled = true;
      this.comboTileBGColor4.Location = new System.Drawing.Point(891, 34);
      this.comboTileBGColor4.Name = "comboTileBGColor4";
      this.comboTileBGColor4.Size = new System.Drawing.Size(59, 21);
      this.comboTileBGColor4.TabIndex = 1;
      this.comboTileBGColor4.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboColor_DrawItem);
      this.comboTileBGColor4.SelectedIndexChanged += new System.EventHandler(this.comboBGColor4_SelectedIndexChanged);
      // 
      // comboTileMulticolor2
      // 
      this.comboTileMulticolor2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboTileMulticolor2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboTileMulticolor2.FormattingEnabled = true;
      this.comboTileMulticolor2.Location = new System.Drawing.Point(891, 8);
      this.comboTileMulticolor2.Name = "comboTileMulticolor2";
      this.comboTileMulticolor2.Size = new System.Drawing.Size(59, 21);
      this.comboTileMulticolor2.TabIndex = 1;
      this.comboTileMulticolor2.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboColor_DrawItem);
      this.comboTileMulticolor2.SelectedIndexChanged += new System.EventHandler(this.comboMulticolor2_SelectedIndexChanged);
      // 
      // comboTileMulticolor1
      // 
      this.comboTileMulticolor1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboTileMulticolor1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboTileMulticolor1.FormattingEnabled = true;
      this.comboTileMulticolor1.Location = new System.Drawing.Point(740, 34);
      this.comboTileMulticolor1.Name = "comboTileMulticolor1";
      this.comboTileMulticolor1.Size = new System.Drawing.Size(59, 21);
      this.comboTileMulticolor1.TabIndex = 1;
      this.comboTileMulticolor1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboColor_DrawItem);
      this.comboTileMulticolor1.SelectedIndexChanged += new System.EventHandler(this.comboMulticolor1_SelectedIndexChanged);
      // 
      // comboTileBackground
      // 
      this.comboTileBackground.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboTileBackground.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboTileBackground.FormattingEnabled = true;
      this.comboTileBackground.Location = new System.Drawing.Point(740, 8);
      this.comboTileBackground.Name = "comboTileBackground";
      this.comboTileBackground.Size = new System.Drawing.Size(59, 21);
      this.comboTileBackground.TabIndex = 1;
      this.comboTileBackground.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboColor_DrawItem);
      this.comboTileBackground.SelectedIndexChanged += new System.EventHandler(this.comboBackground_SelectedIndexChanged_1);
      // 
      // panelCharColors
      // 
      this.panelCharColors.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panelCharColors.DisplayPage = fastImage2;
      this.panelCharColors.Image = null;
      this.panelCharColors.Location = new System.Drawing.Point(371, 389);
      this.panelCharColors.Name = "panelCharColors";
      this.panelCharColors.Size = new System.Drawing.Size(260, 20);
      this.panelCharColors.TabIndex = 0;
      this.panelCharColors.TabStop = false;
      this.panelCharColors.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelCharColors_MouseDown);
      this.panelCharColors.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelCharColors_MouseMove);
      // 
      // pictureTileDisplay
      // 
      this.pictureTileDisplay.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.pictureTileDisplay.DisplayPage = fastImage3;
      this.pictureTileDisplay.Image = null;
      this.pictureTileDisplay.Location = new System.Drawing.Point(642, 123);
      this.pictureTileDisplay.Name = "pictureTileDisplay";
      this.pictureTileDisplay.Size = new System.Drawing.Size(286, 286);
      this.pictureTileDisplay.TabIndex = 0;
      this.pictureTileDisplay.TabStop = false;
      // 
      // tabPage1
      // 
      this.tabPage1.Controls.Add(this.label7);
      this.tabPage1.Controls.Add(this.label8);
      this.tabPage1.Controls.Add(this.label9);
      this.tabPage1.Controls.Add(this.imageListbox1);
      this.tabPage1.Controls.Add(this.checkBox1);
      this.tabPage1.Controls.Add(this.comboBox1);
      this.tabPage1.Controls.Add(this.comboBox2);
      this.tabPage1.Controls.Add(this.comboBox3);
      this.tabPage1.Controls.Add(this.fastPictureBox1);
      this.tabPage1.Controls.Add(this.fastPictureBox2);
      this.tabPage1.Location = new System.Drawing.Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage1.Size = new System.Drawing.Size(956, 475);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "Screen";
      this.tabPage1.UseVisualStyleBackColor = true;
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(658, 65);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(64, 13);
      this.label7.TabIndex = 22;
      this.label7.Text = "Multicolor 2:";
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(658, 38);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(64, 13);
      this.label8.TabIndex = 22;
      this.label8.Text = "Multicolor 1:";
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(658, 11);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(68, 13);
      this.label9.TabIndex = 22;
      this.label9.Text = "Background:";
      // 
      // imageListbox1
      // 
      this.imageListbox1.AutoScroll = true;
      this.imageListbox1.AutoScrollHorizontalMaximum = 100;
      this.imageListbox1.AutoScrollHorizontalMinimum = 0;
      this.imageListbox1.AutoScrollHPos = 0;
      this.imageListbox1.AutoScrollVerticalMaximum = -23;
      this.imageListbox1.AutoScrollVerticalMinimum = 0;
      this.imageListbox1.AutoScrollVPos = 0;
      this.imageListbox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.imageListbox1.EnableAutoScrollHorizontal = true;
      this.imageListbox1.EnableAutoScrollVertical = true;
      this.imageListbox1.HottrackColor = ((uint)(2151694591u));
      this.imageListbox1.ItemHeight = 8;
      this.imageListbox1.ItemWidth = 8;
      this.imageListbox1.Location = new System.Drawing.Point(658, 124);
      this.imageListbox1.Name = "imageListbox1";
      this.imageListbox1.PixelFormat = System.Drawing.Imaging.PixelFormat.DontCare;
      this.imageListbox1.SelectedIndex = -1;
      this.imageListbox1.Size = new System.Drawing.Size(260, 260);
      this.imageListbox1.TabIndex = 21;
      this.imageListbox1.TabStop = true;
      this.imageListbox1.VisibleAutoScrollHorizontal = false;
      this.imageListbox1.VisibleAutoScrollVertical = false;
      // 
      // checkBox1
      // 
      this.checkBox1.AutoSize = true;
      this.checkBox1.Location = new System.Drawing.Point(658, 94);
      this.checkBox1.Name = "checkBox1";
      this.checkBox1.Size = new System.Drawing.Size(71, 17);
      this.checkBox1.TabIndex = 3;
      this.checkBox1.Text = "Multicolor";
      this.checkBox1.UseVisualStyleBackColor = true;
      // 
      // comboBox1
      // 
      this.comboBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBox1.FormattingEnabled = true;
      this.comboBox1.Location = new System.Drawing.Point(765, 62);
      this.comboBox1.Name = "comboBox1";
      this.comboBox1.Size = new System.Drawing.Size(121, 21);
      this.comboBox1.TabIndex = 1;
      // 
      // comboBox2
      // 
      this.comboBox2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBox2.FormattingEnabled = true;
      this.comboBox2.Location = new System.Drawing.Point(765, 35);
      this.comboBox2.Name = "comboBox2";
      this.comboBox2.Size = new System.Drawing.Size(121, 21);
      this.comboBox2.TabIndex = 1;
      // 
      // comboBox3
      // 
      this.comboBox3.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBox3.FormattingEnabled = true;
      this.comboBox3.Location = new System.Drawing.Point(765, 8);
      this.comboBox3.Name = "comboBox3";
      this.comboBox3.Size = new System.Drawing.Size(121, 21);
      this.comboBox3.TabIndex = 1;
      // 
      // fastPictureBox1
      // 
      this.fastPictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.fastPictureBox1.DisplayPage = fastImage4;
      this.fastPictureBox1.Image = null;
      this.fastPictureBox1.Location = new System.Drawing.Point(658, 390);
      this.fastPictureBox1.Name = "fastPictureBox1";
      this.fastPictureBox1.Size = new System.Drawing.Size(260, 20);
      this.fastPictureBox1.TabIndex = 0;
      this.fastPictureBox1.TabStop = false;
      // 
      // fastPictureBox2
      // 
      this.fastPictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.fastPictureBox2.DisplayPage = fastImage5;
      this.fastPictureBox2.Image = null;
      this.fastPictureBox2.Location = new System.Drawing.Point(8, 6);
      this.fastPictureBox2.Name = "fastPictureBox2";
      this.fastPictureBox2.Size = new System.Drawing.Size(644, 404);
      this.fastPictureBox2.TabIndex = 0;
      this.fastPictureBox2.TabStop = false;
      // 
      // tabPage2
      // 
      this.tabPage2.Controls.Add(this.groupBox2);
      this.tabPage2.Controls.Add(this.groupBox3);
      this.tabPage2.Location = new System.Drawing.Point(4, 22);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage2.Size = new System.Drawing.Size(956, 475);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "Project";
      this.tabPage2.UseVisualStyleBackColor = true;
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.button1);
      this.groupBox2.Controls.Add(this.button2);
      this.groupBox2.Location = new System.Drawing.Point(458, 6);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(324, 343);
      this.groupBox2.TabIndex = 4;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Import";
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(6, 48);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(117, 23);
      this.button1.TabIndex = 2;
      this.button1.Text = "Charset...";
      this.button1.UseVisualStyleBackColor = true;
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(6, 19);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(117, 23);
      this.button2.TabIndex = 2;
      this.button2.Text = "From File...";
      this.button2.UseVisualStyleBackColor = true;
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.label10);
      this.groupBox3.Controls.Add(this.comboBox4);
      this.groupBox3.Controls.Add(this.label11);
      this.groupBox3.Controls.Add(this.comboBox5);
      this.groupBox3.Controls.Add(this.comboBox6);
      this.groupBox3.Controls.Add(this.textBox1);
      this.groupBox3.Controls.Add(this.label12);
      this.groupBox3.Controls.Add(this.textBox2);
      this.groupBox3.Controls.Add(this.checkBox2);
      this.groupBox3.Controls.Add(this.checkBox3);
      this.groupBox3.Controls.Add(this.textBox3);
      this.groupBox3.Controls.Add(this.button3);
      this.groupBox3.Controls.Add(this.button4);
      this.groupBox3.Controls.Add(this.button5);
      this.groupBox3.Location = new System.Drawing.Point(11, 6);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(441, 343);
      this.groupBox3.TabIndex = 3;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Export";
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(3, 24);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(66, 13);
      this.label10.TabIndex = 11;
      this.label10.Text = "Export Data:";
      // 
      // comboBox4
      // 
      this.comboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBox4.FormattingEnabled = true;
      this.comboBox4.Items.AddRange(new object[] {
            "characters, then colors",
            "characters only",
            "colors only",
            "colors, then characters"});
      this.comboBox4.Location = new System.Drawing.Point(118, 21);
      this.comboBox4.Name = "comboBox4";
      this.comboBox4.Size = new System.Drawing.Size(317, 21);
      this.comboBox4.TabIndex = 10;
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Location = new System.Drawing.Point(129, 111);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(61, 13);
      this.label11.TabIndex = 9;
      this.label11.Text = "Orientation:";
      // 
      // comboBox5
      // 
      this.comboBox5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBox5.FormattingEnabled = true;
      this.comboBox5.Items.AddRange(new object[] {
            "row by row",
            "column by column"});
      this.comboBox5.Location = new System.Drawing.Point(217, 106);
      this.comboBox5.Name = "comboBox5";
      this.comboBox5.Size = new System.Drawing.Size(131, 21);
      this.comboBox5.TabIndex = 8;
      // 
      // comboBox6
      // 
      this.comboBox6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBox6.FormattingEnabled = true;
      this.comboBox6.Location = new System.Drawing.Point(118, 48);
      this.comboBox6.Name = "comboBox6";
      this.comboBox6.Size = new System.Drawing.Size(160, 21);
      this.comboBox6.TabIndex = 8;
      // 
      // textBox1
      // 
      this.textBox1.Location = new System.Drawing.Point(214, 77);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(43, 20);
      this.textBox1.TabIndex = 7;
      this.textBox1.Text = "!byte ";
      // 
      // label12
      // 
      this.label12.AutoSize = true;
      this.label12.Location = new System.Drawing.Point(394, 80);
      this.label12.Name = "label12";
      this.label12.Size = new System.Drawing.Size(32, 13);
      this.label12.TabIndex = 6;
      this.label12.Text = "bytes";
      // 
      // textBox2
      // 
      this.textBox2.Enabled = false;
      this.textBox2.Location = new System.Drawing.Point(347, 77);
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new System.Drawing.Size(41, 20);
      this.textBox2.TabIndex = 5;
      this.textBox2.Text = "40";
      // 
      // checkBox2
      // 
      this.checkBox2.AutoSize = true;
      this.checkBox2.Location = new System.Drawing.Point(264, 79);
      this.checkBox2.Name = "checkBox2";
      this.checkBox2.Size = new System.Drawing.Size(64, 17);
      this.checkBox2.TabIndex = 4;
      this.checkBox2.Text = "Wrap at";
      this.checkBox2.UseVisualStyleBackColor = true;
      // 
      // checkBox3
      // 
      this.checkBox3.AutoSize = true;
      this.checkBox3.Location = new System.Drawing.Point(118, 79);
      this.checkBox3.Name = "checkBox3";
      this.checkBox3.Size = new System.Drawing.Size(74, 17);
      this.checkBox3.TabIndex = 4;
      this.checkBox3.Text = "Prefix with";
      this.checkBox3.UseVisualStyleBackColor = true;
      // 
      // textBox3
      // 
      this.textBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBox3.Location = new System.Drawing.Point(6, 139);
      this.textBox3.Multiline = true;
      this.textBox3.Name = "textBox3";
      this.textBox3.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.textBox3.Size = new System.Drawing.Size(429, 198);
      this.textBox3.TabIndex = 3;
      this.textBox3.WordWrap = false;
      // 
      // button3
      // 
      this.button3.Location = new System.Drawing.Point(6, 48);
      this.button3.Name = "button3";
      this.button3.Size = new System.Drawing.Size(106, 23);
      this.button3.TabIndex = 2;
      this.button3.Text = "to Basic...";
      this.button3.UseVisualStyleBackColor = true;
      // 
      // button4
      // 
      this.button4.Location = new System.Drawing.Point(6, 106);
      this.button4.Name = "button4";
      this.button4.Size = new System.Drawing.Size(106, 23);
      this.button4.TabIndex = 2;
      this.button4.Text = "as binary file";
      this.button4.UseVisualStyleBackColor = true;
      // 
      // button5
      // 
      this.button5.Location = new System.Drawing.Point(6, 77);
      this.button5.Name = "button5";
      this.button5.Size = new System.Drawing.Size(106, 23);
      this.button5.TabIndex = 2;
      this.button5.Text = "as assembly source";
      this.button5.UseVisualStyleBackColor = true;
      // 
      // btnSetNextTileChar
      // 
      this.btnSetNextTileChar.Enabled = false;
      this.btnSetNextTileChar.Location = new System.Drawing.Point(192, 247);
      this.btnSetNextTileChar.Name = "btnSetNextTileChar";
      this.btnSetNextTileChar.Size = new System.Drawing.Size(75, 23);
      this.btnSetNextTileChar.TabIndex = 28;
      this.btnSetNextTileChar.Text = "Copy to next";
      this.toolTip1.SetToolTip(this.btnSetNextTileChar, "Copy char/color to next slot");
      this.btnSetNextTileChar.UseVisualStyleBackColor = true;
      this.btnSetNextTileChar.Click += new System.EventHandler(this.btnSetNextTileChar_Click);
      // 
      // btnCopyTileCharToNextIncreased
      // 
      this.btnCopyTileCharToNextIncreased.Enabled = false;
      this.btnCopyTileCharToNextIncreased.Location = new System.Drawing.Point(273, 247);
      this.btnCopyTileCharToNextIncreased.Name = "btnCopyTileCharToNextIncreased";
      this.btnCopyTileCharToNextIncreased.Size = new System.Drawing.Size(75, 23);
      this.btnCopyTileCharToNextIncreased.TabIndex = 28;
      this.btnCopyTileCharToNextIncreased.Text = "Copy inc\'ed";
      this.toolTip1.SetToolTip(this.btnCopyTileCharToNextIncreased, "Copy char+1/color to next slot");
      this.btnCopyTileCharToNextIncreased.UseVisualStyleBackColor = true;
      this.btnCopyTileCharToNextIncreased.Click += new System.EventHandler(this.btnCopyTileCharToNextIncreased_Click);
      // 
      // MapEditor
      // 
      this.ClientSize = new System.Drawing.Size(964, 525);
      this.Controls.Add(this.tabMapEditor);
      this.Controls.Add(this.menuStrip1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "MapEditor";
      this.Text = "Map Editor";
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.tabProject.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupExport.ResumeLayout(false);
      this.groupExport.PerformLayout();
      this.tabEditor.ResumeLayout(false);
      this.tabEditor.PerformLayout();
      this.groupMapExtraData.ResumeLayout(false);
      this.groupMapExtraData.PerformLayout();
      this.groupSize.ResumeLayout(false);
      this.groupSize.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureEditor)).EndInit();
      this.tabMapEditor.ResumeLayout(false);
      this.tabTiles.ResumeLayout(false);
      this.tabTiles.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.panelCharColors)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureTileDisplay)).EndInit();
      this.tabPage1.ResumeLayout(false);
      this.tabPage1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.fastPictureBox1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.fastPictureBox2)).EndInit();
      this.tabPage2.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
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
    private GR.Forms.FastPictureBox pictureEditor;
    private System.Windows.Forms.TabControl tabMapEditor;
    private System.Windows.Forms.Button btnImportCharset;
    private System.Windows.Forms.ComboBox comboExportOrientation;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Button btnExportToFile;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.ComboBox comboExportData;
    private System.Windows.Forms.TabPage tabTiles;
    private System.Windows.Forms.Label labelTilesMulticolor2;
    private System.Windows.Forms.Label labelTilesMulticolor1;
    private System.Windows.Forms.Label label15;
    private GR.Forms.ImageListbox panelCharacters;
    private System.Windows.Forms.ComboBox comboTileMulticolor2;
    private System.Windows.Forms.ComboBox comboTileMulticolor1;
    private System.Windows.Forms.ComboBox comboTileBackground;
    private GR.Forms.FastPictureBox panelCharColors;
    private GR.Forms.FastPictureBox pictureTileDisplay;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label label9;
    private GR.Forms.ImageListbox imageListbox1;
    private System.Windows.Forms.CheckBox checkBox1;
    private System.Windows.Forms.ComboBox comboBox1;
    private System.Windows.Forms.ComboBox comboBox2;
    private System.Windows.Forms.ComboBox comboBox3;
    private GR.Forms.FastPictureBox fastPictureBox1;
    private GR.Forms.FastPictureBox fastPictureBox2;
    private System.Windows.Forms.TabPage tabPage2;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.ComboBox comboBox4;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.ComboBox comboBox5;
    private System.Windows.Forms.ComboBox comboBox6;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.TextBox textBox2;
    private System.Windows.Forms.CheckBox checkBox2;
    private System.Windows.Forms.CheckBox checkBox3;
    private System.Windows.Forms.TextBox textBox3;
    private System.Windows.Forms.Button button3;
    private System.Windows.Forms.Button button4;
    private System.Windows.Forms.Button button5;
    private System.Windows.Forms.VScrollBar mapVScroll;
    private System.Windows.Forms.HScrollBar mapHScroll;
    private System.Windows.Forms.GroupBox groupSize;
    private System.Windows.Forms.Button btnMapApply;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox editMapHeight;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox editMapWidth;
    private System.Windows.Forms.ListView listTileInfo;
    private System.Windows.Forms.TextBox editTileName;
    private System.Windows.Forms.TextBox editTileHeight;
    private System.Windows.Forms.TextBox editTileWidth;
    private System.Windows.Forms.Label label17;
    private System.Windows.Forms.Label label16;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Button btnTileDelete;
    private System.Windows.Forms.Button btnTileApply;
    private System.Windows.Forms.Button btnTileAdd;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.ColumnHeader columnHeader4;
    private System.Windows.Forms.Button btnMapAdd;
    private System.Windows.Forms.TextBox editMapName;
    private System.Windows.Forms.Label label18;
    private System.Windows.Forms.Button btnMapDelete;
    private System.Windows.Forms.ComboBox comboMaps;
    private System.Windows.Forms.Label label19;
    private System.Windows.Forms.ListView listTileChars;
    private System.Windows.Forms.ColumnHeader columnHeader5;
    private System.Windows.Forms.ColumnHeader columnHeader6;
    private System.Windows.Forms.ColumnHeader columnHeader7;
    private System.Windows.Forms.TextBox editTileSpacingH;
    private System.Windows.Forms.TextBox editTileSpacingW;
    private System.Windows.Forms.ComboBox comboTiles;
    private System.Windows.Forms.Button btnMoveTileDown;
    private System.Windows.Forms.Button btnMoveTileUp;
    private System.Windows.Forms.GroupBox groupMapExtraData;
    private System.Windows.Forms.TextBox editMapExtraData;
    private System.Windows.Forms.Label label20;
    private System.Windows.Forms.Label labelEditInfo;
    private System.Windows.Forms.RadioButton btnToolSelect;
    private System.Windows.Forms.RadioButton btnToolFill;
    private System.Windows.Forms.RadioButton btnToolQuad;
    private System.Windows.Forms.RadioButton btnToolRect;
    private System.Windows.Forms.RadioButton btnToolEdit;
    private System.Windows.Forms.Button btnCopy;
    private System.Windows.Forms.ComboBox comboMapMultiColor2;
    private System.Windows.Forms.ComboBox comboMapMultiColor1;
    private System.Windows.Forms.Label label21;
    private System.Windows.Forms.ComboBox comboMapBGColor;
    private System.Windows.Forms.Label label22;
    private System.Windows.Forms.Label label23;
    private System.Windows.Forms.ComboBox comboTileMode;
    private System.Windows.Forms.Label label24;
    private System.Windows.Forms.Label labelTilesBGColor4;
    private System.Windows.Forms.ComboBox comboTileBGColor4;
    private System.Windows.Forms.ComboBox comboMapAlternativeMode;
    private System.Windows.Forms.Label label13;
    private System.Windows.Forms.ComboBox comboMapAlternativeBGColor4;
    private System.Windows.Forms.Label label14;
    private System.Windows.Forms.Button btnCopyTileCharToNextIncreased;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.Button btnSetNextTileChar;



  }
}
