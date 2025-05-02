using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RetroDevStudio.Formats;
using GR.Image;
using RetroDevStudio;
using RetroDevStudio.Types;
using GR.Forms;
using GR.Generic;
using GR.Collections;
using GR.Memory;

namespace RetroDevStudio.Controls
{
  public partial class CharacterEditor : UserControl
  {
    public delegate void ModifiedHandler( List<int> ModifiedCharacters );
    public delegate void CharsetShiftedHandler( int[] OldToNew, int[] NewToOld );

    public event ModifiedHandler        Modified;
    public event CharsetShiftedHandler  CharactersShifted;

    private ushort                      m_CurrentChar = 0;
    private ushort                      m_CurrentColor = 1;
    private int                         m_CurrentPaletteMapping = 0;
    private ColorType                   m_CurrentColorType = ColorType.CUSTOM_COLOR;
    private bool                        m_ButtonReleased = false;
    public StudioCore                   Core = null;
    public Undo.UndoManager             UndoManager = null;

    private bool                        DoNotUpdateFromControls = false;
    private bool                        DoNotAddUndo = false;

    private CharsetProject              m_Project = new CharsetProject();

    private GR.Image.MemoryImage        m_ImagePlayground = new GR.Image.MemoryImage( 256, 256, GR.Drawing.PixelFormat.Format32bppRgb );

    private bool                        _AllowModeChange = true;

    private ColorSettingsBase           _ColorSettingsDlg = null;
    private ColorPickerBase             _ColorPickerDlg = null;

    private int                         _NumColorsInColorSelector = 16;

    private int                         m_CharacterWidth = 8;
    private int                         m_CharacterHeight = 8;

    private int                         m_CharacterEditorOrigWidth = -1;
    private int                         m_CharacterEditorOrigHeight = -1;


    public bool AllowModeChange
    {
      get
      {
        return _AllowModeChange;
      }
      set
      {
        _AllowModeChange = value;
        if ( comboCharsetMode != null )
        {
          comboCharsetMode.Enabled = _AllowModeChange;
          labelCharsetMode.Enabled = _AllowModeChange;
        }
      }
    }



    public CharacterEditor()
    {
      this.Core = StudioCore.StaticCore;
      if ( Core == null )
      {
        Core = new StudioCore();
      }
      Setup();
    }



    public CharacterEditor( StudioCore Core )
    {
      this.Core = Core;

      Setup();
    }



    private void Setup()
    {
      AllowModeChange = true;

      InitializeComponent();

      picturePlayground.DisplayPage.Create( 128, 128, GR.Drawing.PixelFormat.Format32bppRgb );
      panelCharacters.PixelFormat = GR.Drawing.PixelFormat.Format32bppRgb;
      panelCharacters.SetDisplaySize( 128, 128 );
      m_ImagePlayground.Create( 256, 256, GR.Drawing.PixelFormat.Format32bppRgb );

      if ( Core != null )
      {
        m_Project.Colors.Palette = Core.Imaging.PaletteFromMachine( MachineType.C64 );
      }
      ChangeColorSettingsDialog();
      ChangeColorPickerDialog();
      OnPaletteChanged();

      m_CharacterEditorOrigWidth = canvasEditor.Width;
      m_CharacterEditorOrigHeight = canvasEditor.Height;

      foreach ( TextCharMode mode in Enum.GetValues( typeof( TextCharMode ) ) )
      {
        if ( mode != TextCharMode.UNKNOWN )
        {
          comboCharsetMode.Items.Add( GR.EnumHelper.GetDescription( mode ) );
        }
      }
      comboCharsetMode.SelectedIndex = 0;

      panelCharacters.SelectedIndex = 0;

      ListViewItem    itemUn = new ListViewItem( "Uncategorized" );
      itemUn.Tag = 0;
      itemUn.SubItems.Add( "0" );
      listCategories.Items.Add( itemUn );
      comboCategories.Items.Add( itemUn.Name );
      RefreshCategoryCounts();
    }



    public void RaiseModifiedEvent( List<int> ModifiedCharacters )
    {
      Modified?.Invoke( ModifiedCharacters );
    }



    protected void RaiseCharactersShiftedEvent( int[] OldToNew, int[] NewToOld )
    {
      CharactersShifted?.Invoke( OldToNew, NewToOld );
    }



    private void RedrawPlayground()
    {
      picturePlayground.DisplayPage.DrawImage( m_ImagePlayground, 0, 0 );
      picturePlayground.Invalidate();
    }



    private void RedrawColorPicker()
    {
      if ( _ColorPickerDlg != null )
      {
        _ColorPickerDlg.SelectedColor          = m_CurrentColor;
        _ColorPickerDlg.SelectedChar           = m_CurrentChar;
        _ColorPickerDlg.SelectedPaletteMapping = m_CurrentPaletteMapping;
        _ColorPickerDlg.Redraw();
      }
    }



    public List<int> SelectedIndices
    {
      get
      {
        return panelCharacters.SelectedIndices;
      }
    }



    public CharsetProject CharacterSet
    {
      get
      {
        return m_Project;
      }
    }



    public bool EditorFocused
    {
      get
      {
        return ( ( canvasEditor.Focused )
            ||   ( panelCharacters.Focused ) );
      }
    }



    private void CopyToClipboard()
    {
      List<int> selectedImages = panelCharacters.SelectedIndices;
      if ( selectedImages.Count == 0 )
      {
        return;
      }

      var clipList = new ClipboardImageList();
      clipList.Mode   = Lookup.GraphicTileModeFromTextCharMode( m_Project.Mode, 8 );
      clipList.Colors = m_Project.Colors;
      clipList.ColumnBased = panelCharacters.IsSelectionColumnBased;

      foreach ( int index in selectedImages )
      {
        int   indexToUse = index;
        if ( Lookup.IsECMMode( m_Project.Mode ) )
        {
          indexToUse %= 64;
        }
        var entry = new ClipboardImageList.Entry();
        var character = m_Project.Characters[indexToUse];

        entry.Tile        = character.Tile;
        entry.Index       = indexToUse;

        clipList.Entries.Add( entry );
      }

      clipList.CopyToClipboard();
    }



