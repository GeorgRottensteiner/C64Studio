namespace RetroDevStudio.Controls
{
  partial class PaletteEditorControl
  {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose( bool disposing )
    {
      if ( disposing && ( components != null ) )
      {
        components.Dispose();
      }
      base.Dispose( disposing );
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      components = new System.ComponentModel.Container();
      toolTip1 = new System.Windows.Forms.ToolTip( components );
      tabPalette = new System.Windows.Forms.TabControl();
      tabEditor = new System.Windows.Forms.TabPage();
      groupBox4 = new System.Windows.Forms.GroupBox();
      comboSystem = new System.Windows.Forms.ComboBox();
      editPaletteName = new System.Windows.Forms.TextBox();
      label6 = new System.Windows.Forms.Label();
      label4 = new System.Windows.Forms.Label();
      paletteList = new ArrangedItemList();
      groupBox1 = new System.Windows.Forms.GroupBox();
      listPalette = new System.Windows.Forms.ListBox();
      groupBox3 = new System.Windows.Forms.GroupBox();
      panelColorPreview = new System.Windows.Forms.Panel();
      groupBox2 = new System.Windows.Forms.GroupBox();
      scrollB = new System.Windows.Forms.HScrollBar();
      scrollG = new System.Windows.Forms.HScrollBar();
      scrollR = new System.Windows.Forms.HScrollBar();
      editBHex = new System.Windows.Forms.TextBox();
      editB = new System.Windows.Forms.TextBox();
      editGHex = new System.Windows.Forms.TextBox();
      editG = new System.Windows.Forms.TextBox();
      editRHex = new System.Windows.Forms.TextBox();
      editR = new System.Windows.Forms.TextBox();
      label3 = new System.Windows.Forms.Label();
      label2 = new System.Windows.Forms.Label();
      label1 = new System.Windows.Forms.Label();
      tabImportExport = new System.Windows.Forms.TabPage();
      groupBox6 = new System.Windows.Forms.GroupBox();
      checkImportColorsSorted = new System.Windows.Forms.CheckBox();
      checkImportSwizzle = new System.Windows.Forms.CheckBox();
      btnImportFromAssembly = new DecentForms.Button();
      btnImportFromFile = new DecentForms.Button();
      editDataImport = new System.Windows.Forms.TextBox();
      groupBox5 = new System.Windows.Forms.GroupBox();
      comboPaletteExportFormat = new System.Windows.Forms.ComboBox();
      checkExportSwizzled = new System.Windows.Forms.CheckBox();
      checkExportHex = new System.Windows.Forms.CheckBox();
      editPrefix = new System.Windows.Forms.TextBox();
      label5 = new System.Windows.Forms.Label();
      editWrapByteCount = new System.Windows.Forms.TextBox();
      checkExportToDataWrap = new System.Windows.Forms.CheckBox();
      checkExportToDataIncludeRes = new System.Windows.Forms.CheckBox();
      editDataExport = new System.Windows.Forms.TextBox();
      btnExportToFile = new DecentForms.Button();
      btnExportToData = new DecentForms.Button();
      tabPalette.SuspendLayout();
      tabEditor.SuspendLayout();
      groupBox4.SuspendLayout();
      groupBox1.SuspendLayout();
      groupBox3.SuspendLayout();
      groupBox2.SuspendLayout();
      tabImportExport.SuspendLayout();
      groupBox6.SuspendLayout();
      groupBox5.SuspendLayout();
      SuspendLayout();
      // 
      // tabPalette
      // 
      tabPalette.Anchor =    System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Bottom   |  System.Windows.Forms.AnchorStyles.Left   |  System.Windows.Forms.AnchorStyles.Right ;
      tabPalette.Controls.Add( tabEditor );
      tabPalette.Controls.Add( tabImportExport );
      tabPalette.Location = new System.Drawing.Point( 3, 3 );
      tabPalette.Name = "tabPalette";
      tabPalette.SelectedIndex = 0;
      tabPalette.Size = new System.Drawing.Size( 722, 426 );
      tabPalette.TabIndex = 8;
      // 
      // tabEditor
      // 
      tabEditor.Controls.Add( groupBox4 );
      tabEditor.Controls.Add( groupBox1 );
      tabEditor.Controls.Add( groupBox3 );
      tabEditor.Controls.Add( groupBox2 );
      tabEditor.Location = new System.Drawing.Point( 4, 24 );
      tabEditor.Name = "tabEditor";
      tabEditor.Padding = new System.Windows.Forms.Padding( 3 );
      tabEditor.Size = new System.Drawing.Size( 714, 398 );
      tabEditor.TabIndex = 0;
      tabEditor.Text = "Palette";
      tabEditor.UseVisualStyleBackColor = true;
      // 
      // groupBox4
      // 
      groupBox4.Anchor =   System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Bottom   |  System.Windows.Forms.AnchorStyles.Left ;
      groupBox4.Controls.Add( comboSystem );
      groupBox4.Controls.Add( editPaletteName );
      groupBox4.Controls.Add( label6 );
      groupBox4.Controls.Add( label4 );
      groupBox4.Controls.Add( paletteList );
      groupBox4.Location = new System.Drawing.Point( 6, 0 );
      groupBox4.Name = "groupBox4";
      groupBox4.Size = new System.Drawing.Size( 222, 373 );
      groupBox4.TabIndex = 6;
      groupBox4.TabStop = false;
      groupBox4.Text = "Palettes";
      // 
      // comboSystem
      // 
      comboSystem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      comboSystem.FormattingEnabled = true;
      comboSystem.Location = new System.Drawing.Point( 50, 332 );
      comboSystem.Name = "comboSystem";
      comboSystem.Size = new System.Drawing.Size( 166, 23 );
      comboSystem.TabIndex = 10;
      comboSystem.SelectedIndexChanged +=  comboSystem_SelectedIndexChanged ;
      // 
      // editPaletteName
      // 
      editPaletteName.Location = new System.Drawing.Point( 50, 302 );
      editPaletteName.Name = "editPaletteName";
      editPaletteName.Size = new System.Drawing.Size( 166, 23 );
      editPaletteName.TabIndex = 9;
      editPaletteName.TextChanged +=  editPaletteName_TextChanged ;
      // 
      // label6
      // 
      label6.AutoSize = true;
      label6.Location = new System.Drawing.Point( 6, 335 );
      label6.Name = "label6";
      label6.Size = new System.Drawing.Size( 48, 15 );
      label6.TabIndex = 8;
      label6.Text = "System:";
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Location = new System.Drawing.Point( 6, 306 );
      label4.Name = "label4";
      label4.Size = new System.Drawing.Size( 42, 15 );
      label4.TabIndex = 8;
      label4.Text = "Name:";
      // 
      // paletteList
      // 
      paletteList.AddButtonEnabled = true;
      paletteList.AllowClone = true;
      paletteList.AllowReordering = true;
      paletteList.DeleteButtonEnabled = false;
      paletteList.HasOwnerDrawColumn = true;
      paletteList.HighlightColor = System.Drawing.SystemColors.HotTrack;
      paletteList.HighlightTextColor = System.Drawing.SystemColors.HighlightText;
      paletteList.Location = new System.Drawing.Point( 6, 19 );
      paletteList.MoveDownButtonEnabled = false;
      paletteList.MoveUpButtonEnabled = false;
      paletteList.MustHaveOneElement = true;
      paletteList.Name = "paletteList";
      paletteList.SelectedIndex = -1;
      paletteList.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      paletteList.SelectionTextColor = System.Drawing.SystemColors.HighlightText;
      paletteList.Size = new System.Drawing.Size( 210, 277 );
      paletteList.TabIndex = 7;
      paletteList.AddingItem +=  paletteList_AddingItem ;
      paletteList.CloningItem +=  paletteList_CloningItem ;
      paletteList.RemovingItem +=  paletteList_RemovingItem ;
      paletteList.ItemRemoved +=  paletteList_ItemRemoved ;
      paletteList.ItemMoved +=  paletteList_ItemMoved ;
      paletteList.SelectedIndexChanged +=  paletteList_SelectedIndexChanged ;
      // 
      // groupBox1
      // 
      groupBox1.Anchor =    System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Bottom   |  System.Windows.Forms.AnchorStyles.Left   |  System.Windows.Forms.AnchorStyles.Right ;
      groupBox1.Controls.Add( listPalette );
      groupBox1.Location = new System.Drawing.Point( 234, 0 );
      groupBox1.Name = "groupBox1";
      groupBox1.Size = new System.Drawing.Size( 226, 373 );
      groupBox1.TabIndex = 3;
      groupBox1.TabStop = false;
      groupBox1.Text = "Current Palette";
      // 
      // listPalette
      // 
      listPalette.Anchor =    System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Bottom   |  System.Windows.Forms.AnchorStyles.Left   |  System.Windows.Forms.AnchorStyles.Right ;
      listPalette.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      listPalette.FormattingEnabled = true;
      listPalette.Location = new System.Drawing.Point( 6, 19 );
      listPalette.Name = "listPalette";
      listPalette.Size = new System.Drawing.Size( 212, 324 );
      listPalette.TabIndex = 2;
      listPalette.DrawItem +=  listPalette_DrawItem ;
      listPalette.SelectedIndexChanged +=  listPalette_SelectedIndexChanged ;
      // 
      // groupBox3
      // 
      groupBox3.Anchor =  System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Right ;
      groupBox3.Controls.Add( panelColorPreview );
      groupBox3.Location = new System.Drawing.Point( 466, 182 );
      groupBox3.Name = "groupBox3";
      groupBox3.Size = new System.Drawing.Size( 238, 66 );
      groupBox3.TabIndex = 5;
      groupBox3.TabStop = false;
      groupBox3.Text = "Preview";
      // 
      // panelColorPreview
      // 
      panelColorPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      panelColorPreview.Location = new System.Drawing.Point( 6, 19 );
      panelColorPreview.Name = "panelColorPreview";
      panelColorPreview.Size = new System.Drawing.Size( 226, 38 );
      panelColorPreview.TabIndex = 0;
      panelColorPreview.Paint +=  panelColorPreview_Paint ;
      // 
      // groupBox2
      // 
      groupBox2.Anchor =  System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Right ;
      groupBox2.Controls.Add( scrollB );
      groupBox2.Controls.Add( scrollG );
      groupBox2.Controls.Add( scrollR );
      groupBox2.Controls.Add( editBHex );
      groupBox2.Controls.Add( editB );
      groupBox2.Controls.Add( editGHex );
      groupBox2.Controls.Add( editG );
      groupBox2.Controls.Add( editRHex );
      groupBox2.Controls.Add( editR );
      groupBox2.Controls.Add( label3 );
      groupBox2.Controls.Add( label2 );
      groupBox2.Controls.Add( label1 );
      groupBox2.Location = new System.Drawing.Point( 466, 0 );
      groupBox2.Name = "groupBox2";
      groupBox2.Size = new System.Drawing.Size( 238, 175 );
      groupBox2.TabIndex = 4;
      groupBox2.TabStop = false;
      groupBox2.Text = "Current Color";
      // 
      // scrollB
      // 
      scrollB.Location = new System.Drawing.Point( 18, 146 );
      scrollB.Maximum = 255;
      scrollB.Name = "scrollB";
      scrollB.Size = new System.Drawing.Size( 214, 17 );
      scrollB.TabIndex = 3;
      scrollB.Scroll +=  scrollB_Scroll ;
      // 
      // scrollG
      // 
      scrollG.Location = new System.Drawing.Point( 18, 97 );
      scrollG.Maximum = 255;
      scrollG.Name = "scrollG";
      scrollG.Size = new System.Drawing.Size( 214, 17 );
      scrollG.TabIndex = 3;
      scrollG.Scroll +=  scrollG_Scroll ;
      // 
      // scrollR
      // 
      scrollR.Location = new System.Drawing.Point( 18, 48 );
      scrollR.Maximum = 255;
      scrollR.Name = "scrollR";
      scrollR.Size = new System.Drawing.Size( 214, 17 );
      scrollR.TabIndex = 3;
      scrollR.Scroll +=  scrollR_Scroll ;
      // 
      // editBHex
      // 
      editBHex.Location = new System.Drawing.Point( 129, 123 );
      editBHex.MaxLength = 2;
      editBHex.Name = "editBHex";
      editBHex.Size = new System.Drawing.Size( 49, 23 );
      editBHex.TabIndex = 2;
      editBHex.TextChanged +=  editBHex_TextChanged ;
      // 
      // editB
      // 
      editB.Location = new System.Drawing.Point( 60, 123 );
      editB.Name = "editB";
      editB.Size = new System.Drawing.Size( 49, 23 );
      editB.TabIndex = 2;
      editB.TextChanged +=  editB_TextChanged ;
      // 
      // editGHex
      // 
      editGHex.Location = new System.Drawing.Point( 129, 74 );
      editGHex.MaxLength = 2;
      editGHex.Name = "editGHex";
      editGHex.Size = new System.Drawing.Size( 49, 23 );
      editGHex.TabIndex = 2;
      editGHex.TextChanged +=  editGHex_TextChanged ;
      // 
      // editG
      // 
      editG.Location = new System.Drawing.Point( 60, 74 );
      editG.Name = "editG";
      editG.Size = new System.Drawing.Size( 49, 23 );
      editG.TabIndex = 2;
      editG.TextChanged +=  editG_TextChanged ;
      // 
      // editRHex
      // 
      editRHex.Location = new System.Drawing.Point( 129, 22 );
      editRHex.MaxLength = 2;
      editRHex.Name = "editRHex";
      editRHex.Size = new System.Drawing.Size( 49, 23 );
      editRHex.TabIndex = 2;
      editRHex.TextChanged +=  editRHex_TextChanged ;
      // 
      // editR
      // 
      editR.Location = new System.Drawing.Point( 60, 22 );
      editR.Name = "editR";
      editR.Size = new System.Drawing.Size( 49, 23 );
      editR.TabIndex = 2;
      editR.TextChanged +=  editR_TextChanged ;
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new System.Drawing.Point( 16, 126 );
      label3.Name = "label3";
      label3.Size = new System.Drawing.Size( 33, 15 );
      label3.TabIndex = 0;
      label3.Text = "Blue:";
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new System.Drawing.Point( 16, 77 );
      label2.Name = "label2";
      label2.Size = new System.Drawing.Size( 41, 15 );
      label2.TabIndex = 0;
      label2.Text = "Green:";
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new System.Drawing.Point( 16, 25 );
      label1.Name = "label1";
      label1.Size = new System.Drawing.Size( 30, 15 );
      label1.TabIndex = 0;
      label1.Text = "Red:";
      // 
      // tabImportExport
      // 
      tabImportExport.Controls.Add( groupBox6 );
      tabImportExport.Controls.Add( groupBox5 );
      tabImportExport.Location = new System.Drawing.Point( 4, 24 );
      tabImportExport.Name = "tabImportExport";
      tabImportExport.Padding = new System.Windows.Forms.Padding( 3 );
      tabImportExport.Size = new System.Drawing.Size( 714, 398 );
      tabImportExport.TabIndex = 1;
      tabImportExport.Text = "Import/Export";
      tabImportExport.UseVisualStyleBackColor = true;
      // 
      // groupBox6
      // 
      groupBox6.Anchor =    System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Bottom   |  System.Windows.Forms.AnchorStyles.Left   |  System.Windows.Forms.AnchorStyles.Right ;
      groupBox6.Controls.Add( checkImportColorsSorted );
      groupBox6.Controls.Add( checkImportSwizzle );
      groupBox6.Controls.Add( btnImportFromAssembly );
      groupBox6.Controls.Add( btnImportFromFile );
      groupBox6.Controls.Add( editDataImport );
      groupBox6.Location = new System.Drawing.Point( 318, 6 );
      groupBox6.Name = "groupBox6";
      groupBox6.Size = new System.Drawing.Size( 390, 386 );
      groupBox6.TabIndex = 1;
      groupBox6.TabStop = false;
      groupBox6.Text = "Import";
      // 
      // checkImportColorsSorted
      // 
      checkImportColorsSorted.AutoSize = true;
      checkImportColorsSorted.Checked = true;
      checkImportColorsSorted.CheckState = System.Windows.Forms.CheckState.Checked;
      checkImportColorsSorted.Location = new System.Drawing.Point( 213, 19 );
      checkImportColorsSorted.Name = "checkImportColorsSorted";
      checkImportColorsSorted.Size = new System.Drawing.Size( 60, 19 );
      checkImportColorsSorted.TabIndex = 27;
      checkImportColorsSorted.Text = "Sorted";
      checkImportColorsSorted.UseVisualStyleBackColor = true;
      // 
      // checkImportSwizzle
      // 
      checkImportSwizzle.AutoSize = true;
      checkImportSwizzle.Checked = true;
      checkImportSwizzle.CheckState = System.Windows.Forms.CheckState.Checked;
      checkImportSwizzle.Location = new System.Drawing.Point( 129, 19 );
      checkImportSwizzle.Name = "checkImportSwizzle";
      checkImportSwizzle.Size = new System.Drawing.Size( 82, 19 );
      checkImportSwizzle.TabIndex = 27;
      checkImportSwizzle.Text = "De-Swizzle";
      checkImportSwizzle.UseVisualStyleBackColor = true;
      // 
      // btnImportFromAssembly
      // 
      btnImportFromAssembly.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnImportFromAssembly.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnImportFromAssembly.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnImportFromAssembly.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnImportFromAssembly.DisplayAntiAliased = true;
      btnImportFromAssembly.Image = null;
      btnImportFromAssembly.Location = new System.Drawing.Point( 6, 44 );
      btnImportFromAssembly.Name = "btnImportFromAssembly";
      btnImportFromAssembly.Size = new System.Drawing.Size( 117, 23 );
      btnImportFromAssembly.TabIndex = 1;
      btnImportFromAssembly.Text = "From assembly";
      btnImportFromAssembly.Click +=  btnImportFromAssembly_Click ;
      // 
      // btnImportFromFile
      // 
      btnImportFromFile.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnImportFromFile.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnImportFromFile.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnImportFromFile.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnImportFromFile.DisplayAntiAliased = true;
      btnImportFromFile.Image = null;
      btnImportFromFile.Location = new System.Drawing.Point( 6, 15 );
      btnImportFromFile.Name = "btnImportFromFile";
      btnImportFromFile.Size = new System.Drawing.Size( 117, 23 );
      btnImportFromFile.TabIndex = 1;
      btnImportFromFile.Text = "From File...";
      btnImportFromFile.Click +=  btnImportFromFile_Click ;
      // 
      // editDataImport
      // 
      editDataImport.Anchor =    System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Bottom   |  System.Windows.Forms.AnchorStyles.Left   |  System.Windows.Forms.AnchorStyles.Right ;
      editDataImport.Location = new System.Drawing.Point( 6, 94 );
      editDataImport.Multiline = true;
      editDataImport.Name = "editDataImport";
      editDataImport.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      editDataImport.Size = new System.Drawing.Size( 378, 286 );
      editDataImport.TabIndex = 29;
      editDataImport.WordWrap = false;
      // 
      // groupBox5
      // 
      groupBox5.Anchor =   System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Bottom   |  System.Windows.Forms.AnchorStyles.Left ;
      groupBox5.Controls.Add( comboPaletteExportFormat );
      groupBox5.Controls.Add( checkExportSwizzled );
      groupBox5.Controls.Add( checkExportHex );
      groupBox5.Controls.Add( editPrefix );
      groupBox5.Controls.Add( label5 );
      groupBox5.Controls.Add( editWrapByteCount );
      groupBox5.Controls.Add( checkExportToDataWrap );
      groupBox5.Controls.Add( checkExportToDataIncludeRes );
      groupBox5.Controls.Add( editDataExport );
      groupBox5.Controls.Add( btnExportToFile );
      groupBox5.Controls.Add( btnExportToData );
      groupBox5.Location = new System.Drawing.Point( 6, 6 );
      groupBox5.Name = "groupBox5";
      groupBox5.Size = new System.Drawing.Size( 306, 386 );
      groupBox5.TabIndex = 0;
      groupBox5.TabStop = false;
      groupBox5.Text = "Export";
      // 
      // comboPaletteExportFormat
      // 
      comboPaletteExportFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      comboPaletteExportFormat.FormattingEnabled = true;
      comboPaletteExportFormat.Location = new System.Drawing.Point( 118, 92 );
      comboPaletteExportFormat.Name = "comboPaletteExportFormat";
      comboPaletteExportFormat.Size = new System.Drawing.Size( 139, 23 );
      comboPaletteExportFormat.TabIndex = 30;
      // 
      // checkExportSwizzled
      // 
      checkExportSwizzled.AutoSize = true;
      checkExportSwizzled.Checked = true;
      checkExportSwizzled.CheckState = System.Windows.Forms.CheckState.Checked;
      checkExportSwizzled.Location = new System.Drawing.Point( 214, 42 );
      checkExportSwizzled.Name = "checkExportSwizzled";
      checkExportSwizzled.Size = new System.Drawing.Size( 63, 19 );
      checkExportSwizzled.TabIndex = 27;
      checkExportSwizzled.Text = "Swizzle";
      checkExportSwizzled.UseVisualStyleBackColor = true;
      // 
      // checkExportHex
      // 
      checkExportHex.AutoSize = true;
      checkExportHex.Checked = true;
      checkExportHex.CheckState = System.Windows.Forms.CheckState.Checked;
      checkExportHex.Location = new System.Drawing.Point( 118, 42 );
      checkExportHex.Name = "checkExportHex";
      checkExportHex.Size = new System.Drawing.Size( 96, 19 );
      checkExportHex.TabIndex = 27;
      checkExportHex.Text = "Export as Hex";
      checkExportHex.UseVisualStyleBackColor = true;
      // 
      // editPrefix
      // 
      editPrefix.Location = new System.Drawing.Point( 214, 17 );
      editPrefix.Name = "editPrefix";
      editPrefix.Size = new System.Drawing.Size( 43, 23 );
      editPrefix.TabIndex = 23;
      editPrefix.Text = "!byte ";
      // 
      // label5
      // 
      label5.AutoSize = true;
      label5.Location = new System.Drawing.Point( 235, 70 );
      label5.Name = "label5";
      label5.Size = new System.Drawing.Size( 35, 15 );
      label5.TabIndex = 26;
      label5.Text = "bytes";
      // 
      // editWrapByteCount
      // 
      editWrapByteCount.Enabled = false;
      editWrapByteCount.Location = new System.Drawing.Point( 188, 66 );
      editWrapByteCount.Name = "editWrapByteCount";
      editWrapByteCount.Size = new System.Drawing.Size( 41, 23 );
      editWrapByteCount.TabIndex = 25;
      editWrapByteCount.Text = "40";
      // 
      // checkExportToDataWrap
      // 
      checkExportToDataWrap.AutoSize = true;
      checkExportToDataWrap.Checked = true;
      checkExportToDataWrap.CheckState = System.Windows.Forms.CheckState.Checked;
      checkExportToDataWrap.Location = new System.Drawing.Point( 118, 69 );
      checkExportToDataWrap.Name = "checkExportToDataWrap";
      checkExportToDataWrap.Size = new System.Drawing.Size( 67, 19 );
      checkExportToDataWrap.TabIndex = 24;
      checkExportToDataWrap.Text = "Wrap at";
      checkExportToDataWrap.UseVisualStyleBackColor = true;
      // 
      // checkExportToDataIncludeRes
      // 
      checkExportToDataIncludeRes.AutoSize = true;
      checkExportToDataIncludeRes.Checked = true;
      checkExportToDataIncludeRes.CheckState = System.Windows.Forms.CheckState.Checked;
      checkExportToDataIncludeRes.Location = new System.Drawing.Point( 118, 19 );
      checkExportToDataIncludeRes.Name = "checkExportToDataIncludeRes";
      checkExportToDataIncludeRes.Size = new System.Drawing.Size( 81, 19 );
      checkExportToDataIncludeRes.TabIndex = 22;
      checkExportToDataIncludeRes.Text = "Prefix with";
      checkExportToDataIncludeRes.UseVisualStyleBackColor = true;
      // 
      // editDataExport
      // 
      editDataExport.Anchor =    System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Bottom   |  System.Windows.Forms.AnchorStyles.Left   |  System.Windows.Forms.AnchorStyles.Right ;
      editDataExport.Location = new System.Drawing.Point( 0, 119 );
      editDataExport.Multiline = true;
      editDataExport.Name = "editDataExport";
      editDataExport.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      editDataExport.Size = new System.Drawing.Size( 300, 261 );
      editDataExport.TabIndex = 29;
      editDataExport.WordWrap = false;
      // 
      // btnExportToFile
      // 
      btnExportToFile.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnExportToFile.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnExportToFile.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnExportToFile.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnExportToFile.DisplayAntiAliased = true;
      btnExportToFile.Image = null;
      btnExportToFile.Location = new System.Drawing.Point( 6, 44 );
      btnExportToFile.Name = "btnExportToFile";
      btnExportToFile.Size = new System.Drawing.Size( 106, 23 );
      btnExportToFile.TabIndex = 28;
      btnExportToFile.Text = "as binary file";
      btnExportToFile.Click +=  btnExportToFile_Click ;
      // 
      // btnExportToData
      // 
      btnExportToData.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnExportToData.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnExportToData.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnExportToData.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnExportToData.DisplayAntiAliased = true;
      btnExportToData.Image = null;
      btnExportToData.Location = new System.Drawing.Point( 6, 15 );
      btnExportToData.Name = "btnExportToData";
      btnExportToData.Size = new System.Drawing.Size( 106, 23 );
      btnExportToData.TabIndex = 21;
      btnExportToData.Text = "as assembly source";
      btnExportToData.Click +=  btnExportToData_Click ;
      // 
      // PaletteEditorControl
      // 
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      Controls.Add( tabPalette );
      Name = "PaletteEditorControl";
      Size = new System.Drawing.Size( 725, 429 );
      tabPalette.ResumeLayout( false );
      tabEditor.ResumeLayout( false );
      groupBox4.ResumeLayout( false );
      groupBox4.PerformLayout();
      groupBox1.ResumeLayout( false );
      groupBox3.ResumeLayout( false );
      groupBox2.ResumeLayout( false );
      groupBox2.PerformLayout();
      tabImportExport.ResumeLayout( false );
      groupBox6.ResumeLayout( false );
      groupBox6.PerformLayout();
      groupBox5.ResumeLayout( false );
      groupBox5.PerformLayout();
      ResumeLayout( false );

    }

    #endregion
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.TabControl tabPalette;
    private System.Windows.Forms.TabPage tabEditor;
    private System.Windows.Forms.GroupBox groupBox4;
    private System.Windows.Forms.TextBox editPaletteName;
    private System.Windows.Forms.Label label4;
    private ArrangedItemList paletteList;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.ListBox listPalette;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.Panel panelColorPreview;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.HScrollBar scrollB;
    private System.Windows.Forms.HScrollBar scrollG;
    private System.Windows.Forms.HScrollBar scrollR;
    private System.Windows.Forms.TextBox editBHex;
    private System.Windows.Forms.TextBox editB;
    private System.Windows.Forms.TextBox editGHex;
    private System.Windows.Forms.TextBox editG;
    private System.Windows.Forms.TextBox editRHex;
    private System.Windows.Forms.TextBox editR;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TabPage tabImportExport;
    private System.Windows.Forms.GroupBox groupBox6;
    private System.Windows.Forms.CheckBox checkImportColorsSorted;
    private System.Windows.Forms.CheckBox checkImportSwizzle;
    private DecentForms.Button btnImportFromAssembly;
    private DecentForms.Button btnImportFromFile;
    private System.Windows.Forms.TextBox editDataImport;
    private System.Windows.Forms.GroupBox groupBox5;
    private System.Windows.Forms.ComboBox comboPaletteExportFormat;
    private System.Windows.Forms.CheckBox checkExportSwizzled;
    private System.Windows.Forms.CheckBox checkExportHex;
    private System.Windows.Forms.TextBox editPrefix;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox editWrapByteCount;
    private System.Windows.Forms.CheckBox checkExportToDataWrap;
    private System.Windows.Forms.CheckBox checkExportToDataIncludeRes;
    private System.Windows.Forms.TextBox editDataExport;
    private DecentForms.Button btnExportToFile;
    private DecentForms.Button btnExportToData;
    private System.Windows.Forms.ComboBox comboSystem;
    private System.Windows.Forms.Label label6;
  }
}
