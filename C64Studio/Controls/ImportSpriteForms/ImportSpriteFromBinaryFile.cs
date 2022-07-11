using RetroDevStudio.Formats;
using RetroDevStudio.Documents;



namespace RetroDevStudio.Controls
{
  public partial class ImportSpriteFromBinaryFile : ImportSpriteFormBase
  {
    public ImportSpriteFromBinaryFile() :
      base( null )
    { 
    }



    public ImportSpriteFromBinaryFile( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleImport( SpriteProject Project, SpriteEditor Editor )
    {
      string filename;

      if ( !Editor.OpenFile( "Open sprite file", Types.Constants.FILEFILTER_SPRITE + Types.Constants.FILEFILTER_SPRITE_SPRITEPAD + Types.Constants.FILEFILTER_ALL, out filename ) )
      {
        return false;
      }
      return Editor.ImportSprites( filename, true, true );
    }



  }
}
