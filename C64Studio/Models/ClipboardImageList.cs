using RetroDevStudio;
using RetroDevStudio.Types;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RetroDevStudio.Types
{
  public class ClipboardImageList
  {
    public class Entry
    {
      public int          Index = 0;
      public GraphicTile  Tile = new GraphicTile();
    }


    public bool             ColumnBased = false;
    public GraphicTileMode  Mode = GraphicTileMode.COMMODORE_HIRES;
    public ColorSettings    Colors = new ColorSettings();
    public List<Entry>      Entries = new List<Entry>();



    public bool CopyToClipboard()
    {
      if ( Entries.Count == 0 )
      {
        return false;
      }

      GR.Memory.ByteBuffer dataSelection = new GR.Memory.ByteBuffer();

      dataSelection.AppendI32( Entries.Count );
      dataSelection.AppendI32( ColumnBased ? 1 : 0 );

      dataSelection.AppendI32( Colors.BackgroundColor );
      dataSelection.AppendI32( Colors.MultiColor1 );
      dataSelection.AppendI32( Colors.MultiColor2 );
      dataSelection.AppendI32( Colors.BGColor4 );

      dataSelection.AppendI32( Colors.Palettes.Count );
      for ( int j = 0; j < Colors.Palettes.Count; ++j )
      {
        var pal = Colors.Palettes[j];
        dataSelection.AppendI32( pal.NumColors );
        for ( int i = 0; i < pal.NumColors; ++i )
        {
          dataSelection.AppendU32( pal.ColorValues[i] );
        }
      }

      int prevIndex = Entries[0].Index;
      foreach ( var entry in Entries )
      {
        // delta in indices
        dataSelection.AppendI32( entry.Index - prevIndex );
        prevIndex = entry.Index;

        dataSelection.AppendI32( (int)entry.Tile.Mode );
        dataSelection.AppendI32( entry.Tile.CustomColor );
        dataSelection.AppendI32( entry.Tile.Colors.ActivePalette );
        dataSelection.AppendI32( entry.Tile.Width );
        dataSelection.AppendI32( entry.Tile.Height );
        dataSelection.AppendU32( entry.Tile.Data.Length );
        dataSelection.Append( entry.Tile.Data );
        dataSelection.AppendI32( entry.Index );
      }

      DataObject dataObj = new DataObject();

      dataObj.SetData( "RetroDevStudio.ImageList", false, dataSelection.MemoryStream() );


      // add as one image
      var fullImage = new GR.Image.MemoryImage( Entries.Count * Entries[0].Tile.Width, Entries[0].Tile.Height, GR.Drawing.PixelFormat.Format32bppRgb );
      int curX = 0;
      foreach ( var entry in Entries )
      {
        entry.Tile.Image.DrawTo( fullImage, curX, 0 );
        curX += entry.Tile.Width;
      }

      GR.Memory.ByteBuffer      dibData2 = fullImage.CreateHDIBAsBuffer();

      System.IO.MemoryStream    ms2 = dibData2.MemoryStream();
      dataObj.SetData( "DeviceIndependentBitmap", ms2 );

      Clipboard.SetDataObject( dataObj, true );
      return true;
    }



    public bool GetFromClipboard()
    {
      IDataObject dataObj = Clipboard.GetDataObject();
      if ( dataObj == null )
      {
        return false;
      }
      if ( !dataObj.GetDataPresent( "RetroDevStudio.ImageList" ) )
      {
        return false; 
      }
      System.IO.MemoryStream ms = (System.IO.MemoryStream)dataObj.GetData( "RetroDevStudio.ImageList" );

      GR.Memory.ByteBuffer spriteData = new GR.Memory.ByteBuffer( (uint)ms.Length );
      ms.Read( spriteData.Data(), 0, (int)ms.Length );
      GR.IO.MemoryReader memIn = spriteData.MemoryReader();

      int numEntries = memIn.ReadInt32();
      ColumnBased = ( memIn.ReadInt32() > 0 ) ? true : false;

      var incomingColorSettings = new ColorSettings();

      incomingColorSettings.BackgroundColor = memIn.ReadInt32();
      incomingColorSettings.MultiColor1 = memIn.ReadInt32();
      incomingColorSettings.MultiColor2 = memIn.ReadInt32();
      incomingColorSettings.BGColor4 = memIn.ReadInt32();

      incomingColorSettings.Palettes.Clear();
      int numPalettes = memIn.ReadInt32();
      for ( int j = 0; j < numPalettes; ++j )
      {
        int numPaletteEntries = memIn.ReadInt32();
        var pal = new Palette( numPaletteEntries );
        for ( int i = 0; i < numPaletteEntries; ++i )
        {
          pal.ColorValues[i] = memIn.ReadUInt32();
        }
        pal.CreateBrushes();
        incomingColorSettings.Palettes.Add( pal );
      }

      for ( int i = 0; i < numEntries; ++i )
      {
        var entry = new Entry();

        entry.Index = memIn.ReadInt32();

        entry.Tile.Mode                 = (GraphicTileMode)memIn.ReadInt32();
        entry.Tile.CustomColor          = (byte)memIn.ReadInt32();
        int palIndex = memIn.ReadInt32();
        entry.Tile.Width                = memIn.ReadInt32();
        entry.Tile.Height               = memIn.ReadInt32();
        uint dataLength                 = memIn.ReadUInt32();
        entry.Tile.Data                 = new GR.Memory.ByteBuffer();
        memIn.ReadBlock( entry.Tile.Data, dataLength );

        entry.Tile.Colors = new ColorSettings( incomingColorSettings );
        entry.Tile.Colors.ActivePalette = palIndex;

        int originalIndex = memIn.ReadInt32();

        Entries.Add( entry );
      }
      return true;
    }



  }
}
