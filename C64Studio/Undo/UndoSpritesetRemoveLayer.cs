using System;
using System.Collections.Generic;
using System.Text;
using C64Studio.Formats;



namespace C64Studio.Undo
{
  public class UndoSpritesetRemoveLayer : UndoTask
  {
    private SpriteEditor          Editor = null;
    private SpriteProject         Project = null;
    private SpriteProject.Layer   RemovedLayer = null;
    private int                   LayerIndex = -1;



    public UndoSpritesetRemoveLayer( SpriteEditor Editor, SpriteProject Project, int LayerIndex )
    {
      this.Editor = Editor;
      this.Project = Project;
      RemovedLayer = Project.SpriteLayers[LayerIndex];
      this.LayerIndex = LayerIndex;
    }




    public override string Description
    {
      get
      {
        return "Spriteset Layer Remove";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoSpritesetAddLayer( Editor, Project, LayerIndex );
    }



    public override void Apply()
    {
      Project.SpriteLayers.Insert( LayerIndex, RemovedLayer );

      Editor.LayersChanged();
    }
  }
}
