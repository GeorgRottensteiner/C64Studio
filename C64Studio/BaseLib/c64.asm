!zone VIC
.SPRITE_X_POS        = $d000
.SPRITE_Y_POS        = $d001
.SPRITE_X_EXTEND     = $d010

;| Bit  7   |    Raster Position Bit 8 from $D012
;| Bit  6   |    Extended Color Text Mode: 1 = Enable
;| Bit  5   |    Bitmap Mode: 1 = Enable
;| Bit  4   |    Blank Screen to Border Color: 0 = Blank
;| Bit  3   |    Select 24/25 Row Text Display: 1 = 25 Rows
;| Bits 2-0 |    Smooth Scroll to Y Dot-Position (0-7)
;Default Value: $9B/155 (%10011011).
.CONTROL_1           = $d011

.RASTER_POS          = $d012
.STROBE_X            = $d013   ;light pen x
.STROBE_Y            = $d014   ;light pen y
.SPRITE_ENABLE       = $d015

;| Bits 7-6 |    Unused
;| Bit  5   |    Reset-Bit: 1 = Stop VIC (no Video Out, no RAM refresh, no bus access)
;| Bit  4   |    Multi-Color Mode: 1 = Enable (Text or Bitmap)
;| Bit  3   |    Select 38/40 Column Text Display: 1 = 40 Cols
;| Bits 2-0 |    Smooth Scroll to X Dot-Position (0-7)
;                Default Value: $08/8 (%00001000).
.CONTROL_2           = $d016
.SPRITE_EXPAND_Y     = $d017

;| Bits 7-4 |   Video Matrix Base (Screen) Address (inside VIC)
;| Bit  3   |   Bitmap-Mode: Select Base Address (inside VIC)
;| Bits 3-1 |   Character Dot-Data Base (Character set) Address (inside VIC)
;| Bit  0   |   Unused
;Default Value: $14/20 (%00010100).
.MEMORY_CONTROL      = $d018

.IRQ_REQUEST         = $d019

;| Bit 7-4  |   Always 1
;| Bit 3    |   Light-Pen Triggered IRQ Flag
;| Bit 2    |   Sprite to Sprite Collision IRQ Flag     (see $D01E)
;| Bit 1    |   Sprite to Background Collision IRQ Flag (see $D01F)
;| Bit 0    |   Raster Compare IRQ Flag                 (see $D012)
.IRQ_MASK            = $d01a
.SPRITE_PRIORITY     = $d01b
.SPRITE_MULTICOLOR   = $d01c
.SPRITE_EXPAND_X     = $d01d
.SPRITE_COLLISION    = $d01e
.SPRITE_BG_COLLISION = $d01f
.BORDER_COLOR        = $d020
.BACKGROUND_COLOR    = $d021
.CHARSET_MULTICOLOR_1= $d022
.CHARSET_MULTICOLOR_2= $d023
.BACKGROUND_COLOR_3  = $d024
.SPRITE_MULTICOLOR_1 = $d025
.SPRITE_MULTICOLOR_2 = $d026
.SPRITE_COLOR        = $d027
.KEYBOARD_LINES      = $d02f
.CLOCK_SWITCH        = $d030


!zone CIA1

;| Bits 7-0 |   Write Keyboard Column Values for Keyboard Scan
;| Bits 7-6 |   Paddles on: 01 = Port A, 10 = Port B
;| Bit  4   |   Joystick A Fire Button: 0 = Pressed
;| Bit  3   |   Joystick A Right: 0 = Pressed, or Paddle Button
;| Bit  2   |   Joystick A Left : 0 = Pressed, or Paddle Button
;| Bit  1   |   Joystick A Down : 0 = Pressed
;| Bit  0   |   Joystick A Up   : 0 = Pressed
;Do NOT use Paddles on both Ports at the same time!
;Joystick A is the one plugged into Port 2.
;Default Value: $7F/127 (%01111111)
.DATA_PORT_A                = $dc00

