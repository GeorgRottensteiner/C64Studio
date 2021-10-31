
namespace C64Studio.Controls
{
  partial class ColorSettingsMC
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
      this.comboMulticolor1 = new System.Windows.Forms.ComboBox();
      this.comboCharColor = new System.Windows.Forms.ComboBox();
      this.radioBackground = new System.Windows.Forms.RadioButton();
      this.radioMultiColor1 = new System.Windows.Forms.RadioButton();
      this.radioMulticolor2 = new System.Windows.Forms.RadioButton();
      this.radioCharColor = new System.Windows.Forms.RadioButton();
      this.btnExchangeColors = new C64Studio.Controls.MenuButton();
      this.comboMulticolor2 = new System.Windows.Forms.ComboBox();
      this.contextMenuExchangeColors = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exchangeMultiColor1WithBGColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exchangeMultiColor2WithBGColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exchangeCharColorWithBGColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exchangeCustomColorWithMultiColor1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exchangeCustomColorWithMultiColor2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
      this.comboMulticolor1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboMulticolor1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboMulticolor1.FormattingEnabled = true;
      this.comboMulticolor1.Location = new System.Drawing.Point(93, 38);
      this.comboMulticolor1.Name = "comboMulticolor1";
      this.comboMulticolor1.Size = new System.Drawing.Size(71, 21);
      this.comboMulticolor1.TabIndex = 31;
      this.comboMulticolor1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboColor_DrawItem);
      this.comboMulticolor1.SelectedIndexChanged += new System.EventHandler(this.comboMulticolor1_SelectedIndexChanged);
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
      this.radioBackground.TabStop = true;
      this.radioBackground.Text = "Background";
      this.radioBackground.UseVisualStyleBackColor = true;
      this.radioBackground.CheckedChanged += new System.EventHandler(this.radioBackground_CheckedChanged);
      // 
      // radioMultiColor1
      // 
      this.radioMultiColor1.AutoSize = true;
      this.radioMultiColor1.Location = new System.Drawing.Point(4, 39);
      this.radioMultiColor1.Name = "radioMultiColor1";
      this.radioMultiColor1.Size = new System.Drawing.Size(79, 17);
      this.radioMultiColor1.TabIndex = 36;
      this.radioMultiColor1.TabStop = true;
      this.radioMultiColor1.Text = "Multicolor 1";
      this.radioMultiColor1.UseVisualStyleBackColor = true;
      this.radioMultiColor1.CheckedChanged += new System.EventHandler(this.radioMultiColor1_CheckedChanged);
      // 
      // radioMulticolor2
      // 
      this.radioMulticolor2.AutoSize = true;
      this.radioMulticolor2.Location = new System.Drawing.Point(4, 65);
      this.radioMulticolor2.Name = "radioMulticolor2";
      this.radioMulticolor2.Size = new System.Drawing.Size(79, 17);
      this.radioMulticolor2.TabIndex = 37;
      this.radioMulticolor2.TabStop = true;
      this.radioMulticolor2.Text = "Multicolor 2";
      this.radioMulticolor2.UseVisualStyleBackColor = true;
      this.radioMulticolor2.CheckedChanged += new System.EventHandler(this.radioMulticolor2_CheckedChanged);
      // 
      // radioCharColor
      // 
      this.radioCharColor.AutoSize = true;
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
      this.btnExchangeColors.UseVisualStyleBackColor = true;
      this.btnExchangeColors.Click += new System.EventHandler(this.btnExchangeColors_Click);
      // 
      // comboMulticolor2
      // 
      this.comboMulticolor2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboMulticolor2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboMulticolor2.FormattingEnabled = true;
      this.comboMulticolor2.Location = new System.Drawing.Point(93, 65);
      this.comboMulticolor2.Name = "comboMulticolor2";
      this.comboMulticolor2.Size = new System.Drawing.Size(71, 21);
      this.comboMulticolor2.TabIndex = 51;
      this.comboMulticolor2.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboColor_DrawItem);
      this.comboMulticolor2.SelectedIndexChanged += new System.EventHandler(this.comboMulticolor2_SelectedIndexChanged);
      // 
      // contextMenuExchangeColors
      // 
      this.contextMenuExchangeColors.ImageScalingSize = new System.Drawing.Size(28, 28);
      this.contextMenuExchangeColors.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem,
            this.exchangeMultiColor1WithBGColorToolStripMenuItem,
            this.exchangeMultiColor2WithBGColorToolStripMenuItem,
            this.exchangeCharColorWithBGColorToolStripMenuItem,
            this.exchangeCustomColorWithMultiColor1ToolStripMenuItem,
            this.exchangeCustomColorWithMultiColor2ToolStripMenuItem});
      this.contextMenuExchangeColors.Name = "contextMenuExchangeColors";
      this.contextMenuExchangeColors.Size = new System.Drawing.Size(301, 136);
      // 
      // exchangeMultiColor1WithMultiColor2ToolStripMenuItem
      // 
      this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem.Name = "exchangeMultiColor1WithMultiColor2ToolStripMenuItem";
      this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
      this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem.Text = "Exchange Multi Color 1 with Multi Color 2";
      this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem.Click += new System.EventHandler(this.exchangeMultiColor1WithMultiColor2ToolStripMenuItem_Click);
      // 
      // exchangeMultiColor1WithBGColorToolStripMenuItem
      // 
      this.exchangeMultiColor1WithBGColorToolStripMenuItem.Name = "exchangeMultiColor1WithBGColorToolStripMenuItem";
      this.exchangeMultiColor1WithBGColorToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
      this.exchangeMultiColor1WithBGColorToolStripMenuItem.Text = "Exchange Multi Color 1 with BG Color";
      this.exchangeMultiColor1WithBGColorToolStripMenuItem.Click += new System.EventHandler(this.exchangeMultiColor1WithBGColorToolStripMenuItem_Click);
      // 
      // exchangeMultiColor2WithBGColorToolStripMenuItem
      // 
      this.exchangeMultiColor2WithBGColorToolStripMenuItem.Name = "exchangeMultiColor2WithBGColorToolStripMenuItem";
      this.exchangeMultiColor2WithBGColorToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
      this.exchangeMultiColor2WithBGColorToolStripMenuItem.Text = "Exchange Multi Color 2 with BG Color";
      this.exchangeMultiColor2WithBGColorToolStripMenuItem.Click += new System.EventHandler(this.exchangeMultiColor2WithBGColorToolStripMenuItem_Click);
      // 
      // exchangeCharColorWithBGColorToolStripMenuItem
      // 
      this.exchangeCharColorWithBGColorToolStripMenuItem.Name = "exchangeCharColorWithBGColorToolStripMenuItem";
      this.exchangeCharColorWithBGColorToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
      this.exchangeCharColorWithBGColorToolStripMenuItem.Text = "Exchange Custom Color with BG Color";
      this.exchangeCharColorWithBGColorToolStripMenuItem.Click += new System.EventHandler(this.exchangeCharColorWithBGColorToolStripMenuItem_Click);
      // 
      // exchangeCustomColorWithMultiColor1ToolStripMenuItem
      // 
      this.exchangeCustomColorWithMultiColor1ToolStripMenuItem.Name = "exchangeCustomColorWithMultiColor1ToolStripMenuItem";
      this.exchangeCustomColorWithMultiColor1ToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
      this.exchangeCustomColorWithMultiColor1ToolStripMenuItem.Text = "Exchange Custom Color with Multi Color 1";
      this.exchangeCustomColorWithMultiColor1ToolStripMenuItem.Click += new System.EventHandler(this.exchangeCustomColorWithMultiColor1ToolStripMenuItem_Click);
      // 
      // exchangeCustomColorWithMultiColor2ToolStripMenuItem
      // 
      this.exchangeCustomColorWithMultiColor2ToolStripMenuItem.Name = "exchangeCustomColorWithMultiColor2ToolStripMenuItem";
      this.exchangeCustomColorWithMultiColor2ToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
      this.exchangeCustomColorWithMultiColor2ToolStripMenuItem.Text = "Exchange Custom Color with Multi Color 2";
      this.exchangeCustomColorWithMultiColor2ToolStripMenuItem.Click += new System.EventHandler(this.exchangeCustomColorWithMultiColor2ToolStripMenuItem_Click);
      // 
      // ColorSettingsMC
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.comboMulticolor2);
      this.Controls.Add(this.btnExchangeColors);
      this.Controls.Add(this.comboBackground);
      this.Controls.Add(this.comboMulticolor1);
      this.Controls.Add(this.comboCharColor);
      this.Controls.Add(this.radioBackground);
      this.Controls.Add(this.radioMultiColor1);
      this.Controls.Add(this.radioMulticolor2);
      this.Controls.Add(this.radioCharColor);
      this.Name = "ColorSettingsMC";
      this.contextMenuExchangeColors.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ComboBox comboBackground;
    private System.Windows.Forms.ComboBox comboMulticolor1;
    private System.Windows.Forms.ComboBox comboCharColor;
    private System.Windows.Forms.RadioButton radioBackground;
    private System.Windows.Forms.RadioButton radioMultiColor1;
    private System.Windows.Forms.RadioButton radioMulticolor2;
    private System.Windows.Forms.RadioButton radioCharColor;
    private MenuButton btnExchangeColors;
    private System.Windows.Forms.ComboBox comboMulticolor2;
    private System.Windows.Forms.ContextMenuStrip contextMenuExchangeColors;
    private System.Windows.Forms.ToolStripMenuItem exchangeMultiColor1WithMultiColor2ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exchangeMultiColor1WithBGColorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exchangeMultiColor2WithBGColorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exchangeCharColorWithBGColorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exchangeCustomColorWithMultiColor1ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exchangeCustomColorWithMultiColor2ToolStripMenuItem;
  }
}
