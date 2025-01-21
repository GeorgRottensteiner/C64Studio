namespace RetroDevStudio.Dialogs.Preferences
{
  partial class PrefBASICEditor
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
      this.checkBASICEditorShowMaxLineLengthIndicator = new System.Windows.Forms.CheckBox();
      this.label1 = new System.Windows.Forms.Label();
      this.editMaxLineLengthIndicatorColumn = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.editBASICC64FontSize = new System.Windows.Forms.TextBox();
      this.labelBASICC64FontSize = new System.Windows.Forms.Label();
      this.checkBASICUseC64Font = new System.Windows.Forms.CheckBox();
      this.btnChangeBASICFont = new DecentForms.Button();
      this.labelBASICFontPreview = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // checkBASICEditorShowMaxLineLengthIndicator
      // 
      this.checkBASICEditorShowMaxLineLengthIndicator.AutoSize = true;
      this.checkBASICEditorShowMaxLineLengthIndicator.Location = new System.Drawing.Point(6, 59);
      this.checkBASICEditorShowMaxLineLengthIndicator.Name = "checkBASICEditorShowMaxLineLengthIndicator";
      this.checkBASICEditorShowMaxLineLengthIndicator.Size = new System.Drawing.Size(169, 17);
      this.checkBASICEditorShowMaxLineLengthIndicator.TabIndex = 24;
      this.checkBASICEditorShowMaxLineLengthIndicator.Text = "Show max line length indicator";
      this.checkBASICEditorShowMaxLineLengthIndicator.UseVisualStyleBackColor = true;
      this.checkBASICEditorShowMaxLineLengthIndicator.CheckedChanged += new System.EventHandler(this.checkBASICEditorShowMaxLineLengthIndicator_CheckedChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(3, 11);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(31, 13);
      this.label1.TabIndex = 23;
      this.label1.Text = "Font:";
      // 
      // editMaxLineLengthIndicatorColumn
      // 
      this.editMaxLineLengthIndicatorColumn.Enabled = false;
      this.editMaxLineLengthIndicatorColumn.Location = new System.Drawing.Point(242, 57);
      this.editMaxLineLengthIndicatorColumn.MaxLength = 3;
      this.editMaxLineLengthIndicatorColumn.Name = "editMaxLineLengthIndicatorColumn";
      this.editMaxLineLengthIndicatorColumn.Size = new System.Drawing.Size(91, 20);
      this.editMaxLineLengthIndicatorColumn.TabIndex = 22;
      this.editMaxLineLengthIndicatorColumn.TextChanged += new System.EventHandler(this.editMaxLineLengthIndicatorColumn_TextChanged);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(206, 60);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(25, 13);
      this.label2.TabIndex = 21;
      this.label2.Text = "Col:";
      // 
      // editBASICC64FontSize
      // 
      this.editBASICC64FontSize.Enabled = false;
      this.editBASICC64FontSize.Location = new System.Drawing.Point(242, 34);
      this.editBASICC64FontSize.MaxLength = 3;
      this.editBASICC64FontSize.Name = "editBASICC64FontSize";
      this.editBASICC64FontSize.Size = new System.Drawing.Size(91, 20);
      this.editBASICC64FontSize.TabIndex = 22;
      this.editBASICC64FontSize.TextChanged += new System.EventHandler(this.editBASICC64FontSize_TextChanged);
      // 
      // labelBASICC64FontSize
      // 
      this.labelBASICC64FontSize.AutoSize = true;
      this.labelBASICC64FontSize.Location = new System.Drawing.Point(206, 37);
      this.labelBASICC64FontSize.Name = "labelBASICC64FontSize";
      this.labelBASICC64FontSize.Size = new System.Drawing.Size(30, 13);
      this.labelBASICC64FontSize.TabIndex = 21;
      this.labelBASICC64FontSize.Text = "Size:";
      // 
      // checkBASICUseC64Font
      // 
      this.checkBASICUseC64Font.AutoSize = true;
      this.checkBASICUseC64Font.Location = new System.Drawing.Point(6, 36);
      this.checkBASICUseC64Font.Name = "checkBASICUseC64Font";
      this.checkBASICUseC64Font.Size = new System.Drawing.Size(88, 17);
      this.checkBASICUseC64Font.TabIndex = 20;
      this.checkBASICUseC64Font.Text = "Use C64 font";
      this.checkBASICUseC64Font.UseVisualStyleBackColor = true;
      this.checkBASICUseC64Font.CheckedChanged += new System.EventHandler(this.checkBASICUseC64Font_CheckedChanged);
      // 
      // btnChangeBASICFont
      // 
      this.btnChangeBASICFont.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnChangeBASICFont.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnChangeBASICFont.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnChangeBASICFont.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnChangeBASICFont.Enabled = false;
      this.btnChangeBASICFont.Image = null;
      this.btnChangeBASICFont.Location = new System.Drawing.Point(209, 5);
      this.btnChangeBASICFont.Name = "btnChangeBASICFont";
      this.btnChangeBASICFont.Size = new System.Drawing.Size(124, 23);
      this.btnChangeBASICFont.TabIndex = 19;
      this.btnChangeBASICFont.Text = "Change Font";
      this.btnChangeBASICFont.Click += new DecentForms.EventHandler(this.btnChangeBASICFont_Click);
      // 
      // labelBASICFontPreview
      // 
      this.labelBASICFontPreview.Location = new System.Drawing.Point(339, 11);
      this.labelBASICFontPreview.Name = "labelBASICFontPreview";
      this.labelBASICFontPreview.Size = new System.Drawing.Size(209, 35);
      this.labelBASICFontPreview.TabIndex = 18;
      this.labelBASICFontPreview.Text = "BASIC Font Preview";
      // 
      // PrefBASICEditor
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.checkBASICEditorShowMaxLineLengthIndicator);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.editMaxLineLengthIndicatorColumn);
      this.Controls.Add(this.labelBASICFontPreview);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.btnChangeBASICFont);
      this.Controls.Add(this.editBASICC64FontSize);
      this.Controls.Add(this.checkBASICUseC64Font);
      this.Controls.Add(this.labelBASICC64FontSize);
      this.Name = "PrefBASICEditor";
      this.Size = new System.Drawing.Size(477, 110);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

        #endregion
        private System.Windows.Forms.TextBox editBASICC64FontSize;
        private System.Windows.Forms.Label labelBASICC64FontSize;
        private System.Windows.Forms.CheckBox checkBASICUseC64Font;
        private DecentForms.Button btnChangeBASICFont;
        private System.Windows.Forms.Label labelBASICFontPreview;
        private System.Windows.Forms.Label label1;
    private System.Windows.Forms.CheckBox checkBASICEditorShowMaxLineLengthIndicator;
    private System.Windows.Forms.TextBox editMaxLineLengthIndicatorColumn;
    private System.Windows.Forms.Label label2;
  }
}
