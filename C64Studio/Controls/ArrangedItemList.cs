using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using GR.Image;

namespace C64Studio
{
  public partial class ArrangedItemList : UserControl, IDPIHandlerResize
  {
    // Declare the delegate (if using non-generic pattern).
    public delegate ArrangedItemEntry AddingItemEventHandler( object sender );
    public delegate ArrangedItemEntry CloningItemEventHandler( object sender, ArrangedItemEntry Item );
    public delegate void RemovingItemEventHandler( object sender, ArrangedItemEntry Item );
    public delegate void ItemModifiedEventHandler( object sender, ArrangedItemEntry Item );
    public delegate bool ItemExchangingEventHandler( object sender, ArrangedItemEntry Item1, ArrangedItemEntry Item2 );
    public delegate void ItemExchangedEventHandler( object sender, ArrangedItemEntry Item1, ArrangedItemEntry Item2 );


    public event AddingItemEventHandler AddingItem;
    public event CloningItemEventHandler CloningItem;
    public event RemovingItemEventHandler RemovingItem;
    public event ItemModifiedEventHandler ItemAdded;
    public event ItemModifiedEventHandler ItemRemoved;
    // return true to allow move
    public event ItemExchangingEventHandler MovingItem;
    public event ItemExchangedEventHandler ItemMoved;
    public event ItemModifiedEventHandler SelectedIndexChanged;

    private ArrangedItemListCollection _Items;
    private bool      _HasOwnerDrawColumn = false;
    private bool      _AllowClone = true;
    private bool      _DoNotFireSelectedIndexChanged = false;

    public bool MustHaveOneElement { get; set; }

    
    public Color SelectionBackColor { get; set; } = SystemColors.Highlight;
    public Color SelectionTextColor { get; set; } = SystemColors.HighlightText;
    public Color HighlightColor { get; set; } = SystemColors.HotTrack;
    public Color HighlightTextColor { get; set; } = SystemColors.HighlightText;



    public ArrangedItemList()
    {
      _Items = new ArrangedItemListCollection( this );
      InitializeComponent();
      using ( var g = listItems.CreateGraphics() )
      {
        listItems.ItemHeight = (int)( g.MeasureString( "Ay", listItems.Font ).Height + 0.5f );
      }

      listItems.DrawItem += ListItems_DrawItem;
      listItems.DrawMode = DrawMode.OwnerDrawFixed;
      listItems.KeyDown += ListItems_KeyDown;
      UpdateUI();
    }



    private void ListItems_KeyDown( object sender, KeyEventArgs e )
    {
      if ( e.KeyCode == Keys.Delete )
      {
        btnDelete_Click( sender, new EventArgs() );
      }
    }



    private void ListItems_DrawItem( object sender, DrawItemEventArgs e )
    {
      if ( !_HasOwnerDrawColumn )
      {
        return;
      }
      e.DrawBackground();

      if ( e.Index != -1 )
      {
        var rect = listItems.GetItemRectangle( e.Index );

        var textColor = ForeColor;
        if ( ( e.State & DrawItemState.Selected ) == DrawItemState.Selected )
        {
          Rectangle r = new Rectangle(e.Bounds.Left + 4, e.Bounds.Top, TextRenderer.MeasureText( Items[e.Index].Text, Font ).Width, e.Bounds.Height);
          e.Graphics.FillRectangle( new SolidBrush( SelectionBackColor ), r );
          textColor = SelectionTextColor;
        }
        else if ( ( e.State & DrawItemState.HotLight ) == DrawItemState.HotLight )
        {
          Rectangle r = new Rectangle(e.Bounds.Left + 4, e.Bounds.Top, TextRenderer.MeasureText( Items[e.Index].Text, Font ).Width, e.Bounds.Height);
          e.Graphics.FillRectangle( new SolidBrush( HighlightColor ), r );
          textColor = HighlightTextColor;
        }
        e.Graphics.DrawString( Items[e.Index].Text, Font, new SolidBrush( textColor ), rect );
      }
      e.DrawFocusRectangle();
      //e.Graphics.FillRectangle( Brushes.Aqua, e.Bounds );
    }



