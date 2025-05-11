using System;
using System.Collections.Generic;
using System.Text;

namespace RetroDevStudio.Tasks
{
  public class TaskDebugRunTo : Task
  {
    private DocumentInfo    m_DocToHandle;
    private DocumentInfo    m_DocToDebug;
    private DocumentInfo    m_DocActive;



    public TaskDebugRunTo( DocumentInfo DocToHandle, DocumentInfo DocToDebug, DocumentInfo DocActive )
    {
      m_DocToHandle = DocToHandle;
      m_DocToDebug = DocToDebug;
      m_DocActive = DocActive;
    }



    protected override bool ProcessTask()
    {
      Types.ASM.FileInfo debugFileInfo        = Core.Navigating.DetermineASMFileInfo( m_DocToHandle );
      Types.ASM.FileInfo localDebugFileInfo   = Core.Navigating.DetermineLocalASMFileInfo( m_DocToDebug );
      Types.ASM.FileInfo localDebugFileInfo2  = Core.Navigating.DetermineLocalASMFileInfo( m_DocActive );

      int           lineIndex = -1;

      if ( debugFileInfo.FindGlobalLineIndex( m_DocActive.BaseDoc.CurrentLineIndex, m_DocActive.FullPath, out lineIndex ) )
      {
        int targetAddress = debugFileInfo.FindLineAddress( lineIndex );
        if ( targetAddress != -1 )
        {
          RunToAddress( m_DocToDebug, m_DocToHandle, targetAddress );
        }
        else
        {
          Core.Notification.MessageBox( "No reachable code detected", "No reachable code was detected in this line (or could not assemble)" );
        }
      }
      else if ( localDebugFileInfo.FindGlobalLineIndex( m_DocActive.BaseDoc.CurrentLineIndex, m_DocActive.FullPath, out lineIndex ) )
      {
        // retry at local debug info
        int targetAddress = localDebugFileInfo.FindLineAddress( lineIndex );
        if ( targetAddress != -1 )
        {
          RunToAddress( m_DocToDebug, m_DocToHandle, targetAddress );
        }
        else
        {
          Core.Notification.MessageBox( "No reachable code detected", "No reachable code was detected in this line (or could not assemble)" );
        }
      }
      else if ( localDebugFileInfo2.FindGlobalLineIndex( m_DocActive.BaseDoc.CurrentLineIndex, m_DocActive.FullPath, out lineIndex ) )
      {
        // retry at local debug info
        int targetAddress = localDebugFileInfo2.FindLineAddress( lineIndex );
        if ( targetAddress != -1 )
        {
          RunToAddress( m_DocToDebug, m_DocToHandle, targetAddress );
        }
        else
        {
          Core.Notification.MessageBox( "No reachable code detected", "No reachable code was detected in this line (or could not assemble)" );
        }
      }
      else
      {
        Core.Notification.MessageBox( "No reachable code detected", "No reachable code was detected in this line (or could not assemble)" );
      }
      return true;
    }



    private void RunToAddress( DocumentInfo m_DocToDebug, DocumentInfo m_DocToHandle, int targetAddress )
    {
      /*
      if ( ( Core.Debugging.Debugger.SupportsFeature( DebuggerFeature.REQUIRES_DOUBLE_ACTION_AFTER_BREAK ) )
      &&   ( Core.Debugging.FirstActionAfterBreak ) )
      {
        Core.Debugging.FirstActionAfterBreak = false;
        Core.MainForm.RunToAddress( m_DocToDebug, m_DocToHandle, targetAddress );
      }*/
      Core.MainForm.RunToAddress( m_DocToDebug, m_DocToHandle, targetAddress );
    }



  }
}
