using RetroDevStudio.Formats;
using RetroDevStudio.Types;
using GR.Memory;
using System;
using RetroDevStudio.Documents;



namespace RetroDevStudio.Controls
{
  public partial class ImportGraphicScreenFromImage : ImportCharscreenFormBase
  {
    public ImportGraphicScreenFromImage() :
      base( null )
    { 
    }



    public ImportGraphicScreenFromImage( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public virtual bool HandleImport( GraphicScreenProject Project, GraphicScreenEditor Editor )
    {
      string filename;

      if ( !Editor.OpenFile( "Import from Image", Types.Constants.FILEFILTER_IMAGE_FILES + Types.Constants.FILEFILTER_ALL, out filename ) )
      {
        return false;
      }
      return Editor.ImportImage( filename, null, GraphicScreenEditor.ImageInsertionMode.AS_FULL_SCREEN );
    }



  }
}
