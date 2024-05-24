using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;



namespace DecentForms
{
  public partial class TreeView : ControlBase
  {
    public delegate void TreeViewCancelEventHandler( object sender, TreeViewCancelEventArgs e );
    public delegate void TreeViewEventHandler( object sender, TreeViewEventArgs e );
    public delegate void TreeNodeMouseClickEventHandler( object sender, TreeNodeMouseClickEventArgs e );



    private VScrollBar    _ScrollBar = new VScrollBar();
    private HScrollBar    _ScrollBarH = new HScrollBar();
    private bool          _ScrollAlwaysVisible = false;
    private int           _PreviousScrollPosition = 0;

    // cache max item width (since that's resource heavy, -1 means required to recalc)
    private int           _CachedMaxItemWidth = -1;

    private TreeNode      _FirstVisibleNode = null;
    private TreeNode      _SelectedNode = null;
    private TreeNode      _MouseOverNode = null;
    private TreeNode      _MouseOverToggleButtonNode = null;

    private int           _SubNodeIndent = 19;
    private int           _ExpandToggleItemSize = 23;

    private bool          _UpdateLocked = false;
    private bool          _RequiresUpdate = false;

    private System.Windows.Forms.ImageList     _ImageList = null;

    //public event TreeViewCancelEventHandler    BeforeCheck;
    //public event TreeViewEventHandler          AfterCheck;
    public event TreeViewCancelEventHandler       BeforeCollapse;
    public event TreeViewEventHandler             AfterCollapse;
    public event TreeViewCancelEventHandler       BeforeExpand;
    public event TreeViewEventHandler             AfterExpand;
    public event TreeViewCancelEventHandler       BeforeSelect;
    public event TreeViewEventHandler             AfterSelect;
    public event TreeNodeMouseClickEventHandler   NodeMouseClick;
    public event TreeNodeMouseClickEventHandler   NodeMouseDoubleClick;



    public TreeView()
    {
      BorderStyle = BorderStyle.SUNKEN;
      Nodes       = new TreeNodeCollection( this, null );

      Controls.Add( _ScrollBar );
      Controls.Add( _ScrollBarH );
      _ScrollBar.Scroll += _ScrollBar_Scroll;
      _ScrollBarH.Scroll += _ScrollBarH_Scroll;
      UpdateScrollbarState();
    }



    public new void Invalidate()
    {
      if ( _UpdateLocked )
      {
        _RequiresUpdate = true;
      }
      else
      {
        base.Invalidate();
      }
    }



    private void _ScrollBar_Scroll( ControlBase Sender )
    {
      if ( _PreviousScrollPosition != _ScrollBar.Value )
      {
        _PreviousScrollPosition = _ScrollBar.Value;

        if ( Nodes.Count > 0 )
        {
          var node = Nodes[0];

          for ( int i = 0; i < _PreviousScrollPosition; ++i )
          {
            node = GetNextVisibleNode( node );
            if ( node == null )
            {
              break;
            }
          }
          _FirstVisibleNode = node;
        }
        Invalidate();
      }
    }



    private void _ScrollBarH_Scroll( ControlBase Sender )
    {
      _DisplayOffsetX = _ScrollBarH.Value;
      Invalidate();
    }



    public System.Windows.Forms.ImageList ImageList
    {
      get
      {
        return _ImageList;
      }
      set
      {
        if ( _ImageList != value )
        {
          _ImageList = value;
          Invalidate();
        }
      }
    }



    public int ExpandToggleItemSize
    {
      get
      {
        return _ExpandToggleItemSize;
      }
    }



    protected override bool IsInputKey( System.Windows.Forms.Keys keyData )
    {
      return true;
    }



    [DefaultValue( 16 )]
    public int ItemHeight { get; set; } = 16;
    public TreeNodeCollection Nodes { get; private set; }
    public SelectionMode SelectionMode { get; set; }



    public bool ScrollAlwaysVisible 
    {
      get
      {
        return _ScrollAlwaysVisible;
      }
      set 
      {
        _ScrollAlwaysVisible = value;
        UpdateScrollbarState();
        Invalidate(); 
      } 
    }



    public void BeginUpdate()
    {
      _UpdateLocked = true;
      _RequiresUpdate = false;
    }



