
namespace RetroDevStudio.Controls
{
  partial class ExportGraphicScreenAsImage
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
      this.checkOptimize = new DecentForms.CheckBox();
      this.label1 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // checkOptimize
      // 
      this.checkOptimize.Appearance = System.Windows.Forms.Appearance.Normal;
      this.checkOptimize.BorderStyle = DecentForms.BorderStyle.NONE;
      this.checkOptimize.CheckAlign = DecentForms.ContentAlignment.MiddleLeft;
      this.checkOptimize.Checked = true;
      this.checkOptimize.Image = null;
      this.checkOptimize.Location = new System.Drawing.Point(3, 3);
      this.checkOptimize.Name = "checkOptimize";
      this.checkOptimize.Size = new System.Drawing.Size(147, 17);
      this.checkOptimize.TabIndex = 0;
      this.checkOptimize.Text = "Optimize Image if possible";
      // 
      // label1
      // 
      this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label1.Location = new System.Drawing.Point(19, 35);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(295, 54);
      this.label1.TabIndex = 1;
      this.label1.Text = "Currently applies only to IFF format, tries to use as few planes as possible";
      // 
      // ExportGraphicScreenAsImage
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.label1);
      this.Controls.Add(this.checkOptimize);
      this.Name = "ExportGraphicScreenAsImage";
      this.Size = new System.Drawing.Size(317, 317);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private DecentForms.CheckBox checkOptimize;
    private System.Windows.Forms.Label label1;
  }
}
