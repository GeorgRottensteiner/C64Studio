using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Be.Windows.Forms;

namespace C64Studio
{
  public partial class DebugMemory : BaseDocument
  {
    private class MemoryView
    {
      public GR.Memory.ByteBuffer      RAM = new GR.Memory.ByteBuffer( 65536 );
      public bool[]                    RAMChanged = new bool[65536];
      public Dictionary<int,int>       ValidMemory = new Dictionary<int, int>();
    };

    private MemoryView                m_MemoryCPU     = new MemoryView();
    private MemoryView                m_MemoryRAM     = new MemoryView();
    private MemoryView                m_ActiveMemory;

    public Project                    DebuggedProject = null;

    private int                       m_Offset = 0;

    private ToolStripMenuItem         m_MenuItemHexStringView = null;
    private ToolStripMenuItem         m_MenuItemHexCharView = null;
    private ToolStripMenuItem         m_MenuItemHexSpriteView = null;



    public class DebugMemoryEvent
    {
      public int      Offset = 0;
      public int      Length = 0;


      public DebugMemoryEvent( int Offset, int Length )
      {
        this.Offset = Offset;
        this.Length = Length;
      }
    }

    public delegate void DebugMemoryEventCallback( object Sender, DebugMemoryEvent Event );

    public event DebugMemoryEventCallback ViewScrolled;



    public DebugMemory( StudioCore Core )
    {
      InitializeComponent();

      this.Core = Core;

      m_ActiveMemory = m_MemoryCPU;

      SetHexData( m_ActiveMemory.RAM );
     
      hexView.SelectedByteProvider = new DynamicByteSelectionProvider( 65536 );
      hexView.ViewScrolled += new EventHandler( hexView_ViewScrolled );

      hexView.ContextMenuStrip.Items.Add( "-" );

      m_MenuItemHexStringView = new ToolStripMenuItem( "Set to String View" );
      m_MenuItemHexStringView.Click += btnBinaryStringView_Click;
      m_MenuItemHexStringView.Checked = true;
      hexView.ContextMenuStrip.Items.Add( m_MenuItemHexStringView );

      m_MenuItemHexCharView = new ToolStripMenuItem( "Set to Character View" );
      m_MenuItemHexCharView.Click += btnBinaryCharView_Click;
      hexView.ContextMenuStrip.Items.Add( m_MenuItemHexCharView );

      m_MenuItemHexSpriteView = new ToolStripMenuItem( "Set to Sprite View" );
      m_MenuItemHexSpriteView.Click += btnBinarySpriteView_Click;
      hexView.ContextMenuStrip.Items.Add( m_MenuItemHexSpriteView );

      switch ( Core.Settings.MemoryDisplay )
      {
        case MemoryDisplayType.CHARSET:
          btnBinaryCharView_Click( null, null );
          hexView.ContextMenuStrip.Items.Add( "-" );
          break;
        case MemoryDisplayType.SPRITES:
          btnBinarySpriteView_Click( null, null );
          hexView.ContextMenuStrip.Items.Add( "-" );

          TODO  
          m_MenuItemHexSpriteView = new ToolStripMenuItem( "Set Colors" );
          m_MenuItemHexSpriteView.Click += btnBinarySpriteView_Click;
          hexView.ContextMenuStrip.Items.Add( m_MenuItemHexSpriteView );
          break;
      }
    }



    void hexView_ViewScrolled( object sender, EventArgs e )
    {
      int     newValue = ( (int)hexView.VScrollPos );

      if ( m_Offset != newValue )
      {
        m_Offset = newValue;

        ViewScrolled( this, new DebugMemoryEvent( m_Offset * hexView.BytesPerLine, hexView.BytesPerLine * hexView.VerticalByteCount ) );
      }
    }




    public void SetHexData( GR.Memory.ByteBuffer Data )
    {
      long     oldOffset = hexView.VScrollPos;

      hexView.ByteProvider = new Be.Windows.Forms.DynamicByteProvider( Data.Data() );
      hexView.PerformScrollToLine( oldOffset );
    }



    public int MemoryStart
    {
      get
      {
        return m_Offset * hexView.BytesPerLine;
      }
    }



    public int MemorySize
    {
      get
      {
        return ( hexView.BytesPerLine * hexView.VerticalByteCount );
      }
    }



    public bool MemoryAsCPU
    {
      get
      {
        return Core.Settings.MemorySource == MemorySourceType.CPU;
      }
    }



    public void InvalidateAllMemory()
    {
      m_ActiveMemory.ValidMemory.Clear();
    }



