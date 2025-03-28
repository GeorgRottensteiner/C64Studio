
namespace RetroDevStudio.Controls
{
  partial class ExportGraphicScreenAsBinaryFile
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
      this.editPrefixLoadAddress = new System.Windows.Forms.TextBox();
      this.checkPrefixLoadAddress = new System.Windows.Forms.CheckBox();
      this.label1 = new System.Windows.Forms.Label();
      this.comboExportContent = new System.Windows.Forms.ComboBox();
      this.label6 = new System.Windows.Forms.Label();
      this.comboExportType = new System.Windows.Forms.ComboBox();
      this.SuspendLayout();
      // 
      // editPrefixLoadAddress
      // 
      this.editPrefixLoadAddress.Enabled = false;
      this.editPrefixLoadAddress.Location = new System.Drawing.Point(160, 57);
      this.editPrefixLoadAddress.MaxLength = 4;
      this.editPrefixLoadAddress.Name = "editPrefixLoadAddress";
      this.editPrefixLoadAddress.Size = new System.Drawing.Size(66, 20);
      this.editPrefixLoadAddress.TabIndex = 1;
      this.editPrefixLoadAddress.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editPrefixLoadAddress_KeyPress);
      // 
      // checkPrefixLoadAddress
      // 
      this.checkPrefixLoadAddress.AutoSize = true;
      this.checkPrefixLoadAddress.Location = new System.Drawing.Point(8, 59);
      this.checkPrefixLoadAddress.Name = "checkPrefixLoadAddress";
      this.checkPrefixLoadAddress.Size = new System.Drawing.Size(146, 17);
      this.checkPrefixLoadAddress.TabIndex = 0;
      this.checkPrefixLoadAddress.Text = "Prefix Load Address (hex)";
      this.checkPrefixLoadAddress.UseVisualStyleBackColor = true;
      this.checkPrefixLoadAddress.CheckedChanged += new System.EventHandler(this.checkPrefixLoadAddress_CheckedChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(5, 32);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(47, 13);
      this.label1.TabIndex = 8;
      this.label1.Text = "Content:";
      // 
      // comboExportContent
      // 
      this.comboExportContent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboExportContent.FormattingEnabled = true;
      this.comboExportContent.Location = new System.Drawing.Point(75, 29);
      this.comboExportContent.Name = "comboExportContent";
      this.comboExportContent.Size = new System.Drawing.Size(222, 21);
      this.comboExportContent.TabIndex = 9;
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(3, 8);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(67, 13);
      this.label6.TabIndex = 10;
      this.label6.Text = "Export Type:";
      // 
      // comboExportType
      // 
      this.comboExportType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboExportType.FormattingEnabled = true;
      this.comboExportType.Location = new System.Drawing.Point(75, 5);
      this.comboExportType.Name = "comboExportType";
      this.comboExportType.Size = new System.Drawing.Size(222, 21);
      this.comboExportType.TabIndex = 11;
      this.comboExportType.SelectedIndexChanged += new System.EventHandler(this.comboExportType_SelectedIndexChanged);
      // 
      // ExportGraphicScreenAsBinaryFile
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.label6);
      this.Controls.Add(this.comboExportType);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.comboExportContent);
      this.Controls.Add(this.editPrefixLoadAddress);
      this.Controls.Add(this.checkPrefixLoadAddress);
      this.Name = "ExportGraphicScreenAsBinaryFile";
      this.Size = new System.Drawing.Size(317, 317);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox editPrefixLoadAddress;
    private System.Windows.Forms.CheckBox checkPrefixLoadAddress;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox comboExportContent;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.ComboBox comboExportType;
  }
}
