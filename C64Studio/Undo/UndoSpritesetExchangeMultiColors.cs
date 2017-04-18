using System;
using System.Collections.Generic;
using System.Text;
using C64Studio.Formats;



namespace C64Studio.Undo
{
  public class UndoSpritesetExchangeMultiColors : UndoTask
  {
    public enum ExchangeMode
    {
      MULTICOLOR_1_WITH_MULTICOLOR_2,
      MULTICOLOR_1_WITH_BACKGROUND,
      MULTICOLOR_2_WITH_BACKGROUND
    };

    private SpriteEditor            Editor = null;
    private ExchangeMode            Mode = ExchangeMode.MULTICOLOR_1_WITH_MULTICOLOR_2;





    public UndoSpritesetExchangeMultiColors( SpriteEditor Editor, ExchangeMode Mode )
    {
      this.Editor = Editor;
      this.Mode   = Mode;
    }




    public string Description
    {
      get
      {
        return "Charset Color Exchange";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoSpritesetExchangeMultiColors( Editor, Mode );
    }



    public override void Apply()
    {
      switch ( Mode )
      {
        case ExchangeMode.MULTICOLOR_1_WITH_MULTICOLOR_2:
          Editor.ExchangeMultiColors();
          break;
        case ExchangeMode.MULTICOLOR_1_WITH_BACKGROUND:
          Editor.ExchangeMultiColor1WithBackground();
          break;
        case ExchangeMode.MULTICOLOR_2_WITH_BACKGROUND:
          Editor.ExchangeMultiColor2WithBackground();
          break;
      }
    }
  }
}
