!cpu m65

IRQ_HANDLER_ADDRESS = $1600

* = $2001
!basic
          ;keep old address
          lda $0314
          sta IRQ_ROUTINE_END
          lda $0315
          sta IRQ_ROUTINE_END + 1

          ;copy handler to address below $2000 (which is always mapped in)
          ldx #0
-
          lda IRQ_HANDLER,x
          sta IRQ_HANDLER_ADDRESS,x
          inx
          cpx #IRQ_HANDLER_END - IRQ_HANDLER
          bne -

          ;install our hook
          sei
          lda #<IRQ_HANDLER_ADDRESS
          sta $0314
          lda #>IRQ_HANDLER_ADDRESS
          sta $0315
          cli

          jmp *



IRQ_HANDLER
!pseudopc IRQ_HANDLER_ADDRESS
          inc $d020
          inc $0800
          jmp (IRQ_ROUTINE_END)

IRQ_ROUTINE_END
          !word 0

!realpc

IRQ_HANDLER_END
