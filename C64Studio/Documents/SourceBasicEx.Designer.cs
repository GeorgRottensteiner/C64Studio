using RetroDevStudio.Controls;

namespace RetroDevStudio.Documents
{
  partial class SourceBasicEx
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SourceBasicEx));
      this.editSource = new FastColoredTextBoxNS.FastColoredTextBox();
      this.contextSource = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
      this.addBookmarkHereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.removeBookmarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.removeAllBookmarksOfThisFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.commentSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.uncommentSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.renumberToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this.btnToggleLabelMode = new DecentForms.MenuButton();
      this.contextMenuLabelButton = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.autoRenumberWithLastValuesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.autoRenumberWith1010ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.autoRenumberWith11ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.autoRenumberSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.menuBASIC = new System.Windows.Forms.MenuStrip();
      this.bASICToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.renumberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.btnToggleSymbolMode = new DecentForms.CheckBox();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.btnToggleUpperLowerCase = new DecentForms.Button();
      this.btnToggleStringEntryMode = new DecentForms.CheckBox();
      this.btnToggleCollapsedTokensMode = new DecentForms.CheckBox();
      this.editBASICStartAddress = new System.Windows.Forms.TextBox();
      this.labelStartAddress = new System.Windows.Forms.Label();
      this.labelBASICVersion = new System.Windows.Forms.Label();
      this.comboBASICVersion = new System.Windows.Forms.ComboBox();
      this.labelCheckSummer = new System.Windows.Forms.Label();
      this.comboCheckSummer = new System.Windows.Forms.ComboBox();
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.editSource)).BeginInit();
      this.contextSource.SuspendLayout();
      this.contextMenuLabelButton.SuspendLayout();
      this.menuBASIC.SuspendLayout();
      this.SuspendLayout();
      // 
      // editSource
      // 
      this.editSource.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.editSource.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
      this.editSource.AutoScrollMinSize = new System.Drawing.Size(25, 14);
      this.editSource.BackBrush = null;
      this.editSource.CharHeight = 14;
      this.editSource.CharWidth = 7;
      this.editSource.ContextMenuStrip = this.contextSource;
      this.editSource.ConvertTabsToSpaces = false;
      this.editSource.Cursor = System.Windows.Forms.Cursors.IBeam;
      this.editSource.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
      this.editSource.Font = new System.Drawing.Font("Consolas", 9F);
      this.editSource.IsReplaceMode = false;
      this.editSource.Location = new System.Drawing.Point(0, 56);
      this.editSource.Name = "editSource";
      this.editSource.Paddings = new System.Windows.Forms.Padding(0);
      this.editSource.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
      this.editSource.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("editSource.ServiceColors")));
      this.editSource.Size = new System.Drawing.Size(858, 532);
      this.editSource.TabIndex = 11;
      this.editSource.Zoom = 100;
      this.editSource.TextChangedDelayed += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.editSource_TextChangedDelayed);
      this.editSource.LineVisited += new System.EventHandler<FastColoredTextBoxNS.LineVisitedArgs>(this.editSource_LineVisited);
      this.editSource.PaintLine += new System.EventHandler<FastColoredTextBoxNS.PaintLineEventArgs>(this.editSource_PaintLine);
      this.editSource.DragDrop += new System.Windows.Forms.DragEventHandler(this.editSource_DragDrop);
      this.editSource.DragEnter += new System.Windows.Forms.DragEventHandler(this.editSource_DragEnter);
      // 
      // contextSource
      // 
      this.contextSource.ImageScalingSize = new System.Drawing.Size(20, 20);
      this.contextSource.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.cutToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripSeparator5,
            this.addBookmarkHereToolStripMenuItem,
            this.removeBookmarkToolStripMenuItem,
            this.removeAllBookmarksOfThisFileToolStripMenuItem,
            this.toolStripSeparator2,
            this.commentSelectionToolStripMenuItem,
            this.uncommentSelectionToolStripMenuItem,
            this.toolStripSeparator1,
            this.renumberToolStripMenuItem1});
      this.contextSource.Name = "contextSource";
      this.contextSource.Size = new System.Drawing.Size(250, 220);
      // 
      // copyToolStripMenuItem
      // 
      this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
      this.copyToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
      this.copyToolStripMenuItem.Text = "&Copy";
      this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
      // 
      // cutToolStripMenuItem
      // 
      this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
      this.cutToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
      this.cutToolStripMenuItem.Text = "C&ut";
      this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
      // 
      // pasteToolStripMenuItem
      // 
      this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
      this.pasteToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
      this.pasteToolStripMenuItem.Text = "&Paste";
      this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
      // 
      // toolStripSeparator5
      // 
      this.toolStripSeparator5.Name = "toolStripSeparator5";
      this.toolStripSeparator5.Size = new System.Drawing.Size(246, 6);
      // 
      // addBookmarkHereToolStripMenuItem
      // 
      this.addBookmarkHereToolStripMenuItem.Name = "addBookmarkHereToolStripMenuItem";
      this.addBookmarkHereToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
      this.addBookmarkHereToolStripMenuItem.Text = "Add Bookmark here";
      this.addBookmarkHereToolStripMenuItem.Click += new System.EventHandler(this.addBookmarkHereToolStripMenuItem_Click);
      // 
      // removeBookmarkToolStripMenuItem
      // 
      this.removeBookmarkToolStripMenuItem.Name = "removeBookmarkToolStripMenuItem";
      this.removeBookmarkToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
      this.removeBookmarkToolStripMenuItem.Text = "Remove Bookmark";
      this.removeBookmarkToolStripMenuItem.Click += new System.EventHandler(this.removeBookmarkToolStripMenuItem_Click);
      // 
      // removeAllBookmarksOfThisFileToolStripMenuItem
      // 
      this.removeAllBookmarksOfThisFileToolStripMenuItem.Name = "removeAllBookmarksOfThisFileToolStripMenuItem";
      this.removeAllBookmarksOfThisFileToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
      this.removeAllBookmarksOfThisFileToolStripMenuItem.Text = "Remove all bookmarks of this file";
      this.removeAllBookmarksOfThisFileToolStripMenuItem.Click += new System.EventHandler(this.removeAllBookmarksOfThisFileToolStripMenuItem_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(246, 6);
      // 
      // commentSelectionToolStripMenuItem
      // 
      this.commentSelectionToolStripMenuItem.Name = "commentSelectionToolStripMenuItem";
      this.commentSelectionToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
      this.commentSelectionToolStripMenuItem.Text = "Comment Selection";
      this.commentSelectionToolStripMenuItem.Click += new System.EventHandler(this.commentSelectionToolStripMenuItem_Click);
      // 
      // uncommentSelectionToolStripMenuItem
      // 
      this.uncommentSelectionToolStripMenuItem.Name = "uncommentSelectionToolStripMenuItem";
      this.uncommentSelectionToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
      this.uncommentSelectionToolStripMenuItem.Text = "Uncomment Selection";
      this.uncommentSelectionToolStripMenuItem.Click += new System.EventHandler(this.uncommentSelectionToolStripMenuItem_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(246, 6);
      // 
      // renumberToolStripMenuItem1
      // 
      this.renumberToolStripMenuItem1.Name = "renumberToolStripMenuItem1";
      this.renumberToolStripMenuItem1.Size = new System.Drawing.Size(249, 22);
      this.renumberToolStripMenuItem1.Text = "Renumber...";
      this.renumberToolStripMenuItem1.Click += new System.EventHandler(this.renumberToolStripMenuItem_Click);
      // 
      // btnToggleLabelMode
      // 
      this.btnToggleLabelMode.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnToggleLabelMode.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnToggleLabelMode.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnToggleLabelMode.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnToggleLabelMode.Image = null;
      this.btnToggleLabelMode.Location = new System.Drawing.Point(0, 27);
      this.btnToggleLabelMode.Menu = this.contextMenuLabelButton;
      this.btnToggleLabelMode.Name = "btnToggleLabelMode";
      this.btnToggleLabelMode.ShowDropDownArrow = false;
      this.btnToggleLabelMode.ShowSplitBar = true;
      this.btnToggleLabelMode.Size = new System.Drawing.Size(130, 23);
      this.btnToggleLabelMode.TabIndex = 0;
      this.btnToggleLabelMode.Text = "To Label Mode";
      this.toolTip1.SetToolTip(this.btnToggleLabelMode, "To Label Mode (Line Number Mode is active)");
      this.btnToggleLabelMode.CheckedChanged += new DecentForms.EventHandler(this.btnToggleLabelMode_CheckedChanged);
      // 
      // contextMenuLabelButton
      // 
      this.contextMenuLabelButton.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoRenumberWithLastValuesToolStripMenuItem,
            this.autoRenumberWith1010ToolStripMenuItem,
            this.autoRenumberWith11ToolStripMenuItem,
            this.autoRenumberSettingsToolStripMenuItem});
      this.contextMenuLabelButton.Name = "contextMenuLabelButton";
      this.contextMenuLabelButton.Size = new System.Drawing.Size(239, 92);
      this.contextMenuLabelButton.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuLabelButton_Opening);
      // 
      // autoRenumberWithLastValuesToolStripMenuItem
      // 
      this.autoRenumberWithLastValuesToolStripMenuItem.Name = "autoRenumberWithLastValuesToolStripMenuItem";
      this.autoRenumberWithLastValuesToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
      this.autoRenumberWithLastValuesToolStripMenuItem.Text = "Auto renumber with last values";
      this.autoRenumberWithLastValuesToolStripMenuItem.Click += new System.EventHandler(this.autoRenumberWithLastValuesToolStripMenuItem_Click);
      // 
      // autoRenumberWith1010ToolStripMenuItem
      // 
      this.autoRenumberWith1010ToolStripMenuItem.Name = "autoRenumberWith1010ToolStripMenuItem";
      this.autoRenumberWith1010ToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
      this.autoRenumberWith1010ToolStripMenuItem.Text = "Auto renumber with 10,10";
      this.autoRenumberWith1010ToolStripMenuItem.Click += new System.EventHandler(this.autoRenumberWith1010ToolStripMenuItem_Click);
      // 
      // autoRenumberWith11ToolStripMenuItem
      // 
      this.autoRenumberWith11ToolStripMenuItem.Name = "autoRenumberWith11ToolStripMenuItem";
      this.autoRenumberWith11ToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
      this.autoRenumberWith11ToolStripMenuItem.Text = "Auto renumber with 1,1";
      this.autoRenumberWith11ToolStripMenuItem.Click += new System.EventHandler(this.autoRenumberWith11ToolStripMenuItem_Click);
      // 
      // autoRenumberSettingsToolStripMenuItem
      // 
      this.autoRenumberSettingsToolStripMenuItem.Name = "autoRenumberSettingsToolStripMenuItem";
      this.autoRenumberSettingsToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
      this.autoRenumberSettingsToolStripMenuItem.Text = "Auto renumber settings...";
      this.autoRenumberSettingsToolStripMenuItem.Click += new System.EventHandler(this.autoRenumberSettingsToolStripMenuItem_Click);
      // 
      // menuBASIC
      // 
      this.menuBASIC.ImageScalingSize = new System.Drawing.Size(20, 20);
      this.menuBASIC.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bASICToolStripMenuItem});
      this.menuBASIC.Location = new System.Drawing.Point(0, 0);
      this.menuBASIC.Name = "menuBASIC";
      this.menuBASIC.Size = new System.Drawing.Size(858, 24);
      this.menuBASIC.TabIndex = 3;
      this.menuBASIC.Text = "menuStrip1";
      // 
      // bASICToolStripMenuItem
      // 
      this.bASICToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renumberToolStripMenuItem});
      this.bASICToolStripMenuItem.Name = "bASICToolStripMenuItem";
      this.bASICToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
      this.bASICToolStripMenuItem.Text = "BASIC";
      // 
      // renumberToolStripMenuItem
      // 
      this.renumberToolStripMenuItem.Name = "renumberToolStripMenuItem";
      this.renumberToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
      this.renumberToolStripMenuItem.Text = "&Renumber...";
      this.renumberToolStripMenuItem.Click += new System.EventHandler(this.renumberToolStripMenuItem_Click);
      // 
      // btnToggleSymbolMode
      // 
      this.btnToggleSymbolMode.Appearance = System.Windows.Forms.Appearance.Button;
      this.btnToggleSymbolMode.BorderStyle = DecentForms.BorderStyle.NONE;
      this.btnToggleSymbolMode.CheckAlign = DecentForms.ContentAlignment.MiddleLeft;
      this.btnToggleSymbolMode.Checked = true;
      this.btnToggleSymbolMode.Image = ((System.Drawing.Image)(resources.GetObject("btnToggleSymbolMode.Image")));
      this.btnToggleSymbolMode.Location = new System.Drawing.Point(136, 27);
      this.btnToggleSymbolMode.Name = "btnToggleSymbolMode";
      this.btnToggleSymbolMode.Size = new System.Drawing.Size(22, 22);
      this.btnToggleSymbolMode.TabIndex = 1;
      this.toolTip1.SetToolTip(this.btnToggleSymbolMode, "Toggle Symbol/Macro");
      this.btnToggleSymbolMode.CheckedChanged += new DecentForms.EventHandler(this.btnToggleSymbolMode_CheckedChanged);
      // 
      // btnToggleUpperLowerCase
      // 
      this.btnToggleUpperLowerCase.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnToggleUpperLowerCase.BorderStyle = DecentForms.BorderStyle.NONE;
      this.btnToggleUpperLowerCase.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnToggleUpperLowerCase.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnToggleUpperLowerCase.Image = ((System.Drawing.Image)(resources.GetObject("btnToggleUpperLowerCase.Image")));
      this.btnToggleUpperLowerCase.Location = new System.Drawing.Point(164, 27);
      this.btnToggleUpperLowerCase.Name = "btnToggleUpperLowerCase";
      this.btnToggleUpperLowerCase.Size = new System.Drawing.Size(22, 22);
      this.btnToggleUpperLowerCase.TabIndex = 2;
      this.toolTip1.SetToolTip(this.btnToggleUpperLowerCase, "Toggle Upper/Lower Case (Currently Upper Case)");
      this.btnToggleUpperLowerCase.Click += new DecentForms.EventHandler(this.btnToggleUpperLowerCase_CheckedChanged);
      // 
      // btnToggleStringEntryMode
      // 
      this.btnToggleStringEntryMode.Appearance = System.Windows.Forms.Appearance.Button;
      this.btnToggleStringEntryMode.BorderStyle = DecentForms.BorderStyle.NONE;
      this.btnToggleStringEntryMode.CheckAlign = DecentForms.ContentAlignment.MiddleLeft;
      this.btnToggleStringEntryMode.Checked = false;
      this.btnToggleStringEntryMode.Image = ((System.Drawing.Image)(resources.GetObject("btnToggleStringEntryMode.Image")));
      this.btnToggleStringEntryMode.Location = new System.Drawing.Point(192, 27);
      this.btnToggleStringEntryMode.Name = "btnToggleStringEntryMode";
      this.btnToggleStringEntryMode.Size = new System.Drawing.Size(22, 22);
      this.btnToggleStringEntryMode.TabIndex = 3;
      this.toolTip1.SetToolTip(this.btnToggleStringEntryMode, "Toggle String Entry Mode (currently inactive)");
      this.btnToggleStringEntryMode.CheckedChanged += new DecentForms.EventHandler(this.btnToggleStringEntryMode_CheckedChanged);
      // 
      // btnToggleCollapsedTokensMode
      // 
      this.btnToggleCollapsedTokensMode.Appearance = System.Windows.Forms.Appearance.Button;
      this.btnToggleCollapsedTokensMode.BorderStyle = DecentForms.BorderStyle.NONE;
      this.btnToggleCollapsedTokensMode.CheckAlign = DecentForms.ContentAlignment.MiddleLeft;
      this.btnToggleCollapsedTokensMode.Checked = false;
      this.btnToggleCollapsedTokensMode.Image = ((System.Drawing.Image)(resources.GetObject("btnToggleCollapsedTokensMode.Image")));
      this.btnToggleCollapsedTokensMode.Location = new System.Drawing.Point(220, 27);
      this.btnToggleCollapsedTokensMode.Name = "btnToggleCollapsedTokensMode";
      this.btnToggleCollapsedTokensMode.Size = new System.Drawing.Size(22, 22);
      this.btnToggleCollapsedTokensMode.TabIndex = 4;
      this.toolTip1.SetToolTip(this.btnToggleCollapsedTokensMode, "Toggle collapsed token mode (currently inactive)");
      this.btnToggleCollapsedTokensMode.CheckedChanged += new DecentForms.EventHandler(this.btnToggleCollapsedTokensMode_CheckedChanged);
      // 
      // editBASICStartAddress
      // 
      this.editBASICStartAddress.Location = new System.Drawing.Point(326, 29);
      this.editBASICStartAddress.MaxLength = 7;
      this.editBASICStartAddress.Name = "editBASICStartAddress";
      this.editBASICStartAddress.Size = new System.Drawing.Size(47, 20);
      this.editBASICStartAddress.TabIndex = 6;
      this.editBASICStartAddress.Text = "2049";
      this.editBASICStartAddress.TextChanged += new System.EventHandler(this.editBASICStartAddress_TextChanged);
      // 
      // labelStartAddress
      // 
      this.labelStartAddress.AutoSize = true;
      this.labelStartAddress.Location = new System.Drawing.Point(247, 32);
      this.labelStartAddress.Name = "labelStartAddress";
      this.labelStartAddress.Size = new System.Drawing.Size(73, 13);
      this.labelStartAddress.TabIndex = 5;
      this.labelStartAddress.Text = "Start Address:";
      // 
      // labelBASICVersion
      // 
      this.labelBASICVersion.AutoSize = true;
      this.labelBASICVersion.Location = new System.Drawing.Point(388, 32);
      this.labelBASICVersion.Name = "labelBASICVersion";
      this.labelBASICVersion.Size = new System.Drawing.Size(79, 13);
      this.labelBASICVersion.TabIndex = 7;
      this.labelBASICVersion.Text = "BASIC Version:";
      // 
      // comboBASICVersion
      // 
      this.comboBASICVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBASICVersion.FormattingEnabled = true;
      this.comboBASICVersion.Location = new System.Drawing.Point(473, 28);
      this.comboBASICVersion.Name = "comboBASICVersion";
      this.comboBASICVersion.Size = new System.Drawing.Size(150, 21);
      this.comboBASICVersion.TabIndex = 8;
      this.comboBASICVersion.SelectedIndexChanged += new System.EventHandler(this.comboBASICVersion_SelectedIndexChanged);
      // 
      // labelCheckSummer
      // 
      this.labelCheckSummer.AutoSize = true;
      this.labelCheckSummer.Location = new System.Drawing.Point(629, 32);
      this.labelCheckSummer.Name = "labelCheckSummer";
      this.labelCheckSummer.Size = new System.Drawing.Size(79, 13);
      this.labelCheckSummer.TabIndex = 9;
      this.labelCheckSummer.Text = "CheckSummer:";
      // 
      // comboCheckSummer
      // 
      this.comboCheckSummer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboCheckSummer.FormattingEnabled = true;
      this.comboCheckSummer.Location = new System.Drawing.Point(714, 28);
      this.comboCheckSummer.Name = "comboCheckSummer";
      this.comboCheckSummer.Size = new System.Drawing.Size(134, 21);
      this.comboCheckSummer.TabIndex = 10;
      this.comboCheckSummer.SelectedIndexChanged += new System.EventHandler(this.comboCheckSummer_SelectedIndexChanged);
      // 
      // SourceBasicEx
      // 
      this.AllowDrop = true;
      this.ClientSize = new System.Drawing.Size(858, 588);
      this.Controls.Add(this.comboCheckSummer);
      this.Controls.Add(this.comboBASICVersion);
      this.Controls.Add(this.labelCheckSummer);
      this.Controls.Add(this.labelBASICVersion);
      this.Controls.Add(this.labelStartAddress);
      this.Controls.Add(this.editBASICStartAddress);
      this.Controls.Add(this.menuBASIC);
      this.Controls.Add(this.editSource);
      this.Controls.Add(this.btnToggleCollapsedTokensMode);
      this.Controls.Add(this.btnToggleStringEntryMode);
      this.Controls.Add(this.btnToggleUpperLowerCase);
      this.Controls.Add(this.btnToggleSymbolMode);
      this.Controls.Add(this.btnToggleLabelMode);
      this.MainMenuStrip = this.menuBASIC;
      this.Name = "SourceBasicEx";
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.editSource)).EndInit();
      this.contextSource.ResumeLayout(false);
      this.contextMenuLabelButton.ResumeLayout(false);
      this.menuBASIC.ResumeLayout(false);
      this.menuBASIC.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    public FastColoredTextBoxNS.FastColoredTextBox    editSource;
    private System.Windows.Forms.ContextMenuStrip contextSource;
    //private System.Windows.Forms.CheckBox btnToggleLabelMode;
    DecentForms.MenuButton btnToggleLabelMode;
    private System.Windows.Forms.MenuStrip menuBASIC;
    private System.Windows.Forms.ToolStripMenuItem bASICToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem renumberToolStripMenuItem;
    private DecentForms.CheckBox btnToggleSymbolMode;
    private System.Windows.Forms.ToolTip toolTip1;
    private DecentForms.Button btnToggleUpperLowerCase;
    private System.Windows.Forms.TextBox editBASICStartAddress;
    private System.Windows.Forms.Label labelStartAddress;
    private System.Windows.Forms.Label labelBASICVersion;
    private System.Windows.Forms.ComboBox comboBASICVersion;
    private DecentForms.CheckBox btnToggleStringEntryMode;
    private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem renumberToolStripMenuItem1;
    private System.Windows.Forms.ContextMenuStrip contextMenuLabelButton;
    private System.Windows.Forms.ToolStripMenuItem autoRenumberWith1010ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem autoRenumberWith11ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem autoRenumberSettingsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem autoRenumberWithLastValuesToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripMenuItem commentSelectionToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem uncommentSelectionToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    private System.Windows.Forms.ToolStripMenuItem addBookmarkHereToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem removeBookmarkToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem removeAllBookmarksOfThisFileToolStripMenuItem;
    private System.Windows.Forms.Label labelCheckSummer;
    private System.Windows.Forms.ComboBox comboCheckSummer;
    private DecentForms.CheckBox btnToggleCollapsedTokensMode;
  }
}
