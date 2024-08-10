* = $0801

!fill 1000

.Data
!fill 20 * 11

.DataRows_Lo
!fill 11, [<.Data + i * 20]





* = $2000

;40 * 1,2,3,1,2,3,...
!fill 40,[1,2,3]

PI = 3.14159265359
!fill 256,math.sin(( i * PI * 2)*360/(2*PI)/256) * 127+127
;!fill 256, math.sin( i * 360 / 256 )*4



