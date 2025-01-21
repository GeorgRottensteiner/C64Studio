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
      this.treePreferences = new DecentForms.TreeView();
      this.btnImportHere = new DecentForms.Button();
      this.btnExportHere = new DecentForms.Button();
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
      this.panelPreferences.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.panelPreferences.Location = new System.Drawing.Point(155, 32);
      this.panelPreferences.Name = "panelPreferences";
      this.panelPreferences.Size = new System.Drawing.Size(775, 483);
      this.panelPreferences.TabIndex = 2;
      // 
      // btnOK
      // 
      this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnOK.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnOK.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.Image = null;
      this.btnOK.Location = new System.Drawing.Point(855, 529);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 3;
      this.btnOK.Text = "Close";
      this.btnOK.Click += new DecentForms.EventHandler(this.btnOK_Click);
      // 
      // btnExportAllSettings
      // 
      this.btnExportAllSettings.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnExportAllSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnExportAllSettings.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnExportAllSettings.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnExportAllSettings.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnExportAllSettings.Image = null;
      this.btnExportAllSettings.Location = new System.Drawing.Point(12, 529);
      this.btnExportAllSettings.Name = "btnExportAllSettings";
      this.btnExportAllSettings.Size = new System.Drawing.Size(75, 23);
      this.btnExportAllSettings.TabIndex = 10;
      this.btnExportAllSettings.Text = "Export all";
      this.btnExportAllSettings.Click += new DecentForms.EventHandler(this.btnExportAllSettings_Click);
      // 
      // btnImportAllSettings
      // 
      this.btnImportAllSettings.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnImportAllSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnImportAllSettings.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnImportAllSettings.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnImportAllSettings.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnImportAllSettings.Image = null;
      this.btnImportAllSettings.Location = new System.Drawing.Point(93, 529);
      this.btnImportAllSettings.Name = "btnImportAllSettings";
      this.btnImportAllSettings.Size = new System.Drawing.Size(75, 23);
      this.btnImportAllSettings.TabIndex = 11;
      this.btnImportAllSettings.Text = "Import all";
      this.btnImportAllSettings.Click += new DecentForms.EventHandler(this.btnImportAllSettings_Click);
      // 
      // treePreferences
      // 
      this.treePreferences.AllowDrag = false;
      this.treePreferences.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.treePreferences.ImageList = null;
      this.treePreferences.LabelEdit = false;
      this.treePreferences.Location = new System.Drawing.Point(8, 32);
      this.treePreferences.Name = "treePreferences";
      this.treePreferences.ScrollAlwaysVisible = false;
      this.treePreferences.SelectedNode = null;
      this.treePreferences.SelectionMode = DecentForms.SelectionMode.NONE;
      this.treePreferences.Size = new System.Drawing.Size(141, 483);
      this.treePreferences.TabIndex = 12;
      this.treePreferences.Text = "treeView1";
      this.treePreferences.AfterSelect += new DecentForms.TreeView.TreeViewEventHandler(this.treePreferences_AfterSelect);
      // 
      // btnImportHere
      // 
      this.btnImportHere.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnImportHere.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnImportHere.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnImportHere.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnImportHere.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnImportHere.Enabled = false;
      this.btnImportHere.Image = null;
      this.btnImportHere.Location = new System.Drawing.Point(341, 529);
      this.btnImportHere.Name = "btnImportHere";
      this.btnImportHere.Size = new System.Drawing.Size(75, 23);
      this.btnImportHere.TabIndex = 11;
      this.btnImportHere.Text = "Import here";
      this.btnImportHere.Click += new DecentForms.EventHandler(this.btnImportCurrentSettings_Click);
      // 
      // btnExportHere
      // 
      this.btnExportHere.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnExportHere.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnExportHere.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnExportHere.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnExportHere.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnExportHere.Enabled = false;
      this.btnExportHere.Image = null;
      this.btnExportHere.Location = new System.Drawing.Point(260, 529);
      this.btnExportHere.Name = "btnExportHere";
      this.btnExportHere.Size = new System.Drawing.Size(75, 23);
      this.btnExportHere.TabIndex = 10;
      this.btnExportHere.Text = "Export this";
      this.btnExportHere.Click += new DecentForms.EventHandler(this.btnExportCurrentSettings_Click);
      // 
      // FormPreferences
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(942, 564);
      this.Controls.Add(this.treePreferences);
      this.Controls.Add(this.btnExportHere);
      this.Controls.Add(this.btnExportAllSettings);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnImportHere);
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
    private DecentForms.TreeView treePreferences;
    private DecentForms.Button btnImportHere;
    private DecentForms.Button btnExportHere;
  }
}