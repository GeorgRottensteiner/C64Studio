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
      this.SuspendLayout();
      // 
      // checkAutoFormatActive
      // 
      this.checkAutoFormatActive.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkAutoFormatActive.Location = new System.Drawing.Point(3, 8);
      this.checkAutoFormatActive.Name = "checkAutoFormatActive";
      this.checkAutoFormatActive.Size = new System.Drawing.Size(271, 17);
      this.checkAutoFormatActive.TabIndex = 22;
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
      this.checkIndentStatements.TabIndex = 22;
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
      this.editIndentStatements.TabIndex = 17;
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
      this.editIndentLabels.TabIndex = 17;
      this.editIndentLabels.TextChanged += new System.EventHandler(this.editIndentLabels_TextChanged);
      // 
      // checkIndentLabels
      // 
      this.checkIndentLabels.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkIndentLabels.Enabled = false;
      this.checkIndentLabels.Location = new System.Drawing.Point(17, 57);
      this.checkIndentLabels.Name = "checkIndentLabels";
      this.checkIndentLabels.Size = new System.Drawing.Size(257, 17);
      this.checkIndentLabels.TabIndex = 22;
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
      this.editIndentPseudoOps.TabIndex = 17;
      this.editIndentPseudoOps.TextChanged += new System.EventHandler(this.editIndentPseudoOps_TextChanged);
      // 
      // checkIndentPseudoOps
      // 
      this.checkIndentPseudoOps.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkIndentPseudoOps.Enabled = false;
      this.checkIndentPseudoOps.Location = new System.Drawing.Point(17, 83);
      this.checkIndentPseudoOps.Name = "checkIndentPseudoOps";
      this.checkIndentPseudoOps.Size = new System.Drawing.Size(257, 17);
      this.checkIndentPseudoOps.TabIndex = 22;
      this.checkIndentPseudoOps.Text = "Indent Pseudo Ops";
      this.checkIndentPseudoOps.UseVisualStyleBackColor = true;
      this.checkIndentPseudoOps.CheckedChanged += new System.EventHandler(this.checkIndentPseudoOps_CheckedChanged);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Enabled = false;
      this.label3.Location = new System.Drawing.Point(365, 110);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(41, 13);
      this.label3.TabIndex = 20;
      this.label3.Text = "spaces";
      // 
      // editInsertSpacesBetweenOpcodesAndArguments
      // 
      this.editInsertSpacesBetweenOpcodesAndArguments.Enabled = false;
      this.editInsertSpacesBetweenOpcodesAndArguments.Location = new System.Drawing.Point(280, 107);
      this.editInsertSpacesBetweenOpcodesAndArguments.Name = "editInsertSpacesBetweenOpcodesAndArguments";
      this.editInsertSpacesBetweenOpcodesAndArguments.Size = new System.Drawing.Size(79, 20);
      this.editInsertSpacesBetweenOpcodesAndArguments.TabIndex = 17;
      this.editInsertSpacesBetweenOpcodesAndArguments.TextChanged += new System.EventHandler(this.editInsertSpacesBetweenOpcodesAndArguments_TextChanged);
      // 
      // checkInsertSpacesBetweenOpcodeAndParameters
      // 
      this.checkInsertSpacesBetweenOpcodeAndParameters.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkInsertSpacesBetweenOpcodeAndParameters.Enabled = false;
      this.checkInsertSpacesBetweenOpcodeAndParameters.Location = new System.Drawing.Point(17, 109);
      this.checkInsertSpacesBetweenOpcodeAndParameters.Name = "checkInsertSpacesBetweenOpcodeAndParameters";
      this.checkInsertSpacesBetweenOpcodeAndParameters.Size = new System.Drawing.Size(257, 17);
      this.checkInsertSpacesBetweenOpcodeAndParameters.TabIndex = 22;
      this.checkInsertSpacesBetweenOpcodeAndParameters.Text = "Insert spaces between opcode and arguments";
      this.checkInsertSpacesBetweenOpcodeAndParameters.UseVisualStyleBackColor = true;
      this.checkInsertSpacesBetweenOpcodeAndParameters.CheckedChanged += new System.EventHandler(this.checkInsertSpacesBetweenOpcodeAndParameters_CheckedChanged);
      // 
      // checkSeparateLineForLabels
      // 
      this.checkSeparateLineForLabels.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkSeparateLineForLabels.Enabled = false;
      this.checkSeparateLineForLabels.Location = new System.Drawing.Point(17, 161);
      this.checkSeparateLineForLabels.Name = "checkSeparateLineForLabels";
      this.checkSeparateLineForLabels.Size = new System.Drawing.Size(257, 17);
      this.checkSeparateLineForLabels.TabIndex = 22;
      this.checkSeparateLineForLabels.Text = "Put Labels on separate line";
      this.checkSeparateLineForLabels.UseVisualStyleBackColor = true;
      this.checkSeparateLineForLabels.CheckedChanged += new System.EventHandler(this.checkSeparateLineForLabels_CheckedChanged);
      // 
      // checkInsertSpacesBetweenOperands
      // 
      this.checkInsertSpacesBetweenOperands.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkInsertSpacesBetweenOperands.Enabled = false;
      this.checkInsertSpacesBetweenOperands.Location = new System.Drawing.Point(17, 135);
      this.checkInsertSpacesBetweenOperands.Name = "checkInsertSpacesBetweenOperands";
      this.checkInsertSpacesBetweenOperands.Size = new System.Drawing.Size(257, 17);
      this.checkInsertSpacesBetweenOperands.TabIndex = 22;
      this.checkInsertSpacesBetweenOperands.Text = "Insert spaces between operands";
      this.checkInsertSpacesBetweenOperands.UseVisualStyleBackColor = true;
      this.checkInsertSpacesBetweenOperands.CheckedChanged += new System.EventHandler(this.checkInsertSpacesBetweenOperands_CheckedChanged);
      // 
      // DlgPrefFormatting
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.checkAutoFormatActive);
      this.Controls.Add(this.checkInsertSpacesBetweenOperands);
      this.Controls.Add(this.checkInsertSpacesBetweenOpcodeAndParameters);
      this.Controls.Add(this.checkIndentPseudoOps);
      this.Controls.Add(this.checkSeparateLineForLabels);
      this.Controls.Add(this.checkIndentLabels);
      this.Controls.Add(this.editInsertSpacesBetweenOpcodesAndArguments);
      this.Controls.Add(this.editIndentPseudoOps);
      this.Controls.Add(this.checkIndentStatements);
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
  }
}
