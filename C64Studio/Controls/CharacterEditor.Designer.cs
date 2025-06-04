﻿namespace RetroDevStudio.Controls
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CharacterEditor));
      GR.Image.FastImage fastImage2 = new GR.Image.FastImage();
      this.btnClearChars = new DecentForms.Button();
      this.comboCategories = new System.Windows.Forms.ComboBox();
      this.btnPasteFromClipboard = new DecentForms.Button();
      this.label4 = new System.Windows.Forms.Label();
      this.labelCharNo = new System.Windows.Forms.Label();
      this.checkShowGrid = new System.Windows.Forms.CheckBox();
      this.checkPasteMultiColor = new System.Windows.Forms.CheckBox();
      this.btnMoveSelectionToTarget = new DecentForms.Button();
      this.editMoveTargetIndex = new System.Windows.Forms.TextBox();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.btnPaste = new DecentForms.Button();
      this.btnCopy = new DecentForms.Button();
      this.btnInvert = new DecentForms.Button();
      this.btnMirrorY = new DecentForms.Button();
      this.btnMirrorX = new DecentForms.Button();
      this.btnShiftDown = new DecentForms.Button();
      this.btnShiftUp = new DecentForms.Button();
      this.btnShiftRight = new DecentForms.Button();
      this.button3 = new DecentForms.Button();
      this.btnRotateLeft = new DecentForms.Button();
      this.btnShiftLeft = new DecentForms.Button();
      this.tabCharacterEditor = new System.Windows.Forms.TabControl();
      this.tabEditor = new System.Windows.Forms.TabPage();
      this.panelColorChooser = new System.Windows.Forms.Panel();
      this.btnHighlightDuplicates = new DecentForms.Button();
      this.panelColorSettings = new System.Windows.Forms.Panel();
      this.canvasEditor = new RetroDevStudio.Controls.CustomDrawControl();
      this.comboCharsetMode = new System.Windows.Forms.ComboBox();
      this.labelCharsetMode = new System.Windows.Forms.Label();
      this.picturePlayground = new GR.Forms.FastPictureBox();
      this.panelCharacters = new GR.Forms.ImageListbox();
      this.tabCategories = new System.Windows.Forms.TabPage();
      this.btnMoveCategoryDown = new DecentForms.Button();
      this.btnMoveCategoryUp = new DecentForms.Button();
      this.groupAllCategories = new System.Windows.Forms.GroupBox();
      this.btnSortCategories = new DecentForms.Button();
      this.groupCategorySpecific = new System.Windows.Forms.GroupBox();
      this.label5 = new System.Windows.Forms.Label();
      this.editCollapseIndex = new System.Windows.Forms.TextBox();
      this.btnCollapseCategory = new DecentForms.Button();
      this.btnReseatCategory = new DecentForms.Button();
      this.btnDelete = new DecentForms.Button();
      this.btnAddCategory = new DecentForms.Button();
      this.listCategories = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.editCategoryName = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.btnRemoveDuplicates = new DecentForms.Button();
      this.tabCharacterEditor.SuspendLayout();
      this.tabEditor.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.canvasEditor)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picturePlayground)).BeginInit();
      this.tabCategories.SuspendLayout();
      this.groupAllCategories.SuspendLayout();
      this.groupCategorySpecific.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnClearChars
      // 
      this.btnClearChars.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnClearChars.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnClearChars.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnClearChars.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnClearChars.Image = null;
      this.btnClearChars.Location = new System.Drawing.Point(681, 269);
      this.btnClearChars.Name = "btnClearChars";
      this.btnClearChars.Size = new System.Drawing.Size(58, 26);
      this.btnClearChars.TabIndex = 0;
      this.btnClearChars.Text = "Clear";
      this.btnClearChars.Click += new DecentForms.EventHandler(this.btnClear_Click);
      // 
      // comboCategories
      // 
      this.comboCategories.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboCategories.FormattingEnabled = true;
      this.comboCategories.Location = new System.Drawing.Point(387, 218);
      this.comboCategories.Name = "comboCategories";
      this.comboCategories.Size = new System.Drawing.Size(121, 21);
      this.comboCategories.TabIndex = 10;
      this.comboCategories.SelectedIndexChanged += new System.EventHandler(this.comboCategories_SelectedIndexChanged);
      // 
      // btnPasteFromClipboard
      // 
      this.btnPasteFromClipboard.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnPasteFromClipboard.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnPasteFromClipboard.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnPasteFromClipboard.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnPasteFromClipboard.Image = null;
      this.btnPasteFromClipboard.Location = new System.Drawing.Point(578, 269);
      this.btnPasteFromClipboard.Name = "btnPasteFromClipboard";
      this.btnPasteFromClipboard.Size = new System.Drawing.Size(97, 26);
      this.btnPasteFromClipboard.TabIndex = 17;
      this.btnPasteFromClipboard.Text = "Paste Image";
      this.toolTip1.SetToolTip(this.btnPasteFromClipboard, "Paste Image");
      this.btnPasteFromClipboard.Click += new DecentForms.EventHandler(this.btnPasteFromClipboard_Click);
      // 
      // label4
      // 
      this.label4.Location = new System.Drawing.Point(277, 221);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(86, 23);
      this.label4.TabIndex = 35;
      this.label4.Text = "Category:";
      // 
      // labelCharNo
      // 
      this.labelCharNo.Location = new System.Drawing.Point(277, 192);
      this.labelCharNo.Name = "labelCharNo";
      this.labelCharNo.Size = new System.Drawing.Size(231, 23);
      this.labelCharNo.TabIndex = 34;
      this.labelCharNo.Text = "label1";
      this.labelCharNo.Paint += new System.Windows.Forms.PaintEventHandler(this.labelCharNo_Paint);
      // 
      // checkShowGrid
      // 
      this.checkShowGrid.AutoSize = true;
      this.checkShowGrid.Location = new System.Drawing.Point(280, 246);
      this.checkShowGrid.Name = "checkShowGrid";
      this.checkShowGrid.Size = new System.Drawing.Size(75, 17);
      this.checkShowGrid.TabIndex = 11;
      this.checkShowGrid.Text = "Show Grid";
      this.checkShowGrid.UseVisualStyleBackColor = true;
      this.checkShowGrid.CheckedChanged += new System.EventHandler(this.checkShowGrid_CheckedChanged);
      // 
      // checkPasteMultiColor
      // 
      this.checkPasteMultiColor.AutoSize = true;
      this.checkPasteMultiColor.Location = new System.Drawing.Point(280, 275);
      this.checkPasteMultiColor.Name = "checkPasteMultiColor";
      this.checkPasteMultiColor.Size = new System.Drawing.Size(145, 17);
      this.checkPasteMultiColor.TabIndex = 12;
      this.checkPasteMultiColor.Text = "Force Multicolor on paste";
      this.checkPasteMultiColor.UseVisualStyleBackColor = true;
      // 
      // btnMoveSelectionToTarget
      // 
      this.btnMoveSelectionToTarget.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnMoveSelectionToTarget.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnMoveSelectionToTarget.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnMoveSelectionToTarget.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnMoveSelectionToTarget.Image = null;
      this.btnMoveSelectionToTarget.Location = new System.Drawing.Point(514, 301);
      this.btnMoveSelectionToTarget.Name = "btnMoveSelectionToTarget";
      this.btnMoveSelectionToTarget.Size = new System.Drawing.Size(97, 26);
      this.btnMoveSelectionToTarget.TabIndex = 1;
      this.btnMoveSelectionToTarget.Text = "Move to Index";
      this.btnMoveSelectionToTarget.Click += new DecentForms.EventHandler(this.btnMoveSelectionToTarget_Click);
      // 
      // editMoveTargetIndex
      // 
      this.editMoveTargetIndex.Location = new System.Drawing.Point(617, 304);
      this.editMoveTargetIndex.Name = "editMoveTargetIndex";
      this.editMoveTargetIndex.Size = new System.Drawing.Size(93, 20);
      this.editMoveTargetIndex.TabIndex = 0;
      this.editMoveTargetIndex.TextChanged += new System.EventHandler(this.editMoveTargetIndex_TextChanged);
      // 
      // btnPaste
      // 
      this.btnPaste.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnPaste.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnPaste.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnPaste.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnPaste.Image = ((System.Drawing.Image)(resources.GetObject("btnPaste.Image")));
      this.btnPaste.Location = new System.Drawing.Point(546, 269);
      this.btnPaste.Name = "btnPaste";
      this.btnPaste.Size = new System.Drawing.Size(26, 26);
      this.btnPaste.TabIndex = 16;
      this.toolTip1.SetToolTip(this.btnPaste, "Paste Characters");
      this.btnPaste.Click += new DecentForms.EventHandler(this.btnPaste_Click);
      // 
      // btnCopy
      // 
      this.btnCopy.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnCopy.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnCopy.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnCopy.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnCopy.Image = ((System.Drawing.Image)(resources.GetObject("btnCopy.Image")));
      this.btnCopy.Location = new System.Drawing.Point(514, 269);
      this.btnCopy.Name = "btnCopy";
      this.btnCopy.Size = new System.Drawing.Size(26, 26);
      this.btnCopy.TabIndex = 15;
      this.toolTip1.SetToolTip(this.btnCopy, "Copy Characters to Clipboard");
      this.btnCopy.Click += new DecentForms.EventHandler(this.btnCopy_Click);
      // 
      // btnInvert
      // 
      this.btnInvert.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnInvert.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnInvert.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnInvert.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnInvert.Image = ((System.Drawing.Image)(resources.GetObject("btnInvert.Image")));
      this.btnInvert.Location = new System.Drawing.Point(198, 269);
      this.btnInvert.Name = "btnInvert";
      this.btnInvert.Size = new System.Drawing.Size(26, 26);
      this.btnInvert.TabIndex = 6;
      this.toolTip1.SetToolTip(this.btnInvert, "Invert");
      this.btnInvert.Click += new DecentForms.EventHandler(this.btnInvert_Click);
      // 
      // btnMirrorY
      // 
      this.btnMirrorY.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnMirrorY.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnMirrorY.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnMirrorY.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnMirrorY.Image = ((System.Drawing.Image)(resources.GetObject("btnMirrorY.Image")));
      this.btnMirrorY.Location = new System.Drawing.Point(166, 269);
      this.btnMirrorY.Name = "btnMirrorY";
      this.btnMirrorY.Size = new System.Drawing.Size(26, 26);
      this.btnMirrorY.TabIndex = 5;
      this.toolTip1.SetToolTip(this.btnMirrorY, "Mirror Y");
      this.btnMirrorY.Click += new DecentForms.EventHandler(this.btnMirrorY_Click);
      // 
      // btnMirrorX
      // 
      this.btnMirrorX.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnMirrorX.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnMirrorX.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnMirrorX.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnMirrorX.Image = ((System.Drawing.Image)(resources.GetObject("btnMirrorX.Image")));
      this.btnMirrorX.Location = new System.Drawing.Point(134, 269);
      this.btnMirrorX.Name = "btnMirrorX";
      this.btnMirrorX.Size = new System.Drawing.Size(26, 26);
      this.btnMirrorX.TabIndex = 4;
      this.toolTip1.SetToolTip(this.btnMirrorX, "Mirror X");
      this.btnMirrorX.Click += new DecentForms.EventHandler(this.btnMirrorX_Click);
      // 
      // btnShiftDown
      // 
      this.btnShiftDown.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnShiftDown.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnShiftDown.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnShiftDown.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnShiftDown.Image = ((System.Drawing.Image)(resources.GetObject("btnShiftDown.Image")));
      this.btnShiftDown.Location = new System.Drawing.Point(102, 269);
      this.btnShiftDown.Name = "btnShiftDown";
      this.btnShiftDown.Size = new System.Drawing.Size(26, 26);
      this.btnShiftDown.TabIndex = 3;
      this.toolTip1.SetToolTip(this.btnShiftDown, "Shift Down");
      this.btnShiftDown.Click += new DecentForms.EventHandler(this.btnShiftDown_Click);
      // 
      // btnShiftUp
      // 
      this.btnShiftUp.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnShiftUp.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnShiftUp.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnShiftUp.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnShiftUp.Image = ((System.Drawing.Image)(resources.GetObject("btnShiftUp.Image")));
      this.btnShiftUp.Location = new System.Drawing.Point(70, 269);
      this.btnShiftUp.Name = "btnShiftUp";
      this.btnShiftUp.Size = new System.Drawing.Size(26, 26);
      this.btnShiftUp.TabIndex = 2;
      this.toolTip1.SetToolTip(this.btnShiftUp, "Shift Up");
      this.btnShiftUp.Click += new DecentForms.EventHandler(this.btnShiftUp_Click);
      // 
      // btnShiftRight
      // 
      this.btnShiftRight.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnShiftRight.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnShiftRight.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnShiftRight.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnShiftRight.Image = ((System.Drawing.Image)(resources.GetObject("btnShiftRight.Image")));
      this.btnShiftRight.Location = new System.Drawing.Point(38, 269);
      this.btnShiftRight.Name = "btnShiftRight";
      this.btnShiftRight.Size = new System.Drawing.Size(26, 26);
      this.btnShiftRight.TabIndex = 1;
      this.toolTip1.SetToolTip(this.btnShiftRight, "Shift Right");
      this.btnShiftRight.Click += new DecentForms.EventHandler(this.btnShiftRight_Click);
      // 
      // button3
      // 
      this.button3.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.button3.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.button3.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.button3.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.button3.Image = ((System.Drawing.Image)(resources.GetObject("button3.Image")));
      this.button3.Location = new System.Drawing.Point(38, 301);
      this.button3.Name = "button3";
      this.button3.Size = new System.Drawing.Size(26, 26);
      this.button3.TabIndex = 8;
      this.toolTip1.SetToolTip(this.button3, "Rotate Right");
      this.button3.Click += new DecentForms.EventHandler(this.btnRotateRight_Click);
      // 
      // btnRotateLeft
      // 
      this.btnRotateLeft.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnRotateLeft.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnRotateLeft.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnRotateLeft.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnRotateLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnRotateLeft.Image")));
      this.btnRotateLeft.Location = new System.Drawing.Point(6, 301);
      this.btnRotateLeft.Name = "btnRotateLeft";
      this.btnRotateLeft.Size = new System.Drawing.Size(26, 26);
      this.btnRotateLeft.TabIndex = 7;
      this.toolTip1.SetToolTip(this.btnRotateLeft, "Rotate Left");
      this.btnRotateLeft.Click += new DecentForms.EventHandler(this.btnRotateLeft_Click);
      // 
      // btnShiftLeft
      // 
      this.btnShiftLeft.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnShiftLeft.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnShiftLeft.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnShiftLeft.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnShiftLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnShiftLeft.Image")));
      this.btnShiftLeft.Location = new System.Drawing.Point(6, 269);
      this.btnShiftLeft.Name = "btnShiftLeft";
      this.btnShiftLeft.Size = new System.Drawing.Size(26, 26);
      this.btnShiftLeft.TabIndex = 0;
      this.toolTip1.SetToolTip(this.btnShiftLeft, "Shift Left");
      this.btnShiftLeft.Click += new DecentForms.EventHandler(this.btnShiftLeft_Click);
      // 
      // tabCharacterEditor
      // 
      this.tabCharacterEditor.Controls.Add(this.tabEditor);
      this.tabCharacterEditor.Controls.Add(this.tabCategories);
      this.tabCharacterEditor.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabCharacterEditor.Location = new System.Drawing.Point(0, 0);
      this.tabCharacterEditor.Name = "tabCharacterEditor";
      this.tabCharacterEditor.SelectedIndex = 0;
      this.tabCharacterEditor.Size = new System.Drawing.Size(1070, 490);
      this.tabCharacterEditor.TabIndex = 0;
      // 
      // tabEditor
      // 
      this.tabEditor.Controls.Add(this.btnMoveSelectionToTarget);
      this.tabEditor.Controls.Add(this.panelColorChooser);
      this.tabEditor.Controls.Add(this.editMoveTargetIndex);
      this.tabEditor.Controls.Add(this.btnClearChars);
      this.tabEditor.Controls.Add(this.btnRemoveDuplicates);
      this.tabEditor.Controls.Add(this.btnHighlightDuplicates);
      this.tabEditor.Controls.Add(this.panelColorSettings);
      this.tabEditor.Controls.Add(this.canvasEditor);
      this.tabEditor.Controls.Add(this.comboCharsetMode);
      this.tabEditor.Controls.Add(this.labelCharsetMode);
      this.tabEditor.Controls.Add(this.picturePlayground);
      this.tabEditor.Controls.Add(this.btnPaste);
      this.tabEditor.Controls.Add(this.btnCopy);
      this.tabEditor.Controls.Add(this.btnInvert);
      this.tabEditor.Controls.Add(this.btnMirrorY);
      this.tabEditor.Controls.Add(this.checkPasteMultiColor);
      this.tabEditor.Controls.Add(this.btnMirrorX);
      this.tabEditor.Controls.Add(this.checkShowGrid);
      this.tabEditor.Controls.Add(this.btnShiftDown);
      this.tabEditor.Controls.Add(this.panelCharacters);
      this.tabEditor.Controls.Add(this.btnShiftUp);
      this.tabEditor.Controls.Add(this.labelCharNo);
      this.tabEditor.Controls.Add(this.btnShiftRight);
      this.tabEditor.Controls.Add(this.label4);
      this.tabEditor.Controls.Add(this.button3);
      this.tabEditor.Controls.Add(this.btnPasteFromClipboard);
      this.tabEditor.Controls.Add(this.btnRotateLeft);
      this.tabEditor.Controls.Add(this.comboCategories);
      this.tabEditor.Controls.Add(this.btnShiftLeft);
      this.tabEditor.Location = new System.Drawing.Point(4, 22);
      this.tabEditor.Name = "tabEditor";
      this.tabEditor.Padding = new System.Windows.Forms.Padding(3);
      this.tabEditor.Size = new System.Drawing.Size(1062, 464);
      this.tabEditor.TabIndex = 0;
      this.tabEditor.Text = "Editor";
      this.tabEditor.UseVisualStyleBackColor = true;
      // 
      // panelColorChooser
      // 
      this.panelColorChooser.Location = new System.Drawing.Point(780, 275);
      this.panelColorChooser.Name = "panelColorChooser";
      this.panelColorChooser.Size = new System.Drawing.Size(280, 120);
      this.panelColorChooser.TabIndex = 55;
      // 
      // btnHighlightDuplicates
      // 
      this.btnHighlightDuplicates.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnHighlightDuplicates.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnHighlightDuplicates.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnHighlightDuplicates.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnHighlightDuplicates.Image = null;
      this.btnHighlightDuplicates.Location = new System.Drawing.Point(514, 333);
      this.btnHighlightDuplicates.Name = "btnHighlightDuplicates";
      this.btnHighlightDuplicates.Size = new System.Drawing.Size(97, 26);
      this.btnHighlightDuplicates.TabIndex = 18;
      this.btnHighlightDuplicates.Text = "Duplicates";
      this.toolTip1.SetToolTip(this.btnHighlightDuplicates, "Highlight duplicates");
      this.btnHighlightDuplicates.Click += new DecentForms.EventHandler(this.btnHighlightDuplicates_Click);
      // 
      // panelColorSettings
      // 
      this.panelColorSettings.Location = new System.Drawing.Point(277, 3);
      this.panelColorSettings.Name = "panelColorSettings";
      this.panelColorSettings.Size = new System.Drawing.Size(231, 186);
      this.panelColorSettings.TabIndex = 9;
      // 
      // canvasEditor
      // 
      this.canvasEditor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.canvasEditor.Location = new System.Drawing.Point(6, 3);
      this.canvasEditor.Name = "canvasEditor";
      this.canvasEditor.Size = new System.Drawing.Size(265, 260);
      this.canvasEditor.TabIndex = 54;
      this.canvasEditor.TabStop = false;
      this.canvasEditor.Paint += new System.Windows.Forms.PaintEventHandler(this.canvasEditor_Paint);
      this.canvasEditor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.canvasEditor_MouseDown);
      this.canvasEditor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.canvasEditor_MouseMove);
      // 
      // comboCharsetMode
      // 
      this.comboCharsetMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboCharsetMode.FormattingEnabled = true;
      this.comboCharsetMode.Location = new System.Drawing.Point(320, 301);
      this.comboCharsetMode.Name = "comboCharsetMode";
      this.comboCharsetMode.Size = new System.Drawing.Size(169, 21);
      this.comboCharsetMode.TabIndex = 13;
      this.comboCharsetMode.SelectedIndexChanged += new System.EventHandler(this.comboCharsetMode_SelectedIndexChanged);
      // 
      // labelCharsetMode
      // 
      this.labelCharsetMode.AutoSize = true;
      this.labelCharsetMode.Location = new System.Drawing.Point(277, 304);
      this.labelCharsetMode.Name = "labelCharsetMode";
      this.labelCharsetMode.Size = new System.Drawing.Size(37, 13);
      this.labelCharsetMode.TabIndex = 17;
      this.labelCharsetMode.Text = "Mode:";
      // 
      // picturePlayground
      // 
      this.picturePlayground.AutoResize = false;
      this.picturePlayground.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.picturePlayground.DisplayPage = fastImage2;
      this.picturePlayground.Image = null;
      this.picturePlayground.Location = new System.Drawing.Point(780, 3);
      this.picturePlayground.Name = "picturePlayground";
      this.picturePlayground.Size = new System.Drawing.Size(260, 260);
      this.picturePlayground.TabIndex = 51;
      this.picturePlayground.TabStop = false;
      this.picturePlayground.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picturePlayground_MouseDown);
      this.picturePlayground.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picturePlayground_MouseMove);
      // 
      // panelCharacters
      // 
      this.panelCharacters.AllowPopup = false;
      this.panelCharacters.AutoScroll = true;
      this.panelCharacters.AutoScrollHorizontalMaximum = 100;
      this.panelCharacters.AutoScrollHorizontalMinimum = 0;
      this.panelCharacters.AutoScrollHPos = 0;
      this.panelCharacters.AutoScrollVerticalMaximum = -23;
      this.panelCharacters.AutoScrollVerticalMinimum = 0;
      this.panelCharacters.AutoScrollVPos = 0;
      this.panelCharacters.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panelCharacters.EnableAutoScrollHorizontal = true;
      this.panelCharacters.EnableAutoScrollVertical = true;
      this.panelCharacters.HottrackColor = ((uint)(2151694591u));
      this.panelCharacters.ItemHeight = 8;
      this.panelCharacters.ItemWidth = 8;
      this.panelCharacters.Location = new System.Drawing.Point(514, 3);
      this.panelCharacters.Name = "panelCharacters";
      this.panelCharacters.PixelFormat = GR.Drawing.PixelFormat.DontCare;
      this.panelCharacters.SelectedIndex = -1;
      this.panelCharacters.Size = new System.Drawing.Size(260, 260);
      this.panelCharacters.TabIndex = 14;
      this.panelCharacters.TabStop = true;
      this.panelCharacters.VisibleAutoScrollHorizontal = false;
      this.panelCharacters.VisibleAutoScrollVertical = false;
      this.panelCharacters.SelectionChanged += new System.EventHandler(this.panelCharacters_SelectionChanged);
      // 
      // tabCategories
      // 
      this.tabCategories.Controls.Add(this.btnMoveCategoryDown);
      this.tabCategories.Controls.Add(this.btnMoveCategoryUp);
      this.tabCategories.Controls.Add(this.groupAllCategories);
      this.tabCategories.Controls.Add(this.groupCategorySpecific);
      this.tabCategories.Controls.Add(this.btnDelete);
      this.tabCategories.Controls.Add(this.btnAddCategory);
      this.tabCategories.Controls.Add(this.listCategories);
      this.tabCategories.Controls.Add(this.editCategoryName);
      this.tabCategories.Controls.Add(this.label3);
      this.tabCategories.Location = new System.Drawing.Point(4, 22);
      this.tabCategories.Name = "tabCategories";
      this.tabCategories.Padding = new System.Windows.Forms.Padding(3);
      this.tabCategories.Size = new System.Drawing.Size(1062, 464);
      this.tabCategories.TabIndex = 1;
      this.tabCategories.Text = "Categories";
      this.tabCategories.UseVisualStyleBackColor = true;
      // 
      // btnMoveCategoryDown
      // 
      this.btnMoveCategoryDown.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnMoveCategoryDown.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnMoveCategoryDown.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnMoveCategoryDown.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnMoveCategoryDown.Enabled = false;
      this.btnMoveCategoryDown.Image = null;
      this.btnMoveCategoryDown.Location = new System.Drawing.Point(90, 201);
      this.btnMoveCategoryDown.Name = "btnMoveCategoryDown";
      this.btnMoveCategoryDown.Size = new System.Drawing.Size(75, 23);
      this.btnMoveCategoryDown.TabIndex = 12;
      this.btnMoveCategoryDown.Text = "Move Down";
      this.btnMoveCategoryDown.Click += new DecentForms.EventHandler(this.btnMoveCategoryDown_Click);
      // 
      // btnMoveCategoryUp
      // 
      this.btnMoveCategoryUp.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnMoveCategoryUp.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnMoveCategoryUp.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnMoveCategoryUp.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnMoveCategoryUp.Enabled = false;
      this.btnMoveCategoryUp.Image = null;
      this.btnMoveCategoryUp.Location = new System.Drawing.Point(9, 201);
      this.btnMoveCategoryUp.Name = "btnMoveCategoryUp";
      this.btnMoveCategoryUp.Size = new System.Drawing.Size(75, 23);
      this.btnMoveCategoryUp.TabIndex = 12;
      this.btnMoveCategoryUp.Text = "Move Up";
      this.btnMoveCategoryUp.Click += new DecentForms.EventHandler(this.btnMoveCategoryUp_Click);
      // 
      // groupAllCategories
      // 
      this.groupAllCategories.Controls.Add(this.btnSortCategories);
      this.groupAllCategories.Location = new System.Drawing.Point(261, 119);
      this.groupAllCategories.Name = "groupAllCategories";
      this.groupAllCategories.Size = new System.Drawing.Size(255, 76);
      this.groupAllCategories.TabIndex = 10;
      this.groupAllCategories.TabStop = false;
      this.groupAllCategories.Text = "All Categories";
      // 
      // btnSortCategories
      // 
      this.btnSortCategories.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnSortCategories.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnSortCategories.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnSortCategories.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnSortCategories.Image = null;
      this.btnSortCategories.Location = new System.Drawing.Point(6, 19);
      this.btnSortCategories.Name = "btnSortCategories";
      this.btnSortCategories.Size = new System.Drawing.Size(105, 23);
      this.btnSortCategories.TabIndex = 3;
      this.btnSortCategories.Text = "Sort by Categories";
      this.btnSortCategories.Click += new DecentForms.EventHandler(this.btnSortCategories_Click);
      // 
      // groupCategorySpecific
      // 
      this.groupCategorySpecific.Controls.Add(this.label5);
      this.groupCategorySpecific.Controls.Add(this.editCollapseIndex);
      this.groupCategorySpecific.Controls.Add(this.btnCollapseCategory);
      this.groupCategorySpecific.Controls.Add(this.btnReseatCategory);
      this.groupCategorySpecific.Location = new System.Drawing.Point(261, 37);
      this.groupCategorySpecific.Name = "groupCategorySpecific";
      this.groupCategorySpecific.Size = new System.Drawing.Size(255, 76);
      this.groupCategorySpecific.TabIndex = 11;
      this.groupCategorySpecific.TabStop = false;
      this.groupCategorySpecific.Text = "Selected Category";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(117, 52);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(47, 13);
      this.label5.TabIndex = 6;
      this.label5.Text = "at index:";
      // 
      // editCollapseIndex
      // 
      this.editCollapseIndex.Location = new System.Drawing.Point(180, 49);
      this.editCollapseIndex.Name = "editCollapseIndex";
      this.editCollapseIndex.Size = new System.Drawing.Size(69, 20);
      this.editCollapseIndex.TabIndex = 5;
      // 
      // btnCollapseCategory
      // 
      this.btnCollapseCategory.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnCollapseCategory.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnCollapseCategory.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnCollapseCategory.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnCollapseCategory.Enabled = false;
      this.btnCollapseCategory.Image = null;
      this.btnCollapseCategory.Location = new System.Drawing.Point(6, 19);
      this.btnCollapseCategory.Name = "btnCollapseCategory";
      this.btnCollapseCategory.Size = new System.Drawing.Size(140, 23);
      this.btnCollapseCategory.TabIndex = 3;
      this.btnCollapseCategory.Text = "Collapse Unique Chars";
      this.btnCollapseCategory.Click += new DecentForms.EventHandler(this.btnCollapseCategory_Click);
      // 
      // btnReseatCategory
      // 
      this.btnReseatCategory.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnReseatCategory.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnReseatCategory.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnReseatCategory.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnReseatCategory.Enabled = false;
      this.btnReseatCategory.Image = null;
      this.btnReseatCategory.Location = new System.Drawing.Point(6, 47);
      this.btnReseatCategory.Name = "btnReseatCategory";
      this.btnReseatCategory.Size = new System.Drawing.Size(105, 23);
      this.btnReseatCategory.TabIndex = 3;
      this.btnReseatCategory.Text = "Reseat Category";
      this.btnReseatCategory.Click += new DecentForms.EventHandler(this.btnReseatCategory_Click);
      // 
      // btnDelete
      // 
      this.btnDelete.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnDelete.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnDelete.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnDelete.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnDelete.Enabled = false;
      this.btnDelete.Image = null;
      this.btnDelete.Location = new System.Drawing.Point(342, 8);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new System.Drawing.Size(96, 23);
      this.btnDelete.TabIndex = 8;
      this.btnDelete.Text = "Delete Category";
      this.btnDelete.Click += new DecentForms.EventHandler(this.btnDelete_Click);
      // 
      // btnAddCategory
      // 
      this.btnAddCategory.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnAddCategory.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnAddCategory.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnAddCategory.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnAddCategory.Enabled = false;
      this.btnAddCategory.Image = null;
      this.btnAddCategory.Location = new System.Drawing.Point(261, 8);
      this.btnAddCategory.Name = "btnAddCategory";
      this.btnAddCategory.Size = new System.Drawing.Size(75, 23);
      this.btnAddCategory.TabIndex = 9;
      this.btnAddCategory.Text = "Add";
      this.btnAddCategory.Click += new DecentForms.EventHandler(this.btnAddCategory_Click);
      // 
      // listCategories
      // 
      this.listCategories.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
      this.listCategories.FullRowSelect = true;
      this.listCategories.HideSelection = false;
      this.listCategories.Location = new System.Drawing.Point(9, 36);
      this.listCategories.Name = "listCategories";
      this.listCategories.ShowGroups = false;
      this.listCategories.Size = new System.Drawing.Size(246, 159);
      this.listCategories.TabIndex = 7;
      this.listCategories.UseCompatibleStateImageBehavior = false;
      this.listCategories.View = System.Windows.Forms.View.Details;
      this.listCategories.SelectedIndexChanged += new System.EventHandler(this.listCategories_SelectedIndexChanged);
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Name";
      this.columnHeader1.Width = 150;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "No. Chars";
      this.columnHeader2.Width = 67;
      // 
      // editCategoryName
      // 
      this.editCategoryName.Location = new System.Drawing.Point(81, 10);
      this.editCategoryName.Name = "editCategoryName";
      this.editCategoryName.Size = new System.Drawing.Size(174, 20);
      this.editCategoryName.TabIndex = 6;
      this.editCategoryName.TextChanged += new System.EventHandler(this.editCategoryName_TextChanged);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(6, 13);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(52, 13);
      this.label3.TabIndex = 5;
      this.label3.Text = "Category:";
      // 
      // btnRemoveDuplicates
      // 
      this.btnRemoveDuplicates.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnRemoveDuplicates.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnRemoveDuplicates.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnRemoveDuplicates.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnRemoveDuplicates.Image = null;
      this.btnRemoveDuplicates.Location = new System.Drawing.Point(617, 333);
      this.btnRemoveDuplicates.Name = "btnRemoveDuplicates";
      this.btnRemoveDuplicates.Size = new System.Drawing.Size(122, 26);
      this.btnRemoveDuplicates.TabIndex = 18;
      this.btnRemoveDuplicates.Text = "Remove Duplicates";
      this.btnRemoveDuplicates.Click += new DecentForms.EventHandler(this.btnRemoveDuplicates_Click);
      // 
      // CharacterEditor
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.Controls.Add(this.tabCharacterEditor);
      this.Name = "CharacterEditor";
      this.Size = new System.Drawing.Size(1070, 490);
      this.tabCharacterEditor.ResumeLayout(false);
      this.tabEditor.ResumeLayout(false);
      this.tabEditor.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.canvasEditor)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picturePlayground)).EndInit();
      this.tabCategories.ResumeLayout(false);
      this.tabCategories.PerformLayout();
      this.groupAllCategories.ResumeLayout(false);
      this.groupCategorySpecific.ResumeLayout(false);
      this.groupCategorySpecific.PerformLayout();
      this.ResumeLayout(false);

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
        private System.Windows.Forms.Label label4;
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
  }
}
