
namespace RetroDevStudio.Controls
{
  partial class ColorSettingsECMTED
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
      comboBGColor4 = new System.Windows.Forms.ComboBox();
      comboMulticolor2 = new System.Windows.Forms.ComboBox();
      comboBackground = new System.Windows.Forms.ComboBox();
      comboMulticolor1 = new System.Windows.Forms.ComboBox();
      comboCharColor = new System.Windows.Forms.ComboBox();
      label3 = new System.Windows.Forms.Label();
      label2 = new System.Windows.Forms.Label();
      label1 = new System.Windows.Forms.Label();
      label4 = new System.Windows.Forms.Label();
      label5 = new System.Windows.Forms.Label();
      SuspendLayout();
      // 
      // comboBGColor4
      // 
      comboBGColor4.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      comboBGColor4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      comboBGColor4.FormattingEnabled = true;
      comboBGColor4.Location = new System.Drawing.Point( 84, 92 );
      comboBGColor4.Name = "comboBGColor4";
      comboBGColor4.Size = new System.Drawing.Size( 71, 24 );
      comboBGColor4.TabIndex = 7;
      comboBGColor4.DrawItem +=  comboColor_DrawItem ;
      comboBGColor4.SelectedIndexChanged +=  comboBGColor4_SelectedIndexChanged ;
      // 
      // comboMulticolor2
      // 
      comboMulticolor2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      comboMulticolor2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      comboMulticolor2.FormattingEnabled = true;
      comboMulticolor2.Location = new System.Drawing.Point( 84, 65 );
      comboMulticolor2.Name = "comboMulticolor2";
      comboMulticolor2.Size = new System.Drawing.Size( 71, 24 );
      comboMulticolor2.TabIndex = 5;
      comboMulticolor2.DrawItem +=  comboColor_DrawItem ;
      comboMulticolor2.SelectedIndexChanged +=  comboMulticolor2_SelectedIndexChanged ;
      // 
      // comboBackground
      // 
      comboBackground.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      comboBackground.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      comboBackground.FormattingEnabled = true;
      comboBackground.Location = new System.Drawing.Point( 84, 11 );
      comboBackground.Name = "comboBackground";
      comboBackground.Size = new System.Drawing.Size( 71, 24 );
      comboBackground.TabIndex = 1;
      comboBackground.DrawItem +=  comboColor_DrawItem ;
      comboBackground.SelectedIndexChanged +=  comboBackground_SelectedIndexChanged ;
      // 
      // comboMulticolor1
      // 
      comboMulticolor1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      comboMulticolor1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      comboMulticolor1.FormattingEnabled = true;
      comboMulticolor1.Location = new System.Drawing.Point( 84, 38 );
      comboMulticolor1.Name = "comboMulticolor1";
      comboMulticolor1.Size = new System.Drawing.Size( 71, 24 );
      comboMulticolor1.TabIndex = 3;
      comboMulticolor1.DrawItem +=  comboColor_DrawItem ;
      comboMulticolor1.SelectedIndexChanged +=  comboMulticolor1_SelectedIndexChanged ;
      // 
      // comboCharColor
      // 
      comboCharColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      comboCharColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      comboCharColor.FormattingEnabled = true;
      comboCharColor.Location = new System.Drawing.Point( 84, 119 );
      comboCharColor.Name = "comboCharColor";
      comboCharColor.Size = new System.Drawing.Size( 71, 24 );
      comboCharColor.TabIndex = 9;
      comboCharColor.DrawItem +=  comboCharColor_DrawItem ;
      comboCharColor.SelectedIndexChanged +=  comboCharColor_SelectedIndexChanged ;
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new System.Drawing.Point( 4, 95 );
      label3.Name = "label3";
      label3.Size = new System.Drawing.Size( 63, 15 );
      label3.TabIndex = 11;
      label3.Text = "BGColor 4:";
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new System.Drawing.Point( 4, 68 );
      label2.Name = "label2";
      label2.Size = new System.Drawing.Size( 63, 15 );
      label2.TabIndex = 12;
      label2.Text = "BGColor 3:";
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new System.Drawing.Point( 4, 41 );
      label1.Name = "label1";
      label1.Size = new System.Drawing.Size( 63, 15 );
      label1.TabIndex = 13;
      label1.Text = "BGColor 2:";
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Location = new System.Drawing.Point( 4, 14 );
      label4.Name = "label4";
      label4.Size = new System.Drawing.Size( 74, 15 );
      label4.TabIndex = 13;
      label4.Text = "Background:";
      // 
      // label5
      // 
      label5.AutoSize = true;
      label5.Location = new System.Drawing.Point( 4, 122 );
      label5.Name = "label5";
      label5.Size = new System.Drawing.Size( 67, 15 );
      label5.TabIndex = 13;
      label5.Text = "Char Color:";
      // 
      // ColorSettingsECMTED
      // 
      AutoScaleDimensions = new System.Drawing.SizeF( 7F, 15F );
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      Controls.Add( label3 );
      Controls.Add( label2 );
      Controls.Add( label5 );
      Controls.Add( label4 );
      Controls.Add( label1 );
      Controls.Add( comboBGColor4 );
      Controls.Add( comboMulticolor2 );
      Controls.Add( comboBackground );
      Controls.Add( comboMulticolor1 );
      Controls.Add( comboCharColor );
      Name = "ColorSettingsECMTED";
      Size = new System.Drawing.Size( 168, 185 );
      ResumeLayout( false );
      PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ComboBox comboBackground;
    private System.Windows.Forms.ComboBox comboMulticolor1;
    private System.Windows.Forms.ComboBox comboCharColor;
    private System.Windows.Forms.ComboBox comboMulticolor2;
    private System.Windows.Forms.ComboBox comboBGColor4;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
  }
}
