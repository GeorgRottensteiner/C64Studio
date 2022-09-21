using RetroDevStudio.Controls;



namespace RetroDevStudio.Dialogs
{
  partial class FormRenameFile
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
      this.label1 = new System.Windows.Forms.Label();
      this.listPETSCII = new GR.Forms.ImageListbox();
      this.editFilename = new RetroDevStudio.Controls.EditC64Filename();
      this.SuspendLayout();
      // 
      // btnCancel
      // 
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(527, 306);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // btnOK
      // 
      this.btnOK.Location = new System.Drawing.Point(446, 306);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 12);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(52, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Filename:";
      // 
      // listPETSCII
      // 
      this.listPETSCII.AutoScrollHorizontalMaximum = 100;
      this.listPETSCII.AutoScrollHorizontalMinimum = 0;
      this.listPETSCII.AutoScrollHPos = 0;
      this.listPETSCII.AutoScrollVerticalMaximum = 100;
      this.listPETSCII.AutoScrollVerticalMinimum = 0;
      this.listPETSCII.AutoScrollVPos = 0;
      this.listPETSCII.EnableAutoScrollHorizontal = true;
      this.listPETSCII.EnableAutoScrollVertical = true;
      this.listPETSCII.HottrackColor = ((uint)(2151694591u));
      this.listPETSCII.ItemHeight = 13;
      this.listPETSCII.ItemWidth = 207;
      this.listPETSCII.Location = new System.Drawing.Point(15, 58);
      this.listPETSCII.Name = "listPETSCII";
      this.listPETSCII.PixelFormat = GR.Drawing.PixelFormat.DontCare;
      this.listPETSCII.SelectedIndex = -1;
      this.listPETSCII.Size = new System.Drawing.Size(587, 233);
      this.listPETSCII.TabIndex = 3;
      this.listPETSCII.VisibleAutoScrollHorizontal = false;
      this.listPETSCII.VisibleAutoScrollVertical = false;
      this.listPETSCII.SizeChanged += new System.EventHandler(this.listPETSCII_SizeChanged);
      this.listPETSCII.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listPETSCII_MouseDoubleClick);
      // 
      // editFilename
      // 
      this.editFilename.CurrentChar = 'e';
      this.editFilename.CursorPos = 0;
      this.editFilename.Font = new System.Drawing.Font("Courier New", 9F);
      this.editFilename.LetterWidth = 18;
      this.editFilename.Location = new System.Drawing.Point(83, 9);
      this.editFilename.MaxLength = 16;
      this.editFilename.Name = "editFilename";
      this.editFilename.Selection = "";
      this.editFilename.Size = new System.Drawing.Size(288, 23);
      this.editFilename.TabIndex = 0;
      this.editFilename.Text = "editC           ";
      // 
      // FormRenameFile
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(614, 341);
      this.Controls.Add(this.listPETSCII);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.editFilename);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormRenameFile";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "Rename File";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private EditC64Filename editFilename;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Label label1;
    private GR.Forms.ImageListbox listPETSCII;
  }
}