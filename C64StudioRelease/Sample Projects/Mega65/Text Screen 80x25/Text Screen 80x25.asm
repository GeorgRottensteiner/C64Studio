;to set the proper CPU
!cpu m65

;to include VIC constants
!source <c64.asm>

;to include Mega65 constants
!source <mega65.asm>


SCREEN_CHAR   = $0800

;for 80x25 the color RAM is up here
SCREEN_COLOR  = $ff80000

* = $2001

          ;this must follow after !cpu m65, because it adds a BANK to the BASIC upstart
          !basic

          sei

          +EnableVIC4Registers

          ;Turn off CIA interrupts
          lda #$7f
          sta CIA1.IRQ_CONTROL
          sta CIA2.NMI_CONTROL

          ;Turn off raster interrupts, used by C65 rom
          lda #$00
          sta VIC.IRQ_MASK

          ;number of chars per row
          lda #80
          sta VIC4.CHRCOUNT


          cli

          lda #0
          sta VIC.BACKGROUND_COLOR

          ;copy text screen data
          ldx #0
-
          lda TEXT_SCREEN_DATA,x
          sta SCREEN_CHAR,x
          lda TEXT_SCREEN_DATA + 1 * 250,x
          sta SCREEN_CHAR + 1 * 250,x
          lda TEXT_SCREEN_DATA + 2 * 250,x
          sta SCREEN_CHAR + 2 * 250,x
          lda TEXT_SCREEN_DATA + 3 * 250,x
          sta SCREEN_CHAR + 3 * 250,x
          lda TEXT_SCREEN_DATA + 4 * 250,x
          sta SCREEN_CHAR + 4 * 250,x
          lda TEXT_SCREEN_DATA + 5 * 250,x
          sta SCREEN_CHAR + 5 * 250,x
          lda TEXT_SCREEN_DATA + 6 * 250,x
          sta SCREEN_CHAR + 6 * 250,x
          lda TEXT_SCREEN_DATA + 7 * 250,x
          sta SCREEN_CHAR + 7 * 250,x

          inx
          cpx #250
          bne -

          ;color RAM is located at $ff80000 so we can't use plain sta
          jsr CopyColorData

          ;endless loop
          jmp *



!zone CopyColorData

          ;use $fa to $fb as source
.ZEROPAGE_POINTER_SOURCE = $fa

          ;use $fc to $ff as target (32bit)
.ZEROPAGE_POINTER_TARGET = $fc

CopyColorData
          lda #<TEXT_SCREEN_DATA_COLOR
          sta .ZEROPAGE_POINTER_SOURCE
          lda #>TEXT_SCREEN_DATA_COLOR
          sta .ZEROPAGE_POINTER_SOURCE + 1

          ;setup target pointer in zeropage
          ldx #<SCREEN_COLOR
          stx .ZEROPAGE_POINTER_TARGET + 0
          ldx #>SCREEN_COLOR
          stx .ZEROPAGE_POINTER_TARGET + 1
          ldx #( SCREEN_COLOR >> 16 ) & $ff
          stx .ZEROPAGE_POINTER_TARGET + 2
          ldx #( SCREEN_COLOR >> 24 )
          stx .ZEROPAGE_POINTER_TARGET + 3

          ;8 pages á 256 (yes, more than the required 2000 bytes)
          ldy #8

          ldz #0
          ldx #0

.Outerloop
-
.ZEROPAGE_POINTER_SOURCE = * + 1
          lda TEXT_SCREEN_DATA_COLOR,x
          sta [.ZEROPAGE_POINTER_TARGET], z
          inz
          inx
          bne -

          dey
          beq +

          inc .ZEROPAGE_POINTER_TARGET + 1
          inc .ZEROPAGE_POINTER_SOURCE + 1

          bra .Outerloop
+
          rts



TEXT_SCREEN_DATA
TEXT_SCREEN_DATA_COLOR = * + 80 * 25
          ;contains 80x25 character bytes, followed by 80x25 color bytes
          !media "Text Screen.charscreen",CHARCOLOR

