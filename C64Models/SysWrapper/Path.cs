using System;

namespace RetroDevStudio
{
  public static class SysWrapper
  {
    private static int    _tempFileIndex = 0;
    internal static bool  s_IsRunningUnderWINE = false;



    private static int RunExternalCommandDirectly( string command, string arguments, out string output )
    {
      output = "";

      try
      {
        System.Diagnostics.Process proc = new System.Diagnostics.Process();
        proc.StartInfo.FileName = command;
        proc.StartInfo.Arguments = arguments;
        proc.StartInfo.CreateNoWindow = true;
        proc.StartInfo.UseShellExecute = false;
        proc.StartInfo.RedirectStandardOutput = true;
        proc.StartInfo.RedirectStandardError = true;

        if ( !proc.Start() )
        {
          proc.Close();
          return -1;
        }

        // Read standard output and error fully (synchronously)
        string stdOut = proc.StandardOutput.ReadToEnd();
        string stdErr = proc.StandardError.ReadToEnd();

        // Wait for process to exit
        proc.WaitForExit();

        output = stdOut;
        if ( !string.IsNullOrEmpty( stdErr ) )
        {
          if ( !string.IsNullOrEmpty( output ) )
          {
            output += System.Environment.NewLine;
          }
          output += stdErr;
        }

        int exitCode = proc.ExitCode;
        proc.Close();
        return exitCode;
      }
      catch ( Exception ex )
      {
        output = ex.Message;
        return -1;
      }
    }



    public static string MakeUnixPath( string path )
    {
      int resultCode = RunExternalCommandDirectly( "winepath", "-u \"" + System.IO.Path.GetTempPath() + "\"", out string output );
      if ( resultCode != 0 )
      {
        return "\"Failed to get temp path\"";
      }
      return output;
    }



    public static string MapPath( string path )
    {
      if ( !s_IsRunningUnderWINE )
      {
        return path;
      }
      return MakeUnixPath( path );
    }



    public static string GetTempPath()
    {
      if ( !s_IsRunningUnderWINE )
      {
        return System.IO.Path.GetTempPath();
      }
      return MakeUnixPath( System.IO.Path.GetTempPath() );
    }



    public static string GetTempFileName()
    {
      if ( !s_IsRunningUnderWINE )
      {
        return System.IO.Path.GetTempFileName();
      }
      // assume Linux/Mac, do it ourselves (Wine does not support GetTempFileName properly?)
      string tempPath = System.IO.Path.GetTempPath();

      string tempFilename = "tmp" + System.Threading.Interlocked.Increment( ref _tempFileIndex ).ToString( "D6" ) + ".tmp";

      return GR.Path.Append( tempPath, tempFilename, "/" );
    }



  }
}