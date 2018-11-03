*=$0801
 !basic 0,58,$8f,20,20,20,20,"LSMF",start
start:
 jsr test
 rts



test:
 rts
 
 
  TOKEN_SYS = $9E ; Could be included from external file
  TOKEN_REM = $8F
  !basic lsmf, ":", TOKEN_SYS, " 64738:", TOKEN_REM, " *** AUTO-RESET ***", .mainStart
  
  lsmf = 13
   


  
  .mainStart