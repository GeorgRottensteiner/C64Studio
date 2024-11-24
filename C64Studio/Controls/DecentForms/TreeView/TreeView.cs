using GR.Image;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;



namespace DecentForms
{
  public partial class TreeView : ControlBase
  {
    public delegate void TreeViewCancelEventHandler( DecentForms.ControlBase Sender, TreeViewCancelEventArgs e );
    public delegate void TreeViewEventHandler( DecentForms.ControlBase Sender, TreeViewEventArgs e );
    public delegate void TreeNodeMouseClickEventHandler( DecentForms.ControlBase Sender, TreeNodeMouseClickEventArgs e );
    public delegate void NodeLabelEditEventHandler( DecentForms.ControlBase Sender, NodeLabelEditEventArgs e );
    public delegate void DrawTreeNodeEventHandler( DecentForms.ControlBase Sender, DrawTreeNodeEventArgs e );


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

    private TreeNode      _EditedNode = null;
    private TextBox       _PopupEditControl = null;

    private int           _SubNodeIndent = 19;
    private int           _ExpandToggleItemSize = 23;

    private bool          _UpdateLocked = false;
    private bool          _RequiresUpdate = false;
    private bool          _RequiresScrollbarUpdate = false;
    private bool          _RequiresVisualIndexRecalc = false;
    private Point         _MouseDownPos = new Point();

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

    public event NodeLabelEditEventHandler        BeforeLabelEdit;
    public event NodeLabelEditEventHandler        AfterLabelEdit;

    public event DrawTreeNodeEventHandler         DrawNode;
    public event DrawTreeNodeEventHandler         DrawNodeImage;

    public event System.Windows.Forms.ItemDragEventHandler    ItemDrag;

    private string        _SelectingByKeyPressPrefix = "";
    private long          _SelectingByKeyPressPreviousTicks = 0;
    private bool          _SelectingByKeyPressUsingSingleStartLetterMode = true;



    public TreeView()
    {
      BorderStyle = BorderStyle.SUNKEN;
      Nodes       = new TreeNodeCollection( this, null );

      Controls.Add( _ScrollBar );
      Controls.Add( _ScrollBarH );
      _ScrollBar.Scroll += _ScrollBar_Scroll;
      _ScrollBarH.Scroll += _ScrollBarH_Scroll;

      ItemHeight = (int)( ItemHeight * DPIHandler.DPIY / 96.0f + 0.5f );
      UpdateScrollbarState();
    }



    protected override void OnLostFocus( EventArgs e )
    {
      if ( ( _EditedNode != null )
      &&   ( !_PopupEditControl.Focused ) )
      {
        EndEdit( true );
      }
      base.OnLostFocus( e );
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
        else
        {
          _FirstVisibleNode = null;
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



    public bool LabelEdit { get; set; } = false;
    public bool AllowDrag { get; set; } = false;

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
        if ( _RequiresVisualIndexRecalc )
        {
          _RequiresVisualIndexRecalc = false;
          if ( Nodes.Any() )
          {
            Nodes[0].RecalcVisualIndexStartingWithMyself();
          }
        }
        if ( _RequiresScrollbarUpdate )
        {
          _RequiresScrollbarUpdate = false;
          UpdateScrollbarState();
        }
        if ( _RequiresUpdate )
        {
          _RequiresUpdate = false;
          Invalidate();
        }
      }
    }



    private void UpdateScrollbarState()
    {
      if ( _UpdateLocked )
      {
        _RequiresScrollbarUpdate = true;
        return;
      }
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
          _PreviousScrollPosition = 0;
        }
        else
        {
          if ( _ScrollBar.Value > newMax )
          {
            _ScrollBar.ScrollTo( newMax );
          }
          _ScrollBar.Maximum = newMax;

          float factor = usableHeight / ( (float)TotalVisibleNodeCount * ItemHeight );
          _ScrollBar.SetSliderSize( (int)( ( _ScrollBar.Height - 2 * 17 ) * factor ) );

          _ScrollBar.LargeChange = visibleItemCount - 1;
          if ( _PreviousScrollPosition > _ScrollBar.Maximum )
          {
            _PreviousScrollPosition = _ScrollBar.Maximum;
          }
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
        _PreviousScrollPosition = 0;
        if ( Nodes.Count > 0 )
        {
          _FirstVisibleNode = Nodes[0];
        }
        else
        {
          _FirstVisibleNode = null;
        }
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
          _SelectedNode.EnsureVisible();
          Invalidate( _SelectedNode.Bounds );
        }
        AfterSelect?.Invoke( this, new TreeViewEventArgs( _SelectedNode ) );
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



