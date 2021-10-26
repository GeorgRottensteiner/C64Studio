using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using C64Studio.Formats;
using GR.Image;
using RetroDevStudio;
using RetroDevStudio.Types;
using C64Studio.Types;

namespace C64Studio.Controls
{
  public partial class CharacterEditor : UserControl
  {
    public delegate void ModifiedHandler();
    public delegate void CharsetShiftedHandler( int[] OldToNew, int[] NewToOld );

    public event ModifiedHandler        Modified;
    public event CharsetShiftedHandler  CharactersShifted;

    private int                         m_CurrentChar = 0;
    private int                         m_CurrentColor = 1;
    private ColorType                   m_CurrentColorType = ColorType.CUSTOM_COLOR;
    private bool                        m_ButtonReleased = false;
    private bool                        m_RButtonReleased = false;
    public StudioCore                   Core = null;
    public Undo.UndoManager             UndoManager = null;
    private bool                        DoNotUpdateFromControls = false;

    private CharsetProject              m_Project = new CharsetProject();

    private GR.Image.MemoryImage        m_ImagePlayground = new GR.Image.MemoryImage( 256, 256, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );

    private bool                        _AllowModeChange = true;



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

      picturePlayground.DisplayPage.Create( 128, 128, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
      panelCharacters.PixelFormat = System.Drawing.Imaging.PixelFormat.Format8bppIndexed;
      panelCharacters.SetDisplaySize( 128, 128 );
      panelCharColors.DisplayPage.Create( 128, 8, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
      m_ImagePlayground.Create( 256, 256, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );

      m_Project.Colors.Palette = PaletteManager.PaletteFromMachine( MachineType.C64 );

      OnPaletteChanged();

      for ( int i = 0; i < 16; ++i )
      {
        comboBackground.Items.Add( i.ToString( "d2" ) );
        comboMulticolor1.Items.Add( i.ToString( "d2" ) );
        comboMulticolor2.Items.Add( i.ToString( "d2" ) );
        comboBGColor4.Items.Add( i.ToString( "d2" ) );
      }
      UpdateCustomColorCombo();

      comboBackground.SelectedIndex = 0;
      comboMulticolor1.SelectedIndex = 0;
      comboMulticolor2.SelectedIndex = 0;
      comboBGColor4.SelectedIndex = 0;
      comboCharColor.SelectedIndex = 1;


      foreach ( TextCharMode mode in Enum.GetValues( typeof( TextCharMode ) ) )
      {
        comboCharsetMode.Items.Add( GR.EnumHelper.GetDescription( mode ) );
      }
      comboCharsetMode.SelectedIndex = 0;

      radioCharColor.Checked = true;
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



    public void RaiseModifiedEvent()
    {
      Modified?.Invoke();
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
      for ( byte i = 0; i < 16; ++i )
      {
        Displayer.CharacterDisplayer.DisplayChar( m_Project, m_CurrentChar, panelCharColors.DisplayPage, i * 8, 0, i );
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



    private void CopyToClipboard()
    {
      List<int> selectedImages = panelCharacters.SelectedIndices;
      if ( selectedImages.Count == 0 )
      {
        return;
      }

      var clipList = new ClipboardImageList();
      clipList.Mode   = Lookup.GraphicTileModeFromTextCharMode( m_Project.Mode );
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
        bool firstEntry = true;
        foreach ( var entry in clipList.Entries )
        {
          int indexGap =  entry.Index;
          pastePos += indexGap;

          if ( pastePos >= m_Project.Characters.Count )
          {
            break;
          }

          UndoManager.AddUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, pastePos ), firstEntry );
          firstEntry = false;

          var targetTile = m_Project.Characters[pastePos].Tile;

          int copyWidth = Math.Min( 8, entry.Tile.Width );
          int copyHeight = Math.Min( 8, entry.Tile.Height );

          for ( int x = 0; x < copyWidth; ++x )
          {
            for ( int y = 0; y < copyHeight; ++y )
            {
              targetTile.SetPixel( x, y, entry.Tile.MapPixelColor( x, y, targetTile ) );
            }
          }

          RebuildCharImage( pastePos );
          panelCharacters.InvalidateItemRect( pastePos );

          if ( pastePos == m_CurrentChar )
          {
            comboCharColor.SelectedIndex = m_Project.Characters[pastePos].Tile.CustomColor;
          }
        }
        canvasEditor.Invalidate();
        RaiseModifiedEvent();
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
        if ( dataObj.GetDataPresent( "C64Studio.ImageList" ) )
        {
          System.IO.MemoryStream ms = (System.IO.MemoryStream)dataObj.GetData( "C64Studio.ImageList" );

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

            UndoManager.AddUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, pastePos ), i == 0 );

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
              var incomingTile = new GraphicTile( (int)width, (int)height, Lookup.GraphicTileModeFromTextCharMode( mode ), incomingColorSettings );
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
                /*
                var origImage = new MemoryImage( (int)width, (int)height, System.Drawing.Imaging.PixelFormat.Format1bppIndexed );
                origImage.SetData( m_Project.Characters[pastePos].Data );

                var pastedImage = new MemoryImage( (int)width, (int)height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
                pastedImage.SetData( tempBuffer );

                for ( int x = 0; x < width; ++x )
                {
                  for ( int j = 0; j < height; ++j )
                  {
                    m_Project.Characters[pastePos].Image.SetPixel( x, j, origImage.GetPixel( x, j ) );
                  }
                }*/
              }
              else
              {
                tempBuffer.CopyTo( m_Project.Characters[pastePos].Tile.Data, 0, Math.Min( Lookup.NumBytesOfSingleCharacter( m_Project.Mode ), (int)dataLength ) );
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
              tempBuffer.CopyTo( m_Project.Characters[pastePos].Tile.Data, 0, Math.Min( Lookup.NumBytesOfSingleCharacter( m_Project.Mode ), (int)dataLength ) );
            }


            int index = memIn.ReadInt32();

            RebuildCharImage( pastePos );
            panelCharacters.InvalidateItemRect( pastePos );

            if ( pastePos == m_CurrentChar )
            {
              comboCharColor.SelectedIndex = m_Project.Characters[pastePos].Tile.CustomColor;
            }
          }
          canvasEditor.Invalidate();
          RaiseModifiedEvent();
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



