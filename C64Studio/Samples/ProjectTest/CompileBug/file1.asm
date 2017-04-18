!to "test.prg",cbm

buff = $6000

*=$2000

        lda #0
        sta 53280
        
        lda #>buff
        sta 1029
        
        lda #<( buff + $20 )
        sta 1028
        
        ;soll $20
        lda #<buff + $20
        sta 1024
        ;soll $80
        lda #>buff + $20
        sta 1025
        ;soll $20
        lda #$20 + <buff
        sta 1026
        ;soll $80
        lda #$20 + >buff
        sta 1027
        
        rts
        
;!bin "lsmf.bin",5000,0 