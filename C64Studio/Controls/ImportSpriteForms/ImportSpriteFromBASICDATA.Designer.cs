
namespace RetroDevStudio.Controls
{
  partial class ImportSpriteFromBASICDATA
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
      this.editInput = new System.Windows.Forms.TextBox();
      this.checkHexData = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // editInput
      // 
      this.editInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.editInput.Location = new System.Drawing.Point(0, 35);
      this.editInput.MaxLength = 1000000;
      this.editInput.Multiline = true;
      this.editInput.Name = "editInput";
      this.editInput.Size = new System.Drawing.Size(317, 282);
      this.editInput.TabIndex = 1;
      this.editInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editInput_KeyPress);
      // 
      // checkHexData
      // 
      this.checkHexData.AutoSize = true;
      this.checkHexData.Location = new System.Drawing.Point(3, 3);
      this.checkHexData.Name = "checkHexData";
      this.checkHexData.Size = new System.Drawing.Size(111, 17);
      this.checkHexData.TabIndex = 0;
      this.checkHexData.Text = "Hex DATA values";
      this.checkHexData.UseVisualStyleBackColor = true;
      // 
      // ImportSpriteFromBASICDATA
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.checkHexData);
      this.Controls.Add(this.editInput);
      this.Name = "ImportSpriteFromBASICDATA";
      this.Size = new System.Drawing.Size(317, 317);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox editInput;
    private System.Windows.Forms.CheckBox checkHexData;
  }
}
