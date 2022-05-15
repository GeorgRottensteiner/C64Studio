using GR.Collections;
using GR.IO;
using GR.Memory;
using System;
using System.Collections.Generic;
using WeifenLuo.WinFormsUI.Docking;

namespace RetroDevStudio
{
  public class PerspectiveDetails
  {
    private StudioCore                          Core;
    private Map<Perspective,List<string>>       ActiveContents = new Map<Perspective, List<string>>();



    public PerspectiveDetails( StudioCore Core )
    {
      this.Core = Core;
    }



    public void StoreActiveContent( Perspective ActivePerspective )
    {
      if ( !ActiveContents.ContainsKey( ActivePerspective ) )
      {
        ActiveContents.Add( ActivePerspective, new List<string>() );
      }
      ActiveContents[ActivePerspective].Clear();

      var activeContents = new List<int>();
      foreach ( DockPane pane in Core.MainForm.panelMain.Panes )
      {
        var activeContentID = Core.MainForm.panelMain.Contents.IndexOf( pane.ActiveContent );
        var cnt = pane.Contents.Count;

        if ( ( cnt > 1 )
        &&   ( activeContentID >= 0 ) )
        {
          ActiveContents[ActivePerspective].Add( pane.ActiveContent.ToString() );
        }
      }
    }



    public void RestoreActiveContent( Perspective ActivePerspective )
    {
      if ( !ActiveContents.ContainsKey( ActivePerspective ) )
      {
        return;
      }

      foreach ( var activeContent in ActiveContents[ActivePerspective] )
      {
        foreach ( var content in Core.MainForm.panelMain.Contents )
        {
          if ( content.ToString() == activeContent )
          {
            var handler = content.DockHandler;
            handler.IsHidden = false;
            handler.Show();
            break;
          }
        }
      }

    }



    public ByteBuffer ToBuffer()
    {
      var result = new ByteBuffer();

      if ( ActiveContents.ContainsKey( Perspective.EDIT ) )
      {
        result.AppendI32( ActiveContents[Perspective.EDIT].Count );

        foreach ( var content in ActiveContents[Perspective.EDIT] )
        {
          result.AppendString( content );
        }
      }
      else
      {
        result.AppendI32( 0 );
      }

      if ( ActiveContents.ContainsKey( Perspective.DEBUG ) )
      {
        result.AppendI32( ActiveContents[Perspective.DEBUG].Count );

        foreach ( var content in ActiveContents[Perspective.DEBUG] )
        {
          result.AppendString( content );
        }
      }
      else
      {
        result.AppendI32( 0 );
      }

      return result;
    }



    public void ReadFromBuffer( IReader BinIn )
    {
      ActiveContents.Clear();
      ActiveContents.Add( Perspective.EDIT, new List<string>() );
      ActiveContents.Add( Perspective.DEBUG, new List<string>() );

      int     numContents = BinIn.ReadInt32();
      for ( int i = 0; i < numContents; ++i )
      {
        ActiveContents[Perspective.EDIT].Add( BinIn.ReadString() );
      }

      numContents = BinIn.ReadInt32();
      for ( int i = 0; i < numContents; ++i )
      {
        ActiveContents[Perspective.DEBUG].Add( BinIn.ReadString() );
      }
    }



  }
}