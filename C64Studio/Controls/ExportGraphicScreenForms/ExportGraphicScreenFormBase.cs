using GR.Memory;
using RetroDevStudio;
using RetroDevStudio.Formats;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RetroDevStudio.Controls
{
  public partial class ExportGraphicScreenFormBase : UserControl
  {
    public StudioCore                   Core = null;



    public ExportGraphicScreenFormBase()
    {
      InitializeComponent();
    }



    public ExportGraphicScreenFormBase( StudioCore Core )
    {
      this.Core         = Core;

      InitializeComponent();
    }



    public virtual bool HandleExport( ExportGraphicScreenInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
    {
      return false;
    }



    protected bool ApplyCharsetChecks( ExportGraphicScreenInfo Info, bool charsetIsMultiColor, bool stripUnusedChars,
                                       out List<uint> screenData,
                                       out ByteBuffer charsetData )
    {
      screenData = new List<uint>();
      charsetData = new ByteBuffer();

      int   x = 0;
      int   y = 0;
      foreach ( var c in Info.Chars )
      {
        Info.ErrornousChars[x / Info.CheckBlockWidth, y / Info.CheckBlockHeight] = !Info.Project.CheckCharBox( c, x, y, Info.CheckBlockWidth, Info.CheckBlockHeight, charsetIsMultiColor );

        x += Info.CheckBlockWidth;
        if ( x >= Info.Image.Width )
        {
          x = 0;
          y += Info.CheckBlockHeight;
        }
      }
      // automatic check
      if ( Info.Chars.Any( c => !string.IsNullOrEmpty( c.Error ) ) )
      {
        Core.Notification.MessageBox( "Cannot export to charset", "Cannot export to charset, conversion had errors!\r\nCheck the chosen colors for possible combinations!" );
        return false;
      }

      // check for duplicates
      int items = Info.Chars.Count;
      int foldedItems = 0;
      int curIndex = 0;

      for ( int index1 = 0; index1 < Info.Chars.Count; ++index1 )
      {
        bool wasFolded = false;
        for ( int index2 = 0; index2 < index1; ++index2 )
        {
          if ( Info.Chars[index1].Tile.Data.Compare( Info.Chars[index2].Tile.Data ) == 0 )
          {
            // same data
            if ( Info.Chars[index2].Replacement != null )
            {
              Info.Chars[index1].Replacement = Info.Chars[index2].Replacement;
            }
            else
            {
              Info.Chars[index1].Replacement = Info.Chars[index2];
            }
            ++foldedItems;
            wasFolded = true;
            break;
          }
        }
        if ( !wasFolded )
        {
          // item was not folded
          Info.Chars[index1].Index = curIndex;
          ++curIndex;
        }
      }
      if ( items - foldedItems > 256 )
      {
        Core.Notification.MessageBox( "Cannot export to charset", "Cannot export to charset, more than 256 unique characters found!\r\nCheck the chosen colors for possible combinations!" );
        return false;
      }

      // BOO TODO!
      if ( stripUnusedChars )
      {
        charsetData.Resize( (uint)( 8 * ( items - foldedItems ) ) );
      }
      else
      {
        charsetData.Resize( 256 * 8 );
      }
      screenData = new List<uint>( Info.Chars.Count );

      for ( int i = 0; i < Info.Chars.Count; ++i )
      {
        screenData.Add( (uint)( ( Info.Chars[i].Index & 0xffff ) | ( Info.Chars[i].Tile.CustomColor << 16 ) ) );
        if ( Info.Chars[i].Replacement == null )
        {
          Info.Chars[i].Tile.Data.CopyTo( charsetData, 0, 8, Info.Chars[i].Index * 8 );
        }
        else
        {
          screenData[i] = (uint)( ( Info.Chars[i].Replacement.Index & 0xffff ) | ( Info.Chars[i].Tile.CustomColor << 16 ) );
        }
      }
      return true;
    }



  }
}
