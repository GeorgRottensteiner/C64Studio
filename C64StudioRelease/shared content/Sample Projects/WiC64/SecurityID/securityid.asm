* = $0801

!basic
          jsr CallSIDPHP
          rts



!lzone CallSIDPHP
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
          ;read output so it's gone
          jsr WiC64.ReadByte
          jsr KERNAL.CHROUT

          dex
          bne .HadContent
          dey
          cpy #$ff
          bne .HadContent

.TimedOut
          ;here we don't care whether it was successful
          rts


HTTP_GET_COMMAND
          !byte 'W'                                       ;'W' for WiC64
          !word HTTP_GET_COMMAND_END - HTTP_GET_COMMAND   ;total length of command
          !byte $01                                       ;command HTTP GET from URL
          !text "https://wic64.net/sid.php"
HTTP_GET_COMMAND_END



!src <wic64.asm>