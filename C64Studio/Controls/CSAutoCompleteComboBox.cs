using RetroDevStudio.CustomRenderer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;



namespace RetroDevStudio.Controls
{
  public class CSAutoCompleteComboBox : UserControl
  {
    private ItemCollection    _Items;
    private TextBox           _EditItems;
    private CSButton          _BtnDropDown;
    private PopupControl      _PopupList;
    private ListBox           _AutoCompleteListBox = new ListBox();

    private int               _ListBoxMouseOverItemIndex = -1;



    public ItemCollection Items
    {
      get
      {
        return _Items;
      }
      set
      {
        _Items = value;
        UpdateItemList();
      }
    }



    public bool AutoFilterListItems
    {
      get;
      set;
    }



    public CSAutoCompleteComboBox()
    {
      AutoFilterListItems           = false;
      _Items = new ItemCollection( this );
      _AutoCompleteListBox.Visible = false;
      _AutoCompleteListBox.MouseClick += _AutoCompleteListBox_MouseClick;
      _AutoCompleteListBox.MouseMove += _AutoCompleteListBox_MouseMove;
      _AutoCompleteListBox.DrawItem += _AutoCompleteListBox_DrawItem;
      _AutoCompleteListBox.DrawMode = DrawMode.OwnerDrawFixed;
      InitializeComponent();
    }



    private void _AutoCompleteListBox_DrawItem( object sender, DrawItemEventArgs e )
    {
      if ( e.Index == _ListBoxMouseOverItemIndex )
      {
        e.DrawBackground();
      }
      else
      {
        e.DrawBackground();
      }

      if ( e.Index != -1 )
      {
        e.Graphics.DrawString( _AutoCompleteListBox.Items[e.Index].ToString(), e.Font, new SolidBrush( e.ForeColor ),
          e.Bounds, 
          StringFormat.GenericDefault );
      }

      // If the ListBox has focus, draw a focus rectangle around the selected item.
      e.DrawFocusRectangle();
    }



    private int DetermineListBoxItemUnderMouse()
    {
      Point screenPosition = Control.MousePosition;
      Point listBoxClientAreaPosition = _AutoCompleteListBox.PointToClient( screenPosition );

      return _AutoCompleteListBox.IndexFromPoint( listBoxClientAreaPosition );
    }



    private void _AutoCompleteListBox_MouseMove( object sender, MouseEventArgs e )
    {
      int   newItem = DetermineListBoxItemUnderMouse();
      if ( newItem != _ListBoxMouseOverItemIndex )
      {
        if ( _ListBoxMouseOverItemIndex != -1 )
        {
          _AutoCompleteListBox.Invalidate( _AutoCompleteListBox.GetItemRectangle( _ListBoxMouseOverItemIndex ) );
        }

        _ListBoxMouseOverItemIndex = newItem;
        if ( _ListBoxMouseOverItemIndex != -1 )
        {
          _AutoCompleteListBox.Invalidate( _AutoCompleteListBox.GetItemRectangle( _ListBoxMouseOverItemIndex ) );
        }
      }
    }



    private void _AutoCompleteListBox_MouseClick( object sender, MouseEventArgs e )
    {
      for ( int i = 0; i < _AutoCompleteListBox.Items.Count; i++ )
      {
        if ( _AutoCompleteListBox.GetItemRectangle( i ).Contains( e.Location ) )
        {
          CloseDropDown();
          _EditItems.Text = (string)_AutoCompleteListBox.Items[i];
          return;
        }
      }
    }



    private void CloseDropDown()
    {
      if ( !_AutoCompleteListBox.Visible )
      {
        return;
      }
      _PopupList.Close();
    }



    public override string Text
    {
      get
      {
        return _EditItems.Text;
      }
      set
      {
        _EditItems.Text = value;
      }
    }



