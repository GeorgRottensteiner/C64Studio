using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Be.Windows.Forms;
using System.Drawing;
using C64Studio.Types;
using GR.Memory;
using C64Studio.Debugger;

namespace C64Studio
{
  public partial class DebugMemory : BaseDocument
  {
    private enum ColorType
    {
      CHARS_BACKGROUND,
      CHARS_CUSTOM,
      CHARS_MULTICOLOR1,
      CHARS_MULTICOLOR2,
      CHARS_MULTICOLOR3,
      CHARS_MULTICOLOR4,
      SPRITES_BACKGROUND,
      SPRITES_CUSTOM,
      SPRITES_MULTICOLOR1,
      SPRITES_MULTICOLOR2,
      SPRITES_MULTICOLOR3,
      SPRITES_MULTICOLOR4
    };


    public Project                    DebuggedProject = null;

    private int                       m_Offset = 0;

    private ToolStripMenuItem         m_MenuItemHexStringView = null;
    private ToolStripMenuItem         m_MenuItemHexCharView = null;
    private ToolStripMenuItem         m_MenuItemHexSpriteView = null;
    private ToolStripMenuItem         m_MenuItemPreSetColorsSeparator = null;
    private ToolStripMenuItem         m_MenuItemSetColors = null;

    private Color                     ChangedColor = Color.Red;
    private Color                     UnchangedColor = Color.Black;



    public class DebugMemoryEvent
    {
      public DebugMemory      Viewer = null;
      public int              Offset = 0;
      public int              Length = 0;


      public DebugMemoryEvent( DebugMemory Viewer, int Offset, int Length )
      {
        this.Viewer = Viewer;
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

      Core.Debugging.MemoryViews.Add( this );
      SetHexData( Core.Debugging.ActiveMemory.RAM );
     
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

      SetMemoryDisplayType();

      ViewScrolled += new DebugMemory.DebugMemoryEventCallback( Core.MainForm.m_DebugMemory_ViewScrolled );
    }



    public void SetMemoryDisplayType()
    {
      switch ( Core.Settings.MemoryDisplay )
      {
        case MemoryDisplayType.CHARSET:
          btnBinaryCharView_Click( null, null );
          break;
        case MemoryDisplayType.SPRITES:
          btnBinarySpriteView_Click( null, null );
          break;
        default:
          btnBinaryStringView_Click( null, null );
          break;
      }
    }



    private void SetColorSubmenu()
    {
      if ( m_MenuItemPreSetColorsSeparator != null )
      {
        hexView.ContextMenuStrip.Items.Remove( m_MenuItemPreSetColorsSeparator );
        m_MenuItemPreSetColorsSeparator = null;
      }
      if ( m_MenuItemSetColors != null )
      {
        m_MenuItemSetColors.DropDownItems.Clear();
        hexView.ContextMenuStrip.Items.Remove( m_MenuItemSetColors );
        m_MenuItemSetColors = null;
      }

      m_MenuItemSetColors = new ToolStripMenuItem( "Set Colors" );
      hexView.ContextMenuStrip.Items.Add( m_MenuItemSetColors );

      switch ( Core.Settings.MemoryDisplay )
      {
        case MemoryDisplayType.CHARSET:
          AddSubMenu( m_MenuItemSetColors, "Background Color", ColorType.CHARS_BACKGROUND, Core.Settings.MemoryDisplayCharsetBackgroundColor );
          AddSubMenu( m_MenuItemSetColors, "Custom Color", ColorType.CHARS_CUSTOM, Core.Settings.MemoryDisplayCharsetCustomColor );
          AddSubMenu( m_MenuItemSetColors, "Multicolor 1", ColorType.CHARS_MULTICOLOR1, Core.Settings.MemoryDisplayCharsetMulticolor1 );
          AddSubMenu( m_MenuItemSetColors, "Multicolor 2", ColorType.CHARS_MULTICOLOR2, Core.Settings.MemoryDisplayCharsetMulticolor2 );
          AddSubMenu( m_MenuItemSetColors, "Multicolor 3", ColorType.CHARS_MULTICOLOR2, Core.Settings.MemoryDisplayCharsetMulticolor3 );
          AddSubMenu( m_MenuItemSetColors, "Multicolor 4", ColorType.CHARS_MULTICOLOR2, Core.Settings.MemoryDisplayCharsetMulticolor4 );
          break;
        case MemoryDisplayType.SPRITES:
          AddSubMenu( m_MenuItemSetColors, "Background Color", ColorType.SPRITES_BACKGROUND, Core.Settings.MemoryDisplaySpriteBackgroundColor );
          AddSubMenu( m_MenuItemSetColors, "Custom Color", ColorType.SPRITES_CUSTOM, Core.Settings.MemoryDisplaySpriteCustomColor );
          AddSubMenu( m_MenuItemSetColors, "Multicolor 1", ColorType.SPRITES_MULTICOLOR1, Core.Settings.MemoryDisplaySpriteMulticolor1 );
          AddSubMenu( m_MenuItemSetColors, "Multicolor 2", ColorType.SPRITES_MULTICOLOR2, Core.Settings.MemoryDisplaySpriteMulticolor2 );
          break;
      }
    }



