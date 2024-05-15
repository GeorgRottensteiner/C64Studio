namespace RetroDevStudio
{
  partial class PropAssemblerType
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

    #region Vom Windows Form-Designer generierter Code

    /// <summary>
    /// Erforderliche Methode für die Designerunterstützung.
    /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
    /// </summary>
    private void InitializeComponent()
    {
      this.comboAssemblerType = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.btnParseAssembler = new DecentForms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // comboAssemblerType
      // 
      this.comboAssemblerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboAssemblerType.FormattingEnabled = true;
      this.comboAssemblerType.Location = new System.Drawing.Point( 100, 14 );
      this.comboAssemblerType.Name = "comboAssemblerType";
      this.comboAssemblerType.Size = new System.Drawing.Size( 180, 21 );
      this.comboAssemblerType.TabIndex = 12;
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point( 394, 17 );
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size( 203, 87 );
      this.label1.TabIndex = 11;
      this.label1.Text = "Choose assembler by inspecting the code";
      // 
      // btnParseAssembler
      // 
      this.btnParseAssembler.Location = new System.Drawing.Point( 286, 12 );
      this.btnParseAssembler.Name = "btnParseAssembler";
      this.btnParseAssembler.Size = new System.Drawing.Size( 102, 23 );
      this.btnParseAssembler.TabIndex = 10;
      this.btnParseAssembler.Text = "Detect Assembler";
      this.btnParseAssembler.Click += new DecentForms.EventHandler( this.btnParseAssembler_Click );
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point( 6, 17 );
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size( 58, 13 );
      this.label2.TabIndex = 8;
      this.label2.Text = "Assembler:";
      // 
      // PropAssemblerType
      // 
      this.ClientSize = new System.Drawing.Size( 599, 364 );
      this.ControlBox = false;
      this.Controls.Add( this.comboAssemblerType );
      this.Controls.Add( this.label1 );
      this.Controls.Add( this.btnParseAssembler );
      this.Controls.Add( this.label2 );
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Name = "PropAssemblerType";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.ResumeLayout( false );
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ComboBox comboAssemblerType;
    private System.Windows.Forms.Label label1;
    private DecentForms.Button btnParseAssembler;
    private System.Windows.Forms.Label label2;
  }
}
