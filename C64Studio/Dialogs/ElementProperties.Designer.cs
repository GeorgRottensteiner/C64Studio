namespace RetroDevStudio
{
  partial class ElementProperties
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
      this.tabElementProperties = new System.Windows.Forms.TabControl();
      this.btnClose = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // tabElementProperties
      // 
      this.tabElementProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tabElementProperties.Location = new System.Drawing.Point(12, 12);
      this.tabElementProperties.Name = "tabElementProperties";
      this.tabElementProperties.SelectedIndex = 0;
      this.tabElementProperties.Size = new System.Drawing.Size(611, 376);
      this.tabElementProperties.TabIndex = 0;
      // 
      // btnClose
      // 
      this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnClose.Location = new System.Drawing.Point(548, 394);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(75, 23);
      this.btnClose.TabIndex = 1;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      // 
      // ElementProperties
      // 
      this.AcceptButton = this.btnClose;
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.CancelButton = this.btnClose;
      this.ClientSize = new System.Drawing.Size(635, 429);
      this.Controls.Add(this.btnClose);
      this.Controls.Add(this.tabElementProperties);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ElementProperties";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "Element Properties";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl tabElementProperties;
    private System.Windows.Forms.Button btnClose;
  }
}