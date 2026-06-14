using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace DecentForms
{
  public interface IControlRenderer
  {
    void DrawDashedLine( int X1, int Y1, int X2, int Y2, uint Color );
    void DrawLine( int X1, int Y1, int X2, int Y2, uint Color );
    void DrawRaisedRectangle( int X, int Y, int Width, int Height, uint BaseColor );

    void RenderButton( bool MouseOver, bool Pushed, bool IsDefaultButton, Button.ButtonStyle Style );
    void RenderCheckBox( string Text, ContentAlignment Alignment, bool MouseOver, bool Pushed, bool Checked );
    void RenderRadioButton( string Text, ContentAlignment Alignment, bool MouseOver, bool Pushed, bool Checked );

    void RenderSlider( int X, int Y, int Width, int Height, bool MouseOver, bool Pushed );

    /*
    void RenderGroupBox( string Text );
    void RenderListBox();

    void RenderTabControlTab( System.Drawing.Rectangle Rect, string Text, bool MouseOver, bool Selected );
    void RenderTabControl();

    void RenderTreeView();*/
  }
}
