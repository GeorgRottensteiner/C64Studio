10 POKE53280,0:POKE53281,11:SC=0:C$=" {blk}{CBM-D}{lblu}{CBM-P}{blk}{CBM-F} {down}{left}{left}{left}{left}{left} {lblu}{CBM-R}{rvon}{Shift-C}{rvof}{CBM-R} {down}{left}{left}{left}{left}{left} {blk}{rvon} {rvof}{lblu}{CBM-U}{blk}{rvon} {rvof} {up}{up}{left}{left}{left}{left}{left}"
20 X=18:FORI=0TO62STEP3:POKE832+I,0:POKE833+I,255:POKE834+I,0:NEXT:V=53248:POKE2040,13
30 POKEV,172:POKEV+1,0:POKEV+21,1:POKEV+28,0:POKEV+39,7:POKEV+27,1:C=172:D=0:PRINT"{blk}{clr}";
40 FORI=0TO960STEP40:POKE1024+I,66:POKE1054+I,66:NEXT
50 PRINT"{home}{down}"SPC(32)"{lblu}SCORE:{down}{left}{left}{left}{left}{left}{left}{left}{wht} ";SC;:PRINT"{home}{down}{down}{down}{down}{down}{down}{down}{down}{down}{DoWn}{down}{down}{down}{down}{down}{down}"SPC(X)C$;
60 J=PEEK(56320):POKEV+1,D:D=D+10:IFD>255THENPRINT"{clr}GAME OVER, SCORE:"SC:POKEV+21,0:END
70 IFPEEK(V+31)>0THEND=0:SC=SC+1:POKEV+1,0:POKEV+31,0:C=RND(1)*224+24:POKEV,C:GOTO50
80 IFX>1AND(JAND4)=0THENX=X-1:PRINT"{left}"C$;:GOTO60
90 IFX<25AND(JAND8)=0THENX=X+1:PRINT"{right}"C$;:GOTO60
100 GOTO60
