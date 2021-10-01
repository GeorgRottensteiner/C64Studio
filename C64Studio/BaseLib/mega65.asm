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


;3 byte address of screen ram
;$d060 = lo byte
;$d061 = medium byte
;$d062 = hi byte
.SCRNPTR        = $d060

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



!zone Mega65
;read the last key pressed (until written to)
.PRESSED_KEY    = $d610

.HTRAP00        = $d640

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
