!zone TED
.TIMER1LO             = $ff00    ;Timer/Counter #1: low bits
.TIMER1HI             = $ff01    ;Timer/Counter #1: high bits
.TIMER2LO             = $ff02    ;Timer/Counter #2: low bits
.TIMER2HI             = $ff03    ;Timer/Counter #2: high bits
.TIMER3LO             = $ff04    ;Timer/Counter #3: low bits
.TIMER3HI             = $ff05    ;Timer/Counter #3: high bits

;| Bit  7   |    Test
;| Bit  6   |    Extended Color Text Mode: 1 = Enable
;| Bit  5   |    Bitmap Mode: 1 = Enable
;| Bit  4   |    Blank Screen to Border Color: 0 = Blank
;| Bit  3   |    Select 24/25 Row Text Display: 1 = 25 Rows
;| Bits 2-0 |    Smooth Scroll to Y Dot-Position (0-7)
;Default Value: $1B/27 (%00011011)
.CONFIG_1             = $ff06

;| Bits 7   |    RVSDIS Inverse/256 characters flag (0 = default, 128 characters + inverse)
;| Bits 6   |    NTSC/PAL (0 = PAL, 1 = NTSC)
;| Bit  5   |    Reset-Bit: 1 = Stop TED (no Video Out, no counters)
;| Bit  4   |    Multi-Color Mode: 1 = Enable (Text or Bitmap)
;| Bit  3   |    Select 38/40 Column Text Display: 1 = 40 Cols
;| Bits 2-0 |    Smooth Scroll to X Dot-Position (0-7)
;Default Value PAL:  $08 (%00001000)
;Default Value NTSC: $48 (%01001000)
.CONFIG_2             = $ff07

;keyboard input latch, Giving a strobe - writing to the register, the latch stores the values
; of the input-lines. Then, we can read them from this register
.KEYBOARD             = $ff08

;| Bit 7    |   IRQ       Interrupt occurred
;| Bit 6    |   TIMER3IF  Counter #3
;| Bit 5    |   unused
;| Bit 4    |   TIMER2IF  Counter #2
;| Bit 3    |   TIMER1IF  Counter #1
;| Bit 2    |   ILP       Lightpen, not implemented
;| Bit 1    |   IRST      Raster counter
;| Bit 0    |   unused
.IRQST                = $ff09

;| Bit 7    |   unused
;| Bit 6    |   TIMER3IE  Counter #3
;| Bit 5    |   unused
;| Bit 4    |   TIMER2IE  Counter #2
;| Bit 3    |   TIMER1IE  Counter #1
;| Bit 2    |   LPIE      Lightpen, not implemented
;| Bit 1    |   ERST      Raster counter
;| Bit 0    |   RSTCMP8   9th bit of $ff0b
.IRQEN                = $ff0a

.RSTCMP               = $ff0b    ;raster interrupt bits 0 to 7

;| Bits 7-2 |   unused
;| Bits 1-0 |   Cursor position: high 2 bits
;Default Value: $14/20 (%00010100) ??
.CURSORHI             = $ff0c

;| Bits 7-0 |   Cursor position: low 8 bits
;Default Value: $14/20 (%00010100) ??
.CURSORLO             = $ff0d

.SND1FREQLO           = $ff0e    ;sound channel #1 frequency: low 8 bits
.SND2FREQLO           = $ff0f    ;sound channel #2 frequency: low 8 bits

;| Bits 7-2 |   unused
;| Bits 1-0 |   sound channel #2 frequency: high 2 bits
.SND2FREQHI           = $ff10

;| Bit  7   |   SNDDC     D/A mode
;| Bit  6   |   SND2NOISE Sound #2 noise on/off
;| Bit  5   |   SND2ON    Sound #2 square-wave on/off
;| Bit  4   |   SND1ON    Sound #1 on/off
;| Bits 3-0 |   Volume    Max value is 8
.SOUND_CONTROL        = $ff11

;| Bits 7-6 |   Unused
;| Bits 5-3 |   Bitmap Address
;| Bit  2   |   BMP+Char ROM/RAM
;| Bits 1-0 |   Sound channel #1 frequency: high 2 bits
;Default Value: $14/20 (%00010100) ??
.BITMAP               = $ff12

