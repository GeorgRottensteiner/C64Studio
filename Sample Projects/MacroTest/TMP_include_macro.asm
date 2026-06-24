.include "TMP_macro.asm"

; comment

        *=$2000

draw    #drawCharacterAt $51,$D,$06,$F
        #drawCharacterAt $58,$E,$06,$E
        #drawCharacterAt $5A,$F,$06,$D
        rts