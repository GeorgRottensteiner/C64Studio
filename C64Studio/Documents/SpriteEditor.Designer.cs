namespace C64Studio
{
  partial class SpriteEditor
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
      GR.Image.FastImage fastImage4 = new GR.Image.FastImage();
      GR.Image.FastImage fastImage3 = new GR.Image.FastImage();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpriteEditor));
      this.tabSpriteEditor = new System.Windows.Forms.TabControl();
      this.tabEditor = new System.Windows.Forms.TabPage();
      this.btnEditPalette = new System.Windows.Forms.Button();
      this.comboSpriteProjectMode = new System.Windows.Forms.ComboBox();
      this.label11 = new System.Windows.Forms.Label();
      this.btnExchangeColors = new C64Studio.Controls.MenuButton();
      this.contextMenuExchangeColors = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.exchangeMultiColor1WithBGColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exchangeMultiColor2WithBGColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.forSelectedSpritesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exchangeMulticolor1WithSpriteColorToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this.exchangeMulticolor2WithSpriteColorToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this.exchangeBGColorWithSpriteColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exchangeMulticolor1WithBGColorSelectedSpritesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exchangeMulticolor2WithBGColorSelectedSpritesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exchangeMulticolor1WithMulticolor2ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this.tabSpriteDetails = new System.Windows.Forms.TabControl();
      this.tabPage1 = new System.Windows.Forms.TabPage();
      this.panelSprites = new GR.Forms.ImageListbox();
      this.tabPage2 = new System.Windows.Forms.TabPage();
      this.btnSavePreviewToGIF = new System.Windows.Forms.Button();
      this.checkAutoplayAnim = new System.Windows.Forms.CheckBox();
      this.label9 = new System.Windows.Forms.Label();
      this.checkExpandY = new System.Windows.Forms.CheckBox();
      this.checkExpandX = new System.Windows.Forms.CheckBox();
      this.listLayerSprites = new C64Studio.ArrangedItemList();
      this.listLayers = new C64Studio.ArrangedItemList();
      this.editLayerY = new System.Windows.Forms.TextBox();
      this.label7 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label10 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.editLayerDelay = new System.Windows.Forms.TextBox();
      this.editLayerName = new System.Windows.Forms.TextBox();
      this.editLayerX = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.comboLayerBGColor = new System.Windows.Forms.ComboBox();
      this.comboLayerColor = new System.Windows.Forms.ComboBox();
      this.comboSprite = new System.Windows.Forms.ComboBox();
      this.layerPreview = new GR.Forms.FastPictureBox();
      this.btnDeleteSprite = new System.Windows.Forms.Button();
      this.btnInvert = new System.Windows.Forms.Button();
      this.btnMirrorY = new System.Windows.Forms.Button();
      this.btnMirrorX = new System.Windows.Forms.Button();
      this.btnShiftDown = new System.Windows.Forms.Button();
      this.btnShiftUp = new System.Windows.Forms.Button();
      this.btnShiftRight = new System.Windows.Forms.Button();
      this.btnRotateRight = new System.Windows.Forms.Button();
      this.btnRotateLeft = new System.Windows.Forms.Button();
      this.btnShiftLeft = new System.Windows.Forms.Button();
      this.btnCopyToClipboard = new System.Windows.Forms.Button();
      this.btnPasteFromClipboard = new System.Windows.Forms.Button();
      this.labelCharNo = new System.Windows.Forms.Label();
      this.checkShowGrid = new System.Windows.Forms.CheckBox();
      this.checkMulticolor = new System.Windows.Forms.CheckBox();
      this.radioSpriteColor = new System.Windows.Forms.RadioButton();
      this.radioMulticolor2 = new System.Windows.Forms.RadioButton();
      this.radioMultiColor1 = new System.Windows.Forms.RadioButton();
      this.radioBackground = new System.Windows.Forms.RadioButton();
      this.comboSpriteColor = new System.Windows.Forms.ComboBox();
      this.comboMulticolor2 = new System.Windows.Forms.ComboBox();
      this.comboMulticolor1 = new System.Windows.Forms.ComboBox();
      this.comboBackground = new System.Windows.Forms.ComboBox();
      this.pictureEditor = new GR.Forms.FastPictureBox();
      this.tabProject = new System.Windows.Forms.TabPage();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.btnImportFromFile = new System.Windows.Forms.Button();
      this.btnClearImport = new System.Windows.Forms.Button();
      this.btnImportFromASM = new System.Windows.Forms.Button();
      this.btnImportFromBASICHex = new System.Windows.Forms.Button();
      this.btnImportFromBASIC = new System.Windows.Forms.Button();
      this.btnImportFromHex = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.editDataImport = new System.Windows.Forms.TextBox();
      this.groupExport = new System.Windows.Forms.GroupBox();
      this.editExportBASICLineOffset = new System.Windows.Forms.TextBox();
      this.comboExportRange = new System.Windows.Forms.ComboBox();
      this.editExportBASICLineNo = new System.Windows.Forms.TextBox();
      this.editSpriteCount = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.editSpriteFrom = new System.Windows.Forms.TextBox();
      this.label8 = new System.Windows.Forms.Label();
      this.labelCharactersTo = new System.Windows.Forms.Label();
      this.btnToBASICHex = new System.Windows.Forms.Button();
      this.btnExportToBASICData = new System.Windows.Forms.Button();
      this.labelCharactersFrom = new System.Windows.Forms.Label();
      this.editPrefix = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.editWrapByteCount = new System.Windows.Forms.TextBox();
      this.checkExportToDataWrap = new System.Windows.Forms.CheckBox();
      this.checkExportToDataIncludeRes = new System.Windows.Forms.CheckBox();
      this.editDataExport = new System.Windows.Forms.TextBox();
      this.button1 = new System.Windows.Forms.Button();
      this.btnExportToData = new System.Windows.Forms.Button();
      this.btnExportCharset = new System.Windows.Forms.Button();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.openCharsetProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveSpriteProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.closeCharsetProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.btnToolFill = new System.Windows.Forms.RadioButton();
      this.btnToolEdit = new System.Windows.Forms.RadioButton();
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      this.tabSpriteEditor.SuspendLayout();
      this.tabEditor.SuspendLayout();
      this.contextMenuExchangeColors.SuspendLayout();
      this.tabSpriteDetails.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.tabPage2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.layerPreview)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureEditor)).BeginInit();
      this.tabProject.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.groupExport.SuspendLayout();
      this.menuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabSpriteEditor
      // 
      this.tabSpriteEditor.Controls.Add(this.tabEditor);
      this.tabSpriteEditor.Controls.Add(this.tabProject);
      this.tabSpriteEditor.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabSpriteEditor.Location = new System.Drawing.Point(0, 24);
      this.tabSpriteEditor.Name = "tabSpriteEditor";
      this.tabSpriteEditor.SelectedIndex = 0;
      this.tabSpriteEditor.Size = new System.Drawing.Size(782, 499);
      this.tabSpriteEditor.TabIndex = 0;
      // 
      // tabEditor
      // 
      this.tabEditor.Controls.Add(this.btnToolEdit);
      this.tabEditor.Controls.Add(this.btnToolFill);
      this.tabEditor.Controls.Add(this.btnEditPalette);
      this.tabEditor.Controls.Add(this.comboSpriteProjectMode);
      this.tabEditor.Controls.Add(this.label11);
      this.tabEditor.Controls.Add(this.btnExchangeColors);
      this.tabEditor.Controls.Add(this.tabSpriteDetails);
      this.tabEditor.Controls.Add(this.btnDeleteSprite);
      this.tabEditor.Controls.Add(this.btnInvert);
      this.tabEditor.Controls.Add(this.btnMirrorY);
      this.tabEditor.Controls.Add(this.btnMirrorX);
      this.tabEditor.Controls.Add(this.btnShiftDown);
      this.tabEditor.Controls.Add(this.btnShiftUp);
      this.tabEditor.Controls.Add(this.btnShiftRight);
      this.tabEditor.Controls.Add(this.btnRotateRight);
      this.tabEditor.Controls.Add(this.btnRotateLeft);
      this.tabEditor.Controls.Add(this.btnShiftLeft);
      this.tabEditor.Controls.Add(this.btnCopyToClipboard);
      this.tabEditor.Controls.Add(this.btnPasteFromClipboard);
      this.tabEditor.Controls.Add(this.labelCharNo);
      this.tabEditor.Controls.Add(this.checkShowGrid);
      this.tabEditor.Controls.Add(this.checkMulticolor);
      this.tabEditor.Controls.Add(this.radioSpriteColor);
      this.tabEditor.Controls.Add(this.radioMulticolor2);
      this.tabEditor.Controls.Add(this.radioMultiColor1);
      this.tabEditor.Controls.Add(this.radioBackground);
      this.tabEditor.Controls.Add(this.comboSpriteColor);
      this.tabEditor.Controls.Add(this.comboMulticolor2);
      this.tabEditor.Controls.Add(this.comboMulticolor1);
      this.tabEditor.Controls.Add(this.comboBackground);
      this.tabEditor.Controls.Add(this.pictureEditor);
      this.tabEditor.Location = new System.Drawing.Point(4, 22);
      this.tabEditor.Name = "tabEditor";
      this.tabEditor.Padding = new System.Windows.Forms.Padding(3);
      this.tabEditor.Size = new System.Drawing.Size(774, 473);
      this.tabEditor.TabIndex = 0;
      this.tabEditor.Text = "Sprite";
      this.tabEditor.UseVisualStyleBackColor = true;
      // 
      // btnEditPalette
      // 
      this.btnEditPalette.Enabled = false;
      this.btnEditPalette.Location = new System.Drawing.Point(13, 408);
      this.btnEditPalette.Name = "btnEditPalette";
      this.btnEditPalette.Size = new System.Drawing.Size(79, 26);
      this.btnEditPalette.TabIndex = 56;
      this.btnEditPalette.Text = "Edit Palette";
      this.btnEditPalette.UseVisualStyleBackColor = true;
      this.btnEditPalette.Click += new System.EventHandler(this.btnEditPalette_Click);
      // 
      // comboSpriteProjectMode
      // 
      this.comboSpriteProjectMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboSpriteProjectMode.FormattingEnabled = true;
      this.comboSpriteProjectMode.Location = new System.Drawing.Point(60, 442);
      this.comboSpriteProjectMode.Name = "comboSpriteProjectMode";
      this.comboSpriteProjectMode.Size = new System.Drawing.Size(288, 21);
      this.comboSpriteProjectMode.TabIndex = 17;
      this.comboSpriteProjectMode.SelectedIndexChanged += new System.EventHandler(this.comboSpriteProjectMode_SelectedIndexChanged);
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Location = new System.Drawing.Point(10, 445);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(37, 13);
      this.label11.TabIndex = 16;
      this.label11.Text = "Mode:";
      // 
      // btnExchangeColors
      // 
      this.btnExchangeColors.Location = new System.Drawing.Point(227, 381);
      this.btnExchangeColors.Menu = this.contextMenuExchangeColors;
      this.btnExchangeColors.Name = "btnExchangeColors";
      this.btnExchangeColors.Size = new System.Drawing.Size(121, 26);
      this.btnExchangeColors.TabIndex = 15;
      this.btnExchangeColors.Text = "Exchange Colors";
      this.btnExchangeColors.UseVisualStyleBackColor = true;
      // 
      // contextMenuExchangeColors
      // 
      this.contextMenuExchangeColors.ImageScalingSize = new System.Drawing.Size(28, 28);
      this.contextMenuExchangeColors.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exchangeMultiColor1WithBGColorToolStripMenuItem,
            this.exchangeMultiColor2WithBGColorToolStripMenuItem,
            this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem,
            this.forSelectedSpritesToolStripMenuItem});
      this.contextMenuExchangeColors.Name = "contextMenuExchangeColors";
      this.contextMenuExchangeColors.Size = new System.Drawing.Size(296, 92);
      // 
      // exchangeMultiColor1WithBGColorToolStripMenuItem
      // 
      this.exchangeMultiColor1WithBGColorToolStripMenuItem.Name = "exchangeMultiColor1WithBGColorToolStripMenuItem";
      this.exchangeMultiColor1WithBGColorToolStripMenuItem.Size = new System.Drawing.Size(295, 22);
      this.exchangeMultiColor1WithBGColorToolStripMenuItem.Text = "Exchange Multi Color 1 with BG Color";
      this.exchangeMultiColor1WithBGColorToolStripMenuItem.Click += new System.EventHandler(this.exchangeMultiColor1WithBGColorToolStripMenuItem_Click);
      // 
      // exchangeMultiColor2WithBGColorToolStripMenuItem
      // 
      this.exchangeMultiColor2WithBGColorToolStripMenuItem.Name = "exchangeMultiColor2WithBGColorToolStripMenuItem";
      this.exchangeMultiColor2WithBGColorToolStripMenuItem.Size = new System.Drawing.Size(295, 22);
      this.exchangeMultiColor2WithBGColorToolStripMenuItem.Text = "Exchange Multi Color 2 with BG Color";
      this.exchangeMultiColor2WithBGColorToolStripMenuItem.Click += new System.EventHandler(this.exchangeMultiColor2WithBGColorToolStripMenuItem_Click);
      // 
      // exchangeMultiColor1WithMultiColor2ToolStripMenuItem
      // 
      this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem.Name = "exchangeMultiColor1WithMultiColor2ToolStripMenuItem";
      this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem.Size = new System.Drawing.Size(295, 22);
      this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem.Text = "Exchange Multi Color 1 with Multi Color 2";
      this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem.Click += new System.EventHandler(this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem_Click);
      // 
      // forSelectedSpritesToolStripMenuItem
      // 
      this.forSelectedSpritesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exchangeMulticolor1WithSpriteColorToolStripMenuItem1,
            this.exchangeMulticolor2WithSpriteColorToolStripMenuItem1,
            this.exchangeBGColorWithSpriteColorToolStripMenuItem,
            this.exchangeMulticolor1WithBGColorSelectedSpritesToolStripMenuItem,
            this.exchangeMulticolor2WithBGColorSelectedSpritesToolStripMenuItem,
            this.exchangeMulticolor1WithMulticolor2ToolStripMenuItem1});
      this.forSelectedSpritesToolStripMenuItem.Name = "forSelectedSpritesToolStripMenuItem";
      this.forSelectedSpritesToolStripMenuItem.Size = new System.Drawing.Size(295, 22);
      this.forSelectedSpritesToolStripMenuItem.Text = "For selected Sprites";
      // 
      // exchangeMulticolor1WithSpriteColorToolStripMenuItem1
      // 
      this.exchangeMulticolor1WithSpriteColorToolStripMenuItem1.Name = "exchangeMulticolor1WithSpriteColorToolStripMenuItem1";
      this.exchangeMulticolor1WithSpriteColorToolStripMenuItem1.Size = new System.Drawing.Size(285, 22);
      this.exchangeMulticolor1WithSpriteColorToolStripMenuItem1.Text = "Exchange Multicolor 1 with sprite color";
      this.exchangeMulticolor1WithSpriteColorToolStripMenuItem1.Click += new System.EventHandler(this.exchangeMulticolor1WithSpriteColorToolStripMenuItem1_Click);
      // 
      // exchangeMulticolor2WithSpriteColorToolStripMenuItem1
      // 
      this.exchangeMulticolor2WithSpriteColorToolStripMenuItem1.Name = "exchangeMulticolor2WithSpriteColorToolStripMenuItem1";
      this.exchangeMulticolor2WithSpriteColorToolStripMenuItem1.Size = new System.Drawing.Size(285, 22);
      this.exchangeMulticolor2WithSpriteColorToolStripMenuItem1.Text = "Exchange Multicolor 2 with sprite color";
      this.exchangeMulticolor2WithSpriteColorToolStripMenuItem1.Click += new System.EventHandler(this.exchangeMulticolor2WithSpriteColorToolStripMenuItem1_Click);
      // 
      // exchangeBGColorWithSpriteColorToolStripMenuItem
      // 
      this.exchangeBGColorWithSpriteColorToolStripMenuItem.Name = "exchangeBGColorWithSpriteColorToolStripMenuItem";
      this.exchangeBGColorWithSpriteColorToolStripMenuItem.Size = new System.Drawing.Size(285, 22);
      this.exchangeBGColorWithSpriteColorToolStripMenuItem.Text = "Exchange BG color with sprite color";
      this.exchangeBGColorWithSpriteColorToolStripMenuItem.Click += new System.EventHandler(this.exchangeBGColorWithSpriteColorToolStripMenuItem_Click);
      // 
      // exchangeMulticolor1WithBGColorSelectedSpritesToolStripMenuItem
      // 
      this.exchangeMulticolor1WithBGColorSelectedSpritesToolStripMenuItem.Name = "exchangeMulticolor1WithBGColorSelectedSpritesToolStripMenuItem";
      this.exchangeMulticolor1WithBGColorSelectedSpritesToolStripMenuItem.Size = new System.Drawing.Size(285, 22);
      this.exchangeMulticolor1WithBGColorSelectedSpritesToolStripMenuItem.Text = "Exchange Multicolor 1 with BG color";
      this.exchangeMulticolor1WithBGColorSelectedSpritesToolStripMenuItem.Click += new System.EventHandler(this.exchangeMulticolor1WithBGColorSelectedSpritesToolStripMenuItem_Click);
      // 
      // exchangeMulticolor2WithBGColorSelectedSpritesToolStripMenuItem
      // 
      this.exchangeMulticolor2WithBGColorSelectedSpritesToolStripMenuItem.Name = "exchangeMulticolor2WithBGColorSelectedSpritesToolStripMenuItem";
      this.exchangeMulticolor2WithBGColorSelectedSpritesToolStripMenuItem.Size = new System.Drawing.Size(285, 22);
      this.exchangeMulticolor2WithBGColorSelectedSpritesToolStripMenuItem.Text = "Exchange Multicolor 2 with BG color";
      this.exchangeMulticolor2WithBGColorSelectedSpritesToolStripMenuItem.Click += new System.EventHandler(this.exchangeMulticolor2WithBGColorSelectedSpritesToolStripMenuItem_Click);
      // 
      // exchangeMulticolor1WithMulticolor2ToolStripMenuItem1
      // 
      this.exchangeMulticolor1WithMulticolor2ToolStripMenuItem1.Name = "exchangeMulticolor1WithMulticolor2ToolStripMenuItem1";
      this.exchangeMulticolor1WithMulticolor2ToolStripMenuItem1.Size = new System.Drawing.Size(285, 22);
      this.exchangeMulticolor1WithMulticolor2ToolStripMenuItem1.Text = "Exchange Multicolor 1 with Multicolor 2";
      this.exchangeMulticolor1WithMulticolor2ToolStripMenuItem1.Click += new System.EventHandler(this.exchangeMulticolor1WithMulticolor2ToolStripMenuItem1_Click);
      // 
      // tabSpriteDetails
      // 
      this.tabSpriteDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tabSpriteDetails.Controls.Add(this.tabPage1);
      this.tabSpriteDetails.Controls.Add(this.tabPage2);
      this.tabSpriteDetails.Location = new System.Drawing.Point(446, 2);
      this.tabSpriteDetails.Name = "tabSpriteDetails";
      this.tabSpriteDetails.SelectedIndex = 0;
      this.tabSpriteDetails.Size = new System.Drawing.Size(328, 463);
      this.tabSpriteDetails.TabIndex = 9;
      // 
      // tabPage1
      // 
      this.tabPage1.Controls.Add(this.panelSprites);
      this.tabPage1.Location = new System.Drawing.Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage1.Size = new System.Drawing.Size(320, 437);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "Sprites";
      this.tabPage1.UseVisualStyleBackColor = true;
      // 
      // panelSprites
      // 
      this.panelSprites.AutoScroll = true;
      this.panelSprites.AutoScrollHorizontalMaximum = 100;
      this.panelSprites.AutoScrollHorizontalMinimum = 0;
      this.panelSprites.AutoScrollHPos = 0;
      this.panelSprites.AutoScrollVerticalMaximum = -23;
      this.panelSprites.AutoScrollVerticalMinimum = 0;
      this.panelSprites.AutoScrollVPos = 0;
      this.panelSprites.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panelSprites.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelSprites.EnableAutoScrollHorizontal = true;
      this.panelSprites.EnableAutoScrollVertical = true;
      this.panelSprites.HottrackColor = ((uint)(2151694591u));
      this.panelSprites.ItemHeight = 21;
      this.panelSprites.ItemWidth = 24;
      this.panelSprites.Location = new System.Drawing.Point(3, 3);
      this.panelSprites.Name = "panelSprites";
      this.panelSprites.PixelFormat = System.Drawing.Imaging.PixelFormat.DontCare;
      this.panelSprites.SelectedIndex = -1;
      this.panelSprites.Size = new System.Drawing.Size(314, 431);
      this.panelSprites.TabIndex = 4;
      this.panelSprites.TabStop = true;
      this.panelSprites.VisibleAutoScrollHorizontal = false;
      this.panelSprites.VisibleAutoScrollVertical = false;
      this.panelSprites.SelectedIndexChanged += new System.EventHandler(this.panelSprites_SelectedIndexChanged);
      this.panelSprites.Resize += new System.EventHandler(this.panelSprites_Resize);
      // 
      // tabPage2
      // 
      this.tabPage2.Controls.Add(this.btnSavePreviewToGIF);
      this.tabPage2.Controls.Add(this.checkAutoplayAnim);
      this.tabPage2.Controls.Add(this.label9);
      this.tabPage2.Controls.Add(this.checkExpandY);
      this.tabPage2.Controls.Add(this.checkExpandX);
      this.tabPage2.Controls.Add(this.listLayerSprites);
      this.tabPage2.Controls.Add(this.listLayers);
      this.tabPage2.Controls.Add(this.editLayerY);
      this.tabPage2.Controls.Add(this.label7);
      this.tabPage2.Controls.Add(this.label4);
      this.tabPage2.Controls.Add(this.label10);
      this.tabPage2.Controls.Add(this.label6);
      this.tabPage2.Controls.Add(this.editLayerDelay);
      this.tabPage2.Controls.Add(this.editLayerName);
      this.tabPage2.Controls.Add(this.editLayerX);
      this.tabPage2.Controls.Add(this.label5);
      this.tabPage2.Controls.Add(this.label3);
      this.tabPage2.Controls.Add(this.comboLayerBGColor);
      this.tabPage2.Controls.Add(this.comboLayerColor);
      this.tabPage2.Controls.Add(this.comboSprite);
      this.tabPage2.Controls.Add(this.layerPreview);
      this.tabPage2.Location = new System.Drawing.Point(4, 22);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage2.Size = new System.Drawing.Size(320, 437);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "Preview";
      this.tabPage2.UseVisualStyleBackColor = true;
      // 
      // btnSavePreviewToGIF
      // 
      this.btnSavePreviewToGIF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnSavePreviewToGIF.Location = new System.Drawing.Point(389, 338);
      this.btnSavePreviewToGIF.Name = "btnSavePreviewToGIF";
      this.btnSavePreviewToGIF.Size = new System.Drawing.Size(75, 23);
      this.btnSavePreviewToGIF.TabIndex = 13;
      this.btnSavePreviewToGIF.Text = "Save as GIF";
      this.btnSavePreviewToGIF.UseVisualStyleBackColor = true;
      this.btnSavePreviewToGIF.Click += new System.EventHandler(this.btnSavePreviewToGIF_Click);
      // 
      // checkAutoplayAnim
      // 
      this.checkAutoplayAnim.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.checkAutoplayAnim.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkAutoplayAnim.Location = new System.Drawing.Point(227, 395);
      this.checkAutoplayAnim.Name = "checkAutoplayAnim";
      this.checkAutoplayAnim.Size = new System.Drawing.Size(132, 24);
      this.checkAutoplayAnim.TabIndex = 12;
      this.checkAutoplayAnim.Text = "Auto-Animation";
      this.checkAutoplayAnim.UseVisualStyleBackColor = true;
      this.checkAutoplayAnim.CheckedChanged += new System.EventHandler(this.checkAutoplayAnim_CheckedChanged);
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(3, 274);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(44, 13);
      this.label9.TabIndex = 24;
      this.label9.Text = "Frames:";
      // 
      // checkExpandY
      // 
      this.checkExpandY.AutoSize = true;
      this.checkExpandY.Location = new System.Drawing.Point(149, 180);
      this.checkExpandY.Name = "checkExpandY";
      this.checkExpandY.Size = new System.Drawing.Size(43, 17);
      this.checkExpandY.TabIndex = 3;
      this.checkExpandY.Text = "Y*2";
      this.checkExpandY.UseVisualStyleBackColor = true;
      this.checkExpandY.CheckedChanged += new System.EventHandler(this.checkExpandY_CheckedChanged);
      // 
      // checkExpandX
      // 
      this.checkExpandX.AutoSize = true;
      this.checkExpandX.Location = new System.Drawing.Point(149, 156);
      this.checkExpandX.Name = "checkExpandX";
      this.checkExpandX.Size = new System.Drawing.Size(43, 17);
      this.checkExpandX.TabIndex = 2;
      this.checkExpandX.Text = "X*2";
      this.checkExpandX.UseVisualStyleBackColor = true;
      this.checkExpandX.CheckedChanged += new System.EventHandler(this.checkExpandX_CheckedChanged);
      // 
      // listLayerSprites
      // 
      this.listLayerSprites.AddButtonEnabled = true;
      this.listLayerSprites.AllowClone = true;
      this.listLayerSprites.DeleteButtonEnabled = false;
      this.listLayerSprites.HasOwnerDrawColumn = true;
      this.listLayerSprites.HighlightColor = System.Drawing.SystemColors.HotTrack;
      this.listLayerSprites.HighlightTextColor = System.Drawing.SystemColors.HighlightText;
      this.listLayerSprites.Location = new System.Drawing.Point(0, 0);
      this.listLayerSprites.MoveDownButtonEnabled = false;
      this.listLayerSprites.MoveUpButtonEnabled = false;
      this.listLayerSprites.MustHaveOneElement = false;
      this.listLayerSprites.Name = "listLayerSprites";
      this.listLayerSprites.SelectedIndex = -1;
      this.listLayerSprites.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      this.listLayerSprites.SelectionTextColor = System.Drawing.SystemColors.HighlightText;
      this.listLayerSprites.Size = new System.Drawing.Size(192, 148);
      this.listLayerSprites.TabIndex = 0;
      this.listLayerSprites.AddingItem += new C64Studio.ArrangedItemList.AddingItemEventHandler(this.listLayerSprites_AddingItem);
      this.listLayerSprites.CloningItem += new C64Studio.ArrangedItemList.CloningItemEventHandler(this.listLayerSprites_CloningItem);
      this.listLayerSprites.ItemAdded += new C64Studio.ArrangedItemList.ItemModifiedEventHandler(this.listLayerSprites_ItemAdded);
      this.listLayerSprites.ItemRemoved += new C64Studio.ArrangedItemList.ItemModifiedEventHandler(this.listLayerSprites_ItemRemoved);
      this.listLayerSprites.MovingItem += new C64Studio.ArrangedItemList.ItemExchangingEventHandler(this.listLayerSprites_MovingItem);
      this.listLayerSprites.ItemMoved += new C64Studio.ArrangedItemList.ItemExchangedEventHandler(this.listLayerSprites_ItemMoved);
      this.listLayerSprites.SelectedIndexChanged += new C64Studio.ArrangedItemList.ItemModifiedEventHandler(this.listLayerSprites_SelectedIndexChanged);
      // 
      // listLayers
      // 
      this.listLayers.AddButtonEnabled = true;
      this.listLayers.AllowClone = true;
      this.listLayers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.listLayers.DeleteButtonEnabled = false;
      this.listLayers.HasOwnerDrawColumn = true;
      this.listLayers.HighlightColor = System.Drawing.SystemColors.HotTrack;
      this.listLayers.HighlightTextColor = System.Drawing.SystemColors.HighlightText;
      this.listLayers.Location = new System.Drawing.Point(0, 290);
      this.listLayers.MoveDownButtonEnabled = false;
      this.listLayers.MoveUpButtonEnabled = false;
      this.listLayers.MustHaveOneElement = true;
      this.listLayers.Name = "listLayers";
      this.listLayers.SelectedIndex = -1;
      this.listLayers.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      this.listLayers.SelectionTextColor = System.Drawing.SystemColors.HighlightText;
      this.listLayers.Size = new System.Drawing.Size(192, 141);
      this.listLayers.TabIndex = 9;
      this.listLayers.AddingItem += new C64Studio.ArrangedItemList.AddingItemEventHandler(this.listLayers_AddingItem);
      this.listLayers.CloningItem += new C64Studio.ArrangedItemList.CloningItemEventHandler(this.listLayers_CloningItem);
      this.listLayers.ItemAdded += new C64Studio.ArrangedItemList.ItemModifiedEventHandler(this.listLayers_ItemAdded);
      this.listLayers.ItemRemoved += new C64Studio.ArrangedItemList.ItemModifiedEventHandler(this.listLayers_ItemRemoved);
      this.listLayers.MovingItem += new C64Studio.ArrangedItemList.ItemExchangingEventHandler(this.listLayers_MovingItem);
      this.listLayers.ItemMoved += new C64Studio.ArrangedItemList.ItemExchangedEventHandler(this.listLayers_ItemMoved);
      this.listLayers.SelectedIndexChanged += new C64Studio.ArrangedItemList.ItemModifiedEventHandler(this.listLayers_SelectedIndexChanged);
      // 
      // editLayerY
      // 
      this.editLayerY.Location = new System.Drawing.Point(91, 208);
      this.editLayerY.Name = "editLayerY";
      this.editLayerY.Size = new System.Drawing.Size(45, 20);
      this.editLayerY.TabIndex = 5;
      this.editLayerY.TextChanged += new System.EventHandler(this.editLayerY_TextChanged);
      this.editLayerY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editLayerY_KeyPress);
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(104, 239);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(25, 13);
      this.label7.TabIndex = 8;
      this.label7.Text = "BG:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(6, 239);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(25, 13);
      this.label4.TabIndex = 6;
      this.label4.Text = "Col:";
      // 
      // label10
      // 
      this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(224, 370);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(37, 13);
      this.label10.TabIndex = 15;
      this.label10.Text = "Delay:";
      // 
      // label6
      // 
      this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(224, 343);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(38, 13);
      this.label6.TabIndex = 15;
      this.label6.Text = "Name:";
      // 
      // editLayerDelay
      // 
      this.editLayerDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.editLayerDelay.Location = new System.Drawing.Point(268, 365);
      this.editLayerDelay.Name = "editLayerDelay";
      this.editLayerDelay.Size = new System.Drawing.Size(91, 20);
      this.editLayerDelay.TabIndex = 11;
      this.editLayerDelay.TextChanged += new System.EventHandler(this.editLayerDelay_TextChanged);
      // 
      // editLayerName
      // 
      this.editLayerName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.editLayerName.Location = new System.Drawing.Point(268, 338);
      this.editLayerName.Name = "editLayerName";
      this.editLayerName.Size = new System.Drawing.Size(91, 20);
      this.editLayerName.TabIndex = 10;
      this.editLayerName.TextChanged += new System.EventHandler(this.editLayerName_TextChanged);
      // 
      // editLayerX
      // 
      this.editLayerX.Location = new System.Drawing.Point(39, 208);
      this.editLayerX.Name = "editLayerX";
      this.editLayerX.Size = new System.Drawing.Size(45, 20);
      this.editLayerX.TabIndex = 4;
      this.editLayerX.TextChanged += new System.EventHandler(this.editLayerX_TextChanged);
      this.editLayerX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editLayerX_KeyPress);
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(6, 170);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(21, 13);
      this.label5.TabIndex = 11;
      this.label5.Text = "Nr:";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(6, 211);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(28, 13);
      this.label3.TabIndex = 12;
      this.label3.Text = "Pos:";
      // 
      // comboLayerBGColor
      // 
      this.comboLayerBGColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboLayerBGColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboLayerBGColor.FormattingEnabled = true;
      this.comboLayerBGColor.Location = new System.Drawing.Point(135, 236);
      this.comboLayerBGColor.Name = "comboLayerBGColor";
      this.comboLayerBGColor.Size = new System.Drawing.Size(57, 21);
      this.comboLayerBGColor.TabIndex = 8;
      this.comboLayerBGColor.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboColor_DrawItem);
      this.comboLayerBGColor.SelectedIndexChanged += new System.EventHandler(this.comboLayerBGColor_SelectedIndexChanged);
      // 
      // comboLayerColor
      // 
      this.comboLayerColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboLayerColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboLayerColor.FormattingEnabled = true;
      this.comboLayerColor.Location = new System.Drawing.Point(39, 236);
      this.comboLayerColor.Name = "comboLayerColor";
      this.comboLayerColor.Size = new System.Drawing.Size(59, 21);
      this.comboLayerColor.TabIndex = 7;
      this.comboLayerColor.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboColor_DrawItem);
      this.comboLayerColor.SelectedIndexChanged += new System.EventHandler(this.comboLayerColor_SelectedIndexChanged);
      // 
      // comboSprite
      // 
      this.comboSprite.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboSprite.DropDownHeight = 320;
      this.comboSprite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboSprite.FormattingEnabled = true;
      this.comboSprite.IntegralHeight = false;
      this.comboSprite.ItemHeight = 42;
      this.comboSprite.Location = new System.Drawing.Point(39, 154);
      this.comboSprite.Name = "comboSprite";
      this.comboSprite.Size = new System.Drawing.Size(97, 48);
      this.comboSprite.TabIndex = 1;
      this.comboSprite.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboSprite_DrawItem);
      this.comboSprite.SelectedIndexChanged += new System.EventHandler(this.comboSprite_SelectedIndexChanged);
      // 
      // layerPreview
      // 
      this.layerPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.layerPreview.AutoResize = false;
      this.layerPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.layerPreview.DisplayPage = fastImage4;
      this.layerPreview.Image = null;
      this.layerPreview.Location = new System.Drawing.Point(208, 3);
      this.layerPreview.Name = "layerPreview";
      this.layerPreview.Size = new System.Drawing.Size(106, 329);
      this.layerPreview.TabIndex = 7;
      this.layerPreview.TabStop = false;
      // 
      // btnDeleteSprite
      // 
      this.btnDeleteSprite.Enabled = false;
      this.btnDeleteSprite.Location = new System.Drawing.Point(365, 382);
      this.btnDeleteSprite.Name = "btnDeleteSprite";
      this.btnDeleteSprite.Size = new System.Drawing.Size(75, 23);
      this.btnDeleteSprite.TabIndex = 8;
      this.btnDeleteSprite.Text = "Delete";
      this.btnDeleteSprite.UseVisualStyleBackColor = true;
      this.btnDeleteSprite.Click += new System.EventHandler(this.btnDeleteSprite_Click);
      // 
      // btnInvert
      // 
      this.btnInvert.Image = ((System.Drawing.Image)(resources.GetObject("btnInvert.Image")));
      this.btnInvert.Location = new System.Drawing.Point(8, 192);
      this.btnInvert.Name = "btnInvert";
      this.btnInvert.Size = new System.Drawing.Size(26, 26);
      this.btnInvert.TabIndex = 7;
      this.toolTip1.SetToolTip(this.btnInvert, "Invert selected sprites colors");
      this.btnInvert.UseVisualStyleBackColor = true;
      this.btnInvert.Click += new System.EventHandler(this.btnInvert_Click);
      // 
      // btnMirrorY
      // 
      this.btnMirrorY.Image = ((System.Drawing.Image)(resources.GetObject("btnMirrorY.Image")));
      this.btnMirrorY.Location = new System.Drawing.Point(8, 161);
      this.btnMirrorY.Name = "btnMirrorY";
      this.btnMirrorY.Size = new System.Drawing.Size(26, 26);
      this.btnMirrorY.TabIndex = 7;
      this.toolTip1.SetToolTip(this.btnMirrorY, "Mirror selected sprites vertically");
      this.btnMirrorY.UseVisualStyleBackColor = true;
      this.btnMirrorY.Click += new System.EventHandler(this.btnMirrorY_Click);
      // 
      // btnMirrorX
      // 
      this.btnMirrorX.Image = ((System.Drawing.Image)(resources.GetObject("btnMirrorX.Image")));
      this.btnMirrorX.Location = new System.Drawing.Point(8, 130);
      this.btnMirrorX.Name = "btnMirrorX";
      this.btnMirrorX.Size = new System.Drawing.Size(26, 26);
      this.btnMirrorX.TabIndex = 7;
      this.toolTip1.SetToolTip(this.btnMirrorX, "Mirror selected sprites horizontally");
      this.btnMirrorX.UseVisualStyleBackColor = true;
      this.btnMirrorX.Click += new System.EventHandler(this.btnMirrorX_Click);
      // 
      // btnShiftDown
      // 
      this.btnShiftDown.Image = ((System.Drawing.Image)(resources.GetObject("btnShiftDown.Image")));
      this.btnShiftDown.Location = new System.Drawing.Point(8, 99);
      this.btnShiftDown.Name = "btnShiftDown";
      this.btnShiftDown.Size = new System.Drawing.Size(26, 26);
      this.btnShiftDown.TabIndex = 7;
      this.toolTip1.SetToolTip(this.btnShiftDown, "Shift selected sprites down");
      this.btnShiftDown.UseVisualStyleBackColor = true;
      this.btnShiftDown.Click += new System.EventHandler(this.btnShiftDown_Click);
      // 
      // btnShiftUp
      // 
      this.btnShiftUp.Image = ((System.Drawing.Image)(resources.GetObject("btnShiftUp.Image")));
      this.btnShiftUp.Location = new System.Drawing.Point(8, 68);
      this.btnShiftUp.Name = "btnShiftUp";
      this.btnShiftUp.Size = new System.Drawing.Size(26, 26);
      this.btnShiftUp.TabIndex = 7;
      this.toolTip1.SetToolTip(this.btnShiftUp, "Shift selected sprites up");
      this.btnShiftUp.UseVisualStyleBackColor = true;
      this.btnShiftUp.Click += new System.EventHandler(this.btnShiftUp_Click);
      // 
      // btnShiftRight
      // 
      this.btnShiftRight.Image = ((System.Drawing.Image)(resources.GetObject("btnShiftRight.Image")));
      this.btnShiftRight.Location = new System.Drawing.Point(8, 37);
      this.btnShiftRight.Name = "btnShiftRight";
      this.btnShiftRight.Size = new System.Drawing.Size(26, 26);
      this.btnShiftRight.TabIndex = 7;
      this.toolTip1.SetToolTip(this.btnShiftRight, "Shift selected sprites right");
      this.btnShiftRight.UseVisualStyleBackColor = true;
      this.btnShiftRight.Click += new System.EventHandler(this.btnShiftRight_Click);
      // 
      // btnRotateRight
      // 
      this.btnRotateRight.Image = ((System.Drawing.Image)(resources.GetObject("btnRotateRight.Image")));
      this.btnRotateRight.Location = new System.Drawing.Point(8, 254);
      this.btnRotateRight.Name = "btnRotateRight";
      this.btnRotateRight.Size = new System.Drawing.Size(26, 26);
      this.btnRotateRight.TabIndex = 7;
      this.toolTip1.SetToolTip(this.btnRotateRight, "Rotate selected sprites right");
      this.btnRotateRight.UseVisualStyleBackColor = true;
      this.btnRotateRight.Click += new System.EventHandler(this.btnRotateRight_Click);
      // 
      // btnRotateLeft
      // 
      this.btnRotateLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnRotateLeft.Image")));
      this.btnRotateLeft.Location = new System.Drawing.Point(8, 223);
      this.btnRotateLeft.Name = "btnRotateLeft";
      this.btnRotateLeft.Size = new System.Drawing.Size(26, 26);
      this.btnRotateLeft.TabIndex = 7;
      this.toolTip1.SetToolTip(this.btnRotateLeft, "Rotate selected sprites left");
      this.btnRotateLeft.UseVisualStyleBackColor = true;
      this.btnRotateLeft.Click += new System.EventHandler(this.btnRotateLeft_Click);
      // 
      // btnShiftLeft
      // 
      this.btnShiftLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnShiftLeft.Image")));
      this.btnShiftLeft.Location = new System.Drawing.Point(8, 6);
      this.btnShiftLeft.Name = "btnShiftLeft";
      this.btnShiftLeft.Size = new System.Drawing.Size(26, 26);
      this.btnShiftLeft.TabIndex = 7;
      this.toolTip1.SetToolTip(this.btnShiftLeft, "Shift selected sprites left");
      this.btnShiftLeft.UseVisualStyleBackColor = true;
      this.btnShiftLeft.Click += new System.EventHandler(this.btnShiftLeft_Click);
      // 
      // btnCopyToClipboard
      // 
      this.btnCopyToClipboard.Location = new System.Drawing.Point(227, 356);
      this.btnCopyToClipboard.Name = "btnCopyToClipboard";
      this.btnCopyToClipboard.Size = new System.Drawing.Size(121, 23);
      this.btnCopyToClipboard.TabIndex = 6;
      this.btnCopyToClipboard.Text = "Copy to Clipboard";
      this.btnCopyToClipboard.UseVisualStyleBackColor = true;
      this.btnCopyToClipboard.Click += new System.EventHandler(this.btnCopyToClipboard_Click);
      // 
      // btnPasteFromClipboard
      // 
      this.btnPasteFromClipboard.Location = new System.Drawing.Point(227, 328);
      this.btnPasteFromClipboard.Name = "btnPasteFromClipboard";
      this.btnPasteFromClipboard.Size = new System.Drawing.Size(121, 23);
      this.btnPasteFromClipboard.TabIndex = 6;
      this.btnPasteFromClipboard.Text = "Paste from Clipboard";
      this.btnPasteFromClipboard.UseVisualStyleBackColor = true;
      this.btnPasteFromClipboard.Click += new System.EventHandler(this.btnPasteFromClipboard_Click);
      // 
      // labelCharNo
      // 
      this.labelCharNo.Location = new System.Drawing.Point(304, 306);
      this.labelCharNo.Name = "labelCharNo";
      this.labelCharNo.Size = new System.Drawing.Size(82, 23);
      this.labelCharNo.TabIndex = 5;
      this.labelCharNo.Text = "label1";
      // 
      // checkShowGrid
      // 
      this.checkShowGrid.AutoSize = true;
      this.checkShowGrid.Location = new System.Drawing.Point(365, 358);
      this.checkShowGrid.Name = "checkShowGrid";
      this.checkShowGrid.Size = new System.Drawing.Size(75, 17);
      this.checkShowGrid.TabIndex = 3;
      this.checkShowGrid.Text = "Show Grid";
      this.checkShowGrid.UseVisualStyleBackColor = true;
      this.checkShowGrid.CheckedChanged += new System.EventHandler(this.checkShowGrid_CheckedChanged);
      // 
      // checkMulticolor
      // 
      this.checkMulticolor.AutoSize = true;
      this.checkMulticolor.Location = new System.Drawing.Point(227, 305);
      this.checkMulticolor.Name = "checkMulticolor";
      this.checkMulticolor.Size = new System.Drawing.Size(71, 17);
      this.checkMulticolor.TabIndex = 3;
      this.checkMulticolor.Text = "Multicolor";
      this.checkMulticolor.UseVisualStyleBackColor = true;
      this.checkMulticolor.CheckedChanged += new System.EventHandler(this.checkMulticolor_CheckedChanged);
      // 
      // radioSpriteColor
      // 
      this.radioSpriteColor.AutoSize = true;
      this.radioSpriteColor.Location = new System.Drawing.Point(44, 385);
      this.radioSpriteColor.Name = "radioSpriteColor";
      this.radioSpriteColor.Size = new System.Drawing.Size(79, 17);
      this.radioSpriteColor.TabIndex = 2;
      this.radioSpriteColor.TabStop = true;
      this.radioSpriteColor.Text = "Sprite Color";
      this.radioSpriteColor.UseVisualStyleBackColor = true;
      this.radioSpriteColor.CheckedChanged += new System.EventHandler(this.radioCharColor_CheckedChanged);
      // 
      // radioMulticolor2
      // 
      this.radioMulticolor2.AutoSize = true;
      this.radioMulticolor2.Location = new System.Drawing.Point(44, 358);
      this.radioMulticolor2.Name = "radioMulticolor2";
      this.radioMulticolor2.Size = new System.Drawing.Size(79, 17);
      this.radioMulticolor2.TabIndex = 2;
      this.radioMulticolor2.TabStop = true;
      this.radioMulticolor2.Text = "Multicolor 2";
      this.radioMulticolor2.UseVisualStyleBackColor = true;
      this.radioMulticolor2.CheckedChanged += new System.EventHandler(this.radioMulticolor2_CheckedChanged);
      // 
      // radioMultiColor1
      // 
      this.radioMultiColor1.AutoSize = true;
      this.radioMultiColor1.Location = new System.Drawing.Point(44, 331);
      this.radioMultiColor1.Name = "radioMultiColor1";
      this.radioMultiColor1.Size = new System.Drawing.Size(79, 17);
      this.radioMultiColor1.TabIndex = 2;
      this.radioMultiColor1.TabStop = true;
      this.radioMultiColor1.Text = "Multicolor 1";
      this.radioMultiColor1.UseVisualStyleBackColor = true;
      this.radioMultiColor1.CheckedChanged += new System.EventHandler(this.radioMultiColor1_CheckedChanged);
      // 
      // radioBackground
      // 
      this.radioBackground.AutoSize = true;
      this.radioBackground.Location = new System.Drawing.Point(44, 304);
      this.radioBackground.Name = "radioBackground";
      this.radioBackground.Size = new System.Drawing.Size(83, 17);
      this.radioBackground.TabIndex = 2;
      this.radioBackground.TabStop = true;
      this.radioBackground.Text = "Background";
      this.radioBackground.UseVisualStyleBackColor = true;
      this.radioBackground.CheckedChanged += new System.EventHandler(this.radioBackground_CheckedChanged);
      // 
      // comboSpriteColor
      // 
      this.comboSpriteColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboSpriteColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboSpriteColor.FormattingEnabled = true;
      this.comboSpriteColor.Location = new System.Drawing.Point(133, 385);
      this.comboSpriteColor.Name = "comboSpriteColor";
      this.comboSpriteColor.Size = new System.Drawing.Size(79, 21);
      this.comboSpriteColor.TabIndex = 1;
      this.comboSpriteColor.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboSpriteColor_DrawItem);
      this.comboSpriteColor.SelectedIndexChanged += new System.EventHandler(this.comboSpriteColor_SelectedIndexChanged);
      // 
      // comboMulticolor2
      // 
      this.comboMulticolor2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboMulticolor2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboMulticolor2.FormattingEnabled = true;
      this.comboMulticolor2.Location = new System.Drawing.Point(133, 358);
      this.comboMulticolor2.Name = "comboMulticolor2";
      this.comboMulticolor2.Size = new System.Drawing.Size(79, 21);
      this.comboMulticolor2.TabIndex = 1;
      this.comboMulticolor2.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboColor_DrawItem);
      this.comboMulticolor2.SelectedIndexChanged += new System.EventHandler(this.comboMulticolor2_SelectedIndexChanged);
      // 
      // comboMulticolor1
      // 
      this.comboMulticolor1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboMulticolor1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboMulticolor1.FormattingEnabled = true;
      this.comboMulticolor1.Location = new System.Drawing.Point(133, 331);
      this.comboMulticolor1.Name = "comboMulticolor1";
      this.comboMulticolor1.Size = new System.Drawing.Size(79, 21);
      this.comboMulticolor1.TabIndex = 1;
      this.comboMulticolor1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboColor_DrawItem);
      this.comboMulticolor1.SelectedIndexChanged += new System.EventHandler(this.comboMulticolor1_SelectedIndexChanged);
      // 
      // comboBackground
      // 
      this.comboBackground.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboBackground.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBackground.FormattingEnabled = true;
      this.comboBackground.Location = new System.Drawing.Point(133, 303);
      this.comboBackground.Name = "comboBackground";
      this.comboBackground.Size = new System.Drawing.Size(79, 21);
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
      this.pictureEditor.Location = new System.Drawing.Point(40, 6);
      this.pictureEditor.Name = "pictureEditor";
      this.pictureEditor.Size = new System.Drawing.Size(357, 282);
      this.pictureEditor.TabIndex = 0;
      this.pictureEditor.TabStop = false;
      this.pictureEditor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureEditor_MouseDown);
      this.pictureEditor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureEditor_MouseMove);
      // 
      // tabProject
      // 
      this.tabProject.Controls.Add(this.groupBox1);
      this.tabProject.Controls.Add(this.groupExport);
      this.tabProject.Location = new System.Drawing.Point(4, 22);
      this.tabProject.Name = "tabProject";
      this.tabProject.Padding = new System.Windows.Forms.Padding(3);
      this.tabProject.Size = new System.Drawing.Size(774, 473);
      this.tabProject.TabIndex = 1;
      this.tabProject.Text = "Import/Export";
      this.tabProject.UseVisualStyleBackColor = true;
      // 
      // groupBox1
      // 
      this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.groupBox1.Controls.Add(this.btnImportFromFile);
      this.groupBox1.Controls.Add(this.btnClearImport);
      this.groupBox1.Controls.Add(this.btnImportFromASM);
      this.groupBox1.Controls.Add(this.btnImportFromBASICHex);
      this.groupBox1.Controls.Add(this.btnImportFromBASIC);
      this.groupBox1.Controls.Add(this.btnImportFromHex);
      this.groupBox1.Controls.Add(this.button2);
      this.groupBox1.Controls.Add(this.editDataImport);
      this.groupBox1.Location = new System.Drawing.Point(425, 6);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(331, 403);
      this.groupBox1.TabIndex = 4;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Import";
      // 
      // btnImportFromFile
      // 
      this.btnImportFromFile.Location = new System.Drawing.Point(9, 19);
      this.btnImportFromFile.Name = "btnImportFromFile";
      this.btnImportFromFile.Size = new System.Drawing.Size(101, 23);
      this.btnImportFromFile.TabIndex = 2;
      this.btnImportFromFile.Text = "From File...";
      this.btnImportFromFile.UseVisualStyleBackColor = true;
      this.btnImportFromFile.Click += new System.EventHandler(this.btnImportSprite_Click);
      // 
      // btnClearImport
      // 
      this.btnClearImport.Location = new System.Drawing.Point(9, 80);
      this.btnClearImport.Name = "btnClearImport";
      this.btnClearImport.Size = new System.Drawing.Size(101, 23);
      this.btnClearImport.TabIndex = 2;
      this.btnClearImport.Text = "Clear";
      this.btnClearImport.UseVisualStyleBackColor = true;
      this.btnClearImport.Click += new System.EventHandler(this.btnClear_Click);
      // 
      // btnImportFromASM
      // 
      this.btnImportFromASM.Location = new System.Drawing.Point(9, 50);
      this.btnImportFromASM.Name = "btnImportFromASM";
      this.btnImportFromASM.Size = new System.Drawing.Size(101, 23);
      this.btnImportFromASM.TabIndex = 2;
      this.btnImportFromASM.Text = "From ASM";
      this.btnImportFromASM.UseVisualStyleBackColor = true;
      this.btnImportFromASM.Click += new System.EventHandler(this.btnImportFromASM_Click);
      // 
      // btnImportFromBASICHex
      // 
      this.btnImportFromBASICHex.Location = new System.Drawing.Point(223, 50);
      this.btnImportFromBASICHex.Name = "btnImportFromBASICHex";
      this.btnImportFromBASICHex.Size = new System.Drawing.Size(101, 23);
      this.btnImportFromBASICHex.TabIndex = 2;
      this.btnImportFromBASICHex.Text = "From BASIC hex";
      this.btnImportFromBASICHex.UseVisualStyleBackColor = true;
      this.btnImportFromBASICHex.Click += new System.EventHandler(this.btnImportFromBASICHex_Click);
      // 
      // btnImportFromBASIC
      // 
      this.btnImportFromBASIC.Location = new System.Drawing.Point(116, 50);
      this.btnImportFromBASIC.Name = "btnImportFromBASIC";
      this.btnImportFromBASIC.Size = new System.Drawing.Size(101, 23);
      this.btnImportFromBASIC.TabIndex = 2;
      this.btnImportFromBASIC.Text = "From BASIC";
      this.btnImportFromBASIC.UseVisualStyleBackColor = true;
      this.btnImportFromBASIC.Click += new System.EventHandler(this.btnImportFromBASIC_Click);
      // 
      // btnImportFromHex
      // 
      this.btnImportFromHex.Location = new System.Drawing.Point(223, 19);
      this.btnImportFromHex.Name = "btnImportFromHex";
      this.btnImportFromHex.Size = new System.Drawing.Size(101, 23);
      this.btnImportFromHex.TabIndex = 2;
      this.btnImportFromHex.Text = "From Hex";
      this.btnImportFromHex.UseVisualStyleBackColor = true;
      this.btnImportFromHex.Click += new System.EventHandler(this.btnImportFromHex_Click);
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(116, 19);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(101, 23);
      this.button2.TabIndex = 2;
      this.button2.Text = "From Image...";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler(this.btnImportFromImage_Click);
      // 
      // editDataImport
      // 
      this.editDataImport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.editDataImport.Location = new System.Drawing.Point(9, 148);
      this.editDataImport.Multiline = true;
      this.editDataImport.Name = "editDataImport";
      this.editDataImport.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.editDataImport.Size = new System.Drawing.Size(316, 249);
      this.editDataImport.TabIndex = 3;
      this.editDataImport.WordWrap = false;
      this.editDataImport.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editDataImport_KeyPress);
      // 
      // groupExport
      // 
      this.groupExport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.groupExport.Controls.Add(this.editExportBASICLineOffset);
      this.groupExport.Controls.Add(this.comboExportRange);
      this.groupExport.Controls.Add(this.editExportBASICLineNo);
      this.groupExport.Controls.Add(this.editSpriteCount);
      this.groupExport.Controls.Add(this.label1);
      this.groupExport.Controls.Add(this.editSpriteFrom);
      this.groupExport.Controls.Add(this.label8);
      this.groupExport.Controls.Add(this.labelCharactersTo);
      this.groupExport.Controls.Add(this.btnToBASICHex);
      this.groupExport.Controls.Add(this.btnExportToBASICData);
      this.groupExport.Controls.Add(this.labelCharactersFrom);
      this.groupExport.Controls.Add(this.editPrefix);
      this.groupExport.Controls.Add(this.label2);
      this.groupExport.Controls.Add(this.editWrapByteCount);
      this.groupExport.Controls.Add(this.checkExportToDataWrap);
      this.groupExport.Controls.Add(this.checkExportToDataIncludeRes);
      this.groupExport.Controls.Add(this.editDataExport);
      this.groupExport.Controls.Add(this.button1);
      this.groupExport.Controls.Add(this.btnExportToData);
      this.groupExport.Controls.Add(this.btnExportCharset);
      this.groupExport.Location = new System.Drawing.Point(8, 6);
      this.groupExport.Name = "groupExport";
      this.groupExport.Size = new System.Drawing.Size(411, 459);
      this.groupExport.TabIndex = 3;
      this.groupExport.TabStop = false;
      this.groupExport.Text = "Export";
      // 
      // editExportBASICLineOffset
      // 
      this.editExportBASICLineOffset.Location = new System.Drawing.Point(352, 108);
      this.editExportBASICLineOffset.Name = "editExportBASICLineOffset";
      this.editExportBASICLineOffset.Size = new System.Drawing.Size(36, 20);
      this.editExportBASICLineOffset.TabIndex = 28;
      this.editExportBASICLineOffset.Text = "10";
      // 
      // comboExportRange
      // 
      this.comboExportRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboExportRange.FormattingEnabled = true;
      this.comboExportRange.Location = new System.Drawing.Point(129, 21);
      this.comboExportRange.Name = "comboExportRange";
      this.comboExportRange.Size = new System.Drawing.Size(88, 21);
      this.comboExportRange.TabIndex = 14;
      this.comboExportRange.SelectedIndexChanged += new System.EventHandler(this.comboExportRange_SelectedIndexChanged);
      // 
      // editExportBASICLineNo
      // 
      this.editExportBASICLineNo.Location = new System.Drawing.Point(181, 108);
      this.editExportBASICLineNo.Name = "editExportBASICLineNo";
      this.editExportBASICLineNo.Size = new System.Drawing.Size(98, 20);
      this.editExportBASICLineNo.TabIndex = 29;
      this.editExportBASICLineNo.Text = "10";
      // 
      // editSpriteCount
      // 
      this.editSpriteCount.Location = new System.Drawing.Point(352, 22);
      this.editSpriteCount.Name = "editSpriteCount";
      this.editSpriteCount.Size = new System.Drawing.Size(36, 20);
      this.editSpriteCount.TabIndex = 12;
      this.editSpriteCount.TextChanged += new System.EventHandler(this.editSpriteCount_TextChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(285, 111);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(55, 13);
      this.label1.TabIndex = 26;
      this.label1.Text = "Line Step:";
      // 
      // editSpriteFrom
      // 
      this.editSpriteFrom.Location = new System.Drawing.Point(262, 21);
      this.editSpriteFrom.Name = "editSpriteFrom";
      this.editSpriteFrom.Size = new System.Drawing.Size(39, 20);
      this.editSpriteFrom.TabIndex = 11;
      this.editSpriteFrom.TextChanged += new System.EventHandler(this.editSpriteFrom_TextChanged);
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(128, 111);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(55, 13);
      this.label8.TabIndex = 27;
      this.label8.Text = "Start Line:";
      // 
      // labelCharactersTo
      // 
      this.labelCharactersTo.AutoSize = true;
      this.labelCharactersTo.Location = new System.Drawing.Point(307, 24);
      this.labelCharactersTo.Name = "labelCharactersTo";
      this.labelCharactersTo.Size = new System.Drawing.Size(37, 13);
      this.labelCharactersTo.TabIndex = 10;
      this.labelCharactersTo.Text = "count:";
      // 
      // btnToBASICHex
      // 
      this.btnToBASICHex.Location = new System.Drawing.Point(6, 135);
      this.btnToBASICHex.Name = "btnToBASICHex";
      this.btnToBASICHex.Size = new System.Drawing.Size(117, 23);
      this.btnToBASICHex.TabIndex = 25;
      this.btnToBASICHex.Text = "To BASIC hex data";
      this.btnToBASICHex.UseVisualStyleBackColor = true;
      this.btnToBASICHex.Click += new System.EventHandler(this.btnExportToBASICHexData_Click);
      // 
      // btnExportToBASICData
      // 
      this.btnExportToBASICData.Location = new System.Drawing.Point(6, 106);
      this.btnExportToBASICData.Name = "btnExportToBASICData";
      this.btnExportToBASICData.Size = new System.Drawing.Size(117, 23);
      this.btnExportToBASICData.TabIndex = 25;
      this.btnExportToBASICData.Text = "To BASIC data";
      this.btnExportToBASICData.UseVisualStyleBackColor = true;
      this.btnExportToBASICData.Click += new System.EventHandler(this.btnExportToBASICData_Click);
      // 
      // labelCharactersFrom
      // 
      this.labelCharactersFrom.AutoSize = true;
      this.labelCharactersFrom.Location = new System.Drawing.Point(226, 24);
      this.labelCharactersFrom.Name = "labelCharactersFrom";
      this.labelCharactersFrom.Size = new System.Drawing.Size(30, 13);
      this.labelCharactersFrom.TabIndex = 9;
      this.labelCharactersFrom.Text = "from:";
      // 
      // editPrefix
      // 
      this.editPrefix.Location = new System.Drawing.Point(225, 48);
      this.editPrefix.Name = "editPrefix";
      this.editPrefix.Size = new System.Drawing.Size(54, 20);
      this.editPrefix.TabIndex = 7;
      this.editPrefix.Text = "!byte ";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(285, 80);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(32, 13);
      this.label2.TabIndex = 6;
      this.label2.Text = "bytes";
      // 
      // editWrapByteCount
      // 
      this.editWrapByteCount.Enabled = false;
      this.editWrapByteCount.Location = new System.Drawing.Point(225, 77);
      this.editWrapByteCount.Name = "editWrapByteCount";
      this.editWrapByteCount.Size = new System.Drawing.Size(54, 20);
      this.editWrapByteCount.TabIndex = 5;
      this.editWrapByteCount.Text = "8";
      // 
      // checkExportToDataWrap
      // 
      this.checkExportToDataWrap.AutoSize = true;
      this.checkExportToDataWrap.Location = new System.Drawing.Point(129, 79);
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
      this.checkExportToDataIncludeRes.Location = new System.Drawing.Point(129, 50);
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
      this.editDataExport.Location = new System.Drawing.Point(6, 166);
      this.editDataExport.Multiline = true;
      this.editDataExport.Name = "editDataExport";
      this.editDataExport.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.editDataExport.Size = new System.Drawing.Size(399, 287);
      this.editDataExport.TabIndex = 3;
      this.editDataExport.WordWrap = false;
      this.editDataExport.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editDataExport_KeyPress);
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(6, 77);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(117, 23);
      this.button1.TabIndex = 2;
      this.button1.Text = "To Image...";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.btnExportSpriteToImage_Click);
      // 
      // btnExportToData
      // 
      this.btnExportToData.Location = new System.Drawing.Point(6, 48);
      this.btnExportToData.Name = "btnExportToData";
      this.btnExportToData.Size = new System.Drawing.Size(117, 23);
      this.btnExportToData.TabIndex = 2;
      this.btnExportToData.Text = "To Data";
      this.btnExportToData.UseVisualStyleBackColor = true;
      this.btnExportToData.Click += new System.EventHandler(this.btnExportSpriteToData_Click);
      // 
      // btnExportCharset
      // 
      this.btnExportCharset.Location = new System.Drawing.Point(6, 19);
      this.btnExportCharset.Name = "btnExportCharset";
      this.btnExportCharset.Size = new System.Drawing.Size(117, 23);
      this.btnExportCharset.TabIndex = 2;
      this.btnExportCharset.Text = "To File...";
      this.btnExportCharset.UseVisualStyleBackColor = true;
      this.btnExportCharset.Click += new System.EventHandler(this.btnExportSprite_Click);
      // 
      // menuStrip1
      // 
      this.menuStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(782, 24);
      this.menuStrip1.TabIndex = 1;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openCharsetProjectToolStripMenuItem,
            this.saveSpriteProjectToolStripMenuItem,
            this.closeCharsetProjectToolStripMenuItem});
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
      this.fileToolStripMenuItem.Text = "&Sprites";
      // 
      // openCharsetProjectToolStripMenuItem
      // 
      this.openCharsetProjectToolStripMenuItem.Name = "openCharsetProjectToolStripMenuItem";
      this.openCharsetProjectToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
      this.openCharsetProjectToolStripMenuItem.Text = "&Open Sprite Project...";
      this.openCharsetProjectToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
      // 
      // saveSpriteProjectToolStripMenuItem
      // 
      this.saveSpriteProjectToolStripMenuItem.Name = "saveSpriteProjectToolStripMenuItem";
      this.saveSpriteProjectToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
      this.saveSpriteProjectToolStripMenuItem.Text = "&Save Project";
      this.saveSpriteProjectToolStripMenuItem.Click += new System.EventHandler(this.saveCharsetProjectToolStripMenuItem_Click);
      // 
      // closeCharsetProjectToolStripMenuItem
      // 
      this.closeCharsetProjectToolStripMenuItem.Enabled = false;
      this.closeCharsetProjectToolStripMenuItem.Name = "closeCharsetProjectToolStripMenuItem";
      this.closeCharsetProjectToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
      this.closeCharsetProjectToolStripMenuItem.Text = "&Close Sprite Project";
      this.closeCharsetProjectToolStripMenuItem.Click += new System.EventHandler(this.closeCharsetProjectToolStripMenuItem_Click);
      // 
      // columnHeader4
      // 
      this.columnHeader4.Text = "Nr.";
      // 
      // columnHeader5
      // 
      this.columnHeader5.Text = "X";
      this.columnHeader5.Width = 30;
      // 
      // columnHeader6
      // 
      this.columnHeader6.Text = "Y";
      this.columnHeader6.Width = 30;
      // 
      // btnToolFill
      // 
      this.btnToolFill.Appearance = System.Windows.Forms.Appearance.Button;
      this.btnToolFill.Image = global::C64Studio.Properties.Resources.tool_fill;
      this.btnToolFill.Location = new System.Drawing.Point(8, 316);
      this.btnToolFill.Name = "btnToolFill";
      this.btnToolFill.Size = new System.Drawing.Size(26, 26);
      this.btnToolFill.TabIndex = 57;
      this.toolTip1.SetToolTip(this.btnToolFill, "Fill");
      this.btnToolFill.UseVisualStyleBackColor = true;
      this.btnToolFill.CheckedChanged += new System.EventHandler(this.btnToolFill_CheckedChanged);
      // 
      // btnToolEdit
      // 
      this.btnToolEdit.Appearance = System.Windows.Forms.Appearance.Button;
      this.btnToolEdit.Checked = true;
      this.btnToolEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnToolEdit.Image")));
      this.btnToolEdit.Location = new System.Drawing.Point(8, 285);
      this.btnToolEdit.Name = "btnToolEdit";
      this.btnToolEdit.Size = new System.Drawing.Size(26, 26);
      this.btnToolEdit.TabIndex = 58;
      this.btnToolEdit.TabStop = true;
      this.toolTip1.SetToolTip(this.btnToolEdit, "Single Character");
      this.btnToolEdit.UseVisualStyleBackColor = true;
      this.btnToolEdit.CheckedChanged += new System.EventHandler(this.btnToolEdit_CheckedChanged);
      // 
      // SpriteEditor
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.ClientSize = new System.Drawing.Size(782, 523);
      this.Controls.Add(this.tabSpriteEditor);
      this.Controls.Add(this.menuStrip1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "SpriteEditor";
      this.Text = "Sprite Editor";
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      this.tabSpriteEditor.ResumeLayout(false);
      this.tabEditor.ResumeLayout(false);
      this.tabEditor.PerformLayout();
      this.contextMenuExchangeColors.ResumeLayout(false);
      this.tabSpriteDetails.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.tabPage2.ResumeLayout(false);
      this.tabPage2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.layerPreview)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureEditor)).EndInit();
      this.tabProject.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupExport.ResumeLayout(false);
      this.groupExport.PerformLayout();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TabControl tabSpriteEditor;
    private System.Windows.Forms.TabPage tabEditor;
    private System.Windows.Forms.TabPage tabProject;
    private GR.Forms.FastPictureBox pictureEditor;
    private System.Windows.Forms.ComboBox comboBackground;
    private System.Windows.Forms.RadioButton radioBackground;
    private System.Windows.Forms.RadioButton radioSpriteColor;
    private System.Windows.Forms.RadioButton radioMulticolor2;
    private System.Windows.Forms.RadioButton radioMultiColor1;
    private System.Windows.Forms.ComboBox comboSpriteColor;
    private System.Windows.Forms.ComboBox comboMulticolor2;
    private System.Windows.Forms.ComboBox comboMulticolor1;
    private System.Windows.Forms.CheckBox checkMulticolor;
    private GR.Forms.ImageListbox panelSprites;
    private System.Windows.Forms.Label labelCharNo;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem openCharsetProjectToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem closeCharsetProjectToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveSpriteProjectToolStripMenuItem;
    private System.Windows.Forms.Button btnExportCharset;
    private System.Windows.Forms.GroupBox groupExport;
    private System.Windows.Forms.TextBox editDataExport;
    private System.Windows.Forms.Button btnExportToData;
    private System.Windows.Forms.CheckBox checkExportToDataIncludeRes;
    private System.Windows.Forms.CheckBox checkExportToDataWrap;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox editWrapByteCount;
    private System.Windows.Forms.TextBox editPrefix;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Button btnImportFromFile;
    private System.Windows.Forms.Button btnCopyToClipboard;
    private System.Windows.Forms.Button btnPasteFromClipboard;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Button btnShiftLeft;
    private System.Windows.Forms.Button btnShiftDown;
    private System.Windows.Forms.Button btnShiftUp;
    private System.Windows.Forms.Button btnShiftRight;
    private System.Windows.Forms.Button btnMirrorX;
    private System.Windows.Forms.Button btnMirrorY;
    private System.Windows.Forms.CheckBox checkShowGrid;
    private System.Windows.Forms.Button btnInvert;
    private System.Windows.Forms.Button btnRotateLeft;
    private System.Windows.Forms.Button btnRotateRight;
    private System.Windows.Forms.Button btnDeleteSprite;
    private System.Windows.Forms.TabControl tabSpriteDetails;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.TabPage tabPage2;
    private System.Windows.Forms.TextBox editLayerY;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TextBox editLayerX;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.ComboBox comboLayerBGColor;
    private System.Windows.Forms.ComboBox comboLayerColor;
    private System.Windows.Forms.ComboBox comboSprite;
    private GR.Forms.FastPictureBox layerPreview;
    private ArrangedItemList listLayers;
    private ArrangedItemList listLayerSprites;
    private System.Windows.Forms.ColumnHeader columnHeader4;
    private System.Windows.Forms.ColumnHeader columnHeader5;
    private System.Windows.Forms.ColumnHeader columnHeader6;
    private System.Windows.Forms.ColumnHeader columnHeader7;
    private System.Windows.Forms.ColumnHeader columnHeader8;
    private System.Windows.Forms.ColumnHeader columnHeader9;
    private System.Windows.Forms.TextBox editLayerName;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.ComboBox comboExportRange;
    private System.Windows.Forms.TextBox editSpriteCount;
    private System.Windows.Forms.TextBox editSpriteFrom;
    private System.Windows.Forms.Label labelCharactersTo;
    private System.Windows.Forms.Label labelCharactersFrom;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.CheckBox checkExpandY;
    private System.Windows.Forms.CheckBox checkExpandX;
    private System.Windows.Forms.Button btnImportFromHex;
    private Controls.MenuButton btnExchangeColors;
    private System.Windows.Forms.ContextMenuStrip contextMenuExchangeColors;
    private System.Windows.Forms.ToolStripMenuItem exchangeMultiColor1WithMultiColor2ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exchangeMultiColor1WithBGColorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exchangeMultiColor2WithBGColorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem forSelectedSpritesToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exchangeMulticolor1WithSpriteColorToolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem exchangeMulticolor2WithSpriteColorToolStripMenuItem1;
    private System.Windows.Forms.TextBox editExportBASICLineOffset;
    private System.Windows.Forms.TextBox editExportBASICLineNo;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Button btnExportToBASICData;
    private System.Windows.Forms.ToolStripMenuItem exchangeBGColorWithSpriteColorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exchangeMulticolor1WithBGColorSelectedSpritesToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exchangeMulticolor2WithBGColorSelectedSpritesToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exchangeMulticolor1WithMulticolor2ToolStripMenuItem1;
    private System.Windows.Forms.TextBox editDataImport;
    private System.Windows.Forms.Button btnImportFromASM;
    private System.Windows.Forms.Button btnImportFromBASIC;
    private System.Windows.Forms.Button btnToBASICHex;
    private System.Windows.Forms.Button btnImportFromBASICHex;
        private System.Windows.Forms.Button btnClearImport;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.TextBox editLayerDelay;
    private System.Windows.Forms.CheckBox checkAutoplayAnim;
    private System.Windows.Forms.Button btnSavePreviewToGIF;
    private System.Windows.Forms.ComboBox comboSpriteProjectMode;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.Button btnEditPalette;
    private System.Windows.Forms.RadioButton btnToolFill;
    private System.Windows.Forms.RadioButton btnToolEdit;
  }
}
