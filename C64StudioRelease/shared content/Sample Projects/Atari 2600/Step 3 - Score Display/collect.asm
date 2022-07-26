;===============================================================================
; Program Information
;===============================================================================

    ; Program:      Collect
    ; Program by:   Darrell Spice, Jr
    ; Last Update:  June 28, 2014
    ;
    ; Super simple game of "collect the boxes" used for presentation on
    ; developing Atari 2600 homebrew games.
    ;
    ; See readme.txt for compile instructions
    
    
;===============================================================================
; Change Log
;===============================================================================
 
    ; 2014.06.24 - generate a stable display
    ; 2014.06.25 - add timers 
    ; 2014.06.28 - add score display and check for TV Type


;===============================================================================
; Initialize dasm
;===============================================================================

    ; Dasm supports a number of processors, this line tells dasm the code
    ; is for the 6502 CPU.  The Atari has a 6507, which is 6502 that's been
    ; put into a "reduced package".  This package limits the 6507 to an 8K
    ; address space and also removes support for external interrupts.
        PROCESSOR 6502
    
    ; vcs.h contains the standard definitions for TIA and RIOT registers 
        include vcs.h       
    
    ; macro.h contains commonly used routines which aid in coding
        include macro.h

    
;===============================================================================
; Define RAM Usage
;===============================================================================

    ; define a segment for variables
    ; .U means uninitialized, does not end up in ROM
        SEG.U VARS
    
    ; RAM starts at $80
        ORG $80             

    ; Holds 2 digit score, stored as BCD (Binary Coded Decimal)
Score:          ds 1    ; stored in $80

    ; Holds 2 digit timer, stored as BCD
Timer:          ds 1    ; stored in $81

    ; Offsets into digit graphic data
DigitOnes:      ds 2    ; stored in $82-$83, DigitOnes = Score, DigitOnes+1 = Timer
DigitTens:      ds 2    ; stored in $84-$85, DigitTens = Score, DigitTens+1 = Timer

    ; graphic data ready to put into PF1
ScoreGfx:       ds 1    ; stored in $86
TimerGfx:       ds 1    ; stored in $87

    ; scratch variable
Temp:           ds 1    ; stored in $88
    

;===============================================================================
; Define Start of Cartridge
;===============================================================================

    ; define a segment for code
    SEG CODE    
    
    ; 2K ROM starts at $F800, 4K ROM starts at $F000
    ORG $F800


;===============================================================================
; Initialize Atari
;===============================================================================
    
InitSystem:
    ; CLEAN_START is a macro found in macro.h
    ; it sets all RAM, TIA registers and CPU registers to 0
        CLEAN_START   
             
    ; from here we "fall into" the main loop
    

;===============================================================================
; Main Program Loop
;===============================================================================

Main:
        jsr VerticalSync    ; Jump to SubRoutine VerticalSync
        jsr VerticalBlank   ; Jump to SubRoutine VerticalBlank
        jsr Kernel          ; Jump to SubRoutine Kernel
        jsr OverScan        ; Jump to SubRoutine OverScan
        jmp Main            ; JuMP to Main
    

;===============================================================================
; Vertical Sync
; -------------
; here we generate the signal that tells the TV to move the beam to the top of
; the screen so we can start the next frame of video.
; The Sync Signal must be on for 3 scanlines.
;===============================================================================

VerticalSync:
        lda #2      ; LoaD Accumulator with 2 so D1=1
        ldx #49     ; LoaD X with 49
        sta WSYNC   ; Wait for SYNC (halts CPU until end of scanline)
        sta VSYNC   ; Accumulator D1=1, turns on Vertical Sync signal
        stx TIM64T  ; set timer to go off in 41 scanlines (49 * 64) / 76
        sta CTRLPF  ; D1=1, playfield now in SCORE mode
        sta WSYNC   ; Wait for Sync - halts CPU until end of 1st scanline of VSYNC
        sta WSYNC   ; wait until end of 2nd scanline of VSYNC
        lda #0      ; LoaD Accumulator with 0 so D1=0
        sta PF0     ; blank the playfield
        sta PF1     ; blank the playfield
        sta PF2     ; blank the playfield
        sta WSYNC   ; wait until end of 3rd scanline of VSYNC
        sta VSYNC   ; Accumulator D1=0, turns off Vertical Sync signal
Sleep12:            ;       jsr here to sleep for 12 cycles        
        rts         ; ReTurn from Subroutine
    
    