;| Bits 7-2 |   Character generator address
;| Bit  1   |   Single clock mode
;| Bit  0   |   Actual ROM/RAM config (read only)
;Default Value: $14/20 (%00010100) ??
.CHARGEN              = $ff13

;| Bits 7-3 |   Screen memory address
;| Bits 2-0 |   Unused
;Default Value: $14/20 (%00010100) ??
.SCREEN_MEMORY        = $ff14

;| Bit  7   |   Unused
;| Bits 6-4 |   Luminance
;| Bits 3-0 |   Chroma
;Default Value: $14/20 (%00010100) ??
.BACKGROUND_COLOR_0   = $ff15

;| Bit  7   |   Unused
;| Bits 6-4 |   Luminance
;| Bits 3-0 |   Chroma
;Default Value: $14/20 (%00010100) ??
.BACKGROUND_COLOR_1   = $ff16

;| Bit  7   |   Unused
;| Bits 6-4 |   Luminance
;| Bits 3-0 |   Chroma
;Default Value: $14/20 (%00010100) ??
.BACKGROUND_COLOR_2   = $ff17

;| Bit  7   |   Unused
;| Bits 6-4 |   Luminance
;| Bits 3-0 |   Chroma
;Default Value: $14/20 (%00010100) ??
.BACKGROUND_COLOR_3   = $ff18

;| Bit  7   |   Unused
;| Bits 6-4 |   Luminance
;| Bits 3-0 |   Chroma
;Default Value: $14/20 (%00010100) ??
.BORDER               = $ff19

;| Bits 7-2 |   Unused
;| Bits 1-0 |   Start position of character row: high 2 bits
;Default Value: $14/20 (%00010100) ??
.STCHPOSHI            = $ff1a

;| Bits 7-0 |   Start position of character row: low 8 bits
;Default Value: $14/20 (%00010100) ??
.STCHPOSLO            = $ff1b

;| Bits 7-1 |   Unused
;| Bit    0 |   Current vertical raster position: high 1 bit
.RASTER_POS_HI        = $ff1c

;| Bits 7-0 |   Current vertical raster position: low 8 bits
.RASTER_POS           = $ff1d

;| Bits 7-1 |   Current horizontal raster position: high 7 bits
;| Bit    0 |   Unused
.LINE_POS             = $ff1e

;| Bit    7 |   Unused
;| Bits 6-3 |   Flash counter
;| Bits 2-0 |   Actual rasterline in a character row
.FLASH_RAST           = $ff1f

;switch to ROM on $8000..$ffff area
.SWITCH_ROM           = $ff3e

;switch to RAM on $8000..$ffff area
.SWITCH_RAM           = $ff3f


!zone KERNAL
;SCINIT. Initialize VIC; restore default input/output to keyboard/screen; clear screen; set PAL/NTSC switch and interrupt timer.
;Input: –
;Output: –
;Used registers: A, X, Y.
;Real address: $FF5B.
.SCINIT           = $ff81

;IOINIT. Initialize CIA's, SID volume; setup memory configuration; set and start interrupt timer.
;Input: –
;Output: –
;Used registers: A, X.
;Real address: $FDA3.
.IOINIT           = $ff84

;RAMTAS. Clear memory addresses $0002-$0101 and $0200-$03FF; run memory test and set start and end address of BASIC work area accordingly; set screen memory to $0400 and datasette buffer to $033C.
;Input: –
;Output: –
;Used registers: A, X, Y.
;Real address: $FD50.
.RAMTAS           = $ff87

;RESTOR. Fill vector table at memory addresses $0314-$0333 with default values.
;Input: –
;Output: –
;Used registers: –
;Real address: $FD15.
.RESTOR           = $ff8a

;VECTOR. Copy vector table at memory addresses $0314-$0333 from or into user table.
;Input: Carry: 0 = Copy user table into vector table, 1 = Copy vector table into user table; X/Y = Pointer to user table.
;Output: –
;Used registers: A, Y.
;Real address: $FD1A.
.VECTOR           = $ff8d