    public void EndUpdate()
    {
      if ( _UpdateLocked )
      {
        _UpdateLocked = false;
        if ( _RequiresUpdate )
        {
          _RequiresUpdate = false;
          Invalidate();
        }
      }
    }



    private void UpdateScrollbarState()
    {
      bool    needVerticalScrollbar   = VerticalScrollbarRequired();
      bool    needHorizontalScrollbar = HorizontalScrollbarRequired();

      int     potentialVScrollWidth = needVerticalScrollbar ? _ScrollBar.Width : 0;

      if ( MaxItemWidth > ClientSize.Width - potentialVScrollWidth )
      {
        _ScrollBarH.Visible = true;
        _ScrollBarH.Bounds = new System.Drawing.Rectangle( 0, ClientSize.Height - _ScrollBarH.Height, ClientSize.Width - potentialVScrollWidth, _ScrollBarH.Height );

        _ScrollBarH.Maximum = MaxItemWidth - ( ClientSize.Width - potentialVScrollWidth );

        float factor = ( ClientSize.Width - potentialVScrollWidth ) / (float)MaxItemWidth;
        _ScrollBarH.SetSliderSize( (int)( ( _ScrollBarH.Width - 2 * 17 ) * factor ) );

      }
      else
      {
        _ScrollBarH.Visible = false;
      }

      if ( needVerticalScrollbar )
      {
        _ScrollBar.Visible  = true;

        int usableHeight = ClientSize.Height;
        if ( needHorizontalScrollbar )
        {
          usableHeight -= _ScrollBarH.Height;
        }
        _ScrollBar.Bounds = new System.Drawing.Rectangle( ClientSize.Width - _ScrollBar.Width, 0, _ScrollBar.Width, usableHeight );

        int   visibleItemCount = 1;
        if ( ItemHeight == 0 )
        {
          visibleItemCount = 1;
        }
        else if ( needHorizontalScrollbar )
        {
          visibleItemCount = ( ClientSize.Height - _ScrollBarH.Height ) / ItemHeight;
        }
        else
        {
          visibleItemCount = ClientSize.Height / ItemHeight;
        }
        int     newMax = TotalVisibleNodeCount - visibleItemCount;
        if ( ( Nodes == null )
        ||   ( Nodes.Count == 0 ) )
        {
          _ScrollBar.Maximum = 0;
          _ScrollBar.SetSliderSize( _ScrollBar.Height - 2 * 17 );
        }
        else
        {

          if ( _ScrollBar.Value > newMax )
          {
            _ScrollBar.Value = newMax;
          }
          _ScrollBar.Maximum = newMax;

          float factor = usableHeight / ( (float)TotalVisibleNodeCount * ItemHeight );
          _ScrollBar.SetSliderSize( (int)( ( _ScrollBar.Height - 2 * 17 ) * factor ) );
        }
      }
      else
      {
        if ( !_ScrollAlwaysVisible )
        {
          _ScrollBar.Visible = false;
        }
        _ScrollBar.Value = 0;
        _ScrollBar.Maximum = 0;
      }

      _ActualWorkWidth = 0;
      _ActualWorkHeight = 0;
      if ( needVerticalScrollbar )
      {
        _ActualWorkWidth = ClientSize.Width - _ScrollBar.Width;
      }
      if ( needHorizontalScrollbar )
      {
        _ActualWorkHeight = ClientSize.Height - _ScrollBarH.Height;
      }
    }



    private bool HorizontalScrollbarRequired()
    {
      if ( VerticalScrollbarRequired() )
      {
        return MaxItemWidth > ClientSize.Width - _ScrollBar.Width;
      }
      return MaxItemWidth > ClientSize.Width;
    }



    protected override void OnSizeChanged( System.EventArgs e )
    {
      base.OnSizeChanged( e );

      UpdateScrollbarState();
    }



    private bool VerticalScrollbarRequired()
    {
      return ( TotalVisibleNodeCount > VisibleItemCount ) 
          || ( _ScrollAlwaysVisible );
    }



    public int VisibleItemCount
    {
      get
      {
        if ( ItemHeight == 0 )
        {
          return 1;
        }
        if ( _ScrollBarH.Visible )
        {
          return ( ClientSize.Height - _ScrollBarH.Height ) / ItemHeight;
        }
        return ClientSize.Height / ItemHeight;
      }
    }



