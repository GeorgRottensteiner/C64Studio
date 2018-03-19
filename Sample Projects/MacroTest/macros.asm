  ; ===============================
  ; Makrodefinitionen
  ; ===============================

  ; lädt einen 16-Bit-Wert in .A und .X
  ; Lo-Byte landet in .A
  ; Hi-Byte landet in .X
  !macro ldax16 value
    lda #<value
    ldx #>value
  !end

  ; schreibt einen 16-Bit-Wert in .A und .X an die angegebene Adresse
  ; Lo-Byte in .A wird in address geschrieben
  ; Hi-Byte in .X wird in address + 1 geschrieben
  !macro stax16 address
    sta address
    stx address + 1
  !end

  ; "POKE 16-Bit"
  ; setzt in ptrAddress (und ptrAddress + 1) den 16-Bit-Wert in address
  ; ändert nur .A
  !macro setPtrA address, ptrAddress
    lda #<address
    sta ptrAddress
    lda #>address
    sta ptrAddress + 1
  !end



