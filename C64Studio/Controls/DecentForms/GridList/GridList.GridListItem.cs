namespace DecentForms
{
  public partial class GridList
  {
    public class GridListItem
    {
      internal GridList      _Owner = null;
      private string        _Text = "";
      private bool          _Checked = false;
      internal bool         _Selected = false;
      internal int          _Index = -1;



      public GridListItem()
      {
      }



      public GridListItem( string Text )
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
          _Owner?.ItemModified( this );
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
          _Owner?.ItemModified( this );
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


}