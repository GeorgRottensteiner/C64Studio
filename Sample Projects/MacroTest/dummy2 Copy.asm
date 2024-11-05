TOKEN_REM = $8F

*=$0801
 !basic 0,58,TOKEN_REM,20,20,20,20,"LSMF",start
start:
 jsr test
 rts



test:
 rts
 
 
  TOKEN_SYS = $9E ; Could be included from external file
  
  !basic lsmf, ":", TOKEN_SYS, " 64738:", TOKEN_REM, " *** AUTO-RESET ***", .mainStart
  
  lsmf = 13
   


  
  .mainStart