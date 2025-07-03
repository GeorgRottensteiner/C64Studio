using RetroDevStudio.Formats;
using RetroDevStudio.Types;
using RetroDevStudio.Documents;
using System;



namespace RetroDevStudio.Controls
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



    public override bool HandleImport( ImportCharsetInfo importInfo, CharsetEditor Editor )
    {
      string filename;

      int bytesToSkip = GR.Convert.ToI32( editImportSkipBytes.Text );
      if ( bytesToSkip < 0 )
      {
        bytesToSkip = 0;
      }

      if ( Editor.OpenFile( "Open charset", RetroDevStudio.Types.Constants.FILEFILTER_CHARSET + RetroDevStudio.Types.Constants.FILEFILTER_CHARSET_CHARPAD + RetroDevStudio.Types.Constants.FILEFILTER_ALL, out filename ) )
      {
        if ( GR.Path.GetExtension( filename ).ToUpper() == ".CHARSETPROJECT" )
        {
          // a project
          GR.Memory.ByteBuffer projectFile = GR.IO.File.ReadAllBytes( filename );

          RetroDevStudio.Formats.CharsetProject project = new RetroDevStudio.Formats.CharsetProject();
          if ( !project.ReadFromBuffer( projectFile ) )
          {
            return false;
          }
          importInfo.Charset.Colors = new ColorSettings( project.Colors );
          importInfo.Charset.ExportNumCharacters = project.ExportNumCharacters;
          importInfo.Charset.ShowGrid = project.ShowGrid;

          int numChars = Math.Min( importInfo.ImportIndices.Count, project.Characters.Count );
          int charIndex = 0;
          foreach ( var i in importInfo.ImportIndices )
          {
            if ( charIndex >= numChars )
            {
              break;
            }
            importInfo.Charset.Characters[i].Tile.CustomColor = project.Characters[charIndex].Tile.CustomColor;
            importInfo.Charset.Characters[i].Tile.Data = new GR.Memory.ByteBuffer( project.Characters[charIndex].Tile.Data );
            ++charIndex;
          }

          Editor.CharsetWasImported();

          Editor.SetModified();
          return true;
        }
        else if ( GR.Path.GetExtension( filename ).ToUpper() == ".CTM" )
        {
          // a charpad project file
          GR.Memory.ByteBuffer projectFile = GR.IO.File.ReadAllBytes( filename );

          Formats.CharpadProject    cpProject = new RetroDevStudio.Formats.CharpadProject();
          if ( !cpProject.LoadFromFile( projectFile ) )
          {
            return false;
          }

          importInfo.Charset.Colors.BackgroundColor = cpProject.BackgroundColor;
          importInfo.Charset.Colors.MultiColor1 = cpProject.MultiColor1;
          importInfo.Charset.Colors.MultiColor2 = cpProject.MultiColor2;

          importInfo.Charset.ExportNumCharacters = cpProject.NumChars;
          if ( importInfo.Charset.ExportNumCharacters > 256 )
          {
            importInfo.Charset.ExportNumCharacters = 256;
          }
          int charIndex = 0;
          int numChars = Math.Min( importInfo.ImportIndices.Count, cpProject.NumChars );
          foreach ( var i in importInfo.ImportIndices )
          {
            if ( charIndex >= numChars )
            {
              break;
            }
            importInfo.Charset.Characters[i].Tile.Data = cpProject.Characters[charIndex].Data;
            importInfo.Charset.Characters[i].Tile.CustomColor = cpProject.Characters[charIndex].Color;
            ++charIndex;
          }

          Editor.CharsetWasImported();
          Editor.SetModified();
          return true;
        }

        // treat as binary .chr file
        GR.Memory.ByteBuffer charData = GR.IO.File.ReadAllBytes( filename );

        if ( checkAutoProcessFileTypes.Checked )
        {
          AutoHandleDataByExtension( GR.Path.GetExtension( filename ).ToUpper(), ref bytesToSkip );
        }

        if ( ( bytesToSkip > 0 )
        &&   ( bytesToSkip < charData.Length ) )
        {
          charData = charData.SubBuffer( bytesToSkip );
        }

        Editor.ImportFromData( charData, importInfo.ImportIndices );
      }
      return true;
    }



    private void AutoHandleDataByExtension( string extension, ref int bytesToSkip )
    {
      switch ( extension )
      {
        case ".PRG":
          bytesToSkip += 2;
          break;
      }
    }



  }
}
