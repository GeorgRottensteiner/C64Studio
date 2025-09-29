using System.Collections.Generic;



namespace DecentForms
{
  public class ListControlItem
  {
    internal ListControl  _Owner = null;
    private bool          _Checked = false;
    internal bool         _Selected = false;
    internal int          _Index = -1;

    internal ListControlSubItemCollection SubItems = new ListControlSubItemCollection( null );



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