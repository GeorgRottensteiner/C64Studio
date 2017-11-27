* = $2000


SCREEN = $0400


          lda (SCREEN)
          sta (SCREEN)
          
          jsr (TEST)
          
          lda #1
          bne (THEEND)
          
THEEND          
TEST          
          rts
          