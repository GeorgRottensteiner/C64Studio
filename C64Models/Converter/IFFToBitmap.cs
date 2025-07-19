using GR.Collections;
using GR.Image;
using RetroDevStudio;
using System;



namespace RetroDevStudio.Converter
{
  class IFFToBitmap
  {
    public static GR.Memory.ByteBuffer IFFFromBitmap( GR.Image.IImage imageSource, bool optimize = false )
    {
      const uint FORM = 0x4d524f46;
      const uint ILBM = 0x4d424c49;
      const uint BMHD = 0x44484d42;
      const uint CMAP = 0x50414d43;
      const uint BODY = 0x59444f42;


      GR.Memory.ByteBuffer      result = new GR.Memory.ByteBuffer();

      int numPlanes = imageSource.BitsPerPixel;
      if ( optimize )
      {
        if ( numPlanes > 8 )
        {
          // can't optimize
        }
        else
        {
          // determine number of used colors
          var usedPixels = new Set<byte>();
          for ( int i = 0; i < imageSource.Width; i++ )
          {
            for ( int j = 0; j < imageSource.Height; j++ )
            {
              usedPixels.Add( (byte)imageSource.GetPixel( i, j ) );
            }
          }
          // can we save on number of planes?
          int numRequiredPlanes = 0;
          int requiredCount = usedPixels.Count;
          while ( requiredCount > 0 )
          {
            ++numRequiredPlanes;
            requiredCount /= 2;
          }
          --numRequiredPlanes;
          if ( numRequiredPlanes < numPlanes )
          {
            // we can save, need to map image

            // calculate a new palette
            var quant = new RetroDevStudio.Converter.ColorQuantizer( 1 << numRequiredPlanes );

            var tmpImage = new MemoryImage( imageSource.Width, imageSource.Height, GR.Drawing.PixelFormat.Format32bppArgb );
            imageSource.DrawTo( tmpImage, 0, 0 );
            quant.AddSourceToColorCube( tmpImage );
            quant.Calculate();

            var resultingImage = quant.Reduce( imageSource );
            imageSource = resultingImage;
            numPlanes = numRequiredPlanes;
          }
        }
      }

      var totalChunk = new GR.Memory.ByteBuffer();
      totalChunk.AppendU32( FORM );

      var ilbmChunk = new GR.Memory.ByteBuffer();

      ilbmChunk.AppendU32( ILBM );


      var chunkBMHD = new GR.Memory.ByteBuffer();
      chunkBMHD.Reserve( 20 );
      chunkBMHD.AppendU32( BMHD );
      chunkBMHD.AppendU32NetworkOrder( 20 );
      chunkBMHD.AppendU16NetworkOrder( (ushort)imageSource.Width );
      chunkBMHD.AppendU16NetworkOrder( (ushort)imageSource.Height );
      chunkBMHD.AppendU16NetworkOrder( 0 );
      chunkBMHD.AppendU16NetworkOrder( 0 );
      chunkBMHD.AppendU8( (byte)numPlanes );
      chunkBMHD.AppendU8( 0 );                // mask
      chunkBMHD.AppendU8( 0 );                // compression
      chunkBMHD.AppendU8( 0 );                // pad 1
      chunkBMHD.AppendU16NetworkOrder( 0 );   // Transparentcolor
      chunkBMHD.AppendU8( 0 );                // x aspect
      chunkBMHD.AppendU8( 0 );                // y aspect
      chunkBMHD.AppendU16NetworkOrder( 0 );   // page width
      chunkBMHD.AppendU16NetworkOrder( 0 );   // page height

      ilbmChunk.Append( chunkBMHD );


      if ( imageSource.BitsPerPixel <= 8 )
      {
        var chunkCMAP = new GR.Memory.ByteBuffer();

        chunkCMAP.AppendU32( CMAP );
        chunkCMAP.AppendU32NetworkOrder( (uint)( ( 1 << numPlanes ) * 3 ) );
        chunkCMAP.Resize( (uint)( chunkCMAP.Length + ( 1 << numPlanes ) * 3 ) );

        var pal = imageSource.Palette;

        for ( int i = 0; i < pal.NumColors; ++i )
        {
          chunkCMAP.SetU8At( 8 + i * 3 + 0, (byte)( ( pal.ColorValues[i] & 0xff0000 ) >> 16 ) );
          chunkCMAP.SetU8At( 8 + i * 3 + 1, (byte)( ( pal.ColorValues[i] & 0x00ff00 ) >> 8 ) );
          chunkCMAP.SetU8At( 8 + i * 3 + 2, (byte)( ( pal.ColorValues[i] & 0x0000ff ) ) );
        }

        ilbmChunk.Append( chunkCMAP );
      }

      var chunkBody = new GR.Memory.ByteBuffer();

      int   numMasksPerLine = ( imageSource.Width + 15 ) / 16;

      chunkBody.AppendU32( BODY );
      chunkBody.AppendU32NetworkOrder( (uint)( imageSource.Height * numMasksPerLine * 2 * numPlanes ) );

      for ( int y = 0; y < imageSource.Height; ++y )
      {
        for ( int i = 0; i < numPlanes; ++i )
        {
          for ( int b = 0; b < numMasksPerLine; ++b )
          {
            ushort   bitMask = 0;
            for ( int x = 0; x < 16; ++x )
            {
              if ( ( imageSource.GetPixel( b * 16 + x, y ) & ( 1 << i ) ) != 0 )
              {
                bitMask |= (ushort)( 1 << ( 15 - x ) );
              }
            }
            chunkBody.AppendU16NetworkOrder( bitMask );
          }
        }
      }
      ilbmChunk.Append( chunkBody );

      totalChunk.AppendU32NetworkOrder( ilbmChunk.Length );
      totalChunk.Append( ilbmChunk );

      return totalChunk;
    }



