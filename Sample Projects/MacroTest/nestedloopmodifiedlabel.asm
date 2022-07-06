* = $2000

SCREEN_CHAR = $0400

xmax=1
!for y=3 to 0 step -1
!for x=1 to xmax step 1
   lda SCREEN_CHAR+x+y*40
   sta SCREEN_CHAR+x-1+(y+1)*40
!end
xmax=xmax+1
!end