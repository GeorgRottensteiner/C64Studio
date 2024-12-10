using RetroDevStudio.Dialogs;



namespace RetroDevStudio.IdleQueue
{
  public class IdleRequest
  {
    public RequestData          DebugRequest = null;
    public string               OpenLastSolution = null;
    public FormSplashScreen     CloseSplashScreen = null;
    public bool                 AutoSaveSettings = false;
  }
}
