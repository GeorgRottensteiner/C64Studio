namespace RetroDevStudio.Documents
{
  partial class PathEditor
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PathEditor));
      tabPathEditor = new System.Windows.Forms.TabControl();
      tabEditor = new System.Windows.Forms.TabPage();
      labelStepLength = new System.Windows.Forms.Label();
      labelStepType = new System.Windows.Forms.Label();
      label3 = new System.Windows.Forms.Label();
      label6 = new System.Windows.Forms.Label();
      label2 = new System.Windows.Forms.Label();
      label1 = new System.Windows.Forms.Label();
      editStepLength = new System.Windows.Forms.TextBox();
      editPathName = new System.Windows.Forms.TextBox();
      listPathSteps = new RetroDevStudio.Controls.ArrangedItemList();
      listPaths = new RetroDevStudio.Controls.ArrangedItemList();
      comboStepTypes = new System.Windows.Forms.ComboBox();
      pictureEditor = new GR.Forms.FastPictureBox();
      tabExport = new System.Windows.Forms.TabPage();
      ( (System.ComponentModel.ISupportInitialize)m_FileWatcher ).BeginInit();
      tabPathEditor.SuspendLayout();
      tabEditor.SuspendLayout();
      ( (System.ComponentModel.ISupportInitialize)pictureEditor ).BeginInit();
      SuspendLayout();
      // 
      // tabPathEditor
      // 
      tabPathEditor.Controls.Add( tabEditor );
      tabPathEditor.Controls.Add( tabExport );
      tabPathEditor.Dock = System.Windows.Forms.DockStyle.Fill;
      tabPathEditor.Location = new System.Drawing.Point( 0, 0 );
      tabPathEditor.Name = "tabPathEditor";
      tabPathEditor.SelectedIndex = 0;
      tabPathEditor.Size = new System.Drawing.Size( 774, 534 );
      tabPathEditor.TabIndex = 8;
      // 
      // tabEditor
      // 
      tabEditor.Controls.Add( labelStepLength );
      tabEditor.Controls.Add( labelStepType );
      tabEditor.Controls.Add( label3 );
      tabEditor.Controls.Add( label6 );
      tabEditor.Controls.Add( label2 );
      tabEditor.Controls.Add( label1 );
      tabEditor.Controls.Add( editStepLength );
      tabEditor.Controls.Add( editPathName );
      tabEditor.Controls.Add( listPathSteps );
      tabEditor.Controls.Add( listPaths );
      tabEditor.Controls.Add( comboStepTypes );
      tabEditor.Controls.Add( pictureEditor );
      tabEditor.Location = new System.Drawing.Point( 4, 22 );
      tabEditor.Name = "tabEditor";
      tabEditor.Padding = new System.Windows.Forms.Padding( 3 );
      tabEditor.Size = new System.Drawing.Size( 766, 508 );
      tabEditor.TabIndex = 0;
      tabEditor.Text = "Editor";
      tabEditor.UseVisualStyleBackColor = true;
      // 
      // labelStepLength
      // 
      labelStepLength.Anchor =  System.Windows.Forms.AnchorStyles.Bottom  |  System.Windows.Forms.AnchorStyles.Left ;
      labelStepLength.AutoSize = true;
      labelStepLength.Enabled = false;
      labelStepLength.Location = new System.Drawing.Point( 217, 464 );
      labelStepLength.Name = "labelStepLength";
      labelStepLength.Size = new System.Drawing.Size( 101, 13 );
      labelStepLength.TabIndex = 31;
      labelStepLength.Text = "Step Length/Count:";
      // 
      // labelStepType
      // 
      labelStepType.Anchor =  System.Windows.Forms.AnchorStyles.Bottom  |  System.Windows.Forms.AnchorStyles.Left ;
      labelStepType.AutoSize = true;
      labelStepType.Enabled = false;
      labelStepType.Location = new System.Drawing.Point( 217, 424 );
      labelStepType.Name = "labelStepType";
      labelStepType.Size = new System.Drawing.Size( 59, 13 );
      labelStepType.TabIndex = 30;
      labelStepType.Text = "Step Type:";
      // 
      // label3
      // 
      label3.Anchor =  System.Windows.Forms.AnchorStyles.Bottom  |  System.Windows.Forms.AnchorStyles.Left ;
      label3.AutoSize = true;
      label3.Location = new System.Drawing.Point( 8, 464 );
      label3.Name = "label3";
      label3.Size = new System.Drawing.Size( 63, 13 );
      label3.TabIndex = 29;
      label3.Text = "Path Name:";
      // 
      // label6
      // 
      label6.AutoSize = true;
      label6.Location = new System.Drawing.Point( 426, 8 );
      label6.Name = "label6";
      label6.Size = new System.Drawing.Size( 73, 13 );
      label6.TabIndex = 28;
      label6.Text = "Path Preview:";
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new System.Drawing.Point( 217, 8 );
      label2.Name = "label2";
      label2.Size = new System.Drawing.Size( 62, 13 );
      label2.TabIndex = 27;
      label2.Text = "Path Steps:";
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new System.Drawing.Point( 8, 8 );
      label1.Name = "label1";
      label1.Size = new System.Drawing.Size( 37, 13 );
      label1.TabIndex = 26;
      label1.Text = "Paths:";
      // 
      // editStepLength
      // 
      editStepLength.Anchor =  System.Windows.Forms.AnchorStyles.Bottom  |  System.Windows.Forms.AnchorStyles.Left ;
      editStepLength.Enabled = false;
      editStepLength.Location = new System.Drawing.Point( 217, 480 );
      editStepLength.Name = "editStepLength";
      editStepLength.Size = new System.Drawing.Size( 203, 20 );
      editStepLength.TabIndex = 25;
      // 
      // editPathName
      // 
      editPathName.Anchor =  System.Windows.Forms.AnchorStyles.Bottom  |  System.Windows.Forms.AnchorStyles.Left ;
      editPathName.Location = new System.Drawing.Point( 8, 480 );
      editPathName.Name = "editPathName";
      editPathName.Size = new System.Drawing.Size( 203, 20 );
      editPathName.TabIndex = 24;
      editPathName.TextChanged +=  editPathName_TextChanged ;
      // 
      // listPathSteps
      // 
      listPathSteps.AddButtonEnabled = true;
      listPathSteps.AllowClone = true;
      listPathSteps.AllowDragReordering = true;
      listPathSteps.Anchor =   System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Bottom   |  System.Windows.Forms.AnchorStyles.Left ;
      listPathSteps.DeleteButtonEnabled = false;
      listPathSteps.Enabled = false;
      listPathSteps.HasOwnerDrawColumn = false;
      listPathSteps.HighlightColor = System.Drawing.SystemColors.HotTrack;
      listPathSteps.HighlightTextColor = System.Drawing.SystemColors.HighlightText;
      listPathSteps.Location = new System.Drawing.Point( 217, 24 );
      listPathSteps.MoveDownButtonEnabled = false;
      listPathSteps.MoveUpButtonEnabled = false;
      listPathSteps.MustHaveOneElement = false;
      listPathSteps.Name = "listPathSteps";
      listPathSteps.SelectedIndex = -1;
      listPathSteps.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      listPathSteps.SelectionTextColor = System.Drawing.SystemColors.HighlightText;
      listPathSteps.Size = new System.Drawing.Size( 203, 397 );
      listPathSteps.TabIndex = 23;
      listPathSteps.ItemAdded +=  listPathSteps_ItemAdded ;
      listPathSteps.ItemRemoved +=  listPathSteps_ItemRemoved ;
      listPathSteps.ItemMoved +=  listPathSteps_ItemMoved ;
      listPathSteps.SelectedIndexChanged +=  listPathSteps_SelectedIndexChanged ;
      // 
      // listPaths
      // 
      listPaths.AddButtonEnabled = false;
      listPaths.AllowClone = true;
      listPaths.AllowDragReordering = true;
      listPaths.Anchor =   System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Bottom   |  System.Windows.Forms.AnchorStyles.Left ;
      listPaths.DeleteButtonEnabled = false;
      listPaths.HasOwnerDrawColumn = false;
      listPaths.HighlightColor = System.Drawing.SystemColors.HotTrack;
      listPaths.HighlightTextColor = System.Drawing.SystemColors.HighlightText;
      listPaths.Location = new System.Drawing.Point( 8, 24 );
      listPaths.MoveDownButtonEnabled = false;
      listPaths.MoveUpButtonEnabled = false;
      listPaths.MustHaveOneElement = false;
      listPaths.Name = "listPaths";
      listPaths.SelectedIndex = -1;
      listPaths.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      listPaths.SelectionTextColor = System.Drawing.SystemColors.HighlightText;
      listPaths.Size = new System.Drawing.Size( 203, 437 );
      listPaths.TabIndex = 22;
      listPaths.ItemAdded +=  listPaths_ItemAdded ;
      listPaths.ItemRemoved +=  listPaths_ItemRemoved ;
      listPaths.ItemMoved +=  listPaths_ItemMoved ;
      listPaths.SelectedIndexChanged +=  listPaths_SelectedIndexChanged ;
      // 
      // comboStepTypes
      // 
      comboStepTypes.Anchor =  System.Windows.Forms.AnchorStyles.Bottom  |  System.Windows.Forms.AnchorStyles.Left ;
      comboStepTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      comboStepTypes.Enabled = false;
      comboStepTypes.FormattingEnabled = true;
      comboStepTypes.Location = new System.Drawing.Point( 217, 440 );
      comboStepTypes.Name = "comboStepTypes";
      comboStepTypes.Size = new System.Drawing.Size( 203, 21 );
      comboStepTypes.TabIndex = 21;
      // 
      // pictureEditor
      // 
      pictureEditor.Anchor =    System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Bottom   |  System.Windows.Forms.AnchorStyles.Left   |  System.Windows.Forms.AnchorStyles.Right ;
      pictureEditor.AutoResize = false;
      pictureEditor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      pictureEditor.Location = new System.Drawing.Point( 426, 24 );
      pictureEditor.Name = "pictureEditor";
      pictureEditor.Size = new System.Drawing.Size( 332, 476 );
      pictureEditor.TabIndex = 20;
      pictureEditor.TabStop = false;
      // 
      // tabExport
      // 
      tabExport.Location = new System.Drawing.Point( 4, 22 );
      tabExport.Name = "tabExport";
      tabExport.Padding = new System.Windows.Forms.Padding( 3 );
      tabExport.Size = new System.Drawing.Size( 766, 508 );
      tabExport.TabIndex = 1;
      tabExport.Text = "Export";
      tabExport.UseVisualStyleBackColor = true;
      // 
      // PathEditor
      // 
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      ClientSize = new System.Drawing.Size( 774, 534 );
      Controls.Add( tabPathEditor );
      Icon = (System.Drawing.Icon)resources.GetObject( "$this.Icon" );
      MinimumSize = new System.Drawing.Size( 274, 159 );
      Name = "PathEditor";
      Text = "Path Editor";
      ( (System.ComponentModel.ISupportInitialize)m_FileWatcher ).EndInit();
      tabPathEditor.ResumeLayout( false );
      tabEditor.ResumeLayout( false );
      tabEditor.PerformLayout();
      ( (System.ComponentModel.ISupportInitialize)pictureEditor ).EndInit();
      ResumeLayout( false );

    }

    #endregion

    private System.Windows.Forms.TabControl tabPathEditor;
    private System.Windows.Forms.TabPage tabEditor;
    private System.Windows.Forms.TabPage tabExport;
    private System.Windows.Forms.Label labelStepLength;
    private System.Windows.Forms.Label labelStepType;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox editStepLength;
    private System.Windows.Forms.TextBox editPathName;
    private Controls.ArrangedItemList listPathSteps;
    private Controls.ArrangedItemList listPaths;
    private System.Windows.Forms.ComboBox comboStepTypes;
    private GR.Forms.FastPictureBox pictureEditor;
  }
}
