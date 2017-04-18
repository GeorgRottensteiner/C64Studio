namespace C64Studio
{
  partial class ProjectExplorer
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( ProjectExplorer ) );
      this.treeProject = new C64Studio.NoDblClkTreeView();
      this.imageListExplorer = new System.Windows.Forms.ImageList( this.components );
      this.timerDragDrop = new System.Windows.Forms.Timer( this.components );
      ( (System.ComponentModel.ISupportInitialize)( this.m_FileWatcher ) ).BeginInit();
      this.SuspendLayout();
      // 
      // treeProject
      // 
      this.treeProject.AllowDrop = true;
      this.treeProject.Dock = System.Windows.Forms.DockStyle.Fill;
      this.treeProject.ImageIndex = 0;
      this.treeProject.ImageList = this.imageListExplorer;
      this.treeProject.LabelEdit = true;
      this.treeProject.Location = new System.Drawing.Point( 0, 0 );
      this.treeProject.Name = "treeProject";
      this.treeProject.SelectedImageIndex = 0;
      this.treeProject.Size = new System.Drawing.Size( 534, 390 );
      this.treeProject.TabIndex = 0;
      this.treeProject.Text = "NoDblClkTreeView";
      this.treeProject.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler( this.treeProject_NodeMouseDoubleClick );
      this.treeProject.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler( this.treeProject_AfterLabelEdit );
      this.treeProject.DragDrop += new System.Windows.Forms.DragEventHandler( this.treeProject_DragDrop );
      this.treeProject.DragEnter += new System.Windows.Forms.DragEventHandler( this.treeProject_DragEnter );
      this.treeProject.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler( this.treeProject_NodeMouseClick );
      this.treeProject.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler( this.treeProject_BeforeLabelEdit );
      this.treeProject.KeyDown += new System.Windows.Forms.KeyEventHandler( this.treeProject_KeyDown );
      this.treeProject.ItemDrag += new System.Windows.Forms.ItemDragEventHandler( this.treeProject_ItemDrag );
      this.treeProject.DragOver += new System.Windows.Forms.DragEventHandler( this.treeProject_DragOver );
      // 
      // imageListExplorer
      // 
      this.imageListExplorer.ImageStream = ( (System.Windows.Forms.ImageListStreamer)( resources.GetObject( "imageListExplorer.ImageStream" ) ) );
      this.imageListExplorer.TransparentColor = System.Drawing.Color.Transparent;
      this.imageListExplorer.Images.SetKeyName( 0, "c64.ico" );
      this.imageListExplorer.Images.SetKeyName( 1, "source.ico" );
      this.imageListExplorer.Images.SetKeyName( 2, "spriteset.ico" );
      this.imageListExplorer.Images.SetKeyName( 3, "charset.ico" );
      this.imageListExplorer.Images.SetKeyName( 4, "source_basic.ico" );
      this.imageListExplorer.Images.SetKeyName( 5, "graphicscreen.ico" );
      this.imageListExplorer.Images.SetKeyName( 6, "charsetscreen.ico" );
      this.imageListExplorer.Images.SetKeyName( 7, "mapeditor.ico" );
      this.imageListExplorer.Images.SetKeyName( 8, "folder.ico" );
      this.imageListExplorer.Images.SetKeyName( 9, "solution.ico" );
      this.imageListExplorer.Images.SetKeyName( 10, "project.ico" );
      // 
      // ProjectExplorer
      // 
      this.ClientSize = new System.Drawing.Size( 534, 390 );
      this.Controls.Add( this.treeProject );
      this.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
      this.Icon = ( (System.Drawing.Icon)( resources.GetObject( "$this.Icon" ) ) );
      this.Name = "ProjectExplorer";
      this.Text = "Project Explorer";
      ( (System.ComponentModel.ISupportInitialize)( this.m_FileWatcher ) ).EndInit();
      this.ResumeLayout( false );

    }

    #endregion

    public NoDblClkTreeView treeProject;
    private System.Windows.Forms.ImageList imageListExplorer;
    private System.Windows.Forms.Timer timerDragDrop;

  }
}