    public int UsableItemWidth
    {
      get
      {
        if ( !_ScrollBar.Visible )
        {
          return Math.Max( MaxItemWidth, ClientSize.Width );
        }
        return Math.Max( MaxItemWidth, ClientSize.Width - _ScrollBar.Width );
      }
    }



    public TreeNode MouseOverNode
    {
      get
      {
        return _MouseOverNode;
      }
    }



    public TreeNode SelectedNode
    {
      get
      {
        return _SelectedNode;
      }
      set
      {
        var cancelArgs = new TreeViewCancelEventArgs( _SelectedNode, false, TreeViewAction.ByMouse );
        BeforeSelect?.Invoke( this, cancelArgs );
        if ( cancelArgs.Cancel )
        {
          return;
        }

        if ( value == null )
        {
          if ( _SelectedNode != null )
          {
            Invalidate( _SelectedNode.Bounds );
          }
          _SelectedNode = null;
          return;
        }
        if ( value._Owner != this )
        {
          throw new ArgumentException( "The node to be selected does not belong to this TreeView!" );
        }
        if ( _SelectedNode != null )
        {
          Invalidate( _SelectedNode.Bounds );
        }
        _SelectedNode = value;
        if ( _SelectedNode != null )
        {
          Invalidate( _SelectedNode.Bounds );
          AfterSelect?.Invoke( this, new TreeViewEventArgs( _SelectedNode ) );
        }
      }
    }



    protected int MaxItemWidth 
    {
      get
      {
        if ( _CachedMaxItemWidth != -1 )
        {
          return _CachedMaxItemWidth;
        }
        RecalcMaxItemWidth();
        return _CachedMaxItemWidth;
      }
    }



    protected int TotalVisibleNodeCount 
    {
      get
      {
        int     totalCount = 0;
        if ( Nodes == null )
        {
          return 0;
        }

        totalCount = Nodes.Count;
        foreach ( var node in Nodes )
        {
          if ( node.IsExpanded )
          {
            totalCount += node.GetNodeCount( true, true );
          }
        }
        return totalCount;
      }
    }



    public TreeNode FirstVisibleNode 
    {
      get
      {
        return _FirstVisibleNode;
      }
    }



    private void RecalcMaxItemWidth()
    {
      if ( ( Nodes == null )
      ||   ( Nodes.Count == 0 ) )
      {
        _CachedMaxItemWidth = 0;
        return;
      }

      int     curMaxWidth = 0;

      var node = Nodes[0];

      while ( node != null )
      {
        var textSize = System.Windows.Forms.TextRenderer.MeasureText( node.Text, Font );
        curMaxWidth = Math.Max( textSize.Width, curMaxWidth );

        node = GetNextVisibleNode( node );
      }
      _CachedMaxItemWidth = curMaxWidth;
    }



    public static TreeNode GetNextVisibleNode( TreeNode Node )
    {
      if ( Node == null )
      {
        return null;
      }
      if ( ( Node.Nodes.Count > 0 )
      &&   ( Node.IsExpanded ) )
      {
        return Node.Nodes[0];
      }
      if ( Node.NextNode != null )
      {
        return Node.NextNode;
      }
      // has the parent above a next node?
      if ( Node.Parent == null )
      {
        return null;
      }
      var node = Node.Parent;
      while ( node != null ) 
      {
        if ( node.NextNode != null )
        {
          return node.NextNode;
        }
        // parent node has no next sibling
        node = node.Parent;
      }
      return null;
    }



    public static TreeNode GetPreviousVisibleNode( TreeNode Node )
    {
      while ( Node != null )
      {
        var   prevNode = Node.PreviousNode;

        if ( ( prevNode != null )
        &&   ( prevNode.IsVisible ) )
        {
          while ( ( prevNode.IsVisible )
          &&      ( prevNode.IsExpanded )
          &&      ( prevNode.Nodes.Count > 0 ) )
          {
            prevNode = prevNode.Nodes.Last();
          }
          return prevNode;
        }
        var parentNode = Node.Parent;
        if ( parentNode == null )
        {
          return null;
        }
        if ( ( parentNode != null )
        &&   ( parentNode.IsVisible ) )
        {
          return parentNode;
        }
        Node = parentNode;
      }
      return null;
    }



