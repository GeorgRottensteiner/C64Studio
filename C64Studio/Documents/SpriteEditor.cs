using C64Studio.CustomRenderer;
using C64Studio.Displayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using C64Studio.Formats;
using GR.Memory;
using C64Studio.Types;
using C64Studio.Converter;
using RetroDevStudio;
using RetroDevStudio.Types;
using System.Linq;
using C64Studio.Controls;

namespace C64Studio
{
  public partial class SpriteEditor : BaseDocument
  {
    enum ToolMode
    {
      SINGLE_PIXEL,
      FILL
    }

    private int                         m_CurrentSprite = 0;

    private string                      m_ImportError = "";

    private bool                        m_IsSpriteProject = true;

    private Formats.SpriteProject       m_SpriteProject = new C64Studio.Formats.SpriteProject();

    private Formats.SpriteProject.Layer m_CurrentLayer = null;

    private bool                        m_ButtonReleased = false;

    private ToolMode                    m_Mode = ToolMode.SINGLE_PIXEL;

    private Timer                       m_AnimTimer = new Timer();

    private int                         m_AnimFramePos = 0;
    private int                         m_AnimFrameTicks = 0;

    private int                         m_SpriteWidth = 24;
    private int                         m_SpriteHeight = 21;

    private int                         m_SpriteEditorOrigWidth = -1;
    private int                         m_SpriteEditorOrigHeight = -1;

    private ColorSettingsBase           _ColorSettingsDlg = null;



    public SpriteEditor( StudioCore Core )
    {
      this.Core = Core;
      DocumentInfo.Type = ProjectElement.ElementType.SPRITE_SET;
      DocumentInfo.UndoManager.MainForm = Core.MainForm;

      m_IsSaveable = true;
      InitializeComponent();

      GR.Image.DPIHandler.ResizeControlsForDPI( this );

      m_SpriteEditorOrigWidth   = pictureEditor.Width;
      m_SpriteEditorOrigHeight  = pictureEditor.Height;

      listLayers.ItemAdded += new ArrangedItemList.ItemModifiedEventHandler( listLayers_ItemAdded );

      pictureEditor.DisplayPage.Create( m_SpriteWidth, m_SpriteHeight, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
      layerPreview.DisplayPage.Create( 320, 200, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
      panelSprites.PixelFormat = System.Drawing.Imaging.PixelFormat.Format24bppRgb;
      panelSprites.SetDisplaySize( 4 * m_SpriteWidth, 6 * m_SpriteHeight );
      panelSprites.ClientSize = new System.Drawing.Size( 4 * m_SpriteWidth * 2 + System.Windows.Forms.SystemInformation.VerticalScrollBarWidth, 6 * m_SpriteHeight * 2 );

      PaletteManager.ApplyPalette( pictureEditor.DisplayPage, m_SpriteProject.Colors.Palette );
      PaletteManager.ApplyPalette( panelSprites.DisplayPage, m_SpriteProject.Colors.Palette );
      PaletteManager.ApplyPalette( layerPreview.DisplayPage, m_SpriteProject.Colors.Palette );

      for ( int i = 0; i < m_SpriteProject.TotalNumberOfSprites; ++i )
      {
        panelSprites.Items.Add( i.ToString(), m_SpriteProject.Sprites[i].Tile.Image );
        PaletteManager.ApplyPalette( m_SpriteProject.Sprites[i].Tile.Image, m_SpriteProject.Colors.Palette );
        comboSprite.Items.Add( i );
      }
      ChangeColorSettingsDialog();

      for ( int i = 0; i < 16; ++i )
      {
        comboLayerColor.Items.Add( i.ToString( "d2" ) );
        comboLayerBGColor.Items.Add( i.ToString( "d2" ) );
      }
      comboLayerColor.SelectedIndex = 1;
      comboLayerBGColor.SelectedIndex = 0;
      comboSprite.SelectedIndex = 0;

      pictureEditor.SetImageSize( m_SpriteWidth, m_SpriteHeight );
      panelSprites.SelectedIndex = 0;

      pictureEditor.PostPaint += new GR.Forms.FastPictureBox.PostPaintCallback( pictureEditor_PostPaint );

      checkExportToDataIncludeRes.Checked = true;
      checkExportToDataWrap.Checked = true;
      comboExportRange.Items.Add( "All" );
      comboExportRange.Items.Add( "Selection" );
      comboExportRange.Items.Add( "Range" );
      comboExportRange.SelectedIndex = 0;

      foreach ( SpriteProject.SpriteProjectMode mode in Enum.GetValues( typeof( SpriteProject.SpriteProjectMode ) ) )
      {
        comboSpriteProjectMode.Items.Add( GR.EnumHelper.GetDescription( mode ) );
      }
      comboSpriteProjectMode.SelectedIndex = 0;

      panelSprites.KeyDown += new KeyEventHandler( HandleKeyDown );
      pictureEditor.PreviewKeyDown += new PreviewKeyDownEventHandler( pictureEditor_PreviewKeyDown );

      RebuildSpriteImage( m_CurrentSprite );

      m_CurrentLayer = new Formats.SpriteProject.Layer();
      m_CurrentLayer.Name = "Default";
      m_SpriteProject.SpriteLayers.Add( m_CurrentLayer );
      foreach ( var layer in m_SpriteProject.SpriteLayers )
      {
        ArrangedItemEntry item = new ArrangedItemEntry( layer.Name );
        item.Tag = layer;
        listLayers.Items.Add( item );
      }

      labelCharNo.Text = "Sprite: " + m_CurrentSprite.ToString();
      pictureEditor.Image = m_SpriteProject.Sprites[m_CurrentSprite].Tile.Image;

      panelSprites_SelectedIndexChanged( null, null );

      RefreshDisplayOptions();

      m_AnimTimer.Tick += animTimer_Tick;
    }



    private void animTimer_Tick( object sender, EventArgs e )
    {
      if ( m_AnimFramePos >= m_SpriteProject.SpriteLayers.Count )
      {
        m_AnimFramePos = 0;
        m_AnimFrameTicks = 0;
        listLayers.SelectedIndex = m_AnimFramePos;
      }
      m_AnimFrameTicks += 100;
      if ( m_AnimFrameTicks >= m_CurrentLayer.DelayMS )
      {
        m_AnimFrameTicks -= m_CurrentLayer.DelayMS;
        m_AnimFramePos = ( m_AnimFramePos + 1 ) % m_SpriteProject.SpriteLayers.Count;
        listLayers.SelectedIndex = m_AnimFramePos;
      }
    }



    void pictureEditor_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
    {
      KeyEventArgs ke = new KeyEventArgs( e.KeyData );
      HandleKeyDown( sender, ke );
    }



    void DrawSpriteImage( GR.Image.IImage Target, int X, int Y, GR.Memory.ByteBuffer Data, int Width, int Height, int CustomColor, SpriteMode Mode, int BackgroundColor, int MultiColor1, int MultiColor2, bool ExpandX, bool ExpandY )
    {
      switch ( Mode )
      {
        case SpriteMode.COMMODORE_24_X_21_MULTICOLOR:
          SpriteDisplayer.DisplayMultiColorSprite( Data, Width, Height, BackgroundColor, MultiColor1, MultiColor2, CustomColor, Target, X, Y, ExpandX, ExpandY );
          break;
        case SpriteMode.COMMODORE_24_X_21_HIRES:
          SpriteDisplayer.DisplayHiResSprite( Data, Width, Height, BackgroundColor, CustomColor, Target, X, Y, ExpandX, ExpandY );
          break;
        case SpriteMode.MEGA65_16_X_21_16_COLORS:
        case SpriteMode.MEGA65_8_X_21_16_COLORS:
          SpriteDisplayer.DisplayFCMSprite( Data, Width, Height, BackgroundColor, Target, X, Y, ExpandX, ExpandY );
          break;
        default:
          Debug.Log( "DrawSpriteImage unsupported mode " + Mode );
          break;
      }
    }



    void RebuildSpriteImage( int SpriteIndex )
    {
      var Data = m_SpriteProject.Sprites[SpriteIndex];

      DrawSpriteImage( Data.Tile.Image, 0, 0, Data.Tile.Data, Data.Tile.Width, Data.Tile.Height,
        Data.Tile.CustomColor,
        Data.Mode,
        m_SpriteProject.Colors.BackgroundColor,
        m_SpriteProject.Colors.MultiColor1, m_SpriteProject.Colors.MultiColor2,
        false, false );
    }



    private new bool Modified
    {
      get
      {
        return base.Modified;
      }
      set
      {
        if ( value )
        {
          SetModified();
        }
        else
        {
          SetUnmodified();
        }
        saveSpriteProjectToolStripMenuItem.Enabled = Modified;
      }
    }



    void MirrorX()
    {
      var selectedSprites = panelSprites.SelectedIndices;

      bool firstEntry = true;
      foreach ( var spriteIndex in selectedSprites )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoSpritesetSpriteChange( this, m_SpriteProject, spriteIndex ), firstEntry );
        firstEntry = false;
        var sprite = m_SpriteProject.Sprites[spriteIndex];

        for ( int y = 0; y < m_SpriteHeight; ++y )
        {
          for ( int x = 0; x < m_SpriteWidth / 2; x += Lookup.PixelWidth( sprite.Tile.Mode ) )
          {
            int  tempColor = sprite.Tile.GetPixel( x, y );
            sprite.Tile.SetPixel( x, y, sprite.Tile.GetPixel( m_SpriteWidth - Lookup.PixelWidth( sprite.Tile.Mode ) - x, y ) );
            sprite.Tile.SetPixel( m_SpriteWidth - Lookup.PixelWidth( sprite.Tile.Mode ) - x, y, tempColor );
          }
        }
        RebuildSpriteImage( spriteIndex );
        panelSprites.InvalidateItemRect( spriteIndex );
      }
      pictureEditor.Invalidate();
      Modified = true;
    }



    void MirrorY()
    {
      var selectedSprites = panelSprites.SelectedIndices;

      bool firstEntry = true;
      foreach ( var spriteIndex in selectedSprites )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoSpritesetSpriteChange( this, m_SpriteProject, spriteIndex ), firstEntry );
        firstEntry = false;
        var sprite = m_SpriteProject.Sprites[spriteIndex];

