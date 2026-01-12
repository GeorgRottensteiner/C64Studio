namespace RetroDevStudio.Documents
{
  partial class SFXEditor
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SFXEditor));
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.comboSFXPlayer = new System.Windows.Forms.ComboBox();
      this.label3 = new System.Windows.Forms.Label();
      this.editEffectName = new System.Windows.Forms.TextBox();
      this.listEffects = new RetroDevStudio.Controls.ArrangedItemList();
      this.panelEffectValues = new System.Windows.Forms.Panel();
      this.btnPlay = new DecentForms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(9, 39);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(74, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Sound Effects";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(9, 9);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(39, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Player:";
      // 
      // comboSFXPlayer
      // 
      this.comboSFXPlayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboSFXPlayer.FormattingEnabled = true;
      this.comboSFXPlayer.Location = new System.Drawing.Point(54, 6);
      this.comboSFXPlayer.Name = "comboSFXPlayer";
      this.comboSFXPlayer.Size = new System.Drawing.Size(161, 21);
      this.comboSFXPlayer.TabIndex = 3;
      this.comboSFXPlayer.SelectedIndexChanged += new System.EventHandler(this.comboSFXPlayer_SelectedIndexChanged);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(9, 383);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(69, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "Effect Name:";
      // 
      // editEffectName
      // 
      this.editEffectName.Location = new System.Drawing.Point(12, 399);
      this.editEffectName.Name = "editEffectName";
      this.editEffectName.Size = new System.Drawing.Size(203, 20);
      this.editEffectName.TabIndex = 4;
      this.editEffectName.TextChanged += new System.EventHandler(this.editEffectName_TextChanged);
      // 
      // listEffects
      // 
      this.listEffects.AddButtonEnabled = true;
      this.listEffects.AllowClone = true;
      this.listEffects.DeleteButtonEnabled = false;
      this.listEffects.HasOwnerDrawColumn = false;
      this.listEffects.HighlightColor = System.Drawing.SystemColors.HotTrack;
      this.listEffects.HighlightTextColor = System.Drawing.SystemColors.HighlightText;
      this.listEffects.Location = new System.Drawing.Point(12, 55);
      this.listEffects.MoveDownButtonEnabled = false;
      this.listEffects.MoveUpButtonEnabled = false;
      this.listEffects.MustHaveOneElement = false;
      this.listEffects.Name = "listEffects";
      this.listEffects.SelectedIndex = -1;
      this.listEffects.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      this.listEffects.SelectionTextColor = System.Drawing.SystemColors.HighlightText;
      this.listEffects.Size = new System.Drawing.Size(203, 325);
      this.listEffects.TabIndex = 5;
      this.listEffects.ItemAdded += new RetroDevStudio.Controls.ArrangedItemList.ItemModifiedEventHandler(this.listEffects_ItemAdded);
      this.listEffects.ItemRemoved += new RetroDevStudio.Controls.ArrangedItemList.ItemModifiedEventHandler(this.listEffects_ItemRemoved);
      this.listEffects.ItemMoved += new RetroDevStudio.Controls.ArrangedItemList.ItemExchangedEventHandler(this.listEffects_ItemMoved);
      // 
      // panelEffectValues
      // 
      this.panelEffectValues.Location = new System.Drawing.Point(230, 6);
      this.panelEffectValues.Name = "panelEffectValues";
      this.panelEffectValues.Size = new System.Drawing.Size(532, 482);
      this.panelEffectValues.TabIndex = 6;
      // 
      // btnPlay
      // 
      this.btnPlay.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnPlay.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnPlay.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnPlay.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnPlay.Image = null;
      this.btnPlay.Location = new System.Drawing.Point(12, 425);
      this.btnPlay.Name = "btnPlay";
      this.btnPlay.Size = new System.Drawing.Size(75, 23);
      this.btnPlay.TabIndex = 7;
      this.btnPlay.Text = "Play";
      this.btnPlay.Click += new DecentForms.EventHandler(this.btnPlay_Click);
      // 
      // SFXEditor
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.ClientSize = new System.Drawing.Size(774, 500);
      this.Controls.Add(this.btnPlay);
      this.Controls.Add(this.panelEffectValues);
      this.Controls.Add(this.listEffects);
      this.Controls.Add(this.editEffectName);
      this.Controls.Add(this.comboSFXPlayer);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimumSize = new System.Drawing.Size(274, 159);
      this.Name = "SFXEditor";
      this.Text = "SFX Editor";
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }


        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboSFXPlayer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox editEffectName;
        private Controls.ArrangedItemList listEffects;
        private System.Windows.Forms.Panel panelEffectValues;
        private DecentForms.Button btnPlay;
    }
}
