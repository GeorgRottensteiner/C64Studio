* = $2000



!Zone Test

!for j = 0 to 2

  *= $4000+256*j

.s_Main

      !for i = 31 to 0 step -1

        sta $2000+8*i+256*j,y

      !end

      jmp .return_1

.return_1

rts

!end          