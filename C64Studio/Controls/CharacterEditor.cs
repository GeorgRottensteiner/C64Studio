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
using RetroDevStudioModels;

namespace C64Studio.Controls
{
  public partial class CharacterEditor : UserControl
  {
    private enum ColorType
    {
      BACKGROUND = 0,
      MULTICOLOR_1,
      MULTICOLOR_2,
      CHAR_COLOR
    }

    public delegate void ModifiedHandler();
    public delegate void CharsetShiftedHandler( int[] OldToNew, int[] NewToOld );

    public event ModifiedHandler        Modified;
    public event CharsetShiftedHandler  CharactersShifted;

    private int                         m_CurrentChar = 0;
    private int                         m_CurrentColor = 1;
    private ColorType                   m_CurrentColorType = ColorType.CHAR_COLOR;
    private bool                        m_ButtonReleased = false;
    private bool                        m_RButtonReleased = false;
    public StudioCore                   Core = null;
    public Undo.UndoManager             UndoManager = null;
    private bool                        DoNotUpdateFromControls = false;

    private CharsetProject              m_Project = new CharsetProject();

    private GR.Image.MemoryImage        m_ImagePlayground = new GR.Image.MemoryImage( 256, 256, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );



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
      InitializeComponent();

      picturePlayground.DisplayPage.Create( 128, 128, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
      panelCharacters.PixelFormat = System.Drawing.Imaging.PixelFormat.Format8bppIndexed;
      panelCharacters.SetDisplaySize( 128, 128 );
      panelCharColors.DisplayPage.Create( 128, 8, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
      m_ImagePlayground.Create( 256, 256, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );

      CustomRenderer.PaletteManager.ApplyPalette( picturePlayground.DisplayPage );
      CustomRenderer.PaletteManager.ApplyPalette( panelCharacters.DisplayPage );
      CustomRenderer.PaletteManager.ApplyPalette( m_ImagePlayground );
      CustomRenderer.PaletteManager.ApplyPalette( panelCharColors.DisplayPage );
      for ( int i = 0; i < 256; ++i )
      {
        CustomRenderer.PaletteManager.ApplyPalette( m_Project.Characters[i].Image );
        panelCharacters.Items.Add( i.ToString(), m_Project.Characters[i].Image );
      }

      for ( int i = 0; i < 16; ++i )
      {
        comboBackground.Items.Add( i.ToString( "d2" ) );
        comboMulticolor1.Items.Add( i.ToString( "d2" ) );
        comboMulticolor2.Items.Add( i.ToString( "d2" ) );
        comboBGColor4.Items.Add( i.ToString( "d2" ) );
        comboCharColor.Items.Add( i.ToString( "d2" ) );
      }
      comboBackground.SelectedIndex = 0;
      comboMulticolor1.SelectedIndex = 0;
      comboMulticolor2.SelectedIndex = 0;
      comboBGColor4.SelectedIndex = 0;
      comboCharColor.SelectedIndex = 1;

      foreach ( TextMode mode in Enum.GetValues( typeof( TextMode ) ) )
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



    //protected void RaiseModifiedEvent()
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
      for ( int i = 0; i < 8; ++i )
      {
        panelCharColors.DisplayPage.SetPixel( m_CurrentColor * 8 + i, 0, 16 );
        panelCharColors.DisplayPage.SetPixel( m_CurrentColor * 8 + i, 7, 16 );
        panelCharColors.DisplayPage.SetPixel( m_CurrentColor * 8, i, 16 );
        panelCharColors.DisplayPage.SetPixel( m_CurrentColor * 8 + 7, i, 16 );
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
        // copy
        CopyToClipboard();
      }
      else if ( ( e.Modifiers == Keys.Control )
      &&        ( e.KeyCode == Keys.V ) )
      {
        // paste
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

      GR.Memory.ByteBuffer dataSelection = new GR.Memory.ByteBuffer();

      dataSelection.AppendI32( selectedImages.Count );
      dataSelection.AppendI32( panelCharacters.IsSelectionColumnBased ? 1 : 0 );
      int prevIndex = selectedImages[0];
      foreach ( int index in selectedImages )
      {
        // delta in indices
        dataSelection.AppendI32( index - prevIndex );
        prevIndex = index;

        dataSelection.AppendI32( (int)m_Project.Characters[index].Mode );
        dataSelection.AppendI32( m_Project.Characters[index].Color );
        dataSelection.AppendU32( 8 );
        dataSelection.AppendU32( 8 );
        dataSelection.AppendU32( m_Project.Characters[index].Data.Length );
        dataSelection.Append( m_Project.Characters[index].Data );
        dataSelection.AppendI32( index );
      }

      DataObject dataObj = new DataObject();

      dataObj.SetData( "C64Studio.ImageList", false, dataSelection.MemoryStream() );

      Clipboard.SetDataObject( dataObj, true );
    }



    private void PasteClipboardImageToChar()
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

          m_Project.Characters[pastePos].Mode = (TextMode)memIn.ReadInt32();
          m_Project.Characters[pastePos].Color = memIn.ReadInt32();

          uint width   = memIn.ReadUInt32();
          uint height  = memIn.ReadUInt32();

          uint dataLength = memIn.ReadUInt32();

          GR.Memory.ByteBuffer    tempBuffer = new GR.Memory.ByteBuffer();
          tempBuffer.Reserve( (int)dataLength );
          memIn.ReadBlock( tempBuffer, dataLength );

          m_Project.Characters[pastePos].Data = new GR.Memory.ByteBuffer( 8 );

          if ( ( width == 8 )
          && ( height == 8 ) )
          {
            tempBuffer.CopyTo( m_Project.Characters[pastePos].Data, 0, 8 );
          }
          else if ( ( width == 24 )
          && ( height == 21 ) )
          {
            for ( int j = 0; j < 8; ++j )
            {
              m_Project.Characters[pastePos].Data.SetU8At( j, tempBuffer.ByteAt( j * 3 ) );
            }
          }
          else
          {
            tempBuffer.CopyTo( m_Project.Characters[pastePos].Data, 0, Math.Min( 8, (int)dataLength ) );
          }


          int index = memIn.ReadInt32();

          RebuildCharImage( pastePos );
          panelCharacters.InvalidateItemRect( pastePos );

          if ( pastePos == m_CurrentChar )
          {
            comboCharColor.SelectedIndex = m_Project.Characters[pastePos].Color;
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

      if ( m_Project.Characters[CharIndex].Mode == RetroDevStudioModels.TextMode.COMMODORE_40_X_25_ECM )
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
        comboCharColor.SelectedIndex = m_Project.Characters[CharIndex].Color;

        comboCharsetMode.SelectedIndex = (int)m_Project.Characters[CharIndex].Mode;
      }

      RefreshCategoryCounts();
      DoNotUpdateFromControls = false;
      RaiseModifiedEvent();
    }



    void RebuildCharImage( int CharIndex )
    {
      Displayer.CharacterDisplayer.DisplayChar( m_Project, CharIndex, m_Project.Characters[CharIndex].Image, 0, 0 );

      bool playgroundChanged = false;
      for ( int i = 0; i < 16; ++i )
      {
        for ( int j = 0; j < 16; ++j )
        {
          if ( ( m_Project.PlaygroundChars[i + j * 16] & 0xff ) == CharIndex )
          {
            playgroundChanged = true;
            Displayer.CharacterDisplayer.DisplayChar( m_Project, CharIndex, m_ImagePlayground, i * 8, j * 8, m_Project.PlaygroundChars[i + j * 16] >> 8 );
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

      Types.MulticolorSettings   mcSettings = new Types.MulticolorSettings();
      mcSettings.BackgroundColor  = m_Project.BackgroundColor;
      mcSettings.MultiColor1      = m_Project.MultiColor1;
      mcSettings.MultiColor2      = m_Project.MultiColor2;

      bool pasteAsBlock = false;
      if ( !Core.MainForm.ImportImage( FromFile, Image, Types.GraphicType.CHARACTERS, mcSettings, out mappedImage, out mcSettings, out pasteAsBlock ) )
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
          else if ( currentTargetChar >= 256 )
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
            comboCharColor.SelectedIndex = m_Project.Characters[m_CurrentChar].Color;
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
        if ( comboCharColor.SelectedIndex != m_Project.Characters[m_CurrentChar].Color )
        {
          comboCharColor.SelectedIndex = m_Project.Characters[m_CurrentChar].Color;
        }
        if ( comboCharsetMode.SelectedIndex != (int)m_Project.Characters[m_CurrentChar].Mode )
        {
          comboCharsetMode.SelectedIndex = (int)m_Project.Characters[m_CurrentChar].Mode;
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
      GR.Memory.ByteBuffer Buffer = new GR.Memory.ByteBuffer( m_Project.Characters[CharIndex].Data );

      int   chosenCharColor = -1;

      bool  isMultiColor = false;

      unsafe
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
              if ( colorIndex == m_Project.BackgroundColor )
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
            && ( i != m_Project.BackgroundColor ) )
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
              if ( i != m_Project.BackgroundColor )
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

              if ( ColorIndex != m_Project.BackgroundColor )
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
              if ( ( i == m_Project.MultiColor1 )
              || ( i == m_Project.MultiColor2 )
              || ( i == m_Project.BackgroundColor ) )
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

              if ( ColorIndex == m_Project.BackgroundColor )
              {
                BitPattern = 0x00;
              }
              else if ( ColorIndex == m_Project.MultiColor1 )
              {
                BitPattern = 0x01;
              }
              else if ( ColorIndex == m_Project.MultiColor2 )
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
      for ( int i = 0; i < 8; ++i )
      {
        m_Project.Characters[CharIndex].Data.SetU8At( i, Buffer.ByteAt( i ) );
      }
      if ( chosenCharColor == -1 )
      {
        chosenCharColor = 0;
      }
      m_Project.Characters[CharIndex].Color = chosenCharColor;
      if ( ( isMultiColor )
      && ( chosenCharColor < 8 ) )
      {
        m_Project.Characters[CharIndex].Color = chosenCharColor + 8;
      }
      m_Project.Characters[CharIndex].Mode = isMultiColor ? TextMode.COMMODORE_40_X_25_MULTICOLOR : TextMode.COMMODORE_40_X_25_HIRES;
      RebuildCharImage( CharIndex );
      return true;
    }



    internal void CharsetUpdated( CharsetProject Project )
    {
      m_Project = Project;
      for ( int i = 0; i < 256; ++i )
      {
        RebuildCharImage( i );

        panelCharacters.Items[i].MemoryImage = m_Project.Characters[i].Image;
      }

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

      comboBackground.SelectedIndex   = m_Project.BackgroundColor;
      comboMulticolor1.SelectedIndex  = m_Project.MultiColor1;
      comboMulticolor2.SelectedIndex  = m_Project.MultiColor2;

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



    private void labelCharNo_Paint( object sender, PaintEventArgs e )
    {
      if ( Core == null )
      {
        return;
      }
      //e.Graphics.FillRectangle( new System.Drawing.SolidBrush( labelCharNo.BackColor ), labelCharNo.ClientRectangle );
      //e.Graphics.DrawString( "lsmf", labelCharNo.Font, new System.Drawing.SolidBrush( labelCharNo.ForeColor ), labelCharNo.ClientRectangle );
      // labelCharNo.Text

      if ( !Types.ConstantData.ScreenCodeToChar.ContainsKey( (byte)m_CurrentChar ) )
      {
        Debug.Log( "Missing char for " + m_CurrentChar );
      }
      else
      {
        int offset = (int)e.Graphics.MeasureString( labelCharNo.Text, labelCharNo.Font ).Width;
        e.Graphics.DrawString( "" + Types.ConstantData.ScreenCodeToChar[(byte)m_CurrentChar].CharValue, new System.Drawing.Font( Core.MainForm.m_FontC64.Families[0], 16, System.Drawing.GraphicsUnit.Pixel ), System.Drawing.SystemBrushes.WindowText, offset + 10, 0 );
      }
    }



    private void SelectCategory( int Category )
    {
      comboCategories.SelectedItem = m_Project.Categories[Category];
    }



    private void HandleMouseOnEditor( int X, int Y, MouseButtons Buttons )
    {
      int     charX = X / ( canvasEditor.ClientRectangle.Width / 8 );
      int     charY = Y / ( canvasEditor.ClientRectangle.Height / 8 );

      // iron out rounding bugs
      for ( int i = 0; i < 7; ++i )
      {
        if ( ( X >= ( i * canvasEditor.ClientRectangle.Width ) / 8 )
        &&   ( X < ( ( i + 1 )  * canvasEditor.ClientRectangle.Width ) / 8 ) )
        {
          charX = i;
        }
        if ( ( Y >= ( i * canvasEditor.ClientRectangle.Height ) / 8 )
        &&   ( Y < ( ( i + 1 )  * canvasEditor.ClientRectangle.Height ) / 8 ) )
        {
          charY = i;
        }
      }

      int     affectedCharIndex = m_CurrentChar;
      var     origAffectedChar = m_Project.Characters[m_CurrentChar];
      var     affectedChar = m_Project.Characters[m_CurrentChar];
      if ( affectedChar.Mode == RetroDevStudioModels.TextMode.COMMODORE_40_X_25_ECM )
      {
        affectedCharIndex %= 64;
        affectedChar = m_Project.Characters[affectedCharIndex];
      }

      if ( ( Buttons & MouseButtons.Left ) != 0 )
      {
        byte    charByte = affectedChar.Data.ByteAt( charY );
        byte    newByte = charByte;
        int     colorIndex = affectedChar.Color;

        if ( ( affectedChar.Mode != RetroDevStudioModels.TextMode.COMMODORE_40_X_25_MULTICOLOR )
        ||   ( affectedChar.Color < 8 ) )
        {
          // single color
          charX = 7 - charX;
          if ( m_CurrentColorType == ColorType.CHAR_COLOR )
          {
            newByte |= (byte)( 1 << charX );
          }
          else
          {
            newByte &= (byte)~( 1 << charX );
            colorIndex = m_Project.BackgroundColor;
          }
        }
        else
        {
          // multi color
          charX = X / ( canvasEditor.ClientRectangle.Width / 4 );
          charX = 3 - charX;

          newByte &= (byte)~( 3 << ( 2 * charX ) );

          int     replacementBytes = 0;

          switch ( m_CurrentColorType )
          {
            case ColorType.BACKGROUND:
              colorIndex = m_Project.BackgroundColor;
              break;
            case ColorType.CHAR_COLOR:
              replacementBytes = 3;
              colorIndex = affectedChar.Color;
              break;
            case ColorType.MULTICOLOR_1:
              replacementBytes = 1;
              colorIndex = m_Project.MultiColor1;
              break;
            case ColorType.MULTICOLOR_2:
              replacementBytes = 2;
              colorIndex = m_Project.MultiColor2;
              break;
          }
          newByte |= (byte)( replacementBytes << ( 2 * charX ) );
        }
        if ( newByte != charByte )
        {
          RaiseModifiedEvent();

          if ( m_ButtonReleased )
          {
            UndoManager.AddUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, affectedCharIndex ) );
            m_ButtonReleased = false;
          }

          affectedChar.Data.SetU8At( charY, newByte );

          if ( origAffectedChar.Mode == RetroDevStudioModels.TextMode.COMMODORE_40_X_25_ECM )
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
        byte charByte = affectedChar.Data.ByteAt( charY );
        byte newByte = charByte;

        if ( ( affectedChar.Mode != RetroDevStudioModels.TextMode.COMMODORE_40_X_25_MULTICOLOR )
        ||   ( affectedChar.Color < 8 ) )
        {
          // single color
          charX = 7 - charX;
          newByte &= (byte)~( 1 << charX );
        }
        else
        {
          // multi color
          charX = X / ( canvasEditor.ClientRectangle.Width / 4 );
          charX = 3 - charX;

          newByte &= (byte)~( 3 << ( 2 * charX ) );
        }
        if ( newByte != charByte )
        {
          if ( m_RButtonReleased )
          {
            m_RButtonReleased = false;
            UndoManager.AddUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, affectedCharIndex ) );
          }

          RaiseModifiedEvent();
          affectedChar.Data.SetU8At( charY, newByte );

          if ( origAffectedChar.Mode == RetroDevStudioModels.TextMode.COMMODORE_40_X_25_ECM )
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

      for ( int i = 0; i < 256; ++i )
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

      for ( int i = 0; i < 256; ++i )
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
      m_CurrentColorType = ColorType.CHAR_COLOR;
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

        for ( int y = 0; y < 8; ++y )
        {
          byte result = (byte)( ~m_Project.Characters[index].Data.ByteAt( y ) );
          m_Project.Characters[index].Data.SetU8At( y, result );
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
        if ( m_Project.PlaygroundChars[charX + charY * 16] != (ushort)( m_CurrentChar | ( m_CurrentColor << 8 ) ) )
        {
          UndoManager.AddUndoTask( new Undo.UndoCharacterEditorPlaygroundCharChange( this, m_Project, charX, charY ) );

          Displayer.CharacterDisplayer.DisplayChar( m_Project, m_CurrentChar, m_ImagePlayground, charX * 8, charY * 8, m_CurrentColor );
          RedrawPlayground();

          m_Project.PlaygroundChars[charX + charY * 16] = (ushort)( m_CurrentChar | ( m_CurrentColor << 8 ) );
          RaiseModifiedEvent();
        }
      }
      if ( ( Buttons & MouseButtons.Right ) != 0 )
      {
        m_CurrentChar = (byte)( m_Project.PlaygroundChars[charX + charY * 16] & 0x00ff );
        m_CurrentColor = (byte)( m_Project.PlaygroundChars[charX + charY * 16] >> 8 );
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
          if ( ( processedChar.Mode == RetroDevStudioModels.TextMode.COMMODORE_40_X_25_MULTICOLOR )
          &&   ( processedChar.Color >= 8 ) )
          {
            byte result = (byte)( (byte)( ( processedChar.Data.ByteAt( y ) & 0xc0 ) >> 6 )
                                | (byte)( ( processedChar.Data.ByteAt( y ) & 0x30 ) >> 2 )
                                | (byte)( ( processedChar.Data.ByteAt( y ) & 0x0c ) << 2 )
                                | (byte)( ( processedChar.Data.ByteAt( y ) & 0x03 ) << 6 ) );
            processedChar.Data.SetU8At( y, result );
          }
          else
          {
            byte result = (byte)( (byte)( ( processedChar.Data.ByteAt( y ) & 0x80 ) >> 7 )
                                | (byte)( ( processedChar.Data.ByteAt( y ) & 0x40 ) >> 5 )
                                | (byte)( ( processedChar.Data.ByteAt( y ) & 0x20 ) >> 3 )
                                | (byte)( ( processedChar.Data.ByteAt( y ) & 0x10 ) >> 1 )
                                | (byte)( ( processedChar.Data.ByteAt( y ) & 0x08 ) << 1 )
                                | (byte)( ( processedChar.Data.ByteAt( y ) & 0x04 ) << 3 )
                                | (byte)( ( processedChar.Data.ByteAt( y ) & 0x02 ) << 5 )
                                | (byte)( ( processedChar.Data.ByteAt( y ) & 0x01 ) << 7 ) );
            processedChar.Data.SetU8At( y, result );
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

        for ( int y = 0; y < 4; ++y )
        {
          byte oldValue = m_Project.Characters[index].Data.ByteAt( y );
          m_Project.Characters[index].Data.SetU8At( y, m_Project.Characters[index].Data.ByteAt( 7 - y ) );
          m_Project.Characters[index].Data.SetU8At( 7 - y, oldValue );
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

        GR.Memory.ByteBuffer resultData = new GR.Memory.ByteBuffer( 8 );

        if ( ( m_Project.Characters[index].Mode == RetroDevStudioModels.TextMode.COMMODORE_40_X_25_MULTICOLOR )
        ||   ( m_Project.Characters[index].Color >= 8 ) )
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
              byte sourceColor = (byte)( ( m_Project.Characters[index].Data.ByteAt( sourceY ) & ( 3 << maskOffset ) ) >> maskOffset );

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
              if ( ( m_Project.Characters[index].Data.ByteAt( sourceY ) & ( 1 << ( 7 - ( sourceX % 8 ) ) ) ) != 0 )
              {
                resultData.SetU8At( targetY, (byte)( resultData.ByteAt( targetY ) | ( 1 << ( 7 - targetX % 8 ) ) ) );
              }
            }
          }
        }
        m_Project.Characters[index].Data = resultData;
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

        GR.Memory.ByteBuffer resultData = new GR.Memory.ByteBuffer( 8 );

        if ( ( m_Project.Characters[index].Mode == RetroDevStudioModels.TextMode.COMMODORE_40_X_25_MULTICOLOR )
        ||   ( m_Project.Characters[index].Color >= 8 ) )
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
              byte sourceColor = (byte)( ( m_Project.Characters[index].Data.ByteAt( sourceY ) & ( 3 << maskOffset ) ) >> maskOffset );

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
              if ( ( m_Project.Characters[index].Data.ByteAt( sourceY ) & ( 1 << ( 7 - ( sourceX % 8 ) ) ) ) != 0 )
              {
                resultData.SetU8At( targetY, (byte)( resultData.ByteAt( targetY ) | ( 1 << ( 7 - targetX % 8 ) ) ) );
              }
            }
          }
        }
        m_Project.Characters[index].Data = resultData;
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

        byte  temp = m_Project.Characters[index].Data.ByteAt( 7 );
        for ( int y = 0; y < 7; ++y )
        {
          m_Project.Characters[index].Data.SetU8At( 7 - y, m_Project.Characters[index].Data.ByteAt( 7 - y - 1 ) );
        }
        m_Project.Characters[index].Data.SetU8At( 0, temp );
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

        byte  temp = m_Project.Characters[index].Data.ByteAt( 0 );
        for ( int y = 0; y < 7; ++y )
        {
          m_Project.Characters[index].Data.SetU8At( y, m_Project.Characters[index].Data.ByteAt( y + 1 ) );
        }
        m_Project.Characters[index].Data.SetU8At( 7, temp );
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
          if ( ( m_Project.Characters[index].Mode == RetroDevStudioModels.TextMode.COMMODORE_40_X_25_MULTICOLOR )
          &&   ( m_Project.Characters[index].Color >= 8 ) )
          {
            byte result = (byte)( (byte)( ( m_Project.Characters[m_CurrentChar].Data.ByteAt( y ) & 0xc0 ) >> 6 )
                                | (byte)( ( m_Project.Characters[index].Data.ByteAt( y ) & 0x3f ) << 2 ) );
            m_Project.Characters[index].Data.SetU8At( y, result );
          }
          else
          {
            byte result = (byte)( (byte)( ( m_Project.Characters[index].Data.ByteAt( y ) & 0x80 ) >> 7 )
                                | (byte)( ( m_Project.Characters[index].Data.ByteAt( y ) & 0x7f ) << 1 ) );
            m_Project.Characters[index].Data.SetU8At( y, result );
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
          if ( ( m_Project.Characters[index].Mode == RetroDevStudioModels.TextMode.COMMODORE_40_X_25_MULTICOLOR )
          && ( m_Project.Characters[index].Color >= 8 ) )
          {
            byte result = (byte)( (byte)( ( m_Project.Characters[index].Data.ByteAt( y ) & 0xfc ) >> 2 )
                                | (byte)( ( m_Project.Characters[index].Data.ByteAt( y ) & 0x03 ) << 6 ) );
            m_Project.Characters[index].Data.SetU8At( y, result );
          }
          else
          {
            byte result = (byte)( (byte)( ( m_Project.Characters[index].Data.ByteAt( y ) & 0x01 ) << 7 )
                                | (byte)( ( m_Project.Characters[index].Data.ByteAt( y ) & 0xfe ) >> 1 ) );
            m_Project.Characters[index].Data.SetU8At( y, result );
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

      Core?.Theming.DrawSingleColorComboBox( combo, e );
    }



    private void comboMulticolor_DrawItem( object sender, DrawItemEventArgs e )
    {
      ComboBox combo = (ComboBox)sender;

      Core?.Theming.DrawMultiColorComboBox( combo, e );
    }



    public void PlaygroundCharacterChanged( int X, int Y )
    {
      Displayer.CharacterDisplayer.DisplayChar( m_Project, m_Project.PlaygroundChars[X + Y * 16] & 0xff, m_ImagePlayground, X * 8, Y * 8, m_Project.PlaygroundChars[X + Y * 16] >> 8 );
      RedrawPlayground();
    }



    private void comboBackground_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( m_Project.BackgroundColor != comboBackground.SelectedIndex )
      {
        UndoManager.AddUndoTask( new Undo.UndoCharacterEditorValuesChange( this, m_Project ) );

        m_Project.BackgroundColor = comboBackground.SelectedIndex;
        for ( int i = 0; i < 256; ++i )
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
      if ( m_Project.MultiColor1 != comboMulticolor1.SelectedIndex )
      {
        UndoManager.AddUndoTask( new Undo.UndoCharacterEditorValuesChange( this, m_Project ) );

        m_Project.MultiColor1 = comboMulticolor1.SelectedIndex;
        for ( int i = 0; i < 256; ++i )
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
      if ( m_Project.MultiColor2 != comboMulticolor2.SelectedIndex )
      {
        UndoManager.AddUndoTask( new Undo.UndoCharacterEditorValuesChange( this, m_Project ) );

        m_Project.MultiColor2 = comboMulticolor2.SelectedIndex;
        for ( int i = 0; i < 256; ++i )
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
      if ( m_Project.BGColor4 != comboBGColor4.SelectedIndex )
      {
        UndoManager.AddUndoTask( new Undo.UndoCharacterEditorValuesChange( this, m_Project ) );

        m_Project.BGColor4 = comboBGColor4.SelectedIndex;
        for ( int i = 0; i < 256; ++i )
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

      List<int>   selectedChars = panelCharacters.SelectedIndices;
      if ( selectedChars.Count == 0 )
      {
        selectedChars.Add( m_CurrentChar );
      }

      bool    modified = false;
      foreach ( int selChar in selectedChars )
      {
        if ( m_Project.Characters[selChar].Color != comboCharColor.SelectedIndex )
        {
          UndoManager.AddUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, selChar ), modified == false );

          m_Project.Characters[selChar].Color = comboCharColor.SelectedIndex;
          RebuildCharImage( selChar );
          modified = true;
          panelCharacters.InvalidateItemRect( selChar );
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
      comboMulticolor1.SelectedIndex = m_Project.MultiColor1;
      comboMulticolor2.SelectedIndex = m_Project.MultiColor2;
      comboBGColor4.SelectedIndex = m_Project.BGColor4;
      comboBackground.SelectedIndex = m_Project.BackgroundColor;

      for ( int i = 0; i < 256; ++i )
      {
        RebuildCharImage( i );
      }
      canvasEditor.Invalidate();
      panelCharacters.Invalidate();
    }



    private void comboCharsetMode_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( DoNotUpdateFromControls )
      {
        return;
      }

      List<int>   selectedChars = panelCharacters.SelectedIndices;
      if ( selectedChars.Count == 0 )
      {
        selectedChars.Add( m_CurrentChar );
      }

      bool modified = false;
      foreach ( int selChar in selectedChars )
      {
        if ( m_Project.Characters[selChar].Mode != (TextMode)comboCharsetMode.SelectedIndex )
        {
          UndoManager.AddUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, selChar ), !modified );
          m_Project.Characters[selChar].Mode = (TextMode)comboCharsetMode.SelectedIndex;
          RebuildCharImage( selChar );
          panelCharacters.InvalidateItemRect( selChar );
          modified = true;
        }
      }

      comboBGColor4.Enabled = ( m_Project.Characters[m_CurrentChar].Mode == RetroDevStudioModels.TextMode.COMMODORE_40_X_25_ECM );
      radioBGColor4.Enabled = ( m_Project.Characters[m_CurrentChar].Mode == RetroDevStudioModels.TextMode.COMMODORE_40_X_25_ECM );
      comboMulticolor1.Enabled = ( m_Project.Characters[m_CurrentChar].Mode != RetroDevStudioModels.TextMode.COMMODORE_40_X_25_HIRES );
      radioMultiColor1.Enabled = ( m_Project.Characters[m_CurrentChar].Mode != RetroDevStudioModels.TextMode.COMMODORE_40_X_25_HIRES );
      comboMulticolor2.Enabled = ( m_Project.Characters[m_CurrentChar].Mode != RetroDevStudioModels.TextMode.COMMODORE_40_X_25_HIRES );
      radioMulticolor2.Enabled = ( m_Project.Characters[m_CurrentChar].Mode != RetroDevStudioModels.TextMode.COMMODORE_40_X_25_HIRES );

      if ( m_Project.Characters[m_CurrentChar].Mode == RetroDevStudioModels.TextMode.COMMODORE_40_X_25_ECM )
      {
        radioMultiColor1.Text = "BGColor 2";
        radioMulticolor2.Text = "BGColor 3";
      }
      else
      {
        radioMultiColor1.Text = "Multicolor 1";
        radioMulticolor2.Text = "Multicolor 2";
      }

      if ( modified )
      {
        RaiseModifiedEvent();
        canvasEditor.Invalidate();
      }
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
      int   temp = m_Project.MultiColor1;
      m_Project.MultiColor1 = m_Project.MultiColor2;
      m_Project.MultiColor2 = temp;

      int   charIndex = 0;
      foreach ( var charInfo in m_Project.Characters )
      {
        if ( ( charInfo.Mode == RetroDevStudioModels.TextMode.COMMODORE_40_X_25_MULTICOLOR )
        &&   ( charInfo.Color >= 8 ) )
        {
          for ( int y = 0; y < 8; ++y )
          {
            for ( int x = 0; x < 4; ++x )
            {
              int pixelValue = ( charInfo.Data.ByteAt( y ) & ( 3 << ( ( 3 - x ) * 2 ) ) ) >> ( ( 3 - x ) * 2 );

              if ( pixelValue == 1 )
              {
                byte  newValue = (byte)( charInfo.Data.ByteAt( y ) & ~( 3 << ( ( 3 - x ) * 2 ) ) );

                newValue |= (byte)( 2 << ( ( 3 - x ) * 2 ) );

                charInfo.Data.SetU8At( y, newValue );
              }
              else if ( pixelValue == 2 )
              {
                byte  newValue = (byte)( charInfo.Data.ByteAt( y ) & ~( 3 << ( ( 3 - x ) * 2 ) ) );

                newValue |= (byte)( 1 << ( ( 3 - x ) * 2 ) );

                charInfo.Data.SetU8At( y, newValue );
              }
            }
          }
          RebuildCharImage( charIndex );
          panelCharacters.Invalidate();
        }
        ++charIndex;
      }
      comboMulticolor1.SelectedIndex = m_Project.MultiColor1;
      comboMulticolor2.SelectedIndex = m_Project.MultiColor2;

      RaiseModifiedEvent();
    }



    private void exchangeMultiColor1AndBGColorToolStripMenuItem_Click( object sender, EventArgs e )
    {
      UndoManager.AddUndoTask( new Undo.UndoCharacterEditorExchangeMultiColors( this, C64Studio.Undo.UndoCharacterEditorExchangeMultiColors.ExchangeMode.MULTICOLOR_1_WITH_BACKGROUND ) );

      ExchangeMultiColor1WithBackground();
    }



    public void ExchangeMultiColor1WithBackground()
    {
      int   temp = m_Project.BackgroundColor;
      m_Project.BackgroundColor = m_Project.MultiColor1;
      m_Project.MultiColor1 = temp;

      int   charIndex = 0;
      foreach ( var charInfo in m_Project.Characters )
      {
        if ( ( charInfo.Mode == RetroDevStudioModels.TextMode.COMMODORE_40_X_25_MULTICOLOR )
        &&   ( charInfo.Color >= 8 ) )
        {
          for ( int y = 0; y < 8; ++y )
          {
            for ( int x = 0; x < 4; ++x )
            {
              int pixelValue = ( charInfo.Data.ByteAt( y ) & ( 3 << ( ( 3 - x ) * 2 ) ) ) >> ( ( 3 - x ) * 2 );

              if ( pixelValue == 1 )
              {
                byte  newValue = (byte)( charInfo.Data.ByteAt( y ) & ~( 3 << ( ( 3 - x ) * 2 ) ) );

                charInfo.Data.SetU8At( y, newValue );
              }
              else if ( pixelValue == 0 )
              {
                byte  newValue = (byte)( charInfo.Data.ByteAt( y ) & ~( 3 << ( ( 3 - x ) * 2 ) ) );

                newValue |= (byte)( 1 << ( ( 3 - x ) * 2 ) );

                charInfo.Data.SetU8At( y, newValue );
              }
            }
          }
        }
        RebuildCharImage( charIndex );
        panelCharacters.Invalidate();
        ++charIndex;
      }
      comboMulticolor1.SelectedIndex  = m_Project.MultiColor1;
      comboBackground.SelectedIndex   = m_Project.BackgroundColor;

      RaiseModifiedEvent();
    }



    private void exchangeMultiColor2AndBGColorToolStripMenuItem_Click( object sender, EventArgs e )
    {
      UndoManager.AddUndoTask( new Undo.UndoCharacterEditorExchangeMultiColors( this, C64Studio.Undo.UndoCharacterEditorExchangeMultiColors.ExchangeMode.MULTICOLOR_2_WITH_BACKGROUND ) );

      ExchangeMultiColor2WithBackground();
    }



    public void ExchangeMultiColor2WithBackground()
    {
      int   temp = m_Project.BackgroundColor;
      m_Project.BackgroundColor = m_Project.MultiColor2;
      m_Project.MultiColor2 = temp;

      int   charIndex = 0;
      foreach ( var charInfo in m_Project.Characters )
      {
        if ( ( charInfo.Mode == RetroDevStudioModels.TextMode.COMMODORE_40_X_25_MULTICOLOR )
        &&   ( charInfo.Color >= 8 ) )
        {
          for ( int y = 0; y < 8; ++y )
          {
            for ( int x = 0; x < 4; ++x )
            {
              int pixelValue = ( charInfo.Data.ByteAt( y ) & ( 3 << ( ( 3 - x ) * 2 ) ) ) >> ( ( 3 - x ) * 2 );

              if ( pixelValue == 2 )
              {
                byte  newValue = (byte)( charInfo.Data.ByteAt( y ) & ~( 3 << ( ( 3 - x ) * 2 ) ) );

                //newValue |= (byte)( 2 << ( ( 3 - x ) * 2 ) );

                charInfo.Data.SetU8At( y, newValue );
              }
              else if ( pixelValue == 0 )
              {
                byte  newValue = (byte)( charInfo.Data.ByteAt( y ) & ~( 3 << ( ( 3 - x ) * 2 ) ) );

                newValue |= (byte)( 2 << ( ( 3 - x ) * 2 ) );

                charInfo.Data.SetU8At( y, newValue );
              }
            }
          }
        }
        RebuildCharImage( charIndex );
        panelCharacters.Invalidate();
        ++charIndex;
      }
      comboMulticolor2.SelectedIndex = m_Project.MultiColor2;
      comboBackground.SelectedIndex = m_Project.BackgroundColor;

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

        for ( int j = 0; j < 8; ++j )
        {
          m_Project.Characters[i].Data.SetU8At( j, 0 );
        }
        RebuildCharImage( i );
        panelCharacters.InvalidateItemRect( i );

        if ( i == m_CurrentChar )
        {
          comboCharsetMode.SelectedIndex = (int)TextMode.COMMODORE_40_X_25_HIRES;
          comboCharColor.SelectedIndex = m_Project.Characters[i].Color;
        }
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
      if ( targetIndex + selection.Count > 256 )
      {
        MessageBox.Show( "Not enough chars for selection starting at the given index!", "Can't move selection" );
        return;
      }

      int[]   charMapNewToOld = new int[256];
      int[]   charMapOldToNew = new int[256];
      for ( int i = 0; i < 256; ++i )
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

      for ( int i = 0; i < 256; ++i )
      {
        UndoManager.AddUndoTask( new Undo.UndoCharacterEditorCharChange( this, m_Project, i ), i == 0 );
      }

      // ..and charset
      List<CharData>    origCharData = new List<CharData>();
      List<GR.Forms.ImageListbox.ImageListItem>    origListItems = new List<GR.Forms.ImageListbox.ImageListItem>();
      List<GR.Forms.ImageListbox.ImageListItem>    origListItems2 = new List<GR.Forms.ImageListbox.ImageListItem>();

      for ( int i = 0; i < 256; ++i )
      {
        origCharData.Add( m_Project.Characters[i] );
        origListItems.Add( panelCharacters.Items[i] );
      }

      for ( int i = 0; i < 256; ++i )
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

      Displayer.CharacterDisplayer.DisplayChar( m_Project, m_CurrentChar, new CustomDrawControlContext( e.Graphics, canvasEditor.ClientSize.Width, canvasEditor.ClientSize.Height ) );

      if ( m_Project.ShowGrid )
      {
        if ( ( m_Project.Characters[m_CurrentChar].Mode == RetroDevStudioModels.TextMode.COMMODORE_40_X_25_MULTICOLOR )
        &&   ( m_Project.Characters[m_CurrentChar].Color >= 8 ) )
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
              if ( ( m_Project.Characters[i].Data.Compare( m_Project.Characters[j].Data ) == 0 )
              &&   ( m_Project.Characters[i].Color == m_Project.Characters[j].Color ) )
              {
                // collapse!
                //Debug.Log( "Collapse " + j.ToString() + " into " + i.ToString() );
                for ( int l = j; l < 256 - 1 - collapsedCount; ++l )
                {
                  m_Project.Characters[l].Data = m_Project.Characters[l + 1].Data;
                  m_Project.Characters[l].Color = m_Project.Characters[l + 1].Color;
                  m_Project.Characters[l].Category = m_Project.Characters[l + 1].Category;
                  m_Project.Characters[l].Mode = m_Project.Characters[l + 1].Mode;
                }
                for ( int l = 0; l < 8; ++l )
                {
                  m_Project.Characters[255 - collapsedCount].Data.SetU8At( l, 0 );
                }
                m_Project.Characters[255 - collapsedCount].Color = 0;
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



  }
}
