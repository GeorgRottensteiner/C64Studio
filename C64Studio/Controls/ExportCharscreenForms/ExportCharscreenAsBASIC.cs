using C64Studio.Types;
using RetroDevStudio;
using RetroDevStudio.Formats;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static C64Studio.BaseDocument;

namespace C64Studio.Controls
{
  public partial class ExportCharsetAsBASIC : ExportCharscreenFormBase
  {
    public ExportCharsetAsBASIC() :
      base( null )
    { 
    }



    public ExportCharsetAsBASIC( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();

      comboBasicFiles.Items.Add( new Types.ComboItem( "To new file" ) );
      comboBasicFiles.Items.Add( new Types.ComboItem( "To output" ) );

      foreach ( BaseDocument doc in Core.MainForm.panelMain.Documents )
      {
        if ( doc.DocumentInfo.Type == ProjectElement.ElementType.BASIC_SOURCE )
        {
          string    nameToUse = doc.DocumentFilename ?? "New File";
          comboBasicFiles.Items.Add( new Types.ComboItem( nameToUse, doc.DocumentInfo ) );
        }
      }
      comboBasicFiles.SelectedIndex = 0;

      Core.MainForm.ApplicationEvent += new MainForm.ApplicationEventHandler( MainForm_ApplicationEvent );
    }



    private void MainForm_ApplicationEvent( ApplicationEvent Event )
    {
      if ( Event.EventType == ApplicationEvent.Type.ELEMENT_CREATED )
      {
        if ( Event.Doc.Type == ProjectElement.ElementType.BASIC_SOURCE )
        {
          foreach ( ComboItem item in comboBasicFiles.Items )
          {
            if ( (DocumentInfo)item.Tag == Event.Doc )
            {
              return;
            }
          }

          string    nameToUse = Event.Doc.DocumentFilename ?? "New File";
          comboBasicFiles.Items.Add( new Types.ComboItem( nameToUse, Event.Doc ) );
        }
      }
      if ( Event.EventType == ApplicationEvent.Type.ELEMENT_REMOVED )
      {
        if ( Event.Doc.Type == ProjectElement.ElementType.BASIC_SOURCE )
        {
          foreach ( Types.ComboItem comboItem in comboBasicFiles.Items )
          {
            if ( (DocumentInfo)comboItem.Tag == Event.Doc )
            {
              comboBasicFiles.Items.Remove( comboItem );
              if ( comboBasicFiles.SelectedIndex == -1 )
              {
                comboBasicFiles.SelectedIndex = 0;
              }
              break;
            }
          }
        }
      }
    }



    private void checkExportToDataWrap_CheckedChanged( object sender, EventArgs e )
    {
      editWrapByteCount.Enabled = checkExportToDataWrap.Checked;
    }



