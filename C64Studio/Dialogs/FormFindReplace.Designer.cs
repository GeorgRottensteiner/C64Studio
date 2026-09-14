using RetroDevStudio.Controls;

namespace RetroDevStudio.Dialogs
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
      tabFindReplace = new System.Windows.Forms.TabControl();
      tabSearch = new System.Windows.Forms.TabPage();
      btnSearchBookmark = new DecentForms.Button();
      btnFindNext = new DecentForms.Button();
      btnFindAll = new DecentForms.Button();
      groupBox1 = new System.Windows.Forms.GroupBox();
      radioSearchDirDown = new System.Windows.Forms.RadioButton();
      radioSearchDirUp = new System.Windows.Forms.RadioButton();
      checkSearchWrap = new System.Windows.Forms.CheckBox();
      checkSearchRegExp = new System.Windows.Forms.CheckBox();
      checkSearchFullWords = new System.Windows.Forms.CheckBox();
      checkSearchIgnoreCase = new System.Windows.Forms.CheckBox();
      comboSearchTarget = new System.Windows.Forms.ComboBox();
      comboSearchText = new System.Windows.Forms.ComboBox();
      labelSearchResult = new System.Windows.Forms.Label();
      label2 = new System.Windows.Forms.Label();
      label1 = new System.Windows.Forms.Label();
      tabReplace = new System.Windows.Forms.TabPage();
      labelReplaceResult = new System.Windows.Forms.Label();
      editReplaceWith = new System.Windows.Forms.TextBox();
      btnReplaceAll = new DecentForms.Button();
      btnReplaceFindNext = new DecentForms.Button();
      btnReplaceNext = new DecentForms.Button();
      groupBox2 = new System.Windows.Forms.GroupBox();
      radioReplaceSearchDown = new System.Windows.Forms.RadioButton();
      radioReplaceSearchUp = new System.Windows.Forms.RadioButton();
      checkReplaceWrap = new System.Windows.Forms.CheckBox();
      checkReplaceRegexp = new System.Windows.Forms.CheckBox();
      checkReplaceWholeWords = new System.Windows.Forms.CheckBox();
      checkReplaceIgnoreCase = new System.Windows.Forms.CheckBox();
      comboReplaceTarget = new System.Windows.Forms.ComboBox();
      comboReplaceSearchText = new System.Windows.Forms.ComboBox();
      label3 = new System.Windows.Forms.Label();
      label5 = new System.Windows.Forms.Label();
      label4 = new System.Windows.Forms.Label();
      ( (System.ComponentModel.ISupportInitialize)m_FileWatcher ).BeginInit();
      tabFindReplace.SuspendLayout();
      tabSearch.SuspendLayout();
      groupBox1.SuspendLayout();
      tabReplace.SuspendLayout();
      groupBox2.SuspendLayout();
      SuspendLayout();
      // 
      // tabFindReplace
      // 
      tabFindReplace.Controls.Add( tabSearch );
      tabFindReplace.Controls.Add( tabReplace );
      tabFindReplace.Dock = System.Windows.Forms.DockStyle.Fill;
      tabFindReplace.Location = new System.Drawing.Point( 0, 0 );
      tabFindReplace.Name = "tabFindReplace";
      tabFindReplace.SelectedIndex = 0;
      tabFindReplace.Size = new System.Drawing.Size( 350, 320 );
      tabFindReplace.TabIndex = 0;
      tabFindReplace.SelectedIndexChanged +=  tabFindReplace_SelectedIndexChanged ;
      // 
      // tabSearch
      // 
      tabSearch.Controls.Add( btnSearchBookmark );
      tabSearch.Controls.Add( btnFindNext );
      tabSearch.Controls.Add( btnFindAll );
      tabSearch.Controls.Add( groupBox1 );
      tabSearch.Controls.Add( comboSearchTarget );
      tabSearch.Controls.Add( comboSearchText );
      tabSearch.Controls.Add( labelSearchResult );
      tabSearch.Controls.Add( label2 );
      tabSearch.Controls.Add( label1 );
      tabSearch.Location = new System.Drawing.Point( 4, 22 );
      tabSearch.Name = "tabSearch";
      tabSearch.Padding = new System.Windows.Forms.Padding( 3 );
      tabSearch.Size = new System.Drawing.Size( 342, 294 );
      tabSearch.TabIndex = 0;
      tabSearch.Text = "Search";
      tabSearch.UseVisualStyleBackColor = true;
      // 
      // btnSearchBookmark
      // 
      btnSearchBookmark.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnSearchBookmark.Anchor =  System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Right ;
      btnSearchBookmark.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnSearchBookmark.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnSearchBookmark.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnSearchBookmark.DisplayAntiAliased = true;
      btnSearchBookmark.Image = null;
      btnSearchBookmark.Location = new System.Drawing.Point( 257, 198 );
      btnSearchBookmark.Name = "btnSearchBookmark";
      btnSearchBookmark.Size = new System.Drawing.Size( 75, 23 );
      btnSearchBookmark.TabIndex = 4;
      btnSearchBookmark.Text = "Bookmark";
      btnSearchBookmark.Click +=  btnSearchBookmark_Click ;
      // 
      // btnFindNext
      // 
      btnFindNext.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnFindNext.Anchor =  System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Right ;
      btnFindNext.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnFindNext.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnFindNext.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnFindNext.DisplayAntiAliased = true;
      btnFindNext.Image = null;
      btnFindNext.Location = new System.Drawing.Point( 95, 198 );
      btnFindNext.Name = "btnFindNext";
      btnFindNext.Size = new System.Drawing.Size( 75, 23 );
      btnFindNext.TabIndex = 2;
      btnFindNext.Text = "Find Next";
      btnFindNext.Click +=  btnFindNext_Click ;
      // 
      // btnFindAll
      // 
      btnFindAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnFindAll.Anchor =  System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Right ;
      btnFindAll.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnFindAll.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnFindAll.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnFindAll.DisplayAntiAliased = true;
      btnFindAll.Image = null;
      btnFindAll.Location = new System.Drawing.Point( 176, 198 );
      btnFindAll.Name = "btnFindAll";
      btnFindAll.Size = new System.Drawing.Size( 75, 23 );
      btnFindAll.TabIndex = 3;
      btnFindAll.Text = "Find All";
      btnFindAll.Click +=  btnFindAll_Click ;
      // 
      // groupBox1
      // 
      groupBox1.Anchor =   System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Left   |  System.Windows.Forms.AnchorStyles.Right ;
      groupBox1.Controls.Add( radioSearchDirDown );
      groupBox1.Controls.Add( radioSearchDirUp );
      groupBox1.Controls.Add( checkSearchWrap );
      groupBox1.Controls.Add( checkSearchRegExp );
      groupBox1.Controls.Add( checkSearchFullWords );
      groupBox1.Controls.Add( checkSearchIgnoreCase );
      groupBox1.Location = new System.Drawing.Point( 6, 86 );
      groupBox1.Name = "groupBox1";
      groupBox1.Size = new System.Drawing.Size( 326, 106 );
      groupBox1.TabIndex = 2;
      groupBox1.TabStop = false;
      groupBox1.Text = "Search Options";
      // 
      // radioSearchDirDown
      // 
      radioSearchDirDown.AutoSize = true;
      radioSearchDirDown.Checked = true;
      radioSearchDirDown.Location = new System.Drawing.Point( 159, 39 );
      radioSearchDirDown.Name = "radioSearchDirDown";
      radioSearchDirDown.Size = new System.Drawing.Size( 118, 17 );
      radioSearchDirDown.TabIndex = 5;
      radioSearchDirDown.TabStop = true;
      radioSearchDirDown.Text = "Search Downwards";
      radioSearchDirDown.UseVisualStyleBackColor = true;
      radioSearchDirDown.Visible = false;
      radioSearchDirDown.CheckedChanged +=  radioSearchDirDown_CheckedChanged ;
      // 
      // radioSearchDirUp
      // 
      radioSearchDirUp.AutoSize = true;
      radioSearchDirUp.Location = new System.Drawing.Point( 159, 19 );
      radioSearchDirUp.Name = "radioSearchDirUp";
      radioSearchDirUp.Size = new System.Drawing.Size( 104, 17 );
      radioSearchDirUp.TabIndex = 4;
      radioSearchDirUp.TabStop = true;
      radioSearchDirUp.Text = "Search Upwards";
      radioSearchDirUp.UseVisualStyleBackColor = true;
      radioSearchDirUp.Visible = false;
      radioSearchDirUp.CheckedChanged +=  radioSearchDirUp_CheckedChanged ;
      // 
      // checkSearchWrap
      // 
      checkSearchWrap.AutoSize = true;
      checkSearchWrap.Checked = true;
      checkSearchWrap.CheckState = System.Windows.Forms.CheckState.Checked;
      checkSearchWrap.Location = new System.Drawing.Point( 6, 83 );
      checkSearchWrap.Name = "checkSearchWrap";
      checkSearchWrap.Size = new System.Drawing.Size( 52, 17 );
      checkSearchWrap.TabIndex = 3;
      checkSearchWrap.Text = "Wrap";
      checkSearchWrap.UseVisualStyleBackColor = true;
      checkSearchWrap.CheckedChanged +=  checkSearchWrap_CheckedChanged ;
      // 
      // checkSearchRegExp
      // 
      checkSearchRegExp.AutoSize = true;
      checkSearchRegExp.Location = new System.Drawing.Point( 6, 61 );
      checkSearchRegExp.Name = "checkSearchRegExp";
      checkSearchRegExp.Size = new System.Drawing.Size( 117, 17 );
      checkSearchRegExp.TabIndex = 2;
      checkSearchRegExp.Text = "Regular Expression";
      checkSearchRegExp.UseVisualStyleBackColor = true;
      checkSearchRegExp.CheckedChanged +=  checkSearchRegExp_CheckedChanged ;
      // 
      // checkSearchFullWords
      // 
      checkSearchFullWords.AutoSize = true;
      checkSearchFullWords.Location = new System.Drawing.Point( 6, 40 );
      checkSearchFullWords.Name = "checkSearchFullWords";
      checkSearchFullWords.Size = new System.Drawing.Size( 95, 17 );
      checkSearchFullWords.TabIndex = 1;
      checkSearchFullWords.Text = "Full words only";
      checkSearchFullWords.UseVisualStyleBackColor = true;
      checkSearchFullWords.CheckedChanged +=  checkSearchFullWords_CheckedChanged ;
      // 
      // checkSearchIgnoreCase
      // 
      checkSearchIgnoreCase.AutoSize = true;
      checkSearchIgnoreCase.Checked = true;
      checkSearchIgnoreCase.CheckState = System.Windows.Forms.CheckState.Checked;
      checkSearchIgnoreCase.Location = new System.Drawing.Point( 6, 19 );
      checkSearchIgnoreCase.Name = "checkSearchIgnoreCase";
      checkSearchIgnoreCase.Size = new System.Drawing.Size( 83, 17 );
      checkSearchIgnoreCase.TabIndex = 0;
      checkSearchIgnoreCase.Text = "Ignore Case";
      checkSearchIgnoreCase.UseVisualStyleBackColor = true;
      checkSearchIgnoreCase.CheckedChanged +=  checkSearchIgnoreCase_CheckedChanged ;
      // 
      // comboSearchTarget
      // 
      comboSearchTarget.Anchor =   System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Left   |  System.Windows.Forms.AnchorStyles.Right ;
      comboSearchTarget.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      comboSearchTarget.FormattingEnabled = true;
      comboSearchTarget.Location = new System.Drawing.Point( 6, 59 );
      comboSearchTarget.Name = "comboSearchTarget";
      comboSearchTarget.Size = new System.Drawing.Size( 328, 21 );
      comboSearchTarget.TabIndex = 1;
      comboSearchTarget.SelectedIndexChanged +=  comboSearchTarget_SelectedIndexChanged ;
      // 
      // comboSearchText
      // 
      comboSearchText.Anchor =   System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Left   |  System.Windows.Forms.AnchorStyles.Right ;
      comboSearchText.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      comboSearchText.FormattingEnabled = true;
      comboSearchText.Location = new System.Drawing.Point( 6, 19 );
      comboSearchText.Name = "comboSearchText";
      comboSearchText.Size = new System.Drawing.Size( 328, 21 );
      comboSearchText.TabIndex = 0;
      // 
      // labelSearchResult
      // 
      labelSearchResult.Anchor =   System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Left   |  System.Windows.Forms.AnchorStyles.Right ;
      labelSearchResult.Location = new System.Drawing.Point( 3, 235 );
      labelSearchResult.Name = "labelSearchResult";
      labelSearchResult.Size = new System.Drawing.Size( 329, 17 );
      labelSearchResult.TabIndex = 0;
      labelSearchResult.Text = "No text searched for";
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new System.Drawing.Point( 3, 43 );
      label2.Name = "label2";
      label2.Size = new System.Drawing.Size( 55, 13 );
      label2.TabIndex = 0;
      label2.Text = "Search in:";
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new System.Drawing.Point( 3, 3 );
      label1.Name = "label1";
      label1.Size = new System.Drawing.Size( 59, 13 );
      label1.TabIndex = 0;
      label1.Text = "Search for:";
      // 
      // tabReplace
      // 
      tabReplace.Controls.Add( labelReplaceResult );
      tabReplace.Controls.Add( editReplaceWith );
      tabReplace.Controls.Add( btnReplaceAll );
      tabReplace.Controls.Add( btnReplaceFindNext );
      tabReplace.Controls.Add( btnReplaceNext );
      tabReplace.Controls.Add( groupBox2 );
      tabReplace.Controls.Add( comboReplaceTarget );
      tabReplace.Controls.Add( comboReplaceSearchText );
      tabReplace.Controls.Add( label3 );
      tabReplace.Controls.Add( label5 );
      tabReplace.Controls.Add( label4 );
      tabReplace.Location = new System.Drawing.Point( 4, 22 );
      tabReplace.Name = "tabReplace";
      tabReplace.Padding = new System.Windows.Forms.Padding( 3 );
      tabReplace.Size = new System.Drawing.Size( 342, 294 );
      tabReplace.TabIndex = 1;
      tabReplace.Text = "Replace";
      tabReplace.UseVisualStyleBackColor = true;
      // 
      // labelReplaceResult
      // 
      labelReplaceResult.Anchor =   System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Left   |  System.Windows.Forms.AnchorStyles.Right ;
      labelReplaceResult.Location = new System.Drawing.Point( 6, 264 );
      labelReplaceResult.Name = "labelReplaceResult";
      labelReplaceResult.Size = new System.Drawing.Size( 328, 17 );
      labelReplaceResult.TabIndex = 8;
      labelReplaceResult.Text = "No text searched for";
      // 
      // editReplaceWith
      // 
      editReplaceWith.AcceptsReturn = true;
      editReplaceWith.Anchor =   System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Left   |  System.Windows.Forms.AnchorStyles.Right ;
      editReplaceWith.Location = new System.Drawing.Point( 6, 60 );
      editReplaceWith.Name = "editReplaceWith";
      editReplaceWith.Size = new System.Drawing.Size( 328, 20 );
      editReplaceWith.TabIndex = 1;
      editReplaceWith.TextChanged +=  editReplaceTarget_TextChanged ;
      // 
      // btnReplaceAll
      // 
      btnReplaceAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnReplaceAll.Anchor =  System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Right ;
      btnReplaceAll.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnReplaceAll.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnReplaceAll.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnReplaceAll.DisplayAntiAliased = true;
      btnReplaceAll.Image = null;
      btnReplaceAll.Location = new System.Drawing.Point( 257, 238 );
      btnReplaceAll.Name = "btnReplaceAll";
      btnReplaceAll.Size = new System.Drawing.Size( 75, 23 );
      btnReplaceAll.TabIndex = 5;
      btnReplaceAll.Text = "Replace All";
      btnReplaceAll.Click +=  btnReplaceAll_Click ;
      // 
      // btnReplaceFindNext
      // 
      btnReplaceFindNext.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnReplaceFindNext.Anchor =  System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Right ;
      btnReplaceFindNext.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnReplaceFindNext.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnReplaceFindNext.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnReplaceFindNext.DisplayAntiAliased = true;
      btnReplaceFindNext.Image = null;
      btnReplaceFindNext.Location = new System.Drawing.Point( 93, 238 );
      btnReplaceFindNext.Name = "btnReplaceFindNext";
      btnReplaceFindNext.Size = new System.Drawing.Size( 75, 23 );
      btnReplaceFindNext.TabIndex = 3;
      btnReplaceFindNext.Text = "Find Next";
      btnReplaceFindNext.Click +=  btnReplaceFindNext_Click ;
      // 
      // btnReplaceNext
      // 
      btnReplaceNext.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnReplaceNext.Anchor =  System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Right ;
      btnReplaceNext.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnReplaceNext.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnReplaceNext.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnReplaceNext.DisplayAntiAliased = true;
      btnReplaceNext.Image = null;
      btnReplaceNext.Location = new System.Drawing.Point( 174, 238 );
      btnReplaceNext.Name = "btnReplaceNext";
      btnReplaceNext.Size = new System.Drawing.Size( 75, 23 );
      btnReplaceNext.TabIndex = 4;
      btnReplaceNext.Text = "Replace";
      btnReplaceNext.Click +=  btnReplaceNext_Click ;
      // 
      // groupBox2
      // 
      groupBox2.Anchor =   System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Left   |  System.Windows.Forms.AnchorStyles.Right ;
      groupBox2.Controls.Add( radioReplaceSearchDown );
      groupBox2.Controls.Add( radioReplaceSearchUp );
      groupBox2.Controls.Add( checkReplaceWrap );
      groupBox2.Controls.Add( checkReplaceRegexp );
      groupBox2.Controls.Add( checkReplaceWholeWords );
      groupBox2.Controls.Add( checkReplaceIgnoreCase );
      groupBox2.Location = new System.Drawing.Point( 6, 126 );
      groupBox2.Name = "groupBox2";
      groupBox2.Size = new System.Drawing.Size( 326, 106 );
      groupBox2.TabIndex = 7;
      groupBox2.TabStop = false;
      groupBox2.Text = "Search Options";
      // 
      // radioReplaceSearchDown
      // 
      radioReplaceSearchDown.AutoSize = true;
      radioReplaceSearchDown.Checked = true;
      radioReplaceSearchDown.Location = new System.Drawing.Point( 159, 39 );
      radioReplaceSearchDown.Name = "radioReplaceSearchDown";
      radioReplaceSearchDown.Size = new System.Drawing.Size( 118, 17 );
      radioReplaceSearchDown.TabIndex = 3;
      radioReplaceSearchDown.TabStop = true;
      radioReplaceSearchDown.Text = "Search Downwards";
      radioReplaceSearchDown.UseVisualStyleBackColor = true;
      radioReplaceSearchDown.Visible = false;
      radioReplaceSearchDown.CheckedChanged +=  radioReplaceSearchDown_CheckedChanged ;
      // 
      // radioReplaceSearchUp
      // 
      radioReplaceSearchUp.AutoSize = true;
      radioReplaceSearchUp.Location = new System.Drawing.Point( 159, 19 );
      radioReplaceSearchUp.Name = "radioReplaceSearchUp";
      radioReplaceSearchUp.Size = new System.Drawing.Size( 104, 17 );
      radioReplaceSearchUp.TabIndex = 1;
      radioReplaceSearchUp.TabStop = true;
      radioReplaceSearchUp.Text = "Search Upwards";
      radioReplaceSearchUp.UseVisualStyleBackColor = true;
      radioReplaceSearchUp.Visible = false;
      radioReplaceSearchUp.CheckedChanged +=  radioReplaceSearchUp_CheckedChanged ;
      // 
      // checkReplaceWrap
      // 
      checkReplaceWrap.AutoSize = true;
      checkReplaceWrap.Checked = true;
      checkReplaceWrap.CheckState = System.Windows.Forms.CheckState.Checked;
      checkReplaceWrap.Location = new System.Drawing.Point( 6, 83 );
      checkReplaceWrap.Name = "checkReplaceWrap";
      checkReplaceWrap.Size = new System.Drawing.Size( 52, 17 );
      checkReplaceWrap.TabIndex = 5;
      checkReplaceWrap.Text = "Wrap";
      checkReplaceWrap.UseVisualStyleBackColor = true;
      checkReplaceWrap.CheckedChanged +=  checkReplaceWrap_CheckedChanged ;
      // 
      // checkReplaceRegexp
      // 
      checkReplaceRegexp.AutoSize = true;
      checkReplaceRegexp.Location = new System.Drawing.Point( 6, 61 );
      checkReplaceRegexp.Name = "checkReplaceRegexp";
      checkReplaceRegexp.Size = new System.Drawing.Size( 117, 17 );
      checkReplaceRegexp.TabIndex = 4;
      checkReplaceRegexp.Text = "Regular Expression";
      checkReplaceRegexp.UseVisualStyleBackColor = true;
      checkReplaceRegexp.CheckedChanged +=  checkReplaceRegexp_CheckedChanged ;
      // 
      // checkReplaceWholeWords
      // 
      checkReplaceWholeWords.AutoSize = true;
      checkReplaceWholeWords.Location = new System.Drawing.Point( 6, 40 );
      checkReplaceWholeWords.Name = "checkReplaceWholeWords";
      checkReplaceWholeWords.Size = new System.Drawing.Size( 95, 17 );
      checkReplaceWholeWords.TabIndex = 2;
      checkReplaceWholeWords.Text = "Full words only";
      checkReplaceWholeWords.UseVisualStyleBackColor = true;
      checkReplaceWholeWords.CheckedChanged +=  checkReplaceWholeWords_CheckedChanged ;
      // 
      // checkReplaceIgnoreCase
      // 
      checkReplaceIgnoreCase.AutoSize = true;
      checkReplaceIgnoreCase.Checked = true;
      checkReplaceIgnoreCase.CheckState = System.Windows.Forms.CheckState.Checked;
      checkReplaceIgnoreCase.Location = new System.Drawing.Point( 6, 19 );
      checkReplaceIgnoreCase.Name = "checkReplaceIgnoreCase";
      checkReplaceIgnoreCase.Size = new System.Drawing.Size( 83, 17 );
      checkReplaceIgnoreCase.TabIndex = 0;
      checkReplaceIgnoreCase.Text = "Ignore Case";
      checkReplaceIgnoreCase.UseVisualStyleBackColor = true;
      checkReplaceIgnoreCase.CheckedChanged +=  checkReplaceIgnoreCase_CheckedChanged ;
      // 
      // comboReplaceTarget
      // 
      comboReplaceTarget.Anchor =   System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Left   |  System.Windows.Forms.AnchorStyles.Right ;
      comboReplaceTarget.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      comboReplaceTarget.FormattingEnabled = true;
      comboReplaceTarget.Location = new System.Drawing.Point( 6, 99 );
      comboReplaceTarget.Name = "comboReplaceTarget";
      comboReplaceTarget.Size = new System.Drawing.Size( 328, 21 );
      comboReplaceTarget.TabIndex = 2;
      comboReplaceTarget.SelectedIndexChanged +=  comboReplaceTarget_SelectedIndexChanged ;
      // 
      // comboReplaceSearchText
      // 
      comboReplaceSearchText.Anchor =   System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Left   |  System.Windows.Forms.AnchorStyles.Right ;
      comboReplaceSearchText.Location = new System.Drawing.Point( 6, 19 );
      comboReplaceSearchText.Name = "comboReplaceSearchText";
      comboReplaceSearchText.Size = new System.Drawing.Size( 328, 21 );
      comboReplaceSearchText.TabIndex = 0;
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new System.Drawing.Point( 3, 83 );
      label3.Name = "label3";
      label3.Size = new System.Drawing.Size( 55, 13 );
      label3.TabIndex = 3;
      label3.Text = "Search in:";
      // 
      // label5
      // 
      label5.AutoSize = true;
      label5.Location = new System.Drawing.Point( 3, 43 );
      label5.Name = "label5";
      label5.Size = new System.Drawing.Size( 72, 13 );
      label5.TabIndex = 4;
      label5.Text = "Replace with:";
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Location = new System.Drawing.Point( 3, 3 );
      label4.Name = "label4";
      label4.Size = new System.Drawing.Size( 59, 13 );
      label4.TabIndex = 4;
      label4.Text = "Search for:";
      // 
      // FormFindReplace
      // 
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      ClientSize = new System.Drawing.Size( 350, 320 );
      Controls.Add( tabFindReplace );
      FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
      HideOnClose = true;
      KeyPreview = true;
      Name = "FormFindReplace";
      ShowIcon = false;
      ShowInTaskbar = false;
      Text = "Find/Replace";
      VisibleChanged +=  FormFindReplace_VisibleChanged ;
      KeyDown +=  FormFindReplace_KeyDown ;
      ( (System.ComponentModel.ISupportInitialize)m_FileWatcher ).EndInit();
      tabFindReplace.ResumeLayout( false );
      tabSearch.ResumeLayout( false );
      tabSearch.PerformLayout();
      groupBox1.ResumeLayout( false );
      groupBox1.PerformLayout();
      tabReplace.ResumeLayout( false );
      tabReplace.PerformLayout();
      groupBox2.ResumeLayout( false );
      groupBox2.PerformLayout();
      ResumeLayout( false );

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
    private DecentForms.Button btnSearchBookmark;
    private System.Windows.Forms.CheckBox checkSearchWrap;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.RadioButton radioReplaceSearchDown;
    private System.Windows.Forms.RadioButton radioReplaceSearchUp;
    private System.Windows.Forms.CheckBox checkReplaceWrap;
    private System.Windows.Forms.CheckBox checkReplaceRegexp;
    private System.Windows.Forms.CheckBox checkReplaceWholeWords;
    private System.Windows.Forms.CheckBox checkReplaceIgnoreCase;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label4;
    private DecentForms.Button btnReplaceAll;
    private DecentForms.Button btnReplaceFindNext;
    private DecentForms.Button btnReplaceNext;
    public DecentForms.Button btnFindNext;
    public System.Windows.Forms.ComboBox comboReplaceTarget;
    public DecentForms.Button btnFindAll;
    public System.Windows.Forms.ComboBox comboReplaceSearchText;
    public System.Windows.Forms.ComboBox comboSearchText;
    private System.Windows.Forms.TextBox editReplaceWith;
    private System.Windows.Forms.Label labelSearchResult;
    private System.Windows.Forms.Label labelReplaceResult;
  }
}