using GR.Memory;
using RetroDevStudio;
using RetroDevStudio.Audio;
using RetroDevStudio.Controls;
using RetroDevStudio.Formats;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;
using System.Windows.Media.Effects;
using WeifenLuo.WinFormsUI.Docking;



namespace RetroDevStudio.Documents
{
  public partial class SFXEditor : BaseDocument
  {
    private AudioHandler              _audio = new AudioHandler();

    private List<SFXPlayerDescriptor> _players = new List<SFXPlayerDescriptor>();

    private SFXProject                _project = new SFXProject();

    private SFXPlayerDescriptor       _currentPlayer = null;
    private bool                      _updatingParams = false;



    public SFXEditor()
    {
      InitializeComponent();

      GR.Image.DPIHandler.ResizeControlsForDPI( this );
    }



    public override void OnApplicationEvent( ApplicationEvent Event )
    {
      switch ( Event.EventType )
      {
        case ApplicationEvent.Type.SHUTTING_DOWN:
          Stop();
          break;
      }
      base.OnApplicationEvent( Event );
    }



    public SFXEditor( StudioCore core )
    {
      Core = core;
      InitializeComponent();
      // manually adding for now
      int sfxDataAddress = 0xa31;
      var player1 = new SFXPlayerDescriptor()
      {
        Name = "Retro Dev Studio SID Player",
        PlayerCode = new ByteBuffer( "0B080A009E32303631000000A90F8D18D4A9018D190E20E408208109AD190EF0F5A9008D190E205C09AD1A0E205F084C1708AC280AB9150A8D1E0AA9008D04D4A200E004F009BD1A0A9D210A9D00D4E8E007D0EEAD1E0A8D250A8D04D460AABD310A29038D280AA8B9150A8D250ABD310A4A4A8D2A0ABD950A8D210ABDF90A8D220ABD5D0B8D230ABDC10B8D240ABD250C8D260ABD890C8D270ABDED0C8D2C0ABD510D8D2E0ABDB50D8D300A4CB008AE210AAC220A8E1A0A8C1B0AA202BD210A9D1A0AE8E007D0F5AD2A0A8D290AAD300A8D2F0AAD2E0A8D2D0AAD2C0A8D2B0A4C3308AD12D0C9F8F0F9AD12D0C9F8D0F96086578458A000B1574A4A8D2A0A8D290AB1572903A88C280AB9150A8D1E0AA9008D04D4A200A001E004F00CB1579D1A0A9D210A9D00D4C8E8E007D0EBA004AD1E0A9D250A8D04D4A007B1578D2B0A8D2C0AC8B1578D2D0A8D2E0AC8B1578D2F0A8D300AA9018D190A60A9FFA2179D00D4CA10FAAA1004A90810F12C11D010FB2C11D030FB4908F0E3A90F8D18D460AD190AD00160AC290AB999098D9709B99D098D98094CFFFFA1D3D9FC09090909CE2D0AF021AD2B0A100CAD1B0A186D2B0A90134CC009AD1B0A186D2B0AB0078D1B0A8D01D460A9008D2B0A8D04D48D190A60CE2D0AF0EF60CE2D0AD01DAD1B0A186D2F0A8D1B0A8D01D4A9008D2B0AAD2E0A8D2D0AA9008D290A60CE2F0AD011AD300A8D2F0AAD2B0A49FF1869018D2B0A4CA1091121418100000000000000000000000000000000000000000000000002020202020202020202020202020202020202020202020202020202020202020202020202020202020202020202020202020202020202020202020202020202020202020202020202020202020202020202020202020202020202020202020202020202252525252525252525252525252525252525252525252525252525252525252525252525252525252525252525252525252525252525252525252525252525252525252525252525252525252525252525252525252525252525252525252525252525254D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4D4DC5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5C5EDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDEDED95959595959595959595959595959595959595959595959595959595959595959595959595959595959595959595959595959595959595959595959595959595959595959595959595959595959595959595959595959595959595959595959595959595BDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBDBD030303030303030303030303030303030303030303030303030303030303030303030303030303030303030303030303030303030303030303030303030303030303030303030303030303030303030303030303030303030303030303030303030303035E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E5E323232323232323232323232323232323232323232323232323232323232323232323232323232323232323232323232323232323232323232323232323232323232323232323232323232323232323232323232323232323232323232323232323232320000" ),
        PlayerCodeAddress = 0x0801,
        AddressToStartPlayer = 2061,
        AddressToTriggerPlaying = 0xe19,
        ValueToTriggerPlaying = 1
      };
      player1.Parameters.Add( new ValueDescriptor()
      {
        AddressToWriteTo = sfxDataAddress,
        Name = "Effect",
        RelevantBits = 0xfc,
        ShiftBitsLeft = 2,
        ValidValues = new Dictionary<string, int>()
        {
          { "Slide", 0 },
          { "None", 1 },
          { "Step", 2 },
          { "Slide Ping Pong", 3 }
        }
      } );
      player1.Parameters.Add( new ValueDescriptor()
      {
        AddressToWriteTo = sfxDataAddress,
        Name = "Waveform",
        RelevantBits = 0x03,
        ValidValues = new Dictionary<string, int>()
        {
          { "Triangle", 0 },
          { "Sawtooth", 1 },
          { "Pulse", 2 },
          { "Noise", 3 }
        }
      } );
      player1.Parameters.Add( new ValueDescriptor()
      {
        AddressToWriteTo = sfxDataAddress + 100,
        Name = "Frequency", // low
        RelevantBits = 0xff,
        MinValue = 0,
        MaxValue = 65535
      } );
      player1.Parameters.Add( new ValueDescriptor()
      {
        AddressToWriteTo = sfxDataAddress + 2 * 100,
        Name = "Frequency",  // hi
        RelevantBits = 0xff,
        ShiftBitsRight = 8,
        MinValue = 0,
        MaxValue = 65535
      } );
      player1.Parameters.Add( new ValueDescriptor()
      {
        AddressToWriteTo = sfxDataAddress + 3 * 100,
        Name = "Attack",
        RelevantBits = 0xf0,
        ShiftBitsLeft = 4,
        MinValue = 0,
        MaxValue = 15
      } );
      player1.Parameters.Add( new ValueDescriptor()
      {
        AddressToWriteTo = sfxDataAddress + 3 * 100,
        Name = "Decay",
        RelevantBits = 0x0f,
        MinValue = 0,
        MaxValue = 15
      } );
      player1.Parameters.Add( new ValueDescriptor()
      {
        AddressToWriteTo = sfxDataAddress + 4 * 100,
        Name = "Sustain",
        RelevantBits = 0xf0,
        ShiftBitsLeft = 4,
        MinValue = 0,
        MaxValue = 15
      } );
      player1.Parameters.Add( new ValueDescriptor()
      {
        AddressToWriteTo = sfxDataAddress + 4 * 100,
        Name = "Release",
        RelevantBits = 0x0f,
        MinValue = 0,
        MaxValue = 15
      } );
      player1.Parameters.Add( new ValueDescriptor()
      {
        AddressToWriteTo = sfxDataAddress + 5 * 100,
        Name = "Pulse", // low
        RelevantBits = 0xff,
        MinValue = 0,
        MaxValue = 65535
      } );
      player1.Parameters.Add( new ValueDescriptor()
      {
        AddressToWriteTo = sfxDataAddress + 6 * 100,
        Name = "Pulse",  // hi
        RelevantBits = 0xff,
        ShiftBitsRight = 8,
        MinValue = 0,
        MaxValue = 65535
      } );
      player1.Parameters.Add( new ValueDescriptor()
      {
        AddressToWriteTo = sfxDataAddress + 7 * 100,
        Name = "Delta",
        MinValue = -128,
        MaxValue = 127
      } );
      player1.Parameters.Add( new ValueDescriptor()
      {
        AddressToWriteTo = sfxDataAddress + 8 * 100,
        Name = "Delay",
        MinValue = 0,
        MaxValue = 255
      } );
      player1.Parameters.Add( new ValueDescriptor()
      {
        AddressToWriteTo = sfxDataAddress + 9 * 100,
        Name = "Step",
        MinValue = -128,
        MaxValue = 127
      } );

      /*
      FX_SLIDE              = 0
FX_NONE               = 1
FX_STEP               = 2
FX_SLIDE_PING_PONG    = 3

FX_WAVE_TRIANGLE      = 0
FX_WAVE_SAWTOOTH      = 1
FX_WAVE_PULSE         = 2
FX_WAVE_NOISE         = 3

      SFX_SLOT_EFFECT_WAVE
          !fill 100,( FX_SLIDE << 2 ) | ( FX_WAVE_PULSE )
      SFX_SLOT_2_FX_LO
          !fill 100,$25
      SFX_SLOT_3_FX_HI
          !fill 100,$4d
      SFX_SLOT_4_AD
          !fill 100,$c5
      SFX_SLOT_5_SR
          !fill 100,$ed
      SFX_SLOT_6_PULSE_LO
          !fill 100,$95
      SFX_SLOT_7_PULSE_HI
          !fill 100,$bd
      SFX_SLOT_8_DELTA
          !fill 100,$03
      SFX_SLOT_9_DELAY
          !fill 100,$5e
      SFX_SLOT_10_STEP
          !fill 100,$32*/

      //_players.Add( player1 );

      _players.AddRange( Core.Audio.SFXPlayers );

      foreach ( var player in _players )
      {
        comboSFXPlayer.Items.Add( new Types.ComboItem( player.Name, player ) );
      }
      if ( comboSFXPlayer.Items.Count > 0 )
      {
        comboSFXPlayer.SelectedIndex = 0;
      }
      GR.Image.DPIHandler.ResizeControlsForDPI( this );
    }



