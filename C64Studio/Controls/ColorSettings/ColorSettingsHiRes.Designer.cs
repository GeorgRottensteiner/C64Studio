
namespace RetroDevStudio.Controls
{
  partial class ColorSettingsHiRes
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
      comboBackground = new System.Windows.Forms.ComboBox();
      comboCharColor = new System.Windows.Forms.ComboBox();
      label1 = new System.Windows.Forms.Label();
      label2 = new System.Windows.Forms.Label();
      SuspendLayout();
      // 
      // comboBackground
      // 
      comboBackground.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      comboBackground.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      comboBackground.FormattingEnabled = true;
      comboBackground.Location = new System.Drawing.Point( 93, 11 );
      comboBackground.Name = "comboBackground";
      comboBackground.Size = new System.Drawing.Size( 71, 24 );
      comboBackground.TabIndex = 1;
      comboBackground.DrawItem +=  comboColor_DrawItem ;
      comboBackground.SelectedIndexChanged +=  comboBackground_SelectedIndexChanged ;
      // 
      // comboCharColor
      // 
      comboCharColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      comboCharColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      comboCharColor.FormattingEnabled = true;
      comboCharColor.Location = new System.Drawing.Point( 93, 38 );
      comboCharColor.Name = "comboCharColor";
      comboCharColor.Size = new System.Drawing.Size( 71, 24 );
      comboCharColor.TabIndex = 3;
      comboCharColor.DrawItem +=  comboCharColor_DrawItem ;
      comboCharColor.SelectedIndexChanged +=  comboCharColor_SelectedIndexChanged ;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new System.Drawing.Point( 3, 14 );
      label1.Name = "label1";
      label1.Size = new System.Drawing.Size( 74, 15 );
      label1.TabIndex = 4;
      label1.Text = "Background:";
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new System.Drawing.Point( 3, 41 );
      label2.Name = "label2";
      label2.Size = new System.Drawing.Size( 67, 15 );
      label2.TabIndex = 4;
      label2.Text = "Char Color:";
      // 
      // ColorSettingsHiRes
      // 
      AutoScaleDimensions = new System.Drawing.SizeF( 7F, 15F );
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      Controls.Add( label2 );
      Controls.Add( label1 );
      Controls.Add( comboBackground );
      Controls.Add( comboCharColor );
      Name = "ColorSettingsHiRes";
      ResumeLayout( false );
      PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ComboBox comboBackground;
    private System.Windows.Forms.ComboBox comboCharColor;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
  }
}
