﻿
namespace RetroDevStudio.Controls
{
  partial class ExportBinaryAsBASICData
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
      this.checkInsertSpaces = new System.Windows.Forms.CheckBox();
      this.editExportBASICLineOffset = new System.Windows.Forms.TextBox();
      this.editExportBASICLineNo = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.checkExportHex = new System.Windows.Forms.CheckBox();
      this.checkWrapAtMaxChars = new System.Windows.Forms.CheckBox();
      this.checkExportToDataWrap = new System.Windows.Forms.CheckBox();
      this.editWrapCharCount = new System.Windows.Forms.TextBox();
      this.editWrapByteCount = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.editTextOutput = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // checkInsertSpaces
      // 
      this.checkInsertSpaces.AutoSize = true;
      this.checkInsertSpaces.Location = new System.Drawing.Point(3, 131);
      this.checkInsertSpaces.Name = "checkInsertSpaces";
      this.checkInsertSpaces.Size = new System.Drawing.Size(91, 17);
      this.checkInsertSpaces.TabIndex = 34;
      this.checkInsertSpaces.Text = "Insert Spaces";
      this.checkInsertSpaces.UseVisualStyleBackColor = true;
      // 
      // editExportBASICLineOffset
      // 
      this.editExportBASICLineOffset.Location = new System.Drawing.Point(73, 29);
      this.editExportBASICLineOffset.Name = "editExportBASICLineOffset";
      this.editExportBASICLineOffset.Size = new System.Drawing.Size(73, 20);
      this.editExportBASICLineOffset.TabIndex = 28;
      this.editExportBASICLineOffset.Text = "10";
      // 
      // editExportBASICLineNo
      // 
      this.editExportBASICLineNo.Location = new System.Drawing.Point(73, 3);
      this.editExportBASICLineNo.Name = "editExportBASICLineNo";
      this.editExportBASICLineNo.Size = new System.Drawing.Size(73, 20);
      this.editExportBASICLineNo.TabIndex = 27;
      this.editExportBASICLineNo.Text = "10";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(0, 32);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(55, 13);
      this.label3.TabIndex = 37;
      this.label3.Text = "Line Step:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(0, 6);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(55, 13);
      this.label4.TabIndex = 38;
      this.label4.Text = "Start Line:";
      // 
      // checkExportHex
      // 
      this.checkExportHex.AutoSize = true;
      this.checkExportHex.Location = new System.Drawing.Point(3, 108);
      this.checkExportHex.Name = "checkExportHex";
      this.checkExportHex.Size = new System.Drawing.Size(175, 17);
      this.checkExportHex.TabIndex = 33;
      this.checkExportHex.Text = "Export with Hex notation (C128)";
      this.checkExportHex.UseVisualStyleBackColor = true;
      // 
      // checkWrapAtMaxChars
      // 
      this.checkWrapAtMaxChars.AutoSize = true;
      this.checkWrapAtMaxChars.Checked = true;
      this.checkWrapAtMaxChars.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkWrapAtMaxChars.Location = new System.Drawing.Point(3, 58);
      this.checkWrapAtMaxChars.Name = "checkWrapAtMaxChars";
      this.checkWrapAtMaxChars.Size = new System.Drawing.Size(64, 17);
      this.checkWrapAtMaxChars.TabIndex = 29;
      this.checkWrapAtMaxChars.Text = "Wrap at";
      this.checkWrapAtMaxChars.UseVisualStyleBackColor = true;
      // 
      // checkExportToDataWrap
      // 
      this.checkExportToDataWrap.AutoSize = true;
      this.checkExportToDataWrap.Location = new System.Drawing.Point(3, 84);
      this.checkExportToDataWrap.Name = "checkExportToDataWrap";
      this.checkExportToDataWrap.Size = new System.Drawing.Size(64, 17);
      this.checkExportToDataWrap.TabIndex = 31;
      this.checkExportToDataWrap.Text = "Wrap at";
      this.checkExportToDataWrap.UseVisualStyleBackColor = true;
      // 
      // editWrapCharCount
      // 
      this.editWrapCharCount.Location = new System.Drawing.Point(73, 56);
      this.editWrapCharCount.Name = "editWrapCharCount";
      this.editWrapCharCount.Size = new System.Drawing.Size(73, 20);
      this.editWrapCharCount.TabIndex = 30;
      this.editWrapCharCount.Text = "80";
      // 
      // editWrapByteCount
      // 
      this.editWrapByteCount.Enabled = false;
      this.editWrapByteCount.Location = new System.Drawing.Point(73, 82);
      this.editWrapByteCount.Name = "editWrapByteCount";
      this.editWrapByteCount.Size = new System.Drawing.Size(73, 20);
      this.editWrapByteCount.TabIndex = 32;
      this.editWrapByteCount.Text = "40";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(152, 59);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(33, 13);
      this.label1.TabIndex = 35;
      this.label1.Text = "chars";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(152, 85);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(32, 13);
      this.label2.TabIndex = 36;
      this.label2.Text = "bytes";
      // 
      // editTextOutput
      // 
      this.editTextOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.editTextOutput.Location = new System.Drawing.Point(191, 3);
      this.editTextOutput.MaxLength = 1000000;
      this.editTextOutput.Multiline = true;
      this.editTextOutput.Name = "editTextOutput";
      this.editTextOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.editTextOutput.Size = new System.Drawing.Size(494, 381);
      this.editTextOutput.TabIndex = 39;
      // 
      // ExportBinaryAsBASICData
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.editTextOutput);
      this.Controls.Add(this.checkInsertSpaces);
      this.Controls.Add(this.editExportBASICLineOffset);
      this.Controls.Add(this.editExportBASICLineNo);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.checkExportHex);
      this.Controls.Add(this.checkWrapAtMaxChars);
      this.Controls.Add(this.checkExportToDataWrap);
      this.Controls.Add(this.editWrapCharCount);
      this.Controls.Add(this.editWrapByteCount);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.label2);
      this.Name = "ExportBinaryAsBASICData";
      this.Size = new System.Drawing.Size(688, 387);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.CheckBox checkInsertSpaces;
    private System.Windows.Forms.TextBox editExportBASICLineOffset;
    private System.Windows.Forms.TextBox editExportBASICLineNo;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.CheckBox checkExportHex;
    private System.Windows.Forms.CheckBox checkWrapAtMaxChars;
    private System.Windows.Forms.CheckBox checkExportToDataWrap;
    private System.Windows.Forms.TextBox editWrapCharCount;
    private System.Windows.Forms.TextBox editWrapByteCount;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox editTextOutput;
  }
}
