!to "po_text_pet_scr.prg",cbm

* = $0801
!BASIC
-   jmp -

        lda #"{SHIFT-A}"


* = $2000
        !text "ABCabc123"

* = $3000
        !pet "ABCabc123"

* = $4000
        !scr "ABCabc123"


* = $5000
        !raw "ABCabc123"