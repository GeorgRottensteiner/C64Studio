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



namespace RetroDevStudio.Controls
{
  public partial class ExportSpriteAsSBASICFCSpritedef : ExportSpriteFormBase
  {
    public ExportSpriteAsSBASICFCSpritedef() :
      base( null )
    { 
    }



    public ExportSpriteAsSBASICFCSpritedef( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();

      comboExportOrder.SelectedIndex = 0;
    }



    public override bool HandleExport( ExportSpriteInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
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
      bool          exportByFrame = ( comboExportOrder.SelectedIndex == 1 );

      int lineNo = startLine;
      int spriteNo = 0;
      int frameNo = 0;

      foreach ( var index in exportIndices )
      {
        for ( int y = 0; y < Lookup.SpriteHeight( Info.Project.Mode ); ++y )
        {
          sb.Append( lineNo );
          sb.Append( "FCSPRDEF " );
          sb.Append( spriteNo );
          sb.Append( "," );
          sb.Append( frameNo + 1 );
          sb.Append( "," );
          sb.Append( y + 1 );
          sb.Append( ",\"" );

          for ( int x = 0; x < Lookup.SpriteWidth( Info.Project.Mode ); ++x )
          {
            var  pixel = Info.Project.Sprites[index].Tile.GetPixel( x, y );
            if ( pixel.second == 0 )
            {
              sb.Append( '.' );
            }
            else
            {
              sb.Append( pixel.second.ToString( "X" ) );
            }
          }
          sb.AppendLine( "\"" );
          lineNo += lineOffset;
        }
        if ( exportByFrame )
        {
          frameNo = ( frameNo + 1 ) % 16;
          if ( frameNo == 0 )
          {
            spriteNo = ( spriteNo + 1 ) % 8;
          }
        }
        else
        {
          spriteNo = ( spriteNo + 1 ) % 8;
          if ( spriteNo == 0 )
          {
            frameNo = ( frameNo + 1 ) % 16;
          }
        }
      }

      EditOutput.Font = Core.Imaging.FontFromMachine( MachineType.C64 );
      EditOutput.Text = sb.ToString();
      return true;
    }



  }
}
  