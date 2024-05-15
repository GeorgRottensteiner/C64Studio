namespace RetroDevStudio.Dialogs
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
      this.label3 = new System.Windows.Forms.Label();
      this.editPathEmulator = new System.Windows.Forms.TextBox();
      this.btnBrowseVice = new DecentForms.Button();
      this.btnCancel = new DecentForms.Button();
      this.btnOK = new DecentForms.Button();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(13, 13);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(520, 76);
      this.label1.TabIndex = 0;
      this.label1.Text = "This wizard sets up a tool entry for an emulator with all needed parameters.\r\n";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(13, 145);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(76, 13);
      this.label3.TabIndex = 1;
      this.label3.Text = "Emulator Path:";
      // 
      // editPathEmulator
      // 
      this.editPathEmulator.Location = new System.Drawing.Point(95, 142);
      this.editPathEmulator.Name = "editPathEmulator";
      this.editPathEmulator.Size = new System.Drawing.Size(365, 20);
      this.editPathEmulator.TabIndex = 2;
      this.editPathEmulator.TextChanged += new System.EventHandler(this.editPathEmulator_TextChanged);
      // 
      // btnBrowseVice
      // 
      this.btnBrowseVice.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnBrowseVice.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnBrowseVice.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnBrowseVice.Image = null;
      this.btnBrowseVice.Location = new System.Drawing.Point(466, 140);
      this.btnBrowseVice.Name = "btnBrowseVice";
      this.btnBrowseVice.Size = new System.Drawing.Size(67, 23);
      this.btnBrowseVice.TabIndex = 3;
      this.btnBrowseVice.Text = "...";
      this.btnBrowseVice.Click += new DecentForms.EventHandler(this.btnBrowseVice_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnCancel.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Image = null;
      this.btnCancel.Location = new System.Drawing.Point(458, 216);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.Click += new DecentForms.EventHandler(this.btnCancel_Click);
      // 
      // btnOK
      // 
      this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnOK.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnOK.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.Image = null;
      this.btnOK.Location = new System.Drawing.Point(377, 216);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 4;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new DecentForms.EventHandler(this.btnOK_Click);
      // 
      // FormWizard
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(545, 251);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnBrowseVice);
      this.Controls.Add(this.editPathEmulator);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormWizard";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Setup Wizard";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label3;
    private DecentForms.Button btnBrowseVice;
    private DecentForms.Button btnCancel;
    private DecentForms.Button btnOK;
    public System.Windows.Forms.TextBox editPathEmulator;
  }
}