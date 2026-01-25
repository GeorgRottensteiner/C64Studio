namespace RetroDevStudio
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
      this.listBuildChainProjects = new RetroDevStudio.Controls.ArrangedItemList();
      this.labelDefines = new System.Windows.Forms.Label();
      this.labelConfig = new System.Windows.Forms.Label();
      this.labelProject = new System.Windows.Forms.Label();
      this.checkBuildChainActive = new System.Windows.Forms.CheckBox();
      this.labelFile = new System.Windows.Forms.Label();
      this.comboBuildChainFile = new System.Windows.Forms.ComboBox();
      this.SuspendLayout();
      // 
      // editBuildChainDefines
      // 
      this.editBuildChainDefines.AcceptsReturn = true;
      this.editBuildChainDefines.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
      this.comboBuildChainConfig.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
      this.comboBuildChainProject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
      this.listBuildChainProjects.AllowClone = true;
      this.listBuildChainProjects.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.listBuildChainProjects.DeleteButtonEnabled = false;
      this.listBuildChainProjects.Enabled = false;
      this.listBuildChainProjects.HasOwnerDrawColumn = true;
      this.listBuildChainProjects.HighlightColor = System.Drawing.SystemColors.HotTrack;
      this.listBuildChainProjects.HighlightTextColor = System.Drawing.SystemColors.HighlightText;
      this.listBuildChainProjects.Location = new System.Drawing.Point(5, 38);
      this.listBuildChainProjects.MoveDownButtonEnabled = false;
      this.listBuildChainProjects.MoveUpButtonEnabled = false;
      this.listBuildChainProjects.MustHaveOneElement = false;
      this.listBuildChainProjects.Name = "listBuildChainProjects";
      this.listBuildChainProjects.SelectedIndex = -1;
      this.listBuildChainProjects.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      this.listBuildChainProjects.SelectionTextColor = System.Drawing.SystemColors.HighlightText;
      this.listBuildChainProjects.Size = new System.Drawing.Size(221, 244);
      this.listBuildChainProjects.TabIndex = 11;
      this.listBuildChainProjects.AddingItem += new RetroDevStudio.Controls.ArrangedItemList.AddingItemEventHandler(this.listBuildChainProjects_AddingItem);
      this.listBuildChainProjects.CloningItem += new RetroDevStudio.Controls.ArrangedItemList.CloningItemEventHandler(this.listBuildChainProjects_CloningItem);
      this.listBuildChainProjects.ItemAdded += new RetroDevStudio.Controls.ArrangedItemList.ItemModifiedEventHandler(this.listBuildChainProjects_ItemAdded);
      this.listBuildChainProjects.ItemRemoved += new RetroDevStudio.Controls.ArrangedItemList.ItemModifiedEventHandler(this.listBuildChainProjects_ItemRemoved);
      this.listBuildChainProjects.ItemMoved += new RetroDevStudio.Controls.ArrangedItemList.ItemExchangedEventHandler(this.listBuildChainProjects_ItemMoved);
      this.listBuildChainProjects.SelectedIndexChanged += new RetroDevStudio.Controls.ArrangedItemList.ItemModifiedEventHandler(this.listBuildChainProjects_SelectedIndexChanged);
      // 
      // labelDefines
      // 
      this.labelDefines.AutoSize = true;
      this.labelDefines.Enabled = false;
      this.labelDefines.Location = new System.Drawing.Point(232, 96);
      this.labelDefines.Name = "labelDefines";
      this.labelDefines.Size = new System.Drawing.Size(46, 13);
      this.labelDefines.TabIndex = 8;
      this.labelDefines.Text = "Defines:";
      // 
      // labelConfig
      // 
      this.labelConfig.AutoSize = true;
      this.labelConfig.Enabled = false;
      this.labelConfig.Location = new System.Drawing.Point(232, 41);
      this.labelConfig.Name = "labelConfig";
      this.labelConfig.Size = new System.Drawing.Size(40, 13);
      this.labelConfig.TabIndex = 9;
      this.labelConfig.Text = "Config:";
      // 
      // labelProject
      // 
      this.labelProject.AutoSize = true;
      this.labelProject.Enabled = false;
      this.labelProject.Location = new System.Drawing.Point(232, 16);
      this.labelProject.Name = "labelProject";
      this.labelProject.Size = new System.Drawing.Size(43, 13);
      this.labelProject.TabIndex = 10;
      this.labelProject.Text = "Project:";
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
      // labelFile
      // 
      this.labelFile.AutoSize = true;
      this.labelFile.Enabled = false;
      this.labelFile.Location = new System.Drawing.Point(232, 67);
      this.labelFile.Name = "labelFile";
      this.labelFile.Size = new System.Drawing.Size(26, 13);
      this.labelFile.TabIndex = 9;
      this.labelFile.Text = "File:";
      // 
      // comboBuildChainFile
      // 
      this.comboBuildChainFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
      this.Controls.Add(this.labelDefines);
      this.Controls.Add(this.labelFile);
      this.Controls.Add(this.labelConfig);
      this.Controls.Add(this.labelProject);
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
    private Controls.ArrangedItemList listBuildChainProjects;
    private System.Windows.Forms.Label labelDefines;
    private System.Windows.Forms.Label labelConfig;
    private System.Windows.Forms.Label labelProject;
    private System.Windows.Forms.CheckBox checkBuildChainActive;
    private System.Windows.Forms.Label labelFile;
    private System.Windows.Forms.ComboBox comboBuildChainFile;


  }
}
