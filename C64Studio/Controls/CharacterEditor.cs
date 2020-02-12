using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using C64Studio.Formats;

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


    public CharsetProject Project
    {
      get; set;
    }

    public bool Modified
    {
      get; set;
    }



    private int                         m_CurrentChar = 0;
    private int                         m_CurrentColor = 1;
    private ColorType                   m_CurrentColorType = ColorType.CHAR_COLOR;
    private bool                        m_ButtonReleased = false;
    private bool                        m_RButtonReleased = false;
    private StudioCore                  Core = null;
    private bool                        DoNotUpdateFromControls = false;

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
      Modified = false;
      Project = new CharsetProject();

      InitializeComponent();

      pictureEditor.DisplayPage.Create( 8, 8, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
      pictureEditor.PostPaint += new GR.Forms.FastPictureBox.PostPaintCallback( pictureEditor_PostPaint );
      picturePlayground.DisplayPage.Create( 128, 128, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
      panelCharacters.PixelFormat = System.Drawing.Imaging.PixelFormat.Format8bppIndexed;
      panelCharacters.SetDisplaySize( 128, 128 );
      panelCharColors.DisplayPage.Create( 128, 8, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
      m_ImagePlayground.Create( 256, 256, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );

      CustomRenderer.PaletteManager.ApplyPalette( pictureEditor.DisplayPage );
      CustomRenderer.PaletteManager.ApplyPalette( picturePlayground.DisplayPage );
      CustomRenderer.PaletteManager.ApplyPalette( panelCharacters.DisplayPage );
      CustomRenderer.PaletteManager.ApplyPalette( m_ImagePlayground );
      CustomRenderer.PaletteManager.ApplyPalette( panelCharColors.DisplayPage );
      for ( int i = 0; i < 256; ++i )
      {
        CustomRenderer.PaletteManager.ApplyPalette( Project.Characters[i].Image );
        panelCharacters.Items.Add( i.ToString(), Project.Characters[i].Image );
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

      comboCharsetMode.Items.Add( "HiRes" );
      comboCharsetMode.Items.Add( "MultiColor" );
      comboCharsetMode.Items.Add( "Enhanced Char Mode (ECM)" );
      comboCharsetMode.SelectedIndex = 0;

      radioCharColor.Checked = true;
      panelCharacters.SelectedIndex = 0;

      ListViewItem    itemUn = new ListViewItem( "Uncategorized" );
      itemUn.Tag = 0;
      itemUn.SubItems.Add( "0" );
      comboCategories.Items.Add( itemUn.Name );

      RedrawColorChooser();

      panelCharacters.KeyDown += new KeyEventHandler( HandleKeyDown );
      pictureEditor.PreviewKeyDown += new PreviewKeyDownEventHandler( pictureEditor_PreviewKeyDown );
    }



    void pictureEditor_PostPaint( GR.Image.FastImage TargetBuffer )
    {
      if ( Project.ShowGrid )
      {
        if ( ( Project.Characters[m_CurrentChar].Mode == C64Studio.Types.CharsetMode.MULTICOLOR )
        &&   ( Project.Characters[m_CurrentChar].Color >= 8 ) )
        {
          for ( int i = 0; i < 4; ++i )
          {
            for ( int j = 0; j < TargetBuffer.Height; ++j )
            {
              TargetBuffer.SetPixel( i * ( pictureEditor.ClientRectangle.Width / 4 ), j, 0xffffffff );
            }
          }
          for ( int i = 0; i < 8; ++i )
          {
            for ( int j = 0; j < TargetBuffer.Height; ++j )
            {
              TargetBuffer.SetPixel( j, i * ( pictureEditor.ClientRectangle.Width / 8 ), 0xffffffff );
            }
          }
        }
        else
        {
          for ( int i = 0; i < 8; ++i )
          {
            for ( int j = 0; j < TargetBuffer.Width; ++j )
            {
              TargetBuffer.SetPixel( i * ( pictureEditor.ClientRectangle.Width / 8 ), j, 0xffffffff );
            }
            for ( int j = 0; j < TargetBuffer.Height; ++j )
            {
              TargetBuffer.SetPixel( j, i * ( pictureEditor.ClientRectangle.Width / 8 ), 0xffffffff );
            }
          }
        }
      }
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
        Displayer.CharacterDisplayer.DisplayChar( Project, m_CurrentChar, panelCharColors.DisplayPage, i * 8, 0, i );
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



    void pictureEditor_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
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

        dataSelection.AppendI32( (int)Project.Characters[index].Mode );
        dataSelection.AppendI32( Project.Characters[index].Color );
        dataSelection.AppendU32( 8 );
        dataSelection.AppendU32( 8 );
        dataSelection.AppendU32( Project.Characters[index].Data.Length );
        dataSelection.Append( Project.Characters[index].Data );
        dataSelection.AppendI32( index );
      }

      DataObject dataObj = new DataObject();

      dataObj.SetData( "C64Studio.ImageList", false, dataSelection.MemoryStream() );

      Clipboard.SetDataObject( dataObj );
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

          if ( pastePos >= Project.Characters.Count )
          {
            break;
          }

          // TODO!!
          //DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharacterEditorCharChange( this, Project, pastePos ), i == 0 );

          Project.Characters[pastePos].Mode = (C64Studio.Types.CharsetMode)memIn.ReadInt32();
          Project.Characters[pastePos].Color = memIn.ReadInt32();

          uint width   = memIn.ReadUInt32();
          uint height  = memIn.ReadUInt32();

          uint dataLength = memIn.ReadUInt32();

          GR.Memory.ByteBuffer    tempBuffer = new GR.Memory.ByteBuffer();
          tempBuffer.Reserve( (int)dataLength );
          memIn.ReadBlock( tempBuffer, dataLength );

          Project.Characters[pastePos].Data = new GR.Memory.ByteBuffer( 8 );

          if ( ( width == 8 )
          && ( height == 8 ) )
          {
            tempBuffer.CopyTo( Project.Characters[pastePos].Data, 0, 8 );
          }
          else if ( ( width == 24 )
          && ( height == 21 ) )
          {
            for ( int j = 0; j < 8; ++j )
            {
              Project.Characters[pastePos].Data.SetU8At( j, tempBuffer.ByteAt( j * 3 ) );
            }
          }
          else
          {
            tempBuffer.CopyTo( Project.Characters[pastePos].Data, 0, Math.Min( 8, (int)dataLength ) );
          }


          int index = memIn.ReadInt32();

          RebuildCharImage( pastePos );
          panelCharacters.InvalidateItemRect( pastePos );

          if ( pastePos == m_CurrentChar )
          {
            comboCharColor.SelectedIndex = Project.Characters[pastePos].Color;
          }
        }
        pictureEditor.Invalidate();
        Modified = true;
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

      if ( Project.Characters[CharIndex].Mode == C64Studio.Types.CharsetMode.ECM )
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
        comboCharColor.SelectedIndex = Project.Characters[CharIndex].Color;

        comboCharsetMode.SelectedIndex = (int)Project.Characters[CharIndex].Mode;
      }

      DoNotUpdateFromControls = false;
    }



    void RebuildCharImage( int CharIndex )
    {
      Displayer.CharacterDisplayer.DisplayChar( Project, CharIndex, Project.Characters[CharIndex].Image, 0, 0 );

      bool playgroundChanged = false;
      for ( int i = 0; i < 16; ++i )
      {
        for ( int j = 0; j < 16; ++j )
        {
          if ( ( Project.PlaygroundChars[i + j * 16] & 0xff ) == CharIndex )
          {
            playgroundChanged = true;
            Displayer.CharacterDisplayer.DisplayChar( Project, CharIndex, m_ImagePlayground, i * 8, j * 8, Project.PlaygroundChars[i + j * 16] >> 8 );
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



    private void PasteImage( string FromFile, GR.Image.FastImage Image, bool ForceMulticolor )
    {
      GR.Image.FastImage mappedImage = null;

      Types.MulticolorSettings   mcSettings = new Types.MulticolorSettings();
      mcSettings.BackgroundColor  = Project.BackgroundColor;
      mcSettings.MultiColor1      = Project.MultiColor1;
      mcSettings.MultiColor2      = Project.MultiColor2;

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
            comboCharColor.SelectedIndex = Project.Characters[m_CurrentChar].Color;
          }
          if ( !pasteAsBlock )
          {
            ++currentTargetChar;
          }
        }
      }

      pictureEditor.Invalidate();
      Modified = true;
    }



    private void panelCharacters_SelectionChanged( object sender, EventArgs e )
    {
      int newChar = panelCharacters.SelectedIndex;
      if ( ( newChar != -1 )
      && ( panelCharacters.SelectedIndices.Count == 1 ) )
      {
        labelCharNo.Text = "Character: " + newChar.ToString();
        m_CurrentChar = newChar;
        if ( comboCharColor.SelectedIndex != Project.Characters[m_CurrentChar].Color )
        {
          comboCharColor.SelectedIndex = Project.Characters[m_CurrentChar].Color;
        }
        if ( comboCharsetMode.SelectedIndex != (int)Project.Characters[m_CurrentChar].Mode )
        {
          comboCharsetMode.SelectedIndex = (int)Project.Characters[m_CurrentChar].Mode;
        }
        pictureEditor.Image = Project.Characters[m_CurrentChar].Image;
        pictureEditor.Invalidate();

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
      GR.Memory.ByteBuffer Buffer = new GR.Memory.ByteBuffer( Project.Characters[CharIndex].Data );

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
              if ( colorIndex == Project.BackgroundColor )
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
            && ( i != Project.BackgroundColor ) )
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
              if ( i != Project.BackgroundColor )
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

              if ( ColorIndex != Project.BackgroundColor )
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
              if ( ( i == Project.MultiColor1 )
              || ( i == Project.MultiColor2 )
              || ( i == Project.BackgroundColor ) )
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

              if ( ColorIndex == Project.BackgroundColor )
              {
                BitPattern = 0x00;
              }
              else if ( ColorIndex == Project.MultiColor1 )
              {
                BitPattern = 0x01;
              }
              else if ( ColorIndex == Project.MultiColor2 )
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
        Project.Characters[CharIndex].Data.SetU8At( i, Buffer.ByteAt( i ) );
      }
      if ( chosenCharColor == -1 )
      {
        chosenCharColor = 0;
      }
      Project.Characters[CharIndex].Color = chosenCharColor;
      if ( ( isMultiColor )
      && ( chosenCharColor < 8 ) )
      {
        Project.Characters[CharIndex].Color = chosenCharColor + 8;
      }
      Project.Characters[CharIndex].Mode = isMultiColor ? Types.CharsetMode.MULTICOLOR : C64Studio.Types.CharsetMode.HIRES;
      RebuildCharImage( CharIndex );
      return true;
    }



    private void pictureEditor_MouseDown( object sender, MouseEventArgs e )
    {
      pictureEditor.Focus();
      HandleMouseOnEditor( e.X, e.Y, e.Button );
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



    private void HandleMouseOnEditor( int X, int Y, MouseButtons Buttons )
    {
      int     charX = X / ( pictureEditor.ClientRectangle.Width / 8 );
      int     charY = Y / ( pictureEditor.ClientRectangle.Height / 8 );
      int     affectedCharIndex = m_CurrentChar;
      var     origAffectedChar = Project.Characters[m_CurrentChar];
      var     affectedChar = Project.Characters[m_CurrentChar];
      if ( affectedChar.Mode == C64Studio.Types.CharsetMode.ECM )
      {
        affectedCharIndex %= 64;
        affectedChar = Project.Characters[affectedCharIndex];
      }

      if ( ( Buttons & MouseButtons.Left ) != 0 )
      {
        byte    charByte = affectedChar.Data.ByteAt( charY );
        byte    newByte = charByte;
        int     colorIndex = affectedChar.Color;

        if ( ( affectedChar.Mode != C64Studio.Types.CharsetMode.MULTICOLOR )
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
            colorIndex = Project.BackgroundColor;
          }
        }
        else
        {
          // multi color
          charX = X / ( pictureEditor.ClientRectangle.Width / 4 );
          charX = 3 - charX;

          newByte &= (byte)~( 3 << ( 2 * charX ) );

          int     replacementBytes = 0;

          switch ( m_CurrentColorType )
          {
            case ColorType.BACKGROUND:
              colorIndex = Project.BackgroundColor;
              break;
            case ColorType.CHAR_COLOR:
              replacementBytes = 3;
              colorIndex = affectedChar.Color;
              break;
            case ColorType.MULTICOLOR_1:
              replacementBytes = 1;
              colorIndex = Project.MultiColor1;
              break;
            case ColorType.MULTICOLOR_2:
              replacementBytes = 2;
              colorIndex = Project.MultiColor2;
              break;
          }
          newByte |= (byte)( replacementBytes << ( 2 * charX ) );
        }
        if ( newByte != charByte )
        {
          Modified = true;

          if ( m_ButtonReleased )
          {
            // TODO!
            //DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharsetCharChange( this, Project, affectedCharIndex ) );
            m_ButtonReleased = false;
          }

          affectedChar.Data.SetU8At( charY, newByte );

          if ( origAffectedChar.Mode == C64Studio.Types.CharsetMode.ECM )
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
          pictureEditor.Invalidate();
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

        if ( ( affectedChar.Mode != C64Studio.Types.CharsetMode.MULTICOLOR )
        ||   ( affectedChar.Color < 8 ) )
        {
          // single color
          charX = 7 - charX;
          newByte &= (byte)~( 1 << charX );
        }
        else
        {
          // multi color
          charX = X / ( pictureEditor.ClientRectangle.Width / 4 );
          charX = 3 - charX;

          newByte &= (byte)~( 3 << ( 2 * charX ) );
        }
        if ( newByte != charByte )
        {
          if ( m_RButtonReleased )
          {
            m_RButtonReleased = false;
            // TODO
            //DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharsetCharChange( this, Project, affectedCharIndex ) );
          }

          Modified = true;
          affectedChar.Data.SetU8At( charY, newByte );

          if ( origAffectedChar.Mode == C64Studio.Types.CharsetMode.ECM )
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
          pictureEditor.Invalidate();
        }
      }
      else
      {
        m_RButtonReleased = true;
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
      m_CurrentColorType = ColorType.CHAR_COLOR;
    }



    private void checkShowGrid_CheckedChanged( object sender, EventArgs e )
    {
      Project.ShowGrid = checkShowGrid.Checked;
      pictureEditor.Invalidate();
    }



    private void btnInvert_Click( object sender, EventArgs e )
    {
      Invert();
    }



    private void Invert()
    {
      List<int>     selectedChars = panelCharacters.SelectedIndices;

      // TODO
      //DocumentInfo.UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        // TODO
        //DocumentInfo.UndoManager.AddGroupedUndoTask( new Undo.UndoCharsetCharChange( this, Project, index ) );

        for ( int y = 0; y < 8; ++y )
        {
          byte result = (byte)( ~Project.Characters[index].Data.ByteAt( y ) );
          Project.Characters[index].Data.SetU8At( y, result );
        }
        RebuildCharImage( index );
        panelCharacters.InvalidateItemRect( index );
      }
      pictureEditor.Invalidate();
      Modified = true;
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
      int     charX = X / ( pictureEditor.ClientRectangle.Width / 16 );
      int     charY = Y / ( pictureEditor.ClientRectangle.Height / 16 );

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
        if ( Project.PlaygroundChars[charX + charY * 16] != (ushort)( m_CurrentChar | ( m_CurrentColor << 8 ) ) )
        {
          // TODO
          //DocumentInfo.UndoManager.AddUndoTask( new Undo.UndoCharsetPlaygroundCharChange( this, Project, charX, charY ) );

          Displayer.CharacterDisplayer.DisplayChar( Project, m_CurrentChar, m_ImagePlayground, charX * 8, charY * 8, m_CurrentColor );
          RedrawPlayground();

          Project.PlaygroundChars[charX + charY * 16] = (ushort)( m_CurrentChar | ( m_CurrentColor << 8 ) );
          Modified = true;
        }
      }
      if ( ( Buttons & MouseButtons.Right ) != 0 )
      {
        m_CurrentChar = (byte)( Project.PlaygroundChars[charX + charY * 16] & 0x00ff );
        m_CurrentColor = (byte)( Project.PlaygroundChars[charX + charY * 16] >> 8 );
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
        int colorIndex = X / 16;
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



    void MirrorX()
    {
      List<int>     selectedChars = panelCharacters.SelectedIndices;

      // TODO
      // DocumentInfo.UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        // TODO
        // DocumentInfo.UndoManager.AddGroupedUndoTask( new Undo.UndoCharsetCharChange( this, Project, index ) );

        var processedChar = Project.Characters[index];

        for ( int y = 0; y < 8; ++y )
        {
          if ( ( processedChar.Mode == C64Studio.Types.CharsetMode.MULTICOLOR )
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
      pictureEditor.Invalidate();
      Modified = true;
    }



    void MirrorY()
    {
      List<int>     selectedChars = panelCharacters.SelectedIndices;

      // TODO
      //DocumentInfo.UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        // TODO
        //DocumentInfo.UndoManager.AddGroupedUndoTask( new Undo.UndoCharsetCharChange( this, Project, index ) );

        for ( int y = 0; y < 4; ++y )
        {
          byte oldValue = Project.Characters[index].Data.ByteAt( y );
          Project.Characters[index].Data.SetU8At( y, Project.Characters[index].Data.ByteAt( 7 - y ) );
          Project.Characters[index].Data.SetU8At( 7 - y, oldValue );
        }
        RebuildCharImage( index );
        panelCharacters.InvalidateItemRect( index );
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
      List<int>     selectedChars = panelCharacters.SelectedIndices;

      // TODO
      // DocumentInfo.UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        // TODO
        // DocumentInfo.UndoManager.AddGroupedUndoTask( new Undo.UndoCharsetCharChange( this, Project, index ) );

        GR.Memory.ByteBuffer resultData = new GR.Memory.ByteBuffer( 8 );

        if ( ( Project.Characters[index].Mode == C64Studio.Types.CharsetMode.MULTICOLOR )
        ||   ( Project.Characters[index].Color >= 8 ) )
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
              byte sourceColor = (byte)( ( Project.Characters[index].Data.ByteAt( sourceY ) & ( 3 << maskOffset ) ) >> maskOffset );

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
              if ( ( Project.Characters[index].Data.ByteAt( sourceY ) & ( 1 << ( 7 - ( sourceX % 8 ) ) ) ) != 0 )
              {
                resultData.SetU8At( targetY, (byte)( resultData.ByteAt( targetY ) | ( 1 << ( 7 - targetX % 8 ) ) ) );
              }
            }
          }
        }
        Project.Characters[index].Data = resultData;
        RebuildCharImage( index );
        panelCharacters.InvalidateItemRect( index );
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
      List<int>     selectedChars = panelCharacters.SelectedIndices;

      // TODO
      // DocumentInfo.UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        // TODO
        // DocumentInfo.UndoManager.AddGroupedUndoTask( new Undo.UndoCharsetCharChange( this, Project, index ) );

        GR.Memory.ByteBuffer resultData = new GR.Memory.ByteBuffer( 8 );

        if ( ( Project.Characters[index].Mode == C64Studio.Types.CharsetMode.MULTICOLOR )
        ||   ( Project.Characters[index].Color >= 8 ) )
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
              byte sourceColor = (byte)( ( Project.Characters[index].Data.ByteAt( sourceY ) & ( 3 << maskOffset ) ) >> maskOffset );

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
              if ( ( Project.Characters[index].Data.ByteAt( sourceY ) & ( 1 << ( 7 - ( sourceX % 8 ) ) ) ) != 0 )
              {
                resultData.SetU8At( targetY, (byte)( resultData.ByteAt( targetY ) | ( 1 << ( 7 - targetX % 8 ) ) ) );
              }
            }
          }
        }
        Project.Characters[index].Data = resultData;
        RebuildCharImage( index );
        panelCharacters.InvalidateItemRect( index );
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
      List<int>     selectedChars = panelCharacters.SelectedIndices;

      // TODO
      // DocumentInfo.UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        // TODO
        // DocumentInfo.UndoManager.AddGroupedUndoTask( new Undo.UndoCharsetCharChange( this, Project, index ) );

        byte  temp = Project.Characters[index].Data.ByteAt( 7 );
        for ( int y = 0; y < 7; ++y )
        {
          Project.Characters[index].Data.SetU8At( 7 - y, Project.Characters[index].Data.ByteAt( 7 - y - 1 ) );
        }
        Project.Characters[index].Data.SetU8At( 0, temp );
        RebuildCharImage( index );
        panelCharacters.InvalidateItemRect( index );
      }
      pictureEditor.Invalidate();
      Modified = true;
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



    private void ShiftUp()
    {
      List<int>     selectedChars = panelCharacters.SelectedIndices;

      // TODO
      // DocumentInfo.UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        // TODO
        // DocumentInfo.UndoManager.AddGroupedUndoTask( new Undo.UndoCharsetCharChange( this, Project, index ) );

        byte  temp = Project.Characters[index].Data.ByteAt( 0 );
        for ( int y = 0; y < 7; ++y )
        {
          Project.Characters[index].Data.SetU8At( y, Project.Characters[index].Data.ByteAt( y + 1 ) );
        }
        Project.Characters[index].Data.SetU8At( 7, temp );
        RebuildCharImage( index );
        panelCharacters.InvalidateItemRect( index );
      }
      pictureEditor.Invalidate();
      Modified = true;
    }


    void ShiftLeft()
    {
      List<int>     selectedChars = panelCharacters.SelectedIndices;

      // TODO
      // DocumentInfo.UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        // TODO
        // DocumentInfo.UndoManager.AddGroupedUndoTask( new Undo.UndoCharsetCharChange( this, Project, index ) );

        for ( int y = 0; y < 8; ++y )
        {
          if ( ( Project.Characters[index].Mode == C64Studio.Types.CharsetMode.MULTICOLOR )
          && ( Project.Characters[index].Color >= 8 ) )
          {
            byte result = (byte)( (byte)( ( Project.Characters[m_CurrentChar].Data.ByteAt( y ) & 0xc0 ) >> 6 )
                                | (byte)( ( Project.Characters[index].Data.ByteAt( y ) & 0x3f ) << 2 ) );
            Project.Characters[index].Data.SetU8At( y, result );
          }
          else
          {
            byte result = (byte)( (byte)( ( Project.Characters[index].Data.ByteAt( y ) & 0x80 ) >> 7 )
                                | (byte)( ( Project.Characters[index].Data.ByteAt( y ) & 0x7f ) << 1 ) );
            Project.Characters[index].Data.SetU8At( y, result );
          }
        }
        RebuildCharImage( index );
        panelCharacters.InvalidateItemRect( index );
      }
      pictureEditor.Invalidate();
      Modified = true;
    }



    void ShiftRight()
    {
      List<int>     selectedChars = panelCharacters.SelectedIndices;

      // TODO
      // DocumentInfo.UndoManager.StartUndoGroup();
      foreach ( var index in selectedChars )
      {
        // TODO
        // DocumentInfo.UndoManager.AddGroupedUndoTask( new Undo.UndoCharsetCharChange( this, Project, index ) );

        for ( int y = 0; y < 8; ++y )
        {
          if ( ( Project.Characters[index].Mode == C64Studio.Types.CharsetMode.MULTICOLOR )
          && ( Project.Characters[index].Color >= 8 ) )
          {
            byte result = (byte)( (byte)( ( Project.Characters[index].Data.ByteAt( y ) & 0xfc ) >> 2 )
                                | (byte)( ( Project.Characters[index].Data.ByteAt( y ) & 0x03 ) << 6 ) );
            Project.Characters[index].Data.SetU8At( y, result );
          }
          else
          {
            byte result = (byte)( (byte)( ( Project.Characters[index].Data.ByteAt( y ) & 0x01 ) << 7 )
                                | (byte)( ( Project.Characters[index].Data.ByteAt( y ) & 0xfe ) >> 1 ) );
            Project.Characters[index].Data.SetU8At( y, result );
          }
        }
        RebuildCharImage( index );
        panelCharacters.InvalidateItemRect( index );
      }
      pictureEditor.Invalidate();
      Modified = true;
    }



    private void comboColor_DrawItem( object sender, DrawItemEventArgs e )
    {
      ComboBox combo = (ComboBox)sender;

      e.DrawBackground();
      if ( e.Index == -1 )
      {
        return;
      }

      System.Drawing.Rectangle itemRect = new System.Drawing.Rectangle( e.Bounds.Left + 20, e.Bounds.Top, e.Bounds.Width - 20, e.Bounds.Height );
      if ( ( e.State & DrawItemState.Disabled ) != 0 )
      {
        e.Graphics.FillRectangle( System.Drawing.SystemBrushes.GrayText, itemRect );
        e.Graphics.DrawString( combo.Items[e.Index].ToString(), combo.Font, new System.Drawing.SolidBrush( System.Drawing.Color.Gray ), 3.0f, e.Bounds.Top + 1.0f );
      }
      else if ( ( e.State & DrawItemState.Selected ) != 0 )
      {
        e.Graphics.FillRectangle( Types.ConstantData.Palette.ColorBrushes[e.Index], itemRect );
        e.Graphics.DrawString( combo.Items[e.Index].ToString(), combo.Font, new System.Drawing.SolidBrush( System.Drawing.Color.White ), 3.0f, e.Bounds.Top + 1.0f );
      }
      else
      {
        e.Graphics.FillRectangle( Types.ConstantData.Palette.ColorBrushes[e.Index], itemRect );
        e.Graphics.DrawString( combo.Items[e.Index].ToString(), combo.Font, new System.Drawing.SolidBrush( System.Drawing.Color.Black ), 3.0f, e.Bounds.Top + 1.0f );
      }
    }



    private void comboMulticolor_DrawItem( object sender, DrawItemEventArgs e )
    {
      ComboBox combo = (ComboBox)sender;

      e.DrawBackground();
      System.Drawing.Rectangle itemRect = new System.Drawing.Rectangle( e.Bounds.Left + 20, e.Bounds.Top, e.Bounds.Width - 20, e.Bounds.Height );
      if ( e.Index >= 8 )
      {
        itemRect = new System.Drawing.Rectangle( e.Bounds.Left + 20, e.Bounds.Top, ( e.Bounds.Width - 20 ) / 2, e.Bounds.Height );
        e.Graphics.FillRectangle( Types.ConstantData.Palette.ColorBrushes[e.Index], itemRect );
        itemRect = new System.Drawing.Rectangle( e.Bounds.Left + 20 + ( e.Bounds.Width - 20 ) / 2, e.Bounds.Top, e.Bounds.Width - ( e.Bounds.Width - 20 ) / 2, e.Bounds.Height );
        e.Graphics.FillRectangle( Types.ConstantData.Palette.ColorBrushes[e.Index - 8], itemRect );
      }
      else
      {
        e.Graphics.FillRectangle( Types.ConstantData.Palette.ColorBrushes[e.Index], itemRect );
      }
      if ( ( e.State & DrawItemState.Selected ) != 0 )
      {
        e.Graphics.DrawString( combo.Items[e.Index].ToString(), combo.Font, new System.Drawing.SolidBrush( System.Drawing.Color.White ), 3.0f, e.Bounds.Top + 1.0f );
      }
      else
      {
        e.Graphics.DrawString( combo.Items[e.Index].ToString(), combo.Font, new System.Drawing.SolidBrush( System.Drawing.Color.Black ), 3.0f, e.Bounds.Top + 1.0f );
      }

    }






  }
}
