namespace C64Studio
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
      //this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
      //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;

      this.listItems = new System.Windows.Forms.ListBox();
      this.btnMoveDown = new System.Windows.Forms.Button();
      this.btnMoveUp = new System.Windows.Forms.Button();
      this.btnDelete = new System.Windows.Forms.Button();
      this.btnAdd = new System.Windows.Forms.Button();
      this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
      this.SuspendLayout();
      // 
      // listItems
      // 
      this.listItems.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                  | System.Windows.Forms.AnchorStyles.Left )
                  | System.Windows.Forms.AnchorStyles.Right ) ) );
      this.listItems.Location = new System.Drawing.Point( 3, 3 );
      this.listItems.Name = "listItems";
      this.listItems.Size = new System.Drawing.Size( 234, 205 );
      this.listItems.TabIndex = 0;
      this.listItems.SelectedIndexChanged += new System.EventHandler( this.listItems_SelectedIndexChanged );

      // 
      // btnMoveDown
      // 
      this.btnMoveDown.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
      this.btnMoveDown.Enabled = false;
      this.btnMoveDown.Image = global::C64Studio.Properties.Resources.arrow_down;
      this.btnMoveDown.Location = new System.Drawing.Point( 186, 210 );
      this.btnMoveDown.Name = "btnMoveDown";
      this.btnMoveDown.Size = new System.Drawing.Size( 50, 23 );
      this.btnMoveDown.TabIndex = 1;
      this.btnMoveDown.UseVisualStyleBackColor = true;
      this.btnMoveDown.Click += new System.EventHandler( this.btnMoveDown_Click );
      // 
      // btnMoveUp
      // 
      this.btnMoveUp.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
      this.btnMoveUp.Enabled = false;
      this.btnMoveUp.Image = global::C64Studio.Properties.Resources.arrow_up;
      this.btnMoveUp.Location = new System.Drawing.Point( 125, 210 );
      this.btnMoveUp.Name = "btnMoveUp";
      this.btnMoveUp.Size = new System.Drawing.Size( 50, 23 );
      this.btnMoveUp.TabIndex = 1;
      this.btnMoveUp.UseVisualStyleBackColor = true;
      this.btnMoveUp.Click += new System.EventHandler( this.btnMoveUp_Click );
      // 
      // btnDelete
      // 
      this.btnDelete.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
      this.btnDelete.Enabled = false;
      this.btnDelete.Image = global::C64Studio.Properties.Resources.delete;
      this.btnDelete.Location = new System.Drawing.Point( 64, 210 );
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new System.Drawing.Size( 50, 23 );
      this.btnDelete.TabIndex = 1;
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new System.EventHandler( this.btnDelete_Click );
      // 
      // btnAdd
      // 
      this.btnAdd.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
      this.btnAdd.Image = global::C64Studio.Properties.Resources.add;
      this.btnAdd.Location = new System.Drawing.Point( 3, 210 );
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new System.Drawing.Size( 50, 23 );
      this.btnAdd.TabIndex = 1;
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new System.EventHandler( this.btnAdd_Click );
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Entries";
      this.columnHeader1.Width = 300;
      // 
      // ArrangedItemList
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add( this.btnMoveDown );
      this.Controls.Add( this.btnMoveUp );
      this.Controls.Add( this.btnDelete );
      this.Controls.Add( this.btnAdd );
      this.Controls.Add( this.listItems );
      this.Name = "ArrangedItemList";
      this.Size = new System.Drawing.Size( 240, 236 );
      this.SizeChanged += new System.EventHandler( this.ArrangedItemList_SizeChanged );
      this.ResumeLayout( false );

    }

    #endregion

    internal System.Windows.Forms.ListBox listItems;
    private System.Windows.Forms.Button btnAdd;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnMoveUp;
    private System.Windows.Forms.Button btnMoveDown;
    private System.Windows.Forms.ColumnHeader columnHeader1;
  }
}
