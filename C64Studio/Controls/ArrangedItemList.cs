using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace C64Studio
{
  public partial class ArrangedItemList : UserControl
  {
    // Declare the delegate (if using non-generic pattern).
    public delegate ListViewItem AddingItemEventHandler( object sender );
    public delegate void RemovingItemEventHandler( object sender, ListViewItem Item );
    public delegate void ItemModifiedEventHandler( object sender, ListViewItem Item );
    public delegate void ItemExchangingEventHandler( object sender, ListViewItem Item1, ListViewItem Item2 );
    public delegate void ItemExchangedEventHandler( object sender, ListViewItem Item1, ListViewItem Item2 );


    public event AddingItemEventHandler AddingItem;
    public event RemovingItemEventHandler RemovingItem;
    public event ItemModifiedEventHandler ItemAdded;
    public event ItemModifiedEventHandler ItemRemoved;
    public event ItemExchangedEventHandler MovingItem;
    public event ItemExchangedEventHandler ItemMoved;
    public event ItemModifiedEventHandler SelectedIndexChanged;

    public bool MustHaveOneElement { get; set; }



    public ArrangedItemList()
    {
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



    public ListView.ColumnHeaderCollection Columns
    {
      get
      {
        return listItems.Columns;
      }
    }



    public ListView.ListViewItemCollection Items
    {
      get
      {
        return listItems.Items;
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
      listItems.Items.Add( newItem );
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

      if ( MovingItem != null )
      {
        MovingItem( this, itemToMove, otherItem );
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

      if ( MovingItem != null )
      {
        MovingItem( this, itemToMove, otherItem );
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
