

* = $0801


          !basic

          lda #$0f
          sta SID.FILTER_MODE_VOLUME

          lda #1
          sta EXTERNAL_INTERFACE_TABLE.PLAY_SOUND_EFFECT

Loop
          jsr WaitFrame

          jsr SFXUpdate

          lda EXTERNAL_INTERFACE_TABLE.PLAY_SOUND_EFFECT
          beq Loop

          lda #0
          sta EXTERNAL_INTERFACE_TABLE.PLAY_SOUND_EFFECT

          jsr ResetSid

          lda EXTERNAL_INTERFACE_TABLE.SOUND_EFFECT_TO_PLAY
          jsr SetCurrentSound
          jmp Loop


!lzone RestartSFX
          ldy WAVE_FORM_SETUP
          lda WAVE_FORM_TABLE,y
          sta SID_MIRROR + 4

          lda #0
          sta SID.FREQUENCY_LO_1 + 4

          ldx #0

-
          cpx #4
          beq ++
          lda SID_MIRROR,x
          sta SID_MIRROR_SETUP,x
          sta SID.FREQUENCY_LO_1,x

++
          inx
          cpx #7
          bne -

          lda SID_MIRROR + 4
          sta SID_MIRROR_SETUP + 4
          sta SID.FREQUENCY_LO_1 + 4

          rts


;a = sound effect
!lzone SetCurrentSound
          tax
          lda SFX_SLOT_EFFECT_WAVE,x
          and #$03
          sta WAVE_FORM_SETUP
          tay
          lda WAVE_FORM_TABLE,y
          sta SID_MIRROR_SETUP + 4

          lda SFX_SLOT_EFFECT_WAVE,x
          lsr
          lsr
          sta SID_EFFECT_SETUP

          lda SFX_SLOT_2_FX_LO,x
          sta SID_MIRROR_SETUP
          lda SFX_SLOT_3_FX_HI,x
          sta SID_MIRROR_SETUP + 1
          lda SFX_SLOT_4_AD,x
          sta SID_MIRROR_SETUP + 2
          lda SFX_SLOT_5_SR,x
          sta SID_MIRROR_SETUP + 3
          lda SFX_SLOT_6_PULSE_LO,x
          sta SID_MIRROR_SETUP + 5
          lda SFX_SLOT_7_PULSE_HI,x
          sta SID_MIRROR_SETUP + 6

          lda SFX_SLOT_8_DELTA,x
          sta EFFECT_DELTA_SETUP
          lda SFX_SLOT_9_DELAY,x
          sta EFFECT_DELAY_SETUP
          lda SFX_SLOT_10_STEP,x
          sta EFFECT_VALUE_SETUP

          jmp FullRestartSFX




!lzone FullRestartSFX
          ldx SID_MIRROR_SETUP
          ldy SID_MIRROR_SETUP + 1

;x = frequency lo
;y = frequency hi
!lzone FullRestartSFXWithFrequency
          stx SID_MIRROR
          sty SID_MIRROR + 1

          ldx #2
-
          lda SID_MIRROR_SETUP,x
          sta SID_MIRROR,x
          inx
          cpx #7
          bne -

          lda SID_EFFECT_SETUP
          sta SID_EFFECT

          lda EFFECT_VALUE_SETUP
          sta EFFECT_VALUE
          lda EFFECT_DELAY_SETUP
          sta EFFECT_DELAY
          lda EFFECT_DELTA_SETUP
          sta EFFECT_DELTA

          jmp RestartSFX


!lzone WaitFrame
          ;are we on line $F8 already? if so, wait for the next full screen
          ;prevents mistimings if called too fast
          lda VIC.RASTER_POS
          cmp #248
          beq WaitFrame

          ;wait for the raster to reach line $f8 (should be closer to the start of this line this way)
.WaitStep2
          lda VIC.RASTER_POS
          cmp #248
          bne .WaitStep2

          rts

!src "sfxplay.asm"

SFX_DATA
SFX_SLOT_EFFECT_WAVE
          !fill 100,( FX_SLIDE << 2 ) | ( FX_WAVE_PULSE )
SFX_SLOT_2_FX_LO
          !fill 100,$25
SFX_SLOT_3_FX_HI
          !fill 100,$4d
SFX_SLOT_4_AD
          !fill 100,$c5
SFX_SLOT_5_SR
          !fill 100,$ed
SFX_SLOT_6_PULSE_LO
          !fill 100,$95
SFX_SLOT_7_PULSE_HI
          !fill 100,$bd
SFX_SLOT_8_DELTA
          !fill 100,$03
SFX_SLOT_9_DELAY
          !fill 100,$5e
SFX_SLOT_10_STEP
          !fill 100,$32





!lzone EXTERNAL_INTERFACE_TABLE
;set to 1 to restart sound effect
.PLAY_SOUND_EFFECT
          !byte 0
;0 to 99 of sound effect to play
.SOUND_EFFECT_TO_PLAY
          !byte 0


!message "PLAY_SOUND_EFFECT = " , EXTERNAL_INTERFACE_TABLE.PLAY_SOUND_EFFECT
!message "SFX_DATA * 100    = " , SFX_DATA

!src <c64.asm>