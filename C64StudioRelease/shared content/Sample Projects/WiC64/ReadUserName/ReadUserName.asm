;Sample for using the WiC64 hardware
;
; Sends the command $26 (read the logged in user name) and displays it on screen
; The first byte of the result tells whether the user was logged in ($00), or not ($01)
; If the user was not logged in the returned name is "unknown user"
; The user name is in upper/lower case!

* = $0801

!basic
          ldx #<READ_LOGGED_IN_USER_NAME
          ldy #>READ_LOGGED_IN_USER_NAME
          jsr WiC64.SendCommand

          ;set read mode and read length bytes
          jsr WiC64.PrepareForRead
          bcs .TimedOut

          ;length 0000 means no reply received
          ;error checking
          cpy #$00

          ;Antwort groesser als $01XX bytes
          bne .HadContent

          cpx #$00
          bne .HadContent

          ;Antwort ist genau null Byte 00
          ; 00 = Keine Antwort verfügbar.
          rts



.HadContent
          ;first byte is success (0), or failure if not logged in ($ff)
          jsr WiC64.ReadByte
          sta IS_LOGGED_IN

          ;display output on screen
.OutputName

          jsr WiC64.ReadByte
          sta $5000,x

          jsr KERNAL.CHROUT
          dex
          bne .OutputName
          dey
          cpy #$ff
          bne .OutputName

          ;toggle to lowercase
          lda #14
          jsr KERNAL.CHROUT

          rts

IS_LOGGED_IN
          !byte 0


.TimedOut
          ;display error message
          ldx #0
-
          lda TIMED_OUT_MESSAGE,x
          beq .Done

          jsr KERNAL.CHROUT
          inx
          jmp -

.Done
          rts


READ_LOGGED_IN_USER_NAME
          !text "W",38,$00,$0f,"https://wic64.net/gun.php?mac=%mac"


TIMED_OUT_MESSAGE
          !pet "read user name timed out, check wic64 state",0

!source <wic64.asm>