    protected override void OnVisibleChanged( EventArgs e )
    {
      if ( IsHidden )
      {
        _audio.StopAll();
      }
      base.OnVisibleChanged( e );
    }



    protected override void OnHandleDestroyed( EventArgs e )
    {
      _audio.StopAll();
      _audio.Dispose();
    }



    public override System.Drawing.Size GetPreferredSize( System.Drawing.Size proposedSize )
    {
      return new System.Drawing.Size( 790, 539 );
    }



    private void comboSFXPlayer_SelectedIndexChanged( object sender, EventArgs e )
    {
      _audio.StopAll();

      if ( comboSFXPlayer.SelectedItem == null )
      {
        _currentPlayer = null;
        return;
      }
      var item = (Types.ComboItem)comboSFXPlayer.SelectedItem;
      _currentPlayer = (SFXPlayerDescriptor)item.Tag;
      GenerateEffectParameterControls( _currentPlayer );
      _audio.SetSFXPlayer( _currentPlayer );
    }



    private void GenerateEffectParameterControls( SFXPlayerDescriptor player )
    {
      _updatingParams = true;
      panelEffectValues.Controls.Clear();

      var uniqueEffectParameters = new Dictionary<string,Control>();
      int yPos = 0;
      int widthNames = 120;

      foreach ( var param in player.Parameters )
      {
        if ( !uniqueEffectParameters.ContainsKey( param.Name ) )
        {
          Control control = null;

          var label = new System.Windows.Forms.Label();
          label.Text = param.Name + ":";
          label.Width = widthNames;
          label.AutoSize = true;
          label.Location = new System.Drawing.Point( 0, yPos );
          panelEffectValues.Controls.Add( label );

          if ( param.ValidValues.Count > 0 )
          {
            var combo = new System.Windows.Forms.ComboBox();
            combo.Tag = param;
            combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            combo.Location = new System.Drawing.Point( widthNames, yPos );
            foreach ( var validValue in param.ValidValues )
            {
              combo.Items.Add( new Types.ComboItem( validValue.Key, validValue.Value ) );
            }
            combo.SelectedIndex = 0;
            combo.SelectedIndexChanged += EffectParameter_Changed;
            combo.Tag = new List<ValueDescriptor>() { param };
            panelEffectValues.Controls.Add( combo );
            control = combo;
          }
          else
          {
            var numericUpDown = new System.Windows.Forms.NumericUpDown();
            numericUpDown.Tag = param;
            numericUpDown.Minimum = param.MinValue;
            numericUpDown.Maximum = param.MaxValue;
            numericUpDown.Location = new System.Drawing.Point( widthNames, yPos );
            numericUpDown.ValueChanged += EffectParameter_Changed;
            numericUpDown.Tag = new List<ValueDescriptor>() { param };
            panelEffectValues.Controls.Add( numericUpDown );
            control = numericUpDown;
          }
          uniqueEffectParameters.Add( param.Name, control );
          yPos += 24;
        }
        else
        {
          // already exists, need to extend
          var existingControl = uniqueEffectParameters[param.Name];
          var paramList = (List<ValueDescriptor>)existingControl.Tag;
          paramList.Add( param );
        }
      }
      _updatingParams = false;
    }



