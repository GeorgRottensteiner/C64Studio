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



namespace RetroDevStudio.Controls
{
  public partial class CharacterEditor : UserControl
  {
    public delegate void ModifiedHandler( List<int> ModifiedCharacters );
    public delegate void CharsetShiftedHandler( int[] OldToNew, int[] NewToOld );

    public event ModifiedHandler        Modified;
    public event CharsetShiftedHandler  CharactersShifted;

    private int                         m_CurrentChar = 0;
    private int                         m_CurrentColor = 1;
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
      panelCharColors.DisplayPage.Create( 128, 8, GR.Drawing.PixelFormat.Format32bppRgb );
      m_ImagePlayground.Create( 256, 256, GR.Drawing.PixelFormat.Format32bppRgb );

      m_Project.Colors.Palette = PaletteManager.PaletteFromMachine( MachineType.C64 );

      ChangeColorSettingsDialog();
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

      RedrawColorChooser();

      panelCharacters.KeyDown += new KeyEventHandler( HandleKeyDown );
      canvasEditor.PreviewKeyDown += new PreviewKeyDownEventHandler( canvasEditor_PreviewKeyDown );
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



    private void RedrawColorChooser()
    {
      for ( byte i = 0; i < _NumColorsInColorSelector; ++i )
      {
        Displayer.CharacterDisplayer.DisplayChar( m_Project, m_Project.Colors.Palette, m_CurrentChar, panelCharColors.DisplayPage, i * 8, 0, i );
      }
      panelCharColors.Invalidate();
    }



    void canvasEditor_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
    {
      KeyEventArgs ke = new KeyEventArgs( e.KeyData );
      HandleKeyDown( sender, ke );
    }



    void HandleKeyDown( object sender, KeyEventArgs e )
    {
      if ( ( e.Modifiers == Keys.Control )
      &&   ( e.KeyCode == Keys.C ) )
      {
        CopyToClipboard();
      }
      else if ( ( e.Modifiers == Keys.Control )
      &&        ( e.KeyCode == Keys.V ) )
      {
        PasteClipboardImageToChar();
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
        return canvasEditor.Focused;
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
        var entry = new ClipboardImageList.Entry();
        var character = m_Project.Characters[index];

        entry.Tile        = character.Tile;
        entry.Index       = index;

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

          if ( pastePos >= m_Project.Characters.Count )
          {
            break;
          }

          modifiedChars.Add( pastePos );

          UndoManager.AddUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, pastePos, 1 ), firstEntry );
          firstEntry = false;

          var targetTile = m_Project.Characters[pastePos].Tile;

          if ( ( ( entry.Tile.Mode == GraphicTileMode.COMMODORE_HIRES )
          ||     ( entry.Tile.Mode == GraphicTileMode.COMMODORE_MULTICOLOR ) )
          &&   ( ( targetTile.Mode == GraphicTileMode.COMMODORE_HIRES )
          ||     ( targetTile.Mode == GraphicTileMode.COMMODORE_MULTICOLOR ) ) )
          {
            // can copy mode
            targetTile.Mode = entry.Tile.Mode;
          }

          int copyWidth = Math.Min( 8, entry.Tile.Width );
          int copyHeight = Math.Min( 8, entry.Tile.Height );

          for ( int x = 0; x < copyWidth; x += Lookup.PixelWidth( targetTile.Mode ) )
          {
            for ( int y = 0; y < copyHeight; ++y )
            {
              targetTile.SetPixel( x, y, entry.Tile.MapPixelColor( x, y, targetTile ) );
            }
          }
          targetTile.CustomColor = entry.Tile.CustomColor;

          RebuildCharImage( pastePos );
          panelCharacters.InvalidateItemRect( pastePos );

          if ( pastePos == m_CurrentChar )
          {
            _ColorSettingsDlg.CustomColor = m_Project.Characters[pastePos].Tile.CustomColor;
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
            m_Project.Characters[pastePos].Tile.CustomColor = memIn.ReadInt32();

            // TODO - mapping the color would be wiser
            if ( m_Project.Characters[pastePos].Tile.CustomColor >= m_Project.Colors.Palette.NumColors )
            {
              m_Project.Characters[pastePos].Tile.CustomColor %= m_Project.Colors.Palette.NumColors;
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
            && ( height == 21 ) )
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

            RebuildCharImage( pastePos );
            panelCharacters.InvalidateItemRect( pastePos );

            if ( pastePos == m_CurrentChar )
            {
              _ColorSettingsDlg.CustomColor = m_Project.Characters[pastePos].Tile.CustomColor;
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



    private void btnCopy_Click( object sender, EventArgs e )
    {
      CopyToClipboard();
    }



    private void btnPaste_Click( object sender, EventArgs e )
    {
      PasteClipboardImageToChar();
    }



    public void CharacterChanged( int CharIndex, int Count )
    {
      DoNotUpdateFromControls = true;

      bool currentCharChanged = false;

      for ( int charIndex = CharIndex; charIndex < CharIndex + Count; ++charIndex )
      {
        if ( m_Project.Mode == TextCharMode.COMMODORE_ECM )
        {
          for ( int i = 0; i < 4; ++i )
          {
            RebuildCharImage( ( charIndex + i * 64 ) % 256 );
            panelCharacters.InvalidateItemRect( ( charIndex + i * 64 ) % 256 );

            if ( m_CurrentChar == charIndex )
            {
              currentCharChanged = true;
            }
          }
        }
        else
        {
          RebuildCharImage( charIndex );
          panelCharacters.InvalidateItemRect( charIndex );
          currentCharChanged = ( m_CurrentChar == charIndex );
        }
      }

      if ( currentCharChanged )
      {
        panelCharacters_SelectionChanged( null, null );
        _ColorSettingsDlg.CustomColor = m_Project.Characters[m_CurrentChar].Tile.CustomColor;

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

      Displayer.CharacterDisplayer.DisplayChar( m_Project, m_Project.Colors.Palette, CharIndex, m_Project.Characters[CharIndex].Tile.Image, 0, 0 );
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
            Displayer.CharacterDisplayer.DisplayChar( m_Project, m_Project.Colors.Palette, CharIndex, m_ImagePlayground, i * m_CharacterWidth, j * m_CharacterHeight, (int)m_Project.PlaygroundChars[i + j * 16] >> 16 );
          }
        }
      }
      if ( playgroundChanged )
      {
        RedrawPlayground();
      }
      if ( CharIndex == m_CurrentChar )
      {
        RedrawColorChooser();
      }
    }



