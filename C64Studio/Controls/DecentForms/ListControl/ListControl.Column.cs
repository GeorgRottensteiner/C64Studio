using RetroDevStudio.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DecentForms
{
  public class Column
  {
    private string                      _name = "";
    private int                         _width = 20;

    public DecentForms.TextAlignment    Alignment = TextAlignment.LEFT | TextAlignment.CENTERED_V;
    public bool                         Sizable = true;
    public int                          Index = -1;
    internal ListControl                _Owner = null;


    public Column( string name, int width = 20 )
    {
      _name  = name;
      _width = width;
    }



    public string Name
    {
      get
      {
        return _name;
      }
      set
      {
        if ( _name != value )
        {
          _name = value;
          _Owner?.ColumnsModified();
        }
      }
    }



    public int Width
    {
      get
      {
        return _width;
      }
      set
      {
        if ( value < 0 )
        {
          value = 0;
        }
        if ( _width != value )
        {
          _width = value;
          _Owner?.ColumnsModified();
        }
      }
    }

  }
}