    public static GR.Image.MemoryImage BitmapFromIFF( GR.Memory.ByteBuffer imageData )
    {
      const uint FORM = 0x4d524f46;
      const uint ILBM = 0x4d424c49;
      const uint BMHD = 0x44484d42;
      const uint CMAP = 0x50414d43;
      const uint BODY = 0x59444f42;
      const uint CAMG = 0x474d4143;
      const uint SPRT = 0x54525053;
      const uint GRAB = 0x42415247;
      const uint DEST = 0x54534544;
      const uint CRNG = 0x474e5243;
      const uint CCRT = 0x54524343;


      if ( ( imageData.Length < 4 )
      ||   ( imageData.UInt32At( 0 ) != FORM ) )    // FORM
      {
        return null;
      }

      var reader = imageData.MemoryReader();

      reader.Skip( 4 );
      var chunkSize = reader.ReadUInt32NetworkOrder();
      // padding
      if ( ( chunkSize & 1 ) != 0 )
      {
        ++chunkSize;
      }
      if ( reader.Size - reader.Position > chunkSize )
      {
        // IFF - FORM chunk size is greater than actual file size!
        return null;
      }

      long   pos = reader.Position;

      GR.Image.MemoryImage image = null;

      while ( chunkSize > 0 )
      {
        uint      chunkType = reader.ReadUInt32();
        chunkSize -= 4;

        byte      compression = 0;
        byte      numPlanes = 0;

        switch ( chunkType )
        {
          case ILBM:
            {
              uint   innerChunkType = 0;

              while ( ( chunkSize > 0 )
              && ( reader.DataAvailable ) )
              {
                innerChunkType = reader.ReadUInt32();
                uint   origInnerChunkSize = reader.ReadUInt32NetworkOrder();
                uint   innerChunkSize = origInnerChunkSize;
                // padding
                if ( ( innerChunkSize & 1 ) != 0 )
                {
                  ++innerChunkSize;
                }

                long currentInnerPos = reader.Position;
                chunkSize -= 8;

                switch ( innerChunkType )
                {
                  case BMHD:
                    {
                      ushort     width = (ushort)( ( reader.ReadUInt8() << 8 ) + reader.ReadUInt8() );
                      ushort     height = (ushort)( ( reader.ReadUInt8() << 8 ) + reader.ReadUInt8() );
                      short     x = (short)( ( reader.ReadUInt8() << 8 ) + reader.ReadUInt8() );
                      short     y = (short)( ( reader.ReadUInt8() << 8 ) + reader.ReadUInt8() );
                      numPlanes = reader.ReadUInt8();
                      byte      mask = reader.ReadUInt8();
                      compression = reader.ReadUInt8();
                      byte      pad1 = reader.ReadUInt8();
                      ushort     transparentColor = (ushort)( ( reader.ReadUInt8() << 8 ) + reader.ReadUInt8() );
                      byte      xAspect = reader.ReadUInt8();
                      byte      yAspect = reader.ReadUInt8();
                      short     pageWidth = (short)( ( reader.ReadUInt8() << 8 ) + reader.ReadUInt8() );
                      short     pageHeight = (short)( ( reader.ReadUInt8() << 8 ) + reader.ReadUInt8() );

                      // round up to the next highest power of 2
                      uint normalizedPlanes = numPlanes;
                      --normalizedPlanes;
                      normalizedPlanes |= normalizedPlanes >> 1;
                      normalizedPlanes |= normalizedPlanes >> 2;
                      normalizedPlanes |= normalizedPlanes >> 4;
                      normalizedPlanes |= normalizedPlanes >> 8;
                      normalizedPlanes |= normalizedPlanes >> 16;
                      normalizedPlanes++;


                      var pixelFormat = GR.Drawing.PixelFormat.Indexed;
                      switch ( normalizedPlanes )
                      {
                        case 1:
                          pixelFormat = GR.Drawing.PixelFormat.Format1bppIndexed;
                          break;
                        case 2:
                          //pixelFormat = GR.Drawing.PixelFormat.Format2bppIndexed;
                          break;
                        case 4:
                          pixelFormat = GR.Drawing.PixelFormat.Format4bppIndexed;
                          break;
                        case 8:
                          pixelFormat = GR.Drawing.PixelFormat.Format8bppIndexed;
                          break;
                        case 15:
                          pixelFormat = GR.Drawing.PixelFormat.Format16bppRgb555;
                          break;
                        case 16:
                          pixelFormat = GR.Drawing.PixelFormat.Format16bppRgb565;
                          break;
                        case 24:
                          pixelFormat = GR.Drawing.PixelFormat.Format24bppRgb;
                          break;
                        case 32:
                          pixelFormat = GR.Drawing.PixelFormat.Format32bppRgb;
                          break;
                      }
                      image = new GR.Image.MemoryImage( width, height, pixelFormat );
                    }
                    break;
                  case CMAP:
                    if ( image != null )
                    {
                      int   paletteIndex = 0;
                      uint  curSize = origInnerChunkSize;
                      while ( curSize >= 3 )
                      {
                        byte    r = reader.ReadUInt8();
                        byte    g = reader.ReadUInt8();
                        byte    b = reader.ReadUInt8();

                        if ( paletteIndex < (int)image.PaletteEntryCount )
                        {
                          image.SetPaletteColor( paletteIndex, r, g, b );
                        }

                        curSize -= 3;
                        ++paletteIndex;
                      }
                    }
                    break;
                  case BODY:
                    if ( image != null )
                    {
                      if ( compression == 1 )
                      {
                        var compressedData = new GR.Memory.ByteBuffer();
                        compressedData.Reserve( (int)origInnerChunkSize );

                        reader.ReadBlock( compressedData, origInnerChunkSize );

                        var uncompressedData = new GR.Memory.ByteBuffer();
                        uncompressedData.Reserve( 4096 );
                        var memIn = compressedData.MemoryReader();

                        byte    nextByte = 0;
                        do
                        {
                          nextByte = memIn.ReadUInt8();
                          if ( nextByte > 128 )
                          {
                            byte   valueToInsert = memIn.ReadUInt8();
                            for ( int j = 0; j < 257 - nextByte; ++j )
                            {
                              uncompressedData.AppendU8( valueToInsert );
                            }
                          }
                          else if ( nextByte < 128 )
                          {
                            for ( int j = 0; j < nextByte + 1; ++j )
                            {
                              uncompressedData.AppendU8( memIn.ReadUInt8() );
                            }
                          }
                          else
                          {
                            // End of compressed data
                            break;
                          }
                        }
                        while ( ( memIn.Position < memIn.Size )
                        && ( nextByte != 128 ) );

                        var memData = uncompressedData.MemoryReader();

                        int   numMasksPerLine = ( image.Width + 15 ) / 16;

                        for ( int y = 0; y < image.Height; ++y )
                        {
                          for ( int i = 0; i < numPlanes; ++i )
                          {
                            for ( int b = 0; b < numMasksPerLine; ++b )
                            {
                              ushort bitMask = (ushort)( ( memData.ReadUInt8() << 8 ) | memData.ReadUInt8() );
                              for ( int x = 0; x < 16; ++x )
                              {
                                if ( ( bitMask & ( 1 << ( 15 - x ) ) ) != 0 )
                                {
                                  image.SetPixel( b * 16 + x, y, image.GetPixel( b * 16 + x, y ) | (uint)( 1 << i ) );
                                }
                              }
                            }
                          }
                        }
                      }
                      else
                      {
                        int   numMasksPerLine = ( image.Width + 15 ) / 16;
                        for ( int y = 0; y < image.Height; ++y )
                        {
                          for ( int i = 0; i < numPlanes; ++i )
                          {
                            for ( int b = 0; b < numMasksPerLine; ++b )
                            {
                              ushort bitMask = (ushort)( ( reader.ReadUInt8() << 8 ) | reader.ReadUInt8() );
                              for ( int x = 0; x < 16; ++x )
                              {
                                if ( ( bitMask & ( 1 << ( 15 - x ) ) ) != 0 )
                                {
                                  image.SetPixel( b * 16 + x, y, image.GetPixel( b * 16 + x, y ) | (uint)( 1 << i ) );
                                }
                              }
                            }
                          }
                        }
                      }
                    }
                    break;
                  case CAMG:
                  case SPRT:
                  case DEST:
                  case GRAB:
                  case CCRT:
                  case CRNG:
                    // known but ignored
                    break;
                  default:
                    // skip unsupported chunks
                    Debug.Log( $"IFF skip unsupported inner chunk type {innerChunkType.ToString( "X" )}" );
                    break;
                }
                reader.SetPosition( currentInnerPos + innerChunkSize );
                chunkSize -= innerChunkSize;
              }
            }
            break;
          default:
            // skip unsupported chunks
            Debug.Log( $"IFF unsupported chunk type {chunkType.ToString( "X" )}" );
            chunkSize = reader.ReadUInt32NetworkOrder();
            reader.SetPosition( chunkSize );
            break;
        }
      }
      return image;
    }



    public static GR.Image.MemoryImage BitmapFromIFF( string Filename )
    {
      GR.Memory.ByteBuffer byteBuffer = GR.IO.File.ReadAllBytes( Filename );
      return BitmapFromIFF( byteBuffer );
    }



  }
}
