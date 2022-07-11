


*=$0801
 ldx #0
.loop
 dex
 bne .loop
 jsr nurEinTest
 rts

 

 !zone nurEinTest
   
   
   
nurEinTest
 ldx #0
.loop
 dex
 bne .loop
 rts