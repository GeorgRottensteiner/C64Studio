
namespace RetroDevStudio.Controls
{
  partial class ImportCharscreenFromBASIC
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
      this.SuspendLayout();
      // 
      // editInput
      // 
      this.editInput.Dock = System.Windows.Forms.DockStyle.Fill;
      this.editInput.Location = new System.Drawing.Point(0, 0);
      this.editInput.MaxLength = 1000000;
      this.editInput.Multiline = true;
      this.editInput.Name = "editInput";
      this.editInput.Size = new System.Drawing.Size(317, 317);
      this.editInput.TabIndex = 0;
      this.editInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editInput_KeyPress);
      // 
      // ImportFromBASIC
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.editInput);
      this.Name = "ImportFromBASIC";
      this.Size = new System.Drawing.Size(317, 317);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox editInput;
  }
}
