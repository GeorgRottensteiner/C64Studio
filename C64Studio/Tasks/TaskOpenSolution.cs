using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio.Tasks
{
  public class TaskOpenSolution : Task
  {
    private string          _SolutionFile;
    private Solution        _Solution = null;



    public Solution Solution
    {
      get
      {
        return _Solution;
      }
    }



    public string SolutionFilename
    {
      get
      {
        return _SolutionFile;
      }
    }



    public TaskOpenSolution( string SolutionFile )
    {
      _SolutionFile = SolutionFile;
    }



    protected override bool ProcessTask()
    {
      GR.Memory.ByteBuffer    solutionData = GR.IO.File.ReadAllBytes( _SolutionFile );
      if ( solutionData == null )
      {
        return false;
      }
      Core.MainForm.m_Solution = new Solution( Core.MainForm );
      _Solution = Core.MainForm.m_Solution;
      
      if ( !_Solution.FromBuffer( solutionData, _SolutionFile ) )
      {
        return false;
      }
      return true;
    }
  }
}
