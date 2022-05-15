namespace RetroDevStudio.Dialogs
{
  partial class DlgImportImageResize
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
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
      this.labelImageInfo = new System.Windows.Forms.Label();
      this.btnClip = new System.Windows.Forms.Button();
      this.btnAdjustScreenSize = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // labelImageInfo
      // 
      this.labelImageInfo.Location = new System.Drawing.Point( 12, 9 );
      this.labelImageInfo.Name = "labelImageInfo";
      this.labelImageInfo.Size = new System.Drawing.Size( 411, 146 );
      this.labelImageInfo.TabIndex = 0;
      this.labelImageInfo.Text = "The imported image is x,y, the current screen size is x2,y2.\\r\\nShould the image " +
          "be clipped or the screen size be adjusted?";
      this.labelImageInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // btnClip
      // 
      this.btnClip.Location = new System.Drawing.Point( 13, 168 );
      this.btnClip.Name = "btnClip";
      this.btnClip.Size = new System.Drawing.Size( 127, 23 );
      this.btnClip.TabIndex = 1;
      this.btnClip.Text = "Clip Image";
      this.btnClip.UseVisualStyleBackColor = true;
      this.btnClip.Click += new System.EventHandler( this.btnClip_Click );
      // 
      // btnAdjustScreenSize
      // 
      this.btnAdjustScreenSize.Location = new System.Drawing.Point( 154, 168 );
      this.btnAdjustScreenSize.Name = "btnAdjustScreenSize";
      this.btnAdjustScreenSize.Size = new System.Drawing.Size( 127, 23 );
      this.btnAdjustScreenSize.TabIndex = 1;
      this.btnAdjustScreenSize.Text = "Adjust Screen Size";
      this.btnAdjustScreenSize.UseVisualStyleBackColor = true;
      this.btnAdjustScreenSize.Click += new System.EventHandler( this.btnAdjustScreenSize_Click );
      // 
      // btnCancel
      // 
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point( 295, 168 );
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size( 127, 23 );
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel Import";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new System.EventHandler( this.btnCancel_Click );
      // 
      // DlgImportImage
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size( 435, 205 );
      this.Controls.Add( this.btnAdjustScreenSize );
      this.Controls.Add( this.btnCancel );
      this.Controls.Add( this.btnClip );
      this.Controls.Add( this.labelImageInfo );
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "DlgImportImage";
      this.Padding = new System.Windows.Forms.Padding( 9 );
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Image size is different from screen size";
      this.ResumeLayout( false );

    }

    #endregion

    private System.Windows.Forms.Label labelImageInfo;
    private System.Windows.Forms.Button btnClip;
    private System.Windows.Forms.Button btnAdjustScreenSize;
    private System.Windows.Forms.Button btnCancel;

  }
}
