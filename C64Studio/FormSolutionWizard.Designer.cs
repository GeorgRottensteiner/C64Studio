namespace C64Studio
{
  partial class FormSolutionWizard
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
      this.labelBasePath = new System.Windows.Forms.Label();
      this.editBasePath = new System.Windows.Forms.TextBox();
      this.btnBrowseBasePath = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.editSolutionName = new System.Windows.Forms.TextBox();
      this.labelSolutionSummary = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point( 13, 13 );
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size( 520, 38 );
      this.label1.TabIndex = 0;
      this.label1.Text = "This wizard walks you through setting up a new solution";
      // 
      // labelBasePath
      // 
      this.labelBasePath.AutoSize = true;
      this.labelBasePath.Location = new System.Drawing.Point( 13, 83 );
      this.labelBasePath.Name = "labelBasePath";
      this.labelBasePath.Size = new System.Drawing.Size( 107, 13 );
      this.labelBasePath.TabIndex = 1;
      this.labelBasePath.Text = "Solution Base Folder:";
      // 
      // editBasePath
      // 
      this.editBasePath.Location = new System.Drawing.Point( 121, 80 );
      this.editBasePath.Name = "editBasePath";
      this.editBasePath.Size = new System.Drawing.Size( 339, 20 );
      this.editBasePath.TabIndex = 0;
      this.editBasePath.TextChanged += new System.EventHandler( this.editBasePath_TextChanged );
      // 
      // btnBrowseBasePath
      // 
      this.btnBrowseBasePath.Location = new System.Drawing.Point( 466, 78 );
      this.btnBrowseBasePath.Name = "btnBrowseBasePath";
      this.btnBrowseBasePath.Size = new System.Drawing.Size( 67, 23 );
      this.btnBrowseBasePath.TabIndex = 1;
      this.btnBrowseBasePath.Text = "...";
      this.btnBrowseBasePath.UseVisualStyleBackColor = true;
      this.btnBrowseBasePath.Click += new System.EventHandler( this.btnBrowseBasePath_Click );
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
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point( 13, 57 );
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size( 79, 13 );
      this.label2.TabIndex = 1;
      this.label2.Text = "Solution Name:";
      // 
      // editSolutionName
      // 
      this.editSolutionName.Location = new System.Drawing.Point( 121, 54 );
      this.editSolutionName.Name = "editSolutionName";
      this.editSolutionName.Size = new System.Drawing.Size( 339, 20 );
      this.editSolutionName.TabIndex = 0;
      this.editSolutionName.Text = "New Solution";
      this.editSolutionName.TextChanged += new System.EventHandler( this.editSolutionName_TextChanged );
      // 
      // labelSolutionSummary
      // 
      this.labelSolutionSummary.Location = new System.Drawing.Point( 12, 153 );
      this.labelSolutionSummary.Name = "labelSolutionSummary";
      this.labelSolutionSummary.Size = new System.Drawing.Size( 520, 60 );
      this.labelSolutionSummary.TabIndex = 0;
      this.labelSolutionSummary.Text = "summary";
      // 
      // FormSolutionWizard
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size( 545, 251 );
      this.Controls.Add( this.btnOK );
      this.Controls.Add( this.btnCancel );
      this.Controls.Add( this.btnBrowseBasePath );
      this.Controls.Add( this.editSolutionName );
      this.Controls.Add( this.label2 );
      this.Controls.Add( this.editBasePath );
      this.Controls.Add( this.labelBasePath );
      this.Controls.Add( this.labelSolutionSummary );
      this.Controls.Add( this.label1 );
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormSolutionWizard";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "New Solution Wizard";
      this.ResumeLayout( false );
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label labelBasePath;
    private System.Windows.Forms.Button btnBrowseBasePath;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnOK;
    public System.Windows.Forms.TextBox editBasePath;
    private System.Windows.Forms.Label label2;
    public System.Windows.Forms.TextBox editSolutionName;
    private System.Windows.Forms.Label labelSolutionSummary;
  }
}