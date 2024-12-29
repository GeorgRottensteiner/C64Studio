using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;



namespace DecentForms
{
  public partial class TreeView
  {
    [DebuggerDisplay( "Text = {_Text}, Level = {Level}" )]
    public class TreeNode : ICloneable
    {
      internal TreeView             _Owner = null;
      internal TreeNode             _Parent = null;
      public TreeNodeCollection     Nodes = new TreeNodeCollection( null, null );
      internal TreeNode             _Previous = null;
      internal TreeNode             _Next = null;

      private string                _Text = "";
      private bool                  _Expanded = false;
      private bool                  _Checked = false;
      private bool                  _Selected = false;

      // index in local TreeNodeCollection
      internal int                  _Index = -1;
      internal int                  _ImageIndex = -1;

      // global index of visible nodes
      internal int                  _VisualIndex = -1;

      internal int                  _TextWidth = -1;

      public object                 Tag = null;

      private Font                  _NodeFont = null;



      public TreeNode()
      {
        Nodes = new TreeNodeCollection( null, this );
      }



      public TreeNode( string Text )
      {
        _Text = Text;
      }



      public bool IsExpanded
      {
        get
        {
          return _Expanded;
        }
      }



      public string Text 
      {
        get
        {
          if ( ( _TextWidth == -1 )
          &&   ( _Owner != null ) )
          {
            _TextWidth = System.Windows.Forms.TextRenderer.MeasureText( _Text, _NodeFont ?? _Owner.Font ).Width;
          }
          return _Text;
        }
        set
        {
          if ( _Text != value )
          {
            _Text = value;
            if ( _Owner != null )
            {
              _TextWidth = System.Windows.Forms.TextRenderer.MeasureText( _Text, _NodeFont ?? _Owner.Font ).Width;
              _Owner.ItemModified( this );
            }
          }
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
          _Owner?.SelectItem( this );
        }
      }



      public int Index
      {
        get
        {
          return _Index;
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
        }
      }



      public TreeNode Parent
      {
        get
        {
          return _Parent;
        }
      }



      public TreeNode PreviousNode
      {
        get
        {
          return _Previous;
        }
      }



      public TreeNode NextNode
      {
        get
        {
          return _Next;
        }
      }



      public Rectangle Bounds 
      {
        get
        {
          if ( _Owner == null )
          {
            return Rectangle.Empty;
          }
          int   level = Level;
          int   visualIndex = VisualIndex - _Owner._PreviousScrollPosition;
          int   offsetOfTextLabel = _Owner.ExpandToggleItemSize + _Owner._SubNodeIndent * level;
          if ( _Owner.ImageList != null )
          {
            offsetOfTextLabel += _Owner.ItemHeight;
          }
          return new Rectangle( offsetOfTextLabel, visualIndex *  _Owner.ItemHeight, _TextWidth, _Owner.ItemHeight );
        }
      }



      public int Level 
      {
        get
        {
          int   level = 0;

          TreeNode node = this;

          while ( node.Parent != null )
          {
            ++level;
            node = node.Parent;
          }
          return level;
        }
      }



      public int VisualIndex 
      {
        get
        {
          if ( !IsVisible )
          {
            return -1;
          }
          if ( _VisualIndex != -1 )
          {
            return _VisualIndex;
          }
          var     node = this;
          int     totalIndex = 0;

          while ( node != null )
          {
            while ( node.PreviousNode != null )
            {
              ++totalIndex;
              node = node.PreviousNode;
              if ( node.IsExpanded )
              {
                totalIndex += node.GetVisibleNodeCount();
              }
            }
            node = node.Parent;
            if ( node != null )
            {
              ++totalIndex;
            }
          }
          _VisualIndex = totalIndex;
          return totalIndex;
        }
      }



      private int GetVisibleNodeCount()
      {
        int     totalCount = 0;
        foreach ( var node in Nodes )
        {
          ++totalCount;
          if ( node.IsExpanded )
          {
            totalCount += node.GetVisibleNodeCount();
          }
        }
        return totalCount;
      }



      public bool IsVisible 
      {
        get
        {
          var node = this;
          while ( node.Parent != null )
          {
            node = node.Parent;
            if ( !node.IsExpanded )
            {
              return false;
            }
          }
          return true;
        }
      }



      public Font NodeFont
      {
        get
        {
          return _NodeFont;
        }
        set
        {
          if ( _NodeFont != value )
          {
            _NodeFont = value;
            _TextWidth = System.Windows.Forms.TextRenderer.MeasureText( _Text, _NodeFont ?? _Owner.Font ).Width;
            _Owner?.ItemModified( this );
          }
        }
      }



      public TreeNode PrevVisibleNode
      {
        get
        {
          return TreeView.GetPreviousVisibleNode( this );
        }
      }



      public TreeNode NextVisibleNode
      {
        get
        {
          return TreeView.GetNextVisibleNode( this );
        }
      }



