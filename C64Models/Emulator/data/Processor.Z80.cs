using System;
using System.Collections.Generic;
using System.Text;
using static Tiny64.Opcode;

namespace Tiny64
{
  public partial class Processor
  {
    public static Processor CreateZ80()
    {
      var  sys = new Processor( "Z80" );

      // r Reg. 
      //  000 B
      //  001 C
      //  010 D
      //  011 E
      //  100 H
      //  101 L
      //  111 A

      // dd,ss is any of the register pairs BC, DE, HL, SP
      // qq is any of the register pairs AF, BC, DE, HL
      // pp is any of the register pairs BC, DE, IX, SP 
      // rr is any of the register pairs BC, DE, IY, SP

      // b Bit Tested
      //  000 0
      //  001 1
      //  010 2
      //  011 3
      //  100 4
      //  101 5
      //  110 6
      //  111 7

      // cc Condition
      //   000 NZ none zero
      //   001 Z  zero
      //   010 NC non carry
      //   011 C  carry
      //   100 PO parity odd
      //   101 PE parity even
      //   110 P  sign positive
      //   111 M  sign negative

      // 8bit group
      sys.AddOpcode( "ld", 0x40, 0, AddressingType.REGISTER_TO_REGISTER, 4 );           // LD r,r'
      sys.AddOpcode( "ld", 0x06, 1, AddressingType.IMMEDIATE_TO_REGISTER, 7 );          // LD r,n
      sys.AddOpcode( "ld", 0x46, 1, AddressingType.HL_INDIRECT_TO_REGISTER, 7 );        // LD r,(HL)
      sys.AddOpcode( "ld", 0xdd, 2, AddressingType.IX_D_INDIRECT_TO_REGISTER, 19 );     // LD r,(IX+d)
      sys.AddOpcode( "ld", 0xfd, 2, AddressingType.IY_D_INDIRECT_TO_REGISTER, 19 );     // LD r,(IY+d)
      sys.AddOpcode( "ld", 0x70, 0, AddressingType.REGISTER_TO_HL_INDIRECT, 7 );        // LD (HL),r
      sys.AddOpcode( "ld", 0xdd, 2, AddressingType.REGISTER_TO_IX_D_INDIRECT, 19 );     // LD (IX+d),r
      sys.AddOpcode( "ld", 0xfd, 2, AddressingType.REGISTER_TO_IY_D_INDIRECT, 19 );     // LD (IY+d),r
      sys.AddOpcode( "ld", 0x36, 1, AddressingType.IMMEDIATE_TO_HL_INDIRECT, 10 );      // LD (HL),n
      sys.AddOpcode( "ld", 0xdd36, 2, AddressingType.IMMEDIATE_TO_IX_D_INDIRECT, 19 );  // LD (IX+d),n
      sys.AddOpcode( "ld", 0xfd36, 2, AddressingType.IMMEDIATE_TO_IY_D_INDIRECT, 19 );  // LD (IY+d),n
      sys.AddOpcode( "ld", 0x0a, 0, AddressingType.BC_INDIRECT_TO_A, 7 );               // LD A,(BC)
      sys.AddOpcode( "ld", 0x1a, 0, AddressingType.DE_INDIRECT_TO_A, 7 );               // LD A,(DE)
      sys.AddOpcode( "ld", 0x3a, 2, AddressingType.IMMEDIATE_INDIRECT_TO_A, 13 );       // LD A,(nn)
      sys.AddOpcode( "ld", 0x02, 0, AddressingType.A_TO_BC_INDIRECT, 7 );               // LD (BC),A
      sys.AddOpcode( "ld", 0x12, 0, AddressingType.A_TO_DE_INDIRECT, 7 );               // LD (DE),A
      sys.AddOpcode( "ld", 0x32, 2, AddressingType.A_TO_IMMEDIATE_INDIRECT, 13 );       // LD (nn),A
      sys.AddOpcode( "ld", 0xed57, 0, AddressingType.I_TO_A, 9 );                       // LD A,I
      sys.AddOpcode( "ld", 0xed5f, 0, AddressingType.R_TO_A, 9 );                       // LD A,R
      sys.AddOpcode( "ld", 0xed47, 0, AddressingType.A_TO_I, 9 );                       // LD I,A
      sys.AddOpcode( "ld", 0xed4f, 0, AddressingType.A_TO_R, 9 );                       // LD R,A

      // 16bit group
      sys.AddOpcode( "ld", 0x01, 2, AddressingType.IMMEDIATE_TO_REGISTER_DD, 10 );      // LD dd,nn
      sys.AddOpcode( "ld", 0xdd21, 2, AddressingType.IMMEDIATE_TO_IX, 14 );             // LD IX,nn
      sys.AddOpcode( "ld", 0xfd21, 2, AddressingType.IMMEDIATE_TO_IY, 14 );             // LD IY,nn
      sys.AddOpcode( "ld", 0x2a, 2, AddressingType.IMMEDIATE_INDIRECT_TO_HL, 16 );      // LD HL,(nn)
      sys.AddOpcode( "ld", 0xed4b, 2, AddressingType.IMMEDIATE_INDIRECT_TO_REGISTER_DD, 20 ); // LD dd,(nn)
      sys.AddOpcode( "ld", 0xdd2a, 2, AddressingType.IMMEDIATE_INDIRECT_TO_IX, 20 );    // LD IX,(nn)
      sys.AddOpcode( "ld", 0xfd2a, 2, AddressingType.IMMEDIATE_INDIRECT_TO_IY, 20 );    // LD IY,(nn)
      sys.AddOpcode( "ld", 0x22, 2, AddressingType.HL_TO_IMMEDIATE_INDIRECT, 16 );      // LD (nn),HL
      sys.AddOpcode( "ld", 0xed43, 2, AddressingType.REGISTER_DD_TO_IMMEDIATE_INDIRECT, 20 ); // LD (nn),dd
      sys.AddOpcode( "ld", 0xdd22, 2, AddressingType.IX_TO_IMMEDIATE_INDIRECT, 20 );    // LD (nn),IX
      sys.AddOpcode( "ld", 0xfd22, 2, AddressingType.IY_TO_IMMEDIATE_INDIRECT, 20 );    // LD (nn),IY
      sys.AddOpcode( "ld", 0xf9, 0, AddressingType.HL_TO_SP, 6 );                       // LD SP,HL
      sys.AddOpcode( "ld", 0xddf9, 0, AddressingType.IX_TO_SP, 10 );                    // LD SP,IX
      sys.AddOpcode( "ld", 0xfdf9, 0, AddressingType.IY_TO_SP, 10 );                    // LD SP,IY

      sys.AddOpcode( "push", 0xc5, 0, AddressingType.REGISTER_QQ, 11 );                 // PUSH qq
      sys.AddOpcode( "push", 0xdde5, 0, AddressingType.REGISTER_IX, 15 );               // PUSH IX
      sys.AddOpcode( "push", 0xfde5, 0, AddressingType.REGISTER_IY, 15 );               // PUSH IY
      sys.AddOpcode( "pop", 0xc1, 0, AddressingType.REGISTER_QQ, 10 );                  // POP qq
      sys.AddOpcode( "pop", 0xdde1, 0, AddressingType.REGISTER_IX, 14 );                // POP IX
      sys.AddOpcode( "pop", 0xfde1, 0, AddressingType.REGISTER_IY, 14 );                // POP IY

      // Exchange, block transfer, search
      sys.AddOpcode( "ex", 0xeb, 0, AddressingType.REGISTER_HL_TO_DE, 4 );              // EX DE,HL
      sys.AddOpcode( "ex", 0x08, 0, AddressingType.REGISTER_AF_TO_AF, 4 );              // EX AF,AF'
      sys.AddOpcode( "exx", 0xd9, 0, AddressingType.IMPLICIT, 4 );                      // EXX
      sys.AddOpcode( "ex", 0xe3, 0, AddressingType.REGISTER_HL_TO_SP_INDIRECT, 19 );    // EX (SP),HL
      sys.AddOpcode( "ex", 0xdde3, 0, AddressingType.REGISTER_IX_TO_SP_INDIRECT, 23 );  // EX (SP),IX
      sys.AddOpcode( "ex", 0xfde3, 0, AddressingType.REGISTER_IY_TO_SP_INDIRECT, 23 );  // EX (SP),IY

      sys.AddOpcode( "ldi", 0xeda0, 0, AddressingType.IMPLICIT, 16 );                   // LDI
      sys.AddOpcode( "ldir", 0xedb0, 0, AddressingType.IMPLICIT, 16, 5 );               // LDIR
      sys.AddOpcode( "ldd", 0xeda8, 0, AddressingType.IMPLICIT, 16 );                   // LDD
      sys.AddOpcode( "lddr", 0xedb8, 0, AddressingType.IMPLICIT, 16, 5 );               // LDDR
      sys.AddOpcode( "cpi", 0xeda1, 0, AddressingType.IMPLICIT, 16 );                   // CPI
      sys.AddOpcode( "cpir", 0xedb1, 0, AddressingType.IMPLICIT, 16, 5 );               // CPIR
      sys.AddOpcode( "cpd", 0xeda9, 0, AddressingType.IMPLICIT, 16 );                   // CPD
      sys.AddOpcode( "cpdr", 0xedb9, 0, AddressingType.IMPLICIT, 16, 5 );               // CPDR

      // 8bit arithmetic and logical
      sys.AddOpcode( "add", 0x80, 0, AddressingType.REGISTER_TO_A, 4 );                 // ADD A,r
      sys.AddOpcode( "add", 0xc6, 0, AddressingType.IMMEDIATE_TO_A, 7 );                // ADD A,n
      sys.AddOpcode( "add", 0x86, 0, AddressingType.HL_INDIRECT_TO_A, 7 );              // ADD A,(HL)
      sys.AddOpcode( "add", 0xdd86, 1, AddressingType.IX_D_INDIRECT_TO_A, 19 );         // ADD A,(IX+d)
      sys.AddOpcode( "add", 0xfd86, 1, AddressingType.IY_D_INDIRECT_TO_A, 19 );         // ADD A,(IY+d)

      sys.AddOpcode( "adc", 0x88, 0, AddressingType.REGISTER_TO_A, 4 );                 // ADC A,r
      sys.AddOpcode( "adc", 0xce, 0, AddressingType.IMMEDIATE_TO_A, 7 );                // ADC A,n
      sys.AddOpcode( "adc", 0x8e, 0, AddressingType.HL_INDIRECT_TO_A, 7 );              // ADC A,(HL)
      sys.AddOpcode( "adc", 0xdd8e, 1, AddressingType.IX_D_INDIRECT_TO_A, 19 );         // ADC A,(IX+d)
      sys.AddOpcode( "adc", 0xfd8e, 1, AddressingType.IY_D_INDIRECT_TO_A, 19 );         // ADC A,(IY+d)

      sys.AddOpcode( "sub", 0x90, 0, AddressingType.REGISTER_TO_A, 4 );                 // SUB r
      sys.AddOpcode( "sub", 0xd6, 0, AddressingType.IMMEDIATE_TO_A, 7 );                // SUB n
      sys.AddOpcode( "sub", 0x96, 0, AddressingType.HL_INDIRECT_TO_A, 7 );              // SUB (HL)
      sys.AddOpcode( "sub", 0xdd96, 1, AddressingType.IX_D_INDIRECT_TO_A, 19 );         // SUB (IX+d)
      sys.AddOpcode( "sub", 0xfd96, 1, AddressingType.IY_D_INDIRECT_TO_A, 19 );         // SUB (IY+d)

      sys.AddOpcode( "sbc", 0x98, 0, AddressingType.REGISTER_TO_A, 4 );                 // SBC A,r
      sys.AddOpcode( "sbc", 0xde, 0, AddressingType.IMMEDIATE_TO_A, 7 );                // SBC A,n
      sys.AddOpcode( "sbc", 0x9e, 0, AddressingType.HL_INDIRECT_TO_A, 7 );              // SBC A,(HL)
      sys.AddOpcode( "sbc", 0xdd9e, 1, AddressingType.IX_D_INDIRECT_TO_A, 19 );         // SBC A,(IX+d)
      sys.AddOpcode( "sbc", 0xfd9e, 1, AddressingType.IY_D_INDIRECT_TO_A, 19 );         // SBC A,(IY+d)

      sys.AddOpcode( "and", 0xa0, 0, AddressingType.REGISTER_TO_A, 4 );                 // AND r
      sys.AddOpcode( "and", 0xe6, 0, AddressingType.IMMEDIATE_TO_A, 7 );                // AND n
      sys.AddOpcode( "and", 0xa6, 0, AddressingType.HL_INDIRECT_TO_A, 7 );              // AND (HL)
      sys.AddOpcode( "and", 0xdda6, 1, AddressingType.IX_D_INDIRECT_TO_A, 19 );         // AND (IX+d)
      sys.AddOpcode( "and", 0xfda6, 1, AddressingType.IY_D_INDIRECT_TO_A, 19 );         // AND (IY+d)

      sys.AddOpcode( "or", 0xb0, 0, AddressingType.REGISTER_TO_A, 4 );                  // OR r
      sys.AddOpcode( "or", 0xf6, 0, AddressingType.IMMEDIATE_TO_A, 7 );                 // OR n
      sys.AddOpcode( "or", 0xb6, 0, AddressingType.HL_INDIRECT_TO_A, 7 );               // OR (HL)
      sys.AddOpcode( "or", 0xddb6, 1, AddressingType.IX_D_INDIRECT_TO_A, 19 );          // OR (IX+d)
      sys.AddOpcode( "or", 0xfdb6, 1, AddressingType.IY_D_INDIRECT_TO_A, 19 );          // OR (IY+d)

      sys.AddOpcode( "xor", 0xa8, 0, AddressingType.REGISTER_TO_A, 4 );                 // XOR r
      sys.AddOpcode( "xor", 0xee, 0, AddressingType.IMMEDIATE_TO_A, 7 );                // XOR n
      sys.AddOpcode( "xor", 0xae, 0, AddressingType.HL_INDIRECT_TO_A, 7 );              // XOR (HL)
      sys.AddOpcode( "xor", 0xddae, 1, AddressingType.IX_D_INDIRECT_TO_A, 19 );         // XOR (IX+d)
      sys.AddOpcode( "xor", 0xfdae, 1, AddressingType.IY_D_INDIRECT_TO_A, 19 );         // XOR (IY+d)

      sys.AddOpcode( "cp", 0xb8, 0, AddressingType.REGISTER_TO_A, 4 );                  // CP r
      sys.AddOpcode( "cp", 0xfe, 0, AddressingType.IMMEDIATE_TO_A, 7 );                 // CP n
      sys.AddOpcode( "cp", 0xbe, 0, AddressingType.HL_INDIRECT_TO_A, 7 );               // CP (HL)
      sys.AddOpcode( "cp", 0xddbe, 1, AddressingType.IX_D_INDIRECT_TO_A, 19 );          // CP (IX+d)
      sys.AddOpcode( "cp", 0xfdbe, 1, AddressingType.IY_D_INDIRECT_TO_A, 19 );          // CP (IY+d)

      sys.AddOpcode( "inc", 0x04, 0, AddressingType.REGISTER, 4 );                      // INC r
      sys.AddOpcode( "inc", 0x34, 0, AddressingType.REGISTER_HL_INDIRECT, 11 );         // INC (HL)
      sys.AddOpcode( "inc", 0xdd34, 0, AddressingType.REGISTER_IX_D_INDIRECT, 23 );     // INC (IX+d)
      sys.AddOpcode( "inc", 0xfd34, 0, AddressingType.REGISTER_IY_D_INDIRECT, 23 );     // INC (IY+d)

      sys.AddOpcode( "dec", 0x05, 0, AddressingType.REGISTER, 4 );                      // DEC r
      sys.AddOpcode( "dec", 0x35, 0, AddressingType.REGISTER_HL_INDIRECT, 11 );         // DEC (HL)
      sys.AddOpcode( "dec", 0xdd35, 0, AddressingType.REGISTER_IX_D_INDIRECT, 23 );     // DEC (IX+d)
      sys.AddOpcode( "dec", 0xfd35, 0, AddressingType.REGISTER_IY_D_INDIRECT, 23 );     // DEC (IY+d)

      // general purpose arithmetic and cpu control
      sys.AddOpcode( "daa", 0x27, 0, AddressingType.IMPLICIT, 4 );                      // DAA
      sys.AddOpcode( "cpl", 0x2f, 0, AddressingType.IMPLICIT, 4 );                      // CPL
      sys.AddOpcode( "neg", 0xed44, 0, AddressingType.IMPLICIT, 8 );                    // NEG
      sys.AddOpcode( "ccf", 0x3f, 0, AddressingType.IMPLICIT, 4 );                      // CCF
      sys.AddOpcode( "scf", 0x37, 0, AddressingType.IMPLICIT, 4 );                      // SCF
      sys.AddOpcode( "nop", 0x00, 0, AddressingType.IMPLICIT, 4 );                      // NOP
      sys.AddOpcode( "halt", 0x76, 0, AddressingType.IMPLICIT, 4 );                     // HALT
      sys.AddOpcode( "di", 0xf3, 0, AddressingType.IMPLICIT, 4 );                       // DI
      sys.AddOpcode( "ei", 0xfb, 0, AddressingType.IMPLICIT, 4 );                       // EI
      sys.AddOpcode( "im", 0xed46, 0, AddressingType.IMMEDIATE_0, 8 );                  // IM 0
      sys.AddOpcode( "im", 0xed56, 0, AddressingType.IMMEDIATE_1, 8 );                  // IM 1
      sys.AddOpcode( "im", 0xed5e, 0, AddressingType.IMMEDIATE_2, 8 );                  // IM 2

      // 16bit arithmetic
      sys.AddOpcode( "add", 0x09, 0, AddressingType.REGISTER_DD_TO_HL, 11 );            // ADD HL,dd
      sys.AddOpcode( "adc", 0xed0a, 0, AddressingType.REGISTER_DD_TO_HL, 15 );          // ADC HL,dd
      sys.AddOpcode( "sbc", 0xed42, 0, AddressingType.REGISTER_DD_TO_HL, 15 );          // SBC HL,dd
      sys.AddOpcode( "add", 0xdd09, 0, AddressingType.REGISTER_PP_TO_IX, 15 );          // ADD IX,pp
      sys.AddOpcode( "add", 0xfd09, 0, AddressingType.REGISTER_PP_TO_IY, 15 );          // ADD IY,pp

      sys.AddOpcode( "inc", 0x03, 0, AddressingType.REGISTER_DD, 6 );                   // INC dd
      sys.AddOpcode( "inc", 0xdd23, 0, AddressingType.REGISTER_IX, 10 );                // INC IX
      sys.AddOpcode( "inc", 0xfd23, 0, AddressingType.REGISTER_IY, 10 );                // INC IY
      sys.AddOpcode( "dec", 0x0b, 0, AddressingType.REGISTER_DD, 6 );                   // DEC dd
      sys.AddOpcode( "dec", 0xdd2b, 0, AddressingType.REGISTER_IX, 10 );                // DEC IX
      sys.AddOpcode( "dec", 0xfd2b, 0, AddressingType.REGISTER_IY, 10 );                // DEC IY

      // rotate and shift group
      sys.AddOpcode( "rlca", 0x07, 0, AddressingType.IMPLICIT, 4 );                     // RLCA
      sys.AddOpcode( "rla", 0x17, 0, AddressingType.IMPLICIT, 4 );                      // RLA
      sys.AddOpcode( "rrca", 0x0f, 0, AddressingType.IMPLICIT, 4 );                     // RRCA
      sys.AddOpcode( "rra", 0x1f, 0, AddressingType.IMPLICIT, 4 );                      // RRA

      sys.AddOpcode( "rlc", 0xcb00, 0, AddressingType.REGISTER, 8 );                    // RLC r
      sys.AddOpcode( "rlc", 0xcb06, 0, AddressingType.HL_INDIRECT, 15 );                // RLC (HL)
      sys.AddOpcode( "rlc", 0xddcb0006, 0, AddressingType.IX_D_INDIRECT, 23 );          // RLC (IX+d)
      sys.AddOpcode( "rlc", 0xfdcb0006, 0, AddressingType.IY_D_INDIRECT, 23 );          // RLC (IY+d)

      sys.AddOpcode( "rl", 0xcb10, 0, AddressingType.REGISTER, 8 );                     // RL r
      sys.AddOpcode( "rl", 0xcb16, 0, AddressingType.HL_INDIRECT, 15 );                 // RL (HL)
      sys.AddOpcode( "rl", 0xddcb0016, 0, AddressingType.IX_D_INDIRECT, 23 );           // RL (IX+d)
      sys.AddOpcode( "rl", 0xfdcb0016, 0, AddressingType.IY_D_INDIRECT, 23 );           // RL (IY+d)

      sys.AddOpcode( "rrc", 0xcb08, 0, AddressingType.REGISTER, 8 );                    // RRC r
      sys.AddOpcode( "rrc", 0xcb0e, 0, AddressingType.HL_INDIRECT, 15 );                // RRC (HL)
      sys.AddOpcode( "rrc", 0xddcb000e, 0, AddressingType.IX_D_INDIRECT, 23 );          // RRC (IX+d)
      sys.AddOpcode( "rrc", 0xfdcb000e, 0, AddressingType.IY_D_INDIRECT, 23 );          // RRC (IY+d)

      sys.AddOpcode( "rr", 0xcb18, 0, AddressingType.REGISTER, 8 );                     // RR r
      sys.AddOpcode( "rr", 0xcb1e, 0, AddressingType.HL_INDIRECT, 15 );                 // RR (HL)
      sys.AddOpcode( "rr", 0xddcb001e, 0, AddressingType.IX_D_INDIRECT, 23 );           // RR (IX+d)
      sys.AddOpcode( "rr", 0xfdcb001e, 0, AddressingType.IY_D_INDIRECT, 23 );           // RR (IY+d)

      sys.AddOpcode( "sla", 0xcb20, 0, AddressingType.REGISTER, 8 );                    // SLA r
      sys.AddOpcode( "sla", 0xcb26, 0, AddressingType.HL_INDIRECT, 15 );                // SLA (HL)
      sys.AddOpcode( "sla", 0xddcb0026, 0, AddressingType.IX_D_INDIRECT, 23 );          // SLA (IX+d)
      sys.AddOpcode( "sla", 0xfdcb0026, 0, AddressingType.IY_D_INDIRECT, 23 );          // SLA (IY+d)

      sys.AddOpcode( "sra", 0xcb28, 0, AddressingType.REGISTER, 8 );                    // SRA r
      sys.AddOpcode( "sra", 0xcb2e, 0, AddressingType.HL_INDIRECT, 15 );                // SRA (HL)
      sys.AddOpcode( "sra", 0xddcb002e, 0, AddressingType.IX_D_INDIRECT, 23 );          // SRA (IX+d)
      sys.AddOpcode( "sra", 0xfdcb002e, 0, AddressingType.IY_D_INDIRECT, 23 );          // SRA (IY+d)

      sys.AddOpcode( "srl", 0xcb38, 0, AddressingType.REGISTER, 8 );                    // SRL r
      sys.AddOpcode( "srl", 0xcb3e, 0, AddressingType.HL_INDIRECT, 15 );                // SRL (HL)
      sys.AddOpcode( "srl", 0xddcb003e, 0, AddressingType.IX_D_INDIRECT, 23 );          // SRL (IX+d)
      sys.AddOpcode( "srl", 0xfdcb003e, 0, AddressingType.IY_D_INDIRECT, 23 );          // SRL (IY+d)

      sys.AddOpcode( "rld", 0xed6f, 0, AddressingType.IMPLICIT, 18 );                   // RLD
      sys.AddOpcode( "rrd", 0xed67, 0, AddressingType.IMPLICIT, 18 );                   // RRD

      // bit set, reset, test group
      sys.AddOpcode( "bit", 0xcb40, 0, AddressingType.REGISTER_TO_BIT, 8 );             // BIT b,r
      sys.AddOpcode( "bit", 0xcb46, 0, AddressingType.HL_INDIRECT_TO_BIT, 12 );         // BIT b,(HL)
      sys.AddOpcode( "bit", 0xddcb0046, 0, AddressingType.IX_D_INDIRECT_TO_BIT, 20 );   // BIT b,(IX+d)
      sys.AddOpcode( "bit", 0xfdcb0046, 0, AddressingType.IY_D_INDIRECT_TO_BIT, 20 );   // BIT b,(IY+d)

      sys.AddOpcode( "set", 0xcbc0, 0, AddressingType.REGISTER_TO_BIT, 8 );             // SET b,r
      sys.AddOpcode( "set", 0xcbc6, 0, AddressingType.HL_INDIRECT_TO_BIT, 15 );         // SET b,(HL)
      sys.AddOpcode( "set", 0xddcb00c6, 0, AddressingType.IX_D_INDIRECT_TO_BIT, 23 );   // SET b,(IX+d)
      sys.AddOpcode( "set", 0xfdcb00c6, 0, AddressingType.IY_D_INDIRECT_TO_BIT, 23 );   // SET b,(IY+d)

      sys.AddOpcode( "res", 0xcb80, 0, AddressingType.REGISTER_TO_BIT, 8 );             // RES b,r
      sys.AddOpcode( "res", 0xcb86, 0, AddressingType.HL_INDIRECT_TO_BIT, 15 );         // RES b,(HL)
      sys.AddOpcode( "res", 0xddcb0086, 0, AddressingType.IX_D_INDIRECT_TO_BIT, 23 );   // RES b,(IX+d)
      sys.AddOpcode( "res", 0xfdcb0086, 0, AddressingType.IY_D_INDIRECT_TO_BIT, 23 );   // RES b,(IY+d)

      // jump group
      sys.AddOpcode( "jp", 0xc3, 2, AddressingType.ABSOLUTE, 10 );                      // JP nn
      sys.AddOpcode( "jp", 0xc2, 2, AddressingType.ABSOLUTE_CONDITION, 10 );            // JP cc,nn
      sys.AddOpcode( "jr", 0x18, 1, AddressingType.RELATIVE, 12 );                      // JR e
      sys.AddOpcode( "jr", 0x38, 1, AddressingType.RELATIVE_C, 7, 5 );                  // JR C,e
      sys.AddOpcode( "jr", 0x30, 1, AddressingType.RELATIVE_NC, 7, 5 );                 // JR NC,e
      sys.AddOpcode( "jr", 0x28, 1, AddressingType.RELATIVE_Z, 7, 5 );                  // JR Z,e
      sys.AddOpcode( "jr", 0x20, 1, AddressingType.RELATIVE_NZ, 7, 5 );                 // JR NZ,e
      sys.AddOpcode( "jp", 0xe9, 0, AddressingType.HL_INDIRECT, 4 );                    // JP (HL)
      sys.AddOpcode( "jp", 0xddc3, 0, AddressingType.IX_INDIRECT, 8 );                  // JP (IX)
      sys.AddOpcode( "jp", 0xfdc3, 0, AddressingType.IY_INDIRECT, 8 );                  // JP (IY)
      sys.AddOpcode( "djnz", 0x10, 1, AddressingType.RELATIVE, 8, 5 );                  // DJNZ e

      // call and return group
      sys.AddOpcode( "call", 0xcd, 2, AddressingType.ABSOLUTE, 17 );                    // CALL nn
      sys.AddOpcode( "call", 0xc4, 2, AddressingType.ABSOLUTE_CONDITION, 10, 7 );       // CALL cc,nn
      sys.AddOpcode( "ret", 0xc9, 0, AddressingType.IMPLICIT, 10 );                     // RET
      sys.AddOpcode( "ret", 0xc0, 0, AddressingType.IMPLICIT_CC, 5, 6 );                // RET cc
      sys.AddOpcode( "reti", 0xed4d, 0, AddressingType.IMPLICIT, 14 );                  // RETI
      sys.AddOpcode( "retn", 0xed45, 0, AddressingType.IMPLICIT, 14 );                  // RETN
      sys.AddOpcode( "rst", 0xc7, 0, AddressingType.IMPLICIT_P, 11 );                   // RST p

      // input and output group
      sys.AddOpcode( "in", 0xdb, 1, AddressingType.IMMEDIATE_INDIRECT_TO_A_8BIT, 11 );  // IN A,(n)
      sys.AddOpcode( "in", 0xed40, 0, AddressingType.INDIRECT_C_TO_REGISTER, 12 );      // IN r,(C)
      sys.AddOpcode( "ini", 0xeda2, 0, AddressingType.IMPLICIT, 16 );                   // INI
      sys.AddOpcode( "inir", 0xedb2, 0, AddressingType.IMPLICIT, 16, 5 );               // INIR
      sys.AddOpcode( "ind", 0xedaa, 0, AddressingType.IMPLICIT, 16 );                   // IND
      sys.AddOpcode( "indr", 0xedba, 0, AddressingType.IMPLICIT, 16, 5 );               // INDR

      sys.AddOpcode( "out", 0xd3, 1, AddressingType.A_TO_IMMEDIATE_INDIRECT_8BIT, 11 ); // OUT (n),A
      sys.AddOpcode( "out", 0xed41, 0, AddressingType.REGISTER_TO_C_INDIRECT, 12 );     // OUT (C),r
      sys.AddOpcode( "outi", 0xeda3, 0, AddressingType.IMPLICIT, 16 );                  // OUTI
      sys.AddOpcode( "otir", 0xedb3, 0, AddressingType.IMPLICIT, 16, 5 );               // OTIR
      sys.AddOpcode( "outd", 0xedab, 0, AddressingType.IMPLICIT, 16 );                  // OUTD
      sys.AddOpcode( "otdr", 0xedbb, 0, AddressingType.IMPLICIT, 16, 5 );               // OTDR
      return sys;
    }



  }
}
