namespace RetroDevStudio.Dialogs.Preferences
{
  partial class PrefBASICEditor
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
      this.btnExportSettings = new DecentForms.Button();
      this.btnImportSettings = new DecentForms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.checkBASICEditorShowMaxLineLengthIndicator = new System.Windows.Forms.CheckBox();
      this.label1 = new System.Windows.Forms.Label();
      this.editMaxLineLengthIndicatorColumn = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.editBASICC64FontSize = new System.Windows.Forms.TextBox();
      this.labelBASICC64FontSize = new System.Windows.Forms.Label();
      this.checkBASICUseC64Font = new System.Windows.Forms.CheckBox();
      this.btnChangeBASICFont = new DecentForms.Button();
      this.labelBASICFontPreview = new System.Windows.Forms.Label();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnExportSettings
      // 
      this.btnExportSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnExportSettings.Location = new System.Drawing.Point(819, 68);
      this.btnExportSettings.Name = "btnExportSettings";
      this.btnExportSettings.Size = new System.Drawing.Size(75, 23);
      this.btnExportSettings.TabIndex = 12;
      this.btnExportSettings.Text = "Export here";
      this.btnExportSettings.Click += new DecentForms.EventHandler(this.btnExportSettings_Click);
      // 
      // btnImportSettings
      // 
      this.btnImportSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnImportSettings.Location = new System.Drawing.Point(738, 68);
      this.btnImportSettings.Name = "btnImportSettings";
      this.btnImportSettings.Size = new System.Drawing.Size(75, 23);
      this.btnImportSettings.TabIndex = 13;
      this.btnImportSettings.Text = "Import here";
      this.btnImportSettings.Click += new DecentForms.EventHandler(this.btnImportSettings_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.checkBASICEditorShowMaxLineLengthIndicator);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.editMaxLineLengthIndicatorColumn);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Controls.Add(this.editBASICC64FontSize);
      this.groupBox1.Controls.Add(this.labelBASICC64FontSize);
      this.groupBox1.Controls.Add(this.checkBASICUseC64Font);
      this.groupBox1.Controls.Add(this.btnChangeBASICFont);
      this.groupBox1.Controls.Add(this.labelBASICFontPreview);
      this.groupBox1.Controls.Add(this.btnExportSettings);
      this.groupBox1.Controls.Add(this.btnImportSettings);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox1.Location = new System.Drawing.Point(0, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(900, 97);
      this.groupBox1.TabIndex = 18;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "BASIC Editor";
      // 
      // checkBASICEditorShowMaxLineLengthIndicator
      // 
      this.checkBASICEditorShowMaxLineLengthIndicator.AutoSize = true;
      this.checkBASICEditorShowMaxLineLengthIndicator.Location = new System.Drawing.Point(22, 73);
      this.checkBASICEditorShowMaxLineLengthIndicator.Name = "checkBASICEditorShowMaxLineLengthIndicator";
      this.checkBASICEditorShowMaxLineLengthIndicator.Size = new System.Drawing.Size(169, 17);
      this.checkBASICEditorShowMaxLineLengthIndicator.TabIndex = 24;
      this.checkBASICEditorShowMaxLineLengthIndicator.Text = "Show max line length indicator";
      this.checkBASICEditorShowMaxLineLengthIndicator.UseVisualStyleBackColor = true;
      this.checkBASICEditorShowMaxLineLengthIndicator.CheckedChanged += new System.EventHandler(this.checkBASICEditorShowMaxLineLengthIndicator_CheckedChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(19, 25);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(31, 13);
      this.label1.TabIndex = 23;
      this.label1.Text = "Font:";
      // 
      // editMaxLineLengthIndicatorColumn
      // 
      this.editMaxLineLengthIndicatorColumn.Enabled = false;
      this.editMaxLineLengthIndicatorColumn.Location = new System.Drawing.Point(258, 71);
      this.editMaxLineLengthIndicatorColumn.MaxLength = 3;
      this.editMaxLineLengthIndicatorColumn.Name = "editMaxLineLengthIndicatorColumn";
      this.editMaxLineLengthIndicatorColumn.Size = new System.Drawing.Size(91, 20);
      this.editMaxLineLengthIndicatorColumn.TabIndex = 22;
      this.editMaxLineLengthIndicatorColumn.TextChanged += new System.EventHandler(this.editMaxLineLengthIndicatorColumn_TextChanged);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Enabled = false;
      this.label2.Location = new System.Drawing.Point(222, 74);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(25, 13);
      this.label2.TabIndex = 21;
      this.label2.Text = "Col:";
      // 
      // editBASICC64FontSize
      // 
      this.editBASICC64FontSize.Enabled = false;
      this.editBASICC64FontSize.Location = new System.Drawing.Point(258, 48);
      this.editBASICC64FontSize.MaxLength = 3;
      this.editBASICC64FontSize.Name = "editBASICC64FontSize";
      this.editBASICC64FontSize.Size = new System.Drawing.Size(91, 20);
      this.editBASICC64FontSize.TabIndex = 22;
      this.editBASICC64FontSize.TextChanged += new System.EventHandler(this.editBASICC64FontSize_TextChanged);
      // 
      // labelBASICC64FontSize
      // 
      this.labelBASICC64FontSize.AutoSize = true;
      this.labelBASICC64FontSize.Enabled = false;
      this.labelBASICC64FontSize.Location = new System.Drawing.Point(222, 51);
      this.labelBASICC64FontSize.Name = "labelBASICC64FontSize";
      this.labelBASICC64FontSize.Size = new System.Drawing.Size(30, 13);
      this.labelBASICC64FontSize.TabIndex = 21;
      this.labelBASICC64FontSize.Text = "Size:";
      // 
      // checkBASICUseC64Font
      // 
      this.checkBASICUseC64Font.AutoSize = true;
      this.checkBASICUseC64Font.Location = new System.Drawing.Point(22, 50);
      this.checkBASICUseC64Font.Name = "checkBASICUseC64Font";
      this.checkBASICUseC64Font.Size = new System.Drawing.Size(88, 17);
      this.checkBASICUseC64Font.TabIndex = 20;
      this.checkBASICUseC64Font.Text = "Use C64 font";
      this.checkBASICUseC64Font.UseVisualStyleBackColor = true;
      this.checkBASICUseC64Font.CheckedChanged += new System.EventHandler(this.checkBASICUseC64Font_CheckedChanged);
      // 
      // btnChangeBASICFont
      // 
      this.btnChangeBASICFont.Enabled = false;
      this.btnChangeBASICFont.Location = new System.Drawing.Point(225, 19);
      this.btnChangeBASICFont.Name = "btnChangeBASICFont";
      this.btnChangeBASICFont.Size = new System.Drawing.Size(124, 23);
      this.btnChangeBASICFont.TabIndex = 19;
      this.btnChangeBASICFont.Text = "Change Font";
      this.btnChangeBASICFont.Click += new DecentForms.EventHandler(this.btnChangeBASICFont_Click);
      // 
      // labelBASICFontPreview
      // 
      this.labelBASICFontPreview.Location = new System.Drawing.Point(355, 25);
      this.labelBASICFontPreview.Name = "labelBASICFontPreview";
      this.labelBASICFontPreview.Size = new System.Drawing.Size(209, 35);
      this.labelBASICFontPreview.TabIndex = 18;
      this.labelBASICFontPreview.Text = "BASIC Font Preview";
      // 
      // PrefBASICEditor
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBox1);
      this.Name = "PrefBASICEditor";
      this.Size = new System.Drawing.Size(900, 97);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);

    }

        #endregion

        private DecentForms.Button btnExportSettings;
        private DecentForms.Button btnImportSettings;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox editBASICC64FontSize;
        private System.Windows.Forms.Label labelBASICC64FontSize;
        private System.Windows.Forms.CheckBox checkBASICUseC64Font;
        private DecentForms.Button btnChangeBASICFont;
        private System.Windows.Forms.Label labelBASICFontPreview;
        private System.Windows.Forms.Label label1;
    private System.Windows.Forms.CheckBox checkBASICEditorShowMaxLineLengthIndicator;
    private System.Windows.Forms.TextBox editMaxLineLengthIndicatorColumn;
    private System.Windows.Forms.Label label2;
  }
}
