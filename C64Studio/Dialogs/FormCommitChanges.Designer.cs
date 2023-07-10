using RetroDevStudio.Controls;

namespace RetroDevStudio.Dialogs
{
  partial class FormCommitChanges
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
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
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.editCommitMessage = new System.Windows.Forms.TextBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.listCommitFiles = new RetroDevStudio.Controls.CSListView();
      this.columnCheck = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.editCommitAuthor = new System.Windows.Forms.TextBox();
      this.groupBox4 = new System.Windows.Forms.GroupBox();
      this.editCommitEmail = new System.Windows.Forms.TextBox();
      this.columnExtension = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.groupBox4.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(499, 502);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // btnOK
      // 
      this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnOK.Location = new System.Drawing.Point(418, 502);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 4;
      this.btnOK.Text = "Commit";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox1.Controls.Add(this.editCommitMessage);
      this.groupBox1.Location = new System.Drawing.Point(12, 59);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(562, 147);
      this.groupBox1.TabIndex = 2;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Commit Message";
      // 
      // editCommitMessage
      // 
      this.editCommitMessage.AcceptsReturn = true;
      this.editCommitMessage.AcceptsTab = true;
      this.editCommitMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.editCommitMessage.Location = new System.Drawing.Point(6, 19);
      this.editCommitMessage.Multiline = true;
      this.editCommitMessage.Name = "editCommitMessage";
      this.editCommitMessage.Size = new System.Drawing.Size(550, 122);
      this.editCommitMessage.TabIndex = 0;
      this.editCommitMessage.TextChanged += new System.EventHandler(this.editCommitMessage_TextChanged);
      // 
      // groupBox2
      // 
      this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox2.Controls.Add(this.listCommitFiles);
      this.groupBox2.Location = new System.Drawing.Point(12, 212);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(562, 284);
      this.groupBox2.TabIndex = 3;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Files to Commit";
      // 
      // listCommitFiles
      // 
      this.listCommitFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.listCommitFiles.CheckBoxes = true;
      this.listCommitFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnCheck,
            this.columnType,
            this.columnFile,
            this.columnExtension});
      this.listCommitFiles.FullRowSelect = true;
      this.listCommitFiles.HideSelection = false;
      this.listCommitFiles.Location = new System.Drawing.Point(6, 19);
      this.listCommitFiles.Name = "listCommitFiles";
      this.listCommitFiles.OwnerDraw = true;
      this.listCommitFiles.SelectedTextBGColor = ((uint)(4278190335u));
      this.listCommitFiles.SelectedTextColor = ((uint)(4294967295u));
      this.listCommitFiles.Size = new System.Drawing.Size(550, 259);
      this.listCommitFiles.Sorting = System.Windows.Forms.SortOrder.Ascending;
      this.listCommitFiles.TabIndex = 0;
      this.listCommitFiles.UseCompatibleStateImageBehavior = false;
      this.listCommitFiles.View = System.Windows.Forms.View.Details;
      this.listCommitFiles.DrawItemImage += new RetroDevStudio.Controls.CSListView.DrawItemImageHandler(this.listCommitFiles_DrawItemImage);
      this.listCommitFiles.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listCommitFiles_ColumnClick);
      this.listCommitFiles.ColumnWidthChanging += new System.Windows.Forms.ColumnWidthChangingEventHandler(this.listCommitFiles_ColumnWidthChanging);
      // 
      // columnCheck
      // 
      this.columnCheck.Text = ".";
      this.columnCheck.Width = 24;
      // 
      // columnType
      // 
      this.columnType.Text = ".";
      this.columnType.Width = 24;
      // 
      // columnFile
      // 
      this.columnFile.Text = "Filename";
      this.columnFile.Width = 400;
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.editCommitAuthor);
      this.groupBox3.Location = new System.Drawing.Point(12, 13);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(270, 40);
      this.groupBox3.TabIndex = 0;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Author";
      // 
      // editCommitAuthor
      // 
      this.editCommitAuthor.Location = new System.Drawing.Point(6, 14);
      this.editCommitAuthor.Name = "editCommitAuthor";
      this.editCommitAuthor.Size = new System.Drawing.Size(258, 20);
      this.editCommitAuthor.TabIndex = 0;
      this.editCommitAuthor.TextChanged += new System.EventHandler(this.editCommitAuthor_TextChanged);
      // 
      // groupBox4
      // 
      this.groupBox4.Controls.Add(this.editCommitEmail);
      this.groupBox4.Location = new System.Drawing.Point(288, 13);
      this.groupBox4.Name = "groupBox4";
      this.groupBox4.Size = new System.Drawing.Size(286, 40);
      this.groupBox4.TabIndex = 1;
      this.groupBox4.TabStop = false;
      this.groupBox4.Text = "Email";
      // 
      // editCommitEmail
      // 
      this.editCommitEmail.Location = new System.Drawing.Point(6, 14);
      this.editCommitEmail.Name = "editCommitEmail";
      this.editCommitEmail.Size = new System.Drawing.Size(274, 20);
      this.editCommitEmail.TabIndex = 0;
      this.editCommitEmail.TextChanged += new System.EventHandler(this.editCommitEmail_TextChanged);
      // 
      // columnExtension
      // 
      this.columnExtension.Text = "Extension";
      // 
      // FormCommitChanges
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(586, 537);
      this.Controls.Add(this.groupBox4);
      this.Controls.Add(this.groupBox3);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnCancel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormCommitChanges";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Commit Changes";
      this.Load += new System.EventHandler(this.FormCommitChanges_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.groupBox4.ResumeLayout(false);
      this.groupBox4.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.TextBox editCommitMessage;
    private System.Windows.Forms.GroupBox groupBox2;
    private CSListView listCommitFiles;
    private System.Windows.Forms.ColumnHeader columnType;
    private System.Windows.Forms.ColumnHeader columnFile;
    private System.Windows.Forms.ColumnHeader columnCheck;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.TextBox editCommitAuthor;
    private System.Windows.Forms.GroupBox groupBox4;
    private System.Windows.Forms.TextBox editCommitEmail;
    private System.Windows.Forms.ColumnHeader columnExtension;
  }
}