    private void PasteClipboardImageToChar()
    {
      var clipList = new ClipboardImageList();

      if ( clipList.GetFromClipboard() )
      {
        int pastePos = panelCharacters.SelectedIndex;
        if ( pastePos == -1 )
        {
          pastePos = 0;
        }
        bool  firstEntry = true;
        var   modifiedChars = new List<int>();
        foreach ( var entry in clipList.Entries )
        {
          int indexGap =  entry.Index;
          pastePos += indexGap;

          if ( Lookup.IsECMMode( m_Project.Mode ) )
          {
            pastePos %= 64;
          }

          if ( pastePos >= m_Project.Characters.Count )
          {
            break;
          }

          modifiedChars.Add( pastePos );
          UndoManager.AddUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, pastePos, 1 ), firstEntry );
          firstEntry = false;

          var targetTile = m_Project.Characters[pastePos].Tile;

          if ( ( ( entry.Tile.Mode == GraphicTileMode.COMMODORE_HIRES )
          ||     ( entry.Tile.Mode == GraphicTileMode.COMMODORE_MULTICOLOR_CHARACTERS ) )
          &&   ( ( targetTile.Mode == GraphicTileMode.COMMODORE_HIRES )
          ||     ( targetTile.Mode == GraphicTileMode.COMMODORE_MULTICOLOR_CHARACTERS ) ) )
          {
            // can copy mode
            targetTile.Mode = entry.Tile.Mode;
          }
          if ( ( entry.Tile.Mode == GraphicTileMode.COMMODORE_ECM )
          &&   ( ( targetTile.Mode == GraphicTileMode.COMMODORE_HIRES )
          ||     ( targetTile.Mode == GraphicTileMode.COMMODORE_MULTICOLOR_CHARACTERS ) ) )
          {
            entry.Tile.Mode = GraphicTileMode.COMMODORE_HIRES;
          }

          int copyWidth = Math.Max( 8, entry.Tile.Width );
          int copyHeight = Math.Max( 8, entry.Tile.Height );

          
          for ( int x = 0; x < copyWidth; x += Lookup.PixelWidth( targetTile.Mode ) )
          {
            for ( int y = 0; y < copyHeight; ++y )
            {
              targetTile.SetPixel( x, y, entry.Tile.MapPixelColor( x, y, targetTile ) );
            }
          }
          targetTile.CustomColor = entry.Tile.CustomColor;

          RebuildAffectedChar( pastePos );

          if ( pastePos == m_CurrentChar )
          {
            _ColorSettingsDlg.CustomColor = (byte)m_Project.Characters[pastePos].Tile.CustomColor;
          }
        }
        canvasEditor.Invalidate();
        RaiseModifiedEvent( modifiedChars );
        return;
      }
      else
      {
        IDataObject dataObj = Clipboard.GetDataObject();
        if ( dataObj == null )
        {
          System.Windows.Forms.MessageBox.Show( "No image on clipboard" );
          return;
        }
        if ( dataObj.GetDataPresent( "RetroDevStudio.ImageList" ) )
        {
          System.IO.MemoryStream ms = (System.IO.MemoryStream)dataObj.GetData( "RetroDevStudio.ImageList" );

          GR.Memory.ByteBuffer spriteData = new GR.Memory.ByteBuffer( (uint)ms.Length );

          ms.Read( spriteData.Data(), 0, (int)ms.Length );

          GR.IO.MemoryReader memIn = spriteData.MemoryReader();

          int rangeCount = memIn.ReadInt32();
          bool columnBased = ( memIn.ReadInt32() > 0 ) ? true : false;

          var incomingColorSettings = new ColorSettings();

          int numPaletteEntries = memIn.ReadInt32();
          incomingColorSettings.Palette = new Palette( numPaletteEntries );
          for ( int i = 0; i < numPaletteEntries; ++i )
          {
            incomingColorSettings.Palette.ColorValues[i] = memIn.ReadUInt32();
          }
          incomingColorSettings.Palette.CreateBrushes();

          var   modifiedChars = new List<int>();
          int pastePos = panelCharacters.SelectedIndex;
          if ( pastePos == -1 )
          {
            pastePos = 0;
          }

          for ( int i = 0; i < rangeCount; ++i )
          {
            int indexGap = memIn.ReadInt32();
            pastePos += indexGap;

            if ( pastePos >= m_Project.Characters.Count )
            {
              break;
            }

            modifiedChars.Add( pastePos );
            UndoManager.AddUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, pastePos, 1 ), i == 0 );

            var mode = (TextCharMode)memIn.ReadInt32();
            m_Project.Characters[pastePos].Tile.CustomColor = (byte)memIn.ReadInt32();

            // TODO - mapping the color would be wiser
            if ( m_Project.Characters[pastePos].Tile.CustomColor >= m_Project.Colors.Palette.NumColors )
            {
              m_Project.Characters[pastePos].Tile.CustomColor %= (byte)m_Project.Colors.Palette.NumColors;
            }

            uint width   = memIn.ReadUInt32();
            uint height  = memIn.ReadUInt32();

            uint dataLength = memIn.ReadUInt32();

            GR.Memory.ByteBuffer    tempBuffer = new GR.Memory.ByteBuffer();
            tempBuffer.Reserve( (int)dataLength );
            memIn.ReadBlock( tempBuffer, dataLength );

            if ( ( width == 8 )
            &&   ( height == 8 ) )
            {
              var incomingTile = new GraphicTile( (int)width, (int)height, Lookup.GraphicTileModeFromTextCharMode( mode, 8 ), incomingColorSettings );
              incomingTile.Data = tempBuffer;
              if ( m_Project.Mode != mode )
              {
                // TODO need to map by image
                for ( int x = 0; x < 8; ++x )
                {
                  for ( int y = 0; y < 8; ++y )
                  {
                    Debug.Log( "Clipboard and Project need colorsettings object!" );
                    //int     colorIndex = incomingTile.MapPixelColor( x, y, incomingColorSettings, m_Project.Characters[pastePos].Tile, m_Project.ColorSettings );

                    //m_Project.Characters[pastePos].Tile.SetPixel( x, y, colorIndex );
                  }
                }
              }
              else
              {
                tempBuffer.CopyTo( m_Project.Characters[pastePos].Tile.Data, 0, Math.Min( Lookup.NumBytesOfSingleCharacterBitmap( m_Project.Mode ), (int)dataLength ) );
              }
            }
            else if ( ( width == 24 )
            &&        ( height == 21 ) )
            {
              // upper left cell of sprite into char (C64)
              for ( int j = 0; j < 8; ++j )
              {
                m_Project.Characters[pastePos].Tile.Data.SetU8At( j, tempBuffer.ByteAt( j * 3 ) );
              }
            }
            else
            {
              tempBuffer.CopyTo( m_Project.Characters[pastePos].Tile.Data, 0, Math.Min( Lookup.NumBytesOfSingleCharacterBitmap( m_Project.Mode ), (int)dataLength ) );
            }


            int index = memIn.ReadInt32();

            RebuildAffectedChar( pastePos );

            if ( pastePos == m_CurrentChar )
            {
              _ColorSettingsDlg.CustomColor = (byte)m_Project.Characters[pastePos].Tile.CustomColor;
            }
          }
          canvasEditor.Invalidate();
          RaiseModifiedEvent( modifiedChars );
          return;
        }
        else if ( !Clipboard.ContainsImage() )
        {
          System.Windows.Forms.MessageBox.Show( "No image on clipboard" );
          return;
        }
        GR.Image.FastImage imgClip = null;
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
        PasteImage( "", imgClip, checkPasteMultiColor.Checked );
      }
    }



    private void btnCopy_Click( DecentForms.ControlBase Sender )
    {
      CopyToClipboard();
    }



    private void btnPaste_Click( DecentForms.ControlBase Sender )
    {
      PasteClipboardImageToChar();
    }



    public void CharacterChanged( int CharIndex, int Count )
    {
      DoNotUpdateFromControls = true;

      bool currentCharChanged = false;

      for ( int charIndex = CharIndex; charIndex < CharIndex + Count; ++charIndex )
      {
        RebuildAffectedChar( charIndex );
        if ( m_CurrentChar == charIndex )
        {
          currentCharChanged = true;
        }
      }

      if ( currentCharChanged )
      {
        panelCharacters_SelectionChanged( null, null );
        _ColorSettingsDlg.CustomColor = (byte)m_Project.Characters[m_CurrentChar].Tile.CustomColor;

        comboCharsetMode.SelectedIndex = (int)m_Project.Mode;
      }

      RefreshCategoryCounts();
      DoNotUpdateFromControls = false;

      var   modifiedChars = new List<int>();
      for ( int i = 0; i < Count; ++i )
      {
        modifiedChars.Add( CharIndex + i );
      }
      RaiseModifiedEvent( modifiedChars );
    }



    void RebuildCharImage( int CharIndex )
    {
      m_Project.Characters[CharIndex].Tile.Data.Resize( (uint)Lookup.NumBytesOfSingleCharacterBitmap( m_Project.Mode ) );

      Displayer.CharacterDisplayer.DisplayChar( m_Project, CharIndex, m_Project.Characters[CharIndex].Tile.Image, 0, 0 );
      if ( CharIndex < panelCharacters.Items.Count )
      {
        panelCharacters.Items[CharIndex].MemoryImage = m_Project.Characters[CharIndex].Tile.Image;
        panelCharacters.InvalidateItemRect( CharIndex );
      }
      bool playgroundChanged = false;
      for ( int i = 0; i < 16; ++i )
      {
        for ( int j = 0; j < 16; ++j )
        {
          if ( ( m_Project.PlaygroundChars[i + j * 16] & 0xffff ) == CharIndex )
          {
            playgroundChanged = true;
            Displayer.CharacterDisplayer.DisplayChar( m_Project, CharIndex, m_ImagePlayground, i * m_CharacterWidth, j * m_CharacterHeight, (int)m_Project.PlaygroundChars[i + j * 16] >> 16 );
          }
        }
      }
      if ( playgroundChanged )
      {
        RedrawPlayground();
      }
      if ( CharIndex == m_CurrentChar )
      {
        RedrawColorPicker();
      }
    }



    public void PasteImage( string FromFile, GR.Image.FastImage Image, bool ForceMulticolor )
    {
      var   mcSettings = new ColorSettings( m_Project.Colors );

      bool pasteAsBlock = false;

      Types.GraphicType   importType = Types.GraphicType.CHARACTERS;
      if ( ( m_Project.Mode == TextCharMode.MEGA65_FCM )
      ||   ( m_Project.Mode == TextCharMode.MEGA65_FCM_16BIT ) )
      {
        importType = Types.GraphicType.CHARACTERS_FCM;
      }
      else if ( ( m_Project.Mode == TextCharMode.X16_HIRES )
      ||        ( m_Project.Mode == TextCharMode.COMMODORE_128_VDC_HIRES )
      ||        ( m_Project.Mode == TextCharMode.COMMODORE_ECM )
      ||        ( m_Project.Mode == TextCharMode.COMMODORE_HIRES )
      ||        ( m_Project.Mode == TextCharMode.MEGA65_HIRES ) )
      {
        importType = Types.GraphicType.CHARACTERS_HIRES;
      }
      if ( !Core.MainForm.ImportImage( FromFile, Image, importType, mcSettings, m_CharacterWidth, m_CharacterHeight, out GR.Image.IImage mappedImage, out mcSettings, out pasteAsBlock, out Types.GraphicType importAsType ) )
      {
        Image.Dispose();
        return;
      }
      if ( mappedImage.PixelFormat != GR.Drawing.PixelFormat.Format8bppIndexed )
      {
        mappedImage.Dispose();
        System.Windows.Forms.MessageBox.Show( "Image format invalid!\nNeeds to be 8bit index" );
        return;
      }

      bool  hadFirstUndoStep = false;
      if ( mcSettings.Palettes.Count > m_Project.Colors.Palettes.Count )
      {
        // a palette was imported!
        UndoManager.AddUndoTask( new Undo.UndoCharacterEditorValuesChange( this, m_Project ) );
        hadFirstUndoStep = true;
        _ColorSettingsDlg.PalettesChanged();
        m_Project.Colors.Palettes.Add( mcSettings.Palettes[mcSettings.Palettes.Count - 1] );
        m_Project.Colors.ActivePalette = m_Project.Colors.Palettes.Count - 1;
        ChangeColorSettingsDialog();
        OnPaletteChanged();
      }

      if ( mcSettings.BackgroundColor != -1 )
      {
        _ColorSettingsDlg.ColorChanged( ColorType.BACKGROUND, mcSettings.BackgroundColor );
      }
      if ( mcSettings.MultiColor1 != -1 )
      {
        _ColorSettingsDlg.ColorChanged( ColorType.MULTICOLOR_1, mcSettings.MultiColor1 );
      }
      if ( mcSettings.MultiColor2 != -1 )
      {
        _ColorSettingsDlg.ColorChanged( ColorType.MULTICOLOR_2, mcSettings.MultiColor2 );
      }

      int charsX = ( mappedImage.Width + 7 ) / 8;
      int charsY = ( mappedImage.Height + 7 ) / 8;
      int curCharX = m_CurrentChar % 16;
      int curCharY = m_CurrentChar / 16;
      int currentTargetChar = m_CurrentChar;
      var   modifiedChars = new List<int>();

      for ( int j = 0; j < charsY; ++j )
      {
        for ( int i = 0; i < charsX; ++i )
        {
          if ( pasteAsBlock )
          {
            int localCharX = ( curCharX + i ) % 16;
            int localCharY = curCharY + j;
            if ( curCharX + i >= 16 )
            {
              // wrap
              localCharY += charsY * ( ( curCharX + i ) / 16 );
            }
            if ( localCharY >= 16 )
            {
              continue;
            }
            currentTargetChar = localCharY * 16 + localCharX;
          }
          else if ( currentTargetChar >= m_Project.TotalNumberOfCharacters )
          {
            continue;
          }

          int copyWidth = mappedImage.Width - i * 8;
          if ( copyWidth > 8 )
          {
            copyWidth = 8;
          }
          int copyHeight = mappedImage.Height - j * 8;
          if ( copyHeight > 8 )
          {
            copyHeight = 8;
          }
          GR.Image.FastImage singleChar = mappedImage.GetImage( i * 8, j * 8, copyWidth, copyHeight ) as GR.Image.FastImage;

          modifiedChars.Add( currentTargetChar );

          UndoManager.AddUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, currentTargetChar, 1 ), !hadFirstUndoStep );
          hadFirstUndoStep = true;

          ImportChar( singleChar, currentTargetChar, ForceMulticolor );
          panelCharacters.InvalidateItemRect( currentTargetChar );

          if ( currentTargetChar == m_CurrentChar )
          {
            byte  newCustomColor = (byte)m_Project.Characters[m_CurrentChar].Tile.CustomColor;
            if ( ( importAsType == GraphicType.CHARACTERS_MULTICOLOR )
            &&   ( newCustomColor < 8 ) )
            {
              newCustomColor += 8;
            }
            _ColorSettingsDlg.CustomColor = newCustomColor;
          }
          if ( !pasteAsBlock )
          {
            ++currentTargetChar;
          }
        }
      }

      canvasEditor.Invalidate();
      RaiseModifiedEvent( modifiedChars );
    }



    private void panelCharacters_SelectionChanged( object sender, EventArgs e )
    {
      ValidateMoveToTarget();

      int newChar = panelCharacters.SelectedIndex;
      if ( ( newChar != -1 )
      &&   ( panelCharacters.SelectedIndices.Count == 1 ) )
      {
        labelCharNo.Text = "Character: " + newChar.ToString();
        m_CurrentChar = (ushort)newChar;

        if ( !Lookup.HasCustomPalette( m_Project.Characters[m_CurrentChar].Tile.Mode ) )
        {
          if ( _ColorSettingsDlg.CustomColor != m_Project.Characters[m_CurrentChar].Tile.CustomColor )
          {
            _ColorSettingsDlg.CustomColor = (byte)m_Project.Characters[m_CurrentChar].Tile.CustomColor;
          }
        }
        else
        {
          _ColorSettingsDlg.ActivePalette = m_Project.Characters[m_CurrentChar].Tile.Colors.ActivePalette;
        }
        canvasEditor.Invalidate();

        SelectCategory( m_Project.Characters[m_CurrentChar].Category );
        RedrawColorPicker();
      }
    }



    private void ValidateMoveToTarget()
    {
      if ( Lookup.IsECMMode( m_Project.Mode ) )
      {
        bool enable = true;
        int targetIndex = GR.Convert.ToI32( editMoveTargetIndex.Text );
        foreach ( var index in panelCharacters.SelectedIndices )
        {
          // all selected characters must be in the same 64 character block
          for ( int j = 0; j < panelCharacters.SelectedIndices.Count; ++j )
          {
            if ( ( panelCharacters.SelectedIndices[j] / 64 ) != ( index / 64 ) )
            {
              enable = false;
              break;
            }
          }
          if ( !enable )
          {
            break;
          }
          // target must be in same 64 character block
          if ( ( index / 64 ) != ( targetIndex / 64 ) )
          {
            enable = false;
            break;
          }
          ++targetIndex;
        }
        btnMoveSelectionToTarget.Enabled = enable;
      }
      else
      {
        btnMoveSelectionToTarget.Enabled = true;
      }
    }



    public bool ImportChar( GR.Image.IImage Image, int CharIndex, bool ForceMulticolor )
    {
      if ( Image.PixelFormat != GR.Drawing.PixelFormat.Format8bppIndexed )
      {
        // invalid format
        return false;
      }
      // Match image data
      GR.Memory.ByteBuffer Buffer = new GR.Memory.ByteBuffer( m_Project.Characters[CharIndex].Tile.Data );

      int   chosenCharColor = -1;

      bool  isMultiColor = false;

      if ( ( m_Project.Mode == TextCharMode.MEGA65_FCM )
      ||   ( m_Project.Mode == TextCharMode.MEGA65_FCM_16BIT ) )
      {
        for ( int i = 0; i < Buffer.Length; ++i )
        {
          Buffer.SetU8At( i, (byte)Image.GetPixel( i % 8, i / 8 ) );
        }
      }
      else
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
            int     colorIndex = (int)Image.GetPixel( x, y ) % 16;
            if ( colorIndex >= 16 )
            {
              return false;
            }
            if ( ( x % 2 ) == 0 )
            {
              if ( colorIndex != (int)Image.GetPixel( x + 1, y ) % 16 )
              {
                // not a double pixel, must be single color then
                hasSinglePixel = true;
              }
            }

            if ( !usedColor[colorIndex] )
            {
              if ( colorIndex == m_Project.Colors.BackgroundColor )
              {
                usedBackgroundColor = true;
              }
              usedColor[colorIndex] = true;
              numColors++;
            }
          }
        }
        //Debug.Log( "For Char " + CharIndex + ", hasSinglePixel = " + hasSinglePixel + ", numColors = " + numColors + ", usedBackgroundColor = " + usedBackgroundColor );
        if ( ( hasSinglePixel )
        &&   ( numColors > 2 ) )
        {
          return false;
        }
        if ( ( hasSinglePixel )
        &&   ( numColors == 2 )
        &&   ( !usedBackgroundColor ) )
        {
          return false;
        }
        if ( ( !hasSinglePixel )
        &&   ( numColors > 4 ) )
        {
          return false;
        }
        if ( ( !hasSinglePixel )
        &&   ( numColors == 4 )
        &&   ( !usedBackgroundColor ) )
        {
          return false;
        }
        int     otherColorIndex = 16;
        if ( ( !hasSinglePixel )
        &&   ( ( ( numColors == 2 )
        &&       ( usedBackgroundColor ) )
        ||     ( numColors == 1 ) ) )
        {
          for ( int i = 0; i < 16; ++i )
          {
            if ( ( usedColor[i] )
            &&   ( i != m_Project.Colors.BackgroundColor ) )
            {
              otherColorIndex = i;
              break;
            }
          }
        }
        if ( ( !ForceMulticolor )
        // single pixel, and uses bg and one custom color
        &&   ( ( ( hasSinglePixel )
        ||       ( ( numColors == 2 )
        &&         ( usedBackgroundColor )
        &&         ( otherColorIndex < 8 ) ) )
        // not a single pixel, but only one color < 8 (e.g. full block)
        ||     ( ( !hasSinglePixel )
        &&       ( numColors == 1 )
        &&       ( !usedBackgroundColor )
        &&       ( otherColorIndex < 8 ) ) ) )
        {
          // eligible for single color
          int usedFreeColor = -1;
          for ( int i = 0; i < 16; ++i )
          {
            if ( usedColor[i] )
            {
              if ( i != m_Project.Colors.BackgroundColor )
              {
                if ( usedFreeColor != -1 )
                {
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
              int ColorIndex = (int)Image.GetPixel( x, y ) % 16;

              int BitPattern = 0;

              if ( ColorIndex != m_Project.Colors.BackgroundColor )
              {
                BitPattern = 1;
              }

              // noch nicht verwendete Farbe
              if ( BitPattern == 1 )
              {
                chosenCharColor = ColorIndex;
              }
              byte byteMask = (byte)( 255 - ( 1 << ( ( 7 - ( x % 8 ) ) ) ) );
              Buffer.SetU8At( y + x / 8, (byte)( ( Buffer.ByteAt( y + x / 8 ) & byteMask ) | ( BitPattern << ( ( 7 - ( x % 8 ) ) ) ) ) );
            }
          }
        }
        else
        {
          // multi color
          isMultiColor = true;
          int     usedMultiColors = 0;
          int     usedFreeColor = -1;
          for ( int i = 0; i < 16; ++i )
          {
            if ( usedColor[i] )
            {
              if ( ( i == m_Project.Colors.MultiColor1 )
              ||   ( i == m_Project.Colors.MultiColor2 )
              ||   ( i == m_Project.Colors.BackgroundColor ) )
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
            return false;
          }
          for ( int y = 0; y < Image.Height; ++y )
          {
            for ( int x = 0; x < Image.Width / 2; ++x )
            {
              int ColorIndex = (int)Image.GetPixel( 2 * x, y ) % 16;

              byte BitPattern = 0;

              if ( ColorIndex == m_Project.Colors.BackgroundColor )
              {
                BitPattern = 0x00;
              }
              else if ( ColorIndex == m_Project.Colors.MultiColor1 )
              {
                BitPattern = 0x01;
              }
              else if ( ColorIndex == m_Project.Colors.MultiColor2 )
              {
                BitPattern = 0x02;
              }
              else
              {
                // noch nicht verwendete Farbe
                chosenCharColor = usedFreeColor;
                BitPattern = 0x03;
              }
              byte byteMask = (byte)( 255 - ( 3 << ( ( 3 - ( x % 4 ) ) * 2 ) ) );
              Buffer.SetU8At( y + x / 4, (byte)( ( Buffer.ByteAt( y + x / 4 ) & byteMask ) | ( BitPattern << ( ( 3 - ( x % 4 ) ) * 2 ) ) ) );
            }
          }
        }
      }
      for ( int i = 0; i < Buffer.Length; ++i )
      {
        m_Project.Characters[CharIndex].Tile.Data.SetU8At( i, Buffer.ByteAt( i ) );
      }
      if ( chosenCharColor == -1 )
      {
        chosenCharColor = 0;
      }
      m_Project.Characters[CharIndex].Tile.CustomColor = (byte)chosenCharColor;
      if ( ( isMultiColor )
      &&   ( chosenCharColor < 8 ) )
      {
        m_Project.Characters[CharIndex].Tile.CustomColor = (byte)( chosenCharColor + 8 );
        if ( m_Project.Characters[CharIndex].Tile.Mode == GraphicTileMode.COMMODORE_HIRES )
        {
          m_Project.Characters[CharIndex].Tile.Mode = GraphicTileMode.COMMODORE_MULTICOLOR_CHARACTERS;
        }
        else if ( m_Project.Characters[CharIndex].Tile.Mode == GraphicTileMode.COMMODORE_HIRES_8X16 )
        {
          m_Project.Characters[CharIndex].Tile.Mode = GraphicTileMode.COMMODORE_MULTICOLOR_CHARACTERS_8X16;
        }
      }
      else
      {
        if ( m_Project.Characters[CharIndex].Tile.Mode == GraphicTileMode.COMMODORE_MULTICOLOR_CHARACTERS )
        {
          m_Project.Characters[CharIndex].Tile.Mode = GraphicTileMode.COMMODORE_HIRES;
        }
        else if ( m_Project.Characters[CharIndex].Tile.Mode == GraphicTileMode.COMMODORE_MULTICOLOR_CHARACTERS_8X16 )
        {
          m_Project.Characters[CharIndex].Tile.Mode = GraphicTileMode.COMMODORE_HIRES_8X16;
        }
      }

      RebuildAffectedChar( CharIndex );
      return true;
    }



    public void BackgroundColor()
    {
      _ColorSettingsDlg.SelectedColor = ColorType.BACKGROUND;
    }



    internal void CharsetUpdated( CharsetProject Project )
    {
      DoNotAddUndo = true;

      m_Project = Project;
      if ( ( comboCharsetMode.SelectedIndex != (int)m_Project.Mode )
      &&   ( (int)m_Project.Mode < comboCharsetMode.Items.Count ) )
      {
        comboCharsetMode.SelectedIndex = (int)m_Project.Mode;
      }
      panelCharacters.ItemWidth   = Lookup.CharacterWidthInPixel( m_Project.Characters[0].Tile.Mode );
      panelCharacters.ItemHeight  = Lookup.CharacterHeightInPixel( m_Project.Characters[0].Tile.Mode );

      ChangeColorSettingsDialog();
      ChangeColorPickerDialog();
      UpdatePalette();
      for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
      {
        RebuildCharImage( i );

        panelCharacters.Items[i].MemoryImage = m_Project.Characters[i].Tile.Image;
      }

      OnPaletteChanged();

      listCategories.Items.Clear();
      int categoryIndex = 0;
      foreach ( var category in m_Project.Categories )
      {
        ListViewItem    itemCat = new ListViewItem( category );
        itemCat.SubItems.Add( "0" );
        itemCat.Tag = categoryIndex;
        listCategories.Items.Add( itemCat );
        ++categoryIndex;
      }
      RefreshCategoryCounts();

      checkShowGrid.Checked = m_Project.ShowGrid;

      _ColorSettingsDlg.ColorChanged( ColorType.BACKGROUND, m_Project.Colors.BackgroundColor );
      _ColorSettingsDlg.ColorChanged( ColorType.MULTICOLOR_1, m_Project.Colors.MultiColor1 );
      _ColorSettingsDlg.ColorChanged( ColorType.MULTICOLOR_2, m_Project.Colors.MultiColor2 );
      _ColorSettingsDlg.ColorChanged( ColorType.BGCOLOR4, m_Project.Colors.BGColor4 );

      comboCategories.Items.Clear();
      foreach ( var category in m_Project.Categories )
      {
        comboCategories.Items.Add( category );
      }
      SelectCategory( m_Project.Characters[m_CurrentChar].Category );

      panelCharacters_SelectionChanged( null, null );

      panelCharacters.Invalidate();
      canvasEditor.Invalidate();
      RedrawColorPicker();

      DoNotAddUndo = false;
    }



    /*
    private void UpdateCustomColorCombo()
    {
      while ( comboCharColor.Items.Count > Lookup.NumberOfColorsInCharacter( m_Project.Mode ) )
      {
        comboCharColor.Items.RemoveAt( comboCharColor.Items.Count - 1 );
      }
      while ( comboCharColor.Items.Count < Lookup.NumberOfColorsInCharacter( m_Project.Mode ) )
      {
        comboCharColor.Items.Add( comboCharColor.Items.Count.ToString() );
      }
    }*/



    private void labelCharNo_Paint( object sender, PaintEventArgs e )
    {
      if ( Core == null )
      {
        return;
      }
      if ( !ConstantData.ScreenCodeToChar.ContainsKey( (byte)m_CurrentChar ) )
      {
        Debug.Log( "Missing char for " + m_CurrentChar );
      }
      else
      {
        try
        {
          if ( Core.Imaging != null )
          {
            int offset = (int)e.Graphics.MeasureString( labelCharNo.Text, labelCharNo.Font ).Width;
            e.Graphics.DrawString( "" + ConstantData.ScreenCodeToChar[(byte)m_CurrentChar].CharValue, 
                                   Core.Imaging.FontFromMachine( MachineType.C64 ), 
                                   System.Drawing.SystemBrushes.WindowText, offset + 10, 0 );
          }
        }
        catch ( Exception ex )
        {
          Debug.Log( "Exception during drawing char " + ex.ToString() );
          if ( !DesignMode )
          {
            Core.AddToOutput( "Exception during drawing char " + ex.ToString() );
          }
        }
      }
    }



    private void SelectCategory( int Category )
    {
      comboCategories.SelectedItem = m_Project.Categories[Category];
    }



    private void HandleMouseOnEditor( int X, int Y, MouseButtons Buttons )
    {
      int     charX = ( m_CharacterWidth * X ) / canvasEditor.ClientRectangle.Width;
      int     charY = ( m_CharacterHeight * Y ) / canvasEditor.ClientRectangle.Height;

      int     affectedCharIndex = m_CurrentChar;
      var     origAffectedChar = m_Project.Characters[m_CurrentChar];
      var     affectedChar = m_Project.Characters[m_CurrentChar];
      if ( m_Project.Mode == TextCharMode.COMMODORE_ECM )
      {
        affectedCharIndex %= 64;
        affectedChar = m_Project.Characters[affectedCharIndex];
      }

      var newColor = new Tupel<ColorType,byte>( _ColorSettingsDlg.SelectedColor, _ColorSettingsDlg.SelectedCustomColor );

      if ( ( Core.Settings.BehaviourRightClickIsBGColorPaint )
      &&   ( ( Buttons & MouseButtons.Right ) != 0 ) )
      {
        Buttons = MouseButtons.Left;
        newColor.first  = ColorType.BACKGROUND;
        newColor.second = 0;
      }

      if ( ( ( Buttons & MouseButtons.Middle ) != 0 )
      ||   ( ( ( Buttons & MouseButtons.Left ) != 0 )
      &&     ( ( Control.ModifierKeys & Keys.Shift ) != 0 ) ) )
      {
        Buttons &= ~MouseButtons.Left;

        if ( m_ButtonReleased )
        {
          // middle button toggles selected color
          _ColorSettingsDlg.ToggleSelectedColor();
          m_ButtonReleased = false;
        }
        return;
      }

      if ( ( Buttons & MouseButtons.Left ) != 0 )
      {
        var potentialUndo = new Undo.UndoCharacterEditorCharChange( this, m_Project, affectedCharIndex, 1 );
        if ( affectedChar.Tile.SetPixel( charX, charY, newColor ) )
        {
          if ( m_ButtonReleased )
          {
            UndoManager.AddUndoTask( potentialUndo );
            m_ButtonReleased = false;
          }
          RaiseModifiedEvent( new List<int>() { affectedCharIndex } );
          RebuildAffectedChar( affectedCharIndex );
          canvasEditor.Invalidate();
        }
      }
      else
      {
        m_ButtonReleased = true;
      }
      if ( ( Buttons & MouseButtons.Right ) != 0 )
      {
        var   pickedColor = affectedChar.Tile.GetPixel( charX, charY );

        _ColorSettingsDlg.SelectedColor       = pickedColor.first;
        _ColorSettingsDlg.SelectedCustomColor = pickedColor.second;

        RedrawColorPicker();
      }
    }



    private void ChangeColorPickerDialog()
    {
      if ( _ColorPickerDlg != null )
      {
        panelColorChooser.Controls.Remove( _ColorPickerDlg );
        _ColorPickerDlg.Dispose();
        _ColorPickerDlg = null;
      }

      switch ( m_Project.Mode )
      {
        case TextCharMode.X16_HIRES:
          _ColorPickerDlg = new ColorPickerX16( Core, m_Project, (ushort)m_CurrentChar, (byte)m_CurrentColor );
          break;
        case TextCharMode.VIC20_8X16:
          _ColorPickerDlg = new ColorPickerCommodoreVIC20X16( Core, m_Project, (ushort)m_CurrentChar, (byte)m_CurrentColor );
          break;
        case TextCharMode.NES:
          _ColorPickerDlg = new ColorPickerNES( Core, m_Project, (ushort)m_CurrentChar, (byte)m_CurrentColor );
          break;
        case TextCharMode.MEGA65_HIRES:
          _ColorPickerDlg = new ColorPickerMega65_32( Core, m_Project, (ushort)m_CurrentChar, (byte)m_CurrentColor );
          break;
        case TextCharMode.MEGA65_FCM:
        case TextCharMode.MEGA65_FCM_16BIT:
        case TextCharMode.MEGA65_NCM:
          return;
          //_ColorPickerDlg = new ColorPickerMega65_32( Core, m_Project, (ushort)m_CurrentChar, (byte)m_CurrentColor );
          //break;
        default:
          _ColorPickerDlg = new ColorPickerCommodore( Core, m_Project, (ushort)m_CurrentChar, (byte)m_CurrentColor );
          break;
      }
      _ColorPickerDlg.SelectedColorChanged += _ColorChooserDlg_SelectedColorChanged;
      _ColorPickerDlg.PaletteMappingSelected += _ColorChooserDlg_PaletteMappingSelected;
      _ColorPickerDlg.Redraw();
      panelColorChooser.Controls.Add( _ColorPickerDlg );
      DPIHandler.ResizeControlsForDPI( _ColorPickerDlg );
    }



    private void _ColorChooserDlg_PaletteMappingSelected( int PaletteMapping )
    {
      m_CurrentPaletteMapping = PaletteMapping;
    }



    private void _ColorChooserDlg_SelectedColorChanged( ushort Color )
    {
      m_CurrentColor = Color;
    }



    public void AddCategory( int Index, string Category )
    {
      m_Project.Categories.Insert( Index, Category );

      ListViewItem    itemNew = new ListViewItem( Category );
      itemNew.Tag = Index;
      itemNew.SubItems.Add( "0" );
      listCategories.Items.Insert( Index, itemNew );

      comboCategories.Items.Insert( Index, Category );

      RefreshCategoryCounts();
    }



    public void RemoveCategory( int CategoryIndex )
    {
      comboCategories.Items.RemoveAt( CategoryIndex );

      listCategories.Items.RemoveAt( CategoryIndex );

      m_Project.Categories.RemoveAt( CategoryIndex );

      for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
      {
        if ( m_Project.Characters[i].Category >= CategoryIndex )
        {
          --m_Project.Characters[i].Category;
        }
      }
      RefreshCategoryCounts();
    }



    private void RefreshCategoryCounts()
    {
      GR.Collections.Map<int,int>   catCounts = new GR.Collections.Map<int, int>();

      for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
      {
        catCounts[m_Project.Characters[i].Category]++;
      }

      int itemIndex = 0;
      foreach ( ListViewItem item in listCategories.Items )
      {
        item.SubItems[1].Text = catCounts[(int)item.Tag].ToString();
        item.Tag = itemIndex;
        ++itemIndex;
      }
    }



    private void comboCategories_SelectedIndexChanged( object sender, EventArgs e )
    {
      string    category = comboCategories.SelectedItem.ToString();

      int       categoryIndex = 0;
      foreach ( var categoryInfo in m_Project.Categories )
      {
        if ( categoryInfo == category )
        {
          break;
        }
        ++categoryIndex;
      }

      List<int>   selectedChars = panelCharacters.SelectedIndices;
      if ( selectedChars.Count == 0 )
      {
        selectedChars.Add( m_CurrentChar );
      }

      bool  firstChar = true;
      var   modifiedChars = new List<int>();
      foreach ( var selChar in selectedChars )
      {
        if ( ( categoryIndex != -1 )
        &&   ( m_Project.Characters[selChar].Category != categoryIndex ) )
        {
          UndoManager.AddUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, selChar, 1 ), firstChar );
          firstChar = false;

          m_Project.Characters[selChar].Category = categoryIndex;
          modifiedChars.Add( selChar );
        }
      }
      if ( modifiedChars.Count > 0 )
      {
        RaiseModifiedEvent( modifiedChars );
        RefreshCategoryCounts();
      }
    }



    private void radioBackground_CheckedChanged( object sender, EventArgs e )
    {
      m_CurrentColorType = ColorType.BACKGROUND;
    }



    private void radioMultiColor1_CheckedChanged( object sender, EventArgs e )
    {
      m_CurrentColorType = ColorType.MULTICOLOR_1;
    }



    private void radioMulticolor2_CheckedChanged( object sender, EventArgs e )
    {
      m_CurrentColorType = ColorType.MULTICOLOR_2;
    }



    private void radioCharColor_CheckedChanged( object sender, EventArgs e )
    {
      m_CurrentColorType = ColorType.CUSTOM_COLOR;
    }



    private void checkShowGrid_CheckedChanged( object sender, EventArgs e )
    {
      m_Project.ShowGrid = checkShowGrid.Checked;

      canvasEditor.Invalidate();
    }



    private void btnInvert_Click( DecentForms.ControlBase Sender )
    {
      Invert();
    }



    public void Invert()
    {
      List<int>     selectedChars = Uniquify( panelCharacters.SelectedIndices );

      UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        UndoManager.AddGroupedUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, index, 1 ) );

        for ( int y = 0; y < Lookup.NumBytesOfSingleCharacterBitmap( m_Project.Mode ); ++y )
        {
          byte result = (byte)( ~m_Project.Characters[index].Tile.Data.ByteAt( y ) );
          m_Project.Characters[index].Tile.Data.SetU8At( y, result );
        }

        RebuildAffectedChar( index );
      }
      canvasEditor.Invalidate();
      RaiseModifiedEvent( selectedChars );
    }



    private void RebuildAffectedChar( int index )
    {
      if ( Lookup.IsECMMode( m_Project.Mode ) )
      {
        for ( int i = 0; i < 4; ++i )
        {
          RebuildCharImage( i * 64 + index % 64 );
          panelCharacters.InvalidateItemRect( i * 64 + index % 64 );
        }
      }
      else
      {
        RebuildCharImage( index );
        panelCharacters.InvalidateItemRect( index );
      }
    }



    private void picturePlayground_MouseDown( object sender, MouseEventArgs e )
    {
      picturePlayground.Focus();
      HandleMouseOnPlayground( e.X, e.Y, e.Button );
    }



    private void picturePlayground_MouseMove( object sender, MouseEventArgs e )
    {
      MouseButtons    buttons = e.Button;
      if ( !picturePlayground.Focused )
      {
        buttons = 0;
      }
      HandleMouseOnPlayground( e.X, e.Y, buttons );
    }



    private void HandleMouseOnPlayground( int X, int Y, MouseButtons Buttons )
    {
      int     charX = (int)( ( 64 / m_CharacterWidth * 2 * X ) / picturePlayground.ClientRectangle.Width );
      int     charY = (int)( ( 64 / m_CharacterHeight * 2 * Y ) / picturePlayground.ClientRectangle.Height );

      if ( ( Buttons & MouseButtons.Left ) == 0 )
      {
        m_ButtonReleased = true;
      }

      if ( ( charX < 0 )
      ||   ( charX >= 16 )
      ||   ( charY < 0 )
      ||   ( charY >= 16 ) )
      {
        return;
      }

      if ( ( Buttons & MouseButtons.Left ) != 0 )
      {
        if ( m_Project.PlaygroundChars[charX + charY * 16] != (uint)( m_CurrentChar | ( m_CurrentColor << 16 ) ) )
        {
          UndoManager.AddUndoTask( new Undo.UndoCharacterEditorPlaygroundCharChange( this, m_Project, charX, charY ) );

          Displayer.CharacterDisplayer.DisplayChar( m_Project, m_CurrentChar, m_ImagePlayground, charX * m_CharacterWidth, charY * m_CharacterHeight, m_CurrentColor );
          RedrawPlayground();

          m_Project.PlaygroundChars[charX + charY * 16] = (uint)( m_CurrentChar | ( m_CurrentColor << 16 ) );
          RaiseModifiedEvent( new List<int>() );
        }
      }
      if ( ( Buttons & MouseButtons.Right ) != 0 )
      {
        m_CurrentChar = (ushort)( m_Project.PlaygroundChars[charX + charY * 16] & 0xffff );
        m_CurrentColor = (ushort)( m_Project.PlaygroundChars[charX + charY * 16] >> 16 );
        panelCharacters.SelectedIndex = m_CurrentChar;
        RedrawColorPicker();
      }
    }



    private void btnMirrorX_Click( DecentForms.ControlBase Sender )
    {
      MirrorX();
    }



    private void btnMirrorY_Click( DecentForms.ControlBase Sender )
    {
      MirrorY();
    }



    public void MirrorX()
    {
      List<int>     selectedChars = Uniquify( panelCharacters.SelectedIndices );

      UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        UndoManager.AddGroupedUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, index, 1 ) );

        var processedChar = m_Project.Characters[index];

        var tile = m_Project.Characters[index].Tile;
        for ( int y = 0; y < tile.Height; ++y )
        {
          for ( int x = 0; x < tile.Width / 2; x += Lookup.PixelWidth( tile.Mode ) )
          {
            var temp = tile.GetPixel( x, y );
            tile.SetPixel( x, y, tile.GetPixel( tile.Width - 1 - x, y ) );
            tile.SetPixel( tile.Width - 1 - x, y, temp );
          }
        }
        RebuildAffectedChar( index );
      }
      canvasEditor.Invalidate();
      RaiseModifiedEvent( selectedChars );
    }



    public void MirrorY()
    {
      List<int>     selectedChars = Uniquify( panelCharacters.SelectedIndices );

      UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        UndoManager.AddGroupedUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, index, 1 ) );

        var tile = m_Project.Characters[index].Tile;
        for ( int x = 0; x < tile.Width; x += Lookup.PixelWidth( tile.Mode ) )
        {
          for ( int y = 0; y < tile.Height / 2; ++y )
          {
            var temp = tile.GetPixel( x, y );
            tile.SetPixel( x, y, tile.GetPixel( x, tile.Height - 1 - y ) );
            tile.SetPixel( x, tile.Height - 1 - y, temp );
          }
        }
        RebuildAffectedChar( index );
      }
      canvasEditor.Invalidate();
      RaiseModifiedEvent( selectedChars );
    }



    private void btnRotateLeft_Click( DecentForms.ControlBase Sender )
    {
      RotateLeft();
    }



    public void RotateLeft()
    {
      List<int>     selectedChars = Uniquify( panelCharacters.SelectedIndices );

      UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        UndoManager.AddGroupedUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, index, 1 ) );

        var tile = m_Project.Characters[index].Tile;
        var resultTile = new GraphicTile( tile );
        int   sideSize = Math.Max( tile.Width, tile.Height );

        for ( int i = 0; i < sideSize; ++i )
        {
          for ( int j = 0; j < sideSize; ++j )
          {
            int sourceX = i;
            int sourceY = j;
            int targetX = j;
            int targetY = sideSize - 1 - i;

            resultTile.SetPixel( targetX, targetY, tile.GetPixel( sourceX, sourceY ) );
          }
        }
        resultTile.Data.CopyTo( tile.Data );
        RebuildAffectedChar( index );
      }
      canvasEditor.Invalidate();
      RaiseModifiedEvent( selectedChars );
    }



    private void btnRotateRight_Click( DecentForms.ControlBase Sender )
    {
      RotateRight();
    }



    public void RotateRight()
    {
      List<int>     selectedChars = Uniquify( panelCharacters.SelectedIndices );

      UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        UndoManager.AddGroupedUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, index, 1 ) );

        var tile = m_Project.Characters[index].Tile;
        var resultTile = new GraphicTile( tile );

        int   sideSize = Math.Max( tile.Width, tile.Height );

        for ( int i = 0; i < sideSize; ++i )
        {
          for ( int j = 0; j < sideSize; ++j )
          {
            int sourceX = i;
            int sourceY = j;
            int targetX = sideSize - 1 - j;
            int targetY = i;

            resultTile.SetPixel( targetX, targetY, tile.GetPixel( sourceX, sourceY ) );
          }
        }
        resultTile.Data.CopyTo( tile.Data );
        RebuildAffectedChar( index );
      }
      canvasEditor.Invalidate();
      RaiseModifiedEvent( selectedChars );
    }



    private void btnShiftDown_Click( DecentForms.ControlBase Sender )
    {
      ShiftDown();
    }



    public void ShiftDown()
    {
      List<int>     selectedChars = Uniquify( panelCharacters.SelectedIndices );

      UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        UndoManager.AddGroupedUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, index, 1 ) );

        var tile = m_Project.Characters[index].Tile;
        for ( int x = 0; x < tile.Width; x += Lookup.PixelWidth( tile.Mode ) )
        {
          var pixel = tile.GetPixel( x, tile.Height - 1 );
          for ( int y = 0; y < tile.Height - 1; ++y )
          {
            tile.SetPixel( x, tile.Height - 1 - y, tile.GetPixel( x, tile.Height - 1 - y - 1 ) );
          }
          tile.SetPixel( x, 0, pixel );
        }
        RebuildAffectedChar( index );
      }
      canvasEditor.Invalidate();
      RaiseModifiedEvent( selectedChars );
    }



    private void btnShiftLeft_Click( DecentForms.ControlBase Sender )
    {
      ShiftLeft();
    }



    private void btnShiftRight_Click( DecentForms.ControlBase Sender )
    {
      ShiftRight();
    }



    private void btnShiftUp_Click( DecentForms.ControlBase Sender )
    {
      ShiftUp();
    }



    public void ShiftUp()
    {
      List<int>     selectedChars = Uniquify( panelCharacters.SelectedIndices );

      UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        UndoManager.AddGroupedUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, index, 1 ) );

        var tile = m_Project.Characters[index].Tile;
        for ( int x = 0; x < tile.Width; x += Lookup.PixelWidth( tile.Mode ) )
        {
          var pixel = tile.GetPixel( x, 0 );
          for ( int y = 0; y < tile.Height - 1; ++y )
          {
            tile.SetPixel( x, y, tile.GetPixel( x, y + 1 ) );
          }
          tile.SetPixel( x, tile.Height - 1, pixel );
        }
        RebuildAffectedChar( index );
      }
      canvasEditor.Invalidate();
      RaiseModifiedEvent( selectedChars );
    }



    public void ShiftLeft()
    {
      List<int>     selectedChars = Uniquify( panelCharacters.SelectedIndices );

      UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        UndoManager.AddGroupedUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, index, 1 ) );

        var tile = m_Project.Characters[index].Tile;
        for ( int y = 0; y < tile.Height; ++y )
        {
          var temp = tile.GetPixel( 0, y );
          for ( int x = 0; x < tile.Width - 1; x += Lookup.PixelWidth( tile.Mode ) )
          {
            tile.SetPixel( x, y, tile.GetPixel( x + Lookup.PixelWidth( tile.Mode ), y ) );
          }
          tile.SetPixel( tile.Width - 1, y, temp );
        }
        RebuildAffectedChar( index );
      }
      canvasEditor.Invalidate();
      RaiseModifiedEvent( selectedChars );
    }



    private List<int> Uniquify( List<int> selectedIndices )
    {
      if ( !Lookup.IsECMMode( m_Project.Mode ) )
      {
        return selectedIndices;
      }
      var uniqueIndices = new HashSet<int>();
      foreach ( var index in selectedIndices )
      {
        uniqueIndices.Add( index % 64 );
      }
      return uniqueIndices.ToList();
    }



    public void ShiftRight()
    {
      List<int>     selectedChars = Uniquify( panelCharacters.SelectedIndices );

      UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        UndoManager.AddGroupedUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, index, 1 ) );

        var tile = m_Project.Characters[index].Tile;
        for ( int y = 0; y < tile.Height; ++y )
        {
          var temp = tile.GetPixel( tile.Width - 1, y );
          for ( int x = 0; x < tile.Width - 1; x += Lookup.PixelWidth( tile.Mode ) )
          {
            tile.SetPixel( tile.Width - 1 - x, y, tile.GetPixel( tile.Width - 1 - x - Lookup.PixelWidth( tile.Mode ), y ) );
          }
          tile.SetPixel( 0, y, temp );
        }
        RebuildAffectedChar( index );
      }
      canvasEditor.Invalidate();
      RaiseModifiedEvent( selectedChars );
    }



    private void comboColor_DrawItem( object sender, DrawItemEventArgs e )
    {
      ComboBox combo = (ComboBox)sender;

      Core?.Theming.DrawSingleColorComboBox( combo, e, m_Project.Colors.Palette );
    }



    private void comboMulticolor_DrawItem( object sender, DrawItemEventArgs e )
    {
      ComboBox combo = (ComboBox)sender;

      Core?.Theming.DrawMultiColorComboBox( combo, e, m_Project.Colors.Palette );
    }



    public void PlaygroundCharacterChanged( int X, int Y )
    {
      Displayer.CharacterDisplayer.DisplayChar( m_Project, (int)( m_Project.PlaygroundChars[X + Y * m_Project.PlaygroundWidth] & 0xffff ), m_ImagePlayground, X * m_CharacterWidth, Y * m_CharacterHeight, (int)( m_Project.PlaygroundChars[X + Y * m_Project.PlaygroundWidth] >> 16 ) );
      RedrawPlayground();
    }



    public void ColorsChanged()
    {
      if ( comboCharsetMode.SelectedIndex != (int)m_Project.Mode )
      {
        comboCharsetMode.SelectedIndex = (int)m_Project.Mode;
        ChangeColorSettingsDialog();
      }
      AdjustCharacterSizes();

      _ColorSettingsDlg.ColorChanged( ColorType.BACKGROUND, m_Project.Colors.BackgroundColor );
      _ColorSettingsDlg.ColorChanged( ColorType.MULTICOLOR_1, m_Project.Colors.MultiColor1 );
      _ColorSettingsDlg.ColorChanged( ColorType.MULTICOLOR_2, m_Project.Colors.MultiColor2 );
      _ColorSettingsDlg.ColorChanged( ColorType.BGCOLOR4, m_Project.Colors.BGColor4 );
      _ColorSettingsDlg.PaletteChanged( m_Project.Colors.Palette );
      _ColorSettingsDlg.Colors.Palettes = m_Project.Colors.Palettes;
      _ColorSettingsDlg.PalettesChanged();
      _ColorSettingsDlg.ActivePalette = m_Project.Colors.ActivePalette;

      OnPaletteChanged();

      for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
      {
        RebuildCharImage( i );
      }
      canvasEditor.Invalidate();
      panelCharacters.Invalidate();

      var modifiedChars = new List<int>();
      for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
      {
        modifiedChars.Add( i );
      }
      RaiseModifiedEvent( modifiedChars );
    }



    private void comboCharsetMode_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( DoNotUpdateFromControls )
      {
        return;
      }

      int origWidth   = Lookup.CharacterWidthInPixel( m_Project.Characters[0].Tile.Mode );
      int origHeight  = Lookup.CharacterHeightInPixel( m_Project.Characters[0].Tile.Mode );
      bool modeChanged = false;

      if ( m_Project.Mode != (TextCharMode)comboCharsetMode.SelectedIndex )
      {
        if ( !DoNotAddUndo )
        {
          UndoManager?.AddUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, 0, m_Project.TotalNumberOfCharacters ), true );
          UndoManager?.AddUndoTask( new Undo.UndoCharacterEditorValuesChange( this, m_Project ), false );
        }
        m_Project.Mode = (TextCharMode)comboCharsetMode.SelectedIndex;
        modeChanged = true;
      }

      AdjustCharacterSizes();

      UpdatePalette();
      ChangeColorPickerDialog();
      ChangeColorSettingsDialog();

      if ( m_Project.TotalNumberOfCharacters != Lookup.NumCharactersForMode( m_Project.Mode ) )
      {
        m_Project.TotalNumberOfCharacters = Lookup.NumCharactersForMode( m_Project.Mode );

        if ( m_Project.Characters.Count > m_Project.TotalNumberOfCharacters )
        {
          m_Project.Characters.RemoveRange( m_Project.TotalNumberOfCharacters, m_Project.Characters.Count - m_Project.TotalNumberOfCharacters );
          panelCharacters.Items.RemoveRange( m_Project.TotalNumberOfCharacters, panelCharacters.Items.Count - m_Project.TotalNumberOfCharacters );
        }

        int   startIndex = m_Project.Characters.Count;
        var   newItems = new List<GR.Forms.ImageListbox.ImageListItem>();

        panelCharacters.BeginUpdate();
        while ( m_Project.Characters.Count < m_Project.TotalNumberOfCharacters )
        {
          var newChar = new CharData()
          {
            Tile = new GraphicTile( m_CharacterWidth, m_CharacterHeight, Lookup.GraphicTileModeFromTextCharMode( m_Project.Mode, 1 ), m_Project.Characters[0].Tile.Colors )
          };
          newChar.Tile.CustomColor = 1;
          m_Project.Characters.Add( newChar );
          panelCharacters.Items.Add( "", newChar.Tile.Image );
        }
        panelCharacters.EndUpdate();
      }

      if ( ( modeChanged )
      ||   ( m_CharacterWidth != origWidth )
      ||   ( m_CharacterHeight != origHeight ) )
      {
        for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
        {
          m_Project.Characters[i].Tile.Mode = Lookup.GraphicTileModeFromTextCharMode( m_Project.Mode, m_Project.Characters[i].Tile.CustomColor );
          m_Project.Characters[i].Tile.Data.Resize( (uint)Lookup.NumBytesOfSingleCharacterBitmap( m_Project.Mode ) );

          m_Project.Characters[i].Tile.Width = m_CharacterWidth;
          m_Project.Characters[i].Tile.Height = m_CharacterHeight;
          m_Project.Characters[i].Tile.Image.Resize( m_CharacterWidth, m_CharacterHeight );
          if ( m_Project.Characters[i].Tile.CustomColor >= Lookup.NumberOfColorsInCharacter( m_Project.Mode ) )
          {
            m_Project.Characters[i].Tile.CustomColor %= (byte)Lookup.NumberOfColorsInCharacter( m_Project.Mode );
          }

          RebuildCharImage( i );
        }
      }
      panelCharacters.Invalidate();

      var modifiedChars = new List<int>();
      for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
      {
        modifiedChars.Add( i );
      }
      RaiseModifiedEvent( modifiedChars );
      canvasEditor.Invalidate();
    }



    private void ChangeColorSettingsDialog()
    {
      if ( _ColorSettingsDlg != null )
      {
        panelColorSettings.Controls.Remove( _ColorSettingsDlg );
        _ColorSettingsDlg.Dispose();
        _ColorSettingsDlg = null;
      }

      switch ( m_Project.Mode )
      {
        case TextCharMode.COMMODORE_ECM:
          _ColorSettingsDlg = new ColorSettingsECM( Core, m_Project.Colors, m_Project.Characters[m_CurrentChar].Tile.CustomColor );
          break;
        case TextCharMode.MEGA65_ECM:
          _ColorSettingsDlg = new ColorSettingsECMMega65( Core, m_Project.Colors, m_Project.Characters[m_CurrentChar].Tile.CustomColor );
          break;
        case TextCharMode.COMMODORE_HIRES:
        case TextCharMode.COMMODORE_128_VDC_HIRES:
          _ColorSettingsDlg = new ColorSettingsHiRes( Core, m_Project.Colors, m_Project.Characters[m_CurrentChar].Tile.CustomColor );
          break;
        case TextCharMode.MEGA65_NCM:
          _ColorSettingsDlg = new ColorSettingsNCMMega65( Core, m_Project.Colors, m_Project.Characters[m_CurrentChar].Tile.CustomColor );
          break;
        case TextCharMode.MEGA65_HIRES:
          _ColorSettingsDlg = new ColorSettingsHiResMega65( Core, m_Project.Colors, m_Project.Characters[m_CurrentChar].Tile.CustomColor );
          break;
        case TextCharMode.COMMODORE_MULTICOLOR:
          _ColorSettingsDlg = new ColorSettingsMCCharacter( Core, m_Project.Colors, m_Project.Characters[m_CurrentChar].Tile.CustomColor );
          break;
        case TextCharMode.VIC20:
        case TextCharMode.VIC20_8X16:
          _ColorSettingsDlg = new ColorSettingsVC20( Core, m_Project.Colors, m_Project.Characters[m_CurrentChar].Tile.CustomColor );
          break;
        case TextCharMode.MEGA65_FCM:
        case TextCharMode.MEGA65_FCM_16BIT:
          _ColorSettingsDlg = new ColorSettingsMega65( Core, m_Project.Colors, m_Project.Characters[m_CurrentChar].Tile.CustomColor );
          break;
        case TextCharMode.X16_HIRES:
          _ColorSettingsDlg = new ColorSettingsX16( Core, m_Project.Colors, m_Project.Characters[m_CurrentChar].Tile.CustomColor );
          break;
        case TextCharMode.NES:
          _ColorSettingsDlg = new ColorSettingsNES( Core, m_Project.Colors, m_Project.Characters[m_CurrentChar].Tile.CustomColor );
          break;
        default:
          Debug.Log( "ChangeColorSettingsDialog unsupported Mode " + m_Project.Mode );
          return;
      }
      panelColorSettings.Controls.Add( _ColorSettingsDlg );
      DPIHandler.ResizeControlsForDPI( _ColorSettingsDlg );
      _ColorSettingsDlg.SelectedColorChanged += _ColorSettingsDlg_SelectedColorChanged;
      _ColorSettingsDlg.ColorsModified += _ColorSettingsDlg_ColorsModified;
      _ColorSettingsDlg.ColorsExchanged += _ColorSettingsDlg_ColorsExchanged;
      _ColorSettingsDlg.PaletteModified += _ColorSettingsDlg_PaletteModified;
      _ColorSettingsDlg.PaletteMappingModified += _ColorSettingsDlg_PaletteMappingModified;
      _ColorSettingsDlg.PaletteSelected += _ColorSettingsDlg_PaletteSelected;
      _ColorSettingsDlg.PaletteMappingSelected += _ColorSettingsDlg_PaletteMappingSelected;
      _ColorSettingsDlg.SelectedCustomColorChanged += _ColorSettingsDlg_SelectedCustomColorChanged;

      _ColorSettingsDlg.SelectedColor = ColorType.CUSTOM_COLOR;
      _ColorSettingsDlg_SelectedColorChanged( _ColorSettingsDlg.SelectedColor );
    }



    private void _ColorSettingsDlg_PaletteMappingSelected( ColorSettings Colors )
    {
      m_Project.Colors.PaletteMappingIndex = Colors.PaletteMappingIndex;
      for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
      {
        RebuildCharImage( i );
        panelCharacters.InvalidateItemRect( i );
      }
      canvasEditor.Invalidate();

      OnPaletteChanged();
    }



    private void _ColorSettingsDlg_PaletteMappingModified( ColorSettings Colors )
    {
      _ColorSettingsDlg_PaletteModified( Colors, -1, null );
    }



    private void _ColorSettingsDlg_SelectedCustomColorChanged( int SelectedCustomColor )
    {
    }



    private void _ColorSettingsDlg_PaletteSelected( ColorSettings Colors )
    {
      if ( m_Project.Colors.ActivePalette != Colors.ActivePalette )
      {
        UndoManager.AddUndoTask( new Undo.UndoCharacterEditorValuesChange( this, m_Project ) );

        m_Project.Colors.ActivePalette = Colors.ActivePalette;
        for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
        {
          m_Project.Characters[i].Tile.Colors.ActivePalette = Colors.ActivePalette;
          RebuildCharImage( i );
        }

        var modifiedChars = new List<int>();
        for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
        {
          modifiedChars.Add( i );
        }
        RaiseModifiedEvent( modifiedChars );
      }
    }



    private void _ColorSettingsDlg_PaletteModified( ColorSettings Colors, int CustomColor, List<int> PaletteMapping )
    {
      UndoManager.AddUndoTask( new Undo.UndoCharacterEditorValuesChange( this, m_Project ) );

      m_Project.Colors = new ColorSettings( Colors );

      // make sure all chars still have valid palette indices!
      UndoManager.AddGroupedUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, 0, m_Project.TotalNumberOfCharacters ) );

      for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
      {
        var character = m_Project.Characters[i];

        character.Tile.Colors = new ColorSettings( Colors );
        character.Tile.Colors.ActivePalette = m_Project.Colors.ActivePalette;
        RebuildCharImage( i );
        panelCharacters.InvalidateItemRect( i );
      }
      canvasEditor.Invalidate();

      OnPaletteChanged();

      var modifiedChars = new List<int>();
      for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
      {
        modifiedChars.Add( i );
      }
      RaiseModifiedEvent( modifiedChars );
    }



    private void _ColorSettingsDlg_ColorsExchanged( ColorType Color1, ColorType Color2 )
    {
      var selectedIndices = panelCharacters.SelectedIndices;

      UndoManager.AddUndoTask( new Undo.UndoCharacterEditorExchangeColors( this, Color1, Color2, selectedIndices ) );

      ExchangeColors( Color1, Color2, selectedIndices );
    }



    public void ExchangeColors( ColorType Color1, ColorType Color2, List<int> AffectedChars )
    {
      foreach ( var index in AffectedChars )
      {
        var charInfo = m_Project.Characters[index];

        for ( int y = 0; y < charInfo.Tile.Height; ++y )
        {
          for ( int x = 0; x < charInfo.Tile.Width; x += Lookup.PixelWidth( charInfo.Tile.Mode ) )
          {
            ColorType pixel = charInfo.Tile.GetPixel( x, y ).first;

            if ( pixel == Color1 )
            {
              charInfo.Tile.SetPixel( x, y, new Tupel<ColorType, byte>( Color2, 0 ) );
            }
            else if ( pixel == Color2 )
            {
              charInfo.Tile.SetPixel( x, y, new Tupel<ColorType, byte>( Color1, 0 ) );
            }
          }
        }
        RebuildCharImage( index );

        panelCharacters.Invalidate();
      }
      canvasEditor.Invalidate();
    }



    private void _ColorSettingsDlg_ColorsModified( ColorType Color, ColorSettings Colors, int CustomColor )
    {
      if ( ( ( Color == ColorType.BACKGROUND )
      &&     ( m_Project.Colors.BackgroundColor != Colors.BackgroundColor ) )
      ||   ( ( Color == ColorType.MULTICOLOR_1 )
      &&     ( m_Project.Colors.MultiColor1 != Colors.MultiColor1 ) )
      ||   ( ( Color == ColorType.MULTICOLOR_2 )
      &&     ( m_Project.Colors.MultiColor2 != Colors.MultiColor2 ) )
      ||   ( ( Color == ColorType.BGCOLOR4 )
      &&     ( m_Project.Colors.BGColor4 != Colors.BGColor4 ) )
      ||   ( ( Color == ColorType.CUSTOM_COLOR )
      &&     ( m_Project.Characters[m_CurrentChar].Tile.CustomColor != CustomColor ) ) )
      {
        if ( Color != ColorType.CUSTOM_COLOR )
        {
          UndoManager.AddUndoTask( new Undo.UndoCharacterEditorValuesChange( this, m_Project ) );
        }
        switch ( Color )
        {
          case ColorType.BACKGROUND:
            m_Project.Colors.BackgroundColor = Colors.BackgroundColor;
            break;
          case ColorType.MULTICOLOR_1:
            m_Project.Colors.MultiColor1 = Colors.MultiColor1;
            break;
          case ColorType.MULTICOLOR_2:
            m_Project.Colors.MultiColor2 = Colors.MultiColor2;
            break;
          case ColorType.BGCOLOR4:
            m_Project.Colors.BGColor4 = Colors.BGColor4;
            break;
          case ColorType.CUSTOM_COLOR:
            {
              // TODO
              List<int>   selectedChars = panelCharacters.SelectedIndices;
              if ( selectedChars.Count == 0 )
              {
                selectedChars.Add( m_CurrentChar );
              }

              bool    modified = false;
              foreach ( int selChar in selectedChars )
              {
                if ( m_Project.Characters[selChar].Tile.CustomColor != CustomColor )
                {
                  {
                    UndoManager.AddUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, selChar, 1 ), modified == false );

                    m_Project.Characters[selChar].Tile.CustomColor = (byte)CustomColor;
                    m_Project.Characters[selChar].Tile.Mode = Lookup.GraphicTileModeFromTextCharMode( m_Project.Mode, m_Project.Characters[selChar].Tile.CustomColor );
                    RebuildCharImage( selChar );
                    modified = true;
                    panelCharacters.InvalidateItemRect( selChar );
                  }
                }
              }
              if ( modified )
              {
                canvasEditor.Invalidate();
              }
            }
            break;
        }

        for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
        {
          RebuildCharImage( i );
        }
        var modifiedChars = new List<int>();
        for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
        {
          modifiedChars.Add( i );
        }
        RaiseModifiedEvent( modifiedChars );
        canvasEditor.Invalidate();
        panelCharacters.Invalidate();
      }
    }



    private void _ColorSettingsDlg_SelectedColorChanged( ColorType Color )
    {
      m_CurrentColorType = Color;
    }



    private void UpdatePalette()
    {
      int numColors = Lookup.NumberOfColorsInCharacter( m_Project.Mode );

      if ( m_Project.Colors.Palette.NumColors != numColors )
      {
        // palette is not matching, create new
        m_Project.Colors.Palette = PaletteManager.PaletteFromMode( m_Project.Mode );
      }

      switch ( m_Project.Mode )
      {
        case TextCharMode.MEGA65_HIRES:
        case TextCharMode.MEGA65_ECM:
          break;
        case TextCharMode.COMMODORE_ECM:
        case TextCharMode.COMMODORE_HIRES:
        case TextCharMode.COMMODORE_MULTICOLOR:
          m_Project.Colors.Palettes[0] = Core.Imaging.PaletteFromMachine( MachineType.C64 );
          break;
        case TextCharMode.COMMODORE_128_VDC_HIRES:
          m_Project.Colors.Palettes[0] = Core.Imaging.PaletteFromMachine( MachineType.C128 );
          break;
        case TextCharMode.VIC20:
        case TextCharMode.VIC20_8X16:
          m_Project.Colors.Palettes[0] = Core.Imaging.PaletteFromMachine( MachineType.VIC20 );
          break;
        case TextCharMode.X16_HIRES:
          m_Project.Colors.Palettes[0] = Core.Imaging.PaletteFromMachine( MachineType.COMMANDER_X16 );
          break;
        case TextCharMode.MEGA65_FCM:
        case TextCharMode.MEGA65_FCM_16BIT:
        case TextCharMode.MEGA65_NCM:
          m_Project.Colors.Palettes[0] = Core.Imaging.PaletteFromMachine( MachineType.MEGA65 );
          break;
        case TextCharMode.NES:
          m_Project.Colors.Palettes[0] = Core.Imaging.PaletteFromMachine( MachineType.NES );
          break;
        default:
          Debug.Log( "UpdatePalette - unsupported TextCharMode " + m_Project.Mode );
          break;
      }

      OnPaletteChanged();
    }



    private void btnPasteFromClipboard_Click( DecentForms.ControlBase Sender )
    {
      if ( !Clipboard.ContainsImage() )
      {
        return;
      }
      IDataObject dataObj = Clipboard.GetDataObject();

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
        return;
      }

      PasteImage( "", imgClip, checkPasteMultiColor.Checked );
    }



    public void MultiColor2()
    {
      _ColorSettingsDlg.SelectedColor = ColorType.MULTICOLOR_2;
    }



    public void MultiColor1()
    {
      _ColorSettingsDlg.SelectedColor = ColorType.MULTICOLOR_1;
    }



    public void CustomColor()
    {
      _ColorSettingsDlg.SelectedColor = ColorType.CUSTOM_COLOR;
    }



    public void Next()
    {
      panelCharacters.SelectedIndex = ( panelCharacters.SelectedIndex + 1 ) % 256;
    }



    public void Previous()
    {
      panelCharacters.SelectedIndex = ( panelCharacters.SelectedIndex + 256 - 1 ) % 256;
    }



    private void btnClear_Click( DecentForms.ControlBase Sender )
    {
      bool  wasModified = false;
      var   selectedChars = panelCharacters.SelectedIndices;
      bool  firstUndoStep = true;

      DoNotUpdateFromControls = true;

      foreach ( int i in selectedChars )
      {
        wasModified = true;

        UndoManager.AddUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, i, 1 ), firstUndoStep );
        firstUndoStep = false;

        for ( int j = 0; j < m_Project.Characters[i].Tile.Data.Length; ++j )
        {
          m_Project.Characters[i].Tile.Data.SetU8At( j, 0 );
        }
        RebuildAffectedChar( i );
      }
      if ( wasModified )
      {
        canvasEditor.Invalidate();
        RaiseModifiedEvent( selectedChars );
      }
      DoNotUpdateFromControls = false;
    }



    private void btnMoveSelectionToTarget_Click( DecentForms.ControlBase Sender )
    {
      int targetIndex = GR.Convert.ToI32( editMoveTargetIndex.Text );

      var selection = panelCharacters.SelectedIndices;
      if ( selection.Count == 0 )
      {
        return;
      }
      if ( targetIndex + selection.Count > m_Project.TotalNumberOfCharacters )
      {
        MessageBox.Show( "Not enough chars for selection starting at the given index!", "Can't move selection" );
        return;
      }

      int numChars = m_Project.TotalNumberOfCharacters;
      if ( Lookup.IsECMMode( m_Project.Mode ) )
      {
        numChars = 64;
      }
      int[]   charMapNewToOld = new int[m_Project.TotalNumberOfCharacters];
      int[]   charMapOldToNew = new int[m_Project.TotalNumberOfCharacters];
      for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
      {
        charMapNewToOld[i] = -1;
        charMapOldToNew[i] = -1;
      }

      int     insertIndex = targetIndex;
      foreach ( var entry in selection )
      {
        if ( Lookup.IsECMMode( m_Project.Mode ) )
        {
          for ( int i = 0; i < 4; ++i )
          {
            charMapNewToOld[i * 64 + insertIndex % 64]  = i * 64 + entry % 64;
            charMapOldToNew[i * 64 + entry % 64]        = i * 64 + insertIndex % 64;
          }
        }
        else
        {
          charMapNewToOld[insertIndex] = entry;
          charMapOldToNew[entry] = insertIndex;
        }
        ++insertIndex;
      }

      // now fill all other entries
      byte    insertCharIndex = 0;
      int     charPos = 0;
      while ( charPos < numChars )
      {
        // already inserted, skip
        if ( charMapNewToOld[charPos] != -1 )
        {
          ++charPos;
          continue;
        }
        while ( selection.Contains( insertCharIndex ) )
        {
          ++insertCharIndex;
        }
        if ( Lookup.IsECMMode( m_Project.Mode ) )
        {
          for ( int i = 0; i < 4; ++i )
          {
            charMapNewToOld[i * 64 + charPos]         = i * 64 + insertCharIndex;
            charMapOldToNew[i * 64 + insertCharIndex] = i * 64 + charPos;
          }
        }
        else
        {
          charMapNewToOld[charPos] = insertCharIndex;
          charMapOldToNew[insertCharIndex] = charPos;
        }
        ++charPos;
        ++insertCharIndex;
      }

      UndoManager.AddUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, 0, m_Project.TotalNumberOfCharacters ) );

      // ..and charset
      List<CharData>    origCharData = new List<CharData>();
      List<GR.Forms.ImageListbox.ImageListItem>    origListItems = new List<GR.Forms.ImageListbox.ImageListItem>();
      List<GR.Forms.ImageListbox.ImageListItem>    origListItems2 = new List<GR.Forms.ImageListbox.ImageListItem>();

      for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
      {
        origCharData.Add( m_Project.Characters[i] );
        origListItems.Add( panelCharacters.Items[i] );
      }

      for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
      {
        m_Project.Characters[i] = origCharData[charMapNewToOld[i]];
        panelCharacters.Items[i] = origListItems[charMapNewToOld[i]];
      }
      panelCharacters.Invalidate();

      RedrawPlayground();
      canvasEditor.Invalidate();
      RedrawColorPicker();

      RaiseCharactersShiftedEvent( charMapOldToNew, charMapNewToOld );

      var modifiedChars = new List<int>();
      for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
      {
        modifiedChars.Add( i );
      }
      RaiseModifiedEvent( modifiedChars );
    }



    private void canvasEditor_Paint( object sender, PaintEventArgs e )
    {
      e.Graphics.FillRectangle( System.Drawing.Brushes.Black, 0, 0, canvasEditor.ClientSize.Width, canvasEditor.ClientSize.Height );

      Displayer.CharacterDisplayer.DisplayChar( m_Project, m_CurrentChar, new CustomDrawControlContext( e.Graphics, canvasEditor.ClientSize.Width, canvasEditor.ClientSize.Height )
      {
        Palette = m_Project.Colors.Palette
      } );

      if ( m_CurrentChar >= m_Project.Characters.Count )
      {
        Debug.Log( $"canvasEditor_Paint, trying to paint character {m_CurrentChar} of {m_Project.Characters.Count}" );
        return;
      }

      int numPixelWidth = Lookup.CharacterWidthInPixel( Lookup.GraphicTileModeFromTextCharMode( m_Project.Mode, m_Project.Characters[m_CurrentChar].Tile.CustomColor ) );
      int numPixelHeight = Lookup.CharacterHeightInPixel( Lookup.GraphicTileModeFromTextCharMode( m_Project.Mode, m_Project.Characters[m_CurrentChar].Tile.CustomColor ) );

      if ( m_Project.ShowGrid )
      {
        if ( ( ( m_Project.Mode == TextCharMode.COMMODORE_MULTICOLOR )
        ||     ( m_Project.Mode == TextCharMode.MEGA65_NCM )
        ||     ( m_Project.Mode == TextCharMode.VIC20 ) )
        &&   ( m_Project.Characters[m_CurrentChar].Tile.CustomColor >= 8 ) )
        {
          for ( int i = 0; i < numPixelWidth / 2; ++i )
          {
            e.Graphics.DrawLine( System.Drawing.Pens.White,
                                  ( i * canvasEditor.ClientSize.Width ) / ( numPixelWidth / 2 ), 0,
                                  ( i * canvasEditor.ClientSize.Width ) / ( numPixelWidth / 2 ), canvasEditor.ClientSize.Height - 1 );
          }
          for ( int i = 0; i < numPixelHeight; ++i )
          {
            e.Graphics.DrawLine( System.Drawing.Pens.White,
                                  0, ( i * canvasEditor.ClientSize.Height ) / numPixelHeight,
                                  canvasEditor.ClientSize.Width - 1, ( i * canvasEditor.ClientSize.Height ) / numPixelHeight );
          }
        }
        else
        {
          for ( int i = 0; i < numPixelWidth; ++i )
          {
            e.Graphics.DrawLine( System.Drawing.Pens.White,
                                  ( i * canvasEditor.ClientSize.Width ) / numPixelWidth, 0,
                                  ( i * canvasEditor.ClientSize.Width ) / numPixelWidth, canvasEditor.ClientSize.Height - 1 );

            e.Graphics.DrawLine( System.Drawing.Pens.White,
                                  0, ( i * canvasEditor.ClientSize.Height ) / numPixelHeight,
                                  canvasEditor.ClientSize.Width - 1, ( i * canvasEditor.ClientSize.Height ) / numPixelHeight );
          }
        }
      }
    }



    private void canvasEditor_MouseDown( object sender, MouseEventArgs e )
    {
      canvasEditor.Focus();
      HandleMouseOnEditor( e.X, e.Y, e.Button );
    }



    private void canvasEditor_MouseMove( object sender, MouseEventArgs e )
    {
      MouseButtons    buttons = e.Button;
      if ( !canvasEditor.Focused )
      {
        buttons = 0;
      }
      HandleMouseOnEditor( e.X, e.Y, buttons );
    }



    private void AdjustCharacterSizes()
    {
      m_CharacterWidth = Lookup.CharacterWidthInPixel( Lookup.GraphicTileModeFromTextCharMode( m_Project.Mode, 0 ) );
      m_CharacterHeight = Lookup.CharacterHeightInPixel( Lookup.GraphicTileModeFromTextCharMode( m_Project.Mode, 0 ) );

      // adjust aspect ratio of the editor
      int   biggerSize = Math.Max( m_CharacterWidth, m_CharacterHeight );

      canvasEditor.Size = new System.Drawing.Size( m_CharacterWidth * m_CharacterEditorOrigWidth / biggerSize,
                                                    m_CharacterHeight * m_CharacterEditorOrigHeight / biggerSize );

      panelCharacters.ItemWidth = m_CharacterWidth;
      panelCharacters.ItemHeight = m_CharacterHeight;
      //panelCharacters.SetDisplaySize( panelCharacters.ClientSize.Width / 2, panelCharacters.ClientSize.Height / 2 );
    }



    private void btnAddCategory_Click( DecentForms.ControlBase Sender )
    {
      string    newCategory = editCategoryName.Text;

      UndoManager.AddUndoTask( new Undo.UndoCharsetAddCategory( this, m_Project, m_Project.Categories.Count ) );

      AddCategory( m_Project.Categories.Count, newCategory );

      RaiseModifiedEvent( new List<int>() );
    }



    private void btnDelete_Click( DecentForms.ControlBase Sender )
    {
      if ( listCategories.SelectedItems.Count == 0 )
      {
        return;
      }
      int category = (int)listCategories.SelectedItems[0].Tag;

      UndoManager.AddUndoTask( new Undo.UndoCharsetRemoveCategory( this, m_Project, category ) );

      RemoveCategory( category );
      RaiseModifiedEvent( new List<int>() );
    }



    private void btnCollapseCategory_Click( DecentForms.ControlBase Sender )
    {
      // collapses similar looking characters
      if ( listCategories.SelectedItems.Count == 0 )
      {
        return;
      }

      UndoManager.AddUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, 0, m_Project.TotalNumberOfCharacters ) );

      int category = (int)listCategories.SelectedItems[0].Tag;
      int collapsedCount = 0;

      for ( int i = 0; i < m_Project.TotalNumberOfCharacters - collapsedCount; ++i )
      {
        if ( m_Project.Characters[i].Category == category )
        {
          for ( int j = i + 1; j < m_Project.TotalNumberOfCharacters - collapsedCount; ++j )
          {
            if ( m_Project.Characters[j].Category == category )
            {
              if ( ( m_Project.Characters[i].Tile.Data.Compare( m_Project.Characters[j].Tile.Data ) == 0 )
              &&   ( m_Project.Characters[i].Tile.CustomColor == m_Project.Characters[j].Tile.CustomColor ) )
              {
                // collapse!
                //Debug.Log( "Collapse " + j.ToString() + " into " + i.ToString() );
                for ( int l = j; l < m_Project.TotalNumberOfCharacters - 1 - collapsedCount; ++l )
                {
                  m_Project.Characters[l].Tile.Data = m_Project.Characters[l + 1].Tile.Data;
                  m_Project.Characters[l].Tile.CustomColor = m_Project.Characters[l + 1].Tile.CustomColor;
                  m_Project.Characters[l].Category = m_Project.Characters[l + 1].Category;
                }
                for ( int l = 0; l < 8; ++l )
                {
                  m_Project.Characters[m_Project.TotalNumberOfCharacters - 1 - collapsedCount].Tile.Data.SetU8At( l, 0 );
                }
                m_Project.Characters[m_Project.TotalNumberOfCharacters - 1 - collapsedCount].Tile.CustomColor = 0;
                ++collapsedCount;
                --j;
                continue;
              }
            }
          }
        }
      }
      if ( collapsedCount > 0 )
      {
        CharsetUpdated( m_Project );
        RefreshCategoryCounts();

        var modifiedChars = new List<int>();
        for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
        {
          modifiedChars.Add( i );
        }
        RaiseModifiedEvent( modifiedChars );
      }
    }



    private void btnSortCategories_Click( DecentForms.ControlBase Sender )
    {
      UndoManager.AddUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, 0, m_Project.TotalNumberOfCharacters ) );

      int[]   charMapNewToOld = new int[256];
      int[]   charMapOldToNew = new int[256];

      // resorts characters by category
      List<Formats.CharData>    newList = new List<RetroDevStudio.Formats.CharData>();
      for ( int j = 0; j < m_Project.Categories.Count; ++j )
      {
        for ( int i = 0; i < 256; ++i )
        {
          if ( m_Project.Characters[i].Category == j )
          {
            charMapOldToNew[i] = newList.Count;
            charMapNewToOld[newList.Count] = i;

            newList.Add( m_Project.Characters[i] );
          }
        }
      }

      RaiseCharactersShiftedEvent( charMapOldToNew, charMapNewToOld );

      m_Project.Characters = newList;
      CharsetUpdated( m_Project );
      RefreshCategoryCounts();

      var modifiedChars = new List<int>();
      for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
      {
        modifiedChars.Add( i );
      }

      RaiseModifiedEvent( modifiedChars );
    }



    private void btnReseatCategory_Click( DecentForms.ControlBase Sender )
    {
      if ( listCategories.SelectedItems.Count == 0 )
      {
        return;
      }

      UndoManager.AddUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, 0, m_Project.TotalNumberOfCharacters ) );

      int category = (int)listCategories.SelectedItems[0].Tag;
      int catTarget = GR.Convert.ToI32( editCollapseIndex.Text );
      int catTargetStart = catTarget;

      List<Formats.CharData> newList = new List<RetroDevStudio.Formats.CharData>();

      int[]   charMapNewToOld = new int[m_Project.TotalNumberOfCharacters];
      int[]   charMapOldToNew = new int[m_Project.TotalNumberOfCharacters];

      int lastIndex = 0;

      // fill in front of target
      if ( catTargetStart > 0 )
      {
        for ( int j = 0; j < m_Project.TotalNumberOfCharacters; ++j )
        {
          if ( m_Project.Characters[j].Category != category )
          {
            charMapOldToNew[j] = newList.Count;
            charMapNewToOld[newList.Count] = j;

            newList.Add( m_Project.Characters[j] );
            lastIndex = j + 1;
            if ( newList.Count == catTargetStart )
            {
              break;
            }
          }
        }
      }

      // fill at target
      for ( int j = 0; j < m_Project.TotalNumberOfCharacters; ++j )
      {
        if ( m_Project.Characters[j].Category == category )
        {
          charMapOldToNew[j] = newList.Count;
          charMapNewToOld[newList.Count] = j;
          newList.Add( m_Project.Characters[j] );
        }
      }

      // fill after target
      for ( int j = lastIndex; j < m_Project.TotalNumberOfCharacters; ++j )
      {
        if ( m_Project.Characters[j].Category != category )
        {
          charMapOldToNew[j] = newList.Count;
          charMapNewToOld[newList.Count] = j;
          newList.Add( m_Project.Characters[j] );
        }
      }
      m_Project.Characters = newList;
      RaiseCharactersShiftedEvent( charMapOldToNew, charMapNewToOld );
      CharsetUpdated( m_Project );
      RefreshCategoryCounts();

      var modifiedChars = new List<int>();
      for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
      {
        modifiedChars.Add( i );
      }
      RaiseModifiedEvent( modifiedChars );
    }



    private void editCategoryName_TextChanged( object sender, EventArgs e )
    {
      bool    validCategory = ( editCategoryName.Text.Length > 0 );
      foreach ( string category in m_Project.Categories )
      {
        if ( category == editCategoryName.Text )
        {
          validCategory = false;
          break;
        }
      }
      btnAddCategory.Enabled = validCategory;
    }



    private void listCategories_SelectedIndexChanged( object sender, EventArgs e )
    {
      bool    deleteAllowed = ( listCategories.SelectedItems.Count > 0 );
      bool    collapseAllowed = ( listCategories.SelectedItems.Count > 0 );
      if ( deleteAllowed )
      {
        if ( (int)listCategories.SelectedItems[0].Tag == 0 )
        {
          deleteAllowed = false;
        }
      }
      btnDelete.Enabled = deleteAllowed;
      btnCollapseCategory.Enabled = collapseAllowed;
      btnReseatCategory.Enabled = collapseAllowed;

      btnMoveCategoryDown.Enabled = ( ( listCategories.Items.Count > 1 )
                                   && ( listCategories.SelectedIndices.Count > 0 )
                                   && ( listCategories.SelectedIndices[0] + 1 < listCategories.Items.Count ) );
      btnMoveCategoryUp.Enabled = ( ( listCategories.Items.Count > 1 )
                                 && ( listCategories.SelectedIndices.Count > 0 )
                                 && ( listCategories.SelectedIndices[0] > 0 ) );
    }



    private void btnMoveCategoryUp_Click( DecentForms.ControlBase Sender )
    {
      if ( ( listCategories.Items.Count > 1 )
      &&   ( listCategories.SelectedIndices.Count > 0 )
      &&   ( listCategories.SelectedIndices[0] > 0 ) )
      {
        int     index1 = listCategories.SelectedIndices[0];

        UndoManager.AddUndoTask( new Undo.UndoCharacterEditorSwapCategories( this, m_Project, index1 - 1, index1 ) );

        SwapCategories( index1, index1 - 1 );

        var modifiedChars = new List<int>();
        for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
        {
          modifiedChars.Add( i );
        }
        RaiseModifiedEvent( modifiedChars );
      }
    }



    public void SwapCategories( int CategoryIndex1, int CategoryIndex2 )
    {
      string    category = m_Project.Categories[CategoryIndex1];

      m_Project.Categories.RemoveAt( CategoryIndex1 );
      m_Project.Categories.Insert( CategoryIndex2, category );

      // swap character categories as well
      for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
      {
        if ( m_Project.Characters[i].Category == CategoryIndex1 )
        {
          m_Project.Characters[i].Category = CategoryIndex2;
        }
        else if ( m_Project.Characters[i].Category == CategoryIndex2 )
        {
          m_Project.Characters[i].Category = CategoryIndex1;
        }
      }

      var item = (ListViewItem)listCategories.SelectedItems[0];
      listCategories.Items.RemoveAt( CategoryIndex1 );
      listCategories.Items.Insert( CategoryIndex2, item );
    }



    private void btnMoveCategoryDown_Click( DecentForms.ControlBase Sender )
    {
      if ( ( listCategories.Items.Count > 1 )
      &&   ( listCategories.SelectedIndices.Count > 0 )
      &&   ( listCategories.SelectedIndices[0] + 1 < listCategories.Items.Count ) )
      {
        int     index1 = listCategories.SelectedIndices[0];

        UndoManager.AddUndoTask( new Undo.UndoCharacterEditorSwapCategories( this, m_Project, index1 + 1, index1 ) );

        SwapCategories( index1, index1 + 1 );

        var modifiedChars = new List<int>();
        for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
        {
          modifiedChars.Add( i );
        }
        RaiseModifiedEvent( modifiedChars );
      }
    }



    private void comboCharColor_DrawItem( object sender, DrawItemEventArgs e )
    {
      ComboBox combo = (ComboBox)sender;

      if ( ( m_Project.Mode == TextCharMode.COMMODORE_ECM )
      ||   ( m_Project.Mode == TextCharMode.COMMODORE_HIRES )
      ||   ( m_Project.Mode == TextCharMode.MEGA65_FCM )
      ||   ( m_Project.Mode == TextCharMode.MEGA65_FCM_16BIT ) )
      {
        Core?.Theming.DrawSingleColorComboBox( combo, e, m_Project.Colors.Palette );
      }
      else
      {
        Core?.Theming.DrawMultiColorComboBox( combo, e, m_Project.Colors.Palette );
      }
    }



    private void OnPaletteChanged()
    {
      PaletteManager.ApplyPalette( picturePlayground.DisplayPage, m_Project.Colors.Palette );
      PaletteManager.ApplyPalette( panelCharacters.DisplayPage, m_Project.Colors.Palette );
      PaletteManager.ApplyPalette( m_ImagePlayground, m_Project.Colors.Palette );
      panelCharacters.Items.Clear();

      for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
      {
        m_Project.Characters[i].Tile.Colors.ActivePalette = m_Project.Colors.ActivePalette;

        RebuildCharImage( i );
        panelCharacters.Items.Add( i.ToString(), m_Project.Characters[i].Tile.Image );
      }

      panelCharacters.Invalidate();
      canvasEditor.Invalidate();
    }



    private void panelCharColors_PostPaint( FastImage TargetBuffer )
    {
      if ( m_Project.Mode == TextCharMode.X16_HIRES )
      {
        return;
      }

      int     x1 = m_CurrentColor * TargetBuffer.Width / _NumColorsInColorSelector;
      int     x2 = ( m_CurrentColor + 1 ) * TargetBuffer.Width / _NumColorsInColorSelector;

      if ( Core != null )
      {
        uint  selColor = Core.Settings.FGColor( ColorableElement.SELECTION_FRAME );

        TargetBuffer.Rectangle( x1, 0, x2 - x1, TargetBuffer.Height, selColor );
      }
    }



    public void Copy()
    {
      CopyToClipboard();
    }



    public void Paste()
    {
      PasteClipboardImageToChar();
    }



    private void btnHighlightDuplicates_Click( DecentForms.ControlBase Sender )
    {
      var duplicateGroups = new Map<ByteBuffer,int>();
      var itemGroup       = new Map<int,int>();

      panelCharacters.BeginUpdate();
      bool  hasHighlight = false;
      for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
      {
        if ( panelCharacters.Items[i].Highlighted )
        {
          hasHighlight = true;
          panelCharacters.Items[i].Highlighted = false;
        }
      }
      if ( hasHighlight )
      {
        panelCharacters.EndUpdate();
        return;
      }

      for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
      {
        panelCharacters.Items[i].Highlighted = false;
      }
      for ( int i = 0; i < m_Project.TotalNumberOfCharacters - 1; ++i )
      {
        for ( int j = i + 1; j < m_Project.TotalNumberOfCharacters; ++j )
        {
          if ( m_Project.Characters[i].Tile.Data == m_Project.Characters[j].Tile.Data )
          {
            int duplicateGroup = -1;
            if ( duplicateGroups.TryGetValue( m_Project.Characters[i].Tile.Data, out duplicateGroup ) )
            {
              itemGroup.Add( i, duplicateGroup );
              itemGroup.Add( j, duplicateGroup );
            }
            else
            {
              duplicateGroup = duplicateGroups.Count;
              itemGroup.Add( i, duplicateGroup );
              itemGroup.Add( j, duplicateGroup );
              duplicateGroups.Add( m_Project.Characters[i].Tile.Data, duplicateGroup );
            }

            panelCharacters.Items[i].SetHighlightGroup( duplicateGroup );
            panelCharacters.Items[j].SetHighlightGroup( duplicateGroup );
          }
        }
      }
      panelCharacters.EndUpdate();
    }



    private void editMoveTargetIndex_TextChanged( object sender, EventArgs e )
    {
      ValidateMoveToTarget();
    }



  }
}
