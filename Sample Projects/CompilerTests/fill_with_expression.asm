* = $0801

HURZ = math.min( 20, 3 * 5 )
HURZ2 = math.max( 20, 3 * 5 )

LSMF = 5 * math.sin( 20 )

SINE_TABLE
          ;!fill 8,[math.sin( i * 360 / 8 )]

 !basic 2018,":",$8f," BY RGCD",START_ADDRESS

START_ADDRESS 

          ldx #0
-          
          lda CHAR_TABLE,x
          sta $0400,x
          
          inx
          cpx #5
          bne -
          
;-
;jmp -          
          rts



CHAR_TABLE
          !fill 5,[i * 8]+[i*8]
          
