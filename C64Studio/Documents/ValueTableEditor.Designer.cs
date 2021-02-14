namespace C64Studio
{
  partial class ValueTableEditor
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ValueTableEditor));
      this.tabValueTableEditor = new System.Windows.Forms.TabControl();
      this.tabEditor = new System.Windows.Forms.TabPage();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.listValues = new C64Studio.ArrangedItemList();
      this.groupBox4 = new System.Windows.Forms.GroupBox();
      this.editValueEntry = new System.Windows.Forms.TextBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.labelGenerationResult = new System.Windows.Forms.Label();
      this.pictureGraphPreview = new GR.Forms.FastPictureBox();
      this.checkGenerateDeltas = new System.Windows.Forms.CheckBox();
      this.checkAutomatedGeneration = new System.Windows.Forms.CheckBox();
      this.checkClearPreviousValues = new System.Windows.Forms.CheckBox();
      this.btnGenerateValues = new System.Windows.Forms.Button();
      this.editStepValue = new System.Windows.Forms.TextBox();
      this.editEndValue = new System.Windows.Forms.TextBox();
      this.label6 = new System.Windows.Forms.Label();
      this.editStartValue = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.editValueFunction = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.tabProject = new System.Windows.Forms.TabPage();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.btnImportFromFile = new System.Windows.Forms.Button();
      this.btnImportFromASM = new System.Windows.Forms.Button();
      this.btnImportFromHex = new System.Windows.Forms.Button();
      this.groupExport = new System.Windows.Forms.GroupBox();
      this.editExportBASICLineOffset = new System.Windows.Forms.TextBox();
      this.editExportBASICLineNo = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label8 = new System.Windows.Forms.Label();
      this.btnExportToBASICHex = new System.Windows.Forms.Button();
      this.btnExportToBASICData = new System.Windows.Forms.Button();
      this.editPrefix = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.editWrapByteCount = new System.Windows.Forms.TextBox();
      this.checkExportToDataWrap = new System.Windows.Forms.CheckBox();
      this.checkExportToDataIncludeRes = new System.Windows.Forms.CheckBox();
      this.btnExportToData = new System.Windows.Forms.Button();
      this.btnExportToFile = new System.Windows.Forms.Button();
      this.editDataExport = new System.Windows.Forms.TextBox();
      this.contextMenuExchangeColors = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.forAllSpritesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.forSelectedSpritesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.openValueTableProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveValueTableProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.closeValueTableProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      this.tabValueTableEditor.SuspendLayout();
      this.tabEditor.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.groupBox4.SuspendLayout();
      this.groupBox2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureGraphPreview)).BeginInit();
      this.tabProject.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.groupExport.SuspendLayout();
      this.contextMenuExchangeColors.SuspendLayout();
      this.menuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabValueTableEditor
      // 
      this.tabValueTableEditor.Controls.Add(this.tabEditor);
      this.tabValueTableEditor.Controls.Add(this.tabProject);
      this.tabValueTableEditor.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabValueTableEditor.Location = new System.Drawing.Point(0, 24);
      this.tabValueTableEditor.Name = "tabValueTableEditor";
      this.tabValueTableEditor.SelectedIndex = 0;
      this.tabValueTableEditor.Size = new System.Drawing.Size(994, 503);
      this.tabValueTableEditor.TabIndex = 0;
      // 
      // tabEditor
      // 
      this.tabEditor.Controls.Add(this.groupBox3);
      this.tabEditor.Controls.Add(this.groupBox2);
      this.tabEditor.Location = new System.Drawing.Point(4, 22);
      this.tabEditor.Name = "tabEditor";
      this.tabEditor.Padding = new System.Windows.Forms.Padding(3);
      this.tabEditor.Size = new System.Drawing.Size(986, 477);
      this.tabEditor.TabIndex = 0;
      this.tabEditor.Text = "Value Table";
      this.tabEditor.UseVisualStyleBackColor = true;
      // 
      // groupBox3
      // 
      this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.groupBox3.Controls.Add(this.listValues);
      this.groupBox3.Controls.Add(this.groupBox4);
      this.groupBox3.Location = new System.Drawing.Point(8, 6);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(329, 463);
      this.groupBox3.TabIndex = 0;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Value Table";
      // 
      // listValues
      // 
      this.listValues.AddButtonEnabled = true;
      this.listValues.AllowClone = true;
      this.listValues.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.listValues.DeleteButtonEnabled = false;
      this.listValues.HasOwnerDrawColumn = true;
      this.listValues.HighlightColor = System.Drawing.SystemColors.HotTrack;
      this.listValues.HighlightTextColor = System.Drawing.SystemColors.HighlightText;
      this.listValues.Location = new System.Drawing.Point(6, 19);
      this.listValues.MoveDownButtonEnabled = false;
      this.listValues.MoveUpButtonEnabled = false;
      this.listValues.MustHaveOneElement = false;
      this.listValues.Name = "listValues";
      this.listValues.SelectedIndex = -1;
      this.listValues.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      this.listValues.SelectionTextColor = System.Drawing.SystemColors.HighlightText;
      this.listValues.Size = new System.Drawing.Size(317, 379);
      this.listValues.TabIndex = 0;
      this.listValues.AddingItem += new C64Studio.ArrangedItemList.AddingItemEventHandler(this.listValues_AddingItem);
      this.listValues.CloningItem += new C64Studio.ArrangedItemList.CloningItemEventHandler(this.listValues_CloningItem);
      this.listValues.ItemAdded += new C64Studio.ArrangedItemList.ItemModifiedEventHandler(this.listValues_ItemAdded);
      this.listValues.ItemRemoved += new C64Studio.ArrangedItemList.ItemModifiedEventHandler(this.listValues_ItemRemoved);
      this.listValues.ItemMoved += new C64Studio.ArrangedItemList.ItemExchangedEventHandler(this.listValues_ItemMoved);
      this.listValues.SelectedIndexChanged += new C64Studio.ArrangedItemList.ItemModifiedEventHandler(this.listValues_SelectedIndexChanged);
      // 
      // groupBox4
      // 
      this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.groupBox4.Controls.Add(this.editValueEntry);
      this.groupBox4.Location = new System.Drawing.Point(6, 404);
      this.groupBox4.Name = "groupBox4";
      this.groupBox4.Size = new System.Drawing.Size(317, 53);
      this.groupBox4.TabIndex = 0;
      this.groupBox4.TabStop = false;
      this.groupBox4.Text = "Value Entry";
      // 
      // editValueEntry
      // 
      this.editValueEntry.Location = new System.Drawing.Point(6, 19);
      this.editValueEntry.Name = "editValueEntry";
      this.editValueEntry.Size = new System.Drawing.Size(305, 20);
      this.editValueEntry.TabIndex = 0;
      this.editValueEntry.TextChanged += new System.EventHandler(this.editValueEntry_TextChanged);
      // 
      // groupBox2
      // 
      this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox2.Controls.Add(this.labelGenerationResult);
      this.groupBox2.Controls.Add(this.pictureGraphPreview);
      this.groupBox2.Controls.Add(this.checkGenerateDeltas);
      this.groupBox2.Controls.Add(this.checkAutomatedGeneration);
      this.groupBox2.Controls.Add(this.checkClearPreviousValues);
      this.groupBox2.Controls.Add(this.btnGenerateValues);
      this.groupBox2.Controls.Add(this.editStepValue);
      this.groupBox2.Controls.Add(this.editEndValue);
      this.groupBox2.Controls.Add(this.label6);
      this.groupBox2.Controls.Add(this.editStartValue);
      this.groupBox2.Controls.Add(this.label5);
      this.groupBox2.Controls.Add(this.editValueFunction);
      this.groupBox2.Controls.Add(this.label4);
      this.groupBox2.Controls.Add(this.label3);
      this.groupBox2.Location = new System.Drawing.Point(343, 6);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(635, 463);
      this.groupBox2.TabIndex = 0;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Value Generation";
      // 
      // labelGenerationResult
      // 
      this.labelGenerationResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
      this.labelGenerationResult.Location = new System.Drawing.Point(217, 45);
      this.labelGenerationResult.Name = "labelGenerationResult";
      this.labelGenerationResult.Size = new System.Drawing.Size(412, 23);
      this.labelGenerationResult.TabIndex = 5;
      this.labelGenerationResult.Text = "No values generated currently";
      // 
      // pictureGraphPreview
      // 
      this.pictureGraphPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.pictureGraphPreview.AutoResize = true;
      this.pictureGraphPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.pictureGraphPreview.DisplayPage = fastImage1;
      this.pictureGraphPreview.Image = null;
      this.pictureGraphPreview.Location = new System.Drawing.Point(9, 134);
      this.pictureGraphPreview.Name = "pictureGraphPreview";
      this.pictureGraphPreview.Size = new System.Drawing.Size(620, 323);
      this.pictureGraphPreview.TabIndex = 4;
      this.pictureGraphPreview.TabStop = false;
      this.pictureGraphPreview.SizeChanged += new System.EventHandler(this.pictureGraphPreview_SizeChanged);
      // 
      // checkGenerateDeltas
      // 
      this.checkGenerateDeltas.AutoSize = true;
      this.checkGenerateDeltas.Location = new System.Drawing.Point(354, 70);
      this.checkGenerateDeltas.Name = "checkGenerateDeltas";
      this.checkGenerateDeltas.Size = new System.Drawing.Size(98, 17);
      this.checkGenerateDeltas.TabIndex = 4;
      this.checkGenerateDeltas.Text = "Generate Delta";
      this.checkGenerateDeltas.UseVisualStyleBackColor = true;
      this.checkGenerateDeltas.CheckedChanged += new System.EventHandler(this.checkGenerateDeltas_CheckedChanged);
      // 
      // checkAutomatedGeneration
      // 
      this.checkAutomatedGeneration.AutoSize = true;
      this.checkAutomatedGeneration.Checked = true;
      this.checkAutomatedGeneration.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkAutomatedGeneration.Location = new System.Drawing.Point(220, 70);
      this.checkAutomatedGeneration.Name = "checkAutomatedGeneration";
      this.checkAutomatedGeneration.Size = new System.Drawing.Size(128, 17);
      this.checkAutomatedGeneration.TabIndex = 4;
      this.checkAutomatedGeneration.Text = "Automatic Generation";
      this.checkAutomatedGeneration.UseVisualStyleBackColor = true;
      // 
      // checkClearPreviousValues
      // 
      this.checkClearPreviousValues.AutoSize = true;
      this.checkClearPreviousValues.Checked = true;
      this.checkClearPreviousValues.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkClearPreviousValues.Location = new System.Drawing.Point(220, 96);
      this.checkClearPreviousValues.Name = "checkClearPreviousValues";
      this.checkClearPreviousValues.Size = new System.Drawing.Size(129, 17);
      this.checkClearPreviousValues.TabIndex = 5;
      this.checkClearPreviousValues.Text = "Clear Previous Values";
      this.checkClearPreviousValues.UseVisualStyleBackColor = true;
      // 
      // btnGenerateValues
      // 
      this.btnGenerateValues.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnGenerateValues.Location = new System.Drawing.Point(534, 92);
      this.btnGenerateValues.Name = "btnGenerateValues";
      this.btnGenerateValues.Size = new System.Drawing.Size(95, 23);
      this.btnGenerateValues.TabIndex = 6;
      this.btnGenerateValues.Text = "Generate Values";
      this.btnGenerateValues.UseVisualStyleBackColor = true;
      this.btnGenerateValues.Click += new System.EventHandler(this.btnGenerateValues_Click);
      // 
      // editStepValue
      // 
      this.editStepValue.Location = new System.Drawing.Point(82, 94);
      this.editStepValue.Name = "editStepValue";
      this.editStepValue.Size = new System.Drawing.Size(129, 20);
      this.editStepValue.TabIndex = 3;
      this.editStepValue.Text = "1";
      this.editStepValue.TextChanged += new System.EventHandler(this.editStepValue_TextChanged);
      // 
      // editEndValue
      // 
      this.editEndValue.Location = new System.Drawing.Point(82, 68);
      this.editEndValue.Name = "editEndValue";
      this.editEndValue.Size = new System.Drawing.Size(129, 20);
      this.editEndValue.TabIndex = 2;
      this.editEndValue.Text = "10";
      this.editEndValue.TextChanged += new System.EventHandler(this.editEndValue_TextChanged);
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(6, 97);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(62, 13);
      this.label6.TabIndex = 0;
      this.label6.Text = "Step Value:";
      // 
      // editStartValue
      // 
      this.editStartValue.Location = new System.Drawing.Point(82, 42);
      this.editStartValue.Name = "editStartValue";
      this.editStartValue.Size = new System.Drawing.Size(129, 20);
      this.editStartValue.TabIndex = 1;
      this.editStartValue.Text = "0";
      this.editStartValue.TextChanged += new System.EventHandler(this.editStartValue_TextChanged);
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(6, 71);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(59, 13);
      this.label5.TabIndex = 0;
      this.label5.Text = "End Value:";
      // 
      // editValueFunction
      // 
      this.editValueFunction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.editValueFunction.Location = new System.Drawing.Point(82, 16);
      this.editValueFunction.Name = "editValueFunction";
      this.editValueFunction.Size = new System.Drawing.Size(547, 20);
      this.editValueFunction.TabIndex = 0;
      this.editValueFunction.Text = "x*2";
      this.editValueFunction.TextChanged += new System.EventHandler(this.editValueFunction_TextChanged);
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(6, 45);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(62, 13);
      this.label4.TabIndex = 0;
      this.label4.Text = "Start Value:";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(6, 19);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(51, 13);
      this.label3.TabIndex = 0;
      this.label3.Text = "Function:";
      // 
      // tabProject
      // 
      this.tabProject.Controls.Add(this.groupBox1);
      this.tabProject.Controls.Add(this.groupExport);
      this.tabProject.Controls.Add(this.editDataExport);
      this.tabProject.Location = new System.Drawing.Point(4, 22);
      this.tabProject.Name = "tabProject";
      this.tabProject.Padding = new System.Windows.Forms.Padding(3);
      this.tabProject.Size = new System.Drawing.Size(986, 477);
      this.tabProject.TabIndex = 1;
      this.tabProject.Text = "Import/Export";
      this.tabProject.UseVisualStyleBackColor = true;
      // 
      // groupBox1
      // 
      this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox1.Controls.Add(this.btnImportFromFile);
      this.groupBox1.Controls.Add(this.btnImportFromASM);
      this.groupBox1.Controls.Add(this.btnImportFromHex);
      this.groupBox1.Location = new System.Drawing.Point(455, 6);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(523, 149);
      this.groupBox1.TabIndex = 4;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Import";
      // 
      // btnImportFromFile
      // 
      this.btnImportFromFile.Location = new System.Drawing.Point(9, 19);
      this.btnImportFromFile.Name = "btnImportFromFile";
      this.btnImportFromFile.Size = new System.Drawing.Size(117, 23);
      this.btnImportFromFile.TabIndex = 2;
      this.btnImportFromFile.Text = "From File...";
      this.btnImportFromFile.UseVisualStyleBackColor = true;
      this.btnImportFromFile.Click += new System.EventHandler(this.btnImportFromFile_Click);
      // 
      // btnImportFromASM
      // 
      this.btnImportFromASM.Location = new System.Drawing.Point(9, 48);
      this.btnImportFromASM.Name = "btnImportFromASM";
      this.btnImportFromASM.Size = new System.Drawing.Size(117, 23);
      this.btnImportFromASM.TabIndex = 2;
      this.btnImportFromASM.Text = "From ASM";
      this.btnImportFromASM.UseVisualStyleBackColor = true;
      this.btnImportFromASM.Click += new System.EventHandler(this.btnImportFromASM_Click);
      // 
      // btnImportFromHex
      // 
      this.btnImportFromHex.Location = new System.Drawing.Point(9, 77);
      this.btnImportFromHex.Name = "btnImportFromHex";
      this.btnImportFromHex.Size = new System.Drawing.Size(117, 23);
      this.btnImportFromHex.TabIndex = 2;
      this.btnImportFromHex.Text = "From Hex";
      this.btnImportFromHex.UseVisualStyleBackColor = true;
      this.btnImportFromHex.Click += new System.EventHandler(this.btnImportFromHex_Click);
      // 
      // groupExport
      // 
      this.groupExport.Controls.Add(this.editExportBASICLineOffset);
      this.groupExport.Controls.Add(this.editExportBASICLineNo);
      this.groupExport.Controls.Add(this.label1);
      this.groupExport.Controls.Add(this.label8);
      this.groupExport.Controls.Add(this.btnExportToBASICHex);
      this.groupExport.Controls.Add(this.btnExportToBASICData);
      this.groupExport.Controls.Add(this.editPrefix);
      this.groupExport.Controls.Add(this.label2);
      this.groupExport.Controls.Add(this.editWrapByteCount);
      this.groupExport.Controls.Add(this.checkExportToDataWrap);
      this.groupExport.Controls.Add(this.checkExportToDataIncludeRes);
      this.groupExport.Controls.Add(this.btnExportToData);
      this.groupExport.Controls.Add(this.btnExportToFile);
      this.groupExport.Location = new System.Drawing.Point(8, 6);
      this.groupExport.Name = "groupExport";
      this.groupExport.Size = new System.Drawing.Size(441, 172);
      this.groupExport.TabIndex = 3;
      this.groupExport.TabStop = false;
      this.groupExport.Text = "Export";
      // 
      // editExportBASICLineOffset
      // 
      this.editExportBASICLineOffset.Location = new System.Drawing.Point(352, 108);
      this.editExportBASICLineOffset.Name = "editExportBASICLineOffset";
      this.editExportBASICLineOffset.Size = new System.Drawing.Size(73, 20);
      this.editExportBASICLineOffset.TabIndex = 28;
      this.editExportBASICLineOffset.Text = "10";
      // 
      // editExportBASICLineNo
      // 
      this.editExportBASICLineNo.Location = new System.Drawing.Point(181, 108);
      this.editExportBASICLineNo.Name = "editExportBASICLineNo";
      this.editExportBASICLineNo.Size = new System.Drawing.Size(98, 20);
      this.editExportBASICLineNo.TabIndex = 29;
      this.editExportBASICLineNo.Text = "10";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(285, 111);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(55, 13);
      this.label1.TabIndex = 26;
      this.label1.Text = "Line Step:";
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(128, 111);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(55, 13);
      this.label8.TabIndex = 27;
      this.label8.Text = "Start Line:";
      // 
      // btnExportToBASICHex
      // 
      this.btnExportToBASICHex.Location = new System.Drawing.Point(6, 135);
      this.btnExportToBASICHex.Name = "btnExportToBASICHex";
      this.btnExportToBASICHex.Size = new System.Drawing.Size(117, 23);
      this.btnExportToBASICHex.TabIndex = 25;
      this.btnExportToBASICHex.Text = "To BASIC data hex";
      this.btnExportToBASICHex.UseVisualStyleBackColor = true;
      this.btnExportToBASICHex.Click += new System.EventHandler(this.btnExportToBASICDataHex_Click);
      // 
      // btnExportToBASICData
      // 
      this.btnExportToBASICData.Location = new System.Drawing.Point(6, 106);
      this.btnExportToBASICData.Name = "btnExportToBASICData";
      this.btnExportToBASICData.Size = new System.Drawing.Size(117, 23);
      this.btnExportToBASICData.TabIndex = 25;
      this.btnExportToBASICData.Text = "To BASIC data";
      this.btnExportToBASICData.UseVisualStyleBackColor = true;
      this.btnExportToBASICData.Click += new System.EventHandler(this.btnExportToBASICData_Click);
      // 
      // editPrefix
      // 
      this.editPrefix.Location = new System.Drawing.Point(225, 48);
      this.editPrefix.Name = "editPrefix";
      this.editPrefix.Size = new System.Drawing.Size(54, 20);
      this.editPrefix.TabIndex = 7;
      this.editPrefix.Text = "!byte ";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(285, 80);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(32, 13);
      this.label2.TabIndex = 6;
      this.label2.Text = "bytes";
      // 
      // editWrapByteCount
      // 
      this.editWrapByteCount.Enabled = false;
      this.editWrapByteCount.Location = new System.Drawing.Point(225, 77);
      this.editWrapByteCount.Name = "editWrapByteCount";
      this.editWrapByteCount.Size = new System.Drawing.Size(54, 20);
      this.editWrapByteCount.TabIndex = 5;
      this.editWrapByteCount.Text = "8";
      // 
      // checkExportToDataWrap
      // 
      this.checkExportToDataWrap.AutoSize = true;
      this.checkExportToDataWrap.Location = new System.Drawing.Point(129, 79);
      this.checkExportToDataWrap.Name = "checkExportToDataWrap";
      this.checkExportToDataWrap.Size = new System.Drawing.Size(64, 17);
      this.checkExportToDataWrap.TabIndex = 4;
      this.checkExportToDataWrap.Text = "Wrap at";
      this.checkExportToDataWrap.UseVisualStyleBackColor = true;
      this.checkExportToDataWrap.CheckedChanged += new System.EventHandler(this.checkExportToDataWrap_CheckedChanged);
      // 
      // checkExportToDataIncludeRes
      // 
      this.checkExportToDataIncludeRes.AutoSize = true;
      this.checkExportToDataIncludeRes.Location = new System.Drawing.Point(129, 50);
      this.checkExportToDataIncludeRes.Name = "checkExportToDataIncludeRes";
      this.checkExportToDataIncludeRes.Size = new System.Drawing.Size(74, 17);
      this.checkExportToDataIncludeRes.TabIndex = 4;
      this.checkExportToDataIncludeRes.Text = "Prefix with";
      this.checkExportToDataIncludeRes.UseVisualStyleBackColor = true;
      this.checkExportToDataIncludeRes.CheckedChanged += new System.EventHandler(this.checkExportToDataIncludeRes_CheckedChanged);
      // 
      // btnExportToData
      // 
      this.btnExportToData.Location = new System.Drawing.Point(6, 48);
      this.btnExportToData.Name = "btnExportToData";
      this.btnExportToData.Size = new System.Drawing.Size(117, 23);
      this.btnExportToData.TabIndex = 2;
      this.btnExportToData.Text = "To Data";
      this.btnExportToData.UseVisualStyleBackColor = true;
      this.btnExportToData.Click += new System.EventHandler(this.btnExportToData_Click);
      // 
      // btnExportToFile
      // 
      this.btnExportToFile.Location = new System.Drawing.Point(6, 19);
      this.btnExportToFile.Name = "btnExportToFile";
      this.btnExportToFile.Size = new System.Drawing.Size(117, 23);
      this.btnExportToFile.TabIndex = 2;
      this.btnExportToFile.Text = "To File...";
      this.btnExportToFile.UseVisualStyleBackColor = true;
      this.btnExportToFile.Click += new System.EventHandler(this.btnExportToFile_Click);
      // 
      // editDataExport
      // 
      this.editDataExport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.editDataExport.Location = new System.Drawing.Point(8, 195);
      this.editDataExport.Multiline = true;
      this.editDataExport.Name = "editDataExport";
      this.editDataExport.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.editDataExport.Size = new System.Drawing.Size(970, 274);
      this.editDataExport.TabIndex = 3;
      this.editDataExport.WordWrap = false;
      this.editDataExport.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editDataExport_KeyPress);
      // 
      // contextMenuExchangeColors
      // 
      this.contextMenuExchangeColors.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.forAllSpritesToolStripMenuItem,
            this.forSelectedSpritesToolStripMenuItem});
      this.contextMenuExchangeColors.Name = "contextMenuExchangeColors";
      this.contextMenuExchangeColors.Size = new System.Drawing.Size(145, 48);
      // 
      // forAllSpritesToolStripMenuItem
      // 
      this.forAllSpritesToolStripMenuItem.Name = "forAllSpritesToolStripMenuItem";
      this.forAllSpritesToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
      this.forAllSpritesToolStripMenuItem.Text = "For all Sprites";
      // 
      // forSelectedSpritesToolStripMenuItem
      // 
      this.forSelectedSpritesToolStripMenuItem.Name = "forSelectedSpritesToolStripMenuItem";
      this.forSelectedSpritesToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(994, 24);
      this.menuStrip1.TabIndex = 1;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openValueTableProjectToolStripMenuItem,
            this.saveValueTableProjectToolStripMenuItem,
            this.closeValueTableProjectToolStripMenuItem});
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
      this.fileToolStripMenuItem.Text = "&Value Table";
      // 
      // openValueTableProjectToolStripMenuItem
      // 
      this.openValueTableProjectToolStripMenuItem.Name = "openValueTableProjectToolStripMenuItem";
      this.openValueTableProjectToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
      this.openValueTableProjectToolStripMenuItem.Text = "&Open Value Table Project...";
      this.openValueTableProjectToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
      // 
      // saveValueTableProjectToolStripMenuItem
      // 
      this.saveValueTableProjectToolStripMenuItem.Name = "saveValueTableProjectToolStripMenuItem";
      this.saveValueTableProjectToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
      this.saveValueTableProjectToolStripMenuItem.Text = "&Save Project";
      this.saveValueTableProjectToolStripMenuItem.Click += new System.EventHandler(this.saveCharsetProjectToolStripMenuItem_Click);
      // 
      // closeValueTableProjectToolStripMenuItem
      // 
      this.closeValueTableProjectToolStripMenuItem.Enabled = false;
      this.closeValueTableProjectToolStripMenuItem.Name = "closeValueTableProjectToolStripMenuItem";
      this.closeValueTableProjectToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
      this.closeValueTableProjectToolStripMenuItem.Text = "&Close Value Table Project";
      this.closeValueTableProjectToolStripMenuItem.Click += new System.EventHandler(this.closeCharsetProjectToolStripMenuItem_Click);
      // 
      // columnHeader4
      // 
      this.columnHeader4.Text = "Nr.";
      // 
      // columnHeader5
      // 
      this.columnHeader5.Text = "X";
      this.columnHeader5.Width = 30;
      // 
      // columnHeader6
      // 
      this.columnHeader6.Text = "Y";
      this.columnHeader6.Width = 30;
      // 
      // ValueTableEditor
      // 
      this.ClientSize = new System.Drawing.Size(994, 527);
      this.Controls.Add(this.tabValueTableEditor);
      this.Controls.Add(this.menuStrip1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "ValueTableEditor";
      this.Text = "Value Table Editor";
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      this.tabValueTableEditor.ResumeLayout(false);
      this.tabEditor.ResumeLayout(false);
      this.groupBox3.ResumeLayout(false);
      this.groupBox4.ResumeLayout(false);
      this.groupBox4.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureGraphPreview)).EndInit();
      this.tabProject.ResumeLayout(false);
      this.tabProject.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupExport.ResumeLayout(false);
      this.groupExport.PerformLayout();
      this.contextMenuExchangeColors.ResumeLayout(false);
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TabControl tabValueTableEditor;
    private System.Windows.Forms.TabPage tabEditor;
    private System.Windows.Forms.TabPage tabProject;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem openValueTableProjectToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem closeValueTableProjectToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveValueTableProjectToolStripMenuItem;
    private System.Windows.Forms.Button btnExportToFile;
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
    private System.Windows.Forms.ColumnHeader columnHeader4;
    private System.Windows.Forms.ColumnHeader columnHeader5;
    private System.Windows.Forms.ColumnHeader columnHeader6;
    private System.Windows.Forms.ColumnHeader columnHeader7;
    private System.Windows.Forms.ColumnHeader columnHeader8;
    private System.Windows.Forms.ColumnHeader columnHeader9;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.Button btnImportFromHex;
    private System.Windows.Forms.ContextMenuStrip contextMenuExchangeColors;
    private System.Windows.Forms.ToolStripMenuItem forAllSpritesToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem forSelectedSpritesToolStripMenuItem;
    private System.Windows.Forms.TextBox editExportBASICLineOffset;
    private System.Windows.Forms.TextBox editExportBASICLineNo;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Button btnExportToBASICData;
    private System.Windows.Forms.GroupBox groupBox3;
    private ArrangedItemList listValues;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.GroupBox groupBox4;
    private System.Windows.Forms.TextBox editValueEntry;
    private System.Windows.Forms.TextBox editValueFunction;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox editStepValue;
    private System.Windows.Forms.TextBox editEndValue;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TextBox editStartValue;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label4;
    private GR.Forms.FastPictureBox pictureGraphPreview;
    private System.Windows.Forms.CheckBox checkClearPreviousValues;
    private System.Windows.Forms.Button btnGenerateValues;
    private System.Windows.Forms.CheckBox checkAutomatedGeneration;
    private System.Windows.Forms.Label labelGenerationResult;
    private System.Windows.Forms.Button btnImportFromASM;
    private System.Windows.Forms.CheckBox checkGenerateDeltas;
    private System.Windows.Forms.Button btnExportToBASICHex;
  }
}
