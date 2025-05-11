;***********************************************************************************;
;***********************************************************************************;
;
; The almost completely commented VIC 20 ROM disassembly. V1.01 Lee Davison 2005-2012.
; With enhancements by Simon Rowe <srowe@mose.org.uk>.

; This is a bit correct assembly listing for the VIC 20 BASIC and KERNAL ROMs as one 16K
; ROM. You should be able to assemble the VIC ROMs from this with most 6502 assemblers,
; as no macros or 'special' features were used. This has been tested using Michal
; Kowalski's 6502 Simulator assemble function. See http://exifpro.com/utils.html for
; this program.

; Many references were used to complete this disassembly including, but not limited to,
; "Mapping the VIC 20", "Mapping the C64", "VIC 20 Programmers Reference", "VIC 20 User
; Guide", "The Complete Commodore Inner Space Anthology", "VIC Revealed" and various
; text files, pictures and other documents.


;***********************************************************************************;
;***********************************************************************************;
;
; BASIC zero page

; These locations contain the JMP instruction target address of the USR command. They
; are initialised so that if you try to execute a USR call without changing them you
; will receive an ILLEGAL QUANTITY error message.

USRPPOK = $00     ; USR() JMP instruction
ADDPRC  = $01     ; USR() vector

; This vector points to the address of the BASIC routine which converts a floating point
; number to an integer, however BASIC does not use this vector. It may be of assistance
; to the programmer who wishes to use data that is stored in floating point format. The
; parameter passed by the USR command is available only in that format for example.

ADRAY1  = $03     ; float to fixed vector

; This vector points to the address of the BASIC routine which converts an integer to a
; floating point number, however BASIC does not use this vector. It may be used by the
; programmer who needs to make such a conversion for a machine language program that
; interacts with BASIC. To return an integer value with the USR command for example.

ADRAY2  = $05     ; fixed to float vector

; The cursor column position prior to the TAB or SPC is moved here from PNTR, and is used
; to calculate where the cursor ends up after one of these functions is invoked.

; Note that the value contained here shows the position of the cursor on a logical line.
; Since one logical line can be up to four physical lines long, the value stored here
; can range from 0 to 87.

CHARAC  = $07     ; search character
ENDCHR  = $08     ; scan quotes flag
TRMPOS  = $09     ; TAB column save

; The routine that converts the text in the input buffer into lines of executable program
; tokens, and the routines that link these program lines together, use this location as an
; index into the input buffer area. After the job of converting text to tokens is done,
; the value in this location is equal to the length of the tokenised line.

; The routines which build an array or locate an element in an array use this location to
; calculate the number of DIMensions called for and the amount of storage required for a
; newly created array, or the number of subscripts when referencing an array element.

VERCHK  = $0A     ; load/verify flag, 0 = load, 1 = verify
COUNT = $0B     ; temporary byte, line crunch/array access/logic operators

; This is used as a flag by the routines that build an array or reference an existing
; array. It is used to determine whether a variable is in an array, whether the array
; has already been DIMensioned, and whether a new array should assume the default size.

DIMFLG  = $0C     ; DIM flag

; This flag is used to indicate whether data being operated upon is string or numeric. A
; value of $FF in this location indicates string data while a $00 indicates numeric data.

VALTYP  = $0D     ; data type flag, $FF = string, $00 = numeric

; If the above flag indicates numeric then a $80 in this location identifies the number
; as an integer, and a $00 indicates a floating point number.

INTFLG  = $0E     ; data type flag, $80 = integer, $00 = floating point

; The garbage collection routine uses this location as a flag to indicate that garbage
; collection has already been tried before adding a new string. If there is still not
; enough memory, an OUT OF MEMORY error message will result.

; LIST uses this byte as a flag to let it know when it has come to a character string in
; quotes. It will then print the string, rather than search it for BASIC keyword tokens.

; This location is also used during the process of converting a line of text in the BASIC
; input buffer into a linked program line of BASIC keyword tokens to flag a DATA line is
; being processed.

GARBFL  = $0F     ; garbage collected/open quote/DATA flag

; If an opening parenthesis is found, this flag is set to indicate that the variable in
; question is either an array variable or a user-defined function.

SUBFLG  = $10     ; subscript/FNx flag

; This location is used to determine whether the sign of the value returned by the
; functions SIN, COS, ATN or TAN is positive or negative.

; Also the comparison routines use this location to indicate the outcome of the compare.
; For A <=> B the value here will be $01 if A > B, $02 if A = B, and $04 if A < B. If
; more than one comparison operator was used to compare the two variables then the value
; here will be a combination of the above values.

INPFLG  = $11     ; input mode flag, $00 = INPUT, $40 = GET, $98 = READ
TANSGN  = $12     ; ATN sign/comparison evaluation flag

; When the default input or output device is used the value here will be a zero, and the
; format of prompting and output will be the standard screen output format. The location
; $B8 is used to decide what device actually to put input from or output to.

CHANNL  = $13     ; current I/O channel

; Used whenever a 16 bit integer is used e.g. the target line number for GOTO, LIST, ON,
; and GOSUB also the number of a BASIC line that is to be added or replaced. Additionally
; PEEK, POKE, WAIT, and SYS use this location as a pointer to the address which is the
; subject of the command.

LINNUM  = $14     ; temporary integer low byte
;   $15     ; temporary integer high byte

; This location points to the next available slot in the temporary string descriptor
; stack located at TEMPST.

TEMPPT  = $16     ; descriptor stack pointer, next free

; This contains information about temporary strings which have not yet been assigned to
; a string variable.

LASTPT  = $17     ; current descriptor stack item pointer low byte
;   $18     ; current descriptor stack item pointer high byte
TEMPST  = $19     ; to $21, descriptor stack

; These locations are used by BASIC multiplication and division routines. They are also
; used by the routines which compute the size of the area required to store an array
; which is being created.

INDEX = $22     ; misc temp byte
;   $23     ; misc temp byte
;   $24     ; misc temp byte
;   $25     ; misc temp byte

RESHO = $26     ; temp mantissa 1
;   $27     ; temp mantissa 2
;   $28     ; temp mantissa 3
;   $29     ; temp mantissa 4

; Word pointer to where the BASIC program text is stored.

TXTTAB  = $2B     ; start of memory low byte
;   $2C     ; start of memory high byte

; Word pointer to the start of the BASIC variable storage area.

VARTAB  = $2D     ; start of variables low byte
;   $2E     ; start of variables high byte

; Word pointer to the start of the BASIC array storage area.

ARYTAB  = $2F     ; end of variables low byte
;   $30     ; end of variables high byte

; Word pointer to end of the start of free RAM.

STREND  = $31     ; end of arrays low byte
;   $32     ; end of arrays high byte

; Word pointer to the bottom of the string text storage area.

FRETOP  = $33     ; bottom of string space low byte
;   $34     ; bottom of string space high byte

; Used as a temporary pointer to the most current string added by the routines which
; build strings or move them in memory.

FRESPC  = $35     ; string utility ptr low byte
;   $36     ; string utility ptr high byte

; Word pointer to the highest address used by BASIC +1.

MEMSIZ  = $37     ; end of memory low byte
;   $38     ; end of memory high byte

; These locations contain the line number of the BASIC statement which is currently being
; executed. A value of $FF in location $3A means that BASIC is in immediate mode.

CURLIN  = $39     ; current line number low byte
;   $3A     ; current line number high byte

; When program execution ends or stops the last line number executed is stored here.

OLDLIN  = $3B     ; break line number low byte
;   $3C     ; break line number high byte

; These locations contain the address of the start of the text of the BASIC statement
; that is being executed. The value of the pointer to the address of the BASIC text
; character currently being scanned is stored here each time a new BASIC statement begins
; execution.

OLDTXT  = $3D     ; continue pointer low byte
;   $3E     ; continue pointer high byte

; These locations hold the line number of the current DATA statement being READ. If an
; error concerning the DATA occurs this number will be moved to CURLIN so that the error
; message will show the line that contains the DATA statement rather than in the line that
; contains the READ statement.

DATLIN  = $3F     ; current DATA line number low byte
;   $40     ; current DATA line number high byte

; These locations point to the address where the next DATA will be READ from. RESTORE
; sets this pointer back to the address indicated by the start of BASIC pointer.

DATPTR  = $41     ; DATA pointer low byte
;   $42     ; DATA pointer high byte

; READ, INPUT and GET all use this as a pointer to the address of the source of incoming
; data, such as DATA statements, or the text input buffer.

INPPTR  = $43     ; READ pointer low byte
;   $44     ; READ pointer high byte

VARNAM  = $45     ; current variable name first byte
;   $46     ; current variable name second byte

; These locations point to the value of the current BASIC variable. Specifically they
; point to the byte just after the two-character variable name.

VARPNT  = $47     ; current variable address low byte
;   $48     ; current variable address high byte

; The address of the BASIC variable which is the subject of a FOR/NEXT loop is first
; stored here before being pushed onto the stack.

FORPNT  = $49     ; FOR/NEXT variable pointer low byte
;   $4A     ; FOR/NEXT variable pointer high byte

; The expression evaluation routine creates this to let it know whether the current
; comparison operation is a < $01, = $02 or > $04 comparison or combination.

OPPTR = $4B     ; BASIC execute pointer temporary low byte/precedence flag
;   $4C     ; BASIC execute pointer temporary high byte
OPMASK  = $4D     ; comparison evaluation flag

; These locations are used as a pointer to the function that is created during function
; definition. During function execution it points to where the evaluation results should
; be saved.

DEFPNT  = $4E     ; FAC temp store/function/variable/garbage pointer low byte
;   $4F     ; FAC temp store/function/variable/garbage pointer high byte

; Temporary pointer to the current string descriptor.

DSCPTN  = $50     ; FAC temp store/descriptor pointer low byte
;   $51     ; FAC temp store/descriptor pointer high byte

FOUR6 = $53     ; garbage collection step size

; The first byte is the 6502 JMP instruction $4C, followed by the address of the required
; function taken from the table at FUNDSP.

JMPER = $54     ; JMP opcode for functions
;   $55     ; functions jump vector

TEMPF3  = $57     ; FAC temp store
GENPTR  = $58     ; FAC temp store
;   $59     ; FAC temp store
GEN2PTR = $5A     ; FAC temp store
;   $5B     ; block end high byte
LAB_5C  = $5C     ; FAC temp store
LAB_5D  = $5D     ; FAC temp store
EXPCNT  = $5E     ; exponent count byte
;   $5F     ; FAC temp store
TMPPTR  = $5F
;   $60     ; block start high byte
FAC1  = $61     ; FAC1 exponent
;   $62     ; FAC1 mantissa 1
;   $63     ; FAC1 mantissa 2
;   $64     ; FAC1 mantissa 3
;   $65     ; FAC1 mantissa 4
;   $66     ; FAC1 sign
SGNFLG  = $67     ; constant count/-ve flag
BITS  = $68     ; FAC1 overflow
FAC2  = $69     ; FAC2 exponent
;   $6A     ; FAC2 mantissa 1
;   $6B     ; FAC2 mantissa 2
;   $6C     ; FAC2 mantissa 3
;   $6D     ; FAC2 mantissa 4
;   $6E     ; FAC2 sign
ARISGN  = $6F     ; FAC sign comparison
FACOV = $70     ; FAC1 rounding
FBUFPT  = $71     ; temp BASIC execute/array pointer low byte/index
;   $72     ; temp BASIC execute/array pointer high byte

CHRGET  = $73     ; increment and scan memory, BASIC byte get
CHRGOT  = $79     ; scan memory, BASIC byte get
;   $7A     ; BASIC execute pointer low byte
;   $7B     ; BASIC execute pointer high byte
CHRSPC  = $80     ; numeric test entry

RNDX  = $8B     ; RND() seed, five bytes


; KERNAL zero page

STATUS  = $90     ; I/O status byte
          ; function
          ; bit cassette    serial bus
          ; --- --------    ----------
          ;  7  end of tape   device not present
          ;  6  end of file   EOI
          ;  5  checksum error
          ;  4  read error
          ;  3  long block
          ;  2  short block
          ;  1        time out read
          ;  0        time out write
STKEY = $91     ; keyboard row, bx = 0 = key down
          ; bit key
          ; --- ------
          ;  7  [DOWN]
          ;  6  /
          ;  5  ,
          ;  4  N
          ;  3  V
          ;  2  X
          ;  1  [L SHIFT]
          ;  0  [STOP]
SVXT  = $92     ; timing constant for tape read
VERCK = $93     ; load/verify flag, load = $00, verify = $01
C3PO  = $94     ; serial output: deferred character flag
          ; $00 = no character waiting, $xx = character waiting
BSOUR = $95     ; serial output: deferred character
          ; $FF = no character waiting, $xx = waiting character
SYNO  = $96     ; tape: leader length
          ; $00 = no block, $10-$7E = leader bits read
XSAV  = $97     ; register save

; The number of currently open I/O files is stored here. The maximum number that can be
; open at one time is ten. The number stored here is used as the index to the end of the
; tables that hold the file numbers, device numbers, and secondary addresses (LAT, FAT,
; SAT).

LDTND = $98     ; open file count

; The default value of this location is 0.

DFLTN = $99     ; input device number

; The default value of this location is 3.

DFLTO = $9A     ; output device number
          ;
          ; number  device
          ; ------  ------
          ;  0    keyboard
          ;  1    cassette
          ;  2    RS-232
          ;  3    screen
          ;  4-31   serial bus

PRTY  = $9B     ; tape character parity
DPSW  = $9C     ; tape dipole switch/byte received flag
MSGFLG  = $9D     ; KERNAL message mode flag,
          ; $C0 = both control and error messages,
          ; $80 = control messages only,
          ; $40 = error messages only,
          ; $00 = neither control nor error messages
PTR1  = $9E     ; tape pass 1 error log/character buffer
PTR2  = $9F     ; tape pass 2 error log corrected

; These three locations form a counter which is updated 60 times a second, and serves as
; a software clock which counts the number of jiffies that have elapsed since the computer
; was turned on. After 24 hours and one jiffy these locations are set back to $000000.

TIME  = $A0     ; jiffy clock high byte
;   $A1     ; jiffy clock mid byte
;   $A2     ; jiffy clock low byte

PCNTR = $A3     ; serial input bit count/tape bit count

; b0 of this location reflects the current phase of the tape output cycle.

FIRT  = $A4     ; input byte/tape bit cycle phase
CNTDN = $A5     ; tape synchronisation byte count/serial bus bit count
BUFPNT  = $A6     ; tape buffer index
INBIT = $A7     ; tape write leader count/block count/RS-232 input bit
BITCI = $A8     ; tape error flags/tape long word marker/RS-232 input bit count
RINONE  = $A9     ; tape dipole count/tape medium word marker/RS-232 start bit flag,
          ; $90 = no start bit received,
          ; $00 = start bit received
RIDATA  = $AA     ; tape input status/tape sync status/RS-232 byte assembly
RIPRTY  = $AB     ; tape leader counter/tape read checksum/RS-232 parity bit
SAL = $AC     ; tape buffer start pointer low byte
        ; next/previous line character pointer low byte
;   $AD     ; tape buffer start pointer high byte
        ; next/previous line character pointer high byte
EAL = $AE     ; tape buffer end pointer low byte
        ; next/previous line colour pointer low byte
;   $AF     ; tape buffer end pointer high byte
        ; next/previous line colour pointer high byte
CMP0  = $B0     ; tape timing constant min byte
;   $B1     ; tape timing constant max byte

; These two locations point to the address of the cassette buffer. This pointer must
; be greater than or equal to $0200 or an ILLEGAL DEVICE NUMBER error will be sent
; when tape I/O is tried. This pointer must also be less that $8000 or the routine
; will terminate early.

TAPE1 = $B2     ; tape buffer start pointer low byte
;   $B3     ; tape buffer start pointer high byte

; RS-232 routines use this to count the number of bits transmitted and for parity and
; stop bit manipulation. Tape load routines use this location to flag when they are
; ready to receive data bytes.

BITTS = $B4     ; transmitter bit count out

; This location is used by the RS-232 routines to hold the next bit to be sent and by the
; tape routines to indicate what part of a block the read routine is currently reading.

NXTBIT  = $B5     ; transmitter next bit to be sent

; RS-232 routines use this area to disassemble each byte to be sent from the transmission
; buffer pointed to by ROBUF. Tape load routines use this location to record read errors.

RODATA  = $B6     ; transmitter byte buffer/disassembly location

; Disk filenames may be up to 16 characters in length while tape filenames may be up to
; 187 characters in length.

; If a tape name is longer than 16 characters the excess will be truncated by the
; SEARCHING and FOUND messages, but will still be present on the tape.

; A disk file is always referred to by a name. This location will always be greater than
; zero if the current file is a disk file.

; An RS-232 OPEN command may specify a filename of up to four characters. These characters
; are copied to M51CTR to M51CTR+3 and determine baud rate, word length, and parity, or
; they would do if the feature was fully implemented.

FNLEN = $B7     ; file name length

LA  = $B8     ; logical file
SA  = $B9     ; secondary address
FA  = $BA     ; current device number
          ; number  device
          ; ------  ------
          ;  0    keyboard
          ;  1    cassette
          ;  2    RS-232
          ;  3    screen
          ;  4-31   serial bus
FNADR = $BB     ; file name pointer low byte
;   $BC     ; file name pointer high byte
ROPRTY  = $BD     ; tape write byte/RS-232 parity byte

; Used by the tape routines to count the number of copies of a data block remaining to
; be read or written.

FSBLK = $BE     ; tape copies remaining
MYCH  = $BF     ; tape read byte
CAS1  = $C0     ; tape motor interlock
STAL  = $C1     ; I/O start address low byte
;   $C2     ; I/O start address high byte
MEMUSS  = $C3     ; load start address low byte
;   $C4     ; load start address high byte
LSTX  = $C5     ; last key pressed
          ;
          ;  # key   # key     # key     # key
          ; -- ---  -- ---    -- ---    -- ---
          ; 00 1    10 none   20 [SPACE]  30 Q
          ; 01 3    11 A    21 Z    31 E
          ; 02 5    12 D    22 C    32 T
          ; 03 7    13 G    23 B    33 U
          ; 04 9    14 J    24 M    34 O
          ; 05 +    15 L    25 .    35 @
          ; 06 Â£     16 ;    26 none   36 ^
          ; 07 [DEL]  17 [CSR R]  27 [F1]   37 [F5]
          ; 08 [<-] 18 [STOP] 28 none   38 2
          ; 09 W    19 none   29 S    39 4
          ; 0A R    1A X    2A F    3A 6
          ; 0B Y    1B V    2B H    3B 8
          ; 0C I    1C N    2C K    3C 0
          ; 0D P    1D ,    2D :    3D -
          ; 0E *    1E /    2E =    3E [HOME]
          ; 0F [RET]  1F [CSR D]  2F [F3]   3F [F7]

NDX = $C6     ; keyboard buffer length/index

; When the [CTRL][RVS-ON] characters are printed this flag is set to $12, and the print
; routines will add $80 to the screen code of each character which is printed, so that
; the character will appear on the screen with its colours reversed.

; Note that the contents of this location are cleared not only upon entry of a
; [CTRL][RVS-OFF] character but also at every carriage return.

RVS = $C7     ; reverse flag, $12 = reverse, $00 = normal

; This pointer indicates the column number of the last non-blank character on the logical
; line that is to be input. Since a logical line can be up to 88 characters long this
; number can range from 0-87.

INDX  = $C8     ; input EOL pointer

; These locations keep track of the logical line that the cursor is on and its column
; position on that logical line.

; Each logical line may contain up to four 22 column physical lines. So there may be as
; many as 23 logical lines, or as few as 6 at any one time. Therefore, the logical line
; number might be anywhere from 1-23. Depending on the length of the logical line, the
; cursor column may be from 1-22, 1-44, 1-66 or 1-88.

; For more on logical lines, see the description of the screen line link table, LDTB1.

LXSP  = $C9     ; input cursor row
;   $CA     ; input cursor column

; The keyscan interrupt routine uses this location to indicate which key is currently
; being pressed. The value here is then used as an index into the appropriate keyboard
; table to determine which character to print when a key is struck.

; The correspondence between the key pressed and the number stored here is as follows:

; $00 1 $10 not used  $20 [SPACE]   $30 Q $40 [NO KEY]
; $01 3 $11 A   $21 Z   $31 E $xx invalid
; $02 5 $12 D   $22 C   $32 T
; $03 7 $13 G   $23 B   $33 U
; $04 9 $14 J   $24 M   $34 O
; $05 + $15 L   $25 .   $35 @
; $06 Â£  $16 ;   $26 not used  $36 ^
; $07 [DEL] $17 [CRSR R]  $27 [F1]    $37 [F5]
; $08 [<-]  $18 [STOP]    $28 not used  $38 2
; $09 W $19 not used  $29 S   $39 4
; $0A R $1A X   $2A F   $3A 6
; $0B Y $1B V   $2B H   $3B 8
; $0C I $1C N   $2C K   $3C 0
; $0D P $1D ,   $2D :   $3D -
; $0E * $1E /   $2E =   $3E [HOME]
; $0F [RET] $1F [CRSR D]  $2F [F3]    $3F [F7]

SFDX  = $CB     ; which key

; When this flag is set to a non-zero value, it indicates to the routine that normally
; flashes the cursor not to do so. The cursor blink is turned off when there are
; characters in the keyboard buffer, or when the program is running.

BLNSW = $CC     ; cursor enable, $00 = flash cursor

; The routine that blinks the cursor uses this location to tell when it's time for a
; blink. The number 20 is put here and decremented every jiffy until it reaches zero.
; Then the cursor state is changed, the number 20 is put back here, and the cycle starts
; all over again.

BLNCT = $CD     ; cursor timing countdown

; The cursor is formed by printing the inverse of the character that occupies the cursor
; position. If that characters is the letter A, for example, the flashing cursor merely
; alternates between printing an A and a reverse-A. This location keeps track of the
; normal screen code of the character that is located at the cursor position, so that it
; may be restored when the cursor moves on.

CDBLN = $CE     ; character under cursor

; This location keeps track of whether, during the current cursor blink, the character
; under the cursor was reversed, or was restored to normal. This location will contain
; $00 if the character is reversed, and $01 if the character is not reversed.

BLNON = $CF     ; cursor blink phase

CRSW  = $D0     ; input from keyboard or screen, $xx = input is available
          ; from the screen, $00 = input should be obtained from the
          ; keyboard

; These locations point to the address in screen RAM of the first column of the logical
; line upon which the cursor is currently positioned.

PNT = $D1     ; current screen line pointer low byte
;   $D2     ; current screen line pointer high byte

; This holds the cursor column position within the logical line pointed to by PNT.
; Since a logical line can comprise up to four physical lines, this value may be from
; 0 to 87.

PNTR  = $D3     ; cursor column

; A non-zero value in this location indicates that the editor is in quote mode. Quote
; mode is toggled every time that you type in a quotation mark on a given line, the
; first quote mark turns it on, the second turns it off, the third turns it on, etc.

; If the editor is in this mode when a cursor control character or other non-printing
; character is entered, a printed equivalent will appear on the screen instead of the
; cursor movement or other control operation taking place. Instead, that action is
; deferred until the string is sent to the string by a PRINT statement, at which time
; the cursor movement or other control operation will take place.

; The exception to this rule is the DELETE key, which will function normally within
; quote mode. The only way to print a character which is equivalent to the DELETE key
; is by entering insert mode. Quote mode may be exited by printing a closing quote or
; by hitting the RETURN or SHIFT-RETURN keys.

QTSW  = $D4     ; cursor quote flag

; The line editor uses this location when the end of a line has been reached to determine
; whether another physical line can be added to the current logical line or if a new
; logical line must be started.

LNMX  = $D5     ; current screen line length

; This location contains the current physical screen line position of the cursor, 0 to 22.

TBLX  = $D6     ; cursor row

; The ASCII value of the last character printed to the screen is held here temporarily.

ASCII = $D7     ; checksum byte/temporary last character

; When the INSERT key is pressed, the screen editor shifts the line to the right, allocates
; another physical line to the logical line if necessary (and possible), updates the
; screen line length in LNMX, and adjusts the screen line link table at LDTB1. This location
; is used to keep track of the number of spaces that has been opened up in this way.

; Until the spaces that have been opened up are filled, the editor acts as if in quote
; mode. See location QTSW, the quote mode flag. This means that cursor control characters
; that are normally non-printing will leave a printed equivalent on the screen when
; entered, instead of having their normal effect on cursor movement, etc. The only
; difference between insert and quote mode is that the DELETE key will leave a printed
; equivalent in insert mode, while the INSERT key will insert spaces as normal.

INSRT = $D8     ; insert count

; This table contains 23 entries, one for each row of the screen display. Each entry has
; two functions. Bits 0-3 indicate on which of the four pages of screen memory the first
; byte of memory for that row is located. This is used in calculating the pointer to the
; starting address of a screen line at PNT.
;
; The high byte is calculated by adding the value of the starting page of screen memory
; held in HIBASE to the displacement page held here.
;
; The other function of this table is to establish the makeup of logical lines on the
; screen. While each screen line is only 22 characters long, BASIC allows the entry of
; program lines that contain up to 88 characters. Therefore, some method must be used
; to determine which physical lines are linked into a longer logical line, so that this
; longer logical line may be edited as a unit.
;
; The high bit of each byte here is used as a flag by the screen editor. That bit is set
; when a line is the first or only physical line in a logical line. The high bit is reset
; to 0 only when a line is an extension to this logical line.

LDTB1 = $D9     ; to LDTB1 + $18 inclusive, screen line link table

LLNKSV  = $F2     ; screen row marker

; This pointer is synchronised with the pointer to the address of the first byte of
; screen RAM for the current line kept in PNT. It holds the address of the first byte
; of colour RAM for the corresponding screen line.

USER  = $F3     ; colour RAM pointer low byte
;   $F4     ; colour RAM pointer high byte

; This pointer points to the address of the keyboard matrix lookup table currently being
; used. Although there are only 64 keys on the keyboard matrix, each key can be used to
; print up to four different characters, depending on whether it is struck by itself or
; in combination with the SHIFT, CTRL, or C= keys.

; These tables hold the ASCII value of each of the 64 keys for one of these possible
; combinations of keypresses. When it comes time to print the character, the table that
; is used determines which character is printed.

; The addresses of the tables are:

; NORMKEYS      ; unshifted
; SHFTKEYS      ; shifted
; LOGOKEYS      ; commodore
; CTRLKEYS      ; control

KEYTAB  = $F5     ; keyboard pointer low byte
;   $F6     ; keyboard pointer high byte

; When device the RS-232 channel is opened two buffers of 256 bytes each are created at
; the top of memory. These locations point to the address of the one which is used to
; store characters as they are received.

RIBUF = $F7     ; RS-232 Rx pointer low byte
;   $F8     ; RS-232 Rx pointer high byte

; These locations point to the address of the 256 byte output buffer that is used for
; transmitting data to RS-232 devices.

ROBUF = $F9     ; RS-232 Tx pointer low byte
;   $FA     ; RS-232 Tx pointer high byte

; $FB to $FE - unused

BASZPT  = $FF     ; FAC1 to string output base

STACK = $0100     ; bottom of the stack page

CHNLNK  = $01FC     ; chain link pointer high byte
; = $01FD     ; chain link pointer low byte

PREVLN  = $01FE     ; line number low byte before crunched line
;   $01FF     ; line number high byte before crunched line

BUF = $0200     ; input buffer, for some routines the byte before the input
        ; buffer needs to be set to a specific value for the routine
        ; to work correctly

LAT = $0259     ; .. to $0262 logical file table
FAT = $0263     ; .. to $026C device number table
SAT = $026D     ; .. to $0276 secondary address table
KEYD  = $0277     ; .. to $0280 keyboard buffer
MEMSTR  = $0281     ; OS start of memory low byte
;   $0282     ; OS start of memory high byte
MEMHIGH = $0283     ; OS top of memory low byte
;   $0284     ; OS top of memory high byte
TIMOUT  = $0285     ; IEEE-488 bus timeout flag ( unused )
COLOR = $0286     ; current colour code
GDCOL = $0287     ; colour under cursor
HIBASE  = $0288     ; screen memory page
XMAX  = $0289     ; maximum keyboard buffer size
RPTFLG  = $028A     ; key repeat. $80 = repeat all, $40 = repeat none,
          ; $00 = repeat cursor movement keys, insert/delete
          ; key and the space bar
KOUNT = $028B     ; repeat speed counter
DELAY = $028C     ; repeat delay counter

; This flag signals which of the SHIFT, CTRL, or C= keys are currently being pressed.

; A value of $01 signifies that one of the SHIFT keys is being pressed, a $02 shows that
; the C= key is down, and $04 means that the CTRL key is being pressed. If more than one
; key is held down, these values will be added e.g. $03 indicates that SHIFT and C= are
; both held down.

; Pressing the SHIFT and C= keys at the same time will toggle the character set that is
; presently being used between the uppercase/graphics set, and the lowercase/uppercase
; set.

; While this changes the appearance of all of the characters on the screen at once it
; has nothing whatever to do with the keyboard shift tables and should not be confused
; with the printing of SHIFTed characters, which affects only one character at a time.

SHFLAG  = $028D     ; keyboard shift/control flag
          ; bit key(s) 1 = down
          ; --- ---------------
          ; 7-3 unused
          ;  2  CTRL
          ;  1  C=
          ;  0  SHIFT

; This location, in combination with the one above, is used to debounce the special
; SHIFT keys. This will keep the SHIFT/C= combination from changing character sets
; back and forth during a single pressing of both keys.

LSTSHF  = $028E     ; SHIFT/CTRL/C= keypress last pattern

; This location points to the address of the Operating System routine which actually
; determines which keyboard matrix lookup table will be used.

; The routine looks at the value of the SHIFT flag at SHFLAG, and based on what value
; it finds there, stores the address of the correct table to use at location KEYTAB.

KEYLOG  = $028F     ; keyboard decode logic pointer low byte
;   $0290     ; keyboard decode logic pointer high byte

; This flag is used to enable or disable the feature which lets you switch between the
; uppercase/graphics and upper/lowercase character sets by pressing the SHIFT and
; Commodore logo keys simultaneously.

MODE  = $0291     ; shift mode switch, $00 = enabled, $80 = locked

; This location is used to determine whether moving the cursor past the ??xx column of
; a logical line will cause another physical line to be added to the logical line.

; A value of 0 enables the screen to scroll the following lines down in order to add
; that line; any nonzero value will disable the scroll.

; This flag is set to disable the scroll temporarily when there are characters waiting
; in the keyboard buffer, these may include cursor movement characters that would
; eliminate the need for a scroll.

AUTODN  = $0292     ; screen scrolling flag, $00 = enabled

M51CTR  = $0293     ; pseudo 6551 control register. the first character of
          ; the OPEN RS-232 filename will be stored here
          ; bit function
          ; --- --------
          ;  7  2 stop bits/1 stop bit
          ; 65  word length
          ; --- -----------
          ; 00  8 bits
          ; 01  7 bits
          ; 10  6 bits
          ; 11  5 bits
          ;  4  unused
          ; 3210  baud rate
          ; ----  ---------
          ; 0000  user rate *
          ; 0001     50
          ; 0010     75
          ; 0011    110
          ; 0100    134.5
          ; 0101    150
          ; 0110    300
          ; 0111    600
          ; 1000   1200
          ; 1001   1800
          ; 1010   2400
          ; 1011   3600
          ; 1100   4800 *
          ; 1101   7200 *
          ; 1110   9600 *
          ; 1111  19200 * * = not implemented
M51CDR  = $0294     ; pseudo 6551 command register. the second character of
          ; the OPEN RS-232 filename will be stored here
          ; bit function
          ; --- --------
          ; 765 parity
          ; --- ------
          ; xx0 disabled
          ; 001 odd
          ; 011 even
          ; 101 mark
          ; 111 space
          ;  4  duplex half/full
          ;  3  unused
          ;  2  unused
          ;  1  unused
          ;  0  handshake - X line/3 line
;LAB_0295 = $0295   ; Nonstandard Bit Timing low byte. the third character
          ; of the OPEN RS-232 filename will be stored here
;LAB_0296 = $0296   ; Nonstandard Bit Timing high byte. the fourth character
          ; of the OPEN RS-232 filename will be stored here
RSSTAT  = $0297     ; RS-232 status register
          ; bit function
          ; --- --------
          ;  7  break
          ;  6  no DSR detected
          ;  5  unused
          ;  4  no CTS detected
          ;  3  unused
          ;  2  Rx buffer overrun
          ;  1  framing error
          ;  0  parity error
BITNUM  = $0298     ; number of bits to be sent/received
BAUDOF  = $0299     ; time of one bit cell low byte
;   $029A     ; time of one bit cell high byte
RIDBE = $029B     ; index to Rx buffer end
RIDBS = $029C     ; index to Rx buffer start
RODBS = $029D     ; index to Tx buffer start
RODBE = $029E     ; index to Tx buffer end
IRQTMP  = $029F     ; saved IRQ low byte
;   $02A0     ; saved IRQ high byte

; $02A1 to $02FF - unused

IERROR  = $0300     ; BASIC vector - print error message
IMAIN = $0302     ; BASIC vector - main command processor
ICRNCH  = $0304     ; BASIC vector - tokenise keywords
IQPLOP  = $0306     ; BASIC vector - list program
IGONE = $0308     ; BASIC vector - execute next command
IEVAL = $030A     ; BASIC vector - get value from line

; Before every SYS command each of the registers is loaded with the value found in the
; corresponding storage address. Upon returning to BASIC with an RTS instruction, the new
; value of each register is stored in the appropriate storage address.

; This feature allows you to place the necessary values into the registers from BASIC
; before you SYS to a KERNAL or BASIC ML routine. It also enables you to examine the
; resulting effect of the routine on the registers, and to preserve the condition of the
; registers on exit for subsequent SYS calls.

SAREG = $030C     ; .A for SYS command
SXREG = $030D     ; .X for SYS command
SYREG = $030E     ; .Y for SYS command
SPREG = $030F     ; .P for SYS command

; $0310 to $0313 - unused

CINV  = $0314     ; IRQ vector
CBINV = $0316     ; BRK vector
NMINV = $0318     ; NMI vector

IOPEN = $031A     ; KERNAL vector - open a logical file
ICLOSE  = $031C     ; KERNAL vector - close a specified logical file
ICHKIN  = $031E     ; KERNAL vector - open channel for input
ICKOUT  = $0320     ; KERNAL vector - open channel for output
ICLRCN  = $0322     ; KERNAL vector - close input and output channels
IBASIN  = $0324     ; KERNAL vector - input character from channel
IBSOUT  = $0326     ; KERNAL vector - output character to channel
ISTOP = $0328     ; KERNAL vector - scan stop key
IGETIN  = $032A     ; KERNAL vector - get character from keyboard queue
ICLALL  = $032C     ; KERNAL vector - close all channels and files
USRCMD  = $032E     ; User vector ( unused )

ILOAD = $0330     ; KERNAL vector - load
ISAVE = $0332     ; KERNAL vector - save

; $0334 to $033B - unused

TBUFFR  = $033C     ; to $03FB - cassette buffer

; $03FC to $03FF - unused


;***********************************************************************************;
;
; hardware equates

VICCR0  = $9000     ; screen origin - horizontal
          ; bit function
          ; --- --------
          ;  7  interlace mode (NTSC only)
          ; 6-0 horizontal origin
VICCR1  = $9001     ; screen origin - vertical
VICCR2  = $9002     ; video address and screen columns
          ; bit function
          ; --- --------
          ;  7  video memory address va9
          ; colour memory address va9
          ; 6-0 number of columns
VICCR3  = $9003     ; screen rows and character height
          ; bit function
          ; --- --------
          ;  7  raster line b0
          ; 6-1 number of rows
          ;  0  character height (8/16 bits)
VICCR4  = $9004     ; raster line b8-b1
VICCR5  = $9005     ; video and character memory addresses
          ; bit function
          ; --- --------
          ; 7-4 video memory address va13-va10
          ; 3-0 character memory address va13-va10

          ; 0000 ROM  $8000 set 1
          ; 0001  " $8400
          ; 0010  " $8800 set 2
          ; 0011  " $8C00
          ; 1100 RAM  $1000
          ; 1101  " $1400
          ; 1110  " $1800
          ; 1111  " $1C00
VICCR6  = $9006     ; light pen horizontal position
VICCR7  = $9007     ; light pen vertical position
VICCR8  = $9008     ; paddle X
VICCR9  = $9009     ; paddle Y
VICCRA  = $900A     ; oscillator 1
          ; bit function
          ; --- --------
          ;  7  enable
          ; 6-0 frequency
VICCRB  = $900B     ; oscillator 2
          ; bit function
          ; --- --------
          ;  7  enable
          ; 6-0 frequency
VICCRC  = $900C     ; oscillator 3
          ; bit function
          ; --- --------
          ;  7  enable
          ; 6-0 frequency
VICCRD  = $900D     ; white noise
          ; bit function
          ; --- --------
          ;  7  enable
          ; 6-0 frequency
VICCRE  = $900E     ; auxiliary colour and volume
          ; bit function
          ; --- --------
          ; 7-4 auxiliary colour
          ; 3-0 volume
VICCRF  = $900F     ; background and border colour
          ; bit function
          ; --- --------
          ; 7-4 background colour
          ;  3  reverse video
          ; 2-0 border colour

VIA1PB    = $9110   ; VIA 1 DRB
          ; bit function
          ; --- --------
          ;  7  DSR in
          ;  6  CTS in
          ;  5  unused
          ;  4  DCD in
          ;  3  RI in
          ;  2  DTR out
          ;  1  RTS out
          ;  0  data in

VIA1PA1   = $9111   ; VIA 1 DRA
          ; bit function
          ; --- --------
          ;  7  serial ATN out
          ;  6  cassette switch
          ;  5  joystick fire, light pen
          ;  4  joystick left, paddle X fire
          ;  3  joystick down
          ;  2  joystick up
          ;  1  serial DATA in
          ;  0  serial CLK in

VIA1DDRB  = $9112   ; VIA 1 DDRB
VIA1DDRA  = $9113   ; VIA 1 DDRA
VIA1T1CL  = $9114   ; VIA 1 T1C_l
VIA1T1CH  = $9115   ; VIA 1 T1C_h
VIA1T2CL  = $9118   ; VIA 1 T2C_l
VIA1T2CH  = $9119   ; VIA 1 T2C_h
VIA1ACR   = $911B   ; VIA 1 ACR
          ; bit function
          ; --- --------
          ;  7  T1 PB7 enabled/disabled
          ;  6  T1 free run/one shot
          ;  5  T2 clock PB6/Ã¸2
          ; 432 function
          ; --- --------
          ; 000 shift register disabled
          ; 001 shift in, rate controlled by T2
          ; 010 shift in, rate controlled by Ã¸2
          ; 011 shift in, rate controlled by external clock
          ; 100 shift out, rate controlled by T2, free run mode
          ; 101 shift out, rate controlled by T2
          ; 110 shift out, rate controlled by Ã¸2
          ; 111 shift out, rate controlled by external clock
          ;  1  PB latch enabled/disabled
          ;  0  PA latch enabled/disabled

VIA1PCR   = $911C   ; VIA 1 PCR
          ; bit function
          ; --- --------
          ; 765 CB2 Tx RS-232 data
          ;  4  CB1 Rx RS-232 data
          ; 321 CA2 cassette motor control
          ;  0  CA1 [RESTORE] key

; The status bit is a not normal flag. It goes high if both an interrupt flag in the IFR
; and the corresponding enable bit in the IER are set. It can be cleared only by clearing
; all the active flags in the IFR or disabling all active interrupts in the IER.

VIA1IFR   = $911D   ; VIA 1 IFR
          ; bit function    cleared by
          ; --- --------    ----------
          ;  7  interrupt status  clearing all enabled interrupts
          ;  6  T1 interrupt    read T1C_l, write T1C_h
          ;  5  T2 interrupt    read T2C_l, write T2C_h
          ;  4  CB1 transition    read or write port B
          ;  3  CB2 transition    read or write port B
          ;  2  8 shifts done   read or write the shift register
          ;  1  CA1 transition    read or write port A
          ;  0  CA2 transition    read or write port A

; If enable/disable bit is a zero during a write to this register, each 1 in bits 0-6
; clears the corresponding bit in the IER. If this bit is a one during a write to this
; register, each 1 in bits 0-6 will set the corresponding IER bit.

VIA1IER   = $911E   ; VIA 1 IER
          ; bit function
          ; --- --------
          ;  7  enable/disable
          ;  6  T1 interrupt
          ;  5  T2 interrupt
          ;  4  CB1 transition
          ;  3  CB2 transition
          ;  2  8 shifts done
          ;  1  CA1 transition
          ;  0  CA2 transition

VIA1PA2   = $911F   ; VIA 1 DRA, no handshake
          ; bit function
          ; --- --------
          ;  7  serial ATN out
          ;  6  cassette switch
          ;  5  joystick fire, light pen
          ;  4  joystick left, paddle X fire
          ;  3  joystick down
          ;  2  joystick up
          ;  1  serial DATA in
          ;  0  serial CLK in

VIA2PB    = $9120   ; VIA 2 DRB, keyboard column
          ; bit function
          ; --- --------
          ;  7  joystick right, paddle Y fire
          ;  3  cassette write line

VIA2PA1   = $9121   ; VIA 2 DRA, keyboard row
          ; VIC 20 keyboard matrix layout
          ; c7  c6  c5  c4  c3  c2  c1  c0
          ;   +----------------------------------------------------------------
          ; r7| [F7]  [F5]  [F3]  [F1]  [DWN] [RGT] [RET] [DEL]
          ; r6| [HOME]  [UP]  = [RSH] / ; * Â£
          ; r5| - @ : . , L P +
          ; r4| 0 O K M N J I 9
          ; r3| 8 U H B V G Y 7
          ; r2| 6 T F C X D R 5
          ; r1| 4 E S Z [LSH] A W 3
          ; r0| 2 Q [C=]  [SP]  [RUN] [CTL] [<-]  1

VIA2DDRB  = $9122   ; VIA 2 DDRB
VIA2DDRA  = $9123   ; VIA 2 DDRA
VIA2T1CL  = $9124   ; VIA 2 T1C_l
VIA2T1CH  = $9125   ; VIA 2 T1C_h
VIA2T2CL  = $9128   ; VIA 2 T2C_l
VIA2T2CH  = $9129   ; VIA 2 T2C_h
VIA2ACR   = $912B   ; VIA 2 ACR
VIA2PCR   = $912C   ; VIA 2 PCR
          ; bit function
          ; --- --------
          ; 765 CB2 serial DATA out
          ;  4  CB1 serial SRQ in
          ; 321 CA2 serial CLK out
          ;  0  CA1 cassette read line

; The status bit is a not normal flag. it goes high if both an interrupt flag in the IFR
; and the corresponding enable bit in the IER are set. It can be cleared only by clearing
; all the active flags in the IFR or disabling all active interrupts in the IER.

VIA2IFR   = $912D   ; VIA 2 IFR
          ; bit function    cleared by
          ; --- --------    ----------
          ;  7  interrupt status  clearing all enabled interrupts
          ;  6  T1 interrupt    read T1C_l, write T1C_h
          ;  5  T2 interrupt    read T2C_l, write T2C_h
          ;  4  CB1 transition    read or write port B
          ;  3  CB2 transition    read or write port B
          ;  2  8 shifts done   read or write the shift register
          ;  1  CA1 transition    read or write port A
          ;  0  CA2 transition    read or write port A

; If enable/disable bit is a zero during a write to this register, each 1 in bits 0-6
; clears the corresponding bit in the IER. If this bit is a one during a write to this
; register, each 1 in bits 0-6 will set the corresponding IER bit.

VIA2IER   = $912E   ; VIA 2 IER
          ; bit function
          ; --- --------
          ;  7  enable/disable
          ;  6  T1 interrupt
          ;  5  T2 interrupt
          ;  4  CB1 transition
          ;  3  CB2 transition
          ;  2  8 shifts done
          ;  1  CA1 transition
          ;  0  CA2 transition

VIA2PA2   = $912F   ; VIA 2 DRA, keyboard row, no handshake

XROMCOLD  = $A000   ; autostart ROM initial entry vector
XROMWARM  = $A002   ; autostart ROM break entry vector
XROMID    = $A004   ; .. to $A008 autostart ROM identifier string start


;***********************************************************************************;
;
; BASIC keyword token values. Tokens not used in the source are included for
; completeness.

; command tokens

TK_END    = $80     ; END token
TK_FOR    = $81     ; FOR token
TK_NEXT   = $82     ; NEXT token
TK_DATA   = $83     ; DATA token
TK_INFL   = $84     ; INPUT# token
TK_INPUT  = $85     ; INPUT token
TK_DIM    = $86     ; DIM token
TK_READ   = $87     ; READ token

TK_LET    = $88     ; LET token
TK_GOTO   = $89     ; GOTO token
TK_RUN    = $8A     ; RUN token
TK_IF   = $8B     ; IF token
TK_RESTORE  = $8C     ; RESTORE token
TK_GOSUB  = $8D     ; GOSUB token
TK_RETURN = $8E     ; RETURN token
TK_REM    = $8F     ; REM token

TK_STOP   = $90     ; STOP token
TK_ON   = $91     ; ON token
TK_WAIT   = $92     ; WAIT token
TK_LOAD   = $93     ; LOAD token
TK_SAVE   = $94     ; SAVE token
TK_VERIFY = $95     ; VERIFY token
TK_DEF    = $96     ; DEF token
TK_POKE   = $97     ; POKE token

TK_PRINFL = $98     ; PRINT# token
TK_PRINT  = $99     ; PRINT token
TK_CONT   = $9A     ; CONT token
TK_LIST   = $9B     ; LIST token
TK_CLR    = $9C     ; CLR token
TK_CMD    = $9D     ; CMD token
TK_SYS    = $9E     ; SYS token
TK_OPEN   = $9F     ; OPEN token

TK_CLOSE  = $A0     ; CLOSE token
TK_GET    = $A1     ; GET token
TK_NEW    = $A2     ; NEW token

; secondary keyword tokens

TK_TAB    = $A3     ; TAB( token
TK_TO   = $A4     ; TO token
TK_FN   = $A5     ; FN token
TK_SPC    = $A6     ; SPC( token
TK_THEN   = $A7     ; THEN token

TK_NOT    = $A8     ; NOT token
TK_STEP   = $A9     ; STEP token

; operator tokens

TK_PLUS   = $AA     ; + token
TK_MINUS  = $AB     ; - token
TK_MUL    = $AC     ; * token
TK_DIV    = $AD     ; / token
TK_POWER  = $AE     ; ^ token
TK_AND    = $AF     ; AND token

TK_OR   = $B0     ; OR token
TK_GT   = $B1     ; > token
TK_EQUAL  = $B2     ; = token
TK_LT   = $B3     ; < token

; function tokens

TK_SGN    = $B4     ; SGN token
TK_INT    = $B5     ; INT token
TK_ABS    = $B6     ; ABS token
TK_USR    = $B7     ; USR token

TK_FRE    = $B8     ; FRE token
TK_POS    = $B9     ; POS token
TK_SQR    = $BA     ; SQR token
TK_RND    = $BB     ; RND token
TK_LOG    = $BC     ; LOG token
TK_EXP    = $BD     ; EXP token
TK_COS    = $BE     ; COS token
TK_SIN    = $BF     ; SIN token

TK_TAN    = $C0     ; TAN token
TK_ATN    = $C1     ; ATN token
TK_PEEK   = $C2     ; PEEK token
TK_LEN    = $C3     ; LEN token
TK_STRS   = $C4     ; STR$ token
TK_VAL    = $C5     ; VAL token
TK_ASC    = $C6     ; ASC token
TK_CHRS   = $C7     ; CHR$ token

TK_LEFTS  = $C8     ; LEFT$ token
TK_RIGHTS = $C9     ; RIGHT$ token
TK_MIDS   = $CA     ; MID$ token
TK_GO   = $CB     ; GO token

TK_PI   = $FF     ; PI token

;***********************************************************************************;
;
; floating point accumulator offsets

FAC_EXPT  = $00
FAC_MANT  = $01
FAC_SIGN  = $05

;***********************************************************************************;
;***********************************************************************************;
;
; BASIC ROM start

* = $C000

COLDST
  .word COLDBA      ; BASIC cold start entry point

WARMST
  .word WARMBAS     ; BASIC warm start entry point

;CBMBASIC
  .byte "CBMBASIC"    ; ROM name, unreferenced


;***********************************************************************************;
;
; Action addresses for primary commands. These are called by pushing the address
; onto the stack and doing an RTS so the actual address - 1 needs to be pushed.

STMDSP
  .word END-1     ; perform END
  .word FOR-1     ; perform FOR
  .word NEXT-1      ; perform NEXT
  .word SKIPST-1    ; perform DATA
  .word INPUTN-1    ; perform INPUT#
  .word INPUT-1     ; perform INPUT
  .word DIM-1     ; perform DIM
  .word READ-1      ; perform READ

  .word LET-1     ; perform LET
  .word GOTO-1      ; perform GOTO
  .word RUN-1     ; perform RUN
  .word IF-1      ; perform IF
  .word RESTORE-1   ; perform RESTORE
  .word GOSUB-1     ; perform GOSUB
  .word RETURN-1    ; perform RETURN
  .word REM-1     ; perform REM

  .word BSTOP-1     ; perform STOP
  .word ON-1      ; perform ON
  .word WAIT-1      ; perform WAIT
  .word BLOAD-1     ; perform LOAD
  .word BSAVE-1     ; perform SAVE
  .word BVERIF-1    ; perform VERIFY
  .word DEF-1     ; perform DEF
  .word POKE-1      ; perform POKE

  .word PRINTN-1    ; perform PRINT#
  .word PRINT-1     ; perform PRINT
  .word CONT-1      ; perform CONT
  .word LIST-1      ; perform LIST
  .word CLR-1     ; perform CLR
  .word CMD-1     ; perform CMD
  .word SYSTEM-1    ; perform SYS
  .word BOPEN-1     ; perform OPEN

  .word BCLOSE-1    ; perform CLOSE
  .word GET-1     ; perform GET
  .word NEW-1     ; perform NEW


;***********************************************************************************;
;
; action addresses for functions

FUNDSP
  .word SGN     ; perform SGN()
  .word INT     ; perform INT()
  .word ABS     ; perform ABS()
  .word USRPPOK     ; perform USR()

  .word FRE     ; perform FRE()
  .word POS     ; perform POS()
  .word SQR     ; perform SQR()
  .word RND     ; perform RND()
  .word LOG     ; perform LOG()
  .word EXP     ; perform EXP()
  .word COS     ; perform COS()
  .word SIN     ; perform SIN()

  .word TAN     ; perform TAN()
  .word ATN     ; perform ATN()
  .word PEEK      ; perform PEEK()
  .word LEN     ; perform LEN()
  .word STR     ; perform STR$()
  .word VAL     ; perform VAL()
  .word ASC     ; perform ASC()
  .word CHR     ; perform CHR$()

  .word LEFT      ; perform LEFT$()
  .word RIGHT     ; perform RIGHT$()
  .word MID     ; perform MID$()


;***********************************************************************************;
;
; Precedence byte and action addresses for operators. Like the primary commands these
; are called by pushing the address onto the stack and doing an RTS, so again the actual
; address - 1 needs to be pushed.

OPTAB
  .byte $79
  .word PLUS-1      ; +
  .byte $79
  .word SUB-1     ; -
  .byte $7B
  .word MULT-1      ; *
  .byte $7B
  .word DIVIDE-1    ; /
  .byte $7F
  .word EXPONT-1    ; ^
  .byte $50
  .word ANDD-1      ; AND
  .byte $46
  .word ORR-1     ; OR
  .byte $7D
  .word NEGFAC-1    ; >
  .byte $5A
  .word EQUAL-1     ; =
LAB_C09B
  .byte $64
  .word COMPAR-1    ; <


;***********************************************************************************;
;
; BASIC keywords. Each word has b7 set in its last character as an end marker,
; even the one character keywords such as "<" or "=".

; first are the primary command keywords, only these can start a statement

RESLST
  .byte "EN",'D'+$80    ; END
  .byte "FO",'R'+$80    ; FOR
  .byte "NEX",'T'+$80   ; NEXT
  .byte "DAT",'A'+$80   ; DATA
  .byte "INPUT",'#'+$80   ; INPUT#
  .byte "INPU",'T'+$80    ; INPUT
  .byte "DI",'M'+$80    ; DIM
  .byte "REA",'D'+$80   ; READ

  .byte "LE",'T'+$80    ; LET
  .byte "GOT",'O'+$80   ; GOTO
  .byte "RU",'N'+$80    ; RUN
  .byte "I",'F'+$80   ; IF
  .byte "RESTOR",'E'+$80  ; RESTORE
  .byte "GOSU",'B'+$80    ; GOSUB
  .byte "RETUR",'N'+$80   ; RETURN
  .byte "RE",'M'+$80    ; REM

  .byte "STO",'P'+$80   ; STOP
  .byte "O",'N'+$80   ; ON
  .byte "WAI",'T'+$80   ; WAIT
  .byte "LOA",'D'+$80   ; LOAD
  .byte "SAV",'E'+$80   ; SAVE
  .byte "VERIF",'Y'+$80   ; VERIFY
  .byte "DE",'F'+$80    ; DEF
  .byte "POK",'E'+$80   ; POKE

  .byte "PRINT",'#'+$80   ; PRINT#
  .byte "PRIN",'T'+$80    ; PRINT
  .byte "CON",'T'+$80   ; CONT
  .byte "LIS",'T'+$80   ; LIST
  .byte "CL",'R'+$80    ; CLR
  .byte "CM",'D'+$80    ; CMD
  .byte "SY",'S'+$80    ; SYS
  .byte "OPE",'N'+$80   ; OPEN

  .byte "CLOS",'E'+$80    ; CLOSE
  .byte "GE",'T'+$80    ; GET
  .byte "NE",'W'+$80    ; NEW

; next are the secondary command keywords, these cannot start a statement

  .byte "TAB",'('+$80   ; TAB(
  .byte "T",'O'+$80   ; TO
  .byte "F",'N'+$80   ; FN
  .byte "SPC",'('+$80   ; SPC(
  .byte "THE",'N'+$80   ; THEN

  .byte "NO",'T'+$80    ; NOT
  .byte "STE",'P'+$80   ; STEP

; the operators

  .byte '+'+$80     ; +
  .byte '-'+$80     ; -
  .byte '*'+$80     ; *
  .byte '/'+$80     ; /
  .byte '^^'+$80    ; ^
  .byte "AN",'D'+$80    ; AND

  .byte "O",'R'+$80   ; OR
  .byte '>'+$80     ; >
  .byte '='+$80     ; =
  .byte '<'+$80     ; <

; the functions

  .byte "SG",'N'+$80    ; SGN
  .byte "IN",'T'+$80    ; INT
  .byte "AB",'S'+$80    ; ABS
  .byte "US",'R'+$80    ; USR

  .byte "FR",'E'+$80    ; FRE
  .byte "PO",'S'+$80    ; POS
  .byte "SQ",'R'+$80    ; SQR
  .byte "RN",'D'+$80    ; RND
  .byte "LO",'G'+$80    ; LOG
  .byte "EX",'P'+$80    ; EXP
  .byte "CO",'S'+$80    ; COS
  .byte "SI",'N'+$80    ; SIN

  .byte "TA",'N'+$80    ; TAN
  .byte "AT",'N'+$80    ; ATN
  .byte "PEE",'K'+$80   ; PEEK
  .byte "LE",'N'+$80    ; LEN
  .byte "STR",'$'+$80   ; STR$
  .byte "VA",'L'+$80    ; VAL
  .byte "AS",'C'+$80    ; ASC
  .byte "CHR",'$'+$80   ; CHR$

  .byte "LEFT",'$'+$80    ; LEFT$
  .byte "RIGHT",'$'+$80   ; RIGHT$
  .byte "MID",'$'+$80   ; MID$

; lastly is GO, this is an add on so that GO TO, as well as GOTO, will work

  .byte "G",'O'+$80   ; GO

  .byte $00     ; end marker


;***********************************************************************************;
;
; error messages

ER_STOP   = $00
ER_2MANYF = $01
ER_FOPEN  = $02
ER_FNOTOPEN = $03
ER_FNOTFND  = $04
ER_DEVNOTP  = $05
ER_NOTINF = $06
ER_NOTOUTF  = $07
ER_MISSFNAM = $08
ER_ILLDEVN  = $09
ER_NXTWOFOR = $0A
ER_SYNTAX = $0B
ER_RETWOGSB = $0C
ER_OODATA = $0D
ER_ILLQUAN  = $0E
ER_OVFLOW = $0F
ER_OOMEM  = $10
ER_UNDSMNT  = $11
ER_BADSSCPT = $12
ER_REDIMARY = $13
ER_DIVBY0 = $14
ER_ILLDIR = $15
ER_TYPMSMCH = $16
ER_STR2LONG = $17
ER_FDATA  = $18
ER_FMLA2CPLX  = $19
ER_CANTCONT = $1A
ER_UNDEFUN  = $1B
ER_VERIFY = $1C
ER_LOAD   = $1D
ER_BREAK  = $1E

ERRSTR01
  .byte "TOO MANY FILE",'S'+$80
ERRSTR02
  .byte "FILE OPE",'N'+$80
ERRSTR03
  .byte "FILE NOT OPE",'N'+$80
ERRSTR04
  .byte "FILE NOT FOUN",'D'+$80
ERRSTR05
  .byte "DEVICE NOT PRESEN",'T'+$80
ERRSTR06
  .byte "NOT INPUT FIL",'E'+$80
ERRSTR07
  .byte "NOT OUTPUT FIL",'E'+$80
ERRSTR08
  .byte "MISSING FILE NAM",'E'+$80
ERRSTR09
  .byte "ILLEGAL DEVICE NUMBE",'R'+$80
ERRSTR0A
  .byte "NEXT WITHOUT FO",'R'+$80
ERRSTR0B
  .byte "SYNTA",'X'+$80
ERRSTR0C
  .byte "RETURN WITHOUT GOSU",'B'+$80
ERRSTR0D
  .byte "OUT OF DAT",'A'+$80
ERRSTR0E
  .byte "ILLEGAL QUANTIT",'Y'+$80
ERRSTR0F
  .byte "OVERFLO",'W'+$80
ERRSTR10
  .byte "OUT OF MEMOR",'Y'+$80
ERRSTR11
  .byte "UNDEF'D STATEMEN",'T'+$80
ERRSTR12
  .byte "BAD SUBSCRIP",'T'+$80
ERRSTR13
  .byte "REDIM'D ARRA",'Y'+$80
ERRSTR14
  .byte "DIVISION BY ZER",'O'+$80
ERRSTR15
  .byte "ILLEGAL DIREC",'T'+$80
ERRSTR16
  .byte "TYPE MISMATC",'H'+$80
ERRSTR17
  .byte "STRING TOO LON",'G'+$80
ERRSTR18
  .byte "FILE DAT",'A'+$80
ERRSTR19
  .byte "FORMULA TOO COMPLE",'X'+$80
ERRSTR1A
  .byte "CAN'T CONTINU",'E'+$80
ERRSTR1B
  .byte "UNDEF'D FUNCTIO",'N'+$80
ERRSTR1C
  .byte "VERIF",'Y'+$80
ERRSTR1D
  .byte "LOA",'D'+$80

; error message pointer table

BMSGS
  .word ERRSTR01    ; $01 TOO MANY FILES
  .word ERRSTR02    ; $02 FILE OPEN
  .word ERRSTR03    ; $03 FILE NOT OPEN
  .word ERRSTR04    ; $04 FILE NOT FOUND
  .word ERRSTR05    ; $05 DEVICE NOT PRESENT
  .word ERRSTR06    ; $06 NOT INPUT FILE
  .word ERRSTR07    ; $07 NOT OUTPUT FILE
  .word ERRSTR08    ; $08 MISSING FILE NAME
  .word ERRSTR09    ; $09 ILLEGAL DEVICE NUMBER
  .word ERRSTR0A    ; $0A NEXT WITHOUT FOR
  .word ERRSTR0B    ; $0B SYNTAX
  .word ERRSTR0C    ; $0C RETURN WITHOUT GOSUB
  .word ERRSTR0D    ; $0D OUT OF DATA
  .word ERRSTR0E    ; $0E ILLEGAL QUANTITY
  .word ERRSTR0F    ; $0F OVERFLOW
  .word ERRSTR10    ; $10 OUT OF MEMORY
  .word ERRSTR11    ; $11 UNDEF'D STATEMENT
  .word ERRSTR12    ; $12 BAD SUBSCRIPT
  .word ERRSTR13    ; $13 REDIM'D ARRAY
  .word ERRSTR14    ; $14 DIVISION BY ZERO
  .word ERRSTR15    ; $15 ILLEGAL DIRECT
  .word ERRSTR16    ; $16 TYPE MISMATCH
  .word ERRSTR17    ; $17 STRING TOO LONG
  .word ERRSTR18    ; $18 FILE DATA
  .word ERRSTR19    ; $19 FORMULA TOO COMPLEX
  .word ERRSTR1A    ; $1A CAN'T CONTINUE
  .word ERRSTR1B    ; $1B UNDEF'D FUNCTION
  .word ERRSTR1C    ; $1C VERIFY
  .word ERRSTR1D    ; $1D LOAD
  .word BREAKSTR    ; $1E BREAK


;***********************************************************************************;
;
; BASIC messages

OKSTR
  .byte $0D,"OK",$0D,$00
ERRORSTR
  .byte $0D," ERROR",$00
INSTR
  .byte " IN ",$00
READYSTR
  .byte $0D,$0A,"READY.",$0D,$0A,$00
CRLFBRK
  .byte $0D,$0A
BREAKSTR
  .byte "BREAK",$00


;***********************************************************************************;
;
; spare byte, not referenced

;LAB_C389
  .byte $A0


;***********************************************************************************;
;
; search the stack for FOR or GOSUB activity
; return Zb=1 if FOR variable found

SCNSTK
  TSX       ; copy stack pointer
  INX       ; +1 pass return address
  INX       ; +2 pass return address
  INX       ; +3 pass calling routine return address
  INX       ; +4 pass calling routine return address
LAB_C38F
  LDA STACK+1,X   ; get token byte from stack
  CMP #TK_FOR     ; is it FOR token
  BNE LAB_C3B7    ; exit if not FOR token

          ; was FOR token
  LDA FORPNT+1    ; get FOR/NEXT variable pointer high byte
  BNE LAB_C3A4    ; branch if not null

  LDA STACK+2,X   ; get FOR variable pointer low byte
  STA FORPNT      ; save FOR/NEXT variable pointer low byte
  LDA STACK+3,X   ; get FOR variable pointer high byte
  STA FORPNT+1    ; save FOR/NEXT variable pointer high byte
LAB_C3A4
  CMP STACK+3,X   ; compare variable pointer with stacked variable pointer
          ; high byte
  BNE LAB_C3B0    ; branch if no match

  LDA FORPNT      ; get FOR/NEXT variable pointer low byte
  CMP STACK+2,X   ; compare variable pointer with stacked variable pointer
          ; low byte
  BEQ LAB_C3B7    ; exit if match found

LAB_C3B0
  TXA       ; copy index
  CLC       ; clear carry for add
  ADC #$12      ; add FOR stack use size
  TAX       ; copy back to index
  BNE LAB_C38F    ; loop if not at start of stack

LAB_C3B7
  RTS


;***********************************************************************************;
;
; open up space in memory, set end of arrays

MAKSPC
  JSR RAMSPC      ; check available memory, do out of memory error if no room
  STA STREND      ; set end of arrays low byte
  STY STREND+1    ; set end of arrays high byte

; open up space in memory, don't set array end

MOVEBL
  SEC       ; set carry for subtract
  LDA GEN2PTR     ; get block end low byte
  SBC TMPPTR      ; subtract block start low byte
  STA INDEX     ; save MOD(block length/$100) byte
  TAY       ; copy MOD(block length/$100) byte to .Y
  LDA GEN2PTR+1   ; get block end high byte
  SBC TMPPTR+1    ; subtract block start high byte
  TAX       ; copy block length high byte to .X
  INX       ; +1 to allow for count=0 exit
  TYA       ; copy block length low byte to .A
  BEQ LAB_C3F3    ; branch if length low byte=0

          ; block is (.X-1)*$100+.Y bytes, do the .Y bytes first
  LDA GEN2PTR     ; get block end low byte
  SEC       ; set carry for subtract
  SBC INDEX     ; subtract MOD(block length/$100) byte
  STA GEN2PTR     ; save corrected old block end low byte
  BCS LAB_C3DC    ; if no underflow skip the high byte decrement

  DEC GEN2PTR+1   ; else decrement block end high byte
  SEC       ; set carry for subtract
LAB_C3DC
  LDA GENPTR      ; get destination end low byte
  SBC INDEX     ; subtract MOD(block length/$100) byte
  STA GENPTR      ; save modified new block end low byte
  BCS LAB_C3EC    ; if no underflow skip the high byte decrement

  DEC GENPTR+1    ; else decrement block end high byte
  BCC LAB_C3EC    ; branch always

LAB_C3E8
  LDA (GEN2PTR),Y   ; get byte from source
  STA (GENPTR),Y    ; copy byte to destination
LAB_C3EC
  DEY       ; decrement index
  BNE LAB_C3E8    ; loop until .Y=0

          ; now do .Y=0 indexed byte
  LDA (GEN2PTR),Y   ; get byte from source
  STA (GENPTR),Y    ; save byte to destination
LAB_C3F3
  DEC GEN2PTR+1   ; decrement source pointer high byte
  DEC GENPTR+1    ; decrement destination pointer high byte
  DEX       ; decrement block count
  BNE LAB_C3EC    ; loop until count = $0

  RTS


;***********************************************************************************;
;
; check there is room on the stack for .A bytes
; if the stack is too deep do an out of memory error

STKSPC
  ASL       ; *2
  ADC #$3E      ; need at least 62d bytes free
  BCS MEMERR      ; if overflow go do out of memory error then warm start

  STA INDEX     ; save result in temp byte
  TSX       ; copy stack
  CPX INDEX     ; compare new limit with stack
  BCC MEMERR      ; if stack < limit do out of memory error then warm start

  RTS


;***********************************************************************************;
;
; check available memory, do out of memory error if no room

RAMSPC
  CPY FRETOP+1    ; compare with bottom of string space high byte
  BCC LAB_C434    ; if less then exit (is ok)

  BNE LAB_C412    ; skip next test if greater (tested <)

          ; high byte was =, now do low byte
  CMP FRETOP      ; compare with bottom of string space low byte
  BCC LAB_C434    ; if less then exit (is ok)

          ; address is > string storage ptr (oops!)
LAB_C412
  PHA       ; push address low byte
  LDX #$09      ; set index to save TEMPF3 to TMPPTR+1 inclusive
  TYA       ; copy address high byte (to push on stack)

          ; save misc numeric work area
LAB_C416
  PHA       ; push byte
  LDA TEMPF3,X    ; get byte from TEMPF3 to TMPPTR+1
  DEX       ; decrement index
  BPL LAB_C416    ; loop until all done

  JSR GRBCOL      ; do garbage collection routine

          ; restore misc numeric work area
  LDX #$F7      ; set index to restore bytes
LAB_C421
  PLA       ; pop byte
  STA TMPPTR+2,X    ; save byte to TEMPF3 to TMPPTR+2
  INX       ; increment index
  BMI LAB_C421    ; loop while -ve

  PLA       ; pop address high byte
  TAY       ; copy back to .Y
  PLA       ; pop address low byte
  CPY FRETOP+1    ; compare with bottom of string space high byte
  BCC LAB_C434    ; if less then exit (is ok)

  BNE MEMERR      ; if greater do out of memory error then warm start

          ; high byte was =, now do low byte
  CMP FRETOP      ; compare with bottom of string space low byte
  BCS MEMERR      ; if >= do out of memory error then warm start

          ; ok exit, carry clear
LAB_C434
  RTS


;***********************************************************************************;
;
; do out of memory error then warm start

MEMERR
  LDX #ER_OOMEM   ; error code $10, out of memory error

; do error #.X then warm start

ERROR
  JMP (IERROR)    ; do error message

; do error #.X then warm start, the error message vector is initialised to point here

ERROR2
  TXA       ; copy error number
  ASL       ; *2
  TAX       ; copy to index
  LDA BMSGS-2,X   ; get error message pointer low byte
  STA INDEX     ; save it
  LDA BMSGS-1,X   ; get error message pointer high byte
  STA INDEX+1     ; save it
  JSR CLRCHN      ; close input and output channels
  LDA #$00      ; clear .A
  STA CHANNL      ; clear current I/O channel, flag default
  JSR LAB_CAD7    ; print CR/LF
  JSR LAB_CB45    ; print "?"
  LDY #$00      ; clear index
LAB_C456
  LDA (INDEX),Y   ; get byte from message
  PHA       ; save status
  AND #$7F      ; mask 0xxx xxxx, clear b7
  JSR LAB_CB47    ; output character
  INY       ; increment index
  PLA       ; restore status
  BPL LAB_C456    ; loop if character was not end marker

  JSR LAB_C67A    ; flush BASIC stack and clear continue pointer
  LDA #<ERRORSTR    ; set " ERROR" pointer low byte
  LDY #>ERRORSTR    ; set " ERROR" pointer high byte


;***********************************************************************************;
;
; print string and do warm start, break entry

PRDY
  JSR PRTSTR      ; print null terminated string
  LDY CURLIN+1    ; get current line number high byte
  INY       ; increment it
  BEQ READY     ; branch if was in immediate mode

  JSR PRTIN     ; do " IN " line number message


;***********************************************************************************;
;
; do warm start

READY
  LDA #<READYSTR    ; set "READY." pointer low byte
  LDY #>READYSTR    ; set "READY." pointer high byte
  JSR PRTSTR      ; print null terminated string
  LDA #$80      ; set for control messages only
  JSR SETMSG      ; control KERNAL messages
MAIN
  JMP (IMAIN)     ; do BASIC warm start


;***********************************************************************************;
;
; BASIC warm start, the warm start vector is initialised to point here

MAIN2
  JSR GETLIN      ; call for BASIC input
  STX CHRGOT+1    ; save BASIC execute pointer low byte
  STY CHRGOT+2    ; save BASIC execute pointer high byte
  JSR CHRGET      ; increment and scan memory
  TAX       ; copy byte to set flags
  BEQ MAIN      ; loop if no input

; got to interpret input line now ...

  LDX #$FF      ; current line high byte to -1, indicates immediate mode
  STX CURLIN+1    ; set current line number high byte
  BCC NEWLIN      ; if numeric character go handle new BASIC line

          ; no line number .. immediate mode
  JSR CRNCH     ; crunch keywords into BASIC tokens
  JMP LAB_C7E1    ; go scan and interpret code

; handle new BASIC line

NEWLIN
  JSR DECBIN      ; get fixed-point number into temporary integer
  JSR CRNCH     ; crunch keywords into BASIC tokens
  STY COUNT     ; save index pointer to end of crunched line
  JSR FINLIN      ; search BASIC for temporary integer line number
  BCC LAB_C4ED    ; if not found skip the line delete

          ; line # already exists so delete it
  LDY #$01      ; set index to next line pointer high byte
  LDA (TMPPTR),Y    ; get next line pointer high byte
  STA INDEX+1     ; save it
  LDA VARTAB      ; get start of variables low byte
  STA INDEX     ; save it
  LDA TMPPTR+1    ; get found line pointer high byte
  STA INDEX+3     ; save it
  LDA TMPPTR      ; get found line pointer low byte
  DEY       ; decrement index
  SBC (TMPPTR),Y    ; subtract next line pointer low byte
  CLC       ; clear carry for add
  ADC VARTAB      ; add start of variables low byte
  STA VARTAB      ; set start of variables low byte
  STA INDEX+2     ; save destination pointer low byte
  LDA VARTAB+1    ; get start of variables high byte
  ADC #$FF      ; -1 + carry
  STA VARTAB+1    ; set start of variables high byte
  SBC TMPPTR+1    ; subtract found line pointer high byte
  TAX       ; copy to block count
  SEC       ; set carry for subtract
  LDA TMPPTR      ; get found line pointer low byte
  SBC VARTAB      ; subtract start of variables low byte
  TAY       ; copy to bytes in first block count
  BCS LAB_C4D7    ; if no underflow skip the high byte decrement

  INX       ; increment block count, correct for = 0 loop exit
  DEC INDEX+3     ; decrement destination high byte
LAB_C4D7
  CLC       ; clear carry for add
  ADC INDEX     ; add source pointer low byte
  BCC LAB_C4DF    ; if no underflow skip the high byte decrement

  DEC INDEX+1     ; else decrement source pointer high byte
  CLC       ; clear carry

          ; close up memory to delete old line
LAB_C4DF
  LDA (INDEX),Y   ; get byte from source
  STA (INDEX+2),Y   ; copy to destination
  INY       ; increment index
  BNE LAB_C4DF    ; while <> 0 do this block

  INC INDEX+1     ; increment source pointer high byte
  INC INDEX+3     ; increment destination pointer high byte
  DEX       ; decrement block count
  BNE LAB_C4DF    ; loop until all done

          ; got new line in buffer and no existing same #
LAB_C4ED
  JSR LAB_C659    ; reset execution to start, clear variables, flush stack
          ; and return
  JSR LNKPRG      ; rebuild BASIC line chaining
  LDA BUF     ; get first byte from buffer
  BEQ MAIN      ; if no line go do BASIC warm start

          ; else insert line into memory
  CLC       ; clear carry for add
  LDA VARTAB      ; get start of variables low byte
  STA GEN2PTR     ; save as source end pointer low byte
  ADC COUNT     ; add index pointer to end of crunched line
  STA GENPTR      ; save as destination end pointer low byte
  LDY VARTAB+1    ; get start of variables high byte
  STY GEN2PTR+1   ; save as source end pointer high byte
  BCC LAB_C508    ; if no carry skip the high byte increment

  INY       ; else increment the high byte
LAB_C508
  STY GENPTR+1    ; save as destination end pointer high byte
  JSR MAKSPC      ; open up space in memory

; Most of what remains to do is copy the crunched line into the space opened up in memory,
; however, before the crunched line comes the next line pointer and the line number. The
; line number is retrieved from the temporary integer and stored in memory, this
; overwrites the bottom two bytes on the stack. Next the line is copied and the next line
; pointer is filled with whatever was in two bytes above the line number in the stack.
; This is ok because the line pointer gets fixed in the line chain re-build.

  LDA LINNUM      ; get line number low byte
  LDY LINNUM+1    ; get line number high byte
  STA PREVLN      ; save line number low byte before crunched line
  STY PREVLN+1    ; save line number high byte before crunched line
  LDA STREND      ; get end of arrays low byte
  LDY STREND+1    ; get end of arrays high byte
  STA VARTAB      ; set start of variables low byte
  STY VARTAB+1    ; set start of variables high byte
  LDY COUNT     ; get index to end of crunched line
  DEY       ; -1
LAB_C522
  LDA CHNLNK,Y    ; get byte from crunched line
  STA (TMPPTR),Y    ; save byte to memory
  DEY       ; decrement index
  BPL LAB_C522    ; loop while more to do

; reset execution, clear variables, flush stack, rebuild BASIC chain and do warm start

LAB_C52A
  JSR LAB_C659    ; reset execution to start, clear variables and flush stack
  JSR LNKPRG      ; rebuild BASIC line chaining
  JMP MAIN      ; go do BASIC warm start


;***********************************************************************************;
;
; rebuild BASIC line chaining

LNKPRG
  LDA TXTTAB      ; get start of memory low byte
  LDY TXTTAB+1    ; get start of memory high byte
  STA INDEX     ; set line start pointer low byte
  STY INDEX+1     ; set line start pointer high byte
  CLC       ; clear carry for add
LAB_C53C
  LDY #$01      ; set index to pointer to next line high byte
  LDA (INDEX),Y   ; get pointer to next line high byte
  BEQ LAB_C55F    ; exit if null, [EOT]

  LDY #$04      ; point to first code byte of line
          ; there is always 1 byte + [EOL] as null entries are deleted
LAB_C544
  INY       ; next code byte
  LDA (INDEX),Y   ; get byte
  BNE LAB_C544    ; loop if not [EOL]

  INY       ; point to byte past [EOL], start of next line
  TYA       ; copy it
  ADC INDEX     ; add line start pointer low byte
  TAX       ; copy to .X
  LDY #$00      ; clear index, point to this line's next line pointer
  STA (INDEX),Y   ; set next line pointer low byte
  LDA INDEX+1     ; get line start pointer high byte
  ADC #$00      ; add any overflow
  INY       ; increment index to high byte
  STA (INDEX),Y   ; set next line pointer high byte
  STX INDEX     ; set line start pointer low byte
  STA INDEX+1     ; set line start pointer high byte
  BCC LAB_C53C    ; go do next line, branch always

LAB_C55F
  RTS


;***********************************************************************************;
;
; call for BASIC input

GETLIN
  LDX #$00      ; set channel $00, keyboard
LAB_C562
  JSR LAB_E10F    ; input character from channel with error check
  CMP #$0D      ; compare with [RETURN]
  BEQ LAB_C576    ; if [RETURN] set .X.Y to BUF - 1, print [RETURN] and exit

          ; character was not [RETURN]
  STA BUF,X     ; save character to buffer
  INX       ; increment buffer index
  CPX #$59      ; compare with max+1
  BCC LAB_C562    ; branch if < max+1

  LDX #ER_STR2LONG    ; error $17, string too long error
  JMP ERROR     ; do error #.X then warm start

LAB_C576
  JMP LAB_CACA    ; set .X.Y to BUF - 1 and print [CR]


;***********************************************************************************;
;
; crunch BASIC tokens vector

CRNCH
  JMP (ICRNCH)    ; do crunch BASIC tokens


;***********************************************************************************;
;
; crunch BASIC tokens, the crunch BASIC tokens vector is initialised to point here

CRNCH2
  LDX CHRGOT+1    ; get BASIC execute pointer low byte
  LDY #$04      ; set save index
  STY GARBFL      ; clear open quote/DATA flag
LAB_C582
  LDA BUF,X     ; get a byte from the input buffer
  BPL LAB_C58E    ; if b7 clear go do crunching

  CMP #TK_PI      ; compare with the token for PI, this token is input
          ; directly from the keyboard as the [PI] character.
  BEQ LAB_C5C9    ; if PI save byte then continue crunching

          ; this is the bit of code that stops you being able to enter
          ; some keywords as just single shifted characters. If this
          ; dropped through you would be able to enter GOTO as just
          ; [SHIFT]G

  INX       ; increment read index
  BNE LAB_C582    ; loop if more to do, branch always

LAB_C58E
  CMP #' '      ; compare with [SPACE]
  BEQ LAB_C5C9    ; if [SPACE] save byte then continue crunching

  STA ENDCHR      ; save buffer byte as search character
  CMP #$22      ; compare with quote character
  BEQ LAB_C5EE    ; if quote go copy quoted string

  BIT GARBFL      ; get open quote/DATA token flag
  BVS LAB_C5C9    ; branch if b6 of open quote set, was DATA
          ; go save byte then continue crunching

  CMP #'?'      ; compare with "?" character
  BNE LAB_C5A4    ; if not "?" continue crunching

  LDA #TK_PRINT   ; else set the token for PRINT
  BNE LAB_C5C9    ; go save byte then continue crunching, branch always

LAB_C5A4
  CMP #'0'      ; compare with "0"
  BCC LAB_C5AC    ; if < "0" continue crunching

  CMP #'<'      ; compare with "<"
  BCC LAB_C5C9    ; if <, 0123456789:; go save byte then continue crunching

          ; gets here with next character not numeric, ";" or ":"
LAB_C5AC
  STY FBUFPT      ; copy save index
  LDY #$00      ; clear table pointer
  STY COUNT     ; clear word index
  DEY       ; adjust for pre increment loop
  STX CHRGOT+1    ; save BASIC execute pointer low byte, buffer index
  DEX       ; adjust for pre increment loop
LAB_C5B6
  INY       ; next table byte
  INX       ; next buffer byte
LAB_C5B8
  LDA BUF,X     ; get byte from input buffer
  SEC       ; set carry for subtract
  SBC RESLST,Y    ; subtract table byte
  BEQ LAB_C5B6    ; go compare next if match

  CMP #$80      ; was it end marker match ?
  BNE LAB_C5F5    ; if not go try the next keyword

          ; actually this works even if the input buffer byte is the
          ; end marker, i.e. a shifted character. As you can't enter
          ; any keywords as a single shifted character, see above,
          ; you can enter keywords in shorthand by shifting any
          ; character after the first. so RETURN can be entered as
          ; R[SHIFT]E, RE[SHIFT]T, RET[SHIFT]U or RETU[SHIFT]R.
          ; RETUR[SHIFT]N however will not work because the [SHIFT]N
          ; will match the RETURN end marker so the routine will try
          ; to match the next character.

          ; else found keyword
  ORA COUNT     ; OR with word index, +$80 in .A makes token
LAB_C5C7
  LDY FBUFPT      ; restore save index

; save byte then continue crunching

LAB_C5C9
  INX       ; increment buffer read index
  INY       ; increment save index
  STA BUF-5,Y     ; save byte to output
  LDA BUF-5,Y     ; get byte from output, set flags
  BEQ LAB_C609    ; branch if was null [EOL]

          ; .A holds the token here
  SEC       ; set carry for subtract
  SBC #':'      ; subtract ":"
  BEQ LAB_C5DC    ; branch if it was (is now $00)

          ; .A now holds token-':'
  CMP #TK_DATA-':'    ; compare with the token for DATA-':'
  BNE LAB_C5DE    ; if not DATA go try REM

          ; token was : or DATA
LAB_C5DC
  STA GARBFL      ; save token-':'
LAB_C5DE
  SEC       ; set carry for subtract
  SBC #TK_REM-':'   ; subtract the token for REM-':'
  BNE LAB_C582    ; if wasn't REM go crunch next bit of line

  STA ENDCHR      ; else was REM so set search for [EOL]

          ; loop for "..." etc.
LAB_C5E5
  LDA BUF,X     ; get byte from input buffer
  BEQ LAB_C5C9    ; if null [EOL] save byte then continue crunching

  CMP ENDCHR      ; compare with stored character
  BEQ LAB_C5C9    ; if match save byte then continue crunching

LAB_C5EE
  INY       ; increment save index
  STA BUF-5,Y     ; save byte to output
  INX       ; increment buffer index
  BNE LAB_C5E5    ; loop while <> 0, should never reach 0

          ; not found keyword this go
LAB_C5F5
  LDX CHRGOT+1    ; restore BASIC execute pointer low byte
  INC COUNT     ; increment word index (next word)

          ; now find end of this word in the table
LAB_C5F9
  INY       ; increment table index
  LDA RESLST-1,Y    ; get table byte
  BPL LAB_C5F9    ; loop if not end of word yet

  LDA RESLST,Y    ; get byte from keyword table
  BNE LAB_C5B8    ; go test next word if not zero byte, end of table

          ; reached end of table with no match
  LDA BUF,X     ; restore byte from input buffer
  BPL LAB_C5C7    ; branch always, all unmatched bytes in the buffer are
          ; $00 to $7F, go save byte in output and continue crunching

          ; reached [EOL]
LAB_C609
  STA BUF-3,Y     ; save [EOL]
  DEC CHRGOT+2    ; decrement BASIC execute pointer high byte
  LDA #$FF      ; point to start of buffer - 1
  STA CHRGOT+1    ; set BASIC execute pointer low byte
  RTS


;***********************************************************************************;
;
; search BASIC for temporary integer line number

FINLIN
  LDA TXTTAB      ; get start of memory low byte
  LDX TXTTAB+1    ; get start of memory high byte

; search BASIC for temp integer line number from .A.X
; returns carry set if found

LAB_C617
  LDY #$01      ; set index to next line pointer high byte
  STA TMPPTR      ; save low byte as current
  STX TMPPTR+1    ; save high byte as current
  LDA (TMPPTR),Y    ; get next line pointer high byte from address
  BEQ LAB_C640    ; pointer was zero so done, exit

  INY       ; increment index ...
  INY       ; ... to line # high byte
  LDA LINNUM+1    ; get temporary integer high byte
  CMP (TMPPTR),Y    ; compare with line # high byte
  BCC LAB_C641    ; exit if temp < this line, target line passed

  BEQ LAB_C62E    ; go check low byte if =

  DEY       ; else decrement index
  BNE LAB_C637    ; branch always

LAB_C62E
  LDA LINNUM      ; get temporary integer low byte
  DEY       ; decrement index to line # low byte
  CMP (TMPPTR),Y    ; compare with line # low byte
  BCC LAB_C641    ; exit if temp < this line, target line passed

  BEQ LAB_C641    ; exit if temp = (found line#)

          ; not quite there yet
LAB_C637
  DEY       ; decrement index to next line pointer high byte
  LDA (TMPPTR),Y    ; get next line pointer high byte
  TAX       ; copy to .X
  DEY       ; decrement index to next line pointer low byte
  LDA (TMPPTR),Y    ; get next line pointer low byte
  BCS LAB_C617    ; go search for line # in temporary integer
          ; from .A.X, carry always set

LAB_C640
  CLC       ; clear found flag
LAB_C641
  RTS


;***********************************************************************************;
;
; perform NEW

NEW
  BNE LAB_C641    ; exit if following byte to allow syntax error

LAB_C644
  LDA #$00      ; clear .A
  TAY       ; clear index
  STA (TXTTAB),Y    ; clear pointer to next line low byte
  INY       ; increment index
  STA (TXTTAB),Y    ; clear pointer to next line high byte, erase program

  LDA TXTTAB      ; get start of memory low byte
  CLC       ; clear carry for add
  ADC #$02      ; add null program length
  STA VARTAB      ; set start of variables low byte
  LDA TXTTAB+1    ; get start of memory high byte
  ADC #$00      ; add carry
  STA VARTAB+1    ; set start of variables high byte

; reset execute pointer and do CLR

LAB_C659
  JSR STXTPT      ; set BASIC execute pointer to start of memory - 1
  LDA #$00      ; set Zb for CLR entry


;***********************************************************************************;
;
; perform CLR

CLR
  BNE LAB_C68D    ; exit if following byte to allow syntax error

LAB_C660
  JSR CLALL     ; close all channels and files
LAB_C663
  LDA MEMSIZ      ; get end of memory low byte
  LDY MEMSIZ+1    ; get end of memory high byte
  STA FRETOP      ; set bottom of string space low byte, clear strings
  STY FRETOP+1    ; set bottom of string space high byte
  LDA VARTAB      ; get start of variables low byte
  LDY VARTAB+1    ; get start of variables high byte
  STA ARYTAB      ; set end of variables low byte, clear variables
  STY ARYTAB+1    ; set end of variables high byte
  STA STREND      ; set end of arrays low byte, clear arrays
  STY STREND+1    ; set end of arrays high byte


;***********************************************************************************;
;
; do RESTORE and clear the stack

LAB_C677
  JSR RESTORE     ; perform RESTORE

; flush BASIC stack and clear the continue pointer

LAB_C67A
  LDX #TEMPST     ; get descriptor stack start
  STX TEMPPT      ; set descriptor stack pointer
  PLA       ; pull return address low byte
  TAY       ; copy it
  PLA       ; pull return address high byte
  LDX #$FA      ; set cleared stack pointer
  TXS       ; set stack
  PHA       ; push return address high byte
  TYA       ; restore return address low byte
  PHA       ; push return address low byte
  LDA #$00      ; clear .A
  STA OLDTXT+1    ; clear continue pointer high byte
  STA SUBFLG      ; clear subscript/FNx flag
LAB_C68D
  RTS


;***********************************************************************************;
;
; set BASIC execute pointer to start of memory - 1

STXTPT
  CLC       ; clear carry for add
  LDA TXTTAB      ; get start of memory low byte
  ADC #$FF      ; add -1 low byte
  STA CHRGOT+1    ; set BASIC execute pointer low byte
  LDA TXTTAB+1    ; get start of memory high byte
  ADC #$FF      ; add -1 high byte
  STA CHRGOT+2    ; save BASIC execute pointer high byte
  RTS


;***********************************************************************************;
;
; perform LIST

LIST
  BCC LAB_C6A4    ; branch if next character not token (LIST n...)

  BEQ LAB_C6A4    ; branch if next character [NULL] (LIST)

  CMP #TK_MINUS   ; compare with token for "-"
  BNE LAB_C68D    ; exit if not - (LIST -m)

          ; LIST [[n][-m]]
          ; this bit sets the n, if present, as the start and end
LAB_C6A4
  JSR DECBIN      ; get fixed-point number into temporary integer
  JSR FINLIN      ; search BASIC for temporary integer line number
  JSR CHRGOT      ; scan memory
  BEQ LAB_C6BB    ; branch if no more chrs

          ; this bit checks the - is present
  CMP #TK_MINUS   ; compare with token for "-"
  BNE LAB_C641    ; return if not "-" (will be SN error)

          ; LIST [n]-m
          ; the - was there so set m as the end value
  JSR CHRGET      ; increment and scan memory
  JSR DECBIN      ; get fixed-point number into temporary integer
  BNE LAB_C641    ; exit if not ok

LAB_C6BB
  PLA       ; dump return address low byte, exit via warm start
  PLA       ; dump return address high byte
  LDA LINNUM      ; get temporary integer low byte
  ORA LINNUM+1    ; OR temporary integer high byte
  BNE LAB_C6C9    ; branch if start set

  LDA #$FF      ; set for -1
  STA LINNUM      ; set temporary integer low byte
  STA LINNUM+1    ; set temporary integer high byte
LAB_C6C9
  LDY #$01      ; set index for line
  STY GARBFL      ; clear open quote flag
  LDA (TMPPTR),Y    ; get next line pointer high byte
  BEQ LAB_C714    ; if null all done so exit

  JSR TSTSTOP     ; do STOP check vector
  JSR LAB_CAD7    ; print CR/LF
  INY       ; increment index for line
  LDA (TMPPTR),Y    ; get line number low byte
  TAX       ; copy to .X
  INY       ; increment index
  LDA (TMPPTR),Y    ; get line number high byte
  CMP LINNUM+1    ; compare with temporary integer high byte
  BNE LAB_C6E6    ; branch if no high byte match

  CPX LINNUM      ; compare with temporary integer low byte
  BEQ LAB_C6E8    ; branch if = last line to do, < will pass next branch

LAB_C6E6        ; else ...
  BCS LAB_C714    ; if greater all done so exit

LAB_C6E8
  STY FORPNT      ; save index for line
  JSR PRTFIX      ; print .X.A as unsigned integer
  LDA #' '      ; space is the next character
LAB_C6EF
  LDY FORPNT      ; get index for line
  AND #$7F      ; mask top out bit of character
LAB_C6F3
  JSR LAB_CB47    ; go print the character
  CMP #$22      ; was it " character
  BNE LAB_C700    ; if not skip the quote handle

          ; we are either entering or leaving a pair of quotes
  LDA GARBFL      ; get open quote flag
  EOR #$FF      ; toggle it
  STA GARBFL      ; save it back
LAB_C700
  INY       ; increment index
  BEQ LAB_C714    ; line too long so just bail out and do a warm start

  LDA (TMPPTR),Y    ; get next byte
  BNE LAB_C717    ; if not [EOL] (go print character)

          ; was [EOL]
  TAY       ; else clear index
  LDA (TMPPTR),Y    ; get next line pointer low byte
  TAX       ; copy to .X
  INY       ; increment index
  LDA (TMPPTR),Y    ; get next line pointer high byte
  STX TMPPTR      ; set pointer to line low byte
  STA TMPPTR+1    ; set pointer to line high byte
  BNE LAB_C6C9    ; go do next line if not [EOT]
          ; else ...
LAB_C714
  JMP READY     ; do warm start


;***********************************************************************************;
;
LAB_C717
  JMP (IQPLOP)    ; do uncrunch BASIC tokens


;***********************************************************************************;
;
; uncrunch BASIC tokens, the uncrunch BASIC tokens vector is initialised to point here

QPLOP
  BPL LAB_C6F3    ; just go print it if not token byte
          ; else was token byte so uncrunch it

  CMP #TK_PI      ; compare with the token for PI. in this case the token
          ; is the same as the PI character so it just needs printing
  BEQ LAB_C6F3    ; just print it if so

  BIT GARBFL      ; test the open quote flag
  BMI LAB_C6F3    ; just go print character if open quote set

  SEC       ; else set carry for subtract
  SBC #$7F      ; reduce token range to 1 to whatever
  TAX       ; copy token # to .X
  STY FORPNT      ; save index for line
  LDY #$FF      ; start from -1, adjust for pre increment
LAB_C72C
  DEX       ; decrement token #
  BEQ LAB_C737    ; if now found go do printing

LAB_C72F
  INY       ; else increment index
  LDA RESLST,Y    ; get byte from keyword table
  BPL LAB_C72F    ; loop until keyword end marker

  BMI LAB_C72C    ; go test if this is required keyword, branch always

          ; found keyword, it's the next one
LAB_C737
  INY       ; increment keyword table index
  LDA RESLST,Y    ; get byte from table
  BMI LAB_C6EF    ; go restore index, mask byte and print if
          ; byte was end marker

  JSR LAB_CB47    ; else go print the character
  BNE LAB_C737    ; go get next character, branch always


;***********************************************************************************;
;
; perform FOR

FOR
  LDA #$80      ; set FNx
  STA SUBFLG      ; set subscript/FNx flag
  JSR LET     ; perform LET
  JSR SCNSTK      ; search the stack for FOR or GOSUB activity
  BNE LAB_C753    ; branch if FOR, this variable, not found

          ; FOR, this variable, was found so first we dump the old one
  TXA       ; copy index
  ADC #$0F      ; add FOR structure size-2
  TAX       ; copy to index
  TXS       ; set stack (dump FOR structure (-2 bytes))
LAB_C753
  PLA       ; pull return address
  PLA       ; pull return address
  LDA #$09      ; we need 18d bytes !
  JSR STKSPC      ; check room on stack for 2*.A bytes
  JSR FIND2     ; scan for next BASIC statement ([:] or [EOL])
  CLC       ; clear carry for add
  TYA       ; copy index to .A
  ADC CHRGOT+1    ; add BASIC execute pointer low byte
  PHA       ; push onto stack
  LDA CHRGOT+2    ; get BASIC execute pointer high byte
  ADC #$00      ; add carry
  PHA       ; push onto stack
  LDA CURLIN+1    ; get current line number high byte
  PHA       ; push onto stack
  LDA CURLIN      ; get current line number low byte
  PHA       ; push onto stack
  LDA #TK_TO      ; set TO token
  JSR SYNCHR      ; scan for CHR$(.A), else do syntax error then warm start
  JSR LAB_CD8D    ; check if source is numeric, else do type mismatch
  JSR TYPCHK      ; evaluate expression and check is numeric, else do
          ; type mismatch
  LDA FAC1+FAC_SIGN   ; get FAC1 sign (b7)
  ORA #$7F      ; set all non sign bits
  AND FAC1+FAC_MANT   ; and FAC1 mantissa 1
  STA FAC1+FAC_MANT   ; save FAC1 mantissa 1
  LDA #<LAB_C78B    ; set return address low byte
  LDY #>LAB_C78B    ; set return address high byte
  STA INDEX     ; save return address low byte
  STY INDEX+1     ; save return address high byte
  JMP LAB_CE43    ; round FAC1 and put on stack, returns to next instruction

LAB_C78B
  LDA #<FPC1      ; set 1 pointer low address, default step size
  LDY #>FPC1      ; set 1 pointer high address
  JSR LODFAC      ; unpack memory (.A.Y) into FAC1
  JSR CHRGOT      ; scan memory
  CMP #TK_STEP    ; compare with STEP token
  BNE LAB_C79F    ; branch if not STEP

          ; was step so ...
  JSR CHRGET      ; increment and scan memory
  JSR TYPCHK      ; evaluate expression and check is numeric, else do
          ; type mismatch
LAB_C79F
  JSR SGNFAC      ; get FAC1 sign, return .A = $FF -ve, .A = $01 +ve
  JSR LAB_CE38    ; push sign, round FAC1 and put on stack
  LDA FORPNT+1    ; get FOR/NEXT variable pointer high byte
  PHA       ; push on stack
  LDA FORPNT      ; get FOR/NEXT variable pointer low byte
  PHA       ; push on stack
  LDA #TK_FOR     ; get FOR token
  PHA       ; push on stack


;***********************************************************************************;
;
; interpreter inner loop

NEWSTT
  JSR TSTSTOP     ; do STOP check vector
  LDA CHRGOT+1    ; get BASIC execute pointer low byte
  LDY CHRGOT+2    ; get BASIC execute pointer high byte
  CPY #>BUF     ; compare with input buffer high byte
  NOP       ; unused byte
  BEQ LAB_C7BE    ; if immediate mode skip the continue pointer save

  STA OLDTXT      ; save the continue pointer low byte
  STY OLDTXT+1    ; save the continue pointer high byte
LAB_C7BE
  LDY #$00      ; clear the index
  LDA (CHRGOT+1),Y    ; get BASIC byte
  BNE LAB_C807    ; if not [EOL] go test for ":"

  LDY #$02      ; else set the index
  LDA (CHRGOT+1),Y    ; get next line pointer high byte
  CLC       ; clear carry for no "BREAK" message
  BNE LAB_C7CE    ; branch if not end of program

  JMP LAB_C84B    ; else go to immediate mode,was immediate or [EOT] marker

LAB_C7CE
  INY       ; increment index
  LDA (CHRGOT+1),Y    ; get line number low byte
  STA CURLIN      ; save current line number low byte
  INY       ; increment index
  LDA (CHRGOT+1),Y    ; get line # high byte
  STA CURLIN+1    ; save current line number high byte
  TYA       ; .A now = 4
  ADC CHRGOT+1    ; add BASIC execute pointer low byte, now points to code
  STA CHRGOT+1    ; save BASIC execute pointer low byte
  BCC LAB_C7E1    ; if no overflow skip the high byte increment

  INC CHRGOT+2    ; else increment BASIC execute pointer high byte
LAB_C7E1
  JMP (IGONE)     ; do start new BASIC code


;***********************************************************************************;
;
; start new BASIC code, the start new BASIC code vector is initialised to point here

GONE
  JSR CHRGET      ; increment and scan memory
  JSR LAB_C7ED    ; go interpret BASIC code from BASIC execute pointer
  JMP NEWSTT      ; loop


;***********************************************************************************;
;
; go interpret BASIC code from BASIC execute pointer

LAB_C7ED
  BEQ LAB_C82B    ; if the first byte is null just exit

LAB_C7EF
  SBC #$80      ; normalise the token
  BCC LAB_C804    ; if wasn't token go do LET

  CMP #TK_TAB-$80   ; compare with token for TAB(-$80
  BCS LAB_C80E    ; branch if >= TAB(

  ASL       ; *2 bytes per vector
  TAY       ; copy to index
  LDA STMDSP+1,Y    ; get vector high byte
  PHA       ; push on stack
  LDA STMDSP,Y    ; get vector low byte
  PHA       ; push on stack
  JMP CHRGET      ; increment and scan memory and return. the return in
          ; this case calls the command code, the return from
          ; that will eventually return to the interpreter inner
          ; loop above

LAB_C804
  JMP LET     ; perform LET

          ; was not [EOL]
LAB_C807
  CMP #':'      ; compare with ":"
  BEQ LAB_C7E1    ; if ":" go execute new code

          ; else ...
LAB_C80B
  JMP LAB_CF08    ; do syntax error then warm start

          ; token was >= TAB(
LAB_C80E
  CMP #TK_GO-$80    ; compare with token for GO
  BNE LAB_C80B    ; if not GO do syntax error then warm start

          ; else was GO
  JSR CHRGET      ; increment and scan memory
  LDA #TK_TO      ; set TO token
  JSR SYNCHR      ; scan for CHR$(.A), else do syntax error then warm start
  JMP GOTO      ; perform GOTO


;***********************************************************************************;
;
; perform RESTORE

RESTORE
  SEC       ; set carry for subtract
  LDA TXTTAB      ; get start of memory low byte
  SBC #$01      ; -1
  LDY TXTTAB+1    ; get start of memory high byte
  BCS LAB_C827    ; if no rollunder skip the high byte decrement

  DEY       ; else decrement high byte
LAB_C827
  STA DATPTR      ; set DATA pointer low byte
  STY DATPTR+1    ; set DATA pointer high byte
LAB_C82B
  RTS


;***********************************************************************************;
;
; do STOP check vector

TSTSTOP
  JSR STOP      ; scan stop key


;***********************************************************************************;
;
; perform STOP

BSTOP
  BCS LAB_C832    ; if carry set do BREAK instead of just END


;***********************************************************************************;
;
; perform END

END
  CLC       ; clear carry
LAB_C832
  BNE LAB_C870    ; return if wasn't STOP

  LDA CHRGOT+1    ; get BASIC execute pointer low byte
  LDY CHRGOT+2    ; get BASIC execute pointer high byte
  LDX CURLIN+1    ; get current line number high byte
  INX       ; increment it
  BEQ LAB_C849    ; branch if was immediate mode

  STA OLDTXT      ; save continue pointer low byte
  STY OLDTXT+1    ; save continue pointer high byte
  LDA CURLIN      ; get current line number low byte
  LDY CURLIN+1    ; get current line number high byte
  STA OLDLIN      ; save break line number low byte
  STY OLDLIN+1    ; save break line number high byte
LAB_C849
  PLA       ; dump return address low byte
  PLA       ; dump return address high byte
LAB_C84B
  LDA #<CRLFBRK   ; set [CR][LF]"BREAK" pointer low byte
  LDY #>CRLFBRK   ; set [CR][LF]"BREAK" pointer high byte
  BCC LAB_C854    ; branch if was program end

  JMP PRDY      ; print string and do warm start

LAB_C854
  JMP READY     ; do warm start


;***********************************************************************************;
;
; perform CONT

CONT
  BNE LAB_C870    ; exit if following byte to allow syntax error

  LDX #ER_CANTCONT    ; error code $1A, can't continue error
  LDY OLDTXT+1    ; get continue pointer high byte
  BNE LAB_C862    ; go do continue if we can

  JMP ERROR     ; else do error #.X then warm start

          ; we can continue so ...
LAB_C862
  LDA OLDTXT      ; get continue pointer low byte
  STA CHRGOT+1    ; save BASIC execute pointer low byte
  STY CHRGOT+2    ; save BASIC execute pointer high byte
  LDA OLDLIN      ; get break line low byte
  LDY OLDLIN+1    ; get break line high byte
  STA CURLIN      ; set current line number low byte
  STY CURLIN+1    ; set current line number high byte
LAB_C870
  RTS


;***********************************************************************************;
;
; perform RUN

RUN
  PHP       ; save status
  LDA #$00      ; no control or KERNAL messages
  JSR SETMSG      ; control KERNAL messages
  PLP       ; restore status
  BNE LAB_C87D    ; branch if RUN n

  JMP LAB_C659    ; reset execution to start, clear variables, flush stack
          ; and return
LAB_C87D
  JSR LAB_C660    ; go do CLR
  JMP LAB_C897    ; get n and do GOTO n


;***********************************************************************************;
;
; perform GOSUB

GOSUB
  LDA #$03      ; need 6 bytes for GOSUB
  JSR STKSPC      ; check room on stack for 2*.A bytes
  LDA CHRGOT+2    ; get BASIC execute pointer high byte
  PHA       ; save it
  LDA CHRGOT+1    ; get BASIC execute pointer low byte
  PHA       ; save it
  LDA CURLIN+1    ; get current line number high byte
  PHA       ; save it
  LDA CURLIN      ; get current line number low byte
  PHA       ; save it
  LDA #TK_GOSUB   ; token for GOSUB
  PHA       ; save it
LAB_C897
  JSR CHRGOT      ; scan memory
  JSR GOTO      ; perform GOTO
  JMP NEWSTT      ; go do interpreter inner loop


;***********************************************************************************;
;
; perform GOTO

GOTO
  JSR DECBIN      ; get fixed-point number into temporary integer
  JSR LAB_C909    ; scan for next BASIC line
  SEC       ; set carry for subtract
  LDA CURLIN      ; get current line number low byte
  SBC LINNUM      ; subtract temporary integer low byte
  LDA CURLIN+1    ; get current line number high byte
  SBC LINNUM+1    ; subtract temporary integer high byte
  BCS LAB_C8BC    ; if current line number >= temporary integer, go search
          ; from the start of memory

  TYA       ; else copy line index to .A
  SEC       ; set carry (+1)
  ADC CHRGOT+1    ; add BASIC execute pointer low byte
  LDX CHRGOT+2    ; get BASIC execute pointer high byte
  BCC LAB_C8C0    ; if no overflow skip the high byte increment

  INX       ; increment high byte
  BCS LAB_C8C0    ; go find the line, branch always


;***********************************************************************************;
;
; search for line number in temporary integer from start of memory pointer

LAB_C8BC
  LDA TXTTAB      ; get start of memory low byte
  LDX TXTTAB+1    ; get start of memory high byte

; search for line # in temporary integer from (.A.X)

LAB_C8C0
  JSR LAB_C617    ; search BASIC for temp integer line number from .A.X
  BCC LAB_C8E3    ; if carry clear go do undefined statement error

          ; carry all ready set for subtract
  LDA TMPPTR      ; get pointer low byte
  SBC #$01      ; -1
  STA CHRGOT+1    ; save BASIC execute pointer low byte
  LDA TMPPTR+1    ; get pointer high byte
  SBC #$00      ; subtract carry
  STA CHRGOT+2    ; save BASIC execute pointer high byte
LAB_C8D1
  RTS


;***********************************************************************************;
;
; perform RETURN

RETURN
  BNE LAB_C8D1    ; exit if following token to allow syntax error

  LDA #$FF      ; set byte so no match possible
  STA FORPNT+1    ; save FOR/NEXT variable pointer high byte
  JSR SCNSTK      ; search the stack for FOR or GOSUB activity,
          ; get token off stack
  TXS       ; correct the stack
  CMP #TK_GOSUB   ; compare with GOSUB token
  BEQ LAB_C8EB    ; if matching GOSUB go continue RETURN

  LDX #ER_RETWOGSB    ; else error code $0C, return without gosub error
  .byte $2C     ; makes next line BIT $11A2
LAB_C8E3
  LDX #ER_UNDSMNT   ; error code $11, undefined statement error
  JMP ERROR     ; do error #.X then warm start

LAB_C8E8
  JMP LAB_CF08    ; do syntax error then warm start

          ; was matching GOSUB token
LAB_C8EB
  PLA       ; dump token byte
  PLA       ; pull return line low byte
  STA CURLIN      ; save current line number low byte
  PLA       ; pull return line high byte
  STA CURLIN+1    ; save current line number high byte
  PLA       ; pull return address low byte
  STA CHRGOT+1    ; save BASIC execute pointer low byte
  PLA       ; pull return address high byte
  STA CHRGOT+2    ; save BASIC execute pointer high byte


;***********************************************************************************;
;
; perform DATA

SKIPST
  JSR FIND2     ; scan for next BASIC statement ([:] or [EOL])

; add .Y to the BASIC execute pointer

BUMPTP
  TYA       ; copy index to .A
  CLC       ; clear carry for add
  ADC CHRGOT+1    ; add BASIC execute pointer low byte
  STA CHRGOT+1    ; save BASIC execute pointer low byte
  BCC LAB_C905    ; skip increment if no carry

  INC CHRGOT+2    ; else increment BASIC execute pointer high byte
LAB_C905
  RTS


;***********************************************************************************;
;
; scan for next BASIC statement ([:] or [EOL])
; returns .Y as index to [:] or [EOL]

FIND2
  LDX #':'      ; set look for character = ":"
  .byte $2C     ; makes next line BIT $00A2

; scan for next BASIC line
; returns .Y as index to [EOL]

LAB_C909
  LDX #$00      ; set alternate search character = [EOL]
  STX CHARAC      ; store alternate search character
  LDY #$00      ; set search character = [EOL]
  STY ENDCHR      ; save the search character
LAB_C911
  LDA ENDCHR      ; get search character
  LDX CHARAC      ; get alternate search character
  STA CHARAC      ; make search character = alternate search character
  STX ENDCHR      ; make alternate search character = search character
LAB_C919
  LDA (CHRGOT+1),Y    ; get BASIC byte
  BEQ LAB_C905    ; exit if null [EOL]

  CMP ENDCHR      ; compare with search character
  BEQ LAB_C905    ; exit if found

  INY       ; else increment index
  CMP #$22      ; compare current character with open quote
  BNE LAB_C919    ; if found go swap search character for alternate search
          ; character

  BEQ LAB_C911    ; loop for next character, branch always


;***********************************************************************************;
;
; perform IF

IF
  JSR FRMEVL      ; evaluate expression
  JSR CHRGOT      ; scan memory
  CMP #TK_GOTO    ; compare with GOTO token
  BEQ LAB_C937    ; if it was the token for GOTO go do IF ... GOTO

          ; wasn't IF ... GOTO so must be IF ... THEN
  LDA #TK_THEN    ; $A7 = "THEN" token
  JSR SYNCHR      ; scan for CHR$(.A), else do syntax error then warm start
LAB_C937
  LDA FAC1+FAC_EXPT   ; get FAC1 exponent
  BNE LAB_C940    ; if result was non zero continue execution
          ; else REM the rest of the line


;***********************************************************************************;
;
; perform REM

REM
  JSR LAB_C909    ; scan for next BASIC line
  BEQ BUMPTP      ; add .Y to the BASIC execute pointer and return, branch
          ; always


;***********************************************************************************;
;
; IF continued .. result was non zero so do rest of line

LAB_C940
  JSR CHRGOT      ; scan memory
  BCS LAB_C948    ; if not numeric character, is variable or keyword

  JMP GOTO      ; else perform GOTO n

          ; is variable or keyword
LAB_C948
  JMP LAB_C7ED    ; interpret BASIC code from BASIC execute pointer


;***********************************************************************************;
;
; perform ON

ON
  JSR LAB_D79E    ; get byte parameter
  PHA       ; push next character
  CMP #TK_GOSUB   ; compare with GOSUB token
  BEQ LAB_C957    ; if GOSUB go see if it should be executed

LAB_C953
  CMP #TK_GOTO    ; compare with GOTO token
  BNE LAB_C8E8    ; if not GOTO do syntax error then warm start

; next character was GOTO or GOSUB, see if it should be executed

LAB_C957
  DEC FAC1+4      ; decrement the byte value
  BNE LAB_C95F    ; if not zero go see if another line number exists

  PLA       ; pull keyword token
  JMP LAB_C7EF    ; go execute it

LAB_C95F
  JSR CHRGET      ; increment and scan memory
  JSR DECBIN      ; get fixed-point number into temporary integer
          ; skip this n
  CMP #','      ; compare next character with ","
  BEQ LAB_C957    ; loop if ","

  PLA       ; else pull keyword token, ran out of options
LAB_C96A
  RTS


;***********************************************************************************;
;
; get fixed-point number into temporary integer

DECBIN
  LDX #$00      ; clear .X
  STX LINNUM      ; clear temporary integer low byte
  STX LINNUM+1    ; clear temporary integer high byte
LAB_C971
  BCS LAB_C96A    ; return if carry set, end of scan, character was not 0-9

  SBC #$2F      ; subtract $30, $2F+carry, from byte
  STA CHARAC      ; store #
  LDA LINNUM+1    ; get temporary integer high byte
  STA INDEX     ; save it for now
  CMP #$19      ; compare with $19
  BCS LAB_C953    ; branch if >= this makes the maximum line number 63999
          ; because the next bit does $1900 * $0A = $FA00 = 64000
          ; decimal. the branch target is really the SYNTAX error
          ; at LAB_C8E8 but that is too far so an intermediate
          ; compare and branch to that location is used. the problem
          ; with this is that line number that gives a partial result
          ; from $8900 to $89FF, 35072x to 35327x, will pass the new
          ; target compare and will try to execute the remainder of
          ; the ON n GOTO/GOSUB. a solution to this is to copy the
          ; byte in .A before the branch to .X and then branch to
          ; LAB_C955 skipping the second compare

  LDA LINNUM      ; get temporary integer low byte
  ASL       ; *2 low byte
  ROL INDEX     ; *2 high byte
  ASL       ; *2 low byte
  ROL INDEX     ; *2 high byte (*4)
  ADC LINNUM      ; + low byte (*5)
  STA LINNUM      ; save it
  LDA INDEX     ; get high byte temp
  ADC LINNUM+1    ; + high byte (*5)
  STA LINNUM+1    ; save it
  ASL LINNUM      ; *2 low byte (*10d)
  ROL LINNUM+1    ; *2 high byte (*10d)
  LDA LINNUM      ; get low byte
  ADC CHARAC      ; add #
  STA LINNUM      ; save low byte
  BCC LAB_C99F    ; if no overflow skip high byte increment

  INC LINNUM+1    ; else increment high byte
LAB_C99F
  JSR CHRGET      ; increment and scan memory
  JMP LAB_C971    ; loop for next character


;***********************************************************************************;
;
; perform LET

LET
  JSR EVLVAR      ; get variable address
  STA FORPNT      ; save variable address low byte
  STY FORPNT+1    ; save variable address high byte
  LDA #TK_EQUAL   ; $B2 is "=" token
  JSR SYNCHR      ; scan for CHR$(.A), else do syntax error then warm start
  LDA INTFLG      ; get data type flag, $80 = integer, $00 = float
  PHA       ; push data type flag
  LDA VALTYP      ; get data type flag, $FF = string, $00 = numeric
  PHA       ; push data type flag
  JSR FRMEVL      ; evaluate expression
  PLA       ; pop data type flag
  ROL       ; string bit into carry
  JSR LAB_CD90    ; do type match check
  BNE LAB_C9D9    ; if string go assign a string value

  PLA       ; pop integer/float data type flag

; assign value to numeric variable

LET2
  BPL LAB_C9D6    ; if float go assign a floating value

          ; expression is numeric integer
  JSR ROUND     ; round FAC1
  JSR MAKINT      ; evaluate integer expression, no sign check
  LDY #$00      ; clear index
  LDA FAC1+FAC_MANT+2   ; get FAC1 mantissa 3
  STA (FORPNT),Y    ; save as integer variable low byte
  INY       ; increment index
  LDA FAC1+FAC_MANT+3   ; get FAC1 mantissa 4
  STA (FORPNT),Y    ; save as integer variable high byte
  RTS

LAB_C9D6
  JMP FACTFP      ; pack FAC1 into variable pointer and return

; assign value to string variable

LAB_C9D9
  PLA       ; dump integer/float data type flag
LET5
  LDY FORPNT+1    ; get variable pointer high byte
  CPY #>NULLVAR   ; was it TI$ pointer
  BNE LET9      ; branch if not

          ; else it's TI$ = <expr$>
  JSR LAB_D6A6    ; pop string off descriptor stack, or from top of string
          ; space returns with .A = length, .X = pointer low byte,
          ; .Y = pointer high byte
  CMP #$06      ; compare length with 6
  BNE LAB_CA24    ; if length not 6 do illegal quantity error then warm start

  LDY #$00      ; clear index
  STY FAC1+FAC_EXPT   ; clear FAC1 exponent
  STY FAC1+FAC_SIGN   ; clear FAC1 sign (b7)
LAB_C9ED
  STY FBUFPT      ; save index
  JSR LAB_CA1D    ; check and evaluate numeric digit
  JSR MULTEN      ; multiply FAC1 by 10
  INC FBUFPT      ; increment index
  LDY FBUFPT      ; restore index
  JSR LAB_CA1D    ; check and evaluate numeric digit
  JSR RFTOA     ; round and copy FAC1 to FAC2
  TAX       ; copy FAC1 exponent
  BEQ LAB_CA07    ; branch if FAC1 zero

  INX       ; increment index, * 2
  TXA       ; copy back to .A
  JSR LAB_DAED    ; FAC1 = (FAC1 + (FAC2 * 2)) * 2 = FAC1 * 6
LAB_CA07
  LDY FBUFPT      ; get index
  INY       ; increment index
  CPY #$06      ; compare index with 6
  BNE LAB_C9ED    ; loop if not 6

  JSR MULTEN      ; multiply FAC1 by 10
  JSR FPINT     ; convert FAC1 floating to fixed
  LDX FAC1+FAC_MANT+2   ; get FAC1 mantissa 3
  LDY FAC1+FAC_MANT+1   ; get FAC1 mantissa 2
  LDA FAC1+FAC_MANT+3   ; get FAC1 mantissa 4
  JMP SETTIM      ; set real time clock and return

; check and evaluate numeric digit

LAB_CA1D
  LDA (INDEX),Y   ; get byte from string
  JSR CHRSPC      ; clear Cb if numeric. this call should be to CHRSPC+4
          ; as the code from CHRSPC first compares the byte with
          ; [SPACE] and does a BASIC increment and get if it is
  BCC LAB_CA27    ; branch if numeric

LAB_CA24
  JMP ILQUAN      ; do illegal quantity error then warm start

LAB_CA27
  SBC #$2F      ; subtract $2F + carry to convert ASCII to binary
  JMP ASCI8     ; evaluate new ASCII digit and return

; assign value to string variable, but not TI$

LET9
  LDY #$02      ; index to string pointer high byte
  LDA (FAC1+3),Y    ; get string pointer high byte
  CMP FRETOP+1    ; compare with bottom of string space high byte
  BCC LAB_CA4B    ; branch if string pointer high byte is less than bottom
          ; of string space high byte

  BNE LAB_CA3D    ; branch if string pointer high byte is greater than
          ; bottom of string space high byte

          ; else high bytes were equal
  DEY       ; decrement index to string pointer low byte
  LDA (FAC1+3),Y    ; get string pointer low byte
  CMP FRETOP      ; compare with bottom of string space low byte
  BCC LAB_CA4B    ; branch if string pointer low byte is less than bottom
          ; of string space low byte

LAB_CA3D
  LDY FAC1+4      ; get descriptor pointer high byte
  CPY VARTAB+1    ; compare with start of variables high byte
  BCC LAB_CA4B    ; branch if less, is on string stack

  BNE LAB_CA52    ; if greater make space and copy string

          ; else high bytes were equal
  LDA FAC1+3      ; get descriptor pointer low byte
  CMP VARTAB      ; compare with start of variables low byte
  BCS LAB_CA52    ; if greater or equal make space and copy string

LAB_CA4B
  LDA FAC1+3      ; get descriptor pointer low byte
  LDY FAC1+4      ; get descriptor pointer high byte
  JMP LAB_CA68    ; go copy descriptor to variable

LAB_CA52
  LDY #$00      ; clear index
  LDA (FAC1+3),Y    ; get string length
  JSR ALC1      ; copy descriptor pointer and make string space .A bytes long
  LDA DSCPTN      ; copy old descriptor pointer low byte
  LDY DSCPTN+1    ; copy old descriptor pointer high byte
  STA ARISGN      ; save old descriptor pointer low byte
  STY FACOV     ; save old descriptor pointer high byte
  JSR XFERSTR     ; copy string from descriptor to utility pointer
  LDA #<FAC1      ; get descriptor pointer low byte
  LDY #>FAC1      ; get descriptor pointer high byte
LAB_CA68
  STA DSCPTN      ; save descriptor pointer low byte
  STY DSCPTN+1    ; save descriptor pointer high byte
  JSR DELTSD      ; clean descriptor stack, .Y.A = pointer
  LDY #$00      ; clear index
  LDA (DSCPTN),Y    ; get string length from new descriptor
  STA (FORPNT),Y    ; copy string length to variable
  INY       ; increment index
  LDA (DSCPTN),Y    ; get string pointer low byte from new descriptor
  STA    (FORPNT),Y   ; copy string pointer low byte to variable
  INY       ; increment index
  LDA (DSCPTN),Y    ; get string pointer high byte from new descriptor
  STA (FORPNT),Y    ; copy string pointer high byte to variable
  RTS


;***********************************************************************************;
;
; perform PRINT#

PRINTN
  JSR CMD     ; perform CMD
  JMP LAB_CBB5    ; close input and output channels and return


;***********************************************************************************;
;
; perform CMD

CMD
  JSR LAB_D79E    ; get byte parameter
  BEQ LAB_CA90    ; branch if following byte is ":" or [EOT]

  LDA #','      ; set ","
  JSR SYNCHR      ; scan for CHR$(.A), else do syntax error then warm start
LAB_CA90
  PHP       ; save status
  STX CHANNL      ; set current I/O channel
  JSR LAB_E115    ; open channel for output with error check
  PLP       ; restore status
  JMP PRINT     ; perform PRINT


;***********************************************************************************;
;
; print string, scan memory and continue PRINT

PRT1
  JSR LAB_CB21    ; print string from utility pointer

; scan memory and continue PRINT

LAB_CA9D
  JSR CHRGOT      ; scan memory


;***********************************************************************************;
;
; perform PRINT

PRINT
  BEQ LAB_CAD7    ; if nothing following just print CR/LF

LAB_CAA2
  BEQ LAB_CAE7    ; if nothing following exit, end of PRINT branch

  CMP #TK_TAB     ; compare with token for TAB(
  BEQ PRT7      ; if TAB( go handle it

  CMP #TK_SPC     ; compare with token for SPC(
  CLC       ; flag SPC(
  BEQ PRT7      ; if SPC( go handle it

  CMP #','      ; compare with ","
  BEQ PRT6      ; if "," go skip to the next TAB position

  CMP #$3B      ; compare with ";"
  BEQ LAB_CB13    ; if ";" go continue the print loop

  JSR FRMEVL      ; evaluate expression
  BIT VALTYP      ; test data type flag, $FF = string, $00 = numeric
  BMI PRT1      ; if string go print string, scan memory and continue PRINT

  JSR FLTASC      ; convert FAC1 to ASCII string result in (.A.Y)
  JSR MAKSTR      ; print " terminated string to utility pointer
  JSR LAB_CB21    ; print string from utility pointer
  JSR PRTOS     ; print [SPACE] or [CURSOR RIGHT]
  BNE LAB_CA9D    ; go scan memory and continue PRINT, branch always


;***********************************************************************************;
;
; set .X.Y to BUF - 1 and print [CR]

LAB_CACA
  LDA #$00      ; clear .A
  STA BUF,X     ; clear first byte of input buffer
  LDX #<BUF-1     ; BUF - 1 low byte
  LDY #>BUF-1     ; BUF - 1 high byte
  LDA CHANNL      ; get current I/O channel
  BNE LAB_CAE7    ; exit if not default channel


;***********************************************************************************;
;
; print CR/LF

LAB_CAD7
  LDA #$0D      ; set [CR]
  JSR LAB_CB47    ; print the character
  BIT CHANNL      ; test current I/O channel
  BPL LAB_CAE5    ; if the AutoLF bit is not set skip the [LF]
  LDA #$0A      ; set [LF]
  JSR LAB_CB47    ; print the character


;***********************************************************************************;
;
; invert .A

LAB_CAE5
  EOR #$FF      ; ones' complement .A
LAB_CAE7
  RTS


;***********************************************************************************;
;
; continuing PRINT, the character was ","

PRT6
  SEC       ; set Cb for read cursor position
  JSR PLOT      ; read/set X,Y cursor position
  TYA       ; copy cursor .Y
  SEC       ; set carry for subtract
LAB_CAEE
  SBC #$0B      ; subtract one TAB length
  BCS LAB_CAEE    ; loop if result was +ve

  EOR #$FF      ; complement it
  ADC #$01      ; +1, two's complement
  BNE LAB_CB0E    ; print .A spaces, branch always, result is never $00


;***********************************************************************************;
;
; handle TAB( or SPC(

PRT7
  PHP       ; save TAB( or SPC( status
  SEC       ; set Cb for read cursor position
  JSR PLOT      ; read/set X,Y cursor position
  STY TRMPOS      ; save current cursor position
  JSR GETBYT      ; scan and get byte parameter
  CMP #$29      ; compare with ")"
  BNE LAB_CB5F    ; if not ")" do syntax error

  PLP       ; restore TAB( or SPC( status
  BCC LAB_CB0F    ; branch if was SPC(

          ; else was TAB(
  TXA       ; copy TAB() byte to .A
  SBC TRMPOS      ; subtract current cursor position
  BCC LAB_CB13    ; go loop for next if already past required position

LAB_CB0E
  TAX       ; copy [SPACE] count to .X
LAB_CB0F
  INX       ; increment count
LAB_CB10
  DEX       ; decrement count
  BNE LAB_CB19    ; branch if count was not zero

          ; was ";" or [SPACES] printed
LAB_CB13
  JSR CHRGET      ; increment and scan memory
  JMP LAB_CAA2    ; continue print loop

LAB_CB19
  JSR PRTOS     ; print [SPACE] or [CURSOR RIGHT]
  BNE LAB_CB10    ; loop, branch always


;***********************************************************************************;
;
; print null terminated string

PRTSTR
  JSR MAKSTR      ; print " terminated string to utility pointer

; print string from utility pointer

LAB_CB21
  JSR LAB_D6A6    ; pop string off descriptor stack, or from top of string
          ; space returns with .A = length, .X = pointer low byte,
          ; .Y = pointer high byte
  TAX       ; copy length
  LDY #$00      ; clear index
  INX       ; increment length, for pre decrement loop
LAB_CB28
  DEX       ; decrement length
  BEQ LAB_CAE7    ; exit if done

  LDA (INDEX),Y   ; get byte from string
  JSR LAB_CB47    ; print the character
  INY       ; increment index
  CMP #$0D      ; compare byte with [CR]
  BNE LAB_CB28    ; loop if not [CR]

  JSR LAB_CAE5    ; toggle .A, EOR #$FF. what is the point of this ??
  JMP LAB_CB28    ; loop


;***********************************************************************************;
;
; print [SPACE] or [CURSOR RIGHT]

PRTOS
  LDA CHANNL      ; get current I/O channel
  BEQ LAB_CB42    ; if default channel go output [CURSOR RIGHT]

  LDA #' '      ; else output [SPACE]
  .byte $2C     ; makes next line BIT $1DA9
LAB_CB42
  LDA #$1D      ; set [CURSOR RIGHT]
  .byte $2C     ; makes next line BIT $3FA9


;***********************************************************************************;
;
; print "?"

LAB_CB45
  LDA #'?'      ; set "?"


;***********************************************************************************;
;
; print a character

LAB_CB47
  JSR LAB_E109    ; output character to channel with error check
  AND #$FF      ; set the flags on .A
  RTS


;***********************************************************************************;
;
; bad input routine

IGRERR
  LDA INPFLG      ; get INPUT mode flag, $00 = INPUT, $40 = GET, $98 = READ
  BEQ LAB_CB62    ; branch if INPUT

  BMI LAB_CB57    ; branch if READ

          ; else was GET
  LDY #$FF      ; set current line high byte to -1, indicate immediate mode
  BNE LAB_CB5B    ; branch always

LAB_CB57
  LDA DATLIN      ; get current DATA line number low byte
  LDY DATLIN+1    ; get current DATA line number high byte
LAB_CB5B
  STA CURLIN      ; set current line number low byte
  STY CURLIN+1    ; set current line number high byte
LAB_CB5F
  JMP LAB_CF08    ; do syntax error then warm start

          ; was INPUT
LAB_CB62
  LDA CHANNL      ; get current I/O channel
  BEQ LAB_CB6B    ; if default channel go do "?REDO FROM START" message

  LDX #ER_FDATA   ; else error $18, file data error
  JMP ERROR     ; do error #.X then warm start

LAB_CB6B
  LDA #<LAB_CD0C    ; set "?REDO FROM START" pointer low byte
  LDY #>LAB_CD0C    ; set "?REDO FROM START" pointer high byte
  JSR PRTSTR      ; print null terminated string
  LDA OLDTXT      ; get continue pointer low byte
  LDY OLDTXT+1    ; get continue pointer high byte
  STA CHRGOT+1    ; save BASIC execute pointer low byte
  STY CHRGOT+2    ; save BASIC execute pointer high byte
  RTS


;***********************************************************************************;
;
; perform GET

GET
  JSR NODIRM      ; check not Direct, back here if ok
  CMP #'#'      ; compare with "#"
  BNE LAB_CB92    ; branch if not GET#

  JSR CHRGET      ; increment and scan memory
  JSR LAB_D79E    ; get byte parameter
  LDA #','      ; set ","
  JSR SYNCHR      ; scan for CHR$(.A), else do syntax error then warm start
  STX CHANNL      ; set current I/O channel
  JSR LAB_E11B    ; open channel for input with error check
LAB_CB92
  LDX #<BUF+1     ; set BUF+1 pointer low byte
  LDY #>BUF+1     ; set BUF+1 pointer high byte
  LDA #$00      ; clear .A
  STA BUF+1     ; ensure null terminator
  LDA #$40      ; input mode = GET
  JSR LAB_CC0F    ; perform GET part of READ
  LDX CHANNL      ; get current I/O channel
  BNE LAB_CBB7    ; if not default channel go do channel close and return

  RTS


;***********************************************************************************;
;
; perform INPUT#

INPUTN
  JSR LAB_D79E    ; get byte parameter
  LDA #','      ; set ","
  JSR SYNCHR      ; scan for CHR$(.A), else do syntax error then warm start
  STX CHANNL      ; set current I/O channel
  JSR LAB_E11B    ; open channel for input with error check
  JSR LAB_CBCE    ; perform INPUT with no prompt string

; close input and output channels

LAB_CBB5
  LDA CHANNL      ; get current I/O channel
LAB_CBB7
  JSR CLRCHN      ; close input and output channels
  LDX #$00      ; clear .X
  STX CHANNL      ; clear current I/O channel, flag default
  RTS


;***********************************************************************************;
;
; perform INPUT

INPUT
  CMP #$22      ; compare next byte with open quote
  BNE LAB_CBCE    ; if no prompt string just do INPUT

  JSR LAB_CEBD    ; print "..." string
  LDA #$3B      ; load .A with ";"
  JSR SYNCHR      ; scan for CHR$(.A), else do syntax error then warm start
  JSR LAB_CB21    ; print string from utility pointer

          ; done with prompt, now get data
LAB_CBCE
  JSR NODIRM      ; check not Direct, back here if ok
  LDA #','      ; set ","
  STA BUF-1     ; save to start of buffer - 1
LAB_CBD6
  JSR LAB_CBF9    ; print "? " and get BASIC input
  LDA CHANNL      ; get current I/O channel
  BEQ LAB_CBEA    ; branch if default I/O channel

  JSR READST      ; read I/O status word
  AND #$02      ; mask no DSR/timeout
  BEQ LAB_CBEA    ; branch if not error

  JSR LAB_CBB5    ; close input and output channels
  JMP SKIPST      ; perform DATA

LAB_CBEA
  LDA BUF     ; get first byte in input buffer
  BNE LAB_CC0D    ; branch if not null

          ; else ..
  LDA CHANNL      ; get current I/O channel
  BNE LAB_CBD6    ; if not default channel go get BASIC input

  JSR FIND2     ; scan for next BASIC statement ([:] or [EOL])
  JMP BUMPTP      ; add .Y to the BASIC execute pointer and return


;***********************************************************************************;
;
; print "? " and get BASIC input

LAB_CBF9
  LDA CHANNL      ; get current I/O channel
  BNE LAB_CC03    ; skip "?" prompt if not default channel

  JSR LAB_CB45    ; print "?"
  JSR PRTOS     ; print [SPACE] or [CURSOR RIGHT]
LAB_CC03
  JMP GETLIN      ; call for BASIC input and return


;***********************************************************************************;
;
; perform READ

READ
  LDX DATPTR      ; get DATA pointer low byte
  LDY DATPTR+1    ; get DATA pointer high byte
  LDA #$98      ; set input mode = READ
  .byte $2C     ; makes next line BIT $00A9
LAB_CC0D
  LDA #$00      ; set input mode = INPUT


;***********************************************************************************;
;
; perform GET

LAB_CC0F
  STA INPFLG      ; set input mode flag, $00 = INPUT, $40 = GET, $98 = READ
  STX INPPTR      ; save READ pointer low byte
  STY INPPTR+1    ; save READ pointer high byte

          ; READ, GET or INPUT next variable from list
LAB_CC15
  JSR EVLVAR      ; get variable address
  STA FORPNT      ; save address low byte
  STY FORPNT+1    ; save address high byte
  LDA CHRGOT+1    ; get BASIC execute pointer low byte
  LDY CHRGOT+2    ; get BASIC execute pointer high byte
  STA OPPTR     ; save BASIC execute pointer low byte
  STY OPPTR+1     ; save BASIC execute pointer high byte
  LDX INPPTR      ; get READ pointer low byte
  LDY INPPTR+1    ; get READ pointer high byte
  STX CHRGOT+1    ; save as BASIC execute pointer low byte
  STY CHRGOT+2    ; save as BASIC execute pointer high byte
  JSR CHRGOT      ; scan memory
  BNE LAB_CC51    ; branch if not null

          ; pointer was to null entry
  BIT INPFLG      ; test input mode flag, $00 = INPUT, $40 = GET, $98 = READ
  BVC LAB_CC41    ; branch if not GET

          ; else was GET
  JSR LAB_E121    ; get character from input device with error check
  STA BUF     ; save to buffer
  LDX #<BUF-1     ; set BUF-1 pointer low byte
  LDY #>BUF-1     ; set BUF-1 pointer high byte
  BNE LAB_CC4D    ; go interpret single character

LAB_CC41
  BMI LAB_CCB8    ; if READ go get some DATA

; else it was INPUT

  LDA CHANNL      ; get current I/O channel
  BNE LAB_CC4A    ; skip "?" prompt if not default channel

  JSR LAB_CB45    ; print "?"
LAB_CC4A
  JSR LAB_CBF9    ; print "? " and get BASIC input
LAB_CC4D
  STX CHRGOT+1    ; save BASIC execute pointer low byte
  STY CHRGOT+2    ; save BASIC execute pointer high byte
LAB_CC51
  JSR CHRGET      ; increment and scan memory, execute pointer now points to
          ; start of next data or null terminator
  BIT VALTYP      ; test data type flag, $FF = string, $00 = numeric
  BPL LAB_CC89    ; branch if numeric

          ; type is string
  BIT INPFLG      ; test INPUT mode flag, $00 = INPUT, $40 = GET, $98 = READ
  BVC LAB_CC65    ; branch if not GET

          ; else do string GET
  INX       ; clear .X ??
  STX CHRGOT+1    ; save BASIC execute pointer low byte
  LDA #$00      ; clear .A
  STA CHARAC      ; clear search character
  BEQ LAB_CC71    ; branch always

          ; is string INPUT or string READ
LAB_CC65
  STA CHARAC      ; save search character
  CMP #$22      ; compare with "
  BEQ LAB_CC72    ; if quote only search for "..." string

          ; else the string is not in quotes so ":", "," or $00 are
          ; the termination characters
  LDA #':'      ; set ":"
  STA CHARAC      ; set search character
  LDA #','      ; set ","
LAB_CC71
  CLC       ; clear carry for add
LAB_CC72
  STA ENDCHR      ; set scan quotes flag
  LDA CHRGOT+1    ; get BASIC execute pointer low byte
  LDY CHRGOT+2    ; get BASIC execute pointer high byte
  ADC #$00      ; add to pointer low byte. this add increments the pointer
          ; if the mode is INPUT or READ and the data is a "..."
          ; string
  BCC LAB_CC7D    ; if no rollover skip the high byte increment

  INY       ; else increment pointer high byte
LAB_CC7D
  JSR LAB_D48D    ; print string to utility pointer
  JSR LAB_D7E2    ; restore BASIC execute pointer from temp
  JSR LET5      ; perform string LET
  JMP LAB_CC91    ; continue processing command

          ; GET, INPUT or READ is numeric
LAB_CC89
  JSR ASCFLT      ; get FAC1 from string
  LDA INTFLG      ; get data type flag, $80 = integer, $00 = float
  JSR LET2      ; assign value to numeric variable
LAB_CC91
  JSR CHRGOT      ; scan memory
  BEQ LAB_CC9D    ; if ":" or [EOL] go handle the string end

  CMP #','      ; compare with ","
  BEQ LAB_CC9D    ; if "," go handle the string end

  JMP IGRERR      ; else go do bad input routine

          ; string terminated with ":", "," or $00
LAB_CC9D
  LDA CHRGOT+1    ; get BASIC execute pointer low byte
  LDY CHRGOT+2    ; get BASIC execute pointer high byte
  STA INPPTR      ; save READ pointer low byte
  STY INPPTR+1    ; save READ pointer high byte
  LDA OPPTR     ; get saved BASIC execute pointer low byte
  LDY OPPTR+1     ; get saved BASIC execute pointer high byte
  STA CHRGOT+1    ; restore BASIC execute pointer low byte
  STY CHRGOT+2    ; restore BASIC execute pointer high byte
  JSR CHRGOT      ; scan memory
  BEQ LAB_CCDF    ; branch if ":" or [EOL]

  JSR COMCHK      ; scan for ",", else do syntax error then warm start
  JMP LAB_CC15    ; go READ or INPUT next variable from list

          ; was READ
LAB_CCB8
  JSR FIND2     ; scan for next BASIC statement ([:] or [EOL])
  INY       ; increment index to next byte
  TAX       ; copy byte to .X
  BNE LAB_CCD1    ; if ":" go look for the next DATA

  LDX #ER_OODATA    ; else set error $0D, out of data error
  INY       ; increment index to next line pointer high byte
  LDA (CHRGOT+1),Y    ; get next line pointer high byte
  BEQ LAB_CD32    ; if program end go do error, eventually does error .X

  INY       ; increment index
  LDA (CHRGOT+1),Y    ; get next line # low byte
  STA DATLIN      ; save current DATA line low byte
  INY       ; increment index
  LDA (CHRGOT+1),Y    ; get next line # high byte
  INY       ; increment index
  STA DATLIN+1    ; save current DATA line high byte
LAB_CCD1
  JSR BUMPTP      ; add .Y to the BASIC execute pointer
  JSR CHRGOT      ; scan memory
  TAX       ; copy byte
  CPX #TK_DATA    ; compare with token for DATA
  BNE LAB_CCB8    ; loop if not DATA

  JMP LAB_CC51    ; continue evaluating READ

LAB_CCDF
  LDA INPPTR      ; get READ pointer low byte
  LDY INPPTR+1    ; get READ pointer high byte
  LDX INPFLG      ; get INPUT mode flag, $00 = INPUT, $40 = GET, $98 = READ
  BPL LAB_CCEA    ; if INPUT or GET go exit or ignore extra input

  JMP LAB_C827    ; else set data pointer and exit

LAB_CCEA
  LDY #$00      ; clear index
  LDA (INPPTR),Y    ; get READ byte
  BEQ LAB_CCFB    ; exit if [EOL]

  LDA CHANNL      ; get current I/O channel
  BNE LAB_CCFB    ; exit if not default channel

  LDA #<EXTRA     ; set "?EXTRA IGNORED" pointer low byte
  LDY #>EXTRA     ; set "?EXTRA IGNORED" pointer high byte
  JMP PRTSTR      ; print null terminated string

LAB_CCFB
  RTS


;***********************************************************************************;
;
; input error messages

EXTRA
  .byte "?EXTRA IGNORED",$0D,$00

LAB_CD0C
  .byte "?REDO FROM START",$0D,$00


;***********************************************************************************;
;
; perform NEXT

NEXT
  BNE LAB_CD24    ; if NEXT variable go find the variable

  LDY #$00      ; else clear .Y
  BEQ LAB_CD27    ; use any variable, branch always

; NEXT variable

LAB_CD24
  JSR EVLVAR      ; get variable address
LAB_CD27
  STA FORPNT      ; save FOR/NEXT variable pointer low byte
  STY FORPNT+1    ; save FOR/NEXT variable pointer high byte
          ; (high byte cleared if no variable defined)
  JSR SCNSTK      ; search the stack for FOR or GOSUB activity
  BEQ LAB_CD35    ; if FOR found continue

  LDX #ER_NXTWOFOR    ; else set error $0A, next without for error
LAB_CD32
  JMP ERROR     ; do error #.X then warm start

; found this FOR variable

LAB_CD35
  TXS       ; update stack pointer
  TXA       ; copy stack pointer
  CLC       ; clear carry for add
  ADC #$04      ; point to STEP value
  PHA       ; save it
  ADC #$06      ; point to TO value
  STA INDEX+2     ; save pointer to TO variable for compare
  PLA       ; restore pointer to STEP value
  LDY #$01      ; point to stack page
  JSR LODFAC      ; unpack memory (.A.Y) into FAC1
  TSX       ; get stack pointer back
  LDA STACK+9,X   ; get step sign
  STA FAC1+FAC_SIGN   ; save FAC1 sign (b7)
  LDA FORPNT      ; get FOR/NEXT variable pointer low byte
  LDY FORPNT+1    ; get FOR/NEXT variable pointer high byte
  JSR LAPLUS      ; add FOR variable to FAC1
  JSR FACTFP      ; pack FAC1 into FOR variable
  LDY #$01      ; point to stack page
  JSR LAB_DC5D    ; compare FAC1 with TO value
  TSX       ; get stack pointer back
  SEC       ; set carry for subtract
  SBC STACK+9,X   ; subtract step sign
  BEQ LAB_CD78    ; if = loop complete, go unstack the FOR

          ; loop back and do it all again
  LDA STACK+$0F,X   ; get FOR line low byte
  STA CURLIN      ; save current line number low byte
  LDA STACK+$10,X   ; get FOR line high byte
  STA CURLIN+1    ; save current line number high byte
  LDA STACK+$12,X   ; get BASIC execute pointer low byte
  STA CHRGOT+1    ; save BASIC execute pointer low byte
  LDA STACK+$11,X   ; get BASIC execute pointer high byte
  STA CHRGOT+2    ; save BASIC execute pointer high byte
LAB_CD75
  JMP NEWSTT      ; go do interpreter inner loop

; NEXT loop complete

LAB_CD78
  TXA       ; stack copy to .A
  ADC #$11      ; add $12, $11 + carry, to dump FOR structure
  TAX       ; copy back to index
  TXS       ; copy to stack pointer
  JSR CHRGOT      ; scan memory
  CMP #','      ; compare with ","
  BNE LAB_CD75    ; if not "," go do interpreter inner loop

          ; was "," so another NEXT variable to do
  JSR CHRGET      ; increment and scan memory
  JSR LAB_CD24    ; do NEXT variable


;***********************************************************************************;
;
; evaluate expression and check type mismatch

TYPCHK
  JSR FRMEVL      ; evaluate expression

; check if source and destination are numeric

LAB_CD8D
  CLC
  .byte $24     ; makes next line BIT $38

; check if source and destination are string

LAB_CD8F
  SEC       ; destination is string

; type match check, set Cb for string, clear Cb for numeric

LAB_CD90
  BIT VALTYP      ; test data type flag, $FF = string, $00 = numeric
  BMI LAB_CD97    ; if string go check string is required

; type found is numeric, check required

  BCS LAB_CD99    ; if string is required go do type mismatch error
LAB_CD96
  RTS

; type found is string, check required

LAB_CD97
  BCS LAB_CD96    ; exit if string is required

; do type mismatch error

LAB_CD99
  LDX #ER_TYPMSMCH    ; error code $16, type mismatch error
  JMP ERROR     ; do error #.X then warm start


;***********************************************************************************;
;
; evaluate expression

FRMEVL
  LDX CHRGOT+1    ; get BASIC execute pointer low byte
  BNE LAB_CDA4    ; skip next if not zero

  DEC CHRGOT+2    ; else decrement BASIC execute pointer high byte
LAB_CDA4
  DEC CHRGOT+1    ; decrement BASIC execute pointer low byte
  LDX #$00      ; set null precedence, flag done
  .byte $24     ; makes next line BIT $48
LAB_CDA9
  PHA       ; push compare evaluation byte if branch to here
  TXA       ; copy precedence byte
  PHA       ; push precedence byte
  LDA #$01      ; 2 bytes
  JSR STKSPC      ; check room on stack for .A*2 bytes
  JSR EVAL      ; get value from line
  LDA #$00      ; clear .A
  STA OPMASK      ; clear comparison evaluation flag
LAB_CDB8
  JSR CHRGOT      ; scan memory
LAB_CDBB
  SEC       ; set carry for subtract
  SBC #TK_GT      ; subtract token for ">"
  BCC LAB_CDD7    ; if < ">" skip comparison test check

  CMP #$03      ; compare with ">" to +3
  BCS LAB_CDD7    ; if >= 3 skip comparison test check

          ; was token for ">" "=" or "<"
  CMP #$01      ; compare with token for "="
  ROL       ; *2, b0 = carry (=1 if token was "=" or "<")
  EOR #$01      ; toggle b0
  EOR OPMASK      ; XOR with comparison evaluation flag
  CMP OPMASK      ; compare with comparison evaluation flag
  BCC LAB_CE30    ; if < saved flag do syntax error then warm start

  STA OPMASK      ; save new comparison evaluation flag
  JSR CHRGET      ; increment and scan memory
  JMP LAB_CDBB    ; go do next character

LAB_CDD7
  LDX OPMASK      ; get comparison evaluation flag
  BNE LAB_CE07    ; if compare function flagged go evaluate right hand side

  BCS LAB_CE58    ; go do functions

          ; else was < TK_GT so is operator or lower
  ADC #$07      ; add # of operators (+, -, *, /, ^, AND or OR)
  BCC LAB_CE58    ; if < + operator go do the function

          ; carry was set so token was +, -, *, /, ^, AND or OR
  ADC VALTYP      ; add data type flag, $FF = string, $00 = numeric
  BNE LAB_CDE8    ; if not string or not + token skip concatenate

          ; will only be $00 if type is string and token was +
  JMP ADDSTR      ; add strings, string 1 is in the descriptor, string 2
          ; is in line, and return

LAB_CDE8
  ADC #$FF      ; -1 (corrects for carry add)
  STA INDEX     ; save it
  ASL       ; *2
  ADC INDEX     ; *3
  TAY       ; copy to index
LAB_CDF0
  PLA       ; pull previous precedence
  CMP OPTAB,Y     ; compare with precedence byte
  BCS LAB_CE5D    ; if .A >= go do the function

  JSR LAB_CD8D    ; check if source is numeric, else do type mismatch
LAB_CDF9
  PHA       ; save precedence
LAB_CDFA
  JSR LAB_CE20    ; get vector, execute function then continue evaluation
  PLA       ; restore precedence
  LDY OPPTR     ; get precedence stacked flag
  BPL LAB_CE19    ; if stacked values go check the precedence

  TAX       ; copy precedence, set flags
  BEQ LAB_CE5B    ; exit if done

  BNE LAB_CE66    ; else pop FAC2 and return, branch always

LAB_CE07
  LSR VALTYP      ; clear data type flag, $FF = string, $00 = numeric
  TXA       ; copy compare function flag
  ROL       ; <<1, shift data type flag into b0, 1 = string, 0 = num
  LDX CHRGOT+1    ; get BASIC execute pointer low byte
  BNE LAB_CE11    ; if no underflow skip the high byte decrement

  DEC CHRGOT+2    ; else decrement BASIC execute pointer high byte
LAB_CE11
  DEC CHRGOT+1    ; decrement BASIC execute pointer low byte
  LDY #LAB_C09B-OPTAB
          ; set offset to = operator precedence entry
  STA OPMASK      ; save new comparison evaluation flag
  BNE LAB_CDF0    ; branch always

LAB_CE19
  CMP OPTAB,Y     ; compare with stacked function precedence
  BCS LAB_CE66    ; if .A >=, pop FAC2 and return

  BCC LAB_CDF9    ; else go stack this one and continue, branch always


;***********************************************************************************;
;
; get vector, execute function then continue evaluation

LAB_CE20
  LDA OPTAB+2,Y   ; get function vector high byte
  PHA       ; onto stack
  LDA OPTAB+1,Y   ; get function vector low byte
  PHA       ; onto stack
          ; now push sign, round FAC1 and put on stack
  JSR LAB_CE33    ; function will return here, then the next RTS will call
          ; the function
  LDA OPMASK      ; get comparison evaluation flag
  JMP LAB_CDA9    ; continue evaluating expression

LAB_CE30
  JMP LAB_CF08    ; do syntax error then warm start

LAB_CE33
  LDA FAC1+FAC_SIGN   ; get FAC1 sign (b7)
  LDX OPTAB,Y     ; get precedence byte


;***********************************************************************************;
;
; push sign, round FAC1 and put on stack

LAB_CE38
  TAY       ; copy sign
  PLA       ; get return address low byte
  STA INDEX     ; save it
  INC INDEX     ; increment it as return - 1 is pushed
          ; note, no check is made on the high byte so if the calling
          ; routine ever assembles to a page edge then this all goes
          ; horribly wrong!
  PLA       ; get return address high byte
  STA INDEX+1     ; save it
  TYA       ; restore sign
  PHA       ; push sign


;***********************************************************************************;
;
; round FAC1 and put on stack

LAB_CE43
  JSR ROUND     ; round FAC1
  LDA FAC1+FAC_MANT+3   ; get FAC1 mantissa 4
  PHA       ; save it
  LDA FAC1+FAC_MANT+2   ; get FAC1 mantissa 3
  PHA       ; save it
  LDA FAC1+FAC_MANT+1   ; get FAC1 mantissa 2
  PHA       ; save it
  LDA FAC1+FAC_MANT   ; get FAC1 mantissa 1
  PHA       ; save it
  LDA FAC1+FAC_EXPT   ; get FAC1 exponent
  PHA       ; save it
  JMP (INDEX)     ; return, sort of


;***********************************************************************************;
;
; do functions

LAB_CE58
  LDY #$FF      ; flag function
  PLA       ; pull precedence byte
LAB_CE5B
  BEQ LAB_CE80    ; exit if done

LAB_CE5D
  CMP #$64      ; compare previous precedence with $64
  BEQ LAB_CE64    ; if was $64 (< function) skip the type check

  JSR LAB_CD8D    ; check if source is numeric, else do type mismatch
LAB_CE64
  STY OPPTR     ; save precedence stacked flag

          ; pop FAC2 and return
LAB_CE66
  PLA       ; pop byte
  LSR       ; shift out comparison evaluation lowest bit
  STA TANSGN      ; save the comparison evaluation flag
  PLA       ; pop exponent
  STA FAC2+FAC_EXPT   ; save FAC2 exponent
  PLA       ; pop mantissa 1
  STA FAC2+FAC_MANT   ; save FAC2 mantissa 1
  PLA       ; pop mantissa 2
  STA FAC2+FAC_MANT+1   ; save FAC2 mantissa 2
  PLA       ; pop mantissa 3
  STA FAC2+FAC_MANT+2   ; save FAC2 mantissa 3
  PLA       ; pop mantissa 4
  STA FAC2+FAC_MANT+3   ; save FAC2 mantissa 4
  PLA       ; pop sign
  STA FAC2+FAC_SIGN   ; save FAC2 sign (b7)
  EOR FAC1+FAC_SIGN   ; XOR FAC1 sign (b7)
  STA ARISGN      ; save sign compare (FAC1 XOR FAC2)
LAB_CE80
  LDA FAC1+FAC_EXPT   ; get FAC1 exponent
  RTS


;***********************************************************************************;
;
; get value from line

EVAL
  JMP (IEVAL)     ; get arithmetic element


;***********************************************************************************;
;
; get arithmetic element, the get arithmetic element vector is initialised to point here

FEVAL
  LDA #$00      ; clear byte
  STA VALTYP      ; clear data type flag, $FF = string, $00 = numeric
LAB_CE8A
  JSR CHRGET      ; increment and scan memory
  BCS LAB_CE92    ; if not numeric character continue

; else numeric string found (e.g. 123)

LAB_CE8F
  JMP ASCFLT      ; get FAC1 from string and return

; get value from line .. continued, wasn't a number so ...

LAB_CE92
  JSR CHRTST      ; check byte, return Cb = 0 if <"A" or >"Z"
  BCC LAB_CE9A    ; if not variable name continue

  JMP FACT12      ; variable name set-up and return

; get value from line .. continued, wasn't a variable name so ...

LAB_CE9A
  CMP #TK_PI      ; compare with token for PI
  BNE LAB_CEAD    ; if not PI continue

; else return PI in FAC1

  LDA #<PIVAL     ; get PI pointer low byte
  LDY #>PIVAL     ; get PI pointer high byte
  JSR LODFAC      ; unpack memory (.A.Y) into FAC1
  JMP CHRGET      ; increment and scan memory and return


;***********************************************************************************;
;
; PI as floating number

PIVAL
  .byte $82,$49,$0F,$DA,$A1
          ; 3.141592653


;***********************************************************************************;
;
; get value from line .. continued, wasn't PI so ...

LAB_CEAD
  CMP #'.'      ; compare with "."
  BEQ LAB_CE8F    ; if so get FAC1 from string and return, e.g. was .123

          ; wasn't .123 so ...
  CMP #TK_MINUS   ; compare with token for "-"
  BEQ FACT10      ; if "-" token, do set-up for functions

          ; wasn't -123 so ...
  CMP #TK_PLUS    ; compare with token for "+"
  BEQ LAB_CE8A    ; if "+" token ignore the leading +, +1 = 1

          ; it wasn't any sort of number so ...
  CMP #$22      ; compare with "
  BNE LAB_CECC    ; if not open quote continue

          ; was open quote so get the enclosed string

; print "..." string to string utility area

LAB_CEBD
  LDA CHRGOT+1    ; get BASIC execute pointer low byte
  LDY CHRGOT+2    ; get BASIC execute pointer high byte
  ADC #$00      ; add carry to low byte
  BCC LAB_CEC6    ; branch if no overflow

  INY       ; increment high byte
LAB_CEC6
  JSR MAKSTR      ; print " terminated string to utility pointer
  JMP LAB_D7E2    ; restore BASIC execute pointer from temp and return

; get value from line .. continued, wasn't a string so ...

LAB_CECC
  CMP #TK_NOT     ; compare with token for NOT
  BNE LAB_CEE3    ; if not token for NOT continue

; was NOT token

  LDY #$18      ; offset to NOT function
  BNE LAB_CF0F    ; do set-up for function then execute, branch always

; do = compare

EQUAL
  JSR MAKINT      ; evaluate integer expression, no sign check
  LDA FAC1+FAC_MANT+3   ; get FAC1 mantissa 4
  EOR #$FF      ; invert it
  TAY       ; copy it
  LDA FAC1+FAC_MANT+2   ; get FAC1 mantissa 3
  EOR #$FF      ; invert it
  JMP MAKFP     ; convert fixed integer .A.Y to float FAC1 and return

; get value from line .. continued, wasn't NOT so ...

LAB_CEE3
  CMP #TK_FN      ; compare with token for FN
  BNE LAB_CEEA    ; if not token for FN continue

  JMP EVALFN      ; else go evaluate FNx

; get value from line .. continued, wasn't FN so ...

LAB_CEEA
  CMP #TK_SGN     ; compare with token for SGN
  BCC PAREXP      ; if less than SGN token go evaluate expression in ()

          ; else was a function token
  JMP FACT17      ; go set up function references, branch always


;***********************************************************************************;
;
; get value from line .. continued
; if here it can only be something in brackets so ...

; evaluate expression within parentheses

PAREXP
  JSR LPACHK      ; scan for "(", else do syntax error then warm start
  JSR FRMEVL      ; evaluate expression


;***********************************************************************************;
;
; all the 'scan for' routines return the character after the sought character

; scan for ")", else do syntax error then warm start

RPACHK
  LDA #$29      ; load .A with ")"
  .byte $2C     ; makes next line BIT $28A9

; scan for "(", else do syntax error then warm start

LPACHK
  LDA #$28      ; load .A with "("
  .byte $2C     ; makes next line BIT $2CA9

; scan for ",", else do syntax error then warm start

COMCHK
  LDA #','      ; load .A with ","

; scan for CHR$(.A), else do syntax error then warm start

SYNCHR
  LDY #$00      ; clear index
  CMP (CHRGOT+1),Y    ; compare with BASIC byte
  BNE LAB_CF08    ; if not expected byte do syntax error then warm start

  JMP CHRGET      ; else increment and scan memory and return


;***********************************************************************************;
;
; syntax error then warm start

LAB_CF08
  LDX #ER_SYNTAX    ; error code $0B, syntax error
  JMP ERROR     ; do error #.X then warm start

FACT10
  LDY #$15      ; set offset from base to > operator
LAB_CF0F
  PLA       ; dump return address low byte
  PLA       ; dump return address high byte
  JMP LAB_CDFA    ; execute function then continue evaluation


;***********************************************************************************;
;
; check address range, return Cb = 1 if address in BASIC ROM

VARRANGE
  SEC       ; set carry for subtract
  LDA FAC1+3      ; get variable address low byte
  SBC #<COLDST    ; subtract $C000 low byte
  LDA FAC1+4      ; get variable address high byte
  SBC #>COLDST    ; subtract $C000 high byte
  BCC LAB_CF27    ; exit if address < $C000

  LDA #<CGIMAG    ; get end of BASIC marker low byte
  SBC FAC1+3      ; subtract variable address low byte
  LDA #>CGIMAG    ; get end of BASIC marker high byte
  SBC FAC1+4      ; subtract variable address high byte
LAB_CF27
  RTS


;***********************************************************************************;
;
; variable name set-up

FACT12
  JSR EVLVAR      ; get variable address
  STA FAC1+3      ; save variable pointer low byte
  STY FAC1+4      ; save variable pointer high byte
  LDX VARNAM      ; get current variable name first character
  LDY VARNAM+1    ; get current variable name second character
  LDA VALTYP      ; get data type flag, $FF = string, $00 = numeric
  BEQ LAB_CF5D    ; if numeric go handle a numeric variable

; variable is string

  LDA #$00      ; else clear .A
  STA FACOV     ; clear FAC1 rounding byte
  JSR VARRANGE    ; check address range
  BCC LAB_CF5C    ; exit if not in BASIC ROM

  CPX #'T'      ; compare variable name first character with "T"
  BNE LAB_CF5C    ; exit if not "T"

  CPY #'I'+$80    ; compare variable name second character with "I$"
  BNE LAB_CF5C    ; exit if not "I$"

          ; variable name was "TI$"
  JSR LAB_CF84    ; read real time clock into FAC1 mantissa, 0HML
  STY EXPCNT      ; clear exponent count adjust
  DEY       ; .Y = $FF
  STY FBUFPT      ; set output string index, -1 to allow for pre increment
  LDY #$06      ; HH:MM:SS is six digits
  STY LAB_5D      ; set number of characters before the decimal point
  LDY #HMSCON-FLTCON
          ; index to jiffy conversion table
  JSR LAB_DE68    ; convert jiffy count to string
  JMP LAB_D46F    ; exit via STR$() code tail

LAB_CF5C
  RTS

; variable name set-up, variable is numeric

LAB_CF5D
  BIT INTFLG      ; test data type flag, $80 = integer, $00 = float
  BPL LAB_CF6E    ; if float go handle float

; else handle integer variable

  LDY #$00      ; clear index
  LDA (FAC1+3),Y    ; get integer variable low byte
  TAX       ; copy to .X
  INY       ; increment index
  LDA (FAC1+3),Y    ; get integer variable high byte
  TAY       ; copy to .Y
  TXA       ; copy low byte to .A
  JMP MAKFP     ; convert fixed integer .A.Y to float FAC1 and return

; variable name set-up, variable is float

LAB_CF6E
  JSR VARRANGE    ; check address range
  BCC LAB_CFA0    ; if not in BASIC ROM get pointer and unpack into FAC1

  CPX #'T'      ; compare variable name first character with "T"
  BNE LAB_CF92    ; if not "T" skip Tx variables

  CPY #'I'      ; compare variable name second character with "I"
  BNE LAB_CFA0    ; if not "I" go do plain float

          ; variable name was "TI"
  JSR LAB_CF84    ; read real time clock into FAC1 mantissa, 0HML
  TYA       ; clear .A
  LDX #$A0      ; set exponent to 32 bit value
  JMP LAB_DC4F    ; set exponent = .X and normalise FAC1


;***********************************************************************************;
;
; read real time clock into FAC1 mantissa, 0HML

LAB_CF84
  JSR RDTIM     ; read real time clock
  STX FAC1+FAC_MANT+2   ; save jiffy clock mid byte as FAC1 mantissa 3
  STY FAC1+FAC_MANT+1   ; save jiffy clock high byte as FAC1 mantissa 2
  STA FAC1+FAC_MANT+3   ; save jiffy clock low byte as FAC1 mantissa 4
  LDY #$00      ; clear .Y
  STY FAC1+FAC_MANT   ; clear FAC1 mantissa 1
  RTS


;***********************************************************************************;
;
; variable name set-up, variable is float and not "Tx"

LAB_CF92
  CPX #'S'      ; compare variable name first character with "S"
  BNE LAB_CFA0    ; if not "S" go do normal floating variable

  CPY #'T'      ; compare variable name second character with "T"
  BNE LAB_CFA0    ; if not "T" go do normal floating variable

          ; variable name was "ST"
  JSR READST      ; read I/O status word
  JMP INTFP     ; save .A as integer byte and return

; variable is plain float

LAB_CFA0
  LDA FAC1+3      ; get variable pointer low byte
  LDY FAC1+4      ; get variable pointer high byte
  JMP LODFAC      ; unpack memory (.A.Y) into FAC1


;***********************************************************************************;
;
; get value from line continued
; only functions left so ..

; set up function references

FACT17
  ASL       ; *2 (2 bytes per function address)
  PHA       ; save function offset
  TAX       ; copy function offset
  JSR CHRGET      ; increment and scan memory
  CPX #$8F      ; compare function offset to CHR$ token offset+1
  BCC LAB_CFD1    ; if < LEFT$ (cannot be =) go do function setup

; get value from line .. continued
; was LEFT$, RIGHT$ or MID$ so..

  JSR LPACHK      ; scan for "(", else do syntax error then warm start
  JSR FRMEVL      ; evaluate, should be string, expression
  JSR COMCHK      ; scan for ",", else do syntax error then warm start
  JSR LAB_CD8F    ; check if source is string, else do type mismatch
  PLA       ; restore function offset
  TAX       ; copy it
  LDA FAC1+4      ; get descriptor pointer high byte
  PHA       ; push string pointer high byte
  LDA FAC1+3      ; get descriptor pointer low byte
  PHA       ; push string pointer low byte
  TXA       ; restore function offset
  PHA       ; save function offset
  JSR LAB_D79E    ; get byte parameter
  PLA       ; restore function offset
  TAY       ; copy function offset
  TXA       ; copy byte parameter to .A
  PHA       ; push byte parameter
  JMP LAB_CFD6    ; go call function

; get value from line .. continued
; was SGN() to CHR$() so..

LAB_CFD1
  JSR PAREXP      ; evaluate expression within parentheses
  PLA       ; restore function offset
  TAY       ; copy to index
LAB_CFD6
  LDA FUNDSP-$68,Y    ; get function jump vector low byte
  STA JMPER+1     ; save functions jump vector low byte
  LDA FUNDSP-$67,Y    ; get function jump vector high byte
  STA JMPER+2     ; save functions jump vector high byte
  JSR JMPER     ; do function call
  JMP LAB_CD8D    ; check if source is numeric and RTS, else do type mismatch
          ; string functions avoid this by dumping the return address


;***********************************************************************************;
;
; perform OR
; this works because NOT(NOT(x) AND NOT(y)) = x OR y

ORR
  LDY #$FF      ; set .Y for OR
  .byte $2C     ; makes next line BIT $00A0


;***********************************************************************************;
;
; perform AND

ANDD
  LDY #$00      ; clear .Y for AND
  STY COUNT     ; set AND/OR invert value
  JSR MAKINT      ; evaluate integer expression, no sign check
  LDA FAC1+FAC_MANT+2   ; get FAC1 mantissa 3
  EOR COUNT     ; XOR low byte
  STA CHARAC      ; save it
  LDA FAC1+FAC_MANT+3   ; get FAC1 mantissa 4
  EOR COUNT     ; XOR high byte
  STA ENDCHR      ; save it
  JSR ATOF      ; copy FAC2 to FAC1, get 2nd value in expression
  JSR MAKINT      ; evaluate integer expression, no sign check
  LDA FAC1+FAC_MANT+3   ; get FAC1 mantissa 4
  EOR COUNT     ; XOR high byte
  AND ENDCHR      ; AND with expression 1 high byte
  EOR COUNT     ; XOR result high byte
  TAY       ; save in .Y
  LDA FAC1+FAC_MANT+2   ; get FAC1 mantissa 3
  EOR COUNT     ; XOR low byte
  AND CHARAC      ; AND with expression 1 low byte
  EOR COUNT     ; XOR result low byte
  JMP MAKFP     ; convert fixed integer .A.Y to float FAC1 and return


;***********************************************************************************;
;
; perform comparisons

; do < compare

COMPAR
  JSR LAB_CD90    ; type match check, set Cb for string
  BCS CMPST     ; if string go do string compare

          ; do numeric < compare
  LDA FAC2+FAC_SIGN   ; get FAC2 sign (b7)
  ORA #$7F      ; set all non sign bits
  AND FAC2+FAC_MANT   ; and FAC2 mantissa 1 (AND in sign bit)
  STA FAC2+FAC_MANT   ; save FAC2 mantissa 1
  LDA #<FAC2      ; set pointer low byte to FAC2
  LDY #>FAC2      ; set pointer high byte to FAC2
  JSR CMPFAC      ; compare FAC1 with (.A.Y)
  TAX       ; copy the result
  JMP LAB_D061    ; go evaluate result

; do string < compare

CMPST
  LDA #$00      ; clear byte
  STA VALTYP      ; clear data type flag, $FF = string, $00 = numeric
  DEC OPMASK      ; clear < bit in comparison evaluation flag
  JSR LAB_D6A6    ; pop string off descriptor stack, or from top of string
          ; space returns with .A = length, .X = pointer low byte,
          ; .Y = pointer high byte
  STA FAC1      ; save length
  STX FAC1+1      ; save string pointer low byte
  STY FAC1+2      ; save string pointer high byte
  LDA FAC2+3      ; get descriptor pointer low byte
  LDY FAC2+4      ; get descriptor pointer high byte
  JSR LAB_D6AA    ; pop (.Y.A) descriptor off stack or from top of string space
          ; returns with .A = length, .X = pointer low byte,
          ; .Y = pointer high byte
  STX FAC2+3      ; save string pointer low byte
  STY FAC2+4      ; save string pointer high byte
  TAX       ; copy length
  SEC       ; set carry for subtract
  SBC FAC1      ; subtract string 1 length
  BEQ LAB_D056    ; if str 1 length = string 2 length go compare the strings

  LDA #$01      ; set str 1 length > string 2 length
  BCC LAB_D056    ; if so return +1 if otherwise equal

  LDX FAC1      ; get string 1 length
  LDA #$FF      ; set str 1 length < string 2 length
LAB_D056
  STA FAC1+5      ; save length compare
  LDY #$FF      ; set index
  INX       ; adjust for loop
LAB_D05B
  INY       ; increment index
  DEX       ; decrement count
  BNE LAB_D066    ; if still bytes to do go compare them

  LDX FAC1+5      ; get length compare back
LAB_D061
  BMI LAB_D072    ; branch if str 1 < str 2

  CLC       ; flag str 1 <= str 2
  BCC LAB_D072    ; go evaluate result, branch always

LAB_D066
  LDA (FAC2+3),Y    ; get string 2 byte
  CMP (FAC1+1),Y    ; compare with string 1 byte
  BEQ LAB_D05B    ; loop if bytes =

  LDX #$FF      ; set str 1 < string 2
  BCS LAB_D072    ; branch if so

  LDX #$01      ; set str 1 > string 2
LAB_D072
  INX       ; x = 0, 1 or 2
  TXA       ; copy to .A
  ROL       ; * 2 (1, 2 or 4)
  AND TANSGN      ; AND with the comparison evaluation flag
  BEQ LAB_D07B    ; branch if 0 (compare is false)

  LDA #$FF      ; else set result true
LAB_D07B
  JMP INTFP     ; save .A as integer byte and return

LAB_D07E
  JSR COMCHK      ; scan for ",", else do syntax error then warm start


;***********************************************************************************;
;
; perform DIM

DIM
  TAX       ; copy "DIM" flag to .X
  JSR LAB_D090    ; search for variable
  JSR CHRGOT      ; scan memory
  BNE LAB_D07E    ; scan for "," and loop if not null

  RTS


;***********************************************************************************;
;
; search for variable

EVLVAR
  LDX #$00      ; set DIM flag = $00
  JSR CHRGOT      ; scan memory, 1st character
LAB_D090
  STX DIMFLG      ; save DIM flag
LAB_D092
  STA VARNAM      ; save 1st character
  JSR CHRGOT      ; scan memory
  JSR CHRTST      ; check byte, return Cb = 0 if <"A" or >"Z"
  BCS LAB_D09F    ; if ok continue

LAB_D09C
  JMP LAB_CF08    ; else syntax error then warm start

; was variable name so ...

LAB_D09F
  LDX #$00      ; clear 2nd character temp
  STX VALTYP      ; clear data type flag, $FF = string, $00 = numeric
  STX INTFLG      ; clear data type flag, $80 = integer, $00 = float
  JSR CHRGET      ; increment and scan memory, 2nd character
  BCC LAB_D0AF    ; if character = "0"-"9" (ok) go save 2nd character

          ; 2nd character wasn't "0" to "9" so ...
  JSR CHRTST      ; check byte, return Cb = 0 if <"A" or >"Z"
  BCC LAB_D0BA    ; if <"A" or >"Z" go check if string

LAB_D0AF
  TAX       ; copy 2nd character

          ; ignore further (valid) characters in the variable name
LAB_D0B0
  JSR CHRGET      ; increment and scan memory, 3rd character
  BCC LAB_D0B0    ; loop if character = "0"-"9" (ignore)

  JSR CHRTST      ; check byte, return Cb = 0 if <"A" or >"Z"
  BCS LAB_D0B0    ; loop if character = "A"-"Z" (ignore)

          ; check if string variable
LAB_D0BA
  CMP #'$'      ; compare with "$"
  BNE LAB_D0C4    ; if not string go check integer

          ; type is string
  LDA #$FF      ; set data type = string
  STA VALTYP      ; set data type flag, $FF = string, $00 = numeric
  BNE LAB_D0D4    ; branch always

LAB_D0C4
  CMP #$25      ; compare with "%"
  BNE LAB_D0DB    ; if not integer go check for an array

  LDA SUBFLG      ; get subscript/FNx flag
  BNE LAB_D09C    ; if ?? do syntax error then warm start

  LDA #$80      ; set integer type
  STA INTFLG      ; set data type = integer
  ORA VARNAM      ; OR current variable name first byte
  STA VARNAM      ; save current variable name first byte
LAB_D0D4
  TXA       ; get 2nd character back
  ORA #$80      ; set top bit, indicate string or integer variable
  TAX       ; copy back to 2nd character temp
  JSR CHRGET      ; increment and scan memory
LAB_D0DB
  STX VARNAM+1    ; save 2nd character
  SEC       ; set carry for subtract
  ORA SUBFLG      ; or with subscript/FNx flag - or FN name
  SBC #$28      ; subtract "("
  BNE FNDVAR      ; if not "(" go find a plain numeric variable

  JMP ARY     ; else go find, or make, array

; either find or create variable

          ; variable name wasn't xx(... so look for plain variable
FNDVAR
  LDY #$00      ; clear .Y
  STY SUBFLG      ; clear subscript/FNx flag
  LDA VARTAB      ; get start of variables low byte
  LDX VARTAB+1    ; get start of variables high byte
LAB_D0EF
  STX TMPPTR+1    ; save search address high byte
LAB_D0F1
  STA TMPPTR      ; save search address low byte
  CPX ARYTAB+1    ; compare with end of variables high byte
  BNE LAB_D0FB    ; skip next compare if <>

          ; high addresses were = so compare low addresses
  CMP ARYTAB      ; compare low address with end of variables low byte
  BEQ MAKVAR      ; if not found go make new variable

LAB_D0FB
  LDA VARNAM      ; get 1st character of variable to find
  CMP (TMPPTR),Y    ; compare with variable name 1st character
  BNE LAB_D109    ; if no match go try the next variable

          ; 1st characters match so compare 2nd character
  LDA VARNAM+1    ; get 2nd character of variable to find
  INY       ; index to point to variable name 2nd character
  CMP (TMPPTR),Y    ; compare with variable name 2nd character
  BEQ RETVP     ; if match go return the variable

  DEY       ; else decrement index (now = $00)
LAB_D109
  CLC       ; clear carry for add
  LDA TMPPTR      ; get search address low byte
  ADC #$07      ; +7, offset to next variable name
  BCC LAB_D0F1    ; loop if no overflow to high byte

  INX       ; else increment high byte
  BNE LAB_D0EF    ; loop always, RAM doesn't extend to $FFFF


;***********************************************************************************;
;
; check byte, return Cb = 0 if<"A" or >"Z"

CHRTST
  CMP #$41      ; compare with "A"
  BCC LAB_D11C    ; exit if less

          ; carry is set
  SBC #$5B      ; subtract "Z"+1
  SEC       ; set carry
  SBC #$A5      ; subtract $A5 (restore byte)
          ; carry clear if byte > $5A
LAB_D11C
  RTS


;***********************************************************************************;
;
          ; reached end of variable memory without match
          ; ... so create new variable
MAKVAR
  PLA       ; pop return address low byte
  PHA       ; push return address low byte
  CMP #<FACT12+2    ; compare with expected calling routine return low byte
  BNE LAB_D128    ; if not get variable go create new variable

; This will only drop through if the call was from FACT12 and is only called
; from there if it is searching for a variable from the right hand side of a LET a=b
; statement, it prevents the creation of variables not assigned a value.

; Value returned by this is either numeric zero, exponent byte is $00, or null string,
; descriptor length byte is $00. in fact a pointer to any $00 byte would have done.

          ; else return dummy null value
LAB_D123
  LDA #<NULLVAR   ; set result pointer low byte
  LDY #>NULLVAR   ; set result pointer high byte
  RTS

          ; create new numeric variable
LAB_D128
  LDA VARNAM      ; get variable name first character
  LDY VARNAM+1    ; get variable name second character
  CMP #'T'      ; compare first character with "T"
  BNE LAB_D13B    ; if not "T" continue

  CPY #'I'+$80    ; compare second character with "I$"
  BEQ LAB_D123    ; if "I$" return null value

  CPY #'I'      ; compare second character with "I"
  BNE LAB_D13B    ; if not "I" continue

          ; if name is "TI" do syntax error
LAB_D138
  JMP LAB_CF08    ; do syntax error then warm start

LAB_D13B
  CMP #'S'      ; compare first character with "S"
  BNE LAB_D143    ; if not "S" continue

  CPY #'T'      ; compare second character with "T"
  BEQ LAB_D138    ; if name is "ST" do syntax error

LAB_D143
  LDA ARYTAB      ; get end of variables low byte
  LDY ARYTAB+1    ; get end of variables high byte
  STA TMPPTR      ; save old block start low byte
  STY TMPPTR+1    ; save old block start high byte
  LDA STREND      ; get end of arrays low byte
  LDY STREND+1    ; get end of arrays high byte
  STA GEN2PTR     ; save old block end low byte
  STY GEN2PTR+1   ; save old block end high byte
  CLC       ; clear carry for add
  ADC #$07      ; +7, space for one variable
  BCC LAB_D159    ; if no overflow skip the high byte increment

  INY       ; else increment high byte
LAB_D159
  STA GENPTR      ; set new block end low byte
  STY GENPTR+1    ; set new block end high byte
  JSR MAKSPC      ; open up space in memory
  LDA GENPTR      ; get new start low byte
  LDY GENPTR+1    ; get new start high byte (-$100)
  INY       ; correct high byte
  STA ARYTAB      ; set end of variables low byte
  STY ARYTAB+1    ; set end of variables high byte
  LDY #$00      ; clear index
  LDA VARNAM      ; get variable name 1st character
  STA (TMPPTR),Y    ; save variable name 1st character
  INY       ; increment index
  LDA VARNAM+1    ; get variable name 2nd character
  STA (TMPPTR),Y    ; save variable name 2nd character
  LDA #$00      ; clear .A
  INY       ; increment index
  STA (TMPPTR),Y    ; initialise variable byte
  INY       ; increment index
  STA (TMPPTR),Y    ; initialise variable byte
  INY       ; increment index
  STA (TMPPTR),Y    ; initialise variable byte
  INY       ; increment index
  STA (TMPPTR),Y    ; initialise variable byte
  INY       ; increment index
  STA (TMPPTR),Y    ; initialise variable byte

          ; found a match for variable
RETVP
  LDA TMPPTR      ; get variable address low byte
  CLC       ; clear carry for add
  ADC #$02      ; +2, offset past variable name bytes
  LDY TMPPTR+1    ; get variable address high byte
  BCC LAB_D18F    ; if no overflow skip the high byte increment

  INY       ; else increment high byte
LAB_D18F
  STA VARPNT      ; save current variable pointer low byte
  STY VARPNT+1    ; save current variable pointer high byte
  RTS


;***********************************************************************************;
;
; set-up array pointer to first element in array

ARYHED
  LDA COUNT     ; get # of dimensions (1, 2 or 3)
  ASL       ; *2 (also clears the carry !)
  ADC #$05      ; +5 (result is 7, 9 or 11 here)
  ADC TMPPTR      ; add array start pointer low byte
  LDY TMPPTR+1    ; get array pointer high byte
  BCC LAB_D1A0    ; if no overflow skip the high byte increment

  INY       ; else increment high byte
LAB_D1A0
  STA GENPTR      ; save array data pointer low byte
  STY GENPTR+1    ; save array data pointer high byte
  RTS


;***********************************************************************************;
;
; -32768 as floating value

MAXINT
  .byte $90,$80,$00,$00,$00 ; -32768


;***********************************************************************************;
;
; convert float to fixed

INTIDX
  JSR MAKINT      ; evaluate integer expression, no sign check
  LDA FAC1+3      ; get result low byte
  LDY FAC1+4      ; get result high byte
  RTS


;***********************************************************************************;
;
; evaluate integer expression

GETSUB
  JSR CHRGET      ; increment and scan memory
  JSR FRMEVL      ; evaluate expression

; evaluate integer expression, sign check

LAB_D1B8
  JSR LAB_CD8D    ; check if source is numeric, else do type mismatch
  LDA FAC1+FAC_SIGN   ; get FAC1 sign (b7)
  BMI LAB_D1CC    ; do illegal quantity error if -ve

; evaluate integer expression, no sign check

MAKINT
  LDA FAC1+FAC_EXPT   ; get FAC1 exponent
  CMP #$90      ; compare with exponent = 2^16 (n>2^15)
  BCC LAB_D1CE    ; if n<2^16 go convert FAC1 floating to fixed and return

  LDA #<MAXINT    ; set pointer low byte to -32768
  LDY #>MAXINT    ; set pointer high byte to -32768
  JSR CMPFAC      ; compare FAC1 with (.A.Y)
LAB_D1CC
  BNE ILQUAN      ; if <> do illegal quantity error then warm start

LAB_D1CE
  JMP FPINT     ; convert FAC1 floating to fixed and return


;***********************************************************************************;
;
; an array is stored as follows
;
; array name        ; two bytes with the following patterns for different types
;         ; 1st char  2nd char
;         ;   b7      b7    type      element size
;         ; --------  --------  -----     ------------
;         ;   0     0   floating point    5
;         ;   0     1   string      3
;         ;   1     1   integer     2
; offset to next array      ; word
; dimension count     ; byte
; 1st dimension size      ; word, this is the number of elements including 0
; 2nd dimension size      ; word, only here if the array has a second dimension
; 2nd dimension size      ; word, only here if the array has a third dimension
;         ; note: the dimension size word is in high byte low byte
;         ; format, unlike most 6502 words
; then for each element the required number of bytes given as the element size above

; find or make array

ARY
  LDA DIMFLG      ; get DIM flag
  ORA INTFLG      ; OR with data type flag
  PHA       ; push it
  LDA VALTYP      ; get data type flag, $FF = string, $00 = numeric
  PHA       ; push it
  LDY #$00      ; clear dimensions count

; now get the array dimension(s) and stack it (them) before the data type and DIM flag

LAB_D1DB
  TYA       ; copy dimensions count
  PHA       ; save it
  LDA VARNAM+1    ; get array name 2nd byte
  PHA       ; save it
  LDA VARNAM      ; get array name 1st byte
  PHA       ; save it
  JSR GETSUB      ; evaluate integer expression
  PLA       ; pull array name 1st byte
  STA VARNAM      ; restore array name 1st byte
  PLA       ; pull array name 2nd byte
  STA VARNAM+1    ; restore array name 2nd byte
  PLA       ; pull dimensions count
  TAY       ; restore it
  TSX       ; copy stack pointer
  LDA STACK+2,X   ; get DIM flag
  PHA       ; push it
  LDA STACK+1,X   ; get data type flag
  PHA       ; push it
  LDA FAC1+3      ; get this dimension size high byte
  STA STACK+2,X   ; stack before flag bytes
  LDA FAC1+4      ; get this dimension size low byte
  STA STACK+1,X   ; stack before flag bytes
  INY       ; increment dimensions count
  JSR CHRGOT      ; scan memory
  CMP #','      ; compare with ","
  BEQ LAB_D1DB    ; if found go do next dimension

  STY COUNT     ; store dimensions count
  JSR RPACHK      ; scan for ")", else do syntax error then warm start
  PLA       ; pull data type flag
  STA VALTYP      ; restore data type flag, $FF = string, $00 = numeric
  PLA       ; pull data type flag
  STA INTFLG      ; restore data type flag, $80 = integer, $00 = float
  AND #$7F      ; mask dim flag
  STA DIMFLG      ; restore DIM flag
  LDX ARYTAB      ; set end of variables low byte
          ; (array memory start low byte)
  LDA ARYTAB+1    ; set end of variables high byte
          ; (array memory start high byte)

; now check to see if we are at the end of array memory, we would be if there were
; no arrays.

LAB_D21C
  STX TMPPTR      ; save as array start pointer low byte
  STA TMPPTR+1    ; save as array start pointer high byte
  CMP STREND+1    ; compare with end of arrays high byte
  BNE LAB_D228    ; if not reached array memory end continue searching

  CPX STREND      ; else compare with end of arrays low byte
  BEQ ARY6      ; go build array if not found

          ; search for array
LAB_D228
  LDY #$00      ; clear index
  LDA (TMPPTR),Y    ; get array name first byte
  INY       ; increment index to second name byte
  CMP VARNAM      ; compare with this array name first byte
  BNE LAB_D237    ; if no match go try the next array

  LDA VARNAM+1    ; else get this array name second byte
  CMP (TMPPTR),Y    ; compare with array name second byte
  BEQ ARY2      ; array found so branch

          ; no match
LAB_D237
  INY       ; increment index
  LDA (TMPPTR),Y    ; get array size low byte
  CLC       ; clear carry for add
  ADC TMPPTR      ; add array start pointer low byte
  TAX       ; copy low byte to .X
  INY       ; increment index
  LDA (TMPPTR),Y    ; get array size high byte
  ADC TMPPTR+1    ; add array memory pointer high byte
  BCC LAB_D21C    ; if no overflow go check next array

; do bad subscript error

BADSUB
  LDX #ER_BADSSCPT    ; error $12, bad subscript error
  .byte $2C     ; makes next line BIT $0EA2


;***********************************************************************************;
;
; do illegal quantity error

ILQUAN
  LDX #ER_ILLQUAN   ; error $0E, illegal quantity error
LAB_D24A
  JMP ERROR     ; do error #.X then warm start


;***********************************************************************************;
;
; array found

ARY2
  LDX #ER_REDIMARY    ; set error $13, double dimension error
  LDA DIMFLG      ; get DIM flag
  BNE LAB_D24A    ; if we are trying to dimension it do error #.X then warm
          ; start

; found the array and we're not dimensioning it so we must find an element in it

  JSR ARYHED      ; set-up array pointer to first element in array
  LDA COUNT     ; get dimensions count
  LDY #$04      ; set index to array's # of dimensions
  CMP (TMPPTR),Y    ; compare with no of dimensions
  BNE BADSUB      ; if wrong do bad subscript error

  JMP ARY14     ; found array so go get element

          ; array not found, so build it
ARY6
  JSR ARYHED      ; set-up array pointer to first element in array
  JSR RAMSPC      ; check available memory, do out of memory error if no room
  LDY #$00      ; clear .Y
  STY FBUFPT+1    ; clear array data size high byte
  LDX #$05      ; set default element size
  LDA VARNAM      ; get variable name 1st byte
  STA (TMPPTR),Y    ; save array name 1st byte
  BPL LAB_D274    ; branch if not string or floating point array

  DEX       ; decrement element size, $04
LAB_D274
  INY       ; increment index
  LDA VARNAM+1    ; get variable name 2nd byte
  STA (TMPPTR),Y    ; save array name 2nd byte
  BPL LAB_D27D    ; branch if not integer or string

  DEX       ; decrement element size, $03
  DEX       ; decrement element size, $02
LAB_D27D
  STX FBUFPT      ; save element size
  LDA COUNT     ; get dimensions count
  INY       ; increment index ..
  INY       ; .. to array  ..
  INY       ; .. dimension count
  STA (TMPPTR),Y    ; save array dimension count
LAB_D286
  LDX #$0B      ; set default dimension size low byte
  LDA #$00      ; set default dimension size high byte
  BIT DIMFLG      ; test DIM flag
  BVC LAB_D296    ; if default to be used don't pull a dimension

  PLA       ; pull dimension size low byte
  CLC       ; clear carry for add
  ADC #$01      ; add 1, allow for zeroth element
  TAX       ; copy low byte to .X
  PLA       ; pull dimension size high byte
  ADC #$00      ; add carry to high byte
LAB_D296
  INY       ; increment index to dimension size high byte
  STA (TMPPTR),Y    ; save dimension size high byte
  INY       ; increment index to dimension size low byte
  TXA       ; copy dimension size low byte
  STA (TMPPTR),Y    ; save dimension size low byte
  JSR M16     ; compute array size
  STX FBUFPT      ; save result low byte
  STA FBUFPT+1    ; save result high byte
  LDY INDEX     ; restore index
  DEC COUNT     ; decrement dimensions count
  BNE LAB_D286    ; loop if not all done

  ADC GENPTR+1    ; add array data pointer high byte
  BCS LAB_D30B    ; if overflow do out of memory error then warm start

  STA GENPTR+1    ; save array data pointer high byte
  TAY       ; copy array data pointer high byte
  TXA       ; copy array size low byte
  ADC GENPTR      ; add array data pointer low byte
  BCC LAB_D2B9    ; if no rollover skip the high byte increment

  INY       ; else increment next array pointer high byte
  BEQ LAB_D30B    ; if rolled over do out of memory error then warm start

LAB_D2B9
  JSR RAMSPC      ; check available memory, do out of memory error if no room
  STA STREND      ; set end of arrays low byte
  STY STREND+1    ; set end of arrays high byte

; now the array is created we need to zero all the elements in it

  LDA #$00      ; clear .A for array clear
  INC FBUFPT+1    ; increment array size high byte, now block count
  LDY FBUFPT      ; get array size low byte, now index to block
  BEQ LAB_D2CD    ; if $00 go do the high byte decrement
LAB_D2C8
  DEY       ; decrement index, do 0 to n - 1
  STA (GENPTR),Y    ; clear array element byte
  BNE LAB_D2C8    ; loop until this block done

LAB_D2CD
  DEC GENPTR+1    ; decrement array pointer high byte
  DEC FBUFPT+1    ; decrement block count high byte
  BNE LAB_D2C8    ; loop until all blocks done

  INC GENPTR+1    ; correct for last loop
  SEC       ; set carry for subtract
  LDA STREND      ; get end of arrays low byte
  SBC TMPPTR      ; subtract array start low byte
  LDY #$02      ; index to array size low byte
  STA (TMPPTR),Y    ; save array size low byte
  LDA STREND+1    ; get end of arrays high byte
  INY       ; index to array size high byte
  SBC TMPPTR+1    ; subtract array start high byte
  STA (TMPPTR),Y    ; save array size high byte
  LDA DIMFLG      ; get default DIM flag
  BNE LAB_D34B    ; exit if this was a DIM command

          ; else, find element
  INY       ; set index to # of dimensions, the dimension indices
          ; are on the stack and will be removed as the position
          ; of the array element is calculated

ARY14
  LDA (TMPPTR),Y    ; get array's dimension count
  STA COUNT     ; save it
  LDA #$00      ; clear byte
  STA FBUFPT      ; clear array data pointer low byte
LAB_D2F2
  STA FBUFPT+1    ; save array data pointer high byte
  INY       ; increment index, point to array bound high byte
  PLA       ; pull array index low byte
  TAX       ; copy to .X
  STA FAC1+FAC_MANT+2   ; save index low byte to FAC1 mantissa 3
  PLA       ; pull array index high byte
  STA FAC1+FAC_MANT+3   ; save index high byte to FAC1 mantissa 4
  CMP (TMPPTR),Y    ; compare with array bound high byte
  BCC LAB_D30E    ; if within bounds continue

  BNE LAB_D308    ; if outside bounds do bad subscript error

          ; else high byte was = so test low bytes
  INY       ; index to array bound low byte
  TXA       ; get array index low byte
  CMP (TMPPTR),Y    ; compare with array bound low byte
  BCC LAB_D30F    ; if within bounds continue

LAB_D308
  JMP BADSUB      ; do bad subscript error

LAB_D30B
  JMP MEMERR      ; do out of memory error then warm start

LAB_D30E
  INY       ; index to array bound low byte
LAB_D30F
  LDA FBUFPT+1    ; get array data pointer high byte
  ORA FBUFPT      ; OR with array data pointer low byte
  CLC       ; clear carry for either add, carry always clear here ??
  BEQ LAB_D320    ; if array data pointer = null skip the multiply

  JSR M16     ; compute array size
  TXA       ; get result low byte
  ADC FAC1+FAC_MANT+2   ; add index low byte from FAC1 mantissa 3
  TAX       ; save result low byte
  TYA       ; get result high byte
  LDY INDEX     ; restore index
LAB_D320
  ADC FAC1+FAC_MANT+3   ; add index high byte from FAC1 mantissa 4
  STX FBUFPT      ; save array data pointer low byte
  DEC COUNT     ; decrement dimensions count
  BNE LAB_D2F2    ; loop if dimensions still to do

  STA FBUFPT+1    ; save array data pointer high byte
  LDX #$05      ; set default element size
  LDA VARNAM      ; get variable name 1st byte
  BPL LAB_D331    ; branch if not string or floating point array

  DEX       ; decrement element size, $04
LAB_D331
  LDA VARNAM+1    ; get variable name 2nd byte
  BPL LAB_D337    ; branch if not integer or string

  DEX       ; decrement element size, $03
  DEX       ; decrement element size, $02
LAB_D337
  STX RESHO+2     ; save dimension size low byte
  LDA #$00      ; clear dimension size high byte
  JSR LAB_D355    ; compute array size
  TXA       ; copy array size low byte
  ADC GENPTR      ; add array data start pointer low byte
  STA VARPNT      ; save as current variable pointer low byte
  TYA       ; copy array size high byte
  ADC GENPTR+1    ; add array data start pointer high byte
  STA VARPNT+1    ; save as current variable pointer high byte
  TAY       ; copy high byte to .Y
  LDA VARPNT      ; get current variable pointer low byte
          ; pointer to element is now in .A.Y
LAB_D34B
  RTS


;***********************************************************************************;
;
; compute array size, result in .X.Y

M16
  STY INDEX     ; save index
  LDA (TMPPTR),Y    ; get dimension size low byte
  STA RESHO+2     ; save dimension size low byte
  DEY       ; decrement index
  LDA (TMPPTR),Y    ; get dimension size high byte
LAB_D355
  STA RESHO+3     ; save dimension size high byte
  LDA #$10      ; count = $10 (16 bit multiply)
  STA LAB_5D      ; save bit count
  LDX #$00      ; clear result low byte
  LDY #$00      ; clear result high byte
LAB_D35F
  TXA       ; get result low byte
  ASL       ; *2
  TAX       ; save result low byte
  TYA       ; get result high byte
  ROL       ; *2
  TAY       ; save result high byte
  BCS LAB_D30B    ; if overflow go do "Out of memory" error

  ASL FBUFPT      ; shift element size low byte
  ROL FBUFPT+1    ; shift element size high byte
  BCC LAB_D378    ; skip add if no carry

  CLC       ; else clear carry for add
  TXA       ; get result low byte
  ADC RESHO+2     ; add dimension size low byte
  TAX       ; save result low byte
  TYA       ; get result high byte
  ADC RESHO+3     ; add dimension size high byte
  TAY       ; save result high byte
  BCS LAB_D30B    ; if overflow go do "Out of memory" error

LAB_D378
  DEC LAB_5D      ; decrement bit count
  BNE LAB_D35F    ; loop until all done

  RTS


;***********************************************************************************;
;
; perform FRE()

FRE
  LDA VALTYP      ; get data type flag, $FF = string, $00 = numeric
  BEQ LAB_D384    ; if numeric don't pop the string

  JSR LAB_D6A6    ; pop string off descriptor stack, or from top of string
          ; space returns with .A = length, .X=$71=pointer low byte,
          ; .Y=$72=pointer high byte

          ; FRE(n) was numeric so do this
LAB_D384
  JSR GRBCOL      ; go do garbage collection
  SEC       ; set carry for subtract
  LDA FRETOP      ; get bottom of string space low byte
  SBC STREND      ; subtract end of arrays low byte
  TAY       ; copy result to .Y
  LDA FRETOP+1    ; get bottom of string space high byte
  SBC STREND+1    ; subtract end of arrays high byte


;***********************************************************************************;
;
; convert fixed integer .A.Y to float FAC1

MAKFP
  LDX #$00      ; set type = numeric
  STX VALTYP      ; clear data type flag, $FF = string, $00 = numeric
  STA FAC1+FAC_MANT   ; save FAC1 mantissa 1
  STY FAC1+FAC_MANT+1   ; save FAC1 mantissa 2
  LDX #$90      ; set exponent=2^16 (integer)
  JMP INTFP1      ; set exp = .X, clear FAC1 3 and 4, normalise and return


;***********************************************************************************;
;
; perform POS()

POS
  SEC       ; set Cb for read cursor position
  JSR PLOT      ; read/set X,Y cursor position
LAB_D3A2
  LDA #$00      ; clear high byte
  BEQ MAKFP     ; convert fixed integer .A.Y to float FAC1, branch always


;***********************************************************************************;
;
; check not Direct, used by DEF and INPUT

NODIRM
  LDX CURLIN+1    ; get current line number high byte
  INX       ; increment it
  BNE LAB_D34B    ; return if not direct mode

          ; else do illegal direct error
  LDX #ER_ILLDIR    ; error $15, illegal direct error
  .byte $2C     ; makes next line BIT $1BA2
UNDEF
  LDX #ER_UNDEFUN   ; error $1B, undefined function error
  JMP ERROR     ; do error #.X then warm start


;***********************************************************************************;
;
; perform DEF

DEF
  JSR FN      ; check FNx syntax
  JSR NODIRM      ; check not direct, back here if ok
  JSR LPACHK      ; scan for "(", else do syntax error then warm start
  LDA #$80      ; set flag for FNx
  STA SUBFLG      ; save subscript/FNx flag
  JSR EVLVAR      ; get variable address
  JSR LAB_CD8D    ; check if source is numeric, else do type mismatch
  JSR RPACHK      ; scan for ")", else do syntax error then warm start
  LDA #TK_EQUAL   ; get = token
  JSR SYNCHR      ; scan for CHR$(.A), else do syntax error then warm start
  PHA       ; push next character
  LDA VARPNT+1    ; get current variable pointer high byte
  PHA       ; push it
  LDA VARPNT      ; get current variable pointer low byte
  PHA       ; push it
  LDA CHRGOT+2    ; get BASIC execute pointer high byte
  PHA       ; push it
  LDA CHRGOT+1    ; get BASIC execute pointer low byte
  PHA       ; push it
  JSR SKIPST      ; perform DATA
  JMP EVFN3     ; put execute pointer and variable pointer into function
          ; and return


;***********************************************************************************;
;
; check FNx syntax

FN
  LDA #TK_FN      ; set FN token
  JSR SYNCHR      ; scan for CHR$(.A), else do syntax error then warm start
  ORA #$80      ; set FN flag bit
  STA SUBFLG      ; save FN name
  JSR LAB_D092    ; search for FN variable
  STA DEFPNT      ; save function pointer low byte
  STY DEFPNT+1    ; save function pointer high byte
  JMP LAB_CD8D    ; check if source is numeric and return, else do type
          ; mismatch


;***********************************************************************************;
;
; Evaluate FNx

EVALFN
  JSR FN      ; check FNx syntax
  LDA DEFPNT+1    ; get function pointer high byte
  PHA       ; push it
  LDA DEFPNT      ; get function pointer low byte
  PHA       ; push it
  JSR PAREXP      ; evaluate expression within parentheses
  JSR LAB_CD8D    ; check if source is numeric, else do type mismatch
  PLA       ; pop function pointer low byte
  STA DEFPNT      ; restore it
  PLA       ; pop function pointer high byte
  STA DEFPNT+1    ; restore it
  LDY #$02      ; index to variable pointer high byte
  LDA (DEFPNT),Y    ; get variable address low byte
  STA VARPNT      ; save current variable pointer low byte
  TAX       ; copy address low byte
  INY       ; index to variable address high byte
  LDA (DEFPNT),Y    ; get variable pointer high byte
  BEQ UNDEF     ; if high byte zero go do undefined function error

  STA VARPNT+1    ; save current variable pointer high byte
  INY       ; index to mantissa 3

          ; now stack the function variable value before use
LAB_D418
  LDA (VARPNT),Y    ; get byte from variable
  PHA       ; stack it
  DEY       ; decrement index
  BPL LAB_D418    ; loop until variable stacked

  LDY VARPNT+1    ; get current variable pointer high byte
  JSR STORFAC     ; pack FAC1 into (.X.Y)
  LDA CHRGOT+2    ; get BASIC execute pointer high byte
  PHA       ; push it
  LDA CHRGOT+1    ; get BASIC execute pointer low byte
  PHA       ; push it
  LDA (DEFPNT),Y    ; get function execute pointer low byte
  STA CHRGOT+1    ; save BASIC execute pointer low byte
  INY       ; index to high byte
  LDA (DEFPNT),Y    ; get function execute pointer high byte
  STA CHRGOT+2    ; save BASIC execute pointer high byte
  LDA VARPNT+1    ; get current variable pointer high byte
  PHA       ; push it
  LDA VARPNT      ; get current variable pointer low byte
  PHA       ; push it
  JSR TYPCHK      ; evaluate expression and check is numeric, else do
          ; type mismatch
  PLA       ; pull variable address low byte
  STA DEFPNT      ; save variable address low byte
  PLA       ; pull variable address high byte
  STA DEFPNT+1    ; save variable address high byte
  JSR CHRGOT      ; scan memory
  BEQ LAB_D449    ; if null (should be [EOL] marker) continue

  JMP LAB_CF08    ; else syntax error then warm start

; restore BASIC execute pointer and function variable from stack

LAB_D449
  PLA       ; pull BASIC execute pointer low byte
  STA CHRGOT+1    ; save BASIC execute pointer low byte
  PLA       ; pull BASIC execute pointer high byte
  STA CHRGOT+2    ; save BASIC execute pointer high byte

; put execute pointer and variable pointer into function

EVFN3
  LDY #$00      ; clear index
  PLA       ; pull BASIC execute pointer low byte
  STA (DEFPNT),Y    ; save to function
  PLA       ; pull BASIC execute pointer high byte
  INY       ; increment index
  STA (DEFPNT),Y    ; save to function
  PLA       ; pull current variable address low byte
  INY       ; increment index
  STA (DEFPNT),Y    ; save to function
  PLA       ; pull current variable address high byte
  INY       ; increment index
  STA (DEFPNT),Y    ; save to function
  PLA       ; pull character following '='
  INY       ; increment index
  STA (DEFPNT),Y    ; save to function
  RTS


;***********************************************************************************;
;
; perform STR$()

STR
  JSR LAB_CD8D    ; check if source is numeric, else do type mismatch
  LDY #$00      ; set string index
  JSR LAB_DDDF    ; convert FAC1 to string
  PLA       ; dump return address (skip type check)
  PLA       ; dump return address (skip type check)
LAB_D46F
  LDA #<BASZPT    ; set result string low pointer
  LDY #>BASZPT    ; set result string high pointer
  BEQ MAKSTR      ; print null terminated string to utility pointer


;***********************************************************************************;
;
; do string vector
; copy descriptor pointer and make string space .A bytes long

ALC1
  LDX FAC1+3      ; get descriptor pointer low byte
  LDY FAC1+4      ; get descriptor pointer high byte
  STX DSCPTN      ; save descriptor pointer low byte
  STY DSCPTN+1    ; save descriptor pointer high byte


;***********************************************************************************;
;
; make string space .A bytes long

LAB_D47D
  JSR ALCSPAC     ; make space in string memory for string .A long
  STX FAC1+1      ; save string pointer low byte
  STY FAC1+2      ; save string pointer high byte
  STA FAC1      ; save length
  RTS


;***********************************************************************************;
;
; scan, set up string
; print " terminated string to utility pointer

MAKSTR
  LDX #$22      ; set terminator to "
  STX CHARAC      ; set search character, terminator 1
  STX ENDCHR      ; set terminator 2

; print search or alternate terminated string to utility pointer
; source is .A.Y

LAB_D48D
  STA ARISGN      ; store string start low byte
  STY FACOV     ; store string start high byte
  STA FAC1+1      ; save string pointer low byte
  STY FAC1+2      ; save string pointer high byte
  LDY #$FF      ; set length to -1
LAB_D497
  INY       ; increment length
  LDA (ARISGN),Y    ; get byte from string
  BEQ LAB_D4A8    ; exit loop if null byte [EOS]

  CMP CHARAC      ; compare with search character, terminator 1
  BEQ LAB_D4A4    ; branch if terminator

  CMP ENDCHR      ; compare with terminator 2
  BNE LAB_D497    ; loop if not terminator 2

LAB_D4A4
  CMP #$22      ; compare with "
  BEQ LAB_D4A9    ; branch if " (carry set if = !)

LAB_D4A8
  CLC       ; clear carry for add (only if [EOL] terminated string)
LAB_D4A9
  STY FAC1+FAC_EXPT   ; save length in FAC1 exponent
  TYA       ; copy length to .A
  ADC ARISGN      ; add string start low byte
  STA FBUFPT      ; save string end low byte
  LDX FACOV     ; get string start high byte
  BCC LAB_D4B5    ; if no low byte overflow skip the high byte increment

  INX       ; else increment high byte
LAB_D4B5
  STX FBUFPT+1    ; save string end high byte
  LDA FACOV     ; get string start high byte
  BEQ LAB_D4BF    ; branch if in utility area

  CMP #$02      ; compare with input buffer memory high byte
  BNE LAB_D4CA    ; branch if not in input buffer memory

          ; string in input buffer or utility area, move to string
          ; memory
LAB_D4BF
  TYA       ; copy length to .A
  JSR ALC1      ; copy descriptor pointer and make string space .A bytes long
  LDX ARISGN      ; get string start low byte
  LDY FACOV     ; get string start high byte
  JSR LAB_D688    ; store string .A bytes long from .X.Y to utility pointer

; check for space on descriptor stack then ...
; put string address and length on descriptor stack and update stack pointers

LAB_D4CA
  LDX TEMPPT      ; get descriptor stack pointer
  CPX #$22      ; compare with max+1
  BNE LAB_D4D5    ; branch if space on string stack

          ; else do string too complex error
  LDX #ER_FMLA2CPLX   ; error $19, formula too complex error
LAB_D4D2
  JMP ERROR     ; do error #.X then warm start

; put string address and length on descriptor stack and update stack pointers

LAB_D4D5
  LDA FAC1      ; get string length
  STA $00,X     ; put on string stack
  LDA FAC1+1      ; get string pointer low byte
  STA $01,X     ; put on string stack
  LDA FAC1+2      ; get string pointer high byte
  STA $02,X     ; put on string stack
  LDY #$00      ; clear .Y
  STX FAC1+3      ; save string descriptor pointer low byte
  STY FAC1+4      ; save string descriptor pointer high byte, always $00
  STY FACOV     ; clear FAC1 rounding byte
  DEY       ; .Y = $FF
  STY VALTYP      ; save data type flag, $FF = string
  STX LASTPT      ; save current descriptor stack item pointer low byte
  INX       ; update stack pointer
  INX       ; update stack pointer
  INX       ; update stack pointer
  STX TEMPPT      ; set new descriptor stack pointer
  RTS

; make space in string memory for string .A long
; return .X = pointer low byte, .Y = pointer high byte

ALCSPAC
  LSR GARBFL      ; clear garbage collected flag (b7)

          ; make space for string .A long
LAB_D4F6
  PHA       ; save string length
  EOR #$FF      ; complement it
  SEC       ; set carry for subtract, two's complement add
  ADC FRETOP      ; add bottom of string space low byte, subtract length
  LDY FRETOP+1    ; get bottom of string space high byte
  BCS LAB_D501    ; skip decrement if no underflow

  DEY       ; decrement bottom of string space high byte
LAB_D501
  CPY STREND+1    ; compare with end of arrays high byte
  BCC LAB_D516    ; do out of memory error if less

  BNE LAB_D50B    ; if not = skip next test

  CMP STREND      ; compare with end of arrays low byte
  BCC LAB_D516    ; do out of memory error if less

LAB_D50B
  STA FRETOP      ; save bottom of string space low byte
  STY FRETOP+1    ; save bottom of string space high byte
  STA FRESPC      ; save string utility ptr low byte
  STY FRESPC+1    ; save string utility ptr high byte
  TAX       ; copy low byte to .X
  PLA       ; get string length back
  RTS

LAB_D516
  LDX #$10      ; error code $10, out of memory error
  LDA GARBFL      ; get garbage collected flag
  BMI LAB_D4D2    ; if set then do error code .X

  JSR GRBCOL      ; else go do garbage collection
  LDA #$80      ; flag for garbage collected
  STA GARBFL      ; set garbage collected flag
  PLA       ; pull length
  BNE LAB_D4F6    ; go try again (loop always, length should never be = $00)


;***********************************************************************************;
;
; garbage collection routine

GRBCOL
  LDX MEMSIZ      ; get end of memory low byte
  LDA MEMSIZ+1    ; get end of memory high byte

; re-run routine from last ending

LAB_D52A
  STX FRETOP      ; set bottom of string space low byte
  STA FRETOP+1    ; set bottom of string space high byte
  LDY #$00      ; clear index
  STY DEFPNT+1    ; clear working pointer high byte
  STY DEFPNT      ; clear working pointer low byte
  LDA STREND      ; get end of arrays low byte
  LDX STREND+1    ; get end of arrays high byte
  STA TMPPTR      ; save as highest uncollected string pointer low byte
  STX TMPPTR+1    ; save as highest uncollected string pointer high byte
  LDA #TEMPST     ; set descriptor stack pointer
  LDX #$00      ; clear .X
  STA INDEX     ; save descriptor stack pointer low byte
  STX INDEX+1     ; save descriptor stack pointer high byte ($00)
LAB_D544
  CMP TEMPPT      ; compare with descriptor stack pointer
  BEQ LAB_D54D    ; branch if =

  JSR LAB_D5C7    ; check string salvageability
  BEQ LAB_D544    ; loop always

          ; done stacked strings, now do string variables
LAB_D54D
  LDA #$07      ; set step size = $07, collecting variables
  STA FOUR6     ; save garbage collection step size
  LDA VARTAB      ; get start of variables low byte
  LDX VARTAB+1    ; get start of variables high byte
  STA INDEX     ; save as pointer low byte
  STX INDEX+1     ; save as pointer high byte
LAB_D559
  CPX ARYTAB+1    ; compare end of variables high byte,
          ; start of arrays high byte
  BNE LAB_D561    ; branch if no high byte match

  CMP ARYTAB      ; else compare end of variables low byte,
          ; start of arrays low byte
  BEQ LAB_D566    ; branch if = variable memory end

LAB_D561
  JSR GCOL13      ; check variable salvageability
  BEQ LAB_D559    ; loop always

          ; done string variables, now do string arrays
LAB_D566
  STA GENPTR      ; save start of arrays low byte as working pointer
  STX GENPTR+1    ; save start of arrays high byte as working pointer
  LDA #$03      ; set step size, collecting descriptors
  STA FOUR6     ; save step size
LAB_D56E
  LDA GENPTR      ; get pointer low byte
  LDX GENPTR+1    ; get pointer high byte
LAB_D572
  CPX STREND+1    ; compare with end of arrays high byte
  BNE LAB_D57D    ; branch if not at end

  CMP STREND      ; else compare with end of arrays low byte
  BNE LAB_D57D    ; branch if not at end

  JMP COLLECT     ; collect string, tidy up and exit if at end ??

LAB_D57D
  STA INDEX     ; save pointer low byte
  STX INDEX+1     ; save pointer high byte
  LDY #$00      ; set index
  LDA (INDEX),Y   ; get array name first byte
  TAX       ; copy it
  INY       ; increment index
  LDA (INDEX),Y   ; get array name second byte
  PHP       ; push the flags
  INY       ; increment index
  LDA (INDEX),Y   ; get array size low byte
  ADC GENPTR      ; add start of this array low byte
  STA GENPTR      ; save start of next array low byte
  INY       ; increment index
  LDA (INDEX),Y   ; get array size high byte
  ADC GENPTR+1    ; add start of this array high byte
  STA GENPTR+1    ; save start of next array high byte
  PLP       ; restore the flags
  BPL LAB_D56E    ; skip if not string array

; was possibly string array so ...

  TXA       ; get name first byte back
  BMI LAB_D56E    ; skip if not string array

  INY       ; increment index
  LDA (INDEX),Y   ; get # of dimensions
  LDY #$00      ; clear index
  ASL       ; *2
  ADC #$05      ; +5 (array header size)
  ADC INDEX     ; add pointer low byte
  STA INDEX     ; save pointer low byte
  BCC LAB_D5AE    ; if no rollover skip the high byte increment

  INC INDEX+1     ; else increment pointer high byte
LAB_D5AE
  LDX INDEX+1     ; get pointer high byte
LAB_D5B0
  CPX GENPTR+1    ; compare pointer high byte with end of this array high byte
  BNE LAB_D5B8    ; branch if not there yet

  CMP GENPTR      ; compare pointer low byte with end of this array low byte
  BEQ LAB_D572    ; if at end of this array go check next array

LAB_D5B8
  JSR LAB_D5C7    ; check string salvageability
  BEQ LAB_D5B0    ; loop

; check variable salvageability

GCOL13
  LDA (INDEX),Y   ; get variable name first byte
  BMI LAB_D5F6    ; add step and exit if not string

  INY       ; increment index
  LDA (INDEX),Y   ; get variable name second byte
  BPL LAB_D5F6    ; add step and exit if not string

  INY       ; increment index

; check string salvageability

LAB_D5C7
  LDA (INDEX),Y   ; get string length
  BEQ LAB_D5F6    ; add step and exit if null string

  INY       ; increment index
  LDA (INDEX),Y   ; get string pointer low byte
  TAX       ; copy to .X
  INY       ; increment index
  LDA (INDEX),Y   ; get string pointer high byte
  CMP FRETOP+1    ; compare string pointer high byte with bottom of string
          ; space high byte
  BCC LAB_D5DC    ; if bottom of string space greater go test against highest
          ; uncollected string

  BNE LAB_D5F6    ; if bottom of string space less string has been collected
          ; so go update pointers, step to next and return

          ; high bytes were equal so test low bytes
  CPX FRETOP      ; compare string pointer low byte with bottom of string
          ; space low byte
  BCS LAB_D5F6    ; if bottom of string space less string has been collected
          ; so go update pointers, step to next and return

          ; else test string against highest uncollected string so far
LAB_D5DC
  CMP TMPPTR+1    ; compare string pointer high byte with highest uncollected
          ; string high byte
  BCC LAB_D5F6    ; if highest uncollected string is greater then go update
          ; pointers, step to next and return

  BNE LAB_D5E6    ; if highest uncollected string is less then go set this
          ; string as highest uncollected so far

          ; high bytes were equal so test low bytes
  CPX TMPPTR      ; compare string pointer low byte with highest uncollected
          ; string low byte
  BCC LAB_D5F6    ; if highest uncollected string is greater then go update
          ; pointers, step to next and return

          ; else set current string as highest uncollected string
LAB_D5E6
  STX TMPPTR      ; save string pointer low byte as highest uncollected string
          ; low byte
  STA TMPPTR+1    ; save string pointer high byte as highest uncollected
          ; string high byte
  LDA INDEX     ; get descriptor pointer low byte
  LDX INDEX+1     ; get descriptor pointer high byte
  STA DEFPNT      ; save working pointer high byte
  STX DEFPNT+1    ; save working pointer low byte
  LDA FOUR6     ; get step size
  STA JMPER+1     ; copy step size
LAB_D5F6
  LDA FOUR6     ; get step size
  CLC       ; clear carry for add
  ADC INDEX     ; add pointer low byte
  STA INDEX     ; save pointer low byte
  BCC LAB_D601    ; if no rollover skip the high byte increment

  INC INDEX+1     ; else increment pointer high byte
LAB_D601
  LDX INDEX+1     ; get pointer high byte
  LDY #$00      ; flag not moved
  RTS


;***********************************************************************************;
;
; collect string

COLLECT
  LDA DEFPNT+1    ; get working pointer low byte
  ORA DEFPNT      ; OR working pointer high byte
  BEQ LAB_D601    ; exit if nothing to collect

  LDA JMPER+1     ; get copied step size
  AND #$04      ; mask step size, $04 for variables, $00 for array or stack
  LSR       ; >> 1
  TAY       ; copy to index
  STA JMPER+1     ; save offset to descriptor start
  LDA (DEFPNT),Y    ; get string length low byte
  ADC TMPPTR      ; add string start low byte
  STA GEN2PTR     ; set block end low byte
  LDA TMPPTR+1    ; get string start high byte
  ADC #$00      ; add carry
  STA GEN2PTR+1   ; set block end high byte
  LDA FRETOP      ; get bottom of string space low byte
  LDX FRETOP+1    ; get bottom of string space high byte
  STA GENPTR      ; save destination end low byte
  STX GENPTR+1    ; save destination end high byte
  JSR MOVEBL      ; open up space in memory, don't set array end. this
          ; copies the string from where it is to the end of the
          ; uncollected string memory
  LDY JMPER+1     ; restore offset to descriptor start
  INY       ; increment index to string pointer low byte
  LDA GENPTR      ; get new string pointer low byte
  STA (DEFPNT),Y    ; save new string pointer low byte
  TAX       ; copy string pointer low byte
  INC GENPTR+1    ; increment new string pointer high byte
  LDA GENPTR+1    ; get new string pointer high byte
  INY       ; increment index to string pointer high byte
  STA (DEFPNT),Y    ; save new string pointer high byte
  JMP LAB_D52A    ; re-run routine from last ending, .X.A holds new bottom
          ; of string memory pointer


;***********************************************************************************;
;
; concatenate
; add strings, the first string is in the descriptor, the second string is in line

ADDSTR
  LDA FAC1+4      ; get descriptor pointer high byte
  PHA       ; put on stack
  LDA FAC1+3      ; get descriptor pointer low byte
  PHA       ; put on stack
  JSR EVAL      ; get value from line
  JSR LAB_CD8F    ; check if source is string, else do type mismatch
  PLA       ; get descriptor pointer low byte back
  STA ARISGN      ; set pointer low byte
  PLA       ; get descriptor pointer high byte back
  STA FACOV     ; set pointer high byte
  LDY #$00      ; clear index
  LDA (ARISGN),Y    ; get length of first string from descriptor
  CLC       ; clear carry for add
  ADC (FAC1+3),Y    ; add length of second string
  BCC LAB_D65D    ; if no overflow continue

  LDX #ER_STR2LONG    ; else error $17, string too long error
  JMP ERROR     ; do error #.X then warm start

LAB_D65D
  JSR ALC1      ; copy descriptor pointer and make string space .A bytes long
  JSR XFERSTR     ; copy string from descriptor to utility pointer
  LDA DSCPTN      ; get descriptor pointer low byte
  LDY DSCPTN+1    ; get descriptor pointer high byte
  JSR LAB_D6AA    ; pop (.Y.A) descriptor off stack or from top of string space
          ; returns with .A = length, .X = pointer low byte,
          ; .Y = pointer high byte
  JSR LAB_D68C    ; store string from pointer to utility pointer
  LDA ARISGN      ; get descriptor pointer low byte
  LDY FACOV     ; get descriptor pointer high byte
  JSR LAB_D6AA    ; pop (.Y.A) descriptor off stack or from top of string space
          ; returns with .A = length, .X = pointer low byte,
          ; .Y = pointer high byte
  JSR LAB_D4CA    ; check space on descriptor stack then put string address
          ; and length on descriptor stack and update stack pointers
  JMP LAB_CDB8    ; continue evaluation


;***********************************************************************************;
;
; copy string from descriptor to utility pointer

XFERSTR
  LDY #$00      ; clear index
  LDA (ARISGN),Y    ; get string length
  PHA       ; save it
  INY       ; increment index
  LDA (ARISGN),Y    ; get string pointer low byte
  TAX       ; copy to .X
  INY       ; increment index
  LDA (ARISGN),Y    ; get string pointer high byte
  TAY       ; copy to .Y
  PLA       ; get length back
LAB_D688
  STX INDEX     ; save string pointer low byte
  STY INDEX+1     ; save string pointer high byte


;***********************************************************************************;
;
; store string from pointer to utility pointer

LAB_D68C
  TAY       ; copy length as index
  BEQ LAB_D699    ; branch if null string

  PHA       ; save length
LAB_D690
  DEY       ; decrement length/index
  LDA (INDEX),Y   ; get byte from string
  STA (FRESPC),Y    ; save byte to destination
  TYA       ; copy length/index
  BNE LAB_D690    ; loop if not all done yet

  PLA       ; restore length
LAB_D699
  CLC       ; clear carry for add
  ADC FRESPC      ; add string utility ptr low byte
  STA FRESPC      ; save string utility ptr low byte
  BCC LAB_D6A2    ; if no rollover skip the high byte increment

  INC FRESPC+1    ; increment string utility ptr high byte
LAB_D6A2
  RTS


;***********************************************************************************;
;
; evaluate string

DELST
  JSR LAB_CD8F    ; check if source is string, else do type mismatch

; pop string off descriptor stack, or from top of string space
; returns with .A = length, .X = pointer low byte, .Y = pointer high byte

LAB_D6A6
  LDA FAC1+3      ; get descriptor pointer low byte
  LDY FAC1+4      ; get descriptor pointer high byte

; pop (.Y.A) descriptor off stack or from top of string space
; returns with .A = length, .X = pointer low byte, .Y = pointer high byte

LAB_D6AA
  STA INDEX     ; save string pointer low byte
  STY INDEX+1     ; save string pointer high byte
  JSR DELTSD      ; clean descriptor stack, .Y.A = pointer
  PHP       ; save status flags
  LDY #$00      ; clear index
  LDA (INDEX),Y   ; get length from string descriptor
  PHA       ; put on stack
  INY       ; increment index
  LDA (INDEX),Y   ; get string pointer low byte from descriptor
  TAX       ; copy to .X
  INY       ; increment index
  LDA (INDEX),Y   ; get string pointer high byte from descriptor
  TAY       ; copy to .Y
  PLA       ; get string length back
  PLP       ; restore status
  BNE LAB_D6D6    ; branch if pointer <> last_sl,last_sh

  CPY FRETOP+1    ; compare with bottom of string space high byte
  BNE LAB_D6D6    ; branch if <>

  CPX FRETOP      ; else compare with bottom of string space low byte
  BNE LAB_D6D6    ; branch if <>

  PHA       ; save string length
  CLC       ; clear carry for add
  ADC FRETOP      ; add bottom of string space low byte
  STA FRETOP      ; set bottom of string space low byte
  BCC LAB_D6D5    ; skip increment if no overflow

  INC FRETOP+1    ; increment bottom of string space high byte
LAB_D6D5
  PLA       ; restore string length
LAB_D6D6
  STX INDEX     ; save string pointer low byte
  STY INDEX+1     ; save string pointer high byte
  RTS


;***********************************************************************************;
;
; clean descriptor stack, .Y.A = pointer
; checks if .A.Y is on the descriptor stack, if so does a stack discard

DELTSD
  CPY LASTPT+1    ; compare high byte with current descriptor stack item
          ; pointer high byte
  BNE LAB_D6EB    ; exit if <>

  CMP LASTPT      ; compare low byte with current descriptor stack item
          ; pointer low byte
  BNE LAB_D6EB    ; exit if <>

  STA TEMPPT      ; set descriptor stack pointer
  SBC #$03      ; update last string pointer low byte
  STA LASTPT      ; save current descriptor stack item pointer low byte
  LDY #$00      ; clear high byte
LAB_D6EB
  RTS


;***********************************************************************************;
;
; perform CHR$()

CHR
  JSR LAB_D7A1    ; evaluate byte expression, result in .X
  TXA       ; copy to .A
  PHA       ; save character
  LDA #$01      ; string is single byte
  JSR LAB_D47D    ; make string space A bytes long
  PLA       ; get character back
  LDY #$00      ; clear index
  STA (FAC1+1),Y    ; save byte in string - byte IS string!
  PLA       ; dump return address (skip type check)
  PLA       ; dump return address (skip type check)
  JMP LAB_D4CA    ; check space on descriptor stack then put string address
          ; and length on descriptor stack and update stack pointers


;***********************************************************************************;
;
; perform LEFT$()

LEFT
  JSR FINLMR      ; pull string data and byte parameter from stack
          ; return pointer in descriptor, byte in .A (and .X), .Y=0
  CMP (DSCPTN),Y    ; compare byte parameter with string length
  TYA       ; clear .A
LAB_D706
  BCC LAB_D70C    ; branch if string length > byte parameter

  LDA (DSCPTN),Y    ; else make parameter = length
  TAX       ; copy to byte parameter copy
  TYA       ; clear string start offset
LAB_D70C
  PHA       ; save string start offset
LAB_D70D
  TXA       ; copy byte parameter (or string length if <)
LAB_D70E
  PHA       ; save string length
  JSR LAB_D47D    ; make string space .A bytes long
  LDA DSCPTN      ; get descriptor pointer low byte
  LDY DSCPTN+1    ; get descriptor pointer high byte
  JSR LAB_D6AA    ; pop (.Y.A) descriptor off stack or from top of string space
          ; returns with .A = length, .X = pointer low byte,
          ; .Y = pointer high byte
  PLA       ; get string length back
  TAY       ; copy length to .Y
  PLA       ; get string start offset back
  CLC       ; clear carry for add
  ADC INDEX     ; add start offset to string start pointer low byte
  STA INDEX     ; save string start pointer low byte
  BCC LAB_D725    ; if no overflow skip the high byte increment

  INC INDEX+1     ; else increment string start pointer high byte
LAB_D725
  TYA       ; copy length to .A
  JSR LAB_D68C    ; store string from pointer to utility pointer
  JMP LAB_D4CA    ; check space on descriptor stack then put string address
          ; and length on descriptor stack and update stack pointers


;***********************************************************************************;
;
; perform RIGHT$()

RIGHT
  JSR FINLMR      ; pull string data and byte parameter from stack
          ; return pointer in descriptor, byte in .A (and .X), .Y=0
  CLC       ; clear carry for add - 1
  SBC (DSCPTN),Y    ; subtract string length
  EOR #$FF      ; invert it (.A=LEN(expression$)-l)
  JMP LAB_D706    ; go do rest of LEFT$()


;***********************************************************************************;
;
; perform MID$()

MID
  LDA #$FF      ; set default length = 255
  STA FAC1+4      ; save default length
  JSR CHRGOT      ; scan memory
  CMP #$29      ; compare with ")"
  BEQ LAB_D748    ; branch if = ")" (skip second byte get)

  JSR COMCHK      ; scan for ",", else do syntax error then warm start
  JSR LAB_D79E    ; get byte parameter
LAB_D748
  JSR FINLMR      ; pull string data and byte parameter from stack
          ; return pointer in descriptor, byte in .A (and .X), .Y=0
  BEQ LAB_D798    ; if null do illegal quantity error then warm start

  DEX       ; decrement start index
  TXA       ; copy to .A
  PHA       ; save string start offset
  CLC       ; clear carry for sub - 1
  LDX #$00      ; clear output string length
  SBC (DSCPTN),Y    ; subtract string length
  BCS LAB_D70D    ; if start>string length go do null string

  EOR #$FF      ; complement -length
  CMP FAC1+4      ; compare byte parameter
  BCC LAB_D70E    ; if length>remaining string go do RIGHT$

  LDA FAC1+4      ; get length byte
  BCS LAB_D70E    ; go do string copy, branch always


;***********************************************************************************;
;
; pull string data and byte parameter from stack
; return pointer in descriptor, byte in .A (and .X), .Y=0

FINLMR
  JSR RPACHK      ; scan for ")", else do syntax error then warm start
  PLA       ; pull return address low byte
  TAY       ; save return address low byte
  PLA       ; pull return address high byte
  STA JMPER+1     ; save return address high byte
  PLA       ; dump call to function vector low byte
  PLA       ; dump call to function vector high byte
  PLA       ; pull byte parameter
  TAX       ; copy byte parameter to .X
  PLA       ; pull string pointer low byte
  STA DSCPTN      ; save it
  PLA       ; pull string pointer high byte
  STA DSCPTN+1    ; save it
  LDA JMPER+1     ; get return address high byte
  PHA       ; back on stack
  TYA       ; get return address low byte
  PHA       ; back on stack
  LDY #$00      ; clear index
  TXA       ; copy byte parameter
  RTS


;***********************************************************************************;
;
; perform LEN()

LEN
  JSR GSINFO      ; evaluate string, get length in .A (and .Y)
  JMP LAB_D3A2    ; convert .Y to byte in FAC1 and return


;***********************************************************************************;
;
; evaluate string, get length in .Y

GSINFO
  JSR DELST     ; evaluate string
  LDX #$00      ; set data type = numeric
  STX VALTYP      ; clear data type flag, $FF = string, $00 = numeric
  TAY       ; copy length to .Y
  RTS


;***********************************************************************************;
;
; perform ASC()

ASC
  JSR GSINFO      ; evaluate string, get length in .A (and .Y)
  BEQ LAB_D798    ; if null do illegal quantity error then warm start

  LDY #$00      ; set index to first character
  LDA (INDEX),Y   ; get byte
  TAY       ; copy to .Y
  JMP LAB_D3A2    ; convert .Y to byte in FAC1 and return


;***********************************************************************************;
;
; do illegal quantity error then warm start

LAB_D798
  JMP ILQUAN      ; do illegal quantity error then warm start


;***********************************************************************************;
;
; scan and get byte parameter

GETBYT
  JSR CHRGET      ; increment and scan memory

; get byte parameter

LAB_D79E
  JSR TYPCHK      ; evaluate expression and check is numeric, else do
          ; type mismatch

; evaluate byte expression, result in .X

LAB_D7A1
  JSR LAB_D1B8    ; evaluate integer expression, sign check

  LDX FAC1+FAC_MANT+2   ; get FAC1 mantissa 3
  BNE LAB_D798    ; if not null do illegal quantity error then warm start

  LDX FAC1+FAC_MANT+3   ; get FAC1 mantissa 4
  JMP CHRGOT      ; scan memory and return


;***********************************************************************************;
;
; perform VAL()

VAL
  JSR GSINFO      ; evaluate string, get length in .A (and .Y)
  BNE LAB_D7B5    ; if not a null string go evaluate it

          ; string was null so set result = $00
  JMP ZERFAC      ; clear FAC1 exponent and sign and return

LAB_D7B5
  LDX CHRGOT+1    ; get BASIC execute pointer low byte
  LDY CHRGOT+2    ; get BASIC execute pointer high byte
  STX FBUFPT      ; save BASIC execute pointer low byte
  STY FBUFPT+1    ; save BASIC execute pointer high byte
  LDX INDEX     ; get string pointer low byte
  STX CHRGOT+1    ; save BASIC execute pointer low byte
  CLC       ; clear carry for add
  ADC INDEX     ; add string length
  STA INDEX+2     ; save string end low byte
  LDX INDEX+1     ; get string pointer high byte
  STX CHRGOT+2    ; save BASIC execute pointer high byte
  BCC LAB_D7CD    ; if no rollover skip the high byte increment

  INX       ; increment string end high byte
LAB_D7CD
  STX INDEX+3     ; save string end high byte
  LDY #$00      ; set index to $00
  LDA (INDEX+2),Y   ; get string end byte
  PHA       ; push it
  TYA       ; clear .A
  STA (INDEX+2),Y   ; terminate string with $00
  JSR CHRGOT      ; scan memory
  JSR ASCFLT      ; get FAC1 from string
  PLA       ; restore string end byte
  LDY #$00      ; clear index
  STA (INDEX+2),Y   ; put string end byte back

; restore BASIC execute pointer from temp

LAB_D7E2
  LDX FBUFPT      ; get BASIC execute pointer low byte back
  LDY FBUFPT+1    ; get BASIC execute pointer high byte back
  STX CHRGOT+1    ; save BASIC execute pointer low byte
  STY CHRGOT+2    ; save BASIC execute pointer high byte
  RTS


;***********************************************************************************;
;
; get parameters for POKE/WAIT

GETAD
  JSR TYPCHK      ; evaluate expression and check is numeric, else do
          ; type mismatch
  JSR MAKADR      ; convert FAC1 to integer in temporary integer
LAB_D7F1
  JSR COMCHK      ; scan for ",", else do syntax error then warm start
  JMP LAB_D79E    ; get byte parameter and return


;***********************************************************************************;
;
; convert FAC1 to integer in temporary integer

MAKADR
  LDA FAC1+FAC_SIGN   ; get FAC1 sign
  BMI LAB_D798    ; if -ve do illegal quantity error then warm start

  LDA FAC1+FAC_EXPT   ; get FAC1 exponent
  CMP #$91      ; compare with exponent = 2^16
  BCS LAB_D798    ; if >= do illegal quantity error then warm start

  JSR FPINT     ; convert FAC1 floating to fixed
  LDA FAC1+FAC_MANT+2   ; get FAC1 mantissa 3
  LDY FAC1+FAC_MANT+3   ; get FAC1 mantissa 4
  STY LINNUM      ; save temporary integer low byte
  STA LINNUM+1    ; save temporary integer high byte
  RTS


;***********************************************************************************;
;
; perform PEEK()

PEEK
  LDA LINNUM+1    ; get line number high byte
  PHA       ; save line number high byte
  LDA LINNUM      ; get line number low byte
  PHA       ; save line number low byte
  JSR MAKADR      ; convert FAC1 to integer in temporary integer
  LDY #$00      ; clear index
  LDA (LINNUM),Y    ; read byte
  TAY       ; copy byte to .A
  PLA       ; pull byte
  STA LINNUM      ; restore line number low byte
  PLA       ; pull byte
  STA LINNUM+1    ; restore line number high byte
  JMP LAB_D3A2    ; convert .Y to byte in FAC1 and return


;***********************************************************************************;
;
; perform POKE

POKE
  JSR GETAD     ; get parameters for POKE/WAIT
  TXA       ; copy byte to .A
  LDY #$00      ; clear index
  STA (LINNUM),Y    ; write byte
  RTS


;***********************************************************************************;
;
; perform WAIT

WAIT
  JSR GETAD     ; get parameters for POKE/WAIT
  STX FORPNT      ; save byte
  LDX #$00      ; clear mask
  JSR CHRGOT      ; scan memory
  BEQ LAB_D83C    ; skip if no third argument

  JSR LAB_D7F1    ; scan for "," and get byte, else syntax error then
          ; warm start
LAB_D83C
  STX FORPNT+1    ; save XOR argument
  LDY #$00      ; clear index
LAB_D840
  LDA (LINNUM),Y    ; get byte via temporary integer  (address)
  EOR FORPNT+1    ; XOR with second argument    (mask)
  AND FORPNT      ; AND with first argument   (byte)
  BEQ LAB_D840    ; loop if result is zero

LAB_D848
  RTS


;***********************************************************************************;
;
; add 0.5 to FAC1 (round FAC1)

ADD05
  LDA #<FLP05     ; set 0.5 pointer low byte
  LDY #>FLP05     ; set 0.5 pointer high byte
  JMP LAPLUS      ; add (.A.Y) to FAC1


;***********************************************************************************;
;
; perform subtraction, FAC1 from (.A.Y)

LAMIN
  JSR LODARG      ; unpack memory (.A.Y) into FAC2

; perform subtraction, FAC1 from FAC2

SUB
  LDA FAC1+FAC_SIGN   ; get FAC1 sign (b7)
  EOR #$FF      ; complement it
  STA FAC1+FAC_SIGN   ; save FAC1 sign (b7)
  EOR FAC2+FAC_SIGN   ; XOR with FAC2 sign (b7)
  STA ARISGN      ; save sign compare (FAC1 XOR FAC2)
  LDA FAC1+FAC_EXPT   ; get FAC1 exponent
  JMP PLUS      ; add FAC2 to FAC1 and return

PLUS1
  JSR LAB_D999    ; shift FAC.X .A times right (>8 shifts)
  BCC LAB_D8A3    ; go subtract the mantissas, branch always


;***********************************************************************************;
;
; add (.A.Y) to FAC1

LAPLUS
  JSR LODARG      ; unpack memory (.A.Y) into FAC2

; add FAC2 to FAC1

PLUS
  BNE LAB_D86F    ; if FAC1 is not zero go do the add

  JMP ATOF      ; FAC1 was zero so copy FAC2 to FAC1 and return

          ; FAC1 is non zero
LAB_D86F
  LDX FACOV     ; get FAC1 rounding byte
  STX JMPER+2     ; save as FAC2 rounding byte
  LDX #FAC2     ; set index to FAC2 exponent address
  LDA FAC2      ; get FAC2 exponent
LAB_D877
  TAY       ; copy exponent
  BEQ LAB_D848    ; exit if zero

  SEC       ; set carry for subtract
  SBC FAC1+FAC_EXPT   ; subtract FAC1 exponent
  BEQ LAB_D8A3    ; if equal go add mantissas

  BCC LAB_D893    ; if FAC2 < FAC1 then go shift FAC2 right

          ; else FAC2 > FAC1
  STY FAC1+FAC_EXPT   ; save FAC1 exponent
  LDY FAC2+FAC_SIGN   ; get FAC2 sign (b7)
  STY FAC1+FAC_SIGN   ; save FAC1 sign (b7)
  EOR #$FF      ; complement .A
  ADC #$00      ; +1, two's complement, carry is set
  LDY #$00      ; clear .Y
  STY JMPER+2     ; clear FAC2 rounding byte
  LDX #FAC1+FAC_EXPT    ; set index to FAC1 exponent address
  BNE LAB_D897    ; branch always

          ; FAC2 < FAC1
LAB_D893
  LDY #$00      ; clear .Y
  STY FACOV     ; clear FAC1 rounding byte
LAB_D897
  CMP #$F9      ; compare exponent diff with $F9
  BMI PLUS1     ; branch if range $79-$F8

  TAY       ; copy exponent difference to .Y
  LDA FACOV     ; get FAC1 rounding byte
  LSR FAC_MANT,X    ; shift FAC.X mantissa 1
  JSR LAB_D9B0    ; shift FAC.X .Y times right

          ; exponents are equal now do mantissa subtract
LAB_D8A3
  BIT ARISGN      ; test sign compare (FAC1 XOR FAC2)
  BPL NORMLZ      ; if = add FAC2 mantissa to FAC1 mantissa and return

  LDY #FAC1+FAC_EXPT    ; set index to FAC1 exponent address
  CPX #FAC2+FAC_EXPT    ; compare .X to FAC2 exponent address
  BEQ LAB_D8AF    ; branch if =

  LDY #FAC2+FAC_EXPT    ; else set index to FAC2 exponent address

          ; subtract smaller from bigger (take sign of bigger)
LAB_D8AF
  SEC       ; set carry for subtract
  EOR #$FF      ; ones' complement .A
  ADC JMPER+2     ; add FAC2 rounding byte
  STA FACOV     ; save FAC1 rounding byte
  LDA FAC_MANT+3,Y    ; get FAC.Y mantissa 4
  SBC FAC_MANT+3,X    ; subtract FAC.X mantissa 4
  STA FAC1+FAC_MANT+3   ; save FAC1 mantissa 4
  LDA FAC_MANT+2,Y    ; get FAC.Y mantissa 3
  SBC FAC_MANT+2,X    ; subtract FAC.X mantissa 3
  STA FAC1+FAC_MANT+2   ; save FAC1 mantissa 3
  LDA FAC_MANT+1,Y    ; get FAC.Y mantissa 2
  SBC FAC_MANT+1,X    ; subtract FAC.X mantissa 2
  STA FAC1+FAC_MANT+1   ; save FAC1 mantissa 2
  LDA FAC_MANT,Y    ; get FAC.Y mantissa 1
  SBC FAC_MANT,X    ; subtract FAC.X mantissa 1
  STA FAC1+FAC_MANT   ; save FAC1 mantissa 1


;***********************************************************************************;
;
; do ABS and normalise FAC1

LAB_D8D2
  BCS LAB_D8D7    ; branch if number is +ve

  JSR COMFAC      ; negate FAC1

; normalise FAC1

LAB_D8D7
  LDY #$00      ; clear .Y
  TYA       ; clear .A
  CLC       ; clear carry for add
LAB_D8DB
  LDX FAC1+FAC_MANT   ; get FAC1 mantissa 1
  BNE LAB_D929    ; if not zero normalise FAC1

  LDX FAC1+FAC_MANT+1   ; get FAC1 mantissa 2
  STX FAC1+FAC_MANT   ; save FAC1 mantissa 1
  LDX FAC1+FAC_MANT+2   ; get FAC1 mantissa 3
  STX FAC1+FAC_MANT+1   ; save FAC1 mantissa 2
  LDX FAC1+FAC_MANT+3   ; get FAC1 mantissa 4
  STX FAC1+FAC_MANT+2   ; save FAC1 mantissa 3
  LDX FACOV     ; get FAC1 rounding byte
  STX FAC1+FAC_MANT+3   ; save FAC1 mantissa 4
  STY FACOV     ; clear FAC1 rounding byte
  ADC #$08      ; add x to exponent offset
  CMP #$20      ; compare with $20, max offset, all bits would be = 0
  BNE LAB_D8DB    ; loop if not max


;***********************************************************************************;
;
; clear FAC1 exponent and sign

ZERFAC
  LDA #$00      ; clear .A
LAB_D8F9
  STA FAC1+FAC_EXPT   ; set FAC1 exponent

; save FAC1 sign

LAB_D8FB
  STA FAC1+FAC_SIGN   ; save FAC1 sign (b7)
  RTS


;***********************************************************************************;
;
; add FAC2 mantissa to FAC1 mantissa

NORMLZ
  ADC JMPER+2     ; add FAC2 rounding byte
  STA FACOV     ; save FAC1 rounding byte
  LDA FAC1+FAC_MANT+3   ; get FAC1 mantissa 4
  ADC FAC2+FAC_MANT+3   ; add FAC2 mantissa 4
  STA FAC1+FAC_MANT+3   ; save FAC1 mantissa 4
  LDA FAC1+FAC_MANT+2   ; get FAC1 mantissa 3
  ADC FAC2+FAC_MANT+2   ; add FAC2 mantissa 3
  STA FAC1+FAC_MANT+2   ; save FAC1 mantissa 3
  LDA FAC1+FAC_MANT+1   ; get FAC1 mantissa 2
  ADC FAC2+FAC_MANT+1   ; add FAC2 mantissa 2
  STA FAC1+FAC_MANT+1   ; save FAC1 mantissa 2
  LDA FAC1+FAC_MANT   ; get FAC1 mantissa 1
  ADC FAC2+FAC_MANT   ; add FAC2 mantissa 1
  STA FAC1+FAC_MANT   ; save FAC1 mantissa 1
  JMP LAB_D936    ; test and normalise FAC1 for Cb=0/1

LAB_D91D
  ADC #$01      ; add 1 to exponent offset
  ASL FACOV     ; shift FAC1 rounding byte
  ROL FAC1+FAC_MANT+3   ; shift FAC1 mantissa 4
  ROL FAC1+FAC_MANT+2   ; shift FAC1 mantissa 3
  ROL FAC1+FAC_MANT+1   ; shift FAC1 mantissa 2
  ROL FAC1+FAC_MANT   ; shift FAC1 mantissa 1


;***********************************************************************************;
;
; normalise FAC1

LAB_D929
  BPL LAB_D91D    ; loop if not normalised

  SEC       ; set carry for subtract
  SBC FAC1+FAC_EXPT   ; subtract FAC1 exponent
  BCS ZERFAC      ; branch if underflow (set result = $0)

  EOR #$FF      ; complement exponent
  ADC #$01      ; +1 (two's complement)
  STA FAC1+FAC_EXPT   ; save FAC1 exponent

; test and normalise FAC1 for Cb=0/1

LAB_D936
  BCC LAB_D946    ; exit if no overflow

; normalise FAC1 for Cb=1

LAB_D938
  INC FAC1+FAC_EXPT   ; increment FAC1 exponent
  BEQ OVERFL      ; if zero do overflow error then warm start

  ROR FAC1+FAC_MANT   ; shift FAC1 mantissa 1
  ROR FAC1+FAC_MANT+1   ; shift FAC1 mantissa 2
  ROR FAC1+FAC_MANT+2   ; shift FAC1 mantissa 3
  ROR FAC1+FAC_MANT+3   ; shift FAC1 mantissa 4
  ROR FACOV     ; shift FAC1 rounding byte
LAB_D946
  RTS


;***********************************************************************************;
;
; negate FAC1

COMFAC
  LDA FAC1+FAC_SIGN   ; get FAC1 sign (b7)
  EOR #$FF      ; complement it
  STA FAC1+FAC_SIGN   ; save FAC1 sign (b7)

; two's complement FAC1 mantissa

LAB_D94D
  LDA FAC1+FAC_MANT   ; get FAC1 mantissa 1
  EOR #$FF      ; complement it
  STA FAC1+FAC_MANT   ; save FAC1 mantissa 1
  LDA FAC1+FAC_MANT+1   ; get FAC1 mantissa 2
  EOR #$FF      ; complement it
  STA FAC1+FAC_MANT+1   ; save FAC1 mantissa 2
  LDA FAC1+FAC_MANT+2   ; get FAC1 mantissa 3
  EOR #$FF      ; complement it
  STA FAC1+FAC_MANT+2   ; save FAC1 mantissa 3
  LDA FAC1+FAC_MANT+3   ; get FAC1 mantissa 4
  EOR #$FF      ; complement it
  STA FAC1+FAC_MANT+3   ; save FAC1 mantissa 4
  LDA FACOV     ; get FAC1 rounding byte
  EOR #$FF      ; complement it
  STA FACOV     ; save FAC1 rounding byte
  INC FACOV     ; increment FAC1 rounding byte
  BNE LAB_D97D    ; exit if no overflow

; increment FAC1 mantissa

LAB_D96F
  INC FAC1+FAC_MANT+3   ; increment FAC1 mantissa 4
  BNE LAB_D97D    ; finished if no rollover

  INC FAC1+FAC_MANT+2   ; increment FAC1 mantissa 3
  BNE LAB_D97D    ; finished if no rollover

  INC FAC1+FAC_MANT+1   ; increment FAC1 mantissa 2
  BNE LAB_D97D    ; finished if no rollover

  INC FAC1+FAC_MANT   ; increment FAC1 mantissa 1
LAB_D97D
  RTS


;***********************************************************************************;
;
; do overflow error then warm start

OVERFL
  LDX #ER_OVFLOW    ; error $0F, overflow error
  JMP ERROR     ; do error #.X then warm start


;***********************************************************************************;
;
; shift FACtemp << A+8 times

ASRRES
  LDX #$25      ; set offset to FACtemp
LAB_D985
  LDY FAC_MANT+3,X    ; get FAC.X mantissa 4
  STY FACOV     ; save as FAC1 rounding byte
  LDY FAC_MANT+2,X    ; get FAC.X mantissa 3
  STY FAC_MANT+3,X    ; save FAC.X mantissa 4
  LDY FAC_MANT+1,X    ; get FAC.X mantissa 2
  STY FAC_MANT+2,X    ; save FAC.X mantissa 3
  LDY FAC_MANT,X    ; get FAC.X mantissa 1
  STY FAC_MANT+1,X    ; save FAC.X mantissa 2
  LDY BITS      ; get FAC1 overflow byte
  STY FAC_MANT,X    ; save FAC.X mantissa 1

; shift FAC.X -.A times right (> 8 shifts)

LAB_D999
  ADC #$08      ; add 8 to shift count
  BMI LAB_D985    ; go do 8 shift if still -ve

  BEQ LAB_D985    ; go do 8 shift if zero

  SBC #$08      ; else subtract 8 again
  TAY       ; save count to .Y
  LDA FACOV     ; get FAC1 rounding byte
  BCS LAB_D9BA    ;.

LAB_D9A6
  ASL FAC_MANT,X    ; shift FAC.X mantissa 1
  BCC LAB_D9AC    ; branch if +ve

  INC FAC_MANT,X    ; this sets b7 eventually
LAB_D9AC
  ROR FAC_MANT,X    ; shift FAC.X mantissa 1 (correct for ASL)
  ROR FAC_MANT,X    ; shift FAC.X mantissa 1 (put carry in b7)

; shift FAC.X .Y times right

LAB_D9B0
  ROR FAC_MANT+1,X    ; shift FAC.X mantissa 2
  ROR FAC_MANT+2,X    ; shift FAC.X mantissa 3
  ROR FAC_MANT+3,X    ; shift FAC.X mantissa 4
  ROR       ; shift FAC.X rounding byte
  INY       ; increment exponent diff
  BNE LAB_D9A6    ; branch if range adjust not complete

LAB_D9BA
  CLC       ; just clear it
  RTS


;***********************************************************************************;
;
; constants and series for LOG(n)

FPC1
  .byte $81,$00,$00,$00,$00 ; 1

LOGCON
  .byte $03     ; series counter
  .byte $7F,$5E,$56,$CB,$79 ; 0.43425 LOG10(e)
  .byte $80,$13,$9B,$0B,$64 ; 0.57658
  .byte $80,$76,$38,$93,$16 ; 0.9618
  .byte $82,$38,$AA,$3B,$20 ; 2.88539 2/LOG(2)

LAB_D9D6
  .byte $80,$35,$04,$F3,$34 ; 0.70711 1/root 2
LAB_D9DB
  .byte $81,$35,$04,$F3,$34 ; 1.41421 root 2
LAB_D9E0
  .byte $80,$80,$00,$00,$00 ; -0.5    1/2
LAB_D9E5
  .byte $80,$31,$72,$17,$F8 ; 0.69315 LOG(2)


;***********************************************************************************;
;
; perform LOG()

LOG
  JSR SGNFAC      ; test sign and zero
  BEQ LAB_D9F1    ; if zero do illegal quantity error then warm start

  BPL LAB_D9F4    ; skip error if +ve

LAB_D9F1
  JMP ILQUAN      ; do illegal quantity error then warm start

LAB_D9F4
  LDA FAC1+FAC_EXPT   ; get FAC1 exponent
  SBC #$7F      ; normalise it
  PHA       ; save it
  LDA #$80      ; set exponent to zero
  STA FAC1+FAC_EXPT   ; save FAC1 exponent
  LDA #<LAB_D9D6    ; pointer to 1/root 2 low byte
  LDY #>LAB_D9D6    ; pointer to 1/root 2 high byte
  JSR LAPLUS      ; add (.A.Y) to FAC1 (1/root2)
  LDA #<LAB_D9DB    ; pointer to root 2 low byte
  LDY #>LAB_D9DB    ; pointer to root 2 high byte
  JSR LADIV     ; convert .A.Y and do (.A.Y)/FAC1 (root2/(x+(1/root2)))
  LDA #<FPC1      ; pointer to 1 low byte
  LDY #>FPC1      ; pointer to 1 high byte
  JSR LAMIN     ; subtract FAC1 ((root2/(x+(1/root2)))-1) from (.A.Y)
  LDA #<LOGCON    ; pointer to series for LOG(n) low byte
  LDY #>LOGCON    ; pointer to series for LOG(n) high byte
  JSR SEREVL      ; ^2 then series evaluation
  LDA #<LAB_D9E0    ; pointer to -0.5 low byte
  LDY #>LAB_D9E0    ; pointer to -0.5 high byte
  JSR LAPLUS      ; add (.A.Y) to FAC1
  PLA       ; restore FAC1 exponent
  JSR ASCI8     ; evaluate new ASCII digit
  LDA #<LAB_D9E5    ; pointer to LOG(2) low byte
  LDY #>LAB_D9E5    ; pointer to LOG(2) high byte

; do convert .A.Y, FAC1*(.A.Y)

TIMES
  JSR LODARG      ; unpack memory (.A.Y) into FAC2
MULT
  BNE LAB_DA30    ; multiply FAC1 by FAC2 ??

  JMP LAB_DA8B    ; exit if zero

LAB_DA30
  JSR MULDIV      ; test and adjust accumulators
  LDA #$00      ; clear .A
  STA RESHO     ; clear temp mantissa 1
  STA RESHO+1     ; clear temp mantissa 2
  STA RESHO+2     ; clear temp mantissa 3
  STA RESHO+3     ; clear temp mantissa 4
  LDA FACOV     ; get FAC1 rounding byte
  JSR TIMES3      ; go do shift/add FAC2
  LDA FAC1+FAC_MANT+3   ; get FAC1 mantissa 4
  JSR TIMES3      ; go do shift/add FAC2
  LDA FAC1+FAC_MANT+2   ; get FAC1 mantissa 3
  JSR TIMES3      ; go do shift/add FAC2
  LDA FAC1+FAC_MANT+1   ; get FAC1 mantissa 2
  JSR TIMES3      ; go do shift/add FAC2
  LDA FAC1+FAC_MANT   ; get FAC1 mantissa 1
  JSR LAB_DA5E    ; go do shift/add FAC2
  JMP LAB_DB8F    ; copy temp to FAC1, normalise and return

TIMES3
  BNE LAB_DA5E    ; branch if byte <> zero

  JMP ASRRES      ; shift FACtemp << .A+8 times

          ; else do shift and add
LAB_DA5E
  LSR       ; shift byte
  ORA #$80      ; set top bit (mark for 8 times)
LAB_DA61
  TAY       ; copy result
  BCC LAB_DA7D    ; skip next if bit was zero

  CLC       ; clear carry for add
  LDA RESHO+3     ; get temp mantissa 4
  ADC FAC2+FAC_MANT+3   ; add FAC2 mantissa 4
  STA RESHO+3     ; save temp mantissa 4
  LDA RESHO+2     ; get temp mantissa 3
  ADC FAC2+FAC_MANT+2   ; add FAC2 mantissa 3
  STA RESHO+2     ; save temp mantissa 3
  LDA RESHO+1     ; get temp mantissa 2
  ADC FAC2+FAC_MANT+1   ; add FAC2 mantissa 2
  STA RESHO+1     ; save temp mantissa 2
  LDA RESHO     ; get temp mantissa 1
  ADC FAC2+FAC_MANT   ; add FAC2 mantissa 1
  STA RESHO     ; save temp mantissa 1
LAB_DA7D
  ROR RESHO     ; shift temp mantissa 1
  ROR RESHO+1     ; shift temp mantissa 2
  ROR RESHO+2     ; shift temp mantissa 3
  ROR RESHO+3     ; shift temp mantissa 4
  ROR FACOV     ; shift temp rounding byte
  TYA       ; get byte back
  LSR       ; shift byte
  BNE LAB_DA61    ; loop if all bits not done

LAB_DA8B
  RTS


;***********************************************************************************;
;
; unpack memory (.A.Y) into FAC2

LODARG
  STA INDEX     ; save pointer low byte
  STY INDEX+1     ; save pointer high byte
  LDY #$04      ; 5 bytes to get (0-4)
  LDA (INDEX),Y   ; get mantissa 4
  STA FAC2+FAC_MANT+3   ; save FAC2 mantissa 4
  DEY       ; decrement index
  LDA (INDEX),Y   ; get mantissa 3
  STA FAC2+FAC_MANT+2   ; save FAC2 mantissa 3
  DEY       ; decrement index
  LDA (INDEX),Y   ; get mantissa 2
  STA FAC2+FAC_MANT+1   ; save FAC2 mantissa 2
  DEY       ; decrement index
  LDA (INDEX),Y   ; get mantissa 1 + sign
  STA FAC2+FAC_SIGN   ; save FAC2 sign (b7)
  EOR FAC1+FAC_SIGN   ; XOR with FAC1 sign (b7)
  STA ARISGN      ; save sign compare (FAC1 XOR FAC2)
  LDA FAC2+FAC_SIGN   ; recover FAC2 sign (b7)
  ORA #$80      ; set 1xxx xxxx (set normal bit)
  STA FAC2+FAC_MANT   ; save FAC2 mantissa 1
  DEY       ; decrement index
  LDA (INDEX),Y   ; get exponent byte
  STA FAC2+FAC_EXPT   ; save FAC2 exponent
  LDA FAC1+FAC_EXPT   ; get FAC1 exponent
  RTS


;***********************************************************************************;
;
; test and adjust accumulators

MULDIV
  LDA FAC2+FAC_EXPT   ; get FAC2 exponent

LAB_DAB9
  BEQ LAB_DADA    ; branch if FAC2 = $00 (handle underflow)

  CLC       ; clear carry for add
  ADC FAC1+FAC_EXPT   ; add FAC1 exponent
  BCC LAB_DAC4    ; branch if sum of exponents < $0100

  BMI LAB_DADF    ; do overflow error

  CLC       ; clear carry for the add
  .byte $2C     ; makes next line BIT $1410
LAB_DAC4
  BPL LAB_DADA    ; if +ve go handle underflow

  ADC #$80      ; adjust exponent
  STA FAC1+FAC_EXPT   ; save FAC1 exponent
  BNE LAB_DACF    ; branch if not zero

  JMP LAB_D8FB    ; save FAC1 sign and return


LAB_DACF
  LDA ARISGN      ; get sign compare (FAC1 XOR FAC2)
  STA FAC1+FAC_SIGN   ; save FAC1 sign (b7)
  RTS


;***********************************************************************************;
;
; handle overflow and underflow

LAB_DAD4
  LDA FAC1+FAC_SIGN   ; get FAC1 sign (b7)
  EOR #$FF      ; complement it
  BMI LAB_DADF    ; do overflow error

          ; handle underflow
LAB_DADA
  PLA       ; pop return address low byte
  PLA       ; pop return address high byte
  JMP ZERFAC      ; clear FAC1 exponent and sign and return

LAB_DADF
  JMP OVERFL      ; do overflow error then warm start


;***********************************************************************************;
;
; multiply FAC1 by 10

MULTEN
  JSR RFTOA     ; round and copy FAC1 to FAC2
  TAX       ; copy exponent (set the flags)
  BEQ LAB_DAF8    ; exit if zero

  CLC       ; clear carry for add
  ADC #$02      ; add two to exponent (*4)
  BCS LAB_DADF    ; do overflow error if > $FF

; FAC1 = (FAC1 + FAC2) * 2

LAB_DAED
  LDX #$00      ; clear byte
  STX ARISGN      ; clear sign compare (FAC1 XOR FAC2)
  JSR LAB_D877    ; add FAC2 to FAC1 (*5)
  INC FAC1+FAC_EXPT   ; increment FAC1 exponent (*10)
  BEQ LAB_DADF    ; if exponent now zero go do overflow error

LAB_DAF8
  RTS


;***********************************************************************************;
;
; 10 as a floating value

FPCTEN
  .byte $84,$20,$00,$00,$00 ; 10


;***********************************************************************************;
;
; divide FAC1 by 10

DIVTEN
  JSR RFTOA     ; round and copy FAC1 to FAC2
  LDA #<FPCTEN    ; set 10 pointer low byte
  LDY #>FPCTEN    ; set 10 pointer high byte
  LDX #$00      ; clear sign

; divide by (.A.Y) (.X=sign)

LAB_DB07
  STX ARISGN      ; save sign compare (FAC1 XOR FAC2)
  JSR LODFAC      ; unpack memory (.A.Y) into FAC1
  JMP DIVIDE      ; do FAC2/FAC1

          ; Perform divide-by

; convert .A.Y and do (.A.Y)/FAC1

LADIV
  JSR LODARG      ; unpack memory (.A.Y) into FAC2
DIVIDE
  BEQ LAB_DB8A    ; if zero go do /0 error

  JSR ROUND     ; round FAC1
  LDA #$00      ; clear .A
  SEC       ; set carry for subtract
  SBC FAC1+FAC_EXPT   ; subtract FAC1 exponent (2's complement)
  STA FAC1+FAC_EXPT   ; save FAC1 exponent
  JSR MULDIV      ; test and adjust accumulators
  INC FAC1+FAC_EXPT   ; increment FAC1 exponent
  BEQ LAB_DADF    ; if zero do overflow error

  LDX #$FC      ; set index to FAC temp
  LDA #$01      ;.set byte
LAB_DB29
  LDY FAC2+FAC_MANT   ; get FAC2 mantissa 1
  CPY FAC1+FAC_MANT   ; compare FAC1 mantissa 1
  BNE LAB_DB3F    ; if <> go use the result

  LDY FAC2+FAC_MANT+1   ; get FAC2 mantissa 2
  CPY FAC1+FAC_MANT+1   ; compare FAC1 mantissa 2
  BNE LAB_DB3F    ; if <> go use the result

  LDY FAC2+FAC_MANT+2   ; get FAC2 mantissa 3
  CPY FAC1+FAC_MANT+2   ; compare FAC1 mantissa 3
  BNE LAB_DB3F    ; if <> go use the result

  LDY FAC2+FAC_MANT+3   ; get FAC2 mantissa 4
  CPY FAC1+FAC_MANT+3   ; compare FAC1 mantissa 4
LAB_DB3F
  PHP       ; save the FAC2-FAC1 compare status
  ROL       ;.shift byte
  BCC LAB_DB4C    ; skip next if no carry

  INX       ; increment index to FAC temp
  STA RESHO+3,X   ;.
  BEQ LAB_DB7A    ;.

  BPL LAB_DB7E    ;.

  LDA #$01      ;.
LAB_DB4C
  PLP       ; restore FAC2-FAC1 compare status
  BCS LAB_DB5D    ; if FAC2 >= FAC1 then do subtract

          ; FAC2 = FAC2*2
LAB_DB4F
  ASL FAC2+FAC_MANT+3   ; shift FAC2 mantissa 4
  ROL FAC2+FAC_MANT+2   ; shift FAC2 mantissa 3
  ROL FAC2+FAC_MANT+1   ; shift FAC2 mantissa 2
  ROL FAC2+FAC_MANT   ; shift FAC2 mantissa 1
  BCS LAB_DB3F    ; loop with no compare

  BMI LAB_DB29    ; loop with compare

  BPL LAB_DB3F    ; loop always with no compare

LAB_DB5D
  TAY       ; save FAC2-FAC1 compare status
  LDA FAC2+FAC_MANT+3   ; get FAC2 mantissa 4
  SBC FAC1+FAC_MANT+3   ; subtract FAC1 mantissa 4
  STA FAC2+FAC_MANT+3   ; save FAC2 mantissa 4
  LDA FAC2+FAC_MANT+2   ; get FAC2 mantissa 3
  SBC FAC1+FAC_MANT+2   ; subtract FAC1 mantissa 3
  STA FAC2+FAC_MANT+2   ; save FAC2 mantissa 3
  LDA FAC2+FAC_MANT+1   ; get FAC2 mantissa 2
  SBC FAC1+FAC_MANT+1   ; subtract FAC1 mantissa 2
  STA FAC2+FAC_MANT+1   ; save FAC2 mantissa 2
  LDA FAC2+FAC_MANT   ; get FAC2 mantissa 1
  SBC FAC1+FAC_MANT   ; subtract FAC1 mantissa 1
  STA FAC2+FAC_MANT   ; save FAC2 mantissa 1
  TYA       ; restore FAC2-FAC1 compare status
  JMP LAB_DB4F    ; go shift FAC2

LAB_DB7A
  LDA #$40      ;.
  BNE LAB_DB4C    ; branch always

; do .A<<6, save as FAC1 rounding byte, normalise and return

LAB_DB7E
  ASL       ;
  ASL       ;
  ASL       ;
  ASL       ;
  ASL       ;
  ASL       ;
  STA FACOV     ; save FAC1 rounding byte
  PLP       ; dump FAC2-FAC1 compare status
  JMP LAB_DB8F    ; copy temp to FAC1, normalise and return

; do "Divide by zero" error

LAB_DB8A
  LDX #ER_DIVBY0    ; error $14, divide by zero error
  JMP ERROR     ; do error #.X then warm start

LAB_DB8F
  LDA RESHO     ; get temp mantissa 1
  STA FAC1+FAC_MANT   ; save FAC1 mantissa 1
  LDA RESHO+1     ; get temp mantissa 2
  STA FAC1+FAC_MANT+1   ; save FAC1 mantissa 2
  LDA RESHO+2     ; get temp mantissa 3
  STA FAC1+FAC_MANT+2   ; save FAC1 mantissa 3
  LDA RESHO+3     ; get temp mantissa 4
  STA FAC1+FAC_MANT+3   ; save FAC1 mantissa 4
  JMP LAB_D8D7    ; normalise FAC1 and return


;***********************************************************************************;
;
; unpack memory (.A.Y) into FAC1

LODFAC
  STA INDEX     ; save pointer low byte
  STY INDEX+1     ; save pointer high byte
  LDY #$04      ; 5 bytes to do
  LDA (INDEX),Y   ; get fifth byte
  STA FAC1+FAC_MANT+3   ; save FAC1 mantissa 4
  DEY       ; decrement index
  LDA (INDEX),Y   ; get fourth byte
  STA FAC1+FAC_MANT+2   ; save FAC1 mantissa 3
  DEY       ; decrement index
  LDA (INDEX),Y   ; get third byte
  STA FAC1+FAC_MANT+1   ; save FAC1 mantissa 2
  DEY       ; decrement index
  LDA (INDEX),Y   ; get second byte
  STA FAC1+FAC_SIGN   ; save FAC1 sign (b7)
  ORA #$80      ; set 1xxx xxxx (add normal bit)
  STA FAC1+FAC_MANT   ; save FAC1 mantissa 1
  DEY       ; decrement index
  LDA (INDEX),Y   ; get first byte (exponent)
  STA FAC1+FAC_EXPT   ; save FAC1 exponent
  STY FACOV     ; clear FAC1 rounding byte
  RTS


;***********************************************************************************;
;
; pack FAC1 into LAB_5C

FACTF2
  LDX #<LAB_5C    ; set pointer low byte
  .byte $2C     ; makes next line BIT $57A2

; pack FAC1 into TEMPF3

FACTF1
  LDX #<TEMPF3    ; set pointer low byte
  LDY #>TEMPF3    ; set pointer high byte
  BEQ STORFAC     ; pack FAC1 into (.X.Y) and return, branch always

; pack FAC1 into variable pointer

FACTFP
  LDX FORPNT      ; get destination pointer low byte
  LDY FORPNT+1    ; get destination pointer high byte

; pack FAC1 into (.X.Y)

STORFAC
  JSR ROUND     ; round FAC1
  STX INDEX     ; save pointer low byte
  STY INDEX+1     ; save pointer high byte
  LDY #$04      ; set index
  LDA FAC1+FAC_MANT+3   ; get FAC1 mantissa 4
  STA (INDEX),Y   ; store in destination
  DEY       ; decrement index
  LDA FAC1+FAC_MANT+2   ; get FAC1 mantissa 3
  STA (INDEX),Y   ; store in destination
  DEY       ; decrement index
  LDA FAC1+FAC_MANT+1   ; get FAC1 mantissa 2
  STA (INDEX),Y   ; store in destination
  DEY       ; decrement index
  LDA FAC1+FAC_SIGN   ; get FAC1 sign (b7)
  ORA #$7F      ; set bits x111 1111
  AND FAC1+FAC_MANT   ; AND in FAC1 mantissa 1
  STA (INDEX),Y   ; store in destination
  DEY       ; decrement index
  LDA FAC1+FAC_EXPT   ; get FAC1 exponent
  STA (INDEX),Y   ; store in destination
  STY FACOV     ; clear FAC1 rounding byte
  RTS


;***********************************************************************************;
;
; copy FAC2 to FAC1

ATOF
  LDA FAC2+FAC_SIGN   ; get FAC2 sign (b7)

; save FAC1 sign and copy ABS(FAC2) to FAC1

LAB_DBFE
  STA FAC1+FAC_SIGN   ; save FAC1 sign (b7)
  LDX #$05      ; 5 bytes to copy
LAB_DC02
  LDA FAC2-1,X    ; get byte from FAC2,X
  STA FAC1-1,X    ; save byte at FAC1,X
  DEX       ; decrement count
  BNE LAB_DC02    ; loop if not all done

  STX FACOV     ; clear FAC1 rounding byte
  RTS


;***********************************************************************************;
;
; round and copy FAC1 to FAC2

RFTOA
  JSR ROUND     ; round FAC1

; copy FAC1 to FAC2

FTOA
  LDX #$06      ; 6 bytes to copy
LAB_DC11
  LDA FAC1-1,X    ; get byte from FAC1,X
  STA FAC2-1,X    ; save byte at FAC2,X
  DEX       ; decrement count
  BNE LAB_DC11    ; loop if not all done

  STX FACOV     ; clear FAC1 rounding byte
LAB_DC1A
  RTS


;***********************************************************************************;
;
; round FAC1

ROUND
  LDA FAC1+FAC_EXPT   ; get FAC1 exponent
  BEQ LAB_DC1A    ; exit if zero

  ASL FACOV     ; shift FAC1 rounding byte
  BCC LAB_DC1A    ; exit if no overflow

; round FAC1 (no check)

LAB_DC23
  JSR LAB_D96F    ; increment FAC1 mantissa
  BNE LAB_DC1A    ; branch if no overflow

  JMP LAB_D938    ; normalise FAC1 for Cb=1 and return

; get FAC1 sign
; return .A = $FF, Cb = 1/-ve .A = $01, Cb = 0/+ve, .A = $00, Cb = ?/0

SGNFAC
  LDA FAC1+FAC_EXPT   ; get FAC1 exponent
  BEQ LAB_DC38    ; exit if zero (already correct SGN(0)=0)

; return .A = $FF, Cb = 1/-ve .A = $01, Cb = 0/+ve
; no = 0 check

LAB_DC2F
  LDA FAC1+FAC_SIGN   ; else get FAC1 sign (b7)

; return .A = $FF, Cb = 1/-ve .A = $01, Cb = 0/+ve
; no = 0 check, sign in .A

LAB_DC31
  ROL       ; move sign bit to carry
  LDA #$FF      ; set byte for -ve result
  BCS LAB_DC38    ; return if sign was set (-ve)

  LDA #$01      ; else set byte for +ve result
LAB_DC38
  RTS


;***********************************************************************************;
;
; perform SGN()

SGN
  JSR SGNFAC      ; get FAC1 sign, return .A = $FF -ve, .A = $01 +ve

; save .A as integer byte

INTFP
  STA FAC1+FAC_MANT   ; save FAC1 mantissa 1
  LDA #$00      ; clear A
  STA FAC1+FAC_MANT+1   ; clear FAC1 mantissa 2
  LDX #$88      ; set exponent

; set exponent = .X, clear FAC1 3 and 4 and normalise

INTFP1
  LDA FAC1+FAC_MANT   ; get FAC1 mantissa 1
  EOR #$FF      ; complement it
  ROL       ; sign bit into carry

; set exponent = .X, clear mantissa 4 and 3 and normalise FAC1

LAB_DC49
  LDA #$00      ; clear .A
  STA FAC1+FAC_MANT+3   ; clear FAC1 mantissa 4
  STA FAC1+FAC_MANT+2   ; clear FAC1 mantissa 3

; set exponent = .X and normalise FAC1

LAB_DC4F
  STX FAC1+FAC_EXPT   ; set FAC1 exponent
  STA FACOV     ; clear FAC1 rounding byte
  STA FAC1+FAC_SIGN   ; clear FAC1 sign (b7)
  JMP LAB_D8D2    ; do ABS and normalise FAC1

; perform ABS()

ABS
  LSR FAC1+FAC_SIGN   ; clear FAC1 sign, put zero in b7
  RTS


;***********************************************************************************;
;
; compare FAC1 with (.A.Y)
; returns .A=$00 if FAC1 = (.A.Y)
; returns .A=$01 if FAC1 > (.A.Y)
; returns .A=$FF if FAC1 < (.A.Y)

CMPFAC
  STA INDEX+2     ; save pointer low byte
LAB_DC5D
  STY INDEX+3     ; save pointer high byte
  LDY #$00      ; clear index
  LDA (INDEX+2),Y   ; get exponent
  INY       ; increment index
  TAX       ; copy (.A.Y) exponent to .X
  BEQ SGNFAC      ; branch if (.A.Y) exponent=0 and get FAC1 sign
          ; .A = $FF, Cb = 1/-ve .A = $01, Cb = 0/+ve

  LDA (INDEX+2),Y   ; get (.A.Y) mantissa 1, with sign
  EOR FAC1+FAC_SIGN   ; XOR FAC1 sign (b7)
  BMI LAB_DC2F    ; if signs <> do return .A = $FF, Cb = 1/-ve
          ; .A = $01, Cb = 0/+ve and return

  CPX FAC1+FAC_EXPT   ; compare (.A.Y) exponent with FAC1 exponent
  BNE LAB_DC92    ; branch if different

  LDA (INDEX+2),Y   ; get (.A.Y) mantissa 1, with sign
  ORA #$80      ; normalise top bit
  CMP FAC1+FAC_MANT   ; compare with FAC1 mantissa 1
  BNE LAB_DC92    ; branch if different

  INY       ; increment index
  LDA (INDEX+2),Y   ; get mantissa 2
  CMP FAC1+FAC_MANT+1   ; compare with FAC1 mantissa 2
  BNE LAB_DC92    ; branch if different

  INY       ; increment index
  LDA (INDEX+2),Y   ; get mantissa 3
  CMP FAC1+FAC_MANT+2   ; compare with FAC1 mantissa 3
  BNE LAB_DC92    ; branch if different

  INY       ; increment index
  LDA #$7F      ; set for 1/2 value rounding byte
  CMP FACOV     ; compare with FAC1 rounding byte (set carry)
  LDA (INDEX+2),Y   ; get mantissa 4
  SBC FAC1+FAC_MANT+3   ; subtract FAC1 mantissa 4
  BEQ LAB_DCBA    ; exit if mantissa 4 equal

; gets here if number <> FAC1

LAB_DC92
  LDA FAC1+FAC_SIGN   ; get FAC1 sign (b7)
  BCC LAB_DC98    ; branch if FAC1 > (.A.Y)

  EOR #$FF      ; else toggle FAC1 sign
LAB_DC98
  JMP LAB_DC31    ; return .A = $FF, Cb = 1/-ve .A = $01, Cb = 0/+ve


;***********************************************************************************;
;
; convert FAC1 floating to fixed

FPINT
  LDA FAC1+FAC_EXPT   ; get FAC1 exponent
  BEQ FILFAC      ; if zero go clear FAC1 and return

  SEC       ; set carry for subtract
  SBC #$A0      ; subtract maximum integer range exponent
  BIT FAC1+FAC_SIGN   ; test FAC1 sign (b7)
  BPL LAB_DCAF    ; branch if FAC1 +ve

          ; FAC1 was -ve
  TAX       ; copy subtracted exponent
  LDA #$FF      ; overflow for -ve number
  STA BITS      ; set FAC1 overflow byte
  JSR LAB_D94D    ; two's complement FAC1 mantissa
  TXA       ; restore subtracted exponent
LAB_DCAF
  LDX #FAC1     ; set index to FAC1
  CMP #$F9      ; compare exponent result
  BPL LAB_DCBB    ; if < 8 shifts shift FAC1 .A times right and return

  JSR LAB_D999    ; shift FAC1 .A times right (> 8 shifts)
  STY BITS      ; clear FAC1 overflow byte
LAB_DCBA
  RTS


;***********************************************************************************;
;
; shift FAC1 .A times right

LAB_DCBB
  TAY       ; copy shift count
  LDA FAC1+FAC_SIGN   ; get FAC1 sign (b7)
  AND #$80      ; mask sign bit only (x000 0000)
  LSR FAC1+FAC_MANT   ; shift FAC1 mantissa 1
  ORA FAC1+FAC_MANT   ; OR sign in b7 FAC1 mantissa 1
  STA FAC1+FAC_MANT   ; save FAC1 mantissa 1
  JSR LAB_D9B0    ; shift FAC1 .Y times right
  STY BITS      ; clear FAC1 overflow byte
  RTS


;***********************************************************************************;
;
; perform INT()

INT
  LDA FAC1+FAC_EXPT   ; get FAC1 exponent
  CMP #$A0      ; compare with max int
  BCS LAB_DCF2    ; exit if >= (already int, too big for fractional part!)

  JSR FPINT     ; convert FAC1 floating to fixed
  STY FACOV     ; save FAC1 rounding byte
  LDA FAC1+FAC_SIGN   ; get FAC1 sign (b7)
  STY FAC1+FAC_SIGN   ; save FAC1 sign (b7)
  EOR #$80      ; toggle FAC1 sign
  ROL       ; shift into carry
  LDA #$A0      ; set new exponent
  STA FAC1+FAC_EXPT   ; save FAC1 exponent
  LDA FAC1+FAC_MANT+3   ; get FAC1 mantissa 4
  STA CHARAC      ; save FAC1 mantissa 4 for power function
  JMP LAB_D8D2    ; do ABS and normalise FAC1


;***********************************************************************************;
;
; clear FAC1 and return

FILFAC
  STA FAC1+FAC_MANT   ; clear FAC1 mantissa 1
  STA FAC1+FAC_MANT+1   ; clear FAC1 mantissa 2
  STA FAC1+FAC_MANT+2   ; clear FAC1 mantissa 3
  STA FAC1+FAC_MANT+3   ; clear FAC1 mantissa 4
  TAY       ; clear .Y
LAB_DCF2
  RTS


;***********************************************************************************;
;
; get FAC1 from string

ASCFLT
  LDY #$00      ; clear .Y
  LDX #$0A      ; set index
LAB_DCF7
  STY LAB_5D,X    ; clear byte
  DEX       ; decrement index
  BPL LAB_DCF7    ; loop until numexp to negnum (and FAC1) = $00

  BCC LAB_DD0D    ; branch if first character is numeric

  CMP #'-'      ; else compare with "-"
  BNE LAB_DD06    ; branch if not "-"

  STX SGNFLG      ; set flag for -ve n (negnum = $FF)
  BEQ LAB_DD0A    ; branch always

LAB_DD06
  CMP #'+'      ; else compare with "+"
  BNE LAB_DD0F    ; branch if not "+"

LAB_DD0A
  JSR CHRGET      ; increment and scan memory
LAB_DD0D
  BCC LAB_DD6A    ; branch if numeric character

LAB_DD0F
  CMP #'.'      ; else compare with "."
  BEQ LAB_DD41    ; branch if "."

  CMP #'E'      ; else compare with "E"
  BNE LAB_DD47    ; branch if not "E"

          ; was "E" so evaluate exponential part
  JSR CHRGET      ; increment and scan memory
  BCC LAB_DD33    ; branch if numeric character

  CMP #TK_MINUS   ; else compare with token for "-"
  BEQ LAB_DD2E    ; branch if token for "-"

  CMP #'-'      ; else compare with "-"
  BEQ LAB_DD2E    ; branch if "-"

  CMP #TK_PLUS    ; else compare with token for "+"
  BEQ LAB_DD30    ; branch if token for "+"

  CMP #'+'      ; else compare with "+"
  BEQ LAB_DD30    ; branch if "+"

  BNE LAB_DD35    ; branch always

LAB_DD2E
  ROR TMPPTR+1    ; set exponent -ve flag (C, which=1, into b7)
LAB_DD30
  JSR CHRGET      ; increment and scan memory
LAB_DD33
  BCC LAB_DD91    ; branch if numeric character

LAB_DD35
  BIT TMPPTR+1    ; test exponent -ve flag
  BPL LAB_DD47    ; if +ve go evaluate exponent

          ; else do exponent = -exponent
  LDA #$00      ; clear result
  SEC       ; set carry for subtract
  SBC EXPCNT      ; subtract exponent byte
  JMP LAB_DD49    ; go evaluate exponent

LAB_DD41
  ROR TMPPTR      ; set decimal point flag
  BIT TMPPTR      ; test decimal point flag
  BVC LAB_DD0A    ; branch if only one decimal point so far

          ; evaluate exponent
LAB_DD47
  LDA EXPCNT      ; get exponent count byte
LAB_DD49
  SEC       ; set carry for subtract
  SBC LAB_5D      ; subtract numerator exponent
  STA EXPCNT      ; save exponent count byte
  BEQ LAB_DD62    ; branch if no adjustment

  BPL LAB_DD5B    ; else if +ve go do FAC1*10^expcnt

          ; else go do FAC1/10^(0-expcnt)
LAB_DD52
  JSR DIVTEN      ; divide FAC1 by 10
  INC EXPCNT      ; increment exponent count byte
  BNE LAB_DD52    ; loop until all done

  BEQ LAB_DD62    ; branch always

LAB_DD5B
  JSR MULTEN      ; multiply FAC1 by 10
  DEC EXPCNT      ; decrement exponent count byte
  BNE LAB_DD5B    ; loop until all done

LAB_DD62
  LDA SGNFLG      ; get -ve flag
  BMI LAB_DD67    ; if -ve do - FAC1 and return

  RTS

; do - FAC1 and return

LAB_DD67
  JMP NEGFAC      ; do - FAC1

; do unsigned FAC1*10+number

LAB_DD6A
  PHA       ; save character
  BIT TMPPTR      ; test decimal point flag
  BPL LAB_DD71    ; skip exponent increment if not set

  INC LAB_5D      ; else increment number exponent
LAB_DD71
  JSR MULTEN      ; multiply FAC1 by 10
  PLA       ; restore character
  SEC       ; set carry for subtract
  SBC #'0'      ; convert to binary
  JSR ASCI8     ; evaluate new ASCII digit
  JMP LAB_DD0A    ; go do next character

; evaluate new ASCII digit
; multiply FAC1 by 10 then (ABS) add in new digit

ASCI8
  PHA       ; save digit
  JSR RFTOA     ; round and copy FAC1 to FAC2
  PLA       ; restore digit
  JSR INTFP     ; save .A as integer byte
  LDA FAC2+FAC_SIGN   ; get FAC2 sign (b7)
  EOR FAC1+FAC_SIGN   ; toggle with FAC1 sign (b7)
  STA ARISGN      ; save sign compare (FAC1 XOR FAC2)
  LDX FAC1+FAC_EXPT   ; get FAC1 exponent
  JMP PLUS      ; add FAC2 to FAC1 and return

; evaluate next character of exponential part of number

LAB_DD91
  LDA EXPCNT      ; get exponent count byte
  CMP #$0A      ; compare with 10 decimal
  BCC LAB_DDA0    ; branch if less

  LDA #$64      ; make all -ve exponents = -100 decimal (causes underflow)
  BIT TMPPTR+1    ; test exponent -ve flag
  BMI LAB_DDAE    ; branch if -ve

  JMP OVERFL      ; else do overflow error then warm start

LAB_DDA0
  ASL       ; *2
  ASL       ; *4
  CLC       ; clear carry for add
  ADC EXPCNT      ; *5
  ASL       ; *10
  CLC       ; clear carry for add
  LDY #$00      ; set index
  ADC (CHRGOT+1),Y    ; add character (will be $30 too much!)
  SEC       ; set carry for subtract
  SBC #'0'      ; convert character to binary
LAB_DDAE
  STA EXPCNT      ; save exponent count byte
  JMP LAB_DD30    ; go get next character


;***********************************************************************************;
;
FPC12
  .byte $9B,$3E,$BC,$1F,$FD
          ; 99999999.90625, maximum value with at least one decimal
LAB_DDB8
  .byte $9E,$6E,$6B,$27,$FD
          ; 999999999.25, maximum value before scientific notation
LAB_DDBD
  .byte $9E,$6E,$6B,$28,$00
          ; 1000000000


;***********************************************************************************;
;
; do " IN " line number message

PRTIN
  LDA #<INSTR     ; set " IN " pointer low byte
  LDY #>INSTR     ; set " IN " pointer high byte
  JSR LAB_DDDA    ; print null terminated string
  LDA CURLIN+1    ; get the current line number high byte
  LDX CURLIN      ; get the current line number low byte


;***********************************************************************************;
;
; print .X.A as unsigned integer

PRTFIX
  STA FAC1+FAC_MANT   ; save high byte as FAC1 mantissa 1
  STX FAC1+FAC_MANT+1   ; save low byte as FAC1 mantissa 2
  LDX #$90      ; set exponent to 16d bits
  SEC       ; set integer is +ve flag
  JSR LAB_DC49    ; set exponent = .X, clear mantissa 4 and 3 and normalise
          ; FAC1
  JSR LAB_DDDF    ; convert FAC1 to string
LAB_DDDA
  JMP PRTSTR      ; print null terminated string


;***********************************************************************************;
;
; convert FAC1 to ASCII string result in (.A.Y)

FLTASC
  LDY #$01      ; set index = 1
LAB_DDDF
  LDA #' '      ; character = " " (assume +ve)
  BIT FAC1+FAC_SIGN   ; test FAC1 sign (b7)
  BPL LAB_DDE7    ; if +ve skip the - sign set

  LDA #'-'      ; else character = "-"
LAB_DDE7
  STA BASZPT,Y    ; save leading character (" " or "-")
  STA FAC1+FAC_SIGN   ; save FAC1 sign (b7)
  STY FBUFPT      ; save the index
  INY       ; increment index
  LDA #'0'      ; set character = "0"
  LDX FAC1+FAC_EXPT   ; get FAC1 exponent
  BNE LAB_DDF8    ; if FAC1<>0 go convert it

          ; exponent was $00 so FAC1 is 0
  JMP LAB_DF04    ; save last character, [EOT] and exit

; FAC1 is some non zero value

LAB_DDF8
  LDA #$00      ; clear (number exponent count)
  CPX #$80      ; compare FAC1 exponent with $80 (<1.00000)
  BEQ LAB_DE00    ; branch if 0.5 <= FAC1 < 1.0

  BCS LAB_DE09    ; branch if FAC1=>1

LAB_DE00
  LDA #<LAB_DDBD    ; set 1000000000 pointer low byte
  LDY #>LAB_DDBD    ; set 1000000000 pointer high byte
  JSR TIMES     ; do convert .A.Y, FAC1*(.A.Y)
  LDA #$F7      ; set number exponent count
LAB_DE09
  STA LAB_5D      ; save number exponent count
LAB_DE0B
  LDA #<LAB_DDB8    ; set 999999999.25 pointer low byte (max before sci note)
  LDY #>LAB_DDB8    ; set 999999999.25 pointer high byte
  JSR CMPFAC      ; compare FAC1 with (.A.Y)
  BEQ LAB_DE32    ; exit if FAC1 = (.A.Y)

  BPL LAB_DE28    ; go do /10 if FAC1 > (.A.Y)

          ; FAC1 < (.A.Y)
LAB_DE16
  LDA #<FPC12     ; set 99999999.90625 pointer low byte
  LDY #>FPC12     ; set 99999999.90625 pointer high byte
  JSR CMPFAC      ; compare FAC1 with (.A.Y)
  BEQ LAB_DE21    ; branch if FAC1 = (.A.Y) (allow decimal places)

  BPL LAB_DE2F    ; branch if FAC1 > (.A.Y) (no decimal places)

          ; FAC1 <= (.A.Y)
LAB_DE21
  JSR MULTEN      ; multiply FAC1 by 10
  DEC LAB_5D      ; decrement number exponent count
  BNE LAB_DE16    ; go test again, branch always

LAB_DE28
  JSR DIVTEN      ; divide FAC1 by 10
  INC LAB_5D      ; increment number exponent count
  BNE LAB_DE0B    ; go test again, branch always

; now we have just the digits to do

LAB_DE2F
  JSR ADD05     ; add 0.5 to FAC1 (round FAC1)
LAB_DE32
  JSR FPINT     ; convert FAC1 floating to fixed
  LDX #$01      ; set default digits before dp = 1
  LDA LAB_5D      ; get number exponent count
  CLC       ; clear carry for add
  ADC #$0A      ; up to 9 digits before point
  BMI LAB_DE47    ; if -ve then 1 digit before dp

  CMP #$0B      ; .A>=$0B if n>=1E9
  BCS LAB_DE48    ; branch if >= $0B

          ; carry is clear
  ADC #$FF      ; take 1 from digit count
  TAX       ; copy to .X
  LDA #$02      ; set the exponent adjust
LAB_DE47
  SEC       ; set carry for subtract
LAB_DE48
  SBC #$02      ; -2
  STA EXPCNT      ; save the exponent adjust
  STX LAB_5D      ; save digits before dp count
  TXA       ; copy digits before dp count to .A
  BEQ LAB_DE53    ; if no digits before the dp go do the "."

  BPL LAB_DE66    ; if there are digits before the dp go do them

LAB_DE53
  LDY FBUFPT      ; get the output string index
  LDA #'.'      ; character "."
  INY       ; increment the index
  STA STACK-1,Y   ; save the "." to the output string
  TXA       ; copy digits before dp count to .A
  BEQ LAB_DE64    ; if no digits before the dp skip the "0"

  LDA #'0'      ; character "0"
  INY       ; increment index
  STA STACK-1,Y   ; save the "0" to the output string
LAB_DE64
  STY FBUFPT      ; save the output string index
LAB_DE66
  LDY #$00      ; clear the powers of 10 index (point to -100,000,000)
LAB_DE68
  LDX #$80      ; clear the digit, set the test sense
LAB_DE6A
  LDA FAC1+FAC_MANT+3   ; get FAC1 mantissa 4
  CLC       ; clear carry for add
  ADC FLTCON+3,Y    ; add byte 4, least significant
  STA FAC1+FAC_MANT+3   ; save FAC1 mantissa 4
  LDA FAC1+FAC_MANT+2   ; get FAC1 mantissa 3
  ADC FLTCON+2,Y    ; add byte 3
  STA FAC1+FAC_MANT+2   ; save FAC1 mantissa 3
  LDA FAC1+FAC_MANT+1   ; get FAC1 mantissa 2
  ADC FLTCON+1,Y    ; add byte 2
  STA FAC1+FAC_MANT+1   ; save FAC1 mantissa 2
  LDA FAC1+FAC_MANT   ; get FAC1 mantissa 1
  ADC FLTCON+0,Y    ; add byte 1, most significant
  STA FAC1+FAC_MANT   ; save FAC1 mantissa 1
  INX       ; increment the digit, set the sign on the test sense bit
  BCS LAB_DE8E    ; if the carry is set go test if the result was positive

          ; else the result needs to be negative
  BPL LAB_DE6A    ; not -ve so try again

  BMI LAB_DE90    ; else done so return the digit

LAB_DE8E
  BMI LAB_DE6A    ; not +ve so try again

; else done so return the digit

LAB_DE90
  TXA       ; copy the digit
  BCC LAB_DE97    ; if Cb=0 just use it

  EOR #$FF      ; else make the two's complement ..
  ADC #$0A      ; .. and subtract it from 10
LAB_DE97
  ADC #'0'-1      ; add "0"-1 to result
  INY       ; increment ..
  INY       ; .. index to..
  INY       ; .. next less ..
  INY       ; .. power of ten
  STY VARPNT      ; save the powers of ten table index
  LDY FBUFPT      ; get output string index
  INY       ; increment output string index
  TAX       ; copy character to .X
  AND #$7F      ; mask out top bit
  STA STACK-1,Y   ; save to output string
  DEC LAB_5D      ; decrement # of characters before the dp
  BNE LAB_DEB2    ; if still characters to do skip the decimal point

          ; else output the point
  LDA #'.'      ; character "."
  INY       ; increment output string index
  STA STACK-1,Y   ; save to output string
LAB_DEB2
  STY FBUFPT      ; save the output string index
  LDY VARPNT      ; get the powers of ten table index
  TXA       ; get the character back
  EOR #$FF      ; toggle the test sense bit
  AND #$80      ; clear the digit
  TAX       ; copy it to the new digit
  CPY #HMSCON-FLTCON
          ; compare the table index with the max for decimal numbers
  BEQ LAB_DEC4    ; if at the max exit the digit loop

  CPY #LAB_DF52-FLTCON
          ; compare the table index with the max for time
  BNE LAB_DE6A    ; loop if not at the max

; now remove trailing zeroes

LAB_DEC4
  LDY FBUFPT      ; restore the output string index
LAB_DEC6
  LDA STACK-1,Y   ; get character from output string
  DEY       ; decrement output string index
  CMP #'0'      ; compare with "0"
  BEQ LAB_DEC6    ; loop until non "0" character found

  CMP #'.'      ; compare with "."
  BEQ LAB_DED3    ; branch if was dp

          ; restore last character
  INY       ; increment output string index
LAB_DED3
  LDA #'+'      ; character "+"
  LDX EXPCNT      ; get exponent count
  BEQ LAB_DF07    ; if zero go set null terminator and exit

          ; exponent isn't zero so write exponent
  BPL LAB_DEE3    ; branch if exponent count +ve

  LDA #$00      ; clear .A
  SEC       ; set carry for subtract
  SBC EXPCNT      ; subtract exponent count adjust (convert -ve to +ve)
  TAX       ; copy exponent count to .X
  LDA #'-'      ; character "-"
LAB_DEE3
  STA STACK+1,Y   ; save to output string
  LDA #'E'      ; character "E"
  STA STACK,Y     ; save exponent sign to output string
  TXA       ; get exponent count back
  LDX #$2F      ; one less than "0" character
  SEC       ; set carry for subtract
LAB_DEEF
  INX       ; increment 10's character
  SBC #$0A      ; subtract 10 from exponent count
  BCS LAB_DEEF    ; loop while still >= 0

  ADC #':'      ; add character ":" ($30+$0A, result is 10 less that value)
  STA STACK+3,Y   ; save to output string
  TXA       ; copy 10's character
  STA STACK+2,Y   ; save to output string
  LDA #$00      ; set null terminator
  STA STACK+4,Y   ; save to output string
  BEQ LAB_DF0C    ; go set string pointer (.A.Y) and exit, branch always

          ; save last character, [EOT] and exit
LAB_DF04
  STA STACK-1,Y   ; save last character to output string

          ; set null terminator and exit
LAB_DF07
  LDA #$00      ; set null terminator
  STA STACK,Y     ; save after last character

          ; set string pointer (.A.Y) and exit
LAB_DF0C
  LDA #<STACK     ; set result string pointer low byte
  LDY #>STACK     ; set result string pointer high byte
  RTS


;***********************************************************************************;
;

FLP05
  .byte $80,$00     ; 0.5, first two bytes
NULLVAR
  .byte $00,$00,$00   ; null return for undefined variables

; decimal conversion tables

FLTCON
  .byte $FA,$0A,$1F,$00 ; -100000000
  .byte $00,$98,$96,$80 ;  +10000000
  .byte $FF,$F0,$BD,$C0 ;   -1000000
  .byte $00,$01,$86,$A0 ;    +100000
  .byte $FF,$FF,$D8,$F0 ;     -10000
  .byte $00,$00,$03,$E8 ;      +1000
  .byte $FF,$FF,$FF,$9C ; -100
  .byte $00,$00,$00,$0A ;  +10
  .byte $FF,$FF,$FF,$FF ;   -1

; jiffy count conversion table

HMSCON
  .byte $FF,$DF,$0A,$80 ; -2160000  10s hours
  .byte $00,$03,$4B,$C0 ;  +216000      hours
  .byte $FF,$FF,$73,$60 ;   -36000  10s mins
  .byte $00,$00,$0E,$10 ;    +3600      mins
  .byte $FF,$FF,$FD,$A8 ;     -600  10s secs
  .byte $00,$00,$00,$3C ;      +60      secs
LAB_DF52


;***********************************************************************************;
;
; spare bytes, not referenced

  .byte $BF,$AA,$AA,$AA,$AA,$AA,$AA,$AA,$AA,$AA,$AA,$AA,$AA,$AA,$AA,$AA
  .byte $AA,$AA,$AA,$AA,$AA,$AA,$AA,$AA,$AA,$AA,$AA,$AA,$AA,$AA,$AA


;***********************************************************************************;
;
; perform SQR()

SQR
  JSR RFTOA     ; round and copy FAC1 to FAC2
  LDA #<FLP05     ; set 0.5 pointer low address
  LDY #>FLP05     ; set 0.5 pointer high address
  JSR LODFAC      ; unpack memory (.A.Y) into FAC1


;***********************************************************************************;
;
; perform power function

EXPONT
  BEQ EXP     ; perform EXP()

  LDA FAC2+FAC_EXPT   ; get FAC2 exponent
  BNE LAB_DF84    ; branch if FAC2<>0

  JMP LAB_D8F9    ; clear FAC1 exponent and sign and return

LAB_DF84
  LDX #<DEFPNT    ; set destination pointer low byte
  LDY #>DEFPNT    ; set destination pointer high byte
  JSR STORFAC     ; pack FAC1 into (.X.Y)
  LDA FAC2+FAC_SIGN   ; get FAC2 sign (b7)
  BPL LAB_DF9E    ; branch if FAC2>0

          ; else FAC2 is -ve and can only be raised to an
          ; integer power which gives an x + j0 result
  JSR INT     ; perform INT()
  LDA #<DEFPNT    ; set source pointer low byte
  LDY #>DEFPNT    ; set source pointer high byte
  JSR CMPFAC      ; compare FAC1 with (.A.Y)
  BNE LAB_DF9E    ; branch if FAC1 <> (.A.Y) to allow Function Call error
          ; this will leave FAC1 -ve and cause a Function Call
          ; error when LOG() is called

  TYA       ; clear sign b7
  LDY CHARAC      ; get FAC1 mantissa 4 from INT() function as sign in
          ; .Y for possible later negation, b0 only needed
LAB_DF9E
  JSR LAB_DBFE    ; save FAC1 sign and copy ABS(FAC2) to FAC1
  TYA       ; copy sign back ..
  PHA       ; .. and save it
  JSR LOG     ; perform LOG()
  LDA #<DEFPNT    ; set pointer low byte
  LDY #>DEFPNT    ; set pointer high byte
  JSR TIMES     ; do convert .A.Y, FAC1*(.A.Y)
  JSR EXP     ; perform EXP()
  PLA       ; pull sign from stack
  LSR       ; b0 is to be tested
  BCC LAB_DFBE    ; if no bit then exit

; do - FAC1

NEGFAC
  LDA FAC1+FAC_EXPT   ; get FAC1 exponent
  BEQ LAB_DFBE    ; exit if FAC1_e = $00

  LDA FAC1+FAC_SIGN   ; get FAC1 sign (b7)
  EOR #$FF      ; complement it
  STA FAC1+FAC_SIGN   ; save FAC1 sign (b7)
LAB_DFBE
  RTS


;***********************************************************************************;
;
; exp(n) constant and series

EXPCON
  .byte $81,$38,$AA,$3B,$29 ; 1.443

LAB_DFC4
  .byte $07     ; series count
  .byte $71,$34,$58,$3E,$56 ; 2.14987637E-5
  .byte $74,$16,$7E,$B3,$1B ; 1.43523140E-4
  .byte $77,$2F,$EE,$E3,$85 ; 1.34226348E-3
  .byte $7A,$1D,$84,$1C,$2A ; 9.61401701E-3
  .byte $7C,$63,$59,$58,$0A ; 5.55051269E-2
  .byte $7E,$75,$FD,$E7,$C6 ; 2.40226385E-1
  .byte $80,$31,$72,$18,$10 ; 6.93147186E-1
  .byte $81,$00,$00,$00,$00 ; 1.00000000


;***********************************************************************************;
;
; perform EXP()

EXP
  LDA #<EXPCON    ; set 1.443 pointer low byte
  LDY #>EXPCON    ; set 1.443 pointer high byte
  JSR TIMES     ; do convert .A.Y, FAC1*(.A.Y)
  LDA FACOV     ; get FAC1 rounding byte
  ADC #$50      ; +$50/$100
  BCC LAB_DFFD    ; skip rounding if no carry

  JSR LAB_DC23    ; round FAC1 (no check)
LAB_DFFD
  STA JMPER+2     ; save FAC2 rounding byte
  JSR FTOA      ; copy FAC1 to FAC2
  LDA FAC1+FAC_EXPT   ; get FAC1 exponent
  CMP #$88      ; compare with EXP limit (256)
  BCC LAB_E00B    ; branch if less

LAB_E008
  JSR LAB_DAD4    ; handle overflow and underflow
LAB_E00B
  JSR INT     ; perform INT()
  LDA CHARAC      ; get mantissa 4 from INT()
  CLC       ; clear carry for add
  ADC #$81      ; normalise +1
  BEQ LAB_E008    ; if $00 result has overflowed so go handle it

  SEC       ; set carry for subtract
  SBC #$01      ; exponent now correct
  PHA       ; save FAC2 exponent
          ; swap FAC1 and FAC2
  LDX #$05      ; 4 bytes to do
LAB_E01B
  LDA FAC2,X      ; get FAC2,X
  LDY FAC1,X      ; get FAC1,X
  STA FAC1,X      ; save FAC1,X
  STY FAC2,X      ; save FAC2,X
  DEX       ; decrement count/index
  BPL LAB_E01B    ; loop if not all done

  LDA JMPER+2     ; get FAC2 rounding byte
  STA FACOV     ; save as FAC1 rounding byte
  JSR SUB     ; perform subtraction, FAC2 from FAC1
  JSR NEGFAC      ; do - FAC1
  LDA #<LAB_DFC4    ; set counter pointer low byte
  LDY #>LAB_DFC4    ; set counter pointer high byte
  JSR SER2      ; go do series evaluation
  LDA #$00      ; clear .A
  STA ARISGN      ; clear sign compare (FAC1 XOR FAC2)
  PLA       ; pull the saved FAC2 exponent
  JSR LAB_DAB9    ; test and adjust accumulators
  RTS


;***********************************************************************************;
;
; ^2 then series evaluation

SEREVL
  STA FBUFPT      ; save count pointer low byte
  STY FBUFPT+1    ; save count pointer high byte
  JSR FACTF1      ; pack FAC1 into LAB_57
  LDA #<TEMPF3    ; set pointer low byte (.Y already $00)
  JSR TIMES     ; do convert .A.Y, FAC1*(.A.Y)
  JSR LAB_E05A    ; go do series evaluation
  LDA #<TEMPF3    ; pointer to original # low byte
  LDY #>TEMPF3    ; pointer to original # high byte
  JMP TIMES     ; do convert .A.Y, FAC1*(.A.Y)


;***********************************************************************************;
;
; do series evaluation

SER2
  STA FBUFPT      ; save count pointer low byte
  STY FBUFPT+1    ; save count pointer high byte

; do series evaluation

LAB_E05A
  JSR FACTF2      ; pack FAC1 into LAB_5C
  LDA (FBUFPT),Y    ; get constants count
  STA SGNFLG      ; save constants count
  LDY FBUFPT      ; get count pointer low byte
  INY       ; increment it (now constants pointer)
  TYA       ; copy it
  BNE LAB_E069    ; skip next if no overflow

  INC FBUFPT+1    ; else increment high byte
LAB_E069
  STA FBUFPT      ; save low byte
  LDY FBUFPT+1    ; get high byte
LAB_E06D
  JSR TIMES     ; do convert .A.Y, FAC1*(.A.Y)
  LDA FBUFPT      ; get constants pointer low byte
  LDY FBUFPT+1    ; get constants pointer high byte
  CLC       ; clear carry for add
  ADC #$05      ; +5 to low pointer (5 bytes per constant)
  BCC LAB_E07A    ; skip next if no overflow

  INY       ; increment high byte
LAB_E07A
  STA FBUFPT      ; save pointer low byte
  STY FBUFPT+1    ; save pointer high byte
  JSR LAPLUS      ; add (.A.Y) to FAC1
  LDA #<LAB_5C    ; set pointer low byte to partial
  LDY #>LAB_5C    ; set pointer high byte to partial
  DEC SGNFLG      ; decrement constants count
  BNE LAB_E06D    ; loop until all done

  RTS


;***********************************************************************************;
;
; RND values

RNDC1
  .byte $98,$35,$44,$7A,$00
          ; 11879546      multiplier
LAB_E08F
  .byte $68,$28,$B1,$46,$00
          ; 3.927677739E-8    offset


;***********************************************************************************;
;
; perform RND()

RND
  JSR SGNFAC      ; get FAC1 sign
          ; return .A = $FF -ve, .A = $01 +ve
  BMI LAB_E0D0    ; if n<0 copy byte swapped FAC1 into RND() seed

  BNE LAB_E0BB    ; if n>0 get next number in RND() sequence

          ; else n=0 so get the RND() number from VIA 1 timers
  JSR IOBASE      ; return base address of I/O devices
  STX INDEX     ; save pointer low byte
  STY INDEX+1     ; save pointer high byte
  LDY #$04      ; set index to T1 low byte
  LDA (INDEX),Y   ; get T1 low byte
  STA FAC1+FAC_MANT   ; save FAC1 mantissa 1
  INY       ; increment index
  LDA (INDEX),Y   ; get T1 high byte
  STA FAC1+FAC_MANT+2   ; save FAC1 mantissa 3
  LDY #$08      ; set index to T2 low byte
  LDA (INDEX),Y   ; get T2 low byte
  STA FAC1+FAC_MANT+1   ; save FAC1 mantissa 2
  INY       ; increment index
  LDA (INDEX),Y   ; get T2 high byte
  STA FAC1+FAC_MANT+3   ; save FAC1 mantissa 4
  JMP LAB_E0E0    ; set exponent and exit

LAB_E0BB
  LDA #<RNDX      ; set seed pointer low address
  LDY #>RNDX      ; set seed pointer high address
  JSR LODFAC      ; unpack memory (.A.Y) into FAC1
  LDA #<RNDC1     ; set 11879546 pointer low byte
  LDY #>RNDC1     ; set 11879546 pointer high byte
  JSR TIMES     ; do convert .A.Y, FAC1*(.A.Y)
  LDA #<LAB_E08F    ; set 3.927677739E-8 pointer low byte
  LDY #>LAB_E08F    ; set 3.927677739E-8 pointer high byte
  JSR LAPLUS      ; add (.A.Y) to FAC1
LAB_E0D0
  LDX FAC1+FAC_MANT+3   ; get FAC1 mantissa 4
  LDA FAC1+FAC_MANT   ; get FAC1 mantissa 1
  STA FAC1+FAC_MANT+3   ; save FAC1 mantissa 4
  STX FAC1+FAC_MANT   ; save FAC1 mantissa 1
  LDX FAC1+FAC_MANT+1   ; get FAC1 mantissa 2
  LDA FAC1+FAC_MANT+2   ; get FAC1 mantissa 3
  STA FAC1+FAC_MANT+1   ; save FAC1 mantissa 2
  STX FAC1+FAC_MANT+2   ; save FAC1 mantissa 3
LAB_E0E0
  LDA #$00      ; clear byte
  STA FAC1+FAC_SIGN   ; clear FAC1 sign (always +ve)
  LDA FAC1+FAC_EXPT   ; get FAC1 exponent
  STA FACOV     ; save FAC1 rounding byte
  LDA #$80      ; set exponent = $80
  STA FAC1+FAC_EXPT   ; save FAC1 exponent
  JSR LAB_D8D7    ; normalise FAC1
  LDX #<RNDX      ; set seed pointer low address
  LDY #>RNDX      ; set seed pointer high address


;***********************************************************************************;
;
; pack FAC1 into (.X.Y)

LAB_E0F3
  JMP STORFAC     ; pack FAC1 into (.X.Y)


;***********************************************************************************;
;
; handle BASIC I/O error

PATCHBAS
  CMP #$F0      ; compare error with $F0
  BNE LAB_E101    ; branch if not $F0

  STY MEMSIZ+1    ; set end of memory high byte
  STX MEMSIZ      ; set end of memory low byte
  JMP LAB_C663    ; clear from start to end and return

          ; error was not $F0
LAB_E101
  TAX       ; copy error #
  BNE LAB_E106    ; branch if not $00

  LDX #ER_BREAK   ; else error $1E, break error
LAB_E106
  JMP ERROR     ; do error #.X then warm start


;***********************************************************************************;
;
; output character to channel with error check

LAB_E109
  JSR CHROUT      ; output character to channel
  BCS PATCHBAS    ; if error go handle BASIC I/O error

  RTS


;***********************************************************************************;
;
; input character from channel with error check

LAB_E10F
  JSR CHRIN     ; input character from channel
  BCS PATCHBAS    ; if error go handle BASIC I/O error

  RTS


;***********************************************************************************;
;
; open channel for output with error check

LAB_E115
  JSR CHKOUT      ; open channel for output
  BCS PATCHBAS    ; if error go handle BASIC I/O error

  RTS


;***********************************************************************************;
;
; open channel for input with error check

LAB_E11B
  JSR CHKIN     ; open channel for input
  BCS PATCHBAS    ; if error go handle BASIC I/O error

  RTS


;***********************************************************************************;
;
; get character from input device with error check

LAB_E121
  JSR GETIN     ; get character from input device
  BCS PATCHBAS    ; if error go handle BASIC I/O error

  RTS


;***********************************************************************************;
;
; perform SYS

SYSTEM
  JSR TYPCHK      ; evaluate expression and check is numeric, else do
          ; type mismatch
  JSR MAKADR      ; convert FAC1 to integer in temporary integer
  LDA #>LAB_E144-1    ; get return address high byte
  PHA       ; push as return address
  LDA #<LAB_E144-1    ; get return address low byte
  PHA       ; push as return address
  LDA SPREG     ; get saved status register
  PHA       ; put on stack
  LDA SAREG     ; get saved .A
  LDX SXREG     ; get saved .X
  LDY SYREG     ; get saved .Y
  PLP       ; pull processor status
  JMP (LINNUM)    ; call SYS address

LAB_E144
  PHP       ; save status
  STA SAREG     ; save returned .A
  STX SXREG     ; save returned .X
  STY SYREG     ; save returned .Y
  PLA       ; restore saved status
  STA SPREG     ; save status
  RTS


;***********************************************************************************;
;
; perform SAVE

BSAVE
  JSR PARSL     ; get parameters for LOAD/SAVE
  LDX VARTAB      ; get start of variables low byte
  LDY VARTAB+1    ; get start of variables high byte
  LDA #TXTTAB     ; index to start of program memory
  JSR SAVE      ; save RAM to device, .A = index to start address, .X.Y = end
          ; address low/high
  BCS PATCHBAS    ; if error go handle BASIC I/O error

  RTS


;***********************************************************************************;
;
; perform VERIFY

BVERIF
  LDA #$01      ; flag verify
  .byte $2C     ; makes next line BIT $00A9


;***********************************************************************************;
;
; perform LOAD

BLOAD
  LDA #$00      ; flag load
  STA VERCHK      ; set load/verify flag
  JSR PARSL     ; get parameters for LOAD/SAVE
  LDA VERCHK      ; get load/verify flag
  LDX TXTTAB      ; get start of memory low byte
  LDY TXTTAB+1    ; get start of memory high byte
  JSR LOAD      ; load RAM from a device
  BCS LAB_E1CE    ; if error go handle BASIC I/O error

  LDA VERCHK      ; get load/verify flag
  BEQ LAB_E195    ; branch if load

  LDX #ER_VERIFY    ; error $1C, verify error
  JSR READST      ; read I/O status word
  AND #$10      ; mask for tape read error
  BEQ LAB_E187    ; branch if no read error

  JMP ERROR     ; do error #.X then warm start

LAB_E187
  LDA CHRGOT+1    ; get BASIC execute pointer low byte

; The above code is wrong. The high byte is in CHRGOT+2, the code should read ..
;
; LDA CHRGOT+2    ; get BASIC execute pointer high byte

  CMP #>BUF     ; compare with input buffer high byte
  BEQ LAB_E194    ; if immediate mode skip "OK" prompt

  LDA #<OKSTR     ; set "OK" pointer low byte
  LDY #>OKSTR     ; set "OK" pointer high byte
  JMP PRTSTR      ; print null terminated string

LAB_E194
  RTS


;***********************************************************************************;
;
; rebuild BASIC line chaining following a LOAD

LAB_E195
  JSR READST      ; read I/O status word
  AND #$BF      ; mask x0xx xxxx, all except EOF
  BEQ LAB_E1A1    ; branch if no errors

  LDX #ER_LOAD    ; error $1D, load error
  JMP ERROR     ; do error #.X then warm start

LAB_E1A1
  LDA CHRGOT+2    ; get BASIC execute pointer high byte
  CMP #>BUF     ; compare with input buffer high byte
  BNE LAB_E1B5    ; branch if not immediate mode

  STX VARTAB      ; set start of variables low byte
  STY VARTAB+1    ; set start of variables high byte
  LDA #<READYSTR    ; set "READY." pointer low byte
  LDY #>READYSTR    ; set "READY." pointer high byte
  JSR PRTSTR      ; print null terminated string
  JMP LAB_C52A    ; reset execution, clear variables, flush stack,
          ; rebuild BASIC chain and do warm start

LAB_E1B5
  JSR STXTPT      ; set BASIC execute pointer to start of memory - 1
  JMP PATCHER     ; rebuild BASIC line chaining, do RESTORE and return


;***********************************************************************************;
;
; perform OPEN

BOPEN
  JSR PAROC     ; get parameters for OPEN/CLOSE
  JSR OPEN      ; open a logical file
  BCS LAB_E1CE    ; branch if error

  RTS


;***********************************************************************************;
;
; perform CLOSE

BCLOSE
  JSR PAROC     ; get parameters for OPEN/CLOSE
  LDA FORPNT      ; get logical file number
  JSR CLOSE     ; close a specified logical file
  BCC LAB_E194    ; exit if no error

LAB_E1CE
  JMP PATCHBAS    ; go handle BASIC I/O error


;***********************************************************************************;
;
; get parameters for LOAD/SAVE

PARSL
  LDA #$00      ; clear file name length
  JSR SETNAM      ; clear filename
  LDX #$01      ; set default device number, cassette
  LDY #$00      ; set default command
  JSR SETLFS      ; set logical, first and second addresses
  JSR IFCHRG      ; exit function if [EOT] or ":"
  JSR LAB_E254    ; set filename
  JSR IFCHRG      ; exit function if [EOT] or ":"
  JSR LAB_E1FD    ; scan and get byte, else do syntax error then warm start
  LDY #$00      ; clear command
  STX FORPNT      ; save device number
  JSR SETLFS      ; set logical, first and second addresses
  JSR IFCHRG      ; exit function if [EOT] or ":"
  JSR LAB_E1FD    ; scan and get byte, else do syntax error then warm start
  TXA       ; copy command to .A
  TAY       ; copy command to .Y
  LDX FORPNT      ; get device number back
  JMP SETLFS      ; set logical, first and second addresses and return


;***********************************************************************************;
;
; scan and get byte, else do syntax error then warm start

LAB_E1FD
  JSR SKPCOM      ; scan for ",byte", else do syntax error then warm start
  JMP LAB_D79E    ; get byte parameter and return


;***********************************************************************************;
;
; exit function if [EOT] or ":"

IFCHRG
  JSR CHRGOT      ; scan memory
  BNE LAB_E20A    ; branch if not [EOL] or ":"

  PLA       ; dump return address low byte
  PLA       ; dump return address high byte
LAB_E20A
  RTS


;***********************************************************************************;
;
; scan for ",valid byte", else do syntax error then warm start

SKPCOM
  JSR COMCHK      ; scan for ",", else do syntax error then warm start

; scan for valid byte, not [EOL] or ":", else do syntax error then warm start

CHRERR
  JSR CHRGOT      ; scan memory
  BNE LAB_E20A    ; exit if following byte

  JMP LAB_CF08    ; else do syntax error then warm start


;***********************************************************************************;
;
; get parameters for OPEN/CLOSE

PAROC
  LDA #$00      ; clear file name length
  JSR SETNAM      ; clear filename
  JSR CHRERR      ; scan for valid byte, else do syntax error then warm start
  JSR LAB_D79E    ; get byte parameter, logical file number
  STX FORPNT      ; save logical file number
  TXA       ; copy logical file number to .A
  LDX #$01      ; set default device number, cassette
  LDY #$00      ; set default command
  JSR SETLFS      ; set logical, first and second addresses
  JSR IFCHRG      ; exit function if [EOT] or ":"
  JSR LAB_E1FD    ; scan and get byte, else do syntax error then warm start
  STX FORPNT+1    ; save device number
  LDY #$00      ; clear command
  LDA FORPNT      ; get logical file number
  CPX #$03      ; compare device number with screen
  BCC LAB_E23C    ; branch if less than screen

  DEY       ; else decrement command
LAB_E23C
  JSR SETLFS      ; set logical, first and second addresses
  JSR IFCHRG      ; exit function if [EOT] or ":"
  JSR LAB_E1FD    ; scan and get byte, else do syntax error then warm start
  TXA       ; copy command to .A
  TAY       ; copy command to .Y
  LDX FORPNT+1    ; get device number
  LDA FORPNT      ; get logical file number
  JSR SETLFS      ; set logical, first and second addresses
  JSR IFCHRG      ; exit function if [EOT] or ":"
  JSR SKPCOM      ; scan for ",byte", else do syntax error then warm start


;***********************************************************************************;
;
; set filename

LAB_E254
  JSR FRMEVL      ; evaluate expression
  JSR DELST     ; evaluate string
  LDX INDEX     ; get string pointer low byte
  LDY INDEX+1     ; get string pointer high byte
  JMP SETNAM      ; set filename and return


;***********************************************************************************;
;
; perform COS()

COS
  LDA #<FPC20     ; set pi/2 pointer low byte
  LDY #>FPC20     ; set pi/2 pointer high byte
  JSR LAPLUS      ; add (.A.Y) to FAC1


;***********************************************************************************;
;
; perform SIN()

SIN
  JSR RFTOA     ; round and copy FAC1 to FAC2
  LDA #<LAB_E2E2    ; set 2*pi pointer low byte
  LDY #>LAB_E2E2    ; set 2*pi pointer high byte
  LDX FAC2+FAC_SIGN   ; get FAC2 sign (b7)
  JSR LAB_DB07    ; divide by (.A.Y) (.X=sign)
  JSR RFTOA     ; round and copy FAC1 to FAC2
  JSR INT     ; perform INT()
  LDA #$00      ; clear byte
  STA ARISGN      ; clear sign compare (FAC1 XOR FAC2)
  JSR SUB     ; perform subtraction, FAC2 from FAC1
  LDA #<LAB_E2E7    ; set 0.25 pointer low byte
  LDY #>LAB_E2E7    ; set 0.25 pointer high byte
  JSR LAMIN     ; perform subtraction, FAC1 from (.A.Y)
  LDA FAC1+FAC_SIGN   ; get FAC1 sign (b7)
  PHA       ; save FAC1 sign
  BPL LAB_E29A    ; branch if +ve

          ; FAC1 sign was -ve
  JSR ADD05     ; add 0.5 to FAC1 (round FAC1)
  LDA FAC1+FAC_SIGN   ; get FAC1 sign (b7)
  BMI LAB_E29D    ; branch if -ve

  LDA TANSGN      ; get the comparison evaluation flag
  EOR #$FF      ; toggle flag
  STA TANSGN      ; save the comparison evaluation flag
LAB_E29A
  JSR NEGFAC      ; do - FAC1
LAB_E29D
  LDA #<LAB_E2E7    ; set 0.25 pointer low byte
  LDY #>LAB_E2E7    ; set 0.25 pointer high byte
  JSR LAPLUS      ; add (.A.Y) to FAC1
  PLA       ; restore FAC1 sign
  BPL LAB_E2AA    ; branch if was +ve

          ; else correct FAC1
  JSR NEGFAC      ; do - FAC1
LAB_E2AA
  LDA #<LAB_E2EC    ; set pointer low byte to counter
  LDY #>LAB_E2EC    ; set pointer high byte to counter
  JMP SEREVL      ; ^2 then series evaluation and return


;***********************************************************************************;
;
; perform TAN()

TAN
  JSR FACTF1      ; pack FAC1 into LAB_57
  LDA #$00      ; clear .A
  STA TANSGN      ; clear the comparison evaluation flag
  JSR SIN     ; perform SIN()
  LDX #<DEFPNT    ; set sin(n) pointer low byte
  LDY #>DEFPNT    ; set sin(n) pointer high byte
  JSR LAB_E0F3    ; pack FAC1 into (.X.Y)
  LDA #<TEMPF3    ; set n pointer low byte
  LDY #>TEMPF3    ; set n pointer high byte
  JSR LODFAC      ; unpack memory (.A.Y) into FAC1
  LDA #$00      ; clear byte
  STA FAC1+FAC_SIGN   ; clear FAC1 sign (b7)
  LDA TANSGN      ; get the comparison evaluation flag
  JSR LAB_E2D9    ; save flag and go do series evaluation
  LDA #<DEFPNT    ; set sin(n) pointer low byte
  LDY #>DEFPNT    ; set sin(n) pointer high byte
  JMP LADIV     ; convert .A.Y and do (.A.Y)/FAC1


;***********************************************************************************;
;
; save comparison flag and do series evaluation

LAB_E2D9
  PHA       ; save comparison flag
  JMP LAB_E29A    ; add 0.25, ^2 then series evaluation


;***********************************************************************************;
;
; constants and series for SIN/COS(n)

FPC20
  .byte $81,$49,$0F,$DA,$A2 ; 1.570796371, pi/2, as floating number
LAB_E2E2
  .byte $83,$49,$0F,$DA,$A2 ; 6.28319, 2*pi, as floating number
LAB_E2E7
  .byte $7F,$00,$00,$00,$00 ; 0.25

LAB_E2EC
  .byte $05     ; series counter
  .byte $84,$E6,$1A,$2D,$1B ; -14.3813907
  .byte $86,$28,$07,$FB,$F8 ;  42.0077971
  .byte $87,$99,$68,$89,$01 ; -76.7041703
  .byte $87,$23,$35,$DF,$E1 ;  81.6052237
  .byte $86,$A5,$5D,$E7,$28 ; -41.3417021
  .byte $83,$49,$0F,$DA,$A2 ;  6.28318531


;***********************************************************************************;
;
; perform ATN()

ATN
  LDA FAC1+FAC_SIGN   ; get FAC1 sign (b7)
  PHA       ; save sign
  BPL LAB_E313    ; branch if +ve

  JSR NEGFAC      ; else do - FAC1
LAB_E313
  LDA FAC1+FAC_EXPT   ; get FAC1 exponent
  PHA       ; push exponent
  CMP #$81      ; compare with 1
  BCC LAB_E321    ; branch if FAC1 < 1

  LDA #<FPC1      ; pointer to 1 low byte
  LDY #>FPC1      ; pointer to 1 high byte
  JSR LADIV     ; convert .A.Y and do (.A.Y)/FAC1
LAB_E321
  LDA #<ATNCON    ; pointer to series low byte
  LDY #>ATNCON    ; pointer to series high byte
  JSR SEREVL      ; ^2 then series evaluation
  PLA       ; restore old FAC1 exponent
  CMP #$81      ; compare with 1
  BCC LAB_E334    ; branch if FAC1 < 1

  LDA #<FPC20     ; pointer to (pi/2) low byte
  LDY #>FPC20     ; pointer to (pi/2) low byte
  JSR LAMIN     ; perform subtraction, FAC1 from (.A.Y)
LAB_E334
  PLA       ; restore FAC1 sign
  BPL LAB_E33A    ; exit if was +ve

  JMP NEGFAC      ; else do - FAC1 and return

LAB_E33A
  RTS


;***********************************************************************************;
;
; series for ATN(n)

ATNCON
  .byte $0B     ; series counter
  .byte $76,$B3,$83,$BD,$D3 ; -6.84793912e-04
  .byte $79,$1E,$F4,$A6,$F5 ;  4.85094216e-03
  .byte $7B,$83,$FC,$B0,$10 ; -0.0161117015
  .byte $7C,$0C,$1F,$67,$CA ;  0.034209638
  .byte $7C,$DE,$53,$CB,$C1 ; -0.054279133
  .byte $7D,$14,$64,$70,$4C ;  0.0724571965
  .byte $7D,$B7,$EA,$51,$7A ; -0.0898019185
  .byte $7D,$63,$30,$88,$7E ;  0.110932413
  .byte $7E,$92,$44,$99,$3A ; -0.142839808
  .byte $7E,$4C,$CC,$91,$C7 ;  0.19999912
  .byte $7F,$AA,$AA,$AA,$13 ; -0.333333316
  .byte $81,$00,$00,$00,$00 ;  1.000000000


;***********************************************************************************;
;
; BASIC cold start entry point

COLDBA
  JSR INITVCTRS   ; initialise BASIC vector table
  JSR INITBA      ; initialise BASIC RAM locations
  JSR FREMSG      ; print start up message and initialise memory pointers
  LDX #$FB      ; value for start stack
  TXS       ; set stack pointer
  JMP READY     ; do "READY." warm start


;***********************************************************************************;
;
; character get subroutine for zero page

; the target address for the LDA LAB_EA60 becomes the BASIC execute pointer once the
; block is copied to its destination, any non zero page address will do at assembly
; time, to assemble a three byte instruction.

; page 0 initialisation table from CHRGET
; increment and scan memory

CGIMAG
  INC CHRGOT+1    ; increment BASIC execute pointer low byte
  BNE LAB_E38D    ; branch if no carry
          ; else
  INC CHRGOT+2    ; increment BASIC execute pointer high byte

; page 0 initialisation table from CHRGOT
; scan memory

LAB_E38D
  LDA LAB_EA60    ; get byte to scan, address set by call routine
  CMP #':'      ; compare with ":"
  BCS LAB_E39E    ; exit if >=

; page 0 initialisation table from CHRSPC
; clear Cb if numeric

  CMP #' '      ; compare with " "
  BEQ CGIMAG      ; if " " go do next

  SEC       ; set carry for SBC
  SBC #'0'      ; subtract "0"
  SEC       ; set carry for SBC
  SBC #$D0      ; subtract -"0"
          ; clear carry if byte = "0"-"9"
LAB_E39E
  RTS


;***********************************************************************************;
;
; spare bytes, not referenced

;LAB_E39F
  .byte $80,$4F,$C7,$52,$58
          ; 0.811635157


;***********************************************************************************;
;
; initialise BASIC RAM locations

INITBA
  LDA #$4C      ; opcode for JMP
  STA JMPER     ; save for functions vector jump
  STA USRPPOK     ; save for USR() vector jump
          ; set USR() vector to illegal quantity error
  LDA #<ILQUAN    ; set USR() vector low byte
  LDY #>ILQUAN    ; set USR() vector high byte
  STA ADDPRC      ; save USR() vector low byte
  STY ADDPRC+1    ; save USR() vector high byte
  LDA #<MAKFP     ; set fixed to float vector low byte
  LDY #>MAKFP     ; set fixed to float vector high byte
  STA ADRAY2      ; save fixed to float vector low byte
  STY ADRAY2+1    ; save fixed to float vector high byte
  LDA #<INTIDX    ; set float to fixed vector low byte
  LDY #>INTIDX    ; set float to fixed vector high byte
  STA ADRAY1      ; save float to fixed vector low byte
  STY ADRAY1+1    ; save float to fixed vector high byte

; copy block from CGIMAG to CHRGET

  LDX #$1C      ; set byte count
LAB_E3C4
  LDA CGIMAG,X    ; get byte from table
  STA CHRGET,X    ; save byte in page zero
  DEX       ; decrement count
  BPL LAB_E3C4    ; loop if not all done

  LDA #$03      ; set step size, collecting descriptors
  STA FOUR6     ; save garbage collection step size
  LDA #$00      ; clear .A
  STA BITS      ; clear FAC1 overflow byte
  STA CHANNL      ; clear current I/O channel, flag default
  STA LASTPT+1    ; clear current descriptor stack item pointer high byte
  LDX #$01      ; set .X
  STX CHNLNK+1    ; set chain link pointer low byte
  STX CHNLNK      ; set chain link pointer high byte
  LDX #TEMPST     ; initial value for descriptor stack
  STX TEMPPT      ; set descriptor stack pointer
  SEC       ; set Cb = 1 to read the bottom of memory
  JSR MEMBOT      ; read/set the bottom of memory
  STX TXTTAB      ; save start of memory low byte
  STY TXTTAB+1    ; save start of memory high byte
  SEC       ; set Cb = 1 to read the top of memory
  JSR MEMTOP      ; read/set the top of memory
  STX MEMSIZ      ; save end of memory low byte
  STY MEMSIZ+1    ; save end of memory high byte
  STX FRETOP      ; set bottom of string space low byte
  STY FRETOP+1    ; set bottom of string space high byte
  LDY #$00      ; clear index
  TYA       ; clear .A
  STA (TXTTAB),Y    ; clear first byte of memory
  INC TXTTAB      ; increment start of memory low byte
  BNE LAB_E403    ; branch if no rollover

  INC TXTTAB+1    ; increment start of memory high byte
LAB_E403
  RTS


;***********************************************************************************;
;
; print start up message and initialise memory pointers

FREMSG
  LDA TXTTAB      ; get start of memory low byte
  LDY TXTTAB+1    ; get start of memory high byte
  JSR RAMSPC      ; check available memory, do out of memory error if no room
  LDA #<BASMSG    ; set "**** CBM BASIC V2 ****" pointer low byte
  LDY #>BASMSG    ; set "**** CBM BASIC V2 ****" pointer high byte
  JSR PRTSTR      ; print null terminated string
  LDA MEMSIZ      ; get end of memory low byte
  SEC       ; set carry for subtract
  SBC TXTTAB      ; subtract start of memory low byte
  TAX       ; copy result to .X
  LDA MEMSIZ+1    ; get end of memory high byte
  SBC TXTTAB+1    ; subtract start of memory high byte
  JSR PRTFIX      ; print .X.A as unsigned integer
  LDA #<BFREMSG   ; set " BYTES FREE" pointer low byte
  LDY #>BFREMSG   ; set " BYTES FREE" pointer high byte
  JSR PRTSTR      ; print null terminated string
  JMP LAB_C644    ; do NEW, CLR, RESTORE and return


;***********************************************************************************;
;
BFREMSG
  .byte " BYTES FREE",$0D,$00
BASMSG
  .byte $93,"**** CBM BASIC V2 ****",$0D,$00


;***********************************************************************************;
;
; BASIC vectors, these are copied to RAM from IERROR onwards

BASVCTRS
  .word ERROR2      ; error message       IERROR
  .word MAIN2     ; BASIC warm start      IMAIN
  .word CRNCH2      ; crunch BASIC tokens     ICRNCH
  .word QPLOP     ; uncrunch BASIC tokens     IQPLOP
  .word GONE      ; start new BASIC code      IGONE
  .word FEVAL     ; get arithmetic element    IEVAL


;***********************************************************************************;
;
; initialise BASIC vectors

INITVCTRS
  LDX #$0B      ; set byte count
LAB_E45D
  LDA BASVCTRS,X    ; get byte from table
  STA IERROR,X    ; save byte to RAM
  DEX       ; decrement index
  BPL LAB_E45D    ; loop if more to do

  RTS


;***********************************************************************************;
;
; BASIC warm start entry point

WARMBAS
  JSR CLRCHN      ; close input and output channels
  LDA #$00      ; clear .A
  STA CHANNL      ; set current I/O channel, flag default
  JSR LAB_C67A    ; flush BASIC stack and clear continue pointer
  CLI       ; enable interrupts
  JMP READY     ; do warm start


;***********************************************************************************;
;
; checksum byte, not referenced

;LAB_E475
  .byte $E8     ; [PAL]
; .byte $41     ; [NTSC]


;***********************************************************************************;
;
; rebuild BASIC line chaining and do RESTORE

PATCHER
  JSR LNKPRG      ; rebuild BASIC line chaining
  JMP LAB_C677    ; do RESTORE, clear stack and return


;***********************************************************************************;
;
; spare bytes, not referenced

;LAB_E47C
  .byte $FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF
  .byte $FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF
  .byte $FF,$FF,$FF,$FF


;***********************************************************************************;
;
; set serial data out high

SEROUT1
  LDA VIA2PCR     ; get VIA 2 PCR
  AND #$DF      ; set CB2 low, serial data out high
  STA VIA2PCR     ; set VIA 2 PCR
  RTS


;***********************************************************************************;
;
; set serial data out low

SEROUT0
  LDA VIA2PCR     ; get VIA 2 PCR
  ORA #$20      ; set CB2 high, serial data out low
  STA VIA2PCR     ; set VIA 2 PCR
  RTS


;***********************************************************************************;
;
; get serial clock status

SERGET
  LDA VIA1PA2     ; get VIA 1 DRA, no handshake
  CMP VIA1PA2     ; compare with self
  BNE SERGET      ; loop if changing

  LSR       ; shift serial clock to Cb
  RTS


;***********************************************************************************;
;
; get secondary address and print "SEARCHING..."

PATCH1
  LDX SA      ; get secondary address
  JMP SRCHING     ; print "SEARCHING..." and return


;***********************************************************************************;
;
; set LOAD address if secondary address = 0

PATCH2
  TXA       ; copy secondary address
  BNE LAB_E4CC    ; load location not set in LOAD call, so
          ; continue with load
  LDA MEMUSS      ; get load start address low byte
  STA EAL     ; save program start address low byte
  LDA MEMUSS+1    ; get load start address high byte
  STA EAL+1     ; save program start address high byte
LAB_E4CC
  JMP LDVRMSG     ; display "LOADING" or "VERIFYING" and return


;***********************************************************************************;
;
; patch for CLOSE

PATCH3
  JSR WBLK      ; initiate tape write
  BCC LAB_E4D7    ; branch if no error

  PLA       ; else dump stacked exit code
  LDA #$00      ; clear exit code
LAB_E4D7
  JMP LAB_F39E    ; go do I/O close


;***********************************************************************************;
;
; spare bytes, not referenced

  .byte $FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF
  .byte $FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF
  .byte $FF,$FF,$FF,$FF,$FF,$FF


;***********************************************************************************;
;
; return base address of I/O devices

; This routine will set .X.Y to the address of the memory section where the memory
; mapped I/O devices are located. This address can then be used with an offset to
; access the memory mapped I/O devices in the computer.

FIOBASE
  LDX #<VIA1PB    ; get I/O base address low byte
  LDY #>VIA1PB    ; get I/O base address high byte
  RTS


;***********************************************************************************;
;
; return X,Y organisation of screen

; this routine returns the x,y organisation of the screen in .X,.Y

FSCREEN
  LDX #$16      ; get screen X, 22 columns
  LDY #$17      ; get screen Y, 23 rows
  RTS


;***********************************************************************************;
;
; read/set X,Y cursor position, Cb = 1 to read, Cb = 0 to set

; This routine, when called with the carry flag set, loads the current position of
; the cursor on the screen into the .X and .Y registers. .Y is the column number of
; the cursor location and .X is the row number of the cursor. A call with the carry
; bit clear moves the cursor to the position determined by the .X and .Y registers.

FPLOT
  BCS LAB_E513    ; if read cursor skip the set cursor

  STX TBLX      ; save cursor row
  STY PNTR      ; save cursor column
  JSR SETSLINK    ; set screen pointers for cursor row, column
LAB_E513
  LDX TBLX      ; get cursor row
  LDY PNTR      ; get cursor column
  RTS


;***********************************************************************************;
;
; initialise hardware

INITSK
  JSR SETIODEF    ; set default devices and initialise VIC chip
  LDA HIBASE      ; get screen memory page
  AND #$FD      ; mask xxxx xx0x, all but va9
  ASL       ; << 1 xxxx x0x0
  ASL       ; << 2 xxxx 0x00
  ORA #$80      ; set  1xxx 0x00
  STA VICCR5      ; set screen and character memory location
  LDA HIBASE      ; get screen memory page
  AND #$02      ; mask va9 bit
  BEQ LAB_E536    ; if zero just go normalise screen

          ; else set va9 in VIC chip
  LDA #$80      ; set b7
  ORA VICCR2      ; OR in as video address 9
  STA VICCR2      ; save new video address

          ; now normalise screen
LAB_E536
  LDA #$00      ; clear .A
  STA MODE      ; clear shift mode switch
  STA BLNON     ; clear cursor blink phase
  LDA #<SETKEYS   ; get keyboard decode logic pointer low byte
  STA KEYLOG      ; set keyboard decode logic pointer low byte
  LDA #>SETKEYS   ; get keyboard decode logic pointer high byte
  STA KEYLOG+1    ; set keyboard decode logic pointer high byte
  LDA #$0A      ; 10d
  STA XMAX      ; set maximum size of keyboard buffer
  STA DELAY     ; set repeat delay counter
  LDA #$06      ; colour blue
  STA COLOR     ; set current colour code
  LDA #$04      ; speed 4
  STA KOUNT     ; set repeat speed counter
  LDA #$0C      ; cursor flash timing
  STA BLNCT     ; set cursor timing countdown
  STA BLNSW     ; set cursor enable, $00 = flash cursor

; clear screen

CLSR
  LDA HIBASE      ; get screen memory page
  ORA #$80      ; set high bit, flag every line is logical line start
  TAY       ; copy to .Y
  LDA #$00      ; clear line start low byte
  TAX       ; clear index
LAB_E568
  STY LDTB1,X     ; save start of line .X pointer high byte
  CLC       ; clear carry for add
  ADC #$16      ; add line length to low byte
  BCC LAB_E570    ; if no rollover skip the high byte increment

  INY       ; else increment high byte
LAB_E570
  INX       ; increment line index
  CPX #$18      ; compare with number of lines + 1
  BNE LAB_E568    ; loop if not all done

  LDA #$FF      ; end of table marker ??
  STA LDTB1,X     ; mark end of table
  LDX #$16      ; set line count, 23 lines to do, 0 to 22
LAB_E57B
  JSR CLRALINE    ; clear screen line .X
  DEX       ; decrement count
  BPL LAB_E57B    ; loop if more to do

; home cursor

HOME
  LDY #$00      ; clear .Y
  STY PNTR      ; clear cursor column
  STY TBLX      ; clear cursor row

; set screen pointers for cursor row, column

SETSLINK
  LDX TBLX      ; get cursor row
  LDA PNTR      ; get cursor column
LAB_E58B
  LDY LDTB1,X     ; get start of line .X pointer high byte
  BMI LAB_E597    ; continue if logical line start

  CLC       ; else clear carry for add
  ADC #$16      ; add one line length
  STA PNTR      ; save cursor column
  DEX       ; decrement cursor row
  BPL LAB_E58B    ; loop, branch always

LAB_E597
  LDA LDTB1,X     ; get start of line .X pointer high byte
  AND #$03      ; mask 0000 00xx, line memory page
  ORA HIBASE      ; OR with screen memory page
  STA PNT+1     ; set current screen line pointer high byte
  LDA LDTB2,X     ; get start of line low byte from ROM table
  STA PNT     ; set current screen line pointer low byte
  LDA #$15      ; set line length
  INX       ; increment cursor row
LAB_E5A8
  LDY LDTB1,X     ; get start of line .X pointer high byte
  BMI LAB_E5B2    ; exit if logical line start

  CLC       ; else clear carry for add
  ADC #$16      ; add one line length to current line length
  INX       ; increment cursor row
  BPL LAB_E5A8    ; loop, branch always

LAB_E5B2
  STA LNMX      ; save current screen line length
  RTS


;***********************************************************************************;
;
; set default devices, initialise VIC chip and home cursor
;
; unreferenced code

;UNUSDNMI
  JSR SETIODEF    ; set default devices and initialise VIC chip
  JMP HOME      ; home cursor and return


;***********************************************************************************;
;
; set default devices and initialise VIC chip

SETIODEF
  LDA #$03      ; set screen
  STA DFLTO     ; set output device number
  LDA #$00      ; set keyboard
  STA DFLTN     ; set input device number

; initialise VIC chip

INITVIC
  LDX #$10      ; set byte count
LAB_E5C5
  LDA VICINIT-1,X   ; get byte from setup table
  STA VICCR0-1,X    ; save byte to VIC chip
  DEX       ; decrement count/index
  BNE LAB_E5C5    ; loop if more to do

  RTS


;***********************************************************************************;
;
; input from keyboard buffer

LP2
  LDY KEYD      ; get current character from buffer
  LDX #$00      ; clear index
LAB_E5D4
  LDA KEYD+1,X    ; get next character,.X from buffer
  STA KEYD,X      ; save as current character,.X in buffer
  INX       ; increment index
  CPX NDX     ; compare with keyboard buffer index
  BNE LAB_E5D4    ; loop if more to do

  DEC NDX     ; decrement keyboard buffer index
  TYA       ; copy key to .A
  CLI       ; enable interrupts
  CLC       ; flag got byte
  RTS


;***********************************************************************************;
;
; write character and wait for key

GETQUE
  JSR SCRNOUT     ; output character

; wait for key from keyboard

LAB_E5E8
  LDA NDX     ; get keyboard buffer index
  STA BLNSW     ; cursor enable, $00 = flash cursor, $xx = no flash
  STA AUTODN      ; screen scrolling flag, $00 = scroll, $xx = no scroll
          ; this disables both the cursor flash and the screen scroll
          ; while there are characters in the keyboard buffer
  BEQ LAB_E5E8    ; loop if buffer empty

  SEI       ; disable interrupts
  LDA BLNON     ; get cursor blink phase
  BEQ LAB_E602    ; branch if cursor phase

          ; else character phase
  LDA CDBLN     ; get character under cursor
  LDX GDCOL     ; get colour under cursor
  LDY #$00      ; clear .Y
  STY BLNON     ; clear cursor blink phase
  JSR SYNPRT      ; print character .A and colour .X
LAB_E602
  JSR LP2     ; input from keyboard buffer
  CMP #$83      ; compare with [SHIFT][RUN]
  BNE GET2RTN     ; branch if not [SHIFT][RUN]

          ; keys are [SHIFT][RUN] so put "LOAD",$0D,"RUN",$0D into
          ; the buffer
  LDX #$09      ; set byte count
  SEI       ; disable interrupts
  STX NDX     ; set keyboard buffer index
LAB_E60E
  LDA RUNTB-1,X   ; get byte from auto load/run table
  STA KEYD-1,X    ; save to keyboard buffer
  DEX       ; decrement count/index
  BNE LAB_E60E    ; loop while more to do

  BEQ LAB_E5E8    ; loop for next key, branch always

          ; was not [SHIFT][RUN]
GET2RTN
  CMP #$0D      ; compare with [RETURN]
  BNE GETQUE      ; if not [RETURN] print character and get next key

          ; was [RETURN]
  LDY LNMX      ; get current screen line length
  STY CRSW      ; set input from screen

LAB_E621
  LDA (PNT),Y     ; get character from current screen line
  CMP #' '      ; compare with [SPACE]
  BNE LAB_E62A    ; branch if not [SPACE]

  DEY       ; else eliminate the space, decrement end of input line
  BNE LAB_E621    ; loop, branch always

LAB_E62A
  INY       ; increment past last non space character on line
  STY INDX      ; save input EOL pointer
  LDY #$00      ; clear .Y
  STY AUTODN      ; clear screen scrolling flag, $00 = scroll, $xx = no scroll
  STY PNTR      ; clear cursor column
  STY QTSW      ; clear cursor quote flag
  LDA LXSP      ; get input cursor row
  BMI LAB_E657    ; branch if input cursor row has become -ve

  LDX TBLX      ; get cursor row
  JSR LAB_E719    ; find and set pointers for start of logical line
  CPX LXSP      ; compare with input cursor row
  BNE LAB_E657    ;.

  BNE LAB_E657    ;.?? what's this? just to make sure or something

  LDA LXSP+1      ; get input cursor column
  STA PNTR      ; save cursor column
  CMP INDX      ; compare with input EOL pointer
  BCC LAB_E657    ; branch if less, cursor is in line

  BCS LAB_E691    ; else cursor is beyond the line end, branch always


;***********************************************************************************;
;
; input from screen or keyboard

GETSCRN
  TYA       ; copy .Y
  PHA       ; save .Y
  TXA       ; copy .X
  PHA       ; save .X
  LDA CRSW      ; get input from keyboard or screen, $xx = screen,
          ; $00 = keyboard
  BEQ LAB_E5E8    ; if keyboard go wait for key

LAB_E657
  LDY PNTR      ; get cursor column
  LDA (PNT),Y     ; get character from the current screen line
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ; just a few wasted cycles
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;

          ; map to ASCII

  STA ASCII     ; save temporary last character
  AND #$3F      ; leave b5 to b0 in .A
  ASL ASCII     ; shift b7 into Cb
  BIT ASCII     ; copy b6 into Sb, b5 into Ob
  BPL LAB_E67E    ; branch if b6 = 0

  ORA #$80      ; map $40-$7F to $C0-$FF
LAB_E67E
  BCC LAB_E684    ; branch if b7 = 0

  LDX QTSW      ; get cursor quote flag, $01 = quote, $00 = no quote
  BNE LAB_E688    ; branch if in quote mode

LAB_E684
  BVS LAB_E688    ; branch if b5 = 1

  ORA #$40      ; map $00-$1F to $40-5F
LAB_E688
  INC PNTR      ; increment cursor column
  JSR QUOTECK     ; if open quote toggle cursor quote flag
  CPY INDX      ; compare with input EOL pointer
  BNE LAB_E6A8    ; branch if not at line end

LAB_E691
  LDA #$00      ; clear .A
  STA CRSW      ; set input from keyboard
  LDA #$0D      ; set character [CR]
  LDX DFLTN     ; get input device number
  CPX #$03      ; compare with screen
  BEQ LAB_E6A3    ; branch if screen

  LDX DFLTO     ; get output device number
  CPX #$03      ; compare with screen
  BEQ LAB_E6A6    ; branch if screen

LAB_E6A3
  JSR SCRNOUT     ; output character
LAB_E6A6
  LDA #$0D      ; set character [CR]
LAB_E6A8
  STA ASCII     ; save character
  PLA       ; pull .X
  TAX       ; restore .X
  PLA       ; pull .Y
  TAY       ; restore .Y
  LDA ASCII     ; restore character
  CMP #$DE      ; compare with [PI]
  BNE LAB_E6B6    ; exit if not [PI]

  LDA #TK_PI      ; set character to BASIC token
LAB_E6B6
  CLC       ; flag ok
  RTS


;***********************************************************************************;
;
; if open quote toggle cursor quote flag

QUOTECK
  CMP #$22      ; compare byte with "
  BNE LAB_E6C4    ; exit if not "

  LDA QTSW      ; get cursor quote flag
  EOR #$01      ; toggle b0
  STA QTSW      ; save cursor quote flag
  LDA #$22      ; restore the "
LAB_E6C4
  RTS


;***********************************************************************************;
;
; insert uppercase/graphic character

SETCHAR
  ORA #$40      ; change to uppercase/graphic
LAB_E6C7
  LDX RVS     ; get reverse flag
  BEQ LAB_E6CD    ; branch if not reverse

          ; else ..
; insert reversed character

LAB_E6CB
  ORA #$80      ; reverse character
LAB_E6CD
  LDX INSRT     ; get insert count
  BEQ LAB_E6D3    ; branch if none

  DEC INSRT     ; else decrement insert count
LAB_E6D3
  LDX COLOR     ; get current colour code
  JSR SYNPRT      ; print character .A and colour .X
  JSR SCROLL      ; advance cursor

; restore registers, set quote flag and exit

LAB_E6DC
  PLA       ; pull .Y
  TAY       ; restore .Y
  LDA INSRT     ; get insert count
  BEQ LAB_E6E4    ; skip quote flag clear if inserts to do

  LSR QTSW      ; clear cursor quote flag
LAB_E6E4
  PLA       ; pull .X
  TAX       ; restore .X
  PLA       ; restore .A
  CLC       ; flag ok
  CLI       ; enable interrupts
  RTS


;***********************************************************************************;
;
; advance cursor

SCROLL
  JSR FORWARD     ; test for line increment
  INC PNTR      ; increment cursor column
  LDA LNMX      ; get current screen line length
  CMP PNTR      ; compare with cursor column
  BCS LAB_E72C    ; exit if line length >= cursor column

  CMP #$57      ; compare with max length
  BEQ LAB_E723    ; if at max clear column, back cursor up and do newline

  LDA AUTODN      ; get autoscroll flag
  BEQ LAB_E701    ; branch if autoscroll on

  JMP LAB_E9F0    ; else open space on screen

LAB_E701
  LDX TBLX      ; get cursor row
  CPX #$17      ; compare with max + 1
  BCC LAB_E70E    ; if less than max + 1 go add this row to the current
          ; logical line

  JSR SCRL      ; else scroll screen
  DEC TBLX      ; decrement cursor row
  LDX TBLX      ; get cursor row

; add this row to the current logical line

LAB_E70E
  ASL LDTB1,X     ; shift start of line .X pointer high byte
  LSR LDTB1,X     ; shift start of line .X pointer high byte back,
          ; clear b7, start of logical line
  JMP WRAPLINE    ; make next screen line start of logical line, increment
          ; line length and set pointers

; add one line length and set pointers for start of line

LAB_E715
  ADC #$16      ; add one line length
  STA LNMX      ; save current screen line length

; find and set pointers for start of logical line

LAB_E719
  LDA LDTB1,X     ; get start of line .X pointer high byte
  BMI LAB_E720    ; exit loop if start of logical line

  DEX       ; else back up one line
  BNE LAB_E719    ; loop if not on first line

LAB_E720
  JMP LINPTR      ; set start of line .X and return

; clear cursor column, back cursor up one line and do newline

LAB_E723
  DEC TBLX      ; decrement cursor row. if the cursor was incremented past
          ; the last line then this decrement and the scroll will
          ; leave the cursor one line above the bottom of the screen
  JSR NXTLINE     ; do newline
  LDA #$00      ; clear .A
  STA PNTR      ; clear cursor column
LAB_E72C
  RTS

; back onto previous line if possible

RETREAT
  LDX TBLX      ; get cursor row
  BNE LAB_E737    ; branch if not top row

  STX PNTR      ; clear cursor column
  PLA       ; dump return address low byte
  PLA       ; dump return address high byte
  BNE LAB_E6DC    ; restore registers, set quote flag and exit, branch always

LAB_E737
  DEX       ; decrement cursor row
  STX TBLX      ; save cursor row
  JSR SETSLINK    ; set screen pointers for cursor row, column
  LDY LNMX      ; get current screen line length
  STY PNTR      ; save as cursor column
  RTS


;***********************************************************************************;
;
; output character to screen

SCRNOUT
  PHA       ; save character
  STA ASCII     ; save temporary last character
  TXA       ; copy .X
  PHA       ; save .X
  TYA       ; copy .Y
  PHA       ; save .Y
  LDA #$00      ; clear .A
  STA CRSW      ; set input from keyboard
  LDY PNTR      ; get cursor column
  LDA ASCII     ; restore last character
  BPL LAB_E756    ; branch if unshifted

  JMP LAB_E800    ; do shifted characters and return

LAB_E756
  CMP #$0D      ; compare with [CR]
  BNE LAB_E75D    ; branch if not [CR]

  JMP RTRN      ; else output [CR] and return

LAB_E75D
  CMP #' '      ; compare with [SPACE]
  BCC LAB_E771    ; branch if < [SPACE]

          ; map to screen code

  CMP #$60      ; compare with first graphic character
  BCC LAB_E769    ; branch if $20 to $5F

          ; character is between $60 and $7F
  AND #$DF      ; mask xx0x xxxx, map to $40-$5F
  BNE LAB_E76B    ; branch always

LAB_E769
  AND #$3F      ; mask 00xx xxxx, map $40-$5F to $00-$1F
LAB_E76B
  JSR QUOTECK     ; if open quote toggle cursor direct/programmed flag
  JMP LAB_E6C7    ; print character, scroll if needed and exit

          ; character was < [SPACE] so is a control character
          ; of some sort
LAB_E771
  LDX INSRT     ; get insert count
  BEQ LAB_E778    ; branch if no characters to insert

  JMP LAB_E6CB    ; insert reversed character

LAB_E778
  CMP #$14      ; compare with [DELETE]
  BNE LAB_E7AA    ; branch if not [DELETE]

  TYA       ; copy cursor column to .A
  BNE LAB_E785    ; branch if not at start of line

  JSR RETREAT     ; back onto previous line if possible
  JMP LAB_E79F    ; clear last character on current screen line

LAB_E785
  JSR BACKUP      ; test for line decrement

          ; now close up the line
  DEY       ; decrement index to previous character
  STY PNTR      ; save cursor column
  JSR COLORSYN    ; calculate pointer to colour RAM
LAB_E78E
  INY       ; increment index to next character
  LDA (PNT),Y     ; get character from current screen line
  DEY       ; decrement index to previous character
  STA (PNT),Y     ; save character to current screen line
  INY       ; increment index to next character
  LDA (USER),Y    ; get colour RAM byte
  DEY       ; decrement index to previous character
  STA (USER),Y    ; save colour RAM byte
  INY       ; increment index to next character
  CPY LNMX      ; compare with current screen line length
  BNE LAB_E78E    ; loop if not there yet

LAB_E79F
  LDA #' '      ; set [SPACE]
  STA (PNT),Y     ; clear last character on current screen line
  LDA COLOR     ; get current colour code
  STA (USER),Y    ; save to colour RAM
  BPL LAB_E7F7    ; restore registers, set quote flag and exit, branch always

LAB_E7AA
  LDX QTSW      ; get cursor quote flag, $01 = quote, $00 = no quote
  BEQ LAB_E7B1    ; branch if not quote mode

  JMP LAB_E6CB    ; insert reversed character, scroll if needed and exit

LAB_E7B1
  CMP #$12      ; compare with [RVS ON]
  BNE LAB_E7B7    ; branch if not [RVS ON]

  STA RVS     ; set reverse flag
LAB_E7B7
  CMP #$13      ; compare with [HOME]
  BNE LAB_E7BE    ; branch if not [HOME]

  JSR HOME      ; home cursor
LAB_E7BE
  CMP #$1D      ; compare with [CURSOR RIGHT]
  BNE LAB_E7D9    ; branch if not [CURSOR RIGHT]

  INY       ; increment cursor column
  JSR FORWARD     ; test for line increment
  STY PNTR      ; save cursor column
  DEY       ; decrement cursor column
  CPY LNMX      ; compare cursor column with current screen line length
  BCC LAB_E7D6    ; exit if less

          ; else the cursor column is >= the current screen line
          ; length so back onto the current line and do a newline
  DEC TBLX      ; decrement cursor row
  JSR NXTLINE     ; do newline
  LDY #$00      ; clear cursor column
LAB_E7D4
  STY PNTR      ; save cursor column
LAB_E7D6
  JMP LAB_E6DC    ; restore registers, set quote flag and exit

LAB_E7D9
  CMP #$11      ; compare with [CURSOR DOWN]
  BNE LAB_E7FA    ; branch if not [CURSOR DOWN]

  CLC       ; clear carry for add
  TYA       ; copy cursor column
  ADC #$16      ; add one line
  TAY       ; copy back to .A
  INC TBLX      ; increment cursor row
  CMP LNMX      ; compare cursor column with current screen line length
  BCC LAB_E7D4    ; save cursor column and exit if less

  BEQ LAB_E7D4    ; save cursor column and exit if equal

          ; else the cursor has moved beyond the end of this line
          ; so back it up until it's on the start of the logical line
  DEC TBLX      ; decrement cursor row
LAB_E7EC
  SBC #$16      ; subtract one line
  BCC LAB_E7F4    ; exit loop if on previous line

  STA PNTR      ; else save cursor column
  BNE LAB_E7EC    ; loop if not at start of line

LAB_E7F4
  JSR NXTLINE     ; do newline
LAB_E7F7
  JMP LAB_E6DC    ; restore registers, set quote flag and exit

LAB_E7FA
  JSR COLORSET    ; set the colour from the character in .A
  JMP CHARSET     ; select VIC character set

          ; character is $80 or greater

LAB_E800
  NOP       ; just a few wasted cycles
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  AND #$7F      ; mask 0xxx xxxx, clear b7
  CMP #$7F      ; was it [PI] before the mask
  BNE LAB_E81D    ; branch if not

  LDA #$5E      ; else make it $5E
LAB_E81D
  NOP       ; just a few wasted cycles
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  CMP #' '      ; compare with [SPACE]
  BCC LAB_E82A    ; branch if < [SPACE]

  JMP SETCHAR     ; insert uppercase/graphic character and return

          ; character was $80 to $9F and is now $00 to $1F
LAB_E82A
  CMP #$0D      ; compare with [CR]
  BNE LAB_E831    ; branch if not [CR]

  JMP RTRN      ; else output [CR] and return

          ; was not [CR]
LAB_E831
  LDX QTSW      ; get cursor quote flag, $01 = quote, $00 = no quote
  BNE LAB_E874    ; branch if quote mode

  CMP #$14      ; compare with [DELETE]
  BNE LAB_E870    ; branch if not [DELETE]

  LDY LNMX      ; get current screen line length
  LDA (PNT),Y     ; get character from current screen line
  CMP #' '      ; compare with [SPACE]
  BNE LAB_E845    ; branch if not [SPACE]

  CPY PNTR      ; compare current column with cursor column
  BNE LAB_E84C    ; if not cursor column go open up space on line

LAB_E845
  CPY #$57      ; compare current column with max line length
  BEQ LAB_E86D    ; exit if at line end

  JSR OPENLIN     ; else open space on screen
          ; now open up space on the line to insert a character
LAB_E84C
  LDY LNMX      ; get current screen line length
  JSR COLORSYN    ; calculate pointer to colour RAM
LAB_E851
  DEY       ; decrement index to previous character
  LDA (PNT),Y     ; get character from current screen line
  INY       ; increment index to next character
  STA (PNT),Y     ; save character to current screen line
  DEY       ; decrement index to previous character
  LDA (USER),Y    ; get current screen line colour RAM byte
  INY       ; increment index to next character
  STA (USER),Y    ; save current screen line colour RAM byte
  DEY       ; decrement index to previous character
  CPY PNTR      ; compare with cursor column
  BNE LAB_E851    ; loop if not there yet

  LDA #' '      ; set [SPACE]
  STA (PNT),Y     ; clear character at cursor position on current screen line
  LDA COLOR     ; get current colour code
  STA (USER),Y    ; save to cursor position on current screen line colour RAM
  INC INSRT     ; increment insert count
LAB_E86D
  JMP LAB_E6DC    ; restore registers, set quote flag and exit

LAB_E870
  LDX INSRT     ; get insert count
  BEQ LAB_E879    ; branch if no insert space

LAB_E874
  ORA #$40      ; change to uppercase/graphic
  JMP LAB_E6CB    ; insert reversed character, scroll if needed and exit

LAB_E879
  CMP #$11      ; compare with [CURSOR UP]
  BNE LAB_E893    ; branch if not [CURSOR UP]

  LDX TBLX      ; get cursor row
  BEQ LAB_E8B8    ; branch if on top line

  DEC TBLX      ; decrement cursor row
  LDA PNTR      ; get cursor column
  SEC       ; set carry for subtract
  SBC #$16      ; subtract one line length
  BCC LAB_E88E    ; branch if stepped back to previous line

  STA PNTR      ; else save cursor column ..
  BPL LAB_E8B8    ; .. and exit, branch always

LAB_E88E
  JSR SETSLINK    ; set screen pointers for cursor row, column ..
  BNE LAB_E8B8    ; .. and exit, branch always

LAB_E893
  CMP #$12      ; compare with [RVS OFF]
  BNE LAB_E89B    ; branch if not [RVS OFF]

  LDA #$00      ; clear .A
  STA RVS     ; clear reverse flag
LAB_E89B
  CMP #$1D      ; compare with [CURSOR LEFT]
  BNE LAB_E8B1    ; branch if not [CURSOR LEFT]

  TYA       ; copy cursor column
  BEQ LAB_E8AB    ; branch if at start of line

  JSR BACKUP      ; test for line decrement
  DEY       ; decrement cursor column
  STY PNTR      ; save cursor column
  JMP LAB_E6DC    ; restore registers, set quote flag and exit

LAB_E8AB
  JSR RETREAT     ; back onto previous line if possible
  JMP LAB_E6DC    ; restore registers, set quote flag and exit

LAB_E8B1
  CMP #$13      ; compare with [CLR]
  BNE LAB_E8BB    ; branch if not [CLR]

  JSR CLSR      ; clear screen
LAB_E8B8
  JMP LAB_E6DC    ; restore registers, set quote flag and exit

LAB_E8BB
  ORA #$80      ; restore b7, colour can only be black, cyan, magenta
          ; or yellow, or [SWITCH TO UPPER CASE]
  JSR COLORSET    ; set the colour from the character in .A
  JMP GRAPHMODE   ; select VIC character set


;***********************************************************************************;
;
; do newline

NXTLINE
  LSR LXSP      ; shift >> input cursor row
  LDX TBLX      ; get cursor row
LAB_E8C7
  INX       ; increment row
  CPX #$17      ; compare with last row + 1
  BNE LAB_E8CF    ; branch if not last row + 1

  JSR SCRL      ; else scroll screen
LAB_E8CF
  LDA LDTB1,X     ; get start of line .X pointer high byte
  BPL LAB_E8C7    ; loop if not start of logical line

  STX TBLX      ; else save cursor row
  JMP SETSLINK    ; set screen pointers for cursor row, column and return


;***********************************************************************************;
;
; output [CR]

RTRN
  LDX #$00      ; clear .X
  STX INSRT     ; clear insert count
  STX RVS     ; clear reverse flag
  STX QTSW      ; clear cursor quote flag
  STX PNTR      ; clear cursor column
  JSR NXTLINE     ; do newline
  JMP LAB_E6DC    ; restore registers, set quote flag and exit


;***********************************************************************************;
;
; test for line decrement

BACKUP
  LDX #$04      ; set count
  LDA #$00      ; set column
LAB_E8EC
  CMP PNTR      ; compare with cursor column
  BEQ LAB_E8F7    ; branch if at start of line

  CLC       ; else clear carry for add
  ADC #$16      ; increment to next line
  DEX       ; decrement loop count
  BNE LAB_E8EC    ; loop if more to test

  RTS

LAB_E8F7
  DEC TBLX      ; else decrement cursor row
  RTS


;***********************************************************************************;
;
; test for line increment. if at end of line, but not at end of last line, increment the
; cursor row

FORWARD
  LDX #$04      ; set count
  LDA #$15      ; set column
LAB_E8FE
  CMP PNTR      ; compare with cursor column
  BEQ LAB_E909    ; if at end of line test and possibly increment cursor row

  CLC       ; else clear carry for add
  ADC #$16      ; increment to next line
  DEX       ; decrement loop count
  BNE LAB_E8FE    ; loop if more to test

  RTS

          ; cursor is at end of line
LAB_E909
  LDX TBLX      ; get cursor row
  CPX #$17      ; compare with end of screen
  BEQ LAB_E911    ; exit if end of screen

  INC TBLX      ; else increment cursor row
LAB_E911
  RTS


;***********************************************************************************;
;
; set colour code. enter with the colour character in .A. if .A does not contain a
; colour character this routine exits without changing the colour

COLORSET
  LDX #LAB_E928-COLORTBL
          ; set colour code count
LAB_E914
  CMP COLORTBL,X    ; compare the character with the table code
  BEQ LAB_E91D    ; if a match go save the colour and exit

  DEX       ; else decrement the index
  BPL LAB_E914    ; loop if more to do

  RTS

LAB_E91D
  STX COLOR     ; set current colour code
  RTS


;***********************************************************************************;
;
; ASCII colour code table
          ; CHR$()  colour
COLORTBL        ; ------  ------
  .byte $90     ;  144    black
  .byte $05     ;    5    white
  .byte $1C     ;   28    red
  .byte $9F     ;  159    cyan
  .byte $9C     ;  156    magenta
  .byte $1E     ;   30    green
  .byte $1F     ;   31    blue
LAB_E928
  .byte $9E     ;  158    yellow


;***********************************************************************************;
;
; code conversion, these don't seem to be used anywhere

;CNVRTCD
  .byte $EF,$A1,$DF,$A6,$E1,$B1,$E2,$B2,$E3,$B3,$E4,$B4,$E5,$B5,$E6,$B6
  .byte $E7,$B7,$E8,$B8,$E9,$B9,$FA,$BA,$FB,$BB,$FC,$BC,$EC,$BD,$FE,$BE
  .byte $84,$BF,$F7,$C0,$F8,$DB,$F9,$DD,$EA,$DE,$5E,$E0,$5B,$E1,$5D,$E2
  .byte $40,$B0,$61,$B1,$78,$DB,$79,$DD,$66,$B6,$77,$C0,$70,$F0,$71,$F1
  .byte $72,$F2,$73,$F3,$74,$F4,$75,$F5,$76,$F6,$7D,$FD


;***********************************************************************************;
;
; scroll screen

SCRL
  LDA SAL     ; copy tape buffer start pointer
  PHA       ; save it
  LDA SAL+1     ; copy tape buffer start pointer
  PHA       ; save it
  LDA EAL     ; copy tape buffer end pointer
  PHA       ; save it
  LDA EAL+1     ; copy tape buffer end pointer
  PHA       ; save it
LAB_E981
  LDX #$FF      ; set to -1 for pre increment loop
  DEC TBLX      ; decrement cursor row
  DEC LXSP      ; decrement input cursor row
  DEC LLNKSV      ; decrement screen row marker
LAB_E989
  INX       ; increment line number
  JSR LINPTR      ; set start of line .X
  CPX #$16      ; compare with last line
  BCS LAB_E99D    ; branch if >= $16

  LDA LDTB2+1,X   ; get start of next line pointer low byte
  STA SAL     ; save next line pointer low byte
  LDA LDTB1+1,X   ; get start of next line pointer high byte
  JSR MOVLIN      ; shift screen line up
  BMI LAB_E989    ; loop, branch always

LAB_E99D
  JSR CLRALINE    ; clear screen line .X

          ; now shift up the start of logical line bits
  LDX #$00      ; clear index
LAB_E9A2
  LDA LDTB1,X     ; get start of line .X pointer high byte
  AND #$7F      ; clear line .X start of logical line bit
  LDY LDTB1+1,X   ; get start of next line pointer high byte
  BPL LAB_E9AC    ; branch if next line not start of line

  ORA #$80      ; set line .X start of logical line bit
LAB_E9AC
  STA LDTB1,X     ; set start of line .X pointer high byte
  INX       ; increment line number
  CPX #$16      ; compare with last line
  BNE LAB_E9A2    ; loop if not last line

  LDA LDTB1+$16   ; get start of last line pointer high byte
  ORA #$80      ; mark as start of logical line
  STA LDTB1+$16   ; set start of last line pointer high byte
  LDA LDTB1     ; get start of first line pointer high byte
  BPL LAB_E981    ; if not start of logical line loop back and
          ; scroll the screen up another line

  INC TBLX      ; increment cursor row
  INC LLNKSV      ; increment screen row marker
  LDA #$FB      ; set keyboard column c2
  STA VIA2PB      ; set VIA 2 DRB, keyboard column
  LDA VIA2PA1     ; get VIA 2 DRA, keyboard row
  CMP #$FE      ; compare with row r0 active, [CTRL]
  PHP       ; save status
  LDA #$F7      ; set keyboard column c3
  STA VIA2PB      ; set VIA 2 DRB, keyboard column
  PLP       ; restore status
  BNE LAB_E9DF    ; skip delay if [CTRL] not pressed

          ; first time round the inner loop .X will be $16
  LDY #$00      ; clear delay outer loop count, do this 256 times
LAB_E9D6
  NOP       ; waste cycles
  DEX       ; decrement inner loop count
  BNE LAB_E9D6    ; loop if not all done

  DEY       ; decrement outer loop count
  BNE LAB_E9D6    ; loop if not all done

  STY NDX     ; clear keyboard buffer index
LAB_E9DF
  LDX TBLX      ; get cursor row
  PLA       ; pull tape buffer end pointer
  STA EAL+1     ; restore it
  PLA       ; pull tape buffer end pointer
  STA EAL     ; restore it
  PLA       ; pull tape buffer pointer
  STA SAL+1     ; restore it
  PLA       ; pull tape buffer pointer
  STA SAL     ; restore it
  RTS


;***********************************************************************************;
;
; open space on screen

OPENLIN
  LDX TBLX      ; get cursor row
LAB_E9F0
  INX       ; increment row
  LDA LDTB1,X     ; get start of line .X pointer high byte
  BPL LAB_E9F0    ; branch if not start of logical line

  STX LLNKSV      ; set screen row marker
  CPX #$16      ; compare with last line
  BEQ LAB_EA08    ; branch if = last line

  BCC LAB_EA08    ; branch if < last line

          ; else was > last line
  JSR SCRL      ; else scroll screen
  LDX LLNKSV      ; get screen row marker
  DEX       ; decrement screen row marker
  DEC TBLX      ; decrement cursor row
  JMP LAB_E70E    ; add this row to the current logical line and return

LAB_EA08
  LDA SAL     ; copy tape buffer pointer
  PHA       ; save it
  LDA SAL+1     ; copy tape buffer pointer
  PHA       ; save it
  LDA EAL     ; copy tape buffer end pointer
  PHA       ; save it
  LDA EAL+1     ; copy tape buffer end pointer
  PHA       ; save it
  LDX #$17      ; set to end line + 1 for predecrement loop
LAB_EA16
  DEX       ; decrement line number
  JSR LINPTR      ; set start of line .X
  CPX LLNKSV      ; compare with screen row marker
  BCC LAB_EA2C    ; branch if < screen row marker

  BEQ LAB_EA2C    ; branch if = screen row marker

  LDA LDTB2-1,X   ; else get start of previous line low byte from ROM table
  STA SAL     ; save previous line pointer low byte
  LDA LDTB1-1,X   ; get start of previous line pointer high byte
  JSR MOVLIN      ; shift screen line down
  BMI LAB_EA16    ; loop, branch always

LAB_EA2C
  JSR CLRALINE    ; clear screen line .X
  LDX #$15      ; set index to last screen row - 1
LAB_EA31
  CPX LLNKSV      ; compare with saved screen row
  BCC LAB_EA44    ; reached insertion point so stop

  LDA LDTB1+1,X   ; get start of line .X + 1 pointer high byte
  AND #$7F      ; mask start of logical line bit
  LDY LDTB1,X     ; get start of line .X pointer high byte
  BPL LAB_EA3F    ; branch if start of logical line bit clear

  ORA #$80      ; set start of logical line bit
LAB_EA3F
  STA LDTB1+1,X   ; update start of line .X + 1 pointer high byte
  DEX       ; decrement index
  BNE LAB_EA31    ; branch always

LAB_EA44
  LDX LLNKSV      ; get saved screen row
  JSR LAB_E70E    ; add this row to the current logical line
  PLA       ; pull tape buffer end pointer
  STA EAL+1     ; restore it
  PLA       ; pull tape buffer end pointer
  STA EAL     ; restore it
  PLA       ; pull tape buffer pointer
  STA SAL+1     ; restore it
  PLA       ; pull tape buffer pointer
  STA SAL     ; restore it
  RTS


;***********************************************************************************;
;
; shift screen line up/down

MOVLIN
  AND #$03      ; mask 0000 00xx, line memory page
  ORA HIBASE      ; OR with screen memory page
  STA SAL+1     ; save next/previous line pointer high byte
  JSR SETADDR     ; calculate pointers to screen lines colour RAM
LAB_EA60
  LDY #$15      ; set column count
LAB_EA62
  LDA (SAL),Y     ; get character from next/previous screen line
  STA (PNT),Y     ; save character to current screen line
  LDA (EAL),Y     ; get colour from next/previous screen line colour RAM
  STA (USER),Y    ; save colour to current screen line colour RAM
  DEY       ; decrement column index/count
  BPL LAB_EA62    ; loop if more to do

  RTS


;***********************************************************************************;
;
; calculate pointers to screen lines colour RAM

SETADDR
  JSR COLORSYN    ; calculate pointer to current screen line colour RAM
  LDA SAL     ; get next screen line pointer low byte
  STA EAL     ; save next screen line colour RAM pointer low byte
  LDA SAL+1     ; get next screen line pointer high byte
  AND #$03      ; mask 0000 00xx, line memory page
  ORA #$94      ; set  1001 01xx, colour memory page
  STA EAL+1     ; save next screen line colour RAM pointer high byte
  RTS


;***********************************************************************************;
;
; set start of line .X

LINPTR
  LDA LDTB2,X     ; get start of line low byte from ROM table
  STA PNT     ; set current screen line pointer low byte
  LDA LDTB1,X     ; get start of line high byte from RAM table
  AND #$03      ; mask 0000 00xx, line memory page
  ORA HIBASE      ; OR with screen memory page
  STA PNT+1     ; set current screen line pointer high byte
  RTS


;***********************************************************************************;
;
; clear screen line .X

CLRALINE
  LDY #$15      ; set number of columns to clear
  JSR LINPTR      ; set start of line .X
  JSR COLORSYN    ; calculate pointer to colour RAM
LAB_EA95
  LDA #' '      ; set [SPACE]
  STA (PNT),Y     ; clear character in current screen line
  LDA #$01      ; set colour, blue on white
  STA (USER),Y    ; set colour RAM in current screen line
  DEY       ; decrement index
  BPL LAB_EA95    ; loop if more to do

  RTS


;***********************************************************************************;
;
; print character .A and colour .X to screen

SYNPRT
  TAY       ; copy character
  LDA #$02      ; set count to $02, usually $14 ??
  STA BLNCT     ; set cursor countdown
  JSR COLORSYN    ; calculate pointer to colour RAM
  TYA       ; get character back

; save character and colour to screen @ cursor

PUTSCRN
  LDY PNTR      ; get cursor column
  STA (PNT),Y     ; save character from current screen line
  TXA       ; copy colour to .A
  STA (USER),Y    ; save to colour RAM
  RTS


;***********************************************************************************;
;
; calculate pointer to colour RAM

COLORSYN
  LDA PNT     ; get current screen line pointer low byte
  STA USER      ; save pointer to colour RAM low byte
  LDA PNT+1     ; get current screen line pointer high byte
  AND #$03      ; mask 0000 00xx, line memory page
  ORA #$94      ; set  1001 01xx, colour memory page
  STA USER+1      ; save pointer to colour RAM high byte
  RTS


;***********************************************************************************;
;
; update the clock, flash the cursor, control the cassette and scan the keyboard

; IRQ handler

IRQ
  JSR UDTIM     ; increment real time clock
  LDA BLNSW     ; get cursor enable
  BNE LAB_EAEF    ; branch if not flash cursor

  DEC BLNCT     ; else decrement cursor timing countdown
  BNE LAB_EAEF    ; branch if not done

  LDA #$14      ; set count
  STA BLNCT     ; save cursor timing countdown
  LDY PNTR      ; get cursor column
  LSR BLNON     ; shift b0 cursor blink phase into carry
  LDX GDCOL     ; get colour under cursor
  LDA (PNT),Y     ; get character from current screen line
  BCS LAB_EAEA    ; branch if cursor phase b0 was 1

  INC BLNON     ; set cursor blink phase to 1
  STA CDBLN     ; save character under cursor
  JSR COLORSYN    ; calculate pointer to colour RAM
  LDA (USER),Y    ; get colour RAM byte
  STA GDCOL     ; save colour under cursor
  LDX COLOR     ; get current colour code
  LDA CDBLN     ; get character under cursor
LAB_EAEA
  EOR #$80      ; toggle b7 of character under cursor
  JSR PUTSCRN     ; save character and colour to screen @ cursor
LAB_EAEF
  LDA VIA1PA2     ; get VIA 1 DRA, no handshake
  AND #$40      ; mask cassette switch sense
  BEQ LAB_EB01    ; branch if cassette sense low

          ; cassette sense was high so turn off motor and clear
          ; the interlock
  LDY #$00      ; clear .Y
  STY CAS1      ; clear the tape motor interlock
  LDA VIA1PCR     ; get VIA 1 PCR
  ORA #$02      ; set CA2 high, turn off motor
  BNE LAB_EB0A    ; branch always

          ; cassette sense was low so turn on motor, perhaps
LAB_EB01
  LDA CAS1      ; get tape motor interlock
  BNE LAB_EB12    ; if cassette interlock <> 0 don't turn on motor

  LDA VIA1PCR     ; get VIA 1 PCR
  AND #$FD      ; set CA2 low, turn on motor
LAB_EB0A
  BIT VIA1IER     ; test VIA 1 IER
  BVS LAB_EB12    ; if T1 interrupt enabled don't change motor state

  STA VIA1PCR     ; set VIA 1 PCR, set CA2 high/low
LAB_EB12
  JSR FSCNKEY     ; scan keyboard
  BIT VIA2T1CL    ; test VIA 2 T1C_l, clear the timer interrupt flag
  PLA       ; pull .Y
  TAY       ; restore .Y
  PLA       ; pull .X
  TAX       ; restore .X
  PLA       ; restore .A
  RTI


;***********************************************************************************;
;
; scan keyboard performs the following ..
;
; 1)  check if key pressed, if not then exit the routine
;
; 2)  init I/O ports of VIA 2 for keyboard scan and set pointers to decode table 1.
; clear the character counter
;
; 3)  set one line of port B low and test for a closed key on port A by shifting the
; byte read from the port. if the carry is clear then a key is closed so save the
; count which is incremented on each shift. check for SHIFT/STOP/C= keys and
; flag if closed
;
; 4)  repeat step 3 for the whole matrix
;
; 5)  evaluate the SHIFT/CTRL/C= keys, this may change the decode table selected
;
; 6)  use the key count saved in step 3 as an index into the table selected in step 5
;
; 7)  check for key repeat operation
;
; 8)  save the decoded key to the buffer if first press or repeat

; scan keyboard

; This routine will scan the keyboard and check for pressed keys. It is the same
; routine called by the interrupt handler. If a key is down, its ASCII value is
; placed in the keyboard queue.

FSCNKEY
  LDA #$00      ; clear .A
  STA SHFLAG      ; clear keyboard shift/control/C= flag
  LDY #$40      ; set no key
  STY SFDX      ; save which key
  STA VIA2PB      ; clear VIA 2 DRB, keyboard column
  LDX VIA2PA1     ; get VIA 2 DRA, keyboard row
  CPX #$FF      ; compare with all bits set
  BEQ LAB_EB8F    ; if no key pressed clear current key and exit (does
          ; further BEQ to LAB_EBBA)

  LDA #$FE      ; set column 0 low
  STA VIA2PB      ; set VIA 2 DRB, keyboard column
  LDY #$00      ; clear key count
  LDA #<NORMKEYS    ; get decode table low byte
  STA KEYTAB      ; set keyboard pointer low byte
  LDA #>NORMKEYS    ; get decode table high byte
  STA KEYTAB+1    ; set keyboard pointer high byte
LAB_EB40
  LDX #$08      ; set row count
  LDA VIA2PA1     ; get VIA 2 DRA, keyboard row
  CMP VIA2PA1     ; compare with itself
  BNE LAB_EB40    ; loop if changing

LAB_EB4A
  LSR       ; shift row to Cb
  BCS LAB_EB63    ; if no key closed on this row go do next row

  PHA       ; save row
  LDA (KEYTAB),Y    ; get character from decode table
  CMP #$05      ; compare with $05, there is no $05 key but the control
          ; keys are all less than $05
  BCS LAB_EB60    ; if not shift/control/C=/stop go save key count

          ; else was shift/control/C=/stop key
  CMP #$03      ; compare with $03, stop
  BEQ LAB_EB60    ; if stop go save key count and continue

          ; character is $01 - shift, $02 - C= or $04 - control
  ORA SHFLAG      ; OR keyboard shift/control/C= flag
  STA SHFLAG      ; save keyboard shift/control/C= flag
  BPL LAB_EB62    ; skip save key, branch always

LAB_EB60
  STY SFDX      ; save which key
LAB_EB62
  PLA       ; restore row
LAB_EB63
  INY       ; increment key count
  CPY #$41      ; compare with max+1
  BCS LAB_EB71    ; exit loop if >= max+1

          ; else still in matrix
  DEX       ; decrement row count
  BNE LAB_EB4A    ; loop if more rows to do

  SEC       ; set carry for keyboard column shift
  ROL VIA2PB      ; shift VIA 2 DRB, keyboard column
  BNE LAB_EB40    ; loop for next column, branch always

LAB_EB71
  JMP (KEYLOG)    ; evaluate the SHIFT/CTRL/C= keys, SETKEYS

; key decoding continues here after the SHIFT/CTRL/C= keys are evaluated

LAB_EB74
  LDY SFDX      ; get which key
  LDA (KEYTAB),Y    ; get character from decode table
  TAX       ; copy character to .X
  CPY LSTX      ; compare which key with last key
  BEQ LAB_EB84    ; if this key = current key, key held, go test repeat

  LDY #$10      ; set repeat delay count
  STY DELAY     ; save repeat delay count
  BNE LAB_EBBA    ; go save key to buffer and exit, branch always

LAB_EB84
  AND #$7F      ; clear b7
  BIT RPTFLG      ; test key repeat
  BMI LAB_EBA1    ; branch if repeat all

  BVS LAB_EBD6    ; branch if repeat none

  CMP #$7F      ; compare with end marker
LAB_EB8F
  BEQ LAB_EBBA    ; if $00/end marker go save key to buffer and exit

  CMP #$14      ; compare with [INSERT]/[DELETE]
  BEQ LAB_EBA1    ; if [INSERT]/[DELETE] go test for repeat

  CMP #' '      ; compare with [SPACE]
  BEQ LAB_EBA1    ; if [SPACE] go test for repeat

  CMP #$1D      ; compare with [CURSOR RIGHT]/[CURSOR LEFT]
  BEQ LAB_EBA1    ; if [CURSOR RIGHT]/[CURSOR LEFT] go test for repeat

  CMP #$11      ; compare with [CURSOR DOWN]/[CURSOR UP]
  BNE LAB_EBD6    ; if not [CURSOR DOWN]/[CURSOR UP] just exit

          ; was one of the cursor movement keys, insert/delete
          ; key or the space bar so always do repeat tests
LAB_EBA1
  LDY DELAY     ; get repeat delay counter
  BEQ LAB_EBAB    ; branch if delay expired

  DEC DELAY     ; else decrement repeat delay counter
  BNE LAB_EBD6    ; branch if delay not expired

          ; repeat delay counter has expired
LAB_EBAB
  DEC KOUNT     ; decrement repeat speed counter
  BNE LAB_EBD6    ; branch if repeat speed count not expired

  LDY #$04      ; set for 4/60ths of a second
  STY KOUNT     ; set repeat speed counter
  LDY NDX     ; get keyboard buffer index
  DEY       ; decrement it
  BPL LAB_EBD6    ; if the buffer isn't empty just exit

          ; else repeat the key immediately

; Possibly save the key to the keyboard buffer. If there was no key pressed or the key
; was not found during the scan (possibly due to key bounce) then .X will be $FF here.

LAB_EBBA
  LDY SFDX      ; get which key
  STY LSTX      ; save as last key pressed
  LDY SHFLAG      ; get keyboard shift/control/C= flag
  STY LSTSHF      ; save as last keyboard shift pattern
  CPX #$FF      ; compare character with table end marker or no key
  BEQ LAB_EBD6    ; if table end marker or no key just exit

  TXA       ; copy character to .A
  LDX NDX     ; get keyboard buffer index
  CPX XMAX      ; compare with keyboard buffer size
  BCS LAB_EBD6    ; if buffer full just exit

  STA KEYD,X      ; save character to keyboard buffer
  INX       ; increment index
  STX NDX     ; save keyboard buffer index
LAB_EBD6
  LDA #$F7      ; enable column 3 for stop key
  STA VIA2PB      ; set VIA 2 DRB, keyboard column
  RTS

; evaluate SHIFT/CTRL/C= keys
;
; 0 $00 EC5E
; 1 $02 EC9F
; 2 $04 ECE0
; 3 ..  ....
; 4 $06 EDA3
; 5 $06 EDA3
; 6 $06 EDA3
; 7 $06 EDA3

SETKEYS
  LDA SHFLAG      ; get keyboard shift/control/C= flag
  CMP #$03      ; compare with [SHIFT][C=]
  BNE LAB_EC0F    ; branch if not [SHIFT][C=]

  CMP LSTSHF      ; compare with last
  BEQ LAB_EBD6    ; exit if still the same

  LDA MODE      ; get shift mode switch $00 = enabled, $80 = locked
  BMI LAB_EC43    ; if locked continue keyboard decode

  NOP       ; just a few wasted cycles
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;

          ; toggle text mode
  LDA VICCR5      ; get start of character memory, ROM
  EOR #$02      ; toggle $8000,$8800
  STA VICCR5      ; set start of character memory, ROM
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  JMP LAB_EC43    ; continue keyboard decode

          ; was not [SHIFT][C=] but could be any other combination
LAB_EC0F
  ASL       ; << 1
  CMP #$08      ; compare with [CTRL]
  BCC LAB_EC18    ; branch if not [CTRL] pressed

  LDA #$06      ; else [CTRL] was pressed so make index = $06
  NOP       ;
  NOP       ;
LAB_EC18
  NOP       ; just a few wasted cycles
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  NOP       ;
  TAX       ; copy index to .X
  LDA KEYVCTRS,X    ; get decode table pointer low byte
  STA KEYTAB      ; save decode table pointer low byte
  LDA KEYVCTRS+1,X    ; get decode table pointer high byte
  STA KEYTAB+1    ; save decode table pointer high byte
LAB_EC43
  JMP LAB_EB74    ; continue keyboard decode


;***********************************************************************************;
;
; keyboard decode table pointers

KEYVCTRS
  .word NORMKEYS    ; unshifted
  .word SHFTKEYS    ; shifted
  .word LOGOKEYS    ; commodore
  .word CTRLKEYS    ; control
  .word NORMKEYS    ; unshifted
  .word SHFTKEYS    ; shifted
  .word WHATKEYS    ; shifted
  .word CTRLKEYS    ; control
  .word CHARSET     ; graphics/text control
  .word WHATKEYS    ; shifted
  .word WHATKEYS    ; shifted
  .word CTRLKEYS    ; control

; keyboard decode table - unshifted

NORMKEYS
  .byte $31,$33,$35,$37,$39,$2B,$5C,$14
  .byte $5F,$57,$52,$59,$49,$50,$2A,$0D
  .byte $04,$41,$44,$47,$4A,$4C,$3B,$1D
  .byte $03,$01,$58,$56,$4E,$2C,$2F,$11
  .byte $20,$5A,$43,$42,$4D,$2E,$01,$85
  .byte $02,$53,$46,$48,$4B,$3A,$3D,$86
  .byte $51,$45,$54,$55,$4F,$40,$5E,$87
  .byte $32,$34,$36,$38,$30,$2D,$13,$88
  .byte $FF

; keyboard decode table - shifted

SHFTKEYS
  .byte $21,$23,$25,$27,$29,$DB,$A9,$94
  .byte $5F,$D7,$D2,$D9,$C9,$D0,$C0,$8D
  .byte $04,$C1,$C4,$C7,$CA,$CC,$5D,$9D
  .byte $83,$01,$D8,$D6,$CE,$3C,$3F,$91
  .byte $A0,$DA,$C3,$C2,$CD,$3E,$01,$89
  .byte $02,$D3,$C6,$C8,$CB,$5B,$3D,$8A
  .byte $D1,$C5,$D4,$D5,$CF,$BA,$DE,$8B
  .byte $22,$24,$26,$28,$30,$DD,$93,$8C
  .byte $FF

; keyboard decode table - commodore

LOGOKEYS
  .byte $21,$23,$25,$27,$29,$A6,$A8,$94
  .byte $5F,$B3,$B2,$B7,$A2,$AF,$DF,$8D
  .byte $04,$B0,$AC,$A5,$B5,$B6,$5D,$9D
  .byte $83,$01,$BD,$BE,$AA,$3C,$3F,$91
  .byte $A0,$AD,$BC,$BF,$A7,$3E,$01,$89
  .byte $02,$AE,$BB,$B4,$A1,$5B,$3D,$8A
  .byte $AB,$B1,$A3,$B8,$B9,$A4,$DE,$8B
  .byte $22,$24,$26,$28,$30,$DC,$93,$8C
  .byte $FF


;***********************************************************************************;
;
; select VIC character set

CHARSET
  CMP #$0E      ; compare with [SWITCH TO LOWER CASE]
  BNE GRAPHMODE   ; branch if not [SWITCH TO LOWER CASE]

  LDA #$02      ; set for $8800, lower case characters
  ORA VICCR5      ; OR with start of character memory, ROM
  STA VICCR5      ; save start of character memory, ROM
  JMP LAB_E6DC    ; restore registers, set quote flag and exit

GRAPHMODE
  CMP #$8E      ; compare with [SWITCH TO UPPER CASE]
  BNE LAB_ED3F    ; branch if not [SWITCH TO UPPER CASE]

  LDA #$FD      ; set for $8000, upper case characters
  AND VICCR5      ; AND with start of character memory, ROM
  STA VICCR5      ; save start of character memory, ROM

LAB_ED3C
  JMP LAB_E6DC    ; restore registers, set quote flag and exit

LAB_ED3F
  CMP #$08      ; compare with disable [SHIFT][C=]
  BNE LAB_ED4D    ; branch if not disable [SHIFT][C=]

  LDA #$80      ; set to lock shift mode switch
  ORA MODE      ; OR with shift mode switch, $00 = enabled, $80 = locked
  STA MODE      ; save shift mode switch
  BMI LAB_ED3C    ; branch always

LAB_ED4D
  CMP #$09      ; compare with enable [SHIFT][C=]
  BNE LAB_ED3C    ; exit if not enable [SHIFT][C=]

  LDA #$7F      ; set to unlock shift mode switch
  AND MODE      ; AND with shift mode switch, $00 = enabled, $80 = locked
  STA MODE      ; save shift mode switch
  BPL LAB_ED3C    ; branch always

; make next screen line start of logical line, increment line length and set pointers

WRAPLINE
  INX       ; increment screen row
  LDA LDTB1,X     ; get start of line X pointer high byte
  ORA #$80      ; mark as start of logical line
  STA LDTB1,X     ; set start of line X pointer high byte
  DEX       ; restore screen row
  LDA LNMX      ; get current screen line length
  CLC       ; clear carry for add
  JMP LAB_E715    ; add one line length, set pointers for start of line and
          ; return


;***********************************************************************************;
;
; keyboard decode table - shifted

WHATKEYS
  .byte $FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF
  .byte $FF,$04,$FF,$FF,$FF,$FF,$FF,$E2
  .byte $9D,$83,$01,$FF,$FF,$FF,$FF,$FF
  .byte $91,$A0,$FF,$FF,$FF,$FF,$EE,$01
  .byte $89,$02,$FF,$FF,$FF,$FF,$E1,$FD
  .byte $8A,$FF,$FF,$FF,$FF,$FF,$B0,$E0
  .byte $8B,$F2,$F4,$F6,$FF,$F0,$ED,$93
  .byte $8C,$FF

; keyboard decode table - control

CTRLKEYS
  .byte $90,$1C,$9C,$1F,$12,$FF,$FF,$FF
  .byte $06,$FF,$12,$FF,$FF,$FF,$FF,$FF
  .byte $FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF
  .byte $FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF
  .byte $FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF
  .byte $FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF
  .byte $FF,$FF,$FF,$FF,$FF,$FF,$FF,$FF
  .byte $05,$9F,$1E,$9E,$92,$FF,$FF,$FF
  .byte $FF


;***********************************************************************************;
;
; initial values for VIC registers

VICINIT
  .byte $0C     ; interlace and horizontal origin [PAL]
; .byte $05     ; interlace and horizontal origin [NTSC]
          ; bit function
          ; --- --------
          ;  7  interlace / non interlace
          ; 6-0 horizontal origin
  .byte $26     ; vertical origin [PAL]
; .byte $19     ; vertical origin [NTSC]
  .byte $16     ; video address and columns, $9400 for colour RAM
          ; bit function
          ; --- --------
          ;  7  video memory address va9
          ; 6-0 number of columns
  .byte $2E     ; screen rows and character height
          ; bit function
          ; --- --------
          ;  7  raster line b0
          ; 6-1 number of rows
          ;  0  character height (8/16 bits)
  .byte $00     ; b8-b1 raster line
  .byte $C0     ; video memory addresses, RAM $1000, ROM $8000
          ; bit function
          ; --- --------
          ; 7-4 video memory address va13-va10
          ; 3-0 character memory address va13-va10

          ; 0000 ROM  $8000 set 1 - we use this
          ; 0001  " $8400
          ; 0010  " $8800 set 2
          ; 0011  " $8C00
          ; 1100 RAM  $1000
          ; 1101  " $1400
          ; 1110  " $1800
          ; 1111  " $1C00

  .byte $00     ; light pen horizontal position
  .byte $00     ; light pen vertical position

  .byte $00     ; paddle X
  .byte $00     ; paddle Y
  .byte $00     ; oscillator 1 frequency
  .byte $00     ; oscillator 2 frequency
  .byte $00     ; oscillator 3 frequency
  .byte $00     ; white noise frequency
  .byte $00     ; auxiliary colour and volume
          ; bit function
          ; --- --------
          ; 7-4 auxiliary colour
          ; 3-0 volume
  .byte $1B     ; background and border colour
          ; bit function
          ; --- --------
          ; 7-4 background colour
          ;  3  reverse video
          ; 2-0 border colour


;***********************************************************************************;
;
; keyboard buffer for auto load/run

RUNTB
  .byte "LOAD",$0D,"RUN",$0D


;***********************************************************************************;
;
; low byte screen line addresses

LDTB2
  .byte $00,$16,$2C,$42
  .byte $58,$6E,$84,$9A
  .byte $B0,$C6,$DC,$F2
  .byte $08,$1E,$34,$4A
  .byte $60,$76,$8C,$A2
  .byte $B8,$CE,$E4


;***********************************************************************************;
;
; command a serial bus device to TALK

; To use this routine the accumulator must first be loaded with a device number
; between 4 and 30. When called this routine converts this device number to a talk
; address. Then this data is transmitted as a command on the Serial bus.

FTALK
  ORA #$40      ; OR with the TALK command
  .byte $2C     ; makes next line BIT $2009


;***********************************************************************************;
;
; command devices on the serial bus to LISTEN

; This routine will command a device on the serial bus to receive data. The
; accumulator must be loaded with a device number between 4 and 30 before calling
; this routine. LISTEN convert this to a listen address then transmit this data as
; a command on the serial bus. The specified device will then go into listen mode
; and be ready to accept information.

FLISTEN
  ORA #$20      ; OR with the LISTEN command
  JSR RSPAUSE     ; check RS-232 bus idle


;***********************************************************************************;
;
; send control character

LIST1
  PHA       ; save device address
  BIT C3PO      ; test deferred character flag
  BPL LAB_EE2B    ; branch if no deferred character

  SEC       ; flag EOI
  ROR PCNTR     ; rotate into EOI flag
  JSR SRSEND      ; Tx byte on serial bus
  LSR C3PO      ; clear deferred character flag
  LSR PCNTR     ; clear EOI flag
LAB_EE2B
  PLA       ; restore device address
  STA BSOUR     ; save as serial deferred character
  JSR SEROUT1     ; set serial data out high
  CMP #$3F      ; compare read byte with $3F
  BNE LAB_EE38    ; branch if not $3F, this branch will always be taken as
          ; after VIA 2's PCR is read it is ANDed with $DF, so the
          ; result can never be $3F

  JSR SRCLKHI     ; set serial clock high
LAB_EE38
  LDA VIA1PA2     ; get VIA 1 DRA, no handshake
  ORA #$80      ; set serial ATN low
  STA VIA1PA2     ; set VIA 1 DRA, no handshake


;***********************************************************************************;
;
; if the code drops through to here the serial clock is low and the serial data has been
; released so the following code will have no effect apart from delaying the first byte
; by 1ms

; set clk/data, wait and Tx byte on serial bus

LAB_EE40
  JSR SRCLKLO     ; set serial clock low
  JSR SEROUT1     ; set serial data out high
  JSR WAITABIT    ; 1ms delay


;***********************************************************************************;
;
; Tx byte on serial bus

SRSEND
  SEI       ; disable interrupts
  JSR SEROUT1     ; set serial data out high
  JSR SERGET      ; get serial clock status
  LSR       ; shift serial data to Cb
  BCS SRBAD     ; if data high do device not present

  JSR SRCLKHI     ; set serial clock high
  BIT PCNTR     ; test EOI flag
  BPL LAB_EE66    ; branch if not EOI

; I think this is the EOI sequence so the serial clock has been released and the serial
; data is being held low by the peripherals. First up wait for the serial data to rise.

LAB_EE5A
  JSR SERGET      ; get serial clock status
  LSR       ; shift serial data to Cb
  BCC LAB_EE5A    ; loop if data low

; Now the data is high, EOI is signalled by waiting for at least 200us without pulling
; the serial clock line low again. The listener should respond by pulling the serial
; data line low.

LAB_EE60
  JSR SERGET      ; get serial clock status
  LSR       ; shift serial data to Cb
  BCS LAB_EE60    ; loop if data high

; The serial data has gone low ending the EOI sequence, now just wait for the serial
; data line to go high again or, if this isn't an EOI sequence, just wait for the serial
; data to go high the first time.

LAB_EE66
  JSR SERGET      ; get serial clock status
  LSR       ; shift serial data to Cb
  BCC LAB_EE66    ; loop if data low

; serial data is high now pull the clock low, preferably within 60us

  JSR SRCLKLO     ; set serial clock low

; Now the VIC has to send the eight bits, LSB first. First it sets the serial data line
; to reflect the bit in the byte, then it sets the serial clock to high. The serial
; clock is left high for 26 cycles, 23us on a PAL VIC, before it is again pulled low
; and the serial data is allowed high again.

  LDA #$08      ; eight bits to do
  STA CNTDN     ; set serial bus bit count
LAB_EE73
  LDA VIA1PA2     ; get VIA 1 DRA, no handshake
  CMP VIA1PA2     ; compare with self
  BNE LAB_EE73    ; loop if changing

  LSR       ; serial clock to carry
  LSR       ; serial data to carry
  BCC LAB_EEB7    ; if data low do timeout on serial bus

  ROR BSOUR     ; rotate transmit byte
  BCS LAB_EE88    ; branch if bit = 1

  JSR SEROUT0     ; else set serial data out low
  BNE LAB_EE8B    ; branch always

LAB_EE88
  JSR SEROUT1     ; set serial data out high
LAB_EE8B
  JSR SRCLKHI     ; set serial clock high
  NOP       ; waste ..
  NOP       ; .. a ..
  NOP       ; .. cycle ..
  NOP       ; .. or two
  LDA VIA2PCR     ; get VIA 2 PCR
  AND #$DF      ; set CB2 low, serial data out high
  ORA #$02      ; set CA2 high, serial clock out low
  STA VIA2PCR     ; save VIA 2 PCR
  DEC CNTDN     ; decrement serial bus bit count
  BNE LAB_EE73    ; loop if not all done

; Now all eight bits have been sent it's up to the peripheral to signal the byte was
; received by pulling the serial data low. This should be done within one millisecond.

  LDA #$04      ; wait for up to about 1ms
  STA VIA2T2CH    ; set VIA 2 T2C_h
LAB_EEA5
  LDA VIA2IFR     ; get VIA 2 IFR
  AND #$20      ; mask T2 interrupt
  BNE LAB_EEB7    ; if T2 interrupt do timeout on serial bus

  JSR SERGET      ; get serial clock status
  LSR       ; shift serial data to Cb
  BCS LAB_EEA5    ; if data high go wait some more

  CLI       ; enable interrupts
  RTS


;***********************************************************************************;
;
; device not present

SRBAD
  LDA #$80      ; set device not present bit
  .byte $2C     ; makes next line BIT $03A9


;***********************************************************************************;
;
; timeout on serial bus

LAB_EEB7
  LDA #$03      ; set time out read and write bits
LAB_EEB9
  JSR ORIOST      ; OR into I/O status byte
  CLI       ; enable interrupts
  CLC       ; clear for branch
  BCC LAB_EF09    ; ATN high, delay, clock high then data high, branch always


;***********************************************************************************;
;
; send secondary address after LISTEN

; This routine is used to send a secondary address to an I/O device after a call to
; the LISTEN routine is made and the device commanded to LISTEN. The routine cannot
; be used to send a secondary address after a call to the TALK routine.

; A secondary address is usually used to give set-up information to a device before
; I/O operations begin.

; When a secondary address is to be sent to a device on the serial bus the address
; must first be ORed with $60.

FSECOND
  STA BSOUR     ; save deferred byte
  JSR LAB_EE40    ; set clk/data, wait and Tx byte on serial bus

; set serial ATN high

SCATN
  LDA VIA1PA2     ; get VIA 1 DRA, no handshake
  AND #$7F      ; set serial ATN high
  STA VIA1PA2     ; set VIA 1 DRA, no handshake
  RTS


;***********************************************************************************;
;
; send secondary address after TALK

; This routine transmits a secondary address on the serial bus for a TALK device.
; This routine must be called with a number between 4 and 30 in the accumulator.
; The routine will send this number as a secondary address command over the serial
; bus. This routine can only be called after a call to the TALK routine. It will
; not work after a LISTEN.

; A secondary address is usually used to give set-up information to a device before
; I/O operations begin.

; When a secondary address is to be sent to a device on the serial bus the address
; must first be ORed with $60.

FTKSA
  STA BSOUR     ; save the secondary address byte to transmit
  JSR LAB_EE40    ; set clk/data, wait and Tx byte on serial bus


;***********************************************************************************;
;
; wait for bus end after send

LAB_EED3
  SEI       ; disable interrupts
  JSR SEROUT0     ; set serial data out low
  JSR SCATN     ; set serial ATN high
  JSR SRCLKHI     ; set serial clock high
LAB_EEDD
  JSR SERGET      ; get serial clock status
  BCS LAB_EEDD    ; branch if clock high

  CLI       ; enable interrupts
  RTS


;***********************************************************************************;
;
; output a byte to the serial bus

; This routine is used to send information to devices on the serial bus. A call to
; this routine will put a data byte onto the serial bus using full handshaking.
; Before this routine is called the LISTEN routine must be used to command a device
; on the serial bus to get ready to receive data.

; The accumulator is loaded with a byte to output as data on the serial bus. A
; device must be listening or the status word will return a timeout. This routine
; always buffers one character. So when a call to the UNLSN routine is made to end
; the data transmission, the buffered character is sent with EOI set. Then the
; UNLISTEN command is sent to the device.

FCIOUT
  BIT C3PO      ; test deferred character flag
  BMI LAB_EEED    ; branch if deferred character

  SEC       ; set carry
  ROR C3PO      ; shift into deferred character flag
  BNE LAB_EEF2    ; save byte and exit, branch always

LAB_EEED
  PHA       ; save byte
  JSR SRSEND      ; Tx byte on serial bus
  PLA       ; restore byte
LAB_EEF2
  STA BSOUR     ; save deferred byte
  CLC       ; flag ok
  RTS


;***********************************************************************************;
;
; command the serial bus to UNTALK

; This routine will transmit an UNTALK command on the serial bus. All devices
; previously set to TALK will stop sending data when this command is received.

FUNTLK
  JSR SRCLKLO     ; set serial clock low
  LDA VIA1PA2     ; get VIA 1 DRA, no handshake
  ORA #$80      ; set serial ATN low
  STA VIA1PA2     ; set VIA 1 DRA, no handshake

  LDA #$5F      ; set the UNTALK command
  .byte $2C     ; makes next line BIT $3FA9


;***********************************************************************************;
;
; command the serial bus to UNLISTEN

; This routine commands all devices on the serial bus to stop receiving data from
; the computer. Calling this routine results in an UNLISTEN command being transmitted
; on the serial bus. Only devices previously commanded to listen will be affected.

; This routine is normally used after the computer is finished sending data to
; external devices. Sending the UNLISTEN will command the listening devices to get
; off the serial bus so it can be used for other purposes.

FUNLSN
  LDA #$3F      ; set the UNLISTEN command
  JSR LIST1     ; send control character

; ATN high, delay, clock high then data high

LAB_EF09
  JSR SCATN     ; set serial ATN high

; 1ms delay, clock high then data high

LAB_EF0C
  TXA       ; save device number
  LDX #$0B      ; short delay
LAB_EF0F
  DEX       ; decrement count
  BNE LAB_EF0F    ; loop if not all done

  TAX       ; restore device number
  JSR SRCLKHI     ; set serial clock high
  JMP SEROUT1     ; set serial data out high and return


;***********************************************************************************;
;
; input a byte from the serial bus

; This routine reads a byte of data from the serial bus using full handshaking. The
; data is returned in the accumulator. Before using this routine the TALK routine
; must have been called first to command the device on the serial bus to send data on
; the bus. If the input device needs a secondary command it must be sent by using the
; TKSA routine before calling this routine.

; Errors are returned in the status word which can be read by calling the READST
; routine.

FACPTR
  SEI       ; disable interrupts
  LDA #$00      ; clear .A
  STA CNTDN     ; clear serial bus bit count
  JSR SRCLKHI     ; set serial clock high
LAB_EF21
  JSR SERGET      ; get serial clock status
  BCC LAB_EF21    ; loop while clock low

  JSR SEROUT1     ; set serial data out high
LAB_EF29
  LDA #$01      ; set timeout count high byte
  STA VIA2T2CH    ; set VIA 2 T2C_h
LAB_EF2E
  LDA VIA2IFR     ; get VIA 2 IFR
  AND #$20      ; mask T2 interrupt
  BNE LAB_EF3C    ; branch if T2 interrupt

  JSR SERGET      ; get serial clock status
  BCS LAB_EF2E    ; loop if clock high

  BCC LAB_EF54    ; else go set 8 bits to do, branch always

          ; T2 timed out
LAB_EF3C
  LDA CNTDN     ; get serial bus bit count
  BEQ LAB_EF45    ; if not already EOI then go flag EOI

  LDA #$02      ; error $02, read timeout
  JMP LAB_EEB9    ; set I/O status and exit

LAB_EF45
  JSR SEROUT0     ; set serial data out low
  JSR LAB_EF0C    ; 1ms delay, clock high then data high
  LDA #$40      ; set EOI bit
  JSR ORIOST      ; OR into I/O status byte
  INC CNTDN     ; increment serial bus bit count, do error on next timeout
  BNE LAB_EF29    ; go try again

LAB_EF54
  LDA #$08      ; 8 bits to do
  STA CNTDN     ; set serial bus bit count
LAB_EF58
  LDA VIA1PA2     ; get VIA 1 DRA, no handshake
  CMP VIA1PA2     ; compare with self
  BNE LAB_EF58    ; loop if changing

  LSR       ; serial clock into carry
  BCC LAB_EF58    ; loop while serial clock low

  LSR       ; serial data into carry
  ROR FIRT      ; shift data bit into input byte
LAB_EF66
  LDA VIA1PA2     ; get VIA 1 DRA, no handshake
  CMP VIA1PA2     ; compare with self
  BNE LAB_EF66    ; loop if changing

  LSR       ; serial clock into carry
  BCS LAB_EF66    ; loop while serial clock high

  DEC CNTDN     ; decrement serial bus bit count
  BNE LAB_EF58    ; loop if not all done

  JSR SEROUT0     ; set serial data out low
  LDA STATUS      ; get I/O status byte
  BEQ LAB_EF7F    ; branch if no error

  JSR LAB_EF0C    ; 1ms delay, clock high then data high
LAB_EF7F
  LDA FIRT      ; get input byte
  CLI       ; enable interrupts
  CLC
  RTS


;***********************************************************************************;
;
; set serial clock high

SRCLKHI
  LDA VIA2PCR     ; get VIA 2 PCR
  AND #$FD      ; set CA2 low, serial clock out high
  STA VIA2PCR     ; set VIA 2 PCR
  RTS


;***********************************************************************************;
;
; set serial clock low

SRCLKLO
  LDA VIA2PCR     ; get VIA 2 PCR
  ORA #$02      ; set CA2 high, serial clock out low
  STA VIA2PCR     ; set VIA 2 PCR
  RTS


;***********************************************************************************;
;
; 1ms delay

WAITABIT
  LDA #$04      ; set for 1024 cycles
  STA VIA2T2CH    ; set VIA 2 T2C_h
LAB_EF9B
  LDA VIA2IFR     ; get VIA 2 IFR
  AND #$20      ; mask T2 interrupt
  BEQ LAB_EF9B    ; loop until T2 interrupt

  RTS


;***********************************************************************************;
;
; RS-232 Tx NMI routine

RSNXTBIT
  LDA BITTS     ; get RS-232 bit count
  BEQ RSNXTBYT    ; if zero go setup next RS-232 Tx byte and return

  BMI RSSTOPS     ; if -ve go do stop bit(s)

          ; else bit count is non zero and +ve
  LSR RODATA      ; shift RS-232 output byte buffer
  LDX #$00      ; set $00 for bit = 0
  BCC LAB_EFB0    ; branch if bit was 0

  DEX       ; set $FF for bit = 1
LAB_EFB0
  TXA       ; copy bit to .A
  EOR ROPRTY      ; XOR with RS-232 parity byte
  STA ROPRTY      ; save RS-232 parity byte
  DEC BITTS     ; decrement RS-232 bit count
  BEQ RSPRTY      ; if RS-232 bit count now zero go do parity bit

; save bit and exit

LAB_EFB9
  TXA       ; copy bit to .A
  AND #$20      ; mask for CB2 control bit
  STA NXTBIT      ; save RS-232 next bit to send
  RTS

; do RS-232 parity bit, enters with RS-232 bit count = 0

RSPRTY
  LDA #$20      ; mask 00x0 0000, parity enable bit
  BIT M51CDR      ; test pseudo 6551 command register
  BEQ LAB_EFDA    ; branch if parity disabled

  BMI LAB_EFE4    ; branch if fixed mark or space parity

  BVS LAB_EFDE    ; branch if even parity

          ; else odd parity
  LDA ROPRTY      ; get RS-232 parity byte
  BNE LAB_EFCF    ; if parity not zero leave parity bit = 0

LAB_EFCE
  DEX       ; make parity bit = 1
LAB_EFCF
  DEC BITTS     ; decrement RS-232 bit count, 1 stop bit
  LDA M51CTR      ; get pseudo 6551 control register
  BPL LAB_EFB9    ; if 1 stop bit save parity bit and exit

          ; else two stop bits ..
  DEC BITTS     ; decrement RS-232 bit count, 2 stop bits
  BNE LAB_EFB9    ; save bit and exit, branch always

          ; parity is disabled so the parity bit becomes the first,
          ; and possibly only, stop bit. to do this increment the bit
          ; count which effectively decrements the stop bit count.
LAB_EFDA
  INC BITTS     ; increment RS-232 bit count, = -1 stop bit
  BNE LAB_EFCE    ; set stop bit = 1 and exit, branch always

          ; do even parity
LAB_EFDE
  LDA ROPRTY      ; get RS-232 parity byte
  BEQ LAB_EFCF    ; if parity zero leave parity bit = 0

  BNE LAB_EFCE    ; else make parity bit = 1, branch always

          ; fixed mark or space parity
LAB_EFE4
  BVS LAB_EFCF    ; if fixed space parity leave parity bit = 0

  BVC LAB_EFCE    ; else fixed mark parity make parity bit = 1, branch always

; decrement stop bit count, set stop bit = 1 and exit. $FF is one stop bit, $FE is two
; stop bits

RSSTOPS
  INC BITTS     ; decrement RS-232 bit count
  LDX #$FF      ; set stop bit = 1
  BNE LAB_EFB9    ; save stop bit and exit, branch always

; setup next RS-232 Tx byte

RSNXTBYT
  LDA M51CDR      ; get pseudo 6551 command register
  LSR       ; handshake bit into Cb
  BCC LAB_EFFB    ; branch if 3 line interface

  BIT VIA2PB      ; test VIA 2 DRB

; The above code is wrong, the address should be VIA1PB which is VIA 1 which is where the
; DSR and CTS inputs really are, the code should read ..
;
; BIT VIA1PB      ; test VIA 1 DRB

  BPL RSMISSNG    ; if DSR = 0 set DSR signal not present and exit

  BVC LAB_F019    ; if CTS = 0 set CTS signal not present and exit

          ; was 3 line interface
LAB_EFFB
  LDA #$00      ; clear .A
  STA ROPRTY      ; clear RS-232 parity byte
  STA NXTBIT      ; clear RS-232 next bit to send
  LDX BITNUM      ; get number of bits to be sent/received
  STX BITTS     ; set RS-232 bit count
  LDY RODBS     ; get index to Tx buffer start
  CPY RODBE     ; compare with index to Tx buffer end
  BEQ LAB_F021    ; if all done go disable T1 interrupt and return

  LDA (ROBUF),Y   ; else get byte from buffer
  STA RODATA      ; save to RS-232 output byte buffer
  INC RODBS     ; increment index to Tx buffer start
  RTS


;***********************************************************************************;
;
; set DSR signal not present

RSMISSNG
  LDA #$40      ; set DSR signal not present
  .byte $2C     ; makes next line BIT $10A9

; set CTS signal not present

LAB_F019
  LDA #$10      ; set CTS signal not present
  ORA RSSTAT      ; OR with RS-232 status register
  STA RSSTAT      ; save RS-232 status register

; disable T1 interrupt

LAB_F021
  LDA #$40      ; disable T1 interrupt
  STA VIA1IER     ; set VIA 1 IER
  RTS


;***********************************************************************************;
;
; compute bit count

RSCPTBIT
  LDX #$09      ; set bit count to 9, 8 data + 1 stop bit
  LDA #$20      ; mask for 8/7 data bits
  BIT M51CTR      ; test pseudo 6551 control register
  BEQ LAB_F031    ; branch if 8 bits

  DEX       ; else decrement count for 7 data bits
LAB_F031
  BVC LAB_F035    ; branch if 7 bits

  DEX       ; else decrement count ..
  DEX       ; .. for 5 data bits
LAB_F035
  RTS


;***********************************************************************************;
;
; RS-232 Rx NMI

RSINBIT
  LDX RINONE      ; get RS-232 start bit check flag
  BNE RSSTRBIT    ; branch if no start bit received

  DEC BITCI     ; decrement RS-232 input bit count
  BEQ RSINBYTE    ; if the byte is complete go add it to the buffer

  BMI LAB_F04D    ; determine if all stop bits have been received

  LDA INBIT     ; get RS-232 input bit
  EOR RIPRTY      ; XOR with RS-232 parity bit
  STA RIPRTY      ; save in RS-232 parity bit
  LSR INBIT     ; shift RS-232 input bit into Cb
  ROR RIDATA      ; shift Cb into RS-232 byte assembly
LAB_F04A
  RTS


;***********************************************************************************;
;
; determine if all stop bits have been received

RSSTPBIT
  DEC BITCI     ; decrement RS-232 input bit count
LAB_F04D
  LDA INBIT     ; get RS-232 input bit
  BEQ LAB_F0B3    ; branch if bit clear

  LDA M51CTR      ; get pseudo 6551 control register
  ASL       ; shift stop bits into Cb
  LDA #$01      ; one stop bit
  ADC BITCI     ; add stop bits to RS-232 input bit count
  BNE LAB_F04A    ; branch always


;***********************************************************************************;
;
; prepare to receive next input byte

RSPREPIN
  LDA #$90      ; enable CB1 interrupt, Rx data bit transition
  STA VIA1IER     ; set VIA 1 IER
  STA RINONE      ; set RS-232 start bit check flag, no start bit received
  LDA #$20      ; disable T2 interrupt
  STA VIA1IER     ; set VIA 1 IER
  RTS


;***********************************************************************************;
;
; no RS-232 start bit received

RSSTRBIT
  LDA INBIT     ; get RS-232 input bit
  BNE RSPREPIN    ; branch if bit set
  STA RINONE      ; set RS-232 start bit check flag, start bit received
  RTS


;***********************************************************************************;
;
; put byte into receive buffer

RSINBYTE
  LDY RIDBE     ; get index to Rx buffer end
  INY       ; increment index
  CPY RIDBS     ; compare with index to Rx buffer start
  BEQ RSOVERUN    ; if buffer full go do Rx overrun error

  STY RIDBE     ; save index to Rx buffer end
  DEY       ; decrement index
  LDA RIDATA      ; get assembled byte
  LDX BITNUM      ; get bit count
LAB_F081
  CPX #$09      ; compare with byte + stop
  BEQ LAB_F089    ; branch if all nine bits received

  LSR       ; else shift byte
  INX       ; increment bit count
  BNE LAB_F081    ; loop, branch always

LAB_F089
  STA (RIBUF),Y   ; save received byte to RS-232 Rx buffer
  LDA #$20      ; mask 00x0 0000, parity enable bit
  BIT M51CDR      ; test pseudo 6551 command register
  BEQ RSSTPBIT    ; branch if parity disabled

  BMI LAB_F04A    ; exit if mark or space parity

  LDA INBIT     ; get RS-232 input bit
  EOR RIPRTY      ; XOR with RS-232 parity bit
  BEQ RSPRTYER    ; branch if parity error

  BVS LAB_F04A    ; exit if even parity

  .byte $2C     ; makes next line BIT $AB50
RSPRTYER
  BVC LAB_F04A    ; exit if odd parity

  LDA #$01      ; set Rx parity error
  .byte $2C     ; makes next line BIT $04A9

RSOVERUN
  LDA #$04      ; set Rx overrun error
  .byte $2C     ; makes next line BIT $80A9

RSBREAK
  LDA #$80      ; Rx break error
  .byte $2C     ; makes next line BIT $02A9

RSFRAMER
  LDA #$02      ; Rx frame error
  ORA RSSTAT      ; OR with RS-232 status byte
  STA RSSTAT      ; save RS-232 status byte
  JMP RSPREPIN    ; prepare to receive next input byte

LAB_F0B3
  LDA RIDATA      ; get assembled byte
  BNE RSFRAMER    ; if not break do frame error

  BEQ RSBREAK     ; else do break error, branch always


;***********************************************************************************;
;
; do illegal device number

RSDVCERR
  JMP FE_ILDEV    ; do illegal device number and return


;***********************************************************************************;
;
; open RS-232 channel for output

RSOPNOUT
  STA DFLTO     ; save output device number
  LDA M51CDR      ; get pseudo 6551 command register
  LSR       ; shift handshake bit to carry
  BCC LAB_F0EB    ; branch if 3 line interface

  LDA #$02      ; mask for RTS out
  BIT VIA1PB      ; test VIA 1 DRB
  BPL LAB_F0E8    ; if DSR = 0 set DSR not present and exit

  BNE LAB_F0EB    ; if RTS = 1 just exit

          ; RTS is low because a half-duplex input channel
          ; was opened, wait for current receive to finish

LAB_F0CD
  LDA VIA1IER     ; get VIA 1 IER
  AND #$30      ; mask 00xx 0000, T2 and CB1 interrupts
  BNE LAB_F0CD    ; loop while either enabled

LAB_F0D4
  BIT VIA1PB      ; test VIA 1 DRB
  BVS LAB_F0D4    ; loop while CTS high

  LDA VIA1PB      ; get VIA 1 DRB
  ORA #$02      ; set RTS high
  STA VIA1PB      ; save VIA 1 DRB
LAB_F0E1
  BIT VIA1PB      ; test VIA 1 DRB
  BVS LAB_F0EB    ; exit if CTS high

  BMI LAB_F0E1    ; loop while DSR high

LAB_F0E8
  JSR RSMISSNG    ; set DSR signal not present
LAB_F0EB
  CLC       ; flag ok
  RTS


;***********************************************************************************;
;
; send byte to RS-232 buffer

RSOUTSAV
  LDY RODBE     ; get index to Tx buffer end
  INY       ; + 1
  CPY RODBS     ; compare with index to Tx buffer start
  BEQ RSOUTSAV    ; loop while buffer full

  STY RODBE     ; set index to Tx buffer end
  DEY       ; index to available buffer byte
  STA (ROBUF),Y   ; save byte to buffer
  BIT VIA1IER     ; test VIA 1 IER
  BVC RSPREPOT    ; branch if T1 not enabled

  RTS

RSPREPOT
  LDA BAUDOF      ; get baud rate bit time low byte
  STA VIA1T1CL    ; set VIA 1 T1C_l
  LDA BAUDOF+1    ; get baud rate bit time high byte
  STA VIA1T1CH    ; set VIA 1 T1C_h
  LDA #$C0      ; enable T1 interrupt
  STA VIA1IER     ; set VIA 1 IER
  JMP RSNXTBYT    ; setup next RS-232 Tx byte and return


;***********************************************************************************;
;
; open RS-232 channel for input

RSOPNIN
  STA DFLTN     ; save input device number
  LDA M51CDR      ; get pseudo 6551 command register
  LSR       ; shift b0 into Cb
  BCC LAB_F146    ; branch if 3 line interface

  AND #$08      ; mask duplex bit, pseudo 6551 command is >> 1
  BEQ LAB_F146    ; branch if full duplex

          ; half-duplex, X-line handshaking

  LDA #$02      ; mask for RTS out
  BIT VIA1PB      ; test VIA 1 DRB
  BPL LAB_F0E8    ; if DSR = 0 set DSR not present and exit

  BEQ LAB_F144    ; if RTS = 0 just exit

          ; wait for current transmit to finish

LAB_F12B
  BIT VIA1IER     ; test VIA 1 IER
  BVS LAB_F12B    ; loop while T1 interrupt enabled

  LDA VIA1PB      ; get VIA 1 DRB
  AND #$FD      ; mask xxxx xx0x, clear RTS out
  STA VIA1PB      ; save VIA 1 DRB
LAB_F138
  LDA VIA1PB      ; get VIA 1 DRB
  AND #$04      ; mask xxxx x1xx, DTR out
          ; DTR is an output and always held high so the
          ; following test should never branch
  BEQ LAB_F138    ; loop while DTR low

LAB_F13F
  LDA #$90      ; enable CB1 interrupt, Rx data bit transition
  STA VIA1IER     ; set VIA 1 IER
LAB_F144
  CLC       ; flag no error
  RTS

LAB_F146
  LDA VIA1IER     ; get VIA 1 IER
  AND #$30      ; mask 0xx0 0000, T1 and T2 interrupts
  BEQ LAB_F13F    ; if both interrupts disabled go enable CB1
          ; interrupt and exit

  CLC       ; flag no error
  RTS


;***********************************************************************************;
;
; get byte from RS-232 buffer

RSNXTIN
  LDY RIDBS     ; get index to Rx buffer start
  CPY RIDBE     ; compare with index to Rx buffer end
  BEQ LAB_F15D    ; return null if buffer empty

  LDA (RIBUF),Y   ; get byte from RS-232 Rx buffer
  INC RIDBS     ; increment index to Rx buffer start
  RTS

LAB_F15D
  LDA #$00      ; return null
  RTS


;***********************************************************************************;
;
; check RS-232 bus idle

RSPAUSE
  PHA       ; save .A
  LDA VIA1IER     ; get VIA 1 IER
  BEQ LAB_F172    ; branch if no interrupts enabled. this branch will
          ; never be taken as b7 of IER always reads as 1
          ; according to the 6522 data sheet
LAB_F166
  LDA VIA1IER     ; get VIA 1 IER
  AND #$60      ; mask 0xx0 0000, T1 and T2 interrupts
  BNE LAB_F166    ; loop if T1 or T2 active

  LDA #$10      ; disable CB1 interrupt, Rx data bit transition
  STA VIA1IER     ; set VIA 1 IER
LAB_F172
  PLA       ; restore .A
  RTS


;***********************************************************************************;
;
; KERNAL I/O messages

KMSGTBL
KM_IOERR
  .byte $0D,"I/O ERROR ",'#'+$80
KM_SRCHG
  .byte $0D,"SEARCHING",' '+$80
KM_FOR
  .byte "FOR",' '+$80
KM_PRPLY
  .byte $0D,"PRESS PLAY ON TAP",'E'+$80
KM_RECPY
  .byte "PRESS RECORD & PLAY ON TAP",'E'+$80
KM_LODNG
  .byte $0D,"LOADIN",'G'+$80
KM_SAVNG
  .byte $0D,"SAVING",' '+$80
KM_VFYNG
  .byte $0D,"VERIFYIN",'G'+$80
KM_FOUND
  .byte $0D,"FOUND",' '+$80
KM_OK
  .byte $0D,"OK",$0D+$80


;***********************************************************************************;
;
; display control I/O message if in direct mode

SPMSG
  BIT MSGFLG      ; test KERNAL message mode flag
  BPL LAB_F1F3    ; exit if control messages off

; display KERNAL I/O message

KMSGSHOW
  LDA KMSGTBL,Y   ; get byte from message table
  PHP       ; save status
  AND #$7F      ; clear b7
  JSR CHROUT      ; output character to channel
  INY       ; increment index
  PLP       ; restore status
  BPL KMSGSHOW    ; loop if not end of message

LAB_F1F3
  CLC       ; flag no error
  RTS


;***********************************************************************************;
;
; get a character from the input device

; In practice this routine operates identically to the CHRIN routine for all devices
; except for the keyboard. If the keyboard is the current input device this routine
; will get one character from the keyboard buffer. It depends on the IRQ routine to
; read the keyboard and put characters into the buffer.

; If the keyboard buffer is empty the value returned in the accumulator will be zero.

FGETIN
  LDA DFLTN     ; get input device number
  BNE LAB_F201    ; branch if not keyboard

          ; input device was keyboard
  LDA NDX     ; get keyboard buffer length
  BEQ LAB_F26A    ; if buffer empty go flag no byte and return

  SEI       ; disable interrupts
  JMP LP2     ; input from keyboard buffer and return

          ; input device was not keyboard
LAB_F201
  CMP #$02      ; compare device with RS-232 device
  BNE LAB_F21D    ; branch if not RS-232 device

          ; input device is RS-232 device
LAB_F205
  STY XSAV      ; save .Y
  JSR RSNXTIN     ; get byte from RS-232 buffer
  LDY XSAV      ; restore .Y
  CLC       ; flag no error
  RTS


;***********************************************************************************;
;
; input character from channel

; This routine will get a byte of data from the channel already set up as the input
; channel by the CHKIN routine.

; If CHKIN has not been used to define another input channel the data is expected to be
; from the keyboard. the data byte is returned in the accumulator. The channel remains
; open after the call.

; Input from the keyboard is handled in a special way. First, the cursor is turned on
; and it will blink until a carriage return is typed on the keyboard. All characters
; on the logical line, up to 88 characters, will be stored in the BASIC input buffer.
; Then the characters can be returned one at a time by calling this routine once for
; each character. When the carriage return is returned the entire line has been
; processed. the next time this routine is called the whole process begins again.

FCHRIN
  LDA DFLTN     ; get input device number
  BNE LAB_F21D    ; if it's not the keyboard continue

          ; the input device is the keyboard
  LDA PNTR      ; get cursor column
  STA LXSP+1      ; set input cursor column
  LDA TBLX      ; get cursor row
  STA LXSP      ; set input cursor row
  JMP GETSCRN     ; go get input from the keyboard

; the input device was not the keyboard

LAB_F21D
  CMP #$03      ; compare device number with screen
  BNE LAB_F22A    ; if it's not the screen continue

          ; the input device is the screen
  STA CRSW      ; set input from screen
  LDA LNMX      ; get current screen line length
  STA INDX      ; save input EOL pointer
  JMP GETSCRN     ; go get input from the screen

; the input device was not the screen

LAB_F22A
  BCS CHRINSR     ; if input device is the serial bus go handle it

; the input device is < the screen so must be the RS-232 or tape device

  CMP #$02      ; compare device with RS-232 device
  BEQ CHRINRS     ; if it's the RS-232 device go handle it

; else there's only the tape device left ..

  STX XSAV      ; save .X
  JSR CHRINTP2    ; get byte from tape
  BCS LAB_F24D    ; exit if error

  PHA       ; save byte
  JSR CHRINTP2    ; get next byte from tape
  BCS LAB_F24A    ; exit if error

  BNE LAB_F244    ; branch if end not reached

  LDA #$40      ; set EOF bit
  JSR ORIOST      ; OR into I/O status byte
LAB_F244
  DEC BUFPNT      ; back up tape buffer index
  LDX XSAV      ; restore .X
  PLA       ; restore saved byte
  RTS

; error exit from input character

LAB_F24A
  TAX       ; copy error byte
  PLA       ; dump saved byte
  TXA       ; restore error byte
LAB_F24D
  LDX XSAV      ; restore .X
  RTS


;***********************************************************************************;
;
; get byte from tape

CHRINTP2
  JSR JTP20     ; bump tape pointer
  BNE LAB_F260    ; if not end get next byte and exit

  JSR RDTPBLKS    ; initiate tape read
  BCS LAB_F26B    ; exit if error flagged

  LDA #$00      ; clear .A
  STA BUFPNT      ; clear tape buffer index
  BEQ CHRINTP2    ; loop, branch always

LAB_F260
  LDA (TAPE1),Y   ; get next byte from buffer
  CLC       ; flag no error
  RTS


;***********************************************************************************;
;
; the input device was the serial bus

CHRINSR
  LDA STATUS      ; get I/O status byte
  BEQ LAB_F26C    ; if no errors flagged go input byte and return

  LDA #$0D      ; else return [EOL]
LAB_F26A
  CLC       ; flag no error
LAB_F26B
  RTS

LAB_F26C
  JMP FACPTR      ; input a byte from the serial bus and return


;***********************************************************************************;
;
; input device was RS-232 device

CHRINRS
  JSR LAB_F205    ; get byte from RS-232 device
  BCS LAB_F279    ; branch if error, this doesn't get taken as the last
          ; instruction in the get byte from RS-232 device routine
          ; is CLC
  CMP #$00      ; compare with null
  BEQ CHRINRS     ; loop if null

  CLC       ; flag no error
LAB_F279
  RTS


;***********************************************************************************;
;
; output a character to channel

; This routine will output a character to an already opened channel. Use the OPEN
; routine, OPEN, and the CHKOUT routine, to set up the output channel before calling
; this routine. If these calls are omitted, data will be sent to the default output
; device, device 3, the screen. The data byte to be output is loaded into the accumulator,
; and this routine is called. The data is then sent to the specified output device.
; The channel is left open after the call.

; NOTE: Care must be taken when using routine to send data to a serial device since
; data will be sent to all open output channels on the bus. Unless this is desired,
; all open output channels on the serial bus other than the actually intended
; destination channel must be closed by a call to the KERNAL close channel routine.

FCHROUT
  PHA       ; save the character to send
  LDA DFLTO     ; get output device number
  CMP #$03      ; compare device number with screen
  BNE LAB_F285    ; if output device not screen continue

; the output device is the screen

  PLA       ; restore character to send
  JMP SCRNOUT     ; output character and return

; the output device was not the screen

LAB_F285
  BCC LAB_F28B    ; if output device < screen continue

; the output device was > screen so it is a serial bus device

  PLA       ; restore character to send
  JMP FCIOUT      ; output a byte to the serial bus and return

; the output device is < screen

LAB_F28B
  CMP #$02      ; compare the device with RS-232 device
  BEQ LAB_F2B9    ; if output device is RS-232 device go handle it

; else the output device is the cassette

  PLA       ; restore the character to send


;***********************************************************************************;
;
; output a character to the cassette

CHROUTTP
  STA PTR1      ; save character to character buffer
  PHA       ; save .A
  TXA       ; copy .X
  PHA       ; save .X
  TYA       ; copy .Y
  PHA       ; save .Y
  JSR JTP20     ; bump tape pointer
  BNE LAB_F2AA    ; if not end save next byte and exit

  JSR WBLK      ; initiate tape write
  BCS LAB_F2AF    ; exit if error

  LDA #$02      ; set data block file type
  LDY #$00      ; clear index
  STA (TAPE1),Y   ; save file type to tape buffer
  INY       ; increment index
  STY BUFPNT      ; save tape buffer index
LAB_F2AA
  LDA PTR1      ; restore character from character buffer
  STA (TAPE1),Y   ; save to buffer
  CLC       ; flag no error
LAB_F2AF
  PLA       ; pull .Y
  TAY       ; restore .Y
  PLA       ; pull .X
  TAX       ; restore .X
  PLA       ; restore .A
  BCC LAB_F2B8    ; exit if no error

  LDA #$00      ; else clear .A
LAB_F2B8
  RTS


;***********************************************************************************;
;
; the output device is RS-232 device

LAB_F2B9
  PLA       ; restore character to send
  STX XSAV      ; save .X
  STY PTR1      ; save .Y
  JSR RSOUTSAV    ; send byte to RS-232 buffer
  LDX XSAV      ; restore .Y
  LDY PTR1      ; restore .X
  CLC       ; flag ok
  RTS


;***********************************************************************************;
;
; open a channel for input

; Any logical file that has already been opened by the OPEN routine can be defined as
; an input channel by this routine. The device on the channel must be an input device
; or an error will occur and the routine will abort.

; If you are getting data from anywhere other than the keyboard, this routine must be
; called before using either the CHRIN routine or the GETIN routine. If you are
; getting data from the keyboard and no other input channels are open then the calls
; to this routine and to the OPEN routine are not needed.

; When used with a device on the serial bus this routine will automatically send the
; listen address specified by the OPEN routine and any secondary address.

; Possible errors are:
;
; 3 : file not open
; 5 : device not present
; 6 : file is not an input file

FCHKIN
  JSR FNDFLNO     ; find file
  BEQ LAB_F2CF    ; branch if file opened

  JMP FE_NTOPN    ; do file not open error and return

LAB_F2CF
  JSR SETFLCH     ; set file details from table,.X
  LDA FA      ; get device number
  BEQ LAB_F2EC    ; if device was keyboard save device #, flag ok and exit

  CMP #$03      ; compare device number with screen
  BEQ LAB_F2EC    ; if device was screen save device #, flag ok and exit

  BCS LAB_F2F0    ; branch if serial bus device

  CMP #$02      ; compare device with RS-232 device
  BNE LAB_F2E3    ; branch if not RS-232 device

  JMP RSOPNIN     ; open RS-232 channel for input

LAB_F2E3
  LDX SA      ; get secondary address
  CPX #$60      ; compare with read
  BEQ LAB_F2EC    ; branch if read

  JMP FE_NTINP    ; do not input file error and return

LAB_F2EC
  STA DFLTN     ; save input device number
  CLC       ; flag ok
  RTS

          ; device was serial bus device
LAB_F2F0
  TAX       ; copy device number to .X
  JSR FTALK     ; command a serial bus device to TALK
  LDA SA      ; get secondary address
  BPL LAB_F2FE    ; branch if address to send

  JSR LAB_EED3    ; wait for bus end after send
  JMP LAB_F301    ; do I/O status test

LAB_F2FE
  JSR FTKSA     ; send secondary address after TALK
LAB_F301
  TXA       ; copy device back to .A
  BIT STATUS      ; test I/O status byte
  BPL LAB_F2EC    ; if device present save device number and exit

  JMP FE_DVNTP    ; do device not present error and return


;***********************************************************************************;
;
; open a channel for output

; Any logical file that has already been opened by the OPEN routine can be defined
; as an output channel by this routine the device on the channel must be an output
; output device or an error will occur and the routine will abort.

; If you are sending data to anywhere other than the screen this routine must be
; called before using the CHROUT routine. If you are sending data to the screen and
; no other output channels are open then the calls to this routine and to the OPEN
; routine are not needed.

; When used with a device on the serial bus this routine will automatically send the
; listen address specified by the OPEN routine and any secondary address.

; Possible errors are:
;
; 3 : file not open
; 5 : device not present
; 7 : file is not an output file

FCHKOUT
  JSR FNDFLNO     ; find file
  BEQ LAB_F311    ; branch if file found

  JMP FE_NTOPN    ; do file not open error and return

LAB_F311
  JSR SETFLCH     ; set file details from table,.X
  LDA FA      ; get device number
  BNE LAB_F31B    ; branch if device is not keyboard

LAB_F318
  JMP FE_NTOUT    ; do not output file error and return

LAB_F31B
  CMP #$03      ; compare device number with screen
  BEQ LAB_F32E    ; if screen save output device number and exit

  BCS LAB_F332    ; branch if > screen, serial bus device

  CMP #$02      ; compare device with RS-232 device
  BNE LAB_F328    ; branch if not RS-232 device, must be tape

  JMP RSOPNOUT    ; open RS-232 channel for output

          ; open tape channel for output
LAB_F328
  LDX SA      ; get secondary address
  CPX #$60      ; compare with read
  BEQ LAB_F318    ; if read do not output file error and return

LAB_F32E
  STA DFLTO     ; save output device number
  CLC       ; flag ok
  RTS

LAB_F332
  TAX       ; copy device number
  JSR FLISTEN     ; command devices on the serial bus to LISTEN
  LDA SA      ; get secondary address
  BPL LAB_F33F    ; branch if address to send

  JSR SCATN     ; else set serial ATN high
  BNE LAB_F342    ; branch always

LAB_F33F
  JSR FSECOND     ; send secondary address after LISTEN
LAB_F342
  TXA       ; copy device number back to .A
  BIT STATUS      ; test I/O status byte
  BPL LAB_F32E    ; if device present save output device number and exit

  JMP FE_DVNTP    ; else do device not present error and return


;***********************************************************************************;
;
; close a specified logical file

; This routine is used to close a logical file after all I/O operations have been
; completed on that file. This routine is called after the accumulator is loaded
; with the logical file number to be closed, the same number used when the file was
; opened using the OPEN routine.

FCLOSE
  JSR LAB_F3D4    ; find file .A
  BEQ LAB_F351    ; if the file is found go close it

  CLC       ; else the file was closed so just flag ok
  RTS

; found the file so close it

LAB_F351
  JSR SETFLCH     ; set file details from table,.X
  TXA       ; copy file index to .A
  PHA       ; save file index
  LDA FA      ; get device number
  BEQ LAB_F3B1    ; if $00, keyboard, restore index and close file

  CMP #$03      ; compare device number with screen
  BEQ LAB_F3B1    ; if screen restore index and close file

  BCS LAB_F3AE    ; if > screen go do serial bus device close

  CMP #$02      ; compare device with RS-232 device
  BNE LAB_F38D    ; branch if not RS-232 device

          ; else close RS-232 device
  PLA       ; restore file index
  JSR LAB_F3B2    ; close file index .A
  LDA #$7D      ; disable T1, T2, CB1, CB2, SR and CA2
  STA VIA1IER     ; set VIA 1 IER
  LDA #$06      ; set DTR and RTS high
  STA VIA1PB      ; set VIA 1 DRB
  LDA #$EE      ; CB2 high, CB1 -ve edge, CA2 high, CA1 -ve edge
  STA VIA1PCR     ; set VIA 1 PCR
  JSR LAB_FE75    ; read the top of memory
  LDA RIBUF+1     ; get RS-232 Rx buffer pointer high byte
  BEQ LAB_F37F    ; branch if no RS-232 input buffer

  INY       ; else reclaim RS-232 input buffer memory
LAB_F37F
  LDA ROBUF+1     ; get RS-232 Tx buffer pointer high byte
  BEQ LAB_F384    ; branch if no RS-232 output buffer

  INY       ; else reclaim RS-232 output buffer memory
LAB_F384
  LDA #$00      ; clear .A
  STA RIBUF+1     ; clear RS-232 Rx buffer pointer high byte
  STA ROBUF+1     ; clear RS-232 Tx buffer pointer high byte
  JMP LAB_F53C    ; go set top of memory and exit

LAB_F38D
  LDA SA      ; get secondary address
  AND #$0F      ; mask the OPEN CHANNEL command
  BEQ LAB_F3B1    ; if read restore index and close file

  JSR TPBUFA      ; get tape buffer start pointer in .X.Y
  LDA #$00      ; character $00
  JSR CHROUTTP    ; output character to cassette
  JMP PATCH3      ; go do CLOSE tail

LAB_F39E
  BCS LAB_F3CE    ; just exit if error

  LDA SA      ; get secondary address
  CMP #$62      ; compare with end of tape flag
  BNE LAB_F3B1    ; if not end of tape restore index and close file

  LDA #$05      ; set logical end of the tape
  JSR TAPEH     ; write tape header
  JMP LAB_F3B1    ; restore index and close file


;***********************************************************************************;
;
; do serial bus device file close

LAB_F3AE
  JSR LAB_F6DA    ; close serial bus device
LAB_F3B1
  PLA       ; restore file index


;***********************************************************************************;
;
; close file index .A

LAB_F3B2
  TAX       ; copy index to file to close
  DEC LDTND     ; decrement open file count
  CPX LDTND     ; compare index with open file count
  BEQ LAB_F3CD    ; exit if equal, last entry was closing file

          ; else entry was not last in list so copy last table entry
          ; file details over the details of the closing one
  LDY LDTND     ; get open file count as index
  LDA LAT,Y     ; get last+1 logical file number from logical file table
  STA LAT,X     ; save logical file number over closed file
  LDA FAT,Y     ; get last+1 device number from device number table
  STA FAT,X     ; save device number over closed file
  LDA SAT,Y     ; get last+1 secondary address from secondary address table
  STA SAT,X     ; save secondary address over closed file
LAB_F3CD
  CLC       ; flag no error
LAB_F3CE
  RTS


;***********************************************************************************;
;
; find file

FNDFLNO
  LDA #$00      ; clear .A
  STA STATUS      ; clear I/O status byte
  TXA       ; copy logical file number to .A

; find file .A

LAB_F3D4
  LDX LDTND     ; get open file count
LAB_F3D6
  DEX       ; decrement count to give index
  BMI LAB_F3EE    ; exit if no files

  CMP LAT,X     ; compare logical file number with table logical file number
  BNE LAB_F3D6    ; loop if no match

  RTS


;***********************************************************************************;
;
; set file details from table,.X

SETFLCH
  LDA LAT,X     ; get logical file from logical file table
  STA LA      ; set logical file
  LDA FAT,X     ; get device number from device number table
  STA FA      ; set device number
  LDA SAT,X     ; get secondary address from secondary address table
  STA SA      ; set secondary address
LAB_F3EE
  RTS


;***********************************************************************************;
;
; close all channels and files

; This routine closes all open files. When this routine is called, the pointers into
; the open file table are reset, closing all files. Also the routine automatically
; resets the I/O channels.

FCLALL
  LDA #$00      ; clear .A
  STA LDTND     ; clear open file count


;***********************************************************************************;
;
; close input and output channels

; This routine is called to clear all open channels and restore the I/O channels to
; their original default values. It is usually called after opening other I/O
; channels and using them for input/output operations. The default input device is
; 0, the keyboard. The default output device is 3, the screen.

; If one of the channels to be closed is to the serial bus, an UNTALK signal is sent
; first to clear the input channel or an UNLISTEN is sent to clear the output channel.
; By not calling this routine and leaving listener(s) active on the serial bus,
; several devices can receive the same data from the VIC at the same time. One way to
; take advantage of this would be to command the printer to LISTEN and the disk to
; TALK. This would allow direct printing of a disk file.

FCLRCHN
  LDX #$03      ; set .X to screen
  CPX DFLTO     ; compare output device number with screen
  BCS LAB_F3FC    ; branch if screen >= device

          ; else was serial bus
  JSR FUNLSN      ; command the serial bus to UNLISTEN
LAB_F3FC
  CPX DFLTN     ; compare input device number with screen
  BCS LAB_F403    ; branch if screen >= device

          ; else was serial bus
  JSR FUNTLK      ; command the serial bus to UNTALK
LAB_F403
  STX DFLTO     ; set output device number to screen
  LDA #$00      ; set for keyboard
  STA DFLTN     ; set input device number to keyboard
  RTS


;***********************************************************************************;
;
; open a logical file

; This routine is used to open a logical file. Once the logical file is set up it
; can be used for input/output operations. Most of the I/O KERNAL routines call on
; this routine to create the logical files to operate on. No arguments need to be
; set up to use this routine, but both the SETLFS and SETNAM KERNAL routines must
; be called before using this routine.

FOPEN
  LDX LA      ; get logical file
  BNE LAB_F411    ; branch if there is a file

  JMP FE_NTINP    ; else do not input file error and return

LAB_F411
  JSR FNDFLNO     ; find file
  BNE LAB_F419    ; branch if file not found

  JMP FE_ALOPN    ; else do file already open error and return

LAB_F419
  LDX LDTND     ; get open file count
  CPX #$0A      ; compare with max
  BCC LAB_F422    ; branch if less

  JMP FE_2MNYF    ; else do too many files error and return

LAB_F422
  INC LDTND     ; increment open file count
  LDA LA      ; get logical file
  STA LAT,X     ; save to logical file table
  LDA SA      ; get secondary address
  ORA #$60      ; OR with the OPEN CHANNEL command
  STA SA      ; set secondary address
  STA SAT,X     ; save to secondary address table
  LDA FA      ; get device number
  STA FAT,X     ; save to device number table
  BEQ LAB_F493    ; do ok exit if keyboard

  CMP #$03      ; compare device number with screen
  BEQ LAB_F493    ; do ok exit if screen

  BCC LAB_F444    ; branch if < screen, tape or RS-232

          ; else is serial bus device
  JSR SERNAME     ; send secondary address and filename
  BCC LAB_F493    ; do ok exit, branch always

LAB_F444
  CMP #$02      ; compare device with RS-232 device
  BNE LAB_F44B    ; branch if not RS-232 device, must be tape

  JMP OPENRS      ; go open RS-232 device and return

LAB_F44B
  JSR TPBUFA      ; get tape buffer start pointer in .X.Y
  BCS LAB_F453    ; branch if >= $0200

  JMP FE_ILDEV    ; do illegal device number and return

LAB_F453
  LDA SA      ; get secondary address
  AND #$0F      ; mask the OPEN CHANNEL command
  BNE LAB_F478    ; branch if write

  JSR CSTEL     ; wait for PLAY
  BCS LAB_F494    ; exit if STOP was pressed

  JSR SRCHING     ; print "SEARCHING..."
  LDA FNLEN     ; get file name length
  BEQ LAB_F46F    ; if null file name just go find header

  JSR FNDHDR      ; find specific tape header
  BCC LAB_F482    ; branch if no error

  BEQ LAB_F494    ; branch always

LAB_F46C
  JMP FE_NTFND    ; do file not found error and return

LAB_F46F
  JSR FAH     ; find tape header, exit with header in buffer
  BEQ LAB_F494    ; exit if end of tape found

  BCC LAB_F482    ; branch if no error

  BCS LAB_F46C    ; branch if error

LAB_F478
  JSR CSTE2     ; wait for PLAY/RECORD
  BCS LAB_F494    ; exit if STOP was pressed

  LDA #$04      ; set data file header
  JSR TAPEH     ; write tape header
LAB_F482
  LDA #$BF      ; set tape buffer length
  LDY SA      ; get secondary address
  CPY #$60      ; compare with read
  BEQ LAB_F491    ; branch if read

  LDY #$00      ; clear index
  LDA #$02      ; set data file block file type
  STA (TAPE1),Y   ; save file type to tape buffer
  TYA       ; clear .A
LAB_F491
  STA BUFPNT      ; save tape buffer index
LAB_F493
  CLC       ; flag ok
LAB_F494
  RTS


;***********************************************************************************;
;
; send secondary address and filename

SERNAME
  LDA SA      ; get secondary address
  BMI LAB_F4C5    ; ok exit if no address

  LDY FNLEN     ; get file name length
  BEQ LAB_F4C5    ; ok exit if null

  LDA FA      ; get device number
  JSR FLISTEN     ; command devices on the serial bus to LISTEN
  LDA SA      ; get the secondary address
  ORA #$F0      ; OR with the OPEN command
  JSR FSECOND     ; send secondary address after LISTEN
  LDA STATUS      ; get I/O status byte
  BPL LAB_F4B2    ; branch if device present

  PLA       ; else dump calling address low byte
  PLA       ; dump calling address high byte
  JMP FE_DVNTP    ; do device not present error and return

LAB_F4B2
  LDA FNLEN     ; get file name length
  BEQ LAB_F4C2    ; branch if null name

  LDY #$00      ; clear index
LAB_F4B8
  LDA (FNADR),Y   ; get file name byte
  JSR FCIOUT      ; output a byte to the serial bus
  INY       ; increment index
  CPY FNLEN     ; compare with file name length
  BNE LAB_F4B8    ; loop if not all done

LAB_F4C2
  JSR FUNLSN      ; command the serial bus to UNLISTEN
LAB_F4C5
  CLC       ; flag ok
  RTS


;***********************************************************************************;
;
; open RS-232

OPENRS
  LDA #$06      ; IIII IOOI, DTR and RTS only as outputs
  STA VIA1DDRB    ; set VIA 1 DDRB
  STA VIA1PB      ; set VIA 1 DRB, DTR and RTS high
  LDA #$EE      ; CB2 high, CB1 -ve edge, CA2 high, CA1 -ve edge
  STA VIA1PCR     ; set VIA 1 PCR
  LDY #$00      ; clear index
  STY RSSTAT      ; clear RS-232 status byte
LAB_F4D9
  CPY FNLEN     ; compare with file name length
  BEQ LAB_F4E7    ; exit loop if done

  LDA (FNADR),Y   ; get file name byte
  STA M51CTR,Y    ; copy to pseudo 6551 register set
  INY       ; increment index
  CPY #$04      ; compare with $04
  BNE LAB_F4D9    ; loop if not to 4 yet

LAB_F4E7
  JSR RSCPTBIT    ; compute bit count
  STX BITNUM      ; save bit count
  LDA M51CTR      ; get pseudo 6551 control register
  AND #$0F      ; mask 0000 xxxx, baud rate
  BNE LAB_F4F4    ; short delay. was this intended to skip code used to
          ; implement the user baud rate ??
LAB_F4F4
  ASL       ; * 2, 2 bytes per baud count
  TAX       ; copy to index
  LDA BAUDTBL-2,X   ; get timer constant low byte
  ASL       ; * 2
  TAY       ; copy to .Y
  LDA BAUDTBL-1,X   ; get timer constant high byte
  ROL       ; * 2
  PHA       ; save it
  TYA       ; get timer constant low byte back
  ADC #$C8      ; + 200, carry cleared by previous ROL
  STA BAUDOF      ; save bit cell time low byte
  PLA       ; restore high byte
  ADC #$00      ; add carry
  STA BAUDOF+1    ; save bit cell time high byte
  LDA M51CDR      ; get pseudo 6551 command register
  LSR       ; shift b0 into Cb
  BCC LAB_F51B    ; branch if 3 line interface

  LDA VIA2PB      ; get VIA 2 DRB

; The above code is wrong, the address should be VIA1PB which is where the DSR input
; really is
;
; LDA VIA1PB      ; get VIA 1 DRB

  ASL       ; shift DSR into Cb
  BCS LAB_F51B    ; branch if DSR = 1

  JMP RSMISSNG    ; set DSR signal not present and return

LAB_F51B
  LDA RIDBE     ; get index to Rx buffer end
  STA RIDBS     ; set index to Rx buffer start, clear Rx buffer
  LDA RODBE     ; get index to Tx buffer end
  STA RODBS     ; set index to Tx buffer start, clear Tx buffer
  JSR LAB_FE75    ; read the top of memory
  LDA RIBUF+1     ; get RS-232 Rx buffer pointer high byte
  BNE LAB_F533    ; branch if buffer already set

  DEY       ; decrement top of memory high byte, 256 byte buffer
  STY RIBUF+1     ; set RS-232 Rx buffer pointer high byte
  STX RIBUF     ; set RS-232 Rx buffer pointer low byte
LAB_F533
  LDA ROBUF+1     ; get RS-232 Tx buffer pointer high byte
  BNE LAB_F53C    ; branch if buffer already set

  DEY       ; decrement Rx buffer pointer high byte, 256 byte buffer
  STY ROBUF+1     ; set RS-232 Tx buffer pointer high byte
  STX ROBUF     ; set RS-232 Tx buffer pointer low byte
LAB_F53C
  SEC       ; non-standard exit, Cb set
  LDA #$F0      ; non-standard exit, $F0 error code
  JMP LAB_FE7B    ; set the top of memory and return


;***********************************************************************************;
;
; load RAM from a device

; This routine will load data bytes from any input device directly into the memory
; of the computer. It can also be used for a verify operation comparing data from a
; device with the data already in memory, leaving the data stored in RAM unchanged.

; The accumulator must be set to 0 for a load operation or 1 for a verify. If the
; input device was OPENed with a secondary address of 0 the header information from
; device will be ignored. In this case .X.Y must contain the starting address for the
; load. If the device was addressed with a secondary address of 1 or 2 the data will
; load into memory starting at the location specified by the header. This routine
; returns the address of the highest RAM location which was loaded.

; Before this routine can be called, the SETLFS and SETNAM routines must be called.

FLOAD
  STX MEMUSS      ; set load start address low byte
  STY MEMUSS+1    ; set load start address high byte
  JMP (ILOAD)     ; do LOAD vector, usually points to FLOAD2


;***********************************************************************************;
;
; load

FLOAD2
  STA VERCK     ; save load/verify flag
  LDA #$00      ; clear .A
  STA STATUS      ; clear I/O status byte
  LDA FA      ; get device number
  BNE LAB_F556    ; branch if not keyboard

          ; can't load from keyboard so ..
LAB_F553
  JMP FE_ILDEV    ; do illegal device number and return

LAB_F556
  CMP #$03      ; compare device number with screen
  BEQ LAB_F553    ; if screen go do illegal device number and return

  BCC LAB_F5CA    ; branch if less than screen

          ; else is serial bus device
  LDY FNLEN     ; get file name length
  BNE LAB_F563    ; branch if not null name

  JMP FE_MISFN    ; else do missing file name error and return

LAB_F563
  JSR PATCH1      ; get secondary address and print "SEARCHING..."
  LDA #$60      ; set secondary address to $00
  STA SA      ; save secondary address
  JSR SERNAME     ; send secondary address and filename
  LDA FA      ; get device number
  JSR FTALK     ; command a serial bus device to TALK
  LDA SA      ; get secondary address
  JSR FTKSA     ; send secondary address after TALK
  JSR FACPTR      ; input a byte from the serial bus
  STA EAL     ; save program start address low byte
  LDA STATUS      ; get I/O status byte
  LSR       ; shift time out read ..
  LSR       ; .. into carry bit
  BCS LAB_F5C7    ; if timed out go do file not found error and return

  JSR FACPTR      ; input a byte from the serial bus
  STA EAL+1     ; save program start address high byte
  JSR PATCH2      ; set LOAD address if secondary address = 0
LAB_F58A
  LDA #$FD      ; mask xxxx xx0x, clear time out read bit
  AND STATUS      ; mask I/O status byte
  STA STATUS      ; set I/O status byte
  JSR STOP      ; scan stop key, return Zb = 1 = [STOP]
  BNE LAB_F598    ; branch if not [STOP]

  JMP LAB_F6CB    ; else close the serial bus device and flag stop

LAB_F598
  JSR FACPTR      ; input a byte from the serial bus
  TAX       ; copy byte
  LDA STATUS      ; get I/O status byte
  LSR       ; shift time out read ..
  LSR       ; .. into carry bit
  BCS LAB_F58A    ; if timed out clear I/O status and retry

  TXA       ; copy received byte back
  LDY VERCK     ; get load/verify flag
  BEQ LAB_F5B3    ; branch if load

          ; else is verify
  LDY #$00      ; clear index
  CMP (EAL),Y     ; compare byte with previously loaded byte
  BEQ LAB_F5B5    ; branch if match

  LDA #$10      ; set read error bit
  JSR ORIOST      ; OR into I/O status byte
  .byte $2C     ; makes next line BIT $AE91
LAB_F5B3
  STA (EAL),Y     ; save byte to memory
LAB_F5B5
  INC EAL     ; increment save pointer low byte
  BNE LAB_F5BB    ; if no rollover skip the high byte increment

  INC EAL+1     ; else increment save pointer high byte
LAB_F5BB
  BIT STATUS      ; test I/O status byte
  BVC LAB_F58A    ; loop if not end of file

  JSR FUNTLK      ; command the serial bus to UNTALK
  JSR LAB_F6DA    ; close serial bus device
  BCC LAB_F641    ; if no error go flag ok and exit

LAB_F5C7
  JMP FE_NTFND    ; do file not found error and return

LAB_F5CA
  CMP #$02      ; compare device with RS-232 device
  BNE LOADTP      ; if not RS-232 device continue

  JMP RSDVCERR    ; else do illegal device number and return

LOADTP
  JSR TPBUFA      ; get tape buffer start pointer in .X.Y
  BCS LAB_F5D9    ; branch if >= $0200

  JMP FE_ILDEV    ; do illegal device number and return

LAB_F5D9
  JSR CSTEL     ; wait for PLAY
  BCS LAB_F646    ; exit if STOP was pressed

  JSR SRCHING     ; print "SEARCHING..."
LAB_F5E1
  LDA FNLEN     ; get file name length
  BEQ LAB_F5EE
  JSR FNDHDR      ; find specific tape header
  BCC LAB_F5F5    ; if no error continue

  BEQ LAB_F646    ; exit if end of tape found

  BCS LAB_F5C7    ; exit on error

LAB_F5EE
  JSR FAH     ; find tape header, exit with header in buffer
  BEQ LAB_F646    ; exit if end of tape found

  BCS LAB_F5C7    ; exit on error

LAB_F5F5
  LDA STATUS      ; get I/O status byte
  AND #$10      ; mask 000x 0000, read error
  SEC       ; flag fail
  BNE LAB_F646    ; if read error just exit

  CPX #$01      ; compare file type with relocatable program
  BEQ LAB_F611    ; branch if relocatable program

  CPX #$03      ; compare file type with non relocatable program
  BNE LAB_F5E1    ; branch if not non relocatable program

LAB_F604
  LDY #$01      ; index to start address
  LDA (TAPE1),Y   ; get start address low byte
  STA MEMUSS      ; save load start address low byte
  INY       ; increment index
  LDA (TAPE1),Y   ; get start address high byte
  STA MEMUSS+1    ; set load start address high byte
  BCS LAB_F615    ; branch always

LAB_F611
  LDA SA      ; get secondary address
  BNE LAB_F604    ; branch if not relocatable

LAB_F615
  LDY #$03      ; index to end address low byte
  LDA (TAPE1),Y   ; get end address low byte
  LDY #$01      ; index to start address low byte
  SBC (TAPE1),Y   ; subtract start address low byte
  TAX       ; copy file length low byte
  LDY #$04      ; index to end address high byte
  LDA (TAPE1),Y   ; get end address high byte
  LDY #$02      ; index to start address high byte
  SBC (TAPE1),Y   ; subtract start address high byte
  TAY       ; copy file length high byte
  CLC       ; clear carry for add
  TXA       ; get file length low byte back
  ADC MEMUSS      ; add load start address low byte
  STA EAL     ; save LOAD end pointer low byte
  TYA       ; get file length high byte back
  ADC MEMUSS+1    ; add load start address high byte
  STA EAL+1     ; save LOAD end pointer high byte
  LDA MEMUSS      ; get load start address low byte
  STA STAL      ; save I/O start address low byte
  LDA MEMUSS+1    ; get load start address high byte
  STA STAL+1      ; save I/O start address high byte
  JSR LDVRMSG     ; display "LOADING" or "VERIFYING"
  JSR RBLK      ; do the tape read
  .byte $24     ; makes next line BIT $18, keep the error flag in Cb
LAB_F641
  CLC       ; flag ok
  LDX EAL     ; get the LOAD end pointer low byte
  LDY EAL+1     ; get the LOAD end pointer high byte
LAB_F646
  RTS


;***********************************************************************************;
;
; print "SEARCHING..."

SRCHING
  LDA MSGFLG      ; get KERNAL message mode flag
  BPL LAB_F669    ; exit if control messages off

  LDY #KM_SRCHG-KMSGTBL
          ; index to "SEARCHING "
  JSR KMSGSHOW    ; display KERNAL I/O message
  LDA FNLEN     ; get file name length
  BEQ LAB_F669    ; exit if null name

  LDY #KM_FOR-KMSGTBL
          ; else index to "FOR "
  JSR KMSGSHOW    ; display KERNAL I/O message

; print file name

FILENAME
  LDY FNLEN     ; get file name length
  BEQ LAB_F669    ; exit if null file name

  LDY #$00      ; clear index
LAB_F65F
  LDA (FNADR),Y   ; get file name byte
  JSR CHROUT      ; output character to channel
  INY       ; increment index
  CPY FNLEN     ; compare with file name length
  BNE LAB_F65F    ; loop if more to do

LAB_F669
  RTS

; display "LOADING" or "VERIFYING"

LDVRMSG
  LDY #KM_LODNG-KMSGTBL
          ; point to "LOADING"
  LDA VERCK     ; get load/verify flag
  BEQ LAB_F672    ; branch if load

  LDY #KM_VFYNG-KMSGTBL
          ; point to "VERIFYING"
LAB_F672
  JMP SPMSG     ; display KERNAL I/O message if in direct mode and return


;***********************************************************************************;
;
; save RAM to device, .A = index to start address, .X.Y = end address low/high

; This routine saves a section of memory. Memory is saved from an indirect address
; on page 0 specified by A, to the address stored in .X.Y, to a logical file. The
; SETLFS and SETNAM routines must be used before calling this routine. However, a
; file name is not required to SAVE to device 1, the cassette. Any attempt to save to
; other devices without using a file name results in an error.

; NOTE: device 0, the keyboard, and device 3, the screen, cannot be SAVEd to. If
; the attempt is made, an error will occur, and the SAVE stopped.

FSAVE
  STX EAL     ; save end address low byte
  STY EAL+1     ; save end address high byte
  TAX       ; copy index to start pointer
  LDA $00,X     ; get start address low byte
  STA STAL      ; set I/O start address low byte
  LDA $01,X     ; get start address high byte
  STA STAL+1      ; set I/O start address high byte
  JMP (ISAVE)     ; go save, usually points to FSAVE2


;***********************************************************************************;
;
; save

FSAVE2
  LDA FA      ; get device number
  BNE LAB_F68C    ; branch if not keyboard

          ; else ..
LAB_F689
  JMP FE_ILDEV    ; do illegal device number and return

LAB_F68C
  CMP #$03      ; compare device number with screen
  BEQ LAB_F689    ; if screen do illegal device number and return

  BCC SAVETP      ; branch if < screen

          ; is greater than screen so is serial bus
  LDA #$61      ; set secondary address to $01
          ; when a secondary address is to be sent to a device on
          ; the serial bus the address must first be ORed with $60
  STA SA      ; save secondary address
  LDY FNLEN     ; get file name length
  BNE LAB_F69D    ; branch if filename not null

  JMP FE_MISFN    ; else do missing file name error and return

LAB_F69D
  JSR SERNAME     ; send secondary address and filename
  JSR SAVING      ; print "SAVING <file name>"
  LDA FA      ; get device number
  JSR FLISTEN     ; command devices on the serial bus to LISTEN
  LDA SA      ; get secondary address
  JSR FSECOND     ; send secondary address after LISTEN
  LDY #$00      ; clear index
  JSR RD300     ; copy I/O start address to buffer address
  LDA SAL     ; get buffer address low byte
  JSR FCIOUT      ; output a byte to the serial bus
  LDA SAL+1     ; get buffer address high byte
  JSR FCIOUT      ; output a byte to the serial bus
LAB_F6BC
  JSR VPRTY     ; check read/write pointer, return Cb = 1 if pointer >= end
  BCS LAB_F6D7    ; go do UNLISTEN if at end

  LDA (SAL),Y     ; get byte from buffer
  JSR FCIOUT      ; output a byte to the serial bus
  JSR STOP      ; scan stop key
  BNE LAB_F6D2    ; if stop not pressed go increment pointer and loop for next

          ; else ..

; close the serial bus device and flag stop

LAB_F6CB
  JSR LAB_F6DA    ; close serial bus device
  LDA #ER_STOP    ; terminated by STOP key
  SEC       ; flag stop
  RTS

LAB_F6D2
  JSR WRT62     ; increment read/write pointer
  BNE LAB_F6BC    ; loop, branch always


;***********************************************************************************;
;
; command serial bus to UNLISTEN then close channel

LAB_F6D7
  JSR FUNLSN      ; command the serial bus to UNLISTEN

; close the serial bus device

LAB_F6DA
  BIT SA      ; test the secondary address
  BMI LAB_F6EF    ; if already closed just exit

  LDA FA      ; get device number
  JSR FLISTEN     ; command devices on the serial bus to LISTEN
  LDA SA      ; get secondary address
  AND #$EF      ; mask the channel number
  ORA #$E0      ; OR with the CLOSE command
  JSR FSECOND     ; send secondary address after LISTEN
  JSR FUNLSN      ; command the serial bus to UNLISTEN
LAB_F6EF
  CLC       ; flag ok
  RTS

SAVETP
  CMP #$02      ; compare device with RS-232 device
  BNE LAB_F6F8    ; branch if not RS-232 device

  JMP RSDVCERR    ; else do illegal device number and return

LAB_F6F8
  JSR TPBUFA      ; get tape buffer start pointer in .X.Y
  BCC LAB_F689    ; if < $0200 do illegal device number and return

  JSR CSTE2     ; wait for PLAY/RECORD
  BCS LAB_F727    ; exit if STOP was pressed

  JSR SAVING      ; print "SAVING <file name>"
  LDX #$03      ; set header for a non relocatable program file
  LDA SA      ; get secondary address
  AND #$01      ; mask non relocatable bit
  BNE LAB_F70F    ; branch if non relocatable program

  LDX #$01      ; else set header for a relocatable program file
LAB_F70F
  TXA       ; copy header type to .A
  JSR TAPEH     ; write tape header
  BCS LAB_F727    ; exit if error

  JSR LAB_F8E6    ; do tape write, 20 cycle count
  BCS LAB_F727    ; exit if error

  LDA SA      ; get secondary address
  AND #$02      ; mask end of tape flag
  BEQ LAB_F726    ; branch if not end of tape

  LDA #$05      ; else set logical end of the tape
  JSR TAPEH     ; write tape header
  .byte $24     ; makes next line BIT $18 so Cb is not changed
LAB_F726
  CLC       ; flag ok
LAB_F727
  RTS


;***********************************************************************************;
;
; print "SAVING <file name>"

SAVING
  LDA MSGFLG      ; get KERNAL message mode flag
  BPL LAB_F727    ; exit if control messages off

  LDY #KM_SAVNG-KMSGTBL
          ; index to "SAVING "
  JSR KMSGSHOW    ; display KERNAL I/O message
  JMP FILENAME    ; print file name and return


;***********************************************************************************;
;
; increment real time clock

; This routine updates the system clock. Normally this routine is called by the
; normal KERNAL interrupt routine every 1/60th of a second. If the user program
; processes its own interrupts this routine must be called to update the time. Also,
; the STOP key routine must be called if the stop key is to remain functional.

FUDTIM
  LDX #$00      ; clear .X
  INC TIME+2      ; increment jiffy low byte
  BNE LAB_F740    ; if no rollover skip the mid byte increment

  INC TIME+1      ; increment jiffy mid byte
  BNE LAB_F740    ; if no rollover skip the high byte increment

  INC TIME      ; increment jiffy high byte

          ; now subtract a days worth of jiffies from current count
          ; and remember only the Cb result
LAB_F740
  SEC       ; set carry for subtract
  LDA TIME+2      ; get jiffy clock low byte
  SBC #$01      ; subtract $4F1A01 low byte
  LDA TIME+1      ; get jiffy clock mid byte
  SBC #$1A      ; subtract $4F1A01 mid byte
  LDA TIME      ; get jiffy clock high byte
  SBC #$4F      ; subtract $4F1A01 high byte
  BCC LAB_F755    ; branch if less than $4F1A01 jiffies

          ; else ..
  STX TIME      ; clear jiffies high byte
  STX TIME+1      ; clear jiffies mid byte
  STX TIME+2      ; clear jiffies low byte
          ; this is wrong, there are $4F1A00 jiffies in a day so
          ; the reset to zero should occur when the value reaches
          ; $4F1A00 and not $4F1A01. this would give an extra jiffy
          ; every day and a possible TI value of 24:00:00
LAB_F755
  LDA VIA2PA2     ; get VIA 2 DRA, keyboard row, no handshake
  CMP VIA2PA2     ; compare with self
  BNE LAB_F755    ; loop if changing

  STA STKEY     ; save VIA 2 DRA, keyboard row
  RTS


;***********************************************************************************;
;
; read the real time clock

; This routine returns the time, in jiffies, in .Y.X.A. The accumulator contains the
; most significant byte.

FRDTIM
  SEI       ; disable interrupts
  LDA TIME+2      ; get jiffy clock low byte
  LDX TIME+1      ; get jiffy clock mid byte
  LDY TIME      ; get jiffy clock high byte


;***********************************************************************************;
;
; set the real time clock

; The system clock is maintained by an interrupt routine that updates the clock
; every 1/60th of a second. The clock is three bytes long which gives the capability
; to count from zero up to 5,184,000 jiffies - 24 hours plus one jiffy. At that point
; the clock resets to zero. Before calling this routine to set the clock the new time,
; in jiffies, should be in .Y.X.A, the accumulator containing the most significant byte.

FSETTIM
  SEI       ; disable interrupts
  STA TIME+2      ; save jiffy clock low byte
  STX TIME+1      ; save jiffy clock mid byte
  STY TIME      ; save jiffy clock high byte
  CLI       ; enable interrupts
  RTS


;***********************************************************************************;
;
; scan stop key, return Zb = 1 = [STOP]

; If the STOP key on the keyboard is pressed when this routine is called the Z flag
; will be set. All other flags remain unchanged. If the STOP key is not pressed then
; the accumulator will contain a byte representing the last row of the keyboard scan.

; The user can also check for certain other keys this way.

FSTOP
  LDA STKEY     ; get keyboard row
  CMP #$FE      ; compare with r0 down
  BNE LAB_F77D    ; branch if not just r0

  PHP       ; save status
  JSR CLRCHN      ; close input and output channels
  STA NDX     ; save keyboard buffer length
  PLP       ; restore status
LAB_F77D
  RTS


;***********************************************************************************;
;
; file error messages

FE_2MNYF
  LDA #$01      ; too many files
  .byte $2C     ; makes next line BIT $02A9
FE_ALOPN
  LDA #$02      ; file already open
  .byte $2C     ; makes next line BIT $03A9
FE_NTOPN
  LDA #$03      ; file not open
  .byte $2C     ; makes next line BIT $04A9
FE_NTFND
  LDA #$04      ; file not found
  .byte $2C     ; makes next line BIT $05A9
FE_DVNTP
  LDA #$05      ; device not present
  .byte $2C     ; makes next line BIT $06A9
FE_NTINP
  LDA #$06      ; not input file
  .byte $2C     ; makes next line BIT $07A9
FE_NTOUT
  LDA #$07      ; not output file
  .byte $2C     ; makes next line BIT $08A9
FE_MISFN
  LDA #$08      ; missing file name
  .byte $2C     ; makes next line BIT $09A9
FE_ILDEV
  LDA #$09      ; illegal device number

  PHA       ; save error #
  JSR CLRCHN      ; close input and output channels
  LDY #KM_IOERR-KMSGTBL
          ; index to "I/O ERROR #"
  BIT MSGFLG      ; test KERNAL message mode flag
  BVC LAB_F7AC    ; exit if error messages off

  JSR KMSGSHOW    ; display KERNAL I/O message
  PLA       ; restore error #
  PHA       ; copy error #
  ORA #'0'      ; convert to ASCII
  JSR CHROUT      ; output character to channel
LAB_F7AC
  PLA       ; pull error number
  SEC       ; flag error
  RTS


;***********************************************************************************;
;
; find tape header, exit with header in buffer

FAH
  LDA VERCK     ; get load/verify flag
  PHA       ; save load/verify flag
  JSR RDTPBLKS    ; initiate tape read
  PLA       ; restore load/verify flag
  STA VERCK     ; save load/verify flag
  BCS LAB_F7E6    ; exit if error

  LDY #$00      ; clear index
  LDA (TAPE1),Y   ; read first byte from tape buffer
  CMP #$05      ; compare with logical end of the tape
  BEQ LAB_F7E6    ; exit if end of the tape

  CMP #$01      ; compare with header for a relocatable program file
  BEQ LAB_F7CE    ; branch if program file header

  CMP #$03      ; compare with header for a non relocatable program file
  BEQ LAB_F7CE    ; branch if program file header

  CMP #$04      ; compare with data file header
  BNE FAH     ; if data file loop to find tape header

          ; was program file header
LAB_F7CE
  TAX       ; copy header type
  BIT MSGFLG      ; get KERNAL message mode flag
  BPL LAB_F7E4    ; exit if control messages off

  LDY #KM_FOUND-KMSGTBL
          ; index to "FOUND "
  JSR KMSGSHOW    ; display KERNAL I/O message
  LDY #$05      ; index to tape filename
LAB_F7DA
  LDA (TAPE1),Y   ; get byte from tape buffer
  JSR CHROUT      ; output character to channel
  INY       ; increment index
  CPY #$15      ; compare with end+1
  BNE LAB_F7DA    ; loop if more to do

LAB_F7E4
  CLC       ; flag no error
  DEY       ; decrement index
LAB_F7E6
  RTS


;***********************************************************************************;
;
; write tape header

TAPEH
  STA PTR1      ; save header type
  JSR TPBUFA      ; get tape buffer start pointer in .X.Y
  BCC LAB_F84C    ; exit if < $0200

  LDA STAL+1      ; get I/O start address high byte
  PHA       ; save it
  LDA STAL      ; get I/O start address low byte
  PHA       ; save it
  LDA EAL+1     ; get tape end address high byte
  PHA       ; save it
  LDA EAL     ; get tape end address low byte
  PHA       ; save it

  LDY #$BF      ; index to header end
  LDA #' '      ; clear byte, [SPACE]
LAB_F7FE
  STA (TAPE1),Y   ; clear header byte
  DEY       ; decrement index
  BNE LAB_F7FE    ; loop if more to do

  LDA PTR1      ; get header type back
  STA (TAPE1),Y   ; write to header
  INY       ; increment index
  LDA STAL      ; get I/O start address low byte
  STA (TAPE1),Y   ; write to header
  INY       ; increment index
  LDA STAL+1      ; get I/O start address high byte
  STA (TAPE1),Y   ; write to header
  INY       ; increment index
  LDA EAL     ; get tape end address low byte
  STA (TAPE1),Y   ; write to header
  INY       ; increment index
  LDA EAL+1     ; get tape end address high byte
  STA (TAPE1),Y   ; write to header
  INY       ; increment index
  STY PTR2      ; save index
  LDY #$00      ; clear .Y
  STY PTR1      ; clear name index
LAB_F822
  LDY PTR1      ; get name index
  CPY FNLEN     ; compare with file name length
  BEQ LAB_F834    ; exit loop if all done

  LDA (FNADR),Y   ; get file name byte
  LDY PTR2      ; get buffer index
  STA (TAPE1),Y   ; save file name byte to buffer
  INC PTR1      ; increment file name index
  INC PTR2      ; increment tape buffer index
  BNE LAB_F822    ; loop, branch always

LAB_F834
  JSR LDAD1     ; set tape buffer start and end pointers
  LDA #$69      ; set write lead cycle count
  STA RIPRTY      ; save write lead cycle count
  JSR LAB_F8EA    ; do tape write, no cycle count set
  TAY       ;.
  PLA       ; pull tape end address low byte
  STA EAL     ; restore it
  PLA       ; pull tape end address high byte
  STA EAL+1     ; restore it
  PLA       ; pull I/O start address low byte
  STA STAL      ; restore it
  PLA       ; pull I/O start address high byte
  STA STAL+1      ; restore it
  TYA       ;.
LAB_F84C
  RTS


;***********************************************************************************;
;
; get tape buffer start pointer

TPBUFA
  LDX TAPE1     ; get tape buffer start pointer low byte
  LDY TAPE1+1     ; get tape buffer start pointer high byte
  CPY #$02      ; compare high byte with $02xx
  RTS


;***********************************************************************************;
;
; set tape buffer start and end pointers

LDAD1
  JSR TPBUFA      ; get tape buffer start pointer in .X.Y
  TXA       ; copy tape buffer start pointer low byte
  STA STAL      ; save as I/O address low byte
  CLC       ; clear carry for add
  ADC #$C0      ; add buffer length low byte
  STA EAL     ; save tape buffer end pointer low byte
  TYA       ; copy tape buffer start pointer high byte
  STA STAL+1      ; save as I/O address high byte
  ADC #$00      ; add buffer length high byte
  STA EAL+1     ; save tape buffer end pointer high byte
  RTS


;***********************************************************************************;
;
; find specific tape header

FNDHDR
  JSR FAH     ; find tape header, exit with header in buffer
  BCS LAB_F889    ; just exit if error

  LDY #$05      ; index to name
  STY PTR2      ; save as tape buffer index
  LDY #$00      ; clear .Y
  STY PTR1      ; save as name buffer index
LAB_F874
  CPY FNLEN     ; compare with file name length
  BEQ LAB_F888    ; ok exit if match

  LDA (FNADR),Y   ; get file name byte
  LDY PTR2      ; get index to tape buffer
  CMP (TAPE1),Y   ; compare with tape header name byte
  BNE FNDHDR      ; if no match go get next header

  INC PTR1      ; else increment name buffer index
  INC PTR2      ; increment tape buffer index
  LDY PTR1      ; get name buffer index
  BNE LAB_F874    ; loop, branch always

LAB_F888
  CLC       ; flag ok
LAB_F889
  RTS


;***********************************************************************************;
;
; bump tape pointer

JTP20
  JSR TPBUFA      ; get tape buffer start pointer in .X.Y
  INC BUFPNT      ; increment tape buffer index
  LDY BUFPNT      ; get tape buffer index
  CPY #$C0      ; compare with buffer length
  RTS


;***********************************************************************************;
;
; wait for PLAY

CSTEL
  JSR CS10      ; return cassette sense in Zb
  BEQ LAB_F8B5    ; exit if switch closed

          ; cassette switch was open
  LDY #KM_PRPLY-KMSGTBL
          ; index to "PRESS PLAY ON TAPE"
LAB_F89B
  JSR KMSGSHOW    ; display KERNAL I/O message
LAB_F89E
  JSR TSTOP     ; scan stop key and flag abort if pressed
          ; note if STOP was pressed the return is to the
          ; routine that called this one and not here
  JSR CS10      ; return cassette sense in Zb
  BNE LAB_F89E    ; loop if cassette switch open

  LDY #KM_OK-KMSGTBL
          ; index to "OK"
  JMP KMSGSHOW    ; display KERNAL I/O message and return


;***********************************************************************************;
;
; return cassette sense in Zb

CS10
  LDA #$40      ; mask for cassette switch
  BIT VIA1PA2     ; test VIA 1 DRA, no handshake
  BNE LAB_F8B5    ; branch if cassette sense high

  BIT VIA1PA2     ; test VIA 1 DRA again
LAB_F8B5
  CLC
  RTS


;***********************************************************************************;
;
; wait for PLAY/RECORD

CSTE2
  JSR CS10      ; return cassette sense in Zb
  BEQ LAB_F8B5    ; exit if switch closed

          ; cassette switch was open
  LDY #KM_RECPY-KMSGTBL
          ; index to "PRESS RECORD & PLAY ON TAPE"
  BNE LAB_F89B    ; display message and wait for switch, branch always


;***********************************************************************************;
;
; initiate tape read

RDTPBLKS
  LDA #$00      ; clear .A
  STA STATUS      ; clear I/O status byte
  STA VERCK     ; clear the load/verify flag
  JSR LDAD1     ; set tape buffer start and end pointers
RBLK
  JSR CSTEL     ; wait for PLAY
  BCS LAB_F8ED    ; exit if STOP was pressed, uses further BCS at target
          ; address to reach final target at LAB_F957

  SEI       ; disable interrupts
  LDA #$00      ; clear .A
  STA RIDATA      ; clear tape input status
  STA BITTS     ; clear tape read ready
  STA CMP0      ; clear tape timing constant min byte
  STA PTR1      ; clear tape pass 1 error log/char buffer
  STA PTR2      ; clear tape pass 2 error log corrected
  STA DPSW      ; clear tape dipole switch
  LDA #$82      ; enable CA1 interrupt
  LDX #$0E      ; set index for tape read vector
  BNE TAPE      ; go do tape read/write, branch always


;***********************************************************************************;
;
; initiate tape write

WBLK
  JSR LDAD1     ; set tape buffer start and end pointers

; do tape write, 20 cycle count

LAB_F8E6
  LDA #$14      ; set write lead cycle count
  STA RIPRTY      ; save write lead cycle count

; do tape write, no cycle count set

LAB_F8EA
  JSR CSTE2     ; wait for PLAY/RECORD
LAB_F8ED
  BCS LAB_F957    ; if STOPped clear save IRQ address and exit

  SEI       ; disable interrupts
  LDA #$A0      ; enable VIA 2 T2 interrupt
  LDX #$08      ; set index for tape write tape leader vector


;***********************************************************************************;
;
; tape read/write

TAPE
  LDY #$7F      ; disable all interrupts
  STY VIA2IER     ; set VIA 2 IER, disable interrupts
  STA VIA2IER     ; set VIA 2 IER, enable interrupts according to .A
  JSR RSPAUSE     ; check RS-232 bus idle
  LDA CINV      ; get IRQ vector low byte
  STA IRQTMP      ; save IRQ vector low byte
  LDA CINV+1      ; get IRQ vector high byte
  STA IRQTMP+1    ; save IRQ vector high byte
  JSR LAB_FCFB    ; set tape vector
  LDA #$02      ; set copies remaining. the first copy is the load copy, the
          ; second copy is the verify copy
  STA FSBLK     ; save copies remaining
  JSR NEWCH     ; new tape byte setup
  LDA VIA1PCR     ; get VIA 1 PCR
  AND #$FD      ; CA2 low, turn on tape motor
  ORA #$0C      ; manual output mode
  STA VIA1PCR     ; set VIA 1 PCR
  STA CAS1      ; set tape motor interlock

          ; 326656 cycle delay, allow tape motor speed to stabilise
  LDX #$FF      ; outer loop count
LAB_F923
  LDY #$FF      ; inner loop count
LAB_F925
  DEY       ; decrement inner loop count
  BNE LAB_F925    ; loop if more to do

  DEX       ; decrement outer loop count
  BNE LAB_F923    ; loop if more to do

  STA VIA2T2CH    ; set VIA 2 T2C_h
  CLI       ; enable tape interrupts
LAB_F92F
  LDA IRQTMP+1    ; get saved IRQ high byte
  CMP CINV+1      ; compare with the current IRQ high byte
  CLC       ; flag ok
  BEQ LAB_F957    ; if tape write done go clear saved IRQ address and exit

  JSR TSTOP     ; scan stop key and flag abort if pressed
          ; note if STOP was pressed the return is to the
          ; routine that called this one and not here
  LDA VIA2IFR     ; get VIA 2 IFR
  AND #$40      ; mask T1 interrupt
  BEQ LAB_F92F    ; loop if not T1 interrupt

          ; else increment jiffy clock
  LDA VIA1T1CL    ; get VIA 1 T1C_l, clear T1 interrupt
  JSR FUDTIM      ; increment the real time clock
  JMP LAB_F92F    ; loop


;***********************************************************************************;
;
; scan stop key and flag abort if pressed

TSTOP
  JSR STOP      ; scan stop key
  CLC       ; flag no stop
  BNE LAB_F95C    ; exit if no stop

  JSR TNIF      ; restore everything for STOP
  SEC       ; flag stopped
  PLA       ; dump return address low byte
  PLA       ; dump return address high byte


;***********************************************************************************;
;
; clear saved IRQ address

LAB_F957
  LDA #$00      ; clear .A
  STA IRQTMP+1    ; clear saved IRQ address high byte
LAB_F95C
  RTS


;***********************************************************************************;
;
;## set timing

STT1
  STX CMP0+1      ; save tape timing constant max byte
  LDA CMP0      ; get tape timing constant min byte
  ASL       ; *2
  ASL       ; *4
  CLC       ; clear carry for add
  ADC CMP0      ; add tape timing constant min byte *5
  CLC       ; clear carry for add
  ADC CMP0+1      ; add tape timing constant max byte
  STA CMP0+1      ; save tape timing constant max byte
  LDA #$00      ;.
  BIT CMP0      ; test tape timing constant min byte
  BMI LAB_F972    ; branch if b7 set

  ROL       ; else shift carry into ??
LAB_F972
  ASL CMP0+1      ; shift tape timing constant max byte
  ROL       ;.
  ASL CMP0+1      ; shift tape timing constant max byte
  ROL       ;.
  TAX       ;.
LAB_F979
  LDA VIA2T2CL    ; get VIA 2 T2C_l
  CMP #$15      ;.compare with ??
  BCC LAB_F979    ; loop if less

  ADC CMP0+1      ; add tape timing constant max byte
  STA VIA2T1CL    ; set VIA 2 T1C_l
  TXA       ;.
  ADC VIA2T2CH    ; add VIA 2 T2C_h
  STA VIA2T1CH    ; set VIA 2 T1C_h
  CLI       ; enable interrupts
  RTS


;***********************************************************************************;
;
;;  On Commodore computers, the streams consist of four kinds of symbols
;;  that denote different kinds of low-to-high-to-low transitions on the
;;  read or write signals of the Commodore cassette interface.
;;
;;  A A break in the communications, or a pulse with very long cycle
;;    time.
;;
;;  B A short pulse, whose cycle time typically ranges from 296 to 424
;;    microseconds, depending on the computer model.
;;
;;  C A medium-length pulse, whose cycle time typically ranges from
;;    440 to 576 microseconds, depending on the computer model.
;;
;;  D A long pulse, whose cycle time typically ranges from 600 to 744
;;    microseconds, depending on the computer model.
;;
;;   The actual interpretation of the serial data takes a little more work to
;; explain. The typical ROM tape loader (and the turbo loaders) will
;; initialise a timer with a specified value and start it counting down. If
;; either the tape data changes or the timer runs out, an IRQ will occur. The
;; loader will determine which condition caused the IRQ. If the tape data
;; changed before the timer ran out, we have a short pulse, or a "0" bit. If
;; the timer ran out first, we have a long pulse, or a "1" bit. Doing this
;; continuously and we decode the entire file.

; read tape bits, IRQ routine

; read T2C which has been counting down from $FFFF. subtract this from $FFFF

READT
  LDX VIA2T2CH    ; get VIA 2 T2C_h
  LDY #$FF      ;.set $FF
  TYA       ; .A = $FF
  SBC VIA2T2CL    ; subtract VIA 2 T2C_l
  CPX VIA2T2CH    ; compare VIA 2 T2C_h with previous
  BNE READT     ; loop if timer low byte rolled over

  STX CMP0+1      ; save tape timing constant max byte
  TAX       ;.copy $FF - T2C_l
  STY VIA2T2CL    ; set VIA 2 T2C_l to $FF
  STY VIA2T2CH    ; set VIA 2 T2C_h to $FF
  TYA       ;.$FF
  SBC CMP0+1      ; subtract tape timing constant max byte
          ; .A = $FF - T2C_h
  STX CMP0+1      ; save tape timing constant max byte
          ; CMP0+1 = $FF - T2C_l
  LSR       ; .A = $FF - T2C_h >> 1
  ROR CMP0+1      ; shift tape timing constant max byte
          ; CMP0+1 = $FF - T2C_l >> 1
  LSR       ; .A = $FF - T2C_h >> 1
  ROR CMP0+1      ; shift tape timing constant max byte
          ; CMP0+1 = $FF - T2C_l >> 1
  LDA CMP0      ; get tape timing constant min byte
  CLC       ; clear carry for add
  ADC #$3C      ;.
  BIT VIA2PA1     ; test VIA 2 DRA, keyboard row
  CMP CMP0+1      ; compare with tape timing constant max byte
          ; compare with ($FFFF - T2C) >> 2
  BCS LAB_FA06    ;.branch if min + $3C >= ($FFFF - T2C) >> 2

          ;.min + $3C < ($FFFF - T2C) >> 2
  LDX DPSW      ; get tape byte received flag
  BEQ LAB_F9C3    ; branch if not byte received

  JMP TPSTORE     ;.store tape character

LAB_F9C3
  LDX PCNTR     ; get tape bit count
  BMI LAB_F9E2    ; branch if character complete

  LDX #$00      ; clear .X
  ADC #$30      ;.
  ADC CMP0      ; add tape timing constant min byte
  CMP CMP0+1      ; compare with tape timing constant max byte
  BCS LAB_F9ED    ;.

  INX       ;.
  ADC #$26      ;.
  ADC CMP0      ; add tape timing constant min byte
  CMP CMP0+1      ; compare with tape timing constant max byte
  BCS LAB_F9F1    ;.

  ADC #$2C      ;.
  ADC CMP0      ; add tape timing constant min byte
  CMP CMP0+1      ; compare with tape timing constant max byte
  BCC LAB_F9E5    ;.

LAB_F9E2
  JMP LAB_FA60    ;.

LAB_F9E5
  LDA BITTS     ; get tape read ready
  BEQ LAB_FA06    ; branch if zero

  STA BITCI     ; save tape long word marker
  BNE LAB_FA06    ; branch always

LAB_F9ED
  INC RINONE      ; increment tape dipole count
  BCS LAB_F9F3    ;.

LAB_F9F1
  DEC RINONE      ; decrement tape dipole count
LAB_F9F3
  SEC       ;.
  SBC #$13      ;.
  SBC CMP0+1      ; subtract tape timing constant max byte
  ADC SVXT      ; add timing constant for tape
  STA SVXT      ; save timing constant for tape
  LDA FIRT      ; get tape bit cycle phase
  EOR #$01      ; toggle b0
  STA FIRT      ; save tape bit cycle phase
  BEQ LAB_FA25    ; if first cycle complete go to second cycle

  STX ASCII     ; save bit value
LAB_FA06
  LDA BITTS     ; get tape read ready
  BEQ LAB_FA22    ; exit if zero

  BIT VIA2IFR     ; test get 2 IFR
  BVC LAB_FA22    ; exit if no T1 interrupt

  LDA #$00      ; clear .A
  STA FIRT      ; clear tape bit cycle phase
  LDA PCNTR     ; get tape bit count
  BPL LAB_FA47    ; branch of more bits

  BMI LAB_F9E2    ; branch always

LAB_FA19
  LDX #$A6      ; set timing max byte
  JSR STT1      ; set timing
  LDA PRTY      ; get tape character parity
  BNE LAB_F9E5    ;.
LAB_FA22
  JMP _RTI      ; restore registers and exit interrupt

LAB_FA25
  LDA SVXT      ; get timing constant for tape
  BEQ LAB_FA30    ;.

  BMI LAB_FA2E    ;.

  DEC CMP0      ; decrement tape timing constant min byte
  .byte $2C     ; makes next line BIT $B0E6
LAB_FA2E
  INC CMP0      ; increment tape timing constant min byte
LAB_FA30
  LDA #$00      ; clear .A
  STA SVXT      ; clear timing constant for tape
  CPX ASCII     ;.
  BNE LAB_FA47    ;.

  TXA       ;.
  BNE LAB_F9E5    ;.

  LDA RINONE      ; get tape dipole count
  BMI LAB_FA06    ;.

  CMP #$10      ;.
  BCC LAB_FA06    ;.

  STA SYNO      ; save leader length
  BCS LAB_FA06    ; branch always

LAB_FA47
  TXA
  EOR PRTY      ; XOR with tape character parity
  STA PRTY      ; save tape character parity
  LDA BITTS     ; get tape read ready
  BEQ LAB_FA22    ; if zero exit interrupt

  DEC PCNTR     ; decrement tape bit count
  BMI LAB_FA19    ; branch if character complete

  LSR ASCII     ; shift dipole into Cb
  ROR MYCH      ; rotate Cb into tape read byte
  LDX #$DA      ; set timing max byte
  JSR STT1      ; set timing
  JMP _RTI      ; restore registers and exit interrupt

LAB_FA60
  LDA SYNO      ; get leader length
  BEQ LAB_FA68    ; branch if no block

  LDA BITTS     ; get tape read ready
  BEQ LAB_FA6C    ;.

LAB_FA68
  LDA PCNTR     ; get tape bit count
  BPL LAB_F9F1    ; branch if more bits

LAB_FA6C
  LSR CMP0+1      ; shift tape timing constant max byte
  LDA #$93      ;.
  SEC       ;.
  SBC CMP0+1      ; subtract tape timing constant max byte
  ADC CMP0      ; add tape timing constant min byte
  ASL       ;.
  TAX       ; copy timing high byte
  JSR STT1      ; set timing
  INC DPSW      ; increment tape dipole switch/byte received flag
  LDA BITTS     ; get tape read ready
  BNE LAB_FA91    ;.

  LDA SYNO      ; get leader length
  BEQ LAB_FAAA    ; branch if no block

  STA BITCI     ; save tape long word marker
  LDA #$00      ; clear .A
  STA SYNO      ; clear leader length
  LDA #$C0      ; enable T1 interrupt
  STA VIA2IER     ; set VIA 2 IER
  STA BITTS     ; save tape read ready
LAB_FA91
  LDA SYNO      ; get leader length
  STA NXTBIT      ;.
  BEQ LAB_FAA0    ;.

  LDA #$00      ; clear .A
  STA BITTS     ; save tape read ready
  LDA #$40      ; disable T1 interrupt
  STA VIA2IER     ; set VIA 2 IER
LAB_FAA0
  LDA MYCH      ; get tape read byte
  STA ROPRTY      ; save tape byte read
  LDA BITCI     ; get tape error flags
  ORA RINONE      ;.
  STA RODATA      ; save tape read errors
LAB_FAAA
  JMP _RTI      ; restore registers and exit interrupt


;***********************************************************************************;
;
;## store character

TPSTORE
  JSR NEWCH     ; new tape byte setup
  STA DPSW      ; clear tape dipole switch/byte received flag
  LDX #$DA      ; set timing max byte
  JSR STT1      ; set timing
  LDA FSBLK     ; get tape copies remaining
  BEQ LAB_FABD    ; branch if all copies done

  STA INBIT     ; save tape read block count
LAB_FABD
  LDA #$0F      ; set block countdown bits
  BIT RIDATA      ; mask from tape input status
  BPL LAB_FADA    ; branch in first block has been loaded

  LDA NXTBIT      ;.
  BNE LAB_FAD3    ;.

  LDX FSBLK     ; get tape copies remaining
  DEX       ; decrement copies remaining
  BNE LAB_FAD7    ; if copies remaining restore registers and exit interrupt

  LDA #$08      ; set long block bit
  JSR ORIOST      ; OR into I/O status byte
  BNE LAB_FAD7    ; restore registers and exit interrupt, branch always

LAB_FAD3
  LDA #$00      ; clear .A
  STA RIDATA      ; clear tape input status flags
LAB_FAD7
  JMP _RTI      ; restore registers and exit interrupt

LAB_FADA
  BVS LAB_FB0D    ; branch if valid data byte received

  BNE LAB_FAF6    ; branch if block countdown bytes received

  LDA NXTBIT      ;.
  BNE LAB_FAD7    ;.

  LDA RODATA      ; get tape read errors
  BNE LAB_FAD7    ; if errors then exit interrupt

  LDA INBIT     ; get tape write leader count
  LSR       ; shift b0 into Cb
  LDA ROPRTY      ; get tape write byte
  BMI LAB_FAF0    ; branch if b7 set

  BCC LAB_FB07    ;.

  CLC       ;.
LAB_FAF0
  BCS LAB_FB07    ;.

  AND #$0F      ; mask block countdown and first block flags
  STA RIDATA      ; clear tape input status flags
LAB_FAF6
  DEC RIDATA      ; decrement block countdown
  BNE LAB_FAD7    ; exit if block countdown bytes received

  LDA #$40      ; set valid block countdown
  STA RIDATA      ; set tape input status flags
  JSR RD300     ; copy I/O start address to buffer address
  LDA #$00      ; clear .A
  STA RIPRTY      ; clear tape read checksum
  BEQ LAB_FAD7    ; exit interrupt always


;***********************************************************************************;
;
;## reset pointer

LAB_FB07
  LDA #$80      ; set first block loaded
  STA RIDATA      ; save tape input status flags
  BNE LAB_FAD7    ; restore registers and exit interrupt, branch always

LAB_FB0D
  LDA NXTBIT      ;.
  BEQ LAB_FB1B    ;.

  LDA #$04      ; set short block bit
  JSR ORIOST      ; OR into I/O status byte
  LDA #$00      ;.
  JMP LAB_FB97    ;.

LAB_FB1B
  JSR VPRTY     ; check read/write pointer, return Cb = 1 if pointer >= end
  BCC LAB_FB23    ; branch if not at end

  JMP LAB_FB95    ;.

LAB_FB23
  LDX INBIT     ; get tape write leader count
  DEX       ; decrement count
  BEQ LAB_FB55    ; branch if all blocks loaded

  LDA VERCK     ; get load/verify flag
  BEQ LAB_FB38    ; branch if load

  LDY #$00      ; clear index
  LDA ROPRTY      ;.get tape byte read
  CMP (SAL),Y     ; compare with byte in buffer
  BEQ LAB_FB38    ; branch if equal

  LDA #$01      ; set read error
  STA RODATA      ; save read errors
LAB_FB38
  LDA RODATA      ; get read errors
  BEQ LAB_FB87    ; branch if no error

  LDX #$3D      ; maximum pass 1 errors
  CPX PTR1      ; compare with tape pass 1 error index
  BCC LAB_FB80    ; branch if space

  LDX PTR1      ; get tape pass 1 error index
  LDA SAL+1     ;.
  STA STACK+1,X   ; store in error log
  LDA SAL     ;.
  STA STACK,X     ; store in error log
  INX       ; increment index
  INX       ; increment index
  STX PTR1      ; store in tape pass 1 error index
  JMP LAB_FB87    ;.

LAB_FB55
  LDX PTR2      ; get tape pass 2 error index
  CPX PTR1      ; compare with tape pass 1 error index
  BEQ LAB_FB90    ; branch if equal

  LDA SAL     ;.
  CMP STACK,X     ; compare with pass 1 error
  BNE LAB_FB90    ; branch if not equal

  LDA SAL+1     ;.
  CMP STACK+1,X   ; compare with pass 1 error
  BNE LAB_FB90    ; branch if not equal

  INC PTR2      ; increment tape pass 2 error index
  INC PTR2      ; increment tape pass 2 error index
  LDA VERCK     ; get load/verify flag
  BEQ LAB_FB7C    ; branch if load

  LDA ROPRTY      ; get tape byte read
  LDY #$00      ; clear index
  CMP (SAL),Y     ; compare with byte in buffer
  BEQ LAB_FB90    ; branch if equal

  INY       ; increment read errors
  STY RODATA      ; save read errors
LAB_FB7C
  LDA RODATA      ; get read errors
  BEQ LAB_FB87    ; branch if no error

LAB_FB80
  LDA #$10      ; set read error bit
  JSR ORIOST      ; OR into I/O status byte
  BNE LAB_FB90    ; branch always

LAB_FB87
  LDA VERCK     ; get load/verify flag
  BNE LAB_FB90    ; branch if verify

  TAY       ; save load/verify flag
  LDA ROPRTY      ; get tape byte read
  STA (SAL),Y     ; save byte into buffer
LAB_FB90
  JSR WRT62     ; increment read/write pointer
  BNE LAB_FBCF    ; restore registers and exit interrupt, branch always

LAB_FB95
  LDA #$80      ; set first block loaded
LAB_FB97
  STA RIDATA      ; save tape input status flags
  LDX FSBLK     ; get tape copies remaining
  DEX       ; decrement copies remaining
  BMI LAB_FBA0    ; branch if become -ve

  STX FSBLK     ; save copies remaining
LAB_FBA0
  DEC INBIT     ; decrement tape write leader count
  BEQ LAB_FBAC    ;.

  LDA PTR1      ; get tape pass 1 error log
  BNE LAB_FBCF    ; if errors restore registers and exit interrupt

  STA FSBLK     ; save tape copies remaining
  BEQ LAB_FBCF    ; restore registers and exit interrupt, branch always

LAB_FBAC
  JSR TNIF      ; restore everything for STOP
  JSR RD300     ; copy I/O start address to buffer address
  LDY #$00      ; clear index
  STY RIPRTY      ; clear tape read checksum
LAB_FBB6
  LDA (SAL),Y     ; get byte from buffer
  EOR RIPRTY      ; XOR with read checksum
  STA RIPRTY      ; save new read checksum
  JSR WRT62     ; increment read/write pointer
  JSR VPRTY     ; check read/write pointer, return Cb = 1 if pointer >= end
  BCC LAB_FBB6    ; loop if not at end

  LDA RIPRTY      ; get computed checksum
  EOR ROPRTY      ; compare with tape write byte
  BEQ LAB_FBCF    ; if checksum ok restore registers and exit interrupt

  LDA #$20      ; else set checksum error bit
  JSR ORIOST      ; OR into I/O status byte
LAB_FBCF
  JMP _RTI      ; restore registers and exit interrupt


;***********************************************************************************;
;
; copy I/O start address to buffer address

RD300
  LDA STAL+1      ; get I/O start address high byte
  STA SAL+1     ; set buffer address high byte
  LDA STAL      ; get I/O start address low byte
  STA SAL     ; set buffer address low byte
  RTS


;***********************************************************************************;
;
; new tape byte setup

NEWCH
  LDA #$08      ; eight bits to do
  STA PCNTR     ; set tape bit count
  LDA #$00      ; clear .A
  STA FIRT      ; clear tape bit cycle phase
  STA BITCI     ; clear tape error flags
  STA PRTY      ; clear tape character parity
  STA RINONE      ; clear tape dipole count
  RTS


;***********************************************************************************;
;
; send LSB from tape write byte to tape

; This routine tests the least significant bit in the tape write byte and sets VIA 2 T2
; depending on the state of the bit. If the bit is a 1 a time of $00B0 cycles is set, if
; the bit is a 0 a time of $0060 cycles is set. Note that this routine does not shift the
; bits of the tape write byte but uses a copy of that byte, the byte itself is shifted
; elsewhere.

TPTOGLE
  LDA ROPRTY      ; get tape write byte
  LSR       ; shift LSB into Cb
  LDA #$60      ; set time constant low byte for bit = 0
  BCC LAB_FBF3    ; branch if bit was 0

; set time constant for bit = 1 and toggle tape

LAB_FBF1
  LDA #$B0      ; set time constant low byte for bit = 1

; write time constant and toggle tape

LAB_FBF3
  LDX #$00      ; set time constant high byte

; write time constant and toggle tape

LAB_FBF5
  STA VIA2T2CL    ; set VIA 2 T2C_l
  STX VIA2T2CH    ; set VIA 2 T2C_h
  LDA VIA2PB      ; get VIA 2 DRB, keyboard column
  EOR #$08      ; toggle tape out bit
  STA VIA2PB      ; set VIA 2 DRB
  AND #$08      ; mask tape out bit
  RTS


;***********************************************************************************;
;
; flag block done and exit interrupt

BLKEND
  SEC       ; set carry flag
  ROR SAL+1     ; set buffer address high byte negative, flag all sync,
          ; data and checksum bytes written
  BMI LAB_FC47    ; restore registers and exit interrupt, branch always


;***********************************************************************************;
;
; tape write, IRQ routine.

; This is the routine that writes the bits to the tape. It is called each time VIA 2 T2
; times out and checks if the start bit is done, if so checks if the data bits are done,
; if so it checks if the byte is done, if so it checks if the synchronisation bytes are
; done, if so it checks if the data bytes are done, if so it checks if the checksum byte
; is done, if so it checks if both the load and verify copies have been done, if so it
; stops the tape.

WRITE
  LDA BITCI     ; get tape long word marker
  BNE LAB_FC21    ; if long word marker done go do rest of byte

; each byte sent starts with two half cycles of $0110 system clocks and the whole block
; ends with two more such half cycles

  LDA #$10      ; set first start cycle time constant low byte
  LDX #$01      ; set first start cycle time constant high byte
  JSR LAB_FBF5    ; write time constant and toggle tape
  BNE LAB_FC47    ; if first half cycle go restore registers and exit
          ; interrupt

  INC BITCI     ; set tape long word marker
  LDA SAL+1     ; get buffer address high byte
  BPL LAB_FC47    ; if block not complete go restore registers and exit
          ; interrupt. the end of a block is indicated by the tape
          ; buffer high byte b7 being set to 1

  JMP WRTN1     ; else do tape routine, block complete exit

; Continue tape byte write. The first start cycle, both half cycles of it, is complete
; so the routine drops straight through to here.

LAB_FC21
  LDA RINONE      ; get tape medium word marker
  BNE LAB_FC2E    ; if word marker already written go send the byte bits

; After the two half cycles of $0110 system clocks the start bit is completed with two
; half cycles of $00B0 system clocks. This is the same as the first part of a 1 bit.

  JSR LAB_FBF1    ; set time constant for bit = 1 and toggle tape
  BNE LAB_FC47    ; if first half cycle go restore registers and exit
          ; interrupt

  INC RINONE      ; set tape medium word marker
  BNE LAB_FC47    ; restore registers and exit interrupt, branch always

; Continue tape byte write. The start bit, both cycles of it, is complete so the routine
; drops straight through to here. Now the cycle pairs for each bit, and the parity bit,
; are sent.

LAB_FC2E
  JSR TPTOGLE     ; send LSB from tape write byte to tape
  BNE LAB_FC47    ; if first half cycle go restore registers and exit
          ; interrupt

          ; else two half cycles have been done
  LDA FIRT      ; get tape bit cycle phase
  EOR #$01      ; toggle b0
  STA FIRT      ; save tape bit cycle phase
  BEQ LAB_FC4A    ; if bit cycle phase complete go setup for next bit

; Each bit is written as two full cycles. A 1 is sent as a full cycle of $0160 system
; clocks then a full cycle of $00C0 system clocks. A 0 is sent as a full cycle of $00C0
; system clocks then a full cycle of $0160 system clocks. To do this each bit from the
; write byte is inverted during the second bit cycle phase. As the bit is inverted it
; is also added to the, one bit, parity count for this byte.

  LDA ROPRTY      ; get tape write byte
  EOR #$01      ; invert bit being sent
  STA ROPRTY      ; save tape write byte
  AND #$01      ; mask b0
  EOR PRTY      ; XOR with tape write byte parity bit
  STA PRTY      ; save tape write byte parity bit
LAB_FC47
  JMP _RTI      ; restore registers and exit interrupt

; the bit cycle phase is complete so shift out the just written bit and test for byte
; end

LAB_FC4A
  LSR ROPRTY      ; shift bit out of tape write byte
  DEC PCNTR     ; decrement tape bit count
  LDA PCNTR     ; get tape bit count
  BEQ LAB_FC8C    ; if all the data bits have been written go setup for
          ; sending the parity bit next and exit the interrupt

  BPL LAB_FC47    ; if all the data bits are not yet sent just restore the
          ; registers and exit the interrupt

; do next tape byte

; The byte is complete. The start bit, data bits and parity bit have been written to
; the tape so setup for the next byte.

LAB_FC54
  JSR NEWCH     ; new tape byte setup
  CLI       ; enable interrupts
  LDA CNTDN     ; get tape synchronisation character count
  BEQ LAB_FC6E    ; if synchronisation characters done go do block data

; At the start of each block sent to tape there are a number of synchronisation bytes
; that count down to the actual data. The Commodore tape system saves two copies of all
; the tape data, the first is loaded and is indicated by the synchronisation bytes
; having b7 set, and the second copy is indicated by the synchronisation bytes having b7
; clear. the sequence goes $09, $08, ... $02, $01, data bytes.

  LDX #$00      ; clear .X
  STX ASCII     ; clear checksum byte
  DEC CNTDN     ; decrement tape synchronisation byte count
  LDX FSBLK     ; get tape copies remaining
  CPX #$02      ; compare with load block indicator
  BNE LAB_FC6A    ; branch if not the load block

  ORA #$80      ; this is the load block so make the synchronisation count
          ; go $89, $88, ... $82, $81
LAB_FC6A
  STA ROPRTY      ; save the synchronisation byte as the tape write byte
  BNE LAB_FC47    ; restore registers and exit interrupt, branch always

; the synchronisation bytes have been done so now check and do the actual block data

LAB_FC6E
  JSR VPRTY     ; check read/write pointer, return Cb = 1 if pointer >= end
  BCC LAB_FC7D    ; if not all done yet go get the byte to send

  BNE BLKEND      ; if pointer > end go flag block done and exit interrupt

          ; else the block is complete, it only remains to write the
          ; checksum byte to the tape so setup for that
  INC SAL+1     ; increment buffer pointer high byte, this means the block
          ; done branch will always be taken next time without having
          ; to worry about the low byte wrapping to zero
  LDA ASCII     ; get checksum byte
  STA ROPRTY      ; save checksum as tape write byte
  BCS LAB_FC47    ; restore registers and exit interrupt, branch always

; the block isn't finished so get the next byte to write to tape

LAB_FC7D
  LDY #$00      ; clear index
  LDA (SAL),Y     ; get byte from buffer
  STA ROPRTY      ; save as tape write byte
  EOR ASCII     ; XOR with checksum byte
  STA ASCII     ; save new checksum byte
  JSR WRT62     ; increment read/write pointer
  BNE LAB_FC47    ; restore registers and exit interrupt, branch always

; set parity as next bit and exit interrupt

LAB_FC8C
  LDA PRTY      ; get tape write byte parity bit
  EOR #$01      ; toggle it
  STA ROPRTY      ; save as tape write byte
LAB_FC92
  JMP _RTI      ; restore registers and exit interrupt

; tape routine, block complete exit

WRTN1
  DEC FSBLK     ; decrement tape copies remaining to read/write
  BNE LAB_FC9C    ; branch if more to do

  JSR TNOFF     ; else stop cassette motor
LAB_FC9C
  LDA #$50      ; set tape write leader count
  STA INBIT     ; save tape write leader count
  LDX #$08      ; set index for write tape leader vector
  SEI       ; disable interrupts
  JSR LAB_FCFB    ; set tape vector
  BNE LAB_FC92    ; restore registers and exit interrupt, branch always


;***********************************************************************************;
;
; write tape leader IRQ routine

WRTZ
  LDA #$78      ; set time constant low byte for bit = leader
  JSR LAB_FBF3    ; write time constant and toggle tape
  BNE LAB_FC92    ; if tape bit high restore registers and exit interrupt

  DEC INBIT     ; decrement tape write leader count
  BNE LAB_FC92    ; if not all done restore registers and exit interrupt

  JSR NEWCH     ; new tape byte setup
  DEC RIPRTY      ; decrement tape leader count
  BPL LAB_FC92    ; if not all done restore registers and exit interrupt

  LDX #$0A      ; set index for tape write vector
  JSR LAB_FCFB    ; set tape vector
  CLI       ; enable interrupts
  INC RIPRTY      ; clear clear leader counter, was $FF
  LDA FSBLK     ; get tape copies remaining
  BEQ BSIV      ; if all done restore everything for STOP and exit interrupt

  JSR RD300     ; copy I/O start address to buffer address
  LDX #$09      ; set nine synchronisation bytes
  STX CNTDN     ; save tape synchronisation byte count
  BNE LAB_FC54    ; go do next tape byte, branch always


;***********************************************************************************;
;
; restore everything for STOP

TNIF
  PHP       ; save status
  SEI       ; disable interrupts
  JSR TNOFF     ; stop cassette motor
  LDA #$7F      ; disable all interrupts
  STA VIA2IER     ; set VIA 2 IER
  LDA #$F7      ; set keyboard column 3 active
  STA VIA2PB      ; set VIA 2 DRB, keyboard column
  LDA #$40      ; set T1 free run, T2 clock Ã¸2,
          ; SR disabled, latches disabled
  STA VIA2ACR     ; set VIA 2 ACR
  JSR LAB_FE39    ; set 60Hz and enable timer
  LDA IRQTMP+1    ; get saved IRQ vector high byte
  BEQ LAB_FCF4    ; branch if null

  STA CINV+1      ; restore IRQ vector high byte
  LDA IRQTMP      ; get saved IRQ vector low byte
  STA CINV      ; restore IRQ vector low byte
LAB_FCF4
  PLP       ; restore status
  RTS


;***********************************************************************************;
;
; reset vector

BSIV
  JSR TNIF      ; restore everything for STOP
  BEQ LAB_FC92    ; restore registers and exit interrupt, branch always


;***********************************************************************************;
;
; set tape vector

LAB_FCFB
  LDA IRQVCTRS-8,X    ; get tape IRQ vector low byte
  STA CINV      ; set IRQ vector low byte
  LDA IRQVCTRS-7,X    ; get tape IRQ vector high byte
  STA CINV+1      ; set IRQ vector high byte
  RTS


;***********************************************************************************;
;
; stop cassette motor

TNOFF
  LDA VIA1PCR     ; get VIA 1 PCR
  ORA #$0E      ; set CA2 high, cassette motor off
  STA VIA1PCR     ; set VIA 1 PCR
  RTS


;***********************************************************************************;
;
; check read/write pointer
; return Cb = 1 if pointer >= end

VPRTY
  SEC       ; set carry for subtract
  LDA SAL     ; get buffer address low byte
  SBC EAL     ; subtract buffer end low byte
  LDA SAL+1     ; get buffer address high byte
  SBC EAL+1     ; subtract buffer end high byte
  RTS


;***********************************************************************************;
;
; increment read/write pointer

WRT62
  INC SAL     ; increment buffer address low byte
  BNE LAB_FD21    ; if no overflow skip the high byte increment

  INC SAL+1     ; increment buffer address high byte
LAB_FD21
  RTS


;***********************************************************************************;
;
; RESET, hardware reset starts here

START
  LDX #$FF      ; set .X for stack
  SEI       ; disable interrupts
  TXS       ; clear stack
  CLD       ; clear decimal mode
  JSR CHKAUTO     ; scan for autostart ROM at $A000
  BNE LAB_FD2F    ; if not there continue VIC startup

  JMP (XROMCOLD)    ; call ROM start code

LAB_FD2F
  JSR INITMEM     ; initialise and test RAM
  JSR FRESTOR     ; restore default I/O vectors
  JSR INITVIA     ; initialise I/O registers
  JSR INITSK      ; initialise hardware
  CLI       ; enable interrupts
  JMP (COLDST)    ; execute BASIC


;***********************************************************************************;
;
; scan for autostart ROM at $A000, returns Zb=1 if ROM found

CHKAUTO
  LDX #$05      ; five characters to test
LAB_FD41
  LDA A0CBM-1,X   ; get test character
  CMP XROMID-1,X    ; compare with byte in ROM space
  BNE LAB_FD4C    ; exit if no match

  DEX       ; decrement index
  BNE LAB_FD41    ; loop if not all done

LAB_FD4C
  RTS


;***********************************************************************************;
;
; autostart ROM signature

A0CBM
  .byte "A0",$C3,$C2,$CD  ; A0CBM


;***********************************************************************************;
;
; restore default I/O vectors

; This routine restores the default values of all system vectors used in KERNAL and
; BASIC routines and interrupts. The KERNAL VECTOR routine is used to read and alter
; individual system vectors.

FRESTOR
  LDX #<VECTORS   ; pointer to vector table low byte
  LDY #>VECTORS   ; pointer to vector table high byte
  CLC       ; flag set vectors


;***********************************************************************************;
;
; set/read vectored I/O from (.X.Y), Cb = 1 to read, Cb = 0 to set

; This routine manages all system vector jump addresses stored in RAM. Calling this
; routine with the accumulator carry bit set will store the current contents of the
; RAM vectors in a list pointed to by the .X and .Y registers.

; When this routine is called with the carry bit clear, the user list pointed to by
; the .X and .Y registers is transferred to the system RAM vectors.

; NOTE: This routine requires caution in its use. The best way to use it is to first
; read the entire vector contents into the user area, alter the desired vectors, and
; then copy the contents back to the system vectors.

FVECTOR
  STX MEMUSS      ; save pointer low byte
  STY MEMUSS+1    ; save pointer high byte
  LDY #$1F      ; set byte count
LAB_FD5D
  LDA CINV,Y      ; read vector byte from vectors
  BCS LAB_FD64    ; if read vectors skip the read from .X.Y

  LDA (MEMUSS),Y    ; read vector byte from (.X.Y)
LAB_FD64
  STA (MEMUSS),Y    ; save byte to (.X.Y)
  STA CINV,Y      ; save byte to vector
  DEY       ; decrement index
  BPL LAB_FD5D    ; loop if more to do

  RTS

; The above code works but it tries to write to the ROM. While this is usually harmless
; systems that use flash ROM may suffer. Here is a version that makes the extra write
; to RAM instead but is otherwise identical in function.
;
; set/read vectored I/O from (.X.Y), Cb = 1 to read, Cb = 0 to set
;
;FVECTOR
; STX MEMUSS      ; save pointer low byte
; STY MEMUSS+1    ; save pointer high byte
; LDY #$1F      ; set byte count
;LAB_FD5D
; LDA (MEMUSS),Y    ; read vector byte from (.X.Y)
; BCC LAB_FD66    ; if set vectors skip the read from .X.Y
;
; LDA CINV,Y      ; else read vector byte from vectors
; STA (MEMUSS),Y    ; save byte to (.X.Y)
;LAB_FD66
; STA CINV,Y      ; save byte to vector
; DEY       ; decrement index
; BPL LAB_FD5D    ; loop if more to do
;
; RTS


;***********************************************************************************;
;
; KERNAL vectors

VECTORS
  .word IRQ     ; CINV    IRQ vector
  .word BREAK     ; CBINV   BRK vector
  .word NMI2      ; NMINV   NMI vector
  .word FOPEN     ; IOPEN   open a logical file
  .word FCLOSE      ; ICLOSE  close a specified logical file
  .word FCHKIN      ; ICHKIN  open channel for input
  .word FCHKOUT     ; ICKOUT  open channel for output
  .word FCLRCHN     ; ICLRCN  close input and output channels
  .word FCHRIN      ; IBASIN  input character from channel
  .word FCHROUT     ; IBSOUT  output character to channel
  .word FSTOP     ; ISTOP   scan stop key
  .word FGETIN      ; IGETIN  get character from keyboard queue
  .word FCLALL      ; ICLALL  close all channels and files
  .word BREAK     ; USRCMD  user function

; Vector to user defined command, currently points to BRK.

; This appears to be a holdover from PET days, when the built-in machine language monitor
; would jump through the $032E vector when it encountered a command that it did not
; understand, allowing the user to add new commands to the monitor.

; Although this vector is initialised to point to the routine called by STOP/RESTORE and
; the BRK interrupt, and is updated by the KERNAL vector routine at $FD57, it no longer
; has any function.

  .word FLOAD2      ; ILOAD   load
  .word FSAVE2      ; ISAVE   save


;***********************************************************************************;
;
; Initialise and test RAM, the RAM from $000 to $03FF is never tested and is just assumed
; to work. First a search is done from $0401 for the start of memory and this is saved, if
; this start is at or beyond $1100 then the routine dead ends. Once the start of memory is
; found the routine looks for the end of memory, if this end is before $2000 the routine
; again dead ends. Lastly, if the end of memory is at $2000 then the screen is set to
; $1E00, but if the memory extends to or beyond $2100 then the screen is moved to $1000.

INITMEM
  LDA #$00      ; clear .A
  TAX       ; clear index
LAB_FD90
  STA USRPPOK,X   ; clear page 0
  STA BUF,X     ; clear page 2
  STA IERROR,X    ; clear page 3
  INX       ; increment index
  BNE LAB_FD90    ; loop if more to do

  LDX #<TBUFFR    ; set cassette buffer pointer low byte
  LDY #>TBUFFR    ; set cassette buffer pointer high byte
  STX TAPE1     ; save tape buffer start pointer low byte
  STY TAPE1+1     ; save tape buffer start pointer high byte

  STA STAL      ; clear RAM test pointer low byte
  STA XSAV      ; clear looking for end flag
  STA MEMSTR      ; clear OS start of memory low byte

  TAY       ; clear .Y
  LDA #$04      ; set RAM test pointer high byte
  STA STAL+1      ; save RAM test pointer high byte
LAB_FDAF
  INC STAL      ; increment RAM test pointer low byte
  BNE LAB_FDB5    ; if no rollover skip the high byte increment

  INC STAL+1      ; increment RAM test pointer high byte
LAB_FDB5
  JSR TSTMEM      ; test RAM byte, return Cb=0 if failed
  LDA XSAV      ; test looking for end flag
  BEQ LAB_FDDE    ; branch if not looking for end

          ; else now looking for the end of memory
  BCS LAB_FDAF    ; loop if byte test passed

  LDY STAL+1      ; get test address high byte
  LDX STAL      ; get test address low byte
  CPY #$20      ; compare with $2000, RAM should always end at or after
          ; $2000 even with no expansion memory as the built in RAM
          ; ends at $1FFF. therefore the following test should
          ; never branch
  BCC LAB_FDEB    ; if end address < $2000 go do dead end loop

  CPY #$21      ; compare with $2100
  BCS LAB_FDD2    ; branch if >= $2100

          ; else memory ended before $2100
  LDY #$1E      ; set screen memory page to $1E00
  STY HIBASE      ; save screen memory page
LAB_FDCF
  JMP LAB_FE7B    ; set the top of memory and return

          ; memory ends beyond $2100
LAB_FDD2
  LDA #$12      ; set OS start of memory high byte
  STA MEMSTR+1    ; save OS start of memory high byte
  LDA #$10      ; set screen memory page to $1000
  STA HIBASE      ; save screen memory page
  BNE LAB_FDCF    ; set the top of memory and return, branch always

LAB_FDDE
  BCC LAB_FDAF    ; loop if byte test failed, not found start yet

          ; else found start of RAM
  LDA STAL+1      ; get test address high byte
  STA MEMSTR+1    ; save OS start of memory high byte
  STA XSAV      ; set looking for end flag
  CMP #$11      ; compare start with $1100, RAM should always start before
          ; $1100 even with no expansion memory as the built in RAM
          ; starts at $1000. therefore the following test should
          ; always branch
  BCC LAB_FDAF    ; go find end of RAM, branch always

          ; if the code drops through here then the RAM has failed
          ; and there is not much else to be done
LAB_FDEB
  JSR INITVIC     ; initialise VIC chip
  JMP LAB_FDEB    ; loop forever


;***********************************************************************************;
;
; tape IRQ vectors

IRQVCTRS
  .word WRTZ      ; $08 write tape leader IRQ routine
  .word WRITE     ; $0A tape write IRQ routine
  .word IRQ     ; $0C normal IRQ vector
  .word READT     ; $0E read tape bits IRQ routine


;***********************************************************************************;
;
; initialise I/O registers

INITVIA
  LDA #$7F      ; disable all interrupts
  STA VIA1IER     ; on VIA 1 IER ..
  STA VIA2IER     ; .. and VIA 2 IER

  LDA #$40      ; set T1 free run, T2 clock Ã¸2,
          ; SR disabled, latches disabled
  STA VIA2ACR     ; set VIA 2 ACR

  LDA #$40      ; set T1 free run, T2 clock Ã¸2,
          ; SR disabled, latches disabled
  STA VIA1ACR     ; set VIA 1 ACR

  LDA #$FE      ; CB2 high, RS-232 Tx
          ; CB1 +ve edge,
          ; CA2 high, tape motor off
          ; CA1 -ve edge
  STA VIA1PCR     ; set VIA 1 PCR

  LDA #$DE      ; CB2 low, serial data out high
          ; CB1 +ve edge,
          ; CA2 high, serial clock out low
          ; CA1 -ve edge
  STA VIA2PCR     ; set VIA 2 PCR

  LDX #$00      ; all inputs, RS-232 interface or parallel user port
  STX VIA1DDRB    ; set VIA 1 DDRB

  LDX #$FF      ; all outputs, keyboard column
  STX VIA2DDRB    ; set VIA 2 DDRB

  LDX #$00      ; all inputs, keyboard row
  STX VIA2DDRA    ; set VIA 2 DDRA

  LDX #$80      ; OIII IIII, ATN out, light pen, joystick, serial data
          ; in, serial clk in
  STX VIA1DDRA    ; set VIA 1 DDRA

  LDX #$00      ; ATN out low, set ATN high
  STX VIA1PA2     ; set VIA 1 DRA, no handshake

  JSR SRCLKHI     ; set serial clock high
  LDA #$82      ; enable CA1 interrupt, [RESTORE] key
  STA VIA1IER     ; set VIA 1 IER
  JSR SRCLKLO     ; set serial clock low


;***********************************************************************************;
;
; set 60Hz and enable timer

LAB_FE39
  LDA #$C0      ; enable T1 interrupt
  STA VIA2IER     ; set VIA 2 IER
  LDA #$26      ; set timer constant low byte [PAL]
; LDA #$89      ; set timer constant low byte [NTSC]
  STA VIA2T1CL    ; set VIA 2 T1C_l
  LDA #$48      ; set timer constant high byte [PAL]
; LDA #$42      ; set timer constant high byte [NTSC]
  STA VIA2T1CH    ; set VIA 2 T1C_h
  RTS


;***********************************************************************************;
;
; set filename

; This routine is used to set up the file name for the OPEN, SAVE, or LOAD routines.
; The accumulator must be loaded with the length of the file and .X.Y with the pointer
; to file name, .X being the low byte. The address can be any valid memory address in
; the system where a string of characters for the file name is stored. If no file
; name desired the accumulator must be set to 0, representing a zero file length,
; in that case .X.Y may be set to any memory address.

FSETNAM
  STA FNLEN     ; set file name length
  STX FNADR     ; set file name pointer low byte
  STY FNADR+1     ; set file name pointer high byte
  RTS


;***********************************************************************************;
;
; set logical file, first and second addresses

; This routine will set the logical file number, device address, and secondary
; address, command number, for other KERNAL routines.

; The logical file number is used by the system as a key to the file table created
; by the OPEN file routine. Device addresses can range from 0 to 30. The following
; codes are used by the computer to stand for the following devices:

; ADDRESS DEVICE
; ======= ======
;  0    Keyboard
;  1    Cassette
;  2    RS-232
;  3    CRT display
;  4    Serial bus printer
;  8    Serial bus disk drive

; device numbers of four or greater automatically refer to devices on the serial
; bus.

; A command to the device is sent as a secondary address on the serial bus after
; the device number is sent during the serial attention handshaking sequence. If
; no secondary address is to be sent .Y should be set to $FF.

FSETLFS
  STA LA      ; set logical file
  STX FA      ; set device number
  STY SA      ; set secondary address or command
  RTS


;***********************************************************************************;
;
; read I/O status word

; This routine returns the current status of the I/O device in the accumulator. The
; routine is usually called after new communication to an I/O device. The routine
; will give information about device status, or errors that have occurred during the
; I/O operation.

FREADST
  LDA FA      ; get device number
  CMP #$02      ; compare device with RS-232 device
  BNE READIOST    ; branch if not RS-232 device

          ; get RS-232 device status
  LDA RSSTAT      ; read RS-232 status word

  LDA #$00      ; clear .A
  STA RSSTAT      ; clear RS-232 status

; The above code is wrong. The RS-232 status is in .A but .A is cleared and that is used
; to clear the RS-232 status byte. So whatever the status the result is always $00 and
; the status byte is always cleared. The C64 code saves the status byte to the stack
; before clearing it ..
;
; PHA       ; save RS-232 status
; LDA #$00      ; clear .A
; STA RSSTAT      ; clear RS-232 status
; PLA       ; restore RS-232 status
  RTS


;***********************************************************************************;
;
; control KERNAL messages

; This routine controls the printing of error and control messages by the KERNAL.
; Either print error messages or print control messages can be selected by setting
; the accumulator when the routine is called.

; FILE NOT FOUND is an example of an error message. PRESS PLAY ON CASSETTE is an
; example of a control message.

; Bits 6 and 7 of this value determine where the message will come from. If bit 7
; is set one of the error messages from the KERNAL will be printed. If bit 6 is set
; a control message will be printed.

FSETMSG
  STA MSGFLG      ; set KERNAL message mode flag
READIOST
  LDA STATUS      ; read I/O status byte

; OR into I/O status byte

ORIOST
  ORA STATUS      ; OR with I/O status byte
  STA STATUS      ; save I/O status byte
  RTS


;***********************************************************************************;
;
; set timeout on IEEE-488 bus

; This routine sets the timeout flag for the serial bus. When the timeout flag is
; set, the computer will wait for a device on the serial bus for 64 milliseconds.
; If the device does not respond to the computer's DAV signal within that time the
; computer will recognize an error condition and leave the handshake sequence. When
; this routine is called and the accumulator contains a 0 in bit 7, timeouts are
; enabled. A 1 in bit 7 will disable the timeouts.

; NOTE: The timeout feature is used to communicate that a disk file is not found on
; an attempt to OPEN a file.

FSETTMO
  STA TIMOUT      ; save serial bus timeout flag
  RTS


;***********************************************************************************;
;
; read/set the top of memory, Cb = 1 to read, Cb = 0 to set

; This routine is used to read and set the top of RAM. When this routine is called
; with the carry bit set the pointer to the top of RAM will be loaded into .X.Y. When
; this routine is called with the carry bit clear .X.Y will be saved as the top of
; memory pointer changing the top of memory.

FMEMTOP
  BCC LAB_FE7B    ; if Cb clear go set the top of memory

; read the top of memory

LAB_FE75
  LDX MEMHIGH     ; get memory top low byte
  LDY MEMHIGH+1   ; get memory top high byte

; set the top of memory

LAB_FE7B
  STX MEMHIGH     ; set memory top low byte
  STY MEMHIGH+1   ; set memory top high byte
  RTS


;***********************************************************************************;
;
; read/set the bottom of memory, Cb = 1 to read, Cb = 0 to set

; This routine is used to read and set the bottom of RAM. When this routine is
; called with the carry bit set the pointer to the bottom of RAM will be loaded
; into .X.Y. When this routine is called with the carry bit clear .X.Y will be saved as
; the bottom of memory pointer changing the bottom of memory.

FMEMBOT
  BCC LAB_FE8A    ; if Cb clear go set the bottom of memory

; read the bottom of memory

  LDX MEMSTR      ; read OS start of memory low byte
  LDY MEMSTR+1    ; read OS start of memory high byte

; set the bottom of memory

LAB_FE8A
  STX MEMSTR      ; set OS start of memory low byte
  STY MEMSTR+1    ; set OS start of memory high byte
  RTS


;***********************************************************************************;
;
; non-destructive test RAM byte, return Cb=0 if failed

TSTMEM
  LDA (STAL),Y    ; get existing RAM byte
  TAX       ; copy to .X
  LDA #$55      ; set first test byte
  STA (STAL),Y    ; save to RAM
  CMP (STAL),Y    ; compare with saved
  BNE LAB_FEA4    ; branch if fail

  ROR       ; make byte $AA, carry is set here
  STA (STAL),Y    ; save to RAM
  CMP (STAL),Y    ; compare with saved
  BNE LAB_FEA4    ; branch if fail
  .byte $A9     ; makes next line LDA #$18

LAB_FEA4
  CLC       ; flag test failed
  TXA       ; get original byte back
  STA (STAL),Y    ; restore original byte
  RTS


;***********************************************************************************;
;
; NMI vector

NMI
  SEI       ; disable interrupts
  JMP (NMINV)     ; do NMI vector


;***********************************************************************************;
;
; NMI handler

NMI2
  PHA       ; save .A
  TXA       ; copy .X
  PHA       ; save .X
  TYA       ; copy .Y
  PHA       ; save .Y
  LDA VIA1IFR     ; get VIA 1 IFR
  BPL LAB_FEFF    ; if no interrupt restore registers and exit

  AND VIA1IER     ; AND with VIA 1 IER
  TAX       ; copy to .X
  AND #$02      ; mask CA1 interrupt, [RESTORE] key
  BEQ RSNMI     ; branch if not [RESTORE] key

  ; This code does not properly handle other bits of the IFR being set.
  ; When neither an autostart ROM nor the [STOP] key are down the CA1 interrupt
  ; bit will clear but any others will remain set. Because the NMI interrupt is
  ; edge-triggered no further interrupts will be triggered and the other VIA
  ; events will never be processed.

          ; else was [RESTORE] key ..
  JSR CHKAUTO     ; scan for autostart ROM at $A000
  BNE LAB_FEC7    ; branch if no autostart ROM

  JMP (XROMWARM)    ; else do autostart ROM break entry

LAB_FEC7
  BIT VIA1PA1     ; test VIA 1 DRA, clear CA1 interrupt
  JSR FUDTIM      ; increment the real time clock
  JSR STOP      ; scan stop key
  BNE LAB_FEFF    ; if not [STOP] restore registers and exit interrupt


;***********************************************************************************;
;
; BRK handler

BREAK
  JSR FRESTOR     ; restore default I/O vectors
  JSR INITVIA     ; initialise I/O registers
  JSR INITSK      ; initialise hardware
  JMP (WARMST)    ; do BASIC break entry


;***********************************************************************************;
;
; RS-232 NMI routine
;
; This code only processes the bits set in the copy of the ISR taken by the caller. If more
; bits become set while the ISR is executing they will be delayed until another interrupt can
; be delivered to the CPU. This would add at least 60 cycles of latency.
;
; It also only processes a single interrupt source in the following order of priority:
;   1. Tx timer
;   2. Rx timer
;   3. Rx data start
; If multiple bits are set in the ISR multiple interrupts must be delivered to the CPU to process
; them.

RSNMI
  LDA VIA1IER     ; get VIA 1 IER
  ORA #$80      ; set enable bit, this bit should be set according to the
          ; Rockwell 6522 datasheet but clear according to the MOS
          ; datasheet. best to assume it's not in the state required
          ; and set it so
  PHA       ; save to re-enable interrupts
  LDA #$7F      ; disable all interrupts
  STA VIA1IER     ; set VIA 1 IER
  TXA       ; get active interrupts back
  AND #$40      ; mask T1 interrupt
  BEQ LAB_FF02    ; branch if not T1 interrupt

          ; was VIA 1 T1 interrupt, Tx timer expired
  LDA #$CE      ; CB2 low, CB1 -ve edge, CA2 high, CA1 -ve edge
  ORA NXTBIT      ; OR RS-232 next bit to send, sets CB2 high if set
  STA VIA1PCR     ; set VIA 1 PCR
  LDA VIA1T1CL    ; get VIA 1 T1C_l, clear T1 interrupt
  PLA       ; restore interrupt enable byte to restore previously
          ; enabled interrupts
  STA VIA1IER     ; set VIA 1 IER
  JSR RSNXTBIT    ; RS-232 Tx NMI routine
LAB_FEFF
  JMP _RTI      ; restore registers and exit interrupt

          ; was not VIA 1 T1 interrupt
LAB_FF02
  TXA       ; get active interrupts back
  AND #$20      ; mask T2 interrupt
  BEQ LAB_FF2C    ; branch if not T2 interrupt

          ; was VIA 1 T2 interrupt, Rx timer expired

  ; The timer has wrapped and is counting down from $FFFF, no further interrupts
  ; will be generated until the latch is written to. Adding the baud rate bit time
  ; to the current value will result in another interrupt at the right interval.

  LDA VIA1PB      ; get VIA 1 DRB
  AND #$01      ; mask RS-232 data in
  STA INBIT     ; save RS-232 input bit
  LDA VIA1T2CL    ; get VIA 1 T2C_l, clear T2 interrupt
  SBC #$16      ; adjust by 22 cycles to cover time taken by the
          ; six instructions needed to write to the latch
  ADC BAUDOF      ; add baud rate bit time low byte
  STA VIA1T2CL    ; set VIA 1 T2C_l
  LDA VIA1T2CH    ; get VIA 1 T2C_h
  ADC BAUDOF+1    ; add baud rate bit time high byte
  STA VIA1T2CH    ; set VIA 1 T2C_h
  PLA       ; restore interrupt enable byte to restore previously
          ; enabled interrupts
  STA VIA1IER     ; set VIA 1 IER, restore interrupts
  JSR RSINBIT     ; RS-232 Rx
  JMP _RTI      ; restore registers and exit interrupt

          ; was not VIA 1 T2 interrupt
LAB_FF2C
  TXA       ; get active interrupts back
  AND #$10      ; mask CB1 interrupt
  BEQ _RTI      ; if no bit restore registers and exit interrupt

          ; was VIA 1 CB1 interrupt, Rx data bit transition
  LDA M51CTR      ; get pseudo 6551 control register
  AND #$0F      ; mask 0000 xxxx, baud rate
  BNE LAB_FF38    ; short delay. was this intended to skip code used to
          ; implement the user baud rate ??
LAB_FF38
  ASL       ; *2, 2 bytes per baud count
  TAX       ; copy to index
  LDA BAUDTBL-2,X   ; get baud count low byte
  STA VIA1T2CL    ; set VIA 1 T2C_l
  LDA BAUDTBL-1,X   ; get baud count high byte
  STA VIA1T2CH    ; set VIA 1 T2C_h
  LDA VIA1PB      ; read VIA 1 DRB, clear interrupt flag
  PLA       ; restore interrupt enable byte to restore previously
          ; enabled interrupts
  ORA #$20      ; enable T2 interrupt
  AND #$EF      ; disable CB1 interrupt, Rx data bit transition
  STA VIA1IER     ; set VIA 1 IER
  LDX BITNUM      ; get number of bits to be sent/received
  STX BITCI     ; save RS-232 input bit count


;***********************************************************************************;
;
; restore the registers and exit the interrupt
;
; If you write your own interrupt code you should either return from the interrupt
; using code that ends up here or code that replicates this code.

_RTI
  PLA       ; pull .Y
  TAY       ; restore .Y
  PLA       ; pull .X
  TAX       ; restore .X
  PLA       ; restore .A
  RTI


;***********************************************************************************;
;
; baud rate word is calculated from ..
;
; (system clock / baud rate) / 2 - 100
;
;   system clock
;   ------------
; PAL   1108404 Hz
; NTSC    1022727 Hz

; baud rate tables for PAL VIC 20

BAUDTBL
  .word $2AE6     ;   50   baud
  .word $1C78     ;   75   baud
  .word $1349     ;  110   baud
  .word $0FB1     ;  134.5 baud
  .word $0E0A     ;  150   baud
  .word $06D3     ;  300   baud
  .word $0338     ;  600   baud
  .word $016A     ; 1200   baud
  .word $00D0     ; 1800   baud
  .word $0083     ; 2400   baud
  .word $0036     ; 3600   baud

; baud rate tables for NTSC VIC 20

; .word $2792     ;   50   baud
; .word $1A40     ;   75   baud
; .word $11C6     ;  110   baud
; .word $0E74     ;  134.5 baud
; .word $0CEE     ;  150   baud
; .word $0645     ;  300   baud
; .word $02F1     ;  600   baud
; .word $0146     ; 1200   baud
; .word $00B8     ; 1800   baud
; .word $0071     ; 2400   baud
; .word $002A     ; 3600   baud


;***********************************************************************************;
;
; IRQ vector

IRQROUT
  PHA       ; save .A
  TXA       ; copy .X
  PHA       ; save .X
  TYA       ; copy .Y
  PHA       ; save .Y
  TSX       ; copy stack pointer
  LDA STACK+4,X   ; get the stacked status register
  AND #$10      ; mask the BRK flag bit
  BEQ LAB_FF82    ; if not BRK go do the hardware IRQ vector

  JMP (CBINV)     ; else do the BRK vector (iBRK)

LAB_FF82
  JMP (CINV)      ; do IRQ vector (iIRQ)


;***********************************************************************************;
;
; spare bytes, not referenced

  .byte $FF,$FF,$FF,$FF,$FF


;***********************************************************************************;
;
; restore default I/O vectors

; This routine restores the default values of all system vectors used in KERNAL and
; BASIC routines and interrupts. The KERNAL VECTOR routine is used to read and alter
; individual system vectors.


RESTOR
  JMP FRESTOR     ; restore default I/O vectors


;***********************************************************************************;
;
; read/set vectored I/O

; This routine manages all system vector jump addresses stored in RAM. Calling this
; routine with the accumulator carry bit set will store the current contents of the
; RAM vectors in a list pointed to by the .X and .Y registers.

; When this routine is called with the carry bit clear, the user list pointed to by
; the .X and .Y registers is transferred to the system RAM vectors.

; NOTE: This routine requires caution in its use. The best way to use it is to first
; read the entire vector contents into the user area, alter the desired vectors, and
; then copy the contents back to the system vectors.

VECTOR
  JMP FVECTOR     ; set/read vectored I/O from (.X.Y)


;***********************************************************************************;
;
; control KERNAL messages

; This routine controls the printing of error and control messages by the KERNAL.
; Either print error messages or print control messages can be selected by setting
; the accumulator when the routine is called.

; FILE NOT FOUND is an example of an error message. PRESS PLAY ON CASSETTE is an
; example of a control message.

; Bits 6 and 7 of this value determine where the message will come from. If bit 7
; is set one of the error messages from the KERNAL will be printed. If bit 6 is set
; a control message will be printed.

SETMSG
  JMP FSETMSG     ; control KERNAL messages


;***********************************************************************************;
;
; send secondary address after LISTEN

; This routine is used to send a secondary address to an I/O device after a call to
; the LISTEN routine is made and the device commanded to LISTEN. The routine cannot
; be used to send a secondary address after a call to the TALK routine.

; A secondary address is usually used to give set-up information to a device before
; I/O operations begin.

; When a secondary address is to be sent to a device on the serial bus the address
; must first be ORed with $60.

SECOND
  JMP FSECOND     ; send secondary address after LISTEN


;***********************************************************************************;
;
; send secondary address after TALK

; This routine transmits a secondary address on the serial bus for a TALK device.
; This routine must be called with a number between 4 and 30 in the accumulator.
; The routine will send this number as a secondary address command over the serial
; bus. This routine can only be called after a call to the TALK routine. It will
; not work after a LISTEN.

TKSA
  JMP FTKSA     ; send secondary address after TALK


;***********************************************************************************;
;
; read/set the top of memory

; This routine is used to read and set the top of RAM. When this routine is called
; with the carry bit set the pointer to the top of RAM will be loaded into .X.Y. When
; this routine is called with the carry bit clear .X.Y will be saved as the top of
; memory pointer changing the top of memory.

MEMTOP
  JMP FMEMTOP     ; read/set the top of memory


;***********************************************************************************;
;
; read/set the bottom of memory

; This routine is used to read and set the bottom of RAM. When this routine is
; called with the carry bit set the pointer to the bottom of RAM will be loaded
; into .X.Y. When this routine is called with the carry bit clear .X.Y will be saved
; as the bottom of memory pointer changing the bottom of memory.

MEMBOT
  JMP FMEMBOT     ; read/set the bottom of memory


;***********************************************************************************;
;
; scan the keyboard

; This routine will scan the keyboard and check for pressed keys. It is the same
; routine called by the interrupt handler. If a key is down, its ASCII value is
; placed in the keyboard queue.

SCNKEY
  JMP FSCNKEY     ; scan keyboard


;***********************************************************************************;
;
; set timeout on IEEE-488 bus

; This routine sets the timeout flag for the serial bus. When the timeout flag is
; set, the computer will wait for a device on the serial bus for 64 milliseconds.
; If the device does not respond to the computer's DAV signal within that time the
; computer will recognize an error condition and leave the handshake sequence. When
; this routine is called and the accumulator contains a 0 in bit 7, timeouts are
; enabled. A 1 in bit 7 will disable the timeouts.

; NOTE: The timeout feature is used to communicate that a disk file is not found on
; an attempt to OPEN a file.

SETTMO
  JMP FSETTMO     ; set timeout on serial bus


;************************************************************************************
;
; input a byte from the serial bus

; This routine reads a byte of data from the serial bus using full handshaking. The
; data is returned in the accumulator. Before using this routine the TALK routine
; must have been called first to command the device on the serial bus to send data on
; the bus. If the input device needs a secondary command it must be sent by using the
; TKSA routine before calling this routine.

; Errors are returned in the status word which can be read by calling the READST
; routine.

ACPTR
  JMP FACPTR      ; input byte from serial bus


;************************************************************************************
;
; output a byte to the serial bus

; This routine is used to send information to devices on the serial bus. A call to
; this routine will put a data byte onto the serial bus using full handshaking.
; Before this routine is called the LISTEN routine must be used to command a device
; on the serial bus to get ready to receive data.

; The accumulator is loaded with a byte to output as data on the serial bus. A
; device must be listening or the status word will return a timeout. This routine
; always buffers one character. So when a call to the UNLSN routine is made to end
; the data transmission, the buffered character is sent with EOI set. The UNLISTEN
; command is sent to the device.

CIOUT
  JMP FCIOUT      ; output a byte to the serial bus


;***********************************************************************************;
;
; command the serial bus to UNTALK

; This routine will transmit an UNTALK command on the serial bus. All devices
; previously set to TALK will stop sending data when this command is received.

UNTLK
  JMP FUNTLK      ; command the serial bus to UNTALK


;***********************************************************************************;
;
; command the serial bus to UNLISTEN

; This routine commands all devices on the serial bus to stop receiving data from
; the computer. Calling this routine results in an UNLISTEN command being transmitted
; on the serial bus. Only devices previously commanded to listen will be affected.

; This routine is normally used after the computer is finished sending data to
; external devices. Sending the UNLISTEN will command the listening devices to get
; off the serial bus so it can be used for other purposes.

UNLSN
  JMP FUNLSN      ; command the serial bus to UNLISTEN


;***********************************************************************************;
;
; command devices on the serial bus to LISTEN

; This routine will command a device on the serial bus to receive data. The
; accumulator must be loaded with a device number between 4 and 30 before calling
; this routine. LISTEN convert this to a listen address then transmit this data as
; a command on the serial bus. The specified device will then go into listen mode
; and be ready to accept information.

LISTEN
  JMP FLISTEN     ; command devices on the serial bus to LISTEN


;***********************************************************************************;
;
; command a serial bus device to TALK

; To use this routine the accumulator must first be loaded with a device number
; between 4 and 30. When called this routine converts this device number to a talk
; address. Then this data is transmitted as a command on the serial bus.

TALK
  JMP FTALK     ; command serial bus device to TALK


;***********************************************************************************;
;
; read I/O status word

; This routine returns the current status of the I/O device in the accumulator. The
; routine is usually called after new communication to an I/O device. The routine
; will give information about device status, or errors that have occurred during the
; I/O operation.

READST
  JMP FREADST     ; read I/O status word


;***********************************************************************************;
;
; set logical, first and second addresses

; This routine will set the logical file number, device address, and secondary
; address, command number, for other KERNAL routines.

; The logical file number is used by the system as a key to the file table created
; by the OPEN file routine. Device addresses can range from 0 to 30. The following
; codes are used by the computer to stand for the following devices:

; ADDRESS DEVICE
; ======= ======
;  0    Keyboard
;  1    Cassette
;  2    RS-232
;  3    CRT display
;  4    Serial bus printer
;  8    Serial bus disk drive

; device numbers of four or greater automatically refer to devices on the serial
; bus.

; A command to the device is sent as a secondary address on the serial bus after
; the device number is sent during the serial attention handshaking sequence. If
; no secondary address is to be sent .Y should be set to $FF.

SETLFS
  JMP FSETLFS     ; set logical, first and second addresses


;***********************************************************************************;
;
; set the filename

; This routine is used to set up the file name for the OPEN, SAVE, or LOAD routines.
; The accumulator must be loaded with the length of the file and .X.Y with the pointer
; to file name, .X being the low byte. The address can be any valid memory address in
; the system where a string of characters for the file name is stored. If no file
; name desired the accumulator must be set to 0, representing a zero file length,
; in that case .X.Y may be set to any memory address.

SETNAM
  JMP FSETNAM     ; set filename


;***********************************************************************************;
;
; open a logical file

; This routine is used to open a logical file. Once the logical file is set up it
; can be used for input/output operations. Most of the I/O KERNAL routines call on
; this routine to create the logical files to operate on. No arguments need to be
; set up to use this routine, but both the SETLFS and SETNAM KERNAL routines must
; be called before using this routine.

OPEN
  JMP (IOPEN)     ; do open file vector


;***********************************************************************************;
;
; close a specified logical file

; This routine is used to close a logical file after all I/O operations have been
; completed on that file. This routine is called after the accumulator is loaded
; with the logical file number to be closed, the same number used when the file was
; opened using the OPEN routine.

CLOSE
  JMP (ICLOSE)    ; do close file vector


;************************************************************************************
;
; open a channel for input

; Any logical file that has already been opened by the OPEN routine can be defined as
; an input channel by this routine. the device on the channel must be an input device
; or an error will occur and the routine will abort.

; If you are getting data from anywhere other than the keyboard, this routine must be
; called before using either the CHRIN routine or the GETIN routine. If you are
; getting data from the keyboard and no other input channels are open then the calls to
; this routine and to the OPEN routine are not needed.

; When used with a device on the serial bus this routine will automatically send the
; listen address specified by the OPEN routine and any secondary address.

; Possible errors are:
;
; 3 : file not open
; 5 : device not present
; 6 : file is not an input file

CHKIN
  JMP (ICHKIN)    ; do open for input vector


;************************************************************************************
;
; open a channel for output

; Any logical file that has already been opened by the OPEN routine can be defined as
; an output channel by this routine the device on the channel must be an output device
; or an error will occur and the routine will abort.

; If you are sending data to anywhere other than the screen this routine must be
; called before using the CHROUT routine. if you are sending data to the screen and no
; other output channels are open then the calls to this routine and to the OPEN routine
; are not needed.

; When used with a device on the serial bus this routine will automatically send the
; listen address specified by the OPEN routine and any secondary address.

; Possible errors are:
;
; 3 : file not open
; 5 : device not present
; 7 : file is not an output file

CHKOUT
  JMP (ICKOUT)    ; do open for output vector


;************************************************************************************
;
; close input and output channels

; This routine is called to clear all open channels and restore the I/O channels to
; their original default values. It is usually called after opening other I/O
; channels and using them for input/output operations. The default input device is
; 0, the keyboard. The default output device is 3, the screen.

; If one of the channels to be closed is to the serial bus, an UNTALK signal is sent
; first to clear the input channel or an UNLISTEN is sent to clear the output channel.
; By not calling this routine and leaving listener(s) active on the serial bus,
; several devices can receive the same data from the VIC at the same time. One way to
; take advantage of this would be to command the printer to LISTEN and the disk to
; TALK. This would allow direct printing of a disk file.

CLRCHN
  JMP (ICLRCN)    ; do close vector


;************************************************************************************
;
; input character from channel

; This routine will get a byte of data from the channel already set up as the input
; channel by the CHKIN routine.

; If CHKIN has not been used to define another input channel the data is expected to
; be from the keyboard. the data byte is returned in the accumulator. The channel
; remains open after the call.

; Input from the keyboard is handled in a special way. first, the cursor is turned on
; and it will blink until a carriage return is typed on the keyboard. All characters
; on the logical line, up to 88 characters, will be stored in the BASIC input buffer.
; then the characters can be returned one at a time by calling this routine once for
; each character. When the carriage return is returned the entire line has been
; processed. the next time this routine is called the whole process begins again.

CHRIN
  JMP (IBASIN)    ; do input vector


;************************************************************************************
;
; output a character to channel

; This routine will output a character to an already opened channel. Use the OPEN
; routine, OPEN, and the CHKOUT routine to set up the output channel before calling
; this routine. If these calls are omitted, data will be sent to the default output
; device, device 3, the screen. The data byte to be output is loaded into the
; accumulator, and this routine is called. The data is then sent to the specified
; output device. The channel is left open after the call.

; NOTE: Care must be taken when using routine to send data to a serial device since
; data will be sent to all open output channels on the bus. Unless this is desired,
; all open output channels on the serial bus other than the actually intended
; destination channel must be closed by a call to the KERNAL close channel routine.

CHROUT
  JMP (IBSOUT)    ; do output vector


;***********************************************************************************;
;
; load RAM from a device

; This routine will load data bytes from any input device directly into the memory
; of the computer. It can also be used for a verify operation comparing data from a
; device with the data already in memory, leaving the data stored in RAM unchanged.

; The accumulator must be set to 0 for a load operation or 1 for a verify. If the
; input device was OPENed with a secondary address of 0 the header information from
; device will be ignored. In this case .X.Y must contain the starting address for the
; load. If the device was addressed with a secondary address of 1 or 2 the data will
; load into memory starting at the location specified by the header. This routine
; returns the address of the highest RAM location which was loaded.

; Before this routine can be called, the SETLFS and SETNAM routines must be called.

LOAD
  JMP FLOAD     ; load RAM from a device


;***********************************************************************************;
;
; save RAM to a device

; This routine saves a section of memory. Memory is saved from an indirect address
; on page 0 specified by .A, to the address stored in .X.Y, to a logical file. The
; SETLFS and SETNAM routines must be used before calling this routine. However, a
; file name is not required to SAVE to device 1, the cassette. Any attempt to save to
; other devices without using a file name results in an error.

; NOTE: device 0, the keyboard, and device 3, the screen, cannot be SAVEd to. If
; the attempt is made, an error will occur, and the SAVE stopped.

SAVE
  JMP FSAVE     ; save RAM to device


;***********************************************************************************;
;
; set the real time clock

; The system clock is maintained by an interrupt routine that updates the clock
; every 1/60th of a second. The clock is three bytes long which gives the capability
; to count from zero up to 5,184,000 jiffies - 24 hours plus one jiffy. At that point
; the clock resets to zero. Before calling this routine to set the clock the new time,
; in jiffies, should be in .Y.X.A, the accumulator containing the most significant byte.

SETTIM
  JMP FSETTIM     ; set real time clock


;***********************************************************************************;
;
; read the real time clock

; This routine returns the time, in jiffies, in .Y.X.A. The accumulator contains the
; most significant byte.

RDTIM
  JMP FRDTIM      ; read real time clock


;***********************************************************************************;
;
; scan the stop key

; If the STOP key on the keyboard is pressed when this routine is called the Z flag
; will be set. All other flags remain unchanged. If the STOP key is not pressed then
; the accumulator will contain a byte representing the last row of the keyboard scan.

; The user can also check for certain other keys this way.

STOP
  JMP (ISTOP)     ; do stop key vector


;***********************************************************************************;
;
; get a character from an input device

; In practice this routine operates identically to the CHRIN routine for all devices
; except for the keyboard. If the keyboard is the current input device this routine
; will get one character from the keyboard buffer. It depends on the IRQ routine to
; read the keyboard and put characters into the buffer.

; If the keyboard buffer is empty the value returned in the accumulator will be zero

GETIN
  JMP (IGETIN)    ; do get vector


;***********************************************************************************;
;
; close all channels and files

; This routine closes all open files. When this routine is called, the pointers into
; the open file table are reset, closing all files. Also the routine automatically
; resets the I/O channels.

CLALL
  JMP (ICLALL)    ; do close all vector


;***********************************************************************************;
;
; increment the real time clock

; This routine updates the system clock. Normally this routine is called by the
; normal KERNAL interrupt routine every 1/60th of a second. If the user program
; processes its own interrupts this routine must be called to update the time. Also,
; the STOP key routine must be called if the stop key is to remain functional.

UDTIM
  JMP FUDTIM      ; increment real time clock


;***********************************************************************************;
;
; return X,Y organisation of screen

; this routine returns the x,y organisation of the screen in .X,.Y

SCREEN
  JMP FSCREEN     ; return X,Y organisation of screen


;***********************************************************************************;
;
; read/set X,Y cursor position

; This routine, when called with the carry flag set, loads the current position of
; the cursor on the screen into the .X and .Y registers. .Y is the column number of
; the cursor location and .X is the row number of the cursor. A call with the carry
; bit clear moves the cursor to the position determined by the .X and .Y registers.

PLOT
  JMP FPLOT     ; read/set X,Y cursor position


;***********************************************************************************;
;
; return the base address of the I/O devices

; This routine will set .X.Y to the address of the memory section where the memory
; mapped I/O devices are located. This address can then be used with an offset to
; access the memory mapped I/O devices in the computer.

IOBASE
  JMP FIOBASE     ; return base address of I/O devices


;***********************************************************************************;
;
; spare bytes, not referenced

  .byte $FF,$FF,$FF,$FF


;***********************************************************************************;
;
; hardware vectors

  .word NMI     ; NMI vector
  .word START     ; RESET vector
  .word IRQROUT     ; IRQ vector

  .END


;***********************************************************************************;
;***********************************************************************************;