    public void PasteImage( string FromFile, GR.Image.FastImage Image, bool ForceMulticolor )
    {
      GR.Image.FastImage mappedImage = null;


      var   mcSettings = new ColorSettings( m_Project.Colors );

      bool pasteAsBlock = false;

      Types.GraphicType   importType = Types.GraphicType.CHARACTERS;
      if ( ( m_Project.Mode == TextCharMode.MEGA65_FCM )
      ||   ( m_Project.Mode == TextCharMode.MEGA65_FCM_16BIT ) )
      {
        importType = Types.GraphicType.CHARACTERS_FCM;
      }
      if ( !Core.MainForm.ImportImage( FromFile, Image, importType, mcSettings, out mappedImage, out mcSettings, out pasteAsBlock ) )
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
            _ColorSettingsDlg.CustomColor = m_Project.Characters[m_CurrentChar].Tile.CustomColor;
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
      int newChar = panelCharacters.SelectedIndex;
      if ( ( newChar != -1 )
      &&   ( panelCharacters.SelectedIndices.Count == 1 ) )
      {
        labelCharNo.Text = "Character: " + newChar.ToString();
        m_CurrentChar = newChar;

        if ( !Lookup.HasCustomPalette( m_Project.Characters[m_CurrentChar].Tile.Mode ) )
        {
          if ( _ColorSettingsDlg.CustomColor != m_Project.Characters[m_CurrentChar].Tile.CustomColor )
          {
            _ColorSettingsDlg.CustomColor = m_Project.Characters[m_CurrentChar].Tile.CustomColor;
          }
        }
        else
        {
          _ColorSettingsDlg.ActivePalette = m_Project.Characters[m_CurrentChar].Tile.Colors.ActivePalette;
        }
        canvasEditor.Invalidate();

        SelectCategory( m_Project.Characters[m_CurrentChar].Category );
        RedrawColorChooser();
      }
    }



    private bool ImportChar( GR.Image.FastImage Image, int CharIndex, bool ForceMulticolor )
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
          Buffer.SetU8At( i, (byte)Image.GetPixelData( i % 8, i / 8 ) );
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
            int     colorIndex = (int)Image.GetPixelData( x, y ) % 16;
            if ( colorIndex >= 16 )
            {
              return false;
            }
            if ( ( x % 2 ) == 0 )
            {
              if ( colorIndex != (int)Image.GetPixelData( x + 1, y ) % 16 )
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
        && ( numColors > 2 ) )
        {
          return false;
        }
        if ( ( hasSinglePixel )
        && ( numColors == 2 )
        && ( !usedBackgroundColor ) )
        {
          return false;
        }
        if ( ( !hasSinglePixel )
        && ( numColors > 4 ) )
        {
          return false;
        }
        if ( ( !hasSinglePixel )
        && ( numColors == 4 )
        && ( !usedBackgroundColor ) )
        {
          return false;
        }
        int     otherColorIndex = 16;
        if ( ( !hasSinglePixel )
        && ( numColors == 2 )
        && ( usedBackgroundColor ) )
        {
          for ( int i = 0; i < 16; ++i )
          {
            if ( ( usedColor[i] )
            && ( i != m_Project.Colors.BackgroundColor ) )
            {
              otherColorIndex = i;
              break;
            }
          }
        }
        if ( ( !ForceMulticolor )
        && ( ( hasSinglePixel )
        || ( ( numColors == 2 )
        && ( usedBackgroundColor )
        && ( otherColorIndex < 8 ) ) ) )
        //||   ( numColors == 2 ) )
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
              int ColorIndex = (int)Image.GetPixelData( x, y ) % 16;

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
              int ColorIndex = (int)Image.GetPixelData( 2 * x, y ) % 16;

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
      m_Project.Characters[CharIndex].Tile.CustomColor = chosenCharColor;
      if ( ( isMultiColor )
      &&   ( chosenCharColor < 8 ) )
      {
        m_Project.Characters[CharIndex].Tile.CustomColor = chosenCharColor + 8;
      }
      RebuildCharImage( CharIndex );
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
      if ( comboCharsetMode.SelectedIndex != (int)m_Project.Mode )
      {
        comboCharsetMode.SelectedIndex = (int)m_Project.Mode;
      }


      ChangeColorSettingsDialog();
      UpdatePalette();
      for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
      {
        RebuildCharImage( i );

        panelCharacters.Items[i].MemoryImage = m_Project.Characters[i].Tile.Image;
      }

      OnPaletteChanged();
      //UpdateCustomColorCombo();

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

      panelCharColors.Visible = Lookup.RequiresCustomColorForCharacter( m_Project.Mode );

