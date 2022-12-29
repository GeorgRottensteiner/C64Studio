* = $2000

tilesdata_HR = $c000
num_tiles    = 10

START


!set i=0
;!do {
!byte <tilesdata_HR+i*4
!set i = i + 1
; } until i > num_tiles


 !for i=0 to num_tiles
 !byte <tilesdata_HR+i*4
 !end

 END

 ;ell