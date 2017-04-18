namespace C64Studio
{
  partial class PropBuildEventBuildChain
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
      this.editBuildChainDefines = new System.Windows.Forms.TextBox();
      this.comboBuildChainConfig = new System.Windows.Forms.ComboBox();
      this.comboBuildChainProject = new System.Windows.Forms.ComboBox();
      this.listBuildChainProjects = new C64Studio.ArrangedItemList();
      this.label9 = new System.Windows.Forms.Label();
      this.label8 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.checkBuildChainActive = new System.Windows.Forms.CheckBox();
      this.label1 = new System.Windows.Forms.Label();
      this.comboBuildChainFile = new System.Windows.Forms.ComboBox();
      this.SuspendLayout();
      // 
      // editBuildChainDefines
      // 
      this.editBuildChainDefines.AcceptsReturn = true;
      this.editBuildChainDefines.Enabled = false;
      this.editBuildChainDefines.Location = new System.Drawing.Point(281, 96);
      this.editBuildChainDefines.Multiline = true;
      this.editBuildChainDefines.Name = "editBuildChainDefines";
      this.editBuildChainDefines.Size = new System.Drawing.Size(312, 182);
      this.editBuildChainDefines.TabIndex = 14;
      this.editBuildChainDefines.TextChanged += new System.EventHandler(this.editBuildChainDefines_TextChanged);
      // 
      // comboBuildChainConfig
      // 
      this.comboBuildChainConfig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBuildChainConfig.Enabled = false;
      this.comboBuildChainConfig.FormattingEnabled = true;
      this.comboBuildChainConfig.Location = new System.Drawing.Point(281, 38);
      this.comboBuildChainConfig.Name = "comboBuildChainConfig";
      this.comboBuildChainConfig.Size = new System.Drawing.Size(312, 21);
      this.comboBuildChainConfig.TabIndex = 12;
      this.comboBuildChainConfig.SelectedIndexChanged += new System.EventHandler(this.comboBuildChainConfig_SelectedIndexChanged);
      // 
      // comboBuildChainProject
      // 
      this.comboBuildChainProject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBuildChainProject.Enabled = false;
      this.comboBuildChainProject.FormattingEnabled = true;
      this.comboBuildChainProject.Location = new System.Drawing.Point(281, 13);
      this.comboBuildChainProject.Name = "comboBuildChainProject";
      this.comboBuildChainProject.Size = new System.Drawing.Size(312, 21);
      this.comboBuildChainProject.TabIndex = 13;
      this.comboBuildChainProject.SelectedIndexChanged += new System.EventHandler(this.comboBuildChainProject_SelectedIndexChanged);
      // 
      // listBuildChainProjects
      // 
      this.listBuildChainProjects.AddButtonEnabled = false;
      this.listBuildChainProjects.Enabled = false;
      this.listBuildChainProjects.Location = new System.Drawing.Point(5, 38);
      this.listBuildChainProjects.MustHaveOneElement = false;
      this.listBuildChainProjects.Name = "listBuildChainProjects";
      this.listBuildChainProjects.Size = new System.Drawing.Size(221, 244);
      this.listBuildChainProjects.TabIndex = 11;
      this.listBuildChainProjects.AddingItem += new C64Studio.ArrangedItemList.AddingItemEventHandler(this.listBuildChainProjects_AddingItem);
      this.listBuildChainProjects.ItemAdded += new C64Studio.ArrangedItemList.ItemModifiedEventHandler(this.listBuildChainProjects_ItemAdded);
      this.listBuildChainProjects.ItemRemoved += new C64Studio.ArrangedItemList.ItemModifiedEventHandler(this.listBuildChainProjects_ItemRemoved);
      this.listBuildChainProjects.ItemMoved += new C64Studio.ArrangedItemList.ItemExchangedEventHandler(this.listBuildChainProjects_ItemMoved);
      this.listBuildChainProjects.SelectedIndexChanged += new C64Studio.ArrangedItemList.ItemModifiedEventHandler(this.listBuildChainProjects_SelectedIndexChanged);
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Enabled = false;
      this.label9.Location = new System.Drawing.Point(232, 96);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(46, 13);
      this.label9.TabIndex = 8;
      this.label9.Text = "Defines:";
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Enabled = false;
      this.label8.Location = new System.Drawing.Point(232, 41);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(40, 13);
      this.label8.TabIndex = 9;
      this.label8.Text = "Config:";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Enabled = false;
      this.label6.Location = new System.Drawing.Point(232, 16);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(43, 13);
      this.label6.TabIndex = 10;
      this.label6.Text = "Project:";
      // 
      // checkBuildChainActive
      // 
      this.checkBuildChainActive.AutoSize = true;
      this.checkBuildChainActive.Location = new System.Drawing.Point(12, 12);
      this.checkBuildChainActive.Name = "checkBuildChainActive";
      this.checkBuildChainActive.Size = new System.Drawing.Size(137, 17);
      this.checkBuildChainActive.TabIndex = 15;
      this.checkBuildChainActive.Text = "Build additional projects";
      this.checkBuildChainActive.UseVisualStyleBackColor = true;
      this.checkBuildChainActive.CheckedChanged += new System.EventHandler(this.checkBuildChainActive_CheckedChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Enabled = false;
      this.label1.Location = new System.Drawing.Point(232, 67);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(26, 13);
      this.label1.TabIndex = 9;
      this.label1.Text = "File:";
      // 
      // comboBuildChainFile
      // 
      this.comboBuildChainFile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBuildChainFile.Enabled = false;
      this.comboBuildChainFile.FormattingEnabled = true;
      this.comboBuildChainFile.Location = new System.Drawing.Point(281, 64);
      this.comboBuildChainFile.Name = "comboBuildChainFile";
      this.comboBuildChainFile.Size = new System.Drawing.Size(312, 21);
      this.comboBuildChainFile.TabIndex = 12;
      this.comboBuildChainFile.SelectedIndexChanged += new System.EventHandler(this.comboBuildChainFile_SelectedIndexChanged);
      // 
      // PropBuildEventBuildChain
      // 
      this.ClientSize = new System.Drawing.Size(599, 290);
      this.ControlBox = false;
      this.Controls.Add(this.checkBuildChainActive);
      this.Controls.Add(this.editBuildChainDefines);
      this.Controls.Add(this.comboBuildChainFile);
      this.Controls.Add(this.comboBuildChainConfig);
      this.Controls.Add(this.comboBuildChainProject);
      this.Controls.Add(this.listBuildChainProjects);
      this.Controls.Add(this.label9);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.label8);
      this.Controls.Add(this.label6);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Name = "PropBuildEventBuildChain";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox editBuildChainDefines;
    private System.Windows.Forms.ComboBox comboBuildChainConfig;
    private System.Windows.Forms.ComboBox comboBuildChainProject;
    private ArrangedItemList listBuildChainProjects;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.CheckBox checkBuildChainActive;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox comboBuildChainFile;


  }
}
