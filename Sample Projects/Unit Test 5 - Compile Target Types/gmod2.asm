!to "gmod2.crt",gmod2crt
;!to "gmod2.bin",gmod2bin


* = $8000

        !word launcher
        !word launcher
        !byte $c3 ;c
        !byte $c2 ;b
        !byte $cd ;m
        !byte $38 ;8
        !byte $30 ;0

launcher:
        sei
        stx $d016
        jsr $fda3 ;prepare irq
        jsr $fd50 ;init memory
        jsr $fd15 ;init i/o
        jsr $ff5b ;init video

        cli
-        
        inc $d020
        jmp -
