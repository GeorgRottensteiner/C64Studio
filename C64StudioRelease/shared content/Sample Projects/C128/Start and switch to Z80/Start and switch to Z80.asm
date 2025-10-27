*=$2000
; wechsle zum Z80-Modus und zurück
; 64er, "128 extra" Nr.1, Seite 88
; ==================================================================
; Verwendete Speicherstellen:
; (aus "Mapping the Commodore 128", Compute!
;  und "Commodore 128 intern", Data Becker)
; ------------------------------------------------------------------
; $ff00: Configuration register
;        "[...] the value here determines what other memory elements will
;        be visible to the processor."
;        Bits: 7 6 5 4 3 2 1 0
;              - - x x x x x -
;          ____/      /    /  \____
;         /          /   $4000-    \
;    RAM-Block 0    /    $7fff:     \
;    auswählen     /     RAM       $d000-
;                 /                $dfff:
;         $8000-$cfff und           I/O
;         $e000-$ffff:
;         RAM einblenden
;
; Zusammenfassung:
; $0000 - $cfff: o RAM
; $d000 - $dfff: # I/O
; $e000 - $ffff: o RAM
; ------------------------------------------------------------------
; $ffee (RAM):
; "Hier hatte der Z80 nach dem Bootversuch von CP/m aufgehört
; zu arbeiten."
; ------------------------------------------------------------------
; $d505: Mode Configuration Register
;        Bits: 7 6 5 4 3 2 1 0
;              x - x x - - - -
;             / /   /      \  \________
;        ____/ / prevent    \__        \
;       /     /  C64 mode(?)   \        \
;      /  make                unused    use
;     /   C128 ROM                      Z80 cpu
; allow   visible
; reading
; of 40/80
; switch
; ==================================================================
;
; *** START
;
;!cpu 6510
          sei       ; Interrupt sperren
          lda $ff00 ; Konfigurationsregister
          pha       ; retten
          lda #$c3  ; Z80-Befehl JP (JUMP)
          sta $ffee
          lda #<z80 ; Lo-Byte
          sta $ffef
          lda #>z80 ; Hi-Byte
          sta $fff0
          lda #$3e  ; %0011 1110
          sta $ff00 ; Konfig.register setzen
          lda $d505 ; MCR
          pha       ; retten
          lda #$b0  ; %1011 0000
          sta $d505 ; MMU-Register: Z80 ein
          nop       ; warten (wichtig!)
          pla       ; MCR korrigieren
          sta $d505
          pla       ; Konfig.register
          sta $ff00 ; korrigieren
          cli       ; Interrupt freigeben
          rts       ; Return
z80
;.mod1   ; Es folgt Z80-Code
!cpu z80

          ld a,$3f      ; Bit0 gesetzt => $d000-$dfff=RAM(?)
          ld ($ff00),a  ; Setzen des Konfig.registers
          ld a,$51      ; Z80-
          ld ($3000),a  ; Programm
          jp $ffe0      ; Betriebssystem-Routine:
                        ; 8502 aktivieren