    private void ValidateMemory( int Offset, int Length )
    {
      foreach ( int offset in m_ActiveMemory.ValidMemory.Keys )
      {
        int     storedlength = m_ActiveMemory.ValidMemory[offset];

        if ( ( Offset >= offset )
        &&   ( Offset < offset + storedlength ) )
        {
          // we are at least partially in here
          if ( Offset + Length < offset + storedlength )
          {
            // already fully validated
            return;
          }
          Length -= Offset - offset;
          Offset = offset + storedlength;
          if ( Length == 0 )
          {
            return;
          }
        }
      }
      if ( Length > 0 )
      {
        m_ActiveMemory.ValidMemory.Add( Offset, Length );
      }
      // TODO normalize entries
      /*
      var enumerator = m_ValidMemory.GetEnumerator();

      foreach ( int offset in m_ValidMemory.Keys )
      {
        int storedlength = m_ValidMemory[offset];

        if ( ( Offset >= offset )
        && ( Offset < offset + storedlength ) )
        {
          // we are at least partially in here
          if ( Offset + Length < offset + storedlength )
          {
            // already fully validated
            return;
          }
          Length -= Offset - offset;
          Offset = offset + storedlength;
          if ( Length == 0 )
          {
            return;
          }
        }
      }*/
    }



    private bool IsMemoryValid( int Offset )
    {
      foreach ( int offset in m_ActiveMemory.ValidMemory.Keys )
      {
        int     storedlength = m_ActiveMemory.ValidMemory[offset];

        if ( ( Offset >= offset )
        &&   ( Offset < offset + storedlength ) )
        {
          return true;
        }
      }
      return false;
    }



    public void UpdateMemory( RemoteDebugger.RequestData Request, GR.Memory.ByteBuffer Data )
    {
      int Offset = Request.Parameter1;

      for ( int i = 0; i < Data.Length; ++i )
      {
        byte    ramByte = Data.ByteAt( i );

        if ( Request.Reason != RemoteDebugger.RequestReason.MEMORY_FETCH )
        {
          if ( ramByte != m_ActiveMemory.RAM.ByteAt( Offset + i ) )
          {
            m_ActiveMemory.RAMChanged[Offset + i] = true;

            hexView.SelectedByteProvider.SetByteSelectionState( Offset + i, true );
          }
          else
          {
            m_ActiveMemory.RAMChanged[Offset + i] = false;

            hexView.SelectedByteProvider.SetByteSelectionState( Offset + i, false );
          }
        }
        m_ActiveMemory.RAM.SetU8At( Offset + i, ramByte );
        hexView.ByteProvider.WriteByte( Offset + i, ramByte );
      }
      ValidateMemory( Offset, (int)Data.Length );

      hexView.Invalidate();
    }



    private void hexView_DoubleClick( object sender, EventArgs e )
    {
      // copy full dump to clipboard
      StringBuilder sb = new StringBuilder();

      for ( int i = 0; i < 65536 / 16; ++i )
      {
        sb.Append( ( i * 16 ).ToString( "x4" ) );
        sb.Append( ": " );
        sb.Append( m_ActiveMemory.RAM.ByteAt( i * 16 + 0 ).ToString( "x2" ) );
        sb.Append( " " );
        sb.Append( m_ActiveMemory.RAM.ByteAt( i * 16 + 1 ).ToString( "x2" ) );
        sb.Append( " " );
        sb.Append( m_ActiveMemory.RAM.ByteAt( i * 16 + 2 ).ToString( "x2" ) );
        sb.Append( " " );
        sb.Append( m_ActiveMemory.RAM.ByteAt( i * 16 + 3 ).ToString( "x2" ) );
        sb.Append( "  " );
        sb.Append( m_ActiveMemory.RAM.ByteAt( i * 16 + 4 ).ToString( "x2" ) );
        sb.Append( " " );
        sb.Append( m_ActiveMemory.RAM.ByteAt( i * 16 + 5 ).ToString( "x2" ) );
        sb.Append( " " );
        sb.Append( m_ActiveMemory.RAM.ByteAt( i * 16 + 6 ).ToString( "x2" ) );
        sb.Append( " " );
        sb.Append( m_ActiveMemory.RAM.ByteAt( i * 16 + 7 ).ToString( "x2" ) );
        sb.Append( "  " );
        sb.Append( m_ActiveMemory.RAM.ByteAt( i * 16 + 8 ).ToString( "x2" ) );
        sb.Append( " " );
        sb.Append( m_ActiveMemory.RAM.ByteAt( i * 16 + 9 ).ToString( "x2" ) );
        sb.Append( " " );
        sb.Append( m_ActiveMemory.RAM.ByteAt( i * 16 + 10 ).ToString( "x2" ) );
        sb.Append( " " );
        sb.Append( m_ActiveMemory.RAM.ByteAt( i * 16 + 11 ).ToString( "x2" ) );
        sb.Append( "  " );
        sb.Append( m_ActiveMemory.RAM.ByteAt( i * 16 + 12 ).ToString( "x2" ) );
        sb.Append( " " );
        sb.Append( m_ActiveMemory.RAM.ByteAt( i * 16 + 13 ).ToString( "x2" ) );
        sb.Append( " " );
        sb.Append( m_ActiveMemory.RAM.ByteAt( i * 16 + 14 ).ToString( "x2" ) );
        sb.Append( " " );
        sb.Append( m_ActiveMemory.RAM.ByteAt( i * 16 + 15 ).ToString( "x2" ) );
        sb.Append( "\r\n" );
      }
      Debug.Log( sb.ToString() );
    }



