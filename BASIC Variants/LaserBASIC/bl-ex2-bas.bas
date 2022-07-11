2 REM BASIC Lightning Supplement Manual Example Page 10
5 REM SETA EXAMPLE
10 SETATR 0,0,1: SCLR0, ATR: WINDOW5
20 STRPLOT 0,1,1, "*******************",0
30 STRPLOT 0,1,2, "* BASIC AND WHITE *",0
40 STRPLOT 0,1,3, "* LIGHTNING       *",0
50 STRPLOT 0,1,4, "*******************",0
60 FOR I = 2 to 15 : SETATR 0,1,1
65 X = INT (RND(1)*40)
70 SETA 0,X,1,1,4, ATR
80 NEXT I
90 GOTO 60