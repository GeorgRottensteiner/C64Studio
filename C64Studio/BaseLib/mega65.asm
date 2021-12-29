!cpu m65

!zone VIC3


;write $A5 and $96 there to enable VIC 3
;write 'G' and 'S' there to enable VIC 4
.KEY            = $d02f

;xxxx xxx1      CRAM2K        $1F800 -   $1FFFF,  $D800 - $DFFF, writeable
;                           $FF80000 - $FF807FF
;                           map 2nd KB of colour RAM to $DC00-$DFFF
;xxxx xx1x      EXTSYNC
;xxxx x1xx      PAL         Use PALETTE ROM or RAM entries for colours 0 - 15
;xxxx 1xxx      ROM8        $38000 - $39FFF,      $8000 - $9FFF, not writeable
;                           map C65 ROM to $8000
;xxx1 xxxx      ROMA        $3A000 - $3BFFF,      $A000 - $BFFF, not writeable
;                           map C65 ROM to $A000
;xx1x xxxx      ROMC        $2C000 - $2CFFF,      $C000 - $CFFF, not writeable
;                           map C65 ROM to $C000
;x1xx xxxx      CROM9       $29000 - $29FFF,      $D000 - $DFFF, not writeable
;                           select between C64 and C65 charset
;1xxx xxxx      ROME        $3E000 - $3FFFF,      $E000 - $FFFF, not writeable
;                           map C65 ROM to $E000
.ROMBANK        = $d030

;1xxx xxxx      H640        0 = use 40 character screen width
;x1xx xxxx      FAST            enable C65 FAST mode (∼3.5MHz)
;xx1x xxxx      ATTR            enable extended attributes and 8 bit colour entries
;xxx1 xxxx      BPM             bit plane mode
;xxxx 1xxx      V400            enable 400 vertical pixels
;xxxx x1xx      H1280           enable 1280 horizontal pixels (not implemented)
;xxxx xx1x      MONO            enable VIC-III MONO video output (not implemented)
;xxxx xxx1      INT             enable VIC-III interlaced mode
.VICDIS         = $d031



!zone VIC4

;half pixel offset (for 320)
;default value = $50
.TEXTXPOS           = $d04c

;1111 xxxx      = SPRTILEN
;xxxx 1111      = TEXTXPOS high nibble
.SPRTILEN_TEXTXPOS  = $d04d

.TEXTYPOS           = $d04e

;1111 xxxx      = SPRTILEN
;xxxx 1111      = TEXTYPOS high nibble
.SPRTILEN_TEXTYPOS  = $d04f

;1xxx xxxx      = NORRDEL       when clear, raster rewrite double buffering is used
;x1xx xxxx      = DBLRR         when set, the Raster Rewrite Buffer is only updated every 2nd raster line,
;                               limiting resolution to V200, but allowing more cycles for Raster-Rewrite actions.
;xx11 1111      = XPOS          Read horizontal raster scan position LSB
.NORRDEL_DBLRR_XPOS = $d051

;VIC4 display settings
;1xxx xxxx      = ALPHEN        alpha compositor enable
;x1xx xxxx      = VFAST         C65GS FAST mode (48MHz)
;xx1x xxxx      = PALEMU        video output pal simulation
;xxx1 xxxx      = SPR640    1 = sprite 640 resolution enabled
;xxxx 1xxx      = SMTH          enable video output horizontal smoothing
;xxxx x1xx      = FCLRHI    1 = enable full color for chars > $ff
;xxxx xx1x      = FCLRLO    1 = enable full color for chars <= $ff
;xxxx xxx1      = CHR16     1 = enable 16 bit character numbers
.VIC4DIS        = $d054

;enable sprite wide mode (bit = sprite index)
.SPRX64EN       = $d057

;number of bytes per text screen line lo
.CHARSTEP_LO    = $d058

;number of bytes per text screen line hi
.CHARSTEP_HI    = $d059

.CHRXSCL        = $d05a

.CHRYSCL        = $d05b

;width of single side border
.SIDBDRWD       = $d05c

;disable hot registers
;1xxx xxxx = HOTREG         1 = disable writing VIC2 registers affecting VIC4 registers
;x1xx xxxx = RSTDELEN       1 = enable raster delay by one line to match output pipeline
;xx11 1111 = SIDBDRWD         = width of single side border
.HOTREG         = $d05d

