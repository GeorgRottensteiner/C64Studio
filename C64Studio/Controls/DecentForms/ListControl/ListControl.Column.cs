using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DecentForms
{
  public class Column
  {
    public string                       Name = "";
    public DecentForms.TextAlignment    Alignment = TextAlignment.LEFT | TextAlignment.CENTERED_V;
    public int                          Width = 20;
    public bool                         Sizable = true;
    public int                          Index = -1;
    internal ListControl                _Owner = null;


    public Column( string name, int width = 20 )
    {
      Name  = name;
      Width = width;
    }
  };
}