    private void EffectParameter_Changed( object sender, EventArgs e )
    {
      if ( _updatingParams )
      {
        return;
      }
      _updatingParams = true;


      if ( GetValueFromControl( (Control)sender, out var effectname, out var effectValue ) )
      {
        if ( listEffects.SelectedItem != null )
        {
          var effect = (SFXProject.SFXEffect)listEffects.SelectedItem.Tag;

          effect.Parameters[effectname] = effectValue;
        }
      }

      RestartSoundEffect();
      _updatingParams = false;
      SetModified();
    }



    private void RestartSoundEffect()
    {
      if ( _currentPlayer == null )
      {
        return;
      }
      var  modifiedData = new ByteBuffer( _currentPlayer.PlayerCode );

      UpdateEffectParameters( modifiedData );
      modifiedData.SetU8At( _currentPlayer.AddressToTriggerPlaying - _currentPlayer.PlayerCodeAddress, _currentPlayer.ValueToTriggerPlaying );

      if ( _currentPlayer.CanReplay )
      {
        _audio.Replay( modifiedData );
      }
      else
      {
        _audio.Play( modifiedData );
      }
    }



    private void UpdateEffectParameters( ByteBuffer playerData )
    {
      var targetValues = new Dictionary<int, byte>();
      foreach ( Control control in panelEffectValues.Controls )
      {
        var parms = (List<ValueDescriptor>)control.Tag;
        if ( parms == null )
        {
          continue;
        }
        int               value = 0;

        if ( control is System.Windows.Forms.ComboBox )
        {
          var combo = (System.Windows.Forms.ComboBox)control;
          if ( combo.SelectedItem != null )
          {
            value = (int)( (Types.ComboItem)combo.SelectedItem ).Tag;
          }
        }
        else if ( control is System.Windows.Forms.NumericUpDown )
        {
          var numericUpDown = (System.Windows.Forms.NumericUpDown)control;
          value = (int)numericUpDown.Value;
        }
        else
        {
          continue;
        }

        // apply relevant bits and shifts
        foreach ( var param in parms )
        {
          int finalValue = value;
          if ( param.ShiftBitsLeft > 0 )
          {
            finalValue <<= param.ShiftBitsLeft;
          }
          if ( param.ShiftBitsRight > 0 )
          {
            finalValue >>= param.ShiftBitsRight;
          }
          finalValue &= param.RelevantBits;

          if ( targetValues.ContainsKey( param.AddressToWriteTo ) )
          {
            targetValues[param.AddressToWriteTo] |= (byte)finalValue;
          }
          else
          {
            targetValues[param.AddressToWriteTo] = (byte)finalValue;
          }
          //Debug.Log( $"set param {param.Name} to {finalValue} (0x{finalValue:X}), writing to {param.AddressToWriteTo:X}" );
        }
      }

      foreach ( var entry in targetValues )
      {
        //Debug.Log( $"Set {entry.Key:X} to {entry.Value:X}" );
        playerData.SetU8At( entry.Key - _currentPlayer.PlayerCodeAddress, entry.Value );
      }
    }



