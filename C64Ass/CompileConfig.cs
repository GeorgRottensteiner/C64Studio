using System;
using System.Collections.Generic;
using System.Text;



namespace C64Ass
{
  public class CompileConfig
  {
    public string                               InputFile = "";
    public string                               OutputFile = null;
    public int                                  StartAddress = -1;
    public C64Studio.Types.CompileTargetType    TargetType = C64Studio.Types.CompileTargetType.NONE;
  }
}
