using C64Studio.Types;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Authentication;
using System.Text;

namespace C64Studio.Tasks
{
  public class VersionInfoParts
  {
    public int    Major = 0;
    public int    Minor = 0;
    public char   Sub = ' ';
  };

  public class TaskCheckForUpdate : Task
  {
    private HttpWebRequest _Request = null;



    public TaskCheckForUpdate()
    {
    }


    // hack to allow TLS1.2 with .NET 3.5
    public const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
    public const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;



    void FinishWebRequest( IAsyncResult result )
    {
      try
      {
        HttpWebResponse response = (HttpWebResponse)_Request.EndGetResponse( result );

        // Insert code that uses the response object
        var encoding = Encoding.GetEncoding( response.CharacterSet );

        using ( var reader = new System.IO.StreamReader( response.GetResponseStream(), encoding ) )
        {
          string responseText = reader.ReadToEnd();

          if ( responseText.StartsWith( "OK" ) )
          {
            string[]    parts = responseText.Trim().Split( new string[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries );
            if ( parts.Length == 2 )
            {
              CompareVersions( parts[1] );
            }
            else
            {
              Core.SetStatus( "Malformed update check reply: " + responseText );
            }
          }
          else
          {
            Core.SetStatus( "Malformed update check reply: " + responseText );
          }
          Core.AddToOutput( responseText );
        }
        response.Close();
      }
      catch ( Exception ex )
      {
        Core.AddToOutput( "Failed to check for update: " + ex.Message );
      }
    }



    protected override bool ProcessTask()
    {
      Core.SetStatus( "Check for update..." );
      Core.Settings.LastUpdateCheck = DateTime.Now;

      try
      {
        ServicePointManager.SecurityProtocol = Tls12;
        _Request = (HttpWebRequest)WebRequest.Create( "https://www.georg-rottensteiner.de/scripts/checkversion.php?checkversion=AppVersion&name=3" );

        _Request.BeginGetResponse( new AsyncCallback( FinishWebRequest ), null );
      }
      catch ( Exception ex )
      {
        Core.AddToOutput( "Failed to check for update: " + ex.Message );
        return false;
      }
      return true;
    }



    private void CompareVersions( string ReceivedVersion )
    {
      var receivedVersionInfo = VersionInfo( ReceivedVersion );
      var myVersionInfo = VersionInfo( StudioCore.StudioVersion );

      bool    newerVersionAvailable = false;

      if ( ( receivedVersionInfo.Major < myVersionInfo.Major )
      ||   ( ( receivedVersionInfo.Major == myVersionInfo.Major )
      &&     ( receivedVersionInfo.Minor < myVersionInfo.Minor ) ) )
      {
        Core.SetStatus( "Newest version looks older than local: " + ReceivedVersion );
        return;
      }
      if ( ( receivedVersionInfo.Major > myVersionInfo.Major )
      ||   ( ( receivedVersionInfo.Major == myVersionInfo.Major )
      &&     ( receivedVersionInfo.Minor > myVersionInfo.Minor ) )
      ||   ( ( receivedVersionInfo.Major == myVersionInfo.Major )
      &&     ( receivedVersionInfo.Minor == myVersionInfo.Minor )
      &&     ( receivedVersionInfo.Sub > myVersionInfo.Sub ) ) )
      {
        newerVersionAvailable = true;
      }

      if ( newerVersionAvailable )
      {
        Core.SetStatus( "A newer version is available: " + ReceivedVersion );
      }
      else
      {
        Core.SetStatus( "Up to date: " + ReceivedVersion );
      }
    }



    /// <summary>
    /// Split version string of expected format <Major>.<Minor>[<Sub>]
    /// </summary>
    /// <param name="Version"></param>
    /// <returns></returns>
    private VersionInfoParts VersionInfo( string Version )
    {
      VersionInfoParts    vInfo = new VersionInfoParts();

      int   dotPos = Version.IndexOf( '.' );
      if ( ( dotPos == -1 )
      ||   ( string.IsNullOrEmpty( Version ) ) )
      {
        // malformed!
        return vInfo;
      }
      vInfo.Major = GR.Convert.ToI32( Version.Substring( 0, dotPos ) );

      if ( char.IsLetter( Version[Version.Length - 1] ) )
      {
        vInfo.Sub = Version[Version.Length - 1];
        vInfo.Minor = GR.Convert.ToI32( Version.Substring( dotPos + 1, Version.Length - 1 - dotPos - 1 ) );
      }
      else
      {
        vInfo.Minor = GR.Convert.ToI32( Version.Substring( dotPos + 1 ) );
      }
      return vInfo;
    }



    public override string ToString()
    {
      return base.ToString() + " - UpdateCheck";
    }

  }
}
