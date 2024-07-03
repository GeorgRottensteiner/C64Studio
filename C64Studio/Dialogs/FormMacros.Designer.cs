﻿namespace RetroDevStudio.Dialogs
{
  partial class FormMacros
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
      this.listMacros = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.btnInsert = new DecentForms.Button();
      this.btnOK = new DecentForms.Button();
      this.SuspendLayout();
      // 
      // listMacros
      // 
      this.listMacros.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.listMacros.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
      this.listMacros.FullRowSelect = true;
      this.listMacros.HideSelection = false;
      this.listMacros.Location = new System.Drawing.Point(12, 12);
      this.listMacros.Name = "listMacros";
      this.listMacros.Size = new System.Drawing.Size(562, 276);
      this.listMacros.TabIndex = 0;
      this.listMacros.UseCompatibleStateImageBehavior = false;
      this.listMacros.View = System.Windows.Forms.View.Details;
      this.listMacros.ItemActivate += new System.EventHandler(this.listMacros_ItemActivate);
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Macro";
      this.columnHeader1.Width = 182;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Filled in";
      this.columnHeader2.Width = 200;
      // 
      // btnInsert
      // 
      this.btnInsert.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnInsert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnInsert.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnInsert.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnInsert.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnInsert.Image = null;
      this.btnInsert.Location = new System.Drawing.Point(12, 294);
      this.btnInsert.Name = "btnInsert";
      this.btnInsert.Size = new System.Drawing.Size(75, 23);
      this.btnInsert.TabIndex = 1;
      this.btnInsert.Text = "Insert";
      this.btnInsert.Click += new DecentForms.EventHandler(this.btnInsert_Click);
      // 
      // btnOK
      // 
      this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnOK.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnOK.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.Image = null;
      this.btnOK.Location = new System.Drawing.Point(499, 294);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new DecentForms.EventHandler(this.btnOK_Click);
      // 
      // FormMacros
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(586, 325);
      this.Controls.Add(this.btnInsert);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.listMacros);
      this.Name = "FormMacros";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "Macro Preview";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMacros_FormClosing);
      this.Load += new System.EventHandler(this.FormMacros_Load);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListView listMacros;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private DecentForms.Button btnOK;
    private DecentForms.Button btnInsert;
  }
}