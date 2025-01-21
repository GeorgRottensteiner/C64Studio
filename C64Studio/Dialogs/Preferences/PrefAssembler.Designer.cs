namespace RetroDevStudio.Dialogs.Preferences
{
  partial class PrefAssembler
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
      this.listHacks = new System.Windows.Forms.CheckedListBox();
      this.label34 = new System.Windows.Forms.Label();
      this.listWarningsAsErrors = new System.Windows.Forms.CheckedListBox();
      this.label33 = new System.Windows.Forms.Label();
      this.listIgnoredWarnings = new System.Windows.Forms.CheckedListBox();
      this.label20 = new System.Windows.Forms.Label();
      this.checkLabelFileSkipAssemblerIDLabels = new System.Windows.Forms.CheckBox();
      this.checkASMAutoTruncateLiteralValues = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // listHacks
      // 
      this.listHacks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.listHacks.CheckOnClick = true;
      this.listHacks.FormattingEnabled = true;
      this.listHacks.Location = new System.Drawing.Point(15, 231);
      this.listHacks.Name = "listHacks";
      this.listHacks.Size = new System.Drawing.Size(586, 79);
      this.listHacks.TabIndex = 2;
      this.listHacks.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listHacks_ItemCheck);
      // 
      // label34
      // 
      this.label34.AutoSize = true;
      this.label34.Location = new System.Drawing.Point(3, 215);
      this.label34.Name = "label34";
      this.label34.Size = new System.Drawing.Size(207, 13);
      this.label34.TabIndex = 20;
      this.label34.Text = "Enable Hacks (C64Studio assembler only):";
      // 
      // listWarningsAsErrors
      // 
      this.listWarningsAsErrors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.listWarningsAsErrors.CheckOnClick = true;
      this.listWarningsAsErrors.FormattingEnabled = true;
      this.listWarningsAsErrors.Location = new System.Drawing.Point(15, 123);
      this.listWarningsAsErrors.Name = "listWarningsAsErrors";
      this.listWarningsAsErrors.Size = new System.Drawing.Size(586, 79);
      this.listWarningsAsErrors.TabIndex = 1;
      this.listWarningsAsErrors.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listWarningsAsErrors_ItemCheck);
      // 
      // label33
      // 
      this.label33.AutoSize = true;
      this.label33.Location = new System.Drawing.Point(3, 107);
      this.label33.Name = "label33";
      this.label33.Size = new System.Drawing.Size(127, 13);
      this.label33.TabIndex = 21;
      this.label33.Text = "Treat Warnings as Errors:";
      // 
      // listIgnoredWarnings
      // 
      this.listIgnoredWarnings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.listIgnoredWarnings.CheckOnClick = true;
      this.listIgnoredWarnings.FormattingEnabled = true;
      this.listIgnoredWarnings.Location = new System.Drawing.Point(15, 19);
      this.listIgnoredWarnings.Name = "listIgnoredWarnings";
      this.listIgnoredWarnings.Size = new System.Drawing.Size(586, 79);
      this.listIgnoredWarnings.TabIndex = 0;
      this.listIgnoredWarnings.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listIgnoredWarnings_ItemCheck);
      // 
      // label20
      // 
      this.label20.AutoSize = true;
      this.label20.Location = new System.Drawing.Point(3, 3);
      this.label20.Name = "label20";
      this.label20.Size = new System.Drawing.Size(88, 13);
      this.label20.TabIndex = 22;
      this.label20.Text = "Ignore Warnings:";
      // 
      // checkLabelFileSkipAssemblerIDLabels
      // 
      this.checkLabelFileSkipAssemblerIDLabels.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkLabelFileSkipAssemblerIDLabels.Location = new System.Drawing.Point(6, 344);
      this.checkLabelFileSkipAssemblerIDLabels.Name = "checkLabelFileSkipAssemblerIDLabels";
      this.checkLabelFileSkipAssemblerIDLabels.Size = new System.Drawing.Size(349, 24);
      this.checkLabelFileSkipAssemblerIDLabels.TabIndex = 4;
      this.checkLabelFileSkipAssemblerIDLabels.Text = "Do not write assembler ID labels in label file";
      this.checkLabelFileSkipAssemblerIDLabels.UseVisualStyleBackColor = true;
      this.checkLabelFileSkipAssemblerIDLabels.CheckedChanged += new System.EventHandler(this.checkLabelFileSkipAssemblerIDLabels_CheckedChanged);
      // 
      // checkASMAutoTruncateLiteralValues
      // 
      this.checkASMAutoTruncateLiteralValues.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkASMAutoTruncateLiteralValues.Location = new System.Drawing.Point(6, 324);
      this.checkASMAutoTruncateLiteralValues.Name = "checkASMAutoTruncateLiteralValues";
      this.checkASMAutoTruncateLiteralValues.Size = new System.Drawing.Size(349, 24);
      this.checkASMAutoTruncateLiteralValues.TabIndex = 3;
      this.checkASMAutoTruncateLiteralValues.Text = "Truncate literal values (no warning on overflow)";
      this.checkASMAutoTruncateLiteralValues.UseVisualStyleBackColor = true;
      this.checkASMAutoTruncateLiteralValues.CheckedChanged += new System.EventHandler(this.checkASMAutoTruncateLiteralValues_CheckedChanged);
      // 
      // PrefAssembler
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.label20);
      this.Controls.Add(this.checkASMAutoTruncateLiteralValues);
      this.Controls.Add(this.checkLabelFileSkipAssemblerIDLabels);
      this.Controls.Add(this.listHacks);
      this.Controls.Add(this.listIgnoredWarnings);
      this.Controls.Add(this.label34);
      this.Controls.Add(this.label33);
      this.Controls.Add(this.listWarningsAsErrors);
      this.Name = "PrefAssembler";
      this.Size = new System.Drawing.Size(628, 394);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

        #endregion
        private System.Windows.Forms.CheckBox checkASMAutoTruncateLiteralValues;
        private System.Windows.Forms.CheckedListBox listHacks;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.CheckedListBox listWarningsAsErrors;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.CheckedListBox listIgnoredWarnings;
        private System.Windows.Forms.Label label20;
    private System.Windows.Forms.CheckBox checkLabelFileSkipAssemblerIDLabels;
  }
}