    public static TreeNode GetNextNode( TreeNode Node )
    {
      if ( Node == null )
      {
        return null;
      }
      if ( Node.Nodes.Count > 0 )
      {
        return Node.Nodes[0];
      }
      if ( Node.NextNode != null )
      {
        return Node.NextNode;
      }
      // has the parent above a next node?
      if ( Node.Parent == null )
      {
        return null;
      }
      var node = Node.Parent;
      while ( node != null )
      {
        if ( node.NextNode != null )
        {
          return node.NextNode;
        }
        // parent node has no next sibling
        node = node.Parent;
      }
      return null;
    }



    protected override void OnControlEvent( ControlEvent Event )
    {
      switch ( Event.Type )
      {
        case ControlEvent.EventType.FOCUS_LOST:
          Invalidate();
          break;
        case ControlEvent.EventType.FOCUSED:
          Invalidate();
          break;
        case ControlEvent.EventType.MOUSE_ENTER:
          Invalidate();
          break;
        case ControlEvent.EventType.MOUSE_DOUBLE_CLICK:
          var hitAreaDC = HitTest( Event.MouseX, Event.MouseY );
          if ( ( ( hitAreaDC.Location & TreeViewHitTestLocations.ONITEM ) != 0 )
          &&     ( hitAreaDC.Node != null ) )
          {
            NodeMouseDoubleClick?.Invoke( this, new TreeNodeMouseClickEventArgs( hitAreaDC.Node, Event.MouseButtons, Event.MouseX, Event.MouseY ) );
          }
          break;
        case ControlEvent.EventType.MOUSE_WHEEL:
          if ( ( _ScrollBarH.Visible )
          &&   ( _ScrollBarH.Bounds.Contains( Event.MouseX, Event.MouseY ) ) )
          {
            _ScrollBarH.RaiseControlEvent( Event );
          }
          else if ( _ScrollBar.Visible )
          {
            _ScrollBar.RaiseControlEvent( Event );
          }
          break;
        case ControlEvent.EventType.MOUSE_UPDATE:
          {
            var hitArea = HitTest( Event.MouseX, Event.MouseY );
            if ( ( hitArea.Location & TreeViewHitTestLocations.ONITEM ) != 0 )
            {
              var nodeBelow = hitArea.Node;
              if ( nodeBelow != _MouseOverNode )
              {
                if ( _MouseOverNode != null )
                {
                  Invalidate( _MouseOverNode.Bounds );
                }
                _MouseOverNode = nodeBelow;
                if ( _MouseOverNode != null )
                {
                  Invalidate( _MouseOverNode.Bounds );
                }
              }
              if ( ( Event.MouseButtons & 1 ) != 0 )
              {
                if ( nodeBelow != _SelectedNode )
                {
                  _SelectedNode = nodeBelow;
                }
              }
              _MouseOverToggleButtonNode = null;
            }
            else
            {
              _MouseOverToggleButtonNode = null;
              if ( hitArea.Location == TreeViewHitTestLocations.ONITEMBUTTON )
              {
                _MouseOverToggleButtonNode = hitArea.Node;
              }
              if ( _MouseOverNode != null )
              {
                Invalidate( _MouseOverNode.Bounds );
                _MouseOverNode = null;
              }
            }
          }
          break;
        case ControlEvent.EventType.MOUSE_LEAVE:
          if ( _MouseOverNode != null )
          {
            Invalidate( _MouseOverNode.Bounds );
            _MouseOverNode = null;
          }
          Invalidate();
          break;
        case ControlEvent.EventType.MOUSE_DOWN:
          Focus();
          if ( _MouseOverToggleButtonNode != null )
          {
            _MouseOverToggleButtonNode.Toggle();
            Invalidate();
          }
          else if ( _MouseOverNode != SelectedNode )
          {
            SelectedNode = _MouseOverNode;
          }
          Capture     = true;
          break;
        case ControlEvent.EventType.MOUSE_UP:
          Capture = false;
          Invalidate();
          break;
        case ControlEvent.EventType.KEY_DOWN:
          if ( Event.Key == System.Windows.Forms.Keys.Down )
          {
            if ( _SelectedNode == null )
            {
              if ( Nodes.Count > 0 )
              {
                _SelectedNode = Nodes[0];
                Invalidate( _SelectedNode.Bounds );
              }
            }
            else
            {
              var   newNode = GetNextVisibleNode( _SelectedNode );
              if ( newNode == null )
              {
                break;
              }
              Invalidate( _SelectedNode.Bounds );
              _SelectedNode = newNode;
              if ( _SelectedNode != null )
              {
                Invalidate( _SelectedNode.Bounds );
              }
              // TODO - event
            }
          }
          else if ( Event.Key == System.Windows.Forms.Keys.Up )
          {
            if ( _SelectedNode == null )
            {
              if ( Nodes.Count > 0 )
              {
                // todo, select last?
                var newNode = Nodes.Last();
                while ( ( newNode.IsExpanded )
                &&      ( newNode.Nodes.Count > 0 ) )
                {
                  newNode = newNode.Nodes.Last();
                }
                _SelectedNode = newNode;
                Invalidate( _SelectedNode.Bounds );
              }
            }
            else
            {
              var   newNode = GetPreviousVisibleNode( _SelectedNode );
              if ( newNode == null )
              {
                break;
              }
              Invalidate( _SelectedNode.Bounds );
              _SelectedNode = newNode;
              if ( _SelectedNode != null )
              {
                Invalidate( _SelectedNode.Bounds );
              }
              // TODO - event
            }
          }
          else if ( Event.Key == System.Windows.Forms.Keys.Left )
          {
            if ( _SelectedNode != null )
            {
              if ( ( _SelectedNode.Nodes.Count == 0 )
              &&   ( _SelectedNode.Parent != null ) )
              {
                Invalidate( _SelectedNode.Bounds );
                _SelectedNode = _SelectedNode.Parent;
                Invalidate( _SelectedNode.Bounds );
                break;
              }
              if ( ( _SelectedNode.Nodes.Count > 0 )
              &&   ( _SelectedNode.IsExpanded ) )
              {
                _SelectedNode.Collapse( true );
              }
            }
            
          }
          else if ( Event.Key == System.Windows.Forms.Keys.Right )
          {
            if ( ( _SelectedNode != null )
            &&   ( _SelectedNode.Nodes.Count > 0 ) )
            {
              if ( !_SelectedNode.IsExpanded )
              {
                _SelectedNode.Expand();
              }
              else
              {
                Invalidate( _SelectedNode.Bounds );
                _SelectedNode = _SelectedNode.Nodes[0];
                Invalidate( _SelectedNode.Bounds );
              }
            }
          }
            /*
            else if ( Event.Key == System.Windows.Forms.Keys.PageUp )
            {
              int   newIndex = _SelectedNode;
              if ( ( _SelectedNode == -1 )
              &&   ( Nodes.Count > 0 ) )
              {
                newIndex = Nodes.Count - 1;
              }
              else
              {
                newIndex = Math.Max( 0, _SelectedNode - VisibleItemCount + 1 );
              }
              if ( newIndex != _SelectedNode )
              {
                SelectedIndex = newIndex;
              }
            }
            else if ( Event.Key == System.Windows.Forms.Keys.PageDown )
            {
              int   newIndex = _SelectedNode;
              if ( ( _SelectedNode == -1 )
              &&   ( Nodes.Count > 0 ) )
              {
                newIndex = 0;
              }
              else
              {
                newIndex = Math.Min( Nodes.Count - 1, _SelectedNode + VisibleItemCount - 1 );
              }
              if ( newIndex != _SelectedNode )
              {
                SelectedIndex = newIndex;
              }
            }
            else if ( Event.Key == System.Windows.Forms.Keys.Home )
            {
              if ( Nodes.Count > 0 )
              {
                if ( _SelectedNode != 0 )
                {
                  SelectedIndex = 0;
                }
              }
            }
            else if ( Event.Key == System.Windows.Forms.Keys.End )
            {
              if ( Nodes.Count > 0 )
              {
                if ( _SelectedNode + 1 != Nodes.Count )
                {
                  SelectedIndex = Nodes.Count - 1;
                }
              }
            }*/
            break;
        case ControlEvent.EventType.KEY_UP:
          if ( Focused )
          {
            if ( Event.Key == System.Windows.Forms.Keys.Space )
            {
            }
          }
          break;
      }
      base.OnControlEvent( Event );
    }



