using RetroDevStudio;
using RetroDevStudio.Documents;
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
  public partial class ExportGraphicScreenAsBASICData : ExportGraphicScreenFormBase
  {
    private enum ExportType
    {
      [Description( "Hires Bitmap" )]
      HIRES_BITMAP,
      [Description( "Multicolor Bitmap" )]
      MULTICOLOR_BITMAP,
      [Description( "HiRes Charset" )]
      HIRES_CHARSET,
      [Description( "HiRes Charset and screen data assembly" )]
      HIRES_CHARSET_SCREEN_ASSEMBLY,
      [Description( "Multicolor Charset" )]
      MULTICOLOR_CHARSET,
      [Description( "Multicolor Charset and screen data assembly" )]
      MULTICOLOR_CHARSET_SCREEN_ASSEMBLY
    };



    private enum ExportContent
    {
      [Description( "Bitmap, Screen, Color" )]
      BITMAP_SCREEN_COLOR = 0,
      [Description( "Bitmap, Color, Screen" )]
      BITMAP_COLOR_SCREEN,
      [Description( "Bitmap, Screen" )]
      BITMAP_SCREEN,
      [Description( "Bitmap, Color" )]
      BITMAP_COLOR,
      [Description( "Bitmap" )]
      BITMAP,
      [Description( "Screen" )]
      SCREEN,
      [Description( "Color" )]
      COLOR
    };



    public ExportGraphicScreenAsBASICData() :
      base( null )
    { 
    }



    public ExportGraphicScreenAsBASICData( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
      UtilForms.FillComboWithEnumDescription( comboExportType, typeof( ExportType ) );
      UtilForms.FillComboWithEnumDescription( comboExportContent, typeof( ExportContent ) );

      comboExportType.SelectedIndex = 0;
      comboExportContent.SelectedIndex = 0;
    }



    private void checkExportToDataWrap_CheckedChanged( object sender, EventArgs e )
    {
      editWrapByteCount.Enabled = checkExportToDataWrap.Checked;
    }



    private int GetExportWrapCount()
    {
      if ( checkExportToDataWrap.Checked )
      {
        return GR.Convert.ToI32( editWrapByteCount.Text );
      }
      return 0;
    }



    private int GetExportCharCount()
    {
      if ( checkWrapAtMaxChars.Checked )
      {
        return GR.Convert.ToI32( editWrapCharCount.Text );
      }
      return 80;
    }



    public override bool HandleExport( ExportGraphicScreenInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
    {
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

      int wrapByteCount = GetExportWrapCount();
      bool asHex = checkExportHex.Checked;
      bool insertSpaces = checkInsertSpaces.Checked;
      int wrapCharCount = GetExportCharCount();

      var exportType = (ExportType)comboExportType.SelectedIndex;
      var exportContent = (ExportContent)comboExportContent.SelectedIndex;

      var             sb = new StringBuilder();

      GR.Memory.ByteBuffer screenChar   = new GR.Memory.ByteBuffer();
      GR.Memory.ByteBuffer screenColor  = new GR.Memory.ByteBuffer();
      GR.Memory.ByteBuffer bitmapData   = new GR.Memory.ByteBuffer();
      var exportedData = new GR.Memory.ByteBuffer();

      switch ( exportType )
      {
        case ExportType.HIRES_BITMAP:
          Info.Project.ImageToHiresBitmapData( Info.Project.ColorMapping, null, null,
                                               0, 0, Info.BlockWidth, Info.BlockHeight,
                                               out bitmapData, out screenChar, out screenColor );
          break;
        case ExportType.MULTICOLOR_BITMAP:
          Info.Project.ImageToMCBitmapData( Info.Project.ColorMapping, null, null,
                                            0, 0, Info.BlockWidth, Info.BlockHeight,
                                            out bitmapData, out screenChar, out screenColor );
          break;
        case ExportType.HIRES_CHARSET:
        case ExportType.MULTICOLOR_CHARSET:
          if ( Info.Chars.Count == 0 )
          {
            Core.Notification.MessageBox( "Cannot export to charset", "Cannot export to charset, conversion had errors!" );
            return false;
          }
          {
            var charsetProject = GraphicScreenEditor.ExportToCharset( Info.Project, Info.Chars );
            if ( charsetProject == null )
            {
              Core.Notification.MessageBox( "Cannot export to charset", "Cannot export to charset, conversion had errors!" );
              return false;
            }
            exportedData = charsetProject.CharacterData();
          }
          break;
        case ExportType.MULTICOLOR_CHARSET_SCREEN_ASSEMBLY:
        case ExportType.HIRES_CHARSET_SCREEN_ASSEMBLY:
          if ( Info.Chars.Count == 0 )
          {
            Core.Notification.MessageBox( "Cannot export to charset", "Cannot export to charset, conversion had errors!" );
            return false;
          }
          {
            var charsetProject = GraphicScreenEditor.ExportToCharset( Info.Project, Info.Chars );
            if ( charsetProject == null )
            {
              Core.Notification.MessageBox( "Cannot export to charset", "Cannot export to charset, conversion had errors!" );
              return false;
            }
            exportedData = charsetProject.CharacterData();
          }
          break;
      }

      switch ( exportContent )
      {
        case ExportContent.BITMAP:
          exportedData = bitmapData;
          break;
        case ExportContent.BITMAP_COLOR:
          exportedData = bitmapData + screenColor;
          break;
        case ExportContent.BITMAP_COLOR_SCREEN:
          exportedData = bitmapData + screenColor + screenChar;
          break;
        case ExportContent.BITMAP_SCREEN:
          exportedData = bitmapData + screenChar;
          break;
        case ExportContent.BITMAP_SCREEN_COLOR:
          exportedData = bitmapData + screenChar + screenColor;
          break;
        case ExportContent.COLOR:
          exportedData = screenColor;
          break;
        case ExportContent.SCREEN:
          exportedData = screenChar;
          break;
      }

      sb.AppendLine( Util.ToBASICData( exportedData, startLine, lineOffset, wrapByteCount, wrapCharCount, insertSpaces, asHex ) );

      EditOutput.Font = Core.Imaging.FontFromMachine( MachineType.C64 );
      EditOutput.Text = sb.ToString();
      return true;
    }



  }
}
