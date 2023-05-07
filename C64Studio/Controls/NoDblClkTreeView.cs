using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;



namespace RetroDevStudio
{
  public partial class NoDblClkTreeView : TreeView
  {
    public NoDblClkTreeView()
    {
      SetStyle( ControlStyles.EnableNotifyMessage, true );
      InitializeComponent();
    }



    protected override void DefWndProc( ref System.Windows.Forms.Message m )
    {
      if ( m.Msg == 515 )
      {
        // swallow double click
        /* WM_LBUTTONDBLCLK */
        return;
      }
      base.DefWndProc( ref m );
    }



    private const int WM_TIMER = 0x0113;
    private bool TriggerLabelEdit = false;
    private string viewedLabel;
    private string editedLabel;



    protected override void OnBeforeLabelEdit( NodeLabelEditEventArgs e )
    {
      // put node label to initial state
      // to ensure that in case of label editing cancelled
      // the initial state of label is preserved
      this.SelectedNode.Text = viewedLabel;
      // base.OnBeforeLabelEdit is not called here
      // it is called only from StartLabelEdit
    }



    protected override void OnAfterLabelEdit( NodeLabelEditEventArgs e )
    {
      bool  origLabelEdit = LabelEdit;
      LabelEdit = false;
      e.CancelEdit = true;
      if ( e.Label == null )
      {
        LabelEdit = origLabelEdit;
        return;
      }
      /*
      ValidateLabelEditEventArgs ea = new ValidateLabelEditEventArgs(e.Label);
      OnValidateLabelEdit( ea );*/
      /*
      if ( ea.Cancel == true )
      {
        e.Node.Text = editedLabel;
        this.LabelEdit = true;
        e.Node.BeginEdit();
      }
      else*/
      {
        base.OnAfterLabelEdit( e );
        LabelEdit = origLabelEdit;
      }
    }



    public void BeginEdit()
    {
      StartLabelEdit();
    }



    protected override void OnNotifyMessage( Message m )
    {
      if ( TriggerLabelEdit )
      {
        if ( m.Msg == WM_TIMER )
        {
          TriggerLabelEdit = false;
          StartLabelEdit();
        }
      }
      base.OnNotifyMessage( m );
    }



    public void StartLabelEdit()
    {
      TreeNode tn = this.SelectedNode;
      viewedLabel = tn.Text;
      var e = new NodeLabelEditEventArgs( tn );
      base.OnBeforeLabelEdit( e );
      editedLabel = tn.Text;
      this.LabelEdit = true;
      tn.BeginEdit();
    }



    protected override void OnMouseDown( MouseEventArgs e )
    {
      if ( e.Button == MouseButtons.Right )
      {
        TreeNode tn = this.GetNodeAt(e.X, e.Y);
        if ( tn != null )
        {
          this.SelectedNode = tn;
        }
      }
      base.OnMouseDown( e );
    }



    protected override void OnMouseUp( MouseEventArgs e )
    {
      TreeNode tn;
      if ( e.Button == MouseButtons.Left )
      {
        tn = this.SelectedNode;
        if ( tn == this.GetNodeAt( e.X, e.Y ) )
        {
          if ( wasDoubleClick )
          {
            wasDoubleClick = false;
          }
          else if ( LabelEdit )
          {
            TriggerLabelEdit = true;
          }
        }
      }
      base.OnMouseUp( e );
    }


    protected override void OnClick( EventArgs e )
    {
      TriggerLabelEdit = false;
      base.OnClick( e );
    }



    private bool wasDoubleClick = false;

    protected override void OnDoubleClick( EventArgs e )
    {
      wasDoubleClick = true;
      base.OnDoubleClick( e );
    }



    // Returns the bounds of the specified node, including the region 
    // occupied by the node label and any node tag displayed.
    private Rectangle NodeBounds( TreeNode Node )
    {
      // Set the return value to the normal node bounds.
      Rectangle bounds = Node.Bounds;
      /*
      if ( node.Tag != null )
      {
        // Retrieve a Graphics object from the TreeView handle
        // and use it to calculate the display width of the tag.
        
      }*/

      Font nodeFont = Node.NodeFont;
      if ( nodeFont == null )
      {
        nodeFont = Font;
      }
      Graphics g = CreateGraphics();
      int tagWidth = (int)g.MeasureString( Node.Text, nodeFont ).Width + 4;

      // Adjust the node bounds using the calculated value.
      bounds.Width = tagWidth;
      //bounds.Offset( tagWidth / 2, 0 );
      //bounds = Rectangle.Inflate( bounds, tagWidth / 2, 0 );
      g.Dispose();

      return bounds;
    }



    /*
    private void NoDblClkTreeView_DrawNode( object sender, DrawTreeNodeEventArgs e )
    {
      Font nodeFont = e.Node.NodeFont;
      if ( nodeFont == null )
      {
        nodeFont = ( (TreeView)sender ).Font;
      }
      e.DrawDefault = false;
      var bounds = NodeBounds( e.Node );
      var textBounds = Rectangle.Inflate( bounds, 3, 0 );
      var bgBounds = new Rectangle( bounds.Location, bounds.Size );
      bgBounds.Offset( -3, 0 );*/

      /*
      UInt32      color = Core.Settings.BGColor( ColorableElement.BACKGROUND_CONTROL );
      if ( ( e.Item.Pressed )
      ||   ( e.Item.Selected ) )
      {
        e.Graphics.FillRectangle( new SolidBrush( GR.Color.Helper.FromARGB( color ) ), 0, 0, e.Item.Width, e.Item.Height );

        color = Core.Settings.FGColor( ColorableElement.SELECTED_TEXT );

        // make transparent
        if ( ( color & 0xff000000 ) == 0xff000000 )
        {
          color = ( color & 0x00ffffff ) | 0x40000000;
        }
      }
      e.Graphics.FillRectangle( new SolidBrush( GR.Color.Helper.FromARGB( color ) ), 0, 0, e.Item.Width, e.Item.Height );
      */

    /*
      // Draw the background and node text for a selected node.
      if ( ( e.State & TreeNodeStates.Selected ) != 0 )
      {
        // Draw the background of the selected node. The NodeBounds
        // method makes the highlight rectangle large enough to
        // include the text of a node tag, if one is present.
        e.Graphics.FillRectangle( Brushes.Green, bgBounds );

        // Retrieve the node font. If the node font has not been set, use the TreeView font.
        e.Graphics.DrawString( e.Node.Text, nodeFont, new SolidBrush( BackColor ), textBounds );
      }
      else
      {
        if ( ( e.State & TreeNodeStates.Focused ) != 0 )
        {
          e.Graphics.FillRectangle( Brushes.LightBlue, bgBounds );
        }
        e.Graphics.DrawString( e.Node.Text, nodeFont, new SolidBrush( ForeColor ), textBounds );
      }

      // If the node has focus, draw the focus rectangle large, making
      // it large enough to include the text of the node tag, if present.
      if ( ( e.State & TreeNodeStates.Focused ) != 0 )
      {
        using ( Pen focusPen = new Pen( Color.Black ) )
        {
          focusPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
          bounds.Size = new Size( bgBounds.Width - 1, bgBounds.Height - 1 );
          bounds.Offset( -3, 0 );
          e.Graphics.DrawRectangle( focusPen, bounds );
        }
      }
    }*/

  }
}
