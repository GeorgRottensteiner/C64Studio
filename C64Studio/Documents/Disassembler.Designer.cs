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
      components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Disassembler));
      editDisassembly = new FastColoredTextBoxNS.FastColoredTextBox();
      contextMenuDisassembler = new System.Windows.Forms.ContextMenuStrip( components );
      addJumpAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      addAsLabelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      panel1 = new System.Windows.Forms.Panel();
      btnOpenBinary = new DecentForms.Button();
      groupBox1 = new System.Windows.Forms.GroupBox();
      btnImportFromBinary = new DecentForms.Button();
      editStartAddress = new System.Windows.Forms.TextBox();
      label1 = new System.Windows.Forms.Label();
      btnExportToASM = new DecentForms.Button();
      btnReloadFile = new DecentForms.Button();
      groupBox2 = new System.Windows.Forms.GroupBox();
      btnDeleteJumpedAtAddress = new DecentForms.Button();
      btnAddJumpAddress = new DecentForms.Button();
      editJumpAddress = new System.Windows.Forms.TextBox();
      listJumpedAtAddresses = new System.Windows.Forms.ListView();
      columnHeader1 = new System.Windows.Forms.ColumnHeader();
      columnHeader2 = new System.Windows.Forms.ColumnHeader();
      tabContent = new System.Windows.Forms.TabControl();
      tabDisassembly = new System.Windows.Forms.TabPage();
      tabBinary = new System.Windows.Forms.TabPage();
      hexView = new Be.Windows.Forms.HexBox();
      groupBox3 = new System.Windows.Forms.GroupBox();
      btnSaveProject = new DecentForms.Button();
      btnOpenProject = new DecentForms.Button();
      editDisassemblyProjectName = new System.Windows.Forms.TextBox();
      label2 = new System.Windows.Forms.Label();
      groupBox4 = new System.Windows.Forms.GroupBox();
      label4 = new System.Windows.Forms.Label();
      label3 = new System.Windows.Forms.Label();
      btnNamedLabelsImport = new DecentForms.Button();
      btnExportNamedLabels = new DecentForms.Button();
      btnDeleteNamedLabel = new DecentForms.Button();
      btnAddNamedLabel = new DecentForms.Button();
      editLabelAddress = new System.Windows.Forms.TextBox();
      editLabelName = new System.Windows.Forms.TextBox();
      listNamedLabels = new System.Windows.Forms.ListView();
      columnHeader3 = new System.Windows.Forms.ColumnHeader();
      columnHeader4 = new System.Windows.Forms.ColumnHeader();
      toolTip1 = new System.Windows.Forms.ToolTip( components );
      groupBox5 = new System.Windows.Forms.GroupBox();
      checkStopAtReturns = new System.Windows.Forms.CheckBox();
      checkOnlyAddUsedLabels = new System.Windows.Forms.CheckBox();
      checkShowHexData = new System.Windows.Forms.CheckBox();
      checkShowLineAddresses = new System.Windows.Forms.CheckBox();
      groupBox6 = new System.Windows.Forms.GroupBox();
      btnDeleteDataTable = new DecentForms.Button();
      label6 = new System.Windows.Forms.Label();
      label5 = new System.Windows.Forms.Label();
      listDataTables = new System.Windows.Forms.ListView();
      columnHeader5 = new System.Windows.Forms.ColumnHeader();
      columnHeader6 = new System.Windows.Forms.ColumnHeader();
      btnAddDataTable = new DecentForms.Button();
      editDataTableLength = new System.Windows.Forms.TextBox();
      editDataTables = new System.Windows.Forms.TextBox();
      ( (System.ComponentModel.ISupportInitialize)m_FileWatcher ).BeginInit();
      ( (System.ComponentModel.ISupportInitialize)editDisassembly ).BeginInit();
      contextMenuDisassembler.SuspendLayout();
      panel1.SuspendLayout();
      groupBox1.SuspendLayout();
      groupBox2.SuspendLayout();
      tabContent.SuspendLayout();
      tabDisassembly.SuspendLayout();
      tabBinary.SuspendLayout();
      groupBox3.SuspendLayout();
      groupBox4.SuspendLayout();
      groupBox5.SuspendLayout();
      groupBox6.SuspendLayout();
      SuspendLayout();
      // 
      // editDisassembly
      // 
      editDisassembly.AutoCompleteBracketsList = new char[]
  {
    '(',
    ')',
    '{',
    '}',
    '[',
    ']',
    '"',
    '"',
    '\'',
    '\''
  };
      editDisassembly.AutoScrollMinSize = new System.Drawing.Size( 2, 13 );
      editDisassembly.BackBrush = null;
      editDisassembly.CharHeight = 13;
      editDisassembly.CharWidth = 7;
      editDisassembly.ContextMenuStrip = contextMenuDisassembler;
      editDisassembly.ConvertTabsToSpaces = false;
      editDisassembly.Cursor = System.Windows.Forms.Cursors.IBeam;
      editDisassembly.DisabledColor = System.Drawing.Color.FromArgb( 100, 180, 180, 180 );
      editDisassembly.Dock = System.Windows.Forms.DockStyle.Fill;
      editDisassembly.Font = new System.Drawing.Font( "Courier New", 9F );
      editDisassembly.Hotkeys = resources.GetString( "editDisassembly.Hotkeys" );
      editDisassembly.IsReplaceMode = false;
      editDisassembly.Location = new System.Drawing.Point( 0, 0 );
      editDisassembly.Name = "editDisassembly";
      editDisassembly.Paddings = new System.Windows.Forms.Padding( 0 );
      editDisassembly.SelectionColor = System.Drawing.Color.FromArgb( 60, 0, 0, 255 );
      editDisassembly.ServiceColors = (FastColoredTextBoxNS.ServiceColors)resources.GetObject( "editDisassembly.ServiceColors" );
      editDisassembly.ShowLineNumbers = false;
      editDisassembly.Size = new System.Drawing.Size( 456, 626 );
      editDisassembly.TabIndex = 0;
      editDisassembly.TabLength = 2;
      editDisassembly.Zoom = 100;
      // 
      // contextMenuDisassembler
      // 
      contextMenuDisassembler.ImageScalingSize = new System.Drawing.Size( 28, 28 );
      contextMenuDisassembler.Items.AddRange( new System.Windows.Forms.ToolStripItem[] { addJumpAddressToolStripMenuItem, addAsLabelToolStripMenuItem } );
      contextMenuDisassembler.Name = "contextMenuDisassembler";
      contextMenuDisassembler.Size = new System.Drawing.Size( 174, 48 );
      contextMenuDisassembler.Opening +=  contextMenuDisassembler_Opening ;
      // 
      // addJumpAddressToolStripMenuItem
      // 
      addJumpAddressToolStripMenuItem.Name = "addJumpAddressToolStripMenuItem";
      addJumpAddressToolStripMenuItem.Size = new System.Drawing.Size( 173, 22 );
      addJumpAddressToolStripMenuItem.Text = "Add Jump Address";
      addJumpAddressToolStripMenuItem.Click +=  addJumpAddressToolStripMenuItem_Click ;
      // 
      // addAsLabelToolStripMenuItem
      // 
      addAsLabelToolStripMenuItem.Name = "addAsLabelToolStripMenuItem";
      addAsLabelToolStripMenuItem.Size = new System.Drawing.Size( 173, 22 );
      addAsLabelToolStripMenuItem.Text = "Add as Label";
      addAsLabelToolStripMenuItem.Click +=  addAsLabelToolStripMenuItem_Click ;
      // 
      // panel1
      // 
      panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      panel1.Controls.Add( editDisassembly );
      panel1.Dock = System.Windows.Forms.DockStyle.Fill;
      panel1.Location = new System.Drawing.Point( 3, 3 );
      panel1.Name = "panel1";
      panel1.Size = new System.Drawing.Size( 460, 630 );
      panel1.TabIndex = 2;
      // 
      // btnOpenBinary
      // 
      btnOpenBinary.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnOpenBinary.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnOpenBinary.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnOpenBinary.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnOpenBinary.Image = null;
      btnOpenBinary.Location = new System.Drawing.Point( 6, 19 );
      btnOpenBinary.Name = "btnOpenBinary";
      btnOpenBinary.Size = new System.Drawing.Size( 122, 23 );
      btnOpenBinary.TabIndex = 0;
      btnOpenBinary.Text = "Open";
      btnOpenBinary.Click +=  btnOpenBinary_Click ;
      // 
      // groupBox1
      // 
      groupBox1.Anchor =   System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Left   |  System.Windows.Forms.AnchorStyles.Right ;
      groupBox1.Controls.Add( btnImportFromBinary );
      groupBox1.Controls.Add( editStartAddress );
      groupBox1.Controls.Add( label1 );
      groupBox1.Controls.Add( btnExportToASM );
      groupBox1.Controls.Add( btnReloadFile );
      groupBox1.Controls.Add( btnOpenBinary );
      groupBox1.Location = new System.Drawing.Point( 492, 14 );
      groupBox1.Name = "groupBox1";
      groupBox1.Size = new System.Drawing.Size( 657, 80 );
      groupBox1.TabIndex = 4;
      groupBox1.TabStop = false;
      groupBox1.Text = "Data";
      // 
      // btnImportFromBinary
      // 
      btnImportFromBinary.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnImportFromBinary.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnImportFromBinary.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnImportFromBinary.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnImportFromBinary.Image = null;
      btnImportFromBinary.Location = new System.Drawing.Point( 134, 48 );
      btnImportFromBinary.Name = "btnImportFromBinary";
      btnImportFromBinary.Size = new System.Drawing.Size( 122, 23 );
      btnImportFromBinary.TabIndex = 3;
      btnImportFromBinary.Text = "Binary from clipboard";
      toolTip1.SetToolTip( btnImportFromBinary, "Binary from clipboard" );
      btnImportFromBinary.Click +=  btnImportBinary_Click ;
      // 
      // editStartAddress
      // 
      editStartAddress.Location = new System.Drawing.Point( 354, 48 );
      editStartAddress.Name = "editStartAddress";
      editStartAddress.Size = new System.Drawing.Size( 100, 20 );
      editStartAddress.TabIndex = 4;
      editStartAddress.TextChanged +=  editStartAddress_TextChanged ;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new System.Drawing.Point( 355, 24 );
      label1.Name = "label1";
      label1.Size = new System.Drawing.Size( 99, 13 );
      label1.TabIndex = 5;
      label1.Text = "Data Start Address:";
      // 
      // btnExportToASM
      // 
      btnExportToASM.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnExportToASM.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnExportToASM.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnExportToASM.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnExportToASM.Image = null;
      btnExportToASM.Location = new System.Drawing.Point( 6, 48 );
      btnExportToASM.Name = "btnExportToASM";
      btnExportToASM.Size = new System.Drawing.Size( 122, 23 );
      btnExportToASM.TabIndex = 2;
      btnExportToASM.Text = "Export to Assembly";
      btnExportToASM.Click +=  btnExportAssembly_Click ;
      // 
      // btnReloadFile
      // 
      btnReloadFile.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnReloadFile.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnReloadFile.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnReloadFile.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnReloadFile.Image = null;
      btnReloadFile.Location = new System.Drawing.Point( 134, 19 );
      btnReloadFile.Name = "btnReloadFile";
      btnReloadFile.Size = new System.Drawing.Size( 122, 23 );
      btnReloadFile.TabIndex = 1;
      btnReloadFile.Text = "Reload File";
      btnReloadFile.Click +=  btnReloadFile_Click ;
      // 
      // groupBox2
      // 
      groupBox2.Controls.Add( btnDeleteJumpedAtAddress );
      groupBox2.Controls.Add( btnAddJumpAddress );
      groupBox2.Controls.Add( editJumpAddress );
      groupBox2.Controls.Add( listJumpedAtAddresses );
      groupBox2.Location = new System.Drawing.Point( 492, 100 );
      groupBox2.Name = "groupBox2";
      groupBox2.Size = new System.Drawing.Size( 182, 197 );
      groupBox2.TabIndex = 4;
      groupBox2.TabStop = false;
      groupBox2.Text = "Jumped at addresses";
      // 
      // btnDeleteJumpedAtAddress
      // 
      btnDeleteJumpedAtAddress.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnDeleteJumpedAtAddress.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnDeleteJumpedAtAddress.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnDeleteJumpedAtAddress.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnDeleteJumpedAtAddress.Enabled = false;
      btnDeleteJumpedAtAddress.Image = null;
      btnDeleteJumpedAtAddress.Location = new System.Drawing.Point( 6, 169 );
      btnDeleteJumpedAtAddress.Name = "btnDeleteJumpedAtAddress";
      btnDeleteJumpedAtAddress.Size = new System.Drawing.Size( 64, 22 );
      btnDeleteJumpedAtAddress.TabIndex = 3;
      btnDeleteJumpedAtAddress.Text = "Delete";
      btnDeleteJumpedAtAddress.Click +=  btnDeleteJumpedAtAddress_Click ;
      // 
      // btnAddJumpAddress
      // 
      btnAddJumpAddress.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnAddJumpAddress.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnAddJumpAddress.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnAddJumpAddress.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnAddJumpAddress.Image = null;
      btnAddJumpAddress.Location = new System.Drawing.Point( 112, 19 );
      btnAddJumpAddress.Name = "btnAddJumpAddress";
      btnAddJumpAddress.Size = new System.Drawing.Size( 64, 20 );
      btnAddJumpAddress.TabIndex = 1;
      btnAddJumpAddress.Text = "add";
      btnAddJumpAddress.Click +=  btnAddJumpAddress_Click ;
      // 
      // editJumpAddress
      // 
      editJumpAddress.Location = new System.Drawing.Point( 6, 19 );
      editJumpAddress.Name = "editJumpAddress";
      editJumpAddress.Size = new System.Drawing.Size( 100, 20 );
      editJumpAddress.TabIndex = 0;
      toolTip1.SetToolTip( editJumpAddress, "Jump Address\r\n$XXXX or 0xXXXX for hex, otherwise decimal" );
      // 
      // listJumpedAtAddresses
      // 
      listJumpedAtAddresses.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2 } );
      listJumpedAtAddresses.FullRowSelect = true;
      listJumpedAtAddresses.Location = new System.Drawing.Point( 6, 47 );
      listJumpedAtAddresses.Name = "listJumpedAtAddresses";
      listJumpedAtAddresses.Size = new System.Drawing.Size( 170, 117 );
      listJumpedAtAddresses.TabIndex = 2;
      listJumpedAtAddresses.UseCompatibleStateImageBehavior = false;
      listJumpedAtAddresses.View = System.Windows.Forms.View.Details;
      listJumpedAtAddresses.SelectedIndexChanged +=  listJumpedAtAddresses_SelectedIndexChanged ;
      listJumpedAtAddresses.KeyDown +=  listJumpedAtAddresses_KeyDown ;
      // 
      // columnHeader1
      // 
      columnHeader1.Text = "Address";
      columnHeader1.Width = 90;
      // 
      // columnHeader2
      // 
      columnHeader2.Text = "Used";
      // 
      // tabContent
      // 
      tabContent.Anchor =   System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Bottom   |  System.Windows.Forms.AnchorStyles.Left ;
      tabContent.Controls.Add( tabDisassembly );
      tabContent.Controls.Add( tabBinary );
      tabContent.Location = new System.Drawing.Point( 12, 14 );
      tabContent.Name = "tabContent";
      tabContent.SelectedIndex = 0;
      tabContent.Size = new System.Drawing.Size( 474, 662 );
      tabContent.TabIndex = 0;
      // 
      // tabDisassembly
      // 
      tabDisassembly.Controls.Add( panel1 );
      tabDisassembly.Location = new System.Drawing.Point( 4, 22 );
      tabDisassembly.Name = "tabDisassembly";
      tabDisassembly.Padding = new System.Windows.Forms.Padding( 3 );
      tabDisassembly.Size = new System.Drawing.Size( 466, 636 );
      tabDisassembly.TabIndex = 0;
      tabDisassembly.Text = "Disassembly";
      tabDisassembly.UseVisualStyleBackColor = true;
      // 
      // tabBinary
      // 
      tabBinary.Controls.Add( hexView );
      tabBinary.Location = new System.Drawing.Point( 4, 22 );
      tabBinary.Name = "tabBinary";
      tabBinary.Padding = new System.Windows.Forms.Padding( 3 );
      tabBinary.Size = new System.Drawing.Size( 466, 636 );
      tabBinary.TabIndex = 1;
      tabBinary.Text = "Binary";
      tabBinary.UseVisualStyleBackColor = true;
      // 
      // hexView
      // 
      hexView.BytesPerLine = 8;
      hexView.ColumnInfoVisible = true;
      hexView.CustomHexViewer = null;
      hexView.DisplayedAddressOffset = 0L;
      hexView.DisplayedByteOffset = 0;
      hexView.Dock = System.Windows.Forms.DockStyle.Fill;
      hexView.Font = new System.Drawing.Font( "Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0 );
      hexView.InfoForeColor = System.Drawing.SystemColors.AppWorkspace;
      hexView.LineInfoVisible = true;
      hexView.Location = new System.Drawing.Point( 3, 3 );
      hexView.MarkedForeColor = System.Drawing.Color.Empty;
      hexView.Name = "hexView";
      hexView.NumDigitsMemorySize = 8;
      hexView.SelectedByteProvider = null;
      hexView.ShadowSelectionColor = System.Drawing.Color.FromArgb( 100, 60, 188, 255 );
      hexView.Size = new System.Drawing.Size( 460, 630 );
      hexView.StringViewVisible = true;
      hexView.TabIndex = 1;
      hexView.TextFont = new System.Drawing.Font( "Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0 );
      hexView.UseFixedBytesPerLine = true;
      hexView.VScrollBarVisible = true;
      // 
      // groupBox3
      // 
      groupBox3.Controls.Add( btnSaveProject );
      groupBox3.Controls.Add( btnOpenProject );
      groupBox3.Controls.Add( editDisassemblyProjectName );
      groupBox3.Controls.Add( label2 );
      groupBox3.Location = new System.Drawing.Point( 492, 303 );
      groupBox3.Name = "groupBox3";
      groupBox3.Size = new System.Drawing.Size( 385, 77 );
      groupBox3.TabIndex = 6;
      groupBox3.TabStop = false;
      groupBox3.Text = "Disassembly Project";
      // 
      // btnSaveProject
      // 
      btnSaveProject.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnSaveProject.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnSaveProject.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnSaveProject.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnSaveProject.Image = null;
      btnSaveProject.Location = new System.Drawing.Point( 91, 45 );
      btnSaveProject.Name = "btnSaveProject";
      btnSaveProject.Size = new System.Drawing.Size( 75, 23 );
      btnSaveProject.TabIndex = 2;
      btnSaveProject.Text = "Save";
      btnSaveProject.Click +=  btnSaveProject_Click ;
      // 
      // btnOpenProject
      // 
      btnOpenProject.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnOpenProject.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnOpenProject.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnOpenProject.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnOpenProject.Image = null;
      btnOpenProject.Location = new System.Drawing.Point( 10, 45 );
      btnOpenProject.Name = "btnOpenProject";
      btnOpenProject.Size = new System.Drawing.Size( 75, 23 );
      btnOpenProject.TabIndex = 1;
      btnOpenProject.Text = "Open";
      btnOpenProject.Click +=  btnOpenProject_Click ;
      // 
      // editDisassemblyProjectName
      // 
      editDisassemblyProjectName.Location = new System.Drawing.Point( 51, 19 );
      editDisassemblyProjectName.Name = "editDisassemblyProjectName";
      editDisassemblyProjectName.Size = new System.Drawing.Size( 320, 20 );
      editDisassemblyProjectName.TabIndex = 0;
      editDisassemblyProjectName.TextChanged +=  editDisassemblyProjectName_TextChanged ;
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new System.Drawing.Point( 7, 22 );
      label2.Name = "label2";
      label2.Size = new System.Drawing.Size( 38, 13 );
      label2.TabIndex = 5;
      label2.Text = "Name:";
      // 
      // groupBox4
      // 
      groupBox4.Controls.Add( label4 );
      groupBox4.Controls.Add( label3 );
      groupBox4.Controls.Add( btnNamedLabelsImport );
      groupBox4.Controls.Add( btnExportNamedLabels );
      groupBox4.Controls.Add( btnDeleteNamedLabel );
      groupBox4.Controls.Add( btnAddNamedLabel );
      groupBox4.Controls.Add( editLabelAddress );
      groupBox4.Controls.Add( editLabelName );
      groupBox4.Controls.Add( listNamedLabels );
      groupBox4.Location = new System.Drawing.Point( 883, 100 );
      groupBox4.Name = "groupBox4";
      groupBox4.Size = new System.Drawing.Size( 266, 197 );
      groupBox4.TabIndex = 4;
      groupBox4.TabStop = false;
      groupBox4.Text = "Named Labels";
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Location = new System.Drawing.Point( 9, 47 );
      label4.Name = "label4";
      label4.Size = new System.Drawing.Size( 37, 13 );
      label4.TabIndex = 3;
      label4.Text = "Value:";
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new System.Drawing.Point( 9, 24 );
      label3.Name = "label3";
      label3.Size = new System.Drawing.Size( 38, 13 );
      label3.TabIndex = 3;
      label3.Text = "Name:";
      // 
      // btnNamedLabelsImport
      // 
      btnNamedLabelsImport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnNamedLabelsImport.Anchor =  System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Right ;
      btnNamedLabelsImport.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnNamedLabelsImport.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnNamedLabelsImport.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnNamedLabelsImport.Image = null;
      btnNamedLabelsImport.Location = new System.Drawing.Point( 127, 169 );
      btnNamedLabelsImport.Name = "btnNamedLabelsImport";
      btnNamedLabelsImport.Size = new System.Drawing.Size( 64, 22 );
      btnNamedLabelsImport.TabIndex = 5;
      btnNamedLabelsImport.Text = "Import";
      btnNamedLabelsImport.Click +=  btnNamedLabelsImport_Click ;
      // 
      // btnExportNamedLabels
      // 
      btnExportNamedLabels.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnExportNamedLabels.Anchor =  System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Right ;
      btnExportNamedLabels.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnExportNamedLabels.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnExportNamedLabels.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnExportNamedLabels.Image = null;
      btnExportNamedLabels.Location = new System.Drawing.Point( 196, 169 );
      btnExportNamedLabels.Name = "btnExportNamedLabels";
      btnExportNamedLabels.Size = new System.Drawing.Size( 64, 22 );
      btnExportNamedLabels.TabIndex = 6;
      btnExportNamedLabels.Text = "Export";
      btnExportNamedLabels.Click +=  btnExportNamedLabels_Click ;
      // 
      // btnDeleteNamedLabel
      // 
      btnDeleteNamedLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnDeleteNamedLabel.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnDeleteNamedLabel.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnDeleteNamedLabel.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnDeleteNamedLabel.Enabled = false;
      btnDeleteNamedLabel.Image = null;
      btnDeleteNamedLabel.Location = new System.Drawing.Point( 6, 169 );
      btnDeleteNamedLabel.Name = "btnDeleteNamedLabel";
      btnDeleteNamedLabel.Size = new System.Drawing.Size( 64, 22 );
      btnDeleteNamedLabel.TabIndex = 4;
      btnDeleteNamedLabel.Text = "Delete";
      btnDeleteNamedLabel.Click +=  btnDeleteNamedLabel_Click ;
      // 
      // btnAddNamedLabel
      // 
      btnAddNamedLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnAddNamedLabel.Anchor =  System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Right ;
      btnAddNamedLabel.BackColor = System.Drawing.SystemColors.Control;
      btnAddNamedLabel.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnAddNamedLabel.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnAddNamedLabel.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnAddNamedLabel.Enabled = false;
      btnAddNamedLabel.Image = null;
      btnAddNamedLabel.Location = new System.Drawing.Point( 196, 42 );
      btnAddNamedLabel.Name = "btnAddNamedLabel";
      btnAddNamedLabel.Size = new System.Drawing.Size( 64, 22 );
      btnAddNamedLabel.TabIndex = 2;
      btnAddNamedLabel.Text = "add";
      btnAddNamedLabel.Click +=  btnAddNamedLabel_Click ;
      // 
      // editLabelAddress
      // 
      editLabelAddress.Anchor =   System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Left   |  System.Windows.Forms.AnchorStyles.Right ;
      editLabelAddress.Location = new System.Drawing.Point( 53, 44 );
      editLabelAddress.Name = "editLabelAddress";
      editLabelAddress.Size = new System.Drawing.Size( 137, 20 );
      editLabelAddress.TabIndex = 1;
      toolTip1.SetToolTip( editLabelAddress, "Label Address\r\n$XXXX or 0xXXXX for hex, otherwise decimal" );
      editLabelAddress.TextChanged +=  editLabelAddress_TextChanged ;
      // 
      // editLabelName
      // 
      editLabelName.Anchor =   System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Left   |  System.Windows.Forms.AnchorStyles.Right ;
      editLabelName.Location = new System.Drawing.Point( 53, 19 );
      editLabelName.Name = "editLabelName";
      editLabelName.Size = new System.Drawing.Size( 207, 20 );
      editLabelName.TabIndex = 0;
      toolTip1.SetToolTip( editLabelName, "Label Name" );
      editLabelName.TextChanged +=  editLabelName_TextChanged ;
      // 
      // listNamedLabels
      // 
      listNamedLabels.Anchor =   System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Left   |  System.Windows.Forms.AnchorStyles.Right ;
      listNamedLabels.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] { columnHeader3, columnHeader4 } );
      listNamedLabels.FullRowSelect = true;
      listNamedLabels.Location = new System.Drawing.Point( 6, 70 );
      listNamedLabels.Name = "listNamedLabels";
      listNamedLabels.Size = new System.Drawing.Size( 254, 93 );
      listNamedLabels.TabIndex = 3;
      listNamedLabels.UseCompatibleStateImageBehavior = false;
      listNamedLabels.View = System.Windows.Forms.View.Details;
      listNamedLabels.SelectedIndexChanged +=  listNamedLabels_SelectedIndexChanged ;
      listNamedLabels.KeyDown +=  listNamedLabels_KeyDown ;
      // 
      // columnHeader3
      // 
      columnHeader3.Text = "Label";
      columnHeader3.Width = 132;
      // 
      // columnHeader4
      // 
      columnHeader4.Text = "Address";
      columnHeader4.Width = 106;
      // 
      // toolTip1
      // 
      toolTip1.ShowAlways = true;
      // 
      // groupBox5
      // 
      groupBox5.Controls.Add( checkStopAtReturns );
      groupBox5.Controls.Add( checkOnlyAddUsedLabels );
      groupBox5.Controls.Add( checkShowHexData );
      groupBox5.Controls.Add( checkShowLineAddresses );
      groupBox5.Location = new System.Drawing.Point( 492, 386 );
      groupBox5.Name = "groupBox5";
      groupBox5.Size = new System.Drawing.Size( 385, 122 );
      groupBox5.TabIndex = 7;
      groupBox5.TabStop = false;
      groupBox5.Text = "Options";
      // 
      // checkStopAtReturns
      // 
      checkStopAtReturns.AutoSize = true;
      checkStopAtReturns.Checked = true;
      checkStopAtReturns.CheckState = System.Windows.Forms.CheckState.Checked;
      checkStopAtReturns.Location = new System.Drawing.Point( 10, 65 );
      checkStopAtReturns.Name = "checkStopAtReturns";
      checkStopAtReturns.Size = new System.Drawing.Size( 253, 17 );
      checkStopAtReturns.TabIndex = 2;
      checkStopAtReturns.Text = "Stop Disassembly at End Points (e.g. RTS/JMP)";
      checkStopAtReturns.UseVisualStyleBackColor = true;
      checkStopAtReturns.CheckedChanged +=  checkStopAtReturns_CheckedChanged ;
      // 
      // checkOnlyAddUsedLabels
      // 
      checkOnlyAddUsedLabels.AutoSize = true;
      checkOnlyAddUsedLabels.Checked = true;
      checkOnlyAddUsedLabels.CheckState = System.Windows.Forms.CheckState.Checked;
      checkOnlyAddUsedLabels.Location = new System.Drawing.Point( 10, 88 );
      checkOnlyAddUsedLabels.Name = "checkOnlyAddUsedLabels";
      checkOnlyAddUsedLabels.Size = new System.Drawing.Size( 123, 17 );
      checkOnlyAddUsedLabels.TabIndex = 3;
      checkOnlyAddUsedLabels.Text = "Add only used labels";
      checkOnlyAddUsedLabels.UseVisualStyleBackColor = true;
      checkOnlyAddUsedLabels.CheckedChanged +=  checkOnlyAddUsedLabels_CheckedChanged ;
      // 
      // checkShowHexData
      // 
      checkShowHexData.AutoSize = true;
      checkShowHexData.Checked = true;
      checkShowHexData.CheckState = System.Windows.Forms.CheckState.Checked;
      checkShowHexData.Location = new System.Drawing.Point( 10, 42 );
      checkShowHexData.Name = "checkShowHexData";
      checkShowHexData.Size = new System.Drawing.Size( 166, 17 );
      checkShowHexData.TabIndex = 1;
      checkShowHexData.Text = "Show Assembled Byte Values";
      checkShowHexData.UseVisualStyleBackColor = true;
      checkShowHexData.CheckedChanged +=  checkShowHexData_CheckedChanged ;
      // 
      // checkShowLineAddresses
      // 
      checkShowLineAddresses.AutoSize = true;
      checkShowLineAddresses.Checked = true;
      checkShowLineAddresses.CheckState = System.Windows.Forms.CheckState.Checked;
      checkShowLineAddresses.Location = new System.Drawing.Point( 10, 19 );
      checkShowLineAddresses.Name = "checkShowLineAddresses";
      checkShowLineAddresses.Size = new System.Drawing.Size( 200, 17 );
      checkShowLineAddresses.TabIndex = 0;
      checkShowLineAddresses.Text = "Show Line Addresses in Disassembly";
      checkShowLineAddresses.UseVisualStyleBackColor = true;
      checkShowLineAddresses.CheckedChanged +=  checkShowLineAddresses_CheckedChanged ;
      // 
      // groupBox6
      // 
      groupBox6.Controls.Add( btnDeleteDataTable );
      groupBox6.Controls.Add( label6 );
      groupBox6.Controls.Add( label5 );
      groupBox6.Controls.Add( listDataTables );
      groupBox6.Controls.Add( btnAddDataTable );
      groupBox6.Controls.Add( editDataTableLength );
      groupBox6.Controls.Add( editDataTables );
      groupBox6.Location = new System.Drawing.Point( 680, 100 );
      groupBox6.Name = "groupBox6";
      groupBox6.Size = new System.Drawing.Size( 197, 197 );
      groupBox6.TabIndex = 8;
      groupBox6.TabStop = false;
      groupBox6.Text = "Data Tables";
      // 
      // btnDeleteDataTable
      // 
      btnDeleteDataTable.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnDeleteDataTable.Anchor =  System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Right ;
      btnDeleteDataTable.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnDeleteDataTable.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnDeleteDataTable.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnDeleteDataTable.Enabled = false;
      btnDeleteDataTable.Image = null;
      btnDeleteDataTable.Location = new System.Drawing.Point( 6, 169 );
      btnDeleteDataTable.Name = "btnDeleteDataTable";
      btnDeleteDataTable.Size = new System.Drawing.Size( 64, 22 );
      btnDeleteDataTable.TabIndex = 4;
      btnDeleteDataTable.Text = "Delete";
      btnDeleteDataTable.Click +=  btnDeleteDataTable_Click ;
      // 
      // label6
      // 
      label6.AutoSize = true;
      label6.Location = new System.Drawing.Point( 6, 47 );
      label6.Name = "label6";
      label6.Size = new System.Drawing.Size( 43, 13 );
      label6.TabIndex = 3;
      label6.Text = "Length:";
      // 
      // label5
      // 
      label5.AutoSize = true;
      label5.Location = new System.Drawing.Point( 6, 22 );
      label5.Name = "label5";
      label5.Size = new System.Drawing.Size( 48, 13 );
      label5.TabIndex = 3;
      label5.Text = "Address:";
      // 
      // listDataTables
      // 
      listDataTables.Anchor =  System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Right ;
      listDataTables.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] { columnHeader5, columnHeader6 } );
      listDataTables.FullRowSelect = true;
      listDataTables.Location = new System.Drawing.Point( 6, 70 );
      listDataTables.Name = "listDataTables";
      listDataTables.Size = new System.Drawing.Size( 177, 89 );
      listDataTables.TabIndex = 3;
      listDataTables.UseCompatibleStateImageBehavior = false;
      listDataTables.View = System.Windows.Forms.View.Details;
      listDataTables.SelectedIndexChanged +=  listDataTables_SelectedIndexChanged ;
      listDataTables.KeyDown +=  listDataTables_KeyDown ;
      // 
      // columnHeader5
      // 
      columnHeader5.Text = "Address";
      columnHeader5.Width = 90;
      // 
      // columnHeader6
      // 
      columnHeader6.Text = "Length";
      // 
      // btnAddDataTable
      // 
      btnAddDataTable.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnAddDataTable.Anchor =  System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Right ;
      btnAddDataTable.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnAddDataTable.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnAddDataTable.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnAddDataTable.Image = null;
      btnAddDataTable.Location = new System.Drawing.Point( 119, 44 );
      btnAddDataTable.Name = "btnAddDataTable";
      btnAddDataTable.Size = new System.Drawing.Size( 64, 20 );
      btnAddDataTable.TabIndex = 2;
      btnAddDataTable.Text = "add";
      btnAddDataTable.Click +=  btnAddDataTable_Click ;
      // 
      // editDataTableLength
      // 
      editDataTableLength.Anchor =  System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Right ;
      editDataTableLength.Location = new System.Drawing.Point( 60, 44 );
      editDataTableLength.Name = "editDataTableLength";
      editDataTableLength.Size = new System.Drawing.Size( 53, 20 );
      editDataTableLength.TabIndex = 1;
      // 
      // editDataTables
      // 
      editDataTables.Anchor =  System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Right ;
      editDataTables.Location = new System.Drawing.Point( 60, 19 );
      editDataTables.Name = "editDataTables";
      editDataTables.Size = new System.Drawing.Size( 123, 20 );
      editDataTables.TabIndex = 0;
      // 
      // Disassembler
      // 
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      ClientSize = new System.Drawing.Size( 1159, 688 );
      Controls.Add( groupBox6 );
      Controls.Add( groupBox5 );
      Controls.Add( groupBox3 );
      Controls.Add( tabContent );
      Controls.Add( groupBox4 );
      Controls.Add( groupBox2 );
      Controls.Add( groupBox1 );
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "Disassembler";
      ShowIcon = false;
      ShowInTaskbar = false;
      Text = "Disassembler";
      ( (System.ComponentModel.ISupportInitialize)m_FileWatcher ).EndInit();
      ( (System.ComponentModel.ISupportInitialize)editDisassembly ).EndInit();
      contextMenuDisassembler.ResumeLayout( false );
      panel1.ResumeLayout( false );
      groupBox1.ResumeLayout( false );
      groupBox1.PerformLayout();
      groupBox2.ResumeLayout( false );
      groupBox2.PerformLayout();
      tabContent.ResumeLayout( false );
      tabDisassembly.ResumeLayout( false );
      tabBinary.ResumeLayout( false );
      groupBox3.ResumeLayout( false );
      groupBox3.PerformLayout();
      groupBox4.ResumeLayout( false );
      groupBox4.PerformLayout();
      groupBox5.ResumeLayout( false );
      groupBox5.PerformLayout();
      groupBox6.ResumeLayout( false );
      groupBox6.PerformLayout();
      ResumeLayout( false );

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
    private System.Windows.Forms.CheckBox checkOnlyAddUsedLabels;
  }
}