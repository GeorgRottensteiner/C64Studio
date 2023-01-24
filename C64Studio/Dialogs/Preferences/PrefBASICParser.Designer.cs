namespace RetroDevStudio.Dialogs.Preferences
{
  partial class PrefBASICParser
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
      this.checkBASICAutoToggleEntryMode = new System.Windows.Forms.CheckBox();
      this.checkBASICShowControlCodes = new System.Windows.Forms.CheckBox();
      this.checkBASICStripREM = new System.Windows.Forms.CheckBox();
      this.checkBASICStripSpaces = new System.Windows.Forms.CheckBox();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnExportSettings
      // 
      this.btnExportSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnExportSettings.Location = new System.Drawing.Point(819, 178);
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
      this.btnImportSettings.Location = new System.Drawing.Point(738, 178);
      this.btnImportSettings.Name = "btnImportSettings";
      this.btnImportSettings.Size = new System.Drawing.Size(75, 23);
      this.btnImportSettings.TabIndex = 13;
      this.btnImportSettings.Text = "Import here";
      this.btnImportSettings.UseVisualStyleBackColor = true;
      this.btnImportSettings.Click += new System.EventHandler(this.btnImportSettings_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.checkBASICAutoToggleEntryMode);
      this.groupBox1.Controls.Add(this.checkBASICShowControlCodes);
      this.groupBox1.Controls.Add(this.checkBASICStripREM);
      this.groupBox1.Controls.Add(this.checkBASICStripSpaces);
      this.groupBox1.Controls.Add(this.btnExportSettings);
      this.groupBox1.Controls.Add(this.btnImportSettings);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox1.Location = new System.Drawing.Point(0, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(900, 207);
      this.groupBox1.TabIndex = 18;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "BASIC Compiler";
      // 
      // checkBASICAutoToggleEntryMode
      // 
      this.checkBASICAutoToggleEntryMode.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkBASICAutoToggleEntryMode.Location = new System.Drawing.Point(6, 109);
      this.checkBASICAutoToggleEntryMode.Name = "checkBASICAutoToggleEntryMode";
      this.checkBASICAutoToggleEntryMode.Size = new System.Drawing.Size(266, 24);
      this.checkBASICAutoToggleEntryMode.TabIndex = 14;
      this.checkBASICAutoToggleEntryMode.Text = "Auto toggle entry mode on quotes (\")";
      this.checkBASICAutoToggleEntryMode.UseVisualStyleBackColor = true;
      this.checkBASICAutoToggleEntryMode.CheckedChanged += new System.EventHandler(this.checkBASICAutoToggleEntryMode_CheckedChanged);
      // 
      // checkBASICShowControlCodes
      // 
      this.checkBASICShowControlCodes.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkBASICShowControlCodes.Location = new System.Drawing.Point(6, 79);
      this.checkBASICShowControlCodes.Name = "checkBASICShowControlCodes";
      this.checkBASICShowControlCodes.Size = new System.Drawing.Size(266, 24);
      this.checkBASICShowControlCodes.TabIndex = 15;
      this.checkBASICShowControlCodes.Text = "Show control codes as characters";
      this.checkBASICShowControlCodes.UseVisualStyleBackColor = true;
      this.checkBASICShowControlCodes.CheckedChanged += new System.EventHandler(this.checkBASICShowControlCodes_CheckedChanged);
      // 
      // checkBASICStripREM
      // 
      this.checkBASICStripREM.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkBASICStripREM.Location = new System.Drawing.Point(6, 49);
      this.checkBASICStripREM.Name = "checkBASICStripREM";
      this.checkBASICStripREM.Size = new System.Drawing.Size(266, 24);
      this.checkBASICStripREM.TabIndex = 16;
      this.checkBASICStripREM.Text = "Strip REM statements";
      this.checkBASICStripREM.UseVisualStyleBackColor = true;
      this.checkBASICStripREM.CheckedChanged += new System.EventHandler(this.checkBASICStripREM_CheckedChanged);
      // 
      // checkBASICStripSpaces
      // 
      this.checkBASICStripSpaces.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkBASICStripSpaces.Location = new System.Drawing.Point(6, 19);
      this.checkBASICStripSpaces.Name = "checkBASICStripSpaces";
      this.checkBASICStripSpaces.Size = new System.Drawing.Size(266, 24);
      this.checkBASICStripSpaces.TabIndex = 17;
      this.checkBASICStripSpaces.Text = "Strip spaces between code";
      this.checkBASICStripSpaces.UseVisualStyleBackColor = true;
      this.checkBASICStripSpaces.CheckedChanged += new System.EventHandler(this.checkBASICStripSpaces_CheckedChanged);
      // 
      // PrefBASICParser
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBox1);
      this.Name = "PrefBASICParser";
      this.Size = new System.Drawing.Size(900, 207);
      this.groupBox1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

        #endregion

        private System.Windows.Forms.Button btnExportSettings;
        private System.Windows.Forms.Button btnImportSettings;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBASICAutoToggleEntryMode;
        private System.Windows.Forms.CheckBox checkBASICShowControlCodes;
        private System.Windows.Forms.CheckBox checkBASICStripREM;
        private System.Windows.Forms.CheckBox checkBASICStripSpaces;
    }
}