;| Bits 7-0 |   Read Keyboard Row Values for Keyboard Scan
;| Bit  4   |   Joystick B Fire Button: 0 = Pressed
;| Bit  3   |   Joystick B Right: 0 = Pressed, or Paddle Button
;| Bit  2   |   Joystick B Left : 0 = Pressed, or Paddle Button
;| Bit  1   |   Joystick B Down : 0 = Pressed
;| Bit  0   |   Joystick B Up   : 0 = Pressed
;Joystick B is the one plugged into Port 1.
.DATA_PORT_B                = $dc01

;| Bit  x   |   1 = Pin PAx set to Output, 0 = Input
;Default Value: $FF/255 (%11111111)
.DATA_DIRECTION_REGISTER_A  = $dc02

;| Bit  x   |   1 = Pin PBx set to Output, 0 = Input
;Default Value: $00/0 (%00000000)
.DATA_DIRECTION_REGISTER_B  = $dc03

;Read : Get current value
;Write: Set Low-Byte of value to count down from
;Default Value: $25/37 on PAL
;               $95/149 on NTSC
.TIMER_A_LO_BYTE            = $dc04

;Read : Get current value
;Write: Set High-Byte of value to count down from
;Default Value: $40/64 on PAL
;               $42/66 on NTSC
.TIMER_A_HI_BYTE            = $dc05

;Read : Get current value
;Write: Set Low-Byte of value to count down from
.TIMER_B_LO_BYTE            = $dc06

;Read : Get current value
;Write: Set High-Byte of value to count down from
.TIMER_B_HI_BYTE            = $dc07

;| Bits 7-4 | Always 0
;| Bits 3-0 | 1/10 Seconds in BCD-Format
.TIME_OF_DAY_CLOCK_10TH_SECONDS= $dc08

;| Bit  7   |   Always 0
;| Bits 6-4 |   10 Seconds in BCD-Format
;| Bits 3-0 |    1 Seconds in BCD-Format
.TIME_OF_DAY_CLOCK_SECONDS  = $dc09

;| Bit  7   |   Always 0
;| Bits 6-4 |   10 Minutes in BCD-Format
;| Bits 3-0 |    1 Minutes in BCD-Format
.TIME_OF_DAY_CLOCK_MINUTES  = $dc0a

;| Bit  7   |   0 = AM / 1 = PM
;| Bit  6-5 |   Always 0
;| Bit  4   |   10 Hours in BCD-Format
;| Bits 3-0 |    1 Hours in BCD-Format
.TIME_OF_DAY_CLOCK_HOURS_APM= $dc0b

;Synchronous Serial I/O Data Buffer
;Bits to be send or received via Pin SP are stored in this register
.SERIAL_SYNC_IO             = $dc0c

;| Bit 7 |   On Read:  1 = Interrupt occured
;|       |   On Write: 1 = Set Int.-Flags, 0 = Clear Int-.Flags
;| Bit 4 |   FLAG1 IRQ (Cassette Read / Serial Bus SRQ Input)
;| Bit 3 |   Serial Port Interrupt ($DC0C full/empty)
;| Bit 2 |   Time-of-Day Clock Alarm Interrupt
;| Bit 1 |   Timer B Interrupt (Tape, Serial Port)
;| Bit 0 |   Timer A Interrupt (Kernal-IRQ, Tape)
;Your CIA does NOT clear this register! You have to do this by simply reading it.
;Default Value: $81/129 (%10000001)
.IRQ_CONTROL                = $dc0d

