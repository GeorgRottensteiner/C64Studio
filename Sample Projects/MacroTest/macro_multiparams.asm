* = $c000
                              !to "test.prg", cbm
                              ; ===============================
                              ; Makrodefinitionen
                              ; ===============================

                              ; lädt einen 16-Bit-Wert in .A und .X
                              ; Lo-Byte landet in .A
                              ; Hi-Byte landet in .X
                              !macro ldax16 value {
                                lda #<value
                                ldx #>value
                              }

                              ; schreibt einen 16-Bit-Wert in .A und .X an die angegebene Adresse
                              ; Lo-Byte in .A wird in address geschrieben
                              ; Hi-Byte in .X wird in address + 1 geschrieben
                              !macro stax16 address {
                                sta address
                                stx address + 1
                              }

                              ; ""POKE 16-Bit""
                              ; setzt in ptrAddress (und ptrAddress + 1) den 16-Bit-Wert in address
                              ; ändert nur .A
                              !macro setPtrA address, ptrAddress {
                                lda #<address
                                sta ptrAddress
                                lda #>address
                                sta ptrAddress + 1
                              }

                              offset_2 =  2
                              offset_8 =  8
                              offset40 = 40
                              zptrMemDest1 = $a7
                              testAdressFix = $8000
                              testAdressPreCalc = .testLongNamedAdr + 2 + offset_2

                              ; ===============================
                              ; Makros mit einem Parameter
                              ; ===============================

                              ; -------------------------------
                              ; ohne Arithmetik ($c000)
                              ; -------------------------------

                              ; feste Adresse, Zeropage
                              +ldax16 zptrMemDest1                            ; OK
                              ;+stax16 zptrMemDest1                            ; OK

                              ; feste Adresse (1), 16 bit
                              +ldax16 testAdressFix                           ; OK
                              ;+stax16 testAdressFix                           ; OK

                              ; feste Adresse (2), 16 bit
                              +ldax16 testAdressPreCalc                       ; OK
                              ;+stax16 testAdressPreCalc                       ; OK

                              ; dynamische Adresse, 16 bit
                              +ldax16 .testLongNamedAdr                       ; OK
                              ;+stax16 .testLongNamedAdr                       ; OK

                              ; -------------------------------
                              ; mit Arithmetik, ohne Klammern ($c026)
                              ; -------------------------------

                              ; feste Adresse, Zeropage
                              +ldax16 zptrMemDest1 + 2                        ; OK
                              ;+stax16 zptrMemDest1 + offset_2                 ; OK

                              ; feste Adresse (1), 16 bit
                              +ldax16 testAdressFix + 2                       ; OK
                              ;+stax16 testAdressFix + offset_2                ; OK

                              ; feste Adresse (2), 16 bit
                              +ldax16 testAdressPreCalc + 2                   ; OK
                              ;+stax16 testAdressPreCalc + offset_2            ; OK

                              ; dynamische Adresse, 16 bit
                              +ldax16 .testLongNamedAdr + 8                   ; OK
                              ;+stax16 .testLongNamedAdr + offset_8            ; OK

                              ;--------------------------------
                              ; mit Arithmetik, mit Klammern ($c04c)
                              ; -------------------------------

                              ; feste Adresse, Zeropage
                              +ldax16 (zptrMemDest1 + offset_2)               ; OK
                              ;+stax16 (zptrMemDest1 + 2)                      ; fast OK: 16 bit statt Zeropage

                              ; feste Adresse (1), 16 bit
                              +ldax16 (testAdressFix + offset_2)              ; OK
                              ;+stax16 (testAdressFix + 2)                     ; OK

                              ; feste Adresse (2), 16 bit
                              +ldax16 (testAdressPreCalc + offset_2)          ; OK
                              ;+stax16 (testAdressPreCalc + 2)                 ; OK

                              ; dynamische Adresse, 16 bit
                              +ldax16 (.testLongNamedAdr + offset_8)          ; OK
                              +stax16 (.testLongNamedAdr + 8)                 ; OK


                              ; ===============================
                              ; Makros mit zwei Parametern
                              ; ===============================

                              ; -------------------------------
                              ; ohne Arithmetik ($c074)
                              ; -------------------------------
                              +setPtrA testAdressFix, zptrMemDest1                                ; OK
                              +setPtrA .testLongNamedAdr, zptrMemDest1                            ; OK

                              ; -------------------------------
                              ; mit Arithmetik, ohne Klammern ($c084)
                              ; -------------------------------
                              +setPtrA testAdressFix + 40, zptrMemDest1                           ; OK
                              +setPtrA testAdressFix + 40, zptrMemDest1 + 2                       ; OK
                              +setPtrA testAdressFix + 40, zptrMemDest1 + offset_2                ; OK
                              nop
                              nop
                              ; funktioniert nicht: !align 3, $ea
                              ;!align 3, $ea

                              ; $c09e
                              +setPtrA testAdressFix + offset40, zptrMemDest1                     ; OK
                              +setPtrA testAdressFix + offset40, zptrMemDest1 + 2                 ; OK
                              +setPtrA testAdressFix + offset40, zptrMemDest1 + offset_2          ; OK
                              nop
                              nop

                              ; $c0b8
                              +setPtrA .testLongNamedAdr + 8, zptrMemDest1                        ; OK
                              +setPtrA .testLongNamedAdr + 8, zptrMemDest1 + 2                    ; OK
                              +setPtrA .testLongNamedAdr + 8, zptrMemDest1 + offset_2             ; OK
                              nop
                              nop

                              ; $c0d2
                              +setPtrA .testLongNamedAdr + offset_8, zptrMemDest1                 ; OK
                              +setPtrA .testLongNamedAdr + offset_8, zptrMemDest1 + 2             ; OK
                              +setPtrA .testLongNamedAdr + offset_8, zptrMemDest1 + offset_2      ; OK
                              nop
                              nop


                              ; -------------------------------
                              ; mit Arithmetik, mit Klammern
                              ; -------------------------------

                              ; $c0ec
                              +setPtrA (testAdressFix + 40), (zptrMemDest1)                       ;fast OK: 16 bit statt Zeropage
                              +setPtrA (testAdressFix + 40), (zptrMemDest1 + 2)                   ;fast OK: 16 bit statt Zeropage
                              +setPtrA (testAdressFix + 40), (zptrMemDest1 + offset_2)            ;fast OK: 16 bit statt Zeropage
                              nop
                              nop

                              ; $c10c
                              +setPtrA (testAdressFix + offset40), (zptrMemDest1)                 ;fast OK: 16 bit statt Zeropage
                              +setPtrA (testAdressFix + offset40), (zptrMemDest1 + 2)             ;fast OK: 16 bit statt Zeropage
                              +setPtrA (testAdressFix + offset40), (zptrMemDest1 + offset_2)      ;fast OK: 16 bit statt Zeropage
                              nop
                              nop

                              ; $c12c
                              +setPtrA (.testLongNamedAdr + 8), (zptrMemDest1)                    ;fast OK: 16 bit statt Zeropage
                              +setPtrA (.testLongNamedAdr + 8), (zptrMemDest1 + 2)                ;fast OK: 16 bit statt Zeropage
                              +setPtrA (.testLongNamedAdr + 8), (zptrMemDest1 + offset_2)         ;fast OK: 16 bit statt Zeropage
                              nop
                              nop

                              ; $c14c
                              +setPtrA (.testLongNamedAdr + offset_8), (zptrMemDest1)             ;fast OK: 16 bit statt Zeropage
                              +setPtrA (.testLongNamedAdr + offset_8), (zptrMemDest1 + 2)         ;fast OK: 16 bit statt Zeropage
                              +setPtrA (.testLongNamedAdr + offset_8), (zptrMemDest1 + offset_2)  ;fast OK: 16 bit statt Zeropage

                              rts



                              !align 255,0

                              .testLongNamedAdr:
                              !word 0