    private void listEffects_ItemAdded( object sender, Controls.ArrangedItemEntry Item )
    {
      var effect = new SFXProject.SFXEffect();
      effect.Name = editEffectName.Text;

      Item.Text = effect.Name;
      Item.Tag = effect;

      if ( string.IsNullOrEmpty( Item.Text ) )
      {
        Item.Text = $"SFX {_project.Effects.Count + 1}";
        effect.Name = Item.Text;
      }

      foreach ( Control control in panelEffectValues.Controls )
      {
        if ( GetValueFromControl( control, out string effectName, out int effectValue ) )
        {
          var parms = (List<ValueDescriptor>)control.Tag;
          if ( parms == null )
          {
            continue;
          }
          int               value = 0;

          if ( control is System.Windows.Forms.ComboBox )
          {
            var combo = (System.Windows.Forms.ComboBox)control;
            var param = control.Tag as List<ValueDescriptor>;
            if ( combo.SelectedItem != null )
            {
              var item = (Types.ComboItem)combo.SelectedItem;
              effect.Parameters[param[0].Name] = (int)item.Tag;
            }
          }
          else if ( control is System.Windows.Forms.NumericUpDown )
          {
            var numericUpDown = (System.Windows.Forms.NumericUpDown)control;
            var param = control.Tag as List<ValueDescriptor>;
            value = (int)numericUpDown.Value;
            effect.Parameters[param[0].Name] = value;
          }
          else
          {
            continue;
          }

        }
      }

      _project.Effects.Add( effect );
      SetModified();
    }



