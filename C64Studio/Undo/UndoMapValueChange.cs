using System;
using System.Collections.Generic;
using System.Text;
using C64Studio.Formats;
using RetroDevStudioModels;

namespace C64Studio.Undo
{
  public class UndoMapValueChange : UndoTask
  {
    public MapProject.Map         AffectedMap = null;
    public MapEditor              MapEditor = null;

    public int                    AlternativeColor1 = -1;
    public int                    AlternativeColor2 = -1;
    public int                    AlternativeBGColor = -1;
    public int                    AlternativeBGColor4 = -1;
    public bool                   UseAlternativeMode = false;
    public TextCharMode           AlternativeMode = TextCharMode.COMMODORE_HIRES;
    public int                    TileSpacingX = 0;
    public int                    TileSpacingY = 0;
    public string                 Name = "";
    public string                 ExtraData = "";



    public UndoMapValueChange( MapEditor Editor, MapProject.Map Map )
    {
      MapEditor = Editor;
      AffectedMap = Map;


      AlternativeColor1   = Map.AlternativeMultiColor1;
      AlternativeColor2   = Map.AlternativeMultiColor2;
      AlternativeBGColor  = Map.AlternativeBackgroundColor;
      AlternativeBGColor4 = Map.AlternativeBGColor4;
      ExtraData           = Map.ExtraDataText;
      Name                = Map.Name;
      TileSpacingX        = Map.TileSpacingX;
      TileSpacingY        = Map.TileSpacingY;
      AlternativeMode     = Map.AlternativeMode;
    }




    public override string Description
    {
      get
      {
        return "Map Value Change";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoMapValueChange( MapEditor, AffectedMap );
    }



    public override void Apply()
    {
      AffectedMap.AlternativeMultiColor1 = AlternativeColor1;
      AffectedMap.AlternativeMultiColor2 = AlternativeColor2;
      AffectedMap.AlternativeBackgroundColor = AlternativeBGColor;
      AffectedMap.AlternativeBGColor4 = AlternativeBGColor4;
      AffectedMap.ExtraDataText       = ExtraData;
      AffectedMap.Name                = Name;
      AffectedMap.TileSpacingX        = TileSpacingX;
      AffectedMap.TileSpacingY        = TileSpacingY;
      AffectedMap.AlternativeMode     = AlternativeMode;

      MapEditor.InvalidateCurrentMap();
    }
  }
}
