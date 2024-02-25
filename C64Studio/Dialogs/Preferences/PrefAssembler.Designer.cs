namespace RetroDevStudio.Dialogs.Preferences
{
  partial class PrefAssembler
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
      this.btnExportSettings = new System.Windows.Forms.Button();
      this.btnImportSettings = new System.Windows.Forms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.btmASMLibraryPathBrowse = new System.Windows.Forms.Button();
      this.editASMLibraryPath = new System.Windows.Forms.TextBox();
      this.label30 = new System.Windows.Forms.Label();
      this.asmLibraryPathList = new RetroDevStudio.Controls.ArrangedItemList();
      this.listHacks = new System.Windows.Forms.CheckedListBox();
      this.label34 = new System.Windows.Forms.Label();
      this.listWarningsAsErrors = new System.Windows.Forms.CheckedListBox();
      this.label33 = new System.Windows.Forms.Label();
      this.listIgnoredWarnings = new System.Windows.Forms.CheckedListBox();
      this.label20 = new System.Windows.Forms.Label();
      this.checkASMAutoTruncateLiteralValues = new System.Windows.Forms.CheckBox();
      this.checkLabelFileSkipAssemblerIDLabels = new System.Windows.Forms.CheckBox();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnExportSettings
      // 
      this.btnExportSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnExportSettings.Location = new System.Drawing.Point(819, 642);
      this.btnExportSettings.Name = "btnExportSettings";
      this.btnExportSettings.Size = new System.Drawing.Size(75, 23);
      this.btnExportSettings.TabIndex = 9;
      this.btnExportSettings.Text = "Export here";
      this.btnExportSettings.UseVisualStyleBackColor = true;
      this.btnExportSettings.Click += new System.EventHandler(this.btnExportSettings_Click);
      // 
      // btnImportSettings
      // 
      this.btnImportSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnImportSettings.Location = new System.Drawing.Point(738, 642);
      this.btnImportSettings.Name = "btnImportSettings";
      this.btnImportSettings.Size = new System.Drawing.Size(75, 23);
      this.btnImportSettings.TabIndex = 8;
      this.btnImportSettings.Text = "Import here";
      this.btnImportSettings.UseVisualStyleBackColor = true;
      this.btnImportSettings.Click += new System.EventHandler(this.btnImportSettings_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.btmASMLibraryPathBrowse);
      this.groupBox1.Controls.Add(this.editASMLibraryPath);
      this.groupBox1.Controls.Add(this.label30);
      this.groupBox1.Controls.Add(this.asmLibraryPathList);
      this.groupBox1.Controls.Add(this.listHacks);
      this.groupBox1.Controls.Add(this.label34);
      this.groupBox1.Controls.Add(this.listWarningsAsErrors);
      this.groupBox1.Controls.Add(this.label33);
      this.groupBox1.Controls.Add(this.listIgnoredWarnings);
      this.groupBox1.Controls.Add(this.label20);
      this.groupBox1.Controls.Add(this.checkLabelFileSkipAssemblerIDLabels);
      this.groupBox1.Controls.Add(this.checkASMAutoTruncateLiteralValues);
      this.groupBox1.Controls.Add(this.btnExportSettings);
      this.groupBox1.Controls.Add(this.btnImportSettings);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox1.Location = new System.Drawing.Point(0, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(900, 671);
      this.groupBox1.TabIndex = 18;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Assembler";
      // 
      // btmASMLibraryPathBrowse
      // 
      this.btmASMLibraryPathBrowse.Location = new System.Drawing.Point(830, 592);
      this.btmASMLibraryPathBrowse.Name = "btmASMLibraryPathBrowse";
      this.btmASMLibraryPathBrowse.Size = new System.Drawing.Size(35, 20);
      this.btmASMLibraryPathBrowse.TabIndex = 7;
      this.btmASMLibraryPathBrowse.Text = "...";
      this.btmASMLibraryPathBrowse.UseVisualStyleBackColor = true;
      this.btmASMLibraryPathBrowse.Click += new System.EventHandler(this.btmASMLibraryPathBrowse_Click);
      // 
      // editASMLibraryPath
      // 
      this.editASMLibraryPath.Location = new System.Drawing.Point(15, 593);
      this.editASMLibraryPath.Name = "editASMLibraryPath";
      this.editASMLibraryPath.Size = new System.Drawing.Size(798, 20);
      this.editASMLibraryPath.TabIndex = 6;
      // 
      // label30
      // 
      this.label30.AutoSize = true;
      this.label30.Location = new System.Drawing.Point(6, 392);
      this.label30.Name = "label30";
      this.label30.Size = new System.Drawing.Size(71, 13);
      this.label30.TabIndex = 26;
      this.label30.Text = "Library Paths:";
      // 
      // asmLibraryPathList
      // 
      this.asmLibraryPathList.AddButtonEnabled = true;
      this.asmLibraryPathList.AllowClone = true;
      this.asmLibraryPathList.DeleteButtonEnabled = false;
      this.asmLibraryPathList.HasOwnerDrawColumn = true;
      this.asmLibraryPathList.HighlightColor = System.Drawing.SystemColors.HotTrack;
      this.asmLibraryPathList.HighlightTextColor = System.Drawing.SystemColors.HighlightText;
      this.asmLibraryPathList.Location = new System.Drawing.Point(15, 415);
      this.asmLibraryPathList.Margin = new System.Windows.Forms.Padding(48, 22, 48, 22);
      this.asmLibraryPathList.MoveDownButtonEnabled = false;
      this.asmLibraryPathList.MoveUpButtonEnabled = false;
      this.asmLibraryPathList.MustHaveOneElement = false;
      this.asmLibraryPathList.Name = "asmLibraryPathList";
      this.asmLibraryPathList.SelectedIndex = -1;
      this.asmLibraryPathList.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      this.asmLibraryPathList.SelectionTextColor = System.Drawing.SystemColors.HighlightText;
      this.asmLibraryPathList.Size = new System.Drawing.Size(850, 166);
      this.asmLibraryPathList.TabIndex = 5;
      this.asmLibraryPathList.AddingItem += new RetroDevStudio.Controls.ArrangedItemList.AddingItemEventHandler(this.asmLibraryPathList_AddingItem);
      this.asmLibraryPathList.ItemAdded += new RetroDevStudio.Controls.ArrangedItemList.ItemModifiedEventHandler(this.asmLibraryPathList_ItemAdded);
      this.asmLibraryPathList.ItemRemoved += new RetroDevStudio.Controls.ArrangedItemList.ItemModifiedEventHandler(this.asmLibraryPathList_ItemRemoved);
      this.asmLibraryPathList.ItemMoved += new RetroDevStudio.Controls.ArrangedItemList.ItemExchangedEventHandler(this.asmLibraryPathList_ItemMoved);
      // 
      // listHacks
      // 
      this.listHacks.CheckOnClick = true;
      this.listHacks.FormattingEnabled = true;
      this.listHacks.Location = new System.Drawing.Point(15, 243);
      this.listHacks.Name = "listHacks";
      this.listHacks.Size = new System.Drawing.Size(850, 79);
      this.listHacks.TabIndex = 2;
      this.listHacks.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listHacks_ItemCheck);
      // 
      // label34
      // 
      this.label34.AutoSize = true;
      this.label34.Location = new System.Drawing.Point(3, 227);
      this.label34.Name = "label34";
      this.label34.Size = new System.Drawing.Size(207, 13);
      this.label34.TabIndex = 20;
      this.label34.Text = "Enable Hacks (C64Studio assembler only):";
      // 
      // listWarningsAsErrors
      // 
      this.listWarningsAsErrors.CheckOnClick = true;
      this.listWarningsAsErrors.FormattingEnabled = true;
      this.listWarningsAsErrors.Location = new System.Drawing.Point(15, 135);
      this.listWarningsAsErrors.Name = "listWarningsAsErrors";
      this.listWarningsAsErrors.Size = new System.Drawing.Size(850, 79);
      this.listWarningsAsErrors.TabIndex = 1;
      this.listWarningsAsErrors.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listWarningsAsErrors_ItemCheck);
      // 
      // label33
      // 
      this.label33.AutoSize = true;
      this.label33.Location = new System.Drawing.Point(3, 119);
      this.label33.Name = "label33";
      this.label33.Size = new System.Drawing.Size(127, 13);
      this.label33.TabIndex = 21;
      this.label33.Text = "Treat Warnings as Errors:";
      // 
      // listIgnoredWarnings
      // 
      this.listIgnoredWarnings.CheckOnClick = true;
      this.listIgnoredWarnings.FormattingEnabled = true;
      this.listIgnoredWarnings.Location = new System.Drawing.Point(15, 31);
      this.listIgnoredWarnings.Name = "listIgnoredWarnings";
      this.listIgnoredWarnings.Size = new System.Drawing.Size(850, 79);
      this.listIgnoredWarnings.TabIndex = 0;
      this.listIgnoredWarnings.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listIgnoredWarnings_ItemCheck);
      // 
      // label20
      // 
      this.label20.AutoSize = true;
      this.label20.Location = new System.Drawing.Point(3, 15);
      this.label20.Name = "label20";
      this.label20.Size = new System.Drawing.Size(88, 13);
      this.label20.TabIndex = 22;
      this.label20.Text = "Ignore Warnings:";
      // 
      // checkASMAutoTruncateLiteralValues
      // 
      this.checkASMAutoTruncateLiteralValues.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkASMAutoTruncateLiteralValues.Location = new System.Drawing.Point(6, 336);
      this.checkASMAutoTruncateLiteralValues.Name = "checkASMAutoTruncateLiteralValues";
      this.checkASMAutoTruncateLiteralValues.Size = new System.Drawing.Size(349, 24);
      this.checkASMAutoTruncateLiteralValues.TabIndex = 3;
      this.checkASMAutoTruncateLiteralValues.Text = "Truncate literal values (no warning on overflow)";
      this.checkASMAutoTruncateLiteralValues.UseVisualStyleBackColor = true;
      this.checkASMAutoTruncateLiteralValues.CheckedChanged += new System.EventHandler(this.checkASMAutoTruncateLiteralValues_CheckedChanged);
      // 
      // checkLabelFileSkipAssemblerIDLabels
      // 
      this.checkLabelFileSkipAssemblerIDLabels.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkLabelFileSkipAssemblerIDLabels.Location = new System.Drawing.Point(6, 356);
      this.checkLabelFileSkipAssemblerIDLabels.Name = "checkLabelFileSkipAssemblerIDLabels";
      this.checkLabelFileSkipAssemblerIDLabels.Size = new System.Drawing.Size(349, 24);
      this.checkLabelFileSkipAssemblerIDLabels.TabIndex = 4;
      this.checkLabelFileSkipAssemblerIDLabels.Text = "Do not write assembler ID labels in label file";
      this.checkLabelFileSkipAssemblerIDLabels.UseVisualStyleBackColor = true;
      this.checkLabelFileSkipAssemblerIDLabels.CheckedChanged += new System.EventHandler(this.checkLabelFileSkipAssemblerIDLabels_CheckedChanged);
      // 
      // PrefAssembler
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBox1);
      this.Name = "PrefAssembler";
      this.Size = new System.Drawing.Size(900, 671);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);

    }

        #endregion

        private System.Windows.Forms.Button btnExportSettings;
        private System.Windows.Forms.Button btnImportSettings;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkASMAutoTruncateLiteralValues;
        private System.Windows.Forms.CheckedListBox listHacks;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.CheckedListBox listWarningsAsErrors;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.CheckedListBox listIgnoredWarnings;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Button btmASMLibraryPathBrowse;
        private System.Windows.Forms.TextBox editASMLibraryPath;
        private System.Windows.Forms.Label label30;
        private Controls.ArrangedItemList asmLibraryPathList;
    private System.Windows.Forms.CheckBox checkLabelFileSkipAssemblerIDLabels;
  }
}
