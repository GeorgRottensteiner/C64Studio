using RetroDevStudio.Formats;
using RetroDevStudio.Documents;



namespace RetroDevStudio.Controls
{
  public partial class ImportCharsetFromDefault : ImportCharsetFormBase
  {
    public ImportCharsetFromDefault() :
      base( null )
    { 
    }



    public ImportCharsetFromDefault( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();

      comboImportFromDefault.Items.Add( "C64 Uppercase" );
      comboImportFromDefault.Items.Add( "C64 Lowercase" );
      comboImportFromDefault.Items.Add( "VIC20 Uppercase" );
      comboImportFromDefault.Items.Add( "VIC20 Lowercase" );
      comboImportFromDefault.Items.Add( "Commander X16 Uppercase" );
      comboImportFromDefault.Items.Add( "Commander X16 Lowercase" );

      comboImportFromDefault.SelectedIndex = 0;
    }



    public override bool HandleImport( CharsetProject Charset, CharsetEditor Editor )
    {
      switch ( comboImportFromDefault.SelectedIndex )
      {
        case 0:
          for ( int i = 0; i < 256; ++i )
          {
            for ( int j = 0; j < 8; ++j )
            {
              Charset.Characters[i].Tile.Data.SetU8At( j, ConstantData.UpperCaseCharsetC64.ByteAt( i * 8 + j ) );
            }
            Charset.Characters[i].Tile.CustomColor = 1;
          }
          Editor.characterEditor.CharsetUpdated( Charset );
          return true;
        case 1:
          for ( int i = 0; i < 256; ++i )
          {
            for ( int j = 0; j < 8; ++j )
            {
              Charset.Characters[i].Tile.Data.SetU8At( j, ConstantData.LowerCaseCharsetC64.ByteAt( i * 8 + j ) );
            }
            Charset.Characters[i].Tile.CustomColor = 1;
          }
          Editor.characterEditor.CharsetUpdated( Charset );
          return true;
        case 2:
          for ( int i = 0; i < 256; ++i )
          {
            for ( int j = 0; j < 8; ++j )
            {
              Charset.Characters[i].Tile.Data.SetU8At( j, ConstantData.UpperCaseCharsetViC20.ByteAt( i * 8 + j ) );
            }
            Charset.Characters[i].Tile.CustomColor = 1;
          }
          Editor.characterEditor.CharsetUpdated( Charset );
          return true;
        case 3:
          for ( int i = 0; i < 256; ++i )
          {
            for ( int j = 0; j < 8; ++j )
            {
              Charset.Characters[i].Tile.Data.SetU8At( j, ConstantData.LowerCaseCharsetViC20.ByteAt( i * 8 + j ) );
            }
            Charset.Characters[i].Tile.CustomColor = 1;
          }
          Editor.characterEditor.CharsetUpdated( Charset );
          return true;
        case 4:
          for ( int i = 0; i < 256; ++i )
          {
            for ( int j = 0; j < 8; ++j )
            {
              Charset.Characters[i].Tile.Data.SetU8At( j, ConstantData.UpperCaseCharsetCommanderX16.ByteAt( i * 8 + j ) );
            }
            Charset.Characters[i].Tile.CustomColor = 1;
          }
          Editor.characterEditor.CharsetUpdated( Charset );
          return true;
        case 5:
          for ( int i = 0; i < 256; ++i )
          {
            for ( int j = 0; j < 8; ++j )
            {
              Charset.Characters[i].Tile.Data.SetU8At( j, ConstantData.LowerCaseCharsetCommanderX16.ByteAt( i * 8 + j ) );
            }
            Charset.Characters[i].Tile.CustomColor = 1;
          }
          Editor.characterEditor.CharsetUpdated( Charset );
          return true;
      }
      return false;
    }



  }
}
