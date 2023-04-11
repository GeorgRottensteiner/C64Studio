namespace RetroDevStudio.Dialogs
{
  partial class FormFilesChanged
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
      this.btnKeepFiles = new System.Windows.Forms.Button();
      this.btnReloadAll = new System.Windows.Forms.Button();
      this.listChangedFiles = new Controls.CSListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(13, 13);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(435, 46);
      this.label1.TabIndex = 0;
      this.label1.Text = "One or more files have been changed externally.  You can choose to reload all fil" +
    "es or leave them in their current state.";
      // 
      // btnKeepFiles
      // 
      this.btnKeepFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnKeepFiles.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnKeepFiles.Location = new System.Drawing.Point(321, 334);
      this.btnKeepFiles.Name = "btnKeepFiles";
      this.btnKeepFiles.Size = new System.Drawing.Size(127, 23);
      this.btnKeepFiles.TabIndex = 5;
      this.btnKeepFiles.Text = "Keep Open Files";
      this.btnKeepFiles.UseVisualStyleBackColor = true;
      // 
      // btnReloadAll
      // 
      this.btnReloadAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnReloadAll.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnReloadAll.Location = new System.Drawing.Point(188, 334);
      this.btnReloadAll.Name = "btnReloadAll";
      this.btnReloadAll.Size = new System.Drawing.Size(127, 23);
      this.btnReloadAll.TabIndex = 4;
      this.btnReloadAll.Text = "Reload All Files";
      this.btnReloadAll.UseVisualStyleBackColor = true;
      this.btnReloadAll.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // listChangedFiles
      // 
      this.listChangedFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
      this.listChangedFiles.FullRowSelect = true;
      this.listChangedFiles.Location = new System.Drawing.Point(12, 74);
      this.listChangedFiles.Name = "listChangedFiles";
      this.listChangedFiles.Size = new System.Drawing.Size(436, 245);
      this.listChangedFiles.TabIndex = 6;
      this.listChangedFiles.UseCompatibleStateImageBehavior = false;
      this.listChangedFiles.View = System.Windows.Forms.View.Details;
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Filename";
      this.columnHeader1.Width = 400;
      // 
      // FormFilesChanged
      // 
      this.AcceptButton = this.btnReloadAll;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnKeepFiles;
      this.ClientSize = new System.Drawing.Size(460, 369);
      this.Controls.Add(this.listChangedFiles);
      this.Controls.Add(this.btnReloadAll);
      this.Controls.Add(this.btnKeepFiles);
      this.Controls.Add(this.label1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormFilesChanged";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Files have been changed externally";
      this.Load += new System.EventHandler(this.FormFilesChanged_Load);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button btnKeepFiles;
    private System.Windows.Forms.Button btnReloadAll;
    private Controls.CSListView listChangedFiles;
    private System.Windows.Forms.ColumnHeader columnHeader1;
  }
}