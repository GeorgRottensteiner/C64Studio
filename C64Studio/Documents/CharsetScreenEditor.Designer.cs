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
      this.btnExport = new System.Windows.Forms.Button();
      this.label12 = new System.Windows.Forms.Label();
      this.comboExportOrientation = new System.Windows.Forms.ComboBox();
      this.panelExport = new System.Windows.Forms.Panel();
      this.editDataExport = new System.Windows.Forms.TextBox();
      this.comboExportMethod = new System.Windows.Forms.ComboBox();
      this.comboExportArea = new System.Windows.Forms.ComboBox();
      this.editAreaHeight = new System.Windows.Forms.TextBox();
      this.labelAreaHeight = new System.Windows.Forms.Label();
      this.editExportY = new System.Windows.Forms.TextBox();
      this.editAreaWidth = new System.Windows.Forms.TextBox();
      this.labelAreaY = new System.Windows.Forms.Label();
      this.labelAreaWidth = new System.Windows.Forms.Label();
      this.editExportX = new System.Windows.Forms.TextBox();
      this.labelAreaX = new System.Windows.Forms.Label();
      this.label11 = new System.Windows.Forms.Label();
      this.label8 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.comboExportData = new System.Windows.Forms.ComboBox();
      this.tabEditor = new System.Windows.Forms.TabPage();
      this.btnPaste = new System.Windows.Forms.Button();
      this.btnCopy = new System.Windows.Forms.Button();
      this.editCharOffset = new System.Windows.Forms.TextBox();
      this.btnShiftDown = new System.Windows.Forms.Button();
      this.btnShiftUp = new System.Windows.Forms.Button();
      this.btnShiftRight = new System.Windows.Forms.Button();
      this.btnShiftLeft = new System.Windows.Forms.Button();
      this.btnClearScreen = new System.Windows.Forms.Button();
      this.checkOverrideOriginalColorSettings = new System.Windows.Forms.CheckBox();
      this.checkShowGrid = new System.Windows.Forms.CheckBox();
      this.checkReverse = new System.Windows.Forms.CheckBox();
      this.checkAutoCenter = new System.Windows.Forms.CheckBox();
      this.checkApplyColors = new System.Windows.Forms.CheckBox();
      this.checkApplyCharacter = new System.Windows.Forms.CheckBox();
      this.label10 = new System.Windows.Forms.Label();
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
      this.charEditor = new C64Studio.Controls.CharacterEditor();
      this.tabImport = new System.Windows.Forms.TabPage();
      this.panelImport = new System.Windows.Forms.Panel();
      this.btnImport = new System.Windows.Forms.Button();
      this.comboImportMethod = new System.Windows.Forms.ComboBox();
      this.label2 = new System.Windows.Forms.Label();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      this.menuStrip1.SuspendLayout();
      this.tabProject.SuspendLayout();
      this.tabEditor.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.panelCharColors)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureEditor)).BeginInit();
      this.tabCharsetEditor.SuspendLayout();
      this.tabCharset.SuspendLayout();
      this.tabImport.SuspendLayout();
      this.SuspendLayout();
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(1069, 24);
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
      this.tabProject.Controls.Add(this.btnExport);
      this.tabProject.Controls.Add(this.label12);
      this.tabProject.Controls.Add(this.comboExportOrientation);
      this.tabProject.Controls.Add(this.panelExport);
      this.tabProject.Controls.Add(this.editDataExport);
      this.tabProject.Controls.Add(this.comboExportMethod);
      this.tabProject.Controls.Add(this.comboExportArea);
      this.tabProject.Controls.Add(this.editAreaHeight);
      this.tabProject.Controls.Add(this.labelAreaHeight);
      this.tabProject.Controls.Add(this.editExportY);
      this.tabProject.Controls.Add(this.editAreaWidth);
      this.tabProject.Controls.Add(this.labelAreaY);
      this.tabProject.Controls.Add(this.labelAreaWidth);
      this.tabProject.Controls.Add(this.editExportX);
      this.tabProject.Controls.Add(this.labelAreaX);
      this.tabProject.Controls.Add(this.label11);
      this.tabProject.Controls.Add(this.label8);
      this.tabProject.Controls.Add(this.label6);
      this.tabProject.Controls.Add(this.comboExportData);
      this.tabProject.Location = new System.Drawing.Point(4, 22);
      this.tabProject.Name = "tabProject";
      this.tabProject.Padding = new System.Windows.Forms.Padding(3);
      this.tabProject.Size = new System.Drawing.Size(1061, 546);
      this.tabProject.TabIndex = 1;
      this.tabProject.Text = "Export";
      this.tabProject.UseVisualStyleBackColor = true;
      // 
      // btnExport
      // 
      this.btnExport.Location = new System.Drawing.Point(370, 118);
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new System.Drawing.Size(75, 21);
      this.btnExport.TabIndex = 29;
      this.btnExport.Text = "Export";
      this.btnExport.UseVisualStyleBackColor = true;
      this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
      // 
      // label12
      // 
      this.label12.AutoSize = true;
      this.label12.Location = new System.Drawing.Point(8, 91);
      this.label12.Name = "label12";
      this.label12.Size = new System.Drawing.Size(61, 13);
      this.label12.TabIndex = 27;
      this.label12.Text = "Orientation:";
      // 
      // comboExportOrientation
      // 
      this.comboExportOrientation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboExportOrientation.FormattingEnabled = true;
      this.comboExportOrientation.Items.AddRange(new object[] {
            "row by row",
            "column by column"});
      this.comboExportOrientation.Location = new System.Drawing.Point(90, 88);
      this.comboExportOrientation.Name = "comboExportOrientation";
      this.comboExportOrientation.Size = new System.Drawing.Size(131, 21);
      this.comboExportOrientation.TabIndex = 28;
      // 
      // panelExport
      // 
      this.panelExport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.panelExport.Location = new System.Drawing.Point(6, 145);
      this.panelExport.Name = "panelExport";
      this.panelExport.Size = new System.Drawing.Size(439, 393);
      this.panelExport.TabIndex = 26;
      // 
      // editDataExport
      // 
      this.editDataExport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.editDataExport.Location = new System.Drawing.Point(451, 6);
      this.editDataExport.Multiline = true;
      this.editDataExport.Name = "editDataExport";
      this.editDataExport.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.editDataExport.Size = new System.Drawing.Size(604, 532);
      this.editDataExport.TabIndex = 24;
      this.editDataExport.WordWrap = false;
      this.editDataExport.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editDataExport_KeyPress);
      this.editDataExport.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.editDataExport_PreviewKeyDown);
      // 
      // comboExportMethod
      // 
      this.comboExportMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboExportMethod.FormattingEnabled = true;
      this.comboExportMethod.Location = new System.Drawing.Point(90, 118);
      this.comboExportMethod.Name = "comboExportMethod";
      this.comboExportMethod.Size = new System.Drawing.Size(243, 21);
      this.comboExportMethod.TabIndex = 1;
      this.comboExportMethod.SelectedIndexChanged += new System.EventHandler(this.comboExportMethod_SelectedIndexChanged);
      // 
      // comboExportArea
      // 
      this.comboExportArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboExportArea.FormattingEnabled = true;
      this.comboExportArea.Location = new System.Drawing.Point(90, 33);
      this.comboExportArea.Name = "comboExportArea";
      this.comboExportArea.Size = new System.Drawing.Size(350, 21);
      this.comboExportArea.TabIndex = 1;
      this.comboExportArea.SelectedIndexChanged += new System.EventHandler(this.comboExportArea_SelectedIndexChanged);
      // 
      // editAreaHeight
      // 
      this.editAreaHeight.Location = new System.Drawing.Point(386, 62);
      this.editAreaHeight.Name = "editAreaHeight";
      this.editAreaHeight.Size = new System.Drawing.Size(54, 20);
      this.editAreaHeight.TabIndex = 5;
      // 
      // labelAreaHeight
      // 
      this.labelAreaHeight.AutoSize = true;
      this.labelAreaHeight.Location = new System.Drawing.Point(339, 65);
      this.labelAreaHeight.Name = "labelAreaHeight";
      this.labelAreaHeight.Size = new System.Drawing.Size(41, 13);
      this.labelAreaHeight.TabIndex = 7;
      this.labelAreaHeight.Text = "Height:";
      // 
      // editExportY
      // 
      this.editExportY.Location = new System.Drawing.Point(175, 62);
      this.editExportY.Name = "editExportY";
      this.editExportY.Size = new System.Drawing.Size(54, 20);
      this.editExportY.TabIndex = 3;
      // 
      // editAreaWidth
      // 
      this.editAreaWidth.Location = new System.Drawing.Point(279, 62);
      this.editAreaWidth.Name = "editAreaWidth";
      this.editAreaWidth.Size = new System.Drawing.Size(54, 20);
      this.editAreaWidth.TabIndex = 4;
      // 
      // labelAreaY
      // 
      this.labelAreaY.AutoSize = true;
      this.labelAreaY.Location = new System.Drawing.Point(152, 65);
      this.labelAreaY.Name = "labelAreaY";
      this.labelAreaY.Size = new System.Drawing.Size(17, 13);
      this.labelAreaY.TabIndex = 5;
      this.labelAreaY.Text = "Y:";
      // 
      // labelAreaWidth
      // 
      this.labelAreaWidth.AutoSize = true;
      this.labelAreaWidth.Location = new System.Drawing.Point(235, 65);
      this.labelAreaWidth.Name = "labelAreaWidth";
      this.labelAreaWidth.Size = new System.Drawing.Size(38, 13);
      this.labelAreaWidth.TabIndex = 6;
      this.labelAreaWidth.Text = "Width:";
      // 
      // editExportX
      // 
      this.editExportX.Location = new System.Drawing.Point(90, 62);
      this.editExportX.Name = "editExportX";
      this.editExportX.Size = new System.Drawing.Size(54, 20);
      this.editExportX.TabIndex = 2;
      // 
      // labelAreaX
      // 
      this.labelAreaX.AutoSize = true;
      this.labelAreaX.Location = new System.Drawing.Point(67, 65);
      this.labelAreaX.Name = "labelAreaX";
      this.labelAreaX.Size = new System.Drawing.Size(17, 13);
      this.labelAreaX.TabIndex = 4;
      this.labelAreaX.Text = "X:";
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Location = new System.Drawing.Point(8, 121);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(79, 13);
      this.label11.TabIndex = 2;
      this.label11.Text = "Export Method:";
      this.label11.Click += new System.EventHandler(this.label11_Click);
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(8, 36);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(65, 13);
      this.label8.TabIndex = 2;
      this.label8.Text = "Export Area:";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(8, 9);
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
      this.comboExportData.Location = new System.Drawing.Point(90, 6);
      this.comboExportData.Name = "comboExportData";
      this.comboExportData.Size = new System.Drawing.Size(350, 21);
      this.comboExportData.TabIndex = 0;
      // 
      // tabEditor
      // 
      this.tabEditor.Controls.Add(this.btnPaste);
      this.tabEditor.Controls.Add(this.btnCopy);
      this.tabEditor.Controls.Add(this.editCharOffset);
      this.tabEditor.Controls.Add(this.btnShiftDown);
      this.tabEditor.Controls.Add(this.btnShiftUp);
      this.tabEditor.Controls.Add(this.btnShiftRight);
      this.tabEditor.Controls.Add(this.btnShiftLeft);
      this.tabEditor.Controls.Add(this.btnClearScreen);
      this.tabEditor.Controls.Add(this.checkOverrideOriginalColorSettings);
      this.tabEditor.Controls.Add(this.checkShowGrid);
      this.tabEditor.Controls.Add(this.checkReverse);
      this.tabEditor.Controls.Add(this.checkAutoCenter);
      this.tabEditor.Controls.Add(this.checkApplyColors);
      this.tabEditor.Controls.Add(this.checkApplyCharacter);
      this.tabEditor.Controls.Add(this.label10);
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
      this.tabEditor.Size = new System.Drawing.Size(1061, 546);
      this.tabEditor.TabIndex = 0;
      this.tabEditor.Text = "Screen";
      this.tabEditor.UseVisualStyleBackColor = true;
      // 
      // btnPaste
      // 
      this.btnPaste.Enabled = false;
      this.btnPaste.Image = ((System.Drawing.Image)(resources.GetObject("btnPaste.Image")));
      this.btnPaste.Location = new System.Drawing.Point(198, 460);
      this.btnPaste.Name = "btnPaste";
      this.btnPaste.Size = new System.Drawing.Size(24, 24);
      this.btnPaste.TabIndex = 48;
      this.toolTip1.SetToolTip(this.btnPaste, "Paste Characters");
      this.btnPaste.UseVisualStyleBackColor = true;
      this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
      // 
      // btnCopy
      // 
      this.btnCopy.Enabled = false;
      this.btnCopy.Image = ((System.Drawing.Image)(resources.GetObject("btnCopy.Image")));
      this.btnCopy.Location = new System.Drawing.Point(168, 460);
      this.btnCopy.Name = "btnCopy";
      this.btnCopy.Size = new System.Drawing.Size(24, 24);
      this.btnCopy.TabIndex = 47;
      this.toolTip1.SetToolTip(this.btnCopy, "Copy Selected Characters to Clipboard");
      this.btnCopy.UseVisualStyleBackColor = true;
      this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
      // 
      // editCharOffset
      // 
      this.editCharOffset.Location = new System.Drawing.Point(897, 35);
      this.editCharOffset.Name = "editCharOffset";
      this.editCharOffset.Size = new System.Drawing.Size(70, 20);
      this.editCharOffset.TabIndex = 18;
      this.editCharOffset.TextChanged += new System.EventHandler(this.editCharOffset_TextChanged);
      // 
      // btnShiftDown
      // 
      this.btnShiftDown.Image = ((System.Drawing.Image)(resources.GetObject("btnShiftDown.Image")));
      this.btnShiftDown.Location = new System.Drawing.Point(104, 460);
      this.btnShiftDown.Name = "btnShiftDown";
      this.btnShiftDown.Size = new System.Drawing.Size(24, 23);
      this.btnShiftDown.TabIndex = 14;
      this.toolTip1.SetToolTip(this.btnShiftDown, "Shift Down");
      this.btnShiftDown.UseVisualStyleBackColor = true;
      this.btnShiftDown.Click += new System.EventHandler(this.btnShiftDown_Click);
      // 
      // btnShiftUp
      // 
      this.btnShiftUp.Image = ((System.Drawing.Image)(resources.GetObject("btnShiftUp.Image")));
      this.btnShiftUp.Location = new System.Drawing.Point(80, 460);
      this.btnShiftUp.Name = "btnShiftUp";
      this.btnShiftUp.Size = new System.Drawing.Size(24, 23);
      this.btnShiftUp.TabIndex = 13;
      this.toolTip1.SetToolTip(this.btnShiftUp, "Shift Up");
      this.btnShiftUp.UseVisualStyleBackColor = true;
      this.btnShiftUp.Click += new System.EventHandler(this.btnShiftUp_Click);
      // 
      // btnShiftRight
      // 
      this.btnShiftRight.Image = ((System.Drawing.Image)(resources.GetObject("btnShiftRight.Image")));
      this.btnShiftRight.Location = new System.Drawing.Point(56, 460);
      this.btnShiftRight.Name = "btnShiftRight";
      this.btnShiftRight.Size = new System.Drawing.Size(24, 23);
      this.btnShiftRight.TabIndex = 12;
      this.toolTip1.SetToolTip(this.btnShiftRight, "Shift Right");
      this.btnShiftRight.UseVisualStyleBackColor = true;
      this.btnShiftRight.Click += new System.EventHandler(this.btnShiftRight_Click);
      // 
      // btnShiftLeft
      // 
      this.btnShiftLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnShiftLeft.Image")));
      this.btnShiftLeft.Location = new System.Drawing.Point(32, 460);
      this.btnShiftLeft.Name = "btnShiftLeft";
      this.btnShiftLeft.Size = new System.Drawing.Size(24, 23);
      this.btnShiftLeft.TabIndex = 11;
      this.toolTip1.SetToolTip(this.btnShiftLeft, "Shift Left");
      this.btnShiftLeft.UseVisualStyleBackColor = true;
      this.btnShiftLeft.Click += new System.EventHandler(this.btnShiftLeft_Click);
      // 
      // btnClearScreen
      // 
      this.btnClearScreen.Image = ((System.Drawing.Image)(resources.GetObject("btnClearScreen.Image")));
      this.btnClearScreen.Location = new System.Drawing.Point(8, 460);
      this.btnClearScreen.Name = "btnClearScreen";
      this.btnClearScreen.Size = new System.Drawing.Size(24, 23);
      this.btnClearScreen.TabIndex = 10;
      this.toolTip1.SetToolTip(this.btnClearScreen, "Clear Screen (set to spaces)");
      this.btnClearScreen.UseVisualStyleBackColor = true;
      this.btnClearScreen.Click += new System.EventHandler(this.btnClearScreen_Click);
      // 
      // checkOverrideOriginalColorSettings
      // 
      this.checkOverrideOriginalColorSettings.AutoSize = true;
      this.checkOverrideOriginalColorSettings.Location = new System.Drawing.Point(751, 37);
      this.checkOverrideOriginalColorSettings.Name = "checkOverrideOriginalColorSettings";
      this.checkOverrideOriginalColorSettings.Size = new System.Drawing.Size(96, 17);
      this.checkOverrideOriginalColorSettings.TabIndex = 17;
      this.checkOverrideOriginalColorSettings.Text = "Override Mode";
      this.checkOverrideOriginalColorSettings.UseVisualStyleBackColor = true;
      this.checkOverrideOriginalColorSettings.CheckedChanged += new System.EventHandler(this.checkOverrideMode_CheckedChanged);
      // 
      // checkShowGrid
      // 
      this.checkShowGrid.AutoSize = true;
      this.checkShowGrid.Location = new System.Drawing.Point(258, 462);
      this.checkShowGrid.Name = "checkShowGrid";
      this.checkShowGrid.Size = new System.Drawing.Size(75, 17);
      this.checkShowGrid.TabIndex = 15;
      this.checkShowGrid.Text = "Show Grid";
      this.checkShowGrid.UseVisualStyleBackColor = true;
      this.checkShowGrid.CheckedChanged += new System.EventHandler(this.checkShowGrid_CheckedChanged);
      // 
      // checkReverse
      // 
      this.checkReverse.Appearance = System.Windows.Forms.Appearance.Button;
      this.checkReverse.Image = global::C64Studio.Properties.Resources.charscreen_reverse_off;
      this.checkReverse.Location = new System.Drawing.Point(228, 432);
      this.checkReverse.Name = "checkReverse";
      this.checkReverse.Size = new System.Drawing.Size(24, 24);
      this.checkReverse.TabIndex = 8;
      this.toolTip1.SetToolTip(this.checkReverse, "Reverse Characters");
      this.checkReverse.UseVisualStyleBackColor = true;
      this.checkReverse.CheckedChanged += new System.EventHandler(this.checkReverse_CheckedChanged);
      // 
      // checkAutoCenter
      // 
      this.checkAutoCenter.Appearance = System.Windows.Forms.Appearance.Button;
      this.checkAutoCenter.Image = ((System.Drawing.Image)(resources.GetObject("checkAutoCenter.Image")));
      this.checkAutoCenter.Location = new System.Drawing.Point(258, 432);
      this.checkAutoCenter.Name = "checkAutoCenter";
      this.checkAutoCenter.Size = new System.Drawing.Size(24, 24);
      this.checkAutoCenter.TabIndex = 9;
      this.toolTip1.SetToolTip(this.checkAutoCenter, "Automatically center text on entry");
      this.checkAutoCenter.UseVisualStyleBackColor = true;
      this.checkAutoCenter.CheckedChanged += new System.EventHandler(this.checkAutoCenterText_CheckedChanged);
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
      this.checkApplyColors.TabIndex = 7;
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
      this.checkApplyCharacter.TabIndex = 6;
      this.toolTip1.SetToolTip(this.checkApplyCharacter, "Affect Characters");
      this.checkApplyCharacter.UseVisualStyleBackColor = true;
      this.checkApplyCharacter.CheckedChanged += new System.EventHandler(this.checkApplyCharacter_CheckedChanged);
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(853, 38);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(38, 13);
      this.label10.TabIndex = 35;
      this.label10.Text = "Offset:";
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(677, 11);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(37, 13);
      this.label9.TabIndex = 35;
      this.label9.Text = "Mode:";
      // 
      // comboCharsetMode
      // 
      this.comboCharsetMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboCharsetMode.FormattingEnabled = true;
      this.comboCharsetMode.Location = new System.Drawing.Point(751, 8);
      this.comboCharsetMode.Name = "comboCharsetMode";
      this.comboCharsetMode.Size = new System.Drawing.Size(216, 21);
      this.comboCharsetMode.TabIndex = 16;
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
      this.btnToolText.TabIndex = 5;
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
      this.btnToolSelect.TabIndex = 4;
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
      this.btnToolFill.TabIndex = 3;
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
      this.btnToolQuad.TabIndex = 2;
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
      this.btnToolRect.TabIndex = 1;
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
      this.btnToolEdit.TabIndex = 0;
      this.btnToolEdit.TabStop = true;
      this.toolTip1.SetToolTip(this.btnToolEdit, "Single Character");
      this.btnToolEdit.UseVisualStyleBackColor = true;
      this.btnToolEdit.CheckedChanged += new System.EventHandler(this.btnToolEdit_CheckedChanged);
      // 
      // btnApplyScreenSize
      // 
      this.btnApplyScreenSize.Location = new System.Drawing.Point(897, 114);
      this.btnApplyScreenSize.Name = "btnApplyScreenSize";
      this.btnApplyScreenSize.Size = new System.Drawing.Size(67, 20);
      this.btnApplyScreenSize.TabIndex = 25;
      this.btnApplyScreenSize.Text = "Apply";
      this.btnApplyScreenSize.UseVisualStyleBackColor = true;
      this.btnApplyScreenSize.Click += new System.EventHandler(this.btnApplyScreenSize_Click);
      // 
      // editScreenHeight
      // 
      this.editScreenHeight.Location = new System.Drawing.Point(830, 114);
      this.editScreenHeight.Name = "editScreenHeight";
      this.editScreenHeight.Size = new System.Drawing.Size(61, 20);
      this.editScreenHeight.TabIndex = 24;
      this.editScreenHeight.TextChanged += new System.EventHandler(this.editScreenHeight_TextChanged);
      // 
      // editScreenWidth
      // 
      this.editScreenWidth.Location = new System.Drawing.Point(751, 114);
      this.editScreenWidth.Name = "editScreenWidth";
      this.editScreenWidth.Size = new System.Drawing.Size(70, 20);
      this.editScreenWidth.TabIndex = 23;
      this.editScreenWidth.TextChanged += new System.EventHandler(this.editScreenWidth_TextChanged);
      // 
      // screenVScroll
      // 
      this.screenVScroll.Location = new System.Drawing.Point(655, 6);
      this.screenVScroll.Name = "screenVScroll";
      this.screenVScroll.Size = new System.Drawing.Size(16, 404);
      this.screenVScroll.TabIndex = 0;
      this.screenVScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.screenVScroll_Scroll);
      // 
      // screenHScroll
      // 
      this.screenHScroll.Location = new System.Drawing.Point(8, 413);
      this.screenHScroll.Name = "screenHScroll";
      this.screenHScroll.Size = new System.Drawing.Size(644, 16);
      this.screenHScroll.TabIndex = 1;
      this.screenHScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.screenHScroll_Scroll);
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(677, 117);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(30, 13);
      this.label7.TabIndex = 22;
      this.label7.Text = "Size:";
      // 
      // labelBGColor4
      // 
      this.labelBGColor4.AutoSize = true;
      this.labelBGColor4.Location = new System.Drawing.Point(677, 90);
      this.labelBGColor4.Name = "labelBGColor4";
      this.labelBGColor4.Size = new System.Drawing.Size(58, 13);
      this.labelBGColor4.TabIndex = 22;
      this.labelBGColor4.Text = "BGColor 4:";
      // 
      // labelMColor2
      // 
      this.labelMColor2.AutoSize = true;
      this.labelMColor2.Location = new System.Drawing.Point(827, 90);
      this.labelMColor2.Name = "labelMColor2";
      this.labelMColor2.Size = new System.Drawing.Size(64, 13);
      this.labelMColor2.TabIndex = 22;
      this.labelMColor2.Text = "Multicolor 2:";
      // 
      // labelMColor1
      // 
      this.labelMColor1.AutoSize = true;
      this.labelMColor1.Location = new System.Drawing.Point(827, 63);
      this.labelMColor1.Name = "labelMColor1";
      this.labelMColor1.Size = new System.Drawing.Size(64, 13);
      this.labelMColor1.TabIndex = 22;
      this.labelMColor1.Text = "Multicolor 1:";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(677, 63);
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
      this.panelCharacters.TabIndex = 26;
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
      this.comboBGColor4.Location = new System.Drawing.Point(751, 87);
      this.comboBGColor4.Name = "comboBGColor4";
      this.comboBGColor4.Size = new System.Drawing.Size(70, 21);
      this.comboBGColor4.TabIndex = 21;
      this.comboBGColor4.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboColor_DrawItem);
      this.comboBGColor4.SelectedIndexChanged += new System.EventHandler(this.comboBGColor4_SelectedIndexChanged);
      // 
      // comboMulticolor2
      // 
      this.comboMulticolor2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboMulticolor2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboMulticolor2.FormattingEnabled = true;
      this.comboMulticolor2.Location = new System.Drawing.Point(897, 87);
      this.comboMulticolor2.Name = "comboMulticolor2";
      this.comboMulticolor2.Size = new System.Drawing.Size(70, 21);
      this.comboMulticolor2.TabIndex = 22;
      this.comboMulticolor2.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboColor_DrawItem);
      this.comboMulticolor2.SelectedIndexChanged += new System.EventHandler(this.comboMulticolor2_SelectedIndexChanged);
      // 
      // comboMulticolor1
      // 
      this.comboMulticolor1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboMulticolor1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboMulticolor1.FormattingEnabled = true;
      this.comboMulticolor1.Location = new System.Drawing.Point(897, 60);
      this.comboMulticolor1.Name = "comboMulticolor1";
      this.comboMulticolor1.Size = new System.Drawing.Size(70, 21);
      this.comboMulticolor1.TabIndex = 20;
      this.comboMulticolor1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboColor_DrawItem);
      this.comboMulticolor1.SelectedIndexChanged += new System.EventHandler(this.comboMulticolor1_SelectedIndexChanged);
      // 
      // comboBackground
      // 
      this.comboBackground.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboBackground.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBackground.FormattingEnabled = true;
      this.comboBackground.Location = new System.Drawing.Point(751, 60);
      this.comboBackground.Name = "comboBackground";
      this.comboBackground.Size = new System.Drawing.Size(70, 21);
      this.comboBackground.TabIndex = 19;
      this.comboBackground.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboColor_DrawItem);
      this.comboBackground.SelectedIndexChanged += new System.EventHandler(this.comboBackground_SelectedIndexChanged);
      // 
      // panelCharColors
      // 
      this.panelCharColors.AutoResize = false;
      this.panelCharColors.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panelCharColors.DisplayPage = fastImage1;
      this.panelCharColors.Image = null;
      this.panelCharColors.Location = new System.Drawing.Point(677, 416);
      this.panelCharColors.Name = "panelCharColors";
      this.panelCharColors.Size = new System.Drawing.Size(260, 20);
      this.panelCharColors.TabIndex = 0;
      this.panelCharColors.TabStop = false;
      this.panelCharColors.PostPaint += new GR.Forms.FastPictureBox.PostPaintCallback(this.panelCharColors_PostPaint);
      this.panelCharColors.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureCharColor_MouseDown);
      this.panelCharColors.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureCharColor_MouseMove);
      // 
      // pictureEditor
      // 
      this.pictureEditor.AutoResize = false;
      this.pictureEditor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.pictureEditor.DisplayPage = fastImage2;
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
      this.pictureEditor.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.pictureEditor_PreviewKeyDown);
      // 
      // tabCharsetEditor
      // 
      this.tabCharsetEditor.Controls.Add(this.tabEditor);
      this.tabCharsetEditor.Controls.Add(this.tabCharset);
      this.tabCharsetEditor.Controls.Add(this.tabProject);
      this.tabCharsetEditor.Controls.Add(this.tabImport);
      this.tabCharsetEditor.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabCharsetEditor.Location = new System.Drawing.Point(0, 24);
      this.tabCharsetEditor.Name = "tabCharsetEditor";
      this.tabCharsetEditor.SelectedIndex = 0;
      this.tabCharsetEditor.Size = new System.Drawing.Size(1069, 572);
      this.tabCharsetEditor.TabIndex = 1;
      // 
      // tabCharset
      // 
      this.tabCharset.Controls.Add(this.charEditor);
      this.tabCharset.Location = new System.Drawing.Point(4, 22);
      this.tabCharset.Name = "tabCharset";
      this.tabCharset.Size = new System.Drawing.Size(1061, 546);
      this.tabCharset.TabIndex = 2;
      this.tabCharset.Text = "Charset";
      this.tabCharset.UseVisualStyleBackColor = true;
      // 
      // charEditor
      // 
      this.charEditor.AllowModeChange = false;
      this.charEditor.Dock = System.Windows.Forms.DockStyle.Fill;
      this.charEditor.Location = new System.Drawing.Point(0, 0);
      this.charEditor.Name = "charEditor";
      this.charEditor.Size = new System.Drawing.Size(1061, 546);
      this.charEditor.TabIndex = 24;
      this.charEditor.Modified += new C64Studio.Controls.CharacterEditor.ModifiedHandler(this.charEditor_Modified);
      this.charEditor.CharactersShifted += new C64Studio.Controls.CharacterEditor.CharsetShiftedHandler(this.charEditor_CharactersShifted);
      // 
      // tabImport
      // 
      this.tabImport.Controls.Add(this.panelImport);
      this.tabImport.Controls.Add(this.btnImport);
      this.tabImport.Controls.Add(this.comboImportMethod);
      this.tabImport.Controls.Add(this.label2);
      this.tabImport.Location = new System.Drawing.Point(4, 22);
      this.tabImport.Name = "tabImport";
      this.tabImport.Padding = new System.Windows.Forms.Padding(3);
      this.tabImport.Size = new System.Drawing.Size(1061, 546);
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
      this.panelImport.Size = new System.Drawing.Size(1047, 505);
      this.panelImport.TabIndex = 33;
      // 
      // btnImport
      // 
      this.btnImport.Location = new System.Drawing.Point(342, 5);
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = new System.Drawing.Size(75, 21);
      this.btnImport.TabIndex = 32;
      this.btnImport.Text = "Import";
      this.btnImport.UseVisualStyleBackColor = true;
      this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
      // 
      // comboImportMethod
      // 
      this.comboImportMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboImportMethod.FormattingEnabled = true;
      this.comboImportMethod.Location = new System.Drawing.Point(93, 6);
      this.comboImportMethod.Name = "comboImportMethod";
      this.comboImportMethod.Size = new System.Drawing.Size(243, 21);
      this.comboImportMethod.TabIndex = 30;
      this.comboImportMethod.SelectedIndexChanged += new System.EventHandler(this.comboImportMethod_SelectedIndexChanged);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(8, 9);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(78, 13);
      this.label2.TabIndex = 31;
      this.label2.Text = "Import Method:";
      // 
      // CharsetScreenEditor
      // 
      this.ClientSize = new System.Drawing.Size(1069, 596);
      this.Controls.Add(this.tabCharsetEditor);
      this.Controls.Add(this.menuStrip1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "CharsetScreenEditor";
      this.Text = "Text Screen Editor";
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.tabProject.ResumeLayout(false);
      this.tabProject.PerformLayout();
      this.tabEditor.ResumeLayout(false);
      this.tabEditor.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.panelCharColors)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureEditor)).EndInit();
      this.tabCharsetEditor.ResumeLayout(false);
      this.tabCharset.ResumeLayout(false);
      this.tabImport.ResumeLayout(false);
      this.tabImport.PerformLayout();
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
    private System.Windows.Forms.TextBox editDataExport;
    private System.Windows.Forms.TabPage tabEditor;
    private GR.Forms.FastPictureBox pictureEditor;
    private System.Windows.Forms.TabControl tabCharsetEditor;
    private GR.Forms.ImageListbox panelCharacters;
    private GR.Forms.FastPictureBox panelCharColors;
    private System.Windows.Forms.Label labelMColor2;
    private System.Windows.Forms.Label labelMColor1;
    private System.Windows.Forms.Label label1;
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
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label labelBGColor4;
    private System.Windows.Forms.CheckBox checkApplyCharacter;
    private System.Windows.Forms.CheckBox checkApplyColors;
    private System.Windows.Forms.RadioButton btnToolText;
    private System.Windows.Forms.CheckBox checkShowGrid;
    private System.Windows.Forms.TabPage tabCharset;
    private System.Windows.Forms.CheckBox checkOverrideOriginalColorSettings;
    private System.Windows.Forms.CheckBox checkAutoCenter;
    private System.Windows.Forms.CheckBox checkReverse;
        private Controls.CharacterEditor charEditor;
    private System.Windows.Forms.Button btnClearScreen;
    private System.Windows.Forms.Button btnShiftDown;
    private System.Windows.Forms.Button btnShiftUp;
    private System.Windows.Forms.Button btnShiftRight;
    private System.Windows.Forms.Button btnShiftLeft;
    private System.Windows.Forms.TextBox editCharOffset;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.Button btnPaste;
    private System.Windows.Forms.Button btnCopy;
    private System.Windows.Forms.TabPage tabImport;
    private System.Windows.Forms.Panel panelExport;
    private System.Windows.Forms.ComboBox comboExportMethod;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.ComboBox comboExportOrientation;
    private System.Windows.Forms.Button btnExport;
    private System.Windows.Forms.Button btnImport;
    private System.Windows.Forms.ComboBox comboImportMethod;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Panel panelImport;
    public System.Windows.Forms.ComboBox comboBackground;
    public System.Windows.Forms.ComboBox comboMulticolor2;
    public System.Windows.Forms.ComboBox comboMulticolor1;
    public System.Windows.Forms.ComboBox comboBGColor4;
    public System.Windows.Forms.ComboBox comboCharsetMode;
  }
}
