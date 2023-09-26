using RetroDevStudio.Types.ASM;
using RetroDevStudio;
using System.Windows.Forms;

namespace RetroDevStudio.Types
{
  public class FunctionInfo
  {
    public Function             Function = Function.NONE;
    public string               Description = "";
    public FunctionStudioState  State = FunctionStudioState.ANY;
    public ToolStripMenuItem    MenuItem = null;
    public ToolStripButton      ToolBarButton = null;

    public FunctionInfo( Function Func, string Desc, FunctionStudioState State )
    {
      Function = Func;
      Description = Desc;
      this.State = State;
    }
  };



}
