namespace RetroDevStudio
{
  partial class PropCompileTarget
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropCompileTarget));
      this.comboTargetType = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.btnParseTarget = new DecentForms.Button();
      this.editTargetFilename = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.listDependencies = new System.Windows.Forms.ListView();
      this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.label5 = new System.Windows.Forms.Label();
      this.btnAddExternalDependency = new DecentForms.Button();
      this.btnRemoveExternalDependency = new DecentForms.Button();
      this.listExternalDependencies = new System.Windows.Forms.ListBox();
      this.SuspendLayout();
      // 
      // comboTargetType
      // 
      this.comboTargetType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboTargetType.FormattingEnabled = true;
      this.comboTargetType.Location = new System.Drawing.Point(100, 44);
      this.comboTargetType.Name = "comboTargetType";
      this.comboTargetType.Size = new System.Drawing.Size(180, 21);
      this.comboTargetType.TabIndex = 12;
      this.comboTargetType.SelectedIndexChanged += new System.EventHandler(this.comboTargetType_SelectedIndexChanged);
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(394, 17);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(203, 87);
      this.label1.TabIndex = 11;
      this.label1.Text = "Provide a target file name the file gets assembled to.\r\nOverrides any !to macro i" +
    "n the code.\r\nUse \"Parse for Target\" to retrieve the target filename from the cod" +
    "e.\r\n";
      // 
      // btnParseTarget
      // 
      this.btnParseTarget.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnParseTarget.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnParseTarget.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnParseTarget.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnParseTarget.Image = null;
      this.btnParseTarget.Location = new System.Drawing.Point(286, 12);
      this.btnParseTarget.Name = "btnParseTarget";
      this.btnParseTarget.Size = new System.Drawing.Size(102, 23);
      this.btnParseTarget.TabIndex = 10;
      this.btnParseTarget.Text = "Parse for Target";
      this.btnParseTarget.Click += new DecentForms.EventHandler(this.btnParseTarget_Click);
      // 
      // editTargetFilename
      // 
      this.editTargetFilename.Location = new System.Drawing.Point(100, 14);
      this.editTargetFilename.Name = "editTargetFilename";
      this.editTargetFilename.Size = new System.Drawing.Size(180, 20);
      this.editTargetFilename.TabIndex = 9;
      this.editTargetFilename.TextChanged += new System.EventHandler(this.editTargetFilename_TextChanged);
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(6, 107);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(79, 13);
      this.label4.TabIndex = 6;
      this.label4.Text = "Dependencies:";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(6, 47);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(68, 13);
      this.label3.TabIndex = 7;
      this.label3.Text = "Target Type:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(6, 17);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(60, 13);
      this.label2.TabIndex = 8;
      this.label2.Text = "Target File:";
      // 
      // listDependencies
      // 
      this.listDependencies.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.listDependencies.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
      this.listDependencies.HideSelection = false;
      this.listDependencies.Location = new System.Drawing.Point(100, 107);
      this.listDependencies.Name = "listDependencies";
      this.listDependencies.OwnerDraw = true;
      this.listDependencies.Size = new System.Drawing.Size(487, 126);
      this.listDependencies.TabIndex = 14;
      this.listDependencies.UseCompatibleStateImageBehavior = false;
      this.listDependencies.View = System.Windows.Forms.View.Details;
      this.listDependencies.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.listDependencies_DrawColumnHeader);
      this.listDependencies.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.listDependencies_DrawItem);
      this.listDependencies.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.listDependencies_DrawSubItem);
      this.listDependencies.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listDependencies_MouseDown);
      // 
      // columnHeader4
      // 
      this.columnHeader4.Text = "Project";
      this.columnHeader4.Width = 100;
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "File";
      this.columnHeader1.Width = 200;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Dependant";
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "Symbols";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(6, 239);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(79, 26);
      this.label5.TabIndex = 6;
      this.label5.Text = "External\r\nDependencies:";
      // 
      // btnAddExternalDependency
      // 
      this.btnAddExternalDependency.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnAddExternalDependency.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnAddExternalDependency.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnAddExternalDependency.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnAddExternalDependency.Image = ((System.Drawing.Image)(resources.GetObject("btnAddExternalDependency.Image")));
      this.btnAddExternalDependency.Location = new System.Drawing.Point(100, 326);
      this.btnAddExternalDependency.Name = "btnAddExternalDependency";
      this.btnAddExternalDependency.Size = new System.Drawing.Size(38, 23);
      this.btnAddExternalDependency.TabIndex = 15;
      this.btnAddExternalDependency.Click += new DecentForms.EventHandler(this.btnAddExternalDependency_Click);
      // 
      // btnRemoveExternalDependency
      // 
      this.btnRemoveExternalDependency.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnRemoveExternalDependency.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnRemoveExternalDependency.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnRemoveExternalDependency.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnRemoveExternalDependency.Enabled = false;
      this.btnRemoveExternalDependency.Image = ((System.Drawing.Image)(resources.GetObject("btnRemoveExternalDependency.Image")));
      this.btnRemoveExternalDependency.Location = new System.Drawing.Point(144, 326);
      this.btnRemoveExternalDependency.Name = "btnRemoveExternalDependency";
      this.btnRemoveExternalDependency.Size = new System.Drawing.Size(38, 23);
      this.btnRemoveExternalDependency.TabIndex = 15;
      this.btnRemoveExternalDependency.Click += new DecentForms.EventHandler(this.btnRemoveExternalDependency_Click);
      // 
      // listExternalDependencies
      // 
      this.listExternalDependencies.FormattingEnabled = true;
      this.listExternalDependencies.Location = new System.Drawing.Point(100, 239);
      this.listExternalDependencies.Name = "listExternalDependencies";
      this.listExternalDependencies.Size = new System.Drawing.Size(487, 82);
      this.listExternalDependencies.TabIndex = 16;
      this.listExternalDependencies.SelectedIndexChanged += new System.EventHandler(this.listExternalDependencies_SelectedIndexChanged);
      // 
      // PropCompileTarget
      // 
      this.ClientSize = new System.Drawing.Size(599, 364);
      this.ControlBox = false;
      this.Controls.Add(this.listExternalDependencies);
      this.Controls.Add(this.btnRemoveExternalDependency);
      this.Controls.Add(this.btnAddExternalDependency);
      this.Controls.Add(this.listDependencies);
      this.Controls.Add(this.comboTargetType);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.btnParseTarget);
      this.Controls.Add(this.editTargetFilename);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Name = "PropCompileTarget";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ComboBox comboTargetType;
    private System.Windows.Forms.Label label1;
    private DecentForms.Button btnParseTarget;
    private System.Windows.Forms.TextBox editTargetFilename;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ListView listDependencies;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.Label label5;
    private DecentForms.Button btnAddExternalDependency;
    private DecentForms.Button btnRemoveExternalDependency;
    private System.Windows.Forms.ListBox listExternalDependencies;
        private System.Windows.Forms.ColumnHeader columnHeader4;
    }
}
