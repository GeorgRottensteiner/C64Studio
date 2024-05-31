*=$0801

!bin "asm_nops.prg",,2   ; das eigene TSB-Programm (PRG-Format, Namen hier anpassen!)

*=$7fd0
  lda #$60    ; RTS-Befehl für vorzeitigen Ausstieg aus TSB-Init einpatchen
  sta $80bf
  jsr $8055   ; TSB Initialisieren
  lda #$20
  sta $80bf   ; Init wiederherstellen

  jsr $a659   ; Basic-Zeiger setzen und CLR

  sei
  lda #$4c    ; TSB-IRQ aktivieren
  sta $0314
  lda #$84
  sta $0315

  jmp $80d2   ; und dann in die TSB-Interpreterschleife springen (RUN)

*=$8000

!bin "asm_nops.prg",20480,2    ; TSB-Kernel einbinden