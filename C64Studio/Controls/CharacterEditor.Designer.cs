namespace C64Studio.Controls
{
  partial class CharacterEditor
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CharacterEditor));
      GR.Image.FastImage fastImage1 = new GR.Image.FastImage();
      GR.Image.FastImage fastImage2 = new GR.Image.FastImage();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.btnClearChars = new System.Windows.Forms.Button();
      this.comboCategories = new System.Windows.Forms.ComboBox();
      this.btnPasteFromClipboard = new System.Windows.Forms.Button();
      this.label4 = new System.Windows.Forms.Label();
      this.labelCharNo = new System.Windows.Forms.Label();
      this.checkShowGrid = new System.Windows.Forms.CheckBox();
      this.checkPasteMultiColor = new System.Windows.Forms.CheckBox();
      this.radioCharColor = new System.Windows.Forms.RadioButton();
      this.radioBGColor4 = new System.Windows.Forms.RadioButton();
      this.radioMulticolor2 = new System.Windows.Forms.RadioButton();
      this.radioMultiColor1 = new System.Windows.Forms.RadioButton();
      this.radioBackground = new System.Windows.Forms.RadioButton();
      this.comboCharColor = new System.Windows.Forms.ComboBox();
      this.comboBGColor4 = new System.Windows.Forms.ComboBox();
      this.comboMulticolor2 = new System.Windows.Forms.ComboBox();
      this.comboMulticolor1 = new System.Windows.Forms.ComboBox();
      this.comboBackground = new System.Windows.Forms.ComboBox();
      this.btnPaste = new System.Windows.Forms.Button();
      this.btnCopy = new System.Windows.Forms.Button();
      this.btnInvert = new System.Windows.Forms.Button();
      this.btnMirrorY = new System.Windows.Forms.Button();
      this.btnMirrorX = new System.Windows.Forms.Button();
      this.btnShiftDown = new System.Windows.Forms.Button();
      this.btnShiftUp = new System.Windows.Forms.Button();
      this.btnShiftRight = new System.Windows.Forms.Button();
      this.button3 = new System.Windows.Forms.Button();
      this.btnRotateLeft = new System.Windows.Forms.Button();
      this.btnShiftLeft = new System.Windows.Forms.Button();
      this.contextMenuExchangeColors = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exchangeMultiColor1WithBGColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exchangeMultiColor2WithBGColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.btnMoveSelectionToTarget = new System.Windows.Forms.Button();
      this.editMoveTargetIndex = new System.Windows.Forms.TextBox();
      this.label10 = new System.Windows.Forms.Label();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.tabCharacterEditor = new System.Windows.Forms.TabControl();
      this.tabEditor = new System.Windows.Forms.TabPage();
      this.btnEditPalette = new System.Windows.Forms.Button();
      this.canvasEditor = new C64Studio.Controls.CustomDrawControl();
      this.comboCharsetMode = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.panelCharColors = new GR.Forms.FastPictureBox();
      this.picturePlayground = new GR.Forms.FastPictureBox();
      this.btnExchangeColors = new C64Studio.Controls.MenuButton();
      this.panelCharacters = new GR.Forms.ImageListbox();
      this.tabCategories = new System.Windows.Forms.TabPage();
      this.btnMoveCategoryDown = new System.Windows.Forms.Button();
      this.btnMoveCategoryUp = new System.Windows.Forms.Button();
      this.groupAllCategories = new System.Windows.Forms.GroupBox();
      this.btnSortCategories = new System.Windows.Forms.Button();
      this.groupCategorySpecific = new System.Windows.Forms.GroupBox();
      this.label5 = new System.Windows.Forms.Label();
      this.editCollapseIndex = new System.Windows.Forms.TextBox();
      this.btnCollapseCategory = new System.Windows.Forms.Button();
      this.btnReseatCategory = new System.Windows.Forms.Button();
      this.btnDelete = new System.Windows.Forms.Button();
      this.btnAddCategory = new System.Windows.Forms.Button();
      this.listCategories = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.editCategoryName = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.groupBox2.SuspendLayout();
      this.contextMenuExchangeColors.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.tabCharacterEditor.SuspendLayout();
      this.tabEditor.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.canvasEditor)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.panelCharColors)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picturePlayground)).BeginInit();
      this.tabCategories.SuspendLayout();
      this.groupAllCategories.SuspendLayout();
      this.groupCategorySpecific.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.btnClearChars);
      this.groupBox2.Location = new System.Drawing.Point(497, 301);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(150, 56);
      this.groupBox2.TabIndex = 50;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "With selected characters";
      // 
      // btnClearChars
      // 
      this.btnClearChars.Location = new System.Drawing.Point(6, 21);
      this.btnClearChars.Name = "btnClearChars";
      this.btnClearChars.Size = new System.Drawing.Size(48, 22);
      this.btnClearChars.TabIndex = 6;
      this.btnClearChars.Text = "Clear";
      this.btnClearChars.UseVisualStyleBackColor = true;
      this.btnClearChars.Click += new System.EventHandler(this.btnClear_Click);
      // 
      // comboCategories
      // 
      this.comboCategories.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboCategories.FormattingEnabled = true;
      this.comboCategories.Location = new System.Drawing.Point(387, 204);
      this.comboCategories.Name = "comboCategories";
      this.comboCategories.Size = new System.Drawing.Size(121, 21);
      this.comboCategories.TabIndex = 37;
      this.comboCategories.SelectedIndexChanged += new System.EventHandler(this.comboCategories_SelectedIndexChanged);
      // 
      // btnPasteFromClipboard
      // 
      this.btnPasteFromClipboard.Location = new System.Drawing.Point(578, 269);
      this.btnPasteFromClipboard.Name = "btnPasteFromClipboard";
      this.btnPasteFromClipboard.Size = new System.Drawing.Size(97, 26);
      this.btnPasteFromClipboard.TabIndex = 36;
      this.btnPasteFromClipboard.Text = "Paste Image";
      this.toolTip1.SetToolTip(this.btnPasteFromClipboard, "Paste Image");
      this.btnPasteFromClipboard.UseVisualStyleBackColor = true;
      this.btnPasteFromClipboard.Click += new System.EventHandler(this.btnPasteFromClipboard_Click);
      // 
      // label4
      // 
      this.label4.Location = new System.Drawing.Point(277, 207);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(86, 23);
      this.label4.TabIndex = 35;
      this.label4.Text = "Category:";
      // 
      // labelCharNo
      // 
      this.labelCharNo.Location = new System.Drawing.Point(277, 178);
      this.labelCharNo.Name = "labelCharNo";
      this.labelCharNo.Size = new System.Drawing.Size(231, 23);
      this.labelCharNo.TabIndex = 34;
      this.labelCharNo.Text = "label1";
      this.labelCharNo.Paint += new System.Windows.Forms.PaintEventHandler(this.labelCharNo_Paint);
      // 
      // checkShowGrid
      // 
      this.checkShowGrid.AutoSize = true;
      this.checkShowGrid.Location = new System.Drawing.Point(280, 246);
      this.checkShowGrid.Name = "checkShowGrid";
      this.checkShowGrid.Size = new System.Drawing.Size(75, 17);
      this.checkShowGrid.TabIndex = 32;
      this.checkShowGrid.Text = "Show Grid";
      this.checkShowGrid.UseVisualStyleBackColor = true;
      this.checkShowGrid.CheckedChanged += new System.EventHandler(this.checkShowGrid_CheckedChanged);
      // 
      // checkPasteMultiColor
      // 
      this.checkPasteMultiColor.AutoSize = true;
      this.checkPasteMultiColor.Location = new System.Drawing.Point(280, 275);
      this.checkPasteMultiColor.Name = "checkPasteMultiColor";
      this.checkPasteMultiColor.Size = new System.Drawing.Size(145, 17);
      this.checkPasteMultiColor.TabIndex = 31;
      this.checkPasteMultiColor.Text = "Force Multicolor on paste";
      this.checkPasteMultiColor.UseVisualStyleBackColor = true;
      // 
      // radioCharColor
      // 
      this.radioCharColor.AutoSize = true;
      this.radioCharColor.Location = new System.Drawing.Point(280, 111);
      this.radioCharColor.Name = "radioCharColor";
      this.radioCharColor.Size = new System.Drawing.Size(74, 17);
      this.radioCharColor.TabIndex = 30;
      this.radioCharColor.TabStop = true;
      this.radioCharColor.Text = "Char Color";
      this.radioCharColor.UseVisualStyleBackColor = true;
      this.radioCharColor.CheckedChanged += new System.EventHandler(this.radioCharColor_CheckedChanged);
      // 
      // radioBGColor4
      // 
      this.radioBGColor4.AutoSize = true;
      this.radioBGColor4.Enabled = false;
      this.radioBGColor4.Location = new System.Drawing.Point(280, 84);
      this.radioBGColor4.Name = "radioBGColor4";
      this.radioBGColor4.Size = new System.Drawing.Size(76, 17);
      this.radioBGColor4.TabIndex = 29;
      this.radioBGColor4.TabStop = true;
      this.radioBGColor4.Text = "BG Color 4";
      this.radioBGColor4.UseVisualStyleBackColor = true;
      // 
      // radioMulticolor2
      // 
      this.radioMulticolor2.AutoSize = true;
      this.radioMulticolor2.Location = new System.Drawing.Point(280, 57);
      this.radioMulticolor2.Name = "radioMulticolor2";
      this.radioMulticolor2.Size = new System.Drawing.Size(79, 17);
      this.radioMulticolor2.TabIndex = 28;
      this.radioMulticolor2.TabStop = true;
      this.radioMulticolor2.Text = "Multicolor 2";
      this.radioMulticolor2.UseVisualStyleBackColor = true;
      this.radioMulticolor2.CheckedChanged += new System.EventHandler(this.radioMulticolor2_CheckedChanged);
      // 
      // radioMultiColor1
      // 
      this.radioMultiColor1.AutoSize = true;
      this.radioMultiColor1.Location = new System.Drawing.Point(280, 30);
      this.radioMultiColor1.Name = "radioMultiColor1";
      this.radioMultiColor1.Size = new System.Drawing.Size(79, 17);
      this.radioMultiColor1.TabIndex = 27;
      this.radioMultiColor1.TabStop = true;
      this.radioMultiColor1.Text = "Multicolor 1";
      this.radioMultiColor1.UseVisualStyleBackColor = true;
      this.radioMultiColor1.CheckedChanged += new System.EventHandler(this.radioMultiColor1_CheckedChanged);
      // 
      // radioBackground
      // 
      this.radioBackground.AutoSize = true;
      this.radioBackground.Location = new System.Drawing.Point(280, 3);
      this.radioBackground.Name = "radioBackground";
      this.radioBackground.Size = new System.Drawing.Size(83, 17);
      this.radioBackground.TabIndex = 26;
      this.radioBackground.TabStop = true;
      this.radioBackground.Text = "Background";
      this.radioBackground.UseVisualStyleBackColor = true;
      this.radioBackground.CheckedChanged += new System.EventHandler(this.radioBackground_CheckedChanged);
      // 
      // comboCharColor
      // 
      this.comboCharColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboCharColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboCharColor.FormattingEnabled = true;
      this.comboCharColor.Location = new System.Drawing.Point(387, 111);
      this.comboCharColor.Name = "comboCharColor";
      this.comboCharColor.Size = new System.Drawing.Size(71, 21);
      this.comboCharColor.TabIndex = 24;
      this.comboCharColor.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboCharColor_DrawItem);
      this.comboCharColor.SelectedIndexChanged += new System.EventHandler(this.comboCharColor_SelectedIndexChanged);
      // 
      // comboBGColor4
      // 
      this.comboBGColor4.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboBGColor4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBGColor4.FormattingEnabled = true;
      this.comboBGColor4.Location = new System.Drawing.Point(387, 84);
      this.comboBGColor4.Name = "comboBGColor4";
      this.comboBGColor4.Size = new System.Drawing.Size(71, 21);
      this.comboBGColor4.TabIndex = 23;
      this.comboBGColor4.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboColor_DrawItem);
      this.comboBGColor4.SelectedIndexChanged += new System.EventHandler(this.comboBGColor4_SelectedIndexChanged);
      // 
      // comboMulticolor2
      // 
      this.comboMulticolor2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboMulticolor2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboMulticolor2.FormattingEnabled = true;
      this.comboMulticolor2.Location = new System.Drawing.Point(387, 57);
      this.comboMulticolor2.Name = "comboMulticolor2";
      this.comboMulticolor2.Size = new System.Drawing.Size(71, 21);
      this.comboMulticolor2.TabIndex = 22;
      this.comboMulticolor2.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboColor_DrawItem);
      this.comboMulticolor2.SelectedIndexChanged += new System.EventHandler(this.comboMulticolor2_SelectedIndexChanged);
      // 
      // comboMulticolor1
      // 
      this.comboMulticolor1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboMulticolor1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboMulticolor1.FormattingEnabled = true;
      this.comboMulticolor1.Location = new System.Drawing.Point(387, 30);
      this.comboMulticolor1.Name = "comboMulticolor1";
      this.comboMulticolor1.Size = new System.Drawing.Size(71, 21);
      this.comboMulticolor1.TabIndex = 21;
      this.comboMulticolor1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboColor_DrawItem);
      this.comboMulticolor1.SelectedIndexChanged += new System.EventHandler(this.comboMulticolor1_SelectedIndexChanged);
      // 
      // comboBackground
      // 
      this.comboBackground.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboBackground.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBackground.FormattingEnabled = true;
      this.comboBackground.Location = new System.Drawing.Point(387, 3);
      this.comboBackground.Name = "comboBackground";
      this.comboBackground.Size = new System.Drawing.Size(71, 21);
      this.comboBackground.TabIndex = 25;
      this.comboBackground.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboColor_DrawItem);
      this.comboBackground.SelectedIndexChanged += new System.EventHandler(this.comboBackground_SelectedIndexChanged);
      // 
      // btnPaste
      // 
      this.btnPaste.Image = ((System.Drawing.Image)(resources.GetObject("btnPaste.Image")));
      this.btnPaste.Location = new System.Drawing.Point(546, 269);
      this.btnPaste.Name = "btnPaste";
      this.btnPaste.Size = new System.Drawing.Size(26, 26);
      this.btnPaste.TabIndex = 46;
      this.toolTip1.SetToolTip(this.btnPaste, "Paste Characters");
      this.btnPaste.UseVisualStyleBackColor = true;
      this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
      // 
      // btnCopy
      // 
      this.btnCopy.Image = ((System.Drawing.Image)(resources.GetObject("btnCopy.Image")));
      this.btnCopy.Location = new System.Drawing.Point(514, 269);
      this.btnCopy.Name = "btnCopy";
      this.btnCopy.Size = new System.Drawing.Size(26, 26);
      this.btnCopy.TabIndex = 45;
      this.toolTip1.SetToolTip(this.btnCopy, "Copy Characters to Clipboard");
      this.btnCopy.UseVisualStyleBackColor = true;
      this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
      // 
      // btnInvert
      // 
      this.btnInvert.Image = ((System.Drawing.Image)(resources.GetObject("btnInvert.Image")));
      this.btnInvert.Location = new System.Drawing.Point(198, 269);
      this.btnInvert.Name = "btnInvert";
      this.btnInvert.Size = new System.Drawing.Size(26, 26);
      this.btnInvert.TabIndex = 44;
      this.toolTip1.SetToolTip(this.btnInvert, "Invert");
      this.btnInvert.UseVisualStyleBackColor = true;
      this.btnInvert.Click += new System.EventHandler(this.btnInvert_Click);
      // 
      // btnMirrorY
      // 
      this.btnMirrorY.Image = ((System.Drawing.Image)(resources.GetObject("btnMirrorY.Image")));
      this.btnMirrorY.Location = new System.Drawing.Point(166, 269);
      this.btnMirrorY.Name = "btnMirrorY";
      this.btnMirrorY.Size = new System.Drawing.Size(26, 26);
      this.btnMirrorY.TabIndex = 43;
      this.toolTip1.SetToolTip(this.btnMirrorY, "Mirror Y");
      this.btnMirrorY.UseVisualStyleBackColor = true;
      this.btnMirrorY.Click += new System.EventHandler(this.btnMirrorY_Click);
      // 
      // btnMirrorX
      // 
      this.btnMirrorX.Image = ((System.Drawing.Image)(resources.GetObject("btnMirrorX.Image")));
      this.btnMirrorX.Location = new System.Drawing.Point(134, 269);
      this.btnMirrorX.Name = "btnMirrorX";
      this.btnMirrorX.Size = new System.Drawing.Size(26, 26);
      this.btnMirrorX.TabIndex = 47;
      this.toolTip1.SetToolTip(this.btnMirrorX, "Mirror X");
      this.btnMirrorX.UseVisualStyleBackColor = true;
      this.btnMirrorX.Click += new System.EventHandler(this.btnMirrorX_Click);
      // 
      // btnShiftDown
      // 
      this.btnShiftDown.Image = ((System.Drawing.Image)(resources.GetObject("btnShiftDown.Image")));
      this.btnShiftDown.Location = new System.Drawing.Point(102, 269);
      this.btnShiftDown.Name = "btnShiftDown";
      this.btnShiftDown.Size = new System.Drawing.Size(26, 26);
      this.btnShiftDown.TabIndex = 48;
      this.toolTip1.SetToolTip(this.btnShiftDown, "Shift Down");
      this.btnShiftDown.UseVisualStyleBackColor = true;
      this.btnShiftDown.Click += new System.EventHandler(this.btnShiftDown_Click);
      // 
      // btnShiftUp
      // 
      this.btnShiftUp.Image = ((System.Drawing.Image)(resources.GetObject("btnShiftUp.Image")));
      this.btnShiftUp.Location = new System.Drawing.Point(70, 269);
      this.btnShiftUp.Name = "btnShiftUp";
      this.btnShiftUp.Size = new System.Drawing.Size(26, 26);
      this.btnShiftUp.TabIndex = 38;
      this.toolTip1.SetToolTip(this.btnShiftUp, "Shift Up");
      this.btnShiftUp.UseVisualStyleBackColor = true;
      this.btnShiftUp.Click += new System.EventHandler(this.btnShiftUp_Click);
      // 
      // btnShiftRight
      // 
      this.btnShiftRight.Image = ((System.Drawing.Image)(resources.GetObject("btnShiftRight.Image")));
      this.btnShiftRight.Location = new System.Drawing.Point(38, 269);
      this.btnShiftRight.Name = "btnShiftRight";
      this.btnShiftRight.Size = new System.Drawing.Size(26, 26);
      this.btnShiftRight.TabIndex = 39;
      this.toolTip1.SetToolTip(this.btnShiftRight, "Shift Right");
      this.btnShiftRight.UseVisualStyleBackColor = true;
      this.btnShiftRight.Click += new System.EventHandler(this.btnShiftRight_Click);
      // 
      // button3
      // 
      this.button3.Image = ((System.Drawing.Image)(resources.GetObject("button3.Image")));
      this.button3.Location = new System.Drawing.Point(38, 301);
      this.button3.Name = "button3";
      this.button3.Size = new System.Drawing.Size(26, 26);
      this.button3.TabIndex = 42;
      this.toolTip1.SetToolTip(this.button3, "Rotate Right");
      this.button3.UseVisualStyleBackColor = true;
      this.button3.Click += new System.EventHandler(this.btnRotateRight_Click);
      // 
      // btnRotateLeft
      // 
      this.btnRotateLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnRotateLeft.Image")));
      this.btnRotateLeft.Location = new System.Drawing.Point(6, 301);
      this.btnRotateLeft.Name = "btnRotateLeft";
      this.btnRotateLeft.Size = new System.Drawing.Size(26, 26);
      this.btnRotateLeft.TabIndex = 41;
      this.toolTip1.SetToolTip(this.btnRotateLeft, "Rotate Left");
      this.btnRotateLeft.UseVisualStyleBackColor = true;
      this.btnRotateLeft.Click += new System.EventHandler(this.btnRotateLeft_Click);
      // 
      // btnShiftLeft
      // 
      this.btnShiftLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnShiftLeft.Image")));
      this.btnShiftLeft.Location = new System.Drawing.Point(6, 269);
      this.btnShiftLeft.Name = "btnShiftLeft";
      this.btnShiftLeft.Size = new System.Drawing.Size(26, 26);
      this.btnShiftLeft.TabIndex = 40;
      this.toolTip1.SetToolTip(this.btnShiftLeft, "Shift Left");
      this.btnShiftLeft.UseVisualStyleBackColor = true;
      this.btnShiftLeft.Click += new System.EventHandler(this.btnShiftLeft_Click);
      // 
      // contextMenuExchangeColors
      // 
      this.contextMenuExchangeColors.ImageScalingSize = new System.Drawing.Size(28, 28);
      this.contextMenuExchangeColors.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem,
            this.exchangeMultiColor1WithBGColorToolStripMenuItem,
            this.exchangeMultiColor2WithBGColorToolStripMenuItem});
      this.contextMenuExchangeColors.Name = "contextMenuExchangeColors";
      this.contextMenuExchangeColors.Size = new System.Drawing.Size(296, 70);
      // 
      // exchangeMultiColor1WithMultiColor2ToolStripMenuItem
      // 
      this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem.Name = "exchangeMultiColor1WithMultiColor2ToolStripMenuItem";
      this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem.Size = new System.Drawing.Size(295, 22);
      this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem.Text = "Exchange Multi Color 1 with Multi Color 2";
      this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem.Click += new System.EventHandler(this.exchangeMultiColors1And2ToolStripMenuItem_Click);
      // 
      // exchangeMultiColor1WithBGColorToolStripMenuItem
      // 
      this.exchangeMultiColor1WithBGColorToolStripMenuItem.Name = "exchangeMultiColor1WithBGColorToolStripMenuItem";
      this.exchangeMultiColor1WithBGColorToolStripMenuItem.Size = new System.Drawing.Size(295, 22);
      this.exchangeMultiColor1WithBGColorToolStripMenuItem.Text = "Exchange Multi Color 1 with BG Color";
      this.exchangeMultiColor1WithBGColorToolStripMenuItem.Click += new System.EventHandler(this.exchangeMultiColor1AndBGColorToolStripMenuItem_Click);
      // 
      // exchangeMultiColor2WithBGColorToolStripMenuItem
      // 
      this.exchangeMultiColor2WithBGColorToolStripMenuItem.Name = "exchangeMultiColor2WithBGColorToolStripMenuItem";
      this.exchangeMultiColor2WithBGColorToolStripMenuItem.Size = new System.Drawing.Size(295, 22);
      this.exchangeMultiColor2WithBGColorToolStripMenuItem.Text = "Exchange Multi Color 2 with BG Color";
      this.exchangeMultiColor2WithBGColorToolStripMenuItem.Click += new System.EventHandler(this.exchangeMultiColor2AndBGColorToolStripMenuItem_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.btnMoveSelectionToTarget);
      this.groupBox1.Controls.Add(this.editMoveTargetIndex);
      this.groupBox1.Controls.Add(this.label10);
      this.groupBox1.Location = new System.Drawing.Point(653, 301);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(249, 56);
      this.groupBox1.TabIndex = 53;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Move Selection To";
      // 
      // btnMoveSelectionToTarget
      // 
      this.btnMoveSelectionToTarget.Location = new System.Drawing.Point(161, 20);
      this.btnMoveSelectionToTarget.Name = "btnMoveSelectionToTarget";
      this.btnMoveSelectionToTarget.Size = new System.Drawing.Size(75, 21);
      this.btnMoveSelectionToTarget.TabIndex = 2;
      this.btnMoveSelectionToTarget.Text = "Move";
      this.btnMoveSelectionToTarget.UseVisualStyleBackColor = true;
      this.btnMoveSelectionToTarget.Click += new System.EventHandler(this.btnMoveSelectionToTarget_Click);
      // 
      // editMoveTargetIndex
      // 
      this.editMoveTargetIndex.Location = new System.Drawing.Point(82, 21);
      this.editMoveTargetIndex.Name = "editMoveTargetIndex";
      this.editMoveTargetIndex.Size = new System.Drawing.Size(73, 20);
      this.editMoveTargetIndex.TabIndex = 1;
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(6, 24);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(70, 13);
      this.label10.TabIndex = 0;
      this.label10.Text = "Target Index:";
      // 
      // tabCharacterEditor
      // 
      this.tabCharacterEditor.Controls.Add(this.tabEditor);
      this.tabCharacterEditor.Controls.Add(this.tabCategories);
      this.tabCharacterEditor.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabCharacterEditor.Location = new System.Drawing.Point(0, 0);
      this.tabCharacterEditor.Name = "tabCharacterEditor";
      this.tabCharacterEditor.SelectedIndex = 0;
      this.tabCharacterEditor.Size = new System.Drawing.Size(1057, 490);
      this.tabCharacterEditor.TabIndex = 55;
      // 
      // tabEditor
      // 
      this.tabEditor.Controls.Add(this.btnEditPalette);
      this.tabEditor.Controls.Add(this.canvasEditor);
      this.tabEditor.Controls.Add(this.comboCharsetMode);
      this.tabEditor.Controls.Add(this.label1);
      this.tabEditor.Controls.Add(this.comboBackground);
      this.tabEditor.Controls.Add(this.comboMulticolor1);
      this.tabEditor.Controls.Add(this.groupBox1);
      this.tabEditor.Controls.Add(this.comboMulticolor2);
      this.tabEditor.Controls.Add(this.panelCharColors);
      this.tabEditor.Controls.Add(this.comboBGColor4);
      this.tabEditor.Controls.Add(this.picturePlayground);
      this.tabEditor.Controls.Add(this.comboCharColor);
      this.tabEditor.Controls.Add(this.groupBox2);
      this.tabEditor.Controls.Add(this.radioBackground);
      this.tabEditor.Controls.Add(this.btnExchangeColors);
      this.tabEditor.Controls.Add(this.radioMultiColor1);
      this.tabEditor.Controls.Add(this.btnPaste);
      this.tabEditor.Controls.Add(this.radioMulticolor2);
      this.tabEditor.Controls.Add(this.btnCopy);
      this.tabEditor.Controls.Add(this.radioBGColor4);
      this.tabEditor.Controls.Add(this.btnInvert);
      this.tabEditor.Controls.Add(this.radioCharColor);
      this.tabEditor.Controls.Add(this.btnMirrorY);
      this.tabEditor.Controls.Add(this.checkPasteMultiColor);
      this.tabEditor.Controls.Add(this.btnMirrorX);
      this.tabEditor.Controls.Add(this.checkShowGrid);
      this.tabEditor.Controls.Add(this.btnShiftDown);
      this.tabEditor.Controls.Add(this.panelCharacters);
      this.tabEditor.Controls.Add(this.btnShiftUp);
      this.tabEditor.Controls.Add(this.labelCharNo);
      this.tabEditor.Controls.Add(this.btnShiftRight);
      this.tabEditor.Controls.Add(this.label4);
      this.tabEditor.Controls.Add(this.button3);
      this.tabEditor.Controls.Add(this.btnPasteFromClipboard);
      this.tabEditor.Controls.Add(this.btnRotateLeft);
      this.tabEditor.Controls.Add(this.comboCategories);
      this.tabEditor.Controls.Add(this.btnShiftLeft);
      this.tabEditor.Location = new System.Drawing.Point(4, 22);
      this.tabEditor.Name = "tabEditor";
      this.tabEditor.Padding = new System.Windows.Forms.Padding(3);
      this.tabEditor.Size = new System.Drawing.Size(1049, 464);
      this.tabEditor.TabIndex = 0;
      this.tabEditor.Text = "Editor";
      this.tabEditor.UseVisualStyleBackColor = true;
      // 
      // btnEditPalette
      // 
      this.btnEditPalette.Enabled = false;
      this.btnEditPalette.Location = new System.Drawing.Point(288, 143);
      this.btnEditPalette.Name = "btnEditPalette";
      this.btnEditPalette.Size = new System.Drawing.Size(75, 26);
      this.btnEditPalette.TabIndex = 55;
      this.btnEditPalette.Text = "Edit Palette";
      this.btnEditPalette.UseVisualStyleBackColor = true;
      this.btnEditPalette.Click += new System.EventHandler(this.btnEditPalette_Click);
      // 
      // canvasEditor
      // 
      this.canvasEditor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.canvasEditor.Location = new System.Drawing.Point(6, 3);
      this.canvasEditor.Name = "canvasEditor";
      this.canvasEditor.Size = new System.Drawing.Size(265, 260);
      this.canvasEditor.TabIndex = 54;
      this.canvasEditor.TabStop = false;
      this.canvasEditor.Paint += new System.Windows.Forms.PaintEventHandler(this.canvasEditor_Paint);
      this.canvasEditor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.canvasEditor_MouseDown);
      this.canvasEditor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.canvasEditor_MouseMove);
      // 
      // comboCharsetMode
      // 
      this.comboCharsetMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboCharsetMode.FormattingEnabled = true;
      this.comboCharsetMode.Location = new System.Drawing.Point(322, 322);
      this.comboCharsetMode.Name = "comboCharsetMode";
      this.comboCharsetMode.Size = new System.Drawing.Size(169, 21);
      this.comboCharsetMode.TabIndex = 16;
      this.comboCharsetMode.SelectedIndexChanged += new System.EventHandler(this.comboCharsetMode_SelectedIndexChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(279, 325);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(37, 13);
      this.label1.TabIndex = 17;
      this.label1.Text = "Mode:";
      // 
      // panelCharColors
      // 
      this.panelCharColors.AutoResize = false;
      this.panelCharColors.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panelCharColors.DisplayPage = fastImage1;
      this.panelCharColors.Image = null;
      this.panelCharColors.Location = new System.Drawing.Point(780, 271);
      this.panelCharColors.Name = "panelCharColors";
      this.panelCharColors.Size = new System.Drawing.Size(260, 20);
      this.panelCharColors.TabIndex = 52;
      this.panelCharColors.TabStop = false;
      this.panelCharColors.PostPaint += new GR.Forms.FastPictureBox.PostPaintCallback(this.panelCharColors_PostPaint);
      this.panelCharColors.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelCharColors_MouseDown);
      this.panelCharColors.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelCharColors_MouseMove);
      // 
      // picturePlayground
      // 
      this.picturePlayground.AutoResize = false;
      this.picturePlayground.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.picturePlayground.DisplayPage = fastImage2;
      this.picturePlayground.Image = null;
      this.picturePlayground.Location = new System.Drawing.Point(780, 3);
      this.picturePlayground.Name = "picturePlayground";
      this.picturePlayground.Size = new System.Drawing.Size(260, 260);
      this.picturePlayground.TabIndex = 51;
      this.picturePlayground.TabStop = false;
      this.picturePlayground.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picturePlayground_MouseDown);
      this.picturePlayground.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picturePlayground_MouseMove);
      // 
      // btnExchangeColors
      // 
      this.btnExchangeColors.Location = new System.Drawing.Point(387, 143);
      this.btnExchangeColors.Name = "btnExchangeColors";
      this.btnExchangeColors.Size = new System.Drawing.Size(121, 26);
      this.btnExchangeColors.TabIndex = 49;
      this.btnExchangeColors.Text = "Exchange Colors";
      this.btnExchangeColors.UseVisualStyleBackColor = true;
      this.btnExchangeColors.Click += new System.EventHandler(this.btnExchangeColors_Click);
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
      this.panelCharacters.Location = new System.Drawing.Point(514, 3);
      this.panelCharacters.Name = "panelCharacters";
      this.panelCharacters.PixelFormat = System.Drawing.Imaging.PixelFormat.DontCare;
      this.panelCharacters.SelectedIndex = -1;
      this.panelCharacters.Size = new System.Drawing.Size(260, 260);
      this.panelCharacters.TabIndex = 33;
      this.panelCharacters.TabStop = true;
      this.panelCharacters.VisibleAutoScrollHorizontal = false;
      this.panelCharacters.VisibleAutoScrollVertical = false;
      this.panelCharacters.SelectionChanged += new System.EventHandler(this.panelCharacters_SelectionChanged);
      // 
      // tabCategories
      // 
      this.tabCategories.Controls.Add(this.btnMoveCategoryDown);
      this.tabCategories.Controls.Add(this.btnMoveCategoryUp);
      this.tabCategories.Controls.Add(this.groupAllCategories);
      this.tabCategories.Controls.Add(this.groupCategorySpecific);
      this.tabCategories.Controls.Add(this.btnDelete);
      this.tabCategories.Controls.Add(this.btnAddCategory);
      this.tabCategories.Controls.Add(this.listCategories);
      this.tabCategories.Controls.Add(this.editCategoryName);
      this.tabCategories.Controls.Add(this.label3);
      this.tabCategories.Location = new System.Drawing.Point(4, 22);
      this.tabCategories.Name = "tabCategories";
      this.tabCategories.Padding = new System.Windows.Forms.Padding(3);
      this.tabCategories.Size = new System.Drawing.Size(1049, 464);
      this.tabCategories.TabIndex = 1;
      this.tabCategories.Text = "Categories";
      this.tabCategories.UseVisualStyleBackColor = true;
      // 
      // btnMoveCategoryDown
      // 
      this.btnMoveCategoryDown.Enabled = false;
      this.btnMoveCategoryDown.Location = new System.Drawing.Point(90, 201);
      this.btnMoveCategoryDown.Name = "btnMoveCategoryDown";
      this.btnMoveCategoryDown.Size = new System.Drawing.Size(75, 23);
      this.btnMoveCategoryDown.TabIndex = 12;
      this.btnMoveCategoryDown.Text = "Move Down";
      this.btnMoveCategoryDown.UseVisualStyleBackColor = true;
      this.btnMoveCategoryDown.Click += new System.EventHandler(this.btnMoveCategoryDown_Click);
      // 
      // btnMoveCategoryUp
      // 
      this.btnMoveCategoryUp.Enabled = false;
      this.btnMoveCategoryUp.Location = new System.Drawing.Point(9, 201);
      this.btnMoveCategoryUp.Name = "btnMoveCategoryUp";
      this.btnMoveCategoryUp.Size = new System.Drawing.Size(75, 23);
      this.btnMoveCategoryUp.TabIndex = 12;
      this.btnMoveCategoryUp.Text = "Move Up";
      this.btnMoveCategoryUp.UseVisualStyleBackColor = true;
      this.btnMoveCategoryUp.Click += new System.EventHandler(this.btnMoveCategoryUp_Click);
      // 
      // groupAllCategories
      // 
      this.groupAllCategories.Controls.Add(this.btnSortCategories);
      this.groupAllCategories.Location = new System.Drawing.Point(261, 119);
      this.groupAllCategories.Name = "groupAllCategories";
      this.groupAllCategories.Size = new System.Drawing.Size(255, 76);
      this.groupAllCategories.TabIndex = 10;
      this.groupAllCategories.TabStop = false;
      this.groupAllCategories.Text = "All Categories";
      // 
      // btnSortCategories
      // 
      this.btnSortCategories.Location = new System.Drawing.Point(6, 19);
      this.btnSortCategories.Name = "btnSortCategories";
      this.btnSortCategories.Size = new System.Drawing.Size(105, 23);
      this.btnSortCategories.TabIndex = 3;
      this.btnSortCategories.Text = "Sort by Categories";
      this.btnSortCategories.UseVisualStyleBackColor = true;
      this.btnSortCategories.Click += new System.EventHandler(this.btnSortCategories_Click);
      // 
      // groupCategorySpecific
      // 
      this.groupCategorySpecific.Controls.Add(this.label5);
      this.groupCategorySpecific.Controls.Add(this.editCollapseIndex);
      this.groupCategorySpecific.Controls.Add(this.btnCollapseCategory);
      this.groupCategorySpecific.Controls.Add(this.btnReseatCategory);
      this.groupCategorySpecific.Location = new System.Drawing.Point(261, 37);
      this.groupCategorySpecific.Name = "groupCategorySpecific";
      this.groupCategorySpecific.Size = new System.Drawing.Size(255, 76);
      this.groupCategorySpecific.TabIndex = 11;
      this.groupCategorySpecific.TabStop = false;
      this.groupCategorySpecific.Text = "Selected Category";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(117, 52);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(47, 13);
      this.label5.TabIndex = 6;
      this.label5.Text = "at index:";
      // 
      // editCollapseIndex
      // 
      this.editCollapseIndex.Location = new System.Drawing.Point(180, 49);
      this.editCollapseIndex.Name = "editCollapseIndex";
      this.editCollapseIndex.Size = new System.Drawing.Size(69, 20);
      this.editCollapseIndex.TabIndex = 5;
      // 
      // btnCollapseCategory
      // 
      this.btnCollapseCategory.Enabled = false;
      this.btnCollapseCategory.Location = new System.Drawing.Point(6, 19);
      this.btnCollapseCategory.Name = "btnCollapseCategory";
      this.btnCollapseCategory.Size = new System.Drawing.Size(140, 23);
      this.btnCollapseCategory.TabIndex = 3;
      this.btnCollapseCategory.Text = "Collapse Unique Chars";
      this.btnCollapseCategory.UseVisualStyleBackColor = true;
      this.btnCollapseCategory.Click += new System.EventHandler(this.btnCollapseCategory_Click);
      // 
      // btnReseatCategory
      // 
      this.btnReseatCategory.Enabled = false;
      this.btnReseatCategory.Location = new System.Drawing.Point(6, 47);
      this.btnReseatCategory.Name = "btnReseatCategory";
      this.btnReseatCategory.Size = new System.Drawing.Size(105, 23);
      this.btnReseatCategory.TabIndex = 3;
      this.btnReseatCategory.Text = "Reseat Category";
      this.btnReseatCategory.UseVisualStyleBackColor = true;
      this.btnReseatCategory.Click += new System.EventHandler(this.btnReseatCategory_Click);
      // 
      // btnDelete
      // 
      this.btnDelete.Enabled = false;
      this.btnDelete.Location = new System.Drawing.Point(342, 8);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new System.Drawing.Size(96, 23);
      this.btnDelete.TabIndex = 8;
      this.btnDelete.Text = "Delete Category";
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
      // 
      // btnAddCategory
      // 
      this.btnAddCategory.Enabled = false;
      this.btnAddCategory.Location = new System.Drawing.Point(261, 8);
      this.btnAddCategory.Name = "btnAddCategory";
      this.btnAddCategory.Size = new System.Drawing.Size(75, 23);
      this.btnAddCategory.TabIndex = 9;
      this.btnAddCategory.Text = "Add";
      this.btnAddCategory.UseVisualStyleBackColor = true;
      this.btnAddCategory.Click += new System.EventHandler(this.btnAddCategory_Click);
      // 
      // listCategories
      // 
      this.listCategories.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
      this.listCategories.FullRowSelect = true;
      this.listCategories.HideSelection = false;
      this.listCategories.Location = new System.Drawing.Point(9, 36);
      this.listCategories.Name = "listCategories";
      this.listCategories.ShowGroups = false;
      this.listCategories.Size = new System.Drawing.Size(246, 159);
      this.listCategories.TabIndex = 7;
      this.listCategories.UseCompatibleStateImageBehavior = false;
      this.listCategories.View = System.Windows.Forms.View.Details;
      this.listCategories.SelectedIndexChanged += new System.EventHandler(this.listCategories_SelectedIndexChanged);
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Name";
      this.columnHeader1.Width = 150;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "No. Chars";
      this.columnHeader2.Width = 67;
      // 
      // editCategoryName
      // 
      this.editCategoryName.Location = new System.Drawing.Point(81, 10);
      this.editCategoryName.Name = "editCategoryName";
      this.editCategoryName.Size = new System.Drawing.Size(174, 20);
      this.editCategoryName.TabIndex = 6;
      this.editCategoryName.TextChanged += new System.EventHandler(this.editCategoryName_TextChanged);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(6, 13);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(52, 13);
      this.label3.TabIndex = 5;
      this.label3.Text = "Category:";
      // 
      // CharacterEditor
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.Controls.Add(this.tabCharacterEditor);
      this.Name = "CharacterEditor";
      this.Size = new System.Drawing.Size(1057, 490);
      this.groupBox2.ResumeLayout(false);
      this.contextMenuExchangeColors.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.tabCharacterEditor.ResumeLayout(false);
      this.tabEditor.ResumeLayout(false);
      this.tabEditor.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.canvasEditor)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.panelCharColors)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picturePlayground)).EndInit();
      this.tabCategories.ResumeLayout(false);
      this.tabCategories.PerformLayout();
      this.groupAllCategories.ResumeLayout(false);
      this.groupCategorySpecific.ResumeLayout(false);
      this.groupCategorySpecific.PerformLayout();
      this.ResumeLayout(false);

    }

        #endregion

        private GR.Forms.FastPictureBox panelCharColors;
        private GR.Forms.FastPictureBox picturePlayground;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnClearChars;
        private MenuButton btnExchangeColors;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnInvert;
        private System.Windows.Forms.Button btnMirrorY;
        private System.Windows.Forms.Button btnMirrorX;
        private System.Windows.Forms.Button btnShiftDown;
        private System.Windows.Forms.Button btnShiftUp;
        private System.Windows.Forms.Button btnShiftRight;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnRotateLeft;
        private System.Windows.Forms.Button btnShiftLeft;
        private System.Windows.Forms.ComboBox comboCategories;
        private System.Windows.Forms.Button btnPasteFromClipboard;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelCharNo;
        private GR.Forms.ImageListbox panelCharacters;
        private System.Windows.Forms.CheckBox checkShowGrid;
        private System.Windows.Forms.CheckBox checkPasteMultiColor;
        private System.Windows.Forms.RadioButton radioCharColor;
        private System.Windows.Forms.RadioButton radioBGColor4;
        private System.Windows.Forms.RadioButton radioMulticolor2;
        private System.Windows.Forms.RadioButton radioMultiColor1;
        private System.Windows.Forms.RadioButton radioBackground;
        private System.Windows.Forms.ComboBox comboCharColor;
        private System.Windows.Forms.ComboBox comboBGColor4;
        private System.Windows.Forms.ComboBox comboMulticolor2;
        private System.Windows.Forms.ComboBox comboMulticolor1;
        private System.Windows.Forms.ComboBox comboBackground;
        private System.Windows.Forms.ContextMenuStrip contextMenuExchangeColors;
        private System.Windows.Forms.ToolStripMenuItem exchangeMultiColor1WithMultiColor2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exchangeMultiColor1WithBGColorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exchangeMultiColor2WithBGColorToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnMoveSelectionToTarget;
        private System.Windows.Forms.TextBox editMoveTargetIndex;
        private System.Windows.Forms.Label label10;
    private CustomDrawControl canvasEditor;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.TabControl tabCharacterEditor;
    private System.Windows.Forms.TabPage tabEditor;
    private System.Windows.Forms.TabPage tabCategories;
    private System.Windows.Forms.GroupBox groupAllCategories;
    private System.Windows.Forms.Button btnSortCategories;
    private System.Windows.Forms.GroupBox groupCategorySpecific;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox editCollapseIndex;
    private System.Windows.Forms.Button btnCollapseCategory;
    private System.Windows.Forms.Button btnReseatCategory;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnAddCategory;
    private System.Windows.Forms.ListView listCategories;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.TextBox editCategoryName;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Button btnMoveCategoryDown;
    private System.Windows.Forms.Button btnMoveCategoryUp;
    private System.Windows.Forms.ComboBox comboCharsetMode;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button btnEditPalette;
  }
}
