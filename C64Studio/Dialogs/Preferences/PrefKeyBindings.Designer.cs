namespace RetroDevStudio.Dialogs.Preferences
{
  partial class PrefKeyBindings
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
      this.btnSetDefaultsKeyBinding = new System.Windows.Forms.Button();
      this.btnUnbindKey = new System.Windows.Forms.Button();
      this.btnBindKeySecondary = new System.Windows.Forms.Button();
      this.btnBindKey = new System.Windows.Forms.Button();
      this.editKeyBinding = new System.Windows.Forms.TextBox();
      this.label10 = new System.Windows.Forms.Label();
      this.listFunctions = new System.Windows.Forms.ListView();
      this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnExportSettings
      // 
      this.btnExportSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnExportSettings.Location = new System.Drawing.Point(819, 455);
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
      this.btnImportSettings.Location = new System.Drawing.Point(738, 455);
      this.btnImportSettings.Name = "btnImportSettings";
      this.btnImportSettings.Size = new System.Drawing.Size(75, 23);
      this.btnImportSettings.TabIndex = 13;
      this.btnImportSettings.Text = "Import here";
      this.btnImportSettings.UseVisualStyleBackColor = true;
      this.btnImportSettings.Click += new System.EventHandler(this.btnImportSettings_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.btnSetDefaultsKeyBinding);
      this.groupBox1.Controls.Add(this.btnUnbindKey);
      this.groupBox1.Controls.Add(this.btnBindKeySecondary);
      this.groupBox1.Controls.Add(this.btnBindKey);
      this.groupBox1.Controls.Add(this.editKeyBinding);
      this.groupBox1.Controls.Add(this.label10);
      this.groupBox1.Controls.Add(this.listFunctions);
      this.groupBox1.Controls.Add(this.btnExportSettings);
      this.groupBox1.Controls.Add(this.btnImportSettings);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox1.Location = new System.Drawing.Point(0, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(900, 484);
      this.groupBox1.TabIndex = 18;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Hot Keys/Key Binding";
      // 
      // btnSetDefaultsKeyBinding
      // 
      this.btnSetDefaultsKeyBinding.Location = new System.Drawing.Point(557, 408);
      this.btnSetDefaultsKeyBinding.Name = "btnSetDefaultsKeyBinding";
      this.btnSetDefaultsKeyBinding.Size = new System.Drawing.Size(124, 23);
      this.btnSetDefaultsKeyBinding.TabIndex = 18;
      this.btnSetDefaultsKeyBinding.Text = "Set Defaults";
      this.btnSetDefaultsKeyBinding.UseVisualStyleBackColor = true;
      this.btnSetDefaultsKeyBinding.Click += new System.EventHandler(this.btnSetDefaultsKeyBinding_Click);
      // 
      // btnUnbindKey
      // 
      this.btnUnbindKey.Enabled = false;
      this.btnUnbindKey.Location = new System.Drawing.Point(464, 408);
      this.btnUnbindKey.Name = "btnUnbindKey";
      this.btnUnbindKey.Size = new System.Drawing.Size(75, 23);
      this.btnUnbindKey.TabIndex = 19;
      this.btnUnbindKey.Text = "Unbind Key";
      this.btnUnbindKey.UseVisualStyleBackColor = true;
      this.btnUnbindKey.Click += new System.EventHandler(this.btnUnbindKey_Click);
      // 
      // btnBindKeySecondary
      // 
      this.btnBindKeySecondary.Location = new System.Drawing.Point(369, 408);
      this.btnBindKeySecondary.Name = "btnBindKeySecondary";
      this.btnBindKeySecondary.Size = new System.Drawing.Size(75, 23);
      this.btnBindKeySecondary.TabIndex = 20;
      this.btnBindKeySecondary.Text = "Bind 2nd";
      this.btnBindKeySecondary.UseVisualStyleBackColor = true;
      this.btnBindKeySecondary.Click += new System.EventHandler(this.btnBindKeySecondary_Click);
      // 
      // btnBindKey
      // 
      this.btnBindKey.Location = new System.Drawing.Point(288, 408);
      this.btnBindKey.Name = "btnBindKey";
      this.btnBindKey.Size = new System.Drawing.Size(75, 23);
      this.btnBindKey.TabIndex = 21;
      this.btnBindKey.Text = "Bind Key";
      this.btnBindKey.UseVisualStyleBackColor = true;
      this.btnBindKey.Click += new System.EventHandler(this.btnBindKey_Click);
      // 
      // editKeyBinding
      // 
      this.editKeyBinding.Location = new System.Drawing.Point(78, 410);
      this.editKeyBinding.Name = "editKeyBinding";
      this.editKeyBinding.ReadOnly = true;
      this.editKeyBinding.Size = new System.Drawing.Size(204, 20);
      this.editKeyBinding.TabIndex = 17;
      this.editKeyBinding.Text = "Press Key here";
      this.editKeyBinding.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.editKeyBinding_PreviewKeyDown);
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(6, 413);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(66, 13);
      this.label10.TabIndex = 16;
      this.label10.Text = "Key Binding:";
      // 
      // listFunctions
      // 
      this.listFunctions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader8});
      this.listFunctions.FullRowSelect = true;
      this.listFunctions.HideSelection = false;
      this.listFunctions.Location = new System.Drawing.Point(9, 19);
      this.listFunctions.MultiSelect = false;
      this.listFunctions.Name = "listFunctions";
      this.listFunctions.Size = new System.Drawing.Size(773, 383);
      this.listFunctions.Sorting = System.Windows.Forms.SortOrder.Ascending;
      this.listFunctions.TabIndex = 15;
      this.listFunctions.UseCompatibleStateImageBehavior = false;
      this.listFunctions.View = System.Windows.Forms.View.Details;
      this.listFunctions.SelectedIndexChanged += new System.EventHandler(this.listFunctions_SelectedIndexChanged);
      // 
      // columnHeader4
      // 
      this.columnHeader4.Text = "State";
      this.columnHeader4.Width = 126;
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Function";
      this.columnHeader1.Width = 304;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Binding";
      this.columnHeader2.Width = 113;
      // 
      // columnHeader8
      // 
      this.columnHeader8.Text = "2nd Binding";
      this.columnHeader8.Width = 113;
      // 
      // PrefKeyBindings
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBox1);
      this.Name = "PrefKeyBindings";
      this.Size = new System.Drawing.Size(900, 484);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);

    }

        #endregion

        private System.Windows.Forms.Button btnExportSettings;
        private System.Windows.Forms.Button btnImportSettings;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSetDefaultsKeyBinding;
        private System.Windows.Forms.Button btnUnbindKey;
        private System.Windows.Forms.Button btnBindKeySecondary;
        private System.Windows.Forms.Button btnBindKey;
        private System.Windows.Forms.TextBox editKeyBinding;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ListView listFunctions;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader8;
    }
}
