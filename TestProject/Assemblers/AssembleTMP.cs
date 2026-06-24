using System;
using System.Diagnostics;
using System.Linq;
using LibGit2Sharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RetroDevStudio.Types;

namespace TestProject
{
  [TestClass]
  public partial class TestAssemblerTMP : TestAssemblerBase
  {
    [TestMethod]
    /// TMP does not have an operator precedence, so all operators are evaluated left to right
    public void TestOperatorPrecedence()
    {
      string      source = @"SHALL_BE_33 = 2 *$10 + 1
                            SHALL_BE_48 = 1 + 2 *$10";

      TestAssemble( AssemblerType.TURBO_MACRO_PRO, source, out RetroDevStudio.Types.ASM.FileInfo info );

      Assert.AreEqual( 33, info.Labels["SHALL_BE_33"].AddressOrValue );
      Assert.AreEqual( 48, info.Labels["SHALL_BE_48"].AddressOrValue );
    }



    [TestMethod]
    public void TestOperatorHiLoByte()
    {
      string      source = @"* = $1000
                            .byte >$0314         ; = hex $03
                            .byte <$0314         ; = hex $14
                            .byte <$0400+(6*$28) ; = hex $f0
                            .byte <($0456+6)*$13 ; = hex $d4";

      var data = TestAssemble( AssemblerType.TURBO_MACRO_PRO, source, out RetroDevStudio.Types.ASM.FileInfo info );

      Assert.AreEqual( "00100314F0D4", data.ToString() );
    }



    [TestMethod]
    public void TestCurrentPosition()
    {
      string      source = @"*= $1000     ;sets the program counter to $1000
                             jmp *+$03    ;assembles as jmp $1003
                             jmp *        ;also assembles as jmp $1003 because the program counter
                                          ;at this line will actually be $1003.";

      var data = TestAssemble( AssemblerType.TURBO_MACRO_PRO, source, out RetroDevStudio.Types.ASM.FileInfo info );

      Assert.AreEqual( "00104C03104C0310", data.ToString() );
    }



    [TestMethod]
    [DataRow( "tmp/include_macro.asm", "a2fda9519d0004a90f9d00d8a2fea9589d0004a90e9d00d8a2ffa95a9d0004a90d9d00d860" )]
    [DataRow( "tmp/bytewordrta.asm", "1941cce2fce1fc" )]
    [DataRow( "tmp/block.asm", "02ff02" )]
    public void AssembleTMP( string sourceFile, string expectedData )
    {
      var result = TestAssemblerBase.AssembleFromFile( AssemblerType.TURBO_MACRO_PRO, sourceFile, 
              out GR.Collections.MultiMap<int, RetroDevStudio.Parser.ParserBase.ParseMessage> messages, 
              out RetroDevStudio.Types.ASM.FileInfo info );

      Assert.AreEqual( expectedData.ToUpper(), result.ToString() );
    }



  }
}
