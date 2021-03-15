;===============================================================================
; Program Information
;===============================================================================

    ; Program:      Collect
    ; Program by:   Darrell Spice, Jr
    ; Last Update:  July 3, 2014
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
    ; 2014.07.03 - add 2LK (2 line kernel)


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
; Define Constants
;===============================================================================
    ; height of the arena (gameplay area).  Since we're using a 2 line kernel,
    ; actual height will be twice this.  Also, we're using 0-89 for the 
    ; scanlines so actual height is 180 = 90*2
ARENA_HEIGHT   = 89    

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
DigitOnes:      ds 2    ; stored in $82-83, DigitOnes = Score, DigitOnes+1 = Timer
DigitTens:      ds 2    ; stored in $84-85, DigitTens = Score, DigitTens+1 = Timer

    ; graphic data ready to put into PF1
ScoreGfx:       ds 1    ; stored in $86
TimerGfx:       ds 1    ; stored in $87

    ; scratch variable
Temp:           ds 1    ; stored in $88

    ; object X positions in $89-8C
ObjectX:        ds 4    ; player0, player1, missile0, missile1

    ; object Y positions in $8D-90
ObjectY:        ds 4    ; player0, player1, missile0, missile1

    ; DoDraw storage in $91-92
HumanDraw:      ds 1    ; used for drawing player0
BoxDraw:        ds 1    ; used for drawing player1

    ; DoDraw Graphic Pointer in $93-94
HumanPtr:       ds 2    ; used for drawing player0
BoxPtr:         ds 2    ; used for drawing player1

Frame:          ds 1    ; counts number of frames drawn

;===============================================================================
; Define Start of Cartridge
;===============================================================================

    ; define a segment for code
    SEG CODE    
    
    ; 2K ROM starts at $F800, 4K ROM starts at $F000
    ORG $F800

;===============================================================================
; PosObject
;----------
; subroutine for setting the X position of any TIA object
; when called, set the following registers:
;   A - holds the X position of the object
;   X - holds which object to position
;       0 = player0
;       1 = player1
;       2 = missile0
;       3 = missile1
;       4 = ball
; the routine will set the coarse X position of the object, as well as the
; fine-tune register that will be used when HMOVE is used.
;===============================================================================
PosObject:
        sec
        sta WSYNC
DivideLoop
        sbc #15        ; 2  2 - each time thru this loop takes 5 cycles, which is 
        bcs DivideLoop ; 2  4 - the same amount of time it takes to draw 15 pixels
        eor #7         ; 2  6 - The EOR & ASL statements convert the remainder
        asl            ; 2  8 - of position/15 to the value needed to fine tune
        asl            ; 2 10 - the X position
        asl            ; 2 12
        asl            ; 2 14
        sta.wx HMP0,X  ; 5 19 - store fine tuning of X
        sta RESP0,X    ; 4 23 - set coarse X position of object
        rts            ; 6 29

        
;===============================================================================
; Initialize Atari
;===============================================================================
    
InitSystem:
    ; CLEAN_START is a macro found in macro.h
    ; it sets all RAM, TIA registers and CPU registers to 0
        CLEAN_START   
             
    ; set starting location of player0 and player1 objects
        ldx #0
        stx ObjectX
        ldx #8
        stx ObjectX+1
        ldy #$30
        sty ObjectY
        sty ObjectY+1
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
        inc Frame   ; increment Frame count
        sta WSYNC   ; Wait for Sync - halts CPU until end of 1st scanline of VSYNC
        sta WSYNC   ; wait until end of 2nd scanline of VSYNC
        lda #0      ; LoaD Accumulator with 0 so D1=0
        sta PF0     ; blank the playfield
        sta PF1     ; blank the playfield
        sta PF2     ; blank the playfield
        sta GRP0    ; blanks player0 if VDELP0 is off
        sta GRP1    ; blanks player1 if VDELP1 is off, player0 if VDELP0 is on
        sta GRP0    ; blanks player1 if VDELP1 is on
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
        jsr ProcessJoystick
        jsr PositionObjects
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
;   xx = cycles instruction will take
;   yy = cumulative cycle count after instruction runs
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

    ; The Arena is drawn using what is known as a 2 line kernel, or 2LK for
    ; short. Basically the code is designed so that the TIA register updates are
    ; spread out over 2 scanlines instead of one.  TIA has a feature for the
    ; player objects, as well as the ball, called Vertical Delay which allows
    ; the objects to still start on any scanline even though they are only
    ; updated every-other scanline.  Vertical Delay is controlled by the TIA
    ; registers VDELP0, VDELP1 and VDELBL.
    ;
    ; ArenaLoop:
    ;       line 1 - updates player0, playfield
    ;       line 2 - updates player1, playfield
    ;       if not at bottom, goto ArenaLoop
    
    ; we need to preload GRP1 so that player1 can appear on the very first
    ; scanline of the Arena
    
        lda #1              ; 2  2
        sta CTRLPF          ; 3  5 - turn off SCORE mode and turn on REFLECT 
        ldy #ARENA_HEIGHT   ; 2  7 - the arena will be 180 scanlines (from 0-89)*2        
        
