using RetroDevStudio.Formats;
using RetroDevStudio.Documents;
using GR.Generic;
using GR.Image;



namespace RetroDevStudio.Controls
{
  public partial class ImportCharscreenCharsetFromDefault : ImportCharscreenFormBase
  {
    public ImportCharscreenCharsetFromDefault() :
      base( null )
    { 
    }



    public ImportCharscreenCharsetFromDefault( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();

      comboImportFromDefault.Items.Add( new Tupel<string, GR.Memory.ByteBuffer>( "C64 Uppercase", ConstantData.UpperCaseCharsetC64 ) );
      comboImportFromDefault.Items.Add( new Tupel<string, GR.Memory.ByteBuffer>( "C64 Lowercase", ConstantData.LowerCaseCharsetC64 ) );
      comboImportFromDefault.Items.Add( new Tupel<string, GR.Memory.ByteBuffer>( "VIC20 Uppercase", ConstantData.UpperCaseCharsetViC20 ) );
      comboImportFromDefault.Items.Add( new Tupel<string, GR.Memory.ByteBuffer>( "VIC20 Lowercase", ConstantData.LowerCaseCharsetViC20 ) );
      comboImportFromDefault.Items.Add( new Tupel<string, GR.Memory.ByteBuffer>( "Commander X16 Uppercase", ConstantData.UpperCaseCharsetCommanderX16 ) );
      comboImportFromDefault.Items.Add( new Tupel<string, GR.Memory.ByteBuffer>( "Commander X16 Lowercase", ConstantData.LowerCaseCharsetCommanderX16 ) );
      comboImportFromDefault.Items.Add( new Tupel<string, GR.Memory.ByteBuffer>( "Commander X16 ISO8859", ConstantData.ISO8859CommanderX16 ) );

      comboImportFromDefault.SelectedIndex = 0;
    }



    public override bool HandleImport( CharsetScreenProject CharScreen, CharsetScreenEditor Editor )
    {
      if ( comboImportFromDefault.SelectedItem == null )
      {
        return false;
      }
      GR.Memory.ByteBuffer imageSource = ( (Tupel<string, GR.Memory.ByteBuffer>)comboImportFromDefault.SelectedItem ).second;

      var image = new MemoryImage( 256 * 8, 8, GR.Drawing.PixelFormat.Format8bppIndexed );
      image.SetPaletteColor( 0, 0, 0, 0 );
      image.SetPaletteColor( 1, 255, 255, 255 );
      for ( int c = 0; c < 256; ++c )
      {
        for ( int i = 0; i < 8; ++i )
        {
          for ( int j = 0; j < 8; ++j )
          {
            if ( ( imageSource.ByteAt( c * 8 + i ) & ( 1 << ( 7 - j ) ) ) != 0 )
            {
              image.SetPixel( c * 8 + j, i, 1 );
            }
          }
        }
      }

      // make up fake image
      Editor.ImportCharsetFromImage( image );
      /*
      switch ( comboImportFromDefault.SelectedIndex )
      {
        case 0:
          for ( int i = 0; i < 256; ++i )
          {
            for ( int j = 0; j < 8; ++j )
            {
              CharScreen.CharSet.Characters[i].Tile.Data.SetU8At( j, ConstantData.UpperCaseCharsetC64.ByteAt( i * 8 + j ) );
            }
            CharScreen.CharSet.Characters[i].Tile.CustomColor = 1;
          }
          Editor.CharsetChanged();
          return true;
        case 1:
          for ( int i = 0; i < 256; ++i )
          {
            for ( int j = 0; j < 8; ++j )
            {
              CharScreen.CharSet.Characters[i].Tile.Data.SetU8At( j, ConstantData.LowerCaseCharsetC64.ByteAt( i * 8 + j ) );
            }
            CharScreen.CharSet.Characters[i].Tile.CustomColor = 1;
          }
          Editor.CharsetChanged();
          return true;
        case 2:
          for ( int i = 0; i < 256; ++i )
          {
            for ( int j = 0; j < 8; ++j )
            {
              CharScreen.CharSet.Characters[i].Tile.Data.SetU8At( j, ConstantData.UpperCaseCharsetViC20.ByteAt( i * 8 + j ) );
            }
            CharScreen.CharSet.Characters[i].Tile.CustomColor = 1;
          }
          Editor.CharsetChanged();
          return true;
        case 3:
          for ( int i = 0; i < 256; ++i )
          {
            for ( int j = 0; j < 8; ++j )
            {
              CharScreen.CharSet.Characters[i].Tile.Data.SetU8At( j, ConstantData.LowerCaseCharsetViC20.ByteAt( i * 8 + j ) );
            }
            CharScreen.CharSet.Characters[i].Tile.CustomColor = 1;
          }
          Editor.CharsetChanged();
          return true;
        case 4:
          for ( int i = 0; i < 256; ++i )
          {
            for ( int j = 0; j < 8; ++j )
            {
              CharScreen.CharSet.Characters[i].Tile.Data.SetU8At( j, ConstantData.UpperCaseCharsetCommanderX16.ByteAt( i * 8 + j ) );
            }
            CharScreen.CharSet.Characters[i].Tile.CustomColor = 1;
          }
          Editor.CharsetChanged();
          return true;
        case 5:
          for ( int i = 0; i < 256; ++i )
          {
            for ( int j = 0; j < 8; ++j )
            {
              CharScreen.CharSet.Characters[i].Tile.Data.SetU8At( j, ConstantData.LowerCaseCharsetCommanderX16.ByteAt( i * 8 + j ) );
            }
            CharScreen.CharSet.Characters[i].Tile.CustomColor = 1;
          }
          Editor.CharsetChanged();
          return true;
        case 6:
          for ( int i = 0; i < 256; ++i )
          {
            for ( int j = 0; j < 8; ++j )
            {
              CharScreen.CharSet.Characters[i].Tile.Data.SetU8At( j, ConstantData.ISO8859CommanderX16.ByteAt( i * 8 + j ) );
            }
            CharScreen.CharSet.Characters[i].Tile.CustomColor = 1;
          }
          Editor.CharsetChanged();
          return true;
      }*/
      return false;
    }



  }
}
