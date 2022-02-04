
;!Zone Wurst
;
;*=$c000
;
;w_Mem   =$2000
;
;b_Farbe   =$fc  ; Das gerade aktuelle Farbbyte
;
;  rts
;
;x1          !by 0
;
;x2          !by 0
;
;s_Sub


; library.libary not showing up in outline?

!zone library

.libary = 17
.libary = 18

!set .libary = 20

!message "library.libary",.libary