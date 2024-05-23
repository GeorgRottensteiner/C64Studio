using System.Diagnostics;
using System.Drawing;



namespace DecentForms
{
  public partial class TreeView
  {
    [DebuggerDisplay( "Text = {_Text}, Level = {Level}" )]
    public class TreeNode
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
            _TextWidth = System.Windows.Forms.TextRenderer.MeasureText( _Text, _Owner.Font ).Width;
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
              _TextWidth = System.Windows.Forms.TextRenderer.MeasureText( Text, _Owner.Font ).Width;
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
          int   visualIndex = VisualIndex - _Owner._PreviousScrollValue;
          int   offsetoOfTextLabel = _Owner.ExpandToggleItemSize + _Owner._SubNodeIndent * level;
          if ( _Owner.ImageList != null )
          {
            offsetoOfTextLabel += _Owner.ItemHeight;
          }
          return new Rectangle( offsetoOfTextLabel, visualIndex *  _Owner.ItemHeight, _TextWidth, _Owner.ItemHeight );
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



      public int GetNodeCount( bool IncludeSubTrees, bool OnlyVisible )
      {
        int total = Nodes.Count;
        if ( IncludeSubTrees )
        {
          for ( int i = 0; i < Nodes.Count; i++ )
          {
            if ( ( !OnlyVisible )
            ||   ( Nodes[i].IsExpanded ) )
            {
              total += Nodes[i].GetNodeCount( true, OnlyVisible );
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
        _Expanded = !_Expanded;
        _Owner?.Invalidate( Bounds );
        RecalcVisualIndexStartingWithMyself();
        if ( _Expanded )
        {
          _Owner?.AfterExpand?.Invoke( _Owner, new TreeViewEventArgs( this, TreeViewAction.Expand ) );
        }
        else
        {
          _Owner?.AfterCollapse?.Invoke( _Owner, new TreeViewEventArgs( this, TreeViewAction.Collapse ) );
        }
      }



      public void Expand()
      {
        if ( !_Expanded )
        {
          _Expanded = true;
          _Owner?.Invalidate();

          RecalcVisualIndexStartingWithMyself();
        }
      }



      private void RecalcVisualIndexStartingWithMyself()
      {
        /*
        if ( ( _Owner != null )
        &&   ( _Owner._UpdateLocked ) )
        {
          _Owner.Invalidate();
          return;
        }*/
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
          _Owner?.Invalidate();

          if ( !IgnoreChildren )
          {
            foreach ( var child in Nodes )
            {
              child._VisualIndex = -1;
              child.Collapse( false );
            }
          }
          RecalcVisualIndexStartingWithMyself();
        }
      }



    }






  }






}