    private bool GetValueFromControl( Control control, out string effectName, out int effectValue )
    {
      effectName = null;
      effectValue = 0;
      var parms = (List<ValueDescriptor>)control.Tag;
      if ( parms == null )
      {
        return false;
      }
      if ( control is System.Windows.Forms.ComboBox )
      {
        var combo = (System.Windows.Forms.ComboBox)control;
        var param = control.Tag as List<ValueDescriptor>;
        if ( combo.SelectedItem != null )
        {
          var item = (Types.ComboItem)combo.SelectedItem;

          effectName = param[0].Name;
          effectValue = (int)item.Tag;
          return true;
        }
      }
      else if ( control is System.Windows.Forms.NumericUpDown )
      {
        var numericUpDown = (System.Windows.Forms.NumericUpDown)control;
        var param = control.Tag as List<ValueDescriptor>;

        effectName = param[0].Name;
        effectValue = (int)numericUpDown.Value;

        return true;
      }
      return false;
    }



    private void listEffects_ItemMoved( object sender, Controls.ArrangedItemEntry Item1, Controls.ArrangedItemEntry Item2 )
    {
      var effect1 = (SFXProject.SFXEffect)Item1.Tag;
      var effect2 = (SFXProject.SFXEffect)Item2.Tag;

      if ( ( effect1 == null )
      || ( effect2 == null ) )
      {
        return;
      }

      int index1 = _project.Effects.IndexOf( effect1 );
      int index2 = _project.Effects.IndexOf( effect2 );

      if ( ( index1 == -1 )
      || ( index2 == -1 ) )
      {
        // If one or both not found, rebuild the list from the control order
        _project.Effects.Clear();
        foreach ( Controls.ArrangedItemEntry entry in listEffects.Items )
        {
          var eff = entry.Tag as SFXProject.SFXEffect;
          if ( eff != null )
          {
            _project.Effects.Add( eff );
          }
        }
        SetModified();
        return;
      }

      // swap the two effects in the underlying list
      var tmp = _project.Effects[index1];
      _project.Effects[index1] = _project.Effects[index2];
      _project.Effects[index2] = tmp;
      SetModified();
    }



    private void listEffects_ItemRemoved( object sender, Controls.ArrangedItemEntry Item )
    {
      var effect = (SFXProject.SFXEffect)Item.Tag;

      _project.Effects.Remove( effect );
      SetModified();
    }



