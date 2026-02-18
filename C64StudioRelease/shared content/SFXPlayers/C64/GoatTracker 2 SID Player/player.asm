

* = $0801


          !basic

          lda #$0f
          sta SID.FILTER_MODE_VOLUME

          lda #1
          sta EXTERNAL_INTERFACE_TABLE.PLAY_SOUND_EFFECT

          ;init player
          lda #0
          sta GOATTRACKER_PLAYER

Loop
          jsr WaitFrame

          jsr GOATTRACKER_PLAYER + 3

          inc EXTERNAL_INTERFACE_TABLE.PLAY_SOUND_EFFECT

          lda EXTERNAL_INTERFACE_TABLE.PLAY_SOUND_EFFECT
          cmp #1
          bne Loop

          ;lda #0
          ;sta EXTERNAL_INTERFACE_TABLE.PLAY_SOUND_EFFECT

          lda #<EXTERNAL_INTERFACE_TABLE.SFX_DATA
          ldy #>EXTERNAL_INTERFACE_TABLE.SFX_DATA
          ldx #7            ;0, 7 or 14 for channels 1-3
          jsr GOATTRACKER_PLAYER + 6
          jmp Loop


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

* = $900
GOATTRACKER_PLAYER
!bin "c64studio900.prg",,2


!lzone EXTERNAL_INTERFACE_TABLE
;set to 1 to restart sound effect
.PLAY_SOUND_EFFECT
          !byte 0
.SFX_DATA
        !byte $0A,$00,$02,$A0,$41,$A0,$A0,$A4,$A4,$A4,$A7,$A7,$A7,$A0,$A0,$A0
        !byte $A4,$A4,$A4,$A7,$A7,$A7,$A0,$A0,$A0,$A4,$A4,$A4,$A7,$A7,$A7,$A0
        !byte $A0,$A0,$A4,$A4,$A4,$A7,$A7,$A7,$00

!message "PLAY_SOUND_EFFECT = " , EXTERNAL_INTERFACE_TABLE.PLAY_SOUND_EFFECT
!message "SFX_DATA * 100    = " , EXTERNAL_INTERFACE_TABLE.SFX_DATA

!src <c64.asm>