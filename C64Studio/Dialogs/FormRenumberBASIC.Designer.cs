namespace RetroDevStudio.Dialogs
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
      this.btnCancel = new DecentForms.Button();
      this.btnOK = new DecentForms.Button();
      this.label3 = new System.Windows.Forms.Label();
      this.editFirstLineNumber = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.editLastLineNumber = new System.Windows.Forms.TextBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.checkAffectOnlyLineNumbers = new System.Windows.Forms.CheckBox();
      this.checkVerifyPlausibility = new System.Windows.Forms.CheckBox();
      this.labelFirstLineIndex = new System.Windows.Forms.Label();
      this.labelLastLineIndex = new System.Windows.Forms.Label();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox3.SuspendLayout();
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
      this.editStartLine.Size = new System.Drawing.Size(100, 20);
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
      this.editLineStep.Size = new System.Drawing.Size(100, 20);
      this.editLineStep.TabIndex = 1;
      this.editLineStep.TextChanged += new System.EventHandler(this.editLineStep_TextChanged);
      // 
      // labelRenumberInfo
      // 
      this.labelRenumberInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.labelRenumberInfo.Location = new System.Drawing.Point(10, 268);
      this.labelRenumberInfo.Name = "labelRenumberInfo";
      this.labelRenumberInfo.Size = new System.Drawing.Size(402, 87);
      this.labelRenumberInfo.TabIndex = 2;
      this.labelRenumberInfo.Text = "label3";
      // 
      // btnCancel
      // 
      this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnCancel.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Image = null;
      this.btnCancel.Location = new System.Drawing.Point(333, 358);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.Click += new DecentForms.EventHandler(this.btnCancel_Click);
      // 
      // btnOK
      // 
      this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnOK.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnOK.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.Enabled = false;
      this.btnOK.Image = null;
      this.btnOK.Location = new System.Drawing.Point(252, 358);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "&OK";
      this.btnOK.Click += new DecentForms.EventHandler(this.btnOK_Click);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(11, 21);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(86, 13);
      this.label3.TabIndex = 0;
      this.label3.Text = "First line number:";
      // 
      // editFirstLineNumber
      // 
      this.editFirstLineNumber.Location = new System.Drawing.Point(128, 18);
      this.editFirstLineNumber.Name = "editFirstLineNumber";
      this.editFirstLineNumber.Size = new System.Drawing.Size(100, 20);
      this.editFirstLineNumber.TabIndex = 0;
      this.editFirstLineNumber.TextChanged += new System.EventHandler(this.editFirstLineNumber_TextChanged);
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(11, 47);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(87, 13);
      this.label4.TabIndex = 0;
      this.label4.Text = "Last line number:";
      // 
      // editLastLineNumber
      // 
      this.editLastLineNumber.Location = new System.Drawing.Point(128, 44);
      this.editLastLineNumber.Name = "editLastLineNumber";
      this.editLastLineNumber.Size = new System.Drawing.Size(100, 20);
      this.editLastLineNumber.TabIndex = 1;
      this.editLastLineNumber.TextChanged += new System.EventHandler(this.editLastLineNumber_TextChanged);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.editStartLine);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Controls.Add(this.editLineStep);
      this.groupBox1.Location = new System.Drawing.Point(10, 12);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(402, 84);
      this.groupBox1.TabIndex = 4;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Renumber Settings";
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.labelLastLineIndex);
      this.groupBox2.Controls.Add(this.labelFirstLineIndex);
      this.groupBox2.Controls.Add(this.label3);
      this.groupBox2.Controls.Add(this.editFirstLineNumber);
      this.groupBox2.Controls.Add(this.label4);
      this.groupBox2.Controls.Add(this.editLastLineNumber);
      this.groupBox2.Location = new System.Drawing.Point(10, 102);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(402, 77);
      this.groupBox2.TabIndex = 5;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Renumber between";
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.checkVerifyPlausibility);
      this.groupBox3.Controls.Add(this.checkAffectOnlyLineNumbers);
      this.groupBox3.Location = new System.Drawing.Point(10, 185);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(402, 80);
      this.groupBox3.TabIndex = 6;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Renumber Options";
      // 
      // checkAffectOnlyLineNumbers
      // 
      this.checkAffectOnlyLineNumbers.AutoSize = true;
      this.checkAffectOnlyLineNumbers.Location = new System.Drawing.Point(14, 19);
      this.checkAffectOnlyLineNumbers.Name = "checkAffectOnlyLineNumbers";
      this.checkAffectOnlyLineNumbers.Size = new System.Drawing.Size(176, 17);
      this.checkAffectOnlyLineNumbers.TabIndex = 0;
      this.checkAffectOnlyLineNumbers.Text = "Change only direct line numbers";
      this.checkAffectOnlyLineNumbers.UseVisualStyleBackColor = true;
      this.checkAffectOnlyLineNumbers.CheckedChanged += new System.EventHandler(this.checkAffectOnlyLineNumbers_CheckedChanged);
      // 
      // checkVerifyPlausibility
      // 
      this.checkVerifyPlausibility.AutoSize = true;
      this.checkVerifyPlausibility.Checked = true;
      this.checkVerifyPlausibility.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkVerifyPlausibility.Location = new System.Drawing.Point(14, 42);
      this.checkVerifyPlausibility.Name = "checkVerifyPlausibility";
      this.checkVerifyPlausibility.Size = new System.Drawing.Size(127, 17);
      this.checkVerifyPlausibility.TabIndex = 0;
      this.checkVerifyPlausibility.Text = "Verify all line numbers";
      this.checkVerifyPlausibility.UseVisualStyleBackColor = true;
      this.checkVerifyPlausibility.CheckedChanged += new System.EventHandler(this.checkVerifyPlausibility_CheckedChanged);
      // 
      // labelFirstLineIndex
      // 
      this.labelFirstLineIndex.Location = new System.Drawing.Point(259, 21);
      this.labelFirstLineIndex.Name = "labelFirstLineIndex";
      this.labelFirstLineIndex.Size = new System.Drawing.Size(137, 23);
      this.labelFirstLineIndex.TabIndex = 2;
      this.labelFirstLineIndex.Text = "label5";
      // 
      // labelLastLineIndex
      // 
      this.labelLastLineIndex.Location = new System.Drawing.Point(259, 47);
      this.labelLastLineIndex.Name = "labelLastLineIndex";
      this.labelLastLineIndex.Size = new System.Drawing.Size(137, 23);
      this.labelLastLineIndex.TabIndex = 2;
      this.labelLastLineIndex.Text = "label5";
      // 
      // FormRenumberBASIC
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(420, 393);
      this.Controls.Add(this.groupBox3);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.labelRenumberInfo);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormRenumberBASIC";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "Renumber BASIC listing";
      this.Load += new System.EventHandler(this.FormRenumberBASIC_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox editStartLine;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox editLineStep;
    private System.Windows.Forms.Label labelRenumberInfo;
    private DecentForms.Button btnCancel;
    private DecentForms.Button btnOK;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox editFirstLineNumber;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox editLastLineNumber;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.CheckBox checkAffectOnlyLineNumbers;
    private System.Windows.Forms.CheckBox checkVerifyPlausibility;
    private System.Windows.Forms.Label labelLastLineIndex;
    private System.Windows.Forms.Label labelFirstLineIndex;
  }
}