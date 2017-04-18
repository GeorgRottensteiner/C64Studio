1020 rem -- laufwerk lfw eingeschaltet?
1021 :
1022 poke768,61:open1,lfw,15:close1:sl=0:poke768,139:ifst=0thensl=1
1023 return
1029 :
1030 rem -- befehl an laufwerk lfw
1031 :
1032 open1,lfw,15,bf$:input#1,nr,er$,tr,sc:close1:return
1039 :
1040 rem -- seq-datei lesen
1041 :
1042 close1:t$="0:"+dt$+",s,r":open1,lfw,2,t$:return
1049 :
1050 rem -- seq-datei schreiben
1051 :
1052 close1:t$="@0:"+dt$+",s,w":open1,lfw,2,t$:return
1059 :
1080 rem -- datei auf diskette ?
1081 :
1082 open15,lfw,15
1083 open1,lfw,18,dt$:close1
1084 input#15,nr,er$,tr,sc
1085 ifnr<>0thenset=1
1086 ifnr=0thenset=0
1087 printer$:input#15,nr,er$,tr,sc:close1:close15:printer$
1088 ifnr=31 then print"Da stimmt was nicht !"
1089 return
1100 rem -- seq-datei anlegen
1101 :
1102 input"Name der SEQ-Datei ";dt$
1103 gosub1050
1104 print"_ fuer Ende"
1105 inputy$:ify$="_"then1107
1106 goto1105
1107 close1:return
1109 :
