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
      this.btnOK = new DecentForms.Button();
      this.btnExportAllSettings = new DecentForms.Button();
      this.btnImportAllSettings = new DecentForms.Button();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(71, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Filter settings:";
      // 
      // editPreferencesFilter
      // 
      this.editPreferencesFilter.Location = new System.Drawing.Point(93, 6);
      this.editPreferencesFilter.Name = "editPreferencesFilter";
      this.editPreferencesFilter.Size = new System.Drawing.Size(187, 20);
      this.editPreferencesFilter.TabIndex = 1;
      this.editPreferencesFilter.TextChanged += new System.EventHandler(this.editPreferencesFilter_TextChanged);
      // 
      // panelPreferences
      // 
      this.panelPreferences.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.panelPreferences.AutoScroll = true;
      this.panelPreferences.Location = new System.Drawing.Point(12, 32);
      this.panelPreferences.Name = "panelPreferences";
      this.panelPreferences.Size = new System.Drawing.Size(918, 483);
      this.panelPreferences.TabIndex = 2;
      // 
      // btnOK
      // 
      this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnOK.Location = new System.Drawing.Point(855, 529);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 3;
      this.btnOK.Text = "Close";
      this.btnOK.Click += new DecentForms.EventHandler(this.btnOK_Click);
      // 
      // btnExportAllSettings
      // 
      this.btnExportAllSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnExportAllSettings.Location = new System.Drawing.Point(12, 529);
      this.btnExportAllSettings.Name = "btnExportAllSettings";
      this.btnExportAllSettings.Size = new System.Drawing.Size(75, 23);
      this.btnExportAllSettings.TabIndex = 10;
      this.btnExportAllSettings.Text = "Export all";
      this.btnExportAllSettings.Click += new DecentForms.EventHandler(this.btnExportAllSettings_Click);
      // 
      // btnImportAllSettings
      // 
      this.btnImportAllSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnImportAllSettings.Location = new System.Drawing.Point(93, 529);
      this.btnImportAllSettings.Name = "btnImportAllSettings";
      this.btnImportAllSettings.Size = new System.Drawing.Size(75, 23);
      this.btnImportAllSettings.TabIndex = 11;
      this.btnImportAllSettings.Text = "Import all";
      this.btnImportAllSettings.Click += new DecentForms.EventHandler(this.btnImportAllSettings_Click);
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
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormPreferences";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Preferences";
      this.Load += new System.EventHandler(this.FormPreferences_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox editPreferencesFilter;
        private System.Windows.Forms.FlowLayoutPanel panelPreferences;
        private DecentForms.Button btnOK;
        private DecentForms.Button btnExportAllSettings;
        private DecentForms.Button btnImportAllSettings;
    }
}