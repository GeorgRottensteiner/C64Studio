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
using RetroDevStudio.Documents;



namespace RetroDevStudio.Controls
{
  public partial class ExportGraphicScreenAsImage : ExportGraphicScreenFormBase
  {
    public ExportGraphicScreenAsImage() :
      base( null )
    { 
    }



    public ExportGraphicScreenAsImage( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();

      comboExportImageNumberOfColors.Items.Add( "Optimize" );
      comboExportImageNumberOfColors.Items.Add( "2" );
      comboExportImageNumberOfColors.Items.Add( "4" );
      comboExportImageNumberOfColors.Items.Add( "8" );
      comboExportImageNumberOfColors.Items.Add( "16" );
      comboExportImageNumberOfColors.Items.Add( "32" );
      comboExportImageNumberOfColors.Items.Add( "64" );
      comboExportImageNumberOfColors.Items.Add( "128" );
      comboExportImageNumberOfColors.Items.Add( "256" );

      comboExportImageNumberOfColors.SelectedIndex = 0;
    }



    public override bool HandleExport( GraphicScreenEditor editor, ExportGraphicScreenInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
    {
      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Export Characters to Image";
      saveDlg.Filter = Core.MainForm.FilterString( RetroDevStudio.Types.Constants.FILEFILTER_IMAGE_FILES );
      if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return false;
      }

      bool      optimizePalette = ( comboExportImageNumberOfColors.SelectedIndex == 0 );
      int       numberOfColorsToExport = 0;
      if ( !optimizePalette )
      {
        int numberOfColors = 1 << comboExportImageNumberOfColors.SelectedIndex;
        if ( numberOfColors > numberOfColorsToExport )
        {
          if ( Core.Notification.UserDecision( Dialogs.DlgDeactivatableMessage.MessageButtons.YES_NO, "Truncate palette?", "The image has more colors than you chose to export.\r\nThe palette can be truncated but that might end with missing pixels.\r\nTruncate the palette?" ) == Dialogs.DlgDeactivatableMessage.UserChoice.NO )
          {
            return false;
          }
        }
        numberOfColorsToExport = numberOfColors;
      }
      bool      useCompression = checkUseCompression.Checked; 
      string    extension = GR.Path.GetExtension( saveDlg.FileName ).ToUpper();

      System.Drawing.Imaging.ImageFormat formatToSave = System.Drawing.Imaging.ImageFormat.Png;

      if ( ( extension == ".KLA" )
      ||   ( extension == ".KOA" ) )
      {
        if ( ( Info.Project.ScreenWidth != 320 )
        ||   ( Info.Project.ScreenHeight != 200 ) )
        {
          Core.Notification.MessageBox( "Can't export to Koala", "A graphic can only be exported to Koala format if the size is 320x200!" );
          return false;
        }

        GR.Memory.ByteBuffer    screenChar;
        GR.Memory.ByteBuffer    screenColor;
        GR.Memory.ByteBuffer    bitmapData;
        bool[,]                 errorBlocks = new bool[40,25];

        var chars = new List<CharData>( 40 * 25 );
        for ( int i = 0; i < 40 * 25; ++i )
        {
          chars.Add( new CharData() );
        }

        Info.Project.ImageToMCBitmapData( Info.Project.ColorMapping, chars, errorBlocks, 0, 0, Info.BlockWidth, Info.BlockHeight, out bitmapData, out screenChar, out screenColor );

        var koalaData = RetroDevStudio.Converter.KoalaToBitmap.KoalaFromBitmap( bitmapData, screenChar, screenColor, (byte)Info.Project.Colors.BackgroundColor );

        if ( !GR.IO.File.WriteAllBytes( saveDlg.FileName, koalaData ) )
        {
          Core.Notification.MessageBox( "Error writing to file", "Could not export to file " + saveDlg.FileName );
        }
        return true;
      }
      else if ( extension == ".IFF" )
      {
        var imageData = RetroDevStudio.Converter.IFFToBitmap.IFFFromBitmap( Info.Project.Image, numberOfColorsToExport, useCompression );
        if ( !GR.IO.File.WriteAllBytes( saveDlg.FileName, imageData ) )
        {
          Core.Notification.MessageBox( "Error writing to file", "Could not export to file " + saveDlg.FileName );
        }
        return true;
      }
      if ( extension == ".BMP" )
      {
        formatToSave = System.Drawing.Imaging.ImageFormat.Bmp;
      }
      else if ( extension == ".PNG" )
      {
        formatToSave = System.Drawing.Imaging.ImageFormat.Png;
      }
      else if ( extension == ".GIF" )
      {
        formatToSave = System.Drawing.Imaging.ImageFormat.Gif;
      }
      else
      {
        Core.Notification.MessageBox( "Unsupported format", "Unsupported image file format " + extension + ". Please make sure to use the proper extension." );
        return false;
      }

      var bitmap = Info.Project.Image.GetAsBitmap();
      try
      {
        bitmap.Save( saveDlg.FileName, formatToSave );
      }
      catch ( Exception ex )
      {
        Core.Notification.MessageBox( "An Error occurred", "An error occurred while writing to file: " + ex.Message );
      }

      return true;
    }



  }
}
