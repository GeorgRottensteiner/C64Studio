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
      tabValueMapping = new System.Windows.Forms.TabPage();
      label4 = new System.Windows.Forms.Label();
      tabExport = new System.Windows.Forms.TabPage();
      label7 = new System.Windows.Forms.Label();
      textBox1 = new System.Windows.Forms.TextBox();
      label5 = new System.Windows.Forms.Label();
      textBox2 = new System.Windows.Forms.TextBox();
      label8 = new System.Windows.Forms.Label();
      textBox3 = new System.Windows.Forms.TextBox();
      label9 = new System.Windows.Forms.Label();
      textBox4 = new System.Windows.Forms.TextBox();
      label10 = new System.Windows.Forms.Label();
      textBox5 = new System.Windows.Forms.TextBox();
      groupBox1 = new System.Windows.Forms.GroupBox();
      groupBox2 = new System.Windows.Forms.GroupBox();
      textBox6 = new System.Windows.Forms.TextBox();
      textBox7 = new System.Windows.Forms.TextBox();
      label11 = new System.Windows.Forms.Label();
      textBox8 = new System.Windows.Forms.TextBox();
      label13 = new System.Windows.Forms.Label();
      label14 = new System.Windows.Forms.Label();
      textBox9 = new System.Windows.Forms.TextBox();
      label15 = new System.Windows.Forms.Label();
      listMappings = new DecentForms.ListBox();
      ( (System.ComponentModel.ISupportInitialize)m_FileWatcher ).BeginInit();
      tabPathEditor.SuspendLayout();
      tabEditor.SuspendLayout();
      ( (System.ComponentModel.ISupportInitialize)pictureEditor ).BeginInit();
      tabValueMapping.SuspendLayout();
      groupBox1.SuspendLayout();
      groupBox2.SuspendLayout();
      SuspendLayout();
      // 
      // tabPathEditor
      // 
      tabPathEditor.Controls.Add( tabEditor );
      tabPathEditor.Controls.Add( tabValueMapping );
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
      editStepLength.TextChanged +=  editStepLength_TextChanged ;
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
      listPathSteps.AllowReordering = true;
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
      listPaths.AllowReordering = true;
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
      listPaths.CloningItem +=  listPaths_CloningItem ;
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
      comboStepTypes.SelectedIndexChanged +=  comboStepTypes_SelectedIndexChanged ;
      // 
      // pictureEditor
      // 
      pictureEditor.Anchor =    System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Bottom   |  System.Windows.Forms.AnchorStyles.Left   |  System.Windows.Forms.AnchorStyles.Right ;
      pictureEditor.AutoResize = true;
      pictureEditor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      pictureEditor.Location = new System.Drawing.Point( 426, 24 );
      pictureEditor.Name = "pictureEditor";
      pictureEditor.Size = new System.Drawing.Size( 332, 476 );
      pictureEditor.TabIndex = 20;
      pictureEditor.TabStop = false;
      // 
      // tabValueMapping
      // 
      tabValueMapping.Controls.Add( listMappings );
      tabValueMapping.Controls.Add( groupBox2 );
      tabValueMapping.Controls.Add( groupBox1 );
      tabValueMapping.Controls.Add( label4 );
      tabValueMapping.Location = new System.Drawing.Point( 4, 22 );
      tabValueMapping.Name = "tabValueMapping";
      tabValueMapping.Padding = new System.Windows.Forms.Padding( 3 );
      tabValueMapping.Size = new System.Drawing.Size( 766, 508 );
      tabValueMapping.TabIndex = 2;
      tabValueMapping.Text = "ValueMapping";
      tabValueMapping.UseVisualStyleBackColor = true;
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Location = new System.Drawing.Point( 8, 8 );
      label4.Name = "label4";
      label4.Size = new System.Drawing.Size( 56, 13 );
      label4.TabIndex = 27;
      label4.Text = "Mappings:";
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
      // label7
      // 
      label7.AutoSize = true;
      label7.Location = new System.Drawing.Point( 6, 22 );
      label7.Name = "label7";
      label7.Size = new System.Drawing.Size( 79, 13 );
      label7.TabIndex = 27;
      label7.Text = "Address Offset:";
      // 
      // textBox1
      // 
      textBox1.Location = new System.Drawing.Point( 114, 19 );
      textBox1.Name = "textBox1";
      textBox1.Size = new System.Drawing.Size( 121, 20 );
      textBox1.TabIndex = 30;
      // 
      // label5
      // 
      label5.AutoSize = true;
      label5.Location = new System.Drawing.Point( 6, 48 );
      label5.Name = "label5";
      label5.Size = new System.Drawing.Size( 37, 13 );
      label5.TabIndex = 27;
      label5.Text = "Value:";
      // 
      // textBox2
      // 
      textBox2.Location = new System.Drawing.Point( 114, 45 );
      textBox2.Name = "textBox2";
      textBox2.Size = new System.Drawing.Size( 121, 20 );
      textBox2.TabIndex = 30;
      // 
      // label8
      // 
      label8.AutoSize = true;
      label8.Location = new System.Drawing.Point( 6, 74 );
      label8.Name = "label8";
      label8.Size = new System.Drawing.Size( 72, 13 );
      label8.TabIndex = 27;
      label8.Text = "Shift Bits Left:";
      // 
      // textBox3
      // 
      textBox3.Location = new System.Drawing.Point( 114, 71 );
      textBox3.Name = "textBox3";
      textBox3.Size = new System.Drawing.Size( 121, 20 );
      textBox3.TabIndex = 30;
      // 
      // label9
      // 
      label9.AutoSize = true;
      label9.Location = new System.Drawing.Point( 6, 100 );
      label9.Name = "label9";
      label9.Size = new System.Drawing.Size( 79, 13 );
      label9.TabIndex = 27;
      label9.Text = "Shift Bits Right:";
      // 
      // textBox4
      // 
      textBox4.Location = new System.Drawing.Point( 114, 97 );
      textBox4.Name = "textBox4";
      textBox4.Size = new System.Drawing.Size( 121, 20 );
      textBox4.TabIndex = 30;
      // 
      // label10
      // 
      label10.AutoSize = true;
      label10.Location = new System.Drawing.Point( 6, 126 );
      label10.Name = "label10";
      label10.Size = new System.Drawing.Size( 73, 13 );
      label10.TabIndex = 27;
      label10.Text = "Relevant Bits:";
      // 
      // textBox5
      // 
      textBox5.Location = new System.Drawing.Point( 114, 123 );
      textBox5.Name = "textBox5";
      textBox5.Size = new System.Drawing.Size( 121, 20 );
      textBox5.TabIndex = 30;
      // 
      // groupBox1
      // 
      groupBox1.Controls.Add( textBox1 );
      groupBox1.Controls.Add( textBox5 );
      groupBox1.Controls.Add( label7 );
      groupBox1.Controls.Add( textBox4 );
      groupBox1.Controls.Add( label5 );
      groupBox1.Controls.Add( label10 );
      groupBox1.Controls.Add( label8 );
      groupBox1.Controls.Add( textBox3 );
      groupBox1.Controls.Add( textBox2 );
      groupBox1.Controls.Add( label9 );
      groupBox1.Location = new System.Drawing.Point( 217, 24 );
      groupBox1.Name = "groupBox1";
      groupBox1.Size = new System.Drawing.Size( 253, 159 );
      groupBox1.TabIndex = 31;
      groupBox1.TabStop = false;
      groupBox1.Text = "Step";
      // 
      // groupBox2
      // 
      groupBox2.Controls.Add( textBox6 );
      groupBox2.Controls.Add( textBox7 );
      groupBox2.Controls.Add( label11 );
      groupBox2.Controls.Add( textBox8 );
      groupBox2.Controls.Add( label13 );
      groupBox2.Controls.Add( label14 );
      groupBox2.Controls.Add( textBox9 );
      groupBox2.Controls.Add( label15 );
      groupBox2.Location = new System.Drawing.Point( 217, 189 );
      groupBox2.Name = "groupBox2";
      groupBox2.Size = new System.Drawing.Size( 253, 131 );
      groupBox2.TabIndex = 31;
      groupBox2.TabStop = false;
      groupBox2.Text = "Duration";
      // 
      // textBox6
      // 
      textBox6.Location = new System.Drawing.Point( 114, 19 );
      textBox6.Name = "textBox6";
      textBox6.Size = new System.Drawing.Size( 121, 20 );
      textBox6.TabIndex = 30;
      // 
      // textBox7
      // 
      textBox7.Location = new System.Drawing.Point( 114, 97 );
      textBox7.Name = "textBox7";
      textBox7.Size = new System.Drawing.Size( 121, 20 );
      textBox7.TabIndex = 30;
      // 
      // label11
      // 
      label11.AutoSize = true;
      label11.Location = new System.Drawing.Point( 6, 22 );
      label11.Name = "label11";
      label11.Size = new System.Drawing.Size( 79, 13 );
      label11.TabIndex = 27;
      label11.Text = "Address Offset:";
      // 
      // textBox8
      // 
      textBox8.Location = new System.Drawing.Point( 114, 71 );
      textBox8.Name = "textBox8";
      textBox8.Size = new System.Drawing.Size( 121, 20 );
      textBox8.TabIndex = 30;
      // 
      // label13
      // 
      label13.AutoSize = true;
      label13.Location = new System.Drawing.Point( 6, 100 );
      label13.Name = "label13";
      label13.Size = new System.Drawing.Size( 73, 13 );
      label13.TabIndex = 27;
      label13.Text = "Relevant Bits:";
      // 
      // label14
      // 
      label14.AutoSize = true;
      label14.Location = new System.Drawing.Point( 6, 48 );
      label14.Name = "label14";
      label14.Size = new System.Drawing.Size( 72, 13 );
      label14.TabIndex = 27;
      label14.Text = "Shift Bits Left:";
      // 
      // textBox9
      // 
      textBox9.Location = new System.Drawing.Point( 114, 45 );
      textBox9.Name = "textBox9";
      textBox9.Size = new System.Drawing.Size( 121, 20 );
      textBox9.TabIndex = 30;
      // 
      // label15
      // 
      label15.AutoSize = true;
      label15.Location = new System.Drawing.Point( 6, 74 );
      label15.Name = "label15";
      label15.Size = new System.Drawing.Size( 79, 13 );
      label15.TabIndex = 27;
      label15.Text = "Shift Bits Right:";
      // 
      // listMappings
      // 
      listMappings.Location = new System.Drawing.Point( 8, 24 );
      listMappings.Name = "listMappings";
      listMappings.Size = new System.Drawing.Size( 203, 472 );
      listMappings.TabIndex = 32;
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
      tabValueMapping.ResumeLayout( false );
      tabValueMapping.PerformLayout();
      groupBox1.ResumeLayout( false );
      groupBox1.PerformLayout();
      groupBox2.ResumeLayout( false );
      groupBox2.PerformLayout();
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
    private System.Windows.Forms.TabPage tabValueMapping;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox textBox2;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox textBox4;
    private System.Windows.Forms.TextBox textBox3;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.TextBox textBox5;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.TextBox textBox6;
    private System.Windows.Forms.TextBox textBox7;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.TextBox textBox8;
    private System.Windows.Forms.Label label13;
    private System.Windows.Forms.Label label14;
    private System.Windows.Forms.TextBox textBox9;
    private System.Windows.Forms.Label label15;
    private DecentForms.ListBox listMappings;
  }
}
