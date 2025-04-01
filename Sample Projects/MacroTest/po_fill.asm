* = $0801

!fill 1000

.Data
!fill 20 * 11

POS_1
!fill 11, [>.Data + i * 20]
POS_2
!fill 11, [>( .Data + i * 20 )]



.DataRows_Lo
;!fill 11, [<.Data + i * 20]


RANGE
!fill 4, 12 to 40

RANGE2
!fill 6, 255 to 128



* = $2000

;40 * 1,2,3,1,2,3,...
!fill 40,[1,2,3]

PI = 3.14159265359
!fill 256,math.sin(( i * PI * 2)*360/(2*PI)/256) * 127+127
;!fill 256, math.sin( i * 360 / 256 )*4

HURZ = math.sqrt( 144 )



		;lda PLAYER.y
		;+PrintValue 0, 22, 3

    ;lda Teleport.Y
		;+PrintValue 4, 22, 3