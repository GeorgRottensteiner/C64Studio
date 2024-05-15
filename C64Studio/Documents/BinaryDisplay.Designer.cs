namespace RetroDevStudio.Documents
{
  partial class BinaryDisplay
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BinaryDisplay));
      this.tabMain = new System.Windows.Forms.TabControl();
      this.tabData = new System.Windows.Forms.TabPage();
      this.hexView = new Be.Windows.Forms.HexBox();
      this.tabModify = new System.Windows.Forms.TabPage();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.editDivideBy = new System.Windows.Forms.TextBox();
      this.editDeleteNthByte = new System.Windows.Forms.TextBox();
      this.btnExport = new DecentForms.Button();
      this.btnImport = new DecentForms.Button();
      this.btnSwizzle = new DecentForms.Button();
      this.btnDivide = new DecentForms.Button();
      this.btnPackNibble = new DecentForms.Button();
      this.btnDeleteNthByte = new DecentForms.Button();
      this.btnUpsize = new DecentForms.Button();
      this.btnInterleave = new DecentForms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.label4 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.editWrapCharsCount = new System.Windows.Forms.TextBox();
      this.editWrapCount = new System.Windows.Forms.TextBox();
      this.editToBASICLineDelta = new System.Windows.Forms.TextBox();
      this.btnFromBASICHex = new DecentForms.Button();
      this.btnFromBASIC = new DecentForms.Button();
      this.editToBASICStartLine = new System.Windows.Forms.TextBox();
      this.button2 = new DecentForms.Button();
      this.btnFromASM = new DecentForms.Button();
      this.button1 = new DecentForms.Button();
      this.btnToBASICHex = new DecentForms.Button();
      this.btnToBASIC = new DecentForms.Button();
      this.btnToText = new DecentForms.Button();
      this.textBinaryData = new System.Windows.Forms.TextBox();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.dataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.importFromFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exportToFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.modifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.interleaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.checkInsertSpaces = new System.Windows.Forms.CheckBox();
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      this.tabMain.SuspendLayout();
      this.tabData.SuspendLayout();
      this.tabModify.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.menuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabMain
      // 
      this.tabMain.Controls.Add(this.tabData);
      this.tabMain.Controls.Add(this.tabModify);
      this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabMain.Location = new System.Drawing.Point(0, 24);
      this.tabMain.Name = "tabMain";
      this.tabMain.SelectedIndex = 0;
      this.tabMain.Size = new System.Drawing.Size(733, 514);
      this.tabMain.TabIndex = 0;
      // 
      // tabData
      // 
      this.tabData.Controls.Add(this.hexView);
      this.tabData.Location = new System.Drawing.Point(4, 22);
      this.tabData.Name = "tabData";
      this.tabData.Padding = new System.Windows.Forms.Padding(3);
      this.tabData.Size = new System.Drawing.Size(725, 488);
      this.tabData.TabIndex = 0;
      this.tabData.Text = "Data";
      this.tabData.UseVisualStyleBackColor = true;
      // 
      // hexView
      // 
      this.hexView.ColumnInfoVisible = true;
      this.hexView.CustomHexViewer = null;
      this.hexView.DisplayedAddressOffset = ((long)(0));
      this.hexView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.hexView.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.hexView.InfoForeColor = System.Drawing.SystemColors.AppWorkspace;
      this.hexView.LineInfoVisible = true;
      this.hexView.Location = new System.Drawing.Point(3, 3);
      this.hexView.MarkedForeColor = System.Drawing.Color.Empty;
      this.hexView.Name = "hexView";
      this.hexView.NumDigitsMemorySize = 8;
      this.hexView.SelectedByteProvider = null;
      this.hexView.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
      this.hexView.Size = new System.Drawing.Size(719, 482);
      this.hexView.StringViewVisible = true;
      this.hexView.TabIndex = 0;
      this.hexView.TextFont = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.hexView.VScrollBarVisible = true;
      // 
      // tabModify
      // 
      this.tabModify.Controls.Add(this.groupBox2);
      this.tabModify.Controls.Add(this.groupBox1);
      this.tabModify.Location = new System.Drawing.Point(4, 22);
      this.tabModify.Name = "tabModify";
      this.tabModify.Padding = new System.Windows.Forms.Padding(3);
      this.tabModify.Size = new System.Drawing.Size(725, 488);
      this.tabModify.TabIndex = 1;
      this.tabModify.Text = "Modify";
      this.tabModify.UseVisualStyleBackColor = true;
      // 
      // groupBox2
      // 
      this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox2.Controls.Add(this.editDivideBy);
      this.groupBox2.Controls.Add(this.editDeleteNthByte);
      this.groupBox2.Controls.Add(this.btnExport);
      this.groupBox2.Controls.Add(this.btnImport);
      this.groupBox2.Controls.Add(this.btnSwizzle);
      this.groupBox2.Controls.Add(this.btnDivide);
      this.groupBox2.Controls.Add(this.btnPackNibble);
      this.groupBox2.Controls.Add(this.btnDeleteNthByte);
      this.groupBox2.Controls.Add(this.btnUpsize);
      this.groupBox2.Controls.Add(this.btnInterleave);
      this.groupBox2.Location = new System.Drawing.Point(528, 6);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(189, 449);
      this.groupBox2.TabIndex = 1;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Actions";
      // 
      // editDivideBy
      // 
      this.editDivideBy.Location = new System.Drawing.Point(87, 194);
      this.editDivideBy.Name = "editDivideBy";
      this.editDivideBy.Size = new System.Drawing.Size(75, 20);
      this.editDivideBy.TabIndex = 9;
      this.editDivideBy.Text = "2";
      // 
      // editDeleteNthByte
      // 
      this.editDeleteNthByte.Location = new System.Drawing.Point(87, 108);
      this.editDeleteNthByte.Name = "editDeleteNthByte";
      this.editDeleteNthByte.Size = new System.Drawing.Size(75, 20);
      this.editDeleteNthByte.TabIndex = 5;
      this.editDeleteNthByte.Text = "2";
      // 
      // btnExport
      // 
      this.btnExport.Location = new System.Drawing.Point(87, 19);
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new System.Drawing.Size(75, 23);
      this.btnExport.TabIndex = 1;
      this.btnExport.Text = "Export";
      this.btnExport.Click += new DecentForms.EventHandler(this.btnExport_Click);
      // 
      // btnImport
      // 
      this.btnImport.Location = new System.Drawing.Point(6, 19);
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = new System.Drawing.Size(75, 23);
      this.btnImport.TabIndex = 0;
      this.btnImport.Text = "Import";
      this.btnImport.Click += new DecentForms.EventHandler(this.btnImport_Click);
      // 
      // btnSwizzle
      // 
      this.btnSwizzle.Location = new System.Drawing.Point(6, 163);
      this.btnSwizzle.Name = "btnSwizzle";
      this.btnSwizzle.Size = new System.Drawing.Size(75, 23);
      this.btnSwizzle.TabIndex = 7;
      this.btnSwizzle.Text = "Swizzle";
      this.btnSwizzle.Click += new DecentForms.EventHandler(this.btnSwizzle_Click);
      // 
      // btnDivide
      // 
      this.btnDivide.Location = new System.Drawing.Point(6, 192);
      this.btnDivide.Name = "btnDivide";
      this.btnDivide.Size = new System.Drawing.Size(75, 23);
      this.btnDivide.TabIndex = 8;
      this.btnDivide.Text = "Divide by";
      this.btnDivide.Click += new DecentForms.EventHandler(this.btnDivide_Click);
      // 
      // btnPackNibble
      // 
      this.btnPackNibble.Location = new System.Drawing.Point(6, 134);
      this.btnPackNibble.Name = "btnPackNibble";
      this.btnPackNibble.Size = new System.Drawing.Size(75, 23);
      this.btnPackNibble.TabIndex = 6;
      this.btnPackNibble.Text = "Pack Nibble";
      this.btnPackNibble.Click += new DecentForms.EventHandler(this.btnPackNibbles_Click);
      // 
      // btnDeleteNthByte
      // 
      this.btnDeleteNthByte.Location = new System.Drawing.Point(6, 106);
      this.btnDeleteNthByte.Name = "btnDeleteNthByte";
      this.btnDeleteNthByte.Size = new System.Drawing.Size(75, 23);
      this.btnDeleteNthByte.TabIndex = 4;
      this.btnDeleteNthByte.Text = "Delete nth";
      this.btnDeleteNthByte.Click += new DecentForms.EventHandler(this.btnDeleteNthByte_Click);
      // 
      // btnUpsize
      // 
      this.btnUpsize.Location = new System.Drawing.Point(6, 77);
      this.btnUpsize.Name = "btnUpsize";
      this.btnUpsize.Size = new System.Drawing.Size(75, 23);
      this.btnUpsize.TabIndex = 3;
      this.btnUpsize.Text = "Upsize";
      this.btnUpsize.Click += new DecentForms.EventHandler(this.btnUpsize_Click);
      // 
      // btnInterleave
      // 
      this.btnInterleave.Location = new System.Drawing.Point(6, 48);
      this.btnInterleave.Name = "btnInterleave";
      this.btnInterleave.Size = new System.Drawing.Size(75, 23);
      this.btnInterleave.TabIndex = 2;
      this.btnInterleave.Text = "Interleave...";
      this.btnInterleave.Click += new DecentForms.EventHandler(this.btnInterleave_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.groupBox1.Controls.Add(this.checkInsertSpaces);
      this.groupBox1.Controls.Add(this.label4);
      this.groupBox1.Controls.Add(this.label3);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.editWrapCharsCount);
      this.groupBox1.Controls.Add(this.editWrapCount);
      this.groupBox1.Controls.Add(this.editToBASICLineDelta);
      this.groupBox1.Controls.Add(this.btnFromBASICHex);
      this.groupBox1.Controls.Add(this.btnFromBASIC);
      this.groupBox1.Controls.Add(this.editToBASICStartLine);
      this.groupBox1.Controls.Add(this.button2);
      this.groupBox1.Controls.Add(this.btnFromASM);
      this.groupBox1.Controls.Add(this.button1);
      this.groupBox1.Controls.Add(this.btnToBASICHex);
      this.groupBox1.Controls.Add(this.btnToBASIC);
      this.groupBox1.Controls.Add(this.btnToText);
      this.groupBox1.Controls.Add(this.textBinaryData);
      this.groupBox1.Location = new System.Drawing.Point(6, 6);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(516, 449);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Text Data";
      // 
      // label4
      // 
      this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(402, 254);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(97, 13);
      this.label4.TabIndex = 3;
      this.label4.Text = "Wrap Chars Count:";
      // 
      // label3
      // 
      this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(402, 215);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(96, 13);
      this.label3.TabIndex = 3;
      this.label3.Text = "Wrap Bytes Count:";
      // 
      // label2
      // 
      this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(402, 176);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(55, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Line Step:";
      // 
      // label1
      // 
      this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(402, 134);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(55, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Start Line:";
      // 
      // editWrapCharsCount
      // 
      this.editWrapCharsCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.editWrapCharsCount.Location = new System.Drawing.Point(401, 270);
      this.editWrapCharsCount.Name = "editWrapCharsCount";
      this.editWrapCharsCount.Size = new System.Drawing.Size(75, 20);
      this.editWrapCharsCount.TabIndex = 2;
      this.editWrapCharsCount.Text = "40";
      // 
      // editWrapCount
      // 
      this.editWrapCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.editWrapCount.Location = new System.Drawing.Point(401, 231);
      this.editWrapCount.Name = "editWrapCount";
      this.editWrapCount.Size = new System.Drawing.Size(75, 20);
      this.editWrapCount.TabIndex = 2;
      this.editWrapCount.Text = "40";
      // 
      // editToBASICLineDelta
      // 
      this.editToBASICLineDelta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.editToBASICLineDelta.Location = new System.Drawing.Point(401, 192);
      this.editToBASICLineDelta.Name = "editToBASICLineDelta";
      this.editToBASICLineDelta.Size = new System.Drawing.Size(75, 20);
      this.editToBASICLineDelta.TabIndex = 2;
      this.editToBASICLineDelta.Text = "10";
      // 
      // btnFromBASICHex
      // 
      this.btnFromBASICHex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnFromBASICHex.Location = new System.Drawing.Point(401, 420);
      this.btnFromBASICHex.Name = "btnFromBASICHex";
      this.btnFromBASICHex.Size = new System.Drawing.Size(109, 23);
      this.btnFromBASICHex.TabIndex = 1;
      this.btnFromBASICHex.Text = "From BASIC Hex";
      this.btnFromBASICHex.Click += new DecentForms.EventHandler(this.btnFromBASICHex_Click);
      // 
      // btnFromBASIC
      // 
      this.btnFromBASIC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnFromBASIC.Location = new System.Drawing.Point(401, 391);
      this.btnFromBASIC.Name = "btnFromBASIC";
      this.btnFromBASIC.Size = new System.Drawing.Size(109, 23);
      this.btnFromBASIC.TabIndex = 1;
      this.btnFromBASIC.Text = "From BASIC";
      this.btnFromBASIC.Click += new DecentForms.EventHandler(this.btnFromBASIC_Click);
      // 
      // editToBASICStartLine
      // 
      this.editToBASICStartLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.editToBASICStartLine.Location = new System.Drawing.Point(401, 153);
      this.editToBASICStartLine.Name = "editToBASICStartLine";
      this.editToBASICStartLine.Size = new System.Drawing.Size(75, 20);
      this.editToBASICStartLine.TabIndex = 2;
      this.editToBASICStartLine.Text = "10";
      // 
      // button2
      // 
      this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.button2.Location = new System.Drawing.Point(401, 362);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(109, 23);
      this.button2.TabIndex = 1;
      this.button2.Text = "From Hex";
      this.button2.Click += new DecentForms.EventHandler(this.btnFromHex_Click);
      // 
      // btnFromASM
      // 
      this.btnFromASM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnFromASM.Location = new System.Drawing.Point(401, 333);
      this.btnFromASM.Name = "btnFromASM";
      this.btnFromASM.Size = new System.Drawing.Size(109, 23);
      this.btnFromASM.TabIndex = 1;
      this.btnFromASM.Text = "From ASM";
      this.btnFromASM.Click += new DecentForms.EventHandler(this.btnFromASM_Click);
      // 
      // button1
      // 
      this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.button1.Location = new System.Drawing.Point(401, 48);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(109, 23);
      this.button1.TabIndex = 1;
      this.button1.Text = "To Hex";
      this.button1.Click += new DecentForms.EventHandler(this.btnToHex_Click);
      // 
      // btnToBASICHex
      // 
      this.btnToBASICHex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnToBASICHex.Location = new System.Drawing.Point(401, 106);
      this.btnToBASICHex.Name = "btnToBASICHex";
      this.btnToBASICHex.Size = new System.Drawing.Size(109, 23);
      this.btnToBASICHex.TabIndex = 1;
      this.btnToBASICHex.Text = "To BASIC Hex";
      this.btnToBASICHex.Click += new DecentForms.EventHandler(this.btnToBASICHex_Click);
      // 
      // btnToBASIC
      // 
      this.btnToBASIC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnToBASIC.Location = new System.Drawing.Point(401, 77);
      this.btnToBASIC.Name = "btnToBASIC";
      this.btnToBASIC.Size = new System.Drawing.Size(109, 23);
      this.btnToBASIC.TabIndex = 1;
      this.btnToBASIC.Text = "To BASIC";
      this.btnToBASIC.Click += new DecentForms.EventHandler(this.btnToBASIC_Click);
      // 
      // btnToText
      // 
      this.btnToText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnToText.Location = new System.Drawing.Point(401, 19);
      this.btnToText.Name = "btnToText";
      this.btnToText.Size = new System.Drawing.Size(109, 23);
      this.btnToText.TabIndex = 1;
      this.btnToText.Text = "To ASM";
      this.btnToText.Click += new DecentForms.EventHandler(this.btnToText_Click);
      // 
      // textBinaryData
      // 
      this.textBinaryData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.textBinaryData.Location = new System.Drawing.Point(6, 19);
      this.textBinaryData.MaxLength = 10000000;
      this.textBinaryData.Multiline = true;
      this.textBinaryData.Name = "textBinaryData";
      this.textBinaryData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.textBinaryData.Size = new System.Drawing.Size(389, 424);
      this.textBinaryData.TabIndex = 0;
      this.textBinaryData.WordWrap = false;
      this.textBinaryData.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.textBinaryData_PreviewKeyDown);
      // 
      // menuStrip1
      // 
      this.menuStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dataToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(733, 24);
      this.menuStrip1.TabIndex = 1;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // dataToolStripMenuItem
      // 
      this.dataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importFromFileToolStripMenuItem,
            this.exportToFileToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.toolStripSeparator1,
            this.modifyToolStripMenuItem});
      this.dataToolStripMenuItem.Name = "dataToolStripMenuItem";
      this.dataToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
      this.dataToolStripMenuItem.Text = "Binary";
      // 
      // importFromFileToolStripMenuItem
      // 
      this.importFromFileToolStripMenuItem.Name = "importFromFileToolStripMenuItem";
      this.importFromFileToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
      this.importFromFileToolStripMenuItem.Text = "&Import from file...";
      this.importFromFileToolStripMenuItem.Click += new System.EventHandler(this.importFromFileToolStripMenuItem_Click);
      // 
      // exportToFileToolStripMenuItem
      // 
      this.exportToFileToolStripMenuItem.Name = "exportToFileToolStripMenuItem";
      this.exportToFileToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
      this.exportToFileToolStripMenuItem.Text = "Export to file...";
      this.exportToFileToolStripMenuItem.Click += new System.EventHandler(this.exportToFileToolStripMenuItem_Click);
      // 
      // closeToolStripMenuItem
      // 
      this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
      this.closeToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
      this.closeToolStripMenuItem.Text = "&Close";
      this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(164, 6);
      // 
      // modifyToolStripMenuItem
      // 
      this.modifyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.interleaveToolStripMenuItem});
      this.modifyToolStripMenuItem.Name = "modifyToolStripMenuItem";
      this.modifyToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
      this.modifyToolStripMenuItem.Text = "Modify";
      // 
      // interleaveToolStripMenuItem
      // 
      this.interleaveToolStripMenuItem.Name = "interleaveToolStripMenuItem";
      this.interleaveToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
      this.interleaveToolStripMenuItem.Text = "Interleave...";
      this.interleaveToolStripMenuItem.Click += new System.EventHandler(this.interleaveToolStripMenuItem_Click);
      // 
      // checkInsertSpaces
      // 
      this.checkInsertSpaces.AutoSize = true;
      this.checkInsertSpaces.Location = new System.Drawing.Point(401, 296);
      this.checkInsertSpaces.Name = "checkInsertSpaces";
      this.checkInsertSpaces.Size = new System.Drawing.Size(91, 17);
      this.checkInsertSpaces.TabIndex = 7;
      this.checkInsertSpaces.Text = "Insert Spaces";
      this.checkInsertSpaces.UseVisualStyleBackColor = true;
      // 
      // BinaryDisplay
      // 
      this.ClientSize = new System.Drawing.Size(733, 538);
      this.Controls.Add(this.tabMain);
      this.Controls.Add(this.menuStrip1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MainMenuStrip = this.menuStrip1;
      this.Name = "BinaryDisplay";
      this.Text = "Binary Editor";
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      this.tabMain.ResumeLayout(false);
      this.tabData.ResumeLayout(false);
      this.tabModify.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TabControl tabMain;
    private System.Windows.Forms.TabPage tabData;
    private System.Windows.Forms.TabPage tabModify;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem dataToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exportToFileToolStripMenuItem;
    private Be.Windows.Forms.HexBox hexView;
    private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem importFromFileToolStripMenuItem;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.TextBox textBinaryData;
    private DecentForms.Button btnFromBASIC;
    private DecentForms.Button btnFromASM;
    private DecentForms.Button btnToBASIC;
    private DecentForms.Button btnToText;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem modifyToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem interleaveToolStripMenuItem;
    private System.Windows.Forms.GroupBox groupBox2;
    private DecentForms.Button btnInterleave;
    private DecentForms.Button btnExport;
    private DecentForms.Button btnImport;
    private DecentForms.Button button1;
    private DecentForms.Button button2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox editToBASICStartLine;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox editToBASICLineDelta;
    private DecentForms.Button btnToBASICHex;
    private DecentForms.Button btnFromBASICHex;
    private DecentForms.Button btnUpsize;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox editWrapCount;
    private System.Windows.Forms.TextBox editDeleteNthByte;
    private DecentForms.Button btnDeleteNthByte;
    private DecentForms.Button btnPackNibble;
    private DecentForms.Button btnSwizzle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox editWrapCharsCount;
    private System.Windows.Forms.TextBox editDivideBy;
    private DecentForms.Button btnDivide;
    private System.Windows.Forms.CheckBox checkInsertSpaces;
  }
}
