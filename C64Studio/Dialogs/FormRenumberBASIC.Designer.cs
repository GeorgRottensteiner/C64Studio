namespace C64Studio
{
  partial class FormRenumberBASIC
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
      this.labelRenumberInfo = new System.Windows.Forms.Label();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(3, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(111, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Start with line number:";
      // 
      // editStartLine
      // 
      this.editStartLine.Location = new System.Drawing.Point(120, 6);
      this.editStartLine.Name = "editStartLine";
      this.editStartLine.Size = new System.Drawing.Size(100, 20);
      this.editStartLine.TabIndex = 0;
      this.editStartLine.TextChanged += new System.EventHandler(this.editStartLine_TextChanged);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(3, 35);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(100, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "Step by Line Count:";
      // 
      // editLineStep
      // 
      this.editLineStep.Location = new System.Drawing.Point(120, 32);
      this.editLineStep.Name = "editLineStep";
      this.editLineStep.Size = new System.Drawing.Size(100, 20);
      this.editLineStep.TabIndex = 1;
      this.editLineStep.TextChanged += new System.EventHandler(this.editLineStep_TextChanged);
      // 
      // labelRenumberInfo
      // 
      this.labelRenumberInfo.Location = new System.Drawing.Point(7, 85);
      this.labelRenumberInfo.Name = "labelRenumberInfo";
      this.labelRenumberInfo.Size = new System.Drawing.Size(212, 87);
      this.labelRenumberInfo.TabIndex = 2;
      this.labelRenumberInfo.Text = "label3";
      // 
      // btnCancel
      // 
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(145, 189);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // btnOK
      // 
      this.btnOK.Enabled = false;
      this.btnOK.Location = new System.Drawing.Point(64, 189);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // FormRenumberBASIC
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(226, 224);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.labelRenumberInfo);
      this.Controls.Add(this.editLineStep);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.editStartLine);
      this.Controls.Add(this.label1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormRenumberBASIC";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "Renumber BASIC listing";
      this.Load += new System.EventHandler(this.FormRenumberBASIC_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox editStartLine;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox editLineStep;
    private System.Windows.Forms.Label labelRenumberInfo;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnOK;
  }
}