using RetroDevStudio.Controls;

namespace RetroDevStudio.Documents
{
  partial class Disassembler
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Disassembler));
      this.editDisassembly = new FastColoredTextBoxNS.FastColoredTextBox();
      this.contextMenuDisassembler = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.addJumpAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.addAsLabelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.panel1 = new System.Windows.Forms.Panel();
      this.btnOpenBinary = new DecentForms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.btnImportFromBinary = new DecentForms.Button();
      this.editStartAddress = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.btnExportToASM = new DecentForms.Button();
      this.btnReloadFile = new DecentForms.Button();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.btnDeleteJumpedAtAddress = new DecentForms.Button();
      this.btnAddJumpAddress = new DecentForms.Button();
      this.editJumpAddress = new System.Windows.Forms.TextBox();
      this.listJumpedAtAddresses = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.tabContent = new System.Windows.Forms.TabControl();
      this.tabDisassembly = new System.Windows.Forms.TabPage();
      this.tabBinary = new System.Windows.Forms.TabPage();
      this.hexView = new Be.Windows.Forms.HexBox();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.btnSaveProject = new DecentForms.Button();
      this.btnOpenProject = new DecentForms.Button();
      this.editDisassemblyProjectName = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.groupBox4 = new System.Windows.Forms.GroupBox();
      this.label4 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.btnNamedLabelsImport = new DecentForms.Button();
      this.btnExportNamedLabels = new DecentForms.Button();
      this.btnDeleteNamedLabel = new DecentForms.Button();
      this.btnAddNamedLabel = new DecentForms.Button();
      this.editLabelAddress = new System.Windows.Forms.TextBox();
      this.editLabelName = new System.Windows.Forms.TextBox();
      this.listNamedLabels = new System.Windows.Forms.ListView();
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.groupBox5 = new System.Windows.Forms.GroupBox();
      this.checkStopAtReturns = new System.Windows.Forms.CheckBox();
      this.checkShowHexData = new System.Windows.Forms.CheckBox();
      this.checkShowLineAddresses = new System.Windows.Forms.CheckBox();
      this.groupBox6 = new System.Windows.Forms.GroupBox();
      this.btnDeleteDataTable = new DecentForms.Button();
      this.listDataTables = new System.Windows.Forms.ListView();
      this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.btnAddDataTable = new DecentForms.Button();
      this.editDataTables = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.editDataTableLength = new System.Windows.Forms.TextBox();
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.editDisassembly)).BeginInit();
      this.contextMenuDisassembler.SuspendLayout();
      this.panel1.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.tabContent.SuspendLayout();
      this.tabDisassembly.SuspendLayout();
      this.tabBinary.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.groupBox4.SuspendLayout();
      this.groupBox5.SuspendLayout();
      this.groupBox6.SuspendLayout();
      this.SuspendLayout();
      // 
      // editDisassembly
      // 
      this.editDisassembly.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
      this.editDisassembly.AutoScrollMinSize = new System.Drawing.Size(2, 13);
      this.editDisassembly.BackBrush = null;
      this.editDisassembly.CharHeight = 13;
      this.editDisassembly.CharWidth = 7;
      this.editDisassembly.ContextMenuStrip = this.contextMenuDisassembler;
      this.editDisassembly.ConvertTabsToSpaces = false;
      this.editDisassembly.Cursor = System.Windows.Forms.Cursors.IBeam;
      this.editDisassembly.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
      this.editDisassembly.Dock = System.Windows.Forms.DockStyle.Fill;
      this.editDisassembly.Font = new System.Drawing.Font("Courier New", 9F);
      this.editDisassembly.IsReplaceMode = false;
      this.editDisassembly.Location = new System.Drawing.Point(0, 0);
      this.editDisassembly.Name = "editDisassembly";
      this.editDisassembly.Paddings = new System.Windows.Forms.Padding(0);
      this.editDisassembly.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
      this.editDisassembly.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("editDisassembly.ServiceColors")));
      this.editDisassembly.ShowLineNumbers = false;
      this.editDisassembly.Size = new System.Drawing.Size(456, 626);
      this.editDisassembly.TabIndex = 1;
      this.editDisassembly.TabLength = 2;
      this.editDisassembly.Zoom = 100;
      // 
      // contextMenuDisassembler
      // 
      this.contextMenuDisassembler.ImageScalingSize = new System.Drawing.Size(28, 28);
      this.contextMenuDisassembler.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addJumpAddressToolStripMenuItem,
            this.addAsLabelToolStripMenuItem});
      this.contextMenuDisassembler.Name = "contextMenuDisassembler";
      this.contextMenuDisassembler.Size = new System.Drawing.Size(174, 48);
      this.contextMenuDisassembler.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuDisassembler_Opening);
      // 
      // addJumpAddressToolStripMenuItem
      // 
      this.addJumpAddressToolStripMenuItem.Name = "addJumpAddressToolStripMenuItem";
      this.addJumpAddressToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
      this.addJumpAddressToolStripMenuItem.Text = "Add Jump Address";
      this.addJumpAddressToolStripMenuItem.Click += new System.EventHandler(this.addJumpAddressToolStripMenuItem_Click);
      // 
      // addAsLabelToolStripMenuItem
      // 
      this.addAsLabelToolStripMenuItem.Name = "addAsLabelToolStripMenuItem";
      this.addAsLabelToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
      this.addAsLabelToolStripMenuItem.Text = "Add as Label";
      this.addAsLabelToolStripMenuItem.Click += new System.EventHandler(this.addAsLabelToolStripMenuItem_Click);
      // 
      // panel1
      // 
      this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panel1.Controls.Add(this.editDisassembly);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel1.Location = new System.Drawing.Point(3, 3);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(460, 630);
      this.panel1.TabIndex = 2;
      // 
      // btnOpenBinary
      // 
      this.btnOpenBinary.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnOpenBinary.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnOpenBinary.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnOpenBinary.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOpenBinary.Image = null;
      this.btnOpenBinary.Location = new System.Drawing.Point(6, 19);
      this.btnOpenBinary.Name = "btnOpenBinary";
      this.btnOpenBinary.Size = new System.Drawing.Size(122, 23);
      this.btnOpenBinary.TabIndex = 0;
      this.btnOpenBinary.Text = "Open";
      this.btnOpenBinary.Click += new DecentForms.EventHandler(this.btnOpenBinary_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox1.Controls.Add(this.btnImportFromBinary);
      this.groupBox1.Controls.Add(this.editStartAddress);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.btnExportToASM);
      this.groupBox1.Controls.Add(this.btnReloadFile);
      this.groupBox1.Controls.Add(this.btnOpenBinary);
      this.groupBox1.Location = new System.Drawing.Point(492, 14);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(657, 80);
      this.groupBox1.TabIndex = 4;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Data";
      // 
      // btnImportFromBinary
      // 
      this.btnImportFromBinary.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnImportFromBinary.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnImportFromBinary.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnImportFromBinary.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnImportFromBinary.Image = null;
      this.btnImportFromBinary.Location = new System.Drawing.Point(134, 48);
      this.btnImportFromBinary.Name = "btnImportFromBinary";
      this.btnImportFromBinary.Size = new System.Drawing.Size(122, 23);
      this.btnImportFromBinary.TabIndex = 3;
      this.btnImportFromBinary.Text = "Binary from clipboard";
      this.toolTip1.SetToolTip(this.btnImportFromBinary, "Binary from clipboard");
      this.btnImportFromBinary.Click += new DecentForms.EventHandler(this.btnImportBinary_Click);
      // 
      // editStartAddress
      // 
      this.editStartAddress.Location = new System.Drawing.Point(354, 48);
      this.editStartAddress.Name = "editStartAddress";
      this.editStartAddress.Size = new System.Drawing.Size(100, 20);
      this.editStartAddress.TabIndex = 4;
      this.editStartAddress.TextChanged += new System.EventHandler(this.editStartAddress_TextChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(355, 24);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(99, 13);
      this.label1.TabIndex = 5;
      this.label1.Text = "Data Start Address:";
      // 
      // btnExportToASM
      // 
      this.btnExportToASM.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnExportToASM.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnExportToASM.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnExportToASM.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnExportToASM.Image = null;
      this.btnExportToASM.Location = new System.Drawing.Point(6, 48);
      this.btnExportToASM.Name = "btnExportToASM";
      this.btnExportToASM.Size = new System.Drawing.Size(122, 23);
      this.btnExportToASM.TabIndex = 2;
      this.btnExportToASM.Text = "Export to Assembly";
      this.btnExportToASM.Click += new DecentForms.EventHandler(this.btnExportAssembly_Click);
      // 
      // btnReloadFile
      // 
      this.btnReloadFile.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnReloadFile.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnReloadFile.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnReloadFile.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnReloadFile.Image = null;
      this.btnReloadFile.Location = new System.Drawing.Point(134, 19);
      this.btnReloadFile.Name = "btnReloadFile";
      this.btnReloadFile.Size = new System.Drawing.Size(122, 23);
      this.btnReloadFile.TabIndex = 1;
      this.btnReloadFile.Text = "Reload File";
      this.btnReloadFile.Click += new DecentForms.EventHandler(this.btnReloadFile_Click);
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.btnDeleteJumpedAtAddress);
      this.groupBox2.Controls.Add(this.btnAddJumpAddress);
      this.groupBox2.Controls.Add(this.editJumpAddress);
      this.groupBox2.Controls.Add(this.listJumpedAtAddresses);
      this.groupBox2.Location = new System.Drawing.Point(492, 100);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(182, 197);
      this.groupBox2.TabIndex = 4;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Jumped at addresses";
      // 
      // btnDeleteJumpedAtAddress
      // 
      this.btnDeleteJumpedAtAddress.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnDeleteJumpedAtAddress.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnDeleteJumpedAtAddress.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnDeleteJumpedAtAddress.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnDeleteJumpedAtAddress.Enabled = false;
      this.btnDeleteJumpedAtAddress.Image = null;
      this.btnDeleteJumpedAtAddress.Location = new System.Drawing.Point(6, 169);
      this.btnDeleteJumpedAtAddress.Name = "btnDeleteJumpedAtAddress";
      this.btnDeleteJumpedAtAddress.Size = new System.Drawing.Size(64, 22);
      this.btnDeleteJumpedAtAddress.TabIndex = 3;
      this.btnDeleteJumpedAtAddress.Text = "Delete";
      this.btnDeleteJumpedAtAddress.Click += new DecentForms.EventHandler(this.btnDeleteJumpedAtAddress_Click);
      // 
      // btnAddJumpAddress
      // 
      this.btnAddJumpAddress.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnAddJumpAddress.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnAddJumpAddress.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnAddJumpAddress.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnAddJumpAddress.Image = null;
      this.btnAddJumpAddress.Location = new System.Drawing.Point(112, 19);
      this.btnAddJumpAddress.Name = "btnAddJumpAddress";
      this.btnAddJumpAddress.Size = new System.Drawing.Size(64, 20);
      this.btnAddJumpAddress.TabIndex = 1;
      this.btnAddJumpAddress.Text = "add";
      this.btnAddJumpAddress.Click += new DecentForms.EventHandler(this.btnAddJumpAddress_Click);
      // 
      // editJumpAddress
      // 
      this.editJumpAddress.Location = new System.Drawing.Point(6, 19);
      this.editJumpAddress.Name = "editJumpAddress";
      this.editJumpAddress.Size = new System.Drawing.Size(100, 20);
      this.editJumpAddress.TabIndex = 0;
      this.toolTip1.SetToolTip(this.editJumpAddress, "Jump Address\r\n$XXXX or 0xXXXX for hex, otherwise decimal");
      // 
      // listJumpedAtAddresses
      // 
      this.listJumpedAtAddresses.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
      this.listJumpedAtAddresses.HideSelection = false;
      this.listJumpedAtAddresses.Location = new System.Drawing.Point(6, 47);
      this.listJumpedAtAddresses.Name = "listJumpedAtAddresses";
      this.listJumpedAtAddresses.Size = new System.Drawing.Size(170, 117);
      this.listJumpedAtAddresses.TabIndex = 2;
      this.listJumpedAtAddresses.UseCompatibleStateImageBehavior = false;
      this.listJumpedAtAddresses.View = System.Windows.Forms.View.Details;
      this.listJumpedAtAddresses.SelectedIndexChanged += new System.EventHandler(this.listJumpedAtAddresses_SelectedIndexChanged);
      this.listJumpedAtAddresses.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listJumpedAtAddresses_KeyDown);
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Address";
      this.columnHeader1.Width = 90;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Used";
      // 
      // tabContent
      // 
      this.tabContent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.tabContent.Controls.Add(this.tabDisassembly);
      this.tabContent.Controls.Add(this.tabBinary);
      this.tabContent.Location = new System.Drawing.Point(12, 14);
      this.tabContent.Name = "tabContent";
      this.tabContent.SelectedIndex = 0;
      this.tabContent.Size = new System.Drawing.Size(474, 662);
      this.tabContent.TabIndex = 0;
      // 
      // tabDisassembly
      // 
      this.tabDisassembly.Controls.Add(this.panel1);
      this.tabDisassembly.Location = new System.Drawing.Point(4, 22);
      this.tabDisassembly.Name = "tabDisassembly";
      this.tabDisassembly.Padding = new System.Windows.Forms.Padding(3);
      this.tabDisassembly.Size = new System.Drawing.Size(466, 636);
      this.tabDisassembly.TabIndex = 0;
      this.tabDisassembly.Text = "Disassembly";
      this.tabDisassembly.UseVisualStyleBackColor = true;
      // 
      // tabBinary
      // 
      this.tabBinary.Controls.Add(this.hexView);
      this.tabBinary.Location = new System.Drawing.Point(4, 22);
      this.tabBinary.Name = "tabBinary";
      this.tabBinary.Padding = new System.Windows.Forms.Padding(3);
      this.tabBinary.Size = new System.Drawing.Size(466, 636);
      this.tabBinary.TabIndex = 1;
      this.tabBinary.Text = "Binary";
      this.tabBinary.UseVisualStyleBackColor = true;
      // 
      // hexView
      // 
      this.hexView.BytesPerLine = 8;
      this.hexView.ColumnInfoVisible = true;
      this.hexView.CustomHexViewer = null;
      this.hexView.DisplayedAddressOffset = ((long)(0));
      this.hexView.DisplayedByteOffset = 0;
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
      this.hexView.Size = new System.Drawing.Size(460, 630);
      this.hexView.StringViewVisible = true;
      this.hexView.TabIndex = 1;
      this.hexView.TextFont = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.hexView.UseFixedBytesPerLine = true;
      this.hexView.VScrollBarVisible = true;
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.btnSaveProject);
      this.groupBox3.Controls.Add(this.btnOpenProject);
      this.groupBox3.Controls.Add(this.editDisassemblyProjectName);
      this.groupBox3.Controls.Add(this.label2);
      this.groupBox3.Location = new System.Drawing.Point(492, 303);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(385, 77);
      this.groupBox3.TabIndex = 6;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Disassembly Project";
      // 
      // btnSaveProject
      // 
      this.btnSaveProject.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnSaveProject.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnSaveProject.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnSaveProject.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnSaveProject.Image = null;
      this.btnSaveProject.Location = new System.Drawing.Point(91, 45);
      this.btnSaveProject.Name = "btnSaveProject";
      this.btnSaveProject.Size = new System.Drawing.Size(75, 23);
      this.btnSaveProject.TabIndex = 2;
      this.btnSaveProject.Text = "Save";
      this.btnSaveProject.Click += new DecentForms.EventHandler(this.btnSaveProject_Click);
      // 
      // btnOpenProject
      // 
      this.btnOpenProject.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnOpenProject.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnOpenProject.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnOpenProject.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOpenProject.Image = null;
      this.btnOpenProject.Location = new System.Drawing.Point(10, 45);
      this.btnOpenProject.Name = "btnOpenProject";
      this.btnOpenProject.Size = new System.Drawing.Size(75, 23);
      this.btnOpenProject.TabIndex = 1;
      this.btnOpenProject.Text = "Open";
      this.btnOpenProject.Click += new DecentForms.EventHandler(this.btnOpenProject_Click);
      // 
      // editDisassemblyProjectName
      // 
      this.editDisassemblyProjectName.Location = new System.Drawing.Point(51, 19);
      this.editDisassemblyProjectName.Name = "editDisassemblyProjectName";
      this.editDisassemblyProjectName.Size = new System.Drawing.Size(320, 20);
      this.editDisassemblyProjectName.TabIndex = 0;
      this.editDisassemblyProjectName.TextChanged += new System.EventHandler(this.editDisassemblyProjectName_TextChanged);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(7, 22);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(38, 13);
      this.label2.TabIndex = 5;
      this.label2.Text = "Name:";
      // 
      // groupBox4
      // 
      this.groupBox4.Controls.Add(this.label4);
      this.groupBox4.Controls.Add(this.label3);
      this.groupBox4.Controls.Add(this.btnNamedLabelsImport);
      this.groupBox4.Controls.Add(this.btnExportNamedLabels);
      this.groupBox4.Controls.Add(this.btnDeleteNamedLabel);
      this.groupBox4.Controls.Add(this.btnAddNamedLabel);
      this.groupBox4.Controls.Add(this.editLabelAddress);
      this.groupBox4.Controls.Add(this.editLabelName);
      this.groupBox4.Controls.Add(this.listNamedLabels);
      this.groupBox4.Location = new System.Drawing.Point(883, 100);
      this.groupBox4.Name = "groupBox4";
      this.groupBox4.Size = new System.Drawing.Size(266, 197);
      this.groupBox4.TabIndex = 4;
      this.groupBox4.TabStop = false;
      this.groupBox4.Text = "Named Labels";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(9, 47);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(37, 13);
      this.label4.TabIndex = 3;
      this.label4.Text = "Value:";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(9, 24);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(38, 13);
      this.label3.TabIndex = 3;
      this.label3.Text = "Name:";
      // 
      // btnNamedLabelsImport
      // 
      this.btnNamedLabelsImport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnNamedLabelsImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnNamedLabelsImport.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnNamedLabelsImport.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnNamedLabelsImport.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnNamedLabelsImport.Image = null;
      this.btnNamedLabelsImport.Location = new System.Drawing.Point(127, 169);
      this.btnNamedLabelsImport.Name = "btnNamedLabelsImport";
      this.btnNamedLabelsImport.Size = new System.Drawing.Size(64, 22);
      this.btnNamedLabelsImport.TabIndex = 5;
      this.btnNamedLabelsImport.Text = "Import";
      this.btnNamedLabelsImport.Click += new DecentForms.EventHandler(this.btnNamedLabelsImport_Click);
      // 
      // btnExportNamedLabels
      // 
      this.btnExportNamedLabels.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnExportNamedLabels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnExportNamedLabels.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnExportNamedLabels.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnExportNamedLabels.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnExportNamedLabels.Image = null;
      this.btnExportNamedLabels.Location = new System.Drawing.Point(196, 169);
      this.btnExportNamedLabels.Name = "btnExportNamedLabels";
      this.btnExportNamedLabels.Size = new System.Drawing.Size(64, 22);
      this.btnExportNamedLabels.TabIndex = 6;
      this.btnExportNamedLabels.Text = "Export";
      this.btnExportNamedLabels.Click += new DecentForms.EventHandler(this.btnExportNamedLabels_Click);
      // 
      // btnDeleteNamedLabel
      // 
      this.btnDeleteNamedLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnDeleteNamedLabel.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnDeleteNamedLabel.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnDeleteNamedLabel.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnDeleteNamedLabel.Enabled = false;
      this.btnDeleteNamedLabel.Image = null;
      this.btnDeleteNamedLabel.Location = new System.Drawing.Point(6, 169);
      this.btnDeleteNamedLabel.Name = "btnDeleteNamedLabel";
      this.btnDeleteNamedLabel.Size = new System.Drawing.Size(64, 22);
      this.btnDeleteNamedLabel.TabIndex = 4;
      this.btnDeleteNamedLabel.Text = "Delete";
      this.btnDeleteNamedLabel.Click += new DecentForms.EventHandler(this.btnDeleteNamedLabel_Click);
      // 
      // btnAddNamedLabel
      // 
      this.btnAddNamedLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnAddNamedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnAddNamedLabel.BackColor = System.Drawing.SystemColors.Control;
      this.btnAddNamedLabel.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnAddNamedLabel.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnAddNamedLabel.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnAddNamedLabel.Enabled = false;
      this.btnAddNamedLabel.Image = null;
      this.btnAddNamedLabel.Location = new System.Drawing.Point(196, 42);
      this.btnAddNamedLabel.Name = "btnAddNamedLabel";
      this.btnAddNamedLabel.Size = new System.Drawing.Size(64, 22);
      this.btnAddNamedLabel.TabIndex = 2;
      this.btnAddNamedLabel.Text = "add";
      this.btnAddNamedLabel.Click += new DecentForms.EventHandler(this.btnAddNamedLabel_Click);
      // 
      // editLabelAddress
      // 
      this.editLabelAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.editLabelAddress.Location = new System.Drawing.Point(53, 44);
      this.editLabelAddress.Name = "editLabelAddress";
      this.editLabelAddress.Size = new System.Drawing.Size(137, 20);
      this.editLabelAddress.TabIndex = 1;
      this.toolTip1.SetToolTip(this.editLabelAddress, "Label Address\r\n$XXXX or 0xXXXX for hex, otherwise decimal");
      this.editLabelAddress.TextChanged += new System.EventHandler(this.editLabelAddress_TextChanged);
      // 
      // editLabelName
      // 
      this.editLabelName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.editLabelName.Location = new System.Drawing.Point(53, 19);
      this.editLabelName.Name = "editLabelName";
      this.editLabelName.Size = new System.Drawing.Size(207, 20);
      this.editLabelName.TabIndex = 0;
      this.toolTip1.SetToolTip(this.editLabelName, "Label Name");
      this.editLabelName.TextChanged += new System.EventHandler(this.editLabelName_TextChanged);
      // 
      // listNamedLabels
      // 
      this.listNamedLabels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.listNamedLabels.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
      this.listNamedLabels.FullRowSelect = true;
      this.listNamedLabels.HideSelection = false;
      this.listNamedLabels.Location = new System.Drawing.Point(6, 70);
      this.listNamedLabels.Name = "listNamedLabels";
      this.listNamedLabels.Size = new System.Drawing.Size(254, 93);
      this.listNamedLabels.TabIndex = 3;
      this.listNamedLabels.UseCompatibleStateImageBehavior = false;
      this.listNamedLabels.View = System.Windows.Forms.View.Details;
      this.listNamedLabels.SelectedIndexChanged += new System.EventHandler(this.listNamedLabels_SelectedIndexChanged);
      this.listNamedLabels.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listNamedLabels_KeyDown);
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "Label";
      this.columnHeader3.Width = 132;
      // 
      // columnHeader4
      // 
      this.columnHeader4.Text = "Address";
      this.columnHeader4.Width = 106;
      // 
      // toolTip1
      // 
      this.toolTip1.ShowAlways = true;
      // 
      // groupBox5
      // 
      this.groupBox5.Controls.Add(this.checkStopAtReturns);
      this.groupBox5.Controls.Add(this.checkShowHexData);
      this.groupBox5.Controls.Add(this.checkShowLineAddresses);
      this.groupBox5.Location = new System.Drawing.Point(492, 386);
      this.groupBox5.Name = "groupBox5";
      this.groupBox5.Size = new System.Drawing.Size(385, 100);
      this.groupBox5.TabIndex = 7;
      this.groupBox5.TabStop = false;
      this.groupBox5.Text = "Options";
      // 
      // checkStopAtReturns
      // 
      this.checkStopAtReturns.AutoSize = true;
      this.checkStopAtReturns.Checked = true;
      this.checkStopAtReturns.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkStopAtReturns.Location = new System.Drawing.Point(10, 65);
      this.checkStopAtReturns.Name = "checkStopAtReturns";
      this.checkStopAtReturns.Size = new System.Drawing.Size(253, 17);
      this.checkStopAtReturns.TabIndex = 2;
      this.checkStopAtReturns.Text = "Stop Disassembly at End Points (e.g. RTS/JMP)";
      this.checkStopAtReturns.UseVisualStyleBackColor = true;
      this.checkStopAtReturns.CheckedChanged += new System.EventHandler(this.checkStopAtReturns_CheckedChanged);
      // 
      // checkShowHexData
      // 
      this.checkShowHexData.AutoSize = true;
      this.checkShowHexData.Checked = true;
      this.checkShowHexData.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkShowHexData.Location = new System.Drawing.Point(10, 42);
      this.checkShowHexData.Name = "checkShowHexData";
      this.checkShowHexData.Size = new System.Drawing.Size(166, 17);
      this.checkShowHexData.TabIndex = 1;
      this.checkShowHexData.Text = "Show Assembled Byte Values";
      this.checkShowHexData.UseVisualStyleBackColor = true;
      this.checkShowHexData.CheckedChanged += new System.EventHandler(this.checkShowHexData_CheckedChanged);
      // 
      // checkShowLineAddresses
      // 
      this.checkShowLineAddresses.AutoSize = true;
      this.checkShowLineAddresses.Checked = true;
      this.checkShowLineAddresses.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkShowLineAddresses.Location = new System.Drawing.Point(10, 19);
      this.checkShowLineAddresses.Name = "checkShowLineAddresses";
      this.checkShowLineAddresses.Size = new System.Drawing.Size(200, 17);
      this.checkShowLineAddresses.TabIndex = 0;
      this.checkShowLineAddresses.Text = "Show Line Addresses in Disassembly";
      this.checkShowLineAddresses.UseVisualStyleBackColor = true;
      this.checkShowLineAddresses.CheckedChanged += new System.EventHandler(this.checkShowLineAddresses_CheckedChanged);
      // 
      // groupBox6
      // 
      this.groupBox6.Controls.Add(this.btnDeleteDataTable);
      this.groupBox6.Controls.Add(this.label6);
      this.groupBox6.Controls.Add(this.label5);
      this.groupBox6.Controls.Add(this.listDataTables);
      this.groupBox6.Controls.Add(this.btnAddDataTable);
      this.groupBox6.Controls.Add(this.editDataTableLength);
      this.groupBox6.Controls.Add(this.editDataTables);
      this.groupBox6.Location = new System.Drawing.Point(680, 100);
      this.groupBox6.Name = "groupBox6";
      this.groupBox6.Size = new System.Drawing.Size(197, 197);
      this.groupBox6.TabIndex = 8;
      this.groupBox6.TabStop = false;
      this.groupBox6.Text = "Data Tables";
      // 
      // btnDeleteDataTable
      // 
      this.btnDeleteDataTable.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnDeleteDataTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnDeleteDataTable.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnDeleteDataTable.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnDeleteDataTable.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnDeleteDataTable.Enabled = false;
      this.btnDeleteDataTable.Image = null;
      this.btnDeleteDataTable.Location = new System.Drawing.Point(6, 169);
      this.btnDeleteDataTable.Name = "btnDeleteDataTable";
      this.btnDeleteDataTable.Size = new System.Drawing.Size(64, 22);
      this.btnDeleteDataTable.TabIndex = 3;
      this.btnDeleteDataTable.Text = "Delete";
      this.btnDeleteDataTable.Click += new DecentForms.EventHandler(this.btnDeleteDataTable_Click);
      // 
      // listDataTables
      // 
      this.listDataTables.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.listDataTables.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6});
      this.listDataTables.HideSelection = false;
      this.listDataTables.Location = new System.Drawing.Point(6, 70);
      this.listDataTables.Name = "listDataTables";
      this.listDataTables.Size = new System.Drawing.Size(177, 89);
      this.listDataTables.TabIndex = 2;
      this.listDataTables.UseCompatibleStateImageBehavior = false;
      this.listDataTables.View = System.Windows.Forms.View.Details;
      this.listDataTables.SelectedIndexChanged += new System.EventHandler(this.listDataTables_SelectedIndexChanged);
      this.listDataTables.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listDataTables_KeyDown);
      // 
      // columnHeader5
      // 
      this.columnHeader5.Text = "Address";
      this.columnHeader5.Width = 90;
      // 
      // columnHeader6
      // 
      this.columnHeader6.Text = "Length";
      // 
      // btnAddDataTable
      // 
      this.btnAddDataTable.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnAddDataTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnAddDataTable.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnAddDataTable.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnAddDataTable.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnAddDataTable.Image = null;
      this.btnAddDataTable.Location = new System.Drawing.Point(119, 44);
      this.btnAddDataTable.Name = "btnAddDataTable";
      this.btnAddDataTable.Size = new System.Drawing.Size(64, 20);
      this.btnAddDataTable.TabIndex = 1;
      this.btnAddDataTable.Text = "add";
      this.btnAddDataTable.Click += new DecentForms.EventHandler(this.btnAddDataTable_Click);
      // 
      // editDataTables
      // 
      this.editDataTables.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.editDataTables.Location = new System.Drawing.Point(60, 19);
      this.editDataTables.Name = "editDataTables";
      this.editDataTables.Size = new System.Drawing.Size(123, 20);
      this.editDataTables.TabIndex = 0;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(6, 22);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(48, 13);
      this.label5.TabIndex = 3;
      this.label5.Text = "Address:";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(6, 47);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(43, 13);
      this.label6.TabIndex = 3;
      this.label6.Text = "Length:";
      // 
      // editDataTableLength
      // 
      this.editDataTableLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.editDataTableLength.Location = new System.Drawing.Point(60, 44);
      this.editDataTableLength.Name = "editDataTableLength";
      this.editDataTableLength.Size = new System.Drawing.Size(53, 20);
      this.editDataTableLength.TabIndex = 0;
      // 
      // Disassembler
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.ClientSize = new System.Drawing.Size(1159, 688);
      this.Controls.Add(this.groupBox6);
      this.Controls.Add(this.groupBox5);
      this.Controls.Add(this.groupBox3);
      this.Controls.Add(this.tabContent);
      this.Controls.Add(this.groupBox4);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.groupBox1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "Disassembler";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "Disassembler";
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.editDisassembly)).EndInit();
      this.contextMenuDisassembler.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.tabContent.ResumeLayout(false);
      this.tabDisassembly.ResumeLayout(false);
      this.tabBinary.ResumeLayout(false);
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.groupBox4.ResumeLayout(false);
      this.groupBox4.PerformLayout();
      this.groupBox5.ResumeLayout(false);
      this.groupBox5.PerformLayout();
      this.groupBox6.ResumeLayout(false);
      this.groupBox6.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    public FastColoredTextBoxNS.FastColoredTextBox editDisassembly;
    private System.Windows.Forms.Panel panel1;
    private DecentForms.Button btnOpenBinary;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.GroupBox groupBox2;
    private DecentForms.Button btnAddJumpAddress;
    private System.Windows.Forms.TextBox editJumpAddress;
    private System.Windows.Forms.ListView listJumpedAtAddresses;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.TextBox editStartAddress;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TabControl tabContent;
    private System.Windows.Forms.TabPage tabDisassembly;
    private System.Windows.Forms.TabPage tabBinary;
    private Be.Windows.Forms.HexBox hexView;
    private DecentForms.Button btnDeleteJumpedAtAddress;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.TextBox editDisassemblyProjectName;
    private System.Windows.Forms.Label label2;
    private DecentForms.Button btnOpenProject;
    private DecentForms.Button btnSaveProject;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private DecentForms.Button btnExportToASM;
    private System.Windows.Forms.GroupBox groupBox4;
    private DecentForms.Button btnDeleteNamedLabel;
    private DecentForms.Button btnAddNamedLabel;
    private System.Windows.Forms.TextBox editLabelAddress;
    private System.Windows.Forms.TextBox editLabelName;
    private System.Windows.Forms.ListView listNamedLabels;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.ColumnHeader columnHeader4;
    private System.Windows.Forms.ToolTip toolTip1;
    private DecentForms.Button btnImportFromBinary;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label3;
    private DecentForms.Button btnReloadFile;
    private System.Windows.Forms.ContextMenuStrip contextMenuDisassembler;
    private System.Windows.Forms.ToolStripMenuItem addJumpAddressToolStripMenuItem;
    private System.Windows.Forms.GroupBox groupBox5;
    private System.Windows.Forms.CheckBox checkShowLineAddresses;
    private System.Windows.Forms.CheckBox checkShowHexData;
        private System.Windows.Forms.ToolStripMenuItem addAsLabelToolStripMenuItem;
    private System.Windows.Forms.CheckBox checkStopAtReturns;
    private DecentForms.Button btnNamedLabelsImport;
    private DecentForms.Button btnExportNamedLabels;
    private System.Windows.Forms.GroupBox groupBox6;
    private DecentForms.Button btnDeleteDataTable;
    private System.Windows.Forms.ListView listDataTables;
    private System.Windows.Forms.ColumnHeader columnHeader5;
    private System.Windows.Forms.ColumnHeader columnHeader6;
    private DecentForms.Button btnAddDataTable;
    private System.Windows.Forms.TextBox editDataTables;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox editDataTableLength;
  }
}