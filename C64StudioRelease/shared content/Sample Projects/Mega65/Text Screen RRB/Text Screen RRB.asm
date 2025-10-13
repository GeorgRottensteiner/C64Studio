; adapted Shallan's RRB sample

; Think of render buffer as a list of instructions per line:
; where each screen location and color ram location defines the instruction
; with xpos incrementing by 8 after each char unless a GOTOX has relocated it
;
; xpos=0, char=0, color=black
; xpos=8, char=1, color=black
; xpos=16, char=2, color=blacK
; ....
; xpos=312, char=39, color=black
;
; GOTOX - [$10, $00] -> jump to new x position
; xpos=100, char=15, color=yellow,

; GOTOX - [$10, $00] -> Jump to x position = 320
; xpos=320, char=0, color=black



;Mega 65
!cpu m65

;to include basic VIC constants
!source <c64.asm>

;to include Mega65 constants VIC3 and VIC4
!source <mega65.asm>


COLOR_RAM         = $ff80000

;the character data is set up as a last of regular characters
;   40    characters screen width
;
;followed by 3 GOTOX statements with an argument value each
;   +1    first GOTOX statement
;   +1    first GOTOX char
;   +1    second GOTOX statement
;   +1    second GOTOX char
;   +1    final right most GOTOX statement
;   +1    pseudo char
; = 46
ROW_SIZE          = 46

;we use 16bit characters, therefor * 2
ROW_SIZE_BYTES    = ROW_SIZE * 2

GOTOX             = $10
TRANSPARENT       = $80


* = $02 "Zeropage"

SCREEN_VECTOR
          !word ?



* = $2001 "Code"

          !basic

          ;Disable ROM banks and interrupts
          sei
          lda #$35
          sta PROCESSOR_PORT

          lda #$00
          sta VIC.IRQ_MASK

          lda #$7f
          sta CIA1.IRQ_CONTROL
          sta CIA2.NMI_CONTROL

          +DisableC65ROM


          ;Initialise Mega65 into VIC4 mode at 40 mhz
          +Enable40Mhz
          +EnableVIC4Registers

          ;disable rom mappings
          ;and disable rom palette for 0-15
          lda #$04
          sta VIC3.ROMBANK

          ;Set to 40 columns by disabling H640 (bit 7) of $d031
          lda #$80
          trb VIC3.VICDIS

          lda #$05          ;Enable 16 bit char numbers (bit0) and
          sta VIC4.VIC4DIS  ;full color for chars>$ff (bit2)

          ;Set logical row width
          ;bytes per screen row (16 bit value in $d058-$d059)
          lda #<ROW_SIZE_BYTES
          sta VIC4.CHARSTEP_LO
          lda #>ROW_SIZE_BYTES
          sta VIC4.CHARSTEP_HI

          ;Set number of chars per row
          lda #ROW_SIZE
          sta VIC4.CHRCOUNT

          ;Relocate screen RAM using $d060-$d063
          lda #<SCREEN_BASE
          sta VIC4.SCRNPTR
          lda #>SCREEN_BASE
          sta VIC4.SCRNPTR + 1
          lda #$00
          sta VIC4.SCRNPTR + 2
          sta VIC4.EXGLYPH_CHRCOUNT_SCRNPTR

          jsr CopyColors

MainLoop
          ;Crappy animation of GOTOX values
-
          lda VIC.CONTROL_1
          bpl -

          ;do stuff
          inc SINUSINDEX

          ldx SINUSINDEX
          ldy #$00

.Loop
          lda SCREEN_OFFSETS, y
          iny
          sta SCREEN_VECTOR + 0

          lda SCREEN_OFFSETS, y
          iny
          sta SCREEN_VECTOR + 1

          ;sine offset
          ldz #$00
          lda SINE_TABLE_LO, x
          sta (SCREEN_VECTOR), z

          inz
          lda SINE_TABLE_HI, x
          sta (SCREEN_VECTOR), z

          ;mirrored size offset for second column
          ldz #$04
          lda #<( SCREEN_WIDTH - 8 )
          sec
          sbc SINE_TABLE_LO, x
          sta (SCREEN_VECTOR), z

          inz
          lda #>( SCREEN_WIDTH - 8 )
          sbc SINE_TABLE_HI, x
          sta (SCREEN_VECTOR), z

          txa
          clc
          adc #$04
          tax

          cpy #$32
          bne .Loop

-
          lda $d011
          bmi -

          jmp MainLoop


!lzone CopyColors
          +RunDMAJob Job
          rts

Job
          +DMAHeader $00, COLOR_RAM >> 20
          +DMACopyJob COLORS, COLOR_RAM, ROW_SIZE_BYTES * 25, 0, 0


SINUSINDEX
          !byte $00


PI = 3.1415926
SCREEN_WIDTH = 320

SINE_TABLE_LO
          !fill 256, <( math.sin( math.todegrees( i / 256.0 * PI * 2 ) ) * ( SCREEN_WIDTH - 8 ) / 2 + ( SCREEN_WIDTH - 8 ) / 2 )

SINE_TABLE_HI
          !fill 256, >( math.sin( math.todegrees( i / 256.0 * PI * 2 ) ) * ( SCREEN_WIDTH - 8 ) / 2 + ( SCREEN_WIDTH - 8 ) / 2 )


SCREEN_OFFSETS
          !for rox = 0 to 24
          !word SCREEN_BASE + rox * ROW_SIZE_BYTES + 80
          !end



* = $4000 "Screen prefilled data"
SCREEN_BASE
          ;Build each row in a loop
          !for LINE = 0 to 24

          ;40 characters, starting with @ in first line, A in second, etc.
          !fill 40, [LINE, 0]  ;0 1 2 3 4 5 6 7 8 9 10 11

          ;Because GOTOX flag set in color ram, the value is interpreted as x pos, not a drawn character!
          ;   set x position to 100
          !byte $64, 0

          ;actual char
          !byte 13, 0

          ;GOTOX 100
          !byte $64, 0

          ;actual char
          !byte 15, 0

          ;Set final GOTOX to 320 to ensure raster ends on far right of screen
          !byte <320, >320

          ;Draw final char offscreen
          !byte 0,0

          !end


COLORS
          !for LINE = 0 to 24

          ;40 16bit chars = 80 bytes
          !fill 40, [0, 0] ;layer 1

          ;+4
          ;Set bit 4 of color ram byte 0 to enable gotox flag
          ;set bit 7 additionally to enable transparency

          !byte GOTOX | TRANSPARENT, $00
          !byte 0, 7 ;standard 16 colors are in bits 0-3 of byte 1 in 16 bit char mode

          ;+4
          !byte GOTOX | TRANSPARENT, $00
          !byte 0, 5 ;standard 16 colors are in bits 0-3 of byte 1 in 16 bit char mode

          ;+4
          ;Final GOTOX to ensure whole row is drawn
          !byte GOTOX, $00 ;Set bit 4 of color ram byte 0 to enable gotox flag
          !byte 0,0

          !end