    public void CharacterChanged( int CharIndex )
    {
      DoNotUpdateFromControls = true;

      bool currentCharChanged = false;

      if ( m_Project.Mode == TextCharMode.COMMODORE_ECM )
      {
        for ( int i = 0; i < 4; ++i )
        {
          RebuildCharImage( ( CharIndex + i * 64 ) % 256 );
          panelCharacters.InvalidateItemRect( ( CharIndex + i * 64 ) % 256 );

          if ( m_CurrentChar == CharIndex )
          {
            currentCharChanged = true;
          }
        }
      }
      else
      {
        RebuildCharImage( CharIndex );
        panelCharacters.InvalidateItemRect( CharIndex );
        currentCharChanged = ( m_CurrentChar == CharIndex );
      }

      if ( currentCharChanged )
      {
        panelCharacters_SelectionChanged( null, null );
        comboCharColor.SelectedIndex = m_Project.Characters[CharIndex].Tile.CustomColor;

        comboCharsetMode.SelectedIndex = (int)m_Project.Mode;
      }

      RefreshCategoryCounts();
      DoNotUpdateFromControls = false;
      RaiseModifiedEvent();
    }



    void RebuildCharImage( int CharIndex )
    {
      if ( m_Project.Mode == TextCharMode.MEGA65_FCM )
      {
        m_Project.Characters[CharIndex].Tile.Data.Resize( 64 );
      }

      Displayer.CharacterDisplayer.DisplayChar( m_Project, CharIndex, m_Project.Characters[CharIndex].Tile.Image, 0, 0 );
      if ( CharIndex < panelCharacters.Items.Count )
      {
        panelCharacters.Items[CharIndex].MemoryImage = m_Project.Characters[CharIndex].Tile.Image;
      }
      bool playgroundChanged = false;
      for ( int i = 0; i < 16; ++i )
      {
        for ( int j = 0; j < 16; ++j )
        {
          if ( ( m_Project.PlaygroundChars[i + j * 16] & 0xff ) == CharIndex )
          {
            playgroundChanged = true;
            Displayer.CharacterDisplayer.DisplayChar( m_Project, CharIndex, m_ImagePlayground, i * 8, j * 8, (int)m_Project.PlaygroundChars[i + j * 16] >> 16 );
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

      if ( mappedImage.PixelFormat != System.Drawing.Imaging.PixelFormat.Format8bppIndexed )
      {
        mappedImage.Dispose();
        System.Windows.Forms.MessageBox.Show( "Image format invalid!\nNeeds to be 8bit index" );
        return;
      }

      comboBackground.SelectedIndex = mcSettings.BackgroundColor;
      comboMulticolor1.SelectedIndex = mcSettings.MultiColor1;
      comboMulticolor2.SelectedIndex = mcSettings.MultiColor2;

      int charsX = ( mappedImage.Width + 7 ) / 8;
      int charsY = ( mappedImage.Height + 7 ) / 8;
      int curCharX = m_CurrentChar % 16;
      int curCharY = m_CurrentChar / 16;
      int currentTargetChar = m_CurrentChar;

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

          ImportChar( singleChar, currentTargetChar, ForceMulticolor );
          panelCharacters.InvalidateItemRect( currentTargetChar );

          if ( currentTargetChar == m_CurrentChar )
          {
            comboCharColor.SelectedIndex = m_Project.Characters[m_CurrentChar].Tile.CustomColor;
          }
          if ( !pasteAsBlock )
          {
            ++currentTargetChar;
          }
        }
      }

      canvasEditor.Invalidate();
      RaiseModifiedEvent();
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
          if ( comboCharColor.SelectedIndex != m_Project.Characters[m_CurrentChar].Tile.CustomColor )
          {
            comboCharColor.SelectedIndex = m_Project.Characters[m_CurrentChar].Tile.CustomColor;
          }
        }
        canvasEditor.Invalidate();

        SelectCategory( m_Project.Characters[m_CurrentChar].Category );
        RedrawColorChooser();
      }
    }



