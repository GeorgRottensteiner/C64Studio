*=$0801

 !basic

 TEST = $0818

 ;lda #$0818   ;Fehler

 ;lda #TEST    ;Fehler

 ;lda #msg     ;kein Fehler  <---
 lda #7

 rts

 ;...

msg

 !text "..."