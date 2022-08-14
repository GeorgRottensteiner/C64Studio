;Sample for using the WiC64 hardware
;
; Sends the command $01 (HTTP GET request) and displays it on screen

* = $0801

!basic
          ldx #<HTTP_GET_COMMAND
          ldy #>HTTP_GET_COMMAND
          jsr WiC64.SendCommand

          ;set read mode
          jsr WiC64.PrepareForRead
          bcs .TimedOut

          ;length byte is in x/y (lo/hi)

          ;length 0000 means no reply received
          ;error checking
          cpy #$00

          ;Antwort groesser als $01XX bytes
          bne .HadContent

          cpx #$00
          bne .HadContent

          ;Antwort ist genau null Byte 00
          ; 00 = Keine Antwort verfügbar.
          jmp .TimedOut



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


HTTP_GET_COMMAND
          !byte 'W'                                       ;'W' for WiC64
          !word HTTP_GET_COMMAND_END - HTTP_GET_COMMAND   ;total length of command
          !byte $01                                       ;command HTTP GET from URL
          !text "http://wic64.georg-rottensteiner.de/httpgetsample.txt"
HTTP_GET_COMMAND_END

          !byte 'W'         ;'W' for WiC64
          !word $0004       ;total length of command
          !byte $01         ;command HTTP GET from URL
          !byte "http://www.georg-rottensteiner.de/test/haus1.html"

TIMED_OUT_MESSAGE
          !pet "get ip timed out, check wic64 state",0

!source <wic64.asm>