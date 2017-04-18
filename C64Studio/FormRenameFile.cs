using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace C64Studio
{
  public partial class FormRenameFile : Form
  {
    public GR.Memory.ByteBuffer         Filename;



    public FormRenameFile( StudioCore Core, GR.Memory.ByteBuffer OrigFilename )
    {
      Filename = new GR.Memory.ByteBuffer( OrigFilename );

      InitializeComponent();

      editFilename.Text = Util.FilenameToUnicode( OrigFilename );
      editFilename.Font = new System.Drawing.Font( Core.MainForm.m_FontC64.Families[0], 16, System.Drawing.GraphicsUnit.Pixel );
    }



    private void btnOK_Click( object sender, EventArgs e )
    {
      Filename = Util.ToFilename( editFilename.Text );

      while ( ( Filename.Length > 0 )
      &&      ( Filename.ByteAt( (int)Filename.Length - 1 ) == 32 ) )
      {
        Filename.Truncate( 1 );
      }
      while ( Filename.Length < 16 )
      {
        Filename.AppendU8( 0xa0 );
      }

      DialogResult = DialogResult.OK;
      Close();
    }

  }
}
