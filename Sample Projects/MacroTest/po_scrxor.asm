* = $0801

!BASIC
-   jmp -


* = $2000
        !scr "ABCabc123"

        !scrxor $00, "ABCabc123"
        !scrxor $55, "ABCabc123"
        !scrxor $AA, "ABCabc123"
        !scrxor $FF, "ABCabc123"