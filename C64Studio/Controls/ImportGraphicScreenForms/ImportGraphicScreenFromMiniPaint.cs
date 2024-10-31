using RetroDevStudio.Formats;
using RetroDevStudio.Types;
using GR.Memory;
using System;
using RetroDevStudio.Documents;



namespace RetroDevStudio.Controls
{
  public partial class ImportGraphicScreenFromMiniPaint : ImportCharscreenFormBase
  {
    public ImportGraphicScreenFromMiniPaint() :
      base( null )
    { 
    }



    public ImportGraphicScreenFromMiniPaint( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public virtual bool HandleImport( GraphicScreenProject Project, GraphicScreenEditor Editor )
    {
      if ( !Editor.OpenFile( "Open MiniPaint .prg file", Constants.FILEFILTER_PRG + Constants.FILEFILTER_ALL, out string filename ) )
      {
        return false;
      }
      GR.Memory.ByteBuffer data = GR.IO.File.ReadAllBytes( filename );

      if ( ( data.Length != 4099 )
      || ( data.UInt16NetworkOrderAt( 0 ) != 0x10f1 ) )
      {
        // not a valid MiniPaint file
        return false;
      }

      //Editor.ImportFromData( data );
      return false;
    }



  }
}
