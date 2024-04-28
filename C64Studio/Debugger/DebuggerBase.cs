using RetroDevStudio.Types;
using GR.Memory;
using RetroDevStudio;
using System;
using System.Collections.Generic;
using System.Text;
using RetroDevStudio.Documents;
using GR.Collections;

namespace RetroDevStudio
{
  public class DebuggerBase
  {
    protected List<MemoryRefreshSection>  m_LastRefreshSections = new List<MemoryRefreshSection>();

    protected StudioCore                  Core = null;



    public DebuggerBase( StudioCore Core )
    {
      this.Core = Core;
    }




    public void SetAutoRefreshMemory( List<MemoryRefreshSection> Sections )
    {
      m_LastRefreshSections = Sections;
    }



    /*
    public void RefreshMemorySections()
    {
      foreach ( var section in m_LastRefreshSections )
      {
        RefreshMemory( section.StartAddress, section.Size, section.Source );
      }
    }*/



  }
}
