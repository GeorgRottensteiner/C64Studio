!zone VERA

;VRAM address (7:0)
.ADDRx_L            = $9f20

;VRAM address (15:8)
.ADDRx_M            = $9f21

;| Bits 7-4 |    Address Increment
;| Bit  3   |    DECR
;| Bit  2-1 |    unused
;| Bit  0   |    VRAM address (16)
;                Default Value: ? $08/8 (%00001000).
.ADDRx_H            = $9f22

.ADDRESS_INCREASE_1     = $10
.ADDRESS_INCREASE_2     = $20
.ADDRESS_INCREASE_4     = $30
.ADDRESS_INCREASE_8     = $40
.ADDRESS_INCREASE_16    = $50
.ADDRESS_INCREASE_32    = $60
.ADDRESS_INCREASE_64    = $70
.ADDRESS_INCREASE_128   = $80
.ADDRESS_INCREASE_256   = $90
.ADDRESS_INCREASE_512   = $a0
.ADDRESS_INCREASE_40    = $b0
.ADDRESS_INCREASE_80    = $c0
.ADDRESS_INCREASE_160   = $d0
.ADDRESS_INCREASE_320   = $e0
.ADDRESS_INCREASE_640   = $f0
.ADDRESS_DECREASE       = $08

;VRAM data port 0
.DATA0              = $9f23

;VRAM data port 1
.DATA1              = $9f24

;| Bits 7   |    Reset
;| Bit  6-2 |    unused
;| Bit  1   |    DCSEL
;| Bit  0   |    ADDRSEL
;                Default Value: ? $08/8 (%00001000).
.CTRL               = $9f25

.CTRL_ADDRSEL_0   = $00
.CTRL_ADDRSEL_1   = $01
.CTRL_DCSEL_0     = $00
.CTRL_DCSEL_1     = $02
.CTRL_RESET       = $80


;| Bits 7   |    IRQ line (8)
;| Bit  6-4 |    unused
;| Bit  3   |    AFLOW
;| Bit  2   |    SPRCOL
;| Bit  1   |    LINE
;| Bit  0   |    VSYNC
;                Default Value: ? $08/8 (%00001000).
.IEN                = $9f26

;| Bits 7-4 |    Sprite collisions
;| Bit  3   |    AFLOW
;| Bit  2   |    SPRCOL
;| Bit  1   |    LINE
;| Bit  0   |    VSYNC
;                Default Value: ? $08/8 (%00001000).
.ISR                = $9f27

;IRQ line (7:0)
;                Default Value: ? $08/8 (%00001000).
.IRQLINE_L          = $9f28

;| Bits 7   |    Current Field
;| Bit  6   |    Sprites Enable
;| Bit  5   |    Layer 0 enable
;| Bit  4   |    Layer 1 enable
;| Bit  3   |    unused
;| Bit  2   |    Chroma disable
;| Bit  1-0 |    Output mode
;                00 = disabled
;                01 = VGA
;                10 = NTSC composite
;                11 = RGB interlaced, composite sync (VGA)
;(DCSEL=0)
;                Default Value: $31
.DC_VIDEO           = $9f29

.DC_VIDEO_CURRENT_FIELD         = $80
.DC_VIDEO_SPRITES_ENABLE        = $40
.DC_VIDEO_LAYER_0_ENABLE        = $20
.DC_VIDEO_LAYER_1_ENABLE        = $10
.DC_VIDEO_CHROMA_DISABLE        = $04
.DC_VIDEO_OUTPUT_NONE           = $00
.DC_VIDEO_OUTPUT_VGA            = $01
.DC_VIDEO_OUTPUT_NTSC           = $02
.DC_VIDEO_OUTPUT_RGB_INTERLACED = $03

;active display H scale (DCSEL=0)
.DC_HSCALE          = $9f2a

;active display V scale (DCSEL=0)
.DC_VSCALE          = $9f2b

;border color  (DCSEL=0)
.DC_BORDER          = $9f2c

;active display H start (9:2)  (DCSEL=1)
.DC_HSTART          = $9f29

;active display H stop (9:2)  (DCSEL=1)
.DC_HSTOP           = $9f2a

;active display V start (8:1)  (DCSEL=1)
.DC_VSTART          = $9f2b

;active display V stop (8:1)  (DCSEL=1)
.DC_VSTOP           = $9f2c

;| Bits 7-6 |    Map Height
;| Bit  5-4 |    Map Width
;| Bit  3   |    T256C
;| Bits 2   |    Bitmap Mode
;| Bit  1-0 |    Color Depth
;layer 0 config
.L0_CONFIG          = $9f2d

;layer 0 map base address (16:9)
.L0_MAPBASE         = $9f2e

