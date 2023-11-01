﻿namespace RetroDevStudio.Documents
{
  partial class DebugMemory
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
      System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
      System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
      System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
      System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
      System.Windows.Forms.ToolStripMenuItem toolStripMenuItem9;
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugMemory));
      this.hexView = new Be.Windows.Forms.HexBox();
      this.toolDebugMemory = new System.Windows.Forms.ToolStrip();
      this.btnBinaryStringView = new System.Windows.Forms.ToolStripButton();
      this.btnBinaryCharView = new System.Windows.Forms.ToolStripButton();
      this.btnBinarySpriteView = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripBtnMemoryFromCPU = new System.Windows.Forms.ToolStripButton();
      this.toolStripBtnGoto = new System.Windows.Forms.ToolStripButton();
      this.toolStripBtnHexCaseSwitch = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripBtnAddView = new System.Windows.Forms.ToolStripButton();
      this.byteWidthDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
      toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
      toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
      toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
      toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
      toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      this.toolDebugMemory.SuspendLayout();
      this.SuspendLayout();
      // 
      // toolStripMenuItem5
      // 
      toolStripMenuItem5.AutoSize = false;
      toolStripMenuItem5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      toolStripMenuItem5.Name = "toolStripMenuItem5";
      toolStripMenuItem5.Size = new System.Drawing.Size(100, 34);
      toolStripMenuItem5.Text = "8";
      // 
      // toolStripMenuItem6
      // 
      toolStripMenuItem6.AutoSize = false;
      toolStripMenuItem6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      toolStripMenuItem6.Name = "toolStripMenuItem6";
      toolStripMenuItem6.Size = new System.Drawing.Size(100, 34);
      toolStripMenuItem6.Text = "16";
      // 
      // toolStripMenuItem7
      // 
      toolStripMenuItem7.AutoSize = false;
      toolStripMenuItem7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      toolStripMenuItem7.Name = "toolStripMenuItem7";
      toolStripMenuItem7.Size = new System.Drawing.Size(100, 34);
      toolStripMenuItem7.Text = "24";
      // 
      // toolStripMenuItem8
      // 
      toolStripMenuItem8.AutoSize = false;
      toolStripMenuItem8.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      toolStripMenuItem8.Name = "toolStripMenuItem8";
      toolStripMenuItem8.Size = new System.Drawing.Size(100, 34);
      toolStripMenuItem8.Text = "32";
      // 
      // toolStripMenuItem9
      // 
      toolStripMenuItem9.AutoSize = false;
      toolStripMenuItem9.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      toolStripMenuItem9.Name = "toolStripMenuItem9";
      toolStripMenuItem9.Size = new System.Drawing.Size(100, 34);
      toolStripMenuItem9.Text = "40";
      //
      // hexView
      // 
      this.hexView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.hexView.BytesPerLine = 8;
      this.hexView.ColumnInfoVisible = true;
      this.hexView.CustomHexViewer = null;
      this.hexView.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.hexView.InfoForeColor = System.Drawing.SystemColors.AppWorkspace;
      this.hexView.LineInfoVisible = true;
      this.hexView.Location = new System.Drawing.Point(0, 28);
      this.hexView.MarkedForeColor = System.Drawing.Color.Empty;
      this.hexView.Name = "hexView";
      this.hexView.NumDigitsMemorySize = 4;
      this.hexView.ReadOnly = true;
      this.hexView.SelectedByteProvider = null;
      this.hexView.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
      this.hexView.Size = new System.Drawing.Size(431, 457);
      this.hexView.StringViewVisible = true;
      this.hexView.TabIndex = 1;
      this.hexView.TextFont = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.hexView.UseFixedBytesPerLine = true;
      this.hexView.VScrollBarVisible = true;
      // 
      // toolDebugMemory
      // 
      this.toolDebugMemory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnBinaryStringView,
            this.btnBinaryCharView,
            this.btnBinarySpriteView,
            this.toolStripSeparator1,
            this.toolStripBtnMemoryFromCPU,
            this.toolStripBtnGoto,
            this.toolStripBtnHexCaseSwitch,
			this.byteWidthDropDownButton,
            this.toolStripSeparator2,
            this.toolStripBtnAddView});
      this.toolDebugMemory.Location = new System.Drawing.Point(0, 0);
      this.toolDebugMemory.Name = "toolDebugMemory";
      this.toolDebugMemory.Size = new System.Drawing.Size(431, 25);
      this.toolDebugMemory.TabIndex = 2;
      this.toolDebugMemory.Text = "toolStrip1";
      // 
      // btnBinaryStringView
      // 
      this.btnBinaryStringView.Checked = true;
      this.btnBinaryStringView.CheckState = System.Windows.Forms.CheckState.Checked;
      this.btnBinaryStringView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.btnBinaryStringView.Image = ((System.Drawing.Image)(resources.GetObject("btnBinaryStringView.Image")));
      this.btnBinaryStringView.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.btnBinaryStringView.Name = "btnBinaryStringView";
      this.btnBinaryStringView.Size = new System.Drawing.Size(23, 22);
      this.btnBinaryStringView.Text = "Set String View";
      this.btnBinaryStringView.ToolTipText = "View as PETSCII";
      this.btnBinaryStringView.Click += new System.EventHandler(this.btnBinaryStringView_Click);
      // 
      // btnBinaryCharView
      // 
      this.btnBinaryCharView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.btnBinaryCharView.Image = ((System.Drawing.Image)(resources.GetObject("btnBinaryCharView.Image")));
      this.btnBinaryCharView.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.btnBinaryCharView.Name = "btnBinaryCharView";
      this.btnBinaryCharView.Size = new System.Drawing.Size(23, 22);
      this.btnBinaryCharView.Text = "toolStripButton1";
      this.btnBinaryCharView.ToolTipText = "View as Characters";
      this.btnBinaryCharView.Click += new System.EventHandler(this.btnBinaryCharView_Click);
      // 
      // btnBinarySpriteView
      // 
      this.btnBinarySpriteView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.btnBinarySpriteView.Image = ((System.Drawing.Image)(resources.GetObject("btnBinarySpriteView.Image")));
      this.btnBinarySpriteView.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.btnBinarySpriteView.Name = "btnBinarySpriteView";
      this.btnBinarySpriteView.Size = new System.Drawing.Size(23, 22);
      this.btnBinarySpriteView.Text = "toolStripButton1";
      this.btnBinarySpriteView.ToolTipText = "View as Sprites";
      this.btnBinarySpriteView.Click += new System.EventHandler(this.btnBinarySpriteView_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
      // 
      // toolStripBtnMemoryFromCPU
      // 
      this.toolStripBtnMemoryFromCPU.Checked = true;
      this.toolStripBtnMemoryFromCPU.CheckOnClick = true;
      this.toolStripBtnMemoryFromCPU.CheckState = System.Windows.Forms.CheckState.Checked;
      this.toolStripBtnMemoryFromCPU.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripBtnMemoryFromCPU.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnMemoryFromCPU.Image")));
      this.toolStripBtnMemoryFromCPU.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripBtnMemoryFromCPU.Name = "toolStripBtnMemoryFromCPU";
      this.toolStripBtnMemoryFromCPU.Size = new System.Drawing.Size(23, 22);
      this.toolStripBtnMemoryFromCPU.ToolTipText = "Show Memory as CPU sees it";
      this.toolStripBtnMemoryFromCPU.Click += new System.EventHandler(this.toolStripBtnMemoryFromCPU_Click);
      // 
      // toolStripBtnGoto
      // 
      this.toolStripBtnGoto.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripBtnGoto.Image = global::RetroDevStudio.Properties.Resources.DebugMemoryGoto;
      this.toolStripBtnGoto.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripBtnGoto.Name = "toolStripBtnGoto";
      this.toolStripBtnGoto.Size = new System.Drawing.Size(23, 22);
      this.toolStripBtnGoto.Text = "Goto Address";
      this.toolStripBtnGoto.Click += new System.EventHandler(this.toolStripButtonGoto_Click);
      // 
      // toolStripBtnHexCaseSwitch
      // 
      this.toolStripBtnHexCaseSwitch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripBtnHexCaseSwitch.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnHexCaseSwitch.Image")));
      this.toolStripBtnHexCaseSwitch.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripBtnHexCaseSwitch.Name = "toolStripBtnHexCaseSwitch";
      this.toolStripBtnHexCaseSwitch.Size = new System.Drawing.Size(23, 22);
      this.toolStripBtnHexCaseSwitch.Text = "Switch Capital";
      this.toolStripBtnHexCaseSwitch.Click += new System.EventHandler(this.toolStripBtnHexCaseSwitch_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
      // 
      // toolStripBtnAddView
      // 
      this.toolStripBtnAddView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripBtnAddView.Image = global::RetroDevStudio.Properties.Resources.add;
      this.toolStripBtnAddView.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripBtnAddView.Name = "toolStripBtnAddView";
      this.toolStripBtnAddView.Size = new System.Drawing.Size(23, 22);
      this.toolStripBtnAddView.Text = "Add View";
      this.toolStripBtnAddView.Click += new System.EventHandler(this.toolStripBtnAddView_Click);
      // 
      // byteWidthDropDownButton
      // 
      this.byteWidthDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.byteWidthDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem5, toolStripMenuItem6, toolStripMenuItem7, toolStripMenuItem8, toolStripMenuItem9 });
      this.byteWidthDropDownButton.Image = Properties.Resources.tool_select;
      this.byteWidthDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.byteWidthDropDownButton.Name = "byteWidthDropDownButton";
      this.byteWidthDropDownButton.Size = new System.Drawing.Size(42, 28);
      this.byteWidthDropDownButton.ToolTipText = "Bytes per line";
      this.byteWidthDropDownButton.DropDownItemClicked += byteWidthDropDownButton_DropDownItemClicked;
      // DebugMemory
      // 
      this.ClientSize = new System.Drawing.Size(431, 485);
      this.Controls.Add(this.toolDebugMemory);
      this.Controls.Add(this.hexView);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "DebugMemory";
      this.Text = "Memory";
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      this.toolDebugMemory.ResumeLayout(false);
      this.toolDebugMemory.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

        #endregion

        public Be.Windows.Forms.HexBox hexView;
        private System.Windows.Forms.ToolStrip toolDebugMemory;
        private System.Windows.Forms.ToolStripButton btnBinaryStringView;
        private System.Windows.Forms.ToolStripButton btnBinaryCharView;
        private System.Windows.Forms.ToolStripButton btnBinarySpriteView;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripBtnMemoryFromCPU;
        private System.Windows.Forms.ToolStripButton toolStripBtnGoto;
        private System.Windows.Forms.ToolStripButton toolStripBtnHexCaseSwitch;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripBtnAddView;
        private System.Windows.Forms.ToolStripDropDownButton byteWidthDropDownButton;
    }
}
