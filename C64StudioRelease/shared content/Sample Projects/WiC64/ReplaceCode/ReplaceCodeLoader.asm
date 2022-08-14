;Sample for using the WiC64 hardware
;
; Uses the command $01 (HTTP GET request) to load a piece of code and runs it

* = $0801

!basic
          ;display info text
          ldx #0
-
          lda INFO_TEXT,x
          beq .DisplayDone

          jsr KERNAL.CHROUT

          inx
          jmp -

.DisplayDone


MainLoop
          ; Keyboard input
          jsr KERNAL.GETIN
          beq MainLoop

          cmp #'1'
          bcc +

          cmp #'4'
          bcs MainLoop

          sta HTTP_CODEPART_DIGIT

          ldx #<HTTP_GET_COMMAND
          ldy #>HTTP_GET_COMMAND
          jsr WiC64.SendCommand

          ;set read mode
          jsr WiC64.PrepareForRead
          bcs .TimedOut

          ;length byte is in x/y (lo/hi)
          stx $c000
          sty $c001

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
          lda #<$2000
          sta .TargetAddress
          lda #>$2000
          sta .TargetAddress + 1

.FetchNextByte
          jsr WiC64.ReadByte

          ;jsr KERNAL.CHROUT

.TargetAddress = * + 1
          sta $ffff

          inc .TargetAddress
          bne +
          inc .TargetAddress + 1

+
          dex
          bne .FetchNextByte

          dey
          cpy #$ff
          bne .FetchNextByte

          ;completed download
          jsr $2000

          jmp MainLoop


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
          !text "http://wic64.georg-rottensteiner.de/codepart"
HTTP_CODEPART_DIGIT
          !text 'x'
          !text ".bin"
HTTP_GET_COMMAND_END

TIMED_OUT_MESSAGE
          !pet "get ip timed out, check wic64 state",$0d,0

INFO_TEXT
          !pet "press 1 to 3 to switch between parts",0

!source <wic64.asm>