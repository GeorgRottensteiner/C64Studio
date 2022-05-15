using RetroDevStudio.Formats;
using RetroDevStudio.Types;
using GR.Memory;
using RetroDevStudio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static RetroDevStudio.BaseDocument;

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

      comboImportFromDefault.Items.Add( "C64 Uppercase" );
      comboImportFromDefault.Items.Add( "C64 Lowercase" );
      comboImportFromDefault.Items.Add( "VIC20 Uppercase" );
      comboImportFromDefault.Items.Add( "VIC20 Lowercase" );

      comboImportFromDefault.SelectedIndex = 0;
    }



    public override bool HandleImport( CharsetScreenProject CharScreen, CharsetScreenEditor Editor )
    {
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
      }
      return false;
    }



  }
}
