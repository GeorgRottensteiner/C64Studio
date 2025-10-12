using GR.Collections;
using GR.IO;
using GR.Memory;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace RetroDevStudio
{
  public class AutoFormatSettings
  {
    public int      NumTabsIndentation = 5;
    public bool     IndentPseudoOpsLikeCode = true;
    public bool     PutLabelsOnSeparateLine = true;



    public ByteBuffer ToBuffer()
    {
      var result = new ByteBuffer();

      GR.IO.FileChunk   chunkDetails = new GR.IO.FileChunk( FileChunkConstants.SETTINGS_CODE_FORMATTING );

      chunkDetails.AppendI32( NumTabsIndentation );
      chunkDetails.AppendI32( IndentPseudoOpsLikeCode ? 1 : 0 );
      chunkDetails.AppendI32( PutLabelsOnSeparateLine ? 1 : 0 );

      return chunkDetails.ToBuffer();
    }



    public void SetDefault()
    {
      NumTabsIndentation      = 5;
      IndentPseudoOpsLikeCode = true;
      PutLabelsOnSeparateLine = true;
    }



    public void ReadFromBuffer( IReader binIn )
    {
      NumTabsIndentation      = binIn.ReadInt32(); 
      IndentPseudoOpsLikeCode = ( binIn.ReadInt32() != 0 );
      PutLabelsOnSeparateLine = ( binIn.ReadInt32() != 0 );
    }



  }
}