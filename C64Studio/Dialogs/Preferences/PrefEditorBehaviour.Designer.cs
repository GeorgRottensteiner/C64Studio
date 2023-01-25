namespace RetroDevStudio.Dialogs.Preferences
{
  partial class PrefEditorBehaviour
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
      this.checkRightClickIsBGColor = new System.Windows.Forms.CheckBox();
      this.label1 = new System.Windows.Forms.Label();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnExportSettings
      // 
      this.btnExportSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnExportSettings.Location = new System.Drawing.Point(819, 66);
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
      this.btnImportSettings.Location = new System.Drawing.Point(738, 66);
      this.btnImportSettings.Name = "btnImportSettings";
      this.btnImportSettings.Size = new System.Drawing.Size(75, 23);
      this.btnImportSettings.TabIndex = 13;
      this.btnImportSettings.Text = "Import here";
      this.btnImportSettings.UseVisualStyleBackColor = true;
      this.btnImportSettings.Click += new System.EventHandler(this.btnImportSettings_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.checkRightClickIsBGColor);
      this.groupBox1.Controls.Add(this.btnExportSettings);
      this.groupBox1.Controls.Add(this.btnImportSettings);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox1.Location = new System.Drawing.Point(0, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(900, 95);
      this.groupBox1.TabIndex = 18;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Editor Behaviour";
      // 
      // checkRightClickIsBGColor
      // 
      this.checkRightClickIsBGColor.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkRightClickIsBGColor.Location = new System.Drawing.Point(17, 19);
      this.checkRightClickIsBGColor.Name = "checkRightClickIsBGColor";
      this.checkRightClickIsBGColor.Size = new System.Drawing.Size(307, 24);
      this.checkRightClickIsBGColor.TabIndex = 14;
      this.checkRightClickIsBGColor.Text = "Right Click is Paint with BG Color";
      this.checkRightClickIsBGColor.UseVisualStyleBackColor = true;
      this.checkRightClickIsBGColor.CheckedChanged += new System.EventHandler(this.checkRightClickIsBGColor_CheckedChanged);
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(370, 24);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(349, 40);
      this.label1.TabIndex = 15;
      this.label1.Text = "Toggles behaviour of right mouse button in drawing editors.  Per default right cl" +
    "ick picks the color under the cursor.";
      // 
      // PrefEditorBehaviour
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBox1);
      this.Name = "PrefEditorBehaviour";
      this.Size = new System.Drawing.Size(900, 95);
      this.groupBox1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

        #endregion

        private System.Windows.Forms.Button btnExportSettings;
        private System.Windows.Forms.Button btnImportSettings;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkRightClickIsBGColor;
        private System.Windows.Forms.Label label1;
    }
}
