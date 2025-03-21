﻿
namespace RetroDevStudio.Controls
{
  partial class ExportGraphicScreenAsBASICData
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
      this.checkExportToDataWrap = new System.Windows.Forms.CheckBox();
      this.editWrapByteCount = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.editExportBASICLineOffset = new System.Windows.Forms.TextBox();
      this.editExportBASICLineNo = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.editWrapCharCount = new System.Windows.Forms.TextBox();
      this.checkWrapAtMaxChars = new System.Windows.Forms.CheckBox();
      this.checkInsertSpaces = new System.Windows.Forms.CheckBox();
      this.label5 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.comboExportContent = new System.Windows.Forms.ComboBox();
      this.comboExportType = new System.Windows.Forms.ComboBox();
      this.SuspendLayout();
      // 
      // checkExportHex
      // 
      this.checkExportHex.AutoSize = true;
      this.checkExportHex.Location = new System.Drawing.Point(6, 162);
      this.checkExportHex.Name = "checkExportHex";
      this.checkExportHex.Size = new System.Drawing.Size(175, 17);
      this.checkExportHex.TabIndex = 6;
      this.checkExportHex.Text = "Export with Hex notation (C128)";
      this.checkExportHex.UseVisualStyleBackColor = true;
      // 
      // checkExportToDataWrap
      // 
      this.checkExportToDataWrap.AutoSize = true;
      this.checkExportToDataWrap.Location = new System.Drawing.Point(6, 138);
      this.checkExportToDataWrap.Name = "checkExportToDataWrap";
      this.checkExportToDataWrap.Size = new System.Drawing.Size(64, 17);
      this.checkExportToDataWrap.TabIndex = 4;
      this.checkExportToDataWrap.Text = "Wrap at";
      this.checkExportToDataWrap.UseVisualStyleBackColor = true;
      this.checkExportToDataWrap.CheckedChanged += new System.EventHandler(this.checkExportToDataWrap_CheckedChanged);
      // 
      // editWrapByteCount
      // 
      this.editWrapByteCount.Enabled = false;
      this.editWrapByteCount.Location = new System.Drawing.Point(93, 136);
      this.editWrapByteCount.Name = "editWrapByteCount";
      this.editWrapByteCount.Size = new System.Drawing.Size(73, 20);
      this.editWrapByteCount.TabIndex = 5;
      this.editWrapByteCount.Text = "40";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(172, 139);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(32, 13);
      this.label2.TabIndex = 19;
      this.label2.Text = "bytes";
      // 
      // editExportBASICLineOffset
      // 
      this.editExportBASICLineOffset.Location = new System.Drawing.Point(93, 83);
      this.editExportBASICLineOffset.Name = "editExportBASICLineOffset";
      this.editExportBASICLineOffset.Size = new System.Drawing.Size(73, 20);
      this.editExportBASICLineOffset.TabIndex = 1;
      this.editExportBASICLineOffset.Text = "10";
      // 
      // editExportBASICLineNo
      // 
      this.editExportBASICLineNo.Location = new System.Drawing.Point(93, 57);
      this.editExportBASICLineNo.Name = "editExportBASICLineNo";
      this.editExportBASICLineNo.Size = new System.Drawing.Size(73, 20);
      this.editExportBASICLineNo.TabIndex = 0;
      this.editExportBASICLineNo.Text = "10";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(3, 86);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(55, 13);
      this.label3.TabIndex = 24;
      this.label3.Text = "Line Step:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(3, 60);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(55, 13);
      this.label4.TabIndex = 26;
      this.label4.Text = "Start Line:";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(172, 113);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(57, 13);
      this.label1.TabIndex = 19;
      this.label1.Text = "characters";
      // 
      // editWrapCharCount
      // 
      this.editWrapCharCount.Location = new System.Drawing.Point(93, 110);
      this.editWrapCharCount.Name = "editWrapCharCount";
      this.editWrapCharCount.Size = new System.Drawing.Size(73, 20);
      this.editWrapCharCount.TabIndex = 3;
      this.editWrapCharCount.Text = "80";
      // 
      // checkWrapAtMaxChars
      // 
      this.checkWrapAtMaxChars.AutoSize = true;
      this.checkWrapAtMaxChars.Checked = true;
      this.checkWrapAtMaxChars.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkWrapAtMaxChars.Location = new System.Drawing.Point(6, 112);
      this.checkWrapAtMaxChars.Name = "checkWrapAtMaxChars";
      this.checkWrapAtMaxChars.Size = new System.Drawing.Size(64, 17);
      this.checkWrapAtMaxChars.TabIndex = 2;
      this.checkWrapAtMaxChars.Text = "Wrap at";
      this.checkWrapAtMaxChars.UseVisualStyleBackColor = true;
      this.checkWrapAtMaxChars.CheckedChanged += new System.EventHandler(this.checkExportToDataWrap_CheckedChanged);
      // 
      // checkInsertSpaces
      // 
      this.checkInsertSpaces.AutoSize = true;
      this.checkInsertSpaces.Location = new System.Drawing.Point(6, 185);
      this.checkInsertSpaces.Name = "checkInsertSpaces";
      this.checkInsertSpaces.Size = new System.Drawing.Size(91, 17);
      this.checkInsertSpaces.TabIndex = 7;
      this.checkInsertSpaces.Text = "Insert Spaces";
      this.checkInsertSpaces.UseVisualStyleBackColor = true;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(3, 33);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(47, 13);
      this.label5.TabIndex = 27;
      this.label5.Text = "Content:";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(3, 6);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(67, 13);
      this.label6.TabIndex = 28;
      this.label6.Text = "Export Type:";
      // 
      // comboExportContent
      // 
      this.comboExportContent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboExportContent.FormattingEnabled = true;
      this.comboExportContent.Location = new System.Drawing.Point(75, 30);
      this.comboExportContent.Name = "comboExportContent";
      this.comboExportContent.Size = new System.Drawing.Size(239, 21);
      this.comboExportContent.TabIndex = 29;
      // 
      // comboExportType
      // 
      this.comboExportType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboExportType.FormattingEnabled = true;
      this.comboExportType.Location = new System.Drawing.Point(75, 3);
      this.comboExportType.Name = "comboExportType";
      this.comboExportType.Size = new System.Drawing.Size(239, 21);
      this.comboExportType.TabIndex = 30;
      // 
      // ExportGraphicScreenAsBASICData
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.label5);
      this.Controls.Add(this.label6);
      this.Controls.Add(this.comboExportContent);
      this.Controls.Add(this.comboExportType);
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
      this.Name = "ExportGraphicScreenAsBASICData";
      this.Size = new System.Drawing.Size(317, 317);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.CheckBox checkExportHex;
    private System.Windows.Forms.CheckBox checkExportToDataWrap;
    private System.Windows.Forms.TextBox editWrapByteCount;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox editExportBASICLineOffset;
    private System.Windows.Forms.TextBox editExportBASICLineNo;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox editWrapCharCount;
    private System.Windows.Forms.CheckBox checkWrapAtMaxChars;
    private System.Windows.Forms.CheckBox checkInsertSpaces;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.ComboBox comboExportContent;
    private System.Windows.Forms.ComboBox comboExportType;
  }
}
