* = $2000
          lda $64
          sta .ReadPos
          lda $65
          sta .ReadPos + 1
          
          
          ldx #0
-          
.ReadPos = * + 1
          lda $ffff,x
          beq .Done
          
          sta $0400,x
          
          inx
          jmp -
          
          
.Done
          lda #0
          sta $000d
          
          ;TODO - store result in $61–$66
          rts
          
          