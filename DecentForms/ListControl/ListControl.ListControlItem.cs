using System.Collections.Generic;
using System.Diagnostics;



namespace DecentForms
{
  [DebuggerDisplay( "Text = {Text}" )]
  public class ListControlItem
  {
    internal ListControl  _Owner = null;
    private bool          _Checked = false;
    internal bool         _Selected = false;
    internal int          _Index = -1;
    internal int          _ImageIndex = -1;

    public ListControlSubItemCollection SubItems = new ListControlSubItemCollection( null );



    public ListControlItem()
    {
      SubItems.Add( new ListControlSubItem() );
    }



    public ListControlItem( string Text )
    {
      SubItems.Add( new ListControlSubItem( Text ) );
    }



    public string Text 
    {
      get
      {
        return SubItems[0].Text;
      }
      set
      {
        SubItems[0].Text = value;
        _Owner?.ItemModified( Index );
      }
    }



    public override string ToString()
    {
      return Text;
    }



    public bool Checked
    {
      get
      {
        return _Checked;
      }
      set
      {
        _Checked = value;
        _Owner?.ItemModified( Index );
      }
    }



    public int ImageIndex
    {
      get
      {
        return _ImageIndex;
      }
      set
      {
        _ImageIndex = value;
        _Owner?.ItemModified( Index );
      }
    }



    public bool Selected
    {
      get
      {
        return _Selected;
      }
      set
      {
        if ( _Selected == value )
        {
          return;
        }
        if ( _Selected )
        {
          _Selected = false;
          _Owner?.UnselectItem( this );
        }
        else
        {
          _Selected = true;
          _Owner?.SelectItem( this );
        }
      }
    }



    public int Index
    {
      get
      {
        return _Index;
      }
    }



    public object Tag { get; set; } = null;



  }


}