ArenaLoop:                  ;   13 - from bpl ArenaLoop
    ; continuation of line 2 of the 2LK
    ; this precalculates data that's used on line 1 of the 2LK
        lda #HUMAN_HEIGHT-1 ; 2 15 - height of the humanoid graphics, subtract 1 due to starting with 0
        dcp HumanDraw       ; 5 20 - Decrement HumanDraw and compare with height
        bcs DoDrawGrp0      ; 2 22 - (3 23) if Carry is Set, then humanoid is on current scanline
        lda #0              ; 2 24 - otherwise use 0 to turn off player0
        .byte $2C           ; 4 28 - $2C = BIT with absolute addressing, trick that
                            ;        causes the lda (HumanPtr),y to be skipped
DoDrawGrp0:                 ;   23 - from bcsDoDrawGrp0
        lda (HumanPtr),y    ; 5 28 - load the shape for player0
        sta WSYNC           ; 3 31
;---------------------------------------
    ; start of line 1 of the 2LK
        sta GRP0            ; 3  3 - @ 0-22, update player0 to draw Human
        ldx #%11111111      ; 2  5 - playfield pattern for vertical alignment testing
        stx PF0             ; 3  8 - @ 0-22
    ; precalculate data that's needed for line 2 of the 2LK        
        lda #HUMAN_HEIGHT-1 ; 2 10 - height of the humanoid graphics, 
        dcp BoxDraw         ; 5 15 - Decrement BoxDraw and compare with height
        bcs DoDrawGrp1      ; 2 17 - (3 18) if Carry is Set, then box is on current scanline
        lda #0              ; 2 19 - otherwise use 0 to turn off player1
        .byte $2C           ; 4 23 - $2C = BIT with absolute addressing, trick that
                            ;        causes the lda (BoxPtr),y to be skipped
DoDrawGrp1:                 ;   18 - from bcs DoDrawGRP1
        lda (BoxPtr),y      ; 5 23 - load the shape for the box
        sta WSYNC           ; 3 26
;---------------------------------------
    ; start of line 2 of the 2LK
        sta GRP1            ; 3  3 - @0-22, update player1 to draw box
        ldx #0              ; 2  5 - PF pattern for alignment testing
        stx PF0             ; 3  8 - @0-22
        dey                 ; 2 10 - decrease the 2LK loop counter
        bpl ArenaLoop       ; 2 12 - (3 13) branch if there's more Arena to draw
        rts                 ; 6 18 - ReTurn from Subroutine

        
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
; ProcessJoystick
; --------------
; Read left joystick and move the humanoid
; for testing, read right joystick and move second humanoid
;
; joystick directions are held in the SWCHA register of the RIOT chip.
; Directions are read via the following bit pattern:
;   76543210
;   RLDUrldu
;
; UPPERCASE denotes the left joystick directions
; lowercase denotes the right joystick directions
;
; NOTE the values are the opposite of what you might expect. If the direction
; is held, the bit value will be 0.
;
; Fire buttons are read via INPT4 (left) and INPT5 (right).  They are currently
; used to slow down player movement to make alignment testing easier.
;===============================================================================
ProcessJoystick:
        lda SWCHA       ; reads joystick positions
        
        ldx #0          ; x=0 for left joystick, x=1 for right
PJloop:    
        ldy INPT4,x     ; check the firebutton for this joystick
        bmi NormalSpeed ; if it's not held down then player moves at full speed 
        pha             ; PusH A onto stack (saves value of A)
        lda Frame       ; if it is held down, then only move once every 8 frames
        and #7
        beq SlowMovement
        pla             ; PuLl A from stack (restores value of A)
        asl             ; shift the 4 direction readings out of A
        asl             ; so the other joystick can be processed
        asl
        asl
        jmp NextJoystick
        
SlowMovement:
        pla                 ; PuLl A from stack (restores value of A)
NormalSpeed:        
        asl                 ; shift A bits left, R is now in the carry bit
        bcs CheckLeft       ; branch if joystick is not held right
        ldy ObjectX,x       ; get the object's X position
        iny                 ; and move it right
        cpy #160            ; test for edge of screen
        bne SaveX           ; save Y if we're not at the edge
        ldy #0              ; else wrap to left edge
