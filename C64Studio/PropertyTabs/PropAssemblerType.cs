using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace C64Studio
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

      comboAssemblerType.Items.Add( new GR.Generic.Tupel<string, Types.AssemblerType>( "Auto", C64Studio.Types.AssemblerType.AUTO ) );
      comboAssemblerType.Items.Add( new GR.Generic.Tupel<string, Types.AssemblerType>( "C64Studio/ACME", C64Studio.Types.AssemblerType.C64_STUDIO ) );
      comboAssemblerType.Items.Add( new GR.Generic.Tupel<string, Types.AssemblerType>( "PDS", C64Studio.Types.AssemblerType.PDS ) );
      comboAssemblerType.Items.Add( new GR.Generic.Tupel<string, Types.AssemblerType>( "DASM", C64Studio.Types.AssemblerType.DASM ) );
      comboAssemblerType.Items.Add( new GR.Generic.Tupel<string, Types.AssemblerType>( "C64ASM", C64Studio.Types.AssemblerType.C64ASM ) );

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
        Element.AssemblerType = C64Studio.Types.AssemblerType.AUTO;
      }
      else
      {
        Element.AssemblerType = entry.second;
      }
    }



    private void btnParseAssembler_Click( object sender, EventArgs e )
    {
      // fetch text from file
      string  textFromDocument = Core.MainForm.GetElementText( Element );

      Types.AssemblerType type = Parser.ASMFileParser.DetectAssemblerType( textFromDocument );

      SetType( type );
    }

  }
}