    private void editEffectName_TextChanged( object sender, EventArgs e )
    {
      if ( listEffects.SelectedItem == null )
      {
        return;
      }
      var effect = (SFXProject.SFXEffect)listEffects.SelectedItem.Tag;
      if ( effect.Name != editEffectName.Text )
      {
        effect.Name = editEffectName.Text;
        listEffects.SelectedItem.Text = effect.Name;
        SetModified();
      }
    }



    private void btnPlay_Click( DecentForms.ControlBase Sender )
    {
      RestartSoundEffect();
    }



    private void listEffects_SelectedIndexChanged( object sender, ArrangedItemEntry Item )
    {
      if ( listEffects.SelectedItem == null )
      {
        return;
      }
      var effect = (SFXProject.SFXEffect)listEffects.SelectedItem.Tag;

      _updatingParams = true;
      editEffectName.Text = effect.Name;
      foreach ( var effectValue in effect.Parameters )
      {
        foreach ( Control control in panelEffectValues.Controls )
        {
          var param = control.Tag as List<ValueDescriptor>;
          if ( param != null )
          {
            if ( param[0].Name == effectValue.Key )
            {
              if ( control is ComboBox )
              {
                var combo = (ComboBox)control;
                int index = 0;
                foreach ( var value in param[0].ValidValues )
                {
                  if ( effectValue.Value == value.Value )
                  {
                    combo.SelectedIndex = index;
                    break;
                  }

                  ++index;
                }
              }
              else if ( control is NumericUpDown )
              {
                control.Text = effectValue.Value.ToString();
              }
            }
          }
        }
      }

      _updatingParams = false;
      RestartSoundEffect();
    }



    protected override bool PerformSave( string FullPath )
    {
      return GR.IO.File.WriteAllBytes( FullPath, _project.SaveToBuffer() );
    }



    public override bool LoadDocument()
    {
      if ( string.IsNullOrEmpty( DocumentInfo.DocumentFilename ) )
      {
        return false;
      }
      try
      {
        if ( !OpenProject( DocumentInfo.FullPath ) )
        {
          return false;
        }
      }
      catch ( System.IO.IOException ex )
      {
        Core.Notification.MessageBox( "Could not load file", "Could not load SFX project file " + DocumentInfo.FullPath + ".\r\n" + ex.Message );
        return false;
      }
      SetUnmodified();
      return true;
    }



    private bool OpenProject( string fullPath )
    {
      var data = GR.IO.File.ReadAllBytes( fullPath );

      if ( !_project.ReadFromBuffer( data ) )
      {
        return false;
      }


      _updatingParams = true;
      listEffects.Items.Clear();
      foreach ( var effect in _project.Effects )
      {
        var item = new ArrangedItemEntry( effect.Name );
        item.Tag = effect;

        listEffects.Items.Add( item );
      }

      _updatingParams = false;
      return true;
    }



    protected override bool QueryFilename( string PreviousFilename, out string Filename )
    {
      Filename = "";

      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Save SFX Project as";
      saveDlg.Filter = "SFX Projects|*.sfxproject|All Files|*.*";
      saveDlg.FileName = GR.Path.GetFileName( PreviousFilename );
      if ( DocumentInfo.Project != null )
      {
        saveDlg.InitialDirectory = DocumentInfo.Project.Settings.BasePath;
      }
      if ( saveDlg.ShowDialog() != DialogResult.OK )
      {
        return false;
      }

      Filename = saveDlg.FileName;
      return true;
    }



    private void btnStop_Click( DecentForms.ControlBase Sender )
    {
      Stop();
    }



    private void Stop()
    {
      if ( _currentPlayer == null )
      {
        return;
      }
      _audio.StopAll();
    }



    private void btnRandomize_Click( DecentForms.ControlBase Sender )
    {
      _updatingParams = true;

      foreach ( var parameter in _currentPlayer.Parameters )
      {

      }

      _updatingParams = false;
      RestartSoundEffect();
    }



  }
}
