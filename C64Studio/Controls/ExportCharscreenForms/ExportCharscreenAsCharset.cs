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
  public partial class ExportCharscreenAsCharset : ExportCharscreenFormBase
  {
    public ExportCharscreenAsCharset() :
      base( null )
    { 
    }



    public ExportCharscreenAsCharset( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();

      comboCharsetFiles.Items.Add( new Types.ComboItem( "To new charset project" ) );
      foreach ( BaseDocument doc in Core.MainForm.panelMain.Documents )
      {
        if ( doc.DocumentInfo.Type == ProjectElement.ElementType.CHARACTER_SET )
        {
          string    nameToUse = doc.DocumentFilename ?? "New File";
          comboCharsetFiles.Items.Add( new Types.ComboItem( nameToUse, doc.DocumentInfo ) );
        }
      }
      comboCharsetFiles.SelectedIndex = 0;

      Core.MainForm.ApplicationEvent += new MainForm.ApplicationEventHandler( MainForm_ApplicationEvent );
    }



    private void MainForm_ApplicationEvent( ApplicationEvent Event )
    {
      if ( Event.EventType == ApplicationEvent.Type.ELEMENT_CREATED )
      {
        if ( Event.Doc.Type == ProjectElement.ElementType.CHARACTER_SET )
        {
          foreach ( ComboItem item in comboCharsetFiles.Items )
          {
            if ( (DocumentInfo)item.Tag == Event.Doc )
            {
              return;
            }
          }

          string    nameToUse = Event.Doc.DocumentFilename ?? "New File";
          comboCharsetFiles.Items.Add( new Types.ComboItem( nameToUse, Event.Doc ) );
        }
      }
      if ( Event.EventType == ApplicationEvent.Type.ELEMENT_REMOVED )
      {
        if ( Event.Doc.Type == ProjectElement.ElementType.CHARACTER_SET )
        {
          foreach ( Types.ComboItem comboItem in comboCharsetFiles.Items )
          {
            if ( (DocumentInfo)comboItem.Tag == Event.Doc )
            {
              comboCharsetFiles.Items.Remove( comboItem );
              if ( comboCharsetFiles.SelectedIndex == -1 )
              {
                comboCharsetFiles.SelectedIndex = 0;
              }
              break;
            }
          }
        }
      }
    }



    public override bool HandleExport( ExportCharsetScreenInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
    {
      Types.ComboItem comboItem = (Types.ComboItem)comboCharsetFiles.SelectedItem;
      if ( comboItem.Tag == null )
      {
        // to new file
        BaseDocument document = null;
        if ( DocInfo.Project == null )
        {
          document = Core.MainForm.CreateNewDocument( ProjectElement.ElementType.CHARACTER_SET, null );
        }
        else
        {
          document = Core.MainForm.CreateNewElement( ProjectElement.ElementType.CHARACTER_SET, "Character Set", DocInfo.Project ).Document;
        }
        if ( document.DocumentInfo.Element != null )
        {
          document.SetDocumentFilename( "New Character Set.charsetproject" );
          document.DocumentInfo.Element.Filename = document.DocumentInfo.DocumentFilename;
        }
        ( (CharsetEditor)document ).OpenProject( Info.CharsetData );
        document.SetModified();
        document.Save( SaveMethod.SAVE );
      }
      else
      {
        DocumentInfo    docInfo = (DocumentInfo)comboItem.Tag;
        CharsetEditor document = (CharsetEditor)docInfo.BaseDoc;
        if ( document == null )
        {
          if ( docInfo.Project != null )
          {
            docInfo.Project.ShowDocument( docInfo.Element );
            document = (CharsetEditor)docInfo.BaseDoc;
          }
        }
        if ( document != null )
        {
          document.OpenProject( Info.CharsetData );
          document.SetModified();
        }
      }

      return true;
    }



  }
}
