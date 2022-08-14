;Sample for using the WiC64 hardware
;
; Sends the command $15 (fetch current data/time from NTP) and displays the reply on screen

* = $0801

!basic
          ldx #<FETCH_TIME_COMMAND
          ldy #>FETCH_TIME_COMMAND
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
          ;display output on screen
          jsr WiC64.ReadByte
          jsr KERNAL.CHROUT
          dex
          bne .HadContent
          dey
          cpy #$ff
          bne .HadContent
          rts


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


FETCH_TIME_COMMAND
          !byte 'W'         ;'W' for WiC64
          !word $0004       ;total length of command
          !byte $15         ;command for reading IP

TIMED_OUT_MESSAGE
          !pet "fetch timed out, check wic64 state",0

!source <wic64.asm>