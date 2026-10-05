namespace RetroDevStudio.Dialogs.Preferences
{
  partial class DlgPrefEditorBehaviour
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
      label1 = new System.Windows.Forms.Label();
      checkRightClickIsBGColor = new System.Windows.Forms.CheckBox();
      checkImageSelectionAsFrame = new System.Windows.Forms.CheckBox();
      label2 = new System.Windows.Forms.Label();
      SuspendLayout();
      // 
      // label1
      // 
      label1.Location = new System.Drawing.Point( 286, 12 );
      label1.Margin = new System.Windows.Forms.Padding( 4, 0, 4, 0 );
      label1.Name = "label1";
      label1.Size = new System.Drawing.Size( 406, 40 );
      label1.TabIndex = 15;
      label1.Text = "Toggles behaviour of right mouse button in drawing editors.  Per default right click picks the color under the cursor.";
      // 
      // checkRightClickIsBGColor
      // 
      checkRightClickIsBGColor.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      checkRightClickIsBGColor.Location = new System.Drawing.Point( 5, 6 );
      checkRightClickIsBGColor.Margin = new System.Windows.Forms.Padding( 4, 3, 4, 3 );
      checkRightClickIsBGColor.Name = "checkRightClickIsBGColor";
      checkRightClickIsBGColor.Size = new System.Drawing.Size( 240, 28 );
      checkRightClickIsBGColor.TabIndex = 14;
      checkRightClickIsBGColor.Text = "Right Click is Paint with BG Color";
      checkRightClickIsBGColor.UseVisualStyleBackColor = true;
      checkRightClickIsBGColor.CheckedChanged +=  checkRightClickIsBGColor_CheckedChanged ;
      // 
      // checkImageSelectionAsFrame
      // 
      checkImageSelectionAsFrame.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      checkImageSelectionAsFrame.Location = new System.Drawing.Point( 5, 40 );
      checkImageSelectionAsFrame.Margin = new System.Windows.Forms.Padding( 4, 3, 4, 3 );
      checkImageSelectionAsFrame.Name = "checkImageSelectionAsFrame";
      checkImageSelectionAsFrame.Size = new System.Drawing.Size( 240, 28 );
      checkImageSelectionAsFrame.TabIndex = 14;
      checkImageSelectionAsFrame.Text = "Show Image Selection as Frame";
      checkImageSelectionAsFrame.UseVisualStyleBackColor = true;
      checkImageSelectionAsFrame.CheckedChanged +=  checkImageSelectionAsFrame_CheckedChanged ;
      // 
      // label2
      // 
      label2.Location = new System.Drawing.Point( 286, 46 );
      label2.Margin = new System.Windows.Forms.Padding( 4, 0, 4, 0 );
      label2.Name = "label2";
      label2.Size = new System.Drawing.Size( 406, 40 );
      label2.TabIndex = 15;
      label2.Text = "Affects the appearance of selected items in image lists (e.g sprites, chars, etc.) as frame instead of an overlayed quad";
      // 
      // DlgPrefEditorBehaviour
      // 
      AutoScaleDimensions = new System.Drawing.SizeF( 7F, 15F );
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      Controls.Add( label2 );
      Controls.Add( label1 );
      Controls.Add( checkImageSelectionAsFrame );
      Controls.Add( checkRightClickIsBGColor );
      Margin = new System.Windows.Forms.Padding( 5, 3, 5, 3 );
      Name = "DlgPrefEditorBehaviour";
      Size = new System.Drawing.Size( 730, 110 );
      ResumeLayout( false );

    }

    #endregion
    private System.Windows.Forms.CheckBox checkRightClickIsBGColor;
        private System.Windows.Forms.Label label1;
    private System.Windows.Forms.CheckBox checkImageSelectionAsFrame;
    private System.Windows.Forms.Label label2;
  }
}
