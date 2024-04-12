using System;
using System.Diagnostics;
using System.Linq;
using LibGit2Sharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RetroDevStudio.Types;

namespace TestProject
{
  [TestClass]
  public class TestSourceMapping : TestAssemblerC64Studio
  {
    private void CompareSourceInfo( RetroDevStudio.Types.ASM.FileInfo Info, int GlobalStartLine, int LineCount, int LocalStartLine, string SourceFile )
    {
      Assert.IsTrue( Info.SourceInfo.ContainsKey( GlobalStartLine - 1 ) );

      var infoEntry = Info.SourceInfo[GlobalStartLine - 1];

      Assert.AreEqual( LineCount, infoEntry.LineCount );
      Assert.AreEqual( LocalStartLine - 1, infoEntry.LocalStartLine );
      Assert.AreEqual( SourceFile, infoEntry.Filename );
    }




    [TestMethod]
    [TestCategory( "SourceMapping" )]
    public void TestAssemblyNestedLoops()
    {
      string source = @"*=$0801

      !for i = 0 to 1
        ;Langer Kommentar
        ;Langer Kommentar
        ;Langer Kommentar
        ;Langer Kommentar
        ;Langer Kommentar
        ;Langer Kommentar
        ;Langer Kommentar
        ;Langer Kommentar
        ;Langer Kommentar
        ;Langer Kommentar
        ;Langer Kommentar
        ;Langer Kommentar
      !end

      tables
      !for j = 0 to 1
        !byte 0
        !for i = 0 to 10
          !byte 0
        !end
        !align 255,0
      !end
      !byte 0,0,0,0


        lda #1
        lda #2
        lda #3






      nop
      nop
      nop
      nop
      nop
      nop
      nop
      nop";

      var assembly = TestAssembleC64Studio( source, out RetroDevStudio.Types.ASM.FileInfo info );

      Assert.AreEqual( "01080000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000A901A902A903EAEAEAEAEAEAEAEA", assembly.ToString() );

      Assert.AreEqual( 27, info.SourceInfo.Count );
      CompareSourceInfo( info, 1, 15, 1, "" );
      CompareSourceInfo( info, 16, 12, 4, "" );
      CompareSourceInfo( info, 28, 7, 16, "" );
      CompareSourceInfo( info, 35, 1, 22, "" );
      CompareSourceInfo( info, 36, 1, 22, "" );
      CompareSourceInfo( info, 37, 1, 22, "" );
      CompareSourceInfo( info, 38, 1, 22, "" );
      CompareSourceInfo( info, 39, 1, 22, "" );
      CompareSourceInfo( info, 40, 1, 22, "" );
      CompareSourceInfo( info, 41, 1, 22, "" );
      CompareSourceInfo( info, 42, 1, 22, "" );
      CompareSourceInfo( info, 43, 1, 22, "" );
      CompareSourceInfo( info, 44, 1, 22, "" );
      CompareSourceInfo( info, 45, 2, 23, "" );
      CompareSourceInfo( info, 47, 3, 20, "" );
      CompareSourceInfo( info, 50, 1, 22, "" );
      CompareSourceInfo( info, 51, 1, 22, "" );
      CompareSourceInfo( info, 52, 1, 22, "" );
      CompareSourceInfo( info, 53, 1, 22, "" );
      CompareSourceInfo( info, 54, 1, 22, "" );
      CompareSourceInfo( info, 55, 1, 22, "" );
      CompareSourceInfo( info, 56, 1, 22, "" );
      CompareSourceInfo( info, 57, 1, 22, "" );
      CompareSourceInfo( info, 58, 1, 22, "" );
      CompareSourceInfo( info, 59, 1, 22, "" );
      CompareSourceInfo( info, 60, 2, 23, "" );
      CompareSourceInfo( info, 62, 21, 25, "" );
    }



    [TestMethod]
    public void TestNestedLoops()
    {
      string      source = @"ColorRAM      = $d800
                              Map.ColorMap  = $c000
                              Map.MapWidth  = 40

                              * = $2000
                              !for r = 0 to 1 ;11
                              !for c = 0 to 1 ;38
                              lda Map.ColorMap + c + r* Map.MapWidth, x
                              sta ColorRAM + 40 + c + r* 40
                              !end
                              !end";

      var assembly = TestAssembleC64Studio( source, out RetroDevStudio.Types.ASM.FileInfo info );
      Assert.AreEqual( "0020BD00C08D28D8BD01C08D29D8BD28C08D50D8BD29C08D51D8", assembly.ToString() );

      Assert.AreEqual( 7, info.SourceInfo.Count );

      CompareSourceInfo( info, 1, 9, 1, "" );     // up to sta
      CompareSourceInfo( info, 10, 2, 8, "" );    // c loop content
      CompareSourceInfo( info, 12, 1, 10, "" );   // c loop end
      CompareSourceInfo( info, 13, 3, 7, "" );    // c loop re-start (c=1)
      CompareSourceInfo( info, 16, 2, 8, "" );    // c loop content
      CompareSourceInfo( info, 18, 1, 10, "" );   // c loop end
      CompareSourceInfo( info, 19, 1, 11, "" );   // rest of file
    }



  }
}
