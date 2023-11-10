using RetroDevStudio.Dialogs;
using RetroDevStudio.Documents;
using RetroDevStudio.Properties;
using System;
using System.Collections.Generic;



namespace RetroDevStudio
{
  public class Notification
  {
    public StudioCore         Core = null;



    public Notification( StudioCore Core )
    {
      this.Core = Core;
    }



    public void BuildSuccess( bool Force = false )
    {
      if ( ( !Force )
      &&   ( !Core.Settings.PlaySoundOnSuccessfulBuild ) )
      {
        return;
      }
      var player = new System.Media.SoundPlayer();
      player.Stream = Resources.ResourceManager.GetStream( "build_success" );
      player.Play();
      //System.Media.SystemSounds.Asterisk.Play();
    }



    public void BuildFailure( bool Force = false )
    {
      if ( ( !Force )
      &&   ( !Core.Settings.PlaySoundOnBuildFailure ) )
      {
        return;
      }
      var player = new System.Media.SoundPlayer();
      player.Stream = Resources.ResourceManager.GetStream( "build_failure" );
      player.Play();
      //System.Media.SystemSounds.Exclamation.Play();
    }



    public void ItemNotFound( bool Force = false )
    {
      if ( ( !Force )
      &&   ( !Core.Settings.PlaySoundOnSearchFoundNoItem ) )
      {
        return;
      }
      var player = new System.Media.SoundPlayer();
      player.Stream = Resources.ResourceManager.GetStream( "item_notfound" );
      player.Play();
      //System.Media.SystemSounds.Question.Play();
    }






  }
}
