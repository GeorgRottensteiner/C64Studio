﻿using RetroDevStudio.Formats;
using System.Windows.Forms;
using RetroDevStudio.Documents;



namespace RetroDevStudio.Controls
{
  public partial class ImportCharscreenCharsetFromASM : ImportCharscreenFormBase
  {
    public ImportCharscreenCharsetFromASM() :
      base( null )
    { 
    }



    public ImportCharscreenCharsetFromASM( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleImport( CharsetScreenProject CharScreen, CharsetScreenEditor Editor )
    {
      Parser.ASMFileParser asmParser = new RetroDevStudio.Parser.ASMFileParser();

      Parser.CompileConfig config = new Parser.CompileConfig();
      config.TargetType = Types.CompileTargetType.PLAIN;
      config.OutputFile = "temp.bin";
      config.Assembler = Types.AssemblerType.C64_STUDIO;

      string    temp = "* = $0801\n" + editInput.Text;
      if ( ( asmParser.Parse( temp, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) )
      &&   ( asmParser.Assemble( config ) ) )
      {
        GR.Memory.ByteBuffer charData = asmParser.AssembledOutput.Assembly;

        int charsToImport = (int)charData.Length / 8;
        if ( charsToImport > CharScreen.CharSet.TotalNumberOfCharacters )
        {
          charsToImport = CharScreen.CharSet.TotalNumberOfCharacters;
        }

        for ( int i = 0; i < charsToImport; ++i )
        {
          charData.CopyTo( CharScreen.CharSet.Characters[i].Tile.Data, i * 8, 8 );
          Editor.RebuildCharImage( i );
        }

        return true;
      }
      return false;
    }



    private void editInput_KeyPress( object sender, KeyPressEventArgs e )
    {
      if ( ( ModifierKeys == Keys.Control )
      &&   ( e.KeyChar == 1 ) )
      {
        editInput.SelectAll();
        e.Handled = true;
      }
    }



  }
}
