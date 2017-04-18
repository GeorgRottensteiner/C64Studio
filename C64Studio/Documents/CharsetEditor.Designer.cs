using C64Studio.Controls;
namespace C64Studio
{
  partial class CharsetEditor
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( CharsetEditor ) );
      GR.Image.FastImage fastImage3 = new GR.Image.FastImage();
      this.tabCharsetEditor = new System.Windows.Forms.TabControl();
      this.tabEditor = new System.Windows.Forms.TabPage();
      this.panelCharColors = new GR.Forms.FastPictureBox();
      this.picturePlayground = new GR.Forms.FastPictureBox();
      this.label1 = new System.Windows.Forms.Label();
      this.comboCharsetMode = new System.Windows.Forms.ComboBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.btnClearChars = new System.Windows.Forms.Button();
      this.btnExchangeColors = new C64Studio.Controls.MenuButton();
      this.contextMenuExchangeColors = new System.Windows.Forms.ContextMenuStrip( this.components );
      this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exchangeMultiColor1WithBGColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exchangeMultiColor2WithBGColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
      this.comboCategories = new System.Windows.Forms.ComboBox();
      this.btnPasteFromClipboard = new System.Windows.Forms.Button();
      this.label4 = new System.Windows.Forms.Label();
      this.labelCharNo = new System.Windows.Forms.Label();
      this.panelCharacters = new GR.Forms.ImageListbox();
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
      this.pictureEditor = new GR.Forms.FastPictureBox();
      this.tabProject = new System.Windows.Forms.TabPage();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.btnDefaultLowerCase = new System.Windows.Forms.Button();
      this.btnDefaultUppercase = new System.Windows.Forms.Button();
      this.btnImportCharsetFromImage = new System.Windows.Forms.Button();
      this.btnImportFromFile = new System.Windows.Forms.Button();
      this.groupExport = new System.Windows.Forms.GroupBox();
      this.comboExportRange = new System.Windows.Forms.ComboBox();
      this.editPrefix = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.editCharactersCount = new System.Windows.Forms.TextBox();
      this.editCharactersFrom = new System.Windows.Forms.TextBox();
      this.editWrapByteCount = new System.Windows.Forms.TextBox();
      this.labelCharactersTo = new System.Windows.Forms.Label();
      this.labelCharactersFrom = new System.Windows.Forms.Label();
      this.checkExportToDataWrap = new System.Windows.Forms.CheckBox();
      this.checkIncludeColor = new System.Windows.Forms.CheckBox();
      this.checkExportToDataIncludeRes = new System.Windows.Forms.CheckBox();
      this.editDataExport = new System.Windows.Forms.TextBox();
      this.button1 = new System.Windows.Forms.Button();
      this.btnExportToData = new System.Windows.Forms.Button();
      this.btnExportCharset = new System.Windows.Forms.Button();
      this.tabCategories = new System.Windows.Forms.TabPage();
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
      this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
      this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
      this.editCategoryName = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.openCharsetProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveCharsetProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.closeCharsetProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.exchangeMultiColors1And2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exchangeMultiColor1AndBGColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exchangeMultiColor2AndBGColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolTip1 = new System.Windows.Forms.ToolTip( this.components );
      ( (System.ComponentModel.ISupportInitialize)( this.m_FileWatcher ) ).BeginInit();
      this.tabCharsetEditor.SuspendLayout();
      this.tabEditor.SuspendLayout();
      ( (System.ComponentModel.ISupportInitialize)( this.panelCharColors ) ).BeginInit();
      ( (System.ComponentModel.ISupportInitialize)( this.picturePlayground ) ).BeginInit();
      this.groupBox2.SuspendLayout();
      this.contextMenuExchangeColors.SuspendLayout();
      ( (System.ComponentModel.ISupportInitialize)( this.pictureEditor ) ).BeginInit();
      this.tabProject.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.groupExport.SuspendLayout();
      this.tabCategories.SuspendLayout();
      this.groupAllCategories.SuspendLayout();
      this.groupCategorySpecific.SuspendLayout();
      this.menuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabCharsetEditor
      // 
      this.tabCharsetEditor.Controls.Add( this.tabEditor );
      this.tabCharsetEditor.Controls.Add( this.tabProject );
      this.tabCharsetEditor.Controls.Add( this.tabCategories );
      this.tabCharsetEditor.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabCharsetEditor.Location = new System.Drawing.Point( 0, 24 );
      this.tabCharsetEditor.Name = "tabCharsetEditor";
      this.tabCharsetEditor.SelectedIndex = 0;
      this.tabCharsetEditor.Size = new System.Drawing.Size( 1064, 503 );
      this.tabCharsetEditor.TabIndex = 0;
      // 
      // tabEditor
      // 
      this.tabEditor.Controls.Add( this.panelCharColors );
      this.tabEditor.Controls.Add( this.picturePlayground );
      this.tabEditor.Controls.Add( this.groupBox2 );
      this.tabEditor.Controls.Add( this.btnExchangeColors );
      this.tabEditor.Controls.Add( this.btnPaste );
      this.tabEditor.Controls.Add( this.btnCopy );
      this.tabEditor.Controls.Add( this.btnInvert );
      this.tabEditor.Controls.Add( this.btnMirrorY );
      this.tabEditor.Controls.Add( this.btnMirrorX );
      this.tabEditor.Controls.Add( this.btnShiftDown );
      this.tabEditor.Controls.Add( this.btnShiftUp );
      this.tabEditor.Controls.Add( this.btnShiftRight );
      this.tabEditor.Controls.Add( this.button3 );
      this.tabEditor.Controls.Add( this.btnRotateLeft );
      this.tabEditor.Controls.Add( this.btnShiftLeft );
      this.tabEditor.Controls.Add( this.comboCategories );
      this.tabEditor.Controls.Add( this.btnPasteFromClipboard );
      this.tabEditor.Controls.Add( this.label4 );
      this.tabEditor.Controls.Add( this.labelCharNo );
      this.tabEditor.Controls.Add( this.panelCharacters );
      this.tabEditor.Controls.Add( this.checkShowGrid );
      this.tabEditor.Controls.Add( this.checkPasteMultiColor );
      this.tabEditor.Controls.Add( this.radioCharColor );
      this.tabEditor.Controls.Add( this.radioBGColor4 );
      this.tabEditor.Controls.Add( this.radioMulticolor2 );
      this.tabEditor.Controls.Add( this.radioMultiColor1 );
      this.tabEditor.Controls.Add( this.radioBackground );
      this.tabEditor.Controls.Add( this.comboCharColor );
      this.tabEditor.Controls.Add( this.comboBGColor4 );
      this.tabEditor.Controls.Add( this.comboMulticolor2 );
      this.tabEditor.Controls.Add( this.comboMulticolor1 );
      this.tabEditor.Controls.Add( this.comboBackground );
      this.tabEditor.Controls.Add( this.pictureEditor );
      this.tabEditor.Location = new System.Drawing.Point( 4, 22 );
      this.tabEditor.Name = "tabEditor";
      this.tabEditor.Padding = new System.Windows.Forms.Padding( 3 );
      this.tabEditor.Size = new System.Drawing.Size( 1056, 477 );
      this.tabEditor.TabIndex = 0;
      this.tabEditor.Text = "Character";
      this.tabEditor.UseVisualStyleBackColor = true;
      // 
      // panelCharColors
      // 
      this.panelCharColors.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panelCharColors.DisplayPage = fastImage1;
      this.panelCharColors.Image = null;
      this.panelCharColors.Location = new System.Drawing.Point( 782, 274 );
      this.panelCharColors.Name = "panelCharColors";
      this.panelCharColors.Size = new System.Drawing.Size( 260, 20 );
      this.panelCharColors.TabIndex = 19;
      this.panelCharColors.TabStop = false;
      this.panelCharColors.MouseMove += new System.Windows.Forms.MouseEventHandler( this.panelCharColors_MouseMove );
      this.panelCharColors.MouseDown += new System.Windows.Forms.MouseEventHandler( this.panelCharColors_MouseDown );
      // 
      // picturePlayground
      // 
      this.picturePlayground.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.picturePlayground.DisplayPage = fastImage2;
      this.picturePlayground.Image = null;
      this.picturePlayground.Location = new System.Drawing.Point( 782, 6 );
      this.picturePlayground.Name = "picturePlayground";
      this.picturePlayground.Size = new System.Drawing.Size( 260, 260 );
      this.picturePlayground.TabIndex = 18;
      this.picturePlayground.TabStop = false;
      this.picturePlayground.MouseMove += new System.Windows.Forms.MouseEventHandler( this.picturePlayground_MouseMove );
      this.picturePlayground.MouseDown += new System.Windows.Forms.MouseEventHandler( this.picturePlayground_MouseDown );
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point( 9, 26 );
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size( 37, 13 );
      this.label1.TabIndex = 17;
      this.label1.Text = "Mode:";
      // 
      // comboCharsetMode
      // 
      this.comboCharsetMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboCharsetMode.FormattingEnabled = true;
      this.comboCharsetMode.Location = new System.Drawing.Point( 52, 23 );
      this.comboCharsetMode.Name = "comboCharsetMode";
      this.comboCharsetMode.Size = new System.Drawing.Size( 109, 21 );
      this.comboCharsetMode.TabIndex = 16;
      this.comboCharsetMode.SelectedIndexChanged += new System.EventHandler( this.comboCharsetMode_SelectedIndexChanged );
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add( this.btnClearChars );
      this.groupBox2.Controls.Add( this.comboCharsetMode );
      this.groupBox2.Controls.Add( this.label1 );
      this.groupBox2.Location = new System.Drawing.Point( 282, 304 );
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size( 494, 56 );
      this.groupBox2.TabIndex = 15;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "with selected characters";
      // 
      // btnClearChars
      // 
      this.btnClearChars.Location = new System.Drawing.Point( 177, 19 );
      this.btnClearChars.Name = "btnClearChars";
      this.btnClearChars.Size = new System.Drawing.Size( 98, 26 );
      this.btnClearChars.TabIndex = 6;
      this.btnClearChars.Text = "Clear";
      this.toolTip1.SetToolTip( this.btnClearChars, "Clear selected characters" );
      this.btnClearChars.UseVisualStyleBackColor = true;
      this.btnClearChars.Click += new System.EventHandler( this.btnClear_Click );
      // 
      // btnExchangeColors
      // 
      this.btnExchangeColors.Location = new System.Drawing.Point( 389, 146 );
      this.btnExchangeColors.Menu = this.contextMenuExchangeColors;
      this.btnExchangeColors.Name = "btnExchangeColors";
      this.btnExchangeColors.Size = new System.Drawing.Size( 121, 26 );
      this.btnExchangeColors.TabIndex = 14;
      this.btnExchangeColors.Text = "Exchange Colors";
      this.btnExchangeColors.UseVisualStyleBackColor = true;
      // 
      // contextMenuExchangeColors
      // 
      this.contextMenuExchangeColors.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem,
            this.exchangeMultiColor1WithBGColorToolStripMenuItem,
            this.exchangeMultiColor2WithBGColorToolStripMenuItem} );
      this.contextMenuExchangeColors.Name = "contextMenuExchangeColors";
      this.contextMenuExchangeColors.Size = new System.Drawing.Size( 295, 70 );
      // 
      // exchangeMultiColor1WithMultiColor2ToolStripMenuItem
      // 
      this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem.Name = "exchangeMultiColor1WithMultiColor2ToolStripMenuItem";
      this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem.Size = new System.Drawing.Size( 294, 22 );
      this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem.Text = "Exchange Multi Color 1 with Multi Color 2";
      this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem.Click += new System.EventHandler( this.exchangeMultiColors1And2ToolStripMenuItem_Click );
      // 
      // exchangeMultiColor1WithBGColorToolStripMenuItem
      // 
      this.exchangeMultiColor1WithBGColorToolStripMenuItem.Name = "exchangeMultiColor1WithBGColorToolStripMenuItem";
      this.exchangeMultiColor1WithBGColorToolStripMenuItem.Size = new System.Drawing.Size( 294, 22 );
      this.exchangeMultiColor1WithBGColorToolStripMenuItem.Text = "Exchange Multi Color 1 with BG Color";
      this.exchangeMultiColor1WithBGColorToolStripMenuItem.Click += new System.EventHandler( this.exchangeMultiColor1AndBGColorToolStripMenuItem_Click );
      // 
      // exchangeMultiColor2WithBGColorToolStripMenuItem
      // 
      this.exchangeMultiColor2WithBGColorToolStripMenuItem.Name = "exchangeMultiColor2WithBGColorToolStripMenuItem";
      this.exchangeMultiColor2WithBGColorToolStripMenuItem.Size = new System.Drawing.Size( 294, 22 );
      this.exchangeMultiColor2WithBGColorToolStripMenuItem.Text = "Exchange Multi Color 2 with BG Color";
      this.exchangeMultiColor2WithBGColorToolStripMenuItem.Click += new System.EventHandler( this.exchangeMultiColor2AndBGColorToolStripMenuItem_Click );
      // 
      // btnPaste
      // 
      this.btnPaste.Image = ( (System.Drawing.Image)( resources.GetObject( "btnPaste.Image" ) ) );
      this.btnPaste.Location = new System.Drawing.Point( 548, 272 );
      this.btnPaste.Name = "btnPaste";
      this.btnPaste.Size = new System.Drawing.Size( 26, 26 );
      this.btnPaste.TabIndex = 11;
      this.btnPaste.UseVisualStyleBackColor = true;
      this.btnPaste.Click += new System.EventHandler( this.btnPaste_Click );
      // 
      // btnCopy
      // 
      this.btnCopy.Image = ( (System.Drawing.Image)( resources.GetObject( "btnCopy.Image" ) ) );
      this.btnCopy.Location = new System.Drawing.Point( 516, 272 );
      this.btnCopy.Name = "btnCopy";
      this.btnCopy.Size = new System.Drawing.Size( 26, 26 );
      this.btnCopy.TabIndex = 11;
      this.btnCopy.UseVisualStyleBackColor = true;
      this.btnCopy.Click += new System.EventHandler( this.btnCopy_Click );
      // 
      // btnInvert
      // 
      this.btnInvert.Image = ( (System.Drawing.Image)( resources.GetObject( "btnInvert.Image" ) ) );
      this.btnInvert.Location = new System.Drawing.Point( 200, 272 );
      this.btnInvert.Name = "btnInvert";
      this.btnInvert.Size = new System.Drawing.Size( 26, 26 );
      this.btnInvert.TabIndex = 11;
      this.toolTip1.SetToolTip( this.btnInvert, "Invert Colors" );
      this.btnInvert.UseVisualStyleBackColor = true;
      this.btnInvert.Click += new System.EventHandler( this.btnInvert_Click );
      // 
      // btnMirrorY
      // 
      this.btnMirrorY.Image = ( (System.Drawing.Image)( resources.GetObject( "btnMirrorY.Image" ) ) );
      this.btnMirrorY.Location = new System.Drawing.Point( 168, 272 );
      this.btnMirrorY.Name = "btnMirrorY";
      this.btnMirrorY.Size = new System.Drawing.Size( 26, 26 );
      this.btnMirrorY.TabIndex = 11;
      this.toolTip1.SetToolTip( this.btnMirrorY, "Mirror Vertically" );
      this.btnMirrorY.UseVisualStyleBackColor = true;
      this.btnMirrorY.Click += new System.EventHandler( this.btnMirrorY_Click );
      // 
      // btnMirrorX
      // 
      this.btnMirrorX.Image = ( (System.Drawing.Image)( resources.GetObject( "btnMirrorX.Image" ) ) );
      this.btnMirrorX.Location = new System.Drawing.Point( 136, 272 );
      this.btnMirrorX.Name = "btnMirrorX";
      this.btnMirrorX.Size = new System.Drawing.Size( 26, 26 );
      this.btnMirrorX.TabIndex = 12;
      this.toolTip1.SetToolTip( this.btnMirrorX, "Mirror Horizontally" );
      this.btnMirrorX.UseVisualStyleBackColor = true;
      this.btnMirrorX.Click += new System.EventHandler( this.btnMirrorX_Click );
      // 
      // btnShiftDown
      // 
      this.btnShiftDown.Image = ( (System.Drawing.Image)( resources.GetObject( "btnShiftDown.Image" ) ) );
      this.btnShiftDown.Location = new System.Drawing.Point( 104, 272 );
      this.btnShiftDown.Name = "btnShiftDown";
      this.btnShiftDown.Size = new System.Drawing.Size( 26, 26 );
      this.btnShiftDown.TabIndex = 13;
      this.toolTip1.SetToolTip( this.btnShiftDown, "Shift Down" );
      this.btnShiftDown.UseVisualStyleBackColor = true;
      this.btnShiftDown.Click += new System.EventHandler( this.btnShiftDown_Click );
      // 
      // btnShiftUp
      // 
      this.btnShiftUp.Image = ( (System.Drawing.Image)( resources.GetObject( "btnShiftUp.Image" ) ) );
      this.btnShiftUp.Location = new System.Drawing.Point( 72, 272 );
      this.btnShiftUp.Name = "btnShiftUp";
      this.btnShiftUp.Size = new System.Drawing.Size( 26, 26 );
      this.btnShiftUp.TabIndex = 8;
      this.toolTip1.SetToolTip( this.btnShiftUp, "Shift Up" );
      this.btnShiftUp.UseVisualStyleBackColor = true;
      this.btnShiftUp.Click += new System.EventHandler( this.btnShiftUp_Click );
      // 
      // btnShiftRight
      // 
      this.btnShiftRight.Image = ( (System.Drawing.Image)( resources.GetObject( "btnShiftRight.Image" ) ) );
      this.btnShiftRight.Location = new System.Drawing.Point( 40, 272 );
      this.btnShiftRight.Name = "btnShiftRight";
      this.btnShiftRight.Size = new System.Drawing.Size( 26, 26 );
      this.btnShiftRight.TabIndex = 9;
      this.toolTip1.SetToolTip( this.btnShiftRight, "Shift Right" );
      this.btnShiftRight.UseVisualStyleBackColor = true;
      this.btnShiftRight.Click += new System.EventHandler( this.btnShiftRight_Click );
      // 
      // button3
      // 
      this.button3.Image = ( (System.Drawing.Image)( resources.GetObject( "button3.Image" ) ) );
      this.button3.Location = new System.Drawing.Point( 40, 304 );
      this.button3.Name = "button3";
      this.button3.Size = new System.Drawing.Size( 26, 26 );
      this.button3.TabIndex = 10;
      this.toolTip1.SetToolTip( this.button3, "Rotate Right" );
      this.button3.UseVisualStyleBackColor = true;
      this.button3.Click += new System.EventHandler( this.btnRotateRight_Click );
      // 
      // btnRotateLeft
      // 
      this.btnRotateLeft.Image = ( (System.Drawing.Image)( resources.GetObject( "btnRotateLeft.Image" ) ) );
      this.btnRotateLeft.Location = new System.Drawing.Point( 8, 304 );
      this.btnRotateLeft.Name = "btnRotateLeft";
      this.btnRotateLeft.Size = new System.Drawing.Size( 26, 26 );
      this.btnRotateLeft.TabIndex = 10;
      this.toolTip1.SetToolTip( this.btnRotateLeft, "Rotate Left" );
      this.btnRotateLeft.UseVisualStyleBackColor = true;
      this.btnRotateLeft.Click += new System.EventHandler( this.btnRotateLeft_Click );
      // 
      // btnShiftLeft
      // 
      this.btnShiftLeft.Image = ( (System.Drawing.Image)( resources.GetObject( "btnShiftLeft.Image" ) ) );
      this.btnShiftLeft.Location = new System.Drawing.Point( 8, 272 );
      this.btnShiftLeft.Name = "btnShiftLeft";
      this.btnShiftLeft.Size = new System.Drawing.Size( 26, 26 );
      this.btnShiftLeft.TabIndex = 10;
      this.toolTip1.SetToolTip( this.btnShiftLeft, "Shift Left" );
      this.btnShiftLeft.UseVisualStyleBackColor = true;
      this.btnShiftLeft.Click += new System.EventHandler( this.btnShiftLeft_Click );
      // 
      // comboCategories
      // 
      this.comboCategories.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboCategories.FormattingEnabled = true;
      this.comboCategories.Location = new System.Drawing.Point( 389, 207 );
      this.comboCategories.Name = "comboCategories";
      this.comboCategories.Size = new System.Drawing.Size( 121, 21 );
      this.comboCategories.TabIndex = 7;
      this.comboCategories.SelectedIndexChanged += new System.EventHandler( this.comboCategories_SelectedIndexChanged );
      // 
      // btnPasteFromClipboard
      // 
      this.btnPasteFromClipboard.Location = new System.Drawing.Point( 580, 274 );
      this.btnPasteFromClipboard.Name = "btnPasteFromClipboard";
      this.btnPasteFromClipboard.Size = new System.Drawing.Size( 117, 23 );
      this.btnPasteFromClipboard.TabIndex = 6;
      this.btnPasteFromClipboard.Text = "Big Paste";
      this.btnPasteFromClipboard.UseVisualStyleBackColor = true;
      this.btnPasteFromClipboard.Click += new System.EventHandler( this.btnPasteFromClipboard_Click );
      // 
      // label4
      // 
      this.label4.Location = new System.Drawing.Point( 279, 210 );
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size( 231, 23 );
      this.label4.TabIndex = 5;
      this.label4.Text = "Category:";
      // 
      // labelCharNo
      // 
      this.labelCharNo.Location = new System.Drawing.Point( 279, 181 );
      this.labelCharNo.Name = "labelCharNo";
      this.labelCharNo.Size = new System.Drawing.Size( 231, 23 );
      this.labelCharNo.TabIndex = 5;
      this.labelCharNo.Text = "label1";
      this.labelCharNo.Paint += new System.Windows.Forms.PaintEventHandler( this.labelCharNo_Paint );
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
      this.panelCharacters.HottrackColor = ( (uint)( 2151694591u ) );
      this.panelCharacters.ItemHeight = 8;
      this.panelCharacters.ItemWidth = 8;
      this.panelCharacters.Location = new System.Drawing.Point( 516, 6 );
      this.panelCharacters.Name = "panelCharacters";
      this.panelCharacters.PixelFormat = System.Drawing.Imaging.PixelFormat.DontCare;
      this.panelCharacters.SelectedIndex = -1;
      this.panelCharacters.Size = new System.Drawing.Size( 260, 260 );
      this.panelCharacters.TabIndex = 4;
      this.panelCharacters.TabStop = true;
      this.panelCharacters.VisibleAutoScrollHorizontal = false;
      this.panelCharacters.VisibleAutoScrollVertical = false;
      this.panelCharacters.SelectionChanged += new System.EventHandler( this.panelCharacters_SelectionChanged );
      // 
      // checkShowGrid
      // 
      this.checkShowGrid.AutoSize = true;
      this.checkShowGrid.Location = new System.Drawing.Point( 282, 249 );
      this.checkShowGrid.Name = "checkShowGrid";
      this.checkShowGrid.Size = new System.Drawing.Size( 75, 17 );
      this.checkShowGrid.TabIndex = 3;
      this.checkShowGrid.Text = "Show Grid";
      this.checkShowGrid.UseVisualStyleBackColor = true;
      this.checkShowGrid.CheckedChanged += new System.EventHandler( this.checkShowGrid_CheckedChanged );
      // 
      // checkPasteMultiColor
      // 
      this.checkPasteMultiColor.AutoSize = true;
      this.checkPasteMultiColor.Location = new System.Drawing.Point( 282, 278 );
      this.checkPasteMultiColor.Name = "checkPasteMultiColor";
      this.checkPasteMultiColor.Size = new System.Drawing.Size( 145, 17 );
      this.checkPasteMultiColor.TabIndex = 3;
      this.checkPasteMultiColor.Text = "Force Multicolor on paste";
      this.checkPasteMultiColor.UseVisualStyleBackColor = true;
      // 
      // radioCharColor
      // 
      this.radioCharColor.AutoSize = true;
      this.radioCharColor.Location = new System.Drawing.Point( 282, 114 );
      this.radioCharColor.Name = "radioCharColor";
      this.radioCharColor.Size = new System.Drawing.Size( 74, 17 );
      this.radioCharColor.TabIndex = 2;
      this.radioCharColor.TabStop = true;
      this.radioCharColor.Text = "Char Color";
      this.radioCharColor.UseVisualStyleBackColor = true;
      this.radioCharColor.CheckedChanged += new System.EventHandler( this.radioCharColor_CheckedChanged );
      // 
      // radioBGColor4
      // 
      this.radioBGColor4.AutoSize = true;
      this.radioBGColor4.Enabled = false;
      this.radioBGColor4.Location = new System.Drawing.Point( 282, 87 );
      this.radioBGColor4.Name = "radioBGColor4";
      this.radioBGColor4.Size = new System.Drawing.Size( 76, 17 );
      this.radioBGColor4.TabIndex = 2;
      this.radioBGColor4.TabStop = true;
      this.radioBGColor4.Text = "BG Color 4";
      this.radioBGColor4.UseVisualStyleBackColor = true;
      this.radioBGColor4.CheckedChanged += new System.EventHandler( this.radioMulticolor2_CheckedChanged );
      // 
      // radioMulticolor2
      // 
      this.radioMulticolor2.AutoSize = true;
      this.radioMulticolor2.Location = new System.Drawing.Point( 282, 60 );
      this.radioMulticolor2.Name = "radioMulticolor2";
      this.radioMulticolor2.Size = new System.Drawing.Size( 79, 17 );
      this.radioMulticolor2.TabIndex = 2;
      this.radioMulticolor2.TabStop = true;
      this.radioMulticolor2.Text = "Multicolor 2";
      this.radioMulticolor2.UseVisualStyleBackColor = true;
      this.radioMulticolor2.CheckedChanged += new System.EventHandler( this.radioMulticolor2_CheckedChanged );
      // 
      // radioMultiColor1
      // 
      this.radioMultiColor1.AutoSize = true;
      this.radioMultiColor1.Location = new System.Drawing.Point( 282, 33 );
      this.radioMultiColor1.Name = "radioMultiColor1";
      this.radioMultiColor1.Size = new System.Drawing.Size( 79, 17 );
      this.radioMultiColor1.TabIndex = 2;
      this.radioMultiColor1.TabStop = true;
      this.radioMultiColor1.Text = "Multicolor 1";
      this.radioMultiColor1.UseVisualStyleBackColor = true;
      this.radioMultiColor1.CheckedChanged += new System.EventHandler( this.radioMultiColor1_CheckedChanged );
      // 
      // radioBackground
      // 
      this.radioBackground.AutoSize = true;
      this.radioBackground.Location = new System.Drawing.Point( 282, 6 );
      this.radioBackground.Name = "radioBackground";
      this.radioBackground.Size = new System.Drawing.Size( 83, 17 );
      this.radioBackground.TabIndex = 2;
      this.radioBackground.TabStop = true;
      this.radioBackground.Text = "Background";
      this.radioBackground.UseVisualStyleBackColor = true;
      this.radioBackground.CheckedChanged += new System.EventHandler( this.radioBackground_CheckedChanged );
      // 
      // comboCharColor
      // 
      this.comboCharColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboCharColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboCharColor.FormattingEnabled = true;
      this.comboCharColor.Location = new System.Drawing.Point( 389, 114 );
      this.comboCharColor.Name = "comboCharColor";
      this.comboCharColor.Size = new System.Drawing.Size( 71, 21 );
      this.comboCharColor.TabIndex = 1;
      this.comboCharColor.DrawItem += new System.Windows.Forms.DrawItemEventHandler( this.comboMulticolor_DrawItem );
      this.comboCharColor.SelectedIndexChanged += new System.EventHandler( this.comboCharColor_SelectedIndexChanged );
      // 
      // comboBGColor4
      // 
      this.comboBGColor4.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboBGColor4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBGColor4.FormattingEnabled = true;
      this.comboBGColor4.Location = new System.Drawing.Point( 389, 87 );
      this.comboBGColor4.Name = "comboBGColor4";
      this.comboBGColor4.Size = new System.Drawing.Size( 71, 21 );
      this.comboBGColor4.TabIndex = 1;
      this.comboBGColor4.DrawItem += new System.Windows.Forms.DrawItemEventHandler( this.comboColor_DrawItem );
      this.comboBGColor4.SelectedIndexChanged += new System.EventHandler( this.comboBGColor4_SelectedIndexChanged );
      // 
      // comboMulticolor2
      // 
      this.comboMulticolor2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboMulticolor2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboMulticolor2.FormattingEnabled = true;
      this.comboMulticolor2.Location = new System.Drawing.Point( 389, 60 );
      this.comboMulticolor2.Name = "comboMulticolor2";
      this.comboMulticolor2.Size = new System.Drawing.Size( 71, 21 );
      this.comboMulticolor2.TabIndex = 1;
      this.comboMulticolor2.DrawItem += new System.Windows.Forms.DrawItemEventHandler( this.comboColor_DrawItem );
      this.comboMulticolor2.SelectedIndexChanged += new System.EventHandler( this.comboMulticolor2_SelectedIndexChanged );
      // 
      // comboMulticolor1
      // 
      this.comboMulticolor1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboMulticolor1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboMulticolor1.FormattingEnabled = true;
      this.comboMulticolor1.Location = new System.Drawing.Point( 389, 33 );
      this.comboMulticolor1.Name = "comboMulticolor1";
      this.comboMulticolor1.Size = new System.Drawing.Size( 71, 21 );
      this.comboMulticolor1.TabIndex = 1;
      this.comboMulticolor1.DrawItem += new System.Windows.Forms.DrawItemEventHandler( this.comboColor_DrawItem );
      this.comboMulticolor1.SelectedIndexChanged += new System.EventHandler( this.comboMulticolor1_SelectedIndexChanged );
      // 
      // comboBackground
      // 
      this.comboBackground.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboBackground.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBackground.FormattingEnabled = true;
      this.comboBackground.Location = new System.Drawing.Point( 389, 6 );
      this.comboBackground.Name = "comboBackground";
      this.comboBackground.Size = new System.Drawing.Size( 71, 21 );
      this.comboBackground.TabIndex = 1;
      this.comboBackground.DrawItem += new System.Windows.Forms.DrawItemEventHandler( this.comboColor_DrawItem );
      this.comboBackground.SelectedIndexChanged += new System.EventHandler( this.comboBackground_SelectedIndexChanged );
      // 
      // pictureEditor
      // 
      this.pictureEditor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.pictureEditor.DisplayPage = fastImage3;
      this.pictureEditor.Image = null;
      this.pictureEditor.Location = new System.Drawing.Point( 8, 6 );
      this.pictureEditor.Name = "pictureEditor";
      this.pictureEditor.Size = new System.Drawing.Size( 260, 260 );
      this.pictureEditor.TabIndex = 0;
      this.pictureEditor.TabStop = false;
      this.pictureEditor.MouseMove += new System.Windows.Forms.MouseEventHandler( this.pictureEditor_MouseMove );
      this.pictureEditor.MouseDown += new System.Windows.Forms.MouseEventHandler( this.pictureEditor_MouseDown );
      // 
      // tabProject
      // 
      this.tabProject.Controls.Add( this.groupBox1 );
      this.tabProject.Controls.Add( this.groupExport );
      this.tabProject.Location = new System.Drawing.Point( 4, 22 );
      this.tabProject.Name = "tabProject";
      this.tabProject.Padding = new System.Windows.Forms.Padding( 3 );
      this.tabProject.Size = new System.Drawing.Size( 1056, 477 );
      this.tabProject.TabIndex = 1;
      this.tabProject.Text = "Project";
      this.tabProject.UseVisualStyleBackColor = true;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add( this.btnDefaultLowerCase );
      this.groupBox1.Controls.Add( this.btnDefaultUppercase );
      this.groupBox1.Controls.Add( this.btnImportCharsetFromImage );
      this.groupBox1.Controls.Add( this.btnImportFromFile );
      this.groupBox1.Location = new System.Drawing.Point( 453, 6 );
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size( 324, 272 );
      this.groupBox1.TabIndex = 4;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Import";
      // 
      // btnDefaultLowerCase
      // 
      this.btnDefaultLowerCase.Location = new System.Drawing.Point( 129, 48 );
      this.btnDefaultLowerCase.Name = "btnDefaultLowerCase";
      this.btnDefaultLowerCase.Size = new System.Drawing.Size( 117, 23 );
      this.btnDefaultLowerCase.TabIndex = 2;
      this.btnDefaultLowerCase.Text = "Default Lowercase";
      this.btnDefaultLowerCase.UseVisualStyleBackColor = true;
      this.btnDefaultLowerCase.Click += new System.EventHandler( this.btnDefaultLowercase_Click );
      // 
      // btnDefaultUppercase
      // 
      this.btnDefaultUppercase.Location = new System.Drawing.Point( 6, 48 );
      this.btnDefaultUppercase.Name = "btnDefaultUppercase";
      this.btnDefaultUppercase.Size = new System.Drawing.Size( 117, 23 );
      this.btnDefaultUppercase.TabIndex = 2;
      this.btnDefaultUppercase.Text = "Default Uppercase";
      this.btnDefaultUppercase.UseVisualStyleBackColor = true;
      this.btnDefaultUppercase.Click += new System.EventHandler( this.btnDefaultUppercase_Click );
      // 
      // btnImportCharsetFromImage
      // 
      this.btnImportCharsetFromImage.Location = new System.Drawing.Point( 6, 77 );
      this.btnImportCharsetFromImage.Name = "btnImportCharsetFromImage";
      this.btnImportCharsetFromImage.Size = new System.Drawing.Size( 117, 23 );
      this.btnImportCharsetFromImage.TabIndex = 2;
      this.btnImportCharsetFromImage.Text = "From Image...";
      this.btnImportCharsetFromImage.UseVisualStyleBackColor = true;
      this.btnImportCharsetFromImage.Click += new System.EventHandler( this.btnImportCharsetFromFile_Click );
      // 
      // btnImportFromFile
      // 
      this.btnImportFromFile.Location = new System.Drawing.Point( 6, 19 );
      this.btnImportFromFile.Name = "btnImportFromFile";
      this.btnImportFromFile.Size = new System.Drawing.Size( 117, 23 );
      this.btnImportFromFile.TabIndex = 2;
      this.btnImportFromFile.Text = "From File...";
      this.btnImportFromFile.UseVisualStyleBackColor = true;
      this.btnImportFromFile.Click += new System.EventHandler( this.btnImportCharset_Click );
      // 
      // groupExport
      // 
      this.groupExport.Controls.Add( this.comboExportRange );
      this.groupExport.Controls.Add( this.editPrefix );
      this.groupExport.Controls.Add( this.label2 );
      this.groupExport.Controls.Add( this.editCharactersCount );
      this.groupExport.Controls.Add( this.editCharactersFrom );
      this.groupExport.Controls.Add( this.editWrapByteCount );
      this.groupExport.Controls.Add( this.labelCharactersTo );
      this.groupExport.Controls.Add( this.labelCharactersFrom );
      this.groupExport.Controls.Add( this.checkExportToDataWrap );
      this.groupExport.Controls.Add( this.checkIncludeColor );
      this.groupExport.Controls.Add( this.checkExportToDataIncludeRes );
      this.groupExport.Controls.Add( this.editDataExport );
      this.groupExport.Controls.Add( this.button1 );
      this.groupExport.Controls.Add( this.btnExportToData );
      this.groupExport.Controls.Add( this.btnExportCharset );
      this.groupExport.Location = new System.Drawing.Point( 6, 6 );
      this.groupExport.Name = "groupExport";
      this.groupExport.Size = new System.Drawing.Size( 441, 272 );
      this.groupExport.TabIndex = 3;
      this.groupExport.TabStop = false;
      this.groupExport.Text = "Export";
      // 
      // comboExportRange
      // 
      this.comboExportRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboExportRange.FormattingEnabled = true;
      this.comboExportRange.Location = new System.Drawing.Point( 138, 21 );
      this.comboExportRange.Name = "comboExportRange";
      this.comboExportRange.Size = new System.Drawing.Size( 88, 21 );
      this.comboExportRange.TabIndex = 8;
      this.comboExportRange.SelectedIndexChanged += new System.EventHandler( this.comboExportRange_SelectedIndexChanged );
      // 
      // editPrefix
      // 
      this.editPrefix.Location = new System.Drawing.Point( 232, 50 );
      this.editPrefix.Name = "editPrefix";
      this.editPrefix.Size = new System.Drawing.Size( 54, 20 );
      this.editPrefix.TabIndex = 7;
      this.editPrefix.Text = "!byte ";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point( 292, 80 );
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size( 32, 13 );
      this.label2.TabIndex = 6;
      this.label2.Text = "bytes";
      // 
      // editCharactersCount
      // 
      this.editCharactersCount.Location = new System.Drawing.Point( 376, 21 );
      this.editCharactersCount.Name = "editCharactersCount";
      this.editCharactersCount.Size = new System.Drawing.Size( 56, 20 );
      this.editCharactersCount.TabIndex = 1;
      this.editCharactersCount.TextChanged += new System.EventHandler( this.editUsedCharacters_TextChanged );
      // 
      // editCharactersFrom
      // 
      this.editCharactersFrom.Location = new System.Drawing.Point( 271, 21 );
      this.editCharactersFrom.Name = "editCharactersFrom";
      this.editCharactersFrom.Size = new System.Drawing.Size( 56, 20 );
      this.editCharactersFrom.TabIndex = 1;
      this.editCharactersFrom.TextChanged += new System.EventHandler( this.editStartCharacters_TextChanged );
      // 
      // editWrapByteCount
      // 
      this.editWrapByteCount.Enabled = false;
      this.editWrapByteCount.Location = new System.Drawing.Point( 232, 77 );
      this.editWrapByteCount.Name = "editWrapByteCount";
      this.editWrapByteCount.Size = new System.Drawing.Size( 54, 20 );
      this.editWrapByteCount.TabIndex = 5;
      this.editWrapByteCount.Text = "8";
      // 
      // labelCharactersTo
      // 
      this.labelCharactersTo.AutoSize = true;
      this.labelCharactersTo.Location = new System.Drawing.Point( 333, 24 );
      this.labelCharactersTo.Name = "labelCharactersTo";
      this.labelCharactersTo.Size = new System.Drawing.Size( 37, 13 );
      this.labelCharactersTo.TabIndex = 0;
      this.labelCharactersTo.Text = "count:";
      // 
      // labelCharactersFrom
      // 
      this.labelCharactersFrom.AutoSize = true;
      this.labelCharactersFrom.Location = new System.Drawing.Point( 235, 24 );
      this.labelCharactersFrom.Name = "labelCharactersFrom";
      this.labelCharactersFrom.Size = new System.Drawing.Size( 30, 13 );
      this.labelCharactersFrom.TabIndex = 0;
      this.labelCharactersFrom.Text = "from:";
      // 
      // checkExportToDataWrap
      // 
      this.checkExportToDataWrap.AutoSize = true;
      this.checkExportToDataWrap.Location = new System.Drawing.Point( 138, 79 );
      this.checkExportToDataWrap.Name = "checkExportToDataWrap";
      this.checkExportToDataWrap.Size = new System.Drawing.Size( 64, 17 );
      this.checkExportToDataWrap.TabIndex = 4;
      this.checkExportToDataWrap.Text = "Wrap at";
      this.checkExportToDataWrap.UseVisualStyleBackColor = true;
      this.checkExportToDataWrap.CheckedChanged += new System.EventHandler( this.checkExportToDataWrap_CheckedChanged );
      // 
      // checkIncludeColor
      // 
      this.checkIncludeColor.AutoSize = true;
      this.checkIncludeColor.Location = new System.Drawing.Point( 336, 52 );
      this.checkIncludeColor.Name = "checkIncludeColor";
      this.checkIncludeColor.Size = new System.Drawing.Size( 88, 17 );
      this.checkIncludeColor.TabIndex = 4;
      this.checkIncludeColor.Text = "Include Color";
      this.checkIncludeColor.UseVisualStyleBackColor = true;
      // 
      // checkExportToDataIncludeRes
      // 
      this.checkExportToDataIncludeRes.AutoSize = true;
      this.checkExportToDataIncludeRes.Location = new System.Drawing.Point( 138, 52 );
      this.checkExportToDataIncludeRes.Name = "checkExportToDataIncludeRes";
      this.checkExportToDataIncludeRes.Size = new System.Drawing.Size( 74, 17 );
      this.checkExportToDataIncludeRes.TabIndex = 4;
      this.checkExportToDataIncludeRes.Text = "Prefix with";
      this.checkExportToDataIncludeRes.UseVisualStyleBackColor = true;
      this.checkExportToDataIncludeRes.CheckedChanged += new System.EventHandler( this.checkExportToDataIncludeRes_CheckedChanged );
      // 
      // editDataExport
      // 
      this.editDataExport.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
                  | System.Windows.Forms.AnchorStyles.Right ) ) );
      this.editDataExport.Location = new System.Drawing.Point( 6, 118 );
      this.editDataExport.Multiline = true;
      this.editDataExport.Name = "editDataExport";
      this.editDataExport.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.editDataExport.Size = new System.Drawing.Size( 429, 148 );
      this.editDataExport.TabIndex = 3;
      this.editDataExport.WordWrap = false;
      this.editDataExport.KeyPress += new System.Windows.Forms.KeyPressEventHandler( this.editDataExport_KeyPress );
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point( 6, 77 );
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size( 117, 23 );
      this.button1.TabIndex = 2;
      this.button1.Text = "To Image...";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler( this.btnExportCharsetToImage_Click );
      // 
      // btnExportToData
      // 
      this.btnExportToData.Location = new System.Drawing.Point( 6, 48 );
      this.btnExportToData.Name = "btnExportToData";
      this.btnExportToData.Size = new System.Drawing.Size( 117, 23 );
      this.btnExportToData.TabIndex = 2;
      this.btnExportToData.Text = "To Data...";
      this.btnExportToData.UseVisualStyleBackColor = true;
      this.btnExportToData.Click += new System.EventHandler( this.btnExportCharsetToData_Click );
      // 
      // btnExportCharset
      // 
      this.btnExportCharset.Location = new System.Drawing.Point( 6, 19 );
      this.btnExportCharset.Name = "btnExportCharset";
      this.btnExportCharset.Size = new System.Drawing.Size( 117, 23 );
      this.btnExportCharset.TabIndex = 2;
      this.btnExportCharset.Text = "To File...";
      this.btnExportCharset.UseVisualStyleBackColor = true;
      this.btnExportCharset.Click += new System.EventHandler( this.btnExportCharset_Click );
      // 
      // tabCategories
      // 
      this.tabCategories.Controls.Add( this.groupAllCategories );
      this.tabCategories.Controls.Add( this.groupCategorySpecific );
      this.tabCategories.Controls.Add( this.btnDelete );
      this.tabCategories.Controls.Add( this.btnAddCategory );
      this.tabCategories.Controls.Add( this.listCategories );
      this.tabCategories.Controls.Add( this.editCategoryName );
      this.tabCategories.Controls.Add( this.label3 );
      this.tabCategories.Location = new System.Drawing.Point( 4, 22 );
      this.tabCategories.Name = "tabCategories";
      this.tabCategories.Size = new System.Drawing.Size( 1056, 477 );
      this.tabCategories.TabIndex = 2;
      this.tabCategories.Text = "Categories";
      this.tabCategories.UseVisualStyleBackColor = true;
      // 
      // groupAllCategories
      // 
      this.groupAllCategories.Controls.Add( this.btnSortCategories );
      this.groupAllCategories.Location = new System.Drawing.Point( 263, 112 );
      this.groupAllCategories.Name = "groupAllCategories";
      this.groupAllCategories.Size = new System.Drawing.Size( 255, 76 );
      this.groupAllCategories.TabIndex = 4;
      this.groupAllCategories.TabStop = false;
      this.groupAllCategories.Text = "All Categories";
      // 
      // btnSortCategories
      // 
      this.btnSortCategories.Location = new System.Drawing.Point( 6, 19 );
      this.btnSortCategories.Name = "btnSortCategories";
      this.btnSortCategories.Size = new System.Drawing.Size( 105, 23 );
      this.btnSortCategories.TabIndex = 3;
      this.btnSortCategories.Text = "Sort by Categories";
      this.btnSortCategories.UseVisualStyleBackColor = true;
      this.btnSortCategories.Click += new System.EventHandler( this.btnSortByCategory_Click );
      // 
      // groupCategorySpecific
      // 
      this.groupCategorySpecific.Controls.Add( this.label5 );
      this.groupCategorySpecific.Controls.Add( this.editCollapseIndex );
      this.groupCategorySpecific.Controls.Add( this.btnCollapseCategory );
      this.groupCategorySpecific.Controls.Add( this.btnReseatCategory );
      this.groupCategorySpecific.Location = new System.Drawing.Point( 263, 30 );
      this.groupCategorySpecific.Name = "groupCategorySpecific";
      this.groupCategorySpecific.Size = new System.Drawing.Size( 255, 76 );
      this.groupCategorySpecific.TabIndex = 4;
      this.groupCategorySpecific.TabStop = false;
      this.groupCategorySpecific.Text = "Selected Category";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point( 117, 52 );
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size( 47, 13 );
      this.label5.TabIndex = 6;
      this.label5.Text = "at index:";
      // 
      // editCollapseIndex
      // 
      this.editCollapseIndex.Location = new System.Drawing.Point( 180, 49 );
      this.editCollapseIndex.Name = "editCollapseIndex";
      this.editCollapseIndex.Size = new System.Drawing.Size( 69, 20 );
      this.editCollapseIndex.TabIndex = 5;
      // 
      // btnCollapseCategory
      // 
      this.btnCollapseCategory.Enabled = false;
      this.btnCollapseCategory.Location = new System.Drawing.Point( 6, 19 );
      this.btnCollapseCategory.Name = "btnCollapseCategory";
      this.btnCollapseCategory.Size = new System.Drawing.Size( 140, 23 );
      this.btnCollapseCategory.TabIndex = 3;
      this.btnCollapseCategory.Text = "Collapse Unique Chars";
      this.btnCollapseCategory.UseVisualStyleBackColor = true;
      this.btnCollapseCategory.Click += new System.EventHandler( this.btnCollapseCategory_Click );
      // 
      // btnReseatCategory
      // 
      this.btnReseatCategory.Enabled = false;
      this.btnReseatCategory.Location = new System.Drawing.Point( 6, 47 );
      this.btnReseatCategory.Name = "btnReseatCategory";
      this.btnReseatCategory.Size = new System.Drawing.Size( 105, 23 );
      this.btnReseatCategory.TabIndex = 3;
      this.btnReseatCategory.Text = "Reseat Category";
      this.btnReseatCategory.UseVisualStyleBackColor = true;
      this.btnReseatCategory.Click += new System.EventHandler( this.btnReseatCategory_Click );
      // 
      // btnDelete
      // 
      this.btnDelete.Enabled = false;
      this.btnDelete.Location = new System.Drawing.Point( 344, 1 );
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new System.Drawing.Size( 96, 23 );
      this.btnDelete.TabIndex = 3;
      this.btnDelete.Text = "Delete Category";
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new System.EventHandler( this.btnDelete_Click );
      // 
      // btnAddCategory
      // 
      this.btnAddCategory.Enabled = false;
      this.btnAddCategory.Location = new System.Drawing.Point( 263, 1 );
      this.btnAddCategory.Name = "btnAddCategory";
      this.btnAddCategory.Size = new System.Drawing.Size( 75, 23 );
      this.btnAddCategory.TabIndex = 3;
      this.btnAddCategory.Text = "Add";
      this.btnAddCategory.UseVisualStyleBackColor = true;
      this.btnAddCategory.Click += new System.EventHandler( this.btnAddCategory_Click );
      // 
      // listCategories
      // 
      this.listCategories.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2} );
      this.listCategories.FullRowSelect = true;
      this.listCategories.Location = new System.Drawing.Point( 11, 29 );
      this.listCategories.Name = "listCategories";
      this.listCategories.ShowGroups = false;
      this.listCategories.Size = new System.Drawing.Size( 246, 155 );
      this.listCategories.TabIndex = 2;
      this.listCategories.UseCompatibleStateImageBehavior = false;
      this.listCategories.View = System.Windows.Forms.View.Details;
      this.listCategories.SelectedIndexChanged += new System.EventHandler( this.listCategories_SelectedIndexChanged );
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
      this.editCategoryName.Location = new System.Drawing.Point( 83, 3 );
      this.editCategoryName.Name = "editCategoryName";
      this.editCategoryName.Size = new System.Drawing.Size( 174, 20 );
      this.editCategoryName.TabIndex = 1;
      this.editCategoryName.TextChanged += new System.EventHandler( this.editCategoryName_TextChanged );
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point( 8, 6 );
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size( 52, 13 );
      this.label3.TabIndex = 0;
      this.label3.Text = "Category:";
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem} );
      this.menuStrip1.Location = new System.Drawing.Point( 0, 0 );
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size( 1064, 24 );
      this.menuStrip1.TabIndex = 1;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.openCharsetProjectToolStripMenuItem,
            this.saveCharsetProjectToolStripMenuItem,
            this.closeCharsetProjectToolStripMenuItem,
            this.toolStripSeparator1,
            this.exchangeMultiColors1And2ToolStripMenuItem,
            this.exchangeMultiColor1AndBGColorToolStripMenuItem,
            this.exchangeMultiColor2AndBGColorToolStripMenuItem} );
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size( 75, 20 );
      this.fileToolStripMenuItem.Text = "&Characters";
      // 
      // openCharsetProjectToolStripMenuItem
      // 
      this.openCharsetProjectToolStripMenuItem.Name = "openCharsetProjectToolStripMenuItem";
      this.openCharsetProjectToolStripMenuItem.Size = new System.Drawing.Size( 265, 22 );
      this.openCharsetProjectToolStripMenuItem.Text = "&Open Charset Project...";
      this.openCharsetProjectToolStripMenuItem.Click += new System.EventHandler( this.openToolStripMenuItem_Click );
      // 
      // saveCharsetProjectToolStripMenuItem
      // 
      this.saveCharsetProjectToolStripMenuItem.Enabled = false;
      this.saveCharsetProjectToolStripMenuItem.Name = "saveCharsetProjectToolStripMenuItem";
      this.saveCharsetProjectToolStripMenuItem.Size = new System.Drawing.Size( 265, 22 );
      this.saveCharsetProjectToolStripMenuItem.Text = "&Save Project";
      this.saveCharsetProjectToolStripMenuItem.Click += new System.EventHandler( this.saveCharsetProjectToolStripMenuItem_Click );
      // 
      // closeCharsetProjectToolStripMenuItem
      // 
      this.closeCharsetProjectToolStripMenuItem.Enabled = false;
      this.closeCharsetProjectToolStripMenuItem.Name = "closeCharsetProjectToolStripMenuItem";
      this.closeCharsetProjectToolStripMenuItem.Size = new System.Drawing.Size( 265, 22 );
      this.closeCharsetProjectToolStripMenuItem.Text = "&Close Charset Project";
      this.closeCharsetProjectToolStripMenuItem.Click += new System.EventHandler( this.closeCharsetProjectToolStripMenuItem_Click );
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size( 262, 6 );
      // 
      // exchangeMultiColors1And2ToolStripMenuItem
      // 
      this.exchangeMultiColors1And2ToolStripMenuItem.Name = "exchangeMultiColors1And2ToolStripMenuItem";
      this.exchangeMultiColors1And2ToolStripMenuItem.Size = new System.Drawing.Size( 265, 22 );
      this.exchangeMultiColors1And2ToolStripMenuItem.Text = "Exchange Multi colors 1 and 2";
      this.exchangeMultiColors1And2ToolStripMenuItem.Click += new System.EventHandler( this.exchangeMultiColors1And2ToolStripMenuItem_Click );
      // 
      // exchangeMultiColor1AndBGColorToolStripMenuItem
      // 
      this.exchangeMultiColor1AndBGColorToolStripMenuItem.Name = "exchangeMultiColor1AndBGColorToolStripMenuItem";
      this.exchangeMultiColor1AndBGColorToolStripMenuItem.Size = new System.Drawing.Size( 265, 22 );
      this.exchangeMultiColor1AndBGColorToolStripMenuItem.Text = "Exchange Multi color 1 and BG color";
      this.exchangeMultiColor1AndBGColorToolStripMenuItem.Click += new System.EventHandler( this.exchangeMultiColor1AndBGColorToolStripMenuItem_Click );
      // 
      // exchangeMultiColor2AndBGColorToolStripMenuItem
      // 
      this.exchangeMultiColor2AndBGColorToolStripMenuItem.Name = "exchangeMultiColor2AndBGColorToolStripMenuItem";
      this.exchangeMultiColor2AndBGColorToolStripMenuItem.Size = new System.Drawing.Size( 265, 22 );
      this.exchangeMultiColor2AndBGColorToolStripMenuItem.Text = "Exchange Multi color 2 and BG color";
      this.exchangeMultiColor2AndBGColorToolStripMenuItem.Click += new System.EventHandler( this.exchangeMultiColor2AndBGColorToolStripMenuItem_Click );
      // 
      // CharsetEditor
      // 
      this.ClientSize = new System.Drawing.Size( 1064, 527 );
      this.Controls.Add( this.tabCharsetEditor );
      this.Controls.Add( this.menuStrip1 );
      this.Icon = ( (System.Drawing.Icon)( resources.GetObject( "$this.Icon" ) ) );
      this.Name = "CharsetEditor";
      this.Text = "Charset Editor";
      ( (System.ComponentModel.ISupportInitialize)( this.m_FileWatcher ) ).EndInit();
      this.tabCharsetEditor.ResumeLayout( false );
      this.tabEditor.ResumeLayout( false );
      this.tabEditor.PerformLayout();
      ( (System.ComponentModel.ISupportInitialize)( this.panelCharColors ) ).EndInit();
      ( (System.ComponentModel.ISupportInitialize)( this.picturePlayground ) ).EndInit();
      this.groupBox2.ResumeLayout( false );
      this.groupBox2.PerformLayout();
      this.contextMenuExchangeColors.ResumeLayout( false );
      ( (System.ComponentModel.ISupportInitialize)( this.pictureEditor ) ).EndInit();
      this.tabProject.ResumeLayout( false );
      this.groupBox1.ResumeLayout( false );
      this.groupExport.ResumeLayout( false );
      this.groupExport.PerformLayout();
      this.tabCategories.ResumeLayout( false );
      this.tabCategories.PerformLayout();
      this.groupAllCategories.ResumeLayout( false );
      this.groupCategorySpecific.ResumeLayout( false );
      this.groupCategorySpecific.PerformLayout();
      this.menuStrip1.ResumeLayout( false );
      this.menuStrip1.PerformLayout();
      this.ResumeLayout( false );
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TabControl tabCharsetEditor;
    private System.Windows.Forms.TabPage tabEditor;
    private System.Windows.Forms.TabPage tabProject;
    private GR.Forms.FastPictureBox pictureEditor;
    private System.Windows.Forms.ComboBox comboBackground;
    private System.Windows.Forms.RadioButton radioBackground;
    private System.Windows.Forms.RadioButton radioCharColor;
    private System.Windows.Forms.RadioButton radioMulticolor2;
    private System.Windows.Forms.RadioButton radioMultiColor1;
    private System.Windows.Forms.ComboBox comboCharColor;
    private System.Windows.Forms.ComboBox comboMulticolor2;
    private System.Windows.Forms.ComboBox comboMulticolor1;
    private GR.Forms.ImageListbox panelCharacters;
    private System.Windows.Forms.Label labelCharNo;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem openCharsetProjectToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem closeCharsetProjectToolStripMenuItem;
    private System.Windows.Forms.TextBox editCharactersFrom;
    private System.Windows.Forms.Label labelCharactersFrom;
    private System.Windows.Forms.ToolStripMenuItem saveCharsetProjectToolStripMenuItem;
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
    private System.Windows.Forms.Button btnDefaultUppercase;
    private System.Windows.Forms.Button btnDefaultLowerCase;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button btnPasteFromClipboard;
    private System.Windows.Forms.Button btnImportCharsetFromImage;
    private System.Windows.Forms.TabPage tabCategories;
    private System.Windows.Forms.TextBox editCategoryName;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.ListView listCategories;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnAddCategory;
    private System.Windows.Forms.ComboBox comboCategories;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Button btnMirrorY;
    private System.Windows.Forms.Button btnMirrorX;
    private System.Windows.Forms.Button btnShiftDown;
    private System.Windows.Forms.Button btnShiftUp;
    private System.Windows.Forms.Button btnShiftRight;
    private System.Windows.Forms.Button btnShiftLeft;
    private System.Windows.Forms.Button btnCollapseCategory;
    private System.Windows.Forms.Button btnPaste;
    private System.Windows.Forms.Button btnCopy;
    private System.Windows.Forms.Button btnReseatCategory;
    private System.Windows.Forms.GroupBox groupAllCategories;
    private System.Windows.Forms.GroupBox groupCategorySpecific;
    private System.Windows.Forms.TextBox editCollapseIndex;
    private System.Windows.Forms.Button btnSortCategories;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.CheckBox checkShowGrid;
    private System.Windows.Forms.Button btnInvert;
    private System.Windows.Forms.Button button3;
    private System.Windows.Forms.Button btnRotateLeft;
    private System.Windows.Forms.CheckBox checkPasteMultiColor;
    private System.Windows.Forms.ComboBox comboExportRange;
    private System.Windows.Forms.TextBox editCharactersCount;
    private System.Windows.Forms.Label labelCharactersTo;
    private System.Windows.Forms.CheckBox checkIncludeColor;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem exchangeMultiColors1And2ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exchangeMultiColor1AndBGColorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exchangeMultiColor2AndBGColorToolStripMenuItem;
    private MenuButton btnExchangeColors;
    private System.Windows.Forms.ContextMenuStrip contextMenuExchangeColors;
    private System.Windows.Forms.ToolStripMenuItem exchangeMultiColor1WithMultiColor2ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exchangeMultiColor1WithBGColorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exchangeMultiColor2WithBGColorToolStripMenuItem;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.Button btnClearChars;
    private System.Windows.Forms.RadioButton radioBGColor4;
    private System.Windows.Forms.ComboBox comboBGColor4;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox comboCharsetMode;
    private GR.Forms.FastPictureBox picturePlayground;
    private GR.Forms.FastPictureBox panelCharColors;



  }
}
