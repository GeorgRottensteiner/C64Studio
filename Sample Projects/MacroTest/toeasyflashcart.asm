BANK_SWITCH = $de00
!to "toeasyflashcrt.crt",easyflashcrt
;ROM part
;---------------------------------
!bank 0,$2000
*=$8000
top
  ; cold start vector => our program
  !word launcher
  ; warm start => $febc => exit interrupt
  !word launcher

  ; magic string
  !byte $c3, $c2, $cd, $38, $30

launcher


  lda #7
  sta $d020
    sta $d021
    ldx #$00
-
          lda movecode1,x
          sta $0100,x
          inx
          bne -


          sei
          lda #$37
          sta $01
cli
          ldx #0
          txa
.sidloop
          sta $d400,x
          inx
          cpx #$19
          bne .sidloop



          ldy #1
          jmp $0100
;====================================================================================================


movecode1

        ;Y = bank to copy

!pseudopc $0100

cppy

copy_loop



              ldx #$86 ;7      on 8k
         stx $de02


          lda INTRO_BANK,y
          sty COPY_BANK
          and #$7f
          sta BANK_SWITCH

          lda #$00
          sta src+1
          lda #$80
          sta src+2

          lda INTRO_TARGET_ADDRESS_LO,y
          sta dst+1
          lda INTRO_TARGET_ADDRESS_HI,y
          sta dst+2





          ldx #32  ; how many blocks --- we always doing $2000     32 * 255 = 8192
          ldy #0

          cmp #$e8
          bne a1
            ldx #24 ;4  ;21


a1


src       lda $8000,y ; From ROM
dst       sta $0800,y ; To Kernal address

          iny
          bne a1

          inc src+2
          inc dst+2

          dex
          bne a1

.LoDone


          ldy COPY_BANK
          lda INTRO_BANK,y
          and #$80
          bne +

          inc COPY_BANK
          ldy COPY_BANK
          jmp copy_loop

+
        lda #0
        jmp $0827


COPY_BANK
          !byte 0
INTRO_BANK            ; bank numbers
          !byte $00 ; not used
          !byte $01 ; game
          !byte $02 ;
          !byte $03 ;
          !byte $04
          !byte $05 ;
          !byte $06 ;
          !byte $07
          !byte $88


INTRO_TARGET_ADDRESS_LO
          !byte 0   ; not used
          !byte 0   ; game
          !byte 0   ;
          !byte 0  ;
          !byte 0  ;
          !byte 0 ;
          !byte 0  ;
          !byte 0  ;
          !byte 0  ;


INTRO_TARGET_ADDRESS_HI
          !byte 0   ; not used
          !byte $08  ; g
          !byte $28  ; g
          !byte $48  ; g
          !byte $68  ; g
          !byte $88  ; g
          !byte $a8  ; g
          !byte $c8  ; g
          !byte $e8  ; game


;============================================ end of loader

;=================================================

mover_end

                                          ; bank 1 + 2 + 3 + 4 + 5 + 6 + 7
!bank 1,$2000
!pseudopc $8000
!binary "k.prg",8192,2

!bank 2,$2000
!pseudopc $8000
!binary "k.prg",8192,2 + (1 * 8192)

!bank 3,$2000
!pseudopc $8000
!binary "k.prg",8192,2 + (2 * 8192)

!bank 4,$2000
!pseudopc $8000
!binary "k.prg",8192,2 + (3 * 8192)

!bank 5,$2000
!pseudopc $8000
!binary "k.prg",8192,2 + (4 * 8192)

!bank 6,$2000
!pseudopc $8000
!binary "k.prg",8192,2 + (5 * 8192)

!bank 7,$2000
!pseudopc $8000
!binary "k.prg",8192,2 + (6 * 8192)

!bank 8,$2000
!pseudopc $8000
!binary "k.prg",8192,2 + (6 * 8192)