    private void ListItems_DrawSubItem( object sender, DrawListViewSubItemEventArgs e )
    {
      if ( !_HasOwnerDrawColumn )
      {
        e.DrawDefault = true;
        return;
      }
      if ( e.ColumnIndex != 1 )
      {
        e.DrawDefault = true;
        return;
      }
      //e.DrawBackground();
      e.Graphics.FillRectangle( Brushes.Aqua, e.SubItem.Bounds );
      e.DrawText();
      e.DrawDefault = false;
    }



    public bool HasOwnerDrawColumn
    {
      get
      {
        return _HasOwnerDrawColumn;
      }

      set
      {
        if ( value != _HasOwnerDrawColumn )
        {
          _HasOwnerDrawColumn = value;
        }
      }
    }



    public bool AddButtonEnabled
    {
      get
      {
        return btnAdd.Enabled;
      }
      set
      {
        btnAdd.Enabled = value;
      }
    }



    public bool AllowClone
    {
      get
      {
        return _AllowClone;
      }
      set
      {
        _AllowClone = value;
        UpdateUI();
      }
    }



    public bool DeleteButtonEnabled
    {
      get
      {
        return btnDelete.Enabled;
      }
      set
      {
        btnDelete.Enabled = value;
      }
    }



    public bool MoveUpButtonEnabled
    {
      get
      {
        return btnMoveUp.Enabled;
      }
      set
      {
        btnMoveUp.Enabled = value;
      }
    }



    public bool MoveDownButtonEnabled
    {
      get
      {
        return btnMoveDown.Enabled;
      }
      set
      {
        btnMoveDown.Enabled = value;
      }
    }



    public ArrangedItemListCollection Items
    {
      get
      {
        return _Items;
      }
    }



    public int SelectedIndex
    {
      get
      {
        return listItems.SelectedIndex;
      }
      set
      {
        listItems.SelectedIndex = value;
      }
    }



    public ArrangedItemEntry SelectedItem
    {
      get
      {
        return (ArrangedItemEntry)listItems.SelectedItem;
      }
    }



    public ListBox.SelectedIndexCollection SelectedIndices
    {
      get
      {
        return listItems.SelectedIndices;
      }
    }



    public ArrangedItemListSelectedItemCollection SelectedItems
    {
      get
      {
        return new ArrangedItemListSelectedItemCollection( this );
      }
    }



    private void btnAdd_Click( object sender, EventArgs e )
    {
      ArrangedItemEntry   newItem = null;
      if ( AddingItem != null )
      {
        newItem = AddingItem( this );
        if ( newItem == null )
        {
          return;
        }
      }
      if ( newItem == null )
      {
        newItem = new ArrangedItemEntry( this );
      }
      newItem = _Items.Add( newItem );
      if ( ItemAdded != null )
      {
        ItemAdded( this, newItem );
      }
      UpdateUI();
    }



    private void listItems_SelectedIndexChanged( object sender, EventArgs e )
    {
      UpdateUI();

      if ( _DoNotFireSelectedIndexChanged )
      {
        return;
      }

      if ( SelectedIndexChanged != null )
      {
        if ( SelectedIndex == -1 )
        {
          SelectedIndexChanged( this, null );
        }
        else
        {
          SelectedIndexChanged( this, (ArrangedItemEntry)SelectedItems[0] );
        }
      }
    }



