using RetroDevStudio.Formats;
using RetroDevStudio.Types;
using RetroDevStudio.Documents;



namespace RetroDevStudio.Controls
{
  public partial class ImportSpriteFromImageFile : ImportSpriteFormBase
  {
    public ImportSpriteFromImageFile() :
      base( null )
    { 
    }



    public ImportSpriteFromImageFile( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleImport( SpriteProject Project, SpriteEditor Editor )
    {
      string filename;

      if ( !Editor.OpenFile( "Import Sprites from Image", RetroDevStudio.Types.Constants.FILEFILTER_IMAGE_FILES, out filename ) )
      {
        return false;
      }

      GR.Image.FastImage spriteImage;

      var mcSettings = new ColorSettings( Project.Colors );

      var importType = Types.GraphicType.SPRITES;
      if ( Project.Mode == SpriteProject.SpriteProjectMode.MEGA65_16_X_21_16_COLORS )
      {
        importType = GraphicType.SPRITES_16_COLORS;
      }

      bool pasteAsBlock = false;
      if ( !Core.MainForm.ImportImage( filename, null, importType, mcSettings, out spriteImage, out mcSettings, out pasteAsBlock ) )
      {
        return false;
      }

      Project.Colors.BackgroundColor = mcSettings.BackgroundColor;
      Project.Colors.MultiColor1 = mcSettings.MultiColor1;
      Project.Colors.MultiColor2 = mcSettings.MultiColor2;
      if ( mcSettings.Palettes.Count > Project.Colors.Palettes.Count )
      {
        // a palette was imported!
        Project.Colors.Palettes.Add( mcSettings.Palettes[mcSettings.Palettes.Count - 1] );
      }

      Editor.ChangeColorSettingsDialog();

      int   currentSpriteIndex = 0;
      int   curX = 0;
      int   curY = 0;
      while ( curY + Editor.m_SpriteHeight <= spriteImage.Height )
      {
        //DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoSpritesetSpriteChange( this, Project, currentSpriteIndex ), firstUndoStep );

        Editor.ImportSprite( spriteImage.GetImage( curX, curY, Editor.m_SpriteWidth, Editor.m_SpriteHeight ) as GR.Image.FastImage, currentSpriteIndex );
        Editor.SpriteChanged( currentSpriteIndex );

        ++currentSpriteIndex;
        curX += Editor.m_SpriteWidth;
        if ( curX >= spriteImage.Width )
        {
          curX = 0;
          curY += Editor.m_SpriteHeight;
        }
      }
      return true;
    }



  }
}