;===============================================================================
; Vertical Blank
; --------------
; game logic runs here.
;===============================================================================

VerticalBlank:    
        jsr SetObjectColors
        jsr PrepScoreForDisplay
        rts             ; ReTurn from Subroutine

    
;===============================================================================
; Kernel
; ------
; here we update the registers in TIA, the video chip, scanline by scanline
; in order to generate what the player sees.
;
; Timing is crucial in the kernel, so we need to count the cycles.  You may
; use your own method of counting cycles, this is how I do it:
;       instruction     ;xx yy - comment
;   xx = cycle instruction will take
;   yy = cycle count for current scanline
;   comment = what's going on.  Some instructions have special notation:
;       @aa-bb where aa and bb are numbers.  These are used to denote that the
;           instruction MUST be done within a range of cycles.  This is especially
;           true of updating the playfield where you need to update the register
;           twice on a scanline if you want the left and right side of the screen
;           to show different images.  If aa > bb that means the instruction can
;           be executed on the prior scanline on or after cycle aa.
;       (a b) where a and b are numbers.  These are used for branches to show
;           the cycles and cycle count if the branch is taken.
;
; The following is used to denote when a new scanline starts:
;---------------------------------------
;
;===============================================================================

Kernel:            
        sta WSYNC       ; Wait for SYNC (halts CPU until end of scanline)
;---------------------------------------                
        lda INTIM       ; 4  4 - check the timer
        bne Kernel      ; 2  6 - (3 7) Branch if its Not Equal to 0
    ; turn on the display
        sta VBLANK      ; 3  9 - Accumulator D1=0, turns off Vertical Blank signal (image output on)
        ldx #5          ; 2 11 - use X as the loop counter for ScoreLoop
        
    ; first thing we draw is the score.  Score is drawn using only PF1 of the
    ; playfield.  The playfield is set for in repeat mode, and SCORE is turned
    ; on so the left and right sides take on the colors of player0 and player1.
    ; To get here we can fall thru from above (cycle 11) OR loop back from below
    ; (cycle 43). We'll cycle count from the worst case scenario
ScoreLoop:              ;   43 - cycle after bpl ScoreLoop
        ldy DigitTens   ; 3 46 - get the tens digit offset for the Score
        lda DigitGfx,y  ; 5 51 -   use it to load the digit graphics
        and #$F0        ; 2 53 -   remove the graphics for the ones digit
        sta ScoreGfx    ; 3 56 -   and save it
        ldy DigitOnes   ; 3 59 - get the ones digit offset for the Score
        lda DigitGfx,y  ; 5 64 -   use it to load the digit graphics
        and #$0F        ; 2 66 -   remove the graphics for the tens digit
        ora ScoreGfx    ; 3 69 -   merge with the tens digit graphics
        sta ScoreGfx    ; 3 72 -   and save it
        sta WSYNC       ; 3 75 - wait for end of scanline
;---------------------------------------        
        sta PF1         ; 3  3 - @66-28, update playfield for Score dislay
        ldy DigitTens+1 ; 3  6 - get the left digit offset for the Timer
        lda DigitGfx,y  ; 5 11 -   use it to load the digit graphics
        and #$F0        ; 2 13 -   remove the graphics for the ones digit
        sta TimerGfx    ; 3 16 -   and save it
        ldy DigitOnes+1 ; 3 19 - get the ones digit offset for the Timer
        lda DigitGfx,y  ; 5 24 -   use it to load the digit graphics
        and #$0F        ; 2 26 -   remove the graphics for the tens digit
        ora TimerGfx    ; 3 29 -   merge with the tens digit graphics
        sta TimerGfx    ; 3 32 -   and save it
        jsr Sleep12     ;12 44 - waste some cycles
        sta PF1         ; 3 47 - @39-54, update playfield for Timer display
        ldy ScoreGfx    ; 3 50 - preload for next scanline 
        sta WSYNC       ; 3 53 - wait for end of scanline
;---------------------------------------
        sty PF1         ; 3  3 - update playfield for the Score display
        inc DigitTens   ; 5  8 - advance for the next line of graphic data
        inc DigitTens+1 ; 5 13 - advance for the next line of graphic data
        inc DigitOnes   ; 5 18 - advance for the next line of graphic data
        inc DigitOnes+1 ; 5 23 - advance for the next line of graphic data
        jsr Sleep12     ;12 35 - waste some cycles
        dex             ; 2 37 - decrease the loop counter
        sta PF1         ; 3 40 - @39-54, update playfield for the Timer display
        bne ScoreLoop   ; 2 42 - (3 43) if dex != 0 then branch to ScoreLoop
        sta WSYNC       ; 3 45 - wait for end of scanline