    private void AddSubMenu( ToolStripMenuItem ParentMenu, string Text, ColorType Type, int Default )
    {
      var menu = new ToolStripMenuItem( Text );
      ParentMenu.DropDownItems.Add( menu );

      for ( int i = 0; i < 16; ++i )
      {
        var newItem = new ToolStripMenuItem( i.ToString() );
        menu.DropDownItems.Add( newItem );
        newItem.Tag = Type;
        if ( i == Default )
        {
          newItem.Enabled = false;
          newItem.Checked = true;
        }
        newItem.Click += ColorMenuItem_Click;
      }
    }



    void ColorMenuItem_Click( object sender, EventArgs e )
    {
      ToolStripMenuItem     item = (ToolStripMenuItem)sender;

      item.Checked = true;
      item.Enabled = false;

      var parentMenu = item.GetCurrentParent();
      foreach ( ToolStripMenuItem menuItem in parentMenu.Items )
      {
        if ( ( menuItem != item )
        &&   ( menuItem.Checked ) )
        {
          menuItem.Enabled = true;
          menuItem.Checked = false;
        }
      }

      int     colorIndex = GR.Convert.ToI32( item.Text );
      switch ( (ColorType)item.Tag )
      {
        case ColorType.CHARS_BACKGROUND:
          Core.Settings.MemoryDisplayCharsetBackgroundColor = colorIndex;
          ( (HexBoxCharViewer)hexView.CustomHexViewer ).BackgroundColor = (byte)colorIndex;
          break;
        case ColorType.CHARS_CUSTOM:
          Core.Settings.MemoryDisplayCharsetCustomColor = colorIndex;
          ( (HexBoxCharViewer)hexView.CustomHexViewer ).CustomColor = (byte)colorIndex;
          break;
        case ColorType.CHARS_MULTICOLOR1:
          Core.Settings.MemoryDisplayCharsetMulticolor1 = colorIndex;
          ( (HexBoxCharViewer)hexView.CustomHexViewer ).MultiColor1 = (byte)colorIndex;
          break;
        case ColorType.CHARS_MULTICOLOR2:
          Core.Settings.MemoryDisplayCharsetMulticolor2 = colorIndex;
          ( (HexBoxCharViewer)hexView.CustomHexViewer ).MultiColor2 = (byte)colorIndex;
          break;
        case ColorType.CHARS_MULTICOLOR3:
          Core.Settings.MemoryDisplayCharsetMulticolor3 = colorIndex;
          ( (HexBoxCharViewer)hexView.CustomHexViewer ).MultiColor3 = (byte)colorIndex;
          break;
        case ColorType.CHARS_MULTICOLOR4:
          Core.Settings.MemoryDisplayCharsetMulticolor4 = colorIndex;
          ( (HexBoxCharViewer)hexView.CustomHexViewer ).MultiColor4 = (byte)colorIndex;
          break;
        case ColorType.SPRITES_BACKGROUND:
          Core.Settings.MemoryDisplaySpriteBackgroundColor = colorIndex;
          ( (HexBoxSpriteViewer)hexView.CustomHexViewer ).BackgroundColor = (byte)colorIndex;
          break;
        case ColorType.SPRITES_CUSTOM:
          Core.Settings.MemoryDisplaySpriteCustomColor = colorIndex;
          ( (HexBoxSpriteViewer)hexView.CustomHexViewer ).CustomColor = (byte)colorIndex;
          break;
        case ColorType.SPRITES_MULTICOLOR1:
          Core.Settings.MemoryDisplaySpriteMulticolor1 = colorIndex;
          ( (HexBoxSpriteViewer)hexView.CustomHexViewer ).MultiColor1 = (byte)colorIndex;
          break;
        case ColorType.SPRITES_MULTICOLOR2:
          Core.Settings.MemoryDisplaySpriteMulticolor2 = colorIndex;
          ( (HexBoxSpriteViewer)hexView.CustomHexViewer ).MultiColor2 = (byte)colorIndex;
          break;
      }
      hexView.Invalidate();
    }



