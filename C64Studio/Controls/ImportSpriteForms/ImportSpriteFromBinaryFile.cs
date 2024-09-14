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
      int bytesToSkip = GR.Convert.ToI32( editImportSkipBytes.Text );
      if ( bytesToSkip < 0 )
      {
        bytesToSkip = 0;
      }

      if ( !Editor.OpenFile( "Open sprite file", Types.Constants.FILEFILTER_SPRITE + Types.Constants.FILEFILTER_SPRITE_SPRITEPAD + Types.Constants.FILEFILTER_ALL, out filename ) )
      {
        return false;
      }
      if ( GR.Path.GetExtension( filename ).ToUpper() == ".PRG" )
      {
        bytesToSkip += 2;
      }
      return Editor.ImportSprites( filename, true, true, bytesToSkip, checkImportExpectPadding.Checked );
    }



  }
}