    private bool ImportChar( GR.Image.FastImage Image, int CharIndex, bool ForceMulticolor )
    {
      if ( Image.PixelFormat != System.Drawing.Imaging.PixelFormat.Format8bppIndexed )
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



    internal void CharsetUpdated( CharsetProject Project )
    {
      m_Project = Project;
      if ( comboCharsetMode.SelectedIndex != (int)m_Project.Mode )
      {
        comboCharsetMode.SelectedIndex = (int)m_Project.Mode;
      }
      for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
      {
        RebuildCharImage( i );

        panelCharacters.Items[i].MemoryImage = m_Project.Characters[i].Tile.Image;
      }

      OnPaletteChanged();
      UpdateCustomColorCombo();

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

      checkShowGrid.Checked           = m_Project.ShowGrid;
      comboBackground.SelectedIndex   = m_Project.Colors.BackgroundColor;
      comboMulticolor1.SelectedIndex  = m_Project.Colors.MultiColor1;
      comboMulticolor2.SelectedIndex  = m_Project.Colors.MultiColor2;
      comboBGColor4.SelectedIndex     = m_Project.Colors.BGColor4;

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
    }



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
    }



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
      int     charX = ( 8 * X ) / canvasEditor.ClientRectangle.Width;
      int     charY = ( 8 * Y ) / canvasEditor.ClientRectangle.Height;

      int     affectedCharIndex = m_CurrentChar;
      var     origAffectedChar = m_Project.Characters[m_CurrentChar];
      var     affectedChar = m_Project.Characters[m_CurrentChar];
      if ( m_Project.Mode == TextCharMode.COMMODORE_ECM )
      {
        affectedCharIndex %= 64;
        affectedChar = m_Project.Characters[affectedCharIndex];
      }

