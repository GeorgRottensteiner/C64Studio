
namespace RetroDevStudio.Controls
{
  partial class ColorSettingsX16Sprites16
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
      comboPaletteOffset = new System.Windows.Forms.ComboBox();
      label2 = new System.Windows.Forms.Label();
      label3 = new System.Windows.Forms.Label();
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
      btnEditPalette.Location = new System.Drawing.Point( 3, 70 );
      btnEditPalette.Name = "btnEditPalette";
      btnEditPalette.Size = new System.Drawing.Size( 161, 26 );
      btnEditPalette.TabIndex = 6;
      btnEditPalette.Text = "Edit Palette";
      btnEditPalette.Click +=  btnEditPalette_Click ;
      // 
      // comboActivePalette
      // 
      comboActivePalette.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      comboActivePalette.FormattingEnabled = true;
      comboActivePalette.Location = new System.Drawing.Point( 54, 102 );
      comboActivePalette.Name = "comboActivePalette";
      comboActivePalette.Size = new System.Drawing.Size( 110, 23 );
      comboActivePalette.TabIndex = 7;
      comboActivePalette.SelectedIndexChanged +=  comboActivePalette_SelectedIndexChanged ;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new System.Drawing.Point( 3, 105 );
      label1.Name = "label1";
      label1.Size = new System.Drawing.Size( 46, 15 );
      label1.TabIndex = 58;
      label1.Text = "Palette:";
      // 
      // comboPaletteOffset
      // 
      comboPaletteOffset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      comboPaletteOffset.FormattingEnabled = true;
      comboPaletteOffset.Location = new System.Drawing.Point( 93, 41 );
      comboPaletteOffset.Name = "comboPaletteOffset";
      comboPaletteOffset.Size = new System.Drawing.Size( 71, 23 );
      comboPaletteOffset.TabIndex = 5;
      comboPaletteOffset.SelectedIndexChanged +=  comboPaletteOffset_SelectedIndexChanged ;
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new System.Drawing.Point( 3, 44 );
      label2.Name = "label2";
      label2.Size = new System.Drawing.Size( 81, 15 );
      label2.TabIndex = 4;
      label2.Text = "Palette Offset:";
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new System.Drawing.Point( 3, 14 );
      label3.Name = "label3";
      label3.Size = new System.Drawing.Size( 74, 15 );
      label3.TabIndex = 4;
      label3.Text = "Background:";
      // 
      // ColorSettingsX16Sprites16
      // 
      AutoScaleDimensions = new System.Drawing.SizeF( 7F, 15F );
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      Controls.Add( label3 );
      Controls.Add( label2 );
      Controls.Add( label1 );
      Controls.Add( comboActivePalette );
      Controls.Add( btnEditPalette );
      Controls.Add( comboBackground );
      Controls.Add( comboPaletteOffset );
      Name = "ColorSettingsX16Sprites16";
      ResumeLayout( false );
      PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ComboBox comboBackground;
    private DecentForms.Button btnEditPalette;
    private System.Windows.Forms.ComboBox comboActivePalette;
    private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboPaletteOffset;
        private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
  }
}