;| Bit  7-2 |    Tile Base Address (16:11)
;| Bits 1   |    Tile Height
;| Bit  0   |    Tile Width
.L0_TILEBASE        = $9f2f

;h-scroll (7:0)
.L0_HSCROLL_L       = $9f30

;| Bit  7-4 |    unused
;| Bits 3-0 |    h-scroll (11:8)
.L0_HSCROLL_H       = $9f31

;v-scroll (7:0)
.L0_VSCROLL_L       = $9f32

;| Bit  7-4 |    unused
;| Bits 3-0 |    v-scroll (11:8)
.L0_VSCROLL_H       = $9f33

;| Bits 7-6 |    Map Height
;| Bit  5-4 |    Map Width
;| Bit  3   |    T256C
;| Bits 2   |    Bitmap Mode
;| Bit  1-0 |    Color Depth
;layer 1 config
.L1_CONFIG          = $9f34

;layer 1 map base address (16:9)
.L1_MAPBASE         = $9f35

;| Bit  7-2 |    Tile Base Address (16:11)
;| Bits 1   |    Tile Height
;| Bit  1   |    Tile Width
.L1_TILEBASE        = $9f36

;h-scroll (7:0)
.L1_HSCROLL_L       = $9f37

;| Bit  7-4 |    unused
;| Bits 3-0 |    h-scroll (11:8)
.L1_HSCROLL_H       = $9f38

;v-scroll (7:0)
.L1_VSCROLL_L       = $9f39

;| Bits 7-4 |    unused
;| Bits 3-0 |    v-scroll (11:8)
.L1_VSCROLL_H       = $9f3a

;| Bit  7   |    FIFO Full/Reset
;| Bit  6   |    FIFO Empty (read only)
;| Bit  5   |    16-bit
;| Bit  4   |    Stereo
;| Bits 3-0 |    PCM Volume
.AUDIO_CTRL         = $9f3b

;PCM Sample Rate
.AUDIO_RATE         = $9f3c

;Audio FIFO data (write only)
.AUDIO_DATA         = $9f3d

;data
.SPI_DATA           = $9f3e

;| Bit  7   |    Busy
;| Bits 6-2 |    unused
;| Bit  1   |    slow clock
;| Bit  0   |    select
.SPI_CTRL           = $9f3f



.SPRITE_MODE_4BPP = $00
.SPRITE_MODE_8BPP = $80

.SPRITE_DISABLE                           = $00
.SPRITE_BETWEEN_BACKGROUND_AND_LAYER_0    = $04
.SPRITE_BETWEEN_LAYER_0_AND_LAYER_1       = $08
.SPRITE_IN_FRONT_OF_LAYER_1               = $0c

.SPRITE_WIDTH_8                           = $00
.SPRITE_WIDTH_16                          = $10
.SPRITE_WIDTH_32                          = $20
.SPRITE_WIDTH_64                          = $30
.SPRITE_HEIGHT_8                          = $00
.SPRITE_HEIGHT_16                         = $40
.SPRITE_HEIGHT_32                         = $80
.SPRITE_HEIGHT_64                         = $c0


.VRAM_PSG_REGISTERS       = $01F9C0

.VRAM_PALETTE             = $01FA00


;128 entries of the following format:
;
;Offset  Bit 7 Bit 6 Bit 5 Bit 4 Bit 3 Bit 2 Bit 1 Bit 0
;0       Address (12:5)
;1       Mode  - Address (16:13)
;2       X (7:0)
;3                        -                    X (9:8)
;4       Y (7:0)
;5                        -                    Y (9:8)
;6       Collision mask            Z-depth  V-flip  H-flip
;7       Sprite height  Sprite width   Palette offset
.VRAM_SPRITE_ATTRIBUTES   = $01FC00

.SPRITE_PALETTE_OFFSET_0    = 0
.SPRITE_PALETTE_OFFSET_16   = 1
.SPRITE_PALETTE_OFFSET_32   = 2
.SPRITE_PALETTE_OFFSET_48   = 3
.SPRITE_PALETTE_OFFSET_64   = 4
.SPRITE_PALETTE_OFFSET_80   = 5
.SPRITE_PALETTE_OFFSET_96   = 6
.SPRITE_PALETTE_OFFSET_112  = 7
.SPRITE_PALETTE_OFFSET_128  = 8
.SPRITE_PALETTE_OFFSET_144  = 9
.SPRITE_PALETTE_OFFSET_160  = 10
.SPRITE_PALETTE_OFFSET_176  = 11
.SPRITE_PALETTE_OFFSET_192  = 12
.SPRITE_PALETTE_OFFSET_208  = 13
.SPRITE_PALETTE_OFFSET_224  = 14
.SPRITE_PALETTE_OFFSET_240  = 15

!zone