* = $2000
          ;lda $2000000

*=$4000:    !bin "testscreen.bin",,2          
          
c_px=8
!macro setlock {
  lda #$4d
.lock sta .lock 
} ;!end
!macro setflag flag {
  sec
  ror flag
} ;!end
 *=$1000
; Erzeugt  "Could not evaluate expression: (>(s_ausgleich) <> >(j_test))"
;!if (>(s_ausgleich) <> >(j_test)) {
    !Message "Pageüberschreitung in der Ausgleichsroutine"
;}
; Erzeugt  "Missing closing bracket"
;!if c_px=8 { ldy j_test,x : sty 1024}
; Ohne Leerzeichen ergibt das mal "Syntax Error :$4d", mal "Redefinition of label lda#"
 lda#$4d
 lda #$4d
   +setlock
; Could not evaluate setflag_6_30_flag1
; Could not evaluate setflag_6_27_flag1
+setflag flag1 
+setflag flag1 
;---------------------------------------------
; Redefinition of label ror1
; Und ror1 wird sogar als Vorschlag bei der Eingabe angezeigt!
+setflag 1
+setflag 1
j_test dec 53280:rts
s_ausgleich
flag1  !by 0
          
          
;---------------------------------------------
; Redefinition of label flop. 
; Passt ja, wird aber in Line 0 gemeldet und nicht da, wo definiert wurde
;flop  =$c000
;  flop  !by 0
;---------------------------------------------
; Makraufruf nach einer Labeldefinition meldet im Macro einen ganz falschen Parameter
; Oder hier schlimmer: .Net-Absturz
Tralafutti  +setflag 1
          


; Meldet, dass .ende unbenutzt wäre
!macro irgendwas flag 
        !by flag
        bne .ende
.ende
!end
        +irgendwas 12
        +irgendwas 13
