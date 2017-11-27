!to "macro_macro.prg",cbm

;* = $1000
;MACRO_START

!macro fill5bytes v1,v2,v3,v4,v5
          lda #v1
          sta 1024
          lda #v2
          sta 1025
          lda #v3
          sta 1026
          lda #v4
          sta 1027
          lda #v5
          sta 1028
!end



;MACRO_END

;!if ( MACRO_START != MACRO_END ) {
;!error Macro has size!
;}

* = $2000
          lda #$01
          sta 53281
CALLEDLED_MACRO
          +fill5bytes 10,20,30,40,50
CALLEDLED_MACRO_END
          inc 53280
          +fill5bytes 1,2,3,4,5

          rts