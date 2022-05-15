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
        if ( m.Msg == WM_TIMER )
        {
          TriggerLabelEdit = false;
          StartLabelEdit();
        }
      base.OnNotifyMessage( m );
    }



    public void StartLabelEdit()
    {
      TreeNode tn = this.SelectedNode;
      viewedLabel = tn.Text;
      NodeLabelEditEventArgs e =
                new NodeLabelEditEventArgs(tn);
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
            wasDoubleClick = false;
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



  }
}
