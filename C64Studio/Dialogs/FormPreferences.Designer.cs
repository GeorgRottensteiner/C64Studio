namespace RetroDevStudio.Dialogs
{
  partial class FormPreferences
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
      this.label1 = new System.Windows.Forms.Label();
      this.editPreferencesFilter = new System.Windows.Forms.TextBox();
      this.panelPreferences = new System.Windows.Forms.FlowLayoutPanel();
      this.btnOK = new System.Windows.Forms.Button();
      this.btnExportAllSettings = new System.Windows.Forms.Button();
      this.btnImportAllSettings = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(32, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Filter:";
      // 
      // editPreferencesFilter
      // 
      this.editPreferencesFilter.Location = new System.Drawing.Point(50, 6);
      this.editPreferencesFilter.Name = "editPreferencesFilter";
      this.editPreferencesFilter.Size = new System.Drawing.Size(173, 20);
      this.editPreferencesFilter.TabIndex = 1;
      // 
      // panelPreferences
      // 
      this.panelPreferences.AutoScroll = true;
      this.panelPreferences.Location = new System.Drawing.Point(12, 32);
      this.panelPreferences.Name = "panelPreferences";
      this.panelPreferences.Size = new System.Drawing.Size(918, 483);
      this.panelPreferences.TabIndex = 2;
      // 
      // btnOK
      // 
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnOK.Location = new System.Drawing.Point(855, 529);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 3;
      this.btnOK.Text = "Close";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // btnExportAllSettings
      // 
      this.btnExportAllSettings.Location = new System.Drawing.Point(12, 529);
      this.btnExportAllSettings.Name = "btnExportAllSettings";
      this.btnExportAllSettings.Size = new System.Drawing.Size(75, 23);
      this.btnExportAllSettings.TabIndex = 10;
      this.btnExportAllSettings.Text = "Export all";
      this.btnExportAllSettings.UseVisualStyleBackColor = true;
      this.btnExportAllSettings.Click += new System.EventHandler(this.btnExportAllSettings_Click);
      // 
      // btnImportAllSettings
      // 
      this.btnImportAllSettings.Location = new System.Drawing.Point(93, 529);
      this.btnImportAllSettings.Name = "btnImportAllSettings";
      this.btnImportAllSettings.Size = new System.Drawing.Size(75, 23);
      this.btnImportAllSettings.TabIndex = 11;
      this.btnImportAllSettings.Text = "Import all";
      this.btnImportAllSettings.UseVisualStyleBackColor = true;
      this.btnImportAllSettings.Click += new System.EventHandler(this.btnImportAllSettings_Click);
      // 
      // FormPreferences
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(942, 564);
      this.Controls.Add(this.btnExportAllSettings);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnImportAllSettings);
      this.Controls.Add(this.panelPreferences);
      this.Controls.Add(this.editPreferencesFilter);
      this.Controls.Add(this.label1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormPreferences";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "FormPreferences";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox editPreferencesFilter;
        private System.Windows.Forms.FlowLayoutPanel panelPreferences;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnExportAllSettings;
        private System.Windows.Forms.Button btnImportAllSettings;
    }
}