!to "po_text_pet_scr.prg",cbm

* = $0801
CODE_START
!BASIC
-   jmp -


* = $2000
CODE_END

!if ( CODE_END - CODE_START ) > 2048 {

!warn "Oh no! ", CODE_END - CODE_START, " bytes!"
}