namespace C64Studio
{
  partial class FormAbout
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
      this.btnOK = new System.Windows.Forms.Button();
      this.pictureLogo = new System.Windows.Forms.PictureBox();
      this.labelInfo = new System.Windows.Forms.Label();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      ((System.ComponentModel.ISupportInitialize)(this.pictureLogo)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      this.SuspendLayout();
      // 
      // btnOK
      // 
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnOK.Location = new System.Drawing.Point(308, 345);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 0;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // pictureLogo
      // 
      this.pictureLogo.Image = ((System.Drawing.Image)(resources.GetObject("pictureLogo.Image")));
      this.pictureLogo.Location = new System.Drawing.Point(112, 23);
      this.pictureLogo.Name = "pictureLogo";
      this.pictureLogo.Size = new System.Drawing.Size(170, 64);
      this.pictureLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.pictureLogo.TabIndex = 1;
      this.pictureLogo.TabStop = false;
      // 
      // labelInfo
      // 
      this.labelInfo.Location = new System.Drawing.Point(22, 246);
      this.labelInfo.Name = "labelInfo";
      this.labelInfo.Size = new System.Drawing.Size(351, 96);
      this.labelInfo.TabIndex = 2;
      this.labelInfo.Text = "C64 Studio 5.5g\r\n\r\nWritten by Georg Rottensteiner 2011-2018\r\n\r\nBased on WinVICE\r\n" +
    "";
      this.labelInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // pictureBox1
      // 
      this.pictureBox1.BackColor = System.Drawing.SystemColors.ButtonFace;
      this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
      this.pictureBox1.Location = new System.Drawing.Point(78, 127);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(239, 97);
      this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 3;
      this.pictureBox1.TabStop = false;
      // 
      // FormAbout
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnOK;
      this.ClientSize = new System.Drawing.Size(395, 380);
      this.Controls.Add(this.pictureBox1);
      this.Controls.Add(this.labelInfo);
      this.Controls.Add(this.pictureLogo);
      this.Controls.Add(this.btnOK);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormAbout";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "C64 Studio";
      ((System.ComponentModel.ISupportInitialize)(this.pictureLogo)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.PictureBox pictureLogo;
    private System.Windows.Forms.Label labelInfo;
    private System.Windows.Forms.PictureBox pictureBox1;
  }
}