namespace RetroDevStudio.Dialogs.Preferences
{
  partial class DlgPrefFormatting
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.checkAutoFormatActive = new System.Windows.Forms.CheckBox();
      this.checkIndentStatements = new System.Windows.Forms.CheckBox();
      this.editIndentStatements = new System.Windows.Forms.TextBox();
      this.label31 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.editIndentLabels = new System.Windows.Forms.TextBox();
      this.checkIndentLabels = new System.Windows.Forms.CheckBox();
      this.label2 = new System.Windows.Forms.Label();
      this.editIndentPseudoOps = new System.Windows.Forms.TextBox();
      this.checkIndentPseudoOps = new System.Windows.Forms.CheckBox();
      this.label3 = new System.Windows.Forms.Label();
      this.editInsertSpacesBetweenOpcodesAndArguments = new System.Windows.Forms.TextBox();
      this.checkInsertSpacesBetweenOpcodeAndParameters = new System.Windows.Forms.CheckBox();
      this.checkSeparateLineForLabels = new System.Windows.Forms.CheckBox();
      this.checkInsertSpacesBetweenOperands = new System.Windows.Forms.CheckBox();
      this.listPseudoOpsToIndent = new DecentForms.ListBox();
      this.label4 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // checkAutoFormatActive
      // 
      this.checkAutoFormatActive.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkAutoFormatActive.Location = new System.Drawing.Point(3, 8);
      this.checkAutoFormatActive.Name = "checkAutoFormatActive";
      this.checkAutoFormatActive.Size = new System.Drawing.Size(271, 17);
      this.checkAutoFormatActive.TabIndex = 0;
      this.checkAutoFormatActive.Text = "Auto Format Active";
      this.checkAutoFormatActive.UseVisualStyleBackColor = true;
      this.checkAutoFormatActive.CheckedChanged += new System.EventHandler(this.checkAutoFormatActive_CheckedChanged);
      // 
      // checkIndentStatements
      // 
      this.checkIndentStatements.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkIndentStatements.Enabled = false;
      this.checkIndentStatements.Location = new System.Drawing.Point(17, 31);
      this.checkIndentStatements.Name = "checkIndentStatements";
      this.checkIndentStatements.Size = new System.Drawing.Size(257, 17);
      this.checkIndentStatements.TabIndex = 1;
      this.checkIndentStatements.Text = "Indent Statements";
      this.checkIndentStatements.UseVisualStyleBackColor = true;
      this.checkIndentStatements.CheckedChanged += new System.EventHandler(this.checkIndentStatements_CheckedChanged);
      // 
      // editIndentStatements
      // 
      this.editIndentStatements.Enabled = false;
      this.editIndentStatements.Location = new System.Drawing.Point(280, 29);
      this.editIndentStatements.Name = "editIndentStatements";
      this.editIndentStatements.Size = new System.Drawing.Size(79, 20);
      this.editIndentStatements.TabIndex = 2;
      this.editIndentStatements.TextChanged += new System.EventHandler(this.editIndentStatements_TextChanged);
      // 
      // label31
      // 
      this.label31.AutoSize = true;
      this.label31.Enabled = false;
      this.label31.Location = new System.Drawing.Point(365, 32);
      this.label31.Name = "label31";
      this.label31.Size = new System.Drawing.Size(27, 13);
      this.label31.TabIndex = 20;
      this.label31.Text = "tabs";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Enabled = false;
      this.label1.Location = new System.Drawing.Point(365, 58);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(27, 13);
      this.label1.TabIndex = 20;
      this.label1.Text = "tabs";
      // 
      // editIndentLabels
      // 
      this.editIndentLabels.Enabled = false;
      this.editIndentLabels.Location = new System.Drawing.Point(280, 55);
      this.editIndentLabels.Name = "editIndentLabels";
      this.editIndentLabels.Size = new System.Drawing.Size(79, 20);
      this.editIndentLabels.TabIndex = 4;
      this.editIndentLabels.TextChanged += new System.EventHandler(this.editIndentLabels_TextChanged);
      // 
      // checkIndentLabels
      // 
      this.checkIndentLabels.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkIndentLabels.Enabled = false;
      this.checkIndentLabels.Location = new System.Drawing.Point(17, 57);
      this.checkIndentLabels.Name = "checkIndentLabels";
      this.checkIndentLabels.Size = new System.Drawing.Size(257, 17);
      this.checkIndentLabels.TabIndex = 3;
      this.checkIndentLabels.Text = "Indent Labels";
      this.checkIndentLabels.UseVisualStyleBackColor = true;
      this.checkIndentLabels.CheckedChanged += new System.EventHandler(this.checkIndentLabels_CheckedChanged);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Enabled = false;
      this.label2.Location = new System.Drawing.Point(365, 84);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(27, 13);
      this.label2.TabIndex = 20;
      this.label2.Text = "tabs";
      // 
      // editIndentPseudoOps
      // 
      this.editIndentPseudoOps.Enabled = false;
      this.editIndentPseudoOps.Location = new System.Drawing.Point(280, 81);
      this.editIndentPseudoOps.Name = "editIndentPseudoOps";
      this.editIndentPseudoOps.Size = new System.Drawing.Size(79, 20);
      this.editIndentPseudoOps.TabIndex = 6;
      this.editIndentPseudoOps.TextChanged += new System.EventHandler(this.editIndentPseudoOps_TextChanged);
      // 
      // checkIndentPseudoOps
      // 
      this.checkIndentPseudoOps.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkIndentPseudoOps.Enabled = false;
      this.checkIndentPseudoOps.Location = new System.Drawing.Point(17, 83);
      this.checkIndentPseudoOps.Name = "checkIndentPseudoOps";
      this.checkIndentPseudoOps.Size = new System.Drawing.Size(257, 17);
      this.checkIndentPseudoOps.TabIndex = 5;
      this.checkIndentPseudoOps.Text = "Indent Pseudo Ops";
      this.checkIndentPseudoOps.UseVisualStyleBackColor = true;
      this.checkIndentPseudoOps.CheckedChanged += new System.EventHandler(this.checkIndentPseudoOps_CheckedChanged);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Enabled = false;
      this.label3.Location = new System.Drawing.Point(369, 271);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(41, 13);
      this.label3.TabIndex = 20;
      this.label3.Text = "spaces";
      // 
      // editInsertSpacesBetweenOpcodesAndArguments
      // 
      this.editInsertSpacesBetweenOpcodesAndArguments.Enabled = false;
      this.editInsertSpacesBetweenOpcodesAndArguments.Location = new System.Drawing.Point(284, 268);
      this.editInsertSpacesBetweenOpcodesAndArguments.Name = "editInsertSpacesBetweenOpcodesAndArguments";
      this.editInsertSpacesBetweenOpcodesAndArguments.Size = new System.Drawing.Size(79, 20);
      this.editInsertSpacesBetweenOpcodesAndArguments.TabIndex = 9;
      this.editInsertSpacesBetweenOpcodesAndArguments.TextChanged += new System.EventHandler(this.editInsertSpacesBetweenOpcodesAndArguments_TextChanged);
      // 
      // checkInsertSpacesBetweenOpcodeAndParameters
      // 
      this.checkInsertSpacesBetweenOpcodeAndParameters.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkInsertSpacesBetweenOpcodeAndParameters.Enabled = false;
      this.checkInsertSpacesBetweenOpcodeAndParameters.Location = new System.Drawing.Point(21, 270);
      this.checkInsertSpacesBetweenOpcodeAndParameters.Name = "checkInsertSpacesBetweenOpcodeAndParameters";
      this.checkInsertSpacesBetweenOpcodeAndParameters.Size = new System.Drawing.Size(257, 17);
      this.checkInsertSpacesBetweenOpcodeAndParameters.TabIndex = 8;
      this.checkInsertSpacesBetweenOpcodeAndParameters.Text = "Insert spaces between opcode and arguments";
      this.checkInsertSpacesBetweenOpcodeAndParameters.UseVisualStyleBackColor = true;
      this.checkInsertSpacesBetweenOpcodeAndParameters.CheckedChanged += new System.EventHandler(this.checkInsertSpacesBetweenOpcodeAndParameters_CheckedChanged);
      // 
      // checkSeparateLineForLabels
      // 
      this.checkSeparateLineForLabels.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkSeparateLineForLabels.Enabled = false;
      this.checkSeparateLineForLabels.Location = new System.Drawing.Point(21, 322);
      this.checkSeparateLineForLabels.Name = "checkSeparateLineForLabels";
      this.checkSeparateLineForLabels.Size = new System.Drawing.Size(257, 17);
      this.checkSeparateLineForLabels.TabIndex = 11;
      this.checkSeparateLineForLabels.Text = "Put Labels on separate line";
      this.checkSeparateLineForLabels.UseVisualStyleBackColor = true;
      this.checkSeparateLineForLabels.CheckedChanged += new System.EventHandler(this.checkSeparateLineForLabels_CheckedChanged);
      // 
      // checkInsertSpacesBetweenOperands
      // 
      this.checkInsertSpacesBetweenOperands.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkInsertSpacesBetweenOperands.Enabled = false;
      this.checkInsertSpacesBetweenOperands.Location = new System.Drawing.Point(21, 296);
      this.checkInsertSpacesBetweenOperands.Name = "checkInsertSpacesBetweenOperands";
      this.checkInsertSpacesBetweenOperands.Size = new System.Drawing.Size(257, 17);
      this.checkInsertSpacesBetweenOperands.TabIndex = 10;
      this.checkInsertSpacesBetweenOperands.Text = "Insert spaces between operands";
      this.checkInsertSpacesBetweenOperands.UseVisualStyleBackColor = true;
      this.checkInsertSpacesBetweenOperands.CheckedChanged += new System.EventHandler(this.checkInsertSpacesBetweenOperands_CheckedChanged);
      // 
      // listPseudoOpsToIndent
      // 
      this.listPseudoOpsToIndent.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.listPseudoOpsToIndent.HasCheckBoxes = true;
      this.listPseudoOpsToIndent.ItemHeight = 15;
      this.listPseudoOpsToIndent.Location = new System.Drawing.Point(36, 125);
      this.listPseudoOpsToIndent.Name = "listPseudoOpsToIndent";
      this.listPseudoOpsToIndent.ScrollAlwaysVisible = false;
      this.listPseudoOpsToIndent.SelectedIndex = -1;
      this.listPseudoOpsToIndent.SelectedItem = null;
      this.listPseudoOpsToIndent.SelectionMode = DecentForms.SelectionMode.NONE;
      this.listPseudoOpsToIndent.Size = new System.Drawing.Size(323, 139);
      this.listPseudoOpsToIndent.TabIndex = 7;
      this.listPseudoOpsToIndent.Text = "listBox1";
      this.listPseudoOpsToIndent.CheckChanged += new DecentForms.EventHandler(this.listPseudoOpsToIndent_CheckChanged);
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Enabled = false;
      this.label4.Location = new System.Drawing.Point(18, 109);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(171, 13);
      this.label4.TabIndex = 20;
      this.label4.Text = "Indent Pseudo Ops like statements";
      // 
      // DlgPrefFormatting
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.listPseudoOpsToIndent);
      this.Controls.Add(this.checkAutoFormatActive);
      this.Controls.Add(this.checkInsertSpacesBetweenOperands);
      this.Controls.Add(this.checkInsertSpacesBetweenOpcodeAndParameters);
      this.Controls.Add(this.checkIndentPseudoOps);
      this.Controls.Add(this.checkSeparateLineForLabels);
      this.Controls.Add(this.checkIndentLabels);
      this.Controls.Add(this.editInsertSpacesBetweenOpcodesAndArguments);
      this.Controls.Add(this.editIndentPseudoOps);
      this.Controls.Add(this.checkIndentStatements);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.editIndentLabels);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.editIndentStatements);
      this.Controls.Add(this.label31);
      this.Name = "DlgPrefFormatting";
      this.Size = new System.Drawing.Size(577, 420);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

        #endregion
    private System.Windows.Forms.TextBox editIndentStatements;
    private System.Windows.Forms.Label label31;
    private System.Windows.Forms.CheckBox checkIndentStatements;
    private System.Windows.Forms.CheckBox checkAutoFormatActive;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox editIndentLabels;
    private System.Windows.Forms.CheckBox checkIndentLabels;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox editIndentPseudoOps;
    private System.Windows.Forms.CheckBox checkIndentPseudoOps;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox editInsertSpacesBetweenOpcodesAndArguments;
    private System.Windows.Forms.CheckBox checkInsertSpacesBetweenOpcodeAndParameters;
    private System.Windows.Forms.CheckBox checkSeparateLineForLabels;
    private System.Windows.Forms.CheckBox checkInsertSpacesBetweenOperands;
    private DecentForms.ListBox listPseudoOpsToIndent;
    private System.Windows.Forms.Label label4;
  }
}