;SETMSG. Set system error display switch at memory address $009D.
;Input: A = Switch value.
;Output: –
;Used registers: –
;Real address: $FE18.
.SETMSG           = $ff90

;LSTNSA. Send LISTEN secondary address to serial bus. (Must call LISTEN beforehands.)
;Input: A = Secondary address.
;Output: –
;Used registers: A.
;Real address: $EDB9.
.LSTNSA           = $ff93

;TALKSA. Send TALK secondary address to serial bus. (Must call TALK beforehands.)
;Input: A = Secondary address.
;Output: –
;Used registers: A.
;Real address: $EDC7.
.TALKSA           = $ff96

;MEMBOT. Save or restore start address of BASIC work area.
;Input: Carry: 0 = Restore from input, 1 = Save to output; X/Y = Address (if Carry = 0).
;Output: X/Y = Address (if Carry = 1).
;Used registers: X, Y.
;Real address: $FE25.
.MEMBOT           = $ff99

;MEMTOP. Save or restore end address of BASIC work area.
;Input: Carry: 0 = Restore from input, 1 = Save to output; X/Y = Address (if Carry = 0).
;Output: X/Y = Address (if Carry = 1).
;Used registers: X, Y.
;Real address: $FE34.
.MEMTOP           = $ff9c

;SCNKEY. Query keyboard; put current matrix code into memory address $00CB, current status of shift keys into memory address $028D and PETSCII code into keyboard buffer.
;Input: –
;Output: –
;Used registers: A, X, Y.
;Real address: $EA87.
.SCNKEY           = $ff9f

;SETTMO. Unknown. (Set serial bus timeout.)
;Input: A = Timeout value.
;Output: –
;Used registers: –
;Real address: $FE21.
.SETTMO           = $ffa2

;IECIN. Read byte from serial bus. (Must call TALK and TALKSA beforehands.)
;Input: –
;Output: A = Byte read.
;Used registers: A.
;Real address: $EE13.
.IECIN            = $ffa5

;IECOUT. Write byte to serial bus. (Must call LISTEN and LSTNSA beforehands.)
;Input: A = Byte to write.
;Output: –
;Used registers: –
;Real address: $EDDD.
.IECOUT           = $ffa8

;UNTALK. Send UNTALK command to serial bus.
;Input: –
;Output: –
;Used registers: A.
;Real address: $EDEF.
.UNTALK           = $ffab

;UNLSTN. Send UNLISTEN command to serial bus.
;Input: –
;Output: –
;Used registers: A.
;Real address: $EDFE.
.UNLSTN           = $ffae

;LISTEN. Send LISTEN command to serial bus.
;Input: A = Device number.
;Output: –
;Used registers: A.
;Real address: $ED0C.
.LISTEN           = $ffb1

;TALK. Send TALK command to serial bus.
;Input: A = Device number.
;Output: –
;Used registers: A.
;Real address: $ED09.
.TALK             = $ffb4

;READST. Fetch status of current input/output device, value of ST variable. (For RS232, status is cleared.)
;Input: –
;Output: A = Device status.
;Used registers: A.
;Real address: $FE07.
.READST           = $ffb7

;SETLFS. Set file parameters.
;Input: A = Logical number; X = Device number; Y = Secondary address.
;Output: –
;Used registers: –
;Real address: $FE00.
.SETLFS           = $ffba

;SETNAM. Set file name parameters.
;Input: A = File name length; X/Y = Pointer to file name.
;Output: –
;Used registers: –
;Real address: $FDF9.
.SETNAM           = $ffbd

;OPEN. Open file. (Must call SETLFS and SETNAM beforehands.)
;Input: –
;Output: –
;Used registers: A, X, Y.
;Real address: ($031A), $F34A.
.OPEN             = $ffc0

;CLOSE. Close file.
;Input: A = Logical number.
;Output: –
;Used registers: A, X, Y.
;Real address: ($031C), $F291.
.CLOSE            = $ffc3

;CHKIN. Define file as default input. (Must call OPEN beforehands.)
;Input: X = Logical number.
;Output: –
;Used registers: A, X.
;Real address: ($031E), $F20E.
.CHKIN            = $ffc6

