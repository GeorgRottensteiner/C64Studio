namespace RetroDevStudio.Dialogs
{
  partial class FormRenumberBASICLabelMode
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
      this.editStartLine = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.editLineStep = new System.Windows.Forms.TextBox();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.labelRenumberInfo = new System.Windows.Forms.Label();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(11, 22);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(111, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Start with line number:";
      // 
      // editStartLine
      // 
      this.editStartLine.Location = new System.Drawing.Point(128, 19);
      this.editStartLine.Name = "editStartLine";
      this.editStartLine.Size = new System.Drawing.Size(136, 20);
      this.editStartLine.TabIndex = 0;
      this.editStartLine.TextChanged += new System.EventHandler(this.editStartLine_TextChanged);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(11, 48);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(100, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "Step by Line Count:";
      // 
      // editLineStep
      // 
      this.editLineStep.Location = new System.Drawing.Point(128, 45);
      this.editLineStep.Name = "editLineStep";
      this.editLineStep.Size = new System.Drawing.Size(136, 20);
      this.editLineStep.TabIndex = 1;
      this.editLineStep.TextChanged += new System.EventHandler(this.editLineStep_TextChanged);
      // 
      // btnCancel
      // 
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(219, 169);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // btnOK
      // 
      this.btnOK.Enabled = false;
      this.btnOK.Location = new System.Drawing.Point(138, 169);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.editStartLine);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Controls.Add(this.editLineStep);
      this.groupBox1.Location = new System.Drawing.Point(10, 12);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(284, 84);
      this.groupBox1.TabIndex = 4;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Renumber Settings";
      // 
      // labelRenumberInfo
      // 
      this.labelRenumberInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.labelRenumberInfo.Location = new System.Drawing.Point(10, 105);
      this.labelRenumberInfo.Name = "labelRenumberInfo";
      this.labelRenumberInfo.Size = new System.Drawing.Size(282, 61);
      this.labelRenumberInfo.TabIndex = 5;
      this.labelRenumberInfo.Text = "label3";
      // 
      // FormRenumberBASICLabelMode
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(304, 204);
      this.Controls.Add(this.labelRenumberInfo);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnCancel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormRenumberBASICLabelMode";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "Auto Renumber settings for label mode";
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox editStartLine;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox editLineStep;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Label labelRenumberInfo;
  }
}