;number of characters to display per row
.CHRCOUNT       = $d05e

;3 byte address of screen ram
;$d060 = lo byte
;$d061 = medium byte
;$d062 = hi byte
.SCRNPTR        = $d060
.SCRNPTR_LO     = $d060
.SCRNPTR_HI     = $d061
.SCRNPTR_BANK   = $d062

;1xxx xxxx = EXGLYPH
;x0xx xxxx = unused
;xx11 xxxx = msb of CHRCOUNT
;xxxx 1111 = msb of SCRNPTR
.EXGLYPH_CHRCOUNT_SCRNPTR = $d063

;16 bit address offset of color RAM
.COLPTR_LO        = $d064
.COLPTR_HI        = $d065

;3 byte address of chargen position
;$d068 = lo byte
;$d069 = medium byte
;$d06a = hi byte
.CHARPTR        = $d068

;enable sprite 16 color mode (bit = sprite index)
.SPR16EN        = $d06b

;sprite pointer list address lo
.SPRPTRADR_LO   = $d06c

;sprite pointer list address hi
.SPRPTRADR_HI   = $d06d

;1xxx xxxx =   SPRPTR16  - enable 16 bit sprite addresses
;x111 1111 =   SPRPTRBNK - sprite pointer bank
.SPRPTR16       = $d06e

;1xxx xxxx =  PALNTSC   - 1 = NTSC emulation mode
;x1xx xxxx =  VGAHDTD   - 1 = select more VGA compatible output
;xx11 1111 =  RASLINE0      = first VIC2 raster line
.PALNTSC_VGAHDTV_RASLINE0 = $d06f

;11xx xxxx = MAPEDPAL     - palette bank mapped at $d100 to $d3ff
;xx11 xxxx = BTPALSEL     - bitmap/text palette bank
;xxxx 11xx = SPRPALSEL    - sprite palette bank
;xxxx xx11 = ABTPALSEL    - VIC4 alternative bitmap/text palette bank
.PALSEL         = $d070

;palette red entries (up to $d1ff) - nibbles are switched in 8bit color mode
.PALRED         = $d100

;palette green entries (up to $d2ff) - nibbles are switched in 8bit color mode
.PALGREEN       = $d200

;palette blue entries (up to $d3ff) - nibbles are switched in 8bit color mode
.PALBLUE        = $d300



!zone FLOPPY

;1xxx xxxx = IRQ          - controller has generated an IRQ (read only)
;x1xx xxxx = LED          - LED blinks if set
;xx1x xxxx = MOTOR        - activates drive motor and LED
;xxx1 xxxx = SWAP         - swap upper and lower halves of data buffer (invert bit 8 of sector buffer)
;xxxx 1xxx = SIDE         - select side of disk
;xxxx x111 = DS           - drive select
;                           internal drive = 0
;                           internal cable drive = 1
.MOTORLED       = $d080

.CMDSTEPDIR     = $d081
.BUSYCRC        = $d082
.REQIRQ         = $d083
.TRACK          = $d084
.SECTOR         = $d085
.SIDE           = $d086
.DATA           = $d087
.CLOCK          = $d088
.STEP           = $d089
.PCODE          = $d08a


!zone SID2
.BASE                           = $d420
.FREQUENCY_LO_1                 = .BASE + 0
.FREQUENCY_HI_1                 = .BASE + 1
.PULSE_WIDTH_LO_1               = .BASE + 2
.PULSE_WIDTH_HI_1               = .BASE + 3
.CONTROL_WAVE_FORM_1            = .BASE + 4
.ATTACK_DECAY_1                 = .BASE + 5
.SUSTAIN_RELEASE_1              = .BASE + 6

.FREQUENCY_LO_2                 = .BASE + 7
.FREQUENCY_HI_2                 = .BASE + 8
.PULSE_WIDTH_LO_2               = .BASE + 9
.PULSE_WIDTH_HI_2               = .BASE + 10
.CONTROL_WAVE_FORM_2            = .BASE + 11
.ATTACK_DECAY_2                 = .BASE + 12
.SUSTAIN_RELEASE_2              = .BASE + 13