    private TreeViewHitTestInfo HitTest( int X, int Y )
    {
      TreeNode    node = null;
      var         loc = TreeViewHitTestLocations.NOWHERE;

      if ( ( X < 0 )
      ||   ( X >= UsableItemWidth )
      ||   ( Y < 0 )
      ||   ( Y >= ClientSize.Height )
      ||   ( Nodes.Count == 0 ) )
      {
        // nowhere
      }
      else
      {
        int       visibleNodeIndex = Y / ItemHeight;

        TreeNode  treeNode = FirstVisibleNode;

        while ( ( treeNode != null )
        &&      ( visibleNodeIndex > 0 ) )
        {
          treeNode = GetNextVisibleNode( treeNode );
          --visibleNodeIndex;
        }

        if ( treeNode != null )
        {
          // treeNode is the node in the "line" we're positioned at
          var bounds = treeNode.Bounds;
          node = treeNode;

          if ( GetImageRect( node ).Contains( X, Y ) )
          {
            loc = TreeViewHitTestLocations.ONITEMICON;
          }
          else if ( X > bounds.Right )
          {
            loc = TreeViewHitTestLocations.ONITEMRIGHT;
          }
          else if ( X >= bounds.Left )
          {
            loc = TreeViewHitTestLocations.ONITEMLABEL;
          }
          else if ( GetToggleRect( treeNode ).Contains( X, Y ) )
          {
            loc = TreeViewHitTestLocations.ONITEMBUTTON;
          }
          else
          {
            // TODO - icon/state image (difference?)
            loc = TreeViewHitTestLocations.ONITEMINDENT;
          }
        }
      }
      return new TreeViewHitTestInfo( node, loc );
    }



