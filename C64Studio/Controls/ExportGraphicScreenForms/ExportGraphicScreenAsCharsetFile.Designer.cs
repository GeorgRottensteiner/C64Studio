
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
      this.label2 = new System.Windows.Forms.Label();
      this.comboExportCharsetType = new System.Windows.Forms.ComboBox();
      this.SuspendLayout();
      // 
      // comboCharScreens
      // 
      this.comboCharScreens.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.comboCharScreens.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboCharScreens.FormattingEnabled = true;
      this.comboCharScreens.Location = new System.Drawing.Point(77, 5);
      this.comboCharScreens.Name = "comboCharScreens";
      this.comboCharScreens.Size = new System.Drawing.Size(237, 21);
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
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(3, 38);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(73, 13);
      this.label2.TabIndex = 14;
      this.label2.Text = "Charset Type:";
      // 
      // comboExportCharsetType
      // 
      this.comboExportCharsetType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.comboExportCharsetType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboExportCharsetType.FormattingEnabled = true;
      this.comboExportCharsetType.Location = new System.Drawing.Point(77, 35);
      this.comboExportCharsetType.Name = "comboExportCharsetType";
      this.comboExportCharsetType.Size = new System.Drawing.Size(237, 21);
      this.comboExportCharsetType.TabIndex = 13;
      // 
      // ExportGraphicScreenAsCharsetFile
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.comboExportCharsetType);
      this.Controls.Add(this.comboCharScreens);
      this.Name = "ExportGraphicScreenAsCharsetFile";
      this.Size = new System.Drawing.Size(317, 317);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ComboBox comboCharScreens;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox comboExportCharsetType;
  }
}
