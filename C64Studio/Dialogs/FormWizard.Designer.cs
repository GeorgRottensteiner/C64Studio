namespace C64Studio
{
  partial class FormWizard
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
      this.labelACMEPath = new System.Windows.Forms.Label();
      this.editPathACME = new System.Windows.Forms.TextBox();
      this.btnBrowseACME = new System.Windows.Forms.Button();
      this.label3 = new System.Windows.Forms.Label();
      this.editPathVice = new System.Windows.Forms.TextBox();
      this.btnBrowseVice = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point( 13, 13 );
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size( 520, 76 );
      this.label1.TabIndex = 0;
      this.label1.Text = "This wizard sets up a tool entry for Vice with all needed parameters.\r\n";
      // 
      // labelACMEPath
      // 
      this.labelACMEPath.AutoSize = true;
      this.labelACMEPath.Location = new System.Drawing.Point( 13, 116 );
      this.labelACMEPath.Name = "labelACMEPath";
      this.labelACMEPath.Size = new System.Drawing.Size( 65, 13 );
      this.labelACMEPath.TabIndex = 1;
      this.labelACMEPath.Text = "ACME Path:";
      // 
      // editPathACME
      // 
      this.editPathACME.Location = new System.Drawing.Point( 84, 113 );
      this.editPathACME.Name = "editPathACME";
      this.editPathACME.Size = new System.Drawing.Size( 376, 20 );
      this.editPathACME.TabIndex = 0;
      this.editPathACME.TextChanged += new System.EventHandler(this.editPath_TextChanged);
      // 
      // btnBrowseACME
      // 
      this.btnBrowseACME.Location = new System.Drawing.Point( 466, 111 );
      this.btnBrowseACME.Name = "btnBrowseACME";
      this.btnBrowseACME.Size = new System.Drawing.Size( 67, 23 );
      this.btnBrowseACME.TabIndex = 1;
      this.btnBrowseACME.Text = "...";
      this.btnBrowseACME.UseVisualStyleBackColor = true;
      this.btnBrowseACME.Click += new System.EventHandler( this.btnBrowseACME_Click );
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point( 13, 145 );
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size( 56, 13 );
      this.label3.TabIndex = 1;
      this.label3.Text = "Vice Path:";
      // 
      // editPathVice
      // 
      this.editPathVice.Location = new System.Drawing.Point( 84, 142 );
      this.editPathVice.Name = "editPathVice";
      this.editPathVice.Size = new System.Drawing.Size( 376, 20 );
      this.editPathVice.TabIndex = 2;
      this.editPathVice.TextChanged += new System.EventHandler(this.editPath_TextChanged);
      // 
      // btnBrowseVice
      // 
      this.btnBrowseVice.Location = new System.Drawing.Point( 466, 140 );
      this.btnBrowseVice.Name = "btnBrowseVice";
      this.btnBrowseVice.Size = new System.Drawing.Size( 67, 23 );
      this.btnBrowseVice.TabIndex = 3;
      this.btnBrowseVice.Text = "...";
      this.btnBrowseVice.UseVisualStyleBackColor = true;
      this.btnBrowseVice.Click += new System.EventHandler( this.btnBrowseVice_Click );
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point( 458, 216 );
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // btnOK
      // 
      this.btnOK.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
      this.btnOK.Location = new System.Drawing.Point( 377, 216 );
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size( 75, 23 );
      this.btnOK.TabIndex = 4;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler( this.btnOK_Click );
      // 
      // FormWizard
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size( 545, 251 );
      this.Controls.Add( this.btnOK );
      this.Controls.Add( this.btnCancel );
      this.Controls.Add( this.btnBrowseVice );
      this.Controls.Add( this.editPathVice );
      this.Controls.Add( this.btnBrowseACME );
      this.Controls.Add( this.label3 );
      this.Controls.Add( this.editPathACME );
      this.Controls.Add( this.labelACMEPath );
      this.Controls.Add( this.label1 );
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormWizard";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Setup Wizard";
      this.ResumeLayout( false );
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label labelACMEPath;
    private System.Windows.Forms.Button btnBrowseACME;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Button btnBrowseVice;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnOK;
    public System.Windows.Forms.TextBox editPathACME;
    public System.Windows.Forms.TextBox editPathVice;
  }
}