    private TreeNode GetNodeAt( int X, int Y )
    {
      if ( ( X < 0 )
      ||   ( X >= UsableItemWidth )
      ||   ( Y < 0 )
      ||   ( Y >= ClientSize.Height )
      ||   ( Nodes.Count == 0 ) )
      {
        return null;
      }
      int       visibleNodeIndex = Y / ItemHeight;

      TreeNode  treeNode = FirstVisibleNode;

      while ( ( treeNode != null )
      &&      ( visibleNodeIndex > 0 ) )
      {
        treeNode = GetNextVisibleNode( treeNode );
        --visibleNodeIndex;
      }

      if ( treeNode == null )
      {
        return null;
      }
      if ( !treeNode.Bounds.Contains( X, Y ) )
      {
        return null;
      }
      return treeNode;
    }



    protected override void OnPaint( ControlRenderer Renderer )
    {
      Renderer.RenderTreeView();
    }



    private void SelectItem( TreeNode Item )
    {
      throw new System.NotImplementedException();
    }



    private void ItemModified( TreeNode Item )
    {
      _CachedMaxItemWidth = -1;
      Invalidate( Item.Bounds );
    }



    private void DetachNode( TreeNode Node )
    {
      Node._Parent    = null;
      Node._Previous  = null;
      // TODO - adjust index of next nodes
      Node._Next      = null;

      DetachChildNode( Node );
    }



    private void DetachChildNode( TreeNode Node )
    {
      Node._Index       = -1;
      Node._VisualIndex = -1;
      Node._Owner       = null;

      foreach ( var node in Node.Nodes )
      {
        DetachChildNode( node );
      }
    }



    private void ItemsModified()
    {
      _CachedMaxItemWidth = -1;
      if ( _FirstVisibleNode == null )
      {
        if ( Nodes.Count > 0 )
        {
          _FirstVisibleNode = Nodes[0];
        }
      }
      UpdateScrollbarState();
      Invalidate();
    }



