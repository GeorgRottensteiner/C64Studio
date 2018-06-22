namespace C64Studio
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
      this.panel1 = new System.Windows.Forms.Panel();
      this.btnOpenBinary = new System.Windows.Forms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.btnImportFromBinary = new System.Windows.Forms.Button();
      this.editStartAddress = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.btnExportToASM = new System.Windows.Forms.Button();
      this.btnReloadFile = new System.Windows.Forms.Button();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.btnDeleteJumpedAtAddress = new System.Windows.Forms.Button();
      this.btnAddJumpAddress = new System.Windows.Forms.Button();
      this.editJumpAddress = new System.Windows.Forms.TextBox();
      this.listJumpedAtAddresses = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.tabContent = new System.Windows.Forms.TabControl();
      this.tabDisassembly = new System.Windows.Forms.TabPage();
      this.tabBinary = new System.Windows.Forms.TabPage();
      this.hexView = new Be.Windows.Forms.HexBox();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.btnSaveProject = new System.Windows.Forms.Button();
      this.btnOpenProject = new System.Windows.Forms.Button();
      this.editDisassemblyProjectName = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.groupBox4 = new System.Windows.Forms.GroupBox();
      this.label4 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.btnDeleteNamedLabel = new System.Windows.Forms.Button();
      this.btnAddNamedLabel = new System.Windows.Forms.Button();
      this.editLabelAddress = new System.Windows.Forms.TextBox();
      this.editLabelName = new System.Windows.Forms.TextBox();
      this.listNamedLabels = new System.Windows.Forms.ListView();
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.contextMenuDisassembler = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.addJumpAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.editDisassembly)).BeginInit();
      this.panel1.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.tabContent.SuspendLayout();
      this.tabDisassembly.SuspendLayout();
      this.tabBinary.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.groupBox4.SuspendLayout();
      this.contextMenuDisassembler.SuspendLayout();
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
      this.editDisassembly.Size = new System.Drawing.Size(456, 618);
      this.editDisassembly.TabIndex = 1;
      this.editDisassembly.TabLength = 2;
      this.editDisassembly.Zoom = 100;
      // 
      // panel1
      // 
      this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panel1.Controls.Add(this.editDisassembly);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel1.Location = new System.Drawing.Point(3, 3);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(460, 622);
      this.panel1.TabIndex = 2;
      // 
      // btnOpenBinary
      // 
      this.btnOpenBinary.Location = new System.Drawing.Point(6, 19);
      this.btnOpenBinary.Name = "btnOpenBinary";
      this.btnOpenBinary.Size = new System.Drawing.Size(122, 23);
      this.btnOpenBinary.TabIndex = 3;
      this.btnOpenBinary.Text = "Open";
      this.btnOpenBinary.UseVisualStyleBackColor = true;
      this.btnOpenBinary.Click += new System.EventHandler(this.btnOpenBinary_Click);
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
      this.groupBox1.Size = new System.Drawing.Size(464, 80);
      this.groupBox1.TabIndex = 4;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Data";
      // 
      // btnImportFromBinary
      // 
      this.btnImportFromBinary.Location = new System.Drawing.Point(134, 48);
      this.btnImportFromBinary.Name = "btnImportFromBinary";
      this.btnImportFromBinary.Size = new System.Drawing.Size(122, 23);
      this.btnImportFromBinary.TabIndex = 3;
      this.btnImportFromBinary.Text = "Binary from clipboard";
      this.toolTip1.SetToolTip(this.btnImportFromBinary, "Binary from clipboard");
      this.btnImportFromBinary.UseVisualStyleBackColor = true;
      this.btnImportFromBinary.Click += new System.EventHandler(this.btnImportBinary_Click);
      // 
      // editStartAddress
      // 
      this.editStartAddress.Location = new System.Drawing.Point(354, 48);
      this.editStartAddress.Name = "editStartAddress";
      this.editStartAddress.Size = new System.Drawing.Size(100, 20);
      this.editStartAddress.TabIndex = 6;
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
      this.btnExportToASM.Location = new System.Drawing.Point(6, 48);
      this.btnExportToASM.Name = "btnExportToASM";
      this.btnExportToASM.Size = new System.Drawing.Size(122, 23);
      this.btnExportToASM.TabIndex = 3;
      this.btnExportToASM.Text = "Export to Assembly";
      this.btnExportToASM.UseVisualStyleBackColor = true;
      this.btnExportToASM.Click += new System.EventHandler(this.btnExportAssembly_Click);
      // 
      // btnReloadFile
      // 
      this.btnReloadFile.Location = new System.Drawing.Point(134, 19);
      this.btnReloadFile.Name = "btnReloadFile";
      this.btnReloadFile.Size = new System.Drawing.Size(122, 23);
      this.btnReloadFile.TabIndex = 3;
      this.btnReloadFile.Text = "Reload File";
      this.btnReloadFile.UseVisualStyleBackColor = true;
      this.btnReloadFile.Click += new System.EventHandler(this.btnReloadFile_Click);
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
      this.btnDeleteJumpedAtAddress.Enabled = false;
      this.btnDeleteJumpedAtAddress.Location = new System.Drawing.Point(6, 169);
      this.btnDeleteJumpedAtAddress.Name = "btnDeleteJumpedAtAddress";
      this.btnDeleteJumpedAtAddress.Size = new System.Drawing.Size(64, 22);
      this.btnDeleteJumpedAtAddress.TabIndex = 2;
      this.btnDeleteJumpedAtAddress.Text = "Delete";
      this.btnDeleteJumpedAtAddress.UseVisualStyleBackColor = true;
      this.btnDeleteJumpedAtAddress.Click += new System.EventHandler(this.btnDeleteJumpedAtAddress_Click);
      // 
      // btnAddJumpAddress
      // 
      this.btnAddJumpAddress.Location = new System.Drawing.Point(112, 19);
      this.btnAddJumpAddress.Name = "btnAddJumpAddress";
      this.btnAddJumpAddress.Size = new System.Drawing.Size(64, 22);
      this.btnAddJumpAddress.TabIndex = 2;
      this.btnAddJumpAddress.Text = "add";
      this.btnAddJumpAddress.UseVisualStyleBackColor = true;
      this.btnAddJumpAddress.Click += new System.EventHandler(this.btnAddJumpAddress_Click);
      // 
      // editJumpAddress
      // 
      this.editJumpAddress.Location = new System.Drawing.Point(6, 19);
      this.editJumpAddress.Name = "editJumpAddress";
      this.editJumpAddress.Size = new System.Drawing.Size(100, 20);
      this.editJumpAddress.TabIndex = 1;
      this.toolTip1.SetToolTip(this.editJumpAddress, "Jump Address\r\n$XXXX or 0xXXXX for hex, otherwise decimal");
      // 
      // listJumpedAtAddresses
      // 
      this.listJumpedAtAddresses.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
      this.listJumpedAtAddresses.Location = new System.Drawing.Point(6, 47);
      this.listJumpedAtAddresses.Name = "listJumpedAtAddresses";
      this.listJumpedAtAddresses.Size = new System.Drawing.Size(170, 117);
      this.listJumpedAtAddresses.TabIndex = 0;
      this.listJumpedAtAddresses.UseCompatibleStateImageBehavior = false;
      this.listJumpedAtAddresses.View = System.Windows.Forms.View.Details;
      this.listJumpedAtAddresses.SelectedIndexChanged += new System.EventHandler(this.listJumpedAtAddresses_SelectedIndexChanged);
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
      this.tabContent.Size = new System.Drawing.Size(474, 654);
      this.tabContent.TabIndex = 5;
      // 
      // tabDisassembly
      // 
      this.tabDisassembly.Controls.Add(this.panel1);
      this.tabDisassembly.Location = new System.Drawing.Point(4, 22);
      this.tabDisassembly.Name = "tabDisassembly";
      this.tabDisassembly.Padding = new System.Windows.Forms.Padding(3);
      this.tabDisassembly.Size = new System.Drawing.Size(466, 628);
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
      this.tabBinary.Size = new System.Drawing.Size(466, 628);
      this.tabBinary.TabIndex = 1;
      this.tabBinary.Text = "Binary";
      this.tabBinary.UseVisualStyleBackColor = true;
      // 
      // hexView
      // 
      this.hexView.BytesPerLine = 8;
      this.hexView.ColumnInfoVisible = true;
      this.hexView.CustomHexViewer = null;
      this.hexView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.hexView.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.hexView.InfoForeColor = System.Drawing.SystemColors.AppWorkspace;
      this.hexView.LineInfoVisible = true;
      this.hexView.Location = new System.Drawing.Point(3, 3);
      this.hexView.Name = "hexView";
      this.hexView.NumDigitsMemorySize = 8;
      this.hexView.SelectedByteProvider = null;
      this.hexView.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
      this.hexView.Size = new System.Drawing.Size(460, 622);
      this.hexView.StringViewVisible = true;
      this.hexView.TabIndex = 1;
      this.hexView.TextFont = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.hexView.UseFixedBytesPerLine = true;
      this.hexView.VScrollBarVisible = true;
      // 
      // groupBox3
      // 
      this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox3.Controls.Add(this.btnSaveProject);
      this.groupBox3.Controls.Add(this.btnOpenProject);
      this.groupBox3.Controls.Add(this.editDisassemblyProjectName);
      this.groupBox3.Controls.Add(this.label2);
      this.groupBox3.Location = new System.Drawing.Point(492, 303);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(464, 77);
      this.groupBox3.TabIndex = 6;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Disassembly Project";
      // 
      // btnSaveProject
      // 
      this.btnSaveProject.Location = new System.Drawing.Point(91, 45);
      this.btnSaveProject.Name = "btnSaveProject";
      this.btnSaveProject.Size = new System.Drawing.Size(75, 23);
      this.btnSaveProject.TabIndex = 7;
      this.btnSaveProject.Text = "Save";
      this.btnSaveProject.UseVisualStyleBackColor = true;
      this.btnSaveProject.Click += new System.EventHandler(this.btnSaveProject_Click);
      // 
      // btnOpenProject
      // 
      this.btnOpenProject.Location = new System.Drawing.Point(10, 45);
      this.btnOpenProject.Name = "btnOpenProject";
      this.btnOpenProject.Size = new System.Drawing.Size(75, 23);
      this.btnOpenProject.TabIndex = 7;
      this.btnOpenProject.Text = "Open";
      this.btnOpenProject.UseVisualStyleBackColor = true;
      this.btnOpenProject.Click += new System.EventHandler(this.btnOpenProject_Click);
      // 
      // editDisassemblyProjectName
      // 
      this.editDisassemblyProjectName.Location = new System.Drawing.Point(51, 19);
      this.editDisassemblyProjectName.Name = "editDisassemblyProjectName";
      this.editDisassemblyProjectName.Size = new System.Drawing.Size(338, 20);
      this.editDisassemblyProjectName.TabIndex = 6;
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
      this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox4.Controls.Add(this.label4);
      this.groupBox4.Controls.Add(this.label3);
      this.groupBox4.Controls.Add(this.btnDeleteNamedLabel);
      this.groupBox4.Controls.Add(this.btnAddNamedLabel);
      this.groupBox4.Controls.Add(this.editLabelAddress);
      this.groupBox4.Controls.Add(this.editLabelName);
      this.groupBox4.Controls.Add(this.listNamedLabels);
      this.groupBox4.Location = new System.Drawing.Point(680, 100);
      this.groupBox4.Name = "groupBox4";
      this.groupBox4.Size = new System.Drawing.Size(276, 197);
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
      // btnDeleteNamedLabel
      // 
      this.btnDeleteNamedLabel.Enabled = false;
      this.btnDeleteNamedLabel.Location = new System.Drawing.Point(6, 169);
      this.btnDeleteNamedLabel.Name = "btnDeleteNamedLabel";
      this.btnDeleteNamedLabel.Size = new System.Drawing.Size(64, 22);
      this.btnDeleteNamedLabel.TabIndex = 2;
      this.btnDeleteNamedLabel.Text = "Delete";
      this.btnDeleteNamedLabel.UseVisualStyleBackColor = true;
      this.btnDeleteNamedLabel.Click += new System.EventHandler(this.btnDeleteNamedLabel_Click);
      // 
      // btnAddNamedLabel
      // 
      this.btnAddNamedLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.btnAddNamedLabel.Enabled = false;
      this.btnAddNamedLabel.Location = new System.Drawing.Point(206, 42);
      this.btnAddNamedLabel.Name = "btnAddNamedLabel";
      this.btnAddNamedLabel.Size = new System.Drawing.Size(64, 22);
      this.btnAddNamedLabel.TabIndex = 2;
      this.btnAddNamedLabel.Text = "add";
      this.btnAddNamedLabel.UseVisualStyleBackColor = true;
      this.btnAddNamedLabel.Click += new System.EventHandler(this.btnAddNamedLabel_Click);
      // 
      // editLabelAddress
      // 
      this.editLabelAddress.Location = new System.Drawing.Point(53, 44);
      this.editLabelAddress.Name = "editLabelAddress";
      this.editLabelAddress.Size = new System.Drawing.Size(147, 20);
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
      this.editLabelName.Size = new System.Drawing.Size(217, 20);
      this.editLabelName.TabIndex = 1;
      this.toolTip1.SetToolTip(this.editLabelName, "Label Name");
      // 
      // listNamedLabels
      // 
      this.listNamedLabels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.listNamedLabels.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
      this.listNamedLabels.Location = new System.Drawing.Point(6, 70);
      this.listNamedLabels.Name = "listNamedLabels";
      this.listNamedLabels.Size = new System.Drawing.Size(264, 93);
      this.listNamedLabels.TabIndex = 0;
      this.listNamedLabels.UseCompatibleStateImageBehavior = false;
      this.listNamedLabels.View = System.Windows.Forms.View.Details;
      this.listNamedLabels.SelectedIndexChanged += new System.EventHandler(this.listNamedLabels_SelectedIndexChanged);
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
      // contextMenuDisassembler
      // 
      this.contextMenuDisassembler.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addJumpAddressToolStripMenuItem});
      this.contextMenuDisassembler.Name = "contextMenuDisassembler";
      this.contextMenuDisassembler.Size = new System.Drawing.Size(174, 26);
      this.contextMenuDisassembler.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuDisassembler_Opening);
      // 
      // addJumpAddressToolStripMenuItem
      // 
      this.addJumpAddressToolStripMenuItem.Name = "addJumpAddressToolStripMenuItem";
      this.addJumpAddressToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
      this.addJumpAddressToolStripMenuItem.Text = "Add Jump Address";
      this.addJumpAddressToolStripMenuItem.Click += new System.EventHandler(this.addJumpAddressToolStripMenuItem_Click);
      // 
      // Disassembler
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(968, 680);
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
      this.contextMenuDisassembler.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    public FastColoredTextBoxNS.FastColoredTextBox editDisassembly;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Button btnOpenBinary;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.Button btnAddJumpAddress;
    private System.Windows.Forms.TextBox editJumpAddress;
    private System.Windows.Forms.ListView listJumpedAtAddresses;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.TextBox editStartAddress;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TabControl tabContent;
    private System.Windows.Forms.TabPage tabDisassembly;
    private System.Windows.Forms.TabPage tabBinary;
    private Be.Windows.Forms.HexBox hexView;
    private System.Windows.Forms.Button btnDeleteJumpedAtAddress;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.TextBox editDisassemblyProjectName;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button btnOpenProject;
    private System.Windows.Forms.Button btnSaveProject;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.Button btnExportToASM;
    private System.Windows.Forms.GroupBox groupBox4;
    private System.Windows.Forms.Button btnDeleteNamedLabel;
    private System.Windows.Forms.Button btnAddNamedLabel;
    private System.Windows.Forms.TextBox editLabelAddress;
    private System.Windows.Forms.TextBox editLabelName;
    private System.Windows.Forms.ListView listNamedLabels;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.ColumnHeader columnHeader4;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.Button btnImportFromBinary;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Button btnReloadFile;
    private System.Windows.Forms.ContextMenuStrip contextMenuDisassembler;
    private System.Windows.Forms.ToolStripMenuItem addJumpAddressToolStripMenuItem;
  }
}