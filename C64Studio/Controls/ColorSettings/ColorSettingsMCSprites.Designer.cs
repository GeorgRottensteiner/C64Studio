
namespace RetroDevStudio.Controls
{
  partial class ColorSettingsMCSprites
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
      components = new System.ComponentModel.Container();
      comboBackground = new System.Windows.Forms.ComboBox();
      comboMulticolor1 = new System.Windows.Forms.ComboBox();
      comboCustomColor = new System.Windows.Forms.ComboBox();
      btnExchangeColors = new DecentForms.MenuButton();
      comboMulticolor2 = new System.Windows.Forms.ComboBox();
      contextMenuExchangeColors = new System.Windows.Forms.ContextMenuStrip( components );
      exchangeMultiColor1WithMultiColor2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      exchangeMultiColor1WithBGColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      exchangeMultiColor2WithBGColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      exchangeCharColorWithBGColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      exchangeCustomColorWithMultiColor1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      exchangeCustomColorWithMultiColor2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      checkMulticolor = new System.Windows.Forms.CheckBox();
      label1 = new System.Windows.Forms.Label();
      labelMulticolor1 = new System.Windows.Forms.Label();
      labelMulticolor2 = new System.Windows.Forms.Label();
      label4 = new System.Windows.Forms.Label();
      contextMenuExchangeColors.SuspendLayout();
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
      // comboMulticolor1
      // 
      comboMulticolor1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      comboMulticolor1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      comboMulticolor1.Enabled = false;
      comboMulticolor1.FormattingEnabled = true;
      comboMulticolor1.Location = new System.Drawing.Point( 93, 38 );
      comboMulticolor1.Name = "comboMulticolor1";
      comboMulticolor1.Size = new System.Drawing.Size( 71, 24 );
      comboMulticolor1.TabIndex = 3;
      comboMulticolor1.DrawItem +=  comboColor_DrawItem ;
      comboMulticolor1.SelectedIndexChanged +=  comboMulticolor1_SelectedIndexChanged ;
      // 
      // comboCustomColor
      // 
      comboCustomColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      comboCustomColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      comboCustomColor.FormattingEnabled = true;
      comboCustomColor.Location = new System.Drawing.Point( 93, 92 );
      comboCustomColor.Name = "comboCustomColor";
      comboCustomColor.Size = new System.Drawing.Size( 71, 24 );
      comboCustomColor.TabIndex = 7;
      comboCustomColor.DrawItem +=  comboColor_DrawItem ;
      comboCustomColor.SelectedIndexChanged +=  comboColor_SelectedIndexChanged ;
      // 
      // btnExchangeColors
      // 
      btnExchangeColors.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnExchangeColors.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnExchangeColors.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnExchangeColors.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnExchangeColors.DisplayAntiAliased = true;
      btnExchangeColors.Image = null;
      btnExchangeColors.Location = new System.Drawing.Point( 3, 156 );
      btnExchangeColors.Name = "btnExchangeColors";
      btnExchangeColors.Size = new System.Drawing.Size( 161, 26 );
      btnExchangeColors.TabIndex = 9;
      btnExchangeColors.Text = "Exchange Colors";
      btnExchangeColors.Click +=  btnExchangeColors_Click ;
      // 
      // comboMulticolor2
      // 
      comboMulticolor2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      comboMulticolor2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      comboMulticolor2.Enabled = false;
      comboMulticolor2.FormattingEnabled = true;
      comboMulticolor2.Location = new System.Drawing.Point( 93, 65 );
      comboMulticolor2.Name = "comboMulticolor2";
      comboMulticolor2.Size = new System.Drawing.Size( 71, 24 );
      comboMulticolor2.TabIndex = 5;
      comboMulticolor2.DrawItem +=  comboColor_DrawItem ;
      comboMulticolor2.SelectedIndexChanged +=  comboMulticolor2_SelectedIndexChanged ;
      // 
      // contextMenuExchangeColors
      // 
      contextMenuExchangeColors.ImageScalingSize = new System.Drawing.Size( 28, 28 );
      contextMenuExchangeColors.Items.AddRange( new System.Windows.Forms.ToolStripItem[] { exchangeMultiColor1WithMultiColor2ToolStripMenuItem, exchangeMultiColor1WithBGColorToolStripMenuItem, exchangeMultiColor2WithBGColorToolStripMenuItem, exchangeCharColorWithBGColorToolStripMenuItem, exchangeCustomColorWithMultiColor1ToolStripMenuItem, exchangeCustomColorWithMultiColor2ToolStripMenuItem } );
      contextMenuExchangeColors.Name = "contextMenuExchangeColors";
      contextMenuExchangeColors.Size = new System.Drawing.Size( 300, 136 );
      // 
      // exchangeMultiColor1WithMultiColor2ToolStripMenuItem
      // 
      exchangeMultiColor1WithMultiColor2ToolStripMenuItem.Name = "exchangeMultiColor1WithMultiColor2ToolStripMenuItem";
      exchangeMultiColor1WithMultiColor2ToolStripMenuItem.Size = new System.Drawing.Size( 299, 22 );
      exchangeMultiColor1WithMultiColor2ToolStripMenuItem.Text = "Exchange Multi Color 1 with Multi Color 2";
      exchangeMultiColor1WithMultiColor2ToolStripMenuItem.Click +=  exchangeMultiColor1WithMultiColor2ToolStripMenuItem_Click ;
      // 
      // exchangeMultiColor1WithBGColorToolStripMenuItem
      // 
      exchangeMultiColor1WithBGColorToolStripMenuItem.Name = "exchangeMultiColor1WithBGColorToolStripMenuItem";
      exchangeMultiColor1WithBGColorToolStripMenuItem.Size = new System.Drawing.Size( 299, 22 );
      exchangeMultiColor1WithBGColorToolStripMenuItem.Text = "Exchange Multi Color 1 with BG Color";
      exchangeMultiColor1WithBGColorToolStripMenuItem.Click +=  exchangeMultiColor1WithBGColorToolStripMenuItem_Click ;
      // 
      // exchangeMultiColor2WithBGColorToolStripMenuItem
      // 
      exchangeMultiColor2WithBGColorToolStripMenuItem.Name = "exchangeMultiColor2WithBGColorToolStripMenuItem";
      exchangeMultiColor2WithBGColorToolStripMenuItem.Size = new System.Drawing.Size( 299, 22 );
      exchangeMultiColor2WithBGColorToolStripMenuItem.Text = "Exchange Multi Color 2 with BG Color";
      exchangeMultiColor2WithBGColorToolStripMenuItem.Click +=  exchangeMultiColor2WithBGColorToolStripMenuItem_Click ;
      // 
      // exchangeCharColorWithBGColorToolStripMenuItem
      // 
      exchangeCharColorWithBGColorToolStripMenuItem.Name = "exchangeCharColorWithBGColorToolStripMenuItem";
      exchangeCharColorWithBGColorToolStripMenuItem.Size = new System.Drawing.Size( 299, 22 );
      exchangeCharColorWithBGColorToolStripMenuItem.Text = "Exchange Custom Color with BG Color";
      exchangeCharColorWithBGColorToolStripMenuItem.Click +=  exchangeCharColorWithBGColorToolStripMenuItem_Click ;
      // 
      // exchangeCustomColorWithMultiColor1ToolStripMenuItem
      // 
      exchangeCustomColorWithMultiColor1ToolStripMenuItem.Name = "exchangeCustomColorWithMultiColor1ToolStripMenuItem";
      exchangeCustomColorWithMultiColor1ToolStripMenuItem.Size = new System.Drawing.Size( 299, 22 );
      exchangeCustomColorWithMultiColor1ToolStripMenuItem.Text = "Exchange Custom Color with Multi Color 1";
      exchangeCustomColorWithMultiColor1ToolStripMenuItem.Click +=  exchangeCustomColorWithMultiColor1ToolStripMenuItem_Click ;
      // 
      // exchangeCustomColorWithMultiColor2ToolStripMenuItem
      // 
      exchangeCustomColorWithMultiColor2ToolStripMenuItem.Name = "exchangeCustomColorWithMultiColor2ToolStripMenuItem";
      exchangeCustomColorWithMultiColor2ToolStripMenuItem.Size = new System.Drawing.Size( 299, 22 );
      exchangeCustomColorWithMultiColor2ToolStripMenuItem.Text = "Exchange Custom Color with Multi Color 2";
      exchangeCustomColorWithMultiColor2ToolStripMenuItem.Click +=  exchangeCustomColorWithMultiColor2ToolStripMenuItem_Click ;
      // 
      // checkMulticolor
      // 
      checkMulticolor.AutoSize = true;
      checkMulticolor.Location = new System.Drawing.Point( 3, 124 );
      checkMulticolor.Name = "checkMulticolor";
      checkMulticolor.Size = new System.Drawing.Size( 81, 19 );
      checkMulticolor.TabIndex = 8;
      checkMulticolor.Text = "Multicolor";
      checkMulticolor.UseVisualStyleBackColor = true;
      checkMulticolor.CheckedChanged +=  checkMulticolor_CheckedChanged ;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new System.Drawing.Point( 4, 14 );
      label1.Name = "label1";
      label1.Size = new System.Drawing.Size( 71, 15 );
      label1.TabIndex = 10;
      label1.Text = "Background";
      // 
      // labelMulticolor1
      // 
      labelMulticolor1.AutoSize = true;
      labelMulticolor1.Enabled = false;
      labelMulticolor1.Location = new System.Drawing.Point( 4, 41 );
      labelMulticolor1.Name = "labelMulticolor1";
      labelMulticolor1.Size = new System.Drawing.Size( 71, 15 );
      labelMulticolor1.TabIndex = 10;
      labelMulticolor1.Text = "Multicolor 1";
      // 
      // labelMulticolor2
      // 
      labelMulticolor2.AutoSize = true;
      labelMulticolor2.Enabled = false;
      labelMulticolor2.Location = new System.Drawing.Point( 4, 68 );
      labelMulticolor2.Name = "labelMulticolor2";
      labelMulticolor2.Size = new System.Drawing.Size( 71, 15 );
      labelMulticolor2.TabIndex = 10;
      labelMulticolor2.Text = "Multicolor 2";
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Location = new System.Drawing.Point( 4, 95 );
      label4.Name = "label4";
      label4.Size = new System.Drawing.Size( 81, 15 );
      label4.TabIndex = 10;
      label4.Text = "Custom Color";
      // 
      // ColorSettingsMCSprites
      // 
      AutoScaleDimensions = new System.Drawing.SizeF( 7F, 15F );
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      Controls.Add( label4 );
      Controls.Add( labelMulticolor2 );
      Controls.Add( labelMulticolor1 );
      Controls.Add( label1 );
      Controls.Add( checkMulticolor );
      Controls.Add( comboMulticolor2 );
      Controls.Add( btnExchangeColors );
      Controls.Add( comboBackground );
      Controls.Add( comboMulticolor1 );
      Controls.Add( comboCustomColor );
      Name = "ColorSettingsMCSprites";
      contextMenuExchangeColors.ResumeLayout( false );
      ResumeLayout( false );
      PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ComboBox comboBackground;
    private System.Windows.Forms.ComboBox comboMulticolor1;
    private System.Windows.Forms.ComboBox comboCustomColor;
    private DecentForms.MenuButton btnExchangeColors;
    private System.Windows.Forms.ComboBox comboMulticolor2;
    private System.Windows.Forms.ContextMenuStrip contextMenuExchangeColors;
    private System.Windows.Forms.ToolStripMenuItem exchangeMultiColor1WithMultiColor2ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exchangeMultiColor1WithBGColorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exchangeMultiColor2WithBGColorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exchangeCharColorWithBGColorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exchangeCustomColorWithMultiColor1ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exchangeCustomColorWithMultiColor2ToolStripMenuItem;
    private System.Windows.Forms.CheckBox checkMulticolor;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label labelMulticolor1;
    private System.Windows.Forms.Label labelMulticolor2;
    private System.Windows.Forms.Label label4;
  }
}
