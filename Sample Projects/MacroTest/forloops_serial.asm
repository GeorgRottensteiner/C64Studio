* = $1000

data_size = 0

start = *
!for i = -256 to 254
    !byte <((i*i)/4)
!end
data_size += * - start

gnu

;!for i = -256 to 254
;    !byte >((i*i)/4)
;!end
;
;
;!for i = -256 to 254
;    !byte >((i*i)/4)
;!end
;
;
;
;!for i = -256 to 254
;    !byte >((i*i)/4)
;!end
