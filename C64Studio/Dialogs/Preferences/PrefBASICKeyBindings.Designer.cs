namespace RetroDevStudio.Dialogs.Preferences
{
  partial class PrefBASICKeyBindings
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
      this.btnUnbindBASICKeyMapBinding = new DecentForms.Button();
      this.btnBindBASICKeyMapBinding = new DecentForms.Button();
      this.editBASICKeyMapBinding = new System.Windows.Forms.TextBox();
      this.label25 = new System.Windows.Forms.Label();
      this.listBASICKeyMap = new RetroDevStudio.Controls.MeasurableListView();
      this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnExportSettings
      // 
      this.btnExportSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnExportSettings.Location = new System.Drawing.Point(819, 371);
      this.btnExportSettings.Name = "btnExportSettings";
      this.btnExportSettings.Size = new System.Drawing.Size(75, 23);
      this.btnExportSettings.TabIndex = 12;
      this.btnExportSettings.Text = "Export here";
      this.btnExportSettings.Click += new DecentForms.EventHandler(this.btnExportSettings_Click);
      // 
      // btnImportSettings
      // 
      this.btnImportSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnImportSettings.Location = new System.Drawing.Point(738, 371);
      this.btnImportSettings.Name = "btnImportSettings";
      this.btnImportSettings.Size = new System.Drawing.Size(75, 23);
      this.btnImportSettings.TabIndex = 13;
      this.btnImportSettings.Text = "Import here";
      this.btnImportSettings.Click += new DecentForms.EventHandler(this.btnImportSettings_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.btnUnbindBASICKeyMapBinding);
      this.groupBox1.Controls.Add(this.btnBindBASICKeyMapBinding);
      this.groupBox1.Controls.Add(this.editBASICKeyMapBinding);
      this.groupBox1.Controls.Add(this.label25);
      this.groupBox1.Controls.Add(this.listBASICKeyMap);
      this.groupBox1.Controls.Add(this.btnExportSettings);
      this.groupBox1.Controls.Add(this.btnImportSettings);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox1.Location = new System.Drawing.Point(0, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(900, 400);
      this.groupBox1.TabIndex = 18;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "BASIC Key Map";
      // 
      // btnUnbindBASICKeyMapBinding
      // 
      this.btnUnbindBASICKeyMapBinding.Enabled = false;
      this.btnUnbindBASICKeyMapBinding.Location = new System.Drawing.Point(369, 371);
      this.btnUnbindBASICKeyMapBinding.Name = "btnUnbindBASICKeyMapBinding";
      this.btnUnbindBASICKeyMapBinding.Size = new System.Drawing.Size(72, 23);
      this.btnUnbindBASICKeyMapBinding.TabIndex = 17;
      this.btnUnbindBASICKeyMapBinding.Text = "Unbind Key";
      this.btnUnbindBASICKeyMapBinding.Click += new DecentForms.EventHandler(this.btnUnbindBASICKeyMapBinding_Click);
      // 
      // btnBindBASICKeyMapBinding
      // 
      this.btnBindBASICKeyMapBinding.Location = new System.Drawing.Point(288, 371);
      this.btnBindBASICKeyMapBinding.Name = "btnBindBASICKeyMapBinding";
      this.btnBindBASICKeyMapBinding.Size = new System.Drawing.Size(72, 23);
      this.btnBindBASICKeyMapBinding.TabIndex = 18;
      this.btnBindBASICKeyMapBinding.Text = "Bind Key";
      this.btnBindBASICKeyMapBinding.Click += new DecentForms.EventHandler(this.btnBindBASICKeyMapBinding_Click);
      // 
      // editBASICKeyMapBinding
      // 
      this.editBASICKeyMapBinding.Location = new System.Drawing.Point(78, 373);
      this.editBASICKeyMapBinding.Name = "editBASICKeyMapBinding";
      this.editBASICKeyMapBinding.ReadOnly = true;
      this.editBASICKeyMapBinding.Size = new System.Drawing.Size(201, 20);
      this.editBASICKeyMapBinding.TabIndex = 16;
      this.editBASICKeyMapBinding.Text = "Press Key here";
      this.editBASICKeyMapBinding.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.editBASICKeyMapBinding_PreviewKeyDown);
      // 
      // label25
      // 
      this.label25.AutoSize = true;
      this.label25.Location = new System.Drawing.Point(6, 376);
      this.label25.Name = "label25";
      this.label25.Size = new System.Drawing.Size(66, 13);
      this.label25.TabIndex = 15;
      this.label25.Text = "Key Binding:";
      // 
      // listBASICKeyMap
      // 
      this.listBASICKeyMap.AllowDrop = true;
      this.listBASICKeyMap.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader10});
      this.listBASICKeyMap.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.listBASICKeyMap.FullRowSelect = true;
      this.listBASICKeyMap.HideSelection = false;
      this.listBASICKeyMap.ItemHeight = 14;
      this.listBASICKeyMap.Location = new System.Drawing.Point(6, 19);
      this.listBASICKeyMap.MultiSelect = false;
      this.listBASICKeyMap.Name = "listBASICKeyMap";
      this.listBASICKeyMap.Size = new System.Drawing.Size(652, 348);
      this.listBASICKeyMap.TabIndex = 14;
      this.listBASICKeyMap.UseCompatibleStateImageBehavior = false;
      this.listBASICKeyMap.View = System.Windows.Forms.View.Details;
      this.listBASICKeyMap.SelectedIndexChanged += new System.EventHandler(this.listBASICKeyMap_SelectedIndexChanged);
      // 
      // columnHeader5
      // 
      this.columnHeader5.Text = "C64 Key";
      this.columnHeader5.Width = 145;
      // 
      // columnHeader6
      // 
      this.columnHeader6.Text = "PETSCII";
      this.columnHeader6.Width = 62;
      // 
      // columnHeader7
      // 
      this.columnHeader7.Text = "PC Key";
      this.columnHeader7.Width = 309;
      // 
      // columnHeader10
      // 
      this.columnHeader10.Text = "Display";
      this.columnHeader10.Width = 108;
      // 
      // PrefBASICKeyBindings
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBox1);
      this.Name = "PrefBASICKeyBindings";
      this.Size = new System.Drawing.Size(900, 400);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);

    }

        #endregion

        private DecentForms.Button btnExportSettings;
        private DecentForms.Button btnImportSettings;
        private System.Windows.Forms.GroupBox groupBox1;
        private DecentForms.Button btnUnbindBASICKeyMapBinding;
        private DecentForms.Button btnBindBASICKeyMapBinding;
        private System.Windows.Forms.TextBox editBASICKeyMapBinding;
        private System.Windows.Forms.Label label25;
        private Controls.MeasurableListView listBASICKeyMap;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader10;
    }
}
