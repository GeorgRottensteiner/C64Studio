
namespace C64Studio.Controls
{
  partial class ImportCharsetFromDefault
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
      this.label1 = new System.Windows.Forms.Label();
      this.comboImportFromDefault = new System.Windows.Forms.ComboBox();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(3, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(44, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Default:";
      // 
      // comboImportFromDefault
      // 
      this.comboImportFromDefault.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboImportFromDefault.FormattingEnabled = true;
      this.comboImportFromDefault.Location = new System.Drawing.Point(53, 6);
      this.comboImportFromDefault.Name = "comboImportFromDefault";
      this.comboImportFromDefault.Size = new System.Drawing.Size(261, 21);
      this.comboImportFromDefault.TabIndex = 1;
      // 
      // ImportFromDefault
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.comboImportFromDefault);
      this.Controls.Add(this.label1);
      this.Name = "ImportFromDefault";
      this.Size = new System.Drawing.Size(317, 317);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox comboImportFromDefault;
  }
}
