;to set the proper CPU
!cpu m65

;to include VIC constants
!source <c64.asm>

;to include Mega65 constants VIC3 and VIC4
!source <mega65.asm>


;we place the screen from $0800 to $0fd0
SCREEN_CHAR   = $0800

;the real color RAM is here (at $d800 only the first 1000 bytes are accessible)
SCREEN_COLOR  = $ff80000

ZEROPAGE_POINTER_1 = $f8
ZEROPAGE_POINTER_2 = $fa

CHAR_PALETTE_ENTRY_COUNT = 256

;use $fc to $ff as target (32bit)
ZEROPAGE_POINTER_TARGET = $fc

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

          cli

          ;set 40x25 mode
          lda #$80
          trb VIC3.VICDIS

          ;80 bytes per screen line
          lda #<80
          sta VIC4.CHARSTEP_LO
          lda #>80
          sta VIC4.CHARSTEP_HI

          ;number of chars per row
          lda #40
          sta VIC4.CHRCOUNT

          ;Turn on FCM mode (and 16bit per char number)
          lda #$07
          sta VIC4.VIC4DIS

          lda #0
          sta VIC.BACKGROUND_COLOR

          lda #<TEXT_SCREEN_DATA
          sta ZEROPAGE_POINTER_1
          lda #>TEXT_SCREEN_DATA
          sta ZEROPAGE_POINTER_1 + 1

          lda #<SCREEN_CHAR
          sta ZEROPAGE_POINTER_2
          lda #>SCREEN_CHAR
          sta ZEROPAGE_POINTER_2 + 1

          ;setup target pointer in zeropage
          ldx #<SCREEN_COLOR
          stx ZEROPAGE_POINTER_TARGET + 0
          ldx #>SCREEN_COLOR
          stx ZEROPAGE_POINTER_TARGET + 1
          ldx #( SCREEN_COLOR >> 16 ) & $ff
          stx ZEROPAGE_POINTER_TARGET + 2
          ldx #( SCREEN_COLOR >> 24 )
          stx ZEROPAGE_POINTER_TARGET + 3

          ;no. of lines
          ldx #25

.NextLine
          ;copy one line of data
          ldy #0
          ldz #0

-
          ;we need to add offset to the character data
          lda #0
          sta [ZEROPAGE_POINTER_TARGET],z

          ;lo byte
          lda (ZEROPAGE_POINTER_1),y
          clc
          adc #<( TILE_DATA / 64 )
          sta (ZEROPAGE_POINTER_2),y
          iny
          inz

          ;hi byte
          lda #0
          sta [ZEROPAGE_POINTER_TARGET],z

          ;this requires the carry bit from the lo byte addition set!
          lda (ZEROPAGE_POINTER_1),y
          adc #>( TILE_DATA / 64 )
          sta (ZEROPAGE_POINTER_2),y

          iny
          inz

          cpy #80
          bne -

          ;update pointers

          ;(we only change the lower 16bit)
          lda ZEROPAGE_POINTER_TARGET
          clc
          adc #80
          sta ZEROPAGE_POINTER_TARGET
          bcc +
          inc ZEROPAGE_POINTER_TARGET + 1
+

          lda ZEROPAGE_POINTER_1
          clc
          adc #80
          sta ZEROPAGE_POINTER_1
          bcc +
          inc ZEROPAGE_POINTER_1 + 1
+

          lda ZEROPAGE_POINTER_2
          clc
          adc #80
          sta ZEROPAGE_POINTER_2
          bcc +
          inc ZEROPAGE_POINTER_2 + 1
+

          dex
          bne .NextLine

          jsr SetPalette


          ;endless loop
          jmp *


!lzone SetPalette
          ;Bit 6-7 = Mapped Palette
          ;bit 0-1 = Char palette index
          lda #%01011001
          sta VIC4.PALSEL

          ;copy palette data (CHAR_PALETTE_ENTRY_COUNT entries),
          ldx #0
          ldy #0
-

          lda PALETTE_DATA, x
          sta VIC4.PALRED,y
          lda PALETTE_DATA + 1 * CHAR_PALETTE_ENTRY_COUNT, x
          sta VIC4.PALGREEN,y
          lda PALETTE_DATA + 2 * CHAR_PALETTE_ENTRY_COUNT, x
          sta VIC4.PALBLUE,y

          iny
          inx
          bne -

          lda #33
          sta VIC.BACKGROUND_COLOR

          ;set sprite pal to bank 1
          lda #%10011001
          sta VIC4.PALSEL

          rts



TEXT_SCREEN_DATA
          ;contains 40x25 character words (40 * 25 * 2 bytes)
          !media "Text Screen.charscreen",CHAR

PALETTE_DATA
          ;holds the 3 RGB values of all 256 colors
          ;note: Mega65 tries to be compatible to the C65, so high and low nybble of a color
          ;      byte are swapped. Hence SWIZZLED is used so we can copy the values over directly
          !media "Text Screen.charscreen",PALETTESWIZZLED

!realign 64
TILE_DATA

!media "Text Screen.charscreen",CHARSET,0,652

