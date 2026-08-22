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
      components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArrangedItemList));
      listItems = new DecentForms.ListBox();
      columnHeader1 = new System.Windows.Forms.ColumnHeader();
      btnMoveDown = new DecentForms.Button();
      btnMoveUp = new DecentForms.Button();
      btnDelete = new DecentForms.Button();
      btnAdd = new DecentForms.Button();
      toolTipArrangedList = new System.Windows.Forms.ToolTip( components );
      btnClone = new DecentForms.Button();
      SuspendLayout();
      // 
      // listItems
      // 
      listItems.AllowDrag = true;
      listItems.AllowDrop = true;
      listItems.Anchor =    System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Bottom   |  System.Windows.Forms.AnchorStyles.Left   |  System.Windows.Forms.AnchorStyles.Right ;
      listItems.BorderStyle = DecentForms.BorderStyle.FLAT;
      listItems.DisplayAntiAliased = true;
      listItems.HasCheckBoxes = false;
      listItems.ItemHeight = 15;
      listItems.Location = new System.Drawing.Point( 0, 0 );
      listItems.Name = "listItems";
      listItems.ScrollAlwaysVisible = false;
      listItems.SelectionMode = DecentForms.SelectionMode.NONE;
      listItems.Size = new System.Drawing.Size( 240, 205 );
      listItems.TabIndex = 0;
      listItems.SelectedIndexChanged +=  listItems_SelectedIndexChanged ;
      listItems.ItemSwapping +=  listItems_ItemSwapping ;
      listItems.ItemSwapped +=  listItems_ItemSwapped ;
      // 
      // columnHeader1
      // 
      columnHeader1.Text = "Entries";
      columnHeader1.Width = 300;
      // 
      // btnMoveDown
      // 
      btnMoveDown.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnMoveDown.Anchor =  System.Windows.Forms.AnchorStyles.Bottom  |  System.Windows.Forms.AnchorStyles.Left ;
      btnMoveDown.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnMoveDown.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnMoveDown.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnMoveDown.DisplayAntiAliased = true;
      btnMoveDown.Enabled = false;
      btnMoveDown.Image = (System.Drawing.Image)resources.GetObject( "btnMoveDown.Image" );
      btnMoveDown.Location = new System.Drawing.Point( 203, 211 );
      btnMoveDown.Name = "btnMoveDown";
      btnMoveDown.Size = new System.Drawing.Size( 29, 23 );
      btnMoveDown.TabIndex = 5;
      toolTipArrangedList.SetToolTip( btnMoveDown, "Move Entry Down" );
      btnMoveDown.Click +=  btnMoveDown_Click ;
      // 
      // btnMoveUp
      // 
      btnMoveUp.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnMoveUp.Anchor =  System.Windows.Forms.AnchorStyles.Bottom  |  System.Windows.Forms.AnchorStyles.Left ;
      btnMoveUp.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnMoveUp.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnMoveUp.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnMoveUp.DisplayAntiAliased = true;
      btnMoveUp.Enabled = false;
      btnMoveUp.Image = (System.Drawing.Image)resources.GetObject( "btnMoveUp.Image" );
      btnMoveUp.Location = new System.Drawing.Point( 153, 211 );
      btnMoveUp.Name = "btnMoveUp";
      btnMoveUp.Size = new System.Drawing.Size( 29, 23 );
      btnMoveUp.TabIndex = 4;
      toolTipArrangedList.SetToolTip( btnMoveUp, "Move Entry Up" );
      btnMoveUp.Click +=  btnMoveUp_Click ;
      // 
      // btnDelete
      // 
      btnDelete.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnDelete.Anchor =  System.Windows.Forms.AnchorStyles.Bottom  |  System.Windows.Forms.AnchorStyles.Left ;
      btnDelete.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnDelete.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnDelete.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnDelete.DisplayAntiAliased = true;
      btnDelete.Enabled = false;
      btnDelete.Image = (System.Drawing.Image)resources.GetObject( "btnDelete.Image" );
      btnDelete.Location = new System.Drawing.Point( 103, 211 );
      btnDelete.Name = "btnDelete";
      btnDelete.Size = new System.Drawing.Size( 29, 23 );
      btnDelete.TabIndex = 3;
      toolTipArrangedList.SetToolTip( btnDelete, "Delete Entry" );
      btnDelete.Click +=  btnDelete_Click ;
      // 
      // btnAdd
      // 
      btnAdd.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnAdd.Anchor =  System.Windows.Forms.AnchorStyles.Bottom  |  System.Windows.Forms.AnchorStyles.Left ;
      btnAdd.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnAdd.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnAdd.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnAdd.DisplayAntiAliased = true;
      btnAdd.Image = (System.Drawing.Image)resources.GetObject( "btnAdd.Image" );
      btnAdd.Location = new System.Drawing.Point( 3, 211 );
      btnAdd.Name = "btnAdd";
      btnAdd.Size = new System.Drawing.Size( 29, 23 );
      btnAdd.TabIndex = 1;
      toolTipArrangedList.SetToolTip( btnAdd, "Add Entry" );
      btnAdd.Click +=  btnAdd_Click ;
      // 
      // btnClone
      // 
      btnClone.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnClone.Anchor =  System.Windows.Forms.AnchorStyles.Bottom  |  System.Windows.Forms.AnchorStyles.Left ;
      btnClone.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnClone.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnClone.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnClone.DisplayAntiAliased = true;
      btnClone.Image = (System.Drawing.Image)resources.GetObject( "btnClone.Image" );
      btnClone.Location = new System.Drawing.Point( 54, 211 );
      btnClone.Name = "btnClone";
      btnClone.Size = new System.Drawing.Size( 29, 23 );
      btnClone.TabIndex = 2;
      toolTipArrangedList.SetToolTip( btnClone, "Clone Entry" );
      btnClone.Click +=  btnClone_Click ;
      // 
      // ArrangedItemList
      // 
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      Controls.Add( btnClone );
      Controls.Add( btnMoveDown );
      Controls.Add( btnMoveUp );
      Controls.Add( btnDelete );
      Controls.Add( btnAdd );
      Controls.Add( listItems );
      Name = "ArrangedItemList";
      Size = new System.Drawing.Size( 240, 237 );
      SizeChanged +=  ArrangedItemList_SizeChanged ;
      ResumeLayout( false );

    }

    #endregion

    internal DecentForms.ListBox listItems;
    private DecentForms.Button btnAdd;
    private DecentForms.Button btnDelete;
    private DecentForms.Button btnMoveUp;
    private DecentForms.Button btnMoveDown;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ToolTip toolTipArrangedList;
    private DecentForms.Button btnClone;
  }
}
