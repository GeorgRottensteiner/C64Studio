using RetroDevStudio.Types;
using RetroDevStudio.Formats;
using System.Windows.Forms;
using RetroDevStudio.Documents;
using static RetroDevStudio.Documents.BaseDocument;

namespace RetroDevStudio.Controls
{
  public partial class ExportMapCharsetAsCharset : ExportMapFormBase
  {
    public ExportMapCharsetAsCharset() :
      base( null )
    { 
    }



    public ExportMapCharsetAsCharset( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();

      comboCharsetFiles.Items.Add( new Types.ComboItem( "To new charset project" ) );
      UtilForms.FillComboWithFilesOfType( Core, comboCharsetFiles, ProjectElement.ElementType.CHARACTER_SET );
      
      comboCharsetFiles.SelectedIndex = 0;

      Core.MainForm.ApplicationEvent += new MainForm.ApplicationEventHandler( MainForm_ApplicationEvent );
    }



    private void MainForm_ApplicationEvent( ApplicationEvent Event )
    {
      UtilForms.AdaptComboWithFilesOfType( Core, Event, comboCharsetFiles, ProjectElement.ElementType.CHARACTER_SET );
    }



    public override bool HandleExport( ExportMapInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
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
          string    newFilename = "New Character Set.charsetproject";
          if ( DocInfo.Project != null )
          {
            string    newFilenameTemplate = DocInfo.Project.FullPath( newFilename );

            int     curAttempt = 2;
            while ( ( DocInfo.Project != null )
            &&      ( DocInfo.Project.IsFilenameInUse( newFilename ) ) )
            {
              newFilename = GR.Path.RenameFilenameWithoutExtension( newFilenameTemplate, GR.Path.GetFileNameWithoutExtension( newFilename ) + " " + curAttempt );
              ++curAttempt;
            }
          }
          document.SetDocumentFilename( newFilename );
          document.DocumentInfo.Element.Filename = document.DocumentInfo.DocumentFilename;
        }
        ( (CharsetEditor)document ).OpenProject( Info.Map.Charset.SaveToBuffer() );
        document.SetModified();
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
          document.OpenProject( Info.Map.Charset.SaveToBuffer() );
          document.SetModified();
        }
      }

      return true;
    }



  }
}
