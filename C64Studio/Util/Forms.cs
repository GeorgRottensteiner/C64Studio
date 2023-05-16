using RetroDevStudio.Documents;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RetroDevStudio
{
  public class UtilForms
  {
    public static void UpdateColorComboItemCount( ComboBox Combo, int ExpectedNumColors )
    {
      if ( Combo.Items.Count != ExpectedNumColors )
      {
        Combo.BeginUpdate();

        while ( Combo.Items.Count < ExpectedNumColors )
        {
          Combo.Items.Add( Combo.Items.Count.ToString( "d2" ) );
        }
        while ( Combo.Items.Count > ExpectedNumColors )
        {
          Combo.Items.RemoveAt( ExpectedNumColors );
        }
        Combo.EndUpdate();
      }
    }



    internal static void AdaptComboWithFilesOfType( StudioCore Core, ApplicationEvent Event, ComboBox ComboFiles, ProjectElement.ElementType RequiredType )
    {
      if ( Event.EventType == ApplicationEvent.Type.ELEMENT_CREATED )
      {
        if ( Event.Doc.Type == ProjectElement.ElementType.CHARACTER_SET )
        {
          foreach ( ComboItem item in ComboFiles.Items )
          {
            if ( (DocumentInfo)item.Tag == Event.Doc )
            {
              return;
            }
          }

          string    nameToUse = System.IO.Path.GetFileName( Event.Doc.DocumentFilename ) ?? "New File";
          ComboFiles.Items.Add( new Types.ComboItem( nameToUse, Event.Doc ) );
        }
      }
      if ( Event.EventType == ApplicationEvent.Type.ELEMENT_REMOVED )
      {
        if ( Event.Doc.Type == ProjectElement.ElementType.CHARACTER_SET )
        {
          foreach ( Types.ComboItem comboItem in ComboFiles.Items )
          {
            if ( (DocumentInfo)comboItem.Tag == Event.Doc )
            {
              ComboFiles.Items.Remove( comboItem );
              if ( ComboFiles.SelectedIndex == -1 )
              {
                ComboFiles.SelectedIndex = 0;
              }
              break;
            }
          }
        }
      }

    }



    internal static void FillComboWithFilesOfType( StudioCore Core, ComboBox ComboFiles, ProjectElement.ElementType RequiredType )
    {
      var items = new List<Types.ComboItem>();

      if ( Core.Navigating.Solution != null )
      {
        foreach ( var project in Core.Navigating.Solution.Projects )
        {
          var elements = project.Elements.Where( e => e.DocumentInfo.Type == RequiredType );
          foreach ( var element in elements )
          {
            string    nameToUse = element.DocumentInfo.DocumentFilename ?? "New File";

            items.Add( new Types.ComboItem( nameToUse, element.DocumentInfo ) );
          }
        }
      }
      // non project files
      foreach ( BaseDocument doc in Core.MainForm.panelMain.Documents )
      {
        if ( ( doc.DocumentInfo.Project == null )
        &&   ( doc.DocumentInfo.Type == RequiredType ) )
        {
          string    nameToUse = doc.DocumentFilename ?? "New File";

          items.Add( new Types.ComboItem( nameToUse, doc.DocumentInfo ) );
        }
      }
      foreach ( var item in items )
      {
        ComboFiles.Items.Add( new Types.ComboItem( item.Desc, (DocumentInfo)item.Tag ) );
      }
    }



  }
}