    private void btnDelete_Click( object sender, EventArgs e )
    {
      if ( listItems.SelectedIndices.Count == 0 )
      {
        return;
      }
      int   indexToRemove = listItems.SelectedIndices[0];
      var itemToRemove = (ArrangedItemEntry)SelectedItems[0];

      if ( RemovingItem != null )
      {
        RemovingItem( this, itemToRemove );
      }

      listItems.Items.Remove( itemToRemove );
      if ( ItemRemoved != null )
      {
        ItemRemoved( this, itemToRemove );
      }
      if ( ( indexToRemove >= listItems.Items.Count )
      &&   ( indexToRemove > 0 ) )
      {
        --indexToRemove;
      }

      if ( indexToRemove < listItems.Items.Count )
      {
        Items[indexToRemove].Selected = true;
      }

      UpdateUI();
    }



    public void UpdateUI()
    {
      btnClone.Enabled    = ( listItems.SelectedIndices.Count > 0 ) && ( AllowClone );
      btnMoveUp.Enabled   = ( ( listItems.SelectedIndices.Count > 0 ) && ( listItems.SelectedIndices[0] > 0 ) );
      btnMoveDown.Enabled = ( ( listItems.SelectedIndices.Count > 0 ) && ( listItems.SelectedIndices[0] + 1 < listItems.Items.Count ) );

      if ( MustHaveOneElement )
      {
        btnDelete.Enabled = ( listItems.SelectedIndices.Count > 0 ) && ( listItems.Items.Count > 1 );
      }
      else
      {
        btnDelete.Enabled = ( listItems.SelectedIndices.Count > 0 );
      }
    }



    private void btnMoveUp_Click( object sender, EventArgs e )
    {
      if ( ( listItems.SelectedIndices.Count == 0 )
      ||   ( listItems.SelectedIndices[0] == 0 ) )
      {
        return;
      }
      int indexToMove = listItems.SelectedIndices[0];
      ArrangedItemEntry  itemToMove = (ArrangedItemEntry)listItems.SelectedItems[0];
      ArrangedItemEntry  otherItem = (ArrangedItemEntry)listItems.Items[indexToMove - 1];

      bool    allowMove = true;
      if ( MovingItem != null )
      {
        allowMove = MovingItem( this, itemToMove, otherItem );
      }
      if ( !allowMove )
      {
        return;
      }

      listItems.Items.RemoveAt( indexToMove );
      listItems.Items.Insert( indexToMove - 1, itemToMove );
      itemToMove.Selected = true;

      if ( ItemMoved != null )
      {
        ItemMoved( this, itemToMove, otherItem );
      }
    }



    private void btnMoveDown_Click( object sender, EventArgs e )
    {
      if ( ( listItems.SelectedIndices.Count == 0 )
      ||   ( listItems.SelectedIndices[0] + 1 == listItems.Items.Count ) )
      {
        return;
      }
      int indexToMove = listItems.SelectedIndices[0];
      ArrangedItemEntry  itemToMove = (ArrangedItemEntry)listItems.SelectedItems[0];
      ArrangedItemEntry  otherItem = (ArrangedItemEntry)listItems.Items[indexToMove + 1];

      bool    allowMove = true;
      if ( MovingItem != null )
      {
        allowMove = MovingItem( this, itemToMove, otherItem );
      }
      if ( !allowMove )
      {
        return;
      }

      listItems.Items.RemoveAt( indexToMove );
      listItems.Items.Insert( indexToMove + 1, itemToMove );
      itemToMove.Selected = true;

      if ( ItemMoved != null )
      {
        ItemMoved( this, itemToMove, otherItem );
      }
    }



    private void ArrangedItemList_SizeChanged( object sender, EventArgs e )
    {
      RescaleButtons();
    }



