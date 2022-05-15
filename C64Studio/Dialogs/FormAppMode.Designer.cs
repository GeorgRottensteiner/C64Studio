namespace RetroDevStudio
{
  partial class FormAppMode
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
      this.label1 = new System.Windows.Forms.Label();
      this.btnNormalAppMode = new System.Windows.Forms.Button();
      this.btnPortableMode = new System.Windows.Forms.Button();
      this.btnAskLater = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(13, 13);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(520, 53);
      this.label1.TabIndex = 0;
      this.label1.Text = "No settings file was found.\r\n\r\nDo you want C64Studio to run in normal mode or as " +
    "portable app?";
      // 
      // btnNormalAppMode
      // 
      this.btnNormalAppMode.Location = new System.Drawing.Point(121, 79);
      this.btnNormalAppMode.Name = "btnNormalAppMode";
      this.btnNormalAppMode.Size = new System.Drawing.Size(97, 79);
      this.btnNormalAppMode.TabIndex = 1;
      this.btnNormalAppMode.Text = "Normal Mode";
      this.btnNormalAppMode.UseVisualStyleBackColor = true;
      this.btnNormalAppMode.Click += new System.EventHandler(this.btnNormalAppMode_Click);
      // 
      // btnPortableMode
      // 
      this.btnPortableMode.Location = new System.Drawing.Point(224, 79);
      this.btnPortableMode.Name = "btnPortableMode";
      this.btnPortableMode.Size = new System.Drawing.Size(97, 79);
      this.btnPortableMode.TabIndex = 1;
      this.btnPortableMode.Text = "Portable App Mode";
      this.btnPortableMode.UseVisualStyleBackColor = true;
      this.btnPortableMode.Click += new System.EventHandler(this.btnPortableMode_Click);
      // 
      // btnAskLater
      // 
      this.btnAskLater.Location = new System.Drawing.Point(327, 79);
      this.btnAskLater.Name = "btnAskLater";
      this.btnAskLater.Size = new System.Drawing.Size(97, 79);
      this.btnAskLater.TabIndex = 1;
      this.btnAskLater.Text = "Ask me later";
      this.btnAskLater.UseVisualStyleBackColor = true;
      this.btnAskLater.Click += new System.EventHandler(this.btnAskLater_Click);
      // 
      // label2
      // 
      this.label2.Location = new System.Drawing.Point(13, 183);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(520, 48);
      this.label2.TabIndex = 0;
      this.label2.Text = "Normal mode saves settings in the users AppData folder.\r\n\r\nPortable App mode save" +
    "s settings in the local application folder.";
      // 
      // FormAppMode
      // 
      this.AcceptButton = this.btnNormalAppMode;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(545, 251);
      this.Controls.Add(this.btnAskLater);
      this.Controls.Add(this.btnPortableMode);
      this.Controls.Add(this.btnNormalAppMode);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormAppMode";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "App Mode Selector";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button btnNormalAppMode;
    private System.Windows.Forms.Button btnPortableMode;
    private System.Windows.Forms.Button btnAskLater;
    private System.Windows.Forms.Label label2;
  }
}