;CHKOUT. Define file as default output. (Must call OPEN beforehands.)
;Input: X = Logical number.
;Output: –
;Used registers: A, X.
;Real address: ($0320), $F250.
.CHKOUT           = $ffc9

;CLRCHN. Close default input/output files (for serial bus, send UNTALK and/or UNLISTEN); restore default input/output to keyboard/screen.
;Input: –
;Output: –
;Used registers: A, X.
;Real address: ($0322), $F333.
.CLRCHN           = $ffcc

;CHRIN. Read byte from default input (for keyboard, read a line from the screen). (If not keyboard, must call OPEN and CHKIN beforehands.)
;Input: –
;Output: A = Byte read.
;Used registers: A, Y.
;Real address: ($0324), $F157.
.CHRIN            = $ffcf

;CHROUT. Write byte to default output. (If not screen, must call OPEN and CHKOUT beforehands.)
;Input: A = Byte to write.
;Output: –
;Used registers: –
;Real address: ($0326), $F1CA.
.CHROUT           = $ffd2

;LOAD. Load or verify file. (Must call SETLFS and SETNAM beforehands.)
;Input: A: 0 = Load, 1-255 = Verify; X/Y = Load address (if secondary address = 0).
;Output: Carry: 0 = No errors, 1 = Error; A = KERNAL error code (if Carry = 1); X/Y = Address of last byte loaded/verified (if Carry = 0).
;Used registers: A, X, Y.
;Real address: $F49E.
.LOAD             = $ffd5

;SAVE. Save file. (Must call SETLFS and SETNAM beforehands.)
;Input: A = Address of zero page register holding start address of memory area to save; X/Y = End address of memory area plus 1.
;Output: Carry: 0 = No errors, 1 = Error; A = KERNAL error code (if Carry = 1).
;Used registers: A, X, Y.
;Real address: $F5DD.
.SAVE             = $ffd8

;SETTIM. Set Time of Day, at memory address $00A0-$00A2.
;Input: A/X/Y = New TOD value.
;Output: –
;Used registers: –
;Real address: $F6E4.
.SETTIM           = $ffdb

;RDTIM. read Time of Day, at memory address $00A0-$00A2.
;Input: –
;Output: A/X/Y = Current TOD value.
;Used registers: A, X, Y.
;Real address: $F6DD.
.RDTIM            = $ffde

;STOP. Query Stop key indicator, at memory address $0091; if pressed, call CLRCHN and clear keyboard buffer.
;Input: –
;Output: Zero: 0 = Not pressed, 1 = Pressed; Carry: 1 = Pressed.
;Used registers: A, X.
;Real address: ($0328), $F6ED.
.STOP             = $ffe1

;GETIN. Read byte from default input. (If not keyboard, must call OPEN and CHKIN beforehands.)
;Input: –
;Output: A = Byte read. 0 = no key
;Used registers: A, X, Y.
;Real address: ($032A), $F13E.
.GETIN            = $ffe4

;CLALL. Clear file table; call CLRCHN.
;Input: –
;Output: –
;Used registers: A, X.
;Real address: ($032C), $F32F.
.CLALL            = $ffe7

;UDTIM. Update Time of Day, at memory address $00A0-$00A2, and Stop key indicator, at memory address $0091.
;Input: –
;Output: –
;Used registers: A, X.
;Real address: $F69B.
.UDTIM            = $ffea

;SCREEN. Fetch number of screen rows and columns.
;Input: –
;Output: X = Number of columns (40); Y = Number of rows (25).
;Used registers: X, Y.
;Real address: $E505.
.SCREEN           = $ffed

;PLOT. Save or restore cursor position.
;Input: Carry: 0 = Restore from input, 1 = Save to output; X = Cursor column (if Carry = 0); Y = Cursor row (if Carry = 0).
;Output: X = Cursor column (if Carry = 1); Y = Cursor row (if Carry = 1).
;Used registers: X, Y.
;Real address: $E50A.
.PLOT             = $fff0

;IOBASE. Fetch CIA #1 base address.
;Input: –
;Output: X/Y = CIA #1 base address ($DC00).
;Used registers: X, Y.
;Real address: $E500.
.IOBASE           = $fff3

!zone