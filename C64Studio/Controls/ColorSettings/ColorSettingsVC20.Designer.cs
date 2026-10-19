
namespace RetroDevStudio.Controls
{
  partial class ColorSettingsVC20
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
      comboBorderColor = new System.Windows.Forms.ComboBox();
      comboCharColor = new System.Windows.Forms.ComboBox();
      btnExchangeColors = new DecentForms.MenuButton();
      comboAuxColor = new System.Windows.Forms.ComboBox();
      contextMenuExchangeColors = new System.Windows.Forms.ContextMenuStrip( components );
      exchangeMultiColor1WithMultiColor2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      exchangeMultiColor1WithBGColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      exchangeMultiColor2WithBGColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      label2 = new System.Windows.Forms.Label();
      label3 = new System.Windows.Forms.Label();
      label1 = new System.Windows.Forms.Label();
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
      comboBackground.Size = new System.Drawing.Size( 80, 24 );
      comboBackground.TabIndex = 1;
      comboBackground.DrawItem +=  comboColor_DrawItem ;
      comboBackground.SelectedIndexChanged +=  comboBackground_SelectedIndexChanged ;
      // 
      // comboBorderColor
      // 
      comboBorderColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      comboBorderColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      comboBorderColor.FormattingEnabled = true;
      comboBorderColor.Location = new System.Drawing.Point( 93, 38 );
      comboBorderColor.Name = "comboBorderColor";
      comboBorderColor.Size = new System.Drawing.Size( 80, 24 );
      comboBorderColor.TabIndex = 3;
      comboBorderColor.DrawItem +=  comboColor_DrawItem ;
      comboBorderColor.SelectedIndexChanged +=  comboMulticolor1_SelectedIndexChanged ;
      // 
      // comboCharColor
      // 
      comboCharColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      comboCharColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      comboCharColor.FormattingEnabled = true;
      comboCharColor.Location = new System.Drawing.Point( 93, 92 );
      comboCharColor.Name = "comboCharColor";
      comboCharColor.Size = new System.Drawing.Size( 80, 24 );
      comboCharColor.TabIndex = 7;
      comboCharColor.DrawItem +=  comboCharColor_DrawItem ;
      comboCharColor.SelectedIndexChanged +=  comboCharColor_SelectedIndexChanged ;
      // 
      // btnExchangeColors
      // 
      btnExchangeColors.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnExchangeColors.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnExchangeColors.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnExchangeColors.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnExchangeColors.DisplayAntiAliased = true;
      btnExchangeColors.Image = null;
      btnExchangeColors.Location = new System.Drawing.Point( 3, 119 );
      btnExchangeColors.Name = "btnExchangeColors";
      btnExchangeColors.Size = new System.Drawing.Size( 170, 26 );
      btnExchangeColors.TabIndex = 8;
      btnExchangeColors.Text = "Exchange Colors";
      btnExchangeColors.Click +=  btnExchangeColors_Click ;
      // 
      // comboAuxColor
      // 
      comboAuxColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      comboAuxColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      comboAuxColor.FormattingEnabled = true;
      comboAuxColor.Location = new System.Drawing.Point( 93, 65 );
      comboAuxColor.Name = "comboAuxColor";
      comboAuxColor.Size = new System.Drawing.Size( 80, 24 );
      comboAuxColor.TabIndex = 5;
      comboAuxColor.DrawItem +=  comboColor_DrawItem ;
      comboAuxColor.SelectedIndexChanged +=  comboMulticolor2_SelectedIndexChanged ;
      // 
      // contextMenuExchangeColors
      // 
      contextMenuExchangeColors.ImageScalingSize = new System.Drawing.Size( 28, 28 );
      contextMenuExchangeColors.Items.AddRange( new System.Windows.Forms.ToolStripItem[] { exchangeMultiColor1WithMultiColor2ToolStripMenuItem, exchangeMultiColor1WithBGColorToolStripMenuItem, exchangeMultiColor2WithBGColorToolStripMenuItem } );
      contextMenuExchangeColors.Name = "contextMenuExchangeColors";
      contextMenuExchangeColors.Size = new System.Drawing.Size( 279, 70 );
      // 
      // exchangeMultiColor1WithMultiColor2ToolStripMenuItem
      // 
      exchangeMultiColor1WithMultiColor2ToolStripMenuItem.Name = "exchangeMultiColor1WithMultiColor2ToolStripMenuItem";
      exchangeMultiColor1WithMultiColor2ToolStripMenuItem.Size = new System.Drawing.Size( 278, 22 );
      exchangeMultiColor1WithMultiColor2ToolStripMenuItem.Text = "Exchange Border Color with Aux. Color";
      exchangeMultiColor1WithMultiColor2ToolStripMenuItem.Click +=  exchangeMultiColor1WithMultiColor2ToolStripMenuItem_Click ;
      // 
      // exchangeMultiColor1WithBGColorToolStripMenuItem
      // 
      exchangeMultiColor1WithBGColorToolStripMenuItem.Name = "exchangeMultiColor1WithBGColorToolStripMenuItem";
      exchangeMultiColor1WithBGColorToolStripMenuItem.Size = new System.Drawing.Size( 278, 22 );
      exchangeMultiColor1WithBGColorToolStripMenuItem.Text = "Exchange Border Color with BG Color";
      exchangeMultiColor1WithBGColorToolStripMenuItem.Click +=  exchangeMultiColor1WithBGColorToolStripMenuItem_Click ;
      // 
      // exchangeMultiColor2WithBGColorToolStripMenuItem
      // 
      exchangeMultiColor2WithBGColorToolStripMenuItem.Name = "exchangeMultiColor2WithBGColorToolStripMenuItem";
      exchangeMultiColor2WithBGColorToolStripMenuItem.Size = new System.Drawing.Size( 278, 22 );
      exchangeMultiColor2WithBGColorToolStripMenuItem.Text = "Exchange Aux. Color with BG Color";
      exchangeMultiColor2WithBGColorToolStripMenuItem.Click +=  exchangeMultiColor2WithBGColorToolStripMenuItem_Click ;
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new System.Drawing.Point( 4, 95 );
      label2.Name = "label2";
      label2.Size = new System.Drawing.Size( 67, 15 );
      label2.TabIndex = 17;
      label2.Text = "Char Color:";
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new System.Drawing.Point( 4, 14 );
      label3.Name = "label3";
      label3.Size = new System.Drawing.Size( 74, 15 );
      label3.TabIndex = 18;
      label3.Text = "Background:";
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new System.Drawing.Point( 4, 41 );
      label1.Name = "label1";
      label1.Size = new System.Drawing.Size( 77, 15 );
      label1.TabIndex = 18;
      label1.Text = "Border Color:";
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Location = new System.Drawing.Point( 4, 68 );
      label4.Name = "label4";
      label4.Size = new System.Drawing.Size( 65, 15 );
      label4.TabIndex = 18;
      label4.Text = "Aux. Color:";
      // 
      // ColorSettingsVC20
      // 
      AutoScaleDimensions = new System.Drawing.SizeF( 7F, 15F );
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      Controls.Add( label2 );
      Controls.Add( label4 );
      Controls.Add( label1 );
      Controls.Add( label3 );
      Controls.Add( comboAuxColor );
      Controls.Add( btnExchangeColors );
      Controls.Add( comboBackground );
      Controls.Add( comboBorderColor );
      Controls.Add( comboCharColor );
      Name = "ColorSettingsVC20";
      contextMenuExchangeColors.ResumeLayout( false );
      ResumeLayout( false );
      PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ComboBox comboBackground;
    private System.Windows.Forms.ComboBox comboBorderColor;
    private System.Windows.Forms.ComboBox comboCharColor;
    private DecentForms.MenuButton btnExchangeColors;
    private System.Windows.Forms.ComboBox comboAuxColor;
    private System.Windows.Forms.ContextMenuStrip contextMenuExchangeColors;
    private System.Windows.Forms.ToolStripMenuItem exchangeMultiColor1WithMultiColor2ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exchangeMultiColor1WithBGColorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exchangeMultiColor2WithBGColorToolStripMenuItem;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label4;
  }
}
