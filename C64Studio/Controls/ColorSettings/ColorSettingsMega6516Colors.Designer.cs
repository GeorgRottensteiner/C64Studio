
namespace RetroDevStudio.Controls
{
  partial class ColorSettingsMega6516Colors
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
      btnEditPalette = new DecentForms.Button();
      comboActivePalette = new System.Windows.Forms.ComboBox();
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
      // btnEditPalette
      // 
      btnEditPalette.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnEditPalette.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnEditPalette.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnEditPalette.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnEditPalette.DisplayAntiAliased = true;
      btnEditPalette.Image = null;
      btnEditPalette.Location = new System.Drawing.Point( 3, 41 );
      btnEditPalette.Name = "btnEditPalette";
      btnEditPalette.Size = new System.Drawing.Size( 161, 26 );
      btnEditPalette.TabIndex = 4;
      btnEditPalette.Text = "Edit Palette";
      btnEditPalette.Click +=  btnEditPalette_Click ;
      // 
      // comboActivePalette
      // 
      comboActivePalette.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      comboActivePalette.FormattingEnabled = true;
      comboActivePalette.Location = new System.Drawing.Point( 54, 73 );
      comboActivePalette.Name = "comboActivePalette";
      comboActivePalette.Size = new System.Drawing.Size( 110, 23 );
      comboActivePalette.TabIndex = 5;
      comboActivePalette.SelectedIndexChanged +=  comboActivePalette_SelectedIndexChanged ;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new System.Drawing.Point( 3, 76 );
      label1.Name = "label1";
      label1.Size = new System.Drawing.Size( 46, 15 );
      label1.TabIndex = 58;
      label1.Text = "Palette:";
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new System.Drawing.Point( 3, 14 );
      label2.Name = "label2";
      label2.Size = new System.Drawing.Size( 71, 15 );
      label2.TabIndex = 60;
      label2.Text = "Background";
      // 
      // ColorSettingsMega6516Colors
      // 
      AutoScaleDimensions = new System.Drawing.SizeF( 7F, 15F );
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      Controls.Add( label2 );
      Controls.Add( label1 );
      Controls.Add( comboActivePalette );
      Controls.Add( btnEditPalette );
      Controls.Add( comboBackground );
      Name = "ColorSettingsMega6516Colors";
      ResumeLayout( false );
      PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ComboBox comboBackground;
    private DecentForms.Button btnEditPalette;
    private System.Windows.Forms.ComboBox comboActivePalette;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
  }
}
