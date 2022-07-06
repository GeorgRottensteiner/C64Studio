;to set the proper CPU
!cpu m65

;to include VIC constants
!source <c64.asm>

;to include Mega65 constants VIC3 and VIC4
!source <mega65.asm>


SCREEN_CHAR   = $0800

;the real color RAM is here (at $d800 only the first 1000 bytes are accessible)
SCREEN_COLOR  = $ff80000

;we use 16bit characters, but in NCM mode 20 characters cover 40 visible characters
ROW_SIZE_BYTES    = 40


;two pointers for copying
ZEROPAGE_POINTER_1 = $f8
ZEROPAGE_POINTER_2 = $fa

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

          ;this sets 80x25 mode
          ;lda #$88
          ;tsb VIC3.VICDIS

          ;Turn 16bit per char number (required for NCM)
          lda #$01
          sta VIC4.VIC4DIS

          ;Set logical row width
          ;bytes per screen row (16 bit value in $d058-$d059)
          lda #<ROW_SIZE_BYTES
          sta VIC4.CHARSTEP_LO
          lda #>ROW_SIZE_BYTES
          sta VIC4.CHARSTEP_HI

          ;number of chars per row
          lda #20
          sta VIC4.CHRCOUNT

          lda #0
          sta VIC.BACKGROUND_COLOR

          lda #<SCREEN_DATA
          sta ZEROPAGE_POINTER_1
          lda #>SCREEN_DATA
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
          ;color byte 0
          lda #$08
          sta [ZEROPAGE_POINTER_TARGET],z

          ;hi byte char data
          lda (ZEROPAGE_POINTER_1),y
          sta (ZEROPAGE_POINTER_2),y
          iny
          inz

          ;color byte 1
          lda #0
          sta [ZEROPAGE_POINTER_TARGET],z

          ;lo byte char data
          lda (ZEROPAGE_POINTER_1),y
          sta (ZEROPAGE_POINTER_2),y

          iny
          inz

          cpy #80
          bne -

          ;update pointers

          ;(we only change the lower 16bit)
          lda ZEROPAGE_POINTER_TARGET
          clc
          adc #ROW_SIZE_BYTES
          sta ZEROPAGE_POINTER_TARGET
          bcc +
          inc ZEROPAGE_POINTER_TARGET + 1
+

          lda ZEROPAGE_POINTER_1
          clc
          adc #ROW_SIZE_BYTES
          sta ZEROPAGE_POINTER_1
          bcc +
          inc ZEROPAGE_POINTER_1 + 1
+

          lda ZEROPAGE_POINTER_2
          clc
          adc #ROW_SIZE_BYTES
          sta ZEROPAGE_POINTER_2
          bcc +
          inc ZEROPAGE_POINTER_2 + 1
+

          dex
          bne .NextLine

          jmp *


          ;screen data has a configured offset of 192, so all character values start with 192
          ;to point at $3000 (192 = $C0, $C0 * 64 = $3000)
SCREEN_DATA
          !media "Text Screen.charscreen",CHARCOLOR

          ;insert character set data at $3000 directly
* = $3000
          !media "Text Screen.charscreen",CHARSET,0,2

