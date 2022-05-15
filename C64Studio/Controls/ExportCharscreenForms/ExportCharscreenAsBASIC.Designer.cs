
namespace RetroDevStudio.Controls
{
  partial class ExportCharsetAsBASIC
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
      this.checkExportToDataWrap = new System.Windows.Forms.CheckBox();
      this.editWrapByteCount = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.editExportBASICLineOffset = new System.Windows.Forms.TextBox();
      this.editExportBASICLineNo = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.comboBasicFiles = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.checkExportToBASICReplaceSpaceWithRight = new System.Windows.Forms.CheckBox();
      this.checkExportToBASICAsString = new System.Windows.Forms.CheckBox();
      this.checkExportToBASICCollapseColors = new System.Windows.Forms.CheckBox();
      this.checkExportToBASICReplaceShiftSpaceWithSpace = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // checkExportToDataWrap
      // 
      this.checkExportToDataWrap.AutoSize = true;
      this.checkExportToDataWrap.Checked = true;
      this.checkExportToDataWrap.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkExportToDataWrap.Location = new System.Drawing.Point(3, 3);
      this.checkExportToDataWrap.Name = "checkExportToDataWrap";
      this.checkExportToDataWrap.Size = new System.Drawing.Size(64, 17);
      this.checkExportToDataWrap.TabIndex = 0;
      this.checkExportToDataWrap.Text = "Wrap at";
      this.checkExportToDataWrap.UseVisualStyleBackColor = true;
      this.checkExportToDataWrap.CheckedChanged += new System.EventHandler(this.checkExportToDataWrap_CheckedChanged);
      // 
      // editWrapByteCount
      // 
      this.editWrapByteCount.Location = new System.Drawing.Point(90, 1);
      this.editWrapByteCount.Name = "editWrapByteCount";
      this.editWrapByteCount.Size = new System.Drawing.Size(73, 20);
      this.editWrapByteCount.TabIndex = 1;
      this.editWrapByteCount.Text = "40";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(169, 3);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(32, 13);
      this.label2.TabIndex = 19;
      this.label2.Text = "bytes";
      // 
      // editExportBASICLineOffset
      // 
      this.editExportBASICLineOffset.Location = new System.Drawing.Point(90, 53);
      this.editExportBASICLineOffset.Name = "editExportBASICLineOffset";
      this.editExportBASICLineOffset.Size = new System.Drawing.Size(73, 20);
      this.editExportBASICLineOffset.TabIndex = 3;
      this.editExportBASICLineOffset.Text = "10";
      // 
      // editExportBASICLineNo
      // 
      this.editExportBASICLineNo.Location = new System.Drawing.Point(90, 27);
      this.editExportBASICLineNo.Name = "editExportBASICLineNo";
      this.editExportBASICLineNo.Size = new System.Drawing.Size(73, 20);
      this.editExportBASICLineNo.TabIndex = 2;
      this.editExportBASICLineNo.Text = "10";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(0, 56);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(55, 13);
      this.label3.TabIndex = 24;
      this.label3.Text = "Line Step:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(0, 30);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(55, 13);
      this.label4.TabIndex = 26;
      this.label4.Text = "Start Line:";
      // 
      // comboBasicFiles
      // 
      this.comboBasicFiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBasicFiles.FormattingEnabled = true;
      this.comboBasicFiles.Location = new System.Drawing.Point(90, 79);
      this.comboBasicFiles.Name = "comboBasicFiles";
      this.comboBasicFiles.Size = new System.Drawing.Size(224, 21);
      this.comboBasicFiles.TabIndex = 4;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(0, 82);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(52, 13);
      this.label1.TabIndex = 24;
      this.label1.Text = "Export to:";
      // 
      // checkExportToBASICReplaceSpaceWithRight
      // 
      this.checkExportToBASICReplaceSpaceWithRight.AutoSize = true;
      this.checkExportToBASICReplaceSpaceWithRight.Location = new System.Drawing.Point(3, 129);
      this.checkExportToBASICReplaceSpaceWithRight.Name = "checkExportToBASICReplaceSpaceWithRight";
      this.checkExportToBASICReplaceSpaceWithRight.Size = new System.Drawing.Size(183, 17);
      this.checkExportToBASICReplaceSpaceWithRight.TabIndex = 6;
      this.checkExportToBASICReplaceSpaceWithRight.Text = "Replace Space with Cursor Right";
      this.checkExportToBASICReplaceSpaceWithRight.UseVisualStyleBackColor = true;
      // 
      // checkExportToBASICAsString
      // 
      this.checkExportToBASICAsString.AutoSize = true;
      this.checkExportToBASICAsString.Location = new System.Drawing.Point(3, 175);
      this.checkExportToBASICAsString.Name = "checkExportToBASICAsString";
      this.checkExportToBASICAsString.Size = new System.Drawing.Size(239, 17);
      this.checkExportToBASICAsString.TabIndex = 8;
      this.checkExportToBASICAsString.Text = "As String (use Down/Left instead of new line)";
      this.checkExportToBASICAsString.UseVisualStyleBackColor = true;
      // 
      // checkExportToBASICCollapseColors
      // 
      this.checkExportToBASICCollapseColors.AutoSize = true;
      this.checkExportToBASICCollapseColors.Location = new System.Drawing.Point(3, 106);
      this.checkExportToBASICCollapseColors.Name = "checkExportToBASICCollapseColors";
      this.checkExportToBASICCollapseColors.Size = new System.Drawing.Size(118, 17);
      this.checkExportToBASICCollapseColors.TabIndex = 5;
      this.checkExportToBASICCollapseColors.Text = "Strip invisible colors";
      this.checkExportToBASICCollapseColors.UseVisualStyleBackColor = true;
      // 
      // checkExportToBASICReplaceShiftSpaceWithSpace
      // 
      this.checkExportToBASICReplaceShiftSpaceWithSpace.AutoSize = true;
      this.checkExportToBASICReplaceShiftSpaceWithSpace.Location = new System.Drawing.Point(3, 152);
      this.checkExportToBASICReplaceShiftSpaceWithSpace.Name = "checkExportToBASICReplaceShiftSpaceWithSpace";
      this.checkExportToBASICReplaceShiftSpaceWithSpace.Size = new System.Drawing.Size(180, 17);
      this.checkExportToBASICReplaceShiftSpaceWithSpace.TabIndex = 7;
      this.checkExportToBASICReplaceShiftSpaceWithSpace.Text = "Replace Shift-Space with Space";
      this.checkExportToBASICReplaceShiftSpaceWithSpace.UseVisualStyleBackColor = true;
      // 
      // ExportAsBASIC
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.checkExportToBASICReplaceShiftSpaceWithSpace);
      this.Controls.Add(this.checkExportToBASICReplaceSpaceWithRight);
      this.Controls.Add(this.checkExportToBASICAsString);
      this.Controls.Add(this.checkExportToBASICCollapseColors);
      this.Controls.Add(this.comboBasicFiles);
      this.Controls.Add(this.editExportBASICLineOffset);
      this.Controls.Add(this.editExportBASICLineNo);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.checkExportToDataWrap);
      this.Controls.Add(this.editWrapByteCount);
      this.Controls.Add(this.label2);
      this.Name = "ExportAsBASIC";
      this.Size = new System.Drawing.Size(317, 317);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.CheckBox checkExportToDataWrap;
    private System.Windows.Forms.TextBox editWrapByteCount;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox editExportBASICLineOffset;
    private System.Windows.Forms.TextBox editExportBASICLineNo;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.ComboBox comboBasicFiles;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.CheckBox checkExportToBASICReplaceSpaceWithRight;
    private System.Windows.Forms.CheckBox checkExportToBASICAsString;
    private System.Windows.Forms.CheckBox checkExportToBASICCollapseColors;
    private System.Windows.Forms.CheckBox checkExportToBASICReplaceShiftSpaceWithSpace;
  }
}
