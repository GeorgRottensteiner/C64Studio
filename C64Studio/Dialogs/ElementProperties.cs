using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace C64Studio
{
  public partial class ElementProperties : Form
  {
    private ProjectElement    m_Element = null;
    private StudioCore        m_Core = null;



    public ElementProperties( StudioCore Core, ProjectElement Element )
    {
      m_Element   = Element;
      m_Core      = Core;
      InitializeComponent();

      AddElementTabs();

      GR.Image.DPIHandler.ResizeControlsForDPI( this );
      Core.Theming.ApplyTheme( this );
    }



    private void AddTab( Form TabForm )
    {
      TabPage   page = new TabPage();
      page.Controls.Add( TabForm );
      page.Text = page.Controls[0].Text;
      page.Controls[0].Visible = true;
      page.Tag = TabForm;
      tabElementProperties.TabPages.Add( page );
    }



    private void AddElementTabs()
    {
      AddTab( new PropGeneral( m_Element, m_Core ) );
      switch ( m_Element.DocumentInfo.Type )
      {
        case ProjectElement.ElementType.ASM_SOURCE:
          AddTab( new PropAssemblerType( m_Element, m_Core ) );
          AddTab( new PropCompileTarget( m_Element, m_Core ) );
          AddTab( new PropDebugging( m_Element, m_Core ) );
          break;
        case ProjectElement.ElementType.BASIC_SOURCE:
          AddTab( new PropCompileTarget( m_Element, m_Core ) );
          AddTab( new PropDebugging( m_Element, m_Core ) );
          break;
      }
      AddTab( new PropBuildEvents( m_Element, m_Core ) );
    }



    private void btnClose_Click( object sender, EventArgs e )
    {
      foreach ( TabPage tab in tabElementProperties.TabPages )
      {
        PropertyTabs.PropertyTabBase tabBase = (PropertyTabs.PropertyTabBase)tab.Tag;

        if ( tabBase != null )
        {
          tabBase.OnClose();
        }
      }
      Close();
    }

  }
}
