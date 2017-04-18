using System;
using System.Collections.Generic;
using System.Text;
using C64Studio.Formats;



namespace C64Studio.Undo
{
  public class UndoSpritesetExchangeLayer : UndoTask
  {
    private SpriteEditor          Editor = null;
    private SpriteProject         Project = null;
    private int                   LayerIndex1 = -1;
    private int                   LayerIndex2 = -1;



    public UndoSpritesetExchangeLayer( SpriteEditor Editor, SpriteProject Project, int LayerIndex1, int LayerIndex2 )
    {
      this.Editor = Editor;
      this.Project = Project;
      this.LayerIndex1 = LayerIndex1;
      this.LayerIndex2 = LayerIndex2;
    }




    public string Description
    {
      get
      {
        return "Spriteset Layer Exchange";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoSpritesetExchangeLayer( Editor, Project, LayerIndex1, LayerIndex2 );
    }



    public override void Apply()
    {
      var tmp = Project.SpriteLayers[LayerIndex1];
      Project.SpriteLayers[LayerIndex1] = Project.SpriteLayers[LayerIndex2];
      Project.SpriteLayers[LayerIndex2] = tmp;

      Editor.LayersChanged();
    }
  }
}
