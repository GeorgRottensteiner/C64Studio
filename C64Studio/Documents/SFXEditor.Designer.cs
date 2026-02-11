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
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            comboSFXPlayer = new System.Windows.Forms.ComboBox();
            label3 = new System.Windows.Forms.Label();
            editEffectName = new System.Windows.Forms.TextBox();
            listEffects = new RetroDevStudio.Controls.ArrangedItemList();
            panelEffectValues = new System.Windows.Forms.Panel();
            btnPlay = new DecentForms.Button();
            ( (System.ComponentModel.ISupportInitialize)m_FileWatcher ).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point( 9, 39 );
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size( 74, 13 );
            label1.TabIndex = 2;
            label1.Text = "Sound Effects";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point( 9, 9 );
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size( 39, 13 );
            label2.TabIndex = 2;
            label2.Text = "Player:";
            // 
            // comboSFXPlayer
            // 
            comboSFXPlayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboSFXPlayer.FormattingEnabled = true;
            comboSFXPlayer.Location = new System.Drawing.Point( 54, 6 );
            comboSFXPlayer.Name = "comboSFXPlayer";
            comboSFXPlayer.Size = new System.Drawing.Size( 387, 21 );
            comboSFXPlayer.TabIndex = 3;
            comboSFXPlayer.SelectedIndexChanged +=  comboSFXPlayer_SelectedIndexChanged ;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point( 9, 383 );
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size( 69, 13 );
            label3.TabIndex = 2;
            label3.Text = "Effect Name:";
            // 
            // editEffectName
            // 
            editEffectName.Location = new System.Drawing.Point( 12, 399 );
            editEffectName.Name = "editEffectName";
            editEffectName.Size = new System.Drawing.Size( 203, 20 );
            editEffectName.TabIndex = 4;
            editEffectName.TextChanged +=  editEffectName_TextChanged ;
            // 
            // listEffects
            // 
            listEffects.AddButtonEnabled = true;
            listEffects.AllowClone = true;
            listEffects.DeleteButtonEnabled = false;
            listEffects.HasOwnerDrawColumn = false;
            listEffects.HighlightColor = System.Drawing.SystemColors.HotTrack;
            listEffects.HighlightTextColor = System.Drawing.SystemColors.HighlightText;
            listEffects.Location = new System.Drawing.Point( 12, 55 );
            listEffects.MoveDownButtonEnabled = false;
            listEffects.MoveUpButtonEnabled = false;
            listEffects.MustHaveOneElement = false;
            listEffects.Name = "listEffects";
            listEffects.SelectedIndex = -1;
            listEffects.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            listEffects.SelectionTextColor = System.Drawing.SystemColors.HighlightText;
            listEffects.Size = new System.Drawing.Size( 203, 325 );
            listEffects.TabIndex = 5;
            listEffects.ItemAdded +=  listEffects_ItemAdded ;
            listEffects.ItemRemoved +=  listEffects_ItemRemoved ;
            listEffects.ItemMoved +=  listEffects_ItemMoved ;
            // 
            // panelEffectValues
            // 
            panelEffectValues.Location = new System.Drawing.Point( 230, 55 );
            panelEffectValues.Name = "panelEffectValues";
            panelEffectValues.Size = new System.Drawing.Size( 532, 433 );
            panelEffectValues.TabIndex = 6;
            // 
            // btnPlay
            // 
            btnPlay.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            btnPlay.BorderStyle = DecentForms.BorderStyle.FLAT;
            btnPlay.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
            btnPlay.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnPlay.Image = null;
            btnPlay.Location = new System.Drawing.Point( 12, 425 );
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new System.Drawing.Size( 75, 23 );
            btnPlay.TabIndex = 7;
            btnPlay.Text = "Play";
            btnPlay.Click +=  btnPlay_Click ;
            // 
            // SFXEditor
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            ClientSize = new System.Drawing.Size( 774, 500 );
            Controls.Add( btnPlay );
            Controls.Add( panelEffectValues );
            Controls.Add( listEffects );
            Controls.Add( editEffectName );
            Controls.Add( comboSFXPlayer );
            Controls.Add( label2 );
            Controls.Add( label3 );
            Controls.Add( label1 );
            Icon = (System.Drawing.Icon)resources.GetObject( "$this.Icon" );
            MinimumSize = new System.Drawing.Size( 274, 159 );
            Name = "SFXEditor";
            Text = "SFX Editor";
            ( (System.ComponentModel.ISupportInitialize)m_FileWatcher ).EndInit();
            ResumeLayout( false );
            PerformLayout();

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