    private void RescaleButtons()
    {
      // TODO - rearrange buttons
      //186+40-3
      //240 all
      //40 buttons
      //10 abstand
      int     buttonWidth = ( ClientSize.Width * 40 ) / 240;
      int     buttonDistance = 10;

      btnAdd.Size = new Size( buttonWidth, btnAdd.Height );

      int     offset = ( 1 * ( 40 + buttonDistance ) * ClientSize.Width ) / 240;
      btnClone.Location = new Point( offset, btnClone.Location.Y );
      btnClone.Size = new Size( buttonWidth, btnAdd.Height );

      offset = ( 2 * ( 40 + buttonDistance ) * ClientSize.Width ) / 240;
      btnDelete.Location = new Point( offset, btnDelete.Location.Y );
      btnDelete.Size = new Size( buttonWidth, btnClone.Height );

      offset = ( 3 * ( 40 + buttonDistance ) * ClientSize.Width ) / 240;
      btnMoveUp.Location = new Point( offset, btnDelete.Location.Y );
      btnMoveUp.Size = new Size( buttonWidth, btnAdd.Height );

      offset = ( 4 * ( 40 + buttonDistance ) * ClientSize.Width ) / 240;
      btnMoveDown.Location = new Point( offset, btnDelete.Location.Y );
      btnMoveDown.Size = new Size( buttonWidth, btnAdd.Height );
    }



    internal void InvalidateItem( int Index )
    {
      if ( ( Index < 0 )
      ||   ( Index >= listItems.Items.Count ) )
      {
        return;
      }
      // force redraw of listbox
      var offset = listItems.AutoScrollOffset;
      var origSelectedItem = listItems.SelectedItem;
      var tempItem = listItems.Items[Index];

      _DoNotFireSelectedIndexChanged = true;
      listItems.Items.RemoveAt( Index );
      listItems.Items.Insert( Index, tempItem );

      if ( origSelectedItem == tempItem )
      {
        listItems.SelectedItem = origSelectedItem;
      }
      _DoNotFireSelectedIndexChanged = false;
      listItems.AutoScrollOffset = offset;
    }



    private void btnClone_Click( object sender, EventArgs e )
    {
      if ( listItems.SelectedIndices.Count == 0 )
      {
        return;
      }
      int     indexToClone = listItems.SelectedIndices[0];
      var     itemToClone = (ArrangedItemEntry)SelectedItems[0];

      ArrangedItemEntry   newItem = null;

      if ( CloningItem != null )
      {
        newItem = CloningItem( this, itemToClone );
        if ( newItem == null )
        {
          return;
        }
      }
      if ( newItem == null )
      {
        newItem = new ArrangedItemEntry( this );
      }
      newItem.Text = itemToClone.Text;
      newItem = _Items.Add( newItem );
      if ( ItemAdded != null )
      {
        ItemAdded( this, newItem );
      }
      UpdateUI();
    }



    public void ResizeControl()
    {
      DPIHandler.AdjustControlSize( this );
      DPIHandler.AdjustControlChildsSize( this );

      RescaleButtons();
    }



  }



  public class ArrangedItemEntry
  {
    internal ArrangedItemList    _Owner = null;



    public ArrangedItemEntry()
    {
    }

    public ArrangedItemEntry( string TextArg )
    {
      this.Text = TextArg;
    }



    internal ArrangedItemEntry( ArrangedItemList Owner )
    {
      _Owner = Owner;
    }



    public override string ToString()
    {
      return Text;
    }



    public string Text 
    {
      get
      {
        return _Text;
      }

      set
      {
        if ( _Text != value )
        {
          _Text = value;
          if ( _Owner != null )
          {
            _Owner.InvalidateItem( Index );
          }
        }
      }
    }



    public object Tag { get; set; } = null;
    public int Index { get; set; } = -1;
    public bool Selected
    {
      get
      {
        if ( _Owner == null )
        {
          return false;
        }
        foreach ( var item in _Owner.SelectedItems )
        {
          if ( item == this )
          {
            return true;
          }
        }
        return false;
      }

      set
      {
        if ( _Owner == null )
        {
          return;
        }
        if ( value )
        {
          _Owner.SelectedItems.Add( this );
        }
        else
        {
          _Owner.SelectedItems.Remove( this );
        }
      }
    }

    private string    _Text;

  };



