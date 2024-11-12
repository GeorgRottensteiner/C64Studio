;BASIC start at $1000 (unexpanded)
;BASIC start at $1200 (expanded)


!zone VIC
;| Bit  7   |    Interlace scan
;| Bit  0-6 |    Horizontal centering
;Default Value = $0B
.HORIZONTAL_CENTERING   = $9000

;| Bit  0-7 |    Vertical centering
;Default Value = $26
.VERTICAL_CENTERING     = $9001

;| Bit  7   |    part of video matrix address
;|          |    if set, Video RAM is at $1E00
;|          |    if clear, Video RAM is at $1000
;| Bit  0-6 |    set # of columns (default 22)
;Default Value = $96
.COLUMNS_ADDRESS        = $9002

;| Bit  7   |    lowest bit of raster counter
;| Bit  1-6 |    set # of rows (default 23)
;| Bit  0   |    8x8 or 8x16 chars
;Default value = $2E
.ROWS_WIDE_CHARS      = $9003

.RASTER_POS           = $9004

;| Bit  4-7 | is rest of video address    (default = F)
;| Bit  0-3 | start of character memory   (default = 0)
;BITS 3,2,1,0 character memory starting address
;             HEX   DEC
;0000   ROM   8000  32768
;0001         8400  33792
;0010         8800  34816
;0011         8C00  35840
;1000   RAM   0000  00000
;1001 xxxx
;1010 xxxx  unavail.
;1011 xxxx
;1100         1000   4096
;1101         1400   5120
;1110         1800   6144
;1111         1C00   7168
.CHAR_MEMORY          = $9005

.LIGHT_PEN_X          = $9006

.LIGHT_PEN_Y          = $9007

.PADDLE_X             = $9008

.PADDLE_Y             = $9009

;Frequency for oscillator 1 (low)
; (on: 128-255)
.SID_FREQUENCY_1      = $900a

;Frequency for oscillator 2 (medium)
; (on: 128-255)
.SID_FREQUENCY_2      = $900b

;Frequency for oscillator 3 (high)
; (on: 128-255)
.SID_FREQUENCY_3      = $900c

;Frequency of noise source
.SID_FREQUENCY_NOISE  = $900d

;| Bit  4-7 | auxiliary color information
;| Bit  0-3 | sets volume of all sound
.AUX_COLOR_VOLUME     = $900e

;| Bits 4-7 | background color
;| Bit  3   | inverted (0) or normal mode (1)
;| Bits 0-2 | border color
.COLORS               = $900f



.COLOR_RAM            = $9600
.COLOR_RAM_EXPANDED   = $9400