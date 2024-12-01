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
      GR.Image.FastImage fastImage4 = new GR.Image.FastImage();
      GR.Image.FastImage fastImage5 = new GR.Image.FastImage();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraphicScreenEditor));
      GR.Image.FastImage fastImage6 = new GR.Image.FastImage();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.importImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveCharsetProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.closeCharsetProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.tabEditor = new System.Windows.Forms.TabPage();
      this.comboCheckType = new System.Windows.Forms.ComboBox();
      this.btnCheck = new DecentForms.Button();
      this.labelCharInfo = new System.Windows.Forms.Label();
      this.panelColorSettings = new System.Windows.Forms.Panel();
      this.colorSelector = new GR.Forms.FastPictureBox();
      this.charEditor = new GR.Forms.FastPictureBox();
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
      this.btnApplyScreenSize = new DecentForms.Button();
      this.editScreenHeight = new System.Windows.Forms.TextBox();
      this.editScreenWidth = new System.Windows.Forms.TextBox();
      this.label7 = new System.Windows.Forms.Label();
      this.screenVScroll = new DecentForms.VScrollBar();
      this.screenHScroll = new DecentForms.HScrollBar();
      this.btnMirrorY = new DecentForms.Button();
      this.btnMirrorX = new DecentForms.Button();
      this.btnShiftDown = new DecentForms.Button();
      this.btnShiftUp = new DecentForms.Button();
      this.btnShiftRight = new DecentForms.Button();
      this.btnShiftLeft = new DecentForms.Button();
      this.btnPaste = new DecentForms.Button();
      this.btnCopy = new DecentForms.Button();
      this.btnFullCopy = new DecentForms.Button();
      this.btnPasteFromClipboard = new DecentForms.Button();
      this.labelCursorInfo = new System.Windows.Forms.Label();
      this.pictureEditor = new GR.Forms.FastPictureBox();
      this.tabGraphicScreenEditor = new System.Windows.Forms.TabControl();
      this.tabColorMapping = new System.Windows.Forms.TabPage();
      this.groupColorMapping = new System.Windows.Forms.GroupBox();
      this.listColorMappingTargets = new RetroDevStudio.Controls.ArrangedItemList();
      this.comboColorMappingTargets = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.listColorMappingColors = new System.Windows.Forms.ListBox();
      this.tabImport = new System.Windows.Forms.TabPage();
      this.panelImport = new System.Windows.Forms.Panel();
      this.btnImport = new DecentForms.Button();
      this.comboImportMethod = new System.Windows.Forms.ComboBox();
      this.label5 = new System.Windows.Forms.Label();
      this.tabExport = new System.Windows.Forms.TabPage();
      this.editExportOutput = new System.Windows.Forms.TextBox();
      this.panelExport = new System.Windows.Forms.Panel();
      this.btnExport = new DecentForms.Button();
      this.comboExportMethod = new System.Windows.Forms.ComboBox();
      this.label6 = new System.Windows.Forms.Label();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      this.menuStrip1.SuspendLayout();
      this.tabEditor.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.colorSelector)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.charEditor)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureEditor)).BeginInit();
      this.tabGraphicScreenEditor.SuspendLayout();
      this.tabColorMapping.SuspendLayout();
      this.groupColorMapping.SuspendLayout();
      this.tabImport.SuspendLayout();
      this.tabExport.SuspendLayout();
      this.SuspendLayout();
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(988, 24);
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
      // tabEditor
      // 
      this.tabEditor.Controls.Add(this.comboCheckType);
      this.tabEditor.Controls.Add(this.btnCheck);
      this.tabEditor.Controls.Add(this.labelCharInfo);
      this.tabEditor.Controls.Add(this.panelColorSettings);
      this.tabEditor.Controls.Add(this.colorSelector);
      this.tabEditor.Controls.Add(this.charEditor);
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
      this.tabEditor.Controls.Add(this.btnApplyScreenSize);
      this.tabEditor.Controls.Add(this.editScreenHeight);
      this.tabEditor.Controls.Add(this.editScreenWidth);
      this.tabEditor.Controls.Add(this.label7);
      this.tabEditor.Controls.Add(this.screenVScroll);
      this.tabEditor.Controls.Add(this.screenHScroll);
      this.tabEditor.Controls.Add(this.btnMirrorY);
      this.tabEditor.Controls.Add(this.btnMirrorX);
      this.tabEditor.Controls.Add(this.btnShiftDown);
      this.tabEditor.Controls.Add(this.btnShiftUp);
      this.tabEditor.Controls.Add(this.btnShiftRight);
      this.tabEditor.Controls.Add(this.btnShiftLeft);
      this.tabEditor.Controls.Add(this.btnPaste);
      this.tabEditor.Controls.Add(this.btnCopy);
      this.tabEditor.Controls.Add(this.btnFullCopy);
      this.tabEditor.Controls.Add(this.btnPasteFromClipboard);
      this.tabEditor.Controls.Add(this.labelCursorInfo);
      this.tabEditor.Controls.Add(this.pictureEditor);
      this.tabEditor.Location = new System.Drawing.Point(4, 22);
      this.tabEditor.Name = "tabEditor";
      this.tabEditor.Padding = new System.Windows.Forms.Padding(3);
      this.tabEditor.Size = new System.Drawing.Size(980, 534);
      this.tabEditor.TabIndex = 0;
      this.tabEditor.Text = "Screen";
      this.tabEditor.UseVisualStyleBackColor = true;
      // 
      // comboCheckType
      // 
      this.comboCheckType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboCheckType.FormattingEnabled = true;
      this.comboCheckType.Location = new System.Drawing.Point(81, 501);
      this.comboCheckType.Name = "comboCheckType";
      this.comboCheckType.Size = new System.Drawing.Size(193, 21);
      this.comboCheckType.TabIndex = 21;
      this.comboCheckType.SelectedIndexChanged += new System.EventHandler(this.comboCheckType_SelectedIndexChanged);
      // 
      // btnCheck
      // 
      this.btnCheck.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnCheck.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnCheck.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnCheck.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnCheck.Image = null;
      this.btnCheck.Location = new System.Drawing.Point(8, 499);
      this.btnCheck.Name = "btnCheck";
      this.btnCheck.Size = new System.Drawing.Size(67, 23);
      this.btnCheck.TabIndex = 6;
      this.btnCheck.Text = "Check as";
      this.btnCheck.Click += new DecentForms.EventHandler(this.btnCheck_Click);
      // 
      // labelCharInfo
      // 
      this.labelCharInfo.Location = new System.Drawing.Point(280, 504);
      this.labelCharInfo.Name = "labelCharInfo";
      this.labelCharInfo.Size = new System.Drawing.Size(372, 24);
      this.labelCharInfo.TabIndex = 5;
      this.labelCharInfo.Text = "No selected block";
      // 
      // panelColorSettings
      // 
      this.panelColorSettings.Location = new System.Drawing.Point(677, 303);
      this.panelColorSettings.Name = "panelColorSettings";
      this.panelColorSettings.Size = new System.Drawing.Size(231, 186);
      this.panelColorSettings.TabIndex = 42;
      // 
      // colorSelector
      // 
      this.colorSelector.AutoResize = false;
      this.colorSelector.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.colorSelector.DisplayPage = fastImage4;
      this.colorSelector.Image = null;
      this.colorSelector.Location = new System.Drawing.Point(677, 272);
      this.colorSelector.Name = "colorSelector";
      this.colorSelector.Size = new System.Drawing.Size(260, 19);
      this.colorSelector.TabIndex = 14;
      this.colorSelector.TabStop = false;
      this.colorSelector.SizeChanged += new System.EventHandler(this.colorSelector_SizeChanged);
      this.colorSelector.MouseDown += new System.Windows.Forms.MouseEventHandler(this.colorSelector_MouseDown);
      // 
      // charEditor
      // 
      this.charEditor.AutoResize = false;
      this.charEditor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.charEditor.DisplayPage = fastImage5;
      this.charEditor.Image = null;
      this.charEditor.Location = new System.Drawing.Point(677, 6);
      this.charEditor.Name = "charEditor";
      this.charEditor.Size = new System.Drawing.Size(260, 260);
      this.charEditor.TabIndex = 14;
      this.charEditor.TabStop = false;
      this.charEditor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.charEditor_MouseDown);
      this.charEditor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.charEditor_MouseMove);
      // 
      // btnZoomOut
      // 
      this.btnZoomOut.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnZoomOut.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnZoomOut.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnZoomOut.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnZoomOut.Enabled = false;
      this.btnZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomOut.Image")));
      this.btnZoomOut.Location = new System.Drawing.Point(244, 438);
      this.btnZoomOut.Name = "btnZoomOut";
      this.btnZoomOut.Size = new System.Drawing.Size(24, 24);
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
      this.btnZoomIn.Location = new System.Drawing.Point(220, 438);
      this.btnZoomIn.Name = "btnZoomIn";
      this.btnZoomIn.Size = new System.Drawing.Size(24, 24);
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
      this.btnClearScreen.Size = new System.Drawing.Size(24, 24);
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
      // btnApplyScreenSize
      // 
      this.btnApplyScreenSize.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnApplyScreenSize.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnApplyScreenSize.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnApplyScreenSize.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnApplyScreenSize.Image = null;
      this.btnApplyScreenSize.Location = new System.Drawing.Point(411, 469);
      this.btnApplyScreenSize.Name = "btnApplyScreenSize";
      this.btnApplyScreenSize.Size = new System.Drawing.Size(50, 24);
      this.btnApplyScreenSize.TabIndex = 34;
      this.btnApplyScreenSize.Text = "Apply";
      this.toolTip1.SetToolTip(this.btnApplyScreenSize, "Apply new size");
      this.btnApplyScreenSize.Click += new DecentForms.EventHandler(this.btnApplyScreenSize_Click);
      // 
      // editScreenHeight
      // 
      this.editScreenHeight.Location = new System.Drawing.Point(368, 473);
      this.editScreenHeight.Name = "editScreenHeight";
      this.editScreenHeight.Size = new System.Drawing.Size(37, 20);
      this.editScreenHeight.TabIndex = 33;
      this.editScreenHeight.TextChanged += new System.EventHandler(this.editScreenHeight_TextChanged);
      // 
      // editScreenWidth
      // 
      this.editScreenWidth.Location = new System.Drawing.Point(325, 473);
      this.editScreenWidth.Name = "editScreenWidth";
      this.editScreenWidth.Size = new System.Drawing.Size(37, 20);
      this.editScreenWidth.TabIndex = 32;
      this.editScreenWidth.TextChanged += new System.EventHandler(this.editScreenWidth_TextChanged);
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(289, 476);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(30, 13);
      this.label7.TabIndex = 31;
      this.label7.Text = "Size:";
      // 
      // screenVScroll
      // 
      this.screenVScroll.BorderStyle = DecentForms.BorderStyle.NONE;
      this.screenVScroll.DisplayType = DecentForms.ScrollBar.SBDisplayType.RAISED;
      this.screenVScroll.LargeChange = 10;
      this.screenVScroll.Location = new System.Drawing.Point(655, 6);
      this.screenVScroll.Maximum = 100;
      this.screenVScroll.Minimum = 0;
      this.screenVScroll.Name = "screenVScroll";
      this.screenVScroll.Size = new System.Drawing.Size(16, 404);
      this.screenVScroll.SmallChange = 1;
      this.screenVScroll.TabIndex = 26;
      this.screenVScroll.Value = 0;
      this.screenVScroll.Scroll += new DecentForms.EventHandler(this.screenVScroll_Scroll);
      // 
      // screenHScroll
      // 
      this.screenHScroll.BorderStyle = DecentForms.BorderStyle.NONE;
      this.screenHScroll.DisplayType = DecentForms.ScrollBar.SBDisplayType.RAISED;
      this.screenHScroll.LargeChange = 10;
      this.screenHScroll.Location = new System.Drawing.Point(8, 413);
      this.screenHScroll.Maximum = 100;
      this.screenHScroll.Minimum = 0;
      this.screenHScroll.Name = "screenHScroll";
      this.screenHScroll.Size = new System.Drawing.Size(644, 16);
      this.screenHScroll.SmallChange = 1;
      this.screenHScroll.TabIndex = 25;
      this.screenHScroll.Value = 0;
      this.screenHScroll.Scroll += new DecentForms.EventHandler(this.screenHScroll_Scroll);
      // 
      // btnMirrorY
      // 
      this.btnMirrorY.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnMirrorY.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnMirrorY.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnMirrorY.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnMirrorY.Image = ((System.Drawing.Image)(resources.GetObject("btnMirrorY.Image")));
      this.btnMirrorY.Location = new System.Drawing.Point(388, 438);
      this.btnMirrorY.Name = "btnMirrorY";
      this.btnMirrorY.Size = new System.Drawing.Size(24, 24);
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
      this.btnMirrorX.Location = new System.Drawing.Point(364, 438);
      this.btnMirrorX.Name = "btnMirrorX";
      this.btnMirrorX.Size = new System.Drawing.Size(24, 24);
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
      this.btnShiftDown.Location = new System.Drawing.Point(340, 438);
      this.btnShiftDown.Name = "btnShiftDown";
      this.btnShiftDown.Size = new System.Drawing.Size(24, 24);
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
      this.btnShiftUp.Location = new System.Drawing.Point(316, 438);
      this.btnShiftUp.Name = "btnShiftUp";
      this.btnShiftUp.Size = new System.Drawing.Size(24, 24);
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
      this.btnShiftRight.Location = new System.Drawing.Point(292, 438);
      this.btnShiftRight.Name = "btnShiftRight";
      this.btnShiftRight.Size = new System.Drawing.Size(24, 24);
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
      this.btnShiftLeft.Location = new System.Drawing.Point(268, 438);
      this.btnShiftLeft.Name = "btnShiftLeft";
      this.btnShiftLeft.Size = new System.Drawing.Size(24, 24);
      this.btnShiftLeft.TabIndex = 17;
      this.toolTip1.SetToolTip(this.btnShiftLeft, "Shift left");
      this.btnShiftLeft.Click += new DecentForms.EventHandler(this.btnShiftLeft_Click);
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
      this.btnPaste.Size = new System.Drawing.Size(24, 24);
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
      this.btnCopy.Size = new System.Drawing.Size(24, 24);
      this.btnCopy.TabIndex = 11;
      this.toolTip1.SetToolTip(this.btnCopy, "Copy selected 8x8 block");
      this.btnCopy.Click += new DecentForms.EventHandler(this.btnCopy_Click);
      // 
      // btnFullCopy
      // 
      this.btnFullCopy.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnFullCopy.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnFullCopy.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnFullCopy.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnFullCopy.Image = null;
      this.btnFullCopy.Location = new System.Drawing.Point(104, 469);
      this.btnFullCopy.Name = "btnFullCopy";
      this.btnFullCopy.Size = new System.Drawing.Size(80, 24);
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
      this.btnPasteFromClipboard.Location = new System.Drawing.Point(190, 469);
      this.btnPasteFromClipboard.Name = "btnPasteFromClipboard";
      this.btnPasteFromClipboard.Size = new System.Drawing.Size(84, 24);
      this.btnPasteFromClipboard.TabIndex = 6;
      this.btnPasteFromClipboard.Text = "Full Paste";
      this.toolTip1.SetToolTip(this.btnPasteFromClipboard, "Paste Full Screen");
      this.btnPasteFromClipboard.Click += new DecentForms.EventHandler(this.btnPasteFromClipboard_Click);
      // 
      // labelCursorInfo
      // 
      this.labelCursorInfo.Location = new System.Drawing.Point(418, 444);
      this.labelCursorInfo.Name = "labelCursorInfo";
      this.labelCursorInfo.Size = new System.Drawing.Size(234, 18);
      this.labelCursorInfo.TabIndex = 5;
      this.labelCursorInfo.Text = "No selected block";
      // 
      // pictureEditor
      // 
      this.pictureEditor.AutoResize = false;
      this.pictureEditor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.pictureEditor.DisplayPage = fastImage6;
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
      this.tabGraphicScreenEditor.Controls.Add(this.tabImport);
      this.tabGraphicScreenEditor.Controls.Add(this.tabExport);
      this.tabGraphicScreenEditor.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabGraphicScreenEditor.Location = new System.Drawing.Point(0, 24);
      this.tabGraphicScreenEditor.Name = "tabGraphicScreenEditor";
      this.tabGraphicScreenEditor.SelectedIndex = 0;
      this.tabGraphicScreenEditor.Size = new System.Drawing.Size(988, 560);
      this.tabGraphicScreenEditor.TabIndex = 0;
      // 
      // tabColorMapping
      // 
      this.tabColorMapping.Controls.Add(this.groupColorMapping);
      this.tabColorMapping.Location = new System.Drawing.Point(4, 22);
      this.tabColorMapping.Name = "tabColorMapping";
      this.tabColorMapping.Padding = new System.Windows.Forms.Padding(3);
      this.tabColorMapping.Size = new System.Drawing.Size(980, 534);
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
      this.groupColorMapping.Size = new System.Drawing.Size(966, 520);
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
      // tabImport
      // 
      this.tabImport.Controls.Add(this.panelImport);
      this.tabImport.Controls.Add(this.btnImport);
      this.tabImport.Controls.Add(this.comboImportMethod);
      this.tabImport.Controls.Add(this.label5);
      this.tabImport.Location = new System.Drawing.Point(4, 22);
      this.tabImport.Name = "tabImport";
      this.tabImport.Padding = new System.Windows.Forms.Padding(3);
      this.tabImport.Size = new System.Drawing.Size(980, 534);
      this.tabImport.TabIndex = 3;
      this.tabImport.Text = "Import";
      this.tabImport.UseVisualStyleBackColor = true;
      // 
      // panelImport
      // 
      this.panelImport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.panelImport.Location = new System.Drawing.Point(8, 33);
      this.panelImport.Name = "panelImport";
      this.panelImport.Size = new System.Drawing.Size(964, 493);
      this.panelImport.TabIndex = 36;
      // 
      // btnImport
      // 
      this.btnImport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnImport.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnImport.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnImport.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnImport.Image = null;
      this.btnImport.Location = new System.Drawing.Point(341, 6);
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = new System.Drawing.Size(75, 21);
      this.btnImport.TabIndex = 35;
      this.btnImport.Text = "Import";
      this.btnImport.Click += new DecentForms.EventHandler(this.btnImport_Click);
      // 
      // comboImportMethod
      // 
      this.comboImportMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboImportMethod.FormattingEnabled = true;
      this.comboImportMethod.Location = new System.Drawing.Point(92, 6);
      this.comboImportMethod.Name = "comboImportMethod";
      this.comboImportMethod.Size = new System.Drawing.Size(243, 21);
      this.comboImportMethod.TabIndex = 33;
      this.comboImportMethod.SelectedIndexChanged += new System.EventHandler(this.comboImportMethod_SelectedIndexChanged);
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(8, 9);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(78, 13);
      this.label5.TabIndex = 34;
      this.label5.Text = "Import Method:";
      // 
      // tabExport
      // 
      this.tabExport.Controls.Add(this.editExportOutput);
      this.tabExport.Controls.Add(this.panelExport);
      this.tabExport.Controls.Add(this.btnExport);
      this.tabExport.Controls.Add(this.comboExportMethod);
      this.tabExport.Controls.Add(this.label6);
      this.tabExport.Location = new System.Drawing.Point(4, 22);
      this.tabExport.Name = "tabExport";
      this.tabExport.Padding = new System.Windows.Forms.Padding(3);
      this.tabExport.Size = new System.Drawing.Size(980, 534);
      this.tabExport.TabIndex = 4;
      this.tabExport.Text = "Export";
      this.tabExport.UseVisualStyleBackColor = true;
      // 
      // editExportOutput
      // 
      this.editExportOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.editExportOutput.Location = new System.Drawing.Point(422, 6);
      this.editExportOutput.Multiline = true;
      this.editExportOutput.Name = "editExportOutput";
      this.editExportOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.editExportOutput.Size = new System.Drawing.Size(552, 521);
      this.editExportOutput.TabIndex = 41;
      this.editExportOutput.WordWrap = false;
      this.editExportOutput.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.editExportOutput_PreviewKeyDown);
      // 
      // panelExport
      // 
      this.panelExport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.panelExport.Location = new System.Drawing.Point(8, 34);
      this.panelExport.Name = "panelExport";
      this.panelExport.Size = new System.Drawing.Size(408, 493);
      this.panelExport.TabIndex = 40;
      // 
      // btnExport
      // 
      this.btnExport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnExport.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnExport.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnExport.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnExport.Image = null;
      this.btnExport.Location = new System.Drawing.Point(341, 7);
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new System.Drawing.Size(75, 21);
      this.btnExport.TabIndex = 39;
      this.btnExport.Text = "Export";
      this.btnExport.Click += new DecentForms.EventHandler(this.btnExport_Click);
      // 
      // comboExportMethod
      // 
      this.comboExportMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboExportMethod.FormattingEnabled = true;
      this.comboExportMethod.Location = new System.Drawing.Point(92, 7);
      this.comboExportMethod.Name = "comboExportMethod";
      this.comboExportMethod.Size = new System.Drawing.Size(243, 21);
      this.comboExportMethod.TabIndex = 37;
      this.comboExportMethod.SelectedIndexChanged += new System.EventHandler(this.comboExportMethod_SelectedIndexChanged);
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(8, 10);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(79, 13);
      this.label6.TabIndex = 38;
      this.label6.Text = "Export Method:";
      // 
      // GraphicScreenEditor
      // 
      this.ClientSize = new System.Drawing.Size(988, 584);
      this.Controls.Add(this.tabGraphicScreenEditor);
      this.Controls.Add(this.menuStrip1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "GraphicScreenEditor";
      this.Text = "Graphic Screen Editor";
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.tabEditor.ResumeLayout(false);
      this.tabEditor.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.colorSelector)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.charEditor)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureEditor)).EndInit();
      this.tabGraphicScreenEditor.ResumeLayout(false);
      this.tabColorMapping.ResumeLayout(false);
      this.groupColorMapping.ResumeLayout(false);
      this.groupColorMapping.PerformLayout();
      this.tabImport.ResumeLayout(false);
      this.tabImport.PerformLayout();
      this.tabExport.ResumeLayout(false);
      this.tabExport.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem importImageToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem closeCharsetProjectToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveCharsetProjectToolStripMenuItem;
    private System.Windows.Forms.TabPage tabEditor;
    private DecentForms.Button btnPaste;
    private DecentForms.Button btnCopy;
    private DecentForms.Button btnPasteFromClipboard;
    private System.Windows.Forms.Label labelCharInfo;
    private GR.Forms.FastPictureBox pictureEditor;
    private System.Windows.Forms.TabControl tabGraphicScreenEditor;
    private DecentForms.Button btnMirrorY;
    private DecentForms.Button btnMirrorX;
    private DecentForms.Button btnShiftDown;
    private DecentForms.Button btnShiftUp;
    private DecentForms.Button btnShiftRight;
    private DecentForms.Button btnShiftLeft;
    private GR.Forms.FastPictureBox charEditor;
    private DecentForms.Button btnCheck;
    private System.Windows.Forms.ComboBox comboCheckType;
    private DecentForms.HScrollBar screenHScroll;
    private DecentForms.VScrollBar screenVScroll;
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
    private GR.Forms.FastPictureBox colorSelector;
    private DecentForms.Button btnFullCopy;
    private DecentForms.RadioButton btnToolValidate;
    private DecentForms.RadioButton btnToolSelect;
    private DecentForms.RadioButton btnToolFill;
    private DecentForms.RadioButton btnToolQuad;
    private DecentForms.RadioButton btnToolRect;
    private DecentForms.RadioButton btnToolPaint;
    private System.Windows.Forms.ToolTip toolTip1;
    private DecentForms.Button btnClearScreen;
    private DecentForms.Button btnZoomIn;
    private DecentForms.Button btnZoomOut;
    private DecentForms.RadioButton btnToolLine;
    private DecentForms.RadioButton btnToolLineDrag;
    private System.Windows.Forms.Label labelCursorInfo;
    private System.Windows.Forms.Panel panelColorSettings;
    private System.Windows.Forms.TabPage tabImport;
    private DecentForms.Button btnImport;
    private System.Windows.Forms.ComboBox comboImportMethod;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Panel panelImport;
    private System.Windows.Forms.TabPage tabExport;
    private System.Windows.Forms.Panel panelExport;
    private DecentForms.Button btnExport;
    private System.Windows.Forms.ComboBox comboExportMethod;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TextBox editExportOutput;
  }
}
