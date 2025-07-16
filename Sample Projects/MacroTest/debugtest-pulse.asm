!to "fillscreen.prg",cbm
*= $0801
        !word next
        !word 2025 
        !byte $9E  
        !text "2061" 
        !byte 0    
next
        !word 0   
        sei             ; maskierte IRQs aus
        lda #$7F
        sta $dc0d       ; beide Timer IRQ aus
        sta $dd0d   
        lda $dc0d       ; Durch lesen wartende CIA IRQ negieren
        lda $dd0d       ; sonst können sie nach setzen der IRQ noch auftreten
        lda #$35        ; BASIC und KERNAL aus
        sta $01
        cli             ; IRQs an
vsync  
        lda #100
        cmp $d012
        bne vsync
        
        ; Bildschirm fuellen
        ; 1000(3E8) Zeichen = 4 * 250($FA)
selfmod
        lda #20
        ldx #$FA
clear
        sta $0400-1,x
        sta $0400-1 + $FA, x
        sta $0400-1 + (2 * $FA), x
        sta $0400-1 + (3 * $FA), x
        dex
        bne clear
        inc selfmod+1
        
        jmp vsync