;---------------------------------------
        stx PF1         ; 3  3 - x = 0, so this blanks out playfield        
        sta WSYNC       ; 3  6 - wait for end of scanline
;---------------------------------------        
        sta WSYNC       ; 3  3 - put some white space between Score/Timer and the arena
;---------------------------------------        
        ldx #179        ; 2  2 - the arena will be 180 scanlines (from 0-179)
KernelLoop:   
        sta WSYNC       ; 3  5 - Wait for SYNC (halts CPU until end of scanline)
;---------------------------------------        
        stx COLUBK      ; 3  3 - STore X into TIA's background color register
        dex             ; 2  5 - DEcrement X by 1
        bne KernelLoop  ; 2  7 - (3 8) Branch if Not Equal to 0
        rts             ; 6 13 - ReTurn from Subroutine

        
;===============================================================================
; Overscan
; --------------
; game logic runs here.  Since we don't have any yet, just delay so that the
; entire video frame consists of 262 scanlines
;===============================================================================

OverScan:
        sta WSYNC   ; Wait for SYNC (halts CPU until end of scanline)
        lda #2      ; LoaD Accumulator with 2 so D1=1
        sta VBLANK  ; STore Accumulator to VBLANK, D1=1 turns image output off
        
    ; set the timer for 27 scanlines.  Each scanline lasts 76 cycles,
    ; but the timer counts down once every 64 cycles, so use this
    ; formula to figure out the value to set.  
    ;       (scanlines * 76) / 64    
    ; Also note that it might be slight off due to when on the scanline TIM64T
    ; is updated.  So use Stella to check how many scanlines the code is
    ; generating and adjust accordingly.
        lda #32     ; set timer for 27 scanlines, 32 = ((27 * 76) / 64)
        sta TIM64T  ; set timer to go off in 27 scanlines
        
    ; game logic will go here
    
OSwait:
        sta WSYNC   ; Wait for SYNC (halts CPU until end of scanline)
        lda INTIM   ; Check the timer
        bne OSwait  ; Branch if its Not Equal to 0
        rts         ; ReTurn from Subroutine
    
        
;===============================================================================
; SetObjectColors
; --------------
; Set the 4 color registers based on the state of TV Type.
; Eventually this will also handle color cycling of attract mode
;===============================================================================
SetObjectColors:        
        ldx #3          ; we're going to set 4 colors (0-3)
        ldy #3          ; default to the color entries in the table (0-3)
        lda SWCHB       ; read the state of the console switches
        and #%00001000  ; test state of D3, the TV Type switch
        bne SOCloop     ; if D3=1 then use color
        ldy #7          ; else use the b&w entries in the table (4-7)
SOCloop:        
        lda Colors,y    ; get the color or b&w value
        sta COLUP0,x    ; and set it
        dey             ; decrease Y
        dex             ; decrease X 
        bpl SOCloop     ; Branch PLus (positive)
        rts             ; ReTurn from Subroutine
        
Colors:   
        .byte $86   ; blue       - goes into COLUP0, color for player0 and missile0
        .byte $C6   ; green      - goes into COLUP1, color for player1 and missile1
        .byte $46   ; red        - goes into COLUPF, color for playfield and ball
        .byte $00   ; black      - goes into COLUBK, color for background
        .byte $0E   ; white      - goes into COLUP0, color for player0 and missile0
        .byte $06   ; dark grey  - goes into COLUP1, color for player1 and missile1
        .byte $0A   ; light grey - goes into COLUPF, color for playfield and ball
        .byte $00   ; black      - goes into COLUBK, color for background
        