;| Bit 7 |   Time-of-Day Clock Frequency: 1 = 50 Hz, 0 = 60 Hz
;| Bit 6 |   Serial Port ($DC0C) I/O Mode: 1 = Output, 0 = Input
;| Bit 5 |   Timer A Counts: 1 = CNT Signals, 0 = System 02 Clock
;| Bit 4 |   Force Load Timer A: 1 = Yes
;| Bit 3 |   Timer A Run Mode: 1 = One-Shot, 0 = Continuous
;| Bit 2 |   Timer A Output Mode to PB6: 1 = Toggle, 0 = Pulse
;| Bit 1 |   Timer A Output on PB6: 1 = Yes, 0 = No
;| Bit 0 |   Start/Stop Timer A: 1 = Start, 0 = Stop
;Default Value: $11/17 (%00010001)
.CONTROL_REGISTER_A         = $dc0e

;| Bit  7   |   Set Alarm/TOD-Clock: 1 = Alarm, 0 = Clock
;| Bits 6-5 |   Timer B Mode Select:
;|          |            00 = Count System 02 Clock Pulses
;|          |            01 = Count Positive CNT Transitions
;|          |            10 = Count Timer A Underflow Pulses
;|          |            11 = Count Timer A Underflows While CNT
;| Bit  4   |   Force Load Timer B: 1 = Yes
;| Bit  3   |   Timer B Run Mode: 1 = One-Shot, 0 = Continuous
;| Bit  2   |   Timer B Output Mode to PB7: 1 = Toggle, 0 = Pulse
;| Bit  1   |   Timer B Output on PB7: 1 = Yes, 0 = No
;| Bit  0   |   Start/Stop Timer B: 1 = Start, 0 = Stop
;Default Value: $08/8 (%00001000)
.CONTROL_REGISTER_B         = $dc0f

!zone CIA2
;| Bit  7   |  Serial Bus Data Input
;| Bit  6   |  Serial Bus Clock Pulse Input
;| Bit  5   |  Serial Bus Data Output
;| Bit  4   |  Serial Bus Clock Pulse Output
;| Bit  3   |  Serial Bus ATN Signal Output
;| Bit  2   |  RS232 Data Output (User Port)
;| Bit  1-0 |  VIC Chip System Memory Bank Select (low active!)
;|          |  %00, 0: Bank 3: $C000-$FFFF, 49152-65535
;|          |  %01, 1: Bank 2: $8000-$BFFF, 32768-49151
;|          |  %10, 2: Bank 1: $4000-$7FFF, 16384-32767
;|          |  %11, 3: Bank 0: $0000-$3FFF, 0-16383 (Standard)
;Default Value: $17/23 (%00010111)
.DATA_PORT_A                = $dd00

;| Bit 7 |   User Port PB7 / RS232 Data Set Ready
;| Bit 6 |   User Port PB6 / RS232 Clear to Send
;| Bit 5 |   User Port PB5
;| Bit 4 |   User Port PB4 / RS232 Carrier Detect
;| Bit 3 |   User Port PB3 / RS232 Ring Indicator
;| Bit 2 |   User Port PB2 / RS232 Data Terminal Ready
;| Bit 1 |   User Port PB1 / RS232 Request to Send
;| Bit 0 |   User Port PB0 / RS232 Received Data
.DATA_PORT_B                = $dd01

;| Bit  x   |   1 = Pin PAx set to Output, 0 = Input
;Default Value: $3F/63 (%00111111)
.DATA_DIRECTION_REGISTER_A  = $dd02

;| Bit  x   |   1 = Pin PBx set to Output, 0 = Input
;Default Value: $00/0 (%00000000)
.DATA_DIRECTION_REGISTER_B  = $dd03

;Read : Get current value
;Write: Set Low-Byte of value to count down from
.TIMER_A_LO_BYTE            = $dd04

;Read : Get current value
;Write: Set High-Byte of value to count down from
.TIMER_A_HI_BYTE            = $dd05

;Read : Get current value
;Write: Set Low-Byte of value to count down from
.TIMER_B_LO_BYTE            = $dd06

;Read : Get current value
;Write: Set High-Byte of value to count down from
.TIMER_B_HI_BYTE            = $dd07

;| Bits 7-4 | Always 0
;| Bits 3-0 | 1/10 Seconds in BCD-Format
.TIME_OF_DAY_CLOCK_10TH_SECONDS= $dd08

