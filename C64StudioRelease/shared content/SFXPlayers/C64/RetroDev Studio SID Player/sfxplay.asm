FX_SLIDE              = 0
FX_NONE               = 1
FX_STEP               = 2
FX_SLIDE_PING_PONG    = 3

FX_WAVE_TRIANGLE      = 0
FX_WAVE_SAWTOOTH      = 1
FX_WAVE_PULSE         = 2
FX_WAVE_NOISE         = 3


ZP_ADDRESS            = $57

NUM_CHANNELS          = 1

!lzone SFXPlay
;x = sfx address lo
;y = sfx address hi
;expect 10 bytes, (FFFFFFWW) = Effect + Waveform
;                FX lo/hi, Pulse Lo, Pulse Hi, AD, SR, Effect Delta, Effect Delay, Effect Step
          stx ZP_ADDRESS
          sty ZP_ADDRESS + 1

          ldy #0
          lda (ZP_ADDRESS),y
          lsr
          lsr
          sta SID_EFFECT_SETUP
          sta SID_EFFECT


          lda (ZP_ADDRESS),y
          and #$03
          tay
          sty WAVE_FORM_SETUP
          lda WAVE_FORM_TABLE,y
          sta SID_MIRROR + 4

          lda #0
          sta SID.FREQUENCY_LO_1 + 4

          ldx #0
          ldy #1

-
          cpx #4
          beq ++

          lda (ZP_ADDRESS),y
          sta SID_MIRROR,x
          sta SID_MIRROR_SETUP,x
          sta SID.FREQUENCY_LO_1,x


          iny
++
          inx
          cpx #7
          bne -

          ldy #4
          lda SID_MIRROR + 4
          sta SID_MIRROR_SETUP + 4,x
          sta SID.FREQUENCY_LO_1 + 4

          ldy #7
          lda (ZP_ADDRESS),y
          sta EFFECT_DELTA
          sta EFFECT_DELTA_SETUP

          iny
          lda (ZP_ADDRESS),y
          sta EFFECT_DELAY
          sta EFFECT_DELAY_SETUP

          iny
          lda (ZP_ADDRESS),y
          sta EFFECT_VALUE
          sta EFFECT_VALUE_SETUP

          lda #1
          sta SFX_ACTIVE
          rts



!lzone ResetSid
          lda #$ff
.resetSidLoop
          ldx #$17
-
          sta $d400, x
          dex
          bpl -
          tax
          bpl +
          lda #$08
          bpl .resetSidLoop
+
-
          bit $d011
          bpl -
-
          bit $d011
          bmi -
          eor #$08
          beq .resetSidLoop

          lda #$0f
          sta $d418

          rts



!lzone SFXUpdate
          ;no SFX playing
          lda SFX_ACTIVE
          bne +

          rts

+
          ldy SID_EFFECT
          lda FX_TABLE_LO,y
          sta .JumpPos
          lda FX_TABLE_HI,y
          sta .JumpPos + 1

.JumpPos = * + 1
          jmp $ffff




FX_TABLE_LO
          !byte <FXSlide
          !byte <FXNone
          !byte <FXStep
          !byte <FXPingPong

FX_TABLE_HI
          !byte >FXSlide
          !byte >FXNone
          !byte >FXStep
          !byte >FXPingPong


!lzone FXSlide
          dec EFFECT_DELAY
          beq FXOff

          lda EFFECT_DELTA
          bpl .Up

          lda SID_MIRROR + 1
          clc
          adc EFFECT_DELTA
          bcc .Overflow
          jmp +


.Up
          lda SID_MIRROR + 1
          clc
          adc EFFECT_DELTA
          bcs .Overflow
+
          sta SID_MIRROR + 1
          sta SID.FREQUENCY_LO_1 + 1
          rts

.Overflow

FXOff
          lda #0
          sta EFFECT_DELTA
          sta SID.CONTROL_WAVE_FORM_1
          sta SFX_ACTIVE
          rts



!lzone FXNone
          dec EFFECT_DELAY
          beq FXOff
          rts



!lzone FXStep
          dec EFFECT_DELAY
          bne .NoStep

          ;step, switch to slide
          lda SID_MIRROR + 1
          clc
          adc EFFECT_VALUE
          sta SID_MIRROR + 1
          sta SID.FREQUENCY_LO_1 + 1

          lda #0
          sta EFFECT_DELTA
          lda EFFECT_DELAY_SETUP
          sta EFFECT_DELAY

          lda #FX_SLIDE
          sta SID_EFFECT

.NoStep
          rts



!lzone FXPingPong
          dec EFFECT_VALUE
          bne .GoSlide

          lda EFFECT_VALUE_SETUP
          sta EFFECT_VALUE

          lda EFFECT_DELTA
          eor #$ff
          clc
          adc #1
          sta EFFECT_DELTA

.GoSlide
          jmp FXSlide



WAVE_FORM_TABLE
          !byte 17,33,65,129

SFX_ACTIVE
          !byte 0



SID_MIRROR
          !fill 7 * NUM_CHANNELS
SID_MIRROR_SETUP
          !fill 7 * NUM_CHANNELS

WAVE_FORM_SETUP
          !fill NUM_CHANNELS

SID_EFFECT
          !fill NUM_CHANNELS
SID_EFFECT_SETUP
          !fill NUM_CHANNELS

EFFECT_DELTA
          !fill NUM_CHANNELS
EFFECT_DELTA_SETUP
          !fill NUM_CHANNELS

EFFECT_DELAY
          !fill NUM_CHANNELS
EFFECT_DELAY_SETUP
          !fill NUM_CHANNELS
EFFECT_VALUE
          !fill NUM_CHANNELS
EFFECT_VALUE_SETUP
          !fill NUM_CHANNELS

