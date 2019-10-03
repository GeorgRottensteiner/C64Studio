5 REM Test windowb label name tokenisation
10 REM Demonstrate multi-tasking and sound in a task
20 SIDCLR
30 ALLOCATE 100,0 'memory for task variables
50 TASK1,windowb:PRINT"A line of text"
60 PROCw   'wait a while...
70 PRINT "Another line of text!"
80 GOTO 60 'wait forever!
90 STOP
100 '
110 LABELwindowb
120 REPEAT
130   FOR I=0TO15
140     TPAPER I
150   VOLUME 15
160   FRQ 1,3000
170  ADSR 1,0,12,5,12
180  NOISE 1
190  PASS 0
200  CUTOFF 120
210  RESONANCE 12
220     MUSIC 1,20
230     FOR D = 1 to 750 : NEXT D 'wait a bit longer
240   NEXT I
250 UNTIL FALSE
260 '
270 LABELw:ti$="000000":REPEAT:UNTILti>50:PROCEND
280 '

