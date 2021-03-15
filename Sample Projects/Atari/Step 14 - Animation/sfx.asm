; Like player graphics, sound data is stored in reverse order.
; two tables are used, SFX_F and SFX_CV.  Values in the tables are used in
; pairs, one from SFX_F and one from SFX_CV.  As such, both tables must be the
; same size.  Also, the size of each table is limited to just 256 bytes. DASM
; will output a compile-time warning if it spots a size problem.
;
; Each pair of values are used for a single frame (ie: 1/60th of a second).  A
; 0 value in the SFX_CV table means "end of sound effect", though for clarity
; it is recommended to also use a matching 0 in SFX_F.
;
; table SFX_F holds the Frequency for the sound effects.  
; each .byte line contains the Frequency data for a single sound effect.
; Frequency values range from 0-31
SFX_F:
    .byte 0, 31 ; collide
    .byte 0,  0,  0,  0,  1,  1,  1,  2,  2,  2,  3,  3,  3 ; collect
    .byte 0,  8,  8,  8,  8,  8,  8,  8,  8,  8,  8,  8,  8,  8,  8,  8 ; ping
    .byte 0, 31, 31, 31, 31, 31, 31, 31, 31, 31, 31, 31, 31, 31, 31, 31 ; game over
 
; calculate size of SFX_F table and validate size
SFX_Fcount = *-SFX_F
 if SFX_Fcount > 256
     echo "SFX Warning: table SFX_F is too large"
 endif
  
 
; table SFX_CV holds the sound effect Channel (tone) and Volume values.
; Both values range from 0-15, so they are combined together.
; The $ denotes a HEX value where the digits are 0123456789abcdef (a=10, f=15).
; the first digit is the Channel value. 
; the second digit is the Volume value.
; each .byte line contains the Channel and Volume data for a single sound effect
; the first value of every .byte line should be 0, which denotes end-of-sfx
; the = line below each .byte line calculates the value used when calling
; sfxtrigger.  
; Channel values are:
; 0 = No sound (silent).
; 1 = Buzzy tones.
; 2 = Carries distortion 1 downward into a rumble.
; 3 = Flangy wavering tones, like a UFO.
; 4 = Pure tone.
; 5 = Same as 4.
; 6 = Between pure tone and buzzy tone (Adventure death uses this).
; 7 = Reedy tones, much brighter, down to Enduro car rumble.
; 8 = White noise/explosions/lightning, jet/spacecraft engine.
; 9 = Same as 7.
; a = Same as 6.	 	 
; b = Same as 0.
; c = Pure tone, goes much lower in pitch than 4 & 5.
; d = Same as c.	 	 
; e = Electronic tones, mostly lows, extends to rumble. 
; f = Electronic tones, mostly lows, extends to rumble.

SFX_CV:
    .byte 0,$8f ; collide
sfxCOLLIDE = *-SFX_CV-1
    .byte 0,$6f,$6f,$6f,$6f,$6f,$6f,$6f,$6f,$6f,$6f,$6f,$6f ; collect
sfxCOLLECT = *-SFX_CV-1
    .byte 0,$41,$42,$43,$44,$45,$46,$47,$48,$49,$4a,$4b,$4c,$4d,$4e,$4f ; ping
sfxPING = *-SFX_CV-1
    .byte 0,$cf,$cf,$cf,$cf,$cf,$cf,$cf,$cf,$cf,$cf,$cf,$cf,$cf,$cf,$cf ; game over
sfxGAMEOVER = *-SFX_CV-1
 
 ; calculate size of SFX_CV table and validate size
SFX_CVcount = *-SFX_CV

 if SFX_CVcount > 256
     echo "SFX Warning: table SFX_CV is too large"
 endif
 if SFX_CVcount != SFX_Fcount
    echo "SFX Warning: table SFX_F is not the same size as table SFX_CV"
 endif


SFX_OFF:
         ldx #0             ; silence sound output
         stx SFX_LEFT
         stx SFX_RIGHT
         stx AUDV0
         stx AUDV1
         stx AUDC0
         stx AUDC1
         rts

SFX_TRIGGER:
         ldx SFX_LEFT       ; test left channel
         lda SFX_CV,x        ; CV value will be 0 if channel is idle 
         bne .leftnotfree   ; if not 0 then skip ahead
         sty SFX_LEFT       ; channel is idle, use it
         rts                ; all done
.leftnotfree: 
         ldx SFX_RIGHT      ; test right channel
         lda SFX_CV,x        ; CV value will be 0 if channel is idle
         bne .rightnotfree  ; if not 0 then skip ahead
         sty SFX_RIGHT      ; channel is idle, use it
         rts                ; all done
.rightnotfree:
         cpy SFX_LEFT       ; test sfx priority with left channel
         bcc .leftnotlower  ; skip ahead if new sfx has lower priority than active sfx
         sty SFX_LEFT       ; new sfx has higher priority so use left channel
         rts                ; all done
.leftnotlower: 
         cpy SFX_RIGHT      ; test sfx with right channel
         bcc .rightnotlower ; skip ahead if new sfx has lower priority than active sfx
         sty SFX_RIGHT      ; new sfx has higher priority so use right channel
.rightnotlower:
        rts
 
SFX_UPDATE:
         ldx SFX_LEFT       ; get the pointer for the left channel
         lda SFX_F,x         ; get the Frequency value
         sta AUDF0          ; update the Frequency register
         lda SFX_CV,x        ; get the combined Control and Volume value
         sta AUDV0          ; update the Volume register
         lsr                ; prep the Control value,
         lsr                ;   it's stored in the upper nybble
         lsr                ;   but must be in the lower nybble
         lsr                ;   when Control is updated
         sta AUDC0          ; update the Control register
         beq .skipleftdec   ; skip ahead if Control = 0
         dec SFX_LEFT       ; update pointer for left channel
.skipleftdec: 
         ldx SFX_RIGHT      ; get the pointer for the right channel
         lda SFX_F,x         ; get the Frequency value
         sta AUDF1          ; update the Frequency register
         lda SFX_CV,x        ; get the combined Control and Volume value
         sta AUDV1          ; update the Volume register
         lsr                ; prep the Control value,
         lsr                ;   it's stored in the upper nybble
         lsr                ;   but must be in the lower nybble
         lsr                ;   when Control is updated
         sta AUDC1          ; update the Control register
         beq .skiprightdec  ; skip ahead if Control = 0
         dec SFX_RIGHT      ; update pointer for right channel
.skiprightdec:
         rts                ; all done
 