SaveX:  sty ObjectX,x       ; saveX
        ldy #0              ; turn off reflect of player, which
        sty REFP0,x         ; makes humanoid image face right

CheckLeft:
        asl                 ; shift A bits left, L is now in the carry bit
        bcs CheckDown       ; branch if joystick not held left
        ldy ObjectX,x       ; get the object's X position
        dey                 ; and move it left
        cpy #255            ; test for edge of screen
        bne SaveX2          ; save X if we're not at the edge
        ldy #159            ; else wrap to right edge
SaveX2: sty ObjectX,x       ; save X
        ldy #8              ; turn on reflect of player, which
        sty REFP0,x         ; makes humanoid image face left 

CheckDown:
        asl                 ; shift A bits left, D is now in the carry bit
        bcs CheckUp         ; branch if joystick not held down
        ldy ObjectY,x       ; get the object's Y position
        dey                 ; move it down
        cpy #255            ; test for edge of screen
        bne SaveY           ; save Y if we're not at the edge
        ldy #ARENA_HEIGHT   ; else wrap to top
SaveY:  sty ObjectY,x       ; save Y

CheckUp:
        asl                 ; shift A bits left, U is now in the carry bit
        bcs NextJoystick    ; branch if joystick not held up
        ldy ObjectY,x       ; get the object's Y position
        iny                 ; move it up
        cpy #ARENA_HEIGHT+1 ; test for edge of screen
        bne SaveY2          ; save Y if we're not at the edge
        ldy #0              ; else wrap to bottom
SaveY2: sty ObjectY,x       ; save Y
        
NextJoystick:               
        inx                 ; increase loop control
        cpx #2              ; check if we've processed both joysticks
        bne PJloop          ; branch if we haven't
        
        rts
        
;===============================================================================
; PositionObjects
; --------------
; Updates TIA for X position of all objects
; Updates Kernel variables for Y position of all objects
;===============================================================================
PositionObjects:
        ldx #1              ; position objects 0-1: player0 and player1
POloop        
        lda ObjectX,x       ; get the object's X position
        jsr PosObject       ; set coarse X position and fine-tune amount 
        dex                 ; DEcrement X
        bpl POloop          ; Branch PLus so we position all objects
        sta WSYNC           ; wait for end of scanline
        sta HMOVE           ; use fine-tune values to set final X positions
        
        ; HumanDraw = ARENA_HEIGHT + HUMAN_HEIGHT - Y position
        lda #(ARENA_HEIGHT + HUMAN_HEIGHT)
        sec
        sbc ObjectY
        sta HumanDraw
        
        ; HumanPtr = HumanGfx + HUMAN_HEIGHT - 1 - Y position
        lda #<(HumanGfx + HUMAN_HEIGHT - 1)
        sec
        sbc ObjectY
        sta HumanPtr
        lda #>(HumanGfx + HUMAN_HEIGHT - 1)
        sbc #0
        sta HumanPtr+1
        
        ; BoxDraw = ARENA_HEIGHT + HUMAN_HEIGHT - Y position
        lda #(ARENA_HEIGHT + HUMAN_HEIGHT)
        sec
        sbc ObjectY+1
        sta BoxDraw
        
        ; BoxPtr = HumanGfx + HUMAN_HEIGHT - 1 - Y position
        lda #<(HumanGfx + HUMAN_HEIGHT - 1)
        sec
        sbc ObjectY+1
        sta BoxPtr
        lda #>(HumanGfx + HUMAN_HEIGHT - 1)
        sbc #0
        sta BoxPtr+1
        
    ; use Difficulty Switches to test how Vertical Delay works        
        ldx #0
        stx VDELP0      ; turn off VDEL for player0
        stx VDELP1      ; turn off VDEL for player1
        inx
        bit SWCHB       ; state of Right Difficult in N (negative flag)
                        ; state of Left Difficult in V (overflow flag)
        bvc LeftIsB
        stx VDELP0      ; Left is A, turn on VDEL for player0
LeftIsB:
        bpl RightIsB
        stx VDELP1      ; Right is A, turn on VDEL for player1
RightIsB:
        rts

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
    ; for testing purposes, set Score to Humanoid Y and Timer to Box Y
        lda ObjectY
        sta Score
        lda ObjectY+1
        sta Timer
        
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
   
HumanGfx:
        .byte %00011100
        .byte %00011000
        .byte %00011000
        .byte %00011000
        .byte %01011010
        .byte %01011010
        .byte %00111100
        .byte %00000000
        .byte %00011000
        .byte %00011000
HUMAN_HEIGHT = * - HumanGfx        
        
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
