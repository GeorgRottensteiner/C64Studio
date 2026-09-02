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
      comboImportFromDefault.Items.Add( "C16/+4 Uppercase" );
      comboImportFromDefault.Items.Add( "C16/+4 Lowercase" );
      comboImportFromDefault.Items.Add( "PET Uppercase" );
      comboImportFromDefault.Items.Add( "PET Lowercase" );
      comboImportFromDefault.Items.Add( "Commander X16 Uppercase" );
      comboImportFromDefault.Items.Add( "Commander X16 Lowercase" );
      comboImportFromDefault.Items.Add( "Commander X16 ISO-8859-15" );
      comboImportFromDefault.Items.Add( "Mega65 Full Color Mode Uppercase" );
      comboImportFromDefault.Items.Add( "Mega65 Full Color Mode Lowercase" );

      comboImportFromDefault.SelectedIndex = 0;
    }



    public override bool HandleImport( ImportCharsetInfo importInfo, CharsetEditor Editor )
    {
      int sourceIndex = 0;
      int numBytesOfChar = Lookup.NumBytesOfSingleCharacterBitmap( importInfo.Charset.Mode );
      foreach ( int i in importInfo.ImportIndices )
      {
        if ( sourceIndex >= importInfo.ImportIndices.Count )
        {
          break;
        }
        switch ( comboImportFromDefault.SelectedIndex )
        {
          case 0:
            ConstantData.UpperCaseCharsetC64.CopyTo( importInfo.Charset.Characters[i].Tile.Data, sourceIndex * numBytesOfChar, numBytesOfChar );
            importInfo.Charset.Characters[i].Tile.CustomColor = 1;
            break;
          case 1:
            ConstantData.LowerCaseCharsetC64.CopyTo( importInfo.Charset.Characters[i].Tile.Data, sourceIndex * numBytesOfChar, numBytesOfChar );
            importInfo.Charset.Characters[i].Tile.CustomColor = 1;
            break;
          case 2:
            ConstantData.UpperCaseCharsetViC20.CopyTo( importInfo.Charset.Characters[i].Tile.Data, sourceIndex * numBytesOfChar, numBytesOfChar );
            importInfo.Charset.Characters[i].Tile.CustomColor = 1;
            break;
          case 3:
            ConstantData.LowerCaseCharsetViC20.CopyTo( importInfo.Charset.Characters[i].Tile.Data, sourceIndex * numBytesOfChar, numBytesOfChar );
            importInfo.Charset.Characters[i].Tile.CustomColor = 1;
            break;
          case 4:
            ConstantData.UpperCaseCharsetPlus4.CopyTo( importInfo.Charset.Characters[i].Tile.Data, sourceIndex * numBytesOfChar, numBytesOfChar );
            importInfo.Charset.Characters[i].Tile.CustomColor = 1;
            break;
          case 5:
            ConstantData.LowerCaseCharsetPlus4.CopyTo( importInfo.Charset.Characters[i].Tile.Data, sourceIndex * numBytesOfChar, numBytesOfChar );
            importInfo.Charset.Characters[i].Tile.CustomColor = 1;
            break;
          case 6:
            ConstantData.UpperCaseCharsetPET.CopyTo( importInfo.Charset.Characters[i].Tile.Data, sourceIndex * numBytesOfChar, numBytesOfChar );
            importInfo.Charset.Characters[i].Tile.CustomColor = 1;
            break;
          case 7:
            ConstantData.LowerCaseCharsetPET.CopyTo( importInfo.Charset.Characters[i].Tile.Data, sourceIndex * numBytesOfChar, numBytesOfChar );
            importInfo.Charset.Characters[i].Tile.CustomColor = 1;
            break;
          case 8:
            ConstantData.UpperCaseCharsetCommanderX16.CopyTo( importInfo.Charset.Characters[i].Tile.Data, sourceIndex * numBytesOfChar, numBytesOfChar );
            importInfo.Charset.Characters[i].Tile.CustomColor = 1;
            break;
          case 9:
            ConstantData.UpperCaseCharsetCommanderX16.CopyTo( importInfo.Charset.Characters[i].Tile.Data, sourceIndex * numBytesOfChar, numBytesOfChar );
            importInfo.Charset.Characters[i].Tile.CustomColor = 1;
            break;
          case 10:
            ConstantData.ISO8859CommanderX16.CopyTo( importInfo.Charset.Characters[i].Tile.Data, sourceIndex * numBytesOfChar, numBytesOfChar );
            importInfo.Charset.Characters[i].Tile.CustomColor = 1;
            break;
          case 11:
            ConstantData.UpperCaseCharsetMega65FCM.CopyTo( importInfo.Charset.Characters[i].Tile.Data, sourceIndex * numBytesOfChar, numBytesOfChar );
            importInfo.Charset.Characters[i].Tile.CustomColor = 1;
            break;
          case 12:
            ConstantData.LowerCaseCharsetMega65FCM.CopyTo( importInfo.Charset.Characters[i].Tile.Data, sourceIndex * numBytesOfChar, numBytesOfChar );
            importInfo.Charset.Characters[i].Tile.CustomColor = 1;
            break;
        }
        ++sourceIndex;
      }
      Editor.characterEditor.CharsetUpdated( importInfo.Charset );
      return true;
    }



  }
}
