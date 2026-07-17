namespace RetroDevStudio.Dialogs.Preferences
{
  partial class DlgPrefDebugger
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
      this.checkDebuggerDeniseStepOverJMPBranches = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // checkDebuggerDeniseStepOverJMPBranches
      // 
      this.checkDebuggerDeniseStepOverJMPBranches.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkDebuggerDeniseStepOverJMPBranches.Location = new System.Drawing.Point(3, 6);
      this.checkDebuggerDeniseStepOverJMPBranches.Name = "checkDebuggerDeniseStepOverJMPBranches";
      this.checkDebuggerDeniseStepOverJMPBranches.Size = new System.Drawing.Size(336, 24);
      this.checkDebuggerDeniseStepOverJMPBranches.TabIndex = 17;
      this.checkDebuggerDeniseStepOverJMPBranches.Text = "Step Over steps over JMP/Branches (Denise only)";
      this.checkDebuggerDeniseStepOverJMPBranches.UseVisualStyleBackColor = true;
      this.checkDebuggerDeniseStepOverJMPBranches.CheckedChanged += new System.EventHandler(this.checkDebuggerDeniseStepOverJMPBranches_CheckedChanged);
      // 
      // DlgPrefDebugger
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.checkDebuggerDeniseStepOverJMPBranches);
      this.Name = "DlgPrefDebugger";
      this.Size = new System.Drawing.Size(489, 241);
      this.ResumeLayout(false);

    }

        #endregion
        private System.Windows.Forms.CheckBox checkDebuggerDeniseStepOverJMPBranches;
  }
}
