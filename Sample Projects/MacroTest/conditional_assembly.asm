* = $2000

SPEED = 0
!IF SPEED{
                      !FOR ZEILE  = 0 TO 880 STEP 40
                         LDA $0401+SPALTE+ZEILE
                         STA $0400+SPALTE+ZEILE
                      !END ZEILE
                     }