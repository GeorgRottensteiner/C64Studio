namespace DecentForms
{
  public class ListControlSubItem
  {
    internal ListControl      _Owner = null;
    private string            _Text = "";
    internal bool             _Selected = false;
    internal int              _Index = -1;



    public ListControlSubItem()
    {
    }



    public ListControlSubItem( string Text )
    {
      _Text = Text;
    }



    public string   Text 
    {
      get
      {
        return _Text;
      }
      set
      {
        _Text = value;
        _Owner?.ItemsModified();
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
          //_Owner?.UnselectItem( this );
        }
        else
        {
          _Selected = true;
          //_Owner?.SelectItem( this );
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