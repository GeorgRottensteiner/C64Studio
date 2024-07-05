
namespace RetroDevStudio.Controls
{
  partial class ColorSettingsNCMMega65
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
      this.comboBackground = new System.Windows.Forms.ComboBox();
      this.comboCharColor = new System.Windows.Forms.ComboBox();
      this.radioBackground = new System.Windows.Forms.RadioButton();
      this.radioCharColor = new System.Windows.Forms.RadioButton();
      this.label1 = new System.Windows.Forms.Label();
      this.comboActivePalette = new System.Windows.Forms.ComboBox();
      this.btnEditPalette = new DecentForms.Button();
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
      this.comboBackground.TabIndex = 1;
      this.comboBackground.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboColor_DrawItem);
      this.comboBackground.SelectedIndexChanged += new System.EventHandler(this.comboBackground_SelectedIndexChanged);
      // 
      // comboCharColor
      // 
      this.comboCharColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboCharColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboCharColor.FormattingEnabled = true;
      this.comboCharColor.Location = new System.Drawing.Point(93, 38);
      this.comboCharColor.Name = "comboCharColor";
      this.comboCharColor.Size = new System.Drawing.Size(71, 21);
      this.comboCharColor.TabIndex = 3;
      this.comboCharColor.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboCharColor_DrawItem);
      this.comboCharColor.SelectedIndexChanged += new System.EventHandler(this.comboCharColor_SelectedIndexChanged);
      // 
      // radioBackground
      // 
      this.radioBackground.AutoSize = true;
      this.radioBackground.Location = new System.Drawing.Point(3, 12);
      this.radioBackground.Name = "radioBackground";
      this.radioBackground.Size = new System.Drawing.Size(83, 17);
      this.radioBackground.TabIndex = 0;
      this.radioBackground.TabStop = true;
      this.radioBackground.Text = "Background";
      this.radioBackground.UseVisualStyleBackColor = true;
      this.radioBackground.CheckedChanged += new System.EventHandler(this.radioBackground_CheckedChanged);
      // 
      // radioCharColor
      // 
      this.radioCharColor.AutoSize = true;
      this.radioCharColor.Location = new System.Drawing.Point(3, 39);
      this.radioCharColor.Name = "radioCharColor";
      this.radioCharColor.Size = new System.Drawing.Size(74, 17);
      this.radioCharColor.TabIndex = 2;
      this.radioCharColor.TabStop = true;
      this.radioCharColor.Text = "Char Color";
      this.radioCharColor.UseVisualStyleBackColor = true;
      this.radioCharColor.CheckedChanged += new System.EventHandler(this.radioCharColor_CheckedChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(3, 101);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(43, 13);
      this.label1.TabIndex = 61;
      this.label1.Text = "Palette:";
      // 
      // comboActivePalette
      // 
      this.comboActivePalette.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboActivePalette.FormattingEnabled = true;
      this.comboActivePalette.Location = new System.Drawing.Point(54, 98);
      this.comboActivePalette.Name = "comboActivePalette";
      this.comboActivePalette.Size = new System.Drawing.Size(110, 21);
      this.comboActivePalette.TabIndex = 5;
      this.comboActivePalette.SelectedIndexChanged += new System.EventHandler(this.comboActivePalette_SelectedIndexChanged);
      // 
      // btnEditPalette
      // 
      this.btnEditPalette.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnEditPalette.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnEditPalette.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnEditPalette.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnEditPalette.Image = null;
      this.btnEditPalette.Location = new System.Drawing.Point(3, 66);
      this.btnEditPalette.Name = "btnEditPalette";
      this.btnEditPalette.Size = new System.Drawing.Size(161, 26);
      this.btnEditPalette.TabIndex = 4;
      this.btnEditPalette.Text = "Edit Palette";
      this.btnEditPalette.Click += new DecentForms.EventHandler(this.btnEditPalette_Click);
      // 
      // ColorSettingsNCMMega65
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.label1);
      this.Controls.Add(this.comboActivePalette);
      this.Controls.Add(this.btnEditPalette);
      this.Controls.Add(this.comboBackground);
      this.Controls.Add(this.comboCharColor);
      this.Controls.Add(this.radioBackground);
      this.Controls.Add(this.radioCharColor);
      this.Name = "ColorSettingsNCMMega65";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ComboBox comboBackground;
    private System.Windows.Forms.ComboBox comboCharColor;
    private System.Windows.Forms.RadioButton radioBackground;
    private System.Windows.Forms.RadioButton radioCharColor;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox comboActivePalette;
    private DecentForms.Button btnEditPalette;
  }
}
