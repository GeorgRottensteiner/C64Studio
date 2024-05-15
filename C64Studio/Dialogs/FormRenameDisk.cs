using RetroDevStudio.Types;
using RetroDevStudio;
using System;
using System.Windows.Forms;
using RetroDevStudio.Formats;

namespace RetroDevStudio.Dialogs
{
  public partial class FormRenameDisk : Form
  {
    public GR.Memory.ByteBuffer             DiskName;
    public GR.Memory.ByteBuffer             DiskID;
    private System.Drawing.Font             _DefaultFont;
    private MediaFilenameType               _FilenameType;
    StudioCore                              Core;
    bool                                    ActiveDiskName = true;



    public FormRenameDisk( StudioCore Core, MediaFilenameType FilenameType, GR.Memory.ByteBuffer OrigDiskName, GR.Memory.ByteBuffer OrigDiskID )
    {
      this.Core     = Core;
      _FilenameType = FilenameType;

      DiskName = new GR.Memory.ByteBuffer( OrigDiskName );
      DiskID = new GR.Memory.ByteBuffer( OrigDiskID );

      InitializeComponent();

      editDiskName.Text = Util.FilenameToUnicode( _FilenameType, DiskName );
      editDiskName.Font = new System.Drawing.Font( Core.MainForm.m_FontC64.Families[0], 16, System.Drawing.GraphicsUnit.Pixel );
      editDiskID.Text = Util.FilenameToUnicode( _FilenameType, DiskID );
      editDiskID.Font = new System.Drawing.Font( Core.MainForm.m_FontC64.Families[0], 16, System.Drawing.GraphicsUnit.Pixel );

      _DefaultFont = listPETSCII.Font;

      listPETSCII.ItemWidth = 80;
      listPETSCII.ItemHeight = 40;
      listPETSCII.SetDisplaySize( listPETSCII.ClientSize.Width, listPETSCII.ClientSize.Height );
      listPETSCII.DisplayPage.Create( 120, 120, GR.Drawing.PixelFormat.Format24bppRgb );
      listPETSCII.PixelFormat = GR.Drawing.PixelFormat.Format24bppRgb;
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



    private void btnOK_Click( DecentForms.ControlBase Sender )
    {
      DiskName = Util.ToPETSCII( editDiskName.Text );

      while ( ( DiskName.Length > 0 )
      &&      ( DiskName.ByteAt( (int)DiskName.Length - 1 ) == 32 ) )
      {
        DiskName.Truncate( 1 );
      }
      while ( DiskName.Length < 16 )
      {
        DiskName.AppendU8( 0xa0 );
      }

      DiskID = Util.ToPETSCII( editDiskID.Text );

      while ( ( DiskID.Length > 0 )
      &&      ( DiskID.ByteAt( (int)DiskName.Length - 1 ) == 32 ) )
      {
        DiskID.Truncate( 1 );
      }
      while ( DiskID.Length < 16 )
      {
        DiskID.AppendU8( 0xa0 );
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

      if ( ActiveDiskName )
      {
        editDiskName.CurrentChar = character.CharValue;
        ++editDiskName.CursorPos;
      }
      else
      {
        editDiskID.CurrentChar = character.CharValue;
        ++editDiskID.CursorPos;
      }
    }



    private void listPETSCII_SizeChanged( object sender, EventArgs e )
    {
      listPETSCII.SetDisplaySize( listPETSCII.ClientSize.Width, listPETSCII.ClientSize.Height );
    }



    private void editDiskName_KeyDown( object sender, KeyEventArgs e )
    {
      ActiveDiskName = true;
    }



    private void editDiskName_MouseDown( object sender, MouseEventArgs e )
    {
      ActiveDiskName = true;
    }



    private void editDiskID_KeyDown( object sender, KeyEventArgs e )
    {
      ActiveDiskName = false;
    }



    private void editDiskID_MouseDown( object sender, MouseEventArgs e )
    {
      ActiveDiskName = false;
    }



    private void btnCancel_Click( DecentForms.ControlBase Sender )
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }



  }
}
