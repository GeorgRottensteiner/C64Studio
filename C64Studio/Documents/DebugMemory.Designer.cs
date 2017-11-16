﻿namespace C64Studio
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
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      this.toolDebugMemory.SuspendLayout();
      this.SuspendLayout();
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
      this.hexView.Name = "hexView";
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
            this.toolStripBtnHexCaseSwitch});
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
      this.toolStripBtnGoto.Image = global::C64Studio.Properties.Resources.DebugMemoryGoto;
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
  }
}