;===============================================================================
; PrepScoreForDisplay
; --------------
; Converts the high and low nybbles of the RAM variables Score and Timer
; into offsets into the digit graphics so the values can be displayed.
; Each digit uses 5 bytes of data for the graphics.  For the low nybble we need
; to multiply by 5, but the 6507 does not have a multiply feature.  It can,
; however, shift the bits in a byte left, which is the same as a multiply by 2.
; Using this, we can get multiply a # by 5 like this:
;       # * 5 = (# * 2 * 2) + #
; The value in the upper nybble is already times 16, so we need to divide it.
; The 6507 can shift the bits the right, which is the same as divide by 2.
;       (# / 16) * 5 = (# / 2 / 2) + (# / 2 / 2 / 2 / 2)  
;===============================================================================

PrepScoreForDisplay:
    ; for testing purposes, change the values in Timer and Score
        inc Timer       ; INCrement Timer by 1
        bne PSFDskip    ; Branch Not Equal to 0
        inc Score       ; INCrement Score by 1 if Timer just rolled to 0
        
PSFDskip        
        ldx #1          ; use X as the loop counter for PSFDloop
PSFDloop:
        lda Score,x     ; LoaD A with Timer(first pass) or Score(second pass)
        and #$0F        ; remove the tens digit
        sta Temp        ; Store A into Temp
        asl             ; Accumulator Shift Left (# * 2)
        asl             ; Accumulator Shift Left (# * 4)
        adc Temp        ; ADd with Carry value in Temp (# * 5)
        sta DigitOnes,x  ; STore A in DigitOnes+1(first pass) or DigitOnes(second pass)
        lda Score,x     ; LoaD A with Timer(first pass) or Score(second pass)
        and #$F0        ; remove the ones digit
        lsr             ; Logical Shift Right (# / 2)
        lsr             ; Logical Shift Right (# / 4)
        sta Temp        ; Store A into Temp
        lsr             ; Logical Shift Right (# / 8)
        lsr             ; Logical Shift Right (# / 16)
        adc Temp        ; ADd with Carry value in Temp ((# / 16) * 5)
        sta DigitTens,x ; STore A in DigitTens+1(first pass) or DigitTens(second pass)
        dex             ; DEcrement X by 1
        bpl PSFDloop    ; Branch PLus (positive) to PSFDloop
        rts             ; ReTurn from Subroutine      

        
;===============================================================================
; free space check before DigitGfx
;===============================================================================
        
 if (* & $FF)
    echo "------", [(>.+1)*256 - .]d, "bytes free before DigitGfx"
    align 256
  endif    
    
  
;===============================================================================
; Digit Graphics
;===============================================================================
        align 256
DigitGfx:
        .byte %01110111
        .byte %01010101
        .byte %01010101
        .byte %01010101
        .byte %01110111
        
        .byte %00010001
        .byte %00010001
        .byte %00010001
        .byte %00010001        
        .byte %00010001
        
        .byte %01110111
        .byte %00010001
        .byte %01110111
        .byte %01000100
        .byte %01110111
        
        .byte %01110111
        .byte %00010001
        .byte %00110011
        .byte %00010001
        .byte %01110111
        
        .byte %01010101
        .byte %01010101
        .byte %01110111
        .byte %00010001
        .byte %00010001
        
        .byte %01110111
        .byte %01000100
        .byte %01110111
        .byte %00010001
        .byte %01110111
           
        .byte %01110111
        .byte %01000100
        .byte %01110111
        .byte %01010101
        .byte %01110111
        
        .byte %01110111
        .byte %00010001
        .byte %00010001
        .byte %00010001
        .byte %00010001
        
        .byte %01110111
        .byte %01010101
        .byte %01110111
        .byte %01010101
        .byte %01110111
        
        .byte %01110111
        .byte %01010101
        .byte %01110111
        .byte %00010001
        .byte %01110111
        
        .byte %00100010
        .byte %01010101
        .byte %01110111
        .byte %01010101
        .byte %01010101
         
        .byte %01100110
        .byte %01010101
        .byte %01100110
        .byte %01010101
        .byte %01100110
        
        .byte %00110011
        .byte %01000100
        .byte %01000100
        .byte %01000100
        .byte %00110011
        
        .byte %01100110
        .byte %01010101
        .byte %01010101
        .byte %01010101
        .byte %01100110
        
        .byte %01110111
        .byte %01000100
        .byte %01100110
        .byte %01000100
        .byte %01110111
        
        .byte %01110111
        .byte %01000100
        .byte %01100110
        .byte %01000100
        .byte %01000100
        
;===============================================================================
; free space check before End of Cartridge
;===============================================================================
        
 if (* & $FF)
    echo "------", [$FFFA - *]d, "bytes free before End of Cartridge"
    align 256
  endif    
        
;===============================================================================
; Define End of Cartridge
;===============================================================================
        ORG $FFFA        ; set address to 6507 Interrupt Vectors 
        .WORD InitSystem ; NMI
        .WORD InitSystem ; RESET
        .WORD InitSystem ; IRQ
