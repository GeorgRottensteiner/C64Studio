using RetroDevStudio.Controls;



namespace RetroDevStudio.Documents
{
  partial class SpriteEditor
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpriteEditor));
      tabSpriteEditor = new System.Windows.Forms.TabControl();
      tabEditor = new System.Windows.Forms.TabPage();
      editMoveTargetIndex = new System.Windows.Forms.TextBox();
      btnMoveSelectionToTarget = new DecentForms.Button();
      labelSelectionInfo = new System.Windows.Forms.Label();
      btnHighlightDuplicates = new DecentForms.Button();
      btnChangeMode = new DecentForms.MenuButton();
      panelColorSettings = new System.Windows.Forms.Panel();
      btnToolEdit = new DecentForms.RadioButton();
      btnToolFill = new DecentForms.RadioButton();
      label11 = new System.Windows.Forms.Label();
      tabSpriteDetails = new System.Windows.Forms.TabControl();
      tabPage1 = new System.Windows.Forms.TabPage();
      panelSprites = new GR.Forms.ImageListbox();
      tabPage2 = new System.Windows.Forms.TabPage();
      btnSavePreviewToGIF = new DecentForms.Button();
      checkAutoplayAnim = new System.Windows.Forms.CheckBox();
      label9 = new System.Windows.Forms.Label();
      checkExpandY = new System.Windows.Forms.CheckBox();
      checkExpandX = new System.Windows.Forms.CheckBox();
      listLayerSprites = new ArrangedItemList();
      listLayers = new ArrangedItemList();
      editLayerY = new System.Windows.Forms.TextBox();
      label7 = new System.Windows.Forms.Label();
      label4 = new System.Windows.Forms.Label();
      label10 = new System.Windows.Forms.Label();
      label6 = new System.Windows.Forms.Label();
      editLayerDelay = new System.Windows.Forms.TextBox();
      editLayerName = new System.Windows.Forms.TextBox();
      editLayerX = new System.Windows.Forms.TextBox();
      label5 = new System.Windows.Forms.Label();
      label3 = new System.Windows.Forms.Label();
      comboLayerBGColor = new System.Windows.Forms.ComboBox();
      comboLayerColor = new System.Windows.Forms.ComboBox();
      comboSprite = new System.Windows.Forms.ComboBox();
      layerPreview = new GR.Forms.FastPictureBox();
      btnClearSprite = new DecentForms.Button();
      btnDeleteSprite = new DecentForms.Button();
      btnInvert = new DecentForms.Button();
      btnMirrorY = new DecentForms.Button();
      btnMirrorX = new DecentForms.Button();
      btnShiftDown = new DecentForms.Button();
      btnShiftUp = new DecentForms.Button();
      btnShiftRight = new DecentForms.Button();
      btnRotateRight = new DecentForms.Button();
      btnRotateLeft = new DecentForms.Button();
      btnShiftLeft = new DecentForms.Button();
      btnCopyToClipboard = new DecentForms.Button();
      btnPasteFromClipboard = new DecentForms.Button();
      labelCharNo = new System.Windows.Forms.Label();
      checkShowGrid = new System.Windows.Forms.CheckBox();
      pictureEditor = new GR.Forms.FastPictureBox();
      tabExport = new System.Windows.Forms.TabPage();
      labelCharactersTo = new System.Windows.Forms.Label();
      editDataExport = new System.Windows.Forms.TextBox();
      btnExport = new DecentForms.Button();
      comboExportMethod = new System.Windows.Forms.ComboBox();
      label12 = new System.Windows.Forms.Label();
      panelExport = new System.Windows.Forms.Panel();
      comboExportRange = new System.Windows.Forms.ComboBox();
      editSpriteCount = new System.Windows.Forms.TextBox();
      editSpriteFrom = new System.Windows.Forms.TextBox();
      labelCharactersFrom = new System.Windows.Forms.Label();
      tabImport = new System.Windows.Forms.TabPage();
      editImportStartIndex = new System.Windows.Forms.TextBox();
      panelImport = new System.Windows.Forms.Panel();
      btnImport = new DecentForms.Button();
      comboImportMethod = new System.Windows.Forms.ComboBox();
      labelImportSelInfo = new System.Windows.Forms.Label();
      label1 = new System.Windows.Forms.Label();
      label2 = new System.Windows.Forms.Label();
      menuStrip1 = new System.Windows.Forms.MenuStrip();
      fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      openCharsetProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      saveSpriteProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      closeCharsetProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      columnHeader4 = new System.Windows.Forms.ColumnHeader();
      columnHeader5 = new System.Windows.Forms.ColumnHeader();
      columnHeader6 = new System.Windows.Forms.ColumnHeader();
      columnHeader7 = new System.Windows.Forms.ColumnHeader();
      columnHeader8 = new System.Windows.Forms.ColumnHeader();
      columnHeader9 = new System.Windows.Forms.ColumnHeader();
      toolTip1 = new System.Windows.Forms.ToolTip( components );
      contextMenuChangeMode = new System.Windows.Forms.ContextMenuStrip( components );
      c64HiResMultiColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      mega65ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      mega65_24x214ColorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      mega65_64x214ColorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      mega65_16x2116ColorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      commanderX16ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_16ColorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_8x8ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_16x8ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_32x8ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_64x8ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_8x16ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_16x16ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_32x16ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_64x16ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_8x32ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_16x32ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_32x32ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_64x32ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_8x64ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_16x64ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_32x64ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_64x64ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_256ColorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_8x8x256ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_16x8x256ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_32x8x256ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_64x8x256ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_8x16x256ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_16x16x256ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_32x16x256ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_64x16x256ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_8x32x256ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_16x32x256ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_32x32x256ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_64x32x256ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_8x64x256ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_16x64x256ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_32x64x256ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      x16_64x64x256ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      ( (System.ComponentModel.ISupportInitialize)m_FileWatcher ).BeginInit();
      tabSpriteEditor.SuspendLayout();
      tabEditor.SuspendLayout();
      tabSpriteDetails.SuspendLayout();
      tabPage1.SuspendLayout();
      tabPage2.SuspendLayout();
      ( (System.ComponentModel.ISupportInitialize)layerPreview ).BeginInit();
      ( (System.ComponentModel.ISupportInitialize)pictureEditor ).BeginInit();
      tabExport.SuspendLayout();
      tabImport.SuspendLayout();
      menuStrip1.SuspendLayout();
      contextMenuChangeMode.SuspendLayout();
      SuspendLayout();
      // 
      // tabSpriteEditor
      // 
      tabSpriteEditor.Controls.Add( tabEditor );
      tabSpriteEditor.Controls.Add( tabExport );
      tabSpriteEditor.Controls.Add( tabImport );
      tabSpriteEditor.Dock = System.Windows.Forms.DockStyle.Fill;
      tabSpriteEditor.Location = new System.Drawing.Point( 0, 24 );
      tabSpriteEditor.Name = "tabSpriteEditor";
      tabSpriteEditor.SelectedIndex = 0;
      tabSpriteEditor.Size = new System.Drawing.Size( 987, 665 );
      tabSpriteEditor.TabIndex = 0;
      // 
      // tabEditor
      // 
      tabEditor.Controls.Add( editMoveTargetIndex );
      tabEditor.Controls.Add( btnMoveSelectionToTarget );
      tabEditor.Controls.Add( labelSelectionInfo );
      tabEditor.Controls.Add( btnHighlightDuplicates );
      tabEditor.Controls.Add( btnChangeMode );
      tabEditor.Controls.Add( panelColorSettings );
      tabEditor.Controls.Add( btnToolEdit );
      tabEditor.Controls.Add( btnToolFill );
      tabEditor.Controls.Add( label11 );
      tabEditor.Controls.Add( tabSpriteDetails );
      tabEditor.Controls.Add( btnClearSprite );
      tabEditor.Controls.Add( btnDeleteSprite );
      tabEditor.Controls.Add( btnInvert );
      tabEditor.Controls.Add( btnMirrorY );
      tabEditor.Controls.Add( btnMirrorX );
      tabEditor.Controls.Add( btnShiftDown );
      tabEditor.Controls.Add( btnShiftUp );
      tabEditor.Controls.Add( btnShiftRight );
      tabEditor.Controls.Add( btnRotateRight );
      tabEditor.Controls.Add( btnRotateLeft );
      tabEditor.Controls.Add( btnShiftLeft );
      tabEditor.Controls.Add( btnCopyToClipboard );
      tabEditor.Controls.Add( btnPasteFromClipboard );
      tabEditor.Controls.Add( labelCharNo );
      tabEditor.Controls.Add( checkShowGrid );
      tabEditor.Controls.Add( pictureEditor );
      tabEditor.Location = new System.Drawing.Point( 4, 22 );
      tabEditor.Name = "tabEditor";
      tabEditor.Padding = new System.Windows.Forms.Padding( 3 );
      tabEditor.Size = new System.Drawing.Size( 979, 639 );
      tabEditor.TabIndex = 0;
      tabEditor.Text = "Sprite";
      tabEditor.UseVisualStyleBackColor = true;
      // 
      // editMoveTargetIndex
      // 
      editMoveTargetIndex.Location = new System.Drawing.Point( 396, 570 );
      editMoveTargetIndex.Name = "editMoveTargetIndex";
      editMoveTargetIndex.Size = new System.Drawing.Size( 73, 20 );
      editMoveTargetIndex.TabIndex = 0;
      // 
      // btnMoveSelectionToTarget
      // 
      btnMoveSelectionToTarget.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnMoveSelectionToTarget.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnMoveSelectionToTarget.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnMoveSelectionToTarget.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnMoveSelectionToTarget.DisplayAntiAliased = true;
      btnMoveSelectionToTarget.Image = null;
      btnMoveSelectionToTarget.Location = new System.Drawing.Point( 269, 568 );
      btnMoveSelectionToTarget.Name = "btnMoveSelectionToTarget";
      btnMoveSelectionToTarget.Size = new System.Drawing.Size( 121, 23 );
      btnMoveSelectionToTarget.TabIndex = 1;
      btnMoveSelectionToTarget.Text = "Move to Index";
      btnMoveSelectionToTarget.Click +=  btnMoveSelectionToTarget_Click ;
      // 
      // labelSelectionInfo
      // 
      labelSelectionInfo.Location = new System.Drawing.Point( 352, 471 );
      labelSelectionInfo.Name = "labelSelectionInfo";
      labelSelectionInfo.Size = new System.Drawing.Size( 122, 23 );
      labelSelectionInfo.TabIndex = 62;
      labelSelectionInfo.Text = "label1";
      // 
      // btnHighlightDuplicates
      // 
      btnHighlightDuplicates.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnHighlightDuplicates.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnHighlightDuplicates.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnHighlightDuplicates.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnHighlightDuplicates.DisplayAntiAliased = true;
      btnHighlightDuplicates.Image = null;
      btnHighlightDuplicates.Location = new System.Drawing.Point( 269, 539 );
      btnHighlightDuplicates.Name = "btnHighlightDuplicates";
      btnHighlightDuplicates.Size = new System.Drawing.Size( 121, 23 );
      btnHighlightDuplicates.TabIndex = 61;
      btnHighlightDuplicates.Text = "Duplicates";
      btnHighlightDuplicates.Click +=  btnHighlightDuplicates_Click ;
      // 
      // btnChangeMode
      // 
      btnChangeMode.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnChangeMode.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnChangeMode.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnChangeMode.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnChangeMode.DisplayAntiAliased = true;
      btnChangeMode.Image = null;
      btnChangeMode.Location = new System.Drawing.Point( 269, 510 );
      btnChangeMode.Name = "btnChangeMode";
      btnChangeMode.Size = new System.Drawing.Size( 205, 23 );
      btnChangeMode.TabIndex = 60;
      btnChangeMode.Text = "btnChangeMode";
      btnChangeMode.Click +=  btnChangeMode_Click ;
      // 
      // panelColorSettings
      // 
      panelColorSettings.Location = new System.Drawing.Point( 40, 365 );
      panelColorSettings.Name = "panelColorSettings";
      panelColorSettings.Size = new System.Drawing.Size( 220, 210 );
      panelColorSettings.TabIndex = 59;
      // 
      // btnToolEdit
      // 
      btnToolEdit.Appearance = System.Windows.Forms.Appearance.Button;
      btnToolEdit.BorderStyle = DecentForms.BorderStyle.NONE;
      btnToolEdit.CheckAlign = DecentForms.ContentAlignment.MiddleLeft;
      btnToolEdit.Checked = true;
      btnToolEdit.DisplayAntiAliased = true;
      btnToolEdit.Image = (System.Drawing.Image)resources.GetObject( "btnToolEdit.Image" );
      btnToolEdit.Location = new System.Drawing.Point( 8, 285 );
      btnToolEdit.Name = "btnToolEdit";
      btnToolEdit.Size = new System.Drawing.Size( 26, 26 );
      btnToolEdit.TabIndex = 9;
      toolTip1.SetToolTip( btnToolEdit, "Single Character" );
      btnToolEdit.CheckedChanged +=  btnToolEdit_CheckedChanged ;
      // 
      // btnToolFill
      // 
      btnToolFill.Appearance = System.Windows.Forms.Appearance.Button;
      btnToolFill.BorderStyle = DecentForms.BorderStyle.NONE;
      btnToolFill.CheckAlign = DecentForms.ContentAlignment.MiddleLeft;
      btnToolFill.Checked = false;
      btnToolFill.DisplayAntiAliased = true;
      btnToolFill.Image = (System.Drawing.Image)resources.GetObject( "btnToolFill.Image" );
      btnToolFill.Location = new System.Drawing.Point( 8, 316 );
      btnToolFill.Name = "btnToolFill";
      btnToolFill.Size = new System.Drawing.Size( 26, 26 );
      btnToolFill.TabIndex = 10;
      toolTip1.SetToolTip( btnToolFill, "Fill" );
      btnToolFill.CheckedChanged +=  btnToolFill_CheckedChanged ;
      // 
      // label11
      // 
      label11.AutoSize = true;
      label11.Location = new System.Drawing.Point( 266, 494 );
      label11.Name = "label11";
      label11.Size = new System.Drawing.Size( 37, 13 );
      label11.TabIndex = 16;
      label11.Text = "Mode:";
      // 
      // tabSpriteDetails
      // 
      tabSpriteDetails.Anchor =    System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Bottom   |  System.Windows.Forms.AnchorStyles.Left   |  System.Windows.Forms.AnchorStyles.Right ;
      tabSpriteDetails.Controls.Add( tabPage1 );
      tabSpriteDetails.Controls.Add( tabPage2 );
      tabSpriteDetails.Location = new System.Drawing.Point( 480, 2 );
      tabSpriteDetails.Name = "tabSpriteDetails";
      tabSpriteDetails.SelectedIndex = 0;
      tabSpriteDetails.Size = new System.Drawing.Size( 499, 563 );
      tabSpriteDetails.TabIndex = 18;
      // 
      // tabPage1
      // 
      tabPage1.Controls.Add( panelSprites );
      tabPage1.Location = new System.Drawing.Point( 4, 22 );
      tabPage1.Name = "tabPage1";
      tabPage1.Padding = new System.Windows.Forms.Padding( 3 );
      tabPage1.Size = new System.Drawing.Size( 491, 537 );
      tabPage1.TabIndex = 0;
      tabPage1.Text = "Sprites";
      tabPage1.UseVisualStyleBackColor = true;
      // 
      // panelSprites
      // 
      panelSprites.AllowPopup = false;
      panelSprites.AutoScroll = true;
      panelSprites.AutoScrollHorizontalMaximum = 100;
      panelSprites.AutoScrollHorizontalMinimum = 0;
      panelSprites.AutoScrollHPos = 0;
      panelSprites.AutoScrollVerticalMaximum = -23;
      panelSprites.AutoScrollVerticalMinimum = 0;
      panelSprites.AutoScrollVPos = 0;
      panelSprites.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      panelSprites.Dock = System.Windows.Forms.DockStyle.Fill;
      panelSprites.EnableAutoScrollHorizontal = true;
      panelSprites.EnableAutoScrollVertical = true;
      panelSprites.HottrackColor = 2151694591U;
      panelSprites.ItemHeight = 21;
      panelSprites.ItemWidth = 24;
      panelSprites.Location = new System.Drawing.Point( 3, 3 );
      panelSprites.Name = "panelSprites";
      panelSprites.PixelFormat = GR.Drawing.PixelFormat.DontCare;
      panelSprites.Size = new System.Drawing.Size( 485, 531 );
      panelSprites.TabIndex = 4;
      panelSprites.TabStop = true;
      panelSprites.VisibleAutoScrollHorizontal = false;
      panelSprites.VisibleAutoScrollVertical = false;
      panelSprites.SelectedIndexChanged +=  panelSprites_SelectedIndexChanged ;
      panelSprites.SelectionChanged +=  panelSprites_SelectionChanged ;
      panelSprites.ClientSizeChanged +=  panelSprites_ClientSizeChanged ;
      // 
      // tabPage2
      // 
      tabPage2.Controls.Add( btnSavePreviewToGIF );
      tabPage2.Controls.Add( checkAutoplayAnim );
      tabPage2.Controls.Add( label9 );
      tabPage2.Controls.Add( checkExpandY );
      tabPage2.Controls.Add( checkExpandX );
      tabPage2.Controls.Add( listLayerSprites );
      tabPage2.Controls.Add( listLayers );
      tabPage2.Controls.Add( editLayerY );
      tabPage2.Controls.Add( label7 );
      tabPage2.Controls.Add( label4 );
      tabPage2.Controls.Add( label10 );
      tabPage2.Controls.Add( label6 );
      tabPage2.Controls.Add( editLayerDelay );
      tabPage2.Controls.Add( editLayerName );
      tabPage2.Controls.Add( editLayerX );
      tabPage2.Controls.Add( label5 );
      tabPage2.Controls.Add( label3 );
      tabPage2.Controls.Add( comboLayerBGColor );
      tabPage2.Controls.Add( comboLayerColor );
      tabPage2.Controls.Add( comboSprite );
      tabPage2.Controls.Add( layerPreview );
      tabPage2.Location = new System.Drawing.Point( 4, 24 );
      tabPage2.Name = "tabPage2";
      tabPage2.Padding = new System.Windows.Forms.Padding( 3 );
      tabPage2.Size = new System.Drawing.Size( 491, 535 );
      tabPage2.TabIndex = 1;
      tabPage2.Text = "Preview";
      tabPage2.UseVisualStyleBackColor = true;
      // 
      // btnSavePreviewToGIF
      // 
      btnSavePreviewToGIF.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnSavePreviewToGIF.Anchor =  System.Windows.Forms.AnchorStyles.Bottom  |  System.Windows.Forms.AnchorStyles.Left ;
      btnSavePreviewToGIF.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnSavePreviewToGIF.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnSavePreviewToGIF.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnSavePreviewToGIF.DisplayAntiAliased = true;
      btnSavePreviewToGIF.Image = null;
      btnSavePreviewToGIF.Location = new System.Drawing.Point( 389, 436 );
      btnSavePreviewToGIF.Name = "btnSavePreviewToGIF";
      btnSavePreviewToGIF.Size = new System.Drawing.Size( 75, 23 );
      btnSavePreviewToGIF.TabIndex = 13;
      btnSavePreviewToGIF.Text = "Save as GIF";
      btnSavePreviewToGIF.Click +=  btnSavePreviewToGIF_Click ;
      // 
      // checkAutoplayAnim
      // 
      checkAutoplayAnim.Anchor =  System.Windows.Forms.AnchorStyles.Bottom  |  System.Windows.Forms.AnchorStyles.Left ;
      checkAutoplayAnim.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      checkAutoplayAnim.Location = new System.Drawing.Point( 223, 491 );
      checkAutoplayAnim.Name = "checkAutoplayAnim";
      checkAutoplayAnim.Size = new System.Drawing.Size( 157, 24 );
      checkAutoplayAnim.TabIndex = 12;
      checkAutoplayAnim.Text = "Auto-Animation";
      checkAutoplayAnim.UseVisualStyleBackColor = true;
      checkAutoplayAnim.CheckedChanged +=  checkAutoplayAnim_CheckedChanged ;
      // 
      // label9
      // 
      label9.AutoSize = true;
      label9.Location = new System.Drawing.Point( 3, 274 );
      label9.Name = "label9";
      label9.Size = new System.Drawing.Size( 44, 13 );
      label9.TabIndex = 24;
      label9.Text = "Frames:";
      // 
      // checkExpandY
      // 
      checkExpandY.AutoSize = true;
      checkExpandY.Location = new System.Drawing.Point( 149, 180 );
      checkExpandY.Name = "checkExpandY";
      checkExpandY.Size = new System.Drawing.Size( 43, 17 );
      checkExpandY.TabIndex = 3;
      checkExpandY.Text = "Y*2";
      checkExpandY.UseVisualStyleBackColor = true;
      checkExpandY.CheckedChanged +=  checkExpandY_CheckedChanged ;
      // 
      // checkExpandX
      // 
      checkExpandX.AutoSize = true;
      checkExpandX.Location = new System.Drawing.Point( 149, 156 );
      checkExpandX.Name = "checkExpandX";
      checkExpandX.Size = new System.Drawing.Size( 43, 17 );
      checkExpandX.TabIndex = 2;
      checkExpandX.Text = "X*2";
      checkExpandX.UseVisualStyleBackColor = true;
      checkExpandX.CheckedChanged +=  checkExpandX_CheckedChanged ;
      // 
      // listLayerSprites
      // 
      listLayerSprites.AddButtonEnabled = true;
      listLayerSprites.AllowClone = true;
      listLayerSprites.AllowReordering = true;
      listLayerSprites.DeleteButtonEnabled = false;
      listLayerSprites.HasOwnerDrawColumn = true;
      listLayerSprites.HighlightColor = System.Drawing.SystemColors.HotTrack;
      listLayerSprites.HighlightTextColor = System.Drawing.SystemColors.HighlightText;
      listLayerSprites.Location = new System.Drawing.Point( 0, 0 );
      listLayerSprites.MoveDownButtonEnabled = false;
      listLayerSprites.MoveUpButtonEnabled = false;
      listLayerSprites.MustHaveOneElement = false;
      listLayerSprites.Name = "listLayerSprites";
      listLayerSprites.SelectedIndex = -1;
      listLayerSprites.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      listLayerSprites.SelectionTextColor = System.Drawing.SystemColors.HighlightText;
      listLayerSprites.Size = new System.Drawing.Size( 192, 148 );
      listLayerSprites.TabIndex = 0;
      listLayerSprites.AddingItem +=  listLayerSprites_AddingItem ;
      listLayerSprites.CloningItem +=  listLayerSprites_CloningItem ;
      listLayerSprites.ItemAdded +=  listLayerSprites_ItemAdded ;
      listLayerSprites.ItemRemoved +=  listLayerSprites_ItemRemoved ;
      listLayerSprites.MovingItem +=  listLayerSprites_MovingItem ;
      listLayerSprites.ItemMoved +=  listLayerSprites_ItemMoved ;
      listLayerSprites.SelectedIndexChanged +=  listLayerSprites_SelectedIndexChanged ;
      // 
      // listLayers
      // 
      listLayers.AddButtonEnabled = true;
      listLayers.AllowClone = true;
      listLayers.AllowReordering = true;
      listLayers.Anchor =   System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Bottom   |  System.Windows.Forms.AnchorStyles.Left ;
      listLayers.DeleteButtonEnabled = false;
      listLayers.HasOwnerDrawColumn = true;
      listLayers.HighlightColor = System.Drawing.SystemColors.HotTrack;
      listLayers.HighlightTextColor = System.Drawing.SystemColors.HighlightText;
      listLayers.Location = new System.Drawing.Point( 0, 290 );
      listLayers.MoveDownButtonEnabled = false;
      listLayers.MoveUpButtonEnabled = false;
      listLayers.MustHaveOneElement = true;
      listLayers.Name = "listLayers";
      listLayers.SelectedIndex = -1;
      listLayers.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      listLayers.SelectionTextColor = System.Drawing.SystemColors.HighlightText;
      listLayers.Size = new System.Drawing.Size( 192, 239 );
      listLayers.TabIndex = 9;
      listLayers.AddingItem +=  listLayers_AddingItem ;
      listLayers.CloningItem +=  listLayers_CloningItem ;
      listLayers.ItemAdded +=  listLayers_ItemAdded ;
      listLayers.ItemRemoved +=  listLayers_ItemRemoved ;
      listLayers.MovingItem +=  listLayers_MovingItem ;
      listLayers.ItemMoved +=  listLayers_ItemMoved ;
      listLayers.SelectedIndexChanged +=  listLayers_SelectedIndexChanged ;
      // 
      // editLayerY
      // 
      editLayerY.Location = new System.Drawing.Point( 91, 208 );
      editLayerY.Name = "editLayerY";
      editLayerY.Size = new System.Drawing.Size( 45, 20 );
      editLayerY.TabIndex = 5;
      editLayerY.TextChanged +=  editLayerY_TextChanged ;
      editLayerY.KeyPress +=  editLayerY_KeyPress ;
      // 
      // label7
      // 
      label7.AutoSize = true;
      label7.Location = new System.Drawing.Point( 104, 239 );
      label7.Name = "label7";
      label7.Size = new System.Drawing.Size( 25, 13 );
      label7.TabIndex = 8;
      label7.Text = "BG:";
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Location = new System.Drawing.Point( 6, 239 );
      label4.Name = "label4";
      label4.Size = new System.Drawing.Size( 25, 13 );
      label4.TabIndex = 6;
      label4.Text = "Col:";
      // 
      // label10
      // 
      label10.Anchor =  System.Windows.Forms.AnchorStyles.Bottom  |  System.Windows.Forms.AnchorStyles.Left ;
      label10.AutoSize = true;
      label10.Location = new System.Drawing.Point( 224, 468 );
      label10.Name = "label10";
      label10.Size = new System.Drawing.Size( 59, 13 );
      label10.TabIndex = 15;
      label10.Text = "Delay (ms):";
      // 
      // label6
      // 
      label6.Anchor =  System.Windows.Forms.AnchorStyles.Bottom  |  System.Windows.Forms.AnchorStyles.Left ;
      label6.AutoSize = true;
      label6.Location = new System.Drawing.Point( 224, 441 );
      label6.Name = "label6";
      label6.Size = new System.Drawing.Size( 38, 13 );
      label6.TabIndex = 15;
      label6.Text = "Name:";
      // 
      // editLayerDelay
      // 
      editLayerDelay.Anchor =  System.Windows.Forms.AnchorStyles.Bottom  |  System.Windows.Forms.AnchorStyles.Left ;
      editLayerDelay.Location = new System.Drawing.Point( 289, 465 );
      editLayerDelay.Name = "editLayerDelay";
      editLayerDelay.Size = new System.Drawing.Size( 91, 20 );
      editLayerDelay.TabIndex = 11;
      editLayerDelay.TextChanged +=  editLayerDelay_TextChanged ;
      // 
      // editLayerName
      // 
      editLayerName.Anchor =  System.Windows.Forms.AnchorStyles.Bottom  |  System.Windows.Forms.AnchorStyles.Left ;
      editLayerName.Location = new System.Drawing.Point( 289, 438 );
      editLayerName.Name = "editLayerName";
      editLayerName.Size = new System.Drawing.Size( 91, 20 );
      editLayerName.TabIndex = 10;
      editLayerName.TextChanged +=  editLayerName_TextChanged ;
      // 
      // editLayerX
      // 
      editLayerX.Location = new System.Drawing.Point( 39, 208 );
      editLayerX.Name = "editLayerX";
      editLayerX.Size = new System.Drawing.Size( 45, 20 );
      editLayerX.TabIndex = 4;
      editLayerX.TextChanged +=  editLayerX_TextChanged ;
      editLayerX.KeyPress +=  editLayerX_KeyPress ;
      // 
      // label5
      // 
      label5.AutoSize = true;
      label5.Location = new System.Drawing.Point( 6, 170 );
      label5.Name = "label5";
      label5.Size = new System.Drawing.Size( 21, 13 );
      label5.TabIndex = 11;
      label5.Text = "Nr:";
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new System.Drawing.Point( 6, 211 );
      label3.Name = "label3";
      label3.Size = new System.Drawing.Size( 28, 13 );
      label3.TabIndex = 12;
      label3.Text = "Pos:";
      // 
      // comboLayerBGColor
      // 
      comboLayerBGColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      comboLayerBGColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      comboLayerBGColor.FormattingEnabled = true;
      comboLayerBGColor.Location = new System.Drawing.Point( 135, 236 );
      comboLayerBGColor.Name = "comboLayerBGColor";
      comboLayerBGColor.Size = new System.Drawing.Size( 57, 21 );
      comboLayerBGColor.TabIndex = 8;
      comboLayerBGColor.DrawItem +=  comboColor_DrawItem ;
      comboLayerBGColor.SelectedIndexChanged +=  comboLayerBGColor_SelectedIndexChanged ;
      // 
      // comboLayerColor
      // 
      comboLayerColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      comboLayerColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      comboLayerColor.FormattingEnabled = true;
      comboLayerColor.Location = new System.Drawing.Point( 39, 236 );
      comboLayerColor.Name = "comboLayerColor";
      comboLayerColor.Size = new System.Drawing.Size( 59, 21 );
      comboLayerColor.TabIndex = 7;
      comboLayerColor.DrawItem +=  comboColor_DrawItem ;
      comboLayerColor.SelectedIndexChanged +=  comboLayerColor_SelectedIndexChanged ;
      // 
      // comboSprite
      // 
      comboSprite.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      comboSprite.DropDownHeight = 320;
      comboSprite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      comboSprite.FormattingEnabled = true;
      comboSprite.IntegralHeight = false;
      comboSprite.ItemHeight = 42;
      comboSprite.Location = new System.Drawing.Point( 39, 154 );
      comboSprite.Name = "comboSprite";
      comboSprite.Size = new System.Drawing.Size( 97, 48 );
      comboSprite.TabIndex = 1;
      comboSprite.DrawItem +=  comboSprite_DrawItem ;
      comboSprite.SelectedIndexChanged +=  comboSprite_SelectedIndexChanged ;
      // 
      // layerPreview
      // 
      layerPreview.Anchor =    System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Bottom   |  System.Windows.Forms.AnchorStyles.Left   |  System.Windows.Forms.AnchorStyles.Right ;
      layerPreview.AutoResize = false;
      layerPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      layerPreview.Location = new System.Drawing.Point( 208, 3 );
      layerPreview.Name = "layerPreview";
      layerPreview.Size = new System.Drawing.Size( 277, 427 );
      layerPreview.TabIndex = 7;
      layerPreview.TabStop = false;
      layerPreview.SizeChanged +=  layerPreview_SizeChanged ;
      // 
      // btnClearSprite
      // 
      btnClearSprite.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnClearSprite.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnClearSprite.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnClearSprite.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnClearSprite.DisplayAntiAliased = true;
      btnClearSprite.Enabled = false;
      btnClearSprite.Image = null;
      btnClearSprite.Location = new System.Drawing.Point( 269, 445 );
      btnClearSprite.Name = "btnClearSprite";
      btnClearSprite.Size = new System.Drawing.Size( 57, 23 );
      btnClearSprite.TabIndex = 14;
      btnClearSprite.Text = "Clear";
      btnClearSprite.Click +=  btnClearSprite_Click ;
      // 
      // btnDeleteSprite
      // 
      btnDeleteSprite.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnDeleteSprite.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnDeleteSprite.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnDeleteSprite.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnDeleteSprite.DisplayAntiAliased = true;
      btnDeleteSprite.Enabled = false;
      btnDeleteSprite.Image = null;
      btnDeleteSprite.Location = new System.Drawing.Point( 333, 445 );
      btnDeleteSprite.Name = "btnDeleteSprite";
      btnDeleteSprite.Size = new System.Drawing.Size( 57, 23 );
      btnDeleteSprite.TabIndex = 15;
      btnDeleteSprite.Text = "Delete";
      btnDeleteSprite.Click +=  btnDeleteSprite_Click ;
      // 
      // btnInvert
      // 
      btnInvert.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnInvert.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnInvert.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnInvert.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnInvert.DisplayAntiAliased = true;
      btnInvert.Image = (System.Drawing.Image)resources.GetObject( "btnInvert.Image" );
      btnInvert.Location = new System.Drawing.Point( 8, 192 );
      btnInvert.Name = "btnInvert";
      btnInvert.Size = new System.Drawing.Size( 26, 26 );
      btnInvert.TabIndex = 6;
      toolTip1.SetToolTip( btnInvert, "Invert selected sprites colors" );
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
      btnMirrorY.Location = new System.Drawing.Point( 8, 161 );
      btnMirrorY.Name = "btnMirrorY";
      btnMirrorY.Size = new System.Drawing.Size( 26, 26 );
      btnMirrorY.TabIndex = 5;
      toolTip1.SetToolTip( btnMirrorY, "Mirror selected sprites vertically" );
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
      btnMirrorX.Location = new System.Drawing.Point( 8, 130 );
      btnMirrorX.Name = "btnMirrorX";
      btnMirrorX.Size = new System.Drawing.Size( 26, 26 );
      btnMirrorX.TabIndex = 4;
      toolTip1.SetToolTip( btnMirrorX, "Mirror selected sprites horizontally" );
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
      btnShiftDown.Location = new System.Drawing.Point( 8, 99 );
      btnShiftDown.Name = "btnShiftDown";
      btnShiftDown.Size = new System.Drawing.Size( 26, 26 );
      btnShiftDown.TabIndex = 3;
      toolTip1.SetToolTip( btnShiftDown, "Shift selected sprites down" );
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
      btnShiftUp.Location = new System.Drawing.Point( 8, 68 );
      btnShiftUp.Name = "btnShiftUp";
      btnShiftUp.Size = new System.Drawing.Size( 26, 26 );
      btnShiftUp.TabIndex = 2;
      toolTip1.SetToolTip( btnShiftUp, "Shift selected sprites up" );
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
      btnShiftRight.Location = new System.Drawing.Point( 8, 37 );
      btnShiftRight.Name = "btnShiftRight";
      btnShiftRight.Size = new System.Drawing.Size( 26, 26 );
      btnShiftRight.TabIndex = 1;
      toolTip1.SetToolTip( btnShiftRight, "Shift selected sprites right" );
      btnShiftRight.Click +=  btnShiftRight_Click ;
      // 
      // btnRotateRight
      // 
      btnRotateRight.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnRotateRight.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnRotateRight.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnRotateRight.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnRotateRight.DisplayAntiAliased = true;
      btnRotateRight.Image = (System.Drawing.Image)resources.GetObject( "btnRotateRight.Image" );
      btnRotateRight.Location = new System.Drawing.Point( 8, 254 );
      btnRotateRight.Name = "btnRotateRight";
      btnRotateRight.Size = new System.Drawing.Size( 26, 26 );
      btnRotateRight.TabIndex = 8;
      toolTip1.SetToolTip( btnRotateRight, "Rotate selected sprites right" );
      btnRotateRight.Click +=  btnRotateRight_Click ;
      // 
      // btnRotateLeft
      // 
      btnRotateLeft.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnRotateLeft.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnRotateLeft.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnRotateLeft.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnRotateLeft.DisplayAntiAliased = true;
      btnRotateLeft.Image = (System.Drawing.Image)resources.GetObject( "btnRotateLeft.Image" );
      btnRotateLeft.Location = new System.Drawing.Point( 8, 223 );
      btnRotateLeft.Name = "btnRotateLeft";
      btnRotateLeft.Size = new System.Drawing.Size( 26, 26 );
      btnRotateLeft.TabIndex = 7;
      toolTip1.SetToolTip( btnRotateLeft, "Rotate selected sprites left" );
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
      btnShiftLeft.Location = new System.Drawing.Point( 8, 6 );
      btnShiftLeft.Name = "btnShiftLeft";
      btnShiftLeft.Size = new System.Drawing.Size( 26, 26 );
      btnShiftLeft.TabIndex = 0;
      toolTip1.SetToolTip( btnShiftLeft, "Shift selected sprites left" );
      btnShiftLeft.Click +=  btnShiftLeft_Click ;
      // 
      // btnCopyToClipboard
      // 
      btnCopyToClipboard.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnCopyToClipboard.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnCopyToClipboard.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnCopyToClipboard.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnCopyToClipboard.DisplayAntiAliased = true;
      btnCopyToClipboard.Enabled = false;
      btnCopyToClipboard.Image = null;
      btnCopyToClipboard.Location = new System.Drawing.Point( 269, 416 );
      btnCopyToClipboard.Name = "btnCopyToClipboard";
      btnCopyToClipboard.Size = new System.Drawing.Size( 121, 23 );
      btnCopyToClipboard.TabIndex = 13;
      btnCopyToClipboard.Text = "Copy to Clipboard";
      btnCopyToClipboard.Click +=  btnCopyToClipboard_Click ;
      // 
      // btnPasteFromClipboard
      // 
      btnPasteFromClipboard.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnPasteFromClipboard.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnPasteFromClipboard.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnPasteFromClipboard.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnPasteFromClipboard.DisplayAntiAliased = true;
      btnPasteFromClipboard.Enabled = false;
      btnPasteFromClipboard.Image = null;
      btnPasteFromClipboard.Location = new System.Drawing.Point( 269, 387 );
      btnPasteFromClipboard.Name = "btnPasteFromClipboard";
      btnPasteFromClipboard.Size = new System.Drawing.Size( 121, 23 );
      btnPasteFromClipboard.TabIndex = 12;
      btnPasteFromClipboard.Text = "Paste from Clipboard";
      btnPasteFromClipboard.Click +=  btnPasteFromClipboard_Click ;
      // 
      // labelCharNo
      // 
      labelCharNo.Location = new System.Drawing.Point( 266, 471 );
      labelCharNo.Name = "labelCharNo";
      labelCharNo.Size = new System.Drawing.Size( 82, 23 );
      labelCharNo.TabIndex = 16;
      labelCharNo.Text = "label1";
      // 
      // checkShowGrid
      // 
      checkShowGrid.AutoSize = true;
      checkShowGrid.Location = new System.Drawing.Point( 269, 365 );
      checkShowGrid.Name = "checkShowGrid";
      checkShowGrid.Size = new System.Drawing.Size( 75, 17 );
      checkShowGrid.TabIndex = 11;
      checkShowGrid.Text = "Show Grid";
      checkShowGrid.UseVisualStyleBackColor = true;
      checkShowGrid.CheckedChanged +=  checkShowGrid_CheckedChanged ;
      // 
      // pictureEditor
      // 
      pictureEditor.AutoResize = false;
      pictureEditor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      pictureEditor.Location = new System.Drawing.Point( 40, 6 );
      pictureEditor.Name = "pictureEditor";
      pictureEditor.Size = new System.Drawing.Size( 434, 353 );
      pictureEditor.TabIndex = 0;
      pictureEditor.TabStop = false;
      pictureEditor.MouseDown +=  pictureEditor_MouseDown ;
      pictureEditor.MouseMove +=  pictureEditor_MouseMove ;
      // 
      // tabExport
      // 
      tabExport.Controls.Add( labelCharactersTo );
      tabExport.Controls.Add( editDataExport );
      tabExport.Controls.Add( btnExport );
      tabExport.Controls.Add( comboExportMethod );
      tabExport.Controls.Add( label12 );
      tabExport.Controls.Add( panelExport );
      tabExport.Controls.Add( comboExportRange );
      tabExport.Controls.Add( editSpriteCount );
      tabExport.Controls.Add( editSpriteFrom );
      tabExport.Controls.Add( labelCharactersFrom );
      tabExport.Location = new System.Drawing.Point( 4, 22 );
      tabExport.Name = "tabExport";
      tabExport.Padding = new System.Windows.Forms.Padding( 3 );
      tabExport.Size = new System.Drawing.Size( 979, 639 );
      tabExport.TabIndex = 2;
      tabExport.Text = "Export";
      tabExport.UseVisualStyleBackColor = true;
      // 
      // labelCharactersTo
      // 
      labelCharactersTo.AutoSize = true;
      labelCharactersTo.Location = new System.Drawing.Point( 188, 9 );
      labelCharactersTo.Name = "labelCharactersTo";
      labelCharactersTo.Size = new System.Drawing.Size( 37, 13 );
      labelCharactersTo.TabIndex = 34;
      labelCharactersTo.Text = "count:";
      // 
      // editDataExport
      // 
      editDataExport.Anchor =    System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Bottom   |  System.Windows.Forms.AnchorStyles.Left   |  System.Windows.Forms.AnchorStyles.Right ;
      editDataExport.Location = new System.Drawing.Point( 314, 7 );
      editDataExport.Multiline = true;
      editDataExport.Name = "editDataExport";
      editDataExport.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      editDataExport.Size = new System.Drawing.Size( 657, 626 );
      editDataExport.TabIndex = 33;
      editDataExport.WordWrap = false;
      // 
      // btnExport
      // 
      btnExport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnExport.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnExport.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnExport.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnExport.DisplayAntiAliased = true;
      btnExport.Image = null;
      btnExport.Location = new System.Drawing.Point( 247, 33 );
      btnExport.Name = "btnExport";
      btnExport.Size = new System.Drawing.Size( 61, 21 );
      btnExport.TabIndex = 32;
      btnExport.Text = "Export";
      btnExport.Click +=  btnExport_Click ;
      // 
      // comboExportMethod
      // 
      comboExportMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      comboExportMethod.FormattingEnabled = true;
      comboExportMethod.Location = new System.Drawing.Point( 90, 34 );
      comboExportMethod.Name = "comboExportMethod";
      comboExportMethod.Size = new System.Drawing.Size( 151, 21 );
      comboExportMethod.TabIndex = 30;
      comboExportMethod.SelectedIndexChanged +=  comboExportMethod_SelectedIndexChanged ;
      // 
      // label12
      // 
      label12.AutoSize = true;
      label12.Location = new System.Drawing.Point( 8, 37 );
      label12.Name = "label12";
      label12.Size = new System.Drawing.Size( 79, 13 );
      label12.TabIndex = 31;
      label12.Text = "Export Method:";
      // 
      // panelExport
      // 
      panelExport.Anchor =   System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Bottom   |  System.Windows.Forms.AnchorStyles.Left ;
      panelExport.Location = new System.Drawing.Point( 6, 61 );
      panelExport.Name = "panelExport";
      panelExport.Size = new System.Drawing.Size( 302, 572 );
      panelExport.TabIndex = 28;
      // 
      // comboExportRange
      // 
      comboExportRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      comboExportRange.FormattingEnabled = true;
      comboExportRange.Location = new System.Drawing.Point( 8, 6 );
      comboExportRange.Name = "comboExportRange";
      comboExportRange.Size = new System.Drawing.Size( 88, 21 );
      comboExportRange.TabIndex = 18;
      comboExportRange.SelectedIndexChanged +=  comboExportRange_SelectedIndexChanged ;
      // 
      // editSpriteCount
      // 
      editSpriteCount.Location = new System.Drawing.Point( 231, 6 );
      editSpriteCount.Name = "editSpriteCount";
      editSpriteCount.Size = new System.Drawing.Size( 36, 20 );
      editSpriteCount.TabIndex = 17;
      // 
      // editSpriteFrom
      // 
      editSpriteFrom.Location = new System.Drawing.Point( 141, 6 );
      editSpriteFrom.Name = "editSpriteFrom";
      editSpriteFrom.Size = new System.Drawing.Size( 39, 20 );
      editSpriteFrom.TabIndex = 16;
      // 
      // labelCharactersFrom
      // 
      labelCharactersFrom.AutoSize = true;
      labelCharactersFrom.Location = new System.Drawing.Point( 105, 9 );
      labelCharactersFrom.Name = "labelCharactersFrom";
      labelCharactersFrom.Size = new System.Drawing.Size( 30, 13 );
      labelCharactersFrom.TabIndex = 15;
      labelCharactersFrom.Text = "from:";
      // 
      // tabImport
      // 
      tabImport.Controls.Add( editImportStartIndex );
      tabImport.Controls.Add( panelImport );
      tabImport.Controls.Add( btnImport );
      tabImport.Controls.Add( comboImportMethod );
      tabImport.Controls.Add( labelImportSelInfo );
      tabImport.Controls.Add( label1 );
      tabImport.Controls.Add( label2 );
      tabImport.Location = new System.Drawing.Point( 4, 22 );
      tabImport.Name = "tabImport";
      tabImport.Padding = new System.Windows.Forms.Padding( 3 );
      tabImport.Size = new System.Drawing.Size( 979, 639 );
      tabImport.TabIndex = 3;
      tabImport.Text = "Import";
      tabImport.UseVisualStyleBackColor = true;
      // 
      // editImportStartIndex
      // 
      editImportStartIndex.Location = new System.Drawing.Point( 526, 6 );
      editImportStartIndex.Name = "editImportStartIndex";
      editImportStartIndex.Size = new System.Drawing.Size( 100, 20 );
      editImportStartIndex.TabIndex = 2;
      editImportStartIndex.Text = "0";
      editImportStartIndex.TextChanged +=  editImportStartIndex_TextChanged ;
      // 
      // panelImport
      // 
      panelImport.Anchor =    System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Bottom   |  System.Windows.Forms.AnchorStyles.Left   |  System.Windows.Forms.AnchorStyles.Right ;
      panelImport.Location = new System.Drawing.Point( 6, 33 );
      panelImport.Name = "panelImport";
      panelImport.Size = new System.Drawing.Size( 965, 600 );
      panelImport.TabIndex = 3;
      // 
      // btnImport
      // 
      btnImport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnImport.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnImport.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnImport.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnImport.DisplayAntiAliased = true;
      btnImport.Image = null;
      btnImport.Location = new System.Drawing.Point( 336, 5 );
      btnImport.Name = "btnImport";
      btnImport.Size = new System.Drawing.Size( 75, 21 );
      btnImport.TabIndex = 1;
      btnImport.Text = "Import";
      btnImport.Click +=  btnImport_Click ;
      // 
      // comboImportMethod
      // 
      comboImportMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      comboImportMethod.FormattingEnabled = true;
      comboImportMethod.Location = new System.Drawing.Point( 87, 6 );
      comboImportMethod.Name = "comboImportMethod";
      comboImportMethod.Size = new System.Drawing.Size( 243, 21 );
      comboImportMethod.TabIndex = 0;
      comboImportMethod.SelectedIndexChanged +=  comboImportMethod_SelectedIndexChanged ;
      // 
      // labelImportSelInfo
      // 
      labelImportSelInfo.Location = new System.Drawing.Point( 643, 9 );
      labelImportSelInfo.Name = "labelImportSelInfo";
      labelImportSelInfo.Size = new System.Drawing.Size( 172, 13 );
      labelImportSelInfo.TabIndex = 38;
      labelImportSelInfo.Text = "Current selection start at: 0";
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new System.Drawing.Point( 429, 9 );
      label1.Name = "label1";
      label1.Size = new System.Drawing.Size( 91, 13 );
      label1.TabIndex = 38;
      label1.Text = "Import start Index:";
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new System.Drawing.Point( 6, 9 );
      label2.Name = "label2";
      label2.Size = new System.Drawing.Size( 78, 13 );
      label2.TabIndex = 38;
      label2.Text = "Import Method:";
      // 
      // menuStrip1
      // 
      menuStrip1.ImageScalingSize = new System.Drawing.Size( 28, 28 );
      menuStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem } );
      menuStrip1.Location = new System.Drawing.Point( 0, 0 );
      menuStrip1.Name = "menuStrip1";
      menuStrip1.Size = new System.Drawing.Size( 987, 24 );
      menuStrip1.TabIndex = 1;
      menuStrip1.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      fileToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] { openCharsetProjectToolStripMenuItem, saveSpriteProjectToolStripMenuItem, closeCharsetProjectToolStripMenuItem } );
      fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      fileToolStripMenuItem.Size = new System.Drawing.Size( 54, 20 );
      fileToolStripMenuItem.Text = "&Sprites";
      // 
      // openCharsetProjectToolStripMenuItem
      // 
      openCharsetProjectToolStripMenuItem.Name = "openCharsetProjectToolStripMenuItem";
      openCharsetProjectToolStripMenuItem.Size = new System.Drawing.Size( 185, 22 );
      openCharsetProjectToolStripMenuItem.Text = "&Open Sprite Project...";
      openCharsetProjectToolStripMenuItem.Click +=  openToolStripMenuItem_Click ;
      // 
      // saveSpriteProjectToolStripMenuItem
      // 
      saveSpriteProjectToolStripMenuItem.Name = "saveSpriteProjectToolStripMenuItem";
      saveSpriteProjectToolStripMenuItem.Size = new System.Drawing.Size( 185, 22 );
      saveSpriteProjectToolStripMenuItem.Text = "&Save Project";
      saveSpriteProjectToolStripMenuItem.Click +=  saveCharsetProjectToolStripMenuItem_Click ;
      // 
      // closeCharsetProjectToolStripMenuItem
      // 
      closeCharsetProjectToolStripMenuItem.Enabled = false;
      closeCharsetProjectToolStripMenuItem.Name = "closeCharsetProjectToolStripMenuItem";
      closeCharsetProjectToolStripMenuItem.Size = new System.Drawing.Size( 185, 22 );
      closeCharsetProjectToolStripMenuItem.Text = "&Close Sprite Project";
      closeCharsetProjectToolStripMenuItem.Click +=  closeCharsetProjectToolStripMenuItem_Click ;
      // 
      // columnHeader4
      // 
      columnHeader4.Text = "Nr.";
      // 
      // columnHeader5
      // 
      columnHeader5.Text = "X";
      columnHeader5.Width = 30;
      // 
      // columnHeader6
      // 
      columnHeader6.Text = "Y";
      columnHeader6.Width = 30;
      // 
      // contextMenuChangeMode
      // 
      contextMenuChangeMode.Items.AddRange( new System.Windows.Forms.ToolStripItem[] { c64HiResMultiColorToolStripMenuItem, mega65ToolStripMenuItem, commanderX16ToolStripMenuItem } );
      contextMenuChangeMode.Name = "contextMenuChangeMode";
      contextMenuChangeMode.Size = new System.Drawing.Size( 222, 70 );
      // 
      // c64HiResMultiColorToolStripMenuItem
      // 
      c64HiResMultiColorToolStripMenuItem.Name = "c64HiResMultiColorToolStripMenuItem";
      c64HiResMultiColorToolStripMenuItem.Size = new System.Drawing.Size( 221, 22 );
      c64HiResMultiColorToolStripMenuItem.Text = "C64 HiRes/MultiColor 24x21";
      c64HiResMultiColorToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // mega65ToolStripMenuItem
      // 
      mega65ToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] { mega65_24x214ColorsToolStripMenuItem, mega65_64x214ColorsToolStripMenuItem, mega65_16x2116ColorsToolStripMenuItem } );
      mega65ToolStripMenuItem.Name = "mega65ToolStripMenuItem";
      mega65ToolStripMenuItem.Size = new System.Drawing.Size( 221, 22 );
      mega65ToolStripMenuItem.Text = "Mega65";
      // 
      // mega65_24x214ColorsToolStripMenuItem
      // 
      mega65_24x214ColorsToolStripMenuItem.Name = "mega65_24x214ColorsToolStripMenuItem";
      mega65_24x214ColorsToolStripMenuItem.Size = new System.Drawing.Size( 155, 22 );
      mega65_24x214ColorsToolStripMenuItem.Text = "24x21 4 Colors";
      mega65_24x214ColorsToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // mega65_64x214ColorsToolStripMenuItem
      // 
      mega65_64x214ColorsToolStripMenuItem.Name = "mega65_64x214ColorsToolStripMenuItem";
      mega65_64x214ColorsToolStripMenuItem.Size = new System.Drawing.Size( 155, 22 );
      mega65_64x214ColorsToolStripMenuItem.Text = "64x21 4 Colors";
      mega65_64x214ColorsToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // mega65_16x2116ColorsToolStripMenuItem
      // 
      mega65_16x2116ColorsToolStripMenuItem.Name = "mega65_16x2116ColorsToolStripMenuItem";
      mega65_16x2116ColorsToolStripMenuItem.Size = new System.Drawing.Size( 155, 22 );
      mega65_16x2116ColorsToolStripMenuItem.Text = "16x21 16 Colors";
      mega65_16x2116ColorsToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // commanderX16ToolStripMenuItem
      // 
      commanderX16ToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] { x16_16ColorsToolStripMenuItem, x16_256ColorsToolStripMenuItem } );
      commanderX16ToolStripMenuItem.Name = "commanderX16ToolStripMenuItem";
      commanderX16ToolStripMenuItem.Size = new System.Drawing.Size( 221, 22 );
      commanderX16ToolStripMenuItem.Text = "Commander X16";
      // 
      // x16_16ColorsToolStripMenuItem
      // 
      x16_16ColorsToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] { x16_8x8ToolStripMenuItem, x16_16x8ToolStripMenuItem, x16_32x8ToolStripMenuItem, x16_64x8ToolStripMenuItem, x16_8x16ToolStripMenuItem, x16_16x16ToolStripMenuItem, x16_32x16ToolStripMenuItem, x16_64x16ToolStripMenuItem, x16_8x32ToolStripMenuItem, x16_16x32ToolStripMenuItem, x16_32x32ToolStripMenuItem, x16_64x32ToolStripMenuItem, x16_8x64ToolStripMenuItem, x16_16x64ToolStripMenuItem, x16_32x64ToolStripMenuItem, x16_64x64ToolStripMenuItem } );
      x16_16ColorsToolStripMenuItem.Name = "x16_16ColorsToolStripMenuItem";
      x16_16ColorsToolStripMenuItem.Size = new System.Drawing.Size( 129, 22 );
      x16_16ColorsToolStripMenuItem.Text = "16 Colors";
      // 
      // x16_8x8ToolStripMenuItem
      // 
      x16_8x8ToolStripMenuItem.Name = "x16_8x8ToolStripMenuItem";
      x16_8x8ToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
      x16_8x8ToolStripMenuItem.Text = "8x8";
      x16_8x8ToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // x16_16x8ToolStripMenuItem
      // 
      x16_16x8ToolStripMenuItem.Name = "x16_16x8ToolStripMenuItem";
      x16_16x8ToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
      x16_16x8ToolStripMenuItem.Text = "16x8";
      x16_16x8ToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // x16_32x8ToolStripMenuItem
      // 
      x16_32x8ToolStripMenuItem.Name = "x16_32x8ToolStripMenuItem";
      x16_32x8ToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
      x16_32x8ToolStripMenuItem.Text = "32x8";
      x16_32x8ToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // x16_64x8ToolStripMenuItem
      // 
      x16_64x8ToolStripMenuItem.Name = "x16_64x8ToolStripMenuItem";
      x16_64x8ToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
      x16_64x8ToolStripMenuItem.Text = "64x8";
      x16_64x8ToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // x16_8x16ToolStripMenuItem
      // 
      x16_8x16ToolStripMenuItem.Name = "x16_8x16ToolStripMenuItem";
      x16_8x16ToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
      x16_8x16ToolStripMenuItem.Text = "8x16";
      x16_8x16ToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // x16_16x16ToolStripMenuItem
      // 
      x16_16x16ToolStripMenuItem.Name = "x16_16x16ToolStripMenuItem";
      x16_16x16ToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
      x16_16x16ToolStripMenuItem.Text = "16x16";
      x16_16x16ToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // x16_32x16ToolStripMenuItem
      // 
      x16_32x16ToolStripMenuItem.Name = "x16_32x16ToolStripMenuItem";
      x16_32x16ToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
      x16_32x16ToolStripMenuItem.Text = "32x16";
      x16_32x16ToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // x16_64x16ToolStripMenuItem
      // 
      x16_64x16ToolStripMenuItem.Name = "x16_64x16ToolStripMenuItem";
      x16_64x16ToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
      x16_64x16ToolStripMenuItem.Text = "64x16";
      x16_64x16ToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // x16_8x32ToolStripMenuItem
      // 
      x16_8x32ToolStripMenuItem.Name = "x16_8x32ToolStripMenuItem";
      x16_8x32ToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
      x16_8x32ToolStripMenuItem.Text = "8x32";
      x16_8x32ToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // x16_16x32ToolStripMenuItem
      // 
      x16_16x32ToolStripMenuItem.Name = "x16_16x32ToolStripMenuItem";
      x16_16x32ToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
      x16_16x32ToolStripMenuItem.Text = "16x32";
      x16_16x32ToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // x16_32x32ToolStripMenuItem
      // 
      x16_32x32ToolStripMenuItem.Name = "x16_32x32ToolStripMenuItem";
      x16_32x32ToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
      x16_32x32ToolStripMenuItem.Text = "32x32";
      x16_32x32ToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // x16_64x32ToolStripMenuItem
      // 
      x16_64x32ToolStripMenuItem.Name = "x16_64x32ToolStripMenuItem";
      x16_64x32ToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
      x16_64x32ToolStripMenuItem.Text = "64x32";
      x16_64x32ToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // x16_8x64ToolStripMenuItem
      // 
      x16_8x64ToolStripMenuItem.Name = "x16_8x64ToolStripMenuItem";
      x16_8x64ToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
      x16_8x64ToolStripMenuItem.Text = "8x64";
      x16_8x64ToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // x16_16x64ToolStripMenuItem
      // 
      x16_16x64ToolStripMenuItem.Name = "x16_16x64ToolStripMenuItem";
      x16_16x64ToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
      x16_16x64ToolStripMenuItem.Text = "16x64";
      x16_16x64ToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // x16_32x64ToolStripMenuItem
      // 
      x16_32x64ToolStripMenuItem.Name = "x16_32x64ToolStripMenuItem";
      x16_32x64ToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
      x16_32x64ToolStripMenuItem.Text = "32x64";
      x16_32x64ToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // x16_64x64ToolStripMenuItem
      // 
      x16_64x64ToolStripMenuItem.Name = "x16_64x64ToolStripMenuItem";
      x16_64x64ToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
      x16_64x64ToolStripMenuItem.Text = "64x64";
      x16_64x64ToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // x16_256ColorsToolStripMenuItem
      // 
      x16_256ColorsToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] { x16_8x8x256ToolStripMenuItem, x16_16x8x256ToolStripMenuItem, x16_32x8x256ToolStripMenuItem, x16_64x8x256ToolStripMenuItem, x16_8x16x256ToolStripMenuItem, x16_16x16x256ToolStripMenuItem, x16_32x16x256ToolStripMenuItem, x16_64x16x256ToolStripMenuItem, x16_8x32x256ToolStripMenuItem, x16_16x32x256ToolStripMenuItem, x16_32x32x256ToolStripMenuItem, x16_64x32x256ToolStripMenuItem, x16_8x64x256ToolStripMenuItem, x16_16x64x256ToolStripMenuItem, x16_32x64x256ToolStripMenuItem, x16_64x64x256ToolStripMenuItem } );
      x16_256ColorsToolStripMenuItem.Name = "x16_256ColorsToolStripMenuItem";
      x16_256ColorsToolStripMenuItem.Size = new System.Drawing.Size( 129, 22 );
      x16_256ColorsToolStripMenuItem.Text = "256 Colors";
      // 
      // x16_8x8x256ToolStripMenuItem
      // 
      x16_8x8x256ToolStripMenuItem.Name = "x16_8x8x256ToolStripMenuItem";
      x16_8x8x256ToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
      x16_8x8x256ToolStripMenuItem.Text = "8x8";
      x16_8x8x256ToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // x16_16x8x256ToolStripMenuItem
      // 
      x16_16x8x256ToolStripMenuItem.Name = "x16_16x8x256ToolStripMenuItem";
      x16_16x8x256ToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
      x16_16x8x256ToolStripMenuItem.Text = "16x8";
      x16_16x8x256ToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // x16_32x8x256ToolStripMenuItem
      // 
      x16_32x8x256ToolStripMenuItem.Name = "x16_32x8x256ToolStripMenuItem";
      x16_32x8x256ToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
      x16_32x8x256ToolStripMenuItem.Text = "32x8";
      x16_32x8x256ToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // x16_64x8x256ToolStripMenuItem
      // 
      x16_64x8x256ToolStripMenuItem.Name = "x16_64x8x256ToolStripMenuItem";
      x16_64x8x256ToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
      x16_64x8x256ToolStripMenuItem.Text = "64x8";
      x16_64x8x256ToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // x16_8x16x256ToolStripMenuItem
      // 
      x16_8x16x256ToolStripMenuItem.Name = "x16_8x16x256ToolStripMenuItem";
      x16_8x16x256ToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
      x16_8x16x256ToolStripMenuItem.Text = "8x16";
      x16_8x16x256ToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // x16_16x16x256ToolStripMenuItem
      // 
      x16_16x16x256ToolStripMenuItem.Name = "x16_16x16x256ToolStripMenuItem";
      x16_16x16x256ToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
      x16_16x16x256ToolStripMenuItem.Text = "16x16";
      x16_16x16x256ToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // x16_32x16x256ToolStripMenuItem
      // 
      x16_32x16x256ToolStripMenuItem.Name = "x16_32x16x256ToolStripMenuItem";
      x16_32x16x256ToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
      x16_32x16x256ToolStripMenuItem.Text = "32x16";
      x16_32x16x256ToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // x16_64x16x256ToolStripMenuItem
      // 
      x16_64x16x256ToolStripMenuItem.Name = "x16_64x16x256ToolStripMenuItem";
      x16_64x16x256ToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
      x16_64x16x256ToolStripMenuItem.Text = "64x16";
      x16_64x16x256ToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // x16_8x32x256ToolStripMenuItem
      // 
      x16_8x32x256ToolStripMenuItem.Name = "x16_8x32x256ToolStripMenuItem";
      x16_8x32x256ToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
      x16_8x32x256ToolStripMenuItem.Text = "8x32";
      x16_8x32x256ToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // x16_16x32x256ToolStripMenuItem
      // 
      x16_16x32x256ToolStripMenuItem.Name = "x16_16x32x256ToolStripMenuItem";
      x16_16x32x256ToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
      x16_16x32x256ToolStripMenuItem.Text = "16x32";
      x16_16x32x256ToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // x16_32x32x256ToolStripMenuItem
      // 
      x16_32x32x256ToolStripMenuItem.Name = "x16_32x32x256ToolStripMenuItem";
      x16_32x32x256ToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
      x16_32x32x256ToolStripMenuItem.Text = "32x32";
      x16_32x32x256ToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // x16_64x32x256ToolStripMenuItem
      // 
      x16_64x32x256ToolStripMenuItem.Name = "x16_64x32x256ToolStripMenuItem";
      x16_64x32x256ToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
      x16_64x32x256ToolStripMenuItem.Text = "64x32";
      x16_64x32x256ToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // x16_8x64x256ToolStripMenuItem
      // 
      x16_8x64x256ToolStripMenuItem.Name = "x16_8x64x256ToolStripMenuItem";
      x16_8x64x256ToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
      x16_8x64x256ToolStripMenuItem.Text = "8x64";
      x16_8x64x256ToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // x16_16x64x256ToolStripMenuItem
      // 
      x16_16x64x256ToolStripMenuItem.Name = "x16_16x64x256ToolStripMenuItem";
      x16_16x64x256ToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
      x16_16x64x256ToolStripMenuItem.Text = "16x64";
      x16_16x64x256ToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // x16_32x64x256ToolStripMenuItem
      // 
      x16_32x64x256ToolStripMenuItem.Name = "x16_32x64x256ToolStripMenuItem";
      x16_32x64x256ToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
      x16_32x64x256ToolStripMenuItem.Text = "32x64";
      x16_32x64x256ToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // x16_64x64x256ToolStripMenuItem
      // 
      x16_64x64x256ToolStripMenuItem.Name = "x16_64x64x256ToolStripMenuItem";
      x16_64x64x256ToolStripMenuItem.Size = new System.Drawing.Size( 103, 22 );
      x16_64x64x256ToolStripMenuItem.Text = "64x64";
      x16_64x64x256ToolStripMenuItem.Click +=  spriteModeChangedMenuItem_Click ;
      // 
      // SpriteEditor
      // 
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      ClientSize = new System.Drawing.Size( 987, 689 );
      Controls.Add( tabSpriteEditor );
      Controls.Add( menuStrip1 );
      Icon = (System.Drawing.Icon)resources.GetObject( "$this.Icon" );
      Name = "SpriteEditor";
      Text = "Sprite Editor";
      ( (System.ComponentModel.ISupportInitialize)m_FileWatcher ).EndInit();
      tabSpriteEditor.ResumeLayout( false );
      tabEditor.ResumeLayout( false );
      tabEditor.PerformLayout();
      tabSpriteDetails.ResumeLayout( false );
      tabPage1.ResumeLayout( false );
      tabPage2.ResumeLayout( false );
      tabPage2.PerformLayout();
      ( (System.ComponentModel.ISupportInitialize)layerPreview ).EndInit();
      ( (System.ComponentModel.ISupportInitialize)pictureEditor ).EndInit();
      tabExport.ResumeLayout( false );
      tabExport.PerformLayout();
      tabImport.ResumeLayout( false );
      tabImport.PerformLayout();
      menuStrip1.ResumeLayout( false );
      menuStrip1.PerformLayout();
      contextMenuChangeMode.ResumeLayout( false );
      ResumeLayout( false );
      PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TabControl tabSpriteEditor;
    private System.Windows.Forms.TabPage tabEditor;
    private GR.Forms.FastPictureBox pictureEditor;
    private GR.Forms.ImageListbox panelSprites;
    private System.Windows.Forms.Label labelCharNo;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem openCharsetProjectToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem closeCharsetProjectToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveSpriteProjectToolStripMenuItem;
    private DecentForms.Button btnCopyToClipboard;
    private DecentForms.Button btnPasteFromClipboard;
    private DecentForms.Button btnShiftLeft;
    private DecentForms.Button btnShiftDown;
    private DecentForms.Button btnShiftUp;
    private DecentForms.Button btnShiftRight;
    private DecentForms.Button btnMirrorX;
    private DecentForms.Button btnMirrorY;
    private System.Windows.Forms.CheckBox checkShowGrid;
    private DecentForms.Button btnInvert;
    private DecentForms.Button btnRotateLeft;
    private DecentForms.Button btnRotateRight;
    private DecentForms.Button btnDeleteSprite;
    private System.Windows.Forms.TabControl tabSpriteDetails;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.TabPage tabPage2;
    private System.Windows.Forms.TextBox editLayerY;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TextBox editLayerX;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.ComboBox comboLayerBGColor;
    private System.Windows.Forms.ComboBox comboLayerColor;
    private System.Windows.Forms.ComboBox comboSprite;
    private GR.Forms.FastPictureBox layerPreview;
    private ArrangedItemList listLayers;
    private ArrangedItemList listLayerSprites;
    private System.Windows.Forms.ColumnHeader columnHeader4;
    private System.Windows.Forms.ColumnHeader columnHeader5;
    private System.Windows.Forms.ColumnHeader columnHeader6;
    private System.Windows.Forms.ColumnHeader columnHeader7;
    private System.Windows.Forms.ColumnHeader columnHeader8;
    private System.Windows.Forms.ColumnHeader columnHeader9;
    private System.Windows.Forms.TextBox editLayerName;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.CheckBox checkExpandY;
    private System.Windows.Forms.CheckBox checkExpandX;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.TextBox editLayerDelay;
    private System.Windows.Forms.CheckBox checkAutoplayAnim;
    private DecentForms.Button btnSavePreviewToGIF;
    private System.Windows.Forms.Label label11;
    private DecentForms.RadioButton btnToolFill;
    private DecentForms.RadioButton btnToolEdit;
    private System.Windows.Forms.Panel panelColorSettings;
    private DecentForms.Button btnClearSprite;
    private System.Windows.Forms.TabPage tabExport;
    private System.Windows.Forms.ComboBox comboExportRange;
    private System.Windows.Forms.TextBox editSpriteCount;
    private System.Windows.Forms.TextBox editSpriteFrom;
    private System.Windows.Forms.Label labelCharactersFrom;
    private System.Windows.Forms.Panel panelExport;
    private DecentForms.Button btnExport;
    private System.Windows.Forms.ComboBox comboExportMethod;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.TextBox editDataExport;
    private System.Windows.Forms.Label labelCharactersTo;
    private System.Windows.Forms.TabPage tabImport;
    private DecentForms.Button btnImport;
    private System.Windows.Forms.ComboBox comboImportMethod;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Panel panelImport;
        private DecentForms.MenuButton btnChangeMode;
        private System.Windows.Forms.ContextMenuStrip contextMenuChangeMode;
        private System.Windows.Forms.ToolStripMenuItem c64HiResMultiColorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mega65ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem commanderX16ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x16_16ColorsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x16_8x8ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x16_16x8ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x16_32x8ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x16_64x8ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x16_8x16ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x16_16x16ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x16_32x16ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x16_64x16ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x16_8x32ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x16_16x32ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x16_32x32ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x16_64x32ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x16_8x64ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x16_16x64ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x16_32x64ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x16_64x64ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x16_256ColorsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem x16_8x8x256ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem x16_16x8x256ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem x16_32x8x256ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem x16_64x8x256ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem x16_8x16x256ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem x16_16x16x256ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem x16_32x16x256ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem x16_64x16x256ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem x16_8x32x256ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem x16_16x32x256ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem x16_32x32x256ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem x16_64x32x256ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem x16_8x64x256ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem x16_16x64x256ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem x16_32x64x256ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem x16_64x64x256ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem mega65_24x214ColorsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem mega65_64x214ColorsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem mega65_16x2116ColorsToolStripMenuItem;
    private DecentForms.Button btnHighlightDuplicates;
    private System.Windows.Forms.Label labelSelectionInfo;
    private System.Windows.Forms.TextBox editMoveTargetIndex;
    private DecentForms.Button btnMoveSelectionToTarget;
    private System.Windows.Forms.TextBox editImportStartIndex;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label labelImportSelInfo;
  }
}
