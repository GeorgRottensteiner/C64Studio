10 REM lb-ex8-bas.PRG
20 REM Example of tokenising issue
30 REM when Laser BASIC commands are
40 REM used in procedure label names.
50 REM C64 Studio compiles this ok
60 REM but it does not run in LB
70 REM until lines containing procedure
80 REM labels are re-tokenised...
90 REM i.e. lines... 210, 240, 280, 330
100 REM         370, 410, 440, 490, 530
110 REM
120 ALLOCATE 100,0
130 PROCintro
140 PROCplot2
150 PROCwindow:PROCmulti
160 PROCtext:PROCdisem
170 PROCscroll1
180 PROCplot3
190 STOP
200 '
210 LABEL scroll1
220 PROCw:PROCEND
230 '
240 LABELwindow
250 TASK1,windowb:PRINT"ABC"
260 PROCEND
270 '
280 LABELwindowb
290 REPEAT
300     FOR I = 1 TO 100 : NEXT I
310 UNTIL FALSE
320 '
330 LABELplot2
340 PROCw
350 PROCEND
360 '
370 LABELtext
380 PROCw
390 PROCEND
400 '
410 LABELintro:PROCw
420 PROCEND
430 '
440 LABELplot3
450 PROCw:PROCEND
460 '
470 LABELw:ti$="000000":REPEAT:UNTILti>10:PROCEND
480 '
490 LABEL disem
510 PROCw:PROCEND
520 '
530 LABEL multi
540 PROC w
550 PROCEND
