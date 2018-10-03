* = $2000


!for j = 0 to 15

           *=$4000+256*j

.Einsprung   Lda #20

.Zweisprung  Ldx $2000+8*j

            inx

!end
          
          