    void hexView_ViewScrolled( object sender, EventArgs e )
    {
      int     newValue = ( (int)hexView.VScrollPos );

      if ( m_Offset != newValue )
      {
        m_Offset = newValue;

        if ( ViewScrolled != null )
        {
          ViewScrolled( this, new DebugMemoryEvent( this, m_Offset * hexView.BytesPerLine, hexView.BytesPerLine * hexView.VerticalByteCount ) );
        }
      }
    }



    public void RefreshViewScroller()
    {
      hexView_ViewScrolled( this, new EventArgs() );
    }



    public void SetHexData( GR.Memory.ByteBuffer Data )
    {
      long     oldOffset = hexView.VScrollPos;

      hexView.ByteProvider = new Be.Windows.Forms.DynamicByteProvider( Data.Data() );

      //Debug.Log( "DebugMemory::SetHexData called with " + oldOffset + ", setting " + Data.Length + " bytes" );
      hexView.PerformScrollToLine( oldOffset );
    }



    private void toolStripButtonGoto_Click( object sender, EventArgs e )
    {
      FormGoto frmGoto = new FormGoto();

      if ( frmGoto.ShowDialog() == DialogResult.OK )
      {
        int address = -1;

        if ( frmGoto.chkHex.Checked )
        {
          int.TryParse( frmGoto.editAddress.Text, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out address );
        }
        else
        {
          int.TryParse( frmGoto.editAddress.Text, out address );
        }

        if ( address >= 0 && address <= 65535 )
        {
          int line = address / hexView.BytesPerLine;

          Debug.Log( "DebugMemory::Goto called with " + line );

          hexView.PerformScrollToLine( line );
          RefreshViewScroller();
        }
      }
    }



