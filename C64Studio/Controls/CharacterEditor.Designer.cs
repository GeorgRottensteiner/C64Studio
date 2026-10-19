namespace RetroDevStudio.Controls
{
  partial class CharacterEditor
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CharacterEditor));
      comboCategories = new System.Windows.Forms.ComboBox();
      labelCategory = new System.Windows.Forms.Label();
      labelCharNo = new System.Windows.Forms.Label();
      checkShowGrid = new System.Windows.Forms.CheckBox();
      checkPasteMultiColor = new System.Windows.Forms.CheckBox();
      editMoveTargetIndex = new System.Windows.Forms.TextBox();
      toolTip1 = new System.Windows.Forms.ToolTip( components );
      btnHighlightDuplicates = new DecentForms.Button();
      btnPaste = new DecentForms.Button();
      btnCopy = new DecentForms.Button();
      btnInvert = new DecentForms.Button();
      btnMirrorY = new DecentForms.Button();
      btnMirrorX = new DecentForms.Button();
      btnShiftDown = new DecentForms.Button();
      btnShiftUp = new DecentForms.Button();
      btnShiftRight = new DecentForms.Button();
      button3 = new DecentForms.Button();
      btnPasteFromClipboard = new DecentForms.Button();
      btnRotateLeft = new DecentForms.Button();
      btnShiftLeft = new DecentForms.Button();
      tabCharacterEditor = new System.Windows.Forms.TabControl();
      tabEditor = new System.Windows.Forms.TabPage();
      panelColorPalette = new System.Windows.Forms.Panel();
      btnMoveSelectionToTarget = new DecentForms.Button();
      panelColorChooser = new System.Windows.Forms.Panel();
      editCharName = new System.Windows.Forms.TextBox();
      editCharsetName = new System.Windows.Forms.TextBox();
      btnRestoreDefault = new DecentForms.Button();
      btnClearChars = new DecentForms.Button();
      btnRemoveDuplicates = new DecentForms.Button();
      panelColorSettings = new System.Windows.Forms.Panel();
      canvasEditor = new CustomDrawControl();
      comboCharsetMode = new System.Windows.Forms.ComboBox();
      label2 = new System.Windows.Forms.Label();
      label1 = new System.Windows.Forms.Label();
      labelCharsetMode = new System.Windows.Forms.Label();
      picturePlayground = new GR.Forms.FastPictureBox();
      panelCharacters = new GR.Forms.ImageListbox();
      tabCategories = new System.Windows.Forms.TabPage();
      btnMoveCategoryDown = new DecentForms.Button();
      btnMoveCategoryUp = new DecentForms.Button();
      groupAllCategories = new System.Windows.Forms.GroupBox();
      btnSortCategories = new DecentForms.Button();
      groupCategorySpecific = new System.Windows.Forms.GroupBox();
      label5 = new System.Windows.Forms.Label();
      editCollapseIndex = new System.Windows.Forms.TextBox();
      btnCollapseCategory = new DecentForms.Button();
      btnReseatCategory = new DecentForms.Button();
      btnDelete = new DecentForms.Button();
      btnAddCategory = new DecentForms.Button();
      listCategories = new System.Windows.Forms.ListView();
      columnHeader1 = new System.Windows.Forms.ColumnHeader();
      columnHeader2 = new System.Windows.Forms.ColumnHeader();
      editCategoryName = new System.Windows.Forms.TextBox();
      label3 = new System.Windows.Forms.Label();
      tabCharacterEditor.SuspendLayout();
      tabEditor.SuspendLayout();
      ( (System.ComponentModel.ISupportInitialize)canvasEditor ).BeginInit();
      ( (System.ComponentModel.ISupportInitialize)picturePlayground ).BeginInit();
      tabCategories.SuspendLayout();
      groupAllCategories.SuspendLayout();
      groupCategorySpecific.SuspendLayout();
      SuspendLayout();
      // 
      // comboCategories
      // 
      comboCategories.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      comboCategories.FormattingEnabled = true;
      comboCategories.Location = new System.Drawing.Point( 368, 245 );
      comboCategories.Name = "comboCategories";
      comboCategories.Size = new System.Drawing.Size( 140, 23 );
      comboCategories.TabIndex = 11;
      comboCategories.SelectedIndexChanged +=  comboCategories_SelectedIndexChanged ;
      // 
      // labelCategory
      // 
      labelCategory.Location = new System.Drawing.Point( 303, 248 );
      labelCategory.Name = "labelCategory";
      labelCategory.Size = new System.Drawing.Size( 59, 23 );
      labelCategory.TabIndex = 35;
      labelCategory.Text = "Category:";
      // 
      // labelCharNo
      // 
      labelCharNo.Location = new System.Drawing.Point( 303, 192 );
      labelCharNo.Name = "labelCharNo";
      labelCharNo.Size = new System.Drawing.Size( 205, 23 );
      labelCharNo.TabIndex = 34;
      labelCharNo.Text = "label1";
      labelCharNo.Paint +=  labelCharNo_Paint ;
      // 
      // checkShowGrid
      // 
      checkShowGrid.AutoSize = true;
      checkShowGrid.Location = new System.Drawing.Point( 280, 298 );
      checkShowGrid.Name = "checkShowGrid";
      checkShowGrid.Size = new System.Drawing.Size( 80, 19 );
      checkShowGrid.TabIndex = 13;
      checkShowGrid.Text = "Show Grid";
      checkShowGrid.UseVisualStyleBackColor = true;
      checkShowGrid.CheckedChanged +=  checkShowGrid_CheckedChanged ;
      // 
      // checkPasteMultiColor
      // 
      checkPasteMultiColor.AutoSize = true;
      checkPasteMultiColor.Location = new System.Drawing.Point( 280, 275 );
      checkPasteMultiColor.Name = "checkPasteMultiColor";
      checkPasteMultiColor.Size = new System.Drawing.Size( 161, 19 );
      checkPasteMultiColor.TabIndex = 12;
      checkPasteMultiColor.Text = "Force Multicolor on paste";
      checkPasteMultiColor.UseVisualStyleBackColor = true;
      // 
      // editMoveTargetIndex
      // 
      editMoveTargetIndex.Location = new System.Drawing.Point( 617, 329 );
      editMoveTargetIndex.Name = "editMoveTargetIndex";
      editMoveTargetIndex.Size = new System.Drawing.Size( 122, 23 );
      editMoveTargetIndex.TabIndex = 23;
      toolTip1.SetToolTip( editMoveTargetIndex, "Index to move selected characters to" );
      editMoveTargetIndex.TextChanged +=  editMoveTargetIndex_TextChanged ;
      // 
      // btnHighlightDuplicates
      // 
      btnHighlightDuplicates.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnHighlightDuplicates.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnHighlightDuplicates.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnHighlightDuplicates.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnHighlightDuplicates.DisplayAntiAliased = true;
      btnHighlightDuplicates.Image = null;
      btnHighlightDuplicates.Location = new System.Drawing.Point( 514, 356 );
      btnHighlightDuplicates.Name = "btnHighlightDuplicates";
      btnHighlightDuplicates.Size = new System.Drawing.Size( 97, 23 );
      btnHighlightDuplicates.TabIndex = 24;
      btnHighlightDuplicates.Text = "Duplicates";
      toolTip1.SetToolTip( btnHighlightDuplicates, "Highlight duplicates" );
      btnHighlightDuplicates.Click +=  btnHighlightDuplicates_Click ;
      // 
      // btnPaste
      // 
      btnPaste.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnPaste.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnPaste.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnPaste.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnPaste.DisplayAntiAliased = true;
      btnPaste.Image = (System.Drawing.Image)resources.GetObject( "btnPaste.Image" );
      btnPaste.Location = new System.Drawing.Point( 546, 269 );
      btnPaste.Name = "btnPaste";
      btnPaste.Size = new System.Drawing.Size( 26, 23 );
      btnPaste.TabIndex = 18;
      toolTip1.SetToolTip( btnPaste, "Paste Characters" );
      btnPaste.Click +=  btnPaste_Click ;
      // 
      // btnCopy
      // 
      btnCopy.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnCopy.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnCopy.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnCopy.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnCopy.DisplayAntiAliased = true;
      btnCopy.Image = (System.Drawing.Image)resources.GetObject( "btnCopy.Image" );
      btnCopy.Location = new System.Drawing.Point( 514, 269 );
      btnCopy.Name = "btnCopy";
      btnCopy.Size = new System.Drawing.Size( 26, 23 );
      btnCopy.TabIndex = 17;
      toolTip1.SetToolTip( btnCopy, "Copy Characters to Clipboard" );
      btnCopy.Click +=  btnCopy_Click ;
      // 
      // btnInvert
      // 
      btnInvert.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnInvert.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnInvert.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnInvert.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnInvert.DisplayAntiAliased = true;
      btnInvert.Image = (System.Drawing.Image)resources.GetObject( "btnInvert.Image" );
      btnInvert.Location = new System.Drawing.Point( 198, 269 );
      btnInvert.Name = "btnInvert";
      btnInvert.Size = new System.Drawing.Size( 26, 26 );
      btnInvert.TabIndex = 6;
      toolTip1.SetToolTip( btnInvert, "Invert" );
      btnInvert.Click +=  btnInvert_Click ;
      // 
      // btnMirrorY
      // 
      btnMirrorY.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnMirrorY.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnMirrorY.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnMirrorY.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnMirrorY.DisplayAntiAliased = true;
      btnMirrorY.Image = (System.Drawing.Image)resources.GetObject( "btnMirrorY.Image" );
      btnMirrorY.Location = new System.Drawing.Point( 166, 269 );
      btnMirrorY.Name = "btnMirrorY";
      btnMirrorY.Size = new System.Drawing.Size( 26, 26 );
      btnMirrorY.TabIndex = 5;
      toolTip1.SetToolTip( btnMirrorY, "Mirror Y" );
      btnMirrorY.Click +=  btnMirrorY_Click ;
      // 
      // btnMirrorX
      // 
      btnMirrorX.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnMirrorX.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnMirrorX.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnMirrorX.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnMirrorX.DisplayAntiAliased = true;
      btnMirrorX.Image = (System.Drawing.Image)resources.GetObject( "btnMirrorX.Image" );
      btnMirrorX.Location = new System.Drawing.Point( 134, 269 );
      btnMirrorX.Name = "btnMirrorX";
      btnMirrorX.Size = new System.Drawing.Size( 26, 26 );
      btnMirrorX.TabIndex = 4;
      toolTip1.SetToolTip( btnMirrorX, "Mirror X" );
      btnMirrorX.Click +=  btnMirrorX_Click ;
      // 
      // btnShiftDown
      // 
      btnShiftDown.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnShiftDown.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnShiftDown.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnShiftDown.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnShiftDown.DisplayAntiAliased = true;
      btnShiftDown.Image = (System.Drawing.Image)resources.GetObject( "btnShiftDown.Image" );
      btnShiftDown.Location = new System.Drawing.Point( 102, 269 );
      btnShiftDown.Name = "btnShiftDown";
      btnShiftDown.Size = new System.Drawing.Size( 26, 26 );
      btnShiftDown.TabIndex = 3;
      toolTip1.SetToolTip( btnShiftDown, "Shift Down" );
      btnShiftDown.Click +=  btnShiftDown_Click ;
      // 
      // btnShiftUp
      // 
      btnShiftUp.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnShiftUp.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnShiftUp.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnShiftUp.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnShiftUp.DisplayAntiAliased = true;
      btnShiftUp.Image = (System.Drawing.Image)resources.GetObject( "btnShiftUp.Image" );
      btnShiftUp.Location = new System.Drawing.Point( 70, 269 );
      btnShiftUp.Name = "btnShiftUp";
      btnShiftUp.Size = new System.Drawing.Size( 26, 26 );
      btnShiftUp.TabIndex = 2;
      toolTip1.SetToolTip( btnShiftUp, "Shift Up" );
      btnShiftUp.Click +=  btnShiftUp_Click ;
      // 
      // btnShiftRight
      // 
      btnShiftRight.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnShiftRight.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnShiftRight.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnShiftRight.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnShiftRight.DisplayAntiAliased = true;
      btnShiftRight.Image = (System.Drawing.Image)resources.GetObject( "btnShiftRight.Image" );
      btnShiftRight.Location = new System.Drawing.Point( 38, 269 );
      btnShiftRight.Name = "btnShiftRight";
      btnShiftRight.Size = new System.Drawing.Size( 26, 26 );
      btnShiftRight.TabIndex = 1;
      toolTip1.SetToolTip( btnShiftRight, "Shift Right" );
      btnShiftRight.Click +=  btnShiftRight_Click ;
      // 
      // button3
      // 
      button3.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      button3.BorderStyle = DecentForms.BorderStyle.FLAT;
      button3.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      button3.DialogResult = System.Windows.Forms.DialogResult.OK;
      button3.DisplayAntiAliased = true;
      button3.Image = (System.Drawing.Image)resources.GetObject( "button3.Image" );
      button3.Location = new System.Drawing.Point( 38, 301 );
      button3.Name = "button3";
      button3.Size = new System.Drawing.Size( 26, 26 );
      button3.TabIndex = 8;
      toolTip1.SetToolTip( button3, "Rotate Right" );
      button3.Click +=  btnRotateRight_Click ;
      // 
      // btnPasteFromClipboard
      // 
      btnPasteFromClipboard.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnPasteFromClipboard.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnPasteFromClipboard.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnPasteFromClipboard.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnPasteFromClipboard.DisplayAntiAliased = true;
      btnPasteFromClipboard.Image = null;
      btnPasteFromClipboard.Location = new System.Drawing.Point( 617, 269 );
      btnPasteFromClipboard.Name = "btnPasteFromClipboard";
      btnPasteFromClipboard.Size = new System.Drawing.Size( 122, 23 );
      btnPasteFromClipboard.TabIndex = 19;
      btnPasteFromClipboard.Text = "Paste Image";
      toolTip1.SetToolTip( btnPasteFromClipboard, "Paste Image" );
      btnPasteFromClipboard.Click +=  btnPasteFromClipboard_Click ;
      // 
      // btnRotateLeft
      // 
      btnRotateLeft.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnRotateLeft.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnRotateLeft.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnRotateLeft.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnRotateLeft.DisplayAntiAliased = true;
      btnRotateLeft.Image = (System.Drawing.Image)resources.GetObject( "btnRotateLeft.Image" );
      btnRotateLeft.Location = new System.Drawing.Point( 6, 301 );
      btnRotateLeft.Name = "btnRotateLeft";
      btnRotateLeft.Size = new System.Drawing.Size( 26, 26 );
      btnRotateLeft.TabIndex = 7;
      toolTip1.SetToolTip( btnRotateLeft, "Rotate Left" );
      btnRotateLeft.Click +=  btnRotateLeft_Click ;
      // 
      // btnShiftLeft
      // 
      btnShiftLeft.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnShiftLeft.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnShiftLeft.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnShiftLeft.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnShiftLeft.DisplayAntiAliased = true;
      btnShiftLeft.Image = (System.Drawing.Image)resources.GetObject( "btnShiftLeft.Image" );
      btnShiftLeft.Location = new System.Drawing.Point( 6, 269 );
      btnShiftLeft.Name = "btnShiftLeft";
      btnShiftLeft.Size = new System.Drawing.Size( 26, 26 );
      btnShiftLeft.TabIndex = 0;
      toolTip1.SetToolTip( btnShiftLeft, "Shift Left" );
      btnShiftLeft.Click +=  btnShiftLeft_Click ;
      // 
      // tabCharacterEditor
      // 
      tabCharacterEditor.Controls.Add( tabEditor );
      tabCharacterEditor.Controls.Add( tabCategories );
      tabCharacterEditor.Dock = System.Windows.Forms.DockStyle.Fill;
      tabCharacterEditor.Location = new System.Drawing.Point( 0, 0 );
      tabCharacterEditor.Name = "tabCharacterEditor";
      tabCharacterEditor.SelectedIndex = 0;
      tabCharacterEditor.Size = new System.Drawing.Size( 1143, 490 );
      tabCharacterEditor.TabIndex = 0;
      // 
      // tabEditor
      // 
      tabEditor.Controls.Add( panelColorPalette );
      tabEditor.Controls.Add( btnMoveSelectionToTarget );
      tabEditor.Controls.Add( panelColorChooser );
      tabEditor.Controls.Add( editCharName );
      tabEditor.Controls.Add( editCharsetName );
      tabEditor.Controls.Add( editMoveTargetIndex );
      tabEditor.Controls.Add( btnRestoreDefault );
      tabEditor.Controls.Add( btnClearChars );
      tabEditor.Controls.Add( btnRemoveDuplicates );
      tabEditor.Controls.Add( btnHighlightDuplicates );
      tabEditor.Controls.Add( panelColorSettings );
      tabEditor.Controls.Add( canvasEditor );
      tabEditor.Controls.Add( comboCharsetMode );
      tabEditor.Controls.Add( label2 );
      tabEditor.Controls.Add( label1 );
      tabEditor.Controls.Add( labelCharsetMode );
      tabEditor.Controls.Add( picturePlayground );
      tabEditor.Controls.Add( btnPaste );
      tabEditor.Controls.Add( btnCopy );
      tabEditor.Controls.Add( btnInvert );
      tabEditor.Controls.Add( btnMirrorY );
      tabEditor.Controls.Add( checkPasteMultiColor );
      tabEditor.Controls.Add( btnMirrorX );
      tabEditor.Controls.Add( checkShowGrid );
      tabEditor.Controls.Add( btnShiftDown );
      tabEditor.Controls.Add( panelCharacters );
      tabEditor.Controls.Add( btnShiftUp );
      tabEditor.Controls.Add( labelCharNo );
      tabEditor.Controls.Add( btnShiftRight );
      tabEditor.Controls.Add( labelCategory );
      tabEditor.Controls.Add( button3 );
      tabEditor.Controls.Add( btnPasteFromClipboard );
      tabEditor.Controls.Add( btnRotateLeft );
      tabEditor.Controls.Add( comboCategories );
      tabEditor.Controls.Add( btnShiftLeft );
      tabEditor.Location = new System.Drawing.Point( 4, 24 );
      tabEditor.Name = "tabEditor";
      tabEditor.Padding = new System.Windows.Forms.Padding( 3 );
      tabEditor.Size = new System.Drawing.Size( 1135, 462 );
      tabEditor.TabIndex = 0;
      tabEditor.Text = "Editor";
      tabEditor.UseVisualStyleBackColor = true;
      // 
      // panelColorPalette
      // 
      panelColorPalette.Location = new System.Drawing.Point( 277, 3 );
      panelColorPalette.Name = "panelColorPalette";
      panelColorPalette.Size = new System.Drawing.Size( 20, 186 );
      panelColorPalette.TabIndex = 65;
      panelColorPalette.SizeChanged +=  panelColorPalette_SizeChanged ;
      // 
      // btnMoveSelectionToTarget
      // 
      btnMoveSelectionToTarget.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnMoveSelectionToTarget.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnMoveSelectionToTarget.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnMoveSelectionToTarget.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnMoveSelectionToTarget.DisplayAntiAliased = true;
      btnMoveSelectionToTarget.Image = null;
      btnMoveSelectionToTarget.Location = new System.Drawing.Point( 514, 327 );
      btnMoveSelectionToTarget.Name = "btnMoveSelectionToTarget";
      btnMoveSelectionToTarget.Size = new System.Drawing.Size( 97, 23 );
      btnMoveSelectionToTarget.TabIndex = 22;
      btnMoveSelectionToTarget.Text = "Move to Index";
      btnMoveSelectionToTarget.Click +=  btnMoveSelectionToTarget_Click ;
      // 
      // panelColorChooser
      // 
      panelColorChooser.Location = new System.Drawing.Point( 780, 275 );
      panelColorChooser.Name = "panelColorChooser";
      panelColorChooser.Size = new System.Drawing.Size( 280, 120 );
      panelColorChooser.TabIndex = 26;
      // 
      // editCharName
      // 
      editCharName.Location = new System.Drawing.Point( 368, 358 );
      editCharName.Name = "editCharName";
      editCharName.Size = new System.Drawing.Size( 140, 23 );
      editCharName.TabIndex = 15;
      editCharName.TextChanged +=  editCharName_TextChanged ;
      // 
      // editCharsetName
      // 
      editCharsetName.Location = new System.Drawing.Point( 368, 329 );
      editCharsetName.Name = "editCharsetName";
      editCharsetName.Size = new System.Drawing.Size( 140, 23 );
      editCharsetName.TabIndex = 14;
      editCharsetName.TextChanged +=  editCharsetName_TextChanged ;
      // 
      // btnRestoreDefault
      // 
      btnRestoreDefault.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnRestoreDefault.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnRestoreDefault.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnRestoreDefault.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnRestoreDefault.DisplayAntiAliased = true;
      btnRestoreDefault.Image = null;
      btnRestoreDefault.Location = new System.Drawing.Point( 617, 298 );
      btnRestoreDefault.Name = "btnRestoreDefault";
      btnRestoreDefault.Size = new System.Drawing.Size( 122, 23 );
      btnRestoreDefault.TabIndex = 21;
      btnRestoreDefault.Text = "Restore Default";
      btnRestoreDefault.Click +=  btnRestoreDefault_Click ;
      // 
      // btnClearChars
      // 
      btnClearChars.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnClearChars.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnClearChars.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnClearChars.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnClearChars.DisplayAntiAliased = true;
      btnClearChars.Image = null;
      btnClearChars.Location = new System.Drawing.Point( 514, 298 );
      btnClearChars.Name = "btnClearChars";
      btnClearChars.Size = new System.Drawing.Size( 97, 23 );
      btnClearChars.TabIndex = 20;
      btnClearChars.Text = "Clear";
      btnClearChars.Click +=  btnClear_Click ;
      // 
      // btnRemoveDuplicates
      // 
      btnRemoveDuplicates.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnRemoveDuplicates.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnRemoveDuplicates.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnRemoveDuplicates.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnRemoveDuplicates.DisplayAntiAliased = true;
      btnRemoveDuplicates.Image = null;
      btnRemoveDuplicates.Location = new System.Drawing.Point( 617, 356 );
      btnRemoveDuplicates.Name = "btnRemoveDuplicates";
      btnRemoveDuplicates.Size = new System.Drawing.Size( 122, 23 );
      btnRemoveDuplicates.TabIndex = 25;
      btnRemoveDuplicates.Text = "Remove Duplicates";
      btnRemoveDuplicates.Click +=  btnRemoveDuplicates_Click ;
      // 
      // panelColorSettings
      // 
      panelColorSettings.Location = new System.Drawing.Point( 303, 3 );
      panelColorSettings.Name = "panelColorSettings";
      panelColorSettings.Size = new System.Drawing.Size( 205, 186 );
      panelColorSettings.TabIndex = 9;
      // 
      // canvasEditor
      // 
      canvasEditor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      canvasEditor.Location = new System.Drawing.Point( 6, 3 );
      canvasEditor.Name = "canvasEditor";
      canvasEditor.Size = new System.Drawing.Size( 265, 260 );
      canvasEditor.TabIndex = 54;
      canvasEditor.TabStop = false;
      canvasEditor.Paint +=  canvasEditor_Paint ;
      canvasEditor.MouseDown +=  canvasEditor_MouseDown ;
      canvasEditor.MouseMove +=  canvasEditor_MouseMove ;
      // 
      // comboCharsetMode
      // 
      comboCharsetMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      comboCharsetMode.FormattingEnabled = true;
      comboCharsetMode.Location = new System.Drawing.Point( 368, 218 );
      comboCharsetMode.Name = "comboCharsetMode";
      comboCharsetMode.Size = new System.Drawing.Size( 140, 23 );
      comboCharsetMode.TabIndex = 10;
      comboCharsetMode.SelectedIndexChanged +=  comboCharsetMode_SelectedIndexChanged ;
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new System.Drawing.Point( 277, 361 );
      label2.Name = "label2";
      label2.Size = new System.Drawing.Size( 70, 15 );
      label2.TabIndex = 17;
      label2.Text = "Char Name:";
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new System.Drawing.Point( 277, 332 );
      label1.Name = "label1";
      label1.Size = new System.Drawing.Size( 85, 15 );
      label1.TabIndex = 17;
      label1.Text = "Charset Name:";
      // 
      // labelCharsetMode
      // 
      labelCharsetMode.AutoSize = true;
      labelCharsetMode.Location = new System.Drawing.Point( 303, 221 );
      labelCharsetMode.Name = "labelCharsetMode";
      labelCharsetMode.Size = new System.Drawing.Size( 41, 15 );
      labelCharsetMode.TabIndex = 17;
      labelCharsetMode.Text = "Mode:";
      // 
      // picturePlayground
      // 
      picturePlayground.AutoResize = false;
      picturePlayground.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      picturePlayground.Location = new System.Drawing.Point( 780, 3 );
      picturePlayground.Name = "picturePlayground";
      picturePlayground.Size = new System.Drawing.Size( 260, 260 );
      picturePlayground.TabIndex = 51;
      picturePlayground.TabStop = false;
      picturePlayground.MouseDown +=  picturePlayground_MouseDown ;
      picturePlayground.MouseMove +=  picturePlayground_MouseMove ;
      // 
      // panelCharacters
      // 
      panelCharacters.AllowPopup = false;
      panelCharacters.AutoScroll = true;
      panelCharacters.AutoScrollHorizontalMaximum = 100;
      panelCharacters.AutoScrollHorizontalMinimum = 0;
      panelCharacters.AutoScrollHPos = 0;
      panelCharacters.AutoScrollVerticalMaximum = -23;
      panelCharacters.AutoScrollVerticalMinimum = 0;
      panelCharacters.AutoScrollVPos = 0;
      panelCharacters.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      panelCharacters.EnableAutoScrollHorizontal = true;
      panelCharacters.EnableAutoScrollVertical = true;
      panelCharacters.HottrackColor = 2151694591U;
      panelCharacters.ItemHeight = 8;
      panelCharacters.ItemWidth = 8;
      panelCharacters.Location = new System.Drawing.Point( 514, 3 );
      panelCharacters.Name = "panelCharacters";
      panelCharacters.PixelFormat = GR.Drawing.PixelFormat.DontCare;
      panelCharacters.SelectionDisplayedAsFrame = false;
      panelCharacters.Size = new System.Drawing.Size( 260, 260 );
      panelCharacters.TabIndex = 16;
      panelCharacters.TabStop = true;
      panelCharacters.VisibleAutoScrollHorizontal = false;
      panelCharacters.VisibleAutoScrollVertical = false;
      panelCharacters.SelectionChanged +=  panelCharacters_SelectionChanged ;
      // 
      // tabCategories
      // 
      tabCategories.Controls.Add( btnMoveCategoryDown );
      tabCategories.Controls.Add( btnMoveCategoryUp );
      tabCategories.Controls.Add( groupAllCategories );
      tabCategories.Controls.Add( groupCategorySpecific );
      tabCategories.Controls.Add( btnDelete );
      tabCategories.Controls.Add( btnAddCategory );
      tabCategories.Controls.Add( listCategories );
      tabCategories.Controls.Add( editCategoryName );
      tabCategories.Controls.Add( label3 );
      tabCategories.Location = new System.Drawing.Point( 4, 24 );
      tabCategories.Name = "tabCategories";
      tabCategories.Padding = new System.Windows.Forms.Padding( 3 );
      tabCategories.Size = new System.Drawing.Size( 1135, 462 );
      tabCategories.TabIndex = 1;
      tabCategories.Text = "Categories";
      tabCategories.UseVisualStyleBackColor = true;
      // 
      // btnMoveCategoryDown
      // 
      btnMoveCategoryDown.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnMoveCategoryDown.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnMoveCategoryDown.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnMoveCategoryDown.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnMoveCategoryDown.DisplayAntiAliased = true;
      btnMoveCategoryDown.Enabled = false;
      btnMoveCategoryDown.Image = null;
      btnMoveCategoryDown.Location = new System.Drawing.Point( 90, 201 );
      btnMoveCategoryDown.Name = "btnMoveCategoryDown";
      btnMoveCategoryDown.Size = new System.Drawing.Size( 75, 23 );
      btnMoveCategoryDown.TabIndex = 12;
      btnMoveCategoryDown.Text = "Move Down";
      btnMoveCategoryDown.Click +=  btnMoveCategoryDown_Click ;
      // 
      // btnMoveCategoryUp
      // 
      btnMoveCategoryUp.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnMoveCategoryUp.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnMoveCategoryUp.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnMoveCategoryUp.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnMoveCategoryUp.DisplayAntiAliased = true;
      btnMoveCategoryUp.Enabled = false;
      btnMoveCategoryUp.Image = null;
      btnMoveCategoryUp.Location = new System.Drawing.Point( 9, 201 );
      btnMoveCategoryUp.Name = "btnMoveCategoryUp";
      btnMoveCategoryUp.Size = new System.Drawing.Size( 75, 23 );
      btnMoveCategoryUp.TabIndex = 12;
      btnMoveCategoryUp.Text = "Move Up";
      btnMoveCategoryUp.Click +=  btnMoveCategoryUp_Click ;
      // 
      // groupAllCategories
      // 
      groupAllCategories.Controls.Add( btnSortCategories );
      groupAllCategories.Location = new System.Drawing.Point( 261, 119 );
      groupAllCategories.Name = "groupAllCategories";
      groupAllCategories.Size = new System.Drawing.Size( 255, 76 );
      groupAllCategories.TabIndex = 10;
      groupAllCategories.TabStop = false;
      groupAllCategories.Text = "All Categories";
      // 
      // btnSortCategories
      // 
      btnSortCategories.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnSortCategories.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnSortCategories.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnSortCategories.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnSortCategories.DisplayAntiAliased = true;
      btnSortCategories.Image = null;
      btnSortCategories.Location = new System.Drawing.Point( 6, 19 );
      btnSortCategories.Name = "btnSortCategories";
      btnSortCategories.Size = new System.Drawing.Size( 105, 23 );
      btnSortCategories.TabIndex = 3;
      btnSortCategories.Text = "Sort by Categories";
      btnSortCategories.Click +=  btnSortCategories_Click ;
      // 
      // groupCategorySpecific
      // 
      groupCategorySpecific.Controls.Add( label5 );
      groupCategorySpecific.Controls.Add( editCollapseIndex );
      groupCategorySpecific.Controls.Add( btnCollapseCategory );
      groupCategorySpecific.Controls.Add( btnReseatCategory );
      groupCategorySpecific.Location = new System.Drawing.Point( 261, 37 );
      groupCategorySpecific.Name = "groupCategorySpecific";
      groupCategorySpecific.Size = new System.Drawing.Size( 255, 76 );
      groupCategorySpecific.TabIndex = 11;
      groupCategorySpecific.TabStop = false;
      groupCategorySpecific.Text = "Selected Category";
      // 
      // label5
      // 
      label5.AutoSize = true;
      label5.Location = new System.Drawing.Point( 117, 52 );
      label5.Name = "label5";
      label5.Size = new System.Drawing.Size( 51, 15 );
      label5.TabIndex = 6;
      label5.Text = "at index:";
      // 
      // editCollapseIndex
      // 
      editCollapseIndex.Location = new System.Drawing.Point( 180, 49 );
      editCollapseIndex.Name = "editCollapseIndex";
      editCollapseIndex.Size = new System.Drawing.Size( 69, 23 );
      editCollapseIndex.TabIndex = 5;
      // 
      // btnCollapseCategory
      // 
      btnCollapseCategory.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnCollapseCategory.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnCollapseCategory.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnCollapseCategory.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnCollapseCategory.DisplayAntiAliased = true;
      btnCollapseCategory.Enabled = false;
      btnCollapseCategory.Image = null;
      btnCollapseCategory.Location = new System.Drawing.Point( 6, 19 );
      btnCollapseCategory.Name = "btnCollapseCategory";
      btnCollapseCategory.Size = new System.Drawing.Size( 140, 23 );
      btnCollapseCategory.TabIndex = 3;
      btnCollapseCategory.Text = "Collapse Unique Chars";
      btnCollapseCategory.Click +=  btnCollapseCategory_Click ;
      // 
      // btnReseatCategory
      // 
      btnReseatCategory.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnReseatCategory.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnReseatCategory.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnReseatCategory.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnReseatCategory.DisplayAntiAliased = true;
      btnReseatCategory.Enabled = false;
      btnReseatCategory.Image = null;
      btnReseatCategory.Location = new System.Drawing.Point( 6, 47 );
      btnReseatCategory.Name = "btnReseatCategory";
      btnReseatCategory.Size = new System.Drawing.Size( 105, 23 );
      btnReseatCategory.TabIndex = 3;
      btnReseatCategory.Text = "Reseat Category";
      btnReseatCategory.Click +=  btnReseatCategory_Click ;
      // 
      // btnDelete
      // 
      btnDelete.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnDelete.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnDelete.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnDelete.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnDelete.DisplayAntiAliased = true;
      btnDelete.Enabled = false;
      btnDelete.Image = null;
      btnDelete.Location = new System.Drawing.Point( 342, 8 );
      btnDelete.Name = "btnDelete";
      btnDelete.Size = new System.Drawing.Size( 96, 23 );
      btnDelete.TabIndex = 8;
      btnDelete.Text = "Delete Category";
      btnDelete.Click +=  btnDelete_Click ;
      // 
      // btnAddCategory
      // 
      btnAddCategory.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnAddCategory.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnAddCategory.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnAddCategory.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnAddCategory.DisplayAntiAliased = true;
      btnAddCategory.Enabled = false;
      btnAddCategory.Image = null;
      btnAddCategory.Location = new System.Drawing.Point( 261, 8 );
      btnAddCategory.Name = "btnAddCategory";
      btnAddCategory.Size = new System.Drawing.Size( 75, 23 );
      btnAddCategory.TabIndex = 9;
      btnAddCategory.Text = "Add";
      btnAddCategory.Click +=  btnAddCategory_Click ;
      // 
      // listCategories
      // 
      listCategories.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2 } );
      listCategories.FullRowSelect = true;
      listCategories.Location = new System.Drawing.Point( 9, 36 );
      listCategories.Name = "listCategories";
      listCategories.ShowGroups = false;
      listCategories.Size = new System.Drawing.Size( 246, 159 );
      listCategories.TabIndex = 7;
      listCategories.UseCompatibleStateImageBehavior = false;
      listCategories.View = System.Windows.Forms.View.Details;
      listCategories.SelectedIndexChanged +=  listCategories_SelectedIndexChanged ;
      // 
      // columnHeader1
      // 
      columnHeader1.Text = "Name";
      columnHeader1.Width = 150;
      // 
      // columnHeader2
      // 
      columnHeader2.Text = "No. Chars";
      columnHeader2.Width = 67;
      // 
      // editCategoryName
      // 
      editCategoryName.Location = new System.Drawing.Point( 81, 10 );
      editCategoryName.Name = "editCategoryName";
      editCategoryName.Size = new System.Drawing.Size( 174, 23 );
      editCategoryName.TabIndex = 6;
      editCategoryName.TextChanged +=  editCategoryName_TextChanged ;
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new System.Drawing.Point( 6, 13 );
      label3.Name = "label3";
      label3.Size = new System.Drawing.Size( 58, 15 );
      label3.TabIndex = 5;
      label3.Text = "Category:";
      // 
      // CharacterEditor
      // 
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      Controls.Add( tabCharacterEditor );
      Name = "CharacterEditor";
      Size = new System.Drawing.Size( 1143, 490 );
      tabCharacterEditor.ResumeLayout( false );
      tabEditor.ResumeLayout( false );
      tabEditor.PerformLayout();
      ( (System.ComponentModel.ISupportInitialize)canvasEditor ).EndInit();
      ( (System.ComponentModel.ISupportInitialize)picturePlayground ).EndInit();
      tabCategories.ResumeLayout( false );
      tabCategories.PerformLayout();
      groupAllCategories.ResumeLayout( false );
      groupCategorySpecific.ResumeLayout( false );
      groupCategorySpecific.PerformLayout();
      ResumeLayout( false );

    }

    #endregion
    private GR.Forms.FastPictureBox picturePlayground;
        private DecentForms.Button btnClearChars;
        private DecentForms.Button btnPaste;
        private DecentForms.Button btnCopy;
        private DecentForms.Button btnInvert;
        private DecentForms.Button btnMirrorY;
        private DecentForms.Button btnMirrorX;
        private DecentForms.Button btnShiftDown;
        private DecentForms.Button btnShiftUp;
        private DecentForms.Button btnShiftRight;
        private DecentForms.Button button3;
        private DecentForms.Button btnRotateLeft;
        private DecentForms.Button btnShiftLeft;
        private System.Windows.Forms.ComboBox comboCategories;
        private DecentForms.Button btnPasteFromClipboard;
        private System.Windows.Forms.Label labelCategory;
        private System.Windows.Forms.Label labelCharNo;
        private GR.Forms.ImageListbox panelCharacters;
        private System.Windows.Forms.CheckBox checkShowGrid;
        private System.Windows.Forms.CheckBox checkPasteMultiColor;
        private DecentForms.Button btnMoveSelectionToTarget;
        private System.Windows.Forms.TextBox editMoveTargetIndex;
    private CustomDrawControl canvasEditor;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.TabControl tabCharacterEditor;
    private System.Windows.Forms.TabPage tabEditor;
    private System.Windows.Forms.TabPage tabCategories;
    private System.Windows.Forms.GroupBox groupAllCategories;
    private DecentForms.Button btnSortCategories;
    private System.Windows.Forms.GroupBox groupCategorySpecific;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox editCollapseIndex;
    private DecentForms.Button btnCollapseCategory;
    private DecentForms.Button btnReseatCategory;
    private DecentForms.Button btnDelete;
    private DecentForms.Button btnAddCategory;
    private System.Windows.Forms.ListView listCategories;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.TextBox editCategoryName;
    private System.Windows.Forms.Label label3;
    private DecentForms.Button btnMoveCategoryDown;
    private DecentForms.Button btnMoveCategoryUp;
    private System.Windows.Forms.ComboBox comboCharsetMode;
    private System.Windows.Forms.Label labelCharsetMode;
    private System.Windows.Forms.Panel panelColorSettings;
    private DecentForms.Button btnHighlightDuplicates;
    private System.Windows.Forms.Panel panelColorChooser;
    private DecentForms.Button btnRemoveDuplicates;
    private DecentForms.Button btnRestoreDefault;
    private System.Windows.Forms.TextBox editCharName;
    private System.Windows.Forms.TextBox editCharsetName;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Panel panelColorPalette;
  }
}
