using RetroDevStudio.Types;
using RetroDevStudio;
using RetroDevStudio.Formats;
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
  public partial class ExportCharscreenAsMarqSPETSCII : ExportCharscreenFormBase
  {
    public ExportCharscreenAsMarqSPETSCII() :
      base( null )
    { 
    }



    public ExportCharscreenAsMarqSPETSCII( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleExport( ExportCharsetScreenInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
    {
      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Save data as";
      saveDlg.Filter = "Marq's PETSCII File|*.c|All Files|*.*";
      if ( DocInfo.Project != null )
      {
        saveDlg.InitialDirectory = DocInfo.Project.Settings.BasePath;
      }
      if ( saveDlg.ShowDialog() != DialogResult.OK )
      {
        return false;
      }

      StringBuilder   sb = new StringBuilder();

      sb.Append( "unsigned char frame0000[]={// border,bg,chars,colors" );
      sb.Append( (char)10 );
      sb.Append( "0," );
      sb.Append( Info.Charscreen.CharSet.Colors.BackgroundColor );
      sb.Append( ',' );
      sb.Append( (char)10 );

      int     bytePos = 0;
      for ( int j = 0; j < Info.Area.Height; ++j )
      {
        for ( int i = 0; i < Info.Area.Width; ++i )
        {
          sb.Append( Info.ScreenCharData.ByteAt( bytePos ) );
          sb.Append( ',' );
          ++bytePos;
        }
        sb.Append( (char)10 );
      }

      bytePos = 0;
      for ( int j = 0; j < Info.Area.Height; ++j )
      {
        for ( int i = 0; i < Info.Area.Width; ++i )
        {
          sb.Append( Info.ScreenColorData.ByteAt( bytePos ) );
          ++bytePos;
          if ( ( j < Info.Area.Height - 1 )
          ||     ( i + 1 < Info.Area.Width ) )
          {
            sb.Append( ',' );
          }
        }
        sb.Append( (char)10 );
      }
      sb.Append( "};" );
      sb.Append( (char)10 );

      sb.Append( "// META: " );
      sb.Append( Info.Area.Width );
      sb.Append( ' ' );
      sb.Append( Info.Area.Height );
      sb.Append( ' ' );
      switch ( Info.Charscreen.Mode )
      {
        case TextMode.COMMODORE_40_X_25_ECM:
        case TextMode.COMMODORE_40_X_25_HIRES:
        case TextMode.COMMODORE_40_X_25_MULTICOLOR:
        default:
          sb.Append( "C64" );
          break;
        case TextMode.COMMODORE_VIC20_22_X_23:
          sb.Append( "VIC20" );
          break;
      }
      sb.Append( " upper" );
      sb.Append( (char)10 );

      GR.IO.File.WriteAllText( saveDlg.FileName, sb.ToString() );

      return true;
    }



  }
}
