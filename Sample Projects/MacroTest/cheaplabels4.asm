* = $0801
   !BASIC


Zone1
  @CheapLabel
        lda #15

        ;directly reference cheap label
        jmp @CheapLabel

Zone2
        ;allow the same label name, overriding the existing label
  @CheapLabel

        ;you cannot reference @CheapLabel from Zone1 anymore
        jmp @CheapLabel