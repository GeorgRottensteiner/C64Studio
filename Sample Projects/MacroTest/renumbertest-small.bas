#RetroDevStudio.MetaData.BASIC:2049,BASIC V2,lowercase,10,10
10 ?"HALLO, PRESS G TO WIN"
20 GETA$:IFA$=""THEN 20
30 IFA$="G"THEN 50
40 ?"ENDE":END
50 ?"YOU WIN!"
60 P=(A$="")*-1+(A$="")
70 PRINT"      NOT A SPACE !!!           "
80 ONXGOSUB10,20,30
90 IFA=1THEN 90
