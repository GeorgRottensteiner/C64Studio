namespace RetroDevStudio.Dialogs.Preferences
{
  partial class PrefApplication
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
      this.comboAppMode = new System.Windows.Forms.ComboBox();
      this.label29 = new System.Windows.Forms.Label();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.btnBrowseDefaultOpenSolutionPath = new System.Windows.Forms.Button();
      this.editDefaultOpenSolutionPath = new System.Windows.Forms.TextBox();
      this.editMaxMRUEntries = new System.Windows.Forms.TextBox();
      this.label32 = new System.Windows.Forms.Label();
      this.label31 = new System.Windows.Forms.Label();
      this.checkAutoOpenLastSolution = new System.Windows.Forms.CheckBox();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnExportSettings
      // 
      this.btnExportSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnExportSettings.Location = new System.Drawing.Point(819, 115);
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
      this.btnImportSettings.Location = new System.Drawing.Point(738, 115);
      this.btnImportSettings.Name = "btnImportSettings";
      this.btnImportSettings.Size = new System.Drawing.Size(75, 23);
      this.btnImportSettings.TabIndex = 13;
      this.btnImportSettings.Text = "Import here";
      this.btnImportSettings.UseVisualStyleBackColor = true;
      this.btnImportSettings.Click += new System.EventHandler(this.btnImportSettings_Click);
      // 
      // comboAppMode
      // 
      this.comboAppMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboAppMode.FormattingEnabled = true;
      this.comboAppMode.Items.AddRange(new object[] {
            "Undecided",
            "Normal (settings in UserAppData)",
            "Portable Mode (settings local)"});
      this.comboAppMode.Location = new System.Drawing.Point(225, 19);
      this.comboAppMode.Name = "comboAppMode";
      this.comboAppMode.Size = new System.Drawing.Size(140, 21);
      this.comboAppMode.TabIndex = 15;
      this.comboAppMode.SelectedIndexChanged += new System.EventHandler(this.comboAppMode_SelectedIndexChanged);
      // 
      // label29
      // 
      this.label29.AutoSize = true;
      this.label29.Location = new System.Drawing.Point(9, 22);
      this.label29.Name = "label29";
      this.label29.Size = new System.Drawing.Size(92, 13);
      this.label29.TabIndex = 14;
      this.label29.Text = "Application Mode:";
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.btnBrowseDefaultOpenSolutionPath);
      this.groupBox1.Controls.Add(this.editDefaultOpenSolutionPath);
      this.groupBox1.Controls.Add(this.editMaxMRUEntries);
      this.groupBox1.Controls.Add(this.label32);
      this.groupBox1.Controls.Add(this.label31);
      this.groupBox1.Controls.Add(this.checkAutoOpenLastSolution);
      this.groupBox1.Controls.Add(this.btnExportSettings);
      this.groupBox1.Controls.Add(this.comboAppMode);
      this.groupBox1.Controls.Add(this.label29);
      this.groupBox1.Controls.Add(this.btnImportSettings);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox1.Location = new System.Drawing.Point(0, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(900, 144);
      this.groupBox1.TabIndex = 16;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Environment";
      // 
      // btnBrowseDefaultOpenSolutionPath
      // 
      this.btnBrowseDefaultOpenSolutionPath.Location = new System.Drawing.Point(371, 99);
      this.btnBrowseDefaultOpenSolutionPath.Name = "btnBrowseDefaultOpenSolutionPath";
      this.btnBrowseDefaultOpenSolutionPath.Size = new System.Drawing.Size(23, 20);
      this.btnBrowseDefaultOpenSolutionPath.TabIndex = 18;
      this.btnBrowseDefaultOpenSolutionPath.Text = "...";
      this.btnBrowseDefaultOpenSolutionPath.UseVisualStyleBackColor = true;
      this.btnBrowseDefaultOpenSolutionPath.Click += new System.EventHandler(this.btnBrowseDefaultOpenSolutionPath_Click);
      // 
      // editDefaultOpenSolutionPath
      // 
      this.editDefaultOpenSolutionPath.Location = new System.Drawing.Point(225, 99);
      this.editDefaultOpenSolutionPath.Name = "editDefaultOpenSolutionPath";
      this.editDefaultOpenSolutionPath.Size = new System.Drawing.Size(140, 20);
      this.editDefaultOpenSolutionPath.TabIndex = 17;
      this.editDefaultOpenSolutionPath.TextChanged += new System.EventHandler(this.editDefaultOpenSolutionPath_TextChanged);
      // 
      // editMaxMRUEntries
      // 
      this.editMaxMRUEntries.Location = new System.Drawing.Point(225, 46);
      this.editMaxMRUEntries.MaxLength = 2;
      this.editMaxMRUEntries.Name = "editMaxMRUEntries";
      this.editMaxMRUEntries.Size = new System.Drawing.Size(140, 20);
      this.editMaxMRUEntries.TabIndex = 21;
      this.editMaxMRUEntries.TextChanged += new System.EventHandler(this.editMaxMRUEntries_TextChanged);
      // 
      // label32
      // 
      this.label32.AutoSize = true;
      this.label32.Location = new System.Drawing.Point(9, 49);
      this.label32.Name = "label32";
      this.label32.Size = new System.Drawing.Size(95, 13);
      this.label32.TabIndex = 19;
      this.label32.Text = "Max. MRU entries:";
      // 
      // label31
      // 
      this.label31.AutoSize = true;
      this.label31.Location = new System.Drawing.Point(9, 102);
      this.label31.Name = "label31";
      this.label31.Size = new System.Drawing.Size(139, 13);
      this.label31.TabIndex = 20;
      this.label31.Text = "Default Solution Open Path:";
      // 
      // checkAutoOpenLastSolution
      // 
      this.checkAutoOpenLastSolution.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkAutoOpenLastSolution.Location = new System.Drawing.Point(9, 72);
      this.checkAutoOpenLastSolution.Name = "checkAutoOpenLastSolution";
      this.checkAutoOpenLastSolution.Size = new System.Drawing.Size(356, 24);
      this.checkAutoOpenLastSolution.TabIndex = 16;
      this.checkAutoOpenLastSolution.Text = "Open last solution on startup";
      this.checkAutoOpenLastSolution.UseVisualStyleBackColor = true;
      this.checkAutoOpenLastSolution.CheckedChanged += new System.EventHandler(this.checkAutoOpenLastSolution_CheckedChanged);
      // 
      // PrefApplication
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBox1);
      this.Name = "PrefApplication";
      this.Size = new System.Drawing.Size(900, 144);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);

    }

        #endregion

        private System.Windows.Forms.Button btnExportSettings;
        private System.Windows.Forms.Button btnImportSettings;
        private System.Windows.Forms.ComboBox comboAppMode;
        private System.Windows.Forms.Label label29;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Button btnBrowseDefaultOpenSolutionPath;
    private System.Windows.Forms.TextBox editDefaultOpenSolutionPath;
    private System.Windows.Forms.TextBox editMaxMRUEntries;
    private System.Windows.Forms.Label label32;
    private System.Windows.Forms.Label label31;
    private System.Windows.Forms.CheckBox checkAutoOpenLastSolution;
  }
}
