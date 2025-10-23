namespace RetroDevStudio.Dialogs.Preferences
{
  partial class DlgPrefBASICParser
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
      this.checkBASICAutoToggleEntryModeOnPosition = new System.Windows.Forms.CheckBox();
      this.checkBASICAutoToggleEntryMode = new System.Windows.Forms.CheckBox();
      this.checkBASICShowControlCodes = new System.Windows.Forms.CheckBox();
      this.checkBASICStripREM = new System.Windows.Forms.CheckBox();
      this.checkBASICStripSpaces = new System.Windows.Forms.CheckBox();
      this.checkBASICAlwaysMappedKeyMode = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // checkBASICAutoToggleEntryModeOnPosition
      // 
      this.checkBASICAutoToggleEntryModeOnPosition.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkBASICAutoToggleEntryModeOnPosition.Location = new System.Drawing.Point(3, 126);
      this.checkBASICAutoToggleEntryModeOnPosition.Name = "checkBASICAutoToggleEntryModeOnPosition";
      this.checkBASICAutoToggleEntryModeOnPosition.Size = new System.Drawing.Size(266, 24);
      this.checkBASICAutoToggleEntryModeOnPosition.TabIndex = 14;
      this.checkBASICAutoToggleEntryModeOnPosition.Text = "Auto toggle entry mode on position";
      this.checkBASICAutoToggleEntryModeOnPosition.UseVisualStyleBackColor = true;
      this.checkBASICAutoToggleEntryModeOnPosition.CheckedChanged += new System.EventHandler(this.checkBASICAutoToggleEntryModeOnPosition_CheckedChanged);
      // 
      // checkBASICAutoToggleEntryMode
      // 
      this.checkBASICAutoToggleEntryMode.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkBASICAutoToggleEntryMode.Location = new System.Drawing.Point(3, 96);
      this.checkBASICAutoToggleEntryMode.Name = "checkBASICAutoToggleEntryMode";
      this.checkBASICAutoToggleEntryMode.Size = new System.Drawing.Size(266, 24);
      this.checkBASICAutoToggleEntryMode.TabIndex = 14;
      this.checkBASICAutoToggleEntryMode.Text = "Auto toggle entry mode on quotes (\")";
      this.checkBASICAutoToggleEntryMode.UseVisualStyleBackColor = true;
      this.checkBASICAutoToggleEntryMode.CheckedChanged += new System.EventHandler(this.checkBASICAutoToggleEntryMode_CheckedChanged);
      // 
      // checkBASICShowControlCodes
      // 
      this.checkBASICShowControlCodes.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkBASICShowControlCodes.Location = new System.Drawing.Point(3, 66);
      this.checkBASICShowControlCodes.Name = "checkBASICShowControlCodes";
      this.checkBASICShowControlCodes.Size = new System.Drawing.Size(266, 24);
      this.checkBASICShowControlCodes.TabIndex = 15;
      this.checkBASICShowControlCodes.Text = "Show control codes as characters";
      this.checkBASICShowControlCodes.UseVisualStyleBackColor = true;
      this.checkBASICShowControlCodes.CheckedChanged += new System.EventHandler(this.checkBASICShowControlCodes_CheckedChanged);
      // 
      // checkBASICStripREM
      // 
      this.checkBASICStripREM.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkBASICStripREM.Location = new System.Drawing.Point(3, 36);
      this.checkBASICStripREM.Name = "checkBASICStripREM";
      this.checkBASICStripREM.Size = new System.Drawing.Size(266, 24);
      this.checkBASICStripREM.TabIndex = 16;
      this.checkBASICStripREM.Text = "Strip REM statements";
      this.checkBASICStripREM.UseVisualStyleBackColor = true;
      this.checkBASICStripREM.CheckedChanged += new System.EventHandler(this.checkBASICStripREM_CheckedChanged);
      // 
      // checkBASICStripSpaces
      // 
      this.checkBASICStripSpaces.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkBASICStripSpaces.Location = new System.Drawing.Point(3, 6);
      this.checkBASICStripSpaces.Name = "checkBASICStripSpaces";
      this.checkBASICStripSpaces.Size = new System.Drawing.Size(266, 24);
      this.checkBASICStripSpaces.TabIndex = 17;
      this.checkBASICStripSpaces.Text = "Strip spaces between code";
      this.checkBASICStripSpaces.UseVisualStyleBackColor = true;
      this.checkBASICStripSpaces.CheckedChanged += new System.EventHandler(this.checkBASICStripSpaces_CheckedChanged);
      // 
      // checkBASICAlwaysMappedKeyMode
      // 
      this.checkBASICAlwaysMappedKeyMode.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkBASICAlwaysMappedKeyMode.Location = new System.Drawing.Point(3, 156);
      this.checkBASICAlwaysMappedKeyMode.Name = "checkBASICAlwaysMappedKeyMode";
      this.checkBASICAlwaysMappedKeyMode.Size = new System.Drawing.Size(266, 24);
      this.checkBASICAlwaysMappedKeyMode.TabIndex = 14;
      this.checkBASICAlwaysMappedKeyMode.Text = "Use mapped keys also outside of strings";
      this.checkBASICAlwaysMappedKeyMode.UseVisualStyleBackColor = true;
      this.checkBASICAlwaysMappedKeyMode.CheckedChanged += new System.EventHandler(this.checkBASICAlwaysMappedKeyMode_CheckedChanged);
      // 
      // DlgPrefBASICParser
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.checkBASICAlwaysMappedKeyMode);
      this.Controls.Add(this.checkBASICAutoToggleEntryModeOnPosition);
      this.Controls.Add(this.checkBASICAutoToggleEntryMode);
      this.Controls.Add(this.checkBASICStripSpaces);
      this.Controls.Add(this.checkBASICShowControlCodes);
      this.Controls.Add(this.checkBASICStripREM);
      this.Name = "DlgPrefBASICParser";
      this.Size = new System.Drawing.Size(304, 241);
      this.ResumeLayout(false);

    }

        #endregion
        private System.Windows.Forms.CheckBox checkBASICAutoToggleEntryMode;
        private System.Windows.Forms.CheckBox checkBASICShowControlCodes;
        private System.Windows.Forms.CheckBox checkBASICStripREM;
        private System.Windows.Forms.CheckBox checkBASICStripSpaces;
    private System.Windows.Forms.CheckBox checkBASICAutoToggleEntryModeOnPosition;
    private System.Windows.Forms.CheckBox checkBASICAlwaysMappedKeyMode;
  }
}
