using RetroDevStudio.Formats;
using System.Windows.Forms;
using RetroDevStudio.Documents;
using GR.Memory;



namespace RetroDevStudio.Controls
{
  public partial class ImportBinaryFromASM : ImportBinaryFormBase
  {
    public ImportBinaryFromASM() :
      base( null )
    { 
    }



    public ImportBinaryFromASM( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleImport( DocumentInfo docInfo, BinaryDisplay parent, out ByteBuffer importedData )
    {
      importedData = null;

      Parser.ASMFileParser asmParser = new RetroDevStudio.Parser.ASMFileParser();

      Parser.CompileConfig config = new Parser.CompileConfig();
      config.TargetType = Types.CompileTargetType.PLAIN;
      config.OutputFile = "temp.bin";
      config.Assembler  = Types.AssemblerType.C64_STUDIO;

      string    temp = "* = $0801\n" + editInput.Text;
      if ( ( asmParser.Parse( temp, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) )
      &&   ( asmParser.Assemble( config ) ) )
      {
        importedData = asmParser.AssembledOutput.Assembly;
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