  public class ArrangedItemListCollection : ICollection
  {
    private ArrangedItemList  _Owner = null;
    private object   _SyncRoot = new object();
    private bool      _IsSynchronized = false;



    public int Count
    {
      get
      {
        return _Owner.listItems.Items.Count;
      }
    }

    public object SyncRoot
    {
      get
      {
        return _SyncRoot;
      }
    }

    public bool IsSynchronized
    {
      get
      {
        return _IsSynchronized;
      }
    }



    public ArrangedItemListCollection( ArrangedItemList owner )
    {
      _Owner = owner;
    }


    public ArrangedItemEntry Add( ArrangedItemEntry Item )
    {
      Item.Index = _Owner.listItems.Items.Add( Item );
      Item._Owner = _Owner;
      /*
      if ( _Owner.ItemAdded != null )
      {
        _Owner.ItemAdded( this, Value );
      }*/
      _Owner.UpdateUI();

      return Item;
    }



    public ArrangedItemEntry Add( string Value )
    {
      var entry = new ArrangedItemEntry( _Owner );
      entry.Text = Value;

      entry.Index = _Owner.listItems.Items.Add( entry );
      /*
      if ( _Owner.ItemAdded != null )
      {
        _Owner.ItemAdded( this, Value );
      }*/
      _Owner.UpdateUI();

      return entry;
    }



    public void CopyTo( Array array, int index )
    {
      _Owner.Items.CopyTo( array, index );
    }



    public IEnumerator GetEnumerator()
    {
      return _Owner.listItems.Items.GetEnumerator();
    }



    public ArrangedItemEntry this[int Index]
    {
      get
      {
        var item = (ArrangedItemEntry)_Owner.listItems.Items[Index];
        item.Index = Index;
        return item;
      }
      set
      {
        _Owner.listItems.Items[Index] = (ArrangedItemEntry)value;
        value.Index = Index;
      }
    }



    internal void Clear()
    {
      _Owner.listItems.Items.Clear();
      _Owner.UpdateUI();
    }



    internal void Remove( ArrangedItemEntry item )
    {
      _Owner.listItems.Items.Remove( item );
      _Owner.UpdateUI();
    }



    internal void Insert( int v, ArrangedItemEntry item )
    {
      _Owner.listItems.Items.Insert( v, item );
      _Owner.UpdateUI();
    }
  }



  public class ArrangedItemListSelectedItemCollection : ICollection
  {
    private ArrangedItemList  _Owner = null;
    private object   _SyncRoot = new object();
    private bool      _IsSynchronized = false;



    public int Count
    {
      get
      {
        return _Owner.listItems.SelectedItems.Count;
      }
    }

    public object SyncRoot
    {
      get
      {
        return _SyncRoot;
      }
    }

    public bool IsSynchronized
    {
      get
      {
        return _IsSynchronized;
      }
    }



    public void CopyTo( Array array, int index )
    {
      _Owner.listItems.SelectedItems.CopyTo( array, index );
    }



    public ArrangedItemListSelectedItemCollection( ArrangedItemList owner )
    {
      _Owner = owner;
    }


    public IEnumerator GetEnumerator()
    {
      return _Owner.listItems.SelectedItems.GetEnumerator();
    }



    public ArrangedItemEntry this[int Index]
    {
      get
      {
        var item = (ArrangedItemEntry)_Owner.listItems.SelectedItems[Index];
        item.Index = Index;
        item._Owner = _Owner;
        return item;
      }
    }



    internal void Clear()
    {
      _Owner.listItems.SelectedItems.Clear();
      _Owner.UpdateUI();
    }



    internal void Remove( ArrangedItemEntry item )
    {
      _Owner.listItems.SelectedItems.Remove( item );
      _Owner.UpdateUI();
    }



    internal void Add( ArrangedItemEntry item )
    {
      _Owner.listItems.SelectedItems.Add( item );
      _Owner.UpdateUI();
    }
  }


}
