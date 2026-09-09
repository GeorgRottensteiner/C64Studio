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
      listMappings = new RetroDevStudio.Controls.ArrangedItemList();
      groupDurationValues = new System.Windows.Forms.GroupBox();
      editMappingDurationOffset = new System.Windows.Forms.TextBox();
      editMappingDurationMask = new System.Windows.Forms.TextBox();
      label11 = new System.Windows.Forms.Label();
      editMappingDurationShiftRight = new System.Windows.Forms.TextBox();
      label13 = new System.Windows.Forms.Label();
      label14 = new System.Windows.Forms.Label();
      editMappingDurationShiftLeft = new System.Windows.Forms.TextBox();
      label15 = new System.Windows.Forms.Label();
      groupBox1 = new System.Windows.Forms.GroupBox();
      editMappingLastStepAddressOffset = new System.Windows.Forms.TextBox();
      label16 = new System.Windows.Forms.Label();
      label17 = new System.Windows.Forms.Label();
      editMappingLastStepValue = new System.Windows.Forms.TextBox();
      groupStepValues = new System.Windows.Forms.GroupBox();
      comboMappingStepType = new System.Windows.Forms.ComboBox();
      editMappingStepOffset = new System.Windows.Forms.TextBox();
      editMappingStepMask = new System.Windows.Forms.TextBox();
      label9 = new System.Windows.Forms.Label();
      label7 = new System.Windows.Forms.Label();
      label5 = new System.Windows.Forms.Label();
      label10 = new System.Windows.Forms.Label();
      editMappingStepValue = new System.Windows.Forms.TextBox();
      label4 = new System.Windows.Forms.Label();
      tabExport = new System.Windows.Forms.TabPage();
      panelExport = new System.Windows.Forms.Panel();
      btnExport = new DecentForms.Button();
      comboExportMethod = new System.Windows.Forms.ComboBox();
      label8 = new System.Windows.Forms.Label();
      ( (System.ComponentModel.ISupportInitialize)m_FileWatcher ).BeginInit();
      tabPathEditor.SuspendLayout();
      tabEditor.SuspendLayout();
      ( (System.ComponentModel.ISupportInitialize)pictureEditor ).BeginInit();
      tabValueMapping.SuspendLayout();
      groupDurationValues.SuspendLayout();
      groupBox1.SuspendLayout();
      groupStepValues.SuspendLayout();
      tabExport.SuspendLayout();
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
      tabValueMapping.Controls.Add( groupDurationValues );
      tabValueMapping.Controls.Add( groupBox1 );
      tabValueMapping.Controls.Add( groupStepValues );
      tabValueMapping.Controls.Add( label4 );
      tabValueMapping.Location = new System.Drawing.Point( 4, 22 );
      tabValueMapping.Name = "tabValueMapping";
      tabValueMapping.Padding = new System.Windows.Forms.Padding( 3 );
      tabValueMapping.Size = new System.Drawing.Size( 766, 508 );
      tabValueMapping.TabIndex = 2;
      tabValueMapping.Text = "ValueMapping";
      tabValueMapping.UseVisualStyleBackColor = true;
      // 
      // listMappings
      // 
      listMappings.AddButtonEnabled = false;
      listMappings.AllowClone = true;
      listMappings.AllowReordering = true;
      listMappings.Anchor =   System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Bottom   |  System.Windows.Forms.AnchorStyles.Left ;
      listMappings.DeleteButtonEnabled = false;
      listMappings.HasOwnerDrawColumn = false;
      listMappings.HighlightColor = System.Drawing.SystemColors.HotTrack;
      listMappings.HighlightTextColor = System.Drawing.SystemColors.HighlightText;
      listMappings.Location = new System.Drawing.Point( 8, 24 );
      listMappings.MoveDownButtonEnabled = false;
      listMappings.MoveUpButtonEnabled = false;
      listMappings.MustHaveOneElement = false;
      listMappings.Name = "listMappings";
      listMappings.SelectedIndex = -1;
      listMappings.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      listMappings.SelectionTextColor = System.Drawing.SystemColors.HighlightText;
      listMappings.Size = new System.Drawing.Size( 270, 476 );
      listMappings.TabIndex = 0;
      listMappings.CloningItem +=  listMappings_CloningItem ;
      listMappings.ItemAdded +=  listMappings_ItemAdded ;
      listMappings.ItemRemoved +=  listMappings_ItemRemoved ;
      listMappings.ItemMoved +=  listMappings_ItemMoved ;
      listMappings.SelectedIndexChanged +=  listMappings_SelectedIndexChanged ;
      // 
      // groupDurationValues
      // 
      groupDurationValues.Controls.Add( editMappingDurationOffset );
      groupDurationValues.Controls.Add( editMappingDurationMask );
      groupDurationValues.Controls.Add( label11 );
      groupDurationValues.Controls.Add( editMappingDurationShiftRight );
      groupDurationValues.Controls.Add( label13 );
      groupDurationValues.Controls.Add( label14 );
      groupDurationValues.Controls.Add( editMappingDurationShiftLeft );
      groupDurationValues.Controls.Add( label15 );
      groupDurationValues.Location = new System.Drawing.Point( 284, 161 );
      groupDurationValues.Name = "groupDurationValues";
      groupDurationValues.Size = new System.Drawing.Size( 268, 131 );
      groupDurationValues.TabIndex = 31;
      groupDurationValues.TabStop = false;
      groupDurationValues.Text = "Duration";
      // 
      // editMappingDurationOffset
      // 
      editMappingDurationOffset.Location = new System.Drawing.Point( 141, 19 );
      editMappingDurationOffset.MaxLength = 2;
      editMappingDurationOffset.Name = "editMappingDurationOffset";
      editMappingDurationOffset.Size = new System.Drawing.Size( 121, 20 );
      editMappingDurationOffset.TabIndex = 0;
      editMappingDurationOffset.TextChanged +=  editMappingDurationOffset_TextChanged ;
      // 
      // editMappingDurationMask
      // 
      editMappingDurationMask.Location = new System.Drawing.Point( 141, 97 );
      editMappingDurationMask.MaxLength = 4;
      editMappingDurationMask.Name = "editMappingDurationMask";
      editMappingDurationMask.Size = new System.Drawing.Size( 121, 20 );
      editMappingDurationMask.TabIndex = 3;
      editMappingDurationMask.TextChanged +=  editMappingDurationMask_TextChanged ;
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
      // editMappingDurationShiftRight
      // 
      editMappingDurationShiftRight.Location = new System.Drawing.Point( 141, 71 );
      editMappingDurationShiftRight.MaxLength = 2;
      editMappingDurationShiftRight.Name = "editMappingDurationShiftRight";
      editMappingDurationShiftRight.Size = new System.Drawing.Size( 121, 20 );
      editMappingDurationShiftRight.TabIndex = 2;
      editMappingDurationShiftRight.TextChanged +=  editMappingDurationShiftRight_TextChanged ;
      // 
      // label13
      // 
      label13.AutoSize = true;
      label13.Location = new System.Drawing.Point( 6, 100 );
      label13.Name = "label13";
      label13.Size = new System.Drawing.Size( 125, 13 );
      label13.TabIndex = 27;
      label13.Text = "Relevant Bit Mask (Hex):";
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
      // editMappingDurationShiftLeft
      // 
      editMappingDurationShiftLeft.Location = new System.Drawing.Point( 141, 45 );
      editMappingDurationShiftLeft.MaxLength = 2;
      editMappingDurationShiftLeft.Name = "editMappingDurationShiftLeft";
      editMappingDurationShiftLeft.Size = new System.Drawing.Size( 121, 20 );
      editMappingDurationShiftLeft.TabIndex = 1;
      editMappingDurationShiftLeft.TextChanged +=  editMappingDurationShiftLeft_TextChanged ;
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
      // groupBox1
      // 
      groupBox1.Controls.Add( editMappingLastStepAddressOffset );
      groupBox1.Controls.Add( label16 );
      groupBox1.Controls.Add( label17 );
      groupBox1.Controls.Add( editMappingLastStepValue );
      groupBox1.Location = new System.Drawing.Point( 284, 298 );
      groupBox1.Name = "groupBox1";
      groupBox1.Size = new System.Drawing.Size( 268, 80 );
      groupBox1.TabIndex = 31;
      groupBox1.TabStop = false;
      groupBox1.Text = "Last Step";
      // 
      // editMappingLastStepAddressOffset
      // 
      editMappingLastStepAddressOffset.Location = new System.Drawing.Point( 141, 19 );
      editMappingLastStepAddressOffset.MaxLength = 2;
      editMappingLastStepAddressOffset.Name = "editMappingLastStepAddressOffset";
      editMappingLastStepAddressOffset.Size = new System.Drawing.Size( 121, 20 );
      editMappingLastStepAddressOffset.TabIndex = 1;
      editMappingLastStepAddressOffset.TextChanged +=  editMappingLastStepAddressOffset_TextChanged ;
      // 
      // label16
      // 
      label16.AutoSize = true;
      label16.Location = new System.Drawing.Point( 6, 22 );
      label16.Name = "label16";
      label16.Size = new System.Drawing.Size( 79, 13 );
      label16.TabIndex = 27;
      label16.Text = "Address Offset:";
      // 
      // label17
      // 
      label17.AutoSize = true;
      label17.Location = new System.Drawing.Point( 6, 48 );
      label17.Name = "label17";
      label17.Size = new System.Drawing.Size( 65, 13 );
      label17.TabIndex = 27;
      label17.Text = "Value (Hex):";
      // 
      // editMappingLastStepValue
      // 
      editMappingLastStepValue.Location = new System.Drawing.Point( 141, 45 );
      editMappingLastStepValue.Name = "editMappingLastStepValue";
      editMappingLastStepValue.Size = new System.Drawing.Size( 121, 20 );
      editMappingLastStepValue.TabIndex = 2;
      editMappingLastStepValue.TextChanged +=  editMappingLastStepValue_TextChanged ;
      // 
      // groupStepValues
      // 
      groupStepValues.Controls.Add( comboMappingStepType );
      groupStepValues.Controls.Add( editMappingStepOffset );
      groupStepValues.Controls.Add( editMappingStepMask );
      groupStepValues.Controls.Add( label9 );
      groupStepValues.Controls.Add( label7 );
      groupStepValues.Controls.Add( label5 );
      groupStepValues.Controls.Add( label10 );
      groupStepValues.Controls.Add( editMappingStepValue );
      groupStepValues.Location = new System.Drawing.Point( 284, 24 );
      groupStepValues.Name = "groupStepValues";
      groupStepValues.Size = new System.Drawing.Size( 268, 131 );
      groupStepValues.TabIndex = 31;
      groupStepValues.TabStop = false;
      groupStepValues.Text = "Step";
      // 
      // comboMappingStepType
      // 
      comboMappingStepType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      comboMappingStepType.FormattingEnabled = true;
      comboMappingStepType.Location = new System.Drawing.Point( 141, 19 );
      comboMappingStepType.Name = "comboMappingStepType";
      comboMappingStepType.Size = new System.Drawing.Size( 121, 21 );
      comboMappingStepType.TabIndex = 0;
      // 
      // editMappingStepOffset
      // 
      editMappingStepOffset.Location = new System.Drawing.Point( 141, 45 );
      editMappingStepOffset.MaxLength = 2;
      editMappingStepOffset.Name = "editMappingStepOffset";
      editMappingStepOffset.Size = new System.Drawing.Size( 121, 20 );
      editMappingStepOffset.TabIndex = 1;
      editMappingStepOffset.TextChanged +=  editMappingStepOffset_TextChanged ;
      // 
      // editMappingStepMask
      // 
      editMappingStepMask.Location = new System.Drawing.Point( 141, 97 );
      editMappingStepMask.MaxLength = 2;
      editMappingStepMask.Name = "editMappingStepMask";
      editMappingStepMask.Size = new System.Drawing.Size( 121, 20 );
      editMappingStepMask.TabIndex = 3;
      editMappingStepMask.TextChanged +=  editMappingStepMask_TextChanged ;
      // 
      // label9
      // 
      label9.AutoSize = true;
      label9.Location = new System.Drawing.Point( 6, 22 );
      label9.Name = "label9";
      label9.Size = new System.Drawing.Size( 34, 13 );
      label9.TabIndex = 27;
      label9.Text = "Type:";
      // 
      // label7
      // 
      label7.AutoSize = true;
      label7.Location = new System.Drawing.Point( 6, 48 );
      label7.Name = "label7";
      label7.Size = new System.Drawing.Size( 79, 13 );
      label7.TabIndex = 27;
      label7.Text = "Address Offset:";
      // 
      // label5
      // 
      label5.AutoSize = true;
      label5.Location = new System.Drawing.Point( 6, 74 );
      label5.Name = "label5";
      label5.Size = new System.Drawing.Size( 65, 13 );
      label5.TabIndex = 27;
      label5.Text = "Value (Hex):";
      // 
      // label10
      // 
      label10.AutoSize = true;
      label10.Location = new System.Drawing.Point( 6, 100 );
      label10.Name = "label10";
      label10.Size = new System.Drawing.Size( 125, 13 );
      label10.TabIndex = 27;
      label10.Text = "Relevant Bit Mask (Hex):";
      // 
      // editMappingStepValue
      // 
      editMappingStepValue.Location = new System.Drawing.Point( 141, 71 );
      editMappingStepValue.Name = "editMappingStepValue";
      editMappingStepValue.Size = new System.Drawing.Size( 121, 20 );
      editMappingStepValue.TabIndex = 2;
      editMappingStepValue.TextChanged +=  editMappingStepValue_TextChanged ;
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
      tabExport.Controls.Add( panelExport );
      tabExport.Controls.Add( btnExport );
      tabExport.Controls.Add( comboExportMethod );
      tabExport.Controls.Add( label8 );
      tabExport.Location = new System.Drawing.Point( 4, 22 );
      tabExport.Name = "tabExport";
      tabExport.Padding = new System.Windows.Forms.Padding( 3 );
      tabExport.Size = new System.Drawing.Size( 766, 508 );
      tabExport.TabIndex = 1;
      tabExport.Text = "Export";
      tabExport.UseVisualStyleBackColor = true;
      // 
      // panelExport
      // 
      panelExport.Anchor =    System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Bottom   |  System.Windows.Forms.AnchorStyles.Left   |  System.Windows.Forms.AnchorStyles.Right ;
      panelExport.Location = new System.Drawing.Point( 7, 33 );
      panelExport.Name = "panelExport";
      panelExport.Size = new System.Drawing.Size( 751, 467 );
      panelExport.TabIndex = 35;
      // 
      // btnExport
      // 
      btnExport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnExport.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnExport.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnExport.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnExport.DisplayAntiAliased = true;
      btnExport.Image = null;
      btnExport.Location = new System.Drawing.Point( 369, 6 );
      btnExport.Name = "btnExport";
      btnExport.Size = new System.Drawing.Size( 75, 21 );
      btnExport.TabIndex = 33;
      btnExport.Text = "Export";
      btnExport.Click +=  btnExport_Click ;
      // 
      // comboExportMethod
      // 
      comboExportMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      comboExportMethod.FormattingEnabled = true;
      comboExportMethod.Location = new System.Drawing.Point( 89, 6 );
      comboExportMethod.Name = "comboExportMethod";
      comboExportMethod.Size = new System.Drawing.Size( 274, 21 );
      comboExportMethod.TabIndex = 32;
      comboExportMethod.SelectedIndexChanged +=  comboExportMethod_SelectedIndexChanged ;
      // 
      // label8
      // 
      label8.AutoSize = true;
      label8.Location = new System.Drawing.Point( 7, 9 );
      label8.Name = "label8";
      label8.Size = new System.Drawing.Size( 79, 13 );
      label8.TabIndex = 34;
      label8.Text = "Export Method:";
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
      groupDurationValues.ResumeLayout( false );
      groupDurationValues.PerformLayout();
      groupBox1.ResumeLayout( false );
      groupBox1.PerformLayout();
      groupStepValues.ResumeLayout( false );
      groupStepValues.PerformLayout();
      tabExport.ResumeLayout( false );
      tabExport.PerformLayout();
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
    private System.Windows.Forms.TextBox editMappingStepOffset;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox editMappingStepValue;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.GroupBox groupStepValues;
    private System.Windows.Forms.TextBox editMappingStepMask;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.GroupBox groupDurationValues;
    private System.Windows.Forms.TextBox editMappingDurationOffset;
    private System.Windows.Forms.TextBox editMappingDurationMask;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.TextBox editMappingDurationShiftRight;
    private System.Windows.Forms.Label label13;
    private System.Windows.Forms.Label label14;
    private System.Windows.Forms.TextBox editMappingDurationShiftLeft;
    private System.Windows.Forms.Label label15;
    private DecentForms.Button btnExport;
    private System.Windows.Forms.ComboBox comboExportMethod;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Panel panelExport;
    private Controls.ArrangedItemList listMappings;
    private System.Windows.Forms.ComboBox comboMappingStepType;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.TextBox editMappingLastStepAddressOffset;
    private System.Windows.Forms.Label label16;
    private System.Windows.Forms.Label label17;
    private System.Windows.Forms.TextBox editMappingLastStepValue;
  }
}
