namespace C64Studio
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
            this.btnBinarySpriteView});
      this.toolDebugMemory.Location = new System.Drawing.Point(0, 0);
      this.toolDebugMemory.Name = "toolDebugMemory";
      this.toolDebugMemory.Size = new System.Drawing.Size(431, 25);
      this.toolDebugMemory.TabIndex = 2;
      this.toolDebugMemory.Text = "toolStrip1";
      // 
      // btnBinaryStringView
      // 
      this.btnBinaryStringView.Checked = true;
      this.btnBinaryStringView.CheckOnClick = true;
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
      this.btnBinaryCharView.CheckOnClick = true;
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
      this.btnBinarySpriteView.CheckOnClick = true;
      this.btnBinarySpriteView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.btnBinarySpriteView.Image = ((System.Drawing.Image)(resources.GetObject("btnBinarySpriteView.Image")));
      this.btnBinarySpriteView.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.btnBinarySpriteView.Name = "btnBinarySpriteView";
      this.btnBinarySpriteView.Size = new System.Drawing.Size(23, 22);
      this.btnBinarySpriteView.Text = "toolStripButton1";
      this.btnBinarySpriteView.ToolTipText = "View as Sprites";
      this.btnBinarySpriteView.Click += new System.EventHandler(this.btnBinarySpriteView_Click);
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


  }
}
