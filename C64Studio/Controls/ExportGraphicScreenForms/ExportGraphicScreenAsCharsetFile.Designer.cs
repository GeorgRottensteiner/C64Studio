
namespace RetroDevStudio.Controls
{
  partial class ExportGraphicScreenAsCharsetFile
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
      this.comboCharScreens = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // comboCharScreens
      // 
      this.comboCharScreens.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.comboCharScreens.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboCharScreens.FormattingEnabled = true;
      this.comboCharScreens.Location = new System.Drawing.Point(61, 5);
      this.comboCharScreens.Name = "comboCharScreens";
      this.comboCharScreens.Size = new System.Drawing.Size(253, 21);
      this.comboCharScreens.TabIndex = 13;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(3, 8);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(52, 13);
      this.label1.TabIndex = 14;
      this.label1.Text = "Export to:";
      // 
      // ExportGraphicScreenAsCharsetFile
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.Controls.Add(this.label1);
      this.Controls.Add(this.comboCharScreens);
      this.Name = "ExportGraphicScreenAsCharsetFile";
      this.Size = new System.Drawing.Size(317, 317);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ComboBox comboCharScreens;
    private System.Windows.Forms.Label label1;
  }
}
