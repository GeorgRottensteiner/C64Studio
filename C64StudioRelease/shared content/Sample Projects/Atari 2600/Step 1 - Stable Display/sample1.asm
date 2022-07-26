;===============================================================================
; Program Information
;===============================================================================

    ; Program:      Collect
    ; Program by:   Darrell Spice, Jr
    ; Last Update:  June 24, 2014
    ;
    ; Super simple game of "collect the boxes" used for presentation on
    ; developing Atari 2600 homebrew games.
    ;
    ; See readme.txt for compile instructions
    
    
;===============================================================================
; Change Log
;===============================================================================
 
    ; 2013.06.24 - ver.  1 - generate a stable display 


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

    ; coming soon!

    
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
        sta WSYNC   ; Wait for SYNC (halts CPU until end of scanline)
        sta VSYNC   ; Accumulator D1=1, turns on Vertical Sync signal
        sta WSYNC   ; Wait for Sync - halts CPU until end of 1st scanline of VSYNC
        sta WSYNC   ; wait until end of 2nd scanline of VSYNC
        lda #0      ; LoaD Accumulator with 0 so D1=0
        sta WSYNC   ; wait until end of 3rd scanline of VSYNC
        sta VSYNC   ; Accumulator D1=0, turns off Vertical Sync signal
        rts         ; ReTurn from Subroutine
    
    
;===============================================================================
; Vertical Blank
; --------------
; game logic runs here.  
; Since we don't have any yet, just delay for 37 scanlines
;===============================================================================

VerticalBlank:    
        ldx #37         ; LoaD X with 37
vbLoop:        
        sta WSYNC       ; Wait for SYNC (halts CPU until end of scanline)
        dex             ; DEcrement X by 1
        bne vbLoop      ; Branch if Not Equal to 0
        rts             ; ReTurn from Subroutine

    
;===============================================================================
; Kernel
; ------
; here we update the registers in TIA (the video chip) in order to generate
; what the player sees.  For now we're just going to output 192 colored
; scanlines lines so we have something to see.
;===============================================================================

Kernel:            
    ; turn on the display
        sta WSYNC       ; Wait for SYNC (halts CPU until end of scanline)
        lda #0          ; LoaD Accumulator with 0 so D1=0
        sta VBLANK      ; Accumulator D1=1, turns off Vertical Blank signal (image output on)
        
    ; draw the screen        
        ldx #192        ; Load X with 192
KernelLoop:   
        sta WSYNC       ; Wait for SYNC (halts CPU until end of scanline)
        stx COLUBK      ; STore X into TIA's background color register
        dex             ; DEcrement X by 1
        bne KernelLoop  ; Branch if Not Equal to 0
        rts             ; ReTurn from Subroutine

        
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
    
        ldx #27     ; LoaD X with 27
osLoop:
        sta WSYNC   ; Wait for SYNC (halts CPU until end of scanline)
        dex         ; DEcrement X by 1
        bne osLoop  ; Branch if Not Equal to 0
        rts         ; ReTurn from Subroutine
    
        
;===============================================================================
; Define End of Cartridge
;===============================================================================
        ORG $FFFA        ; set address to 6507 Interrupt Vectors 
        .WORD InitSystem ; NMI
        .WORD InitSystem ; RESET
        .WORD InitSystem ; IRQ
        
