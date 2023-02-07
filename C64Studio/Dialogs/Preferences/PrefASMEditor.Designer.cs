namespace RetroDevStudio.Dialogs.Preferences
{
  partial class PrefASMEditor
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
      this.checkASMShowAddress = new System.Windows.Forms.CheckBox();
      this.checkASMShowAutoComplete = new System.Windows.Forms.CheckBox();
      this.checkASMShowMiniMap = new System.Windows.Forms.CheckBox();
      this.checkASMShowSizes = new System.Windows.Forms.CheckBox();
      this.checkASMShowCycles = new System.Windows.Forms.CheckBox();
      this.checkASMShowLineNumbers = new System.Windows.Forms.CheckBox();
      this.btnSetDefaultsFont = new System.Windows.Forms.Button();
      this.btnChooseFont = new System.Windows.Forms.Button();
      this.labelFontPreview = new System.Windows.Forms.Label();
      this.editTabSize = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label14 = new System.Windows.Forms.Label();
      this.checkStripTrailingSpaces = new System.Windows.Forms.CheckBox();
      this.checkConvertTabsToSpaces = new System.Windows.Forms.CheckBox();
      this.comboASMEncoding = new System.Windows.Forms.ComboBox();
      this.label35 = new System.Windows.Forms.Label();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnExportSettings
      // 
      this.btnExportSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnExportSettings.Location = new System.Drawing.Point(819, 378);
      this.btnExportSettings.Name = "btnExportSettings";
      this.btnExportSettings.Size = new System.Drawing.Size(75, 23);
      this.btnExportSettings.TabIndex = 12;
      this.btnExportSettings.Text = "Export here";
      this.btnExportSettings.UseVisualStyleBackColor = true;
      this.btnExportSettings.Click += new System.EventHandler(this.btnExportSettings_Click);
      // 
      // btnImportSettings
      // 
      this.btnImportSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnImportSettings.Location = new System.Drawing.Point(738, 378);
      this.btnImportSettings.Name = "btnImportSettings";
      this.btnImportSettings.Size = new System.Drawing.Size(75, 23);
      this.btnImportSettings.TabIndex = 13;
      this.btnImportSettings.Text = "Import here";
      this.btnImportSettings.UseVisualStyleBackColor = true;
      this.btnImportSettings.Click += new System.EventHandler(this.btnImportSettings_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.comboASMEncoding);
      this.groupBox1.Controls.Add(this.label35);
      this.groupBox1.Controls.Add(this.checkASMShowAddress);
      this.groupBox1.Controls.Add(this.checkASMShowAutoComplete);
      this.groupBox1.Controls.Add(this.checkASMShowMiniMap);
      this.groupBox1.Controls.Add(this.checkASMShowSizes);
      this.groupBox1.Controls.Add(this.checkASMShowCycles);
      this.groupBox1.Controls.Add(this.checkASMShowLineNumbers);
      this.groupBox1.Controls.Add(this.btnSetDefaultsFont);
      this.groupBox1.Controls.Add(this.btnChooseFont);
      this.groupBox1.Controls.Add(this.labelFontPreview);
      this.groupBox1.Controls.Add(this.editTabSize);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.label14);
      this.groupBox1.Controls.Add(this.checkStripTrailingSpaces);
      this.groupBox1.Controls.Add(this.checkConvertTabsToSpaces);
      this.groupBox1.Controls.Add(this.btnExportSettings);
      this.groupBox1.Controls.Add(this.btnImportSettings);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox1.Location = new System.Drawing.Point(0, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(900, 407);
      this.groupBox1.TabIndex = 18;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "ASM Editor";
      // 
      // checkASMShowAddress
      // 
      this.checkASMShowAddress.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkASMShowAddress.Location = new System.Drawing.Point(16, 313);
      this.checkASMShowAddress.Name = "checkASMShowAddress";
      this.checkASMShowAddress.Size = new System.Drawing.Size(279, 24);
      this.checkASMShowAddress.TabIndex = 25;
      this.checkASMShowAddress.Text = "Show Address";
      this.checkASMShowAddress.UseVisualStyleBackColor = true;
      this.checkASMShowAddress.CheckedChanged += new System.EventHandler(this.checkASMShowAddress_CheckedChanged);
      // 
      // checkASMShowAutoComplete
      // 
      this.checkASMShowAutoComplete.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkASMShowAutoComplete.Location = new System.Drawing.Point(16, 283);
      this.checkASMShowAutoComplete.Name = "checkASMShowAutoComplete";
      this.checkASMShowAutoComplete.Size = new System.Drawing.Size(279, 24);
      this.checkASMShowAutoComplete.TabIndex = 26;
      this.checkASMShowAutoComplete.Text = "Show Auto-Complete";
      this.checkASMShowAutoComplete.UseVisualStyleBackColor = true;
      this.checkASMShowAutoComplete.CheckedChanged += new System.EventHandler(this.checkASMShowAutoComplete_CheckedChanged);
      // 
      // checkASMShowMiniMap
      // 
      this.checkASMShowMiniMap.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkASMShowMiniMap.Location = new System.Drawing.Point(16, 253);
      this.checkASMShowMiniMap.Name = "checkASMShowMiniMap";
      this.checkASMShowMiniMap.Size = new System.Drawing.Size(279, 24);
      this.checkASMShowMiniMap.TabIndex = 24;
      this.checkASMShowMiniMap.Text = "Show Mini View";
      this.checkASMShowMiniMap.UseVisualStyleBackColor = true;
      this.checkASMShowMiniMap.CheckedChanged += new System.EventHandler(this.checkASMShowMiniMap_CheckedChanged);
      // 
      // checkASMShowSizes
      // 
      this.checkASMShowSizes.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkASMShowSizes.Location = new System.Drawing.Point(16, 223);
      this.checkASMShowSizes.Name = "checkASMShowSizes";
      this.checkASMShowSizes.Size = new System.Drawing.Size(279, 24);
      this.checkASMShowSizes.TabIndex = 23;
      this.checkASMShowSizes.Text = "Show Sizes";
      this.checkASMShowSizes.UseVisualStyleBackColor = true;
      this.checkASMShowSizes.CheckedChanged += new System.EventHandler(this.checkASMShowSizes_CheckedChanged);
      // 
      // checkASMShowCycles
      // 
      this.checkASMShowCycles.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkASMShowCycles.Location = new System.Drawing.Point(16, 193);
      this.checkASMShowCycles.Name = "checkASMShowCycles";
      this.checkASMShowCycles.Size = new System.Drawing.Size(279, 24);
      this.checkASMShowCycles.TabIndex = 22;
      this.checkASMShowCycles.Text = "Show Cycles";
      this.checkASMShowCycles.UseVisualStyleBackColor = true;
      this.checkASMShowCycles.CheckedChanged += new System.EventHandler(this.checkASMShowCycles_CheckedChanged);
      // 
      // checkASMShowLineNumbers
      // 
      this.checkASMShowLineNumbers.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkASMShowLineNumbers.Location = new System.Drawing.Point(16, 163);
      this.checkASMShowLineNumbers.Name = "checkASMShowLineNumbers";
      this.checkASMShowLineNumbers.Size = new System.Drawing.Size(279, 24);
      this.checkASMShowLineNumbers.TabIndex = 21;
      this.checkASMShowLineNumbers.Text = "Show Line Numbers";
      this.checkASMShowLineNumbers.UseVisualStyleBackColor = true;
      this.checkASMShowLineNumbers.CheckedChanged += new System.EventHandler(this.checkASMShowLineNumbers_CheckedChanged);
      // 
      // btnSetDefaultsFont
      // 
      this.btnSetDefaultsFont.Location = new System.Drawing.Point(171, 134);
      this.btnSetDefaultsFont.Name = "btnSetDefaultsFont";
      this.btnSetDefaultsFont.Size = new System.Drawing.Size(124, 23);
      this.btnSetDefaultsFont.TabIndex = 20;
      this.btnSetDefaultsFont.Text = "Set Default Fonts";
      this.btnSetDefaultsFont.UseVisualStyleBackColor = true;
      this.btnSetDefaultsFont.Click += new System.EventHandler(this.btnSetDefaultsFont_Click);
      // 
      // btnChooseFont
      // 
      this.btnChooseFont.Location = new System.Drawing.Point(171, 105);
      this.btnChooseFont.Name = "btnChooseFont";
      this.btnChooseFont.Size = new System.Drawing.Size(124, 23);
      this.btnChooseFont.TabIndex = 19;
      this.btnChooseFont.Text = "Change Font";
      this.btnChooseFont.UseVisualStyleBackColor = true;
      this.btnChooseFont.Click += new System.EventHandler(this.btnChooseFont_Click);
      // 
      // labelFontPreview
      // 
      this.labelFontPreview.Location = new System.Drawing.Point(318, 110);
      this.labelFontPreview.Name = "labelFontPreview";
      this.labelFontPreview.Size = new System.Drawing.Size(210, 44);
      this.labelFontPreview.TabIndex = 18;
      this.labelFontPreview.Text = "Font Preview";
      // 
      // editTabSize
      // 
      this.editTabSize.Location = new System.Drawing.Point(171, 49);
      this.editTabSize.MaxLength = 2;
      this.editTabSize.Name = "editTabSize";
      this.editTabSize.Size = new System.Drawing.Size(124, 20);
      this.editTabSize.TabIndex = 17;
      this.editTabSize.TextChanged += new System.EventHandler(this.editTabSize_TextChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(16, 110);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(31, 13);
      this.label1.TabIndex = 14;
      this.label1.Text = "Font:";
      // 
      // label14
      // 
      this.label14.AutoSize = true;
      this.label14.Location = new System.Drawing.Point(16, 52);
      this.label14.Name = "label14";
      this.label14.Size = new System.Drawing.Size(52, 13);
      this.label14.TabIndex = 14;
      this.label14.Text = "Tab Size:";
      // 
      // checkStripTrailingSpaces
      // 
      this.checkStripTrailingSpaces.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkStripTrailingSpaces.Location = new System.Drawing.Point(16, 75);
      this.checkStripTrailingSpaces.Name = "checkStripTrailingSpaces";
      this.checkStripTrailingSpaces.Size = new System.Drawing.Size(279, 24);
      this.checkStripTrailingSpaces.TabIndex = 16;
      this.checkStripTrailingSpaces.Text = "Strip Trailing Spaces/Tabs";
      this.checkStripTrailingSpaces.UseVisualStyleBackColor = true;
      this.checkStripTrailingSpaces.CheckedChanged += new System.EventHandler(this.checkStripTrailingSpaces_CheckedChanged);
      // 
      // checkConvertTabsToSpaces
      // 
      this.checkConvertTabsToSpaces.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkConvertTabsToSpaces.Location = new System.Drawing.Point(16, 22);
      this.checkConvertTabsToSpaces.Name = "checkConvertTabsToSpaces";
      this.checkConvertTabsToSpaces.Size = new System.Drawing.Size(279, 24);
      this.checkConvertTabsToSpaces.TabIndex = 15;
      this.checkConvertTabsToSpaces.Text = "Convert tabs to spaces";
      this.checkConvertTabsToSpaces.UseVisualStyleBackColor = true;
      this.checkConvertTabsToSpaces.CheckedChanged += new System.EventHandler(this.checkConvertTabsToSpaces_CheckedChanged);
      // 
      // comboASMEncoding
      // 
      this.comboASMEncoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboASMEncoding.FormattingEnabled = true;
      this.comboASMEncoding.Location = new System.Drawing.Point(171, 343);
      this.comboASMEncoding.Name = "comboASMEncoding";
      this.comboASMEncoding.Size = new System.Drawing.Size(555, 21);
      this.comboASMEncoding.TabIndex = 31;
      this.comboASMEncoding.SelectedIndexChanged += new System.EventHandler(this.comboASMEncoding_SelectedIndexChanged);
      // 
      // label35
      // 
      this.label35.AutoSize = true;
      this.label35.Location = new System.Drawing.Point(16, 346);
      this.label35.Name = "label35";
      this.label35.Size = new System.Drawing.Size(55, 13);
      this.label35.TabIndex = 32;
      this.label35.Text = "Encoding:";
      // 
      // PrefASMEditor
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBox1);
      this.Name = "PrefASMEditor";
      this.Size = new System.Drawing.Size(900, 407);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);

    }

        #endregion

        private System.Windows.Forms.Button btnExportSettings;
        private System.Windows.Forms.Button btnImportSettings;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox editTabSize;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox checkStripTrailingSpaces;
        private System.Windows.Forms.CheckBox checkConvertTabsToSpaces;
        private System.Windows.Forms.Button btnSetDefaultsFont;
        private System.Windows.Forms.Button btnChooseFont;
        private System.Windows.Forms.Label labelFontPreview;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkASMShowAddress;
        private System.Windows.Forms.CheckBox checkASMShowAutoComplete;
        private System.Windows.Forms.CheckBox checkASMShowMiniMap;
        private System.Windows.Forms.CheckBox checkASMShowSizes;
        private System.Windows.Forms.CheckBox checkASMShowCycles;
        private System.Windows.Forms.CheckBox checkASMShowLineNumbers;
        private System.Windows.Forms.ComboBox comboASMEncoding;
        private System.Windows.Forms.Label label35;
    }
}
