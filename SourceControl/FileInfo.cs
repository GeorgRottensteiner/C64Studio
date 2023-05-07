using System;
using System.Collections.Generic;
#if NET6_0_OR_GREATER
using LibGit2Sharp;
#endif

namespace SourceControl
{
  public class FileInfo
  {
    public string     Filename = "";
    public FileState  FileState = FileState.Nonexistent;
  }



}