    private void toolStripBtnHexCaseSwitch_Click( object sender, EventArgs e )
    {
      if ( hexView.HexCasing == Be.Windows.Forms.HexCasing.Lower )
      {
        hexView.HexCasing = Be.Windows.Forms.HexCasing.Upper;
      }
      else
      {
        hexView.HexCasing = Be.Windows.Forms.HexCasing.Lower;
      }
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



    public ByteBuffer HexData
    {
      get;

    }



    public void InvalidateAllMemory()
    {
      Core.Debugging.ActiveMemory.ValidMemory.Clear();
    }



    public void UpdateMemory( RequestData Request, GR.Memory.ByteBuffer Data )
    {
      int Offset = Request.Parameter1;

      for ( int i = 0; i < Data.Length; ++i )
      {
        byte    ramByte = Data.ByteAt( i );

        if ( Request.Reason != RequestReason.MEMORY_FETCH )
        {
          /*
          if ( ramByte != Core.Debugging.ActiveMemory.RAM.ByteAt( Offset + i ) )
          {
            Core.Debugging.ActiveMemory.RAMChanged[Offset + i] = true;

            hexView.SelectedByteProvider.SetByteSelectionState( Offset + i, true );
          }
          else
          {
            Core.Debugging.ActiveMemory.RAMChanged[Offset + i] = false;

            hexView.SelectedByteProvider.SetByteSelectionState( Offset + i, false );
          }*/
          hexView.SelectedByteProvider.SetByteSelectionState( Offset + i, Core.Debugging.ActiveMemory.RAMChanged[Offset + i] );
        }
        //Core.Debugging.ActiveMemory.RAM.SetU8At( Offset + i, ramByte );
        hexView.ByteProvider.WriteByte( Offset + i, ramByte );
      }
      //ValidateMemory( Offset, (int)Data.Length );

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
        sb.Append( Core.Debugging.ActiveMemory.RAM.ByteAt( i * 16 + 0 ).ToString( "x2" ) );
        sb.Append( " " );
        sb.Append( Core.Debugging.ActiveMemory.RAM.ByteAt( i * 16 + 1 ).ToString( "x2" ) );
        sb.Append( " " );
        sb.Append( Core.Debugging.ActiveMemory.RAM.ByteAt( i * 16 + 2 ).ToString( "x2" ) );
        sb.Append( " " );
        sb.Append( Core.Debugging.ActiveMemory.RAM.ByteAt( i * 16 + 3 ).ToString( "x2" ) );
        sb.Append( "  " );
        sb.Append( Core.Debugging.ActiveMemory.RAM.ByteAt( i * 16 + 4 ).ToString( "x2" ) );
        sb.Append( " " );
        sb.Append( Core.Debugging.ActiveMemory.RAM.ByteAt( i * 16 + 5 ).ToString( "x2" ) );
        sb.Append( " " );
        sb.Append( Core.Debugging.ActiveMemory.RAM.ByteAt( i * 16 + 6 ).ToString( "x2" ) );
        sb.Append( " " );
        sb.Append( Core.Debugging.ActiveMemory.RAM.ByteAt( i * 16 + 7 ).ToString( "x2" ) );
        sb.Append( "  " );
        sb.Append( Core.Debugging.ActiveMemory.RAM.ByteAt( i * 16 + 8 ).ToString( "x2" ) );
        sb.Append( " " );
        sb.Append( Core.Debugging.ActiveMemory.RAM.ByteAt( i * 16 + 9 ).ToString( "x2" ) );
        sb.Append( " " );
        sb.Append( Core.Debugging.ActiveMemory.RAM.ByteAt( i * 16 + 10 ).ToString( "x2" ) );
        sb.Append( " " );
        sb.Append( Core.Debugging.ActiveMemory.RAM.ByteAt( i * 16 + 11 ).ToString( "x2" ) );
        sb.Append( "  " );
        sb.Append( Core.Debugging.ActiveMemory.RAM.ByteAt( i * 16 + 12 ).ToString( "x2" ) );
        sb.Append( " " );
        sb.Append( Core.Debugging.ActiveMemory.RAM.ByteAt( i * 16 + 13 ).ToString( "x2" ) );
        sb.Append( " " );
        sb.Append( Core.Debugging.ActiveMemory.RAM.ByteAt( i * 16 + 14 ).ToString( "x2" ) );
        sb.Append( " " );
        sb.Append( Core.Debugging.ActiveMemory.RAM.ByteAt( i * 16 + 15 ).ToString( "x2" ) );
        sb.Append( "\r\n" );
      }
      Debug.Log( sb.ToString() );
    }



    private void btnBinaryStringView_Click( object sender, EventArgs e )
    {
      btnBinaryCharView.Checked = false;
      btnBinarySpriteView.Checked = false;
      btnBinaryStringView.Checked = true;

      if ( hexView.CustomHexViewer is HexBoxPETSCIIViewer )
      {
        ( (HexBoxPETSCIIViewer)hexView.CustomHexViewer ).ToggleViewMode();
      }
      else
      {
        hexView.CustomHexViewer = new HexBoxPETSCIIViewer();
      }

      Core.Settings.MemoryDisplay = MemoryDisplayType.ASCII;

      m_MenuItemHexStringView.Checked = true;

      btnBinarySpriteView.Checked = false;
      m_MenuItemHexSpriteView.Checked = false;
      btnBinaryCharView.Checked = false;
      m_MenuItemHexCharView.Checked = false;
      hexView.Invalidate();
      SetColorSubmenu();
    }