.FREQUENCY_LO_3                 = .BASE + 14
.FREQUENCY_HI_3                 = .BASE + 15
.PULSE_WIDTH_LO_3               = .BASE + 16
.PULSE_WIDTH_HI_3               = .BASE + 17
.CONTROL_WAVE_FORM_3            = .BASE + 18
.ATTACK_DECAY_3                 = .BASE + 19
.SUSTAIN_RELEASE_3              = .BASE + 20

.FILTER_CUTOFF_LO               = .BASE + 21
.FILTER_CUTOFF_HI               = .BASE + 22

.FILTER_RESONANCE_VOICE_INPUT   = .BASE + 23
.FILTER_MODE_VOLUME             = .BASE + 24

.AD_CONVERTER_PADDLE_1          = .BASE + 25
.AD_CONVERTER_PADDLE_2          = .BASE + 26

.OSCILLATOR_3_OUTPUT            = .BASE + 27
.ENV_GENERATOR_3_OUTPUT         = .BASE + 28

!zone SID3
.BASE                           = $d440
.FREQUENCY_LO_1                 = .BASE + 0
.FREQUENCY_HI_1                 = .BASE + 1
.PULSE_WIDTH_LO_1               = .BASE + 2
.PULSE_WIDTH_HI_1               = .BASE + 3
.CONTROL_WAVE_FORM_1            = .BASE + 4
.ATTACK_DECAY_1                 = .BASE + 5
.SUSTAIN_RELEASE_1              = .BASE + 6

.FREQUENCY_LO_2                 = .BASE + 7
.FREQUENCY_HI_2                 = .BASE + 8
.PULSE_WIDTH_LO_2               = .BASE + 9
.PULSE_WIDTH_HI_2               = .BASE + 10
.CONTROL_WAVE_FORM_2            = .BASE + 11
.ATTACK_DECAY_2                 = .BASE + 12
.SUSTAIN_RELEASE_2              = .BASE + 13

.FREQUENCY_LO_3                 = .BASE + 14
.FREQUENCY_HI_3                 = .BASE + 15
.PULSE_WIDTH_LO_3               = .BASE + 16
.PULSE_WIDTH_HI_3               = .BASE + 17
.CONTROL_WAVE_FORM_3            = .BASE + 18
.ATTACK_DECAY_3                 = .BASE + 19
.SUSTAIN_RELEASE_3              = .BASE + 20

.FILTER_CUTOFF_LO               = .BASE + 21
.FILTER_CUTOFF_HI               = .BASE + 22

.FILTER_RESONANCE_VOICE_INPUT   = .BASE + 23
.FILTER_MODE_VOLUME             = .BASE + 24

.AD_CONVERTER_PADDLE_1          = .BASE + 25
.AD_CONVERTER_PADDLE_2          = .BASE + 26

.OSCILLATOR_3_OUTPUT            = .BASE + 27
.ENV_GENERATOR_3_OUTPUT         = .BASE + 28

!zone SID4
.BASE                           = $d460
.FREQUENCY_LO_1                 = .BASE + 0
.FREQUENCY_HI_1                 = .BASE + 1
.PULSE_WIDTH_LO_1               = .BASE + 2
.PULSE_WIDTH_HI_1               = .BASE + 3
.CONTROL_WAVE_FORM_1            = .BASE + 4
.ATTACK_DECAY_1                 = .BASE + 5
.SUSTAIN_RELEASE_1              = .BASE + 6

.FREQUENCY_LO_2                 = .BASE + 7
.FREQUENCY_HI_2                 = .BASE + 8
.PULSE_WIDTH_LO_2               = .BASE + 9
.PULSE_WIDTH_HI_2               = .BASE + 10
.CONTROL_WAVE_FORM_2            = .BASE + 11
.ATTACK_DECAY_2                 = .BASE + 12
.SUSTAIN_RELEASE_2              = .BASE + 13

.FREQUENCY_LO_3                 = .BASE + 14
.FREQUENCY_HI_3                 = .BASE + 15
.PULSE_WIDTH_LO_3               = .BASE + 16
.PULSE_WIDTH_HI_3               = .BASE + 17
.CONTROL_WAVE_FORM_3            = .BASE + 18
.ATTACK_DECAY_3                 = .BASE + 19
.SUSTAIN_RELEASE_3              = .BASE + 20

.FILTER_CUTOFF_LO               = .BASE + 21
.FILTER_CUTOFF_HI               = .BASE + 22

.FILTER_RESONANCE_VOICE_INPUT   = .BASE + 23
.FILTER_MODE_VOLUME             = .BASE + 24

