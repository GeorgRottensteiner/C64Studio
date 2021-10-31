namespace C64Studio
{
  partial class DlgPaletteEditor
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
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.listPalette = new System.Windows.Forms.ListBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.scrollB = new System.Windows.Forms.HScrollBar();
      this.scrollG = new System.Windows.Forms.HScrollBar();
      this.scrollR = new System.Windows.Forms.HScrollBar();
      this.editBHex = new System.Windows.Forms.TextBox();
      this.editB = new System.Windows.Forms.TextBox();
      this.editGHex = new System.Windows.Forms.TextBox();
      this.editG = new System.Windows.Forms.TextBox();
      this.editRHex = new System.Windows.Forms.TextBox();
      this.editR = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.panelColorPreview = new System.Windows.Forms.Panel();
      this.groupBox4 = new System.Windows.Forms.GroupBox();
      this.editPaletteName = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.paletteList = new C64Studio.ArrangedItemList();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.groupBox4.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnCancel
      // 
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(633, 266);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // btnOK
      // 
      this.btnOK.Location = new System.Drawing.Point(552, 266);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // listPalette
      // 
      this.listPalette.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listPalette.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.listPalette.FormattingEnabled = true;
      this.listPalette.Location = new System.Drawing.Point(3, 16);
      this.listPalette.Name = "listPalette";
      this.listPalette.Size = new System.Drawing.Size(218, 229);
      this.listPalette.TabIndex = 2;
      this.listPalette.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listPalette_DrawItem);
      this.listPalette.SelectedIndexChanged += new System.EventHandler(this.listPalette_SelectedIndexChanged);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.listPalette);
      this.groupBox1.Location = new System.Drawing.Point(240, 12);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(224, 248);
      this.groupBox1.TabIndex = 3;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Current Palette";
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.scrollB);
      this.groupBox2.Controls.Add(this.scrollG);
      this.groupBox2.Controls.Add(this.scrollR);
      this.groupBox2.Controls.Add(this.editBHex);
      this.groupBox2.Controls.Add(this.editB);
      this.groupBox2.Controls.Add(this.editGHex);
      this.groupBox2.Controls.Add(this.editG);
      this.groupBox2.Controls.Add(this.editRHex);
      this.groupBox2.Controls.Add(this.editR);
      this.groupBox2.Controls.Add(this.label3);
      this.groupBox2.Controls.Add(this.label2);
      this.groupBox2.Controls.Add(this.label1);
      this.groupBox2.Location = new System.Drawing.Point(470, 12);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(238, 175);
      this.groupBox2.TabIndex = 4;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Current Color";
      // 
      // scrollB
      // 
      this.scrollB.Location = new System.Drawing.Point(18, 146);
      this.scrollB.Maximum = 255;
      this.scrollB.Name = "scrollB";
      this.scrollB.Size = new System.Drawing.Size(214, 17);
      this.scrollB.TabIndex = 3;
      this.scrollB.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scrollB_Scroll);
      // 
      // scrollG
      // 
      this.scrollG.Location = new System.Drawing.Point(18, 97);
      this.scrollG.Maximum = 255;
      this.scrollG.Name = "scrollG";
      this.scrollG.Size = new System.Drawing.Size(214, 17);
      this.scrollG.TabIndex = 3;
      this.scrollG.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scrollG_Scroll);
      // 
      // scrollR
      // 
      this.scrollR.Location = new System.Drawing.Point(18, 48);
      this.scrollR.Maximum = 255;
      this.scrollR.Name = "scrollR";
      this.scrollR.Size = new System.Drawing.Size(214, 17);
      this.scrollR.TabIndex = 3;
      this.scrollR.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scrollR_Scroll);
      // 
      // editBHex
      // 
      this.editBHex.Location = new System.Drawing.Point(129, 123);
      this.editBHex.MaxLength = 2;
      this.editBHex.Name = "editBHex";
      this.editBHex.Size = new System.Drawing.Size(49, 20);
      this.editBHex.TabIndex = 2;
      this.editBHex.TextChanged += new System.EventHandler(this.editBHex_TextChanged);
      // 
      // editB
      // 
      this.editB.Location = new System.Drawing.Point(60, 123);
      this.editB.Name = "editB";
      this.editB.Size = new System.Drawing.Size(49, 20);
      this.editB.TabIndex = 2;
      this.editB.TextChanged += new System.EventHandler(this.editB_TextChanged);
      // 
      // editGHex
      // 
      this.editGHex.Location = new System.Drawing.Point(129, 74);
      this.editGHex.MaxLength = 2;
      this.editGHex.Name = "editGHex";
      this.editGHex.Size = new System.Drawing.Size(49, 20);
      this.editGHex.TabIndex = 2;
      this.editGHex.TextChanged += new System.EventHandler(this.editGHex_TextChanged);
      // 
      // editG
      // 
      this.editG.Location = new System.Drawing.Point(60, 74);
      this.editG.Name = "editG";
      this.editG.Size = new System.Drawing.Size(49, 20);
      this.editG.TabIndex = 2;
      this.editG.TextChanged += new System.EventHandler(this.editG_TextChanged);
      // 
      // editRHex
      // 
      this.editRHex.Location = new System.Drawing.Point(129, 22);
      this.editRHex.MaxLength = 2;
      this.editRHex.Name = "editRHex";
      this.editRHex.Size = new System.Drawing.Size(49, 20);
      this.editRHex.TabIndex = 2;
      this.editRHex.TextChanged += new System.EventHandler(this.editRHex_TextChanged);
      // 
      // editR
      // 
      this.editR.Location = new System.Drawing.Point(60, 22);
      this.editR.Name = "editR";
      this.editR.Size = new System.Drawing.Size(49, 20);
      this.editR.TabIndex = 2;
      this.editR.TextChanged += new System.EventHandler(this.editR_TextChanged);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(16, 126);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(31, 13);
      this.label3.TabIndex = 0;
      this.label3.Text = "Blue:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(16, 77);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(39, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "Green:";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(16, 25);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(30, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Red:";
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.panelColorPreview);
      this.groupBox3.Location = new System.Drawing.Point(470, 194);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(238, 66);
      this.groupBox3.TabIndex = 5;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Preview";
      // 
      // panelColorPreview
      // 
      this.panelColorPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panelColorPreview.Location = new System.Drawing.Point(6, 19);
      this.panelColorPreview.Name = "panelColorPreview";
      this.panelColorPreview.Size = new System.Drawing.Size(226, 38);
      this.panelColorPreview.TabIndex = 0;
      this.panelColorPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.panelColorPreview_Paint);
      // 
      // groupBox4
      // 
      this.groupBox4.Controls.Add(this.editPaletteName);
      this.groupBox4.Controls.Add(this.label4);
      this.groupBox4.Controls.Add(this.paletteList);
      this.groupBox4.Location = new System.Drawing.Point(12, 12);
      this.groupBox4.Name = "groupBox4";
      this.groupBox4.Size = new System.Drawing.Size(222, 248);
      this.groupBox4.TabIndex = 6;
      this.groupBox4.TabStop = false;
      this.groupBox4.Text = "Palettes";
      // 
      // editPaletteName
      // 
      this.editPaletteName.Location = new System.Drawing.Point(50, 222);
      this.editPaletteName.Name = "editPaletteName";
      this.editPaletteName.Size = new System.Drawing.Size(166, 20);
      this.editPaletteName.TabIndex = 9;
      this.editPaletteName.TextChanged += new System.EventHandler(this.editPaletteName_TextChanged);
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(6, 226);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(38, 13);
      this.label4.TabIndex = 8;
      this.label4.Text = "Name:";
      // 
      // paletteList
      // 
      this.paletteList.AddButtonEnabled = true;
      this.paletteList.AllowClone = true;
      this.paletteList.DeleteButtonEnabled = false;
      this.paletteList.HasOwnerDrawColumn = true;
      this.paletteList.HighlightColor = System.Drawing.SystemColors.HotTrack;
      this.paletteList.HighlightTextColor = System.Drawing.SystemColors.HighlightText;
      this.paletteList.Location = new System.Drawing.Point(6, 22);
      this.paletteList.MoveDownButtonEnabled = false;
      this.paletteList.MoveUpButtonEnabled = false;
      this.paletteList.MustHaveOneElement = true;
      this.paletteList.Name = "paletteList";
      this.paletteList.SelectedIndex = -1;
      this.paletteList.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      this.paletteList.SelectionTextColor = System.Drawing.SystemColors.HighlightText;
      this.paletteList.Size = new System.Drawing.Size(210, 189);
      this.paletteList.TabIndex = 7;
      this.paletteList.AddingItem += new C64Studio.ArrangedItemList.AddingItemEventHandler(this.paletteList_AddingItem);
      this.paletteList.CloningItem += new C64Studio.ArrangedItemList.CloningItemEventHandler(this.paletteList_CloningItem);
      this.paletteList.ItemRemoved += new C64Studio.ArrangedItemList.ItemModifiedEventHandler(this.paletteList_ItemRemoved);
      this.paletteList.ItemMoved += new C64Studio.ArrangedItemList.ItemExchangedEventHandler(this.paletteList_ItemMoved);
      this.paletteList.SelectedIndexChanged += new C64Studio.ArrangedItemList.ItemModifiedEventHandler(this.paletteList_SelectedIndexChanged);
      // 
      // DlgPaletteEditor
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(717, 300);
      this.Controls.Add(this.groupBox4);
      this.Controls.Add(this.groupBox3);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnCancel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "DlgPaletteEditor";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Palette Editor";
      this.groupBox1.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.groupBox3.ResumeLayout(false);
      this.groupBox4.ResumeLayout(false);
      this.groupBox4.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.ListBox listPalette;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.Panel panelColorPreview;
    private System.Windows.Forms.HScrollBar scrollB;
    private System.Windows.Forms.HScrollBar scrollG;
    private System.Windows.Forms.HScrollBar scrollR;
    private System.Windows.Forms.TextBox editBHex;
    private System.Windows.Forms.TextBox editB;
    private System.Windows.Forms.TextBox editGHex;
    private System.Windows.Forms.TextBox editG;
    private System.Windows.Forms.TextBox editRHex;
    private System.Windows.Forms.TextBox editR;
    private System.Windows.Forms.GroupBox groupBox4;
    private ArrangedItemList paletteList;
    private System.Windows.Forms.TextBox editPaletteName;
    private System.Windows.Forms.Label label4;
  }
}