!to "nested_macro_loop.prg",cbm

* = $2000

!macro mov_2sprite src, dst
 	!for x = 0 to 21 step 1
 		lda src+x
 		sta dst+x*3
 	!end
 !end


 +mov_2sprite DATA_SOURCE, DATA_DEST
          rts


 DATA_SOURCE
          !fill 22,0

 DATA_DEST
          !fill 3 * 22,0
 