5 REM FILTER EXAMPLE BL Manual Pg 31
6 REM COMMENTED OUT FILTER TO MAKE
7 REM EXAMPLE WORK In LASER BASIC
10 VOLUME 15
20 FRQ 1,3000
30 ADSR 1,0,12,5,12
40 NOISE 1
50 REM FILTER 1,TRUE
60 PASS 0
70 CUTOFF 120
80 RESONANCE 12
90 MUSIC 1,20
