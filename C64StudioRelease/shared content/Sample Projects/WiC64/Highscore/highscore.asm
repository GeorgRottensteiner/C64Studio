;this game id must be registered/received from the WiC64 team!
WiC64GameID = 99

* = $0801
!basic
          ;read highscore
          ldx #<WIC64_COMMAND_READ_HIGHSCORE_LIST
          ldy #>WIC64_COMMAND_READ_HIGHSCORE_LIST
          jsr WiC64.SendCommand

          jmp ReadAndDisplayResponse

          ;set level (optional, keep it zero if not used)
          lda #5
          sta SendScoreAndLevel.SEND_LEVEL_POS

          ;set score (3 bytes)
          lda #$56
          sta SendScoreAndLevel.SEND_SCORE_LO
          lda #$34
          sta SendScoreAndLevel.SEND_SCORE_MID
          lda #$12
          sta SendScoreAndLevel.SEND_SCORE_HI

          jsr SendScoreAndLevel
          rts


!lzone SendScoreAndLevel
          ;set up first call (f=d) -> resets the security ID
          lda #'d'
          sta .FUNCTION_ID

          jsr .PerformSend
          ;we ignore the result code here

          ;set up second call (f=s) -> actual transfer of score data
          lda #'s'
          sta .FUNCTION_ID

.PerformSend
          ldx #<WIC64_COMMAND
          ldy #>WIC64_COMMAND
          jsr WiC64.SendCommand

          jmp ReadAndDisplayResponse


WIC64_COMMAND
          !byte 'W'                                 ;'W' for WiC64
          !word WIC64_COMMAND_END - WIC64_COMMAND   ;total length of command
          !text $0F                                 ;command HTTP STRING CONVERSION FOR HTTP CHAT
          !text "https://wic64.net/hc.php?f="
.FUNCTION_ID = *
          !text "s"
          !text "&1=%mac&2=<$",$05,$00
          !text WiC64GameID
.SEND_LEVEL_POS
          !byte $00
.SEND_SCORE_HI
          !byte $00
.SEND_SCORE_MID
          !byte $00
.SEND_SCORE_LO
          !byte $00

WIC64_COMMAND_END



!lzone ReadAndDisplayResponse
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

          rts


.TimedOut
          ;here we don't care whether it was successful
          rts




WIC64_COMMAND_READ_HIGHSCORE_LIST
          !byte 'W'                                 ;'W' for WiC64
          !word WIC64_COMMAND_READ_HIGHSCORE_LIST_END - WIC64_COMMAND_READ_HIGHSCORE_LIST   ;total length of command
          !text $0F                                 ;command HTTP STRING CONVERSION FOR HTTP CHAT
          !text "https://wic64.net/hc.php?f=g&1=99"
WIC64_COMMAND_READ_HIGHSCORE_LIST_END




!src <wic64.asm>