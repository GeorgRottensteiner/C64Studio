namespace RetroDevStudio.Dialogs.Preferences
{
  partial class DlgPrefASMEditor
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
      checkEditorShowMaxLineLengthIndicator = new System.Windows.Forms.CheckBox();
      editMaxLineLengthIndicatorColumn = new System.Windows.Forms.TextBox();
      label2 = new System.Windows.Forms.Label();
      comboASMEncoding = new System.Windows.Forms.ComboBox();
      label35 = new System.Windows.Forms.Label();
      checkASMShowAddress = new System.Windows.Forms.CheckBox();
      checkASMShowAutoComplete = new System.Windows.Forms.CheckBox();
      checkASMShowMiniMap = new System.Windows.Forms.CheckBox();
      checkASMShowSizes = new System.Windows.Forms.CheckBox();
      checkASMShowCycles = new System.Windows.Forms.CheckBox();
      checkASMShowLineNumbers = new System.Windows.Forms.CheckBox();
      btnSetDefaultsFont = new DecentForms.Button();
      btnChooseFont = new DecentForms.Button();
      editCaretWidth = new System.Windows.Forms.TextBox();
      editTabSize = new System.Windows.Forms.TextBox();
      label3 = new System.Windows.Forms.Label();
      label14 = new System.Windows.Forms.Label();
      checkStripTrailingSpaces = new System.Windows.Forms.CheckBox();
      checkConvertTabsToSpaces = new System.Windows.Forms.CheckBox();
      checkASMFontNoAntiAliasing = new System.Windows.Forms.CheckBox();
      labelFontPreview = new DecentForms.Label();
      checkShowTabs = new System.Windows.Forms.CheckBox();
      SuspendLayout();
      // 
      // checkEditorShowMaxLineLengthIndicator
      // 
      checkEditorShowMaxLineLengthIndicator.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      checkEditorShowMaxLineLengthIndicator.Location = new System.Drawing.Point( 4, 438 );
      checkEditorShowMaxLineLengthIndicator.Margin = new System.Windows.Forms.Padding( 4, 3, 4, 3 );
      checkEditorShowMaxLineLengthIndicator.Name = "checkEditorShowMaxLineLengthIndicator";
      checkEditorShowMaxLineLengthIndicator.Size = new System.Drawing.Size( 326, 20 );
      checkEditorShowMaxLineLengthIndicator.TabIndex = 14;
      checkEditorShowMaxLineLengthIndicator.Text = "Show max line length indicator";
      checkEditorShowMaxLineLengthIndicator.UseVisualStyleBackColor = true;
      checkEditorShowMaxLineLengthIndicator.CheckedChanged +=  checkEditorShowMaxLineLengthIndicator_CheckedChanged ;
      // 
      // editMaxLineLengthIndicatorColumn
      // 
      editMaxLineLengthIndicatorColumn.Enabled = false;
      editMaxLineLengthIndicatorColumn.Location = new System.Drawing.Point( 415, 434 );
      editMaxLineLengthIndicatorColumn.Margin = new System.Windows.Forms.Padding( 4, 3, 4, 3 );
      editMaxLineLengthIndicatorColumn.MaxLength = 3;
      editMaxLineLengthIndicatorColumn.Name = "editMaxLineLengthIndicatorColumn";
      editMaxLineLengthIndicatorColumn.Size = new System.Drawing.Size( 111, 23 );
      editMaxLineLengthIndicatorColumn.TabIndex = 15;
      editMaxLineLengthIndicatorColumn.TextChanged +=  editMaxLineLengthIndicatorColumn_TextChanged ;
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Enabled = false;
      label2.Location = new System.Drawing.Point( 356, 438 );
      label2.Margin = new System.Windows.Forms.Padding( 4, 0, 4, 0 );
      label2.Name = "label2";
      label2.Size = new System.Drawing.Size( 53, 15 );
      label2.TabIndex = 33;
      label2.Text = "Column:";
      // 
      // comboASMEncoding
      // 
      comboASMEncoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      comboASMEncoding.FormattingEnabled = true;
      comboASMEncoding.Location = new System.Drawing.Point( 184, 469 );
      comboASMEncoding.Margin = new System.Windows.Forms.Padding( 4, 3, 4, 3 );
      comboASMEncoding.Name = "comboASMEncoding";
      comboASMEncoding.Size = new System.Drawing.Size( 342, 23 );
      comboASMEncoding.TabIndex = 16;
      comboASMEncoding.SelectedIndexChanged +=  comboASMEncoding_SelectedIndexChanged ;
      // 
      // label35
      // 
      label35.AutoSize = true;
      label35.Location = new System.Drawing.Point( 4, 472 );
      label35.Margin = new System.Windows.Forms.Padding( 4, 0, 4, 0 );
      label35.Name = "label35";
      label35.Size = new System.Drawing.Size( 60, 15 );
      label35.TabIndex = 32;
      label35.Text = "Encoding:";
      // 
      // checkASMShowAddress
      // 
      checkASMShowAddress.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      checkASMShowAddress.Location = new System.Drawing.Point( 4, 403 );
      checkASMShowAddress.Margin = new System.Windows.Forms.Padding( 4, 3, 4, 3 );
      checkASMShowAddress.Name = "checkASMShowAddress";
      checkASMShowAddress.Size = new System.Drawing.Size( 326, 28 );
      checkASMShowAddress.TabIndex = 13;
      checkASMShowAddress.Text = "Show Address";
      checkASMShowAddress.UseVisualStyleBackColor = true;
      checkASMShowAddress.CheckedChanged +=  checkASMShowAddress_CheckedChanged ;
      // 
      // checkASMShowAutoComplete
      // 
      checkASMShowAutoComplete.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      checkASMShowAutoComplete.Location = new System.Drawing.Point( 4, 369 );
      checkASMShowAutoComplete.Margin = new System.Windows.Forms.Padding( 4, 3, 4, 3 );
      checkASMShowAutoComplete.Name = "checkASMShowAutoComplete";
      checkASMShowAutoComplete.Size = new System.Drawing.Size( 326, 28 );
      checkASMShowAutoComplete.TabIndex = 12;
      checkASMShowAutoComplete.Text = "Show Auto-Complete";
      checkASMShowAutoComplete.UseVisualStyleBackColor = true;
      checkASMShowAutoComplete.CheckedChanged +=  checkASMShowAutoComplete_CheckedChanged ;
      // 
      // checkASMShowMiniMap
      // 
      checkASMShowMiniMap.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      checkASMShowMiniMap.Location = new System.Drawing.Point( 4, 334 );
      checkASMShowMiniMap.Margin = new System.Windows.Forms.Padding( 4, 3, 4, 3 );
      checkASMShowMiniMap.Name = "checkASMShowMiniMap";
      checkASMShowMiniMap.Size = new System.Drawing.Size( 326, 28 );
      checkASMShowMiniMap.TabIndex = 11;
      checkASMShowMiniMap.Text = "Show Mini View";
      checkASMShowMiniMap.UseVisualStyleBackColor = true;
      checkASMShowMiniMap.CheckedChanged +=  checkASMShowMiniMap_CheckedChanged ;
      // 
      // checkASMShowSizes
      // 
      checkASMShowSizes.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      checkASMShowSizes.Location = new System.Drawing.Point( 4, 299 );
      checkASMShowSizes.Margin = new System.Windows.Forms.Padding( 4, 3, 4, 3 );
      checkASMShowSizes.Name = "checkASMShowSizes";
      checkASMShowSizes.Size = new System.Drawing.Size( 326, 28 );
      checkASMShowSizes.TabIndex = 10;
      checkASMShowSizes.Text = "Show Sizes";
      checkASMShowSizes.UseVisualStyleBackColor = true;
      checkASMShowSizes.CheckedChanged +=  checkASMShowSizes_CheckedChanged ;
      // 
      // checkASMShowCycles
      // 
      checkASMShowCycles.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      checkASMShowCycles.Location = new System.Drawing.Point( 4, 265 );
      checkASMShowCycles.Margin = new System.Windows.Forms.Padding( 4, 3, 4, 3 );
      checkASMShowCycles.Name = "checkASMShowCycles";
      checkASMShowCycles.Size = new System.Drawing.Size( 326, 28 );
      checkASMShowCycles.TabIndex = 9;
      checkASMShowCycles.Text = "Show Cycles";
      checkASMShowCycles.UseVisualStyleBackColor = true;
      checkASMShowCycles.CheckedChanged +=  checkASMShowCycles_CheckedChanged ;
      // 
      // checkASMShowLineNumbers
      // 
      checkASMShowLineNumbers.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      checkASMShowLineNumbers.Location = new System.Drawing.Point( 4, 230 );
      checkASMShowLineNumbers.Margin = new System.Windows.Forms.Padding( 4, 3, 4, 3 );
      checkASMShowLineNumbers.Name = "checkASMShowLineNumbers";
      checkASMShowLineNumbers.Size = new System.Drawing.Size( 326, 28 );
      checkASMShowLineNumbers.TabIndex = 8;
      checkASMShowLineNumbers.Text = "Show Line Numbers";
      checkASMShowLineNumbers.UseVisualStyleBackColor = true;
      checkASMShowLineNumbers.CheckedChanged +=  checkASMShowLineNumbers_CheckedChanged ;
      // 
      // btnSetDefaultsFont
      // 
      btnSetDefaultsFont.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnSetDefaultsFont.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnSetDefaultsFont.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnSetDefaultsFont.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnSetDefaultsFont.DisplayAntiAliased = true;
      btnSetDefaultsFont.Image = null;
      btnSetDefaultsFont.Location = new System.Drawing.Point( 184, 197 );
      btnSetDefaultsFont.Margin = new System.Windows.Forms.Padding( 4, 3, 4, 3 );
      btnSetDefaultsFont.Name = "btnSetDefaultsFont";
      btnSetDefaultsFont.Size = new System.Drawing.Size( 145, 27 );
      btnSetDefaultsFont.TabIndex = 7;
      btnSetDefaultsFont.Text = "Set Default Font";
      btnSetDefaultsFont.Click +=  btnSetDefaultsFont_Click ;
      // 
      // btnChooseFont
      // 
      btnChooseFont.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      btnChooseFont.BorderStyle = DecentForms.BorderStyle.FLAT;
      btnChooseFont.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      btnChooseFont.DialogResult = System.Windows.Forms.DialogResult.OK;
      btnChooseFont.DisplayAntiAliased = true;
      btnChooseFont.Image = null;
      btnChooseFont.Location = new System.Drawing.Point( 184, 163 );
      btnChooseFont.Margin = new System.Windows.Forms.Padding( 4, 3, 4, 3 );
      btnChooseFont.Name = "btnChooseFont";
      btnChooseFont.Size = new System.Drawing.Size( 145, 27 );
      btnChooseFont.TabIndex = 5;
      btnChooseFont.Text = "Change Font";
      btnChooseFont.Click +=  btnChooseFont_Click ;
      // 
      // editCaretWidth
      // 
      editCaretWidth.Location = new System.Drawing.Point( 184, 133 );
      editCaretWidth.Margin = new System.Windows.Forms.Padding( 4, 3, 4, 3 );
      editCaretWidth.MaxLength = 2;
      editCaretWidth.Name = "editCaretWidth";
      editCaretWidth.Size = new System.Drawing.Size( 144, 23 );
      editCaretWidth.TabIndex = 4;
      editCaretWidth.TextChanged +=  editCaretWidth_TextChanged ;
      // 
      // editTabSize
      // 
      editTabSize.Location = new System.Drawing.Point( 184, 35 );
      editTabSize.Margin = new System.Windows.Forms.Padding( 4, 3, 4, 3 );
      editTabSize.MaxLength = 2;
      editTabSize.Name = "editTabSize";
      editTabSize.Size = new System.Drawing.Size( 144, 23 );
      editTabSize.TabIndex = 1;
      editTabSize.TextChanged +=  editTabSize_TextChanged ;
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new System.Drawing.Point( 4, 137 );
      label3.Margin = new System.Windows.Forms.Padding( 4, 0, 4, 0 );
      label3.Name = "label3";
      label3.Size = new System.Drawing.Size( 80, 15 );
      label3.TabIndex = 14;
      label3.Text = "Cursor Width:";
      // 
      // label14
      // 
      label14.AutoSize = true;
      label14.Location = new System.Drawing.Point( 4, 38 );
      label14.Margin = new System.Windows.Forms.Padding( 4, 0, 4, 0 );
      label14.Name = "label14";
      label14.Size = new System.Drawing.Size( 52, 15 );
      label14.TabIndex = 14;
      label14.Text = "Tab Size:";
      // 
      // checkStripTrailingSpaces
      // 
      checkStripTrailingSpaces.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      checkStripTrailingSpaces.Location = new System.Drawing.Point( 4, 65 );
      checkStripTrailingSpaces.Margin = new System.Windows.Forms.Padding( 4, 3, 4, 3 );
      checkStripTrailingSpaces.Name = "checkStripTrailingSpaces";
      checkStripTrailingSpaces.Size = new System.Drawing.Size( 326, 28 );
      checkStripTrailingSpaces.TabIndex = 2;
      checkStripTrailingSpaces.Text = "Strip Trailing Spaces/Tabs";
      checkStripTrailingSpaces.UseVisualStyleBackColor = true;
      checkStripTrailingSpaces.CheckedChanged +=  checkStripTrailingSpaces_CheckedChanged ;
      // 
      // checkConvertTabsToSpaces
      // 
      checkConvertTabsToSpaces.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      checkConvertTabsToSpaces.Location = new System.Drawing.Point( 4, 3 );
      checkConvertTabsToSpaces.Margin = new System.Windows.Forms.Padding( 4, 3, 4, 3 );
      checkConvertTabsToSpaces.Name = "checkConvertTabsToSpaces";
      checkConvertTabsToSpaces.Size = new System.Drawing.Size( 326, 28 );
      checkConvertTabsToSpaces.TabIndex = 0;
      checkConvertTabsToSpaces.Text = "Convert tabs to spaces";
      checkConvertTabsToSpaces.UseVisualStyleBackColor = true;
      checkConvertTabsToSpaces.CheckedChanged +=  checkConvertTabsToSpaces_CheckedChanged ;
      // 
      // checkASMFontNoAntiAliasing
      // 
      checkASMFontNoAntiAliasing.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      checkASMFontNoAntiAliasing.Location = new System.Drawing.Point( 23, 197 );
      checkASMFontNoAntiAliasing.Margin = new System.Windows.Forms.Padding( 4, 3, 4, 3 );
      checkASMFontNoAntiAliasing.Name = "checkASMFontNoAntiAliasing";
      checkASMFontNoAntiAliasing.Size = new System.Drawing.Size( 154, 28 );
      checkASMFontNoAntiAliasing.TabIndex = 6;
      checkASMFontNoAntiAliasing.Text = "No Anti Aliasing";
      checkASMFontNoAntiAliasing.UseVisualStyleBackColor = true;
      checkASMFontNoAntiAliasing.CheckedChanged +=  checkASMFontNoAntiAliasing_CheckedChanged ;
      // 
      // labelFontPreview
      // 
      labelFontPreview.AccessibleRole = System.Windows.Forms.AccessibleRole.StaticText;
      labelFontPreview.BorderStyle = DecentForms.BorderStyle.FLAT;
      labelFontPreview.DisplayAntiAliased = true;
      labelFontPreview.Image = null;
      labelFontPreview.Location = new System.Drawing.Point( 336, 163 );
      labelFontPreview.Margin = new System.Windows.Forms.Padding( 4, 3, 4, 3 );
      labelFontPreview.Name = "labelFontPreview";
      labelFontPreview.Size = new System.Drawing.Size( 266, 61 );
      labelFontPreview.TabIndex = 18;
      labelFontPreview.Text = "Font Preview";
      labelFontPreview.TextAlignment = DecentForms.TextAlignment.LEFT;
      // 
      // checkShowTabs
      // 
      checkShowTabs.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      checkShowTabs.Location = new System.Drawing.Point( 4, 99 );
      checkShowTabs.Margin = new System.Windows.Forms.Padding( 4, 3, 4, 3 );
      checkShowTabs.Name = "checkShowTabs";
      checkShowTabs.Size = new System.Drawing.Size( 326, 28 );
      checkShowTabs.TabIndex = 3;
      checkShowTabs.Text = "Display Tab Character";
      checkShowTabs.UseVisualStyleBackColor = true;
      checkShowTabs.CheckedChanged +=  checkShowTabs_CheckedChanged ;
      // 
      // DlgPrefASMEditor
      // 
      AutoScaleDimensions = new System.Drawing.SizeF( 7F, 15F );
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      Controls.Add( checkEditorShowMaxLineLengthIndicator );
      Controls.Add( editMaxLineLengthIndicatorColumn );
      Controls.Add( checkConvertTabsToSpaces );
      Controls.Add( label2 );
      Controls.Add( checkShowTabs );
      Controls.Add( checkStripTrailingSpaces );
      Controls.Add( comboASMEncoding );
      Controls.Add( label14 );
      Controls.Add( label35 );
      Controls.Add( checkASMShowAddress );
      Controls.Add( label3 );
      Controls.Add( checkASMShowAutoComplete );
      Controls.Add( editTabSize );
      Controls.Add( checkASMShowMiniMap );
      Controls.Add( editCaretWidth );
      Controls.Add( checkASMShowSizes );
      Controls.Add( labelFontPreview );
      Controls.Add( checkASMShowCycles );
      Controls.Add( btnChooseFont );
      Controls.Add( checkASMFontNoAntiAliasing );
      Controls.Add( checkASMShowLineNumbers );
      Controls.Add( btnSetDefaultsFont );
      Margin = new System.Windows.Forms.Padding( 5, 3, 5, 3 );
      Name = "DlgPrefASMEditor";
      Size = new System.Drawing.Size( 616, 526 );
      ResumeLayout( false );
      PerformLayout();

    }

    #endregion
    private System.Windows.Forms.TextBox editTabSize;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox checkStripTrailingSpaces;
        private System.Windows.Forms.CheckBox checkConvertTabsToSpaces;
        private DecentForms.Button btnSetDefaultsFont;
        private DecentForms.Button btnChooseFont;
    private System.Windows.Forms.CheckBox checkASMShowAddress;
        private System.Windows.Forms.CheckBox checkASMShowAutoComplete;
        private System.Windows.Forms.CheckBox checkASMShowMiniMap;
        private System.Windows.Forms.CheckBox checkASMShowSizes;
        private System.Windows.Forms.CheckBox checkASMShowCycles;
        private System.Windows.Forms.CheckBox checkASMShowLineNumbers;
        private System.Windows.Forms.ComboBox comboASMEncoding;
        private System.Windows.Forms.Label label35;
    private System.Windows.Forms.CheckBox checkEditorShowMaxLineLengthIndicator;
    private System.Windows.Forms.TextBox editMaxLineLengthIndicatorColumn;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox editCaretWidth;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.CheckBox checkASMFontNoAntiAliasing;
    private DecentForms.Label labelFontPreview;
    private System.Windows.Forms.CheckBox checkShowTabs;
  }
}
