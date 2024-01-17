!source <c64.asm>

!zone WiC64 {

.ZEROPAGE_POINTER_1 = $fe



;Sends a command, max. supported length 256 bytes!
;x = lo byte of command
;y = hi byte of command
;returns with carry set in case of an error
.SendCommand
          stx .ZEROPAGE_POINTER_1
          sty .ZEROPAGE_POINTER_1 + 1

          ;length of command
          ldy #2
          lda (.ZEROPAGE_POINTER_1),y
          clc
          adc #4
          tax

          jsr WiC64.PrepareForWrite

          ldy #0
-
          lda (.ZEROPAGE_POINTER_1),y
          jsr WiC64.WriteByte
          bcs .TimedOut

          iny
          dex
          bne -

.TimedOut
          rts



;write byte in A to WiC64
;requires set write mode
;if no WiC64 is connected, times out or any other error occurs, carry is set
;if carry is clear, result byte is in A
.WriteByte
          ; bits 0..7 parallel to WiC64 (userport PB 0-7)
          sta CIA2.DATA_PORT_B
          jmp .WaitHandshake


;read byte from WiC64 into A
;if no WiC64 is connected, times out or any other error occurs, carry is set
;if carry is clear, result byte is in A
.ReadByte
          jsr .WaitHandshake
          ; bits 0..7 parallel to WiC64 (userport PB 0-7)
          lda CIA2.DATA_PORT_B
          rts

.WaitHandshake
          lda #$68              ; short timeout
          sta .OuterLoopCount   ; looplength for timeout
          lda #$0
          sta .InnerLoopCount

-
          ;check handshake, wait for NMI FLAG2
          lda CIA2.NMI_CONTROL
          and #$10
          bne .HandshakeOK

          dec .InnerLoopCount      ; inner loop: 256 passes
          bne -
          dec .OuterLoopCount
          bne -

          sec
          rts

.HandshakeOK
          clc
          rts



.InnerLoopCount
          !byte 0
.OuterLoopCount
          !byte 0



;prepare WiC64 for receiving a command
.PrepareForWrite
          ;Datenrichtung Port A, Leitung PA2 auf Ausgang
          lda CIA2.DATA_DIRECTION_REGISTER_A
          ora #$04
          sta CIA2.DATA_DIRECTION_REGISTER_A

          ;PA2 auf HIGH = ESP im Empfangsmodus
          ;= WiC64 ready for reading data from C64
          lda CIA2.DATA_PORT_A
          ora #$04
          sta CIA2.DATA_PORT_A

          ;Datenrichtung Port B Ausgang
          lda #$ff
          sta CIA2.DATA_DIRECTION_REGISTER_B

          ;reset flag2 handshake
          lda CIA2.NMI_CONTROL
          rts



;prepare WiC64 for reading the result of a command
;if carry is clear, result byte is in A
;                   received data length lo byte in X
;                   received data length hi byte in Y
.PrepareForRead
          lda #$00          ;Datenrichtung Port B Eingang
          sta CIA2.DATA_DIRECTION_REGISTER_B
          lda CIA2.DATA_PORT_A
          and #251          ;PA2 auf LOW = ESP im Sendemodus
          sta CIA2.DATA_PORT_A


          ;Dummy Byte - IRQ anschieben
          jsr WiC64.ReadByte
          bcs .Error

          ;data length hi byte
          jsr WiC64.ReadByte
          tay

          ;data length lo byte
          jsr WiC64.ReadByte
          tax

.Error
          rts

}

