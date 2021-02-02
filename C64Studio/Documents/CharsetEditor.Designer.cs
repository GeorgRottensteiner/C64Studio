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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CharsetEditor));
      this.tabCharsetEditor = new System.Windows.Forms.TabControl();
      this.tabCharEditor = new System.Windows.Forms.TabPage();
      this.characterEditor = new C64Studio.Controls.CharacterEditor();
      this.tabProject = new System.Windows.Forms.TabPage();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.btnDefaultLowerCase = new System.Windows.Forms.Button();
      this.btnDefaultUppercase = new System.Windows.Forms.Button();
      this.btnClearImportData = new System.Windows.Forms.Button();
      this.btnImportFromBASICHex = new System.Windows.Forms.Button();
      this.btnImportFromBASIC = new System.Windows.Forms.Button();
      this.btnImportFromAssembly = new System.Windows.Forms.Button();
      this.btnImportCharsetFromImage = new System.Windows.Forms.Button();
      this.btnImportFromFile = new System.Windows.Forms.Button();
      this.editDataImport = new System.Windows.Forms.TextBox();
      this.groupExport = new System.Windows.Forms.GroupBox();
      this.editExportBASICLineOffset = new System.Windows.Forms.TextBox();
      this.editExportBASICLineNo = new System.Windows.Forms.TextBox();
      this.label7 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.comboExportRange = new System.Windows.Forms.ComboBox();
      this.editPrefix = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.editCharactersCount = new System.Windows.Forms.TextBox();
      this.editPrefixLoadAddress = new System.Windows.Forms.TextBox();
      this.editCharactersFrom = new System.Windows.Forms.TextBox();
      this.editWrapByteCount = new System.Windows.Forms.TextBox();
      this.labelCharactersTo = new System.Windows.Forms.Label();
      this.labelCharactersFrom = new System.Windows.Forms.Label();
      this.checkExportToDataWrap = new System.Windows.Forms.CheckBox();
      this.checkIncludeColor = new System.Windows.Forms.CheckBox();
      this.checkPrefixLoadAddress = new System.Windows.Forms.CheckBox();
      this.checkExportToDataIncludeRes = new System.Windows.Forms.CheckBox();
      this.editDataExport = new System.Windows.Forms.TextBox();
      this.button1 = new System.Windows.Forms.Button();
      this.btnExportToBASICHex = new System.Windows.Forms.Button();
      this.btnExportToBASIC = new System.Windows.Forms.Button();
      this.btnExportToData = new System.Windows.Forms.Button();
      this.btnExportCharset = new System.Windows.Forms.Button();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.openCharsetProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveCharsetProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.closeCharsetProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.exchangeMultiColors1And2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exchangeMultiColor1AndBGColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exchangeMultiColor2AndBGColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      this.tabCharsetEditor.SuspendLayout();
      this.tabCharEditor.SuspendLayout();
      this.tabProject.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.groupExport.SuspendLayout();
      this.menuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabCharsetEditor
      // 
      this.tabCharsetEditor.Controls.Add(this.tabCharEditor);
      this.tabCharsetEditor.Controls.Add(this.tabProject);
      this.tabCharsetEditor.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabCharsetEditor.Location = new System.Drawing.Point(0, 24);
      this.tabCharsetEditor.Name = "tabCharsetEditor";
      this.tabCharsetEditor.SelectedIndex = 0;
      this.tabCharsetEditor.Size = new System.Drawing.Size(1064, 503);
      this.tabCharsetEditor.TabIndex = 0;
      // 
      // tabCharEditor
      // 
      this.tabCharEditor.Controls.Add(this.characterEditor);
      this.tabCharEditor.Location = new System.Drawing.Point(4, 22);
      this.tabCharEditor.Name = "tabCharEditor";
      this.tabCharEditor.Size = new System.Drawing.Size(1056, 477);
      this.tabCharEditor.TabIndex = 3;
      this.tabCharEditor.Text = "Editor";
      this.tabCharEditor.UseVisualStyleBackColor = true;
      // 
      // characterEditor
      // 
      this.characterEditor.Dock = System.Windows.Forms.DockStyle.Fill;
      this.characterEditor.Location = new System.Drawing.Point(0, 0);
      this.characterEditor.Name = "characterEditor";
      this.characterEditor.Size = new System.Drawing.Size(1056, 477);
      this.characterEditor.TabIndex = 0;
      this.characterEditor.Modified += new C64Studio.Controls.CharacterEditor.ModifiedHandler(this.characterEditor_Modified);
      // 
      // tabProject
      // 
      this.tabProject.Controls.Add(this.groupBox1);
      this.tabProject.Controls.Add(this.groupExport);
      this.tabProject.Location = new System.Drawing.Point(4, 22);
      this.tabProject.Name = "tabProject";
      this.tabProject.Padding = new System.Windows.Forms.Padding(3);
      this.tabProject.Size = new System.Drawing.Size(1056, 477);
      this.tabProject.TabIndex = 1;
      this.tabProject.Text = "Import/Export";
      this.tabProject.UseVisualStyleBackColor = true;
      // 
      // groupBox1
      // 
      this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox1.Controls.Add(this.btnDefaultLowerCase);
      this.groupBox1.Controls.Add(this.btnDefaultUppercase);
      this.groupBox1.Controls.Add(this.btnClearImportData);
      this.groupBox1.Controls.Add(this.btnImportFromBASICHex);
      this.groupBox1.Controls.Add(this.btnImportFromBASIC);
      this.groupBox1.Controls.Add(this.btnImportFromAssembly);
      this.groupBox1.Controls.Add(this.btnImportCharsetFromImage);
      this.groupBox1.Controls.Add(this.btnImportFromFile);
      this.groupBox1.Controls.Add(this.editDataImport);
      this.groupBox1.Location = new System.Drawing.Point(453, 6);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(595, 465);
      this.groupBox1.TabIndex = 4;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Import";
      // 
      // btnDefaultLowerCase
      // 
      this.btnDefaultLowerCase.Location = new System.Drawing.Point(129, 77);
      this.btnDefaultLowerCase.Name = "btnDefaultLowerCase";
      this.btnDefaultLowerCase.Size = new System.Drawing.Size(117, 23);
      this.btnDefaultLowerCase.TabIndex = 2;
      this.btnDefaultLowerCase.Text = "Default Lowercase";
      this.btnDefaultLowerCase.UseVisualStyleBackColor = true;
      this.btnDefaultLowerCase.Click += new System.EventHandler(this.btnDefaultLowercase_Click);
      // 
      // btnDefaultUppercase
      // 
      this.btnDefaultUppercase.Location = new System.Drawing.Point(6, 77);
      this.btnDefaultUppercase.Name = "btnDefaultUppercase";
      this.btnDefaultUppercase.Size = new System.Drawing.Size(117, 23);
      this.btnDefaultUppercase.TabIndex = 2;
      this.btnDefaultUppercase.Text = "Default Uppercase";
      this.btnDefaultUppercase.UseVisualStyleBackColor = true;
      this.btnDefaultUppercase.Click += new System.EventHandler(this.btnDefaultUppercase_Click);
      // 
      // btnClearImportData
      // 
      this.btnClearImportData.Location = new System.Drawing.Point(129, 106);
      this.btnClearImportData.Name = "btnClearImportData";
      this.btnClearImportData.Size = new System.Drawing.Size(117, 23);
      this.btnClearImportData.TabIndex = 2;
      this.btnClearImportData.Text = "Clear";
      this.btnClearImportData.UseVisualStyleBackColor = true;
      this.btnClearImportData.Click += new System.EventHandler(this.btnClearImportData_Click);
      // 
      // btnImportFromBASICHex
      // 
      this.btnImportFromBASICHex.Location = new System.Drawing.Point(252, 48);
      this.btnImportFromBASICHex.Name = "btnImportFromBASICHex";
      this.btnImportFromBASICHex.Size = new System.Drawing.Size(117, 23);
      this.btnImportFromBASICHex.TabIndex = 2;
      this.btnImportFromBASICHex.Text = "From BASIC Hex";
      this.btnImportFromBASICHex.UseVisualStyleBackColor = true;
      this.btnImportFromBASICHex.Click += new System.EventHandler(this.btnImportCharsetFromBASICHex_Click);
      // 
      // btnImportFromBASIC
      // 
      this.btnImportFromBASIC.Location = new System.Drawing.Point(129, 48);
      this.btnImportFromBASIC.Name = "btnImportFromBASIC";
      this.btnImportFromBASIC.Size = new System.Drawing.Size(117, 23);
      this.btnImportFromBASIC.TabIndex = 2;
      this.btnImportFromBASIC.Text = "From BASIC";
      this.btnImportFromBASIC.UseVisualStyleBackColor = true;
      this.btnImportFromBASIC.Click += new System.EventHandler(this.btnImportCharsetFromBASIC_Click);
      // 
      // btnImportFromAssembly
      // 
      this.btnImportFromAssembly.Location = new System.Drawing.Point(6, 48);
      this.btnImportFromAssembly.Name = "btnImportFromAssembly";
      this.btnImportFromAssembly.Size = new System.Drawing.Size(117, 23);
      this.btnImportFromAssembly.TabIndex = 2;
      this.btnImportFromAssembly.Text = "From ASM";
      this.btnImportFromAssembly.UseVisualStyleBackColor = true;
      this.btnImportFromAssembly.Click += new System.EventHandler(this.btnImportCharsetFromASM_Click);
      // 
      // btnImportCharsetFromImage
      // 
      this.btnImportCharsetFromImage.Location = new System.Drawing.Point(129, 19);
      this.btnImportCharsetFromImage.Name = "btnImportCharsetFromImage";
      this.btnImportCharsetFromImage.Size = new System.Drawing.Size(117, 23);
      this.btnImportCharsetFromImage.TabIndex = 2;
      this.btnImportCharsetFromImage.Text = "From Image...";
      this.btnImportCharsetFromImage.UseVisualStyleBackColor = true;
      this.btnImportCharsetFromImage.Click += new System.EventHandler(this.btnImportCharsetFromFile_Click);
      // 
      // btnImportFromFile
      // 
      this.btnImportFromFile.Location = new System.Drawing.Point(6, 19);
      this.btnImportFromFile.Name = "btnImportFromFile";
      this.btnImportFromFile.Size = new System.Drawing.Size(117, 23);
      this.btnImportFromFile.TabIndex = 2;
      this.btnImportFromFile.Text = "From File...";
      this.btnImportFromFile.UseVisualStyleBackColor = true;
      this.btnImportFromFile.Click += new System.EventHandler(this.btnImportCharset_Click);
      // 
      // editDataImport
      // 
      this.editDataImport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.editDataImport.Location = new System.Drawing.Point(6, 145);
      this.editDataImport.Multiline = true;
      this.editDataImport.Name = "editDataImport";
      this.editDataImport.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.editDataImport.Size = new System.Drawing.Size(583, 314);
      this.editDataImport.TabIndex = 3;
      this.editDataImport.WordWrap = false;
      this.editDataImport.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editDataImport_KeyPress);
      // 
      // groupExport
      // 
      this.groupExport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.groupExport.Controls.Add(this.editExportBASICLineOffset);
      this.groupExport.Controls.Add(this.editExportBASICLineNo);
      this.groupExport.Controls.Add(this.label7);
      this.groupExport.Controls.Add(this.label6);
      this.groupExport.Controls.Add(this.comboExportRange);
      this.groupExport.Controls.Add(this.editPrefix);
      this.groupExport.Controls.Add(this.label2);
      this.groupExport.Controls.Add(this.editCharactersCount);
      this.groupExport.Controls.Add(this.editPrefixLoadAddress);
      this.groupExport.Controls.Add(this.editCharactersFrom);
      this.groupExport.Controls.Add(this.editWrapByteCount);
      this.groupExport.Controls.Add(this.labelCharactersTo);
      this.groupExport.Controls.Add(this.labelCharactersFrom);
      this.groupExport.Controls.Add(this.checkExportToDataWrap);
      this.groupExport.Controls.Add(this.checkIncludeColor);
      this.groupExport.Controls.Add(this.checkPrefixLoadAddress);
      this.groupExport.Controls.Add(this.checkExportToDataIncludeRes);
      this.groupExport.Controls.Add(this.editDataExport);
      this.groupExport.Controls.Add(this.button1);
      this.groupExport.Controls.Add(this.btnExportToBASICHex);
      this.groupExport.Controls.Add(this.btnExportToBASIC);
      this.groupExport.Controls.Add(this.btnExportToData);
      this.groupExport.Controls.Add(this.btnExportCharset);
      this.groupExport.Location = new System.Drawing.Point(6, 6);
      this.groupExport.Name = "groupExport";
      this.groupExport.Size = new System.Drawing.Size(441, 465);
      this.groupExport.TabIndex = 3;
      this.groupExport.TabStop = false;
      this.groupExport.Text = "Export";
      // 
      // editExportBASICLineOffset
      // 
      this.editExportBASICLineOffset.Location = new System.Drawing.Point(359, 137);
      this.editExportBASICLineOffset.Name = "editExportBASICLineOffset";
      this.editExportBASICLineOffset.Size = new System.Drawing.Size(73, 20);
      this.editExportBASICLineOffset.TabIndex = 10;
      this.editExportBASICLineOffset.Text = "10";
      // 
      // editExportBASICLineNo
      // 
      this.editExportBASICLineNo.Location = new System.Drawing.Point(188, 137);
      this.editExportBASICLineNo.Name = "editExportBASICLineNo";
      this.editExportBASICLineNo.Size = new System.Drawing.Size(98, 20);
      this.editExportBASICLineNo.TabIndex = 10;
      this.editExportBASICLineNo.Text = "10";
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(292, 140);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(55, 13);
      this.label7.TabIndex = 9;
      this.label7.Text = "Line Step:";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(135, 140);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(55, 13);
      this.label6.TabIndex = 9;
      this.label6.Text = "Start Line:";
      // 
      // comboExportRange
      // 
      this.comboExportRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboExportRange.FormattingEnabled = true;
      this.comboExportRange.Location = new System.Drawing.Point(138, 21);
      this.comboExportRange.Name = "comboExportRange";
      this.comboExportRange.Size = new System.Drawing.Size(88, 21);
      this.comboExportRange.TabIndex = 8;
      this.comboExportRange.SelectedIndexChanged += new System.EventHandler(this.comboExportRange_SelectedIndexChanged);
      // 
      // editPrefix
      // 
      this.editPrefix.Location = new System.Drawing.Point(232, 50);
      this.editPrefix.Name = "editPrefix";
      this.editPrefix.Size = new System.Drawing.Size(54, 20);
      this.editPrefix.TabIndex = 7;
      this.editPrefix.Text = "!byte ";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(292, 80);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(32, 13);
      this.label2.TabIndex = 6;
      this.label2.Text = "bytes";
      // 
      // editCharactersCount
      // 
      this.editCharactersCount.Location = new System.Drawing.Point(376, 21);
      this.editCharactersCount.Name = "editCharactersCount";
      this.editCharactersCount.Size = new System.Drawing.Size(56, 20);
      this.editCharactersCount.TabIndex = 1;
      this.editCharactersCount.TextChanged += new System.EventHandler(this.editUsedCharacters_TextChanged);
      // 
      // editPrefixLoadAddress
      // 
      this.editPrefixLoadAddress.Location = new System.Drawing.Point(271, 107);
      this.editPrefixLoadAddress.Name = "editPrefixLoadAddress";
      this.editPrefixLoadAddress.Size = new System.Drawing.Size(56, 20);
      this.editPrefixLoadAddress.TabIndex = 1;
      this.editPrefixLoadAddress.TextChanged += new System.EventHandler(this.editPrefixLoadAddress_TextChanged);
      // 
      // editCharactersFrom
      // 
      this.editCharactersFrom.Location = new System.Drawing.Point(271, 21);
      this.editCharactersFrom.Name = "editCharactersFrom";
      this.editCharactersFrom.Size = new System.Drawing.Size(56, 20);
      this.editCharactersFrom.TabIndex = 1;
      this.editCharactersFrom.TextChanged += new System.EventHandler(this.editStartCharacters_TextChanged);
      // 
      // editWrapByteCount
      // 
      this.editWrapByteCount.Enabled = false;
      this.editWrapByteCount.Location = new System.Drawing.Point(232, 77);
      this.editWrapByteCount.Name = "editWrapByteCount";
      this.editWrapByteCount.Size = new System.Drawing.Size(54, 20);
      this.editWrapByteCount.TabIndex = 5;
      this.editWrapByteCount.Text = "8";
      // 
      // labelCharactersTo
      // 
      this.labelCharactersTo.AutoSize = true;
      this.labelCharactersTo.Location = new System.Drawing.Point(333, 24);
      this.labelCharactersTo.Name = "labelCharactersTo";
      this.labelCharactersTo.Size = new System.Drawing.Size(37, 13);
      this.labelCharactersTo.TabIndex = 0;
      this.labelCharactersTo.Text = "count:";
      // 
      // labelCharactersFrom
      // 
      this.labelCharactersFrom.AutoSize = true;
      this.labelCharactersFrom.Location = new System.Drawing.Point(235, 24);
      this.labelCharactersFrom.Name = "labelCharactersFrom";
      this.labelCharactersFrom.Size = new System.Drawing.Size(30, 13);
      this.labelCharactersFrom.TabIndex = 0;
      this.labelCharactersFrom.Text = "from:";
      // 
      // checkExportToDataWrap
      // 
      this.checkExportToDataWrap.AutoSize = true;
      this.checkExportToDataWrap.Location = new System.Drawing.Point(138, 81);
      this.checkExportToDataWrap.Name = "checkExportToDataWrap";
      this.checkExportToDataWrap.Size = new System.Drawing.Size(64, 17);
      this.checkExportToDataWrap.TabIndex = 4;
      this.checkExportToDataWrap.Text = "Wrap at";
      this.checkExportToDataWrap.UseVisualStyleBackColor = true;
      this.checkExportToDataWrap.CheckedChanged += new System.EventHandler(this.checkExportToDataWrap_CheckedChanged);
      // 
      // checkIncludeColor
      // 
      this.checkIncludeColor.AutoSize = true;
      this.checkIncludeColor.Location = new System.Drawing.Point(336, 52);
      this.checkIncludeColor.Name = "checkIncludeColor";
      this.checkIncludeColor.Size = new System.Drawing.Size(88, 17);
      this.checkIncludeColor.TabIndex = 4;
      this.checkIncludeColor.Text = "Include Color";
      this.checkIncludeColor.UseVisualStyleBackColor = true;
      // 
      // checkPrefixLoadAddress
      // 
      this.checkPrefixLoadAddress.AutoSize = true;
      this.checkPrefixLoadAddress.Location = new System.Drawing.Point(138, 110);
      this.checkPrefixLoadAddress.Name = "checkPrefixLoadAddress";
      this.checkPrefixLoadAddress.Size = new System.Drawing.Size(117, 17);
      this.checkPrefixLoadAddress.TabIndex = 4;
      this.checkPrefixLoadAddress.Text = "Load Address (hex)";
      this.checkPrefixLoadAddress.UseVisualStyleBackColor = true;
      // 
      // checkExportToDataIncludeRes
      // 
      this.checkExportToDataIncludeRes.AutoSize = true;
      this.checkExportToDataIncludeRes.Location = new System.Drawing.Point(138, 52);
      this.checkExportToDataIncludeRes.Name = "checkExportToDataIncludeRes";
      this.checkExportToDataIncludeRes.Size = new System.Drawing.Size(74, 17);
      this.checkExportToDataIncludeRes.TabIndex = 4;
      this.checkExportToDataIncludeRes.Text = "Prefix with";
      this.checkExportToDataIncludeRes.UseVisualStyleBackColor = true;
      this.checkExportToDataIncludeRes.CheckedChanged += new System.EventHandler(this.checkExportToDataIncludeRes_CheckedChanged);
      // 
      // editDataExport
      // 
      this.editDataExport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.editDataExport.Location = new System.Drawing.Point(6, 179);
      this.editDataExport.Multiline = true;
      this.editDataExport.Name = "editDataExport";
      this.editDataExport.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.editDataExport.Size = new System.Drawing.Size(429, 280);
      this.editDataExport.TabIndex = 3;
      this.editDataExport.WordWrap = false;
      this.editDataExport.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editDataExport_KeyPress);
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(6, 77);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(117, 23);
      this.button1.TabIndex = 2;
      this.button1.Text = "To Image...";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.btnExportCharsetToImage_Click);
      // 
      // btnExportToBASICHex
      // 
      this.btnExportToBASICHex.Location = new System.Drawing.Point(6, 135);
      this.btnExportToBASICHex.Name = "btnExportToBASICHex";
      this.btnExportToBASICHex.Size = new System.Drawing.Size(117, 23);
      this.btnExportToBASICHex.TabIndex = 2;
      this.btnExportToBASICHex.Text = "To BASIC hex";
      this.btnExportToBASICHex.UseVisualStyleBackColor = true;
      this.btnExportToBASICHex.Click += new System.EventHandler(this.btnExportCharsetToBASICHex_Click);
      // 
      // btnExportToBASIC
      // 
      this.btnExportToBASIC.Location = new System.Drawing.Point(6, 106);
      this.btnExportToBASIC.Name = "btnExportToBASIC";
      this.btnExportToBASIC.Size = new System.Drawing.Size(117, 23);
      this.btnExportToBASIC.TabIndex = 2;
      this.btnExportToBASIC.Text = "To BASIC";
      this.btnExportToBASIC.UseVisualStyleBackColor = true;
      this.btnExportToBASIC.Click += new System.EventHandler(this.btnExportCharsetToBASIC_Click);
      // 
      // btnExportToData
      // 
      this.btnExportToData.Location = new System.Drawing.Point(6, 48);
      this.btnExportToData.Name = "btnExportToData";
      this.btnExportToData.Size = new System.Drawing.Size(117, 23);
      this.btnExportToData.TabIndex = 2;
      this.btnExportToData.Text = "To Data";
      this.btnExportToData.UseVisualStyleBackColor = true;
      this.btnExportToData.Click += new System.EventHandler(this.btnExportCharsetToData_Click);
      // 
      // btnExportCharset
      // 
      this.btnExportCharset.Location = new System.Drawing.Point(6, 19);
      this.btnExportCharset.Name = "btnExportCharset";
      this.btnExportCharset.Size = new System.Drawing.Size(117, 23);
      this.btnExportCharset.TabIndex = 2;
      this.btnExportCharset.Text = "To File...";
      this.btnExportCharset.UseVisualStyleBackColor = true;
      this.btnExportCharset.Click += new System.EventHandler(this.btnExportCharset_Click);
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(1064, 24);
      this.menuStrip1.TabIndex = 1;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openCharsetProjectToolStripMenuItem,
            this.saveCharsetProjectToolStripMenuItem,
            this.closeCharsetProjectToolStripMenuItem,
            this.toolStripSeparator1,
            this.exchangeMultiColors1And2ToolStripMenuItem,
            this.exchangeMultiColor1AndBGColorToolStripMenuItem,
            this.exchangeMultiColor2AndBGColorToolStripMenuItem});
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
      this.fileToolStripMenuItem.Text = "&Characters";
      // 
      // openCharsetProjectToolStripMenuItem
      // 
      this.openCharsetProjectToolStripMenuItem.Name = "openCharsetProjectToolStripMenuItem";
      this.openCharsetProjectToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
      this.openCharsetProjectToolStripMenuItem.Text = "&Open Charset Project...";
      this.openCharsetProjectToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
      // 
      // saveCharsetProjectToolStripMenuItem
      // 
      this.saveCharsetProjectToolStripMenuItem.Enabled = false;
      this.saveCharsetProjectToolStripMenuItem.Name = "saveCharsetProjectToolStripMenuItem";
      this.saveCharsetProjectToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
      this.saveCharsetProjectToolStripMenuItem.Text = "&Save Project";
      this.saveCharsetProjectToolStripMenuItem.Click += new System.EventHandler(this.saveCharsetProjectToolStripMenuItem_Click);
      // 
      // closeCharsetProjectToolStripMenuItem
      // 
      this.closeCharsetProjectToolStripMenuItem.Enabled = false;
      this.closeCharsetProjectToolStripMenuItem.Name = "closeCharsetProjectToolStripMenuItem";
      this.closeCharsetProjectToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
      this.closeCharsetProjectToolStripMenuItem.Text = "&Close Charset Project";
      this.closeCharsetProjectToolStripMenuItem.Click += new System.EventHandler(this.closeCharsetProjectToolStripMenuItem_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(263, 6);
      // 
      // exchangeMultiColors1And2ToolStripMenuItem
      // 
      this.exchangeMultiColors1And2ToolStripMenuItem.Name = "exchangeMultiColors1And2ToolStripMenuItem";
      this.exchangeMultiColors1And2ToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
      this.exchangeMultiColors1And2ToolStripMenuItem.Text = "Exchange Multi colors 1 and 2";
      // 
      // exchangeMultiColor1AndBGColorToolStripMenuItem
      // 
      this.exchangeMultiColor1AndBGColorToolStripMenuItem.Name = "exchangeMultiColor1AndBGColorToolStripMenuItem";
      this.exchangeMultiColor1AndBGColorToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
      this.exchangeMultiColor1AndBGColorToolStripMenuItem.Text = "Exchange Multi color 1 and BG color";
      // 
      // exchangeMultiColor2AndBGColorToolStripMenuItem
      // 
      this.exchangeMultiColor2AndBGColorToolStripMenuItem.Name = "exchangeMultiColor2AndBGColorToolStripMenuItem";
      this.exchangeMultiColor2AndBGColorToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
      this.exchangeMultiColor2AndBGColorToolStripMenuItem.Text = "Exchange Multi color 2 and BG color";
      // 
      // CharsetEditor
      // 
      this.ClientSize = new System.Drawing.Size(1064, 527);
      this.Controls.Add(this.tabCharsetEditor);
      this.Controls.Add(this.menuStrip1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "CharsetEditor";
      this.Text = "Charset Editor";
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      this.tabCharsetEditor.ResumeLayout(false);
      this.tabCharEditor.ResumeLayout(false);
      this.tabProject.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupExport.ResumeLayout(false);
      this.groupExport.PerformLayout();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TabControl tabCharsetEditor;
    private System.Windows.Forms.TabPage tabProject;
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
    private System.Windows.Forms.Button btnImportCharsetFromImage;
    private System.Windows.Forms.ComboBox comboExportRange;
    private System.Windows.Forms.TextBox editCharactersCount;
    private System.Windows.Forms.Label labelCharactersTo;
    private System.Windows.Forms.CheckBox checkIncludeColor;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem exchangeMultiColors1And2ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exchangeMultiColor1AndBGColorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exchangeMultiColor2AndBGColorToolStripMenuItem;
    private System.Windows.Forms.TextBox editExportBASICLineOffset;
    private System.Windows.Forms.TextBox editExportBASICLineNo;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Button btnExportToBASIC;
    private System.Windows.Forms.Button btnImportFromAssembly;
    private System.Windows.Forms.TextBox editDataImport;
    private System.Windows.Forms.Button btnClearImportData;
    private System.Windows.Forms.Button btnExportToBASICHex;
        private System.Windows.Forms.TabPage tabCharEditor;
        private CharacterEditor characterEditor;
    private System.Windows.Forms.Button btnImportFromBASICHex;
    private System.Windows.Forms.Button btnImportFromBASIC;
    private System.Windows.Forms.TextBox editPrefixLoadAddress;
    private System.Windows.Forms.CheckBox checkPrefixLoadAddress;
  }
}
