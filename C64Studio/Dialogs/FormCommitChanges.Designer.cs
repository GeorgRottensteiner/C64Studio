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
      this.btnCancel = new DecentForms.Button();
      this.btnOK = new DecentForms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.editCommitMessage = new System.Windows.Forms.TextBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.listCommitFiles = new DecentForms.ListControl();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.editCommitAuthor = new System.Windows.Forms.TextBox();
      this.groupBox4 = new System.Windows.Forms.GroupBox();
      this.editCommitEmail = new System.Windows.Forms.TextBox();
      this.checkShowUnversionedFiles = new DecentForms.CheckBox();
      this.checkShowUnmodifiedFiles = new DecentForms.CheckBox();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.groupBox4.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnCancel
      // 
      this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnCancel.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Image = null;
      this.btnCancel.Location = new System.Drawing.Point(499, 502);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.Click += new DecentForms.EventHandler(this.btnCancel_Click);
      // 
      // btnOK
      // 
      this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnOK.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnOK.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.Image = null;
      this.btnOK.Location = new System.Drawing.Point(418, 502);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 4;
      this.btnOK.Text = "Commit";
      this.btnOK.Click += new DecentForms.EventHandler(this.btnOK_Click);
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
      this.listCommitFiles.BorderStyle = DecentForms.BorderStyle.SUNKEN;
      this.listCommitFiles.CheckBoxes = true;
      this.listCommitFiles.FirstVisibleItemIndex = 0;
      this.listCommitFiles.HasHeader = true;
      this.listCommitFiles.HeaderHeight = 24;
      this.listCommitFiles.ImageList = null;
      this.listCommitFiles.ItemHeight = 15;
      this.listCommitFiles.ListViewItemSorter = null;
      this.listCommitFiles.Location = new System.Drawing.Point(6, 19);
      this.listCommitFiles.MultiSelected = false;
      this.listCommitFiles.Name = "listCommitFiles";
      this.listCommitFiles.ScrollAlwaysVisible = false;
      this.listCommitFiles.SelectedIndex = -1;
      this.listCommitFiles.SelectedItem = null;
      this.listCommitFiles.SelectionMode = DecentForms.SelectionMode.NONE;
      this.listCommitFiles.Size = new System.Drawing.Size(550, 259);
      this.listCommitFiles.SortColumn = 0;
      this.listCommitFiles.SortOrder = DecentForms.SortOrder.NONE;
      this.listCommitFiles.TabIndex = 0;
      this.listCommitFiles.ColumnClicked += new DecentForms.EventHandler(this.listCommitFiles_ColumnClick);
      this.listCommitFiles.MouseClick += new DecentForms.EventHandlerWithEvent(this.listCommitFiles_MouseClick);
      this.listCommitFiles.DrawItemImage += new DecentForms.ListControl.ListControlDrawItemImageHandler(this.listCommitFiles_DrawItemImage);
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
      // checkShowUnversionedFiles
      // 
      this.checkShowUnversionedFiles.Appearance = System.Windows.Forms.Appearance.Normal;
      this.checkShowUnversionedFiles.BorderStyle = DecentForms.BorderStyle.NONE;
      this.checkShowUnversionedFiles.CheckAlign = DecentForms.ContentAlignment.MiddleLeft;
      this.checkShowUnversionedFiles.Checked = true;
      this.checkShowUnversionedFiles.Image = null;
      this.checkShowUnversionedFiles.Location = new System.Drawing.Point(12, 502);
      this.checkShowUnversionedFiles.Name = "checkShowUnversionedFiles";
      this.checkShowUnversionedFiles.Size = new System.Drawing.Size(151, 23);
      this.checkShowUnversionedFiles.TabIndex = 6;
      this.checkShowUnversionedFiles.Text = "Show unversioned files";
      this.checkShowUnversionedFiles.CheckedChanged += new DecentForms.EventHandler(this.checkShowUnversionedFiles_CheckedChanged);
      // 
      // checkShowUnmodifiedFiles
      // 
      this.checkShowUnmodifiedFiles.Appearance = System.Windows.Forms.Appearance.Normal;
      this.checkShowUnmodifiedFiles.BorderStyle = DecentForms.BorderStyle.NONE;
      this.checkShowUnmodifiedFiles.CheckAlign = DecentForms.ContentAlignment.MiddleLeft;
      this.checkShowUnmodifiedFiles.Checked = false;
      this.checkShowUnmodifiedFiles.Image = null;
      this.checkShowUnmodifiedFiles.Location = new System.Drawing.Point(180, 502);
      this.checkShowUnmodifiedFiles.Name = "checkShowUnmodifiedFiles";
      this.checkShowUnmodifiedFiles.Size = new System.Drawing.Size(151, 23);
      this.checkShowUnmodifiedFiles.TabIndex = 6;
      this.checkShowUnmodifiedFiles.Text = "Show unmodified files";
      this.checkShowUnmodifiedFiles.CheckedChanged += new DecentForms.EventHandler(this.checkShowUnmodifiedFiles_CheckedChanged);
      // 
      // FormCommitChanges
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(586, 537);
      this.Controls.Add(this.checkShowUnmodifiedFiles);
      this.Controls.Add(this.checkShowUnversionedFiles);
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
    private DecentForms.Button btnCancel;
    private DecentForms.Button btnOK;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.TextBox editCommitMessage;
    private System.Windows.Forms.GroupBox groupBox2;
    private DecentForms.ListControl listCommitFiles;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.TextBox editCommitAuthor;
    private System.Windows.Forms.GroupBox groupBox4;
    private System.Windows.Forms.TextBox editCommitEmail;
    private DecentForms.CheckBox checkShowUnversionedFiles;
    private DecentForms.CheckBox checkShowUnmodifiedFiles;
  }
}