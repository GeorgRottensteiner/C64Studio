namespace RetroDevStudio
{
  partial class FormFindReplace
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.tabFindReplace = new System.Windows.Forms.TabControl();
      this.tabSearch = new System.Windows.Forms.TabPage();
      this.btnSearchBookmark = new System.Windows.Forms.Button();
      this.btnFindNext = new System.Windows.Forms.Button();
      this.btnFindAll = new System.Windows.Forms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.radioSearchDirDown = new System.Windows.Forms.RadioButton();
      this.radioSearchDirUp = new System.Windows.Forms.RadioButton();
      this.checkSearchWrap = new System.Windows.Forms.CheckBox();
      this.checkSearchRegExp = new System.Windows.Forms.CheckBox();
      this.checkSearchFullWords = new System.Windows.Forms.CheckBox();
      this.checkSearchIgnoreCase = new System.Windows.Forms.CheckBox();
      this.comboSearchTarget = new System.Windows.Forms.ComboBox();
      this.comboSearchText = new System.Windows.Forms.ComboBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.tabReplace = new System.Windows.Forms.TabPage();
      this.btnReplaceAll = new System.Windows.Forms.Button();
      this.btnReplaceFindNext = new System.Windows.Forms.Button();
      this.btnReplaceNext = new System.Windows.Forms.Button();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.radioReplaceSearchDown = new System.Windows.Forms.RadioButton();
      this.radioReplaceSearchUp = new System.Windows.Forms.RadioButton();
      this.checkReplaceWrap = new System.Windows.Forms.CheckBox();
      this.checkReplaceRegexp = new System.Windows.Forms.CheckBox();
      this.checkReplaceWholeWords = new System.Windows.Forms.CheckBox();
      this.checkReplaceIgnoreCase = new System.Windows.Forms.CheckBox();
      this.comboReplaceTarget = new System.Windows.Forms.ComboBox();
      this.comboReplaceWith = new System.Windows.Forms.ComboBox();
      this.comboReplaceSearchText = new System.Windows.Forms.ComboBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      this.tabFindReplace.SuspendLayout();
      this.tabSearch.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.tabReplace.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabFindReplace
      // 
      this.tabFindReplace.Controls.Add(this.tabSearch);
      this.tabFindReplace.Controls.Add(this.tabReplace);
      this.tabFindReplace.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabFindReplace.Location = new System.Drawing.Point(0, 0);
      this.tabFindReplace.Name = "tabFindReplace";
      this.tabFindReplace.SelectedIndex = 0;
      this.tabFindReplace.Size = new System.Drawing.Size(350, 294);
      this.tabFindReplace.TabIndex = 0;
      this.tabFindReplace.SelectedIndexChanged += new System.EventHandler(this.tabFindReplace_SelectedIndexChanged);
      // 
      // tabSearch
      // 
      this.tabSearch.Controls.Add(this.btnSearchBookmark);
      this.tabSearch.Controls.Add(this.btnFindNext);
      this.tabSearch.Controls.Add(this.btnFindAll);
      this.tabSearch.Controls.Add(this.groupBox1);
      this.tabSearch.Controls.Add(this.comboSearchTarget);
      this.tabSearch.Controls.Add(this.comboSearchText);
      this.tabSearch.Controls.Add(this.label2);
      this.tabSearch.Controls.Add(this.label1);
      this.tabSearch.Location = new System.Drawing.Point(4, 22);
      this.tabSearch.Name = "tabSearch";
      this.tabSearch.Padding = new System.Windows.Forms.Padding(3);
      this.tabSearch.Size = new System.Drawing.Size(342, 268);
      this.tabSearch.TabIndex = 0;
      this.tabSearch.Text = "Search";
      this.tabSearch.UseVisualStyleBackColor = true;
      // 
      // btnSearchBookmark
      // 
      this.btnSearchBookmark.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnSearchBookmark.Location = new System.Drawing.Point(257, 198);
      this.btnSearchBookmark.Name = "btnSearchBookmark";
      this.btnSearchBookmark.Size = new System.Drawing.Size(75, 23);
      this.btnSearchBookmark.TabIndex = 3;
      this.btnSearchBookmark.Text = "Bookmark";
      this.btnSearchBookmark.UseVisualStyleBackColor = true;
      this.btnSearchBookmark.Click += new System.EventHandler(this.btnSearchBookmark_Click);
      // 
      // btnFindNext
      // 
      this.btnFindNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnFindNext.Location = new System.Drawing.Point(95, 198);
      this.btnFindNext.Name = "btnFindNext";
      this.btnFindNext.Size = new System.Drawing.Size(75, 23);
      this.btnFindNext.TabIndex = 2;
      this.btnFindNext.Text = "Find Next";
      this.btnFindNext.UseVisualStyleBackColor = true;
      this.btnFindNext.Click += new System.EventHandler(this.btnFindNext_Click);
      // 
      // btnFindAll
      // 
      this.btnFindAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnFindAll.Location = new System.Drawing.Point(176, 198);
      this.btnFindAll.Name = "btnFindAll";
      this.btnFindAll.Size = new System.Drawing.Size(75, 23);
      this.btnFindAll.TabIndex = 2;
      this.btnFindAll.Text = "Find All";
      this.btnFindAll.UseVisualStyleBackColor = true;
      this.btnFindAll.Click += new System.EventHandler(this.btnFindAll_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox1.Controls.Add(this.radioSearchDirDown);
      this.groupBox1.Controls.Add(this.radioSearchDirUp);
      this.groupBox1.Controls.Add(this.checkSearchWrap);
      this.groupBox1.Controls.Add(this.checkSearchRegExp);
      this.groupBox1.Controls.Add(this.checkSearchFullWords);
      this.groupBox1.Controls.Add(this.checkSearchIgnoreCase);
      this.groupBox1.Location = new System.Drawing.Point(6, 86);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(326, 106);
      this.groupBox1.TabIndex = 2;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Search Options";
      // 
      // radioSearchDirDown
      // 
      this.radioSearchDirDown.AutoSize = true;
      this.radioSearchDirDown.Checked = true;
      this.radioSearchDirDown.Location = new System.Drawing.Point(159, 39);
      this.radioSearchDirDown.Name = "radioSearchDirDown";
      this.radioSearchDirDown.Size = new System.Drawing.Size(118, 17);
      this.radioSearchDirDown.TabIndex = 5;
      this.radioSearchDirDown.TabStop = true;
      this.radioSearchDirDown.Text = "Search Downwards";
      this.radioSearchDirDown.UseVisualStyleBackColor = true;
      this.radioSearchDirDown.Visible = false;
      this.radioSearchDirDown.CheckedChanged += new System.EventHandler(this.radioSearchDirDown_CheckedChanged);
      // 
      // radioSearchDirUp
      // 
      this.radioSearchDirUp.AutoSize = true;
      this.radioSearchDirUp.Location = new System.Drawing.Point(159, 19);
      this.radioSearchDirUp.Name = "radioSearchDirUp";
      this.radioSearchDirUp.Size = new System.Drawing.Size(104, 17);
      this.radioSearchDirUp.TabIndex = 4;
      this.radioSearchDirUp.TabStop = true;
      this.radioSearchDirUp.Text = "Search Upwards";
      this.radioSearchDirUp.UseVisualStyleBackColor = true;
      this.radioSearchDirUp.Visible = false;
      this.radioSearchDirUp.CheckedChanged += new System.EventHandler(this.radioSearchDirUp_CheckedChanged);
      // 
      // checkSearchWrap
      // 
      this.checkSearchWrap.AutoSize = true;
      this.checkSearchWrap.Checked = true;
      this.checkSearchWrap.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkSearchWrap.Location = new System.Drawing.Point(6, 83);
      this.checkSearchWrap.Name = "checkSearchWrap";
      this.checkSearchWrap.Size = new System.Drawing.Size(52, 17);
      this.checkSearchWrap.TabIndex = 3;
      this.checkSearchWrap.Text = "Wrap";
      this.checkSearchWrap.UseVisualStyleBackColor = true;
      this.checkSearchWrap.CheckedChanged += new System.EventHandler(this.checkSearchWrap_CheckedChanged);
      // 
      // checkSearchRegExp
      // 
      this.checkSearchRegExp.AutoSize = true;
      this.checkSearchRegExp.Location = new System.Drawing.Point(6, 61);
      this.checkSearchRegExp.Name = "checkSearchRegExp";
      this.checkSearchRegExp.Size = new System.Drawing.Size(117, 17);
      this.checkSearchRegExp.TabIndex = 2;
      this.checkSearchRegExp.Text = "Regular Expression";
      this.checkSearchRegExp.UseVisualStyleBackColor = true;
      this.checkSearchRegExp.CheckedChanged += new System.EventHandler(this.checkSearchRegExp_CheckedChanged);
      // 
      // checkSearchFullWords
      // 
      this.checkSearchFullWords.AutoSize = true;
      this.checkSearchFullWords.Location = new System.Drawing.Point(6, 40);
      this.checkSearchFullWords.Name = "checkSearchFullWords";
      this.checkSearchFullWords.Size = new System.Drawing.Size(95, 17);
      this.checkSearchFullWords.TabIndex = 1;
      this.checkSearchFullWords.Text = "Full words only";
      this.checkSearchFullWords.UseVisualStyleBackColor = true;
      this.checkSearchFullWords.CheckedChanged += new System.EventHandler(this.checkSearchFullWords_CheckedChanged);
      // 
      // checkSearchIgnoreCase
      // 
      this.checkSearchIgnoreCase.AutoSize = true;
      this.checkSearchIgnoreCase.Checked = true;
      this.checkSearchIgnoreCase.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkSearchIgnoreCase.Location = new System.Drawing.Point(6, 19);
      this.checkSearchIgnoreCase.Name = "checkSearchIgnoreCase";
      this.checkSearchIgnoreCase.Size = new System.Drawing.Size(83, 17);
      this.checkSearchIgnoreCase.TabIndex = 0;
      this.checkSearchIgnoreCase.Text = "Ignore Case";
      this.checkSearchIgnoreCase.UseVisualStyleBackColor = true;
      this.checkSearchIgnoreCase.CheckedChanged += new System.EventHandler(this.checkSearchIgnoreCase_CheckedChanged);
      // 
      // comboSearchTarget
      // 
      this.comboSearchTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.comboSearchTarget.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboSearchTarget.FormattingEnabled = true;
      this.comboSearchTarget.Location = new System.Drawing.Point(6, 59);
      this.comboSearchTarget.Name = "comboSearchTarget";
      this.comboSearchTarget.Size = new System.Drawing.Size(328, 21);
      this.comboSearchTarget.TabIndex = 1;
      // 
      // comboSearchText
      // 
      this.comboSearchText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.comboSearchText.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.comboSearchText.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.comboSearchText.FormattingEnabled = true;
      this.comboSearchText.Location = new System.Drawing.Point(6, 19);
      this.comboSearchText.Name = "comboSearchText";
      this.comboSearchText.Size = new System.Drawing.Size(328, 21);
      this.comboSearchText.TabIndex = 0;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(3, 43);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(55, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "Search in:";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(3, 3);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(59, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Search for:";
      // 
      // tabReplace
      // 
      this.tabReplace.Controls.Add(this.btnReplaceAll);
      this.tabReplace.Controls.Add(this.btnReplaceFindNext);
      this.tabReplace.Controls.Add(this.btnReplaceNext);
      this.tabReplace.Controls.Add(this.groupBox2);
      this.tabReplace.Controls.Add(this.comboReplaceTarget);
      this.tabReplace.Controls.Add(this.comboReplaceWith);
      this.tabReplace.Controls.Add(this.comboReplaceSearchText);
      this.tabReplace.Controls.Add(this.label3);
      this.tabReplace.Controls.Add(this.label5);
      this.tabReplace.Controls.Add(this.label4);
      this.tabReplace.Location = new System.Drawing.Point(4, 22);
      this.tabReplace.Name = "tabReplace";
      this.tabReplace.Padding = new System.Windows.Forms.Padding(3);
      this.tabReplace.Size = new System.Drawing.Size(342, 268);
      this.tabReplace.TabIndex = 1;
      this.tabReplace.Text = "Replace";
      this.tabReplace.UseVisualStyleBackColor = true;
      // 
      // btnReplaceAll
      // 
      this.btnReplaceAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnReplaceAll.Location = new System.Drawing.Point(257, 238);
      this.btnReplaceAll.Name = "btnReplaceAll";
      this.btnReplaceAll.Size = new System.Drawing.Size(75, 23);
      this.btnReplaceAll.TabIndex = 5;
      this.btnReplaceAll.Text = "Replace All";
      this.btnReplaceAll.UseVisualStyleBackColor = true;
      this.btnReplaceAll.Click += new System.EventHandler(this.btnReplaceAll_Click);
      // 
      // btnReplaceFindNext
      // 
      this.btnReplaceFindNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnReplaceFindNext.Location = new System.Drawing.Point(93, 238);
      this.btnReplaceFindNext.Name = "btnReplaceFindNext";
      this.btnReplaceFindNext.Size = new System.Drawing.Size(75, 23);
      this.btnReplaceFindNext.TabIndex = 3;
      this.btnReplaceFindNext.Text = "Find Next";
      this.btnReplaceFindNext.UseVisualStyleBackColor = true;
      this.btnReplaceFindNext.Click += new System.EventHandler(this.btnReplaceFindNext_Click);
      // 
      // btnReplaceNext
      // 
      this.btnReplaceNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnReplaceNext.Location = new System.Drawing.Point(174, 238);
      this.btnReplaceNext.Name = "btnReplaceNext";
      this.btnReplaceNext.Size = new System.Drawing.Size(75, 23);
      this.btnReplaceNext.TabIndex = 4;
      this.btnReplaceNext.Text = "Replace";
      this.btnReplaceNext.UseVisualStyleBackColor = true;
      this.btnReplaceNext.Click += new System.EventHandler(this.btnReplaceNext_Click);
      // 
      // groupBox2
      // 
      this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox2.Controls.Add(this.radioReplaceSearchDown);
      this.groupBox2.Controls.Add(this.radioReplaceSearchUp);
      this.groupBox2.Controls.Add(this.checkReplaceWrap);
      this.groupBox2.Controls.Add(this.checkReplaceRegexp);
      this.groupBox2.Controls.Add(this.checkReplaceWholeWords);
      this.groupBox2.Controls.Add(this.checkReplaceIgnoreCase);
      this.groupBox2.Location = new System.Drawing.Point(6, 126);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(326, 106);
      this.groupBox2.TabIndex = 7;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Search Options";
      // 
      // radioReplaceSearchDown
      // 
      this.radioReplaceSearchDown.AutoSize = true;
      this.radioReplaceSearchDown.Checked = true;
      this.radioReplaceSearchDown.Location = new System.Drawing.Point(159, 39);
      this.radioReplaceSearchDown.Name = "radioReplaceSearchDown";
      this.radioReplaceSearchDown.Size = new System.Drawing.Size(118, 17);
      this.radioReplaceSearchDown.TabIndex = 5;
      this.radioReplaceSearchDown.TabStop = true;
      this.radioReplaceSearchDown.Text = "Search Downwards";
      this.radioReplaceSearchDown.UseVisualStyleBackColor = true;
      this.radioReplaceSearchDown.Visible = false;
      this.radioReplaceSearchDown.CheckedChanged += new System.EventHandler(this.radioReplaceSearchDown_CheckedChanged);
      // 
      // radioReplaceSearchUp
      // 
      this.radioReplaceSearchUp.AutoSize = true;
      this.radioReplaceSearchUp.Location = new System.Drawing.Point(159, 19);
      this.radioReplaceSearchUp.Name = "radioReplaceSearchUp";
      this.radioReplaceSearchUp.Size = new System.Drawing.Size(104, 17);
      this.radioReplaceSearchUp.TabIndex = 4;
      this.radioReplaceSearchUp.TabStop = true;
      this.radioReplaceSearchUp.Text = "Search Upwards";
      this.radioReplaceSearchUp.UseVisualStyleBackColor = true;
      this.radioReplaceSearchUp.Visible = false;
      this.radioReplaceSearchUp.CheckedChanged += new System.EventHandler(this.radioReplaceSearchUp_CheckedChanged);
      // 
      // checkReplaceWrap
      // 
      this.checkReplaceWrap.AutoSize = true;
      this.checkReplaceWrap.Checked = true;
      this.checkReplaceWrap.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkReplaceWrap.Location = new System.Drawing.Point(6, 83);
      this.checkReplaceWrap.Name = "checkReplaceWrap";
      this.checkReplaceWrap.Size = new System.Drawing.Size(52, 17);
      this.checkReplaceWrap.TabIndex = 3;
      this.checkReplaceWrap.Text = "Wrap";
      this.checkReplaceWrap.UseVisualStyleBackColor = true;
      this.checkReplaceWrap.CheckedChanged += new System.EventHandler(this.checkReplaceWrap_CheckedChanged);
      // 
      // checkReplaceRegexp
      // 
      this.checkReplaceRegexp.AutoSize = true;
      this.checkReplaceRegexp.Location = new System.Drawing.Point(6, 61);
      this.checkReplaceRegexp.Name = "checkReplaceRegexp";
      this.checkReplaceRegexp.Size = new System.Drawing.Size(117, 17);
      this.checkReplaceRegexp.TabIndex = 2;
      this.checkReplaceRegexp.Text = "Regular Expression";
      this.checkReplaceRegexp.UseVisualStyleBackColor = true;
      this.checkReplaceRegexp.CheckedChanged += new System.EventHandler(this.checkReplaceRegexp_CheckedChanged);
      // 
      // checkReplaceWholeWords
      // 
      this.checkReplaceWholeWords.AutoSize = true;
      this.checkReplaceWholeWords.Location = new System.Drawing.Point(6, 40);
      this.checkReplaceWholeWords.Name = "checkReplaceWholeWords";
      this.checkReplaceWholeWords.Size = new System.Drawing.Size(95, 17);
      this.checkReplaceWholeWords.TabIndex = 1;
      this.checkReplaceWholeWords.Text = "Full words only";
      this.checkReplaceWholeWords.UseVisualStyleBackColor = true;
      this.checkReplaceWholeWords.CheckedChanged += new System.EventHandler(this.checkReplaceWholeWords_CheckedChanged);
      // 
      // checkReplaceIgnoreCase
      // 
      this.checkReplaceIgnoreCase.AutoSize = true;
      this.checkReplaceIgnoreCase.Checked = true;
      this.checkReplaceIgnoreCase.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkReplaceIgnoreCase.Location = new System.Drawing.Point(6, 19);
      this.checkReplaceIgnoreCase.Name = "checkReplaceIgnoreCase";
      this.checkReplaceIgnoreCase.Size = new System.Drawing.Size(83, 17);
      this.checkReplaceIgnoreCase.TabIndex = 0;
      this.checkReplaceIgnoreCase.Text = "Ignore Case";
      this.checkReplaceIgnoreCase.UseVisualStyleBackColor = true;
      this.checkReplaceIgnoreCase.CheckedChanged += new System.EventHandler(this.checkReplaceIgnoreCase_CheckedChanged);
      // 
      // comboReplaceTarget
      // 
      this.comboReplaceTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.comboReplaceTarget.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboReplaceTarget.FormattingEnabled = true;
      this.comboReplaceTarget.Location = new System.Drawing.Point(6, 99);
      this.comboReplaceTarget.Name = "comboReplaceTarget";
      this.comboReplaceTarget.Size = new System.Drawing.Size(328, 21);
      this.comboReplaceTarget.TabIndex = 2;
      this.comboReplaceTarget.SelectedIndexChanged += new System.EventHandler(this.comboReplaceTarget_SelectedIndexChanged);
      // 
      // comboReplaceWith
      // 
      this.comboReplaceWith.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.comboReplaceWith.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.comboReplaceWith.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.comboReplaceWith.FormattingEnabled = true;
      this.comboReplaceWith.Location = new System.Drawing.Point(6, 59);
      this.comboReplaceWith.Name = "comboReplaceWith";
      this.comboReplaceWith.Size = new System.Drawing.Size(328, 21);
      this.comboReplaceWith.TabIndex = 1;
      // 
      // comboReplaceSearchText
      // 
      this.comboReplaceSearchText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.comboReplaceSearchText.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.comboReplaceSearchText.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.comboReplaceSearchText.FormattingEnabled = true;
      this.comboReplaceSearchText.Location = new System.Drawing.Point(6, 19);
      this.comboReplaceSearchText.Name = "comboReplaceSearchText";
      this.comboReplaceSearchText.Size = new System.Drawing.Size(328, 21);
      this.comboReplaceSearchText.TabIndex = 0;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(3, 83);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(55, 13);
      this.label3.TabIndex = 3;
      this.label3.Text = "Search in:";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(3, 43);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(72, 13);
      this.label5.TabIndex = 4;
      this.label5.Text = "Replace with:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(3, 3);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(59, 13);
      this.label4.TabIndex = 4;
      this.label4.Text = "Search for:";
      // 
      // FormFindReplace
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.ClientSize = new System.Drawing.Size(350, 294);
      this.Controls.Add(this.tabFindReplace);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
      this.HideOnClose = true;
      this.KeyPreview = true;
      this.Name = "FormFindReplace";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "Find/Replace";
      this.VisibleChanged += new System.EventHandler(this.FormFindReplace_VisibleChanged);
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormFindReplace_KeyDown);
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      this.tabFindReplace.ResumeLayout(false);
      this.tabSearch.ResumeLayout(false);
      this.tabSearch.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.tabReplace.ResumeLayout(false);
      this.tabReplace.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabPage tabSearch;
    private System.Windows.Forms.TabPage tabReplace;
    public System.Windows.Forms.TabControl tabFindReplace;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.RadioButton radioSearchDirUp;
    private System.Windows.Forms.CheckBox checkSearchRegExp;
    private System.Windows.Forms.CheckBox checkSearchFullWords;
    private System.Windows.Forms.CheckBox checkSearchIgnoreCase;
    public System.Windows.Forms.ComboBox comboSearchTarget;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.RadioButton radioSearchDirDown;
    private System.Windows.Forms.Button btnSearchBookmark;
    private System.Windows.Forms.CheckBox checkSearchWrap;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.RadioButton radioReplaceSearchDown;
    private System.Windows.Forms.RadioButton radioReplaceSearchUp;
    private System.Windows.Forms.CheckBox checkReplaceWrap;
    private System.Windows.Forms.CheckBox checkReplaceRegexp;
    private System.Windows.Forms.CheckBox checkReplaceWholeWords;
    private System.Windows.Forms.CheckBox checkReplaceIgnoreCase;
    private System.Windows.Forms.ComboBox comboReplaceWith;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Button btnReplaceAll;
    private System.Windows.Forms.Button btnReplaceFindNext;
    private System.Windows.Forms.Button btnReplaceNext;
    public System.Windows.Forms.Button btnFindNext;
    public System.Windows.Forms.ComboBox comboReplaceTarget;
    public System.Windows.Forms.Button btnFindAll;
    public System.Windows.Forms.ComboBox comboReplaceSearchText;
    public System.Windows.Forms.ComboBox comboSearchText;
  }
}