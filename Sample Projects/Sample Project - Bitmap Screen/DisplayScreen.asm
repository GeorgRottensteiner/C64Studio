* = $0801

!basic
          lda #0
          sta $D020 
          sta $D021 
          lda #$18 
          sta $D018 ;Charset location where Bitmap is 
          sta $D016 ;Enable char/bitmap multicolour 
          lda #$3B 
          sta $D011 
          
          ldx #0
.CopyLoop      
          lda $4000 + 0 * 250,X 
          sta $0400 + 0 * 250,X 
          lda $4000 + 1 * 250,X 
          sta $0400 + 1 * 250,X 
          lda $4000 + 2 * 250,X 
          sta $0400 + 2 * 250,X 
          lda $4000 + 3 * 250,X 
          sta $0400 + 3 * 250,X 
          
          lda $4400 + 0 * 250,X 
          sta $D800 + 0 * 250,X 
          lda $4400 + 1 * 250,X 
          sta $D800 + 1 * 250,X 
          lda $4400 + 2 * 250,X 
          sta $D800 + 2 * 250,X 
          lda $4400 + 1 * 250,X 
          sta $D800 + 1 * 250,X 
          
          inx
          cpx #250
          bne .CopyLoop
          
EndlessLoop
          jmp EndlessLoop


;bitmap data
* = $2000
!bin "Bitmap-Binary.bin",$1f40

;charscreen colors
* = $4000
!bin "Bitmap-Binary.bin",1000,$1f40

;color ram colors
* = $4400
!bin "Bitmap-Binary.bin",1000,$1f40 + 1000

