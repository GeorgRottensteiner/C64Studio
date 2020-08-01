;-------------------------------------------------------------------------
; NMI Sample player (w) Groepaz/Hitmen
;-------------------------------------------------------------------------

* = $0801

; call NMIDIGI_Init once for init, then call NMIDIGI_Play with a pointer (x=hi,y=lo) to a table with parameters 
;(data start lo/hi, data end lo/hi, samplerate lo/hi). NMIDIGI_Off temporarily disables the player, NMIDIGI_On enables it again.
        !basic
        
        jsr NMIDIGI_Init
        
        ldy #<DATA
        ldx #>DATA
        jsr NMIDIGI_Play
        
-
        jmp -

__SID__ = $d400

NMIDIGIPTR = $0d
NMIPOINT = $fb

DEBUG = 0
withsidplayer=0
!if(withsidplayer=1) {
sidplayervol=D418HLP
}

;0: high nibble first 1: low nibble first
firstnibble=1
;1: no nibbles
nonibbles=1

NMIDIGI_Init
                      jsr NMIDIGI_Off
                      
                      ;digi boost
                      lda #$ff
                      sta $d406
                      sta $d406+7
                      sta $d496+14
                      lda #$49
                      sta $d404
                      sta $d404+7
                      sta $d404+14
                      
                      ;set nmi frequency
                      lda #$3c
                      sta $dd04
                      lda #$00
                      sta $dd05 

                      lda #$00
                      ldx #$00
@l:
                      sta __SID__,x
                      inx
                      bne @l

                      lda #$00
                      sta __SID__+$05   ; voice 1 ad
                      lda #$f0
                      sta __SID__+$06   ;         sr
                      lda #$01
                      sta __SID__+$04   ;         ctrl
                      lda #$00
                      sta __SID__+$0c   ; voice 2 ad
                      lda #$f0
                      sta __SID__+$0d   ;         sr
                      lda #$01
                      sta __SID__+$0b   ;         ctrl
                      lda #$00
                      sta __SID__+$13   ; voice 3 ad
                      lda #$f0
                      sta __SID__+$14   ;         sr
                      lda #$01
                      sta __SID__+$12   ;         ctrl
                      lda #$00
                      sta __SID__+$15   ; filter lo
                      lda #$10
                      sta __SID__+$16   ; filter hi
                      lda #%11110111
                      sta __SID__+$17   ; filter voices+reso

                      rts

NMIDIGI_On:
                      LDA #>NMIDIGION
                      STA $FFFB
                      LDA #<NMIDIGION
                      STA $FFFA

                      LDA #%10000001    ; enable CIA-2 timer A nmi
                      STA $DD0D
                      lda $DD0D
                      LDA #%00000001    ; timer A start
                      STA $DD0E
                      rts
NMIDIGI_Off
                      LDA #%00000000
                      STA $DD0E         ; timer A stop
                      LDA #%01001111    ; disable all CIA-2 nmi's
                      STA $DD0D
                      lda $DD0D

                      LDA #>NMIDIGIOFF
                      STA $FFFB
                      LDA #<NMIDIGIOFF
                      STA $FFFA

                      lda #$00
                      sta NMIPOINT
                      sta NMIPOINT+1
                      sta DIGISTOPLO+1
                      sta DIGISTOPHI+1

                      rts


NMIDIGI_Play:

                      jsr NMIDIGI_Off

                      stx NMIDIGIPTR+1
                      sty NMIDIGIPTR

                      ldy #$00
                      lda (NMIDIGIPTR),y
                      sta NMIPOINT
                      iny
                      lda (NMIDIGIPTR),y
                      sta NMIPOINT+1
                      iny
                      lda (NMIDIGIPTR),y
                      sta DIGISTOPLO+1
                      iny
                      lda (NMIDIGIPTR),y
                      sta DIGISTOPHI+1
                      iny
                      lda (NMIDIGIPTR),y
                      sta $DD04
                      iny
                      lda (NMIDIGIPTR),y
                      sta $DD05


                        !if(nonibbles=0) {
                      lda #$00
                      sta nib+1
                        }

                      jsr NMIDIGI_On

                      rts

;-------------------------------------------

NMIDIGION:
                STA NMIABUFF
                STY NMIYBUFF

                !if (withsidplayer=1) {
                  lda sidplayervol
                  and #$f0
                } else {
                  lda #$10
                }

D418NMI:        ora #$00
                sta __SID__+$18          ; volume reg

                !if (withsidplayer=1) {
                  sta sidplayervol
                }

                !if (DEBUG=1) {
                 sta $d020
                }

                LDA NMIPOINT+1
DIGISTOPHI:      CMP #$12                ;ENDHIGH
                BNE SK1

                LDA NMIPOINT
DIGISTOPLO:      CMP #$00                ;ENDLOW
                BNE SK1

                !if (withsidplayer=1) {
                  lda #$08
                } else {
                  lda #$00
                }

                STA D418NMI+1

                jsr NMIDIGI_Off

                LDA NMIABUFF

                RTI

SK1:

                LDY #$00

                !if(nonibbles=0) {
nib:            lda #$00
                and #$01
                bne s1
                }

                LDA (NMIPOINT),Y

                !if(nonibbles=0) {
                !if(firstnibble=0) { ; high nibble first
                  lsr a
                  lsr a
                  lsr a
                  lsr a
                } else {              ; low nibble first
                  AND #$0F
                }

                jmp s2
s1:
                LDA (NMIPOINT),Y
                !if(firstnibble=1) { ; high nibble second
                  lsr a
                  lsr a
                  lsr a
                  lsr a
                } else {              ; low nibble second
                  AND #$0F
                }
                }

                INC NMIPOINT
                BNE @SK
                INC NMIPOINT+1
@SK:
s2:
                STA D418NMI+1

                !if(nonibbles=0) {
                inc nib+1
                }

                LDA $DD0D

NMIABUFF=*+1
                LDA #$00
NMIYBUFF=*+1
                LDY #$00

NMIDIGIOFF:
                RTI
                
DATA
!bin "pickup.snd"                