    private int GetExportWrapCount()
    {
      if ( checkExportToDataWrap.Checked )
      {
        return GR.Convert.ToI32( editWrapByteCount.Text );
      }
      return 80;
    }


    
    public override bool HandleExport( ExportCharsetScreenInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
    {
      var     sb = new StringBuilder();
      int     curColor = -1;
      bool    isReverse = false;
      bool    replaceSpaceWithCursorRight = checkExportToBASICReplaceSpaceWithRight.Checked;
      bool    replaceShiftSpaceWithSpace = checkExportToBASICReplaceShiftSpaceWithSpace.Checked;
      bool    stripInvisibleColors = checkExportToBASICCollapseColors.Checked;
      bool    asString = checkExportToBASICAsString.Checked;


      int startLine = GR.Convert.ToI32( editExportBASICLineNo.Text );
      if ( ( startLine < 0 )
      ||   ( startLine > 63999 ) )
      {
        startLine = 10;
      }
      int lineStep = GR.Convert.ToI32( editExportBASICLineOffset.Text );
      if ( ( lineStep < 0 )
      ||   ( lineStep > 63999 ) )
      {
        lineStep = 10;
      }

      int wrapByteCount = GetExportWrapCount();

      if ( !asString )
      {
        sb.Append( startLine );
        sb.Append( " PRINT\"" + ConstantData.PetSCIIToChar[147].CharValue + "\";\n" );
        startLine += lineStep;

        sb.Append( startLine );
        startLine += lineStep;
        sb.Append( " POKE53280," + Info.Charscreen.CharSet.Colors.BackgroundColor.ToString() + ":POKE53281," + Info.Charscreen.CharSet.Colors.BackgroundColor.ToString() + "\n" );
      }
      
      int     colorChangeCache = -1;

      if ( asString )
      {
        sb.Append( startLine );
        sb.Append( " B$=\"" );
        startLine += lineStep;
      }

      int   startLength = sb.Length;

      for ( int i = Info.Area.Top; i < Info.Area.Bottom; ++i )
      {
        if ( !asString )
        {
          startLength = sb.Length;
          sb.Append( startLine );
          startLine += lineStep;
          sb.Append( " PRINT\"" );
          if ( ( isReverse )
          &&   ( Info.Area.Width != Info.Charscreen.ScreenWidth ) )
          {
            // need to re-start reverse mode
            sb.Append( ConstantData.PetSCIIToChar[18].CharValue );
          }
        }
        
        for ( int x = Info.Area.Left; x < Info.Area.Right; ++x )
        {
          ushort newColor = (ushort)( Info.Charscreen.ColorAt( x, i ) & 0x0f );
          ushort newChar = Info.Charscreen.CharacterAt( x, i );

          if ( ( replaceShiftSpaceWithSpace )
          &&   ( ( newChar == 96 )
          ||     ( newChar == 96 + 128 ) ) )
          {
            newChar -= 64;
          }

          List<string>  charsToAppend = new List<string>();

          if ( newColor != curColor )
          {
            if ( stripInvisibleColors )
            {
              colorChangeCache = newColor;
            }
            else
            {
              charsToAppend.Add( "" + ConstantData.PetSCIIToChar[ConstantData.ColorToPetSCIIChar[(byte)newColor]].CharValue );
            }
            curColor = newColor;
          }
          // skip color changes for space and shift-space
          if ( ( newChar != 32 )
          &&   ( newChar != 96 )
          &&   ( stripInvisibleColors ) )
          {
            if ( colorChangeCache != -1 )
            {
              charsToAppend.Add( "" + ConstantData.PetSCIIToChar[ConstantData.ColorToPetSCIIChar[(byte)colorChangeCache]].CharValue );
              colorChangeCache = -1;
            }
          }
          if ( newChar >= 128 )
          {
            if ( !isReverse )
            {
              isReverse = true;
              charsToAppend.Add( "" + ConstantData.PetSCIIToChar[18].CharValue );
            }
          }
          else if ( isReverse )
          {
            isReverse = false;
            charsToAppend.Add( "" + ConstantData.PetSCIIToChar[146].CharValue );
          }
          if ( isReverse )
          {
            if ( newChar == 128 + 34 )
            {
              // reverse apostrophe
              string    replacement = "\"CHR$(34)CHR$(20)CHR$(34)\"";
              if ( asString )
              {
                replacement = "\"+CHR$(34)+CHR$(20)+CHR$(34)+\"";
              }

              string    replacementString = "";
              for ( int t = 0; t < replacement.Length; ++t )
              {
                replacementString += ConstantData.CharToC64Char[replacement[t]].CharValue;
              }
              charsToAppend.Add( replacementString );
            }
            else
            {
              charsToAppend.Add( "" + ConstantData.ScreenCodeToChar[(byte)( newChar - 128 )].CharValue );
            }
          }
          else
          {
            if ( newChar == 34 )
            {
              // a regular apostrophe
              string    replacement = "\"CHR$(34)CHR$(20)CHR$(34)\"";
              if ( asString )
              {
                replacement = "\"+CHR$(34)+CHR$(20)+CHR$(34)+\"";
              }

              string    replacementString = "";
              for ( int t = 0; t < replacement.Length; ++t )
              {
                replacementString += ConstantData.CharToC64Char[replacement[t]].CharValue;
              }
              charsToAppend.Add( replacementString );
            }
            else if ( ( replaceSpaceWithCursorRight )
            &&        ( newChar == 32 ) )
            {
              charsToAppend.Add( "" + ConstantData.PetSCIIToChar[29].CharValue );
            }
            else
            {
              charsToAppend.Add( "" + ConstantData.ScreenCodeToChar[(byte)newChar].CharValue );
            }
          }

          // don't make lines too long!
          foreach ( var stringToAppend in charsToAppend )
          {
            if ( sb.Length - startLength + stringToAppend.Length >= wrapByteCount - 1 )
            {
              // we need to break and start a new line
              if ( !asString )
              {
                sb.Append( "\"" );
                if ( Info.Area.Width == Info.Charscreen.ScreenWidth )
                {
                  sb.Append( ";" );
                }
                sb.Append( "\n" );
                startLength = sb.Length;
                sb.Append( startLine );
                startLine += lineStep;
                sb.Append( " PRINT\"" );
                if ( ( isReverse )
                &&   ( Info.Area.Width != Info.Charscreen.ScreenWidth ) )
                {
                  // need to re-start reverse mode
                  sb.Append( ConstantData.PetSCIIToChar[18].CharValue );
                }
              }
              else
              {
                if ( ( sb.Length >= 2 )
                &&   ( sb[sb.Length - 2] == '+' )
                &&   ( sb[sb.Length - 1] == '\"' ) )
                {
                  sb.Length -= 2;
                }
                else
                {
                  sb.Append( "\"" );
                }
                sb.Append( "\n" );
                startLength = sb.Length;
                sb.Append( startLine );
                startLine += lineStep;
                sb.Append( " B$=B$+\"" );
              }
            }
            foreach ( char toAppend in stringToAppend )
            {
              sb.Append( toAppend );
            }
          }
        }

        if ( !asString )
        {
          sb.Append( "\"" );
          if ( Info.Area.Width == Info.Charscreen.ScreenWidth )
          {
            sb.Append( ";" );
          }
          sb.Append( "\n" );
        }
        else
        {
          // down
          sb.Append( ConstantData.PetSCIIToChar[17].CharValue );
          // left
          for ( int x = 0; x < Info.Area.Width; ++x )
          {
            sb.Append( ConstantData.PetSCIIToChar[157].CharValue );
          }
        }
      }
      if ( asString )
      {
        if ( ( sb.Length >= 2 )
        &&   ( sb[sb.Length - 2] == '+' )
        &&   ( sb[sb.Length - 1] == '\"' ) )
        {
          sb.Length -= 2;
        }
        else
        {
          sb.Append( "\"" );
        }
        sb.Append( "\n" );
      }

      Types.ComboItem comboItem = (Types.ComboItem)comboBasicFiles.SelectedItem;
      if ( comboItem.Tag == null )
      {
        if ( comboItem.Desc == "To output" )
        {
          EditOutput.Font = new System.Drawing.Font( Core.MainForm.m_FontC64.Families[0], 16, System.Drawing.GraphicsUnit.Pixel );
          EditOutput.Text = sb.ToString().Replace( "\n", "\r\n" );
        }
        else
        {
          // to new file
          BaseDocument document = null;
          if ( DocInfo.Project == null )
          {
            document = Core.MainForm.CreateNewDocument( ProjectElement.ElementType.BASIC_SOURCE, null );
          }
          else
          {
            document = Core.MainForm.CreateNewElement( ProjectElement.ElementType.BASIC_SOURCE, "BASIC Screen", DocInfo.Project ).Document;
          }
          if ( document.DocumentInfo.Element != null )
          {
            document.SetDocumentFilename( "New BASIC File.bas" );
            document.DocumentInfo.Element.Filename = document.DocumentInfo.DocumentFilename;
          }
          document.FillContent( sb.ToString(), false );
          document.SetModified();
          document.Save( SaveMethod.SAVE );
        }
      }
      else
      {
        var document = (DocumentInfo)comboItem.Tag;
        if ( document.BaseDoc == null )
        {
          if ( document.Project == null )
          {
            return false;
          }
          document.Project.ShowDocument( document.Element );
        }
        document.BaseDoc.InsertText( sb.ToString() );
        document.BaseDoc.SetModified();
      }

      return true;
    }



  }
}