.AD_CONVERTER_PADDLE_1          = .BASE + 25
.AD_CONVERTER_PADDLE_2          = .BASE + 26

.OSCILLATOR_3_OUTPUT            = .BASE + 27
.ENV_GENERATOR_3_OUTPUT         = .BASE + 28


!zone Mega65
;read the last key pressed (until written to)
.PRESSED_KEY    = $d610

.HTRAP00        = $d640



!zone DMA

;DMAgic DMA list address LSB, and trigger DMA (when written to)
.ADDRLSB_TRIG   = $d700

;DMA list address high byte (address bits 8 – 15)
.ADDRMSB        = $d701

;ADDRBANK DMA list address bank (address bits 16 - 22)
;                               writing clears $d704
.ADDRBANK       = $d702

;xxxx xxx1      - enable F018B mode (sub command byte)
.EN018B         = $d703

;DMA list address mega-byte
.ADDRMB         = $d704

;DMAgic DMA list address LSB, and trigger enhanced DMA job
.ETRIG          = $d705

.COMMAND_COPY         = $00
.COMMAND_MIX          = $01   ;via MINTERMs
.COMMAND_SWAP         = $02
.COMMAND_FILL         = $03

.ADDRESSING_LINEAR    = $00   ;Linear (normal) addressing
.ADDRESSING_MODULO    = $01   ;Modulo (rectangular) addressing
.ADDRESSING_HOLD      = $02   ;Hold (constant address)
.ADDRESSING_XY_MOD    = $03   ;XY MOD (bitmap rectangular) addressing



!macro Enable40Mhz
          lda #$41
          sta $00
!end

!macro EnableVIC3Registers {
          lda #$00
          tax
          tay
          taz
          map
          eom

          ;Enable VIC III
          lda #$A5
          sta VIC3.KEY
          lda #$96
          sta VIC3.KEY
}

!macro EnableVIC4Registers {
          ; Enable VIC4 registers
          lda #$00
          tax
          tay
          taz
          map
          eom

          ;Enable VIC IV
          lda #$47        ;'G'
          sta VIC3.KEY
          lda #$53        ;'S'
          sta VIC3.KEY
}


!macro DisableC65ROM {
          ;Disable C65 rom protection using
          ;hypervisor trap (see mega65 manual)
          lda #$70
          sta $d640
          eom

          ; Unmap C65 Roms $d030 by clearing bits 3-7
          lda #%11111000
          trb $d030
}

!macro DMACacheEnable {
  poke32bit $BFFFFF2, $E0
}


!macro poke32bit addr, val {
    RunDMAJob job
    bra +
  job
    +DMAHeader $00, addr >> 20
    +DMAFillJob val, addr, 1, 0
  +
}

!macro RunDMAJob JobPointer {
    lda #[JobPointer >> 16]
    sta $d702
    sta $d704
    lda #>JobPointer
    sta $d701
    lda #<JobPointer
    sta $d705
}

!macro DMAHeader SourceBank, DestBank {
    !byte $0A ; Request format is F018A
    !byte $80, SourceBank
    !byte $81, DestBank
}

!macro DMACopyJob Source, Destination, Length, Chain, Backwards {
  !byte $00 ;No more options
  !if Chain = 1 {
    !byte $04 ;Copy and chain
  } else {
    !byte $00 ;Copy and last request
  }

  .backByte = 0
  !if (Backwards) {
    .backByte = $40
    Source = Source + Length - 1
    Destination = Destination + Length - 1
  }
  !word Length ;Size of Copy

  ;byte 04
  !word Source & $ffff
  !byte [Source >> 16] + .backByte

  ;byte 07
  !word Destination & $ffff
  !byte [[Destination >> 16] & $0f]  + .backByte
  ; .if(Chain) {
  !word $0000
  ; }
}

!macro DMAFillJob SourceByte, Destination, Length, Chain {
  !byte $00 ;No more options
  !if Chain = 1 {
    !byte $07 ;Fill and chain
  } else {
    !byte $03 ;Fill and last request
  }

  !word Length ;Size of Copy
  ;byte 4
  !word SourceByte
  !byte $00
  ;byte 7
  !word Destination & $ffff
  !byte [[Destination >> 16] & $0f]
  !if(Chain) {
    !word $0000
  }
}