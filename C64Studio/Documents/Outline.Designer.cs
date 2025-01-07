namespace RetroDevStudio.Documents
{
  partial class Outline
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Outline));
      this.treeProject = new DecentForms.TreeView();
      this.m_ImageListOutline = new System.Windows.Forms.ImageList(this.components);
      this.toolStripOutline = new System.Windows.Forms.ToolStrip();
      this.checkShowLocalLabels = new System.Windows.Forms.ToolStripButton();
      this.checkShowShortCutLabels = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.checkSortBySource = new System.Windows.Forms.ToolStripButton();
      this.checkSortAlphabetically = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.editOutlineFilter = new System.Windows.Forms.ToolStripTextBox();
      this.checkSortByType = new System.Windows.Forms.ToolStripButton();
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      this.toolStripOutline.SuspendLayout();
      this.SuspendLayout();
      // 
      // treeProject
      // 
      this.treeProject.AllowDrag = false;
      this.treeProject.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.treeProject.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.treeProject.ImageList = this.m_ImageListOutline;
      this.treeProject.LabelEdit = false;
      this.treeProject.Location = new System.Drawing.Point(0, 28);
      this.treeProject.Name = "treeProject";
      this.treeProject.ScrollAlwaysVisible = false;
      this.treeProject.SelectedNode = null;
      this.treeProject.SelectionMode = DecentForms.SelectionMode.NONE;
      this.treeProject.Size = new System.Drawing.Size(534, 364);
      this.treeProject.TabIndex = 0;
      this.treeProject.Text = "NoDblClkTreeView";
      this.treeProject.AfterSelect += new DecentForms.TreeView.TreeViewEventHandler(this.treeProject_AfterSelect);
      this.treeProject.NodeMouseClick += new DecentForms.TreeView.TreeNodeMouseClickEventHandler(this.treeProject_NodeMouseClick);
      this.treeProject.NodeMouseDoubleClick += new DecentForms.TreeView.TreeNodeMouseClickEventHandler(this.treeProject_NodeMouseDoubleClick);
      // 
      // m_ImageListOutline
      // 
      this.m_ImageListOutline.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ImageListOutline.ImageStream")));
      this.m_ImageListOutline.TransparentColor = System.Drawing.Color.Transparent;
      this.m_ImageListOutline.Images.SetKeyName(0, "outline-zone.ico");
      this.m_ImageListOutline.Images.SetKeyName(1, "outline-label.ico");
      this.m_ImageListOutline.Images.SetKeyName(2, "outline-constant.ico");
      this.m_ImageListOutline.Images.SetKeyName(3, "outline-preprocessor-constant.ico");
      this.m_ImageListOutline.Images.SetKeyName(4, "outline-macro.ico");
      // 
      // toolStripOutline
      // 
      this.toolStripOutline.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkShowLocalLabels,
            this.checkShowShortCutLabels,
            this.toolStripSeparator1,
            this.checkSortBySource,
            this.checkSortAlphabetically,
            this.checkSortByType,
            this.toolStripSeparator2,
            this.editOutlineFilter});
      this.toolStripOutline.Location = new System.Drawing.Point(0, 0);
      this.toolStripOutline.Name = "toolStripOutline";
      this.toolStripOutline.Size = new System.Drawing.Size(534, 25);
      this.toolStripOutline.TabIndex = 1;
      this.toolStripOutline.Text = "toolStrip1";
      // 
      // checkShowLocalLabels
      // 
      this.checkShowLocalLabels.CheckOnClick = true;
      this.checkShowLocalLabels.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.checkShowLocalLabels.Image = ((System.Drawing.Image)(resources.GetObject("checkShowLocalLabels.Image")));
      this.checkShowLocalLabels.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.checkShowLocalLabels.Name = "checkShowLocalLabels";
      this.checkShowLocalLabels.Size = new System.Drawing.Size(23, 22);
      this.checkShowLocalLabels.Text = "show/hide local labels";
      this.checkShowLocalLabels.Click += new System.EventHandler(this.checkShowLocalLabels_Click);
      // 
      // checkShowShortCutLabels
      // 
      this.checkShowShortCutLabels.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.checkShowShortCutLabels.Image = ((System.Drawing.Image)(resources.GetObject("checkShowShortCutLabels.Image")));
      this.checkShowShortCutLabels.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.checkShowShortCutLabels.Name = "checkShowShortCutLabels";
      this.checkShowShortCutLabels.Size = new System.Drawing.Size(23, 22);
      this.checkShowShortCutLabels.Text = "show/hide short cut labels";
      this.checkShowShortCutLabels.Click += new System.EventHandler(this.checkShowShortCutLabels_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
      // 
      // checkSortBySource
      // 
      this.checkSortBySource.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.checkSortBySource.Enabled = false;
      this.checkSortBySource.Image = ((System.Drawing.Image)(resources.GetObject("checkSortBySource.Image")));
      this.checkSortBySource.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.checkSortBySource.Name = "checkSortBySource";
      this.checkSortBySource.Size = new System.Drawing.Size(23, 22);
      this.checkSortBySource.Text = "Sort by Source";
      this.checkSortBySource.Click += new System.EventHandler(this.checkSortBySource_Click);
      // 
      // checkSortAlphabetically
      // 
      this.checkSortAlphabetically.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.checkSortAlphabetically.Image = ((System.Drawing.Image)(resources.GetObject("checkSortAlphabetically.Image")));
      this.checkSortAlphabetically.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.checkSortAlphabetically.Name = "checkSortAlphabetically";
      this.checkSortAlphabetically.Size = new System.Drawing.Size(23, 22);
      this.checkSortAlphabetically.Text = "Sort alphabetically";
      this.checkSortAlphabetically.Click += new System.EventHandler(this.checkSortAlphabetically_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
      // 
      // editOutlineFilter
      // 
      this.editOutlineFilter.AutoSize = false;
      this.editOutlineFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.editOutlineFilter.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.editOutlineFilter.Name = "editOutlineFilter";
      this.editOutlineFilter.Size = new System.Drawing.Size(200, 23);
      this.editOutlineFilter.ToolTipText = "Label Filter";
      this.editOutlineFilter.TextChanged += new System.EventHandler(this.editOutlineFilter_TextChanged);
      // 
      // checkSortByType
      // 
      this.checkSortByType.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.checkSortByType.Image = ((System.Drawing.Image)(resources.GetObject("checkSortByType.Image")));
      this.checkSortByType.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.checkSortByType.Name = "checkSortByType";
      this.checkSortByType.Size = new System.Drawing.Size(23, 22);
      this.checkSortByType.Text = "Sort by type";
      this.checkSortByType.Click += new System.EventHandler(this.checkSortByType_Click);
      // 
      // Outline
      // 
      this.ClientSize = new System.Drawing.Size(534, 390);
      this.Controls.Add(this.toolStripOutline);
      this.Controls.Add(this.treeProject);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "Outline";
      this.Text = "Outline";
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      this.toolStripOutline.ResumeLayout(false);
      this.toolStripOutline.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    public DecentForms.TreeView treeProject;
    private System.Windows.Forms.ImageList m_ImageListOutline;
    private System.Windows.Forms.ToolStrip toolStripOutline;
    public System.Windows.Forms.ToolStripButton checkShowLocalLabels;
    public System.Windows.Forms.ToolStripButton checkShowShortCutLabels;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    public System.Windows.Forms.ToolStripTextBox editOutlineFilter;
    public System.Windows.Forms.ToolStripButton checkSortBySource;
    public System.Windows.Forms.ToolStripButton checkSortAlphabetically;
    public System.Windows.Forms.ToolStripButton checkSortByType;
  }
}