    public TreeNode LastVisibleNode
    {
      get
      {
        var node = _FirstVisibleNode;
        while ( node != null )
        {
          var nextNode = GetNextVisibleNode( node );
          if ( nextNode == null )
          {
            return node;
          }
          node = nextNode;
        }
        return null;
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
                // start dragging?
                if ( ( AllowDrag )
                &&   ( _SelectedNode != null )
                &&   ( ( Math.Abs( _MouseDownPos.X - Event.MouseX ) >= 5 )
                ||     ( Math.Abs( _MouseDownPos.Y - Event.MouseY ) >= 5 ) ) )
                {
                  OnItemDrag( new ItemDragEventArgs( ToMouseButtons( Event.MouseButtons ), _SelectedNode ) );
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
          _MouseDownPos.X = Event.MouseX;
          _MouseDownPos.Y = Event.MouseY;
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
          if ( _SelectedNode == _MouseOverNode )
          {
            var hitAreaDC2 = HitTest( Event.MouseX, Event.MouseY );
            if ( ( ( hitAreaDC2.Location & TreeViewHitTestLocations.ONITEM ) != 0 )
            &&   ( hitAreaDC2.Node != null ) )
            {
              NodeMouseClick?.Invoke( this, new TreeNodeMouseClickEventArgs( hitAreaDC2.Node, Event.MouseButtons, Event.MouseX, Event.MouseY ) );
            }
          }
          Invalidate();
          break;
        case ControlEvent.EventType.KEY_PRESS:
          if ( (char)Event.Key >= 32 )
          {
            char    pressedKey = (char)Event.Key;

            long curTicks = DateTime.Now.Ticks;
            // more than a second pause, restart entry
            if ( curTicks - _SelectingByKeyPressPreviousTicks > 10000 * 1000 )
            {
              // reset entry mode
              _SelectingByKeyPressPrefix                      = "";
              _SelectingByKeyPressUsingSingleStartLetterMode  = true;
            }
            _SelectingByKeyPressPreviousTicks = curTicks;

            if ( ( _SelectingByKeyPressPrefix.Length == 1 )
            &&   ( _SelectingByKeyPressPrefix[0] == pressedKey ) )
            {
              _SelectingByKeyPressUsingSingleStartLetterMode = true;
            }
            else
            {
              _SelectingByKeyPressPrefix += pressedKey;
              _SelectingByKeyPressUsingSingleStartLetterMode = ( _SelectingByKeyPressPrefix.Length == 1 );
            }
            var node = FindNextNodeStartingWith( SelectedNode, _SelectingByKeyPressPrefix );
            if ( node != null )
            {
              SelectedNode = node;
            }
          }
          break;
        case ControlEvent.EventType.KEY_DOWN:
          if ( Event.Key == System.Windows.Forms.Keys.Down )
          {
            if ( _SelectedNode == null )
            {
              if ( Nodes.Count > 0 )
              {
                SelectedNode = Nodes[0];
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
              SelectedNode = newNode;
              if ( _SelectedNode != null )
              {
                Invalidate( _SelectedNode.Bounds );
              }
            }
          }
          else if ( Event.Key == System.Windows.Forms.Keys.PageUp )
          {
            _ScrollBar.ScrollBy( -( ClientSize.Height + ItemHeight - 1 ) / ItemHeight );
            SelectedNode = _FirstVisibleNode;
          }
          else if ( Event.Key == System.Windows.Forms.Keys.PageDown )
          {
            _ScrollBar.ScrollBy( ( ClientSize.Height + ItemHeight - 1 ) / ItemHeight );

            // get last fully visible item
            var node = _FirstVisibleNode;

            // go down at the end of the screen, but still visible item (-1), also take in account any half displayed item (-1 again)
            int numItemsToWalk = ( ClientSize.Height + ItemHeight - 1 ) / ItemHeight - 2;
            while ( ( numItemsToWalk > 0 )
            &&      ( node != null ) )
            {
              var nextNode = GetNextVisibleNode( node );
              --numItemsToWalk;
              if ( nextNode != null )
              {
                node = nextNode;
              }
              else
              {
                break;
              }
            }
            SelectedNode = node;
          }
          else if ( Event.Key == System.Windows.Forms.Keys.Home )
          {
            _ScrollBar.ScrollTo( 0 );
            if ( Nodes.Count > 0 )
            {
              SelectedNode = Nodes[0];
            }
          }
          else if ( Event.Key == System.Windows.Forms.Keys.End )
          {
            _ScrollBar.ScrollTo( _ScrollBar.Maximum );
            if ( Nodes.Count > 0 )
            {
              SelectedNode = LastVisibleNode;
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
                SelectedNode = newNode;
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
              SelectedNode = newNode;
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
              if ( ( ( _SelectedNode.Nodes.Count == 0 )
              ||     ( !_SelectedNode.IsExpanded ) )
              &&   ( _SelectedNode.Parent != null ) )
              {
                Invalidate( _SelectedNode.Bounds );
                SelectedNode = _SelectedNode.Parent;
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
                SelectedNode = _SelectedNode.Nodes[0];
                Invalidate( _SelectedNode.Bounds );
              }
            }
          }
          break;
      }
      base.OnControlEvent( Event );
    }



    private TreeNode FindNextNodeStartingWith( TreeNode CurrentNode, string Prefix )
    {
      if ( Nodes.Count == 0 )
      {
        return null;
      }
      if ( _SelectingByKeyPressUsingSingleStartLetterMode )
      {
        if ( CurrentNode == null )
        {
          var node = Nodes[0];

          while ( ( node != null )
          &&      ( !node.Text.StartsWith( Prefix, StringComparison.InvariantCultureIgnoreCase ) ) )
          {
            node = GetNextVisibleNode( node );
          }
          return node;
        }
        var startNode1 = CurrentNode;
        bool wrappedOver1 = false;
        CurrentNode = GetNextVisibleNode( CurrentNode );
        while ( ( CurrentNode != null )
        &&      ( !CurrentNode.Text.StartsWith( Prefix, StringComparison.InvariantCultureIgnoreCase ) ) )
        {
          CurrentNode = GetNextVisibleNode( CurrentNode );
          if ( CurrentNode == null )
          {
            CurrentNode = Nodes[0];
            wrappedOver1 = true;
          }
          if ( ( CurrentNode == startNode1 )
          &&   ( wrappedOver1 ) )
          {
            return CurrentNode;
          }
        }
        return CurrentNode;
      }

      if ( CurrentNode == null )
      {
        var node = Nodes[0];

        while ( ( node != null )
        &&      ( !node.Text.StartsWith( Prefix, StringComparison.InvariantCultureIgnoreCase ) ) )
        {
          node = GetNextVisibleNode( node );
        }
        return node;
      }
      var   startNode = CurrentNode;
      bool wrappedOver = false;
      while ( ( CurrentNode != null )
      &&      ( !CurrentNode.Text.StartsWith( Prefix, StringComparison.InvariantCultureIgnoreCase ) ) )
      {
        CurrentNode = GetNextVisibleNode( CurrentNode );
        if ( CurrentNode == null )
        {
          CurrentNode = Nodes[0];
          wrappedOver = true;
        }
        if ( ( CurrentNode == startNode )
        &&   ( wrappedOver ) )
        {
          return CurrentNode;
        }
      }
      return CurrentNode;
    }



    private void OnItemDrag( ItemDragEventArgs e )
    {
      if ( ItemDrag != null )
      {
        ItemDrag( this, e );
      }
    }



    private MouseButtons ToMouseButtons( uint MouseButtons )
    {
      MouseButtons buttons = 0;

      if ( ( MouseButtons & 1 ) != 0 )
      {
        buttons |= System.Windows.Forms.MouseButtons.Left;
      }
      if ( ( MouseButtons & 2 ) != 0 )
      {
        buttons |= System.Windows.Forms.MouseButtons.Right;
      }
      if ( ( MouseButtons & 4 ) != 0 )
      {
        buttons |= System.Windows.Forms.MouseButtons.Middle;
      }

      return buttons;
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
          else if ( GetToggleRectForMouse( treeNode ).Contains( X, Y ) )
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



    public TreeNode GetNodeAt( Point Location )
    {
      return GetNodeAt( Location.X, Location.Y );
    }



    public TreeNode GetNodeAt( int X, int Y )
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

      var node = Node._Next;
      while ( node != null )
      {
        --node._Index;
        node = node.NextNode;
      }

      Node._Next      = null;

      DetachChildNode( Node );
    }



    private void DetachChildNode( TreeNode Node )
    {
      if ( _FirstVisibleNode == Node )
      {
        _FirstVisibleNode = null;
      }

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
      if ( _UpdateLocked )
      {
        _RequiresVisualIndexRecalc = true;
      }
      else
      {
        if ( Nodes.Any() )
        {
          Nodes[0]._VisualIndex = 0;
          Nodes[0].RecalcVisualIndexStartingWithMyself();
        }
      }
        _CachedMaxItemWidth = -1;
      if ( _FirstVisibleNode == null )
      {
        if ( Nodes.Count > 0 )
        {
          _FirstVisibleNode = Nodes[0];
          _FirstVisibleNode.EnsureVisible();
        }
      }
      UpdateScrollbarState();
      Invalidate();
    }



    // lets mouse register on whole node height +2 extra pixels left and right
    internal Rectangle GetToggleRectForMouse( TreeNode Node )
    {
      var rect = Node.Bounds;
      var visualRect = GetToggleRect( Node );

      rect.X = visualRect.X - 2;
      rect.Width = visualRect.Width + 4;

      return rect;
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
        rect = new Rectangle( rect.Left - ItemHeight - rect.Height * 4 / 16, rect.Top, ExpandToggleItemSize, rect.Height );
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
        Button = button;
        Action = TreeViewAction.Unknown;
      }



      public TreeNodeMouseClickEventArgs( TreeNode node, TreeViewAction action, uint button, int x, int y )
      {
        Node = node;
        MouseX = x;
        MouseY = y;
        Button = button;
        Action = action;
      }



      public Point Location
      {
        get
        {
          return new Point( MouseX, MouseY );
        }
      }

      public int MouseX 
      {
        get;
      }

      public int MouseY
      {
        get;
      }

      public uint Button
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



    public class NodeLabelEditEventArgs : EventArgs
    {
      public NodeLabelEditEventArgs( TreeNode Node, string Label )
      {
        this.Node   = Node;
        this.Label  = Label;
      }

      public TreeNode Node
      {
        get;
      }

      public string Label
      {
        get; set;
      }

      public bool CancelEdit { get; set; } 

    }



    public void StartLabelEdit()
    {
      if ( ( _EditedNode != null )
      ||   ( _SelectedNode == null ) )
      {
        return;
      }

      var nodeLabelEditEventArgs = new NodeLabelEditEventArgs( _SelectedNode, _SelectedNode.Text );
      OnBeforeLabelEdit( nodeLabelEditEventArgs );
      if ( nodeLabelEditEventArgs.CancelEdit )
      {
        return;
      }
      _SelectedNode.EnsureVisible();
      _EditedNode = _SelectedNode;
      if ( _PopupEditControl == null )
      {
        _PopupEditControl = new TextBox();
        _PopupEditControl.LostFocus += _PopupEditControl_LostFocus;
        _PopupEditControl.HandleDestroyed += _PopupEditControl_HandleDestroyed;
        _PopupEditControl.PreviewKeyDown += _PopupEditControl_PreviewKeyDown;
        _PopupEditControl.KeyDown += _PopupEditControl_KeyDown;
      }
      var bounds = _SelectedNode.Bounds;

      _PopupEditControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      _PopupEditControl.Text = nodeLabelEditEventArgs.Label;
      _PopupEditControl.Font = nodeLabelEditEventArgs.Node.NodeFont;
      _PopupEditControl.ClientSize = bounds.Size;
      _PopupEditControl.Bounds = bounds;
      if ( _PopupEditControl.Parent != this )
      {
        Controls.Add( _PopupEditControl );
      }

      _PopupEditControl.Visible = true;
      _PopupEditControl.Focus();
      _PopupEditControl.SelectAll();
    }



    private void _PopupEditControl_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
    {
      if ( e.KeyCode == Keys.Escape )
      {
        e.IsInputKey = true;
        EndEdit( true );
      }
      else if ( e.KeyCode == Keys.Enter )
      {
        e.IsInputKey = true;
        EndEdit( false );
      }
    }



    private void _PopupEditControl_HandleDestroyed( object sender, EventArgs e )
    {
      _PopupEditControl = null;
    }



    private void _PopupEditControl_LostFocus( object sender, EventArgs e )
    {
      EndEdit( true );
    }



    public class DrawTreeNodeEventArgs : EventArgs
    {
      private readonly ControlRenderer renderer;

      private readonly TreeNode node;



      public TreeNode Node => node;

      public ControlRenderer Renderer => renderer;



      public DrawTreeNodeEventArgs( ControlRenderer Renderer, TreeNode Node )
      {
        renderer  = Renderer;
        node      = Node;
      }
    }



    protected virtual void OnBeforeLabelEdit( NodeLabelEditEventArgs e )
    {
      if ( BeforeLabelEdit != null ) 
      {
        BeforeLabelEdit( this, e );
      }
    }



    protected virtual void OnAfterLabelEdit( NodeLabelEditEventArgs e )
    {
      if ( AfterLabelEdit != null )
      {
        AfterLabelEdit( this, e );
      }
    }



    private void EndEdit( bool Cancel )
    {
      if ( Cancel )
      {
      }
      else
      {
        var nodeLabelEditEventArgs = new NodeLabelEditEventArgs( _EditedNode, _PopupEditControl.Text );
        OnAfterLabelEdit( nodeLabelEditEventArgs );
        if ( !nodeLabelEditEventArgs.CancelEdit )
        {
          _EditedNode.Text = nodeLabelEditEventArgs.Label;
        }
      }
      _PopupEditControl.Visible = false;

      _EditedNode = null;
    }



    private void _PopupEditControl_KeyDown( object sender, KeyEventArgs e )
    {
      if ( ( e.KeyCode == Keys.Escape )
      ||   ( e.KeyCode == Keys.Enter ) )
      {
        e.SuppressKeyPress = true;
      }
    }



    internal void RenderNodeImage( ControlRenderer Renderer, TreeNode Node )
    {
      if ( DrawNodeImage != null )
      {
        DrawNodeImage( this, new TreeView.DrawTreeNodeEventArgs( Renderer, Node ) );
      }
      else
      {
        Renderer.DrawTreeViewNodeImage( Node );
      }
    }



    internal void RenderNode( ControlRenderer Renderer, TreeNode Node )
    {
      if ( DrawNode != null )
      {
        DrawNode( this, new TreeView.DrawTreeNodeEventArgs( Renderer, Node ) );
      }
      else
      {
        Renderer.DrawTreeViewNode( Node );
      }
    }



    public void DumpNodes()
    {
      if ( Nodes.Count > 0 )
      {
        var node = Nodes[0];
        while ( node != null )
        {
          //Debug.Log( $"{node.Level} - {node.Text} - VI {node.VisualIndex} - PV {node.PreviousNode?.Text} - NX {node.NextNode?.Text} - PN {node._Parent?.Text} - OWN {node._Owner} - Nodes { node.Nodes._OwnerControl}" );
          Debug.Log( $"{node.Level} - {node.Text} - OWN {node._Owner} - Nodes {node.Nodes._OwnerControl}" );

          node = GetNextNode( node );
        }
      }
    }


  }






}