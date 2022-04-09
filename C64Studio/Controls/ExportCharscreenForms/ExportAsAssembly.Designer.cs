
namespace C64Studio.Controls
{
  partial class ExportAsAssembly
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
      this.checkExportASMAsPetSCII = new System.Windows.Forms.CheckBox();
      this.checkExportHex = new System.Windows.Forms.CheckBox();
      this.checkExportToDataIncludeRes = new System.Windows.Forms.CheckBox();
      this.checkExportToDataWrap = new System.Windows.Forms.CheckBox();
      this.editWrapByteCount = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.editPrefix = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // checkExportASMAsPetSCII
      // 
      this.checkExportASMAsPetSCII.AutoSize = true;
      this.checkExportASMAsPetSCII.Location = new System.Drawing.Point(3, 72);
      this.checkExportASMAsPetSCII.Name = "checkExportASMAsPetSCII";
      this.checkExportASMAsPetSCII.Size = new System.Drawing.Size(93, 17);
      this.checkExportASMAsPetSCII.TabIndex = 22;
      this.checkExportASMAsPetSCII.Text = "Prefer PetSCII";
      this.checkExportASMAsPetSCII.UseVisualStyleBackColor = true;
      // 
      // checkExportHex
      // 
      this.checkExportHex.AutoSize = true;
      this.checkExportHex.Checked = true;
      this.checkExportHex.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkExportHex.Location = new System.Drawing.Point(3, 49);
      this.checkExportHex.Name = "checkExportHex";
      this.checkExportHex.Size = new System.Drawing.Size(141, 17);
      this.checkExportHex.TabIndex = 20;
      this.checkExportHex.Text = "Export with Hex notation";
      this.checkExportHex.UseVisualStyleBackColor = true;
      // 
      // checkExportToDataIncludeRes
      // 
      this.checkExportToDataIncludeRes.AutoSize = true;
      this.checkExportToDataIncludeRes.Checked = true;
      this.checkExportToDataIncludeRes.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkExportToDataIncludeRes.Location = new System.Drawing.Point(3, 3);
      this.checkExportToDataIncludeRes.Name = "checkExportToDataIncludeRes";
      this.checkExportToDataIncludeRes.Size = new System.Drawing.Size(74, 17);
      this.checkExportToDataIncludeRes.TabIndex = 16;
      this.checkExportToDataIncludeRes.Text = "Prefix with";
      this.checkExportToDataIncludeRes.UseVisualStyleBackColor = true;
      this.checkExportToDataIncludeRes.CheckedChanged += new System.EventHandler(this.checkExportToDataIncludeRes_CheckedChanged);
      // 
      // checkExportToDataWrap
      // 
      this.checkExportToDataWrap.AutoSize = true;
      this.checkExportToDataWrap.Checked = true;
      this.checkExportToDataWrap.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkExportToDataWrap.Location = new System.Drawing.Point(3, 26);
      this.checkExportToDataWrap.Name = "checkExportToDataWrap";
      this.checkExportToDataWrap.Size = new System.Drawing.Size(64, 17);
      this.checkExportToDataWrap.TabIndex = 17;
      this.checkExportToDataWrap.Text = "Wrap at";
      this.checkExportToDataWrap.UseVisualStyleBackColor = true;
      this.checkExportToDataWrap.CheckedChanged += new System.EventHandler(this.checkExportToDataWrap_CheckedChanged);
      // 
      // editWrapByteCount
      // 
      this.editWrapByteCount.Location = new System.Drawing.Point(99, 24);
      this.editWrapByteCount.Name = "editWrapByteCount";
      this.editWrapByteCount.Size = new System.Drawing.Size(64, 20);
      this.editWrapByteCount.TabIndex = 21;
      this.editWrapByteCount.Text = "40";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(169, 26);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(32, 13);
      this.label2.TabIndex = 19;
      this.label2.Text = "bytes";
      // 
      // editPrefix
      // 
      this.editPrefix.Location = new System.Drawing.Point(99, 1);
      this.editPrefix.Name = "editPrefix";
      this.editPrefix.Size = new System.Drawing.Size(64, 20);
      this.editPrefix.TabIndex = 18;
      this.editPrefix.Text = "!byte ";
      // 
      // ExportAsAssembly
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.checkExportASMAsPetSCII);
      this.Controls.Add(this.checkExportHex);
      this.Controls.Add(this.checkExportToDataIncludeRes);
      this.Controls.Add(this.checkExportToDataWrap);
      this.Controls.Add(this.editWrapByteCount);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.editPrefix);
      this.Name = "ExportAsAssembly";
      this.Size = new System.Drawing.Size(317, 317);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.CheckBox checkExportASMAsPetSCII;
    private System.Windows.Forms.CheckBox checkExportHex;
    private System.Windows.Forms.CheckBox checkExportToDataIncludeRes;
    private System.Windows.Forms.CheckBox checkExportToDataWrap;
    private System.Windows.Forms.TextBox editWrapByteCount;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox editPrefix;
  }
}
