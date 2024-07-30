using System.Windows.Forms;

namespace RetroDevStudio.Dialogs.Preferences
{
  partial class PrefPalettes
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrefPalettes));
      this.btnExportSettings = new DecentForms.Button();
      this.btnImportSettings = new DecentForms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.paletteEditor = new RetroDevStudio.Controls.PaletteEditor();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnExportSettings
      // 
      this.btnExportSettings.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnExportSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnExportSettings.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnExportSettings.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnExportSettings.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnExportSettings.Image = null;
      this.btnExportSettings.Location = new System.Drawing.Point(819, 449);
      this.btnExportSettings.Name = "btnExportSettings";
      this.btnExportSettings.Size = new System.Drawing.Size(75, 23);
      this.btnExportSettings.TabIndex = 12;
      this.btnExportSettings.Text = "Export here";
      this.btnExportSettings.Click += new DecentForms.EventHandler(this.btnExportSettings_Click);
      // 
      // btnImportSettings
      // 
      this.btnImportSettings.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnImportSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnImportSettings.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnImportSettings.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnImportSettings.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnImportSettings.Image = null;
      this.btnImportSettings.Location = new System.Drawing.Point(738, 449);
      this.btnImportSettings.Name = "btnImportSettings";
      this.btnImportSettings.Size = new System.Drawing.Size(75, 23);
      this.btnImportSettings.TabIndex = 13;
      this.btnImportSettings.Text = "Import here";
      this.btnImportSettings.Click += new DecentForms.EventHandler(this.btnImportSettings_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.paletteEditor);
      this.groupBox1.Controls.Add(this.btnExportSettings);
      this.groupBox1.Controls.Add(this.btnImportSettings);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox1.Location = new System.Drawing.Point(0, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(900, 478);
      this.groupBox1.TabIndex = 18;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Palettes";
      // 
      // paletteEditor
      // 
      this.paletteEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.paletteEditor.Location = new System.Drawing.Point(6, 19);
      this.paletteEditor.Name = "paletteEditor";
      this.paletteEditor.Palettes = ((System.Collections.Generic.Dictionary<RetroDevStudio.PaletteType, System.Collections.Generic.List<RetroDevStudio.Palette>>)(resources.GetObject("paletteEditor.Palettes")));
      this.paletteEditor.Size = new System.Drawing.Size(888, 419);
      this.paletteEditor.TabIndex = 14;
      this.paletteEditor.PaletteOrderModified += new RetroDevStudio.Controls.PaletteEditor.PaletteOrderModifiedHandler(this.paletteEditor_PaletteOrderModified);
      // 
      // PrefPalettes
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBox1);
      this.Name = "PrefPalettes";
      this.Size = new System.Drawing.Size(900, 478);
      this.groupBox1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

        #endregion

        private DecentForms.Button btnExportSettings;
        private DecentForms.Button btnImportSettings;
        private System.Windows.Forms.GroupBox groupBox1;
        private Controls.PaletteEditor paletteEditor;
  }
}
