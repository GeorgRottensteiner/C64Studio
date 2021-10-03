using C64Studio.Types;
using RetroDevStudio;
using System;
using System.Windows.Forms;



namespace C64Studio
{
  public partial class FormRenameFile : Form
  {
    public GR.Memory.ByteBuffer             Filename;
    private System.Drawing.Font             _DefaultFont;
    StudioCore                              Core;



    public FormRenameFile( StudioCore Core, GR.Memory.ByteBuffer OrigFilename )
    {
      this.Core = Core;

      Filename = new GR.Memory.ByteBuffer( OrigFilename );

      InitializeComponent();

      editFilename.Text = Util.FilenameToUnicode( OrigFilename );
      editFilename.Font = new System.Drawing.Font( Core.MainForm.m_FontC64.Families[0], 16, System.Drawing.GraphicsUnit.Pixel );

      _DefaultFont = listPETSCII.Font;

      listPETSCII.ItemWidth = 80;
      listPETSCII.ItemHeight = 40;
      listPETSCII.SetDisplaySize( listPETSCII.ClientSize.Width, listPETSCII.ClientSize.Height );
      listPETSCII.DisplayPage.Create( 120, 120, System.Drawing.Imaging.PixelFormat.Format24bppRgb );
      listPETSCII.PixelFormat = System.Drawing.Imaging.PixelFormat.Format24bppRgb;
      listPETSCII.Font = new System.Drawing.Font( Core.MainForm.m_FontC64.Families[0], Core.Settings.SourceFontSize, Core.Settings.SourceFontStyle );

      foreach ( Types.C64Character character in ConstantData.PetSCIIToChar.Values )
      {
        if ( character.HasChar )
        {
          listPETSCII.Items.Add( CreateItem( character ) );
        }
      }

      Core.Theming.ApplyTheme( this );
    }



    private GR.Forms.ImageListbox.ImageListItem CreateItem( C64Character character )
    {
      var item = new GR.Forms.ImageListbox.ImageListItem( listPETSCII );

      item.Value = character;
      item.Image = CreateImageForCharacter( character );

      return item;
    }



    private System.Drawing.Image CreateImageForCharacter( C64Character character )
    {
      var bitmap = new System.Drawing.Bitmap( 80, 40, System.Drawing.Imaging.PixelFormat.Format24bppRgb );
      var g = System.Drawing.Graphics.FromImage( bitmap );

      System.Drawing.Brush  brushBackground = new System.Drawing.SolidBrush( GR.Color.Helper.FromARGB( Core.Settings.BGColor( ColorableElement.BACKGROUND_CONTROL ) ) );
      System.Drawing.Pen  penBorder = new System.Drawing.Pen( GR.Color.Helper.FromARGB( Core.Settings.FGColor( ColorableElement.CONTROL_TEXT ) ) );

      g.FillRectangle( brushBackground, 0, 0, 80, 40 );
      g.DrawRectangle( penBorder, 0, 0, 80, 40 );

      System.Drawing.Brush  brush = new System.Drawing.SolidBrush( GR.Color.Helper.FromARGB( Core.Settings.FGColor( ColorableElement.CONTROL_TEXT ) ) );

      g.DrawString( "" + character.CharValue, listPETSCII.Font, brush, new System.Drawing.PointF( 2, 4 ) );
      g.DrawString( character.PetSCIIValue.ToString( "X02" ), _DefaultFont, brush, new System.Drawing.PointF( 40, 4 ) );
      g.DrawString( character.Desc, _DefaultFont, brush, new System.Drawing.PointF( 0, 24 ) );

      g.Dispose();

      return bitmap;
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



    private void listPETSCII_MouseDoubleClick( object sender, MouseEventArgs e )
    {
      if ( listPETSCII.SelectedIndex == -1 )
      {
        return;
      }
      Types.C64Character character = (Types.C64Character)listPETSCII.Items[listPETSCII.SelectedIndex].Value;

      editFilename.CurrentChar = character.CharValue;
      ++editFilename.CursorPos;
    }



    private void listPETSCII_SizeChanged( object sender, EventArgs e )
    {
      listPETSCII.SetDisplaySize( listPETSCII.ClientSize.Width, listPETSCII.ClientSize.Height );
    }



  }
}
