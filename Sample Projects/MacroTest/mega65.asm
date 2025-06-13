!src <mega65.asm>

!cpu m65

!macro enable40Mhz
    lda #$41
    sta $00   ;40 Mhz mode
!end

!macro enableVIC3Registers {
    lda #$00
    tax
    tay
    taz
    map
    eom

    lda #$A5  //Enable VIC III
    sta $d02f
    lda #$96
    sta $d02f
}

!macro enableVIC4Registers {
    lda #$00
    tax
    tay
    taz
    map
    eom

    lda #$47  ;Enable VIC IV
    sta $d02f
    lda #$53
    sta $d02f
}

;#import "../_include/m65macros.s"

SCREEN_RAM  = $10000
COLOR_RAM   = $ff80000


* = $02
!zone ZP
  .Screen
    !word ?
    !word ?
  .Color
    !word ?
    !word ?

* = $2001
!basic

Entry
    jsr SetupSystem

    jsr ScreenClear80

    jsr SetupScreenVector
    jsr SetupColorVector
    ;Move default screen location
    ;$d060-$d063
    lda #<SCREEN_RAM
    sta $d060
    lda #>SCREEN_RAM
    sta $d061
    lda #( SCREEN_RAM >> 16 )
    sta $d062

    ;Relocate charset (Only for hires and MCM)
    ;$d068-$d06a
    ; lda #<CHARROM
    ; sta $d068
    ; lda #>CHARROM
    ; sta $d069
    ; lda #[CHARROM >> 16]
    ; sta $d06a



    ;Disable hot register so VIC2 registers
    ;turn off bit 7
    lda #$80
    trb $d05d   ;wont destroy VIC4 values (bit 7)

    ; Set VIC to use 40 column mode display
    ;turn off bit 7
    lda #$80
    trb $d031

    ;Turn on MCM (same as C64)
    ; lda #$10
    ; tsb $d016

    ;Turn on FCM mode and
    ;16bit per char number
    ;bit 0 = Enable 16 bit char numbers
    ;bit 1 = Enable Fullcolor for chars <=$ff
    ;bit 2 = Enable Fullcolor for chars >$ff
    lda #$07
    sta $d054


    ;Clear 8 pages (2000 bytes) for Hires and MCM
    ; lda #$00
    ; ldx #$08;Pages to clear
    ; jsr ScreenClear32bitAddr
    ; lda #$08
    ; ldx #$08;Pages to clear
    ; jsr ColorClear32bitAddr

    ;Clear 16 pages (4000 bytes) FCM
    lda #$00
    ldx #$10;Pages to clear
    jsr ScreenClear32bitAddr
    ; lda #$08
    ; ldx #$10;Pages to clear
    ; jsr ColorClear32bitAddr




    jsr SetPalette
    ;Set a 16 bit char on screen
    ldz #$00
    lda #$00
    sta [ZP.Screen], z
    lda #$01
    inz
    sta [ZP.Screen], z
    lda #$00
    inz
    sta [ZP.Screen], z

    ;change the 16 bit extended atrributes
    ldz #$00
    lda #%10000000 ;Set bit 7 = Vertical flip
    sta [ZP.Color], z
    lda #$00000000
    inz
    sta [ZP.Color], z

    lda #%10000000 ;Set bit 7 = Vertical flip
    inz
    ;sta [ZP.Color], z
    jmp *


SetPalette:
    ;Bit 6-7 = Mapped Palette
    ;bit 0-1 = Char palette index
    lda #%01000001
    sta $d070

    ldx #$00
-
    lda PaletteData + $000, x
    sta $d100, x ;Red
    lda PaletteData + $100, x
    sta $d200, x ;Green
    lda PaletteData + $200, x
    sta $d300, x ;Blue
    inx
    bne -

    rts


PaletteData:
; #4CAF50

  ;256 reds (only color that is 7 bits not 8)
  ;Missing bit because of nybble swap = bit 4
  !byte $00,$0f,$00,$00
  !fill 252, 0

  ;256 greens
  !byte $00,$00,$0f,$00
  !fill 252, 0

  ;256 blues
  !byte $00,$00,$00,$0f
  !fill 252, 0


ColorClear32bitAddr

  .Outerloop
    ldz #$00
-
    sta ((ZP.Color)), z
    inz
    bne -
    dex
    beq +
    inc ZP.Color + 1
    bra .Outerloop
+

    jsr SetupColorVector
    rts

ScreenClear32bitAddr
    lda #$01
--
    ldz #$00
-
    sta [ZP.Screen], z
    inz
    bne -
    dex
    beq +
    inc ZP.Screen + 1
    bra --
+

    jsr SetupScreenVector
    rts

SetupScreenVector
    lda #<SCREEN_RAM
    sta ZP.Screen + 0
    lda #>SCREEN_RAM
    sta ZP.Screen + 1
    lda #[SCREEN_RAM >> 16]
    sta ZP.Screen + 2
    lda #$00
    sta ZP.Screen + 3
    rts

SetupColorVector
    lda #<COLOR_RAM
    sta ZP.Color + 0
    lda #>COLOR_RAM
    sta ZP.Color + 1
    lda #[[COLOR_RAM >> 16] & $ff]
    sta ZP.Color + 2
    lda #[[COLOR_RAM >> 24] & $ff]
    sta ZP.Color + 3
    rts

ScreenClear80
    ;DEFAULT SCREEN CLEAR
    lda #$20
    ldx #$00
-
    sta $0800, x
    sta $0900, x
    sta $0a00, x
    sta $0b00, x
    sta $0c00, x
    sta $0d00, x
    sta $0e00, x
    sta $0f00, x
    inx
    bne -
    rts


SetupSystem
    sei
    ; Set memory layout (c64??)
    lda #$35
    sta $01

    +enable40Mhz
    +enableVIC4Registers

    ;Turn off CIA interrupts
    lda #$7f
    sta $dc0d
    sta $dd0d

    ;Turn off raster interrupts, used by C65 rom
    lda #$00
    sta $d01a


    ;Disable C65 rom write protection
    ;$20000 - $3ffff
    lda #$70
    sta $d640
    eom

    cli
    rts

;16 bit char addresses (charnum = addr/64)
;  $000 = $0000
;  $001 = $0040
;  $002 = $0080
;  ...
;  $100 = $4000


* = $4000
  !fill 8, 0
  !fill 8, 0
  !fill 8, 1
  !fill 8, 1
  !fill 8, 2
  !fill 8, 2
  !fill 8, 3
  !fill 8, 3


* = $8127
CHARROM
  !fill 8, $01
  !fill 8, $10