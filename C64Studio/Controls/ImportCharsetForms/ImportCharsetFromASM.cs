using RetroDevStudio.Formats;
using System.Windows.Forms;
using RetroDevStudio.Documents;



namespace RetroDevStudio.Controls
{
  public partial class ImportCharsetFromASM : ImportCharsetFormBase
  {
    public ImportCharsetFromASM() :
      base( null )
    { 
    }



    public ImportCharsetFromASM( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleImport( ImportCharsetInfo importInfo, CharsetEditor Editor )
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

        int numBytesOfChar = Lookup.NumBytesOfSingleCharacterBitmap( importInfo.Charset.Mode );

        int charsToImport = (int)charData.Length / numBytesOfChar;

        charsToImport = System.Math.Min( charsToImport, importInfo.ImportIndices.Count );

        for ( int i = 0; i < charsToImport; ++i )
        {
          charData.CopyTo( importInfo.Charset.Characters[importInfo.ImportIndices[i]].Tile.Data, i * numBytesOfChar, numBytesOfChar );
          Editor.CharacterChanged( importInfo.ImportIndices[i] );
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
