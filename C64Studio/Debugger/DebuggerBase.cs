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

    protected volatile bool               m_ShuttingDown = false;



    public bool ShuttingDown
    {
      get
      {
        return m_ShuttingDown;
      }
    }



    public DebuggerBase( StudioCore Core )
    {
      this.Core = Core;
    }




    public void SetAutoRefreshMemory( List<MemoryRefreshSection> Sections )
    {
      m_LastRefreshSections = Sections;
    }



  }
}
