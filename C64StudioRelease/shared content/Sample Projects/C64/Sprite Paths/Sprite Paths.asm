!src <c64.asm>

;zero page position for a pointer to the current step data of the current path
CURRENT_PATH_POS = $fe



* = $0801
!basic
          ;setup sprite
          ;image
          lda #( SPRITE_DATA / 64 )
          sta 2040

          ;colors
          lda #6
          sta VIC.SPRITE_MULTICOLOR_1
          lda #2
          sta VIC.SPRITE_MULTICOLOR_2
          lda #$01
          sta VIC.SPRITE_MULTICOLOR

          ;and enable
          lda #$01
          sta VIC.SPRITE_ENABLE

          lda #14
          sta VIC.BACKGROUND_COLOR

          ;start with the first path of the project
          lda #0
          jsr InitPath


!lzone Loop
          ;wait for raster line 240 (e.g. one frame)
          lda #240
-
          cmp VIC.RASTER_POS
          bne -

          ;press 1,2, ... to use different paths
          jsr KERNAL.GETIN
          cmp #'1'
          bcc .InvalidKey
          cmp #'1' + SAMPLE_NUM_PATHS
          bcs .InvalidKey

          sec
          sbc #'1'
          jsr InitPath

.InvalidKey
          ;keep moving in current direction?
          dec CURRENT_DURATION
          bne .KeepGoingInCurrentDirection

          ;jump to next step (or reset to first)
          ldy #0
          lda ( CURRENT_PATH_POS ), y
          bpl .SetupNextStep

          ;reset current path to start
          ldy CURRENT_PATH
          lda SAMPLE_PATH_LIST_LO, y
          sta CURRENT_PATH_POS
          lda SAMPLE_PATH_LIST_HI, y
          sta CURRENT_PATH_POS + 1
          jmp .NextStepIsSetUp


.SetupNextStep
          ;increase pointer by two bytes
          lda CURRENT_PATH_POS
          clc
          adc #2
          sta CURRENT_PATH_POS
          bcc +
          inc CURRENT_PATH_POS + 1
+
.NextStepIsSetUp
          jsr SetupPathStep

.KeepGoingInCurrentDirection
          ;handle step movement
          ldy CURRENT_DIR

          ;handle x (a bit complicated to handle the extended X bit)
          lda DIR_DELTA_X, y
          beq .NoXMovement      ;delta = 0, nothing to do
          bmi .GoLeft           ;negative, go left

          ;positive, go right
          inc SPRITE_POS_X
          bne .XMovementDone
          jmp .WrappedX

.GoLeft
          dec SPRITE_POS_X
          lda SPRITE_POS_X
          cmp #$ff
          beq .WrappedX
          jmp .XMovementDone


.WrappedX
          ;wrap over 256 byte barrier, simply toggle the X bit
          lda VIC.SPRITE_X_EXTEND
          eor #$01
          sta VIC.SPRITE_X_EXTEND

.NoXMovement
.XMovementDone
          ;handle y
          lda SPRITE_POS_Y
          clc
          adc DIR_DELTA_Y, y
          sta SPRITE_POS_Y

          ;set coordinates
          lda SPRITE_POS_X
          sta VIC.SPRITE_X_POS
          lda SPRITE_POS_Y
          sta VIC.SPRITE_Y_POS
          jmp Loop


;these deltas must match the configured step values in the path project
DIR_DELTA_X
          !byte 0     ;DIR_NONE
          !byte 0     ;DIR_N
          !byte 1     ;DIR_NE
          !byte 1     ;DIR_E
          !byte 1     ;DIR_SE
          !byte 0     ;DIR_S
          !byte -1    ;DIR_SW
          !byte -1    ;DIR_W
          !byte -1    ;DIR_NW

DIR_DELTA_Y
          !byte 0     ;DIR_NONE
          !byte -1    ;DIR_N
          !byte -1    ;DIR_NE
          !byte 0     ;DIR_E
          !byte 1     ;DIR_SE
          !byte 1     ;DIR_S
          !byte 1     ;DIR_SW
          !byte 0     ;DIR_W
          !byte -1    ;DIR_NW


;a = path index to move
!lzone InitPath
          sta CURRENT_PATH

          ;reset sprite position
          lda #160
          sta SPRITE_POS_X
          lda #60
          sta SPRITE_POS_Y
          lda #0
          sta VIC.SPRITE_X_EXTEND

          ;reset path
          ldy CURRENT_PATH
          lda SAMPLE_PATH_LIST_LO, y
          sta CURRENT_PATH_POS
          lda SAMPLE_PATH_LIST_HI, y
          sta CURRENT_PATH_POS + 1

          ;our path data is using two bytes
          ;  byte #1:  Lxxx SSSS   (L = last step, SSSS = step type)
          ;  byte #2:  DDDD DDDD   (D = duration of step in frames)
          jmp SetupPathStep



!lzone SetupPathStep
          ;first byte, step type
          ldy #0
          lda ( CURRENT_PATH_POS ), y
          and #$0f
          sta CURRENT_DIR

          ;second byte is the duration
          iny
          lda ( CURRENT_PATH_POS ), y
          sta CURRENT_DURATION
          rts



SPRITE_POS_X
          !byte 160

SPRITE_POS_Y
          !byte 60

;the direction the path is moving towards
CURRENT_DIR
          !byte 0

;the duration (number of frames) still left to move for the current step
CURRENT_DURATION
          !byte 0

CURRENT_PATH
          !byte 0

;place the sprite data in memory, at a 64 byte page
!realign 64

SPRITE_DATA
!media "paths.spriteproject",SPRITE,0,1


          ;!mediasrc with "PATH" includes a list with pointers to all paths, and the path data itself
          ;
          ;  SAMPLE_NUM_PATHS = 3
          ;  SAMPLE_PATH_LIST_LO
          ;    !byte <SAMPLE_PATH_BACKFORTHHORZ
          ;    !byte <SAMPLE_PATH_SQUARE
          ;    !byte <SAMPLE_PATH_CIRCLE
          ;  SAMPLE_PATH_LIST_HI
          ;    !byte >SAMPLE_PATH_BACKFORTHHORZ
          ;    !byte >SAMPLE_PATH_SQUARE
          ;    !byte >SAMPLE_PATH_CIRCLE
          ;  SAMPLE_PATH_BACKFORTHHORZ
          ;    !byte 3,20,0,5,7,20,128,5
          ;  SAMPLE_PATH_SQUARE
          ;    !byte 3,20,5,20,7,20,129,20
          ;  SAMPLE_PATH_CIRCLE
          ;    !byte 3,40,4,40,5,40,6,40,7,40,8,40,1,40,130,40

!mediasrc "paths.pathproject","SAMPLE_","PATH"


