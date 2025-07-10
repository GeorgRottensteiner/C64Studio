using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using GR.Memory;
using LibGit2Sharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RetroDevStudio.Formats;
using RetroDevStudio.Types;



namespace TestProject
{
  [TestClass]
  public class TestFormatD81
  {
    [TestMethod]
    public void TestAddFileAndDelete()
    {
      var filename = new ByteBuffer( "414243" );
      var fileContent = new ByteBuffer( "0123456789ABCDEF" );

      var disk = new D81();
      disk.CreateEmptyMedia();

      var emptyDisk = disk.Compile();
      disk.WriteFile( filename, fileContent, FileTypeNative.COMMODORE_PRG );

      var diskWithFile = disk.Compile();
      disk.DeleteFile( filename, true );

      var diskWithDeletedFile = disk.Compile();
      BinaryCompare( emptyDisk, diskWithDeletedFile );
    }



    [TestMethod]
    public void TestAddFileAndDeleteEnhanced()
    {
      var filename = new ByteBuffer( "41424331" );
      var filename2 = new ByteBuffer( "41424332" );
      var filename3 = new ByteBuffer( "41424333" );
      var fileContent = new ByteBuffer( "0123456789ABCDEF" );
      var fileContent2 = new ByteBuffer( "FFEEDDCCBBAA99887766554433221100" );
      var fileContent3 = new ByteBuffer( "CAFE" );

      var disk = new D81();
      disk.CreateEmptyMedia();

      var emptyDisk = disk.Compile();
      disk.WriteFile( filename, fileContent, FileTypeNative.COMMODORE_PRG );
      disk.WriteFile( filename2, fileContent2, FileTypeNative.COMMODORE_PRG );
      disk.WriteFile( filename3, fileContent3, FileTypeNative.COMMODORE_PRG );

      var diskWithFile = disk.Compile();
      disk.DeleteFile( filename3, true );
      disk.DeleteFile( filename2, true );
      disk.DeleteFile( filename, true );

      var diskWithDeletedFile = disk.Compile();
      BinaryCompare( emptyDisk, diskWithDeletedFile );
    }



    [TestMethod]
    public void TestAddAndReadFile()
    {
      var filename = new ByteBuffer( "414243" );
      var fileContent = new ByteBuffer( "0123456789ABCDEF" );

      var disk = new D81();
      disk.CreateEmptyMedia();

      disk.WriteFile( filename, fileContent, FileTypeNative.COMMODORE_PRG );
      var file = disk.LoadFile( filename );
      Assert.IsNotNull( file, "File was not found on disk!" );
      Assert.AreEqual( filename + new ByteBuffer( 13, 0xa0 ), file.Filename );
      BinaryCompare( fileContent, file.Data );
    }



    public static void BinaryCompare( ByteBuffer Block1, ByteBuffer Block2 )
    {
      if ( Block1.Length != Block2.Length )
      {
        Assert.Fail( $"Block sizes does not match {Block1.Length} != {Block2.Length}" );
      }

      bool  hadDifference = false;
      List<ulong> diffs = new List<ulong>();
      for ( int i = 0; i < Block1.Length; ++i )
      {
        var byte1 = Block1.ByteAt( i );
        var byte2 = Block2.ByteAt( i );

        if ( byte1 != byte2 )
        {
          hadDifference = true;
          diffs.Add( (ulong)( (ulong)i + ( (ulong)byte1 << 32 ) + ( (ulong)byte2 << 40 ) ) );
          if ( diffs.Count > 100 )
          {
            ListDiffs( diffs );
          }
        }
      }
      if ( hadDifference )
      {
        ListDiffs( diffs );
      }
    }



    public static void BinaryDump( string Description, ByteBuffer Block )
    {
      var sb = new StringBuilder();

      sb.Append( "Hex Dump of " );
      sb.AppendLine( Description );

      for ( int i = 0; i < Block.Length; ++i )
      {
        if ( ( i % 16 ) == 0 )
        {
          sb.Append( i.ToString( "X8" ) );
          sb.Append( ':' );
        }

        sb.Append( Block.ByteAt( i ).ToString( "X2" ) );
        if ( ( i % 16 ) != 15 )
        {
          sb.Append( ' ' );
        }
        else
        {
          System.Diagnostics.Debug.WriteLine( sb.ToString() );
          sb.Clear();
        }
      }
    }



    private static void ListDiffs( List<ulong> Diffs )
    {
      var sb = new StringBuilder();

      sb.AppendLine();
      foreach ( ulong diff in Diffs )
      {
        sb.Append( "Difference at $" );
        sb.Append( ( diff & 0xffffffff ).ToString( "X8" ) );
        sb.Append( ": Expected $" );
        sb.Append( ( ( diff >> 32 ) & 0xff ).ToString( "X2" ) );
        sb.Append( " != got $" );
        sb.AppendLine( ( ( diff >> 40 ) & 0xff ).ToString( "X2" ) );
      }
      Assert.Fail( sb.ToString() );
    }



  }
}
