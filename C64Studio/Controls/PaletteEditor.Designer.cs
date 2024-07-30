namespace RetroDevStudio.Controls
{
  partial class PaletteEditor
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
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.tabPalette = new System.Windows.Forms.TabControl();
      this.tabEditor = new System.Windows.Forms.TabPage();
      this.groupBox4 = new System.Windows.Forms.GroupBox();
      this.comboSystem = new System.Windows.Forms.ComboBox();
      this.editPaletteName = new System.Windows.Forms.TextBox();
      this.label6 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.listPalette = new System.Windows.Forms.ListBox();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.panelColorPreview = new System.Windows.Forms.Panel();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.scrollB = new System.Windows.Forms.HScrollBar();
      this.scrollG = new System.Windows.Forms.HScrollBar();
      this.scrollR = new System.Windows.Forms.HScrollBar();
      this.editBHex = new System.Windows.Forms.TextBox();
      this.editB = new System.Windows.Forms.TextBox();
      this.editGHex = new System.Windows.Forms.TextBox();
      this.editG = new System.Windows.Forms.TextBox();
      this.editRHex = new System.Windows.Forms.TextBox();
      this.editR = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.tabImportExport = new System.Windows.Forms.TabPage();
      this.groupBox6 = new System.Windows.Forms.GroupBox();
      this.checkImportColorsSorted = new System.Windows.Forms.CheckBox();
      this.checkImportSwizzle = new System.Windows.Forms.CheckBox();
      this.editDataImport = new System.Windows.Forms.TextBox();
      this.groupBox5 = new System.Windows.Forms.GroupBox();
      this.comboPaletteExportFormat = new System.Windows.Forms.ComboBox();
      this.checkExportSwizzled = new System.Windows.Forms.CheckBox();
      this.checkExportHex = new System.Windows.Forms.CheckBox();
      this.editPrefix = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.editWrapByteCount = new System.Windows.Forms.TextBox();
      this.checkExportToDataWrap = new System.Windows.Forms.CheckBox();
      this.checkExportToDataIncludeRes = new System.Windows.Forms.CheckBox();
      this.editDataExport = new System.Windows.Forms.TextBox();
      this.paletteList = new RetroDevStudio.Controls.ArrangedItemList();
      this.btnImportFromAssembly = new DecentForms.Button();
      this.btnImportFromFile = new DecentForms.Button();
      this.btnExportToFile = new DecentForms.Button();
      this.btnExportToData = new DecentForms.Button();
      this.tabPalette.SuspendLayout();
      this.tabEditor.SuspendLayout();
      this.groupBox4.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.tabImportExport.SuspendLayout();
      this.groupBox6.SuspendLayout();
      this.groupBox5.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabPalette
      // 
      this.tabPalette.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tabPalette.Controls.Add(this.tabEditor);
      this.tabPalette.Controls.Add(this.tabImportExport);
      this.tabPalette.Location = new System.Drawing.Point(3, 3);
      this.tabPalette.Name = "tabPalette";
      this.tabPalette.SelectedIndex = 0;
      this.tabPalette.Size = new System.Drawing.Size(722, 426);
      this.tabPalette.TabIndex = 8;
      // 
      // tabEditor
      // 
      this.tabEditor.Controls.Add(this.groupBox4);
      this.tabEditor.Controls.Add(this.groupBox1);
      this.tabEditor.Controls.Add(this.groupBox3);
      this.tabEditor.Controls.Add(this.groupBox2);
      this.tabEditor.Location = new System.Drawing.Point(4, 22);
      this.tabEditor.Name = "tabEditor";
      this.tabEditor.Padding = new System.Windows.Forms.Padding(3);
      this.tabEditor.Size = new System.Drawing.Size(714, 400);
      this.tabEditor.TabIndex = 0;
      this.tabEditor.Text = "Palette";
      this.tabEditor.UseVisualStyleBackColor = true;
      // 
      // groupBox4
      // 
      this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.groupBox4.Controls.Add(this.comboSystem);
      this.groupBox4.Controls.Add(this.editPaletteName);
      this.groupBox4.Controls.Add(this.label6);
      this.groupBox4.Controls.Add(this.label4);
      this.groupBox4.Controls.Add(this.paletteList);
      this.groupBox4.Location = new System.Drawing.Point(6, 0);
      this.groupBox4.Name = "groupBox4";
      this.groupBox4.Size = new System.Drawing.Size(222, 375);
      this.groupBox4.TabIndex = 6;
      this.groupBox4.TabStop = false;
      this.groupBox4.Text = "Palettes";
      // 
      // comboSystem
      // 
      this.comboSystem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboSystem.FormattingEnabled = true;
      this.comboSystem.Location = new System.Drawing.Point(50, 332);
      this.comboSystem.Name = "comboSystem";
      this.comboSystem.Size = new System.Drawing.Size(166, 21);
      this.comboSystem.TabIndex = 10;
      this.comboSystem.SelectedIndexChanged += new System.EventHandler(this.comboSystem_SelectedIndexChanged);
      // 
      // editPaletteName
      // 
      this.editPaletteName.Location = new System.Drawing.Point(50, 302);
      this.editPaletteName.Name = "editPaletteName";
      this.editPaletteName.Size = new System.Drawing.Size(166, 20);
      this.editPaletteName.TabIndex = 9;
      this.editPaletteName.TextChanged += new System.EventHandler(this.editPaletteName_TextChanged);
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(6, 335);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(44, 13);
      this.label6.TabIndex = 8;
      this.label6.Text = "System:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(6, 306);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(38, 13);
      this.label4.TabIndex = 8;
      this.label4.Text = "Name:";
      // 
      // groupBox1
      // 
      this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox1.Controls.Add(this.listPalette);
      this.groupBox1.Location = new System.Drawing.Point(234, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(226, 375);
      this.groupBox1.TabIndex = 3;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Current Palette";
      // 
      // listPalette
      // 
      this.listPalette.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.listPalette.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.listPalette.FormattingEnabled = true;
      this.listPalette.ItemHeight = 16;
      this.listPalette.Location = new System.Drawing.Point(6, 19);
      this.listPalette.Name = "listPalette";
      this.listPalette.Size = new System.Drawing.Size(212, 340);
      this.listPalette.TabIndex = 2;
      this.listPalette.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listPalette_DrawItem);
      this.listPalette.SelectedIndexChanged += new System.EventHandler(this.listPalette_SelectedIndexChanged);
      // 
      // groupBox3
      // 
      this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox3.Controls.Add(this.panelColorPreview);
      this.groupBox3.Location = new System.Drawing.Point(466, 182);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(238, 66);
      this.groupBox3.TabIndex = 5;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Preview";
      // 
      // panelColorPreview
      // 
      this.panelColorPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panelColorPreview.Location = new System.Drawing.Point(6, 19);
      this.panelColorPreview.Name = "panelColorPreview";
      this.panelColorPreview.Size = new System.Drawing.Size(226, 38);
      this.panelColorPreview.TabIndex = 0;
      this.panelColorPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.panelColorPreview_Paint);
      // 
      // groupBox2
      // 
      this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox2.Controls.Add(this.scrollB);
      this.groupBox2.Controls.Add(this.scrollG);
      this.groupBox2.Controls.Add(this.scrollR);
      this.groupBox2.Controls.Add(this.editBHex);
      this.groupBox2.Controls.Add(this.editB);
      this.groupBox2.Controls.Add(this.editGHex);
      this.groupBox2.Controls.Add(this.editG);
      this.groupBox2.Controls.Add(this.editRHex);
      this.groupBox2.Controls.Add(this.editR);
      this.groupBox2.Controls.Add(this.label3);
      this.groupBox2.Controls.Add(this.label2);
      this.groupBox2.Controls.Add(this.label1);
      this.groupBox2.Location = new System.Drawing.Point(466, 0);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(238, 175);
      this.groupBox2.TabIndex = 4;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Current Color";
      // 
      // scrollB
      // 
      this.scrollB.Location = new System.Drawing.Point(18, 146);
      this.scrollB.Maximum = 255;
      this.scrollB.Name = "scrollB";
      this.scrollB.Size = new System.Drawing.Size(214, 17);
      this.scrollB.TabIndex = 3;
      this.scrollB.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scrollB_Scroll);
      // 
      // scrollG
      // 
      this.scrollG.Location = new System.Drawing.Point(18, 97);
      this.scrollG.Maximum = 255;
      this.scrollG.Name = "scrollG";
      this.scrollG.Size = new System.Drawing.Size(214, 17);
      this.scrollG.TabIndex = 3;
      this.scrollG.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scrollG_Scroll);
      // 
      // scrollR
      // 
      this.scrollR.Location = new System.Drawing.Point(18, 48);
      this.scrollR.Maximum = 255;
      this.scrollR.Name = "scrollR";
      this.scrollR.Size = new System.Drawing.Size(214, 17);
      this.scrollR.TabIndex = 3;
      this.scrollR.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scrollR_Scroll);
      // 
      // editBHex
      // 
      this.editBHex.Location = new System.Drawing.Point(129, 123);
      this.editBHex.MaxLength = 2;
      this.editBHex.Name = "editBHex";
      this.editBHex.Size = new System.Drawing.Size(49, 20);
      this.editBHex.TabIndex = 2;
      this.editBHex.TextChanged += new System.EventHandler(this.editBHex_TextChanged);
      // 
      // editB
      // 
      this.editB.Location = new System.Drawing.Point(60, 123);
      this.editB.Name = "editB";
      this.editB.Size = new System.Drawing.Size(49, 20);
      this.editB.TabIndex = 2;
      this.editB.TextChanged += new System.EventHandler(this.editB_TextChanged);
      // 
      // editGHex
      // 
      this.editGHex.Location = new System.Drawing.Point(129, 74);
      this.editGHex.MaxLength = 2;
      this.editGHex.Name = "editGHex";
      this.editGHex.Size = new System.Drawing.Size(49, 20);
      this.editGHex.TabIndex = 2;
      this.editGHex.TextChanged += new System.EventHandler(this.editGHex_TextChanged);
      // 
      // editG
      // 
      this.editG.Location = new System.Drawing.Point(60, 74);
      this.editG.Name = "editG";
      this.editG.Size = new System.Drawing.Size(49, 20);
      this.editG.TabIndex = 2;
      this.editG.TextChanged += new System.EventHandler(this.editG_TextChanged);
      // 
      // editRHex
      // 
      this.editRHex.Location = new System.Drawing.Point(129, 22);
      this.editRHex.MaxLength = 2;
      this.editRHex.Name = "editRHex";
      this.editRHex.Size = new System.Drawing.Size(49, 20);
      this.editRHex.TabIndex = 2;
      this.editRHex.TextChanged += new System.EventHandler(this.editRHex_TextChanged);
      // 
      // editR
      // 
      this.editR.Location = new System.Drawing.Point(60, 22);
      this.editR.Name = "editR";
      this.editR.Size = new System.Drawing.Size(49, 20);
      this.editR.TabIndex = 2;
      this.editR.TextChanged += new System.EventHandler(this.editR_TextChanged);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(16, 126);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(31, 13);
      this.label3.TabIndex = 0;
      this.label3.Text = "Blue:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(16, 77);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(39, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "Green:";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(16, 25);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(30, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Red:";
      // 
      // tabImportExport
      // 
      this.tabImportExport.Controls.Add(this.groupBox6);
      this.tabImportExport.Controls.Add(this.groupBox5);
      this.tabImportExport.Location = new System.Drawing.Point(4, 22);
      this.tabImportExport.Name = "tabImportExport";
      this.tabImportExport.Padding = new System.Windows.Forms.Padding(3);
      this.tabImportExport.Size = new System.Drawing.Size(714, 400);
      this.tabImportExport.TabIndex = 1;
      this.tabImportExport.Text = "Import/Export";
      this.tabImportExport.UseVisualStyleBackColor = true;
      // 
      // groupBox6
      // 
      this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox6.Controls.Add(this.checkImportColorsSorted);
      this.groupBox6.Controls.Add(this.checkImportSwizzle);
      this.groupBox6.Controls.Add(this.btnImportFromAssembly);
      this.groupBox6.Controls.Add(this.btnImportFromFile);
      this.groupBox6.Controls.Add(this.editDataImport);
      this.groupBox6.Location = new System.Drawing.Point(318, 6);
      this.groupBox6.Name = "groupBox6";
      this.groupBox6.Size = new System.Drawing.Size(390, 388);
      this.groupBox6.TabIndex = 1;
      this.groupBox6.TabStop = false;
      this.groupBox6.Text = "Import";
      // 
      // checkImportColorsSorted
      // 
      this.checkImportColorsSorted.AutoSize = true;
      this.checkImportColorsSorted.Checked = true;
      this.checkImportColorsSorted.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkImportColorsSorted.Location = new System.Drawing.Point(213, 19);
      this.checkImportColorsSorted.Name = "checkImportColorsSorted";
      this.checkImportColorsSorted.Size = new System.Drawing.Size(57, 17);
      this.checkImportColorsSorted.TabIndex = 27;
      this.checkImportColorsSorted.Text = "Sorted";
      this.checkImportColorsSorted.UseVisualStyleBackColor = true;
      // 
      // checkImportSwizzle
      // 
      this.checkImportSwizzle.AutoSize = true;
      this.checkImportSwizzle.Checked = true;
      this.checkImportSwizzle.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkImportSwizzle.Location = new System.Drawing.Point(129, 19);
      this.checkImportSwizzle.Name = "checkImportSwizzle";
      this.checkImportSwizzle.Size = new System.Drawing.Size(78, 17);
      this.checkImportSwizzle.TabIndex = 27;
      this.checkImportSwizzle.Text = "De-Swizzle";
      this.checkImportSwizzle.UseVisualStyleBackColor = true;
      // 
      // editDataImport
      // 
      this.editDataImport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.editDataImport.Location = new System.Drawing.Point(6, 94);
      this.editDataImport.Multiline = true;
      this.editDataImport.Name = "editDataImport";
      this.editDataImport.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.editDataImport.Size = new System.Drawing.Size(378, 288);
      this.editDataImport.TabIndex = 29;
      this.editDataImport.WordWrap = false;
      // 
      // groupBox5
      // 
      this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.groupBox5.Controls.Add(this.comboPaletteExportFormat);
      this.groupBox5.Controls.Add(this.checkExportSwizzled);
      this.groupBox5.Controls.Add(this.checkExportHex);
      this.groupBox5.Controls.Add(this.editPrefix);
      this.groupBox5.Controls.Add(this.label5);
      this.groupBox5.Controls.Add(this.editWrapByteCount);
      this.groupBox5.Controls.Add(this.checkExportToDataWrap);
      this.groupBox5.Controls.Add(this.checkExportToDataIncludeRes);
      this.groupBox5.Controls.Add(this.editDataExport);
      this.groupBox5.Controls.Add(this.btnExportToFile);
      this.groupBox5.Controls.Add(this.btnExportToData);
      this.groupBox5.Location = new System.Drawing.Point(6, 6);
      this.groupBox5.Name = "groupBox5";
      this.groupBox5.Size = new System.Drawing.Size(306, 388);
      this.groupBox5.TabIndex = 0;
      this.groupBox5.TabStop = false;
      this.groupBox5.Text = "Export";
      // 
      // comboPaletteExportFormat
      // 
      this.comboPaletteExportFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboPaletteExportFormat.FormattingEnabled = true;
      this.comboPaletteExportFormat.Location = new System.Drawing.Point(118, 92);
      this.comboPaletteExportFormat.Name = "comboPaletteExportFormat";
      this.comboPaletteExportFormat.Size = new System.Drawing.Size(139, 21);
      this.comboPaletteExportFormat.TabIndex = 30;
      // 
      // checkExportSwizzled
      // 
      this.checkExportSwizzled.AutoSize = true;
      this.checkExportSwizzled.Checked = true;
      this.checkExportSwizzled.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkExportSwizzled.Location = new System.Drawing.Point(214, 42);
      this.checkExportSwizzled.Name = "checkExportSwizzled";
      this.checkExportSwizzled.Size = new System.Drawing.Size(61, 17);
      this.checkExportSwizzled.TabIndex = 27;
      this.checkExportSwizzled.Text = "Swizzle";
      this.checkExportSwizzled.UseVisualStyleBackColor = true;
      // 
      // checkExportHex
      // 
      this.checkExportHex.AutoSize = true;
      this.checkExportHex.Checked = true;
      this.checkExportHex.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkExportHex.Location = new System.Drawing.Point(118, 42);
      this.checkExportHex.Name = "checkExportHex";
      this.checkExportHex.Size = new System.Drawing.Size(92, 17);
      this.checkExportHex.TabIndex = 27;
      this.checkExportHex.Text = "Export as Hex";
      this.checkExportHex.UseVisualStyleBackColor = true;
      // 
      // editPrefix
      // 
      this.editPrefix.Location = new System.Drawing.Point(214, 17);
      this.editPrefix.Name = "editPrefix";
      this.editPrefix.Size = new System.Drawing.Size(43, 20);
      this.editPrefix.TabIndex = 23;
      this.editPrefix.Text = "!byte ";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(235, 70);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(32, 13);
      this.label5.TabIndex = 26;
      this.label5.Text = "bytes";
      // 
      // editWrapByteCount
      // 
      this.editWrapByteCount.Enabled = false;
      this.editWrapByteCount.Location = new System.Drawing.Point(188, 66);
      this.editWrapByteCount.Name = "editWrapByteCount";
      this.editWrapByteCount.Size = new System.Drawing.Size(41, 20);
      this.editWrapByteCount.TabIndex = 25;
      this.editWrapByteCount.Text = "40";
      // 
      // checkExportToDataWrap
      // 
      this.checkExportToDataWrap.AutoSize = true;
      this.checkExportToDataWrap.Checked = true;
      this.checkExportToDataWrap.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkExportToDataWrap.Location = new System.Drawing.Point(118, 69);
      this.checkExportToDataWrap.Name = "checkExportToDataWrap";
      this.checkExportToDataWrap.Size = new System.Drawing.Size(64, 17);
      this.checkExportToDataWrap.TabIndex = 24;
      this.checkExportToDataWrap.Text = "Wrap at";
      this.checkExportToDataWrap.UseVisualStyleBackColor = true;
      // 
      // checkExportToDataIncludeRes
      // 
      this.checkExportToDataIncludeRes.AutoSize = true;
      this.checkExportToDataIncludeRes.Checked = true;
      this.checkExportToDataIncludeRes.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkExportToDataIncludeRes.Location = new System.Drawing.Point(118, 19);
      this.checkExportToDataIncludeRes.Name = "checkExportToDataIncludeRes";
      this.checkExportToDataIncludeRes.Size = new System.Drawing.Size(74, 17);
      this.checkExportToDataIncludeRes.TabIndex = 22;
      this.checkExportToDataIncludeRes.Text = "Prefix with";
      this.checkExportToDataIncludeRes.UseVisualStyleBackColor = true;
      // 
      // editDataExport
      // 
      this.editDataExport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.editDataExport.Location = new System.Drawing.Point(0, 119);
      this.editDataExport.Multiline = true;
      this.editDataExport.Name = "editDataExport";
      this.editDataExport.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.editDataExport.Size = new System.Drawing.Size(300, 263);
      this.editDataExport.TabIndex = 29;
      this.editDataExport.WordWrap = false;
      // 
      // paletteList
      // 
      this.paletteList.AddButtonEnabled = true;
      this.paletteList.AllowClone = true;
      this.paletteList.DeleteButtonEnabled = false;
      this.paletteList.HasOwnerDrawColumn = true;
      this.paletteList.HighlightColor = System.Drawing.SystemColors.HotTrack;
      this.paletteList.HighlightTextColor = System.Drawing.SystemColors.HighlightText;
      this.paletteList.Location = new System.Drawing.Point(6, 19);
      this.paletteList.MoveDownButtonEnabled = false;
      this.paletteList.MoveUpButtonEnabled = false;
      this.paletteList.MustHaveOneElement = true;
      this.paletteList.Name = "paletteList";
      this.paletteList.SelectedIndex = -1;
      this.paletteList.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      this.paletteList.SelectionTextColor = System.Drawing.SystemColors.HighlightText;
      this.paletteList.Size = new System.Drawing.Size(210, 277);
      this.paletteList.TabIndex = 7;
      this.paletteList.AddingItem += new RetroDevStudio.Controls.ArrangedItemList.AddingItemEventHandler(this.paletteList_AddingItem);
      this.paletteList.CloningItem += new RetroDevStudio.Controls.ArrangedItemList.CloningItemEventHandler(this.paletteList_CloningItem);
      this.paletteList.RemovingItem += new RetroDevStudio.Controls.ArrangedItemList.RemovingItemEventHandler(this.paletteList_RemovingItem);
      this.paletteList.ItemRemoved += new RetroDevStudio.Controls.ArrangedItemList.ItemModifiedEventHandler(this.paletteList_ItemRemoved);
      this.paletteList.ItemMoved += new RetroDevStudio.Controls.ArrangedItemList.ItemExchangedEventHandler(this.paletteList_ItemMoved);
      this.paletteList.SelectedIndexChanged += new RetroDevStudio.Controls.ArrangedItemList.ItemModifiedEventHandler(this.paletteList_SelectedIndexChanged);
      // 
      // btnImportFromAssembly
      // 
      this.btnImportFromAssembly.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnImportFromAssembly.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnImportFromAssembly.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnImportFromAssembly.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnImportFromAssembly.Image = null;
      this.btnImportFromAssembly.Location = new System.Drawing.Point(6, 44);
      this.btnImportFromAssembly.Name = "btnImportFromAssembly";
      this.btnImportFromAssembly.Size = new System.Drawing.Size(117, 23);
      this.btnImportFromAssembly.TabIndex = 1;
      this.btnImportFromAssembly.Text = "From assembly";
      // 
      // btnImportFromFile
      // 
      this.btnImportFromFile.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnImportFromFile.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnImportFromFile.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnImportFromFile.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnImportFromFile.Image = null;
      this.btnImportFromFile.Location = new System.Drawing.Point(6, 15);
      this.btnImportFromFile.Name = "btnImportFromFile";
      this.btnImportFromFile.Size = new System.Drawing.Size(117, 23);
      this.btnImportFromFile.TabIndex = 1;
      this.btnImportFromFile.Text = "From File...";
      // 
      // btnExportToFile
      // 
      this.btnExportToFile.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnExportToFile.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnExportToFile.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnExportToFile.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnExportToFile.Image = null;
      this.btnExportToFile.Location = new System.Drawing.Point(6, 44);
      this.btnExportToFile.Name = "btnExportToFile";
      this.btnExportToFile.Size = new System.Drawing.Size(106, 23);
      this.btnExportToFile.TabIndex = 28;
      this.btnExportToFile.Text = "as binary file";
      // 
      // btnExportToData
      // 
      this.btnExportToData.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnExportToData.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnExportToData.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnExportToData.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnExportToData.Image = null;
      this.btnExportToData.Location = new System.Drawing.Point(6, 15);
      this.btnExportToData.Name = "btnExportToData";
      this.btnExportToData.Size = new System.Drawing.Size(106, 23);
      this.btnExportToData.TabIndex = 21;
      this.btnExportToData.Text = "as assembly source";
      // 
      // PaletteEditor
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.Controls.Add(this.tabPalette);
      this.Name = "PaletteEditor";
      this.Size = new System.Drawing.Size(725, 429);
      this.tabPalette.ResumeLayout(false);
      this.tabEditor.ResumeLayout(false);
      this.groupBox4.ResumeLayout(false);
      this.groupBox4.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox3.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.tabImportExport.ResumeLayout(false);
      this.groupBox6.ResumeLayout(false);
      this.groupBox6.PerformLayout();
      this.groupBox5.ResumeLayout(false);
      this.groupBox5.PerformLayout();
      this.ResumeLayout(false);

    }

        #endregion
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.TabControl tabPalette;
    private System.Windows.Forms.TabPage tabEditor;
    private System.Windows.Forms.GroupBox groupBox4;
    private System.Windows.Forms.TextBox editPaletteName;
    private System.Windows.Forms.Label label4;
    private ArrangedItemList paletteList;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.ListBox listPalette;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.Panel panelColorPreview;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.HScrollBar scrollB;
    private System.Windows.Forms.HScrollBar scrollG;
    private System.Windows.Forms.HScrollBar scrollR;
    private System.Windows.Forms.TextBox editBHex;
    private System.Windows.Forms.TextBox editB;
    private System.Windows.Forms.TextBox editGHex;
    private System.Windows.Forms.TextBox editG;
    private System.Windows.Forms.TextBox editRHex;
    private System.Windows.Forms.TextBox editR;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TabPage tabImportExport;
    private System.Windows.Forms.GroupBox groupBox6;
    private System.Windows.Forms.CheckBox checkImportColorsSorted;
    private System.Windows.Forms.CheckBox checkImportSwizzle;
    private DecentForms.Button btnImportFromAssembly;
    private DecentForms.Button btnImportFromFile;
    private System.Windows.Forms.TextBox editDataImport;
    private System.Windows.Forms.GroupBox groupBox5;
    private System.Windows.Forms.ComboBox comboPaletteExportFormat;
    private System.Windows.Forms.CheckBox checkExportSwizzled;
    private System.Windows.Forms.CheckBox checkExportHex;
    private System.Windows.Forms.TextBox editPrefix;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox editWrapByteCount;
    private System.Windows.Forms.CheckBox checkExportToDataWrap;
    private System.Windows.Forms.CheckBox checkExportToDataIncludeRes;
    private System.Windows.Forms.TextBox editDataExport;
    private DecentForms.Button btnExportToFile;
    private DecentForms.Button btnExportToData;
    private System.Windows.Forms.ComboBox comboSystem;
    private System.Windows.Forms.Label label6;
  }
}
