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

;   40    characters screen width
;   +1    first GOTOX statement
;   +40   chars
;   +1    final right most GOTOX statement
;   +1    pseudo char
; = 83

ROW_SIZE          = 83

;we use 16bit characters
ROW_SIZE_BYTES    = ROW_SIZE * 2

GOTOX             = $10
TRANSPARENT       = $80

NUM_LINES         = 25


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

          lda #$07          ;Enable 16 bit char numbers (bit0) and
          sta VIC4.VIC4DIS  ;full color for all chars

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

          ;since the export of characters affected also the pseudo characters with offset we
          ;need to manually restore it (for now)

          ;fix GOTOX offsets for upper layer
          ldx #0
-
          lda SCREEN_LINE_OFFSET_LO, x
          sta SCREEN_VECTOR
          lda SCREEN_LINE_OFFSET_HI, x
          sta SCREEN_VECTOR + 1

          ;offset 0 so it starts at the left border
          ldy #80
          lda #<0
          sta (SCREEN_VECTOR),y
          iny
          lda #>0
          sta (SCREEN_VECTOR),y

          ;offset 320 for the last pseudo character so it ends at the right border
          ldy #162
          lda #<320
          sta (SCREEN_VECTOR),y
          iny
          lda #>320
          sta (SCREEN_VECTOR),y

          inx
          cpx #NUM_LINES + 1
          bne -


          jsr CopyColors

MainLoop
          ;Crappy animation of GOTOX values
-
          lda VIC.CONTROL_1
          bpl -

          ;do stuff
          inc SinusIndex

          ldx SinusIndex
          ldy #$00

.Loop
          lda SCREEN_LINE_OFFSET_LO, y
          sta SCREEN_VECTOR + 0
          lda SCREEN_LINE_OFFSET_HI, y
          sta SCREEN_VECTOR + 1
          iny

          ;sine offset
          ldz #$00 + 80
          lda SINE_TABLE_LO, x
          sta (SCREEN_VECTOR), z

          inz
          lda SINE_TABLE_HI, x
          sta (SCREEN_VECTOR), z

          ;inx

          cpy #NUM_LINES
          bne .Loop

-
          lda $d011
          bmi -

          jmp MainLoop


!zone CopyColors
CopyColors
          +RunDMAJob Job
          rts

Job
          +DMAHeader $00, COLOR_RAM >> 20
          +DMACopyJob COLORS, COLOR_RAM, ROW_SIZE_BYTES * NUM_LINES, 0, 0


SinusIndex
          !byte $00


PI = 3.1415926
SCREEN_WIDTH = 320

SINE_TABLE_LO
          !fill 256, <( math.sin(math.todegrees(i / 256.0 * PI * 2)) * SCREEN_WIDTH / 2 + SCREEN_WIDTH / 2 )

SINE_TABLE_HI
          !fill 256, >( math.sin(math.todegrees(i / 256.0 * PI * 2)) * SCREEN_WIDTH / 2 + SCREEN_WIDTH / 2 )


SCREEN_LINE_OFFSET_LO
          !for rox = 0 to 24
          !byte <( SCREEN_BASE + rox * ROW_SIZE_BYTES )
          !end

SCREEN_LINE_OFFSET_HI
          !for rox = 0 to 24
          !byte >( SCREEN_BASE + rox * ROW_SIZE_BYTES )
          !end

* = $3000
FCM_CHARS
          !media "Text Screen.charscreen",CHARSET,0,7

* = $4000 "Screen prefilled data"
SCREEN_BASE
          !media "Text Screen.charscreen",CHAR
          ;;Build each row in a loop
;          !for LINE = 0 to 24
;
;          !fill 40, [<( FCM_CHARS / 64 ), >( FCM_CHARS / 64 )]
;
;          ;Because GOTOX flag set in color ram, no draw!!!,
;          ;instead set XPosition to 100
;          !byte <0, >0
;
;          ;2nd layer chars
;          !fill 40, [<( FCM_CHARS / 64 + 1 ), >( FCM_CHARS / 64 + 1 )]
;
;          ;Set final GOTOX to 320 to ensure raster ends on far right of screen
;          !byte <320, >320
;
;          ;Draw final char offscreen
;          !byte 0,0
;
;          !end


COLORS
          !for LINE = 0 to 24

          ;40 16bit chars = 80 bytes
          !fill 40, [0, 0] ;layer 1

          ;+4
          ;Set bit 4 of color ram byte 0 to enable gotox flag
          ;set bit 7 additionally to enable transparency

          !byte GOTOX | TRANSPARENT, $00

          ;second layer colors
          !fill 40, [0, 0] ;layer 1

          !byte GOTOX | TRANSPARENT, $00
          !byte 0,0

          !end


