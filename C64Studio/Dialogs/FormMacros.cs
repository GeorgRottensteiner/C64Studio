using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RetroDevStudio.Dialogs
{
  public partial class FormMacros : Form
  {
    private DocumentInfo      m_Doc = null;
    private TextBox           m_Edit = null;
    private StudioCore        Core = null;
    private string            _AppearanceKey = "";



    public FormMacros( StudioCore Core, DocumentInfo Doc, TextBox EditToInsert, bool ShowRunCommands, string AppearanceKey )
    {
      InitializeComponent();

      _AppearanceKey = AppearanceKey;
      m_Doc = Doc;
      m_Edit = EditToInsert;
      this.Core = Core;

      InsertMacro( "$(Filename)" );
      InsertMacro( "$(File)" );
      InsertMacro( "$(FilenameWithoutExtension)" );
      InsertMacro( "$(FilePath)" );
      InsertMacro( "$(BuildTargetPath)" );
      InsertMacro( "$(BuildTargetFilename)" );
      InsertMacro( "$(BuildTargetFilenameWithoutExtension)" );
      InsertMacro( "$(BuildTargetFile)" );
      InsertMacro( "$(BuildTargetFileWithoutExtension)" );
      if ( ShowRunCommands )
      {
        InsertMacro( "$(RunPath)" );
        InsertMacro( "$(RunFilename)" );
        InsertMacro( "$(RunFile)" );
        InsertMacro( "$(RunFilenameWithoutExtension)" );
      }
      InsertMacro( "$(DebugStartAddress)" );
      InsertMacro( "$(DebugStartAddressHex)" );

      InsertMacro( "$(ConfigName)" );
      InsertMacro( "$(ProjectPath)" );
      InsertMacro( "$(SolutionPath)" );
      InsertMacro( "$(MediaManager)" );
      InsertMacro( "$(MediaTool)" );

      btnInsert.Visible = ( m_Edit != null );

      Core.Theming.ApplyTheme( this );
    }



    private void InsertMacro( string Macro )
    {
      bool    error = false;

      ListViewItem    item = new ListViewItem( Macro );
      item.SubItems.Add( Core.MainForm.FillParameters( Macro, m_Doc, false, out error ) );

      listMacros.Items.Add( item );
    }



    private void listMacros_ItemActivate( object sender, EventArgs e )
    {
      if ( listMacros.SelectedIndices.Count == 0 )
      {
        return;
      }
      InsertMacro();
    }



    private void InsertMacro()
    {
      int     caretPos = m_Edit.SelectionStart;
      m_Edit.Text = m_Edit.Text.Insert( m_Edit.SelectionStart, listMacros.SelectedItems[0].SubItems[0].Text );
      m_Edit.SelectionStart   = caretPos + listMacros.SelectedItems[0].SubItems[0].Text.Length;
      m_Edit.SelectionLength  = 0;
    }



    private void btnInsert_Click( DecentForms.ControlBase Sender )
    {
      if ( listMacros.SelectedIndices.Count == 0 )
      {
        return;
      }
      InsertMacro();
    }



    private void btnOK_Click( DecentForms.ControlBase Sender )
    {
      Close();
    }



    private void FormMacros_FormClosing( object sender, FormClosingEventArgs e )
    {
      Core.Settings.DialogSettings.StoreAppearance( _AppearanceKey, this );
      Core.Settings.DialogSettings.StoreListViewColumns( _AppearanceKey, listMacros );
    }



    private void FormMacros_Load( object sender, EventArgs e )
    {
      Core.Settings.DialogSettings.RestoreAppearance( _AppearanceKey, this );
      Core.Settings.DialogSettings.RestoreListViewColumns( _AppearanceKey, listMacros );
    }



  }
}
