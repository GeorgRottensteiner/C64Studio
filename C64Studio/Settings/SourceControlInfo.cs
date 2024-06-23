using GR.Collections;
using GR.IO;
using GR.Memory;
using System;
using System.Collections.Generic;
using WeifenLuo.WinFormsUI.Docking;

namespace RetroDevStudio
{
  public class SourceControlInfo
  {
    public string           CommitAuthor = "";
    public string           CommitAuthorEmail = "";

    public bool             CreateSolutionRepository = true;
    public bool             CreateProjectRepository = true;
  }



}