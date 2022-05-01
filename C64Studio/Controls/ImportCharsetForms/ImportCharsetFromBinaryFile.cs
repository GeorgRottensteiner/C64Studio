using C64Studio.Formats;
using C64Studio.Types;
using GR.Memory;
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
  public partial class ImportCharsetFromBinaryFile : ImportCharsetFormBase
  {
    public ImportCharsetFromBinaryFile() :
      base( null )
    { 
    }



    public ImportCharsetFromBinaryFile( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleImport( CharsetProject Charset, CharsetEditor Editor )
    {
      string filename;

      //Clear();
      if ( Editor.OpenFile( "Open charset", C64Studio.Types.Constants.FILEFILTER_CHARSET + C64Studio.Types.Constants.FILEFILTER_CHARSET_CHARPAD + C64Studio.Types.Constants.FILEFILTER_ALL, out filename ) )
      {
        if ( System.IO.Path.GetExtension( filename ).ToUpper() == ".CHARSETPROJECT" )
        {
          // a project
          GR.Memory.ByteBuffer projectFile = GR.IO.File.ReadAllBytes( filename );

          C64Studio.Formats.CharsetProject project = new C64Studio.Formats.CharsetProject();
          if ( !project.ReadFromBuffer( projectFile ) )
          {
            return false;
          }
          Charset.Colors = new ColorSettings( project.Colors );
          Charset.ExportNumCharacters = project.ExportNumCharacters;
          Charset.ShowGrid = project.ShowGrid;

          for ( int i = 0; i < Charset.TotalNumberOfCharacters; ++i )
          {
            Charset.Characters[i].Tile.CustomColor = project.Characters[i].Tile.CustomColor;
            Charset.Characters[i].Tile.Data = new GR.Memory.ByteBuffer( project.Characters[i].Tile.Data );
          }

          Editor.CharsetWasImported();

          Editor.SetModified();
          return true;
        }
        else if ( System.IO.Path.GetExtension( filename ).ToUpper() == ".CTM" )
        {
          // a charpad project file
          GR.Memory.ByteBuffer projectFile = GR.IO.File.ReadAllBytes( filename );

          Formats.CharpadProject    cpProject = new C64Studio.Formats.CharpadProject();
          if ( !cpProject.LoadFromFile( projectFile ) )
          {
            return false;
          }

          Charset.Colors.BackgroundColor = cpProject.BackgroundColor;
          Charset.Colors.MultiColor1 = cpProject.MultiColor1;
          Charset.Colors.MultiColor2 = cpProject.MultiColor2;

          Charset.ExportNumCharacters = cpProject.NumChars;
          if ( Charset.ExportNumCharacters > 256 )
          {
            Charset.ExportNumCharacters = 256;
          }
          for ( int charIndex = 0; charIndex < Charset.ExportNumCharacters; ++charIndex )
          {
            Charset.Characters[charIndex].Tile.Data = cpProject.Characters[charIndex].Data;
            Charset.Characters[charIndex].Tile.CustomColor = cpProject.Characters[charIndex].Color;
          }

          Editor.CharsetWasImported();
          Editor.SetModified();
          return true;
        }

        // treat as binary .chr file
        GR.Memory.ByteBuffer charData = GR.IO.File.ReadAllBytes( filename );

        Editor.ImportFromData( charData );
      }
      return true;
    }



  }
}
