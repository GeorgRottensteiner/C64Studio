namespace RetroDevStudio.Dialogs.Preferences
{
  partial class PrefColors
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
      this.btnSetDefaultsColors = new System.Windows.Forms.Button();
      this.btnChooseBG = new System.Windows.Forms.Button();
      this.btnChooseFG = new System.Windows.Forms.Button();
      this.panelElementPreview = new System.Windows.Forms.Panel();
      this.comboElementBG = new System.Windows.Forms.ComboBox();
      this.comboElementFG = new System.Windows.Forms.ComboBox();
      this.label27 = new System.Windows.Forms.Label();
      this.label19 = new System.Windows.Forms.Label();
      this.label18 = new System.Windows.Forms.Label();
      this.label17 = new System.Windows.Forms.Label();
      this.listColoring = new System.Windows.Forms.ListView();
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.label16 = new System.Windows.Forms.Label();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnExportSettings
      // 
      this.btnExportSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnExportSettings.Location = new System.Drawing.Point(819, 422);
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
      this.btnImportSettings.Location = new System.Drawing.Point(738, 422);
      this.btnImportSettings.Name = "btnImportSettings";
      this.btnImportSettings.Size = new System.Drawing.Size(75, 23);
      this.btnImportSettings.TabIndex = 13;
      this.btnImportSettings.Text = "Import here";
      this.btnImportSettings.UseVisualStyleBackColor = true;
      this.btnImportSettings.Click += new System.EventHandler(this.btnImportSettings_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.btnSetDefaultsColors);
      this.groupBox1.Controls.Add(this.btnChooseBG);
      this.groupBox1.Controls.Add(this.btnChooseFG);
      this.groupBox1.Controls.Add(this.panelElementPreview);
      this.groupBox1.Controls.Add(this.comboElementBG);
      this.groupBox1.Controls.Add(this.comboElementFG);
      this.groupBox1.Controls.Add(this.label27);
      this.groupBox1.Controls.Add(this.label19);
      this.groupBox1.Controls.Add(this.label18);
      this.groupBox1.Controls.Add(this.label17);
      this.groupBox1.Controls.Add(this.listColoring);
      this.groupBox1.Controls.Add(this.label16);
      this.groupBox1.Controls.Add(this.btnExportSettings);
      this.groupBox1.Controls.Add(this.btnImportSettings);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox1.Location = new System.Drawing.Point(0, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(900, 451);
      this.groupBox1.TabIndex = 18;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Colors/Theme";
      // 
      // btnSetDefaultsColors
      // 
      this.btnSetDefaultsColors.Location = new System.Drawing.Point(608, 422);
      this.btnSetDefaultsColors.Name = "btnSetDefaultsColors";
      this.btnSetDefaultsColors.Size = new System.Drawing.Size(124, 23);
      this.btnSetDefaultsColors.TabIndex = 25;
      this.btnSetDefaultsColors.Text = "Set Defaults";
      this.btnSetDefaultsColors.UseVisualStyleBackColor = true;
      this.btnSetDefaultsColors.Click += new System.EventHandler(this.btnSetDefaultsColors_Click);
      // 
      // btnChooseBG
      // 
      this.btnChooseBG.Location = new System.Drawing.Point(518, 76);
      this.btnChooseBG.Name = "btnChooseBG";
      this.btnChooseBG.Size = new System.Drawing.Size(75, 23);
      this.btnChooseBG.TabIndex = 23;
      this.btnChooseBG.Text = "Choose...";
      this.btnChooseBG.UseVisualStyleBackColor = true;
      this.btnChooseBG.Click += new System.EventHandler(this.btnChooseBG_Click);
      // 
      // btnChooseFG
      // 
      this.btnChooseFG.Location = new System.Drawing.Point(518, 30);
      this.btnChooseFG.Name = "btnChooseFG";
      this.btnChooseFG.Size = new System.Drawing.Size(75, 23);
      this.btnChooseFG.TabIndex = 24;
      this.btnChooseFG.Text = "Choose...";
      this.btnChooseFG.UseVisualStyleBackColor = true;
      this.btnChooseFG.Click += new System.EventHandler(this.btnChooseFG_Click);
      // 
      // panelElementPreview
      // 
      this.panelElementPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panelElementPreview.Location = new System.Drawing.Point(269, 159);
      this.panelElementPreview.Name = "panelElementPreview";
      this.panelElementPreview.Size = new System.Drawing.Size(463, 190);
      this.panelElementPreview.TabIndex = 22;
      this.panelElementPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.panelElementPreview_Paint);
      // 
      // comboElementBG
      // 
      this.comboElementBG.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboElementBG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboElementBG.FormattingEnabled = true;
      this.comboElementBG.Location = new System.Drawing.Point(269, 78);
      this.comboElementBG.Name = "comboElementBG";
      this.comboElementBG.Size = new System.Drawing.Size(243, 21);
      this.comboElementBG.TabIndex = 20;
      this.comboElementBG.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboElementBG_DrawItem);
      this.comboElementBG.SelectedIndexChanged += new System.EventHandler(this.comboElementBG_SelectedIndexChanged);
      // 
      // comboElementFG
      // 
      this.comboElementFG.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboElementFG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboElementFG.FormattingEnabled = true;
      this.comboElementFG.Location = new System.Drawing.Point(269, 32);
      this.comboElementFG.Name = "comboElementFG";
      this.comboElementFG.Size = new System.Drawing.Size(243, 21);
      this.comboElementFG.TabIndex = 21;
      this.comboElementFG.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboElementFG_DrawItem);
      this.comboElementFG.SelectedIndexChanged += new System.EventHandler(this.comboElementFG_SelectedIndexChanged);
      // 
      // label27
      // 
      this.label27.AutoSize = true;
      this.label27.Location = new System.Drawing.Point(266, 102);
      this.label27.Name = "label27";
      this.label27.Size = new System.Drawing.Size(281, 13);
      this.label27.TabIndex = 16;
      this.label27.Text = "\"Auto\" takes the corresponding color from \"Empty Space\"";
      // 
      // label19
      // 
      this.label19.AutoSize = true;
      this.label19.Location = new System.Drawing.Point(266, 143);
      this.label19.Name = "label19";
      this.label19.Size = new System.Drawing.Size(48, 13);
      this.label19.TabIndex = 17;
      this.label19.Text = "Preview:";
      // 
      // label18
      // 
      this.label18.AutoSize = true;
      this.label18.Location = new System.Drawing.Point(266, 62);
      this.label18.Name = "label18";
      this.label18.Size = new System.Drawing.Size(109, 13);
      this.label18.TabIndex = 18;
      this.label18.Text = "Element Background:";
      // 
      // label17
      // 
      this.label17.AutoSize = true;
      this.label17.Location = new System.Drawing.Point(266, 16);
      this.label17.Name = "label17";
      this.label17.Size = new System.Drawing.Size(105, 13);
      this.label17.TabIndex = 19;
      this.label17.Text = "Element Foreground:";
      // 
      // listColoring
      // 
      this.listColoring.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3});
      this.listColoring.FullRowSelect = true;
      this.listColoring.HideSelection = false;
      this.listColoring.Location = new System.Drawing.Point(8, 32);
      this.listColoring.Name = "listColoring";
      this.listColoring.Size = new System.Drawing.Size(252, 409);
      this.listColoring.TabIndex = 15;
      this.listColoring.UseCompatibleStateImageBehavior = false;
      this.listColoring.View = System.Windows.Forms.View.Details;
      this.listColoring.SelectedIndexChanged += new System.EventHandler(this.listColoring_SelectedIndexChanged);
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "Item";
      this.columnHeader3.Width = 240;
      // 
      // label16
      // 
      this.label16.AutoSize = true;
      this.label16.Location = new System.Drawing.Point(5, 16);
      this.label16.Name = "label16";
      this.label16.Size = new System.Drawing.Size(39, 13);
      this.label16.TabIndex = 14;
      this.label16.Text = "Colors:";
      // 
      // PrefColors
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBox1);
      this.Name = "PrefColors";
      this.Size = new System.Drawing.Size(900, 451);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);

    }

        #endregion

        private System.Windows.Forms.Button btnExportSettings;
        private System.Windows.Forms.Button btnImportSettings;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSetDefaultsColors;
        private System.Windows.Forms.Button btnChooseBG;
        private System.Windows.Forms.Button btnChooseFG;
        private System.Windows.Forms.Panel panelElementPreview;
        private System.Windows.Forms.ComboBox comboElementBG;
        private System.Windows.Forms.ComboBox comboElementFG;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ListView listColoring;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Label label16;
    }
}
