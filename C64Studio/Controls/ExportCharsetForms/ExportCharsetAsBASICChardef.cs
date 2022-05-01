using RetroDevStudio;
using RetroDevStudio.Formats;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace C64Studio.Controls
{
  public partial class ExportCharsetAsBASICChardef : ExportCharsetFormBase
  {
    public ExportCharsetAsBASICChardef() :
      base( null )
    { 
    }



    public ExportCharsetAsBASICChardef( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleExport( ExportCharsetInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
    {
      var   sb = new StringBuilder();

      int startLine = GR.Convert.ToI32( editExportBASICLineNo.Text );
      if ( ( startLine < 0 )
      ||   ( startLine > 63999 ) )
      {
        startLine = 10;
      }
      int lineOffset = GR.Convert.ToI32( editExportBASICLineOffset.Text );
      if ( ( lineOffset < 0 )
      ||   ( lineOffset > 63999 ) )
      {
        lineOffset = 10;
      }

      List<int>     exportIndices = Info.ExportIndices;

      foreach ( int index in exportIndices )
      {
        sb.Append( startLine );
        sb.Append( "CHARDEF" );
        sb.Append( index );

        for ( int i = 0; i < 8; ++i )
        {
          if ( checkExportHex.Checked )
          {
            sb.Append( ",$" );
            sb.Append( Info.Charset.Characters[index].Tile.Data.ByteAt( i ).ToString( "X2" ) );
          }
          else
          {
            sb.Append( ',' );
            sb.Append( Info.Charset.Characters[index].Tile.Data.ByteAt( i ) );
          }
        }
        sb.AppendLine();

        startLine += lineOffset;
      }

      EditOutput.Font = new System.Drawing.Font( Core.MainForm.m_FontC64.Families[0], 16, System.Drawing.GraphicsUnit.Pixel );
      EditOutput.Text = sb.ToString();
      return true;
    }



  }
}
