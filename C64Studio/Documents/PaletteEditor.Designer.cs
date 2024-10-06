namespace RetroDevStudio.Documents
{
  partial class PaletteEditor
  {
    /// <summary>
    /// Erforderliche Designervariable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Verwendete Ressourcen bereinigen.
    /// </summary>
    /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
    protected override void Dispose( bool disposing )
    {
      if ( disposing && ( components != null ) )
      {
        components.Dispose();
      }
      base.Dispose( disposing );
    }

    #region Vom Komponenten-Designer generierter Code

    /// <summary>
    /// Erforderliche Methode für die Designerunterstützung.
    /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaletteEditor));
      this.paletteEditorControl = new RetroDevStudio.Controls.PaletteEditorControl();
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      this.SuspendLayout();
      // 
      // paletteEditorControl
      // 
      this.paletteEditorControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this.paletteEditorControl.Location = new System.Drawing.Point(0, 0);
      this.paletteEditorControl.Name = "paletteEditorControl";
      this.paletteEditorControl.Size = new System.Drawing.Size(744, 447);
      this.paletteEditorControl.TabIndex = 1;
      // 
      // PaletteEditor
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.ClientSize = new System.Drawing.Size(744, 447);
      this.Controls.Add(this.paletteEditorControl);
      this.MinimumSize = new System.Drawing.Size(274, 159);
      this.Name = "PaletteEditor";
      this.Text = "Palette Editor";
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      this.ResumeLayout(false);

    }


    #endregion

    private Controls.PaletteEditorControl paletteEditorControl;
  }
}