      if ( ( Buttons & MouseButtons.Left ) != 0 )
      {
        int     colorIndex = comboCharColor.SelectedIndex;

        if ( ( m_Project.Mode != TextCharMode.MEGA65_FCM )
        &&   ( m_Project.Mode != TextCharMode.MEGA65_FCM_16BIT ) )
        {
          colorIndex = (int)m_CurrentColorType;
        }

        var potentialUndo = new Undo.UndoCharacterEditorCharChange( this, m_Project, affectedCharIndex );
        if ( affectedChar.Tile.SetPixel( charX, charY, colorIndex ) )
        {
          if ( m_ButtonReleased )
          {
            UndoManager.AddUndoTask( potentialUndo );
            m_ButtonReleased = false;
          }
          RaiseModifiedEvent();
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

        switch ( m_Project.Mode )
        {
          case TextCharMode.COMMODORE_ECM:
          case TextCharMode.COMMODORE_HIRES:
          case TextCharMode.COMMODORE_MULTICOLOR:
          case TextCharMode.VIC20:
            switch ( (ColorType)pickedColor )
            {
              case ColorType.CUSTOM_COLOR:
                radioCharColor.Checked = true;
                break;
              case ColorType.MULTICOLOR_1:
                radioMultiColor1.Checked = true;
                break;
              case ColorType.MULTICOLOR_2:
                radioMulticolor2.Checked = true;
                break;
              case ColorType.BACKGROUND:
                radioBackground.Checked = true;
                break;
            }
            break;
          case TextCharMode.MEGA65_FCM:
          case TextCharMode.MEGA65_FCM_16BIT:
            comboCharColor.SelectedIndex = pickedColor;
            break;
        }
      }
      else
      {
        m_RButtonReleased = true;
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
      foreach ( var selChar in selectedChars )
      {
        if ( ( categoryIndex != -1 )
        &&   ( m_Project.Characters[selChar].Category != categoryIndex ) )
        {
          UndoManager.AddUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, selChar ), firstChar );
          firstChar = false;

          m_Project.Characters[selChar].Category = categoryIndex;
          RaiseModifiedEvent();
        }
      }
      RefreshCategoryCounts();
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
        UndoManager.AddGroupedUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, index ) );

        for ( int y = 0; y < Lookup.NumBytesOfSingleCharacter( m_Project.Mode ); ++y )
        {
          byte result = (byte)( ~m_Project.Characters[index].Tile.Data.ByteAt( y ) );
          m_Project.Characters[index].Tile.Data.SetU8At( y, result );
        }
        RebuildCharImage( index );
        panelCharacters.InvalidateItemRect( index );
      }
      canvasEditor.Invalidate();
      RaiseModifiedEvent();
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
      int     charX = (int)( ( 16 * X ) / picturePlayground.ClientRectangle.Width );
      int     charY = (int)( ( 16 * Y ) / picturePlayground.ClientRectangle.Height );

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

          Displayer.CharacterDisplayer.DisplayChar( m_Project, m_CurrentChar, m_ImagePlayground, charX * 8, charY * 8, m_CurrentColor );
          RedrawPlayground();

          m_Project.PlaygroundChars[charX + charY * 16] = (uint)( m_CurrentChar | ( m_CurrentColor << 16 ) );
          RaiseModifiedEvent();
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
      if ( ( Buttons & MouseButtons.Left ) == MouseButtons.Left )
      {
        int colorIndex = (int)( ( 16 * X ) / panelCharColors.ClientSize.Width );
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
        UndoManager.AddGroupedUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, index ) );

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
          else if ( ( m_Project.Mode == TextCharMode.COMMODORE_MULTICOLOR )
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
      RaiseModifiedEvent();
    }



    public void MirrorY()
    {
      List<int>     selectedChars = panelCharacters.SelectedIndices;

      UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        UndoManager.AddGroupedUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, index ) );

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
      RaiseModifiedEvent();
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
        UndoManager.AddGroupedUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, index ) );

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
      RaiseModifiedEvent();
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
        UndoManager.AddGroupedUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, index ) );

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
      RaiseModifiedEvent();
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
        UndoManager.AddGroupedUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, index ) );

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
      RaiseModifiedEvent();
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
        UndoManager.AddGroupedUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, index ) );

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
      RaiseModifiedEvent();
    }


    public void ShiftLeft()
    {
      List<int>     selectedChars = panelCharacters.SelectedIndices;

      UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        UndoManager.AddGroupedUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, index ) );

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
      RaiseModifiedEvent();
    }



    public void ShiftRight()
    {
      List<int>     selectedChars = panelCharacters.SelectedIndices;

      UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        UndoManager.AddGroupedUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, index ) );

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
      RaiseModifiedEvent();
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
      Displayer.CharacterDisplayer.DisplayChar( m_Project, (int)( m_Project.PlaygroundChars[X + Y * m_Project.PlaygroundWidth] & 0xffff ), m_ImagePlayground, X * 8, Y * 8, (int)( m_Project.PlaygroundChars[X + Y * m_Project.PlaygroundWidth] >> 16 ) );
      RedrawPlayground();
    }



    private void comboBackground_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_Project.Colors.BackgroundColor != comboBackground.SelectedIndex )
      {
        UndoManager.AddUndoTask( new Undo.UndoCharacterEditorValuesChange( this, m_Project ) );

        m_Project.Colors.BackgroundColor = comboBackground.SelectedIndex;
        for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
        {
          RebuildCharImage( i );
        }
        RaiseModifiedEvent();
        canvasEditor.Invalidate();
        panelCharacters.Invalidate();
      }
    }



    private void comboMulticolor1_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_Project.Colors.MultiColor1 != comboMulticolor1.SelectedIndex )
      {
        UndoManager.AddUndoTask( new Undo.UndoCharacterEditorValuesChange( this, m_Project ) );

        m_Project.Colors.MultiColor1 = comboMulticolor1.SelectedIndex;
        for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
        {
          RebuildCharImage( i );
        }
        RaiseModifiedEvent();
        canvasEditor.Invalidate();
        panelCharacters.Invalidate();
      }
    }



    private void comboMulticolor2_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_Project.Colors.MultiColor2 != comboMulticolor2.SelectedIndex )
      {
        UndoManager.AddUndoTask( new Undo.UndoCharacterEditorValuesChange( this, m_Project ) );

        m_Project.Colors.MultiColor2 = comboMulticolor2.SelectedIndex;
        for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
        {
          RebuildCharImage( i );
        }
        RaiseModifiedEvent();
        canvasEditor.Invalidate();
        panelCharacters.Invalidate();
      }
    }



    private void comboBGColor4_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_Project.Colors.BGColor4 != comboBGColor4.SelectedIndex )
      {
        UndoManager.AddUndoTask( new Undo.UndoCharacterEditorValuesChange( this, m_Project ) );

        m_Project.Colors.BGColor4 = comboBGColor4.SelectedIndex;
        for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
        {
          RebuildCharImage( i );
        }
        RaiseModifiedEvent();
        canvasEditor.Invalidate();
        panelCharacters.Invalidate();
      }
    }



    private void comboCharColor_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( DoNotUpdateFromControls )
      {
        return;
      }

      if ( UndoManager == null )
      {
        return;
      }

      List<int>   selectedChars = panelCharacters.SelectedIndices;
      if ( selectedChars.Count == 0 )
      {
        selectedChars.Add( m_CurrentChar );
      }

      bool    modified = false;
      foreach ( int selChar in selectedChars )
      {
        if ( m_Project.Characters[selChar].Tile.CustomColor != comboCharColor.SelectedIndex )
        {
          if ( !Lookup.HasCustomPalette( m_Project.Characters[selChar].Tile.Mode ) )
          {
            UndoManager.AddUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, selChar ), modified == false );

            m_Project.Characters[selChar].Tile.CustomColor = comboCharColor.SelectedIndex;
            if ( m_Project.Mode == TextCharMode.COMMODORE_MULTICOLOR )
            {
              if ( ( m_Project.Characters[selChar].Tile.Mode == GraphicTileMode.COMMODORE_HIRES )
              &&   ( m_Project.Characters[selChar].Tile.CustomColor >= 8 ) )
              {
                m_Project.Characters[selChar].Tile.Mode = GraphicTileMode.COMMODORE_MULTICOLOR;
              }
              else if ( ( m_Project.Characters[selChar].Tile.Mode == GraphicTileMode.COMMODORE_MULTICOLOR )
              &&        ( m_Project.Characters[selChar].Tile.CustomColor < 8 ) )
              {
                m_Project.Characters[selChar].Tile.Mode = GraphicTileMode.COMMODORE_HIRES;
              }
            }
            RebuildCharImage( selChar );
            modified = true;
            panelCharacters.InvalidateItemRect( selChar );
          }
        }
      }
      if ( modified )
      {
        RaiseModifiedEvent();
        canvasEditor.Invalidate();
      }
    }



    public void ColorsChanged()
    {
      comboMulticolor1.SelectedIndex  = m_Project.Colors.MultiColor1;
      comboMulticolor2.SelectedIndex  = m_Project.Colors.MultiColor2;
      comboBGColor4.SelectedIndex     = m_Project.Colors.BGColor4;
      comboBackground.SelectedIndex   = m_Project.Colors.BackgroundColor;

      OnPaletteChanged();

      for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
      {
        RebuildCharImage( i );
      }
      canvasEditor.Invalidate();
      panelCharacters.Invalidate();
      RaiseModifiedEvent();
    }



    private void comboCharsetMode_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( DoNotUpdateFromControls )
      {
        return;
      }

      if ( m_Project.Mode == (TextCharMode)comboCharsetMode.SelectedIndex )
      {
        return;
      }

      UndoManager.AddUndoTask( new Undo.UndoCharacterEditorValuesChange( this, m_Project ), true );

      m_Project.Mode = (TextCharMode)comboCharsetMode.SelectedIndex;

      if ( m_Project.TotalNumberOfCharacters != Lookup.NumCharactersForMode( m_Project.Mode ) )
      {
        m_Project.TotalNumberOfCharacters = Lookup.NumCharactersForMode( m_Project.Mode );

        if ( m_Project.Characters.Count > m_Project.TotalNumberOfCharacters )
        {
          m_Project.Characters.RemoveRange( m_Project.TotalNumberOfCharacters, m_Project.Characters.Count - m_Project.TotalNumberOfCharacters );

          panelCharacters.Items.RemoveRange( m_Project.TotalNumberOfCharacters, m_Project.Characters.Count - m_Project.TotalNumberOfCharacters );
        }
        while ( m_Project.Characters.Count < m_Project.TotalNumberOfCharacters )
        {
          var newChar = new CharData()
          {
            Tile = new GraphicTile( 8, 8, Lookup.GraphicTileModeFromTextCharMode( m_Project.Mode ), m_Project.Characters[0].Tile.Colors )
          };
          newChar.Tile.CustomColor = 1;
          m_Project.Characters.Add( newChar );
          panelCharacters.Items.Add( "", newChar.Tile.Image );
        }
      }

      UpdatePalette();
      UpdateCustomColorCombo();

      for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
      {
        UndoManager.AddUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, i ), false );

        m_Project.Characters[i].Tile.Mode = Lookup.GraphicTileModeFromTextCharMode( m_Project.Mode );
        m_Project.Characters[i].Tile.Data.Resize( (uint)Lookup.NumBytesOfSingleCharacter( m_Project.Mode ) );
        RebuildCharImage( i );
      }

      /*
      List<int>   selectedChars = panelCharacters.SelectedIndices;
      if ( selectedChars.Count == 0 )
      {
        selectedChars.Add( m_CurrentChar );
      }

      bool modified = false;
      foreach ( int selChar in selectedChars )
      {
        if ( m_Project.Characters[selChar].Mode != (TextCharMode)comboCharsetMode.SelectedIndex )
        {
          UndoManager.AddUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, selChar ), !modified );

          m_Project.Characters[selChar].Mode = (TextCharMode)comboCharsetMode.SelectedIndex;
          if ( ( m_Project.Characters[selChar].Mode == TextCharMode.MEGA65_FCM )
          &&   ( m_Project.Characters[selChar].Data.Length < 64 ) )
          {
            m_Project.Characters[selChar].Data.Resize( 64 );
          }
          RebuildCharImage( selChar );
          panelCharacters.InvalidateItemRect( selChar );
          modified = true;
        }
      }*/

      
      panelCharacters.Invalidate();
      RaiseModifiedEvent();
      canvasEditor.Invalidate();
    }



    private void UpdatePalette()
    {
      int numColors = Lookup.NumberOfColorsInCharacter( m_Project.Mode );

      if ( m_Project.Colors.Palette.NumColors != numColors )
      {
        // palette is not matching, create new
        m_Project.Colors.Palette = PaletteManager.PaletteFromNumColors( Lookup.NumberOfColorsInCharacter( m_Project.Mode ) );
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



    private void exchangeMultiColors1And2ToolStripMenuItem_Click( object sender, EventArgs e )
    {
      UndoManager.AddUndoTask( new Undo.UndoCharacterEditorExchangeMultiColors( this, C64Studio.Undo.UndoCharacterEditorExchangeMultiColors.ExchangeMode.MULTICOLOR_1_WITH_MULTICOLOR_2 ) );

      ExchangeMultiColors();
    }



    public void ExchangeMultiColors()
    {
      int   temp = m_Project.Colors.MultiColor1;
      m_Project.Colors.MultiColor1 = m_Project.Colors.MultiColor2;
      m_Project.Colors.MultiColor2 = temp;

      int   charIndex = 0;
      foreach ( var charInfo in m_Project.Characters )
      {
        if ( ( m_Project.Mode == TextCharMode.COMMODORE_MULTICOLOR )
        &&   ( charInfo.Tile.CustomColor >= 8 ) )
        {
          for ( int y = 0; y < 8; ++y )
          {
            for ( int x = 0; x < 4; ++x )
            {
              int pixelValue = ( charInfo.Tile.Data.ByteAt( y ) & ( 3 << ( ( 3 - x ) * 2 ) ) ) >> ( ( 3 - x ) * 2 );

              if ( pixelValue == 1 )
              {
                byte  newValue = (byte)( charInfo.Tile.Data.ByteAt( y ) & ~( 3 << ( ( 3 - x ) * 2 ) ) );

                newValue |= (byte)( 2 << ( ( 3 - x ) * 2 ) );

                charInfo.Tile.Data.SetU8At( y, newValue );
              }
              else if ( pixelValue == 2 )
              {
                byte  newValue = (byte)( charInfo.Tile.Data.ByteAt( y ) & ~( 3 << ( ( 3 - x ) * 2 ) ) );

                newValue |= (byte)( 1 << ( ( 3 - x ) * 2 ) );

                charInfo.Tile.Data.SetU8At( y, newValue );
              }
            }
          }
          RebuildCharImage( charIndex );
          panelCharacters.Invalidate();
        }
        ++charIndex;
      }
      comboMulticolor1.SelectedIndex = m_Project.Colors.MultiColor1;
      comboMulticolor2.SelectedIndex = m_Project.Colors.MultiColor2;

      RaiseModifiedEvent();
    }



    private void exchangeMultiColor1AndBGColorToolStripMenuItem_Click( object sender, EventArgs e )
    {
      UndoManager.AddUndoTask( new Undo.UndoCharacterEditorExchangeMultiColors( this, C64Studio.Undo.UndoCharacterEditorExchangeMultiColors.ExchangeMode.MULTICOLOR_1_WITH_BACKGROUND ) );

      ExchangeMultiColor1WithBackground();
    }



    public void ExchangeMultiColor1WithBackground()
    {
      int   temp = m_Project.Colors.BackgroundColor;
      m_Project.Colors.BackgroundColor = m_Project.Colors.MultiColor1;
      m_Project.Colors.MultiColor1 = temp;

      int   charIndex = 0;
      foreach ( var charInfo in m_Project.Characters )
      {
        if ( ( m_Project.Mode == TextCharMode.COMMODORE_MULTICOLOR )
        &&   ( charInfo.Tile.CustomColor >= 8 ) )
        {
          for ( int y = 0; y < 8; ++y )
          {
            for ( int x = 0; x < 4; ++x )
            {
              int pixelValue = ( charInfo.Tile.Data.ByteAt( y ) & ( 3 << ( ( 3 - x ) * 2 ) ) ) >> ( ( 3 - x ) * 2 );

              if ( pixelValue == 1 )
              {
                byte  newValue = (byte)( charInfo.Tile.Data.ByteAt( y ) & ~( 3 << ( ( 3 - x ) * 2 ) ) );

                charInfo.Tile.Data.SetU8At( y, newValue );
              }
              else if ( pixelValue == 0 )
              {
                byte  newValue = (byte)( charInfo.Tile.Data.ByteAt( y ) & ~( 3 << ( ( 3 - x ) * 2 ) ) );

                newValue |= (byte)( 1 << ( ( 3 - x ) * 2 ) );

                charInfo.Tile.Data.SetU8At( y, newValue );
              }
            }
          }
        }
        RebuildCharImage( charIndex );
        panelCharacters.Invalidate();
        ++charIndex;
      }
      comboMulticolor1.SelectedIndex  = m_Project.Colors.MultiColor1;
      comboBackground.SelectedIndex   = m_Project.Colors.BackgroundColor;

      RaiseModifiedEvent();
    }



    private void exchangeMultiColor2AndBGColorToolStripMenuItem_Click( object sender, EventArgs e )
    {
      UndoManager.AddUndoTask( new Undo.UndoCharacterEditorExchangeMultiColors( this, C64Studio.Undo.UndoCharacterEditorExchangeMultiColors.ExchangeMode.MULTICOLOR_2_WITH_BACKGROUND ) );

      ExchangeMultiColor2WithBackground();
    }



    public void ExchangeMultiColor2WithBackground()
    {
      int   temp = m_Project.Colors.BackgroundColor;
      m_Project.Colors.BackgroundColor = m_Project.Colors.MultiColor2;
      m_Project.Colors.MultiColor2 = temp;

      int   charIndex = 0;
      foreach ( var charInfo in m_Project.Characters )
      {
        if ( ( m_Project.Mode == TextCharMode.COMMODORE_MULTICOLOR )
        &&   ( charInfo.Tile.CustomColor >= 8 ) )
        {
          for ( int y = 0; y < 8; ++y )
          {
            for ( int x = 0; x < 4; ++x )
            {
              int pixelValue = ( charInfo.Tile.Data.ByteAt( y ) & ( 3 << ( ( 3 - x ) * 2 ) ) ) >> ( ( 3 - x ) * 2 );

              if ( pixelValue == 2 )
              {
                byte  newValue = (byte)( charInfo.Tile.Data.ByteAt( y ) & ~( 3 << ( ( 3 - x ) * 2 ) ) );

                //newValue |= (byte)( 2 << ( ( 3 - x ) * 2 ) );

                charInfo.Tile.Data.SetU8At( y, newValue );
              }
              else if ( pixelValue == 0 )
              {
                byte  newValue = (byte)( charInfo.Tile.Data.ByteAt( y ) & ~( 3 << ( ( 3 - x ) * 2 ) ) );

                newValue |= (byte)( 2 << ( ( 3 - x ) * 2 ) );

                charInfo.Tile.Data.SetU8At( y, newValue );
              }
            }
          }
        }
        RebuildCharImage( charIndex );
        panelCharacters.Invalidate();
        ++charIndex;
      }
      comboMulticolor2.SelectedIndex = m_Project.Colors.MultiColor2;
      comboBackground.SelectedIndex = m_Project.Colors.BackgroundColor;

      RaiseModifiedEvent();
    }



    public void MultiColor2()
    {
      comboMulticolor2.SelectedIndex = ( comboMulticolor2.SelectedIndex + 1 ) % 16;
    }



    public void MultiColor1()
    {
      comboMulticolor1.SelectedIndex = ( comboMulticolor1.SelectedIndex + 1 ) % 16;
    }



    public void CustomColor()
    {
      comboCharColor.SelectedIndex = ( comboCharColor.SelectedIndex + 1 ) % 16;
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

        UndoManager.AddUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, i ), firstUndoStep );
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
        RaiseModifiedEvent();
      }
      DoNotUpdateFromControls = false;
    }



    private void btnExchangeColors_Click( object sender, EventArgs e )
    {
      contextMenuExchangeColors.Show( btnExchangeColors, new Point( 0, btnExchangeColors.Height ) );
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

      for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
      {
        UndoManager.AddUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, i ), i == 0 );
      }

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
      RaiseModifiedEvent();
    }



    private void canvasEditor_Paint( object sender, PaintEventArgs e )
    {
      e.Graphics.FillRectangle( System.Drawing.Brushes.Black, 0, 0, canvasEditor.ClientSize.Width, canvasEditor.ClientSize.Height );

      Displayer.CharacterDisplayer.DisplayChar( m_Project, m_CurrentChar, new CustomDrawControlContext( e.Graphics, canvasEditor.ClientSize.Width, canvasEditor.ClientSize.Height )
        {
          Palette = m_Project.Colors.Palette
      } );

      if ( m_Project.ShowGrid )
      {
        if ( ( m_Project.Mode == TextCharMode.COMMODORE_MULTICOLOR )
        &&   ( m_Project.Characters[m_CurrentChar].Tile.CustomColor >= 8 ) )
        {
          for ( int i = 0; i < 4; ++i )
          {
            e.Graphics.DrawLine( System.Drawing.Pens.White,
                                  ( i * canvasEditor.ClientSize.Width ) / 4, 0,
                                  ( i * canvasEditor.ClientSize.Width ) / 4, canvasEditor.ClientSize.Height - 1 );
          }
          for ( int i = 0; i < 8; ++i )
          {
            e.Graphics.DrawLine( System.Drawing.Pens.White,
                                  0, ( i * canvasEditor.ClientSize.Height ) / 8,
                                  canvasEditor.ClientSize.Width - 1, ( i * canvasEditor.ClientSize.Height ) / 8 );
          }
        }
        else
        {
          for ( int i = 0; i < 8; ++i )
          {
            e.Graphics.DrawLine( System.Drawing.Pens.White,
                                  ( i * canvasEditor.ClientSize.Width ) / 8, 0,
                                  ( i * canvasEditor.ClientSize.Width ) / 8, canvasEditor.ClientSize.Height - 1 );

            e.Graphics.DrawLine( System.Drawing.Pens.White,
                                  0, ( i * canvasEditor.ClientSize.Height ) / 8,
                                  canvasEditor.ClientSize.Width - 1, ( i * canvasEditor.ClientSize.Height ) / 8 );
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



    private void btnAddCategory_Click( object sender, EventArgs e )
    {
      string    newCategory = editCategoryName.Text;

      UndoManager.AddUndoTask( new Undo.UndoCharsetAddCategory( this, m_Project, m_Project.Categories.Count ) );

      AddCategory( m_Project.Categories.Count, newCategory );

      RaiseModifiedEvent();
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
      RaiseModifiedEvent();
    }



    private void btnCollapseCategory_Click( object sender, EventArgs e )
    {
      // collapses similar looking characters
      if ( listCategories.SelectedItems.Count == 0 )
      {
        return;
      }

      for ( int i = 0; i < 256; ++i )
      {
        UndoManager.AddUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, i ), i == 0 );
      }

      int category = (int)listCategories.SelectedItems[0].Tag;
      int collapsedCount = 0;

      for ( int i = 0; i < 256 - collapsedCount; ++i )
      {
        if ( m_Project.Characters[i].Category == category )
        {
          for ( int j = i + 1; j < 256 - collapsedCount; ++j )
          {
            if ( m_Project.Characters[j].Category == category )
            {
              if ( ( m_Project.Characters[i].Tile.Data.Compare( m_Project.Characters[j].Tile.Data ) == 0 )
              &&   ( m_Project.Characters[i].Tile.CustomColor == m_Project.Characters[j].Tile.CustomColor ) )
              {
                // collapse!
                //Debug.Log( "Collapse " + j.ToString() + " into " + i.ToString() );
                for ( int l = j; l < 256 - 1 - collapsedCount; ++l )
                {
                  m_Project.Characters[l].Tile.Data = m_Project.Characters[l + 1].Tile.Data;
                  m_Project.Characters[l].Tile.CustomColor = m_Project.Characters[l + 1].Tile.CustomColor;
                  m_Project.Characters[l].Category = m_Project.Characters[l + 1].Category;
                }
                for ( int l = 0; l < 8; ++l )
                {
                  m_Project.Characters[255 - collapsedCount].Tile.Data.SetU8At( l, 0 );
                }
                m_Project.Characters[255 - collapsedCount].Tile.CustomColor = 0;
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

        RaiseModifiedEvent();
      }
    }



    private void btnSortCategories_Click( object sender, EventArgs e )
    {
      for ( int i = 0; i < 256; ++i )
      {
        UndoManager.AddUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, i ), i == 0 );
      }


      int[]   charMapNewToOld = new int[256];
      int[]   charMapOldToNew = new int[256];

      // resorts characters by category
      List<Formats.CharData>    newList = new List<C64Studio.Formats.CharData>();
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
      RaiseModifiedEvent();
    }



    private void btnReseatCategory_Click( object sender, EventArgs e )
    {
      if ( listCategories.SelectedItems.Count == 0 )
      {
        return;
      }

      for ( int i = 0; i < 256; ++i )
      {
        UndoManager.AddUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, i ), i == 0 );
      }

      int category = (int)listCategories.SelectedItems[0].Tag;
      int catTarget = GR.Convert.ToI32( editCollapseIndex.Text );
      int catTargetStart = catTarget;

      List<Formats.CharData> newList = new List<C64Studio.Formats.CharData>();

      int[]   charMapNewToOld = new int[256];
      int[]   charMapOldToNew = new int[256];

      int lastIndex = 0;

      // fill in front of target
      if ( catTargetStart > 0 )
      {
        for ( int j = 0; j < 256; ++j )
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
      for ( int j = 0; j < 256; ++j )
      {
        if ( m_Project.Characters[j].Category == category )
        {
          charMapOldToNew[j] = newList.Count;
          charMapNewToOld[newList.Count] = j;
          newList.Add( m_Project.Characters[j] );
        }
      }

      // fill after target
      for ( int j = lastIndex; j < 256; ++j )
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

      RaiseModifiedEvent();
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

        RaiseModifiedEvent();
      }
    }



    public void SwapCategories( int CategoryIndex1, int CategoryIndex2 )
    {
      string    category = m_Project.Categories[CategoryIndex1];

      m_Project.Categories.RemoveAt( CategoryIndex1 );
      m_Project.Categories.Insert( CategoryIndex2, category );

      // swap character categories as well
      for ( int i = 0; i < 256; ++i )
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

        RaiseModifiedEvent();
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



    private void btnEditPalette_Click( object sender, EventArgs e )
    {
      var dlgPalette = new DlgPaletteEditor( Core, m_Project.Colors.Palette );
      if ( dlgPalette.ShowDialog() == DialogResult.OK )
      {
        UndoManager.AddUndoTask( new Undo.UndoCharacterEditorValuesChange( this, m_Project ) );
        m_Project.Colors.Palette = dlgPalette.Palette;

        RaiseModifiedEvent();
        
        OnPaletteChanged();
      }
    }



    private void OnPaletteChanged()
    {
      btnEditPalette.Enabled = Lookup.HasCustomPalette( Lookup.GraphicTileModeFromTextCharMode( m_Project.Mode ) );

      PaletteManager.ApplyPalette( picturePlayground.DisplayPage, m_Project.Colors.Palette );
      PaletteManager.ApplyPalette( panelCharacters.DisplayPage, m_Project.Colors.Palette );
      PaletteManager.ApplyPalette( m_ImagePlayground, m_Project.Colors.Palette );
      PaletteManager.ApplyPalette( panelCharColors.DisplayPage, m_Project.Colors.Palette );
      panelCharacters.Items.Clear();
      for ( int i = 0; i < m_Project.TotalNumberOfCharacters; ++i )
      {
        PaletteManager.ApplyPalette( m_Project.Characters[i].Tile.Image, m_Project.Colors.Palette );
        RebuildCharImage( i );
        panelCharacters.Items.Add( i.ToString(), m_Project.Characters[i].Tile.Image );
      }

      // update controls for mode
      comboBGColor4.Enabled = ( m_Project.Mode == TextCharMode.COMMODORE_ECM );
      radioBGColor4.Enabled = ( m_Project.Mode == TextCharMode.COMMODORE_ECM );
      comboMulticolor1.Enabled = ( m_Project.Mode == TextCharMode.COMMODORE_MULTICOLOR );
      radioMultiColor1.Enabled = ( m_Project.Mode == TextCharMode.COMMODORE_MULTICOLOR );
      comboMulticolor2.Enabled = ( m_Project.Mode == TextCharMode.COMMODORE_MULTICOLOR );
      radioMulticolor2.Enabled = ( m_Project.Mode == TextCharMode.COMMODORE_MULTICOLOR );

      panelCharColors.Visible = Lookup.RequiresCustomColorForCharacter( m_Project.Mode );

      if ( m_Project.Mode == TextCharMode.COMMODORE_ECM )
      {
        radioMultiColor1.Text = "BGColor 2";
        radioMulticolor2.Text = "BGColor 3";
      }
      else
      {
        radioMultiColor1.Text = "Multicolor 1";
        radioMulticolor2.Text = "Multicolor 2";
      }

      panelCharacters.Invalidate();
      canvasEditor.Invalidate();
    }



    private void panelCharColors_PostPaint( FastImage TargetBuffer )
    {
      int     x1 = m_CurrentColor * TargetBuffer.Width / 16;
      int     x2 = ( m_CurrentColor + 1 ) * TargetBuffer.Width / 16;

      if ( Core != null )
      {
        uint  selColor = Core.Settings.FGColor( ColorableElement.SELECTION_FRAME );

        TargetBuffer.Rectangle( x1, 0, x2 - x1, TargetBuffer.Height, selColor );
      }
    }



  }
}