      comboCategories.Items.Clear();
      foreach ( var category in m_Project.Categories )
      {
        comboCategories.Items.Add( category );
      }
      SelectCategory( m_Project.Characters[m_CurrentChar].Category );

      panelCharacters_SelectionChanged( null, null );

      panelCharacters.Invalidate();
      canvasEditor.Invalidate();
      RedrawColorChooser();

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
      //e.Graphics.FillRectangle( new System.Drawing.SolidBrush( labelCharNo.BackColor ), labelCharNo.ClientRectangle );
      //e.Graphics.DrawString( "lsmf", labelCharNo.Font, new System.Drawing.SolidBrush( labelCharNo.ForeColor ), labelCharNo.ClientRectangle );

      if ( !ConstantData.ScreenCodeToChar.ContainsKey( (byte)m_CurrentChar ) )
      {
        Debug.Log( "Missing char for " + m_CurrentChar );
      }
      else
      {
        try
        {
          int offset = (int)e.Graphics.MeasureString( labelCharNo.Text, labelCharNo.Font ).Width;
          e.Graphics.DrawString( "" + ConstantData.ScreenCodeToChar[(byte)m_CurrentChar].CharValue, new System.Drawing.Font( Core.MainForm.m_FontC64.Families[0], 16, System.Drawing.GraphicsUnit.Pixel ), System.Drawing.SystemBrushes.WindowText, offset + 10, 0 );
        }
        catch ( Exception ex )
        {
          Debug.Log( "Exception during drawing char " + ex.ToString() );
          Core.AddToOutput( "Exception during drawing char " + ex.ToString() );
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

      int     colorIndex = _ColorSettingsDlg.CustomColor;
      if ( ( m_Project.Mode != TextCharMode.MEGA65_FCM )
      &&   ( m_Project.Mode != TextCharMode.MEGA65_FCM_16BIT ) )
      {
        colorIndex = (int)m_CurrentColorType;
      }

      if ( ( Core.Settings.BehaviourRightClickIsBGColorPaint )
      &&   ( ( Buttons & MouseButtons.Right ) != 0 ) )
      {
        Buttons = MouseButtons.Left;
        colorIndex = (int)ColorType.BACKGROUND;
      }

      if ( ( Buttons & MouseButtons.Left ) != 0 )
      {
        var potentialUndo = new Undo.UndoCharacterEditorCharChange( this, m_Project, affectedCharIndex, 1 );
        if ( affectedChar.Tile.SetPixel( charX, charY, colorIndex ) )
        {
          if ( m_ButtonReleased )
          {
            UndoManager.AddUndoTask( potentialUndo );
            m_ButtonReleased = false;
          }
          RaiseModifiedEvent( new List<int>() { affectedCharIndex } );
          if ( m_Project.Mode == TextCharMode.COMMODORE_ECM )
          {
            for ( int i = 0; i < 4; ++i )
            {
              RebuildCharImage( affectedCharIndex + i * 64 );
              panelCharacters.InvalidateItemRect( affectedCharIndex + i * 64 );
            }
          }
          else
          {
            RebuildCharImage( affectedCharIndex );
            panelCharacters.InvalidateItemRect( affectedCharIndex );
          }
          canvasEditor.Invalidate();
        }
      }
      else
      {
        m_ButtonReleased = true;
      }
      if ( ( Buttons & MouseButtons.Right ) != 0 )
      {
        int   pickedColor = affectedChar.Tile.GetPixel( charX, charY );

        if ( ( m_Project.Mode == TextCharMode.MEGA65_NCM )
        &&   ( (ColorType)pickedColor != ColorType.BACKGROUND ) )
        {
          _ColorSettingsDlg.CustomColor = pickedColor;
          _ColorSettingsDlg.SelectedColor = ColorType.CUSTOM_COLOR;
        }
        else
        {
          _ColorSettingsDlg.SelectedColor = (ColorType)pickedColor;
        }

        if ( ( m_Project.Mode == TextCharMode.MEGA65_FCM )
        ||   ( m_Project.Mode == TextCharMode.MEGA65_FCM_16BIT ) )
        {
          affectedChar.Tile.CustomColor = pickedColor;
        }
      }
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



    private void btnInvert_Click( object sender, EventArgs e )
    {
      Invert();
    }



    public void Invert()
    {
      List<int>     selectedChars = panelCharacters.SelectedIndices;

      UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        UndoManager.AddGroupedUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, index, 1 ) );

        for ( int y = 0; y < Lookup.NumBytesOfSingleCharacterBitmap( m_Project.Mode ); ++y )
        {
          byte result = (byte)( ~m_Project.Characters[index].Tile.Data.ByteAt( y ) );
          m_Project.Characters[index].Tile.Data.SetU8At( y, result );
        }
        RebuildCharImage( index );
        panelCharacters.InvalidateItemRect( index );
      }
      canvasEditor.Invalidate();
      RaiseModifiedEvent( selectedChars );
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
      int     charY = (int)( ( 8 * 2 * Y ) / picturePlayground.ClientRectangle.Height );

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

