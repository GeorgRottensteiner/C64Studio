namespace Tiny64Emu
{
  partial class FormMain
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.menuMain = new System.Windows.Forms.MenuStrip();
      this.tiny64ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.pictureOutput = new System.Windows.Forms.PictureBox();
      this.menuMain.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureOutput)).BeginInit();
      this.SuspendLayout();
      // 
      // menuMain
      // 
      this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tiny64ToolStripMenuItem});
      this.menuMain.Location = new System.Drawing.Point(0, 0);
      this.menuMain.Name = "menuMain";
      this.menuMain.Size = new System.Drawing.Size(553, 24);
      this.menuMain.TabIndex = 0;
      this.menuMain.Text = "menuStrip1";
      // 
      // tiny64ToolStripMenuItem
      // 
      this.tiny64ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
      this.tiny64ToolStripMenuItem.Name = "tiny64ToolStripMenuItem";
      this.tiny64ToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
      this.tiny64ToolStripMenuItem.Text = "Tiny64";
      // 
      // exitToolStripMenuItem
      // 
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.Size = new System.Drawing.Size(93, 22);
      this.exitToolStripMenuItem.Text = "E&xit";
      this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
      // 
      // pictureOutput
      // 
      this.pictureOutput.Location = new System.Drawing.Point(12, 27);
      this.pictureOutput.Name = "pictureOutput";
      this.pictureOutput.Size = new System.Drawing.Size(529, 364);
      this.pictureOutput.TabIndex = 1;
      this.pictureOutput.TabStop = false;
      this.pictureOutput.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.pictureOutput_PreviewKeyDown);
      // 
      // FormMain
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(553, 403);
      this.Controls.Add(this.pictureOutput);
      this.Controls.Add(this.menuMain);
      this.MainMenuStrip = this.menuMain;
      this.Name = "FormMain";
      this.Text = "Tiny64 Emu";
      this.menuMain.ResumeLayout(false);
      this.menuMain.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureOutput)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.MenuStrip menuMain;
    private System.Windows.Forms.ToolStripMenuItem tiny64ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.PictureBox pictureOutput;
  }
}

