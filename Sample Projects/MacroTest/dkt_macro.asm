* = $0801

stab_irq_line = $30

!basic

ekran1 = $0400


!macro IRQ_SETUP irq_proc, irq_line
          lda #<(irq_line)
          sta $d012
          lda $d011
          and #$7f
          ora #(>(irq_line) << 7 )
          sta $d011
          lda #<irq_proc
          sta $fffe
          lda #>irq_proc
          sta $ffff
!end          
        



!macro FILL_MEM add, pages, val
            lda #<add
            sta $02
            lda #>add
            sta $03
            ldy #0
            lda #val
            ldx #pages
-           sta ($02),y
            iny
            bne -
            inc $03
            dex
            bne -
!end

!macro SYNC
            lda $d011
            bpl *-3
            lda $d011
            bmi *-3
!end

          +IRQ_SETUP stab_irq, stab_irq_line
          
stab_irq          

            sei
            +SYNC
            lda #$00
            sta $d011
            
            +FILL_MEM ekran1, 4, $1b
            
            
            
.Loop
            jmp .Loop
            
            
            