    protected override void WndProc( ref Message m )
    {
      /*
      //Debug.Log( "msg " + m.Msg.ToString( "x" ) );
      if ( m.Msg == 0x8 )
      {
        // WM_KILLFOCUS
        string    text = Text;
        Debug.Log( "store " + text );

        base.WndProc( ref m );

        Text = text;
        Debug.Log( "restored " + text );
        return;
      }*/
      base.WndProc( ref m );
    }



    protected void OnSelectedItemChanged( EventArgs e )
    {
      Debug.Log( "selected item" );


    }
   
    
    
    protected void OnDropDownClosed( EventArgs e )
    {
      string    text = Text;
      Debug.Log( "store " + text );

    }



    protected override void OnTextChanged( EventArgs e )
    {
      if ( Text == string.Empty )
      {
        _AutoCompleteListBox.Items.Clear();
        _AutoCompleteListBox.Items.AddRange( Items.ToArray() );
      }
    }



    protected void OnTextUpdate( EventArgs e )
    {
      IEnumerable<string>     items = Items;

      if ( AutoFilterListItems )
      {
        items = Items.Where( x => x.ToString().ToLower().Contains( _EditItems.Text.ToLower() ) );
      }

      string  origText = Text;
      _AutoCompleteListBox.Items.Clear();
      if ( Text != string.Empty )
      {
        _AutoCompleteListBox.Items.AddRange( items.ToArray() );
      }
      else
      {
        _AutoCompleteListBox.Items.AddRange( Items.ToArray() );
      }
      _EditItems.SelectionStart = origText.Length;
      _EditItems.SelectionLength = Text.Length - origText.Length;
      ResizeListBoxToContent();
    }



    private void ResizeListBoxToContent()
    {
      var g = _AutoCompleteListBox.CreateGraphics();

      int   numItems = Math.Min( 20, _AutoCompleteListBox.Items.Count );

      int     maxWidth = 0;
      int     maxHeight = numItems * _AutoCompleteListBox.Font.Height;

      var currentScreen = Screen.FromHandle( _AutoCompleteListBox.Handle );
      if ( maxHeight > currentScreen.Bounds.Height )
      {
        maxHeight = currentScreen.Bounds.Height;
      }

      foreach ( string item in _AutoCompleteListBox.Items )
      {
        int   itemWidth = (int)g.MeasureString( item, _AutoCompleteListBox.Font ).Width;

        if ( itemWidth > maxWidth )
        {
          maxWidth = itemWidth;
        }
      }
      if ( maxWidth < Width )
      {
        maxWidth = Width;
      }
      _AutoCompleteListBox.ClientSize = new Size( maxWidth, maxHeight );
      if ( _PopupList != null )
      {
        _PopupList.Size = new Size( maxWidth, maxHeight );
      }
    }



    [DllImport( "user32.dll" )]
    static extern bool SetWindowPos( IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags );

    static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
    const UInt32 SWP_NOSIZE = 0x0001;
    const UInt32 SWP_NOMOVE = 0x0002;
    const UInt32 SWP_SHOWWINDOW = 0x0040;


    private void SetTopMost( Control Control )
    {
      // Call this way:
      SetWindowPos( Control.Handle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW );
    }



    public void OpenDropDown()
    {
      if ( ( _PopupList != null )
      &&   ( _PopupList.Visible ) )
      {
        return;
      }
      if ( Items.Count == 0 )
      {
        return;
      }

      if ( _AutoCompleteListBox == null )
      {
        _AutoCompleteListBox = new ListBox();
        _AutoCompleteListBox.Visible = false;
        UpdateItemList();
      }

      ResizeListBoxToContent();

      _PopupList = new PopupControl( _AutoCompleteListBox );
      _PopupList.Closed += _PopupList_Closed;
      _PopupList.Show( this );
      _AutoCompleteListBox.Visible = true;
      _PopupList.Visible = true;
      SetTopMost( _PopupList );
    }



    private void _PopupList_Closed( object sender, ToolStripDropDownClosedEventArgs e )
    {
      /*
      if ( _AutoCompleteListBox == null )
      {
        return;
      }
      _AutoCompleteListBox.Dispose();
      _AutoCompleteListBox = null;*/
    }



