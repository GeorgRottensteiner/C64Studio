using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RetroDevStudio.Types
{
  public class SampleProject
  {
    public SampleProjectType Type = SampleProjectType.SAMPLE;
    public string URL = "";
    public string SourceFolder = "";
    public string Name = "";
    public string ShortDescription = "";
    public string LongDescription = "";
    public Image  Image = null;
    public MachineType Machine = MachineType.C64;
  }
}
