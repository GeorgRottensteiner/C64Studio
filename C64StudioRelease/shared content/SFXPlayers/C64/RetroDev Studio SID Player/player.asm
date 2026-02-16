

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

          lda #0
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

          lda #1
          sta SFX_ACTIVE

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


!lzone EXTERNAL_INTERFACE_TABLE
;set to 1 to restart sound effect
.PLAY_SOUND_EFFECT
          !byte 0
.SFX_DATA
SFX_SLOT_EFFECT_WAVE
          !byte ( FX_SLIDE << 2 ) | ( FX_WAVE_TRIANGLE )
SFX_SLOT_2_FX_LO
          !byte $40 ; $81
SFX_SLOT_3_FX_HI
          !byte $1f ;$a9
SFX_SLOT_4_AD
          !byte $05 ;$22
SFX_SLOT_5_SR
          !byte $00 ;$4a
SFX_SLOT_6_PULSE_LO
          !byte $55 ;$d1
SFX_SLOT_7_PULSE_HI
          !byte $55 ;$fa
SFX_SLOT_8_DELTA
          !byte $05 ;$04
SFX_SLOT_9_DELAY
          !byte $05 ;$a6
SFX_SLOT_10_STEP
          !byte $05 ;$72


!message "PLAY_SOUND_EFFECT = " , EXTERNAL_INTERFACE_TABLE.PLAY_SOUND_EFFECT
!message "SFX_DATA * 100    = " , EXTERNAL_INTERFACE_TABLE.SFX_DATA

!src <c64.asm>