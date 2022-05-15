using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RetroDevStudio
{
  public partial class FormFilesChanged : Form
  {
    public List<DocumentInfo>        ChangedDocuments = new List<DocumentInfo>();



    public FormFilesChanged( DocumentInfo Document, StudioCore Core )
    {
      ChangedDocuments.Add( Document );
      InitializeComponent();

      Core.Theming.ApplyTheme( this );
    }



    public void AddChangedFile( DocumentInfo Document )
    {
      if ( ChangedDocuments.Contains( Document ) )
      {
        return;
      }
      ChangedDocuments.Add( Document );
      if ( listChangedFiles != null )
      {
        listChangedFiles.Items.Add( Document.FullPath );
      }
    }



    private void btnOK_Click( object sender, EventArgs e )
    {
      DialogResult = DialogResult.OK;
      Close();
    }



    private string FilterString( string Source )
    {
      return Source.Substring( 0, Source.Length - 1 );
    }



    private void FormFilesChanged_Load( object sender, EventArgs e )
    {
      foreach ( var doc in ChangedDocuments )
      {
        listChangedFiles.Items.Add( doc.FullPath );
      }
    }



  }
}
