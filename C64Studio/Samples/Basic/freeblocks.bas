1050 rem *** freie blocks auf disk
1051 :
1055 o=8
1059 open15,o,15,"i"
1060 print#15,"m-r"chr$(250)chr$(2)chr$(1):get#15,bl$:bl=asc(bl$+chr$(0))
1070 print#15,"m-r"chr$(252)chr$(2)chr$(1):get#15,bh$:bh=asc(bh$+chr$(0))
1080 close15:fb=bl+256*bh:return