    private void InitializeComponent()
    {
      this._EditItems = new System.Windows.Forms.TextBox();
      this._BtnDropDown = new RetroDevStudio.Controls.CSButton();
      this.SuspendLayout();
      // 
      // _EditItems
      // 
      this._EditItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this._EditItems.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this._EditItems.Location = new System.Drawing.Point(3, 3);
      this._EditItems.Multiline = true;
      this._EditItems.Name = "_EditItems";
      this._EditItems.Size = new System.Drawing.Size(138, 19);
      this._EditItems.TabIndex = 0;
      this._EditItems.TextChanged += new System.EventHandler(this.editItems_TextChanged);
      // 
      // _BtnDropDown
      // 
      this._BtnDropDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this._BtnDropDown.Image = null;
      this._BtnDropDown.Location = new System.Drawing.Point(141, 0);
      this._BtnDropDown.Name = "_BtnDropDown";
      this._BtnDropDown.Size = new System.Drawing.Size(20, 25);
      this._BtnDropDown.TabIndex = 1;
      this._BtnDropDown.Text = "▼";
      this._BtnDropDown.UseVisualStyleBackColor = true;
      this._BtnDropDown.Click += new System.EventHandler(this.btnDropDown_Click);
      // 
      // CSAutoCompleteComboBox
      // 
      this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.Controls.Add(this._BtnDropDown);
      this.Controls.Add(this._EditItems);
      this.Name = "CSAutoCompleteComboBox";
      this.Size = new System.Drawing.Size(161, 25);
      this.ResumeLayout(false);
      this.PerformLayout();

    }



    private void btnDropDown_Click( object sender, EventArgs e )
    {
      OpenDropDown();
    }



    private void editItems_TextChanged( object sender, EventArgs e )
    {
      UpdateItemList();
    }



    private void UpdateItemList()
    {
      IEnumerable<string>     items = Items;

      if ( AutoFilterListItems )
      {
        items = Items.Where( x => x.ToString().ToLower().Contains( _EditItems.Text.ToLower() ) );
      }

      string  origText = _EditItems.Text;
      _AutoCompleteListBox.Items.Clear();
      if ( _EditItems.Text != string.Empty )
      {
        _AutoCompleteListBox.Items.AddRange( items.ToArray() );
      }
      else
      {
        _AutoCompleteListBox.Items.AddRange( Items.ToArray() );
      }
      //_AutoCompleteListBox.Visible = _AutoCompleteListBox.Items.Count > 0;
      _EditItems.SelectionStart = origText.Length;
      _EditItems.SelectionLength = _EditItems.Text.Length - origText.Length;
    }



    public class ItemCollection : IEnumerable<string>
    {
      private List<string>              _Items = new List<string>();
      private CSAutoCompleteComboBox    _Owner;



      public int Count
      {
        get
        {
          return _Items.Count;
        }
      }



      public ItemCollection( CSAutoCompleteComboBox Owner )
      {
        _Owner = Owner;
      }



      public void Add( string Item )
      {
        _Items.Add( Item );
        _Items.Distinct().ToList();
        _Owner.UpdateItemList();
      }



      public IEnumerator<string> GetEnumerator()
      {
        return _Items.GetEnumerator();
      }



      IEnumerator IEnumerable.GetEnumerator()
      {
        return _Items.GetEnumerator();
      }



      public void RemoveAt( int Index )
      {
        _Items.RemoveAt( Index );
        _Owner.UpdateItemList();
      }



      public void Insert( int Index, string Text )
      {
        _Items.Insert( Index, Text );
        _Items = _Items.Distinct().ToList();
        _Owner.UpdateItemList();
      }



      public void AddRange( IEnumerable<string> List )
      {
        _Items.AddRange( List );
        _Items = _Items.Distinct().ToList();
        _Owner.UpdateItemList();
      }
    }

  }
}
