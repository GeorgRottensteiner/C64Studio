namespace RetroDevStudio.Dialogs.Preferences
{
  partial class DlgPrefBASICWarnings
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
      this.listWarningsAsErrors = new DecentForms.ListBox();
      this.label33 = new System.Windows.Forms.Label();
      this.listIgnoredWarnings = new DecentForms.ListBox();
      this.label20 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // listWarningsAsErrors
      // 
      this.listWarningsAsErrors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.listWarningsAsErrors.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.listWarningsAsErrors.HasCheckBoxes = true;
      this.listWarningsAsErrors.ItemHeight = 15;
      this.listWarningsAsErrors.Location = new System.Drawing.Point(15, 123);
      this.listWarningsAsErrors.Name = "listWarningsAsErrors";
      this.listWarningsAsErrors.ScrollAlwaysVisible = false;
      this.listWarningsAsErrors.SelectionMode = DecentForms.SelectionMode.NONE;
      this.listWarningsAsErrors.Size = new System.Drawing.Size(586, 79);
      this.listWarningsAsErrors.TabIndex = 1;
      this.listWarningsAsErrors.ItemCheck += new DecentForms.EventHandler(this.listWarningsAsErrors_ItemCheck);
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
      this.listIgnoredWarnings.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.listIgnoredWarnings.HasCheckBoxes = true;
      this.listIgnoredWarnings.ItemHeight = 15;
      this.listIgnoredWarnings.Location = new System.Drawing.Point(15, 19);
      this.listIgnoredWarnings.Name = "listIgnoredWarnings";
      this.listIgnoredWarnings.ScrollAlwaysVisible = false;
      this.listIgnoredWarnings.SelectionMode = DecentForms.SelectionMode.NONE;
      this.listIgnoredWarnings.Size = new System.Drawing.Size(586, 79);
      this.listIgnoredWarnings.TabIndex = 0;
      this.listIgnoredWarnings.ItemCheck += new DecentForms.EventHandler(this.listIgnoredWarnings_ItemCheck);
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
      // DlgPrefBASICWarnings
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.label20);
      this.Controls.Add(this.listIgnoredWarnings);
      this.Controls.Add(this.label33);
      this.Controls.Add(this.listWarningsAsErrors);
      this.Name = "DlgPrefBASICWarnings";
      this.Size = new System.Drawing.Size(628, 233);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

        #endregion
        private DecentForms.ListBox listWarningsAsErrors;
        private System.Windows.Forms.Label label33;
        private DecentForms.ListBox listIgnoredWarnings;
        private System.Windows.Forms.Label label20;
  }
}
