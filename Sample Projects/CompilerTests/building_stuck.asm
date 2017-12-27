!to "test.prg", cbm


 * = $0801



 !macro Set16 .adr, .value {
 lda #<.value
 sta .adr
 lda #>.value
 sta .adr+1
 }




 +Set16 .screen+1, $4800
 
 .screen