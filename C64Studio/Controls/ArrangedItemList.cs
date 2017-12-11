using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using static System.Windows.Forms.ListView;

namespace C64Studio
{
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


    public ListViewItem Add( ListViewItem Value )
    {
      _Owner.listItems.Items.Add( Value );
      /*
      if ( _Owner.ItemAdded != null )
      {
        _Owner.ItemAdded( this, Value );
      }*/
      _Owner.UpdateUI();

      return Value;
    }



    public ListViewItem Add( string Value )
    {
      var result = _Owner.listItems.Items.Add( Value );
      /*
      if ( _Owner.ItemAdded != null )
      {
        _Owner.ItemAdded( this, Value );
      }*/
      _Owner.UpdateUI();

      return result;
    }

    public void CopyTo( Array array, int index )
    {
      _Owner.Items.CopyTo( array, index );
    }

    public IEnumerator GetEnumerator()
    {
      return _Owner.listItems.Items.GetEnumerator();
    }

    public ListViewItem this[int Index]
    {
      get
      {
        return _Owner.listItems.Items[Index];
      }
      set
      {
        _Owner.listItems.Items[Index] = value;
      }
    }

    internal void Clear()
    {
      _Owner.listItems.Items.Clear();
      _Owner.UpdateUI();
    }

    internal void Remove( ListViewItem item )
    {
      _Owner.listItems.Items.Remove( item );
      _Owner.UpdateUI();
    }

    internal void Insert( int v, ListViewItem item )
    {
      _Owner.listItems.Items.Insert( v, item );
      _Owner.UpdateUI();
    }
  }



  public partial class ArrangedItemList : UserControl
  {
    // Declare the delegate (if using non-generic pattern).
    public delegate ListViewItem AddingItemEventHandler( object sender );
    public delegate void RemovingItemEventHandler( object sender, ListViewItem Item );
    public delegate void ItemModifiedEventHandler( object sender, ListViewItem Item );
    public delegate bool ItemExchangingEventHandler( object sender, ListViewItem Item1, ListViewItem Item2 );
    public delegate void ItemExchangedEventHandler( object sender, ListViewItem Item1, ListViewItem Item2 );


    public event AddingItemEventHandler AddingItem;
    public event RemovingItemEventHandler RemovingItem;
    public event ItemModifiedEventHandler ItemAdded;
    public event ItemModifiedEventHandler ItemRemoved;
    // return true to allow move
    public event ItemExchangingEventHandler MovingItem;
    public event ItemExchangedEventHandler ItemMoved;
    public event ItemModifiedEventHandler SelectedIndexChanged;

    private ArrangedItemListCollection _Items;

    public bool MustHaveOneElement { get; set; }



    public ArrangedItemList()
    {
      _Items = new ArrangedItemListCollection( this );
      InitializeComponent();
      UpdateUI();
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



    public ListView.ColumnHeaderCollection Columns
    {
      get
      {
        return listItems.Columns;
      }
    }



    public ArrangedItemListCollection Items
    {
      get
      {
        return _Items;
      }
    }



    public ListView.SelectedIndexCollection SelectedIndices
    {
      get
      {
        return listItems.SelectedIndices;
      }
    }



    public ListView.SelectedListViewItemCollection SelectedItems
    {
      get
      {
        return listItems.SelectedItems;
      }
    }



    private void btnAdd_Click( object sender, EventArgs e )
    {
      ListViewItem  newItem = null;
      if ( AddingItem != null )
      {
        newItem = AddingItem( this );
        if ( newItem == null )
        {
          return;
        }
      }
      else
      {
        newItem = new ListViewItem();
        foreach ( System.Windows.Forms.ColumnHeader column in Columns )
        {
          newItem.SubItems.Add( "" );
        }
      }
      _Items.Add( newItem );
      //listItems.Items.Add( newItem );
      if ( ItemAdded != null )
      {
        ItemAdded( this, newItem );
      }
      UpdateUI();
    }



    private void listItems_SelectedIndexChanged( object sender, EventArgs e )
    {
      UpdateUI();

      if ( SelectedIndexChanged != null )
      {
        if ( listItems.SelectedIndices.Count > 0 )
        {
          SelectedIndexChanged( this, listItems.SelectedItems[0] );
        }
        else
        {
          SelectedIndexChanged( this, null );
        }
      }
    }



    private void btnDelete_Click( object sender, EventArgs e )
    {
      if ( listItems.SelectedIndices.Count == 0 )
      {
        return;
      }
      ListViewItem  itemToRemove = listItems.SelectedItems[0];

      if ( RemovingItem != null )
      {
        RemovingItem( this, itemToRemove );
      }

      listItems.Items.Remove( itemToRemove );
      if ( ItemRemoved != null )
      {
        ItemRemoved( this, itemToRemove );
      }

      UpdateUI();
    }



    public void UpdateUI()
    {
      btnMoveUp.Enabled = ( ( listItems.SelectedIndices.Count > 0 ) && ( listItems.SelectedIndices[0] > 0 ) );
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
      ListViewItem  itemToMove = listItems.SelectedItems[0];
      ListViewItem  otherItem = listItems.Items[indexToMove - 1];

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
      ListViewItem  itemToMove = listItems.SelectedItems[0];
      ListViewItem  otherItem = listItems.Items[indexToMove + 1];

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

      if ( ItemMoved != null )
      {
        ItemMoved( this, itemToMove, otherItem );
      }
    }



    private void ArrangedItemList_SizeChanged( object sender, EventArgs e )
    {
      // TODO - rearrange buttons
      //186+50-3
      //233 all
      //50 buttons
      //11 abstand
      int     buttonWidth = ( ClientSize.Width * 50 ) / 233;
      int     buttonOffset = ( ClientSize.Width * 11 ) / 233;

      btnAdd.Size = new Size( buttonWidth, btnAdd.Height );

      int     offset = ( 1 * ( 50 + 11 ) * ClientSize.Width ) / 233;
      btnDelete.Location = new Point( offset, btnDelete.Location.Y );
      btnDelete.Size = new Size( buttonWidth, btnAdd.Height );

      offset = ( 2 * ( 50 + 11 ) * ClientSize.Width ) / 233;
      btnMoveUp.Location = new Point( offset, btnDelete.Location.Y );
      btnMoveUp.Size = new Size( buttonWidth, btnAdd.Height );

      offset = ( 3 * ( 50 + 11 ) * ClientSize.Width ) / 233;
      btnMoveDown.Location = new Point( offset, btnDelete.Location.Y );
      btnMoveDown.Size = new Size( buttonWidth, btnAdd.Height );
    }



  }
}