    internal Rectangle GetToggleRect( TreeNode Node )
    {
      var   rect = Node.Bounds;
      int   rectSize = (int)( rect.Height * 9 / 16 );

      int   extraOffset = 0;
      if ( ImageList != null )
      {
        extraOffset += ItemHeight;
      }

      rect = new Rectangle( rect.Left - ExpandToggleItemSize - extraOffset, rect.Top, ExpandToggleItemSize, rect.Height );
      rect = new Rectangle( rect.Left + ( rect.Width - rectSize ) / 2, rect.Top + rectSize / 2, rectSize, rectSize );

      return rect;
    }



    internal Rectangle GetImageRect( TreeNode Node )
    {
      var   rect = Node.Bounds;
      int   rectSize = rect.Height / 2;

      if ( ImageList != null )
      {
        rect = new Rectangle( rect.Left - ItemHeight, rect.Top, ExpandToggleItemSize, rect.Height );
      }
      else
      {
        rect = Rectangle.Empty;
      }

      return rect;
    }



    public enum TreeViewAction
    {
      /// <summary>
      ///  The action is unknown.
      /// </summary>
      Unknown = 0,

      /// <summary>
      ///  The event was caused by a keystroke.
      /// </summary>
      ByKeyboard = 1,

      /// <summary>
      ///  The event was caused by a mouse click.
      /// </summary>
      ByMouse = 2,

      /// <summary>
      ///  The tree node is collapsing.
      /// </summary>
      Collapse = 3,

      /// <summary>
      ///  The tree node is expanding.
      /// </summary>
      Expand = 4,
    }



    [Flags]
    public enum TreeViewHitTestLocations
    {
      NOWHERE         = 0x0001,
      ONITEMICON      = 0x0002,
      ONITEMLABEL     = 0x0004,
      ONITEM          = ( ONITEMICON | ONITEMLABEL | ONITEMSTATEICON ),
      ONITEMINDENT    = 0x0008,
      ONITEMBUTTON    = 0x0010,
      ONITEMRIGHT     = 0x0020,
      ONITEMSTATEICON = 0x0040,
                      
      ABOVE           = 0x0100,
      BELOW           = 0x0200,
      TORIGHT         = 0x0400,
      TOLEFT          = 0x0800
    }

    private class TreeViewHitTestInfo
    {
      private readonly TreeViewHitTestLocations     _Locations;
      private readonly TreeNode                     _Node;


      public TreeViewHitTestInfo( TreeNode Node, TreeViewHitTestLocations Location )
      {
        _Node       = Node;
        _Locations  = Location;
      }



      public TreeNode Node
      {
        get
        {
          return _Node;
        }
      }



      public TreeViewHitTestLocations Location
      {
        get
        {
          return _Locations;
        }
      }



    }



    public class TreeViewEventArgs : EventArgs
    {
      public TreeViewEventArgs( TreeNode node )
      {
        Node = node;
        Action = TreeViewAction.Unknown;
      }



      public TreeViewEventArgs( TreeNode node, TreeViewAction action )
      {
        Node = node;
        Action = action;
      }



      public TreeNode Node { get; }



      /// <summary>
      ///  An event specific action-flag.
      /// </summary>
      public TreeViewAction Action { get; }
    }



    public class TreeNodeMouseClickEventArgs
    {

      public TreeNodeMouseClickEventArgs( TreeNode node, uint button, int x, int y )
      {
        Node = node;
        MouseX = x;
        MouseY = y;
        MouseButtons = button;
        Action = TreeViewAction.Unknown;
      }



      public TreeNodeMouseClickEventArgs( TreeNode node, TreeViewAction action, uint button, int x, int y )
      {
        Node = node;
        MouseX = x;
        MouseY = y;
        MouseButtons = button;
        Action = action;
      }



      public int MouseX 
      {
        get;
      }

      public int MouseY
      {
        get;
      }

      public uint MouseButtons
      {
        get;
      }

      public TreeNode Node
      {
        get;
      }



      /// <summary>
      ///  An event specific action-flag.
      /// </summary>
      public TreeViewAction Action
      {
        get;
      }
    }



    public class TreeViewCancelEventArgs : CancelEventArgs
    {
      public TreeViewCancelEventArgs( TreeNode node, bool cancel, TreeViewAction action )
          : base( cancel )
      {
        Node = node;
        Action = action;
      }

      public TreeNode Node { get; }

      public TreeViewAction Action { get; }
    }






  }






}