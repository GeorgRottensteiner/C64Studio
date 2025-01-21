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
      this.paletteEditor = new RetroDevStudio.Controls.PaletteEditorControl();
      this.SuspendLayout();
      // 
      // paletteEditor
      // 
      this.paletteEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.paletteEditor.Location = new System.Drawing.Point(3, 3);
      this.paletteEditor.Name = "paletteEditor";
      this.paletteEditor.Size = new System.Drawing.Size(653, 462);
      this.paletteEditor.TabIndex = 14;
      this.paletteEditor.PaletteOrderModified += new RetroDevStudio.Controls.PaletteEditorControl.PaletteOrderModifiedHandler(this.paletteEditor_PaletteOrderModified);
      this.paletteEditor.PaletteModified += new RetroDevStudio.Controls.PaletteEditorControl.PaletteModifiedHandler(this.paletteEditor_PaletteModified);
      // 
      // PrefPalettes
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.paletteEditor);
      this.Name = "PrefPalettes";
      this.Size = new System.Drawing.Size(664, 476);
      this.ResumeLayout(false);

    }

        #endregion
        private Controls.PaletteEditorControl paletteEditor;
  }
}