    private void btnBinaryStringView_Click( object sender, EventArgs e )
    {
      hexView.CustomHexViewer = null;

      Core.Settings.MemoryDisplay = MemoryDisplayType.ASCII;

      //btnBinaryStringView.Enabled = false;
      m_MenuItemHexStringView.Checked = true;

      btnBinarySpriteView.Checked = false;
      //btnBinarySpriteView.Enabled = true;
      m_MenuItemHexSpriteView.Checked = false;
      btnBinaryCharView.Checked = false;
      //btnBinaryCharView.Enabled = true;
      m_MenuItemHexCharView.Checked = false;
      hexView.Invalidate();
    }



    private void btnBinaryCharView_Click( object sender, EventArgs e )
    {
      if ( hexView.CustomHexViewer is HexBoxCharViewer )
      {
        ( (HexBoxCharViewer)hexView.CustomHexViewer ).ToggleViewMode();
      }
      else
      {
        hexView.CustomHexViewer = new HexBoxCharViewer();
      }

      Core.Settings.MemoryDisplay = MemoryDisplayType.CHARSET;

      //btnBinaryCharView.Enabled = false;
      m_MenuItemHexCharView.Checked = true;

      btnBinaryStringView.Checked = false;
      //btnBinaryStringView.Enabled = true;
      m_MenuItemHexStringView.Checked = false;
      btnBinarySpriteView.Checked = false;
      //btnBinarySpriteView.Enabled = true;
      m_MenuItemHexSpriteView.Checked = false;
      hexView.Invalidate();
    }



    private void btnBinarySpriteView_Click( object sender, EventArgs e )
    {
      if ( hexView.CustomHexViewer is HexBoxSpriteViewer )
      {
        ( (HexBoxSpriteViewer)hexView.CustomHexViewer ).ToggleViewMode();
      }
      else
      {
        hexView.CustomHexViewer = new HexBoxSpriteViewer();
      }

      Core.Settings.MemoryDisplay = MemoryDisplayType.SPRITES;

      //btnBinarySpriteView.Enabled = false;
      m_MenuItemHexSpriteView.Checked = true;

      btnBinaryStringView.Checked = false;
      //btnBinaryStringView.Enabled = true;
      m_MenuItemHexStringView.Checked = false;
      btnBinaryCharView.Checked = false;
      //btnBinaryCharView.Enabled = true;
      m_MenuItemHexCharView.Checked = false;
      hexView.Invalidate();
    }



    private void toolStripBtnMemoryFromCPU_Click( object sender, EventArgs e )
    {
      if ( toolStripBtnMemoryFromCPU.Checked )
      {
        Core.Settings.MemorySource = MemorySourceType.CPU;
      }
      else
      {
        Core.Settings.MemorySource = MemorySourceType.RAM;
      }

      if ( MemoryAsCPU )
      {
        toolStripBtnMemoryFromCPU.Image = C64Studio.Properties.Resources.icon_memory_cpu.ToBitmap();
        toolStripBtnMemoryFromCPU.ToolTipText = "Show RAM as CPU sees it";
        m_ActiveMemory = m_MemoryCPU;
      }
      else
      {
        toolStripBtnMemoryFromCPU.Image = C64Studio.Properties.Resources.icon_memory_ram.ToBitmap();
        toolStripBtnMemoryFromCPU.ToolTipText = "Show RAM";
        m_ActiveMemory = m_MemoryRAM;
      }
      SetHexData( m_ActiveMemory.RAM );
      ViewScrolled( this, new DebugMemoryEvent( m_Offset * hexView.BytesPerLine, hexView.BytesPerLine * hexView.VerticalByteCount ) );
    }

  }
}
