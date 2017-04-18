namespace C64Studio
{
  partial class DlgInterleaveData
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
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.hexOrig = new Be.Windows.Forms.HexBox();
      this.hexPreview = new Be.Windows.Forms.HexBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.editInterleave = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnCancel
      // 
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point( 632, 344 );
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // btnOK
      // 
      this.btnOK.Location = new System.Drawing.Point( 551, 344 );
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size( 75, 23 );
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler( this.btnOK_Click );
      // 
      // hexOrig
      // 
      this.hexOrig.BytesPerLine = 8;
      this.hexOrig.ColumnInfoVisible = true;
      this.hexOrig.Font = new System.Drawing.Font( "Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
      this.hexOrig.InfoForeColor = System.Drawing.SystemColors.AppWorkspace;
      this.hexOrig.LineInfoVisible = true;
      this.hexOrig.Location = new System.Drawing.Point( 8, 8 );
      this.hexOrig.Name = "hexOrig";
      this.hexOrig.ReadOnly = true;
      this.hexOrig.ShadowSelectionColor = System.Drawing.Color.FromArgb( ( (int)( ( (byte)( 100 ) ) ) ), ( (int)( ( (byte)( 60 ) ) ) ), ( (int)( ( (byte)( 188 ) ) ) ), ( (int)( ( (byte)( 255 ) ) ) ) );
      this.hexOrig.Size = new System.Drawing.Size( 395, 165 );
      this.hexOrig.StringViewVisible = true;
      this.hexOrig.TabIndex = 2;
      this.hexOrig.UseFixedBytesPerLine = true;
      this.hexOrig.VScrollBarVisible = true;
      // 
      // hexPreview
      // 
      this.hexPreview.BytesPerLine = 8;
      this.hexPreview.ColumnInfoVisible = true;
      this.hexPreview.Font = new System.Drawing.Font( "Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
      this.hexPreview.InfoForeColor = System.Drawing.SystemColors.AppWorkspace;
      this.hexPreview.LineInfoVisible = true;
      this.hexPreview.Location = new System.Drawing.Point( 8, 202 );
      this.hexPreview.Name = "hexPreview";
      this.hexPreview.ReadOnly = true;
      this.hexPreview.ShadowSelectionColor = System.Drawing.Color.FromArgb( ( (int)( ( (byte)( 100 ) ) ) ), ( (int)( ( (byte)( 60 ) ) ) ), ( (int)( ( (byte)( 188 ) ) ) ), ( (int)( ( (byte)( 255 ) ) ) ) );
      this.hexPreview.Size = new System.Drawing.Size( 395, 165 );
      this.hexPreview.StringViewVisible = true;
      this.hexPreview.TabIndex = 2;
      this.hexPreview.UseFixedBytesPerLine = true;
      this.hexPreview.VScrollBarVisible = true;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add( this.editInterleave );
      this.groupBox1.Controls.Add( this.label1 );
      this.groupBox1.Location = new System.Drawing.Point( 468, 8 );
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size( 239, 55 );
      this.groupBox1.TabIndex = 3;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Interleave settings";
      // 
      // editInterleave
      // 
      this.editInterleave.Location = new System.Drawing.Point( 70, 22 );
      this.editInterleave.Name = "editInterleave";
      this.editInterleave.Size = new System.Drawing.Size( 163, 20 );
      this.editInterleave.TabIndex = 1;
      this.editInterleave.TextChanged += new System.EventHandler( this.editInterleave_TextChanged );
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point( 6, 25 );
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size( 58, 13 );
      this.label1.TabIndex = 0;
      this.label1.Text = "Num Bytes";
      // 
      // DlgInterleaveData
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size( 719, 379 );
      this.Controls.Add( this.groupBox1 );
      this.Controls.Add( this.hexPreview );
      this.Controls.Add( this.hexOrig );
      this.Controls.Add( this.btnOK );
      this.Controls.Add( this.btnCancel );
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "DlgInterleaveData";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "Interleave Data";
      this.groupBox1.ResumeLayout( false );
      this.groupBox1.PerformLayout();
      this.ResumeLayout( false );

    }

    #endregion

    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnOK;
    private Be.Windows.Forms.HexBox hexOrig;
    private Be.Windows.Forms.HexBox hexPreview;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.TextBox editInterleave;
    private System.Windows.Forms.Label label1;
  }
}