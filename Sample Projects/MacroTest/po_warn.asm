!to "po_text_pet_scr.prg",cbm

SpriteData2 = $0840

* = $0801
CODE_START
!BASIC TEST_LABEL
;-   jmp -


* = SpriteData2
CODE_END

!if ( CODE_END - CODE_START ) > 2048 {

!warn "Oh no! ", CODE_END - CODE_START, " bytes!"
}

* = 10000
TEST_LABEL