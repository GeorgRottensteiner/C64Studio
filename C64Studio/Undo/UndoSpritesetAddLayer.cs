using System;
using System.Collections.Generic;
using System.Text;
using C64Studio.Formats;



namespace C64Studio.Undo
{
  public class UndoSpritesetAddLayer : UndoTask
  {
    private SpriteEditor          Editor = null;
    private SpriteProject         Project = null;
    private int                   LayerIndex = -1;



    public UndoSpritesetAddLayer( SpriteEditor Editor, SpriteProject Project, int LayerIndex )
    {
      this.Editor = Editor;
      this.Project = Project;
      this.LayerIndex = LayerIndex;
    }




    public string Description
    {
      get
      {
        return "Spriteset Layer Add";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoSpritesetRemoveLayer( Editor, Project, LayerIndex );
    }



    public override void Apply()
    {
      // TODO
      Project.SpriteLayers.RemoveAt( LayerIndex );

      Editor.LayersChanged();
    }
  }
  
}
