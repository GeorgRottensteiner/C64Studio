!to "Lady-Bug.prg",cbm

Pos       = $50
xBank        = $4000*3
Matrix      = $3C00
Farbram     = $D800
Fenster_X     = $C000
Bild        = $FB
Farben_1      = $A000
Farben_2      = $A000
Farben_3      = $A000
Farben_4      = $A000

*=$810

        !FOR Y = 000 TO 800 STEP 80

        ldy Fenster_X

        !FOR X = 00 TO 36 STEP 02

        lda (Bild),y

        sta Bank+Matrix+Pos+X+Y+$00

        lda Farben_1,x
        sta Farbram+Pos+X+Y+$00

        iny

        !END

        lda (Bild),y

        sta Bank+Matrix+Pos+Y+$26

        inc Bild+1

        !END

        ldy Fenster_X

        !FOR X = 00 TO 36 STEP 02

        lda (Bild),y

        sta Bank+Matrix+Pos+X+$370

        inx

        lda Farben_1,x
        sta Farbram+Pos+X+$370

        lda Farben_2,x
        sta Farbram+Pos+X+$370

        lda Farben_3,x
        sta Farbram+Pos+X+$370

        lda Farben_3,x
        sta Farbram+Pos+X+$370
        
        iny

        !END
