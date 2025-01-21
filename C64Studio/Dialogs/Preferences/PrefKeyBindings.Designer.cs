namespace RetroDevStudio.Dialogs.Preferences
{
  partial class PrefKeyBindings
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
      this.btnSetDefaultsKeyBinding = new DecentForms.Button();
      this.btnUnbindKey = new DecentForms.Button();
      this.btnBindKeySecondary = new DecentForms.Button();
      this.btnBindKey = new DecentForms.Button();
      this.editKeyBinding = new System.Windows.Forms.TextBox();
      this.label10 = new System.Windows.Forms.Label();
      this.listFunctions = new System.Windows.Forms.ListView();
      this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.SuspendLayout();
      // 
      // btnSetDefaultsKeyBinding
      // 
      this.btnSetDefaultsKeyBinding.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnSetDefaultsKeyBinding.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnSetDefaultsKeyBinding.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnSetDefaultsKeyBinding.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnSetDefaultsKeyBinding.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnSetDefaultsKeyBinding.Image = null;
      this.btnSetDefaultsKeyBinding.Location = new System.Drawing.Point(548, 394);
      this.btnSetDefaultsKeyBinding.Name = "btnSetDefaultsKeyBinding";
      this.btnSetDefaultsKeyBinding.Size = new System.Drawing.Size(75, 23);
      this.btnSetDefaultsKeyBinding.TabIndex = 18;
      this.btnSetDefaultsKeyBinding.Text = "Set Defaults";
      this.btnSetDefaultsKeyBinding.Click += new DecentForms.EventHandler(this.btnSetDefaultsKeyBinding_Click);
      // 
      // btnUnbindKey
      // 
      this.btnUnbindKey.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnUnbindKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnUnbindKey.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnUnbindKey.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnUnbindKey.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnUnbindKey.Enabled = false;
      this.btnUnbindKey.Image = null;
      this.btnUnbindKey.Location = new System.Drawing.Point(467, 394);
      this.btnUnbindKey.Name = "btnUnbindKey";
      this.btnUnbindKey.Size = new System.Drawing.Size(75, 23);
      this.btnUnbindKey.TabIndex = 19;
      this.btnUnbindKey.Text = "Unbind Key";
      this.btnUnbindKey.Click += new DecentForms.EventHandler(this.btnUnbindKey_Click);
      // 
      // btnBindKeySecondary
      // 
      this.btnBindKeySecondary.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnBindKeySecondary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnBindKeySecondary.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnBindKeySecondary.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnBindKeySecondary.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnBindKeySecondary.Image = null;
      this.btnBindKeySecondary.Location = new System.Drawing.Point(386, 394);
      this.btnBindKeySecondary.Name = "btnBindKeySecondary";
      this.btnBindKeySecondary.Size = new System.Drawing.Size(75, 23);
      this.btnBindKeySecondary.TabIndex = 20;
      this.btnBindKeySecondary.Text = "Bind 2nd";
      this.btnBindKeySecondary.Click += new DecentForms.EventHandler(this.btnBindKeySecondary_Click);
      // 
      // btnBindKey
      // 
      this.btnBindKey.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnBindKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnBindKey.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnBindKey.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnBindKey.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnBindKey.Image = null;
      this.btnBindKey.Location = new System.Drawing.Point(305, 394);
      this.btnBindKey.Name = "btnBindKey";
      this.btnBindKey.Size = new System.Drawing.Size(75, 23);
      this.btnBindKey.TabIndex = 21;
      this.btnBindKey.Text = "Bind Key";
      this.btnBindKey.Click += new DecentForms.EventHandler(this.btnBindKey_Click);
      // 
      // editKeyBinding
      // 
      this.editKeyBinding.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.editKeyBinding.Location = new System.Drawing.Point(72, 396);
      this.editKeyBinding.Name = "editKeyBinding";
      this.editKeyBinding.ReadOnly = true;
      this.editKeyBinding.Size = new System.Drawing.Size(227, 20);
      this.editKeyBinding.TabIndex = 17;
      this.editKeyBinding.Text = "Press Key here";
      this.editKeyBinding.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.editKeyBinding_PreviewKeyDown);
      // 
      // label10
      // 
      this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(0, 399);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(66, 13);
      this.label10.TabIndex = 16;
      this.label10.Text = "Key Binding:";
      // 
      // listFunctions
      // 
      this.listFunctions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.listFunctions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader8});
      this.listFunctions.FullRowSelect = true;
      this.listFunctions.HideSelection = false;
      this.listFunctions.Location = new System.Drawing.Point(3, 5);
      this.listFunctions.MultiSelect = false;
      this.listFunctions.Name = "listFunctions";
      this.listFunctions.Size = new System.Drawing.Size(620, 383);
      this.listFunctions.Sorting = System.Windows.Forms.SortOrder.Ascending;
      this.listFunctions.TabIndex = 15;
      this.listFunctions.UseCompatibleStateImageBehavior = false;
      this.listFunctions.View = System.Windows.Forms.View.Details;
      this.listFunctions.SelectedIndexChanged += new System.EventHandler(this.listFunctions_SelectedIndexChanged);
      // 
      // columnHeader4
      // 
      this.columnHeader4.Text = "State";
      this.columnHeader4.Width = 126;
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Function";
      this.columnHeader1.Width = 222;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Binding";
      this.columnHeader2.Width = 113;
      // 
      // columnHeader8
      // 
      this.columnHeader8.Text = "2nd Binding";
      this.columnHeader8.Width = 113;
      // 
      // PrefKeyBindings
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.btnSetDefaultsKeyBinding);
      this.Controls.Add(this.btnUnbindKey);
      this.Controls.Add(this.listFunctions);
      this.Controls.Add(this.btnBindKeySecondary);
      this.Controls.Add(this.label10);
      this.Controls.Add(this.btnBindKey);
      this.Controls.Add(this.editKeyBinding);
      this.Name = "PrefKeyBindings";
      this.Size = new System.Drawing.Size(645, 436);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

        #endregion
        private DecentForms.Button btnSetDefaultsKeyBinding;
        private DecentForms.Button btnUnbindKey;
        private DecentForms.Button btnBindKeySecondary;
        private DecentForms.Button btnBindKey;
        private System.Windows.Forms.TextBox editKeyBinding;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ListView listFunctions;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader8;
    }
}
