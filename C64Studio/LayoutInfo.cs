using RetroDevStudioModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio
{
  public class LayoutInfo
  {
    public string                 Name        = "";
    public BaseDocument           Document = null;

    // dock panel suite specific
    public System.Drawing.Point   Position = new System.Drawing.Point();
    public System.Drawing.Size    Size = new System.Drawing.Size();
    public bool                   Hidden = false;
    public WeifenLuo.WinFormsUI.Docking.DockState DockState = WeifenLuo.WinFormsUI.Docking.DockState.Unknown;



    public GR.Memory.ByteBuffer ToBuffer()
    {
      GR.IO.FileChunk   chunkLayout = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_LAYOUT );

      chunkLayout.AppendString( Name );
      chunkLayout.AppendI32( Position.X );
      chunkLayout.AppendI32( Position.Y );
      chunkLayout.AppendI32( Size.Width );
      chunkLayout.AppendI32( Size.Height );
      chunkLayout.AppendI32( (int)DockState );

      return chunkLayout.ToBuffer();
    }



    public void FromChunk( GR.IO.FileChunk Chunk )
    {
      GR.IO.MemoryReader memIn = Chunk.MemoryReader();

      Name = memIn.ReadString();
      Position.X = memIn.ReadInt32();
      Position.Y = memIn.ReadInt32();
      Size.Width = memIn.ReadInt32();
      Size.Height = memIn.ReadInt32();
      DockState = (WeifenLuo.WinFormsUI.Docking.DockState)memIn.ReadInt32();
    }



    public void StoreLayout()
    {
      if ( Document == null )
      {
        Hidden = true;
        return;
      }
      Hidden = false;
      DockState = Document.DockState;
      if ( !Document.Visible )
      {
        return;
      }
      if ( Document.ParentForm == null )
      {
        Position = Document.Location;
      }
      else
      {
        Position = Document.ParentForm.PointToClient( Document.Location );
      }
      Size.Width = Document.Size.Width;
      Size.Height = Document.Size.Height;
    }



    public void RestoreLayout()
    {
      if ( Document == null )
      {
        return;
      }
      /*
      Document.Location = new System.Drawing.Point( Position.X, Position.Y );
      Document.Size = new System.Drawing.Size( Size.Width, Size.Height );
      switch ( DockState )
      {
        case WeifenLuo.WinFormsUI.Docking.DockState.Float:
          Document.FloatAt( new System.Drawing.Rectangle( Position, Size ) );
          Document.DockState = DockState;
          break;
        default:
          Document.DockState = DockState;
          break;
      }
      // TODO - set parent?
       */
    }




  }
}
