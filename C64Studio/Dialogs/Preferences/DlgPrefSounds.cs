﻿using GR.Strings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace RetroDevStudio.Dialogs.Preferences
{
  [Description( "General.Sounds" )]
  public partial class DlgPrefSounds : DlgPrefBase
  {
    public DlgPrefSounds()
    {
      InitializeComponent();
    }



    public DlgPrefSounds( StudioCore Core ) : base( Core )
    {
      _Keywords.AddRange( new string[] { "sound", "play", "notification" } );

      InitializeComponent();
    }



    public override void ApplySettingsToControls()
    {
      checkPlaySoundCompileSuccessful.Checked   = Core.Settings.PlaySoundOnSuccessfulBuild;
      checkPlaySoundCompileFail.Checked         = Core.Settings.PlaySoundOnBuildFailure;
      checkPlaySoundSearchTextNotFound.Checked  = Core.Settings.PlaySoundOnSearchFoundNoItem;
    }



    public override void ExportSettings( XMLElement SettingsRoot )
    {
      var xmlSounds = SettingsRoot.AddChild( "Generic.Sounds", "" );

      var xmlSound = new XMLElement( "Sound" );
      xmlSound.AddAttribute( "Reason", "FailedBuild" );
      xmlSound.AddAttribute( "Play", Core.Settings.PlaySoundOnBuildFailure ? "yes" : "no" );
      xmlSounds.AddChild( xmlSound );

      xmlSound = new XMLElement( "Sound" );
      xmlSound.AddAttribute( "Reason", "SuccessfulBuild" );
      xmlSound.AddAttribute( "Play", Core.Settings.PlaySoundOnSuccessfulBuild ? "yes" : "no" );
      xmlSounds.AddChild( xmlSound );

      xmlSound = new XMLElement( "Sound" );
      xmlSound.AddAttribute( "Reason", "SearchFoundNoItem" );
      xmlSound.AddAttribute( "Play", Core.Settings.PlaySoundOnSearchFoundNoItem ? "yes" : "no" );
      xmlSounds.AddChild( xmlSound );
    }



    public override void ImportSettings( XMLElement SettingsRoot )
    {
      var sounds = SettingsRoot.FindByType( "Generic.Sounds" );
      if ( sounds != null )
      {
        foreach ( var child in sounds.ChildElements )
        {
          if ( child.Type != "Sound" )
          {
            continue;
          }
          string  reason = child.Attribute( "Reason" ).ToUpper();
          bool playYes = IsSettingTrue( child.Attribute( "Play" ) );

          switch ( reason )
          {
            case "FAILEDBUILD":
              Core.Settings.PlaySoundOnBuildFailure = playYes;
              break;
            case "SEARCHFOUNDNOITEM":
              Core.Settings.PlaySoundOnSearchFoundNoItem = playYes;
              break;
            case "SUCCESSFULBUILD":
              Core.Settings.PlaySoundOnSuccessfulBuild = playYes;
              break;
          }
        }
      }
    }



    private void checkPlaySoundCompileFail_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.PlaySoundOnBuildFailure = checkPlaySoundCompileFail.Checked;
    }



    private void checkPlaySoundCompileSuccessful_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.PlaySoundOnSuccessfulBuild = checkPlaySoundCompileSuccessful.Checked;
    }



    private void checkPlaySoundSearchTextNotFound_CheckedChanged( object sender, EventArgs e )
    {
      Core.Settings.PlaySoundOnSearchFoundNoItem = checkPlaySoundSearchTextNotFound.Checked;
    }



    private void btnTestSoundBuildFailure_Click( DecentForms.ControlBase Sender )
    {
      Core.Notification.BuildFailure( true );
    }



    private void btnTestSoundBuildSuccess_Click( DecentForms.ControlBase Sender )
    {
      Core.Notification.BuildSuccess( true );
    }



    private void btnTestSoundNotFound_Click( DecentForms.ControlBase Sender )
    {
      Core.Notification.ItemNotFound( true );
    }



  }
}
