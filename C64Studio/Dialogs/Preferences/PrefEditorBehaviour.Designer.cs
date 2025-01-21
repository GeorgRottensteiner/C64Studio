namespace RetroDevStudio.Dialogs.Preferences
{
  partial class PrefEditorBehaviour
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
      this.label1 = new System.Windows.Forms.Label();
      this.checkRightClickIsBGColor = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(245, 10);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(348, 72);
      this.label1.TabIndex = 15;
      this.label1.Text = "Toggles behaviour of right mouse button in drawing editors.  Per default right cl" +
    "ick picks the color under the cursor.";
      // 
      // checkRightClickIsBGColor
      // 
      this.checkRightClickIsBGColor.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkRightClickIsBGColor.Location = new System.Drawing.Point(4, 5);
      this.checkRightClickIsBGColor.Name = "checkRightClickIsBGColor";
      this.checkRightClickIsBGColor.Size = new System.Drawing.Size(206, 24);
      this.checkRightClickIsBGColor.TabIndex = 14;
      this.checkRightClickIsBGColor.Text = "Right Click is Paint with BG Color";
      this.checkRightClickIsBGColor.UseVisualStyleBackColor = true;
      this.checkRightClickIsBGColor.CheckedChanged += new System.EventHandler(this.checkRightClickIsBGColor_CheckedChanged);
      // 
      // PrefEditorBehaviour
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.label1);
      this.Controls.Add(this.checkRightClickIsBGColor);
      this.Name = "PrefEditorBehaviour";
      this.Size = new System.Drawing.Size(626, 95);
      this.ResumeLayout(false);

    }

        #endregion
        private System.Windows.Forms.CheckBox checkRightClickIsBGColor;
        private System.Windows.Forms.Label label1;
    }
}