    private void btnBinaryCharView_Click( object sender, EventArgs e )
    {
      btnBinaryCharView.Checked = true;
      btnBinarySpriteView.Checked = false;
      btnBinaryStringView.Checked = false;

      if ( hexView.CustomHexViewer is HexBoxCharViewer )
      {
        ( (HexBoxCharViewer)hexView.CustomHexViewer ).ToggleViewMode();
        Core.Settings.MemoryDisplayCharsetMode = ((HexBoxCharViewer)hexView.CustomHexViewer).Mode;
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

      ApplyHexViewColors();

      hexView.Invalidate();
      SetColorSubmenu();
    }



    private void btnBinarySpriteView_Click( object sender, EventArgs e )
    {
      btnBinaryCharView.Checked = false;
      btnBinarySpriteView.Checked = true;
      btnBinaryStringView.Checked = false;
      if ( hexView.CustomHexViewer is HexBoxSpriteViewer )
      {
        ( (HexBoxSpriteViewer)hexView.CustomHexViewer ).ToggleViewMode();
        Core.Settings.MemoryDisplaySpriteMulticolor = ( (HexBoxSpriteViewer)hexView.CustomHexViewer ).MultiColor;
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

      ApplyHexViewColors();

      hexView.Invalidate();
      SetColorSubmenu();
    }



    public void ApplyHexViewColors()
    {
      if ( hexView.CustomHexViewer is HexBoxCharViewer )
      {
        var charViewer = (HexBoxCharViewer)hexView.CustomHexViewer;

        charViewer.BackgroundColor  = (byte)Core.Settings.MemoryDisplayCharsetBackgroundColor;
        charViewer.CustomColor      = (byte)Core.Settings.MemoryDisplayCharsetCustomColor;
        charViewer.MultiColor1      = (byte)Core.Settings.MemoryDisplayCharsetMulticolor1;
        charViewer.MultiColor2      = (byte)Core.Settings.MemoryDisplayCharsetMulticolor2;
        charViewer.MultiColor3      = (byte)Core.Settings.MemoryDisplayCharsetMulticolor3;
        charViewer.MultiColor4      = (byte)Core.Settings.MemoryDisplayCharsetMulticolor4;
        charViewer.Mode             = Core.Settings.MemoryDisplayCharsetMode;
      }
      if ( hexView.CustomHexViewer is HexBoxSpriteViewer )
      {
        var spriteViewer = (HexBoxSpriteViewer)hexView.CustomHexViewer;

        spriteViewer.BackgroundColor  = (byte)Core.Settings.MemoryDisplaySpriteBackgroundColor;
        spriteViewer.CustomColor      = (byte)Core.Settings.MemoryDisplaySpriteCustomColor;
        spriteViewer.MultiColor1      = (byte)Core.Settings.MemoryDisplaySpriteMulticolor1;
        spriteViewer.MultiColor2      = (byte)Core.Settings.MemoryDisplaySpriteMulticolor2;
        spriteViewer.MultiColor       = Core.Settings.MemoryDisplaySpriteMulticolor;
      }
    }



    protected override void OnClosed( EventArgs e )
    {
      ViewScrolled -= new DebugMemory.DebugMemoryEventCallback( Core.MainForm.m_DebugMemory_ViewScrolled );
      Core.Debugging.MemoryViews.Remove( this );
      base.OnClosed( e );
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
        Core.Debugging.ActiveMemory = Core.Debugging.MemoryCPU;
      }
      else
      {
        toolStripBtnMemoryFromCPU.Image = C64Studio.Properties.Resources.icon_memory_ram.ToBitmap();
        toolStripBtnMemoryFromCPU.ToolTipText = "Show RAM";
        Core.Debugging.ActiveMemory = Core.Debugging.MemoryRAM;
      }
      SetHexData( Core.Debugging.ActiveMemory.RAM );

      if ( ViewScrolled != null )
      {
        ViewScrolled( this, new DebugMemoryEvent( this, m_Offset * hexView.BytesPerLine, hexView.BytesPerLine * hexView.VerticalByteCount ) );
      }
    }



    public override void RefreshDisplayOptions()
    {
      base.RefreshDisplayOptions();

      ChangedColor = GR.Color.Helper.FromARGB( Core.Settings.FGColor( ColorableElement.CHANGED_DEBUG_ELEMENT ) );
      UnchangedColor = GR.Color.Helper.FromARGB( Core.Settings.FGColor( ColorableElement.BACKGROUND_CONTROL ) );

      hexView.MarkedForeColor = ChangedColor;
    }



    private void toolStripBtnAddView_Click( object sender, EventArgs e )
    {
      var document = new DebugMemory( Core );
      document.ShowHint = DockState.Float;
      document.Core = Core;
      document.Text = "Memory View";
      document.Load();
      document.Show( Core.MainForm.panelMain );
    }



  }
}