        for ( int y = 0; y < m_SpriteHeight / 2; ++y )
        {
          for ( int x = 0; x < m_SpriteWidth; ++x )
          {
            int oldValue = sprite.Tile.GetPixel( x, y );
            sprite.Tile.SetPixel( x, y, sprite.Tile.GetPixel( x, m_SpriteHeight - 1 - y ) );
            sprite.Tile.SetPixel( x, m_SpriteHeight - 1 - y, oldValue );
          }
        }
        RebuildSpriteImage( spriteIndex );
        panelSprites.InvalidateItemRect( spriteIndex );
      }
      pictureEditor.Invalidate();
      Modified = true;
    }



    void HandleKeyDown( object sender, KeyEventArgs e )
    {
      if ( ( e.Modifiers == Keys.Control )
      &&   ( e.KeyCode == Keys.C ) )
      {
        // copy
        CopySpriteToClipboard();
      }
      else if ( ( e.Modifiers == Keys.Control )
      &&        ( e.KeyCode == Keys.V ) )
      {
        PasteFromClipboard();
        if ( m_ImportError.Length > 0 )
        {
          System.Windows.Forms.MessageBox.Show( m_ImportError, "Error while converting" );
        }
      }
    }



    private void comboColor_DrawItem( object sender, DrawItemEventArgs e )
    {
      ComboBox combo = (ComboBox)sender;

      Core.Theming.DrawSingleColorComboBox( combo, e, ConstantData.Palette );
    }



    private void comboMulticolor_DrawItem( object sender, DrawItemEventArgs e )
    {
      ComboBox combo = (ComboBox)sender;

      Core.Theming.DrawMultiColorComboBox( combo, e, ConstantData.Palette );
    }



    private void pictureEditor_MouseDown( object sender, MouseEventArgs e )
    {
      pictureEditor.Focus();
      HandleMouseOnEditor( e.X, e.Y, e.Button );
    }



    private void HandleMouseOnEditor( int X, int Y, MouseButtons Buttons )
    {
      if ( !pictureEditor.ClientRectangle.Contains( X, Y ) )
      {
        return;
      }

      int     charX = ( X * m_SpriteWidth ) / pictureEditor.ClientRectangle.Width;
      int     charY = ( Y * m_SpriteHeight ) / pictureEditor.ClientRectangle.Height;

      var affectedSprite = m_SpriteProject.Sprites[m_CurrentSprite];

      if ( ( Buttons & MouseButtons.Left ) != 0 )
      {
        int     colorIndex = (int)_ColorSettingsDlg.SelectedColor;
        if ( ( m_SpriteProject.Mode != SpriteProject.SpriteProjectMode.MEGA65_16_X_21_16_COLORS )
        &&   ( m_SpriteProject.Mode != SpriteProject.SpriteProjectMode.MEGA65_8_X_21_16_COLORS ) )
        {
          if ( colorIndex == (int)ColorType.MULTICOLOR_2 )
          {
            colorIndex = (int)ColorType.CUSTOM_COLOR;
          }
          else if ( colorIndex == (int)ColorType.CUSTOM_COLOR )
          {
            colorIndex = (int)ColorType.MULTICOLOR_2;
          }
        }

        Undo.UndoTask undo = null;
        if ( m_ButtonReleased )
        {
          undo = new Undo.UndoSpritesetSpriteChange( this, m_SpriteProject, m_CurrentSprite );
        }

        bool  modified = false;
        switch ( m_Mode )
        {
          case ToolMode.SINGLE_PIXEL:
            modified = affectedSprite.Tile.SetPixel( charX, charY, colorIndex );
            break;
          case ToolMode.FILL:
            modified = affectedSprite.Tile.Fill( charX, charY, colorIndex );
            break;
        }

        if ( modified )
        {
          Modified = true;

          if ( undo != null )
          {
            DocumentInfo.UndoManager.AddUndoTask( undo );
            m_ButtonReleased = false;
          }

          SpriteChanged( m_CurrentSprite );
        }
      }
      else
      {
        m_ButtonReleased = true;
      }
      if ( ( Buttons & MouseButtons.Right ) != 0 )
      {
        int   pickedColor = affectedSprite.Tile.GetPixel( charX, charY );

        if ( ( m_SpriteProject.Mode != SpriteProject.SpriteProjectMode.MEGA65_16_X_21_16_COLORS )
        &&   ( m_SpriteProject.Mode != SpriteProject.SpriteProjectMode.MEGA65_8_X_21_16_COLORS ) )
        {
          if ( pickedColor == (int)ColorType.MULTICOLOR_2 )
          {
            pickedColor = (int)ColorType.CUSTOM_COLOR;
          }
          else if ( pickedColor == (int)ColorType.CUSTOM_COLOR )
          {
            pickedColor = (int)ColorType.MULTICOLOR_2;
          }
        }

        _ColorSettingsDlg.SelectedColor = (ColorType)pickedColor;

        if ( ( m_SpriteProject.Mode == SpriteProject.SpriteProjectMode.MEGA65_16_X_21_16_COLORS )
        ||   ( m_SpriteProject.Mode == SpriteProject.SpriteProjectMode.MEGA65_8_X_21_16_COLORS ) )
        {
          affectedSprite.Tile.CustomColor = pickedColor;
        }
      }
    }



    private void pictureEditor_MouseMove( object sender, MouseEventArgs e )
    {
      MouseButtons    buttons = e.Button;
      if ( !pictureEditor.Focused )
      {
        buttons = 0;
      }
      HandleMouseOnEditor( e.X, e.Y, buttons );
    }



    private void panelSprites_SelectedIndexChanged( object sender, EventArgs e )
    {
      int newChar = panelSprites.SelectedIndex;
      if ( newChar != -1 )
      {
        labelCharNo.Text = "Sprite: " + newChar.ToString();
        m_CurrentSprite = newChar;

        DoNotUpdateFromControls = true;

        if ( !Lookup.HasCustomPalette( m_SpriteProject.Sprites[m_CurrentSprite].Tile.Mode ) )
        {
          _ColorSettingsDlg.CustomColor = m_SpriteProject.Sprites[m_CurrentSprite].Tile.CustomColor;

          _ColorSettingsDlg.MultiColorEnabled = ( m_SpriteProject.Sprites[m_CurrentSprite].Mode == SpriteMode.COMMODORE_24_X_21_MULTICOLOR );
        }
        else
        {
          _ColorSettingsDlg.ActivePalette = m_SpriteProject.Sprites[m_CurrentSprite].Tile.Colors.ActivePalette;
          m_SpriteProject.Colors.ActivePalette = m_SpriteProject.Sprites[m_CurrentSprite].Tile.Colors.ActivePalette;
        }
        DoNotUpdateFromControls = false;

        PaletteManager.ApplyPalette( pictureEditor.DisplayPage, m_SpriteProject.Colors.Palettes[m_SpriteProject.Sprites[m_CurrentSprite].Tile.Colors.ActivePalette] );
        pictureEditor.Image = m_SpriteProject.Sprites[m_CurrentSprite].Tile.Image;
      }
      btnDeleteSprite.Enabled = ( panelSprites.SelectedIndex != -1 );
    }



    private void openToolStripMenuItem_Click( object sender, EventArgs e )
    {
      string filename;

      if ( OpenFile( "Open Sprite Project or File", Types.Constants.FILEFILTER_SPRITE + Types.Constants.FILEFILTER_SPRITE_SPRITEPAD + Types.Constants.FILEFILTER_ALL, out filename ) )
      {
        ImportSprites( filename, true, true );
      }
    }



    public void Clear()
    {
      for ( int i = 0; i < m_SpriteProject.TotalNumberOfSprites; ++i )
      {
        m_SpriteProject.Sprites[i].Tile.CustomColor = 0;
        m_SpriteProject.Sprites[i].Mode = SpriteMode.COMMODORE_24_X_21_HIRES;
      }
      //DocumentInfo.DocumentFilename = "";
      m_SpriteProject.SpriteLayers.Clear();

      m_CurrentSprite = 0;
      m_CurrentLayer = new C64Studio.Formats.SpriteProject.Layer();
      m_CurrentLayer.Name = "Default";
      m_SpriteProject.SpriteLayers.Add( m_CurrentLayer );
      listLayerSprites.Items.Clear();
      listLayers.Items.Clear();

      foreach ( var layer in m_SpriteProject.SpriteLayers )
      {
        ArrangedItemEntry item = new ArrangedItemEntry( layer.Name );
        item.Tag = layer;
        listLayers.Items.Add( item );
      }
      CurrentSpriteModified();
      RedrawPreviewLayer();
    }



    private void CurrentSpriteModified()
    {
      // check if preview needs to be redrawn
      if ( m_CurrentLayer != null )
      {
        foreach ( var sprite in m_CurrentLayer.Sprites )
        {
          if ( sprite.Index == m_CurrentSprite )
          {
            RedrawPreviewLayer();
            break;
          }
        }
      }
    }



    public bool ImportSprites( string Filename, bool OnlyImportFromProject, bool AddUndo )
    {
      Clear();

      GR.Memory.ByteBuffer projectFile = GR.IO.File.ReadAllBytes( Filename );
      if ( projectFile == null )
      {
        return false;
      }

      GR.IO.MemoryReader memIn = projectFile.MemoryReader();

      if ( System.IO.Path.GetExtension( Filename ).ToUpper() == ".SPD" )
      {
        var spritePad = new SpritePadProject();

        if ( !spritePad.ReadFromBuffer( projectFile ) )
        {
          return false;
        }
        comboSpriteProjectMode.SelectedIndex = (int)SpriteProject.SpriteProjectMode.COMMODORE_24_X_21_HIRES_OR_MC;

        m_SpriteProject.Colors.BackgroundColor = spritePad.BackgroundColor;
        m_SpriteProject.Colors.MultiColor1     = spritePad.MultiColor1;
        m_SpriteProject.Colors.MultiColor2     = spritePad.MultiColor2;
        for ( int i = 0; i < spritePad.NumSprites; ++i )
        {
          if ( i < m_SpriteProject.Sprites.Count )
          {
            if ( AddUndo )
            {
              DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoSpritesetSpriteChange( this, m_SpriteProject, i ), i == 0 );
            }

            spritePad.Sprites[i].Data.CopyTo( m_SpriteProject.Sprites[i].Tile.Data, 0, 63 );
            m_SpriteProject.Sprites[i].Tile.CustomColor = spritePad.Sprites[i].Color;
            m_SpriteProject.Sprites[i].Mode   = spritePad.Sprites[i].Multicolor ? SpriteMode.COMMODORE_24_X_21_MULTICOLOR : SpriteMode.COMMODORE_24_X_21_HIRES;
          }
        }
        ChangeColorSettingsDialog();
        OnPaletteChanged();

        editSpriteFrom.Text = "0";
        editSpriteCount.Text = spritePad.NumSprites.ToString();

        if ( ( m_SpriteProject.ExportStartIndex != 0 )
        ||   ( m_SpriteProject.ExportSpriteCount != 256 ) )
        {
          comboExportRange.SelectedIndex = 2;
        }

        panelSprites.Invalidate();
        pictureEditor.Invalidate();
        Modified = false;

        saveSpriteProjectToolStripMenuItem.Enabled = true;
        closeCharsetProjectToolStripMenuItem.Enabled = true;
        EnableFileWatcher();
        return true;
      }
      else if ( System.IO.Path.GetExtension( Filename ).ToUpper() != ".SPRITEPROJECT" )
      {
        comboSpriteProjectMode.SelectedIndex = (int)SpriteProject.SpriteProjectMode.COMMODORE_24_X_21_HIRES_OR_MC;
        int numSprites = (int)projectFile.Length / 64;
        for ( int i = 0; i < numSprites; ++i )
        {
          GR.Memory.ByteBuffer tempBuffer = new GR.Memory.ByteBuffer();

          memIn.ReadBlock( tempBuffer, 64 );
          if ( i < m_SpriteProject.Sprites.Count )
          {
            if ( AddUndo )
            {
              DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoSpritesetSpriteChange( this, m_SpriteProject, i ), i == 0 );
            }

            tempBuffer.CopyTo( m_SpriteProject.Sprites[i].Tile.Data, 0, 63 );
            m_SpriteProject.Sprites[i].Tile.CustomColor = ( tempBuffer.ByteAt( 63 ) & 0xf );
            m_SpriteProject.Sprites[i].Mode = ( ( tempBuffer.ByteAt( 63 ) & 0x80 ) != 0 ) ? SpriteMode.COMMODORE_24_X_21_MULTICOLOR : SpriteMode.COMMODORE_24_X_21_HIRES;
          }
        }
        ChangeColorSettingsDialog();
        OnPaletteChanged();

        editSpriteFrom.Text = "0";
        editSpriteCount.Text  = numSprites.ToString();

        panelSprites.Invalidate();
        pictureEditor.Invalidate();
        Modified = false;

        saveSpriteProjectToolStripMenuItem.Enabled = true;
        closeCharsetProjectToolStripMenuItem.Enabled = true;
        EnableFileWatcher();
        return true;
      }

      // sprite project
      if ( OnlyImportFromProject )
      {
        // only import sprite data
        Formats.SpriteProject   sprites = new C64Studio.Formats.SpriteProject();

        if ( !sprites.ReadFromBuffer( projectFile ) )
        {
          return false;
        }

        panelSprites.Items.Clear();
        comboSprite.Items.Clear();

        if ( AddUndo )
        {
          for ( int spriteIndex = 0; spriteIndex < m_SpriteProject.TotalNumberOfSprites; ++spriteIndex )
          {
            DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoSpritesetSpriteChange( this, m_SpriteProject, spriteIndex ), spriteIndex == 0 );
          }
        }

        m_SpriteProject.TotalNumberOfSprites = sprites.TotalNumberOfSprites;
        m_SpriteProject.Sprites.Clear();
        for ( int i = 0; i < m_SpriteProject.TotalNumberOfSprites; ++i )
        {
          m_SpriteProject.Sprites.Add( new SpriteProject.SpriteData( sprites.Sprites[i] ) );

          panelSprites.Items.Add( i.ToString(), m_SpriteProject.Sprites[i].Tile.Image );
          comboSprite.Items.Add( i );
        }
        ChangeColorSettingsDialog();
        OnPaletteChanged();
        comboSprite.SelectedIndex = 0;
        panelSprites.Invalidate();
        pictureEditor.Invalidate();
        Modified = false;
        return true;
      }

      if ( AddUndo )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoSpritesetValuesChange( this, m_SpriteProject ), true );
        for ( int spriteIndex = 0; spriteIndex < m_SpriteProject.TotalNumberOfSprites; ++spriteIndex )
        {
          DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoSpritesetSpriteChange( this, m_SpriteProject, spriteIndex ) );
        }
      }

      m_IsSpriteProject = true;
      m_CurrentLayer = null;
      m_SpriteProject.SpriteLayers.Clear();
      listLayerSprites.Items.Clear();
      listLayers.Items.Clear();

      if ( !m_SpriteProject.ReadFromBuffer( projectFile ) )
      {
        return false;
      }

      AdjustSpriteSizes();
      panelSprites.Items.Clear();
      comboSprite.Items.Clear();

      comboSpriteProjectMode.SelectedIndex = (int)m_SpriteProject.Mode;

      ChangeColorSettingsDialog();

      editSpriteFrom.Text = m_SpriteProject.ExportStartIndex.ToString();
      editSpriteCount.Text = m_SpriteProject.ExportSpriteCount.ToString();
      if ( ( m_SpriteProject.ExportStartIndex != 0 )
      ||   ( m_SpriteProject.ExportSpriteCount != 256 ) )
      {
        comboExportRange.SelectedIndex = 2;
      }

      // re-add item (update tags)
      for ( int i = 0; i < m_SpriteProject.TotalNumberOfSprites; ++i )
      {
        panelSprites.Items.Add( i.ToString(), m_SpriteProject.Sprites[i].Tile.Image );
        comboSprite.Items.Add( i );
      }
      pictureEditor.Image = m_SpriteProject.Sprites[m_CurrentSprite].Tile.Image;
      panelSprites.Invalidate();
      pictureEditor.Invalidate();

      if ( m_SpriteProject.SpriteLayers.Count == 0 )
      {
        m_CurrentLayer = new C64Studio.Formats.SpriteProject.Layer();
        m_CurrentLayer.Name = "Default";
        m_SpriteProject.SpriteLayers.Add( m_CurrentLayer );
      }
      else
      {
        m_CurrentLayer = m_SpriteProject.SpriteLayers[0];
      }
      comboLayerBGColor.SelectedIndex = m_CurrentLayer.BackgroundColor;

      AddAllLayers();
      if ( listLayers.Items.Count > 0 )
      {
        listLayers.SelectedIndices.Add( 0 );
      }
      OnPaletteChanged();

      Modified = false;

      if ( DocumentInfo.Element == null )
      {
        DocumentInfo.DocumentFilename = Filename;
      }

      saveSpriteProjectToolStripMenuItem.Enabled = true;
      closeCharsetProjectToolStripMenuItem.Enabled = true;
      comboSprite.SelectedIndex = 0;
      EnableFileWatcher();
      return true;
    }



    private void AddAllLayers()
    {
      foreach ( var layer in m_SpriteProject.SpriteLayers )
      {
        ArrangedItemEntry item = new ArrangedItemEntry( layer.Name );
        item.Tag = layer;
        listLayers.Items.Add( item );
      }
      listLayers.UpdateUI();

      int   spriteIndex = 0;
      foreach ( var sprite in m_CurrentLayer.Sprites )
      {
        ArrangedItemEntry item = new ArrangedItemEntry();

        item.Text = sprite.Index.ToString() + ", " + sprite.X.ToString() + ", " + sprite.Y.ToString();
        item.Tag = sprite;
        listLayerSprites.Items.Add( item );

        ++spriteIndex;
      }
    }



    public override bool Load()
    {
      if ( string.IsNullOrEmpty( DocumentInfo.DocumentFilename ) )
      {
        return false;
      }
      try
      {
        ImportSprites( DocumentInfo.FullPath, false, false );
      }
      catch ( System.IO.IOException ex )
      {
        System.Windows.Forms.MessageBox.Show( "Could not load sprite project file " + DocumentInfo.FullPath + ".\r\n" + ex.Message, "Could not load file" );
        return false;
      }
      SetUnmodified();
      return true;
    }



    protected override bool QueryFilename( out string Filename )
    {
      Filename = "";

      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Save Sprite Project as";
      saveDlg.Filter = "Sprite Projects|*.spriteproject|All Files|*.*";
      if ( DocumentInfo.Project != null )
      {
        saveDlg.InitialDirectory = DocumentInfo.Project.Settings.BasePath;
      }
      if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return false;
      }

      Filename = saveDlg.FileName;
      return true;
    }



    protected override bool PerformSave( string FullPath )
    {
      GR.Memory.ByteBuffer dataToSave = SaveToBuffer();

      return SaveDocumentData( FullPath, dataToSave );
    }



    private void closeCharsetProjectToolStripMenuItem_Click( object sender, EventArgs e )
    {
      if ( DocumentInfo.DocumentFilename == "" )
      {
        return;
      }
      if ( Modified )
      {
        DialogResult doSave = MessageBox.Show( "There are unsaved changes in your character set. Save now?", "Save changes?", MessageBoxButtons.YesNoCancel );
        if ( doSave == DialogResult.Cancel )
        {
          return;
        }
        if ( doSave == DialogResult.Yes )
        {
          Save( SaveMethod.SAVE );
        }
      }
      Clear();
      DocumentInfo.DocumentFilename = "";
      Modified = false;
      panelSprites.Invalidate();
      pictureEditor.Invalidate();

      closeCharsetProjectToolStripMenuItem.Enabled = false;
      saveSpriteProjectToolStripMenuItem.Enabled = false;
    }



    private void saveCharsetProjectToolStripMenuItem_Click( object sender, EventArgs e )
    {
      Save( SaveMethod.SAVE );
    }



    public override GR.Memory.ByteBuffer SaveToBuffer()
    {
      GR.Memory.ByteBuffer projectFile = new GR.Memory.ByteBuffer();

      List<int> exportIndices = GetExportIndices();

      if ( m_IsSpriteProject )
      {
        projectFile = m_SpriteProject.SaveToBuffer();
      }
      else
      {
        for ( int i = 0; i < exportIndices.Count; ++i )
        {
          projectFile.Append( m_SpriteProject.Sprites[exportIndices[i]].Tile.Data );
          projectFile.AppendU8( (byte)m_SpriteProject.Sprites[exportIndices[i]].Tile.CustomColor );
        }
      }
      return projectFile;
    }



    private List<int> GetExportIndices()
    {
      List<int>     exportIndices = new List<int>();

      switch ( comboExportRange.SelectedIndex )
      {
        case 0:
          // all
          for ( int i = 0; i < m_SpriteProject.TotalNumberOfSprites; ++i )
          {
            exportIndices.Add( i );
          }
          break;
        case 1:
          // selection
          exportIndices = panelSprites.SelectedIndices;
          break;
        case 2:
          // rage
          {
            int     startIndex = GR.Convert.ToI32( editSpriteFrom.Text );
            int     numSprites = GR.Convert.ToI32( editSpriteCount.Text );

            if ( startIndex < 0 )
            {
              startIndex = 0;
            }
            if ( startIndex >= m_SpriteProject.TotalNumberOfSprites )
            {
              startIndex = m_SpriteProject.TotalNumberOfSprites - 1;
            }
            if ( numSprites < 0 )
            {
              numSprites = 1;
            }
            if ( startIndex + numSprites > m_SpriteProject.TotalNumberOfSprites )
            {
              numSprites = m_SpriteProject.TotalNumberOfSprites - startIndex;
            }
            for ( int i = 0; i < numSprites; ++i )
            {
              exportIndices.Add( startIndex + i );
            }
          }
          break;
      }
      return exportIndices;
    }



    private void btnExportSprite_Click( object sender, EventArgs e )
    {
      var exportIndices = GetExportIndices();
      if ( exportIndices.Count == 0 )
      {
        return;
      }

      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Export Sprites to";
      saveDlg.Filter = "Sprites|*.spr|All Files|*.*";
      saveDlg.FileName = m_SpriteProject.ExportFilename;
      if ( DocumentInfo.Project != null )
      {
        saveDlg.InitialDirectory = DocumentInfo.Project.Settings.BasePath;
      }
      if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return;
      }
      if ( m_SpriteProject.ExportFilename != saveDlg.FileName )
      {
        m_SpriteProject.ExportFilename = saveDlg.FileName;
        Modified = true;
      }

      GR.Memory.ByteBuffer exportData = GatherExportData();
      GR.IO.File.WriteAllBytes( m_SpriteProject.ExportFilename, exportData );
    }



    private void btnExportSpriteToData_Click( object sender, EventArgs e )
    {
      int wrapByteCount = GR.Convert.ToI32( editWrapByteCount.Text );
      if ( wrapByteCount <= 0 )
      {
        wrapByteCount = 8;
      }
      string prefix = editPrefix.Text;

      GR.Memory.ByteBuffer exportData = GatherExportData();

      bool wrapData = checkExportToDataWrap.Checked;
      bool prefixRes = checkExportToDataIncludeRes.Checked;
      if ( !prefixRes )
      {
        prefix = "";
      }

      string line = Util.ToASMData( exportData, wrapData, wrapByteCount, prefix );
      editDataExport.Text = line;
    }



    private void checkExportToDataWrap_CheckedChanged( object sender, EventArgs e )
    {
      editWrapByteCount.Enabled = checkExportToDataWrap.Checked;
    }



    private void checkExportToDataIncludeRes_CheckedChanged( object sender, EventArgs e )
    {
      editPrefix.Enabled = checkExportToDataIncludeRes.Checked;
    }



    private void btnImportSprite_Click( object sender, EventArgs e )
    {
      string filename;

      if ( OpenFile( "Open sprite file", Types.Constants.FILEFILTER_SPRITE + Types.Constants.FILEFILTER_SPRITE_SPRITEPAD + Types.Constants.FILEFILTER_ALL, out filename ) )
      {
        ImportSprites( filename, true, true );
      }
    }



    private void btnPasteFromClipboard_Click( object sender, EventArgs e )
    {
      PasteFromClipboard();
      if ( m_ImportError.Length > 0 )
      {
        System.Windows.Forms.MessageBox.Show( m_ImportError, "Error while converting" );
      }
    }



    private void PasteFromClipboard()
    {
      m_ImportError = "";
      IDataObject dataObj = Clipboard.GetDataObject();
      if ( dataObj == null )
      {
        System.Windows.Forms.MessageBox.Show( "No image on clipboard" );
        return; 
      }

      var clipList = new ClipboardImageList();

      if ( clipList.GetFromClipboard() )
      {
        int pastePos = panelSprites.SelectedIndex;
        if ( pastePos == -1 )
        {
          pastePos = 0;
        }
        bool firstEntry = true;
        foreach ( var entry in clipList.Entries )
        {
          int indexGap =  entry.Index;
          pastePos += indexGap;

          if ( pastePos >= m_SpriteProject.Sprites.Count )
          {
            break;
          }

          DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoSpritesetSpriteChange( this, m_SpriteProject, pastePos ), firstEntry );
          firstEntry = false;

          var targetTile = m_SpriteProject.Sprites[pastePos].Tile;

          if ( ( ( entry.Tile.Mode == GraphicTileMode.COMMODORE_HIRES )
          ||     ( entry.Tile.Mode == GraphicTileMode.COMMODORE_MULTICOLOR ) )
          &&   ( ( targetTile.Mode == GraphicTileMode.COMMODORE_HIRES )
          ||     ( targetTile.Mode == GraphicTileMode.COMMODORE_MULTICOLOR ) ) )
          {
            // can copy mode
            targetTile.Mode = entry.Tile.Mode;

            m_SpriteProject.Sprites[pastePos].Mode = Lookup.SpriteModeFromTileMode( targetTile.Mode );
          }

          if ( Lookup.HaveCustomSpriteColor( m_SpriteProject.Mode ) )
          {
            targetTile.CustomColor = entry.Tile.CustomColor;
          }

          int copyWidth = Math.Min( m_SpriteWidth, entry.Tile.Width );
          int copyHeight = Math.Min( m_SpriteHeight, entry.Tile.Height );

          for ( int x = 0; x < copyWidth; ++x )
          {
            for ( int y = 0; y < copyHeight; ++y )
            {
              targetTile.SetPixel( x, y, entry.Tile.MapPixelColor( x, y, targetTile ) );
            }
          }

          RebuildSpriteImage( pastePos );
          panelSprites.InvalidateItemRect( pastePos );

          if ( pastePos == m_CurrentSprite )
          {
            _ColorSettingsDlg.CustomColor       = m_SpriteProject.Sprites[pastePos].Tile.CustomColor;
            _ColorSettingsDlg.MultiColorEnabled = ( m_SpriteProject.Sprites[pastePos].Tile.Mode == GraphicTileMode.COMMODORE_MULTICOLOR );
          }
        }
        pictureEditor.Invalidate();
        SetModified();
        return;
      }
      else if ( !Clipboard.ContainsImage() )
      {
        System.Windows.Forms.MessageBox.Show( "No image on clipboard" );
        return;
      }
      GR.Image.FastImage    imgClip = null;
      foreach ( string format in dataObj.GetFormats() )
      {
        if ( format == "DeviceIndependentBitmap" )
        {
          object dibData = dataObj.GetData( format );
          imgClip = GR.Image.FastImage.CreateImageFromHDIB( dibData );
          break;
        }
      }
      if ( imgClip == null )
      {
        System.Windows.Forms.MessageBox.Show( "No image on clipboard" );
        return;
      }
      GR.Image.FastImage mappedImage = null;

      var mcSettings = new ColorSettings();
      mcSettings.BackgroundColor  = m_SpriteProject.Colors.BackgroundColor;
      mcSettings.MultiColor1      = m_SpriteProject.Colors.MultiColor1;
      mcSettings.MultiColor2      = m_SpriteProject.Colors.MultiColor2;
      Debug.Log( "Replace with spriteproject palette!" );
      mcSettings.Palette          = Core.MainForm.ActivePalette;

      bool pasteAsBlock = false;
      if ( !Core.MainForm.ImportImage( "", imgClip, Types.GraphicType.SPRITES, mcSettings, out mappedImage, out mcSettings, out pasteAsBlock ) )
      {
        imgClip.Dispose();
        m_ImportError = "";
        return;
      }

      m_SpriteProject.Colors.BackgroundColor = mcSettings.BackgroundColor;
      m_SpriteProject.Colors.MultiColor1 = mcSettings.MultiColor1;
      m_SpriteProject.Colors.MultiColor2 = mcSettings.MultiColor2;

      ChangeColorSettingsDialog();

      int spritesY = ( mappedImage.Height + m_SpriteHeight - 1 ) / m_SpriteHeight;
      int spritesX = ( mappedImage.Width + m_SpriteWidth - 1 ) / m_SpriteWidth;
      int spritesPerLine = panelSprites.ItemsPerLine;
      int currentTargetSprite = m_CurrentSprite;

      for ( int j = 0; j < spritesY; ++j )
      {
        for ( int i = 0; i < spritesX; ++i )
        {
          if ( pasteAsBlock )
          {
            int localX = ( m_CurrentSprite % spritesPerLine ) + i;
            int localY = m_CurrentSprite / spritesPerLine + j;
            if ( localX >= spritesPerLine )
            {
              continue;
            }
            if ( localY * spritesPerLine >= 256 )
            {
              break;
            }
            currentTargetSprite = localX + localY * spritesPerLine;
          }

          int copyWidth = mappedImage.Width - i * m_SpriteWidth;
          if ( copyWidth > m_SpriteWidth )
          {
            copyWidth = m_SpriteWidth;
          }
          int copyHeight = mappedImage.Height - j * m_SpriteHeight;
          if ( copyHeight > m_SpriteHeight )
          {
            copyHeight = m_SpriteHeight;
          }

          DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoSpritesetSpriteChange( this, m_SpriteProject, currentTargetSprite ), ( i == 0 ) && ( j == 0 ) );

          GR.Image.FastImage imgSprite = mappedImage.GetImage( i * m_SpriteWidth, j * m_SpriteHeight, copyWidth, copyHeight ) as GR.Image.FastImage;
          ImportSprite( imgSprite, currentTargetSprite );
          imgSprite.Dispose();

          if ( currentTargetSprite == m_CurrentSprite )
          {
            CurrentSpriteModified();
            DoNotUpdateFromControls = true;

            _ColorSettingsDlg.CustomColor = m_SpriteProject.Sprites[currentTargetSprite].Tile.CustomColor;
            DoNotUpdateFromControls = false;
          }
          RebuildSpriteImage( currentTargetSprite );

          panelSprites.InvalidateItemRect( currentTargetSprite );

          if ( !pasteAsBlock )
          {
            ++currentTargetSprite;
          }
        }
      }
      mappedImage.Dispose();
      imgClip.Dispose();
      pictureEditor.Invalidate();
      Modified = true;
    }



    private void CopySpriteToClipboard()
    {
      // copy selected range/column (put custom data in clipboard)
      List<int>     selectedImages = panelSprites.SelectedIndices;
      if ( selectedImages.Count == 0 )
      {
        return;
      }

      var clipList = new ClipboardImageList();
      clipList.Mode         = Lookup.GraphicTileModeFromSpriteProjectMode( m_SpriteProject.Mode );
      clipList.Colors       = m_SpriteProject.Colors;
      clipList.ColumnBased  = panelSprites.IsSelectionColumnBased;

      foreach ( int index in selectedImages )
      {
        var entry = new ClipboardImageList.Entry();
        var sprite = m_SpriteProject.Sprites[index];

        entry.Tile = sprite.Tile;
        entry.Index = index;

        clipList.Entries.Add( entry );
      }
      clipList.CopyToClipboard();
    }



    private void btnCopyToClipboard_Click( object sender, EventArgs e )
    {
      CopySpriteToClipboard();
    }



    private void btnExportSpriteToImage_Click( object sender, EventArgs e )
    {
      List<int> exportIndices = GetExportIndices();
      if ( exportIndices.Count == 0 )
      {
        return;
      }

      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Export Sprites to Image";
      saveDlg.Filter = "PNG File|*.png";
      if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return;
      }

      int     neededWidth = exportIndices.Count * m_SpriteWidth;
      if ( neededWidth > 4 * m_SpriteWidth )
      {
        neededWidth = 4 * m_SpriteWidth;
      }
      int     neededHeight = ( exportIndices.Count + 3 ) / 4 + 1;
      neededHeight *= m_SpriteHeight;

      GR.Image.MemoryImage targetImg = new GR.Image.MemoryImage( neededWidth, neededHeight, m_SpriteProject.Sprites[0].Tile.Image.PixelFormat );

      PaletteManager.ApplyPalette( targetImg );
      for ( int i = 0; i < exportIndices.Count; ++i )
      {
        m_SpriteProject.Sprites[exportIndices[i]].Tile.Image.DrawTo( targetImg, ( i % 4 ) * m_SpriteWidth, ( i / 4 ) * m_SpriteHeight );
      }
      System.Drawing.Bitmap bmpTarget = targetImg.GetAsBitmap();
      bmpTarget.Save( saveDlg.FileName, System.Drawing.Imaging.ImageFormat.Png );
    }



    private bool ImportSprite( GR.Image.FastImage Image, int SpriteIndex )
    {
      m_ImportError = "";
      /*
      if ( ( Image.Width != m_SpriteWidth )
      ||   ( Image.Height != m_SpriteHeight )
      ||   ( Image.PixelFormat != System.Drawing.Imaging.PixelFormat.Format8bppIndexed ) )*/
      if ( Image.PixelFormat != System.Drawing.Imaging.PixelFormat.Format8bppIndexed )
      {
        // invalid format
        m_ImportError = "Invalid image format, must be 8 bit index";
        return false;
      }

      // Match image data
      GR.Memory.ByteBuffer Buffer = new GR.Memory.ByteBuffer( m_SpriteProject.Sprites[SpriteIndex].Tile.Data );

      int   ChosenSpriteColor = -1;

      SpriteMode insertMode = SpriteMode.COMMODORE_24_X_21_HIRES;

      if ( m_SpriteProject.Mode == SpriteProject.SpriteProjectMode.COMMODORE_24_X_21_HIRES_OR_MC )
      {
        // determine single/multi color
        bool[]  usedColor = new bool[16];
        int     numColors = 0;
        bool    hasSinglePixel = false;
        bool    usedBackgroundColor = false;

        for ( int y = 0; y < Image.Height; ++y )
        {
          for ( int x = 0; x < Image.Width; ++x )
          {
            int     colorIndex = (int)Image.GetPixelData( x, y );
            if ( colorIndex >= 16 )
            {
              m_ImportError = "Encountered color index >= 16 at " + x + "," + y;
              return false;
            }
            if ( ( x % 2 ) == 0 )
            {
              if ( colorIndex != (int)Image.GetPixelData( x + 1, y ) )
              {
                // not a double pixel, must be single color then
                hasSinglePixel = true;
              }
            }

            if ( !usedColor[colorIndex] )
            {
              if ( colorIndex == m_SpriteProject.Colors.BackgroundColor )
              {
                usedBackgroundColor = true;
              }
              usedColor[colorIndex] = true;
              numColors++;
            }
          }
        }
        if ( ( hasSinglePixel )
        && ( numColors > 2 ) )
        {
          m_ImportError = "Has a single pixel, but more than two colors";
          return false;
        }
        if ( ( hasSinglePixel )
        && ( numColors == 2 )
        && ( !usedBackgroundColor ) )
        {
          m_ImportError = "Looks like single color, but doesn't use the set background color";
          return false;
        }
        if ( ( !hasSinglePixel )
        && ( numColors > 4 ) )
        {
          m_ImportError = "Uses more than 4 colors";
          return false;
        }
        if ( ( !hasSinglePixel )
        && ( numColors == 4 )
        && ( !usedBackgroundColor ) )
        {
          m_ImportError = "Uses 4 colors, but doesn't use the set background color";
          return false;
        }
        if ( ( hasSinglePixel )
        || ( ( numColors == 2 )
        && ( usedBackgroundColor ) ) )
        {
          // eligible for single color
          int usedFreeColor = -1;
          for ( int i = 0; i < 16; ++i )
          {
            if ( usedColor[i] )
            {
              if ( i != m_SpriteProject.Colors.BackgroundColor )
              {
                if ( usedFreeColor != -1 )
                {
                  m_ImportError = "Uses more than one free color";
                  return false;
                }
                usedFreeColor = i;
              }
            }
          }

          for ( int y = 0; y < Image.Height; ++y )
          {
            for ( int x = 0; x < Image.Width; ++x )
            {
              int ColorIndex = (int)Image.GetPixelData( x, y );

              int BitPattern = 0;

              if ( ColorIndex != m_SpriteProject.Colors.BackgroundColor )
              {
                BitPattern = 1;
              }

              // noch nicht verwendete Farbe
              if ( BitPattern == 1 )
              {
                ChosenSpriteColor = ColorIndex;
              }
              byte byteMask = (byte)( 255 - ( 1 << ( ( 7 - ( x % 8 ) ) ) ) );
              Buffer.SetU8At( y * 3 + x / 8, (byte)( ( Buffer.ByteAt( y * 3 + x / 8 ) & byteMask ) | ( BitPattern << ( ( 7 - ( x % 8 ) ) ) ) ) );
            }
          }
        }
        else
        {
          // multi color
          insertMode = SpriteMode.COMMODORE_24_X_21_MULTICOLOR;
          int     usedMultiColors = 0;
          int     usedFreeColor = -1;
          for ( int i = 0; i < 16; ++i )
          {
            if ( usedColor[i] )
            {
              if ( ( i == m_SpriteProject.Colors.MultiColor1 )
              ||   ( i == m_SpriteProject.Colors.MultiColor2 )
              ||   ( i == m_SpriteProject.Colors.BackgroundColor ) )
              {
                ++usedMultiColors;
              }
              else
              {
                usedFreeColor = i;
              }
            }
          }
          if ( numColors - usedMultiColors > 1 )
          {
            // only one free color allowed
            m_ImportError = "Uses more than one free color";
            return false;
          }
          for ( int y = 0; y < Image.Height; ++y )
          {
            for ( int x = 0; x < Image.Width / 2; ++x )
            {
              int ColorIndex = (int)Image.GetPixelData( 2 * x, y );

              byte BitPattern = 0;

              if ( ColorIndex == m_SpriteProject.Colors.BackgroundColor )
              {
                BitPattern = 0x00;
              }
              else if ( ColorIndex == m_SpriteProject.Colors.MultiColor1 )
              {
                BitPattern = 0x01;
              }
              else if ( ColorIndex == m_SpriteProject.Colors.MultiColor2 )
              {
                BitPattern = 0x03;
              }
              else
              {
                // noch nicht verwendete Farbe
                ChosenSpriteColor = usedFreeColor;
                BitPattern = 0x02;
              }
              byte byteMask = (byte)( 255 - ( 3 << ( ( 3 - ( x % 4 ) ) * 2 ) ) );
              Buffer.SetU8At( y * 3 + x / 4, (byte)( ( Buffer.ByteAt( y * 3 + x / 4 ) & byteMask ) | ( BitPattern << ( ( 3 - ( x % 4 ) ) * 2 ) ) ) );
            }
          }
        }
        for ( int i = 0; i < Buffer.Length; ++i )
        {
          m_SpriteProject.Sprites[SpriteIndex].Tile.Data.SetU8At( i, Buffer.ByteAt( i ) );
        }
      }
      else
      {
        insertMode = SpriteMode.MEGA65_16_X_21_16_COLORS;

        for ( int y = 0; y < Image.Height; ++y )
        {
          for ( int x = 0; x < Image.Width; ++x )
          {
            int     colorIndex = (int)Image.GetPixelData( x, y );
            if ( colorIndex >= 16 )
            {
              m_ImportError = "Encountered color index >= 16 at " + x + "," + y;
              return false;
            }
            m_SpriteProject.Sprites[SpriteIndex].Tile.SetPixel( x, y, colorIndex );
          }
        }
      }
      m_SpriteProject.Sprites[SpriteIndex].Tile.CustomColor = ChosenSpriteColor;
      m_SpriteProject.Sprites[SpriteIndex].Mode = insertMode;
      RebuildSpriteImage( SpriteIndex );
      return true;
    }



    private void btnImportFromImage_Click( object sender, EventArgs e )
    {
      string filename;

      if ( !OpenFile( "Import Sprites from Image", C64Studio.Types.Constants.FILEFILTER_IMAGE_FILES, out filename ) )
      {
        return;
      }

      GR.Image.FastImage spriteImage;

      var mcSettings = new ColorSettings();
      mcSettings.BackgroundColor  = m_SpriteProject.Colors.BackgroundColor;
      mcSettings.MultiColor1      = m_SpriteProject.Colors.MultiColor1;
      mcSettings.MultiColor2      = m_SpriteProject.Colors.MultiColor2;
      Debug.Log( "Replace with spriteproject palette!" );
      mcSettings.Palette          = Core.MainForm.ActivePalette;

      bool pasteAsBlock = false;
      if ( !Core.MainForm.ImportImage( filename, null, C64Studio.Types.GraphicType.SPRITES, mcSettings, out spriteImage, out mcSettings, out pasteAsBlock ) )
      {
        return;
      }

      m_SpriteProject.Colors.BackgroundColor = mcSettings.BackgroundColor;
      m_SpriteProject.Colors.MultiColor1 = mcSettings.MultiColor1;
      m_SpriteProject.Colors.MultiColor2 = mcSettings.MultiColor2;

      ChangeColorSettingsDialog();

      int   currentSpriteIndex = 0;
      int   curX = 0;
      int   curY = 0;
      while ( curY + m_SpriteHeight <= spriteImage.Height )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoSpritesetSpriteChange( this, m_SpriteProject, currentSpriteIndex ), currentSpriteIndex == 0 );

        ImportSprite( spriteImage.GetImage( curX, curY, m_SpriteWidth, m_SpriteHeight ) as GR.Image.FastImage, currentSpriteIndex );

        ++currentSpriteIndex;
        curX += m_SpriteWidth;
        if ( curX >= spriteImage.Width )
        {
          curX = 0;
          curY += m_SpriteHeight;
        }
      }
      panelSprites.Invalidate();
      pictureEditor.Invalidate();
      Modified = true;
    }



    private void btnShiftLeft_Click( object sender, EventArgs e )
    {
      ShiftLeft();
    }



    private void ShiftLeft()
    {
      var selectedSprites = panelSprites.SelectedIndices;

      bool firstEntry = true;
      foreach ( var spriteIndex in selectedSprites )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoSpritesetSpriteChange( this, m_SpriteProject, spriteIndex ), firstEntry );
        firstEntry = false;
        var sprite = m_SpriteProject.Sprites[spriteIndex];

        for ( int y = 0; y < m_SpriteHeight; ++y )
        {
          int     tempColor = sprite.Tile.GetPixel( 0, y );
          for ( int x = 0; x < m_SpriteWidth - 1; ++x )
          {

            sprite.Tile.SetPixel( x, y, sprite.Tile.GetPixel( x + 1, y ) );
          }
          sprite.Tile.SetPixel( m_SpriteWidth - 1, y, tempColor );
        }
        RebuildSpriteImage( spriteIndex );
        panelSprites.InvalidateItemRect( spriteIndex );
      }
      pictureEditor.Invalidate();
      Modified = true;
    }



    private void btnShiftRight_Click( object sender, EventArgs e )
    {
      ShiftRight();
    }



    private void ShiftRight()
    {
      var selectedSprites = panelSprites.SelectedIndices;

      bool firstEntry = true;
      foreach ( var spriteIndex in selectedSprites )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoSpritesetSpriteChange( this, m_SpriteProject, spriteIndex ), firstEntry );
        firstEntry = false;
        var sprite = m_SpriteProject.Sprites[spriteIndex];

        for ( int y = 0; y < m_SpriteHeight; ++y )
        {
          int     tempColor = sprite.Tile.GetPixel( m_SpriteWidth - 1, y );
          for ( int x = 0; x < m_SpriteWidth - 1; ++x )
          {

            sprite.Tile.SetPixel( m_SpriteWidth - 1 - x, y, sprite.Tile.GetPixel( m_SpriteWidth - x - 2, y ) );
          }
          sprite.Tile.SetPixel( 0, y, tempColor );
        }
        RebuildSpriteImage( spriteIndex );
        panelSprites.InvalidateItemRect( spriteIndex );
      }
      pictureEditor.Invalidate();
      Modified = true;
    }



    private void btnShiftUp_Click( object sender, EventArgs e )
    {
      ShiftUp();
    }



    private void ShiftUp()
    {
      var selectedSprites = panelSprites.SelectedIndices;

      bool firstEntry = true;
      foreach ( var spriteIndex in selectedSprites )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoSpritesetSpriteChange( this, m_SpriteProject, spriteIndex ), firstEntry );
        firstEntry = false;
        var sprite = m_SpriteProject.Sprites[spriteIndex];

        for ( int x = 0; x < m_SpriteWidth; x += Lookup.PixelWidth( sprite.Tile.Mode ) )
        {
          int   tempPixel = sprite.Tile.GetPixel( x, 0 );
          for ( int y = 0; y < m_SpriteHeight - 1; ++y )
          {
            sprite.Tile.SetPixel( x, y, sprite.Tile.GetPixel( x, y + 1 ) );
          }
          sprite.Tile.SetPixel( x, m_SpriteHeight - 1, tempPixel );
        }

        RebuildSpriteImage( spriteIndex );
        panelSprites.InvalidateItemRect( spriteIndex );
      }
      pictureEditor.Invalidate();
      Modified = true;
    }



    private void btnShiftDown_Click( object sender, EventArgs e )
    {
      ShiftDown();
    }



    private void ShiftDown()
    {
      var selectedSprites = panelSprites.SelectedIndices;

      bool firstEntry = true;
      foreach ( var spriteIndex in selectedSprites )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoSpritesetSpriteChange( this, m_SpriteProject, spriteIndex ), firstEntry );
        firstEntry = false;
        var sprite = m_SpriteProject.Sprites[spriteIndex];

        for ( int x = 0; x < m_SpriteWidth; x += Lookup.PixelWidth( sprite.Tile.Mode ) )
        {
          int   tempPixel = sprite.Tile.GetPixel( x, m_SpriteHeight - 1 );
          for ( int y = 0; y < m_SpriteHeight - 1; ++y )
          {
            sprite.Tile.SetPixel( x, m_SpriteHeight - 1 - y, sprite.Tile.GetPixel( x, m_SpriteHeight - 1 - y - 1 ) );
          }
          sprite.Tile.SetPixel( x, 0, tempPixel );
        }

        RebuildSpriteImage( spriteIndex );
        panelSprites.InvalidateItemRect( spriteIndex );
      }
      pictureEditor.Invalidate();
      Modified = true;
    }



    private void btnMirrorX_Click( object sender, EventArgs e )
    {
      MirrorX();
    }



    private void btnMirrorY_Click( object sender, EventArgs e )
    {
      MirrorY();
    }



    void pictureEditor_PostPaint( GR.Image.FastImage TargetBuffer )
    {
      if ( m_SpriteProject.ShowGrid )
      {
        if ( m_SpriteProject.Sprites[m_CurrentSprite].Mode == SpriteMode.COMMODORE_24_X_21_MULTICOLOR )
        {
          for ( int i = 0; i < m_SpriteWidth / 2; ++i )
          {
            for ( int j = 0; j < TargetBuffer.Height; ++j )
            {
              TargetBuffer.SetPixel( ( i * pictureEditor.ClientRectangle.Width / ( m_SpriteWidth / 2 ) ), j, 0xffffffff );
            }
          }
          for ( int i = 0; i < m_SpriteHeight; ++i )
          {
            for ( int j = 0; j < TargetBuffer.Width; ++j )
            {
              TargetBuffer.SetPixel( j, ( i * pictureEditor.ClientRectangle.Height / m_SpriteHeight ), 0xffffffff );
            }
          }
        }
        else
        {
          for ( int i = 0; i < m_SpriteWidth; ++i )
          {
            for ( int j = 0; j < TargetBuffer.Height; ++j )
            {
              TargetBuffer.SetPixel( i * pictureEditor.ClientRectangle.Width / m_SpriteWidth, j, 0xffffffff );
            }
          }
          for ( int i = 0; i < m_SpriteHeight; ++i )
          {
            for ( int j = 0; j < TargetBuffer.Width; ++j )
            {
              TargetBuffer.SetPixel( j, ( i * pictureEditor.ClientRectangle.Height / m_SpriteHeight ), 0xffffffff );
            }
          }
        }
      }
    }



    private void checkShowGrid_CheckedChanged( object sender, EventArgs e )
    {
      m_SpriteProject.ShowGrid = checkShowGrid.Checked;
      pictureEditor.Invalidate();
    }



    private void btnInvert_Click( object sender, EventArgs e )
    {
      Invert();
    }



    private void Invert()
    {
      var selectedSprites = panelSprites.SelectedIndices;

      bool firstEntry = true;
      foreach ( var spriteIndex in selectedSprites )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoSpritesetSpriteChange( this, m_SpriteProject, spriteIndex ), firstEntry );
        firstEntry = false;
        var sprite = m_SpriteProject.Sprites[spriteIndex];

        for ( int i = 0; i < sprite.Tile.Data.Length; ++i )
        {
          byte value = (byte)( ~sprite.Tile.Data.ByteAt( i ) );
          sprite.Tile.Data.SetU8At( i, value );
        }
        RebuildSpriteImage( spriteIndex );
        panelSprites.InvalidateItemRect( spriteIndex );
      }
      pictureEditor.Invalidate();
      Modified = true;
    }



    private void btnRotateLeft_Click( object sender, EventArgs e )
    {
      RotateLeft();
    }



    private void RotateLeft()
    {
      var selectedSprites = panelSprites.SelectedIndices;

      bool firstEntry = true;
      foreach ( var spriteIndex in selectedSprites )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoSpritesetSpriteChange( this, m_SpriteProject, spriteIndex ), firstEntry );
        firstEntry = false;
        var sprite = m_SpriteProject.Sprites[spriteIndex];

        int     side = Math.Min( m_SpriteHeight, m_SpriteWidth );

        var resultTile = new GraphicTile( sprite.Tile );

        for ( int i = 0; i < side; ++i )
        {
          for ( int j = 0; j < side; ++j )
          {
            int sourceX = i;
            int sourceY = j;
            int targetX = j;
            int targetY = side - 1 - i;

            int   sourceColor = sprite.Tile.GetPixel( sourceX, sourceY );
            resultTile.SetPixel( targetX, targetY, sourceColor );
          }
        }
        sprite.Tile.Data = resultTile.Data;
        SpriteChanged( spriteIndex );
      }
      pictureEditor.Invalidate();
      Modified = true;
    }



    private void btnRotateRight_Click( object sender, EventArgs e )
    {
      RotateRight();
    }



    private void RotateRight()
    {
      var selectedSprites = panelSprites.SelectedIndices;

      bool firstEntry = true;
      foreach ( var spriteIndex in selectedSprites )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoSpritesetSpriteChange( this, m_SpriteProject, spriteIndex ), firstEntry );
        firstEntry = false;
        var sprite = m_SpriteProject.Sprites[spriteIndex];

        int     side = Math.Min( m_SpriteHeight, m_SpriteWidth );

        var resultTile = new GraphicTile( sprite.Tile );

        for ( int i = 0; i < side; ++i )
        {
          for ( int j = 0; j < side; ++j )
          {
            int sourceX = i;
            int sourceY = j;
            int targetX = side - 1 - j;
            int targetY = i;

            int   sourceColor = sprite.Tile.GetPixel( sourceX, sourceY );
            resultTile.SetPixel( targetX, targetY, sourceColor );
          }
        }
        sprite.Tile.Data = resultTile.Data;
        SpriteChanged( spriteIndex );
      }
      pictureEditor.Invalidate();
      Modified = true;
    }



    private void comboSprite_DrawItem( object sender, DrawItemEventArgs e )
    {
      e.DrawBackground();
      if ( e.Index == -1 )
      {
        return;
      }
      System.Drawing.RectangleF textRect = new System.Drawing.RectangleF( e.Bounds.Left, e.Bounds.Top + 10, e.Bounds.Width, e.Bounds.Height );
      System.Drawing.Brush textBrush = new System.Drawing.SolidBrush( e.ForeColor );
      e.Graphics.DrawString( e.Index.ToString(), e.Font, textBrush, textRect );

      GR.Image.FastImage    fastImage = new GR.Image.FastImage( m_SpriteWidth, m_SpriteHeight, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
      GR.Image.MemoryImage  memImage = new GR.Image.MemoryImage( m_SpriteWidth, m_SpriteHeight, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );

      PaletteManager.ApplyPalette( fastImage );
      PaletteManager.ApplyPalette( memImage );

      DrawSpriteImage( memImage, 0, 0, 
                       m_SpriteProject.Sprites[e.Index].Tile.Data,
                       m_SpriteProject.Sprites[e.Index].Tile.Width, m_SpriteProject.Sprites[e.Index].Tile.Height,
                       comboLayerColor.SelectedIndex,
                       m_SpriteProject.Sprites[e.Index].Mode,
                       m_SpriteProject.Colors.BackgroundColor, 
                       m_SpriteProject.Colors.MultiColor1, m_SpriteProject.Colors.MultiColor2,
                       false, false );
      fastImage.DrawImage( memImage, 0, 0 );
      System.Drawing.Rectangle drawRect = new System.Drawing.Rectangle( e.Bounds.Location, e.Bounds.Size );
      drawRect.X += 20;
      drawRect.Width = 48;
      drawRect.Height = 42;
      fastImage.DrawToHDC( e.Graphics.GetHdc(), drawRect );
      e.Graphics.ReleaseHdc();
      fastImage.Dispose();
    }



    private void comboLayerColor_SelectedIndexChanged( object sender, EventArgs e )
    {
      comboSprite.Invalidate();

      if ( listLayerSprites.SelectedIndices.Count > 0 )
      {
        Formats.SpriteProject.LayerSprite sprite = (Formats.SpriteProject.LayerSprite)listLayerSprites.SelectedItems[0].Tag;

        if ( sprite.Color != comboLayerColor.SelectedIndex )
        {
          sprite.Color = comboLayerColor.SelectedIndex;
          RedrawPreviewLayer();
          SetModified();
        }
      }
    }



    private void btnAdd_Click( object sender, EventArgs e )
    {
    }



    private void RedrawPreviewLayer()
    {
      if ( m_CurrentLayer != null )
      {
        layerPreview.DisplayPage.Box( 0, 0, layerPreview.Width, layerPreview.Height, (uint)m_CurrentLayer.BackgroundColor );
        foreach ( Formats.SpriteProject.LayerSprite sprite in m_CurrentLayer.Sprites )
        {
          DrawSpriteImage( layerPreview.DisplayPage, 
                           sprite.X, sprite.Y, 
                           m_SpriteProject.Sprites[sprite.Index].Tile.Data,
                           m_SpriteProject.Sprites[sprite.Index].Tile.Width, m_SpriteProject.Sprites[sprite.Index].Tile.Height,
                           sprite.Color, 
                           m_SpriteProject.Sprites[sprite.Index].Mode,
                           m_SpriteProject.Colors.BackgroundColor,
                           m_SpriteProject.Colors.MultiColor1, 
                           m_SpriteProject.Colors.MultiColor2,
                           sprite.ExpandX, sprite.ExpandY );
        }
      }
      else
      {
        layerPreview.DisplayPage.Box( 0, 0, layerPreview.Width, layerPreview.Height, 0 );
      }
      layerPreview.Invalidate();
    }



    private void btnDelete_Click( object sender, EventArgs e )
    {
      if ( listLayerSprites.SelectedIndices.Count == 0 )
      {
        return;
      }
      Formats.SpriteProject.LayerSprite sprite = (Formats.SpriteProject.LayerSprite)listLayerSprites.SelectedItems[0].Tag;

      m_CurrentLayer.Sprites.Remove( sprite );
      listLayerSprites.Items.Remove( listLayerSprites.SelectedItems[0] );
      RedrawPreviewLayer();
      SetModified();
    }



    private void btnUp_Click( object sender, EventArgs e )
    {
      if ( ( listLayerSprites.SelectedIndices.Count > 0 )
      &&   ( listLayerSprites.SelectedIndices[0] > 0 ) )
      {
        int insertIndex = listLayerSprites.SelectedIndices[0] - 1;

        Formats.SpriteProject.LayerSprite sprite1 = (Formats.SpriteProject.LayerSprite)listLayerSprites.SelectedItems[0].Tag;
        Formats.SpriteProject.LayerSprite sprite2 = (Formats.SpriteProject.LayerSprite)listLayerSprites.Items[insertIndex].Tag;
        
        m_CurrentLayer.Sprites.Remove( sprite1 );
        m_CurrentLayer.Sprites.Insert( insertIndex, sprite1 );

        ArrangedItemEntry itemToSwap = listLayerSprites.SelectedItems[0];
        listLayerSprites.Items.Remove( itemToSwap );
        listLayerSprites.Items.Insert( insertIndex, itemToSwap );
        itemToSwap.Selected = true;
        RedrawPreviewLayer();
        SetModified();
      }
    }



    private void btnDown_Click( object sender, EventArgs e )
    {
      if ( ( listLayerSprites.SelectedIndices.Count > 0 )
      &&   ( listLayerSprites.SelectedIndices[0] + 1 < listLayerSprites.Items.Count ) )
      {
        int insertIndex = listLayerSprites.SelectedIndices[0] + 1;

        Formats.SpriteProject.LayerSprite sprite1 = (Formats.SpriteProject.LayerSprite)listLayerSprites.SelectedItems[0].Tag;
        Formats.SpriteProject.LayerSprite sprite2 = (Formats.SpriteProject.LayerSprite)listLayerSprites.Items[insertIndex].Tag;

        m_CurrentLayer.Sprites.Remove( sprite1 );
        m_CurrentLayer.Sprites.Insert( insertIndex, sprite1 );

        ArrangedItemEntry itemToSwap = listLayerSprites.SelectedItems[0];
        listLayerSprites.Items.Remove( itemToSwap );
        listLayerSprites.Items.Insert( insertIndex, itemToSwap );
        itemToSwap.Selected = true;
        RedrawPreviewLayer();
        SetModified();
      }

    }



    private void comboSprite_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( listLayerSprites.SelectedIndices.Count > 0 )
      {
        Formats.SpriteProject.LayerSprite sprite = (Formats.SpriteProject.LayerSprite)listLayerSprites.SelectedItems[0].Tag;

        if ( sprite.Index != comboSprite.SelectedIndex )
        {
          sprite.Index = comboSprite.SelectedIndex;
          listLayerSprites.SelectedItems[0].Text = sprite.Index.ToString() + ", " + sprite.X.ToString() + ", " + sprite.Y.ToString();
          RedrawPreviewLayer();
          SetModified();
        }
      }
    }



    private void editLayerX_TextChanged( object sender, EventArgs e )
    {
      if ( listLayerSprites.SelectedIndices.Count > 0 )
      {
        Formats.SpriteProject.LayerSprite sprite = (Formats.SpriteProject.LayerSprite)listLayerSprites.SelectedItems[0].Tag;

        int newPos = GR.Convert.ToI32( editLayerX.Text );
        if ( sprite.X != newPos )
        {
          sprite.X = newPos;
          listLayerSprites.SelectedItems[0].Text = sprite.Index.ToString() + ", " + sprite.X.ToString() + ", " + sprite.Y.ToString();
          RedrawPreviewLayer();
          SetModified();
        }
      }
    }



    private void editLayerY_TextChanged( object sender, EventArgs e )
    {
      if ( listLayerSprites.SelectedIndices.Count > 0 )
      {
        Formats.SpriteProject.LayerSprite sprite = (Formats.SpriteProject.LayerSprite)listLayerSprites.SelectedItems[0].Tag;

        int newPos = GR.Convert.ToI32( editLayerY.Text );
        if ( sprite.Y != newPos )
        {
          sprite.Y = newPos;
          listLayerSprites.SelectedItems[0].Text = sprite.Index.ToString() + ", " + sprite.X.ToString() + ", " + sprite.Y.ToString();
          RedrawPreviewLayer();
          SetModified();
        }
      }
    }



    private void comboLayerBGColor_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_CurrentLayer == null )
      {
        return;
      }

      if ( comboLayerBGColor.SelectedIndex != m_CurrentLayer.BackgroundColor )
      {
        m_CurrentLayer.BackgroundColor = comboLayerBGColor.SelectedIndex;
        RedrawPreviewLayer();
        SetModified();
      }
    }



    private void btnDeleteSprite_Click( object sender, EventArgs e )
    {
      if ( panelSprites.SelectedIndex == -1 )
      {
        return;
      }

      for ( int i = 0; i < m_SpriteProject.Sprites.Count; ++i )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoSpritesetSpriteChange( this, m_SpriteProject, i ), i == 0 );
      }

      List<int>     selectedSprites = panelSprites.SelectedIndices;

      int     firstSelectedIndex = 0;
      if ( selectedSprites.Count > 0 )
      {
        firstSelectedIndex = selectedSprites[0];
      }

      selectedSprites.Reverse();

      foreach ( var index in selectedSprites )
      {
        int indexToRemove = index;

        m_SpriteProject.Sprites.RemoveAt( indexToRemove );
        panelSprites.Items.RemoveAt( indexToRemove );

        // add empty sprite in back
        m_SpriteProject.Sprites.Add( new SpriteProject.SpriteData( m_SpriteProject.Colors ) );
        panelSprites.Items.Add( ( m_SpriteProject.Sprites.Count - 1 ).ToString(), m_SpriteProject.Sprites[m_SpriteProject.Sprites.Count - 1].Tile.Image );
      }
      if ( firstSelectedIndex < panelSprites.Items.Count )
      {
        panelSprites.SelectedIndex = firstSelectedIndex;
      }
      else
      {
        panelSprites.SelectedIndex = 0;
      }
      panelSprites_SelectedIndexChanged( sender, e );

      SetModified();
    }



    private void editDataExport_KeyPress( object sender, KeyPressEventArgs e )
    {
      if ( ( System.Windows.Forms.Control.ModifierKeys == Keys.Control )
      &&   ( e.KeyChar == 1 ) )
      {
        editDataExport.SelectAll();
        e.Handled = true;
      }
    }



    private ArrangedItemEntry listLayerSprites_AddingItem( object sender )
    {
      Formats.SpriteProject.LayerSprite sprite = new Formats.SpriteProject.LayerSprite();
      sprite.X        = GR.Convert.ToI32( editLayerX.Text );
      sprite.Y        = GR.Convert.ToI32( editLayerY.Text );
      sprite.Index    = comboSprite.SelectedIndex;
      sprite.Color    = comboLayerColor.SelectedIndex;
      sprite.ExpandX  = checkExpandX.Checked;
      sprite.ExpandY  = checkExpandY.Checked;

      m_CurrentLayer.Sprites.Add( sprite );

      ArrangedItemEntry item = new ArrangedItemEntry();
      item.Text = sprite.Index.ToString() + ", " + sprite.X.ToString() + ", " + sprite.Y.ToString();
      item.Tag = sprite;

      return item;
    }



    private void listLayerSprites_ItemMoved( object sender, ArrangedItemEntry Item, ArrangedItemEntry OtherItem )
    {
      m_CurrentLayer.Sprites.Clear();
      foreach ( ArrangedItemEntry item in listLayerSprites.Items )
      {
        var sprite = (Formats.SpriteProject.LayerSprite)item.Tag;

        m_CurrentLayer.Sprites.Add( sprite );
      }
      SetModified();
      RedrawPreviewLayer();
    }



    private void listLayerSprites_ItemRemoved( object sender, ArrangedItemEntry Item )
    {
      m_CurrentLayer.Sprites.Clear();
      foreach ( ArrangedItemEntry item in listLayerSprites.Items )
      {
        var sprite = (Formats.SpriteProject.LayerSprite)item.Tag;

        m_CurrentLayer.Sprites.Add( sprite );
      }
      SetModified();
      RedrawPreviewLayer();
    }



    private void listLayerSprites_SelectedIndexChanged( object sender, ArrangedItemEntry Item )
    {
      if ( Item != null )
      {
        Formats.SpriteProject.LayerSprite sprite = (Formats.SpriteProject.LayerSprite)Item.Tag;
        editLayerX.Text               = sprite.X.ToString();
        editLayerY.Text               = sprite.Y.ToString();
        comboLayerColor.SelectedIndex = sprite.Color;
        comboSprite.SelectedIndex     = sprite.Index;
        checkExpandX.Checked          = sprite.ExpandX;
        checkExpandY.Checked          = sprite.ExpandY;
      }
    }



    private void listLayers_ItemAdded( object sender, ArrangedItemEntry Item )
    {
      m_CurrentLayer = (Formats.SpriteProject.Layer)Item.Tag;
      SetModified();
      RedrawPreviewLayer();
    }



    private void listLayerSprites_ItemAdded( object sender, ArrangedItemEntry Item )
    {
      var sprite = (Formats.SpriteProject.LayerSprite)Item.Tag;
      SetModified();
      RedrawPreviewLayer();
    }



    private ArrangedItemEntry listLayers_AddingItem( object sender )
    {
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoSpritesetAddLayer( this, m_SpriteProject, m_SpriteProject.SpriteLayers.Count ) );

      Formats.SpriteProject.Layer   layer = new C64Studio.Formats.SpriteProject.Layer();

      layer.Name = editLayerName.Text;

      var item = new ArrangedItemEntry( editLayerName.Text );
      item.Tag = layer;

      m_SpriteProject.SpriteLayers.Add( layer );

      return item;
    }



    private void listLayers_SelectedIndexChanged( object sender, ArrangedItemEntry Item )
    {
      listLayerSprites.Items.Clear();
      if ( Item == null )
      {
        m_CurrentLayer = null;
        editLayerName.Text = "";
        editLayerDelay.Text = "";
        RedrawPreviewLayer();
        return;
      }
      m_CurrentLayer = (Formats.SpriteProject.Layer)Item.Tag;
      if ( editLayerName.Text != m_CurrentLayer.Name )
      {
        editLayerName.Text = m_CurrentLayer.Name;
      }
      string  delayText = m_CurrentLayer.DelayMS.ToString();
      if ( editLayerDelay.Text != delayText )
      {
        editLayerDelay.Text = delayText;
      }

      comboLayerBGColor.SelectedIndex = m_CurrentLayer.BackgroundColor;

      int   spriteIndex = 0;
      foreach ( var sprite in m_CurrentLayer.Sprites )
      {
        var item = new ArrangedItemEntry();

        item.Text = sprite.Index.ToString() + ", " + sprite.X.ToString() + ", " + sprite.Y.ToString();
        item.Tag = sprite;
        listLayerSprites.Items.Add( item );

        ++spriteIndex;
      }
      RedrawPreviewLayer();
    }



    private void listLayers_ItemRemoved( object sender, ArrangedItemEntry Item )
    {
      m_SpriteProject.SpriteLayers.Clear();
      foreach ( ArrangedItemEntry item in listLayers.Items )
      {
        var layer = (Formats.SpriteProject.Layer)item.Tag;

        m_SpriteProject.SpriteLayers.Add( layer );
      }
      SetModified();
      if ( m_CurrentLayer == (Formats.SpriteProject.Layer)Item.Tag )
      {
        m_CurrentLayer = null;
        RedrawPreviewLayer();
      }
    }



    private void editLayerName_TextChanged( object sender, EventArgs e )
    {
      if ( m_CurrentLayer != null )
      {
        if ( m_CurrentLayer.Name != editLayerName.Text )
        {
          m_CurrentLayer.Name = editLayerName.Text;
          listLayers.SelectedItems[0].Text = m_CurrentLayer.Name;
        }
      }
    }



    private void listLayers_ItemMoved( object sender, ArrangedItemEntry Item, ArrangedItemEntry OtherItem )
    {
      m_SpriteProject.SpriteLayers.Clear();
      foreach ( ArrangedItemEntry item in listLayers.Items )
      {
        var layer = (Formats.SpriteProject.Layer)item.Tag;

        m_SpriteProject.SpriteLayers.Add( layer );
      }
      SetModified();
    }



    private void comboExportRange_SelectedIndexChanged( object sender, EventArgs e )
    {
      labelCharactersFrom.Enabled = ( comboExportRange.SelectedIndex == 2 );
      editSpriteFrom.Enabled = ( comboExportRange.SelectedIndex == 2 );
      labelCharactersTo.Enabled = ( comboExportRange.SelectedIndex == 2 );
      editSpriteCount.Enabled = ( comboExportRange.SelectedIndex == 2 );
    }



    private void editSpriteFrom_TextChanged( object sender, EventArgs e )
    {
      int   newStart = GR.Convert.ToI32( editSpriteFrom.Text );

      if ( m_SpriteProject.ExportStartIndex != newStart )
      {
        m_SpriteProject.ExportStartIndex = newStart;
        SetModified();
      }
    }



    private void editSpriteCount_TextChanged( object sender, EventArgs e )
    {
      int   newCount = GR.Convert.ToI32( editSpriteCount.Text );

      if ( m_SpriteProject.ExportSpriteCount != newCount )
      {
        m_SpriteProject.ExportSpriteCount = newCount;
        SetModified();
      }
    }



    public void SpriteChanged( int SpriteIndex )
    {
      RebuildSpriteImage( SpriteIndex );
      panelSprites.Items[SpriteIndex].MemoryImage = m_SpriteProject.Sprites[SpriteIndex].Tile.Image;
      if ( m_CurrentSprite == SpriteIndex )
      {
        pictureEditor.Image = m_SpriteProject.Sprites[SpriteIndex].Tile.Image;
        pictureEditor.Invalidate();
      }
      panelSprites.InvalidateItemRect( SpriteIndex );
      if ( m_CurrentSprite == SpriteIndex )
      {
        // can only do those once a list of undo steps has been completely finished undoing
        DoNotUpdateFromControls = true;

        if ( ( m_SpriteProject.Sprites[SpriteIndex].Mode == SpriteMode.COMMODORE_24_X_21_MULTICOLOR ) != _ColorSettingsDlg.MultiColorEnabled )
        {
          _ColorSettingsDlg.MultiColorEnabled = ( m_SpriteProject.Sprites[SpriteIndex].Mode == SpriteMode.COMMODORE_24_X_21_MULTICOLOR );
        }
        if ( Lookup.HaveCustomSpriteColor( m_SpriteProject.Mode ) )
        {
          if ( m_SpriteProject.Sprites[SpriteIndex].Tile.CustomColor != _ColorSettingsDlg.CustomColor )
          {
            _ColorSettingsDlg.CustomColor = m_SpriteProject.Sprites[SpriteIndex].Tile.CustomColor;
          }
        }
        DoNotUpdateFromControls = false;
      }
      if ( comboSprite.SelectedIndex == SpriteIndex )
      {
        comboSprite.Invalidate();

        if ( ( m_CurrentLayer != null )
        &&   ( m_CurrentLayer.Sprites.Any( s => s.Index == SpriteIndex ) ) )
        {
          RedrawPreviewLayer();
        }
      }
      CurrentSpriteModified();
      SetModified();
    }



    public void ColorsChanged()
    {
      comboSpriteProjectMode.SelectedIndex = (int)m_SpriteProject.Mode;
      AdjustSpriteSizes();
      ChangeColorSettingsDialog();
      OnPaletteChanged();

      for ( int i = 0; i < m_SpriteProject.TotalNumberOfSprites; ++i )
      {
        RebuildSpriteImage( i );
      }
      pictureEditor.Invalidate();
      panelSprites.Invalidate();

      SetModified();
    }



    public void LayersChanged()
    {
      int   currentLayer = m_SpriteProject.SpriteLayers.IndexOf( m_CurrentLayer );

      listLayerSprites.Items.Clear();
      listLayers.Items.Clear();

      AddAllLayers();
      RedrawPreviewLayer();
    }



    private bool listLayerSprites_MovingItem( object sender, ArrangedItemEntry Item1, ArrangedItemEntry Item2 )
    {
      return true;
    }



    private bool listLayers_MovingItem( object sender, ArrangedItemEntry Item1, ArrangedItemEntry Item2 )
    {
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoSpritesetExchangeLayer( this, m_SpriteProject, Item1.Index, Item2.Index ) );

      return true;
    }



    private void checkExpandX_CheckedChanged( object sender, EventArgs e )
    {
      if ( listLayerSprites.SelectedIndices.Count > 0 )
      {
        Formats.SpriteProject.LayerSprite sprite = (Formats.SpriteProject.LayerSprite)listLayerSprites.SelectedItems[0].Tag;

        if ( sprite.ExpandX != checkExpandX.Checked )
        {
          sprite.ExpandX = checkExpandX.Checked;
          Modified = true;
          RedrawPreviewLayer();
          SetModified();
        }
      }
    }



    private void checkExpandY_CheckedChanged( object sender, EventArgs e )
    {
      if ( listLayerSprites.SelectedIndices.Count > 0 )
      {
        Formats.SpriteProject.LayerSprite sprite = (Formats.SpriteProject.LayerSprite)listLayerSprites.SelectedItems[0].Tag;

        if ( sprite.ExpandY != checkExpandY.Checked )
        {
          sprite.ExpandY = checkExpandY.Checked;
          Modified = true;
          RedrawPreviewLayer();
          SetModified();
        }
      }
    }



    private void editLayerX_KeyPress( object sender, KeyPressEventArgs e )
    {
      if ( ( char.IsDigit( e.KeyChar ) )
      ||   ( (Keys)e.KeyChar == Keys.Back )
      ||   ( ( e.KeyChar == '-' )
      &&     ( editLayerX.SelectionStart == 0 ) ) )
      {
        // ok
      }
      else
      {
        e.Handled = true;
      }
    }



    private void editLayerY_KeyPress( object sender, KeyPressEventArgs e )
    {
      if ( ( char.IsDigit( e.KeyChar ) )
      ||   ( (Keys)e.KeyChar == Keys.Back )
      ||   ( ( e.KeyChar == '-' )
      &&     ( editLayerY.SelectionStart == 0 ) ) )
      {
        // ok
      }
      else
      {
        e.Handled = true;
      }

    }



    private void btnImportFromHex_Click( object sender, EventArgs e )
    {
      string    binaryText = editDataImport.Text.Replace( " ", "" ).Replace( "\r", "" ).Replace( "\n", "" );

      GR.Memory.ByteBuffer    data = new GR.Memory.ByteBuffer( binaryText );

      int numSprites = ( (int)data.Length + 63 ) / 64;
      for ( int i = 0; i < numSprites; ++i )
      {
        var tempBuffer = new GR.Memory.ByteBuffer( 64 );

        int     numBytesToCopy = 64;
        if ( i * 64 + numBytesToCopy > (int)data.Length )
        {
          numBytesToCopy = (int)data.Length - i * 64;
        }
        data.CopyTo( tempBuffer, i * 64, numBytesToCopy );


        if ( i < m_SpriteProject.Sprites.Count )
        {
          DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoSpritesetSpriteChange( this, m_SpriteProject, i ), i == 0 );

          tempBuffer.CopyTo( m_SpriteProject.Sprites[i].Tile.Data, 0, 63 );
          m_SpriteProject.Sprites[i].Tile.CustomColor = ( tempBuffer.ByteAt( 63 ) & 0xf );
          RebuildSpriteImage( i );
        }
      }

      panelSprites.Invalidate();
      pictureEditor.Invalidate();
      Modified = true;
    }



    public void ReplaceSpriteColors( ColorType Color1, ColorType Color2 )
    {
      foreach ( var spriteIndex in panelSprites.SelectedIndices )
      {
        ReplaceSpriteColors( m_SpriteProject.Sprites[spriteIndex], Color1, Color2 );
      }
    }



    private void ReplaceSpriteColors( SpriteProject.SpriteData Sprite, ColorType Color1, ColorType Color2 )
    {
      if ( Sprite == null )
      {
        Debug.Log( "ReplaceSpriteColors invalid sprite passed" );
        return;
      }
      for ( int y = 0; y < m_SpriteHeight; ++y )
      {
        for ( int x = 0; x < m_SpriteWidth; x += Lookup.PixelWidth( Sprite.Tile.Mode ) )
        {
          ColorType color = (ColorType)Sprite.Tile.GetPixel( x, y );
          if ( color == Color1 )
          {
            Sprite.Tile.SetPixel( x, y, Color2 );
          }
          else if ( color == Color2 )
          {
            Sprite.Tile.SetPixel( x, y, Color1 );
          }
        }
      }
    }



    private void panelSprites_Resize( object sender, EventArgs e )
    {
      panelSprites.SetDisplaySize( panelSprites.ClientSize.Width / 2, panelSprites.ClientSize.Height / 2 );
    }



    private void btnExportToBASICData_Click( object sender, EventArgs e )
    {
      int startLine = GR.Convert.ToI32( editExportBASICLineNo.Text );
      if ( ( startLine < 0 )
      ||   ( startLine > 63999 ) )
      {
        startLine = 10;
      }
      int lineOffset = GR.Convert.ToI32( editExportBASICLineOffset.Text );
      if ( ( lineOffset < 0 )
      ||   ( lineOffset > 63999 ) )
      {
        startLine = 10;
      }

      GR.Memory.ByteBuffer exportData = GatherExportData();
      if ( checkExportToDataWrap.Checked )
      {
        editDataExport.Text = Util.ToBASICData( exportData, startLine, lineOffset, GR.Convert.ToI32( editWrapByteCount.Text ) );
      }
      else
      {
        editDataExport.Text = Util.ToBASICData( exportData, startLine, lineOffset, 80 );
      }
    }



    private void btnImportFromASM_Click( object sender, EventArgs e )
    {
      Parser.ASMFileParser asmParser = new C64Studio.Parser.ASMFileParser();

      Parser.CompileConfig config = new Parser.CompileConfig();
      config.TargetType = Types.CompileTargetType.PLAIN;
      config.OutputFile = "temp.bin";
      config.Assembler = Types.AssemblerType.C64_STUDIO;

      string    temp = "* = $0801\n" + editDataImport.Text;
      if ( ( asmParser.Parse( temp, null, config, null ) )
      &&   ( asmParser.Assemble( config ) ) )
      {
        GR.Memory.ByteBuffer spriteData = asmParser.AssembledOutput.Assembly;

        ImportFromData( spriteData );
      }
    }



    private void ImportFromData( ByteBuffer SpriteData )
    {
      if ( SpriteData == null )
      {
        return;
      }
      int   spriteSizeGuess = 63;
      if ( ( SpriteData.Length % 64 ) == 0 )
      {
        spriteSizeGuess = 64;
      }
      int numSprites = (int)SpriteData.Length / spriteSizeGuess;
      for ( int i = 0; i < numSprites; ++i )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoSpritesetSpriteChange( this, m_SpriteProject, i ), i == 0 );

        SpriteData.CopyTo( m_SpriteProject.Sprites[i].Tile.Data, i * spriteSizeGuess, 63 );
        if ( spriteSizeGuess == 64 )
        {
          m_SpriteProject.Sprites[i].Tile.CustomColor = ( SpriteData.ByteAt( i * 64 + 63 ) & 0xf );
        }
        else
        {
          m_SpriteProject.Sprites[i].Tile.CustomColor = 1;
        }
        RebuildSpriteImage( i );
      }

      editSpriteFrom.Text = "0";
      editSpriteCount.Text = numSprites.ToString();

      panelSprites.Invalidate();
      pictureEditor.Invalidate();
      Modified = false;

      saveSpriteProjectToolStripMenuItem.Enabled = true;
      closeCharsetProjectToolStripMenuItem.Enabled = true;
    }



    private void btnImportFromBASIC_Click( object sender, EventArgs e )
    {
      ImportFromData( Util.FromBASIC( editDataImport.Text ) );
      Modified = true;
    }



    public override bool ApplyFunction( Function Function )
    {
      if ( !pictureEditor.Focused )
      {
        return false;
      }
      switch ( Function )
      {
        case Function.GRAPHIC_ELEMENT_MIRROR_H:
          MirrorX();
          return true;
        case Function.GRAPHIC_ELEMENT_MIRROR_V:
          MirrorY();
          return true;
        case Function.GRAPHIC_ELEMENT_SHIFT_D:
          ShiftDown();
          return true;
        case Function.GRAPHIC_ELEMENT_SHIFT_U:
          ShiftUp();
          return true;
        case Function.GRAPHIC_ELEMENT_SHIFT_L:
          ShiftLeft();
          return true;
        case Function.GRAPHIC_ELEMENT_SHIFT_R:
          ShiftRight();
          return true;
        case Function.GRAPHIC_ELEMENT_ROTATE_L:
          RotateLeft();
          return true;
        case Function.GRAPHIC_ELEMENT_ROTATE_R:
          RotateRight();
          return true;
        case Function.GRAPHIC_ELEMENT_INVERT:
          Invert();
          return true;
        case Function.GRAPHIC_ELEMENT_PREVIOUS:
          Previous();
          return true;
        case Function.GRAPHIC_ELEMENT_NEXT:
          Next();
          return true;
        case Function.GRAPHIC_ELEMENT_CUSTOM_COLOR:
          CustomColor();
          return true;
        case Function.GRAPHIC_ELEMENT_MULTI_COLOR_1:
          MultiColor1();
          return true;
        case Function.GRAPHIC_ELEMENT_MULTI_COLOR_2:
          MultiColor2();
          return true;
      }
      return base.ApplyFunction( Function );
    }



    private void MultiColor2()
    {
      _ColorSettingsDlg.ColorChanged( ColorType.MULTICOLOR_2, ( m_SpriteProject.Colors.MultiColor2 + 1 ) % 16 );
    }



    private void MultiColor1()
    {
      _ColorSettingsDlg.ColorChanged( ColorType.MULTICOLOR_1, ( m_SpriteProject.Colors.MultiColor1 + 1 ) % 16 );
    }



    private void CustomColor()
    {
      _ColorSettingsDlg.ColorChanged( ColorType.CUSTOM_COLOR, ( m_SpriteProject.Sprites[m_CurrentSprite].Tile.CustomColor + 1 ) % 16 );
    }



    private void Next()
    {
      panelSprites.SelectedIndex = ( panelSprites.SelectedIndex + 1 ) % 256;
    }



    private void Previous()
    {
      panelSprites.SelectedIndex = ( panelSprites.SelectedIndex + 256 - 1 ) % 256;
    }



    private void btnExportToBASICHexData_Click( object sender, EventArgs e )
    {
      GR.Memory.ByteBuffer exportData = GatherExportData();

      int startLine = GR.Convert.ToI32( editExportBASICLineNo.Text );

      if ( ( startLine < 0 )
      ||   ( startLine > 63999 ) )
      {
        startLine = 10;
      }
      int lineOffset = GR.Convert.ToI32( editExportBASICLineOffset.Text );
      if ( ( lineOffset < 0 )
      ||   ( lineOffset > 63999 ) )
      {
        startLine = 10;
      }
      if ( checkExportToDataWrap.Checked )
      {
        editDataExport.Text = Util.ToBASICHexData( exportData, startLine, lineOffset, GR.Convert.ToI32( editWrapByteCount.Text ) );
      }
      else
      {
        editDataExport.Text = Util.ToBASICHexData( exportData, startLine, lineOffset );
      }
    }



    private ByteBuffer GatherExportData()
    {
      GR.Memory.ByteBuffer exportData = new GR.Memory.ByteBuffer();

      var exportIndices = GetExportIndices();
      for ( int i = 0; i < exportIndices.Count; ++i )
      {
        exportData.Append( m_SpriteProject.Sprites[exportIndices[i]].Tile.Data );

        byte color = (byte)m_SpriteProject.Sprites[exportIndices[i]].Tile.CustomColor;
        if ( m_SpriteProject.Sprites[exportIndices[i]].Mode == SpriteMode.COMMODORE_24_X_21_MULTICOLOR )
        {
          color |= 0x80;
        }
        exportData.AppendU8( color );
      }
      return exportData;
    }



    private void btnImportFromBASICHex_Click( object sender, EventArgs e )
    {
      ImportFromData( Util.FromBASICHex( editDataImport.Text ) );
      Modified = true;
    }



    private void editDataImport_KeyPress( object sender, KeyPressEventArgs e )
    {
      if ( ( System.Windows.Forms.Control.ModifierKeys == Keys.Control )
      &&   ( e.KeyChar == 1 ) )
      {
        editDataImport.SelectAll();
        e.Handled = true;
      }
    }



    private void btnClear_Click( object sender, EventArgs e )
    {
      editDataImport.Clear();
    }



    private ArrangedItemEntry listLayers_CloningItem( object sender, ArrangedItemEntry Item )
    {
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoSpritesetAddLayer( this, m_SpriteProject, m_SpriteProject.SpriteLayers.Count ) );

      var layer = new SpriteProject.Layer();
      var origLayer = (SpriteProject.Layer)Item.Tag;

      layer.Name = origLayer.Name;
      layer.BackgroundColor = origLayer.BackgroundColor;
      foreach ( var sprite in origLayer.Sprites )
      {
        layer.Sprites.Add( new SpriteProject.LayerSprite() { Color = sprite.Color, ExpandX = sprite.ExpandX, ExpandY = sprite.ExpandY, Index = sprite.Index, X = sprite.X, Y = sprite.Y } );
      }
      var item = new ArrangedItemEntry( layer.Name );
      item.Tag = layer;

      m_SpriteProject.SpriteLayers.Add( layer );

      return item;
    }



    private ArrangedItemEntry listLayerSprites_CloningItem( object sender, ArrangedItemEntry Item )
    {
      var origSprite = (SpriteProject.LayerSprite)Item.Tag;

      Formats.SpriteProject.LayerSprite sprite = new Formats.SpriteProject.LayerSprite();
      sprite.X = origSprite.X;
      sprite.Y = origSprite.Y;
      sprite.Index = origSprite.Index;
      sprite.Color = origSprite.Color;
      sprite.ExpandX = origSprite.ExpandX;
      sprite.ExpandY = origSprite.ExpandY;

      m_CurrentLayer.Sprites.Add( sprite );

      ArrangedItemEntry item = new ArrangedItemEntry();
      item.Text = sprite.Index.ToString() + ", " + sprite.X.ToString() + ", " + sprite.Y.ToString();
      item.Tag = sprite;

      return item;
    }



    private void editLayerDelay_TextChanged( object sender, EventArgs e )
    {
      if ( m_CurrentLayer != null )
      {
        if ( m_CurrentLayer.DelayMS != GR.Convert.ToI32( editLayerDelay.Text ) )
        {
          m_CurrentLayer.DelayMS = GR.Convert.ToI32( editLayerDelay.Text );
          SetModified();
        }
      }
    }



    private void checkAutoplayAnim_CheckedChanged( object sender, EventArgs e )
    {
      m_AnimTimer.Enabled = checkAutoplayAnim.Checked;
      if ( m_AnimTimer.Enabled )
      {
        m_AnimTimer.Interval = 100;
        m_AnimTimer.Start();
      }
      else
      {
        m_AnimTimer.Stop();
      }
    }



    private void btnSavePreviewToGIF_Click( object sender, EventArgs e )
    {
      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Save Preview as GIF";
      saveDlg.Filter = "GIF Image|*.gif";
      if ( DocumentInfo.Project != null )
      {
        saveDlg.InitialDirectory = DocumentInfo.Project.Settings.BasePath;
      }
      if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return;
      }

      // determine bounds
      int     minX = 64000;
      int     maxX = 0;
      int     minY = 48000;
      int     maxY = 0;
      foreach ( var layer in m_SpriteProject.SpriteLayers )
      {
        foreach ( var entry in layer.Sprites )
        {
          minX = Math.Min( entry.X, minX );
          if ( entry.ExpandX )
          {
            maxX = Math.Max( entry.X + 2 * m_SpriteWidth, minX );
          }
          else
          {
            maxX = Math.Max( entry.X + m_SpriteWidth, minX );
          }
          minY = Math.Min( entry.Y, minY );
          if ( entry.ExpandY )
          {
            maxY = Math.Max( entry.Y + 2 * m_SpriteHeight, minY );
          }
          else
          {
            maxY = Math.Max( entry.Y + m_SpriteHeight, minY );
          }
        }
      }

      try
      {
        using ( var outStream = new System.IO.FileStream( saveDlg.FileName, System.IO.FileMode.Create, System.IO.FileAccess.Write ) )
        using ( var gif = new GIFEncoder( outStream, maxX - minX, maxY - minY ) )
        {
          var layerImage = new GR.Image.MemoryImage( maxX - minX, maxY - minY, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
          PaletteManager.ApplyPalette( layerImage );

          foreach ( var layer in m_SpriteProject.SpriteLayers )
          {
            foreach ( var entry in layer.Sprites )
            {
              if ( m_SpriteProject.Sprites[entry.Index].Mode == SpriteMode.COMMODORE_24_X_21_MULTICOLOR )
              {
                SpriteDisplayer.DisplayMultiColorSprite( m_SpriteProject.Sprites[entry.Index].Tile.Data,
                                                         m_SpriteProject.Sprites[entry.Index].Tile.Width,
                                                         m_SpriteProject.Sprites[entry.Index].Tile.Height,
                                                         layer.BackgroundColor,
                                                         m_SpriteProject.Colors.MultiColor1,
                                                         m_SpriteProject.Colors.MultiColor2,
                                                         entry.Color,
                                                         layerImage,
                                                         entry.X,
                                                         entry.Y,
                                                         entry.ExpandX,
                                                         entry.ExpandY );
              }
              else if ( m_SpriteProject.Sprites[entry.Index].Mode == SpriteMode.COMMODORE_24_X_21_HIRES )
              {
                SpriteDisplayer.DisplayHiResSprite( m_SpriteProject.Sprites[entry.Index].Tile.Data,
                                                    m_SpriteProject.Sprites[entry.Index].Tile.Width,
                                                    m_SpriteProject.Sprites[entry.Index].Tile.Height,
                                                    layer.BackgroundColor,
                                                    entry.Color,
                                                    layerImage,
                                                    entry.X,
                                                    entry.Y,
                                                    entry.ExpandX,
                                                    entry.ExpandY );
              }
              else
              {
                Debug.Log( "SavePreviewToGIF unsupported mode " + m_SpriteProject.Sprites[entry.Index].Mode );
              }
            }

            gif.AddFrame( layerImage.GetAsBitmap(), minX, minY, new TimeSpan( 0, 0, 0, 0, layer.DelayMS ) );
          }
          gif.Close();
        }
      }
      catch ( Exception ex )
      {
        MessageBox.Show( "An exception occurred during saving:\r\n" + ex.ToString(), "Error saving GIF file" );
      }
    }



    private void comboSpriteProjectMode_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( DoNotUpdateFromControls )
      {
        return;
      }

      if ( m_SpriteProject.Mode == (SpriteProject.SpriteProjectMode)comboSpriteProjectMode.SelectedIndex )
      {
        return;
      }

      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoSpritesetValuesChange( this, m_SpriteProject ), true );

      m_SpriteProject.Mode = (SpriteProject.SpriteProjectMode)comboSpriteProjectMode.SelectedIndex;

      AdjustSpriteSizes();

      m_SpriteProject.Colors.Palette = PaletteManager.PaletteFromNumColors( Lookup.NumberOfColorsInSprite( m_SpriteProject.Mode ) );
      ChangeColorSettingsDialog();
      OnPaletteChanged();

      for ( int i = 0; i < m_SpriteProject.TotalNumberOfSprites; ++i )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoSpritesetSpriteChange( this, m_SpriteProject, i ), false );

        m_SpriteProject.Sprites[i].Mode       = Lookup.SpriteModeFromSpriteProjectMode( m_SpriteProject.Mode );
        m_SpriteProject.Sprites[i].Tile.Mode  = Lookup.GraphicTileModeFromSpriteProjectMode( m_SpriteProject.Mode );
        m_SpriteProject.Sprites[i].Tile.Data.Resize( (uint)Lookup.NumBytesOfSingleSprite( m_SpriteProject.Mode ) );
        m_SpriteProject.Sprites[i].Tile.Width = m_SpriteWidth;
        m_SpriteProject.Sprites[i].Tile.Height = m_SpriteHeight;

        switch ( m_SpriteProject.Mode )
        {
          case SpriteProject.SpriteProjectMode.COMMODORE_24_X_21_HIRES_OR_MC:
            if ( ( m_SpriteProject.Sprites[i].Mode != SpriteMode.COMMODORE_24_X_21_HIRES )
            &&   ( m_SpriteProject.Sprites[i].Mode != SpriteMode.COMMODORE_24_X_21_MULTICOLOR ) )
            {
              m_SpriteProject.Sprites[i].Mode = SpriteMode.COMMODORE_24_X_21_HIRES;
            }
            break;
          case SpriteProject.SpriteProjectMode.MEGA65_8_X_21_16_COLORS:
            m_SpriteProject.Sprites[i].Mode = SpriteMode.MEGA65_8_X_21_16_COLORS;
            break;
          case SpriteProject.SpriteProjectMode.MEGA65_16_X_21_16_COLORS:
            m_SpriteProject.Sprites[i].Mode = SpriteMode.MEGA65_16_X_21_16_COLORS;
            break;
        }

        RebuildSpriteImage( i );
      }

      panelSprites.Invalidate();
      SetModified();
      pictureEditor.Invalidate();
    }



    private void AdjustSpriteSizes()
    {
      switch ( m_SpriteProject.Mode )
      {
        case SpriteProject.SpriteProjectMode.COMMODORE_24_X_21_HIRES_OR_MC:
          m_SpriteWidth = 24;
          m_SpriteHeight = 21;
          break;
        case SpriteProject.SpriteProjectMode.MEGA65_8_X_21_16_COLORS:
          m_SpriteWidth = 8;
          m_SpriteHeight = 21;
          break;
        case SpriteProject.SpriteProjectMode.MEGA65_16_X_21_16_COLORS:
          m_SpriteWidth = 16;
          m_SpriteHeight = 21;
          break;
        default:
          Debug.Log( "comboSpriteProjectMode_SelectedIndexChanged, unsupported mode " + m_SpriteProject.Mode );
          break;
      }

      // adjust aspect ratio of the editor
      int   biggerSize = Math.Max( m_SpriteWidth, m_SpriteHeight );

      pictureEditor.Size = new System.Drawing.Size( m_SpriteWidth * m_SpriteEditorOrigWidth / biggerSize,
                                                    m_SpriteHeight * m_SpriteEditorOrigHeight / biggerSize );

      pictureEditor.DisplayPage.Create( m_SpriteWidth, m_SpriteHeight, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
      panelSprites.ItemWidth = m_SpriteWidth;
      panelSprites.ItemHeight = m_SpriteHeight;
      panelSprites.SetDisplaySize( 4 * m_SpriteWidth, 6 * m_SpriteHeight );
    }



    private void OnPaletteChanged()
    {
      PaletteManager.ApplyPalette( pictureEditor.DisplayPage, m_SpriteProject.Colors.Palette );
      //PaletteManager.ApplyPalette( panelSprites.DisplayPage, m_SpriteProject.Colors.Palette );
      PaletteManager.ApplyPalette( layerPreview.DisplayPage, m_SpriteProject.Colors.Palette );

      for ( int i = 0; i < m_SpriteProject.TotalNumberOfSprites; ++i )
      {
        PaletteManager.ApplyPalette( m_SpriteProject.Sprites[i].Tile.Image, m_SpriteProject.Colors.Palettes[m_SpriteProject.Sprites[i].Tile.Colors.ActivePalette] );
        RebuildSpriteImage( i );
        panelSprites.Items[i].MemoryImage = m_SpriteProject.Sprites[i].Tile.Image;
      }

      panelSprites.Invalidate();
      pictureEditor.Invalidate();
    }



    private void btnEditPalette_Click( object sender, EventArgs e )
    {
      var dlgPalette = new DlgPaletteEditor( Core, m_SpriteProject.Colors );
      if ( dlgPalette.ShowDialog() == DialogResult.OK )
      {
        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoSpritesetValuesChange( this, m_SpriteProject ) );
        m_SpriteProject.Colors.Palettes = dlgPalette.Colors.Palettes;

        SetModified();
        OnPaletteChanged();
      }
    }



    private void comboSpriteColor_DrawItem( object sender, DrawItemEventArgs e )
    {
      ComboBox combo = (ComboBox)sender;

      if ( ( m_SpriteProject.Mode == SpriteProject.SpriteProjectMode.MEGA65_16_X_21_16_COLORS )
      ||   ( m_SpriteProject.Mode == SpriteProject.SpriteProjectMode.MEGA65_8_X_21_16_COLORS ) )
      {
        Core?.Theming.DrawSingleColorComboBox( combo, e, m_SpriteProject.Colors.Palette );
      }
      else
      {
        Core?.Theming.DrawMultiColorComboBox( combo, e, m_SpriteProject.Colors.Palette );
      }
    }



    private void btnToolEdit_CheckedChanged( object sender, EventArgs e )
    {
      m_Mode = ToolMode.SINGLE_PIXEL;
    }



    private void btnToolFill_CheckedChanged( object sender, EventArgs e )
    {
      m_Mode = ToolMode.FILL;
    }



    private void ChangeColorSettingsDialog()
    {
      if ( _ColorSettingsDlg != null )
      {
        panelColorSettings.Controls.Remove( _ColorSettingsDlg );
        _ColorSettingsDlg.Dispose();
        _ColorSettingsDlg = null;
      }

      switch ( m_SpriteProject.Mode )
      {
        case SpriteProject.SpriteProjectMode.COMMODORE_24_X_21_HIRES_OR_MC:
          _ColorSettingsDlg = new ColorSettingsMCSprites( Core, 
                                                          m_SpriteProject.Colors, 
                                                          m_SpriteProject.Sprites[m_CurrentSprite].Tile.CustomColor, 
                                                          m_SpriteProject.Sprites[m_CurrentSprite].Tile.Mode == GraphicTileMode.COMMODORE_MULTICOLOR );
          break;
        case SpriteProject.SpriteProjectMode.MEGA65_8_X_21_16_COLORS:
        case SpriteProject.SpriteProjectMode.MEGA65_16_X_21_16_COLORS:
          _ColorSettingsDlg = new ColorSettingsMega65( Core, m_SpriteProject.Colors, m_SpriteProject.Sprites[m_CurrentSprite].Tile.CustomColor );
          break;
      }
      panelColorSettings.Controls.Add( _ColorSettingsDlg );
      _ColorSettingsDlg.SelectedColorChanged += _ColorSettingsDlg_SelectedColorChanged;
      _ColorSettingsDlg.ColorsModified += _ColorSettingsDlg_ColorsModified;
      _ColorSettingsDlg.ColorsExchanged += _ColorSettingsDlg_ColorsExchanged;
      _ColorSettingsDlg.PaletteModified += _ColorSettingsDlg_PaletteModified;
      _ColorSettingsDlg.MulticolorFlagChanged += _ColorSettingsDlg_MulticolorFlagChanged;

      _ColorSettingsDlg_SelectedColorChanged( _ColorSettingsDlg.SelectedColor );
    }



    private void _ColorSettingsDlg_MulticolorFlagChanged()
    {
      if ( DoNotUpdateFromControls )
      {
        return;
      }
      DocumentInfo.UndoManager.StartUndoGroup();

      var selectedSprites = panelSprites.SelectedIndices;

      foreach ( var i in selectedSprites )
      {
        if ( ( m_SpriteProject.Sprites[i].Mode == SpriteMode.COMMODORE_24_X_21_HIRES )
        ||   ( m_SpriteProject.Sprites[i].Mode == SpriteMode.COMMODORE_24_X_21_MULTICOLOR ) )
        {
          if ( ( m_SpriteProject.Sprites[i].Mode == SpriteMode.COMMODORE_24_X_21_MULTICOLOR ) != _ColorSettingsDlg.MultiColorEnabled )
          {
            DocumentInfo.UndoManager.AddGroupedUndoTask( new Undo.UndoSpritesetSpriteChange( this, m_SpriteProject, i ) );

            m_SpriteProject.Sprites[i].Mode = _ColorSettingsDlg.MultiColorEnabled ? SpriteMode.COMMODORE_24_X_21_MULTICOLOR : SpriteMode.COMMODORE_24_X_21_HIRES;
            m_SpriteProject.Sprites[i].Tile.Mode = _ColorSettingsDlg.MultiColorEnabled ? GraphicTileMode.COMMODORE_MULTICOLOR : GraphicTileMode.COMMODORE_HIRES;
            Modified = true;
            RebuildSpriteImage( i );
            if ( m_CurrentSprite == i )
            {
              pictureEditor.Invalidate();
            }
            panelSprites.InvalidateItemRect( i );
          }
        }
      }
    }



    private void _ColorSettingsDlg_PaletteModified( ColorSettings Colors, int CustomColor )
    {
      DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoSpritesetValuesChange( this, m_SpriteProject ) );

      m_SpriteProject.Colors = new ColorSettings( Colors );
      foreach ( int spriteIndex in panelSprites.SelectedIndices )
      {
        var sprite = m_SpriteProject.Sprites[spriteIndex];

        sprite.Tile.Colors = new ColorSettings( Colors );

        PaletteManager.ApplyPalette( sprite.Tile.Image, m_SpriteProject.Colors.Palettes[sprite.Tile.Colors.ActivePalette] );

        RebuildSpriteImage( spriteIndex );
        if ( m_CurrentSprite == spriteIndex )
        {
          PaletteManager.ApplyPalette( pictureEditor.DisplayPage, m_SpriteProject.Colors.Palettes[sprite.Tile.Colors.ActivePalette] );
          pictureEditor.Invalidate();
        }
        panelSprites.InvalidateItemRect( spriteIndex );
      }

      OnPaletteChanged();

      SetModified();
    }



    private void _ColorSettingsDlg_ColorsExchanged( ColorType Color1, ColorType Color2 )
    {
      bool    firstEntry = true;
      foreach ( int spriteIndex in panelSprites.SelectedIndices )
      {
        var sprite = m_SpriteProject.Sprites[spriteIndex];

        DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoSpritesetSpriteChange( this, m_SpriteProject, spriteIndex ), firstEntry );
        firstEntry = false;

        ReplaceSpriteColors( sprite, Color1, Color2 );

        RebuildSpriteImage( spriteIndex );
        panelSprites.InvalidateItemRect( spriteIndex );
      }
    }



    private void _ColorSettingsDlg_ColorsModified( ColorType Color, ColorSettings Colors, int CustomColor )
    {
      switch ( Color )
      {
        case ColorType.BACKGROUND:
          if ( m_SpriteProject.Colors.BackgroundColor != _ColorSettingsDlg.Colors.BackgroundColor )
          {
            DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoSpritesetValuesChange( this, m_SpriteProject ) );

            m_SpriteProject.Colors.BackgroundColor = _ColorSettingsDlg.Colors.BackgroundColor;
            Modified = true;
            for ( int i = 0; i < m_SpriteProject.TotalNumberOfSprites; ++i )
            {
              RebuildSpriteImage( i );
            }
            pictureEditor.Invalidate();
            panelSprites.Invalidate();
          }
          break;
        case ColorType.MULTICOLOR_1:
          if ( m_SpriteProject.Colors.MultiColor1 != _ColorSettingsDlg.Colors.MultiColor1 )
          {
            DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoSpritesetValuesChange( this, m_SpriteProject ) );

            m_SpriteProject.Colors.MultiColor1 = _ColorSettingsDlg.Colors.MultiColor1;
            Modified = true;
            for ( int i = 0; i < m_SpriteProject.TotalNumberOfSprites; ++i )
            {
              RebuildSpriteImage( i );
            }
            pictureEditor.Invalidate();
            panelSprites.Invalidate();
          }
          break;
        case ColorType.MULTICOLOR_2:
          if ( m_SpriteProject.Colors.MultiColor2 != _ColorSettingsDlg.Colors.MultiColor2 )
          {
            DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoSpritesetValuesChange( this, m_SpriteProject ) );

            m_SpriteProject.Colors.MultiColor2 = _ColorSettingsDlg.Colors.MultiColor2;
            Modified = true;
            for ( int i = 0; i < m_SpriteProject.TotalNumberOfSprites; ++i )
            {
              RebuildSpriteImage( i );
            }
            pictureEditor.Invalidate();
            panelSprites.Invalidate();
          }
          break;
        case ColorType.CUSTOM_COLOR:
          if ( Lookup.HaveCustomSpriteColor( m_SpriteProject.Mode ) )
          {
            DocumentInfo.UndoManager.StartUndoGroup();

            var selectedSprites = panelSprites.SelectedIndices;

            foreach ( var i in selectedSprites )
            {
              if ( m_SpriteProject.Sprites[i].Tile.CustomColor != _ColorSettingsDlg.CustomColor )
              {
                DocumentInfo.UndoManager.AddGroupedUndoTask( new Undo.UndoSpritesetSpriteChange( this, m_SpriteProject, i ) );
                m_SpriteProject.Sprites[i].Tile.CustomColor = _ColorSettingsDlg.CustomColor;
                Modified = true;
                RebuildSpriteImage( i );
                if ( i == m_CurrentSprite )
                {
                  pictureEditor.Invalidate();
                }
                panelSprites.InvalidateItemRect( i );
              }
            }
          }
          break;
        default:
          throw new NotImplementedException();
      }
    }



    private void _ColorSettingsDlg_SelectedColorChanged( ColorType Color )
    {
    }



  }
}
