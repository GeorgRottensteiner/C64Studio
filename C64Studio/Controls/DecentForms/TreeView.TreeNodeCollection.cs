using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;



namespace DecentForms
{
  public partial class TreeView
  {
    public class TreeNodeCollection : IEnumerable<TreeNode>
    {
      //private TreeView            _OwnerControl;
      public TreeView            _OwnerControl;
      private TreeNode            _Owner; 
      public List<TreeNode>       Nodes = new List<TreeNode>();



      internal TreeNodeCollection( TreeView Owner, TreeNode Node )
      {
        _OwnerControl = Owner;
        _Owner        = Node;
      }




      public int Count
      {
        get
        {
          return Nodes.Count;
        }
      }



      public void Add( TreeNode Item )
      {
        Item._Index               = Nodes.Count;
        Item._Owner               = _OwnerControl;
        Item.Nodes._OwnerControl  = _OwnerControl;
        if ( Item.Index > 0 )
        {
          Item._Previous = Nodes[Item.Index - 1];
          Nodes[Item.Index - 1]._Next = Item;

          // find last visual item in prev node
          var lastNode = Nodes[Item.Index - 1];
          while ( ( lastNode.Nodes.Count > 0 )
          &&      ( lastNode.IsExpanded ) )
          {
            lastNode = lastNode.Nodes.Last();
          }
          Item._VisualIndex = lastNode.VisualIndex + 1;

          // TODO - adapt visual index of all items after
        }
        else
        {
          Item._VisualIndex = -1;
        }
        Item.Nodes._Owner = Item;
        Item._Parent      = _Owner;
        Nodes.Add( Item );

        // recursively set owner for all child nodes as well!
        SetNodeOwner( Item, _OwnerControl );

        _OwnerControl?.ItemsModified();
      }



      private void SetNodeOwner( TreeNode Node, TreeView OwnerControl )
      {
        Node._Owner              = OwnerControl;
        Node.Nodes._OwnerControl = OwnerControl;
        foreach ( var node in Node.Nodes )
        {
          node._Owner               = OwnerControl;
          node.Nodes._OwnerControl  = OwnerControl;
          SetNodeOwner( node, OwnerControl );
        }
      }



      public TreeNode Add( string Text )
      {
        var newNode = new TreeNode( Text );
        Add( newNode );

        return newNode;
      }



      public void AddRange( string[] Items )
      {
        foreach ( var item in Items )
        {
          var newNode = new TreeNode( item );

          Add( newNode );
        }
        _OwnerControl?.ItemsModified();
      }



      public bool Remove( TreeNode Item )
      {
        if ( Item.Index == -1 )
        {
          return false;
        }
        RemoveAt( Item.Index );
        return true;
      }



      public void RemoveAt( int Index )
      {
        if ( ( Index < 0 )
        ||   ( Index >= Nodes.Count ) )
        {
          return;
        }
        for ( int i = Nodes[Index].Index + 1; i < Nodes.Count; i++ )
        {
          --Nodes[i]._Index;
          if ( Nodes[i]._VisualIndex != -1 )
          {
            --Nodes[i]._VisualIndex;
          }
        }
        var node = Nodes[Index];

        if ( ( _OwnerControl != null )
        &&   ( _OwnerControl.SelectedNode == node ) )
        {
          var newNode = GetNextVisibleNode( node );
          if ( newNode == null )
          {
            newNode = GetPreviousVisibleNode( node );
          }
          _OwnerControl.SelectedNode = newNode;
        }

        if ( node.PreviousNode != null )
        {
          node.PreviousNode._Next = node.NextNode;
        }
        if ( node.NextNode != null )
        {
          node.NextNode._Previous = node.PreviousNode;
        }
        node._Next      = null;
        node._Previous  = null;
        SetNodeOwner( node, null );
        _OwnerControl?.DetachNode( Nodes[Index] );

        Nodes.RemoveAt( Index );
        _OwnerControl?.ItemsModified();
      }



      public void RemoveRange( int Index, int Count )
      {
        if ( Index < 0 )
        {
          Index = 0;
        }
        if ( Count <= 0 )
        {
          return;
        }
        if ( Index + Count > Nodes.Count )
        {
          Count = Nodes.Count - Index;
        }
        if ( Count > 0 )
        {
          for ( int i = Index + Count; i < Nodes.Count; i++ )
          {
            Nodes[i]._Index -= Count;
          }
          Nodes.RemoveRange( Index, Count );
          _OwnerControl?.ItemsModified();
        }
      }



      public void Clear()
      {
        if ( Nodes.Count > 0 )
        {
          Nodes.Clear();
          _OwnerControl?.ItemsModified();
        }
      }



      public TreeNode this[int Index]
      {
        get
        {
          if ( ( Index < 0 )
          ||   ( Index >= Nodes.Count ) )
          {
            throw new ArgumentOutOfRangeException( $"Tried to access item {Index} of {Nodes.Count}" );
          }
          return Nodes[Index];
        }
        set
        {
          if ( ( Index < 0 )
          ||   ( Index >= Nodes.Count ) )
          {
            throw new ArgumentOutOfRangeException( $"Tried to access item {Index} of {Nodes.Count}" );
          }
          Nodes[Index] = value;
          _OwnerControl?.ItemModified( value );
        }
      }



      public IEnumerator<TreeNode> GetEnumerator()
      {
        return Nodes.GetEnumerator();
      }



      IEnumerator IEnumerable.GetEnumerator()
      {
        return Nodes.GetEnumerator();
      }



      public void Insert( int Index, TreeNode treeNode )
      {
        if ( ( Index < 0 )
        ||   ( Index >= Nodes.Count ) )
        {
          throw new ArgumentOutOfRangeException( $"Index {Index} is out of bounds!" );
        }
        treeNode._Owner   = _OwnerControl;
        treeNode._Parent  = _Owner;
        if ( Index > 0 )
        {
          Nodes[Index - 1]._Next = treeNode;
          treeNode._Previous = Nodes[Index - 1];
        }
        Nodes.Insert( Index, treeNode );
        if ( Index + 1 < Nodes.Count )
        {
          Nodes[Index + 1]._Previous = treeNode;
          treeNode._Next = Nodes[Index + 1];
        }
        treeNode._Index = Index;
        for ( int i = Index + 1; i < Nodes.Count; i++ )
        {
          ++Nodes[i]._Index;
          if ( Nodes[i]._VisualIndex != -1 )
          {
            ++Nodes[i]._VisualIndex;
          }
        }
        _OwnerControl?.ItemsModified();
      }



    }






  }






}