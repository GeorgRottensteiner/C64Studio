namespace RetroDevStudio
{
  public static class SysWrapper
  {
    private static int    _tempFileIndex = 0;
    internal static bool  s_IsRunningUnderWINE = false;



    public static string GetTempPath()
    {
      if ( !s_IsRunningUnderWINE )
      {
        return System.IO.Path.GetTempPath();
      }
      // assume Linux/Mac, do it ourselves (Wine does not support GetTempPath properly?)
      string path = System.Environment.GetEnvironmentVariable( "TMPDIR" );
      return string.IsNullOrEmpty( path ) ? "/tmp/" :
             ( path[path.Length - 1] == '/' ) ? path :
             path + "/";
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

      return GR.Path.Append( tempPath, tempFilename );
    }



  }
}