          Displayer.CharacterDisplayer.DisplayChar( m_Project, m_Project.Colors.Palette, m_CurrentChar, m_ImagePlayground, charX * m_CharacterWidth, charY * m_CharacterHeight, m_CurrentColor );
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
        RedrawColorChooser();
      }
    }



    private void panelCharColors_MouseDown( object sender, MouseEventArgs e )
    {
      HandleMouseOnColorChooser( e.X, e.Y, e.Button );
    }



    private void panelCharColors_MouseMove( object sender, MouseEventArgs e )
    {
      MouseButtons    buttons = e.Button;
      if ( !panelCharColors.Focused )
      {
        buttons = 0;
      }
      HandleMouseOnColorChooser( e.X, e.Y, buttons );
    }



    private void HandleMouseOnColorChooser( int X, int Y, MouseButtons Buttons )
    {
      if ( ( X < 0 )
      ||   ( X >= panelCharColors.ClientSize.Width ) )
      {
        return;
      }
      if ( ( Buttons & MouseButtons.Left ) == MouseButtons.Left )
      {
        int colorIndex = (int)( ( _NumColorsInColorSelector * X ) / panelCharColors.ClientSize.Width );
        m_CurrentColor = (byte)colorIndex;
        RedrawColorChooser();
      }
    }



    private void btnMirrorX_Click( object sender, EventArgs e )
    {
      MirrorX();
    }



    private void btnMirrorY_Click( object sender, EventArgs e )
    {
      MirrorY();
    }



    public void MirrorX()
    {
      List<int>     selectedChars = panelCharacters.SelectedIndices;

      UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        UndoManager.AddGroupedUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, index, 1 ) );

        var processedChar = m_Project.Characters[index];

        for ( int y = 0; y < 8; ++y )
        {
          if ( ( m_Project.Mode == TextCharMode.MEGA65_FCM )
          ||   ( m_Project.Mode == TextCharMode.MEGA65_FCM_16BIT ) )
          {
            for ( int x = 0; x < 4; ++x )
            {
              byte temp = processedChar.Tile.Data.ByteAt( y * 8 + x );
              processedChar.Tile.Data.SetU8At( y * 8 + x, processedChar.Tile.Data.ByteAt( y * 8 + ( 7 - x ) ) );
              processedChar.Tile.Data.SetU8At( y * 8 + ( 7 - x ), temp );
            }
          }
          else if ( ( ( m_Project.Mode == TextCharMode.COMMODORE_MULTICOLOR )
          ||          ( m_Project.Mode == TextCharMode.VIC20 ) )
          &&        ( processedChar.Tile.CustomColor >= 8 ) )
          {
            byte result = (byte)( (byte)( ( processedChar.Tile.Data.ByteAt( y ) & 0xc0 ) >> 6 )
                                | (byte)( ( processedChar.Tile.Data.ByteAt( y ) & 0x30 ) >> 2 )
                                | (byte)( ( processedChar.Tile.Data.ByteAt( y ) & 0x0c ) << 2 )
                                | (byte)( ( processedChar.Tile.Data.ByteAt( y ) & 0x03 ) << 6 ) );
            processedChar.Tile.Data.SetU8At( y, result );
          }
          else
          {
            byte result = (byte)( (byte)( ( processedChar.Tile.Data.ByteAt( y ) & 0x80 ) >> 7 )
                                | (byte)( ( processedChar.Tile.Data.ByteAt( y ) & 0x40 ) >> 5 )
                                | (byte)( ( processedChar.Tile.Data.ByteAt( y ) & 0x20 ) >> 3 )
                                | (byte)( ( processedChar.Tile.Data.ByteAt( y ) & 0x10 ) >> 1 )
                                | (byte)( ( processedChar.Tile.Data.ByteAt( y ) & 0x08 ) << 1 )
                                | (byte)( ( processedChar.Tile.Data.ByteAt( y ) & 0x04 ) << 3 )
                                | (byte)( ( processedChar.Tile.Data.ByteAt( y ) & 0x02 ) << 5 )
                                | (byte)( ( processedChar.Tile.Data.ByteAt( y ) & 0x01 ) << 7 ) );
            processedChar.Tile.Data.SetU8At( y, result );
          }
        }
        RebuildCharImage( index );
        panelCharacters.InvalidateItemRect( index );
      }
      canvasEditor.Invalidate();
      RaiseModifiedEvent( selectedChars );
    }



    public void MirrorY()
    {
      List<int>     selectedChars = panelCharacters.SelectedIndices;

      UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        UndoManager.AddGroupedUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, index, 1 ) );

        var processedChar = m_Project.Characters[index];

        if ( ( m_Project.Mode == TextCharMode.MEGA65_FCM )
        ||   ( m_Project.Mode == TextCharMode.MEGA65_FCM_16BIT ) )
        {
          for ( int x = 0; x < 8; ++x )
          {
            for ( int y = 0; y < 4; ++y )
            {
              byte temp = processedChar.Tile.Data.ByteAt( y * 8 + x );
              processedChar.Tile.Data.SetU8At( y * 8 + x, processedChar.Tile.Data.ByteAt( ( 7 - y ) * 8 + x ) );
              processedChar.Tile.Data.SetU8At( ( 7 - y ) * 8 + x, temp );
            }
          }
        }
        else
        {
          for ( int y = 0; y < 4; ++y )
          {
            byte oldValue = m_Project.Characters[index].Tile.Data.ByteAt( y );
            m_Project.Characters[index].Tile.Data.SetU8At( y, m_Project.Characters[index].Tile.Data.ByteAt( 7 - y ) );
            m_Project.Characters[index].Tile.Data.SetU8At( 7 - y, oldValue );
          }
        }
        RebuildCharImage( index );
        panelCharacters.InvalidateItemRect( index );
      }
      canvasEditor.Invalidate();
      RaiseModifiedEvent( selectedChars );
    }



    private void btnRotateLeft_Click( object sender, EventArgs e )
    {
      RotateLeft();
    }



    public void RotateLeft()
    {
      List<int>     selectedChars = panelCharacters.SelectedIndices;

      UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        UndoManager.AddGroupedUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, index, 1 ) );

        GR.Memory.ByteBuffer resultData = new GR.Memory.ByteBuffer( m_Project.Characters[index].Tile.Data.Length );

        if ( ( m_Project.Mode == TextCharMode.MEGA65_FCM )
        ||   ( m_Project.Mode == TextCharMode.MEGA65_FCM_16BIT ) )
        {
          for ( int i = 0; i < 8; ++i )
          {
            for ( int j = 0; j < 8; ++j )
            {
              int sourceX = i;
              int sourceY = j;
              int targetX = j;
              int targetY = 7 - i;

              resultData.SetU8At( targetY * 8 + targetX, m_Project.Characters[index].Tile.Data.ByteAt( sourceY * 8 + sourceX ) );
            }
          }
        }
        else if ( ( m_Project.Mode == TextCharMode.COMMODORE_MULTICOLOR )
        ||        ( m_Project.Characters[index].Tile.CustomColor >= 8 ) )
        {
          for ( int i = 0; i < 8; i += 2 )
          {
            for ( int j = 0; j < 8; ++j )
            {
              int sourceX = 7 - j;
              int sourceY = i;

              if ( ( sourceX < 0 )
              ||   ( sourceX >= 8 )
              ||   ( sourceY < 0 )
              ||   ( sourceY >= 8 ) )
              {
                continue;
              }
              int maskOffset = 6 - ( ( sourceX % 8 ) / 2 ) * 2;
              byte sourceColor = (byte)( ( m_Project.Characters[index].Tile.Data.ByteAt( sourceY ) & ( 3 << maskOffset ) ) >> maskOffset );

              maskOffset = 6 - ( ( i % 8 ) / 2 ) * 2;
              resultData.SetU8At( j, (byte)( resultData.ByteAt( j ) | ( sourceColor << maskOffset ) ) );
            }
          }
        }
        else
        {
          for ( int i = 0; i < 8; ++i )
          {
            for ( int j = 0; j < 8; ++j )
            {
              int sourceX = i;
              int sourceY = j;
              int targetX = j;
              int targetY = 7 - i;
              if ( ( m_Project.Characters[index].Tile.Data.ByteAt( sourceY ) & ( 1 << ( 7 - ( sourceX % 8 ) ) ) ) != 0 )
              {
                resultData.SetU8At( targetY, (byte)( resultData.ByteAt( targetY ) | ( 1 << ( 7 - targetX % 8 ) ) ) );
              }
            }
          }
        }
        m_Project.Characters[index].Tile.Data = resultData;
        RebuildCharImage( index );
        panelCharacters.InvalidateItemRect( index );
      }
      canvasEditor.Invalidate();
      RaiseModifiedEvent( selectedChars );
    }



    private void btnRotateRight_Click( object sender, EventArgs e )
    {
      RotateRight();
    }



    public void RotateRight()
    {
      List<int>     selectedChars = panelCharacters.SelectedIndices;

      UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        UndoManager.AddGroupedUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, index, 1 ) );

        GR.Memory.ByteBuffer resultData = new GR.Memory.ByteBuffer( m_Project.Characters[index].Tile.Data.Length );

        if ( ( m_Project.Mode == TextCharMode.MEGA65_FCM )
        ||   ( m_Project.Mode == TextCharMode.MEGA65_FCM_16BIT ) )
        {
          for ( int i = 0; i < 8; ++i )
          {
            for ( int j = 0; j < 8; ++j )
            {
              int sourceX = i;
              int sourceY = j;
              int targetX = 7 - j;
              int targetY = i;

              resultData.SetU8At( targetY * 8 + targetX, m_Project.Characters[index].Tile.Data.ByteAt( sourceY * 8 + sourceX ) );
            }
          }
        }
        else if ( ( m_Project.Mode == TextCharMode.COMMODORE_MULTICOLOR )
        ||        ( m_Project.Characters[index].Tile.CustomColor >= 8 ) )
        {
          for ( int i = 0; i < 8; i += 2 )
          {
            for ( int j = 0; j < 8; ++j )
            {
              int sourceX = j;
              int sourceY = 7 - i;

              if ( ( sourceX < 0 )
              ||   ( sourceX >= 8 )
              ||   ( sourceY < 0 )
              ||   ( sourceY >= 8 ) )
              {
                continue;
              }
              int maskOffset = 6 - ( ( sourceX % 8 ) / 2 ) * 2;
              byte sourceColor = (byte)( ( m_Project.Characters[index].Tile.Data.ByteAt( sourceY ) & ( 3 << maskOffset ) ) >> maskOffset );

              maskOffset = 6 - ( ( i % 8 ) / 2 ) * 2;
              resultData.SetU8At( j, (byte)( resultData.ByteAt( j ) | ( sourceColor << maskOffset ) ) );
            }
          }
        }
        else
        {
          for ( int i = 0; i < 8; ++i )
          {
            for ( int j = 0; j < 8; ++j )
            {
              int sourceX = i;
              int sourceY = j;
              int targetX = 7 - j;
              int targetY = i;
              if ( ( m_Project.Characters[index].Tile.Data.ByteAt( sourceY ) & ( 1 << ( 7 - ( sourceX % 8 ) ) ) ) != 0 )
              {
                resultData.SetU8At( targetY, (byte)( resultData.ByteAt( targetY ) | ( 1 << ( 7 - targetX % 8 ) ) ) );
              }
            }
          }
        }
        m_Project.Characters[index].Tile.Data = resultData;
        RebuildCharImage( index );
        panelCharacters.InvalidateItemRect( index );
      }
      canvasEditor.Invalidate();
      RaiseModifiedEvent( selectedChars );
    }



    private void btnShiftDown_Click( object sender, EventArgs e )
    {
      ShiftDown();
    }



    public void ShiftDown()
    {
      List<int>     selectedChars = panelCharacters.SelectedIndices;

      UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        UndoManager.AddGroupedUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, index, 1 ) );

        if ( ( m_Project.Mode == TextCharMode.MEGA65_FCM )
        ||   ( m_Project.Mode == TextCharMode.MEGA65_FCM_16BIT ) )
        {
          for ( int x = 0; x < 8; ++x )
          {
            byte  temp = m_Project.Characters[index].Tile.Data.ByteAt( 7 * 8 + x );
            for ( int y = 0; y < 7; ++y )
            {
              m_Project.Characters[index].Tile.Data.SetU8At( ( 7 - y ) * 8 + x, m_Project.Characters[index].Tile.Data.ByteAt( ( 7 - y - 1 ) * 8 + x ) );
            }
            m_Project.Characters[index].Tile.Data.SetU8At( x, temp );
          }
        }
        else
        {
          byte  temp = m_Project.Characters[index].Tile.Data.ByteAt( 7 );
          for ( int y = 0; y < 7; ++y )
          {
            m_Project.Characters[index].Tile.Data.SetU8At( 7 - y, m_Project.Characters[index].Tile.Data.ByteAt( 7 - y - 1 ) );
          }
          m_Project.Characters[index].Tile.Data.SetU8At( 0, temp );
        }
        RebuildCharImage( index );
        panelCharacters.InvalidateItemRect( index );
      }
      canvasEditor.Invalidate();
      RaiseModifiedEvent( selectedChars );
    }



    private void btnShiftLeft_Click( object sender, EventArgs e )
    {
      ShiftLeft();
    }



    private void btnShiftRight_Click( object sender, EventArgs e )
    {
      ShiftRight();
    }



    private void btnShiftUp_Click( object sender, EventArgs e )
    {
      ShiftUp();
    }



    public void ShiftUp()
    {
      List<int>     selectedChars = panelCharacters.SelectedIndices;

      UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        UndoManager.AddGroupedUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, index, 1 ) );

        if ( ( m_Project.Mode == TextCharMode.MEGA65_FCM )
        ||   ( m_Project.Mode == TextCharMode.MEGA65_FCM_16BIT ) )
        {
          for ( int x = 0; x < 8; ++x )
          {
            byte  temp = m_Project.Characters[index].Tile.Data.ByteAt( x );
            for ( int y = 0; y < 7; ++y )
            {
              m_Project.Characters[index].Tile.Data.SetU8At( y * 8 + x, m_Project.Characters[index].Tile.Data.ByteAt( ( y + 1 ) * 8 + x ) );
            }
            m_Project.Characters[index].Tile.Data.SetU8At( x + 7 * 8, temp );
          }
        }
        else
        {
          byte  temp = m_Project.Characters[index].Tile.Data.ByteAt( 0 );
          for ( int y = 0; y < 7; ++y )
          {
            m_Project.Characters[index].Tile.Data.SetU8At( y, m_Project.Characters[index].Tile.Data.ByteAt( y + 1 ) );
          }
          m_Project.Characters[index].Tile.Data.SetU8At( 7, temp );
        }
        RebuildCharImage( index );
        panelCharacters.InvalidateItemRect( index );
      }
      canvasEditor.Invalidate();
      RaiseModifiedEvent( selectedChars );
    }


    public void ShiftLeft()
    {
      List<int>     selectedChars = panelCharacters.SelectedIndices;

      UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        UndoManager.AddGroupedUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, index, 1 ) );

        for ( int y = 0; y < 8; ++y )
        {
          if ( ( m_Project.Mode == TextCharMode.MEGA65_FCM )
          ||   ( m_Project.Mode == TextCharMode.MEGA65_FCM_16BIT ) )
          {
            byte temp = m_Project.Characters[index].Tile.Data.ByteAt( y * 8 );
            for ( int x = 0; x < 7; ++x )
            {
              m_Project.Characters[index].Tile.Data.SetU8At( y * 8 + x, m_Project.Characters[index].Tile.Data.ByteAt( y * 8 + x + 1 ) );
            }
            m_Project.Characters[index].Tile.Data.SetU8At( y * 8 + 7, temp );
          }
          else if ( ( m_Project.Mode == TextCharMode.COMMODORE_MULTICOLOR )
          &&        ( m_Project.Characters[index].Tile.CustomColor >= 8 ) )
          {
            byte result = (byte)( (byte)( ( m_Project.Characters[index].Tile.Data.ByteAt( y ) & 0xc0 ) >> 6 )
                                | (byte)( ( m_Project.Characters[index].Tile.Data.ByteAt( y ) & 0x3f ) << 2 ) );
            m_Project.Characters[index].Tile.Data.SetU8At( y, result );
          }
          else
          {
            byte result = (byte)( (byte)( ( m_Project.Characters[index].Tile.Data.ByteAt( y ) & 0x80 ) >> 7 )
                                | (byte)( ( m_Project.Characters[index].Tile.Data.ByteAt( y ) & 0x7f ) << 1 ) );
            m_Project.Characters[index].Tile.Data.SetU8At( y, result );
          }
        }
        RebuildCharImage( index );
        panelCharacters.InvalidateItemRect( index );
      }
      canvasEditor.Invalidate();
      RaiseModifiedEvent( selectedChars );
    }



    public void ShiftRight()
    {
      List<int>     selectedChars = panelCharacters.SelectedIndices;

      UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        UndoManager.AddGroupedUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, index, 1 ) );

        for ( int y = 0; y < 8; ++y )
        {
          if ( ( m_Project.Mode == TextCharMode.MEGA65_FCM )
          ||   ( m_Project.Mode == TextCharMode.MEGA65_FCM_16BIT ) )
          {
            byte temp = m_Project.Characters[index].Tile.Data.ByteAt( y * 8 + 7 );
            for ( int x = 0; x < 7; ++x )
            {
              m_Project.Characters[index].Tile.Data.SetU8At( y * 8 + 7 - x, m_Project.Characters[index].Tile.Data.ByteAt( y * 8 + 7 - x - 1 ) );
            }
            m_Project.Characters[index].Tile.Data.SetU8At( y * 8, temp );
          }
          else if ( ( m_Project.Mode == TextCharMode.COMMODORE_MULTICOLOR )
          &&        ( m_Project.Characters[index].Tile.CustomColor >= 8 ) )
          {
            byte result = (byte)( (byte)( ( m_Project.Characters[index].Tile.Data.ByteAt( y ) & 0xfc ) >> 2 )
                                | (byte)( ( m_Project.Characters[index].Tile.Data.ByteAt( y ) & 0x03 ) << 6 ) );
            m_Project.Characters[index].Tile.Data.SetU8At( y, result );
          }
          else
          {
            byte result = (byte)( (byte)( ( m_Project.Characters[index].Tile.Data.ByteAt( y ) & 0x01 ) << 7 )
                                | (byte)( ( m_Project.Characters[index].Tile.Data.ByteAt( y ) & 0xfe ) >> 1 ) );
            m_Project.Characters[index].Tile.Data.SetU8At( y, result );
          }
        }
        RebuildCharImage( index );
        panelCharacters.InvalidateItemRect( index );
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
      Displayer.CharacterDisplayer.DisplayChar( m_Project, m_Project.Colors.Palette, (int)( m_Project.PlaygroundChars[X + Y * m_Project.PlaygroundWidth] & 0xffff ), m_ImagePlayground, X * m_CharacterWidth, Y * m_CharacterHeight, (int)( m_Project.PlaygroundChars[X + Y * m_Project.PlaygroundWidth] >> 16 ) );
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

      if ( m_Project.Mode != (TextCharMode)comboCharsetMode.SelectedIndex )
      {
        if ( !DoNotAddUndo )
        {
          UndoManager?.AddUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, 0, m_Project.TotalNumberOfCharacters ), true );
          UndoManager?.AddUndoTask( new Undo.UndoCharacterEditorValuesChange( this, m_Project ), false );
        }
        m_Project.Mode = (TextCharMode)comboCharsetMode.SelectedIndex;
      }

      AdjustCharacterSizes();

      UpdatePalette();
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

      for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
      {
        m_Project.Characters[i].Tile.Mode = Lookup.GraphicTileModeFromTextCharMode( m_Project.Mode, m_Project.Characters[i].Tile.CustomColor );
        m_Project.Characters[i].Tile.Data.Resize( (uint)Lookup.NumBytesOfSingleCharacterBitmap( m_Project.Mode ) );
        m_Project.Characters[i].Tile.Width = m_CharacterWidth;
        m_Project.Characters[i].Tile.Height = m_CharacterHeight;
        m_Project.Characters[i].Tile.Image.Resize( m_CharacterWidth, m_CharacterHeight );
        if ( m_Project.Characters[i].Tile.CustomColor >= Lookup.NumberOfColorsInCharacter( m_Project.Mode ) )
        {
          m_Project.Characters[i].Tile.CustomColor %= Lookup.NumberOfColorsInCharacter( m_Project.Mode );
        }

        RebuildCharImage( i );
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
          _ColorSettingsDlg = new ColorSettingsVC20( Core, m_Project.Colors, m_Project.Characters[m_CurrentChar].Tile.CustomColor );
          break;
        case TextCharMode.MEGA65_FCM:
        case TextCharMode.MEGA65_FCM_16BIT:
          _ColorSettingsDlg = new ColorSettingsMega65( Core, m_Project.Colors, m_Project.Characters[m_CurrentChar].Tile.CustomColor );
          break;
      }
      panelColorSettings.Controls.Add( _ColorSettingsDlg );
      _ColorSettingsDlg.SelectedColorChanged += _ColorSettingsDlg_SelectedColorChanged;
      _ColorSettingsDlg.ColorsModified += _ColorSettingsDlg_ColorsModified;
      _ColorSettingsDlg.ColorsExchanged += _ColorSettingsDlg_ColorsExchanged;
      _ColorSettingsDlg.PaletteModified += _ColorSettingsDlg_PaletteModified;
      _ColorSettingsDlg.PaletteSelected += _ColorSettingsDlg_PaletteSelected;
      _ColorSettingsDlg_SelectedColorChanged( _ColorSettingsDlg.SelectedColor );
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
            ColorType pixel = (ColorType)charInfo.Tile.GetPixel( x, y );

            if ( pixel == Color1 )
            {
              charInfo.Tile.SetPixel( x, y, Color2 );
            }
            else if ( pixel == Color2 )
            {
              charInfo.Tile.SetPixel( x, y, Color1 );
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
                  if ( ( !Lookup.HasCustomPalette( m_Project.Characters[selChar].Tile.Mode ) )
                  ||   ( m_Project.Characters[selChar].Tile.Mode == GraphicTileMode.MEGA65_NCM ) )
                  {
                    UndoManager.AddUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, selChar, 1 ), modified == false );

                    m_Project.Characters[selChar].Tile.CustomColor = CustomColor;
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
        m_Project.Colors.Palette = PaletteManager.PaletteFromNumColors( Lookup.NumberOfColorsInCharacter( m_Project.Mode ) );
      }

      int   colorsInSelector = 16;
      switch ( m_Project.Mode )
      {
        case TextCharMode.MEGA65_HIRES:
          colorsInSelector = 32;
          break;
        case TextCharMode.COMMODORE_ECM:
        case TextCharMode.COMMODORE_HIRES:
        case TextCharMode.COMMODORE_MULTICOLOR:
          m_Project.Colors.Palettes[0] = PaletteManager.PaletteFromMachine( MachineType.C64 );
          break;
        case TextCharMode.VIC20:
          m_Project.Colors.Palettes[0] = PaletteManager.PaletteFromMachine( MachineType.VIC20 );
          break;
      }


      if ( _NumColorsInColorSelector != colorsInSelector )
      {
        _NumColorsInColorSelector = colorsInSelector;
        panelCharColors.DisplayPage.Create( 8 * _NumColorsInColorSelector, 8, GR.Drawing.PixelFormat.Format32bppRgb );
      }

      OnPaletteChanged();
    }



    private void btnPasteFromClipboard_Click( object sender, EventArgs e )
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



    private void btnClear_Click( object sender, EventArgs e )
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
        RebuildCharImage( i );
        panelCharacters.InvalidateItemRect( i );
      }
      if ( wasModified )
      {
        canvasEditor.Invalidate();
        RaiseModifiedEvent( selectedChars );
      }
      DoNotUpdateFromControls = false;
    }



    private void btnMoveSelectionToTarget_Click( object sender, EventArgs e )
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
        charMapNewToOld[insertIndex] = entry;
        charMapOldToNew[entry] = insertIndex;
        ++insertIndex;
      }

      // now fill all other entries
      byte    insertCharIndex = 0;
      int     charPos = 0;
      while ( charPos < 256 )
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
        charMapNewToOld[charPos] = insertCharIndex;
        charMapOldToNew[insertCharIndex] = charPos;
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
      RedrawColorChooser();

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
      switch ( m_Project.Mode )
      {
        case TextCharMode.MEGA65_NCM:
          m_CharacterWidth  = 16;
          m_CharacterHeight = 8;
          break;
        case TextCharMode.COMMODORE_HIRES:
        case TextCharMode.COMMODORE_ECM:
        case TextCharMode.COMMODORE_MULTICOLOR:
        case TextCharMode.MEGA65_HIRES:
        case TextCharMode.MEGA65_ECM:
        case TextCharMode.MEGA65_FCM:
        case TextCharMode.MEGA65_FCM_16BIT:
        case TextCharMode.VIC20:
          m_CharacterWidth  = 8;
          m_CharacterHeight = 8;
          break;
        default:
          Debug.Log( "AdjustCharacterSizes, unsupported mode " + m_Project.Mode );
          break;
      }

      // adjust aspect ratio of the editor
      int   biggerSize = Math.Max( m_CharacterWidth, m_CharacterHeight );

      canvasEditor.Size = new System.Drawing.Size( m_CharacterWidth * m_CharacterEditorOrigWidth / biggerSize,
                                                    m_CharacterHeight * m_CharacterEditorOrigHeight / biggerSize );

      panelCharacters.ItemWidth = m_CharacterWidth;
      panelCharacters.ItemHeight = m_CharacterHeight;
      //panelCharacters.SetDisplaySize( panelCharacters.ClientSize.Width / 2, panelCharacters.ClientSize.Height / 2 );
    }



    private void btnAddCategory_Click( object sender, EventArgs e )
    {
      string    newCategory = editCategoryName.Text;

      UndoManager.AddUndoTask( new Undo.UndoCharsetAddCategory( this, m_Project, m_Project.Categories.Count ) );

      AddCategory( m_Project.Categories.Count, newCategory );

      RaiseModifiedEvent( new List<int>() );
    }



    private void btnDelete_Click( object sender, EventArgs e )
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



    private void btnCollapseCategory_Click( object sender, EventArgs e )
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



    private void btnSortCategories_Click( object sender, EventArgs e )
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



    private void btnReseatCategory_Click( object sender, EventArgs e )
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



    private void btnMoveCategoryUp_Click( object sender, EventArgs e )
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



    private void btnMoveCategoryDown_Click( object sender, EventArgs e )
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
      PaletteManager.ApplyPalette( panelCharColors.DisplayPage, m_Project.Colors.Palette );
      panelCharacters.Items.Clear();

      for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
      {
        m_Project.Characters[i].Tile.Colors.ActivePalette = m_Project.Colors.ActivePalette;

        RebuildCharImage( i );
        panelCharacters.Items.Add( i.ToString(), m_Project.Characters[i].Tile.Image );
      }

      panelCharColors.Visible = Lookup.RequiresCustomColorForCharacter( m_Project.Mode );

      panelCharacters.Invalidate();
      canvasEditor.Invalidate();
    }



    private void panelCharColors_PostPaint( FastImage TargetBuffer )
    {
      int     x1 = m_CurrentColor * TargetBuffer.Width / _NumColorsInColorSelector;
      int     x2 = ( m_CurrentColor + 1 ) * TargetBuffer.Width / _NumColorsInColorSelector;

      if ( Core != null )
      {
        uint  selColor = Core.Settings.FGColor( ColorableElement.SELECTION_FRAME );

        TargetBuffer.Rectangle( x1, 0, x2 - x1, TargetBuffer.Height, selColor );
      }
    }

  }
}
