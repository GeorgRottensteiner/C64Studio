
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
      this.components = new System.ComponentModel.Container();
      this.comboBackground = new System.Windows.Forms.ComboBox();
      this.comboBorderColor = new System.Windows.Forms.ComboBox();
      this.comboCharColor = new System.Windows.Forms.ComboBox();
      this.radioBackground = new System.Windows.Forms.RadioButton();
      this.radioBorderColor = new System.Windows.Forms.RadioButton();
      this.radioAuxColor = new System.Windows.Forms.RadioButton();
      this.radioCharColor = new System.Windows.Forms.RadioButton();
      this.btnExchangeColors = new DecentForms.MenuButton();
      this.comboAuxColor = new System.Windows.Forms.ComboBox();
      this.contextMenuExchangeColors = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exchangeMultiColor1WithBGColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exchangeMultiColor2WithBGColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.contextMenuExchangeColors.SuspendLayout();
      this.SuspendLayout();
      // 
      // comboBackground
      // 
      this.comboBackground.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboBackground.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBackground.FormattingEnabled = true;
      this.comboBackground.Location = new System.Drawing.Point(93, 11);
      this.comboBackground.Name = "comboBackground";
      this.comboBackground.Size = new System.Drawing.Size(71, 21);
      this.comboBackground.TabIndex = 34;
      this.comboBackground.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboColor_DrawItem);
      this.comboBackground.SelectedIndexChanged += new System.EventHandler(this.comboBackground_SelectedIndexChanged);
      // 
      // comboMulticolor1
      // 
      this.comboBorderColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboBorderColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBorderColor.FormattingEnabled = true;
      this.comboBorderColor.Location = new System.Drawing.Point(93, 38);
      this.comboBorderColor.Name = "comboMulticolor1";
      this.comboBorderColor.Size = new System.Drawing.Size(71, 21);
      this.comboBorderColor.TabIndex = 31;
      this.comboBorderColor.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboColor_DrawItem);
      this.comboBorderColor.SelectedIndexChanged += new System.EventHandler(this.comboMulticolor1_SelectedIndexChanged);
      // 
      // comboCharColor
      // 
      this.comboCharColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboCharColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboCharColor.FormattingEnabled = true;
      this.comboCharColor.Location = new System.Drawing.Point(93, 92);
      this.comboCharColor.Name = "comboCharColor";
      this.comboCharColor.Size = new System.Drawing.Size(71, 21);
      this.comboCharColor.TabIndex = 33;
      this.comboCharColor.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboCharColor_DrawItem);
      this.comboCharColor.SelectedIndexChanged += new System.EventHandler(this.comboCharColor_SelectedIndexChanged);
      // 
      // radioBackground
      // 
      this.radioBackground.AutoSize = true;
      this.radioBackground.Location = new System.Drawing.Point(4, 12);
      this.radioBackground.Name = "radioBackground";
      this.radioBackground.Size = new System.Drawing.Size(83, 17);
      this.radioBackground.TabIndex = 35;
      this.radioBackground.Text = "Background";
      this.radioBackground.UseVisualStyleBackColor = true;
      this.radioBackground.CheckedChanged += new System.EventHandler(this.radioBackground_CheckedChanged);
      // 
      // radioMultiColor1
      // 
      this.radioBorderColor.AutoSize = true;
      this.radioBorderColor.Location = new System.Drawing.Point(4, 39);
      this.radioBorderColor.Name = "radioMultiColor1";
      this.radioBorderColor.Size = new System.Drawing.Size(83, 17);
      this.radioBorderColor.TabIndex = 36;
      this.radioBorderColor.Text = "Border Color";
      this.radioBorderColor.UseVisualStyleBackColor = true;
      this.radioBorderColor.CheckedChanged += new System.EventHandler(this.radioMultiColor1_CheckedChanged);
      // 
      // radioMulticolor2
      // 
      this.radioAuxColor.AutoSize = true;
      this.radioAuxColor.Location = new System.Drawing.Point(4, 65);
      this.radioAuxColor.Name = "radioMulticolor2";
      this.radioAuxColor.Size = new System.Drawing.Size(73, 17);
      this.radioAuxColor.TabIndex = 37;
      this.radioAuxColor.Text = "Aux. Color";
      this.radioAuxColor.UseVisualStyleBackColor = true;
      this.radioAuxColor.CheckedChanged += new System.EventHandler(this.radioMulticolor2_CheckedChanged);
      // 
      // radioCharColor
      // 
      this.radioCharColor.AutoSize = true;
      this.radioCharColor.Checked = true;
      this.radioCharColor.Location = new System.Drawing.Point(4, 92);
      this.radioCharColor.Name = "radioCharColor";
      this.radioCharColor.Size = new System.Drawing.Size(74, 17);
      this.radioCharColor.TabIndex = 39;
      this.radioCharColor.TabStop = true;
      this.radioCharColor.Text = "Char Color";
      this.radioCharColor.UseVisualStyleBackColor = true;
      this.radioCharColor.CheckedChanged += new System.EventHandler(this.radioCharColor_CheckedChanged);
      // 
      // btnExchangeColors
      // 
      this.btnExchangeColors.Location = new System.Drawing.Point(3, 156);
      this.btnExchangeColors.Name = "btnExchangeColors";
      this.btnExchangeColors.Size = new System.Drawing.Size(161, 26);
      this.btnExchangeColors.TabIndex = 50;
      this.btnExchangeColors.Text = "Exchange Colors";
      this.btnExchangeColors.Click += new DecentForms.EventHandler(this.btnExchangeColors_Click);
      // 
      // comboMulticolor2
      // 
      this.comboAuxColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboAuxColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboAuxColor.FormattingEnabled = true;
      this.comboAuxColor.Location = new System.Drawing.Point(93, 65);
      this.comboAuxColor.Name = "comboMulticolor2";
      this.comboAuxColor.Size = new System.Drawing.Size(71, 21);
      this.comboAuxColor.TabIndex = 51;
      this.comboAuxColor.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboColor_DrawItem);
      this.comboAuxColor.SelectedIndexChanged += new System.EventHandler(this.comboMulticolor2_SelectedIndexChanged);
      // 
      // contextMenuExchangeColors
      // 
      this.contextMenuExchangeColors.ImageScalingSize = new System.Drawing.Size(28, 28);
      this.contextMenuExchangeColors.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem,
            this.exchangeMultiColor1WithBGColorToolStripMenuItem,
            this.exchangeMultiColor2WithBGColorToolStripMenuItem});
      this.contextMenuExchangeColors.Name = "contextMenuExchangeColors";
      this.contextMenuExchangeColors.Size = new System.Drawing.Size(281, 92);
      // 
      // exchangeMultiColor1WithMultiColor2ToolStripMenuItem
      // 
      this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem.Name = "exchangeMultiColor1WithMultiColor2ToolStripMenuItem";
      this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem.Size = new System.Drawing.Size(280, 22);
      this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem.Text = "Exchange Border Color with Aux. Color";
      this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem.Click += new System.EventHandler(this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem_Click);
      // 
      // exchangeMultiColor1WithBGColorToolStripMenuItem
      // 
      this.exchangeMultiColor1WithBGColorToolStripMenuItem.Name = "exchangeMultiColor1WithBGColorToolStripMenuItem";
      this.exchangeMultiColor1WithBGColorToolStripMenuItem.Size = new System.Drawing.Size(280, 22);
      this.exchangeMultiColor1WithBGColorToolStripMenuItem.Text = "Exchange Border Color with BG Color";
      this.exchangeMultiColor1WithBGColorToolStripMenuItem.Click += new System.EventHandler(this.exchangeMultiColor1WithBGColorToolStripMenuItem_Click);
      // 
      // exchangeMultiColor2WithBGColorToolStripMenuItem
      // 
      this.exchangeMultiColor2WithBGColorToolStripMenuItem.Name = "exchangeMultiColor2WithBGColorToolStripMenuItem";
      this.exchangeMultiColor2WithBGColorToolStripMenuItem.Size = new System.Drawing.Size(280, 22);
      this.exchangeMultiColor2WithBGColorToolStripMenuItem.Text = "Exchange Aux. Color with BG Color";
      this.exchangeMultiColor2WithBGColorToolStripMenuItem.Click += new System.EventHandler(this.exchangeMultiColor2WithBGColorToolStripMenuItem_Click);
      // 
      // ColorSettingsVC20
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.comboAuxColor);
      this.Controls.Add(this.btnExchangeColors);
      this.Controls.Add(this.comboBackground);
      this.Controls.Add(this.comboBorderColor);
      this.Controls.Add(this.comboCharColor);
      this.Controls.Add(this.radioBackground);
      this.Controls.Add(this.radioBorderColor);
      this.Controls.Add(this.radioAuxColor);
      this.Controls.Add(this.radioCharColor);
      this.Name = "ColorSettingsVC20";
      this.contextMenuExchangeColors.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ComboBox comboBackground;
    private System.Windows.Forms.ComboBox comboBorderColor;
    private System.Windows.Forms.ComboBox comboCharColor;
    private System.Windows.Forms.RadioButton radioBackground;
    private System.Windows.Forms.RadioButton radioBorderColor;
    private System.Windows.Forms.RadioButton radioAuxColor;
    private System.Windows.Forms.RadioButton radioCharColor;
    private DecentForms.MenuButton btnExchangeColors;
    private System.Windows.Forms.ComboBox comboAuxColor;
    private System.Windows.Forms.ContextMenuStrip contextMenuExchangeColors;
    private System.Windows.Forms.ToolStripMenuItem exchangeMultiColor1WithMultiColor2ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exchangeMultiColor1WithBGColorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exchangeMultiColor2WithBGColorToolStripMenuItem;
  }
}
