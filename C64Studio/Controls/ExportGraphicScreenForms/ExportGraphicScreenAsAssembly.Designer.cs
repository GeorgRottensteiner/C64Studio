
namespace RetroDevStudio.Controls
{
  partial class ExportGraphicScreenAsAssembly
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
      this.checkExportHex = new System.Windows.Forms.CheckBox();
      this.checkExportToDataIncludeRes = new System.Windows.Forms.CheckBox();
      this.checkExportToDataWrap = new System.Windows.Forms.CheckBox();
      this.editWrapByteCount = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.editPrefix = new System.Windows.Forms.TextBox();
      this.label6 = new System.Windows.Forms.Label();
      this.comboExportType = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.comboExportContent = new System.Windows.Forms.ComboBox();
      this.SuspendLayout();
      // 
      // checkExportHex
      // 
      this.checkExportHex.AutoSize = true;
      this.checkExportHex.Checked = true;
      this.checkExportHex.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkExportHex.Location = new System.Drawing.Point(6, 108);
      this.checkExportHex.Name = "checkExportHex";
      this.checkExportHex.Size = new System.Drawing.Size(141, 17);
      this.checkExportHex.TabIndex = 5;
      this.checkExportHex.Text = "Export with Hex notation";
      this.checkExportHex.UseVisualStyleBackColor = true;
      // 
      // checkExportToDataIncludeRes
      // 
      this.checkExportToDataIncludeRes.AutoSize = true;
      this.checkExportToDataIncludeRes.Checked = true;
      this.checkExportToDataIncludeRes.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkExportToDataIncludeRes.Location = new System.Drawing.Point(6, 62);
      this.checkExportToDataIncludeRes.Name = "checkExportToDataIncludeRes";
      this.checkExportToDataIncludeRes.Size = new System.Drawing.Size(74, 17);
      this.checkExportToDataIncludeRes.TabIndex = 0;
      this.checkExportToDataIncludeRes.Text = "Prefix with";
      this.checkExportToDataIncludeRes.UseVisualStyleBackColor = true;
      this.checkExportToDataIncludeRes.CheckedChanged += new System.EventHandler(this.checkExportToDataIncludeRes_CheckedChanged);
      // 
      // checkExportToDataWrap
      // 
      this.checkExportToDataWrap.AutoSize = true;
      this.checkExportToDataWrap.Checked = true;
      this.checkExportToDataWrap.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkExportToDataWrap.Location = new System.Drawing.Point(6, 85);
      this.checkExportToDataWrap.Name = "checkExportToDataWrap";
      this.checkExportToDataWrap.Size = new System.Drawing.Size(64, 17);
      this.checkExportToDataWrap.TabIndex = 2;
      this.checkExportToDataWrap.Text = "Wrap at";
      this.checkExportToDataWrap.UseVisualStyleBackColor = true;
      this.checkExportToDataWrap.CheckedChanged += new System.EventHandler(this.checkExportToDataWrap_CheckedChanged);
      // 
      // editWrapByteCount
      // 
      this.editWrapByteCount.Location = new System.Drawing.Point(102, 83);
      this.editWrapByteCount.Name = "editWrapByteCount";
      this.editWrapByteCount.Size = new System.Drawing.Size(64, 20);
      this.editWrapByteCount.TabIndex = 3;
      this.editWrapByteCount.Text = "40";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(172, 85);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(32, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "bytes";
      // 
      // editPrefix
      // 
      this.editPrefix.Location = new System.Drawing.Point(102, 60);
      this.editPrefix.Name = "editPrefix";
      this.editPrefix.Size = new System.Drawing.Size(64, 20);
      this.editPrefix.TabIndex = 1;
      this.editPrefix.Text = "!byte ";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(3, 6);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(67, 13);
      this.label6.TabIndex = 6;
      this.label6.Text = "Export Type:";
      // 
      // comboExportType
      // 
      this.comboExportType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboExportType.FormattingEnabled = true;
      this.comboExportType.Location = new System.Drawing.Point(75, 3);
      this.comboExportType.Name = "comboExportType";
      this.comboExportType.Size = new System.Drawing.Size(239, 21);
      this.comboExportType.TabIndex = 7;
      this.comboExportType.SelectedIndexChanged += new System.EventHandler(this.comboExportType_SelectedIndexChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(3, 33);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(47, 13);
      this.label1.TabIndex = 6;
      this.label1.Text = "Content:";
      // 
      // comboExportContent
      // 
      this.comboExportContent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboExportContent.FormattingEnabled = true;
      this.comboExportContent.Location = new System.Drawing.Point(75, 30);
      this.comboExportContent.Name = "comboExportContent";
      this.comboExportContent.Size = new System.Drawing.Size(239, 21);
      this.comboExportContent.TabIndex = 7;
      // 
      // ExportGraphicScreenAsAssembly
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.label1);
      this.Controls.Add(this.label6);
      this.Controls.Add(this.comboExportContent);
      this.Controls.Add(this.comboExportType);
      this.Controls.Add(this.checkExportHex);
      this.Controls.Add(this.checkExportToDataIncludeRes);
      this.Controls.Add(this.checkExportToDataWrap);
      this.Controls.Add(this.editWrapByteCount);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.editPrefix);
      this.Name = "ExportGraphicScreenAsAssembly";
      this.Size = new System.Drawing.Size(317, 317);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.CheckBox checkExportHex;
    private System.Windows.Forms.CheckBox checkExportToDataIncludeRes;
    private System.Windows.Forms.CheckBox checkExportToDataWrap;
    private System.Windows.Forms.TextBox editWrapByteCount;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox editPrefix;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.ComboBox comboExportType;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox comboExportContent;
  }
}
