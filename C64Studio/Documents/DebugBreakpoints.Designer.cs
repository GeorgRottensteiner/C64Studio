namespace RetroDevStudio.Documents
{
  partial class DebugBreakpoints
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugBreakpoints));
      this.listBreakpoints = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.btnDeleteBreakpoint = new DecentForms.Button();
      this.groupBreakpointData = new System.Windows.Forms.GroupBox();
      this.comboSymbols = new System.Windows.Forms.ComboBox();
      this.checkTriggerStore = new System.Windows.Forms.CheckBox();
      this.checkTriggerLoad = new System.Windows.Forms.CheckBox();
      this.checkTriggerExec = new System.Windows.Forms.CheckBox();
      this.editTriggerConditions = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.editBPAddress = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.btnDeleteAll = new DecentForms.Button();
      this.btnApplyChanges = new DecentForms.Button();
      this.btnAddBreakpoint = new DecentForms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      this.groupBreakpointData.SuspendLayout();
      this.SuspendLayout();
      // 
      // listBreakpoints
      // 
      this.listBreakpoints.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.listBreakpoints.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader3,
            this.columnHeader2});
      this.listBreakpoints.FullRowSelect = true;
      this.listBreakpoints.HideSelection = false;
      this.listBreakpoints.Location = new System.Drawing.Point(12, 12);
      this.listBreakpoints.Name = "listBreakpoints";
      this.listBreakpoints.Size = new System.Drawing.Size(510, 166);
      this.listBreakpoints.TabIndex = 0;
      this.listBreakpoints.UseCompatibleStateImageBehavior = false;
      this.listBreakpoints.View = System.Windows.Forms.View.Details;
      this.listBreakpoints.ItemActivate += new System.EventHandler(this.listBreakpoints_ItemActivate);
      this.listBreakpoints.SelectedIndexChanged += new System.EventHandler(this.listBreakpoints_SelectedIndexChanged);
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "No";
      this.columnHeader1.Width = 32;
      // 
      // columnHeader4
      // 
      this.columnHeader4.Text = "Document";
      this.columnHeader4.Width = 102;
      // 
      // columnHeader5
      // 
      this.columnHeader5.Text = "Line";
      // 
      // columnHeader6
      // 
      this.columnHeader6.Text = "Trigger";
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "Conditions";
      this.columnHeader3.Width = 127;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Address";
      this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.columnHeader2.Width = 59;
      // 
      // btnDeleteBreakpoint
      // 
      this.btnDeleteBreakpoint.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnDeleteBreakpoint.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnDeleteBreakpoint.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnDeleteBreakpoint.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnDeleteBreakpoint.Enabled = false;
      this.btnDeleteBreakpoint.Image = null;
      this.btnDeleteBreakpoint.Location = new System.Drawing.Point(93, 184);
      this.btnDeleteBreakpoint.Name = "btnDeleteBreakpoint";
      this.btnDeleteBreakpoint.Size = new System.Drawing.Size(75, 23);
      this.btnDeleteBreakpoint.TabIndex = 2;
      this.btnDeleteBreakpoint.Text = "Del";
      this.btnDeleteBreakpoint.Click += new DecentForms.EventHandler(this.btnDeleteBreakpoint_Click);
      // 
      // groupBreakpointData
      // 
      this.groupBreakpointData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBreakpointData.Controls.Add(this.comboSymbols);
      this.groupBreakpointData.Controls.Add(this.checkTriggerStore);
      this.groupBreakpointData.Controls.Add(this.checkTriggerLoad);
      this.groupBreakpointData.Controls.Add(this.checkTriggerExec);
      this.groupBreakpointData.Controls.Add(this.editTriggerConditions);
      this.groupBreakpointData.Controls.Add(this.label3);
      this.groupBreakpointData.Controls.Add(this.label2);
      this.groupBreakpointData.Controls.Add(this.editBPAddress);
      this.groupBreakpointData.Controls.Add(this.label1);
      this.groupBreakpointData.Location = new System.Drawing.Point(12, 213);
      this.groupBreakpointData.Name = "groupBreakpointData";
      this.groupBreakpointData.Size = new System.Drawing.Size(510, 99);
      this.groupBreakpointData.TabIndex = 2;
      this.groupBreakpointData.TabStop = false;
      this.groupBreakpointData.Text = "Breakpoint Settings";
      // 
      // comboSymbols
      // 
      this.comboSymbols.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboSymbols.FormattingEnabled = true;
      this.comboSymbols.Location = new System.Drawing.Point(210, 19);
      this.comboSymbols.Name = "comboSymbols";
      this.comboSymbols.Size = new System.Drawing.Size(145, 21);
      this.comboSymbols.TabIndex = 1;
      this.comboSymbols.SelectedIndexChanged += new System.EventHandler(this.comboSymbols_SelectedIndexChanged);
      // 
      // checkTriggerStore
      // 
      this.checkTriggerStore.AutoSize = true;
      this.checkTriggerStore.Location = new System.Drawing.Point(210, 71);
      this.checkTriggerStore.Name = "checkTriggerStore";
      this.checkTriggerStore.Size = new System.Drawing.Size(51, 17);
      this.checkTriggerStore.TabIndex = 5;
      this.checkTriggerStore.Text = "Store";
      this.checkTriggerStore.UseVisualStyleBackColor = true;
      this.checkTriggerStore.CheckedChanged += new System.EventHandler(this.checkTriggerStore_CheckedChanged);
      // 
      // checkTriggerLoad
      // 
      this.checkTriggerLoad.AutoSize = true;
      this.checkTriggerLoad.Location = new System.Drawing.Point(146, 71);
      this.checkTriggerLoad.Name = "checkTriggerLoad";
      this.checkTriggerLoad.Size = new System.Drawing.Size(50, 17);
      this.checkTriggerLoad.TabIndex = 4;
      this.checkTriggerLoad.Text = "Load";
      this.checkTriggerLoad.UseVisualStyleBackColor = true;
      this.checkTriggerLoad.CheckedChanged += new System.EventHandler(this.checkTriggerLoad_CheckedChanged);
      // 
      // checkTriggerExec
      // 
      this.checkTriggerExec.AutoSize = true;
      this.checkTriggerExec.Checked = true;
      this.checkTriggerExec.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkTriggerExec.Location = new System.Drawing.Point(67, 71);
      this.checkTriggerExec.Name = "checkTriggerExec";
      this.checkTriggerExec.Size = new System.Drawing.Size(65, 17);
      this.checkTriggerExec.TabIndex = 3;
      this.checkTriggerExec.Text = "Execute";
      this.checkTriggerExec.UseVisualStyleBackColor = true;
      this.checkTriggerExec.CheckedChanged += new System.EventHandler(this.checkTriggerExec_CheckedChanged);
      // 
      // editTriggerConditions
      // 
      this.editTriggerConditions.Location = new System.Drawing.Point(67, 45);
      this.editTriggerConditions.Name = "editTriggerConditions";
      this.editTriggerConditions.Size = new System.Drawing.Size(288, 20);
      this.editTriggerConditions.TabIndex = 2;
      this.editTriggerConditions.TextChanged += new System.EventHandler(this.editTriggerConditions_TextChanged);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(2, 72);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(58, 13);
      this.label3.TabIndex = 0;
      this.label3.Text = "Trigger on:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(2, 48);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(59, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "Conditions:";
      // 
      // editBPAddress
      // 
      this.editBPAddress.Location = new System.Drawing.Point(67, 19);
      this.editBPAddress.Name = "editBPAddress";
      this.editBPAddress.Size = new System.Drawing.Size(129, 20);
      this.editBPAddress.TabIndex = 0;
      this.editBPAddress.TextChanged += new System.EventHandler(this.editBPAddress_TextChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(2, 22);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(48, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Address:";
      // 
      // btnDeleteAll
      // 
      this.btnDeleteAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnDeleteAll.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnDeleteAll.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnDeleteAll.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnDeleteAll.Image = null;
      this.btnDeleteAll.Location = new System.Drawing.Point(174, 184);
      this.btnDeleteAll.Name = "btnDeleteAll";
      this.btnDeleteAll.Size = new System.Drawing.Size(75, 23);
      this.btnDeleteAll.TabIndex = 3;
      this.btnDeleteAll.Text = "Del All";
      this.btnDeleteAll.Click += new DecentForms.EventHandler(this.btnDeleteAllBreakpoints_Click);
      // 
      // btnApplyChanges
      // 
      this.btnApplyChanges.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnApplyChanges.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnApplyChanges.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnApplyChanges.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnApplyChanges.Enabled = false;
      this.btnApplyChanges.Image = null;
      this.btnApplyChanges.Location = new System.Drawing.Point(277, 184);
      this.btnApplyChanges.Name = "btnApplyChanges";
      this.btnApplyChanges.Size = new System.Drawing.Size(75, 23);
      this.btnApplyChanges.TabIndex = 4;
      this.btnApplyChanges.Text = "Apply";
      this.btnApplyChanges.Click += new DecentForms.EventHandler(this.btnApplyChanges_Click);
      // 
      // btnAddBreakpoint
      // 
      this.btnAddBreakpoint.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnAddBreakpoint.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnAddBreakpoint.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnAddBreakpoint.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnAddBreakpoint.Enabled = false;
      this.btnAddBreakpoint.Image = null;
      this.btnAddBreakpoint.Location = new System.Drawing.Point(12, 184);
      this.btnAddBreakpoint.Name = "btnAddBreakpoint";
      this.btnAddBreakpoint.Size = new System.Drawing.Size(75, 23);
      this.btnAddBreakpoint.TabIndex = 1;
      this.btnAddBreakpoint.Text = "Add";
      this.btnAddBreakpoint.Click += new DecentForms.EventHandler(this.btnAddBreakpoint_Click);
      // 
      // DebugBreakpoints
      // 
      this.ClientSize = new System.Drawing.Size(534, 390);
      this.Controls.Add(this.groupBreakpointData);
      this.Controls.Add(this.btnDeleteAll);
      this.Controls.Add(this.btnDeleteBreakpoint);
      this.Controls.Add(this.btnAddBreakpoint);
      this.Controls.Add(this.btnApplyChanges);
      this.Controls.Add(this.listBreakpoints);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "DebugBreakpoints";
      this.Text = "Breakpoints";
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      this.groupBreakpointData.ResumeLayout(false);
      this.groupBreakpointData.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListView listBreakpoints;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.GroupBox groupBreakpointData;
    private System.Windows.Forms.TextBox editTriggerConditions;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox editBPAddress;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ColumnHeader columnHeader4;
    private System.Windows.Forms.ColumnHeader columnHeader5;
    private System.Windows.Forms.CheckBox checkTriggerStore;
    private System.Windows.Forms.CheckBox checkTriggerLoad;
    private System.Windows.Forms.CheckBox checkTriggerExec;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.ColumnHeader columnHeader6;
    private System.Windows.Forms.ComboBox comboSymbols;
    private DecentForms.Button btnDeleteBreakpoint;
    private DecentForms.Button btnDeleteAll;
    private DecentForms.Button btnApplyChanges;
    private DecentForms.Button btnAddBreakpoint;
  }
}
