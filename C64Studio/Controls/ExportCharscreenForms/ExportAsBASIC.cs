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
  public partial class ExportAsBASIC : ExportCharscreenFormBase
  {
    public ExportAsBASIC() :
      base( null )
    { 
    }



    public ExportAsBASIC( StudioCore Core ) :
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

      sb.Append( startLine );
      sb.Append( " PRINT\"" + ConstantData.PetSCIIToChar[147].CharValue + "\";\n" );
      startLine += lineStep;

      sb.Append( startLine );
      startLine += lineStep;
      sb.Append( " POKE53280," + Info.Charscreen.CharSet.Colors.BackgroundColor.ToString() + ":POKE53281," + Info.Charscreen.CharSet.Colors.BackgroundColor.ToString() + "\n" );

      for ( int i = Info.Area.Top; i < Info.Area.Bottom; ++i )
      {
        int   startLength = sb.Length;
        sb.Append( startLine );
        startLine += lineStep;
        sb.Append( " PRINT\"" );
        for ( int x = Info.Area.Left; x < Info.Area.Right; ++x )
        {
          ushort newColor = (ushort)( Info.Charscreen.ColorAt( x, i ) & 0x0f );
          ushort newChar = Info.Charscreen.CharacterAt( x, i );

          List<char>  charsToAppend = new List<char>();

          if ( newColor != curColor )
          {
            charsToAppend.Add( ConstantData.PetSCIIToChar[ConstantData.ColorToPetSCIIChar[(byte)newColor]].CharValue );
            curColor = newColor;
          }
          if ( newChar >= 128 )
          {
            if ( !isReverse )
            {
              isReverse = true;
              charsToAppend.Add( ConstantData.PetSCIIToChar[18].CharValue );
            }
          }
          else if ( isReverse )
          {
            isReverse = false;
            charsToAppend.Add( ConstantData.PetSCIIToChar[146].CharValue );
          }
          if ( isReverse )
          {
            if ( newChar == 128 + 34 )
            {
              // reverse apostrophe
              string    replacement = "\"CHR$(34)CHR$(20)CHR$(34)\"";

              for ( int t = 0; t < replacement.Length; ++t )
              {
                charsToAppend.Add( ConstantData.CharToC64Char[replacement[t]].CharValue );
              }
            }
            else
            {
              charsToAppend.Add( ConstantData.ScreenCodeToChar[(byte)( newChar - 128 )].CharValue );
            }
          }
          else
          {
            if ( newChar == 34 )
            {
              // a regular apostrophe
              string    replacement = "\"CHR$(34)CHR$(20)CHR$(34)\"";

              for ( int t = 0; t < replacement.Length; ++t )
              {
                charsToAppend.Add( ConstantData.CharToC64Char[replacement[t]].CharValue );
              }
            }
            else
            {
              charsToAppend.Add( ConstantData.ScreenCodeToChar[(byte)newChar].CharValue );
            }
          }

          // don't make lines too long!
          if ( sb.Length - startLength + charsToAppend.Count >= wrapByteCount - 1 )
          {
            // we need to break and start a new line
            sb.Append( "\";\n" );
            startLength = sb.Length;
            sb.Append( startLine );
            startLine += lineStep;
            sb.Append( " PRINT\"" );
          }
          foreach ( char toAppend in charsToAppend )
          {
            sb.Append( toAppend );
          }
        }
        sb.Append( "\";\n" );
      }

      Types.ComboItem comboItem = (Types.ComboItem)comboBasicFiles.SelectedItem;
      if ( comboItem.Tag == null )
      {
        if ( comboItem.Desc == "To output" )
        {
          EditOutput.Text = sb.ToString();
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
