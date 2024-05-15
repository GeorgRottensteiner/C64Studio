namespace RetroDevStudio.Controls
{
  partial class ArrangedItemList
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArrangedItemList));
      this.listItems = new System.Windows.Forms.ListBox();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.btnMoveDown = new DecentForms.Button();
      this.btnMoveUp = new DecentForms.Button();
      this.btnDelete = new DecentForms.Button();
      this.btnAdd = new DecentForms.Button();
      this.toolTipArrangedList = new System.Windows.Forms.ToolTip(this.components);
      this.btnClone = new DecentForms.Button();
      this.SuspendLayout();
      // 
      // listItems
      // 
      this.listItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.listItems.Location = new System.Drawing.Point(3, 3);
      this.listItems.Name = "listItems";
      this.listItems.Size = new System.Drawing.Size(234, 199);
      this.listItems.TabIndex = 0;
      this.listItems.SelectedIndexChanged += new System.EventHandler(this.listItems_SelectedIndexChanged);
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Entries";
      this.columnHeader1.Width = 300;
      // 
      // btnMoveDown
      // 
      this.btnMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnMoveDown.Enabled = false;
      this.btnMoveDown.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveDown.Image")));
      this.btnMoveDown.Location = new System.Drawing.Point(203, 211);
      this.btnMoveDown.Name = "btnMoveDown";
      this.btnMoveDown.Size = new System.Drawing.Size(29, 23);
      this.btnMoveDown.TabIndex = 5;
      this.toolTipArrangedList.SetToolTip(this.btnMoveDown, "Move Entry Down");
      this.btnMoveDown.Click += new DecentForms.EventHandler(this.btnMoveDown_Click);
      // 
      // btnMoveUp
      // 
      this.btnMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnMoveUp.Enabled = false;
      this.btnMoveUp.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveUp.Image")));
      this.btnMoveUp.Location = new System.Drawing.Point(153, 211);
      this.btnMoveUp.Name = "btnMoveUp";
      this.btnMoveUp.Size = new System.Drawing.Size(29, 23);
      this.btnMoveUp.TabIndex = 4;
      this.toolTipArrangedList.SetToolTip(this.btnMoveUp, "Move Entry Up");
      this.btnMoveUp.Click += new DecentForms.EventHandler(this.btnMoveUp_Click);
      // 
      // btnDelete
      // 
      this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnDelete.Enabled = false;
      this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
      this.btnDelete.Location = new System.Drawing.Point(103, 211);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new System.Drawing.Size(29, 23);
      this.btnDelete.TabIndex = 3;
      this.toolTipArrangedList.SetToolTip(this.btnDelete, "Delete Entry");
      this.btnDelete.Click += new DecentForms.EventHandler(this.btnDelete_Click);
      // 
      // btnAdd
      // 
      this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
      this.btnAdd.Location = new System.Drawing.Point(3, 211);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new System.Drawing.Size(29, 23);
      this.btnAdd.TabIndex = 1;
      this.toolTipArrangedList.SetToolTip(this.btnAdd, "Add Entry");
      this.btnAdd.Click += new DecentForms.EventHandler(this.btnAdd_Click);
      // 
      // btnClone
      // 
      this.btnClone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnClone.Image = ((System.Drawing.Image)(resources.GetObject("btnClone.Image")));
      this.btnClone.Location = new System.Drawing.Point(54, 211);
      this.btnClone.Name = "btnClone";
      this.btnClone.Size = new System.Drawing.Size(29, 23);
      this.btnClone.TabIndex = 2;
      this.toolTipArrangedList.SetToolTip(this.btnClone, "Clone Entry");
      this.btnClone.Click += new DecentForms.EventHandler(this.btnClone_Click);
      // 
      // ArrangedItemList
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.Controls.Add(this.btnClone);
      this.Controls.Add(this.btnMoveDown);
      this.Controls.Add(this.btnMoveUp);
      this.Controls.Add(this.btnDelete);
      this.Controls.Add(this.btnAdd);
      this.Controls.Add(this.listItems);
      this.Name = "ArrangedItemList";
      this.Size = new System.Drawing.Size(240, 237);
      this.SizeChanged += new System.EventHandler(this.ArrangedItemList_SizeChanged);
      this.ResumeLayout(false);

    }

    #endregion

    internal System.Windows.Forms.ListBox listItems;
    private DecentForms.Button btnAdd;
    private DecentForms.Button btnDelete;
    private DecentForms.Button btnMoveUp;
    private DecentForms.Button btnMoveDown;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ToolTip toolTipArrangedList;
    private DecentForms.Button btnClone;
  }
}
