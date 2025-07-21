!to "po_text_pet_scr.prg",cbm

VERSION = "1.8"

* = $0801
!BASIC
-   jmp -

        lda #VERSION 


* = $2000
        !text "ABCabc123",VERSION

* = $3000
        !pet "ABCabc123",VERSION

* = $4000
        !scr "ABCabc123",VERSION


* = $5000
        !raw "ABCabc123",VERSION