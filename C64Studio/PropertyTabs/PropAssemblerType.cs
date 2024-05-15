using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RetroDevStudio
{
  public partial class PropAssemblerType : PropertyTabs.PropertyTabBase
  {
    ProjectElement        Element;
    StudioCore            Core;



    public PropAssemblerType( ProjectElement Element, StudioCore Core )
    {
      this.Element = Element;
      this.Core = Core;
      TopLevel = false;
      Text = "Assembler";
      InitializeComponent();

      comboAssemblerType.Items.Add( new GR.Generic.Tupel<string, Types.AssemblerType>( "Auto", RetroDevStudio.Types.AssemblerType.AUTO ) );
      comboAssemblerType.Items.Add( new GR.Generic.Tupel<string, Types.AssemblerType>( "C64Studio", RetroDevStudio.Types.AssemblerType.C64_STUDIO ) );
      comboAssemblerType.Items.Add( new GR.Generic.Tupel<string, Types.AssemblerType>( "ACME", RetroDevStudio.Types.AssemblerType.ACME ) );
      comboAssemblerType.Items.Add( new GR.Generic.Tupel<string, Types.AssemblerType>( "PDS", RetroDevStudio.Types.AssemblerType.PDS ) );
      comboAssemblerType.Items.Add( new GR.Generic.Tupel<string, Types.AssemblerType>( "DASM", RetroDevStudio.Types.AssemblerType.DASM ) );
      comboAssemblerType.Items.Add( new GR.Generic.Tupel<string, Types.AssemblerType>( "C64ASM", RetroDevStudio.Types.AssemblerType.C64ASM ) );
      comboAssemblerType.Items.Add( new GR.Generic.Tupel<string, Types.AssemblerType>( "CBMPRGSTUDIO", RetroDevStudio.Types.AssemblerType.CBMPRGSTUDIO ) );
      comboAssemblerType.Items.Add( new GR.Generic.Tupel<string, Types.AssemblerType>( "TASM", RetroDevStudio.Types.AssemblerType.TASM ) );
      comboAssemblerType.Items.Add( new GR.Generic.Tupel<string, Types.AssemblerType>( "Kick Assembler", RetroDevStudio.Types.AssemblerType.KICKASSEMBLER ) );

      SetType( Element.AssemblerType );
    }



    private void SetType( Types.AssemblerType Type )
    {
      for ( int i = 0; i < comboAssemblerType.Items.Count; ++i )
      {
        GR.Generic.Tupel<string, Types.AssemblerType> entry = (GR.Generic.Tupel<string, Types.AssemblerType>)comboAssemblerType.Items[i];
        if ( entry.second == Type )
        {
          comboAssemblerType.SelectedIndex = i;
          break;
        }
      }
    }



    public override void OnClose()
    {
      GR.Generic.Tupel<string, Types.AssemblerType> entry = (GR.Generic.Tupel<string, Types.AssemblerType>)comboAssemblerType.SelectedItem;

      if ( entry == null )
      {
        Element.AssemblerType = RetroDevStudio.Types.AssemblerType.AUTO;
      }
      else
      {
        Element.AssemblerType = entry.second;
      }
    }



    private void btnParseAssembler_Click( DecentForms.ControlBase Sender )
    {
      // fetch text from file
      string  textFromDocument = Core.Searching.GetDocumentInfoText( Element.DocumentInfo );

      Types.AssemblerType type = Parser.ASMFileParser.DetectAssemblerType( textFromDocument );

      SetType( type );
    }

  }
}
