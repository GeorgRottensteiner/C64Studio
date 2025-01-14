
namespace RetroDevStudio.Controls
{
  partial class ExportBinaryAsHex
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
      this.editTextOutput = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // editTextOutput
      // 
      this.editTextOutput.Dock = System.Windows.Forms.DockStyle.Fill;
      this.editTextOutput.Location = new System.Drawing.Point(0, 0);
      this.editTextOutput.MaxLength = 1000000;
      this.editTextOutput.Multiline = true;
      this.editTextOutput.Name = "editTextOutput";
      this.editTextOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.editTextOutput.Size = new System.Drawing.Size(194, 404);
      this.editTextOutput.TabIndex = 21;
      // 
      // ExportBinaryAsHex
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.editTextOutput);
      this.Name = "ExportBinaryAsHex";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox editTextOutput;
  }
}
