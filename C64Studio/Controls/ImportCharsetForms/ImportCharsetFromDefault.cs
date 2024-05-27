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
      comboImportFromDefault.Items.Add( "Commander X16 ISO-8859-15" );
      comboImportFromDefault.Items.Add( "Mega65 Full Color Mode Uppercase" );
      comboImportFromDefault.Items.Add( "Mega65 Full Color Mode Lowercase" );

      comboImportFromDefault.SelectedIndex = 0;
    }



    public override bool HandleImport( CharsetProject Charset, CharsetEditor Editor )
    {
      switch ( comboImportFromDefault.SelectedIndex )
      {
        case 0:
          for ( int i = 0; i < 256; ++i )
          {
            ConstantData.UpperCaseCharsetC64.CopyTo( Charset.Characters[i].Tile.Data, i * 8, 8 );
            Charset.Characters[i].Tile.CustomColor = 1;
          }
          Editor.characterEditor.CharsetUpdated( Charset );
          return true;
        case 1:
          for ( int i = 0; i < 256; ++i )
          {
            ConstantData.LowerCaseCharsetC64.CopyTo( Charset.Characters[i].Tile.Data, i * 8, 8 );
            Charset.Characters[i].Tile.CustomColor = 1;
          }
          Editor.characterEditor.CharsetUpdated( Charset );
          return true;
        case 2:
          for ( int i = 0; i < 256; ++i )
          {
            ConstantData.UpperCaseCharsetViC20.CopyTo( Charset.Characters[i].Tile.Data, i * 8, 8 );
            Charset.Characters[i].Tile.CustomColor = 1;
          }
          Editor.characterEditor.CharsetUpdated( Charset );
          return true;
        case 3:
          for ( int i = 0; i < 256; ++i )
          {
            ConstantData.LowerCaseCharsetViC20.CopyTo( Charset.Characters[i].Tile.Data, i * 8, 8 );
            Charset.Characters[i].Tile.CustomColor = 1;
          }
          Editor.characterEditor.CharsetUpdated( Charset );
          return true;
        case 4:
          for ( int i = 0; i < 256; ++i )
          {
            ConstantData.UpperCaseCharsetCommanderX16.CopyTo( Charset.Characters[i].Tile.Data, i * 8, 8 );
            Charset.Characters[i].Tile.CustomColor = 1;
          }
          Editor.characterEditor.CharsetUpdated( Charset );
          return true;
        case 5:
          for ( int i = 0; i < 256; ++i )
          {
            ConstantData.UpperCaseCharsetCommanderX16.CopyTo( Charset.Characters[i].Tile.Data, i * 8, 8 );
            Charset.Characters[i].Tile.CustomColor = 1;
          }
          Editor.characterEditor.CharsetUpdated( Charset );
          return true;
        case 6:
          for ( int i = 0; i < 256; ++i )
          {
            ConstantData.ISO8859CommanderX16.CopyTo( Charset.Characters[i].Tile.Data, i * 8, 8 );
            Charset.Characters[i].Tile.CustomColor = 1;
          }
          Editor.characterEditor.CharsetUpdated( Charset );
          return true;
        case 7:
          for ( int i = 0; i < 256; ++i )
          {
            ConstantData.UpperCaseCharsetMega65FCM.CopyTo( Charset.Characters[i].Tile.Data, i * 64, 64 );
            Charset.Characters[i].Tile.CustomColor = 1;
          }
          Editor.characterEditor.CharsetUpdated( Charset );
          return true;
        case 8:
          for ( int i = 0; i < 256; ++i )
          {
            ConstantData.LowerCaseCharsetMega65FCM.CopyTo( Charset.Characters[i].Tile.Data, i * 64, 64 );
            Charset.Characters[i].Tile.CustomColor = 1;
          }
          Editor.characterEditor.CharsetUpdated( Charset );
          return true;
      }
      return false;
    }



  }
}