      public int GetNodeCount( bool IncludeSubTrees, bool VisibleOnly )
      {
        int total = Nodes.Count;
        if ( IncludeSubTrees )
        {
          for ( int i = 0; i < Nodes.Count; i++ )
          {
            if ( ( !VisibleOnly )
            ||   ( Nodes[i].IsExpanded ) )
            {
               total += Nodes[i].GetNodeCount( true, VisibleOnly );
            }
          }
        }

        return total;
      }



      public void Toggle()
      {
        if ( _Expanded )
        {
          var cancelArgs = new TreeViewCancelEventArgs( this, false, TreeViewAction.Collapse );
          _Owner?.BeforeCollapse?.Invoke( _Owner, cancelArgs );
          if ( cancelArgs.Cancel )
          {
            return;
          }
        }
        else
        {
          var cancelArgs = new TreeViewCancelEventArgs( this, false, TreeViewAction.Expand );
          _Owner?.BeforeExpand?.Invoke( _Owner, cancelArgs );
          if ( cancelArgs.Cancel )
          {
            return;
          }
        }
        if ( _Expanded )
        {
          Collapse( true );
        }
        else
        {
          Expand();
        }
      }



      public void Expand()
      {
        if ( !_Expanded )
        {
          _Expanded = true;
          if ( Nodes.Count > 0 )
          {
            _Owner?.ItemsModified();
            RecalcVisualIndexStartingWithMyself();
          }
          else
          {
            _Owner?.ItemModified( this );
          }
          _Owner?.AfterExpand?.Invoke( _Owner, new TreeViewEventArgs( this, TreeViewAction.Expand ) );
        }
      }



      internal void RecalcVisualIndexStartingWithMyself()
      {
        if ( ( _Owner != null )
        &&   ( _Owner._UpdateLocked ) )
        {
          _Owner._RequiresVisualIndexRecalc = true;
          return;
        }

        TreeNode node      = this;
        TreeNode prevNode  = null;
        while ( node != null )
        {
          node      = GetNextNode( node );
          if ( node != null )
          {
            if ( node.IsVisible )
            {
              if ( ( prevNode != null )
              &&   ( prevNode.IsVisible ) )
              {
                node._VisualIndex = prevNode.VisualIndex + 1;
              }
              else
              {
                var prevVisibleNode = GetPreviousVisibleNode( node );
                if ( prevVisibleNode != null )
                {
                  node._VisualIndex = prevVisibleNode.VisualIndex + 1;
                }
                else
                {
                  node._VisualIndex = -1;
                }
              }
              prevNode = node;
            }
            else
            {
              node._VisualIndex = -1;
            }
          }
        }
        /*
        node = Nodes[0];
        while ( node != null )
        {
          Debug.Log( $"{node.Text} VI {node.VisualIndex}" );
          node = GetNextNode( node );
        }*/
      }



      public void Collapse( bool IgnoreChildren = false )
      {
        if ( _Expanded )
        {
          if ( _Owner != null )
          {
            var cancelArgs = new TreeViewCancelEventArgs( this, false, TreeViewAction.Collapse );
            _Owner.BeforeCollapse?.Invoke( _Owner, cancelArgs );
            if ( cancelArgs.Cancel )
            {
              return;
            }
          }
          _Expanded = false;

          if ( Nodes.Count > 0 )
          {
            if ( !IgnoreChildren )
            {
              foreach ( var child in Nodes )
              {
                child._VisualIndex = -1;
                child.Collapse( false );
              }
            }
            _Owner?.ItemsModified();
            RecalcVisualIndexStartingWithMyself();
          }
          else
          {
            _Owner?.ItemModified( this );
          }
          _Owner?.AfterCollapse?.Invoke( _Owner, new TreeViewEventArgs( this, TreeViewAction.Collapse ) );
        }
      }



      public void Remove()
      {
        if ( _Parent != null )
        {
          _Parent.Nodes.Remove( this );
        }
        else if ( ( _Owner != null )
        &&        ( _Owner.Nodes.Contains( this ) ) )
        {
          _Owner.Nodes.Remove( this );
        }
      }



      public void EnsureVisible()
      {
        if ( _Owner == null )
        {
          return;
        }
        if ( _VisualIndex < _Owner.FirstVisibleNode.VisualIndex )
        {
          _Owner._ScrollBar.ScrollTo( _VisualIndex );
        }
        else if ( _VisualIndex > _Owner.FirstVisibleNode.VisualIndex + _Owner.VisibleItemCount - 1 )
        {
          _Owner._ScrollBar.ScrollTo( _VisualIndex + _Owner.VisibleItemCount - 1 );
        }
      }



      public object Clone()
      {
        Type type = GetType();

        var treeNode = new TreeNode();
        treeNode.Text         = Text;
        treeNode.ImageIndex   = _ImageIndex;
        treeNode._Expanded    = _Expanded;
        treeNode.Checked      = _Checked;
        treeNode.NodeFont     = NodeFont;
        treeNode.Tag          = Tag;

        if ( Nodes.Count > 0 )
        {
          treeNode.Nodes = new TreeNodeCollection( null, treeNode );
          foreach ( var node in Nodes )
          {
            treeNode.Nodes.Add( (TreeNode)node.Clone() );
          }
        }
        return treeNode;
      }

    }


  }

}