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
;| Bit  5   |    Layer 1 enable
;| Bit  4   |    Layer 2 enable
;| Bit  3   |    unused
;| Bit  2   |    Chroma disable
;| Bit  1-0 |    Output mode
;(DCSEL=0)
;                Default Value: ? $08/8 (%00001000).
.DC_VIDEO           = $9f29

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
.L0_CONFIG          = $9f2f

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
.L1_CONFIG          = $9f36

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


!zone