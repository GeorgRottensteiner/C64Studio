* = $2000


!for j = 0 to 5

           *=$4000+256*j

.Einsprung   Lda #20

;.Zweisprung  Ldx $2000+8*j

            jsr .Einsprung

            inx

!end
          
          