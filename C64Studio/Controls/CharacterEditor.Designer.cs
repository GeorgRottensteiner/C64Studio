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
      this.comboCharsetMode = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
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
      this.canvasEditor = new C64Studio.Controls.CustomDrawControl();
      this.panelCharColors = new GR.Forms.FastPictureBox();
      this.picturePlayground = new GR.Forms.FastPictureBox();
      this.btnExchangeColors = new C64Studio.Controls.MenuButton();
      this.panelCharacters = new GR.Forms.ImageListbox();
      this.groupBox2.SuspendLayout();
      this.contextMenuExchangeColors.SuspendLayout();
      this.groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.canvasEditor)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.panelCharColors)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picturePlayground)).BeginInit();
      this.SuspendLayout();
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.btnClearChars);
      this.groupBox2.Controls.Add(this.comboCharsetMode);
      this.groupBox2.Controls.Add(this.label1);
      this.groupBox2.Location = new System.Drawing.Point(282, 301);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(292, 56);
      this.groupBox2.TabIndex = 50;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "with selected characters";
      // 
      // btnClearChars
      // 
      this.btnClearChars.Location = new System.Drawing.Point(177, 23);
      this.btnClearChars.Name = "btnClearChars";
      this.btnClearChars.Size = new System.Drawing.Size(98, 22);
      this.btnClearChars.TabIndex = 6;
      this.btnClearChars.Text = "Clear";
      this.btnClearChars.UseVisualStyleBackColor = true;
      this.btnClearChars.Click += new System.EventHandler(this.btnClear_Click);
      // 
      // comboCharsetMode
      // 
      this.comboCharsetMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboCharsetMode.FormattingEnabled = true;
      this.comboCharsetMode.Location = new System.Drawing.Point(52, 23);
      this.comboCharsetMode.Name = "comboCharsetMode";
      this.comboCharsetMode.Size = new System.Drawing.Size(109, 21);
      this.comboCharsetMode.TabIndex = 16;
      this.comboCharsetMode.SelectedIndexChanged += new System.EventHandler(this.comboCharsetMode_SelectedIndexChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(9, 26);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(37, 13);
      this.label1.TabIndex = 17;
      this.label1.Text = "Mode:";
      // 
      // comboCategories
      // 
      this.comboCategories.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboCategories.FormattingEnabled = true;
      this.comboCategories.Location = new System.Drawing.Point(389, 204);
      this.comboCategories.Name = "comboCategories";
      this.comboCategories.Size = new System.Drawing.Size(121, 21);
      this.comboCategories.TabIndex = 37;
      this.comboCategories.SelectedIndexChanged += new System.EventHandler(this.comboCategories_SelectedIndexChanged);
      // 
      // btnPasteFromClipboard
      // 
      this.btnPasteFromClipboard.Location = new System.Drawing.Point(580, 271);
      this.btnPasteFromClipboard.Name = "btnPasteFromClipboard";
      this.btnPasteFromClipboard.Size = new System.Drawing.Size(117, 23);
      this.btnPasteFromClipboard.TabIndex = 36;
      this.btnPasteFromClipboard.Text = "Big Paste";
      this.btnPasteFromClipboard.UseVisualStyleBackColor = true;
      this.btnPasteFromClipboard.Click += new System.EventHandler(this.btnPasteFromClipboard_Click);
      // 
      // label4
      // 
      this.label4.Location = new System.Drawing.Point(279, 207);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(231, 23);
      this.label4.TabIndex = 35;
      this.label4.Text = "Category:";
      // 
      // labelCharNo
      // 
      this.labelCharNo.Location = new System.Drawing.Point(279, 178);
      this.labelCharNo.Name = "labelCharNo";
      this.labelCharNo.Size = new System.Drawing.Size(231, 23);
      this.labelCharNo.TabIndex = 34;
      this.labelCharNo.Text = "label1";
      this.labelCharNo.Paint += new System.Windows.Forms.PaintEventHandler(this.labelCharNo_Paint);
      // 
      // checkShowGrid
      // 
      this.checkShowGrid.AutoSize = true;
      this.checkShowGrid.Location = new System.Drawing.Point(282, 246);
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
      this.checkPasteMultiColor.Location = new System.Drawing.Point(282, 275);
      this.checkPasteMultiColor.Name = "checkPasteMultiColor";
      this.checkPasteMultiColor.Size = new System.Drawing.Size(145, 17);
      this.checkPasteMultiColor.TabIndex = 31;
      this.checkPasteMultiColor.Text = "Force Multicolor on paste";
      this.checkPasteMultiColor.UseVisualStyleBackColor = true;
      // 
      // radioCharColor
      // 
      this.radioCharColor.AutoSize = true;
      this.radioCharColor.Location = new System.Drawing.Point(282, 111);
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
      this.radioBGColor4.Location = new System.Drawing.Point(282, 84);
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
      this.radioMulticolor2.Location = new System.Drawing.Point(282, 57);
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
      this.radioMultiColor1.Location = new System.Drawing.Point(282, 30);
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
      this.radioBackground.Location = new System.Drawing.Point(282, 3);
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
      this.comboCharColor.Location = new System.Drawing.Point(389, 111);
      this.comboCharColor.Name = "comboCharColor";
      this.comboCharColor.Size = new System.Drawing.Size(71, 21);
      this.comboCharColor.TabIndex = 24;
      this.comboCharColor.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboMulticolor_DrawItem);
      this.comboCharColor.SelectedIndexChanged += new System.EventHandler(this.comboCharColor_SelectedIndexChanged);
      // 
      // comboBGColor4
      // 
      this.comboBGColor4.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboBGColor4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBGColor4.FormattingEnabled = true;
      this.comboBGColor4.Location = new System.Drawing.Point(389, 84);
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
      this.comboMulticolor2.Location = new System.Drawing.Point(389, 57);
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
      this.comboMulticolor1.Location = new System.Drawing.Point(389, 30);
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
      this.comboBackground.Location = new System.Drawing.Point(389, 3);
      this.comboBackground.Name = "comboBackground";
      this.comboBackground.Size = new System.Drawing.Size(71, 21);
      this.comboBackground.TabIndex = 25;
      this.comboBackground.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboColor_DrawItem);
      this.comboBackground.SelectedIndexChanged += new System.EventHandler(this.comboBackground_SelectedIndexChanged);
      // 
      // btnPaste
      // 
      this.btnPaste.Image = ((System.Drawing.Image)(resources.GetObject("btnPaste.Image")));
      this.btnPaste.Location = new System.Drawing.Point(548, 269);
      this.btnPaste.Name = "btnPaste";
      this.btnPaste.Size = new System.Drawing.Size(26, 26);
      this.btnPaste.TabIndex = 46;
      this.btnPaste.UseVisualStyleBackColor = true;
      this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
      // 
      // btnCopy
      // 
      this.btnCopy.Image = ((System.Drawing.Image)(resources.GetObject("btnCopy.Image")));
      this.btnCopy.Location = new System.Drawing.Point(516, 269);
      this.btnCopy.Name = "btnCopy";
      this.btnCopy.Size = new System.Drawing.Size(26, 26);
      this.btnCopy.TabIndex = 45;
      this.btnCopy.UseVisualStyleBackColor = true;
      this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
      // 
      // btnInvert
      // 
      this.btnInvert.Image = ((System.Drawing.Image)(resources.GetObject("btnInvert.Image")));
      this.btnInvert.Location = new System.Drawing.Point(200, 269);
      this.btnInvert.Name = "btnInvert";
      this.btnInvert.Size = new System.Drawing.Size(26, 26);
      this.btnInvert.TabIndex = 44;
      this.btnInvert.UseVisualStyleBackColor = true;
      this.btnInvert.Click += new System.EventHandler(this.btnInvert_Click);
      // 
      // btnMirrorY
      // 
      this.btnMirrorY.Image = ((System.Drawing.Image)(resources.GetObject("btnMirrorY.Image")));
      this.btnMirrorY.Location = new System.Drawing.Point(168, 269);
      this.btnMirrorY.Name = "btnMirrorY";
      this.btnMirrorY.Size = new System.Drawing.Size(26, 26);
      this.btnMirrorY.TabIndex = 43;
      this.btnMirrorY.UseVisualStyleBackColor = true;
      this.btnMirrorY.Click += new System.EventHandler(this.btnMirrorY_Click);
      // 
      // btnMirrorX
      // 
      this.btnMirrorX.Image = ((System.Drawing.Image)(resources.GetObject("btnMirrorX.Image")));
      this.btnMirrorX.Location = new System.Drawing.Point(136, 269);
      this.btnMirrorX.Name = "btnMirrorX";
      this.btnMirrorX.Size = new System.Drawing.Size(26, 26);
      this.btnMirrorX.TabIndex = 47;
      this.btnMirrorX.UseVisualStyleBackColor = true;
      this.btnMirrorX.Click += new System.EventHandler(this.btnMirrorX_Click);
      // 
      // btnShiftDown
      // 
      this.btnShiftDown.Image = ((System.Drawing.Image)(resources.GetObject("btnShiftDown.Image")));
      this.btnShiftDown.Location = new System.Drawing.Point(104, 269);
      this.btnShiftDown.Name = "btnShiftDown";
      this.btnShiftDown.Size = new System.Drawing.Size(26, 26);
      this.btnShiftDown.TabIndex = 48;
      this.btnShiftDown.UseVisualStyleBackColor = true;
      this.btnShiftDown.Click += new System.EventHandler(this.btnShiftDown_Click);
      // 
      // btnShiftUp
      // 
      this.btnShiftUp.Image = ((System.Drawing.Image)(resources.GetObject("btnShiftUp.Image")));
      this.btnShiftUp.Location = new System.Drawing.Point(72, 269);
      this.btnShiftUp.Name = "btnShiftUp";
      this.btnShiftUp.Size = new System.Drawing.Size(26, 26);
      this.btnShiftUp.TabIndex = 38;
      this.btnShiftUp.UseVisualStyleBackColor = true;
      this.btnShiftUp.Click += new System.EventHandler(this.btnShiftUp_Click);
      // 
      // btnShiftRight
      // 
      this.btnShiftRight.Image = ((System.Drawing.Image)(resources.GetObject("btnShiftRight.Image")));
      this.btnShiftRight.Location = new System.Drawing.Point(40, 269);
      this.btnShiftRight.Name = "btnShiftRight";
      this.btnShiftRight.Size = new System.Drawing.Size(26, 26);
      this.btnShiftRight.TabIndex = 39;
      this.btnShiftRight.UseVisualStyleBackColor = true;
      this.btnShiftRight.Click += new System.EventHandler(this.btnShiftRight_Click);
      // 
      // button3
      // 
      this.button3.Image = ((System.Drawing.Image)(resources.GetObject("button3.Image")));
      this.button3.Location = new System.Drawing.Point(40, 301);
      this.button3.Name = "button3";
      this.button3.Size = new System.Drawing.Size(26, 26);
      this.button3.TabIndex = 42;
      this.button3.UseVisualStyleBackColor = true;
      this.button3.Click += new System.EventHandler(this.btnRotateRight_Click);
      // 
      // btnRotateLeft
      // 
      this.btnRotateLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnRotateLeft.Image")));
      this.btnRotateLeft.Location = new System.Drawing.Point(8, 301);
      this.btnRotateLeft.Name = "btnRotateLeft";
      this.btnRotateLeft.Size = new System.Drawing.Size(26, 26);
      this.btnRotateLeft.TabIndex = 41;
      this.btnRotateLeft.UseVisualStyleBackColor = true;
      this.btnRotateLeft.Click += new System.EventHandler(this.btnRotateLeft_Click);
      // 
      // btnShiftLeft
      // 
      this.btnShiftLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnShiftLeft.Image")));
      this.btnShiftLeft.Location = new System.Drawing.Point(8, 269);
      this.btnShiftLeft.Name = "btnShiftLeft";
      this.btnShiftLeft.Size = new System.Drawing.Size(26, 26);
      this.btnShiftLeft.TabIndex = 40;
      this.btnShiftLeft.UseVisualStyleBackColor = true;
      this.btnShiftLeft.Click += new System.EventHandler(this.btnShiftLeft_Click);
      // 
      // contextMenuExchangeColors
      // 
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
      this.groupBox1.Location = new System.Drawing.Point(580, 301);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(249, 56);
      this.groupBox1.TabIndex = 53;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Move Selection To";
      // 
      // btnMoveSelectionToTarget
      // 
      this.btnMoveSelectionToTarget.Location = new System.Drawing.Point(161, 23);
      this.btnMoveSelectionToTarget.Name = "btnMoveSelectionToTarget";
      this.btnMoveSelectionToTarget.Size = new System.Drawing.Size(75, 21);
      this.btnMoveSelectionToTarget.TabIndex = 2;
      this.btnMoveSelectionToTarget.Text = "Move";
      this.btnMoveSelectionToTarget.UseVisualStyleBackColor = true;
      this.btnMoveSelectionToTarget.Click += new System.EventHandler(this.btnMoveSelectionToTarget_Click);
      // 
      // editMoveTargetIndex
      // 
      this.editMoveTargetIndex.Location = new System.Drawing.Point(82, 23);
      this.editMoveTargetIndex.Name = "editMoveTargetIndex";
      this.editMoveTargetIndex.Size = new System.Drawing.Size(73, 20);
      this.editMoveTargetIndex.TabIndex = 1;
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(6, 26);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(70, 13);
      this.label10.TabIndex = 0;
      this.label10.Text = "Target Index:";
      // 
      // canvasEditor
      // 
      this.canvasEditor.Location = new System.Drawing.Point(8, 3);
      this.canvasEditor.Name = "canvasEditor";
      this.canvasEditor.Size = new System.Drawing.Size(265, 260);
      this.canvasEditor.TabIndex = 54;
      this.canvasEditor.TabStop = false;
      this.canvasEditor.Paint += new System.Windows.Forms.PaintEventHandler(this.canvasEditor_Paint);
      this.canvasEditor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.canvasEditor_MouseDown);
      this.canvasEditor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.canvasEditor_MouseMove);
      // 
      // panelCharColors
      // 
      this.panelCharColors.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panelCharColors.DisplayPage = fastImage1;
      this.panelCharColors.Image = null;
      this.panelCharColors.Location = new System.Drawing.Point(782, 271);
      this.panelCharColors.Name = "panelCharColors";
      this.panelCharColors.Size = new System.Drawing.Size(260, 20);
      this.panelCharColors.TabIndex = 52;
      this.panelCharColors.TabStop = false;
      this.panelCharColors.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelCharColors_MouseDown);
      this.panelCharColors.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelCharColors_MouseMove);
      // 
      // picturePlayground
      // 
      this.picturePlayground.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.picturePlayground.DisplayPage = fastImage2;
      this.picturePlayground.Image = null;
      this.picturePlayground.Location = new System.Drawing.Point(782, 3);
      this.picturePlayground.Name = "picturePlayground";
      this.picturePlayground.Size = new System.Drawing.Size(260, 260);
      this.picturePlayground.TabIndex = 51;
      this.picturePlayground.TabStop = false;
      this.picturePlayground.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picturePlayground_MouseDown);
      this.picturePlayground.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picturePlayground_MouseMove);
      // 
      // btnExchangeColors
      // 
      this.btnExchangeColors.Location = new System.Drawing.Point(389, 143);
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
      this.panelCharacters.Location = new System.Drawing.Point(516, 3);
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
      // CharacterEditor
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.canvasEditor);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.panelCharColors);
      this.Controls.Add(this.picturePlayground);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.btnExchangeColors);
      this.Controls.Add(this.btnPaste);
      this.Controls.Add(this.btnCopy);
      this.Controls.Add(this.btnInvert);
      this.Controls.Add(this.btnMirrorY);
      this.Controls.Add(this.btnMirrorX);
      this.Controls.Add(this.btnShiftDown);
      this.Controls.Add(this.btnShiftUp);
      this.Controls.Add(this.btnShiftRight);
      this.Controls.Add(this.button3);
      this.Controls.Add(this.btnRotateLeft);
      this.Controls.Add(this.btnShiftLeft);
      this.Controls.Add(this.comboCategories);
      this.Controls.Add(this.btnPasteFromClipboard);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.labelCharNo);
      this.Controls.Add(this.panelCharacters);
      this.Controls.Add(this.checkShowGrid);
      this.Controls.Add(this.checkPasteMultiColor);
      this.Controls.Add(this.radioCharColor);
      this.Controls.Add(this.radioBGColor4);
      this.Controls.Add(this.radioMulticolor2);
      this.Controls.Add(this.radioMultiColor1);
      this.Controls.Add(this.radioBackground);
      this.Controls.Add(this.comboCharColor);
      this.Controls.Add(this.comboBGColor4);
      this.Controls.Add(this.comboMulticolor2);
      this.Controls.Add(this.comboMulticolor1);
      this.Controls.Add(this.comboBackground);
      this.Name = "CharacterEditor";
      this.Size = new System.Drawing.Size(1057, 490);
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.contextMenuExchangeColors.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.canvasEditor)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.panelCharColors)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picturePlayground)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

        #endregion

        private GR.Forms.FastPictureBox panelCharColors;
        private GR.Forms.FastPictureBox picturePlayground;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnClearChars;
        private System.Windows.Forms.ComboBox comboCharsetMode;
        private System.Windows.Forms.Label label1;
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
  }
}
