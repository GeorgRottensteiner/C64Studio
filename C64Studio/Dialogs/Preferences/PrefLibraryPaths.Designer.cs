namespace RetroDevStudio.Dialogs.Preferences
{
  partial class PrefLibraryPaths
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
      this.btmASMLibraryPathBrowse = new DecentForms.Button();
      this.editASMLibraryPath = new System.Windows.Forms.TextBox();
      this.label30 = new System.Windows.Forms.Label();
      this.asmLibraryPathList = new RetroDevStudio.Controls.ArrangedItemList();
      this.SuspendLayout();
      // 
      // btmASMLibraryPathBrowse
      // 
      this.btmASMLibraryPathBrowse.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btmASMLibraryPathBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btmASMLibraryPathBrowse.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btmASMLibraryPathBrowse.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btmASMLibraryPathBrowse.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btmASMLibraryPathBrowse.Image = null;
      this.btmASMLibraryPathBrowse.Location = new System.Drawing.Point(477, 191);
      this.btmASMLibraryPathBrowse.Name = "btmASMLibraryPathBrowse";
      this.btmASMLibraryPathBrowse.Size = new System.Drawing.Size(35, 20);
      this.btmASMLibraryPathBrowse.TabIndex = 7;
      this.btmASMLibraryPathBrowse.Text = "...";
      this.btmASMLibraryPathBrowse.Click += new DecentForms.EventHandler(this.btmASMLibraryPathBrowse_Click);
      // 
      // editASMLibraryPath
      // 
      this.editASMLibraryPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.editASMLibraryPath.Location = new System.Drawing.Point(14, 191);
      this.editASMLibraryPath.Name = "editASMLibraryPath";
      this.editASMLibraryPath.Size = new System.Drawing.Size(457, 20);
      this.editASMLibraryPath.TabIndex = 6;
      // 
      // label30
      // 
      this.label30.AutoSize = true;
      this.label30.Location = new System.Drawing.Point(5, 3);
      this.label30.Name = "label30";
      this.label30.Size = new System.Drawing.Size(71, 13);
      this.label30.TabIndex = 26;
      this.label30.Text = "Library Paths:";
      // 
      // asmLibraryPathList
      // 
      this.asmLibraryPathList.AddButtonEnabled = true;
      this.asmLibraryPathList.AllowClone = true;
      this.asmLibraryPathList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.asmLibraryPathList.DeleteButtonEnabled = false;
      this.asmLibraryPathList.HasOwnerDrawColumn = true;
      this.asmLibraryPathList.HighlightColor = System.Drawing.SystemColors.HotTrack;
      this.asmLibraryPathList.HighlightTextColor = System.Drawing.SystemColors.HighlightText;
      this.asmLibraryPathList.Location = new System.Drawing.Point(14, 26);
      this.asmLibraryPathList.Margin = new System.Windows.Forms.Padding(48, 22, 48, 22);
      this.asmLibraryPathList.MoveDownButtonEnabled = false;
      this.asmLibraryPathList.MoveUpButtonEnabled = false;
      this.asmLibraryPathList.MustHaveOneElement = false;
      this.asmLibraryPathList.Name = "asmLibraryPathList";
      this.asmLibraryPathList.SelectedIndex = -1;
      this.asmLibraryPathList.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      this.asmLibraryPathList.SelectionTextColor = System.Drawing.SystemColors.HighlightText;
      this.asmLibraryPathList.Size = new System.Drawing.Size(498, 153);
      this.asmLibraryPathList.TabIndex = 5;
      this.asmLibraryPathList.AddingItem += new RetroDevStudio.Controls.ArrangedItemList.AddingItemEventHandler(this.asmLibraryPathList_AddingItem);
      this.asmLibraryPathList.ItemAdded += new RetroDevStudio.Controls.ArrangedItemList.ItemModifiedEventHandler(this.asmLibraryPathList_ItemAdded);
      this.asmLibraryPathList.ItemRemoved += new RetroDevStudio.Controls.ArrangedItemList.ItemModifiedEventHandler(this.asmLibraryPathList_ItemRemoved);
      this.asmLibraryPathList.ItemMoved += new RetroDevStudio.Controls.ArrangedItemList.ItemExchangedEventHandler(this.asmLibraryPathList_ItemMoved);
      // 
      // PrefLibraryPaths
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.btmASMLibraryPathBrowse);
      this.Controls.Add(this.editASMLibraryPath);
      this.Controls.Add(this.label30);
      this.Controls.Add(this.asmLibraryPathList);
      this.Name = "PrefLibraryPaths";
      this.Size = new System.Drawing.Size(540, 234);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

        #endregion
        private DecentForms.Button btmASMLibraryPathBrowse;
        private System.Windows.Forms.TextBox editASMLibraryPath;
        private System.Windows.Forms.Label label30;
        private Controls.ArrangedItemList asmLibraryPathList;
  }
}
