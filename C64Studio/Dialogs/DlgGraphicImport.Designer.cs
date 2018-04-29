namespace C64Studio
{
  partial class DlgGraphicImport
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DlgGraphicImport));
      GR.Image.FastImage fastImage1 = new GR.Image.FastImage();
      GR.Image.FastImage fastImage2 = new GR.Image.FastImage();
      this.tabImportSettings = new System.Windows.Forms.TabControl();
      this.tabSettings = new System.Windows.Forms.TabPage();
      this.groupBox5 = new System.Windows.Forms.GroupBox();
      this.checkPasteAsBlock = new System.Windows.Forms.CheckBox();
      this.groupBox4 = new System.Windows.Forms.GroupBox();
      this.label3 = new System.Windows.Forms.Label();
      this.listDirectReplaceColors = new System.Windows.Forms.ListView();
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.comboColorMatching = new System.Windows.Forms.ComboBox();
      this.label2 = new System.Windows.Forms.Label();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.comboImportType = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.comboBackground = new System.Windows.Forms.ComboBox();
      this.radioMulticolor2 = new System.Windows.Forms.Label();
      this.comboMulticolor1 = new System.Windows.Forms.ComboBox();
      this.radioMultiColor1 = new System.Windows.Forms.Label();
      this.comboMulticolor2 = new System.Windows.Forms.ComboBox();
      this.radioBackground = new System.Windows.Forms.Label();
      this.tabPalette = new System.Windows.Forms.TabPage();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.listProblems = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.menuImport = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.btnZoomIn = new System.Windows.Forms.Button();
      this.btnZoomOut = new System.Windows.Forms.Button();
      this.btnReload = new System.Windows.Forms.Button();
      this.contextMenuOrigPic = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.forceTargetColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.blackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.whiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.redToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.cyanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.purpleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.greenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.blueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.yellowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.orangeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.brownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.lightRedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.darkGreyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.mediumGreyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.lightGreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.lightBlueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.lightGreyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.picPreview = new GR.Forms.FastPictureBox();
      this.picOriginal = new GR.Forms.FastPictureBox();
      this.tabImportSettings.SuspendLayout();
      this.tabSettings.SuspendLayout();
      this.groupBox5.SuspendLayout();
      this.groupBox4.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.menuImport.SuspendLayout();
      this.contextMenuOrigPic.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picOriginal)).BeginInit();
      this.SuspendLayout();
      // 
      // tabImportSettings
      // 
      this.tabImportSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.tabImportSettings.Controls.Add(this.tabSettings);
      this.tabImportSettings.Controls.Add(this.tabPalette);
      this.tabImportSettings.Location = new System.Drawing.Point(650, 29);
      this.tabImportSettings.Name = "tabImportSettings";
      this.tabImportSettings.SelectedIndex = 0;
      this.tabImportSettings.Size = new System.Drawing.Size(291, 476);
      this.tabImportSettings.TabIndex = 2;
      // 
      // tabSettings
      // 
      this.tabSettings.Controls.Add(this.groupBox5);
      this.tabSettings.Controls.Add(this.groupBox4);
      this.tabSettings.Controls.Add(this.groupBox3);
      this.tabSettings.Controls.Add(this.groupBox2);
      this.tabSettings.Controls.Add(this.groupBox1);
      this.tabSettings.Location = new System.Drawing.Point(4, 22);
      this.tabSettings.Name = "tabSettings";
      this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
      this.tabSettings.Size = new System.Drawing.Size(283, 450);
      this.tabSettings.TabIndex = 0;
      this.tabSettings.Text = "Settings";
      this.tabSettings.UseVisualStyleBackColor = true;
      // 
      // groupBox5
      // 
      this.groupBox5.Controls.Add(this.checkPasteAsBlock);
      this.groupBox5.Location = new System.Drawing.Point(6, 388);
      this.groupBox5.Name = "groupBox5";
      this.groupBox5.Size = new System.Drawing.Size(265, 56);
      this.groupBox5.TabIndex = 12;
      this.groupBox5.TabStop = false;
      this.groupBox5.Text = "Paste Options";
      // 
      // checkPasteAsBlock
      // 
      this.checkPasteAsBlock.AutoSize = true;
      this.checkPasteAsBlock.Location = new System.Drawing.Point(9, 19);
      this.checkPasteAsBlock.Name = "checkPasteAsBlock";
      this.checkPasteAsBlock.Size = new System.Drawing.Size(97, 17);
      this.checkPasteAsBlock.TabIndex = 0;
      this.checkPasteAsBlock.Text = "Paste as Block";
      this.checkPasteAsBlock.UseVisualStyleBackColor = true;
      this.checkPasteAsBlock.CheckedChanged += new System.EventHandler(this.checkPasteAsBlock_CheckedChanged);
      // 
      // groupBox4
      // 
      this.groupBox4.Controls.Add(this.label3);
      this.groupBox4.Controls.Add(this.listDirectReplaceColors);
      this.groupBox4.Location = new System.Drawing.Point(6, 228);
      this.groupBox4.Name = "groupBox4";
      this.groupBox4.Size = new System.Drawing.Size(271, 154);
      this.groupBox4.TabIndex = 11;
      this.groupBox4.TabStop = false;
      this.groupBox4.Text = "Directly replace colors";
      // 
      // label3
      // 
      this.label3.Location = new System.Drawing.Point(6, 119);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(259, 32);
      this.label3.TabIndex = 1;
      this.label3.Text = "Right click on a source pixel to force a replacement color";
      // 
      // listDirectReplaceColors
      // 
      this.listDirectReplaceColors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
      this.listDirectReplaceColors.Location = new System.Drawing.Point(6, 19);
      this.listDirectReplaceColors.Name = "listDirectReplaceColors";
      this.listDirectReplaceColors.Size = new System.Drawing.Size(259, 97);
      this.listDirectReplaceColors.TabIndex = 0;
      this.listDirectReplaceColors.UseCompatibleStateImageBehavior = false;
      this.listDirectReplaceColors.View = System.Windows.Forms.View.Details;
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "Original Color";
      this.columnHeader3.Width = 111;
      // 
      // columnHeader4
      // 
      this.columnHeader4.Text = "Replace With";
      this.columnHeader4.Width = 141;
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.comboColorMatching);
      this.groupBox3.Controls.Add(this.label2);
      this.groupBox3.Location = new System.Drawing.Point(6, 168);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(271, 51);
      this.groupBox3.TabIndex = 10;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Color Matching";
      // 
      // comboColorMatching
      // 
      this.comboColorMatching.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboColorMatching.FormattingEnabled = true;
      this.comboColorMatching.Location = new System.Drawing.Point(65, 16);
      this.comboColorMatching.Name = "comboColorMatching";
      this.comboColorMatching.Size = new System.Drawing.Size(200, 21);
      this.comboColorMatching.TabIndex = 1;
      this.comboColorMatching.SelectedIndexChanged += new System.EventHandler(this.comboColorMatching_SelectedIndexChanged);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(6, 19);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(54, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "Match by:";
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.comboImportType);
      this.groupBox2.Controls.Add(this.label1);
      this.groupBox2.Location = new System.Drawing.Point(6, 6);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(271, 51);
      this.groupBox2.TabIndex = 10;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Import Settings";
      // 
      // comboImportType
      // 
      this.comboImportType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboImportType.FormattingEnabled = true;
      this.comboImportType.Location = new System.Drawing.Point(65, 16);
      this.comboImportType.Name = "comboImportType";
      this.comboImportType.Size = new System.Drawing.Size(200, 21);
      this.comboImportType.TabIndex = 1;
      this.comboImportType.SelectedIndexChanged += new System.EventHandler(this.comboImportType_SelectedIndexChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 19);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(53, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Import as:";
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.comboBackground);
      this.groupBox1.Controls.Add(this.radioMulticolor2);
      this.groupBox1.Controls.Add(this.comboMulticolor1);
      this.groupBox1.Controls.Add(this.radioMultiColor1);
      this.groupBox1.Controls.Add(this.comboMulticolor2);
      this.groupBox1.Controls.Add(this.radioBackground);
      this.groupBox1.Location = new System.Drawing.Point(6, 63);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(271, 99);
      this.groupBox1.TabIndex = 9;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Predefined Colors";
      // 
      // comboBackground
      // 
      this.comboBackground.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboBackground.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBackground.FormattingEnabled = true;
      this.comboBackground.Location = new System.Drawing.Point(113, 19);
      this.comboBackground.Name = "comboBackground";
      this.comboBackground.Size = new System.Drawing.Size(121, 21);
      this.comboBackground.TabIndex = 5;
      this.comboBackground.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.combo_DrawItem);
      this.comboBackground.SelectedIndexChanged += new System.EventHandler(this.comboBackground_SelectedIndexChanged);
      // 
      // radioMulticolor2
      // 
      this.radioMulticolor2.AutoSize = true;
      this.radioMulticolor2.Location = new System.Drawing.Point(6, 73);
      this.radioMulticolor2.Name = "radioMulticolor2";
      this.radioMulticolor2.Size = new System.Drawing.Size(61, 13);
      this.radioMulticolor2.TabIndex = 6;
      this.radioMulticolor2.TabStop = true;
      this.radioMulticolor2.Text = "Multicolor 2";
      // 
      // comboMulticolor1
      // 
      this.comboMulticolor1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboMulticolor1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboMulticolor1.FormattingEnabled = true;
      this.comboMulticolor1.Location = new System.Drawing.Point(113, 46);
      this.comboMulticolor1.Name = "comboMulticolor1";
      this.comboMulticolor1.Size = new System.Drawing.Size(121, 21);
      this.comboMulticolor1.TabIndex = 3;
      this.comboMulticolor1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.combo_DrawItem);
      this.comboMulticolor1.SelectedIndexChanged += new System.EventHandler(this.comboMulticolor1_SelectedIndexChanged);
      // 
      // radioMultiColor1
      // 
      this.radioMultiColor1.AutoSize = true;
      this.radioMultiColor1.Location = new System.Drawing.Point(6, 46);
      this.radioMultiColor1.Name = "radioMultiColor1";
      this.radioMultiColor1.Size = new System.Drawing.Size(61, 13);
      this.radioMultiColor1.TabIndex = 7;
      this.radioMultiColor1.TabStop = true;
      this.radioMultiColor1.Text = "Multicolor 1";
      // 
      // comboMulticolor2
      // 
      this.comboMulticolor2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboMulticolor2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboMulticolor2.FormattingEnabled = true;
      this.comboMulticolor2.Location = new System.Drawing.Point(113, 73);
      this.comboMulticolor2.Name = "comboMulticolor2";
      this.comboMulticolor2.Size = new System.Drawing.Size(121, 21);
      this.comboMulticolor2.TabIndex = 4;
      this.comboMulticolor2.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.combo_DrawItem);
      this.comboMulticolor2.SelectedIndexChanged += new System.EventHandler(this.comboMulticolor2_SelectedIndexChanged);
      // 
      // radioBackground
      // 
      this.radioBackground.AutoSize = true;
      this.radioBackground.Location = new System.Drawing.Point(6, 19);
      this.radioBackground.Name = "radioBackground";
      this.radioBackground.Size = new System.Drawing.Size(65, 13);
      this.radioBackground.TabIndex = 8;
      this.radioBackground.TabStop = true;
      this.radioBackground.Text = "Background";
      // 
      // tabPalette
      // 
      this.tabPalette.Location = new System.Drawing.Point(4, 22);
      this.tabPalette.Name = "tabPalette";
      this.tabPalette.Padding = new System.Windows.Forms.Padding(3);
      this.tabPalette.Size = new System.Drawing.Size(283, 450);
      this.tabPalette.TabIndex = 1;
      this.tabPalette.Text = "Palette";
      this.tabPalette.UseVisualStyleBackColor = true;
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(862, 820);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // btnOK
      // 
      this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnOK.Location = new System.Drawing.Point(781, 820);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 3;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // listProblems
      // 
      this.listProblems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.listProblems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
      this.listProblems.FullRowSelect = true;
      this.listProblems.Location = new System.Drawing.Point(650, 511);
      this.listProblems.Name = "listProblems";
      this.listProblems.Size = new System.Drawing.Size(287, 303);
      this.listProblems.TabIndex = 4;
      this.listProblems.UseCompatibleStateImageBehavior = false;
      this.listProblems.View = System.Windows.Forms.View.Details;
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Pos";
      this.columnHeader1.Width = 70;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Problem";
      this.columnHeader2.Width = 190;
      // 
      // menuImport
      // 
      this.menuImport.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
      this.menuImport.Location = new System.Drawing.Point(0, 0);
      this.menuImport.Name = "menuImport";
      this.menuImport.Size = new System.Drawing.Size(939, 24);
      this.menuImport.TabIndex = 5;
      this.menuImport.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.exitToolStripMenuItem});
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
      this.fileToolStripMenuItem.Text = "&File";
      // 
      // openToolStripMenuItem
      // 
      this.openToolStripMenuItem.Name = "openToolStripMenuItem";
      this.openToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
      this.openToolStripMenuItem.Text = "&Open...";
      this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
      // 
      // exitToolStripMenuItem
      // 
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
      this.exitToolStripMenuItem.Text = "E&xit";
      this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
      // 
      // btnZoomIn
      // 
      this.btnZoomIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomIn.Image")));
      this.btnZoomIn.Location = new System.Drawing.Point(650, 819);
      this.btnZoomIn.Name = "btnZoomIn";
      this.btnZoomIn.Size = new System.Drawing.Size(24, 24);
      this.btnZoomIn.TabIndex = 6;
      this.toolTip1.SetToolTip(this.btnZoomIn, "Zoom In");
      this.btnZoomIn.UseVisualStyleBackColor = true;
      this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
      // 
      // btnZoomOut
      // 
      this.btnZoomOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomOut.Image")));
      this.btnZoomOut.Location = new System.Drawing.Point(680, 819);
      this.btnZoomOut.Name = "btnZoomOut";
      this.btnZoomOut.Size = new System.Drawing.Size(24, 24);
      this.btnZoomOut.TabIndex = 6;
      this.toolTip1.SetToolTip(this.btnZoomOut, "Zoom Out");
      this.btnZoomOut.UseVisualStyleBackColor = true;
      this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
      // 
      // btnReload
      // 
      this.btnReload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnReload.Image = ((System.Drawing.Image)(resources.GetObject("btnReload.Image")));
      this.btnReload.Location = new System.Drawing.Point(710, 819);
      this.btnReload.Name = "btnReload";
      this.btnReload.Size = new System.Drawing.Size(24, 24);
      this.btnReload.TabIndex = 6;
      this.toolTip1.SetToolTip(this.btnReload, "Reload from File");
      this.btnReload.UseVisualStyleBackColor = true;
      this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
      // 
      // contextMenuOrigPic
      // 
      this.contextMenuOrigPic.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.forceTargetColorToolStripMenuItem});
      this.contextMenuOrigPic.Name = "contextMenuStrip1";
      this.contextMenuOrigPic.Size = new System.Drawing.Size(168, 26);
      this.contextMenuOrigPic.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuOrigPic_Opening);
      // 
      // forceTargetColorToolStripMenuItem
      // 
      this.forceTargetColorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.blackToolStripMenuItem,
            this.whiteToolStripMenuItem,
            this.redToolStripMenuItem,
            this.cyanToolStripMenuItem,
            this.purpleToolStripMenuItem,
            this.greenToolStripMenuItem,
            this.blueToolStripMenuItem,
            this.yellowToolStripMenuItem,
            this.orangeToolStripMenuItem,
            this.brownToolStripMenuItem,
            this.lightRedToolStripMenuItem,
            this.darkGreyToolStripMenuItem,
            this.mediumGreyToolStripMenuItem,
            this.lightGreenToolStripMenuItem,
            this.lightBlueToolStripMenuItem,
            this.lightGreyToolStripMenuItem});
      this.forceTargetColorToolStripMenuItem.Name = "forceTargetColorToolStripMenuItem";
      this.forceTargetColorToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
      this.forceTargetColorToolStripMenuItem.Text = "Force target color";
      // 
      // blackToolStripMenuItem
      // 
      this.blackToolStripMenuItem.Name = "blackToolStripMenuItem";
      this.blackToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
      this.blackToolStripMenuItem.Tag = "0";
      this.blackToolStripMenuItem.Text = "0 - Black";
      this.blackToolStripMenuItem.Click += new System.EventHandler(this.replaceColorMenuItem_Click);
      // 
      // whiteToolStripMenuItem
      // 
      this.whiteToolStripMenuItem.Name = "whiteToolStripMenuItem";
      this.whiteToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
      this.whiteToolStripMenuItem.Tag = "1";
      this.whiteToolStripMenuItem.Text = "1 - White";
      this.whiteToolStripMenuItem.Click += new System.EventHandler(this.replaceColorMenuItem_Click);
      // 
      // redToolStripMenuItem
      // 
      this.redToolStripMenuItem.Name = "redToolStripMenuItem";
      this.redToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
      this.redToolStripMenuItem.Tag = "2";
      this.redToolStripMenuItem.Text = "2 - Red";
      this.redToolStripMenuItem.Click += new System.EventHandler(this.replaceColorMenuItem_Click);
      // 
      // cyanToolStripMenuItem
      // 
      this.cyanToolStripMenuItem.Name = "cyanToolStripMenuItem";
      this.cyanToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
      this.cyanToolStripMenuItem.Tag = "3";
      this.cyanToolStripMenuItem.Text = "3 - Cyan";
      this.cyanToolStripMenuItem.Click += new System.EventHandler(this.replaceColorMenuItem_Click);
      // 
      // purpleToolStripMenuItem
      // 
      this.purpleToolStripMenuItem.Name = "purpleToolStripMenuItem";
      this.purpleToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
      this.purpleToolStripMenuItem.Tag = "4";
      this.purpleToolStripMenuItem.Text = "4 - Purple";
      this.purpleToolStripMenuItem.Click += new System.EventHandler(this.replaceColorMenuItem_Click);
      // 
      // greenToolStripMenuItem
      // 
      this.greenToolStripMenuItem.Name = "greenToolStripMenuItem";
      this.greenToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
      this.greenToolStripMenuItem.Tag = "5";
      this.greenToolStripMenuItem.Text = "5 - Green";
      this.greenToolStripMenuItem.Click += new System.EventHandler(this.replaceColorMenuItem_Click);
      // 
      // blueToolStripMenuItem
      // 
      this.blueToolStripMenuItem.Name = "blueToolStripMenuItem";
      this.blueToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
      this.blueToolStripMenuItem.Tag = "6";
      this.blueToolStripMenuItem.Text = "6 - Blue";
      this.blueToolStripMenuItem.Click += new System.EventHandler(this.replaceColorMenuItem_Click);
      // 
      // yellowToolStripMenuItem
      // 
      this.yellowToolStripMenuItem.Name = "yellowToolStripMenuItem";
      this.yellowToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
      this.yellowToolStripMenuItem.Tag = "7";
      this.yellowToolStripMenuItem.Text = "7 - Yellow";
      this.yellowToolStripMenuItem.Click += new System.EventHandler(this.replaceColorMenuItem_Click);
      // 
      // orangeToolStripMenuItem
      // 
      this.orangeToolStripMenuItem.Name = "orangeToolStripMenuItem";
      this.orangeToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
      this.orangeToolStripMenuItem.Tag = "8";
      this.orangeToolStripMenuItem.Text = "8 - Orange";
      this.orangeToolStripMenuItem.Click += new System.EventHandler(this.replaceColorMenuItem_Click);
      // 
      // brownToolStripMenuItem
      // 
      this.brownToolStripMenuItem.Name = "brownToolStripMenuItem";
      this.brownToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
      this.brownToolStripMenuItem.Tag = "9";
      this.brownToolStripMenuItem.Text = "9 - Brown";
      this.brownToolStripMenuItem.Click += new System.EventHandler(this.replaceColorMenuItem_Click);
      // 
      // lightRedToolStripMenuItem
      // 
      this.lightRedToolStripMenuItem.Name = "lightRedToolStripMenuItem";
      this.lightRedToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
      this.lightRedToolStripMenuItem.Tag = "10";
      this.lightRedToolStripMenuItem.Text = "10 - Light Red";
      this.lightRedToolStripMenuItem.Click += new System.EventHandler(this.replaceColorMenuItem_Click);
      // 
      // darkGreyToolStripMenuItem
      // 
      this.darkGreyToolStripMenuItem.Name = "darkGreyToolStripMenuItem";
      this.darkGreyToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
      this.darkGreyToolStripMenuItem.Tag = "11";
      this.darkGreyToolStripMenuItem.Text = "11 - Dark Grey";
      this.darkGreyToolStripMenuItem.Click += new System.EventHandler(this.replaceColorMenuItem_Click);
      // 
      // mediumGreyToolStripMenuItem
      // 
      this.mediumGreyToolStripMenuItem.Name = "mediumGreyToolStripMenuItem";
      this.mediumGreyToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
      this.mediumGreyToolStripMenuItem.Tag = "12";
      this.mediumGreyToolStripMenuItem.Text = "12 - Medium Grey";
      this.mediumGreyToolStripMenuItem.Click += new System.EventHandler(this.replaceColorMenuItem_Click);
      // 
      // lightGreenToolStripMenuItem
      // 
      this.lightGreenToolStripMenuItem.Name = "lightGreenToolStripMenuItem";
      this.lightGreenToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
      this.lightGreenToolStripMenuItem.Tag = "13";
      this.lightGreenToolStripMenuItem.Text = "13 - Light Green";
      this.lightGreenToolStripMenuItem.Click += new System.EventHandler(this.replaceColorMenuItem_Click);
      // 
      // lightBlueToolStripMenuItem
      // 
      this.lightBlueToolStripMenuItem.Name = "lightBlueToolStripMenuItem";
      this.lightBlueToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
      this.lightBlueToolStripMenuItem.Tag = "14";
      this.lightBlueToolStripMenuItem.Text = "14 - Light Blue";
      this.lightBlueToolStripMenuItem.Click += new System.EventHandler(this.replaceColorMenuItem_Click);
      // 
      // lightGreyToolStripMenuItem
      // 
      this.lightGreyToolStripMenuItem.Name = "lightGreyToolStripMenuItem";
      this.lightGreyToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
      this.lightGreyToolStripMenuItem.Tag = "15";
      this.lightGreyToolStripMenuItem.Text = "15 - Light Grey";
      this.lightGreyToolStripMenuItem.Click += new System.EventHandler(this.replaceColorMenuItem_Click);
      // 
      // picPreview
      // 
      this.picPreview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.picPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.picPreview.DisplayPage = fastImage1;
      this.picPreview.Image = null;
      this.picPreview.Location = new System.Drawing.Point(0, 439);
      this.picPreview.Name = "picPreview";
      this.picPreview.Size = new System.Drawing.Size(644, 404);
      this.picPreview.TabIndex = 1;
      this.picPreview.TabStop = false;
      this.picPreview.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picPreview_MouseDown);
      this.picPreview.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picPreview_MouseMove);
      this.picPreview.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picPreview_MouseUp);
      // 
      // picOriginal
      // 
      this.picOriginal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.picOriginal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.picOriginal.ContextMenuStrip = this.contextMenuOrigPic;
      this.picOriginal.DisplayPage = fastImage2;
      this.picOriginal.Image = null;
      this.picOriginal.Location = new System.Drawing.Point(0, 29);
      this.picOriginal.Name = "picOriginal";
      this.picOriginal.Size = new System.Drawing.Size(644, 404);
      this.picOriginal.TabIndex = 1;
      this.picOriginal.TabStop = false;
      this.picOriginal.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picOriginal_MouseDown);
      this.picOriginal.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picOriginal_MouseMove);
      this.picOriginal.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picOriginal_MouseUp);
      // 
      // DlgGraphicImport
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(939, 846);
      this.Controls.Add(this.btnReload);
      this.Controls.Add(this.btnZoomOut);
      this.Controls.Add(this.btnZoomIn);
      this.Controls.Add(this.listProblems);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.tabImportSettings);
      this.Controls.Add(this.picPreview);
      this.Controls.Add(this.picOriginal);
      this.Controls.Add(this.menuImport);
      this.MainMenuStrip = this.menuImport;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "DlgGraphicImport";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Import Graphic";
      this.ResizeEnd += new System.EventHandler(this.DlgGraphicImport_ResizeEnd);
      this.SizeChanged += new System.EventHandler(this.DlgGraphicImport_SizeChanged);
      this.tabImportSettings.ResumeLayout(false);
      this.tabSettings.ResumeLayout(false);
      this.groupBox5.ResumeLayout(false);
      this.groupBox5.PerformLayout();
      this.groupBox4.ResumeLayout(false);
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.menuImport.ResumeLayout(false);
      this.menuImport.PerformLayout();
      this.contextMenuOrigPic.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picOriginal)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private GR.Forms.FastPictureBox picOriginal;
    private System.Windows.Forms.TabControl tabImportSettings;
    private System.Windows.Forms.TabPage tabSettings;
    private System.Windows.Forms.TabPage tabPalette;
    private GR.Forms.FastPictureBox picPreview;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.ListView listProblems;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.ComboBox comboBackground;
    private System.Windows.Forms.Label radioMulticolor2;
    private System.Windows.Forms.ComboBox comboMulticolor1;
    private System.Windows.Forms.Label radioMultiColor1;
    private System.Windows.Forms.ComboBox comboMulticolor2;
    private System.Windows.Forms.Label radioBackground;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.ComboBox comboImportType;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.MenuStrip menuImport;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.ComboBox comboColorMatching;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button btnZoomIn;
    private System.Windows.Forms.Button btnZoomOut;
    private System.Windows.Forms.Button btnReload;
    private System.Windows.Forms.GroupBox groupBox4;
    private System.Windows.Forms.ListView listDirectReplaceColors;
    private System.Windows.Forms.ContextMenuStrip contextMenuOrigPic;
    private System.Windows.Forms.ToolStripMenuItem forceTargetColorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem blackToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem whiteToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem redToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem cyanToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem purpleToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem greenToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem blueToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem yellowToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem orangeToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem brownToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem lightRedToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem darkGreyToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem mediumGreyToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem lightGreenToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem lightBlueToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem lightGreyToolStripMenuItem;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.ColumnHeader columnHeader4;
    private System.Windows.Forms.GroupBox groupBox5;
    private System.Windows.Forms.CheckBox checkPasteAsBlock;
    private System.Windows.Forms.ToolTip toolTip1;
  }
}