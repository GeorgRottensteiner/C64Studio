	  	!byte 1,2,3  ;tab, space tab
			!byte 4,5,6		;tab,tab,tab
      !byte 7,8,9   ;space * 6
  	  !byte a,b,c		;space, tab, space


.gnu
			!byte 1,2,3
			!byte 1,2,3
			!byte 1,2,3
			!byte 1,2,3
			!byte 1,2,3
			!byte 1,2,3
.lsmf


.data		;!byte 1,2,3,4
;				!byte 5,6,7,8



.
			!byte 1,2,3
			!byte 1,2,3
			!byte 1,2,3
		gnu	!byte 1,2,3
			!byte 1,2,3
			!byte 1,2,3
.lsmf


* = $0801

					lda #4 + (3 * 20)
					sta $d800
					rts