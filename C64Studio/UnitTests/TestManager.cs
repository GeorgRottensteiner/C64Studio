using System;
using System.Collections.Generic;
using System.Text;

namespace RetroDevStudio.UnitTests
{
  public class TestManager
  {
    public MainForm      Main = null;



    public TestManager( MainForm Form )
    {
      Main = Form;
    }



    public void RunTests()
    {
      RunBuildTests();
    }



    public void RunBuildTests()
    {
      OpenAndBuildSolution( "Sample Projects/UnitTest 1 - Preprocessor" );
    }



    public void OpenAndBuildSolution( string Solution )
    {
      Main.CloseSolution();

      string[]  files = System.IO.Directory.GetFiles( GR.Path.Append( System.Windows.Forms.Application.StartupPath, "../../../" + Solution ), "*.s64" );
      if ( files.Length < 1 )
      {
        return;
      }
      if ( !Main.OpenSolution( files[0] ) )
      {
        return;
      }
      Main.Build( Main.ActiveDocumentInfo );
    }
  }
}
