
namespace RetroDevStudio.Controls
{
  partial class ColorChooserCommodoreVIC20
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
      this.comboBorderColor = new System.Windows.Forms.ComboBox();
      this.comboAuxColor = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
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
      // comboBorderColor
      // 
      this.comboBorderColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboBorderColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBorderColor.FormattingEnabled = true;
      this.comboBorderColor.Location = new System.Drawing.Point(93, 38);
      this.comboBorderColor.Name = "comboBorderColor";
      this.comboBorderColor.Size = new System.Drawing.Size(71, 21);
      this.comboBorderColor.TabIndex = 3;
      this.comboBorderColor.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboColor_DrawItem);
      this.comboBorderColor.SelectedIndexChanged += new System.EventHandler(this.comboMulticolor1_SelectedIndexChanged);
      // 
      // comboAuxColor
      // 
      this.comboAuxColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.comboAuxColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboAuxColor.FormattingEnabled = true;
      this.comboAuxColor.Location = new System.Drawing.Point(93, 65);
      this.comboAuxColor.Name = "comboAuxColor";
      this.comboAuxColor.Size = new System.Drawing.Size(71, 21);
      this.comboAuxColor.TabIndex = 5;
      this.comboAuxColor.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboColor_DrawItem);
      this.comboAuxColor.SelectedIndexChanged += new System.EventHandler(this.comboMulticolor2_SelectedIndexChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(3, 14);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(68, 13);
      this.label1.TabIndex = 6;
      this.label1.Text = "Background:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(3, 41);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(68, 13);
      this.label2.TabIndex = 6;
      this.label2.Text = "Border Color:";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(3, 68);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(55, 13);
      this.label3.TabIndex = 6;
      this.label3.Text = "Aux Color:";
      // 
      // ColorChooserCommodoreVIC20
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.comboAuxColor);
      this.Controls.Add(this.comboBackground);
      this.Controls.Add(this.comboBorderColor);
      this.Name = "ColorChooserCommodoreVIC20";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ComboBox comboBackground;
    private System.Windows.Forms.ComboBox comboBorderColor;
    private System.Windows.Forms.ComboBox comboAuxColor;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
  }
}
