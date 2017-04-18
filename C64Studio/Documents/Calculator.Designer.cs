namespace C64Studio
{
  partial class Calculator
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( Calculator ) );
      this.label1 = new System.Windows.Forms.Label();
      this.editDec = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.editHex = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.editBin = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.editCalc = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.editResult = new System.Windows.Forms.TextBox();
      ( (System.ComponentModel.ISupportInitialize)( this.m_FileWatcher ) ).BeginInit();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point( 12, 9 );
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size( 30, 13 );
      this.label1.TabIndex = 0;
      this.label1.Text = "Dec:";
      // 
      // editDec
      // 
      this.editDec.Location = new System.Drawing.Point( 57, 6 );
      this.editDec.Name = "editDec";
      this.editDec.Size = new System.Drawing.Size( 75, 20 );
      this.editDec.TabIndex = 1;
      this.editDec.TextChanged += new System.EventHandler( this.editDec_TextChanged );
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point( 136, 9 );
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size( 29, 13 );
      this.label2.TabIndex = 0;
      this.label2.Text = "Hex:";
      // 
      // editHex
      // 
      this.editHex.Location = new System.Drawing.Point( 171, 6 );
      this.editHex.Name = "editHex";
      this.editHex.Size = new System.Drawing.Size( 75, 20 );
      this.editHex.TabIndex = 1;
      this.editHex.TextChanged += new System.EventHandler( this.editHex_TextChanged );
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point( 12, 35 );
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size( 25, 13 );
      this.label3.TabIndex = 0;
      this.label3.Text = "Bin:";
      // 
      // editBin
      // 
      this.editBin.Location = new System.Drawing.Point( 57, 32 );
      this.editBin.Name = "editBin";
      this.editBin.Size = new System.Drawing.Size( 189, 20 );
      this.editBin.TabIndex = 1;
      this.editBin.TextChanged += new System.EventHandler( this.editBin_TextChanged );
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point( 12, 61 );
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size( 31, 13 );
      this.label4.TabIndex = 0;
      this.label4.Text = "Calc:";
      // 
      // editCalc
      // 
      this.editCalc.Location = new System.Drawing.Point( 57, 58 );
      this.editCalc.Name = "editCalc";
      this.editCalc.Size = new System.Drawing.Size( 189, 20 );
      this.editCalc.TabIndex = 1;
      this.editCalc.TextChanged += new System.EventHandler( this.editCalc_TextChanged );
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point( 12, 85 );
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size( 40, 13 );
      this.label5.TabIndex = 0;
      this.label5.Text = "Result:";
      // 
      // editResult
      // 
      this.editResult.Location = new System.Drawing.Point( 57, 82 );
      this.editResult.Name = "editResult";
      this.editResult.Size = new System.Drawing.Size( 189, 20 );
      this.editResult.TabIndex = 1;
      // 
      // Calculator
      // 
      this.ClientSize = new System.Drawing.Size( 258, 121 );
      this.Controls.Add( this.editHex );
      this.Controls.Add( this.label2 );
      this.Controls.Add( this.editBin );
      this.Controls.Add( this.editCalc );
      this.Controls.Add( this.editResult );
      this.Controls.Add( this.label4 );
      this.Controls.Add( this.editDec );
      this.Controls.Add( this.label5 );
      this.Controls.Add( this.label3 );
      this.Controls.Add( this.label1 );
      this.Icon = ( (System.Drawing.Icon)( resources.GetObject( "$this.Icon" ) ) );
      this.MinimumSize = new System.Drawing.Size( 274, 159 );
      this.Name = "Calculator";
      this.Text = "Calculator";
      ( (System.ComponentModel.ISupportInitialize)( this.m_FileWatcher ) ).EndInit();
      this.ResumeLayout( false );
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox editDec;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox editHex;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox editBin;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox editCalc;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox editResult;


  }
}
