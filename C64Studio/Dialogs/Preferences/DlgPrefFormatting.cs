using GR.Collections;
using GR.Strings;
using RetroDevStudio.Types;
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
  [Description( "General.Formatting" )]
  public partial class DlgPrefFormatting : DlgPrefBase
  {
    public DlgPrefFormatting()
    {
      InitializeComponent();
    }



    public DlgPrefFormatting( StudioCore Core ) : base( Core )
    {
      _Keywords.AddRange( new string[] { "source", "format", "layout", "space", "indent" } );

      InitializeComponent();
    }



    public override void ApplySettingsToControls()
    {
      checkAutoFormatActive.Checked = Core.Settings.FormatSettings.AutoFormatActive;

      checkIndentStatements.Checked = Core.Settings.FormatSettings.IndentStatements;
      editIndentStatements.Text     = Core.Settings.FormatSettings.NumTabsIndentationStatements.ToString();

      checkIndentLabels.Checked     = Core.Settings.FormatSettings.IndentLabels;
      editIndentLabels.Text         = Core.Settings.FormatSettings.NumTabsIndentationLabels.ToString();

      checkIndentPseudoOps.Checked  = Core.Settings.FormatSettings.IndentPseudoOpsLikeCode;
      editIndentPseudoOps.Text      = Core.Settings.FormatSettings.NumTabsIndentationPseudoOps.ToString();

      checkInsertSpacesBetweenOpcodeAndParameters.Checked   = Core.Settings.FormatSettings.SeparateInstructionsAndOperands;
      editInsertSpacesBetweenOpcodesAndArguments.Text       = Core.Settings.FormatSettings.IndentOperandsFromInstructions.ToString();

      checkInsertSpacesBetweenOperands.Checked              = Core.Settings.FormatSettings.InsertSpacesBetweenOperands;

      checkSeparateLineForLabels.Checked                    = Core.Settings.FormatSettings.PutLabelsOnSeparateLine;

      foreach ( Control control in Controls )
      {
        if ( control != checkAutoFormatActive )
        {
          control.Enabled = Core.Settings.FormatSettings.AutoFormatActive;
        }
      }

      var namedPOs = new Dictionary<MacroInfo.PseudoOpType, Set<string>>();
      foreach ( AssemblerType assemblerType in Enum.GetValues( typeof( AssemblerType ) ) )
      {
        if ( assemblerType == AssemblerType.AUTO )
        {
          continue;
        }
        var assemblerSettings = new RetroDevStudio.Parser.AssemblerSettings();
        assemblerSettings.SetAssemblerType( assemblerType );

        foreach ( MacroInfo.PseudoOpType pseudoOp in Enum.GetValues( typeof( MacroInfo.PseudoOpType ) ) )
        {
          var pos = assemblerSettings.PseudoOps.Where( po => po.Value.Type == pseudoOp ).Select( po => po.Key );

          foreach ( var singlePO in pos )
          {
            if ( namedPOs.ContainsKey( pseudoOp ) )
            {
              namedPOs[pseudoOp].Add( singlePO );
            }
            else
            {
              namedPOs.Add( pseudoOp, new Set<string>() );
              namedPOs[pseudoOp].Add( singlePO );
            }
          }
        }
      }

      listPseudoOpsToIndent.BeginUpdate();
      foreach ( var pseudoOp in namedPOs )
      {
        int itemIndex = -1;
        var displayText = string.Join( ",",  pseudoOp.Value.ToArray() );
        if ( !string.IsNullOrEmpty( displayText ) )
        {
          itemIndex = listPseudoOpsToIndent.Items.Add( displayText );
        }
        else
        {
          itemIndex = listPseudoOpsToIndent.Items.Add( GR.EnumHelper.GetDescription( pseudoOp.Key ) );
        }
        listPseudoOpsToIndent.Items[itemIndex].Tag      = pseudoOp.Key;
        listPseudoOpsToIndent.Items[itemIndex].Checked  = Core.Settings.FormatSettings.PseudoOpsToIndent.Contains( pseudoOp.Key );
      }
      listPseudoOpsToIndent.EndUpdate();
    }



    public override void ExportSettings( XMLElement SettingsRoot )
    {
      var xmlFormatting = SettingsRoot.AddChild( "Formatting" );

      xmlFormatting.AddAttribute( "Active", Core.Settings.FormatSettings.AutoFormatActive ? "yes" : "no" );
      xmlFormatting.AddAttribute( "IndentLabels", Core.Settings.FormatSettings.IndentLabels ? "yes" : "no" );
      xmlFormatting.AddAttribute( "IndentLabelsTabs", Core.Settings.FormatSettings.NumTabsIndentationLabels.ToString() );
      xmlFormatting.AddAttribute( "IndentStatements", Core.Settings.FormatSettings.IndentStatements ? "yes" : "no" );
      xmlFormatting.AddAttribute( "IndentStatementsTabs", Core.Settings.FormatSettings.NumTabsIndentationStatements.ToString() );
      xmlFormatting.AddAttribute( "IndentPseudoOps", Core.Settings.FormatSettings.IndentPseudoOpsLikeCode ? "yes" : "no" );
      xmlFormatting.AddAttribute( "IndentPseudoOpsTabs", Core.Settings.FormatSettings.NumTabsIndentationPseudoOps.ToString() );

      xmlFormatting.AddAttribute( "SpacingInstructionsOperands", Core.Settings.FormatSettings.SeparateInstructionsAndOperands ? "yes" : "no" );
      xmlFormatting.AddAttribute( "SpacingInstructionsOperandsSpaces", Core.Settings.FormatSettings.IndentOperandsFromInstructions.ToString() );

      xmlFormatting.AddAttribute( "SeparateLineForLabels", Core.Settings.FormatSettings.PutLabelsOnSeparateLine ? "yes" : "no" );
      xmlFormatting.AddAttribute( "InsertSpacesBetweenOperands", Core.Settings.FormatSettings.InsertSpacesBetweenOperands ? "yes" : "no" );
    }



    public override void ImportSettings( XMLElement SettingsRoot )
    {
      var xmlFormatting = SettingsRoot.FindByType( "Formatting" );
      if ( xmlFormatting != null )
      {
        Core.Settings.FormatSettings.AutoFormatActive = IsSettingTrue( xmlFormatting.Attribute( "Active" ) );

        Core.Settings.FormatSettings.IndentLabels = IsSettingTrue( xmlFormatting.Attribute( "IndentLabels" ) );
        Core.Settings.FormatSettings.NumTabsIndentationLabels = GR.Convert.ToI32( xmlFormatting.Attribute( "IndentLabelsTabs" ) );

        Core.Settings.FormatSettings.IndentStatements = IsSettingTrue( xmlFormatting.Attribute( "IndentStatements" ) );
        Core.Settings.FormatSettings.NumTabsIndentationStatements = GR.Convert.ToI32( xmlFormatting.Attribute( "IndentStatementsTabs" ) );

        Core.Settings.FormatSettings.IndentPseudoOpsLikeCode = IsSettingTrue( xmlFormatting.Attribute( "IndentPseudoOps" ) );
        Core.Settings.FormatSettings.NumTabsIndentationPseudoOps = GR.Convert.ToI32( xmlFormatting.Attribute( "IndentPseudoOpsTabs" ) );

        Core.Settings.FormatSettings.SeparateInstructionsAndOperands = IsSettingTrue( xmlFormatting.Attribute( "SpacingInstructionsOperands" ) );
        Core.Settings.FormatSettings.IndentOperandsFromInstructions = GR.Convert.ToI32( xmlFormatting.Attribute( "SpacingInstructionsOperandsSpaces" ) );

        Core.Settings.FormatSettings.PutLabelsOnSeparateLine = IsSettingTrue( xmlFormatting.Attribute( "SeparateLineForLabels" ) );
        Core.Settings.FormatSettings.InsertSpacesBetweenOperands = IsSettingTrue( xmlFormatting.Attribute( "InsertSpacesBetweenOperands" ) );
      }
    }



    private void checkAutoFormatActive_CheckedChanged( object sender, EventArgs e )
    {
      if ( checkAutoFormatActive.Checked != Core.Settings.FormatSettings.AutoFormatActive )
      {
        Core.Settings.FormatSettings.AutoFormatActive = checkAutoFormatActive.Checked;

        foreach ( Control control in Controls )
        {
          if ( control != checkAutoFormatActive )
          {
            control.Enabled = Core.Settings.FormatSettings.AutoFormatActive;
          }
        }
      }
    }



    private void checkIndentStatements_CheckedChanged( object sender, EventArgs e )
    {
      if ( checkIndentStatements.Checked != Core.Settings.FormatSettings.IndentStatements )
      {
        Core.Settings.FormatSettings.IndentStatements = checkIndentStatements.Checked;
        editIndentStatements.Enabled = Core.Settings.FormatSettings.IndentStatements;
      }
    }



    private void checkIndentLabels_CheckedChanged( object sender, EventArgs e )
    {
      if ( checkIndentLabels.Checked != Core.Settings.FormatSettings.IndentLabels )
      {
        Core.Settings.FormatSettings.IndentLabels = checkIndentLabels.Checked;
        editIndentLabels.Enabled = Core.Settings.FormatSettings.IndentLabels;
      }
    }



    private void checkIndentPseudoOps_CheckedChanged( object sender, EventArgs e )
    {
      if ( checkIndentPseudoOps.Checked != Core.Settings.FormatSettings.IndentPseudoOpsLikeCode )
      {
        Core.Settings.FormatSettings.IndentPseudoOpsLikeCode = checkIndentPseudoOps.Checked;
        editIndentPseudoOps.Enabled = Core.Settings.FormatSettings.IndentPseudoOpsLikeCode;
      }
    }



    private void checkInsertSpacesBetweenOpcodeAndParameters_CheckedChanged( object sender, EventArgs e )
    {
      if ( checkInsertSpacesBetweenOpcodeAndParameters.Checked != Core.Settings.FormatSettings.SeparateInstructionsAndOperands )
      {
        Core.Settings.FormatSettings.SeparateInstructionsAndOperands = checkInsertSpacesBetweenOpcodeAndParameters.Checked;
        editInsertSpacesBetweenOpcodesAndArguments.Enabled = Core.Settings.FormatSettings.SeparateInstructionsAndOperands;
      }
    }



    private void checkInsertSpacesBetweenOperands_CheckedChanged( object sender, EventArgs e )
    {
      if ( checkInsertSpacesBetweenOperands.Checked != Core.Settings.FormatSettings.InsertSpacesBetweenOperands )
      {
        Core.Settings.FormatSettings.InsertSpacesBetweenOperands = checkInsertSpacesBetweenOperands.Checked;
      }
    }

    
    
    private void checkSeparateLineForLabels_CheckedChanged( object sender, EventArgs e )
    {
      if ( checkSeparateLineForLabels.Checked != Core.Settings.FormatSettings.PutLabelsOnSeparateLine )
      {
        Core.Settings.FormatSettings.PutLabelsOnSeparateLine = checkSeparateLineForLabels.Checked;
      }
    }

    
    
    private void editIndentStatements_TextChanged( object sender, EventArgs e )
    {
      if ( int.TryParse( editIndentStatements.Text, out int numTabs ) )
      {
        Core.Settings.FormatSettings.NumTabsIndentationStatements = numTabs;
      }
    }

    
    
    private void editIndentLabels_TextChanged( object sender, EventArgs e )
    {
      if ( int.TryParse( editIndentLabels.Text, out int numTabs ) )
      {
        Core.Settings.FormatSettings.NumTabsIndentationLabels = numTabs;
      }
    }



    private void editIndentPseudoOps_TextChanged( object sender, EventArgs e )
    {
      if ( int.TryParse( editIndentPseudoOps.Text, out int numTabs ) )
      {
        Core.Settings.FormatSettings.NumTabsIndentationPseudoOps = numTabs;
      }
    }



    private void editInsertSpacesBetweenOpcodesAndArguments_TextChanged( object sender, EventArgs e )
    {
      if ( int.TryParse( editInsertSpacesBetweenOpcodesAndArguments.Text, out int numSpaces ) )
      {
        Core.Settings.FormatSettings.IndentOperandsFromInstructions = numSpaces;
      }
    }



    private void listPseudoOpsToIndent_CheckChanged( DecentForms.ControlBase Sender )
    {
      if ( listPseudoOpsToIndent.SelectedIndex != -1 )
      {
        var pseudoOp = (MacroInfo.PseudoOpType)listPseudoOpsToIndent.Items[listPseudoOpsToIndent.SelectedIndex].Tag;
        if ( listPseudoOpsToIndent.Items[listPseudoOpsToIndent.SelectedIndex].Checked )
        {
          if ( !Core.Settings.FormatSettings.PseudoOpsToIndent.Contains( pseudoOp ) )
          {
            Core.Settings.FormatSettings.PseudoOpsToIndent.Add( pseudoOp );
          }
        }
        else
        {
          Core.Settings.FormatSettings.PseudoOpsToIndent.Remove( pseudoOp );
        }
      }
    }



  }
}