;| Bit  7   |   Always 0
;| Bits 6-4 |   10 Seconds in BCD-Format
;| Bits 3-0 |    1 Seconds in BCD-Format
.TIME_OF_DAY_CLOCK_SECONDS  = $dd09

;| Bit  7   |   Always 0
;| Bits 6-4 |   10 Minutes in BCD-Format
;| Bits 3-0 |    1 Minutes in BCD-Format
.TIME_OF_DAY_CLOCK_MINUTES  = $dd0a

;| Bit  7   |   0 = AM / 1 = PM
;| Bit  6-5 |   Always 0
;| Bit  4   |   10 Hours in BCD-Format
;| Bits 3-0 |    1 Hours in BCD-Format
.TIME_OF_DAY_CLOCK_HOURS_APM= $dd0b

;Synchronous Serial I/O Data Buffer
;Bits to be send or received via Pin SP are stored in this register
.SERIAL_SYNC_IO             = $dd0c

;| Bit 7 |   On Read:  1 = Interrupt occured
;|       |   On Write: 1 = Set Int.-Flags, 0 = Clear Int.-Flags
;| Bit 4 |   FLAG1 NMI (User/RS232 Received Data Input)
;| Bit 3 |   Serial Port Interrupt ($DD0C full/empty)
;| Bit 2 |   Time-of-Day Clock Alarm Interrupt
;| Bit 1 |   Timer B Interrupt (RS232)
;| Bit 0 |   Timer A Interrupt (RS232)
;Your CIA does NOT clear this register! You have to do this by simply reading it.
;Default Value: $7F/127 (%01111111)
.NMI_CONTROL                = $dd0d

;| Bit 7 |   Time-of-Day Clock Frequency: 1 = 50 Hz, 0 = 60 Hz
;| Bit 6 |   Serial Port ($DD0C) I/O Mode: 1 = Output, 0 = Input
;| Bit 5 |   Timer A Counts: 1 = CNT Signals, 0 = System 02 Clock
;| Bit 4 |   Force Load Timer A: 1 = Yes
;| Bit 3 |   Timer A Run Mode: 1 = One-Shot, 0 = Continuous
;| Bit 2 |   Timer A Output Mode to PB6: 1 = Toggle, 0 = Pulse
;| Bit 1 |   Timer A Output on PB6: 1 = Yes, 0 = No
;| Bit 0 |   Start/Stop Timer A: 1 = Start, 0 = Stop
;Default Value: $08/8 (%00001000)
.CONTROL_REGISTER_A         = $dd0e

;| Bit  7   |   Set Alarm/TOD-Clock: 1 = Alarm, 0 = Clock
;| Bits 6-5 |   Timer B Mode Select:
;|          |            00 = Count System 02 Clock Pulses
;|          |            01 = Count Positive CNT Transitions
;|          |            10 = Count Timer A Underflow Pulses
;|          |            11 = Count Timer A Underflows While CNT
;| Bit  4   |   Force Load Timer B: 1 = Yes
;| Bit  3   |   Timer B Run Mode: 1 = One-Shot, 0 = Continuous
;| Bit  2   |   Timer B Output Mode to PB7: 1 = Toggle, 0 = Pulse
;| Bit  1   |   Timer B Output on PB7: 1 = Yes, 0 = No
;| Bit  0   |   Start/Stop Timer B: 1 = Start, 0 = Stop
;Default Value: $08/8 (%00001000)
.CONTROL_REGISTER_B         = $dd0f

IRQ_RETURN_KERNAL           = $ea81
IRQ_RETURN_KERNAL_KEYBOARD  = $ea31

JOYSTICK_PORT_II        = $dc00
JOYSTICK_PORT_I         = $dc01


PROCESSOR_PORT          = $01

KERNAL_IRQ_LO           = $fffe
KERNAL_IRQ_HI           = $ffff



!zone SID
.BASE                           = $d400
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