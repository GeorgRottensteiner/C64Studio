namespace RetroDevStudio.Documents
{
  partial class DebugRegisters
  {
    /// <summary>
    /// Erforderliche Designervariable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Verwendete Ressourcen bereinigen.
    /// </summary>
    /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
    protected override void Dispose( bool disposing )
    {
      if ( disposing && ( components != null ) )
      {
        components.Dispose();
      }
      base.Dispose( disposing );
    }

    #region Vom Komponenten-Designer generierter Code

    /// <summary>
    /// Erforderliche Methode für die Designerunterstützung.
    /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugRegisters));
      this.label1 = new System.Windows.Forms.Label();
      this.editX = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.editY = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.editA = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.editPC = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.editStack = new System.Windows.Forms.TextBox();
      this.label6 = new System.Windows.Forms.Label();
      this.editStatus = new System.Windows.Forms.TextBox();
      this.label7 = new System.Windows.Forms.Label();
      this.editLIN = new System.Windows.Forms.TextBox();
      this.label8 = new System.Windows.Forms.Label();
      this.editCycle = new System.Windows.Forms.TextBox();
      this.editXDec = new System.Windows.Forms.TextBox();
      this.editYDec = new System.Windows.Forms.TextBox();
      this.editADec = new System.Windows.Forms.TextBox();
      this.editPCDec = new System.Windows.Forms.TextBox();
      this.editStackDec = new System.Windows.Forms.TextBox();
      this.label9 = new System.Windows.Forms.Label();
      this.label10 = new System.Windows.Forms.Label();
      this.edit01 = new System.Windows.Forms.TextBox();
      this.editXBin = new System.Windows.Forms.TextBox();
      this.editYBin = new System.Windows.Forms.TextBox();
      this.editABin = new System.Windows.Forms.TextBox();
      this.label11 = new System.Windows.Forms.Label();
      this.label12 = new System.Windows.Forms.Label();
      this.label13 = new System.Windows.Forms.Label();
      this.label14 = new System.Windows.Forms.Label();
      this.label15 = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 10);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(14, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "X";
      // 
      // editX
      // 
      this.editX.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.editX.Location = new System.Drawing.Point(56, 6);
      this.editX.MaxLength = 2;
      this.editX.Name = "editX";
      this.editX.ReadOnly = true;
      this.editX.Size = new System.Drawing.Size(25, 21);
      this.editX.TabIndex = 1;
      this.editX.TextChanged += new System.EventHandler(this.editX_TextChanged);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(12, 36);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(14, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "Y";
      // 
      // editY
      // 
      this.editY.Font = new System.Drawing.Font("Courier New", 9F);
      this.editY.Location = new System.Drawing.Point(56, 32);
      this.editY.MaxLength = 2;
      this.editY.Name = "editY";
      this.editY.ReadOnly = true;
      this.editY.Size = new System.Drawing.Size(25, 21);
      this.editY.TabIndex = 1;
      this.editY.TextChanged += new System.EventHandler(this.editY_TextChanged);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(12, 62);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(14, 13);
      this.label3.TabIndex = 0;
      this.label3.Text = "A";
      // 
      // editA
      // 
      this.editA.Font = new System.Drawing.Font("Courier New", 9F);
      this.editA.Location = new System.Drawing.Point(56, 58);
      this.editA.MaxLength = 2;
      this.editA.Name = "editA";
      this.editA.ReadOnly = true;
      this.editA.Size = new System.Drawing.Size(25, 21);
      this.editA.TabIndex = 1;
      this.editA.TextChanged += new System.EventHandler(this.editA_TextChanged);
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(12, 87);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(21, 13);
      this.label4.TabIndex = 0;
      this.label4.Text = "PC";
      // 
      // editPC
      // 
      this.editPC.Font = new System.Drawing.Font("Courier New", 9F);
      this.editPC.Location = new System.Drawing.Point(56, 84);
      this.editPC.MaxLength = 4;
      this.editPC.Name = "editPC";
      this.editPC.ReadOnly = true;
      this.editPC.Size = new System.Drawing.Size(64, 21);
      this.editPC.TabIndex = 1;
      this.editPC.TextChanged += new System.EventHandler(this.editPC_TextChanged);
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(12, 113);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(21, 13);
      this.label5.TabIndex = 0;
      this.label5.Text = "SP";
      // 
      // editStack
      // 
      this.editStack.Font = new System.Drawing.Font("Courier New", 9F);
      this.editStack.Location = new System.Drawing.Point(56, 110);
      this.editStack.MaxLength = 4;
      this.editStack.Name = "editStack";
      this.editStack.ReadOnly = true;
      this.editStack.Size = new System.Drawing.Size(64, 21);
      this.editStack.TabIndex = 1;
      this.editStack.TextChanged += new System.EventHandler(this.editStack_TextChanged);
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(12, 139);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(37, 13);
      this.label6.TabIndex = 0;
      this.label6.Text = "Status";
      // 
      // editStatus
      // 
      this.editStatus.Font = new System.Drawing.Font("Courier New", 9F);
      this.editStatus.Location = new System.Drawing.Point(56, 136);
      this.editStatus.MaxLength = 8;
      this.editStatus.Name = "editStatus";
      this.editStatus.ReadOnly = true;
      this.editStatus.Size = new System.Drawing.Size(134, 21);
      this.editStatus.TabIndex = 1;
      this.editStatus.TextChanged += new System.EventHandler(this.editStatus_TextChanged);
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(199, 8);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(24, 13);
      this.label7.TabIndex = 0;
      this.label7.Text = "LIN";
      // 
      // editLIN
      // 
      this.editLIN.Font = new System.Drawing.Font("Courier New", 9F);
      this.editLIN.Location = new System.Drawing.Point(229, 6);
      this.editLIN.Name = "editLIN";
      this.editLIN.ReadOnly = true;
      this.editLIN.Size = new System.Drawing.Size(46, 21);
      this.editLIN.TabIndex = 1;
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(199, 36);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(28, 13);
      this.label8.TabIndex = 0;
      this.label8.Text = "CYC";
      // 
      // editCycle
      // 
      this.editCycle.Font = new System.Drawing.Font("Courier New", 9F);
      this.editCycle.Location = new System.Drawing.Point(229, 32);
      this.editCycle.Name = "editCycle";
      this.editCycle.ReadOnly = true;
      this.editCycle.Size = new System.Drawing.Size(46, 21);
      this.editCycle.TabIndex = 1;
      // 
      // editXDec
      // 
      this.editXDec.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.editXDec.Location = new System.Drawing.Point(87, 6);
      this.editXDec.MaxLength = 3;
      this.editXDec.Name = "editXDec";
      this.editXDec.ReadOnly = true;
      this.editXDec.Size = new System.Drawing.Size(33, 21);
      this.editXDec.TabIndex = 1;
      this.editXDec.TextChanged += new System.EventHandler(this.editXDec_TextChanged);
      // 
      // editYDec
      // 
      this.editYDec.Font = new System.Drawing.Font("Courier New", 9F);
      this.editYDec.Location = new System.Drawing.Point(87, 32);
      this.editYDec.MaxLength = 3;
      this.editYDec.Name = "editYDec";
      this.editYDec.ReadOnly = true;
      this.editYDec.Size = new System.Drawing.Size(33, 21);
      this.editYDec.TabIndex = 1;
      this.editYDec.TextChanged += new System.EventHandler(this.editYDec_TextChanged);
      // 
      // editADec
      // 
      this.editADec.Font = new System.Drawing.Font("Courier New", 9F);
      this.editADec.Location = new System.Drawing.Point(87, 58);
      this.editADec.MaxLength = 3;
      this.editADec.Name = "editADec";
      this.editADec.ReadOnly = true;
      this.editADec.Size = new System.Drawing.Size(33, 21);
      this.editADec.TabIndex = 1;
      this.editADec.TextChanged += new System.EventHandler(this.editADec_TextChanged);
      // 
      // editPCDec
      // 
      this.editPCDec.Font = new System.Drawing.Font("Courier New", 9F);
      this.editPCDec.Location = new System.Drawing.Point(126, 84);
      this.editPCDec.MaxLength = 5;
      this.editPCDec.Name = "editPCDec";
      this.editPCDec.ReadOnly = true;
      this.editPCDec.Size = new System.Drawing.Size(64, 21);
      this.editPCDec.TabIndex = 1;
      this.editPCDec.TextChanged += new System.EventHandler(this.editPCDec_TextChanged);
      // 
      // editStackDec
      // 
      this.editStackDec.Font = new System.Drawing.Font("Courier New", 9F);
      this.editStackDec.Location = new System.Drawing.Point(126, 110);
      this.editStackDec.MaxLength = 5;
      this.editStackDec.Name = "editStackDec";
      this.editStackDec.ReadOnly = true;
      this.editStackDec.Size = new System.Drawing.Size(64, 21);
      this.editStackDec.TabIndex = 1;
      this.editStackDec.TextChanged += new System.EventHandler(this.editStackDec_TextChanged);
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label9.Location = new System.Drawing.Point(57, 160);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(63, 15);
      this.label9.TabIndex = 0;
      this.label9.Text = "NV-BDIZC";
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(199, 62);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(25, 13);
      this.label10.TabIndex = 0;
      this.label10.Text = "$01";
      // 
      // edit01
      // 
      this.edit01.Font = new System.Drawing.Font("Courier New", 9F);
      this.edit01.Location = new System.Drawing.Point(229, 58);
      this.edit01.Name = "edit01";
      this.edit01.ReadOnly = true;
      this.edit01.Size = new System.Drawing.Size(46, 21);
      this.edit01.TabIndex = 1;
      // 
      // editXBin
      // 
      this.editXBin.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.editXBin.Location = new System.Drawing.Point(126, 6);
      this.editXBin.MaxLength = 8;
      this.editXBin.Name = "editXBin";
      this.editXBin.ReadOnly = true;
      this.editXBin.Size = new System.Drawing.Size(64, 21);
      this.editXBin.TabIndex = 1;
      this.editXBin.TextChanged += new System.EventHandler(this.editXBin_TextChanged);
      // 
      // editYBin
      // 
      this.editYBin.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.editYBin.Location = new System.Drawing.Point(126, 32);
      this.editYBin.MaxLength = 8;
      this.editYBin.Name = "editYBin";
      this.editYBin.ReadOnly = true;
      this.editYBin.Size = new System.Drawing.Size(64, 21);
      this.editYBin.TabIndex = 1;
      this.editYBin.TextChanged += new System.EventHandler(this.editYBin_TextChanged);
      // 
      // editABin
      // 
      this.editABin.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.editABin.Location = new System.Drawing.Point(126, 58);
      this.editABin.MaxLength = 8;
      this.editABin.Name = "editABin";
      this.editABin.ReadOnly = true;
      this.editABin.Size = new System.Drawing.Size(64, 21);
      this.editABin.TabIndex = 1;
      this.editABin.TextChanged += new System.EventHandler(this.editABin_TextChanged);
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Location = new System.Drawing.Point(41, 10);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(13, 13);
      this.label11.TabIndex = 0;
      this.label11.Text = "$";
      // 
      // label12
      // 
      this.label12.AutoSize = true;
      this.label12.Location = new System.Drawing.Point(41, 36);
      this.label12.Name = "label12";
      this.label12.Size = new System.Drawing.Size(13, 13);
      this.label12.TabIndex = 0;
      this.label12.Text = "$";
      // 
      // label13
      // 
      this.label13.AutoSize = true;
      this.label13.Location = new System.Drawing.Point(41, 62);
      this.label13.Name = "label13";
      this.label13.Size = new System.Drawing.Size(13, 13);
      this.label13.TabIndex = 0;
      this.label13.Text = "$";
      // 
      // label14
      // 
      this.label14.AutoSize = true;
      this.label14.Location = new System.Drawing.Point(41, 87);
      this.label14.Name = "label14";
      this.label14.Size = new System.Drawing.Size(13, 13);
      this.label14.TabIndex = 0;
      this.label14.Text = "$";
      // 
      // label15
      // 
      this.label15.AutoSize = true;
      this.label15.Location = new System.Drawing.Point(41, 113);
      this.label15.Name = "label15";
      this.label15.Size = new System.Drawing.Size(13, 13);
      this.label15.TabIndex = 0;
      this.label15.Text = "$";
      // 
      // DebugRegisters
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.ClientSize = new System.Drawing.Size(534, 390);
      this.Controls.Add(this.editStatus);
      this.Controls.Add(this.label9);
      this.Controls.Add(this.label6);
      this.Controls.Add(this.edit01);
      this.Controls.Add(this.label10);
      this.Controls.Add(this.editCycle);
      this.Controls.Add(this.label8);
      this.Controls.Add(this.editLIN);
      this.Controls.Add(this.label7);
      this.Controls.Add(this.editStackDec);
      this.Controls.Add(this.editStack);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.editPCDec);
      this.Controls.Add(this.editPC);
      this.Controls.Add(this.editADec);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.editA);
      this.Controls.Add(this.editYDec);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.editY);
      this.Controls.Add(this.editABin);
      this.Controls.Add(this.editYBin);
      this.Controls.Add(this.editXBin);
      this.Controls.Add(this.editXDec);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.editX);
      this.Controls.Add(this.label15);
      this.Controls.Add(this.label14);
      this.Controls.Add(this.label13);
      this.Controls.Add(this.label12);
      this.Controls.Add(this.label11);
      this.Controls.Add(this.label1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "DebugRegisters";
      this.Text = "Registers";
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label6;
    public System.Windows.Forms.TextBox editX;
    public System.Windows.Forms.TextBox editY;
    public System.Windows.Forms.TextBox editA;
    public System.Windows.Forms.TextBox editPC;
    public System.Windows.Forms.TextBox editStack;
    public System.Windows.Forms.TextBox editStatus;
    private System.Windows.Forms.Label label7;
    public System.Windows.Forms.TextBox editLIN;
    private System.Windows.Forms.Label label8;
    public System.Windows.Forms.TextBox editCycle;
    public System.Windows.Forms.TextBox editXDec;
    public System.Windows.Forms.TextBox editYDec;
    public System.Windows.Forms.TextBox editADec;
    public System.Windows.Forms.TextBox editPCDec;
    public System.Windows.Forms.TextBox editStackDec;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label label10;
    public System.Windows.Forms.TextBox edit01;
    public System.Windows.Forms.TextBox editXBin;
    public System.Windows.Forms.TextBox editYBin;
    public System.Windows.Forms.TextBox editABin;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.Label label13;
    private System.Windows.Forms.Label label14;
    private System.Windows.Forms.Label label15;
  }
}
